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

    Public Sub New()
        InitializeComponent()
        ApplyGridStyles()
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
                Dim sql As String = "SELECT * FROM td_sisn WHERE 1=1"
                Dim parameters As New List(Of NpgsqlParameter)()

                If Not String.IsNullOrEmpty(userFilter) Then
                    sql &= " AND (updusrnm LIKE @user OR updusrid LIKE @user)"
                    parameters.Add(New NpgsqlParameter("@user", "%" & userFilter & "%"))
                End If

                If Not String.IsNullOrEmpty(contractNoFilter) Then
                    sql &= " AND kykno LIKE @contractNo"
                    parameters.Add(New NpgsqlParameter("@contractNo", "%" & contractNoFilter & "%"))
                End If

                sql &= " ORDER BY kykno"

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

            dgvRow.Cells("colMgmtUnit").Value = SafeValue(row, "knrtni")
            dgvRow.Cells("colContractType").Value = SafeValue(row, "kykkbn")
            dgvRow.Cells("colAccountTarget").Value = SafeValue(row, "kjotis")
            dgvRow.Cells("colPayee").Value = SafeValue(row, "shrsk")
            dgvRow.Cells("colContractNo").Value = SafeValue(row, "kykno")
            dgvRow.Cells("colOwnMgmt").Value = SafeValue(row, "jshknr")
            dgvRow.Cells("colApprovalNo").Value = SafeValue(row, "rngno")
            dgvRow.Cells("colReleaseCount").Value = SafeValue(row, "srsks")
            dgvRow.Cells("colContractName").Value = SafeValue(row, "kyknm")
            dgvRow.Cells("colStartDate").Value = SafeDateValue(row, "kisymd")
            dgvRow.Cells("colEndDate").Value = SafeDateValue(row, "syrymd")
            dgvRow.Cells("colContractPeriod").Value = SafeValue(row, "kykkk")
            dgvRow.Cells("colCashPrice").Value = SafeDecimalValue(row, "gnknkngk")
            dgvRow.Cells("colMonthlyLease").Value = SafeDecimalValue(row, "gtkrsry")
            dgvRow.Cells("colAssetQty").Value = SafeValue(row, "ssnsry")
            dgvRow.Cells("colUpdateDate").Value = SafeDateTimeValue(row, "kosndt")
            dgvRow.Cells("colConsistency").Value = SafeValue(row, "sigo")
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
