Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' RISO顧客 仕訳出力 最終確認フォーム
''' Access版 fc_仕訳出力_最終確認_RISO 相当
''' tw_fc_swk_wrk（RISO仕訳データ）を表示し、GLOVIAフォーマットのテキストファイルに出力する。
''' txt_OUTPUT_FPATH: 出力先ファイルパス（フルパス指定）
''' GLOVIA形式: 固定フォーマットの仕訳伝票テキスト出力
''' </summary>
Partial Public Class Form_fc_仕訳出力_最終確認_RISO
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_仕訳出力_最終確認_RISO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFirstRow()
    End Sub

    Private Sub LoadFirstRow()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT id, den_date, den_no, gyo_no, " &
                "       dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin, " &
                "       cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin, " &
                "       tekiyo, lsryo, zei, rec_kbn, kjkbn_id " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'RISO' ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_仕訳_ID.Text = row("id").ToString()
                txt_処理年月.Text = If(IsDBNull(row("den_date")), "", row("den_date").ToString().Substring(0, 7))
                txt_伝票日付.Text = If(IsDBNull(row("den_date")), "", row("den_date").ToString())
                txt_伝票番号.Text = If(IsDBNull(row("den_no")), "", row("den_no").ToString())
                txt_行番号.Text = If(IsDBNull(row("gyo_no")), "0", row("gyo_no").ToString())
                勘定科目コード.Text = If(IsDBNull(row("dr_kmk_cd")), "", row("dr_kmk_cd").ToString())
                会計部門コード.Text = If(IsDBNull(row("dr_bmn_cd")), "", row("dr_bmn_cd").ToString())
                基本通貨発生金額.Text = If(IsDBNull(row("dr_kin")), "0", CType(row("dr_kin"), Double).ToString("N0"))
                参考消費税金額.Text = If(IsDBNull(row("dr_zei_kin")), "0", CType(row("dr_zei_kin"), Double).ToString("N0"))
                文字摘要１.Text = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())
            End If

            ' 借方・貸方合計
            Dim dtSum = _crud.GetDataTable(
                "SELECT COALESCE(SUM(dr_kin), 0) AS d_sum, COALESCE(SUM(cr_kin), 0) AS c_sum " &
                "FROM tw_fc_swk_wrk WHERE customer_cd = 'RISO'")
            If dtSum IsNot Nothing AndAlso dtSum.Rows.Count > 0 Then
                txt_借方金額_SUM.Text = CType(dtSum.Rows(0)("d_sum"), Double).ToString("N0")
                txt_貸方金額_SUM.Text = CType(dtSum.Rows(0)("c_sum"), Double).ToString("N0")
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New SaveFileDialog()
            dlg.Title = "出力先ファイルの指定"
            dlg.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*"
            If Not String.IsNullOrWhiteSpace(txt_OUTPUT_FPATH.Text) Then
                dlg.FileName = txt_OUTPUT_FPATH.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_OUTPUT_FPATH.Text = dlg.FileName
            End If
        End Using
    End Sub

    Private Sub cmd_FlexReportDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexReportDLG.Click
        MessageBox.Show("印刷プレビューは未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If String.IsNullOrWhiteSpace(txt_OUTPUT_FPATH.Text) Then
            MessageBox.Show("出力先ファイルを設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmd_選択.Focus()
            Return
        End If

        If MessageBox.Show("実行してよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            Dim dt = _crud.GetDataTable(
                "SELECT den_no, den_date, gyo_no, " &
                "       dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin, " &
                "       cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin, " &
                "       tekiyo, lsryo, zei, rec_kbn, kjkbn_id " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'RISO' ORDER BY den_no, gyo_no")

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim outputPath = txt_OUTPUT_FPATH.Text.Trim()
            Dim folder = Path.GetDirectoryName(outputPath)
            If Not String.IsNullOrWhiteSpace(folder) AndAlso Not Directory.Exists(folder) Then
                MessageBox.Show("出力先フォルダが存在しません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            WriteGloviaFile(dt, outputPath)

            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {outputPath}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"出力エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' GLOVIAフォーマット（CSV形式）でファイル出力する。
    ''' Access版 mTextOut() 相当 — RecordsetCloneを反復してGLOVIA固定フォーマットを出力。
    ''' </summary>
    Private Sub WriteGloviaFile(dt As DataTable, filePath As String)
        Using sw As New StreamWriter(filePath, False, Encoding.UTF8)
            ' GLOVIAヘッダー行（Access版と同一フィールド順）
            sw.WriteLine("伝票番号,伝票日付,行番号,伝票明細貸借区分,勘定科目コード,会計部門コード," &
                         "取引先コード,履歴物件コード,セグメントコード,取引通貨コード,資金コード," &
                         "基本通貨発生金額,消費税区分コード,参考消費税金額,文字摘要１," &
                         "会社コード,起票社員コード,起票部門コード,承認社員コード,承認日付," &
                         "承認状態区分,仕訳種別区分,伝票操作禁止区分,伝票備考," &
                         "細目コード識別区分,細目コード,集計拡張コード１識別区分,集計拡張コード１," &
                         "課税区分,税率区分,取引通貨発生金額,入力システム区分,入力番号")

            For Each row As DataRow In dt.Rows
                Dim drKin = If(IsDBNull(row("dr_kin")), 0.0, CType(row("dr_kin"), Double))
                Dim crKin = If(IsDBNull(row("cr_kin")), 0.0, CType(row("cr_kin"), Double))
                Dim zei = If(IsDBNull(row("zei")), 0.0, CType(row("zei"), Double))
                Dim tekiyo = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())
                Dim denNo = If(IsDBNull(row("den_no")), "", row("den_no").ToString())
                Dim denDt = If(IsDBNull(row("den_date")), "", row("den_date").ToString())
                Dim gyoNo = If(IsDBNull(row("gyo_no")), 0, CInt(row("gyo_no")))
                Dim drKmkCd = If(IsDBNull(row("dr_kmk_cd")), "", row("dr_kmk_cd").ToString())
                Dim drBmnCd = If(IsDBNull(row("dr_bmn_cd")), "", row("dr_bmn_cd").ToString())
                Dim crKmkCd = If(IsDBNull(row("cr_kmk_cd")), "", row("cr_kmk_cd").ToString())

                ' 借方行（伝票明細貸借区分=1）
                sw.WriteLine(String.Join(",",
                    CsvEsc(denNo), CsvEsc(denDt), gyoNo, "1",
                    CsvEsc(drKmkCd), CsvEsc(drBmnCd),
                    "", "", "", "", "",
                    drKin.ToString("F0"), "", zei.ToString("F0"), CsvEsc(tekiyo),
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""))

                ' 貸方行（伝票明細貸借区分=2）
                sw.WriteLine(String.Join(",",
                    CsvEsc(denNo), CsvEsc(denDt), gyoNo, "2",
                    CsvEsc(crKmkCd), "",
                    "", "", "", "", "",
                    crKin.ToString("F0"), "", "", CsvEsc(tekiyo),
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""))
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

    Private Sub Form_fc_仕訳出力_最終確認_RISO_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
