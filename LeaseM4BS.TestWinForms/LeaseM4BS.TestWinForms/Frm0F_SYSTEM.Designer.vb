<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm0F_SYSTEM
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
        Me.unnamed_Label_1917970212160 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970212736 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917970213632 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Start = New System.Windows.Forms.Button()
        Me.txt_PWD = New System.Windows.Forms.TextBox()
        Me.ラベル22 = New System.Windows.Forms.Label()
        Me.ラベル9 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frm0F_SYSTEM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(377, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970212160)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970212736)
        Me.Controls.Add(Me.unnamed_TextBox_1917970213632)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970212160
        Me.unnamed_Label_1917970212160.Name = "unnamed_Label_1917970212160"
        Me.unnamed_Label_1917970212160.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970212160.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970212736
        Me.unnamed_CommandButton_1917970212736.Name = "unnamed_CommandButton_1917970212736"
        Me.unnamed_CommandButton_1917970212736.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970212736.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917970213632
        Me.unnamed_TextBox_1917970213632.Name = "unnamed_TextBox_1917970213632"
        Me.unnamed_TextBox_1917970213632.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970213632.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Start
        Me.cmd_Start.Name = "cmd_Start"
        Me.cmd_Start.Location = New System.Drawing.Point(102, 75)
        Me.cmd_Start.Size = New System.Drawing.Size(189, 22)
        Me.cmd_Start.Text = "開始"
        Me.cmd_Start.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_Start)

        ' txt_PWD
        Me.txt_PWD.Name = "txt_PWD"
        Me.txt_PWD.Location = New System.Drawing.Point(102, 41)
        Me.txt_PWD.Size = New System.Drawing.Size(188, 18)
        Me.pnlDetail.Controls.Add(Me.txt_PWD)

        ' ラベル22
        Me.ラベル22.Name = "ラベル22"
        Me.ラベル22.Location = New System.Drawing.Point(18, 41)
        Me.ラベル22.Size = New System.Drawing.Size(69, 14)
        Me.ラベル22.Text = "パスワード"
        Me.pnlDetail.Controls.Add(Me.ラベル22)

        ' ラベル9
        Me.ラベル9.Name = "ラベル9"
        Me.ラベル9.Location = New System.Drawing.Point(18, 7)
        Me.ラベル9.Size = New System.Drawing.Size(325, 22)
        Me.ラベル9.Text = "AutoExecマクロを実行を先に実行して下さい。\015\012"
        Me.pnlDetail.Controls.Add(Me.ラベル9)

        Me.Name = "Frm0F_SYSTEM"
        Me.Text = "Frm0F_SYSTEM"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970212160 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970212736 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917970213632 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Start As System.Windows.Forms.Button
    Friend WithEvents txt_PWD As System.Windows.Forms.TextBox
    Friend WithEvents ラベル22 As System.Windows.Forms.Label
    Friend WithEvents ラベル9 As System.Windows.Forms.Label

End Class
