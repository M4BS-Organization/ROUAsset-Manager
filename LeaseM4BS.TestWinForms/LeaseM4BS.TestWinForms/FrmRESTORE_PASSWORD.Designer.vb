<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmRESTORE_PASSWORD
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
        Me.unnamed_Label_1917977701888 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977703360 = New System.Windows.Forms.Button()
        Me.unnamed_OptionGroup_1917977704576 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917977705472 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917977362624 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917977363776 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Jikko = New System.Windows.Forms.Button()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.txt_PWD = New System.Windows.Forms.TextBox()
        Me.ラベル99 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' FrmRESTORE_PASSWORD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(211, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977701888)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977703360)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917977704576)
        Me.Controls.Add(Me.unnamed_TextBox_1917977705472)
        Me.Controls.Add(Me.unnamed_ListBox_1917977362624)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977363776)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977701888
        Me.unnamed_Label_1917977701888.Name = "unnamed_Label_1917977701888"
        Me.unnamed_Label_1917977701888.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977701888.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977703360
        Me.unnamed_CommandButton_1917977703360.Name = "unnamed_CommandButton_1917977703360"
        Me.unnamed_CommandButton_1917977703360.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977703360.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionGroup_1917977704576
        Me.unnamed_OptionGroup_1917977704576.Name = "unnamed_OptionGroup_1917977704576"
        Me.unnamed_OptionGroup_1917977704576.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917977704576.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917977705472
        Me.unnamed_TextBox_1917977705472.Name = "unnamed_TextBox_1917977705472"
        Me.unnamed_TextBox_1917977705472.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977705472.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917977362624
        Me.unnamed_ListBox_1917977362624.Name = "unnamed_ListBox_1917977362624"
        Me.unnamed_ListBox_1917977362624.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917977362624.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917977363776
        Me.unnamed_ComboBox_1917977363776.Name = "unnamed_ComboBox_1917977363776"
        Me.unnamed_ComboBox_1917977363776.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977363776.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Jikko
        Me.cmd_Jikko.Name = "cmd_Jikko"
        Me.cmd_Jikko.Location = New System.Drawing.Point(22, 52)
        Me.cmd_Jikko.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Jikko.Text = "実行(&R)"
        Me.cmd_Jikko.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_Jikko)

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(113, 52)
        Me.cmd_Cancel.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Cancel.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_Cancel.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' txt_PWD
        Me.txt_PWD.Name = "txt_PWD"
        Me.txt_PWD.Location = New System.Drawing.Point(11, 22)
        Me.txt_PWD.Size = New System.Drawing.Size(188, 18)
        Me.pnlDetail.Controls.Add(Me.txt_PWD)

        ' ラベル99
        Me.ラベル99.Name = "ラベル99"
        Me.ラベル99.Location = New System.Drawing.Point(11, 3)
        Me.ラベル99.Size = New System.Drawing.Size(162, 15)
        Me.ラベル99.Text = "パスワードを入力して下さい"
        Me.pnlDetail.Controls.Add(Me.ラベル99)

        Me.Name = "FrmRESTORE_PASSWORD"
        Me.Text = "保存用MDBのパスワード指定"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977701888 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977703360 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionGroup_1917977704576 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917977705472 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917977362624 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917977363776 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Jikko As System.Windows.Forms.Button
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents txt_PWD As System.Windows.Forms.TextBox
    Friend WithEvents ラベル99 As System.Windows.Forms.Label

End Class
