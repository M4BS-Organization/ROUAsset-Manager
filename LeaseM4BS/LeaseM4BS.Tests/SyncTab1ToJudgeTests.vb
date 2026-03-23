Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

Namespace LeaseM4BS.Tests

    <TestClass>
    Public Class SyncTab1ToJudgeTests

        ' =====================================================
        '  CalcLeaseMonths: 基本計算
        ' =====================================================

        <TestMethod>
        Public Sub CalcLeaseMonths_2年契約_無償0_24ヶ月()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1),
                New DateTime(2026, 4, 1),
                freePeriod:=0)
            Assert.AreEqual(24, result)
        End Sub

        <TestMethod>
        Public Sub CalcLeaseMonths_2年契約_無償3ヶ月_21ヶ月()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1),
                New DateTime(2026, 4, 1),
                freePeriod:=3)
            Assert.AreEqual(21, result)
        End Sub

        <TestMethod>
        Public Sub CalcLeaseMonths_同月_0ヶ月()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1),
                New DateTime(2024, 4, 30),
                freePeriod:=0)
            Assert.AreEqual(0, result)
        End Sub

        ' =====================================================
        '  CalcLeaseMonths: 無償期間が期間を超える場合
        ' =====================================================

        <TestMethod>
        Public Sub CalcLeaseMonths_無償期間が総月数を超える_0を返す()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1),
                New DateTime(2024, 10, 1),
                freePeriod:=12)
            Assert.AreEqual(0, result)
        End Sub

        ' =====================================================
        '  CalcLeaseMonths: 年跨ぎ
        ' =====================================================

        <TestMethod>
        Public Sub CalcLeaseMonths_年跨ぎ_正しい月数()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 11, 1),
                New DateTime(2025, 3, 1),
                freePeriod:=0)
            Assert.AreEqual(4, result)
        End Sub

        ' =====================================================
        '  CalcLeaseMonths: 開始日 > 終了日（逆転）
        ' =====================================================

        <TestMethod>
        Public Sub CalcLeaseMonths_開始が終了より後_0を返す()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2026, 4, 1),
                New DateTime(2024, 4, 1),
                freePeriod:=0)
            Assert.AreEqual(0, result)
        End Sub

        ' =====================================================
        '  CalcLeaseMonths: 長期契約
        ' =====================================================

        <TestMethod>
        Public Sub CalcLeaseMonths_10年契約_120ヶ月()
            Dim result = ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2020, 1, 1),
                New DateTime(2030, 1, 1),
                freePeriod:=0)
            Assert.AreEqual(120, result)
        End Sub

        ' =====================================================
        '  SyncTab1ToJudge で使われる lblLeaseMonths パース検証
        '  "XXヶ月" → Integer 変換が正しく動くことを確認
        ' =====================================================

        <TestMethod>
        Public Sub ParseLeaseMonthsText_正常値()
            Dim text = "24ヶ月"
            Dim monthsText = text.Replace("ヶ月", "").Trim()
            Dim months As Integer = 0
            Integer.TryParse(monthsText, months)
            Assert.AreEqual(24, months)
        End Sub

        <TestMethod>
        Public Sub ParseLeaseMonthsText_エラー値()
            Dim text = "---ヶ月"
            Dim monthsText = text.Replace("ヶ月", "").Trim()
            Dim months As Integer = 0
            Dim ok = Integer.TryParse(monthsText, months)
            Assert.IsFalse(ok)
            Assert.AreEqual(0, months)
        End Sub

    End Class

End Namespace
