Imports System.Text
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

' --- 経費明細表 ---
Partial Public Class Form_f_flx_経費明細表
    Inherits Form

    Public Property DtFrom As Date
    Public Property DtTo As Date
    Public Property Taisho As Integer = 3
    Public Property LabelText As String

    Private Const FMT_CURRENCY As String = "#,##0"
    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_経費明細表_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SearchData()
        SecurityChecker.ApplyListLimit(Me)
    End Sub

    Private Sub SearchData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim sql = BuildSql(prms)

            dgvMain.AutoGenerateColumns = False
            dgvMain.DataSource = _crud.GetDataTable(sql, prms)

            ApplyGridStyle()

        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message)
        End Try
    End Sub

    Private Function BuildSql(ByRef prms As List(Of NpgsqlParameter)) As String
        prms.Add(New NpgsqlParameter("@dtFrom", GetMonthStart(DtFrom)))
        prms.Add(New NpgsqlParameter("@dtTo", GetMonthEnd(DtTo)))

        ' 集計期間の月数（パラメータ化）
        Dim months As Integer = GetDuration(DtFrom, DtTo)
        If months < 1 Then months = 1
        prms.Add(New NpgsqlParameter("@months", months))

        Dim sb As New StringBuilder()

        sb.AppendLine("SELECT ")
        sb.AppendLine("  COALESCE(kykm.kykm_no, 0) AS KYKM_NO, ")
        sb.AppendLine("  COALESCE(kykm.saikaisu, 0) AS SAIKAISU, ")
        sb.AppendLine("  COALESCE(haif.line_id, 0) AS LINE_ID, ")
        sb.AppendLine("  kkbn.kkbn_nm AS KKBN_NM, ")
        sb.AppendLine("  CASE WHEN kykm.kjkbn_id = 1 THEN '費用' WHEN kykm.kjkbn_id = 2 THEN '資産' ELSE '' END AS REC_KBN_STR, ")
        sb.AppendLine("  kykm.bukn_nm AS BUKN_NM, ")
        sb.AppendLine("  kykh.start_dt AS START_DT, ")
        sb.AppendLine("  kykm.ckaiyk_dt AS CKAIYK_DT, ")
        sb.AppendLine("  kykh.kykbnl AS KYKBNL, ")
        sb.AppendLine("  lcpt.lcpt1_nm AS LCPT1_NM, ")
        sb.AppendLine("  b_bcat.bcat1_nm AS B_BCAT_NM, ")
        sb.AppendLine("  h_bcat.bcat1_nm AS H_BCAT_NM, ")
        sb.AppendLine("  hkmk.hkmk_nm AS HKMK_NM, ")
        sb.AppendLine("  kykh.start_dt AS K_KJYO_ST_DT, ")
        sb.AppendLine("  kykh.end_dt AS B_KJYO_EN_DT, ")

        ' リース料総額
        sb.AppendLine("  COALESCE(haif.h_klsryo, 0) AS LSRYO_TOTAL, ")

        ' 前期末残高・前払
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) - (COALESCE(haif.h_klsryo, 0) / kykh.lkikan) * ")
        sb.AppendLine("      GREATEST(0, EXTRACT(YEAR FROM AGE(@dtFrom, kykh.start_dt)) * 12 + EXTRACT(MONTH FROM AGE(@dtFrom, kykh.start_dt))) AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS MAE_ZZAN, ")

        ' リース料残高・前払
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) - (COALESCE(haif.h_klsryo, 0) / kykh.lkikan) * ")
        sb.AppendLine("      GREATEST(0, EXTRACT(YEAR FROM AGE(@dtFrom, kykh.start_dt)) * 12 + EXTRACT(MONTH FROM AGE(@dtFrom, kykh.start_dt))) AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS LSRYO_ZZAN, ")

        ' 当期経費額
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) / kykh.lkikan * @months AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS KEIHI_TOKI, ")

        ' 当期リース料
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) / kykh.lkikan * @months AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS LSRYO_TOKI, ")

        ' 前払費用残高
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) - (COALESCE(haif.h_klsryo, 0) / kykh.lkikan) * ")
        sb.AppendLine("      LEAST(kykh.lkikan, GREATEST(0, EXTRACT(YEAR FROM AGE(@dtFrom, kykh.start_dt)) * 12 + EXTRACT(MONTH FROM AGE(@dtFrom, kykh.start_dt))) + @months) AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS MAE_ZAN, ")

        ' リース料残高
        sb.AppendLine("  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 THEN ")
        sb.AppendLine("    ROUND(CAST(COALESCE(haif.h_klsryo, 0) - (COALESCE(haif.h_klsryo, 0) / kykh.lkikan) * ")
        sb.AppendLine("      LEAST(kykh.lkikan, GREATEST(0, EXTRACT(YEAR FROM AGE(@dtFrom, kykh.start_dt)) * 12 + EXTRACT(MONTH FROM AGE(@dtFrom, kykh.start_dt))) + @months) AS NUMERIC), 0) ")
        sb.AppendLine("  ELSE 0 END AS LSRYO_ZAN, ")

        ' 解約前残高
        sb.AppendLine("  0 AS KAIYAK_MAE_ZAN, ")
        ' 解約経費
        sb.AppendLine("  0 AS KAIYAK_KEIHI, ")

        ' 月別経費計上額 (G01～G12)
        For m As Integer = 1 To 12
            Dim monthOffset = m - 1
            Dim colName = $"KEIHI_TOKIG{m:00}"
            sb.AppendLine($"  CASE WHEN COALESCE(kykh.lkikan, 0) > 0 AND {monthOffset} < @months THEN ")
            sb.AppendLine($"    ROUND(CAST(COALESCE(haif.h_klsryo, 0) / kykh.lkikan AS NUMERIC), 0) ")
            sb.AppendLine($"  ELSE 0 END AS {colName}, ")
        Next

        ' 合計列（集約は行レベルでは個別行のためそのまま同値）
        sb.AppendLine("  COALESCE(haif.h_klsryo, 0) AS LSRYO_TOTAL_SUM, ")
        sb.AppendLine("  0 AS MAE_ZZAN_SUM, ")
        sb.AppendLine("  0 AS LSRYO_ZZAN_SUM, ")
        sb.AppendLine("  0 AS KEIHI_TOKI_SUM, ")
        sb.AppendLine("  0 AS LSRYO_TOKI_SUM, ")
        sb.AppendLine("  0 AS MAE_ZAN_SUM, ")
        sb.AppendLine("  0 AS LSRYO_ZAN_SUM, ")
        sb.AppendLine("  0 AS KAIYAK_MAE_ZAN_SUM, ")
        sb.AppendLine("  0 AS KAIYAK_KEIHI_SUM, ")

        For m As Integer = 1 To 12
            Dim colName = $"KEIHI_TOKIG{m:00}_SUM"
            sb.AppendLine($"  0 AS {colName}, ")
        Next

        sb.AppendLine("  kykm.kykm_id AS ID ")

        sb.AppendLine("FROM d_kykm kykm ")
        sb.AppendLine("LEFT JOIN d_kykh kykh ON kykm.kykh_id = kykh.kykh_id ")
        sb.AppendLine("LEFT JOIN d_haif haif ON kykm.kykm_id = haif.kykm_id ")
        sb.AppendLine("LEFT JOIN c_kkbn kkbn ON kykh.kkbn_id = kkbn.kkbn_id ")
        sb.AppendLine("LEFT JOIN m_lcpt lcpt ON kykh.lcpt_id = lcpt.lcpt_id ")
        sb.AppendLine("LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_bcat h_bcat ON haif.h_bcat_id = h_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_hkmk hkmk ON haif.hkmk_id = hkmk.hkmk_id ")

        sb.AppendLine("WHERE ((kykh.start_dt <= @dtTo AND kykh.end_dt >= @dtFrom) ")
        sb.AppendLine("  OR (kykm.b_shdt_fst_sum BETWEEN @dtFrom AND @dtTo)) ")

        ' 集計対象フィルタ (1=リース料のみ, 2=保守料のみ, 3=全部)
        If Taisho = 1 Then
            sb.AppendLine("AND kykm.kjkbn_id = 1 ")
        ElseIf Taisho = 2 Then
            sb.AppendLine("AND kykm.kjkbn_id = 2 ")
        End If

        sb.AppendLine("ORDER BY kykm.kykm_id;")

        Return sb.ToString()
    End Function

    Private Sub ApplyGridStyle()
        ' 通貨フォーマット
        For Each col As DataGridViewColumn In dgvMain.Columns
            If col.Name.StartsWith("txt_LSRYO_") OrElse
               col.Name.StartsWith("txt_MAE_") OrElse
               col.Name.StartsWith("txt_KEIHI_") OrElse
               col.Name.StartsWith("txt_KAIYAK_") Then
                col.DefaultCellStyle.Format = FMT_CURRENCY
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        Next
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    ' [照会]ボタン
    Private Sub cmd_照会_Click(sender As Object, e As EventArgs) Handles cmd_照会.Click
        If dgvMain.CurrentRow Is Nothing OrElse dgvMain.CurrentRow.IsNewRow Then Return

        Dim idVal = dgvMain.CurrentRow.Cells("txt_ID").Value
        If idVal Is Nothing OrElse IsDBNull(idVal) Then Return

        Dim frmBukn As New Form_BuknEntry
        frmBukn.KykmId = Convert.ToDouble(idVal)
        frmBukn.ShowDialog()
    End Sub

    ' [ファイル出力]ボタン
    Private Sub cmd_Output_Click(sender As Object, e As EventArgs) Handles cmd_Output.Click
        Dim frm As New Form_f_FlexOutputDLG
        frm.Dgv = dgvMain
        frm.ShowDialog()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class
