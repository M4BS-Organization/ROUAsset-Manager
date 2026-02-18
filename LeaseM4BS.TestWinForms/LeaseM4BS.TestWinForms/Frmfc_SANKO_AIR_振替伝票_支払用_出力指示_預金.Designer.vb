<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_預金
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
        Me.unnamed_Label_1917970146304 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970141888 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917970140928 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917970149312 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.ラベル3 = New System.Windows.Forms.Label()
        Me.ラベル4 = New System.Windows.Forms.Label()
        Me.cmd_閉じる = New System.Windows.Forms.Button()
        Me.cmd_Del = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_現金預金科目CD = New System.Windows.Forms.TextBox()
        Me.txt_現金預金科目 = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.ラベル533 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_預金
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(343, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970146304)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970141888)
        Me.Controls.Add(Me.unnamed_TextBox_1917970140928)
        Me.Controls.Add(Me.unnamed_ComboBox_1917970149312)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970146304
        Me.unnamed_Label_1917970146304.Name = "unnamed_Label_1917970146304"
        Me.unnamed_Label_1917970146304.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970146304.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970141888
        Me.unnamed_CommandButton_1917970141888.Name = "unnamed_CommandButton_1917970141888"
        Me.unnamed_CommandButton_1917970141888.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970141888.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917970140928
        Me.unnamed_TextBox_1917970140928.Name = "unnamed_TextBox_1917970140928"
        Me.unnamed_TextBox_1917970140928.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970140928.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917970149312
        Me.unnamed_ComboBox_1917970149312.Name = "unnamed_ComboBox_1917970149312"
        Me.unnamed_ComboBox_1917970149312.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917970149312.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(343, 60)

        ' ラベル3
        Me.ラベル3.Name = "ラベル3"
        Me.ラベル3.Location = New System.Drawing.Point(3, 37)
        Me.ラベル3.Size = New System.Drawing.Size(94, 18)
        Me.ラベル3.Text = "預金科目CD"
        Me.pnlHeader.Controls.Add(Me.ラベル3)

        ' ラベル4
        Me.ラベル4.Name = "ラベル4"
        Me.ラベル4.Location = New System.Drawing.Point(98, 37)
        Me.ラベル4.Size = New System.Drawing.Size(242, 18)
        Me.ラベル4.Text = "預金科目"
        Me.pnlHeader.Controls.Add(Me.ラベル4)

        ' cmd_閉じる
        Me.cmd_閉じる.Name = "cmd_閉じる"
        Me.cmd_閉じる.Location = New System.Drawing.Point(3, 3)
        Me.cmd_閉じる.Size = New System.Drawing.Size(75, 26)
        Me.cmd_閉じる.Text = "閉じる(&C)"
        Me.pnlHeader.Controls.Add(Me.cmd_閉じる)

        ' cmd_Del
        Me.cmd_Del.Name = "cmd_Del"
        Me.cmd_Del.Location = New System.Drawing.Point(264, 3)
        Me.cmd_Del.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Del.Text = "行削除(&D)"
        Me.cmd_Del.TabIndex = 1
        Me.pnlHeader.Controls.Add(Me.cmd_Del)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' txt_現金預金科目CD
        Me.txt_現金預金科目CD.Name = "txt_現金預金科目CD"
        Me.txt_現金預金科目CD.Location = New System.Drawing.Point(3, 0)
        Me.txt_現金預金科目CD.Size = New System.Drawing.Size(94, 15)
        Me.pnlDetail.Controls.Add(Me.txt_現金預金科目CD)

        ' txt_現金預金科目
        Me.txt_現金預金科目.Name = "txt_現金預金科目"
        Me.txt_現金預金科目.Location = New System.Drawing.Point(98, 0)
        Me.txt_現金預金科目.Size = New System.Drawing.Size(242, 15)
        Me.txt_現金預金科目.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_現金預金科目)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(343, 37)

        ' ラベル533
        Me.ラベル533.Name = "ラベル533"
        Me.ラベル533.Location = New System.Drawing.Point(3, 3)
        Me.ラベル533.Size = New System.Drawing.Size(336, 25)
        Me.ラベル533.Text = "※ここで設定した内容はデータベースには反映されません。\015\012　（当該クライアントにのみ反映）"
        Me.pnlFooter.Controls.Add(Me.ラベル533)

        Me.Name = "Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_預金"
        Me.Text = "預金科目の登録"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970146304 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970141888 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917970140928 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917970149312 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents ラベル3 As System.Windows.Forms.Label
    Friend WithEvents ラベル4 As System.Windows.Forms.Label
    Friend WithEvents cmd_閉じる As System.Windows.Forms.Button
    Friend WithEvents cmd_Del As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents txt_現金預金科目CD As System.Windows.Forms.TextBox
    Friend WithEvents txt_現金預金科目 As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents ラベル533 As System.Windows.Forms.Label

End Class
