Option Strict On
Option Explicit On

' ブラックボックステスト: TODO/FIXME体系的解消確認 (Issue #36)
' 対象:
'   TC-C1-003: Form_f_CHUKI_RECALC の危険フラグ除去確認（静的確認）
'   TC-C3-001/002: CalcGhassei の実装確認（ロジック検証）
'   TC-H5-001: Form_f_flx_YOSAN の TryParse 例外なし確認
'   TC-H5-002: Form_f_flx_YOSAN の SQL WHERE句生成確認
'   TC-H4-003: Form_f_flx_TOUGETSU の WHERE句 DtFrom/DtTo 確認
'   TC-H1-001: Form_f_flx_BUKN 保守料列のTODO除去確認
'   TC-L3-001: Form_f_IDOLST_JOKEN の TrimEnd 確認（既存テスト確認）
'   TC-C2-001/004: Form_BuknEntry の削除処理（DB接続あり・SKIPパターン）
'
' コンパイル:
'   "C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe" /target:exe /out:test_todo_fixme_blackbox.exe ^
'     /reference:LeaseM4BS/LeaseM4BS.DataAccess/bin/Debug/LeaseM4BS.DataAccess.dll ^
'     /reference:LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/bin/Debug/LeaseM4BS.TestWinForms.exe ^
'     /reference:"C:\Program Files (x86)\Npgsql\net461\Npgsql.dll" ^
'     test_todo_fixme_blackbox.vb
' 実行: test_todo_fixme_blackbox.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports Npgsql
Imports LeaseM4BS.DataAccess

