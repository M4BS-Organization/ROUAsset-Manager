<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm税率変更ツール
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
        Me.unnamed_Label_1917970487680 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970446784 = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_貼付 = New System.Windows.Forms.Button()
        Me.cmd_税率変更 = New System.Windows.Forms.Button()
        Me.ラベル2 = New System.Windows.Forms.Label()
        Me.cmd_CLOSE = New System.Windows.Forms.Button()
        Me.cmd_CHECK = New System.Windows.Forms.Button()
        Me.ラベル4 = New System.Windows.Forms.Label()
        Me.cmd_臨時_変額税率を契約書に反映 = New System.Windows.Forms.Button()
        Me.ラベル8 = New System.Windows.Forms.Label()
        Me.ラベル7 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' Frm税率変更ツール
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(642, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970487680)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970446784)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917970487680
        Me.unnamed_Label_1917970487680.Name = "unnamed_Label_1917970487680"
        Me.unnamed_Label_1917970487680.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970487680.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970446784
        Me.unnamed_CommandButton_1917970446784.Name = "unnamed_CommandButton_1917970446784"
        Me.unnamed_CommandButton_1917970446784.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970446784.Size = New System.Drawing.Size(113, 26)

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

        ' cmd_税率変更
        Me.cmd_税率変更.Name = "cmd_税率変更"
        Me.cmd_税率変更.Location = New System.Drawing.Point(37, 226)
        Me.cmd_税率変更.Size = New System.Drawing.Size(189, 38)
        Me.cmd_税率変更.Text = "税率変更"
        Me.cmd_税率変更.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.cmd_税率変更)

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

        ' cmd_臨時_変額税率を契約書に反映
        Me.cmd_臨時_変額税率を契約書に反映.Name = "cmd_臨時_変額税率を契約書に反映"
        Me.cmd_臨時_変額税率を契約書に反映.Location = New System.Drawing.Point(37, 302)
        Me.cmd_臨時_変額税率を契約書に反映.Size = New System.Drawing.Size(189, 38)
        Me.cmd_臨時_変額税率を契約書に反映.Text = "(臨時)(MSYS)変額税率を契約書に反映"
        Me.cmd_臨時_変額税率を契約書に反映.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.cmd_臨時_変額税率を契約書に反映)

        ' ラベル8
        Me.ラベル8.Name = "ラベル8"
        Me.ラベル8.Location = New System.Drawing.Point(245, 302)
        Me.ラベル8.Size = New System.Drawing.Size(378, 18)
        Me.ラベル8.Text = "・変額リース料の最終行の税率を契約書にセットします。"
        Me.pnlDetail.Controls.Add(Me.ラベル8)

        ' ラベル7
        Me.ラベル7.Name = "ラベル7"
        Me.ラベル7.Location = New System.Drawing.Point(264, 321)
        Me.ラベル7.Size = New System.Drawing.Size(359, 18)
        Me.ラベル7.Text = "－ 定額が入力されている契約は更新対象から除外します。"
        Me.pnlDetail.Controls.Add(Me.ラベル7)

        Me.Name = "Frm税率変更ツール"
        Me.Text = "Frm税率変更ツール"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970487680 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970446784 As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_貼付 As System.Windows.Forms.Button
    Friend WithEvents cmd_税率変更 As System.Windows.Forms.Button
    Friend WithEvents ラベル2 As System.Windows.Forms.Label
    Friend WithEvents cmd_CLOSE As System.Windows.Forms.Button
    Friend WithEvents cmd_CHECK As System.Windows.Forms.Button
    Friend WithEvents ラベル4 As System.Windows.Forms.Label
    Friend WithEvents cmd_臨時_変額税率を契約書に反映 As System.Windows.Forms.Button
    Friend WithEvents ラベル8 As System.Windows.Forms.Label
    Friend WithEvents ラベル7 As System.Windows.Forms.Label

End Class
