<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLINK_KAKUNIN
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
        Me.unnamed_Label_1917977122752 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977123328 = New System.Windows.Forms.Button()
        Me.unnamed_OptionGroup_1917977127616 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917977127040 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917977123904 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917977118016 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_CLOSE = New System.Windows.Forms.Button()
        Me.txt_DBNAME = New System.Windows.Forms.TextBox()
        Me.ラベル86 = New System.Windows.Forms.Label()
        Me.cmd_LINK_DEL = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        ' FrmLINK_KAKUNIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977122752)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977123328)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917977127616)
        Me.Controls.Add(Me.unnamed_TextBox_1917977127040)
        Me.Controls.Add(Me.unnamed_ListBox_1917977123904)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977118016)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977122752
        Me.unnamed_Label_1917977122752.Name = "unnamed_Label_1917977122752"
        Me.unnamed_Label_1917977122752.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977122752.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977123328
        Me.unnamed_CommandButton_1917977123328.Name = "unnamed_CommandButton_1917977123328"
        Me.unnamed_CommandButton_1917977123328.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977123328.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionGroup_1917977127616
        Me.unnamed_OptionGroup_1917977127616.Name = "unnamed_OptionGroup_1917977127616"
        Me.unnamed_OptionGroup_1917977127616.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917977127616.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917977127040
        Me.unnamed_TextBox_1917977127040.Name = "unnamed_TextBox_1917977127040"
        Me.unnamed_TextBox_1917977127040.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977127040.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917977123904
        Me.unnamed_ListBox_1917977123904.Name = "unnamed_ListBox_1917977123904"
        Me.unnamed_ListBox_1917977123904.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917977123904.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917977118016
        Me.unnamed_ComboBox_1917977118016.Name = "unnamed_ComboBox_1917977118016"
        Me.unnamed_ComboBox_1917977118016.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977118016.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_CLOSE
        Me.cmd_CLOSE.Name = "cmd_CLOSE"
        Me.cmd_CLOSE.Location = New System.Drawing.Point(18, 18)
        Me.cmd_CLOSE.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CLOSE.Text = "閉じる(&C)"
        Me.pnlDetail.Controls.Add(Me.cmd_CLOSE)

        ' txt_DBNAME
        Me.txt_DBNAME.Name = "txt_DBNAME"
        Me.txt_DBNAME.Location = New System.Drawing.Point(169, 75)
        Me.txt_DBNAME.Size = New System.Drawing.Size(321, 22)
        Me.txt_DBNAME.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_DBNAME)

        ' ラベル86
        Me.ラベル86.Name = "ラベル86"
        Me.ラベル86.Location = New System.Drawing.Point(37, 75)
        Me.ラベル86.Size = New System.Drawing.Size(132, 22)
        Me.ラベル86.Text = "接続先データベース"
        Me.pnlDetail.Controls.Add(Me.ラベル86)

        ' cmd_LINK_DEL
        Me.cmd_LINK_DEL.Name = "cmd_LINK_DEL"
        Me.cmd_LINK_DEL.Location = New System.Drawing.Point(498, 75)
        Me.cmd_LINK_DEL.Size = New System.Drawing.Size(75, 22)
        Me.cmd_LINK_DEL.Text = "解除(&D)"
        Me.cmd_LINK_DEL.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_LINK_DEL)

        Me.Name = "FrmLINK_KAKUNIN"
        Me.Text = "リンク先の確認"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977122752 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977123328 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionGroup_1917977127616 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917977127040 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917977123904 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917977118016 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_CLOSE As System.Windows.Forms.Button
    Friend WithEvents txt_DBNAME As System.Windows.Forms.TextBox
    Friend WithEvents ラベル86 As System.Windows.Forms.Label
    Friend WithEvents cmd_LINK_DEL As System.Windows.Forms.Button

End Class
