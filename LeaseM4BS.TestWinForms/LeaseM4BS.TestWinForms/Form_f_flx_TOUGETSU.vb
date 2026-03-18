Imports LeaseM4BS.DataAccess
Imports Npgsql

Public Class Form_f_flx_TOUGETSU
    Public Property LabelText As String
    Public Property Joken As KeijoJoken
    Public Property DtFrom As Date
    Public Property DtTo As Date

    Private _crud As CrudHelper = New CrudHelper()
    Private _engine As MonthlyJournalEngine

    Private Sub Form_f_flx_MONTHLY_JOURNAL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_CONDITION.Text = LabelText

        SearchData()
    End Sub

    Private Sub SearchData()
        If Joken IsNot Nothing Then
            ExecuteEngine()
            Return
        End If

        SearchDataLegacy()
    End Sub

    ''' <summary>MonthlyJournalEngine で計上計算を実行し、結果を表示する</summary>
    Private Sub ExecuteEngine()
        Try
            Me.Cursor = Cursors.WaitCursor

            _engine = New MonthlyJournalEngine(_crud)
            Dim success As Boolean = _engine.Execute(DtFrom, DtTo, Joken)

            If Not success Then
                MessageBox.Show("計上計算に失敗しました。")
                Return
            End If

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True
            dgv_LIST.DataSource = _engine.GetChukiKeijoResult()

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
            Dim sql = BuildSql(txt_SEARCH.Text.Trim(), prms)

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True

            dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)

            ApplyGridStyle()

        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message)
        End Try
    End Sub

    Private Function BuildSql(searchText As String, ByRef prms As List(Of NpgsqlParameter))
        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("SELECT ")
        sb.AppendLine("  kykm.kykm_id, ")                                 ' 契約明細ID(D_KYKM)
        sb.AppendLine("  kykh.kykh_id, ")                                 ' 契約ID(D_KYKH)
        sb.AppendLine("  kykm.kykm_no AS 物件No, ")                       ' 契約明細No(D_KYKM)
        sb.AppendLine("  kykm.saikaisu AS 再回, ")                        ' 再リース回数(D_KYKM)
        sb.AppendLine("  haif.line_id AS 配No, ")                         ' 行ID(D_HAIF)
        sb.AppendLine("  kkbn.kkbn_nm AS 契区, ")                         ' 契約区分名(C_KKBN)
        sb.AppendLine("  kjkbn.kjkbn_nm AS 計上区分, ")                   ' 計上区分名(C_KJKBN)

        ' 法令区分: Form_f_flx_KEIJO.vb:46-53 と同一ロジック (SEKOU_DT との比較)
        ' 契約日(kyak_dt)があればそれを、なければ開始日(start_dt)を施行日と比較
        sb.AppendLine("  CASE ")
        sb.AppendLine("    WHEN kykh.start_dt IS NULL THEN '' ")
        sb.AppendLine("    WHEN COALESCE(kykh.kyak_dt, kykh.start_dt) >= (SELECT val_datetime FROM t_settei WHERE settei_nm = 'SEKOU_DT') THEN '新法' ")
        sb.AppendLine("    ELSE '旧法' ")
        sb.AppendLine("  END AS 法令区分, ")

        ' 取引区分: Access版VBA参照不可のため leakbn_nm で代替
        sb.AppendLine("  leakbn.leakbn_nm AS 取引区分, ")                 ' 取引区分(C_LEAKBN)

        sb.AppendLine("  leakbn.leakbn_nm AS リース区分, ")               ' リース区分(C_LEAKBN)
        sb.AppendLine("  kykh.kykbnl AS 契約番号, ")                      ' 契約番号(D_KYKH)
        sb.AppendLine("  lcpt.lcpt1_nm AS 支払先, ")                       ' 支払先(M_LCPT)
        sb.AppendLine("  kykm.bukn_nm AS 物件名, ")                       ' 物件名(D_KYKM)
        sb.AppendLine("  b_bcat.bcat1_nm AS 管理部署, ")                  ' 管理部署名(M_BCAT)
        sb.AppendLine("  h_bcat.bcat1_nm AS 費用負担部署, ")              ' 費用負担部署名(M_BCAT)
        sb.AppendLine("  hkmk.hkmk_nm AS 費用区分, ")                     ' 費用区分名(M_HKMK)
        sb.AppendLine("  kykh.start_dt AS 開始日, ")                      ' 開始日(D_KYKH)
        sb.AppendLine("  kykh.end_dt AS 終了日, ")                        ' 終了日(D_KYKH)

        ' 請求月: Form_f_flx_KEIJO.vb:65 と同一ロジック（現在月ベース）
        sb.AppendLine("  TO_CHAR(date_trunc('month', CURRENT_DATE), 'YYYY/MM') AS 請求月, ")

        sb.AppendLine("  kykm.ckaiyk_dt AS 中途解約日 ")                 ' 中途解約日(D_KYKM)

        sb.AppendLine("FROM d_kykm kykm ")
        sb.AppendLine("LEFT JOIN d_kykh kykh ON kykm.kykh_id = kykh.kykh_id ")
        sb.AppendLine("LEFT JOIN d_haif haif ON kykm.kykm_id = haif.kykm_id ")
        sb.AppendLine("LEFT JOIN c_kkbn kkbn ON kykh.kkbn_id = kkbn.kkbn_id ")
        sb.AppendLine("LEFT JOIN c_kjkbn kjkbn ON kykh.kjkbn_id = kjkbn.kjkbn_id ")
        sb.AppendLine("LEFT JOIN c_leakbn leakbn ON kykm.leakbn_id = leakbn.leakbn_id ")
        sb.AppendLine("LEFT JOIN m_lcpt lcpt ON kykh.lcpt_id = lcpt.lcpt_id ")
        sb.AppendLine("LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_bcat h_bcat ON haif.h_bcat_id = h_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_hkmk hkmk ON haif.hkmk_id = hkmk.hkmk_id ")

        ' --- 検索条件 (WHERE) ---
        ' 契約期間が画面の集計期間とオーバーラップするもののみ表示
        sb.AppendLine("WHERE (kykh.start_dt <= @dtTo AND kykh.end_dt >= @dtFrom) ")
        ' 中途解約済みの物件は除外（中途解約日が集計開始日より前の場合は除外）
        sb.AppendLine("AND (kykm.ckaiyk_dt IS NULL OR kykm.ckaiyk_dt >= @dtFrom) ")
        prms.Add(New NpgsqlParameter("@dtFrom", DtFrom))
        prms.Add(New NpgsqlParameter("@dtTo", DtTo))

        If txt_SEARCH.Text.Trim() <> "" Then
            sb.AppendLine("AND kykm.kykm_id::text LIKE @search ")
            prms.Add(New NpgsqlParameter("@search", $"%{searchText}%"))
        End If

        sb.AppendLine("ORDER BY kykm.kykm_id;")

        Return sb.ToString()
    End Function

    ' --- グリッドの見た目調整 ---
    Private Sub ApplyGridStyle()
        dgv_LIST.HideColumns("kykm_id", "kykh_id")
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