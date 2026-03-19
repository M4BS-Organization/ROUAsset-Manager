Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' RISO顧客固有 支払仕訳出力フォーム
''' Access版 fc_支払仕訳_RISO 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に支払仕訳を出力して CSV に書き込む。
''' kjkbn_id=1（費用）、rec_kbn IN(1,3) を対象。
''' chk_検索条件加味F: 検索条件加味フラグ（keijo_f フィルタ適用有無）
''' txt_未払費用: 未払費用科目コード（貸方科目CDの上書き用）
''' txt_部署コード: 部署コード（借方部門CDの一括指定用）
''' </summary>
Partial Public Class Form_fc_支払仕訳_RISO
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "RISO"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "支払仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "RISO_HARAISWK_SLIP_DT"
    Private Const KEY_MIHARAI As String = "RISO_HARAISWK_MIHARAI_KAMOKU"
    Private Const KEY_BUMON As String = "RISO_HARAISWK_BUMON_CD"
    Private Const KEY_FOLDER As String = "RISO_HARAISWK_OUTPUT_FOLDER"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("RISO")
    End Sub

    Private Sub Form_fc_支払仕訳_RISO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'RISO' AND swk_kbn = '支払仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'RISO' AND swk_kbn = '支払仕訳' ORDER BY den_no, gyo_no")
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

    Private Sub cmd_支払日確認_Click(sender As Object, e As EventArgs) Handles cmd_支払日確認.Click
        MessageBox.Show("支払日確認機能は未実装です", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_振込_Click(sender As Object, e As EventArgs) Handles cmd_振込.Click
        MessageBox.Show("振込機能は未実装です", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（RISO支払仕訳版）
    ''' kjkbn_id=1（費用）、rec_kbn IN(1,3)。
    ''' txt_部署コード を dr_bmn_cd に使用。txt_未払費用 を cr_kmk_cd に上書き。
    ''' chk_検索条件加味F=True の場合 keijo_f=TRUE フィルタを適用。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_伝票日付.Text),
                        Format(Now, "yyyy/MM/dd"), txt_伝票日付.Text)
        Dim bumonCd = txt_部署コード.Text.Trim()
        Dim miharaiCd = txt_未払費用.Text.Trim()
        Dim keijoFilter = If(chk_検索条件加味F.Checked, " AND k.keijo_f = TRUE", "")

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'RISO', '支払仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    k.lsryo AS dr_kin,
    k.zei AS dr_zei_kin,
    CASE WHEN '{miharaiCd}' <> '' THEN '{miharaiCd}' ELSE COALESCE(h.cr_kmk_cd, '') END AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.lsryo + k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3){keijoFilter}
ORDER BY k.kykm_id"
    End Function

    ''' <summary>CSV形式でファイル出力する。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, $"RISO_支払仕訳_{Format(Now, "yyyyMMdd")}.csv")
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
        txt_伝票日付.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_未払費用.Text = _settei.GetText(KEY_MIHARAI)
        txt_部署コード.Text = _settei.GetText(KEY_BUMON)
        txt_OUTPUT_FPATH.Text = _settei.GetText(KEY_FOLDER)
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_伝票日付.Text)
        _settei.SetText(KEY_MIHARAI, txt_未払費用.Text)
        _settei.SetText(KEY_BUMON, txt_部署コード.Text)
        _settei.SetText(KEY_FOLDER, txt_OUTPUT_FPATH.Text)
    End Sub

    Private Sub Form_fc_支払仕訳_RISO_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
