Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Microsoft.VisualBasic.Information
Imports Npgsql

Public Enum RecTiming
    SmdtBase    '〆日ベース
    ShdtBase    ' 約定支払日ベース
End Enum

Public Enum Symbol
    Minus
    Plus
End Enum

Partial Public Class Form_f_flx_KHIYO
    Inherits Form

    Public Property DtFrom As Date
    Public Property DtTo As Date
    Public Property NextDtTo As Date
    Public Property RecBase As RecTiming
    Public Property CheckRecFlags As Boolean() = New Boolean(6) {}
    Public Property RadioSymbol As Symbol

    Private Const FMT_CURRENCY As String = "#,##0"
    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_KHIYO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_CONDITION.Text = GetLabelText()

        SearchData()
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
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Function BuildSql(searchText As String, ByRef prms As List(Of NpgsqlParameter)) As String
        Dim selectPart1 As String = "kykm.kykm_id, " &                      ' 契約明細ID(D_KYKM)
                                    "kykh.kykh_id, " &                      ' 契約ID(D_KYKH)
                                    "kykm.kykm_no AS 物件No, " &            ' 契約明細No(D_KYKM)
                                    "haif.line_id AS 配No, " &              ' 配賦行No(D_HAIF)
                                    "kjkbn.kjkbn_nm AS 計上区分, "          ' 計上区分(C_KJKBN)

        Dim selectPart2 As String = "kykh.kykbnl AS 契約番号, " &           ' 契約番号(D_KYKH)
                                    "lcpt.lcpt1_nm AS 支払先, " &           ' 支払先(M_LCPT)
                                    "kykm.bukn_nm AS 物件名, " &            ' 物件名(D_KYKM)
                                    "b_bcat.bcat1_nm AS 管理部署, " &       ' 管理部署(M_BCAT)
                                    "h_bcat.bcat1_nm AS 費用負担部署, " &   ' 費用負担部署(M_BCAT)
                                    "haif.haifritu AS 配賦率, " &           ' 配賦率(D_HAIF)
                                    "kykh.start_dt AS 開始日, " &           ' 開始日(D_KYKH)
                                    "kykh.end_dt AS 終了日, " &             ' 終了日(D_KYKH)
                                    "kykh.lkikan AS 期間, " &               ' 契約期間(D_KYKH)
                                    "kykm.ckaiyk_dt AS 中途解約日, "        ' 中途解約日(D_KYKM)

        Dim fromClause As String = "FROM d_kykm kykm " &
                                   "LEFT JOIN d_kykh kykh ON kykm.kykh_id = kykh.kykh_id " &
                                   "LEFT JOIN d_haif haif ON kykm.kykm_id = haif.kykm_id " &
                                   "LEFT JOIN c_kjkbn kjkbn ON kykm.kjkbn_id = kjkbn.kjkbn_id " &
                                   "LEFT JOIN m_lcpt lcpt ON kykh.lcpt_id = lcpt.lcpt_id " &
                                   "LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id " &
                                   "LEFT JOIN m_bcat h_bcat ON haif.h_bcat_id = h_bcat.bcat_id "

        Dim whereClause As String = "WHERE ((kykh.start_dt <= @NextDtTo AND kykh.end_dt >= @dtFrom) " &
                                    "OR (kykm.b_shdt_fst_sum BETWEEN @dtFrom AND @dtTo)) "

        prms.Add(New NpgsqlParameter("@dtFrom", GetMonthStart(DtFrom)))
        prms.Add(New NpgsqlParameter("@dtTo", GetMonthEnd(DtTo)))
        prms.Add(New NpgsqlParameter("@NextDtTo", GetMonthEnd(NextDtTo)))

        If searchText <> "" Then
            whereClause &= "AND kykm.kykm_no = @search "
            prms.Add(New NpgsqlParameter("@search", Double.Parse(txt_SEARCH.Text.Trim())))
        End If

        ' 期間月数計算（契約期間と集計期間の交差月数）
        Dim periodMonths As String =
            "GREATEST(0, " &
            "(EXTRACT(YEAR FROM AGE(LEAST(@dtTo, COALESCE(kykh.end_dt, @dtTo)), " &
            "GREATEST(@dtFrom, kykh.start_dt))) * 12 " &
            "+ EXTRACT(MONTH FROM AGE(LEAST(@dtTo, COALESCE(kykh.end_dt, @dtTo)), " &
            "GREATEST(@dtFrom, kykh.start_dt))) + 1))"

        Dim sqls As New List(Of String)

        ' sql1: 支払額（計上区分=費用）
        If CheckRecFlags(0) Then
            Dim sql1 As String = "SELECT " &
                                  selectPart1 &
                                  "'支払額' AS 行区, " &
                                  selectPart2 &
                                  "haif.h_klsryo AS 期間計上額 " &
                                  fromClause &
                                  whereClause &
                                  "AND kykm.kjkbn_id = 1"

            sqls.Add(sql1)
        End If

        ' sql2: 保守料（変更情報テーブルから取得）
        If CheckRecFlags(1) Then
            Dim sql2 As String = "SELECT " &
                                 selectPart1 &
                                 "'保守料' AS 行区, " &
                                 selectPart2 &
                                 "henf.klsryo AS 期間計上額 " &
                                 "FROM d_henf henf " &
                                 "LEFT JOIN d_kykm kykm ON henf.kykm_id = kykm.kykm_id " &
                                 "LEFT JOIN d_kykh kykh ON kykm.kykh_id = kykh.kykh_id " &
                                 "LEFT JOIN d_haif haif ON kykm.kykm_id = haif.kykm_id " &
                                 "LEFT JOIN c_kjkbn kjkbn ON kykm.kjkbn_id = kjkbn.kjkbn_id " &
                                 "LEFT JOIN m_lcpt lcpt ON kykh.lcpt_id = lcpt.lcpt_id " &
                                 "LEFT JOIN m_bcat b_bcat ON kykm.b_bcat_id = b_bcat.bcat_id " &
                                 "LEFT JOIN m_bcat h_bcat ON haif.h_bcat_id = h_bcat.bcat_id " &
                                 "WHERE kykm.b_henf_f = True " &
                                 "AND NOT (kykh.end_dt < @dtFrom OR kykh.start_dt > @dtTo) "

            If searchText <> "" Then
                sql2 &= "AND kykm.kykm_no = @search "
            End If

            sqls.Add(sql2)
        End If

        ' sql3: 償却費（月割按分: 取得価額 / (耐用年数 * 12) * 期間月数）
        If CheckRecFlags(2) Then
            Dim sql3 As String = "SELECT " &
                                 selectPart1 &
                                 "'償却費' AS 行区, " &
                                 selectPart2 &
                                 "CASE WHEN kykm.taiyo_nen > 0 THEN " &
                                 "ROUND(kykm.b_syutok / (kykm.taiyo_nen * 12.0) * " & periodMonths & ", 0) " &
                                 "ELSE 0 END AS 期間計上額 " &
                                 fromClause &
                                 whereClause &
                                 "AND kykh.kkbn_id = 1 AND kykm.saikaisu = 0 " &
                                 "AND (kykm.kjkbn_id = 2 OR kykm.b_gson_f = True)"

            sqls.Add(sql3)
        End If

        ' sql4: 支払利息（月割按分: (リース料総額 - 購入価額) / リース期間 * 期間月数）
        If CheckRecFlags(3) Then
            Dim sql4 As String = "SELECT " &
                                 selectPart1 &
                                 "'支払利息' AS 行区, " &
                                 selectPart2 &
                                 "CASE WHEN kykh.lkikan > 0 THEN " &
                                 "ROUND((kykm.b_slsryo - kykm.b_knyukn) / kykh.lkikan * " & periodMonths & ", 0) " &
                                 "ELSE 0 END AS 期間計上額 " &
                                 fromClause &
                                 whereClause &
                                 "AND kykh.kkbn_id = 1 AND kykm.saikaisu = 0 " &
                                 "AND kykm.kjkbn_id = 2 AND kykm.kari_ritu_ms_f = True "

            If RecBase = RecTiming.SmdtBase Then
                sql4 &= "AND kykm.b_smdt_fst_sum <= @dtTo"
            Else
                sql4 &= "AND kykm.b_shdt_fst_sum <= @dtTo"
            End If

            sqls.Add(sql4)
        End If

        ' sql5: 維持管理費用（月割按分: 維持管理費 / リース期間 * 期間月数）
        If CheckRecFlags(4) Then
            Dim sql5 As String = "SELECT " &
                                 selectPart1 &
                                 "'維持管理費用' AS 行区, " &
                                 selectPart2 &
                                 "CASE WHEN kykh.lkikan > 0 THEN " &
                                 "ROUND(kykm.b_ijiknr / kykh.lkikan * " & periodMonths & ", 0) " &
                                 "ELSE kykm.b_ijiknr END AS 期間計上額 " &
                                 fromClause &
                                 whereClause &
                                 "AND kykm.b_ijiknr <> 0 AND kykm.kjkbn_id = 2 " &
                                 "AND kykh.kkbn_id = 1 AND kykm.saikaisu = 0"

            sqls.Add(sql5)
        End If

        ' sql6: 減損損失（現時点では固定値0）
        If CheckRecFlags(5) Then
            Dim sql6 As String = "SELECT " &
                                 selectPart1 &
                                 "'減損損失' AS 行区, " &
                                 selectPart2 &
                                 "0 AS 期間計上額 " &
                                 fromClause &
                                 whereClause &
                                 "AND kykm.b_gson_f = True AND kykh.kkbn_id = 1 AND kykm.saikaisu = 0"

            sqls.Add(sql6)
        End If

        ' sql7: 減損勘定取崩額（現時点では固定値0）
        If CheckRecFlags(6) Then
            Dim sql7 As String = "SELECT " &
                                 selectPart1 &
                                 "'減損勘定取崩額' AS 行区, " &
                                 selectPart2 &
                                 "0 AS 期間計上額 " &
                                 fromClause &
                                 whereClause &
                                 "AND kykm.b_gson_f = True AND kykh.kkbn_id = 1 AND kykm.saikaisu = 0"

            sqls.Add(sql7)
        End If

        Dim sql = String.Join(" UNION ALL ", sqls)

        ' 金額符号フィルタ（マイナス/プラス選択に応じて絞り込み）
        If RadioSymbol = Symbol.Minus Then
            sql = "SELECT * FROM (" & sql & ") sub WHERE sub.期間計上額 < 0 "
        ElseIf RadioSymbol = Symbol.Plus Then
            sql = "SELECT * FROM (" & sql & ") sub WHERE sub.期間計上額 > 0 "
        End If

        sql &= " "
        sql &= "ORDER BY kykm_id;"   'kykm.にするとエラーが出る

        Return sql
    End Function

    Private Sub ApplyGridStyle()
        dgv_LIST.HideColumns("kykm_id", "kykh_id")

        dgv_LIST.FormatColumn("期間計上額", FMT_CURRENCY)
    End Sub

    ' グレーアウト判定（期間外かつ計上額0の行を灰色表示）
    Private Sub ApplyGrayOut()
        For Each row As DataGridViewRow In dgv_LIST.Rows
            If row.IsNewRow Then Continue For

            Dim startDt = If(IsDBNull(row.Cells("開始日").Value), Date.MinValue, CDate(row.Cells("開始日").Value))
            Dim endDt = If(IsDBNull(row.Cells("終了日").Value), Date.MaxValue, CDate(row.Cells("終了日").Value))
            Dim keijoAmount = If(IsDBNull(row.Cells("期間計上額").Value), 0D, CDec(row.Cells("期間計上額").Value))
            Dim isTaisho As Boolean = False

            If startDt <= DtTo AndAlso endDt >= DtFrom Then
                isTaisho = True
            ElseIf keijoAmount <> 0 Then
                isTaisho = True
            End If

            If Not isTaisho Then
                row.DefaultCellStyle.ForeColor = Color.Gray
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
            End If
        Next
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' ラベルテキストを生成
    Private Function GetLabelText()
        ' 移動日
        Dim labelText As String = "移動日:　" & DtFrom.ToString("yyyy/MM") & "～" & DtTo.ToString("yyyy/MM")

        ' 出力対象
        If CheckRecFlags(0) Then
            labelText &= "　支払額"
        End If

        If CheckRecFlags(1) Then
            labelText &= "　保守料"
        End If

        If CheckRecFlags(2) Then
            labelText &= "　償却費"
        End If

        If CheckRecFlags(3) Then
            labelText &= "　支払利息"
        End If

        If CheckRecFlags(4) Then
            labelText &= "　維持管理費用"
        End If

        If CheckRecFlags(5) Then
            labelText &= "　減損損失"
        End If

        If CheckRecFlags(6) Then
            labelText &= "　減損勘定取崩額"
        End If

        If RadioSymbol = Symbol.Minus Then
            labelText &= "　マイナス金額"
        Else
            labelText &= "　プラス金額"
        End If

        Return labelText
    End Function

    ' [再計算]ボタン
    Private Sub cmd_RECALCULATE_Click(sender As Object, e As EventArgs) Handles cmd_RECALCULATE.Click
        Me.Close()
    End Sub

    ' [照会]ボタン
    Private Sub cmd_REF_Click(sender As Object, e As EventArgs) Handles cmd_REF.Click
        Dim selectedRow = dgv_LIST.GetSelectedRow()

        If selectedRow Is Nothing Then
            Return
        End If

        Dim frmBukn As New Form_BuknEntry

        frmBukn.KykmId = Convert.ToDouble(selectedRow.Cells("kykm_id").Value)
        frmBukn.ShowDialog()

        Dim frmContract As New Form_ContractEntry

        frmContract.KykhId = Convert.ToDouble(selectedRow.Cells("kykh_id").Value)
        frmContract.ShowDialog()
    End Sub

    ' グリッドのダブルクリック (Accessの txt_KKNRI1_NM_DblClick 相当)
    Private Sub dgv_LIST_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_LIST.CellDoubleClick
        If e.RowIndex < 0 Then Return

        Dim selectedRow = dgv_LIST.GetSelectedRow()

        If selectedRow Is Nothing Then
            Return
        End If

        Dim frmBukn As New Form_BuknEntry

        frmBukn.KykmId = Convert.ToDouble(selectedRow.Cells("kykm_id").Value)
        frmBukn.ShowDialog()

        Dim frmContract As New Form_ContractEntry

        frmContract.KykhId = Convert.ToDouble(selectedRow.Cells("kykh_id").Value)
        frmContract.ShowDialog()
    End Sub

    ' [検索]ボタン
    Private Sub cmd_SEARCH_Click(sender As Object, e As EventArgs) Handles cmd_SEARCH.Click
        SearchData()
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