Imports System

''' <summary>
''' ASBJ第34号（リース会計基準）に基づく会計計算ロジックを提供するクラス
''' </summary>
Public Class AccountingCalculator

    ''' <summary>
    ''' 会計期間を計算する（基本期間 + 更新回数 × 更新期間）
    ''' </summary>
    ''' <param name="baseMonths">基本契約月数</param>
    ''' <param name="renewalCount">更新回数</param>
    ''' <param name="renewalMonths">1回あたりの更新月数</param>
    ''' <returns>会計期間（月数）</returns>
    Public Shared Function CalcAccountingPeriod(baseMonths As Integer, renewalCount As Integer, renewalMonths As Integer) As Integer
        Return baseMonths + renewalCount * renewalMonths
    End Function

    ''' <summary>
    ''' 賃料総額を計算する（月額賃料 × 基本月数）
    ''' </summary>
    ''' <param name="monthlyRent">月額賃料</param>
    ''' <param name="baseMonths">基本契約月数</param>
    ''' <returns>賃料総額</returns>
    Public Shared Function CalcRentTotal(monthlyRent As Decimal, baseMonths As Integer) As Decimal
        Return monthlyRent * baseMonths
    End Function

    ''' <summary>
    ''' 算定総額を計算する（月額賃料 × 会計期間月数）
    ''' </summary>
    ''' <param name="monthlyRent">月額賃料</param>
    ''' <param name="accountingMonths">会計期間月数</param>
    ''' <returns>算定総額</returns>
    Public Shared Function CalcAssessmentTotal(monthlyRent As Decimal, accountingMonths As Integer) As Decimal
        Return monthlyRent * accountingMonths
    End Function

    ''' <summary>
    ''' リース配分額を計算する（算定総額 × リース比率 / (リース比率 + 非リース比率)）
    ''' ゼロ割の場合は算定総額をそのまま返す
    ''' </summary>
    ''' <param name="assessmentTotal">算定総額</param>
    ''' <param name="leaseRatio">リース比率</param>
    ''' <param name="nonLeaseRatio">非リース比率</param>
    ''' <returns>リース配分額</returns>
    Public Shared Function CalcLeaseAllocation(assessmentTotal As Decimal, leaseRatio As Decimal, nonLeaseRatio As Decimal) As Decimal
        Dim totalRatio As Decimal = leaseRatio + nonLeaseRatio
        If totalRatio = 0D Then
            Return assessmentTotal
        End If
        Return assessmentTotal * leaseRatio / totalRatio
    End Function

    ''' <summary>
    ''' 現在価値（PV）を年金現価公式で計算する
    ''' PV = payment × (1 - (1 + r)^-n) / r
    ''' 月利 r = 年利 / 12、r=0の場合は payment × n
    ''' </summary>
    ''' <param name="monthlyPayment">月額支払額</param>
    ''' <param name="months">支払月数</param>
    ''' <param name="annualRate">年利（例: 0.03 = 3%）</param>
    ''' <returns>現在価値</returns>
    Public Shared Function CalcPresentValue(monthlyPayment As Decimal, months As Integer, annualRate As Decimal) As Decimal
        If annualRate = 0D Then
            Return monthlyPayment * months
        End If

        Dim r As Double = CDbl(annualRate) / 12.0
        Dim n As Integer = months
        Dim factor As Double = (1.0 - Math.Pow(1.0 + r, -n)) / r
        Return CDec(CDbl(monthlyPayment) * factor)
    End Function

    ''' <summary>
    ''' 使用権資産（ROU資産）を計算する
    ''' ROU = PV + 初期直接費用 + 原状回復費用 - リースインセンティブ
    ''' </summary>
    ''' <param name="pv">現在価値</param>
    ''' <param name="initialDirectCost">初期直接費用</param>
    ''' <param name="restorationCost">原状回復費用</param>
    ''' <param name="leaseIncentive">リースインセンティブ</param>
    ''' <returns>使用権資産額</returns>
    Public Shared Function CalcRouAsset(pv As Decimal, initialDirectCost As Decimal, restorationCost As Decimal, leaseIncentive As Decimal) As Decimal
        Return pv + initialDirectCost + restorationCost - leaseIncentive
    End Function

    ''' <summary>
    ''' リース負債を計算する（現在価値と同額）
    ''' </summary>
    ''' <param name="pv">現在価値</param>
    ''' <returns>リース負債額</returns>
    Public Shared Function CalcLeaseLiability(pv As Decimal) As Decimal
        Return pv
    End Function

    ''' <summary>
    ''' リース判定を行う（4つの判定基準に基づく）
    ''' </summary>
    ''' <param name="q1Yes">基準1: 原資産の所有権が移転するか</param>
    ''' <param name="q2No">基準2: 割安購入選択権がないか</param>
    ''' <param name="q3Yes">基準3: リース期間が経済的耐用年数の大部分か</param>
    ''' <param name="q4Yes">基準4: 現在価値が公正価値のほぼ全額か</param>
    ''' <returns>リースに該当する場合True</returns>
    Public Shared Function JudgeLease(q1Yes As Boolean, q2No As Boolean, q3Yes As Boolean, q4Yes As Boolean) As Boolean
        Return q1Yes AndAlso q2No AndAlso q3Yes AndAlso q4Yes
    End Function

    ''' <summary>
    ''' 短期リース免除規定の判定（契約月数が12ヶ月以下）
    ''' </summary>
    ''' <param name="contractMonths">契約月数</param>
    ''' <returns>短期リース免除に該当する場合True</returns>
    Public Shared Function IsExemptShortTerm(contractMonths As Integer) As Boolean
        Return contractMonths <= 12
    End Function

    ''' <summary>
    ''' 少額リース免除規定の判定（公正価値が閾値以下）
    ''' </summary>
    ''' <param name="fairValue">公正価値</param>
    ''' <param name="threshold">閾値</param>
    ''' <returns>少額リース免除に該当する場合True</returns>
    Public Shared Function IsExemptSmallAmount(fairValue As Decimal, threshold As Decimal) As Boolean
        Return fairValue <= threshold
    End Function

End Class
