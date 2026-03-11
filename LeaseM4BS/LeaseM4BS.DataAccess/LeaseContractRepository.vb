Imports System.Data
Imports System.Collections.Generic
Imports Npgsql

''' <summary>
''' tw_lease_contract テーブルに対する CRUD 操作を提供するリポジトリクラス
''' </summary>
Public Class LeaseContractRepository

    Private Const TABLE_NAME As String = "tw_lease_contract"

    ''' <summary>
    ''' 全契約レコードを取得します
    ''' </summary>
    ''' <returns>全契約データを含む DataTable</returns>
    Public Function GetAll() As DataTable
        Try
            Using helper As New CrudHelper()
                Dim sql As String = $"SELECT * FROM {TABLE_NAME} ORDER BY contract_id"
                Return helper.GetDataTable(sql)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約一覧の取得に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 契約番号で契約レコードを検索します
    ''' </summary>
    ''' <param name="contractNo">契約番号</param>
    ''' <returns>該当する契約データを含む DataTable</returns>
    Public Function GetByContractNo(contractNo As String) As DataTable
        If String.IsNullOrWhiteSpace(contractNo) Then
            Throw New ArgumentException("契約番号を指定してください。", NameOf(contractNo))
        End If

        Try
            Using helper As New CrudHelper()
                Dim sql As String = $"SELECT * FROM {TABLE_NAME} WHERE contract_no = @contract_no"
                Dim parameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@contract_no", contractNo)
                }
                Return helper.GetDataTable(sql, parameters)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約番号 '{contractNo}' の検索に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 契約レコードを新規挿入します
    ''' </summary>
    ''' <param name="values">カラム名と値の辞書</param>
    ''' <returns>挿入された行数</returns>
    Public Function Insert(values As Dictionary(Of String, Object)) As Integer
        If values Is Nothing OrElse values.Count = 0 Then
            Throw New ArgumentException("挿入するデータを指定してください。", NameOf(values))
        End If

        Try
            Using helper As New CrudHelper()
                Return helper.Insert(TABLE_NAME, values)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約レコードの挿入に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 契約IDを指定して契約レコードを更新します
    ''' </summary>
    ''' <param name="contractId">契約ID</param>
    ''' <param name="values">更新するカラム名と値の辞書</param>
    ''' <returns>更新された行数</returns>
    Public Function Update(contractId As Integer, values As Dictionary(Of String, Object)) As Integer
        If values Is Nothing OrElse values.Count = 0 Then
            Throw New ArgumentException("更新するデータを指定してください。", NameOf(values))
        End If

        Try
            Using helper As New CrudHelper()
                Dim whereClause As String = "contract_id = @where_contract_id"
                Dim whereParameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@where_contract_id", contractId)
                }
                Return helper.Update(TABLE_NAME, values, whereClause, whereParameters)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約ID {contractId} の更新に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 契約IDを指定して契約レコードを削除します
    ''' </summary>
    ''' <param name="contractId">契約ID</param>
    ''' <returns>削除された行数</returns>
    Public Function Delete(contractId As Integer) As Integer
        Try
            Using helper As New CrudHelper()
                Dim whereClause As String = "contract_id = @where_contract_id"
                Dim whereParameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@where_contract_id", contractId)
                }
                Return helper.Delete(TABLE_NAME, whereClause, whereParameters)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約ID {contractId} の削除に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 指定カラムと値で契約レコードを検索します
    ''' </summary>
    ''' <param name="searchColumn">検索対象のカラム名</param>
    ''' <param name="searchValue">検索値（部分一致）</param>
    ''' <returns>検索結果を含む DataTable</returns>
    Public Function SearchContracts(searchColumn As String, searchValue As String) As DataTable
        If String.IsNullOrWhiteSpace(searchColumn) Then
            Throw New ArgumentException("検索カラム名を指定してください。", NameOf(searchColumn))
        End If

        If searchValue Is Nothing Then
            Throw New ArgumentNullException(NameOf(searchValue), "検索値を指定してください。")
        End If

        Try
            Using helper As New CrudHelper()
                ' カラム名はホワイトリスト検証が望ましいが、呼び出し元で制御する前提
                Dim sql As String = $"SELECT * FROM {TABLE_NAME} WHERE {searchColumn} LIKE @search_value ORDER BY contract_id"
                Dim parameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@search_value", $"%{searchValue}%")
                }
                Return helper.GetDataTable(sql, parameters)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約の検索に失敗しました (カラム: {searchColumn}, 値: {searchValue}): {ex.Message}", ex)
        End Try
    End Function

End Class
