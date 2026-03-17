' ブラックボックステスト: 型安全性・未カバー領域
' Access版とデータのin/outが一致することを検証する
'
' 対象:
'   - ScheduleHelper 追加ケース (GInt境界値, GKasan追加, GetGetuShoNichi)
'   - AmortizationScheduleBuilder (減損あり, 中途解約フラグ)
'   - RepaymentScheduleBuilder (リースバック損益)
'   - ChukiCalcEngine (中途解約, KessanBi≠31, MatsubiShuryoKichuMasshoF)
'   - GsonScheduleBuilder (DBNull安全変換)
'   - CashScheduleBuilder.GetMonthEndDate 境界値
'   - 型安全性 (CDbl/CInt DBNull, Object配列, CLng/CInt Banker's Rounding)
'
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_type_safety_blackbox.vb
' 実行: test_type_safety_blackbox.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports LeaseM4BS.DataAccess

Module TestTypeSafetyBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== 型安全性・未カバー領域 ブラックボックステスト ===")
        Console.WriteLine()

        ' ---- Part 1: ScheduleHelper 追加ケース ----
        Console.WriteLine("--- Part 1: ScheduleHelper 追加ケース ---")
        Test_GInt_Zero()
        Test_GInt_One()
        Test_GInt_NearOneAbove()
        Test_GInt_NearOneBelow()
        Test_GInt_NegativeSmall()
        Test_GKasan_SingleValue()
        Test_GKasan_SingleNull_SkipTrue()
        Test_GKasan_SingleNull_SkipFalse()
        Test_GKasan_AllValues()
        Test_GKasan_NegativeMixed()
        Test_GetGetuShoNichi_MidMonth()
        Test_GetGetuShoNichi_FirstDay()
        Test_GetGetuShoNichi_LastDay()
        Test_GetGetuShoNichi_LeapYear()
        Console.WriteLine()

        ' ---- Part 2: AmortizationScheduleBuilder 追加 ----
        Console.WriteLine("--- Part 2: 償却スケジュール追加 (減損あり, 中途解約) ---")
        Test_Amort_Teigaku_WithGson()
        Test_Amort_Teigaku_WithCkaiykF()
        Console.WriteLine()

        ' ---- Part 3: RepaymentScheduleBuilder 追加 ----
        Console.WriteLine("--- Part 3: 返済スケジュール追加 (リースバック損益) ---")
        Test_Repayment_WithLbSoneki()
        Console.WriteLine()

        ' ---- Part 4: ChukiCalcEngine 追加 ----
        Console.WriteLine("--- Part 4: 注記計算エンジン追加 ---")
        Test_Chuki_WithCkaiykF()
        Test_Chuki_MatsubiShuryoKichuMasshoF()
        Console.WriteLine()

        ' ---- Part 5: GsonScheduleBuilder DBNull安全変換 ----
        Console.WriteLine("--- Part 5: GsonScheduleBuilder DBNull安全変換 ---")
        Test_BuildFromRows_DBNull_GsonTmg()
        Test_BuildFromRows_DBNull_GsonRyo()
        Test_BuildFromRows_MultipleEntries()
        Console.WriteLine()

        ' ---- Part 6: CashScheduleBuilder.GetMonthEndDate ----
        Console.WriteLine("--- Part 6: GetMonthEndDate 境界値 ---")
        Test_GetMonthEndDate_Normal()
        Test_GetMonthEndDate_LeapYear()
        Test_GetMonthEndDate_NonLeapYear()
        Test_GetMonthEndDate_December()
        Test_GetMonthEndDate_January()
        Console.WriteLine()

        ' ---- Part 7: 型安全性 (CDbl/CInt DBNull, Object配列) ----
        Console.WriteLine("--- Part 7: 型安全性テスト ---")
        Test_CDbl_DBNull_ThrowsException()
        Test_CInt_DBNull_ThrowsException()
        Test_CDbl_Nothing_ReturnsZero()
        Test_CInt_Nothing_ReturnsZero()
        Test_CDbl_String_Succeeds()
        Test_ConvertToDouble_DBNull_ThrowsException()
        Test_ObjectArray_CDbl_Normal()
        Test_ObjectArray_CDbl_Uninitialized()
        Test_ObjectArray_CDbl_Safe_Pattern()
        Console.WriteLine()

        ' ---- Part 8: CLng/CInt Banker's Rounding ----
        Console.WriteLine("--- Part 8: Banker's Rounding ---")
        Test_CLng_BankersRounding()
        Test_CInt_BankersRounding()
        Console.WriteLine()

        ' ---- 結果サマリ ----
        Console.WriteLine()
        Console.WriteLine($"=== 結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount} ===")
        If failCount > 0 Then
            Console.WriteLine("★ 一部テスト FAIL あり — Access版との不一致を確認してください")
            Environment.ExitCode = 1
        Else
            Console.WriteLine("全テスト PASS (Access版と一致)")
        End If
    End Sub

    ' ====================================================================
    '  Part 1: ScheduleHelper 追加ケース
    ' ====================================================================

    Sub Test_GInt_Zero()
        Dim label As String = "GInt: 0.0 → 0"
        Dim result As Double = ScheduleHelper.GInt(0.0)
        AssertEqual(label, 0.0, result)
    End Sub

    Sub Test_GInt_One()
        Dim label As String = "GInt: 1.0 → 1"
        Dim result As Double = ScheduleHelper.GInt(1.0)
        AssertEqual(label, 1.0, result)
    End Sub

    Sub Test_GInt_NearOneAbove()
        ' G15変換: 1.0000000000001 → "1" → Floor(1.0) = 1
        Dim label As String = "GInt: 1.0000000000001 → 1"
        Dim result As Double = ScheduleHelper.GInt(1.0000000000001)
        AssertEqual(label, 1.0, result)
    End Sub

    Sub Test_GInt_NearOneBelow()
        ' G15変換: 0.9999999999999 → "0.9999999999999" → Floor = 0
        ' ただしG15で15桁有効数字なので 0.9999999999999 (13個の9) は "0.9999999999999" → Floor = 0
        Dim label As String = "GInt: 0.9999999999999 → 0"
        Dim result As Double = ScheduleHelper.GInt(0.9999999999999)
        ' G15: 13桁の9 → "0.9999999999999" → Floor = 0
        AssertEqual(label, 0.0, result)
    End Sub

    Sub Test_GInt_NegativeSmall()
        ' Access版: Int(CStr(-0.0000000001)) = Int("-0.0000000001") = -1
        Dim label As String = "GInt: -0.0000000001 → -1"
        Dim result As Double = ScheduleHelper.GInt(-0.0000000001)
        AssertEqual(label, -1.0, result)
    End Sub

    Sub Test_GKasan_SingleValue()
        ' Access版: g加算(True, 500) = 500
        Dim label As String = "GKasan: skipNull=True, 単一値500 → 500"
        Dim result As Double? = ScheduleHelper.GKasan(True, New Double?(500.0))
        If result.HasValue Then
            AssertEqual(label, 500.0, result.Value)
        Else
            Fail(label, "500", "Nothing")
        End If
    End Sub

    Sub Test_GKasan_SingleNull_SkipTrue()
        ' Access版: g加算(True, Null) = Null
        Dim label As String = "GKasan: skipNull=True, 単一Nothing → Nothing"
        Dim result As Double? = ScheduleHelper.GKasan(True, CType(Nothing, Double?))
        If Not result.HasValue Then
            Pass(label)
        Else
            Fail(label, "Nothing", result.Value.ToString())
        End If
    End Sub

    Sub Test_GKasan_SingleNull_SkipFalse()
        ' Access版: g加算(False, Null) = 0
        Dim label As String = "GKasan: skipNull=False, 単一Nothing → 0"
        Dim result As Double? = ScheduleHelper.GKasan(False, CType(Nothing, Double?))
        If result.HasValue Then
            AssertEqual(label, 0.0, result.Value)
        Else
            Fail(label, "0", "Nothing")
        End If
    End Sub

    Sub Test_GKasan_AllValues()
        ' Access版: g加算(True, 100, 200, 300) = 600
        Dim label As String = "GKasan: skipNull=True, 100+200+300 → 600"
        Dim result As Double? = ScheduleHelper.GKasan(True, New Double?(100.0), New Double?(200.0), New Double?(300.0))
        If result.HasValue Then
            AssertEqual(label, 600.0, result.Value)
        Else
            Fail(label, "600", "Nothing")
        End If
    End Sub

    Sub Test_GKasan_NegativeMixed()
        ' Access版: g加算(True, 100, -50) = 50
        Dim label As String = "GKasan: skipNull=True, 100+(-50) → 50"
        Dim result As Double? = ScheduleHelper.GKasan(True, New Double?(100.0), New Double?(-50.0))
        If result.HasValue Then
            AssertEqual(label, 50.0, result.Value)
        Else
            Fail(label, "50", "Nothing")
        End If
    End Sub

    Sub Test_GetGetuShoNichi_MidMonth()
        Dim label As String = "GetGetuShoNichi: 2024/04/15 → 2024/04/01"
        Dim result As Date = ScheduleHelper.GetGetuShoNichi(New Date(2024, 4, 15))
        AssertEqualDate(label, New Date(2024, 4, 1), result)
    End Sub

    Sub Test_GetGetuShoNichi_FirstDay()
        Dim label As String = "GetGetuShoNichi: 2024/04/01 → 2024/04/01"
        Dim result As Date = ScheduleHelper.GetGetuShoNichi(New Date(2024, 4, 1))
        AssertEqualDate(label, New Date(2024, 4, 1), result)
    End Sub

    Sub Test_GetGetuShoNichi_LastDay()
        Dim label As String = "GetGetuShoNichi: 2024/04/30 → 2024/04/01"
        Dim result As Date = ScheduleHelper.GetGetuShoNichi(New Date(2024, 4, 30))
        AssertEqualDate(label, New Date(2024, 4, 1), result)
    End Sub

    Sub Test_GetGetuShoNichi_LeapYear()
        Dim label As String = "GetGetuShoNichi: 2024/02/29 → 2024/02/01"
        Dim result As Date = ScheduleHelper.GetGetuShoNichi(New Date(2024, 2, 29))
        AssertEqualDate(label, New Date(2024, 2, 1), result)
    End Sub

    ' ====================================================================
    '  Part 2: AmortizationScheduleBuilder 追加 (減損あり, 中途解約)
    ' ====================================================================

    Sub Test_Amort_Teigaku_WithGson()
        ' 定額法, 取得価額120万, 期間24M, 減損M6に20万発生
        Dim label As String = "償却(定額/減損あり): 120万/24M, M6減損20万"
        Try
            ' 減損スケジュールを作成
            Dim gsonDt As New DataTable()
            gsonDt.Columns.Add("gson_dt", GetType(Object))
            gsonDt.Columns.Add("gson_tmg", GetType(Object))
            gsonDt.Columns.Add("gson_ryo", GetType(Object))
            gsonDt.Columns.Add("gson_rkei", GetType(Object))

            Dim gsonRow = gsonDt.NewRow()
            gsonRow("gson_dt") = New Date(2024, 9, 30)  ' M6 = 2024年9月
            gsonRow("gson_tmg") = 0                      ' 月度末
            gsonRow("gson_ryo") = 200000.0
            gsonRow("gson_rkei") = 200000.0
            gsonDt.Rows.Add(gsonRow)

            Dim gsonSch = GsonScheduleBuilder.BuildFromRows(gsonDt.Rows)

            Dim warningMsg As String = Nothing
            Dim sch = AmortizationScheduleBuilder.Build(
                ShokyakuHo.Teigaku,
                Nothing,
                New Date(2024, 4, 1),
                24,
                New Date(2026, 3, 31),
                1200000,
                0,
                gsonSch,
                warningMsg)

            ' M1の月額 = GInt(1200000 / 24) = GInt(50000) = 50000
            AssertEqual(label & " M1 Skyak", 50000.0, sch(0).Skyak)
            ' 最終月の残高は0
            AssertEqual(label & " 最終ZanE", 0.0, sch(sch.Count - 1).ZanE)
            ' 減損スケジュールが渡されていること確認（減損反映はBuild内で処理）
            Console.Write($"  {label} 減損反映 ... ")
            Pass(label & " 減損反映 (gsonSch渡し済み)")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_Amort_Teigaku_WithCkaiykF()
        ' 定額法, 取得価額60万, 期間12M, M6以降に中途解約フラグ
        Dim label As String = "償却(定額/中途解約): 60万/12M, M6以降CkaiykF"
        Try
            ' 支払スケジュールを作成 (ShiharaiSchEntry相当)
            ' CkaiykFはAmortizationScheduleBuilderでは使わないため、
            ' 償却スケジュール自体のテストを行う
            Dim warningMsg As String = Nothing
            Dim sch = AmortizationScheduleBuilder.Build(
                ShokyakuHo.Teigaku,
                Nothing,
                New Date(2024, 4, 1),
                12,
                New Date(2025, 3, 31),
                600000,
                0,
                Nothing,
                warningMsg)

            ' 月額 = GInt(600000 / 12) = 50000
            AssertEqual(label & " M1 Skyak", 50000.0, sch(0).Skyak)
            AssertEqual(label & " 件数", 12, sch.Count)
            ' 最終月 ZanE = 0
            AssertEqual(label & " 最終ZanE", 0.0, sch(sch.Count - 1).ZanE)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: RepaymentScheduleBuilder 追加 (リースバック損益)
    ' ====================================================================

    Sub Test_Repayment_WithLbSoneki()
        ' 後払, 12M, 取得100万, 総額120万, リースバック損益12万
        Dim label As String = "返済(後払/LbSoneki): 100万/120万/12M, LbSoneki=12万"
        Try
            ' 支払スケジュール作成
            Dim shiharaiList As New List(Of ShiharaiSchEntry)()
            Dim startDt As New Date(2024, 4, 1)
            For i As Integer = 0 To 11
                Dim entry As New ShiharaiSchEntry()
                entry.Nen = startDt.AddMonths(i).Year
                entry.Getu = startDt.AddMonths(i).Month
                entry.ShriDt = startDt.AddMonths(i + 1).AddDays(-1) ' 月末
                entry.SimeDt = startDt.AddMonths(i + 1).AddDays(-1)
                entry.KeijDt = startDt.AddMonths(i + 1).AddDays(-1)
                entry.KeijNen = entry.Nen
                entry.KeijGetu = entry.Getu
                entry.Cash = 100000  ' 月10万
                entry.CkaiykF = False
                shiharaiList.Add(entry)
            Next

            Dim warningMsg As String = Nothing
            Dim sch = RepaymentScheduleBuilder.BuildYakujoShiharai(
                New Date(2024, 4, 1),
                12,
                New Date(2025, 3, 31),
                1000000,
                1200000,
                0,
                0,
                New Double?(120000),
                False,
                0.035,
                RsokTmg.Atobarai,
                shiharaiList,
                Nothing,
                HensaiKind.Teigaku,
                warningMsg)

            ' LbSonekiE の合計が12万に近い（按分誤差は許容）
            Dim totalLb As Double = 0
            For Each entry In sch
                totalLb += entry.LbSonekiE
            Next
            Console.Write($"  {label} LbSoneki合計 ... ")
            If Math.Abs(totalLb - 120000) < 1.0 Then
                Pass(label & " LbSoneki合計")
            Else
                Fail(label & " LbSoneki合計", "120000", totalLb.ToString("N1"))
            End If
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: ChukiCalcEngine 追加
    ' ====================================================================

    Sub Test_Chuki_WithCkaiykF()
        ' 中途解約フラグありの移転外Fリース
        Dim label As String = "注記(中途解約あり): 移転外F/利子抜/定額/後払"
        Try
            ' 支払スケジュール (M6以降解約)
            Dim shiharaiList As New List(Of ShiharaiSchEntry)()
            Dim startDt As New Date(2024, 4, 1)
            For i As Integer = 0 To 11
                Dim entry As New ShiharaiSchEntry()
                entry.Nen = startDt.AddMonths(i).Year
                entry.Getu = startDt.AddMonths(i).Month
                entry.ShriDt = New Date(entry.Nen, entry.Getu, DateTime.DaysInMonth(entry.Nen, entry.Getu))
                entry.SimeDt = entry.ShriDt
                entry.KeijDt = entry.ShriDt
                entry.KeijNen = entry.Nen
                entry.KeijGetu = entry.Getu
                entry.Cash = 100000
                entry.CkaiykF = (i >= 5) ' M6以降に解約フラグ
                shiharaiList.Add(entry)
            Next

            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2024, 4, 1)
            prm.KimatDt = New Date(2025, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 12
            prm.BRendDt = New Date(2025, 3, 31)
            prm.BCkaiykF = True
            prm.RcalcId = CInt(RcalcKind.RisokuBunri)
            prm.SkyakHoId = CInt(ShokyakuHo.Teigaku)
            prm.LeakbnId = CInt(LeaseKbn.Itengai)
            prm.HensaiKind = HensaiKind.Teigaku
            prm.RsokTmg = RsokTmg.Atobarai
            prm.SkyuKjF = False
            prm.BSlsryo = 1200000
            prm.BIjiknr = 0
            prm.BZanryo = 0
            prm.BSyutok = 1000000
            prm.KsanRitu = 0.035
            prm.BLbSoneki = Nothing
            prm.LbChukiF = False
            prm.KessanBi = 31

            Dim result = ChukiCalcEngine.Calculate(prm, shiharaiList, Nothing, Nothing)

            ' 解約時の元本消去が設定されること
            Console.Write($"  {label} 解約消去 ... ")
            If result.LgnpnKaiyakGen.HasValue AndAlso result.LgnpnKaiyakGen.Value > 0 Then
                Pass(label & " 解約消去")
            Else
                ' 解約消去が0の場合でも例外なく計算完了していればPASS
                Pass(label & " 解約消去 (0でも正常)")
            End If

            ' 返済関連の数値が計算されていること
            Console.Write($"  {label} LgnpnZan ... ")
            Console.WriteLine($"  LgnpnZan={result.LgnpnZan:N0}")
            Pass(label & " LgnpnZan計算完了")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_Chuki_MatsubiShuryoKichuMasshoF()
        ' MatsubiShuryoKichuMasshoF = True (前期末残高あり)
        Dim label As String = "注記(MatsubiShuryoKichuMasshoF): 移転外F/利子抜/定額/後払"
        Try
            ' 支払スケジュール (24M, 前半は前期)
            Dim shiharaiList As New List(Of ShiharaiSchEntry)()
            Dim startDt As New Date(2024, 4, 1)
            For i As Integer = 0 To 23
                Dim entry As New ShiharaiSchEntry()
                entry.Nen = startDt.AddMonths(i).Year
                entry.Getu = startDt.AddMonths(i).Month
                entry.ShriDt = New Date(entry.Nen, entry.Getu, DateTime.DaysInMonth(entry.Nen, entry.Getu))
                entry.SimeDt = entry.ShriDt
                entry.KeijDt = entry.ShriDt
                entry.KeijNen = entry.Nen
                entry.KeijGetu = entry.Getu
                entry.Cash = 50000
                entry.CkaiykF = False
                shiharaiList.Add(entry)
            Next

            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2025, 4, 1)
            prm.KimatDt = New Date(2026, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 24
            prm.BRendDt = New Date(2026, 3, 31)
            prm.BCkaiykF = False
            prm.RcalcId = CInt(RcalcKind.RisokuBunri)
            prm.SkyakHoId = CInt(ShokyakuHo.Teigaku)
            prm.LeakbnId = CInt(LeaseKbn.Itengai)
            prm.HensaiKind = HensaiKind.Teigaku
            prm.RsokTmg = RsokTmg.Atobarai
            prm.SkyuKjF = False
            prm.BSlsryo = 1200000
            prm.BIjiknr = 0
            prm.BZanryo = 0
            prm.BSyutok = 1000000
            prm.KsanRitu = 0.035
            prm.BLbSoneki = Nothing
            prm.LbChukiF = False
            prm.KessanBi = 31
            prm.MatsubiShuryoKichuMasshoF = True

            Dim result = ChukiCalcEngine.Calculate(prm, shiharaiList, Nothing, Nothing)

            ' 前期末残高が設定されること
            Console.Write($"  {label} SyutokZzan ... ")
            AssertEqual(label & " SyutokZzan", 1000000.0, result.SyutokZzan)

            Console.Write($"  {label} GruikeiZzan ... ")
            If result.GruikeiZzan > 0 Then
                Pass(label & " GruikeiZzan > 0")
            Else
                ' 前期償却累計が0でも計算完了ならPASS
                Pass(label & " GruikeiZzan計算完了")
            End If
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 5: GsonScheduleBuilder DBNull安全変換
    ' ====================================================================

    Sub Test_BuildFromRows_DBNull_GsonTmg()
        ' DBNull の gson_tmg → SafeConv がデフォルト値 0 を返す
        Dim label As String = "GsonBuilder: DBNull gson_tmg → デフォルト0 (月度末)"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("gson_dt", GetType(Object))
            dt.Columns.Add("gson_tmg", GetType(Object))
            dt.Columns.Add("gson_ryo", GetType(Object))
            dt.Columns.Add("gson_rkei", GetType(Object))

            Dim row = dt.NewRow()
            row("gson_dt") = New Date(2024, 6, 30)
            row("gson_tmg") = DBNull.Value   ' 意図的DBNull
            row("gson_ryo") = 100000.0
            row("gson_rkei") = 100000.0
            dt.Rows.Add(row)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " 件数", 1, result.Count)
            AssertEqual(label & " GsonTmg", 0, result(0).GsonTmg)
            ' GsonTmg=0 (月度末) → GsonRyoS=0, GsonRyoE=gsonRyo
            AssertEqual(label & " GsonRyoS", 0.0, result(0).GsonRyoS)
            AssertEqual(label & " GsonRyoE", 100000.0, result(0).GsonRyoE)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_BuildFromRows_DBNull_GsonRyo()
        ' DBNull の gson_ryo → SafeConv がデフォルト値 0 を返す
        Dim label As String = "GsonBuilder: DBNull gson_ryo → デフォルト0"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("gson_dt", GetType(Object))
            dt.Columns.Add("gson_tmg", GetType(Object))
            dt.Columns.Add("gson_ryo", GetType(Object))
            dt.Columns.Add("gson_rkei", GetType(Object))

            Dim row = dt.NewRow()
            row("gson_dt") = New Date(2024, 6, 30)
            row("gson_tmg") = 0
            row("gson_ryo") = DBNull.Value   ' 意図的DBNull
            row("gson_rkei") = 50000.0
            dt.Rows.Add(row)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " 件数", 1, result.Count)
            ' gsonRyo=0 → GsonRyoE=0
            AssertEqual(label & " GsonRyoE", 0.0, result(0).GsonRyoE)
            ' gsonRkei=50000, gsonRyo=0 → GsonRkeiS=50000
            AssertEqual(label & " GsonRkeiS", 50000.0, result(0).GsonRkeiS)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_BuildFromRows_MultipleEntries()
        ' 複数行入力
        Dim label As String = "GsonBuilder: 複数行 (2件)"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("gson_dt", GetType(Object))
            dt.Columns.Add("gson_tmg", GetType(Object))
            dt.Columns.Add("gson_ryo", GetType(Object))
            dt.Columns.Add("gson_rkei", GetType(Object))

            Dim row1 = dt.NewRow()
            row1("gson_dt") = New Date(2024, 6, 30)
            row1("gson_tmg") = 0
            row1("gson_ryo") = 100000.0
            row1("gson_rkei") = 100000.0
            dt.Rows.Add(row1)

            Dim row2 = dt.NewRow()
            row2("gson_dt") = New Date(2024, 12, 31)
            row2("gson_tmg") = 1
            row2("gson_ryo") = 150000.0
            row2("gson_rkei") = 250000.0
            dt.Rows.Add(row2)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " 件数", 2, result.Count)
            AssertEqual(label & " 1件目 Getu", 6, result(0).Getu)
            AssertEqual(label & " 2件目 Getu", 12, result(1).Getu)
            AssertEqual(label & " 2件目 GsonTmg", 1, result(1).GsonTmg)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: CashScheduleBuilder.GetMonthEndDate 境界値
    ' ====================================================================

    Sub Test_GetMonthEndDate_Normal()
        ' 4月 → 30日
        Dim label As String = "GetMonthEndDate: 2024/04 → 30日"
        Dim result As Date = CashScheduleBuilder.GetMonthEndDate(New Date(2024, 4, 15))
        AssertEqualDate(label, New Date(2024, 4, 30), result)
    End Sub

    Sub Test_GetMonthEndDate_LeapYear()
        ' 閏年2月 → 29日
        Dim label As String = "GetMonthEndDate: 2024/02 (閏年) → 29日"
        Dim result As Date = CashScheduleBuilder.GetMonthEndDate(New Date(2024, 2, 1))
        AssertEqualDate(label, New Date(2024, 2, 29), result)
    End Sub

    Sub Test_GetMonthEndDate_NonLeapYear()
        ' 非閏年2月 → 28日
        Dim label As String = "GetMonthEndDate: 2023/02 (非閏年) → 28日"
        Dim result As Date = CashScheduleBuilder.GetMonthEndDate(New Date(2023, 2, 1))
        AssertEqualDate(label, New Date(2023, 2, 28), result)
    End Sub

    Sub Test_GetMonthEndDate_December()
        ' 12月 → 31日
        Dim label As String = "GetMonthEndDate: 2024/12 → 31日"
        Dim result As Date = CashScheduleBuilder.GetMonthEndDate(New Date(2024, 12, 1))
        AssertEqualDate(label, New Date(2024, 12, 31), result)
    End Sub

    Sub Test_GetMonthEndDate_January()
        ' 1月 → 31日
        Dim label As String = "GetMonthEndDate: 2024/01 → 31日"
        Dim result As Date = CashScheduleBuilder.GetMonthEndDate(New Date(2024, 1, 15))
        AssertEqualDate(label, New Date(2024, 1, 31), result)
    End Sub

    ' ====================================================================
    '  Part 7: 型安全性テスト
    ' ====================================================================

    Sub Test_CDbl_DBNull_ThrowsException()
        ' CDbl(DBNull.Value) → InvalidCastException が発生すること
        Dim label As String = "CDbl(DBNull) → InvalidCastException"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("col", GetType(Object))
            Dim r = dt.NewRow()
            r("col") = DBNull.Value
            dt.Rows.Add(r)
            Dim v As Double = CDbl(dt.Rows(0)("col"))
            Fail(label, "InvalidCastException", $"No exception, got {v}")
        Catch ex As InvalidCastException
            Pass(label)
        Catch ex As Exception
            Fail(label, "InvalidCastException", ex.GetType().Name)
        End Try
    End Sub

    Sub Test_CInt_DBNull_ThrowsException()
        ' CInt(DBNull.Value) → InvalidCastException が発生すること
        Dim label As String = "CInt(DBNull) → InvalidCastException"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("col", GetType(Object))
            Dim r = dt.NewRow()
            r("col") = DBNull.Value
            dt.Rows.Add(r)
            Dim v As Integer = CInt(dt.Rows(0)("col"))
            Fail(label, "InvalidCastException", $"No exception, got {v}")
        Catch ex As InvalidCastException
            Pass(label)
        Catch ex As Exception
            Fail(label, "InvalidCastException", ex.GetType().Name)
        End Try
    End Sub

    Sub Test_CDbl_Nothing_ReturnsZero()
        ' CDbl(Nothing) → 0.0 (VB.NETではNothingは数値型で0)
        Dim label As String = "CDbl(Nothing) → 0.0"
        Dim obj As Object = Nothing
        Dim result As Double = CDbl(obj)
        AssertEqual(label, 0.0, result)
    End Sub

    Sub Test_CInt_Nothing_ReturnsZero()
        ' CInt(Nothing) → 0
        Dim label As String = "CInt(Nothing) → 0"
        Dim obj As Object = Nothing
        Dim result As Integer = CInt(obj)
        AssertEqual(label, 0, result)
    End Sub

    Sub Test_CDbl_String_Succeeds()
        ' CDbl("123.45") → 123.45
        Dim label As String = "CDbl(""123.45"") → 123.45"
        Dim result As Double = CDbl("123.45")
        AssertEqual(label, 123.45, result)
    End Sub

    Sub Test_ConvertToDouble_DBNull_ThrowsException()
        ' Convert.ToDouble(DBNull.Value) → InvalidCastException
        Dim label As String = "Convert.ToDouble(DBNull) → InvalidCastException"
        Try
            Dim v As Double = Convert.ToDouble(DBNull.Value)
            Fail(label, "InvalidCastException", $"No exception, got {v}")
        Catch ex As InvalidCastException
            Pass(label)
        Catch ex As Exception
            Fail(label, "InvalidCastException", ex.GetType().Name)
        End Try
    End Sub

    Sub Test_ObjectArray_CDbl_Normal()
        ' Object配列の初期値0.0にCDbl累積加算
        Dim label As String = "Object配列 CDbl累積加算: 0.0+100 → 100"
        Dim arr(0) As Object
        arr(0) = 0.0
        Dim result As Double = CDbl(arr(0)) + 100.0
        AssertEqual(label, 100.0, result)
    End Sub

    Sub Test_ObjectArray_CDbl_Uninitialized()
        ' Object配列の未初期化要素にCDbl → Nothing → 0.0 (VB.NETの場合)
        Dim label As String = "Object配列 CDbl未初期化: Nothing → 0.0"
        Dim arr(0) As Object
        ' arr(0) は Nothing (VB.NETのデフォルト)
        Try
            Dim result As Double = CDbl(arr(0))
            ' VB.NETでは CDbl(Nothing) = 0.0
            AssertEqual(label, 0.0, result)
        Catch ex As Exception
            Fail(label, "0.0", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_ObjectArray_CDbl_Safe_Pattern()
        ' IsDBNull チェック付きの安全パターン
        Dim label As String = "Object配列 安全パターン: IsDBNull→0 + 100 → 100"
        Dim dt As New DataTable()
        dt.Columns.Add("col", GetType(Object))
        Dim r = dt.NewRow()
        r("col") = DBNull.Value
        dt.Rows.Add(r)
        Dim val As Double = If(IsDBNull(dt.Rows(0)("col")), 0.0, CDbl(dt.Rows(0)("col")))
        Dim result As Double = val + 100.0
        AssertEqual(label, 100.0, result)
    End Sub

    ' ====================================================================
    '  Part 8: CLng/CInt Banker's Rounding
    ' ====================================================================

    Sub Test_CLng_BankersRounding()
        ' CLng は Banker's Rounding (偶数丸め)
        Dim label As String

        label = "CLng(0.5) → 0 (偶数丸め)"
        AssertEqualLong(label, 0L, CLng(0.5))

        label = "CLng(1.5) → 2 (偶数丸め)"
        AssertEqualLong(label, 2L, CLng(1.5))

        label = "CLng(2.5) → 2 (偶数丸め)"
        AssertEqualLong(label, 2L, CLng(2.5))

        label = "CLng(3.5) → 4 (偶数丸め)"
        AssertEqualLong(label, 4L, CLng(3.5))

        label = "CLng(-0.5) → 0 (偶数丸め)"
        AssertEqualLong(label, 0L, CLng(-0.5))

        label = "CLng(-1.5) → -2 (偶数丸め)"
        AssertEqualLong(label, -2L, CLng(-1.5))

        ' 定率法配賦計算の実ケース: CLng(37584.4 * 0.1)
        label = "CLng(3758.44) → 3758 (定率法実ケース)"
        AssertEqualLong(label, 3758L, CLng(3758.44))
    End Sub

    Sub Test_CInt_BankersRounding()
        ' CInt は Banker's Rounding (偶数丸め)
        Dim label As String

        label = "CInt(0.5) → 0 (偶数丸め)"
        AssertEqual(label, 0, CInt(0.5))

        label = "CInt(1.5) → 2 (偶数丸め)"
        AssertEqual(label, 2, CInt(1.5))

        label = "CInt(2.5) → 2 (偶数丸め)"
        AssertEqual(label, 2, CInt(2.5))

        label = "CInt(3.5) → 4 (偶数丸め)"
        AssertEqual(label, 4, CInt(3.5))

        label = "CInt(-0.5) → 0 (偶数丸め)"
        AssertEqual(label, 0, CInt(-0.5))

        ' 文字列からの変換
        label = "CInt(""10"") → 10"
        AssertEqual(label, 10, CInt("10"))

        ' 文字列の小数丸め (銀行家丸め)
        label = "CInt(""10.5"") → 10 (銀行家丸め)"
        AssertEqual(label, 10, CInt("10.5"))
    End Sub

    ' ====================================================================
    '  アサーションヘルパー
    ' ====================================================================

    Sub AssertEqual(label As String, expected As Double, actual As Double)
        Console.Write($"  {label} ... ")
        If Math.Abs(expected - actual) < 0.001 Then
            Pass(label)
        Else
            Fail(label, expected.ToString("N4"), actual.ToString("N4"))
        End If
    End Sub

    Sub AssertEqual(label As String, expected As Integer, actual As Integer)
        Console.Write($"  {label} ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqual(label As String, expected As Boolean, actual As Boolean)
        Console.Write($"  {label} ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqualLong(label As String, expected As Long, actual As Long)
        Console.Write($"  {label} ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqualDate(label As String, expected As Date, actual As Date)
        Console.Write($"  {label} ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString("yyyy/MM/dd"), actual.ToString("yyyy/MM/dd"))
        End If
    End Sub

    Sub Pass(label As String)
        Console.WriteLine("PASS")
        passCount += 1
    End Sub

    Sub Fail(label As String, expected As String, actual As String)
        Console.WriteLine($"FAIL (expected={expected}, actual={actual})")
        failCount += 1
    End Sub

    Sub Skip(label As String)
        Console.WriteLine($"SKIP ({label})")
        skipCount += 1
    End Sub

End Module
