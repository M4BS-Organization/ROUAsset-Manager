Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 物件サブ画面（分割管理用） (f_KYKM_SUB_BKN)
''' d_haif (配賦行) の一覧を表示し、配賦率合計・金額合計を算出する。
''' </summary>
Partial Public Class Form_f_KYKM_SUB_BKN
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
    Private Sub Form_f_KYKM_SUB_BKN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String =
                "SELECT h.*, " &
                "       b.bcat1_nm, k.hkmk_nm, r.rsrvh1_nm " &
                "FROM d_haif h " &
                "LEFT JOIN m_bcat b ON b.bcat_id = h.h_bcat_id " &
                "LEFT JOIN m_hkmk k ON k.hkmk_id = h.hkmk_id " &
                "LEFT JOIN m_rsrvh1 r ON r.rsrvh1_id = h.rsrvh1_id " &
                "WHERE h.kykm_id = @id " &
                "ORDER BY h.line_id"
            Dim prm As New List(Of NpgsqlParameter)
            prm.Add(New NpgsqlParameter("@id", _kykmId))
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
            ClearFields()
            txt_COUNT.Text = "0"
            Return
        End If

        Dim r = _rows(_currentIndex)
        Dim lineIdVal As Integer = 0
        Integer.TryParse(NzStr(r("line_id")), lineIdVal)
        SelectedLineId = lineIdVal
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_LINE_ID.Text = NzStr(r("line_id"))
        txt_HAIFRITU.Text = NzStr(r("haifritu"))
        txt_BCAT1_NM.Text = NzStr(r("bcat1_nm"))
        txt_HKMK_NM.Text = NzStr(r("hkmk_nm"))
        txt_RSRVH1_NM.Text = NzStr(r("rsrvh1_nm"))
        txt_H_KLSRYO.Text = NzAmtStr(r("h_klsryo"))
        txt_H_MLSRYO.Text = NzAmtStr(r("h_mlsryo"))
        txt_H_KZEI.Text = NzAmtStr(r("h_kzei"))
        txt_H_MZEI.Text = NzAmtStr(r("h_mzei"))
        txt_H_KLSRYO_ZKOMI.Text = NzAmtStr(r("h_klsryo_zkomi"))
        txt_H_MLSRYO_ZKOMI.Text = NzAmtStr(r("h_mlsryo_zkomi"))
        txt_H_ZOKUSEI1.Text = NzStr(r("h_zokusei1"))
        txt_COUNT.Text = $"{_currentIndex + 1}/{_rows.Count}"
    End Sub

    Private Sub ClearFields()
        Dim fields() As TextBox = {
            txt_W_KYKM_ID, txt_LINE_ID, txt_HAIFRITU,
            txt_BCAT1_NM, txt_HKMK_NM, txt_RSRVH1_NM,
            txt_H_KLSRYO, txt_H_MLSRYO, txt_H_KZEI, txt_H_MZEI,
            txt_H_KLSRYO_ZKOMI, txt_H_MLSRYO_ZKOMI, txt_H_ZOKUSEI1
        }
        For Each t In fields
            t.Text = ""
        Next
    End Sub

    ' =========================================================
    '  合計表示
    ' =========================================================
    Private Sub ShowTotals()
        Dim sumHaifritu As Double = 0
        Dim sumKlsryo As Double = 0, sumMlsryo As Double = 0
        Dim sumKzei As Double = 0, sumMzei As Double = 0

        For Each r In _rows
            sumHaifritu += NzDbl(r("haifritu"))
            sumKlsryo += NzDbl(r("h_klsryo"))
            sumMlsryo += NzDbl(r("h_mlsryo"))
            sumKzei += NzDbl(r("h_kzei"))
            sumMzei += NzDbl(r("h_mzei"))
        Next

        txt_HAIFRITU_SUM.Text = sumHaifritu.ToString("N2")
        txt_H_KLSRYO_SUM.Text = sumKlsryo.ToString("N0")
        txt_H_MLSRYO_SUM.Text = sumMlsryo.ToString("N0")
        txt_H_KZEI_SUM.Text = sumKzei.ToString("N0")
        txt_H_MZEI_SUM.Text = sumMzei.ToString("N0")

        If _rows.Count > 0 AndAlso Math.Abs(sumHaifritu - 100.0) > 0.001 Then
            txt_HAIFRITU_SUM.BackColor = System.Drawing.Color.LightYellow
        Else
            txt_HAIFRITU_SUM.BackColor = System.Drawing.SystemColors.Window
        End If
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_配賦率_Click(sender As Object, e As EventArgs) Handles cmd_配賦率.Click
        MessageBox.Show("配賦率計算は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_期間額_Click(sender As Object, e As EventArgs) Handles cmd_期間額.Click
        MessageBox.Show("期間額計算は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Form_f_KYKM_SUB_BKN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
