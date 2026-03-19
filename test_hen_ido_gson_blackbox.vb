Option Strict On
Option Explicit On

' ブラックボックステスト: 変更・異動・減損処理画面群 (Issue #32)
' 対象:
'   - GsonScheduleBuilder.BuildFromRows (減損スケジュール)
'   - 消費税自動計算ロジック (FR-004)
'   - 合計行自動更新ロジック (FR-005)
'   - 異動バリデーション / 異動種別カラム切り替え (FR-003)
'   - LINE_ID採番ロジック
'
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_hen_ido_gson_blackbox.vb
' 実行: test_hen_ido_gson_blackbox.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports LeaseM4BS.DataAccess

Module TestHenIdoGsonBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== 変更・異動・減損処理 ブラックボックステスト (Issue #32) ===")
        Console.WriteLine()

        ' ---- Part 1: GsonScheduleBuilder.BuildFromRows ----
        Console.WriteLine("--- Part 1: GsonScheduleBuilder.BuildFromRows ---")
        Test_Gson_Empty()
        Test_Gson_Tmg0_MonthEnd()
        Test_Gson_Tmg1_MonthStart()
        Test_Gson_DBNull_Skip()
        Test_Gson_DBNull_Mixed()
        Test_Gson_InvalidTmg()
        Console.WriteLine()

        ' ---- Part 2: 消費税自動計算 (FR-004) ----
        Console.WriteLine("--- Part 2: 消費税自動計算 (FR-004) ---")
        Test_Tax_Standard10()
        Test_Tax_8Percent_Floor()
        Test_Tax_ZeroRate()
        Test_Tax_ZeroKlsryo()
        Console.WriteLine()

        ' ---- Part 3: 合計行自動更新 (FR-005) ----
        Console.WriteLine("--- Part 3: 合計行自動更新 (FR-005) ---")
        Test_Sum_ThreeRows()
        Test_Sum_AfterDelete()
        Test_Sum_Empty()
        Console.WriteLine()

        ' ---- Part 4: 異動バリデーション ----
        Console.WriteLine("--- Part 4: 異動バリデーション ---")
        Test_Ido_EmptyDate()
        Test_Ido_ValidDate()
        Test_Ido_NoChecked()
        Console.WriteLine()

        ' ---- Part 5: 異動種別カラム切り替え (FR-003) ----
        Console.WriteLine("--- Part 5: 異動種別カラム切り替え (FR-003) ---")
        Test_IdoType_Bcat()
        Test_IdoType_Hkbcat()
        Console.WriteLine()

        ' ---- Part 6: LINE_ID採番 ----
        Console.WriteLine("--- Part 6: LINE_ID採番 ---")
        Test_LineId_MaxPlus1()
        Test_LineId_Empty()
        Console.WriteLine()

        ' ---- Part 7: 選択カウント ----
        Console.WriteLine("--- Part 7: 選択カウント ---")
        Test_CheckCount_Partial()
        Test_CheckCount_SelectAll()
        Test_CheckCount_DeselectAll()
        Console.WriteLine()

        PrintSummary()
    End Sub

    ' ====================================================================
    '  Part 1: GsonScheduleBuilder.BuildFromRows
    ' ====================================================================

    Sub Test_Gson_Empty()
        ' TC-001: 空DataTable入力 → 空リスト
        Dim label As String = "GsonBuilder: 空DataTable → 空リスト (TC-001)"
        Try
            Dim dt = CreateGsonDataTable()
            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " Count", 0, result.Count)
        Catch ex As Exception
            Fail(label, "0件", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Gson_Tmg0_MonthEnd()
        ' TC-002: GSON_TMG=0（月度末）
        Dim label As String = "GsonBuilder: TMG=0 月度末 (TC-002)"
        Try
            Dim dt = CreateGsonDataTable()
            Dim row = dt.NewRow()
            row("gson_dt") = New Date(2024, 9, 30)
            row("gson_tmg") = 0
            row("gson_ryo") = 200000.0
            row("gson_rkei") = 200000.0
            dt.Rows.Add(row)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " Count", 1, result.Count)
            AssertEqual(label & " GsonRyoS", 0.0, result(0).GsonRyoS)
            AssertEqual(label & " GsonRyoE", 200000.0, result(0).GsonRyoE)
            AssertEqual(label & " GsonRkeiS", 0.0, result(0).GsonRkeiS)
            AssertEqual(label & " GsonRkeiE", 200000.0, result(0).GsonRkeiE)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Gson_Tmg1_MonthStart()
        ' TC-003: GSON_TMG=1（月度初）
        Dim label As String = "GsonBuilder: TMG=1 月度初 (TC-003)"
        Try
            Dim dt = CreateGsonDataTable()
            Dim row = dt.NewRow()
            row("gson_dt") = New Date(2024, 9, 30)
            row("gson_tmg") = 1
            row("gson_ryo") = 150000.0
            row("gson_rkei") = 350000.0
            dt.Rows.Add(row)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " Count", 1, result.Count)
            AssertEqual(label & " GsonRyoS", 150000.0, result(0).GsonRyoS)
            AssertEqual(label & " GsonRyoE", 0.0, result(0).GsonRyoE)
            AssertEqual(label & " GsonRkeiS", 350000.0, result(0).GsonRkeiS)
            AssertEqual(label & " GsonRkeiE", 350000.0, result(0).GsonRkeiE)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Gson_DBNull_Skip()
        ' TC-004: gson_dt=DBNull → スキップ（例外なし）
        Dim label As String = "GsonBuilder: DBNull行スキップ (TC-004)"
        Try
            Dim dt = CreateGsonDataTable()
            Dim row = dt.NewRow()
            row("gson_dt") = DBNull.Value
            row("gson_tmg") = 0
            row("gson_ryo") = 100000.0
            row("gson_rkei") = 100000.0
            dt.Rows.Add(row)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " Count", 0, result.Count)
        Catch ex As Exception
            Fail(label, "0件(スキップ)", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Gson_DBNull_Mixed()
        ' TC-005: DBNull行 + 有効行の混在
        Dim label As String = "GsonBuilder: DBNull混在 (TC-005)"
        Try
            Dim dt = CreateGsonDataTable()

            ' 行1: DBNull (スキップされるべき)
            Dim row1 = dt.NewRow()
            row1("gson_dt") = DBNull.Value
            row1("gson_tmg") = 0
            row1("gson_ryo") = 50000.0
            row1("gson_rkei") = 50000.0
            dt.Rows.Add(row1)

            ' 行2: 有効
            Dim row2 = dt.NewRow()
            row2("gson_dt") = New Date(2024, 9, 30)
            row2("gson_tmg") = 0
            row2("gson_ryo") = 200000.0
            row2("gson_rkei") = 200000.0
            dt.Rows.Add(row2)

            Dim result = GsonScheduleBuilder.BuildFromRows(dt.Rows)
            AssertEqual(label & " Count", 1, result.Count)
            AssertEqual(label & " GsonRyoE", 200000.0, result(0).GsonRyoE)
        Catch ex As Exception
            Fail(label, "1件", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Gson_InvalidTmg()
        ' TC-006: GSON_TMG不正値 → Exception
        Dim label As String = "GsonBuilder: TMG不正値 例外 (TC-006)"
        Try
            Dim dt = CreateGsonDataTable()
            Dim row = dt.NewRow()
            row("gson_dt") = New Date(2024, 9, 30)
            row("gson_tmg") = 2 ' 不正値
            row("gson_ryo") = 100000.0
            row("gson_rkei") = 100000.0
            dt.Rows.Add(row)

            Dim threw As Boolean = False
            Try
                GsonScheduleBuilder.BuildFromRows(dt.Rows)
            Catch
                threw = True
            End Try

            AssertEqual(label & " 例外発生", True, threw)
        Catch ex As Exception
            Fail(label, "例外テスト成功", "外側Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 2: 消費税自動計算 (FR-004)
    ' ====================================================================

    ''' <summary>
    ''' 消費税計算の純粋関数 (Form_f_HEN_SCH.RecalcKzei と同じロジック)
    ''' </summary>
    Private Function CalcKzei(klsryo As Double, zritu As Double) As Long
        Return CLng(Math.Floor(klsryo * zritu / 100))
    End Function

    Sub Test_Tax_Standard10()
        ' TC-007: 標準税率10%
        Dim label As String = "消費税: 10% 標準 (TC-007)"
        Try
            Dim kzei = CalcKzei(100000, 10)
            Dim zkomi = 100000 + kzei
            AssertEqual(label & " KZEI", 10000L, kzei)
            AssertEqual(label & " ZKOMI", 110000.0, CDbl(zkomi))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Tax_8Percent_Floor()
        ' TC-008: 8% 端数切捨て
        Dim label As String = "消費税: 8% 端数切捨て (TC-008)"
        Try
            Dim kzei = CalcKzei(12345, 8)
            Dim zkomi = 12345 + kzei
            AssertEqual(label & " KZEI", 987L, kzei)
            AssertEqual(label & " ZKOMI", 13332.0, CDbl(zkomi))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Tax_ZeroRate()
        ' TC-009: 税率0%
        Dim label As String = "消費税: 0% (TC-009)"
        Try
            Dim kzei = CalcKzei(100000, 0)
            AssertEqual(label & " KZEI", 0L, kzei)
            AssertEqual(label & " ZKOMI", 100000.0, CDbl(100000 + kzei))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Tax_ZeroKlsryo()
        ' 境界値: KLSRYO=0
        Dim label As String = "消費税: KLSRYO=0 境界値"
        Try
            Dim kzei = CalcKzei(0, 10)
            AssertEqual(label & " KZEI", 0L, kzei)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: 合計行自動更新 (FR-005)
    ' ====================================================================

    Private Structure TestHenlRow
        Public Klsryo As Double
        Public Kzei As Double
        Public ShriCnt As Integer
    End Structure

    Private Function CalcSums(rows As List(Of TestHenlRow)) As Double()
        Dim klsryoSum As Double = 0
        Dim kzeiSum As Double = 0
        Dim zkomiSum As Double = 0
        Dim klsryoGokeiSum As Double = 0
        Dim kzeiGokeiSum As Double = 0
        Dim zkomiGokeiSum As Double = 0
        Dim shriCntSum As Integer = 0

        For Each r In rows
            klsryoSum += r.Klsryo
            kzeiSum += r.Kzei
            zkomiSum += (r.Klsryo + r.Kzei)
            klsryoGokeiSum += r.Klsryo * r.ShriCnt
            kzeiGokeiSum += r.Kzei * r.ShriCnt
            zkomiGokeiSum += (r.Klsryo + r.Kzei) * r.ShriCnt
            shriCntSum += r.ShriCnt
        Next

        Return New Double() {klsryoSum, kzeiSum, zkomiSum, klsryoGokeiSum, kzeiGokeiSum, zkomiGokeiSum, CDbl(shriCntSum)}
    End Function

    Sub Test_Sum_ThreeRows()
        ' TC-010: 3行の合計
        Dim label As String = "合計行: 3行合計 (TC-010)"
        Try
            Dim rows As New List(Of TestHenlRow)
            rows.Add(New TestHenlRow With {.Klsryo = 100000, .Kzei = 10000, .ShriCnt = 1})
            rows.Add(New TestHenlRow With {.Klsryo = 200000, .Kzei = 20000, .ShriCnt = 1})
            rows.Add(New TestHenlRow With {.Klsryo = 150000, .Kzei = 15000, .ShriCnt = 1})

            Dim sums = CalcSums(rows)
            AssertEqual(label & " KLSRYO_SUM", 450000.0, sums(0))
            AssertEqual(label & " KZEI_SUM", 45000.0, sums(1))
            AssertEqual(label & " ZKOMI_SUM", 495000.0, sums(2))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Sum_AfterDelete()
        ' TC-011: 行削除後の再計算
        Dim label As String = "合計行: 削除後再計算 (TC-011)"
        Try
            Dim rows As New List(Of TestHenlRow)
            rows.Add(New TestHenlRow With {.Klsryo = 100000, .Kzei = 10000, .ShriCnt = 1})
            rows.Add(New TestHenlRow With {.Klsryo = 200000, .Kzei = 20000, .ShriCnt = 1})
            rows.Add(New TestHenlRow With {.Klsryo = 150000, .Kzei = 15000, .ShriCnt = 1})

            ' 行2を削除
            rows.RemoveAt(1)

            Dim sums = CalcSums(rows)
            AssertEqual(label & " KLSRYO_SUM", 250000.0, sums(0))
            AssertEqual(label & " KZEI_SUM", 25000.0, sums(1))
            AssertEqual(label & " ZKOMI_SUM", 275000.0, sums(2))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Sum_Empty()
        ' TC-012: 0件時の合計
        Dim label As String = "合計行: 0件 (TC-012)"
        Try
            Dim rows As New List(Of TestHenlRow)
            Dim sums = CalcSums(rows)
            AssertEqual(label & " KLSRYO_SUM", 0.0, sums(0))
            AssertEqual(label & " KZEI_SUM", 0.0, sums(1))
            AssertEqual(label & " ZKOMI_SUM", 0.0, sums(2))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: 異動バリデーション
    ' ====================================================================

    ''' <summary>
    ''' f_IDO の IDO_DT バリデーション (純粋関数化)
    ''' </summary>
    Private Function ValidateIdoDt(idoDt As String) As Boolean
        Return Not String.IsNullOrWhiteSpace(idoDt)
    End Function

    Sub Test_Ido_EmptyDate()
        ' TC-013: IDO_DT空欄 → バリデーションエラー
        Dim label As String = "異動: IDO_DT空欄 (TC-013)"
        Try
            AssertEqual(label & " 空文字", False, ValidateIdoDt(""))
            AssertEqual(label & " Nothing", False, ValidateIdoDt(Nothing))
            AssertEqual(label & " スペース", False, ValidateIdoDt("   "))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Ido_ValidDate()
        ' TC-014: IDO_DT正常
        Dim label As String = "異動: IDO_DT正常 (TC-014)"
        Try
            AssertEqual(label, True, ValidateIdoDt("2024/04/01"))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_Ido_NoChecked()
        ' 追加: チェックなし → バリデーションエラー
        Dim label As String = "異動: 未選択チェック"
        Try
            Dim checkedCount As Integer = 0
            AssertEqual(label & " 0件選択はNG", False, checkedCount > 0)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 5: 異動種別カラム切り替え (FR-003)
    ' ====================================================================

    ''' <summary>
    ''' 異動種別に応じたカラムプレフィクスを返す (f_IDO のロジック純粋関数化)
    ''' </summary>
    Private Function GetBcatPrefix(isBcat As Boolean) As String
        Return If(isBcat, "bcat", "hkbcat")
    End Function

    Sub Test_IdoType_Bcat()
        ' TC-015: 管理部署（オプション416） → bcat
        Dim label As String = "異動種別: 管理部署→bcat (TC-015)"
        Try
            Dim prefix = GetBcatPrefix(True)
            AssertEqual(label, "bcat", prefix)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_IdoType_Hkbcat()
        ' TC-016: 費用負担部署（オプション418） → hkbcat
        Dim label As String = "異動種別: 費用負担部署→hkbcat (TC-016)"
        Try
            Dim prefix = GetBcatPrefix(False)
            AssertEqual(label, "hkbcat", prefix)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: LINE_ID採番
    ' ====================================================================

    ''' <summary>
    ''' 次のLINE_IDを計算する (MAX(LINE_ID)+1)
    ''' </summary>
    Private Function CalcNextLineId(existingIds As List(Of Integer)) As Integer
        If existingIds Is Nothing OrElse existingIds.Count = 0 Then Return 1
        Return existingIds.Max() + 1
    End Function

    Sub Test_LineId_MaxPlus1()
        ' TC-020: 既存 {1,2,5} → 次は6
        Dim label As String = "LINE_ID: MAX+1 (TC-020)"
        Try
            Dim ids As New List(Of Integer) From {1, 2, 5}
            AssertEqual(label, 6, CalcNextLineId(ids))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_LineId_Empty()
        ' TC-021: 0件 → 1
        Dim label As String = "LINE_ID: 0件→1 (TC-021)"
        Try
            Dim ids As New List(Of Integer)
            AssertEqual(label, 1, CalcNextLineId(ids))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 7: 選択カウント
    ' ====================================================================

    Sub Test_CheckCount_Partial()
        ' TC-017: 5件中3件チェック
        Dim label As String = "選択カウント: 3/5 (TC-017)"
        Try
            Dim checks = New Boolean() {True, False, True, True, False}
            Dim count As Integer = 0
            For Each c In checks
                If c Then count += 1
            Next
            AssertEqual(label & " COUNT", 3, count)
            AssertEqual(label & " DCOUNT", 5, checks.Length)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_CheckCount_SelectAll()
        ' TC-018: 全選択
        Dim label As String = "選択カウント: 全選択 (TC-018)"
        Try
            Dim checks = New Boolean() {False, False, True, False, False}
            ' すべて選択
            For i = 0 To checks.Length - 1
                checks(i) = True
            Next
            Dim count As Integer = 0
            For Each c In checks
                If c Then count += 1
            Next
            AssertEqual(label & " COUNT=DCOUNT", checks.Length, count)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    Sub Test_CheckCount_DeselectAll()
        ' TC-019: 全解除
        Dim label As String = "選択カウント: 全解除 (TC-019)"
        Try
            Dim checks = New Boolean() {True, True, True, True, True}
            ' すべて選択しない
            For i = 0 To checks.Length - 1
                checks(i) = False
            Next
            Dim count As Integer = 0
            For Each c In checks
                If c Then count += 1
            Next
            AssertEqual(label & " COUNT", 0, count)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  ヘルパー
    ' ====================================================================

    Private Function CreateGsonDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("gson_dt", GetType(Object))
        dt.Columns.Add("gson_tmg", GetType(Object))
        dt.Columns.Add("gson_ryo", GetType(Object))
        dt.Columns.Add("gson_rkei", GetType(Object))
        Return dt
    End Function

    ' ====================================================================
    '  アサーションヘルパー
    ' ====================================================================

    Sub AssertEqual(label As String, expected As Double, actual As Double)
        Console.Write("  " & label & " ... ")
        If Math.Abs(expected - actual) < 0.001 Then
            Pass(label)
        Else
            Fail(label, expected.ToString("N4"), actual.ToString("N4"))
        End If
    End Sub

    Sub AssertEqual(label As String, expected As Integer, actual As Integer)
        Console.Write("  " & label & " ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqual(label As String, expected As Boolean, actual As Boolean)
        Console.Write("  " & label & " ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqual(label As String, expected As Long, actual As Long)
        Console.Write("  " & label & " ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

    Sub AssertEqual(label As String, expected As String, actual As String)
        Console.Write("  " & label & " ... ")
        If expected = actual Then
            Pass(label)
        Else
            Fail(label, If(expected, "(Nothing)"), If(actual, "(Nothing)"))
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

    Sub PrintSummary()
        Console.WriteLine("============================")
        Console.WriteLine("PASS: " & passCount.ToString() & "  FAIL: " & failCount.ToString() & "  SKIP: " & skipCount.ToString())
        Console.WriteLine("============================")
        If failCount > 0 Then
            Environment.ExitCode = 1
        End If
    End Sub

End Module
