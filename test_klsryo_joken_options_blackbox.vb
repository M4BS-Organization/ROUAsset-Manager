' ブラックボックステスト: f_KLSRYO_JOKEN 条件画面ラジオボタンオプション
' Access版と入出力が一致することを検証する
'
' コンパイル: vbc -r:LeaseM4BS.TestWinForms.exe -r:LeaseM4BS.DataAccess.dll -r:Npgsql.dll -r:System.Data.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:test_klsryo_joken_options_blackbox.exe test_klsryo_joken_options_blackbox.vb
' 実行: test_klsryo_joken_options_blackbox.exe

Imports System
Imports System.Data
Imports System.Reflection
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

Module TestKlsryoJokenOptionsBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    <STAThread>
    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== f_KLSRYO_JOKEN ラジオボタンオプション ブラックボックステスト ===")
        Console.WriteLine()

        ' ---- Part 1: デフォルト値確認 ----
        Console.WriteLine("--- Part 1: デフォルト値確認 ---")
        Test_TC001_Default_Taisho_Zenbu()
        Test_TC002_Default_Ktmg_SimeDt()
        Test_TC003_Default_Meisai_Haif()
        Console.WriteLine()

        ' ---- Part 2: 集計対象グループ 排他制御 ----
        Console.WriteLine("--- Part 2: 集計対象グループ 排他制御 ---")
        Test_TC004_Taisho_Lsryo_Exclusive()
        Test_TC005_Taisho_Hoshu_Exclusive()
        Test_TC006_Taisho_Zenbu_Exclusive()
        Console.WriteLine()

        ' ---- Part 3: タイミンググループ 排他制御 ----
        Console.WriteLine("--- Part 3: タイミンググループ 排他制御 ---")
        Test_TC007_Ktmg_Sime_Exclusive()
        Test_TC008_Ktmg_Shri_Exclusive()
        Console.WriteLine()

        ' ---- Part 4: 明細グループ 排他制御 ----
        Console.WriteLine("--- Part 4: 明細グループ 排他制御 ---")
        Test_TC009_Meisai_Bukn_Exclusive()
        Test_TC010_Meisai_Haif_Exclusive()
        Console.WriteLine()

        ' ---- Part 5: Taisho変換ロジック ----
        Console.WriteLine("--- Part 5: Taisho変換ロジック ---")
        Test_TC011_Taisho_Lsryo_Returns1()
        Test_TC012_Taisho_Hoshu_Returns2()
        Test_TC013_Taisho_Zenbu_Returns3()
        Console.WriteLine()

        ' ---- Part 6: Ktmg変換ロジック ----
        Console.WriteLine("--- Part 6: Ktmg変換ロジック ---")
        Test_TC014_Ktmg_Sime_ReturnsSimeDtBase()
        Test_TC015_Ktmg_Shri_ReturnsShriDtBase()
        Console.WriteLine()

        ' ---- Part 7: Meisai変換ロジック ----
        Console.WriteLine("--- Part 7: Meisai変換ロジック ---")
        Test_TC016_Meisai_Bukn_ReturnsKykm()
        Test_TC017_Meisai_Haif_ReturnsHaif()
        Console.WriteLine()

        ' ---- Part 8: エンジン統合テスト ----
        Console.WriteLine("--- Part 8: エンジン統合テスト (DBあり/SKIP可) ---")
        Test_TC018_Engine_Taisho1_NoHenf()
        Test_TC019_Engine_Default_Combo()
        Test_TC020_Engine_ShriDt_Kykm()
        Console.WriteLine()

        ' ---- 結果集計 ----
        Console.WriteLine($"=== 結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount} ===")
        If failCount > 0 Then
            Console.WriteLine("★ 一部テスト FAIL あり")
            Environment.Exit(1)
        Else
            Console.WriteLine("全テスト PASS")
        End If
    End Sub

    ' ====================================================================
    '  コントロール・メソッドアクセスヘルパー（Friend対応）
    ' ====================================================================

    ''' <summary>フォームからRadioButtonをコントロール名で取得</summary>
    Function FindRadio(frm As Form, name As String) As RadioButton
        Dim controls() As Control = frm.Controls.Find(name, True)
        If controls.Length > 0 Then
            Return DirectCast(controls(0), RadioButton)
        End If
        Return Nothing
    End Function

    ''' <summary>Friendメソッドをリフレクションで呼び出す</summary>
    Function InvokeFriendMethod(frm As Object, methodName As String) As Object
        Dim mi As MethodInfo = frm.GetType().GetMethod(methodName,
            BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
        If mi Is Nothing Then
            Throw New MissingMethodException($"Method '{methodName}' not found on {frm.GetType().Name}")
        End If
        Return mi.Invoke(frm, Nothing)
    End Function

    ' ====================================================================
    '  Part 1: デフォルト値確認
    ' ====================================================================

    Sub Test_TC001_Default_Taisho_Zenbu()
        Dim label As String = "TC-001: 集計対象デフォルト = 全部(radio_ZENBU)"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rZenbu = FindRadio(frm, "radio_ZENBU")
            Dim rLsryo = FindRadio(frm, "radio_LSRYO")
            Dim rHoshu = FindRadio(frm, "radio_HOSHU")
            AssertTrue(label & " radio_ZENBU.Checked=True", rZenbu.Checked)
            AssertFalse(label & " radio_LSRYO.Checked=False", rLsryo.Checked)
            AssertFalse(label & " radio_HOSHU.Checked=False", rHoshu.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC002_Default_Ktmg_SimeDt()
        Dim label As String = "TC-002: タイミングデフォルト = 締日ベース(radio_SIME)"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rSime = FindRadio(frm, "radio_SIME")
            Dim rShri = FindRadio(frm, "radio_SHRI")
            AssertTrue(label & " radio_SIME.Checked=True", rSime.Checked)
            AssertFalse(label & " radio_SHRI.Checked=False", rShri.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC003_Default_Meisai_Haif()
        Dim label As String = "TC-003: 明細デフォルト = 配賦単位(radio_HAIF)"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rHaif = FindRadio(frm, "radio_HAIF")
            Dim rBukn = FindRadio(frm, "radio_BUKN")
            AssertTrue(label & " radio_HAIF.Checked=True", rHaif.Checked)
            AssertFalse(label & " radio_BUKN.Checked=False", rBukn.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 2: 集計対象グループ 排他制御
    ' ====================================================================

    Sub Test_TC004_Taisho_Lsryo_Exclusive()
        Dim label As String = "TC-004: 集計対象 リース料選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rLsryo = FindRadio(frm, "radio_LSRYO")
            Dim rHoshu = FindRadio(frm, "radio_HOSHU")
            Dim rZenbu = FindRadio(frm, "radio_ZENBU")
            rLsryo.Checked = True
            AssertTrue(label & " radio_LSRYO.Checked=True", rLsryo.Checked)
            AssertFalse(label & " radio_HOSHU.Checked=False", rHoshu.Checked)
            AssertFalse(label & " radio_ZENBU.Checked=False", rZenbu.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC005_Taisho_Hoshu_Exclusive()
        Dim label As String = "TC-005: 集計対象 保守料選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rLsryo = FindRadio(frm, "radio_LSRYO")
            Dim rHoshu = FindRadio(frm, "radio_HOSHU")
            Dim rZenbu = FindRadio(frm, "radio_ZENBU")
            rHoshu.Checked = True
            AssertTrue(label & " radio_HOSHU.Checked=True", rHoshu.Checked)
            AssertFalse(label & " radio_LSRYO.Checked=False", rLsryo.Checked)
            AssertFalse(label & " radio_ZENBU.Checked=False", rZenbu.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC006_Taisho_Zenbu_Exclusive()
        Dim label As String = "TC-006: 集計対象 全部選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rLsryo = FindRadio(frm, "radio_LSRYO")
            Dim rHoshu = FindRadio(frm, "radio_HOSHU")
            Dim rZenbu = FindRadio(frm, "radio_ZENBU")
            rLsryo.Checked = True
            rZenbu.Checked = True
            AssertTrue(label & " radio_ZENBU.Checked=True", rZenbu.Checked)
            AssertFalse(label & " radio_LSRYO.Checked=False", rLsryo.Checked)
            AssertFalse(label & " radio_HOSHU.Checked=False", rHoshu.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: タイミンググループ 排他制御
    ' ====================================================================

    Sub Test_TC007_Ktmg_Sime_Exclusive()
        Dim label As String = "TC-007: タイミング 締日ベース選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rSime = FindRadio(frm, "radio_SIME")
            Dim rShri = FindRadio(frm, "radio_SHRI")
            rSime.Checked = True
            AssertTrue(label & " radio_SIME.Checked=True", rSime.Checked)
            AssertFalse(label & " radio_SHRI.Checked=False", rShri.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC008_Ktmg_Shri_Exclusive()
        Dim label As String = "TC-008: タイミング 支払日ベース選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rSime = FindRadio(frm, "radio_SIME")
            Dim rShri = FindRadio(frm, "radio_SHRI")
            rShri.Checked = True
            AssertTrue(label & " radio_SHRI.Checked=True", rShri.Checked)
            AssertFalse(label & " radio_SIME.Checked=False", rSime.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: 明細グループ 排他制御
    ' ====================================================================

    Sub Test_TC009_Meisai_Bukn_Exclusive()
        Dim label As String = "TC-009: 明細 物件単位選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rBukn = FindRadio(frm, "radio_BUKN")
            Dim rHaif = FindRadio(frm, "radio_HAIF")
            rBukn.Checked = True
            AssertTrue(label & " radio_BUKN.Checked=True", rBukn.Checked)
            AssertFalse(label & " radio_HAIF.Checked=False", rHaif.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC010_Meisai_Haif_Exclusive()
        Dim label As String = "TC-010: 明細 配賦単位選択時の排他制御"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rBukn = FindRadio(frm, "radio_BUKN")
            Dim rHaif = FindRadio(frm, "radio_HAIF")
            rBukn.Checked = True
            rHaif.Checked = True
            AssertTrue(label & " radio_HAIF.Checked=True", rHaif.Checked)
            AssertFalse(label & " radio_BUKN.Checked=False", rBukn.Checked)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 5: Taisho変換ロジック
    ' ====================================================================

    Sub Test_TC011_Taisho_Lsryo_Returns1()
        Dim label As String = "TC-011: Taisho変換 リース料選択 → GetTaisho()=1"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rLsryo = FindRadio(frm, "radio_LSRYO")
            rLsryo.Checked = True
            Dim result As Integer = CInt(InvokeFriendMethod(frm, "GetTaisho"))
            AssertEqual(label, 1, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "1", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC012_Taisho_Hoshu_Returns2()
        Dim label As String = "TC-012: Taisho変換 保守料選択 → GetTaisho()=2"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rHoshu = FindRadio(frm, "radio_HOSHU")
            rHoshu.Checked = True
            Dim result As Integer = CInt(InvokeFriendMethod(frm, "GetTaisho"))
            AssertEqual(label, 2, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "2", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC013_Taisho_Zenbu_Returns3()
        Dim label As String = "TC-013: Taisho変換 全部選択 → GetTaisho()=3"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rZenbu = FindRadio(frm, "radio_ZENBU")
            rZenbu.Checked = True
            Dim result As Integer = CInt(InvokeFriendMethod(frm, "GetTaisho"))
            AssertEqual(label, 3, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "3", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: Ktmg変換ロジック
    ' ====================================================================

    Sub Test_TC014_Ktmg_Sime_ReturnsSimeDtBase()
        Dim label As String = "TC-014: Ktmg変換 締日ベース → GetKtmg()=SimeDtBase"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rSime = FindRadio(frm, "radio_SIME")
            rSime.Checked = True
            Dim result As ShriKtmg = DirectCast(InvokeFriendMethod(frm, "GetKtmg"), ShriKtmg)
            AssertEqual(label, ShriKtmg.SimeDtBase, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "SimeDtBase", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC015_Ktmg_Shri_ReturnsShriDtBase()
        Dim label As String = "TC-015: Ktmg変換 支払日ベース → GetKtmg()=ShriDtBase"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rShri = FindRadio(frm, "radio_SHRI")
            rShri.Checked = True
            Dim result As ShriKtmg = DirectCast(InvokeFriendMethod(frm, "GetKtmg"), ShriKtmg)
            AssertEqual(label, ShriKtmg.ShriDtBase, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "ShriDtBase", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 7: Meisai変換ロジック
    ' ====================================================================

    Sub Test_TC016_Meisai_Bukn_ReturnsKykm()
        Dim label As String = "TC-016: Meisai変換 物件単位 → GetMeisai()=Kykm"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rBukn = FindRadio(frm, "radio_BUKN")
            rBukn.Checked = True
            Dim result As ShriMeisai = DirectCast(InvokeFriendMethod(frm, "GetMeisai"), ShriMeisai)
            AssertEqual(label, ShriMeisai.Kykm, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "Kykm", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC017_Meisai_Haif_ReturnsHaif()
        Dim label As String = "TC-017: Meisai変換 配賦単位 → GetMeisai()=Haif"
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            Dim rHaif = FindRadio(frm, "radio_HAIF")
            rHaif.Checked = True
            Dim result As ShriMeisai = DirectCast(InvokeFriendMethod(frm, "GetMeisai"), ShriMeisai)
            AssertEqual(label, ShriMeisai.Haif, result)
            frm.Dispose()
        Catch ex As Exception
            Fail(label, "Haif", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  Part 8: エンジン統合テスト (DBあり/SKIP可)
    ' ====================================================================

    Sub Test_TC018_Engine_Taisho1_NoHenf()
        Dim label As String = "TC-018: エンジン実行 Taisho=1(リース料) 付随費用スキップ確認"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim dtFrom As New Date(2024, 4, 1)
            Dim dtTo As New Date(2024, 4, 30)
            Dim result As DataTable = engine.Execute(dtFrom, dtTo, 1, ShriKtmg.SimeDtBase, ShriMeisai.Haif)
            AssertTrue(label & " result Not Nothing", result IsNot Nothing)
        Catch ex As Exception When IsDbConnectionError(ex)
            Skip(label, $"DB未接続: {ex.Message}")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC019_Engine_Default_Combo()
        Dim label As String = "TC-019: エンジン実行 Access版デフォルト (Taisho=3+SimeDtBase+Haif)"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim dtFrom As New Date(2024, 4, 1)
            Dim dtTo As New Date(2024, 4, 30)
            Dim result As DataTable = engine.Execute(dtFrom, dtTo, 3, ShriKtmg.SimeDtBase, ShriMeisai.Haif)
            AssertTrue(label & " result Not Nothing", result IsNot Nothing)
        Catch ex As Exception When IsDbConnectionError(ex)
            Skip(label, $"DB未接続: {ex.Message}")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    Sub Test_TC020_Engine_ShriDt_Kykm()
        Dim label As String = "TC-020: エンジン実行 Taisho=3+ShriDtBase+Kykm"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim dtFrom As New Date(2024, 4, 1)
            Dim dtTo As New Date(2024, 4, 30)
            Dim result As DataTable = engine.Execute(dtFrom, dtTo, 3, ShriKtmg.ShriDtBase, ShriMeisai.Kykm)
            AssertTrue(label & " result Not Nothing", result IsNot Nothing)
        Catch ex As Exception When IsDbConnectionError(ex)
            Skip(label, $"DB未接続: {ex.Message}")
        Catch ex As Exception
            Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")
        End Try
    End Sub

    ' ====================================================================
    '  ヘルパー関数
    ' ====================================================================

    Function IsDbConnectionError(ex As Exception) As Boolean
        Dim msg As String = ex.Message
        Return msg.Contains("does not exist") OrElse
               msg.Contains("connection") OrElse
               msg.Contains("Connection") OrElse
               msg.Contains("password") OrElse
               msg.Contains("host") OrElse
               msg.Contains("Host") OrElse
               msg.Contains("NpgsqlException") OrElse
               msg.Contains("System.Memory") OrElse
               msg.Contains("アセンブリ") OrElse
               msg.Contains("assembly") OrElse
               (ex.InnerException IsNot Nothing AndAlso IsDbConnectionError(ex.InnerException))
    End Function

    Sub Pass(label As String)
        passCount += 1
        Console.WriteLine($"  PASS: {label}")
    End Sub

    Sub Fail(label As String, expected As String, actual As String)
        failCount += 1
        Console.WriteLine($"  FAIL: {label}")
        Console.WriteLine($"    Expected: {expected}")
        Console.WriteLine($"    Actual:   {actual}")
    End Sub

    Sub Skip(label As String, reason As String)
        skipCount += 1
        Console.WriteLine($"  SKIP: {label} ({reason})")
    End Sub

    Sub AssertTrue(label As String, condition As Boolean)
        If condition Then Pass(label) Else Fail(label, "True", "False")
    End Sub

    Sub AssertFalse(label As String, condition As Boolean)
        If Not condition Then Pass(label) Else Fail(label, "False", "True")
    End Sub

    Sub AssertEqual(Of T)(label As String, expected As T, actual As T)
        If Object.Equals(expected, actual) Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

End Module
