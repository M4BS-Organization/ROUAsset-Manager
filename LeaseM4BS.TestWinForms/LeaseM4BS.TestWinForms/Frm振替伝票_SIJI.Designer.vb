<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm振替伝票_SIJI
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
        Me.unnamed_Label_1917970303808 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970305088 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970305856 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970440320 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_TAISYOKENSU = New System.Windows.Forms.TextBox()
        Me.Lbl = New System.Windows.Forms.Label()
        Me.txt_KENSAKUKENSU = New System.Windows.Forms.TextBox()
        Me.ラベル0 = New System.Windows.Forms.Label()
        Me.chk_EXCEL_OUT = New System.Windows.Forms.CheckBox()
        Me.ラベル23 = New System.Windows.Forms.Label()
        Me.chk_KENSAKU = New System.Windows.Forms.CheckBox()
        Me.ラベル25 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frm振替伝票_SIJI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(491, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970303808)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970305088)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970305856)
        Me.Controls.Add(Me.unnamed_TextBox_1917970440320)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970303808
        Me.unnamed_Label_1917970303808.Name = "unnamed_Label_1917970303808"
        Me.unnamed_Label_1917970303808.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970303808.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970305088
        Me.unnamed_CommandButton_1917970305088.Name = "unnamed_CommandButton_1917970305088"
        Me.unnamed_CommandButton_1917970305088.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970305088.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970305856
        Me.unnamed_CheckBox_1917970305856.Name = "unnamed_CheckBox_1917970305856"
        Me.unnamed_CheckBox_1917970305856.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970305856.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970440320
        Me.unnamed_TextBox_1917970440320.Name = "unnamed_TextBox_1917970440320"
        Me.unnamed_TextBox_1917970440320.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970440320.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(0, 0)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.cmd_実行.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(82, 0)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_TAISYOKENSU
        Me.txt_TAISYOKENSU.Name = "txt_TAISYOKENSU"
        Me.txt_TAISYOKENSU.Location = New System.Drawing.Point(139, 45)
        Me.txt_TAISYOKENSU.Size = New System.Drawing.Size(75, 18)
        Me.pnlDetail.Controls.Add(Me.txt_TAISYOKENSU)

        ' Lbl
        Me.Lbl.Name = "Lbl"
        Me.Lbl.Location = New System.Drawing.Point(45, 45)
        Me.Lbl.Size = New System.Drawing.Size(94, 18)
        Me.Lbl.Text = "対象件数"
        Me.pnlDetail.Controls.Add(Me.Lbl)

        ' txt_KENSAKUKENSU
        Me.txt_KENSAKUKENSU.Name = "txt_KENSAKUKENSU"
        Me.txt_KENSAKUKENSU.Location = New System.Drawing.Point(139, 71)
        Me.txt_KENSAKUKENSU.Size = New System.Drawing.Size(75, 18)
        Me.txt_KENSAKUKENSU.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_KENSAKUKENSU)

        ' ラベル0
        Me.ラベル0.Name = "ラベル0"
        Me.ラベル0.Location = New System.Drawing.Point(45, 71)
        Me.ラベル0.Size = New System.Drawing.Size(94, 18)
        Me.ラベル0.Text = "内検索件数"
        Me.pnlDetail.Controls.Add(Me.ラベル0)

        ' chk_EXCEL_OUT
        Me.chk_EXCEL_OUT.Name = "chk_EXCEL_OUT"
        Me.chk_EXCEL_OUT.Location = New System.Drawing.Point(75, 104)
        Me.chk_EXCEL_OUT.Size = New System.Drawing.Size(252, 18)
        Me.chk_EXCEL_OUT.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.chk_EXCEL_OUT)

        ' ラベル23
        Me.ラベル23.Name = "ラベル23"
        Me.ラベル23.Location = New System.Drawing.Point(90, 102)
        Me.ラベル23.Size = New System.Drawing.Size(162, 15)
        Me.ラベル23.Text = "明細のEXCEL出力を行う"
        Me.chk_EXCEL_OUT.Controls.Add(Me.ラベル23)

        ' chk_KENSAKU
        Me.chk_KENSAKU.Name = "chk_KENSAKU"
        Me.chk_KENSAKU.Location = New System.Drawing.Point(75, 126)
        Me.chk_KENSAKU.Size = New System.Drawing.Size(252, 18)
        Me.chk_KENSAKU.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.chk_KENSAKU)

        ' ラベル25
        Me.ラベル25.Name = "ラベル25"
        Me.ラベル25.Location = New System.Drawing.Point(90, 124)
        Me.ラベル25.Size = New System.Drawing.Size(234, 15)
        Me.ラベル25.Text = "月次仕訳計上フレックスの検索条件に従う"
        Me.chk_KENSAKU.Controls.Add(Me.ラベル25)

        Me.Name = "Frm振替伝票_SIJI"
        Me.Text = "振替伝票印刷"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970303808 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970305088 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970305856 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970440320 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_TAISYOKENSU As System.Windows.Forms.TextBox
    Friend WithEvents Lbl As System.Windows.Forms.Label
    Friend WithEvents txt_KENSAKUKENSU As System.Windows.Forms.TextBox
    Friend WithEvents ラベル0 As System.Windows.Forms.Label
    Friend WithEvents chk_EXCEL_OUT As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル23 As System.Windows.Forms.Label
    Friend WithEvents chk_KENSAKU As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル25 As System.Windows.Forms.Label

End Class
