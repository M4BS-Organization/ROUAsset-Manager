Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' KYOTO顧客固有 支払仕訳出力フォーム
''' Access版 fc_支払仕訳_KYOTO 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に支払仕訳を出力して CSV に書き込む。
''' kjkbn_id=1（費用）、rec_kbn IN(1,3) を対象。
''' chk_CHK_01: 既存データ上書き確認フラグ（部分出力時の警告用）
''' </summary>
Partial Public Class Form_fc_支払仕訳_KYOTO
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "KYOTO"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "支払仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "KYOTO_HARAISWK_SLIP_DT"
    Private Const KEY_FOLDER As String = "KYOTO_HARAISWK_OUTPUT_FOLDER"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("KYOTO")
    End Sub

    Private Sub Form_fc_支払仕訳_KYOTO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        If chk_CHK_01.Checked Then
            If MessageBox.Show("月次仕訳計上フレックスに組み込み設定がされている場合、データを上書きしますが、よろしいですか？",
                               "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        End If

        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'KYOTO' AND swk_kbn = '支払仕訳'")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'KYOTO' AND swk_kbn = '支払仕訳' ORDER BY den_no, gyo_no")
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
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（KYOTO支払仕訳版）
    ''' kjkbn_id=1（費用）、rec_kbn IN(1,3)。勘定科目は t_haifu_keijo から取得。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_YMD_02.Text),
                        Format(Now, "yyyy/MM/dd"), txt_YMD_02.Text)

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'KYOTO', '支払仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '' AS dr_bmn_cd,
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
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY k.kykm_id"
    End Function

    ''' <summary>CSV形式でファイル出力する。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, $"KYOTO_支払仕訳_{Format(Now, "yyyyMMdd")}.csv")
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
        txt_TEXT_01.Text = _settei.GetText(KEY_FOLDER)
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_YMD_02.Text)
        _settei.SetText(KEY_FOLDER, txt_TEXT_01.Text)
    End Sub

    Private Sub Form_fc_支払仕訳_KYOTO_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class