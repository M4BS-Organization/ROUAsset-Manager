' 計上ワークテーブル管理 (Access版 tw_S_CHUKI_KEIJO / tw_D_HENL_KEIJO / tw_D_GSON_KEIJO)
' ワークテーブルへの書込・クリア・取得を担当

Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

''' <summary>
''' 計上ワークテーブル管理クラス
''' KeijoCalculationEngine の計算結果をワークテーブルに書き込む。
''' </summary>
Public Class KeijoWorkTableManager

    Private _crud As CrudHelper

    Public Sub New(crud As CrudHelper)
        _crud = crud
    End Sub

    Public Sub New()
        _crud = New CrudHelper()
    End Sub

    ' ======================================================================
    '  ワークテーブルクリア
    ' ======================================================================

    ''' <summary>tw_s_chuki_keijo 全件削除</summary>
    Public Sub ClearChukiKeijo()
        _crud.ExecuteNonQuery("DELETE FROM tw_s_chuki_keijo")
    End Sub

    ''' <summary>tw_d_henl_keijo 全件削除</summary>
    Public Sub ClearHenlKeijo()
        _crud.ExecuteNonQuery("DELETE FROM tw_d_henl_keijo")
    End Sub

    ''' <summary>tw_d_gson_keijo 全件削除</summary>
    Public Sub ClearGsonKeijo()
        _crud.ExecuteNonQuery("DELETE FROM tw_d_gson_keijo")
    End Sub

    ''' <summary>全ワークテーブルクリア</summary>
    Public Sub ClearAll()
        ClearChukiKeijo()
        ClearHenlKeijo()
        ClearGsonKeijo()
    End Sub

    ' ======================================================================
    '  ワーク行書込
    ' ======================================================================

    ''' <summary>
    ''' KeijoWorkRow リストをワークテーブルに一括書込
    ''' TargetTable プロパティに応じて出力先を振り分ける。
    ''' </summary>
    Public Sub InsertWorkRows(rows As List(Of KeijoWorkRow))
        If rows Is Nothing OrElse rows.Count = 0 Then Return

        For Each row As KeijoWorkRow In rows
            Select Case row.TargetTable
                Case KeijoTargetTable.ChukiKeijo
                    InsertChukiKeijo(row)
                Case KeijoTargetTable.HenlKeijo
                    InsertHenlKeijo(row)
            End Select
        Next
    End Sub

    ''' <summary>注記計上ワーク1行書込</summary>
    Public Sub InsertChukiKeijo(row As KeijoWorkRow)
        InsertKeijoRow("tw_s_chuki_keijo", row)
    End Sub

    ''' <summary>変額仕訳ワーク1行書込</summary>
    Public Sub InsertHenlKeijo(row As KeijoWorkRow)
        InsertKeijoRow("tw_d_henl_keijo", row)
    End Sub

    ''' <summary>共通 INSERT ロジック (tw_s_chuki_keijo / tw_d_henl_keijo 共通スキーマ)</summary>
    Private Sub InsertKeijoRow(tableName As String, row As KeijoWorkRow)
        Dim sql As String =
            $"INSERT INTO {tableName} (" &
            "kykh_id, kykm_id, kykm_no, saikaisu_kykm, bukn_nm, kykbnl_no, " &
            "kkbn_id, kjkbn_id, leakbn_id, lcpt_id, " &
            "hensai_kind, rec_kbn, " &
            "shri_dt, sime_dt, lsryo, zei, zritu, shho_id, mae_f, ckaiyk_f, " &
            "keijo_f, keijo_dt, sumikaisu_zen, keijo_shri_cnt, " &
            "lsryo_total, lsryo_toki, zei_total, zei_toki, " &
            "line_id, haifritu, hkmk_id, h_bcat_id, rsrvh1_id, " &
            "h_zokusei1, h_zokusei2, h_zokusei3, h_zokusei4, h_zokusei5, " &
            "mae_zou, mae_gen, mzei_zou, mzei_gen, " &
            "gson_dt, shori_dt" &
            ") VALUES (" &
            "@kykh_id, @kykm_id, @kykm_no, @saikaisu_kykm, @bukn_nm, @kykbnl_no, " &
            "@kkbn_id, @kjkbn_id, @leakbn_id, @lcpt_id, " &
            "@hensai_kind, @rec_kbn, " &
            "@shri_dt, @sime_dt, @lsryo, @zei, @zritu, @shho_id, @mae_f, @ckaiyk_f, " &
            "@keijo_f, @keijo_dt, @sumikaisu_zen, @keijo_shri_cnt, " &
            "@lsryo_total, @lsryo_toki, @zei_total, @zei_toki, " &
            "@line_id, @haifritu, @hkmk_id, @h_bcat_id, @rsrvh1_id, " &
            "@h_zokusei1, @h_zokusei2, @h_zokusei3, @h_zokusei4, @h_zokusei5, " &
            "@mae_zou, @mae_gen, @mzei_zou, @mzei_gen, " &
            "@gson_dt, @shori_dt)"

        Dim params As New List(Of NpgsqlParameter)()
        params.Add(New NpgsqlParameter("@kykh_id", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.KykhId})
        params.Add(New NpgsqlParameter("@kykm_id", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.KykmId})
        params.Add(New NpgsqlParameter("@kykm_no", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.KykmNo})
        params.Add(MakeParam("@saikaisu_kykm", row.SaikaisuKykm))
        params.Add(New NpgsqlParameter("@bukn_nm", NpgsqlTypes.NpgsqlDbType.Varchar) With {.Value = If(row.BuknNm, "")})
        params.Add(New NpgsqlParameter("@kykbnl_no", NpgsqlTypes.NpgsqlDbType.Varchar) With {.Value = If(row.KykbnlNo, "")})
        params.Add(MakeParam("@kkbn_id", row.KkbnId))
        params.Add(MakeParam("@kjkbn_id", row.KjkbnId))
        params.Add(MakeParam("@leakbn_id", row.LeakbnId))
        params.Add(MakeParam("@lcpt_id", row.LcptId))
        params.Add(New NpgsqlParameter("@hensai_kind", NpgsqlTypes.NpgsqlDbType.Integer) With {.Value = CInt(row.HensaiKind)})
        params.Add(New NpgsqlParameter("@rec_kbn", NpgsqlTypes.NpgsqlDbType.Integer) With {.Value = CInt(row.RecKbn)})
        params.Add(New NpgsqlParameter("@shri_dt", NpgsqlTypes.NpgsqlDbType.Date) With {.Value = row.ShriDt})
        params.Add(New NpgsqlParameter("@sime_dt", NpgsqlTypes.NpgsqlDbType.Date) With {.Value = row.SimeDt})
        params.Add(New NpgsqlParameter("@lsryo", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.Lsryo})
        params.Add(New NpgsqlParameter("@zei", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.Zei})
        params.Add(New NpgsqlParameter("@zritu", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.Zritu})
        params.Add(MakeParam("@shho_id", row.ShhoId))
        params.Add(New NpgsqlParameter("@mae_f", NpgsqlTypes.NpgsqlDbType.Boolean) With {.Value = row.MaeF})
        params.Add(New NpgsqlParameter("@ckaiyk_f", NpgsqlTypes.NpgsqlDbType.Boolean) With {.Value = row.CkaiykF})
        params.Add(New NpgsqlParameter("@keijo_f", NpgsqlTypes.NpgsqlDbType.Boolean) With {.Value = row.KeijoF})
        params.Add(MakeParam("@keijo_dt", row.KejoDt))
        params.Add(New NpgsqlParameter("@sumikaisu_zen", NpgsqlTypes.NpgsqlDbType.Integer) With {.Value = row.SumikaisuZen})
        params.Add(New NpgsqlParameter("@keijo_shri_cnt", NpgsqlTypes.NpgsqlDbType.Integer) With {.Value = row.KeijoShriCnt})
        params.Add(New NpgsqlParameter("@lsryo_total", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.LsryoTotal})
        params.Add(New NpgsqlParameter("@lsryo_toki", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.LsryoToki})
        params.Add(New NpgsqlParameter("@zei_total", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.ZeiTotal})
        params.Add(New NpgsqlParameter("@zei_toki", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.ZeiToki})
        params.Add(MakeParam("@line_id", row.LineId))
        params.Add(MakeParam("@haifritu", row.Haifritu))
        params.Add(MakeParam("@hkmk_id", row.HkmkId))
        params.Add(MakeParam("@h_bcat_id", row.HBcatId))
        params.Add(MakeParam("@rsrvh1_id", row.Rsrvh1Id))
        params.Add(MakeParam("@h_zokusei1", row.HZokusei1))
        params.Add(MakeParam("@h_zokusei2", row.HZokusei2))
        params.Add(MakeParam("@h_zokusei3", row.HZokusei3))
        params.Add(MakeParam("@h_zokusei4", row.HZokusei4))
        params.Add(MakeParam("@h_zokusei5", row.HZokusei5))
        params.Add(New NpgsqlParameter("@mae_zou", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.MaeZou})
        params.Add(New NpgsqlParameter("@mae_gen", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.MaeGen})
        params.Add(New NpgsqlParameter("@mzei_zou", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.MzeiZou})
        params.Add(New NpgsqlParameter("@mzei_gen", NpgsqlTypes.NpgsqlDbType.Double) With {.Value = row.MzeiGen})
        params.Add(MakeParam("@gson_dt", row.GsonDt))
        params.Add(New NpgsqlParameter("@shori_dt", NpgsqlTypes.NpgsqlDbType.Date) With {.Value = row.ShoriDt})

        _crud.ExecuteNonQuery(sql, params)
    End Sub

    ' ======================================================================
    '  ワークテーブル取得
    ' ======================================================================

    ''' <summary>tw_s_chuki_keijo 全件取得（結果表示用）</summary>
    Public Function GetChukiKeijoAll() As DataTable
        Return _crud.GetDataTable("SELECT * FROM tw_s_chuki_keijo ORDER BY kykm_id, keijo_dt")
    End Function

    ''' <summary>tw_d_henl_keijo 全件取得</summary>
    Public Function GetHenlKeijoAll() As DataTable
        Return _crud.GetDataTable("SELECT * FROM tw_d_henl_keijo ORDER BY kykm_id, keijo_dt")
    End Function

    ''' <summary>tw_d_gson_keijo 全件取得</summary>
    Public Function GetGsonKeijoAll() As DataTable
        Return _crud.GetDataTable("SELECT * FROM tw_d_gson_keijo ORDER BY kykm_id, keijo_dt")
    End Function

    ''' <summary>tw_s_chuki_keijo 件数取得</summary>
    Public Function GetChukiKeijoCount() As Integer
        Return _crud.ExecuteScalar(Of Integer)("SELECT COUNT(*) FROM tw_s_chuki_keijo")
    End Function

    ''' <summary>tw_d_henl_keijo 件数取得</summary>
    Public Function GetHenlKeijoCount() As Integer
        Return _crud.ExecuteScalar(Of Integer)("SELECT COUNT(*) FROM tw_d_henl_keijo")
    End Function

    ' ======================================================================
    '  ヘルパー
    ' ======================================================================

    ''' <summary>DBNull対応パラメータ生成</summary>
    Private Shared Function MakeParam(name As String, value As Object) As NpgsqlParameter
        Dim p As New NpgsqlParameter(name, NpgsqlTypes.NpgsqlDbType.Unknown)
        If value Is Nothing OrElse IsDBNull(value) Then
            p.Value = DBNull.Value
        Else
            p.Value = value
        End If
        Return p
    End Function

End Class
