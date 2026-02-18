<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_JOT_支払仕訳
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
        Me.unnamed_Label_1917977792640 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977795264 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977795392 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977794432 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977790592 = New System.Windows.Forms.ComboBox()
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
        Me.txt_KAMOKU_CD_01 = New System.Windows.Forms.TextBox()
        Me.lbl_KAMOKU_CD_01 = New System.Windows.Forms.Label()
        Me.txt_BUMON_NM_01 = New System.Windows.Forms.TextBox()
        Me.lbl_科目ｺｰﾄﾞ = New System.Windows.Forms.Label()
        Me.txt_YMD_02 = New System.Windows.Forms.TextBox()
        Me.lbl_伝票日付 = New System.Windows.Forms.Label()
        Me.cmb_BUMON_CD_01 = New System.Windows.Forms.ComboBox()
        Me.lbl_KAMOKU_CD_02 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_02 = New System.Windows.Forms.TextBox()
        Me.ラベル75 = New System.Windows.Forms.Label()
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.ラベル515 = New System.Windows.Forms.Label()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.txt_KAMOKU_CD_03 = New System.Windows.Forms.TextBox()
        Me.ラベル78 = New System.Windows.Forms.Label()
        Me.ラベル79 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_04 = New System.Windows.Forms.TextBox()
        Me.ラベル81 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_NM_03 = New System.Windows.Forms.TextBox()
        Me.ラベル83 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_JOT_支払仕訳
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977792640)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977795264)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977795392)
        Me.Controls.Add(Me.unnamed_TextBox_1917977794432)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977790592)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977792640
        Me.unnamed_Label_1917977792640.Name = "unnamed_Label_1917977792640"
        Me.unnamed_Label_1917977792640.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977792640.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977795264
        Me.unnamed_CommandButton_1917977795264.Name = "unnamed_CommandButton_1917977795264"
        Me.unnamed_CommandButton_1917977795264.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977795264.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917977795392
        Me.unnamed_CheckBox_1917977795392.Name = "unnamed_CheckBox_1917977795392"
        Me.unnamed_CheckBox_1917977795392.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977795392.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977794432
        Me.unnamed_TextBox_1917977794432.Name = "unnamed_TextBox_1917977794432"
        Me.unnamed_TextBox_1917977794432.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977794432.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977790592
        Me.unnamed_ComboBox_1917977790592.Name = "unnamed_ComboBox_1917977790592"
        Me.unnamed_ComboBox_1917977790592.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977790592.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(136, 132)
        Me.lbl_EXPLANATION2.Size = New System.Drawing.Size(393, 18)
        Me.lbl_EXPLANATION2.Text = "   仕訳ﾃﾞｰﾀを作成します。部分出力以外に使用しないでください。"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION2)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(7, 7)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 30)
        Me.cmd_実行.Text = "実行(&R)"
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(90, 7)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 30)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_YMD_01
        Me.txt_YMD_01.Name = "txt_YMD_01"
        Me.txt_YMD_01.Location = New System.Drawing.Point(136, 56)
        Me.txt_YMD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_01.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_YMD_01)

        ' lbl_YMD_01
        Me.lbl_YMD_01.Name = "lbl_YMD_01"
        Me.lbl_YMD_01.Location = New System.Drawing.Point(26, 56)
        Me.lbl_YMD_01.Size = New System.Drawing.Size(109, 18)
        Me.lbl_YMD_01.Text = "処理年月"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_01)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(136, 113)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(393, 18)
        Me.lbl_EXPLANATION1.Text = "※月次支払照合ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_出力元の抽出
        Me.lbl_出力元の抽出.Name = "lbl_出力元の抽出"
        Me.lbl_出力元の抽出.Location = New System.Drawing.Point(26, 86)
        Me.lbl_出力元の抽出.Size = New System.Drawing.Size(109, 18)
        Me.lbl_出力元の抽出.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_出力元の抽出)

        ' chk_CHK_01
        Me.chk_CHK_01.Name = "chk_CHK_01"
        Me.chk_CHK_01.Location = New System.Drawing.Point(154, 90)
        Me.chk_CHK_01.Size = New System.Drawing.Size(120, 11)
        Me.chk_CHK_01.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_CHK_01)

        ' lbl_CHK_01
        Me.lbl_CHK_01.Name = "lbl_CHK_01"
        Me.lbl_CHK_01.Location = New System.Drawing.Point(170, 86)
        Me.lbl_CHK_01.Size = New System.Drawing.Size(113, 15)
        Me.lbl_CHK_01.Text = "検索条件を加味する"
        Me.chk_CHK_01.Controls.Add(Me.lbl_CHK_01)

        ' txt_KAMOKU_CD_01
        Me.txt_KAMOKU_CD_01.Name = "txt_KAMOKU_CD_01"
        Me.txt_KAMOKU_CD_01.Location = New System.Drawing.Point(249, 192)
        Me.txt_KAMOKU_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_01.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_01)

        ' lbl_KAMOKU_CD_01
        Me.lbl_KAMOKU_CD_01.Name = "lbl_KAMOKU_CD_01"
        Me.lbl_KAMOKU_CD_01.Location = New System.Drawing.Point(26, 222)
        Me.lbl_KAMOKU_CD_01.Size = New System.Drawing.Size(109, 18)
        Me.lbl_KAMOKU_CD_01.Text = "部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_01)

        ' txt_BUMON_NM_01
        Me.txt_BUMON_NM_01.Name = "txt_BUMON_NM_01"
        Me.txt_BUMON_NM_01.Location = New System.Drawing.Point(230, 222)
        Me.txt_BUMON_NM_01.Size = New System.Drawing.Size(226, 18)
        Me.txt_BUMON_NM_01.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_BUMON_NM_01)

        ' lbl_科目ｺｰﾄﾞ
        Me.lbl_科目ｺｰﾄﾞ.Name = "lbl_科目ｺｰﾄﾞ"
        Me.lbl_科目ｺｰﾄﾞ.Location = New System.Drawing.Point(26, 192)
        Me.lbl_科目ｺｰﾄﾞ.Size = New System.Drawing.Size(109, 18)
        Me.lbl_科目ｺｰﾄﾞ.Text = "科目ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_科目ｺｰﾄﾞ)

        ' txt_YMD_02
        Me.txt_YMD_02.Name = "txt_YMD_02"
        Me.txt_YMD_02.Location = New System.Drawing.Point(136, 162)
        Me.txt_YMD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_YMD_02.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_YMD_02)

        ' lbl_伝票日付
        Me.lbl_伝票日付.Name = "lbl_伝票日付"
        Me.lbl_伝票日付.Location = New System.Drawing.Point(26, 162)
        Me.lbl_伝票日付.Size = New System.Drawing.Size(109, 18)
        Me.lbl_伝票日付.Text = "伝票日付"
        Me.pnlDetail.Controls.Add(Me.lbl_伝票日付)

        ' cmb_BUMON_CD_01
        Me.cmb_BUMON_CD_01.Name = "cmb_BUMON_CD_01"
        Me.cmb_BUMON_CD_01.Location = New System.Drawing.Point(136, 222)
        Me.cmb_BUMON_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.cmb_BUMON_CD_01.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmb_BUMON_CD_01)

        ' lbl_KAMOKU_CD_02
        Me.lbl_KAMOKU_CD_02.Name = "lbl_KAMOKU_CD_02"
        Me.lbl_KAMOKU_CD_02.Location = New System.Drawing.Point(136, 192)
        Me.lbl_KAMOKU_CD_02.Size = New System.Drawing.Size(113, 18)
        Me.lbl_KAMOKU_CD_02.Text = "未払金･未決勘定"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_02)

        ' txt_KAMOKU_CD_02
        Me.txt_KAMOKU_CD_02.Name = "txt_KAMOKU_CD_02"
        Me.txt_KAMOKU_CD_02.Location = New System.Drawing.Point(457, 192)
        Me.txt_KAMOKU_CD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_02.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_02)

        ' ラベル75
        Me.ラベル75.Name = "ラベル75"
        Me.ラベル75.Location = New System.Drawing.Point(343, 192)
        Me.ラベル75.Size = New System.Drawing.Size(113, 18)
        Me.ラベル75.Text = "未払金･営業外費用"
        Me.pnlDetail.Controls.Add(Me.ラベル75)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(136, 283)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(378, 30)
        Me.txt_TEXT_01.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' ラベル515
        Me.ラベル515.Name = "ラベル515"
        Me.ラベル515.Location = New System.Drawing.Point(26, 283)
        Me.ラベル515.Size = New System.Drawing.Size(109, 30)
        Me.ラベル515.Text = "出力先フォルダ"
        Me.pnlDetail.Controls.Add(Me.ラベル515)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(517, 283)
        Me.cmd_選択.Size = New System.Drawing.Size(52, 30)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 13
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' txt_KAMOKU_CD_03
        Me.txt_KAMOKU_CD_03.Name = "txt_KAMOKU_CD_03"
        Me.txt_KAMOKU_CD_03.Location = New System.Drawing.Point(166, 253)
        Me.txt_KAMOKU_CD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_03.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_03)

        ' ラベル78
        Me.ラベル78.Name = "ラベル78"
        Me.ラベル78.Location = New System.Drawing.Point(26, 253)
        Me.ラベル78.Size = New System.Drawing.Size(109, 18)
        Me.ラベル78.Text = "ｾｸﾞﾒﾝﾄ(共通)"
        Me.pnlDetail.Controls.Add(Me.ラベル78)

        ' ラベル79
        Me.ラベル79.Name = "ラベル79"
        Me.ラベル79.Location = New System.Drawing.Point(136, 253)
        Me.ラベル79.Size = New System.Drawing.Size(30, 18)
        Me.ラベル79.Text = "CD1"
        Me.pnlDetail.Controls.Add(Me.ラベル79)

        ' txt_KAMOKU_CD_04
        Me.txt_KAMOKU_CD_04.Name = "txt_KAMOKU_CD_04"
        Me.txt_KAMOKU_CD_04.Location = New System.Drawing.Point(415, 253)
        Me.txt_KAMOKU_CD_04.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_04.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_04)

        ' ラベル81
        Me.ラベル81.Name = "ラベル81"
        Me.ラベル81.Location = New System.Drawing.Point(385, 253)
        Me.ラベル81.Size = New System.Drawing.Size(30, 18)
        Me.ラベル81.Text = "CD2"
        Me.pnlDetail.Controls.Add(Me.ラベル81)

        ' txt_KAMOKU_NM_03
        Me.txt_KAMOKU_NM_03.Name = "txt_KAMOKU_NM_03"
        Me.txt_KAMOKU_NM_03.Location = New System.Drawing.Point(291, 253)
        Me.txt_KAMOKU_NM_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_NM_03.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_NM_03)

        ' ラベル83
        Me.ラベル83.Name = "ラベル83"
        Me.ラベル83.Location = New System.Drawing.Point(260, 253)
        Me.ラベル83.Size = New System.Drawing.Size(30, 18)
        Me.ラベル83.Text = "名1"
        Me.pnlDetail.Controls.Add(Me.ラベル83)

        Me.Name = "Frmfc_JOT_支払仕訳"
        Me.Text = "月次支払照合ﾌﾚｯｸｽ － 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977792640 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977795264 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977795392 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977794432 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977790592 As System.Windows.Forms.ComboBox
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
    Friend WithEvents txt_KAMOKU_CD_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_KAMOKU_CD_01 As System.Windows.Forms.Label
    Friend WithEvents txt_BUMON_NM_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_科目ｺｰﾄﾞ As System.Windows.Forms.Label
    Friend WithEvents txt_YMD_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_伝票日付 As System.Windows.Forms.Label
    Friend WithEvents cmb_BUMON_CD_01 As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_KAMOKU_CD_02 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_02 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル75 As System.Windows.Forms.Label
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル515 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents txt_KAMOKU_CD_03 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル78 As System.Windows.Forms.Label
    Friend WithEvents ラベル79 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_04 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル81 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_NM_03 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル83 As System.Windows.Forms.Label

End Class
