Imports System.Data
Imports System.Collections.Generic
Imports Npgsql

''' <summary>
''' tw_lease_accounting テーブルに対する CRUD 操作を提供するリポジトリクラス
''' </summary>
Public Class LeaseAccountingRepository

    Private Const TABLE_NAME As String = "tw_lease_accounting"

    ''' <summary>
    ''' 契約IDに紐づく会計レコードを取得します
    ''' </summary>
    ''' <param name="contractId">契約ID</param>
    ''' <returns>該当する会計データを含む DataTable</returns>
    Public Function GetByContractId(contractId As Integer) As DataTable
        Try
            Using helper As New CrudHelper()
                Dim sql As String = $"SELECT * FROM {TABLE_NAME} WHERE contract_id = @contract_id ORDER BY contract_id"
                Dim parameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@contract_id", contractId)
                }
                Return helper.GetDataTable(sql, parameters)
            End Using
        Catch ex As Exception
            Throw New Exception($"契約ID {contractId} の会計データ取得に失敗しました: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' 契約IDに紐づく会計レコードを挿入または更新します（Upsert）。
    ''' 既存レコードがある場合は更新、なければ挿入します。
    ''' </summary>
    ''' <param name="contractId">契約ID</param>
    ''' <param name="values">カラム名と値の辞書（contract_id は自動設定されます）</param>
    ''' <returns>影響を受けた行数</returns>
    Public Function Upsert(contractId As Integer, values As Dictionary(Of String, Object)) As Integer
        If values Is Nothing OrElse values.Count = 0 Then
            Throw New ArgumentException("挿入/更新するデータを指定してください。", NameOf(values))
        End If

        Try
            Using helper As New CrudHelper()
                ' 既存レコードの存在チェック
                Dim whereClause As String = "contract_id = @where_contract_id"
                Dim whereParameters As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@where_contract_id", contractId)
                }
                Dim exists As Boolean = helper.Exists(TABLE_NAME, whereClause, whereParameters)

                If exists Then
                    ' 更新: values から contract_id を除外して更新対象にする
                    Dim updateValues As New Dictionary(Of String, Object)(values)
                    updateValues.Remove("contract_id")

                    If updateValues.Count = 0 Then
                        Return 0
                    End If

                    Dim updateWhereParams As New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@where_contract_id", contractId)
                    }
                    Return helper.Update(TABLE_NAME, updateValues, "contract_id = @where_contract_id", updateWhereParams)
                Else
                    ' 挿入: contract_id を値に含める
                    Dim insertValues As New Dictionary(Of String, Object)(values)
                    insertValues("contract_id") = contractId
                    Return helper.Insert(TABLE_NAME, insertValues)
                End If
            End Using
        Catch ex As Exception
            Throw New Exception($"契約ID {contractId} の会計データ Upsert に失敗しました: {ex.Message}", ex)
        End Try
    End Function

End Class
