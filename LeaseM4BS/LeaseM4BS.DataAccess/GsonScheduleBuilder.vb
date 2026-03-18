' 減損スケジュール生成 (Access版 pc_注記.gMake減損_SCH 相当)
' d_gson テーブルから減損情報を読み取り、GsonScheduleEntry のリストを生成する

Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

''' <summary>
''' 減損スケジュール生成 (Access版 pc_注記.gMake減損_SCH)
''' </summary>
Public Class GsonScheduleBuilder

    ''' <summary>
    ''' 減損スケジュールを生成する (Access版 gMake減損_SCH)
    ''' d_gson テーブルの減損レコードから GsonScheduleEntry リストを作成する
    ''' </summary>
    ''' <param name="crud">CrudHelper インスタンス</param>
    ''' <param name="kykmId">物件ID</param>
    ''' <returns>減損スケジュールリスト</returns>
    Public Shared Function Build(
        crud As CrudHelper,
        kykmId As Double
    ) As List(Of GsonScheduleEntry)

        Dim result As New List(Of GsonScheduleEntry)()

        ' d_gson から減損データを取得
        Dim sql As String = "SELECT gson_dt, gson_tmg, gson_ryo, gson_rkei " &
                            "FROM d_gson " &
                            "WHERE kykm_id = @kykm_id " &
                            "ORDER BY gson_dt"

        Dim params As New List(Of NpgsqlParameter)()
        params.Add(New NpgsqlParameter("@kykm_id", kykmId))

        Dim dt As DataTable = crud.GetDataTable(sql, params)

        For Each row As DataRow In dt.Rows
            If IsDBNull(row("gson_dt")) Then Continue For
            Dim entry As New GsonScheduleEntry()

            Dim gsonDt As Date = CDate(row("gson_dt"))
            entry.Nen = gsonDt.Year
            entry.Getu = gsonDt.Month

            Dim gsonTmg As Integer = SafeConv(Of Integer)(row("gson_tmg"), 0)
            entry.GsonTmg = gsonTmg

            Dim gsonRyo As Double = SafeConv(Of Double)(row("gson_ryo"), 0)
            Dim gsonRkei As Double = SafeConv(Of Double)(row("gson_rkei"), 0)

            Select Case gsonTmg
                Case 0  ' 月度末
                    entry.GsonRyoS = 0
                    entry.GsonRyoE = gsonRyo
                    entry.GsonRkeiS = gsonRkei - gsonRyo
                Case 1  ' 月度初
                    entry.GsonRyoS = gsonRyo
                    entry.GsonRyoE = 0
                    entry.GsonRkeiS = gsonRkei
                Case Else
                    Throw New Exception($"GsonScheduleBuilder: 不正なGSON_TMG値={gsonTmg}")
            End Select

            entry.GsonRkeiE = gsonRkei

            result.Add(entry)
        Next

        Return result
    End Function

    ''' <summary>
    ''' DataRow配列から減損スケジュールを生成する (レコードセット未使用版)
    ''' </summary>
    Public Shared Function BuildFromRows(
        rows As DataRowCollection
    ) As List(Of GsonScheduleEntry)

        Dim result As New List(Of GsonScheduleEntry)()
        If rows Is Nothing OrElse rows.Count = 0 Then Return result

        For Each row As DataRow In rows
            If IsDBNull(row("gson_dt")) Then Continue For
            Dim entry As New GsonScheduleEntry()

            Dim gsonDt As Date = CDate(row("gson_dt"))
            entry.Nen = gsonDt.Year
            entry.Getu = gsonDt.Month

            Dim gsonTmg As Integer = SafeConv(Of Integer)(row("gson_tmg"), 0)
            entry.GsonTmg = gsonTmg

            Dim gsonRyo As Double = SafeConv(Of Double)(row("gson_ryo"), 0)
            Dim gsonRkei As Double = SafeConv(Of Double)(row("gson_rkei"), 0)

            Select Case gsonTmg
                Case 0
                    entry.GsonRyoS = 0
                    entry.GsonRyoE = gsonRyo
                    entry.GsonRkeiS = gsonRkei - gsonRyo
                Case 1
                    entry.GsonRyoS = gsonRyo
                    entry.GsonRyoE = 0
                    entry.GsonRkeiS = gsonRkei
                Case Else
                    Throw New Exception($"GsonScheduleBuilder: 不正なGSON_TMG値={gsonTmg}")
            End Select

            entry.GsonRkeiE = gsonRkei

            result.Add(entry)
        Next

        Return result
    End Function

    ''' <summary>DBNull安全変換ヘルパー</summary>
    Private Shared Function SafeConv(Of T)(value As Object, Optional defaultValue As T = Nothing) As T
        If value Is Nothing OrElse IsDBNull(value) Then Return defaultValue
        Try
            Return CType(Convert.ChangeType(value, GetType(T)), T)
        Catch
            Return defaultValue
        End Try
    End Function

End Class
