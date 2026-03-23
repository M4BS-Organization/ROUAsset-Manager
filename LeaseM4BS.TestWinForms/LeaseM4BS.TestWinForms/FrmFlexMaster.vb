Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class FrmFlexMaster
    Inherits UserControl

    Private ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
    Private ReadOnly CLR_CARD As Color = Color.White
    Private ReadOnly CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
    Private ReadOnly CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
    Private ReadOnly CLR_BORDER As Color = Color.FromArgb(222, 226, 230)
    Private ReadOnly CLR_ACCENT As Color = Color.FromArgb(0, 123, 255)
    Private ReadOnly CLR_SUCCESS As Color = Color.FromArgb(40, 167, 69)
    Private ReadOnly CLR_BG As Color = Color.FromArgb(248, 249, 250)

    Private ReadOnly FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)
    Private ReadOnly FNT_INPUT As New Font("Meiryo", 9.75F, FontStyle.Regular)
    Private ReadOnly FNT_SECTION As New Font("Meiryo", 10.0F, FontStyle.Bold)

    Private lstTables As ListBox
    Private dgvData As DataGridView
    Private pnlInput As Panel
    Private tblInput As TableLayoutPanel
    Private btnInsert As Button
    Private btnRefresh As Button
    Private btnDelete As Button
    Private lblStatus As Label
    Private lblTableName As Label

    Private _crud As CrudHelper
    Private _isConnected As Boolean = False
    Private _currentTable As String = ""

    Private Structure TableDef
        Public TableName As String
        Public DisplayName As String
        Public Columns() As ColumnDef
    End Structure

    Private Structure ColumnDef
        Public Name As String
        Public DisplayName As String
        Public DbType As String
        Public IsRequired As Boolean
        Public IsAutoGen As Boolean
    End Structure

    Private _tables() As TableDef
    Private _inputControls As New Dictionary(Of String, Control)

    Public Sub New()
        InitializeComponent()
        Me.BackColor = CLR_BG
        Me.Padding = New Padding(8, 0, 8, 8)

        InitTableDefs()
        BuildUI()

        If lstTables.Items.Count > 0 Then
            lstTables.SelectedIndex = 0
        End If

        InitDbConnectionAsync()
    End Sub

    Private Async Sub InitDbConnectionAsync()
        lblStatus.Text = "DB: Connecting..."
        lblStatus.ForeColor = CLR_LABEL
        btnInsert.Enabled = False
        btnDelete.Enabled = False

        Try
            Dim crud = Await Task.Run(Function() New CrudHelper())
            _crud = crud
            _isConnected = True

            lblStatus.Text = "DB: Connected"
            lblStatus.ForeColor = CLR_SUCCESS
            btnInsert.Enabled = True
            btnDelete.Enabled = True

            If lstTables.SelectedIndex >= 0 Then
                LoadTableData(_tables(lstTables.SelectedIndex))
            End If
        Catch
            _isConnected = False
            lblStatus.Text = "DB: Not connected"
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub InitTableDefs()
        _tables = New TableDef() {
            New TableDef() With {
                .TableName = "m_company",
                .DisplayName = "m_company",
                .Columns = New ColumnDef() {
                    Col("company_cd", "company_cd", "VARCHAR", True),
                    Col("company_nm", "company_nm", "VARCHAR", True),
                    Col("company_cd2", "company_cd2", "VARCHAR", False),
                    Col("company_nm2", "company_nm2", "VARCHAR", False),
                    Col("company_cd3", "company_cd3", "VARCHAR", False),
                    Col("company_nm3", "company_nm3", "VARCHAR", False),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_supplier",
                .DisplayName = "m_supplier",
                .Columns = New ColumnDef() {
                    Col("supplier_cd", "supplier_cd", "VARCHAR", True),
                    Col("supplier_nm", "supplier_nm", "VARCHAR", True),
                    Col("supplier_cd2", "supplier_cd2", "VARCHAR", False),
                    Col("supplier_nm2", "supplier_nm2", "VARCHAR", False),
                    Col("row1_contract_closing_day", "row1_contract_closing_day", "SMALLINT", False),
                    Col("row1_first_pay_months", "row1_first_pay_months", "SMALLINT", False),
                    Col("row1_first_pay_day", "row1_first_pay_day", "SMALLINT", False),
                    Col("row1_second_pay_months", "row1_second_pay_months", "SMALLINT", False),
                    Col("row1_second_pay_day", "row1_second_pay_day", "SMALLINT", False),
                    Col("row2_contract_closing_day", "row2_contract_closing_day", "SMALLINT", False),
                    Col("row2_first_pay_months", "row2_first_pay_months", "SMALLINT", False),
                    Col("row2_first_pay_day", "row2_first_pay_day", "SMALLINT", False),
                    Col("row2_second_pay_months", "row2_second_pay_months", "SMALLINT", False),
                    Col("row2_second_pay_day", "row2_second_pay_day", "SMALLINT", False),
                    Col("row3_contract_closing_day", "row3_contract_closing_day", "SMALLINT", False),
                    Col("row3_first_pay_months", "row3_first_pay_months", "SMALLINT", False),
                    Col("row3_first_pay_day", "row3_first_pay_day", "SMALLINT", False),
                    Col("row3_second_pay_months", "row3_second_pay_months", "SMALLINT", False),
                    Col("row3_second_pay_day", "row3_second_pay_day", "SMALLINT", False),
                    Col("re_lease_param", "re_lease_param", "SMALLINT", False),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_payment_method",
                .DisplayName = "m_payment_method",
                .Columns = New ColumnDef() {
                    Col("payment_method_cd", "payment_method_cd", "VARCHAR", True),
                    Col("payment_method_nm", "payment_method_nm", "VARCHAR", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_department",
                .DisplayName = "m_department",
                .Columns = New ColumnDef() {
                    Col("dept_cd", "dept_cd", "VARCHAR", True),
                    Col("dept_nm", "dept_nm", "VARCHAR", True),
                    Col("dept_cd2", "dept_cd2", "VARCHAR", False),
                    Col("dept_nm2", "dept_nm2", "VARCHAR", False),
                    Col("dept_cd3", "dept_cd3", "VARCHAR", False),
                    Col("dept_nm3", "dept_nm3", "VARCHAR", False),
                    Col("dept_cd4", "dept_cd4", "VARCHAR", False),
                    Col("dept_nm4", "dept_nm4", "VARCHAR", False),
                    Col("dept_cd5", "dept_cd5", "VARCHAR", False),
                    Col("dept_nm5", "dept_nm5", "VARCHAR", False),
                    Col("cost_category_nm", "cost_category_nm", "VARCHAR", False),
                    Col("agg_category1_cd", "agg_category1_cd", "VARCHAR", False),
                    Col("agg_category1_nm", "agg_category1_nm", "VARCHAR", False),
                    Col("agg_category2_cd", "agg_category2_cd", "VARCHAR", False),
                    Col("agg_category2_nm", "agg_category2_nm", "VARCHAR", False),
                    Col("agg_category3_cd", "agg_category3_cd", "VARCHAR", False),
                    Col("agg_category3_nm", "agg_category3_nm", "VARCHAR", False),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_asset_category",
                .DisplayName = "m_asset_category",
                .Columns = New ColumnDef() {
                    Col("asset_category_cd", "asset_category_cd", "VARCHAR", True),
                    Col("asset_category_nm", "asset_category_nm", "VARCHAR", True),
                    Col("note_asset_account_cd", "note_asset_account_cd", "VARCHAR", False),
                    Col("asset_account_cd", "asset_account_cd", "VARCHAR", False),
                    Col("accum_account_cd", "accum_account_cd", "VARCHAR", False),
                    Col("impair_accum_account_cd", "impair_accum_account_cd", "VARCHAR", False),
                    Col("liability_account_cd", "liability_account_cd", "VARCHAR", False),
                    Col("unpaid_tax_account_cd", "unpaid_tax_account_cd", "VARCHAR", False),
                    Col("impair_account_cd", "impair_account_cd", "VARCHAR", False),
                    Col("liability_account_1y_cd", "liability_account_1y_cd", "VARCHAR", False),
                    Col("unpaid_tax_account_1y_cd", "unpaid_tax_account_1y_cd", "VARCHAR", False),
                    Col("impair_account_1y_cd", "impair_account_1y_cd", "VARCHAR", False),
                    Col("cost_item_cd", "cost_item_cd", "VARCHAR", False),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_bank_account",
                .DisplayName = "m_bank_account",
                .Columns = New ColumnDef() {
                    Col("bank_account_cd", "bank_account_cd", "VARCHAR", True),
                    Col("bank_account_nm", "bank_account_nm", "VARCHAR", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_contract_type",
                .DisplayName = "m_contract_type",
                .Columns = New ColumnDef() {
                    Col("contract_type_cd", "contract_type_cd", "VARCHAR", True),
                    Col("contract_type_nm", "contract_type_nm", "VARCHAR", True),
                    Col("sort_order", "sort_order", "SMALLINT", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_initial_cost_item",
                .DisplayName = "m_initial_cost_item",
                .Columns = New ColumnDef() {
                    Col("cost_item_cd", "cost_item_cd", "VARCHAR", True),
                    Col("cost_item_nm", "cost_item_nm", "VARCHAR", True),
                    Col("sort_order", "sort_order", "SMALLINT", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_acct_treatment",
                .DisplayName = "m_acct_treatment",
                .Columns = New ColumnDef() {
                    Col("acct_treatment_cd", "acct_treatment_cd", "VARCHAR", True),
                    Col("acct_treatment_nm", "acct_treatment_nm", "VARCHAR", True),
                    Col("sort_order", "sort_order", "SMALLINT", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            },
            New TableDef() With {
                .TableName = "m_monthly_item",
                .DisplayName = "m_monthly_item",
                .Columns = New ColumnDef() {
                    Col("monthly_item_cd", "monthly_item_cd", "VARCHAR", True),
                    Col("monthly_item_nm", "monthly_item_nm", "VARCHAR", True),
                    Col("sort_order", "sort_order", "SMALLINT", True),
                    Col("remarks", "remarks", "VARCHAR", False),
                    Col("id", "id", "SERIAL", False, True)
                }
            }
        }
    End Sub

    Private Function Col(name As String, displayName As String, dbType As String, isRequired As Boolean, Optional isAutoGen As Boolean = False) As ColumnDef
        Return New ColumnDef() With {
            .Name = name,
            .DisplayName = displayName,
            .DbType = dbType,
            .IsRequired = isRequired,
            .IsAutoGen = isAutoGen
        }
    End Function

    Private Sub BuildUI()
        Dim splitMain As New SplitContainer() With {
            .Dock = DockStyle.Fill,
            .Orientation = Orientation.Vertical,
            .SplitterDistance = 200,
            .FixedPanel = FixedPanel.Panel1,
            .BackColor = CLR_BG
        }

        ' Left panel: table list
        Dim grpTables As New GroupBox() With {
            .Dock = DockStyle.Fill,
            .Text = "Tables",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .BackColor = CLR_CARD,
            .Padding = New Padding(6, 16, 6, 6)
        }

        lstTables = New ListBox() With {
            .Dock = DockStyle.Fill,
            .Font = FNT_INPUT,
            .BorderStyle = BorderStyle.None,
            .BackColor = CLR_CARD
        }

        For Each tbl As TableDef In _tables
            lstTables.Items.Add(tbl.DisplayName)
        Next

        AddHandler lstTables.SelectedIndexChanged, AddressOf OnTableSelected
        grpTables.Controls.Add(lstTables)
        splitMain.Panel1.Controls.Add(grpTables)

        ' Right panel: data grid + input form
        Dim rightPanel As New Panel() With {.Dock = DockStyle.Fill}

        ' Status label
        lblStatus = New Label() With {
            .Dock = DockStyle.Bottom,
            .Height = 28,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .BackColor = Color.FromArgb(230, 240, 250),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(8, 0, 0, 0)
        }
        If Not _isConnected Then
            lblStatus.Text = "DB: Not connected"
            lblStatus.ForeColor = Color.Red
        Else
            lblStatus.Text = "DB: Connected"
        End If

        ' Table name header
        lblTableName = New Label() With {
            .Dock = DockStyle.Top,
            .Height = 36,
            .Font = New Font("Meiryo", 12.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER,
            .BackColor = CLR_CARD,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Padding = New Padding(8, 0, 0, 0),
            .Text = ""
        }

        ' Buttons panel
        Dim pnlButtons As New FlowLayoutPanel() With {
            .Dock = DockStyle.Top,
            .Height = 40,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(4, 4, 0, 4),
            .BackColor = CLR_CARD
        }

        btnInsert = New Button() With {
            .Text = "INSERT",
            .Width = 100, .Height = 30,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = CLR_SUCCESS,
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand,
            .Margin = New Padding(0, 0, 4, 0)
        }
        btnInsert.FlatAppearance.BorderSize = 0
        AddHandler btnInsert.Click, AddressOf OnInsertClick

        btnRefresh = New Button() With {
            .Text = "REFRESH",
            .Width = 100, .Height = 30,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = CLR_ACCENT,
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand,
            .Margin = New Padding(0, 0, 4, 0)
        }
        btnRefresh.FlatAppearance.BorderSize = 0
        AddHandler btnRefresh.Click, AddressOf OnRefreshClick

        btnDelete = New Button() With {
            .Text = "DELETE",
            .Width = 100, .Height = 30,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(220, 53, 69),
            .ForeColor = Color.White,
            .Font = FNT_LABEL,
            .Cursor = Cursors.Hand,
            .Margin = New Padding(0, 0, 4, 0)
        }
        btnDelete.FlatAppearance.BorderSize = 0
        AddHandler btnDelete.Click, AddressOf OnDeleteClick

        pnlButtons.Controls.AddRange(New Control() {btnInsert, btnRefresh, btnDelete})

        ' Input panel (scrollable)
        pnlInput = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 0,
            .AutoScroll = True,
            .BackColor = CLR_CARD,
            .Padding = New Padding(4)
        }

        ' Data grid
        dgvData = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .ReadOnly = True,
            .BackgroundColor = CLR_CARD,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .Font = FNT_INPUT
        }
        dgvData.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(240, 244, 248),
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }
        dgvData.EnableHeadersVisualStyles = False

        ' Add controls in correct order (bottom-up for docking)
        rightPanel.Controls.Add(dgvData)
        rightPanel.Controls.Add(pnlInput)
        rightPanel.Controls.Add(pnlButtons)
        rightPanel.Controls.Add(lblTableName)
        rightPanel.Controls.Add(lblStatus)

        splitMain.Panel2.Controls.Add(rightPanel)
        Me.Controls.Add(splitMain)
    End Sub

    Private Sub OnTableSelected(sender As Object, e As EventArgs)
        If lstTables.SelectedIndex < 0 OrElse lstTables.SelectedIndex >= _tables.Length Then Return

        Dim tblDef As TableDef = _tables(lstTables.SelectedIndex)
        _currentTable = tblDef.TableName
        lblTableName.Text = tblDef.TableName

        BuildInputForm(tblDef)
        LoadTableData(tblDef)
    End Sub

    Private Sub BuildInputForm(tblDef As TableDef)
        pnlInput.Controls.Clear()
        _inputControls.Clear()

        ' Count editable columns (exclude auto-gen)
        Dim editableCols As New List(Of ColumnDef)
        For Each c As ColumnDef In tblDef.Columns
            If Not c.IsAutoGen Then editableCols.Add(c)
        Next

        tblInput = New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 4,
            .Padding = New Padding(4)
        }
        tblInput.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 180.0F))
        tblInput.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblInput.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 180.0F))
        tblInput.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        Dim r As Integer = 0
        Dim colIdx As Integer = 0
        tblInput.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))

        For Each c As ColumnDef In editableCols
            Dim lbl As New Label() With {
                .Text = If(c.IsRequired, c.DisplayName & " *", c.DisplayName),
                .Font = FNT_LABEL,
                .ForeColor = If(c.IsRequired, CLR_HEADER, CLR_LABEL),
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleRight,
                .Padding = New Padding(0, 0, 4, 0)
            }

            Dim txt As New TextBox() With {
                .Dock = DockStyle.Fill,
                .Font = FNT_INPUT,
                .Tag = c.Name
            }

            tblInput.Controls.Add(lbl, colIdx, r)
            tblInput.Controls.Add(txt, colIdx + 1, r)
            _inputControls(c.Name) = txt

            colIdx += 2
            If colIdx >= 4 Then
                colIdx = 0
                r += 1
                tblInput.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
            End If
        Next

        If colIdx > 0 Then
            r += 1
        End If

        pnlInput.Controls.Add(tblInput)
        pnlInput.Height = Math.Min((r + 1) * 32 + 16, 200)
    End Sub

    Private Sub LoadTableData(tblDef As TableDef)
        dgvData.DataSource = Nothing
        If Not _isConnected Then Return

        Try
            Dim sql As String = "SELECT * FROM " & tblDef.TableName & " ORDER BY id"
            Dim dt As DataTable = _crud.GetDataTable(sql)
            dgvData.DataSource = dt
            lblStatus.Text = "DB: Connected | " & tblDef.TableName & " - " & dt.Rows.Count.ToString() & " rows"
            lblStatus.ForeColor = CLR_LABEL
        Catch ex As Exception
            lblStatus.Text = "Error: " & ex.Message
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub OnInsertClick(sender As Object, e As EventArgs)
        If Not _isConnected Then
            MessageBox.Show("DB is not connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(_currentTable) Then Return

        Dim tblDef As TableDef = _tables(lstTables.SelectedIndex)
        Dim values As New Dictionary(Of String, Object)

        ' Validate required fields and collect values
        For Each c As ColumnDef In tblDef.Columns
            If c.IsAutoGen Then Continue For

            If Not _inputControls.ContainsKey(c.Name) Then Continue For

            Dim txt As TextBox = DirectCast(_inputControls(c.Name), TextBox)
            Dim val As String = txt.Text.Trim()

            If c.IsRequired AndAlso String.IsNullOrEmpty(val) Then
                MessageBox.Show(c.DisplayName & " is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt.Focus()
                Return
            End If

            If String.IsNullOrEmpty(val) Then
                Continue For
            End If

            ' Type conversion
            If c.DbType = "SMALLINT" Then
                Dim numVal As Short
                If Short.TryParse(val, numVal) Then
                    values(c.Name) = numVal
                Else
                    MessageBox.Show(c.DisplayName & " must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txt.Focus()
                    Return
                End If
            Else
                values(c.Name) = val
            End If
        Next

        If values.Count = 0 Then
            MessageBox.Show("Please enter at least one value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            _crud.Insert(_currentTable, values)
            lblStatus.Text = "INSERT completed: " & _currentTable
            lblStatus.ForeColor = CLR_SUCCESS

            ' Clear input fields
            For Each kvp As KeyValuePair(Of String, Control) In _inputControls
                DirectCast(kvp.Value, TextBox).Text = ""
            Next

            ' Reload data
            LoadTableData(tblDef)
        Catch ex As Exception
            MessageBox.Show("INSERT error: " & ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblStatus.Text = "INSERT error"
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub OnRefreshClick(sender As Object, e As EventArgs)
        If lstTables.SelectedIndex < 0 Then Return
        LoadTableData(_tables(lstTables.SelectedIndex))
    End Sub

    Private Sub OnDeleteClick(sender As Object, e As EventArgs)
        If Not _isConnected Then
            MessageBox.Show("DB is not connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(_currentTable) OrElse lstTables.SelectedIndex < 0 Then Return

        If dgvData.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim row As DataGridViewRow = dgvData.SelectedRows(0)
        Dim idValue As Object = row.Cells("id").Value
        If idValue Is Nothing OrElse IsDBNull(idValue) Then
            MessageBox.Show("Cannot determine the row ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show(
            "id=" & idValue.ToString() & " : Delete this row?",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result <> DialogResult.Yes Then Return

        Try
            Dim params As New List(Of NpgsqlParameter)()
            params.Add(New NpgsqlParameter("@id", Convert.ToInt32(idValue)))
            _crud.Delete(_currentTable, "id = @id", params)

            lblStatus.Text = "DELETE completed: " & _currentTable
            lblStatus.ForeColor = CLR_SUCCESS

            LoadTableData(_tables(lstTables.SelectedIndex))
        Catch ex As Exception
            MessageBox.Show("DELETE error: " & ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblStatus.Text = "DELETE error"
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub

End Class
