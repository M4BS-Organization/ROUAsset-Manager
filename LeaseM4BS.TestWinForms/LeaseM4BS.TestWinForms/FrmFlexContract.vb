Imports System
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' 契約書（フレックス）画面
''' CTBデータストアからCTBレコード（管理ID単位）を一覧表示する。
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

    Public Sub New()
        InitializeComponent()
        ApplyGridStyles()
        LoadCtbData()
    End Sub

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

        ' データが0件の場合、案内メッセージ表示用の空行は追加しない
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
        LoadCtbData()
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
                    ' 新規登録時: 契約番号を自動採番してセット
                    frm.InitContractNo = GetNextContractNo()
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

    ''' <summary>
    ''' 契約登録イベントハンドラ: 登録完了後にCTBデータストアから一覧を再読込する
    ''' </summary>
    Private Sub OnContractRegistered(sender As Object, e As FrmLeaseContractMain.ContractRegisteredEventArgs)
        LoadCtbData()
    End Sub

    Private Enum ContractOpenMode
        ''' <summary>新規登録</summary>
        NewEntry
        ''' <summary>変更（編集）</summary>
        Edit
        ''' <summary>照会（読み取り専用）</summary>
        Inquiry
    End Enum

End Class
