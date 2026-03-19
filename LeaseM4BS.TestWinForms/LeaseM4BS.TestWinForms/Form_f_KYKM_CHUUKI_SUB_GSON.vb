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

        ' gson_dt を YYYY/MM 形式で表示
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
    '  ヘルパー
    ' =========================================================
    Private Function NzStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Return v.ToString()
    End Function

    Private Function NzAmtStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return "0"
        Try : Return Convert.ToDouble(v).ToString("N0")
        Catch : Return v.ToString()
        End Try
    End Function

    Private Function NzDbl(v As Object) As Double
        If IsDBNull(v) OrElse v Is Nothing Then Return 0.0
        Try : Return Convert.ToDouble(v)
        Catch : Return 0.0
        End Try
    End Function

    Private Sub Form_f_KYKM_CHUUKI_SUB_GSON_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
