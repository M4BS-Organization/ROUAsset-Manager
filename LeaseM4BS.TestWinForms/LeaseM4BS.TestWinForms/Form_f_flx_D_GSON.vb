Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 減損フレックス ---
' Access版 f_flx_D_GSON 相当
Partial Public Class Form_f_flx_D_GSON
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_D_GSON_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SecurityChecker.ApplyDataUpdateLimit(Me)
        SearchData()
    End Sub

    ' 検索処理
    Private Sub SearchData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            Dim searchText = txt_SEARCH.Text.Trim()
            prms.Add(New NpgsqlParameter("@search", searchText))

            Dim sql =
                "SELECT " &
                "dk.kykm_no, ck.kjkbn_nm, dg.line_id, dk.bukn_nm, " &
                "dg.gson_dt, " &
                "CASE dg.gson_tmg WHEN 0 THEN '月度末' WHEN 1 THEN '月度初' END AS gson_tmg_nm, " &
                "dg.gson_ryo, dg.gson_rkei, " &
                "mb.bcat_nm AS kknri1_nm, mb2.bcat_nm AS b_bcat1_nm, " &
                "cc.kkbn_nm, ml.lcpt_nm AS k_lcpt1_nm, " &
                "dkh.kykbnh AS kykbnl, dk.saikaisu, dkh.kishu_dt AS start_dt, " &
                "dk.kykm_id AS id " &
                "FROM d_gson dg " &
                "INNER JOIN d_kykm dk ON dg.kykm_id = dk.kykm_id " &
                "INNER JOIN d_kykh dkh ON dk.kykm_id = dkh.kykm_id " &
                "LEFT JOIN c_kjkbn ck ON dkh.kjkbn_id = ck.kjkbn_id " &
                "LEFT JOIN m_bcat mb ON dk.bcat1_id = mb.bcat_id " &
                "LEFT JOIN m_bcat mb2 ON dk.bcat2_id = mb2.bcat_id " &
                "LEFT JOIN c_kkbn cc ON dkh.kkbn_id = cc.kkbn_id " &
                "LEFT JOIN m_lcpt ml ON dkh.lcpt1_id = ml.lcpt_id " &
                "WHERE (@search = '' OR dk.kykm_no ILIKE '%' || @search || '%' " &
                "    OR dkh.kykbnh ILIKE '%' || @search || '%' " &
                "    OR dk.bukn_nm ILIKE '%' || @search || '%') " &
                "ORDER BY dk.kykm_no, dg.gson_dt"

            dgv_LIST.AutoGenerateColumns = False
            dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)
        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [検索] ボタン
    Private Sub cmd_SEARCH_Click(sender As Object, e As EventArgs) Handles cmd_SEARCH.Click
        SearchData()
    End Sub

    ' [照会] ボタン → 参照サブフォームを開く
    Private Sub cmd_REF_Click(sender As Object, e As EventArgs) Handles cmd_REF.Click
        Dim kykmId = GetSelectedKykmId()
        If kykmId Is Nothing Then Return

        Dim frm As New Form_f_REF_D_KYKM_CHUUKI_SUB_GSON()
        frm.KykmId = CDbl(kykmId)
        frm.ShowDialog()
    End Sub

    ' [変更] ボタン → 編集サブフォームを開く
    Private Sub cmd_CHANGE_Click(sender As Object, e As EventArgs) Handles cmd_CHANGE.Click
        Dim kykmId = GetSelectedKykmId()
        If kykmId Is Nothing Then Return

        Dim frm As New Form_f_KYKM_CHUUKI_SUB_GSON()
        frm.KykmId = CDbl(kykmId)
        frm.ShowDialog()

        ' 変更後にリロード
        SearchData()
    End Sub

    Private Function GetSelectedKykmId() As Object
        If dgv_LIST.SelectedRows.Count = 0 AndAlso dgv_LIST.CurrentRow Is Nothing Then
            MessageBox.Show("行を選択してください。")
            Return Nothing
        End If

        Dim row = If(dgv_LIST.SelectedRows.Count > 0,
                     dgv_LIST.SelectedRows(0),
                     dgv_LIST.CurrentRow)

        If row Is Nothing Then Return Nothing

        ' ID列からkykm_idを取得
        Dim idColIdx = dgv_LIST.Columns("txt_ID")
        If idColIdx IsNot Nothing Then
            Return row.Cells(idColIdx.Index).Value
        End If

        Return Nothing
    End Function

    ' [閉じる] ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' [ファイル出力] ボタン
    Private Sub cmd_OUTPUT_FILE_Click(sender As Object, e As EventArgs) Handles cmd_OUTPUT_FILE.Click
        Dim frm As New Form_f_FlexOutputDLG
        frm.Dgv = dgv_LIST
        frm.ShowDialog()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class