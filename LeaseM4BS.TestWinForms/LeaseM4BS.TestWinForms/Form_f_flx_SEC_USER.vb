Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

' =========================================================
' システム利用者 一覧画面
' Access版 f_flx_SEC_USER + qsel_df_flx_SEC_USER 相当
' =========================================================
Partial Public Class Form_f_flx_SEC_USER
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_SEC_USER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        ' 権限制御
        SecurityChecker.ApplyMasterUpdateLimit(Me)
    End Sub

    ''' <summary>
    ''' データ読込・グリッド表示
    ''' </summary>
    Private Sub LoadData()
        Try
            Dim sql As String =
                "SELECT u.user_cd, u.user_nm, k.kngn_nm, u.biko, " &
                "u.create_dt, u.update_dt, u.user_id AS id, " &
                "u.history_f, u.login_attempts, u.pwd_life_time, " &
                "u.pwd_grace_time, u.pwd_min, u.pwd_moji_chk, " &
                "u.err_ct, u.last_err_dt " &
                "FROM sec_user u " &
                "LEFT JOIN sec_kngn k ON u.kngn_id = k.kngn_id " &
                "WHERE u.history_f = FALSE " &
                "ORDER BY u.user_cd"

            Dim dt = _crud.GetDataTable(sql)
            dgvMain.AutoGenerateColumns = False
            dgvMain.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' --- 閉じる ---
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    ' --- 新規 ---
    Private Sub cmd_新規_Click(sender As Object, e As EventArgs) Handles cmd_新規.Click
        Dim frm As New Form_f_SEC_USER_INP()
        frm.EditMode = "NEW"
        frm.ShowDialog()
        LoadData()
    End Sub

    ' --- 変更 ---
    Private Sub cmd_変更_Click(sender As Object, e As EventArgs) Handles cmd_変更.Click
        If dgvMain.CurrentRow Is Nothing Then Return
        Dim userId As Integer = CInt(dgvMain.CurrentRow.Cells("txt_ID").Value)

        Dim frm As New Form_f_SEC_USER_INP()
        frm.EditMode = "EDIT"
        frm.TargetUserId = userId
        frm.ShowDialog()
        LoadData()
    End Sub

    ' --- 再表示 ---
    Private Sub cmd_再表示_Click(sender As Object, e As EventArgs) Handles cmd_再表示.Click
        LoadData()
    End Sub

    ' --- ファイル出力 ---
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

    ' --- 検索 ---
    Private Sub cmd_FlexSearchDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexSearchDLG.Click
        ' 簡易検索: InputBoxで検索文字列を取得してフィルタ
        Dim keyword As String = InputBox("検索文字列を入力してください:", "検索")
        If String.IsNullOrEmpty(keyword) Then Return

        Try
            Dim sql As String =
                "SELECT u.user_cd, u.user_nm, k.kngn_nm, u.biko, " &
                "u.create_dt, u.update_dt, u.user_id AS id, " &
                "u.history_f, u.login_attempts, u.pwd_life_time, " &
                "u.pwd_grace_time, u.pwd_min, u.pwd_moji_chk, " &
                "u.err_ct, u.last_err_dt " &
                "FROM sec_user u " &
                "LEFT JOIN sec_kngn k ON u.kngn_id = k.kngn_id " &
                "WHERE u.history_f = FALSE " &
                "AND (u.user_cd LIKE @kw OR u.user_nm LIKE @kw OR k.kngn_nm LIKE @kw) " &
                "ORDER BY u.user_cd"
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

    ' --- 並べ替え ---
    Private Sub cmd_FlexSortDLG_Click(sender As Object, e As EventArgs) Handles cmd_FlexSortDLG.Click
        ' DataGridViewの列ヘッダークリックでソート可能なため、メッセージのみ
        MessageBox.Show("列ヘッダーをクリックすると並べ替えできます。", "並べ替え",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' --- DataGridViewダブルクリックで変更画面を開く ---
    Private Sub dgvMain_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMain.CellDoubleClick
        If e.RowIndex < 0 Then Return
        cmd_変更_Click(sender, EventArgs.Empty)
    End Sub

    ' --- Enterキー処理 ---
    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub

End Class
