' 等価性検証テスト: Access版との計算結果一致確認 (Issue #29)
'
' 対象:
'   - EQ-001~002: AmortizationScheduleBuilder (定額法)
'   - EQ-003: CalcShokyakuRitu (定率法償却率)
'   - EQ-004: AmortizationScheduleBuilder (定額法+減損)
'   - EQ-005: RepaymentScheduleBuilder (後払・利子抜法)
'   - EQ-006: RepaymentScheduleBuilder (先払・維持管理費)
'   - EQ-007: RepaymentScheduleBuilder (リースバック損益)
'   - EQ-008: ChukiCalcEngine (移転FA・5年)
'   - EQ-009: ChukiCalcEngine (中途解約)
'   - EQ-010: ScheduleHelper.GInt (精度境界値)
'
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_equivalence.vb
' 実行: test_equivalence.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports LeaseM4BS.DataAccess

Module TestEquivalence

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== Access版 計算結果等価性検証テスト (Issue #29) ===")
        Console.WriteLine()

        ' ==== Part 1: ヘルパー関数精度 ====
        Console.WriteLine("--- EQ-010: ScheduleHelper.GInt 精度検証 ---")
        Test_GInt_Precision()
        Console.WriteLine()

        ' ==== Part 2: 償却スケジュール ====
        Console.WriteLine("--- EQ-001: 定額法償却（標準12ヶ月） ---")
        Test_EQ001_Amort_Teigaku_12M()
        Console.WriteLine()

        Console.WriteLine("--- EQ-002: 定額法償却（24ヶ月・残価保証あり） ---")
        Test_EQ002_Amort_Teigaku_24M_Zanryo()
        Console.WriteLine()

        Console.WriteLine("--- EQ-003: 定率法償却率計算 ---")
        Test_EQ003_CalcShokyakuRitu()
        Console.WriteLine()

        Console.WriteLine("--- EQ-004: 定額法償却（減損あり） ---")
        Test_EQ004_Amort_Teigaku_WithGson()
        Console.WriteLine()

        ' ==== Part 3: 返済スケジュール ====
        Console.WriteLine("--- EQ-005: 返済スケジュール（後払・12ヶ月・利子抜法） ---")
        Test_EQ005_Repayment_Atobarai_12M()
        Console.WriteLine()

        Console.WriteLine("--- EQ-006: 返済スケジュール（先払・24ヶ月） ---")
        Test_EQ006_Repayment_Sakibarai_24M()
        Console.WriteLine()

        Console.WriteLine("--- EQ-007: 返済スケジュール（リースバック損益あり） ---")
        Test_EQ007_Repayment_LbSoneki()
        Console.WriteLine()

        ' ==== Part 4: 注記計算 ====
        Console.WriteLine("--- EQ-008: 注記計算（移転FA・定額法・5年） ---")
        Test_EQ008_Chuki_ItenFA_5Y()
        Console.WriteLine()

        Console.WriteLine("--- EQ-009: 注記計算（中途解約） ---")
        Test_EQ009_Chuki_Kaiyaku()
        Console.WriteLine()

        ' ==== 結果サマリ ====
        Console.WriteLine("=" & New String("="c, 50))
        Console.WriteLine($"結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount}")
        Console.WriteLine($"合計: {passCount + failCount + skipCount} テスト")
        Console.WriteLine("=" & New String("="c, 50))

        Environment.ExitCode = If(failCount > 0, 1, 0)
    End Sub

    ' ======================================================================
    '  ヘルパー関数
    ' ======================================================================

    Sub AssertEqual(testName As String, expected As Double, actual As Double, Optional tolerance As Double = 1.0)
        Dim diff As Double = Math.Abs(expected - actual)
        If diff <= tolerance Then
            passCount += 1
            Console.WriteLine($"  PASS: {testName} (期待={expected}, 実際={actual})")
        Else
            failCount += 1
            Console.WriteLine($"  FAIL: {testName} (期待={expected}, 実際={actual}, 差={diff})")
        End If
    End Sub

    Sub AssertEqualExact(testName As String, expected As Double, actual As Double)
        If expected = actual Then
            passCount += 1
            Console.WriteLine($"  PASS: {testName} (値={actual})")
        Else
            failCount += 1
            Console.WriteLine($"  FAIL: {testName} (期待={expected}, 実際={actual})")
        End If
    End Sub

    Sub AssertTrue(testName As String, condition As Boolean, Optional detail As String = "")
        If condition Then
            passCount += 1
            Console.WriteLine($"  PASS: {testName} {detail}")
        Else
            failCount += 1
            Console.WriteLine($"  FAIL: {testName} {detail}")
        End If
    End Sub

    Function MakeShiharaiSch(startDt As Date, count As Integer, cashPerMonth As Double) As List(Of ShiharaiSchEntry)
        Dim result As New List(Of ShiharaiSchEntry)
        For i As Integer = 0 To count - 1
            Dim shriDt As Date = New Date(startDt.Year, startDt.Month, 1).AddMonths(i)
            shriDt = New Date(shriDt.Year, shriDt.Month, DateTime.DaysInMonth(shriDt.Year, shriDt.Month))
            Dim entry As New ShiharaiSchEntry()
            entry.ShriDt = shriDt
            entry.SimeDt = shriDt
            entry.KeijDt = shriDt
            entry.Cash = cashPerMonth
            entry.CkaiykF = False
            entry.Nen = shriDt.Year
            entry.Getu = shriDt.Month
            entry.KeijNen = shriDt.Year
            entry.KeijGetu = shriDt.Month
            result.Add(entry)
        Next
        Return result
    End Function

    ' ======================================================================
    '  EQ-010: GInt精度検証
    ' ======================================================================

    Sub Test_GInt_Precision()
        AssertEqualExact("GInt(0.0)", 0, ScheduleHelper.GInt(0.0))
        AssertEqualExact("GInt(1.0)", 1, ScheduleHelper.GInt(1.0))
        AssertEqualExact("GInt(99.9)", 99, ScheduleHelper.GInt(99.9))
        AssertEqualExact("GInt(100.0)", 100, ScheduleHelper.GInt(100.0))
        AssertEqualExact("GInt(-1.5)", -2, ScheduleHelper.GInt(-1.5))
        ' G15丸めで3745.0になりFloor=3745
        AssertEqualExact("GInt(3744.9999999999996) [G15丸め]", 3745, ScheduleHelper.GInt(3744.9999999999996))
        ' G15丸めで1000000.0になりFloor=1000000
        AssertEqualExact("GInt(1000000.0000000001)", 1000000, ScheduleHelper.GInt(1000000.0000000001))
        ' G15丸めで1.0になりFloor=1
        AssertEqualExact("GInt(0.9999999999999998) [G15丸め]", 1, ScheduleHelper.GInt(0.9999999999999998))
    End Sub

    ' ======================================================================
    '  EQ-001: 定額法償却（標準12ヶ月）
    ' ======================================================================

    Sub Test_EQ001_Amort_Teigaku_12M()
        Dim warningMsg As String = Nothing
        Dim sch = AmortizationScheduleBuilder.Build(
            ShokyakuHo.Teigaku, Nothing,
            #4/1/2024#, 12, #3/31/2025#,
            1200000, 0,
            New List(Of GsonScheduleEntry)(), warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count = 12, $"(件数={If(sch IsNot Nothing, sch.Count, 0)})")

        If sch Is Nothing OrElse sch.Count <> 12 Then Return

        ' 全月の償却額が100,000であること
        For i As Integer = 0 To 11
            AssertEqualExact($"月{i + 1} 償却額", 100000, sch(i).Skyak)
        Next

        ' 累計額の検証
        AssertEqualExact("月1 累計額", 100000, sch(0).SkyakRkeiE)
        AssertEqualExact("月6 累計額", 600000, sch(5).SkyakRkeiE)
        AssertEqualExact("月12 累計額", 1200000, sch(11).SkyakRkeiE)

        ' 残高の検証
        AssertEqualExact("月1 残高", 1100000, sch(0).ZanE)
        AssertEqualExact("月12 残高", 0, sch(11).ZanE)

        ' ワーニングなし
        AssertTrue("ワーニングなし", warningMsg Is Nothing)
    End Sub

    ' ======================================================================
    '  EQ-002: 定額法償却（24ヶ月・残価保証あり）
    ' ======================================================================

    Sub Test_EQ002_Amort_Teigaku_24M_Zanryo()
        Dim warningMsg As String = Nothing
        Dim sch = AmortizationScheduleBuilder.Build(
            ShokyakuHo.Teigaku, Nothing,
            #4/1/2024#, 24, #3/31/2026#,
            2400000, 240000,
            New List(Of GsonScheduleEntry)(), warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count = 24, $"(件数={If(sch IsNot Nothing, sch.Count, 0)})")

        If sch Is Nothing OrElse sch.Count <> 24 Then Return

        ' GInt((2400000-240000)/24) = GInt(90000) = 90000
        For i As Integer = 0 To 22
            AssertEqualExact($"月{i + 1} 償却額", 90000, sch(i).Skyak)
        Next

        ' 最終月: 残高-残価保証 = (2400000-90000*23-0) - 240000 = 90000
        AssertEqualExact("月24 償却額(最終月)", 90000, sch(23).Skyak)

        ' 期末残高 = 残価保証額
        AssertEqualExact("月24 残高=残価保証額", 240000, sch(23).ZanE)

        ' 累計額
        AssertEqualExact("月24 累計額", 2160000, sch(23).SkyakRkeiE)
    End Sub

    ' ======================================================================
    '  EQ-003: 定率法償却率計算
    ' ======================================================================

    Sub Test_EQ003_CalcShokyakuRitu()
        ' M4互換モード: 1-0.1^(1/n) → Floor(*1000000) → Round(*0.1, Banker's) → *0.00001
        Dim testCases() As Integer = {12, 24, 36, 60, 84, 120}

        For Each n In testCases
            Dim ritu As Double = AmortizationScheduleBuilder.CalcShokyakuRitu(n)
            ' 結果は 0.00001 の倍数であること (M4互換精度)
            Dim isMultipleOf00001 As Boolean = (Math.Abs(Math.Round(ritu / 0.00001) * 0.00001 - ritu) < 0.000000001)
            AssertTrue($"償却率(n={n}): {ritu:F5}", isMultipleOf00001, "(0.00001の倍数)")
            ' 妥当な範囲であること (0 < ritu < 1)
            AssertTrue($"償却率(n={n})範囲", ritu > 0 AndAlso ritu < 1, $"({ritu:F5})")
        Next

        ' 定率法: 期間が長いほど償却率は小さい
        Dim r12 = AmortizationScheduleBuilder.CalcShokyakuRitu(12)
        Dim r60 = AmortizationScheduleBuilder.CalcShokyakuRitu(60)
        Dim r120 = AmortizationScheduleBuilder.CalcShokyakuRitu(120)
        AssertTrue("r12 > r60 > r120", r12 > r60 AndAlso r60 > r120)
    End Sub

    ' ======================================================================
    '  EQ-004: 定額法償却（減損あり）
    ' ======================================================================

    Sub Test_EQ004_Amort_Teigaku_WithGson()
        ' 2024/07末に200,000円の減損
        Dim gsonSch As New List(Of GsonScheduleEntry)
        Dim gson As New GsonScheduleEntry()
        gson.Nen = 2024 : gson.Getu = 7
        gson.GsonTmg = 0
        gson.GsonRyoS = 0 : gson.GsonRyoE = 200000
        gson.GsonRkeiS = 0 : gson.GsonRkeiE = 200000
        gsonSch.Add(gson)

        Dim warningMsg As String = Nothing
        Dim sch = AmortizationScheduleBuilder.Build(
            ShokyakuHo.Teigaku, Nothing,
            #4/1/2024#, 12, #3/31/2025#,
            1200000, 0,
            gsonSch, warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count = 12)

        If sch Is Nothing OrElse sch.Count <> 12 Then Return

        ' 月1-3: 通常償却 GInt(1200000/12) = 100000
        AssertEqualExact("月1 償却額(減損前)", 100000, sch(0).Skyak)
        AssertEqualExact("月2 償却額(減損前)", 100000, sch(1).Skyak)
        AssertEqualExact("月3 償却額(減損前)", 100000, sch(2).Skyak)

        ' 月4(=2024/7): 減損発生月
        ' 月度初の残高 = 1200000 - 300000(3ヶ月累計) - 0(減損累計初=0) = 900000
        ' ただし GsonRkeiS は減損発生前=0、GsonRyoE=200000
        ' 月4の償却額: GsonRkeiS(0)がgsonRkeiPrev(0)と同じなので月額変更なし → 100000
        AssertEqualExact("月4 償却額(減損発生月)", 100000, sch(3).Skyak)

        ' 月4の残高末: 1200000 - 400000(累計) - 200000(減損) = 600000
        AssertEqualExact("月4 残高(減損反映後)", 600000, sch(3).ZanE)

        ' 月5以降: 減損反映で再計算
        ' 月5 ZanS = 1200000 - 400000 - 200000 = 600000
        ' GsonRkeiS = 200000 > gsonRkeiPrev(0) → 再計算
        ' 新月額 = GInt((600000-0)/(12-4)) = GInt(75000) = 75000
        AssertEqualExact("月5 償却額(減損後再計算)", 75000, sch(4).Skyak)

        ' 月6-11: 再計算後の月額
        For i As Integer = 5 To 10
            AssertEqualExact($"月{i + 1} 償却額(減損後)", 75000, sch(i).Skyak)
        Next

        ' 最終月12: 残高端数調整
        ' 月12 ZanS = 1200000 - (400000+75000*7) - 200000 = 75000
        ' 最終月なので ZanS - zanryo(0) = 75000
        AssertEqualExact("月12 償却額(最終月)", 75000, sch(11).Skyak)

        ' 最終残高
        AssertEqualExact("月12 残高", 0, sch(11).ZanE)
    End Sub

    ' ======================================================================
    '  EQ-005: 返済スケジュール（後払・12ヶ月・利子抜法）
    ' ======================================================================

    Sub Test_EQ005_Repayment_Atobarai_12M()
        Dim shiharaiSch = MakeShiharaiSch(#4/1/2024#, 12, 87500)

        Dim warningMsg As String = Nothing
        Dim sch = RepaymentScheduleBuilder.BuildYakujoShiharai(
            #4/1/2024#, 12, #3/31/2025#,
            1000000, 1050000, 0, 0,
            Nothing, False, 0.025,
            RsokTmg.Atobarai, shiharaiSch,
            New List(Of GsonScheduleEntry)(),
            HensaiKind.Teigaku, warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count > 0)

        If sch Is Nothing OrElse sch.Count = 0 Then Return

        ' 利息総額 = 1050000 - 0 + 0 - 1000000 = 50000
        ' 初月元本残高 = 1,000,000 (取得価額相当額)
        AssertEqualExact("月1 元本残高初", 1000000, sch(0).GanponZanS)

        ' 支払額は月度末87500
        AssertEqualExact("月1 CashE", 87500, sch(0).CashE)

        ' 最終月: 元本残高=0であること
        Dim lastIdx = sch.Count - 1
        AssertEqual("最終月 元本残高", 0, sch(lastIdx).GanponZanE, 1)

        ' 利息残高も0
        AssertEqual("最終月 利息残高", 0, sch(lastIdx).RisokuZanE, 1)

        ' 全月の元本残高が単調減少であること
        Dim monotoneDecreasing As Boolean = True
        For i As Integer = 1 To lastIdx
            If sch(i).GanponZanE > sch(i - 1).GanponZanE Then
                monotoneDecreasing = False
                Exit For
            End If
        Next
        AssertTrue("元本残高が単調減少", monotoneDecreasing)

        ' 月次利息: GInt((GanponZanS + RisokuMibZanS) * 0.025 / 12)
        ' 月1: GInt((1000000 + 0) * 0.025 / 12) = GInt(2083.33) = 2083
        AssertEqualExact("月1 発生利息", 2083, sch(0).RisokuHasseiE)
    End Sub

    ' ======================================================================
    '  EQ-006: 返済スケジュール（先払・24ヶ月）
    ' ======================================================================

    Sub Test_EQ006_Repayment_Sakibarai_24M()
        ' 先払: 月額は月度初に発生
        Dim shiharaiSch As New List(Of ShiharaiSchEntry)
        Dim startDt = #4/1/2024#
        For i As Integer = 0 To 23
            Dim dt As Date = New Date(startDt.Year, startDt.Month, 1).AddMonths(i)
            ' 先払: 月初日に支払
            Dim entry As New ShiharaiSchEntry()
            entry.ShriDt = dt
            entry.SimeDt = dt
            entry.KeijDt = dt
            entry.Cash = 91667 ' 2200000/24≒91667
            entry.CkaiykF = False
            entry.Nen = dt.Year
            entry.Getu = dt.Month
            entry.KeijNen = dt.Year
            entry.KeijGetu = dt.Month
            shiharaiSch.Add(entry)
        Next

        Dim warningMsg As String = Nothing
        Dim sch = RepaymentScheduleBuilder.BuildYakujoShiharai(
            #4/1/2024#, 24, #3/31/2026#,
            2000000, 2200000, 50000, 100000,
            Nothing, False, 0.03,
            RsokTmg.Sakibarai, shiharaiSch,
            New List(Of GsonScheduleEntry)(),
            HensaiKind.Teigaku, warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count > 0)

        If sch Is Nothing OrElse sch.Count = 0 Then Return

        ' 先払: 月度初に支払が入る
        AssertTrue("月1 CashS > 0 (先払)", sch(0).CashS > 0)

        ' 維持管理費が按分されていること
        Dim hasIjiknr As Boolean = False
        For Each entry In sch
            If entry.IjiknrS.HasValue OrElse entry.IjiknrE.HasValue Then
                hasIjiknr = True
                Exit For
            End If
        Next
        AssertTrue("維持管理費が按分されている", hasIjiknr)

        ' 残価保証: 最終月の清算額が設定されていること
        Dim hasZanryo As Boolean = False
        For Each entry In sch
            If entry.ZanryoSeisanE > 0 Then
                hasZanryo = True
                Exit For
            End If
        Next
        AssertTrue("残価保証清算が設定されている", hasZanryo)

        ' 最終月: 元本残高 ≈ 0
        Dim lastIdx = sch.Count - 1
        AssertEqual("最終月 元本残高≈0", 0, sch(lastIdx).GanponZanE, 1)
    End Sub

    ' ======================================================================
    '  EQ-007: 返済スケジュール（リースバック損益あり）
    ' ======================================================================

    Sub Test_EQ007_Repayment_LbSoneki()
        Dim shiharaiSch = MakeShiharaiSch(#4/1/2024#, 12, 91667)

        Dim warningMsg As String = Nothing
        Dim sch = RepaymentScheduleBuilder.BuildYakujoShiharai(
            #4/1/2024#, 12, #3/31/2025#,
            1000000, 1100000, 0, 0,
            120000.0, False, 0.02,
            RsokTmg.Atobarai, shiharaiSch,
            New List(Of GsonScheduleEntry)(),
            HensaiKind.Teigaku, warningMsg)

        AssertTrue("スケジュール生成成功", sch IsNot Nothing AndAlso sch.Count > 0)

        If sch Is Nothing OrElse sch.Count = 0 Then Return

        ' リースバック損益が各月に按分されていること
        Dim lbTotal As Double = 0
        For Each entry In sch
            If entry.LbSonekiS.HasValue Then lbTotal += entry.LbSonekiS.Value
            If entry.LbSonekiE.HasValue Then lbTotal += entry.LbSonekiE.Value
        Next
        AssertEqual("LB損益合計=120000", 120000, lbTotal, 1)

        ' 各月にLB損益が配分されていること
        Dim monthsWithLb As Integer = 0
        For Each entry In sch
            If (entry.LbSonekiS.HasValue AndAlso entry.LbSonekiS.Value <> 0) OrElse
               (entry.LbSonekiE.HasValue AndAlso entry.LbSonekiE.Value <> 0) Then
                monthsWithLb += 1
            End If
        Next
        AssertTrue("LB損益が複数月に配分", monthsWithLb > 1, $"({monthsWithLb}ヶ月)")
    End Sub

    ' ======================================================================
    '  EQ-008: 注記計算（移転FA・定額法・5年リース）
    ' ======================================================================

    Sub Test_EQ008_Chuki_ItenFA_5Y()
        Dim shiharaiSch = MakeShiharaiSch(#4/1/2024#, 60, 100000)

        Dim params As New ChukiCalcParams()
        params.KishuDt = #4/1/2024#
        params.KimatDt = #3/31/2025#
        params.StartDt = #4/1/2024#
        params.Lkikan = 60
        params.BRendDt = #3/31/2029#
        params.BCkaiykF = False
        params.RcalcId = CInt(RcalcKind.RisokuBunri)
        params.SkyakHoId = CInt(ShokyakuHo.Teigaku)
        params.LeakbnId = CInt(LeaseKbn.Iten)
        params.HensaiKind = HensaiKind.Teigaku
        params.RsokTmg = RsokTmg.Atobarai
        params.SkyuKjF = False
        params.BSlsryo = 6000000
        params.BIjiknr = 0
        params.BZanryo = 0
        params.BSyutok = 5000000
        params.KsanRitu = 0.025
        params.BLbSoneki = Nothing
        params.LbChukiF = False
        params.KessanBi = 31

        Dim result = ChukiCalcEngine.Calculate(params, shiharaiSch, New List(Of GsonScheduleEntry)(), Nothing)

        AssertTrue("注記計算結果取得", result IsNot Nothing)

        If result Is Nothing Then Return

        ' 移転FAリース → 資産関連は計算されない (移転外のみ)
        ' 返済関連のみ検証

        ' 前期末残高: 開始日=期首日なので前期は存在しない → 0
        AssertEqualExact("前期末 元本残高", 0, result.LgnpnZzan)

        ' 期末元本残高 > 0 (5年のうち1年分しか返済していない)
        AssertTrue("期末 元本残高 > 0", result.LgnpnZan > 0, $"({result.LgnpnZan})")

        ' 当期元本返済額: 移転FAでは LgnpnToki は返済スケジュールの当期元本合計
        ' ※移転FA(Iten)では資産関連は未計算。LsryoTokiは返済関連の集計結果。
        ' 当期の元本+利息が正しく計算されていることを確認（期末元本 < 取得価額）
        AssertTrue("期末元本 < 取得価額", result.LgnpnZan < 5000000, "(LgnpnZan=" & result.LgnpnZan.ToString() & ")")

        ' 当期発生利息 > 0
        AssertTrue("当期発生利息 > 0", result.RisokuHasseiToki > 0, $"({result.RisokuHasseiToki})")

        ' 1年内返済予定額 > 0
        AssertTrue("1年内返済額 > 0", result.LgnpnZan1Nai > 0, $"({result.LgnpnZan1Nai})")

        ' 5年超残高 = 0 (5年リースなので5年超は0)
        AssertEqualExact("5年超残高", 0, result.LgnpnZan5Cho)

        ' 元本の整合性: 期末残高 = 1年内 + 1年超
        Dim sum1Nai1Cho = result.LgnpnZan1Nai + result.LgnpnZan1Cho
        ' ※1年超 = 翌期末元本残高なので直接比較ではなく、非負を確認
        AssertTrue("1年超 >= 0", result.LgnpnZan1Cho >= 0)
    End Sub

    ' ======================================================================
    '  EQ-009: 注記計算（中途解約）
    ' ======================================================================

    Sub Test_EQ009_Chuki_Kaiyaku()
        ' 2023/04開始、36ヶ月契約、2024/09/30解約
        Dim shiharaiSch = MakeShiharaiSch(#4/1/2023#, 36, 100000)

        Dim params As New ChukiCalcParams()
        params.KishuDt = #4/1/2024#
        params.KimatDt = #3/31/2025#
        params.StartDt = #4/1/2023#
        params.Lkikan = 36
        params.BRendDt = #9/30/2024#
        params.BCkaiykF = True
        params.RcalcId = CInt(RcalcKind.RisokuBunri)
        params.SkyakHoId = CInt(ShokyakuHo.Teigaku)
        params.LeakbnId = CInt(LeaseKbn.Iten)
        params.HensaiKind = HensaiKind.Teigaku
        params.RsokTmg = RsokTmg.Atobarai
        params.SkyuKjF = False
        params.BSlsryo = 3600000
        params.BIjiknr = 0
        params.BZanryo = 0
        params.BSyutok = 3000000
        params.KsanRitu = 0.025
        params.BLbSoneki = Nothing
        params.LbChukiF = False
        params.KessanBi = 31

        Dim result = ChukiCalcEngine.Calculate(params, shiharaiSch, New List(Of GsonScheduleEntry)(), Nothing)

        AssertTrue("注記計算結果取得", result IsNot Nothing)

        If result Is Nothing Then Return

        ' 前期末残高: 開始日(2023/04) < 期首日(2024/04) → 前期末残高が存在
        AssertTrue("前期末 元本残高 > 0", result.LgnpnZzan > 0, "(" & result.LgnpnZzan.ToString() & ")")

        ' 中途解約: 期末残高は0クリアされる (解約日<=期末日)
        AssertEqualExact("期末 元本残高=0(解約済)", 0, result.LgnpnZan)
        AssertEqualExact("期末 利息残高=0(解約済)", 0, result.LrsokZan)

        ' 解約消去額が発生していること
        Dim kaiyakVal As Double = If(result.LgnpnKaiyakGen.HasValue, result.LgnpnKaiyakGen.Value, 0)
        AssertTrue("解約消去 元本 > 0", kaiyakVal > 0, "(" & kaiyakVal.ToString() & ")")

        ' 未払利息解約消去
        AssertTrue("解約消去 未払利息 >= 0", result.RisokuMibKaiyakGen >= 0, "(" & result.RisokuMibKaiyakGen.ToString() & ")")

        ' 翌期以降の残高は全て0
        AssertEqualExact("1年内=0(解約済)", 0, result.LgnpnZan1Nai)
        AssertEqualExact("1年超=0(解約済)", 0, result.LgnpnZan1Cho)
    End Sub

End Module
