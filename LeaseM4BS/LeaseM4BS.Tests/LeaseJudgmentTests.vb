Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports LeaseM4BS.DataAccess

''' <summary>
''' AccountingCalculator のリース判定・免除規定に対するテストクラス
''' </summary>
<TestClass>
Public Class LeaseJudgmentTests

#Region "JudgeLease テスト"

    ''' <summary>
    ''' 全条件を満たす場合はTrue
    ''' </summary>
    <TestMethod>
    Public Sub JudgeLease_AllConditionsMet_ReturnsTrue()
        Assert.IsTrue(AccountingCalculator.JudgeLease(True, True, True, True))
    End Sub

    ''' <summary>
    ''' 基準1がFalseの場合はFalse
    ''' </summary>
    <TestMethod>
    Public Sub JudgeLease_Q1False_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.JudgeLease(False, True, True, True))
    End Sub

    ''' <summary>
    ''' 基準2がFalseの場合はFalse
    ''' </summary>
    <TestMethod>
    Public Sub JudgeLease_Q2False_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.JudgeLease(True, False, True, True))
    End Sub

    ''' <summary>
    ''' 基準3がFalseの場合はFalse
    ''' </summary>
    <TestMethod>
    Public Sub JudgeLease_Q3False_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.JudgeLease(True, True, False, True))
    End Sub

    ''' <summary>
    ''' 全条件がFalseの場合はFalse
    ''' </summary>
    <TestMethod>
    Public Sub JudgeLease_AllFalse_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.JudgeLease(False, False, False, False))
    End Sub

#End Region

#Region "IsExemptShortTerm テスト"

    ''' <summary>
    ''' 12ヶ月ちょうどは短期リース免除に該当
    ''' </summary>
    <TestMethod>
    Public Sub IsExemptShortTerm_Exactly12_ReturnsTrue()
        Assert.IsTrue(AccountingCalculator.IsExemptShortTerm(12))
    End Sub

    ''' <summary>
    ''' 13ヶ月は短期リース免除に該当しない
    ''' </summary>
    <TestMethod>
    Public Sub IsExemptShortTerm_13Months_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.IsExemptShortTerm(13))
    End Sub

#End Region

#Region "IsExemptSmallAmount テスト"

    ''' <summary>
    ''' 閾値以下は少額リース免除に該当
    ''' </summary>
    <TestMethod>
    Public Sub IsExemptSmallAmount_BelowThreshold_ReturnsTrue()
        Assert.IsTrue(AccountingCalculator.IsExemptSmallAmount(300000D, 500000D))
    End Sub

    ''' <summary>
    ''' 閾値超過は少額リース免除に該当しない
    ''' </summary>
    <TestMethod>
    Public Sub IsExemptSmallAmount_AboveThreshold_ReturnsFalse()
        Assert.IsFalse(AccountingCalculator.IsExemptSmallAmount(600000D, 500000D))
    End Sub

#End Region

End Class
