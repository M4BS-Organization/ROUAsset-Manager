<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm仕訳出力標準_KJ
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
        Me.unnamed_Label_1917977145216 = New System.Windows.Forms.Label()
        Me.unnamed_Rectangle_1917977135680 = New System.Windows.Forms.Panel()
        Me.unnamed_CommandButton_1917977136640 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977148288 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977145344 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977136064 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_GUIDE3 = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FOLDER_NM = New System.Windows.Forms.TextBox()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.ラベル514 = New System.Windows.Forms.Label()
        Me.lbl_GUIDE1 = New System.Windows.Forms.Label()
        Me.txt_KEIJO_DT = New System.Windows.Forms.TextBox()
        Me.ラベル521 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.lbl_GUIDE2 = New System.Windows.Forms.Label()
        Me.cmd_設定 = New System.Windows.Forms.Button()
        Me.txt_対象年月 = New System.Windows.Forms.TextBox()
        Me.ラベル253 = New System.Windows.Forms.Label()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' Frm仕訳出力標準_KJ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977145216)
        Me.Controls.Add(Me.unnamed_Rectangle_1917977135680)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977136640)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977148288)
        Me.Controls.Add(Me.unnamed_TextBox_1917977145344)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977136064)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917977145216
        Me.unnamed_Label_1917977145216.Name = "unnamed_Label_1917977145216"
        Me.unnamed_Label_1917977145216.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977145216.Size = New System.Drawing.Size(133, 26)

        ' unnamed_Rectangle_1917977135680
        Me.unnamed_Rectangle_1917977135680.Name = "unnamed_Rectangle_1917977135680"
        Me.unnamed_Rectangle_1917977135680.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Rectangle_1917977135680.Size = New System.Drawing.Size(56, 56)

        ' unnamed_CommandButton_1917977136640
        Me.unnamed_CommandButton_1917977136640.Name = "unnamed_CommandButton_1917977136640"
        Me.unnamed_CommandButton_1917977136640.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977136640.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917977148288
        Me.unnamed_CheckBox_1917977148288.Name = "unnamed_CheckBox_1917977148288"
        Me.unnamed_CheckBox_1917977148288.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977148288.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977145344
        Me.unnamed_TextBox_1917977145344.Name = "unnamed_TextBox_1917977145344"
        Me.unnamed_TextBox_1917977145344.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977145344.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977136064
        Me.unnamed_ComboBox_1917977136064.Name = "unnamed_ComboBox_1917977136064"
        Me.unnamed_ComboBox_1917977136064.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977136064.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(695, 0)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_GUIDE3
        Me.lbl_GUIDE3.Name = "lbl_GUIDE3"
        Me.lbl_GUIDE3.Location = New System.Drawing.Point(75, 192)
        Me.lbl_GUIDE3.Size = New System.Drawing.Size(567, 18)
        Me.lbl_GUIDE3.Text = "YYYYMM_月次仕訳計上フレックス_仕訳_YYYYMMDDHHMM.xls の名前でファイルが生成されます。"
        Me.pnlDetail.Controls.Add(Me.lbl_GUIDE3)

        ' txt_OUTPUT_FOLDER_NM
        Me.txt_OUTPUT_FOLDER_NM.Name = "txt_OUTPUT_FOLDER_NM"
        Me.txt_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(132, 170)
        Me.txt_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(483, 18)
        Me.txt_OUTPUT_FOLDER_NM.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FOLDER_NM)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(619, 170)
        Me.cmd_選択.Size = New System.Drawing.Size(56, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' ラベル514
        Me.ラベル514.Name = "ラベル514"
        Me.ラベル514.Location = New System.Drawing.Point(37, 170)
        Me.ラベル514.Size = New System.Drawing.Size(94, 18)
        Me.ラベル514.Text = "出力先ﾌｫﾙﾀﾞ名"
        Me.pnlDetail.Controls.Add(Me.ラベル514)

        ' lbl_GUIDE1
        Me.lbl_GUIDE1.Name = "lbl_GUIDE1"
        Me.lbl_GUIDE1.Location = New System.Drawing.Point(37, 56)
        Me.lbl_GUIDE1.Size = New System.Drawing.Size(567, 18)
        Me.lbl_GUIDE1.Text = "月次仕訳計上フレックスに表示中のデータから仕訳データを作成します。"
        Me.pnlDetail.Controls.Add(Me.lbl_GUIDE1)

        ' txt_KEIJO_DT
        Me.txt_KEIJO_DT.Name = "txt_KEIJO_DT"
        Me.txt_KEIJO_DT.Location = New System.Drawing.Point(132, 139)
        Me.txt_KEIJO_DT.Size = New System.Drawing.Size(75, 18)
        Me.txt_KEIJO_DT.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_KEIJO_DT)

        ' ラベル521
        Me.ラベル521.Name = "ラベル521"
        Me.ラベル521.Location = New System.Drawing.Point(37, 139)
        Me.ラベル521.Size = New System.Drawing.Size(94, 18)
        Me.ラベル521.Text = "計上日"
        Me.pnlDetail.Controls.Add(Me.ラベル521)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(7, 7)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.cmd_実行.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(90, 7)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmd_CANCEL)

        ' lbl_GUIDE2
        Me.lbl_GUIDE2.Name = "lbl_GUIDE2"
        Me.lbl_GUIDE2.Location = New System.Drawing.Point(75, 75)
        Me.lbl_GUIDE2.Size = New System.Drawing.Size(567, 25)
        Me.lbl_GUIDE2.Text = "※検索条件が加味されます（検索条件が入力されている場合、検索条件で抽出された結果を元に仕訳データを作成します。）"
        Me.pnlDetail.Controls.Add(Me.lbl_GUIDE2)

        ' cmd_設定
        Me.cmd_設定.Name = "cmd_設定"
        Me.cmd_設定.Location = New System.Drawing.Point(600, 7)
        Me.cmd_設定.Size = New System.Drawing.Size(75, 26)
        Me.cmd_設定.Text = "設定(&D)"
        Me.cmd_設定.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.cmd_設定)

        ' txt_対象年月
        Me.txt_対象年月.Name = "txt_対象年月"
        Me.txt_対象年月.Location = New System.Drawing.Point(132, 113)
        Me.txt_対象年月.Size = New System.Drawing.Size(75, 18)
        Me.pnlDetail.Controls.Add(Me.txt_対象年月)

        ' ラベル253
        Me.ラベル253.Name = "ラベル253"
        Me.ラベル253.Location = New System.Drawing.Point(37, 113)
        Me.ラベル253.Size = New System.Drawing.Size(94, 18)
        Me.ラベル253.Text = "対象年月"
        Me.pnlDetail.Controls.Add(Me.ラベル253)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(695, 0)

        Me.Name = "Frm仕訳出力標準_KJ"
        Me.Text = "月次仕訳計上フレックス 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977145216 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Rectangle_1917977135680 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_CommandButton_1917977136640 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977148288 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977145344 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977136064 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_GUIDE3 As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FOLDER_NM As System.Windows.Forms.TextBox
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents ラベル514 As System.Windows.Forms.Label
    Friend WithEvents lbl_GUIDE1 As System.Windows.Forms.Label
    Friend WithEvents txt_KEIJO_DT As System.Windows.Forms.TextBox
    Friend WithEvents ラベル521 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents lbl_GUIDE2 As System.Windows.Forms.Label
    Friend WithEvents cmd_設定 As System.Windows.Forms.Button
    Friend WithEvents txt_対象年月 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル253 As System.Windows.Forms.Label
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
