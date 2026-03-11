Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

Partial Public Class FrmLogin

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtLoginId.Focus()
        lblVersion.Text = "v" & My.Application.Info.Version.ToString(3)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' バリデーション
        Dim loginId As String = txtLoginId.Text.Trim()
        If String.IsNullOrEmpty(loginId) Then
            lblError.Text = "ログインIDを入力してください"
            txtLoginId.Focus()
            Return
        End If
        If String.IsNullOrEmpty(txtPassword.Text) Then
            lblError.Text = "パスワードを入力してください"
            txtPassword.Focus()
            Return
        End If

        lblError.Text = ""
        Try
            Dim result As AuthResult = AuthorizationService.Current.Authenticate(
                loginId, txtPassword.Text)

            Select Case result
                Case AuthResult.Success
                    ShowMainMenu()
                Case AuthResult.UserNotFound, AuthResult.InvalidPassword
                    lblError.Text = "ログインIDまたはパスワードが正しくありません"
                    ClearPasswordAndFocus()
                Case AuthResult.AccountDisabled
                    lblError.Text = "このアカウントは無効化されています"
                    ClearPasswordAndFocus()
                Case AuthResult.AccountLocked
                    lblError.Text = "アカウントがロックされています"
                    ClearPasswordAndFocus()
            End Select
        Catch ex As Exception
            Debug.WriteLine(ex.ToString())
            lblError.Text = "システムエラーが発生しました。管理者に連絡してください。"
            ClearPasswordAndFocus()
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub ShowMainMenu()
        Me.Hide()
        Dim mainMenu As New FrmFlexMenu(AuthorizationService.Current.CurrentUser)
        AddHandler mainMenu.FormClosed, Sub(s, ev) Me.Close()
        mainMenu.Show()
    End Sub

    Private Sub ClearPasswordAndFocus()
        txtPassword.Text = ""
        txtLoginId.Focus()
    End Sub

End Class
