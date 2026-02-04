<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLOGIN_JET
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
        Me.unnamed_Label_1917977666752 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977671936 = New System.Windows.Forms.Button()
        Me.unnamed_OptionGroup_1917977663744 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917970339200 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917970341504 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917970334208 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Jikko = New System.Windows.Forms.Button()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.txt_USER_CD = New System.Windows.Forms.TextBox()
        Me.txt_PWD = New System.Windows.Forms.TextBox()
        Me.ラベル81 = New System.Windows.Forms.Label()
        Me.ラベル82 = New System.Windows.Forms.Label()
        Me.txt_PATH = New System.Windows.Forms.TextBox()
        Me.ラベル88 = New System.Windows.Forms.Label()
        Me.ラベル94 = New System.Windows.Forms.Label()
        Me.cmb_AP_USER_SAVE_KIND = New System.Windows.Forms.ComboBox()
        Me.txt_USER_CD_SAVE = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' FrmLOGIN_JET
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977666752)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977671936)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917977663744)
        Me.Controls.Add(Me.unnamed_TextBox_1917970339200)
        Me.Controls.Add(Me.unnamed_ListBox_1917970341504)
        Me.Controls.Add(Me.unnamed_ComboBox_1917970334208)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977666752
        Me.unnamed_Label_1917977666752.Name = "unnamed_Label_1917977666752"
        Me.unnamed_Label_1917977666752.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977666752.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977671936
        Me.unnamed_CommandButton_1917977671936.Name = "unnamed_CommandButton_1917977671936"
        Me.unnamed_CommandButton_1917977671936.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977671936.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionGroup_1917977663744
        Me.unnamed_OptionGroup_1917977663744.Name = "unnamed_OptionGroup_1917977663744"
        Me.unnamed_OptionGroup_1917977663744.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917977663744.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917970339200
        Me.unnamed_TextBox_1917970339200.Name = "unnamed_TextBox_1917970339200"
        Me.unnamed_TextBox_1917970339200.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970339200.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917970341504
        Me.unnamed_ListBox_1917970341504.Name = "unnamed_ListBox_1917970341504"
        Me.unnamed_ListBox_1917970341504.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917970341504.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917970334208
        Me.unnamed_ComboBox_1917970334208.Name = "unnamed_ComboBox_1917970334208"
        Me.unnamed_ComboBox_1917970334208.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917970334208.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Jikko
        Me.cmd_Jikko.Name = "cmd_Jikko"
        Me.cmd_Jikko.Location = New System.Drawing.Point(18, 18)
        Me.cmd_Jikko.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Jikko.Text = "実行(&R)"
        Me.cmd_Jikko.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_Jikko)

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(102, 18)
        Me.cmd_Cancel.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Cancel.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_Cancel.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' txt_USER_CD
        Me.txt_USER_CD.Name = "txt_USER_CD"
        Me.txt_USER_CD.Location = New System.Drawing.Point(170, 75)
        Me.txt_USER_CD.Size = New System.Drawing.Size(151, 22)
        Me.pnlDetail.Controls.Add(Me.txt_USER_CD)

        ' txt_PWD
        Me.txt_PWD.Name = "txt_PWD"
        Me.txt_PWD.Location = New System.Drawing.Point(170, 113)
        Me.txt_PWD.Size = New System.Drawing.Size(151, 22)
        Me.txt_PWD.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_PWD)

        ' ラベル81
        Me.ラベル81.Name = "ラベル81"
        Me.ラベル81.Location = New System.Drawing.Point(37, 75)
        Me.ラベル81.Size = New System.Drawing.Size(113, 22)
        Me.ラベル81.Text = "利用者ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル81)

        ' ラベル82
        Me.ラベル82.Name = "ラベル82"
        Me.ラベル82.Location = New System.Drawing.Point(37, 113)
        Me.ラベル82.Size = New System.Drawing.Size(113, 22)
        Me.ラベル82.Text = "ﾊﾟｽﾜｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル82)

        ' txt_PATH
        Me.txt_PATH.Name = "txt_PATH"
        Me.txt_PATH.Location = New System.Drawing.Point(170, 177)
        Me.txt_PATH.Size = New System.Drawing.Size(378, 22)
        Me.txt_PATH.Visible = False
        Me.txt_PATH.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_PATH)

        ' ラベル88
        Me.ラベル88.Name = "ラベル88"
        Me.ラベル88.Location = New System.Drawing.Point(37, 177)
        Me.ラベル88.Size = New System.Drawing.Size(113, 22)
        Me.ラベル88.Text = "ﾃﾞｰﾀﾌｧｲﾙ"
        Me.ラベル88.Visible = False
        Me.pnlDetail.Controls.Add(Me.ラベル88)

        ' ラベル94
        Me.ラベル94.Name = "ラベル94"
        Me.ラベル94.Location = New System.Drawing.Point(37, 154)
        Me.ラベル94.Size = New System.Drawing.Size(113, 18)
        Me.ラベル94.Text = "利用者ｺｰﾄﾞの記録"
        Me.pnlDetail.Controls.Add(Me.ラベル94)

        ' cmb_AP_USER_SAVE_KIND
        Me.cmb_AP_USER_SAVE_KIND.Name = "cmb_AP_USER_SAVE_KIND"
        Me.cmb_AP_USER_SAVE_KIND.Location = New System.Drawing.Point(170, 154)
        Me.cmb_AP_USER_SAVE_KIND.Size = New System.Drawing.Size(207, 18)
        Me.cmb_AP_USER_SAVE_KIND.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmb_AP_USER_SAVE_KIND)

        ' txt_USER_CD_SAVE
        Me.txt_USER_CD_SAVE.Name = "txt_USER_CD_SAVE"
        Me.txt_USER_CD_SAVE.Location = New System.Drawing.Point(332, 79)
        Me.txt_USER_CD_SAVE.Size = New System.Drawing.Size(37, 18)
        Me.txt_USER_CD_SAVE.Visible = False
        Me.txt_USER_CD_SAVE.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_USER_CD_SAVE)

        Me.Name = "FrmLOGIN_JET"
        Me.Text = "リースＭ４ＢＳ　ログイン"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977666752 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977671936 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionGroup_1917977663744 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917970339200 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917970341504 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917970334208 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Jikko As System.Windows.Forms.Button
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents txt_USER_CD As System.Windows.Forms.TextBox
    Friend WithEvents txt_PWD As System.Windows.Forms.TextBox
    Friend WithEvents ラベル81 As System.Windows.Forms.Label
    Friend WithEvents ラベル82 As System.Windows.Forms.Label
    Friend WithEvents txt_PATH As System.Windows.Forms.TextBox
    Friend WithEvents ラベル88 As System.Windows.Forms.Label
    Friend WithEvents ラベル94 As System.Windows.Forms.Label
    Friend WithEvents cmb_AP_USER_SAVE_KIND As System.Windows.Forms.ComboBox
    Friend WithEvents txt_USER_CD_SAVE As System.Windows.Forms.TextBox

End Class
