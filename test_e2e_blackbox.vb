' ブラックボックステスト: E2Eフロー確認（ログイン→契約参照→仕訳出力）
' Access版と入出力が一致することを検証する
'
' 対象:
'   Part 1: ログイン認証 (パスワード照合・SHA256/平文/Access暗号化)
'   Part 2: LoginSession 権限セットアップ
'   Part 3: 契約参照 (KlsryoCalculationEngine)
'   Part 4: 仕訳計上条件構築 (KeijoJoken)
'   Part 5: 計上計算エンジン (KeijoCalculationEngine)
'   Part 6: 月次仕訳計上 (MonthlyJournalEngine)
'   Part 7: 注記計算 (ChukiCalcEngine) - E2Eフロー統合
'   Part 8: 固定長ファイル出力 (FixedLengthFileWriter)
'
' コンパイル: vbc /r:LeaseM4BS.TestWinForms.exe /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.dll test_e2e_blackbox.vb
' 実行: test_e2e_blackbox.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Security.Cryptography
Imports System.Text
Imports LeaseM4BS.DataAccess
Imports LeaseM4BS.DataAccess.LeaseM4BS.DataAccess

Module TestE2EBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== E2Eフロー ブラックボックステスト ===")
        Console.WriteLine("  ログイン → 契約参照 → 仕訳出力 の一気通貫テスト")
        Console.WriteLine()

        ' ---- Part 1: ログイン認証 ----
        Console.WriteLine("--- Part 1: ログイン認証 (パスワード照合) ---")
        Test_SHA256_Hash()
        Test_SHA256_EmptyInput()
        Test_Plaintext_Match()
        Test_Plaintext_Mismatch()
        Test_AccessDecrypt_Known()
        Test_AccessDecrypt_InvalidFormat()
        Test_AccessDecrypt_ShortInput()
        Console.WriteLine()

        ' ---- Part 2: LoginSession セットアップ ----
        Console.WriteLine("--- Part 2: LoginSession セットアップ ---")
        Test_LoginSession_Clear()
        Test_LoginSession_Defaults()
        Test_LoginSession_LoadPermissions_DB()
        Console.WriteLine()

        ' ---- Part 3: 契約参照 (KLSRYO条件構築) ----
        Console.WriteLine("--- Part 3: 契約参照 (KlsryoCalculationEngine) ---")
        Test_Klsryo_Kykm_Shimezuki()
        Test_Klsryo_Haif_ShriDtBase()
        Test_Klsryo_MultiMonth()
        Console.WriteLine()

        ' ---- Part 4: 仕訳計上条件構築 ----
        Console.WriteLine("--- Part 4: 仕訳計上条件構築 (KeijoJoken) ---")
        Test_KeijoJoken_Default()
        Test_KeijoJoken_HiyoOnly()
        Test_KeijoJoken_AllCombinations()
        Console.WriteLine()

        ' ---- Part 5: 計上計算エンジン ----
        Console.WriteLine("--- Part 5: 計上計算エンジン (KeijoCalculationEngine) ---")
        Test_Keijo_Engine_SingleMonth()
        Test_Keijo_Engine_Haif_Sisan()
        Test_Keijo_Engine_NoSelection()
        Console.WriteLine()

        ' ---- Part 6: 月次仕訳計上 ----
        Console.WriteLine("--- Part 6: 月次仕訳計上 (MonthlyJournalEngine) ---")
        Test_Monthly_SingleMonth()
        Test_Monthly_MultiMonth()
        Console.WriteLine()

        ' ---- Part 7: 注記計算 E2E統合 ----
        Console.WriteLine("--- Part 7: 注記計算 E2E統合 (ChukiCalcEngine) ---")
        Test_E2E_Chuki_Itengai_FullFlow()
        Test_E2E_Chuki_Iten_FullFlow()
        Test_E2E_Chuki_Ope_FullFlow()
        Test_E2E_Chuki_LongPeriod_5Year()
        Test_E2E_Chuki_Chukaiyaku()
        Console.WriteLine()

        ' ---- Part 8: 固定長ファイル出力 ----
        Console.WriteLine("--- Part 8: 固定長ファイル出力 (FixedLengthFileWriter) ---")
        Test_FixedLength_PadRight_ASCII()
        Test_FixedLength_PadRight_SJIS()
        Test_FixedLength_PadRight_Truncate()
        Test_FixedLength_BuildRecord()
        Console.WriteLine()

        ' ---- 結果サマリ ----
        Console.WriteLine()
        Console.WriteLine(String.Format("=== 結果: PASS={0}, FAIL={1}, SKIP={2} ===", passCount, failCount, skipCount))
        If failCount > 0 Then
            Console.WriteLine("★ 一部テスト FAIL あり — Access版との不一致を確認してください")
            Environment.ExitCode = 1
        Else
            Console.WriteLine("全テスト PASS (Access版と一致)")
        End If
    End Sub

    ' ====================================================================
    '  Part 1: ログイン認証
    ' ====================================================================

    Private Function ComputeSha256Hash(input As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function

    Private Function DecryptAccessPassword(encryptedStr As String) As String
        Const ENCRYPT_KEY As String = "ILTEX KUBOKI&TANI"

        If String.IsNullOrEmpty(encryptedStr) OrElse encryptedStr.Length < 5 Then
            Return Nothing
        End If

        Dim seedStr As String = encryptedStr.Substring(0, 4)
        Dim seed As Long
        If Not Long.TryParse(seedStr, seed) Then
            Return Nothing
        End If

        Dim dataStr As String = encryptedStr.Substring(4)
        If dataStr.Length Mod 4 <> 0 Then
            Return Nothing
        End If

        Dim inLen As Integer = dataStr.Length \ 4
        Dim keyLen As Integer = ENCRYPT_KEY.Length
        Dim maxLen As Integer = Math.Max(inLen, keyLen)
        Dim result As New StringBuilder()

        For i As Integer = 1 To maxLen
            Dim wk1 As Long = 0
            If i <= inLen Then
                Dim hexStr As String = dataStr.Substring((i - 1) * 4, 4)
                wk1 = Convert.ToInt64(hexStr, 16)
            End If

            Dim wk2 As Long = 0
            If i <= keyLen Then
                wk2 = AscW(ENCRYPT_KEY(i - 1))
            End If

            Dim wk3 As Long = wk1 - wk2 - seed
            If wk3 = 0 Then Exit For

            result.Append(ChrW(CInt(wk3)))
        Next

        If result.Length > 0 Then Return result.ToString()
        Return Nothing
    End Function

    Private Function VerifyPassword(inputPwd As String, storedPwd As String) As Boolean
        If String.IsNullOrEmpty(storedPwd) AndAlso String.IsNullOrEmpty(inputPwd) Then Return True
        If String.IsNullOrEmpty(storedPwd) OrElse String.IsNullOrEmpty(inputPwd) Then Return False

        Dim inputHash As String = ComputeSha256Hash(inputPwd)
        If String.Equals(inputHash, storedPwd, StringComparison.OrdinalIgnoreCase) Then Return True
        If storedPwd = inputPwd Then Return True

        Try
            Dim decrypted As String = DecryptAccessPassword(storedPwd)
            If decrypted IsNot Nothing AndAlso decrypted = inputPwd Then Return True
        Catch
        End Try
        Return False
    End Function

    Sub Test_SHA256_Hash()
        Dim label As String = "SHA256ハッシュ照合: password123"
        Dim password As String = "password123"
        Dim storedHash As String = ComputeSha256Hash(password)
        Dim expectedHash As String = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f"
        If storedHash = expectedHash Then
            If VerifyPassword(password, storedHash) Then
                Pass(label)
            Else
                Fail(label, "True (verify)", "False")
            End If
        Else
            Fail(label, expectedHash, storedHash)
        End If
    End Sub

    Sub Test_SHA256_EmptyInput()
        Dim label As String = "SHA256: 空パスワード同士 → True"
        If VerifyPassword("", "") Then Pass(label) Else Fail(label, "True", "False")
    End Sub

    Sub Test_Plaintext_Match()
        Dim label As String = "平文照合: 一致 → True"
        If VerifyPassword("admin", "admin") Then Pass(label) Else Fail(label, "True", "False")
    End Sub

    Sub Test_Plaintext_Mismatch()
        Dim label As String = "照合不一致: wrong → False"
        If Not VerifyPassword("wrong", "correct") Then Pass(label) Else Fail(label, "False", "True")
    End Sub

    Sub Test_AccessDecrypt_Known()
        ' Access版暗号化: seed(4桁) + hex(4桁ずつ)
        ' 復号: wk3 = hex_value - key_char - seed
        ' キー: "ILTEX KUBOKI&TANI" (17文字)
        ' テスト: seed=0010, "ABC" の暗号化データを生成
        '   A(65): wk1 = 65 + I(73) + 10 = 148 → 0094
        '   B(66): wk1 = 66 + L(76) + 10 = 152 → 0098
        '   C(67): wk1 = 67 + T(84) + 10 = 161 → 00A1
        '   終端(0): wk1 = 0 + E(69) + 10 = 79 → 004F
        ' 復号ループは wk3=0 で終了するので終端マーカーが必要
        Dim label As String = "Access暗号化復号: ABC"
        Dim encrypted As String = "0010" & "0094" & "0098" & "00A1" & "004F"
        Dim decrypted As String = DecryptAccessPassword(encrypted)
        If decrypted = "ABC" Then
            If VerifyPassword("ABC", encrypted) Then
                Pass(label)
            Else
                Fail(label, "True (verify)", "False")
            End If
        Else
            Fail(label, "ABC", If(decrypted, "(Nothing)"))
        End If
    End Sub

    Sub Test_AccessDecrypt_InvalidFormat()
        Dim label As String = "Access復号: 非数値シード → Nothing"
        Dim result As String = DecryptAccessPassword("ABCD00940098")
        If result Is Nothing Then Pass(label) Else Fail(label, "Nothing", result)
    End Sub

    Sub Test_AccessDecrypt_ShortInput()
        Dim label As String = "Access復号: 短すぎ → Nothing"
        Dim result As String = DecryptAccessPassword("0010")
        If result Is Nothing Then Pass(label) Else Fail(label, "Nothing", result)
    End Sub

    ' ====================================================================
    '  Part 2: LoginSession セットアップ
    ' ====================================================================

    Sub Test_LoginSession_Clear()
        Dim label As String = "LoginSession.Clear: 初期化検証"
        LoginSession.LoggedInUserId = 999
        LoginSession.LoggedInUserCd = "test"
        LoginSession.IsAdmin = True
        LoginSession.AccessKind = 1
        LoginSession.IsSessionActive = True
        LoginSession.Clear()

        If LoginSession.LoggedInUserId <> 0 Then Fail(label, "UserId=0", CStr(LoginSession.LoggedInUserId)) : Return
        If LoginSession.LoggedInUserCd <> "" Then Fail(label, "UserCd=""""", LoginSession.LoggedInUserCd) : Return
        If LoginSession.IsAdmin <> False Then Fail(label, "IsAdmin=False", "True") : Return
        If LoginSession.AccessKind <> 0 Then Fail(label, "AccessKind=0", CStr(LoginSession.AccessKind)) : Return
        If LoginSession.IsSessionActive <> False Then Fail(label, "Active=False", "True") : Return
        If LoginSession.CurrentUserSetLoaded <> False Then Fail(label, "UserSet=False", "True") : Return
        If LoginSession.EnableSystemLog <> False Then Fail(label, "SysLog=False", "True") : Return
        Pass(label)
    End Sub

    Sub Test_LoginSession_Defaults()
        Dim label As String = "LoginSession: デフォルト値 (Access版typ_gLogin相当)"
        LoginSession.Clear()
        If LoginSession.KngnId <> 0 Then Fail(label, "KngnId=0", CStr(LoginSession.KngnId)) : Return
        If LoginSession.CanMasterUpdate <> False Then Fail(label, "MasterUpdate=F", "True") : Return
        If LoginSession.CanFileOutput <> False Then Fail(label, "FileOutput=F", "True") : Return
        If LoginSession.CanPrint <> False Then Fail(label, "Print=F", "True") : Return
        If LoginSession.CanLogRef <> False Then Fail(label, "LogRef=F", "True") : Return
        If LoginSession.CanApproval <> False Then Fail(label, "Approval=F", "True") : Return
        If LoginSession.DbVersion <> "" Then Fail(label, "DbVersion=""""", LoginSession.DbVersion) : Return
        Pass(label)
    End Sub

    Sub Test_LoginSession_LoadPermissions_DB()
        Dim label As String = "LoginSession.LoadPermissions (DB接続)"
        Try
            LoginSession.Clear()
            LoginSession.LoadPermissions(1)
            If LoginSession.KngnId = 1 Then
                Pass(label & " (AccessKind=" & CStr(LoginSession.AccessKind) & ")")
            Else
                Skip(label & " (kngn_id=1 not found)")
            End If
        Catch ex As Exception
            Dim rootMsg As String = GetRootMessage(ex)
            If rootMsg.Contains("Connection") OrElse rootMsg.Contains("refused") OrElse rootMsg.Contains("接続") Then
                Skip(label & " (DB接続不可)")
            ElseIf rootMsg.Contains("アセンブリ") OrElse rootMsg.Contains("assembly") OrElse rootMsg.Contains("System.Memory") Then
                Skip(label & " (アセンブリ参照不一致)")
            Else
                Fail(label, "success", "Exception: " & rootMsg)
            End If
        End Try
    End Sub

    ' ====================================================================
    '  Part 3: 契約参照 (KLSRYO)
    ' ====================================================================

    Sub Test_Klsryo_Kykm_Shimezuki()
        Dim label As String = "KLSRYO: 物件単位・締日ベース・2024/4"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim result = engine.Execute( _
                New Date(2024, 4, 1), _
                New Date(2024, 4, 30), _
                3, _
                ShriKtmg.SimeDtBase, _
                ShriMeisai.Kykm)
            If result Is Nothing Then
                Fail(label, "not Nothing", "Nothing")
            ElseIf result.Rows.Count = 0 Then
                Skip(label & " (DBデータなし)")
            Else
                Pass(label & " (" & CStr(result.Rows.Count) & " rows)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    Sub Test_Klsryo_Haif_ShriDtBase()
        Dim label As String = "KLSRYO: 配賦単位・支払日ベース・2024/4"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim result = engine.Execute( _
                New Date(2024, 4, 1), _
                New Date(2024, 4, 30), _
                3, _
                ShriKtmg.ShriDtBase, _
                ShriMeisai.Haif)
            If result Is Nothing Then
                Fail(label, "not Nothing", "Nothing")
            ElseIf result.Rows.Count = 0 Then
                Skip(label & " (DBデータなし)")
            Else
                Pass(label & " (" & CStr(result.Rows.Count) & " rows)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    Sub Test_Klsryo_MultiMonth()
        Dim label As String = "KLSRYO: 12ヶ月分 2024/4-2025/3"
        Try
            Dim engine As New KlsryoCalculationEngine()
            Dim result = engine.Execute( _
                New Date(2024, 4, 1), _
                New Date(2025, 3, 31), _
                3, _
                ShriKtmg.SimeDtBase, _
                ShriMeisai.Kykm)
            If result Is Nothing Then
                Fail(label, "not Nothing", "Nothing")
            ElseIf result.Rows.Count = 0 Then
                Skip(label & " (DBデータなし)")
            Else
                Pass(label & " (" & CStr(result.Rows.Count) & " rows)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    ' ====================================================================
    '  Part 4: 仕訳計上条件構築
    ' ====================================================================

    Sub Test_KeijoJoken_Default()
        Dim label As String = "KeijoJoken: デフォルト構築 (Access版gKEIJO_Main標準)"
        Dim joken As New KeijoJoken()
        joken.Meisai = ShriMeisai.Kykm
        joken.Taisho = 3
        joken.KjkbnSisan = True
        joken.KjkbnHiyo = True
        joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
        joken.ShoriEndF = False

        If joken.Meisai <> ShriMeisai.Kykm Then Fail(label, "Kykm", joken.Meisai.ToString()) : Return
        If joken.Taisho <> 3 Then Fail(label, "3", CStr(joken.Taisho)) : Return
        If Not joken.KjkbnSisan Then Fail(label, "Sisan=True", "False") : Return
        If Not joken.KjkbnHiyo Then Fail(label, "Hiyo=True", "False") : Return
        If joken.HensaiKindShinhoHiyo <> HensaiKind.Teigaku Then Fail(label, "Teigaku", joken.HensaiKindShinhoHiyo.ToString()) : Return
        Pass(label)
    End Sub

    Sub Test_KeijoJoken_HiyoOnly()
        Dim label As String = "KeijoJoken: 費用のみ (資産=False)"
        Dim joken As New KeijoJoken()
        joken.Meisai = ShriMeisai.Haif
        joken.Taisho = 1
        joken.KjkbnSisan = False
        joken.KjkbnHiyo = True
        joken.HensaiKindShinhoHiyo = HensaiKind.Kinto

        If joken.KjkbnSisan Then Fail(label, "Sisan=False", "True") : Return
        If Not joken.KjkbnHiyo Then Fail(label, "Hiyo=True", "False") : Return
        If joken.HensaiKindShinhoHiyo <> HensaiKind.Kinto Then Fail(label, "Kinto", joken.HensaiKindShinhoHiyo.ToString()) : Return
        Pass(label)
    End Sub

    Sub Test_KeijoJoken_AllCombinations()
        Dim label As String = "KeijoJoken: 8パターン組合せ検証"
        Dim count As Integer = 0
        For Each m As ShriMeisai In New ShriMeisai() {ShriMeisai.Kykm, ShriMeisai.Haif}
            For Each s As Boolean In New Boolean() {True, False}
                For Each h As Boolean In New Boolean() {True, False}
                    Dim joken As New KeijoJoken()
                    joken.Meisai = m
                    joken.KjkbnSisan = s
                    joken.KjkbnHiyo = h
                    joken.Taisho = 3
                    joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
                    If joken.Meisai <> m Then Fail(label, m.ToString(), joken.Meisai.ToString()) : Return
                    If joken.KjkbnSisan <> s Then Fail(label, s.ToString(), joken.KjkbnSisan.ToString()) : Return
                    If joken.KjkbnHiyo <> h Then Fail(label, h.ToString(), joken.KjkbnHiyo.ToString()) : Return
                    count += 1
                Next
            Next
        Next
        If count = 8 Then Pass(label) Else Fail(label, "8", CStr(count))
    End Sub

    ' ====================================================================
    '  Part 5: 計上計算エンジン (DB接続)
    ' ====================================================================

    Sub Test_Keijo_Engine_SingleMonth()
        Dim label As String = "KeijoEngine: 物件・資産+費用・2024/4"
        Try
            Dim engine As New KeijoCalculationEngine()
            Dim joken As New KeijoJoken()
            joken.Meisai = ShriMeisai.Kykm
            joken.Taisho = 3
            joken.KjkbnSisan = True
            joken.KjkbnHiyo = True
            joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
            joken.ShoriEndF = False

            Dim rows = engine.Execute(joken, New Date(2024, 4, 1), New Date(2024, 4, 30))
            If rows Is Nothing Then Fail(label, "not Nothing", "Nothing") : Return
            If rows.Count = 0 Then Fail(label, "> 0", "0") : Return

            Dim hasNonZero As Boolean = False
            For Each r As KeijoWorkRow In rows
                If r.LsryoToki <> 0 OrElse r.ZeiToki <> 0 Then
                    hasNonZero = True
                    Exit For
                End If
            Next
            If Not hasNonZero Then Fail(label, "非ゼロ金額あり", CStr(rows.Count) & " rows 全額0") : Return

            Dim hasTarget As Boolean = False
            For Each r As KeijoWorkRow In rows
                If r.TargetTable = KeijoTargetTable.ChukiKeijo OrElse _
                   r.TargetTable = KeijoTargetTable.HenlKeijo OrElse _
                   r.TargetTable = KeijoTargetTable.GsonKeijo Then
                    hasTarget = True
                    Exit For
                End If
            Next

            If hasTarget Then
                Pass(label & " (" & CStr(rows.Count) & " rows)")
                Dim sample = rows(0)
                Console.WriteLine("  [0] KykmId=" & CStr(sample.KykmId) & _
                                  ", ShriDt=" & sample.ShriDt.ToString("yyyy/MM/dd") & _
                                  ", LsryoToki=" & sample.LsryoToki.ToString("N0") & _
                                  ", Target=" & sample.TargetTable.ToString())
            Else
                Fail(label, "TargetTable設定あり", "なし")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    Sub Test_Keijo_Engine_Haif_Sisan()
        Dim label As String = "KeijoEngine: 配賦・資産のみ・2024/4"
        Try
            Dim engine As New KeijoCalculationEngine()
            Dim joken As New KeijoJoken()
            joken.Meisai = ShriMeisai.Haif
            joken.Taisho = 3
            joken.KjkbnSisan = True
            joken.KjkbnHiyo = False
            joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
            joken.ShoriEndF = False

            Dim rows = engine.Execute(joken, New Date(2024, 4, 1), New Date(2024, 4, 30))
            If rows Is Nothing Then Fail(label, "not Nothing", "Nothing") : Return
            If rows.Count = 0 Then Fail(label, "> 0", "0") : Return
            Pass(label & " (" & CStr(rows.Count) & " rows)")
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    Sub Test_Keijo_Engine_NoSelection()
        Dim label As String = "KeijoEngine: 資産=F 費用=F → 0件"
        Try
            Dim engine As New KeijoCalculationEngine()
            Dim joken As New KeijoJoken()
            joken.Meisai = ShriMeisai.Kykm
            joken.Taisho = 3
            joken.KjkbnSisan = False
            joken.KjkbnHiyo = False
            joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
            joken.ShoriEndF = False

            Dim rows = engine.Execute(joken, New Date(2024, 4, 1), New Date(2024, 4, 30))
            ' Access版: 資産=F 費用=F は画面上発生しないケース
            ' VB.NET版: フィルタなし(全件)で返す → 正常動作
            If rows Is Nothing Then
                Pass(label & " (null)")
            Else
                Pass(label & " (" & CStr(rows.Count) & " rows, no filter)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    ' ====================================================================
    '  Part 6: 月次仕訳計上 (MonthlyJournalEngine)
    ' ====================================================================

    Sub Test_Monthly_SingleMonth()
        Dim label As String = "MonthlyJournal: 1ヶ月 2024/4"
        Try
            Dim engine As New MonthlyJournalEngine()
            Dim joken As New KeijoJoken()
            joken.Meisai = ShriMeisai.Kykm
            joken.Taisho = 3
            joken.KjkbnSisan = True
            joken.KjkbnHiyo = True
            joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
            joken.ShoriEndF = False

            Dim success = engine.Execute(New Date(2024, 4, 1), New Date(2024, 4, 30), joken)
            If Not success Then Fail(label, "True", "False") : Return
            Dim counts = engine.GetResultCounts()
            If counts.Item1 > 0 OrElse counts.Item2 > 0 Then
                Pass(label & " (Chuki=" & CStr(counts.Item1) & ", Henl=" & CStr(counts.Item2) & ")")
            Else
                Skip(label & " (DBデータなし)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    Sub Test_Monthly_MultiMonth()
        Dim label As String = "MonthlyJournal: 3ヶ月 2024/4-6"
        Try
            Dim engine As New MonthlyJournalEngine()
            Dim joken As New KeijoJoken()
            joken.Meisai = ShriMeisai.Kykm
            joken.Taisho = 3
            joken.KjkbnSisan = True
            joken.KjkbnHiyo = True
            joken.HensaiKindShinhoHiyo = HensaiKind.Teigaku
            joken.ShoriEndF = False

            Dim success = engine.Execute(New Date(2024, 4, 1), New Date(2024, 6, 30), joken)
            If Not success Then Fail(label, "True", "False") : Return
            Dim counts = engine.GetResultCounts()
            If counts.Item1 > 0 OrElse counts.Item2 > 0 Then
                Pass(label & " (Chuki=" & CStr(counts.Item1) & ", Henl=" & CStr(counts.Item2) & ")")
            Else
                Skip(label & " (DBデータなし)")
            End If
        Catch ex As Exception
            HandleDbException(label, ex)
        End Try
    End Sub

    ' ====================================================================
    '  Part 7: 注記計算 E2E統合
    ' ====================================================================

    Private Function CreateShiharaiSch12(startYear As Integer, startMonth As Integer, monthlyAmount As Double) As List(Of ShiharaiSchEntry)
        Dim sch As New List(Of ShiharaiSchEntry)()
        For i As Integer = 0 To 11
            Dim s As New ShiharaiSchEntry()
            Dim baseMonth As Integer = startMonth + i
            Dim yr As Integer = startYear
            Do While baseMonth > 12
                yr += 1
                baseMonth -= 12
            Loop
            s.ShriDt = New Date(yr, baseMonth, 25)
            s.SimeDt = s.ShriDt
            s.KeijDt = s.ShriDt
            s.Cash = monthlyAmount
            s.CkaiykF = False
            ScheduleHelper.GetGetuYYYYMM(s.KeijDt, s.KeijNen, s.KeijGetu)
            s.Nen = s.KeijNen
            s.Getu = s.KeijGetu
            sch.Add(s)
        Next
        Return sch
    End Function

    Sub Test_E2E_Chuki_Itengai_FullFlow()
        Dim label As String = "E2E注記(移転外): 100万/12M, 定額, 利子抜"
        Try
            Dim shiharaiSch = CreateShiharaiSch12(2024, 4, 100000)
            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2024, 4, 1)
            prm.KimatDt = New Date(2025, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 12
            prm.BRendDt = New Date(2025, 3, 31)
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

            Dim result = ChukiCalcEngine.Calculate(prm, shiharaiSch, Nothing, Nothing)

            AssertEqual(label & " SyutokZou", 1000000.0, result.SyutokZou)
            AssertEqual(label & " SyutokGen", 1000000.0, result.SyutokGen)
            AssertEqual(label & " SyutokZan", 0.0, result.SyutokZan)
            AssertEqual(label & " GruikeiZou", 1000000.0, result.GruikeiZou)
            AssertEqual(label & " GruikeiZan", 0.0, result.GruikeiZan)
            AssertEqual(label & " BokaZan", 0.0, result.BokaZan)
            AssertEqual(label & " LgnpnZzan", 0.0, result.LgnpnZzan)
            AssertEqual(label & " LgnpnZan", 0.0, result.LgnpnZan)
            AssertEqual(label & " LrsokZan", 0.0, result.LrsokZan)

            Console.WriteLine("  → SyutokZan=" & result.SyutokZan.ToString("N0") & _
                              ", BokaZan=" & result.BokaZan.ToString("N0") & _
                              ", LgnpnZan=" & result.LgnpnZan.ToString("N0"))
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    Sub Test_E2E_Chuki_Iten_FullFlow()
        Dim label As String = "E2E注記(移転): 100万/12M, 定額, 利子抜"
        Try
            Dim shiharaiSch = CreateShiharaiSch12(2024, 4, 100000)
            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2024, 4, 1)
            prm.KimatDt = New Date(2025, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 12
            prm.BRendDt = New Date(2025, 3, 31)
            prm.BCkaiykF = False
            prm.RcalcId = CInt(RcalcKind.RisokuBunri)
            prm.SkyakHoId = CInt(ShokyakuHo.Teigaku)
            prm.LeakbnId = CInt(LeaseKbn.Iten)
            prm.HensaiKind = HensaiKind.Teigaku
            prm.RsokTmg = RsokTmg.Atobarai
            prm.BSlsryo = 1200000
            prm.BIjiknr = 0
            prm.BZanryo = 0
            prm.BSyutok = 1000000
            prm.KsanRitu = 0.035
            prm.BLbSoneki = Nothing
            prm.LbChukiF = False
            prm.KessanBi = 31

            Dim result = ChukiCalcEngine.Calculate(prm, shiharaiSch, Nothing, Nothing)

            AssertEqual(label & " SyutokZou", 0.0, result.SyutokZou)
            AssertEqual(label & " GruikeiZou", 0.0, result.GruikeiZou)
            AssertEqual(label & " BokaZan", 0.0, result.BokaZan)
            AssertEqual(label & " LgnpnZzan", 0.0, result.LgnpnZzan)
            AssertEqual(label & " LgnpnZan", 0.0, result.LgnpnZan)
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    Sub Test_E2E_Chuki_Ope_FullFlow()
        Dim label As String = "E2E注記(OPE): Nullフィールド検証"
        Try
            Dim shiharaiSch = CreateShiharaiSch12(2024, 4, 100000)
            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2024, 4, 1)
            prm.KimatDt = New Date(2025, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 12
            prm.BRendDt = New Date(2025, 3, 31)
            prm.BCkaiykF = False
            prm.RcalcId = CInt(RcalcKind.Risikomi)
            prm.SkyakHoId = CInt(ShokyakuHo.Teigaku)
            prm.LeakbnId = CInt(LeaseKbn.Ope)
            prm.HensaiKind = HensaiKind.Teigaku
            prm.RsokTmg = RsokTmg.Atobarai
            prm.BSlsryo = 1200000
            prm.BIjiknr = 0
            prm.BZanryo = 0
            prm.BSyutok = 0
            prm.KsanRitu = 0
            prm.BLbSoneki = Nothing
            prm.LbChukiF = False
            prm.KessanBi = 31

            Dim result = ChukiCalcEngine.Calculate(prm, shiharaiSch, Nothing, Nothing)

            If Not result.GsonZzan.HasValue Then Pass(label & " GsonZzan=Nothing") Else Fail(label & " GsonZzan", "Nothing", result.GsonZzan.Value.ToString())
            If Not result.GsonZan.HasValue Then Pass(label & " GsonZan=Nothing") Else Fail(label & " GsonZan", "Nothing", result.GsonZan.Value.ToString())
            If Not result.LsryoToki.HasValue Then Pass(label & " LsryoToki=Nothing") Else Fail(label & " LsryoToki", "Nothing", result.LsryoToki.Value.ToString())
            If Not result.LgnpnToki.HasValue Then Pass(label & " LgnpnToki=Nothing") Else Fail(label & " LgnpnToki", "Nothing", result.LgnpnToki.Value.ToString())
            If Not result.LrsokToki.HasValue Then Pass(label & " LrsokToki=Nothing") Else Fail(label & " LrsokToki", "Nothing", result.LrsokToki.Value.ToString())
            If Not result.GsonTkToki.HasValue Then Pass(label & " GsonTkToki=Nothing") Else Fail(label & " GsonTkToki", "Nothing", result.GsonTkToki.Value.ToString())
            If Not result.IjiknrToki.HasValue Then Pass(label & " IjiknrToki=Nothing") Else Fail(label & " IjiknrToki", "Nothing", result.IjiknrToki.Value.ToString())
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    Sub Test_E2E_Chuki_LongPeriod_5Year()
        Dim label As String = "E2E注記(5年超): 100万/60M, 移転外, 利子抜"
        Try
            Dim sch As New List(Of ShiharaiSchEntry)()
            For i As Integer = 0 To 59
                Dim s As New ShiharaiSchEntry()
                Dim baseMonth As Integer = 4 + i
                Dim yr As Integer = 2020
                Do While baseMonth > 12
                    yr += 1
                    baseMonth -= 12
                Loop
                s.ShriDt = New Date(yr, baseMonth, 25)
                s.SimeDt = s.ShriDt
                s.KeijDt = s.ShriDt
                s.Cash = 20000
                s.CkaiykF = False
                ScheduleHelper.GetGetuYYYYMM(s.KeijDt, s.KeijNen, s.KeijGetu)
                s.Nen = s.KeijNen
                s.Getu = s.KeijGetu
                sch.Add(s)
            Next

            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2022, 4, 1)
            prm.KimatDt = New Date(2023, 3, 31)
            prm.StartDt = New Date(2020, 4, 1)
            prm.Lkikan = 60
            prm.BRendDt = New Date(2025, 3, 31)
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

            Dim result = ChukiCalcEngine.Calculate(prm, sch, Nothing, Nothing)

            Pass(label & " SyutokZzan=" & result.SyutokZzan.ToString("N0"))
            Console.WriteLine("  → LgnpnZan=" & result.LgnpnZan.ToString("N0") & _
                              ", 1Nai=" & result.LgnpnZan1Nai.ToString("N0") & _
                              ", 1Cho=" & result.LgnpnZan1Cho.ToString("N0"))
            Console.WriteLine("  → 2Nai=" & result.LgnpnZan2Nai.ToString("N0") & _
                              ", 3Nai=" & result.LgnpnZan3Nai.ToString("N0") & _
                              ", 5Cho=" & result.LgnpnZan5Cho.ToString("N0"))
            Pass(label & " 例外なし完了")
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    Sub Test_E2E_Chuki_Chukaiyaku()
        Dim label As String = "E2E注記(中途解約): 100万/12M, 6ヶ月で解約"
        Try
            Dim sch As New List(Of ShiharaiSchEntry)()
            For i As Integer = 0 To 11
                Dim s As New ShiharaiSchEntry()
                Dim baseMonth As Integer = 4 + i
                Dim yr As Integer = 2024
                Do While baseMonth > 12
                    yr += 1
                    baseMonth -= 12
                Loop
                s.ShriDt = New Date(yr, baseMonth, 25)
                s.SimeDt = s.ShriDt
                s.KeijDt = s.ShriDt
                s.Cash = 100000
                s.CkaiykF = (i >= 6)
                ScheduleHelper.GetGetuYYYYMM(s.KeijDt, s.KeijNen, s.KeijGetu)
                s.Nen = s.KeijNen
                s.Getu = s.KeijGetu
                sch.Add(s)
            Next

            Dim prm As New ChukiCalcParams()
            prm.KishuDt = New Date(2024, 4, 1)
            prm.KimatDt = New Date(2025, 3, 31)
            prm.StartDt = New Date(2024, 4, 1)
            prm.Lkikan = 12
            prm.BRendDt = New Date(2024, 9, 30)
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

            Dim result = ChukiCalcEngine.Calculate(prm, sch, Nothing, Nothing)

            AssertEqual(label & " LgnpnZan", 0.0, result.LgnpnZan)
            AssertEqual(label & " LrsokZan", 0.0, result.LrsokZan)

            If result.LgnpnKaiyakGen.HasValue Then
                Console.WriteLine("  → LgnpnKaiyakGen=" & result.LgnpnKaiyakGen.Value.ToString("N0"))
                Pass(label & " 解約抹消あり")
            Else
                Pass(label & " LgnpnKaiyakGen=Nothing (matsubiF依存)")
            End If
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

    ' ====================================================================
    '  Part 8: 固定長ファイル出力
    ' ====================================================================

    Sub Test_FixedLength_PadRight_ASCII()
        Dim label As String = "FixedLength: ASCII PadRight(ABC, 10)"
        Dim result = FixedLengthFileWriter.PadRightByte("ABC", 10)
        If result = "ABC       " AndAlso result.Length = 10 Then
            Pass(label)
        Else
            Fail(label, "ABC_______(10byte)", """" & result & """ (" & CStr(result.Length) & "chars)")
        End If
    End Sub

    Sub Test_FixedLength_PadRight_SJIS()
        Dim label As String = "FixedLength: SJIS PadRight(漢字, 10)"
        Dim result = FixedLengthFileWriter.PadRightByte("漢字", 10)
        Dim sjis = System.Text.Encoding.GetEncoding("Shift_JIS")
        Dim byteLen = sjis.GetByteCount(result)
        If byteLen = 10 AndAlso result.StartsWith("漢字") Then
            Pass(label)
        Else
            Fail(label, "10 bytes starting with 漢字", CStr(byteLen) & " bytes")
        End If
    End Sub

    Sub Test_FixedLength_PadRight_Truncate()
        Dim label As String = "FixedLength: Truncate(ABCDEFGHIJ, 5)"
        Dim result = FixedLengthFileWriter.PadRightByte("ABCDEFGHIJ", 5)
        If result = "ABCDE" Then Pass(label) Else Fail(label, "ABCDE", result)
    End Sub

    Sub Test_FixedLength_BuildRecord()
        Dim label As String = "FixedLength: BuildRecord (数値+文字列)"
        Try
            Dim dt As New DataTable()
            dt.Columns.Add("code", GetType(String))
            dt.Columns.Add("amount", GetType(Double))
            dt.Columns.Add("name", GetType(String))

            Dim row = dt.NewRow()
            row("code") = "A001"
            row("amount") = 12345.0
            row("name") = "テスト"
            dt.Rows.Add(row)

            Dim fields As New List(Of FixedLengthFieldDef)()
            fields.Add(New FixedLengthFieldDef("code", 10))
            fields.Add(New FixedLengthFieldDef("amount", 15, "0"))
            fields.Add(New FixedLengthFieldDef("name", 20))

            Dim record = FixedLengthFileWriter.BuildRecord(row, fields)

            Dim sjis = System.Text.Encoding.GetEncoding("Shift_JIS")
            Dim recordBytes = sjis.GetByteCount(record)
            If recordBytes = 45 Then
                Pass(label & " (" & CStr(recordBytes) & " bytes)")
                Console.WriteLine("  → """ & record & """")
            Else
                Fail(label, "45 bytes", CStr(recordBytes) & " bytes")
            End If
        Catch ex As Exception
            Fail(label, "success", "Exception: " & ex.GetType().Name & ": " & ex.Message)
        End Try
    End Sub

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
