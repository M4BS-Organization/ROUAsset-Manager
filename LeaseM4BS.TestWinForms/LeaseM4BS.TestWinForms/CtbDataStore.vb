Imports System.Collections.Generic

''' <summary>
''' CTBレコード: ctb_lease_integrated + ctb_dept_allocation を統合した1レコード
''' 1レコード = 1契約 × 1資産 × 1配賦部門
''' ※種別固有情報はctb_property_attribute（EAV）に移行済み
''' </summary>
Public Class CtbRecord
    ' === 識別キー ===
    Public Property CtbId As Integer
    Public Property ContractNo As String = ""
    Public Property PropertyNo As Integer = 1

    ' === 契約画面: 基本・管理情報 ===
    Public Property ContractName As String = ""
    Public Property ContractTypeCd As String = ""
    Public Property SupplierCd As String = ""
    Public Property MgmtDeptCd As String = ""
    Public Property LeaseStartDate As Date? = Nothing
    Public Property LeaseEndDate As Date? = Nothing
    Public Property FreeRentMonths As Integer = 0
    Public Property LeaseTermMonths As Integer? = Nothing

    ' === 資産入力画面: 基本情報 ===
    Public Property AssetNo As String = ""
    Public Property AssetCategoryCd As String = ""
    Public Property AssetName As String = ""
    Public Property CompanyName As String = ""
    Public Property InstallLocation As String = ""
    Public Property Remarks As String = ""

    ' === 配賦情報（1レコードに1部門） ===
    Public Property DeptCd As String = ""
    Public Property DeptName As String = ""
    Public Property AllocationRatio As Decimal = 0D

    ' === 金額・状況 ===
    Public Property MonthlyPayment As Decimal = 0D
    Public Property LeaseDepreciation As Decimal = 0D
    Public Property TotalPayment As Decimal = 0D
    Public Property SplitStatus As String = "unsplit"

    ' === 入力時の一時保持用（複数部門） ===
    Public Property DeptAllocations As New List(Of CtbDeptAllocation)

    ' === 物件マスタ参照（EAV属性含む） ===
    Public Property PropertyRec As PropertyRecord = Nothing

    ''' <summary>
    ''' 配賦部門の表示文字列（例: "本社(100%)"）
    ''' </summary>
    Public ReadOnly Property DeptAllocationDisplay As String
        Get
            If String.IsNullOrEmpty(DeptName) Then Return ""
            Return String.Format("{0}({1}%)", DeptName, AllocationRatio)
        End Get
    End Property
End Class

''' <summary>
''' 配賦明細: 資産入力画面からの戻り値用
''' </summary>
Public Class CtbDeptAllocation
    Public Property DeptCd As String = ""
    Public Property DeptName As String = ""
    Public Property AllocationRatio As Decimal = 0D
    Public Property PaymentAmount As Decimal = 0D
End Class

''' <summary>
''' CTBデータストア: DB永続化を一次的に行い、メモリキャッシュも保持するシングルトン
''' DB接続失敗時はメモリのみで動作（フォールバック）
''' </summary>
Public Class CtbDataStore
    Private Shared _instance As CtbDataStore
    Private Shared ReadOnly _lock As New Object()

    Private ReadOnly _records As New List(Of CtbRecord)
    Private ReadOnly _repo As New CtbRepository()
    Private _nextCtbId As Integer = 1
    Private _dbAvailable As Boolean = True

    Private Sub New()
        ' 初回: DBからキャッシュをロード
        Try
            Dim dbRecords = _repo.SelectAll()
            _records.AddRange(dbRecords)
            If _records.Count > 0 Then
                _nextCtbId = _records.Max(Function(r) r.CtbId) + 1
            End If
        Catch
            _dbAvailable = False
        End Try
    End Sub

    Public Shared ReadOnly Property Instance As CtbDataStore
        Get
            If _instance Is Nothing Then
                SyncLock _lock
                    If _instance Is Nothing Then
                        _instance = New CtbDataStore()
                    End If
                End SyncLock
            End If
            Return _instance
        End Get
    End Property

    ''' <summary>
    ''' レコード追加: DB永続化→メモリキャッシュ追加
    ''' </summary>
    Public Sub Add(record As CtbRecord)
        record.CtbId = _nextCtbId
        _nextCtbId += 1
        _records.Add(record)
    End Sub

    ''' <summary>
    ''' 複数レコードをDB永続化 + メモリキャッシュ更新
    ''' </summary>
    Public Function SaveToDb(records As List(Of CtbRecord)) As Boolean
        If records Is Nothing OrElse records.Count = 0 Then Return True

        Try
            _repo.UpsertAll(records)
            ' DB成功: メモリキャッシュをリフレッシュ
            RefreshFromDb()
            _dbAvailable = True
            Return True
        Catch
            _dbAvailable = False
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 契約番号を指定して論理削除（DB + メモリキャッシュ）
    ''' </summary>
    Public Function DeleteByContractNo(contractNo As String) As Boolean
        Try
            _repo.SoftDeleteByContractNo(contractNo)
            _records.RemoveAll(Function(r) r.ContractNo = contractNo)
            Return True
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' DBからメモリキャッシュをリフレッシュ
    ''' </summary>
    Public Sub RefreshFromDb()
        Try
            Dim dbRecords = _repo.SelectAll()
            _records.Clear()
            _records.AddRange(dbRecords)
            If _records.Count > 0 Then
                _nextCtbId = _records.Max(Function(r) r.CtbId) + 1
            End If
            _dbAvailable = True
        Catch
            _dbAvailable = False
        End Try
    End Sub

    Public Function GetAll() As List(Of CtbRecord)
        Return New List(Of CtbRecord)(_records)
    End Function

    Public Function GetByContractNo(contractNo As String) As List(Of CtbRecord)
        Dim result As New List(Of CtbRecord)
        For Each r In _records
            If r.ContractNo = contractNo Then
                result.Add(r)
            End If
        Next
        Return result
    End Function

    Public ReadOnly Property Count As Integer
        Get
            Return _records.Count
        End Get
    End Property

    ''' <summary>DB接続が利用可能かどうか</summary>
    Public ReadOnly Property IsDbAvailable As Boolean
        Get
            Return _dbAvailable
        End Get
    End Property
End Class
