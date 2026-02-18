<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_計上仕訳_RISO
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
        Me.unnamed_Label_1917970473856 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970496256 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917978180480 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977631296 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_EXPLANATION2 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_出力年月 = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_DT = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION1 = New System.Windows.Forms.Label()
        Me.lbl_TITL = New System.Windows.Forms.Label()
        Me.chk_検索条件加味F = New System.Windows.Forms.CheckBox()
        Me.ラベル22 = New System.Windows.Forms.Label()
        Me.txt_伝票日付 = New System.Windows.Forms.TextBox()
        Me.ラベル49 = New System.Windows.Forms.Label()
        Me.txt_部署コード_一括用 = New System.Windows.Forms.TextBox()
        Me.ラベル51 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_計上仕訳_RISO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970473856)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970496256)
        Me.Controls.Add(Me.unnamed_CheckBox_1917978180480)
        Me.Controls.Add(Me.unnamed_TextBox_1917977631296)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970473856
        Me.unnamed_Label_1917970473856.Name = "unnamed_Label_1917970473856"
        Me.unnamed_Label_1917970473856.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970473856.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970496256
        Me.unnamed_CommandButton_1917970496256.Name = "unnamed_CommandButton_1917970496256"
        Me.unnamed_CommandButton_1917970496256.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970496256.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917978180480
        Me.unnamed_CheckBox_1917978180480.Name = "unnamed_CheckBox_1917978180480"
        Me.unnamed_CheckBox_1917978180480.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917978180480.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977631296
        Me.unnamed_TextBox_1917977631296.Name = "unnamed_TextBox_1917977631296"
        Me.unnamed_TextBox_1917977631296.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977631296.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_EXPLANATION2
        Me.lbl_EXPLANATION2.Name = "lbl_EXPLANATION2"
        Me.lbl_EXPLANATION2.Location = New System.Drawing.Point(143, 120)
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

        ' txt_出力年月
        Me.txt_出力年月.Name = "txt_出力年月"
        Me.txt_出力年月.Location = New System.Drawing.Point(139, 45)
        Me.txt_出力年月.Size = New System.Drawing.Size(78, 18)
        Me.txt_出力年月.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_出力年月)

        ' lbl_SLIP_DT
        Me.lbl_SLIP_DT.Name = "lbl_SLIP_DT"
        Me.lbl_SLIP_DT.Location = New System.Drawing.Point(26, 45)
        Me.lbl_SLIP_DT.Size = New System.Drawing.Size(113, 18)
        Me.lbl_SLIP_DT.Text = "出力年月"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_DT)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(143, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(366, 18)
        Me.lbl_EXPLANATION1.Text = "※月次仕訳計上ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_TITL
        Me.lbl_TITL.Name = "lbl_TITL"
        Me.lbl_TITL.Location = New System.Drawing.Point(26, 75)
        Me.lbl_TITL.Size = New System.Drawing.Size(113, 18)
        Me.lbl_TITL.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_TITL)

        ' chk_検索条件加味F
        Me.chk_検索条件加味F.Name = "chk_検索条件加味F"
        Me.chk_検索条件加味F.Location = New System.Drawing.Point(166, 77)
        Me.chk_検索条件加味F.Size = New System.Drawing.Size(120, 18)
        Me.chk_検索条件加味F.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_検索条件加味F)

        ' ラベル22
        Me.ラベル22.Name = "ラベル22"
        Me.ラベル22.Location = New System.Drawing.Point(181, 75)
        Me.ラベル22.Size = New System.Drawing.Size(114, 15)
        Me.ラベル22.Text = "検索条件を加味する"
        Me.chk_検索条件加味F.Controls.Add(Me.ラベル22)

        ' txt_伝票日付
        Me.txt_伝票日付.Name = "txt_伝票日付"
        Me.txt_伝票日付.Location = New System.Drawing.Point(139, 151)
        Me.txt_伝票日付.Size = New System.Drawing.Size(90, 18)
        Me.txt_伝票日付.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_伝票日付)

        ' ラベル49
        Me.ラベル49.Name = "ラベル49"
        Me.ラベル49.Location = New System.Drawing.Point(26, 151)
        Me.ラベル49.Size = New System.Drawing.Size(113, 18)
        Me.ラベル49.Text = "伝票日付"
        Me.pnlDetail.Controls.Add(Me.ラベル49)

        ' txt_部署コード_一括用
        Me.txt_部署コード_一括用.Name = "txt_部署コード_一括用"
        Me.txt_部署コード_一括用.Location = New System.Drawing.Point(139, 185)
        Me.txt_部署コード_一括用.Size = New System.Drawing.Size(90, 18)
        Me.txt_部署コード_一括用.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.txt_部署コード_一括用)

        ' ラベル51
        Me.ラベル51.Name = "ラベル51"
        Me.ラベル51.Location = New System.Drawing.Point(26, 185)
        Me.ラベル51.Size = New System.Drawing.Size(113, 18)
        Me.ラベル51.Text = "部署ｺｰﾄﾞ(一括用)"
        Me.pnlDetail.Controls.Add(Me.ラベル51)

        Me.Name = "Frmfc_計上仕訳_RISO"
        Me.Text = "月次仕訳計上ﾌﾚｯｸｽ - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970473856 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970496256 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917978180480 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977631296 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_EXPLANATION2 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_出力年月 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_DT As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION1 As System.Windows.Forms.Label
    Friend WithEvents lbl_TITL As System.Windows.Forms.Label
    Friend WithEvents chk_検索条件加味F As System.Windows.Forms.CheckBox
    Friend WithEvents ラベル22 As System.Windows.Forms.Label
    Friend WithEvents txt_伝票日付 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル49 As System.Windows.Forms.Label
    Friend WithEvents txt_部署コード_一括用 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル51 As System.Windows.Forms.Label

End Class
