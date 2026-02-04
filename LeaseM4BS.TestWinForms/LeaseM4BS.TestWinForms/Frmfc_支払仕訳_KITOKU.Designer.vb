<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_支払仕訳_KITOKU
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
        Me.unnamed_Label_1917977709376 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977711232 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917977711168 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917977719104 = New System.Windows.Forms.ListBox()
        Me.unnamed_Subform_1917977709760 = New System.Windows.Forms.Panel()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_SLIP_DT = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_DT = New System.Windows.Forms.Label()
        Me.txt_SLIP_NO_START_VAL = New System.Windows.Forms.TextBox()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_SLIP_NO_START_VAL = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_CD = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_NM = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU_NM = New System.Windows.Forms.TextBox()
        Me.lbl_OUTPUT_FOLDER_NM = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FOLDER_NM = New System.Windows.Forms.TextBox()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.lbl_EXPLANATION3 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION4 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION5 = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FILE1_NM = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FILE2_NM = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FILE3_NM = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FILE4_NM = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FILE1_NM = New System.Windows.Forms.TextBox()
        Me.txt_OUTPUT_FILE2_NM = New System.Windows.Forms.TextBox()
        Me.txt_OUTPUT_FILE3_NM = New System.Windows.Forms.TextBox()
        Me.txt_OUTPUT_FILE4_NM = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU = New System.Windows.Forms.Label()
        Me.lbl_BSHO_CD = New System.Windows.Forms.Label()
        Me.txt_BSHO_CD = New System.Windows.Forms.TextBox()
        Me.sub_fc_支払仕訳_KITOKU_SUB = New System.Windows.Forms.Panel()
        Me.ラベル25 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_支払仕訳_KITOKU
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977709376)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977711232)
        Me.Controls.Add(Me.unnamed_TextBox_1917977711168)
        Me.Controls.Add(Me.unnamed_ListBox_1917977719104)
        Me.Controls.Add(Me.unnamed_Subform_1917977709760)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977709376
        Me.unnamed_Label_1917977709376.Name = "unnamed_Label_1917977709376"
        Me.unnamed_Label_1917977709376.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977709376.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977711232
        Me.unnamed_CommandButton_1917977711232.Name = "unnamed_CommandButton_1917977711232"
        Me.unnamed_CommandButton_1917977711232.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977711232.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917977711168
        Me.unnamed_TextBox_1917977711168.Name = "unnamed_TextBox_1917977711168"
        Me.unnamed_TextBox_1917977711168.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977711168.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917977719104
        Me.unnamed_ListBox_1917977719104.Name = "unnamed_ListBox_1917977719104"
        Me.unnamed_ListBox_1917977719104.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917977719104.Size = New System.Drawing.Size(113, 94)

        ' unnamed_Subform_1917977709760
        Me.unnamed_Subform_1917977709760.Name = "unnamed_Subform_1917977709760"
        Me.unnamed_Subform_1917977709760.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Subform_1917977709760.Size = New System.Drawing.Size(113, 113)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(136, 83)
        Me.lbl_EXPLANATION2.Size = New System.Drawing.Size(321, 18)
        Me.lbl_EXPLANATION2.Text = "   貸方が現預金の仕訳の伝票日付は実際支払日となります。"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION2)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(3, 3)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.cmd_実行.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(86, 3)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 13
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_SLIP_DT
        Me.txt_SLIP_DT.Name = "txt_SLIP_DT"
        Me.txt_SLIP_DT.Location = New System.Drawing.Point(136, 45)
        Me.txt_SLIP_DT.Size = New System.Drawing.Size(94, 18)
        Me.pnlDetail.Controls.Add(Me.txt_SLIP_DT)

        ' lbl_SLIP_DT
        Me.lbl_SLIP_DT.Name = "lbl_SLIP_DT"
        Me.lbl_SLIP_DT.Location = New System.Drawing.Point(26, 45)
        Me.lbl_SLIP_DT.Size = New System.Drawing.Size(109, 18)
        Me.lbl_SLIP_DT.Text = "伝票日付"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_DT)

        ' txt_SLIP_NO_START_VAL
        Me.txt_SLIP_NO_START_VAL.Name = "txt_SLIP_NO_START_VAL"
        Me.txt_SLIP_NO_START_VAL.Location = New System.Drawing.Point(136, 117)
        Me.txt_SLIP_NO_START_VAL.Size = New System.Drawing.Size(94, 18)
        Me.txt_SLIP_NO_START_VAL.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_SLIP_NO_START_VAL)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(136, 64)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(321, 18)
        Me.lbl_EXPLANATION1.Text = "※貸方が現預金でない仕訳の伝票日付を指定します。"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_SLIP_NO_START_VAL
        Me.lbl_SLIP_NO_START_VAL.Name = "lbl_SLIP_NO_START_VAL"
        Me.lbl_SLIP_NO_START_VAL.Location = New System.Drawing.Point(26, 117)
        Me.lbl_SLIP_NO_START_VAL.Size = New System.Drawing.Size(109, 18)
        Me.lbl_SLIP_NO_START_VAL.Text = "伝票番号開始値"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_NO_START_VAL)

        ' lbl_KAMOKU
        Me.lbl_KAMOKU.Name = "lbl_KAMOKU"
        Me.lbl_KAMOKU.Location = New System.Drawing.Point(26, 151)
        Me.lbl_KAMOKU.Size = New System.Drawing.Size(109, 18)
        Me.lbl_KAMOKU.Text = "科目"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU)

        ' lbl_KAMOKU_CD
        Me.lbl_KAMOKU_CD.Name = "lbl_KAMOKU_CD"
        Me.lbl_KAMOKU_CD.Location = New System.Drawing.Point(136, 151)
        Me.lbl_KAMOKU_CD.Size = New System.Drawing.Size(94, 18)
        Me.lbl_KAMOKU_CD.Text = "ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD)

        ' lbl_KAMOKU_NM
        Me.lbl_KAMOKU_NM.Name = "lbl_KAMOKU_NM"
        Me.lbl_KAMOKU_NM.Location = New System.Drawing.Point(230, 151)
        Me.lbl_KAMOKU_NM.Size = New System.Drawing.Size(185, 18)
        Me.lbl_KAMOKU_NM.Text = "名称"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_NM)

        ' txt_KAMOKU_CD
        Me.txt_KAMOKU_CD.Name = "txt_KAMOKU_CD"
        Me.txt_KAMOKU_CD.Location = New System.Drawing.Point(136, 170)
        Me.txt_KAMOKU_CD.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD)

        ' txt_KAMOKU_NM
        Me.txt_KAMOKU_NM.Name = "txt_KAMOKU_NM"
        Me.txt_KAMOKU_NM.Location = New System.Drawing.Point(230, 170)
        Me.txt_KAMOKU_NM.Size = New System.Drawing.Size(185, 18)
        Me.txt_KAMOKU_NM.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM)

        ' lbl_OUTPUT_FOLDER_NM
        Me.lbl_OUTPUT_FOLDER_NM.Name = "lbl_OUTPUT_FOLDER_NM"
        Me.lbl_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(26, 340)
        Me.lbl_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(109, 18)
        Me.lbl_OUTPUT_FOLDER_NM.Text = "出力先ﾌｫﾙﾀﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FOLDER_NM)

        ' txt_OUTPUT_FOLDER_NM
        Me.txt_OUTPUT_FOLDER_NM.Name = "txt_OUTPUT_FOLDER_NM"
        Me.txt_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(136, 340)
        Me.txt_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(340, 18)
        Me.txt_OUTPUT_FOLDER_NM.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FOLDER_NM)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(480, 340)
        Me.cmd_選択.Size = New System.Drawing.Size(45, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' lbl_EXPLANATION3
        Me.lbl_EXPLANATION3.Name = "lbl_EXPLANATION3"
        Me.lbl_EXPLANATION3.Location = New System.Drawing.Point(26, 377)
        Me.lbl_EXPLANATION3.Size = New System.Drawing.Size(49, 18)
        Me.lbl_EXPLANATION3.Text = "ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION3)

        ' lbl_EXPLANATION4
        Me.lbl_EXPLANATION4.Name = "lbl_EXPLANATION4"
        Me.lbl_EXPLANATION4.Location = New System.Drawing.Point(75, 396)
        Me.lbl_EXPLANATION4.Size = New System.Drawing.Size(49, 18)
        Me.lbl_EXPLANATION4.Text = "CORE"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION4)

        ' lbl_EXPLANATION5
        Me.lbl_EXPLANATION5.Name = "lbl_EXPLANATION5"
        Me.lbl_EXPLANATION5.Location = New System.Drawing.Point(75, 438)
        Me.lbl_EXPLANATION5.Size = New System.Drawing.Size(49, 18)
        Me.lbl_EXPLANATION5.Text = "AP+"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION5)

        ' lbl_OUTPUT_FILE1_NM
        Me.lbl_OUTPUT_FILE1_NM.Name = "lbl_OUTPUT_FILE1_NM"
        Me.lbl_OUTPUT_FILE1_NM.Location = New System.Drawing.Point(75, 415)
        Me.lbl_OUTPUT_FILE1_NM.Size = New System.Drawing.Size(207, 18)
        Me.lbl_OUTPUT_FILE1_NM.Text = "その他システム用仕訳ワーク"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FILE1_NM)

        ' lbl_OUTPUT_FILE2_NM
        Me.lbl_OUTPUT_FILE2_NM.Name = "lbl_OUTPUT_FILE2_NM"
        Me.lbl_OUTPUT_FILE2_NM.Location = New System.Drawing.Point(75, 457)
        Me.lbl_OUTPUT_FILE2_NM.Size = New System.Drawing.Size(207, 18)
        Me.lbl_OUTPUT_FILE2_NM.Text = "外部システム用伝票見出しワーク"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FILE2_NM)

        ' lbl_OUTPUT_FILE3_NM
        Me.lbl_OUTPUT_FILE3_NM.Name = "lbl_OUTPUT_FILE3_NM"
        Me.lbl_OUTPUT_FILE3_NM.Location = New System.Drawing.Point(75, 476)
        Me.lbl_OUTPUT_FILE3_NM.Size = New System.Drawing.Size(207, 18)
        Me.lbl_OUTPUT_FILE3_NM.Text = "外部システム用伝票明細ワーク"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FILE3_NM)

        ' lbl_OUTPUT_FILE4_NM
        Me.lbl_OUTPUT_FILE4_NM.Name = "lbl_OUTPUT_FILE4_NM"
        Me.lbl_OUTPUT_FILE4_NM.Location = New System.Drawing.Point(75, 495)
        Me.lbl_OUTPUT_FILE4_NM.Size = New System.Drawing.Size(207, 18)
        Me.lbl_OUTPUT_FILE4_NM.Text = "外部システム用伝票支払明細ワーク"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FILE4_NM)

        ' txt_OUTPUT_FILE1_NM
        Me.txt_OUTPUT_FILE1_NM.Name = "txt_OUTPUT_FILE1_NM"
        Me.txt_OUTPUT_FILE1_NM.Location = New System.Drawing.Point(283, 415)
        Me.txt_OUTPUT_FILE1_NM.Size = New System.Drawing.Size(132, 18)
        Me.txt_OUTPUT_FILE1_NM.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FILE1_NM)

        ' txt_OUTPUT_FILE2_NM
        Me.txt_OUTPUT_FILE2_NM.Name = "txt_OUTPUT_FILE2_NM"
        Me.txt_OUTPUT_FILE2_NM.Location = New System.Drawing.Point(283, 457)
        Me.txt_OUTPUT_FILE2_NM.Size = New System.Drawing.Size(132, 18)
        Me.txt_OUTPUT_FILE2_NM.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FILE2_NM)

        ' txt_OUTPUT_FILE3_NM
        Me.txt_OUTPUT_FILE3_NM.Name = "txt_OUTPUT_FILE3_NM"
        Me.txt_OUTPUT_FILE3_NM.Location = New System.Drawing.Point(283, 476)
        Me.txt_OUTPUT_FILE3_NM.Size = New System.Drawing.Size(132, 18)
        Me.txt_OUTPUT_FILE3_NM.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FILE3_NM)

        ' txt_OUTPUT_FILE4_NM
        Me.txt_OUTPUT_FILE4_NM.Name = "txt_OUTPUT_FILE4_NM"
        Me.txt_OUTPUT_FILE4_NM.Location = New System.Drawing.Point(283, 495)
        Me.txt_OUTPUT_FILE4_NM.Size = New System.Drawing.Size(132, 18)
        Me.txt_OUTPUT_FILE4_NM.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FILE4_NM)

        ' txt_KAMOKU
        Me.txt_KAMOKU.Name = "txt_KAMOKU"
        Me.txt_KAMOKU.Location = New System.Drawing.Point(26, 170)
        Me.txt_KAMOKU.Size = New System.Drawing.Size(109, 18)
        Me.txt_KAMOKU.Text = "未払金"
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU)

        ' lbl_BSHO_CD
        Me.lbl_BSHO_CD.Name = "lbl_BSHO_CD"
        Me.lbl_BSHO_CD.Location = New System.Drawing.Point(26, 200)
        Me.lbl_BSHO_CD.Size = New System.Drawing.Size(109, 18)
        Me.lbl_BSHO_CD.Text = "部署ｺｰﾄﾞ(一括)"
        Me.pnlDetail.Controls.Add(Me.lbl_BSHO_CD)

        ' txt_BSHO_CD
        Me.txt_BSHO_CD.Name = "txt_BSHO_CD"
        Me.txt_BSHO_CD.Location = New System.Drawing.Point(136, 200)
        Me.txt_BSHO_CD.Size = New System.Drawing.Size(94, 18)
        Me.txt_BSHO_CD.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_BSHO_CD)

        ' sub_fc_支払仕訳_KITOKU_SUB
        Me.sub_fc_支払仕訳_KITOKU_SUB.Name = "sub_fc_支払仕訳_KITOKU_SUB"
        Me.sub_fc_支払仕訳_KITOKU_SUB.Location = New System.Drawing.Point(26, 249)
        Me.sub_fc_支払仕訳_KITOKU_SUB.Size = New System.Drawing.Size(211, 79)
        Me.sub_fc_支払仕訳_KITOKU_SUB.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.sub_fc_支払仕訳_KITOKU_SUB)

        ' ラベル25
        Me.ラベル25.Name = "ラベル25"
        Me.ラベル25.Location = New System.Drawing.Point(26, 230)
        Me.ラベル25.Size = New System.Drawing.Size(90, 18)
        Me.ラベル25.Text = "＜税処理ｺｰﾄﾞ＞"
        Me.pnlDetail.Controls.Add(Me.ラベル25)

        Me.Name = "Frmfc_支払仕訳_KITOKU"
        Me.Text = "支払仕訳出力画面"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977709376 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977711232 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917977711168 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917977719104 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_Subform_1917977709760 As System.Windows.Forms.Panel
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_SLIP_DT As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_DT As System.Windows.Forms.Label
    Friend WithEvents txt_SLIP_NO_START_VAL As System.Windows.Forms.TextBox
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_SLIP_NO_START_VAL As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_CD As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_NM As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_OUTPUT_FOLDER_NM As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FOLDER_NM As System.Windows.Forms.TextBox
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents lbl_EXPLANATION3 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION4 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION5 As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FILE1_NM As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FILE2_NM As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FILE3_NM As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FILE4_NM As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FILE1_NM As System.Windows.Forms.TextBox
    Friend WithEvents txt_OUTPUT_FILE2_NM As System.Windows.Forms.TextBox
    Friend WithEvents txt_OUTPUT_FILE3_NM As System.Windows.Forms.TextBox
    Friend WithEvents txt_OUTPUT_FILE4_NM As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU As System.Windows.Forms.Label
    Friend WithEvents lbl_BSHO_CD As System.Windows.Forms.Label
    Friend WithEvents txt_BSHO_CD As System.Windows.Forms.TextBox
    Friend WithEvents sub_fc_支払仕訳_KITOKU_SUB As System.Windows.Forms.Panel
    Friend WithEvents ラベル25 As System.Windows.Forms.Label

End Class