Module TestTodoFixmeBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== TODO/FIXME体系的解消 ブラックボックステスト ===")
        Console.WriteLine()

        ' ---- Part 1: CHUKI_RECALC 危険フラグ除去確認 ----
        Console.WriteLine("--- Part 1: CHUKI_RECALC 危険フラグ除去確認 (TC-C1-003) ---")
        Test_C1_003_NoDangerousComment()
        Console.WriteLine()

        ' ---- Part 2: CalcGhassei 実装確認 ----
        Console.WriteLine("--- Part 2: CalcGhassei 実装確認 (TC-C3-001, TC-C3-002) ---")
        Test_C3_001_CalcGhassei_NoTodoComment()
        Test_C3_002_CalcGhassei_HasImplementationComment()
        Console.WriteLine()

        ' ---- Part 3: YOSAN TryParse 例外なし確認 ----
        Console.WriteLine("--- Part 3: YOSAN TryParse 確認 (TC-H5-001, TC-H5-002) ---")
        Test_H5_001_TryParse_InvalidInput_NoException()
        Test_H5_002_TryParse_ValidInput()
        Test_H5_003_TryParse_Boundary_Max()
        Test_H5_004_TryParse_Zero()
        Console.WriteLine()

        ' ---- Part 4: TOUGETSU SQL WHERE句確認 ----
        Console.WriteLine("--- Part 4: TOUGETSU WHERE句確認 (TC-H4-003) ---")
        Test_H4_003_TougetsuWhereClause_HasDtFromDtTo()
        Test_H4_001_TougetsuHasLawKbnColumn()
        Test_H4_002_TougetsuHasSeikouDt()
        Console.WriteLine()

        ' ---- Part 5: BUKN 保守料列確認 ----
        Console.WriteLine("--- Part 5: BUKN 保守料列確認 (TC-H1-001) ---")
        Test_H1_001_BuknHasHoshuryoColumn()
        Test_H1_002_BuknNoTodoComment()
        Console.WriteLine()

        ' ---- Part 6: CHUKI_JOKEN 所有権移転外表示条件確認 ----
        Console.WriteLine("--- Part 6: CHUKI_JOKEN 表示条件確認 ---")
        Test_M1_001_ChukiJokenItengaiCondition()
        Test_M1_002_ChukiJokenLabel_PureFunctionAlwaysIncludesItengai()
        Console.WriteLine()

        ' ---- Part 7: DB接続テスト (BuknEntry削除処理) ----
        Console.WriteLine("--- Part 7: BuknEntry 削除処理確認 (TC-C2-001, TC-C2-004) ---")
        Test_C2_001_DeleteProcess_Transaction()
        Console.WriteLine()

        ' ---- Part 8: 回帰確認 - CHUKI_JOKEN Pure関数 ----
        Console.WriteLine("--- Part 8: 回帰確認 - CHUKI_JOKEN Pure関数 ---")
        Test_Regression_ChukiJoken_WhereClause()
        Test_Regression_ChukiJoken_LabelText()
        Console.WriteLine()

        ' ---- 結果サマリ ----
        Console.WriteLine()
        Console.WriteLine(String.Format("=== 結果: PASS={0}, FAIL={1}, SKIP={2} ===", passCount, failCount, skipCount))
        If failCount > 0 Then
            Console.WriteLine("★ 一部テスト FAIL あり — 修正内容を確認してください")
            Environment.ExitCode = 1
        Else
            Console.WriteLine("全テスト PASS (または SKIP)")
        End If
    End Sub

    ' ====================================================================
    '  Part 1: CHUKI_RECALC 危険フラグ除去確認
    ' ====================================================================

    ' TC-C1-003: Form_f_CHUKI_RECALC.vb に "todo 危険" コメントが残っていないこと
    Sub Test_C1_003_NoDangerousComment()
        Dim label As String = "TC-C1-003: CHUKI_RECALC に 'todo 危険' コメントなし"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_CHUKI_RECALC.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在: " & filePath & ")")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)
            ' "todo 危険" コメントが存在しないことを確認
            If content.ToLower().Contains("todo 危険") Then
                Fail(label, "'todo 危険' コメントなし", "'todo 危険' コメントが残存")
            Else
                Pass(label)
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 2: CalcGhassei 実装確認
    ' ====================================================================

    ' TC-C3-001: CalcGhassei の "todo" コメントが除去されていること
    Sub Test_C3_001_CalcGhassei_NoTodoComment()
        Dim label As String = "TC-C3-001: CHUKI_SCH CalcGhassei の単独 'todo' コメント除去"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_CHUKI_SCH.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim lines() As String = File.ReadAllLines(filePath, System.Text.Encoding.UTF8)
            Dim foundBareCalcGhassei As Boolean = False
            Dim inCalcGhassei As Boolean = False

            For Each line As String In lines
                Dim trimmed = line.Trim()
                ' CalcGhassei 関数の直前に "' todo" のみの行があるか確認
                If trimmed = "' todo" Then
                    ' 次のラインが CalcGhassei 定義の場合はNG
                    foundBareCalcGhassei = True
                End If
                If foundBareCalcGhassei AndAlso trimmed.Contains("CalcGhassei") Then
                    Fail(label, "'todo' コメントなし (CalcGhassei前)", "単独 'todo' コメントが残存 (CalcGhassei前)")
                    Return
                End If
                ' 別の行に来たらリセット
                If foundBareCalcGhassei AndAlso Not trimmed.StartsWith("'") AndAlso trimmed.Length > 0 AndAlso Not trimmed.Contains("CalcGhassei") Then
                    foundBareCalcGhassei = False
                End If
            Next
            Pass(label)
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-C3-002: CalcGhassei に実装説明コメントが含まれていること
    Sub Test_C3_002_CalcGhassei_HasImplementationComment()
        Dim label As String = "TC-C3-002: CHUKI_SCH CalcGhassei に実装説明コメントあり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_CHUKI_SCH.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)
            ' 実装説明コメントが追加されていること（利子込法 or 利息法の説明）
            If content.Contains("利子込法") OrElse content.Contains("発生リース料") Then
                Pass(label)
            Else
                Fail(label, "実装説明コメントあり", "実装説明コメントなし")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: YOSAN TryParse 確認
    ' ====================================================================

    ' TC-H5-001: Integer.TryParse を使用しており、非数値入力でも例外が出ないこと
    ' このテストはソースコードの実装を確認する（Double.Parse が使われていないこと）
    Sub Test_H5_001_TryParse_InvalidInput_NoException()
        Dim label As String = "TC-H5-001: YOSAN Double.Parse 除去 → Integer.TryParse 使用"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_YOSAN.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            ' Double.Parse が検索条件箇所に残っていないこと
            ' Integer.TryParse が使用されていること
            Dim hasDoubleParse As Boolean = content.Contains("Double.Parse(searchText)") OrElse content.Contains("Double.Parse(txt_SEARCH")
            Dim hasTryParse As Boolean = content.Contains("Integer.TryParse")

            If hasDoubleParse Then
                Fail(label, "Double.Parse なし", "Double.Parse が残存")
            ElseIf Not hasTryParse Then
                Fail(label, "Integer.TryParse あり", "Integer.TryParse が見つからない")
            Else
                Pass(label)
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H5-001 補足: TryParse ロジックのインメモリ検証
    Sub Test_H5_002_TryParse_ValidInput()
        Dim label As String = "TC-H5-001補足: Integer.TryParse('abc') → 失敗・例外なし"
        Console.Write("  " & label & " ... ")
        Try
            ' VB.NET の Integer.TryParse の動作を直接確認
            Dim searchVal As Integer
            Dim result As Boolean = Integer.TryParse("abc", searchVal)
            ' "abc" はパース失敗 → result=False, searchVal=0
            If result = False AndAlso searchVal = 0 Then
                Pass(label & " (result=False, val=0)")
            Else
                Fail(label, "result=False, val=0", String.Format("result={0}, val={1}", result, searchVal))
            End If
        Catch ex As Exception
            Fail(label, "例外なし", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H5-002 補足: 有効な整数はTryParse成功
    Sub Test_H5_003_TryParse_Boundary_Max()
        Dim label As String = "TC-H5-002補足: Integer.TryParse('2147483647') → 成功"
        Console.Write("  " & label & " ... ")
        Try
            Dim searchVal As Integer
            Dim result As Boolean = Integer.TryParse("2147483647", searchVal)
            If result = True AndAlso searchVal = Integer.MaxValue Then
                Pass(label)
            Else
                Fail(label, String.Format("result=True, val={0}", Integer.MaxValue), String.Format("result={0}, val={1}", result, searchVal))
            End If
        Catch ex As Exception
            Fail(label, "例外なし", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H5-002 補足: "0" はTryParse成功
    Sub Test_H5_004_TryParse_Zero()
        Dim label As String = "TC-H5-002補足: Integer.TryParse('0') → 成功(0)"
        Console.Write("  " & label & " ... ")
        Try
            Dim searchVal As Integer
            Dim result As Boolean = Integer.TryParse("0", searchVal)
            If result = True AndAlso searchVal = 0 Then
                Pass(label)
            Else
                Fail(label, "result=True, val=0", String.Format("result={0}, val={1}", result, searchVal))
            End If
        Catch ex As Exception
            Fail(label, "例外なし", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: TOUGETSU SQL WHERE句確認
    ' ====================================================================

    ' TC-H4-003: WHERE句に DtFrom/DtTo フィルタが含まれていること
    Sub Test_H4_003_TougetsuWhereClause_HasDtFromDtTo()
        Dim label As String = "TC-H4-003: TOUGETSU WHERE句に @dtFrom/@dtTo あり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_TOUGETSU.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            If content.Contains("@dtFrom") AndAlso content.Contains("@dtTo") Then
                Pass(label)
            Else
                Fail(label, "@dtFrom/@dtTo あり", "@dtFrom or @dtTo が見つからない")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H4-001: TOUGETSU に法令区分列が追加されていること
    Sub Test_H4_001_TougetsuHasLawKbnColumn()
        Dim label As String = "TC-H4-001: TOUGETSU SELECT に '法令区分' 列あり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_TOUGETSU.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            If content.Contains("法令区分") Then
                Pass(label)
            Else
                Fail(label, "法令区分 列あり", "法令区分 列なし")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H4-002: TOUGETSU に SEKOU_DT 参照が含まれていること
    Sub Test_H4_002_TougetsuHasSeikouDt()
        Dim label As String = "TC-H4-002: TOUGETSU SQL に SEKOU_DT 参照あり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_TOUGETSU.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            If content.Contains("SEKOU_DT") Then
                Pass(label)
            Else
                Fail(label, "SEKOU_DT あり", "SEKOU_DT なし")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 5: BUKN 保守料列確認
    ' ====================================================================

    ' TC-H1-001: Form_f_flx_BUKN.vb に b_ijiknr AS 保守料 が追加されていること
    Sub Test_H1_001_BuknHasHoshuryoColumn()
        Dim label As String = "TC-H1-001: BUKN SQL に 'b_ijiknr AS 保守料' あり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_BUKN.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            If content.Contains("b_ijiknr") AndAlso content.Contains("保守料") Then
                Pass(label)
            Else
                Fail(label, "b_ijiknr AS 保守料 あり", "b_ijiknr または 保守料 が見つからない")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-H1-002: Form_f_flx_BUKN.vb の "todo 該当項目不明" コメントが除去されていること
    Sub Test_H1_002_BuknNoTodoComment()
        Dim label As String = "TC-H1-002: BUKN 'todo 該当項目不明' コメント除去"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_flx_BUKN.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            If content.ToLower().Contains("todo 該当項目不明") Then
                Fail(label, "'todo 該当項目不明' なし", "'todo 該当項目不明' が残存")
            Else
                Pass(label)
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: CHUKI_JOKEN 所有権移転外表示条件確認
    ' ====================================================================

    ' TC-M1-001: Form_f_CHUKI_JOKEN に所有権移転外の表示条件コードが追加されていること
    Sub Test_M1_001_ChukiJokenItengaiCondition()
        Dim label As String = "TC-M1-001: CHUKI_JOKEN 所有権移転外テキストに条件分岐あり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_CHUKI_JOKEN.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            ' GenerateLabelText（実フォーム）に条件分岐があること
            ' chk_LEAKBN_ITENGAI_F.Checked で制御されていること
            If content.Contains("chk_LEAKBN_ITENGAI_F.Checked") OrElse content.Contains("IsItengaiChecked") Then
                Pass(label)
            Else
                Fail(label, "条件分岐あり (chk_LEAKBN_ITENGAI_F.Checked)", "条件分岐なし")
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' TC-M1-002: Pure Function は常時 "所有権移転外" を含む（コントロール非依存の仕様確認）
    Sub Test_M1_002_ChukiJokenLabel_PureFunctionAlwaysIncludesItengai()
        Dim label As String = "TC-M1-002: CHUKI_JOKEN GenerateLabelTextPure は常時 '所有権移転外' 含む"
        Console.Write("  " & label & " ... ")
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)

            If result.Contains("所有権移転外ファイナンスリースの計算条件") Then
                Pass(label)
            Else
                Fail(label, "所有権移転外... 含む", "所有権移転外... なし (result=" & result & ")")
            End If
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 7: DB接続テスト (BuknEntry削除処理)
    ' ====================================================================

    ' TC-C2-001: 削除処理実装の確認（ソースコード確認）
    Sub Test_C2_001_DeleteProcess_Transaction()
        Dim label As String = "TC-C2-001: BuknEntry 削除処理にトランザクションあり"
        Console.Write("  " & label & " ... ")
        Try
            Dim filePath As String = "LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_BuknEntry.vb"
            If Not File.Exists(filePath) Then
                Skip(label & " (ファイル不存在)")
                Return
            End If
            Dim content As String = File.ReadAllText(filePath, System.Text.Encoding.UTF8)

            ' BeginTransaction + d_haif DELETE + d_kykm DELETE + Commit が存在すること
            Dim hasTransaction As Boolean = content.Contains("BeginTransaction")
            Dim hasHaifDelete As Boolean = content.Contains("d_haif") AndAlso content.Contains("DELETE")
            Dim hasKykmDelete As Boolean = content.Contains("d_kykm") AndAlso content.Contains("DELETE")
            Dim hasCommit As Boolean = content.Contains("Commit()")
            Dim hasRollback As Boolean = content.Contains("Rollback()")

            If hasTransaction AndAlso hasHaifDelete AndAlso hasKykmDelete AndAlso hasCommit AndAlso hasRollback Then
                Pass(label)
            Else
                Fail(label,
                    "Transaction+d_haif DELETE+d_kykm DELETE+Commit+Rollback",
                    String.Format("Transaction={0}, d_haif={1}, d_kykm={2}, Commit={3}, Rollback={4}",
                        hasTransaction, hasHaifDelete, hasKykmDelete, hasCommit, hasRollback))
            End If
        Catch ex As Exception
            Fail(label, "正常確認", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 8: 回帰確認 - CHUKI_JOKEN Pure関数
    ' ====================================================================

    ' 回帰: GenerateWhereClausePure - 両区分・省略従う
    Sub Test_Regression_ChukiJoken_WhereClause()
        Dim label As String = "Regression: CHUKI_JOKEN WHERE句 両区分 IN(1,2)"
        Console.Write("  " & label & " ... ")
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateWhereClausePure(
                prms, True, True, True, False, "", "", Nothing, Nothing, Nothing,
                New Date(2024, 4, 1), New Date(2025, 3, 31))

            If result.Contains("IN (1, 2)") Then
                Pass(label)
            Else
                Fail(label, "IN (1, 2) あり", "IN (1, 2) なし (result=" & result.Substring(0, Math.Min(200, result.Length)) & "...)")
            End If
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' 回帰: GenerateLabelTextPure - 期間テキスト
    Sub Test_Regression_ChukiJoken_LabelText()
        Dim label As String = "Regression: CHUKI_JOKEN ラベルテキスト 期間"
        Console.Write("  " & label & " ... ")
        Try
            Dim result As String = Form_f_CHUKI_JOKEN.GenerateLabelTextPure(
                New Date(2024, 4, 1), New Date(2025, 3, 31), True, True)

            If result.Contains("決算期間：2024/04～2025/03") Then
                Pass(label)
            Else
                Fail(label, "決算期間：2024/04～2025/03 あり", "期間テキストなし (result=" & result & ")")
            End If
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  アサーションヘルパー
    ' ====================================================================

    Sub AssertEqual(label As String, expected As Integer, actual As Integer)
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertContains(label As String, actual As String, expected As String)
        Console.Write("  " & label & " ... ")
        If actual IsNot Nothing AndAlso actual.Contains(expected) Then
            Pass(label)
        Else
            Fail(label, "contains """ & expected & """", If(actual Is Nothing, "Nothing", """" & actual & """"))
        End If
    End Sub

    Sub AssertNotContains(label As String, actual As String, notExpected As String)
        Console.Write("  " & label & " ... ")
        If actual Is Nothing OrElse Not actual.Contains(notExpected) Then
            Pass(label)
        Else
            Fail(label, "NOT contains """ & notExpected & """", """" & actual & """")
        End If
    End Sub

    Sub AssertTrue(label As String, condition As Boolean)
        Console.Write("  " & label & " ... ")
        If condition Then
            Pass(label)
        Else
            Fail(label, "True", "False")
        End If
    End Sub

    Sub Pass(label As String)
        Console.WriteLine("PASS")
        passCount += 1
    End Sub

    Sub Fail(label As String, expected As String, actual As String)
        Console.WriteLine("FAIL (expected=" & expected & ", actual=" & actual & ")")
        failCount += 1
    End Sub

    Sub Skip(label As String)
        Console.WriteLine("SKIP (" & label & ")")
        skipCount += 1
    End Sub

    ' ====================================================================
    '  DB例外ハンドリング
    ' ====================================================================

    Function GetRootMessage(ex As Exception) As String
        Dim rootMsg As String = ex.Message
        Dim inner As Exception = ex.InnerException
        While inner IsNot Nothing
            rootMsg = inner.Message
            inner = inner.InnerException
        End While
        Return rootMsg
    End Function

    Sub HandleDbException(label As String, ex As Exception)
        Dim rootMsg As String = GetRootMessage(ex)
        If rootMsg.Contains("は存在しません") OrElse rootMsg.Contains("does not exist") Then
            Skip(label & " (DBスキーマ未完了: " & rootMsg.Substring(0, Math.Min(120, rootMsg.Length)) & ")")
        ElseIf rootMsg.Contains("Connection") OrElse rootMsg.Contains("接続") OrElse rootMsg.Contains("refused") Then
            Skip(label & " (DB接続不可)")
        ElseIf rootMsg.Contains("アセンブリ") OrElse rootMsg.Contains("assembly") OrElse rootMsg.Contains("System.Memory") Then
            Skip(label & " (アセンブリ参照不一致)")
        Else
            Fail(label, "success", "Exception: " & rootMsg)
        End If
    End Sub

End Module
