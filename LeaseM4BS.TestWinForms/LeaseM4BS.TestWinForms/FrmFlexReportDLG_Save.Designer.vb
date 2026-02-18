<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexReportDLG_Save
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
        Me.unnamed_Label_1917977221056 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977222528 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917977219712 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977224000 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.テキスト13 = New System.Windows.Forms.Label()
        Me.cmd_Save = New System.Windows.Forms.Button()
        Me.cmd_Close = New System.Windows.Forms.Button()
        Me.cmd_Delete = New System.Windows.Forms.Button()
        Me.txt_SaveNo = New System.Windows.Forms.TextBox()
        Me.cmb_SaveName = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        ' FrmFlexReportDLG_Save
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977221056)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977222528)
        Me.Controls.Add(Me.unnamed_TextBox_1917977219712)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977224000)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977221056
        Me.unnamed_Label_1917977221056.Name = "unnamed_Label_1917977221056"
        Me.unnamed_Label_1917977221056.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977221056.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977222528
        Me.unnamed_CommandButton_1917977222528.Name = "unnamed_CommandButton_1917977222528"
        Me.unnamed_CommandButton_1917977222528.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977222528.Size = New System.Drawing.Size(113, 18)

        ' unnamed_TextBox_1917977219712
        Me.unnamed_TextBox_1917977219712.Name = "unnamed_TextBox_1917977219712"
        Me.unnamed_TextBox_1917977219712.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977219712.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977224000
        Me.unnamed_ComboBox_1917977224000.Name = "unnamed_ComboBox_1917977224000"
        Me.unnamed_ComboBox_1917977224000.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977224000.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' テキスト13
        Me.テキスト13.Name = "テキスト13"
        Me.テキスト13.Location = New System.Drawing.Point(7, 56)
        Me.テキスト13.Size = New System.Drawing.Size(113, 18)
        Me.テキスト13.Text = "記 録 名"
        Me.pnlDetail.Controls.Add(Me.テキスト13)

        ' cmd_Save
        Me.cmd_Save.Name = "cmd_Save"
        Me.cmd_Save.Location = New System.Drawing.Point(83, 3)
        Me.cmd_Save.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Save.Text = "登録(&S)"
        Me.cmd_Save.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_Save)

        ' cmd_Close
        Me.cmd_Close.Name = "cmd_Close"
        Me.cmd_Close.Location = New System.Drawing.Point(3, 3)
        Me.cmd_Close.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Close.Text = "閉じる(&C)"
        Me.cmd_Close.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_Close)

        ' cmd_Delete
        Me.cmd_Delete.Name = "cmd_Delete"
        Me.cmd_Delete.Location = New System.Drawing.Point(158, 3)
        Me.cmd_Delete.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Delete.Text = "削除(&D)"
        Me.cmd_Delete.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_Delete)

        ' txt_SaveNo
        Me.txt_SaveNo.Name = "txt_SaveNo"
        Me.txt_SaveNo.Location = New System.Drawing.Point(302, 75)
        Me.txt_SaveNo.Size = New System.Drawing.Size(37, 18)
        Me.txt_SaveNo.Visible = False
        Me.txt_SaveNo.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_SaveNo)

        ' cmb_SaveName
        Me.cmb_SaveName.Name = "cmb_SaveName"
        Me.cmb_SaveName.Location = New System.Drawing.Point(120, 56)
        Me.cmb_SaveName.Size = New System.Drawing.Size(340, 18)
        Me.pnlDetail.Controls.Add(Me.cmb_SaveName)

        Me.Name = "FrmFlexReportDLG_Save"
        Me.Text = "記録名の登録／削除"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977221056 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977222528 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917977219712 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977224000 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents テキスト13 As System.Windows.Forms.Label
    Friend WithEvents cmd_Save As System.Windows.Forms.Button
    Friend WithEvents cmd_Close As System.Windows.Forms.Button
    Friend WithEvents cmd_Delete As System.Windows.Forms.Button
    Friend WithEvents txt_SaveNo As System.Windows.Forms.TextBox
    Friend WithEvents cmb_SaveName As System.Windows.Forms.ComboBox

End Class
