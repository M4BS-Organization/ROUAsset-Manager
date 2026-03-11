Imports System
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' 契約書（フレックス）画面
''' CTBデータストアからCTBレコード（管理ID単位）を一覧表示する。
''' </summary>
Partial Public Class FrmFlexContract

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
    Private txtContractNo As TextBox
    Private btnSearch As Button
    Private btnNewEntry As Button
    Private btnEdit As Button
    Private btnInquiry As Button
    Private dgvContractList As DataGridView

    Public Sub New()
        InitializeComponent()
        Me.BackColor = CLR_BG
        Me.Padding = New Padding(8, 0, 8, 8)

        ' プレースホルダーラベルを非表示
        If lblPlaceholder IsNot Nothing Then lblPlaceholder.Visible = False

        BuildUI()
        ApplyGridStyles()
        LoadCtbData()
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
            .Text = "契約書一覧",
            .Font = FNT_TITLE,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 6)
        }
        pnlHeader.Controls.Add(lblTitle)

        Dim lblContractNo As New Label() With {
            .Text = "契約番号:",
            .Font = FNT_LABEL,
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(12, 35)
        }
        pnlHeader.Controls.Add(lblContractNo)

        txtContractNo = New TextBox() With {
            .Font = FNT_INPUT,
            .Size = New Size(150, 24),
            .Location = New Point(84, 32)
        }
        pnlHeader.Controls.Add(txtContractNo)

        btnSearch = New Button() With {
            .Text = "検索",
            .Font = FNT_LABEL,
            .Size = New Size(70, 26),
            .Location = New Point(244, 31),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = CLR_HEADER
        }
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        pnlHeader.Controls.Add(btnSearch)

        ' 右側ボタン群
        btnNewEntry = New Button() With {
            .Text = "新規登録",
            .Font = FNT_LABEL,
            .Size = New Size(90, 26),
            .Location = New Point(pnlHeader.Width - 290, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(39, 174, 96),
            .ForeColor = Color.White
        }
        AddHandler btnNewEntry.Click, AddressOf BtnNewEntry_Click
        pnlHeader.Controls.Add(btnNewEntry)

        btnEdit = New Button() With {
            .Text = "変更",
            .Font = FNT_LABEL,
            .Size = New Size(80, 26),
            .Location = New Point(pnlHeader.Width - 190, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(243, 156, 18),
            .ForeColor = Color.White
        }
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        pnlHeader.Controls.Add(btnEdit)

        btnInquiry = New Button() With {
            .Text = "照会",
            .Font = FNT_LABEL,
            .Size = New Size(80, 26),
            .Location = New Point(pnlHeader.Width - 100, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(142, 68, 173),
            .ForeColor = Color.White
        }
        AddHandler btnInquiry.Click, AddressOf BtnInquiry_Click
        pnlHeader.Controls.Add(btnInquiry)

        Me.Controls.Add(pnlHeader)

        ' === DataGridView (中央) ===
        dgvContractList = New DataGridView() With {
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
        AddGridColumns()
        AddHandler dgvContractList.CellDoubleClick, AddressOf DgvContractList_CellDoubleClick
        Me.Controls.Add(dgvContractList)

        ' 順序調整
        dgvContractList.BringToFront()
    End Sub

    Private Sub AddGridColumns()
        dgvContractList.Columns.Add("colCtbId", "管理ID")
        dgvContractList.Columns.Add("colContractNo", "契約番号")
        dgvContractList.Columns.Add("colPropertyNo", "物件No")
        dgvContractList.Columns.Add("colContractName", "契約名")
        dgvContractList.Columns.Add("colAssetNo", "資産番号")
        dgvContractList.Columns.Add("colAssetName", "資産名")
        dgvContractList.Columns.Add("colAssetCategory", "資産種類")
        dgvContractList.Columns.Add("colStartDate", "開始日")
        dgvContractList.Columns.Add("colEndDate", "終了日")
        dgvContractList.Columns.Add("colContractPeriod", "期間(月)")
        dgvContractList.Columns.Add("colDeptName", "配賦部門")
        dgvContractList.Columns.Add("colAllocationRatio", "配賦率(%)")
        dgvContractList.Columns.Add("colTotalPayment", "合計支払額")
        dgvContractList.Columns.Add("colSplitStatus", "状況")

        ' 右寄せ
        For Each col As String In {"colCtbId", "colPropertyNo", "colContractPeriod", "colAllocationRatio", "colTotalPayment"}
            dgvContractList.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub

    ''' <summary>
    ''' DataGridView のヘッダースタイルを設定する
    ''' </summary>
    Private Sub ApplyGridStyles()
        dgvContractList.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(240, 244, 248),
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL,
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }
        dgvContractList.DefaultCellStyle = New DataGridViewCellStyle() With {
            .Font = FNT_INPUT,
            .ForeColor = CLR_TEXT,
            .SelectionBackColor = Color.FromArgb(209, 226, 243),
            .SelectionForeColor = CLR_TEXT
        }
        dgvContractList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(248, 249, 250)
        }
    End Sub

    ' =====================================================
    ' CTBデータロジック（変更なし）
    ' =====================================================

    ''' <summary>
    ''' DBとメモリストアから次の契約番号を生成する
    ''' </summary>
    Private Shared Function GetNextContractNo() As String
        Dim maxCounter As Integer = 0

        ' DBから最大カウンタを取得
        Try
            Dim connMgr As New LeaseM4BS.DataAccess.DbConnectionManager()
            Using conn = connMgr.GetConnection()
                ' 全契約番号を取得してVB側でパース（DB関数依存を排除）
                Using cmd As New Npgsql.NpgsqlCommand(
                    "SELECT contract_no FROM ctb_lease_integrated WHERE contract_no LIKE 'LC-%'", conn)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim contractNo As String = reader.GetString(0)
                            Dim num As Integer = ParseContractCounter(contractNo)
                            If num > maxCounter Then maxCounter = num
                        End While
                    End Using
                End Using
            End Using
        Catch
        End Try

        ' メモリストアにのみ存在するレコードも考慮
        For Each rec In CtbDataStore.Instance.GetAll()
            Dim num As Integer = ParseContractCounter(rec.ContractNo)
            If num > maxCounter Then maxCounter = num
        Next

        Return String.Format("LC-{0}-{1:D4}", Date.Now.Year, maxCounter + 1)
    End Function

    ''' <summary>
    ''' 契約番号 "LC-YYYY-NNNN" からカウンタ部分を取得する
    ''' </summary>
    Private Shared Function ParseContractCounter(contractNo As String) As Integer
        If String.IsNullOrEmpty(contractNo) OrElse Not contractNo.StartsWith("LC-") Then Return 0
        Dim parts = contractNo.Split("-"c)
        If parts.Length <> 3 Then Return 0
        Dim num As Integer
        If Integer.TryParse(parts(2), num) Then Return num
        Return 0
    End Function

    ''' <summary>
    ''' CTBデータストアからレコードを読み込んでグリッドに表示する
    ''' </summary>
    Private Sub LoadCtbData()
        dgvContractList.Rows.Clear()

        ' DBから取得を試み、失敗時はメモリストアにフォールバック
        Dim records As List(Of CtbRecord)
        Try
            Dim repo As New CtbRepository()
            records = repo.SelectAll()
        Catch
            records = CtbDataStore.Instance.GetAll()
        End Try

        For Each rec As CtbRecord In records
            Dim rowIndex As Integer = dgvContractList.Rows.Add()
            Dim dgvRow As DataGridViewRow = dgvContractList.Rows(rowIndex)
            dgvRow.Cells("colCtbId").Value = rec.CtbId
            dgvRow.Cells("colContractNo").Value = rec.ContractNo
            ' 契約番号末尾(上) - property_no(下) 形式で表示
            Dim contractSuffix As String = ""
            Dim parts = rec.ContractNo.Split("-"c)
            If parts.Length = 3 Then contractSuffix = parts(2)
            dgvRow.Cells("colPropertyNo").Value = contractSuffix & "-" & rec.PropertyNo.ToString()
            dgvRow.Cells("colContractName").Value = rec.ContractName
            dgvRow.Cells("colAssetNo").Value = rec.AssetNo
            dgvRow.Cells("colAssetName").Value = rec.AssetName
            dgvRow.Cells("colAssetCategory").Value = rec.AssetCategory
            dgvRow.Cells("colStartDate").Value = If(rec.LeaseStartDate.HasValue,
                rec.LeaseStartDate.Value.ToString("yyyy/MM/dd"), "")
            dgvRow.Cells("colEndDate").Value = If(rec.LeaseEndDate.HasValue,
                rec.LeaseEndDate.Value.ToString("yyyy/MM/dd"), "")
            dgvRow.Cells("colContractPeriod").Value = If(rec.LeaseTermMonths.HasValue,
                rec.LeaseTermMonths.Value.ToString(), "")
            dgvRow.Cells("colDeptName").Value = rec.DeptName
            dgvRow.Cells("colAllocationRatio").Value = rec.AllocationRatio
            dgvRow.Cells("colTotalPayment").Value = rec.TotalPayment
            dgvRow.Cells("colSplitStatus").Value = If(rec.SplitStatus = "unsplit", "未分割", "分割済")
        Next
    End Sub

    ' =====================================================
    ' イベントハンドラ
    ' =====================================================

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
        LoadCtbData()
    End Sub

    Private Sub BtnNewEntry_Click(sender As Object, e As EventArgs)
        OpenContractMain("", ContractOpenMode.NewEntry)
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        Dim contractNo As String = GetSelectedContractNo()
        If String.IsNullOrEmpty(contractNo) Then
            MessageBox.Show(
                "変更する契約を一覧から選択してください。",
                "選択エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If
        OpenContractMain(contractNo, ContractOpenMode.Edit)
    End Sub

    Private Sub BtnInquiry_Click(sender As Object, e As EventArgs)
        Dim contractNo As String = GetSelectedContractNo()
        If String.IsNullOrEmpty(contractNo) Then
            MessageBox.Show(
                "照会する契約を一覧から選択してください。",
                "選択エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If
        OpenContractMain(contractNo, ContractOpenMode.Inquiry)
    End Sub

    Private Sub DgvContractList_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return

        Dim row As DataGridViewRow = dgvContractList.Rows(e.RowIndex)
        Dim contractNo As String = ""
        If row.Cells("colContractNo").Value IsNot Nothing Then
            contractNo = row.Cells("colContractNo").Value.ToString()
        End If

        OpenContractMain(contractNo, ContractOpenMode.Edit)
    End Sub

    ' =====================================================
    ' ヘルパー
    ' =====================================================

    Private Function GetSelectedContractNo() As String
        If dgvContractList.SelectedRows.Count = 0 Then Return ""
        Dim row As DataGridViewRow = dgvContractList.SelectedRows(0)
        If row.Cells("colContractNo").Value IsNot Nothing Then
            Return row.Cells("colContractNo").Value.ToString()
        End If
        Return ""
    End Function

    Private Sub OpenContractMain(contractNo As String, mode As ContractOpenMode)
        Try
            Dim frm As New FrmLeaseContractMain()

            Select Case mode
                Case ContractOpenMode.NewEntry
                    frm.InitContractNo = GetNextContractNo()
                    frm.Text = "新リース会計対応 リース契約管理 - 新規登録"
                    frm.Tag = ""
                Case ContractOpenMode.Edit
                    frm.Text = "新リース会計対応 リース契約管理 - " & contractNo
                    frm.Tag = contractNo
                Case ContractOpenMode.Inquiry
                    frm.Text = "新リース会計対応 リース契約管理 - 照会 - " & contractNo
                    frm.Tag = contractNo
            End Select

            AddHandler frm.ContractRegistered, AddressOf OnContractRegistered
            frm.Show()
        Catch ex As Exception
            MessageBox.Show(
                "契約詳細画面の表示中にエラーが発生しました。" & Environment.NewLine & ex.Message,
                "画面遷移エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnContractRegistered(sender As Object, e As FrmLeaseContractMain.ContractRegisteredEventArgs)
        LoadCtbData()
    End Sub

    Private Enum ContractOpenMode
        NewEntry
        Edit
        Inquiry
    End Enum

End Class
