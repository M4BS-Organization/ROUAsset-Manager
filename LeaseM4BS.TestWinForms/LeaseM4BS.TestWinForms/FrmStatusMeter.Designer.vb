<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmStatusMeter
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
        Me.unnamed_Label_1917977661440 = New System.Windows.Forms.Label()
        Me.unnamed_Rectangle_1917977660672 = New System.Windows.Forms.Panel()
        Me.unnamed_CommandButton_1917977671680 = New System.Windows.Forms.Button()
        Me.unnamed_TextBox_1917977673408 = New System.Windows.Forms.TextBox()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.cmd_Cancel = New System.Windows.Forms.Button()
        Me.lbl_ProcName = New System.Windows.Forms.Label()
        Me.lbl_ProcID = New System.Windows.Forms.Label()
        Me.lbl_GaugeBox = New System.Windows.Forms.Panel()
        Me.lbl_Gauge = New System.Windows.Forms.Panel()
        Me.lbl_recsts_Blue = New System.Windows.Forms.Label()
        Me.lbl_recsts_White = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' FrmStatusMeter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(396, 800)
        Me.Controls.Add(Me.unnamed_Label_1917977661440)
        Me.Controls.Add(Me.unnamed_Rectangle_1917977660672)
        Me.Controls.Add(Me.unnamed_CommandButton_1917977671680)
        Me.Controls.Add(Me.unnamed_TextBox_1917977673408)
        Me.Controls.Add(Me.pnlDetail)
        '
        ' Properties
        '
        ' unnamed_Label_1917977661440
        Me.unnamed_Label_1917977661440.Name = "unnamed_Label_1917977661440"
        Me.unnamed_Label_1917977661440.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917977661440.Size = New System.Drawing.Size(133, 26)

        ' unnamed_Rectangle_1917977660672
        Me.unnamed_Rectangle_1917977660672.Name = "unnamed_Rectangle_1917977660672"
        Me.unnamed_Rectangle_1917977660672.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Rectangle_1917977660672.Size = New System.Drawing.Size(0, 24)

        ' unnamed_CommandButton_1917977671680
        Me.unnamed_CommandButton_1917977671680.Name = "unnamed_CommandButton_1917977671680"
        Me.unnamed_CommandButton_1917977671680.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917977671680.Size = New System.Drawing.Size(76, 26)

        ' unnamed_TextBox_1917977673408
        Me.unnamed_TextBox_1917977673408.Name = "unnamed_TextBox_1917977673408"
        Me.unnamed_TextBox_1917977673408.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977673408.Size = New System.Drawing.Size(113, 26)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' cmd_Cancel
        Me.cmd_Cancel.Name = "cmd_Cancel"
        Me.cmd_Cancel.Location = New System.Drawing.Point(170, 81)
        Me.cmd_Cancel.Size = New System.Drawing.Size(57, 26)
        Me.cmd_Cancel.Text = "&Cancel"
        Me.cmd_Cancel.Visible = False
        Me.pnlDetail.Controls.Add(Me.cmd_Cancel)

        ' lbl_ProcName
        Me.lbl_ProcName.Name = "lbl_ProcName"
        Me.lbl_ProcName.Location = New System.Drawing.Point(45, 18)
        Me.lbl_ProcName.Size = New System.Drawing.Size(298, 31)
        Me.pnlDetail.Controls.Add(Me.lbl_ProcName)

        ' lbl_ProcID
        Me.lbl_ProcID.Name = "lbl_ProcID"
        Me.lbl_ProcID.Location = New System.Drawing.Point(343, 18)
        Me.lbl_ProcID.Size = New System.Drawing.Size(37, 15)
        Me.pnlDetail.Controls.Add(Me.lbl_ProcID)

        ' lbl_GaugeBox
        Me.lbl_GaugeBox.Name = "lbl_GaugeBox"
        Me.lbl_GaugeBox.Location = New System.Drawing.Point(7, 56)
        Me.lbl_GaugeBox.Size = New System.Drawing.Size(374, 19)
        Me.lbl_GaugeBox.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_GaugeBox)

        ' lbl_Gauge
        Me.lbl_Gauge.Name = "lbl_Gauge"
        Me.lbl_Gauge.Location = New System.Drawing.Point(7, 69)
        Me.lbl_Gauge.Size = New System.Drawing.Size(133, 17)
        Me.lbl_Gauge.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_Gauge)

        ' lbl_recsts_Blue
        Me.lbl_recsts_Blue.Name = "lbl_recsts_Blue"
        Me.lbl_recsts_Blue.Location = New System.Drawing.Point(158, 59)
        Me.lbl_recsts_Blue.Size = New System.Drawing.Size(215, 14)
        Me.lbl_recsts_Blue.Text = "0% Completed"
        Me.lbl_recsts_Blue.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_recsts_Blue)

        ' lbl_recsts_White
        Me.lbl_recsts_White.Name = "lbl_recsts_White"
        Me.lbl_recsts_White.Location = New System.Drawing.Point(26, 78)
        Me.lbl_recsts_White.Size = New System.Drawing.Size(65, 14)
        Me.lbl_recsts_White.Text = "0%"
        Me.lbl_recsts_White.Visible = False
        Me.pnlDetail.Controls.Add(Me.lbl_recsts_White)

        Me.Name = "FrmStatusMeter"
        Me.Text = "Status Meter"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917977661440 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Rectangle_1917977660672 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_CommandButton_1917977671680 As System.Windows.Forms.Button
    Friend WithEvents unnamed_TextBox_1917977673408 As System.Windows.Forms.TextBox
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents cmd_Cancel As System.Windows.Forms.Button
    Friend WithEvents lbl_ProcName As System.Windows.Forms.Label
    Friend WithEvents lbl_ProcID As System.Windows.Forms.Label
    Friend WithEvents lbl_GaugeBox As System.Windows.Forms.Panel
    Friend WithEvents lbl_Gauge As System.Windows.Forms.Panel
    Friend WithEvents lbl_recsts_Blue As System.Windows.Forms.Label
    Friend WithEvents lbl_recsts_White As System.Windows.Forms.Label

End Class
