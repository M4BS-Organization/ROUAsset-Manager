<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_修正
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
        Me.unnamed_Label_1917977301504 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977312704 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917977301888 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977302080 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.ラベル1 = New System.Windows.Forms.Label()
        Me.ラベル3 = New System.Windows.Forms.Label()
        Me.ラベル4 = New System.Windows.Forms.Label()
        Me.ラベル9 = New System.Windows.Forms.Label()
        Me.ラベル11 = New System.Windows.Forms.Label()
        Me.ラベル13 = New System.Windows.Forms.Label()
        Me.ラベル15 = New System.Windows.Forms.Label()
        Me.ラベル17 = New System.Windows.Forms.Label()
        Me.ラベル19 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.ラベル544 = New System.Windows.Forms.Label()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_契約管理単位 = New System.Windows.Forms.TextBox()
        Me.txt_貸方科目 = New System.Windows.Forms.TextBox()
        Me.cmb_貸方科目CD = New System.Windows.Forms.ComboBox()
        Me.txt_支払先 = New System.Windows.Forms.TextBox()
        Me.txt_契約番号 = New System.Windows.Forms.TextBox()
        Me.txt_物件No = New System.Windows.Forms.TextBox()
        Me.txt_約定支払日 = New System.Windows.Forms.TextBox()
        Me.txt_実際支払日 = New System.Windows.Forms.TextBox()
        Me.txt_金額 = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.テキスト21 = New System.Windows.Forms.TextBox()
        Me.ラベル22 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_修正
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(873, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977301504)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977312704)
        Me.Controls.Add(Me.unnamed_TextBox_1917977301888)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977302080)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917977301504
        Me.unnamed_Label_1917977301504.Name = "unnamed_Label_1917977301504"
        Me.unnamed_Label_1917977301504.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977301504.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977312704
        Me.unnamed_CommandButton_1917977312704.Name = "unnamed_CommandButton_1917977312704"
        Me.unnamed_CommandButton_1917977312704.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977312704.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917977301888
        Me.unnamed_TextBox_1917977301888.Name = "unnamed_TextBox_1917977301888"
        Me.unnamed_TextBox_1917977301888.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977301888.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977302080
        Me.unnamed_ComboBox_1917977302080.Name = "unnamed_ComboBox_1917977302080"
        Me.unnamed_ComboBox_1917977302080.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977302080.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(873, 98)

        ' ラベル1
        Me.ラベル1.Name = "ラベル1"
        Me.ラベル1.Location = New System.Drawing.Point(3, 64)
        Me.ラベル1.Size = New System.Drawing.Size(113, 30)
        Me.ラベル1.Text = "契約管理単位"
        Me.pnlHeader.Controls.Add(Me.ラベル1)

        ' ラベル3
        Me.ラベル3.Name = "ラベル3"
        Me.ラベル3.Location = New System.Drawing.Point(529, 64)
        Me.ラベル3.Size = New System.Drawing.Size(94, 30)
        Me.ラベル3.Text = "預金科目\015\012CD"
        Me.pnlHeader.Controls.Add(Me.ラベル3)

        ' ラベル4
        Me.ラベル4.Name = "ラベル4"
        Me.ラベル4.Location = New System.Drawing.Point(623, 64)
        Me.ラベル4.Size = New System.Drawing.Size(151, 30)
        Me.ラベル4.Text = "預金科目"
        Me.pnlHeader.Controls.Add(Me.ラベル4)

        ' ラベル9
        Me.ラベル9.Name = "ラベル9"
        Me.ラベル9.Location = New System.Drawing.Point(283, 64)
        Me.ラベル9.Size = New System.Drawing.Size(113, 30)
        Me.ラベル9.Text = "支払先"
        Me.pnlHeader.Controls.Add(Me.ラベル9)

        ' ラベル11
        Me.ラベル11.Name = "ラベル11"
        Me.ラベル11.Location = New System.Drawing.Point(396, 64)
        Me.ラベル11.Size = New System.Drawing.Size(94, 30)
        Me.ラベル11.Text = "契約番号"
        Me.pnlHeader.Controls.Add(Me.ラベル11)

        ' ラベル13
        Me.ラベル13.Name = "ラベル13"
        Me.ラベル13.Location = New System.Drawing.Point(491, 64)
        Me.ラベル13.Size = New System.Drawing.Size(37, 30)
        Me.ラベル13.Text = "物件\015\012No"
        Me.pnlHeader.Controls.Add(Me.ラベル13)

        ' ラベル15
        Me.ラベル15.Name = "ラベル15"
        Me.ラベル15.Location = New System.Drawing.Point(117, 64)
        Me.ラベル15.Size = New System.Drawing.Size(83, 30)
        Me.ラベル15.Text = "約定支払日"
        Me.pnlHeader.Controls.Add(Me.ラベル15)

        ' ラベル17
        Me.ラベル17.Name = "ラベル17"
        Me.ラベル17.Location = New System.Drawing.Point(200, 64)
        Me.ラベル17.Size = New System.Drawing.Size(83, 30)
        Me.ラベル17.Text = "実際支払日\015\012(計上処理日)"
        Me.pnlHeader.Controls.Add(Me.ラベル17)

        ' ラベル19
        Me.ラベル19.Name = "ラベル19"
        Me.ラベル19.Location = New System.Drawing.Point(774, 64)
        Me.ラベル19.Size = New System.Drawing.Size(94, 30)
        Me.ラベル19.Text = "金額"
        Me.pnlHeader.Controls.Add(Me.ラベル19)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(3, 3)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.pnlHeader.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(86, 3)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlHeader.Controls.Add(Me.cmd_CANCEL)

        ' ラベル544
        Me.ラベル544.Name = "ラベル544"
        Me.ラベル544.Location = New System.Drawing.Point(3, 37)
        Me.ラベル544.Size = New System.Drawing.Size(453, 15)
        Me.ラベル544.Text = "引落明細（支払方法＝引落　のデータ明細）の実際支払日および預金科目の修正"
        Me.pnlHeader.Controls.Add(Me.ラベル544)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' txt_契約管理単位
        Me.txt_契約管理単位.Name = "txt_契約管理単位"
        Me.txt_契約管理単位.Location = New System.Drawing.Point(3, 0)
        Me.txt_契約管理単位.Size = New System.Drawing.Size(133, 15)
        Me.pnlDetail.Controls.Add(Me.txt_契約管理単位)

        ' txt_貸方科目
        Me.txt_貸方科目.Name = "txt_貸方科目"
        Me.txt_貸方科目.Location = New System.Drawing.Point(623, 0)
        Me.txt_貸方科目.Size = New System.Drawing.Size(151, 15)
        Me.txt_貸方科目.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.txt_貸方科目)

        ' cmb_貸方科目CD
        Me.cmb_貸方科目CD.Name = "cmb_貸方科目CD"
        Me.cmb_貸方科目CD.Location = New System.Drawing.Point(529, 0)
        Me.cmb_貸方科目CD.Size = New System.Drawing.Size(94, 15)
        Me.cmb_貸方科目CD.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.cmb_貸方科目CD)

        ' txt_支払先
        Me.txt_支払先.Name = "txt_支払先"
        Me.txt_支払先.Location = New System.Drawing.Point(283, 0)
        Me.txt_支払先.Size = New System.Drawing.Size(133, 15)
        Me.txt_支払先.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_支払先)

        ' txt_契約番号
        Me.txt_契約番号.Name = "txt_契約番号"
        Me.txt_契約番号.Location = New System.Drawing.Point(396, 0)
        Me.txt_契約番号.Size = New System.Drawing.Size(94, 15)
        Me.txt_契約番号.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_契約番号)

        ' txt_物件No
        Me.txt_物件No.Name = "txt_物件No"
        Me.txt_物件No.Location = New System.Drawing.Point(491, 0)
        Me.txt_物件No.Size = New System.Drawing.Size(37, 15)
        Me.txt_物件No.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_物件No)

        ' txt_約定支払日
        Me.txt_約定支払日.Name = "txt_約定支払日"
        Me.txt_約定支払日.Location = New System.Drawing.Point(117, 0)
        Me.txt_約定支払日.Size = New System.Drawing.Size(83, 15)
        Me.txt_約定支払日.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_約定支払日)

        ' txt_実際支払日
        Me.txt_実際支払日.Name = "txt_実際支払日"
        Me.txt_実際支払日.Location = New System.Drawing.Point(200, 0)
        Me.txt_実際支払日.Size = New System.Drawing.Size(83, 15)
        Me.txt_実際支払日.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_実際支払日)

        ' txt_金額
        Me.txt_金額.Name = "txt_金額"
        Me.txt_金額.Location = New System.Drawing.Point(774, 0)
        Me.txt_金額.Size = New System.Drawing.Size(94, 15)
        Me.txt_金額.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_金額)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(873, 26)

        ' テキスト21
        Me.テキスト21.Name = "テキスト21"
        Me.テキスト21.Location = New System.Drawing.Point(774, 3)
        Me.テキスト21.Size = New System.Drawing.Size(94, 15)
        Me.pnlFooter.Controls.Add(Me.テキスト21)

        ' ラベル22
        Me.ラベル22.Name = "ラベル22"
        Me.ラベル22.Location = New System.Drawing.Point(695, 3)
        Me.ラベル22.Size = New System.Drawing.Size(75, 15)
        Me.ラベル22.Text = "合計"
        Me.pnlFooter.Controls.Add(Me.ラベル22)

        Me.Name = "Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_修正"
        Me.Text = "振替伝票（支払用）出力指示　－　修正"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977301504 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977312704 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917977301888 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977302080 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents ラベル1 As System.Windows.Forms.Label
    Friend WithEvents ラベル3 As System.Windows.Forms.Label
    Friend WithEvents ラベル4 As System.Windows.Forms.Label
    Friend WithEvents ラベル9 As System.Windows.Forms.Label
    Friend WithEvents ラベル11 As System.Windows.Forms.Label
    Friend WithEvents ラベル13 As System.Windows.Forms.Label
    Friend WithEvents ラベル15 As System.Windows.Forms.Label
    Friend WithEvents ラベル17 As System.Windows.Forms.Label
    Friend WithEvents ラベル19 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents ラベル544 As System.Windows.Forms.Label
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents txt_契約管理単位 As System.Windows.Forms.TextBox
    Friend WithEvents txt_貸方科目 As System.Windows.Forms.TextBox
    Friend WithEvents cmb_貸方科目CD As System.Windows.Forms.ComboBox
    Friend WithEvents txt_支払先 As System.Windows.Forms.TextBox
    Friend WithEvents txt_契約番号 As System.Windows.Forms.TextBox
    Friend WithEvents txt_物件No As System.Windows.Forms.TextBox
    Friend WithEvents txt_約定支払日 As System.Windows.Forms.TextBox
    Friend WithEvents txt_実際支払日 As System.Windows.Forms.TextBox
    Friend WithEvents txt_金額 As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents テキスト21 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル22 As System.Windows.Forms.Label

End Class
