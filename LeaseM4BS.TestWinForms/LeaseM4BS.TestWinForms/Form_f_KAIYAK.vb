Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 中途解約フォーム (f_KAIYAK)
''' d_kykh の契約情報を表示し、解約実行（d_kykm.b_ckaiyk_f=TRUE）を行う。
''' </summary>
Partial Public Class Form_f_KAIYAK
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部からパラメータをセット</summary>
    Public Sub SetParams(kykhId As Double)
        _kykhId = kykhId
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KAIYAK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContract()
    End Sub

    ' =========================================================
    '  契約情報読み込み
    ' =========================================================
    Private Sub LoadContract()
        If _kykhId = 0 Then Return
        Try
            Dim sql As String =
                "SELECT kyak_dt, start_dt, lkikan, shri_kn, shri_cnt, " &
                "       shri_dt1, shri_dt2, shri_dt3, shri_en_dt, jencho_f " &
                "FROM d_kykh WHERE kykh_id = @id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)}
            Dim dt = _crud.GetDataTable(sql, prm)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim r = dt.Rows(0)
            txt_KYAK_DT.Text = NzDtStr(r("kyak_dt"))
            txt_START_DT.Text = NzDtStr(r("start_dt"))
            txt_LKIKAN.Text = NzStr(r("lkikan"))
            txt_SHRI_KN.Text = NzStr(r("shri_kn"))
            txt_SHRI_CNT.Text = NzStr(r("shri_cnt"))
            txt_SHRI_DT1.Text = NzDtStr(r("shri_dt1"))
            txt_SHRI_DT2.Text = NzDtStr(r("shri_dt2"))
            txt_SHRI_DT3.Text = NzStr(r("shri_dt3"))
            txt_SHRI_EN_DT.Text = NzDtStr(r("shri_en_dt"))
            chk_JENCHO_F.Checked = NzBool(r("jencho_f"))
        Catch ex As Exception
            MessageBox.Show("契約情報読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If _kykhId = 0 Then
            MessageBox.Show("契約が指定されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show(
            "選択した契約の全物件を解約します。よろしいですか？",
            "解約確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            _crud.BeginTransaction()
            Dim val As New Dictionary(Of String, Object) From {
                {"b_ckaiyk_f", True},
                {"ckaiyk_dt", DateTime.Now}
            }
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kykh_id", _kykhId)
            }
            _crud.Update("d_kykm", val, "kykh_id = @kykh_id AND b_ckaiyk_f = FALSE", whereParams)
            _crud.Commit()

            MessageBox.Show("解約処理が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("解約処理エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_KAIYAK_ALL_Click(sender As Object, e As EventArgs) Handles cmd_KAIYAK_ALL.Click
        Dim frm As New Form_f_KAIYAK_ALL()
        frm.SetParams(_kykhId)
        frm.ShowDialog(Me)
        LoadContract()
    End Sub

    ' =========================================================
    '  ヘルパー
    ' =========================================================
    Private Function NzStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Return v.ToString()
    End Function

    Private Function NzDtStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Dim dt As DateTime
        If DateTime.TryParse(v.ToString(), dt) Then Return dt.ToString("yyyy/MM/dd")
        Return v.ToString()
    End Function

    Private Function NzBool(v As Object) As Boolean
        If IsDBNull(v) OrElse v Is Nothing Then Return False
        Try : Return Convert.ToBoolean(v)
        Catch : Return False
        End Try
    End Function

    Private Sub Form_f_KAIYAK_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
