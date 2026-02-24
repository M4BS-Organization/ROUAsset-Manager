<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAssetDetailEntry
    Inherits PopupBaseForm

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
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabAsset = New System.Windows.Forms.TabPage()
        Me.tabInitialCost = New System.Windows.Forms.TabPage()
        Me.tabMonthlyDetail = New System.Windows.Forms.TabPage()
        Me.grpProperty = New System.Windows.Forms.GroupBox()
        Me.tblProperty = New System.Windows.Forms.TableLayoutPanel()
        Me.lblPropertyName = New System.Windows.Forms.Label()
        Me.txtPropertyName = New System.Windows.Forms.TextBox()
        Me.lblLocation = New System.Windows.Forms.Label()
        Me.txtLocation = New System.Windows.Forms.TextBox()
        Me.lblSection = New System.Windows.Forms.Label()
        Me.txtSection = New System.Windows.Forms.TextBox()
        Me.lblArea = New System.Windows.Forms.Label()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.lblLayout = New System.Windows.Forms.Label()
        Me.txtLayout = New System.Windows.Forms.TextBox()
        Me.lblStructure = New System.Windows.Forms.Label()
        Me.txtStructure = New System.Windows.Forms.TextBox()
        Me.lblUsefulLife = New System.Windows.Forms.Label()
        Me.numUsefulLife = New System.Windows.Forms.NumericUpDown()
        Me.lblCompletion = New System.Windows.Forms.Label()
        Me.dtpCompletion = New System.Windows.Forms.DateTimePicker()
        Me.lblBuildingAgeCaption = New System.Windows.Forms.Label()
        Me.lblBuildingAge = New System.Windows.Forms.Label()
        Me.lblLandlordName = New System.Windows.Forms.Label()
        Me.txtLandlordName = New System.Windows.Forms.TextBox()
        Me.lblBrokerCompany = New System.Windows.Forms.Label()
        Me.txtBrokerCompany = New System.Windows.Forms.TextBox()
        Me.lblUsageRestrictions = New System.Windows.Forms.Label()
        Me.txtUsageRestrictions = New System.Windows.Forms.TextBox()
        Me.lblPaymentAgent = New System.Windows.Forms.Label()
        Me.txtPaymentAgent = New System.Windows.Forms.TextBox()
        Me.lblGuarantor = New System.Windows.Forms.Label()
        Me.txtGuarantor = New System.Windows.Forms.TextBox()
        Me.lblSelfEquipment = New System.Windows.Forms.Label()
        Me.dgvSelfEquipment = New System.Windows.Forms.DataGridView()
        Me.colSelfEquipName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSelfEquipDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSelfEquipAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.grpAllocations = New System.Windows.Forms.GroupBox()
        Me.dgvAllocations = New System.Windows.Forms.DataGridView()
        Me.colAllocId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDeptNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCostDeptName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAllocRate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colValidStartDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colValidEndDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colJournalPatternId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblAccountClass = New System.Windows.Forms.Label()
        Me.cmbAccountClass = New System.Windows.Forms.ComboBox()
        Me.lblAssetNo = New System.Windows.Forms.Label()
        Me.txtAssetNo = New System.Windows.Forms.TextBox()
        Me.lblQuantity = New System.Windows.Forms.Label()
        Me.numQuantity = New System.Windows.Forms.NumericUpDown()
        Me.grpInitialCost = New System.Windows.Forms.GroupBox()
        Me.tblInitialCost = New System.Windows.Forms.TableLayoutPanel()
        Me.lblShikikin = New System.Windows.Forms.Label()
        Me.txtShikikin = New System.Windows.Forms.TextBox()
        Me.lblHoshokin = New System.Windows.Forms.Label()
        Me.txtHoshokin = New System.Windows.Forms.TextBox()
        Me.lblReikin = New System.Windows.Forms.Label()
        Me.txtReikin = New System.Windows.Forms.TextBox()
        Me.lblBrokerFee = New System.Windows.Forms.Label()
        Me.txtBrokerFee = New System.Windows.Forms.TextBox()
        Me.lblPrepaidRent = New System.Windows.Forms.Label()
        Me.txtPrepaidRent = New System.Windows.Forms.TextBox()
        Me.pnlBottom.SuspendLayout()
        Me.tabControl.SuspendLayout()
        Me.tabAsset.SuspendLayout()
        Me.tabInitialCost.SuspendLayout()
        Me.grpProperty.SuspendLayout()
        Me.tblProperty.SuspendLayout()
        CType(Me.numUsefulLife, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvSelfEquipment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAllocations.SuspendLayout()
        CType(Me.dgvAllocations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpInitialCost.SuspendLayout()
        Me.tblInitialCost.SuspendLayout()
        Me.SuspendLayout()
        '
        ' pnlBottom
        '
        Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(240, Integer), CType(240, Integer))
        Me.pnlBottom.Controls.Add(Me.btnCancel)
        Me.pnlBottom.Controls.Add(Me.btnSave)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 730)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(800, 50)
        Me.pnlBottom.TabIndex = 2
        '
        ' btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(40, Integer), CType(167, Integer), CType(69, Integer))
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(598, 12)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 28)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "保存"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        ' btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(108, Integer), CType(117, Integer), CType(125, Integer))
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(694, 12)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        ' tabControl
        '
        Me.tabControl.Controls.Add(Me.tabAsset)
        Me.tabControl.Controls.Add(Me.tabInitialCost)
        Me.tabControl.Controls.Add(Me.tabMonthlyDetail)
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Font = New System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Bold)
        Me.tabControl.Location = New System.Drawing.Point(0, 0)
        Me.tabControl.Name = "tabControl"
        Me.tabControl.Padding = New System.Drawing.Point(12, 4)
        Me.tabControl.SelectedIndex = 0
        Me.tabControl.Size = New System.Drawing.Size(800, 730)
        Me.tabControl.TabIndex = 0
        '
        ' tabAsset
        '
        Me.tabAsset.AutoScroll = True
        Me.tabAsset.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.tabAsset.Controls.Add(Me.grpAllocations)
        Me.tabAsset.Controls.Add(Me.grpProperty)
        Me.tabAsset.Location = New System.Drawing.Point(4, 30)
        Me.tabAsset.Name = "tabAsset"
        Me.tabAsset.Padding = New System.Windows.Forms.Padding(8)
        Me.tabAsset.Size = New System.Drawing.Size(792, 696)
        Me.tabAsset.TabIndex = 0
        Me.tabAsset.Text = "資産"
        '
        ' tabInitialCost
        '
        Me.tabInitialCost.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.tabInitialCost.Controls.Add(Me.grpInitialCost)
        Me.tabInitialCost.Location = New System.Drawing.Point(4, 30)
        Me.tabInitialCost.Name = "tabInitialCost"
        Me.tabInitialCost.Padding = New System.Windows.Forms.Padding(8)
        Me.tabInitialCost.Size = New System.Drawing.Size(792, 696)
        Me.tabInitialCost.TabIndex = 1
        Me.tabInitialCost.Text = "初回金"
        '
        ' tabMonthlyDetail
        '
        Me.tabMonthlyDetail.AutoScroll = True
        Me.tabMonthlyDetail.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.tabMonthlyDetail.Location = New System.Drawing.Point(4, 30)
        Me.tabMonthlyDetail.Name = "tabMonthlyDetail"
        Me.tabMonthlyDetail.Padding = New System.Windows.Forms.Padding(8)
        Me.tabMonthlyDetail.Size = New System.Drawing.Size(792, 696)
        Me.tabMonthlyDetail.TabIndex = 2
        Me.tabMonthlyDetail.Text = "月次明細"
        '
        ' grpProperty
        '
        Me.grpProperty.BackColor = System.Drawing.Color.White
        Me.grpProperty.Controls.Add(Me.dgvSelfEquipment)
        Me.grpProperty.Controls.Add(Me.lblSelfEquipment)
        Me.grpProperty.Controls.Add(Me.tblProperty)
        Me.grpProperty.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpProperty.Font = New System.Drawing.Font("Meiryo", 10.0F, System.Drawing.FontStyle.Bold)
        Me.grpProperty.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.grpProperty.Location = New System.Drawing.Point(8, 8)
        Me.grpProperty.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Me.grpProperty.Name = "grpProperty"
        Me.grpProperty.Padding = New System.Windows.Forms.Padding(6, 12, 6, 6)
        Me.grpProperty.Size = New System.Drawing.Size(776, 432)
        Me.grpProperty.TabIndex = 0
        Me.grpProperty.TabStop = False
        Me.grpProperty.Text = "基本情報"
        '
        ' tblProperty
        '
        Me.tblProperty.ColumnCount = 6
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0F))
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0F))
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0F))
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.0F))
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0F))
        Me.tblProperty.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.0F))
        Me.tblProperty.RowCount = 7
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.Dock = System.Windows.Forms.DockStyle.Top
        Me.tblProperty.Location = New System.Drawing.Point(6, 24)
        Me.tblProperty.Name = "tblProperty"
        Me.tblProperty.Padding = New System.Windows.Forms.Padding(4)
        Me.tblProperty.Size = New System.Drawing.Size(764, 232)
        Me.tblProperty.TabIndex = 0
        '
        ' Labels
        '
        '
        Me.lblPropertyName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPropertyName.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblPropertyName.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblPropertyName.Name = "lblPropertyName"
        Me.lblPropertyName.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblPropertyName.Text = "物件名"
        Me.lblPropertyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLocation.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblLocation.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblLocation.Text = "所在地"
        Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblSection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSection.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblSection.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblSection.Name = "lblSection"
        Me.lblSection.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblSection.Text = "区画"
        Me.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblArea.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblArea.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblArea.Name = "lblArea"
        Me.lblArea.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblArea.Text = "面積(㎡)"
        Me.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLayout.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblLayout.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblLayout.Name = "lblLayout"
        Me.lblLayout.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblLayout.Text = "間取り"
        Me.lblLayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblStructure.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStructure.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblStructure.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblStructure.Name = "lblStructure"
        Me.lblStructure.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblStructure.Text = "構造・用途"
        Me.lblStructure.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblUsefulLife.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUsefulLife.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblUsefulLife.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblUsefulLife.Name = "lblUsefulLife"
        Me.lblUsefulLife.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblUsefulLife.Text = "耐用年数"
        Me.lblUsefulLife.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblCompletion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCompletion.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblCompletion.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblCompletion.Name = "lblCompletion"
        Me.lblCompletion.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblCompletion.Text = "竣工"
        Me.lblCompletion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblBuildingAgeCaption.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBuildingAgeCaption.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblBuildingAgeCaption.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblBuildingAgeCaption.Name = "lblBuildingAgeCaption"
        Me.lblBuildingAgeCaption.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblBuildingAgeCaption.Text = "築年数"
        Me.lblBuildingAgeCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblLandlordName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLandlordName.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblLandlordName.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblLandlordName.Name = "lblLandlordName"
        Me.lblLandlordName.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblLandlordName.Text = "貸主名"
        Me.lblLandlordName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblBrokerCompany.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBrokerCompany.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblBrokerCompany.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblBrokerCompany.Name = "lblBrokerCompany"
        Me.lblBrokerCompany.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblBrokerCompany.Text = "仲介会社"
        Me.lblBrokerCompany.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblUsageRestrictions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblUsageRestrictions.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblUsageRestrictions.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblUsageRestrictions.Name = "lblUsageRestrictions"
        Me.lblUsageRestrictions.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblUsageRestrictions.Text = "用途・制限"
        Me.lblUsageRestrictions.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblPaymentAgent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPaymentAgent.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblPaymentAgent.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblPaymentAgent.Name = "lblPaymentAgent"
        Me.lblPaymentAgent.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblPaymentAgent.Text = "決済代行"
        Me.lblPaymentAgent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblGuarantor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblGuarantor.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblGuarantor.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblGuarantor.Name = "lblGuarantor"
        Me.lblGuarantor.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblGuarantor.Text = "連帯保証人"
        Me.lblGuarantor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblAccountClass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAccountClass.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblAccountClass.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblAccountClass.Name = "lblAccountClass"
        Me.lblAccountClass.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblAccountClass.Text = "計上区分"
        Me.lblAccountClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblAssetNo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAssetNo.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblAssetNo.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblAssetNo.Name = "lblAssetNo"
        Me.lblAssetNo.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblAssetNo.Text = "資産番号"
        Me.lblAssetNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblQuantity.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblQuantity.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblQuantity.Text = "数量"
        Me.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' Input Controls
        '
        '
        Me.cmbAccountClass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbAccountClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAccountClass.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.cmbAccountClass.Name = "cmbAccountClass"
        Me.cmbAccountClass.TabIndex = 0
        '
        Me.txtAssetNo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAssetNo.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtAssetNo.Name = "txtAssetNo"
        Me.txtAssetNo.ReadOnly = True
        Me.txtAssetNo.BackColor = System.Drawing.Color.FromArgb(CType(233, Integer), CType(236, Integer), CType(239, Integer))
        Me.txtAssetNo.TabIndex = 0
        '
        Me.numQuantity.Dock = System.Windows.Forms.DockStyle.Fill
        Me.numQuantity.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.numQuantity.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numQuantity.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.numQuantity.Name = "numQuantity"
        Me.numQuantity.TabIndex = 0
        Me.numQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numQuantity.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        Me.txtPropertyName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPropertyName.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtPropertyName.Name = "txtPropertyName"
        Me.txtPropertyName.TabIndex = 1
        '
        Me.txtLocation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLocation.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.TabIndex = 2
        '
        Me.txtSection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtSection.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtSection.Name = "txtSection"
        Me.txtSection.TabIndex = 3
        '
        Me.txtArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtArea.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.TabIndex = 4
        '
        Me.txtLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLayout.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtLayout.Name = "txtLayout"
        Me.txtLayout.TabIndex = 5
        '
        Me.txtStructure.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtStructure.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtStructure.Name = "txtStructure"
        Me.txtStructure.TabIndex = 6
        '
        Me.numUsefulLife.Dock = System.Windows.Forms.DockStyle.Fill
        Me.numUsefulLife.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.numUsefulLife.Maximum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numUsefulLife.Name = "numUsefulLife"
        Me.numUsefulLife.TabIndex = 7
        Me.numUsefulLife.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numUsefulLife.Value = New Decimal(New Integer() {47, 0, 0, 0})
        '
        Me.dtpCompletion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpCompletion.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.dtpCompletion.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpCompletion.Name = "dtpCompletion"
        Me.dtpCompletion.ShowCheckBox = True
        Me.dtpCompletion.TabIndex = 8
        '
        Me.lblBuildingAge.BackColor = System.Drawing.Color.FromArgb(CType(233, Integer), CType(236, Integer), CType(239, Integer))
        Me.lblBuildingAge.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBuildingAge.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.lblBuildingAge.ForeColor = System.Drawing.Color.FromArgb(CType(33, Integer), CType(37, Integer), CType(41, Integer))
        Me.lblBuildingAge.Name = "lblBuildingAge"
        Me.lblBuildingAge.Text = "---年"
        Me.lblBuildingAge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        Me.txtLandlordName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLandlordName.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtLandlordName.Name = "txtLandlordName"
        Me.txtLandlordName.TabIndex = 9
        '
        Me.txtBrokerCompany.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrokerCompany.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtBrokerCompany.Name = "txtBrokerCompany"
        Me.txtBrokerCompany.TabIndex = 10
        '
        Me.txtPaymentAgent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPaymentAgent.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtPaymentAgent.Name = "txtPaymentAgent"
        Me.txtPaymentAgent.TabIndex = 12
        '
        Me.txtGuarantor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtGuarantor.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtGuarantor.Name = "txtGuarantor"
        Me.txtGuarantor.TabIndex = 13
        '
        Me.txtUsageRestrictions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUsageRestrictions.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtUsageRestrictions.Multiline = True
        Me.txtUsageRestrictions.Name = "txtUsageRestrictions"
        Me.txtUsageRestrictions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtUsageRestrictions.TabIndex = 11
        '
        ' tblProperty - Add controls to layout
        '
        ' Row 0: 計上区分 / 資産番号 / 数量
        Me.tblProperty.Controls.Add(Me.lblAccountClass, 0, 0)
        Me.tblProperty.Controls.Add(Me.cmbAccountClass, 1, 0)
        Me.tblProperty.Controls.Add(Me.lblAssetNo, 2, 0)
        Me.tblProperty.Controls.Add(Me.txtAssetNo, 3, 0)
        Me.tblProperty.Controls.Add(Me.lblQuantity, 4, 0)
        Me.tblProperty.Controls.Add(Me.numQuantity, 5, 0)
        ' Row 1: 物件名
        Me.tblProperty.Controls.Add(Me.lblPropertyName, 0, 1)
        Me.tblProperty.Controls.Add(Me.txtPropertyName, 1, 1)
        Me.tblProperty.SetColumnSpan(Me.txtPropertyName, 5)
        ' Row 2: 所在地 / 区画
        Me.tblProperty.Controls.Add(Me.lblLocation, 0, 2)
        Me.tblProperty.Controls.Add(Me.txtLocation, 1, 2)
        Me.tblProperty.SetColumnSpan(Me.txtLocation, 3)
        Me.tblProperty.Controls.Add(Me.lblSection, 4, 2)
        Me.tblProperty.Controls.Add(Me.txtSection, 5, 2)
        ' Row 3: 面積 / 間取り / 構造・用途
        Me.tblProperty.Controls.Add(Me.lblArea, 0, 3)
        Me.tblProperty.Controls.Add(Me.txtArea, 1, 3)
        Me.tblProperty.Controls.Add(Me.lblLayout, 2, 3)
        Me.tblProperty.Controls.Add(Me.txtLayout, 3, 3)
        Me.tblProperty.Controls.Add(Me.lblStructure, 4, 3)
        Me.tblProperty.Controls.Add(Me.txtStructure, 5, 3)
        ' Row 4: 耐用年数 / 竣工 / 築年数
        Me.tblProperty.Controls.Add(Me.lblUsefulLife, 0, 4)
        Me.tblProperty.Controls.Add(Me.numUsefulLife, 1, 4)
        Me.tblProperty.Controls.Add(Me.lblCompletion, 2, 4)
        Me.tblProperty.Controls.Add(Me.dtpCompletion, 3, 4)
        Me.tblProperty.Controls.Add(Me.lblBuildingAgeCaption, 4, 4)
        Me.tblProperty.Controls.Add(Me.lblBuildingAge, 5, 4)
        ' Row 5: 貸主名 / 仲介会社 / 用途・制限
        Me.tblProperty.Controls.Add(Me.lblLandlordName, 0, 5)
        Me.tblProperty.Controls.Add(Me.txtLandlordName, 1, 5)
        Me.tblProperty.Controls.Add(Me.lblBrokerCompany, 2, 5)
        Me.tblProperty.Controls.Add(Me.txtBrokerCompany, 3, 5)
        Me.tblProperty.Controls.Add(Me.lblUsageRestrictions, 4, 5)
        Me.tblProperty.Controls.Add(Me.txtUsageRestrictions, 5, 5)
        Me.tblProperty.SetRowSpan(Me.txtUsageRestrictions, 2)
        ' Row 6: 決済代行 / 連帯保証人
        Me.tblProperty.Controls.Add(Me.lblPaymentAgent, 0, 6)
        Me.tblProperty.Controls.Add(Me.txtPaymentAgent, 1, 6)
        Me.tblProperty.Controls.Add(Me.lblGuarantor, 2, 6)
        Me.tblProperty.Controls.Add(Me.txtGuarantor, 3, 6)
        '
        ' lblSelfEquipment
        '
        Me.lblSelfEquipment.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(244, Integer), CType(248, Integer))
        Me.lblSelfEquipment.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblSelfEquipment.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblSelfEquipment.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.lblSelfEquipment.Name = "lblSelfEquipment"
        Me.lblSelfEquipment.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblSelfEquipment.Size = New System.Drawing.Size(764, 24)
        Me.lblSelfEquipment.TabIndex = 1
        Me.lblSelfEquipment.Text = "自己設備明細"
        Me.lblSelfEquipment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' dgvSelfEquipment
        '
        Me.dgvSelfEquipment.AllowUserToAddRows = True
        Me.dgvSelfEquipment.BackgroundColor = System.Drawing.Color.White
        Me.dgvSelfEquipment.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvSelfEquipment.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(244, Integer), CType(248, Integer))
        Me.dgvSelfEquipment.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.dgvSelfEquipment.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.dgvSelfEquipment.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dgvSelfEquipment.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSelfEquipName, Me.colSelfEquipDate, Me.colSelfEquipAmount})
        Me.dgvSelfEquipment.DefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.dgvSelfEquipment.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(33, Integer), CType(37, Integer), CType(41, Integer))
        Me.dgvSelfEquipment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSelfEquipment.EnableHeadersVisualStyles = False
        Me.dgvSelfEquipment.GridColor = System.Drawing.Color.FromArgb(CType(222, Integer), CType(226, Integer), CType(230, Integer))
        Me.dgvSelfEquipment.Location = New System.Drawing.Point(6, 248)
        Me.dgvSelfEquipment.Name = "dgvSelfEquipment"
        Me.dgvSelfEquipment.RowHeadersVisible = False
        Me.dgvSelfEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvSelfEquipment.Size = New System.Drawing.Size(764, 146)
        Me.dgvSelfEquipment.TabIndex = 14
        Me.dgvSelfEquipment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        '
        ' colSelfEquipName
        '
        Me.colSelfEquipName.HeaderText = "設備名"
        Me.colSelfEquipName.Name = "SelfEquipName"
        '
        ' colSelfEquipDate
        '
        Me.colSelfEquipDate.HeaderText = "日付"
        Me.colSelfEquipDate.Name = "SelfEquipDate"
        '
        ' colSelfEquipAmount
        '
        Me.colSelfEquipAmount.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colSelfEquipAmount.DefaultCellStyle.Format = "N0"
        Me.colSelfEquipAmount.HeaderText = "金額"
        Me.colSelfEquipAmount.Name = "SelfEquipAmount"
        '
        ' grpAllocations
        '
        Me.grpAllocations.BackColor = System.Drawing.Color.White
        Me.grpAllocations.Controls.Add(Me.dgvAllocations)
        Me.grpAllocations.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpAllocations.Font = New System.Drawing.Font("Meiryo", 10.0F, System.Drawing.FontStyle.Bold)
        Me.grpAllocations.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.grpAllocations.Location = New System.Drawing.Point(8, 408)
        Me.grpAllocations.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Me.grpAllocations.Name = "grpAllocations"
        Me.grpAllocations.Padding = New System.Windows.Forms.Padding(6, 12, 6, 6)
        Me.grpAllocations.Size = New System.Drawing.Size(776, 210)
        Me.grpAllocations.TabIndex = 1
        Me.grpAllocations.TabStop = False
        Me.grpAllocations.Text = "配賦行情報"
        '
        ' dgvAllocations
        '
        Me.dgvAllocations.AllowUserToAddRows = True
        Me.dgvAllocations.BackgroundColor = System.Drawing.Color.White
        Me.dgvAllocations.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvAllocations.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(244, Integer), CType(248, Integer))
        Me.dgvAllocations.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.dgvAllocations.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.dgvAllocations.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dgvAllocations.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colAllocId, Me.colDeptNo, Me.colCostDeptName, Me.colAllocRate, Me.colValidStartDate, Me.colValidEndDate, Me.colJournalPatternId})
        Me.dgvAllocations.DefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.dgvAllocations.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(33, Integer), CType(37, Integer), CType(41, Integer))
        Me.dgvAllocations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAllocations.EnableHeadersVisualStyles = False
        Me.dgvAllocations.GridColor = System.Drawing.Color.FromArgb(CType(222, Integer), CType(226, Integer), CType(230, Integer))
        Me.dgvAllocations.Location = New System.Drawing.Point(6, 24)
        Me.dgvAllocations.Name = "dgvAllocations"
        Me.dgvAllocations.RowHeadersVisible = False
        Me.dgvAllocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvAllocations.Size = New System.Drawing.Size(764, 180)
        Me.dgvAllocations.TabIndex = 15
        Me.dgvAllocations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        '
        ' colAllocId
        '
        Me.colAllocId.HeaderText = "配賦行ID"
        Me.colAllocId.Name = "AllocId"
        Me.colAllocId.ReadOnly = True
        Me.colAllocId.Visible = False
        '
        ' colDeptNo
        '
        Me.colDeptNo.HeaderText = "部署番号"
        Me.colDeptNo.Name = "DeptNo"
        Me.colDeptNo.FillWeight = 80.0F
        '
        ' colCostDeptName
        '
        Me.colCostDeptName.HeaderText = "費用負担部署"
        Me.colCostDeptName.Name = "CostDeptName"
        Me.colCostDeptName.FillWeight = 120.0F
        '
        ' colAllocRate
        '
        Me.colAllocRate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colAllocRate.HeaderText = "配賦率(%)"
        Me.colAllocRate.Name = "AllocRate"
        Me.colAllocRate.FillWeight = 80.0F
        '
        ' colValidStartDate
        '
        Me.colValidStartDate.HeaderText = "有効開始日"
        Me.colValidStartDate.Name = "ValidStartDate"
        Me.colValidStartDate.FillWeight = 90.0F
        '
        ' colValidEndDate
        '
        Me.colValidEndDate.HeaderText = "有効終了日"
        Me.colValidEndDate.Name = "ValidEndDate"
        Me.colValidEndDate.FillWeight = 90.0F
        '
        ' colJournalPatternId
        '
        Me.colJournalPatternId.HeaderText = "仕訳パターンID"
        Me.colJournalPatternId.Name = "JournalPatternId"
        Me.colJournalPatternId.FillWeight = 100.0F
        '
        ' grpInitialCost
        '
        Me.grpInitialCost.BackColor = System.Drawing.Color.White
        Me.grpInitialCost.Controls.Add(Me.tblInitialCost)
        Me.grpInitialCost.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpInitialCost.Font = New System.Drawing.Font("Meiryo", 10.0F, System.Drawing.FontStyle.Bold)
        Me.grpInitialCost.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.grpInitialCost.Location = New System.Drawing.Point(8, 8)
        Me.grpInitialCost.Name = "grpInitialCost"
        Me.grpInitialCost.Padding = New System.Windows.Forms.Padding(6, 12, 6, 6)
        Me.grpInitialCost.Size = New System.Drawing.Size(776, 220)
        Me.grpInitialCost.TabIndex = 0
        Me.grpInitialCost.TabStop = False
        Me.grpInitialCost.Text = "初回金情報"
        '
        ' tblInitialCost
        '
        Me.tblInitialCost.ColumnCount = 4
        Me.tblInitialCost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0F))
        Me.tblInitialCost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
        Me.tblInitialCost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0F))
        Me.tblInitialCost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F))
        Me.tblInitialCost.RowCount = 3
        Me.tblInitialCost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0F))
        Me.tblInitialCost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0F))
        Me.tblInitialCost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0F))
        Me.tblInitialCost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblInitialCost.Location = New System.Drawing.Point(6, 24)
        Me.tblInitialCost.Name = "tblInitialCost"
        Me.tblInitialCost.Padding = New System.Windows.Forms.Padding(4)
        Me.tblInitialCost.Size = New System.Drawing.Size(764, 190)
        Me.tblInitialCost.TabIndex = 0
        '
        ' Initial Cost Labels
        '
        '
        Me.lblShikikin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblShikikin.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblShikikin.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblShikikin.Name = "lblShikikin"
        Me.lblShikikin.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblShikikin.Text = "敷金"
        Me.lblShikikin.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblHoshokin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblHoshokin.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblHoshokin.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblHoshokin.Name = "lblHoshokin"
        Me.lblHoshokin.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblHoshokin.Text = "保証金"
        Me.lblHoshokin.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblReikin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblReikin.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblReikin.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblReikin.Name = "lblReikin"
        Me.lblReikin.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblReikin.Text = "礼金"
        Me.lblReikin.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblBrokerFee.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblBrokerFee.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblBrokerFee.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblBrokerFee.Name = "lblBrokerFee"
        Me.lblBrokerFee.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblBrokerFee.Text = "仲介手数料"
        Me.lblBrokerFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        Me.lblPrepaidRent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPrepaidRent.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.lblPrepaidRent.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.lblPrepaidRent.Name = "lblPrepaidRent"
        Me.lblPrepaidRent.Padding = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.lblPrepaidRent.Text = "前払賃料"
        Me.lblPrepaidRent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        ' Initial Cost Input Controls
        '
        '
        Me.txtShikikin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtShikikin.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtShikikin.Name = "txtShikikin"
        Me.txtShikikin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtShikikin.TabIndex = 20
        '
        Me.txtHoshokin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHoshokin.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtHoshokin.Name = "txtHoshokin"
        Me.txtHoshokin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtHoshokin.TabIndex = 21
        '
        Me.txtReikin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtReikin.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtReikin.Name = "txtReikin"
        Me.txtReikin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtReikin.TabIndex = 22
        '
        Me.txtBrokerFee.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBrokerFee.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtBrokerFee.Name = "txtBrokerFee"
        Me.txtBrokerFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBrokerFee.TabIndex = 23
        '
        Me.txtPrepaidRent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPrepaidRent.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtPrepaidRent.Name = "txtPrepaidRent"
        Me.txtPrepaidRent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPrepaidRent.TabIndex = 24
        '
        ' tblInitialCost - Add controls to layout
        '
        Me.tblInitialCost.Controls.Add(Me.lblShikikin, 0, 0)
        Me.tblInitialCost.Controls.Add(Me.txtShikikin, 1, 0)
        Me.tblInitialCost.Controls.Add(Me.lblHoshokin, 2, 0)
        Me.tblInitialCost.Controls.Add(Me.txtHoshokin, 3, 0)
        Me.tblInitialCost.Controls.Add(Me.lblReikin, 0, 1)
        Me.tblInitialCost.Controls.Add(Me.txtReikin, 1, 1)
        Me.tblInitialCost.Controls.Add(Me.lblBrokerFee, 2, 1)
        Me.tblInitialCost.Controls.Add(Me.txtBrokerFee, 3, 1)
        Me.tblInitialCost.Controls.Add(Me.lblPrepaidRent, 0, 2)
        Me.tblInitialCost.Controls.Add(Me.txtPrepaidRent, 1, 2)
        '
        ' FrmAssetDetailEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 12.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 812)
        Me.Controls.Add(Me.tabControl)
        Me.Controls.Add(Me.pnlBottom)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmAssetDetailEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "資産詳細入力"
        Me.pnlBottom.ResumeLayout(False)
        Me.tabInitialCost.ResumeLayout(False)
        Me.tabAsset.ResumeLayout(False)
        Me.tabControl.ResumeLayout(False)
        CType(Me.dgvSelfEquipment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAllocations.ResumeLayout(False)
        CType(Me.dgvAllocations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpInitialCost.ResumeLayout(False)
        Me.tblInitialCost.ResumeLayout(False)
        Me.tblInitialCost.PerformLayout()
        Me.grpProperty.ResumeLayout(False)
        Me.tblProperty.ResumeLayout(False)
        Me.tblProperty.PerformLayout()
        CType(Me.numUsefulLife, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numQuantity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabAsset As System.Windows.Forms.TabPage
    Friend WithEvents tabInitialCost As System.Windows.Forms.TabPage
    Friend WithEvents grpProperty As System.Windows.Forms.GroupBox
    Friend WithEvents tblProperty As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblPropertyName As System.Windows.Forms.Label
    Friend WithEvents txtPropertyName As System.Windows.Forms.TextBox
    Friend WithEvents lblLocation As System.Windows.Forms.Label
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents lblSection As System.Windows.Forms.Label
    Friend WithEvents txtSection As System.Windows.Forms.TextBox
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents lblLayout As System.Windows.Forms.Label
    Friend WithEvents txtLayout As System.Windows.Forms.TextBox
    Friend WithEvents lblStructure As System.Windows.Forms.Label
    Friend WithEvents txtStructure As System.Windows.Forms.TextBox
    Friend WithEvents lblUsefulLife As System.Windows.Forms.Label
    Friend WithEvents numUsefulLife As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblCompletion As System.Windows.Forms.Label
    Friend WithEvents dtpCompletion As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblBuildingAgeCaption As System.Windows.Forms.Label
    Friend WithEvents lblBuildingAge As System.Windows.Forms.Label
    Friend WithEvents lblLandlordName As System.Windows.Forms.Label
    Friend WithEvents txtLandlordName As System.Windows.Forms.TextBox
    Friend WithEvents lblBrokerCompany As System.Windows.Forms.Label
    Friend WithEvents txtBrokerCompany As System.Windows.Forms.TextBox
    Friend WithEvents lblUsageRestrictions As System.Windows.Forms.Label
    Friend WithEvents txtUsageRestrictions As System.Windows.Forms.TextBox
    Friend WithEvents lblPaymentAgent As System.Windows.Forms.Label
    Friend WithEvents txtPaymentAgent As System.Windows.Forms.TextBox
    Friend WithEvents lblGuarantor As System.Windows.Forms.Label
    Friend WithEvents txtGuarantor As System.Windows.Forms.TextBox
    Friend WithEvents lblSelfEquipment As System.Windows.Forms.Label
    Friend WithEvents dgvSelfEquipment As System.Windows.Forms.DataGridView
    Friend WithEvents colSelfEquipName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSelfEquipDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSelfEquipAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents grpAllocations As System.Windows.Forms.GroupBox
    Friend WithEvents dgvAllocations As System.Windows.Forms.DataGridView
    Friend WithEvents colAllocId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDeptNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCostDeptName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colAllocRate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colValidStartDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colValidEndDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colJournalPatternId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblAccountClass As System.Windows.Forms.Label
    Friend WithEvents cmbAccountClass As System.Windows.Forms.ComboBox
    Friend WithEvents lblAssetNo As System.Windows.Forms.Label
    Friend WithEvents txtAssetNo As System.Windows.Forms.TextBox
    Friend WithEvents lblQuantity As System.Windows.Forms.Label
    Friend WithEvents numQuantity As System.Windows.Forms.NumericUpDown
    Friend WithEvents tabMonthlyDetail As System.Windows.Forms.TabPage
    Friend WithEvents grpInitialCost As System.Windows.Forms.GroupBox
    Friend WithEvents tblInitialCost As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblShikikin As System.Windows.Forms.Label
    Friend WithEvents txtShikikin As System.Windows.Forms.TextBox
    Friend WithEvents lblHoshokin As System.Windows.Forms.Label
    Friend WithEvents txtHoshokin As System.Windows.Forms.TextBox
    Friend WithEvents lblReikin As System.Windows.Forms.Label
    Friend WithEvents txtReikin As System.Windows.Forms.TextBox
    Friend WithEvents lblBrokerFee As System.Windows.Forms.Label
    Friend WithEvents txtBrokerFee As System.Windows.Forms.TextBox
    Friend WithEvents lblPrepaidRent As System.Windows.Forms.Label
    Friend WithEvents txtPrepaidRent As System.Windows.Forms.TextBox

End Class
