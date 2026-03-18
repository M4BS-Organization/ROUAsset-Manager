Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

' =========================================================
' システム利用権限 一覧画面
' Access版 f_flx_SEC_KNGN + qsel_df_flx_SEC_KNGN 相当
' =========================================================
Partial Public Class Form_f_flx_SEC_KNGN
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_SEC_KNGN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        SecurityChecker.ApplyMasterUpdateLimit(Me)
    End Sub

    Private Sub LoadData()
        Try
            Dim sql As String =
                "SELECT k.kngn_cd, k.kngn_nm, " &
                "CASE k.access_kind WHEN 1 THEN '全データ変更' WHEN 2 THEN '全データ参照' WHEN 3 THEN '管理単位限定' ELSE '' END AS access_kind_str, " &
                "CASE k.access_kind_b WHEN 1 THEN '全データ変更' WHEN 2 THEN '全データ参照' WHEN 3 THEN '管理単位限定' ELSE '' END AS access_kind_b_str, " &
                "CASE WHEN k.admin THEN '○' ELSE '' END AS admin, " &
                "CASE WHEN k.master_update THEN '○' ELSE '' END AS master_update, " &
                "CASE WHEN k.approval THEN '○' ELSE '' END AS approval, " &
                "CASE WHEN k.file_output THEN '○' ELSE '' END AS file_output, " &
                "CASE WHEN k.print THEN '○' ELSE '' END AS print, " &
                "CASE WHEN k.log_ref THEN '○' ELSE '' END AS log_ref, " &
                "k.biko, k.create_dt, k.update_dt, k.kngn_id, k.kngn_id AS id " &
                "FROM sec_kngn k " &
                "WHERE k.history_f = FALSE " &
                "ORDER BY k.kngn_cd"

            dgvMain.AutoGenerateColumns = False
            dgvMain.DataSource = _crud.GetDataTable(sql)
        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    Private Sub cmd_新規_Click(sender As Object, e As EventArgs) Handles cmd_新規.Click
        Dim frm As New Form_f_SEC_KNGN_INP()
        frm.EditMode = "NEW"
        frm.ShowDialog()
        LoadData()
    End Sub

    Private Sub cmd_変更_Click(sender As Object, e As EventArgs) Handles cmd_変更.Click
        If dgvMain.CurrentRow Is Nothing Then Return
        Dim kngnId As Integer = CInt(dgvMain.CurrentRow.Cells("txt_KNGN_ID").Value)

        Dim frm As New Form_f_SEC_KNGN_INP()
        frm.EditMode = "EDIT"
        frm.TargetKngnId = kngnId
        frm.ShowDialog()
        LoadData()
    End Sub

    Private Sub cmd_再表示_Click(sender As Object, e As EventArgs) Handles cmd_再表示.Click
        LoadData()
    End Sub

    Private Sub cmd_Output_Click(sender As Object, e As EventArgs) Handles cmd_Output.Click
        Try
            Dim frm As New Form_f_FlexOutputDLG()
            frm.Dgv = dgvMain
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("出力エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_FlexSearchDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexSearchDLG.Click
        Dim keyword As String = InputBox("検索文字列を入力してください:", "検索")
        If String.IsNullOrEmpty(keyword) Then Return
        Try
            Dim sql As String =
                "SELECT k.kngn_cd, k.kngn_nm, " &
                "CASE k.access_kind WHEN 1 THEN '全データ変更' WHEN 2 THEN '全データ参照' WHEN 3 THEN '管理単位限定' ELSE '' END AS access_kind_str, " &
                "CASE k.access_kind_b WHEN 1 THEN '全データ変更' WHEN 2 THEN '全データ参照' WHEN 3 THEN '管理単位限定' ELSE '' END AS access_kind_b_str, " &
                "CASE WHEN k.admin THEN '○' ELSE '' END AS admin, " &
                "CASE WHEN k.master_update THEN '○' ELSE '' END AS master_update, " &
                "CASE WHEN k.approval THEN '○' ELSE '' END AS approval, " &
                "CASE WHEN k.file_output THEN '○' ELSE '' END AS file_output, " &
                "CASE WHEN k.print THEN '○' ELSE '' END AS print, " &
                "CASE WHEN k.log_ref THEN '○' ELSE '' END AS log_ref, " &
                "k.biko, k.create_dt, k.update_dt, k.kngn_id, k.kngn_id AS id " &
                "FROM sec_kngn k " &
                "WHERE k.history_f = FALSE " &
                "AND (k.kngn_cd LIKE @kw OR k.kngn_nm LIKE @kw) " &
                "ORDER BY k.kngn_cd"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kw", $"%{keyword}%")
            }
            dgvMain.AutoGenerateColumns = False
            dgvMain.DataSource = _crud.GetDataTable(sql, prms)
        Catch ex As Exception
            MessageBox.Show("検索エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_FlexSortDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexSortDLG.Click
        MessageBox.Show("列ヘッダーをクリックすると並べ替えできます。", "並べ替え",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub dgvMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMain.CellDoubleClick
        If e.RowIndex < 0 Then Return
        cmd_変更_Click(sender, EventArgs.Empty)
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub

End Class
