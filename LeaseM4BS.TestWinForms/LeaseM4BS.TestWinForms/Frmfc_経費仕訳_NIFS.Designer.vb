<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_経費仕訳_NIFS
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
        Me.unnamed_Label_1917970496256 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977392896 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970490112 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970491520 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_SLIP_DT = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_DT = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_出力元の抽出 = New System.Windows.Forms.Label()
        Me.chk_検索条件加味F = New System.Windows.Forms.CheckBox()
        Me.lbl_検索条件加味F = New System.Windows.Forms.Label()
        Me.txt_部署コード_一括 = New System.Windows.Forms.TextBox()
        Me.ラベル51 = New System.Windows.Forms.Label()
        Me.txt_長期前払費用 = New System.Windows.Forms.TextBox()
        Me.lbl_未払費用 = New System.Windows.Forms.Label()
        Me.ラベル68 = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FPATH = New System.Windows.Forms.TextBox()
        Me.lbl_OUTPUT_FPATH = New System.Windows.Forms.Label()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.ラベル62 = New System.Windows.Forms.Label()
        Me.ラベル63 = New System.Windows.Forms.Label()
        Me.txt_販管費リース料 = New System.Windows.Forms.TextBox()
        Me.ラベル65 = New System.Windows.Forms.Label()
        Me.chk_長期 = New System.Windows.Forms.CheckBox()
        Me.ラベル67 = New System.Windows.Forms.Label()
        Me.chk_販管費 = New System.Windows.Forms.CheckBox()
        Me.ラベル69 = New System.Windows.Forms.Label()
        Me.lbl_KAMOKU_CD_03 = New System.Windows.Forms.Label()
        Me.txt_KAMOKU_CD_03 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' Frmfc_経費仕訳_NIFS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970496256)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977392896)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970490112)
        Me.Controls.Add(Me.unnamed_TextBox_1917970491520)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970496256
        Me.unnamed_Label_1917970496256.Name = "unnamed_Label_1917970496256"
        Me.unnamed_Label_1917970496256.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970496256.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977392896
        Me.unnamed_CommandButton_1917977392896.Name = "unnamed_CommandButton_1917977392896"
        Me.unnamed_CommandButton_1917977392896.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977392896.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970490112
        Me.unnamed_CheckBox_1917970490112.Name = "unnamed_CheckBox_1917970490112"
        Me.unnamed_CheckBox_1917970490112.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970490112.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970491520
        Me.unnamed_TextBox_1917970491520.Name = "unnamed_TextBox_1917970491520"
        Me.unnamed_TextBox_1917970491520.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970491520.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(124, 120)
        Me.lbl_EXPLANATION2.Size = New System.Drawing.Size(366, 18)
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
        Me.cmd_CANCEL.Location = New System.Drawing.Point(86, 3)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' txt_SLIP_DT
        Me.txt_SLIP_DT.Name = "txt_SLIP_DT"
        Me.txt_SLIP_DT.Location = New System.Drawing.Point(136, 45)
        Me.txt_SLIP_DT.Size = New System.Drawing.Size(94, 18)
        Me.txt_SLIP_DT.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_SLIP_DT)

        ' lbl_SLIP_DT
        Me.lbl_SLIP_DT.Name = "lbl_SLIP_DT"
        Me.lbl_SLIP_DT.Location = New System.Drawing.Point(26, 45)
        Me.lbl_SLIP_DT.Size = New System.Drawing.Size(109, 18)
        Me.lbl_SLIP_DT.Text = "対象月"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_DT)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(124, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(366, 18)
        Me.lbl_EXPLANATION1.Text = "※経費明細表ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_出力元の抽出
        Me.lbl_出力元の抽出.Name = "lbl_出力元の抽出"
        Me.lbl_出力元の抽出.Location = New System.Drawing.Point(26, 75)
        Me.lbl_出力元の抽出.Size = New System.Drawing.Size(109, 18)
        Me.lbl_出力元の抽出.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_出力元の抽出)

        ' chk_検索条件加味F
        Me.chk_検索条件加味F.Name = "chk_検索条件加味F"
        Me.chk_検索条件加味F.Location = New System.Drawing.Point(154, 79)
        Me.chk_検索条件加味F.Size = New System.Drawing.Size(120, 11)
        Me.chk_検索条件加味F.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_検索条件加味F)

        ' lbl_検索条件加味F
        Me.lbl_検索条件加味F.Name = "lbl_検索条件加味F"
        Me.lbl_検索条件加味F.Location = New System.Drawing.Point(170, 75)
        Me.lbl_検索条件加味F.Size = New System.Drawing.Size(113, 15)
        Me.lbl_検索条件加味F.Text = "検索条件を加味する"
        Me.chk_検索条件加味F.Controls.Add(Me.lbl_検索条件加味F)

        ' txt_部署コード_一括
        Me.txt_部署コード_一括.Name = "txt_部署コード_一括"
        Me.txt_部署コード_一括.Location = New System.Drawing.Point(132, 158)
        Me.txt_部署コード_一括.Size = New System.Drawing.Size(90, 18)
        Me.txt_部署コード_一括.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_部署コード_一括)

        ' ラベル51
        Me.ラベル51.Name = "ラベル51"
        Me.ラベル51.Location = New System.Drawing.Point(22, 158)
        Me.ラベル51.Size = New System.Drawing.Size(109, 18)
        Me.ラベル51.Text = "部署ｺｰﾄﾞ(一括)"
        Me.pnlDetail.Controls.Add(Me.ラベル51)

        ' txt_長期前払費用
        Me.txt_長期前払費用.Name = "txt_長期前払費用"
        Me.txt_長期前払費用.Location = New System.Drawing.Point(241, 188)
        Me.txt_長期前払費用.Size = New System.Drawing.Size(94, 18)
        Me.txt_長期前払費用.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_長期前払費用)

        ' lbl_未払費用
        Me.lbl_未払費用.Name = "lbl_未払費用"
        Me.lbl_未払費用.Location = New System.Drawing.Point(22, 188)
        Me.lbl_未払費用.Size = New System.Drawing.Size(109, 56)
        Me.lbl_未払費用.Text = "科目ｺｰﾄﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_未払費用)

        ' ラベル68
        Me.ラベル68.Name = "ラベル68"
        Me.ラベル68.Location = New System.Drawing.Point(22, 279)
        Me.ラベル68.Size = New System.Drawing.Size(109, 18)
        Me.ラベル68.Text = "通常ﾘｰｽ"
        Me.pnlDetail.Controls.Add(Me.ラベル68)

        ' txt_OUTPUT_FPATH
        Me.txt_OUTPUT_FPATH.Name = "txt_OUTPUT_FPATH"
        Me.txt_OUTPUT_FPATH.Location = New System.Drawing.Point(132, 343)
        Me.txt_OUTPUT_FPATH.Size = New System.Drawing.Size(378, 18)
        Me.txt_OUTPUT_FPATH.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FPATH)

        ' lbl_OUTPUT_FPATH
        Me.lbl_OUTPUT_FPATH.Name = "lbl_OUTPUT_FPATH"
        Me.lbl_OUTPUT_FPATH.Location = New System.Drawing.Point(22, 343)
        Me.lbl_OUTPUT_FPATH.Size = New System.Drawing.Size(109, 18)
        Me.lbl_OUTPUT_FPATH.Text = "出力先ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FPATH)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(510, 344)
        Me.cmd_選択.Size = New System.Drawing.Size(56, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' ラベル62
        Me.ラベル62.Name = "ラベル62"
        Me.ラベル62.Location = New System.Drawing.Point(132, 188)
        Me.ラベル62.Size = New System.Drawing.Size(109, 18)
        Me.ラベル62.Text = "長期前払費用(ﾘｰｽ)"
        Me.pnlDetail.Controls.Add(Me.ラベル62)

        ' ラベル63
        Me.ラベル63.Name = "ラベル63"
        Me.ラベル63.Location = New System.Drawing.Point(132, 207)
        Me.ラベル63.Size = New System.Drawing.Size(109, 18)
        Me.ラベル63.Text = "販管費ﾘｰｽ料"
        Me.pnlDetail.Controls.Add(Me.ラベル63)

        ' txt_販管費リース料
        Me.txt_販管費リース料.Name = "txt_販管費リース料"
        Me.txt_販管費リース料.Location = New System.Drawing.Point(241, 207)
        Me.txt_販管費リース料.Size = New System.Drawing.Size(94, 18)
        Me.txt_販管費リース料.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_販管費リース料)

        ' ラベル65
        Me.ラベル65.Name = "ラベル65"
        Me.ラベル65.Location = New System.Drawing.Point(22, 257)
        Me.ラベル65.Size = New System.Drawing.Size(109, 18)
        Me.ラベル65.Text = "出力対象仕訳"
        Me.pnlDetail.Controls.Add(Me.ラベル65)

        ' chk_長期
        Me.chk_長期.Name = "chk_長期"
        Me.chk_長期.Location = New System.Drawing.Point(154, 287)
        Me.chk_長期.Size = New System.Drawing.Size(120, 11)
        Me.chk_長期.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.chk_長期)

        ' ラベル67
        Me.ラベル67.Name = "ラベル67"
        Me.ラベル67.Location = New System.Drawing.Point(170, 283)
        Me.ラベル67.Size = New System.Drawing.Size(226, 15)
        Me.ラベル67.Text = "月次入力(長期前払費用→販管費ﾘｰｽ料)"
        Me.chk_長期.Controls.Add(Me.ラベル67)

        ' chk_販管費
        Me.chk_販管費.Name = "chk_販管費"
        Me.chk_販管費.Location = New System.Drawing.Point(154, 306)
        Me.chk_販管費.Size = New System.Drawing.Size(120, 11)
        Me.chk_販管費.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.chk_販管費)

        ' ラベル69
        Me.ラベル69.Name = "ラベル69"
        Me.ラベル69.Location = New System.Drawing.Point(170, 302)
        Me.ラベル69.Size = New System.Drawing.Size(226, 15)
        Me.ラベル69.Text = "月次入力(販管費ﾘｰｽ料→本来のﾘｰｽ料)"
        Me.chk_販管費.Controls.Add(Me.ラベル69)

        ' lbl_KAMOKU_CD_03
        Me.lbl_KAMOKU_CD_03.Name = "lbl_KAMOKU_CD_03"
        Me.lbl_KAMOKU_CD_03.Location = New System.Drawing.Point(132, 226)
        Me.lbl_KAMOKU_CD_03.Size = New System.Drawing.Size(109, 18)
        Me.lbl_KAMOKU_CD_03.Text = "転ﾘｰｽ原価"
        Me.pnlDetail.Controls.Add(Me.lbl_KAMOKU_CD_03)

        ' txt_KAMOKU_CD_03
        Me.txt_KAMOKU_CD_03.Name = "txt_KAMOKU_CD_03"
        Me.txt_KAMOKU_CD_03.Location = New System.Drawing.Point(241, 226)
        Me.txt_KAMOKU_CD_03.Size = New System.Drawing.Size(94, 18)
        Me.txt_KAMOKU_CD_03.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.txt_KAMOKU_CD_03)

        Me.Name = "Frmfc_経費仕訳_NIFS"
        Me.Text = "経費明細表 － 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970496256 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977392896 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970490112 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970491520 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_SLIP_DT As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_DT As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_出力元の抽出 As System.Windows.Forms.Label
    Friend WithEvents chk_検索条件加味F As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_検索条件加味F As System.Windows.Forms.Label
    Friend WithEvents txt_部署コード_一括 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル51 As System.Windows.Forms.Label
    Friend WithEvents txt_長期前払費用 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_未払費用 As System.Windows.Forms.Label
    Friend WithEvents ラベル68 As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FPATH As System.Windows.Forms.TextBox
    Friend WithEvents lbl_OUTPUT_FPATH As System.Windows.Forms.Label
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents ラベル62 As System.Windows.Forms.Label
    Friend WithEvents ラベル63 As System.Windows.Forms.Label
    Friend WithEvents txt_販管費リース料 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル65 As System.Windows.Forms.Label
    Friend WithEvents chk_長期 As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル67 As System.Windows.Forms.Label
    Friend WithEvents chk_販管費 As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル69 As System.Windows.Forms.Label
    Friend WithEvents lbl_KAMOKU_CD_03 As System.Windows.Forms.Label
    Friend WithEvents txt_KAMOKU_CD_03 As System.Windows.Forms.TextBox

End Class
