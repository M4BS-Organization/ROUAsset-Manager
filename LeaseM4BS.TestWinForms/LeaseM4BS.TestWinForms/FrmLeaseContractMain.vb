Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmLeaseContractMain
    Inherits Form

    Private ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
    Private ReadOnly CLR_BG As Color = Color.FromArgb(248, 249, 250)
    Private ReadOnly CLR_CARD As Color = Color.White
    Private ReadOnly CLR_BORDER As Color = Color.FromArgb(222, 226, 230)
    Private ReadOnly CLR_READONLY As Color = Color.FromArgb(233, 236, 239)
    Private ReadOnly CLR_ACCENT As Color = Color.FromArgb(40, 167, 69)
    Private ReadOnly CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
    Private ReadOnly CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
    Private ReadOnly FNT_LABEL As Font = New Font("Meiryo", 9.0F, FontStyle.Bold)
    Private ReadOnly FNT_INPUT As Font = New Font("Meiryo", 9.75F, FontStyle.Regular)
    Private ReadOnly FNT_SECTION As Font = New Font("Meiryo", 10.0F, FontStyle.Bold)

    Private _isLoaded As Boolean = False
    Private _defaultTaxRate As Decimal = 0.10D
    Private _tooltipProvider As ToolTip

    Private pnlHeader As Panel
    Private pnlBody As Panel
    Private tabMain As TabControl
    Private pgContract As TabPage
    Private pgInitial As TabPage
    Private pgAccounting As TabPage
    Private pgSublease As TabPage
    Private pgJudgment As TabPage

    Private txtUpdateCount As TextBox
    Private txtChangeCount As TextBox
    Private txtDrafter As TextBox
    Private txtApprovalNo As TextBox
    Private txtApprovalDate As TextBox
    Private lblApprovalBadge As Label

    Private cmbContractType As ComboBox
    Private txtContractNo As TextBox
    Private lblContractClass As Label
    Private cmbMgmtDeptCode As ComboBox
    Private txtMgmtDeptName As TextBox
    Private cmbCostDeptCode As ComboBox
    Private txtCostDeptName As TextBox
    Private txtManagementNo As TextBox
    Private txtLandlordName As TextBox
    Private txtBrokerCompany As TextBox
    Private txtPaymentAgent As TextBox
    Private txtGuarantor As TextBox

    Private txtPropertyName As TextBox
    Private txtLocation As TextBox
    Private txtSection As TextBox
    Private txtArea As TextBox
    Private txtLayout As TextBox
    Private txtUsageRestrictions As TextBox
    Private txtStructure As TextBox
    Private numUsefulLife As NumericUpDown
    Private dtpCompletion As DateTimePicker
    Private lblBuildingAge As Label
    Private dgvEquipment As DataGridView

    Private dtpStartDate As DateTimePicker
    Private dtpEndDate As DateTimePicker
    Private numFreePeriod As NumericUpDown
    Private lblLeaseMonths As Label
    Private numRenewalCount As NumericUpDown
    Private numRenewalRent As NumericUpDown
    Private cmbRenewalLikelihood As ComboBox
    Private numCancelNoticePeriod As NumericUpDown
    Private numCancelPenalty As NumericUpDown
    Private cmbCancelLikelihood As ComboBox
    Private numPurchasePrice As NumericUpDown
    Private cmbPurchaseLikelihood As ComboBox
    Private dtpMoveOutDate As DateTimePicker

    Private dgvInitialCosts As DataGridView
    Private numInitialDirectCost As NumericUpDown
    Private numRestorationCost As NumericUpDown
    Private numLeaseIncentive As NumericUpDown

    Private dgvMonthlyPayments As DataGridView
    Private lblMonthlyTotalExTax As Label
    Private lblMonthlyTotalTax As Label
    Private lblMonthlyTotalIncTax As Label
    Private numFairValue As NumericUpDown
    Private numEconomicLife As NumericUpDown
    Private numImplicitRate As NumericUpDown
    Private numIBR As NumericUpDown
    Private lblAppliedRate As Label

    Private chkSublease As CheckBox
    Private pnlSubleaseDetail As Panel
    Private txtSublesseeName As TextBox
    Private dtpSubleaseStart As DateTimePicker
    Private dtpSubleaseEnd As DateTimePicker
    Private txtSubleaseArea As TextBox
    Private dgvSubleaseIncome As DataGridView

    Private chkSpecificAsset As CheckBox
    Private chkOwnershipTransfer As CheckBox
    Private chkBargainPurchase As CheckBox
    Private chkSpecializedAsset As CheckBox
    Private chkShortTerm As CheckBox
    Private chkLowValue As CheckBox
    Private lblPeriodRatio As Label
    Private lblPeriodRatioCalc As Label
    Private lblPVRatio As Label
    Private lblPVRatioCalc As Label
    Private lblJudgmentResult As Label
    Private txtJudgmentRationale As TextBox
    Private btnManualOverride As Button
    Private txtOverrideReason As TextBox
    Private _isManualOverride As Boolean = False
    Private cmbManualResult As ComboBox

    Private lblJudgmentPreview As Label

    Public Sub New()
        Me.Text = "新リース会計基準対応 不動産賃貸借・転貸管理システム"
        Me.Size = New Size(1400, 950)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = FNT_INPUT
        Me.MinimumSize = New Size(1280, 800)
        Me.BackColor = CLR_BG

        _tooltipProvider = New ToolTip() With {
            .AutoPopDelay = 10000,
            .InitialDelay = 300,
            .ReshowDelay = 200,
            .ShowAlways = True
        }

        BuildUI()

        _isLoaded = True
        RecalcAll()
    End Sub

    Private Sub BuildUI()
        Me.SuspendLayout()

        BuildGlobalHeader()
        BuildTabs()

        Me.Controls.Add(pnlBody)
        Me.Controls.Add(pnlHeader)

        Me.ResumeLayout(False)
    End Sub

    Private Sub BuildGlobalHeader()
        pnlHeader = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 110,
            .BackColor = CLR_HEADER,
            .Padding = New Padding(10, 8, 10, 8)
        }

        Dim tblHeader As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1
        }
        tblHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        tblHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))

        Dim flowButtons As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.LeftToRight,
            .WrapContents = False,
            .Padding = New Padding(0, 8, 0, 0)
        }

        Dim btnNames() As String = {"登録", "修正・変更", "更新・満了解約", "中途解約", "削除"}
        Dim btnColors() As Color = {
            Color.FromArgb(0, 123, 255),
            Color.FromArgb(23, 162, 184),
            Color.FromArgb(255, 193, 7),
            Color.FromArgb(220, 53, 69),
            Color.FromArgb(108, 117, 125)
        }
        For i As Integer = 0 To btnNames.Length - 1
            Dim btn As New Button() With {
                .Text = btnNames(i),
                .Width = 100,
                .Height = 32,
                .FlatStyle = FlatStyle.Flat,
                .BackColor = btnColors(i),
                .ForeColor = Color.White,
                .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
                .Margin = New Padding(0, 0, 4, 0),
                .Cursor = Cursors.Hand
            }
            btn.FlatAppearance.BorderSize = 0
            flowButtons.Controls.Add(btn)
        Next

        Dim pnlMgmt As New Panel() With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10, 2, 0, 2)
        }

        Dim tblMgmt As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 7,
            .RowCount = 2,
            .BackColor = Color.FromArgb(0, 40, 80),
            .Padding = New Padding(4),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        }
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 65.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 65.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 55.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
        tblMgmt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblMgmt.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        tblMgmt.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))

        txtUpdateCount = CreateHeaderField("3")
        txtChangeCount = CreateHeaderField("1")
        txtDrafter = CreateHeaderField("山田太郎")
        txtApprovalNo = CreateHeaderField("R2025-0123")
        txtApprovalDate = CreateHeaderField("2025/01/15")

        lblApprovalBadge = New Label() With {
            .Text = "承認済",
            .BackColor = CLR_ACCENT,
            .ForeColor = Color.White,
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(4)
        }

        tblMgmt.Controls.Add(CreateHeaderLabel("更新回数"), 0, 0)
        tblMgmt.Controls.Add(txtUpdateCount, 1, 0)
        tblMgmt.Controls.Add(CreateHeaderLabel("変更回数"), 2, 0)
        tblMgmt.Controls.Add(txtChangeCount, 3, 0)
        tblMgmt.Controls.Add(CreateHeaderLabel("起案者"), 4, 0)
        tblMgmt.Controls.Add(txtDrafter, 5, 0)
        tblMgmt.Controls.Add(lblApprovalBadge, 6, 0)
        tblMgmt.SetRowSpan(lblApprovalBadge, 2)

        tblMgmt.Controls.Add(CreateHeaderLabel("稟議No"), 0, 1)
        tblMgmt.Controls.Add(txtApprovalNo, 1, 1)
        tblMgmt.Controls.Add(CreateHeaderLabel("承認日"), 2, 1)
        tblMgmt.Controls.Add(txtApprovalDate, 3, 1)

        pnlMgmt.Controls.Add(tblMgmt)

        tblHeader.Controls.Add(flowButtons, 0, 0)
        tblHeader.Controls.Add(pnlMgmt, 1, 0)

        pnlHeader.Controls.Add(tblHeader)
    End Sub

    Private Function CreateHeaderLabel(text As String) As Label
        Return New Label() With {
            .Text = text,
            .ForeColor = Color.FromArgb(180, 200, 220),
            .Font = New Font("Meiryo", 8.0F, FontStyle.Regular),
            .TextAlign = ContentAlignment.MiddleRight,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(2)
        }
    End Function

    Private Function CreateHeaderField(text As String) As TextBox
        Return New TextBox() With {
            .Text = text,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(0, 40, 80),
            .ForeColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
            .TextAlign = HorizontalAlignment.Center,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(2)
        }
    End Function

    Private Sub BuildTabs()
        pnlBody = New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(6, 4, 6, 4)}

        Dim tblBodyMain As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2
        }
        tblBodyMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        tblBodyMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))

        tabMain = New TabControl() With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold)
        }

        pgContract = New TabPage("契約") With {.BackColor = CLR_BG, .Padding = New Padding(6)}
        pgInitial = New TabPage("初回金") With {.BackColor = CLR_BG, .Padding = New Padding(6)}
        pgAccounting = New TabPage("会計") With {.BackColor = CLR_BG, .Padding = New Padding(6)}
        pgSublease = New TabPage("転貸") With {.BackColor = CLR_BG, .Padding = New Padding(6)}
        pgJudgment = New TabPage("リース判定") With {.BackColor = CLR_BG, .Padding = New Padding(6)}

        InitTabContract()
        InitTabInitialCost()
        InitTabAccounting()
        InitTabSublease()
        InitTabJudgment()

        tabMain.TabPages.AddRange({pgContract, pgInitial, pgAccounting, pgSublease, pgJudgment})

        lblJudgmentPreview = New Label() With {
            .Dock = DockStyle.Fill,
            .Text = "リース判定: ---",
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER,
            .BackColor = Color.FromArgb(230, 240, 250),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(10, 0, 0, 0)
        }
        _tooltipProvider.SetToolTip(lblJudgmentPreview, "各タブの入力値からリアルタイム算出された判定結果です")

        tblBodyMain.Controls.Add(tabMain, 0, 0)
        tblBodyMain.Controls.Add(lblJudgmentPreview, 0, 1)

        pnlBody.Controls.Add(tblBodyMain)
    End Sub

    Private Sub InitTabContract()
        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 1,
            .RowCount = 3
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim grpBasic As GroupBox = CreateSection("基本・管理情報")
        Dim tblBasic As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        cmbContractType = New ComboBox() With {
            .Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = FNT_INPUT
        }
        cmbContractType.Items.AddRange({"普通賃貸", "定期賃貸"})
        cmbContractType.SelectedIndex = 0

        txtContractNo = New TextBox() With {
            .Dock = DockStyle.Fill, .ReadOnly = True,
            .BackColor = CLR_READONLY, .Text = "LC-2025-0001"
        }
        _tooltipProvider.SetToolTip(txtContractNo, "契約番号は自動採番されます")

        lblContractClass = New Label() With {
            .Dock = DockStyle.Fill, .Text = "（自動判定）", .BackColor = CLR_READONLY,
            .TextAlign = ContentAlignment.MiddleCenter, .Font = FNT_LABEL
        }
        _tooltipProvider.SetToolTip(lblContractClass, "リース判定タブの結果が自動反映されます")

        AddFieldRow(tblBasic, "契約種類", cmbContractType, "契約番号", txtContractNo)

        Dim r2 As Integer = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("契約区分"), 0, r2)
        tblBasic.Controls.Add(lblContractClass, 1, r2)
        tblBasic.SetColumnSpan(lblContractClass, 3)
        tblBasic.RowCount += 1

        Dim pnlMgmtDept As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1
        }
        pnlMgmtDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        pnlMgmtDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        cmbMgmtDeptCode = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDown}
        txtMgmtDeptName = New TextBox() With {.Dock = DockStyle.Fill}
        pnlMgmtDept.Controls.Add(cmbMgmtDeptCode, 0, 0)
        pnlMgmtDept.Controls.Add(txtMgmtDeptName, 1, 0)

        Dim rMgmt As Integer = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("契約管理所属"), 0, rMgmt)
        tblBasic.Controls.Add(pnlMgmtDept, 1, rMgmt)
        tblBasic.SetColumnSpan(pnlMgmtDept, 3)
        tblBasic.RowCount += 1

        Dim pnlCostDept As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1
        }
        pnlCostDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        pnlCostDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        cmbCostDeptCode = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDown}
        txtCostDeptName = New TextBox() With {.Dock = DockStyle.Fill}
        pnlCostDept.Controls.Add(cmbCostDeptCode, 0, 0)
        pnlCostDept.Controls.Add(txtCostDeptName, 1, 0)

        Dim rCost As Integer = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("費用負担部署"), 0, rCost)
        tblBasic.Controls.Add(pnlCostDept, 1, rCost)
        tblBasic.SetColumnSpan(pnlCostDept, 3)
        tblBasic.RowCount += 1

        txtManagementNo = New TextBox() With {.Dock = DockStyle.Fill}
        AddFieldRow(tblBasic, "管理番号", txtManagementNo, Nothing, Nothing)

        grpBasic.Controls.Add(tblBasic)

        Dim grpProperty As GroupBox = CreateSection("物件詳細（不動産）")
        Dim tblProp As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 6, .Padding = New Padding(8)
        }
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))

        txtPropertyName = New TextBox() With {.Dock = DockStyle.Fill}
        txtLocation = New TextBox() With {.Dock = DockStyle.Fill}
        txtSection = New TextBox() With {.Dock = DockStyle.Fill}
        txtArea = New TextBox() With {.Dock = DockStyle.Fill}
        txtLayout = New TextBox() With {.Dock = DockStyle.Fill}
        txtStructure = New TextBox() With {.Dock = DockStyle.Fill}
        txtUsageRestrictions = New TextBox() With {
            .Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical
        }
        numUsefulLife = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 100, .Value = 47,
            .TextAlign = HorizontalAlignment.Right
        }
        _tooltipProvider.SetToolTip(numUsefulLife, "法定耐用年数（年）。リース判定の経済的耐用年数基準に使用")
        dtpCompletion = New DateTimePicker() With {
            .Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short,
            .ShowCheckBox = True
        }
        lblBuildingAge = New Label() With {
            .Dock = DockStyle.Fill, .Text = "---年", .BackColor = CLR_READONLY,
            .TextAlign = ContentAlignment.MiddleCenter
        }
        _tooltipProvider.SetToolTip(lblBuildingAge, "竣工日から自動計算")
        txtLandlordName = New TextBox() With {.Dock = DockStyle.Fill}
        txtBrokerCompany = New TextBox() With {.Dock = DockStyle.Fill}
        txtPaymentAgent = New TextBox() With {.Dock = DockStyle.Fill}
        txtGuarantor = New TextBox() With {.Dock = DockStyle.Fill}

        AddHandler dtpCompletion.ValueChanged, Sub(s, e) CalcBuildingAge()

        Dim rPropName As Integer = tblProp.RowCount
        tblProp.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblProp.Controls.Add(CreateFieldLabel("物件名"), 0, rPropName)
        tblProp.Controls.Add(txtPropertyName, 1, rPropName)
        tblProp.SetColumnSpan(txtPropertyName, 3)
        tblProp.RowCount += 1

        Dim rLoc As Integer = tblProp.RowCount
        tblProp.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblProp.Controls.Add(CreateFieldLabel("所在地"), 0, rLoc)
        tblProp.Controls.Add(txtLocation, 1, rLoc)
        tblProp.SetColumnSpan(txtLocation, 3)
        tblProp.Controls.Add(CreateFieldLabel("区画"), 4, rLoc)
        tblProp.Controls.Add(txtSection, 5, rLoc)
        tblProp.RowCount += 1

        AddField6Col(tblProp, "面積(㎡)", txtArea, "間取り", txtLayout, "構造・用途", txtStructure)
        AddField6Col(tblProp, "耐用年数(年)", numUsefulLife, "竣工(年月)", dtpCompletion, "築年数", lblBuildingAge)

        AddField6Col(tblProp, "貸主名", txtLandlordName, "仲介会社", txtBrokerCompany, Nothing, Nothing)
        AddField6Col(tblProp, "決済代行", txtPaymentAgent, "連帯保証人", txtGuarantor, Nothing, Nothing)

        dgvEquipment = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .Height = 120,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }
        dgvEquipment.Columns.Add("EquipName", "設備名")
        dgvEquipment.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "金額", .Name = "EquipAmount",
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvEquipment.Columns.Add("EquipDate", "日付")

        Dim rEquip As Integer = tblProp.RowCount
        tblProp.RowStyles.Add(New RowStyle(SizeType.Absolute, 130.0F))
        tblProp.Controls.Add(CreateFieldLabel("付属設備"), 0, rEquip)
        tblProp.Controls.Add(dgvEquipment, 1, rEquip)
        tblProp.SetColumnSpan(dgvEquipment, 3)
        tblProp.Controls.Add(CreateFieldLabel("用途・制限事項"), 4, rEquip)
        tblProp.Controls.Add(txtUsageRestrictions, 5, rEquip)
        tblProp.RowCount += 1

        grpProperty.Controls.Add(tblProp)

        Dim grpPeriod As GroupBox = CreateSection("期間・オプション・解約規定")
        Dim tblPeriod As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblPeriod.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tblPeriod.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblPeriod.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tblPeriod.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        dtpStartDate = New DateTimePicker() With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short}
        dtpEndDate = New DateTimePicker() With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short}
        numFreePeriod = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 60,
            .TextAlign = HorizontalAlignment.Right
        }
        lblLeaseMonths = New Label() With {
            .Dock = DockStyle.Fill, .Text = "---ヶ月",
            .BackColor = CLR_READONLY, .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Meiryo", 11.0F, FontStyle.Bold)
        }
        _tooltipProvider.SetToolTip(lblLeaseMonths, "リース期間 = (終了日 - 開始日の月数) - 無償期間")

        numRenewalCount = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 99,
            .TextAlign = HorizontalAlignment.Right
        }
        numRenewalRent = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        cmbRenewalLikelihood = New ComboBox() With {
            .Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbRenewalLikelihood.Items.AddRange({"確実", "高い", "低い"})

        numCancelNoticePeriod = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 120,
            .TextAlign = HorizontalAlignment.Right
        }
        numCancelPenalty = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        cmbCancelLikelihood = New ComboBox() With {
            .Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbCancelLikelihood.Items.AddRange({"確実", "高い", "低い"})

        numPurchasePrice = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        cmbPurchaseLikelihood = New ComboBox() With {
            .Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbPurchaseLikelihood.Items.AddRange({"確実", "高い", "低い"})

        dtpMoveOutDate = New DateTimePicker() With {
            .Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short,
            .ShowCheckBox = True, .Checked = False
        }

        AddHandler dtpStartDate.ValueChanged, Sub(s, e)
                                                  CalcLeaseMonths()
                                                  RecalcAll()
                                              End Sub
        AddHandler dtpEndDate.ValueChanged, Sub(s, e)
                                                CalcLeaseMonths()
                                                RecalcAll()
                                            End Sub
        AddHandler numFreePeriod.ValueChanged, Sub(s, e)
                                                   CalcLeaseMonths()
                                                   RecalcAll()
                                               End Sub

        AddSectionLabel(tblPeriod, "■ 契約期間")
        AddFieldRow(tblPeriod, "開始日", dtpStartDate, "終了日", dtpEndDate)
        AddFieldRow(tblPeriod, "無償期間（月）", numFreePeriod, "リース期間（月）", lblLeaseMonths)

        AddSectionLabel(tblPeriod, "■ 更新オプション")
        AddFieldRow(tblPeriod, "更新予測回数", numRenewalCount, "更新時賃料", numRenewalRent)
        AddFieldRow(tblPeriod, "更新行使可能性", cmbRenewalLikelihood, Nothing, Nothing)

        AddSectionLabel(tblPeriod, "■ 解約規定")
        AddFieldRow(tblPeriod, "解約告知期間（月）", numCancelNoticePeriod, "解約違約金", numCancelPenalty)
        AddFieldRow(tblPeriod, "解約行使可能性", cmbCancelLikelihood, Nothing, Nothing)

        AddSectionLabel(tblPeriod, "■ 購入オプション")
        AddFieldRow(tblPeriod, "購入オプション価額", numPurchasePrice, "購入行使可能性", cmbPurchaseLikelihood)

        AddSectionLabel(tblPeriod, "■ その他")
        AddFieldRow(tblPeriod, "退去予定日", dtpMoveOutDate, Nothing, Nothing)

        grpPeriod.Controls.Add(tblPeriod)

        mainTbl.Controls.Add(grpBasic, 0, 0)
        mainTbl.Controls.Add(grpProperty, 0, 1)
        mainTbl.Controls.Add(grpPeriod, 0, 2)

        scroll.Controls.Add(mainTbl)
        pgContract.Controls.Add(scroll)
    End Sub

    Private Sub InitTabInitialCost()
        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .RowCount = 2
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim grpCost As GroupBox = CreateSection("初回費用明細")
        grpCost.Height = 280
        grpCost.AutoSize = False

        dgvInitialCosts = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL,
                .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }

        Dim colItem As New DataGridViewComboBoxColumn() With {
            .HeaderText = "費目", .Name = "CostItem", .FillWeight = 20
        }
        colItem.Items.AddRange("敷金", "敷金償却額（返還不能分）", "礼金", "仲介手数料")

        dgvInitialCosts.Columns.Add(colItem)
        dgvInitialCosts.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "金額（税抜）", .Name = "AmountExTax", .FillWeight = 18,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvInitialCosts.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "消費税(10%)", .Name = "Tax", .FillWeight = 15, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvInitialCosts.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "金額（税込）", .Name = "AmountIncTax", .FillWeight = 18, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })

        Dim colAcctTreat As New DataGridViewComboBoxColumn() With {
            .HeaderText = "会計処理", .Name = "AcctTreatment", .FillWeight = 20
        }
        colAcctTreat.Items.AddRange("資産計上", "費用処理", "繰延処理", "預り金処理")
        dgvInitialCosts.Columns.Add(colAcctTreat)

        dgvInitialCosts.Rows.Add("敷金", 2000000, 0, 2000000, "預り金処理")
        dgvInitialCosts.Rows.Add("敷金償却額（返還不能分）", 500000, 50000, 550000, "費用処理")
        dgvInitialCosts.Rows.Add("礼金", 300000, 30000, 330000, "費用処理")
        dgvInitialCosts.Rows.Add("仲介手数料", 200000, 20000, 220000, "費用処理")

        AddHandler dgvInitialCosts.CellValueChanged, AddressOf OnInitialCostChanged
        AddHandler dgvInitialCosts.CellEndEdit, AddressOf OnInitialCostCellEndEdit

        grpCost.Controls.Add(dgvInitialCosts)

        Dim grpOther As GroupBox = CreateSection("その他初回費用（判定・会計連動）")
        Dim tblOther As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblOther.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblOther.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblOther.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblOther.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        numInitialDirectCost = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        _tooltipProvider.SetToolTip(numInitialDirectCost, "リース判定の現在価値計算に使用されます")
        numRestorationCost = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        numLeaseIncentive = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 9999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right
        }
        _tooltipProvider.SetToolTip(numLeaseIncentive, "貸主から受領したインセンティブ。使用権資産の測定額から控除")

        AddHandler numInitialDirectCost.ValueChanged, Sub(s, e) RecalcAll()

        AddFieldRow(tblOther, "初期直接費用", numInitialDirectCost, "原状回復費用見積", numRestorationCost)
        AddFieldRow(tblOther, "リース・インセンティブ", numLeaseIncentive, Nothing, Nothing)
        grpOther.Controls.Add(tblOther)

        mainTbl.Controls.Add(grpCost, 0, 0)
        mainTbl.Controls.Add(grpOther, 0, 1)

        scroll.Controls.Add(mainTbl)
        pgInitial.Controls.Add(scroll)
    End Sub

    Private Sub InitTabAccounting()
        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .RowCount = 2
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim grpMonthly As GroupBox = CreateSection("月額支払明細")
        grpMonthly.Height = 260
        grpMonthly.AutoSize = False

        Dim pnlGrid As New Panel() With {.Dock = DockStyle.Fill}

        dgvMonthlyPayments = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL,
                .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }

        Dim colMItem As New DataGridViewComboBoxColumn() With {
            .HeaderText = "科目", .Name = "MItem", .FillWeight = 14
        }
        colMItem.Items.AddRange("賃料", "管理費", "共益費")
        dgvMonthlyPayments.Columns.Add(colMItem)
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "支払額（税抜）", .Name = "MAmountExTax", .FillWeight = 14,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "消費税", .Name = "MTax", .FillWeight = 12, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "税込合計", .Name = "MTotalIncTax", .FillWeight = 14, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "振込先口座", .Name = "MBankAccount", .FillWeight = 18
        })
        Dim colPayMethod As New DataGridViewComboBoxColumn() With {
            .HeaderText = "支払方法", .Name = "MPayMethod", .FillWeight = 12
        }
        colPayMethod.Items.AddRange("振込", "口座振替", "手形", "現金")
        dgvMonthlyPayments.Columns.Add(colPayMethod)
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "支払日", .Name = "MPayDate", .FillWeight = 10
        })

        dgvMonthlyPayments.Rows.Add("賃料", 500000, 50000, 550000, "三菱UFJ 本店 1234567", "振込", "毎月末")
        dgvMonthlyPayments.Rows.Add("管理費", 50000, 5000, 55000, "三菱UFJ 本店 1234567", "振込", "毎月末")
        dgvMonthlyPayments.Rows.Add("共益費", 30000, 3000, 33000, "三菱UFJ 本店 1234567", "振込", "毎月末")

        AddHandler dgvMonthlyPayments.CellValueChanged, AddressOf OnMonthlyPaymentChanged
        AddHandler dgvMonthlyPayments.CellEndEdit, AddressOf OnMonthlyPaymentCellEndEdit

        Dim pnlTotal As New Panel() With {
            .Dock = DockStyle.Bottom, .Height = 30,
            .BackColor = Color.FromArgb(230, 240, 250)
        }
        Dim flowTotal As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.LeftToRight,
            .WrapContents = False,
            .Padding = New Padding(4, 4, 0, 0)
        }
        flowTotal.Controls.Add(New Label() With {
            .Text = "月額合計:", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 10, 0)
        })
        lblMonthlyTotalExTax = New Label() With {
            .Text = "税抜: 580,000", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 20, 0)
        }
        _tooltipProvider.SetToolTip(lblMonthlyTotalExTax, "月額支払明細の税抜金額合計")
        lblMonthlyTotalTax = New Label() With {
            .Text = "税: 58,000", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 20, 0)
        }
        lblMonthlyTotalIncTax = New Label() With {
            .Text = "税込: 638,000",
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold),
            .AutoSize = True, .ForeColor = CLR_HEADER,
            .Padding = New Padding(0, 1, 0, 0)
        }
        flowTotal.Controls.Add(lblMonthlyTotalExTax)
        flowTotal.Controls.Add(lblMonthlyTotalTax)
        flowTotal.Controls.Add(lblMonthlyTotalIncTax)
        pnlTotal.Controls.Add(flowTotal)

        pnlGrid.Controls.Add(dgvMonthlyPayments)
        pnlGrid.Controls.Add(pnlTotal)
        grpMonthly.Controls.Add(pnlGrid)

        Dim grpFinancial As GroupBox = CreateSection("財務パラメータ")
        Dim tblFin As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        numFairValue = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 99999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right,
            .Value = 50000000
        }
        _tooltipProvider.SetToolTip(numFairValue, "原資産の見積公正価値。PV割合判定の分母となります")
        numEconomicLife = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 100,
            .TextAlign = HorizontalAlignment.Right, .Value = 47
        }
        _tooltipProvider.SetToolTip(numEconomicLife, "経済的耐用年数（年）。リース期間割合の分母となります")
        numImplicitRate = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .DecimalPlaces = 2, .Maximum = 100,
            .Increment = 0.01D, .TextAlign = HorizontalAlignment.Right
        }
        _tooltipProvider.SetToolTip(numImplicitRate, "リースの計算利子率。0の場合はIBRが自動適用されます")
        numIBR = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .DecimalPlaces = 2, .Maximum = 100,
            .Increment = 0.01D, .TextAlign = HorizontalAlignment.Right,
            .Value = 2.5D
        }

        lblAppliedRate = New Label() With {
            .Dock = DockStyle.Fill, .Text = "適用割引率: IBR 2.50%",
            .BackColor = Color.FromArgb(255, 248, 220),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(133, 100, 4)
        }
        _tooltipProvider.SetToolTip(lblAppliedRate, "計算利子率が0またはnullの場合、自動的にIBRが適用されます")

        AddHandler numImplicitRate.ValueChanged, Sub(s, e)
                                                     UpdateAppliedRate()
                                                     RecalcAll()
                                                 End Sub
        AddHandler numIBR.ValueChanged, Sub(s, e)
                                            UpdateAppliedRate()
                                            RecalcAll()
                                        End Sub
        AddHandler numFairValue.ValueChanged, Sub(s, e) RecalcAll()
        AddHandler numEconomicLife.ValueChanged, Sub(s, e) RecalcAll()

        AddFieldRow(tblFin, "原資産見積公正価値", numFairValue, "経済的耐用年数(年)", numEconomicLife)
        AddFieldRow(tblFin, "リース計算利子率(%)", numImplicitRate, "追加借入利子率IBR(%)", numIBR)

        Dim rowRate As Integer = tblFin.RowCount
        tblFin.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblFin.Controls.Add(lblAppliedRate, 0, rowRate)
        tblFin.SetColumnSpan(lblAppliedRate, 4)
        tblFin.RowCount += 1

        grpFinancial.Controls.Add(tblFin)

        mainTbl.Controls.Add(grpMonthly, 0, 0)
        mainTbl.Controls.Add(grpFinancial, 0, 1)

        scroll.Controls.Add(mainTbl)
        pgAccounting.Controls.Add(scroll)
    End Sub

    Private Sub InitTabSublease()
        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .RowCount = 2
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim pnlToggle As New Panel() With {.Dock = DockStyle.Top, .Height = 40, .Padding = New Padding(8)}
        chkSublease = New CheckBox() With {
            .Text = "転貸（サブリース）あり",
            .AutoSize = True, .Font = FNT_SECTION,
            .Padding = New Padding(0, 4, 0, 0)
        }
        pnlToggle.Controls.Add(chkSublease)

        pnlSubleaseDetail = New Panel() With {.Dock = DockStyle.Top, .AutoSize = True, .Visible = False}
        AddHandler chkSublease.CheckedChanged, Sub(s, e) pnlSubleaseDetail.Visible = chkSublease.Checked

        Dim grpSubInfo As GroupBox = CreateSection("転貸先情報")
        Dim tblSub As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblSub.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tblSub.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblSub.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tblSub.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        txtSublesseeName = New TextBox() With {.Dock = DockStyle.Fill}
        dtpSubleaseStart = New DateTimePicker() With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short}
        dtpSubleaseEnd = New DateTimePicker() With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short}
        txtSubleaseArea = New TextBox() With {.Dock = DockStyle.Fill}

        AddFieldRow(tblSub, "転貸先名称", txtSublesseeName, "転貸面積", txtSubleaseArea)
        AddFieldRow(tblSub, "転貸開始日", dtpSubleaseStart, "転貸終了日", dtpSubleaseEnd)
        grpSubInfo.Controls.Add(tblSub)

        Dim grpSubIncome As GroupBox = CreateSection("転貸料受取テーブル")
        grpSubIncome.Height = 200
        grpSubIncome.AutoSize = False

        dgvSubleaseIncome = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }
        dgvSubleaseIncome.Columns.Add("SubItem", "科目")
        dgvSubleaseIncome.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "月額受取額（税抜）", .Name = "SubAmount",
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvSubleaseIncome.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "消費税", .Name = "SubTax", .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvSubleaseIncome.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "税込合計", .Name = "SubTotal", .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })

        grpSubIncome.Controls.Add(dgvSubleaseIncome)

        pnlSubleaseDetail.Controls.Add(grpSubIncome)
        pnlSubleaseDetail.Controls.Add(grpSubInfo)

        mainTbl.Controls.Add(pnlToggle, 0, 0)
        mainTbl.Controls.Add(pnlSubleaseDetail, 0, 1)

        scroll.Controls.Add(mainTbl)
        pgSublease.Controls.Add(scroll)
    End Sub

    Private Sub InitTabJudgment()
        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .RowCount = 4
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim grpCriteria As GroupBox = CreateSection("判定基準チェックリスト")
        Dim tblCriteria As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 2, .Padding = New Padding(8)
        }
        tblCriteria.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblCriteria.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Dim pnlApplicability As New Panel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        Dim lblAppTitle As New Label() With {
            .Text = "■ 該当性判定", .Dock = DockStyle.Top,
            .Font = FNT_SECTION, .ForeColor = CLR_HEADER, .Height = 24
        }
        chkSpecificAsset = New CheckBox() With {
            .Text = "特定資産等に該当", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        _tooltipProvider.SetToolTip(chkSpecificAsset, "リース対象が特定の資産に該当するかを判定")
        chkShortTerm = New CheckBox() With {
            .Text = "短期リース（12ヶ月以内）", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        chkLowValue = New CheckBox() With {
            .Text = "少額リース", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        pnlApplicability.Controls.Add(chkLowValue)
        pnlApplicability.Controls.Add(chkShortTerm)
        pnlApplicability.Controls.Add(chkSpecificAsset)
        pnlApplicability.Controls.Add(lblAppTitle)

        Dim pnlClassification As New Panel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        Dim lblClsTitle As New Label() With {
            .Text = "■ 分類判定", .Dock = DockStyle.Top,
            .Font = FNT_SECTION, .ForeColor = CLR_HEADER, .Height = 24
        }
        chkOwnershipTransfer = New CheckBox() With {
            .Text = "所有権移転条項あり", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        chkBargainPurchase = New CheckBox() With {
            .Text = "割安購入選択権あり", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        chkSpecializedAsset = New CheckBox() With {
            .Text = "借手専用仕様（特殊資産）", .AutoSize = True,
            .Dock = DockStyle.Top, .Padding = New Padding(10, 2, 0, 2)
        }
        pnlClassification.Controls.Add(chkSpecializedAsset)
        pnlClassification.Controls.Add(chkBargainPurchase)
        pnlClassification.Controls.Add(chkOwnershipTransfer)
        pnlClassification.Controls.Add(lblClsTitle)

        AddHandler chkSpecificAsset.CheckedChanged, Sub(s, e) RecalcAll()
        AddHandler chkShortTerm.CheckedChanged, Sub(s, e) RecalcAll()
        AddHandler chkLowValue.CheckedChanged, Sub(s, e) RecalcAll()
        AddHandler chkOwnershipTransfer.CheckedChanged, Sub(s, e) RecalcAll()
        AddHandler chkBargainPurchase.CheckedChanged, Sub(s, e) RecalcAll()
        AddHandler chkSpecializedAsset.CheckedChanged, Sub(s, e) RecalcAll()

        tblCriteria.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tblCriteria.Controls.Add(pnlApplicability, 0, 0)
        tblCriteria.Controls.Add(pnlClassification, 1, 0)
        grpCriteria.Controls.Add(tblCriteria)

        Dim grpIndicators As GroupBox = CreateSection("判定指標")
        Dim tblInd As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 2, .Padding = New Padding(8)
        }
        tblInd.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblInd.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Dim pnlPeriod As New Panel() With {
            .Dock = DockStyle.Fill, .AutoSize = True,
            .BackColor = CLR_CARD, .Padding = New Padding(10)
        }
        Dim lblPeriodTitle As New Label() With {
            .Text = "リース期間割合", .Dock = DockStyle.Top,
            .Font = FNT_SECTION, .ForeColor = CLR_HEADER, .Height = 24
        }
        lblPeriodRatio = New Label() With {
            .Text = "--.--%", .Dock = DockStyle.Top,
            .Font = New Font("Meiryo", 22.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER, .Height = 40,
            .TextAlign = ContentAlignment.MiddleCenter
        }
        lblPeriodRatioCalc = New Label() With {
            .Text = "リース期間 / 経済的耐用年数 = --- / --- (基準: 75%以上)",
            .Dock = DockStyle.Top,
            .Font = New Font("Meiryo", 8.5F), .ForeColor = Color.Gray,
            .Height = 20, .TextAlign = ContentAlignment.MiddleCenter
        }
        _tooltipProvider.SetToolTip(lblPeriodRatio, "リース期間(月) / (経済的耐用年数(年)*12) * 100。75%以上でFL判定")
        pnlPeriod.Controls.Add(lblPeriodRatioCalc)
        pnlPeriod.Controls.Add(lblPeriodRatio)
        pnlPeriod.Controls.Add(lblPeriodTitle)

        Dim pnlPV As New Panel() With {
            .Dock = DockStyle.Fill, .AutoSize = True,
            .BackColor = CLR_CARD, .Padding = New Padding(10)
        }
        Dim lblPVTitle As New Label() With {
            .Text = "現在価値割合", .Dock = DockStyle.Top,
            .Font = FNT_SECTION, .ForeColor = CLR_HEADER, .Height = 24
        }
        lblPVRatio = New Label() With {
            .Text = "--.--%", .Dock = DockStyle.Top,
            .Font = New Font("Meiryo", 22.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER, .Height = 40,
            .TextAlign = ContentAlignment.MiddleCenter
        }
        lblPVRatioCalc = New Label() With {
            .Text = "(初期直接費用 + 月額PV合計) / 公正価値 (基準: 90%以上)",
            .Dock = DockStyle.Top,
            .Font = New Font("Meiryo", 8.5F), .ForeColor = Color.Gray,
            .Height = 20, .TextAlign = ContentAlignment.MiddleCenter
        }
        _tooltipProvider.SetToolTip(lblPVRatio, "(初期直接費用 + 月額合計の現在価値) / 公正価値 * 100。90%以上でFL判定")
        pnlPV.Controls.Add(lblPVRatioCalc)
        pnlPV.Controls.Add(lblPVRatio)
        pnlPV.Controls.Add(lblPVTitle)

        tblInd.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tblInd.Controls.Add(pnlPeriod, 0, 0)
        tblInd.Controls.Add(pnlPV, 1, 0)
        grpIndicators.Controls.Add(tblInd)

        Dim grpResult As GroupBox = CreateSection("判定結果")
        grpResult.BackColor = Color.FromArgb(240, 248, 255)
        Dim tblResult As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .Padding = New Padding(10)
        }

        lblJudgmentResult = New Label() With {
            .Text = "---", .Dock = DockStyle.Top,
            .Font = New Font("Meiryo", 18.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER, .Height = 50,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = CLR_CARD
        }

        txtJudgmentRationale = New TextBox() With {
            .Dock = DockStyle.Top, .Multiline = True,
            .ReadOnly = True, .BackColor = CLR_READONLY,
            .ScrollBars = ScrollBars.Vertical,
            .Height = 80, .Font = New Font("Meiryo", 9.0F)
        }
        _tooltipProvider.SetToolTip(txtJudgmentRationale, "判定根拠は入力値から自動生成されます。監査証跡としてご利用ください")

        tblResult.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tblResult.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tblResult.Controls.Add(lblJudgmentResult, 0, 0)
        tblResult.Controls.Add(txtJudgmentRationale, 0, 1)
        grpResult.Controls.Add(tblResult)

        Dim grpOverride As GroupBox = CreateSection("手動変更")
        Dim tblOverride As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 3, .Padding = New Padding(8)
        }
        tblOverride.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tblOverride.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 200.0F))
        tblOverride.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        btnManualOverride = New Button() With {
            .Text = "手動で変更",
            .Width = 120, .Height = 28,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(255, 193, 7),
            .ForeColor = Color.Black,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand
        }
        btnManualOverride.FlatAppearance.BorderSize = 0

        cmbManualResult = New ComboBox() With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Enabled = False
        }
        cmbManualResult.Items.AddRange({
            "ファイナンス・リース（所有権移転）",
            "ファイナンス・リース（所有権移転外）",
            "オペレーティング・リース",
            "短期リース免除",
            "少額リース免除"
        })

        txtOverrideReason = New TextBox() With {.Dock = DockStyle.Fill, .Enabled = False}

        AddHandler btnManualOverride.Click, Sub(s, e)
                                                _isManualOverride = Not _isManualOverride
                                                cmbManualResult.Enabled = _isManualOverride
                                                txtOverrideReason.Enabled = _isManualOverride
                                                If _isManualOverride Then
                                                    btnManualOverride.Text = "自動判定に戻す"
                                                    btnManualOverride.BackColor = Color.FromArgb(108, 117, 125)
                                                    btnManualOverride.ForeColor = Color.White
                                                Else
                                                    btnManualOverride.Text = "手動で変更"
                                                    btnManualOverride.BackColor = Color.FromArgb(255, 193, 7)
                                                    btnManualOverride.ForeColor = Color.Black
                                                    RecalcAll()
                                                End If
                                            End Sub

        tblOverride.RowStyles.Add(New RowStyle(SizeType.Absolute, 34.0F))
        tblOverride.Controls.Add(btnManualOverride, 0, 0)
        tblOverride.Controls.Add(cmbManualResult, 1, 0)
        tblOverride.Controls.Add(txtOverrideReason, 2, 0)
        grpOverride.Controls.Add(tblOverride)

        mainTbl.Controls.Add(grpCriteria, 0, 0)
        mainTbl.Controls.Add(grpIndicators, 0, 1)
        mainTbl.Controls.Add(grpResult, 0, 2)
        mainTbl.Controls.Add(grpOverride, 0, 3)

        scroll.Controls.Add(mainTbl)
        pgJudgment.Controls.Add(scroll)
    End Sub

    Private Sub RecalcAll()
        If Not _isLoaded Then Return

        CalcLeaseMonths()
        CalcBuildingAge()
        UpdateAppliedRate()
        CalcMonthlyTotals()

        If Not _isManualOverride Then
            CalcJudgment()
        End If
    End Sub

    Private Sub CalcLeaseMonths()
        If Not _isLoaded Then Return
        Try
            Dim startDt As DateTime = dtpStartDate.Value
            Dim endDt As DateTime = dtpEndDate.Value
            Dim totalMonths As Integer = ((endDt.Year - startDt.Year) * 12) + (endDt.Month - startDt.Month)
            Dim freePeriodVal As Integer = CInt(numFreePeriod.Value)
            Dim leaseMonths As Integer = Math.Max(0, totalMonths - freePeriodVal)
            lblLeaseMonths.Text = leaseMonths.ToString() & "ヶ月"
            _tooltipProvider.SetToolTip(lblLeaseMonths,
                String.Format("計算式: ({0} - {1})の月数{2} - 無償期間{3}ヶ月 = {4}ヶ月",
                    startDt.ToString("yyyy/MM/dd"), endDt.ToString("yyyy/MM/dd"),
                    totalMonths, freePeriodVal, leaseMonths))
        Catch ex As Exception
            lblLeaseMonths.Text = "---ヶ月"
        End Try
    End Sub

    Private Sub CalcBuildingAge()
        If Not _isLoaded Then Return
        Try
            If dtpCompletion.Checked Then
                Dim age As Integer = DateTime.Now.Year - dtpCompletion.Value.Year
                lblBuildingAge.Text = age.ToString() & "年"
            Else
                lblBuildingAge.Text = "---年"
            End If
        Catch ex As Exception
            lblBuildingAge.Text = "---年"
        End Try
    End Sub

    Private Sub UpdateAppliedRate()
        If Not _isLoaded Then Return
        If numImplicitRate.Value > 0 Then
            lblAppliedRate.Text = String.Format("適用割引率: 計算利子率 {0:F2}%", numImplicitRate.Value)
            _tooltipProvider.SetToolTip(lblAppliedRate, "リースの計算利子率が設定されているため、こちらを適用")
        Else
            lblAppliedRate.Text = String.Format("適用割引率: IBR {0:F2}%", numIBR.Value)
            _tooltipProvider.SetToolTip(lblAppliedRate, "計算利子率が0のため、追加借入利子率(IBR)を自動適用")
        End If
    End Sub

    Private Sub CalcMonthlyTotals()
        If Not _isLoaded Then Return
        Dim totalExTax As Decimal = 0
        Dim totalTax As Decimal = 0
        Dim totalIncTax As Decimal = 0

        For Each row As DataGridViewRow In dgvMonthlyPayments.Rows
            If row.IsNewRow Then Continue For
            Try
                Dim exTax As Decimal = 0
                If row.Cells("MAmountExTax").Value IsNot Nothing Then
                    Decimal.TryParse(row.Cells("MAmountExTax").Value.ToString().Replace(",", ""), exTax)
                End If
                Dim tax As Decimal = Math.Floor(exTax * _defaultTaxRate)
                Dim incTax As Decimal = exTax + tax
                row.Cells("MTax").Value = tax
                row.Cells("MTotalIncTax").Value = incTax
                totalExTax += exTax
                totalTax += tax
                totalIncTax += incTax
            Catch ex As Exception
            End Try
        Next

        lblMonthlyTotalExTax.Text = String.Format("税抜: {0:N0}", totalExTax)
        lblMonthlyTotalTax.Text = String.Format("税: {0:N0}", totalTax)
        lblMonthlyTotalIncTax.Text = String.Format("税込: {0:N0}", totalIncTax)
    End Sub

    Private Sub CalcJudgment()
        If Not _isLoaded Then Return

        Dim rationale As New System.Text.StringBuilder()
        Dim result As String = ""

        If chkShortTerm.Checked Then
            result = "短期リース免除"
            rationale.AppendLine("- 短期リース（12ヶ月以内）に該当するため、認識免除を適用")
            UpdateJudgmentDisplay(result, rationale.ToString())
            Return
        End If

        If chkLowValue.Checked Then
            result = "少額リース免除"
            rationale.AppendLine("- 少額リースに該当するため、認識免除を適用")
            UpdateJudgmentDisplay(result, rationale.ToString())
            Return
        End If

        Dim leaseMonths As Integer = GetLeaseMonths()
        Dim economicLifeMonths As Integer = CInt(numEconomicLife.Value) * 12
        Dim periodRatio As Double = 0
        If economicLifeMonths > 0 Then
            periodRatio = (leaseMonths / CDbl(economicLifeMonths)) * 100.0
        End If

        lblPeriodRatio.Text = String.Format("{0:F1}%", periodRatio)
        lblPeriodRatioCalc.Text = String.Format(
            "{0}ヶ月 / {1}ヶ月 = {2:F1}% (基準: 75%以上)",
            leaseMonths, economicLifeMonths, periodRatio)
        _tooltipProvider.SetToolTip(lblPeriodRatio,
            String.Format("リース期間{0}ヶ月 / 経済的耐用年数{1}年({2}ヶ月) = {3:F1}%",
                leaseMonths, numEconomicLife.Value, economicLifeMonths, periodRatio))

        If periodRatio >= 75.0 Then
            lblPeriodRatio.ForeColor = CLR_ACCENT
        Else
            lblPeriodRatio.ForeColor = CLR_HEADER
        End If

        Dim monthlyExTax As Decimal = GetMonthlyTotalExTax()
        Dim appliedRate As Double = GetAppliedRate()
        Dim monthlyRate As Double = appliedRate / 100.0 / 12.0
        Dim pvMonthly As Double = 0
        If monthlyRate > 0 AndAlso leaseMonths > 0 Then
            pvMonthly = CDbl(monthlyExTax) * ((1 - Math.Pow(1 + monthlyRate, -leaseMonths)) / monthlyRate)
        ElseIf leaseMonths > 0 Then
            pvMonthly = CDbl(monthlyExTax) * leaseMonths
        End If

        Dim initialDirect As Double = CDbl(numInitialDirectCost.Value)
        Dim totalPV As Double = initialDirect + pvMonthly
        Dim fairValue As Double = CDbl(numFairValue.Value)
        Dim pvRatio As Double = 0
        If fairValue > 0 Then
            pvRatio = (totalPV / fairValue) * 100.0
        End If

        lblPVRatio.Text = String.Format("{0:F1}%", pvRatio)
        lblPVRatioCalc.Text = String.Format(
            "({0:N0} + {1:N0}) / {2:N0} = {3:F1}% (基準: 90%以上)",
            initialDirect, pvMonthly, fairValue, pvRatio)
        _tooltipProvider.SetToolTip(lblPVRatio,
            String.Format("初期直接費用{0:N0} + 月額PV{1:N0}(利率{2:F2}%,{3}ヶ月) = {4:N0} / 公正価値{5:N0} = {6:F1}%",
                initialDirect, pvMonthly, appliedRate, leaseMonths, totalPV, fairValue, pvRatio))

        If pvRatio >= 90.0 Then
            lblPVRatio.ForeColor = CLR_ACCENT
        Else
            lblPVRatio.ForeColor = CLR_HEADER
        End If

        Dim isFL As Boolean = False

        If chkOwnershipTransfer.Checked Then
            rationale.AppendLine("- 所有権移転条項あり → ファイナンス・リース（所有権移転）")
            result = "ファイナンス・リース（所有権移転）"
            isFL = True
        End If

        If chkBargainPurchase.Checked Then
            rationale.AppendLine("- 割安購入選択権あり → ファイナンス・リース（所有権移転）")
            If result = "" Then result = "ファイナンス・リース（所有権移転）"
            isFL = True
        End If

        If chkSpecializedAsset.Checked Then
            rationale.AppendLine("- 借手専用仕様の資産 → ファイナンス・リース（所有権移転外）")
            If result = "" Then result = "ファイナンス・リース（所有権移転外）"
            isFL = True
        End If

        If periodRatio >= 75.0 Then
            rationale.AppendLine(
                String.Format("- リース期間割合 {0:F1}% >= 75% → ファイナンス・リース該当", periodRatio))
            If result = "" Then result = "ファイナンス・リース（所有権移転外）"
            isFL = True
        End If

        If pvRatio >= 90.0 Then
            rationale.AppendLine(
                String.Format("- 現在価値割合 {0:F1}% >= 90% → ファイナンス・リース該当", pvRatio))
            If result = "" Then result = "ファイナンス・リース（所有権移転外）"
            isFL = True
        End If

        If Not isFL Then
            result = "オペレーティング・リース"
            rationale.AppendLine("- 上記いずれの基準にも該当しないため、オペレーティング・リースと判定")
            rationale.AppendLine(String.Format("  リース期間割合: {0:F1}% (< 75%)", periodRatio))
            rationale.AppendLine(String.Format("  現在価値割合: {0:F1}% (< 90%)", pvRatio))
        End If

        UpdateJudgmentDisplay(result, rationale.ToString())
    End Sub

    Private Sub UpdateJudgmentDisplay(result As String, rationale As String)
        lblJudgmentResult.Text = result
        txtJudgmentRationale.Text = rationale.TrimEnd()

        If result.Contains("ファイナンス") Then
            lblJudgmentResult.ForeColor = Color.FromArgb(220, 53, 69)
            lblJudgmentResult.BackColor = Color.FromArgb(255, 240, 240)
        ElseIf result.Contains("免除") Then
            lblJudgmentResult.ForeColor = Color.FromArgb(23, 162, 184)
            lblJudgmentResult.BackColor = Color.FromArgb(230, 248, 255)
        Else
            lblJudgmentResult.ForeColor = CLR_ACCENT
            lblJudgmentResult.BackColor = Color.FromArgb(235, 255, 240)
        End If

        lblContractClass.Text = result
        lblJudgmentPreview.Text = "リース判定: " & result
    End Sub

    Private Function GetLeaseMonths() As Integer
        Try
            Dim txt As String = lblLeaseMonths.Text.Replace("ヶ月", "").Replace("---", "0")
            Return CInt(txt)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function GetMonthlyTotalExTax() As Decimal
        Dim total As Decimal = 0
        For Each row As DataGridViewRow In dgvMonthlyPayments.Rows
            If row.IsNewRow Then Continue For
            Try
                Dim val As Decimal = 0
                If row.Cells("MAmountExTax").Value IsNot Nothing Then
                    Decimal.TryParse(row.Cells("MAmountExTax").Value.ToString().Replace(",", ""), val)
                End If
                total += val
            Catch ex As Exception
            End Try
        Next
        Return total
    End Function

    Private Function GetAppliedRate() As Double
        If numImplicitRate.Value > 0 Then
            Return CDbl(numImplicitRate.Value)
        Else
            Return CDbl(numIBR.Value)
        End If
    End Function

    Private Sub OnInitialCostChanged(sender As Object, e As DataGridViewCellEventArgs)
        If Not _isLoaded Then Return
        If e.RowIndex < 0 Then Return
        Try
            Dim row As DataGridViewRow = dgvInitialCosts.Rows(e.RowIndex)
            If e.ColumnIndex = dgvInitialCosts.Columns("AmountExTax").Index Then
                Dim exTax As Decimal = 0
                If row.Cells("AmountExTax").Value IsNot Nothing Then
                    Decimal.TryParse(row.Cells("AmountExTax").Value.ToString().Replace(",", ""), exTax)
                End If
                Dim costItem As String = ""
                If row.Cells("CostItem").Value IsNot Nothing Then
                    costItem = row.Cells("CostItem").Value.ToString()
                End If
                Dim taxRate As Decimal = _defaultTaxRate
                If costItem = "敷金" Then taxRate = 0
                Dim tax As Decimal = Math.Floor(exTax * taxRate)
                row.Cells("Tax").Value = tax
                row.Cells("AmountIncTax").Value = exTax + tax
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub OnInitialCostCellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        dgvInitialCosts.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub OnMonthlyPaymentChanged(sender As Object, e As DataGridViewCellEventArgs)
        If Not _isLoaded Then Return
        If e.RowIndex < 0 Then Return
        CalcMonthlyTotals()
        RecalcAll()
    End Sub

    Private Sub OnMonthlyPaymentCellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        dgvMonthlyPayments.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Function CreateSection(title As String) As GroupBox
        Return New GroupBox() With {
            .Text = title,
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .BackColor = CLR_CARD,
            .Padding = New Padding(6, 12, 6, 6),
            .Margin = New Padding(0, 0, 0, 8)
        }
    End Function

    Private Function CreateFieldLabel(text As String) As Label
        Return New Label() With {
            .Text = text,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .TextAlign = ContentAlignment.MiddleRight,
            .Dock = DockStyle.Fill,
            .Padding = New Padding(0, 0, 4, 0)
        }
    End Function

    Private Sub AddFieldRow(tbl As TableLayoutPanel, lbl1 As String, ctrl1 As Control, lbl2 As String, ctrl2 As Control)
        Dim r As Integer = tbl.RowCount
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))

        tbl.Controls.Add(CreateFieldLabel(lbl1), 0, r)
        If ctrl1 IsNot Nothing Then tbl.Controls.Add(ctrl1, 1, r)

        If lbl2 IsNot Nothing Then
            tbl.Controls.Add(CreateFieldLabel(lbl2), 2, r)
            If ctrl2 IsNot Nothing Then tbl.Controls.Add(ctrl2, 3, r)
        ElseIf ctrl1 IsNot Nothing Then
            tbl.SetColumnSpan(ctrl1, 3)
        End If

        tbl.RowCount += 1
    End Sub

    Private Sub AddField6Col(tbl As TableLayoutPanel, lbl1 As String, ctrl1 As Control, lbl2 As String, ctrl2 As Control, lbl3 As String, ctrl3 As Control)
        Dim r As Integer = tbl.RowCount
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))

        tbl.Controls.Add(CreateFieldLabel(lbl1), 0, r)
        If ctrl1 IsNot Nothing Then tbl.Controls.Add(ctrl1, 1, r)

        If lbl2 IsNot Nothing Then
            tbl.Controls.Add(CreateFieldLabel(lbl2), 2, r)
            If ctrl2 IsNot Nothing Then tbl.Controls.Add(ctrl2, 3, r)
        End If

        If lbl3 IsNot Nothing Then
            tbl.Controls.Add(CreateFieldLabel(lbl3), 4, r)
            If ctrl3 IsNot Nothing Then tbl.Controls.Add(ctrl3, 5, r)
        End If

        tbl.RowCount += 1
    End Sub

    Private Sub AddSectionLabel(tbl As TableLayoutPanel, text As String)
        Dim r As Integer = tbl.RowCount
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        Dim lbl As New Label() With {
            .Text = text,
            .Font = FNT_LABEL,
            .ForeColor = CLR_HEADER,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.BottomLeft,
            .Padding = New Padding(0, 4, 0, 0)
        }
        tbl.Controls.Add(lbl, 0, r)
        tbl.SetColumnSpan(lbl, tbl.ColumnCount)
        tbl.RowCount += 1
    End Sub

End Class
