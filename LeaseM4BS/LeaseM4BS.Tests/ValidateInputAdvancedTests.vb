Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

Namespace LeaseM4BS.Tests

    ''' <summary>
    ''' ValidateInput 後半チェック（チェック6〜10）単体テスト
    '''
    ''' ContractValidationHelper の Pure ロジックをテストする。
    '''
    ''' 【チェック6】 資産グリッド: 最低1件の有効行（BuknBango1 が非空）
    ''' 【チェック7】 月額支払明細: 負値チェック
    ''' 【チェック8】 月額支払明細: 上限チェック（9,999,999,999 超）
    ''' 【チェック9】 月額支払明細: 有効金額あり（正の金額行が1件以上）
    ''' 【チェック10】転貸日付: 警告判定（開始日 >= 終了日）
    ''' </summary>
    <TestClass>
    Public Class ValidateInputAdvancedTests

        ' =============================================================
        '  チェック6: 資産グリッド（最低1件の有効行）
        ' =============================================================

        <TestMethod>
        Public Sub Check6_01_有効行1件あり_OK()
            Dim values = New List(Of String) From {"A001"}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_02_有効行複数あり_OK()
            Dim values = New List(Of String) From {"A001", "A002", "A003"}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_03_空リスト_Fail()
            Dim values = New List(Of String)
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("Assets", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check6_04_Nothing_Fail()
            Dim result = ContractValidationHelper.ValidateAssetGrid(Nothing)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_05_全行空文字_Fail()
            Dim values = New List(Of String) From {"", "", ""}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_06_全行ホワイトスペース_Fail()
            Dim values = New List(Of String) From {"   ", vbTab, " "}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_07_空行の中に有効行1件_OK()
            Dim values = New List(Of String) From {"", Nothing, "A001", ""}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check6_08_Nothing混在_有効行なし_Fail()
            Dim values = New List(Of String) From {Nothing, "", Nothing}
            Dim result = ContractValidationHelper.ValidateAssetGrid(values)
            Assert.IsFalse(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック7: 月額支払明細の負値チェック
        ' =============================================================

        <TestMethod>
        Public Sub Check7_01_正常金額_OK()
            Dim amounts = New List(Of String) From {"10000", "20000"}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_02_負値あり_Fail()
            Dim amounts = New List(Of String) From {"10000", "-500"}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("MAmountExTax", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check7_03_先頭が負値_Fail()
            Dim amounts = New List(Of String) From {"-1", "10000"}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_04_カンマ付き負値_Fail()
            Dim amounts = New List(Of String) From {"-1,000"}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_05_ゼロ_OK()
            Dim amounts = New List(Of String) From {"0"}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_06_空リスト_OK()
            Dim amounts = New List(Of String)
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_07_Nothing_OK()
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(Nothing)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check7_08_NothingセルValue_OK()
            Dim amounts = New List(Of String) From {Nothing, Nothing}
            Dim result = ContractValidationHelper.ValidateNoNegativeAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック8: 月額支払明細の上限チェック
        ' =============================================================

        <TestMethod>
        Public Sub Check8_01_上限丁度_OK()
            Dim amounts = New List(Of String) From {"9999999999"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_02_上限超過_Fail()
            Dim amounts = New List(Of String) From {"10000000000"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("MAmountExTax", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check8_03_上限1超過_Fail()
            ' 9,999,999,999 + 1 = 10,000,000,000
            Dim amounts = New List(Of String) From {"10000000000"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_04_カンマ付き上限超過_Fail()
            Dim amounts = New List(Of String) From {"10,000,000,000"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_05_カンマ付き上限丁度_OK()
            Dim amounts = New List(Of String) From {"9,999,999,999"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_06_通常金額_OK()
            Dim amounts = New List(Of String) From {"100000", "200000"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_07_2件目が上限超過_Fail()
            Dim amounts = New List(Of String) From {"1000", "10000000000"}
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check8_08_Nothing_OK()
            Dim result = ContractValidationHelper.ValidateAmountUpperLimit(Nothing)
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック9: 有効金額あり（正の金額行が1件以上）
        ' =============================================================

        <TestMethod>
        Public Sub Check9_01_正の金額1件_OK()
            Dim amounts = New List(Of String) From {"10000"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_02_全行ゼロ_Fail()
            Dim amounts = New List(Of String) From {"0", "0", "0"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("MAmountExTax", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check9_03_空リスト_Fail()
            Dim amounts = New List(Of String)
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_04_Nothing_Fail()
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(Nothing)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_05_ゼロと正の混在_OK()
            Dim amounts = New List(Of String) From {"0", "0", "5000"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_06_負値のみ_Fail()
            Dim amounts = New List(Of String) From {"-100", "-200"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_07_パースできない文字列_Fail()
            Dim amounts = New List(Of String) From {"abc", "xyz"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_08_カンマ付き正の金額_OK()
            Dim amounts = New List(Of String) From {"1,000,000"}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check9_09_Nothingセルのみ_Fail()
            Dim amounts = New List(Of String) From {Nothing, Nothing}
            Dim result = ContractValidationHelper.ValidateHasPositiveAmount(amounts)
            Assert.IsFalse(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック10: 転貸日付（警告判定）
        ' =============================================================

        <TestMethod>
        Public Sub Check10_01_転貸なし_警告不要()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                False, New DateTime(2025, 4, 1), New DateTime(2025, 3, 1))
            Assert.IsFalse(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_02_転貸あり_正常日付_警告不要()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2025, 4, 1), New DateTime(2025, 10, 1))
            Assert.IsFalse(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_03_転貸あり_開始日と終了日が同一_警告()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2025, 4, 1), New DateTime(2025, 4, 1))
            Assert.IsTrue(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_04_転貸あり_開始日が終了日より後_警告()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2025, 10, 1), New DateTime(2025, 4, 1))
            Assert.IsTrue(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_05_転貸あり_1日差で正常_警告不要()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2025, 4, 1), New DateTime(2025, 4, 2))
            Assert.IsFalse(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_06_転貸なし_日付逆転でも_警告不要()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                False, New DateTime(2026, 1, 1), New DateTime(2025, 1, 1))
            Assert.IsFalse(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_07_転貸あり_年跨ぎ正常_警告不要()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2024, 12, 1), New DateTime(2025, 6, 1))
            Assert.IsFalse(warn)
        End Sub

        <TestMethod>
        Public Sub Check10_08_転貸あり_年跨ぎ逆転_警告()
            Dim warn = ContractValidationHelper.IsSubleaseDateWarning(
                True, New DateTime(2025, 6, 1), New DateTime(2024, 12, 1))
            Assert.IsTrue(warn)
        End Sub

    End Class

End Namespace
