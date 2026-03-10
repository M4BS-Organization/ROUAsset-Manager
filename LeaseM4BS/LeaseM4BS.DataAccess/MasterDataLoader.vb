Imports System.Data

''' <summary>
''' マスタテーブルからデータを読み込むユーティリティクラス。
''' DB接続失敗時は空のリストを返す（フォールバック値なし）。
''' </summary>
Public Class MasterDataLoader

    Private _crud As CrudHelper
    Private _isConnected As Boolean = False

    ''' <summary>DB接続の可否</summary>
    Public ReadOnly Property IsConnected As Boolean
        Get
            Return _isConnected
        End Get
    End Property

    Public Sub New()
        Try
            _crud = New CrudHelper()
            _isConnected = True
        Catch
            _isConnected = False
        End Try
    End Sub

    ''' <summary>
    ''' 汎用: 指定テーブルからコード・名称の一覧を取得する。
    ''' DB未接続時は空のDataTableを返す。
    ''' </summary>
    Private Function LoadCodeName(tableName As String, codCol As String, nmCol As String,
                                  Optional orderCol As String = Nothing) As DataTable
        If Not _isConnected Then Return CreateEmptyCodeNameTable(codCol, nmCol)

        Try
            Dim orderBy As String = If(orderCol IsNot Nothing, orderCol, codCol)
            Dim sql As String = String.Format("SELECT {0}, {1} FROM {2} ORDER BY {3}",
                                              codCol, nmCol, tableName, orderBy)
            Return _crud.GetDataTable(sql)
        Catch
            Return CreateEmptyCodeNameTable(codCol, nmCol)
        End Try
    End Function

    Private Function CreateEmptyCodeNameTable(codCol As String, nmCol As String) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add(codCol, GetType(String))
        dt.Columns.Add(nmCol, GetType(String))
        Return dt
    End Function

    ' ------------------------------------------------------------------
    ' 各マスタのロードメソッド (DBデータのみ)
    ' ------------------------------------------------------------------

    ''' <summary>部署マスタ: コード＋名称</summary>
    Public Function LoadDepartments() As DataTable
        Return LoadCodeName("m_department", "dept_cd", "dept_nm")
    End Function

    ''' <summary>取引先マスタ: コード＋名称</summary>
    Public Function LoadSuppliers() As DataTable
        Return LoadCodeName("m_supplier", "supplier_cd", "supplier_nm")
    End Function

    ''' <summary>契約種類マスタ: 名称リスト</summary>
    Public Function LoadContractTypes() As String()
        Return LoadNameList("m_contract_type", "contract_type_cd", "contract_type_nm", "sort_order")
    End Function

    ''' <summary>契約種類マスタ: コード＋名称</summary>
    Public Function LoadContractTypesTable() As DataTable
        Return LoadCodeName("m_contract_type", "contract_type_cd", "contract_type_nm", "sort_order")
    End Function

    ''' <summary>初回費用費目マスタ: 名称リスト</summary>
    Public Function LoadInitialCostItems() As String()
        Return LoadNameList("m_initial_cost_item", "cost_item_cd", "cost_item_nm", "sort_order")
    End Function

    ''' <summary>会計処理区分マスタ: 名称リスト</summary>
    Public Function LoadAcctTreatments() As String()
        Return LoadNameList("m_acct_treatment", "acct_treatment_cd", "acct_treatment_nm", "sort_order")
    End Function

    ''' <summary>月額科目マスタ: 名称リスト</summary>
    Public Function LoadMonthlyItems() As String()
        Return LoadNameList("m_monthly_item", "monthly_item_cd", "monthly_item_nm", "sort_order")
    End Function

    ''' <summary>支払方法マスタ: 名称リスト</summary>
    Public Function LoadPaymentMethods() As String()
        Return LoadNameList("m_payment_method", "payment_method_cd", "payment_method_nm")
    End Function

    ''' <summary>銀行口座マスタ: 名称リスト</summary>
    Public Function LoadBankAccounts() As String()
        Return LoadNameList("m_bank_account", "bank_account_cd", "bank_account_nm")
    End Function

    ''' <summary>資産区分マスタ: 名称リスト</summary>
    Public Function LoadAssetCategories() As String()
        Return LoadNameList("m_asset_category", "asset_category_cd", "asset_category_nm")
    End Function

    ' ------------------------------------------------------------------
    ' 内部ヘルパー
    ' ------------------------------------------------------------------

    ''' <summary>
    ''' テーブルから名称カラムだけを String() で返す。
    ''' DB未接続 or 0件時は空配列を返す。
    ''' </summary>
    Private Function LoadNameList(tableName As String, codCol As String, nmCol As String,
                                  Optional orderCol As String = Nothing) As String()
        Dim dt As DataTable = LoadCodeName(tableName, codCol, nmCol, orderCol)
        If dt.Rows.Count = 0 Then Return New String() {}

        Dim result(dt.Rows.Count - 1) As String
        For i As Integer = 0 To dt.Rows.Count - 1
            result(i) = dt.Rows(i)(nmCol).ToString()
        Next
        Return result
    End Function

    ''' <summary>リソース解放</summary>
    Public Sub Dispose()
        If _crud IsNot Nothing Then
            _crud.Dispose()
            _crud = Nothing
        End If
    End Sub

End Class
