<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexMenu
    Inherits System.Windows.Forms.Form

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
        Me.pnlMenuBar = New System.Windows.Forms.Panel()
        Me.flowMenu = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnContract = New System.Windows.Forms.Button()
        Me.btnROUAsset = New System.Windows.Forms.Button()
        Me.btnMonthlyPayments = New System.Windows.Forms.Button()
        Me.btnMonthlyAccounting = New System.Windows.Forms.Button()
        Me.btnPeriodBalance = New System.Windows.Forms.Button()
        Me.btnTaxAdjustment = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.pnlMenuBar.SuspendLayout()
        Me.SuspendLayout()
        '
        ' pnlMenuBar
        '
        Me.pnlMenuBar.BackColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.pnlMenuBar.Controls.Add(Me.flowMenu)
        Me.pnlMenuBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlMenuBar.Location = New System.Drawing.Point(0, 0)
        Me.pnlMenuBar.Name = "pnlMenuBar"
        Me.pnlMenuBar.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlMenuBar.Size = New System.Drawing.Size(1400, 40)
        Me.pnlMenuBar.TabIndex = 0
        '
        ' flowMenu
        '
        Me.flowMenu.Controls.Add(Me.btnContract)
        Me.flowMenu.Controls.Add(Me.btnROUAsset)
        Me.flowMenu.Controls.Add(Me.btnMonthlyPayments)
        Me.flowMenu.Controls.Add(Me.btnMonthlyAccounting)
        Me.flowMenu.Controls.Add(Me.btnPeriodBalance)
        Me.flowMenu.Controls.Add(Me.btnTaxAdjustment)
        Me.flowMenu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowMenu.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
        Me.flowMenu.Location = New System.Drawing.Point(4, 4)
        Me.flowMenu.Name = "flowMenu"
        Me.flowMenu.Size = New System.Drawing.Size(1392, 32)
        Me.flowMenu.TabIndex = 0
        Me.flowMenu.WrapContents = False
        '
        ' btnContract
        '
        Me.btnContract.FlatAppearance.BorderSize = 0
        Me.btnContract.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnContract.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnContract.ForeColor = System.Drawing.Color.White
        Me.btnContract.Location = New System.Drawing.Point(0, 0)
        Me.btnContract.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnContract.Name = "btnContract"
        Me.btnContract.Size = New System.Drawing.Size(160, 32)
        Me.btnContract.TabIndex = 0
        Me.btnContract.Text = "契約書(フレックス)"
        Me.btnContract.UseVisualStyleBackColor = True
        '
        ' btnROUAsset
        '
        Me.btnROUAsset.FlatAppearance.BorderSize = 0
        Me.btnROUAsset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnROUAsset.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnROUAsset.ForeColor = System.Drawing.Color.White
        Me.btnROUAsset.Location = New System.Drawing.Point(162, 0)
        Me.btnROUAsset.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnROUAsset.Name = "btnROUAsset"
        Me.btnROUAsset.Size = New System.Drawing.Size(190, 32)
        Me.btnROUAsset.TabIndex = 1
        Me.btnROUAsset.Text = "使用権資産(フレックス)"
        Me.btnROUAsset.UseVisualStyleBackColor = True
        '
        ' btnMonthlyPayments
        '
        Me.btnMonthlyPayments.FlatAppearance.BorderSize = 0
        Me.btnMonthlyPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMonthlyPayments.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnMonthlyPayments.ForeColor = System.Drawing.Color.White
        Me.btnMonthlyPayments.Location = New System.Drawing.Point(354, 0)
        Me.btnMonthlyPayments.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMonthlyPayments.Name = "btnMonthlyPayments"
        Me.btnMonthlyPayments.Size = New System.Drawing.Size(170, 32)
        Me.btnMonthlyPayments.TabIndex = 2
        Me.btnMonthlyPayments.Text = "月次支払(フレックス)"
        Me.btnMonthlyPayments.UseVisualStyleBackColor = True
        '
        ' btnMonthlyAccounting
        '
        Me.btnMonthlyAccounting.FlatAppearance.BorderSize = 0
        Me.btnMonthlyAccounting.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMonthlyAccounting.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnMonthlyAccounting.ForeColor = System.Drawing.Color.White
        Me.btnMonthlyAccounting.Location = New System.Drawing.Point(526, 0)
        Me.btnMonthlyAccounting.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMonthlyAccounting.Name = "btnMonthlyAccounting"
        Me.btnMonthlyAccounting.Size = New System.Drawing.Size(170, 32)
        Me.btnMonthlyAccounting.TabIndex = 3
        Me.btnMonthlyAccounting.Text = "月次会計(フレックス)"
        Me.btnMonthlyAccounting.UseVisualStyleBackColor = True
        '
        ' btnPeriodBalance
        '
        Me.btnPeriodBalance.FlatAppearance.BorderSize = 0
        Me.btnPeriodBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPeriodBalance.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnPeriodBalance.ForeColor = System.Drawing.Color.White
        Me.btnPeriodBalance.Location = New System.Drawing.Point(698, 0)
        Me.btnPeriodBalance.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnPeriodBalance.Name = "btnPeriodBalance"
        Me.btnPeriodBalance.Size = New System.Drawing.Size(170, 32)
        Me.btnPeriodBalance.TabIndex = 4
        Me.btnPeriodBalance.Text = "期間残高(フレックス)"
        Me.btnPeriodBalance.UseVisualStyleBackColor = True
        '
        ' btnTaxAdjustment
        '
        Me.btnTaxAdjustment.FlatAppearance.BorderSize = 0
        Me.btnTaxAdjustment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTaxAdjustment.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnTaxAdjustment.ForeColor = System.Drawing.Color.White
        Me.btnTaxAdjustment.Location = New System.Drawing.Point(870, 0)
        Me.btnTaxAdjustment.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnTaxAdjustment.Name = "btnTaxAdjustment"
        Me.btnTaxAdjustment.Size = New System.Drawing.Size(170, 32)
        Me.btnTaxAdjustment.TabIndex = 5
        Me.btnTaxAdjustment.Text = "税法調整(フレックス)"
        Me.btnTaxAdjustment.UseVisualStyleBackColor = True
        '
        ' pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 40)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(1400, 910)
        Me.pnlContent.TabIndex = 1
        '
        ' FrmFlexMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.ClientSize = New System.Drawing.Size(1400, 950)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.pnlMenuBar)
        Me.Font = New System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(1280, 800)
        Me.Name = "FrmFlexMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "新リース会計対応 フレックスメニュー"
        Me.pnlMenuBar.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlMenuBar As System.Windows.Forms.Panel
    Friend WithEvents flowMenu As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnContract As System.Windows.Forms.Button
    Friend WithEvents btnROUAsset As System.Windows.Forms.Button
    Friend WithEvents btnMonthlyPayments As System.Windows.Forms.Button
    Friend WithEvents btnMonthlyAccounting As System.Windows.Forms.Button
    Friend WithEvents btnPeriodBalance As System.Windows.Forms.Button
    Friend WithEvents btnTaxAdjustment As System.Windows.Forms.Button
    Friend WithEvents pnlContent As System.Windows.Forms.Panel

End Class
