Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

Namespace LeaseM4BS.Tests

    ''' <summary>
    ''' CalcLeaseMonths テスト: MECE 分類
    '''
    ''' 【軸1】日付関係: start &lt; end / start = end / start &gt; end
    ''' 【軸2】期間スパン: 同月 / 隣月 / 同年内 / 年跨ぎ / 複数年 / 長期(10年超)
    ''' 【軸3】無償期間: 0 / 正(期間未満) / 正(期間と同値) / 正(期間超過)
    ''' 【軸4】月末境界: 通常日 / 31日月末 / 30日月末 / 2月末(平年) / 2月末(うるう年)
    ''' 【軸5】テキストパース: 正常値 / エラー値 / 空文字 / 大きい数値
    ''' </summary>
    <TestClass>
    Public Class SyncTab1ToJudgeTests

        ' =============================================================
        '  A. 基本計算（正常系: start < end, freePeriod = 0）
        ' =============================================================

        <TestMethod>
        Public Sub A01_1ヶ月契約()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 5, 1), 0))
        End Sub

        <TestMethod>
        Public Sub A02_6ヶ月契約()
            Assert.AreEqual(6, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 10, 1), 0))
        End Sub

        <TestMethod>
        Public Sub A03_12ヶ月契約()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 4, 1), 0))
        End Sub

        <TestMethod>
        Public Sub A04_24ヶ月契約()
            Assert.AreEqual(24, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2026, 4, 1), 0))
        End Sub

        <TestMethod>
        Public Sub A05_隣月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 12, 15), New DateTime(2025, 1, 15), 0))
        End Sub

        ' =============================================================
        '  B. 長期契約
        ' =============================================================

        <TestMethod>
        Public Sub B01_10年契約_120ヶ月()
            Assert.AreEqual(120, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2020, 1, 1), New DateTime(2030, 1, 1), 0))
        End Sub

        <TestMethod>
        Public Sub B02_20年契約_240ヶ月()
            Assert.AreEqual(240, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2010, 1, 1), New DateTime(2030, 1, 1), 0))
        End Sub

        <TestMethod>
        Public Sub B03_50年契約_600ヶ月()
            Assert.AreEqual(600, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2000, 1, 1), New DateTime(2050, 1, 1), 0))
        End Sub

        ' =============================================================
        '  C. 年跨ぎ
        ' =============================================================

        <TestMethod>
        Public Sub C01_12月から3月_4ヶ月()
            Assert.AreEqual(4, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 11, 1), New DateTime(2025, 3, 1), 0))
        End Sub

        <TestMethod>
        Public Sub C02_年末年始跨ぎ_1ヶ月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 12, 1), New DateTime(2025, 1, 1), 0))
        End Sub

        <TestMethod>
        Public Sub C03_複数年跨ぎ_25ヶ月()
            Assert.AreEqual(25, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2023, 12, 1), New DateTime(2026, 1, 1), 0))
        End Sub

        ' =============================================================
        '  D. 同一日付・同月（start = end / 同月内）
        ' =============================================================

        <TestMethod>
        Public Sub D01_同一日付_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 4, 1), 0))
        End Sub

        <TestMethod>
        Public Sub D02_同月内の異なる日_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 4, 30), 0))
        End Sub

        ' =============================================================
        '  E. 日付逆転（start > end）
        ' =============================================================

        <TestMethod>
        Public Sub E01_1ヶ月逆転_0を返す()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 5, 1), New DateTime(2024, 4, 1), 0))
        End Sub

        <TestMethod>
        Public Sub E02_2年逆転_0を返す()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2026, 4, 1), New DateTime(2024, 4, 1), 0))
        End Sub

        ' =============================================================
        '  F. 無償期間バリエーション
        ' =============================================================

        <TestMethod>
        Public Sub F01_無償1ヶ月_12ヶ月から11ヶ月()
            Assert.AreEqual(11, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 4, 1), 1))
        End Sub

        <TestMethod>
        Public Sub F02_無償3ヶ月_24ヶ月から21ヶ月()
            Assert.AreEqual(21, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2026, 4, 1), 3))
        End Sub

        <TestMethod>
        Public Sub F03_無償期間と総月数が同値_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 4, 1), 12))
        End Sub

        <TestMethod>
        Public Sub F04_無償期間が総月数を超過_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 10, 1), 12))
        End Sub

        <TestMethod>
        Public Sub F05_無償期間が総月数の大幅超過_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2024, 5, 1), 100))
        End Sub

        <TestMethod>
        Public Sub F06_無償期間で結果が1ヶ月になるギリギリ()
            ' 12ヶ月 - 11 = 1
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 4, 1), 11))
        End Sub

        <TestMethod>
        Public Sub F07_逆転日付に無償期間_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 4, 1), New DateTime(2024, 4, 1), 3))
        End Sub

        ' =============================================================
        '  G. 月末境界（31日/30日/28日/29日）
        ' =============================================================

        <TestMethod>
        Public Sub G01_1月31日から2月28日_1ヶ月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 1, 31), New DateTime(2025, 2, 28), 0))
        End Sub

        <TestMethod>
        Public Sub G02_1月31日から3月31日_2ヶ月()
            Assert.AreEqual(2, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 1, 31), New DateTime(2025, 3, 31), 0))
        End Sub

        <TestMethod>
        Public Sub G03_3月31日から4月30日_1ヶ月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 3, 31), New DateTime(2025, 4, 30), 0))
        End Sub

        <TestMethod>
        Public Sub G04_月末同士で12ヶ月()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 1, 31), New DateTime(2025, 1, 31), 0))
        End Sub

        <TestMethod>
        Public Sub G05_30日月末から31日月末()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 4, 30), New DateTime(2025, 5, 31), 0))
        End Sub

        ' =============================================================
        '  H. うるう年
        ' =============================================================

        <TestMethod>
        Public Sub H01_うるう年2月29日開始_12ヶ月()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 2, 29), New DateTime(2025, 2, 28), 0))
        End Sub

        <TestMethod>
        Public Sub H02_うるう年2月29日終了_1ヶ月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 1, 29), New DateTime(2024, 2, 29), 0))
        End Sub

        <TestMethod>
        Public Sub H03_うるう年跨ぎ_24ヶ月()
            Assert.AreEqual(24, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 2, 29), New DateTime(2026, 2, 28), 0))
        End Sub

        <TestMethod>
        Public Sub H04_平年2月28日から翌年2月28日_12ヶ月()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 2, 28), New DateTime(2026, 2, 28), 0))
        End Sub

        <TestMethod>
        Public Sub H05_うるう年2月29日開始_無償期間あり()
            ' 12ヶ月 - 2 = 10
            Assert.AreEqual(10, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 2, 29), New DateTime(2025, 2, 28), 2))
        End Sub

        ' =============================================================
        '  I. 1日だけ月が変わる境界
        ' =============================================================

        <TestMethod>
        Public Sub I01_月末から翌月1日_1ヶ月差()
            ' 1/31 → 2/1: year*12+month の差は 1
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 1, 31), New DateTime(2025, 2, 1), 0))
        End Sub

        <TestMethod>
        Public Sub I02_月初から同月末_0ヶ月()
            Assert.AreEqual(0, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 3, 1), New DateTime(2025, 3, 31), 0))
        End Sub

        <TestMethod>
        Public Sub I03_12月31日から翌1月1日_1ヶ月()
            Assert.AreEqual(1, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 12, 31), New DateTime(2025, 1, 1), 0))
        End Sub

        ' =============================================================
        '  J. 特殊な日付
        ' =============================================================

        <TestMethod>
        Public Sub J01_元日から大晦日_11ヶ月()
            Assert.AreEqual(11, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 1, 1), New DateTime(2025, 12, 1), 0))
        End Sub

        <TestMethod>
        Public Sub J02_年初から年末まで_12ヶ月()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2025, 1, 1), New DateTime(2026, 1, 1), 0))
        End Sub

        <TestMethod>
        Public Sub J03_会計年度開始_4月1日から3月1日_11ヶ月()
            Assert.AreEqual(11, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 3, 1), 0))
        End Sub

        <TestMethod>
        Public Sub J04_会計年度丸1年_4月1日から翌4月1日_12ヶ月()
            Assert.AreEqual(12, ContractCalcHelper.CalcLeaseMonths(
                New DateTime(2024, 4, 1), New DateTime(2025, 4, 1), 0))
        End Sub

        ' =============================================================
        '  K. テキストパース（SyncTab1ToJudge 内の lblLeaseMonths → lblTermMonths 転記）
        ' =============================================================

        Private Shared Function ParseMonthsText(text As String) As Integer
            Dim monthsText As String = text.Replace("ヶ月", "").Trim()
            Dim months As Integer = 0
            Integer.TryParse(monthsText, months)
            Return months
        End Function

        <TestMethod>
        Public Sub K01_正常値_24ヶ月()
            Assert.AreEqual(24, ParseMonthsText("24ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K02_正常値_0ヶ月()
            Assert.AreEqual(0, ParseMonthsText("0ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K03_正常値_1ヶ月()
            Assert.AreEqual(1, ParseMonthsText("1ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K04_正常値_120ヶ月()
            Assert.AreEqual(120, ParseMonthsText("120ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K05_エラー値_ハイフン()
            Assert.AreEqual(0, ParseMonthsText("---ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K06_空文字_0を返す()
            Assert.AreEqual(0, ParseMonthsText(""))
        End Sub

        <TestMethod>
        Public Sub K07_ヶ月のみ_0を返す()
            Assert.AreEqual(0, ParseMonthsText("ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K08_大きい数値_600()
            Assert.AreEqual(600, ParseMonthsText("600ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K09_前後スペース_正しくパース()
            Assert.AreEqual(12, ParseMonthsText(" 12 ヶ月"))
        End Sub

        <TestMethod>
        Public Sub K10_負数テキスト_TryParseで負数を返す()
            ' 実運用では発生しないが、パースの挙動を確認
            Assert.AreEqual(-1, ParseMonthsText("-1ヶ月"))
        End Sub

    End Class

End Namespace
