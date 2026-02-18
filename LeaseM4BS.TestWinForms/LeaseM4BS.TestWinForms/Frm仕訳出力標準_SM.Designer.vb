<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm仕訳出力標準_SM
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
        Me.unnamed_Label_1917977139840 = New System.Windows.Forms.Label()
        Me.unnamed_Rectangle_1917977135232 = New System.Windows.Forms.Panel()
        Me.unnamed_CommandButton_1917977135552 = New System.Windows.Forms.Button()
        Me.unnamed_CheckBox_1917977133504 = New System.Windows.Forms.CheckBox()
        Me.unnamed_TextBox_1917977141760 = New System.Windows.Forms.TextBox()
        Me.unnamed_ComboBox_1917977626880 = New System.Windows.Forms.ComboBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.lbl_GUIDE3 = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FOLDER_NM = New System.Windows.Forms.TextBox()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.ラベル514 = New System.Windows.Forms.Label()
        Me.lbl_GUIDE1 = New System.Windows.Forms.Label()
        Me.txt_KEIJO1_DT = New System.Windows.Forms.TextBox()
        Me.ラベル521 = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.lbl_GUIDE2 = New System.Windows.Forms.Label()
        Me.cmd_設定 = New System.Windows.Forms.Button()
        Me.txt_対象年月 = New System.Windows.Forms.TextBox()
        Me.ラベル253 = New System.Windows.Forms.Label()
        Me.txt_KEIJO2_DT = New System.Windows.Forms.TextBox()
        Me.ラベル256 = New System.Windows.Forms.Label()
        Me.ラベル257 = New System.Windows.Forms.Label()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' Frm仕訳出力標準_SM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977139840)
        Me.Controls.Add(Me.unnamed_Rectangle_1917977135232)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977135552)
        Me.Controls.Add(Me.unnamed_CheckBox_1917977133504)
        Me.Controls.Add(Me.unnamed_TextBox_1917977141760)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977626880)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917977139840
        Me.unnamed_Label_1917977139840.Name = "unnamed_Label_1917977139840"
        Me.unnamed_Label_1917977139840.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977139840.Size = New System.Drawing.Size(133, 26)

        ' unnamed_Rectangle_1917977135232
        Me.unnamed_Rectangle_1917977135232.Name = "unnamed_Rectangle_1917977135232"
        Me.unnamed_Rectangle_1917977135232.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Rectangle_1917977135232.Size = New System.Drawing.Size(56, 56)

        ' unnamed_CommandButton_1917977135552
        Me.unnamed_CommandButton_1917977135552.Name = "unnamed_CommandButton_1917977135552"
        Me.unnamed_CommandButton_1917977135552.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977135552.Size = New System.Drawing.Size(113, 26)

        ' unnamed_CheckBox_1917977133504
        Me.unnamed_CheckBox_1917977133504.Name = "unnamed_CheckBox_1917977133504"
        Me.unnamed_CheckBox_1917977133504.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917977133504.Size = New System.Drawing.Size(133, 26)

        ' unnamed_TextBox_1917977141760
        Me.unnamed_TextBox_1917977141760.Name = "unnamed_TextBox_1917977141760"
        Me.unnamed_TextBox_1917977141760.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977141760.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ComboBox_1917977626880
        Me.unnamed_ComboBox_1917977626880.Name = "unnamed_ComboBox_1917977626880"
        Me.unnamed_ComboBox_1917977626880.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977626880.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(695, 0)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' lbl_GUIDE3
        Me.lbl_GUIDE3.Name = "lbl_GUIDE3"
        Me.lbl_GUIDE3.Location = New System.Drawing.Point(75, 219)
        Me.lbl_GUIDE3.Size = New System.Drawing.Size(567, 18)
        Me.lbl_GUIDE3.Text = "YYYYMM_月次仕訳計上フレックス_仕訳_YYYYMMDDHHMM.xls の名前でファイルが生成されます。"
        Me.pnlDetail.Controls.Add(Me.lbl_GUIDE3)

        ' txt_OUTPUT_FOLDER_NM
        Me.txt_OUTPUT_FOLDER_NM.Name = "txt_OUTPUT_FOLDER_NM"
        Me.txt_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(132, 196)
        Me.txt_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(483, 18)
        Me.txt_OUTPUT_FOLDER_NM.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FOLDER_NM)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(619, 196)
        Me.cmd_選択.Size = New System.Drawing.Size(56, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' ラベル514
        Me.ラベル514.Name = "ラベル514"
        Me.ラベル514.Location = New System.Drawing.Point(37, 196)
        Me.ラベル514.Size = New System.Drawing.Size(94, 18)
        Me.ラベル514.Text = "出力先ﾌｫﾙﾀﾞ名"
        Me.pnlDetail.Controls.Add(Me.ラベル514)

        ' lbl_GUIDE1
        Me.lbl_GUIDE1.Name = "lbl_GUIDE1"
        Me.lbl_GUIDE1.Location = New System.Drawing.Point(37, 56)
        Me.lbl_GUIDE1.Size = New System.Drawing.Size(567, 18)
        Me.lbl_GUIDE1.Text = "リース債務返済明細表に表示中のデータから仕訳データを作成します。"
        Me.pnlDetail.Controls.Add(Me.lbl_GUIDE1)

        ' txt_KEIJO1_DT
        Me.txt_KEIJO1_DT.Name = "txt_KEIJO1_DT"
        Me.txt_KEIJO1_DT.Location = New System.Drawing.Point(226, 139)
        Me.txt_KEIJO1_DT.Size = New System.Drawing.Size(75, 18)
        Me.txt_KEIJO1_DT.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_KEIJO1_DT)

        ' ラベル521
        Me.ラベル521.Name = "ラベル521"
        Me.ラベル521.Location = New System.Drawing.Point(37, 139)
        Me.ラベル521.Size = New System.Drawing.Size(94, 37)
        Me.ラベル521.Text = "計上日"
        Me.pnlDetail.Controls.Add(Me.ラベル521)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(7, 7)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.cmd_実行.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(90, 7)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 6
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
        Me.cmd_設定.TabIndex = 7
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

        ' txt_KEIJO2_DT
        Me.txt_KEIJO2_DT.Name = "txt_KEIJO2_DT"
        Me.txt_KEIJO2_DT.Location = New System.Drawing.Point(226, 158)
        Me.txt_KEIJO2_DT.Size = New System.Drawing.Size(75, 18)
        Me.txt_KEIJO2_DT.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_KEIJO2_DT)

        ' ラベル256
        Me.ラベル256.Name = "ラベル256"
        Me.ラベル256.Location = New System.Drawing.Point(132, 139)
        Me.ラベル256.Size = New System.Drawing.Size(94, 18)
        Me.ラベル256.Text = "長短振替"
        Me.pnlDetail.Controls.Add(Me.ラベル256)

        ' ラベル257
        Me.ラベル257.Name = "ラベル257"
        Me.ラベル257.Location = New System.Drawing.Point(132, 158)
        Me.ラベル257.Size = New System.Drawing.Size(94, 18)
        Me.ラベル257.Text = "長短振替戻し"
        Me.pnlDetail.Controls.Add(Me.ラベル257)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(695, 0)

        Me.Name = "Frm仕訳出力標準_SM"
        Me.Text = "リース債務返済明細表 仕訳出力"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977139840 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Rectangle_1917977135232 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_CommandButton_1917977135552 As System.Windows.Forms.Button
    Friend WithEvents unnamed_CheckBox_1917977133504 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_TextBox_1917977141760 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ComboBox_1917977626880 As System.Windows.Forms.ComboBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents lbl_GUIDE3 As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FOLDER_NM As System.Windows.Forms.TextBox
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents ラベル514 As System.Windows.Forms.Label
    Friend WithEvents lbl_GUIDE1 As System.Windows.Forms.Label
    Friend WithEvents txt_KEIJO1_DT As System.Windows.Forms.TextBox
    Friend WithEvents ラベル521 As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents lbl_GUIDE2 As System.Windows.Forms.Label
    Friend WithEvents cmd_設定 As System.Windows.Forms.Button
    Friend WithEvents txt_対象年月 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル253 As System.Windows.Forms.Label
    Friend WithEvents txt_KEIJO2_DT As System.Windows.Forms.TextBox
    Friend WithEvents ラベル256 As System.Windows.Forms.Label
    Friend WithEvents ラベル257 As System.Windows.Forms.Label
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
