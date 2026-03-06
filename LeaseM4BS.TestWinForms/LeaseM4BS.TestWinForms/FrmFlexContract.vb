Imports System
Imports System.Drawing
Imports System.Windows.Forms

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
    ''' 検索条件に基づいて契約一覧を取得・表示する（DB未接続のためサンプル表示）
    ''' </summary>
    Private Sub SearchContracts()
        LoadSampleData()
    End Sub

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
