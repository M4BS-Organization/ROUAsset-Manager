<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_計上仕訳_YAMASHIN
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
        Me.unnamed_Label_1917977628224 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977636864 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977628608 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977625344 = New System.Windows.Forms.TextBox()
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
        Me.txt_FileNM = New System.Windows.Forms.TextBox()
        Me.lbl_出力ﾌｧｲﾙ名 = New System.Windows.Forms.Label()
        Me.cmd_選択2 = New System.Windows.Forms.Button()
        Me.lbl_OUTPUT_FOLDER_NM = New System.Windows.Forms.Label()
        Me.txt_FolderNM = New System.Windows.Forms.TextBox()
        Me.cmd_選択1 = New System.Windows.Forms.Button()
        Me.ラベル53 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_計上仕訳_YAMASHIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977628224)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977636864)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977628608)
        Me.Controls.Add(Me.unnamed_TextBox_1917977625344)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977628224
        Me.unnamed_Label_1917977628224.Name = "unnamed_Label_1917977628224"
        Me.unnamed_Label_1917977628224.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977628224.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977636864
        Me.unnamed_CommandButton_1917977636864.Name = "unnamed_CommandButton_1917977636864"
        Me.unnamed_CommandButton_1917977636864.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977636864.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917977628608
        Me.unnamed_CheckBox_1917977628608.Name = "unnamed_CheckBox_1917977628608"
        Me.unnamed_CheckBox_1917977628608.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977628608.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977625344
        Me.unnamed_TextBox_1917977625344.Name = "unnamed_TextBox_1917977625344"
        Me.unnamed_TextBox_1917977625344.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977625344.Size = New System.Drawing.Size(113, 26)

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

        ' txt_FileNM
        Me.txt_FileNM.Name = "txt_FileNM"
        Me.txt_FileNM.Location = New System.Drawing.Point(139, 317)
        Me.txt_FileNM.Size = New System.Drawing.Size(359, 18)
        Me.txt_FileNM.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.txt_FileNM)

        ' lbl_出力ﾌｧｲﾙ名
        Me.lbl_出力ﾌｧｲﾙ名.Name = "lbl_出力ﾌｧｲﾙ名"
        Me.lbl_出力ﾌｧｲﾙ名.Location = New System.Drawing.Point(26, 317)
        Me.lbl_出力ﾌｧｲﾙ名.Size = New System.Drawing.Size(113, 18)
        Me.lbl_出力ﾌｧｲﾙ名.Text = "出力ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_出力ﾌｧｲﾙ名)

        ' cmd_選択2
        Me.cmd_選択2.Name = "cmd_選択2"
        Me.cmd_選択2.Location = New System.Drawing.Point(502, 317)
        Me.cmd_選択2.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択2.Text = "選択(&S)"
        Me.cmd_選択2.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.cmd_選択2)

        ' lbl_OUTPUT_FOLDER_NM
        Me.lbl_OUTPUT_FOLDER_NM.Name = "lbl_OUTPUT_FOLDER_NM"
        Me.lbl_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(26, 215)
        Me.lbl_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(113, 18)
        Me.lbl_OUTPUT_FOLDER_NM.Text = "出力先ﾌｫﾙﾀﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FOLDER_NM)

        ' txt_FolderNM
        Me.txt_FolderNM.Name = "txt_FolderNM"
        Me.txt_FolderNM.Location = New System.Drawing.Point(139, 215)
        Me.txt_FolderNM.Size = New System.Drawing.Size(359, 18)
        Me.txt_FolderNM.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_FolderNM)

        ' cmd_選択1
        Me.cmd_選択1.Name = "cmd_選択1"
        Me.cmd_選択1.Location = New System.Drawing.Point(502, 215)
        Me.cmd_選択1.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択1.Text = "選択(&F)"
        Me.cmd_選択1.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.cmd_選択1)

        ' ラベル53
        Me.ラベル53.Name = "ラベル53"
        Me.ラベル53.Location = New System.Drawing.Point(45, 245)
        Me.ラベル53.Size = New System.Drawing.Size(390, 64)
        Me.ラベル53.Text = "M4BS_YYYYMMDD_HHMM_MM月_資産_開始.00002.dat\011\011\011\011\011\011\011\011\011\011\011"
        Me.pnlDetail.Controls.Add(Me.ラベル53)

        Me.Name = "Frmfc_計上仕訳_YAMASHIN"
        Me.Text = "月次仕訳計上ﾌﾚｯｸｽ - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977628224 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977636864 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977628608 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977625344 As System.Windows.Forms.TextBox
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
    Friend WithEvents txt_FileNM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_出力ﾌｧｲﾙ名 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択2 As System.Windows.Forms.Button
    Friend WithEvents lbl_OUTPUT_FOLDER_NM As System.Windows.Forms.Label
    Friend WithEvents txt_FolderNM As System.Windows.Forms.TextBox
    Friend WithEvents cmd_選択1 As System.Windows.Forms.Button
    Friend WithEvents ラベル53 As System.Windows.Forms.Label

End Class
