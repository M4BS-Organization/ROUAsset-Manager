Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports LeaseM4BS.DataAccess

''' <summary>
''' 期間残高（フレックス）画面
''' 年度/四半期ごとの残高マトリックスと契約別内訳を表示する。
''' </summary>
Partial Public Class FrmFlexPeriodBalance

    Private ReadOnly CLR_HEADER As Color = Color.FromArgb(41, 128, 185)
    Private ReadOnly CLR_BG As Color = Color.FromArgb(236, 240, 241)
    Private ReadOnly CLR_CARD As Color = Color.White
    Private ReadOnly CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
    Private ReadOnly CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
    Private ReadOnly CLR_BORDER As Color = Color.FromArgb(222, 226, 230)

    Private ReadOnly FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)
    Private ReadOnly FNT_INPUT As New Font("Meiryo", 9.75F, FontStyle.Regular)
    Private ReadOnly FNT_SECTION As New Font("Meiryo", 10.0F, FontStyle.Bold)
    Private ReadOnly FNT_TITLE As New Font("Meiryo", 12.0F, FontStyle.Bold)

    Private pnlHeader As Panel
    Private cmbFiscalYear As ComboBox
    Private cmbQuarter As ComboBox
    Private btnDisplay As Button
    Private dgvPeriodBalance As DataGridView
    Private pnlContractBreakdown As Panel
    Private dgvBreakdown As DataGridView
    Private lblBreakdownTitle As Label

    Public Sub New()
        InitializeComponent()
        Me.BackColor = CLR_BG
        Me.Padding = New Padding(8, 0, 8, 8)

        If lblPlaceholder IsNot Nothing Then lblPlaceholder.Visible = False

        BuildUI()
        LoadData()
    End Sub

    Private Sub BuildUI()
        ' === ヘッダーパネル ===
        pnlHeader = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = CLR_HEADER,
            .Padding = New Padding(12, 8, 12, 8)
        }

        Dim lblTitle As New Label() With {
            .Text = "期間残高",
            .Font = FNT_TITLE,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 6)
        }
        pnlHeader.Controls.Add(lblTitle)

        Dim lblYear As New Label() With {
            .Text = "年度:",
            .Font = FNT_LABEL,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 35)
        }
        pnlHeader.Controls.Add(lblYear)

        cmbFiscalYear = New ComboBox() With {
            .Font = FNT_INPUT,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Size = New Size(90, 24),
            .Location = New Point(56, 32)
        }
        Dim currentYear As Integer = Date.Today.Year
        For y As Integer = currentYear - 5 To currentYear + 1
            cmbFiscalYear.Items.Add(y.ToString())
        Next
        cmbFiscalYear.SelectedItem = currentYear.ToString()
        pnlHeader.Controls.Add(cmbFiscalYear)

        Dim lblQ As New Label() With {
            .Text = "四半期:",
            .Font = FNT_LABEL,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(160, 35)
        }
        pnlHeader.Controls.Add(lblQ)

        cmbQuarter = New ComboBox() With {
            .Font = FNT_INPUT,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Size = New Size(80, 24),
            .Location = New Point(218, 32)
        }
        cmbQuarter.Items.AddRange(New Object() {"通期", "Q1", "Q2", "Q3", "Q4"})
        cmbQuarter.SelectedIndex = 0
        pnlHeader.Controls.Add(cmbQuarter)

        btnDisplay = New Button() With {
            .Text = "表示",
            .Font = FNT_LABEL,
            .Size = New Size(70, 26),
            .Location = New Point(310, 31),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = CLR_HEADER
        }
        AddHandler btnDisplay.Click, AddressOf BtnDisplay_Click
        pnlHeader.Controls.Add(btnDisplay)

        Me.Controls.Add(pnlHeader)

        ' === 契約別内訳パネル (下部) ===
        pnlContractBreakdown = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 180,
            .BackColor = CLR_CARD,
            .Padding = New Padding(12),
            .BorderStyle = BorderStyle.FixedSingle
        }

        lblBreakdownTitle = New Label() With {
            .Text = "契約別内訳",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(12, 8),
            .AutoSize = True
        }
        pnlContractBreakdown.Controls.Add(lblBreakdownTitle)

        dgvBreakdown = New DataGridView() With {
            .Location = New Point(12, 32),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BackgroundColor = CLR_CARD,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .Font = FNT_INPUT
        }
        dgvBreakdown.Size = New Size(pnlContractBreakdown.Width - 24, 136)
        ApplyGridStyles(dgvBreakdown)
        AddBreakdownColumns()
        pnlContractBreakdown.Controls.Add(dgvBreakdown)

        Me.Controls.Add(pnlContractBreakdown)

        ' === DataGridView 残高マトリックス (中央) ===
        dgvPeriodBalance = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BackgroundColor = CLR_CARD,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .Font = FNT_INPUT
        }
        ApplyGridStyles(dgvPeriodBalance)
        AddBalanceColumns()
        AddHandler dgvPeriodBalance.SelectionChanged, AddressOf DgvPeriodBalance_SelectionChanged
        Me.Controls.Add(dgvPeriodBalance)

        dgvPeriodBalance.BringToFront()
    End Sub

    Private Sub AddBalanceColumns()
        dgvPeriodBalance.Columns.Add("colAccountItem", "科目")
        dgvPeriodBalance.Columns.Add("colOpeningBal", "期首残高")
        dgvPeriodBalance.Columns.Add("colIncrease", "増加")
        dgvPeriodBalance.Columns.Add("colChangeAdj", "変更増減")
        dgvPeriodBalance.Columns.Add("colDecrease", "減少")
        dgvPeriodBalance.Columns.Add("colClosingBal", "期末残高")

        For Each col As String In {"colOpeningBal", "colIncrease", "colChangeAdj", "colDecrease", "colClosingBal"}
            dgvPeriodBalance.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    Private Sub AddBreakdownColumns()
        dgvBreakdown.Columns.Add("colBdContractNo", "契約番号")
        dgvBreakdown.Columns.Add("colBdContractName", "契約名")
        dgvBreakdown.Columns.Add("colBdOpeningBal", "期首残高")
        dgvBreakdown.Columns.Add("colBdIncrease", "増加")
        dgvBreakdown.Columns.Add("colBdDecrease", "減少")
        dgvBreakdown.Columns.Add("colBdClosingBal", "期末残高")

        For Each col As String In {"colBdOpeningBal", "colBdIncrease", "colBdDecrease", "colBdClosingBal"}
            dgvBreakdown.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    Private Sub ApplyGridStyles(dgv As DataGridView)
        dgv.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(240, 244, 248),
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }
        dgv.DefaultCellStyle = New DataGridViewCellStyle() With {
            .Font = FNT_INPUT,
            .ForeColor = CLR_TEXT,
            .SelectionBackColor = Color.FromArgb(209, 226, 243),
            .SelectionForeColor = CLR_TEXT
        }
        dgv.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(248, 249, 250)
        }
    End Sub

    Private Sub LoadData()
        Try
            dgvPeriodBalance.Rows.Clear()

            Dim fiscalYear As String = If(cmbFiscalYear.SelectedItem IsNot Nothing,
                cmbFiscalYear.SelectedItem.ToString(), Date.Today.Year.ToString())
            Dim quarter As String = If(cmbQuarter.SelectedItem IsNot Nothing,
                cmbQuarter.SelectedItem.ToString(), "通期")

            Dim sql As String =
                "SELECT b.account_item, b.opening_balance, b.increase_amount, " &
                "b.change_adjustment, b.decrease_amount, b.closing_balance " &
                "FROM tw_lease_period_balance b " &
                "WHERE b.fiscal_year = @year"

            If quarter <> "通期" Then
                sql &= " AND b.quarter = @quarter"
            End If

            sql &= " ORDER BY b.display_order"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            params.Add(New Npgsql.NpgsqlParameter("@year", fiscalYear))
            If quarter <> "通期" Then
                params.Add(New Npgsql.NpgsqlParameter("@quarter", quarter))
            End If

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvPeriodBalance.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvPeriodBalance.Rows(idx)
                    dgvRow.Cells("colAccountItem").Value = row("account_item").ToString()
                    dgvRow.Cells("colOpeningBal").Value = FormatCurrency(row("opening_balance"))
                    dgvRow.Cells("colIncrease").Value = FormatCurrency(row("increase_amount"))
                    dgvRow.Cells("colChangeAdj").Value = FormatCurrency(row("change_adjustment"))
                    dgvRow.Cells("colDecrease").Value = FormatCurrency(row("decrease_amount"))
                    dgvRow.Cells("colClosingBal").Value = FormatCurrency(row("closing_balance"))
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DgvPeriodBalance_SelectionChanged(sender As Object, e As EventArgs)
        If dgvPeriodBalance.SelectedRows.Count = 0 Then Return

        Dim accountItem As String = dgvPeriodBalance.SelectedRows(0).Cells("colAccountItem").Value?.ToString()
        If String.IsNullOrEmpty(accountItem) Then Return

        LoadContractBreakdown(accountItem)
    End Sub

    Private Sub LoadContractBreakdown(accountItem As String)
        Try
            dgvBreakdown.Rows.Clear()

            Dim fiscalYear As String = If(cmbFiscalYear.SelectedItem IsNot Nothing,
                cmbFiscalYear.SelectedItem.ToString(), Date.Today.Year.ToString())

            Dim sql As String =
                "SELECT c.contract_no, c.contract_name, " &
                "bd.opening_balance, bd.increase_amount, bd.decrease_amount, bd.closing_balance " &
                "FROM tw_lease_balance_breakdown bd " &
                "LEFT JOIN tw_lease_contract c ON bd.contract_id = c.contract_id " &
                "WHERE bd.fiscal_year = @year AND bd.account_item = @item " &
                "ORDER BY c.contract_no"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            params.Add(New Npgsql.NpgsqlParameter("@year", fiscalYear))
            params.Add(New Npgsql.NpgsqlParameter("@item", accountItem))

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvBreakdown.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvBreakdown.Rows(idx)
                    dgvRow.Cells("colBdContractNo").Value = row("contract_no").ToString()
                    dgvRow.Cells("colBdContractName").Value = row("contract_name").ToString()
                    dgvRow.Cells("colBdOpeningBal").Value = FormatCurrency(row("opening_balance"))
                    dgvRow.Cells("colBdIncrease").Value = FormatCurrency(row("increase_amount"))
                    dgvRow.Cells("colBdDecrease").Value = FormatCurrency(row("decrease_amount"))
                    dgvRow.Cells("colBdClosingBal").Value = FormatCurrency(row("closing_balance"))
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("内訳データ読み込みエラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnDisplay_Click(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Function FormatCurrency(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then Return ""
        Dim d As Decimal
        If Decimal.TryParse(value.ToString(), d) Then
            Return d.ToString("#,##0")
        End If
        Return value.ToString()
    End Function

End Class
