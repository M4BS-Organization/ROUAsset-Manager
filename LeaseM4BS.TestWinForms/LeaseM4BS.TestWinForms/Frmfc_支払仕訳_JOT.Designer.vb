<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_支払仕訳_JOT
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
        Me.unnamed_Label_1917970281472 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970280256 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970278080 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970276288 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917970279424 = New System.Windows.Forms.ComboBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_YMD_01 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_01 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_出力元の抽出 = New System.Windows.Forms.Label()
        Me.chk_CHK_01 = New System.Windows.Forms.CheckBox()
        Me.lbl_CHK_01 = New System.Windows.Forms.Label()
        Me.cmd_伝票番号 = New System.Windows.Forms.Button()
        Me.txt_KAMOKU_CD_02 = New System.Windows.Forms.TextBox()
        Me.txt_YMD_02 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_02 = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_CD_01 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_NM_01 = New System.Windows.Forms.TextBox()
        Me.lbl_科目ｺｰﾄﾞ = New System.Windows.Forms.Label()
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.lbl_OUTPUT_FPATH = New System.Windows.Forms.Label()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.txt_YMD_03 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_03 = New System.Windows.Forms.Label()
        Me.cmb_KAMOKU_CD_01 = New System.Windows.Forms.ComboBox()
        Me.lbl_KAMOKU_CD_02 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_03 = New System.Windows.Forms.TextBox()
        Me.ラベル75 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_支払仕訳_JOT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970281472)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970280256)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970278080)
        Me.Controls.Add(Me.unnamed_TextBox_1917970276288)
        Me.Controls.Add(Me.unnamed_ComboBox_1917970279424)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970281472
        Me.unnamed_Label_1917970281472.Name = "unnamed_Label_1917970281472"
        Me.unnamed_Label_1917970281472.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970281472.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970280256
        Me.unnamed_CommandButton_1917970280256.Name = "unnamed_CommandButton_1917970280256"
        Me.unnamed_CommandButton_1917970280256.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970280256.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970278080
        Me.unnamed_CheckBox_1917970278080.Name = "unnamed_CheckBox_1917970278080"
        Me.unnamed_CheckBox_1917970278080.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970278080.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970276288
        Me.unnamed_TextBox_1917970276288.Name = "unnamed_TextBox_1917970276288"
        Me.unnamed_TextBox_1917970276288.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970276288.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917970279424
        Me.unnamed_ComboBox_1917970279424.Name = "unnamed_ComboBox_1917970279424"
        Me.unnamed_ComboBox_1917970279424.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917970279424.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(136, 120)
        Me.lbl_EXPLANATION2.Size = New System.Drawing.Size(393, 18)
        Me.lbl_EXPLANATION2.Text = "   仕訳ﾃﾞｰﾀを作成します。部分出力以外に使用しないでください。"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION2)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(3, 3)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行"
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(86, 3)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_YMD_01
        Me.txt_YMD_01.Name = "txt_YMD_01"
        Me.txt_YMD_01.Location = New System.Drawing.Point(136, 45)
        Me.txt_YMD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_01.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_YMD_01)

        ' lbl_YMD_01
        Me.lbl_YMD_01.Name = "lbl_YMD_01"
        Me.lbl_YMD_01.Location = New System.Drawing.Point(26, 45)
        Me.lbl_YMD_01.Size = New System.Drawing.Size(109, 18)
        Me.lbl_YMD_01.Text = "処理年月"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_01)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(136, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(393, 18)
        Me.lbl_EXPLANATION1.Text = "※月次支払照合ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_出力元の抽出
        Me.lbl_出力元の抽出.Name = "lbl_出力元の抽出"
        Me.lbl_出力元の抽出.Location = New System.Drawing.Point(26, 75)
        Me.lbl_出力元の抽出.Size = New System.Drawing.Size(109, 18)
        Me.lbl_出力元の抽出.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_出力元の抽出)

        ' chk_CHK_01
        Me.chk_CHK_01.Name = "chk_CHK_01"
        Me.chk_CHK_01.Location = New System.Drawing.Point(154, 79)
        Me.chk_CHK_01.Size = New System.Drawing.Size(120, 11)
        Me.chk_CHK_01.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.chk_CHK_01)

        ' lbl_CHK_01
        Me.lbl_CHK_01.Name = "lbl_CHK_01"
        Me.lbl_CHK_01.Location = New System.Drawing.Point(170, 75)
        Me.lbl_CHK_01.Size = New System.Drawing.Size(113, 15)
        Me.lbl_CHK_01.Text = "検索条件を加味する"
        Me.chk_CHK_01.Controls.Add(Me.lbl_CHK_01)

        ' cmd_伝票番号
        Me.cmd_伝票番号.Name = "cmd_伝票番号"
        Me.cmd_伝票番号.Location = New System.Drawing.Point(393, 3)
        Me.cmd_伝票番号.Size = New System.Drawing.Size(136, 26)
        Me.cmd_伝票番号.Text = "伝票番号管理ﾃｰﾌﾞﾙ"
        Me.cmd_伝票番号.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_伝票番号)

        ' txt_KAMOKU_CD_02
        Me.txt_KAMOKU_CD_02.Name = "txt_KAMOKU_CD_02"
        Me.txt_KAMOKU_CD_02.Location = New System.Drawing.Point(245, 238)
        Me.txt_KAMOKU_CD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_02.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_02)

        ' txt_YMD_02
        Me.txt_YMD_02.Name = "txt_YMD_02"
        Me.txt_YMD_02.Location = New System.Drawing.Point(136, 151)
        Me.txt_YMD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_02.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_YMD_02)

        ' lbl_YMD_02
        Me.lbl_YMD_02.Name = "lbl_YMD_02"
        Me.lbl_YMD_02.Location = New System.Drawing.Point(26, 151)
        Me.lbl_YMD_02.Size = New System.Drawing.Size(109, 18)
        Me.lbl_YMD_02.Text = "処理日付"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_02)

        ' lbl_KAMOKU_CD_01
        Me.lbl_KAMOKU_CD_01.Name = "lbl_KAMOKU_CD_01"
        Me.lbl_KAMOKU_CD_01.Location = New System.Drawing.Point(26, 207)
        Me.lbl_KAMOKU_CD_01.Size = New System.Drawing.Size(109, 18)
        Me.lbl_KAMOKU_CD_01.Text = "部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_01)

        ' txt_KAMOKU_NM_01
        Me.txt_KAMOKU_NM_01.Name = "txt_KAMOKU_NM_01"
        Me.txt_KAMOKU_NM_01.Location = New System.Drawing.Point(230, 207)
        Me.txt_KAMOKU_NM_01.Size = New System.Drawing.Size(226, 18)
        Me.txt_KAMOKU_NM_01.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_01)

        ' lbl_科目ｺｰﾄﾞ
        Me.lbl_科目ｺｰﾄﾞ.Name = "lbl_科目ｺｰﾄﾞ"
        Me.lbl_科目ｺｰﾄﾞ.Location = New System.Drawing.Point(26, 238)
        Me.lbl_科目ｺｰﾄﾞ.Size = New System.Drawing.Size(109, 37)
        Me.lbl_科目ｺｰﾄﾞ.Text = "科目ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_科目ｺｰﾄﾞ)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(136, 287)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(321, 18)
        Me.txt_TEXT_01.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' lbl_OUTPUT_FPATH
        Me.lbl_OUTPUT_FPATH.Name = "lbl_OUTPUT_FPATH"
        Me.lbl_OUTPUT_FPATH.Location = New System.Drawing.Point(26, 287)
        Me.lbl_OUTPUT_FPATH.Size = New System.Drawing.Size(109, 18)
        Me.lbl_OUTPUT_FPATH.Text = "出力ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FPATH)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(461, 287)
        Me.cmd_選択.Size = New System.Drawing.Size(56, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' txt_YMD_03
        Me.txt_YMD_03.Name = "txt_YMD_03"
        Me.txt_YMD_03.Location = New System.Drawing.Point(136, 177)
        Me.txt_YMD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_03.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_YMD_03)

        ' lbl_YMD_03
        Me.lbl_YMD_03.Name = "lbl_YMD_03"
        Me.lbl_YMD_03.Location = New System.Drawing.Point(26, 177)
        Me.lbl_YMD_03.Size = New System.Drawing.Size(109, 18)
        Me.lbl_YMD_03.Text = "計上日付"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_03)

        ' cmb_KAMOKU_CD_01
        Me.cmb_KAMOKU_CD_01.Name = "cmb_KAMOKU_CD_01"
        Me.cmb_KAMOKU_CD_01.Location = New System.Drawing.Point(136, 207)
        Me.cmb_KAMOKU_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.cmb_KAMOKU_CD_01.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmb_KAMOKU_CD_01)

        ' lbl_KAMOKU_CD_02
        Me.lbl_KAMOKU_CD_02.Name = "lbl_KAMOKU_CD_02"
        Me.lbl_KAMOKU_CD_02.Location = New System.Drawing.Point(136, 238)
        Me.lbl_KAMOKU_CD_02.Size = New System.Drawing.Size(109, 18)
        Me.lbl_KAMOKU_CD_02.Text = "前払費用"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_02)

        ' txt_KAMOKU_CD_03
        Me.txt_KAMOKU_CD_03.Name = "txt_KAMOKU_CD_03"
        Me.txt_KAMOKU_CD_03.Location = New System.Drawing.Point(245, 257)
        Me.txt_KAMOKU_CD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_03.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_03)

        ' ラベル75
        Me.ラベル75.Name = "ラベル75"
        Me.ラベル75.Location = New System.Drawing.Point(136, 257)
        Me.ラベル75.Size = New System.Drawing.Size(109, 18)
        Me.ラベル75.Text = "未払利息"
        Me.pnlDetail.Controls.Add(Me.ラベル75)

        Me.Name = "Frmfc_支払仕訳_JOT"
        Me.Text = "月次支払照合ﾌﾚｯｸｽ － 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970281472 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970280256 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970278080 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970276288 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917970279424 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_YMD_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_01 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_出力元の抽出 As System.Windows.Forms.Label
    Friend WithEvents chk_CHK_01 As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_CHK_01 As System.Windows.Forms.Label
    Friend WithEvents cmd_伝票番号 As System.Windows.Forms.Button
    Friend WithEvents txt_KAMOKU_CD_02 As System.Windows.Forms.TextBox
    Friend WithEvents txt_YMD_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_02 As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_CD_01 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_NM_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_科目ｺｰﾄﾞ As System.Windows.Forms.Label
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_OUTPUT_FPATH As System.Windows.Forms.Label
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents txt_YMD_03 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_03 As System.Windows.Forms.Label
    Friend WithEvents cmb_KAMOKU_CD_01 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_KAMOKU_CD_02 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_03 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル75 As System.Windows.Forms.Label

End Class
