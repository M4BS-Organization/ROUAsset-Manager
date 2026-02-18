<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm中途解約ツール
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
        Me.unnamed_Label_1917970203392 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917977355328 = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_貼付 = New System.Windows.Forms.Button()
        Me.ラベル2 = New System.Windows.Forms.Label()
        Me.cmd_CLOSE = New System.Windows.Forms.Button()
        Me.cmd_CHECK = New System.Windows.Forms.Button()
        Me.ラベル4 = New System.Windows.Forms.Label()
        Me.cmd_中途解約 = New System.Windows.Forms.Button()
        Me.ラベル6 = New System.Windows.Forms.Label()
        Me.ラベル7 = New System.Windows.Forms.Label()
        Me.ラベル8 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frm中途解約ツール
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(642, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970203392)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977355328)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970203392
        Me.unnamed_Label_1917970203392.Name = "unnamed_Label_1917970203392"
        Me.unnamed_Label_1917970203392.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970203392.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917977355328
        Me.unnamed_CommandButton_1917977355328.Name = "unnamed_CommandButton_1917977355328"
        Me.unnamed_CommandButton_1917977355328.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977355328.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_貼付
        Me.cmd_貼付.Name = "cmd_貼付"
        Me.cmd_貼付.Location = New System.Drawing.Point(37, 75)
        Me.cmd_貼付.Size = New System.Drawing.Size(189, 38)
        Me.cmd_貼付.Text = "変更情報の貼り付け"
        Me.cmd_貼付.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.cmd_貼付)

        ' ラベル2
        Me.ラベル2.Name = "ラベル2"
        Me.ラベル2.Location = New System.Drawing.Point(113, 120)
        Me.ラベル2.Size = New System.Drawing.Size(37, 22)
        Me.ラベル2.Text = "↓"
        Me.pnlDetail.Controls.Add(Me.ラベル2)

        ' cmd_CLOSE
        Me.cmd_CLOSE.Name = "cmd_CLOSE"
        Me.cmd_CLOSE.Location = New System.Drawing.Point(18, 18)
        Me.cmd_CLOSE.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CLOSE.Text = "閉じる(&C)"
        Me.pnlDetail.Controls.Add(Me.cmd_CLOSE)

        ' cmd_CHECK
        Me.cmd_CHECK.Name = "cmd_CHECK"
        Me.cmd_CHECK.Location = New System.Drawing.Point(37, 151)
        Me.cmd_CHECK.Size = New System.Drawing.Size(189, 38)
        Me.cmd_CHECK.Text = "チェック"
        Me.cmd_CHECK.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.cmd_CHECK)

        ' ラベル4
        Me.ラベル4.Name = "ラベル4"
        Me.ラベル4.Location = New System.Drawing.Point(113, 196)
        Me.ラベル4.Size = New System.Drawing.Size(37, 22)
        Me.ラベル4.Text = "↓"
        Me.pnlDetail.Controls.Add(Me.ラベル4)

        ' cmd_中途解約
        Me.cmd_中途解約.Name = "cmd_中途解約"
        Me.cmd_中途解約.Location = New System.Drawing.Point(37, 226)
        Me.cmd_中途解約.Size = New System.Drawing.Size(189, 38)
        Me.cmd_中途解約.Text = "中途解約"
        Me.cmd_中途解約.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_中途解約)

        ' ラベル6
        Me.ラベル6.Name = "ラベル6"
        Me.ラベル6.Location = New System.Drawing.Point(245, 170)
        Me.ラベル6.Size = New System.Drawing.Size(378, 26)
        Me.ラベル6.Text = "・物件No(KYKM_NO)、再ﾘｰｽ回数(SAIKAISU) が同じ行が複数行ある場合、2行目以降は削除されます。"
        Me.pnlDetail.Controls.Add(Me.ラベル6)

        ' ラベル7
        Me.ラベル7.Name = "ラベル7"
        Me.ラベル7.Location = New System.Drawing.Point(245, 75)
        Me.ラベル7.Size = New System.Drawing.Size(378, 18)
        Me.ラベル7.Text = "・契約No(KYKH_NO) はセット不要です。"
        Me.pnlDetail.Controls.Add(Me.ラベル7)

        ' ラベル8
        Me.ラベル8.Name = "ラベル8"
        Me.ラベル8.Location = New System.Drawing.Point(245, 151)
        Me.ラベル8.Size = New System.Drawing.Size(378, 18)
        Me.ラベル8.Text = "・契約No(KYKH_NO) がセットされます。"
        Me.pnlDetail.Controls.Add(Me.ラベル8)

        Me.Name = "Frm中途解約ツール"
        Me.Text = "中途解約ツール"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970203392 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917977355328 As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_貼付 As System.Windows.Forms.Button
    Friend WithEvents ラベル2 As System.Windows.Forms.Label
    Friend WithEvents cmd_CLOSE As System.Windows.Forms.Button
    Friend WithEvents cmd_CHECK As System.Windows.Forms.Button
    Friend WithEvents ラベル4 As System.Windows.Forms.Label
    Friend WithEvents cmd_中途解約 As System.Windows.Forms.Button
    Friend WithEvents ラベル6 As System.Windows.Forms.Label
    Friend WithEvents ラベル7 As System.Windows.Forms.Label
    Friend WithEvents ラベル8 As System.Windows.Forms.Label

End Class
