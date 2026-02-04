<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_計上仕訳_VTC
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
        Me.unnamed_Label_1917970215488 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970203200 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917970212800 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917970208896 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_処理年月 = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_DT = New System.Windows.Forms.Label()
        Me.txt_前払費用_科目CD = New System.Windows.Forms.TextBox()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_TITL = New System.Windows.Forms.Label()
        Me.chk_検索条件加味F = New System.Windows.Forms.CheckBox()
        Me.ラベル22 = New System.Windows.Forms.Label()
        Me.ラベル23 = New System.Windows.Forms.Label()
        Me.ラベル24 = New System.Windows.Forms.Label()
        Me.ラベル25 = New System.Windows.Forms.Label()
        Me.ラベル26 = New System.Windows.Forms.Label()
        Me.ラベル27 = New System.Windows.Forms.Label()
        Me.ラベル28 = New System.Windows.Forms.Label()
        Me.ラベル29 = New System.Windows.Forms.Label()
        Me.ラベル30 = New System.Windows.Forms.Label()
        Me.ラベル31 = New System.Windows.Forms.Label()
        Me.ラベル32 = New System.Windows.Forms.Label()
        Me.ラベル33 = New System.Windows.Forms.Label()
        Me.ラベル34 = New System.Windows.Forms.Label()
        Me.ラベル35 = New System.Windows.Forms.Label()
        Me.ラベル38 = New System.Windows.Forms.Label()
        Me.ラベル39 = New System.Windows.Forms.Label()
        Me.ラベル40 = New System.Windows.Forms.Label()
        Me.ラベル41 = New System.Windows.Forms.Label()
        Me.ラベル42 = New System.Windows.Forms.Label()
        Me.ラベル43 = New System.Windows.Forms.Label()
        Me.ラベル45 = New System.Windows.Forms.Label()
        Me.txt_減損勘定償却引当金1_科目CD = New System.Windows.Forms.TextBox()
        Me.txt_減損勘定償却引当金2_科目CD = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' Frmfc_計上仕訳_VTC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970215488)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970203200)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970212800)
        Me.Controls.Add(Me.unnamed_TextBox_1917970208896)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970215488
        Me.unnamed_Label_1917970215488.Name = "unnamed_Label_1917970215488"
        Me.unnamed_Label_1917970215488.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970215488.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970203200
        Me.unnamed_CommandButton_1917970203200.Name = "unnamed_CommandButton_1917970203200"
        Me.unnamed_CommandButton_1917970203200.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970203200.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917970212800
        Me.unnamed_CheckBox_1917970212800.Name = "unnamed_CheckBox_1917970212800"
        Me.unnamed_CheckBox_1917970212800.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970212800.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917970208896
        Me.unnamed_TextBox_1917970208896.Name = "unnamed_TextBox_1917970208896"
        Me.unnamed_TextBox_1917970208896.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970208896.Size = New System.Drawing.Size(113, 26)

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

        ' txt_処理年月
        Me.txt_処理年月.Name = "txt_処理年月"
        Me.txt_処理年月.Location = New System.Drawing.Point(136, 45)
        Me.txt_処理年月.Size = New System.Drawing.Size(78, 18)
        Me.txt_処理年月.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_処理年月)

        ' lbl_SLIP_DT
        Me.lbl_SLIP_DT.Name = "lbl_SLIP_DT"
        Me.lbl_SLIP_DT.Location = New System.Drawing.Point(26, 45)
        Me.lbl_SLIP_DT.Size = New System.Drawing.Size(109, 18)
        Me.lbl_SLIP_DT.Text = "処理年月"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_DT)

        ' txt_前払費用_科目CD
        Me.txt_前払費用_科目CD.Name = "txt_前払費用_科目CD"
        Me.txt_前払費用_科目CD.Location = New System.Drawing.Point(404, 230)
        Me.txt_前払費用_科目CD.Size = New System.Drawing.Size(56, 15)
        Me.txt_前払費用_科目CD.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_前払費用_科目CD)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(124, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(366, 18)
        Me.lbl_EXPLANATION1.Text = "※月次仕訳計上ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_TITL
        Me.lbl_TITL.Name = "lbl_TITL"
        Me.lbl_TITL.Location = New System.Drawing.Point(26, 75)
        Me.lbl_TITL.Size = New System.Drawing.Size(109, 18)
        Me.lbl_TITL.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_TITL)

        ' chk_検索条件加味F
        Me.chk_検索条件加味F.Name = "chk_検索条件加味F"
        Me.chk_検索条件加味F.Location = New System.Drawing.Point(154, 79)
        Me.chk_検索条件加味F.Size = New System.Drawing.Size(120, 18)
        Me.chk_検索条件加味F.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_検索条件加味F)

        ' ラベル22
        Me.ラベル22.Name = "ラベル22"
        Me.ラベル22.Location = New System.Drawing.Point(170, 77)
        Me.ラベル22.Size = New System.Drawing.Size(114, 15)
        Me.ラベル22.Text = "検索条件を加味する"
        Me.chk_検索条件加味F.Controls.Add(Me.ラベル22)

        ' ラベル23
        Me.ラベル23.Name = "ラベル23"
        Me.ラベル23.Location = New System.Drawing.Point(22, 151)
        Me.ラベル23.Size = New System.Drawing.Size(185, 15)
        Me.ラベル23.Text = "本機能で出力する仕訳データ"
        Me.pnlDetail.Controls.Add(Me.ラベル23)

        ' ラベル24
        Me.ラベル24.Name = "ラベル24"
        Me.ラベル24.Location = New System.Drawing.Point(37, 170)
        Me.ラベル24.Size = New System.Drawing.Size(378, 15)
        Me.ラベル24.Text = "(1) 所有権移転外ﾌｧｲﾅﾝｽ･ﾘｰｽ　売買取引"
        Me.pnlDetail.Controls.Add(Me.ラベル24)

        ' ラベル25
        Me.ラベル25.Name = "ラベル25"
        Me.ラベル25.Location = New System.Drawing.Point(90, 192)
        Me.ラベル25.Size = New System.Drawing.Size(49, 15)
        Me.ラベル25.Text = "<借方>"
        Me.pnlDetail.Controls.Add(Me.ラベル25)

        ' ラベル26
        Me.ラベル26.Name = "ラベル26"
        Me.ラベル26.Location = New System.Drawing.Point(302, 192)
        Me.ラベル26.Size = New System.Drawing.Size(49, 15)
        Me.ラベル26.Text = "<貸方>"
        Me.pnlDetail.Controls.Add(Me.ラベル26)

        ' ラベル27
        Me.ラベル27.Name = "ラベル27"
        Me.ラベル27.Location = New System.Drawing.Point(90, 211)
        Me.ラベル27.Size = New System.Drawing.Size(94, 15)
        Me.ラベル27.Text = "減価償却費"
        Me.pnlDetail.Controls.Add(Me.ラベル27)

        ' ラベル28
        Me.ラベル28.Name = "ラベル28"
        Me.ラベル28.Location = New System.Drawing.Point(302, 211)
        Me.ラベル28.Size = New System.Drawing.Size(94, 15)
        Me.ラベル28.Text = "減価償却累計額"
        Me.pnlDetail.Controls.Add(Me.ラベル28)

        ' ラベル29
        Me.ラベル29.Name = "ラベル29"
        Me.ラベル29.Location = New System.Drawing.Point(90, 230)
        Me.ラベル29.Size = New System.Drawing.Size(94, 15)
        Me.ラベル29.Text = "支払利息"
        Me.pnlDetail.Controls.Add(Me.ラベル29)

        ' ラベル30
        Me.ラベル30.Name = "ラベル30"
        Me.ラベル30.Location = New System.Drawing.Point(302, 230)
        Me.ラベル30.Size = New System.Drawing.Size(94, 15)
        Me.ラベル30.Text = "前払費用"
        Me.pnlDetail.Controls.Add(Me.ラベル30)

        ' ラベル31
        Me.ラベル31.Name = "ラベル31"
        Me.ラベル31.Location = New System.Drawing.Point(37, 260)
        Me.ラベル31.Size = New System.Drawing.Size(378, 15)
        Me.ラベル31.Text = "(2) 所有権移転外ﾌｧｲﾅﾝｽ･ﾘｰｽ　賃貸借取引　会計基準適用後契約"
        Me.pnlDetail.Controls.Add(Me.ラベル31)

        ' ラベル32
        Me.ラベル32.Name = "ラベル32"
        Me.ラベル32.Location = New System.Drawing.Point(90, 283)
        Me.ラベル32.Size = New System.Drawing.Size(49, 15)
        Me.ラベル32.Text = "<借方>"
        Me.pnlDetail.Controls.Add(Me.ラベル32)

        ' ラベル33
        Me.ラベル33.Name = "ラベル33"
        Me.ラベル33.Location = New System.Drawing.Point(302, 283)
        Me.ラベル33.Size = New System.Drawing.Size(49, 15)
        Me.ラベル33.Text = "<貸方>"
        Me.pnlDetail.Controls.Add(Me.ラベル33)

        ' ラベル34
        Me.ラベル34.Name = "ラベル34"
        Me.ラベル34.Location = New System.Drawing.Point(90, 302)
        Me.ラベル34.Size = New System.Drawing.Size(120, 15)
        Me.ラベル34.Text = "減損勘定償却引当金"
        Me.pnlDetail.Controls.Add(Me.ラベル34)

        ' ラベル35
        Me.ラベル35.Name = "ラベル35"
        Me.ラベル35.Location = New System.Drawing.Point(302, 302)
        Me.ラベル35.Size = New System.Drawing.Size(120, 15)
        Me.ラベル35.Text = "減損勘定償却額"
        Me.pnlDetail.Controls.Add(Me.ラベル35)

        ' ラベル38
        Me.ラベル38.Name = "ラベル38"
        Me.ラベル38.Location = New System.Drawing.Point(37, 332)
        Me.ラベル38.Size = New System.Drawing.Size(378, 15)
        Me.ラベル38.Text = "(3) 所有権移転外ﾌｧｲﾅﾝｽ･ﾘｰｽ　賃貸借取引　会計基準適用前契約"
        Me.pnlDetail.Controls.Add(Me.ラベル38)

        ' ラベル39
        Me.ラベル39.Name = "ラベル39"
        Me.ラベル39.Location = New System.Drawing.Point(90, 355)
        Me.ラベル39.Size = New System.Drawing.Size(49, 15)
        Me.ラベル39.Text = "<借方>"
        Me.pnlDetail.Controls.Add(Me.ラベル39)

        ' ラベル40
        Me.ラベル40.Name = "ラベル40"
        Me.ラベル40.Location = New System.Drawing.Point(302, 355)
        Me.ラベル40.Size = New System.Drawing.Size(49, 15)
        Me.ラベル40.Text = "<貸方>"
        Me.pnlDetail.Controls.Add(Me.ラベル40)

        ' ラベル41
        Me.ラベル41.Name = "ラベル41"
        Me.ラベル41.Location = New System.Drawing.Point(90, 374)
        Me.ラベル41.Size = New System.Drawing.Size(120, 15)
        Me.ラベル41.Text = "減損勘定償却引当金"
        Me.pnlDetail.Controls.Add(Me.ラベル41)

        ' ラベル42
        Me.ラベル42.Name = "ラベル42"
        Me.ラベル42.Location = New System.Drawing.Point(302, 374)
        Me.ラベル42.Size = New System.Drawing.Size(120, 15)
        Me.ラベル42.Text = "減損勘定償却額"
        Me.pnlDetail.Controls.Add(Me.ラベル42)

        ' ラベル43
        Me.ラベル43.Name = "ラベル43"
        Me.ラベル43.Location = New System.Drawing.Point(37, 404)
        Me.ラベル43.Size = New System.Drawing.Size(378, 15)
        Me.ラベル43.Text = "(4) ｵﾍﾟﾚｰﾃｨﾝｸﾞ･ﾘｰｽ、保守料、その他"
        Me.pnlDetail.Controls.Add(Me.ラベル43)

        ' ラベル45
        Me.ラベル45.Name = "ラベル45"
        Me.ラベル45.Location = New System.Drawing.Point(90, 427)
        Me.ラベル45.Size = New System.Drawing.Size(49, 15)
        Me.ラベル45.Text = "なし"
        Me.pnlDetail.Controls.Add(Me.ラベル45)

        ' txt_減損勘定償却引当金1_科目CD
        Me.txt_減損勘定償却引当金1_科目CD.Name = "txt_減損勘定償却引当金1_科目CD"
        Me.txt_減損勘定償却引当金1_科目CD.Location = New System.Drawing.Point(215, 302)
        Me.txt_減損勘定償却引当金1_科目CD.Size = New System.Drawing.Size(56, 15)
        Me.txt_減損勘定償却引当金1_科目CD.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_減損勘定償却引当金1_科目CD)

        ' txt_減損勘定償却引当金2_科目CD
        Me.txt_減損勘定償却引当金2_科目CD.Name = "txt_減損勘定償却引当金2_科目CD"
        Me.txt_減損勘定償却引当金2_科目CD.Location = New System.Drawing.Point(215, 374)
        Me.txt_減損勘定償却引当金2_科目CD.Size = New System.Drawing.Size(56, 15)
        Me.txt_減損勘定償却引当金2_科目CD.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_減損勘定償却引当金2_科目CD)

        Me.Name = "Frmfc_計上仕訳_VTC"
        Me.Text = "月次仕訳計上ﾌﾚｯｸｽ - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970215488 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970203200 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917970212800 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917970208896 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_処理年月 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_DT As System.Windows.Forms.Label
    Friend WithEvents txt_前払費用_科目CD As System.Windows.Forms.TextBox
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_TITL As System.Windows.Forms.Label
    Friend WithEvents chk_検索条件加味F As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル22 As System.Windows.Forms.Label
    Friend WithEvents ラベル23 As System.Windows.Forms.Label
    Friend WithEvents ラベル24 As System.Windows.Forms.Label
    Friend WithEvents ラベル25 As System.Windows.Forms.Label
    Friend WithEvents ラベル26 As System.Windows.Forms.Label
    Friend WithEvents ラベル27 As System.Windows.Forms.Label
    Friend WithEvents ラベル28 As System.Windows.Forms.Label
    Friend WithEvents ラベル29 As System.Windows.Forms.Label
    Friend WithEvents ラベル30 As System.Windows.Forms.Label
    Friend WithEvents ラベル31 As System.Windows.Forms.Label
    Friend WithEvents ラベル32 As System.Windows.Forms.Label
    Friend WithEvents ラベル33 As System.Windows.Forms.Label
    Friend WithEvents ラベル34 As System.Windows.Forms.Label
    Friend WithEvents ラベル35 As System.Windows.Forms.Label
    Friend WithEvents ラベル38 As System.Windows.Forms.Label
    Friend WithEvents ラベル39 As System.Windows.Forms.Label
    Friend WithEvents ラベル40 As System.Windows.Forms.Label
    Friend WithEvents ラベル41 As System.Windows.Forms.Label
    Friend WithEvents ラベル42 As System.Windows.Forms.Label
    Friend WithEvents ラベル43 As System.Windows.Forms.Label
    Friend WithEvents ラベル45 As System.Windows.Forms.Label
    Friend WithEvents txt_減損勘定償却引当金1_科目CD As System.Windows.Forms.TextBox
    Friend WithEvents txt_減損勘定償却引当金2_科目CD As System.Windows.Forms.TextBox

End Class
