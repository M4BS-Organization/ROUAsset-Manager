Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports LeaseM4BS.DataAccess

''' <summary>
''' 税法調整（フレックス）画面
''' 年度ごとの税法調整データと調整明細を表示する。
''' </summary>
Partial Public Class FrmFlexTaxAdjustment

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
    Private btnDisplay As Button
    Private dgvTaxAdjustment As DataGridView
    Private pnlTaxDetail As Panel
    Private lblDetailTitle As Label
    Private lblAdjType As Label
    Private txtAdjType As TextBox
    Private lblAccountingAmt As Label
    Private txtAccountingAmt As TextBox
    Private lblTaxAmt As Label
    Private txtTaxAmt As TextBox
    Private lblDiffAmt As Label
    Private txtDiffAmt As TextBox
    Private lblAdjReason As Label
    Private txtAdjReason As TextBox
    Private lblTempPerm As Label
    Private txtTempPerm As TextBox

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
            .Text = "税法調整",
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

        btnDisplay = New Button() With {
            .Text = "表示",
            .Font = FNT_LABEL,
            .Size = New Size(70, 26),
            .Location = New Point(160, 31),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = CLR_HEADER
        }
        AddHandler btnDisplay.Click, AddressOf BtnDisplay_Click
        pnlHeader.Controls.Add(btnDisplay)

        Me.Controls.Add(pnlHeader)

        ' === 調整明細パネル (下部) ===
        pnlTaxDetail = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 160,
            .BackColor = CLR_CARD,
            .Padding = New Padding(12),
            .BorderStyle = BorderStyle.FixedSingle
        }
        BuildDetailPanel()
        Me.Controls.Add(pnlTaxDetail)

        ' === DataGridView (中央) ===
        dgvTaxAdjustment = New DataGridView() With {
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
        ApplyGridStyles(dgvTaxAdjustment)
        AddGridColumns()
        AddHandler dgvTaxAdjustment.SelectionChanged, AddressOf DgvTaxAdjustment_SelectionChanged
        Me.Controls.Add(dgvTaxAdjustment)

        dgvTaxAdjustment.BringToFront()
    End Sub

    Private Sub AddGridColumns()
        dgvTaxAdjustment.Columns.Add("colContractNo", "契約番号")
        dgvTaxAdjustment.Columns.Add("colContractName", "契約名")
        dgvTaxAdjustment.Columns.Add("colAdjType", "調整種別")
        dgvTaxAdjustment.Columns.Add("colAccountingAmt", "会計上金額")
        dgvTaxAdjustment.Columns.Add("colTaxAmt", "税務上金額")
        dgvTaxAdjustment.Columns.Add("colDiffAmt", "差額")
        dgvTaxAdjustment.Columns.Add("colTempPerm", "一時/永久")
        dgvTaxAdjustment.Columns.Add("colAdjReason", "調整理由")
        dgvTaxAdjustment.Columns.Add("colStatus", "ステータス")

        For Each col As String In {"colAccountingAmt", "colTaxAmt", "colDiffAmt"}
            dgvTaxAdjustment.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    Private Sub BuildDetailPanel()
        lblDetailTitle = New Label() With {
            .Text = "調整明細",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(12, 8),
            .AutoSize = True
        }
        pnlTaxDetail.Controls.Add(lblDetailTitle)

        Dim yPos As Integer = 36

        ' 左列
        lblAdjType = CreateDetailLabel("調整種別:", 12, yPos)
        txtAdjType = CreateDetailTextBox(110, yPos, 160)
        lblAccountingAmt = CreateDetailLabel("会計上金額:", 12, yPos + 30)
        txtAccountingAmt = CreateDetailTextBox(110, yPos + 30, 160)
        lblTaxAmt = CreateDetailLabel("税務上金額:", 12, yPos + 60)
        txtTaxAmt = CreateDetailTextBox(110, yPos + 60, 160)

        ' 右列
        lblDiffAmt = CreateDetailLabel("差額:", 300, yPos)
        txtDiffAmt = CreateDetailTextBox(370, yPos, 160)
        lblTempPerm = CreateDetailLabel("一時/永久:", 300, yPos + 30)
        txtTempPerm = CreateDetailTextBox(370, yPos + 30, 160)
        lblAdjReason = CreateDetailLabel("調整理由:", 300, yPos + 60)
        txtAdjReason = CreateDetailTextBox(370, yPos + 60, 260)

        pnlTaxDetail.Controls.AddRange(New Control() {
            lblAdjType, txtAdjType, lblAccountingAmt, txtAccountingAmt,
            lblTaxAmt, txtTaxAmt, lblDiffAmt, txtDiffAmt,
            lblTempPerm, txtTempPerm, lblAdjReason, txtAdjReason
        })
    End Sub

    Private Function CreateDetailLabel(text As String, x As Integer, y As Integer) As Label
        Return New Label() With {
            .Text = text,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Location = New Point(x, y),
            .AutoSize = True
        }
    End Function

    Private Function CreateDetailTextBox(x As Integer, y As Integer, Optional w As Integer = 140) As TextBox
        Return New TextBox() With {
            .Font = FNT_INPUT,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(233, 236, 239),
            .BorderStyle = BorderStyle.FixedSingle,
            .Size = New Size(w, 24),
            .Location = New Point(x, y)
        }
    End Function

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
            dgvTaxAdjustment.Rows.Clear()

            Dim fiscalYear As String = If(cmbFiscalYear.SelectedItem IsNot Nothing,
                cmbFiscalYear.SelectedItem.ToString(), Date.Today.Year.ToString())

            Dim sql As String =
                "SELECT c.contract_no, c.contract_name, " &
                "t.adjustment_type, t.accounting_amount, t.tax_amount, t.difference_amount, " &
                "t.temp_or_permanent, t.adjustment_reason, t.status " &
                "FROM tw_lease_tax_adjustment t " &
                "LEFT JOIN tw_lease_contract c ON t.contract_id = c.contract_id " &
                "WHERE t.fiscal_year = @year " &
                "ORDER BY c.contract_no, t.adjustment_type"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            params.Add(New Npgsql.NpgsqlParameter("@year", fiscalYear))

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvTaxAdjustment.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvTaxAdjustment.Rows(idx)
                    dgvRow.Cells("colContractNo").Value = row("contract_no").ToString()
                    dgvRow.Cells("colContractName").Value = row("contract_name").ToString()
                    dgvRow.Cells("colAdjType").Value = row("adjustment_type").ToString()
                    dgvRow.Cells("colAccountingAmt").Value = FormatCurrency(row("accounting_amount"))
                    dgvRow.Cells("colTaxAmt").Value = FormatCurrency(row("tax_amount"))
                    dgvRow.Cells("colDiffAmt").Value = FormatCurrency(row("difference_amount"))
                    dgvRow.Cells("colTempPerm").Value = row("temp_or_permanent").ToString()
                    dgvRow.Cells("colAdjReason").Value = row("adjustment_reason").ToString()
                    dgvRow.Cells("colStatus").Value = row("status").ToString()
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DgvTaxAdjustment_SelectionChanged(sender As Object, e As EventArgs)
        If dgvTaxAdjustment.SelectedRows.Count = 0 Then Return

        Dim row As DataGridViewRow = dgvTaxAdjustment.SelectedRows(0)
        txtAdjType.Text = row.Cells("colAdjType").Value?.ToString()
        txtAccountingAmt.Text = row.Cells("colAccountingAmt").Value?.ToString()
        txtTaxAmt.Text = row.Cells("colTaxAmt").Value?.ToString()
        txtDiffAmt.Text = row.Cells("colDiffAmt").Value?.ToString()
        txtTempPerm.Text = row.Cells("colTempPerm").Value?.ToString()
        txtAdjReason.Text = row.Cells("colAdjReason").Value?.ToString()
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
