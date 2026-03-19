Imports LeaseM4BS.DataAccess

''' <summary>
''' JOT顧客 支払仕訳 伝票番号管理フォーム
''' Access版 fc_支払仕訳_JOT_伝票番号 相当
''' t_kykbnj_seq（契約番号採番テーブル）の年度・次回採番値・年度記号を表示・編集する。
''' 登録ボタンでt_kykbnj_seqを更新（UPSERT）。
''' </summary>
Partial Public Class Form_fc_支払仕訳_JOT_伝票番号
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_支払仕訳_JOT_伝票番号_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            Dim dt = _crud.GetDataTable("SELECT key, nextval, biko FROM t_kykbnj_seq ORDER BY key LIMIT 1")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_KEY.Text = If(IsDBNull(row("key")), "", row("key").ToString())
                txt_BIKO.Text = If(IsDBNull(row("biko")), "", row("biko").ToString())
                txt_NEXTVAL.Text = If(IsDBNull(row("nextval")), "", CType(row("nextval"), Double).ToString("F0"))
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    Private Sub cmd_Touroku_Click(sender As Object, e As EventArgs) Handles cmd_Touroku.Click
        If String.IsNullOrWhiteSpace(txt_KEY.Text) Then
            MessageBox.Show("年度を入力してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("登録してよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            Dim nextVal As Double = 0
            If Not Double.TryParse(txt_NEXTVAL.Text, nextVal) Then
                MessageBox.Show("次回採番値に数値を入力してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim key = txt_KEY.Text.Trim().Replace("'", "''")
            Dim biko = txt_BIKO.Text.Trim().Replace("'", "''")

            Dim countDt = _crud.GetDataTable($"SELECT COUNT(*) FROM t_kykbnj_seq WHERE key = '{key}'")
            Dim exists = CInt(countDt.Rows(0)(0)) > 0

            If exists Then
                _crud.ExecuteNonQuery(
                    $"UPDATE t_kykbnj_seq SET nextval = {nextVal}, biko = '{biko}' WHERE key = '{key}'")
            Else
                _crud.ExecuteNonQuery(
                    $"INSERT INTO t_kykbnj_seq (key, nextval, biko) VALUES ('{key}', {nextVal}, '{biko}')")
            End If

            MessageBox.Show("登録完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"登録エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_fc_支払仕訳_JOT_伝票番号_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
