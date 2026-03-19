Imports System.Globalization
Imports System.IO
Imports LeaseM4BS.DataAccess

''' <summary>
''' SNKO顧客 計上仕訳出力 条件フォーム
''' Access版 fc_SNKO_計上仕訳出力_JOKEN 相当
''' tw_fc_swk_wrk（SNKO 計上仕訳データ）の計上日・出力先・固定値科目を設定し、
''' 最終確認フォームへ渡す。
''' 固定値: 使用者社員番号・国内仮払消費税科目・雑損失科目・リース総額仮勘定科目
''' </summary>
Partial Public Class Form_fc_SNKO_計上仕訳出力_JOKEN
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_SNKO_計上仕訳出力_JOKEN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDefaults()
    End Sub

    Private Sub LoadDefaults()
        Try
            ' tw_fc_swk_wrk から計上日を取得
            Dim dt = _crud.GetDataTable(
                "SELECT den_date " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'SNKO' AND swk_kbn = '計上仕訳' " &
                "ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                Dim denDate = If(IsDBNull(row("den_date")), CType(Nothing, DateTime?),
                                 CType(row("den_date"), DateTime))
                If denDate.HasValue Then
                    txt_計上日.Text = denDate.Value.ToString("yyyy/MM/dd")
                    txt_計上日_曜日.Text = denDate.Value.ToString("ddd", New CultureInfo("ja-JP"))
                End If
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateOutputFileName()
        If Not String.IsNullOrWhiteSpace(txt_OUTPUT_FOLDER_NM.Text) AndAlso
           Not String.IsNullOrWhiteSpace(txt_計上日.Text) Then
            Dim dateStr = txt_計上日.Text.Replace("/", "")
            Dim fileName = $"SNKO_計上仕訳_{dateStr}.txt"
            txt_OUTPUT_FILE_NM.Text = fileName
            txt_OUTPUT_FPATH.Text = Path.Combine(txt_OUTPUT_FOLDER_NM.Text, fileName)
        End If
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "出力先フォルダを選択してください"
            If Not String.IsNullOrWhiteSpace(txt_OUTPUT_FOLDER_NM.Text) Then
                dlg.SelectedPath = txt_OUTPUT_FOLDER_NM.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_OUTPUT_FOLDER_NM.Text = dlg.SelectedPath
                UpdateOutputFileName()
            End If
        End Using
    End Sub

    Private Sub txt_OUTPUT_FOLDER_NM_Leave(sender As Object, e As EventArgs) Handles txt_OUTPUT_FOLDER_NM.Leave
        UpdateOutputFileName()
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If String.IsNullOrWhiteSpace(txt_OUTPUT_FOLDER_NM.Text) Then
            MessageBox.Show("出力先フォルダを設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmd_選択.Focus()
            Return
        End If

        If Not Directory.Exists(txt_OUTPUT_FOLDER_NM.Text) Then
            MessageBox.Show("出力先フォルダが存在しません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' データ存在チェック
        Dim dtCheck = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk " &
            "WHERE customer_cd = 'SNKO' AND swk_kbn = '計上仕訳'")
        If dtCheck Is Nothing OrElse CInt(dtCheck.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力対象の計上仕訳データがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        UpdateOutputFileName()

        ' 最終確認フォームを開く
        Dim frmFinal As New Form_fc_SNKO_計上仕訳出力_最終確認()
        frmFinal.SetParams(txt_OUTPUT_FPATH.Text, txt_計上日.Text,
                           txt_使用者社員番号.Text, txt_仮払_科目.Text,
                           txt_雑損_科目.Text, txt_総額仮勘定_科目.Text)
        frmFinal.ShowDialog(Me)
        Me.Close()
    End Sub

    Private Sub Form_fc_SNKO_計上仕訳出力_JOKEN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
