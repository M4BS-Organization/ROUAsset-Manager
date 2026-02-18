
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class ContractEntryForm
    Inherits Form

    ' ===== 共通: 下部コマンドボタン =====
    Private WithEvents btnRegister As Button
    Private WithEvents btnEdit As Button
    Private WithEvents btnUpdate As Button
    Private WithEvents btnDelete As Button
    Private WithEvents btnClose As Button

    ' ===== 画面上部: タイトル／パンくず（任意） =====
    Private lblTitle As Label

    ' ===== ルートレイアウト =====
    Private root As TableLayoutPanel
    Private tab As TabControl

    ' ===== [Tab1] 契約ヘッダー =====
    Private tabHeader As TabPage
    Private tlpHeader As TableLayoutPanel

    ' 上段（契約種別・資産内訳・管理部署・費用負担）
    Private lblContractType As Label, cboContractType As ComboBox
    Private lblAssetBreakdown As Label, cboAssetBreakdown As ComboBox
    Private lblMgmtDept As Label, cboMgmtDept As ComboBox
    Private lblCostDept As Label, cboCostDept As ComboBox
    Private lblGroup As Label, cboGroup As ComboBox

    ' 契約番号・契約名
    Private lblContractNo As Label, txtContractNo As TextBox
    Private lblContractName As Label, txtContractName As TextBox

    ' 資産属性（物件系）
    Private grpAsset As GroupBox, tlpAsset As TableLayoutPanel
    Private lblPropName As Label, txtPropName As TextBox
    Private lblKuakku As Label, txtKuakku As TextBox
    Private lblAddress As Label, txtAddress As TextBox
    Private lblMadori As Label, txtMadori As TextBox
    Private lblShunko As Label, dtpShunko As DateTimePicker
    Private lblKibo As Label, txtKibo As TextBox
    Private lblYoto As Label, cboYoto As ComboBox
    Private lblTaiyo As Label, numTaiyo As NumericUpDown
    Private lblChikun As Label, numChikun As NumericUpDown
    Private lblKinsi As Label, txtKinsi As TextBox

    ' 契約当事者
    Private grpParties As GroupBox, tlpParties As TableLayoutPanel
    Private lblLessorAddr As Label, txtLessorAddr As TextBox
    Private lblLessorName As Label, txtLessorName As TextBox
    Private lblLessorAcct As Label, txtLessorAcct As TextBox

    Private lblAgentAddr As Label, txtAgentAddr As TextBox
    Private lblAgentName As Label, txtAgentName As TextBox
    Private lblAgentAcct As Label, txtAgentAcct As TextBox

    Private lblLesseeAddr As Label, txtLesseeAddr As TextBox
    Private lblLesseeName As Label, txtLesseeName As TextBox
    Private lblLesseeAcct As Label, txtLesseeAcct As TextBox

    Private lblGuarantor1 As Label, txtGuarantor1 As TextBox
    Private lblGuarantor2 As Label, txtGuarantor2 As TextBox

    ' ===== [Tab2] 契約条項 =====
    Private tabTerms As TabPage
    Private tlpTerms As TableLayoutPanel
    Private lblStart As Label, dtpStart As DateTimePicker
    Private lblPeriod As Label, numPeriod As NumericUpDown
    Private lblEnd As Label, dtpEnd As DateTimePicker
    Private lblMushou As Label, numMushou As NumericUpDown
    Private lblNcn As Label, numNonCancelable As NumericUpDown
    Private lblNotice As Label, dtpNotice As DateTimePicker

    Private lblFirstPay As Label, dtpFirstPay As DateTimePicker
    Private lblApplyPeriod As Label, cboApplyPeriod As ComboBox
    Private lblPayInterval As Label, numPayInterval As NumericUpDown
    Private lblPayCount As Label, numPayCount As NumericUpDown
    Private lblLastPay As Label, dtpLastPay As DateTimePicker
    Private lblLastApply As Label, dtpLastApply As DateTimePicker

    ' ===== [Tab3] 約定支払 =====
    Private tabPayment As TabPage
    Private tlpPayment As TableLayoutPanel
    Private lblPay1 As Label, numPay1 As NumericUpDown
    Private lblTax As Label, numTax As NumericUpDown
    Private lblTaxIncl As Label, numTaxIncl As NumericUpDown
    Private lblAccountDetail As Label, cboAccountDetail As ComboBox
    Private lblBankAcct As Label, cboBankAcct As ComboBox
    Private lblAcctTotal As Label, numAcctTotal As NumericUpDown
    Private lblIndex As Label, cboIndex As ComboBox
    Private lblIndexAtContract As Label, txtIndexAtContract As TextBox

    Private lblRentTotal As Label, numRentTotal As NumericUpDown
    Private lblTaxSum As Label, numTaxSum As NumericUpDown
    Private lblGrossRent As Label, numGrossRent As NumericUpDown
    Private lblMaintIn As Label, numMaintIn As NumericUpDown
    Private lblResidualGuarantee As Label, numResidualGuarantee As NumericUpDown
    Private chkResidualGuarantee As CheckBox
    Private lblExpectedPay As Label, numExpectedPay As NumericUpDown
    Private lblLeaseFeeTotal As Label, numLeaseFeeTotal As NumericUpDown

    ' ===== [Tab4] 初回一時金 =====
    Private tabInitial As TabPage
    Private dgvInitial As DataGridView

    ' ===== [Tab5] オプション（延長／解約） =====
    Private tabOptions As TabPage
    Private splitOptions As SplitContainer

    ' 延長
    Private grpExtend As GroupBox, tlpExtend As TableLayoutPanel
    Private chkExtendRule As CheckBox
    Private lblExtendCond1 As Label, txtExtendCond1 As TextBox
    Private lblExtendCond2 As Label, txtExtendCond2 As TextBox
    Private lblExtendStart As Label, dtpExtendStart As DateTimePicker
    Private lblExtendPeriod As Label, numExtendPeriod As NumericUpDown
    Private lblExtendEnd As Label, dtpExtendEnd As DateTimePicker
    Private lblExtendMushou As Label, numExtendMushou As NumericUpDown
    Private lblExtendNonCancelable As Label, numExtendNonCancelable As NumericUpDown
    Private lblExtendNotice As Label, dtpExtendNotice As DateTimePicker
    Private lblExtendCount As Label, numExtendCount As NumericUpDown

    ' 解約
    Private grpCancel As GroupBox, tlpCancel As TableLayoutPanel
    Private chkCancelRule As CheckBox
    Private lblCancelCond1 As Label, txtCancelCond1 As TextBox
    Private lblCancelCond2 As Label, txtCancelCond2 As TextBox
    Private lblSaleProcAmt As Label, numSaleProcAmt As NumericUpDown
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents btnPanel As FlowLayoutPanel

    ' ▼ ContractEntryForm クラス内の先頭付近に追加
    Private Shared Function IsInDesignMode() As Boolean
        Try
            If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then
                Return True
            End If
            ' devenv(Visual Studio) から生成されているかの保険
            Dim p = System.Diagnostics.Process.GetCurrentProcess()
            If p IsNot Nothing AndAlso p.ProcessName?.ToLowerInvariant() = "devenv" Then
                Return True
            End If
        Catch
        End Try
        Return False
    End Function


    Public Sub New()
        Me.Font = New Font("Meiryo UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CType(128, Byte))
        Me.Text = "契約入力"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1100, 750)
        Me.KeyPreview = True

        If IsInDesignMode() Then
            ' --- デザイナー用の最小 UI（例：タイトルだけ） ---
            MyBase.SuspendLayout()
            Dim lbl As New Label() With {
            .Dock = DockStyle.Fill,
            .Text = "（デザイナー表示：実行時に本UIが構築されます）",
            .TextAlign = ContentAlignment.MiddleCenter
        }
            Me.Controls.Add(lbl)
            MyBase.ResumeLayout()
            Return
        End If

        ' --- 実行時だけ本来のUIを構築 ---
        InitializeComponent()   ' ← 既存のレイアウト構築メソッド
    End Sub

    Private Sub InitializeComponent()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.root = New System.Windows.Forms.TableLayoutPanel()
        Me.tab = New System.Windows.Forms.TabControl()
        Me.tabHeader = New System.Windows.Forms.TabPage()
        Me.tlpHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.lblContractType = New System.Windows.Forms.Label()
        Me.cboContractType = New System.Windows.Forms.ComboBox()
        Me.lblAssetBreakdown = New System.Windows.Forms.Label()
        Me.cboAssetBreakdown = New System.Windows.Forms.ComboBox()
        Me.lblMgmtDept = New System.Windows.Forms.Label()
        Me.cboMgmtDept = New System.Windows.Forms.ComboBox()
        Me.lblCostDept = New System.Windows.Forms.Label()
        Me.cboCostDept = New System.Windows.Forms.ComboBox()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.cboGroup = New System.Windows.Forms.ComboBox()
        Me.lblContractNo = New System.Windows.Forms.Label()
        Me.txtContractNo = New System.Windows.Forms.TextBox()
        Me.lblContractName = New System.Windows.Forms.Label()
        Me.txtContractName = New System.Windows.Forms.TextBox()
        Me.grpAsset = New System.Windows.Forms.GroupBox()
        Me.tlpAsset = New System.Windows.Forms.TableLayoutPanel()
        Me.lblPropName = New System.Windows.Forms.Label()
        Me.txtPropName = New System.Windows.Forms.TextBox()
        Me.lblKuakku = New System.Windows.Forms.Label()
        Me.txtKuakku = New System.Windows.Forms.TextBox()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.lblMadori = New System.Windows.Forms.Label()
        Me.txtMadori = New System.Windows.Forms.TextBox()
        Me.lblShunko = New System.Windows.Forms.Label()
        Me.dtpShunko = New System.Windows.Forms.DateTimePicker()
        Me.lblKibo = New System.Windows.Forms.Label()
        Me.txtKibo = New System.Windows.Forms.TextBox()
        Me.lblYoto = New System.Windows.Forms.Label()
        Me.cboYoto = New System.Windows.Forms.ComboBox()
        Me.lblTaiyo = New System.Windows.Forms.Label()
        Me.numTaiyo = New System.Windows.Forms.NumericUpDown()
        Me.lblChikun = New System.Windows.Forms.Label()
        Me.numChikun = New System.Windows.Forms.NumericUpDown()
        Me.lblKinsi = New System.Windows.Forms.Label()
        Me.txtKinsi = New System.Windows.Forms.TextBox()
        Me.grpParties = New System.Windows.Forms.GroupBox()
        Me.tlpParties = New System.Windows.Forms.TableLayoutPanel()
        Me.lblLessorAddr = New System.Windows.Forms.Label()
        Me.txtLessorAddr = New System.Windows.Forms.TextBox()
        Me.lblLessorName = New System.Windows.Forms.Label()
        Me.txtLessorName = New System.Windows.Forms.TextBox()
        Me.lblLessorAcct = New System.Windows.Forms.Label()
        Me.txtLessorAcct = New System.Windows.Forms.TextBox()
        Me.lblAgentAddr = New System.Windows.Forms.Label()
        Me.txtAgentAddr = New System.Windows.Forms.TextBox()
        Me.lblAgentName = New System.Windows.Forms.Label()
        Me.txtAgentName = New System.Windows.Forms.TextBox()
        Me.lblAgentAcct = New System.Windows.Forms.Label()
        Me.txtAgentAcct = New System.Windows.Forms.TextBox()
        Me.lblLesseeAddr = New System.Windows.Forms.Label()
        Me.txtLesseeAddr = New System.Windows.Forms.TextBox()
        Me.lblLesseeName = New System.Windows.Forms.Label()
        Me.txtLesseeName = New System.Windows.Forms.TextBox()
        Me.lblLesseeAcct = New System.Windows.Forms.Label()
        Me.txtLesseeAcct = New System.Windows.Forms.TextBox()
        Me.lblGuarantor1 = New System.Windows.Forms.Label()
        Me.txtGuarantor1 = New System.Windows.Forms.TextBox()
        Me.lblGuarantor2 = New System.Windows.Forms.Label()
        Me.txtGuarantor2 = New System.Windows.Forms.TextBox()
        Me.tabTerms = New System.Windows.Forms.TabPage()
        Me.tlpTerms = New System.Windows.Forms.TableLayoutPanel()
        Me.lblStart = New System.Windows.Forms.Label()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me.numPeriod = New System.Windows.Forms.NumericUpDown()
        Me.lblEnd = New System.Windows.Forms.Label()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.lblMushou = New System.Windows.Forms.Label()
        Me.numMushou = New System.Windows.Forms.NumericUpDown()
        Me.lblNcn = New System.Windows.Forms.Label()
        Me.numNonCancelable = New System.Windows.Forms.NumericUpDown()
        Me.lblNotice = New System.Windows.Forms.Label()
        Me.dtpNotice = New System.Windows.Forms.DateTimePicker()
        Me.lblFirstPay = New System.Windows.Forms.Label()
        Me.dtpFirstPay = New System.Windows.Forms.DateTimePicker()
        Me.lblApplyPeriod = New System.Windows.Forms.Label()
        Me.cboApplyPeriod = New System.Windows.Forms.ComboBox()
        Me.lblPayInterval = New System.Windows.Forms.Label()
        Me.numPayInterval = New System.Windows.Forms.NumericUpDown()
        Me.lblPayCount = New System.Windows.Forms.Label()
        Me.numPayCount = New System.Windows.Forms.NumericUpDown()
        Me.lblLastPay = New System.Windows.Forms.Label()
        Me.dtpLastPay = New System.Windows.Forms.DateTimePicker()
        Me.lblLastApply = New System.Windows.Forms.Label()
        Me.dtpLastApply = New System.Windows.Forms.DateTimePicker()
        Me.tabPayment = New System.Windows.Forms.TabPage()
        Me.tlpPayment = New System.Windows.Forms.TableLayoutPanel()
        Me.lblPay1 = New System.Windows.Forms.Label()
        Me.numPay1 = New System.Windows.Forms.NumericUpDown()
        Me.lblTax = New System.Windows.Forms.Label()
        Me.numTax = New System.Windows.Forms.NumericUpDown()
        Me.lblTaxIncl = New System.Windows.Forms.Label()
        Me.numTaxIncl = New System.Windows.Forms.NumericUpDown()
        Me.lblAccountDetail = New System.Windows.Forms.Label()
        Me.cboAccountDetail = New System.Windows.Forms.ComboBox()
        Me.lblBankAcct = New System.Windows.Forms.Label()
        Me.cboBankAcct = New System.Windows.Forms.ComboBox()
        Me.lblAcctTotal = New System.Windows.Forms.Label()
        Me.numAcctTotal = New System.Windows.Forms.NumericUpDown()
        Me.lblIndex = New System.Windows.Forms.Label()
        Me.cboIndex = New System.Windows.Forms.ComboBox()
        Me.lblIndexAtContract = New System.Windows.Forms.Label()
        Me.txtIndexAtContract = New System.Windows.Forms.TextBox()
        Me.lblRentTotal = New System.Windows.Forms.Label()
        Me.numRentTotal = New System.Windows.Forms.NumericUpDown()
        Me.lblTaxSum = New System.Windows.Forms.Label()
        Me.numTaxSum = New System.Windows.Forms.NumericUpDown()
        Me.lblGrossRent = New System.Windows.Forms.Label()
        Me.numGrossRent = New System.Windows.Forms.NumericUpDown()
        Me.lblMaintIn = New System.Windows.Forms.Label()
        Me.numMaintIn = New System.Windows.Forms.NumericUpDown()
        Me.lblResidualGuarantee = New System.Windows.Forms.Label()
        Me.numResidualGuarantee = New System.Windows.Forms.NumericUpDown()
        Me.chkResidualGuarantee = New System.Windows.Forms.CheckBox()
        Me.lblExpectedPay = New System.Windows.Forms.Label()
        Me.numExpectedPay = New System.Windows.Forms.NumericUpDown()
        Me.lblLeaseFeeTotal = New System.Windows.Forms.Label()
        Me.numLeaseFeeTotal = New System.Windows.Forms.NumericUpDown()
        Me.tabInitial = New System.Windows.Forms.TabPage()
        Me.dgvInitial = New System.Windows.Forms.DataGridView()
        Me.tabOptions = New System.Windows.Forms.TabPage()
        Me.splitOptions = New System.Windows.Forms.SplitContainer()
        Me.grpExtend = New System.Windows.Forms.GroupBox()
        Me.tlpExtend = New System.Windows.Forms.TableLayoutPanel()
        Me.chkExtendRule = New System.Windows.Forms.CheckBox()
        Me.lblExtendCond1 = New System.Windows.Forms.Label()
        Me.txtExtendCond1 = New System.Windows.Forms.TextBox()
        Me.lblExtendCond2 = New System.Windows.Forms.Label()
        Me.txtExtendCond2 = New System.Windows.Forms.TextBox()
        Me.lblExtendStart = New System.Windows.Forms.Label()
        Me.dtpExtendStart = New System.Windows.Forms.DateTimePicker()
        Me.lblExtendPeriod = New System.Windows.Forms.Label()
        Me.numExtendPeriod = New System.Windows.Forms.NumericUpDown()
        Me.lblExtendEnd = New System.Windows.Forms.Label()
        Me.dtpExtendEnd = New System.Windows.Forms.DateTimePicker()
        Me.lblExtendMushou = New System.Windows.Forms.Label()
        Me.numExtendMushou = New System.Windows.Forms.NumericUpDown()
        Me.lblExtendNonCancelable = New System.Windows.Forms.Label()
        Me.numExtendNonCancelable = New System.Windows.Forms.NumericUpDown()
        Me.lblExtendNotice = New System.Windows.Forms.Label()
        Me.dtpExtendNotice = New System.Windows.Forms.DateTimePicker()
        Me.lblExtendCount = New System.Windows.Forms.Label()
        Me.numExtendCount = New System.Windows.Forms.NumericUpDown()
        Me.grpCancel = New System.Windows.Forms.GroupBox()
        Me.tlpCancel = New System.Windows.Forms.TableLayoutPanel()
        Me.chkCancelRule = New System.Windows.Forms.CheckBox()
        Me.lblCancelCond1 = New System.Windows.Forms.Label()
        Me.txtCancelCond1 = New System.Windows.Forms.TextBox()
        Me.lblCancelCond2 = New System.Windows.Forms.Label()
        Me.txtCancelCond2 = New System.Windows.Forms.TextBox()
        Me.lblSaleProcAmt = New System.Windows.Forms.Label()
        Me.numSaleProcAmt = New System.Windows.Forms.NumericUpDown()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnRegister = New System.Windows.Forms.Button()
        Me.root.SuspendLayout()
        Me.tab.SuspendLayout()
        Me.tabHeader.SuspendLayout()
        Me.tlpHeader.SuspendLayout()
        Me.grpAsset.SuspendLayout()
        Me.tlpAsset.SuspendLayout()
        CType(Me.numTaiyo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numChikun, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpParties.SuspendLayout()
        Me.tlpParties.SuspendLayout()
        Me.tabTerms.SuspendLayout()
        Me.tlpTerms.SuspendLayout()
        CType(Me.numPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numMushou, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numNonCancelable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numPayInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numPayCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPayment.SuspendLayout()
        Me.tlpPayment.SuspendLayout()
        CType(Me.numPay1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTaxIncl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numAcctTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRentTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTaxSum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numGrossRent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numMaintIn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numResidualGuarantee, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numExpectedPay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numLeaseFeeTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabInitial.SuspendLayout()
        CType(Me.dgvInitial, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabOptions.SuspendLayout()
        CType(Me.splitOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitOptions.Panel1.SuspendLayout()
        Me.splitOptions.Panel2.SuspendLayout()
        Me.splitOptions.SuspendLayout()
        Me.grpExtend.SuspendLayout()
        Me.tlpExtend.SuspendLayout()
        CType(Me.numExtendPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numExtendMushou, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numExtendNonCancelable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numExtendCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCancel.SuspendLayout()
        Me.tlpCancel.SuspendLayout()
        CType(Me.numSaleProcAmt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.btnPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(100, 23)
        Me.lblTitle.TabIndex = 1
        '
        'root
        '
        Me.root.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.root.Controls.Add(Me.tab, 0, 0)
        Me.root.Controls.Add(Me.btnPanel, 0, 1)
        Me.root.Location = New System.Drawing.Point(0, 0)
        Me.root.Name = "root"
        Me.root.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.root.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        Me.root.Size = New System.Drawing.Size(200, 100)
        Me.root.TabIndex = 0
        '
        'tab
        '
        Me.tab.Controls.Add(Me.tabHeader)
        Me.tab.Controls.Add(Me.tabTerms)
        Me.tab.Controls.Add(Me.tabPayment)
        Me.tab.Controls.Add(Me.tabInitial)
        Me.tab.Controls.Add(Me.tabOptions)
        Me.tab.Location = New System.Drawing.Point(3, 3)
        Me.tab.Name = "tab"
        Me.tab.SelectedIndex = 0
        Me.tab.Size = New System.Drawing.Size(194, 38)
        Me.tab.TabIndex = 0
        '
        'tabHeader
        '
        Me.tabHeader.Controls.Add(Me.tlpHeader)
        Me.tabHeader.Location = New System.Drawing.Point(4, 22)
        Me.tabHeader.Name = "tabHeader"
        Me.tabHeader.Size = New System.Drawing.Size(186, 12)
        Me.tabHeader.TabIndex = 0
        Me.tabHeader.Text = "契約ヘッダー"
        '
        'tlpHeader
        '
        Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpHeader.Controls.Add(Me.lblContractType, 0, 0)
        Me.tlpHeader.Controls.Add(Me.cboContractType, 1, 0)
        Me.tlpHeader.Controls.Add(Me.lblAssetBreakdown, 2, 0)
        Me.tlpHeader.Controls.Add(Me.cboAssetBreakdown, 3, 0)
        Me.tlpHeader.Controls.Add(Me.lblMgmtDept, 0, 1)
        Me.tlpHeader.Controls.Add(Me.cboMgmtDept, 1, 1)
        Me.tlpHeader.Controls.Add(Me.lblCostDept, 2, 1)
        Me.tlpHeader.Controls.Add(Me.cboCostDept, 3, 1)
        Me.tlpHeader.Controls.Add(Me.lblGroup, 0, 2)
        Me.tlpHeader.Controls.Add(Me.cboGroup, 1, 2)
        Me.tlpHeader.Controls.Add(Me.lblContractNo, 2, 2)
        Me.tlpHeader.Controls.Add(Me.txtContractNo, 3, 2)
        Me.tlpHeader.Controls.Add(Me.lblContractName, 0, 3)
        Me.tlpHeader.Controls.Add(Me.txtContractName, 1, 3)
        Me.tlpHeader.Controls.Add(Me.grpAsset, 0, 4)
        Me.tlpHeader.Controls.Add(Me.grpParties, 0, 5)
        Me.tlpHeader.Location = New System.Drawing.Point(0, 0)
        Me.tlpHeader.Name = "tlpHeader"
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpHeader.Size = New System.Drawing.Size(200, 100)
        Me.tlpHeader.TabIndex = 0
        '
        'lblContractType
        '
        Me.lblContractType.Location = New System.Drawing.Point(3, 0)
        Me.lblContractType.Name = "lblContractType"
        Me.lblContractType.Size = New System.Drawing.Size(100, 20)
        Me.lblContractType.TabIndex = 0
        '
        'cboContractType
        '
        Me.cboContractType.Items.AddRange(New Object() {"リース", "非リース"})
        Me.cboContractType.Location = New System.Drawing.Point(153, 3)
        Me.cboContractType.Name = "cboContractType"
        Me.cboContractType.Size = New System.Drawing.Size(1, 20)
        Me.cboContractType.TabIndex = 1
        '
        'lblAssetBreakdown
        '
        Me.lblAssetBreakdown.Location = New System.Drawing.Point(103, 0)
        Me.lblAssetBreakdown.Name = "lblAssetBreakdown"
        Me.lblAssetBreakdown.Size = New System.Drawing.Size(100, 20)
        Me.lblAssetBreakdown.TabIndex = 2
        '
        'cboAssetBreakdown
        '
        Me.cboAssetBreakdown.Items.AddRange(New Object() {"土地建物普通賃借", "土地建物定期賃借", "車両リース", "その他施設利用"})
        Me.cboAssetBreakdown.Location = New System.Drawing.Point(253, 3)
        Me.cboAssetBreakdown.Name = "cboAssetBreakdown"
        Me.cboAssetBreakdown.Size = New System.Drawing.Size(1, 20)
        Me.cboAssetBreakdown.TabIndex = 3
        '
        'lblMgmtDept
        '
        Me.lblMgmtDept.Location = New System.Drawing.Point(3, 20)
        Me.lblMgmtDept.Name = "lblMgmtDept"
        Me.lblMgmtDept.Size = New System.Drawing.Size(100, 20)
        Me.lblMgmtDept.TabIndex = 4
        '
        'cboMgmtDept
        '
        Me.cboMgmtDept.Items.AddRange(New Object() {"渋谷本社"})
        Me.cboMgmtDept.Location = New System.Drawing.Point(153, 23)
        Me.cboMgmtDept.Name = "cboMgmtDept"
        Me.cboMgmtDept.Size = New System.Drawing.Size(1, 20)
        Me.cboMgmtDept.TabIndex = 5
        '
        'lblCostDept
        '
        Me.lblCostDept.Location = New System.Drawing.Point(103, 20)
        Me.lblCostDept.Name = "lblCostDept"
        Me.lblCostDept.Size = New System.Drawing.Size(100, 20)
        Me.lblCostDept.TabIndex = 6
        '
        'cboCostDept
        '
        Me.cboCostDept.Items.AddRange(New Object() {"配賦"})
        Me.cboCostDept.Location = New System.Drawing.Point(253, 23)
        Me.cboCostDept.Name = "cboCostDept"
        Me.cboCostDept.Size = New System.Drawing.Size(1, 20)
        Me.cboCostDept.TabIndex = 7
        '
        'lblGroup
        '
        Me.lblGroup.Location = New System.Drawing.Point(3, 40)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(100, 20)
        Me.lblGroup.TabIndex = 8
        '
        'cboGroup
        '
        Me.cboGroup.Location = New System.Drawing.Point(153, 43)
        Me.cboGroup.Name = "cboGroup"
        Me.cboGroup.Size = New System.Drawing.Size(1, 20)
        Me.cboGroup.TabIndex = 9
        '
        'lblContractNo
        '
        Me.lblContractNo.Location = New System.Drawing.Point(103, 40)
        Me.lblContractNo.Name = "lblContractNo"
        Me.lblContractNo.Size = New System.Drawing.Size(100, 20)
        Me.lblContractNo.TabIndex = 10
        '
        'txtContractNo
        '
        Me.txtContractNo.Location = New System.Drawing.Point(253, 43)
        Me.txtContractNo.Name = "txtContractNo"
        Me.txtContractNo.Size = New System.Drawing.Size(1, 19)
        Me.txtContractNo.TabIndex = 11
        '
        'lblContractName
        '
        Me.lblContractName.Location = New System.Drawing.Point(3, 60)
        Me.lblContractName.Name = "lblContractName"
        Me.lblContractName.Size = New System.Drawing.Size(100, 20)
        Me.lblContractName.TabIndex = 12
        '
        'txtContractName
        '
        Me.tlpHeader.SetColumnSpan(Me.txtContractName, 3)
        Me.txtContractName.Location = New System.Drawing.Point(153, 63)
        Me.txtContractName.Name = "txtContractName"
        Me.txtContractName.Size = New System.Drawing.Size(44, 19)
        Me.txtContractName.TabIndex = 13
        '
        'grpAsset
        '
        Me.tlpHeader.SetColumnSpan(Me.grpAsset, 4)
        Me.grpAsset.Controls.Add(Me.tlpAsset)
        Me.grpAsset.Location = New System.Drawing.Point(3, 83)
        Me.grpAsset.Name = "grpAsset"
        Me.grpAsset.Size = New System.Drawing.Size(194, 14)
        Me.grpAsset.TabIndex = 14
        Me.grpAsset.TabStop = False
        '
        'tlpAsset
        '
        Me.tlpAsset.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tlpAsset.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAsset.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tlpAsset.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAsset.Controls.Add(Me.lblPropName)
        Me.tlpAsset.Controls.Add(Me.txtPropName)
        Me.tlpAsset.Controls.Add(Me.lblKuakku)
        Me.tlpAsset.Controls.Add(Me.txtKuakku)
        Me.tlpAsset.Controls.Add(Me.lblAddress)
        Me.tlpAsset.Controls.Add(Me.txtAddress)
        Me.tlpAsset.Controls.Add(Me.lblMadori)
        Me.tlpAsset.Controls.Add(Me.txtMadori)
        Me.tlpAsset.Controls.Add(Me.lblShunko)
        Me.tlpAsset.Controls.Add(Me.dtpShunko)
        Me.tlpAsset.Controls.Add(Me.lblKibo)
        Me.tlpAsset.Controls.Add(Me.txtKibo)
        Me.tlpAsset.Controls.Add(Me.lblYoto)
        Me.tlpAsset.Controls.Add(Me.cboYoto)
        Me.tlpAsset.Controls.Add(Me.lblTaiyo)
        Me.tlpAsset.Controls.Add(Me.numTaiyo)
        Me.tlpAsset.Controls.Add(Me.lblChikun)
        Me.tlpAsset.Controls.Add(Me.numChikun)
        Me.tlpAsset.Controls.Add(Me.lblKinsi)
        Me.tlpAsset.Controls.Add(Me.txtKinsi)
        Me.tlpAsset.Location = New System.Drawing.Point(0, 0)
        Me.tlpAsset.Name = "tlpAsset"
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpAsset.Size = New System.Drawing.Size(200, 100)
        Me.tlpAsset.TabIndex = 0
        '
        'lblPropName
        '
        Me.lblPropName.Location = New System.Drawing.Point(3, 0)
        Me.lblPropName.Name = "lblPropName"
        Me.lblPropName.Size = New System.Drawing.Size(100, 20)
        Me.lblPropName.TabIndex = 0
        '
        'txtPropName
        '
        Me.txtPropName.Location = New System.Drawing.Point(3, 23)
        Me.txtPropName.Name = "txtPropName"
        Me.txtPropName.Size = New System.Drawing.Size(100, 19)
        Me.txtPropName.TabIndex = 1
        '
        'lblKuakku
        '
        Me.lblKuakku.Location = New System.Drawing.Point(3, 40)
        Me.lblKuakku.Name = "lblKuakku"
        Me.lblKuakku.Size = New System.Drawing.Size(100, 20)
        Me.lblKuakku.TabIndex = 2
        '
        'txtKuakku
        '
        Me.txtKuakku.Location = New System.Drawing.Point(3, 63)
        Me.txtKuakku.Name = "txtKuakku"
        Me.txtKuakku.Size = New System.Drawing.Size(100, 19)
        Me.txtKuakku.TabIndex = 3
        '
        'lblAddress
        '
        Me.lblAddress.Location = New System.Drawing.Point(3, 80)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(100, 20)
        Me.lblAddress.TabIndex = 4
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(3, 103)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(100, 19)
        Me.txtAddress.TabIndex = 5
        '
        'lblMadori
        '
        Me.lblMadori.Location = New System.Drawing.Point(3, 120)
        Me.lblMadori.Name = "lblMadori"
        Me.lblMadori.Size = New System.Drawing.Size(100, 20)
        Me.lblMadori.TabIndex = 6
        '
        'txtMadori
        '
        Me.txtMadori.Location = New System.Drawing.Point(3, 143)
        Me.txtMadori.Name = "txtMadori"
        Me.txtMadori.Size = New System.Drawing.Size(100, 19)
        Me.txtMadori.TabIndex = 7
        '
        'lblShunko
        '
        Me.lblShunko.Location = New System.Drawing.Point(3, 160)
        Me.lblShunko.Name = "lblShunko"
        Me.lblShunko.Size = New System.Drawing.Size(100, 20)
        Me.lblShunko.TabIndex = 8
        '
        'dtpShunko
        '
        Me.dtpShunko.Location = New System.Drawing.Point(3, 183)
        Me.dtpShunko.Name = "dtpShunko"
        Me.dtpShunko.Size = New System.Drawing.Size(194, 19)
        Me.dtpShunko.TabIndex = 9
        '
        'lblKibo
        '
        Me.lblKibo.Location = New System.Drawing.Point(3, 200)
        Me.lblKibo.Name = "lblKibo"
        Me.lblKibo.Size = New System.Drawing.Size(100, 20)
        Me.lblKibo.TabIndex = 10
        '
        'txtKibo
        '
        Me.txtKibo.Location = New System.Drawing.Point(3, 223)
        Me.txtKibo.Name = "txtKibo"
        Me.txtKibo.Size = New System.Drawing.Size(100, 19)
        Me.txtKibo.TabIndex = 11
        '
        'lblYoto
        '
        Me.lblYoto.Location = New System.Drawing.Point(3, 240)
        Me.lblYoto.Name = "lblYoto"
        Me.lblYoto.Size = New System.Drawing.Size(100, 20)
        Me.lblYoto.TabIndex = 12
        '
        'cboYoto
        '
        Me.cboYoto.Location = New System.Drawing.Point(3, 263)
        Me.cboYoto.Name = "cboYoto"
        Me.cboYoto.Size = New System.Drawing.Size(121, 20)
        Me.cboYoto.TabIndex = 13
        '
        'lblTaiyo
        '
        Me.lblTaiyo.Location = New System.Drawing.Point(3, 280)
        Me.lblTaiyo.Name = "lblTaiyo"
        Me.lblTaiyo.Size = New System.Drawing.Size(100, 20)
        Me.lblTaiyo.TabIndex = 14
        '
        'numTaiyo
        '
        Me.numTaiyo.Location = New System.Drawing.Point(3, 303)
        Me.numTaiyo.Name = "numTaiyo"
        Me.numTaiyo.Size = New System.Drawing.Size(120, 19)
        Me.numTaiyo.TabIndex = 15
        '
        'lblChikun
        '
        Me.lblChikun.Location = New System.Drawing.Point(3, 320)
        Me.lblChikun.Name = "lblChikun"
        Me.lblChikun.Size = New System.Drawing.Size(100, 20)
        Me.lblChikun.TabIndex = 16
        '
        'numChikun
        '
        Me.numChikun.Location = New System.Drawing.Point(3, 343)
        Me.numChikun.Name = "numChikun"
        Me.numChikun.Size = New System.Drawing.Size(120, 19)
        Me.numChikun.TabIndex = 17
        '
        'lblKinsi
        '
        Me.lblKinsi.Location = New System.Drawing.Point(3, 360)
        Me.lblKinsi.Name = "lblKinsi"
        Me.lblKinsi.Size = New System.Drawing.Size(100, 20)
        Me.lblKinsi.TabIndex = 18
        '
        'txtKinsi
        '
        Me.txtKinsi.Location = New System.Drawing.Point(3, 383)
        Me.txtKinsi.Name = "txtKinsi"
        Me.txtKinsi.Size = New System.Drawing.Size(100, 19)
        Me.txtKinsi.TabIndex = 19
        '
        'grpParties
        '
        Me.tlpHeader.SetColumnSpan(Me.grpParties, 4)
        Me.grpParties.Controls.Add(Me.tlpParties)
        Me.grpParties.Location = New System.Drawing.Point(3, 103)
        Me.grpParties.Name = "grpParties"
        Me.grpParties.Size = New System.Drawing.Size(194, 14)
        Me.grpParties.TabIndex = 15
        Me.grpParties.TabStop = False
        '
        'tlpParties
        '
        Me.tlpParties.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tlpParties.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpParties.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tlpParties.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpParties.Controls.Add(Me.lblLessorAddr)
        Me.tlpParties.Controls.Add(Me.txtLessorAddr)
        Me.tlpParties.Controls.Add(Me.lblLessorName)
        Me.tlpParties.Controls.Add(Me.txtLessorName)
        Me.tlpParties.Controls.Add(Me.lblLessorAcct)
        Me.tlpParties.Controls.Add(Me.txtLessorAcct)
        Me.tlpParties.Controls.Add(Me.lblAgentAddr)
        Me.tlpParties.Controls.Add(Me.txtAgentAddr)
        Me.tlpParties.Controls.Add(Me.lblAgentName)
        Me.tlpParties.Controls.Add(Me.txtAgentName)
        Me.tlpParties.Controls.Add(Me.lblAgentAcct)
        Me.tlpParties.Controls.Add(Me.txtAgentAcct)
        Me.tlpParties.Controls.Add(Me.lblLesseeAddr)
        Me.tlpParties.Controls.Add(Me.txtLesseeAddr)
        Me.tlpParties.Controls.Add(Me.lblLesseeName)
        Me.tlpParties.Controls.Add(Me.txtLesseeName)
        Me.tlpParties.Controls.Add(Me.lblLesseeAcct)
        Me.tlpParties.Controls.Add(Me.txtLesseeAcct)
        Me.tlpParties.Controls.Add(Me.lblGuarantor1)
        Me.tlpParties.Controls.Add(Me.txtGuarantor1)
        Me.tlpParties.Controls.Add(Me.lblGuarantor2)
        Me.tlpParties.Controls.Add(Me.txtGuarantor2)
        Me.tlpParties.Location = New System.Drawing.Point(0, 0)
        Me.tlpParties.Name = "tlpParties"
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpParties.Size = New System.Drawing.Size(200, 100)
        Me.tlpParties.TabIndex = 0
        '
        'lblLessorAddr
        '
        Me.lblLessorAddr.Location = New System.Drawing.Point(3, 0)
        Me.lblLessorAddr.Name = "lblLessorAddr"
        Me.lblLessorAddr.Size = New System.Drawing.Size(100, 20)
        Me.lblLessorAddr.TabIndex = 0
        '
        'txtLessorAddr
        '
        Me.txtLessorAddr.Location = New System.Drawing.Point(3, 23)
        Me.txtLessorAddr.Name = "txtLessorAddr"
        Me.txtLessorAddr.Size = New System.Drawing.Size(100, 19)
        Me.txtLessorAddr.TabIndex = 1
        '
        'lblLessorName
        '
        Me.lblLessorName.Location = New System.Drawing.Point(3, 40)
        Me.lblLessorName.Name = "lblLessorName"
        Me.lblLessorName.Size = New System.Drawing.Size(100, 20)
        Me.lblLessorName.TabIndex = 2
        '
        'txtLessorName
        '
        Me.txtLessorName.Location = New System.Drawing.Point(3, 63)
        Me.txtLessorName.Name = "txtLessorName"
        Me.txtLessorName.Size = New System.Drawing.Size(100, 19)
        Me.txtLessorName.TabIndex = 3
        '
        'lblLessorAcct
        '
        Me.lblLessorAcct.Location = New System.Drawing.Point(3, 80)
        Me.lblLessorAcct.Name = "lblLessorAcct"
        Me.lblLessorAcct.Size = New System.Drawing.Size(100, 20)
        Me.lblLessorAcct.TabIndex = 4
        '
        'txtLessorAcct
        '
        Me.txtLessorAcct.Location = New System.Drawing.Point(3, 103)
        Me.txtLessorAcct.Name = "txtLessorAcct"
        Me.txtLessorAcct.Size = New System.Drawing.Size(100, 19)
        Me.txtLessorAcct.TabIndex = 5
        '
        'lblAgentAddr
        '
        Me.lblAgentAddr.Location = New System.Drawing.Point(3, 120)
        Me.lblAgentAddr.Name = "lblAgentAddr"
        Me.lblAgentAddr.Size = New System.Drawing.Size(100, 20)
        Me.lblAgentAddr.TabIndex = 6
        '
        'txtAgentAddr
        '
        Me.txtAgentAddr.Location = New System.Drawing.Point(3, 143)
        Me.txtAgentAddr.Name = "txtAgentAddr"
        Me.txtAgentAddr.Size = New System.Drawing.Size(100, 19)
        Me.txtAgentAddr.TabIndex = 7
        '
        'lblAgentName
        '
        Me.lblAgentName.Location = New System.Drawing.Point(3, 160)
        Me.lblAgentName.Name = "lblAgentName"
        Me.lblAgentName.Size = New System.Drawing.Size(100, 20)
        Me.lblAgentName.TabIndex = 8
        '
        'txtAgentName
        '
        Me.txtAgentName.Location = New System.Drawing.Point(3, 183)
        Me.txtAgentName.Name = "txtAgentName"
        Me.txtAgentName.Size = New System.Drawing.Size(100, 19)
        Me.txtAgentName.TabIndex = 9
        '
        'lblAgentAcct
        '
        Me.lblAgentAcct.Location = New System.Drawing.Point(3, 200)
        Me.lblAgentAcct.Name = "lblAgentAcct"
        Me.lblAgentAcct.Size = New System.Drawing.Size(100, 20)
        Me.lblAgentAcct.TabIndex = 10
        '
        'txtAgentAcct
        '
        Me.txtAgentAcct.Location = New System.Drawing.Point(3, 223)
        Me.txtAgentAcct.Name = "txtAgentAcct"
        Me.txtAgentAcct.Size = New System.Drawing.Size(100, 19)
        Me.txtAgentAcct.TabIndex = 11
        '
        'lblLesseeAddr
        '
        Me.lblLesseeAddr.Location = New System.Drawing.Point(3, 240)
        Me.lblLesseeAddr.Name = "lblLesseeAddr"
        Me.lblLesseeAddr.Size = New System.Drawing.Size(100, 20)
        Me.lblLesseeAddr.TabIndex = 12
        '
        'txtLesseeAddr
        '
        Me.txtLesseeAddr.Location = New System.Drawing.Point(3, 263)
        Me.txtLesseeAddr.Name = "txtLesseeAddr"
        Me.txtLesseeAddr.Size = New System.Drawing.Size(100, 19)
        Me.txtLesseeAddr.TabIndex = 13
        '
        'lblLesseeName
        '
        Me.lblLesseeName.Location = New System.Drawing.Point(3, 280)
        Me.lblLesseeName.Name = "lblLesseeName"
        Me.lblLesseeName.Size = New System.Drawing.Size(100, 20)
        Me.lblLesseeName.TabIndex = 14
        '
        'txtLesseeName
        '
        Me.txtLesseeName.Location = New System.Drawing.Point(3, 303)
        Me.txtLesseeName.Name = "txtLesseeName"
        Me.txtLesseeName.Size = New System.Drawing.Size(100, 19)
        Me.txtLesseeName.TabIndex = 15
        '
        'lblLesseeAcct
        '
        Me.lblLesseeAcct.Location = New System.Drawing.Point(3, 320)
        Me.lblLesseeAcct.Name = "lblLesseeAcct"
        Me.lblLesseeAcct.Size = New System.Drawing.Size(100, 20)
        Me.lblLesseeAcct.TabIndex = 16
        '
        'txtLesseeAcct
        '
        Me.txtLesseeAcct.Location = New System.Drawing.Point(3, 343)
        Me.txtLesseeAcct.Name = "txtLesseeAcct"
        Me.txtLesseeAcct.Size = New System.Drawing.Size(100, 19)
        Me.txtLesseeAcct.TabIndex = 17
        '
        'lblGuarantor1
        '
        Me.lblGuarantor1.Location = New System.Drawing.Point(3, 360)
        Me.lblGuarantor1.Name = "lblGuarantor1"
        Me.lblGuarantor1.Size = New System.Drawing.Size(100, 20)
        Me.lblGuarantor1.TabIndex = 18
        '
        'txtGuarantor1
        '
        Me.txtGuarantor1.Location = New System.Drawing.Point(3, 383)
        Me.txtGuarantor1.Name = "txtGuarantor1"
        Me.txtGuarantor1.Size = New System.Drawing.Size(100, 19)
        Me.txtGuarantor1.TabIndex = 19
        '
        'lblGuarantor2
        '
        Me.lblGuarantor2.Location = New System.Drawing.Point(3, 400)
        Me.lblGuarantor2.Name = "lblGuarantor2"
        Me.lblGuarantor2.Size = New System.Drawing.Size(100, 20)
        Me.lblGuarantor2.TabIndex = 20
        '
        'txtGuarantor2
        '
        Me.txtGuarantor2.Location = New System.Drawing.Point(3, 423)
        Me.txtGuarantor2.Name = "txtGuarantor2"
        Me.txtGuarantor2.Size = New System.Drawing.Size(100, 19)
        Me.txtGuarantor2.TabIndex = 21
        '
        'tabTerms
        '
        Me.tabTerms.Controls.Add(Me.tlpTerms)
        Me.tabTerms.Location = New System.Drawing.Point(4, 22)
        Me.tabTerms.Name = "tabTerms"
        Me.tabTerms.Size = New System.Drawing.Size(192, 12)
        Me.tabTerms.TabIndex = 1
        Me.tabTerms.Text = "契約条項"
        '
        'tlpTerms
        '
        Me.tlpTerms.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpTerms.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTerms.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpTerms.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTerms.Controls.Add(Me.lblStart)
        Me.tlpTerms.Controls.Add(Me.dtpStart)
        Me.tlpTerms.Controls.Add(Me.lblPeriod)
        Me.tlpTerms.Controls.Add(Me.numPeriod)
        Me.tlpTerms.Controls.Add(Me.lblEnd)
        Me.tlpTerms.Controls.Add(Me.dtpEnd)
        Me.tlpTerms.Controls.Add(Me.lblMushou)
        Me.tlpTerms.Controls.Add(Me.numMushou)
        Me.tlpTerms.Controls.Add(Me.lblNcn)
        Me.tlpTerms.Controls.Add(Me.numNonCancelable)
        Me.tlpTerms.Controls.Add(Me.lblNotice)
        Me.tlpTerms.Controls.Add(Me.dtpNotice)
        Me.tlpTerms.Controls.Add(Me.lblFirstPay)
        Me.tlpTerms.Controls.Add(Me.dtpFirstPay)
        Me.tlpTerms.Controls.Add(Me.lblApplyPeriod)
        Me.tlpTerms.Controls.Add(Me.cboApplyPeriod)
        Me.tlpTerms.Controls.Add(Me.lblPayInterval)
        Me.tlpTerms.Controls.Add(Me.numPayInterval)
        Me.tlpTerms.Controls.Add(Me.lblPayCount)
        Me.tlpTerms.Controls.Add(Me.numPayCount)
        Me.tlpTerms.Controls.Add(Me.lblLastPay)
        Me.tlpTerms.Controls.Add(Me.dtpLastPay)
        Me.tlpTerms.Controls.Add(Me.lblLastApply)
        Me.tlpTerms.Controls.Add(Me.dtpLastApply)
        Me.tlpTerms.Location = New System.Drawing.Point(0, 0)
        Me.tlpTerms.Name = "tlpTerms"
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpTerms.Size = New System.Drawing.Size(200, 100)
        Me.tlpTerms.TabIndex = 0
        '
        'lblStart
        '
        Me.lblStart.Location = New System.Drawing.Point(3, 0)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(100, 20)
        Me.lblStart.TabIndex = 0
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(3, 23)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(194, 19)
        Me.dtpStart.TabIndex = 1
        '
        'lblPeriod
        '
        Me.lblPeriod.Location = New System.Drawing.Point(3, 40)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(100, 20)
        Me.lblPeriod.TabIndex = 2
        '
        'numPeriod
        '
        Me.numPeriod.Location = New System.Drawing.Point(3, 63)
        Me.numPeriod.Name = "numPeriod"
        Me.numPeriod.Size = New System.Drawing.Size(120, 19)
        Me.numPeriod.TabIndex = 3
        '
        'lblEnd
        '
        Me.lblEnd.Location = New System.Drawing.Point(3, 80)
        Me.lblEnd.Name = "lblEnd"
        Me.lblEnd.Size = New System.Drawing.Size(100, 20)
        Me.lblEnd.TabIndex = 4
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(3, 103)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(194, 19)
        Me.dtpEnd.TabIndex = 5
        '
        'lblMushou
        '
        Me.lblMushou.Location = New System.Drawing.Point(3, 120)
        Me.lblMushou.Name = "lblMushou"
        Me.lblMushou.Size = New System.Drawing.Size(100, 20)
        Me.lblMushou.TabIndex = 6
        '
        'numMushou
        '
        Me.numMushou.Location = New System.Drawing.Point(3, 143)
        Me.numMushou.Name = "numMushou"
        Me.numMushou.Size = New System.Drawing.Size(120, 19)
        Me.numMushou.TabIndex = 7
        '
        'lblNcn
        '
        Me.lblNcn.Location = New System.Drawing.Point(3, 160)
        Me.lblNcn.Name = "lblNcn"
        Me.lblNcn.Size = New System.Drawing.Size(100, 20)
        Me.lblNcn.TabIndex = 8
        '
        'numNonCancelable
        '
        Me.numNonCancelable.Location = New System.Drawing.Point(3, 183)
        Me.numNonCancelable.Name = "numNonCancelable"
        Me.numNonCancelable.Size = New System.Drawing.Size(120, 19)
        Me.numNonCancelable.TabIndex = 9
        '
        'lblNotice
        '
        Me.lblNotice.Location = New System.Drawing.Point(3, 200)
        Me.lblNotice.Name = "lblNotice"
        Me.lblNotice.Size = New System.Drawing.Size(100, 20)
        Me.lblNotice.TabIndex = 10
        '
        'dtpNotice
        '
        Me.dtpNotice.Location = New System.Drawing.Point(3, 223)
        Me.dtpNotice.Name = "dtpNotice"
        Me.dtpNotice.Size = New System.Drawing.Size(194, 19)
        Me.dtpNotice.TabIndex = 11
        '
        'lblFirstPay
        '
        Me.lblFirstPay.Location = New System.Drawing.Point(3, 240)
        Me.lblFirstPay.Name = "lblFirstPay"
        Me.lblFirstPay.Size = New System.Drawing.Size(100, 20)
        Me.lblFirstPay.TabIndex = 12
        '
        'dtpFirstPay
        '
        Me.dtpFirstPay.Location = New System.Drawing.Point(3, 263)
        Me.dtpFirstPay.Name = "dtpFirstPay"
        Me.dtpFirstPay.Size = New System.Drawing.Size(194, 19)
        Me.dtpFirstPay.TabIndex = 13
        '
        'lblApplyPeriod
        '
        Me.lblApplyPeriod.Location = New System.Drawing.Point(3, 280)
        Me.lblApplyPeriod.Name = "lblApplyPeriod"
        Me.lblApplyPeriod.Size = New System.Drawing.Size(100, 20)
        Me.lblApplyPeriod.TabIndex = 14
        '
        'cboApplyPeriod
        '
        Me.cboApplyPeriod.Items.AddRange(New Object() {"当月分", "翌月分", "8月分"})
        Me.cboApplyPeriod.Location = New System.Drawing.Point(3, 303)
        Me.cboApplyPeriod.Name = "cboApplyPeriod"
        Me.cboApplyPeriod.Size = New System.Drawing.Size(121, 20)
        Me.cboApplyPeriod.TabIndex = 15
        '
        'lblPayInterval
        '
        Me.lblPayInterval.Location = New System.Drawing.Point(3, 320)
        Me.lblPayInterval.Name = "lblPayInterval"
        Me.lblPayInterval.Size = New System.Drawing.Size(100, 20)
        Me.lblPayInterval.TabIndex = 16
        '
        'numPayInterval
        '
        Me.numPayInterval.Location = New System.Drawing.Point(3, 343)
        Me.numPayInterval.Name = "numPayInterval"
        Me.numPayInterval.Size = New System.Drawing.Size(120, 19)
        Me.numPayInterval.TabIndex = 17
        '
        'lblPayCount
        '
        Me.lblPayCount.Location = New System.Drawing.Point(3, 360)
        Me.lblPayCount.Name = "lblPayCount"
        Me.lblPayCount.Size = New System.Drawing.Size(100, 20)
        Me.lblPayCount.TabIndex = 18
        '
        'numPayCount
        '
        Me.numPayCount.Location = New System.Drawing.Point(3, 383)
        Me.numPayCount.Name = "numPayCount"
        Me.numPayCount.Size = New System.Drawing.Size(120, 19)
        Me.numPayCount.TabIndex = 19
        '
        'lblLastPay
        '
        Me.lblLastPay.Location = New System.Drawing.Point(3, 400)
        Me.lblLastPay.Name = "lblLastPay"
        Me.lblLastPay.Size = New System.Drawing.Size(100, 20)
        Me.lblLastPay.TabIndex = 20
        '
        'dtpLastPay
        '
        Me.dtpLastPay.Location = New System.Drawing.Point(3, 423)
        Me.dtpLastPay.Name = "dtpLastPay"
        Me.dtpLastPay.Size = New System.Drawing.Size(194, 19)
        Me.dtpLastPay.TabIndex = 21
        '
        'lblLastApply
        '
        Me.lblLastApply.Location = New System.Drawing.Point(3, 440)
        Me.lblLastApply.Name = "lblLastApply"
        Me.lblLastApply.Size = New System.Drawing.Size(100, 20)
        Me.lblLastApply.TabIndex = 22
        '
        'dtpLastApply
        '
        Me.dtpLastApply.Location = New System.Drawing.Point(3, 463)
        Me.dtpLastApply.Name = "dtpLastApply"
        Me.dtpLastApply.Size = New System.Drawing.Size(194, 19)
        Me.dtpLastApply.TabIndex = 23
        '
        'tabPayment
        '
        Me.tabPayment.Controls.Add(Me.tlpPayment)
        Me.tabPayment.Location = New System.Drawing.Point(4, 22)
        Me.tabPayment.Name = "tabPayment"
        Me.tabPayment.Size = New System.Drawing.Size(192, 12)
        Me.tabPayment.TabIndex = 2
        Me.tabPayment.Text = "約定支払"
        '
        'tlpPayment
        '
        Me.tlpPayment.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpPayment.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpPayment.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpPayment.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpPayment.Controls.Add(Me.lblPay1)
        Me.tlpPayment.Controls.Add(Me.numPay1)
        Me.tlpPayment.Controls.Add(Me.lblTax)
        Me.tlpPayment.Controls.Add(Me.numTax)
        Me.tlpPayment.Controls.Add(Me.lblTaxIncl)
        Me.tlpPayment.Controls.Add(Me.numTaxIncl)
        Me.tlpPayment.Controls.Add(Me.lblAccountDetail)
        Me.tlpPayment.Controls.Add(Me.cboAccountDetail)
        Me.tlpPayment.Controls.Add(Me.lblBankAcct)
        Me.tlpPayment.Controls.Add(Me.cboBankAcct)
        Me.tlpPayment.Controls.Add(Me.lblAcctTotal)
        Me.tlpPayment.Controls.Add(Me.numAcctTotal)
        Me.tlpPayment.Controls.Add(Me.lblIndex)
        Me.tlpPayment.Controls.Add(Me.cboIndex)
        Me.tlpPayment.Controls.Add(Me.lblIndexAtContract)
        Me.tlpPayment.Controls.Add(Me.txtIndexAtContract)
        Me.tlpPayment.Controls.Add(Me.lblRentTotal)
        Me.tlpPayment.Controls.Add(Me.numRentTotal)
        Me.tlpPayment.Controls.Add(Me.lblTaxSum)
        Me.tlpPayment.Controls.Add(Me.numTaxSum)
        Me.tlpPayment.Controls.Add(Me.lblGrossRent)
        Me.tlpPayment.Controls.Add(Me.numGrossRent)
        Me.tlpPayment.Controls.Add(Me.lblMaintIn)
        Me.tlpPayment.Controls.Add(Me.numMaintIn)
        Me.tlpPayment.Controls.Add(Me.lblResidualGuarantee)
        Me.tlpPayment.Controls.Add(Me.numResidualGuarantee)
        Me.tlpPayment.Controls.Add(Me.chkResidualGuarantee)
        Me.tlpPayment.Controls.Add(Me.lblExpectedPay)
        Me.tlpPayment.Controls.Add(Me.numExpectedPay)
        Me.tlpPayment.Controls.Add(Me.lblLeaseFeeTotal)
        Me.tlpPayment.Controls.Add(Me.numLeaseFeeTotal)
        Me.tlpPayment.Location = New System.Drawing.Point(0, 0)
        Me.tlpPayment.Name = "tlpPayment"
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpPayment.Size = New System.Drawing.Size(200, 100)
        Me.tlpPayment.TabIndex = 0
        '
        'lblPay1
        '
        Me.lblPay1.Location = New System.Drawing.Point(3, 0)
        Me.lblPay1.Name = "lblPay1"
        Me.lblPay1.Size = New System.Drawing.Size(100, 20)
        Me.lblPay1.TabIndex = 0
        '
        'numPay1
        '
        Me.numPay1.Location = New System.Drawing.Point(3, 23)
        Me.numPay1.Name = "numPay1"
        Me.numPay1.Size = New System.Drawing.Size(120, 19)
        Me.numPay1.TabIndex = 1
        '
        'lblTax
        '
        Me.lblTax.Location = New System.Drawing.Point(3, 40)
        Me.lblTax.Name = "lblTax"
        Me.lblTax.Size = New System.Drawing.Size(100, 20)
        Me.lblTax.TabIndex = 2
        '
        'numTax
        '
        Me.numTax.Location = New System.Drawing.Point(3, 63)
        Me.numTax.Name = "numTax"
        Me.numTax.Size = New System.Drawing.Size(120, 19)
        Me.numTax.TabIndex = 3
        '
        'lblTaxIncl
        '
        Me.lblTaxIncl.Location = New System.Drawing.Point(3, 80)
        Me.lblTaxIncl.Name = "lblTaxIncl"
        Me.lblTaxIncl.Size = New System.Drawing.Size(100, 20)
        Me.lblTaxIncl.TabIndex = 4
        '
        'numTaxIncl
        '
        Me.numTaxIncl.Location = New System.Drawing.Point(3, 103)
        Me.numTaxIncl.Name = "numTaxIncl"
        Me.numTaxIncl.Size = New System.Drawing.Size(120, 19)
        Me.numTaxIncl.TabIndex = 5
        '
        'lblAccountDetail
        '
        Me.lblAccountDetail.Location = New System.Drawing.Point(3, 120)
        Me.lblAccountDetail.Name = "lblAccountDetail"
        Me.lblAccountDetail.Size = New System.Drawing.Size(100, 20)
        Me.lblAccountDetail.TabIndex = 6
        '
        'cboAccountDetail
        '
        Me.cboAccountDetail.Location = New System.Drawing.Point(3, 143)
        Me.cboAccountDetail.Name = "cboAccountDetail"
        Me.cboAccountDetail.Size = New System.Drawing.Size(121, 20)
        Me.cboAccountDetail.TabIndex = 7
        '
        'lblBankAcct
        '
        Me.lblBankAcct.Location = New System.Drawing.Point(3, 160)
        Me.lblBankAcct.Name = "lblBankAcct"
        Me.lblBankAcct.Size = New System.Drawing.Size(100, 20)
        Me.lblBankAcct.TabIndex = 8
        '
        'cboBankAcct
        '
        Me.cboBankAcct.Location = New System.Drawing.Point(3, 183)
        Me.cboBankAcct.Name = "cboBankAcct"
        Me.cboBankAcct.Size = New System.Drawing.Size(121, 20)
        Me.cboBankAcct.TabIndex = 9
        '
        'lblAcctTotal
        '
        Me.lblAcctTotal.Location = New System.Drawing.Point(3, 200)
        Me.lblAcctTotal.Name = "lblAcctTotal"
        Me.lblAcctTotal.Size = New System.Drawing.Size(100, 20)
        Me.lblAcctTotal.TabIndex = 10
        '
        'numAcctTotal
        '
        Me.numAcctTotal.Location = New System.Drawing.Point(3, 223)
        Me.numAcctTotal.Name = "numAcctTotal"
        Me.numAcctTotal.Size = New System.Drawing.Size(120, 19)
        Me.numAcctTotal.TabIndex = 11
        '
        'lblIndex
        '
        Me.lblIndex.Location = New System.Drawing.Point(3, 240)
        Me.lblIndex.Name = "lblIndex"
        Me.lblIndex.Size = New System.Drawing.Size(100, 20)
        Me.lblIndex.TabIndex = 12
        '
        'cboIndex
        '
        Me.cboIndex.Items.AddRange(New Object() {"CPI"})
        Me.cboIndex.Location = New System.Drawing.Point(3, 263)
        Me.cboIndex.Name = "cboIndex"
        Me.cboIndex.Size = New System.Drawing.Size(121, 20)
        Me.cboIndex.TabIndex = 13
        '
        'lblIndexAtContract
        '
        Me.lblIndexAtContract.Location = New System.Drawing.Point(3, 280)
        Me.lblIndexAtContract.Name = "lblIndexAtContract"
        Me.lblIndexAtContract.Size = New System.Drawing.Size(100, 20)
        Me.lblIndexAtContract.TabIndex = 14
        '
        'txtIndexAtContract
        '
        Me.txtIndexAtContract.Location = New System.Drawing.Point(3, 303)
        Me.txtIndexAtContract.Name = "txtIndexAtContract"
        Me.txtIndexAtContract.Size = New System.Drawing.Size(100, 19)
        Me.txtIndexAtContract.TabIndex = 15
        '
        'lblRentTotal
        '
        Me.lblRentTotal.Location = New System.Drawing.Point(3, 320)
        Me.lblRentTotal.Name = "lblRentTotal"
        Me.lblRentTotal.Size = New System.Drawing.Size(100, 20)
        Me.lblRentTotal.TabIndex = 16
        '
        'numRentTotal
        '
        Me.numRentTotal.Location = New System.Drawing.Point(3, 343)
        Me.numRentTotal.Name = "numRentTotal"
        Me.numRentTotal.Size = New System.Drawing.Size(120, 19)
        Me.numRentTotal.TabIndex = 17
        '
        'lblTaxSum
        '
        Me.lblTaxSum.Location = New System.Drawing.Point(3, 360)
        Me.lblTaxSum.Name = "lblTaxSum"
        Me.lblTaxSum.Size = New System.Drawing.Size(100, 20)
        Me.lblTaxSum.TabIndex = 18
        '
        'numTaxSum
        '
        Me.numTaxSum.Location = New System.Drawing.Point(3, 383)
        Me.numTaxSum.Name = "numTaxSum"
        Me.numTaxSum.Size = New System.Drawing.Size(120, 19)
        Me.numTaxSum.TabIndex = 19
        '
        'lblGrossRent
        '
        Me.lblGrossRent.Location = New System.Drawing.Point(3, 400)
        Me.lblGrossRent.Name = "lblGrossRent"
        Me.lblGrossRent.Size = New System.Drawing.Size(100, 20)
        Me.lblGrossRent.TabIndex = 20
        '
        'numGrossRent
        '
        Me.numGrossRent.Location = New System.Drawing.Point(3, 423)
        Me.numGrossRent.Name = "numGrossRent"
        Me.numGrossRent.Size = New System.Drawing.Size(120, 19)
        Me.numGrossRent.TabIndex = 21
        '
        'lblMaintIn
        '
        Me.lblMaintIn.Location = New System.Drawing.Point(3, 440)
        Me.lblMaintIn.Name = "lblMaintIn"
        Me.lblMaintIn.Size = New System.Drawing.Size(100, 20)
        Me.lblMaintIn.TabIndex = 22
        '
        'numMaintIn
        '
        Me.numMaintIn.Location = New System.Drawing.Point(3, 463)
        Me.numMaintIn.Name = "numMaintIn"
        Me.numMaintIn.Size = New System.Drawing.Size(120, 19)
        Me.numMaintIn.TabIndex = 23
        '
        'lblResidualGuarantee
        '
        Me.lblResidualGuarantee.Location = New System.Drawing.Point(3, 480)
        Me.lblResidualGuarantee.Name = "lblResidualGuarantee"
        Me.lblResidualGuarantee.Size = New System.Drawing.Size(100, 20)
        Me.lblResidualGuarantee.TabIndex = 24
        '
        'numResidualGuarantee
        '
        Me.numResidualGuarantee.Location = New System.Drawing.Point(3, 503)
        Me.numResidualGuarantee.Name = "numResidualGuarantee"
        Me.numResidualGuarantee.Size = New System.Drawing.Size(120, 19)
        Me.numResidualGuarantee.TabIndex = 25
        '
        'chkResidualGuarantee
        '
        Me.chkResidualGuarantee.Location = New System.Drawing.Point(3, 523)
        Me.chkResidualGuarantee.Name = "chkResidualGuarantee"
        Me.chkResidualGuarantee.Size = New System.Drawing.Size(104, 14)
        Me.chkResidualGuarantee.TabIndex = 26
        '
        'lblExpectedPay
        '
        Me.lblExpectedPay.Location = New System.Drawing.Point(3, 560)
        Me.lblExpectedPay.Name = "lblExpectedPay"
        Me.lblExpectedPay.Size = New System.Drawing.Size(100, 20)
        Me.lblExpectedPay.TabIndex = 28
        '
        'numExpectedPay
        '
        Me.numExpectedPay.Location = New System.Drawing.Point(3, 583)
        Me.numExpectedPay.Name = "numExpectedPay"
        Me.numExpectedPay.Size = New System.Drawing.Size(120, 19)
        Me.numExpectedPay.TabIndex = 29
        '
        'lblLeaseFeeTotal
        '
        Me.lblLeaseFeeTotal.Location = New System.Drawing.Point(3, 600)
        Me.lblLeaseFeeTotal.Name = "lblLeaseFeeTotal"
        Me.lblLeaseFeeTotal.Size = New System.Drawing.Size(100, 20)
        Me.lblLeaseFeeTotal.TabIndex = 30
        '
        'numLeaseFeeTotal
        '
        Me.numLeaseFeeTotal.Location = New System.Drawing.Point(3, 623)
        Me.numLeaseFeeTotal.Name = "numLeaseFeeTotal"
        Me.numLeaseFeeTotal.Size = New System.Drawing.Size(120, 19)
        Me.numLeaseFeeTotal.TabIndex = 31
        '
        'tabInitial
        '
        Me.tabInitial.Controls.Add(Me.dgvInitial)
        Me.tabInitial.Location = New System.Drawing.Point(4, 22)
        Me.tabInitial.Name = "tabInitial"
        Me.tabInitial.Size = New System.Drawing.Size(192, 12)
        Me.tabInitial.TabIndex = 3
        Me.tabInitial.Text = "初回一時金"
        '
        'dgvInitial
        '
        Me.dgvInitial.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7, Me.DataGridViewTextBoxColumn8, Me.DataGridViewTextBoxColumn9, Me.DataGridViewTextBoxColumn10, Me.DataGridViewTextBoxColumn11, Me.DataGridViewTextBoxColumn12})
        Me.dgvInitial.Location = New System.Drawing.Point(0, 0)
        Me.dgvInitial.Name = "dgvInitial"
        Me.dgvInitial.Size = New System.Drawing.Size(240, 150)
        Me.dgvInitial.TabIndex = 0
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me.splitOptions)
        Me.tabOptions.Location = New System.Drawing.Point(4, 22)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.Size = New System.Drawing.Size(192, 12)
        Me.tabOptions.TabIndex = 4
        Me.tabOptions.Text = "オプション"
        '
        'splitOptions
        '
        Me.splitOptions.Location = New System.Drawing.Point(0, 0)
        Me.splitOptions.Name = "splitOptions"
        '
        'splitOptions.Panel1
        '
        Me.splitOptions.Panel1.Controls.Add(Me.grpExtend)
        '
        'splitOptions.Panel2
        '
        Me.splitOptions.Panel2.Controls.Add(Me.grpCancel)
        Me.splitOptions.Size = New System.Drawing.Size(150, 100)
        Me.splitOptions.TabIndex = 0
        '
        'grpExtend
        '
        Me.grpExtend.Controls.Add(Me.tlpExtend)
        Me.grpExtend.Location = New System.Drawing.Point(0, 0)
        Me.grpExtend.Name = "grpExtend"
        Me.grpExtend.Size = New System.Drawing.Size(200, 100)
        Me.grpExtend.TabIndex = 0
        Me.grpExtend.TabStop = False
        '
        'tlpExtend
        '
        Me.tlpExtend.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tlpExtend.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpExtend.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tlpExtend.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpExtend.Controls.Add(Me.chkExtendRule, 0, 0)
        Me.tlpExtend.Controls.Add(Me.lblExtendCond1, 0, 1)
        Me.tlpExtend.Controls.Add(Me.txtExtendCond1, 1, 1)
        Me.tlpExtend.Controls.Add(Me.lblExtendCond2, 2, 1)
        Me.tlpExtend.Controls.Add(Me.txtExtendCond2, 3, 1)
        Me.tlpExtend.Controls.Add(Me.lblExtendStart, 0, 2)
        Me.tlpExtend.Controls.Add(Me.dtpExtendStart, 1, 2)
        Me.tlpExtend.Controls.Add(Me.lblExtendPeriod, 2, 2)
        Me.tlpExtend.Controls.Add(Me.numExtendPeriod, 3, 2)
        Me.tlpExtend.Controls.Add(Me.lblExtendEnd, 0, 3)
        Me.tlpExtend.Controls.Add(Me.dtpExtendEnd, 1, 3)
        Me.tlpExtend.Controls.Add(Me.lblExtendMushou, 2, 3)
        Me.tlpExtend.Controls.Add(Me.numExtendMushou, 3, 3)
        Me.tlpExtend.Controls.Add(Me.lblExtendNonCancelable, 0, 4)
        Me.tlpExtend.Controls.Add(Me.numExtendNonCancelable, 1, 4)
        Me.tlpExtend.Controls.Add(Me.lblExtendNotice, 2, 4)
        Me.tlpExtend.Controls.Add(Me.dtpExtendNotice, 3, 4)
        Me.tlpExtend.Controls.Add(Me.lblExtendCount, 0, 5)
        Me.tlpExtend.Controls.Add(Me.numExtendCount, 1, 5)
        Me.tlpExtend.Location = New System.Drawing.Point(0, 0)
        Me.tlpExtend.Name = "tlpExtend"
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpExtend.Size = New System.Drawing.Size(200, 100)
        Me.tlpExtend.TabIndex = 0
        '
        'chkExtendRule
        '
        Me.tlpExtend.SetColumnSpan(Me.chkExtendRule, 4)
        Me.chkExtendRule.Location = New System.Drawing.Point(3, 3)
        Me.chkExtendRule.Name = "chkExtendRule"
        Me.chkExtendRule.Size = New System.Drawing.Size(104, 14)
        Me.chkExtendRule.TabIndex = 0
        '
        'lblExtendCond1
        '
        Me.lblExtendCond1.Location = New System.Drawing.Point(3, 20)
        Me.lblExtendCond1.Name = "lblExtendCond1"
        Me.lblExtendCond1.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendCond1.TabIndex = 1
        '
        'txtExtendCond1
        '
        Me.txtExtendCond1.Location = New System.Drawing.Point(163, 23)
        Me.txtExtendCond1.Name = "txtExtendCond1"
        Me.txtExtendCond1.Size = New System.Drawing.Size(1, 19)
        Me.txtExtendCond1.TabIndex = 2
        '
        'lblExtendCond2
        '
        Me.lblExtendCond2.Location = New System.Drawing.Point(103, 20)
        Me.lblExtendCond2.Name = "lblExtendCond2"
        Me.lblExtendCond2.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendCond2.TabIndex = 3
        '
        'txtExtendCond2
        '
        Me.txtExtendCond2.Location = New System.Drawing.Point(263, 23)
        Me.txtExtendCond2.Name = "txtExtendCond2"
        Me.txtExtendCond2.Size = New System.Drawing.Size(1, 19)
        Me.txtExtendCond2.TabIndex = 4
        '
        'lblExtendStart
        '
        Me.lblExtendStart.Location = New System.Drawing.Point(3, 40)
        Me.lblExtendStart.Name = "lblExtendStart"
        Me.lblExtendStart.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendStart.TabIndex = 5
        '
        'dtpExtendStart
        '
        Me.dtpExtendStart.Location = New System.Drawing.Point(163, 43)
        Me.dtpExtendStart.Name = "dtpExtendStart"
        Me.dtpExtendStart.Size = New System.Drawing.Size(1, 19)
        Me.dtpExtendStart.TabIndex = 6
        '
        'lblExtendPeriod
        '
        Me.lblExtendPeriod.Location = New System.Drawing.Point(103, 40)
        Me.lblExtendPeriod.Name = "lblExtendPeriod"
        Me.lblExtendPeriod.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendPeriod.TabIndex = 7
        '
        'numExtendPeriod
        '
        Me.numExtendPeriod.Location = New System.Drawing.Point(263, 43)
        Me.numExtendPeriod.Name = "numExtendPeriod"
        Me.numExtendPeriod.Size = New System.Drawing.Size(1, 19)
        Me.numExtendPeriod.TabIndex = 8
        '
        'lblExtendEnd
        '
        Me.lblExtendEnd.Location = New System.Drawing.Point(3, 60)
        Me.lblExtendEnd.Name = "lblExtendEnd"
        Me.lblExtendEnd.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendEnd.TabIndex = 9
        '
        'dtpExtendEnd
        '
        Me.dtpExtendEnd.Location = New System.Drawing.Point(163, 63)
        Me.dtpExtendEnd.Name = "dtpExtendEnd"
        Me.dtpExtendEnd.Size = New System.Drawing.Size(1, 19)
        Me.dtpExtendEnd.TabIndex = 10
        '
        'lblExtendMushou
        '
        Me.lblExtendMushou.Location = New System.Drawing.Point(103, 60)
        Me.lblExtendMushou.Name = "lblExtendMushou"
        Me.lblExtendMushou.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendMushou.TabIndex = 11
        '
        'numExtendMushou
        '
        Me.numExtendMushou.Location = New System.Drawing.Point(263, 63)
        Me.numExtendMushou.Name = "numExtendMushou"
        Me.numExtendMushou.Size = New System.Drawing.Size(1, 19)
        Me.numExtendMushou.TabIndex = 12
        '
        'lblExtendNonCancelable
        '
        Me.lblExtendNonCancelable.Location = New System.Drawing.Point(3, 80)
        Me.lblExtendNonCancelable.Name = "lblExtendNonCancelable"
        Me.lblExtendNonCancelable.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendNonCancelable.TabIndex = 13
        '
        'numExtendNonCancelable
        '
        Me.numExtendNonCancelable.Location = New System.Drawing.Point(163, 83)
        Me.numExtendNonCancelable.Name = "numExtendNonCancelable"
        Me.numExtendNonCancelable.Size = New System.Drawing.Size(1, 19)
        Me.numExtendNonCancelable.TabIndex = 14
        '
        'lblExtendNotice
        '
        Me.lblExtendNotice.Location = New System.Drawing.Point(103, 80)
        Me.lblExtendNotice.Name = "lblExtendNotice"
        Me.lblExtendNotice.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendNotice.TabIndex = 15
        '
        'dtpExtendNotice
        '
        Me.dtpExtendNotice.Location = New System.Drawing.Point(263, 83)
        Me.dtpExtendNotice.Name = "dtpExtendNotice"
        Me.dtpExtendNotice.Size = New System.Drawing.Size(1, 19)
        Me.dtpExtendNotice.TabIndex = 16
        '
        'lblExtendCount
        '
        Me.lblExtendCount.Location = New System.Drawing.Point(3, 100)
        Me.lblExtendCount.Name = "lblExtendCount"
        Me.lblExtendCount.Size = New System.Drawing.Size(100, 20)
        Me.lblExtendCount.TabIndex = 17
        '
        'numExtendCount
        '
        Me.numExtendCount.Location = New System.Drawing.Point(163, 103)
        Me.numExtendCount.Name = "numExtendCount"
        Me.numExtendCount.Size = New System.Drawing.Size(1, 19)
        Me.numExtendCount.TabIndex = 18
        '
        'grpCancel
        '
        Me.grpCancel.Controls.Add(Me.tlpCancel)
        Me.grpCancel.Location = New System.Drawing.Point(0, 0)
        Me.grpCancel.Name = "grpCancel"
        Me.grpCancel.Size = New System.Drawing.Size(200, 100)
        Me.grpCancel.TabIndex = 0
        Me.grpCancel.TabStop = False
        '
        'tlpCancel
        '
        Me.tlpCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tlpCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tlpCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCancel.Controls.Add(Me.chkCancelRule, 0, 0)
        Me.tlpCancel.Controls.Add(Me.lblCancelCond1, 0, 1)
        Me.tlpCancel.Controls.Add(Me.txtCancelCond1, 1, 1)
        Me.tlpCancel.Controls.Add(Me.lblCancelCond2, 2, 1)
        Me.tlpCancel.Controls.Add(Me.txtCancelCond2, 3, 1)
        Me.tlpCancel.Controls.Add(Me.lblSaleProcAmt, 0, 2)
        Me.tlpCancel.Controls.Add(Me.numSaleProcAmt, 1, 2)
        Me.tlpCancel.Location = New System.Drawing.Point(0, 0)
        Me.tlpCancel.Name = "tlpCancel"
        Me.tlpCancel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpCancel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpCancel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpCancel.Size = New System.Drawing.Size(200, 100)
        Me.tlpCancel.TabIndex = 0
        '
        'chkCancelRule
        '
        Me.tlpCancel.SetColumnSpan(Me.chkCancelRule, 4)
        Me.chkCancelRule.Location = New System.Drawing.Point(3, 3)
        Me.chkCancelRule.Name = "chkCancelRule"
        Me.chkCancelRule.Size = New System.Drawing.Size(104, 14)
        Me.chkCancelRule.TabIndex = 0
        '
        'lblCancelCond1
        '
        Me.lblCancelCond1.Location = New System.Drawing.Point(3, 20)
        Me.lblCancelCond1.Name = "lblCancelCond1"
        Me.lblCancelCond1.Size = New System.Drawing.Size(100, 20)
        Me.lblCancelCond1.TabIndex = 1
        '
        'txtCancelCond1
        '
        Me.txtCancelCond1.Location = New System.Drawing.Point(163, 23)
        Me.txtCancelCond1.Name = "txtCancelCond1"
        Me.txtCancelCond1.Size = New System.Drawing.Size(1, 19)
        Me.txtCancelCond1.TabIndex = 2
        '
        'lblCancelCond2
        '
        Me.lblCancelCond2.Location = New System.Drawing.Point(103, 20)
        Me.lblCancelCond2.Name = "lblCancelCond2"
        Me.lblCancelCond2.Size = New System.Drawing.Size(100, 20)
        Me.lblCancelCond2.TabIndex = 3
        '
        'txtCancelCond2
        '
        Me.txtCancelCond2.Location = New System.Drawing.Point(263, 23)
        Me.txtCancelCond2.Name = "txtCancelCond2"
        Me.txtCancelCond2.Size = New System.Drawing.Size(1, 19)
        Me.txtCancelCond2.TabIndex = 4
        '
        'lblSaleProcAmt
        '
        Me.lblSaleProcAmt.Location = New System.Drawing.Point(3, 40)
        Me.lblSaleProcAmt.Name = "lblSaleProcAmt"
        Me.lblSaleProcAmt.Size = New System.Drawing.Size(100, 23)
        Me.lblSaleProcAmt.TabIndex = 5
        '
        'numSaleProcAmt
        '
        Me.numSaleProcAmt.Location = New System.Drawing.Point(163, 43)
        Me.numSaleProcAmt.Name = "numSaleProcAmt"
        Me.numSaleProcAmt.Size = New System.Drawing.Size(1, 19)
        Me.numSaleProcAmt.TabIndex = 6
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        '
        'btnPanel
        '
        Me.btnPanel.Controls.Add(Me.btnClose)
        Me.btnPanel.Controls.Add(Me.btnUpdate)
        Me.btnPanel.Controls.Add(Me.btnEdit)
        Me.btnPanel.Controls.Add(Me.btnDelete)
        Me.btnPanel.Controls.Add(Me.btnRegister)
        Me.btnPanel.Location = New System.Drawing.Point(3, 47)
        Me.btnPanel.Name = "btnPanel"
        Me.btnPanel.Size = New System.Drawing.Size(194, 50)
        Me.btnPanel.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(3, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 0
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(84, 3)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 1
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(3, 32)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 23)
        Me.btnEdit.TabIndex = 2
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(84, 32)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 3
        '
        'btnRegister
        '
        Me.btnRegister.Location = New System.Drawing.Point(3, 61)
        Me.btnRegister.Name = "btnRegister"
        Me.btnRegister.Size = New System.Drawing.Size(75, 23)
        Me.btnRegister.TabIndex = 4
        '
        'ContractEntryForm
        '
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.root)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "ContractEntryForm"
        Me.root.ResumeLayout(False)
        Me.tab.ResumeLayout(False)
        Me.tabHeader.ResumeLayout(False)
        Me.tlpHeader.ResumeLayout(False)
        Me.tlpHeader.PerformLayout()
        Me.grpAsset.ResumeLayout(False)
        Me.tlpAsset.ResumeLayout(False)
        Me.tlpAsset.PerformLayout()
        CType(Me.numTaiyo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numChikun, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpParties.ResumeLayout(False)
        Me.tlpParties.ResumeLayout(False)
        Me.tlpParties.PerformLayout()
        Me.tabTerms.ResumeLayout(False)
        Me.tlpTerms.ResumeLayout(False)
        CType(Me.numPeriod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numMushou, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numNonCancelable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numPayInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numPayCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPayment.ResumeLayout(False)
        Me.tlpPayment.ResumeLayout(False)
        Me.tlpPayment.PerformLayout()
        CType(Me.numPay1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTaxIncl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numAcctTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRentTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTaxSum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numGrossRent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numMaintIn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numResidualGuarantee, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numExpectedPay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numLeaseFeeTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabInitial.ResumeLayout(False)
        CType(Me.dgvInitial, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabOptions.ResumeLayout(False)
        Me.splitOptions.Panel1.ResumeLayout(False)
        Me.splitOptions.Panel2.ResumeLayout(False)
        CType(Me.splitOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitOptions.ResumeLayout(False)
        Me.grpExtend.ResumeLayout(False)
        Me.tlpExtend.ResumeLayout(False)
        Me.tlpExtend.PerformLayout()
        CType(Me.numExtendPeriod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numExtendMushou, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numExtendNonCancelable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numExtendCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCancel.ResumeLayout(False)
        Me.tlpCancel.ResumeLayout(False)
        Me.tlpCancel.PerformLayout()
        CType(Me.numSaleProcAmt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.btnPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

End Class
