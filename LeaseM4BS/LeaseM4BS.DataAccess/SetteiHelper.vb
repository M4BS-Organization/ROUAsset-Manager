' 設定管理ヘルパー (Access版 cn_typ_mSETTEI / gSET_* 系関数 相当)
' t_settei テーブルと4つのワークテーブル間のデータ入出力を管理する

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

''' <summary>
''' 設定レコード（Access版 cn_typ_mSETTEI 相当）
''' </summary>
Public Class SetteiRecord
    Public Property SetteiId As Integer
    Public Property SetteiNm As String
    Public Property SetteiNmJpn As String
    Public Property SetteiType As Integer   ' 0=テキスト, 1=数値, 2=日時
    Public Property ValText As String
    Public Property ValNumber As Double?
    Public Property ValDatetime As DateTime?
    Public Property Biko As String
End Class

''' <summary>
''' 設定管理ヘルパークラス
''' t_settei テーブルと仕訳出力設定ワークテーブル間のデータ変換を行う。
''' Access版 gSET_T_SETTEI_to_tmSETTEI / gSET_tmSETTEI_to_T_SETTEI /
''' gSET_tmSETTEI_to_WKTBL / gSET_WKTBL_to_tmSETTEI 等に対応。
''' </summary>
Public Class SetteiHelper
    Implements IDisposable

    Private _crud As CrudHelper
    Private _disposed As Boolean = False

    ' ワークテーブル名
    Private Const WORK_TABLE_SH As String = "tw_f_仕訳出力標準_設定_swksh"
    Private Const WORK_TABLE_KJ As String = "tw_f_仕訳出力標準_設定_swkkj"
    Private Const WORK_TABLE_SM As String = "tw_f_仕訳出力標準_設定_swksm"
    Private Const WORK_TABLE_KY As String = "tw_f_仕訳出力標準_設定_swkky"

    ' 設定名プレフィックス
    Private Const PREFIX_SH As String = "SWKSH_"
    Private Const PREFIX_KJ As String = "SWKKJ_"
    Private Const PREFIX_SM As String = "SWKSM_"
    Private Const PREFIX_KY As String = "SWKKY_"

    ' boolean型カラム判定用サフィックス
    Private Shared ReadOnly BooleanSuffixes As String() = {"_out_f", "_kno_togo_f", "_krzei_out_f", "_kaiyk_out_f"}

    ' ================================================================
    '  コンストラクタ
    ' ================================================================

    ''' <summary>
    ''' デフォルトコンストラクタ（内部でCrudHelperを生成）
    ''' </summary>
    Public Sub New()
        _crud = New CrudHelper()
    End Sub

    ''' <summary>
    ''' CrudHelper注入コンストラクタ
    ''' </summary>
    Public Sub New(crud As CrudHelper)
        If crud Is Nothing Then
            Throw New ArgumentNullException(NameOf(crud))
        End If
        _crud = crud
    End Sub

    ' ================================================================
    '  プロパティ
    ' ================================================================

    ''' <summary>
    ''' トランザクションが進行中かどうか
    ''' </summary>
    Public ReadOnly Property IsInTransaction As Boolean
        Get
            Return _crud.IsInTransaction
        End Get
    End Property

    ' ================================================================
    '  1. LoadAllFromTSettei  ← gSET_T_SETTEI_to_tmSETTEI
    ' ================================================================

    ''' <summary>
    ''' t_settei から仕訳出力設定レコードを全件読み込む
    ''' </summary>
    Public Function LoadAllFromTSettei() As List(Of SetteiRecord)
        Dim sql As String =
            "SELECT * FROM t_settei " &
            "WHERE settei_nm LIKE 'SWKSH_%' OR settei_nm LIKE 'SWKKJ_%' " &
            "   OR settei_nm LIKE 'SWKSM_%' OR settei_nm LIKE 'SWKKY_%'"

        Dim dt As DataTable = _crud.GetDataTable(sql)
        Dim records As New List(Of SetteiRecord)

        For Each row As DataRow In dt.Rows
            Dim rec As New SetteiRecord()
            rec.SetteiId = _crud.SafeConvert(Of Integer)(row("settei_id"))
            rec.SetteiNm = _crud.SafeConvert(Of String)(row("settei_nm"), "")
            rec.SetteiNmJpn = _crud.SafeConvert(Of String)(row("settei_nm_jpn"), "")
            rec.SetteiType = _crud.SafeConvert(Of Integer)(row("settei_type"))
            rec.ValText = _crud.SafeConvert(Of String)(row("val_text"), "")
            rec.ValNumber = If(IsDBNull(row("val_number")), CType(Nothing, Double?), Convert.ToDouble(row("val_number")))
            rec.ValDatetime = If(IsDBNull(row("val_datetime")), CType(Nothing, DateTime?), Convert.ToDateTime(row("val_datetime")))
            rec.Biko = _crud.SafeConvert(Of String)(row("biko"), "")
            records.Add(rec)
        Next

        Return records
    End Function

    ' ================================================================
    '  2. SaveRecordsToTSettei  ← gSET_tmSETTEI_to_T_SETTEI
    ' ================================================================

    ''' <summary>
    ''' 設定レコードリストを t_settei に保存する（DELETE+INSERT）
    ''' </summary>
    Public Sub SaveRecordsToTSettei(records As List(Of SetteiRecord))
        If records Is Nothing Then
            Throw New ArgumentNullException(NameOf(records))
        End If

        ' 既存トランザクション対応
        Dim ownTransaction As Boolean = Not _crud.IsInTransaction

        Try
            If ownTransaction Then
                _crud.BeginTransaction()
            End If

            ' 4プレフィックス分のレコードを削除
            Dim deleteSql As String =
                "DELETE FROM t_settei " &
                "WHERE settei_nm LIKE 'SWKSH_%' OR settei_nm LIKE 'SWKKJ_%' " &
                "   OR settei_nm LIKE 'SWKSM_%' OR settei_nm LIKE 'SWKKY_%'"
            _crud.ExecuteNonQuery(deleteSql)

            ' 各レコードをINSERT
            For Each rec As SetteiRecord In records
                Dim insertSql As String =
                    "INSERT INTO t_settei (settei_id, settei_nm, settei_nm_jpn, settei_type, val_text, val_number, val_datetime, biko) " &
                    "VALUES (@settei_id, @settei_nm, @settei_nm_jpn, @settei_type, @val_text, @val_number, @val_datetime, @biko)"

                Dim params As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@settei_id", rec.SetteiId),
                    New NpgsqlParameter("@settei_nm", If(CObj(rec.SetteiNm), DBNull.Value)),
                    New NpgsqlParameter("@settei_nm_jpn", If(CObj(rec.SetteiNmJpn), DBNull.Value)),
                    New NpgsqlParameter("@settei_type", rec.SetteiType),
                    New NpgsqlParameter("@val_text", If(CObj(rec.ValText), DBNull.Value)),
                    New NpgsqlParameter("@val_number", If(rec.ValNumber.HasValue, CObj(rec.ValNumber.Value), DBNull.Value)),
                    New NpgsqlParameter("@val_datetime", If(rec.ValDatetime.HasValue, CObj(rec.ValDatetime.Value), DBNull.Value)),
                    New NpgsqlParameter("@biko", If(CObj(rec.Biko), DBNull.Value))
                }

                _crud.ExecuteNonQuery(insertSql, params)
            Next

            If ownTransaction Then
                _crud.Commit()
            End If

        Catch ex As Exception
            If ownTransaction AndAlso _crud.IsInTransaction Then
                Try
                    _crud.Rollback()
                Catch
                End Try
            End If
            Throw New Exception($"設定レコードの保存に失敗しました: {ex.Message}", ex)
        End Try
    End Sub

    ' ================================================================
    '  3. DistributeToWorkTables  ← gSET_tmSETTEI_to_WKTBL
    ' ================================================================

    ''' <summary>
    ''' 設定レコードリストを4つのワークテーブルに振り分けて書き込む
    ''' </summary>
    Public Sub DistributeToWorkTables(records As List(Of SetteiRecord))
        If records Is Nothing Then
            Throw New ArgumentNullException(NameOf(records))
        End If

        ' 4ワークテーブルをクリアして空行1行を作成
        ClearAllWorkTables()
        InsertDefaultRow(WORK_TABLE_SH)
        InsertDefaultRow(WORK_TABLE_KJ)
        InsertDefaultRow(WORK_TABLE_SM)
        InsertDefaultRow(WORK_TABLE_KY)

        ' 各レコードを対応するワークテーブルに反映
        For Each rec As SetteiRecord In records
            Dim prefix As String = GetPrefix(rec.SetteiNm)
            If String.IsNullOrEmpty(prefix) Then Continue For

            Dim tableName As String = GetWorkTableByPrefix(prefix)
            If String.IsNullOrEmpty(tableName) Then Continue For

            Dim columnName As String = rec.SetteiNm.ToLower()

            Try
                Dim quotedTable As String = """" & tableName & """"
                Dim quotedColumn As String = """" & columnName & """"

                If rec.SetteiType = 0 Then
                    ' テキスト型
                    Dim sql As String =
                        $"UPDATE {quotedTable} SET {quotedColumn} = @val " &
                        $"WHERE id = (SELECT MIN(id) FROM {quotedTable})"
                    Dim params As New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@val", If(CObj(rec.ValText), DBNull.Value))
                    }
                    _crud.ExecuteNonQuery(sql, params)

                ElseIf rec.SetteiType = 1 Then
                    ' 数値型
                    If IsBooleanColumn(columnName) Then
                        ' boolean型カラムの場合: val_number(1/0) → boolean(true/false)
                        Dim boolVal As Boolean = (rec.ValNumber.GetValueOrDefault(0) = 1)
                        Dim sql As String =
                            $"UPDATE {quotedTable} SET {quotedColumn} = @val " &
                            $"WHERE id = (SELECT MIN(id) FROM {quotedTable})"
                        Dim params As New List(Of NpgsqlParameter) From {
                            New NpgsqlParameter("@val", boolVal)
                        }
                        _crud.ExecuteNonQuery(sql, params)
                    Else
                        Dim sql As String =
                            $"UPDATE {quotedTable} SET {quotedColumn} = @val " &
                            $"WHERE id = (SELECT MIN(id) FROM {quotedTable})"
                        Dim params As New List(Of NpgsqlParameter) From {
                            New NpgsqlParameter("@val", If(rec.ValNumber.HasValue, CObj(rec.ValNumber.Value), DBNull.Value))
                        }
                        _crud.ExecuteNonQuery(sql, params)
                    End If
                End If

            Catch ex As Exception
                ' カラムが存在しない場合等は無視（Access版 On Error Resume Next 相当）
            End Try
        Next
    End Sub

    ' ================================================================
    '  4. CollectFromWorkTables  ← gSET_WKTBL_to_tmSETTEI
    ' ================================================================

    ''' <summary>
    ''' 4ワークテーブルの値を設定レコードリストに書き戻す
    ''' </summary>
    Public Sub CollectFromWorkTables(records As List(Of SetteiRecord))
        If records Is Nothing Then
            Throw New ArgumentNullException(NameOf(records))
        End If

        ' 4ワークテーブルの1行目を取得してキャッシュ
        Dim tableData As New Dictionary(Of String, DataRow)

        For Each tbl As String In {WORK_TABLE_SH, WORK_TABLE_KJ, WORK_TABLE_SM, WORK_TABLE_KY}
            Dim quotedTable As String = """" & tbl & """"
            Dim dt As DataTable = _crud.GetDataTable($"SELECT * FROM {quotedTable} LIMIT 1")
            If dt.Rows.Count > 0 Then
                tableData(tbl) = dt.Rows(0)
            End If
        Next

        ' 各レコードに対応するワークテーブルの値を書き戻す
        For Each rec As SetteiRecord In records
            Dim prefix As String = GetPrefix(rec.SetteiNm)
            If String.IsNullOrEmpty(prefix) Then Continue For

            Dim tableName As String = GetWorkTableByPrefix(prefix)
            If String.IsNullOrEmpty(tableName) Then Continue For

            If Not tableData.ContainsKey(tableName) Then Continue For

            Dim row As DataRow = tableData(tableName)
            Dim columnName As String = rec.SetteiNm.ToLower()

            Try
                If Not row.Table.Columns.Contains(columnName) Then Continue For

                Dim val As Object = row(columnName)

                If rec.SetteiType = 0 Then
                    ' テキスト型
                    rec.ValText = If(IsDBNull(val), "", Convert.ToString(val))

                ElseIf rec.SetteiType = 1 Then
                    ' 数値型
                    If IsDBNull(val) Then
                        rec.ValNumber = Nothing
                    ElseIf IsBooleanColumn(columnName) Then
                        ' boolean型カラム: true→1, false→0
                        rec.ValNumber = If(Convert.ToBoolean(val), 1.0, 0.0)
                    Else
                        rec.ValNumber = Convert.ToDouble(val)
                    End If
                End If

            Catch ex As Exception
                ' カラム不在等は無視（Access版 On Error Resume Next 相当）
            End Try
        Next
    End Sub

    ' ================================================================
    '  5. LoadSettingsToWorkTables  ← gSET_T_SETTEI_to_WKTBL（一括フロー）
    ' ================================================================

    ''' <summary>
    ''' t_settei からワークテーブルへ設定を一括ロードする
    ''' </summary>
    Public Function LoadSettingsToWorkTables() As Boolean
        Try
            Dim records As List(Of SetteiRecord) = LoadAllFromTSettei()

            If records.Count = 0 Then
                ClearAllWorkTables()
                Return True
            End If

            DistributeToWorkTables(records)
            Return True

        Catch ex As Exception
            Throw New Exception($"設定のワークテーブルへのロードに失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ' ================================================================
    '  6. SaveWorkTablesToTSettei  ← gSET_WKTBL_to_tmSETTEI + gSET_tmSETTEI_to_T_SETTEI
    ' ================================================================

    ''' <summary>
    ''' ワークテーブルの値を t_settei に保存する（一括フロー）
    ''' </summary>
    Public Function SaveWorkTablesToTSettei() As Boolean
        Try
            Dim records As List(Of SetteiRecord) = LoadAllFromTSettei()
            CollectFromWorkTables(records)
            SaveRecordsToTSettei(records)
            Return True

        Catch ex As Exception
            Throw New Exception($"ワークテーブルからの設定保存に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ' ================================================================
    '  7. InitializeDefaultSettings  ← gSET_デフォルト値_to_T_SETTEI
    ' ================================================================

    ''' <summary>
    ''' ワークテーブルのデフォルト値を t_settei に初期登録する
    ''' </summary>
    Public Function InitializeDefaultSettings() As Boolean
        Try
            ' 4ワークテーブルをクリアしてデフォルト行を作成
            ClearAllWorkTables()
            InsertDefaultRow(WORK_TABLE_SH)
            InsertDefaultRow(WORK_TABLE_KJ)
            InsertDefaultRow(WORK_TABLE_SM)
            InsertDefaultRow(WORK_TABLE_KY)

            ' 現在の最大SETTEI_IDを取得
            Dim maxId As Integer = _crud.ExecuteScalar(Of Integer)(
                "SELECT COALESCE(MAX(settei_id), 0) FROM t_settei")

            Dim newRecords As New List(Of SetteiRecord)

            ' 各ワークテーブルからカラム情報を読み取りレコード化
            For Each tblInfo In New Tuple(Of String, String)() {
                Tuple.Create(WORK_TABLE_SH, PREFIX_SH),
                Tuple.Create(WORK_TABLE_KJ, PREFIX_KJ),
                Tuple.Create(WORK_TABLE_SM, PREFIX_SM),
                Tuple.Create(WORK_TABLE_KY, PREFIX_KY)
            }
                Dim tableName As String = tblInfo.Item1
                Dim prefix As String = tblInfo.Item2
                Dim quotedTable As String = """" & tableName & """"

                Dim dt As DataTable = _crud.GetDataTable($"SELECT * FROM {quotedTable} LIMIT 1")
                If dt.Rows.Count = 0 Then Continue For

                Dim row As DataRow = dt.Rows(0)

                For Each col As DataColumn In dt.Columns
                    ' id カラムは除外
                    If col.ColumnName.ToLower() = "id" Then Continue For

                    maxId += 1
                    Dim setteiNm As String = col.ColumnName.ToUpper()
                    Dim rec As New SetteiRecord()
                    rec.SetteiId = maxId
                    rec.SetteiNm = setteiNm
                    rec.SetteiNmJpn = setteiNm

                    Dim val As Object = row(col.ColumnName)

                    If col.DataType Is GetType(Boolean) Then
                        ' boolean → type=1, val_number=0/1
                        rec.SetteiType = 1
                        If IsDBNull(val) Then
                            rec.ValNumber = 0
                        Else
                            rec.ValNumber = If(Convert.ToBoolean(val), 1.0, 0.0)
                        End If
                    ElseIf col.DataType Is GetType(Integer) OrElse
                           col.DataType Is GetType(Long) OrElse
                           col.DataType Is GetType(Short) OrElse
                           col.DataType Is GetType(Double) OrElse
                           col.DataType Is GetType(Decimal) OrElse
                           col.DataType Is GetType(Single) Then
                        ' 数値型 → type=1
                        rec.SetteiType = 1
                        If IsDBNull(val) Then
                            rec.ValNumber = Nothing
                        Else
                            rec.ValNumber = Convert.ToDouble(val)
                        End If
                    Else
                        ' 文字列等 → type=0
                        rec.SetteiType = 0
                        rec.ValText = If(IsDBNull(val), "", Convert.ToString(val))
                    End If

                    newRecords.Add(rec)
                Next
            Next

            ' t_settei から4プレフィックスのレコードを削除してINSERT
            Dim ownTransaction As Boolean = Not _crud.IsInTransaction

            Try
                If ownTransaction Then
                    _crud.BeginTransaction()
                End If

                Dim deleteSql As String =
                    "DELETE FROM t_settei " &
                    "WHERE settei_nm LIKE 'SWKSH_%' OR settei_nm LIKE 'SWKKJ_%' " &
                    "   OR settei_nm LIKE 'SWKSM_%' OR settei_nm LIKE 'SWKKY_%'"
                _crud.ExecuteNonQuery(deleteSql)

                For Each rec As SetteiRecord In newRecords
                    Dim insertSql As String =
                        "INSERT INTO t_settei (settei_id, settei_nm, settei_nm_jpn, settei_type, val_text, val_number, val_datetime, biko) " &
                        "VALUES (@settei_id, @settei_nm, @settei_nm_jpn, @settei_type, @val_text, @val_number, @val_datetime, @biko)"

                    Dim params As New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@settei_id", rec.SetteiId),
                        New NpgsqlParameter("@settei_nm", If(CObj(rec.SetteiNm), DBNull.Value)),
                        New NpgsqlParameter("@settei_nm_jpn", If(CObj(rec.SetteiNmJpn), DBNull.Value)),
                        New NpgsqlParameter("@settei_type", rec.SetteiType),
                        New NpgsqlParameter("@val_text", If(CObj(rec.ValText), DBNull.Value)),
                        New NpgsqlParameter("@val_number", If(rec.ValNumber.HasValue, CObj(rec.ValNumber.Value), DBNull.Value)),
                        New NpgsqlParameter("@val_datetime", If(rec.ValDatetime.HasValue, CObj(rec.ValDatetime.Value), DBNull.Value)),
                        New NpgsqlParameter("@biko", If(CObj(rec.Biko), DBNull.Value))
                    }

                    _crud.ExecuteNonQuery(insertSql, params)
                Next

                If ownTransaction Then
                    _crud.Commit()
                End If

            Catch ex As Exception
                If ownTransaction AndAlso _crud.IsInTransaction Then
                    Try
                        _crud.Rollback()
                    Catch
                    End Try
                End If
                Throw
            End Try

            Return True

        Catch ex As Exception
            Throw New Exception($"デフォルト設定の初期化に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ' ================================================================
    '  8. GetSettingValue  ← gGET_tmSETTEI_Element
    ' ================================================================

    ''' <summary>
    ''' 設定名を指定して設定値を取得する（型に応じた値を返却）
    ''' </summary>
    Public Function GetSettingValue(setteiNm As String) As Object
        If String.IsNullOrWhiteSpace(setteiNm) Then
            Throw New ArgumentException("設定名を指定してください。", NameOf(setteiNm))
        End If

        Dim sql As String = "SELECT settei_type, val_text, val_number, val_datetime FROM t_settei WHERE settei_nm = @nm"
        Dim params As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@nm", setteiNm)
        }

        Dim dt As DataTable = _crud.GetDataTable(sql, params)

        If dt.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim row As DataRow = dt.Rows(0)
        Dim setteiType As Integer = _crud.SafeConvert(Of Integer)(row("settei_type"))

        Select Case setteiType
            Case 0
                Return _crud.SafeConvert(Of String)(row("val_text"), "")
            Case 1
                If IsDBNull(row("val_number")) Then Return Nothing
                Return Convert.ToDouble(row("val_number"))
            Case 2
                If IsDBNull(row("val_datetime")) Then Return Nothing
                Return Convert.ToDateTime(row("val_datetime"))
            Case Else
                Return Nothing
        End Select
    End Function

    ' ================================================================
    '  9. CheckVersion  ← gCHK_VERSION_*
    ' ================================================================

    ''' <summary>
    ''' バージョンキーの値と期待値を比較する
    ''' </summary>
    Public Function CheckVersion(versionKey As String, expectedVersion As String) As Boolean
        If String.IsNullOrWhiteSpace(versionKey) Then
            Throw New ArgumentException("バージョンキーを指定してください。", NameOf(versionKey))
        End If

        Dim sql As String = "SELECT val_text FROM t_settei WHERE settei_nm = @key"
        Dim params As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@key", versionKey)
        }

        Dim dt As DataTable = _crud.GetDataTable(sql, params)

        If dt.Rows.Count = 0 Then
            Return False
        End If

        Dim actualVersion As String = _crud.SafeConvert(Of String)(dt.Rows(0)("val_text"), "")
        Return String.Equals(actualVersion, expectedVersion, StringComparison.Ordinal)
    End Function

    ' ================================================================
    '  10a. LoadFromWorkTable - ワークテーブル1行目をDictionary化
    ' ================================================================

    ''' <summary>
    ''' 指定ワークテーブルの1行目を Dictionary(Of String, Object) として返す。
    ''' キーは小文字カラム名。行が無い場合は空のDictionaryを返す。
    ''' </summary>
    Public Function LoadFromWorkTable(tableName As String) As Dictionary(Of String, Object)
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If

        Dim result As New Dictionary(Of String, Object)
        Dim quotedTable As String = """" & tableName & """"
        Dim dt As DataTable = _crud.GetDataTable($"SELECT * FROM {quotedTable} LIMIT 1")

        If dt.Rows.Count = 0 Then
            Return result
        End If

        Dim row As DataRow = dt.Rows(0)
        For Each col As DataColumn In dt.Columns
            result(col.ColumnName.ToLower()) = row(col)
        Next

        Return result
    End Function

    ' ================================================================
    '  10b. UpdateWorkTable - DictionaryからワークテーブルをUPDATE
    ' ================================================================

    ''' <summary>
    ''' Dictionary の値を指定ワークテーブルの1行目にUPDATEする。
    ''' キーはカラム名（小文字）。idカラムは除外。
    ''' </summary>
    Public Sub UpdateWorkTable(tableName As String, values As Dictionary(Of String, Object))
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If
        If values Is Nothing OrElse values.Count = 0 Then Return

        Dim quotedTable As String = """" & tableName & """"
        Dim setClauses As New List(Of String)
        Dim params As New List(Of NpgsqlParameter)
        Dim paramIndex As Integer = 0

        For Each kvp In values
            If kvp.Key.ToLower() = "id" Then Continue For

            paramIndex += 1
            Dim paramName As String = $"@p{paramIndex}"
            Dim quotedCol As String = """" & kvp.Key & """"
            setClauses.Add($"{quotedCol} = {paramName}")

            Dim val As Object = If(kvp.Value Is Nothing OrElse IsDBNull(kvp.Value), CObj(DBNull.Value), kvp.Value)
            params.Add(New NpgsqlParameter(paramName, val))
        Next

        If setClauses.Count = 0 Then Return

        Dim sql As String = $"UPDATE {quotedTable} SET {String.Join(", ", setClauses)} " &
                            $"WHERE id = (SELECT MIN(id) FROM {quotedTable})"
        _crud.ExecuteNonQuery(sql, params)
    End Sub

    ' ================================================================
    '  10c. ClearAllWorkTables - ヘルパー
    ' ================================================================

    ''' <summary>
    ''' 4ワークテーブルを全件削除する
    ''' </summary>
    Public Sub ClearAllWorkTables()
        _crud.ExecuteNonQuery($"DELETE FROM ""{WORK_TABLE_SH}""")
        _crud.ExecuteNonQuery($"DELETE FROM ""{WORK_TABLE_KJ}""")
        _crud.ExecuteNonQuery($"DELETE FROM ""{WORK_TABLE_SM}""")
        _crud.ExecuteNonQuery($"DELETE FROM ""{WORK_TABLE_KY}""")
    End Sub

    ' ================================================================
    '  11. Dispose - IDisposable
    ' ================================================================

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                If _crud IsNot Nothing Then
                    _crud.Dispose()
                    _crud = Nothing
                End If
            End If
            _disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' ================================================================
    '  Private ヘルパーメソッド
    ' ================================================================

    ''' <summary>
    ''' 設定名の先頭6文字からプレフィックスを取得する
    ''' </summary>
    Private Function GetPrefix(setteiNm As String) As String
        If String.IsNullOrEmpty(setteiNm) OrElse setteiNm.Length < 6 Then
            Return Nothing
        End If

        Dim prefix As String = setteiNm.Substring(0, 6).ToUpper()

        Select Case prefix
            Case PREFIX_SH, PREFIX_KJ, PREFIX_SM, PREFIX_KY
                Return prefix
            Case Else
                Return Nothing
        End Select
    End Function

    ''' <summary>
    ''' プレフィックスに対応するワークテーブル名を取得する
    ''' </summary>
    Private Function GetWorkTableByPrefix(prefix As String) As String
        Select Case prefix
            Case PREFIX_SH
                Return WORK_TABLE_SH
            Case PREFIX_KJ
                Return WORK_TABLE_KJ
            Case PREFIX_SM
                Return WORK_TABLE_SM
            Case PREFIX_KY
                Return WORK_TABLE_KY
            Case Else
                Return Nothing
        End Select
    End Function

    ''' <summary>
    ''' ワークテーブルにデフォルト行（1行）を挿入する
    ''' </summary>
    Private Sub InsertDefaultRow(tableName As String)
        Dim quotedTable As String = """" & tableName & """"
        _crud.ExecuteNonQuery($"INSERT INTO {quotedTable} DEFAULT VALUES")
    End Sub

    ''' <summary>
    ''' カラム名がboolean型カラムかどうかを判定する
    ''' </summary>
    Private Shared Function IsBooleanColumn(columnName As String) As Boolean
        If String.IsNullOrEmpty(columnName) Then Return False

        Dim lowerName As String = columnName.ToLower()
        For Each suffix As String In BooleanSuffixes
            If lowerName.EndsWith(suffix) Then
                Return True
            End If
        Next
        Return False
    End Function

End Class
