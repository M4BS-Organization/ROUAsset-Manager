<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_計上仕訳_NKSOL
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
        Me.unnamed_Label_1917978177792 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917978180928 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917978178688 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970486080 = New System.Windows.Forms.TextBox()
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
        Me.lbl_ﾌｧｲﾙ名1 = New System.Windows.Forms.Label()
        Me.txt_TEXT_02 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_02 = New System.Windows.Forms.Label()
        Me.cmd_選択2 = New System.Windows.Forms.Button()
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_01 = New System.Windows.Forms.Label()
        Me.cmd_選択1 = New System.Windows.Forms.Button()
        Me.lbl_ﾌｧｲﾙ名2 = New System.Windows.Forms.Label()
        Me.lbl_ﾌｧｲﾙ名3 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_NM_01 = New System.Windows.Forms.TextBox()
        Me.ラベル83 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_02 = New System.Windows.Forms.TextBox()
        Me.txt_KAMOKU_NM_02 = New System.Windows.Forms.TextBox()
        Me.ラベル86 = New System.Windows.Forms.Label()
        Me.ラベル87 = New System.Windows.Forms.Label()
        Me.ラベル88 = New System.Windows.Forms.Label()
        Me.ラベル89 = New System.Windows.Forms.Label()
        Me.ラベル90 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_計上仕訳_NKSOL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 800)
        Me.Controls.Add(Me.unnamed_Label_1917978177792)
        Me.Controls.Add(Me.unnamed_CommandButton_1917978180928)
        Me.Controls.Add(Me.unnamed_CheckBox_1917978178688)
        Me.Controls.Add(Me.unnamed_TextBox_1917970486080)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917978177792
        Me.unnamed_Label_1917978177792.Name = "unnamed_Label_1917978177792"
        Me.unnamed_Label_1917978177792.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917978177792.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917978180928
        Me.unnamed_CommandButton_1917978180928.Name = "unnamed_CommandButton_1917978180928"
        Me.unnamed_CommandButton_1917978180928.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917978180928.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917978178688
        Me.unnamed_CheckBox_1917978178688.Name = "unnamed_CheckBox_1917978178688"
        Me.unnamed_CheckBox_1917978178688.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917978178688.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970486080
        Me.unnamed_TextBox_1917970486080.Name = "unnamed_TextBox_1917970486080"
        Me.unnamed_TextBox_1917970486080.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970486080.Size = New System.Drawing.Size(113, 26)

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
        Me.lbl_EXPLANATION1.Text = "※月次仕訳計上ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
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
        Me.lbl_KAMOKU_CD_01.Text = "部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_01)

        ' txt_KAMOKU_CD_01
        Me.txt_KAMOKU_CD_01.Name = "txt_KAMOKU_CD_01"
        Me.txt_KAMOKU_CD_01.Location = New System.Drawing.Point(143, 177)
        Me.txt_KAMOKU_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_01.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_01)

        ' lbl_ﾌｧｲﾙ名1
        Me.lbl_ﾌｧｲﾙ名1.Name = "lbl_ﾌｧｲﾙ名1"
        Me.lbl_ﾌｧｲﾙ名1.Location = New System.Drawing.Point(45, 275)
        Me.lbl_ﾌｧｲﾙ名1.Size = New System.Drawing.Size(283, 15)
        Me.lbl_ﾌｧｲﾙ名1.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_開始.xls"
        Me.pnlDetail.Controls.Add(Me.lbl_ﾌｧｲﾙ名1)

        ' txt_TEXT_02
        Me.txt_TEXT_02.Name = "txt_TEXT_02"
        Me.txt_TEXT_02.Location = New System.Drawing.Point(143, 408)
        Me.txt_TEXT_02.Size = New System.Drawing.Size(377, 18)
        Me.txt_TEXT_02.Visible = False
        Me.txt_TEXT_02.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_02)

        ' lbl_TEXT_02
        Me.lbl_TEXT_02.Name = "lbl_TEXT_02"
        Me.lbl_TEXT_02.Location = New System.Drawing.Point(26, 408)
        Me.lbl_TEXT_02.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_02.Text = "出力先EXCELﾌｧｲﾙ名"
        Me.lbl_TEXT_02.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_02)

        ' cmd_選択2
        Me.cmd_選択2.Name = "cmd_選択2"
        Me.cmd_選択2.Location = New System.Drawing.Point(529, 408)
        Me.cmd_選択2.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択2.Text = "選択(&S)"
        Me.cmd_選択2.Visible = False
        Me.cmd_選択2.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.cmd_選択2)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(143, 249)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(377, 18)
        Me.txt_TEXT_01.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' lbl_TEXT_01
        Me.lbl_TEXT_01.Name = "lbl_TEXT_01"
        Me.lbl_TEXT_01.Location = New System.Drawing.Point(26, 249)
        Me.lbl_TEXT_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_01.Text = "出力先ﾌｫﾙﾀﾞ名"
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_01)

        ' cmd_選択1
        Me.cmd_選択1.Name = "cmd_選択1"
        Me.cmd_選択1.Location = New System.Drawing.Point(529, 249)
        Me.cmd_選択1.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択1.Text = "選択(&F)"
        Me.cmd_選択1.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.cmd_選択1)

        ' lbl_ﾌｧｲﾙ名2
        Me.lbl_ﾌｧｲﾙ名2.Name = "lbl_ﾌｧｲﾙ名2"
        Me.lbl_ﾌｧｲﾙ名2.Location = New System.Drawing.Point(45, 291)
        Me.lbl_ﾌｧｲﾙ名2.Size = New System.Drawing.Size(283, 15)
        Me.lbl_ﾌｧｲﾙ名2.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_償却.xls"
        Me.pnlDetail.Controls.Add(Me.lbl_ﾌｧｲﾙ名2)

        ' lbl_ﾌｧｲﾙ名3
        Me.lbl_ﾌｧｲﾙ名3.Name = "lbl_ﾌｧｲﾙ名3"
        Me.lbl_ﾌｧｲﾙ名3.Location = New System.Drawing.Point(45, 306)
        Me.lbl_ﾌｧｲﾙ名3.Size = New System.Drawing.Size(283, 15)
        Me.lbl_ﾌｧｲﾙ名3.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_減損.xls"
        Me.pnlDetail.Controls.Add(Me.lbl_ﾌｧｲﾙ名3)

        ' txt_KAMOKU_NM_01
        Me.txt_KAMOKU_NM_01.Name = "txt_KAMOKU_NM_01"
        Me.txt_KAMOKU_NM_01.Location = New System.Drawing.Point(238, 177)
        Me.txt_KAMOKU_NM_01.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_01.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_01)

        ' ラベル83
        Me.ラベル83.Name = "ラベル83"
        Me.ラベル83.Location = New System.Drawing.Point(26, 196)
        Me.ラベル83.Size = New System.Drawing.Size(117, 18)
        Me.ラベル83.Text = "科目ｺｰﾄﾞ(未払費用)"
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

        ' ラベル86
        Me.ラベル86.Name = "ラベル86"
        Me.ラベル86.Location = New System.Drawing.Point(45, 321)
        Me.ラベル86.Size = New System.Drawing.Size(283, 15)
        Me.ラベル86.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_終了.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル86)

        ' ラベル87
        Me.ラベル87.Name = "ラベル87"
        Me.ラベル87.Location = New System.Drawing.Point(45, 336)
        Me.ラベル87.Size = New System.Drawing.Size(283, 15)
        Me.ラベル87.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_債務抹消.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル87)

        ' ラベル88
        Me.ラベル88.Name = "ラベル88"
        Me.ラベル88.Location = New System.Drawing.Point(45, 355)
        Me.ラベル88.Size = New System.Drawing.Size(283, 15)
        Me.ラベル88.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_減損.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル88)

        ' ラベル89
        Me.ラベル89.Name = "ラベル89"
        Me.ラベル89.Location = New System.Drawing.Point(45, 370)
        Me.ラベル89.Size = New System.Drawing.Size(283, 15)
        Me.ラベル89.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_減損取崩.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル89)

        ' ラベル90
        Me.ラベル90.Name = "ラベル90"
        Me.ラベル90.Location = New System.Drawing.Point(45, 385)
        Me.ラベル90.Size = New System.Drawing.Size(283, 15)
        Me.ラベル90.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_減損抹消.xls"
        Me.pnlDetail.Controls.Add(Me.ラベル90)

        Me.Name = "Frmfc_計上仕訳_NKSOL"
        Me.Text = "月次仕訳計上ﾌﾚｯｸｽ - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917978177792 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917978180928 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917978178688 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970486080 As System.Windows.Forms.TextBox
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
    Friend WithEvents lbl_ﾌｧｲﾙ名1 As System.Windows.Forms.Label
    Friend WithEvents txt_TEXT_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_02 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択2 As System.Windows.Forms.Button
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_01 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択1 As System.Windows.Forms.Button
    Friend WithEvents lbl_ﾌｧｲﾙ名2 As System.Windows.Forms.Label
    Friend WithEvents lbl_ﾌｧｲﾙ名3 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_NM_01 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル83 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_02 As System.Windows.Forms.TextBox
    Friend WithEvents txt_KAMOKU_NM_02 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル86 As System.Windows.Forms.Label
    Friend WithEvents ラベル87 As System.Windows.Forms.Label
    Friend WithEvents ラベル88 As System.Windows.Forms.Label
    Friend WithEvents ラベル89 As System.Windows.Forms.Label
    Friend WithEvents ラベル90 As System.Windows.Forms.Label

End Class
