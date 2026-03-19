' ============================================================
' ブラックボックステスト: tw_ ワークテーブルスキーマ検証
' Issue #35: PostgreSQLワークテーブル（tw_系）スキーマ設計と管理戦略
'
' テスト内容:
'   TC-TW-001: 全tw_テーブル存在確認
'   TC-TW-002: tw_s_chuki_calc CRUD
'   TC-TW-003: 条件ワークテーブル CRUD
'   TC-TW-004: 仕訳出力ワークテーブル CRUD
'   TC-TW-005: カラム数検証
'   TC-TW-006: インデックス存在確認
'
' コンパイル:
'   vbc /out:test_tw_schema_blackbox.exe /r:Npgsql.dll test_tw_schema_blackbox.vb
' ============================================================

Imports System
Imports System.Data
Imports Npgsql

Module TestTwSchemaBlackbox

    Private _connStr As String = ""
    Private _passCount As Integer = 0
    Private _failCount As Integer = 0

    Sub Main()
        Console.WriteLine("=== tw_ ワークテーブルスキーマ ブラックボックステスト (Issue #35) ===")
        Console.WriteLine()

        ' 接続文字列取得
        _connStr = Environment.GetEnvironmentVariable("LEASE_M4BS_CONNECTION_STRING")
        If String.IsNullOrEmpty(_connStr) Then
            _connStr = "Host=localhost;Port=5432;Database=lease_m4bs;Username=lease_m4bs_user;Password=iltex_mega_pass_m4"
        End If

        Try
            ' 接続テスト
            Using conn As New NpgsqlConnection(_connStr)
                conn.Open()
                Console.WriteLine("[OK] PostgreSQL接続成功")
                Console.WriteLine()
            End Using

            ' テスト実行
            Test_TC_TW_001_TableExists()
            Test_TC_TW_002_ChukiCalcCrud()
            Test_TC_TW_003_JokenCrud()
            Test_TC_TW_004_ShiwakeOutputCrud()
            Test_TC_TW_005_ColumnCount()
            Test_TC_TW_006_IndexExists()

        Catch ex As Exception
            Console.WriteLine("[FATAL] テスト実行エラー: " & ex.Message)
            _failCount += 1
        End Try

        ' 結果サマリー
        Console.WriteLine()
        Console.WriteLine("=== テスト結果サマリー ===")
        Console.WriteLine("PASS: " & _passCount)
        Console.WriteLine("FAIL: " & _failCount)
        Console.WriteLine("合計: " & (_passCount + _failCount))

        If _failCount > 0 Then
            Environment.ExitCode = 1
        End If
    End Sub

    ' ================================================================
    '  TC-TW-001: 全tw_テーブル存在確認
    ' ================================================================
    Sub Test_TC_TW_001_TableExists()
        Console.WriteLine("--- TC-TW-001: 全tw_テーブル存在確認 ---")

        Dim tables() As String = {
            "tw_s_chuki_keijo",
            "tw_d_henl_keijo",
            "tw_d_gson_keijo",
            "tw_s_chuki_calc",
            "tw_s_keijo_joken",
            "tw_s_tougetsu_joken",
            "tw_s_saimu_joken",
            "tw_kitoku_cmsw2wrk",
            "tw_kitoku_apgdhwrk",
            "tw_kitoku_apgddwrk",
            "tw_kitoku_apgdswrk",
            "tw_f_仕訳出力標準_設定_swksh",
            "tw_f_仕訳出力標準_設定_swkkj",
            "tw_f_仕訳出力標準_設定_swksm",
            "tw_f_仕訳出力標準_設定_swkky",
            "tw_f_仕訳出力標準_kj",
            "tw_f_仕訳出力標準_kj_仕訳data",
            "tw_f_仕訳出力標準_sh",
            "tw_f_仕訳出力標準_sh_仕訳data",
            "tw_f_仕訳出力標準_sm",
            "tw_f_仕訳出力標準_sm_仕訳data"
        }

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            For Each tbl As String In tables
                Dim sql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public' AND table_name = @name"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@name", tbl)
                    Dim cnt = CInt(cmd.ExecuteScalar())
                    If cnt > 0 Then
                        Console.WriteLine("  [PASS] " & tbl & " 存在確認OK")
                        _passCount += 1
                    Else
                        Console.WriteLine("  [FAIL] " & tbl & " が見つかりません")
                        _failCount += 1
                    End If
                End Using
            Next
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  TC-TW-002: tw_s_chuki_calc CRUD
    ' ================================================================
    Sub Test_TC_TW_002_ChukiCalcCrud()
        Console.WriteLine("--- TC-TW-002: tw_s_chuki_calc CRUD ---")

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            Using tx = conn.BeginTransaction()
                Try
                    ' DELETE
                    ExecuteNQ(conn, tx, "DELETE FROM tw_s_chuki_calc")
                    Dim cnt0 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM tw_s_chuki_calc")
                    AssertEqual("TC-TW-002a", "DELETE後の件数", 0, cnt0)

                    ' INSERT
                    ExecuteNQ(conn, tx,
                        "INSERT INTO tw_s_chuki_calc (kykm_id, kykh_id, kykm_no, bukn_nm, kykbnl_no, leakbn_id, kjkbn_id, " &
                        "syutok_zzan, syutok_zou, syutok_gen, syutok_zan, " &
                        "gruikei_zzan, gruikei_zou, gruikei_gen, gruikei_zan, " &
                        "gson_rkei_zzan, gson_rkei_zou, gson_rkei_gen, gson_rkei_zan, " &
                        "boka_zan, " &
                        "lgnpn_zzan, lgnpn_zan, lgnpn_zan_1nai, lgnpn_zan_1cho, " &
                        "lgnpn_zan_2nai, lgnpn_zan_3nai, lgnpn_zan_4nai, lgnpn_zan_5nai, lgnpn_zan_5cho, " &
                        "lrsok_zzan, lrsok_zan, lrsok_zan_1nai, lrsok_zan_1cho, " &
                        "lrsok_zan_2nai, lrsok_zan_3nai, lrsok_zan_4nai, lrsok_zan_5nai, lrsok_zan_5cho, " &
                        "risoku_mib_zan, risoku_hassei_toki, risoku_mib_kaiyak_gen" &
                        ") VALUES (" &
                        "1001, 100, 1, 'テスト物件', 'K-001', 1, 2, " &
                        "100000, 0, 0, 100000, " &
                        "10000, 5000, 0, 15000, " &
                        "0, 0, 0, 0, " &
                        "85000, " &
                        "90000, 85000, 20000, 65000, " &
                        "15000, 15000, 15000, 5000, 0, " &
                        "10000, 8000, 2000, 6000, " &
                        "1500, 1500, 1500, 500, 0, " &
                        "500, 100, 0)")

                    Dim cnt1 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM tw_s_chuki_calc")
                    AssertEqual("TC-TW-002b", "INSERT後の件数", 1, cnt1)

                    ' SELECT確認
                    Using cmd As New NpgsqlCommand("SELECT bukn_nm FROM tw_s_chuki_calc WHERE kykm_id = 1001", conn, tx)
                        Dim val = CStr(cmd.ExecuteScalar())
                        AssertEqual("TC-TW-002c", "bukn_nm", "テスト物件", val)
                    End Using

                Finally
                    tx.Rollback()
                End Try
            End Using
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  TC-TW-003: 条件ワークテーブル CRUD
    ' ================================================================
    Sub Test_TC_TW_003_JokenCrud()
        Console.WriteLine("--- TC-TW-003: 条件ワークテーブル CRUD ---")

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            Using tx = conn.BeginTransaction()
                Try
                    ' tw_s_keijo_joken
                    ExecuteNQ(conn, tx, "DELETE FROM tw_s_keijo_joken")
                    ExecuteNQ(conn, tx, "INSERT INTO tw_s_keijo_joken (meisai, taisho, kjkbn_sisan, kjkbn_hiyo) VALUES (1, 3, true, true)")
                    Dim cnt1 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM tw_s_keijo_joken")
                    AssertEqual("TC-TW-003a", "keijo_joken INSERT", 1, cnt1)

                    ' tw_s_tougetsu_joken (sw_rsok 確認)
                    ExecuteNQ(conn, tx, "DELETE FROM tw_s_tougetsu_joken")
                    ExecuteNQ(conn, tx, "INSERT INTO tw_s_tougetsu_joken (sw_rsok) VALUES (true)")
                    Using cmd As New NpgsqlCommand("SELECT sw_rsok FROM tw_s_tougetsu_joken LIMIT 1", conn, tx)
                        Dim swRsok = CBool(cmd.ExecuteScalar())
                        AssertEqual("TC-TW-003b", "sw_rsok BOOLEAN", True, swRsok)
                    End Using

                    ' tw_s_saimu_joken
                    ExecuteNQ(conn, tx, "DELETE FROM tw_s_saimu_joken")
                    ExecuteNQ(conn, tx, "INSERT INTO tw_s_saimu_joken (kikan_from, kikan_to) VALUES ('2026-04-01', '2027-03-31')")
                    Dim cnt3 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM tw_s_saimu_joken")
                    AssertEqual("TC-TW-003c", "saimu_joken INSERT", 1, cnt3)

                Finally
                    tx.Rollback()
                End Try
            End Using
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  TC-TW-004: 仕訳出力ワークテーブル CRUD
    ' ================================================================
    Sub Test_TC_TW_004_ShiwakeOutputCrud()
        Console.WriteLine("--- TC-TW-004: 仕訳出力ワークテーブル CRUD ---")

        Dim quotedTables() As String = {
            """tw_f_仕訳出力標準_kj""",
            """tw_f_仕訳出力標準_kj_仕訳data""",
            """tw_f_仕訳出力標準_sh""",
            """tw_f_仕訳出力標準_sh_仕訳data""",
            """tw_f_仕訳出力標準_sm""",
            """tw_f_仕訳出力標準_sm_仕訳data"""
        }

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            Using tx = conn.BeginTransaction()
                Try
                    ' KJ出力制御テーブル
                    ExecuteNQ(conn, tx, "DELETE FROM ""tw_f_仕訳出力標準_kj"" WHERE TRUE")
                    ExecuteNQ(conn, tx, "INSERT INTO ""tw_f_仕訳出力標準_kj"" (""対象年月"", keijo_dt) VALUES ('2026-04-01', '2027-03-31')")
                    Dim cnt1 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM ""tw_f_仕訳出力標準_kj""")
                    AssertEqual("TC-TW-004a", "KJ出力制御INSERT", 1, cnt1)

                    ' SM出力制御テーブル（keijo1_dt, keijo2_dt）
                    ExecuteNQ(conn, tx, "DELETE FROM ""tw_f_仕訳出力標準_sm"" WHERE TRUE")
                    ExecuteNQ(conn, tx, "INSERT INTO ""tw_f_仕訳出力標準_sm"" (""対象年月"", keijo1_dt, keijo2_dt) VALUES ('2026-04-01', '2027-03-31', '2027-04-01')")
                    Dim cnt2 = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM ""tw_f_仕訳出力標準_sm""")
                    AssertEqual("TC-TW-004b", "SM出力制御INSERT", 1, cnt2)

                    ' 全テーブルDELETE確認
                    For Each tbl In quotedTables
                        ExecuteNQ(conn, tx, "DELETE FROM " & tbl & " WHERE TRUE")
                        Dim cnt = ExecuteScalarInt(conn, tx, "SELECT COUNT(*) FROM " & tbl)
                        AssertEqual("TC-TW-004c", tbl & " DELETE", 0, cnt)
                    Next

                Finally
                    tx.Rollback()
                End Try
            End Using
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  TC-TW-005: カラム数検証
    ' ================================================================
    Sub Test_TC_TW_005_ColumnCount()
        Console.WriteLine("--- TC-TW-005: カラム数検証 ---")

        ' (テーブル名, 期待カラム数) ペア
        Dim checks()() As Object = {
            New Object() {"tw_s_chuki_calc", 56},
            New Object() {"tw_s_keijo_joken", 11},
            New Object() {"tw_s_tougetsu_joken", 5},
            New Object() {"tw_s_saimu_joken", 4},
            New Object() {"tw_f_仕訳出力標準_kj", 3},
            New Object() {"tw_f_仕訳出力標準_kj_仕訳data", 30},
            New Object() {"tw_f_仕訳出力標準_sh", 4},
            New Object() {"tw_f_仕訳出力標準_sh_仕訳data", 33},
            New Object() {"tw_f_仕訳出力標準_sm", 4},
            New Object() {"tw_f_仕訳出力標準_sm_仕訳data", 26}
        }

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            For Each chk In checks
                Dim tblName = CStr(chk(0))
                Dim expected = CInt(chk(1))
                Dim sql = "SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = 'public' AND table_name = @name"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@name", tblName)
                    Dim actual = CInt(cmd.ExecuteScalar())
                    AssertEqual("TC-TW-005", tblName & " カラム数", expected, actual)
                End Using
            Next
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  TC-TW-006: インデックス存在確認
    ' ================================================================
    Sub Test_TC_TW_006_IndexExists()
        Console.WriteLine("--- TC-TW-006: インデックス存在確認 ---")

        Dim indexes() As String = {
            "idx_tw_s_chuki_calc_kykm",
            "idx_tw_f_kj_仕訳data_seq",
            "idx_tw_f_sh_仕訳data_seq",
            "idx_tw_f_sm_仕訳data_seq"
        }

        Using conn As New NpgsqlConnection(_connStr)
            conn.Open()
            For Each idxName As String In indexes
                Dim sql = "SELECT COUNT(*) FROM pg_indexes WHERE schemaname = 'public' AND indexname = @name"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@name", idxName)
                    Dim cnt = CInt(cmd.ExecuteScalar())
                    If cnt > 0 Then
                        Console.WriteLine("  [PASS] インデックス " & idxName & " 存在確認OK")
                        _passCount += 1
                    Else
                        Console.WriteLine("  [FAIL] インデックス " & idxName & " が見つかりません")
                        _failCount += 1
                    End If
                End Using
            Next
        End Using
        Console.WriteLine()
    End Sub

    ' ================================================================
    '  ヘルパー
    ' ================================================================

    Sub ExecuteNQ(conn As NpgsqlConnection, tx As NpgsqlTransaction, sql As String)
        Using cmd As New NpgsqlCommand(sql, conn, tx)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Function ExecuteScalarInt(conn As NpgsqlConnection, tx As NpgsqlTransaction, sql As String) As Integer
        Using cmd As New NpgsqlCommand(sql, conn, tx)
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Sub AssertEqual(Of T)(testId As String, label As String, expected As T, actual As T)
        If expected.Equals(actual) Then
            Console.WriteLine("  [PASS] " & testId & ": " & label & " = " & actual.ToString())
            _passCount += 1
        Else
            Console.WriteLine("  [FAIL] " & testId & ": " & label & " 期待=" & expected.ToString() & " 実際=" & actual.ToString())
            _failCount += 1
        End If
    End Sub

End Module
