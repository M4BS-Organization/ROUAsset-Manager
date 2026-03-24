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
''' CTBデータストア: メモリ上でCTBレコードを管理するシングルトン
''' DB未接続の段階ではこのストアを使用する
''' </summary>
Public Class CtbDataStore
    Private Shared _instance As CtbDataStore
    Private Shared ReadOnly _lock As New Object()

    Private ReadOnly _records As New List(Of CtbRecord)
    Private _nextCtbId As Integer = 1

    Private Sub New()
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

    Public Sub Add(record As CtbRecord)
        record.CtbId = _nextCtbId
        _nextCtbId += 1
        _records.Add(record)
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
End Class
