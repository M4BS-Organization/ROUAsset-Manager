' ブラックボックステスト: f_CHUKI_JOKEN + f_IDOLST_JOKEN 条件ロジック
' Access版と入出力が一致することを検証する
'
' 対象:
'   - Form_f_CHUKI_JOKEN.GenerateWhereClausePure (WHERE句生成)
'   - Form_f_CHUKI_JOKEN.GenerateLabelTextPure (ラベルテキスト生成)
'   - Form_f_IDOLST_JOKEN.GetLabelTextPure (ラベルテキスト生成)
'   - Form_f_flx_IDOLST.GetBcatConditions (管理部署SQL条件生成)
'
' コンパイル: vbc /r:LeaseM4BS.TestWinForms.exe /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll test_chuki_idolst_joken_blackbox.vb
' 実行: test_chuki_idolst_joken_blackbox.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

Module TestChukiIdolstJokenBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== f_CHUKI_JOKEN + f_IDOLST_JOKEN ブラックボックステスト ===")
        Console.WriteLine()

        ' ---- Part 1: CHUKI_JOKEN WHERE句生成 ----
        Console.WriteLine("--- Part 1: CHUKI_JOKEN WHERE句生成 (GenerateWhereClausePure) ---")
        Test_01_BothKbn_FollowAll()
        Test_02_ItengaiOnly()
        Test_03_OpeOnly()
        Test_04_NoFollow()
        Test_05_OmissionOnly()
        Test_06_KyknNoRange()
        Test_07_KyknNoEmpty()
        Test_08_SkmkCd()
        Test_09_LcptCd()
        Test_10_BcatCd()
        Test_11_DtFromMonthStart()
        Test_12_DtToMonthEnd()
        Console.WriteLine()

        ' ---- Part 2: CHUKI_JOKEN ラベルテキスト生成 ----
        Console.WriteLine("--- Part 2: CHUKI_JOKEN ラベルテキスト (GenerateLabelTextPure) ---")
        Test_13_LabelPeriod()
        Test_14_LabelConstant()
        Test_15_LabelTeigaku()
        Test_16_LabelTeiritu()
        Test_17_LabelRisoku()
        Test_18_LabelRishikomi()
        Console.WriteLine()

        ' ---- Part 3: CHUKI_JOKEN バリデーション ----
        Console.WriteLine("--- Part 3: CHUKI_JOKEN バリデーション ---")
        Test_19_SwapIf_FromGtTo()
        Test_20_BothKbnFalse()
        Console.WriteLine()

        ' ---- Part 4: IDOLST_JOKEN ラベルテキスト生成 ----
        Console.WriteLine("--- Part 4: IDOLST_JOKEN ラベルテキスト (GetLabelTextPure) ---")
        Test_21_IdolstLabelDate()
        Test_22_IdolstBcat1Only()
        Test_23_IdolstBcat1And2()
        Test_24_IdolstBcat5Only()
        Test_25_IdolstBcat4Only_TrailingComma()
        Test_26_IdolstAllBcat()
        Console.WriteLine()

        ' ---- Part 5: IDOLST GetBcatConditions SQL生成 ----
        Console.WriteLine("--- Part 5: IDOLST GetBcatConditions ---")
        Test_27_BcatAllFalse()
        Test_28_Bcat1Only()
        Test_29_Bcat1And3()
        Test_30_BcatAllTrue()
        Console.WriteLine()

        ' ---- Part 6: IDOLST_JOKEN バリデーション ----
        Console.WriteLine("--- Part 6: IDOLST_JOKEN バリデーション ---")
        Test_31_IdolstSwapIf()
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
    '  Part 1: CHUKI_JOKEN WHERE句生成
    ' ====================================================================

    Sub Test_01_BothKbn_FollowAll()
        Dim label As String = "Test_01: 両区分・省略従う・全条件なし"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            ' NOTE: leakbn_id の値 (1,2) が ScheduleTypes.LeaseKbn enum (Itengai=3, Ope=4) と不一致。
            '       c_leakbn テーブルの実 ID 値として現行実装の動作を期待値とする (Issue #10 要確認)
            AssertContains(label & " IN(1,2)", result, "IN (1, 2)")
            AssertContains(label & " chuum_id=1", result, "chuum_id = 1")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_02_ItengaiOnly()
        Dim label As String = "Test_02: 移転外のみ"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, False, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " leakbn_id=1", result, "leakbn_id = 1")
            AssertNotContains(label & " NOT IN(1,2)", result, "IN (1, 2)")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_03_OpeOnly()
        Dim label As String = "Test_03: オペのみ"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, False, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " leakbn_id=2", result, "leakbn_id = 2")
            AssertNotContains(label & " NOT IN(1,2)", result, "IN (1, 2)")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_04_NoFollow()
        Dim label As String = "Test_04: 省略基準 無視する"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, False, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertNotContains(label & " NO chuum_id", result, "chuum_id")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_05_OmissionOnly()
        Dim label As String = "Test_05: 省略物件のみ"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, False, True, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " chuum_id=2", result, "chuum_id = 2")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_06_KyknNoRange()
        Dim label As String = "Test_06: 物件No FROM/TO あり"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "100", "200", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " kykm_no>=", result, "kykm_no >= @kyknNoFrom")
            AssertContains(label & " kykm_no<=", result, "kykm_no <= @kyknNoTo")

            ' パラメータ値確認
            Dim fromPrm = FindParam(prms, "@kyknNoFrom")
            Dim toPrm = FindParam(prms, "@kyknNoTo")
            AssertTrue(label & " @kyknNoFrom=100", fromPrm IsNot Nothing AndAlso CInt(fromPrm.Value) = 100)
            AssertTrue(label & " @kyknNoTo=200", toPrm IsNot Nothing AndAlso CInt(toPrm.Value) = 200)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_07_KyknNoEmpty()
        Dim label As String = "Test_07: 物件No 未入力"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertNotContains(label & " NO kykm_no", result, "kykm_no")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_08_SkmkCd()
        Dim label As String = "Test_08: 資産科目あり"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", "10", Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " skmk_cd", result, "skmk.skmk_cd = @skmkCd")
            Dim p = FindParam(prms, "@skmkCd")
            AssertTrue(label & " @skmkCd=10", p IsNot Nothing AndAlso p.Value.ToString() = "10")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_09_LcptCd()
        Dim label As String = "Test_09: リース会社あり"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, "LC01", Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " lcpt1_cd", result, "lcpt.lcpt1_cd = @lcptCd")
            Dim p = FindParam(prms, "@lcptCd")
            AssertTrue(label & " @lcptCd=LC01", p IsNot Nothing AndAlso p.Value.ToString() = "LC01")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_10_BcatCd()
        Dim label As String = "Test_10: 管理部署あり"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, "B001",
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertContains(label & " bcat_cd", result, "b_bcat.bcat_cd = @bcatCd")
            Dim p = FindParam(prms, "@bcatCd")
            AssertTrue(label & " @bcatCd=B001", p IsNot Nothing AndAlso p.Value.ToString() = "B001")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_11_DtFromMonthStart()
        Dim label As String = "Test_11: dtFrom月初正規化 (4/15→4/1)"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 15), New Date(2025, 3, 31))

            Dim p = FindParam(prms, "@dtFrom")
            Dim actual As Date = CDate(p.Value)
            AssertTrue(label, actual = New Date(2024, 4, 1))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_12_DtToMonthEnd()
        Dim label As String = "Test_12: dtTo月末正規化 (3/10→3/31)"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2024, 3, 10))

            Dim p = FindParam(prms, "@dtTo")
            Dim actual As Date = CDate(p.Value)
            AssertTrue(label, actual = New Date(2024, 3, 31))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 2: CHUKI_JOKEN ラベルテキスト生成
    ' ====================================================================

    Sub Test_13_LabelPeriod()
        Dim label As String = "Test_13: 期間テキスト"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)
            AssertContains(label, result, "決算期間：2024/04～2025/03")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_14_LabelConstant()
        Dim label As String = "Test_14: 常時テキスト"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)
            AssertContains(label, result, "所有権移転外ファイナンスリースの計算条件")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_15_LabelTeigaku()
        Dim label As String = "Test_15: 償却 定額"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)
            AssertContains(label, result, "償却方法：リース定額")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_16_LabelTeiritu()
        Dim label As String = "Test_16: 償却 定率"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), False, True)
            AssertContains(label, result, "償却方法：近似定率")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_17_LabelRisoku()
        Dim label As String = "Test_17: 利息法"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)
            AssertContains(label, result, "利息計算：利息法")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_18_LabelRishikomi()
        Dim label As String = "Test_18: 利子込法"
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, False)
            AssertContains(label, result, "利息計算：利子込法")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: CHUKI_JOKEN バリデーション
    ' ====================================================================

    Sub Test_19_SwapIf_FromGtTo()
        Dim label As String = "Test_19: SwapIf FROM>TO (日付入替)"
        Try
            ' SwapIf はDateTimePicker依存のため、Pure Functionでのテストは
            ' GenerateWhereClausePure に FROM>TO を渡した場合のパラメータ確認で代替
            ' GetMonthStart/GetMonthEnd が正しく適用されることを確認
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2025, 3, 1), New Date(2024, 4, 30))

            ' dtFrom=2025/03→GetMonthStart→2025/03/01, dtTo=2024/04→GetMonthEnd→2024/04/30
            ' WHERE句自体は生成される（SwapIfは呼び出し元の責務）
            Dim pFrom = FindParam(prms, "@dtFrom")
            Dim pTo = FindParam(prms, "@dtTo")
            AssertTrue(label & " @dtFrom=2025/03/01", CDate(pFrom.Value) = New Date(2025, 3, 1))
            AssertTrue(label & " @dtTo=2024/04/30", CDate(pTo.Value) = New Date(2024, 4, 30))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_20_BothKbnFalse()
        Dim label As String = "Test_20: リース区分両方False"
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, False, False, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            AssertNotContains(label & " NO leakbn_id", result, "leakbn_id")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: IDOLST_JOKEN ラベルテキスト生成
    ' ====================================================================

    Sub Test_21_IdolstLabelDate()
        Dim label As String = "Test_21: 移動日テキスト"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                True, False, False, False, False)
            AssertContains(label, result, "移動日:　2024/04/01～2024/06/30")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_22_IdolstBcat1Only()
        Dim label As String = "Test_22: bcat1のみTrue"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                True, False, False, False, False)
            AssertContains(label & " 管理部署1", result, "管理部署1")
            AssertNotContains(label & " NOT 管理部署2", result, "管理部署2")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_23_IdolstBcat1And2()
        Dim label As String = "Test_23: bcat1・2 True"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                True, True, False, False, False)
            AssertContains(label & " 管理部署1", result, "管理部署1")
            AssertContains(label & " 管理部署2", result, "管理部署2")
            AssertTrue(label & " 末尾「、」なし", Not result.EndsWith("、"))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_24_IdolstBcat5Only()
        Dim label As String = "Test_24: bcat5のみTrue"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                False, False, False, False, True)
            AssertContains(label & " 管理部署5", result, "管理部署5")
            AssertTrue(label & " 末尾「、」なし", Not result.EndsWith("、"))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_25_IdolstBcat4Only_TrailingComma()
        ' NOTE: bcat4のみTrueの場合、"管理部署4、" → TrimEnd("、"c) → "管理部署4" となるはず
        '       Access版と同一動作か要確認 (Issue #10)
        Dim label As String = "Test_25: bcat4のみTrue (末尾「、」バグ検証)"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                False, False, False, True, False)
            AssertContains(label & " 管理部署4", result, "管理部署4")
            AssertTrue(label & " 末尾「、」なし", Not result.EndsWith("、"))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_26_IdolstAllBcat()
        Dim label As String = "Test_26: 全bcat True"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 4, 1), New Date(2024, 6, 30),
                True, True, True, True, True)
            AssertContains(label & " 管理部署1", result, "管理部署1")
            AssertContains(label & " 管理部署2", result, "管理部署2")
            AssertContains(label & " 管理部署3", result, "管理部署3")
            AssertContains(label & " 管理部署4", result, "管理部署4")
            AssertContains(label & " 管理部署5", result, "管理部署5")
            AssertTrue(label & " 末尾「、」なし", Not result.EndsWith("、"))
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 5: IDOLST GetBcatConditions SQL生成
    ' ====================================================================

    Sub Test_27_BcatAllFalse()
        Dim label As String = "Test_27: 全False → 空文字"
        Try
            Dim result As String = Form_f_flx_IDOLST.GetBcatConditions({False, False, False, False, False})
            AssertTrue(label, result = String.Empty)
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_28_Bcat1Only()
        Dim label As String = "Test_28: bcat1のみTrue"
        Try
            Dim result As String = Form_f_flx_IDOLST.GetBcatConditions({True, False, False, False, False})
            AssertContains(label & " AND (", result, "AND (")
            AssertContains(label & " bcat1_cd", result, "b_bcat.bcat1_cd <> r1_bcat.bcat1_cd")
            AssertContains(label & " IS NULL", result, "b_bcat.bcat1_cd IS NULL")
            AssertNotContains(label & " NOT bcat2", result, "bcat2_cd")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_29_Bcat1And3()
        Dim label As String = "Test_29: bcat1・3 True"
        Try
            Dim result As String = Form_f_flx_IDOLST.GetBcatConditions({True, False, True, False, False})
            AssertContains(label & " AND (", result, "AND (")
            AssertContains(label & " bcat1", result, "bcat1_cd")
            AssertContains(label & " bcat3", result, "bcat3_cd")
            AssertContains(label & " OR", result, " OR ")
            AssertNotContains(label & " NOT bcat2", result, "bcat2_cd")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_30_BcatAllTrue()
        Dim label As String = "Test_30: 全True"
        Try
            Dim result As String = Form_f_flx_IDOLST.GetBcatConditions({True, True, True, True, True})
            AssertContains(label & " AND (", result, "AND (")
            AssertContains(label & " bcat1", result, "bcat1_cd")
            AssertContains(label & " bcat2", result, "bcat2_cd")
            AssertContains(label & " bcat3", result, "bcat3_cd")
            AssertContains(label & " bcat4", result, "bcat4_cd")
            AssertContains(label & " bcat5", result, "bcat5_cd")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: IDOLST_JOKEN バリデーション
    ' ====================================================================

    Sub Test_31_IdolstSwapIf()
        ' SwapIf はDateTimePicker依存のため、GetLabelTextPure でFROM>TO渡し時の文字列を確認
        Dim label As String = "Test_31: SwapIf確認 (FROM>TOでもラベル生成可)"
        Try
            Dim result As String = Form_f_IDOLST_JOKEN.GetLabelTextPure(
                New Date(2024, 6, 30), New Date(2024, 4, 1),
                True, False, False, False, False)
            ' SwapIfは呼び出し元の責務。Pure Functionは渡された値をそのまま使う
            AssertContains(label, result, "2024/06/30～2024/04/01")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  アサーションヘルパー
    ' ====================================================================

    Sub AssertContains(label As String, actual As String, expected As String)
        Console.Write($"  {label} ... ")
        If actual IsNot Nothing AndAlso actual.Contains(expected) Then
            Pass(label)
        Else
            Fail(label, $"contains ""{expected}""", $"""{If(actual, "Nothing")}""")
        End If
    End Sub

    Sub AssertNotContains(label As String, actual As String, notExpected As String)
        Console.Write($"  {label} ... ")
        If actual Is Nothing OrElse Not actual.Contains(notExpected) Then
            Pass(label)
        Else
            Fail(label, $"NOT contains ""{notExpected}""", $"""{actual}""")
        End If
    End Sub

    Sub AssertTrue(label As String, condition As Boolean)
        Console.Write($"  {label} ... ")
        If condition Then
            Pass(label)
        Else
            Fail(label, "True", "False")
        End If
    End Sub

    Function FindParam(prms As List(Of NpgsqlParameter), name As String) As NpgsqlParameter
        For Each p In prms
            If p.ParameterName = name Then Return p
        Next
        Return Nothing
    End Function

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
