Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

Namespace LeaseM4BS.Tests

    ''' <summary>
    ''' ValidateInput() 前半チェック（1〜5）の単体テスト
    ''' ContractValidationHelper に抽出した純粋ロジックをテストする。
    '''
    ''' チェック1: 契約名称（必須）
    ''' チェック2: 契約種類（必須選択）
    ''' チェック3: 取引先（必須選択）
    ''' チェック4: 管理部署（必須選択）
    ''' チェック5: 日付整合性（開始日 &lt; 終了日）
    ''' </summary>
    <TestClass>
    Public Class ValidateInputBasicTests

        ' =============================================================
        '  チェック1: 契約名称（必須）
        ' =============================================================

        <TestMethod>
        Public Sub Check1_契約名称_正常値_Trueを返す()
            Dim result = ContractValidationHelper.ValidateContractName("テスト契約")
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check1_契約名称_空文字_Falseを返す()
            Dim result = ContractValidationHelper.ValidateContractName("")
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractName", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check1_契約名称_Nothing_Falseを返す()
            Dim result = ContractValidationHelper.ValidateContractName(Nothing)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractName", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check1_契約名称_空白のみ_Falseを返す()
            Dim result = ContractValidationHelper.ValidateContractName("   ")
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractName", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check1_契約名称_タブ文字のみ_Falseを返す()
            Dim result = ContractValidationHelper.ValidateContractName(vbTab)
            Assert.IsFalse(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check1_契約名称_前後空白あり_Trueを返す()
            Dim result = ContractValidationHelper.ValidateContractName("  テスト  ")
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック2: 契約種類（必須選択）
        ' =============================================================

        <TestMethod>
        Public Sub Check2_契約種類_選択済み_Trueを返す()
            Dim result = ContractValidationHelper.ValidateContractType(0)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check2_契約種類_未選択_Falseを返す()
            Dim result = ContractValidationHelper.ValidateContractType(-1)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractType", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check2_契約種類_2番目選択_Trueを返す()
            Dim result = ContractValidationHelper.ValidateContractType(1)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check2_契約種類_大きいインデックス_Trueを返す()
            Dim result = ContractValidationHelper.ValidateContractType(99)
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック3: 取引先（必須選択）
        ' =============================================================

        <TestMethod>
        Public Sub Check3_取引先_選択済み_Trueを返す()
            Dim result = ContractValidationHelper.ValidateSupplier(0)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check3_取引先_未選択_Falseを返す()
            Dim result = ContractValidationHelper.ValidateSupplier(-1)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("Supplier", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check3_取引先_3番目選択_Trueを返す()
            Dim result = ContractValidationHelper.ValidateSupplier(2)
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック4: 管理部署（必須選択）
        ' =============================================================

        <TestMethod>
        Public Sub Check4_管理部署_選択済み_Trueを返す()
            Dim result = ContractValidationHelper.ValidateMgmtDeptCode(0)
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check4_管理部署_未選択_Falseを返す()
            Dim result = ContractValidationHelper.ValidateMgmtDeptCode(-1)
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("MgmtDeptCode", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check4_管理部署_5番目選択_Trueを返す()
            Dim result = ContractValidationHelper.ValidateMgmtDeptCode(4)
            Assert.IsTrue(result.IsValid)
        End Sub

        ' =============================================================
        '  チェック5: 日付整合性（開始日 < 終了日）
        ' =============================================================

        <TestMethod>
        Public Sub Check5_日付_開始日が終了日より前_Trueを返す()
            Dim result = ContractValidationHelper.ValidateDateRange(
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check5_日付_開始日と終了日が同じ_Falseを返す()
            Dim result = ContractValidationHelper.ValidateDateRange(
                New DateTime(2024, 4, 1), New DateTime(2024, 4, 1))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("StartDate", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check5_日付_開始日が終了日より後_Falseを返す()
            Dim result = ContractValidationHelper.ValidateDateRange(
                New DateTime(2025, 4, 1), New DateTime(2024, 4, 1))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("StartDate", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub Check5_日付_1日差_Trueを返す()
            Dim result = ContractValidationHelper.ValidateDateRange(
                New DateTime(2024, 4, 1), New DateTime(2024, 4, 2))
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check5_日付_年跨ぎ_Trueを返す()
            Dim result = ContractValidationHelper.ValidateDateRange(
                New DateTime(2024, 12, 31), New DateTime(2025, 1, 1))
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub Check5_日付_同日時刻違いでも日付部分が同一_Falseを返す()
            ' DateTime の Value 比較なので同一日付なら >= で False
            Dim d = New DateTime(2024, 6, 15)
            Dim result = ContractValidationHelper.ValidateDateRange(d, d)
            Assert.IsFalse(result.IsValid)
        End Sub

        ' =============================================================
        '  一括チェック（ValidateBasicInputs）
        ' =============================================================

        <TestMethod>
        Public Sub 一括_全項目正常_Trueを返す()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", 0, 0, 0,
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsTrue(result.IsValid)
        End Sub

        <TestMethod>
        Public Sub 一括_契約名称が空_最初のエラーで停止()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "", -1, -1, -1,
                New DateTime(2025, 4, 1), New DateTime(2024, 4, 1))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractName", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub 一括_契約種類のみ未選択_ContractTypeエラー()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", -1, 0, 0,
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractType", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub 一括_取引先のみ未選択_Supplierエラー()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", 0, -1, 0,
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("Supplier", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub 一括_管理部署のみ未選択_MgmtDeptCodeエラー()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", 0, 0, -1,
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("MgmtDeptCode", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub 一括_日付のみ不正_StartDateエラー()
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", 0, 0, 0,
                New DateTime(2025, 4, 1), New DateTime(2024, 4, 1))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("StartDate", result.ErrorField)
        End Sub

        <TestMethod>
        Public Sub 一括_複数エラーでも最初の項目で停止()
            ' 契約種類・取引先・管理部署すべて未選択でも、契約種類で停止
            Dim result = ContractValidationHelper.ValidateBasicInputs(
                "テスト契約", -1, -1, -1,
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 31))
            Assert.IsFalse(result.IsValid)
            Assert.AreEqual("ContractType", result.ErrorField)
        End Sub

    End Class

End Namespace
