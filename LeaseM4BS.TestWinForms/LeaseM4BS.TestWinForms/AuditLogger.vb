' =========================================================
' AuditLogger - 操作・更新ログ記録モジュール
' Access版 p_LOG.txt (gLOG_RecEdit, p_ULOG_LOGSUB) に準拠
' Issue #28: L_SLOG / L_ULOG / L_BKLOG の記録基盤
' =========================================================
Imports System.Data
Imports System.Text
Imports LeaseM4BS.DataAccess
Imports Npgsql

Public Module AuditLogger

    ' =========================================================
    ' L_ULOG: 更新ログ記録
    ' Access版 pc_ULOG.OutputULOG + p_ULOG_LOGSUB に準拠
    ' =========================================================

    ''' <summary>
    ''' 更新ログを1件記録する（Access版 pc_ULOG.OutputULOG 相当）
    ''' </summary>
    ''' <param name="slogNo">親操作ログ番号（WriteAuditLog の戻り値）</param>
    ''' <param name="tableName">対象テーブル名（例: "D_KYKH"）</param>
    ''' <param name="updateType">更新種別（"追加"/"更新"/"削除"）※Access版準拠で日本語</param>
    ''' <param name="keyName1">キー項目名1（日本語名）</param>
    ''' <param name="keyValue1">キー値1</param>
    ''' <param name="keyName2">キー項目名2（日本語名）。省略時は空文字</param>
    ''' <param name="keyValue2">キー値2。省略時は空文字</param>
    ''' <param name="rec1">更新前レコード（CSV形式）。省略時は空文字</param>
    ''' <param name="rec2">更新後レコード（CSV形式）。省略時は空文字</param>
    ''' <param name="crud">トランザクション内で使用する場合のCrudHelperインスタンス。省略時は新規作成</param>
    ''' <returns>採番された ulog_no。失敗時は -1</returns>
    Public Function WriteUpdateLog(slogNo As Integer,
                                   tableName As String,
                                   updateType As String,
                                   keyName1 As String,
                                   keyValue1 As String,
                                   Optional keyName2 As String = "",
                                   Optional keyValue2 As String = "",
                                   Optional rec1 As String = "",
                                   Optional rec2 As String = "",
                                   Optional crud As CrudHelper = Nothing) As Integer
        Try
            ' Access版準拠: fgNT_ULOGOUT チェック
            If Not LoginSession.EnableUserLog Then
                Return -1
            End If

            If slogNo < 0 Then
                Return -1
            End If

            Dim ownCrud As Boolean = (crud Is Nothing)
            If ownCrud Then
                crud = New CrudHelper()
            End If

            ' ulog_no 採番: Access版と同じ MAX+1 方式（slog_no 単位）
            Dim seqSql As String = "SELECT COALESCE(MAX(ulog_no), 0) + 1 FROM l_ulog WHERE slog_no = @slog_no"
            Dim seqPrms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@slog_no", slogNo)
            }
            Dim ulogNo As Integer = crud.ExecuteScalar(Of Integer)(seqSql, seqPrms)

            ' Access版準拠: fgNT_RECOUT チェック（rec1/rec2 の記録制御）
            ' Access版: fgNT_RECOUT=True → rec1/rec2格納, RECF=Null
            '           fgNT_RECOUT=False → rec1/rec2空, RECF="1"
            Dim actualRec1 As String = ""
            Dim actualRec2 As String = ""
            Dim recf As String = "1"  ' デフォルト: レコード非格納
            If LoginSession.EnableRecordLog Then
                actualRec1 = If(rec1, "")
                actualRec2 = If(rec2, "")
                recf = ""  ' Access版準拠: レコード格納時は Null/空文字
            End If

            Dim sql As String = "INSERT INTO l_ulog (slog_no, ulog_no, tbl_nm, upd_nm, " &
                                "key_nm1, key_val1, key_nm2, key_val2, " &
                                "rec1, rec2, db_version, recf) " &
                                "VALUES (@slog_no, @ulog_no, @tbl_nm, @upd_nm, " &
                                "@key_nm1, @key_val1, @key_nm2, @key_val2, " &
                                "@rec1, @rec2, @db_version, @recf)"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@slog_no", slogNo),
                New NpgsqlParameter("@ulog_no", ulogNo),
                New NpgsqlParameter("@tbl_nm", If(tableName, "")),
                New NpgsqlParameter("@upd_nm", If(updateType, "")),
                New NpgsqlParameter("@key_nm1", If(keyName1, "")),
                New NpgsqlParameter("@key_val1", If(keyValue1, "")),
                New NpgsqlParameter("@key_nm2", If(keyName2, "")),
                New NpgsqlParameter("@key_val2", If(keyValue2, "")),
                New NpgsqlParameter("@rec1", actualRec1),
                New NpgsqlParameter("@rec2", actualRec2),
                New NpgsqlParameter("@db_version", If(LoginSession.DbVersion, "")),
                New NpgsqlParameter("@recf", recf)
            }
            crud.ExecuteNonQuery(sql, prms)
            Return ulogNo
        Catch ex As Exception
            Console.WriteLine($"[WriteUpdateLog] ログ記録失敗: {ex.Message}")
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' SQLで取得したレコードセットの全行を更新ログに記録する
    ''' （Access版 p_ULOG_LOGSUB 完全準拠）
    ''' </summary>
    ''' <param name="slogNo">親操作ログ番号</param>
    ''' <param name="selectSql">対象レコード取得用SELECT文</param>
    ''' <param name="tableName">テーブル名</param>
    ''' <param name="updateType">更新種別（"追加"/"更新"/"削除"）</param>
    ''' <param name="keyField1">主キーフィールド名1（SQLカラム名）</param>
    ''' <param name="keyDisplayName1">主キー表示名1（日本語名）</param>
    ''' <param name="keyField2">主キーフィールド名2。省略時は空文字</param>
    ''' <param name="keyDisplayName2">主キー表示名2。省略時は空文字</param>
    ''' <param name="selectParams">SELECT文のパラメータ。省略時はなし</param>
    ''' <param name="crud">トランザクション内で使用する場合のCrudHelper。省略時は新規作成</param>
    ''' <returns>記録した件数。失敗時は -1</returns>
    Public Function WriteUpdateLogBatch(slogNo As Integer,
                                        selectSql As String,
                                        tableName As String,
                                        updateType As String,
                                        keyField1 As String,
                                        keyDisplayName1 As String,
                                        Optional keyField2 As String = "",
                                        Optional keyDisplayName2 As String = "",
                                        Optional selectParams As List(Of NpgsqlParameter) = Nothing,
                                        Optional crud As CrudHelper = Nothing) As Integer
        Try
            If Not LoginSession.EnableUserLog Then
                Return -1
            End If

            If slogNo < 0 Then
                Return -1
            End If

            Dim ownCrud As Boolean = (crud Is Nothing)
            If ownCrud Then
                crud = New CrudHelper()
            End If

            Dim dt As DataTable = crud.GetDataTable(selectSql, selectParams)
            Dim count As Integer = 0

            For Each row As DataRow In dt.Rows
                Dim kv1 As String = ""
                Dim kv2 As String = ""

                If Not String.IsNullOrEmpty(keyField1) AndAlso dt.Columns.Contains(keyField1) Then
                    kv1 = If(row(keyField1) IsNot DBNull.Value, row(keyField1).ToString(), "")
                End If
                If Not String.IsNullOrEmpty(keyField2) AndAlso dt.Columns.Contains(keyField2) Then
                    kv2 = If(row(keyField2) IsNot DBNull.Value, row(keyField2).ToString(), "")
                End If

                ' rec1: 現在のレコード内容をCSV化（fgNT_RECOUT=True時のみ実際に格納）
                Dim csvRecord As String = SerializeRecord(row)

                WriteUpdateLog(slogNo, tableName, updateType,
                               keyDisplayName1, kv1,
                               keyDisplayName2, kv2,
                               csvRecord, "",
                               crud)
                count += 1
            Next

            Return count
        Catch ex As Exception
            Console.WriteLine($"[WriteUpdateLogBatch] ログ記録失敗: {ex.Message}")
            Return -1
        End Try
    End Function

    ' =========================================================
    ' L_BKLOG: バックアップログ記録
    ' Access版 pc_BKLOG.OutputBKLOG に準拠
    ' =========================================================

    ''' <summary>
    ''' バックアップログを記録する（Access版 pc_BKLOG.OutputBKLOG 相当）
    ''' バックアップ機能本体の実装時に呼び出す
    ''' </summary>
    ''' <param name="operationName">操作名（"保存"/"復元"/"再構築"）</param>
    ''' <param name="operationDetail">操作内容</param>
    ''' <param name="fileName">バックアップファイル名</param>
    ''' <param name="folder">バックアップフォルダパス</param>
    ''' <param name="password">パスワード（暗号化して保存）。省略時は空文字</param>
    Public Sub WriteBkLog(operationName As String,
                          operationDetail As String,
                          fileName As String,
                          folder As String,
                          Optional password As String = "")
        Try
            Dim crud As New CrudHelper()
            Dim sql As String = "INSERT INTO l_bklog (op_dt, op_nm, op_s, op_user_cd, op_user_nm, " &
                                "pc_name, ip_adr, win_user, file_name, folder, pwd, db_version) " &
                                "VALUES (@op_dt, @op_nm, @op_s, @op_user_cd, @op_user_nm, " &
                                "@pc_name, @ip_adr, @win_user, @file_name, @folder, @pwd, @db_version)"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@op_dt", DateTime.Now),
                New NpgsqlParameter("@op_nm", If(operationName, "")),
                New NpgsqlParameter("@op_s", If(operationDetail, "")),
                New NpgsqlParameter("@op_user_cd", If(LoginSession.LoggedInUserCd, "")),
                New NpgsqlParameter("@op_user_nm", If(LoginSession.LoggedInUserNm, "")),
                New NpgsqlParameter("@pc_name", Environment.MachineName),
                New NpgsqlParameter("@ip_adr", LoginSession.GetLocalIpAddress()),
                New NpgsqlParameter("@win_user", Environment.UserName),
                New NpgsqlParameter("@file_name", If(fileName, "")),
                New NpgsqlParameter("@folder", If(folder, "")),
                New NpgsqlParameter("@pwd", If(password, "")),
                New NpgsqlParameter("@db_version", If(LoginSession.DbVersion, ""))
            }
            crud.ExecuteNonQuery(sql, prms)
        Catch ex As Exception
            Console.WriteLine($"[WriteBkLog] ログ記録失敗: {ex.Message}")
        End Try
    End Sub

    ' =========================================================
    ' CSV シリアライズ（Access版 gLOG_RecEdit 完全準拠）
    ' =========================================================

    ''' <summary>
    ''' DataRow の全フィールドをCSV形式にシリアライズする
    ''' Access版 gLOG_RecEdit (p_LOG.txt:11-27) 完全準拠
    ''' </summary>
    Public Function SerializeRecord(row As DataRow) As String
        Dim sb As New StringBuilder()
        For i As Integer = 0 To row.Table.Columns.Count - 1
            If i > 0 Then sb.Append(",")
            sb.Append(FormatFieldForCsv(row(i)))
        Next
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Dictionary の全値をCSV形式にシリアライズする
    ''' </summary>
    Public Function SerializeRecord(columnValues As Dictionary(Of String, Object)) As String
        Dim sb As New StringBuilder()
        Dim first As Boolean = True
        For Each kvp In columnValues
            If Not first Then sb.Append(",")
            sb.Append(FormatFieldForCsv(kvp.Value))
            first = False
        Next
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' フィールド値をCSV用にフォーマットする
    ''' Access版 gFldValConvForCSV (p_LOG.txt:38-91) 完全準拠
    ''' </summary>
    Private Function FormatFieldForCsv(value As Object) As String
        ' Null / DBNull → 空文字
        If value Is Nothing OrElse value Is DBNull.Value Then
            Return ""
        End If

        ' Boolean → Access互換: True="-1", False="0"（ダブルクォート囲み）
        If TypeOf value Is Boolean Then
            Dim boolStr As String = If(CBool(value), "-1", "0")
            Return """" & boolStr & """"
        End If

        ' 文字列 → 改行除去 + ダブルクォートエスケープ + ダブルクォート囲み
        If TypeOf value Is String Then
            Dim strVal As String = CStr(value)
            ' 改行文字を除去（Access版: vbCr, vbLf を除去）
            strVal = strVal.Replace(vbCr, "").Replace(vbLf, "")
            ' ダブルクォートをエスケープ（""→""""）
            strVal = strVal.Replace("""", """""")
            Return """" & strVal & """"
        End If

        ' その他の型 → CStr() で文字列変換
        Return CStr(value)
    End Function

End Module
