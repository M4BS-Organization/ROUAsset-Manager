Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 注記フォーム (f_KYKM_CHUUKI)
''' d_kykm の注記計上判定結果・フラグ一覧を表示し、MS_F フラグを保存する。
''' </summary>
Partial Public Class Form_f_KYKM_CHUUKI
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
    Private Sub Form_f_KYKM_CHUUKI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String =
                "SELECT m.*, h.lkikan, ch.chu_hnti_nm " &
                "FROM d_kykm m " &
                "LEFT JOIN d_kykh h ON h.kykh_id = m.kykh_id " &
                "LEFT JOIN c_chu_hnti ch ON ch.chu_hnti_id = m.chu_hnti_id " &
                "WHERE m.kykm_id = @id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)}
            Dim dt = _crud.GetDataTable(sql, prm)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim r = dt.Rows(0)
            txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
            txt_B_GNZAI_KT.Text = NzAmtStr(r("b_gnzai_kt"))
            txt_B_GHASSEI.Text = NzAmtStr(r("b_ghassei"))
            txt_B_ZANRYO.Text = NzAmtStr(r("b_zanryo"))
            txt_KARI_RITU.Text = NzStr(r("kari_ritu"))
            txt_B_KNYUKN.Text = NzAmtStr(r("b_knyukn"))
            txt_B_KNYUKN_2.Text = NzAmtStr(r("b_knyukn"))
            txt_B_SYUTOK.Text = NzAmtStr(r("b_syutok"))
            txt_KSAN_RITU.Text = NzStr(r("ksan_ritu"))
            txt_RSLT90P_STR.Text = NzStr(r("rslt90p_str"))
            txt_RSLT75P_STR.Text = NzStr(r("rslt75p_str"))
            txt_TAIYO_NEN.Text = NzStr(r("taiyo_nen"))
            txt_CHU_HNTI_NM.Text = NzStr(r("chu_hnti_nm"))
            txt_B_LB_SONEKI.Text = NzAmtStr(r("b_lb_soneki"))
            txt_KJ_KKAKAKU.Text = NzAmtStr(r("kj_kkakaku"))
            txt_KJ_KSAN_RITU.Text = NzStr(r("kj_ksan_ritu"))
            テキスト107.Text = NzAmtStr(r("b_syutok"))

            ' リース期間 (d_kykh.lkikan)
            Dim lkikanVal As Integer = 0
            Integer.TryParse(NzStr(r("lkikan")), lkikanVal)
            txt_LKIKAN.Text = lkikanVal.ToString()
            txt_LKIKAN_NEN.Text = If(lkikanVal > 0, Math.Round(lkikanVal / 12.0, 1).ToString("N1"), "")

            ' 自動設定禁止フラグ (チェックボックス)
            chk_LEAKBN_ID_MS_F.Checked = NzBool(r("leakbn_id_ms_f"))
            chk_TAIYO_NEN_MS_F.Checked = NzBool(r("taiyo_nen_ms_f"))
            chk_CHUUM_ID_MS_F.Checked = NzBool(r("chuum_id_ms_f"))
            chk_KARI_RITU_MS_F.Checked = NzBool(r("kari_ritu_ms_f"))
            chk_LB_CHUKI_F.Checked = NzBool(r("lb_chuki_f"))
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
                "  leakbn_id_ms_f    = @leakbn_id_ms_f,    " &
                "  taiyo_nen_ms_f    = @taiyo_nen_ms_f,    " &
                "  chuum_id_ms_f     = @chuum_id_ms_f,     " &
                "  kari_ritu_ms_f    = @kari_ritu_ms_f,    " &
                "  lb_chuki_f        = @lb_chuki_f,        " &
                "  rsok_tmg_ms_f     = @rsok_tmg_ms_f,     " &
                "  gk_calc_kind_ms_f = @gk_calc_kind_ms_f, " &
                "  hensai_kind_ms_f  = @hensai_kind_ms_f,  " &
                "  ij_kjyo_kind_ms_f = @ij_kjyo_kind_ms_f, " &
                "  gson_tk_kind_ms_f = @gson_tk_kind_ms_f, " &
                "  lb_kjyo_kind_ms_f = @lb_kjyo_kind_ms_f  " &
                "WHERE kykm_id = @id"
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@leakbn_id_ms_f", chk_LEAKBN_ID_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@taiyo_nen_ms_f", chk_TAIYO_NEN_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@chuum_id_ms_f", chk_CHUUM_ID_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@kari_ritu_ms_f", chk_KARI_RITU_MS_F.Checked))
            prms.Add(New NpgsqlParameter("@lb_chuki_f", chk_LB_CHUKI_F.Checked))
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

    Private Sub cmd_拡張設定_Click(sender As Object, e As EventArgs) Handles cmd_拡張設定.Click
        Dim frm As New Form_f_KYKM_CHUUKI_拡張設定()
        frm.SetParams(_kykmId)
        frm.ShowDialog(Me)
        LoadData()
    End Sub

    Private Sub cmd_GSON_ADD_Click(sender As Object, e As EventArgs) Handles cmd_GSON_ADD.Click
        MessageBox.Show("減損行追加は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_GSON_DEL_Click(sender As Object, e As EventArgs) Handles cmd_GSON_DEL.Click
        Dim frm As New Form_f_KYKM_CHUUKI_SUB_GSON()
        frm.SetParams(_kykmId)
        frm.ShowDialog(Me)

        Dim lineId = frm.SelectedLineId
        If lineId <= 0 Then Return

        If MessageBox.Show(
            $"減損行 (LINE_ID={lineId}) を削除しますか？",
            "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return

        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", _kykmId))
            prms.Add(New NpgsqlParameter("@line_id", lineId))
            _crud.ExecuteNonQuery(
                "DELETE FROM d_gson WHERE kykm_id = @kykm_id AND line_id = @line_id",
                prms)
            MessageBox.Show("減損行を削除しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_注記判定_Click(sender As Object, e As EventArgs) Handles cmd_注記判定.Click
        MessageBox.Show("注記判定再計算は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_SCH_Click(sender As Object, e As EventArgs) Handles cmd_SCH.Click
        MessageBox.Show("返済スケジュールは未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =========================================================
    '  ヘルパー
    ' =========================================================
    Private Function NzStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Return v.ToString()
    End Function

    Private Function NzAmtStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return "0"
        Try : Return Convert.ToDouble(v).ToString("N0")
        Catch : Return v.ToString()
        End Try
    End Function

    Private Function NzBool(v As Object) As Boolean
        If IsDBNull(v) OrElse v Is Nothing Then Return False
        Try : Return Convert.ToBoolean(v)
        Catch : Return False
        End Try
    End Function

    Private Sub Form_f_KYKM_CHUUKI_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
