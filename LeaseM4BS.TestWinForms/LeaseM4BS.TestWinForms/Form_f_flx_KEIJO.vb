Imports LeaseM4BS.DataAccess
Imports Npgsql

Public Class Form_f_flx_KEIJO
    Public Property LabelText As String
    Public Property Joken As KeijoJoken
    Public Property DtFrom As Date
    Public Property DtTo As Date

    Private Const FMT_CURRENCY As String = "#,##0"
    Private Const FMT_DATE As String = "yyyy/MM/dd"
    Private _crud As New CrudHelper()
    Private _engine As MonthlyJournalEngine

    Private Sub Form_f_flx_MONTHLY_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_CONDITION.Text = LabelText

        SearchData()
        SecurityChecker.ApplyListLimit(Me)
    End Sub

    Private Sub SearchData()
        If Joken IsNot Nothing Then
            ExecuteEngine()
            Return
        End If

        SearchDataLegacy()
    End Sub

    ''' <summary>MonthlyJournalEngine で計上計算を実行し、tw_s_chuki_keijo から表示する</summary>
    Private Sub ExecuteEngine()
        Try
            Me.Cursor = Cursors.WaitCursor

            _engine = New MonthlyJournalEngine(_crud)
            Dim success As Boolean = _engine.Execute(DtFrom, DtTo, Joken)

            If Not success Then
                MessageBox.Show("計上計算に失敗しました。")
                Return
            End If

            ' ワークテーブルから名称JOINしたデータを取得
            Dim prms As New List(Of NpgsqlParameter)
            Dim sql = BuildWorkTableSql(txt_SEARCH.Text.Trim(), prms)

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True
            dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)

            ApplyGridStyle()

            Dim counts = _engine.GetResultCounts()
            lbl_CONDITION.Text = LabelText & $"  [注記:{counts.ChukiCount}件 変額:{counts.HenlCount}件]"

        Catch ex As Exception
            MessageBox.Show("計上計算エラー: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>従来のSQL一覧表示（条件パラメータなしの場合の互換動作）</summary>
    Private Sub SearchDataLegacy()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim sql = BuildWorkTableSql(txt_SEARCH.Text.Trim(), prms)

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True

            dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)

            ApplyGridStyle()

        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo ベースのSQL（マスタJOINで名称取得、Access版 qsel_df_flx_KEIJO 互換）
    ''' Issue #49 記載の欠落カラムを全て含む。
    ''' </summary>
    Private Function BuildWorkTableSql(searchText As String, ByRef prms As List(Of NpgsqlParameter)) As String
        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("SELECT ")
        sb.AppendLine("  tw.kykm_id, ")
        sb.AppendLine("  tw.kykh_id, ")
        sb.AppendLine("  tw.kykm_no AS 物件No, ")
        sb.AppendLine("  tw.saikaisu_kykm AS 再回, ")
        sb.AppendLine("  tw.line_id AS 配No, ")
        sb.AppendLine("  kkbn.kkbn_nm AS 契区, ")

        ' 行区分 (REC_KBN_STR) — Access版 tw_S_KEIJO由来
        sb.AppendLine("  CASE tw.rec_kbn ")
        sb.AppendLine("    WHEN 1 THEN '定額' ")
        sb.AppendLine("    WHEN 2 THEN '変払' ")
        sb.AppendLine("    WHEN 3 THEN '付随' ")
        sb.AppendLine("    ELSE '' END AS 行区, ")

        sb.AppendLine("  kjkbn.kjkbn_nm AS 計上区分, ")

        ' 法令区分
        sb.AppendLine("  CASE ")
        sb.AppendLine("    WHEN kykh.start_dt IS NULL THEN '' ")
        sb.AppendLine("    WHEN COALESCE(kykh.kyak_dt, kykh.start_dt) >= (SELECT val_datetime FROM t_settei WHERE settei_nm = 'SEKOU_DT') THEN '新法' ")
        sb.AppendLine("    ELSE '旧法' ")
        sb.AppendLine("  END AS 法令区分, ")

        sb.AppendLine("  leakbn.leakbn_nm AS リース区分, ")

        ' 取引区分 (TRHK_KBN) — Access版 tw_S_KEIJO由来
        sb.AppendLine("  CASE ")
        sb.AppendLine("    WHEN tw.keijo_shri_cnt = 1 THEN '開始' ")
        sb.AppendLine("    WHEN tw.keijo_shri_cnt >= COALESCE(kykh.mkaisu, 0) THEN '末消' ")
        sb.AppendLine("    ELSE '継続' ")
        sb.AppendLine("  END AS 取引区分, ")

        sb.AppendLine("  tw.kykbnl_no AS 契約番号, ")
        sb.AppendLine("  lcpt.lcpt1_nm AS 支払先, ")
        sb.AppendLine("  tw.bukn_nm AS 物件名, ")
        sb.AppendLine("  b_bcat.bcat1_nm AS 管理部署, ")
        sb.AppendLine("  h_bcat.bcat1_nm AS 費用負担部署, ")
        sb.AppendLine("  hkmk.hkmk_nm AS 費用区分, ")
        sb.AppendLine("  kykh.start_dt AS 開始日, ")
        sb.AppendLine("  kykh.end_dt AS 終了日, ")

        ' 計上日 (KEIJO_DT) — Access版 tw_S_KEIJO由来
        sb.AppendLine("  tw.keijo_dt AS 計上日, ")

        ' 請求月
        sb.AppendLine("  TO_CHAR(tw.shri_dt, 'YYYY/MM') AS 請求月, ")

        sb.AppendLine("  kykm.ckaiyk_dt AS 中途解約日, ")

        ' 回数済/総
        sb.AppendLine("  CONCAT(tw.keijo_shri_cnt, '/', COALESCE(kykh.mkaisu, 0)) AS ""回数済/総"", ")

        sb.AppendLine("  kykh.k_knyukn AS 現金購入価額_物件, ")

        ' === 会計計算カラム（tw_s_chuki_keijo由来） ===
        sb.AppendLine("  tw.lsryo_total AS 総支払額, ")

        ' 前月末残高 (ZZAN) — Access版はスケジュール計算済み値
        sb.AppendLine("  CASE WHEN tw.keijo_shri_cnt > 0 THEN ")
        sb.AppendLine("    tw.lsryo_total - (tw.lsryo_toki * (tw.keijo_shri_cnt - 1)) ")
        sb.AppendLine("  ELSE tw.lsryo_total END AS 前月末残高, ")

        sb.AppendLine("  tw.lsryo_toki AS 当月計上額_税抜き, ")
        sb.AppendLine("  tw.zei_toki AS 当月計上額_消費税, ")
        sb.AppendLine("  (tw.lsryo_toki + tw.zei_toki) AS 当月計上額_税込み, ")

        ' 当月末残高
        sb.AppendLine("  CASE WHEN tw.keijo_shri_cnt > 0 THEN ")
        sb.AppendLine("    tw.lsryo_total - (tw.lsryo_toki * tw.keijo_shri_cnt) ")
        sb.AppendLine("  ELSE tw.lsryo_total END AS 当月末残高, ")

        ' 内1年内
        sb.AppendLine("  CASE WHEN COALESCE(kykh.mkaisu, 0) > 0 THEN ")
        sb.AppendLine("    tw.lsryo_toki * LEAST(12, GREATEST(kykh.mkaisu - tw.keijo_shri_cnt, 0)) ")
        sb.AppendLine("  ELSE 0 END AS 内1年内, ")

        sb.AppendLine("  tw.zritu AS 消費税率, ")
        sb.AppendLine("  tw.shri_dt AS 支払日, ")
        sb.AppendLine("  shho.shho_nm AS 支払方法, ")

        ' === 取得増減 (SYUTOK_ZOU / SYUTOK_GEN) — tw_S_KEIJO由来 ===
        sb.AppendLine("  tw.mae_zou AS 取得増, ")
        sb.AppendLine("  tw.mae_gen AS 取得減, ")

        ' === 税仮払/税未払 (ZEI_KHRI / ZEI_MHRI) ===
        sb.AppendLine("  tw.mzei_zou AS 税仮払, ")
        sb.AppendLine("  tw.mzei_gen AS 税未払, ")

        ' === 利権/利息 (LGNPN_TOKI/LGNPN_ZZAN/LGNPN_ZAN / LRSOK_TOKI) ===
        ' 利権・利息はワークテーブルに直接列がないため、前払増減から導出
        ' 利権当期 = 前払増(取得増) - リース料当期
        sb.AppendLine("  CASE WHEN tw.mae_zou > 0 THEN tw.mae_zou - tw.lsryo_toki ELSE 0 END AS 利権当期, ")
        ' 利権前残 = 総支払額 - 現金購入 - (利権当期 × 計上済回数-1)
        sb.AppendLine("  CASE WHEN COALESCE(kykh.k_knyukn, 0) > 0 THEN ")
        sb.AppendLine("    tw.lsryo_total - kykh.k_knyukn ")
        sb.AppendLine("  ELSE 0 END AS 利権前残, ")
        ' 利権残
        sb.AppendLine("  CASE WHEN COALESCE(kykh.k_knyukn, 0) > 0 THEN ")
        sb.AppendLine("    tw.lsryo_total - kykh.k_knyukn - CASE WHEN tw.mae_zou > 0 THEN tw.mae_zou - tw.lsryo_toki ELSE 0 END ")
        sb.AppendLine("  ELSE 0 END AS 利権残, ")
        ' 利息当期
        sb.AppendLine("  CASE WHEN tw.lsryo > 0 THEN tw.lsryo - tw.lsryo_toki ELSE 0 END AS 利息当期, ")

        ' === G累計増 / 減損累計増 / 減損特別当期 ===
        sb.AppendLine("  0 AS G累計増, ")
        sb.AppendLine("  0 AS 減損累計増, ")
        sb.AppendLine("  0 AS 減損特別当期, ")

        ' === 簿価残高 (BOKA_ZAN) ===
        sb.AppendLine("  CASE WHEN COALESCE(kykh.k_knyukn, 0) > 0 THEN ")
        sb.AppendLine("    kykh.k_knyukn - CASE WHEN COALESCE(kykh.mkaisu, 0) > 0 THEN ")
        sb.AppendLine("      ROUND(CAST(kykh.k_knyukn AS NUMERIC) / kykh.mkaisu * tw.keijo_shri_cnt, 0) ELSE 0 END ")
        sb.AppendLine("  ELSE 0 END AS 簿価残高 ")

        sb.AppendLine("FROM tw_s_chuki_keijo tw ")
        sb.AppendLine("LEFT JOIN d_kykh kykh ON tw.kykh_id = kykh.kykh_id ")
        sb.AppendLine("LEFT JOIN d_kykm kykm ON tw.kykm_id = kykm.kykm_id ")
        sb.AppendLine("LEFT JOIN c_kkbn kkbn ON tw.kkbn_id = kkbn.kkbn_id ")
        sb.AppendLine("LEFT JOIN c_kjkbn kjkbn ON tw.kjkbn_id = kjkbn.kjkbn_id ")
        sb.AppendLine("LEFT JOIN c_leakbn leakbn ON tw.leakbn_id = leakbn.leakbn_id ")
        sb.AppendLine("LEFT JOIN m_lcpt lcpt ON tw.lcpt_id = lcpt.lcpt_id ")
        sb.AppendLine("LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_bcat h_bcat ON tw.h_bcat_id = h_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_hkmk hkmk ON tw.hkmk_id = hkmk.hkmk_id ")
        sb.AppendLine("LEFT JOIN m_shho shho ON tw.shho_id = shho.shho_id ")

        ' 検索条件
        If searchText <> "" Then
            sb.AppendLine("WHERE tw.kykbnl_no LIKE @search ")
            prms.Add(New NpgsqlParameter("@search", $"%{searchText}%"))
        End If

        sb.AppendLine("ORDER BY tw.kykm_id, tw.keijo_dt;")

        Return sb.ToString()
    End Function

    ' --- グリッドの見た目調整 ---
    Private Sub ApplyGridStyle()
        dgv_LIST.HideColumns("kykm_id", "kykh_id")

        dgv_LIST.FormatColumn("現金購入価額_物件", FMT_CURRENCY)
        dgv_LIST.FormatColumn("総支払額", FMT_CURRENCY)
        dgv_LIST.FormatColumn("前月末残高", FMT_CURRENCY)
        dgv_LIST.FormatColumn("当月計上額_税抜き", FMT_CURRENCY)
        dgv_LIST.FormatColumn("当月計上額_消費税", FMT_CURRENCY)
        dgv_LIST.FormatColumn("当月計上額_税込み", FMT_CURRENCY)
        dgv_LIST.FormatColumn("当月末残高", FMT_CURRENCY)
        dgv_LIST.FormatColumn("内1年内", FMT_CURRENCY)
        dgv_LIST.FormatColumn("取得増", FMT_CURRENCY)
        dgv_LIST.FormatColumn("取得減", FMT_CURRENCY)
        dgv_LIST.FormatColumn("税仮払", FMT_CURRENCY)
        dgv_LIST.FormatColumn("税未払", FMT_CURRENCY)
        dgv_LIST.FormatColumn("利権当期", FMT_CURRENCY)
        dgv_LIST.FormatColumn("利権前残", FMT_CURRENCY)
        dgv_LIST.FormatColumn("利権残", FMT_CURRENCY)
        dgv_LIST.FormatColumn("利息当期", FMT_CURRENCY)
        dgv_LIST.FormatColumn("G累計増", FMT_CURRENCY)
        dgv_LIST.FormatColumn("減損累計増", FMT_CURRENCY)
        dgv_LIST.FormatColumn("減損特別当期", FMT_CURRENCY)
        dgv_LIST.FormatColumn("簿価残高", FMT_CURRENCY)

        dgv_LIST.FormatColumn("開始日", FMT_DATE)
        dgv_LIST.FormatColumn("終了日", FMT_DATE)
        dgv_LIST.FormatColumn("計上日", FMT_DATE)
        dgv_LIST.FormatColumn("中途解約日", FMT_DATE)
        dgv_LIST.FormatColumn("支払日", FMT_DATE)
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' [再計算]ボタン
    Private Sub cmd_RECALCULATE_Click(sender As Object, e As EventArgs) Handles cmd_RECALCULATE.Click
        Dim frm As New Form_f_KEIJO_JOKEN
        frm.ShowDialog()
    End Sub

    ' [照会]ボタン
    Private Sub cmd_REF_Click(sender As Object, e As EventArgs) Handles cmd_REF.Click
        Dim selectedRow = dgv_LIST.GetSelectedRow()

        If selectedRow Is Nothing Then Return

        Dim frmBukn As New Form_BuknEntry
        frmBukn.KykmId = Convert.ToDouble(selectedRow.Cells("kykm_id").Value)

        frmBukn.ShowDialog()

        Dim frmContract As New Form_ContractEntry
        frmContract.KykhId = Convert.ToDouble(selectedRow.Cells("kykh_id").Value)

        frmContract.ShowDialog()
    End Sub

    ' [ファイル出力]ボタン
    Private Sub cmd_OUTPUT_FILE_Click(sender As Object, e As EventArgs) Handles cmd_OUTPUT_FILE.Click
        Dim frm As New Form_f_FlexOutputDLG
        frm.Dgv = dgv_LIST

        frm.ShowDialog()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class
