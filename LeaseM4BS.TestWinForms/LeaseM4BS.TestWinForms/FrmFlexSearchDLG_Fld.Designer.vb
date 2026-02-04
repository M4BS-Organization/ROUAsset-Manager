<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexSearchDLG_Fld
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
        Me.unnamed_Label_1917977218240 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977224064 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977226752 = New System.Windows.Forms.CheckBox()
        Me.unnamed_OptionGroup_1917977229312 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917977134976 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977230656 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.テキスト48 = New System.Windows.Forms.Label()
        Me.cmd_Go = New System.Windows.Forms.Button()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.cmd_Clear = New System.Windows.Forms.Button()
        Me.opg_Ketugou = New System.Windows.Forms.GroupBox()
        Me.chk_NotFlag = New System.Windows.Forms.CheckBox()
        Me.ラベル66 = New System.Windows.Forms.Label()
        Me.txt_From = New System.Windows.Forms.TextBox()
        Me.txt_To = New System.Windows.Forms.TextBox()
        Me.lbl__ = New System.Windows.Forms.Label()
        Me.txt_String = New System.Windows.Forms.TextBox()
        Me.cmb_Val = New System.Windows.Forms.ComboBox()
        Me.txt_VALNM = New System.Windows.Forms.TextBox()
        Me.txt_FldNmJPN = New System.Windows.Forms.TextBox()
        Me.txt_Ketugou = New System.Windows.Forms.TextBox()
        Me.txt_SearchNo = New System.Windows.Forms.TextBox()
        Me.txt_Jouken = New System.Windows.Forms.TextBox()
        Me.txt_NoInputFlag = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' FrmFlexSearchDLG_Fld
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977218240)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977224064)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977226752)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917977229312)
        Me.Controls.Add(Me.unnamed_TextBox_1917977134976)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977230656)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977218240
        Me.unnamed_Label_1917977218240.Name = "unnamed_Label_1917977218240"
        Me.unnamed_Label_1917977218240.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977218240.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977224064
        Me.unnamed_CommandButton_1917977224064.Name = "unnamed_CommandButton_1917977224064"
        Me.unnamed_CommandButton_1917977224064.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977224064.Size = New System.Drawing.Size(113, 18)

        ' unnamed_CheckBox_1917977226752
        Me.unnamed_CheckBox_1917977226752.Name = "unnamed_CheckBox_1917977226752"
        Me.unnamed_CheckBox_1917977226752.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977226752.Size = New System.Drawing.Size(133, 26)

        ' unnamed_OptionGroup_1917977229312
        Me.unnamed_OptionGroup_1917977229312.Name = "unnamed_OptionGroup_1917977229312"
        Me.unnamed_OptionGroup_1917977229312.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917977229312.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917977134976
        Me.unnamed_TextBox_1917977134976.Name = "unnamed_TextBox_1917977134976"
        Me.unnamed_TextBox_1917977134976.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977134976.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977230656
        Me.unnamed_ComboBox_1917977230656.Name = "unnamed_ComboBox_1917977230656"
        Me.unnamed_ComboBox_1917977230656.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977230656.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' テキスト48
        Me.テキスト48.Name = "テキスト48"
        Me.テキスト48.Location = New System.Drawing.Point(215, 105)
        Me.テキスト48.Size = New System.Drawing.Size(75, 45)
        Me.テキスト48.Text = "結合"
        Me.pnlDetail.Controls.Add(Me.テキスト48)

        ' cmd_Go
        Me.cmd_Go.Name = "cmd_Go"
        Me.cmd_Go.Location = New System.Drawing.Point(7, 3)
        Me.cmd_Go.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Go.Text = "実行(&R)"
        Me.cmd_Go.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.cmd_Go)

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(162, 3)
        Me.cmd_Cancel.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Cancel.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_Cancel.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' cmd_Clear
        Me.cmd_Clear.Name = "cmd_Clear"
        Me.cmd_Clear.Location = New System.Drawing.Point(83, 3)
        Me.cmd_Clear.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Clear.Text = "未入力(&L)"
        Me.cmd_Clear.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmd_Clear)

        ' opg_Ketugou
        Me.opg_Ketugou.Name = "opg_Ketugou"
        Me.opg_Ketugou.Location = New System.Drawing.Point(291, 105)
        Me.opg_Ketugou.Size = New System.Drawing.Size(189, 52)
        Me.opg_Ketugou.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.opg_Ketugou)

        ' chk_NotFlag
        Me.chk_NotFlag.Name = "chk_NotFlag"
        Me.chk_NotFlag.Location = New System.Drawing.Point(529, 60)
        Me.chk_NotFlag.Size = New System.Drawing.Size(30, 22)
        Me.chk_NotFlag.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.chk_NotFlag)

        ' ラベル66
        Me.ラベル66.Name = "ラベル66"
        Me.ラベル66.Location = New System.Drawing.Point(514, 37)
        Me.ラベル66.Size = New System.Drawing.Size(49, 15)
        Me.ラベル66.Text = "以外(&N)"
        Me.chk_NotFlag.Controls.Add(Me.ラベル66)

        ' txt_From
        Me.txt_From.Name = "txt_From"
        Me.txt_From.Location = New System.Drawing.Point(188, 56)
        Me.txt_From.Size = New System.Drawing.Size(151, 18)
        Me.txt_From.Visible = False
        Me.pnlDetail.Controls.Add(Me.txt_From)

        ' txt_To
        Me.txt_To.Name = "txt_To"
        Me.txt_To.Location = New System.Drawing.Point(359, 56)
        Me.txt_To.Size = New System.Drawing.Size(151, 18)
        Me.txt_To.Visible = False
        Me.txt_To.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_To)

        ' lbl__
        Me.lbl__.Name = "lbl__"
        Me.lbl__.Location = New System.Drawing.Point(340, 56)
        Me.lbl__.Size = New System.Drawing.Size(18, 18)
        Me.lbl__.Text = "～"
        Me.lbl__.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl__)

        ' txt_String
        Me.txt_String.Name = "txt_String"
        Me.txt_String.Location = New System.Drawing.Point(188, 56)
        Me.txt_String.Size = New System.Drawing.Size(321, 18)
        Me.txt_String.Visible = False
        Me.txt_String.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_String)

        ' cmb_Val
        Me.cmb_Val.Name = "cmb_Val"
        Me.cmb_Val.Location = New System.Drawing.Point(188, 56)
        Me.cmb_Val.Size = New System.Drawing.Size(321, 18)
        Me.cmb_Val.Visible = False
        Me.cmb_Val.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmb_Val)

        ' txt_VALNM
        Me.txt_VALNM.Name = "txt_VALNM"
        Me.txt_VALNM.Location = New System.Drawing.Point(188, 75)
        Me.txt_VALNM.Size = New System.Drawing.Size(321, 18)
        Me.txt_VALNM.Visible = False
        Me.txt_VALNM.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_VALNM)

        ' txt_FldNmJPN
        Me.txt_FldNmJPN.Name = "txt_FldNmJPN"
        Me.txt_FldNmJPN.Location = New System.Drawing.Point(7, 56)
        Me.txt_FldNmJPN.Size = New System.Drawing.Size(181, 18)
        Me.txt_FldNmJPN.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_FldNmJPN)

        ' txt_Ketugou
        Me.txt_Ketugou.Name = "txt_Ketugou"
        Me.txt_Ketugou.Location = New System.Drawing.Point(491, 113)
        Me.txt_Ketugou.Size = New System.Drawing.Size(75, 18)
        Me.txt_Ketugou.Visible = False
        Me.txt_Ketugou.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_Ketugou)

        ' txt_SearchNo
        Me.txt_SearchNo.Name = "txt_SearchNo"
        Me.txt_SearchNo.Location = New System.Drawing.Point(37, 94)
        Me.txt_SearchNo.Size = New System.Drawing.Size(75, 18)
        Me.txt_SearchNo.Visible = False
        Me.txt_SearchNo.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.txt_SearchNo)

        ' txt_Jouken
        Me.txt_Jouken.Name = "txt_Jouken"
        Me.txt_Jouken.Location = New System.Drawing.Point(37, 113)
        Me.txt_Jouken.Size = New System.Drawing.Size(75, 18)
        Me.txt_Jouken.Visible = False
        Me.txt_Jouken.TabIndex = 13
        Me.pnlDetail.Controls.Add(Me.txt_Jouken)

        ' txt_NoInputFlag
        Me.txt_NoInputFlag.Name = "txt_NoInputFlag"
        Me.txt_NoInputFlag.Location = New System.Drawing.Point(37, 139)
        Me.txt_NoInputFlag.Size = New System.Drawing.Size(75, 18)
        Me.txt_NoInputFlag.Visible = False
        Me.txt_NoInputFlag.TabIndex = 14
        Me.pnlDetail.Controls.Add(Me.txt_NoInputFlag)

        Me.Name = "FrmFlexSearchDLG_Fld"
        Me.Text = "FrmFlexSearchDLG_Fld"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977218240 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977224064 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977226752 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_OptionGroup_1917977229312 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917977134976 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977230656 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents テキスト48 As System.Windows.Forms.Label
    Friend WithEvents cmd_Go As System.Windows.Forms.Button
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents cmd_Clear As System.Windows.Forms.Button
    Friend WithEvents opg_Ketugou As System.Windows.Forms.GroupBox
    Friend WithEvents chk_NotFlag As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル66 As System.Windows.Forms.Label
    Friend WithEvents txt_From As System.Windows.Forms.TextBox
    Friend WithEvents txt_To As System.Windows.Forms.TextBox
    Friend WithEvents lbl__ As System.Windows.Forms.Label
    Friend WithEvents txt_String As System.Windows.Forms.TextBox
    Friend WithEvents cmb_Val As System.Windows.Forms.ComboBox
    Friend WithEvents txt_VALNM As System.Windows.Forms.TextBox
    Friend WithEvents txt_FldNmJPN As System.Windows.Forms.TextBox
    Friend WithEvents txt_Ketugou As System.Windows.Forms.TextBox
    Friend WithEvents txt_SearchNo As System.Windows.Forms.TextBox
    Friend WithEvents txt_Jouken As System.Windows.Forms.TextBox
    Friend WithEvents txt_NoInputFlag As System.Windows.Forms.TextBox

End Class
