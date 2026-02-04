<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDummy2
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
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        ' FrmDummy2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(7, 800)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        Me.Name = "FrmDummy2"
        Me.Text = "FrmDummy2"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents pnlDetail As System.Windows.Forms.Panel

End Class
