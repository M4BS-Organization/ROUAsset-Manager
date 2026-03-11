Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

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
    Private _masterData As MasterDataLoader

    ''' <summary>
    ''' 新規登録時に外部から設定される契約番号（自動採番値）
    ''' </summary>
    Public Property InitContractNo As String = ""


    Private pnlHeader As Panel
    Private pnlBody As Panel
    Private tabMain As TabControl
    Private pgContract As TabPage
    Private pgInitial As TabPage
    Private pgAccounting As TabPage
    Private pgSublease As TabPage
    Private pgJudgment As TabPage

    Private cmbContractType As ComboBox
    Private txtContractNo As TextBox
    Private cmbMgmtDeptCode As ComboBox
    Private txtMgmtDeptName As TextBox
    Private txtContractName As TextBox
    Private cmbSupplier As ComboBox
    Private txtSupplierName As TextBox
    Private cmbAssetCategory As ComboBox
    Private txtAssetNo As TextBox
    Private btnAssetSearch As Button
    Private btnAssetNew As Button
    Private dgvAssets As DataGridView
    Private lblAssetCount As Label
    Private btnDeleteRow As Button

    Private _contractTypeTable As Data.DataTable
    Private _supplierTable As Data.DataTable
    Private _deptTable As Data.DataTable

    Private dtpStartDate As DateTimePicker
    Private dtpEndDate As DateTimePicker
    Private numFreePeriod As NumericUpDown
    Private lblLeaseMonths As Label

    Private dgvInitialCosts As DataGridView
    Private numInitialDirectCost As NumericUpDown
    Private numRestorationCost As NumericUpDown
    Private numLeaseIncentive As NumericUpDown

    ' === 会計タブ: 現契約期間 ===
    Private txtSchContractDate As TextBox
    Private txtSchStartDate As TextBox
    Private txtSchContractPeriod As TextBox
    Private txtSchEndDate As TextBox

    ' === 会計タブ: 現支払情報 ===
    Private txtSchFirstPayDate As TextBox
    Private txtSchPayInterval As TextBox
    Private txtSchPayCount As TextBox
    Private txtSchFreePeriod As TextBox
    Private txtSchLastPayDate As TextBox

    ' === 会計タブ: 会計期間・金額 ===
    Private txtSchRenewalForecastCount As TextBox
    Private txtSchAccStartDate As TextBox
    Private txtSchAccPeriod As TextBox
    Private txtSchAccEndDate As TextBox
    Private txtSchRent As TextBox
    Private txtSchRenewalRent As TextBox
    Private txtSchRenewalPayDate As TextBox
    Private txtSchDiscountRate As TextBox
    Private txtSchRentTotal As TextBox
    Private txtSchCalcTotal As TextBox
    Private txtSchLeaseRatio As TextBox
    Private txtSchNonLeaseRatio As TextBox
    Private txtSchAllocTotal As TextBox
    Private txtSchMaintenanceCost As TextBox
    Private txtSchDistributionRate As TextBox

    ' === 会計タブ: 返済スケジュールマトリックス ===
    Private txtSchPresentValue As TextBox
    Private txtSchRouBegin As TextBox
    Private txtSchRouIncrease As TextBox
    Private txtSchRouChange As TextBox
    Private txtSchRouDecrease As TextBox
    Private txtSchRouEnd As TextBox
    Private txtSchLiabBegin As TextBox
    Private txtSchLiabIncrease As TextBox
    Private txtSchLiabChange As TextBox
    Private txtSchLiabDecrease As TextBox
    Private txtSchLiabEnd As TextBox
    Private txtSchAroBegin As TextBox
    Private txtSchAroIncrease As TextBox
    Private txtSchAroChange As TextBox
    Private txtSchAroDecrease As TextBox
    Private txtSchAroEnd As TextBox

    ' === 会計タブ: 変更履歴 ===
    Private dgvChangeHistory As DataGridView

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

    Private grpQ1 As GroupBox, rbQ1Yes As RadioButton, rbQ1No As RadioButton
    Private grpQ2 As GroupBox, rbQ2Yes As RadioButton, rbQ2No As RadioButton
    Private grpQ3 As GroupBox, rbQ3Yes As RadioButton, rbQ3No As RadioButton
    Private grpQ4 As GroupBox, rbQ4Yes As RadioButton, rbQ4No As RadioButton
    Private dtpJudgeStart As DateTimePicker
    Private dtpJudgeEnd As DateTimePicker
    Private lblTermMonths As Label
    Private lblDateError As Label
    Private chkExtOption As CheckBox
    Private cboExtCertainty As ComboBox
    Private numExtMonths As NumericUpDown
    Private chkTerminateOption As CheckBox
    Private cboTerminateCertainty As ComboBox
    Private lblShortTermResult As Label
    Private numAssetValue As NumericUpDown
    Private lblLowValueResult As Label
    Private chkApplyExemption As CheckBox
    Private chkServiceComponent As CheckBox
    Private chkOwnershipTransfer As CheckBox
    Private numDiscountRate As NumericUpDown
    Private numMonthlyRentJudge As NumericUpDown
    Private lblResultText As Label
    Private lblResultBadge As Label
    Private lblResultReason As Label
    Private Const LOW_VALUE_THRESHOLD As Decimal = 3000000D
    Private _boldFont As Font
    Private _regularFont As Font
    Private _font11Bold As Font
    Private _font18Bold As Font
    Private _fontResultBadge As Font
    Private _fontResultReason As Font
    Private _prevExemptEligible As Boolean = False

    ''' <summary>
    ''' 資産番号 → CTBレコード(種別固有データ含む)のマッピング
    ''' </summary>
    Private ReadOnly _assetCtbMap As New Dictionary(Of String, CtbRecord)

    ' === 判定タブ: 契約詳細・関係者情報 (grpParty) ===
    Private txtLenderName As TextBox
    Private txtLenderAddr As TextBox
    Private txtLenderAccount As TextBox
    Private txtAgentName As TextBox
    Private txtAgentAddr As TextBox
    Private txtBrokerName As TextBox
    Private txtBrokerAddr As TextBox
    Private txtBrokerAccount As TextBox
    Private txtBorrowerName As TextBox
    Private txtBorrowerAddr As TextBox
    Private txtBorrowerAccount As TextBox
    Private txtGuarantor1 As TextBox
    Private txtGuarantor2 As TextBox
    Private dtpPartyStartDate As DateTimePicker
    Private dtpPartyEndDate As DateTimePicker
    Private numPartyContractMonths As NumericUpDown
    Private txtNonCancelPeriod As TextBox
    Private cboCancelOption As ComboBox
    Private cboExtendOption As ComboBox
    Private numRenewalCount As NumericUpDown

    ' === 判定タブ: 会計判定結果 (pnlResult) ===
    Private lblTotalPayment As Label
    Private lblTotalCount As Label
    Private lblRouAsset As Label
    Private lblLeaseLiab As Label
    Private lblDeposit As Label
    Private lblPrepaidRent As Label
    Private lblEquipmentCost As Label
    Private txtResultDiscountRate As TextBox
    Private lblPresentValue As Label

    Private lblJudgmentPreview As Label
    Private btnRegister As Button

    ''' <summary>
    ''' 登録ボタン押下時に発火するイベント。契約一覧画面へデータを渡す。
    ''' </summary>
    Public Event ContractRegistered As EventHandler(Of ContractRegisteredEventArgs)

    ''' <summary>
    ''' 契約登録イベントの引数
    ''' </summary>
    Public Class ContractRegisteredEventArgs
        Inherits EventArgs
        Public Property MgmtDept As String
        Public Property ContractType As String
        Public Property ContractNo As String
        Public Property ContractName As String
        Public Property StartDate As String
        Public Property EndDate As String
        Public Property ContractPeriod As String
        Public Property AssetQty As String
    End Class

    Public Sub New()
        Me.Text = "新リース会計対応 リース契約管理"
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

        _boldFont = New Font("Meiryo", 9.0F, FontStyle.Bold)
        _regularFont = New Font("Meiryo", 9.0F, FontStyle.Regular)
        _font11Bold = New Font("Meiryo", 11.0F, FontStyle.Bold)
        _font18Bold = New Font("Meiryo", 18.0F, FontStyle.Bold)
        _fontResultBadge = New Font("Meiryo", 9.0F, FontStyle.Bold)
        _fontResultReason = New Font("Meiryo", 9.0F)

        _masterData = New MasterDataLoader()

        BuildUI()

        _isLoaded = True
        RecalcAll()

        AddHandler Me.Shown, Sub(s, ev) ApplyInitialValues()
    End Sub

    ''' <summary>
    ''' 外部から設定された初期値（自動採番値）をコントロールに反映する。
    ''' 新規登録モード時のみ値が設定される。編集・照会モードでは空文字のため上書きしない。
    ''' </summary>
    Private Sub ApplyInitialValues()
        If Not String.IsNullOrEmpty(InitContractNo) Then
            txtContractNo.Text = InitContractNo
        End If
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
            .Height = 55,
            .BackColor = CLR_HEADER,
            .Padding = New Padding(10, 8, 10, 8)
        }

        Dim flowButtons As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.LeftToRight,
            .WrapContents = False,
            .Padding = New Padding(0, 4, 0, 0)
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
            If i = 0 Then
                btnRegister = btn
                AddHandler btn.Click, AddressOf OnRegisterClick
            End If
            flowButtons.Controls.Add(btn)
        Next

        pnlHeader.Controls.Add(flowButtons)
    End Sub

    ''' <summary>
    ''' 登録ボタンクリック時の処理
    ''' </summary>
    Private Sub OnRegisterClick(sender As Object, e As EventArgs)
        ' 必須項目チェック
        If String.IsNullOrWhiteSpace(txtContractNo.Text) Then
            MessageBox.Show("契約番号が設定されていません。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtContractName.Text) Then
            MessageBox.Show("契約名称を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContractName.Focus()
            Return
        End If

        ' リース期間（月数）を計算
        Dim leaseMonths As String = ""
        If lblLeaseMonths.Text.Contains("ヶ月") Then
            leaseMonths = lblLeaseMonths.Text.Replace("ヶ月", "").Trim()
        End If

        ' 管理部署名を取得
        Dim deptName As String = ""
        If cmbMgmtDeptCode.SelectedIndex >= 0 Then
            deptName = txtMgmtDeptName.Text
        ElseIf Not String.IsNullOrWhiteSpace(cmbMgmtDeptCode.Text) Then
            deptName = cmbMgmtDeptCode.Text
        End If

        ' 資産数量をグリッドから取得
        Dim assetCount As Integer = 0
        If dgvAssets IsNot Nothing Then
            For Each row As DataGridViewRow In dgvAssets.Rows
                If Not row.IsNewRow AndAlso
                   row.Cells("AssetNo").Value IsNot Nothing AndAlso
                   Not String.IsNullOrWhiteSpace(row.Cells("AssetNo").Value.ToString()) Then
                    assetCount += 1
                End If
            Next
        End If

        ' CTBレコード生成: 資産×配賦部門ごとに1レコード作成
        Dim assetIdx As Integer = 0
        Dim childIdx As Integer = 0
        Dim propertyCounter As Integer = 1
        Dim ctbCount As Integer = 0
        Dim dbRecords As New List(Of CtbRecord)
        Dim contractTypeCd As String = ""
        If cmbContractType.SelectedIndex >= 0 AndAlso cmbContractType.SelectedIndex < _contractTypeTable.Rows.Count Then
            contractTypeCd = _contractTypeTable.Rows(cmbContractType.SelectedIndex)("contract_type_cd").ToString()
        End If
        Dim supplierCd As String = ""
        If cmbSupplier.SelectedIndex >= 0 AndAlso cmbSupplier.SelectedIndex < _supplierTable.Rows.Count Then
            supplierCd = _supplierTable.Rows(cmbSupplier.SelectedIndex)("supplier_cd").ToString()
        End If
        Dim mgmtDeptCd As String = ""
        If cmbMgmtDeptCode.SelectedIndex >= 0 AndAlso cmbMgmtDeptCode.SelectedIndex < _deptTable.Rows.Count Then
            mgmtDeptCd = _deptTable.Rows(cmbMgmtDeptCode.SelectedIndex)("dept_cd").ToString()
        End If
        Dim freeMonths As Integer = CInt(numFreePeriod.Value)
        Dim termMonthsVal As Integer = 0
        Integer.TryParse(leaseMonths, termMonthsVal)

        If dgvAssets IsNot Nothing Then
            For Each row As DataGridViewRow In dgvAssets.Rows
                If row.IsNewRow Then Continue For
                Dim assetNoVal As String = If(row.Cells("AssetNo").Value?.ToString(), "")
                If String.IsNullOrWhiteSpace(assetNoVal) Then Continue For

                ' _assetCtbMapに保持済みの詳細データがあればそれを使う
                Dim baseCtb As CtbRecord = Nothing
                If _assetCtbMap.ContainsKey(assetNoVal) Then
                    baseCtb = _assetCtbMap(assetNoVal)
                Else
                    baseCtb = New CtbRecord()
                    baseCtb.AssetNo = assetNoVal
                    baseCtb.AssetCategory = If(row.Cells("AssetCategory").Value?.ToString(), "")
                    baseCtb.AssetName = If(row.Cells("AssetName").Value?.ToString(), "")
                    baseCtb.InstallLocation = If(row.Cells("InstallLocation").Value?.ToString(), "")
                End If

                ' 配賦部門リストを取得
                Dim allocations As List(Of CtbDeptAllocation) = baseCtb.DeptAllocations
                If allocations Is Nothing OrElse allocations.Count = 0 Then
                    ' 配賦情報なしの場合は1レコードだけ作成
                    allocations = New List(Of CtbDeptAllocation)
                    allocations.Add(New CtbDeptAllocation() With {.DeptName = "", .AllocationRatio = 100D})
                End If

                ' 配賦部門ごとに1レコード生成
                assetIdx += 1
                childIdx = 0
                For Each alloc As CtbDeptAllocation In allocations
                    childIdx += 1

                    Dim ctb As New CtbRecord()
                    ctb.ContractNo = txtContractNo.Text
                    ctb.PropertyNo = propertyCounter
                    propertyCounter += 1
                    ctb.ContractName = txtContractName.Text
                    ctb.ContractTypeCd = contractTypeCd
                    ctb.SupplierCd = supplierCd
                    ctb.MgmtDeptCd = mgmtDeptCd
                    ctb.LeaseStartDate = dtpStartDate.Value
                    ctb.LeaseEndDate = dtpEndDate.Value
                    ctb.FreeRentMonths = freeMonths
                    If termMonthsVal > 0 Then ctb.LeaseTermMonths = termMonthsVal

                    ' 資産情報コピー
                    ctb.AssetNo = baseCtb.AssetNo
                    ctb.AssetCategory = baseCtb.AssetCategory
                    ctb.AssetName = baseCtb.AssetName
                    ctb.CompanyName = baseCtb.CompanyName
                    ctb.InstallLocation = baseCtb.InstallLocation
                    ctb.Remarks = baseCtb.Remarks

                    ' 種別固有コピー
                    ctb.ReStructure = baseCtb.ReStructure
                    ctb.ReArea = baseCtb.ReArea
                    ctb.ReLayout = baseCtb.ReLayout
                    ctb.ReCompletionDate = baseCtb.ReCompletionDate
                    ctb.ReLandlordName = baseCtb.ReLandlordName
                    ctb.ReBrokerCompany = baseCtb.ReBrokerCompany
                    ctb.ReUsageRestrictions = baseCtb.ReUsageRestrictions
                    ctb.VhChassisNo = baseCtb.VhChassisNo
                    ctb.VhRegistrationNo = baseCtb.VhRegistrationNo
                    ctb.VhVehicleType = baseCtb.VhVehicleType
                    ctb.VhInspectionDate = baseCtb.VhInspectionDate
                    ctb.VhMileageLimit = baseCtb.VhMileageLimit
                    ctb.OaModelNo = baseCtb.OaModelNo
                    ctb.OaSerialNo = baseCtb.OaSerialNo
                    ctb.OaMaintenanceDate = baseCtb.OaMaintenanceDate
                    ctb.OaMaintenanceContract = baseCtb.OaMaintenanceContract

                    ' 配賦情報（1レコードに1部門）
                    ctb.DeptCd = alloc.DeptCd
                    ctb.DeptName = alloc.DeptName
                    ctb.AllocationRatio = alloc.AllocationRatio

                    CtbDataStore.Instance.Add(ctb)
                    dbRecords.Add(ctb)
                    ctbCount += 1
                Next
            Next
        End If

        ' DB永続化
        Try
            Dim repo As New CtbRepository()
            repo.InsertAll(dbRecords)
        Catch ex As Exception
            MessageBox.Show("DB登録でエラーが発生しました。メモリには保持されています。" & Environment.NewLine & ex.Message,
                            "DB登録エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        Dim args As New ContractRegisteredEventArgs() With {
            .MgmtDept = If(Not String.IsNullOrEmpty(deptName), deptName, cmbMgmtDeptCode.Text),
            .ContractType = contractTypeCd,
            .ContractNo = txtContractNo.Text,
            .ContractName = txtContractName.Text,
            .StartDate = dtpStartDate.Value.ToString("yyyy/MM/dd"),
            .EndDate = dtpEndDate.Value.ToString("yyyy/MM/dd"),
            .ContractPeriod = leaseMonths,
            .AssetQty = assetCount.ToString()
        }

        RaiseEvent ContractRegistered(Me, args)

        MessageBox.Show("契約を登録しました。（CTBレコード: " & ctbCount.ToString() & "件）",
                        "登録完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close()
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
        InitTabJudge_Pro()

        tabMain.TabPages.AddRange({pgContract, pgInitial, pgAccounting, pgSublease, pgJudgment})
        AddHandler tabMain.SelectedIndexChanged, Sub(s, e)
            If tabMain.SelectedTab Is pgAccounting Then UpdateAccountingTabValues()
        End Sub

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
            .ColumnCount = 8, .Padding = New Padding(8)
        }
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblBasic.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))

        txtContractNo = New TextBox() With {
            .Dock = DockStyle.Fill, .ReadOnly = True,
            .BackColor = CLR_READONLY, .Text = "LC-2025-0001"
        }
        _tooltipProvider.SetToolTip(txtContractNo, "契約番号は自動採番されます")
        txtContractName = New TextBox() With {.Dock = DockStyle.Fill}
        cmbContractType = New ComboBox() With {
            .Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = FNT_INPUT
        }
        _contractTypeTable = _masterData.LoadContractTypesTable()
        For Each row As Data.DataRow In _contractTypeTable.Rows
            cmbContractType.Items.Add(row("contract_type_nm").ToString())
        Next
        If cmbContractType.Items.Count > 0 Then cmbContractType.SelectedIndex = 0

        Dim pnlSupplier As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1
        }
        pnlSupplier.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        pnlSupplier.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        cmbSupplier = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = FNT_INPUT}
        txtSupplierName = New TextBox() With {.Dock = DockStyle.Fill, .ReadOnly = True, .BackColor = CLR_READONLY}
        _tooltipProvider.SetToolTip(cmbSupplier, "取引先マスタから選択")

        _supplierTable = _masterData.LoadSuppliers()
        For Each row As Data.DataRow In _supplierTable.Rows
            cmbSupplier.Items.Add(row("supplier_cd").ToString() & " " & row("supplier_nm").ToString())
        Next
        AddHandler cmbSupplier.SelectedIndexChanged, Sub(s, ev)
                                                          If cmbSupplier.SelectedIndex >= 0 AndAlso
                                                             cmbSupplier.SelectedIndex < _supplierTable.Rows.Count Then
                                                              txtSupplierName.Text = _supplierTable.Rows(cmbSupplier.SelectedIndex)("supplier_nm").ToString()
                                                          End If
                                                      End Sub
        pnlSupplier.Controls.Add(cmbSupplier, 0, 0)
        pnlSupplier.Controls.Add(txtSupplierName, 1, 0)

        Dim pnlMgmtDept As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1
        }
        pnlMgmtDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        pnlMgmtDept.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        cmbMgmtDeptCode = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDown}
        txtMgmtDeptName = New TextBox() With {.Dock = DockStyle.Fill, .ReadOnly = True, .BackColor = CLR_READONLY}

        _deptTable = _masterData.LoadDepartments()
        For Each row As Data.DataRow In _deptTable.Rows
            cmbMgmtDeptCode.Items.Add(row("dept_cd").ToString() & " " & row("dept_nm").ToString())
        Next
        AddHandler cmbMgmtDeptCode.SelectedIndexChanged, Sub(s, ev)
                                                              If cmbMgmtDeptCode.SelectedIndex >= 0 AndAlso
                                                                 cmbMgmtDeptCode.SelectedIndex < _deptTable.Rows.Count Then
                                                                  txtMgmtDeptName.Text = _deptTable.Rows(cmbMgmtDeptCode.SelectedIndex)("dept_nm").ToString()
                                                              End If
                                                          End Sub
        pnlMgmtDept.Controls.Add(cmbMgmtDeptCode, 0, 0)
        pnlMgmtDept.Controls.Add(txtMgmtDeptName, 1, 0)

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

        ' Row 1: 契約番号, 契約名称
        Dim r As Integer = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("契約番号"), 0, r)
        tblBasic.Controls.Add(txtContractNo, 1, r)
        tblBasic.Controls.Add(CreateFieldLabel("契約名称"), 2, r)
        tblBasic.Controls.Add(txtContractName, 3, r)
        tblBasic.SetColumnSpan(txtContractName, 5)
        tblBasic.RowCount += 1

        ' Row 2: 契約種類, 取引先ID, 管理部署
        r = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("契約種類"), 0, r)
        tblBasic.Controls.Add(cmbContractType, 1, r)
        tblBasic.Controls.Add(CreateFieldLabel("取引先"), 2, r)
        tblBasic.Controls.Add(pnlSupplier, 3, r)
        tblBasic.Controls.Add(CreateFieldLabel("管理部署"), 4, r)
        tblBasic.Controls.Add(pnlMgmtDept, 5, r)
        tblBasic.SetColumnSpan(pnlMgmtDept, 3)
        tblBasic.RowCount += 1

        ' Row 3: 契約開始日, 契約終了日
        r = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("契約開始日"), 0, r)
        tblBasic.Controls.Add(dtpStartDate, 1, r)
        tblBasic.SetColumnSpan(dtpStartDate, 3)
        tblBasic.Controls.Add(CreateFieldLabel("契約終了日"), 4, r)
        tblBasic.Controls.Add(dtpEndDate, 5, r)
        tblBasic.SetColumnSpan(dtpEndDate, 3)
        tblBasic.RowCount += 1

        ' Row 4: 無償期間, リース期間
        r = tblBasic.RowCount
        tblBasic.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblBasic.Controls.Add(CreateFieldLabel("無償期間"), 0, r)
        tblBasic.Controls.Add(numFreePeriod, 1, r)
        tblBasic.SetColumnSpan(numFreePeriod, 3)
        tblBasic.Controls.Add(CreateFieldLabel("リース期間"), 4, r)
        tblBasic.Controls.Add(lblLeaseMonths, 5, r)
        tblBasic.SetColumnSpan(lblLeaseMonths, 3)
        tblBasic.RowCount += 1

        grpBasic.Controls.Add(tblBasic)

        Dim grpProperty As GroupBox = CreateSection("資産情報")
        Dim tblProp As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 8, .Padding = New Padding(8, 8, 8, 2)
        }
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 0: 資産種類ラベル
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))    ' 1: コンボボックス
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 2: 資産番号ラベル
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))    ' 3: テキスト
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 4: 検索ボタン
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))  ' 5: 新規登録ボタン
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))    ' 6: 余白
        tblProp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))    ' 7: 余白

        cmbAssetCategory = New ComboBox() With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = FNT_INPUT
        }
        cmbAssetCategory.Items.AddRange(New String() {"不動産", "車両", "OA機器"})
        cmbAssetCategory.SelectedIndex = 0

        txtAssetNo = New TextBox() With {.Dock = DockStyle.Fill}
        _tooltipProvider.SetToolTip(txtAssetNo, "資産番号を入力して検索、または＋新規登録で資産を作成")

        btnAssetSearch = New Button() With {
            .Text = "検索",
            .Dock = DockStyle.Fill,
            .Height = 28,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(0, 123, 255),
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand
        }
        btnAssetSearch.FlatAppearance.BorderSize = 0

        btnAssetNew = New Button() With {
            .Text = "＋新規登録",
            .Dock = DockStyle.Fill,
            .Height = 28,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = CLR_ACCENT,
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand
        }
        btnAssetNew.FlatAppearance.BorderSize = 0

        AddHandler btnAssetSearch.Click, AddressOf OnAssetSearchClick
        AddHandler btnAssetNew.Click, AddressOf OnAssetNewClick

        Dim rAsset As Integer = tblProp.RowCount
        tblProp.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblProp.Controls.Add(CreateFieldLabel("資産種類"), 0, rAsset)
        tblProp.Controls.Add(cmbAssetCategory, 1, rAsset)
        tblProp.Controls.Add(CreateFieldLabel("資産番号"), 2, rAsset)
        tblProp.Controls.Add(txtAssetNo, 3, rAsset)
        tblProp.Controls.Add(btnAssetSearch, 4, rAsset)
        tblProp.Controls.Add(btnAssetNew, 5, rAsset)
        tblProp.RowCount += 1

        btnDeleteRow = New Button() With {
            .Text = "行削除",
            .Width = 70,
            .Height = 28,
            .Anchor = AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(108, 117, 125),
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand
        }
        btnDeleteRow.FlatAppearance.BorderSize = 0
        AddHandler btnDeleteRow.Click, AddressOf OnDeleteRowClick

        lblAssetCount = New Label() With {
            .Text = "資産件数: 0件",
            .Dock = DockStyle.Fill,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Height = 28,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(0, 4, 0, 0)
        }

        Dim rAssetBar As Integer = tblProp.RowCount
        tblProp.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tblProp.Controls.Add(lblAssetCount, 0, rAssetBar)
        tblProp.SetColumnSpan(lblAssetCount, 5)
        tblProp.Controls.Add(btnDeleteRow, 5, rAssetBar)
        tblProp.SetColumnSpan(btnDeleteRow, 3)
        tblProp.RowCount += 1

        dgvAssets = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
            .ScrollBars = ScrollBars.Both,
            .AllowUserToAddRows = False,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .SelectionMode = DataGridViewSelectionMode.CellSelect,
            .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }
        dgvAssets.Columns.Add(New DataGridViewCheckBoxColumn() With {
            .HeaderText = "削除フラグ", .Name = "DeleteCheck", .Width = 60,
            .MinimumWidth = 60, .SortMode = DataGridViewColumnSortMode.NotSortable
        })
        dgvAssets.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "資産番号", .Name = "AssetNo", .Width = 120, .MinimumWidth = 80
        })
        dgvAssets.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "資産種類", .Name = "AssetCategory", .Width = 90, .MinimumWidth = 70
        })
        dgvAssets.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "資産名", .Name = "AssetName", .Width = 250, .MinimumWidth = 150
        })
        dgvAssets.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "設置場所", .Name = "InstallLocation", .Width = 150, .MinimumWidth = 80
        })
        dgvAssets.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "配賦部門", .Name = "DeptAllocation", .Width = 250, .MinimumWidth = 120
        })

        dgvAssets.AllowUserToAddRows = True
        dgvAssets.ReadOnly = False
        For i As Integer = 0 To 6
            dgvAssets.Rows.Add()
        Next

        Dim pnlGrid As New Panel()With {
            .Dock = DockStyle.Top,
            .Height = 180,
            .Padding = New Padding(8, 0, 8, 8)
        }
        pnlGrid.Controls.Add(dgvAssets)

        grpProperty.Controls.Add(pnlGrid)
        grpProperty.Controls.Add(tblProp)

        ' === 月額支払明細（会計タブから移動） ===
        Dim grpMonthly As GroupBox = CreateSection("月額支払明細")
        grpMonthly.Height = 260
        grpMonthly.AutoSize = False

        Dim pnlMonthlyGrid As New Panel() With {.Dock = DockStyle.Fill}

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
        Dim monthlyItems = _masterData.LoadMonthlyItems()
        If monthlyItems.Length = 0 Then monthlyItems = New String() {"賃料", "管理費", "共益費"}
        colMItem.Items.AddRange(monthlyItems)
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
        Dim colBankAccount As New DataGridViewComboBoxColumn() With {
            .HeaderText = "振込先口座", .Name = "MBankAccount", .FillWeight = 18
        }
        Dim bankAccts = _masterData.LoadBankAccounts()
        If bankAccts.Length = 0 Then bankAccts = New String() {"三菱UFJ銀行 本店 普通 1234567", "みずほ銀行 丸の内支店 普通 2345678"}
        colBankAccount.Items.AddRange(bankAccts)
        dgvMonthlyPayments.Columns.Add(colBankAccount)
        Dim colPayMethod As New DataGridViewComboBoxColumn() With {
            .HeaderText = "支払方法", .Name = "MPayMethod", .FillWeight = 12
        }
        Dim payMethods = _masterData.LoadPaymentMethods()
        If payMethods.Length = 0 Then payMethods = New String() {"振込", "口座振替", "手形", "現金"}
        colPayMethod.Items.AddRange(payMethods)
        dgvMonthlyPayments.Columns.Add(colPayMethod)
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "支払日", .Name = "MPayDate", .FillWeight = 10
        })

        dgvMonthlyPayments.Rows.Add("賃料", 500000, 50000, 550000, Nothing, "振込", "毎月末")
        dgvMonthlyPayments.Rows.Add("管理費", 50000, 5000, 55000, Nothing, "振込", "毎月末")
        dgvMonthlyPayments.Rows.Add("共益費", 30000, 3000, 33000, Nothing, "振込", "毎月末")

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

        pnlMonthlyGrid.Controls.Add(dgvMonthlyPayments)
        pnlMonthlyGrid.Controls.Add(pnlTotal)
        grpMonthly.Controls.Add(pnlMonthlyGrid)

        mainTbl.Controls.Add(grpBasic, 0, 0)
        mainTbl.Controls.Add(grpProperty, 0, 1)
        mainTbl.Controls.Add(grpMonthly, 0, 2)

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
        Dim costItems = _masterData.LoadInitialCostItems()
        If costItems.Length = 0 Then costItems = New String() {"敷金", "敷金償却額（返還不能分）", "礼金", "仲介手数料"}
        colItem.Items.AddRange(costItems)

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
        Dim acctItems = _masterData.LoadAcctTreatments()
        If acctItems.Length = 0 Then acctItems = New String() {"資産計上", "費用処理", "繰延処理", "預り金処理"}
        colAcctTreat.Items.AddRange(acctItems)
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
            .ColumnCount = 1, .RowCount = 3
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))  ' 現契約期間 + 現支払情報
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))  ' 会計期間・金額 + 返済スケジュールマトリックス
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))  ' 変更履歴

        mainTbl.Controls.Add(BuildAccSchTopRowSection(), 0, 0)
        mainTbl.Controls.Add(BuildAccSchAccountingSection(), 0, 1)
        mainTbl.Controls.Add(BuildAccChangeHistorySection(), 0, 2)

        scroll.Controls.Add(mainTbl)
        pgAccounting.Controls.Add(scroll)
    End Sub

    ''' <summary>＜現契約期間＞ + ＜現支払情報＞ (横並び)</summary>
    Private Function BuildAccSchTopRowSection() As Panel
        Dim pnlSchTopRow As New Panel() With {
            .Dock = DockStyle.Top, .AutoSize = True, .Padding = New Padding(0, 0, 0, 4)
        }
        Dim tblSchTopRow As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 2, .RowCount = 1
        }
        tblSchTopRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45.0F))
        tblSchTopRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 55.0F))
        tblSchTopRow.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        ' --- ＜現契約期間＞ ---
        Dim grpCurrentContract As GroupBox = CreateSection("＜現契約期間＞")
        Dim tblCC As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblCC.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblCC.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblCC.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblCC.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        txtSchContractDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchStartDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchContractPeriod = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchEndDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}

        AddFieldRow(tblCC, "契約日", txtSchContractDate, "開始日", txtSchStartDate)
        AddFieldRow(tblCC, "契約期間", txtSchContractPeriod, "終了日", txtSchEndDate)
        grpCurrentContract.Controls.Add(tblCC)

        ' --- ＜現支払情報＞ ---
        Dim grpPayInfo As GroupBox = CreateSection("＜現支払情報＞")
        Dim tblPI As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblPI.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tblPI.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblPI.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tblPI.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        txtSchFirstPayDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchPayInterval = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchPayCount = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchFreePeriod = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        txtSchLastPayDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}

        AddFieldRow(tblPI, "初回支払日", txtSchFirstPayDate, "支払間隔", txtSchPayInterval)
        AddFieldRow(tblPI, "支払回数", txtSchPayCount, "無償期間", txtSchFreePeriod)
        AddFieldRow(tblPI, "最終支払日", txtSchLastPayDate, Nothing, Nothing)
        grpPayInfo.Controls.Add(tblPI)

        tblSchTopRow.Controls.Add(grpCurrentContract, 0, 0)
        tblSchTopRow.Controls.Add(grpPayInfo, 1, 0)
        pnlSchTopRow.Controls.Add(tblSchTopRow)
        Return pnlSchTopRow
    End Function

    ''' <summary>＜会計期間＞ 統合表形式</summary>
    Private Function BuildAccSchAccountingSection() As GroupBox
        Dim grpAccounting As GroupBox = CreateSection("＜会計期間＞")
        Dim tbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 10, .Padding = New Padding(4)
        }
        ' 列幅: ラベル(100px) + 値(%) を5ペア = 10列
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))  ' 0: ラベル
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))    ' 1: 値
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 2: ラベル
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))    ' 3: 値
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 4: ラベル
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))    ' 5: 値
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 6: ラベル
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))    ' 7: 値
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))   ' 8: ラベル
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))    ' 9: 値

        ' === 1段目: 更新予想回数 | 開始日 | 会計期間 | 終了日 ===
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tbl.Controls.Add(New Label() With {
            .Text = "更新予想回数",
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(255, 235, 59),
            .ForeColor = CLR_TEXT,
            .Font = FNT_LABEL,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Margin = New Padding(2)
        }, 0, 0)
        txtSchRenewalForecastCount = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Center, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchRenewalForecastCount, 1, 0)
        tbl.Controls.Add(CreateGridLabel("開始日"), 2, 0)
        txtSchAccStartDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Center, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchAccStartDate, 3, 0)
        tbl.Controls.Add(CreateGridLabel("会計期間"), 4, 0)
        txtSchAccPeriod = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Center, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchAccPeriod, 5, 0)
        tbl.Controls.Add(CreateGridLabel("終了日"), 6, 0)
        txtSchAccEndDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Center, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchAccEndDate, 7, 0)
        ' 8-9列は空白
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 8, 0)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 9, 0)

        ' === 2段目: 賃料 (xxx /月) | 更新賃料 (xxx /月) | 更新支払日 ===
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tbl.Controls.Add(CreateGridLabel("賃料"), 0, 1)
        txtSchRent = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchRent, 1, 1)
        tbl.Controls.Add(CreateGridLabel("更新賃料"), 2, 1)
        txtSchRenewalRent = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchRenewalRent, 3, 1)
        tbl.Controls.Add(CreateGridLabel("更新支払日"), 4, 1)
        txtSchRenewalPayDate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Center, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchRenewalPayDate, 5, 1)
        ' 6-9列: 配分総額・割引率を列ヘッダーとして表示
        tbl.Controls.Add(CreateGridLabel("配分総額"), 6, 1)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 7, 1)
        tbl.Controls.Add(CreateGridLabel("割引率"), 8, 1)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 9, 1)

        ' === 3段目: 賃料総額 | 算定総額 | リース割合 | 配分総額 | 割引率 ===
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tbl.Controls.Add(CreateGridLabel("賃料総額"), 0, 2)
        txtSchRentTotal = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchRentTotal, 1, 2)
        tbl.Controls.Add(CreateGridLabel("算定総額"), 2, 2)
        txtSchCalcTotal = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchCalcTotal, 3, 2)
        tbl.Controls.Add(CreateGridLabel("リース割合"), 4, 2)
        txtSchLeaseRatio = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchLeaseRatio, 5, 2)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 6, 2)
        txtSchAllocTotal = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchAllocTotal, 7, 2)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 8, 2)
        txtSchDiscountRate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchDiscountRate, 9, 2)

        ' === 4段目: 維持管理費用 | 非リース割合 | (配分総額の非リース分) ===
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        tbl.Controls.Add(CreateGridLabel("維持管理費用"), 0, 3)
        txtSchMaintenanceCost = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchMaintenanceCost, 1, 3)
        tbl.Controls.Add(CreateGridLabel("非リース割合"), 2, 3)
        txtSchNonLeaseRatio = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchNonLeaseRatio, 3, 3)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 4, 3)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 5, 3)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 6, 3)
        txtSchDistributionRate = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tbl.Controls.Add(txtSchDistributionRate, 7, 3)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 8, 3)
        tbl.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 9, 3)

        ' リース割合・非リース割合・維持管理費用はユーザー入力後に再計算
        AddHandler txtSchLeaseRatio.Leave, Sub(s, e) RecalcAll()
        AddHandler txtSchNonLeaseRatio.Leave, Sub(s, e) RecalcAll()
        AddHandler txtSchMaintenanceCost.Leave, Sub(s, e) RecalcAll()

        tbl.RowCount = 4
        grpAccounting.Controls.Add(BuildAccSchMatrixSection())
        grpAccounting.Controls.Add(tbl)
        Return grpAccounting
    End Function

    ''' <summary>返済スケジュールマトリックス (フラット集計表形式)</summary>
    Private Function BuildAccSchMatrixSection() As Panel
        Dim pnlMatrix As New Panel() With {
            .Dock = DockStyle.Top, .AutoSize = True, .Padding = New Padding(0, 0, 0, 4)
        }

        ' 8列: 現在価値(120) | 使用権資産(80) | 返済スケジュール行名(100) | 期首(%) | 増加(%) | 変更増減(%) | 減少(%) | 期末(%)
        Dim tblMatrix As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 8, .RowCount = 4, .Padding = New Padding(4)
        }
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        tblMatrix.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))
        For r As Integer = 0 To 3
            tblMatrix.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        Next

        ' ヘッダー行: [現在価値(黄色)] [使用権資産] [返済スケジュール] [期首] [増加] [変更増減] [減少] [期末]
        tblMatrix.Controls.Add(New Label() With {
            .Text = "現在価値",
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(255, 235, 59),
            .ForeColor = CLR_TEXT,
            .Font = FNT_LABEL,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Margin = New Padding(2)
        }, 0, 0)
        tblMatrix.Controls.Add(CreateGridLabel("使用権資産"), 1, 0)
        tblMatrix.Controls.Add(CreateGridLabel("返済スケジュール"), 2, 0)
        Dim schHeaders() As String = {"期首", "増加", "変更増減", "減少", "期末"}
        For i As Integer = 0 To schHeaders.Length - 1
            tblMatrix.Controls.Add(CreateGridLabel(schHeaders(i)), i + 3, 0)
        Next

        ' Row 1: 現在価値 | 空 | 使用権資産 | values
        txtSchPresentValue = New TextBox() With {
            .Dock = DockStyle.Fill, .BackColor = CLR_CARD,
            .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)
        }
        tblMatrix.Controls.Add(txtSchPresentValue, 0, 1)
        tblMatrix.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 1, 1)
        tblMatrix.Controls.Add(CreateGridLabel("使用権資産"), 2, 1)
        txtSchRouBegin = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchRouIncrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchRouChange = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchRouDecrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchRouEnd = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tblMatrix.Controls.Add(txtSchRouBegin, 3, 1)
        tblMatrix.Controls.Add(txtSchRouIncrease, 4, 1)
        tblMatrix.Controls.Add(txtSchRouChange, 5, 1)
        tblMatrix.Controls.Add(txtSchRouDecrease, 6, 1)
        tblMatrix.Controls.Add(txtSchRouEnd, 7, 1)

        ' Row 2: 空 | 空 | リース負債 | values
        tblMatrix.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 0, 2)
        tblMatrix.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 1, 2)
        tblMatrix.Controls.Add(CreateGridLabel("リース負債"), 2, 2)
        txtSchLiabBegin = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchLiabIncrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchLiabChange = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchLiabDecrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchLiabEnd = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tblMatrix.Controls.Add(txtSchLiabBegin, 3, 2)
        tblMatrix.Controls.Add(txtSchLiabIncrease, 4, 2)
        tblMatrix.Controls.Add(txtSchLiabChange, 5, 2)
        tblMatrix.Controls.Add(txtSchLiabDecrease, 6, 2)
        tblMatrix.Controls.Add(txtSchLiabEnd, 7, 2)

        ' Row 3: 空 | 空 | 除去債務 | values
        tblMatrix.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 0, 3)
        tblMatrix.Controls.Add(New Label() With {.Dock = DockStyle.Fill}, 1, 3)
        tblMatrix.Controls.Add(CreateGridLabel("除去債務"), 2, 3)
        txtSchAroBegin = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchAroIncrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchAroChange = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchAroDecrease = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        txtSchAroEnd = New TextBox() With {.Dock = DockStyle.Fill, .BackColor = CLR_CARD, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = HorizontalAlignment.Right, .Margin = New Padding(2)}
        tblMatrix.Controls.Add(txtSchAroBegin, 3, 3)
        tblMatrix.Controls.Add(txtSchAroIncrease, 4, 3)
        tblMatrix.Controls.Add(txtSchAroChange, 5, 3)
        tblMatrix.Controls.Add(txtSchAroDecrease, 6, 3)
        tblMatrix.Controls.Add(txtSchAroEnd, 7, 3)

        pnlMatrix.Controls.Add(tblMatrix)
        Return pnlMatrix
    End Function

    ''' <summary>＜変更履歴＞</summary>
    Private Function BuildAccChangeHistorySection() As GroupBox
        Dim grpChangeHistory As GroupBox = CreateSection("＜変更履歴＞")
        grpChangeHistory.Height = 160
        grpChangeHistory.AutoSize = False

        dgvChangeHistory = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = False,
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

        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更", .Name = "ChangeNo", .FillWeight = 6,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更計上日", .Name = "ChangeDate", .FillWeight = 12,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "事由", .Name = "Reason", .FillWeight = 8
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "内容", .Name = "Content", .FillWeight = 10
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更額", .Name = "ChangeAmount", .FillWeight = 12,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更後資産額", .Name = "AfterAssetAmount", .FillWeight = 12,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更前負債額", .Name = "BeforeLiabAmount", .FillWeight = 12,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvChangeHistory.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "変更後負債額", .Name = "AfterLiabAmount", .FillWeight = 12,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })

        grpChangeHistory.Controls.Add(dgvChangeHistory)
        Return grpChangeHistory
    End Function

    ''' <summary>
    ''' 契約タブ・初回金タブの入力値を会計タブに反映し、各種会計金額を自動計算する。
    ''' 企業会計基準第34号に基づく当初認識（使用権資産・リース負債）を算定する。
    ''' </summary>
    Private Sub UpdateAccountingTabValues()
        If Not _isLoaded Then Return
        Try
            Dim startDt As DateTime = dtpStartDate.Value
            Dim endDt As DateTime = dtpEndDate.Value
            Dim freePeriodMonths As Integer = CInt(numFreePeriod.Value)

            ' 基本リース期間（無償期間控除後、月数）
            Dim baseMonths As Integer = Math.Max(0, CalcMonthsBetween(startDt, endDt) - freePeriodMonths)
            ' 会計期間 = 基本期間 + 延長オプション月数（第34号§17 延長オプション合理的確実性）
            Dim renewalExtMonths As Integer = CInt(numExtMonths.Value)
            Dim accountingMonths As Integer = baseMonths + renewalExtMonths

            ' --- ＜現契約期間＞ ---
            txtSchContractDate.Text = startDt.ToString("yyyy/MM/dd")
            txtSchStartDate.Text = startDt.ToString("yyyy/MM/dd")
            txtSchEndDate.Text = endDt.ToString("yyyy/MM/dd")
            txtSchContractPeriod.Text = lblLeaseMonths.Text

            ' --- ＜現支払情報＞ ---
            txtSchFreePeriod.Text = freePeriodMonths.ToString() & " ヶ月"
            txtSchPayCount.Text = baseMonths.ToString() & " 回"
            txtSchPayInterval.Text = "1 ヶ月"
            Dim firstPayDt As DateTime = startDt.AddMonths(freePeriodMonths + 1)
            txtSchFirstPayDate.Text = firstPayDt.ToString("yyyy/MM/dd")
            txtSchLastPayDate.Text = endDt.AddMonths(-1).ToString("yyyy/MM/dd")

            ' --- ＜会計期間＞ ---
            txtSchRenewalForecastCount.Text = "0"
            txtSchAccStartDate.Text = startDt.ToString("yyyy/MM/dd")
            Dim accEndDt As DateTime = startDt.AddMonths(accountingMonths)
            txtSchAccEndDate.Text = accEndDt.ToString("yyyy/MM/dd")
            txtSchAccPeriod.Text = accountingMonths.ToString() & "ヶ月"

            ' 月額賃料 = リース判定タブの月額リース料（numMonthlyRentJudge）を使用
            Dim monthlyRentExTax As Decimal = numMonthlyRentJudge.Value
            txtSchRent.Text = monthlyRentExTax.ToString("N0")
            txtSchRenewalRent.Text = "0"

            ' --- 賃料総額・算定総額（第34号§22 リース負債の当初測定） ---
            Dim rentTotal As Decimal = monthlyRentExTax * baseMonths
            Dim calcTotal As Decimal = monthlyRentExTax * accountingMonths
            txtSchRentTotal.Text = rentTotal.ToString("N0")
            txtSchCalcTotal.Text = calcTotal.ToString("N0")

            ' --- 割引率（第34号§22: 計算利子率、又は追加借入利子率） ---
            Dim discRate As Decimal = numDiscountRate.Value
            txtSchDiscountRate.Text = discRate.ToString("0.00")

            ' --- リース・非リース 配分計算（第34号§13 構成要素の区分） ---
            Dim leaseRatio As Decimal = 0
            Dim nonLeaseRatio As Decimal = 0
            Decimal.TryParse(txtSchLeaseRatio.Text.Replace(",", ""),
                             System.Globalization.NumberStyles.Any,
                             System.Globalization.CultureInfo.InvariantCulture, leaseRatio)
            Decimal.TryParse(txtSchNonLeaseRatio.Text.Replace(",", ""),
                             System.Globalization.NumberStyles.Any,
                             System.Globalization.CultureInfo.InvariantCulture, nonLeaseRatio)
            Dim totalRatio As Decimal = leaseRatio + nonLeaseRatio
            Dim leaseAllocTotal As Decimal
            If totalRatio > 0 Then
                leaseAllocTotal = Math.Round(calcTotal * leaseRatio / totalRatio, 0)
                txtSchAllocTotal.Text = leaseAllocTotal.ToString("N0")
                txtSchDistributionRate.Text = Math.Round(calcTotal * nonLeaseRatio / totalRatio, 0).ToString("N0")
            Else
                leaseAllocTotal = calcTotal
                txtSchAllocTotal.Text = calcTotal.ToString("N0")
                txtSchDistributionRate.Text = "0"
            End If

            ' --- 現在価値計算（第34号§22: 将来リース料の現在価値、期末払い年金現価公式） ---
            Dim monthlyPayment As Decimal = If(accountingMonths > 0, leaseAllocTotal / accountingMonths, 0)
            Dim pv As Decimal = 0
            If discRate > 0 AndAlso accountingMonths > 0 Then
                Dim r As Double = CDbl(discRate) / 12.0 / 100.0
                pv = CDec(CDbl(monthlyPayment) * (1.0 - Math.Pow(1.0 + r, -accountingMonths)) / r)
            Else
                pv = leaseAllocTotal
            End If
            pv = Math.Round(pv, 0)
            txtSchPresentValue.Text = pv.ToString("N0")

            ' --- 使用権資産・リース負債 当初認識（第34号§27・§22） ---
            ' 期首が未設定（新規契約）の場合のみ自動計算する
            Dim rouBeginVal As Decimal = 0
            Dim liabBeginVal As Decimal = 0
            Decimal.TryParse(txtSchRouBegin.Text.Replace(",", ""),
                             System.Globalization.NumberStyles.Any,
                             System.Globalization.CultureInfo.InvariantCulture, rouBeginVal)
            Decimal.TryParse(txtSchLiabBegin.Text.Replace(",", ""),
                             System.Globalization.NumberStyles.Any,
                             System.Globalization.CultureInfo.InvariantCulture, liabBeginVal)

            If rouBeginVal = 0 AndAlso liabBeginVal = 0 Then
                ' 使用権資産 = PV + 初期直接費用 + 原状回復費用見積 - リース・インセンティブ（第34号§27）
                Dim rouAmount As Decimal = pv + numInitialDirectCost.Value + numRestorationCost.Value - numLeaseIncentive.Value
                rouAmount = Math.Max(0, rouAmount)
                txtSchRouBegin.Text = "0"
                txtSchRouIncrease.Text = rouAmount.ToString("N0")
                txtSchRouChange.Text = "0"
                txtSchRouDecrease.Text = "0"
                txtSchRouEnd.Text = rouAmount.ToString("N0")

                ' リース負債 = PV（第34号§22）
                txtSchLiabBegin.Text = "0"
                txtSchLiabIncrease.Text = pv.ToString("N0")
                txtSchLiabChange.Text = "0"
                txtSchLiabDecrease.Text = "0"
                txtSchLiabEnd.Text = pv.ToString("N0")
            End If

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"[UpdateAccountingTabValues] Error: {ex.Message}")
        End Try
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

    Private Sub InitTabJudge_Pro()
        pgJudgment.BackColor = CLR_BG
        pgJudgment.Padding = New Padding(6)

        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 1,
            .RowCount = 5
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Dim grpIdent As GroupBox = CreateSection("識別判定")
        Dim tlpIdent As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        grpQ1 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 5), .AutoSize = True, .BackColor = CLR_CARD}
        Dim flowQ1 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ1Yes = New RadioButton() With {.Text = "あり (特定されている)", .AutoSize = True, .Font = FNT_INPUT}
        rbQ1No = New RadioButton() With {.Text = "なし", .AutoSize = True, .Font = FNT_INPUT}
        AddHandler rbQ1Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ1No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ1.Controls.AddRange({rbQ1Yes, rbQ1No})
        grpQ1.Controls.Add(flowQ1)
        _tooltipProvider.SetToolTip(grpQ1, "リース対象の資産が契約において特定されているか")

        Dim rQ1 As Integer = tlpIdent.RowCount
        tlpIdent.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpIdent.Controls.Add(CreateFieldLabel("資産の特定"), 0, rQ1)
        tlpIdent.Controls.Add(grpQ1, 1, rQ1)
        tlpIdent.SetColumnSpan(grpQ1, 3)
        tlpIdent.RowCount += 1

        grpQ2 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 5), .AutoSize = True, .BackColor = CLR_CARD}
        Dim flowQ2 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ2Yes = New RadioButton() With {.Text = "あり (サプライヤーの権利)", .AutoSize = True, .Font = FNT_INPUT}
        rbQ2No = New RadioButton() With {.Text = "なし", .AutoSize = True, .Checked = True, .Font = FNT_INPUT}
        AddHandler rbQ2Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ2No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ2.Controls.AddRange({rbQ2Yes, rbQ2No})
        grpQ2.Controls.Add(flowQ2)
        _tooltipProvider.SetToolTip(grpQ2, "サプライヤーが実質的な代替権を有しているか")

        Dim rQ2 As Integer = tlpIdent.RowCount
        tlpIdent.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpIdent.Controls.Add(CreateFieldLabel("実質的代替権"), 0, rQ2)
        tlpIdent.Controls.Add(grpQ2, 1, rQ2)
        tlpIdent.SetColumnSpan(grpQ2, 3)
        tlpIdent.RowCount += 1

        grpQ3 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 5), .AutoSize = True, .BackColor = CLR_CARD}
        Dim flowQ3 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ3Yes = New RadioButton() With {.Text = "あり", .AutoSize = True, .Checked = True, .Font = FNT_INPUT}
        rbQ3No = New RadioButton() With {.Text = "なし", .AutoSize = True, .Font = FNT_INPUT}
        AddHandler rbQ3Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ3No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ3.Controls.AddRange({rbQ3Yes, rbQ3No})
        grpQ3.Controls.Add(flowQ3)
        _tooltipProvider.SetToolTip(grpQ3, "借手が使用から経済的利益のほぼすべてを得る権利を有しているか")

        grpQ4 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 5), .AutoSize = True, .BackColor = CLR_CARD}
        Dim flowQ4 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ4Yes = New RadioButton() With {.Text = "あり", .AutoSize = True, .Checked = True, .Font = FNT_INPUT}
        rbQ4No = New RadioButton() With {.Text = "なし", .AutoSize = True, .Font = FNT_INPUT}
        AddHandler rbQ4Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ4No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ4.Controls.AddRange({rbQ4Yes, rbQ4No})
        grpQ4.Controls.Add(flowQ4)
        _tooltipProvider.SetToolTip(grpQ4, "借手が資産の使用方法を指図する権利を有しているか")

        Dim rQ34 As Integer = tlpIdent.RowCount
        tlpIdent.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpIdent.Controls.Add(CreateFieldLabel("経済的利益"), 0, rQ34)
        tlpIdent.Controls.Add(grpQ3, 1, rQ34)
        tlpIdent.Controls.Add(CreateFieldLabel("使用指図権"), 2, rQ34)
        tlpIdent.Controls.Add(grpQ4, 3, rQ34)
        tlpIdent.RowCount += 1

        grpIdent.Controls.Add(tlpIdent)

        Dim grpExempt As GroupBox = CreateSection("期間・免除規定判定")
        Dim tlpExempt As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        dtpJudgeStart = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Value = New DateTime(2024, 7, 24), .Font = FNT_INPUT}
        AddHandler dtpJudgeStart.ValueChanged, AddressOf OnJudgeTrigger
        dtpJudgeEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Value = New DateTime(2026, 7, 23), .Font = FNT_INPUT}
        AddHandler dtpJudgeEnd.ValueChanged, AddressOf OnJudgeTrigger
        _tooltipProvider.SetToolTip(dtpJudgeStart, "リース開始日")
        _tooltipProvider.SetToolTip(dtpJudgeEnd, "リース終了日")
        AddFieldRow(tlpExempt, "開始日", dtpJudgeStart, "終了日", dtpJudgeEnd)

        Dim flowTerm As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        lblTermMonths = New Label() With {.Text = "24", .Font = _font11Bold, .AutoSize = True, .ForeColor = CLR_HEADER}
        Dim lblMonthUnit As New Label() With {.Text = " ヶ月", .AutoSize = True, .Padding = New Padding(0, 4, 0, 0), .Font = FNT_INPUT}
        lblDateError = New Label() With {.Text = "", .ForeColor = Color.FromArgb(220, 53, 69), .Font = FNT_LABEL, .AutoSize = True, .Padding = New Padding(5, 4, 0, 0)}
        flowTerm.Controls.AddRange({lblTermMonths, lblMonthUnit, lblDateError})
        lblShortTermResult = New Label() With {.Text = "-", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        AddFieldRow(tlpExempt, "見積期間", flowTerm, "短期判定", lblShortTermResult)

        Dim flowExt As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        chkExtOption = New CheckBox() With {.Text = "あり", .AutoSize = True, .Font = FNT_INPUT}
        cboExtCertainty = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Width = 130, .Enabled = False, .Font = FNT_INPUT}
        cboExtCertainty.Items.AddRange({"低い(行使しない)", "高い(行使する)"})
        cboExtCertainty.SelectedIndex = 0
        AddHandler chkExtOption.CheckedChanged, AddressOf OnExtOptionChanged
        AddHandler cboExtCertainty.SelectedIndexChanged, AddressOf OnJudgeTrigger
        flowExt.Controls.AddRange({chkExtOption, cboExtCertainty})
        numExtMonths = New NumericUpDown() With {.Dock = DockStyle.Fill, .Minimum = 0, .Maximum = 600, .Value = 0, .Enabled = False, .Font = FNT_INPUT, .TextAlign = HorizontalAlignment.Right}
        AddHandler numExtMonths.ValueChanged, AddressOf OnJudgeTrigger
        _tooltipProvider.SetToolTip(chkExtOption, "更新・延長オプションの有無")
        AddFieldRow(tlpExempt, "延長OP", flowExt, "延長期間", numExtMonths)

        Dim flowTerm2 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        chkTerminateOption = New CheckBox() With {.Text = "あり", .AutoSize = True, .Font = FNT_INPUT}
        cboTerminateCertainty = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Width = 160, .Enabled = False, .Font = FNT_INPUT}
        cboTerminateCertainty.Items.AddRange({"行使しない(期間短縮なし)", "行使する(期間短縮)"})
        cboTerminateCertainty.SelectedIndex = 0
        AddHandler chkTerminateOption.CheckedChanged, AddressOf OnTerminateOptionChanged
        AddHandler cboTerminateCertainty.SelectedIndexChanged, AddressOf OnJudgeTrigger
        flowTerm2.Controls.AddRange({chkTerminateOption, cboTerminateCertainty})
        _tooltipProvider.SetToolTip(chkTerminateOption, "中途解約オプションの有無")

        Dim rTerminate As Integer = tlpExempt.RowCount
        tlpExempt.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tlpExempt.Controls.Add(CreateFieldLabel("解約OP"), 0, rTerminate)
        tlpExempt.Controls.Add(flowTerm2, 1, rTerminate)
        tlpExempt.SetColumnSpan(flowTerm2, 3)
        tlpExempt.RowCount += 1

        numAssetValue = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Fill, .TextAlign = HorizontalAlignment.Right, .Font = FNT_INPUT}
        AddHandler numAssetValue.ValueChanged, AddressOf OnJudgeTrigger
        lblLowValueResult = New Label() With {.Text = "-", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = FNT_INPUT, .ForeColor = CLR_TEXT}
        _tooltipProvider.SetToolTip(numAssetValue, "リース対象資産の取得価額（300万円以下で少額リース免除の対象）")
        AddFieldRow(tlpExempt, "取得価額", numAssetValue, "少額判定", lblLowValueResult)

        chkApplyExemption = New CheckBox() With {.Text = "免除規定を適用する (オフバランス処理)", .AutoSize = True, .Enabled = False, .Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddHandler chkApplyExemption.CheckedChanged, AddressOf OnJudgeTrigger

        Dim rExempt As Integer = tlpExempt.RowCount
        tlpExempt.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tlpExempt.Controls.Add(CreateFieldLabel("免除規定"), 0, rExempt)
        tlpExempt.Controls.Add(chkApplyExemption, 1, rExempt)
        tlpExempt.SetColumnSpan(chkApplyExemption, 3)
        tlpExempt.RowCount += 1

        AddSectionLabel(tlpExempt, "■ 会計処理オプション")

        chkServiceComponent = New CheckBox() With {.Text = "非リース構成要素を分離せず、リース料として結合する（実務的便法）", .AutoSize = True, .Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddHandler chkServiceComponent.CheckedChanged, AddressOf OnJudgeTrigger

        Dim rService As Integer = tlpExempt.RowCount
        tlpExempt.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tlpExempt.Controls.Add(CreateFieldLabel("構成要素"), 0, rService)
        tlpExempt.Controls.Add(chkServiceComponent, 1, rService)
        tlpExempt.SetColumnSpan(chkServiceComponent, 3)
        tlpExempt.RowCount += 1

        chkOwnershipTransfer = New CheckBox() With {.Text = "所有権移転条項あり（または割安購入選択権あり）", .AutoSize = True, .Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddHandler chkOwnershipTransfer.CheckedChanged, AddressOf OnJudgeTrigger

        Dim rOwnership As Integer = tlpExempt.RowCount
        tlpExempt.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tlpExempt.Controls.Add(CreateFieldLabel("所有権移転"), 0, rOwnership)
        tlpExempt.Controls.Add(chkOwnershipTransfer, 1, rOwnership)
        tlpExempt.SetColumnSpan(chkOwnershipTransfer, 3)
        tlpExempt.RowCount += 1

        numMonthlyRentJudge = New NumericUpDown() With {.Maximum = 9900000000D, .Minimum = 0D, .Value = 0D, .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right, .Width = 120, .Font = FNT_INPUT}
        AddHandler numMonthlyRentJudge.ValueChanged, AddressOf OnJudgeTrigger
        _tooltipProvider.SetToolTip(numMonthlyRentJudge, "月額リース料（入力時はPVベースで少額判定を行います）")
        AddFieldRow(tlpExempt, "月額リース料", numMonthlyRentJudge, Nothing, Nothing)

        numDiscountRate = New NumericUpDown() With {.DecimalPlaces = 2, .Increment = 0.01D, .Maximum = 20D, .Minimum = 0D, .Value = 0D, .TextAlign = HorizontalAlignment.Right, .Width = 100, .Font = FNT_INPUT}
        AddHandler numDiscountRate.ValueChanged, AddressOf OnJudgeTrigger
        _tooltipProvider.SetToolTip(numDiscountRate, "年利割引率（Tab4のスケジュール計算で使用）")
        AddFieldRow(tlpExempt, "割引率(%)", numDiscountRate, Nothing, Nothing)

        grpExempt.Controls.Add(tlpExempt)

        Dim grpResult As GroupBox = CreateSection("判定結果")
        grpResult.BackColor = Color.FromArgb(240, 248, 255)
        Dim tlpResult As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .Padding = New Padding(10)
        }

        Dim flowResultTop As New FlowLayoutPanel() With {.Dock = DockStyle.Top, .AutoSize = True, .WrapContents = False}
        lblResultText = New Label() With {
            .Text = "---", .AutoSize = True,
            .Font = _font18Bold,
            .ForeColor = CLR_HEADER
        }
        lblResultBadge = New Label() With {
            .Text = "---", .AutoSize = True,
            .ForeColor = Color.White,
            .BackColor = Color.FromArgb(204, 204, 204),
            .Padding = New Padding(6, 3, 6, 3),
            .Margin = New Padding(15, 8, 0, 0),
            .Font = _fontResultBadge
        }
        flowResultTop.Controls.AddRange({lblResultText, lblResultBadge})

        lblResultReason = New Label() With {
            .Text = "判定条件を入力してください。",
            .AutoSize = True,
            .ForeColor = Color.Gray,
            .Font = _fontResultReason,
            .Dock = DockStyle.Top,
            .Padding = New Padding(0, 4, 0, 0)
        }
        _tooltipProvider.SetToolTip(lblResultReason, "判定根拠は入力値から自動生成されます")

        tlpResult.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpResult.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpResult.Controls.Add(flowResultTop, 0, 0)
        tlpResult.Controls.Add(lblResultReason, 0, 1)
        grpResult.Controls.Add(tlpResult)

        ' =============================================
        ' セクション3: 契約詳細・関係者情報 (grpParty)
        ' =============================================
        Dim grpParty As GroupBox = CreateSection("契約詳細・関係者情報")
        Dim tlpParty As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tlpParty.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F))
        tlpParty.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpParty.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F))
        tlpParty.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        ' --- 貸主 ---
        AddSectionLabel(tlpParty, "■ 貸主")
        txtLenderName = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        txtLenderAddr = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "貸主名", txtLenderName, "住所", txtLenderAddr)
        txtLenderAccount = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "口座情報", txtLenderAccount, Nothing, Nothing)

        ' --- 代行者 ---
        AddSectionLabel(tlpParty, "■ 代行者")
        txtAgentName = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        txtAgentAddr = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "代行者名", txtAgentName, "住所", txtAgentAddr)

        ' --- 仲介者 ---
        AddSectionLabel(tlpParty, "■ 仲介者")
        txtBrokerName = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        txtBrokerAddr = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "仲介者名", txtBrokerName, "住所", txtBrokerAddr)
        txtBrokerAccount = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "口座情報", txtBrokerAccount, Nothing, Nothing)

        ' --- 借主 ---
        AddSectionLabel(tlpParty, "■ 借主")
        txtBorrowerName = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        txtBorrowerAddr = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "借主名", txtBorrowerName, "住所", txtBorrowerAddr)
        txtBorrowerAccount = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "口座情報", txtBorrowerAccount, Nothing, Nothing)

        ' --- 連帯保証人 ---
        AddSectionLabel(tlpParty, "■ 連帯保証人")
        txtGuarantor1 = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        txtGuarantor2 = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "保証人1", txtGuarantor1, "保証人2", txtGuarantor2)

        ' --- 契約期間・解約・延長 ---
        AddSectionLabel(tlpParty, "■ 契約期間")
        dtpPartyStartDate = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Font = FNT_INPUT}
        dtpPartyEndDate = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "開始日", dtpPartyStartDate, "終了日", dtpPartyEndDate)

        numPartyContractMonths = New NumericUpDown() With {.Dock = DockStyle.Fill, .Minimum = 0, .Maximum = 9999, .Value = 0, .Font = FNT_INPUT, .TextAlign = HorizontalAlignment.Right}
        txtNonCancelPeriod = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT}
        AddFieldRow(tlpParty, "契約月数", numPartyContractMonths, "解約不能期間", txtNonCancelPeriod)

        cboCancelOption = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = FNT_INPUT}
        cboCancelOption.Items.AddRange({"なし", "あり（違約金あり）", "あり（違約金なし）"})
        cboCancelOption.SelectedIndex = 0
        cboExtendOption = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = FNT_INPUT}
        cboExtendOption.Items.AddRange({"なし", "自動更新", "合意更新"})
        cboExtendOption.SelectedIndex = 0
        AddFieldRow(tlpParty, "解約オプション", cboCancelOption, "延長オプション", cboExtendOption)

        numRenewalCount = New NumericUpDown() With {.Dock = DockStyle.Fill, .Minimum = 0, .Maximum = 999, .Value = 0, .Font = FNT_INPUT, .TextAlign = HorizontalAlignment.Right}
        AddFieldRow(tlpParty, "更新回数", numRenewalCount, Nothing, Nothing)

        grpParty.Controls.Add(tlpParty)

        ' =============================================
        ' セクション5: 会計判定結果 (pnlResult)
        ' =============================================
        Dim pnlResult As GroupBox = CreateSection("会計判定結果")
        pnlResult.BackColor = Color.FromArgb(240, 248, 255)
        Dim tlpAccResult As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(10)
        }
        tlpAccResult.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        tlpAccResult.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpAccResult.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        tlpAccResult.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        ' --- 支払サマリ ---
        AddSectionLabel(tlpAccResult, "■ 支払サマリ")
        lblTotalPayment = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = ContentAlignment.MiddleLeft}
        lblTotalCount = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = ContentAlignment.MiddleLeft}
        AddFieldRow(tlpAccResult, "総支払額", lblTotalPayment, "支払回数", lblTotalCount)

        ' --- 仕訳科目 ---
        AddSectionLabel(tlpAccResult, "■ 仕訳科目")
        lblRouAsset = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_HEADER, .TextAlign = ContentAlignment.MiddleLeft}
        _tooltipProvider.SetToolTip(lblRouAsset, "勘定科目コード: 2100")
        lblLeaseLiab = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_HEADER, .TextAlign = ContentAlignment.MiddleLeft}
        _tooltipProvider.SetToolTip(lblLeaseLiab, "勘定科目コード: 1100")
        AddFieldRow(tlpAccResult, "使用権資産(2100)", lblRouAsset, "リース負債(1100)", lblLeaseLiab)

        lblDeposit = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = ContentAlignment.MiddleLeft}
        _tooltipProvider.SetToolTip(lblDeposit, "勘定科目コード: 2210")
        lblPrepaidRent = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = ContentAlignment.MiddleLeft}
        _tooltipProvider.SetToolTip(lblPrepaidRent, "勘定科目コード: 3010")
        AddFieldRow(tlpAccResult, "敷金・保証金(2210)", lblDeposit, "前払リース料(3010)", lblPrepaidRent)

        lblEquipmentCost = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = FNT_INPUT, .ForeColor = CLR_TEXT, .TextAlign = ContentAlignment.MiddleLeft}
        _tooltipProvider.SetToolTip(lblEquipmentCost, "勘定科目コード: 2150")
        AddFieldRow(tlpAccResult, "設備関連費(2150)", lblEquipmentCost, Nothing, Nothing)

        ' --- 割引率・現在価値 ---
        AddSectionLabel(tlpAccResult, "■ 割引計算")
        txtResultDiscountRate = New TextBox() With {.Dock = DockStyle.Fill, .Font = FNT_INPUT, .ReadOnly = True, .BackColor = CLR_READONLY, .TextAlign = HorizontalAlignment.Right, .Text = "0.00"}
        lblPresentValue = New Label() With {.Text = "0", .Dock = DockStyle.Fill, .Font = New Font("Meiryo", 10.0F, FontStyle.Bold), .ForeColor = CLR_HEADER, .TextAlign = ContentAlignment.MiddleLeft}
        AddFieldRow(tlpAccResult, "適用割引率(%)", txtResultDiscountRate, "現在価値合計", lblPresentValue)

        pnlResult.Controls.Add(tlpAccResult)

        ' === mainTbl にセクションを配置 ===
        mainTbl.Controls.Add(grpIdent, 0, 0)
        mainTbl.Controls.Add(grpExempt, 0, 1)
        mainTbl.Controls.Add(grpParty, 0, 2)
        mainTbl.Controls.Add(grpResult, 0, 3)
        mainTbl.Controls.Add(pnlResult, 0, 4)

        scroll.Controls.Add(mainTbl)
        pgJudgment.Controls.Add(scroll)
    End Sub

    Private Sub RecalcAll()
        If Not _isLoaded Then Return

        CalcLeaseMonths()
        CalcMonthlyTotals()
        UpdateAccountingTabValues()
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

    Private Sub UpdateAppliedRate()
        If Not _isLoaded Then Return
        If numImplicitRate Is Nothing OrElse lblAppliedRate Is Nothing Then Return
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

    Private Sub OnExtOptionChanged(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        Dim isDisabled As Boolean = Not chkExtOption.Checked
        cboExtCertainty.Enabled = Not isDisabled
        numExtMonths.Enabled = Not isDisabled
        If isDisabled Then
            RemoveHandler numExtMonths.ValueChanged, AddressOf OnJudgeTrigger
            numExtMonths.Value = 0
            AddHandler numExtMonths.ValueChanged, AddressOf OnJudgeTrigger
        End If
        RecalcJudge()
    End Sub

    Private Sub OnTerminateOptionChanged(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        cboTerminateCertainty.Enabled = chkTerminateOption.Checked
        If Not chkTerminateOption.Checked Then
            RemoveHandler cboTerminateCertainty.SelectedIndexChanged, AddressOf OnJudgeTrigger
            cboTerminateCertainty.SelectedIndex = 0
            AddHandler cboTerminateCertainty.SelectedIndexChanged, AddressOf OnJudgeTrigger
        End If
        RecalcJudge()
    End Sub

    Private Sub OnJudgeTrigger(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        RecalcJudge()
    End Sub

    Private Sub RecalcJudge()
        If lblResultText Is Nothing Then Return

        Dim q1Yes As Boolean = rbQ1Yes.Checked
        Dim q2No As Boolean = rbQ2No.Checked
        Dim q3Yes As Boolean = rbQ3Yes.Checked
        Dim q4Yes As Boolean = rbQ4Yes.Checked

        Dim startDt As DateTime = dtpJudgeStart.Value.Date
        Dim endDt As DateTime = dtpJudgeEnd.Value.Date
        Dim assetVal As Decimal = numAssetValue.Value

        Dim hasExt As Boolean = chkExtOption.Checked
        Dim isExtCertain As Boolean = (cboExtCertainty.SelectedIndex = 1)
        Dim extMonths As Integer = CInt(numExtMonths.Value)

        Dim isLease As Boolean = (q1Yes AndAlso q2No AndAlso q3Yes AndAlso q4Yes)

        Dim months As Integer = 0
        Dim isValidDate As Boolean = True

        If endDt < startDt Then
            months = 0
            lblDateError.Text = "終了日が開始日より前です"
            isValidDate = False
        Else
            lblDateError.Text = ""
            months = (endDt.Year - startDt.Year) * 12 + (endDt.Month - startDt.Month)
            Dim tempDate As DateTime = startDt.AddMonths(months)
            If endDt < tempDate.AddDays(-1) Then
                months -= 1
            End If
            If hasExt AndAlso isExtCertain Then
                months += extMonths
            End If
        End If

        lblTermMonths.Text = months.ToString()

        Dim isShortTerm As Boolean = (isValidDate AndAlso months > 0 AndAlso months <= 12)
        If isShortTerm Then
            lblShortTermResult.Text = "該当 (12ヶ月以内)"
            lblShortTermResult.ForeColor = Color.FromArgb(0, 123, 255)
            lblShortTermResult.Font = _boldFont
        Else
            lblShortTermResult.Text = "非該当"
            lblShortTermResult.ForeColor = CLR_TEXT
            lblShortTermResult.Font = _regularFont
        End If

        Dim monthlyRent As Decimal = numMonthlyRentJudge.Value
        Dim discountRate As Decimal = numDiscountRate.Value
        Dim isLowValue As Boolean = False

        If monthlyRent > 0 AndAlso months > 0 Then
            Dim pvValue As Double
            Dim r As Double = CDbl(discountRate) / 12.0 / 100.0
            If r > 0 Then
                ' 期末払い（Ordinary Annuity）前提の年金現価係数を使用
                pvValue = CDbl(monthlyRent) * ((1.0 - Math.Pow(1.0 + r, -months)) / r)
            Else
                pvValue = CDbl(monthlyRent) * months
            End If
            isLowValue = (pvValue <= CDbl(LOW_VALUE_THRESHOLD))
            If isLowValue Then
                lblLowValueResult.Text = String.Format("該当 (PV: ¥{0:N0})", pvValue)
                lblLowValueResult.ForeColor = Color.FromArgb(0, 123, 255)
                lblLowValueResult.Font = _boldFont
            Else
                lblLowValueResult.Text = String.Format("非該当 (PV: ¥{0:N0})", pvValue)
                lblLowValueResult.ForeColor = CLR_TEXT
                lblLowValueResult.Font = _regularFont
            End If
        ElseIf assetVal > 0 Then
            isLowValue = (assetVal <= LOW_VALUE_THRESHOLD)
            If isLowValue Then
                lblLowValueResult.Text = "該当 (基準額以下)"
                lblLowValueResult.ForeColor = Color.FromArgb(0, 123, 255)
                lblLowValueResult.Font = _boldFont
            Else
                lblLowValueResult.Text = "非該当"
                lblLowValueResult.ForeColor = CLR_TEXT
                lblLowValueResult.Font = _regularFont
            End If
        Else
            lblLowValueResult.Text = "-"
            lblLowValueResult.ForeColor = CLR_TEXT
        End If

        Dim exemptEligible As Boolean = (isShortTerm OrElse isLowValue)

        RemoveHandler chkApplyExemption.CheckedChanged, AddressOf OnJudgeTrigger

        chkApplyExemption.Enabled = False

        chkApplyExemption.Checked = exemptEligible

        AddHandler chkApplyExemption.CheckedChanged, AddressOf OnJudgeTrigger

        lblResultText.ForeColor = CLR_HEADER
        lblResultBadge.BackColor = Color.FromArgb(204, 204, 204)

        Dim reasonParts As New List(Of String)

        If Not isLease Then
            lblResultText.Text = "対象外"
            lblResultBadge.Text = "リース資産計上不要"
            reasonParts.Add("識別判定の条件を満たさないため、通常の賃貸借処理（オフバランス）となります。")
        Else
            If chkApplyExemption.Checked Then
                lblResultText.Text = "オフバランス処理"
                lblResultText.ForeColor = Color.FromArgb(23, 162, 184)
                lblResultBadge.Text = "免除規定適用"
                lblResultBadge.BackColor = Color.FromArgb(23, 162, 184)
                reasonParts.Add("短期または少額資産の免除規定を適用し、賃貸借処理として処理します。")
            Else
                lblResultText.Text = "オンバランス処理"
                lblResultText.ForeColor = Color.FromArgb(220, 53, 69)
                lblResultBadge.Text = "資産計上必須"
                lblResultBadge.BackColor = CLR_ACCENT
                reasonParts.Add("使用権資産およびリース負債の計上が必要です。")
            End If

            If chkTerminateOption.Checked AndAlso cboTerminateCertainty.SelectedIndex = 1 Then
                ' ※ 本システムでは期間の自動減算は行わず、ユーザーに通知する仕様としている。
                '    (ユーザーは終了日を変更するか、判定結果を見て運用で対応する想定)
                reasonParts.Add("※解約オプションの行使が見込まれるため、期間短縮を考慮する必要があります")
            End If

            If chkServiceComponent.Checked Then
                reasonParts.Add("※実務的便法を適用し、非リース構成要素を含めた金額で資産計上します")
            End If

            If Not chkApplyExemption.Checked Then
                If chkOwnershipTransfer.Checked Then
                    reasonParts.Add("※所有権移転リースとして、経済的耐用年数に基づき償却計算を行います。")
                Else
                    reasonParts.Add("※所有権移転外リースとして、リース期間に基づき償却計算を行います。")
                End If
            End If
        End If

        lblResultReason.Text = String.Join(vbCrLf, reasonParts)
        lblJudgmentPreview.Text = "リース判定: " & lblResultText.Text
    End Sub

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

    Private Sub OnAssetSearchClick(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtAssetNo.Text) Then
            MessageBox.Show("資産番号を入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using frm As New FrmAssetDetailEntry()
                frm.InitAssetNo = txtAssetNo.Text
                frm.AssetCategory = cmbAssetCategory.SelectedItem.ToString()
                If frm.ShowDialog(Me) = DialogResult.OK Then
                    AddAssetRow(frm)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("資産情報の取得に失敗しました。" & vbCrLf & ex.Message,
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnAssetNewClick(sender As Object, e As EventArgs)
        Using frm As New FrmAssetDetailEntry()
            frm.InitAssetNo = GetNextAssetNo()
            frm.AssetCategory = cmbAssetCategory.SelectedItem.ToString()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                AddAssetRow(frm)
            End If
        End Using
    End Sub

    ''' <summary>
    ''' DBとメモリストアと画面上の未登録資産から次の資産番号を生成する
    ''' </summary>
    Private Function GetNextAssetNo() As String
        Dim maxCounter As Integer = 0

        ' DBから最大カウンタを取得
        Try
            Dim connMgr As New LeaseM4BS.DataAccess.DbConnectionManager()
            Using conn = connMgr.GetConnection()
                Using cmd As New Npgsql.NpgsqlCommand(
                    "SELECT asset_no FROM ctb_lease_integrated WHERE asset_no LIKE 'ASSET-%'", conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                Dim num As Integer = ParseAssetCounter(reader.GetString(0))
                                If num > maxCounter Then maxCounter = num
                            End If
                        End While
                    End Using
                End Using
            End Using
        Catch
        End Try

        ' メモリストアも考慮
        For Each rec In CtbDataStore.Instance.GetAll()
            Dim num As Integer = ParseAssetCounter(rec.AssetNo)
            If num > maxCounter Then maxCounter = num
        Next

        ' 画面上の未登録資産（_assetCtbMap）も考慮
        For Each key In _assetCtbMap.Keys
            Dim num As Integer = ParseAssetCounter(key)
            If num > maxCounter Then maxCounter = num
        Next

        Return String.Format("ASSET-{0:D4}", maxCounter + 1)
    End Function

    Private Shared Function ParseAssetCounter(assetNo As String) As Integer
        If String.IsNullOrEmpty(assetNo) OrElse Not assetNo.StartsWith("ASSET-") Then Return 0
        Dim suffix As String = assetNo.Substring(6)
        Dim num As Integer
        If Integer.TryParse(suffix, num) Then Return num
        Return 0
    End Function

    Private Sub AddAssetRow(frm As FrmAssetDetailEntry)
        Dim emptyRowIndex As Integer = -1
        For i As Integer = 0 To dgvAssets.Rows.Count - 1
            Dim row As DataGridViewRow = dgvAssets.Rows(i)
            If row.IsNewRow Then Continue For
            If row.Cells("AssetNo").Value Is Nothing OrElse
               String.IsNullOrEmpty(row.Cells("AssetNo").Value.ToString()) Then
                emptyRowIndex = i
                Exit For
            End If
        Next

        If emptyRowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvAssets.Rows(emptyRowIndex)
            row.Cells("AssetNo").Value = frm.AssetNo
            row.Cells("AssetCategory").Value = frm.AssetCategory
            row.Cells("AssetName").Value = frm.AssetName
            row.Cells("InstallLocation").Value = frm.InstallLocation
            row.Cells("DeptAllocation").Value = frm.DeptAllocationSummary
        Else
            dgvAssets.Rows.Add(
                False,
                frm.AssetNo,
                frm.AssetCategory,
                frm.AssetName,
                frm.InstallLocation,
                frm.DeptAllocationSummary)
        End If

        ' 資産詳細データをCTBレコードとして保持
        Dim ctb As New CtbRecord()
        ctb.AssetNo = frm.AssetNo
        ctb.AssetCategory = frm.AssetCategory
        ctb.AssetName = frm.AssetName
        ctb.CompanyName = frm.CompanyNameValue
        ctb.InstallLocation = frm.InstallLocation
        ctb.Remarks = frm.RemarksValue
        ctb.DeptAllocations = frm.DeptAllocationList

        ' 種別固有データ
        Select Case frm.AssetCategory
            Case "不動産"
                ctb.ReStructure = frm.ReStructure
                ctb.ReArea = frm.ReArea
                ctb.ReLayout = frm.ReLayout
                ctb.ReCompletionDate = frm.ReCompletionDate
                ctb.ReLandlordName = frm.ReLandlordName
                ctb.ReBrokerCompany = frm.ReBrokerCompany
                ctb.ReUsageRestrictions = frm.ReUsageRestrictions
            Case "車両"
                ctb.VhChassisNo = frm.VhChassisNo
                ctb.VhRegistrationNo = frm.VhRegistrationNo
                ctb.VhVehicleType = frm.VhVehicleType
                ctb.VhInspectionDate = frm.VhInspectionDate
                ctb.VhMileageLimit = frm.VhMileageLimit
            Case "OA機器"
                ctb.OaModelNo = frm.OaModelNo
                ctb.OaSerialNo = frm.OaSerialNo
                ctb.OaMaintenanceDate = frm.OaMaintenanceDate
                ctb.OaMaintenanceContract = frm.OaMaintenanceContract
        End Select

        _assetCtbMap(frm.AssetNo) = ctb

        UpdateAssetCount()
    End Sub

    Private Sub UpdateAssetCount()
        Dim count As Integer = 0
        For Each row As DataGridViewRow In dgvAssets.Rows
            If row.IsNewRow Then Continue For
            If row.Cells("AssetNo").Value IsNot Nothing AndAlso
               Not String.IsNullOrEmpty(row.Cells("AssetNo").Value.ToString()) Then
                count += 1
            End If
        Next
        lblAssetCount.Text = "資産件数: " & count.ToString() & "件"
    End Sub

    Private Sub OnDeleteRowClick(sender As Object, e As EventArgs)
        Dim rowsToDelete As New List(Of DataGridViewRow)()
        For Each row As DataGridViewRow In dgvAssets.Rows
            If row.IsNewRow Then Continue For
            If row.Cells("DeleteCheck").Value IsNot Nothing AndAlso
               CBool(row.Cells("DeleteCheck").Value) = True Then
                rowsToDelete.Add(row)
            End If
        Next

        If rowsToDelete.Count = 0 Then
            MessageBox.Show("削除する行を選択してください。", "確認",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show(rowsToDelete.Count.ToString() & "件の行を削除します。よろしいですか？",
                           "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            For Each row As DataGridViewRow In rowsToDelete
                dgvAssets.Rows.Remove(row)
            Next
            UpdateAssetCount()
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            FNT_LABEL?.Dispose()
            FNT_INPUT?.Dispose()
            FNT_SECTION?.Dispose()
            _boldFont?.Dispose()
            _regularFont?.Dispose()
            _font11Bold?.Dispose()
            _font18Bold?.Dispose()
            _fontResultBadge?.Dispose()
            _fontResultReason?.Dispose()
            _tooltipProvider?.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Function CalcMonthsBetween(startDt As DateTime, endDt As DateTime) As Integer
        Dim months As Integer = ((endDt.Year - startDt.Year) * 12) + (endDt.Month - startDt.Month)
        Dim tempDate As DateTime = startDt.AddMonths(months)
        If endDt < tempDate.AddDays(-1) Then
            months -= 1
        End If
        Return Math.Max(0, months)
    End Function

    Private Function CreateGridLabel(text As String) As Label
        Return New Label() With {
            .Text = text,
            .Dock = DockStyle.Fill,
            .BackColor = CLR_READONLY,
            .ForeColor = CLR_TEXT,
            .Font = FNT_LABEL,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Margin = New Padding(2)
        }
    End Function

End Class
