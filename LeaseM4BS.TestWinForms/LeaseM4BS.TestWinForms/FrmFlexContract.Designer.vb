<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmFlexContract
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
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.tblSearch = New System.Windows.Forms.TableLayoutPanel()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.lblContractNo = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvContractList = New System.Windows.Forms.DataGridView()
        Me.colMgmtUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colContractType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAccountTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPayee = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colContractNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOwnMgmt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colApprovalNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colReleaseCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colContractName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEndDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colContractPeriod = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCashPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMonthlyLease = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAssetQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUpdateDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colConsistency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlSearch.SuspendLayout()
        Me.tblSearch.SuspendLayout()
        CType(Me.dgvContractList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' pnlSearch
        '
        Me.pnlSearch.BackColor = System.Drawing.Color.White
        Me.pnlSearch.Controls.Add(Me.tblSearch)
        Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSearch.Location = New System.Drawing.Point(0, 0)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Padding = New System.Windows.Forms.Padding(8, 8, 8, 8)
        Me.pnlSearch.Size = New System.Drawing.Size(1200, 50)
        Me.pnlSearch.TabIndex = 0
        '
        ' tblSearch
        '
        Me.tblSearch.ColumnCount = 5
        Me.tblSearch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0F))
        Me.tblSearch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0F))
        Me.tblSearch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0F))
        Me.tblSearch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0F))
        Me.tblSearch.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0F))
        Me.tblSearch.Controls.Add(Me.lblUser, 0, 0)
        Me.tblSearch.Controls.Add(Me.txtUser, 1, 0)
        Me.tblSearch.Controls.Add(Me.lblContractNo, 2, 0)
        Me.tblSearch.Controls.Add(Me.txtContractNo, 3, 0)
        Me.tblSearch.Controls.Add(Me.btnSearch, 4, 0)
        Me.tblSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblSearch.Location = New System.Drawing.Point(8, 8)
        Me.tblSearch.Name = "tblSearch"
        Me.tblSearch.RowCount = 1
        Me.tblSearch.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0F))
        Me.tblSearch.Size = New System.Drawing.Size(1184, 34)
        Me.tblSearch.TabIndex = 0
        '
        ' lblUser
        '
        Me.lblUser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUser.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblUser.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblUser.Location = New System.Drawing.Point(3, 0)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblUser.Size = New System.Drawing.Size(74, 34)
        Me.lblUser.TabIndex = 0
        Me.lblUser.Text = "ユーザー"
        Me.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' txtUser
        '
        Me.txtUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUser.Font = New System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular)
        Me.txtUser.Location = New System.Drawing.Point(83, 5)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(194, 24)
        Me.txtUser.TabIndex = 1
        '
        ' lblContractNo
        '
        Me.lblContractNo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblContractNo.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblContractNo.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblContractNo.Location = New System.Drawing.Point(283, 0)
        Me.lblContractNo.Name = "lblContractNo"
        Me.lblContractNo.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblContractNo.Size = New System.Drawing.Size(94, 34)
        Me.lblContractNo.TabIndex = 2
        Me.lblContractNo.Text = "契約書番号"
        Me.lblContractNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' txtContractNo
        '
        Me.txtContractNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtContractNo.Font = New System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular)
        Me.txtContractNo.Location = New System.Drawing.Point(383, 5)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(194, 24)
        Me.txtContractNo.TabIndex = 3
        '
        ' btnSearch
        '
        Me.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnSearch.BackColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(123, Integer), CType(255, Integer))
        Me.btnSearch.FlatAppearance.BorderSize = 0
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnSearch.ForeColor = System.Drawing.Color.White
        Me.btnSearch.Location = New System.Drawing.Point(583, 3)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(90, 28)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        ' dgvContractList
        '
        Me.dgvContractList.AllowUserToAddRows = False
        Me.dgvContractList.AllowUserToDeleteRows = False
        Me.dgvContractList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
        Me.dgvContractList.BackgroundColor = System.Drawing.Color.White
        Me.dgvContractList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvContractList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvContractList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
        Me.dgvContractList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvContractList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {
            Me.colMgmtUnit, Me.colContractType, Me.colAccountTarget, Me.colPayee,
            Me.colContractNo, Me.colOwnMgmt, Me.colApprovalNo, Me.colReleaseCount,
            Me.colContractName, Me.colStartDate, Me.colEndDate, Me.colContractPeriod,
            Me.colCashPrice, Me.colMonthlyLease, Me.colAssetQty, Me.colUpdateDate,
            Me.colConsistency})
        Me.dgvContractList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvContractList.EnableHeadersVisualStyles = False
        Me.dgvContractList.GridColor = System.Drawing.Color.FromArgb(CType(222, Integer), CType(226, Integer), CType(230, Integer))
        Me.dgvContractList.Location = New System.Drawing.Point(0, 50)
        Me.dgvContractList.MultiSelect = False
        Me.dgvContractList.Name = "dgvContractList"
        Me.dgvContractList.ReadOnly = True
        Me.dgvContractList.RowHeadersVisible = False
        Me.dgvContractList.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.dgvContractList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvContractList.Size = New System.Drawing.Size(1200, 550)
        Me.dgvContractList.TabIndex = 1
        '
        ' colMgmtUnit
        '
        Me.colMgmtUnit.HeaderText = "管理単位"
        Me.colMgmtUnit.Name = "colMgmtUnit"
        Me.colMgmtUnit.ReadOnly = True
        Me.colMgmtUnit.Width = 80
        '
        ' colContractType
        '
        Me.colContractType.HeaderText = "契約区分"
        Me.colContractType.Name = "colContractType"
        Me.colContractType.ReadOnly = True
        Me.colContractType.Width = 80
        '
        ' colAccountTarget
        '
        Me.colAccountTarget.HeaderText = "計上対象"
        Me.colAccountTarget.Name = "colAccountTarget"
        Me.colAccountTarget.ReadOnly = True
        Me.colAccountTarget.Width = 80
        '
        ' colPayee
        '
        Me.colPayee.HeaderText = "支払先"
        Me.colPayee.Name = "colPayee"
        Me.colPayee.ReadOnly = True
        Me.colPayee.Width = 120
        '
        ' colContractNo
        '
        Me.colContractNo.HeaderText = "契約番号"
        Me.colContractNo.Name = "colContractNo"
        Me.colContractNo.ReadOnly = True
        Me.colContractNo.Width = 120
        '
        ' colOwnMgmt
        '
        Me.colOwnMgmt.HeaderText = "自社管理"
        Me.colOwnMgmt.Name = "colOwnMgmt"
        Me.colOwnMgmt.ReadOnly = True
        Me.colOwnMgmt.Width = 100
        '
        ' colApprovalNo
        '
        Me.colApprovalNo.HeaderText = "稟議番号"
        Me.colApprovalNo.Name = "colApprovalNo"
        Me.colApprovalNo.ReadOnly = True
        Me.colApprovalNo.Width = 100
        '
        ' colReleaseCount
        '
        Me.colReleaseCount.HeaderText = "再リース回数"
        Me.colReleaseCount.Name = "colReleaseCount"
        Me.colReleaseCount.ReadOnly = True
        Me.colReleaseCount.Width = 90
        '
        ' colContractName
        '
        Me.colContractName.HeaderText = "契約名"
        Me.colContractName.Name = "colContractName"
        Me.colContractName.ReadOnly = True
        Me.colContractName.Width = 200
        '
        ' colStartDate
        '
        Me.colStartDate.HeaderText = "開始日"
        Me.colStartDate.Name = "colStartDate"
        Me.colStartDate.ReadOnly = True
        Me.colStartDate.Width = 100
        '
        ' colEndDate
        '
        Me.colEndDate.HeaderText = "終了日"
        Me.colEndDate.Name = "colEndDate"
        Me.colEndDate.ReadOnly = True
        Me.colEndDate.Width = 100
        '
        ' colContractPeriod
        '
        Me.colContractPeriod.HeaderText = "契約期間"
        Me.colContractPeriod.Name = "colContractPeriod"
        Me.colContractPeriod.ReadOnly = True
        Me.colContractPeriod.Width = 80
        '
        ' colCashPrice
        '
        Me.colCashPrice.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {
            .Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight,
            .Format = "N0"
        }
        Me.colCashPrice.HeaderText = "現金購入価額"
        Me.colCashPrice.Name = "colCashPrice"
        Me.colCashPrice.ReadOnly = True
        Me.colCashPrice.Width = 120
        '
        ' colMonthlyLease
        '
        Me.colMonthlyLease.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {
            .Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight,
            .Format = "N0"
        }
        Me.colMonthlyLease.HeaderText = "月額リース料"
        Me.colMonthlyLease.Name = "colMonthlyLease"
        Me.colMonthlyLease.ReadOnly = True
        Me.colMonthlyLease.Width = 120
        '
        ' colAssetQty
        '
        Me.colAssetQty.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {
            .Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        }
        Me.colAssetQty.HeaderText = "資産数量"
        Me.colAssetQty.Name = "colAssetQty"
        Me.colAssetQty.ReadOnly = True
        Me.colAssetQty.Width = 80
        '
        ' colUpdateDate
        '
        Me.colUpdateDate.HeaderText = "更新日時"
        Me.colUpdateDate.Name = "colUpdateDate"
        Me.colUpdateDate.ReadOnly = True
        Me.colUpdateDate.Width = 140
        '
        ' colConsistency
        '
        Me.colConsistency.HeaderText = "整合"
        Me.colConsistency.Name = "colConsistency"
        Me.colConsistency.ReadOnly = True
        Me.colConsistency.Width = 60
        '
        ' FrmFlexContract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.Controls.Add(Me.dgvContractList)
        Me.Controls.Add(Me.pnlSearch)
        Me.Name = "FrmFlexContract"
        Me.Size = New System.Drawing.Size(1200, 600)
        Me.pnlSearch.ResumeLayout(False)
        Me.tblSearch.ResumeLayout(False)
        Me.tblSearch.PerformLayout()
        CType(Me.dgvContractList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents tblSearch As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents lblContractNo As System.Windows.Forms.Label
    Friend WithEvents txtContractNo As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents dgvContractList As System.Windows.Forms.DataGridView
    Friend WithEvents colMgmtUnit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colContractType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAccountTarget As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPayee As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colContractNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOwnMgmt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colApprovalNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colReleaseCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colContractName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStartDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEndDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colContractPeriod As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCashPrice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMonthlyLease As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAssetQty As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUpdateDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colConsistency As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
