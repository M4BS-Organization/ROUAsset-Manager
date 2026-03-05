Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 契約書（フレックス）画面
''' 契約一覧の検索・表示と、契約詳細画面への遷移を提供する。
''' </summary>
Partial Public Class FrmFlexContract

    Private ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
    Private ReadOnly CLR_CARD As Color = Color.White
    Private ReadOnly CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
    Private ReadOnly CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
    Private ReadOnly CLR_BORDER As Color = Color.FromArgb(222, 226, 230)
    Private ReadOnly CLR_ACCENT As Color = Color.FromArgb(0, 123, 255)

    Private ReadOnly FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)
    Private ReadOnly FNT_SECTION As New Font("Meiryo", 10.0F, FontStyle.Bold)

    ''' <summary>
    ''' 契約番号の自動採番用カウンタ
    ''' </summary>
    Private Shared _contractCounter As Integer = 0

    ''' <summary>
    ''' 管理番号の自動採番用カウンタ
    ''' </summary>
    Private Shared _managementCounter As Integer = 0

    ''' <summary>
    ''' 稟議番号の自動採番用カウンタ
    ''' </summary>
    Private Shared _approvalCounter As Integer = 0

    Public Sub New()
        InitializeComponent()
        ApplyGridStyles()
        LoadSampleData()
    End Sub

    ''' <summary>
    ''' DataGridView のヘッダースタイルを設定する
    ''' </summary>
    Private Sub ApplyGridStyles()
        dgvContractList.ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(240, 244, 248),
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
            .ForeColor = CLR_LABEL,
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        }
        dgvContractList.DefaultCellStyle = New DataGridViewCellStyle() With {
            .Font = New Font("Meiryo", 9.75F, FontStyle.Regular),
            .ForeColor = CLR_TEXT,
            .SelectionBackColor = Color.FromArgb(209, 226, 243),
            .SelectionForeColor = CLR_TEXT
        }
        dgvContractList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {
            .BackColor = Color.FromArgb(248, 249, 250)
        }
    End Sub

    ''' <summary>
    ''' DBデータが未格納の状態でも一覧にサンプル行を表示する
    ''' </summary>
    Private Sub LoadSampleData()
        dgvContractList.Rows.Clear()

        ' サンプルデータ（5行）
        Dim sampleData()() As String = {
            New String() {"本社", "新規", "対象", "ABC不動産", "LC-2025-0001", "MGR-001", "APP-2025-0001", "0", "本社ビル賃貸借契約", "2025/04/01", "2030/03/31", "60", "50,000,000", "800,000", "3", "2025/03/15 10:30", "○"},
            New String() {"本社", "新規", "対象", "XYZ設備リース", "LC-2025-0002", "MGR-002", "APP-2025-0002", "0", "複合機リース契約", "2025/04/01", "2029/03/31", "48", "3,600,000", "75,000", "5", "2025/03/15 11:00", "○"},
            New String() {"大阪支店", "新規", "対象", "DEFモータース", "LC-2025-0003", "MGR-003", "APP-2025-0003", "0", "社用車リース契約", "2025/05/01", "2028/04/30", "36", "4,800,000", "133,333", "2", "2025/03/20 09:15", "○"},
            New String() {"本社", "更新", "対象", "GHIテクノロジー", "LC-2025-0004", "MGR-004", "APP-2025-0004", "1", "サーバー機器リース", "2025/06/01", "2030/05/31", "60", "12,000,000", "200,000", "10", "2025/04/01 14:00", "―"},
            New String() {"名古屋支店", "新規", "除外", "JKLプロパティ", "LC-2025-0005", "MGR-005", "APP-2025-0005", "0", "倉庫賃貸借契約", "2025/07/01", "2035/06/30", "120", "80,000,000", "666,667", "1", "2025/05/10 16:45", "○"}
        }

        For Each rowData As String() In sampleData
            Dim rowIndex As Integer = dgvContractList.Rows.Add()
            Dim dgvRow As DataGridViewRow = dgvContractList.Rows(rowIndex)
            dgvRow.Cells("colMgmtUnit").Value = rowData(0)
            dgvRow.Cells("colContractType").Value = rowData(1)
            dgvRow.Cells("colAccountTarget").Value = rowData(2)
            dgvRow.Cells("colPayee").Value = rowData(3)
            dgvRow.Cells("colContractNo").Value = rowData(4)
            dgvRow.Cells("colOwnMgmt").Value = rowData(5)
            dgvRow.Cells("colApprovalNo").Value = rowData(6)
            dgvRow.Cells("colReleaseCount").Value = rowData(7)
            dgvRow.Cells("colContractName").Value = rowData(8)
            dgvRow.Cells("colStartDate").Value = rowData(9)
            dgvRow.Cells("colEndDate").Value = rowData(10)
            dgvRow.Cells("colContractPeriod").Value = rowData(11)
            dgvRow.Cells("colCashPrice").Value = rowData(12)
            dgvRow.Cells("colMonthlyLease").Value = rowData(13)
            dgvRow.Cells("colAssetQty").Value = rowData(14)
            dgvRow.Cells("colUpdateDate").Value = rowData(15)
            dgvRow.Cells("colConsistency").Value = rowData(16)
        Next
    End Sub

    ''' <summary>
    ''' 検索ボタンクリック時の処理
    ''' </summary>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        SearchContracts()
    End Sub

    ''' <summary>
    ''' 新規登録ボタンクリック時の処理
    ''' FrmLeaseContractMain を新規作成モードで開く
    ''' </summary>
    Private Sub btnNewEntry_Click(sender As Object, e As EventArgs) Handles btnNewEntry.Click
        OpenContractMain("", ContractOpenMode.NewEntry)
    End Sub

    ''' <summary>
    ''' 変更ボタンクリック時の処理
    ''' グリッドで選択されている行の契約番号を取得し、FrmLeaseContractMain を編集モードで開く
    ''' </summary>
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
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

    ''' <summary>
    ''' 照会ボタンクリック時の処理
    ''' グリッドで選択されている行の契約番号を取得し、FrmLeaseContractMain を読み取り専用モードで開く
    ''' </summary>
    Private Sub btnInquiry_Click(sender As Object, e As EventArgs) Handles btnInquiry.Click
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

    ''' <summary>
    ''' グリッドで選択されている行の契約番号を取得する
    ''' </summary>
    Private Function GetSelectedContractNo() As String
        If dgvContractList.SelectedRows.Count = 0 Then Return ""
        Dim row As DataGridViewRow = dgvContractList.SelectedRows(0)
        If row.Cells("colContractNo").Value IsNot Nothing Then
            Return row.Cells("colContractNo").Value.ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' 検索条件に基づいて契約一覧を取得・表示する
    ''' </summary>
    Private Sub SearchContracts()
        Try
            Dim userFilter As String = If(cboUser.SelectedItem IsNot Nothing, cboUser.SelectedItem.ToString(), "")
            Dim contractNoFilter As String = txtContractNo.Text.Trim()

            Using db As New CrudHelper()
                Dim sql As String = "SELECT * FROM d_asset WHERE 1=1"
                Dim parameters As New List(Of NpgsqlParameter)()

                If Not String.IsNullOrEmpty(userFilter) Then
                    sql &= " AND (upd_user_nm LIKE @user OR upd_user_id LIKE @user)"
                    parameters.Add(New NpgsqlParameter("@user", "%" & userFilter & "%"))
                End If

                If Not String.IsNullOrEmpty(contractNoFilter) Then
                    sql &= " AND contract_no LIKE @contractNo"
                    parameters.Add(New NpgsqlParameter("@contractNo", "%" & contractNoFilter & "%"))
                End If

                sql &= " ORDER BY contract_no"

                Dim dt As DataTable = db.GetDataTable(sql, parameters)
                BindDataToGrid(dt)
            End Using

        Catch ex As Exception
            MessageBox.Show(
                "契約データの検索中にエラーが発生しました。" & Environment.NewLine & ex.Message,
                "検索エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' DataTable の内容を DataGridView にバインドする
    ''' </summary>
    Private Sub BindDataToGrid(dt As DataTable)
        dgvContractList.Rows.Clear()

        For Each row As DataRow In dt.Rows
            Dim rowIndex As Integer = dgvContractList.Rows.Add()
            Dim dgvRow As DataGridViewRow = dgvContractList.Rows(rowIndex)

            ' カラムマッピング: d_asset マスタFK参照カラムへ更新
            '   mgmt_unit     → contract_mgmt_unit_cd (m_contract_mgmt_unit)
            '   contract_type → property_type_cd      (m_property_type)
            '   payee         → supplier_cd           (m_supplier)
            dgvRow.Cells("colMgmtUnit").Value = SafeValue(row, "contract_mgmt_unit_cd") ' 旧 mgmt_unit
            dgvRow.Cells("colContractType").Value = SafeValue(row, "property_type_cd")  ' 旧 contract_type
            dgvRow.Cells("colAccountTarget").Value = SafeValue(row, "acct_target")
            dgvRow.Cells("colPayee").Value = SafeValue(row, "supplier_cd")              ' 旧 payee
            dgvRow.Cells("colContractNo").Value = SafeValue(row, "contract_no")       ' kykno → contract_no
            dgvRow.Cells("colOwnMgmt").Value = SafeValue(row, "own_mgmt")             ' jshknr → own_mgmt
            dgvRow.Cells("colApprovalNo").Value = SafeValue(row, "approval_no")       ' rngno → approval_no
            dgvRow.Cells("colReleaseCount").Value = SafeValue(row, "re_lease_cnt")    ' srsks → re_lease_cnt
            dgvRow.Cells("colContractName").Value = SafeValue(row, "contract_nm")     ' kyknm → contract_nm
            dgvRow.Cells("colStartDate").Value = SafeDateValue(row, "start_dt")       ' kisymd → start_dt
            dgvRow.Cells("colEndDate").Value = SafeDateValue(row, "end_dt")           ' syrymd → end_dt
            dgvRow.Cells("colContractPeriod").Value = SafeValue(row, "contract_period") ' kykkk → contract_period
            dgvRow.Cells("colCashPrice").Value = SafeDecimalValue(row, "cash_price")  ' gnknkngk → cash_price
            dgvRow.Cells("colMonthlyLease").Value = SafeDecimalValue(row, "monthly_lease") ' gtkrsry → monthly_lease
            dgvRow.Cells("colAssetQty").Value = SafeValue(row, "quantity")             ' ssnsry → quantity
            dgvRow.Cells("colUpdateDate").Value = SafeDateTimeValue(row, "update_dt") ' kosndt → update_dt
            dgvRow.Cells("colConsistency").Value = SafeValue(row, "consistency")       ' sigo → consistency
        Next
    End Sub

    ''' <summary>
    ''' DataRow から安全に文字列値を取得する
    ''' </summary>
    Private Function SafeValue(row As DataRow, columnName As String) As String
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Return row(columnName).ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' DataRow から安全に日付値を取得する（yyyy/MM/dd 形式）
    ''' </summary>
    Private Function SafeDateValue(row As DataRow, columnName As String) As String
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Dim dateVal As Date
            If Date.TryParse(row(columnName).ToString(), dateVal) Then
                Return dateVal.ToString("yyyy/MM/dd")
            End If
            Return row(columnName).ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' DataRow から安全に日時値を取得する（yyyy/MM/dd HH:mm 形式）
    ''' </summary>
    Private Function SafeDateTimeValue(row As DataRow, columnName As String) As String
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Dim dateVal As Date
            If Date.TryParse(row(columnName).ToString(), dateVal) Then
                Return dateVal.ToString("yyyy/MM/dd HH:mm")
            End If
            Return row(columnName).ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' DataRow から安全に数値を取得する
    ''' </summary>
    Private Function SafeDecimalValue(row As DataRow, columnName As String) As Object
        If row.Table.Columns.Contains(columnName) AndAlso Not IsDBNull(row(columnName)) Then
            Dim decVal As Decimal
            If Decimal.TryParse(row(columnName).ToString(), decVal) Then
                Return decVal
            End If
            Return row(columnName)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' グリッド行ダブルクリック時に FrmLeaseContractMain を編集モードで開く
    ''' </summary>
    Private Sub dgvContractList_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvContractList.CellDoubleClick
        If e.RowIndex < 0 Then Return

        Dim row As DataGridViewRow = dgvContractList.Rows(e.RowIndex)
        Dim contractNo As String = ""
        If row.Cells("colContractNo").Value IsNot Nothing Then
            contractNo = row.Cells("colContractNo").Value.ToString()
        End If

        OpenContractMain(contractNo, ContractOpenMode.Edit)
    End Sub

    ''' <summary>
    ''' FrmLeaseContractMain を指定モードで開く
    ''' </summary>
    Private Sub OpenContractMain(contractNo As String, mode As ContractOpenMode)
        Try
            Dim frm As New FrmLeaseContractMain()

            Select Case mode
                Case ContractOpenMode.NewEntry
                    ' 新規登録時: 各番号を自動採番してセット
                    _contractCounter += 1
                    _managementCounter += 1
                    _approvalCounter += 1
                    frm.InitContractNo = String.Format("LC-{0}-{1:D4}", Date.Now.Year, _contractCounter)
                    frm.InitManagementNo = String.Format("MGMT-{0}-{1:D4}", Date.Now.Year, _managementCounter)
                    frm.InitApprovalNo = String.Format("APP-{0}-{1:D4}", Date.Now.Year, _approvalCounter)
                    frm.Text = "新リース会計対応 リース契約管理 - 新規登録"
                    frm.Tag = ""
                Case ContractOpenMode.Edit
                    frm.Text = "新リース会計対応 リース契約管理 - " & contractNo
                    frm.Tag = contractNo
                Case ContractOpenMode.Inquiry
                    frm.Text = "新リース会計対応 リース契約管理 - 照会 - " & contractNo
                    frm.Tag = contractNo
                    ' TODO: 将来的に ReadOnly モードの制御を追加
            End Select

            frm.Show()
        Catch ex As Exception
            MessageBox.Show(
                "契約詳細画面の表示中にエラーが発生しました。" & Environment.NewLine & ex.Message,
                "画面遷移エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 契約画面の表示モード
    ''' </summary>
    Private Enum ContractOpenMode
        ''' <summary>新規登録</summary>
        NewEntry
        ''' <summary>変更（編集）</summary>
        Edit
        ''' <summary>照会（読み取り専用）</summary>
        Inquiry
    End Enum

End Class
