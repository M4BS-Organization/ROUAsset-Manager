<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_SUB
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
        Me.unnamed_Label_1917970150592 = New System.Windows.Forms.Label()
        Me.unnamed_TextBox_1917970149056 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917970146944 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.ラベル0 = New System.Windows.Forms.Label()
        Me.ラベル1 = New System.Windows.Forms.Label()
        Me.ラベル3 = New System.Windows.Forms.Label()
        Me.ラベル4 = New System.Windows.Forms.Label()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.契約管理単位CD = New System.Windows.Forms.TextBox()
        Me.契約管理単位 = New System.Windows.Forms.TextBox()
        Me.txt_現金預金科目 = New System.Windows.Forms.TextBox()
        Me.cmb_現金預金科目CD = New System.Windows.Forms.ComboBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_SUB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970150592)
        Me.Controls.Add(Me.unnamed_TextBox_1917970149056)
        Me.Controls.Add(Me.unnamed_ComboBox_1917970146944)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970150592
        Me.unnamed_Label_1917970150592.Name = "unnamed_Label_1917970150592"
        Me.unnamed_Label_1917970150592.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970150592.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970149056
        Me.unnamed_TextBox_1917970149056.Name = "unnamed_TextBox_1917970149056"
        Me.unnamed_TextBox_1917970149056.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970149056.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917970146944
        Me.unnamed_ComboBox_1917970146944.Name = "unnamed_ComboBox_1917970146944"
        Me.unnamed_ComboBox_1917970146944.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917970146944.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(498, 18)

        ' ラベル0
        Me.ラベル0.Name = "ラベル0"
        Me.ラベル0.Location = New System.Drawing.Point(3, 0)
        Me.ラベル0.Size = New System.Drawing.Size(94, 18)
        Me.ラベル0.Text = "契約管理単位CD"
        Me.pnlHeader.Controls.Add(Me.ラベル0)

        ' ラベル1
        Me.ラベル1.Name = "ラベル1"
        Me.ラベル1.Location = New System.Drawing.Point(98, 0)
        Me.ラベル1.Size = New System.Drawing.Size(151, 18)
        Me.ラベル1.Text = "契約管理単位"
        Me.pnlHeader.Controls.Add(Me.ラベル1)

        ' ラベル3
        Me.ラベル3.Name = "ラベル3"
        Me.ラベル3.Location = New System.Drawing.Point(249, 0)
        Me.ラベル3.Size = New System.Drawing.Size(94, 18)
        Me.ラベル3.Text = "預金科目CD"
        Me.pnlHeader.Controls.Add(Me.ラベル3)

        ' ラベル4
        Me.ラベル4.Name = "ラベル4"
        Me.ラベル4.Location = New System.Drawing.Point(343, 0)
        Me.ラベル4.Size = New System.Drawing.Size(151, 18)
        Me.ラベル4.Text = "預金科目"
        Me.pnlHeader.Controls.Add(Me.ラベル4)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' 契約管理単位CD
        Me.契約管理単位CD.Name = "契約管理単位CD"
        Me.契約管理単位CD.Location = New System.Drawing.Point(3, 0)
        Me.契約管理単位CD.Size = New System.Drawing.Size(94, 15)
        Me.pnlDetail.Controls.Add(Me.契約管理単位CD)

        ' 契約管理単位
        Me.契約管理単位.Name = "契約管理単位"
        Me.契約管理単位.Location = New System.Drawing.Point(98, 0)
        Me.契約管理単位.Size = New System.Drawing.Size(151, 15)
        Me.契約管理単位.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.契約管理単位)

        ' txt_現金預金科目
        Me.txt_現金預金科目.Name = "txt_現金預金科目"
        Me.txt_現金預金科目.Location = New System.Drawing.Point(343, 0)
        Me.txt_現金預金科目.Size = New System.Drawing.Size(151, 15)
        Me.txt_現金預金科目.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_現金預金科目)

        ' cmb_現金預金科目CD
        Me.cmb_現金預金科目CD.Name = "cmb_現金預金科目CD"
        Me.cmb_現金預金科目CD.Location = New System.Drawing.Point(249, 0)
        Me.cmb_現金預金科目CD.Size = New System.Drawing.Size(94, 15)
        Me.cmb_現金預金科目CD.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmb_現金預金科目CD)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(498, 0)

        Me.Name = "Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_SUB"
        Me.Text = "Frmfc_SANKO_AIR_振替伝票_支払用_出力指示_SUB"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970150592 As System.Windows.Forms.Label
    Friend WithEvents unnamed_TextBox_1917970149056 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917970146944 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents ラベル0 As System.Windows.Forms.Label
    Friend WithEvents ラベル1 As System.Windows.Forms.Label
    Friend WithEvents ラベル3 As System.Windows.Forms.Label
    Friend WithEvents ラベル4 As System.Windows.Forms.Label
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents 契約管理単位CD As System.Windows.Forms.TextBox
    Friend WithEvents 契約管理単位 As System.Windows.Forms.TextBox
    Friend WithEvents txt_現金預金科目 As System.Windows.Forms.TextBox
    Friend WithEvents cmb_現金預金科目CD As System.Windows.Forms.ComboBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
