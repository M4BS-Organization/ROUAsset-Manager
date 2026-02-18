<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_支払仕訳_YAMASHIN
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
        Me.unnamed_Label_1917977805632 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970507584 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970509120 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977391808 = New System.Windows.Forms.TextBox()
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
        Me.txt_KAMOKU_CD_02 = New System.Windows.Forms.TextBox()
        Me.txt_YMD_02 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_02 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION3 = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_CD_01 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_01 = New System.Windows.Forms.TextBox()
        Me.lbl_KAMOKU_CD_02 = New System.Windows.Forms.Label()
        Me.lbl_ﾌｧｲﾙ名_資産_返済 = New System.Windows.Forms.Label()
        Me.txt_TEXT_02 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_02 = New System.Windows.Forms.Label()
        Me.cmd_選択2 = New System.Windows.Forms.Button()
        Me.txt_KAMOKU_CD_03 = New System.Windows.Forms.TextBox()
        Me.lbl_KAMOKU_CD_03 = New System.Windows.Forms.Label()
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.lbl_TEXT_01 = New System.Windows.Forms.Label()
        Me.cmd_選択1 = New System.Windows.Forms.Button()
        Me.lbl_ﾌｧｲﾙ名_費用_支払 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_支払仕訳_YAMASHIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977805632)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970507584)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970509120)
        Me.Controls.Add(Me.unnamed_TextBox_1917977391808)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977805632
        Me.unnamed_Label_1917977805632.Name = "unnamed_Label_1917977805632"
        Me.unnamed_Label_1917977805632.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977805632.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970507584
        Me.unnamed_CommandButton_1917970507584.Name = "unnamed_CommandButton_1917970507584"
        Me.unnamed_CommandButton_1917970507584.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970507584.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970509120
        Me.unnamed_CheckBox_1917970509120.Name = "unnamed_CheckBox_1917970509120"
        Me.unnamed_CheckBox_1917970509120.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970509120.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977391808
        Me.unnamed_TextBox_1917977391808.Name = "unnamed_TextBox_1917977391808"
        Me.unnamed_TextBox_1917977391808.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977391808.Size = New System.Drawing.Size(113, 26)

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
        Me.lbl_EXPLANATION1.Text = "※月次支払照合ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
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

        ' txt_KAMOKU_CD_02
        Me.txt_KAMOKU_CD_02.Name = "txt_KAMOKU_CD_02"
        Me.txt_KAMOKU_CD_02.Location = New System.Drawing.Point(143, 238)
        Me.txt_KAMOKU_CD_02.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_02.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_02)

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

        ' lbl_EXPLANATION3
        Me.lbl_EXPLANATION3.Name = "lbl_EXPLANATION3"
        Me.lbl_EXPLANATION3.Location = New System.Drawing.Point(143, 173)
        Me.lbl_EXPLANATION3.Size = New System.Drawing.Size(177, 18)
        Me.lbl_EXPLANATION3.Text = "※未払計上時の伝票日付"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION3)

        ' lbl_KAMOKU_CD_01
        Me.lbl_KAMOKU_CD_01.Name = "lbl_KAMOKU_CD_01"
        Me.lbl_KAMOKU_CD_01.Location = New System.Drawing.Point(26, 204)
        Me.lbl_KAMOKU_CD_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_KAMOKU_CD_01.Text = "部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_01)

        ' txt_KAMOKU_CD_01
        Me.txt_KAMOKU_CD_01.Name = "txt_KAMOKU_CD_01"
        Me.txt_KAMOKU_CD_01.Location = New System.Drawing.Point(143, 204)
        Me.txt_KAMOKU_CD_01.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_01.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_01)

        ' lbl_KAMOKU_CD_02
        Me.lbl_KAMOKU_CD_02.Name = "lbl_KAMOKU_CD_02"
        Me.lbl_KAMOKU_CD_02.Location = New System.Drawing.Point(26, 238)
        Me.lbl_KAMOKU_CD_02.Size = New System.Drawing.Size(117, 18)
        Me.lbl_KAMOKU_CD_02.Text = "科目ｺｰﾄﾞ(未払金)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_02)

        ' lbl_ﾌｧｲﾙ名_資産_返済
        Me.lbl_ﾌｧｲﾙ名_資産_返済.Name = "lbl_ﾌｧｲﾙ名_資産_返済"
        Me.lbl_ﾌｧｲﾙ名_資産_返済.Location = New System.Drawing.Point(41, 317)
        Me.lbl_ﾌｧｲﾙ名_資産_返済.Size = New System.Drawing.Size(321, 18)
        Me.lbl_ﾌｧｲﾙ名_資産_返済.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_返済.00002.dat"
        Me.pnlDetail.Controls.Add(Me.lbl_ﾌｧｲﾙ名_資産_返済)

        ' txt_TEXT_02
        Me.txt_TEXT_02.Name = "txt_TEXT_02"
        Me.txt_TEXT_02.Location = New System.Drawing.Point(143, 366)
        Me.txt_TEXT_02.Size = New System.Drawing.Size(321, 18)
        Me.txt_TEXT_02.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_02)

        ' lbl_TEXT_02
        Me.lbl_TEXT_02.Name = "lbl_TEXT_02"
        Me.lbl_TEXT_02.Location = New System.Drawing.Point(26, 366)
        Me.lbl_TEXT_02.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_02.Text = "出力先EXCELﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_02)

        ' cmd_選択2
        Me.cmd_選択2.Name = "cmd_選択2"
        Me.cmd_選択2.Location = New System.Drawing.Point(468, 366)
        Me.cmd_選択2.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択2.Text = "選択(&S)"
        Me.cmd_選択2.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.cmd_選択2)

        ' txt_KAMOKU_CD_03
        Me.txt_KAMOKU_CD_03.Name = "txt_KAMOKU_CD_03"
        Me.txt_KAMOKU_CD_03.Location = New System.Drawing.Point(143, 257)
        Me.txt_KAMOKU_CD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_03.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_03)

        ' lbl_KAMOKU_CD_03
        Me.lbl_KAMOKU_CD_03.Name = "lbl_KAMOKU_CD_03"
        Me.lbl_KAMOKU_CD_03.Location = New System.Drawing.Point(26, 257)
        Me.lbl_KAMOKU_CD_03.Size = New System.Drawing.Size(117, 18)
        Me.lbl_KAMOKU_CD_03.Text = "科目ｺｰﾄﾞ(仮払金)"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_03)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(143, 287)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(321, 18)
        Me.txt_TEXT_01.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' lbl_TEXT_01
        Me.lbl_TEXT_01.Name = "lbl_TEXT_01"
        Me.lbl_TEXT_01.Location = New System.Drawing.Point(26, 287)
        Me.lbl_TEXT_01.Size = New System.Drawing.Size(117, 18)
        Me.lbl_TEXT_01.Text = "出力先ﾌｫﾙﾀﾞ名"
        Me.pnlDetail.Controls.Add(Me.lbl_TEXT_01)

        ' cmd_選択1
        Me.cmd_選択1.Name = "cmd_選択1"
        Me.cmd_選択1.Location = New System.Drawing.Point(468, 287)
        Me.cmd_選択1.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択1.Text = "選択(&F)"
        Me.cmd_選択1.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.cmd_選択1)

        ' lbl_ﾌｧｲﾙ名_費用_支払
        Me.lbl_ﾌｧｲﾙ名_費用_支払.Name = "lbl_ﾌｧｲﾙ名_費用_支払"
        Me.lbl_ﾌｧｲﾙ名_費用_支払.Location = New System.Drawing.Point(41, 336)
        Me.lbl_ﾌｧｲﾙ名_費用_支払.Size = New System.Drawing.Size(321, 18)
        Me.lbl_ﾌｧｲﾙ名_費用_支払.Text = "M4BS_YYYYMMDD_HHMM_MM月_費用_支払.00002.dat"
        Me.pnlDetail.Controls.Add(Me.lbl_ﾌｧｲﾙ名_費用_支払)

        Me.Name = "Frmfc_支払仕訳_YAMASHIN"
        Me.Text = "月次支払照合ﾌﾚｯｸｽ － 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977805632 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970507584 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970509120 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977391808 As System.Windows.Forms.TextBox
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
    Friend WithEvents txt_KAMOKU_CD_02 As System.Windows.Forms.TextBox
    Friend WithEvents txt_YMD_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_02 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION3 As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_CD_01 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_KAMOKU_CD_02 As System.Windows.Forms.Label
    Friend WithEvents lbl_ﾌｧｲﾙ名_資産_返済 As System.Windows.Forms.Label
    Friend WithEvents txt_TEXT_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_02 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択2 As System.Windows.Forms.Button
    Friend WithEvents txt_KAMOKU_CD_03 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_KAMOKU_CD_03 As System.Windows.Forms.Label
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_TEXT_01 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択1 As System.Windows.Forms.Button
    Friend WithEvents lbl_ﾌｧｲﾙ名_費用_支払 As System.Windows.Forms.Label

End Class
