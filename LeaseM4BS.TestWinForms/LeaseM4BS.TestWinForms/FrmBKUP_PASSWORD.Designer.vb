<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmBKUP_PASSWORD
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
        Me.unnamed_Label_1917970276480 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970277440 = New System.Windows.Forms.Button()
        Me.unnamed_OptionGroup_1917970277376 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917970279616 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917970279680 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917977717568 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Jikko = New System.Windows.Forms.Button()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.txt_PWD = New System.Windows.Forms.TextBox()
        Me.txt_PWD_RETRY = New System.Windows.Forms.TextBox()
        Me.ラベル96 = New System.Windows.Forms.Label()
        Me.ラベル100 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' FrmBKUP_PASSWORD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(283, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970276480)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970277440)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917970277376)
        Me.Controls.Add(Me.unnamed_TextBox_1917970279616)
        Me.Controls.Add(Me.unnamed_ListBox_1917970279680)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977717568)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970276480
        Me.unnamed_Label_1917970276480.Name = "unnamed_Label_1917970276480"
        Me.unnamed_Label_1917970276480.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970276480.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970277440
        Me.unnamed_CommandButton_1917970277440.Name = "unnamed_CommandButton_1917970277440"
        Me.unnamed_CommandButton_1917970277440.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970277440.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionGroup_1917970277376
        Me.unnamed_OptionGroup_1917970277376.Name = "unnamed_OptionGroup_1917970277376"
        Me.unnamed_OptionGroup_1917970277376.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917970277376.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917970279616
        Me.unnamed_TextBox_1917970279616.Name = "unnamed_TextBox_1917970279616"
        Me.unnamed_TextBox_1917970279616.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970279616.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917970279680
        Me.unnamed_ListBox_1917970279680.Name = "unnamed_ListBox_1917970279680"
        Me.unnamed_ListBox_1917970279680.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917970279680.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917977717568
        Me.unnamed_ComboBox_1917977717568.Name = "unnamed_ComboBox_1917977717568"
        Me.unnamed_ComboBox_1917977717568.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977717568.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Jikko
        Me.cmd_Jikko.Name = "cmd_Jikko"
        Me.cmd_Jikko.Location = New System.Drawing.Point(196, 3)
        Me.cmd_Jikko.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Jikko.Text = "実行(&R)"
        Me.cmd_Jikko.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_Jikko)

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(196, 37)
        Me.cmd_Cancel.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Cancel.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_Cancel.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' txt_PWD
        Me.txt_PWD.Name = "txt_PWD"
        Me.txt_PWD.Location = New System.Drawing.Point(7, 22)
        Me.txt_PWD.Size = New System.Drawing.Size(181, 18)
        Me.pnlDetail.Controls.Add(Me.txt_PWD)

        ' txt_PWD_RETRY
        Me.txt_PWD_RETRY.Name = "txt_PWD_RETRY"
        Me.txt_PWD_RETRY.Location = New System.Drawing.Point(7, 68)
        Me.txt_PWD_RETRY.Size = New System.Drawing.Size(181, 18)
        Me.txt_PWD_RETRY.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_PWD_RETRY)

        ' ラベル96
        Me.ラベル96.Name = "ラベル96"
        Me.ラベル96.Location = New System.Drawing.Point(7, 49)
        Me.ラベル96.Size = New System.Drawing.Size(113, 15)
        Me.ラベル96.Text = "ﾊﾟｽﾜｰﾄﾞ（確認）"
        Me.pnlDetail.Controls.Add(Me.ラベル96)

        ' ラベル100
        Me.ラベル100.Name = "ラベル100"
        Me.ラベル100.Location = New System.Drawing.Point(7, 3)
        Me.ラベル100.Size = New System.Drawing.Size(113, 15)
        Me.ラベル100.Text = "ﾊﾟｽﾜｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.ラベル100)

        Me.Name = "FrmBKUP_PASSWORD"
        Me.Text = "保存用MDBのパスワード指定"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970276480 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970277440 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionGroup_1917970277376 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917970279616 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917970279680 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917977717568 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Jikko As System.Windows.Forms.Button
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents txt_PWD As System.Windows.Forms.TextBox
    Friend WithEvents txt_PWD_RETRY As System.Windows.Forms.TextBox
    Friend WithEvents ラベル96 As System.Windows.Forms.Label
    Friend WithEvents ラベル100 As System.Windows.Forms.Label

End Class
