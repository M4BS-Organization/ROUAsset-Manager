<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLogin
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblLoginId = New System.Windows.Forms.Label()
        Me.txtLoginId = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()

        Dim CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
        Dim CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
        Dim CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
        Dim CLR_BG As Color = Color.White
        Dim FNT_TITLE As New Font("Meiryo", 13.0F, FontStyle.Bold)
        Dim FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)
        Dim FNT_INPUT As New Font("Meiryo", 9.75F, FontStyle.Regular)
        Dim FNT_SMALL As New Font("Meiryo", 8.0F, FontStyle.Regular)

        Me.SuspendLayout()

        '
        ' FrmLogin
        '
        Me.Text = "LeaseM4BS ログイン"
        Me.ClientSize = New Size(400, 350)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = True
        Me.BackColor = CLR_BG
        Me.Font = New Font("Meiryo", 9.0F)
        Me.AcceptButton = Me.btnLogin
        Me.CancelButton = Me.btnExit

        '
        ' lblTitle
        '
        Me.lblTitle.Text = "リース会計管理システム"
        Me.lblTitle.Font = FNT_TITLE
        Me.lblTitle.ForeColor = CLR_HEADER
        Me.lblTitle.TextAlign = ContentAlignment.MiddleCenter
        Me.lblTitle.SetBounds(20, 30, 360, 36)

        '
        ' lblLoginId
        '
        Me.lblLoginId.Text = "ログインID:"
        Me.lblLoginId.Font = FNT_LABEL
        Me.lblLoginId.ForeColor = CLR_LABEL
        Me.lblLoginId.SetBounds(60, 96, 100, 20)

        '
        ' txtLoginId
        '
        Me.txtLoginId.Font = FNT_INPUT
        Me.txtLoginId.MaxLength = 50
        Me.txtLoginId.SetBounds(60, 118, 280, 24)

        '
        ' lblPassword
        '
        Me.lblPassword.Text = "パスワード:"
        Me.lblPassword.Font = FNT_LABEL
        Me.lblPassword.ForeColor = CLR_LABEL
        Me.lblPassword.SetBounds(60, 158, 100, 20)

        '
        ' txtPassword
        '
        Me.txtPassword.Font = FNT_INPUT
        Me.txtPassword.PasswordChar = "*"c
        Me.txtPassword.UseSystemPasswordChar = True
        Me.txtPassword.MaxLength = 100
        Me.txtPassword.SetBounds(60, 180, 280, 24)

        '
        ' lblError
        '
        Me.lblError.ForeColor = Color.Red
        Me.lblError.Font = FNT_LABEL
        Me.lblError.Text = ""
        Me.lblError.TextAlign = ContentAlignment.MiddleCenter
        Me.lblError.SetBounds(20, 218, 360, 20)

        '
        ' btnLogin
        '
        Me.btnLogin.Text = "ログイン"
        Me.btnLogin.Font = FNT_LABEL
        Me.btnLogin.BackColor = CLR_HEADER
        Me.btnLogin.ForeColor = Color.White
        Me.btnLogin.FlatStyle = FlatStyle.Flat
        Me.btnLogin.FlatAppearance.BorderSize = 0
        Me.btnLogin.Cursor = Cursors.Hand
        Me.btnLogin.SetBounds(80, 252, 100, 34)

        '
        ' btnExit
        '
        Me.btnExit.Text = "終了"
        Me.btnExit.Font = FNT_LABEL
        Me.btnExit.FlatStyle = FlatStyle.Standard
        Me.btnExit.SetBounds(220, 252, 100, 34)

        '
        ' lblVersion
        '
        Me.lblVersion.Text = "v1.0.0"
        Me.lblVersion.Font = FNT_SMALL
        Me.lblVersion.ForeColor = CLR_LABEL
        Me.lblVersion.TextAlign = ContentAlignment.MiddleRight
        Me.lblVersion.SetBounds(280, 318, 100, 16)

        '
        ' Controls
        '
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblLoginId)
        Me.Controls.Add(Me.txtLoginId)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblVersion)

        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblLoginId As System.Windows.Forms.Label
    Friend WithEvents txtLoginId As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label

End Class
