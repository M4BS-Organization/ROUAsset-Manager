Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' JOT顧客固有 支払仕訳出力フォーム
''' Access版 fc_JOT_支払仕訳 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に支払仕訳を出力して CSV ファイルに書き込む。
''' 借方: t_haifu_keijo の dr_kmk_cd / lsryo
''' 貸方: 同月=h.cr_kmk_cd、翌月以降=txt_KAMOKU_CD_01(売買) または txt_KAMOKU_CD_02(賃貸)
''' </summary>
Partial Public Class Form_fc_JOT_支払仕訳
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "JOT"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "支払仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "JOT_HARAISWK_SLIP_DT"
    Private Const KEY_KAMOKU_01 As String = "JOT_HARAISWK_KAMOKU_CD_01"
    Private Const KEY_KAMOKU_02 As String = "JOT_HARAISWK_KAMOKU_CD_02"
    Private Const KEY_BUMON As String = "JOT_HARAISWK_BUMON_NM"
    Private Const KEY_SEG_CD1 As String = "JOT_HARAISWK_SEG_CD1"
    Private Const KEY_SEG_NM1 As String = "JOT_HARAISWK_SEG_NM1"
    Private Const KEY_SEG_CD2 As String = "JOT_HARAISWK_SEG_CD2"
    Private Const KEY_FOLDER As String = "JOT_HARAISWK_OUTPUT_FOLDER"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("JOT")
    End Sub

    Private Sub Form_fc_JOT_支払仕訳_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'JOT' AND swk_kbn = '支払仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'JOT' AND swk_kbn = '支払仕訳' ORDER BY den_no, gyo_no")
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
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（JOT支払仕訳版）
    ''' Access版 pc_JOT_仕訳出力_COM 相当。
    ''' kjkbn_id=1（費用）、rec_kbn IN(1,3)（定額・付随費用）を対象。
    ''' 借方: h.dr_kmk_cd、貸方: 同月=h.cr_kmk_cd、翌月以降=KAMOKU_CD_01(売買) or KAMOKU_CD_02(賃貸)
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_YMD_02.Text),
                        Format(Now, "yyyy/MM/dd"), txt_YMD_02.Text)
        Dim kmkCd01 = txt_KAMOKU_CD_01.Text  ' 未払金・未決勘定（売買 leakbn_id=1 用）
        Dim kmkCd02 = txt_KAMOKU_CD_02.Text  ' 未払金・営業外費用（賃貸 leakbn_id=3/4 用）
        Dim bumonCd = txt_BUMON_NM_01.Text
        Dim kikanFromStr = Format(kikanFrom, "yyyy-MM-dd")

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'JOT', '支払仕訳',
    LPAD((1 + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    k.lsryo AS dr_kin,
    CASE WHEN k.leakbn_id IN (3, 4) AND k.zei = 0 THEN 0 ELSE k.zei END AS dr_zei_kin,
    CASE
        WHEN DATE_TRUNC('month', k.shri_dt) = DATE_TRUNC('month', '{kikanFromStr}'::date)
        THEN COALESCE(h.cr_kmk_cd, '')
        ELSE CASE WHEN k.leakbn_id = 1 THEN '{kmkCd01}' ELSE '{kmkCd02}' END
    END AS cr_kmk_cd,
    CASE
        WHEN DATE_TRUNC('month', k.shri_dt) = DATE_TRUNC('month', '{kikanFromStr}'::date)
        THEN COALESCE(h.cr_hkm_cd, '')
        ELSE ''
    END AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    CASE WHEN k.leakbn_id IN (3, 4) AND k.zei = 0 THEN k.lsryo ELSE k.lsryo + k.zei END AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY k.kykm_id"
    End Function

    ''' <summary>CSV形式でファイル出力する。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, $"JOT_支払仕訳_{Format(Now, "yyyyMMdd")}.csv")
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

    Private Sub LoadSettings()
        txt_YMD_02.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_KAMOKU_CD_01.Text = _settei.GetText(KEY_KAMOKU_01, "21010601")
        txt_KAMOKU_CD_02.Text = _settei.GetText(KEY_KAMOKU_02, "21010616")
        txt_BUMON_NM_01.Text = _settei.GetText(KEY_BUMON)
        txt_KAMOKU_CD_03.Text = _settei.GetText(KEY_SEG_CD1, "90101")
        txt_KAMOKU_NM_03.Text = _settei.GetText(KEY_SEG_NM1, "航空")
        txt_KAMOKU_CD_04.Text = _settei.GetText(KEY_SEG_CD2, "392")
        txt_TEXT_01.Text = _settei.GetText(KEY_FOLDER)
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
    End Sub

    Private Sub Form_fc_JOT_支払仕訳_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
