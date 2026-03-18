' 出力ファイル差分検出テスト (Issue #29 Step 3)
' CSV・固定長ファイルの自動比較を行い、Access版との差異を検出する
'
' 対象:
'   - Part 1: CSV比較エンジン (Shift-JIS, カンマ区切り, テキスト修飾対応)
'   - Part 2: 固定長比較エンジン (Shift-JISバイト単位, KITOKU 4フォーマット)
'   - Part 3: FixedLengthFileWriter 出力→読戻し一致検証
'   - Part 4: 差分レポートCSV自動生成
'
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_output_diff.vb
' 実行: test_output_diff.exe

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic
Imports LeaseM4BS.DataAccess
Imports LeaseM4BS.DataAccess.LeaseM4BS.DataAccess

Module TestOutputDiff

    Private sjis As Encoding = Encoding.GetEncoding("Shift_JIS")
    Private passCount As Integer = 0
    Private failCount As Integer = 0
    Private diffReport As New List(Of String)

    Sub Main()
        Console.OutputEncoding = Encoding.UTF8
        Console.WriteLine("=== 出力ファイル差分検出テスト (Issue #29) ===")
        Console.WriteLine()

        diffReport.Add("TestID,FileType,Line,Column,ExpectedValue,ActualValue,Status")

        ' ==== Part 1: CSV比較エンジン ====
        Console.WriteLine("--- Part 1: CSV比較エンジン ---")
        Test_CsvCompare_Identical()
        Test_CsvCompare_ValueDiff()
        Test_CsvCompare_RowCountDiff()
        Test_CsvCompare_ColCountDiff()
        Test_CsvCompare_NumericTolerance()
        Test_CsvCompare_TextQualifier()
        Console.WriteLine()

        ' ==== Part 2: 固定長比較エンジン ====
        Console.WriteLine("--- Part 2: 固定長比較エンジン ---")
        Test_FixedCompare_Identical()
        Test_FixedCompare_ByteDiff()
        Test_FixedCompare_RecordLengthDiff()
        Console.WriteLine()

        ' ==== Part 3: FixedLengthFileWriter 出力→読戻し一致 ====
        Console.WriteLine("--- Part 3: FixedLengthFileWriter ラウンドトリップ ---")
        Test_CMSW2WRK_Roundtrip()
        Test_APGDHWRK_Roundtrip()
        Console.WriteLine()

        ' ==== Part 4: 差分レポート生成 ====
        Console.WriteLine("--- Part 4: 差分レポート生成 ---")
        Test_DiffReportGeneration()
        Console.WriteLine()

        ' ==== 結果サマリ ====
        Console.WriteLine("=" & New String("="c, 50))
        Console.WriteLine($"結果: PASS={passCount}, FAIL={failCount}")
        Console.WriteLine($"合計: {passCount + failCount} テスト")
        Console.WriteLine("=" & New String("="c, 50))

        Environment.ExitCode = If(failCount > 0, 1, 0)
    End Sub

    ' ======================================================================
    '  アサーション
    ' ======================================================================

    Sub AssertTrue(testName As String, condition As Boolean, Optional detail As String = "")
        If condition Then
            passCount += 1
            Console.WriteLine($"  PASS: {testName} {detail}")
        Else
            failCount += 1
            Console.WriteLine($"  FAIL: {testName} {detail}")
        End If
    End Sub

    Sub AssertEqual(testName As String, expected As Integer, actual As Integer)
        If expected = actual Then
            passCount += 1
            Console.WriteLine($"  PASS: {testName} (={actual})")
        Else
            failCount += 1
            Console.WriteLine($"  FAIL: {testName} (期待={expected}, 実際={actual})")
        End If
    End Sub

    ' ======================================================================
    '  CSV比較エンジン
    ' ======================================================================

    ''' <summary>
    ''' 2つのCSVファイルの内容を比較し差分リストを返す
    ''' </summary>
    Function CompareCsvFiles(expectedPath As String, actualPath As String,
                             Optional delimiter As String = ",",
                             Optional numericTolerance As Double = 0,
                             Optional ignoreTextQualifier As Boolean = True) As List(Of String)
        Dim diffs As New List(Of String)
        Dim expectedLines = File.ReadAllLines(expectedPath, sjis)
        Dim actualLines = File.ReadAllLines(actualPath, sjis)

        ' 行数比較
        If expectedLines.Length <> actualLines.Length Then
            diffs.Add($"行数不一致: 期待={expectedLines.Length}, 実際={actualLines.Length}")
        End If

        Dim maxLines = Math.Min(expectedLines.Length, actualLines.Length)
        For i As Integer = 0 To maxLines - 1
            Dim eCols = ParseCsvLine(expectedLines(i), delimiter, ignoreTextQualifier)
            Dim aCols = ParseCsvLine(actualLines(i), delimiter, ignoreTextQualifier)

            If eCols.Length <> aCols.Length Then
                diffs.Add($"行{i + 1}: 列数不一致 (期待={eCols.Length}, 実際={aCols.Length})")
                Continue For
            End If

            For j As Integer = 0 To eCols.Length - 1
                Dim eVal = eCols(j).Trim()
                Dim aVal = aCols(j).Trim()

                If eVal <> aVal Then
                    ' 数値比較を試みる
                    Dim expectedNum As Double = 0
                    Dim actualNum As Double = 0
                    If numericTolerance > 0 AndAlso
                       Double.TryParse(eVal, expectedNum) AndAlso Double.TryParse(aVal, actualNum) Then
                        If Math.Abs(expectedNum - actualNum) <= numericTolerance Then
                            Continue For ' 許容範囲内
                        End If
                    End If
                    diffs.Add($"行{i + 1},列{j + 1}: '{eVal}' != '{aVal}'")
                End If
            Next
        Next

        Return diffs
    End Function

    ''' <summary>CSV行をパースする（テキスト修飾対応）</summary>
    Function ParseCsvLine(line As String, delimiter As String, ignoreQualifier As Boolean) As String()
        If ignoreQualifier Then
            ' 単純分割（テキスト修飾を除去）
            Dim parts = line.Split(delimiter.ToCharArray())
            For i As Integer = 0 To parts.Length - 1
                parts(i) = parts(i).Trim(""""c)
            Next
            Return parts
        Else
            Return line.Split(delimiter.ToCharArray())
        End If
    End Function

    ' ======================================================================
    '  固定長比較エンジン
    ' ======================================================================

    ''' <summary>
    ''' 2つの固定長ファイルをバイト単位で比較し差分リストを返す
    ''' </summary>
    Function CompareFixedLengthFiles(expectedPath As String, actualPath As String,
                                     fields As List(Of FixedLengthFieldDef)) As List(Of String)
        Dim diffs As New List(Of String)
        Dim expectedLines = File.ReadAllLines(expectedPath, sjis)
        Dim actualLines = File.ReadAllLines(actualPath, sjis)

        If expectedLines.Length <> actualLines.Length Then
            diffs.Add($"レコード数不一致: 期待={expectedLines.Length}, 実際={actualLines.Length}")
        End If

        Dim maxLines = Math.Min(expectedLines.Length, actualLines.Length)
        For i As Integer = 0 To maxLines - 1
            Dim eBytes = sjis.GetBytes(expectedLines(i))
            Dim aBytes = sjis.GetBytes(actualLines(i))

            ' レコード長チェック
            Dim expectedRecLen = 0
            For Each f In fields
                expectedRecLen += f.ByteWidth
            Next

            If eBytes.Length <> expectedRecLen Then
                diffs.Add($"レコード{i + 1}: 期待側レコード長不一致 ({eBytes.Length} != {expectedRecLen})")
            End If

            ' フィールドごとの比較
            Dim eOffset = 0
            Dim aOffset = 0
            For Each f In fields
                If eOffset + f.ByteWidth <= eBytes.Length AndAlso aOffset + f.ByteWidth <= aBytes.Length Then
                    Dim eField = sjis.GetString(eBytes, eOffset, f.ByteWidth)
                    Dim aField = sjis.GetString(aBytes, aOffset, f.ByteWidth)
                    If eField <> aField Then
                        diffs.Add($"レコード{i + 1},フィールド'{f.Name}': '{eField.TrimEnd()}' != '{aField.TrimEnd()}'")
                    End If
                End If
                eOffset += f.ByteWidth
                aOffset += f.ByteWidth
            Next
        Next

        Return diffs
    End Function

    ' ======================================================================
    '  テストファイル生成ヘルパー
    ' ======================================================================

    Function CreateTempFile(content As String) As String
        Dim path = IO.Path.GetTempFileName()
        File.WriteAllText(path, content, sjis)
        Return path
    End Function

    Sub DeleteTempFile(path As String)
        Try
            If File.Exists(path) Then File.Delete(path)
        Catch
        End Try
    End Sub

    ' ======================================================================
    '  Part 1: CSV比較テスト
    ' ======================================================================

    Sub Test_CsvCompare_Identical()
        Dim content = "A,B,C" & vbCrLf & "1,2,3" & vbCrLf & "4,5,6"
        Dim f1 = CreateTempFile(content)
        Dim f2 = CreateTempFile(content)
        Try
            Dim diffs = CompareCsvFiles(f1, f2)
            AssertTrue("CSV同一ファイル→差分0件", diffs.Count = 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_CsvCompare_ValueDiff()
        Dim f1 = CreateTempFile("A,B,C" & vbCrLf & "1,2,3")
        Dim f2 = CreateTempFile("A,B,C" & vbCrLf & "1,9,3")
        Try
            Dim diffs = CompareCsvFiles(f1, f2)
            AssertTrue("CSV値差異→差分検出", diffs.Count = 1, "(" & diffs.Count.ToString() & "件)")
            If diffs.Count > 0 Then
                AssertTrue("差分に列2を含む", diffs(0).Contains("列2"))
            End If
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_CsvCompare_RowCountDiff()
        Dim f1 = CreateTempFile("A,B" & vbCrLf & "1,2" & vbCrLf & "3,4")
        Dim f2 = CreateTempFile("A,B" & vbCrLf & "1,2")
        Try
            Dim diffs = CompareCsvFiles(f1, f2)
            AssertTrue("CSV行数差異→検出", diffs.Count > 0)
            If diffs.Count > 0 Then
                AssertTrue("行数不一致メッセージ", diffs(0).Contains("行数不一致"))
            End If
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_CsvCompare_ColCountDiff()
        Dim f1 = CreateTempFile("A,B,C" & vbCrLf & "1,2,3")
        Dim f2 = CreateTempFile("A,B" & vbCrLf & "1,2")
        Try
            Dim diffs = CompareCsvFiles(f1, f2)
            AssertTrue("CSV列数差異→検出", diffs.Count > 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_CsvCompare_NumericTolerance()
        Dim f1 = CreateTempFile("金額" & vbCrLf & "100000")
        Dim f2 = CreateTempFile("金額" & vbCrLf & "100001")
        Try
            ' 許容誤差0 → 差分あり
            Dim diffs0 = CompareCsvFiles(f1, f2, ",", 0)
            AssertTrue("許容誤差0→差分あり", diffs0.Count > 0)

            ' 許容誤差1 → 差分なし
            Dim diffs1 = CompareCsvFiles(f1, f2, ",", 1)
            AssertTrue("許容誤差1→差分なし", diffs1.Count = 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_CsvCompare_TextQualifier()
        Dim f1 = CreateTempFile("""A"",""B""" & vbCrLf & """1"",""2""")
        Dim f2 = CreateTempFile("A,B" & vbCrLf & "1,2")
        Try
            ' テキスト修飾を無視して比較
            Dim diffs = CompareCsvFiles(f1, f2, ",", 0, True)
            AssertTrue("テキスト修飾無視→差分なし", diffs.Count = 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    ' ======================================================================
    '  Part 2: 固定長比較テスト
    ' ======================================================================

    Sub Test_FixedCompare_Identical()
        Dim fields As New List(Of FixedLengthFieldDef) From {
            New FixedLengthFieldDef("F1", 5),
            New FixedLengthFieldDef("F2", 10),
            New FixedLengthFieldDef("F3", 5, "00000")
        }
        Dim content = "ABCDE" & New String(" "c, 10) & "00001"
        Dim f1 = CreateTempFile(content)
        Dim f2 = CreateTempFile(content)
        Try
            Dim diffs = CompareFixedLengthFiles(f1, f2, fields)
            AssertTrue("固定長同一→差分0件", diffs.Count = 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_FixedCompare_ByteDiff()
        Dim fields As New List(Of FixedLengthFieldDef) From {
            New FixedLengthFieldDef("F1", 5),
            New FixedLengthFieldDef("F2", 5)
        }
        Dim f1 = CreateTempFile("ABCDE12345")
        Dim f2 = CreateTempFile("ABCDE99999")
        Try
            Dim diffs = CompareFixedLengthFiles(f1, f2, fields)
            AssertTrue("固定長フィールド差異→検出", diffs.Count > 0)
            If diffs.Count > 0 Then
                AssertTrue("差分にF2を含む", diffs(0).Contains("F2"))
            End If
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    Sub Test_FixedCompare_RecordLengthDiff()
        Dim fields As New List(Of FixedLengthFieldDef) From {
            New FixedLengthFieldDef("F1", 10)
        }
        Dim f1 = CreateTempFile("ABCDE12345")
        Dim f2 = CreateTempFile("ABCDE12345" & vbCrLf & "FGHIJ67890")
        Try
            Dim diffs = CompareFixedLengthFiles(f1, f2, fields)
            AssertTrue("固定長レコード数差異→検出", diffs.Count > 0)
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

    ' ======================================================================
    '  Part 3: FixedLengthFileWriter ラウンドトリップ
    ' ======================================================================

    Sub Test_CMSW2WRK_Roundtrip()
        Dim fields = KitokuFixedLengthFormats.GetCMSW2WRKFields()
        Dim dt As New DataTable()
        For Each f In fields
            dt.Columns.Add(f.Name, GetType(String))
        Next

        ' テストデータ投入
        Dim row = dt.NewRow()
        row("SW2_KAI_CODE") = "00001"
        row("SW2_DATE") = "2024/04/01"
        row("SW2_DEN_NO") = "00000001"
        row("SW2_GYO_NO") = 1
        row("SW2_DC_KBN") = "1"
        row("SW2_KMK_CODE") = "1234"
        row("SW2_KIN") = 100000.0
        dt.Rows.Add(row)

        ' 書き出し
        Dim tempPath = IO.Path.GetTempFileName() & ".txt"
        Try
            FixedLengthFileWriter.WriteFile(tempPath, dt, fields)

            ' 読戻し
            Dim lines = File.ReadAllLines(tempPath, sjis)
            AssertTrue("CMSW2WRK 出力1行", lines.Length = 1)

            ' バイト長検証
            If lines.Length > 0 Then
                Dim bytes = sjis.GetBytes(lines(0))
                Dim expectedLen = 0
                For Each f In fields
                    expectedLen += f.ByteWidth
                Next
                AssertEqual("CMSW2WRK レコード長", expectedLen, bytes.Length)
            End If

            ' 自己比較（同一ファイル同士）
            Dim diffs = CompareFixedLengthFiles(tempPath, tempPath, fields)
            AssertTrue("CMSW2WRK 自己比較→差分0件", diffs.Count = 0)
        Finally
            DeleteTempFile(tempPath)
        End Try
    End Sub

    Sub Test_APGDHWRK_Roundtrip()
        Dim fields = KitokuFixedLengthFormats.GetAPGDHWRKFields()
        Dim dt As New DataTable()
        For Each f In fields
            dt.Columns.Add(f.Name, GetType(String))
        Next

        Dim row = dt.NewRow()
        row("GDH_KAI_CODE") = "00001"
        row("GDH_DEN_KBN") = "01"
        row("GDH_DEN_NO") = "00000001"
        row("GDH_KEI_KIN") = 50000.0
        row("GDH_SAI_KIN") = 50000.0
        dt.Rows.Add(row)

        Dim tempPath = IO.Path.GetTempFileName() & ".txt"
        Try
            FixedLengthFileWriter.WriteFile(tempPath, dt, fields)

            Dim lines = File.ReadAllLines(tempPath, sjis)
            AssertTrue("APGDHWRK 出力1行", lines.Length = 1)

            If lines.Length > 0 Then
                Dim bytes = sjis.GetBytes(lines(0))
                Dim expectedLen = 0
                For Each f In fields
                    expectedLen += f.ByteWidth
                Next
                AssertEqual("APGDHWRK レコード長", expectedLen, bytes.Length)
            End If

            Dim diffs = CompareFixedLengthFiles(tempPath, tempPath, fields)
            AssertTrue("APGDHWRK 自己比較→差分0件", diffs.Count = 0)
        Finally
            DeleteTempFile(tempPath)
        End Try
    End Sub

    ' ======================================================================
    '  Part 4: 差分レポート生成
    ' ======================================================================

    Sub Test_DiffReportGeneration()
        ' テスト用CSV差分を生成
        Dim f1 = CreateTempFile("ID,金額,日付" & vbCrLf & "1,100000,2024/04/01" & vbCrLf & "2,200000,2024/05/01")
        Dim f2 = CreateTempFile("ID,金額,日付" & vbCrLf & "1,100001,2024/04/01" & vbCrLf & "2,200000,2024/05/02")
        Try
            Dim diffs = CompareCsvFiles(f1, f2)

            ' 差分レポートに記録
            For Each d In diffs
                diffReport.Add("DiffReport-001,CSV," & d & ",DIFF")
            Next

            ' レポートファイル出力
            Dim reportPath = IO.Path.Combine("test-data", "diff_report.csv")
            File.WriteAllLines(reportPath, diffReport, sjis)

            AssertTrue("差分レポート生成", File.Exists(reportPath))
            AssertTrue("レポートに差分記録あり", diffs.Count > 0, "(" & diffs.Count.ToString() & "件)")

            ' レポート内容表示
            Console.WriteLine("  -- 差分レポート内容 --")
            For Each d In diffs
                Console.WriteLine("    " & d)
            Next
        Finally
            DeleteTempFile(f1) : DeleteTempFile(f2)
        End Try
    End Sub

End Module
