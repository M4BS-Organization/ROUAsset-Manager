Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 減損サブ画面 (f_KYKM_CHUUKI_SUB_GSON)
''' d_gson の一覧を表示し、選択した line_id を返す（削除対象選択用）。
''' </summary>
Partial Public Class Form_f_KYKM_CHUUKI_SUB_GSON
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykmId As Integer = 0
    Private _rows As New List(Of System.Data.DataRow)
    Private _currentIndex As Integer = 0

    ''' <summary>ダイアログ終了時に選択中の line_id を返す（-1 = 未選択）</summary>
    Public Property SelectedLineId As Integer = -1

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部からパラメータをセット（Double互換）</summary>
    Public Property KykmId As Double
        Get
            Return _kykmId
        End Get
        Set(value As Double)
            _kykmId = CInt(value)
        End Set
    End Property

    ''' <summary>外部からパラメータをセット</summary>
    Public Sub SetParams(kykmId As Integer)
        _kykmId = kykmId
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KYKM_CHUUKI_SUB_GSON_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String =
                "SELECT * FROM d_gson WHERE kykm_id = @id ORDER BY line_id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)}
            Dim dt = _crud.GetDataTable(sql, prm)

            _rows.Clear()
            If dt IsNot Nothing Then
                For Each row As System.Data.DataRow In dt.Rows
                    _rows.Add(row)
                Next
            End If

            _currentIndex = 0
            ShowCurrentRecord()
            ShowTotals()
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  現在レコード表示
    ' =========================================================
    Private Sub ShowCurrentRecord()
        If _rows.Count = 0 Then
            txt_W_KYKM_ID.Text = _kykmId.ToString()
            txt_LINE_ID.Text = ""
            txt_GSON_DT.Text = ""
            txt_GSON_RYO.Text = ""
            txt_GSON_RKEI.Text = ""
            txt_COUNT.Text = "0"
            SelectedLineId = -1
            Return
        End If

        Dim r = _rows(_currentIndex)
        Dim lineIdVal As Integer = 0
        Integer.TryParse(NzStr(r("line_id")), lineIdVal)
        SelectedLineId = lineIdVal

        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_LINE_ID.Text = NzStr(r("line_id"))

        Dim dtVal As Object = r("gson_dt")
        If Not IsDBNull(dtVal) AndAlso dtVal IsNot Nothing Then
            Try
                txt_GSON_DT.Text = Convert.ToDateTime(dtVal).ToString("yyyy/MM")
            Catch
                txt_GSON_DT.Text = NzStr(dtVal)
            End Try
        Else
            txt_GSON_DT.Text = ""
        End If

        txt_GSON_RYO.Text = NzAmtStr(r("gson_ryo"))
        txt_GSON_RKEI.Text = NzAmtStr(r("gson_rkei"))
        txt_COUNT.Text = $"{_currentIndex + 1}/{_rows.Count}"
    End Sub

    ' =========================================================
    '  合計表示
    ' =========================================================
    Private Sub ShowTotals()
        Dim sumRyo As Double = 0
        For Each r In _rows
            sumRyo += NzDbl(r("gson_ryo"))
        Next
        txt_GSON_RYO_SUM.Text = sumRyo.ToString("N0")
    End Sub

    ' =========================================================
    '  AfterUpdate イベント
    ' =========================================================

    ''' <summary>減損年月: 月初日に正規化 (yyyy/MM → yyyy/MM/01)</summary>
    Private Sub txt_GSON_DT_Leave(sender As Object, e As EventArgs) Handles txt_GSON_DT.Leave
        Dim s = txt_GSON_DT.Text.Trim()
        If String.IsNullOrEmpty(s) Then Return
        Dim dt As DateTime
        If DateTime.TryParse(s, dt) Then
            txt_GSON_DT.Text = New DateTime(dt.Year, dt.Month, 1).ToString("yyyy/MM")
        End If
    End Sub

    ''' <summary>減損金額変更 → 累計額を再計算してDB更新</summary>
    Private Sub txt_GSON_RYO_Leave(sender As Object, e As EventArgs) Handles txt_GSON_RYO.Leave
        If _kykmId = 0 OrElse _rows.Count = 0 OrElse _currentIndex < 0 Then Return
        Try
            ' 現在行のgson_ryoを更新
            Dim lineId = Convert.ToInt32(_rows(_currentIndex)("line_id"))
            Dim newRyo = ParseDblFromText(txt_GSON_RYO.Text)

            _crud.ExecuteNonQuery(
                "UPDATE d_gson SET gson_ryo = @ryo, g_update_dt = NOW() WHERE kykm_id = @id AND line_id = @lid",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@ryo", newRyo),
                    New NpgsqlParameter("@id", _kykmId),
                    New NpgsqlParameter("@lid", lineId)
                })

            ' 全行の累計額(gson_rkei)を再計算
            RecalcGsonRkei()
            LoadData()
        Catch ex As Exception
            MessageBox.Show("減損金額更新エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>全減損行の累計額(gson_rkei)を再計算</summary>
    Private Sub RecalcGsonRkei()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT line_id, COALESCE(gson_ryo, 0) AS gson_ryo FROM d_gson WHERE kykm_id = @id ORDER BY line_id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            If dt Is Nothing Then Return

            Dim cumulative As Double = 0
            For Each r As System.Data.DataRow In dt.Rows
                cumulative += Convert.ToDouble(r("gson_ryo"))
                _crud.ExecuteNonQuery(
                    "UPDATE d_gson SET gson_rkei = @rkei WHERE kykm_id = @id AND line_id = @lid",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@rkei", cumulative),
                        New NpgsqlParameter("@id", _kykmId),
                        New NpgsqlParameter("@lid", Convert.ToInt32(r("line_id")))
                    })
            Next
        Catch ex As Exception
            MessageBox.Show("累計額再計算エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_f_KYKM_CHUUKI_SUB_GSON_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
