<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_支払仕訳_KITOKU_SUB
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
        Me.unnamed_Label_1917970495552 = New System.Windows.Forms.Label()
        Me.unnamed_Rectangle_1917970496768 = New System.Windows.Forms.Panel()
        Me.unnamed_CommandButton_1917978166272 = New System.Windows.Forms.Button()
        Me.unnamed_OptionButton_1917978167936 = New System.Windows.Forms.RadioButton()
        Me.unnamed_CheckBox_1917978168512 = New System.Windows.Forms.CheckBox()
        Me.unnamed_OptionGroup_1917978168832 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917978170304 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917978171776 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.テキスト48 = New System.Windows.Forms.Label()
        Me.テキスト63 = New System.Windows.Forms.Label()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_ZRITU = New System.Windows.Forms.TextBox()
        Me.txt_ZEI_CD = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' Frmfc_支払仕訳_KITOKU_SUB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(170, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970495552)
        Me.Controls.Add(Me.unnamed_Rectangle_1917970496768)
        Me.Controls.Add(Me.unnamed_CommandButton_1917978166272)
        Me.Controls.Add(Me.unnamed_OptionButton_1917978167936)
        Me.Controls.Add(Me.unnamed_CheckBox_1917978168512)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917978168832)
        Me.Controls.Add(Me.unnamed_TextBox_1917978170304)
        Me.Controls.Add(Me.unnamed_ComboBox_1917978171776)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970495552
        Me.unnamed_Label_1917970495552.Name = "unnamed_Label_1917970495552"
        Me.unnamed_Label_1917970495552.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970495552.Size = New System.Drawing.Size(133, 26)

        ' unnamed_Rectangle_1917970496768
        Me.unnamed_Rectangle_1917970496768.Name = "unnamed_Rectangle_1917970496768"
        Me.unnamed_Rectangle_1917970496768.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Rectangle_1917970496768.Size = New System.Drawing.Size(56, 56)

        ' unnamed_CommandButton_1917978166272
        Me.unnamed_CommandButton_1917978166272.Name = "unnamed_CommandButton_1917978166272"
        Me.unnamed_CommandButton_1917978166272.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917978166272.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionButton_1917978167936
        Me.unnamed_OptionButton_1917978167936.Name = "unnamed_OptionButton_1917978167936"
        Me.unnamed_OptionButton_1917978167936.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionButton_1917978167936.Size = New System.Drawing.Size(13, 13)

        ' unnamed_CheckBox_1917978168512
        Me.unnamed_CheckBox_1917978168512.Name = "unnamed_CheckBox_1917978168512"
        Me.unnamed_CheckBox_1917978168512.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917978168512.Size = New System.Drawing.Size(13, 13)

        ' unnamed_OptionGroup_1917978168832
        Me.unnamed_OptionGroup_1917978168832.Name = "unnamed_OptionGroup_1917978168832"
        Me.unnamed_OptionGroup_1917978168832.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917978168832.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917978170304
        Me.unnamed_TextBox_1917978170304.Name = "unnamed_TextBox_1917978170304"
        Me.unnamed_TextBox_1917978170304.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917978170304.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917978171776
        Me.unnamed_ComboBox_1917978171776.Name = "unnamed_ComboBox_1917978171776"
        Me.unnamed_ComboBox_1917978171776.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917978171776.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(170, 15)

        ' テキスト48
        Me.テキスト48.Name = "テキスト48"
        Me.テキスト48.Location = New System.Drawing.Point(3, 0)
        Me.テキスト48.Size = New System.Drawing.Size(83, 15)
        Me.テキスト48.Text = "消費税率"
        Me.pnlHeader.Controls.Add(Me.テキスト48)

        ' テキスト63
        Me.テキスト63.Name = "テキスト63"
        Me.テキスト63.Location = New System.Drawing.Point(86, 0)
        Me.テキスト63.Size = New System.Drawing.Size(83, 15)
        Me.テキスト63.Text = "税処理ｺｰﾄﾞ"
        Me.pnlHeader.Controls.Add(Me.テキスト63)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' txt_ZRITU
        Me.txt_ZRITU.Name = "txt_ZRITU"
        Me.txt_ZRITU.Location = New System.Drawing.Point(3, 0)
        Me.txt_ZRITU.Size = New System.Drawing.Size(83, 15)
        Me.pnlDetail.Controls.Add(Me.txt_ZRITU)

        ' txt_ZEI_CD
        Me.txt_ZEI_CD.Name = "txt_ZEI_CD"
        Me.txt_ZEI_CD.Location = New System.Drawing.Point(86, 0)
        Me.txt_ZEI_CD.Size = New System.Drawing.Size(83, 15)
        Me.txt_ZEI_CD.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_ZEI_CD)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(170, 0)

        Me.Name = "Frmfc_支払仕訳_KITOKU_SUB"
        Me.Text = "Frmfc_支払仕訳_KITOKU_SUB"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970495552 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Rectangle_1917970496768 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_CommandButton_1917978166272 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionButton_1917978167936 As System.Windows.Forms.RadioButton
    Friend WithEvents unnamed_CheckBox_1917978168512 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_OptionGroup_1917978168832 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917978170304 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917978171776 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents テキスト48 As System.Windows.Forms.Label
    Friend WithEvents テキスト63 As System.Windows.Forms.Label
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents txt_ZRITU As System.Windows.Forms.TextBox
    Friend WithEvents txt_ZEI_CD As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
