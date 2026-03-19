Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' NIFS顧客固有 経費仕訳出力フォーム
''' Access版 fc_経費仕訳_NIFS 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に経費仕訳を出力して CSV に書き込む。
''' kjkbn_id=3（経費）、rec_kbn IN(1,3) を対象。
''' chk_検索条件加味F: 検索条件を加味するフラグ
''' chk_長期: 長期前払費用→販管費リース料 出力フラグ
''' chk_販管費: 販管費リース料→本来のリース料 出力フラグ
''' </summary>
Partial Public Class Form_fc_経費仕訳_NIFS
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "NIFS"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "経費仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "NIFS_KEIHI_SLIP_DT"
    Private Const KEY_BUMON_CD As String = "NIFS_KEIHI_BUMON_CD"
    Private Const KEY_HANKANZEI_CD As String = "NIFS_KEIHI_HANKANZEI_CD"
    Private Const KEY_CHOUKI_CD As String = "NIFS_KEIHI_CHOUKI_CD"
    Private Const KEY_KAMOKU3_CD As String = "NIFS_KEIHI_KAMOKU3_CD"
    Private Const KEY_FOLDER As String = "NIFS_KEIHI_FOLDER"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("NIFS")
    End Sub

    Private Sub Form_fc_経費仕訳_NIFS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            Me.Close()
            Return
        End If
        txt_SLIP_DT.Text = Format(kikanFrom, "yyyy/MM")
        txt_SLIP_DT.ReadOnly = True
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ValidateOutputFolder(txt_OUTPUT_FPATH.Text) Then Return

        ' 出力対象仕訳チェック
        If Not chk_長期.Checked AndAlso Not chk_販管費.Checked Then
            MessageBox.Show("出力対象が、選択されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If chk_検索条件加味F.Checked Then
            If MessageBox.Show("経費明細表フレックスに検索条件設定がされている場合、データを上書きしますが、よろしいですか？",
                               "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If

        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'NIFS' AND swk_kbn = '経費仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'NIFS' AND swk_kbn = '経費仕訳' ORDER BY den_no, gyo_no")
        Dim result = WriteOutputFile(dt, txt_OUTPUT_FPATH.Text)

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
            If Not String.IsNullOrWhiteSpace(txt_OUTPUT_FPATH.Text) Then
                dlg.SelectedPath = txt_OUTPUT_FPATH.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_OUTPUT_FPATH.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（NIFS経費仕訳版）
    ''' kjkbn_id=3（経費）、rec_kbn IN(1,3)。
    ''' 部署コード一括を dr_bmn_cd に設定。科目CDはCASE条件で切り替え。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = Format(kikanFrom, "yyyy/MM")
        Dim bumonCd = txt_部署コード_一括.Text
        Dim hankanCd = txt_販管費リース料.Text    ' 販管費リース料科目CD
        Dim choukiCd = txt_長期前払費用.Text      ' 長期前払費用科目CD
        Dim kamoku3Cd = txt_KAMOKU_CD_03.Text     ' 転リース原価科目CD

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'NIFS', '経費仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
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
WHERE k.kjkbn_id = 3 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY k.kykm_id"
    End Function

    ''' <summary>CSV形式でファイル出力する。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, $"NIFS_経費仕訳_{Format(Now, "yyyyMMdd")}.csv")
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
        txt_部署コード_一括.Text = _settei.GetText(KEY_BUMON_CD, "900")
        txt_販管費リース料.Text = _settei.GetText(KEY_HANKANZEI_CD, "27200")
        txt_長期前払費用.Text = _settei.GetText(KEY_CHOUKI_CD, "77500")
        txt_KAMOKU_CD_03.Text = _settei.GetText(KEY_KAMOKU3_CD, "62555")
        txt_OUTPUT_FPATH.Text = _settei.GetText(KEY_FOLDER)
        chk_検索条件加味F.Checked = True
        chk_長期.Checked = True
        chk_販管費.Checked = True
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_BUMON_CD, txt_部署コード_一括.Text)
        _settei.SetText(KEY_HANKANZEI_CD, txt_販管費リース料.Text)
        _settei.SetText(KEY_CHOUKI_CD, txt_長期前払費用.Text)
        _settei.SetText(KEY_KAMOKU3_CD, txt_KAMOKU_CD_03.Text)
        _settei.SetText(KEY_FOLDER, txt_OUTPUT_FPATH.Text)
    End Sub

    Private Sub Form_fc_経費仕訳_NIFS_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
