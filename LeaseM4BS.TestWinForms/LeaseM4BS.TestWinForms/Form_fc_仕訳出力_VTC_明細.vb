Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' VTC顧客 仕訳出力 明細フォーム
''' Access版 fc_仕訳出力_VTC_明細 相当
''' tw_fc_swk_wrk（VTC仕訳データ）を表示し、テキストファイルに出力する。
''' 出力ファイル名: [ファイル名接頭辞]_yyyymmdd_hhmm.txt
''' 固定ファイル名: siwake_m4bs2.txt（同内容を同フォルダに出力）
''' </summary>
Partial Public Class Form_fc_仕訳出力_VTC_明細
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_仕訳出力_VTC_明細_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFirstRow()
    End Sub

    Private Sub LoadFirstRow()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT id, swk_kbn, den_date, den_no, dr_kmk_cd, dr_kin, " &
                "       cr_kmk_cd, cr_kin, zei, tekiyo " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'VTC' ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_ID.Text = row("id").ToString()
                txt_SWK_KBN_NM.Text = If(IsDBNull(row("swk_kbn")), "", row("swk_kbn").ToString())
                txt_GETSUDO.Text = If(IsDBNull(row("den_date")), "", row("den_date").ToString().Substring(0, 7))
                txt_SEIKYU_NO.Text = If(IsDBNull(row("den_no")), "", row("den_no").ToString())
                txt_D_KAMOKU.Text = If(IsDBNull(row("dr_kmk_cd")), "", row("dr_kmk_cd").ToString())
                txt_C_KAMOKU.Text = If(IsDBNull(row("cr_kmk_cd")), "", row("cr_kmk_cd").ToString())
                txt_D_KINGAKU.Text = If(IsDBNull(row("dr_kin")), "0", CType(row("dr_kin"), Double).ToString("N0"))
                txt_C_KINGAKU.Text = If(IsDBNull(row("cr_kin")), "0", CType(row("cr_kin"), Double).ToString("N0"))
                txt_ZEI.Text = If(IsDBNull(row("zei")), "0", CType(row("zei"), Double).ToString("N0"))
                txt_TEKIYO_CD.Text = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())
                txt_KEIJO_DT.Text = If(IsDBNull(row("den_date")), "", row("den_date").ToString())
            End If

            ' 合計表示
            Dim dtSum = _crud.GetDataTable(
                "SELECT COALESCE(SUM(dr_kin), 0) AS d_sum, " &
                "       COALESCE(SUM(cr_kin), 0) AS c_sum, " &
                "       COALESCE(SUM(zei), 0) AS zei_sum " &
                "FROM tw_fc_swk_wrk WHERE customer_cd = 'VTC'")
            If dtSum IsNot Nothing AndAlso dtSum.Rows.Count > 0 Then
                txt_D_KINGAKU_SUM.Text = CType(dtSum.Rows(0)("d_sum"), Double).ToString("N0")
                txt_C_KINGAKU_SUM.Text = CType(dtSum.Rows(0)("c_sum"), Double).ToString("N0")
                txt_ZEI_SUM.Text = CType(dtSum.Rows(0)("zei_sum"), Double).ToString("N0")
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

    Private Sub cmd_FlexReportDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexReportDLG.Click
        MessageBox.Show("印刷プレビューは未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If String.IsNullOrWhiteSpace(txt_FOLDER.Text) Then
            MessageBox.Show("出力先フォルダを設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmd_選択.Focus()
            Return
        End If

        If Not Directory.Exists(txt_FOLDER.Text) Then
            MessageBox.Show("出力先フォルダが存在しません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("実行してよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            Dim dt = _crud.GetDataTable(
                "SELECT swk_kbn, den_no, den_date, gyo_no, " &
                "       dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin, " &
                "       cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin, " &
                "       tekiyo, lsryo, zei " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'VTC' ORDER BY den_no, gyo_no")

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ファイル名: [接頭辞]_yyyymmdd_hhmm.txt
            Dim prefix = If(String.IsNullOrWhiteSpace(txt_FILE_NM接頭辞.Text), "VTC_仕訳", txt_FILE_NM接頭辞.Text.Trim())
            Dim timestamp = Format(Now, "yyyyMMdd_HHmm")
            Dim fileName = Path.Combine(txt_FOLDER.Text, $"{prefix}_{timestamp}.txt")
            Dim fixedName = Path.Combine(txt_FOLDER.Text, "siwake_m4bs2.txt")

            WriteVtcFile(dt, fileName)
            WriteVtcFile(dt, fixedName)

            txt_FILE_NM.Text = fileName
            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {fileName}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"出力エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub WriteVtcFile(dt As DataTable, filePath As String)
        Using sw As New StreamWriter(filePath, False, Encoding.UTF8)
            sw.WriteLine("仕訳区分,伝票番号,伝票日付,行番号,借方科目CD,借方補助科目CD,借方部門CD,借方金額,借方税額,貸方科目CD,貸方補助科目CD,貸方部門CD,貸方金額,摘要,税額")
            For Each row As DataRow In dt.Rows
                sw.WriteLine(String.Join(",",
                    CsvEsc(row("swk_kbn")), CsvEsc(row("den_no")), CsvEsc(row("den_date")),
                    row("gyo_no"),
                    CsvEsc(row("dr_kmk_cd")), CsvEsc(row("dr_hkm_cd")), CsvEsc(row("dr_bmn_cd")),
                    row("dr_kin"), row("dr_zei_kin"),
                    CsvEsc(row("cr_kmk_cd")), CsvEsc(row("cr_hkm_cd")), CsvEsc(row("cr_bmn_cd")),
                    row("cr_kin"), CsvEsc(row("tekiyo")), row("zei")))
            Next
        End Using
    End Sub

    Private Function CsvEsc(v As Object) As String
        If IsDBNull(v) Then Return ""
        Dim s = v.ToString()
        If s.Contains(",") OrElse s.Contains("""") OrElse s.Contains(vbNewLine) Then
            Return """" & s.Replace("""", """""") & """"
        End If
        Return s
    End Function

    Private Sub Form_fc_仕訳出力_VTC_明細_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
