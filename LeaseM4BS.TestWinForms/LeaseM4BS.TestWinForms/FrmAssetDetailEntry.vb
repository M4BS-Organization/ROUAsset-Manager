Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class FrmAssetDetailEntry

    Public Property AssetCategory As String = ""
    Public Property InitAssetNo As String = ""
    Public Property IsReadOnly As Boolean = False
    Public Property PropertyAttributes As Dictionary(Of Integer, String) = Nothing

    Private _attrDefs As List(Of PropertyAttributeDef) = Nothing
    Private ReadOnly _dynamicControls As New Dictionary(Of Integer, Control)

    ' === 戻り値プロパティ ===
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
            Return ""
        End Get
    End Property
    Public ReadOnly Property CompanyNameValue As String
        Get
            Return ""
        End Get
    End Property
    Public ReadOnly Property RemarksValue As String
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DeptAllocationSummary As String
        Get
            Dim parts As New List(Of String)
            For Each row As DataGridViewRow In dgvDeptAllocation.Rows
                If row.IsNewRow Then Continue For
                Dim dn As String = If(row.Cells("DeptName").Value?.ToString(), "")
                Dim r As String = If(row.Cells("AllocationRatio").Value?.ToString(), "0")
                If dn <> "" Then parts.Add(dn & "(" & r & "%)")
            Next
            Return String.Join(",", parts)
        End Get
    End Property

    Public ReadOnly Property DeptAllocationList As List(Of CtbDeptAllocation)
        Get
            Dim result As New List(Of CtbDeptAllocation)
            For Each row As DataGridViewRow In dgvDeptAllocation.Rows
                If row.IsNewRow Then Continue For
                Dim dn As String = If(row.Cells("DeptName").Value?.ToString(), "")
                If dn <> "" Then
                    Dim a As New CtbDeptAllocation()
                    a.DeptCd = If(row.Cells("DeptCd").Value?.ToString(), "")
                    a.DeptName = dn
                    Dim d As Decimal
                    If Decimal.TryParse(If(row.Cells("AllocationRatio").Value?.ToString(), "0"), d) Then a.AllocationRatio = d
                    result.Add(a)
                End If
            Next
            Return result
        End Get
    End Property

    ' === 旧互換プロパティ（動的コントロールから取得） ===
    Public ReadOnly Property ReStructure As String
        Get
            Return GetAttrValue("re_structure")
        End Get
    End Property
    Public ReadOnly Property ReArea As String
        Get
            Return GetAttrValue("re_area")
        End Get
    End Property
    Public ReadOnly Property ReLayout As String
        Get
            Return GetAttrValue("re_layout")
        End Get
    End Property
    Public ReadOnly Property ReCompletionDate As Date
        Get
            Dim dt As Date
            If Date.TryParse(GetAttrValue("re_completion_date"), dt) Then Return dt
            Return Date.Today
        End Get
    End Property
    Public ReadOnly Property ReLandlordName As String
        Get
            Return GetAttrValue("re_landlord_name")
        End Get
    End Property
    Public ReadOnly Property ReBrokerCompany As String
        Get
            Return GetAttrValue("re_broker_company")
        End Get
    End Property
    Public ReadOnly Property ReUsageRestrictions As String
        Get
            Return GetAttrValue("re_usage_restrictions")
        End Get
    End Property
    Public ReadOnly Property VhChassisNo As String
        Get
            Return GetAttrValue("vh_chassis_no")
        End Get
    End Property
    Public ReadOnly Property VhRegistrationNo As String
        Get
            Return GetAttrValue("vh_registration_no")
        End Get
    End Property
    Public ReadOnly Property VhVehicleType As String
        Get
            Return GetAttrValue("vh_vehicle_type")
        End Get
    End Property
    Public ReadOnly Property VhInspectionDate As Date
        Get
            Dim dt As Date
            If Date.TryParse(GetAttrValue("vh_inspection_date"), dt) Then Return dt
            Return Date.Today
        End Get
    End Property
    Public ReadOnly Property VhMileageLimit As String
        Get
            Return GetAttrValue("vh_mileage_limit")
        End Get
    End Property
    Public ReadOnly Property OaModelNo As String
        Get
            Return GetAttrValue("oa_model_no")
        End Get
    End Property
    Public ReadOnly Property OaSerialNo As String
        Get
            Return GetAttrValue("oa_serial_no")
        End Get
    End Property
    Public ReadOnly Property OaMaintenanceDate As Date
        Get
            Dim dt As Date
            If Date.TryParse(GetAttrValue("oa_maintenance_date"), dt) Then Return dt
            Return Date.Today
        End Get
    End Property
    Public ReadOnly Property OaMaintenanceContract As String
        Get
            Return GetAttrValue("oa_maintenance_contract")
        End Get
    End Property

    Private Function GetAttrValue(attrCd As String) As String
        If _attrDefs Is Nothing Then Return ""
        For Each def In _attrDefs
            If def.AttrCd = attrCd AndAlso _dynamicControls.ContainsKey(def.AttrDefId) Then
                Return GetControlValue(_dynamicControls(def.AttrDefId), def.DataType)
            End If
        Next
        Return ""
    End Function

    ' =============================================
    ' Load
    ' =============================================
    Private Sub FrmAssetDetailEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not String.IsNullOrEmpty(InitAssetNo) Then txtAssetNo.Text = InitAssetNo
        lblAssetCategoryDisplay.Text = AssetCategory
        pnlRealEstate.Visible = False
        pnlVehicle.Visible = False
        pnlOfficeEquip.Visible = False
        BuildDynamicAttributePanel()
        UpdateCategorySpecificLabel()
        InitComboBoxes()
        InitDeptAllocationGrid()

        ' 物件種別ComboBox変更時に種別固有パネルを再生成
        AddHandler cmbBkind.SelectedIndexChanged, Sub(s, ev)
            If cmbBkind.SelectedItem IsNot Nothing Then
                AssetCategory = cmbBkind.SelectedItem.ToString()
                lblAssetCategoryDisplay.Text = AssetCategory
                BuildDynamicAttributePanel()
                UpdateCategorySpecificLabel()
            End If
        End Sub

        If IsReadOnly Then ApplyReadOnlyMode()
    End Sub

    ''' <summary>
    ''' 種別固有情報のGroupBoxラベルをカテゴリに応じて動的に変更する。
    ''' </summary>
    Private Sub UpdateCategorySpecificLabel()
        If grpCategorySpecific Is Nothing Then Return
        Dim categoryCd As String = ResolveAssetCategoryCd(AssetCategory)
        Try
            Dim crud As New CrudHelper()
            Dim result = crud.ExecuteScalar(Of Object)(
                "SELECT asset_category_nm FROM m_asset_category WHERE asset_category_cd = @cd",
                New List(Of Npgsql.NpgsqlParameter) From {
                    New Npgsql.NpgsqlParameter("@cd", categoryCd)
                })
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                grpCategorySpecific.Text = result.ToString() & "情報"
                Return
            End If
        Catch
        End Try
        grpCategorySpecific.Text = "種別固有情報"
    End Sub

    ' =============================================
    ' 動的属性パネル生成（EAV）
    ' =============================================
    Private Sub BuildDynamicAttributePanel()
        Dim categoryCd As String = ResolveAssetCategoryCd(AssetCategory)
        Try
            Dim repo As New PropertyRepository()
            _attrDefs = repo.GetAttributeDefs(categoryCd)
        Catch
            _attrDefs = New List(Of PropertyAttributeDef)
        End Try
        If _attrDefs.Count = 0 Then
            grpCategorySpecific.Visible = False
            Return
        End If
        grpCategorySpecific.Visible = True

        Dim tbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .ColumnCount = 4, .Padding = New Padding(3)
        }
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Dim rowIdx As Integer = 0, colIdx As Integer = 0
        For Each def As PropertyAttributeDef In _attrDefs
            If def.DisplayType = "TEXTAREA" Then
                If colIdx > 0 Then
                    rowIdx += 1
                    colIdx = 0
                End If
                tbl.RowCount = rowIdx + 1
                tbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
                tbl.Controls.Add(MakeLabel(def.AttrNm), 0, rowIdx)
                Dim ctrl As Control = CreateDynamicControl(def)
                tbl.Controls.Add(ctrl, 1, rowIdx)
                tbl.SetColumnSpan(ctrl, 3)
                _dynamicControls(def.AttrDefId) = ctrl
                rowIdx += 1
            Else
                If colIdx >= 4 Then
                    rowIdx += 1
                    colIdx = 0
                End If
                If colIdx = 0 Then
                    tbl.RowCount = rowIdx + 1
                    tbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
                End If
                tbl.Controls.Add(MakeLabel(def.AttrNm), colIdx, rowIdx)
                Dim ctrl As Control = CreateDynamicControl(def)
                tbl.Controls.Add(ctrl, colIdx + 1, rowIdx)
                _dynamicControls(def.AttrDefId) = ctrl
                colIdx += 2
            End If
        Next
        grpCategorySpecific.Controls.Add(tbl)
        tbl.BringToFront()
    End Sub

    Private Shared Function MakeLabel(text As String) As Label
        Return New Label() With {.Text = text, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleRight,
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(73, 80, 87)}
    End Function

    Private Shared Shadows Function CreateDynamicControl(def As PropertyAttributeDef) As Control
        Dim fnt As New Font("Meiryo", 9.75F)
        Select Case def.DisplayType.ToUpper()
            Case "DATEPICKER"
                Return New DateTimePicker() With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short, .Font = fnt, .ShowCheckBox = True, .Checked = False}
            Case "NUMERICUPDOWN"
                Return New NumericUpDown() With {.Dock = DockStyle.Fill, .Font = fnt, .Maximum = 999999999D, .DecimalPlaces = 2, .TextAlign = HorizontalAlignment.Right}
            Case "CHECKBOX"
                Return New CheckBox() With {.Dock = DockStyle.Fill, .Font = fnt}
            Case "TEXTAREA"
                Dim t As New TextBox() With {.Dock = DockStyle.Fill, .Font = fnt, .Multiline = True, .Height = 60, .ScrollBars = ScrollBars.Vertical}
                If def.MaxLength.HasValue Then t.MaxLength = def.MaxLength.Value
                Return t
            Case Else
                Dim t As New TextBox() With {.Dock = DockStyle.Fill, .Font = fnt}
                If def.MaxLength.HasValue Then t.MaxLength = def.MaxLength.Value
                Return t
        End Select
    End Function

    Private Shared Function GetControlValue(ctrl As Control, dataType As String) As String
        If TypeOf ctrl Is DateTimePicker Then
            Dim dtp = DirectCast(ctrl, DateTimePicker)
            If Not dtp.Checked Then Return ""
            Return dtp.Value.ToString("yyyy-MM-dd")
        ElseIf TypeOf ctrl Is NumericUpDown Then
            Return DirectCast(ctrl, NumericUpDown).Value.ToString()
        ElseIf TypeOf ctrl Is CheckBox Then
            Return If(DirectCast(ctrl, CheckBox).Checked, "true", "false")
        Else
            Return ctrl.Text
        End If
    End Function

    Private Sub CollectPropertyAttributes()
        PropertyAttributes = New Dictionary(Of Integer, String)
        For Each kvp In _dynamicControls
            Dim dataType As String = "TEXT"
            If _attrDefs IsNot Nothing Then
                For Each def In _attrDefs
                    If def.AttrDefId = kvp.Key Then
                        dataType = def.DataType
                        Exit For
                    End If
                Next
            End If
            Dim val As String = GetControlValue(kvp.Value, dataType)
            If val <> "" Then PropertyAttributes(kvp.Key) = val
        Next
    End Sub

    Private Shared Function ResolveAssetCategoryCd(category As String) As String
        ' ACコードが直接渡された場合はそのまま返す
        If Not String.IsNullOrEmpty(category) AndAlso category.StartsWith("AC") Then Return category

        ' m_bkind.asset_category_cd から取得（ハードコード廃止）
        Try
            Dim crud As New CrudHelper()
            Dim result = crud.ExecuteScalar(Of Object)(
                "SELECT asset_category_cd FROM m_bkind WHERE bkind_nm = @nm AND history_f IS NOT TRUE LIMIT 1",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@nm", category)
                })
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then Return result.ToString()
        Catch
        End Try

        ' DB接続不可時のみデフォルト
        Return "AC05"
    End Function

    ' =============================================
    ' コンボボックス・配賦グリッド
    ' =============================================
    Private Sub InitComboBoxes()
        ' cmbBkind に m_bkind からデータをロード
        cmbBkind.Items.Clear()
        Try
            Dim crud As New CrudHelper()
            Dim dt = crud.GetDataTable(
                "SELECT bkind_nm FROM m_bkind WHERE history_f IS NOT TRUE ORDER BY bkind_id",
                New List(Of NpgsqlParameter))
            For Each row As Data.DataRow In dt.Rows
                cmbBkind.Items.Add(row("bkind_nm").ToString())
            Next
        Catch
        End Try
        If cmbBkind.Items.Count = 0 Then
            cmbBkind.Items.AddRange(New String() {"不動産", "車両", "OA機器", "機械装置", "その他"})
        End If

        ' AssetCategory で初期選択
        If Not String.IsNullOrEmpty(AssetCategory) Then
            Dim idx As Integer = cmbBkind.Items.IndexOf(AssetCategory)
            If idx >= 0 Then cmbBkind.SelectedIndex = idx
        End If
    End Sub

    Private _deptTable As Data.DataTable
    Private _deptNameList As String() = {}

    Private Sub InitDeptAllocationGrid()
        dgvDeptAllocation.Columns.Clear()
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
        dgvDeptAllocation.Columns.Add(New DataGridViewTextBoxColumn() With {.Name = "DeptCd", .Visible = False})
        Dim cmbCol As New DataGridViewComboBoxColumn() With {.HeaderText = "部門", .Name = "DeptName", .Width = 200, .MinimumWidth = 120, .FlatStyle = FlatStyle.Flat}
        cmbCol.Items.AddRange(_deptNameList)
        dgvDeptAllocation.Columns.Add(cmbCol)
        dgvDeptAllocation.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "配賦率(%)", .Name = "AllocationRatio", .Width = 120, .MinimumWidth = 80,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N2"}})
        AddHandler dgvDeptAllocation.CellValueChanged, AddressOf OnDeptCellValueChanged
        AddHandler dgvDeptAllocation.CurrentCellDirtyStateChanged, Sub(s, ev)
                                                                        If dgvDeptAllocation.IsCurrentCellDirty Then dgvDeptAllocation.CommitEdit(DataGridViewDataErrorContexts.Commit)
                                                                    End Sub
        If _deptTable.Rows.Count > 0 Then
            dgvDeptAllocation.Rows.Add(_deptTable.Rows(0)("dept_cd").ToString(), _deptNameList(0), 100.00D)
        Else
            dgvDeptAllocation.Rows.Add("", "", 100.00D)
        End If
        UpdateAllocationTotal()
    End Sub

    Private Sub OnDeptCellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvDeptAllocation.Columns(e.ColumnIndex).Name <> "DeptName" Then Return
        Dim dn As String = If(dgvDeptAllocation.Rows(e.RowIndex).Cells("DeptName").Value?.ToString(), "")
        Dim cd As String = ""
        If _deptTable IsNot Nothing Then
            For Each row As Data.DataRow In _deptTable.Rows
                If row("dept_nm").ToString() = dn Then
                    cd = row("dept_cd").ToString()
                    Exit For
                End If
            Next
        End If
        dgvDeptAllocation.Rows(e.RowIndex).Cells("DeptCd").Value = cd
    End Sub

    Private Sub btnAddDept_Click(sender As Object, e As EventArgs) Handles btnAddDept.Click
        dgvDeptAllocation.Rows.Add("", "", 0D)
    End Sub
    Private Sub btnRemoveDept_Click(sender As Object, e As EventArgs) Handles btnRemoveDept.Click
        If dgvDeptAllocation.SelectedRows.Count > 0 Then
            For Each row As DataGridViewRow In dgvDeptAllocation.SelectedRows
                If Not row.IsNewRow Then dgvDeptAllocation.Rows.Remove(row)
            Next
            UpdateAllocationTotal()
        End If
    End Sub
    Private Sub dgvDeptAllocation_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDeptAllocation.CellValueChanged
        If e.RowIndex >= 0 Then UpdateAllocationTotal()
    End Sub
    Private Sub dgvDeptAllocation_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvDeptAllocation.CurrentCellDirtyStateChanged
        If dgvDeptAllocation.IsCurrentCellDirty Then dgvDeptAllocation.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub UpdateAllocationTotal()
        Dim total As Decimal = 0D
        For Each row As DataGridViewRow In dgvDeptAllocation.Rows
            If row.IsNewRow Then Continue For
            Dim val As Object = row.Cells("AllocationRatio").Value
            If val IsNot Nothing Then
                Dim d As Decimal
                If Decimal.TryParse(val.ToString(), d) Then total += d
            End If
        Next
        lblAllocationTotal.Text = String.Format("配賦率合計: {0:N2}%", total)
        lblAllocationTotal.ForeColor = If(total = 100D, Color.FromArgb(40, 167, 69), Color.FromArgb(220, 53, 69))
    End Sub

    ' =============================================
    ' ボタン
    ' =============================================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtAssetName.Text) Then
            MessageBox.Show("資産名を入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAssetName.Focus()
            Return
        End If
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
            MessageBox.Show("配賦率の合計が100%になるように設定してください。" & vbCrLf & String.Format("現在の合計: {0:N2}%", total),
                            "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        CollectPropertyAttributes()
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
        Dim rc As Color = Color.FromArgb(233, 236, 239)
        txtAssetName.ReadOnly = True
        txtAssetName.BackColor = rc
        If cmbBkind IsNot Nothing Then cmbBkind.Enabled = False
        If numSuuryo IsNot Nothing Then numSuuryo.Enabled = False
        If txtKedaban IsNot Nothing Then txtKedaban.ReadOnly = True : txtKedaban.BackColor = rc
        If dtpSettiDt IsNot Nothing Then dtpSettiDt.Enabled = False
        dgvDeptAllocation.ReadOnly = True
        btnAddDept.Visible = False
        btnRemoveDept.Visible = False
        For Each kvp In _dynamicControls
            If TypeOf kvp.Value Is TextBox Then
                DirectCast(kvp.Value, TextBox).ReadOnly = True
                kvp.Value.BackColor = rc
            End If
            If TypeOf kvp.Value Is DateTimePicker Then DirectCast(kvp.Value, DateTimePicker).Enabled = False
            If TypeOf kvp.Value Is NumericUpDown Then
                DirectCast(kvp.Value, NumericUpDown).ReadOnly = True
                kvp.Value.BackColor = rc
            End If
            If TypeOf kvp.Value Is CheckBox Then DirectCast(kvp.Value, CheckBox).Enabled = False
        Next
        pnlRealEstate.Visible = False
        pnlVehicle.Visible = False
        pnlOfficeEquip.Visible = False
        btnAdd.Visible = False
        Me.Text = "資産詳細（照会）"
    End Sub

End Class
