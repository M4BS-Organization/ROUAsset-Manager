Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' NIFS顧客固有 計上仕訳出力フォーム
''' Access版 fc_計上仕訳_NIFS 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に計上仕訳を出力して CSV に書き込む。
''' kjkbn_id=2（計上）、rec_kbn IN(1,3) + rec_kbn=2(税額>0) を対象。
''' 通常リース: chk_CHK_02(減損時), chk_CHK_03(月次入力・減損分)
''' 資産リース: chk_CHK_04(開始時), chk_CHK_05(支払時), chk_CHK_06(月次入力・債務返済), chk_CHK_07(月次入力・減価償却), chk_CHK_08(減損時)
''' 転リース: chk_CHK_09(月次入力・債務返済)
''' </summary>
Partial Public Class Form_fc_計上仕訳_NIFS
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "NIFS"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "計上仕訳"
        End Get
    End Property

    Private Const KEY_YMD As String = "NIFS_KEIJOSWK_YMD"
    Private Const KEY_KMK01 As String = "NIFS_KEIJOSWK_KMK01"
    Private Const KEY_KMK02 As String = "NIFS_KEIJOSWK_KMK02"
    Private Const KEY_KMK03 As String = "NIFS_KEIJOSWK_KMK03"
    Private Const KEY_KMK04 As String = "NIFS_KEIJOSWK_KMK04"
    Private Const KEY_KMK05 As String = "NIFS_KEIJOSWK_KMK05"
    Private Const KEY_KMK06 As String = "NIFS_KEIJOSWK_KMK06"
    Private Const KEY_KMK07 As String = "NIFS_KEIJOSWK_KMK07"
    Private Const KEY_BUMON01 As String = "NIFS_KEIJOSWK_BUMON01"
    Private Const KEY_BUMON02 As String = "NIFS_KEIJOSWK_BUMON02"
    Private Const KEY_FILENM As String = "NIFS_KEIJOSWK_FILENM"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("NIFS")
    End Sub

    Private Sub Form_fc_計上仕訳_NIFS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        ' 出力ファイル名チェック
        If String.IsNullOrWhiteSpace(txt_FileNM.Text) Then
            MessageBox.Show("出力ファイル名を指定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If chk_CHK_01.Checked Then
            If MessageBox.Show("月次仕訳計上フレックスに検索条件設定がされている場合、データを上書きしますが、よろしいですか？",
                               "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If

        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'NIFS' AND swk_kbn = '計上仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'NIFS' AND swk_kbn = '計上仕訳' ORDER BY den_no, gyo_no")

        ' txt_FileNM はフルパスなので、フォルダ部分を抽出して渡す
        Dim folder = Path.GetDirectoryName(txt_FileNM.Text)
        If String.IsNullOrWhiteSpace(folder) Then folder = ""
        Dim result = WriteOutputFile(dt, folder)

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
        Using dlg As New SaveFileDialog()
            dlg.Title = "出力先ファイル名の指定"
            dlg.Filter = "Microsoft Excel(*.xls)|*.xls|すべてのファイル(*.*)|*.*"
            If Not String.IsNullOrWhiteSpace(txt_FileNM.Text) Then
                dlg.FileName = txt_FileNM.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_FileNM.Text = dlg.FileName
            End If
        End Using
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（NIFS計上仕訳版）
    ''' kjkbn_id=2（計上）、rec_kbn IN(1,3) UNION ALL rec_kbn=2(税額>0)。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = Format(kikanFrom, "yyyy/MM")
        Dim bumonCd01 = txt_KAMOKU_CD_01.Text  ' 部署コード一括
        Dim kmkCd02 = txt_KAMOKU_CD_02.Text    ' 長期前払費用(リース)
        Dim kmkCd03 = txt_KAMOKU_CD_03.Text    ' リース債務(資産リース)
        Dim kmkCd04 = txt_KAMOKU_CD_04.Text    ' 長期未払金
        Dim kmkCd07 = txt_KAMOKU_CD_07.Text    ' リース債務(転リース)
        Dim bumon01 = txt_BUMON_CD_01.Text     ' 上位部門CD
        Dim bumon02 = txt_BUMON_CD_02.Text     ' 部門CD
        Dim kmkCd05 = txt_KAMOKU_CD_05.Text    ' 減損勘定取崩科目CD
        Dim kmkCd06 = txt_KAMOKU_CD_06.Text    ' 減損勘定取崩セグメントCD

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'NIFS', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd01}' AS dr_bmn_cd,
    k.lsryo AS dr_kin,
    k.zei AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.lsryo + k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE

UNION ALL

SELECT
    'NIFS', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1 + 90000000)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd01}' AS dr_bmn_cd,
    k.zei AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, 0, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 2 AND k.zei > 0 AND k.keijo_f = TRUE

ORDER BY 1"
    End Function

    ''' <summary>CSV形式でファイル出力する。txt_FileNM をフルパスとして使用。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        ' txt_FileNM がフルパスの場合はそのまま使用
        Dim fileName As String
        If Not String.IsNullOrWhiteSpace(txt_FileNM.Text) Then
            fileName = txt_FileNM.Text
        Else
            fileName = Path.Combine(outputFolder, $"NIFS_計上仕訳_{Format(Now, "yyyyMMdd")}.csv")
        End If

        Using sw As New StreamWriter(fileName, False, Encoding.UTF8)
            sw.WriteLine("伝票番号,伝票日付,借方科目CD,借方補助科目CD,借方部門CD,借方金額,借方税額,貸方科目CD,貸方補助科目CD,貸方金額,摘要")
            For Each row As DataRow In dt.Rows
                sw.WriteLine(String.Join(",",
                    CsvEsc(row("den_no")), CsvEsc(row("den_date")),
                    CsvEsc(row("dr_kmk_cd")), CsvEsc(row("dr_hkm_cd")),
                    CsvEsc(row("dr_bmn_cd")),
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
        txt_KAMOKU_CD_01.Text = _settei.GetText(KEY_KMK01, "900")
        txt_KAMOKU_CD_02.Text = _settei.GetText(KEY_KMK02, "27200")
        txt_KAMOKU_CD_03.Text = _settei.GetText(KEY_KMK03, "A9840")
        txt_KAMOKU_CD_04.Text = _settei.GetText(KEY_KMK04, "A9845")
        txt_KAMOKU_CD_05.Text = _settei.GetText(KEY_KMK05, "77500")
        txt_KAMOKU_CD_06.Text = _settei.GetText(KEY_KMK06, "7")
        txt_KAMOKU_CD_07.Text = _settei.GetText(KEY_KMK07, "A9841")
        txt_BUMON_CD_01.Text = _settei.GetText(KEY_BUMON01, "900")
        txt_BUMON_CD_02.Text = _settei.GetText(KEY_BUMON02, "910")
        txt_FileNM.Text = _settei.GetText(KEY_FILENM)
        ' デフォルトチェック状態（Access版と同一）
        chk_CHK_01.Checked = True
        chk_CHK_02.Checked = True
        chk_CHK_03.Checked = True
        chk_CHK_04.Checked = True
        chk_CHK_05.Checked = True
        chk_CHK_06.Checked = True
        chk_CHK_07.Checked = True
        chk_CHK_08.Checked = True
        chk_CHK_09.Checked = True
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_KMK01, txt_KAMOKU_CD_01.Text)
        _settei.SetText(KEY_KMK02, txt_KAMOKU_CD_02.Text)
        _settei.SetText(KEY_KMK03, txt_KAMOKU_CD_03.Text)
        _settei.SetText(KEY_KMK04, txt_KAMOKU_CD_04.Text)
        _settei.SetText(KEY_KMK05, txt_KAMOKU_CD_05.Text)
        _settei.SetText(KEY_KMK06, txt_KAMOKU_CD_06.Text)
        _settei.SetText(KEY_KMK07, txt_KAMOKU_CD_07.Text)
        _settei.SetText(KEY_BUMON01, txt_BUMON_CD_01.Text)
        _settei.SetText(KEY_BUMON02, txt_BUMON_CD_02.Text)
        _settei.SetText(KEY_FILENM, txt_FileNM.Text)
    End Sub

    Private Sub Form_fc_計上仕訳_NIFS_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
