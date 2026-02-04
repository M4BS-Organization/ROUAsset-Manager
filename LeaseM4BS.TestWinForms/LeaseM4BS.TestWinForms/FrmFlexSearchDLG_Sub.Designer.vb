<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexSearchDLG_Sub
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
        Me.unnamed_Label_1917970513728 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977310656 = New System.Windows.Forms.Button()
        Me.unnamed_OptionButton_1917970509888 = New System.Windows.Forms.RadioButton()
        Me.unnamed_CheckBox_1917970504832 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970511168 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917970498560 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917970507264 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lbl_2 = New System.Windows.Forms.Label()
        Me.lbl_3 = New System.Windows.Forms.Label()
        Me.lbl_4 = New System.Windows.Forms.Label()
        Me.lbl_1 = New System.Windows.Forms.Label()
        Me.cmd_Del = New System.Windows.Forms.Button()
        Me.cmd_UP = New System.Windows.Forms.Button()
        Me.cmd_DOWN = New System.Windows.Forms.Button()
        Me.cmd_Henkou = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_FldNmJPN = New System.Windows.Forms.TextBox()
        Me.txt_Jouken = New System.Windows.Forms.TextBox()
        Me.txt_Ketugou = New System.Windows.Forms.TextBox()
        Me.txt_SearchNo = New System.Windows.Forms.TextBox()
        Me.txt_FldNo = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' FrmFlexSearchDLG_Sub
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(446, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970513728)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977310656)
        Me.Controls.Add(Me.unnamed_OptionButton_1917970509888)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970504832)
        Me.Controls.Add(Me.unnamed_TextBox_1917970511168)
        Me.Controls.Add(Me.unnamed_ListBox_1917970498560)
        Me.Controls.Add(Me.unnamed_ComboBox_1917970507264)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970513728
        Me.unnamed_Label_1917970513728.Name = "unnamed_Label_1917970513728"
        Me.unnamed_Label_1917970513728.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970513728.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977310656
        Me.unnamed_CommandButton_1917977310656.Name = "unnamed_CommandButton_1917977310656"
        Me.unnamed_CommandButton_1917977310656.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977310656.Size = New System.Drawing.Size(113, 18)

        ' unnamed_OptionButton_1917970509888
        Me.unnamed_OptionButton_1917970509888.Name = "unnamed_OptionButton_1917970509888"
        Me.unnamed_OptionButton_1917970509888.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionButton_1917970509888.Size = New System.Drawing.Size(12, 12)

        ' unnamed_CheckBox_1917970504832
        Me.unnamed_CheckBox_1917970504832.Name = "unnamed_CheckBox_1917970504832"
        Me.unnamed_CheckBox_1917970504832.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970504832.Size = New System.Drawing.Size(12, 12)

        ' unnamed_TextBox_1917970511168
        Me.unnamed_TextBox_1917970511168.Name = "unnamed_TextBox_1917970511168"
        Me.unnamed_TextBox_1917970511168.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970511168.Size = New System.Drawing.Size(84, 15)

        ' unnamed_ListBox_1917970498560
        Me.unnamed_ListBox_1917970498560.Name = "unnamed_ListBox_1917970498560"
        Me.unnamed_ListBox_1917970498560.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917970498560.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917970507264
        Me.unnamed_ComboBox_1917970507264.Name = "unnamed_ComboBox_1917970507264"
        Me.unnamed_ComboBox_1917970507264.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917970507264.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(446, 45)

        ' lbl_2
        Me.lbl_2.Name = "lbl_2"
        Me.lbl_2.Location = New System.Drawing.Point(3, 26)
        Me.lbl_2.Size = New System.Drawing.Size(136, 18)
        Me.lbl_2.Text = "項目名"
        Me.pnlHeader.Controls.Add(Me.lbl_2)

        ' lbl_3
        Me.lbl_3.Name = "lbl_3"
        Me.lbl_3.Location = New System.Drawing.Point(139, 26)
        Me.lbl_3.Size = New System.Drawing.Size(241, 18)
        Me.lbl_3.Text = "条件"
        Me.pnlHeader.Controls.Add(Me.lbl_3)

        ' lbl_4
        Me.lbl_4.Name = "lbl_4"
        Me.lbl_4.Location = New System.Drawing.Point(381, 26)
        Me.lbl_4.Size = New System.Drawing.Size(60, 18)
        Me.lbl_4.Text = "結合"
        Me.pnlHeader.Controls.Add(Me.lbl_4)

        ' lbl_1
        Me.lbl_1.Name = "lbl_1"
        Me.lbl_1.Location = New System.Drawing.Point(3, 0)
        Me.lbl_1.Size = New System.Drawing.Size(438, 26)
        Me.lbl_1.Text = "実行リスト"
        Me.pnlHeader.Controls.Add(Me.lbl_1)

        ' cmd_Del
        Me.cmd_Del.Name = "cmd_Del"
        Me.cmd_Del.Location = New System.Drawing.Point(377, 3)
        Me.cmd_Del.Size = New System.Drawing.Size(64, 18)
        Me.cmd_Del.Text = "行削除(&D)"
        Me.cmd_Del.TabIndex = 1
        Me.pnlHeader.Controls.Add(Me.cmd_Del)

        ' cmd_UP
        Me.cmd_UP.Name = "cmd_UP"
        Me.cmd_UP.Location = New System.Drawing.Point(260, 3)
        Me.cmd_UP.Size = New System.Drawing.Size(22, 18)
        Me.cmd_UP.Text = "▲"
        Me.cmd_UP.TabIndex = 2
        Me.pnlHeader.Controls.Add(Me.cmd_UP)

        ' cmd_DOWN
        Me.cmd_DOWN.Name = "cmd_DOWN"
        Me.cmd_DOWN.Location = New System.Drawing.Point(283, 3)
        Me.cmd_DOWN.Size = New System.Drawing.Size(22, 18)
        Me.cmd_DOWN.Text = "▼"
        Me.cmd_DOWN.TabIndex = 3
        Me.pnlHeader.Controls.Add(Me.cmd_DOWN)

        ' cmd_Henkou
        Me.cmd_Henkou.Name = "cmd_Henkou"
        Me.cmd_Henkou.Location = New System.Drawing.Point(313, 3)
        Me.cmd_Henkou.Size = New System.Drawing.Size(64, 18)
        Me.cmd_Henkou.Text = "変更(&U)"
        Me.pnlHeader.Controls.Add(Me.cmd_Henkou)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' txt_FldNmJPN
        Me.txt_FldNmJPN.Name = "txt_FldNmJPN"
        Me.txt_FldNmJPN.Location = New System.Drawing.Point(3, 0)
        Me.txt_FldNmJPN.Size = New System.Drawing.Size(136, 15)
        Me.pnlDetail.Controls.Add(Me.txt_FldNmJPN)

        ' txt_Jouken
        Me.txt_Jouken.Name = "txt_Jouken"
        Me.txt_Jouken.Location = New System.Drawing.Point(139, 0)
        Me.txt_Jouken.Size = New System.Drawing.Size(241, 15)
        Me.txt_Jouken.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_Jouken)

        ' txt_Ketugou
        Me.txt_Ketugou.Name = "txt_Ketugou"
        Me.txt_Ketugou.Location = New System.Drawing.Point(381, 0)
        Me.txt_Ketugou.Size = New System.Drawing.Size(60, 15)
        Me.txt_Ketugou.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_Ketugou)

        ' txt_SearchNo
        Me.txt_SearchNo.Name = "txt_SearchNo"
        Me.txt_SearchNo.Location = New System.Drawing.Point(56, 0)
        Me.txt_SearchNo.Size = New System.Drawing.Size(41, 15)
        Me.txt_SearchNo.Visible = False
        Me.txt_SearchNo.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_SearchNo)

        ' txt_FldNo
        Me.txt_FldNo.Name = "txt_FldNo"
        Me.txt_FldNo.Location = New System.Drawing.Point(204, 0)
        Me.txt_FldNo.Size = New System.Drawing.Size(41, 15)
        Me.txt_FldNo.Visible = False
        Me.txt_FldNo.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_FldNo)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(446, 0)

        Me.Name = "FrmFlexSearchDLG_Sub"
        Me.Text = "検索実行ﾘｽﾄ"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970513728 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977310656 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionButton_1917970509888 As System.Windows.Forms.RadioButton
    Friend WithEvents unnamed_CheckBox_1917970504832 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970511168 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917970498560 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917970507264 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lbl_2 As System.Windows.Forms.Label
    Friend WithEvents lbl_3 As System.Windows.Forms.Label
    Friend WithEvents lbl_4 As System.Windows.Forms.Label
    Friend WithEvents lbl_1 As System.Windows.Forms.Label
    Friend WithEvents cmd_Del As System.Windows.Forms.Button
    Friend WithEvents cmd_UP As System.Windows.Forms.Button
    Friend WithEvents cmd_DOWN As System.Windows.Forms.Button
    Friend WithEvents cmd_Henkou As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents txt_FldNmJPN As System.Windows.Forms.TextBox
    Friend WithEvents txt_Jouken As System.Windows.Forms.TextBox
    Friend WithEvents txt_Ketugou As System.Windows.Forms.TextBox
    Friend WithEvents txt_SearchNo As System.Windows.Forms.TextBox
    Friend WithEvents txt_FldNo As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
