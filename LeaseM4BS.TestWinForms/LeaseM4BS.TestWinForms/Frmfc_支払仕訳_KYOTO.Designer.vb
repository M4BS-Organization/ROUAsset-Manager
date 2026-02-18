<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_支払仕訳_KYOTO
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
        Me.unnamed_Label_1917977387776 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977389696 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977391296 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977936896 = New System.Windows.Forms.TextBox()
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
        Me.txt_TEXT_01 = New System.Windows.Forms.TextBox()
        Me.lbl_出力ﾌｧｲﾙ名 = New System.Windows.Forms.Label()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.txt_YMD_02 = New System.Windows.Forms.TextBox()
        Me.lbl_YMD_02 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frmfc_支払仕訳_KYOTO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977387776)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977389696)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977391296)
        Me.Controls.Add(Me.unnamed_TextBox_1917977936896)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977387776
        Me.unnamed_Label_1917977387776.Name = "unnamed_Label_1917977387776"
        Me.unnamed_Label_1917977387776.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977387776.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977389696
        Me.unnamed_CommandButton_1917977389696.Name = "unnamed_CommandButton_1917977389696"
        Me.unnamed_CommandButton_1917977389696.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977389696.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917977391296
        Me.unnamed_CheckBox_1917977391296.Name = "unnamed_CheckBox_1917977391296"
        Me.unnamed_CheckBox_1917977391296.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977391296.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977936896
        Me.unnamed_TextBox_1917977936896.Name = "unnamed_TextBox_1917977936896"
        Me.unnamed_TextBox_1917977936896.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977936896.Size = New System.Drawing.Size(113, 26)

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

        ' txt_YMD_01
        Me.txt_YMD_01.Name = "txt_YMD_01"
        Me.txt_YMD_01.Location = New System.Drawing.Point(139, 45)
        Me.txt_YMD_01.Size = New System.Drawing.Size(79, 18)
        Me.txt_YMD_01.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_YMD_01)

        ' lbl_YMD_01
        Me.lbl_YMD_01.Name = "lbl_YMD_01"
        Me.lbl_YMD_01.Location = New System.Drawing.Point(26, 45)
        Me.lbl_YMD_01.Size = New System.Drawing.Size(113, 18)
        Me.lbl_YMD_01.Text = "対象月"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_01)

        ' lbl_EXPLANATION1
        Me.lbl_EXPLANATION1.Name = "lbl_EXPLANATION1"
        Me.lbl_EXPLANATION1.Location = New System.Drawing.Point(143, 102)
        Me.lbl_EXPLANATION1.Size = New System.Drawing.Size(366, 18)
        Me.lbl_EXPLANATION1.Text = "※月次支払照合ﾌﾚｯｸｽを検索条件で抽出した結果に対して"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION1)

        ' lbl_出力元の抽出
        Me.lbl_出力元の抽出.Name = "lbl_出力元の抽出"
        Me.lbl_出力元の抽出.Location = New System.Drawing.Point(26, 75)
        Me.lbl_出力元の抽出.Size = New System.Drawing.Size(113, 18)
        Me.lbl_出力元の抽出.Text = "出力元の抽出"
        Me.pnlDetail.Controls.Add(Me.lbl_出力元の抽出)

        ' chk_CHK_01
        Me.chk_CHK_01.Name = "chk_CHK_01"
        Me.chk_CHK_01.Location = New System.Drawing.Point(166, 75)
        Me.chk_CHK_01.Size = New System.Drawing.Size(120, 18)
        Me.chk_CHK_01.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.chk_CHK_01)

        ' lbl_CHK_01
        Me.lbl_CHK_01.Name = "lbl_CHK_01"
        Me.lbl_CHK_01.Location = New System.Drawing.Point(181, 75)
        Me.lbl_CHK_01.Size = New System.Drawing.Size(113, 15)
        Me.lbl_CHK_01.Text = "検索条件を加味する"
        Me.chk_CHK_01.Controls.Add(Me.lbl_CHK_01)

        ' txt_TEXT_01
        Me.txt_TEXT_01.Name = "txt_TEXT_01"
        Me.txt_TEXT_01.Location = New System.Drawing.Point(139, 181)
        Me.txt_TEXT_01.Size = New System.Drawing.Size(359, 18)
        Me.txt_TEXT_01.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_TEXT_01)

        ' lbl_出力ﾌｧｲﾙ名
        Me.lbl_出力ﾌｧｲﾙ名.Name = "lbl_出力ﾌｧｲﾙ名"
        Me.lbl_出力ﾌｧｲﾙ名.Location = New System.Drawing.Point(26, 181)
        Me.lbl_出力ﾌｧｲﾙ名.Size = New System.Drawing.Size(113, 18)
        Me.lbl_出力ﾌｧｲﾙ名.Text = "出力ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_出力ﾌｧｲﾙ名)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(502, 181)
        Me.cmd_選択.Size = New System.Drawing.Size(64, 18)
        Me.cmd_選択.Text = "選択(&S)"
        Me.cmd_選択.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' txt_YMD_02
        Me.txt_YMD_02.Name = "txt_YMD_02"
        Me.txt_YMD_02.Location = New System.Drawing.Point(139, 151)
        Me.txt_YMD_02.Size = New System.Drawing.Size(90, 18)
        Me.txt_YMD_02.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_YMD_02)

        ' lbl_YMD_02
        Me.lbl_YMD_02.Name = "lbl_YMD_02"
        Me.lbl_YMD_02.Location = New System.Drawing.Point(26, 151)
        Me.lbl_YMD_02.Size = New System.Drawing.Size(113, 18)
        Me.lbl_YMD_02.Text = "処理日付"
        Me.pnlDetail.Controls.Add(Me.lbl_YMD_02)

        Me.Name = "Frmfc_支払仕訳_KYOTO"
        Me.Text = "月次支払照合ﾌﾚｯｸｽ - 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977387776 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977389696 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977391296 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977936896 As System.Windows.Forms.TextBox
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
    Friend WithEvents txt_TEXT_01 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_出力ﾌｧｲﾙ名 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents txt_YMD_02 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_YMD_02 As System.Windows.Forms.Label

End Class
