Imports System.Text
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class Form_f_flx_YOSAN
    Inherits Form

    Public Property DtFrom As Date
    Public Property DtTo As Date
    Public Property NextDtFrom As Date
    Public Property NextDtTo As Date

    Private Const FMT_CURRENCY As String = "#,##0"
    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_YOSAN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DtFrom = GetMonthStart(DtFrom)
        DtTo = GetMonthEnd(DtTo)
        NextDtFrom = GetMonthStart(NextDtFrom)
        NextDtTo = GetMonthEnd(NextDtTo)

        SearchData()

        LoadDgvTotal()

        lbl_CONDITION.Text = "集計期間：  " & DtFrom.ToString("yyyy/MM") & "～" & NextDtTo.ToString("yyyy/MM")
        SecurityChecker.ApplyListLimit(Me)
    End Sub

    Private Sub SearchData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim sql = BuildSql(txt_SEARCH.Text.Trim(), prms)

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True

            dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)

            ApplyGridStyle()
            ApplyGrayOut()

        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message)
        End Try
    End Sub

    Private Function BuildSql(searchText As String, ByRef prms As List(Of NpgsqlParameter))
        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("WITH MainData AS (")
        sb.AppendLine("SELECT ")
        sb.AppendLine("  kykm.kykm_id, ")
        sb.AppendLine("  kykm.kykm_no AS 物件No, ")
        sb.AppendLine("  kykm.saikaisu AS 再回, ")
        sb.AppendLine("  haif.line_id AS 配No, ")
        ' 「予想/既存」: 開始日が集計期間開始日より後 → 予想（まだ始まっていない契約）、それ以外 → 既存
        ' Access版VBA参照不可のため仮実装。確認後に修正が必要な場合は別Issueで対応
        sb.AppendLine("  CASE WHEN kykh.start_dt > @dtFrom THEN '予想' ELSE '既存' END AS 予想, ")
        sb.AppendLine("  kkbn.kkbn_nm AS 契区, ")
        ' 「行区」: Access版VBA参照不可のため固定値維持。確認後に修正予定
        sb.AppendLine("  '定額' AS 行区, ")
        sb.AppendLine("  kjkbn.kjkbn_nm AS 計上区分, ")
        sb.AppendLine("  kykh.kykbnl AS 契約番号, ")
        sb.AppendLine("  lcpt.lcpt1_nm AS 支払先, ")
        sb.AppendLine("  kykm.bukn_nm AS 物件名, ")
        sb.AppendLine("  b_bcat.bcat1_nm AS 管理部署, ")
        sb.AppendLine("  h_bcat.bcat1_nm AS 費用負担部署, ")
        sb.AppendLine("  haif.haifritu AS 配賦率, ")
        sb.AppendLine("  hkmk.hkmk_nm AS 費用区分, ")
        sb.AppendLine("  kykh.start_dt AS 開始日, ")
        sb.AppendLine("  kykh.end_dt AS 終了日, ")
        sb.AppendLine("  kykm.ckaiyk_dt AS 中途解約日, ")

        AddPaymentMonths(sb)

        sb.AppendLine("FROM d_kykm kykm ")
        sb.AppendLine("LEFT JOIN d_haif haif ON kykm.kykm_id = haif.kykm_id ")
        sb.AppendLine("LEFT JOIN d_kykh kykh ON kykm.kykh_id = kykh.kykh_id ")
        sb.AppendLine("LEFT JOIN c_kkbn kkbn ON kykh.kkbn_id = kkbn.kkbn_id ")
        sb.AppendLine("LEFT JOIN c_kjkbn kjkbn ON kykm.kjkbn_id = kjkbn.kjkbn_id ")
        sb.AppendLine("LEFT JOIN m_lcpt lcpt ON kykh.lcpt_id = lcpt.lcpt_id ")
        sb.AppendLine("LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_bcat h_bcat ON haif.h_bcat_id = h_bcat.bcat_id ")
        sb.AppendLine("LEFT JOIN m_hkmk hkmk ON haif.hkmk_id = hkmk.hkmk_id ")

        sb.AppendLine("WHERE ((kykh.start_dt <= @NextDtTo AND kykh.end_dt >= @dtFrom) ")    ' 範囲がかぶるか
        sb.AppendLine("OR (kykm.b_shdt_fst_sum BETWEEN @dtFrom AND @NextDtTo)) ")
        sb.AppendLine("AND kykm.b_shdt_fst_sum <= @NextDtTo ")
        sb.AppendLine(") ")

        sb.AppendLine("SELECT * FROM MainData ")
        ' 支払額がすべて0の行は出力しない(todo シンプルな比較文にしたい)
        sb.AppendLine("WHERE (m0 + m1 + m2 + m3 + m4 + m5 + m6 + m7 + m8 + m9 + m10 + m11 + m12 + m13 + m14 + m15 + m16 + m17 + m18 + m19 + m20 + m21 + m22 + m23) > 0 ")
        ' sb.AppendLine("ORDER BY kykm_no, saikaisu")

        ' パラメータ設定
        prms.Add(New NpgsqlParameter("@dtFrom", DtFrom))
        prms.Add(New NpgsqlParameter("@NextDtTo", NextDtTo))

        For i = 0 To 23
            prms.Add(New NpgsqlParameter($"dt{i}", DtFrom.AddMonths(i)))
        Next

        If txt_SEARCH.Text.Trim() <> "" Then
            Dim searchVal As Integer
            If Integer.TryParse(searchText, searchVal) Then
                sb.AppendLine("AND kykm.kykm_no = @search ")
                prms.Add(New NpgsqlParameter("@search", searchVal))
            End If
        End If

        Return sb.ToString()
    End Function

    Private Sub LoadDgvTotal()
        Dim dtTotal As New DataTable()
        For Each col As DataGridViewColumn In dgv_LIST.Columns
            dtTotal.Columns.Add(col.Name)
        Next

        Dim totalRow As DataRow = dtTotal.NewRow()
        For i = 0 To 23
            totalRow($"m{i}") = CalculateTotalPayments($"m{i}").ToString("#,##0")
        Next

        dtTotal.Rows.Add(totalRow)

        dgv_TOTAL.DataSource = dtTotal

        dgv_TOTAL.HideColumns("kykm_id")
    End Sub

    Private Sub ApplyGridStyle()
        dgv_LIST.HideColumns("kykm_id")

        For i = 1 To 23
            dgv_LIST.FormatColumn($"m{i}", FMT_CURRENCY)
        Next
    End Sub

    ' グレーアウト判定（Form_f_flx_KHIYO.ApplyGrayOut() と同一パターン）
    ' 集計期間外かつ全月支払額が0の行を灰色表示
    Private Sub ApplyGrayOut()
        For Each row As DataGridViewRow In dgv_LIST.Rows
            If row.IsNewRow Then Continue For

            Dim startDt = If(IsDBNull(row.Cells("開始日").Value), Date.MinValue, CDate(row.Cells("開始日").Value))
            Dim endDt = If(IsDBNull(row.Cells("終了日").Value), Date.MaxValue, CDate(row.Cells("終了日").Value))

            Dim hasPayment As Boolean = False
            For i = 0 To 23
                Dim colName = $"m{i}"
                If dgv_LIST.Columns.Contains(colName) Then
                    Dim val = If(IsDBNull(row.Cells(colName).Value), 0D, CDec(row.Cells(colName).Value))
                    If val <> 0 Then
                        hasPayment = True
                        Exit For
                    End If
                End If
            Next

            Dim inPeriod As Boolean = (startDt <= NextDtTo AndAlso endDt >= DtFrom)

            If Not inPeriod AndAlso Not hasPayment Then
                row.DefaultCellStyle.ForeColor = Color.Gray
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
            End If
        Next
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' [再計算]ボタン
    Private Sub cmd_RECALCULATE_Click(sender As Object, e As EventArgs) Handles cmd_RECALCULATE.Click
        Dim frm As New Form_f_YOSAN_JOKEN

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
        Dim frm As New Form_f_FlexOutputDLG()

        frm.Dgv = dgv_LIST
        frm.ShowDialog()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub

    Private Sub dgv_LIST_Scroll(sender As Object, e As ScrollEventArgs) Handles dgv_LIST.Scroll
        dgv_LIST.SyncDgvScroll(dgv_TOTAL)
    End Sub

    ' 支払額の出力SQL文
    Private Sub AddPaymentMonths(ByRef sb As StringBuilder)
        Dim dt = DtFrom
        Dim dtRows As New List(Of String)
        Dim i As Integer = 0
        While dt <= NextDtTo
            ' 支払月ならリース料(初回は2倍の可能性あり)、違うなら0
            Dim query As String = $"CASE WHEN @dt{i} BETWEEN date_trunc('month', kykh.shri_dt1) AND date_trunc('month', kykh.shri_en_dt) " &
                                  $"AND (EXTRACT(year FROM age(date_trunc('month', @dt{i}), date_trunc('month', kykh.shri_dt1))) * 12 + " &
                                  $"EXTRACT(month FROM age(date_trunc('month', @dt{i}), date_trunc('month', kykh.shri_dt1)))) " &
                                  $"% kykh.shri_kn = 0 " &
                                  $"THEN CASE WHEN date_trunc('month', @dt{i}) = date_trunc('month', kykh.shri_dt1) AND date_trunc('month', @dt{i}) = date_trunc('month', kykh.shri_dt2) " &
                                  $"THEN haif.h_klsryo * 2 ELSE haif.h_klsryo END " &
                                  $"ELSE 0 " &
                                  $"END AS m{i}"
            ' 列名は m0〜m23 を維持（ApplyGridStyle/LoadDgvTotal が同名で参照しているため）
            ' ヘッダ表示の yyyy/MM 形式変換は DataGridView の HeaderText を設定することで対応可能

            dtRows.Add(query)

            dt = dt.AddMonths(1)
            i += 1
        End While

        sb.AppendLine(String.Join(", ", dtRows))
    End Sub

    Private Function CalculateTotalPayments(colName As String) As Integer
        Dim totalAmount = 0
        For Each row As DataGridViewRow In dgv_LIST.Rows
            ' 新規行（一番下の空行）は計算から除外する
            If Not row.IsNewRow Then
                ' セルの値を取得
                Dim val As Object = row.Cells(colName).Value

                ' Nullや空文字でないことを確認
                If val IsNot Nothing AndAlso Not IsDBNull(val) Then
                    totalAmount += Convert.ToDecimal(val)
                End If
            End If
        Next

        Return totalAmount
    End Function
End Class