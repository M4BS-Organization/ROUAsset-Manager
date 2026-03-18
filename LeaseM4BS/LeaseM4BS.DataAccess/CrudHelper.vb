Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Text
Imports Npgsql

''' <summary>
''' PostgreSQL データベースに対する汎用的な CRUD 操作を提供するヘルパークラス
''' DAO.Recordset の代替として使用します
''' </summary>
Public Class CrudHelper
    Implements IDisposable

    Private _connectionManager As DbConnectionManager
    Private _disposed As Boolean = False
    Private _activeConnection As NpgsqlConnection = Nothing
    Private _activeTransaction As NpgsqlTransaction = Nothing

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    Public Sub New()
        _connectionManager = New DbConnectionManager()
    End Sub

    ''' <summary>
    ''' 接続文字列を指定するコンストラクタ
    ''' </summary>
    Public Sub New(connectionString As String)
        _connectionManager = New DbConnectionManager(connectionString)
    End Sub

    ''' <summary>
    ''' DbConnectionManager を指定するコンストラクタ
    ''' </summary>
    Public Sub New(connectionManager As DbConnectionManager)
        If connectionManager Is Nothing Then
            Throw New ArgumentNullException(NameOf(connectionManager))
        End If
        _connectionManager = connectionManager
    End Sub

    ''' <summary>
    ''' SQL クエリを実行し、結果を DataTable として取得します
    ''' </summary>
    Public Function GetDataTable(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As DataTable
        If String.IsNullOrWhiteSpace(sql) Then
            Throw New ArgumentException("SQL クエリを指定してください。", NameOf(sql))
        End If

        Dim dataTable As New DataTable()
        Dim conn As NpgsqlConnection = Nothing
        Dim useInternalConnection As Boolean = False

        Try

            ' トランザクション中の場合は既存の接続を使用
            If _activeConnection IsNot Nothing AndAlso _activeTransaction IsNot Nothing Then
                conn = _activeConnection
            Else
                conn = _connectionManager.GetConnection()
                useInternalConnection = True
            End If

            Using cmd As New NpgsqlCommand(sql, conn)
                If _activeTransaction IsNot Nothing Then
                    cmd.Transaction = _activeTransaction
                End If

                ' パラメータを追加（Cloneを使用）
                If parameters IsNot Nothing Then
                    For Each param As NpgsqlParameter In parameters
                        cmd.Parameters.Add(CType(param.Clone(), NpgsqlParameter))
                    Next
                End If

                Using adapter As New NpgsqlDataAdapter(cmd)
                    adapter.Fill(dataTable)
                End Using
            End Using

        Catch ex As Exception
            Dim detailedMsg As String = CreateErrorMessage(ex, sql, parameters)
            Throw New Exception(detailedMsg, ex)
        Finally
            If useInternalConnection AndAlso conn IsNot Nothing Then
                conn.Dispose()
            End If
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' INSERT、UPDATE、DELETE などの非クエリ SQL コマンドを実行します
    ''' </summary>
    Public Function ExecuteNonQuery(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As Integer
        If String.IsNullOrWhiteSpace(sql) Then
            Throw New ArgumentException("SQL コマンドを指定してください。", NameOf(sql))
        End If

        Dim rowsAffected As Integer = 0
        Dim conn As NpgsqlConnection = Nothing
        Dim useInternalConnection As Boolean = False

        Try

            ' トランザクション中の場合は既存の接続を使用
            If _activeConnection IsNot Nothing AndAlso _activeTransaction IsNot Nothing Then
                conn = _activeConnection
            Else
                conn = _connectionManager.GetConnection()
                useInternalConnection = True
            End If

            Using cmd As New NpgsqlCommand(sql, conn)
                If _activeTransaction IsNot Nothing Then
                    cmd.Transaction = _activeTransaction
                End If

                ' パラメータを追加（Cloneを使用）
                If parameters IsNot Nothing Then
                    For Each param As NpgsqlParameter In parameters
                        cmd.Parameters.Add(CType(param.Clone(), NpgsqlParameter))
                    Next
                End If

                rowsAffected = cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Dim detailedMsg As String = CreateErrorMessage(ex, sql, parameters)
            Throw New Exception(detailedMsg, ex)
        Finally
            If useInternalConnection AndAlso conn IsNot Nothing Then
                conn.Dispose()
            End If
        End Try

        Return rowsAffected
    End Function

    ''' <summary>
    ''' SQLを実行し、単一の値を指定した型で取得します。（NULLの場合はデフォルト値を返します）
    ''' 使用例: Dim count = db.ExecuteScalar(Of Integer)("SELECT count(*) FROM ...")
    ''' </summary>
    Public Function ExecuteScalar(Of T)(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As T
        If String.IsNullOrWhiteSpace(sql) Then
            Throw New ArgumentException("SQL クエリを指定してください。", NameOf(sql))
        End If

        Dim result As Object = Nothing
        Dim conn As NpgsqlConnection = Nothing
        Dim useInternalConnection As Boolean = False

        Try

            ' トランザクション中の場合は既存の接続を使用
            If _activeConnection IsNot Nothing AndAlso _activeTransaction IsNot Nothing Then
                conn = _activeConnection
            Else
                conn = _connectionManager.GetConnection()
                useInternalConnection = True
            End If

            Using cmd As New NpgsqlCommand(sql, conn)
                If _activeTransaction IsNot Nothing Then
                    cmd.Transaction = _activeTransaction
                End If

                ' パラメータを追加（Cloneを使用）
                If parameters IsNot Nothing Then
                    For Each param As NpgsqlParameter In parameters
                        cmd.Parameters.Add(CType(param.Clone(), NpgsqlParameter))
                    Next
                End If

                result = cmd.ExecuteScalar()
            End Using

        Catch ex As Exception
            Dim detailedMsg As String = CreateErrorMessage(ex, sql, parameters)
            Throw New Exception(detailedMsg, ex)
        Finally
            If useInternalConnection AndAlso conn IsNot Nothing Then
                conn.Dispose()
            End If
        End Try

        ' === NULL安全な型変換 ===
        If result Is Nothing OrElse IsDBNull(result) Then
            Return Nothing ' TがIntegerなら0, StringならNothing, BooleanならFalse
        End If

        ' 取得した値を指定の型(T)に変換して返す
        Return CType(result, T)
    End Function
    ' CrudHelper.vb に追加

    ''' <summary>
    ''' DBの値を安全に指定の型に変換します（DBNullの場合はデフォルト値を返します）
    ''' 使用例: Dim name = db.SafeConvert(Of String)(row("user_name"))
    ''' </summary>
    Public Function SafeConvert(Of T)(value As Object, Optional defaultValue As T = Nothing) As T
        If value Is Nothing OrElse IsDBNull(value) Then
            Return defaultValue
        End If
        Try
            Return CType(value, T)
        Catch
            Return defaultValue
        End Try
    End Function
    ''' <summary>
    ''' エラー発生時に、実行しようとしたSQLとパラメータ情報を文字列化します
    ''' </summary>
    Private Function CreateErrorMessage(ex As Exception, sql As String, parameters As List(Of NpgsqlParameter)) As String
        Dim sb As New StringBuilder()
        sb.AppendLine($"エラー内容: {ex.Message}")
        
        ' 修正箇所: 文字列を作成してから AppendLine します
        sb.AppendLine(New String("-"c, 20))
        
        sb.AppendLine($"[実行SQL]:")
        sb.AppendLine(sql)
        
        ' 修正箇所: 文字列を作成してから AppendLine します
        sb.AppendLine(New String("-"c, 20))
        
        sb.AppendLine($"[パラメータ]:")

        If parameters IsNot Nothing AndAlso parameters.Count > 0 Then
            For Each p In parameters
                Dim val As String = If(p.Value Is Nothing OrElse IsDBNull(p.Value), "NULL", p.Value.ToString())
                sb.AppendLine($"  {p.ParameterName} = {val}")
            Next
        Else
            sb.AppendLine("  (なし)")
        End If

        Return sb.ToString()
    End Function

    ' ----------------------------------------------------------------
    '  以下、Insert, Update, Delete, Exists, Transaction 関連メソッド
    ' ----------------------------------------------------------------

    ''' <summary>
    ''' テーブルにレコードを挿入します
    ''' </summary>
    Public Function Insert(tableName As String, columnValues As Dictionary(Of String, Object)) As Integer
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If

        If columnValues Is Nothing OrElse columnValues.Count = 0 Then
            Throw New ArgumentException("挿入する列と値を指定してください。", NameOf(columnValues))
        End If

        Dim columns As New List(Of String)
        Dim paramNames As New List(Of String)
        Dim parameters As New List(Of NpgsqlParameter)

        For Each kvp As KeyValuePair(Of String, Object) In columnValues
            columns.Add(kvp.Key)
            Dim paramName As String = $"@{kvp.Key}"
            paramNames.Add(paramName)
            parameters.Add(New NpgsqlParameter(paramName, If(kvp.Value, DBNull.Value)))
        Next

        Dim sql As String = $"INSERT INTO {tableName} ({String.Join(", ", columns)}) VALUES ({String.Join(", ", paramNames)})"

        Return ExecuteNonQuery(sql, parameters)
    End Function

    ''' <summary>
    ''' テーブルのレコードを更新します
    ''' </summary>
    Public Function Update(tableName As String, columnValues As Dictionary(Of String, Object), whereClause As String, Optional whereParameters As List(Of NpgsqlParameter) = Nothing) As Integer
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If

        If columnValues Is Nothing OrElse columnValues.Count = 0 Then
            Throw New ArgumentException("更新する列と値を指定してください。", NameOf(columnValues))
        End If

        If String.IsNullOrWhiteSpace(whereClause) Then
            Throw New ArgumentException("WHERE 句を指定してください。全レコード更新を防ぐため、WHERE 句は必須です。", NameOf(whereClause))
        End If

        Dim setClauses As New List(Of String)
        Dim parameters As New List(Of NpgsqlParameter)

        For Each kvp As KeyValuePair(Of String, Object) In columnValues
            Dim paramName As String = $"@set_{kvp.Key}"
            setClauses.Add($"{kvp.Key} = {paramName}")
            parameters.Add(New NpgsqlParameter(paramName, If(kvp.Value, DBNull.Value)))
        Next

        If whereParameters IsNot Nothing Then
            parameters.AddRange(whereParameters)
        End If

        Dim sql As String = $"UPDATE {tableName} SET {String.Join(", ", setClauses)} WHERE {whereClause}"

        Return ExecuteNonQuery(sql, parameters)
    End Function

    ''' <summary>
    ''' テーブルからレコードを削除します
    ''' </summary>
    Public Function Delete(tableName As String, whereClause As String, Optional whereParameters As List(Of NpgsqlParameter) = Nothing) As Integer
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If

        If String.IsNullOrWhiteSpace(whereClause) Then
            Throw New ArgumentException("WHERE 句を指定してください。全レコード削除を防ぐため、WHERE 句は必須です。", NameOf(whereClause))
        End If

        Dim sql As String = $"DELETE FROM {tableName} WHERE {whereClause}"

        Return ExecuteNonQuery(sql, whereParameters)
    End Function

    ''' <summary>
    ''' レコードが存在するかチェックします
    ''' </summary>
    Public Function Exists(tableName As String, whereClause As String, Optional whereParameters As List(Of NpgsqlParameter) = Nothing) As Boolean
        If String.IsNullOrWhiteSpace(tableName) Then
            Throw New ArgumentException("テーブル名を指定してください。", NameOf(tableName))
        End If

        If String.IsNullOrWhiteSpace(whereClause) Then
            Throw New ArgumentException("WHERE 句を指定してください。", NameOf(whereClause))
        End If

        Dim sql As String = $"SELECT COUNT(*) FROM {tableName} WHERE {whereClause}"
        Dim result As Object = ExecuteScalar(Of Object)(sql, whereParameters)

        If result IsNot Nothing AndAlso Not IsDBNull(result) Then
            Return Convert.ToInt32(result) > 0
        End If

        Return False
    End Function

    ''' <summary>
    ''' トランザクションを開始します
    ''' </summary>
    Public Sub BeginTransaction()
        If _activeConnection IsNot Nothing Then
            Throw New InvalidOperationException("トランザクションは既に開始されています。")
        End If

        Try
            _activeConnection = _connectionManager.GetConnection()
            _activeTransaction = _activeConnection.BeginTransaction()
        Catch ex As Exception
            If _activeConnection IsNot Nothing Then
                _activeConnection.Dispose()
                _activeConnection = Nothing
            End If
            Throw New InvalidOperationException($"トランザクションの開始に失敗しました: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' トランザクションをコミットします
    ''' </summary>
    Public Sub Commit()
        If _activeTransaction Is Nothing Then
            Throw New InvalidOperationException("トランザクションが開始されていません。")
        End If

        Try
            _activeTransaction.Commit()
        Catch ex As Exception
            Throw New InvalidOperationException($"トランザクションのコミットに失敗しました: {ex.Message}", ex)
        Finally
            CleanupTransaction()
        End Try
    End Sub

    ''' <summary>
    ''' トランザクションをロールバックします
    ''' </summary>
    Public Sub Rollback()
        If _activeTransaction Is Nothing Then
            Throw New InvalidOperationException("トランザクションが開始されていません。")
        End If

        Try
            _activeTransaction.Rollback()
        Catch ex As Exception
            Throw New InvalidOperationException($"トランザクションのロールバックに失敗しました: {ex.Message}", ex)
        Finally
            CleanupTransaction()
        End Try
    End Sub

    ''' <summary>
    ''' トランザクションが進行中かどうかを取得します
    ''' </summary>
    Public ReadOnly Property IsInTransaction As Boolean
        Get
            Return _activeTransaction IsNot Nothing
        End Get
    End Property

    Private Sub CleanupTransaction()
        If _activeTransaction IsNot Nothing Then
            _activeTransaction.Dispose()
            _activeTransaction = Nothing
        End If

        If _activeConnection IsNot Nothing Then
            _activeConnection.Dispose()
            _activeConnection = Nothing
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                If _activeTransaction IsNot Nothing Then
                    Try
                        _activeTransaction.Rollback()
                    Catch rollbackEx As Exception
                        DbConnectionManager.WriteError($"Rollback失敗: {rollbackEx.Message}", rollbackEx)
                    End Try
                End If

                CleanupTransaction()

                If _connectionManager IsNot Nothing Then
                    _connectionManager.Dispose()
                    _connectionManager = Nothing
                End If
            End If
            _disposed = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

End Class