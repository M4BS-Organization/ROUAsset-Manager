<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAssetDetailEntry
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
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.grpEquipment = New System.Windows.Forms.GroupBox()
        Me.dgvEquipment = New System.Windows.Forms.DataGridView()
        Me.colEquipName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEquipAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEquipDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.pnlBottom.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        Me.grpEquipment.SuspendLayout()
        CType(Me.dgvEquipment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpProperty.SuspendLayout()
        Me.tblProperty.SuspendLayout()
        CType(Me.numUsefulLife, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' pnlBottom
        '
        Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(240, Integer), CType(240, Integer))
        Me.pnlBottom.Controls.Add(Me.btnCancel)
        Me.pnlBottom.Controls.Add(Me.btnSave)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 490)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(780, 50)
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
        Me.btnSave.Location = New System.Drawing.Point(578, 12)
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
        Me.btnCancel.Location = New System.Drawing.Point(674, 12)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        ' pnlMain
        '
        Me.pnlMain.AutoScroll = True
        Me.pnlMain.BackColor = System.Drawing.Color.FromArgb(CType(248, Integer), CType(249, Integer), CType(250, Integer))
        Me.pnlMain.Controls.Add(Me.grpEquipment)
        Me.pnlMain.Controls.Add(Me.grpProperty)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Padding = New System.Windows.Forms.Padding(8, 8, 8, 0)
        Me.pnlMain.Size = New System.Drawing.Size(780, 490)
        Me.pnlMain.TabIndex = 1
        '
        ' grpProperty
        '
        Me.grpProperty.BackColor = System.Drawing.Color.White
        Me.grpProperty.Controls.Add(Me.tblProperty)
        Me.grpProperty.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpProperty.Font = New System.Drawing.Font("Meiryo", 10.0F, System.Drawing.FontStyle.Bold)
        Me.grpProperty.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.grpProperty.Location = New System.Drawing.Point(8, 8)
        Me.grpProperty.Margin = New System.Windows.Forms.Padding(0, 0, 0, 8)
        Me.grpProperty.Name = "grpProperty"
        Me.grpProperty.Padding = New System.Windows.Forms.Padding(6, 12, 6, 6)
        Me.grpProperty.Size = New System.Drawing.Size(764, 260)
        Me.grpProperty.TabIndex = 0
        Me.grpProperty.TabStop = False
        Me.grpProperty.Text = "物件情報"
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
        Me.tblProperty.RowCount = 6
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0F))
        Me.tblProperty.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblProperty.Location = New System.Drawing.Point(6, 24)
        Me.tblProperty.Name = "tblProperty"
        Me.tblProperty.Padding = New System.Windows.Forms.Padding(4)
        Me.tblProperty.Size = New System.Drawing.Size(752, 230)
        Me.tblProperty.TabIndex = 0
        '
        ' Labels
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
        ' Input Controls
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
        Me.txtUsageRestrictions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUsageRestrictions.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.txtUsageRestrictions.Multiline = True
        Me.txtUsageRestrictions.Name = "txtUsageRestrictions"
        Me.txtUsageRestrictions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtUsageRestrictions.TabIndex = 11
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
        ' tblProperty - Add controls to layout
        '
        ' Row 0: 物件名 (spans full width)
        Me.tblProperty.Controls.Add(Me.lblPropertyName, 0, 0)
        Me.tblProperty.Controls.Add(Me.txtPropertyName, 1, 0)
        Me.tblProperty.SetColumnSpan(Me.txtPropertyName, 5)
        ' Row 1: 所在地 + 区画
        Me.tblProperty.Controls.Add(Me.lblLocation, 0, 1)
        Me.tblProperty.Controls.Add(Me.txtLocation, 1, 1)
        Me.tblProperty.SetColumnSpan(Me.txtLocation, 3)
        Me.tblProperty.Controls.Add(Me.lblSection, 4, 1)
        Me.tblProperty.Controls.Add(Me.txtSection, 5, 1)
        ' Row 2: 面積 + 間取り + 構造・用途
        Me.tblProperty.Controls.Add(Me.lblArea, 0, 2)
        Me.tblProperty.Controls.Add(Me.txtArea, 1, 2)
        Me.tblProperty.Controls.Add(Me.lblLayout, 2, 2)
        Me.tblProperty.Controls.Add(Me.txtLayout, 3, 2)
        Me.tblProperty.Controls.Add(Me.lblStructure, 4, 2)
        Me.tblProperty.Controls.Add(Me.txtStructure, 5, 2)
        ' Row 3: 耐用年数 + 竣工 + 築年数
        Me.tblProperty.Controls.Add(Me.lblUsefulLife, 0, 3)
        Me.tblProperty.Controls.Add(Me.numUsefulLife, 1, 3)
        Me.tblProperty.Controls.Add(Me.lblCompletion, 2, 3)
        Me.tblProperty.Controls.Add(Me.dtpCompletion, 3, 3)
        Me.tblProperty.Controls.Add(Me.lblBuildingAgeCaption, 4, 3)
        Me.tblProperty.Controls.Add(Me.lblBuildingAge, 5, 3)
        ' Row 4: 貸主名 + 仲介会社 + 用途・制限 (rowspan=2)
        Me.tblProperty.Controls.Add(Me.lblLandlordName, 0, 4)
        Me.tblProperty.Controls.Add(Me.txtLandlordName, 1, 4)
        Me.tblProperty.Controls.Add(Me.lblBrokerCompany, 2, 4)
        Me.tblProperty.Controls.Add(Me.txtBrokerCompany, 3, 4)
        Me.tblProperty.Controls.Add(Me.lblUsageRestrictions, 4, 4)
        Me.tblProperty.Controls.Add(Me.txtUsageRestrictions, 5, 4)
        Me.tblProperty.SetRowSpan(Me.txtUsageRestrictions, 2)
        ' Row 5: 決済代行 + 連帯保証人
        Me.tblProperty.Controls.Add(Me.lblPaymentAgent, 0, 5)
        Me.tblProperty.Controls.Add(Me.txtPaymentAgent, 1, 5)
        Me.tblProperty.Controls.Add(Me.lblGuarantor, 2, 5)
        Me.tblProperty.Controls.Add(Me.txtGuarantor, 3, 5)
        '
        ' grpEquipment
        '
        Me.grpEquipment.BackColor = System.Drawing.Color.White
        Me.grpEquipment.Controls.Add(Me.dgvEquipment)
        Me.grpEquipment.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpEquipment.Font = New System.Drawing.Font("Meiryo", 10.0F, System.Drawing.FontStyle.Bold)
        Me.grpEquipment.ForeColor = System.Drawing.Color.FromArgb(CType(0, Integer), CType(51, Integer), CType(102, Integer))
        Me.grpEquipment.Location = New System.Drawing.Point(8, 268)
        Me.grpEquipment.Name = "grpEquipment"
        Me.grpEquipment.Padding = New System.Windows.Forms.Padding(6, 12, 6, 6)
        Me.grpEquipment.Size = New System.Drawing.Size(764, 200)
        Me.grpEquipment.TabIndex = 1
        Me.grpEquipment.TabStop = False
        Me.grpEquipment.Text = "付属設備"
        '
        ' dgvEquipment
        '
        Me.dgvEquipment.AllowUserToAddRows = True
        Me.dgvEquipment.BackgroundColor = System.Drawing.Color.White
        Me.dgvEquipment.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvEquipment.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(CType(240, Integer), CType(244, Integer), CType(248, Integer))
        Me.dgvEquipment.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.0F, System.Drawing.FontStyle.Bold)
        Me.dgvEquipment.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(73, Integer), CType(80, Integer), CType(87, Integer))
        Me.dgvEquipment.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.dgvEquipment.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEquipName, Me.colEquipAmount, Me.colEquipDate})
        Me.dgvEquipment.DefaultCellStyle.Font = New System.Drawing.Font("Meiryo", 9.75F)
        Me.dgvEquipment.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(CType(33, Integer), CType(37, Integer), CType(41, Integer))
        Me.dgvEquipment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEquipment.EnableHeadersVisualStyles = False
        Me.dgvEquipment.GridColor = System.Drawing.Color.FromArgb(CType(222, Integer), CType(226, Integer), CType(230, Integer))
        Me.dgvEquipment.Location = New System.Drawing.Point(6, 24)
        Me.dgvEquipment.Name = "dgvEquipment"
        Me.dgvEquipment.RowHeadersVisible = False
        Me.dgvEquipment.Size = New System.Drawing.Size(752, 170)
        Me.dgvEquipment.TabIndex = 14
        Me.dgvEquipment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        '
        ' colEquipName
        '
        Me.colEquipName.HeaderText = "設備名"
        Me.colEquipName.Name = "EquipName"
        '
        ' colEquipAmount
        '
        Me.colEquipAmount.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colEquipAmount.DefaultCellStyle.Format = "N0"
        Me.colEquipAmount.HeaderText = "金額"
        Me.colEquipAmount.Name = "EquipAmount"
        '
        ' colEquipDate
        '
        Me.colEquipDate.HeaderText = "日付"
        Me.colEquipDate.Name = "EquipDate"
        '
        ' FrmAssetDetailEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 12.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(780, 540)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.pnlBottom)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmAssetDetailEntry"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "資産詳細入力"
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlMain.ResumeLayout(False)
        Me.grpEquipment.ResumeLayout(False)
        CType(Me.dgvEquipment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpProperty.ResumeLayout(False)
        Me.tblProperty.ResumeLayout(False)
        Me.tblProperty.PerformLayout()
        CType(Me.numUsefulLife, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
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
    Friend WithEvents grpEquipment As System.Windows.Forms.GroupBox
    Friend WithEvents dgvEquipment As System.Windows.Forms.DataGridView
    Friend WithEvents colEquipName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEquipAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEquipDate As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
