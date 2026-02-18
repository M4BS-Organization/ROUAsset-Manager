<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_経費仕訳_NKSOL
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
        Me.unnamed_Label_1917970507456 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970466752 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970485696 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977380352 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_YMD_01 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_01 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_CHK_01 = New System.Windows.Forms.Label()
        Me.chk_CHK_01 = New System.Windows.Forms.CheckBox()
        Me.lbl_CHK_01_F = New System.Windows.Forms.Label()
        Me.txt_YMD_02 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_02 = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_CD_01 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_01 = New System.Windows.Forms.TextBox()
        Me.txt_TEXT_02 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_02 = New System.Windows.Forms.Label()
        Me.cmd_選択2 = New System.Windows.Forms.Button()
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_01 = New System.Windows.Forms.Label()
        Me.cmd_選択1 = New System.Windows.Forms.Button()
        Me.txt_KAMOKU_NM_01 = New System.Windows.Forms.TextBox()
        Me.ラベル83 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_02 = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU_NM_02 = New System.Windows.Forms.TextBox()
        Me.ラベル88 = New System.Windows.Forms.Label()
        Me.ラベル89 = New System.Windows.Forms.Label()
        Me.ラベル91 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_03 = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU_NM_03 = New System.Windows.Forms.TextBox()
        Me.ラベル94 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_04 = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU_NM_04 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' Frmfc_経費仕訳_NKSOL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970507456)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970466752)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970485696)
        Me.Controls.Add(Me.unnamed_TextBox_1917977380352)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970507456
        Me.unnamed_Label_1917970507456.Name = "unnamed_Label_1917970507456"
        Me.unnamed_Label_1917970507456.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970507456.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970466752
        Me.unnamed_CommandButton_1917970466752.Name = "unnamed_CommandButton_1917970466752"
        Me.unnamed_CommandButton_1917970466752.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970466752.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970485696
        Me.unnamed_CheckBox_1917970485696.Name = "unnamed_CheckBox_1917970485696"
        Me.unnamed_CheckBox_1917970485696.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970485696.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977380352
        Me.unnamed_TextBox_1917977380352.Name = "unnamed_TextBox_1917977380352"
        Me.unnamed_TextBox_1917977380352.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977380352.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(143, 120)
        Me.lbl_EXPLANATION2.Size = New System.Drawing.Size(385, 18)
        Me.lbl_EXPLANATION2.Text = "   仕訳ﾃﾞｰﾀを作成します。部分出力以外に使用しないでください。"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION2)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(3, 3)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(83, 3)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_YMD_01
        Me.txt_YMD_01.Name = "txt_YMD_01"
        Me.txt_YMD_01.Location = New System.Drawing.Point(143, 45)
        Me.txt_YMD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_01.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_YMD_01)

        ' lbl_YMD_01
        Me.lbl_YMD_01.Name = "lbl_YMD_01"
        Me.lbl_YMD_01.Location = New System.Drawing.Point(26, 45)
        Me.lbl_YMD_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_YMD_01.Text = "出力年月"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_01)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(143, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(385, 18)
        Me.lbl_EXPLANATION1.Text = "※経費明細表を検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_CHK_01
        Me.lbl_CHK_01.Name = "lbl_CHK_01"
        Me.lbl_CHK_01.Location = New System.Drawing.Point(26, 75)
        Me.lbl_CHK_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_CHK_01.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_CHK_01)

        ' chk_CHK_01
        Me.chk_CHK_01.Name = "chk_CHK_01"
        Me.chk_CHK_01.Location = New System.Drawing.Point(162, 79)
        Me.chk_CHK_01.Size = New System.Drawing.Size(120, 11)
        Me.chk_CHK_01.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_CHK_01)

        ' lbl_CHK_01_F
        Me.lbl_CHK_01_F.Name = "lbl_CHK_01_F"
        Me.lbl_CHK_01_F.Location = New System.Drawing.Point(177, 75)
        Me.lbl_CHK_01_F.Size = New System.Drawing.Size(113, 15)
        Me.lbl_CHK_01_F.Text = "検索条件を加味する"
        Me.chk_CHK_01.Controls.Add(Me.lbl_CHK_01_F)

        ' txt_YMD_02
        Me.txt_YMD_02.Name = "txt_YMD_02"
        Me.txt_YMD_02.Location = New System.Drawing.Point(143, 151)
        Me.txt_YMD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_02.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_YMD_02)

        ' lbl_YMD_02
        Me.lbl_YMD_02.Name = "lbl_YMD_02"
        Me.lbl_YMD_02.Location = New System.Drawing.Point(26, 151)
        Me.lbl_YMD_02.Size = New System.Drawing.Size(117, 18)
        Me.lbl_YMD_02.Text = "伝票日付"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_02)

        ' lbl_KAMOKU_CD_01
        Me.lbl_KAMOKU_CD_01.Name = "lbl_KAMOKU_CD_01"
        Me.lbl_KAMOKU_CD_01.Location = New System.Drawing.Point(26, 177)
        Me.lbl_KAMOKU_CD_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_KAMOKU_CD_01.Text = " 部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_01)

        ' txt_KAMOKU_CD_01
        Me.txt_KAMOKU_CD_01.Name = "txt_KAMOKU_CD_01"
        Me.txt_KAMOKU_CD_01.Location = New System.Drawing.Point(143, 177)
        Me.txt_KAMOKU_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_01.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_01)

        ' txt_TEXT_02
        Me.txt_TEXT_02.Name = "txt_TEXT_02"
        Me.txt_TEXT_02.Location = New System.Drawing.Point(143, 325)
        Me.txt_TEXT_02.Size = New System.Drawing.Size(377, 18)
        Me.txt_TEXT_02.Visible = False
        Me.txt_TEXT_02.TabIndex = 15
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_02)

        ' lbl_TEXT_02
        Me.lbl_TEXT_02.Name = "lbl_TEXT_02"
        Me.lbl_TEXT_02.Location = New System.Drawing.Point(26, 325)
        Me.lbl_TEXT_02.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_02.Text = "出力先EXCELﾌｧｲﾙ名"
        Me.lbl_TEXT_02.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_02)

        ' cmd_選択2
        Me.cmd_選択2.Name = "cmd_選択2"
        Me.cmd_選択2.Location = New System.Drawing.Point(529, 325)
        Me.cmd_選択2.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択2.Text = "選択(&S)"
        Me.cmd_選択2.Visible = False
        Me.cmd_選択2.TabIndex = 16
        Me.pnlDetail.Controls.Add(Me.cmd_選択2)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(143, 268)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(377, 18)
        Me.txt_TEXT_01.TabIndex = 13
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' lbl_TEXT_01
        Me.lbl_TEXT_01.Name = "lbl_TEXT_01"
        Me.lbl_TEXT_01.Location = New System.Drawing.Point(26, 268)
        Me.lbl_TEXT_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_01.Text = "出力先ﾌｫﾙﾀﾞ名"
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_01)

        ' cmd_選択1
        Me.cmd_選択1.Name = "cmd_選択1"
        Me.cmd_選択1.Location = New System.Drawing.Point(529, 268)
        Me.cmd_選択1.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択1.Text = "選択(&F)"
        Me.cmd_選択1.TabIndex = 14
        Me.pnlDetail.Controls.Add(Me.cmd_選択1)

        ' txt_KAMOKU_NM_01
        Me.txt_KAMOKU_NM_01.Name = "txt_KAMOKU_NM_01"
        Me.txt_KAMOKU_NM_01.Location = New System.Drawing.Point(238, 177)
        Me.txt_KAMOKU_NM_01.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_01.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_01)

        ' ラベル83
        Me.ラベル83.Name = "ラベル83"
        Me.ラベル83.Location = New System.Drawing.Point(26, 222)
        Me.ラベル83.Size = New System.Drawing.Size(117, 18)
        Me.ラベル83.Text = " 科目ｺｰﾄﾞ(前払費用)"
        Me.pnlDetail.Controls.Add(Me.ラベル83)

        ' txt_KAMOKU_CD_02
        Me.txt_KAMOKU_CD_02.Name = "txt_KAMOKU_CD_02"
        Me.txt_KAMOKU_CD_02.Location = New System.Drawing.Point(143, 196)
        Me.txt_KAMOKU_CD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_02.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_02)

        ' txt_KAMOKU_NM_02
        Me.txt_KAMOKU_NM_02.Name = "txt_KAMOKU_NM_02"
        Me.txt_KAMOKU_NM_02.Location = New System.Drawing.Point(238, 196)
        Me.txt_KAMOKU_NM_02.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_02.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_02)

        ' ラベル88
        Me.ラベル88.Name = "ラベル88"
        Me.ラベル88.Location = New System.Drawing.Point(45, 294)
        Me.ラベル88.Size = New System.Drawing.Size(283, 15)
        Me.ラベル88.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_前払取崩.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル88)

        ' ラベル89
        Me.ラベル89.Name = "ラベル89"
        Me.ラベル89.Location = New System.Drawing.Point(45, 309)
        Me.ラベル89.Size = New System.Drawing.Size(283, 15)
        Me.ラベル89.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_前払抹消.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル89)

        ' ラベル91
        Me.ラベル91.Name = "ラベル91"
        Me.ラベル91.Location = New System.Drawing.Point(26, 196)
        Me.ラベル91.Size = New System.Drawing.Size(117, 18)
        Me.ラベル91.Text = " 部署ｺｰﾄﾞ(賃借料)"
        Me.pnlDetail.Controls.Add(Me.ラベル91)

        ' txt_KAMOKU_CD_03
        Me.txt_KAMOKU_CD_03.Name = "txt_KAMOKU_CD_03"
        Me.txt_KAMOKU_CD_03.Location = New System.Drawing.Point(143, 222)
        Me.txt_KAMOKU_CD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_03.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_03)

        ' txt_KAMOKU_NM_03
        Me.txt_KAMOKU_NM_03.Name = "txt_KAMOKU_NM_03"
        Me.txt_KAMOKU_NM_03.Location = New System.Drawing.Point(238, 222)
        Me.txt_KAMOKU_NM_03.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_03.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_03)

        ' ラベル94
        Me.ラベル94.Name = "ラベル94"
        Me.ラベル94.Location = New System.Drawing.Point(26, 241)
        Me.ラベル94.Size = New System.Drawing.Size(117, 18)
        Me.ラベル94.Text = " 科目ｺｰﾄﾞ(解約損)"
        Me.pnlDetail.Controls.Add(Me.ラベル94)

        ' txt_KAMOKU_CD_04
        Me.txt_KAMOKU_CD_04.Name = "txt_KAMOKU_CD_04"
        Me.txt_KAMOKU_CD_04.Location = New System.Drawing.Point(143, 241)
        Me.txt_KAMOKU_CD_04.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_04.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_04)

        ' txt_KAMOKU_NM_04
        Me.txt_KAMOKU_NM_04.Name = "txt_KAMOKU_NM_04"
        Me.txt_KAMOKU_NM_04.Location = New System.Drawing.Point(238, 241)
        Me.txt_KAMOKU_NM_04.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_04.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_04)

        Me.Name = "Frmfc_経費仕訳_NKSOL"
        Me.Text = "経費明細表 - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970507456 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970466752 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970485696 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977380352 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_YMD_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_01 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_CHK_01 As System.Windows.Forms.Label
    Friend WithEvents chk_CHK_01 As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_CHK_01_F As System.Windows.Forms.Label
    Friend WithEvents txt_YMD_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_02 As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_CD_01 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_01 As System.Windows.Forms.TextBox
    Friend WithEvents txt_TEXT_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_02 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択2 As System.Windows.Forms.Button
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_01 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択1 As System.Windows.Forms.Button
    Friend WithEvents txt_KAMOKU_NM_01 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル83 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_02 As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU_NM_02 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル88 As System.Windows.Forms.Label
    Friend WithEvents ラベル89 As System.Windows.Forms.Label
    Friend WithEvents ラベル91 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_03 As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU_NM_03 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル94 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_04 As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU_NM_04 As System.Windows.Forms.TextBox

End Class
