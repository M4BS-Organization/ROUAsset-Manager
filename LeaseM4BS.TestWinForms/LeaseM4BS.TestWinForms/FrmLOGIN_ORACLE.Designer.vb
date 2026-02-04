<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLOGIN_ORACLE
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.unnamed_Label_1917977116800 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977126016 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917977132992 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977126208 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Jikko = New System.Windows.Forms.Button()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.txt_USER_CD = New System.Windows.Forms.TextBox()
        Me.txt_PWD = New System.Windows.Forms.TextBox()
        Me.ラベル81 = New System.Windows.Forms.Label()
        Me.ラベル82 = New System.Windows.Forms.Label()
        Me.txt_DB_USER_NM = New System.Windows.Forms.TextBox()
        Me.txt_DB_PWD = New System.Windows.Forms.TextBox()
        Me.ラベル29 = New System.Windows.Forms.Label()
        Me.ラベル31 = New System.Windows.Forms.Label()
        Me.txt_DB_SERVICE_NM = New System.Windows.Forms.TextBox()
        Me.ラベル86 = New System.Windows.Forms.Label()
        Me.ラベル93 = New System.Windows.Forms.Label()
        Me.ラベル94 = New System.Windows.Forms.Label()
        Me.cmb_DB_USER_SAVE_KIND = New System.Windows.Forms.ComboBox()
        Me.cmb_AP_USER_SAVE_KIND = New System.Windows.Forms.ComboBox()
        Me.txt_DB_SERVICE_NM_SAVE = New System.Windows.Forms.TextBox()
        Me.txt_DB_USER_NM_SAVE = New System.Windows.Forms.TextBox()
        Me.txt_USER_CD_SAVE = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' FrmLOGIN_ORACLE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977116800)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977126016)
        Me.Controls.Add(Me.unnamed_TextBox_1917977132992)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977126208)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977116800
        Me.unnamed_Label_1917977116800.Name = "unnamed_Label_1917977116800"
        Me.unnamed_Label_1917977116800.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977116800.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977126016
        Me.unnamed_CommandButton_1917977126016.Name = "unnamed_CommandButton_1917977126016"
        Me.unnamed_CommandButton_1917977126016.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977126016.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917977132992
        Me.unnamed_TextBox_1917977132992.Name = "unnamed_TextBox_1917977132992"
        Me.unnamed_TextBox_1917977132992.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977132992.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977126208
        Me.unnamed_ComboBox_1917977126208.Name = "unnamed_ComboBox_1917977126208"
        Me.unnamed_ComboBox_1917977126208.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977126208.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Jikko
        Me.cmd_Jikko.Name = "cmd_Jikko"
        Me.cmd_Jikko.Location = New System.Drawing.Point(18, 18)
        Me.cmd_Jikko.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Jikko.Text = "実行(&R)"
        Me.cmd_Jikko.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmd_Jikko)

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(102, 18)
        Me.cmd_Cancel.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Cancel.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_Cancel.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' txt_USER_CD
        Me.txt_USER_CD.Name = "txt_USER_CD"
        Me.txt_USER_CD.Location = New System.Drawing.Point(188, 188)
        Me.txt_USER_CD.Size = New System.Drawing.Size(151, 22)
        Me.txt_USER_CD.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_USER_CD)

        ' txt_PWD
        Me.txt_PWD.Name = "txt_PWD"
        Me.txt_PWD.Location = New System.Drawing.Point(188, 219)
        Me.txt_PWD.Size = New System.Drawing.Size(151, 22)
        Me.txt_PWD.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_PWD)

        ' ラベル81
        Me.ラベル81.Name = "ラベル81"
        Me.ラベル81.Location = New System.Drawing.Point(37, 188)
        Me.ラベル81.Size = New System.Drawing.Size(132, 22)
        Me.ラベル81.Text = "利用者ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル81)

        ' ラベル82
        Me.ラベル82.Name = "ラベル82"
        Me.ラベル82.Location = New System.Drawing.Point(37, 219)
        Me.ラベル82.Size = New System.Drawing.Size(132, 22)
        Me.ラベル82.Text = "ﾊﾟｽﾜｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル82)

        ' txt_DB_USER_NM
        Me.txt_DB_USER_NM.Name = "txt_DB_USER_NM"
        Me.txt_DB_USER_NM.Location = New System.Drawing.Point(188, 105)
        Me.txt_DB_USER_NM.Size = New System.Drawing.Size(151, 22)
        Me.txt_DB_USER_NM.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_DB_USER_NM)

        ' txt_DB_PWD
        Me.txt_DB_PWD.Name = "txt_DB_PWD"
        Me.txt_DB_PWD.Location = New System.Drawing.Point(188, 136)
        Me.txt_DB_PWD.Size = New System.Drawing.Size(151, 22)
        Me.txt_DB_PWD.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_DB_PWD)

        ' ラベル29
        Me.ラベル29.Name = "ラベル29"
        Me.ラベル29.Location = New System.Drawing.Point(37, 105)
        Me.ラベル29.Size = New System.Drawing.Size(132, 22)
        Me.ラベル29.Text = "ﾃﾞｰﾀﾍﾞｰｽ・ﾕｰｻﾞｰ"
        Me.pnlDetail.Controls.Add(Me.ラベル29)

        ' ラベル31
        Me.ラベル31.Name = "ラベル31"
        Me.ラベル31.Location = New System.Drawing.Point(37, 136)
        Me.ラベル31.Size = New System.Drawing.Size(132, 22)
        Me.ラベル31.Text = "ﾃﾞｰﾀﾍﾞｰｽ・ﾊﾟｽﾜｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル31)

        ' txt_DB_SERVICE_NM
        Me.txt_DB_SERVICE_NM.Name = "txt_DB_SERVICE_NM"
        Me.txt_DB_SERVICE_NM.Location = New System.Drawing.Point(188, 75)
        Me.txt_DB_SERVICE_NM.Size = New System.Drawing.Size(151, 22)
        Me.pnlDetail.Controls.Add(Me.txt_DB_SERVICE_NM)

        ' ラベル86
        Me.ラベル86.Name = "ラベル86"
        Me.ラベル86.Location = New System.Drawing.Point(37, 75)
        Me.ラベル86.Size = New System.Drawing.Size(132, 22)
        Me.ラベル86.Text = "ﾃﾞｰﾀﾍﾞｰｽ・ｻｰﾋﾞｽ名"
        Me.pnlDetail.Controls.Add(Me.ラベル86)

        ' ラベル93
        Me.ラベル93.Name = "ラベル93"
        Me.ラベル93.Location = New System.Drawing.Point(37, 264)
        Me.ラベル93.Size = New System.Drawing.Size(151, 18)
        Me.ラベル93.Text = "ﾃﾞｰﾀﾍﾞｰｽ・ﾕｰｻﾞｰの記録"
        Me.pnlDetail.Controls.Add(Me.ラベル93)

        ' ラベル94
        Me.ラベル94.Name = "ラベル94"
        Me.ラベル94.Location = New System.Drawing.Point(37, 287)
        Me.ラベル94.Size = New System.Drawing.Size(151, 18)
        Me.ラベル94.Text = "利用者ｺｰﾄﾞの記録"
        Me.pnlDetail.Controls.Add(Me.ラベル94)

        ' cmb_DB_USER_SAVE_KIND
        Me.cmb_DB_USER_SAVE_KIND.Name = "cmb_DB_USER_SAVE_KIND"
        Me.cmb_DB_USER_SAVE_KIND.Location = New System.Drawing.Point(188, 264)
        Me.cmb_DB_USER_SAVE_KIND.Size = New System.Drawing.Size(207, 18)
        Me.cmb_DB_USER_SAVE_KIND.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmb_DB_USER_SAVE_KIND)

        ' cmb_AP_USER_SAVE_KIND
        Me.cmb_AP_USER_SAVE_KIND.Name = "cmb_AP_USER_SAVE_KIND"
        Me.cmb_AP_USER_SAVE_KIND.Location = New System.Drawing.Point(188, 287)
        Me.cmb_AP_USER_SAVE_KIND.Size = New System.Drawing.Size(207, 18)
        Me.cmb_AP_USER_SAVE_KIND.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.cmb_AP_USER_SAVE_KIND)

        ' txt_DB_SERVICE_NM_SAVE
        Me.txt_DB_SERVICE_NM_SAVE.Name = "txt_DB_SERVICE_NM_SAVE"
        Me.txt_DB_SERVICE_NM_SAVE.Location = New System.Drawing.Point(370, 75)
        Me.txt_DB_SERVICE_NM_SAVE.Size = New System.Drawing.Size(37, 18)
        Me.txt_DB_SERVICE_NM_SAVE.Visible = False
        Me.txt_DB_SERVICE_NM_SAVE.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_DB_SERVICE_NM_SAVE)

        ' txt_DB_USER_NM_SAVE
        Me.txt_DB_USER_NM_SAVE.Name = "txt_DB_USER_NM_SAVE"
        Me.txt_DB_USER_NM_SAVE.Location = New System.Drawing.Point(370, 105)
        Me.txt_DB_USER_NM_SAVE.Size = New System.Drawing.Size(37, 18)
        Me.txt_DB_USER_NM_SAVE.Visible = False
        Me.txt_DB_USER_NM_SAVE.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_DB_USER_NM_SAVE)

        ' txt_USER_CD_SAVE
        Me.txt_USER_CD_SAVE.Name = "txt_USER_CD_SAVE"
        Me.txt_USER_CD_SAVE.Location = New System.Drawing.Point(370, 188)
        Me.txt_USER_CD_SAVE.Size = New System.Drawing.Size(37, 18)
        Me.txt_USER_CD_SAVE.Visible = False
        Me.txt_USER_CD_SAVE.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_USER_CD_SAVE)

        Me.Name = "FrmLOGIN_ORACLE"
        Me.Text = "リースＭ４ＢＳ　ログイン"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977116800 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977126016 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917977132992 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977126208 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Jikko As System.Windows.Forms.Button
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents txt_USER_CD As System.Windows.Forms.TextBox
    Friend WithEvents txt_PWD As System.Windows.Forms.TextBox
    Friend WithEvents ラベル81 As System.Windows.Forms.Label
    Friend WithEvents ラベル82 As System.Windows.Forms.Label
    Friend WithEvents txt_DB_USER_NM As System.Windows.Forms.TextBox
    Friend WithEvents txt_DB_PWD As System.Windows.Forms.TextBox
    Friend WithEvents ラベル29 As System.Windows.Forms.Label
    Friend WithEvents ラベル31 As System.Windows.Forms.Label
    Friend WithEvents txt_DB_SERVICE_NM As System.Windows.Forms.TextBox
    Friend WithEvents ラベル86 As System.Windows.Forms.Label
    Friend WithEvents ラベル93 As System.Windows.Forms.Label
    Friend WithEvents ラベル94 As System.Windows.Forms.Label
    Friend WithEvents cmb_DB_USER_SAVE_KIND As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_AP_USER_SAVE_KIND As System.Windows.Forms.ComboBox
    Friend WithEvents txt_DB_SERVICE_NM_SAVE As System.Windows.Forms.TextBox
    Friend WithEvents txt_DB_USER_NM_SAVE As System.Windows.Forms.TextBox
    Friend WithEvents txt_USER_CD_SAVE As System.Windows.Forms.TextBox

End Class
