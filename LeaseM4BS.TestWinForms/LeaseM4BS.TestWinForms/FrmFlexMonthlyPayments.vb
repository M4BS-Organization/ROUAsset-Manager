Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports LeaseM4BS.DataAccess

''' <summary>
''' 月次支払（フレックス）画面
''' 期間指定で月次支払実績を一覧表示・集計する。
''' </summary>
Partial Public Class FrmFlexMonthlyPayments

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
    Private dtpFrom As DateTimePicker
    Private dtpTo As DateTimePicker
    Private btnDisplay As Button
    Private dgvMonthlyPayments As DataGridView
    Private pnlSummary As Panel
    Private lblSumTitle As Label
    Private lblTotalPayment As Label
    Private txtTotalPayment As TextBox
    Private lblTotalCount As Label
    Private txtTotalCount As TextBox
    Private lblAvgPayment As Label
    Private txtAvgPayment As TextBox
    Private lblMaxPayment As Label
    Private txtMaxPayment As TextBox

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
            .Text = "月次支払一覧",
            .Font = FNT_TITLE,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 6)
        }
        pnlHeader.Controls.Add(lblTitle)

        Dim lblFrom As New Label() With {
            .Text = "期間:",
            .Font = FNT_LABEL,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 35)
        }
        pnlHeader.Controls.Add(lblFrom)

        dtpFrom = New DateTimePicker() With {
            .Font = FNT_INPUT,
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "yyyy/MM",
            .ShowUpDown = True,
            .Size = New Size(110, 24),
            .Location = New Point(56, 32),
            .Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        }
        pnlHeader.Controls.Add(dtpFrom)

        Dim lblTilde As New Label() With {
            .Text = "~",
            .Font = FNT_LABEL,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(172, 35)
        }
        pnlHeader.Controls.Add(lblTilde)

        dtpTo = New DateTimePicker() With {
            .Font = FNT_INPUT,
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "yyyy/MM",
            .ShowUpDown = True,
            .Size = New Size(110, 24),
            .Location = New Point(188, 32),
            .Value = New Date(Date.Today.Year, Date.Today.Month, 1)
        }
        pnlHeader.Controls.Add(dtpTo)

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

        ' === 集計パネル (下部) ===
        pnlSummary = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 80,
            .BackColor = CLR_CARD,
            .Padding = New Padding(12),
            .BorderStyle = BorderStyle.FixedSingle
        }
        BuildSummaryPanel()
        Me.Controls.Add(pnlSummary)

        ' === DataGridView (中央) ===
        dgvMonthlyPayments = New DataGridView() With {
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
        ApplyGridStyles(dgvMonthlyPayments)
        AddGridColumns()
        Me.Controls.Add(dgvMonthlyPayments)

        dgvMonthlyPayments.BringToFront()
    End Sub

    Private Sub AddGridColumns()
        dgvMonthlyPayments.Columns.Add("colPaymentYM", "支払年月")
        dgvMonthlyPayments.Columns.Add("colContractNo", "契約番号")
        dgvMonthlyPayments.Columns.Add("colContractName", "契約名")
        dgvMonthlyPayments.Columns.Add("colPaymentType", "支払種別")
        dgvMonthlyPayments.Columns.Add("colPaymentAmount", "支払額")
        dgvMonthlyPayments.Columns.Add("colTaxAmount", "消費税額")
        dgvMonthlyPayments.Columns.Add("colTotalAmount", "合計額")
        dgvMonthlyPayments.Columns.Add("colPaymentDate", "支払日")
        dgvMonthlyPayments.Columns.Add("colSupplier", "取引先")
        dgvMonthlyPayments.Columns.Add("colStatus", "ステータス")

        For Each col As String In {"colPaymentAmount", "colTaxAmount", "colTotalAmount"}
            dgvMonthlyPayments.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    Private Sub BuildSummaryPanel()
        lblSumTitle = New Label() With {
            .Text = "集計",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(12, 8),
            .AutoSize = True
        }
        pnlSummary.Controls.Add(lblSumTitle)

        Dim xPos As Integer = 12
        Dim yPos As Integer = 36

        lblTotalPayment = CreateSummaryLabel("支払合計:", xPos, yPos)
        txtTotalPayment = CreateSummaryTextBox(xPos + 80, yPos)
        lblTotalCount = CreateSummaryLabel("件数:", xPos + 240, yPos)
        txtTotalCount = CreateSummaryTextBox(xPos + 290, yPos, 80)
        lblAvgPayment = CreateSummaryLabel("平均:", xPos + 400, yPos)
        txtAvgPayment = CreateSummaryTextBox(xPos + 450, yPos)
        lblMaxPayment = CreateSummaryLabel("最大:", xPos + 600, yPos)
        txtMaxPayment = CreateSummaryTextBox(xPos + 650, yPos)

        pnlSummary.Controls.AddRange(New Control() {
            lblTotalPayment, txtTotalPayment, lblTotalCount, txtTotalCount,
            lblAvgPayment, txtAvgPayment, lblMaxPayment, txtMaxPayment
        })
    End Sub

    Private Function CreateSummaryLabel(text As String, x As Integer, y As Integer) As Label
        Return New Label() With {
            .Text = text,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Location = New Point(x, y),
            .AutoSize = True
        }
    End Function

    Private Function CreateSummaryTextBox(x As Integer, y As Integer, Optional w As Integer = 130) As TextBox
        Return New TextBox() With {
            .Font = FNT_INPUT,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(233, 236, 239),
            .BorderStyle = BorderStyle.FixedSingle,
            .Size = New Size(w, 24),
            .Location = New Point(x, y),
            .TextAlign = HorizontalAlignment.Right
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
            dgvMonthlyPayments.Rows.Clear()

            Dim fromYM As String = dtpFrom.Value.ToString("yyyyMM")
            Dim toYM As String = dtpTo.Value.ToString("yyyyMM")

            Dim sql As String =
                "SELECT pa.payment_ym, c.contract_no, c.contract_name, " &
                "pa.payment_type, pa.actual_amount, pa.tax_amount, pa.total_amount, " &
                "pa.payment_date, pa.supplier_name, pa.status " &
                "FROM tw_lease_payment_actual pa " &
                "LEFT JOIN tw_lease_contract c ON pa.contract_id = c.contract_id " &
                "WHERE pa.payment_ym BETWEEN @from AND @to " &
                "ORDER BY pa.payment_ym, c.contract_no"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            params.Add(New Npgsql.NpgsqlParameter("@from", fromYM))
            params.Add(New Npgsql.NpgsqlParameter("@to", toYM))

            Dim totalPay As Decimal = 0
            Dim maxPay As Decimal = 0
            Dim cnt As Integer = 0

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvMonthlyPayments.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvMonthlyPayments.Rows(idx)
                    dgvRow.Cells("colPaymentYM").Value = row("payment_ym").ToString()
                    dgvRow.Cells("colContractNo").Value = row("contract_no").ToString()
                    dgvRow.Cells("colContractName").Value = row("contract_name").ToString()
                    dgvRow.Cells("colPaymentType").Value = row("payment_type").ToString()
                    dgvRow.Cells("colPaymentAmount").Value = FormatCurrency(row("actual_amount"))
                    dgvRow.Cells("colTaxAmount").Value = FormatCurrency(row("tax_amount"))
                    dgvRow.Cells("colTotalAmount").Value = FormatCurrency(row("total_amount"))
                    dgvRow.Cells("colPaymentDate").Value = FormatDate(row("payment_date"))
                    dgvRow.Cells("colSupplier").Value = row("supplier_name").ToString()
                    dgvRow.Cells("colStatus").Value = row("status").ToString()

                    Dim amt As Decimal = 0
                    If Not IsDBNull(row("total_amount")) Then
                        Decimal.TryParse(row("total_amount").ToString(), amt)
                    End If
                    totalPay += amt
                    If amt > maxPay Then maxPay = amt
                    cnt += 1
                Next
            End Using

            ' 集計更新
            txtTotalPayment.Text = totalPay.ToString("#,##0")
            txtTotalCount.Text = cnt.ToString()
            txtAvgPayment.Text = If(cnt > 0, (totalPay / cnt).ToString("#,##0"), "0")
            txtMaxPayment.Text = maxPay.ToString("#,##0")

        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー",
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

    Private Function FormatDate(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then Return ""
        Dim dt As Date
        If Date.TryParse(value.ToString(), dt) Then
            Return dt.ToString("yyyy/MM/dd")
        End If
        Return value.ToString()
    End Function

End Class
