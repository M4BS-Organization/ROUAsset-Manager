Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

''' <summary>
''' AccountingCalculator の会計計算ロジックに対するテストクラス
''' </summary>
<TestClass>
Public Class AccountingCalcTests

    Private Const TOLERANCE As Decimal = 0.01D

#Region "CalcAccountingPeriod テスト"

    ''' <summary>
    ''' 基本期間のみ（更新なし）
    ''' </summary>
    <TestMethod>
    Public Sub CalcAccountingPeriod_NoRenewal_ReturnsBaseMonths()
        Assert.AreEqual(60, AccountingCalculator.CalcAccountingPeriod(60, 0, 12))
    End Sub

    ''' <summary>
    ''' 更新1回あり
    ''' </summary>
    <TestMethod>
    Public Sub CalcAccountingPeriod_OneRenewal_ReturnsCorrect()
        Assert.AreEqual(72, AccountingCalculator.CalcAccountingPeriod(60, 1, 12))
    End Sub

    ''' <summary>
    ''' 更新複数回あり
    ''' </summary>
    <TestMethod>
    Public Sub CalcAccountingPeriod_MultipleRenewals_ReturnsCorrect()
        Assert.AreEqual(96, AccountingCalculator.CalcAccountingPeriod(60, 3, 12))
    End Sub

    ''' <summary>
    ''' 基本期間0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcAccountingPeriod_ZeroBase_ReturnsRenewalOnly()
        Assert.AreEqual(24, AccountingCalculator.CalcAccountingPeriod(0, 2, 12))
    End Sub

#End Region

#Region "CalcRentTotal テスト"

    ''' <summary>
    ''' 標準的な賃料総額計算
    ''' </summary>
    <TestMethod>
    Public Sub CalcRentTotal_Standard_ReturnsCorrect()
        Assert.AreEqual(6000000D, AccountingCalculator.CalcRentTotal(100000D, 60))
    End Sub

    ''' <summary>
    ''' 月額0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcRentTotal_ZeroRent_ReturnsZero()
        Assert.AreEqual(0D, AccountingCalculator.CalcRentTotal(0D, 60))
    End Sub

    ''' <summary>
    ''' 月数0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcRentTotal_ZeroMonths_ReturnsZero()
        Assert.AreEqual(0D, AccountingCalculator.CalcRentTotal(100000D, 0))
    End Sub

#End Region

#Region "CalcAssessmentTotal テスト"

    ''' <summary>
    ''' 標準的な算定総額計算
    ''' </summary>
    <TestMethod>
    Public Sub CalcAssessmentTotal_Standard_ReturnsCorrect()
        Assert.AreEqual(7200000D, AccountingCalculator.CalcAssessmentTotal(100000D, 72))
    End Sub

    ''' <summary>
    ''' 月額0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcAssessmentTotal_ZeroRent_ReturnsZero()
        Assert.AreEqual(0D, AccountingCalculator.CalcAssessmentTotal(0D, 72))
    End Sub

#End Region

#Region "CalcLeaseAllocation テスト"

    ''' <summary>
    ''' リース比率100%の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseAllocation_FullLease_ReturnsTotal()
        Dim result = AccountingCalculator.CalcLeaseAllocation(1000000D, 100D, 0D)
        Assert.AreEqual(1000000D, result)
    End Sub

    ''' <summary>
    ''' リース比率50%の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseAllocation_HalfLease_ReturnsHalf()
        Dim result = AccountingCalculator.CalcLeaseAllocation(1000000D, 50D, 50D)
        Assert.AreEqual(500000D, result)
    End Sub

    ''' <summary>
    ''' リース比率70:30の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseAllocation_SeventyThirty_ReturnsCorrect()
        Dim result = AccountingCalculator.CalcLeaseAllocation(1000000D, 70D, 30D)
        Assert.AreEqual(700000D, result)
    End Sub

    ''' <summary>
    ''' ゼロ割の場合は算定総額をそのまま返す
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseAllocation_ZeroDivision_ReturnsTotal()
        Dim result = AccountingCalculator.CalcLeaseAllocation(1000000D, 0D, 0D)
        Assert.AreEqual(1000000D, result)
    End Sub

