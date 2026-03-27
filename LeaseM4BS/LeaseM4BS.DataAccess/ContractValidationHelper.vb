''' <summary>
''' 契約入力バリデーションのロジックヘルパー
''' FrmLeaseContractMain.ValidateInput() の純粋ロジック部分を抽出。
''' UI (MessageBox / Focus) に依存しない検証のみを提供する。
''' </summary>
Public NotInheritable Class ContractValidationHelper

    Private Sub New()
    End Sub

    ''' <summary>
    ''' バリデーション結果を格納する構造体
    ''' </summary>
    Public Class ValidationResult
        Public Property IsValid As Boolean = True
        Public Property ErrorField As String = ""
        Public Property ErrorMessage As String = ""

        Public Shared Function Ok() As ValidationResult
            Return New ValidationResult With {.IsValid = True}
        End Function

        Public Shared Function Fail(field As String, message As String) As ValidationResult
            Return New ValidationResult With {
                .IsValid = False,
                .ErrorField = field,
                .ErrorMessage = message
            }
        End Function
    End Class

    ''' <summary>
    ''' チェック1: 契約名称（必須）
    ''' </summary>
    Public Shared Function ValidateContractName(contractName As String) As ValidationResult
        If String.IsNullOrWhiteSpace(contractName) Then
            Return ValidationResult.Fail("ContractName", "「契約名称」を入力してください。")
        End If
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック2: 契約種類（必須選択）
    ''' </summary>
    Public Shared Function ValidateContractType(selectedIndex As Integer) As ValidationResult
        If selectedIndex < 0 Then
            Return ValidationResult.Fail("ContractType", "「契約種類」を選択してください。")
        End If
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック3: 取引先（必須選択）
    ''' </summary>
    Public Shared Function ValidateSupplier(selectedIndex As Integer) As ValidationResult
        If selectedIndex < 0 Then
            Return ValidationResult.Fail("Supplier", "「取引先」を選択してください。")
        End If
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック4: 管理部署（必須選択）
    ''' </summary>
    Public Shared Function ValidateMgmtDeptCode(selectedIndex As Integer) As ValidationResult
        If selectedIndex < 0 Then
            Return ValidationResult.Fail("MgmtDeptCode", "「管理部署」を選択してください。")
        End If
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック5: 日付整合性（開始日 &lt; 終了日）
    ''' </summary>
    Public Shared Function ValidateDateRange(startDate As DateTime, endDate As DateTime) As ValidationResult
        If startDate >= endDate Then
            Return ValidationResult.Fail("StartDate", "「契約開始日」が「契約終了日」以降になっています。")
        End If
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック1〜5を一括実行する。最初に失敗した項目の結果を返す。
    ''' </summary>
    Public Shared Function ValidateBasicInputs(
            contractName As String,
            contractTypeIndex As Integer,
            supplierIndex As Integer,
            mgmtDeptCodeIndex As Integer,
            startDate As DateTime,
            endDate As DateTime) As ValidationResult

        Dim result As ValidationResult

        result = ValidateContractName(contractName)
        If Not result.IsValid Then Return result

        result = ValidateContractType(contractTypeIndex)
        If Not result.IsValid Then Return result

        result = ValidateSupplier(supplierIndex)
        If Not result.IsValid Then Return result

        result = ValidateMgmtDeptCode(mgmtDeptCodeIndex)
        If Not result.IsValid Then Return result

        result = ValidateDateRange(startDate, endDate)
        If Not result.IsValid Then Return result

        Return ValidationResult.Ok()
    End Function

    ' =============================================================
    '  チェック6〜10: 後半バリデーション
    ' =============================================================

    ''' <summary>上限金額定数 (9,999,999,999)</summary>
    Public Const MAX_AMOUNT As Decimal = 9999999999D

    ''' <summary>
    ''' チェック6: 資産グリッドに有効な行（BuknBango1 が非空）が1件以上あるか検証する。
    ''' </summary>
    ''' <param name="buknBangoValues">各行の BuknBango1 セル値のリスト（Nothing/空を含み得る）</param>
    ''' <returns>有効行が1件以上あれば Ok、なければ Fail</returns>
    Public Shared Function ValidateAssetGrid(buknBangoValues As IEnumerable(Of String)) As ValidationResult
        If buknBangoValues Is Nothing Then
            Return ValidationResult.Fail("Assets", "資産が登録されていません。少なくとも1件の資産を登録してください。")
        End If
        For Each cellValue In buknBangoValues
            If Not String.IsNullOrWhiteSpace(cellValue) Then Return ValidationResult.Ok()
        Next
        Return ValidationResult.Fail("Assets", "資産が登録されていません。少なくとも1件の資産を登録してください。")
    End Function

    ''' <summary>
    ''' チェック7: 月額支払明細の金額に負値が含まれるか検証する。
    ''' </summary>
    ''' <param name="amountTexts">各行の MAmountExTax セル値テキスト（カンマ付きあり得る）</param>
    ''' <returns>負値がなければ Ok、あれば Fail</returns>
    Public Shared Function ValidateNoNegativeAmount(amountTexts As IEnumerable(Of String)) As ValidationResult
        If amountTexts Is Nothing Then Return ValidationResult.Ok()
        For Each txt In amountTexts
            Dim amountVal As Decimal = 0
            If txt IsNot Nothing Then
                Decimal.TryParse(txt.Replace(",", ""), amountVal)
            End If
            If amountVal < 0 Then
                Return ValidationResult.Fail("MAmountExTax", "月額支払明細に負の金額が入力されています。")
            End If
        Next
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック8: 月額支払明細の金額に上限超過が含まれるか検証する。
    ''' </summary>
    ''' <param name="amountTexts">各行の MAmountExTax セル値テキスト</param>
    ''' <returns>上限超過がなければ Ok、あれば Fail</returns>
    Public Shared Function ValidateAmountUpperLimit(amountTexts As IEnumerable(Of String)) As ValidationResult
        If amountTexts Is Nothing Then Return ValidationResult.Ok()
        For Each txt In amountTexts
            Dim amountVal As Decimal = 0
            If txt IsNot Nothing Then
                Decimal.TryParse(txt.Replace(",", ""), amountVal)
            End If
            If amountVal > MAX_AMOUNT Then
                Return ValidationResult.Fail("MAmountExTax", "月額支払明細の金額が上限（" & MAX_AMOUNT.ToString("N0") & "円）を超えています。")
            End If
        Next
        Return ValidationResult.Ok()
    End Function

    ''' <summary>
    ''' チェック9: 月額支払に有効金額（正の値）が1件以上あるか検証する。
    ''' </summary>
    ''' <param name="amountTexts">各行の MAmountExTax セル値テキスト</param>
    ''' <returns>正の金額行が1件以上あれば Ok、なければ Fail</returns>
    Public Shared Function ValidateHasPositiveAmount(amountTexts As IEnumerable(Of String)) As ValidationResult
        If amountTexts Is Nothing Then
            Return ValidationResult.Fail("MAmountExTax", "月額支払明細に有効な金額が入力されていません。")
        End If
        For Each txt In amountTexts
            Dim amountVal As Decimal = 0
            If txt IsNot Nothing Then
                Decimal.TryParse(txt.Replace(",", ""), amountVal)
            End If
            If amountVal > 0 Then Return ValidationResult.Ok()
        Next
        Return ValidationResult.Fail("MAmountExTax", "月額支払明細に有効な金額が入力されていません。")
    End Function

    ''' <summary>
    ''' チェック10: 転貸日付の警告判定（転貸有の場合、開始日 >= 終了日なら警告対象）。
    ''' </summary>
    ''' <param name="isSubleaseChecked">転貸チェックボックスがオンか</param>
    ''' <param name="subleaseStart">転貸開始日</param>
    ''' <param name="subleaseEnd">転貸終了日</param>
    ''' <returns>警告が必要な場合 True</returns>
    Public Shared Function IsSubleaseDateWarning(isSubleaseChecked As Boolean,
                                                  subleaseStart As DateTime,
                                                  subleaseEnd As DateTime) As Boolean
        If Not isSubleaseChecked Then Return False
        Return subleaseStart >= subleaseEnd
    End Function

End Class
