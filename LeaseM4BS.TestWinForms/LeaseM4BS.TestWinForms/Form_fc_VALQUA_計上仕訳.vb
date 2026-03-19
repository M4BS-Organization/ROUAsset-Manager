Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' VALQUA顧客固有 計上仕訳出力フォーム
''' Access版 fc_VALQUA_計上仕訳 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に計上仕訳を出力して複数CSVに書き込む。
''' kjkbn_id=2（資産）を対象。
''' 資産計上/債務取崩の2ファイルをチェックボックスに応じて出力。
''' </summary>
Partial Public Class Form_fc_VALQUA_計上仕訳
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "VALQUA"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "計上仕訳"
        End Get
    End Property

    Private Const KEY_KEIJYO_DT As String = "VALQUA_KEIJOSWK_KEIJYO_DT"
    Private Const KEY_FOLDER As String = "VALQUA_KEIJOSWK_FOLDER"
    Private Const KEY_FNAME_SHISAN As String = "VALQUA_KEIJOSWK_FNAME_SHISAN"
    Private Const KEY_FNAME_SAIMU As String = "VALQUA_KEIJOSWK_FNAME_SAIMU"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("VALQUA")
    End Sub

    Private Sub Form_fc_VALQUA_計上仕訳_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        chk_資産計上F.Checked = True
        chk_債務取崩F.Checked = True
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ValidateOutputFolder(txt_FOLDER.Text) Then Return
        If Not ConfirmExecute() Then Return

        ClearWorkTable()

        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_KEIJYO_DT.Text),
                        Format(Now, "yyyy/MM/dd"), txt_KEIJYO_DT.Text)

        Dim sql = $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'VALQUA', '計上仕訳',
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
WHERE k.kjkbn_id = 2 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY k.kykm_id"

        _crud.ExecuteNonQuery(sql)

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'VALQUA' AND swk_kbn = '計上仕訳'")
        Dim cnt = CInt(dtCount.Rows(0)(0))

        If cnt = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = 'VALQUA' AND swk_kbn = '計上仕訳' ORDER BY den_no, gyo_no")

        Dim results As New List(Of String)
        If chk_資産計上F.Checked AndAlso Not String.IsNullOrWhiteSpace(txt_FNAME_資産計上.Text) Then
            results.Add(WriteCsvFile(dt, txt_FOLDER.Text, txt_FNAME_資産計上.Text))
        End If
        If chk_債務取崩F.Checked AndAlso Not String.IsNullOrWhiteSpace(txt_FNAME_債務取崩.Text) Then
            results.Add(WriteCsvFile(dt, txt_FOLDER.Text, txt_FNAME_債務取崩.Text))
        End If

        If results.Count > 0 Then
            SaveSettings()
            MessageBox.Show($"出力完了しました。{Environment.NewLine}{String.Join(Environment.NewLine, results)}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "出力先フォルダを選択してください"
            If Not String.IsNullOrWhiteSpace(txt_FOLDER.Text) Then
                dlg.SelectedPath = txt_FOLDER.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_FOLDER.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ''' <summary>VALQUA計上仕訳は BuildInsertToWrkSql を使わず cmd_実行_Click 内で直接SQL実行。</summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Return ""
    End Function

    ''' <summary>VALQUAは複数ファイル出力のため、WriteOutputFile は使用しない。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Return Nothing
    End Function

    Private Function WriteCsvFile(dt As DataTable, folder As String, fileName As String) As String
        Dim filePath = Path.Combine(folder, fileName)
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
        txt_KEIJYO_DT.Text = _settei.GetText(KEY_KEIJYO_DT, Format(Now, "yyyy/MM/dd"))
        txt_FOLDER.Text = _settei.GetText(KEY_FOLDER)
        txt_FNAME_資産計上.Text = _settei.GetText(KEY_FNAME_SHISAN)
        txt_FNAME_債務取崩.Text = _settei.GetText(KEY_FNAME_SAIMU)
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_KEIJYO_DT, txt_KEIJYO_DT.Text)
        _settei.SetText(KEY_FOLDER, txt_FOLDER.Text)
        _settei.SetText(KEY_FNAME_SHISAN, txt_FNAME_資産計上.Text)
        _settei.SetText(KEY_FNAME_SAIMU, txt_FNAME_債務取崩.Text)
    End Sub

    Private Sub Form_fc_VALQUA_計上仕訳_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
