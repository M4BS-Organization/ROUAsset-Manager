Imports System
Imports System.Drawing
Imports System.Windows.Forms

Partial Public Class FrmAssetDetailEntry

    ''' <summary>
    ''' 資産種別（"不動産"/"車両"/"OA機器"）。契約画面から設定される。
    ''' </summary>
    Public Property AssetCategory As String = ""

    ''' <summary>
    ''' 新規登録時に外部から設定される資産番号（自動採番値）
    ''' </summary>
    Public Property InitAssetNo As String = ""

    ''' <summary>
    ''' 照会モード
    ''' </summary>
    Public Property IsReadOnly As Boolean = False

    ' === 契約画面への戻り値プロパティ（ReadOnly） ===

    Public ReadOnly Property AssetNo As String
        Get
            Return txtAssetNo.Text
        End Get
    End Property

    Public ReadOnly Property AssetName As String
        Get
            Return txtAssetName.Text
        End Get
    End Property

    Public ReadOnly Property InstallLocation As String
        Get
            Return txtInstallLocation.Text
        End Get
    End Property

    ''' <summary>
    ''' 配賦部門のサマリ文字列（例: "総務部(50%),経理部(50%)"）
    ''' </summary>
    Public ReadOnly Property DeptAllocationSummary As String
        Get
            Dim parts As New System.Collections.Generic.List(Of String)
            For Each row As DataGridViewRow In dgvDeptAllocation.Rows
                If row.IsNewRow Then Continue For
                Dim deptName As String = If(row.Cells("DeptName").Value?.ToString(), "")
                Dim ratio As String = If(row.Cells("AllocationRatio").Value?.ToString(), "0")
                If Not String.IsNullOrEmpty(deptName) Then
                    parts.Add(String.Format("{0}({1}%)", deptName, ratio))
                End If
            Next
            Return String.Join(",", parts)
        End Get
    End Property

    ''' <summary>
    ''' 配賦明細をCtbDeptAllocationリストとして返す
    ''' </summary>
    Public ReadOnly Property DeptAllocationList As System.Collections.Generic.List(Of CtbDeptAllocation)
        Get
            Dim result As New System.Collections.Generic.List(Of CtbDeptAllocation)
            For Each row As DataGridViewRow In dgvDeptAllocation.Rows
                If row.IsNewRow Then Continue For
                Dim deptName As String = If(row.Cells("DeptName").Value?.ToString(), "")
                If Not String.IsNullOrEmpty(deptName) Then
                    Dim alloc As New CtbDeptAllocation()
                    alloc.DeptCd = If(row.Cells("DeptCd").Value?.ToString(), "")
                    alloc.DeptName = deptName
                    Dim d As Decimal
                    If Decimal.TryParse(If(row.Cells("AllocationRatio").Value?.ToString(), "0"), d) Then
                        alloc.AllocationRatio = d
                    End If
                    result.Add(alloc)
                End If
            Next
            Return result
        End Get
    End Property

    ' === 種別固有データの戻り値プロパティ ===

    ' 不動産
    Public ReadOnly Property ReStructure As String
        Get
            Return txtStructure.Text
        End Get
    End Property
    Public ReadOnly Property ReArea As String
        Get
            Return txtArea.Text
        End Get
    End Property
    Public ReadOnly Property ReLayout As String
        Get
            Return txtLayout.Text
        End Get
    End Property
    Public ReadOnly Property ReCompletionDate As Date
        Get
            Return dtpCompletion.Value
        End Get
    End Property
    Public ReadOnly Property ReLandlordName As String
        Get
            Return txtLandlordName.Text
        End Get
    End Property
    Public ReadOnly Property ReBrokerCompany As String
        Get
            Return txtBrokerCompany.Text
        End Get
    End Property
    Public ReadOnly Property ReUsageRestrictions As String
        Get
            Return txtUsageRestrictions.Text
        End Get
    End Property

    ' 車両
    Public ReadOnly Property VhChassisNo As String
        Get
            Return txtChassisNo.Text
        End Get
    End Property
    Public ReadOnly Property VhRegistrationNo As String
        Get
            Return txtRegistrationNo.Text
        End Get
    End Property
    Public ReadOnly Property VhVehicleType As String
        Get
            Return txtVehicleType.Text
        End Get
    End Property
    Public ReadOnly Property VhInspectionDate As Date
        Get
            Return dtpInspectionDate.Value
        End Get
    End Property
    Public ReadOnly Property VhMileageLimit As String
        Get
            Return txtMileageLimit.Text
        End Get
    End Property

    ' OA機器
    Public ReadOnly Property OaModelNo As String
        Get
            Return txtModelNo.Text
        End Get
    End Property
    Public ReadOnly Property OaSerialNo As String
        Get
            Return txtSerialNo.Text
        End Get
    End Property
    Public ReadOnly Property OaMaintenanceDate As Date
        Get
            Return dtpMaintenanceDate.Value
        End Get
    End Property
    Public ReadOnly Property OaMaintenanceContract As String
        Get
            Return txtMaintenanceContract.Text
        End Get
    End Property

    ' 会社名
    Public ReadOnly Property CompanyNameValue As String
        Get
            If cmbCompany.SelectedItem IsNot Nothing Then
                Return cmbCompany.SelectedItem.ToString()
            End If
            Return ""
        End Get
    End Property

    ' 備考
    Public ReadOnly Property RemarksValue As String
        Get
            Return txtRemarks.Text
        End Get
    End Property

    ' =============================================
    ' イベントハンドラ
    ' =============================================

    Private Sub FrmAssetDetailEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 資産番号セット
        If Not String.IsNullOrEmpty(InitAssetNo) Then
            txtAssetNo.Text = InitAssetNo
        End If

        ' 資産種類表示
        lblAssetCategoryDisplay.Text = AssetCategory

        ' 種別固有パネル切替
        SwitchCategoryPanel(AssetCategory)

        ' コンボボックス初期化
        InitComboBoxes()

        ' 配賦グリッド初期化
        InitDeptAllocationGrid()

        ' 読取専用モード
        If IsReadOnly Then ApplyReadOnlyMode()
    End Sub

    ''' <summary>
    ''' 資産種別に応じて固有入力パネルを切り替える
    ''' </summary>
    Private Sub SwitchCategoryPanel(category As String)
        pnlRealEstate.Visible = (category = "不動産")
        pnlVehicle.Visible = (category = "車両")
        pnlOfficeEquip.Visible = (category = "OA機器")
    End Sub

    ''' <summary>
    ''' コンボボックスの選択肢を初期化する
    ''' </summary>
    Private Sub InitComboBoxes()
        cmbCompany.Items.AddRange(New String() {"本社", "大阪支店", "名古屋支店"})
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
    End Sub

    ' =============================================
    ' 配賦グリッド
    ' =============================================

    Private _deptTable As Data.DataTable
    Private _deptNameList As String() = {}

    ''' <summary>
    ''' 配賦グリッドの列を初期化する（DBマスタからコード＋名称を取得）
    ''' </summary>
    Private Sub InitDeptAllocationGrid()
        dgvDeptAllocation.Columns.Clear()

        ' DBから部門マスタ取得
        Try
            Dim mdl As New LeaseM4BS.DataAccess.MasterDataLoader()
            _deptTable = mdl.LoadDepartments()
            mdl.Dispose()
        Catch
            _deptTable = New Data.DataTable()
            _deptTable.Columns.Add("dept_cd", GetType(String))
            _deptTable.Columns.Add("dept_nm", GetType(String))
        End Try
        ReDim _deptNameList(_deptTable.Rows.Count - 1)
        For i As Integer = 0 To _deptTable.Rows.Count - 1
            _deptNameList(i) = _deptTable.Rows(i)("dept_nm").ToString()
        Next

        ' 隠し部門コード列
        dgvDeptAllocation.Columns.Add(New DataGridViewTextBoxColumn() With {
            .Name = "DeptCd",
            .Visible = False
        })

        ' 部門名（コンボボックス列）
        Dim cmbCol As New DataGridViewComboBoxColumn()
        cmbCol.HeaderText = "部門"
        cmbCol.Name = "DeptName"
        cmbCol.Width = 200
        cmbCol.MinimumWidth = 120
        cmbCol.Items.AddRange(_deptNameList)
        cmbCol.FlatStyle = FlatStyle.Flat
        dgvDeptAllocation.Columns.Add(cmbCol)

        ' 配賦率
        dgvDeptAllocation.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "配賦率(%)",
            .Name = "AllocationRatio",
            .Width = 120,
            .MinimumWidth = 80,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight,
                .Format = "N2"
            }
        })

        ' 部門名変更時に隠しコード列を自動更新
        AddHandler dgvDeptAllocation.CellValueChanged, AddressOf OnDeptCellValueChanged
        AddHandler dgvDeptAllocation.CurrentCellDirtyStateChanged, Sub(s, ev)
                                                                        If dgvDeptAllocation.IsCurrentCellDirty Then
                                                                            dgvDeptAllocation.CommitEdit(DataGridViewDataErrorContexts.Commit)
                                                                        End If
                                                                    End Sub

        ' デフォルト1行追加（最初の部門）
        If _deptTable.Rows.Count > 0 Then
            dgvDeptAllocation.Rows.Add(_deptTable.Rows(0)("dept_cd").ToString(), _deptNameList(0), 100.00D)
        Else
            dgvDeptAllocation.Rows.Add("", "", 100.00D)
        End If
        UpdateAllocationTotal()
    End Sub

    ''' <summary>
    ''' 部門名コンボ変更時に隠しDeptCd列を連動更新
    ''' </summary>
    Private Sub OnDeptCellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        If dgvDeptAllocation.Columns(e.ColumnIndex).Name <> "DeptName" Then Return

        Dim deptName As String = If(dgvDeptAllocation.Rows(e.RowIndex).Cells("DeptName").Value?.ToString(), "")
        Dim deptCd As String = ""
        If _deptTable IsNot Nothing Then
            For Each row As Data.DataRow In _deptTable.Rows
                If row("dept_nm").ToString() = deptName Then
                    deptCd = row("dept_cd").ToString()
                    Exit For
                End If
            Next
        End If
        dgvDeptAllocation.Rows(e.RowIndex).Cells("DeptCd").Value = deptCd
    End Sub

    Private Sub btnAddDept_Click(sender As Object, e As EventArgs) Handles btnAddDept.Click
        dgvDeptAllocation.Rows.Add("", "", 0D)
    End Sub

    Private Sub btnRemoveDept_Click(sender As Object, e As EventArgs) Handles btnRemoveDept.Click
        If dgvDeptAllocation.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In dgvDeptAllocation.SelectedRows
                If Not row.IsNewRow Then
                    dgvDeptAllocation.Rows.Remove(row)
                End If
            Next
            UpdateAllocationTotal()
        End If
    End Sub

    Private Sub dgvDeptAllocation_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDeptAllocation.CellValueChanged
        If e.RowIndex >= 0 Then
            UpdateAllocationTotal()
        End If
    End Sub

    Private Sub dgvDeptAllocation_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvDeptAllocation.CurrentCellDirtyStateChanged
        If dgvDeptAllocation.IsCurrentCellDirty Then
            dgvDeptAllocation.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    ''' <summary>
    ''' 配賦率合計を更新する
    ''' </summary>
    Private Sub UpdateAllocationTotal()
        Dim total As Decimal = 0D
        For Each row As DataGridViewRow In dgvDeptAllocation.Rows
            If row.IsNewRow Then Continue For
            Dim val As Object = row.Cells("AllocationRatio").Value
            If val IsNot Nothing Then
                Dim d As Decimal
                If Decimal.TryParse(val.ToString(), d) Then
                    total += d
                End If
            End If
        Next
        lblAllocationTotal.Text = String.Format("配賦率合計: {0:N2}%", total)

        If total = 100D Then
            lblAllocationTotal.ForeColor = Color.FromArgb(40, 167, 69)
        Else
            lblAllocationTotal.ForeColor = Color.FromArgb(220, 53, 69)
        End If
    End Sub

    ' =============================================
    ' ボタン
    ' =============================================

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' バリデーション
        If String.IsNullOrWhiteSpace(txtAssetName.Text) Then
            MessageBox.Show("資産名を入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAssetName.Focus()
            Return
        End If

        ' 配賦率合計チェック
        Dim total As Decimal = 0D
        For Each row As DataGridViewRow In dgvDeptAllocation.Rows
            If row.IsNewRow Then Continue For
            Dim val As Object = row.Cells("AllocationRatio").Value
            If val IsNot Nothing Then
                Dim d As Decimal
                If Decimal.TryParse(val.ToString(), d) Then total += d
            End If
        Next
        If total <> 100D Then
            MessageBox.Show("配賦率の合計が100%になるように設定してください。" & vbCrLf &
                            String.Format("現在の合計: {0:N2}%", total),
                            "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' =============================================
    ' ReadOnlyモード
    ' =============================================

    Private Sub ApplyReadOnlyMode()
        Dim readOnlyColor As Color = Color.FromArgb(233, 236, 239)

        ' 基本情報
        txtAssetName.ReadOnly = True
        txtAssetName.BackColor = readOnlyColor
        cmbCompany.Enabled = False
        txtInstallLocation.ReadOnly = True
        txtInstallLocation.BackColor = readOnlyColor
        txtRemarks.ReadOnly = True
        txtRemarks.BackColor = readOnlyColor

        ' 配賦グリッド
        dgvDeptAllocation.ReadOnly = True
        btnAddDept.Visible = False
        btnRemoveDept.Visible = False

        ' 不動産固有
        txtStructure.ReadOnly = True
        txtStructure.BackColor = readOnlyColor
        txtArea.ReadOnly = True
        txtArea.BackColor = readOnlyColor
        txtLayout.ReadOnly = True
        txtLayout.BackColor = readOnlyColor
        dtpCompletion.Enabled = False
        txtLandlordName.ReadOnly = True
        txtLandlordName.BackColor = readOnlyColor
        txtBrokerCompany.ReadOnly = True
        txtBrokerCompany.BackColor = readOnlyColor
        txtUsageRestrictions.ReadOnly = True
        txtUsageRestrictions.BackColor = readOnlyColor

        ' 車両固有
        txtChassisNo.ReadOnly = True
        txtChassisNo.BackColor = readOnlyColor
        txtRegistrationNo.ReadOnly = True
        txtRegistrationNo.BackColor = readOnlyColor
        txtVehicleType.ReadOnly = True
        txtVehicleType.BackColor = readOnlyColor
        dtpInspectionDate.Enabled = False
        txtMileageLimit.ReadOnly = True
        txtMileageLimit.BackColor = readOnlyColor

        ' OA機器固有
        txtModelNo.ReadOnly = True
        txtModelNo.BackColor = readOnlyColor
        txtSerialNo.ReadOnly = True
        txtSerialNo.BackColor = readOnlyColor
        dtpMaintenanceDate.Enabled = False
        txtMaintenanceContract.ReadOnly = True
        txtMaintenanceContract.BackColor = readOnlyColor

        ' 追加ボタン非表示
        btnAdd.Visible = False
        Me.Text = "資産詳細（照会）"
    End Sub

End Class
