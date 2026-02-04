<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm説明表示
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
        Me.unnamed_Label_1917970432896 = New System.Windows.Forms.Label()
        Me.unnamed_CommandButton_1917970443264 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917970436288 = New System.Windows.Forms.TextBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.cmd_Close = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_HYOJ_TEXT = New System.Windows.Forms.TextBox()
        Me.txt_TOPICS = New System.Windows.Forms.TextBox()
        Me.txt_HYOJ_KIND = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' Frm説明表示
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(566, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970432896)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970443264)
        Me.Controls.Add(Me.unnamed_TextBox_1917970436288)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970432896
        Me.unnamed_Label_1917970432896.Name = "unnamed_Label_1917970432896"
        Me.unnamed_Label_1917970432896.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970432896.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CommandButton_1917970443264
        Me.unnamed_CommandButton_1917970443264.Name = "unnamed_CommandButton_1917970443264"
        Me.unnamed_CommandButton_1917970443264.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970443264.Size = New System.Drawing.Size(113, 26)

        ' unnamed_TextBox_1917970436288
        Me.unnamed_TextBox_1917970436288.Name = "unnamed_TextBox_1917970436288"
        Me.unnamed_TextBox_1917970436288.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917970436288.Size = New System.Drawing.Size(113, 26)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(566, 52)

        ' cmd_Close
        Me.cmd_Close.Name = "cmd_Close"
        Me.cmd_Close.Location = New System.Drawing.Point(3, 3)
        Me.cmd_Close.Size = New System.Drawing.Size(75, 26)
        Me.cmd_Close.Text = "閉じる(&C)"
        Me.pnlHeader.Controls.Add(Me.cmd_Close)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' txt_HYOJ_TEXT
        Me.txt_HYOJ_TEXT.Name = "txt_HYOJ_TEXT"
        Me.txt_HYOJ_TEXT.Location = New System.Drawing.Point(18, 0)
        Me.txt_HYOJ_TEXT.Size = New System.Drawing.Size(548, 15)
        Me.pnlDetail.Controls.Add(Me.txt_HYOJ_TEXT)

        ' txt_TOPICS
        Me.txt_TOPICS.Name = "txt_TOPICS"
        Me.txt_TOPICS.Location = New System.Drawing.Point(415, 0)
        Me.txt_TOPICS.Size = New System.Drawing.Size(37, 15)
        Me.txt_TOPICS.Visible = False
        Me.txt_TOPICS.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.txt_TOPICS)

        ' txt_HYOJ_KIND
        Me.txt_HYOJ_KIND.Name = "txt_HYOJ_KIND"
        Me.txt_HYOJ_KIND.Location = New System.Drawing.Point(377, 0)
        Me.txt_HYOJ_KIND.Size = New System.Drawing.Size(37, 15)
        Me.txt_HYOJ_KIND.Visible = False
        Me.txt_HYOJ_KIND.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.txt_HYOJ_KIND)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(566, 18)

        Me.Name = "Frm説明表示"
        Me.Text = " "
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970432896 As System.Windows.Forms.Label
    Friend WithEvents unnamed_CommandButton_1917970443264 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917970436288 As System.Windows.Forms.TextBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents cmd_Close As System.Windows.Forms.Button
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents txt_HYOJ_TEXT As System.Windows.Forms.TextBox
    Friend WithEvents txt_TOPICS As System.Windows.Forms.TextBox
    Friend WithEvents txt_HYOJ_KIND As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel

End Class
