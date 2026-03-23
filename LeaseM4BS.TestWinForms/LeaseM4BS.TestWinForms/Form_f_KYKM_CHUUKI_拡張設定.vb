Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 注記拡張設定フォーム (f_KYKM_CHUUKI_拡張設定)
''' d_kykm の計算方法 MS_F フラグ（拡張）を表示・保存する。
''' </summary>
Partial Public Class Form_f_KYKM_CHUUKI_拡張設定
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykmId As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部からパラメータをセット</summary>
    Public Sub SetParams(kykmId As Integer)
        _kykmId = kykmId
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KYKM_CHUUKI_拡張設定_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String = "SELECT * FROM d_kykm WHERE kykm_id = @id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)}
            Dim dt = _crud.GetDataTable(sql, prm)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim r = dt.Rows(0)
            chk_RSOK_TMG_MS_F.Checked = NzBool(r("rsok_tmg_ms_f"))
            chk_GK_CALC_KIND_MS_F.Checked = NzBool(r("gk_calc_kind_ms_f"))
            chk_HENSAI_KIND_MS_F.Checked = NzBool(r("hensai_kind_ms_f"))
            chk_IJ_KJYO_KIND_MS_F.Checked = NzBool(r("ij_kjyo_kind_ms_f"))
            chk_GSON_TK_KIND_MS_F.Checked = NzBool(r("gson_tk_kind_ms_f"))
            chk_LB_KJYO_KIND_MS_F.Checked = NzBool(r("lb_kjyo_kind_ms_f"))
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  MS_F フラグ保存
    ' =========================================================
    Private Sub SaveMsFlags()
        If _kykmId = 0 Then Return
        Try
            Dim sql As String =
                "UPDATE d_kykm SET " &
                "  rsok_tmg_ms_f     = @rsok_tmg_ms_f,     " &
                "  gk_calc_kind_ms_f = @gk_calc_kind_ms_f, " &
                "  hensai_kind_ms_f  = @hensai_kind_ms_f,  " &
                "  ij_kjyo_kind_ms_f = @ij_kjyo_kind_ms_f, " &
                "  gson_tk_kind_ms_f = @gson_tk_kind_ms_f, " &
                "  lb_kjyo_kind_ms_f = @lb_kjyo_kind_ms_f  " &
                "WHERE kykm_id = @id"
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@rsok_tmg_ms_f", chk_RSOK_TMG_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@gk_calc_kind_ms_f", chk_GK_CALC_KIND_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@hensai_kind_ms_f", chk_HENSAI_KIND_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@ij_kjyo_kind_ms_f", chk_IJ_KJYO_KIND_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@gson_tk_kind_ms_f", chk_GSON_TK_KIND_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@lb_kjyo_kind_ms_f", chk_LB_KJYO_KIND_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@id", _kykmId))
            _crud.ExecuteNonQuery(sql, prms)
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        SaveMsFlags()
        Me.Close()
    End Sub

    Private Sub cmd_注記判定_Click(sender As Object, e As EventArgs) Handles cmd_注記判定.Click
        MessageBox.Show("注記判定再計算は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Form_f_KYKM_CHUUKI_拡張設定_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
