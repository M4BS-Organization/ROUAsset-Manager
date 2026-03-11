Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports LeaseM4BS.DataAccess

''' <summary>
''' 使用権資産（フレックス）画面
''' ROU資産の一覧表示・検索・詳細表示を行う。
''' </summary>
Partial Public Class FrmFlexROUAsset

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
    Private txtSearch As TextBox
    Private btnSearch As Button
    Private btnExport As Button
    Private btnPrint As Button
    Private dgvROUAsset As DataGridView
    Private pnlDetail As Panel
    Private lblInitialTitle As Label
    Private lblInitialDirect As Label
    Private lblRestoration As Label
    Private lblIncentive As Label
    Private lblROUTotal As Label
    Private txtInitialDirect As TextBox
    Private txtRestoration As TextBox
    Private txtIncentive As TextBox
    Private txtROUTotal As TextBox
    Private lblDeprecTitle As Label
    Private lblMethod As Label
    Private lblUsefulLife As Label
    Private lblAccumDeprec As Label
    Private lblBookValue As Label
    Private txtMethod As TextBox
    Private txtUsefulLife As TextBox
    Private txtAccumDeprec As TextBox
    Private txtBookValue As TextBox

    Public Sub New()
        InitializeComponent()
        Me.BackColor = CLR_BG
        Me.Padding = New Padding(8, 0, 8, 8)

        ' プレースホルダーラベルを非表示
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
            .Text = "使用権資産一覧",
            .Font = FNT_TITLE,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 6)
        }
        pnlHeader.Controls.Add(lblTitle)

        txtSearch = New TextBox() With {
            .Font = FNT_INPUT,
            .Size = New Size(200, 24),
            .Location = New Point(12, 32)
        }
        txtSearch.Text = ""
        pnlHeader.Controls.Add(txtSearch)

        btnSearch = New Button() With {
            .Text = "検索",
            .Font = FNT_LABEL,
            .Size = New Size(70, 26),
            .Location = New Point(220, 31),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = CLR_HEADER
        }
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        pnlHeader.Controls.Add(btnSearch)

        btnExport = New Button() With {
            .Text = "Export",
            .Font = FNT_LABEL,
            .Size = New Size(80, 26),
            .Location = New Point(pnlHeader.Width - 180, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(39, 174, 96),
            .ForeColor = Color.White
        }
        AddHandler btnExport.Click, AddressOf BtnExport_Click
        pnlHeader.Controls.Add(btnExport)

        btnPrint = New Button() With {
            .Text = "Print",
            .Font = FNT_LABEL,
            .Size = New Size(80, 26),
            .Location = New Point(pnlHeader.Width - 90, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(142, 68, 173),
            .ForeColor = Color.White
        }
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        pnlHeader.Controls.Add(btnPrint)

        Me.Controls.Add(pnlHeader)

        ' === 詳細パネル (下部) ===
        pnlDetail = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 160,
            .BackColor = CLR_CARD,
            .Padding = New Padding(12),
            .BorderStyle = BorderStyle.FixedSingle
        }
        BuildDetailPanel()
        Me.Controls.Add(pnlDetail)

        ' === DataGridView (中央) ===
        dgvROUAsset = New DataGridView() With {
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
        ApplyGridStyles(dgvROUAsset)
        AddGridColumns()
        AddHandler dgvROUAsset.SelectionChanged, AddressOf DgvROUAsset_SelectionChanged
        Me.Controls.Add(dgvROUAsset)

        ' 順序調整
        dgvROUAsset.BringToFront()
    End Sub

    Private Sub AddGridColumns()
        dgvROUAsset.Columns.Add("colContractNo", "契約番号")
        dgvROUAsset.Columns.Add("colContractName", "契約名")
        dgvROUAsset.Columns.Add("colAssetCategory", "資産区分")
        dgvROUAsset.Columns.Add("colInitialAmount", "当初認識額")
        dgvROUAsset.Columns.Add("colAccumDeprec", "累計償却")
        dgvROUAsset.Columns.Add("colBookValue", "帳簿価額")
        dgvROUAsset.Columns.Add("colStartDate", "開始日")
        dgvROUAsset.Columns.Add("colEndDate", "終了日")
        dgvROUAsset.Columns.Add("colUsefulLife", "耐用年数")
        dgvROUAsset.Columns.Add("colMethod", "償却方法")

        ' 金額列は右寄せ
        dgvROUAsset.Columns("colInitialAmount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvROUAsset.Columns("colAccumDeprec").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvROUAsset.Columns("colBookValue").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub BuildDetailPanel()
        ' 当初認識額内訳
        lblInitialTitle = New Label() With {
            .Text = "当初認識額内訳",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(12, 8),
            .AutoSize = True
        }
        pnlDetail.Controls.Add(lblInitialTitle)

        Dim yPos As Integer = 32
        lblInitialDirect = CreateDetailLabel("初期直接費用:", 12, yPos)
        txtInitialDirect = CreateDetailTextBox(130, yPos)
        lblRestoration = CreateDetailLabel("原状回復費用:", 12, yPos + 28)
        txtRestoration = CreateDetailTextBox(130, yPos + 28)
        lblIncentive = CreateDetailLabel("リースインセンティブ:", 12, yPos + 56)
        txtIncentive = CreateDetailTextBox(160, yPos + 56)
        lblROUTotal = CreateDetailLabel("ROU資産合計:", 12, yPos + 84)
        txtROUTotal = CreateDetailTextBox(130, yPos + 84)

        pnlDetail.Controls.AddRange(New Control() {
            lblInitialDirect, txtInitialDirect, lblRestoration, txtRestoration,
            lblIncentive, txtIncentive, lblROUTotal, txtROUTotal
        })

        ' 減価償却情報
        lblDeprecTitle = New Label() With {
            .Text = "減価償却情報",
            .Font = FNT_SECTION,
            .ForeColor = CLR_HEADER,
            .Location = New Point(380, 8),
            .AutoSize = True
        }
        pnlDetail.Controls.Add(lblDeprecTitle)

        lblMethod = CreateDetailLabel("償却方法:", 380, yPos)
        txtMethod = CreateDetailTextBox(480, yPos)
        lblUsefulLife = CreateDetailLabel("耐用年数:", 380, yPos + 28)
        txtUsefulLife = CreateDetailTextBox(480, yPos + 28)
        lblAccumDeprec = CreateDetailLabel("累計償却額:", 380, yPos + 56)
        txtAccumDeprec = CreateDetailTextBox(480, yPos + 56)
        lblBookValue = CreateDetailLabel("帳簿価額:", 380, yPos + 84)
        txtBookValue = CreateDetailTextBox(480, yPos + 84)

        pnlDetail.Controls.AddRange(New Control() {
            lblDeprecTitle, lblMethod, txtMethod, lblUsefulLife, txtUsefulLife,
            lblAccumDeprec, txtAccumDeprec, lblBookValue, txtBookValue
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

    Private Function CreateDetailTextBox(x As Integer, y As Integer) As TextBox
        Return New TextBox() With {
            .Font = FNT_INPUT,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(233, 236, 239),
            .BorderStyle = BorderStyle.FixedSingle,
            .Size = New Size(140, 24),
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

    Private Sub LoadData(Optional searchText As String = "")
        Try
            dgvROUAsset.Rows.Clear()

            Dim sql As String =
                "SELECT c.contract_no, c.contract_name, c.asset_breakdown, ac.rou_asset, " &
                "ac.accum_depreciation, ac.book_value, c.start_date, c.end_date, " &
                "ac.useful_life, ac.depreciation_method, " &
                "ac.initial_direct_cost, ac.restoration_cost, ac.lease_incentive " &
                "FROM tw_lease_contract c " &
                "LEFT JOIN tw_lease_accounting ac ON c.contract_id = ac.contract_id"

            If Not String.IsNullOrWhiteSpace(searchText) Then
                sql &= " WHERE c.contract_no LIKE @search OR c.contract_name LIKE @search"
            End If

            sql &= " ORDER BY c.contract_no"

            Dim params As New List(Of Npgsql.NpgsqlParameter)
            If Not String.IsNullOrWhiteSpace(searchText) Then
                params.Add(New Npgsql.NpgsqlParameter("@search", "%" & searchText & "%"))
            End If

            Using crud As New CrudHelper()
                Dim dt As DataTable = crud.GetDataTable(sql, params)
                For Each row As DataRow In dt.Rows
                    Dim idx As Integer = dgvROUAsset.Rows.Add()
                    Dim dgvRow As DataGridViewRow = dgvROUAsset.Rows(idx)
                    dgvRow.Cells("colContractNo").Value = row("contract_no").ToString()
                    dgvRow.Cells("colContractName").Value = row("contract_name").ToString()
                    dgvRow.Cells("colAssetCategory").Value = row("asset_breakdown").ToString()
                    dgvRow.Cells("colInitialAmount").Value = FormatCurrency(row("rou_asset"))
                    dgvRow.Cells("colAccumDeprec").Value = FormatCurrency(row("accum_depreciation"))
                    dgvRow.Cells("colBookValue").Value = FormatCurrency(row("book_value"))
                    dgvRow.Cells("colStartDate").Value = FormatDate(row("start_date"))
                    dgvRow.Cells("colEndDate").Value = FormatDate(row("end_date"))
                    dgvRow.Cells("colUsefulLife").Value = row("useful_life").ToString()
                    dgvRow.Cells("colMethod").Value = row("depreciation_method").ToString()

                    ' タグに詳細情報を保存
                    dgvRow.Tag = New Object() {
                        row("initial_direct_cost"),
                        row("restoration_cost"),
                        row("lease_incentive")
                    }
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DgvROUAsset_SelectionChanged(sender As Object, e As EventArgs)
        If dgvROUAsset.SelectedRows.Count = 0 Then Return

        Dim row As DataGridViewRow = dgvROUAsset.SelectedRows(0)
        txtMethod.Text = row.Cells("colMethod").Value?.ToString()
        txtUsefulLife.Text = row.Cells("colUsefulLife").Value?.ToString()
        txtAccumDeprec.Text = row.Cells("colAccumDeprec").Value?.ToString()
        txtBookValue.Text = row.Cells("colBookValue").Value?.ToString()

        Dim detail As Object() = TryCast(row.Tag, Object())
        If detail IsNot Nothing AndAlso detail.Length >= 3 Then
            txtInitialDirect.Text = FormatCurrency(detail(0))
            txtRestoration.Text = FormatCurrency(detail(1))
            txtIncentive.Text = FormatCurrency(detail(2))
            txtROUTotal.Text = row.Cells("colInitialAmount").Value?.ToString()
        Else
            txtInitialDirect.Text = ""
            txtRestoration.Text = ""
            txtIncentive.Text = ""
            txtROUTotal.Text = ""
        End If
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
        LoadData(txtSearch.Text.Trim())
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs)
        MessageBox.Show("エクスポート機能は今後実装予定です。", "情報",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs)
        MessageBox.Show("印刷機能は今後実装予定です。", "情報",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
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
