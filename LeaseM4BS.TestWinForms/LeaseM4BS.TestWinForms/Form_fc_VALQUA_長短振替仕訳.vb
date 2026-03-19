Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' VALQUA顧客固有 長短振替仕訳出力フォーム
''' Access版 fc_VALQUA_長短振替仕訳 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に長短振替仕訳を出力して CSV に書き込む。
''' kjkbn_id=2（資産）、長短振替（leakbn_id 変更）データを対象。
''' chk_長短振替F: 長短振替仕訳を出力
''' chk_短長戻しF: 短長戻し仕訳を出力
''' </summary>
Partial Public Class Form_fc_VALQUA_長短振替仕訳
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "VALQUA"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "長短振替仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "VALQUA_CHOSHINFURI_KEIJYO_DT"
    Private Const KEY_SLIP_DT_MODOSHI As String = "VALQUA_CHOSHINFURI_KEIJYO_DT_MODOSHI"
    Private Const KEY_FOLDER As String = "VALQUA_CHOSHINFURI_FOLDER"
    Private Const KEY_FNAME_FURI As String = "VALQUA_CHOSHINFURI_FNAME_FURI"
    Private Const KEY_FNAME_MODOSHI As String = "VALQUA_CHOSHINFURI_FNAME_MODOSHI"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("VALQUA")
    End Sub

    Private Sub Form_fc_VALQUA_長短振替仕訳_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ValidateOutputFolder(txt_FOLDER.Text) Then Return

        If Not chk_長短振替F.Checked AndAlso Not chk_短長戻しF.Checked Then
            MessageBox.Show("出力する仕訳種別を選択してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            kikanFrom = New Date(Now.Year, Now.Month, 1)
        End If

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'VALQUA' AND swk_kbn = '長短振替仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'VALQUA' AND swk_kbn = '長短振替仕訳' ORDER BY den_no, gyo_no")

        Dim results As New List(Of String)
        If chk_長短振替F.Checked Then
            Dim path1 = WriteCsvFile(dt, txt_FOLDER.Text, txt_FNAME_長短振替.Text)
            If path1 IsNot Nothing Then results.Add(path1)
        End If
        If chk_短長戻しF.Checked Then
            Dim path2 = WriteCsvFile(dt, txt_FOLDER.Text, txt_FNAME_短長戻し.Text)
            If path2 IsNot Nothing Then results.Add(path2)
        End If

        If results.Count > 0 Then
            SaveSettings()
            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {String.Join(Environment.NewLine, results)}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "CSV出力先フォルダを選択してください"
            If Not String.IsNullOrWhiteSpace(txt_FOLDER.Text) Then
                dlg.SelectedPath = txt_FOLDER.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_FOLDER.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（VALQUA長短振替仕訳版）
    ''' kjkbn_id=2（資産）、長短振替対象データ（rec_kbn IN(1,3)）を対象。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_KEIJYO_DT.Text),
                        Format(Now, "yyyy/MM/dd"), txt_KEIJYO_DT.Text)

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'VALQUA', '長短振替仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '' AS dr_bmn_cd,
    k.lsryo AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.lsryo AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2
  AND k.rec_kbn IN (1, 3)
  AND k.keijo_f = TRUE
ORDER BY k.kykm_id"
    End Function

    ''' <summary>WriteOutputFile は使用しない（cmd_実行で直接ファイル出力）。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Return Nothing
    End Function

    Private Function WriteCsvFile(dt As DataTable, folder As String, fileName As String) As String
        If String.IsNullOrWhiteSpace(fileName) Then
            fileName = $"VALQUA_長短振替仕訳_{Format(Now, "yyyyMMdd")}.csv"
        End If
        Dim filePath = If(Path.IsPathRooted(fileName), fileName, Path.Combine(folder, fileName))
        Using sw As New StreamWriter(filePath, False, Encoding.UTF8)
            sw.WriteLine("伝票番号,伝票日付,借方科目CD,借方補助科目CD,借方金額,借方税額,貸方科目CD,貸方補助科目CD,貸方金額,摘要")
            For Each row As DataRow In dt.Rows
                sw.WriteLine(String.Join(",",
                    CsvEsc(row("den_no")), CsvEsc(row("den_date")),
                    CsvEsc(row("dr_kmk_cd")), CsvEsc(row("dr_hkm_cd")),
                    row("dr_kin"), row("dr_zei_kin"),
                    CsvEsc(row("cr_kmk_cd")), CsvEsc(row("cr_hkm_cd")),
                    row("cr_kin"), CsvEsc(row("tekiyo"))))
            Next
        End Using
        Return filePath
    End Function

    Private Function CsvEsc(v As Object) As String
        If IsDBNull(v) Then Return ""
        Dim s = v.ToString()
        If s.Contains(",") OrElse s.Contains("""") OrElse s.Contains(vbNewLine) Then
            Return """" & s.Replace("""", """""") & """"
        End If
        Return s
    End Function

    Private Sub LoadSettings()
        txt_KEIJYO_DT.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_KEIJYO_DT_戻し.Text = _settei.GetText(KEY_SLIP_DT_MODOSHI, Format(Now, "yyyy/MM/dd"))
        txt_FOLDER.Text = _settei.GetText(KEY_FOLDER)
        txt_FNAME_長短振替.Text = _settei.GetText(KEY_FNAME_FURI, $"VALQUA_長短振替仕訳_{Format(Now, "yyyyMMdd")}.csv")
        txt_FNAME_短長戻し.Text = _settei.GetText(KEY_FNAME_MODOSHI, $"VALQUA_短長戻し仕訳_{Format(Now, "yyyyMMdd")}.csv")
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_KEIJYO_DT.Text)
        _settei.SetText(KEY_SLIP_DT_MODOSHI, txt_KEIJYO_DT_戻し.Text)
        _settei.SetText(KEY_FOLDER, txt_FOLDER.Text)
        _settei.SetText(KEY_FNAME_FURI, txt_FNAME_長短振替.Text)
        _settei.SetText(KEY_FNAME_MODOSHI, txt_FNAME_短長戻し.Text)
    End Sub

    Private Sub Form_fc_VALQUA_長短振替仕訳_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class