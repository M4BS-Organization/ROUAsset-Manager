<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_計上仕訳_KITOKU
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
        Me.unnamed_Label_1917977386560 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970493184 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917970465152 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_SLIP_DT = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_DT = New System.Windows.Forms.Label()
        Me.txt_SLIP_NO_START_VAL = New System.Windows.Forms.TextBox()
        Me.lbl_SLIP_NO_START_VAL = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FOLDER_NM = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FOLDER_NM = New System.Windows.Forms.TextBox()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.lbl_EXPLANATION3 = New System.Windows.Forms.Label()
        Me.lbl_EXPLANATION4 = New System.Windows.Forms.Label()
        Me.lbl_OUTPUT_FILE_NM = New System.Windows.Forms.Label()
        Me.txt_OUTPUT_FILE_NM = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' Frmfc_計上仕訳_KITOKU
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977386560)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970493184)
        Me.Controls.Add(Me.unnamed_TextBox_1917970465152)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977386560
        Me.unnamed_Label_1917977386560.Name = "unnamed_Label_1917977386560"
        Me.unnamed_Label_1917977386560.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977386560.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970493184
        Me.unnamed_CommandButton_1917970493184.Name = "unnamed_CommandButton_1917970493184"
        Me.unnamed_CommandButton_1917970493184.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970493184.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917970465152
        Me.unnamed_TextBox_1917970465152.Name = "unnamed_TextBox_1917970465152"
        Me.unnamed_TextBox_1917970465152.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970465152.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

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
        Me.lbl_SLIP_DT.Text = "伝票日付"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_DT)

        ' txt_SLIP_NO_START_VAL
        Me.txt_SLIP_NO_START_VAL.Name = "txt_SLIP_NO_START_VAL"
        Me.txt_SLIP_NO_START_VAL.Location = New System.Drawing.Point(136, 75)
        Me.txt_SLIP_NO_START_VAL.Size = New System.Drawing.Size(94, 18)
        Me.txt_SLIP_NO_START_VAL.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.txt_SLIP_NO_START_VAL)

        ' lbl_SLIP_NO_START_VAL
        Me.lbl_SLIP_NO_START_VAL.Name = "lbl_SLIP_NO_START_VAL"
        Me.lbl_SLIP_NO_START_VAL.Location = New System.Drawing.Point(26, 75)
        Me.lbl_SLIP_NO_START_VAL.Size = New System.Drawing.Size(109, 18)
        Me.lbl_SLIP_NO_START_VAL.Text = "伝票番号開始値"
        Me.pnlDetail.Controls.Add(Me.lbl_SLIP_NO_START_VAL)

        ' lbl_OUTPUT_FOLDER_NM
        Me.lbl_OUTPUT_FOLDER_NM.Name = "lbl_OUTPUT_FOLDER_NM"
        Me.lbl_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(26, 105)
        Me.lbl_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(109, 18)
        Me.lbl_OUTPUT_FOLDER_NM.Text = "出力先ﾌｫﾙﾀﾞ"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FOLDER_NM)

        ' txt_OUTPUT_FOLDER_NM
        Me.txt_OUTPUT_FOLDER_NM.Name = "txt_OUTPUT_FOLDER_NM"
        Me.txt_OUTPUT_FOLDER_NM.Location = New System.Drawing.Point(136, 105)
        Me.txt_OUTPUT_FOLDER_NM.Size = New System.Drawing.Size(340, 18)
        Me.txt_OUTPUT_FOLDER_NM.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FOLDER_NM)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(480, 105)
        Me.cmd_選択.Size = New System.Drawing.Size(45, 18)
        Me.cmd_選択.Text = "選択"
        Me.cmd_選択.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.cmd_選択)

        ' lbl_EXPLANATION3
        Me.lbl_EXPLANATION3.Name = "lbl_EXPLANATION3"
        Me.lbl_EXPLANATION3.Location = New System.Drawing.Point(26, 139)
        Me.lbl_EXPLANATION3.Size = New System.Drawing.Size(49, 18)
        Me.lbl_EXPLANATION3.Text = "ﾌｧｲﾙ名"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION3)

        ' lbl_EXPLANATION4
        Me.lbl_EXPLANATION4.Name = "lbl_EXPLANATION4"
        Me.lbl_EXPLANATION4.Location = New System.Drawing.Point(75, 158)
        Me.lbl_EXPLANATION4.Size = New System.Drawing.Size(49, 18)
        Me.lbl_EXPLANATION4.Text = "CORE"
        Me.pnlDetail.Controls.Add(Me.lbl_EXPLANATION4)

        ' lbl_OUTPUT_FILE_NM
        Me.lbl_OUTPUT_FILE_NM.Name = "lbl_OUTPUT_FILE_NM"
        Me.lbl_OUTPUT_FILE_NM.Location = New System.Drawing.Point(75, 177)
        Me.lbl_OUTPUT_FILE_NM.Size = New System.Drawing.Size(207, 18)
        Me.lbl_OUTPUT_FILE_NM.Text = "その他システム用仕訳ワーク"
        Me.pnlDetail.Controls.Add(Me.lbl_OUTPUT_FILE_NM)

        ' txt_OUTPUT_FILE_NM
        Me.txt_OUTPUT_FILE_NM.Name = "txt_OUTPUT_FILE_NM"
        Me.txt_OUTPUT_FILE_NM.Location = New System.Drawing.Point(283, 177)
        Me.txt_OUTPUT_FILE_NM.Size = New System.Drawing.Size(132, 18)
        Me.txt_OUTPUT_FILE_NM.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.txt_OUTPUT_FILE_NM)

        Me.Name = "Frmfc_計上仕訳_KITOKU"
        Me.Text = "計上仕訳出力画面"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977386560 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970493184 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917970465152 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_SLIP_DT As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_DT As System.Windows.Forms.Label
    Friend WithEvents txt_SLIP_NO_START_VAL As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SLIP_NO_START_VAL As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FOLDER_NM As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FOLDER_NM As System.Windows.Forms.TextBox
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents lbl_EXPLANATION3 As System.Windows.Forms.Label
    Friend WithEvents lbl_EXPLANATION4 As System.Windows.Forms.Label
    Friend WithEvents lbl_OUTPUT_FILE_NM As System.Windows.Forms.Label
    Friend WithEvents txt_OUTPUT_FILE_NM As System.Windows.Forms.TextBox

End Class
