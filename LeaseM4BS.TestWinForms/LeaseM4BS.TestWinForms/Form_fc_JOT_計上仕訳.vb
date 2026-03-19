Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' JOT顧客固有 計上仕訳出力フォーム
''' Access版 fc_JOT_計上仕訳 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に計上仕訳を出力して CSV ファイルに書き込む。
''' kjkbn_id=2（資産）、rec_kbn IN(1,2,3) を対象。
'''   rec_kbn=1: 開始計上（リース資産/リース債務）、金額=lsryo
'''   rec_kbn=2: 税一括（仮払消費税/リース未払金）、金額=zei
'''   rec_kbn=3: 減価償却（減価償却費/累計額）、金額=lsryo
''' 会社区分（親会社/関係会社）により部署CDの取得元を切替。
''' txt_TEXT_02 に消費税率設定を "rate1,kbn1,..." 形式で保持。
''' </summary>
Partial Public Class Form_fc_JOT_計上仕訳
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "JOT"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "計上仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "JOT_KEIJOSWK_SLIP_DT"
    Private Const KEY_KAMOKU_01 As String = "JOT_KEIJOSWK_KAMOKU_CD_01"
    Private Const KEY_KAMOKU_02 As String = "JOT_KEIJOSWK_KAMOKU_CD_02"
    Private Const KEY_BUMON As String = "JOT_KEIJOSWK_BUMON_NM"
    Private Const KEY_SEG_CD1 As String = "JOT_KEIJOSWK_SEG_CD1"
    Private Const KEY_SEG_NM1 As String = "JOT_KEIJOSWK_SEG_NM1"
    Private Const KEY_SEG_CD2 As String = "JOT_KEIJOSWK_SEG_CD2"
    Private Const KEY_ZRITU As String = "JOT_KEIJOSWK_ZRITU_TEXT"
    Private Const KEY_FOLDER As String = "JOT_KEIJOSWK_OUTPUT_FOLDER"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("JOT")
    End Sub

    Private Sub Form_fc_JOT_計上仕訳_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            Me.Close()
            Return
        End If
        txt_YMD_01.Text = Format(kikanFrom, "yyyy/MM")
        txt_YMD_01.ReadOnly = True
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ValidateOutputFolder(txt_TEXT_01.Text) Then Return
        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        SaveZrituToText()
        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'JOT' AND swk_kbn = '計上仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'JOT' AND swk_kbn = '計上仕訳' ORDER BY den_no, gyo_no")
        Dim result = WriteOutputFile(dt, txt_TEXT_01.Text)

        If result IsNot Nothing Then
            SaveSettings()
            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {result}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "出力先フォルダを選択してください"
            If Not String.IsNullOrWhiteSpace(txt_TEXT_01.Text) Then
                dlg.SelectedPath = txt_TEXT_01.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_TEXT_01.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（JOT計上仕訳版）
    ''' Access版 m計上データ作成 相当。
    ''' kjkbn_id=2（資産）、rec_kbn IN(1,2,3) を対象。
    '''   rec_kbn=1: 開始計上、金額=lsryo
    '''   rec_kbn=2: 税一括（zei > 0 のみ）、金額=zei
    '''   rec_kbn=3: 減価償却、金額=lsryo
    ''' 部署CD: 親会社=txt_BUMON_NM_01、関係会社=空（B_BCAT1_CD は PostgreSQL スキーマ外）
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_YMD_02.Text),
                        Format(Now, "yyyy/MM/dd"), txt_YMD_02.Text)
        Dim bumonCd = If(オプション487.Checked, txt_BUMON_NM_01.Text, "")
        Dim segCd1 = txt_KAMOKU_CD_03.Text
        Dim segCd2 = txt_KAMOKU_CD_04.Text

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'JOT', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id, k.rec_kbn) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    CASE WHEN k.rec_kbn = 2 THEN k.zei ELSE k.lsryo END AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    CASE WHEN k.rec_kbn = 2 THEN k.zei ELSE k.lsryo END AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2
  AND k.rec_kbn IN (1, 3)
  AND k.keijo_f = TRUE
UNION ALL
SELECT
    'JOT', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    k.zei AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2
  AND k.rec_kbn = 2
  AND k.keijo_f = TRUE
  AND k.zei > 0
