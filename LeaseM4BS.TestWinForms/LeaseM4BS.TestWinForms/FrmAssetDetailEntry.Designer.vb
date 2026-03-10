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
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.scrollPanel = New System.Windows.Forms.Panel()
        Me.grpBasicInfo = New System.Windows.Forms.GroupBox()
        Me.tblBasicInfo = New System.Windows.Forms.TableLayoutPanel()
        Me.lblAssetNo = New System.Windows.Forms.Label()
        Me.txtAssetNo = New System.Windows.Forms.TextBox()
        Me.lblAssetCategory = New System.Windows.Forms.Label()
        Me.lblAssetCategoryDisplay = New System.Windows.Forms.Label()
        Me.lblAssetName = New System.Windows.Forms.Label()
        Me.txtAssetName = New System.Windows.Forms.TextBox()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.lblInstallLocation = New System.Windows.Forms.Label()
        Me.txtInstallLocation = New System.Windows.Forms.TextBox()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.grpDeptAllocation = New System.Windows.Forms.GroupBox()
        Me.pnlDeptButtons = New System.Windows.Forms.Panel()
        Me.btnAddDept = New System.Windows.Forms.Button()
        Me.btnRemoveDept = New System.Windows.Forms.Button()
        Me.lblAllocationTotal = New System.Windows.Forms.Label()
        Me.dgvDeptAllocation = New System.Windows.Forms.DataGridView()
        Me.grpCategorySpecific = New System.Windows.Forms.GroupBox()
        Me.pnlRealEstate = New System.Windows.Forms.Panel()
        Me.tblRealEstate = New System.Windows.Forms.TableLayoutPanel()
        Me.lblStructure = New System.Windows.Forms.Label()
        Me.txtStructure = New System.Windows.Forms.TextBox()
        Me.lblArea = New System.Windows.Forms.Label()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.lblLayout = New System.Windows.Forms.Label()
        Me.txtLayout = New System.Windows.Forms.TextBox()
        Me.lblCompletion = New System.Windows.Forms.Label()
        Me.dtpCompletion = New System.Windows.Forms.DateTimePicker()
        Me.lblLandlordName = New System.Windows.Forms.Label()
        Me.txtLandlordName = New System.Windows.Forms.TextBox()
        Me.lblBrokerCompany = New System.Windows.Forms.Label()
        Me.txtBrokerCompany = New System.Windows.Forms.TextBox()
        Me.lblUsageRestrictions = New System.Windows.Forms.Label()
        Me.txtUsageRestrictions = New System.Windows.Forms.TextBox()
        Me.pnlVehicle = New System.Windows.Forms.Panel()
        Me.tblVehicle = New System.Windows.Forms.TableLayoutPanel()
        Me.lblChassisNo = New System.Windows.Forms.Label()
        Me.txtChassisNo = New System.Windows.Forms.TextBox()
        Me.lblRegistrationNo = New System.Windows.Forms.Label()
        Me.txtRegistrationNo = New System.Windows.Forms.TextBox()
        Me.lblVehicleType = New System.Windows.Forms.Label()
        Me.txtVehicleType = New System.Windows.Forms.TextBox()
        Me.lblInspectionDate = New System.Windows.Forms.Label()
        Me.dtpInspectionDate = New System.Windows.Forms.DateTimePicker()
        Me.lblMileageLimit = New System.Windows.Forms.Label()
        Me.txtMileageLimit = New System.Windows.Forms.TextBox()
        Me.pnlOfficeEquip = New System.Windows.Forms.Panel()
        Me.tblOfficeEquip = New System.Windows.Forms.TableLayoutPanel()
        Me.lblModelNo = New System.Windows.Forms.Label()
        Me.txtModelNo = New System.Windows.Forms.TextBox()
        Me.lblSerialNo = New System.Windows.Forms.Label()
        Me.txtSerialNo = New System.Windows.Forms.TextBox()
        Me.lblMaintenanceDate = New System.Windows.Forms.Label()
        Me.dtpMaintenanceDate = New System.Windows.Forms.DateTimePicker()
        Me.lblMaintenanceContract = New System.Windows.Forms.Label()
        Me.txtMaintenanceContract = New System.Windows.Forms.TextBox()

        Dim CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
        Dim CLR_CARD As Color = Color.White
        Dim CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
        Dim CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
        Dim CLR_BORDER As Color = Color.FromArgb(222, 226, 230)
        Dim CLR_ACCENT As Color = Color.FromArgb(40, 167, 69)
        Dim CLR_READONLY As Color = Color.FromArgb(233, 236, 239)
        Dim FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)
        Dim FNT_INPUT As New Font("Meiryo", 9.75F, FontStyle.Regular)
        Dim FNT_SECTION As New Font("Meiryo", 10.0F, FontStyle.Bold)

        Me.SuspendLayout()
        Me.pnlBottom.SuspendLayout()
        Me.scrollPanel.SuspendLayout()
        Me.grpBasicInfo.SuspendLayout()
        Me.tblBasicInfo.SuspendLayout()
        Me.grpDeptAllocation.SuspendLayout()
        Me.pnlDeptButtons.SuspendLayout()
        CType(Me.dgvDeptAllocation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCategorySpecific.SuspendLayout()
        Me.pnlRealEstate.SuspendLayout()
        Me.tblRealEstate.SuspendLayout()
        Me.pnlVehicle.SuspendLayout()
        Me.tblVehicle.SuspendLayout()
        Me.pnlOfficeEquip.SuspendLayout()
        Me.tblOfficeEquip.SuspendLayout()

        ' === pnlBottom ===
        Me.pnlBottom.Dock = DockStyle.Bottom
        Me.pnlBottom.Height = 50
        Me.pnlBottom.BackColor = CLR_CARD
        Me.pnlBottom.Padding = New Padding(0, 8, 12, 8)
        Me.pnlBottom.Controls.Add(Me.btnCancel)
        Me.pnlBottom.Controls.Add(Me.btnAdd)

        Me.btnAdd.Text = "追加"
        Me.btnAdd.Dock = DockStyle.Right
        Me.btnAdd.Width = 100
        Me.btnAdd.Height = 34
        Me.btnAdd.BackColor = CLR_ACCENT
        Me.btnAdd.ForeColor = Color.White
        Me.btnAdd.FlatStyle = FlatStyle.Flat
        Me.btnAdd.FlatAppearance.BorderSize = 0
        Me.btnAdd.Font = New Font("Meiryo", 9.75F, FontStyle.Bold)
        Me.btnAdd.Margin = New Padding(0, 0, 8, 0)
        Me.btnAdd.Cursor = Cursors.Hand

        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.Dock = DockStyle.Right
        Me.btnCancel.Width = 100
        Me.btnCancel.Height = 34
        Me.btnCancel.BackColor = Color.FromArgb(108, 117, 125)
        Me.btnCancel.ForeColor = Color.White
        Me.btnCancel.FlatStyle = FlatStyle.Flat
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.Font = New Font("Meiryo", 9.75F, FontStyle.Bold)
        Me.btnCancel.Cursor = Cursors.Hand

        ' === scrollPanel ===
        Me.scrollPanel.Dock = DockStyle.Fill
        Me.scrollPanel.AutoScroll = True
        Me.scrollPanel.Padding = New Padding(8)
        Me.scrollPanel.BackColor = CLR_CARD
        Me.scrollPanel.Controls.Add(Me.grpCategorySpecific)
        Me.scrollPanel.Controls.Add(Me.grpDeptAllocation)
        Me.scrollPanel.Controls.Add(Me.grpBasicInfo)

        ' === grpBasicInfo ===
        Me.grpBasicInfo.Text = "基本情報"
        Me.grpBasicInfo.Dock = DockStyle.Top
        Me.grpBasicInfo.AutoSize = True
        Me.grpBasicInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.grpBasicInfo.BackColor = CLR_CARD
        Me.grpBasicInfo.ForeColor = CLR_HEADER
        Me.grpBasicInfo.Font = FNT_SECTION
        Me.grpBasicInfo.Padding = New Padding(6, 12, 6, 6)
        Me.grpBasicInfo.Controls.Add(Me.tblBasicInfo)

        Me.tblBasicInfo.Dock = DockStyle.Top
        Me.tblBasicInfo.AutoSize = True
        Me.tblBasicInfo.ColumnCount = 4
        Me.tblBasicInfo.Padding = New Padding(8)
        Me.tblBasicInfo.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblBasicInfo.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Me.tblBasicInfo.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblBasicInfo.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        ' Row 0: 資産番号 | 資産種類
        Me.tblBasicInfo.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblAssetNo.Text = "資産番号"
        Me.lblAssetNo.Dock = DockStyle.Fill
        Me.lblAssetNo.Font = FNT_LABEL
        Me.lblAssetNo.ForeColor = CLR_LABEL
        Me.lblAssetNo.TextAlign = ContentAlignment.MiddleRight
        Me.lblAssetNo.Padding = New Padding(0, 0, 4, 0)
        Me.txtAssetNo.Dock = DockStyle.Fill
        Me.txtAssetNo.Font = FNT_INPUT
        Me.txtAssetNo.ForeColor = CLR_TEXT
        Me.txtAssetNo.ReadOnly = True
        Me.txtAssetNo.BackColor = CLR_READONLY
        Me.lblAssetCategory.Text = "資産種類"
        Me.lblAssetCategory.Dock = DockStyle.Fill
        Me.lblAssetCategory.Font = FNT_LABEL
        Me.lblAssetCategory.ForeColor = CLR_LABEL
        Me.lblAssetCategory.TextAlign = ContentAlignment.MiddleRight
        Me.lblAssetCategory.Padding = New Padding(0, 0, 4, 0)
        Me.lblAssetCategoryDisplay.Dock = DockStyle.Fill
        Me.lblAssetCategoryDisplay.Font = FNT_INPUT
        Me.lblAssetCategoryDisplay.ForeColor = CLR_TEXT
        Me.lblAssetCategoryDisplay.TextAlign = ContentAlignment.MiddleLeft
        Me.lblAssetCategoryDisplay.BackColor = CLR_READONLY
        Me.lblAssetCategoryDisplay.Padding = New Padding(4, 0, 0, 0)
        Me.tblBasicInfo.Controls.Add(Me.lblAssetNo, 0, 0)
        Me.tblBasicInfo.Controls.Add(Me.txtAssetNo, 1, 0)
        Me.tblBasicInfo.Controls.Add(Me.lblAssetCategory, 2, 0)
        Me.tblBasicInfo.Controls.Add(Me.lblAssetCategoryDisplay, 3, 0)

        ' Row 1: 資産名 (spans 3)
        Me.tblBasicInfo.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblAssetName.Text = "資産名"
        Me.lblAssetName.Dock = DockStyle.Fill
        Me.lblAssetName.Font = FNT_LABEL
        Me.lblAssetName.ForeColor = CLR_LABEL
        Me.lblAssetName.TextAlign = ContentAlignment.MiddleRight
        Me.lblAssetName.Padding = New Padding(0, 0, 4, 0)
        Me.txtAssetName.Dock = DockStyle.Fill
        Me.txtAssetName.Font = FNT_INPUT
        Me.txtAssetName.ForeColor = CLR_TEXT
        Me.tblBasicInfo.Controls.Add(Me.lblAssetName, 0, 1)
        Me.tblBasicInfo.Controls.Add(Me.txtAssetName, 1, 1)
        Me.tblBasicInfo.SetColumnSpan(Me.txtAssetName, 3)

        ' Row 2: 管理会社 | 設置場所
        Me.tblBasicInfo.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblCompany.Text = "管理会社"
        Me.lblCompany.Dock = DockStyle.Fill
        Me.lblCompany.Font = FNT_LABEL
        Me.lblCompany.ForeColor = CLR_LABEL
        Me.lblCompany.TextAlign = ContentAlignment.MiddleRight
        Me.lblCompany.Padding = New Padding(0, 0, 4, 0)
        Me.cmbCompany.Dock = DockStyle.Fill
        Me.cmbCompany.Font = FNT_INPUT
        Me.cmbCompany.ForeColor = CLR_TEXT
        Me.cmbCompany.DropDownStyle = ComboBoxStyle.DropDownList
        Me.lblInstallLocation.Text = "設置場所"
        Me.lblInstallLocation.Dock = DockStyle.Fill
        Me.lblInstallLocation.Font = FNT_LABEL
        Me.lblInstallLocation.ForeColor = CLR_LABEL
        Me.lblInstallLocation.TextAlign = ContentAlignment.MiddleRight
        Me.lblInstallLocation.Padding = New Padding(0, 0, 4, 0)
        Me.txtInstallLocation.Dock = DockStyle.Fill
        Me.txtInstallLocation.Font = FNT_INPUT
        Me.txtInstallLocation.ForeColor = CLR_TEXT
        Me.tblBasicInfo.Controls.Add(Me.lblCompany, 0, 2)
        Me.tblBasicInfo.Controls.Add(Me.cmbCompany, 1, 2)
        Me.tblBasicInfo.Controls.Add(Me.lblInstallLocation, 2, 2)
        Me.tblBasicInfo.Controls.Add(Me.txtInstallLocation, 3, 2)

        ' Row 3: 備考 (spans 3)
        Me.tblBasicInfo.RowStyles.Add(New RowStyle(SizeType.Absolute, 50.0F))
        Me.lblRemarks.Text = "備考"
        Me.lblRemarks.Dock = DockStyle.Fill
        Me.lblRemarks.Font = FNT_LABEL
        Me.lblRemarks.ForeColor = CLR_LABEL
        Me.lblRemarks.TextAlign = ContentAlignment.MiddleRight
        Me.lblRemarks.Padding = New Padding(0, 0, 4, 0)
        Me.txtRemarks.Dock = DockStyle.Fill
        Me.txtRemarks.Font = FNT_INPUT
        Me.txtRemarks.ForeColor = CLR_TEXT
        Me.txtRemarks.Multiline = True
        Me.tblBasicInfo.Controls.Add(Me.lblRemarks, 0, 3)
        Me.tblBasicInfo.Controls.Add(Me.txtRemarks, 1, 3)
        Me.tblBasicInfo.SetColumnSpan(Me.txtRemarks, 3)
        Me.tblBasicInfo.RowCount = 4

        ' === grpDeptAllocation ===
        Me.grpDeptAllocation.Text = "部門配賦"
        Me.grpDeptAllocation.Dock = DockStyle.Top
        Me.grpDeptAllocation.Height = 220
        Me.grpDeptAllocation.BackColor = CLR_CARD
        Me.grpDeptAllocation.ForeColor = CLR_HEADER
        Me.grpDeptAllocation.Font = FNT_SECTION
        Me.grpDeptAllocation.Padding = New Padding(6, 12, 6, 6)
        Me.grpDeptAllocation.Controls.Add(Me.dgvDeptAllocation)
        Me.grpDeptAllocation.Controls.Add(Me.pnlDeptButtons)

        ' === pnlDeptButtons ===
        Me.pnlDeptButtons.Dock = DockStyle.Top
        Me.pnlDeptButtons.Height = 36
        Me.pnlDeptButtons.BackColor = CLR_CARD
        Me.pnlDeptButtons.Padding = New Padding(8, 4, 8, 4)
        Me.pnlDeptButtons.Controls.Add(Me.lblAllocationTotal)
        Me.pnlDeptButtons.Controls.Add(Me.btnRemoveDept)
        Me.pnlDeptButtons.Controls.Add(Me.btnAddDept)

        Me.btnAddDept.Text = "行追加"
        Me.btnAddDept.Dock = DockStyle.Left
        Me.btnAddDept.Width = 80
        Me.btnAddDept.Height = 28
        Me.btnAddDept.BackColor = CLR_ACCENT
        Me.btnAddDept.ForeColor = Color.White
        Me.btnAddDept.FlatStyle = FlatStyle.Flat
        Me.btnAddDept.FlatAppearance.BorderSize = 0
        Me.btnAddDept.Font = New Font("Meiryo", 9.0F, FontStyle.Bold)
        Me.btnAddDept.Cursor = Cursors.Hand
        Me.btnAddDept.Margin = New Padding(0, 0, 4, 0)

        Me.btnRemoveDept.Text = "行削除"
        Me.btnRemoveDept.Dock = DockStyle.Left
        Me.btnRemoveDept.Width = 80
        Me.btnRemoveDept.Height = 28
        Me.btnRemoveDept.BackColor = Color.FromArgb(220, 53, 69)
        Me.btnRemoveDept.ForeColor = Color.White
        Me.btnRemoveDept.FlatStyle = FlatStyle.Flat
        Me.btnRemoveDept.FlatAppearance.BorderSize = 0
        Me.btnRemoveDept.Font = New Font("Meiryo", 9.0F, FontStyle.Bold)
        Me.btnRemoveDept.Cursor = Cursors.Hand

        Me.lblAllocationTotal.Text = "配賦率合計: 0.00%"
        Me.lblAllocationTotal.Dock = DockStyle.Right
        Me.lblAllocationTotal.Width = 200
        Me.lblAllocationTotal.Font = FNT_LABEL
        Me.lblAllocationTotal.ForeColor = CLR_LABEL
        Me.lblAllocationTotal.TextAlign = ContentAlignment.MiddleRight

        ' === dgvDeptAllocation ===
        Me.dgvDeptAllocation.Dock = DockStyle.Fill
        Me.dgvDeptAllocation.BackgroundColor = CLR_CARD
        Me.dgvDeptAllocation.BorderStyle = BorderStyle.FixedSingle
        Me.dgvDeptAllocation.GridColor = CLR_BORDER
        Me.dgvDeptAllocation.Font = FNT_INPUT
        Me.dgvDeptAllocation.RowHeadersVisible = False
        Me.dgvDeptAllocation.AllowUserToAddRows = False
        Me.dgvDeptAllocation.AllowUserToDeleteRows = False
        Me.dgvDeptAllocation.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.dgvDeptAllocation.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(240, 244, 248),
            .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }
        Me.dgvDeptAllocation.EnableHeadersVisualStyles = False
        Me.dgvDeptAllocation.RowTemplate.Height = 28

        ' === grpCategorySpecific ===
        Me.grpCategorySpecific.Text = "種別固有情報"
        Me.grpCategorySpecific.Dock = DockStyle.Top
        Me.grpCategorySpecific.AutoSize = True
        Me.grpCategorySpecific.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.grpCategorySpecific.BackColor = CLR_CARD
        Me.grpCategorySpecific.ForeColor = CLR_HEADER
        Me.grpCategorySpecific.Font = FNT_SECTION
        Me.grpCategorySpecific.Padding = New Padding(6, 12, 6, 6)
        Me.grpCategorySpecific.Controls.Add(Me.pnlOfficeEquip)
        Me.grpCategorySpecific.Controls.Add(Me.pnlVehicle)
        Me.grpCategorySpecific.Controls.Add(Me.pnlRealEstate)

        ' === pnlRealEstate ===
        Me.pnlRealEstate.Dock = DockStyle.Top
        Me.pnlRealEstate.AutoSize = True
        Me.pnlRealEstate.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.pnlRealEstate.Visible = False
        Me.pnlRealEstate.Controls.Add(Me.tblRealEstate)

        Me.tblRealEstate.Dock = DockStyle.Top
        Me.tblRealEstate.AutoSize = True
        Me.tblRealEstate.ColumnCount = 4
        Me.tblRealEstate.Padding = New Padding(8)
        Me.tblRealEstate.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblRealEstate.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Me.tblRealEstate.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblRealEstate.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Me.tblRealEstate.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblStructure.Text = "構造"
        Me.lblStructure.Dock = DockStyle.Fill
        Me.lblStructure.Font = FNT_LABEL
        Me.lblStructure.ForeColor = CLR_LABEL
        Me.lblStructure.TextAlign = ContentAlignment.MiddleRight
        Me.lblStructure.Padding = New Padding(0, 0, 4, 0)
        Me.txtStructure.Dock = DockStyle.Fill
        Me.txtStructure.Font = FNT_INPUT
        Me.txtStructure.ForeColor = CLR_TEXT
        Me.lblArea.Text = "面積"
        Me.lblArea.Dock = DockStyle.Fill
        Me.lblArea.Font = FNT_LABEL
        Me.lblArea.ForeColor = CLR_LABEL
        Me.lblArea.TextAlign = ContentAlignment.MiddleRight
        Me.lblArea.Padding = New Padding(0, 0, 4, 0)
        Me.txtArea.Dock = DockStyle.Fill
        Me.txtArea.Font = FNT_INPUT
        Me.txtArea.ForeColor = CLR_TEXT
        Me.tblRealEstate.Controls.Add(Me.lblStructure, 0, 0)
        Me.tblRealEstate.Controls.Add(Me.txtStructure, 1, 0)
        Me.tblRealEstate.Controls.Add(Me.lblArea, 2, 0)
        Me.tblRealEstate.Controls.Add(Me.txtArea, 3, 0)

        Me.tblRealEstate.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblLayout.Text = "間取り"
        Me.lblLayout.Dock = DockStyle.Fill
        Me.lblLayout.Font = FNT_LABEL
        Me.lblLayout.ForeColor = CLR_LABEL
        Me.lblLayout.TextAlign = ContentAlignment.MiddleRight
        Me.lblLayout.Padding = New Padding(0, 0, 4, 0)
        Me.txtLayout.Dock = DockStyle.Fill
        Me.txtLayout.Font = FNT_INPUT
        Me.txtLayout.ForeColor = CLR_TEXT
        Me.lblCompletion.Text = "築年月"
        Me.lblCompletion.Dock = DockStyle.Fill
        Me.lblCompletion.Font = FNT_LABEL
        Me.lblCompletion.ForeColor = CLR_LABEL
        Me.lblCompletion.TextAlign = ContentAlignment.MiddleRight
        Me.lblCompletion.Padding = New Padding(0, 0, 4, 0)
        Me.dtpCompletion.Dock = DockStyle.Fill
        Me.dtpCompletion.Font = FNT_INPUT
        Me.dtpCompletion.Format = DateTimePickerFormat.Short
        Me.dtpCompletion.ShowCheckBox = True
        Me.dtpCompletion.Checked = False
        Me.tblRealEstate.Controls.Add(Me.lblLayout, 0, 1)
        Me.tblRealEstate.Controls.Add(Me.txtLayout, 1, 1)
        Me.tblRealEstate.Controls.Add(Me.lblCompletion, 2, 1)
        Me.tblRealEstate.Controls.Add(Me.dtpCompletion, 3, 1)

        Me.tblRealEstate.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblLandlordName.Text = "貸主名"
        Me.lblLandlordName.Dock = DockStyle.Fill
        Me.lblLandlordName.Font = FNT_LABEL
        Me.lblLandlordName.ForeColor = CLR_LABEL
        Me.lblLandlordName.TextAlign = ContentAlignment.MiddleRight
        Me.lblLandlordName.Padding = New Padding(0, 0, 4, 0)
        Me.txtLandlordName.Dock = DockStyle.Fill
        Me.txtLandlordName.Font = FNT_INPUT
        Me.txtLandlordName.ForeColor = CLR_TEXT
        Me.lblBrokerCompany.Text = "仲介業者"
        Me.lblBrokerCompany.Dock = DockStyle.Fill
        Me.lblBrokerCompany.Font = FNT_LABEL
        Me.lblBrokerCompany.ForeColor = CLR_LABEL
        Me.lblBrokerCompany.TextAlign = ContentAlignment.MiddleRight
        Me.lblBrokerCompany.Padding = New Padding(0, 0, 4, 0)
        Me.txtBrokerCompany.Dock = DockStyle.Fill
        Me.txtBrokerCompany.Font = FNT_INPUT
        Me.txtBrokerCompany.ForeColor = CLR_TEXT
        Me.tblRealEstate.Controls.Add(Me.lblLandlordName, 0, 2)
        Me.tblRealEstate.Controls.Add(Me.txtLandlordName, 1, 2)
        Me.tblRealEstate.Controls.Add(Me.lblBrokerCompany, 2, 2)
        Me.tblRealEstate.Controls.Add(Me.txtBrokerCompany, 3, 2)

        Me.tblRealEstate.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblUsageRestrictions.Text = "用途制限"
        Me.lblUsageRestrictions.Dock = DockStyle.Fill
        Me.lblUsageRestrictions.Font = FNT_LABEL
        Me.lblUsageRestrictions.ForeColor = CLR_LABEL
        Me.lblUsageRestrictions.TextAlign = ContentAlignment.MiddleRight
        Me.lblUsageRestrictions.Padding = New Padding(0, 0, 4, 0)
        Me.txtUsageRestrictions.Dock = DockStyle.Fill
        Me.txtUsageRestrictions.Font = FNT_INPUT
        Me.txtUsageRestrictions.ForeColor = CLR_TEXT
        Me.tblRealEstate.Controls.Add(Me.lblUsageRestrictions, 0, 3)
        Me.tblRealEstate.Controls.Add(Me.txtUsageRestrictions, 1, 3)
        Me.tblRealEstate.SetColumnSpan(Me.txtUsageRestrictions, 3)
        Me.tblRealEstate.RowCount = 4

        ' === pnlVehicle ===
        Me.pnlVehicle.Dock = DockStyle.Top
        Me.pnlVehicle.AutoSize = True
        Me.pnlVehicle.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.pnlVehicle.Visible = False
        Me.pnlVehicle.Controls.Add(Me.tblVehicle)

        Me.tblVehicle.Dock = DockStyle.Top
        Me.tblVehicle.AutoSize = True
        Me.tblVehicle.ColumnCount = 4
        Me.tblVehicle.Padding = New Padding(8)
        Me.tblVehicle.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblVehicle.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Me.tblVehicle.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblVehicle.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Me.tblVehicle.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblChassisNo.Text = "車台番号"
        Me.lblChassisNo.Dock = DockStyle.Fill
        Me.lblChassisNo.Font = FNT_LABEL
        Me.lblChassisNo.ForeColor = CLR_LABEL
        Me.lblChassisNo.TextAlign = ContentAlignment.MiddleRight
        Me.lblChassisNo.Padding = New Padding(0, 0, 4, 0)
        Me.txtChassisNo.Dock = DockStyle.Fill
        Me.txtChassisNo.Font = FNT_INPUT
        Me.txtChassisNo.ForeColor = CLR_TEXT
        Me.lblRegistrationNo.Text = "登録番号"
        Me.lblRegistrationNo.Dock = DockStyle.Fill
        Me.lblRegistrationNo.Font = FNT_LABEL
        Me.lblRegistrationNo.ForeColor = CLR_LABEL
        Me.lblRegistrationNo.TextAlign = ContentAlignment.MiddleRight
        Me.lblRegistrationNo.Padding = New Padding(0, 0, 4, 0)
        Me.txtRegistrationNo.Dock = DockStyle.Fill
        Me.txtRegistrationNo.Font = FNT_INPUT
        Me.txtRegistrationNo.ForeColor = CLR_TEXT
        Me.tblVehicle.Controls.Add(Me.lblChassisNo, 0, 0)
        Me.tblVehicle.Controls.Add(Me.txtChassisNo, 1, 0)
        Me.tblVehicle.Controls.Add(Me.lblRegistrationNo, 2, 0)
        Me.tblVehicle.Controls.Add(Me.txtRegistrationNo, 3, 0)

        Me.tblVehicle.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblVehicleType.Text = "車種"
        Me.lblVehicleType.Dock = DockStyle.Fill
        Me.lblVehicleType.Font = FNT_LABEL
        Me.lblVehicleType.ForeColor = CLR_LABEL
        Me.lblVehicleType.TextAlign = ContentAlignment.MiddleRight
        Me.lblVehicleType.Padding = New Padding(0, 0, 4, 0)
        Me.txtVehicleType.Dock = DockStyle.Fill
        Me.txtVehicleType.Font = FNT_INPUT
        Me.txtVehicleType.ForeColor = CLR_TEXT
        Me.lblInspectionDate.Text = "車検期限"
        Me.lblInspectionDate.Dock = DockStyle.Fill
        Me.lblInspectionDate.Font = FNT_LABEL
        Me.lblInspectionDate.ForeColor = CLR_LABEL
        Me.lblInspectionDate.TextAlign = ContentAlignment.MiddleRight
        Me.lblInspectionDate.Padding = New Padding(0, 0, 4, 0)
        Me.dtpInspectionDate.Dock = DockStyle.Fill
        Me.dtpInspectionDate.Font = FNT_INPUT
        Me.dtpInspectionDate.Format = DateTimePickerFormat.Short
        Me.dtpInspectionDate.ShowCheckBox = True
        Me.dtpInspectionDate.Checked = False
        Me.tblVehicle.Controls.Add(Me.lblVehicleType, 0, 1)
        Me.tblVehicle.Controls.Add(Me.txtVehicleType, 1, 1)
        Me.tblVehicle.Controls.Add(Me.lblInspectionDate, 2, 1)
        Me.tblVehicle.Controls.Add(Me.dtpInspectionDate, 3, 1)

        Me.tblVehicle.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblMileageLimit.Text = "走行距離制限"
        Me.lblMileageLimit.Dock = DockStyle.Fill
        Me.lblMileageLimit.Font = FNT_LABEL
        Me.lblMileageLimit.ForeColor = CLR_LABEL
        Me.lblMileageLimit.TextAlign = ContentAlignment.MiddleRight
        Me.lblMileageLimit.Padding = New Padding(0, 0, 4, 0)
        Me.txtMileageLimit.Dock = DockStyle.Fill
        Me.txtMileageLimit.Font = FNT_INPUT
        Me.txtMileageLimit.ForeColor = CLR_TEXT
        Me.tblVehicle.Controls.Add(Me.lblMileageLimit, 0, 2)
        Me.tblVehicle.Controls.Add(Me.txtMileageLimit, 1, 2)
        Me.tblVehicle.SetColumnSpan(Me.txtMileageLimit, 3)
        Me.tblVehicle.RowCount = 3

        ' === pnlOfficeEquip ===
        Me.pnlOfficeEquip.Dock = DockStyle.Top
        Me.pnlOfficeEquip.AutoSize = True
        Me.pnlOfficeEquip.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.pnlOfficeEquip.Visible = False
        Me.pnlOfficeEquip.Controls.Add(Me.tblOfficeEquip)

        Me.tblOfficeEquip.Dock = DockStyle.Top
        Me.tblOfficeEquip.AutoSize = True
        Me.tblOfficeEquip.ColumnCount = 4
        Me.tblOfficeEquip.Padding = New Padding(8)
        Me.tblOfficeEquip.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblOfficeEquip.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Me.tblOfficeEquip.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        Me.tblOfficeEquip.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Me.tblOfficeEquip.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblModelNo.Text = "型番"
        Me.lblModelNo.Dock = DockStyle.Fill
        Me.lblModelNo.Font = FNT_LABEL
        Me.lblModelNo.ForeColor = CLR_LABEL
        Me.lblModelNo.TextAlign = ContentAlignment.MiddleRight
        Me.lblModelNo.Padding = New Padding(0, 0, 4, 0)
        Me.txtModelNo.Dock = DockStyle.Fill
        Me.txtModelNo.Font = FNT_INPUT
        Me.txtModelNo.ForeColor = CLR_TEXT
        Me.lblSerialNo.Text = "シリアルNo"
        Me.lblSerialNo.Dock = DockStyle.Fill
        Me.lblSerialNo.Font = FNT_LABEL
        Me.lblSerialNo.ForeColor = CLR_LABEL
        Me.lblSerialNo.TextAlign = ContentAlignment.MiddleRight
        Me.lblSerialNo.Padding = New Padding(0, 0, 4, 0)
        Me.txtSerialNo.Dock = DockStyle.Fill
        Me.txtSerialNo.Font = FNT_INPUT
        Me.txtSerialNo.ForeColor = CLR_TEXT
        Me.tblOfficeEquip.Controls.Add(Me.lblModelNo, 0, 0)
        Me.tblOfficeEquip.Controls.Add(Me.txtModelNo, 1, 0)
        Me.tblOfficeEquip.Controls.Add(Me.lblSerialNo, 2, 0)
        Me.tblOfficeEquip.Controls.Add(Me.txtSerialNo, 3, 0)

        Me.tblOfficeEquip.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        Me.lblMaintenanceDate.Text = "保守期限"
        Me.lblMaintenanceDate.Dock = DockStyle.Fill
        Me.lblMaintenanceDate.Font = FNT_LABEL
        Me.lblMaintenanceDate.ForeColor = CLR_LABEL
        Me.lblMaintenanceDate.TextAlign = ContentAlignment.MiddleRight
        Me.lblMaintenanceDate.Padding = New Padding(0, 0, 4, 0)
        Me.dtpMaintenanceDate.Dock = DockStyle.Fill
        Me.dtpMaintenanceDate.Font = FNT_INPUT
        Me.dtpMaintenanceDate.Format = DateTimePickerFormat.Short
        Me.dtpMaintenanceDate.ShowCheckBox = True
        Me.dtpMaintenanceDate.Checked = False
        Me.lblMaintenanceContract.Text = "保守契約"
        Me.lblMaintenanceContract.Dock = DockStyle.Fill
        Me.lblMaintenanceContract.Font = FNT_LABEL
        Me.lblMaintenanceContract.ForeColor = CLR_LABEL
        Me.lblMaintenanceContract.TextAlign = ContentAlignment.MiddleRight
        Me.lblMaintenanceContract.Padding = New Padding(0, 0, 4, 0)
        Me.txtMaintenanceContract.Dock = DockStyle.Fill
        Me.txtMaintenanceContract.Font = FNT_INPUT
        Me.txtMaintenanceContract.ForeColor = CLR_TEXT
        Me.tblOfficeEquip.Controls.Add(Me.lblMaintenanceDate, 0, 1)
        Me.tblOfficeEquip.Controls.Add(Me.dtpMaintenanceDate, 1, 1)
        Me.tblOfficeEquip.Controls.Add(Me.lblMaintenanceContract, 2, 1)
        Me.tblOfficeEquip.Controls.Add(Me.txtMaintenanceContract, 3, 1)
        Me.tblOfficeEquip.RowCount = 2

        ' === Form ===
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 12.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 650)
        Me.Text = "資産入力"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.BackColor = CLR_CARD
        Me.Controls.Add(Me.scrollPanel)
        Me.Controls.Add(Me.pnlBottom)

        CType(Me.dgvDeptAllocation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tblOfficeEquip.ResumeLayout(False)
        Me.pnlOfficeEquip.ResumeLayout(False)
        Me.pnlOfficeEquip.PerformLayout()
        Me.tblVehicle.ResumeLayout(False)
        Me.pnlVehicle.ResumeLayout(False)
        Me.pnlVehicle.PerformLayout()
        Me.tblRealEstate.ResumeLayout(False)
        Me.pnlRealEstate.ResumeLayout(False)
        Me.pnlRealEstate.PerformLayout()
        Me.grpCategorySpecific.ResumeLayout(False)
        Me.grpCategorySpecific.PerformLayout()
        Me.pnlDeptButtons.ResumeLayout(False)
        Me.grpDeptAllocation.ResumeLayout(False)
        Me.tblBasicInfo.ResumeLayout(False)
        Me.grpBasicInfo.ResumeLayout(False)
        Me.grpBasicInfo.PerformLayout()
        Me.scrollPanel.ResumeLayout(False)
        Me.scrollPanel.PerformLayout()
        Me.pnlBottom.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents scrollPanel As System.Windows.Forms.Panel
    Friend WithEvents grpBasicInfo As System.Windows.Forms.GroupBox
    Friend WithEvents tblBasicInfo As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblAssetNo As System.Windows.Forms.Label
    Friend WithEvents txtAssetNo As System.Windows.Forms.TextBox
    Friend WithEvents lblAssetCategory As System.Windows.Forms.Label
    Friend WithEvents lblAssetCategoryDisplay As System.Windows.Forms.Label
    Friend WithEvents lblAssetName As System.Windows.Forms.Label
    Friend WithEvents txtAssetName As System.Windows.Forms.TextBox
    Friend WithEvents lblCompany As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents lblInstallLocation As System.Windows.Forms.Label
    Friend WithEvents txtInstallLocation As System.Windows.Forms.TextBox
    Friend WithEvents lblRemarks As System.Windows.Forms.Label
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents grpDeptAllocation As System.Windows.Forms.GroupBox
    Friend WithEvents pnlDeptButtons As System.Windows.Forms.Panel
    Friend WithEvents btnAddDept As System.Windows.Forms.Button
    Friend WithEvents btnRemoveDept As System.Windows.Forms.Button
    Friend WithEvents lblAllocationTotal As System.Windows.Forms.Label
    Friend WithEvents dgvDeptAllocation As System.Windows.Forms.DataGridView
    Friend WithEvents grpCategorySpecific As System.Windows.Forms.GroupBox
    Friend WithEvents pnlRealEstate As System.Windows.Forms.Panel
    Friend WithEvents tblRealEstate As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblStructure As System.Windows.Forms.Label
    Friend WithEvents txtStructure As System.Windows.Forms.TextBox
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents lblLayout As System.Windows.Forms.Label
    Friend WithEvents txtLayout As System.Windows.Forms.TextBox
    Friend WithEvents lblCompletion As System.Windows.Forms.Label
    Friend WithEvents dtpCompletion As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblLandlordName As System.Windows.Forms.Label
    Friend WithEvents txtLandlordName As System.Windows.Forms.TextBox
    Friend WithEvents lblBrokerCompany As System.Windows.Forms.Label
    Friend WithEvents txtBrokerCompany As System.Windows.Forms.TextBox
    Friend WithEvents lblUsageRestrictions As System.Windows.Forms.Label
    Friend WithEvents txtUsageRestrictions As System.Windows.Forms.TextBox
    Friend WithEvents pnlVehicle As System.Windows.Forms.Panel
    Friend WithEvents tblVehicle As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblChassisNo As System.Windows.Forms.Label
    Friend WithEvents txtChassisNo As System.Windows.Forms.TextBox
    Friend WithEvents lblRegistrationNo As System.Windows.Forms.Label
    Friend WithEvents txtRegistrationNo As System.Windows.Forms.TextBox
    Friend WithEvents lblVehicleType As System.Windows.Forms.Label
    Friend WithEvents txtVehicleType As System.Windows.Forms.TextBox
    Friend WithEvents lblInspectionDate As System.Windows.Forms.Label
    Friend WithEvents dtpInspectionDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblMileageLimit As System.Windows.Forms.Label
    Friend WithEvents txtMileageLimit As System.Windows.Forms.TextBox
    Friend WithEvents pnlOfficeEquip As System.Windows.Forms.Panel
    Friend WithEvents tblOfficeEquip As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblModelNo As System.Windows.Forms.Label
    Friend WithEvents txtModelNo As System.Windows.Forms.TextBox
    Friend WithEvents lblSerialNo As System.Windows.Forms.Label
    Friend WithEvents txtSerialNo As System.Windows.Forms.TextBox
    Friend WithEvents lblMaintenanceDate As System.Windows.Forms.Label
    Friend WithEvents dtpMaintenanceDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblMaintenanceContract As System.Windows.Forms.Label
    Friend WithEvents txtMaintenanceContract As System.Windows.Forms.TextBox

End Class
