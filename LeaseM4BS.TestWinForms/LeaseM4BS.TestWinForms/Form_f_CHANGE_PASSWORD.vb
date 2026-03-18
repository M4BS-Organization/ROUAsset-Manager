Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Imports LeaseM4BS.DataAccess
Imports Npgsql

' =========================================================
' 利用者パスワード変更画面
' Access版 Form_f_CHANGE_PASSWORD 相当
' =========================================================
Partial Public Class Form_f_CHANGE_PASSWORD
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_CHANGE_PASSWORD_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' パスワードマスク
        txt_OLD_PWD.PasswordChar = "*"c
        txt_NEW_PWD.PasswordChar = "*"c
        txt_NEW_PWD_RETRY.PasswordChar = "*"c

        ' ユーザー情報（読取専用）
        txt_USER_CD.Text = LoginSession.LoggedInUserCd
        txt_USER_CD.ReadOnly = True
        txt_USER_NM.Text = LoginSession.LoggedInUserNm
        txt_USER_NM.ReadOnly = True

        txt_OLD_PWD.Focus()
    End Sub

    ''' <summary>
    ''' 実行ボタン — パスワード変更処理
    ''' Access版 f_CHANGE_PASSWORD.cmd_Jikko_Click に準拠
    ''' </summary>
    Private Sub cmd_Jikko_Click(sender As Object, e As EventArgs) Handles cmd_Jikko.Click
        ' --- 入力チェック ---
        If String.IsNullOrEmpty(txt_OLD_PWD.Text) Then
            MessageBox.Show("旧パスワードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_OLD_PWD.Focus()
            Return
        End If
        If String.IsNullOrEmpty(txt_NEW_PWD.Text) Then
            MessageBox.Show("新パスワードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NEW_PWD.Focus()
            Return
        End If

        ' --- 新パスワード確認入力の一致チェック ---
        If txt_NEW_PWD.Text <> txt_NEW_PWD_RETRY.Text Then
            MessageBox.Show("新パスワード（確認）に誤りがあります。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NEW_PWD_RETRY.Focus()
            Return
        End If

        ' --- 旧新パスワード同一チェック (Access版 line 541-545) ---
        If txt_OLD_PWD.Text = txt_NEW_PWD.Text Then
            MessageBox.Show("新、旧に同じパスワードが入力されています。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NEW_PWD.Focus()
            Return
        End If

        ' --- 旧パスワード照合 ---
        Try
            Dim sql As String = "SELECT pwd FROM sec_user WHERE user_id = @user_id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@user_id", LoginSession.LoggedInUserId)
            }
            Dim dt = _crud.GetDataTable(sql, prms)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("ユーザー情報が見つかりません。", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim storedPwd As String = If(dt.Rows(0)("pwd") IsNot DBNull.Value, dt.Rows(0)("pwd").ToString(), "")
            If Not VerifyOldPassword(txt_OLD_PWD.Text, storedPwd) Then
                MessageBox.Show("旧パスワードに誤りがあります。", "認証エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_OLD_PWD.Focus()
                Return
            End If
        Catch ex As Exception
            MessageBox.Show("パスワード照合エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' --- PasswordValidator で新パスワード検証 ---
        Dim valResult = PasswordValidator.ValidateWithCurrentPolicy(txt_NEW_PWD.Text)
        If Not valResult.IsValid Then
            MessageBox.Show(valResult.ErrorMessage, "パスワードエラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NEW_PWD.Focus()
            Return
        End If

        ' --- DB更新 ---
        Try
            Dim newHash As String = ComputeSha256Hash(txt_NEW_PWD.Text)
            Dim updateSql As String =
                "UPDATE sec_user SET pwd = @pwd, pwd_upd_dt = @now, " &
                "d_first_login = NULL, history_f = FALSE, err_ct = 0 " &
                "WHERE user_id = @user_id"
            Dim updatePrms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@pwd", newHash),
                New NpgsqlParameter("@now", DateTime.Now),
                New NpgsqlParameter("@user_id", LoginSession.LoggedInUserId)
            }
            _crud.ExecuteNonQuery(updateSql, updatePrms)

            ' LoginSession のパスワード関連情報を更新
            LoginSession.PwdUpdDt = DateTime.Now
            LoginSession.IsFirstLogin = False

            ' 監査ログ
            LoginSession.WriteAuditLog(LoginSession.OP_KBN_USERPASSWORD, "パスワード変更")

            MessageBox.Show("パスワードを変更しました。", "完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("パスワード更新エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' =========================================================
    '  旧パスワード照合（SHA256 + 平文 + Access互換）
    ' =========================================================
    Private Function VerifyOldPassword(inputPwd As String, storedPwd As String) As Boolean
        If String.IsNullOrEmpty(storedPwd) AndAlso String.IsNullOrEmpty(inputPwd) Then Return True
        If String.IsNullOrEmpty(storedPwd) OrElse String.IsNullOrEmpty(inputPwd) Then Return False

        ' SHA256ハッシュ比較
        If String.Equals(ComputeSha256Hash(inputPwd), storedPwd, StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        ' 平文フォールバック
        If storedPwd = inputPwd Then Return True

        Return False
    End Function

    Private Shared Function ComputeSha256Hash(input As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function

End Class
