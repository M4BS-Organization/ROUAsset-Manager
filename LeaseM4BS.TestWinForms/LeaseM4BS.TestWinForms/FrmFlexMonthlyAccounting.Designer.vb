<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexMonthlyAccounting
    Inherits System.Windows.Forms.UserControl

    Private components As System.ComponentModel.IContainer

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblPlaceholder = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' lblPlaceholder
        '
        Me.lblPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPlaceholder.Font = New System.Drawing.Font("Meiryo", 14.0F, System.Drawing.FontStyle.Bold)
        Me.lblPlaceholder.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.lblPlaceholder.Location = New System.Drawing.Point(0, 0)
        Me.lblPlaceholder.Name = "lblPlaceholder"
        Me.lblPlaceholder.Size = New System.Drawing.Size(800, 600)
        Me.lblPlaceholder.TabIndex = 0
        Me.lblPlaceholder.Text = "月次会計（フレックス）- 準備中"
        Me.lblPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        ' FrmFlexMonthlyAccounting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.Controls.Add(Me.lblPlaceholder)
        Me.Name = "FrmFlexMonthlyAccounting"
        Me.Size = New System.Drawing.Size(800, 600)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblPlaceholder As System.Windows.Forms.Label

End Class