ORDER BY kykm_id, rec_kbn"
    End Function

    ''' <summary>CSV形式でファイル出力する。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, $"JOT_計上仕訳_{Format(Now, "yyyyMMdd")}.csv")
        Using sw As New StreamWriter(fileName, False, Encoding.UTF8)
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
        Return fileName
    End Function

    Private Function CsvEsc(v As Object) As String
        If IsDBNull(v) Then Return ""
        Dim s = v.ToString()
        If s.Contains(",") OrElse s.Contains("""") OrElse s.Contains(vbNewLine) Then
            Return """" & s.Replace("""", """""") & """"
        End If
        Return s
    End Function

    ''' <summary>
    ''' 消費税率設定 (txt_ZRITU_1-5, txt_ZRITU_KBN_1-5) を txt_TEXT_02 にカンマ区切りで保存。
    ''' 形式: "rate1,kbn1,rate2,kbn2,...,rate5,kbn5"
    ''' </summary>
    Private Sub SaveZrituToText()
        Dim zrituControls = {txt_ZRITU_1, txt_ZRITU_2, txt_ZRITU_3, txt_ZRITU_4, txt_ZRITU_5}
        Dim zrituKbnControls = {txt_ZRITU_KBN_1, txt_ZRITU_KBN_2, txt_ZRITU_KBN_3, txt_ZRITU_KBN_4, txt_ZRITU_KBN_5}
        Dim parts As New List(Of String)
        For i = 0 To 4
            parts.Add(zrituControls(i).Text)
            parts.Add(zrituKbnControls(i).Text)
        Next
        txt_TEXT_02.Text = String.Join(",", parts)
    End Sub

    ''' <summary>
    ''' txt_TEXT_02 の内容を消費税率フィールドに展開する。
    ''' 空の場合はデフォルト値（0%/3%/5%/8%/10%）をセット。
    ''' </summary>
    Private Sub LoadZrituFromText(zrituText As String)
        If String.IsNullOrWhiteSpace(zrituText) Then
            txt_ZRITU_1.Text = "0" : txt_ZRITU_KBN_1.Text = "0"
            txt_ZRITU_2.Text = "0.03" : txt_ZRITU_KBN_2.Text = "1"
            txt_ZRITU_3.Text = "0.05" : txt_ZRITU_KBN_3.Text = "2"
            txt_ZRITU_4.Text = "0.08" : txt_ZRITU_KBN_4.Text = "3"
            txt_ZRITU_5.Text = "0.1" : txt_ZRITU_KBN_5.Text = "4"
            Return
        End If
        Dim parts = zrituText.Split(","c)
        Dim zrituControls = {txt_ZRITU_1, txt_ZRITU_2, txt_ZRITU_3, txt_ZRITU_4, txt_ZRITU_5}
        Dim zrituKbnControls = {txt_ZRITU_KBN_1, txt_ZRITU_KBN_2, txt_ZRITU_KBN_3, txt_ZRITU_KBN_4, txt_ZRITU_KBN_5}
        For i = 0 To 4
            Dim idx = i * 2
            If idx + 1 < parts.Length Then
                zrituControls(i).Text = parts(idx)
                zrituKbnControls(i).Text = parts(idx + 1)
            End If
        Next
    End Sub

    Private Sub LoadSettings()
        txt_YMD_02.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_KAMOKU_CD_01.Text = _settei.GetText(KEY_KAMOKU_01, "21010601")
        txt_KAMOKU_CD_02.Text = _settei.GetText(KEY_KAMOKU_02, "21010616")
        txt_BUMON_NM_01.Text = _settei.GetText(KEY_BUMON, "011307")
        txt_KAMOKU_CD_03.Text = _settei.GetText(KEY_SEG_CD1, "90101")
        txt_KAMOKU_NM_03.Text = _settei.GetText(KEY_SEG_NM1, "航空")
        txt_KAMOKU_CD_04.Text = _settei.GetText(KEY_SEG_CD2, "392")
        txt_TEXT_01.Text = _settei.GetText(KEY_FOLDER)
        LoadZrituFromText(_settei.GetText(KEY_ZRITU))
        オプション487.Checked = True  ' JOT は親会社がデフォルト
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_YMD_02.Text)
        _settei.SetText(KEY_KAMOKU_01, txt_KAMOKU_CD_01.Text)
        _settei.SetText(KEY_KAMOKU_02, txt_KAMOKU_CD_02.Text)
        _settei.SetText(KEY_BUMON, txt_BUMON_NM_01.Text)
        _settei.SetText(KEY_SEG_CD1, txt_KAMOKU_CD_03.Text)
        _settei.SetText(KEY_SEG_NM1, txt_KAMOKU_NM_03.Text)
        _settei.SetText(KEY_SEG_CD2, txt_KAMOKU_CD_04.Text)
        _settei.SetText(KEY_FOLDER, txt_TEXT_01.Text)
        _settei.SetText(KEY_ZRITU, txt_TEXT_02.Text)
    End Sub

    Private Sub Form_fc_JOT_計上仕訳_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