#End Region

#Region "CalcPresentValue テスト"

    ''' <summary>
    ''' 標準的なPV計算（年利3%、60ヶ月）
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_Standard3Percent60Months_ReturnsCorrect()
        ' PV = 100000 × (1 - (1+0.0025)^-60) / 0.0025
        ' 期待値: 約 5,565,236
        Dim result = AccountingCalculator.CalcPresentValue(100000D, 60, 0.03D)
        Assert.IsTrue(Math.Abs(result - 5565236D) < 100D, $"PV計算結果が期待値と異なります: {result}")
    End Sub

    ''' <summary>
    ''' 金利0%の場合はpayment × n
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_ZeroRate_ReturnsSimpleProduct()
        Dim result = AccountingCalculator.CalcPresentValue(100000D, 60, 0D)
        Assert.AreEqual(6000000D, result)
    End Sub

    ''' <summary>
    ''' 月数1の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_OneMonth_ReturnsDiscounted()
        Dim result = AccountingCalculator.CalcPresentValue(100000D, 1, 0.12D)
        ' r=0.01, PV = 100000 × (1-(1.01)^-1)/0.01 = 100000 × 0.9901/0.01 ≒ 99009.90
        Assert.IsTrue(Math.Abs(result - 99009.90D) < TOLERANCE, $"PV計算結果が期待値と異なります: {result}")
    End Sub

    ''' <summary>
    ''' 高金利（年利12%、12ヶ月）
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_HighRate12Months_ReturnsCorrect()
        ' r=0.01, n=12
        ' PV = 100000 × (1-(1.01)^-12)/0.01 ≒ 1,125,508
        Dim result = AccountingCalculator.CalcPresentValue(100000D, 12, 0.12D)
        Assert.IsTrue(Math.Abs(result - 1125508D) < 100D, $"PV計算結果が期待値と異なります: {result}")
    End Sub

    ''' <summary>
    ''' 月額0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_ZeroPayment_ReturnsZero()
        Dim result = AccountingCalculator.CalcPresentValue(0D, 60, 0.03D)
        Assert.AreEqual(0D, result)
    End Sub

    ''' <summary>
    ''' 月数0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcPresentValue_ZeroMonths_ReturnsZero()
        Dim result = AccountingCalculator.CalcPresentValue(100000D, 0, 0.03D)
        Assert.AreEqual(0D, result)
    End Sub

#End Region

#Region "CalcRouAsset テスト"

    ''' <summary>
    ''' 標準的なROU資産計算
    ''' </summary>
    <TestMethod>
    Public Sub CalcRouAsset_Standard_ReturnsCorrect()
        Dim result = AccountingCalculator.CalcRouAsset(5000000D, 100000D, 200000D, 50000D)
        Assert.AreEqual(5250000D, result)
    End Sub

    ''' <summary>
    ''' インセンティブなしの場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcRouAsset_NoIncentive_ReturnsSum()
        Dim result = AccountingCalculator.CalcRouAsset(5000000D, 100000D, 200000D, 0D)
        Assert.AreEqual(5300000D, result)
    End Sub

    ''' <summary>
    ''' PVのみの場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcRouAsset_PvOnly_ReturnsPv()
        Dim result = AccountingCalculator.CalcRouAsset(5000000D, 0D, 0D, 0D)
        Assert.AreEqual(5000000D, result)
    End Sub

#End Region

#Region "CalcLeaseLiability テスト"

    ''' <summary>
    ''' 標準的なリース負債計算
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseLiability_Standard_ReturnsPv()
        Assert.AreEqual(5000000D, AccountingCalculator.CalcLeaseLiability(5000000D))
    End Sub

    ''' <summary>
    ''' PV=0の場合
    ''' </summary>
    <TestMethod>
    Public Sub CalcLeaseLiability_Zero_ReturnsZero()
        Assert.AreEqual(0D, AccountingCalculator.CalcLeaseLiability(0D))
    End Sub

#End Region

End Class
