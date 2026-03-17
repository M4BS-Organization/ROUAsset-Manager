' 計上用SQL生成 (Access版 pc_SHRI_COM.gMOTO_RSETSQL_EDIT_KEIJO 相当)
' KeijoCalculationEngine から呼び出される静的メソッド群

Imports System.Collections.Generic
Imports System.Text
Imports Npgsql

''' <summary>
''' 計上用SQL生成クラス (Access版 gMOTO_RSETSQL_EDIT_KEIJO + m仕訳計算_SQLMAKE 相当)
''' </summary>
Public Class KeijoSqlBuilder

    ''' <summary>
    ''' 計上用ソースデータSQL生成 (Access版 gMOTO_RSETSQL_EDIT_KEIJO)
    ''' d_kykh JOIN d_kykm (物件単位) / d_kykh JOIN d_kykm JOIN d_haif (配賦単位)
    ''' </summary>
    ''' <param name="meisai">明細単位 (物件/配賦)</param>
    ''' <param name="taisho">対象区分 (1:リース, 2:保守, 3:全部)</param>
    ''' <param name="kjkbnSisan">計上区分: 資産を含む</param>
    ''' <param name="kjkbnHiyo">計上区分: 費用を含む</param>
    ''' <returns>SQL文字列とパラメータのタプル</returns>
    Public Shared Function BuildSourceSql(
        meisai As ShriMeisai,
        taisho As Integer,
        kjkbnSisan As Boolean,
        kjkbnHiyo As Boolean
    ) As (Sql As String, Parameters As List(Of NpgsqlParameter))

        Dim sb As New StringBuilder()
        Dim params As New List(Of NpgsqlParameter)()

        sb.AppendLine("SELECT")
        sb.AppendLine("  h.kykh_id AS kykh_kykh_id,")
        sb.AppendLine("  h.saikaisu AS kykh_saikaisu,")
        sb.AppendLine("  m.kykm_id AS kykm_kykm_id,")
        sb.AppendLine("  m.kykm_no AS kykm_kykm_no,")
        sb.AppendLine("  m.kjkbn_id AS kykm_kjkbn_id,")
        sb.AppendLine("  m.bukn_nm,")
        sb.AppendLine("  h.kykbnl AS kykbnl,")
        sb.AppendLine("  h.kkbn_id,")
        sb.AppendLine("  m.leakbn_id,")
        sb.AppendLine("  h.lcpt_id,")
        sb.AppendLine("  m.hensai_kind,")
        sb.AppendLine("  h.shri_kn,")
        sb.AppendLine("  h.shri_cnt,")
        sb.AppendLine("  h.sshri_kn_m,")
        sb.AppendLine("  h.sshri_kn_1,")
        sb.AppendLine("  h.sshri_kn_2,")
        sb.AppendLine("  h.sshri_kn_3,")
        sb.AppendLine("  h.shho_m_id,")
        sb.AppendLine("  h.shho_1_id,")
        sb.AppendLine("  h.shho_2_id,")
        sb.AppendLine("  h.shho_3_id,")
        sb.AppendLine("  h.mae_dt,")
        sb.AppendLine("  h.shri_dt1,")
        sb.AppendLine("  h.shri_dt2,")
        sb.AppendLine("  h.shri_dt3,")
        sb.AppendLine("  h.zritu,")
        sb.AppendLine("  m.b_klsryo,")
        sb.AppendLine("  m.b_kzei,")
        sb.AppendLine("  m.b_mlsryo,")
        sb.AppendLine("  m.b_mzei,")
        sb.AppendLine("  m.b_henl_f,")
        sb.AppendLine("  m.b_gson_f,")
        sb.AppendLine("  h.jencho_f,")
        sb.AppendLine("  h.start_dt,")
        sb.AppendLine("  m.b_rend_dt,")
        sb.AppendLine("  h.kyak_dt,")
        sb.AppendLine("  m.ckaiyk_esdt_t,")
        sb.AppendLine("  m.ckaiyk_esdt_h,")
        sb.AppendLine("  h.skyu_kj_f,")
        sb.AppendLine("  h.k_kjyo_st_dt,")
        sb.AppendLine("  m.b_smdt_fst_sum,")
        sb.AppendLine("  m.b_shdt_fst_sum,")
        sb.AppendLine("  m.b_smdt_lst_sum,")
        sb.AppendLine("  m.b_shdt_lst_sum,")
        sb.AppendLine("  m.szei_kjkbn_id")

        ' 配賦単位の場合は配賦情報を結合
        If meisai = ShriMeisai.Haif Then
            sb.AppendLine("  , ha.line_id")
            sb.AppendLine("  , ha.haifritu")
            sb.AppendLine("  , ha.hkmk_id")
            sb.AppendLine("  , ha.h_bcat_id")
            sb.AppendLine("  , ha.rsrvh1_id")
            sb.AppendLine("  , ha.h_zokusei1")
            sb.AppendLine("  , ha.h_zokusei2")
            sb.AppendLine("  , ha.h_zokusei3")
            sb.AppendLine("  , ha.h_zokusei4")
            sb.AppendLine("  , ha.h_zokusei5")
        End If

        sb.AppendLine("FROM d_kykh h")
        sb.AppendLine("  INNER JOIN d_kykm m ON h.kykh_id = m.kykh_id")

        If meisai = ShriMeisai.Haif Then
            sb.AppendLine("  LEFT JOIN d_haif ha ON m.kykm_id = ha.kykm_id")
        End If

        sb.AppendLine("WHERE 1=1")

        ' 計上区分フィルタ
        If kjkbnSisan AndAlso Not kjkbnHiyo Then
            sb.AppendLine("  AND m.kjkbn_id = 2")  ' 資産のみ
        ElseIf Not kjkbnSisan AndAlso kjkbnHiyo Then
            sb.AppendLine("  AND m.kjkbn_id = 1")  ' 費用のみ
        End If
        ' 両方 True の場合はフィルタなし（全件）

        ' 対象区分フィルタ (1:リース, 2:保守, 3:全部)
        If taisho = 1 Then
            ' リースのみ: kkbn_id IN (1,2,4) リース/レンタル/移転リース
            sb.AppendLine("  AND h.kkbn_id IN (1, 2, 4)")
        ElseIf taisho = 2 Then
            ' 保守のみ: kkbn_id = 3
            sb.AppendLine("  AND h.kkbn_id = 3")
        End If
        ' taisho = 3 は全部なのでフィルタなし

        ' ソート順
        If meisai = ShriMeisai.Haif Then
            sb.AppendLine("ORDER BY m.kykm_id, ha.line_id")
        Else
            sb.AppendLine("ORDER BY m.kykm_id")
        End If

        Return (sb.ToString(), params)
    End Function

    ''' <summary>
    ''' 減損データ取得SQL (Access版 D_GSON テーブル参照)
    ''' </summary>
    Public Shared Function BuildGsonSql(kykmId As Integer) As (Sql As String, Parameters As List(Of NpgsqlParameter))
        Dim sql As String = "SELECT gson_dt, gson_ryo FROM d_gson WHERE kykm_id = @kykm_id ORDER BY gson_dt"
        Dim params As New List(Of NpgsqlParameter)()
        params.Add(New NpgsqlParameter("@kykm_id", NpgsqlTypes.NpgsqlDbType.Integer) With {.Value = kykmId})
        Return (sql, params)
    End Function

    ''' <summary>
    ''' 付随費用ソースSQL (Access版 mKEIJO_Sub_HENF 内の SQL)
    ''' d_henf テーブルから付随費用データを取得
    ''' </summary>
    Public Shared Function BuildHenfSql(meisai As ShriMeisai) As String
        Dim sb As New StringBuilder()
        sb.AppendLine("SELECT")
        sb.AppendLine("  f.line_id AS henf_id,")
        sb.AppendLine("  f.kykm_id,")
        sb.AppendLine("  m.kykh_id,")
        sb.AppendLine("  m.kykm_no,")
        sb.AppendLine("  m.bukn_nm,")
        sb.AppendLine("  h.kykbnl,")
        sb.AppendLine("  h.kkbn_id,")
        sb.AppendLine("  m.kjkbn_id,")
        sb.AppendLine("  m.leakbn_id,")
        sb.AppendLine("  h.start_dt,")
        sb.AppendLine("  f.shri_en_dt AS end_dt,")
        sb.AppendLine("  m.f_lcpt_id,")
        sb.AppendLine("  m.f_hkmk_id,")
        sb.AppendLine("  f.shri_kn,")
        sb.AppendLine("  f.shri_cnt,")
        sb.AppendLine("  f.sshri_kn,")
        sb.AppendLine("  f.shho_id,")
        sb.AppendLine("  f.shri_dt1,")
        sb.AppendLine("  f.zritu,")
        sb.AppendLine("  f.klsryo,")
        sb.AppendLine("  f.kzei,")
        sb.AppendLine("  h.skyu_kj_f,")
        sb.AppendLine("  h.k_kjyo_st_dt,")
        sb.AppendLine("  f.shri_en_dt")
        sb.AppendLine("FROM d_henf f")
        sb.AppendLine("  INNER JOIN d_kykm m ON f.kykm_id = m.kykm_id")
        sb.AppendLine("  INNER JOIN d_kykh h ON m.kykh_id = h.kykh_id")
        sb.AppendLine("WHERE h.kkbn_id = 3")  ' 保守契約のみ
        sb.AppendLine("ORDER BY f.kykm_id, f.line_id")
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' 付随費用配賦情報SQL
    ''' </summary>
    Public Shared Function BuildHenfHaifSql() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("SELECT")
        sb.AppendLine("  ha.kykm_id AS kykm_kykm_id,")
        sb.AppendLine("  ha.line_id,")
        sb.AppendLine("  ha.haifritu,")
        sb.AppendLine("  ha.hkmk_id,")
        sb.AppendLine("  ha.h_bcat_id,")
        sb.AppendLine("  ha.rsrvh1_id,")
        sb.AppendLine("  ha.h_zokusei1,")
        sb.AppendLine("  ha.h_zokusei2,")
        sb.AppendLine("  ha.h_zokusei3,")
        sb.AppendLine("  ha.h_zokusei4,")
        sb.AppendLine("  ha.h_zokusei5")
        sb.AppendLine("FROM d_haif ha")
        sb.AppendLine("  INNER JOIN d_kykm m ON ha.kykm_id = m.kykm_id")
        sb.AppendLine("  INNER JOIN d_kykh h ON m.kykh_id = h.kykh_id")
        sb.AppendLine("WHERE h.kkbn_id = 3")
        sb.AppendLine("ORDER BY ha.kykm_id, ha.line_id")
        Return sb.ToString()
    End Function

End Class
