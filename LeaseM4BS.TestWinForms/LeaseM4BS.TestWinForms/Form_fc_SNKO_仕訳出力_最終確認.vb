Imports System.Globalization
Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' SNKO顧客 支払仕訳出力 最終確認フォーム
''' Access版 fc_SNKO_仕訳出力_最終確認 相当
''' tw_fc_swk_wrk（SNKO 支払仕訳データ）を表示し、CSVファイルに出力する。
''' SNKO固有フォーマット: LINE_ID,貸借区分,勘定科目CD,予算組織CD,使用者社員番号,MAPCD,品目CD,金額,消費税CD,消費税額,個別摘要
''' </summary>
Partial Public Class Form_fc_SNKO_仕訳出力_最終確認
    Inherits Form

    Private _crud As New CrudHelper()
    Private _outputPath As String = ""
    Private _keijoBi As String = ""
    Private _userEmpNo As String = ""

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetParams(outputPath As String, keijoBi As String, userEmpNo As String)
        _outputPath = outputPath
        _keijoBi = keijoBi
        _userEmpNo = userEmpNo
    End Sub

    Private Sub Form_fc_SNKO_仕訳出力_最終確認_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' パラメータを画面に反映
        txt_OUTPUT_FPATH.Text = _outputPath
        If Not String.IsNullOrWhiteSpace(_keijoBi) Then
            Dim dt As DateTime
            If DateTime.TryParse(_keijoBi, dt) Then
                txt_計上日.Text = dt.ToString("yyyy/MM/dd")
                txt_計上日_曜日.Text = dt.ToString("ddd", New CultureInfo("ja-JP"))
            Else
                txt_計上日.Text = _keijoBi
            End If
        End If
        LoadFirstRow()
    End Sub

    Private Sub LoadFirstRow()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT id, den_no, gyo_no, den_date, " &
                "       dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin, " &
                "       cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin, " &
                "       tekiyo, lsryo, zei, rec_kbn " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'SNKO' AND swk_kbn = '支払仕訳' " &
                "ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_仕訳_ID.Text = row("id").ToString()
                LINE_ID.Text = If(IsDBNull(row("gyo_no")), "1", row("gyo_no").ToString())
                貸借区分.Text = If(IsDBNull(row("rec_kbn")), "1", row("rec_kbn").ToString())
                勘定科目CD.Text = If(IsDBNull(row("dr_kmk_cd")), "", row("dr_kmk_cd").ToString())
                勘定科目.Text = ""  ' マスタ未連携
                予算組織CD.Text = If(IsDBNull(row("dr_bmn_cd")), "", row("dr_bmn_cd").ToString())
                予算組織.Text = ""  ' マスタ未連携
                使用者社員番号.Text = _userEmpNo
                MAPCD.Text = If(IsDBNull(row("dr_hkm_cd")), "", row("dr_hkm_cd").ToString())
                MAP.Text = ""  ' マスタ未連携
                品目CD.Text = If(IsDBNull(row("cr_hkm_cd")), "", row("cr_hkm_cd").ToString())
                品目.Text = ""  ' マスタ未連携
                Dim drKin = If(IsDBNull(row("dr_kin")), 0.0, CType(row("dr_kin"), Double))
                金額.Text = drKin.ToString("N0")
                消費税CD.Text = ""
                Dim zeiKin = If(IsDBNull(row("dr_zei_kin")), 0.0, CType(row("dr_zei_kin"), Double))
                消費税額.Text = zeiKin.ToString("N0")
                個別摘要.Text = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())
            End If

            ' 借方・貸方合計
            Dim dtSum = _crud.GetDataTable(
                "SELECT COALESCE(SUM(dr_kin), 0) AS d_sum, COALESCE(SUM(cr_kin), 0) AS c_sum " &
                "FROM tw_fc_swk_wrk WHERE customer_cd = 'SNKO' AND swk_kbn = '支払仕訳'")
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
                "SELECT den_no, gyo_no, den_date, " &
                "       dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin, " &
                "       cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin, " &
                "       tekiyo, lsryo, zei, rec_kbn " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'SNKO' AND swk_kbn = '支払仕訳' ORDER BY den_no, gyo_no")

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

            WriteSnkoFile(dt, outputPath)

            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {outputPath}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"出力エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' SNKOフォーマット（CSV）でファイル出力する。
    ''' 借方行（貸借区分=1）・貸方行（貸借区分=2）を各レコードから出力。
    ''' </summary>
    Private Sub WriteSnkoFile(dt As DataTable, filePath As String)
        Using sw As New StreamWriter(filePath, False, Encoding.UTF8)
            sw.WriteLine("LINE_ID,貸借区分,勘定科目CD,予算組織CD,使用者社員番号,MAPCD,品目CD,金額,消費税CD,消費税額,個別摘要")
            Dim lineId As Integer = 1
            For Each row As DataRow In dt.Rows
                Dim drKmkCd = If(IsDBNull(row("dr_kmk_cd")), "", row("dr_kmk_cd").ToString())
                Dim crKmkCd = If(IsDBNull(row("cr_kmk_cd")), "", row("cr_kmk_cd").ToString())
                Dim drBmnCd = If(IsDBNull(row("dr_bmn_cd")), "", row("dr_bmn_cd").ToString())
                Dim drHkmCd = If(IsDBNull(row("dr_hkm_cd")), "", row("dr_hkm_cd").ToString())
                Dim crHkmCd = If(IsDBNull(row("cr_hkm_cd")), "", row("cr_hkm_cd").ToString())
                Dim drKin = If(IsDBNull(row("dr_kin")), 0.0, CType(row("dr_kin"), Double))
                Dim crKin = If(IsDBNull(row("cr_kin")), 0.0, CType(row("cr_kin"), Double))
                Dim zeiKin = If(IsDBNull(row("dr_zei_kin")), 0.0, CType(row("dr_zei_kin"), Double))
                Dim tekiyo = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())

                ' 借方行
                sw.WriteLine(String.Join(",",
                    lineId, "1",
                    CsvEsc(drKmkCd), CsvEsc(drBmnCd), CsvEsc(_userEmpNo),
                    CsvEsc(drHkmCd), "",
                    drKin.ToString("F0"), "", zeiKin.ToString("F0"),
                    CsvEsc(tekiyo)))
                lineId += 1

                ' 貸方行
                sw.WriteLine(String.Join(",",
                    lineId, "2",
                    CsvEsc(crKmkCd), "", CsvEsc(_userEmpNo),
                    CsvEsc(crHkmCd), "",
                    crKin.ToString("F0"), "", "",
                    CsvEsc(tekiyo)))
                lineId += 1
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

    Private Sub Form_fc_SNKO_仕訳出力_最終確認_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
