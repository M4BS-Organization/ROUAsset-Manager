Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports LeaseM4BS.DataAccess

''' <summary>
''' 月次会計（フレックス）画面
''' 月次会計仕訳の一覧表示・仕訳生成を行う。
''' </summary>
Partial Public Class FrmFlexMonthlyAccounting

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
    Private btnGenerateJournal As Button
    Private dgvMonthlyAccounting As DataGridView
    Private pnlJournalDetail As Panel
    Private lblJournalTitle As Label
    Private lblDebitAccount As Label
    Private txtDebitAccount As TextBox
    Private lblDebitAmount As Label
    Private txtDebitAmount As TextBox
    Private lblCreditAccount As Label
    Private txtCreditAccount As TextBox
    Private lblCreditAmount As Label
    Private txtCreditAmount As TextBox
    Private lblJournalDesc As Label
    Private txtJournalDesc As TextBox

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
            .Text = "月次会計仕訳",
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

        btnGenerateJournal = New Button() With {
            .Text = "仕訳生成",
            .Font = FNT_LABEL,
            .Size = New Size(90, 26),
            .Location = New Point(310, 31),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(39, 174, 96),
            .ForeColor = Color.White
        }
        AddHandler btnGenerateJournal.Click, AddressOf BtnGenerateJournal_Click
        pnlHeader.Controls.Add(btnGenerateJournal)

        Me.Controls.Add(pnlHeader)

        ' === 仕訳明細パネル (下部) ===
        pnlJournalDetail = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 140,
            .BackColor = CLR_CARD,
            .Padding = New Padding(12),
            .BorderStyle = BorderStyle.FixedSingle
        }
        BuildJournalDetailPanel()
        Me.Controls.Add(pnlJournalDetail)

        ' === DataGridView (中央) ===
        dgvMonthlyAccounting = New DataGridView() With {
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
        ApplyGridStyles(dgvMonthlyAccounting)
        AddGridColumns()
        AddHandler dgvMonthlyAccounting.SelectionChanged, AddressOf DgvMonthlyAccounting_SelectionChanged
        Me.Controls.Add(dgvMonthlyAccounting)

        dgvMonthlyAccounting.BringToFront()
    End Sub

    Private Sub AddGridColumns()
        dgvMonthlyAccounting.Columns.Add("colAccYM", "会計年月")
        dgvMonthlyAccounting.Columns.Add("colContractNo", "契約番号")
        dgvMonthlyAccounting.Columns.Add("colContractName", "契約名")
        dgvMonthlyAccounting.Columns.Add("colJournalType", "仕訳種別")
        dgvMonthlyAccounting.Columns.Add("colDebitAccount", "借方科目")
        dgvMonthlyAccounting.Columns.Add("colDebitAmount", "借方金額")
        dgvMonthlyAccounting.Columns.Add("colCreditAccount", "貸方科目")
        dgvMonthlyAccounting.Columns.Add("colCreditAmount", "貸方金額")
        dgvMonthlyAccounting.Columns.Add("colStatus", "ステータス")

        For Each col As String In {"colDebitAmount", "colCreditAmount"}
            dgvMonthlyAccounting.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    Private Sub BuildJournalDetailPanel()
        lblJournalTitle = New Label() With {
            .Text = "仕訳明細",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(12, 8),
            .AutoSize = True
        }
        pnlJournalDetail.Controls.Add(lblJournalTitle)

        Dim yPos As Integer = 36

        ' 借方
        lblDebitAccount = CreateDetailLabel("借方科目:", 12, yPos)
        txtDebitAccount = CreateDetailTextBox(100, yPos, 180)
        lblDebitAmount = CreateDetailLabel("借方金額:", 300, yPos)
        txtDebitAmount = CreateDetailTextBox(388, yPos, 140)

        ' 貸方
        lblCreditAccount = CreateDetailLabel("貸方科目:", 12, yPos + 30)
        txtCreditAccount = CreateDetailTextBox(100, yPos + 30, 180)
        lblCreditAmount = CreateDetailLabel("貸方金額:", 300, yPos + 30)
        txtCreditAmount = CreateDetailTextBox(388, yPos + 30, 140)

        ' 摘要
        lblJournalDesc = CreateDetailLabel("摘要:", 12, yPos + 60)
        txtJournalDesc = CreateDetailTextBox(100, yPos + 60, 428)

        pnlJournalDetail.Controls.AddRange(New Control() {
            lblDebitAccount, txtDebitAccount, lblDebitAmount, txtDebitAmount,
            lblCreditAccount, txtCreditAccount, lblCreditAmount, txtCreditAmount,
            lblJournalDesc, txtJournalDesc
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
            dgvMonthlyAccounting.Rows.Clear()

            Dim fromYM As String = dtpFrom.Value.ToString("yyyyMM")
            Dim toYM As String = dtpTo.Value.ToString("yyyyMM")

            Dim sql As String =
                "SELECT j.journal_ym, c.contract_no, c.contract_name, " &
                "j.journal_type, j.debit_account_name, j.debit_amount, " &
                "j.credit_account_name, j.credit_amount, j.status, j.description " &
                "FROM tw_lease_journal j " &
                "LEFT JOIN tw_lease_contract c ON j.contract_id = c.contract_id " &
                "WHERE j.journal_ym BETWEEN @from AND @to " &
                "ORDER BY j.journal_ym, c.contract_no"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            params.Add(New Npgsql.NpgsqlParameter("@from", fromYM))
            params.Add(New Npgsql.NpgsqlParameter("@to", toYM))

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvMonthlyAccounting.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvMonthlyAccounting.Rows(idx)
                    dgvRow.Cells("colAccYM").Value = row("journal_ym").ToString()
                    dgvRow.Cells("colContractNo").Value = row("contract_no").ToString()
                    dgvRow.Cells("colContractName").Value = row("contract_name").ToString()
                    dgvRow.Cells("colJournalType").Value = row("journal_type").ToString()
                    dgvRow.Cells("colDebitAccount").Value = row("debit_account_name").ToString()
                    dgvRow.Cells("colDebitAmount").Value = FormatCurrency(row("debit_amount"))
                    dgvRow.Cells("colCreditAccount").Value = row("credit_account_name").ToString()
                    dgvRow.Cells("colCreditAmount").Value = FormatCurrency(row("credit_amount"))
                    dgvRow.Cells("colStatus").Value = row("status").ToString()
                    dgvRow.Tag = row("description").ToString()
                Next
            End Using

        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DgvMonthlyAccounting_SelectionChanged(sender As Object, e As EventArgs)
        If dgvMonthlyAccounting.SelectedRows.Count = 0 Then Return

        Dim row As DataGridViewRow = dgvMonthlyAccounting.SelectedRows(0)
        txtDebitAccount.Text = row.Cells("colDebitAccount").Value?.ToString()
        txtDebitAmount.Text = row.Cells("colDebitAmount").Value?.ToString()
        txtCreditAccount.Text = row.Cells("colCreditAccount").Value?.ToString()
        txtCreditAmount.Text = row.Cells("colCreditAmount").Value?.ToString()
        txtJournalDesc.Text = TryCast(row.Tag, String)
    End Sub

    Private Sub BtnGenerateJournal_Click(sender As Object, e As EventArgs)
        Dim result As DialogResult = MessageBox.Show(
            "選択した期間の仕訳を生成しますか？",
            "仕訳生成確認",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Try
                ' 仕訳生成ロジック（将来実装）
                MessageBox.Show("仕訳生成機能は今後実装予定です。", "情報",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("仕訳生成エラー: " & ex.Message, "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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
