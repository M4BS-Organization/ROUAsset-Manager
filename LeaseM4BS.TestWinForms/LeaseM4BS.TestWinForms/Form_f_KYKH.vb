Imports System.Globalization
Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 契約書主画面 (f_KYKH)
''' Access版 f_KYKH / pc_f_KYKH 相当。
''' d_kykh を表示・編集する。物件画面(f_KYKM)へのナビゲーション。
''' </summary>
Partial Public Class Form_f_KYKH
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0
    Private _isEditMode As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部から契約ID をセット</summary>
    Public Sub SetParams(kykhId As Double)
        _kykhId = kykhId
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KYKH_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContract()
        SetReadOnlyMode()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Private Sub LoadContract()
        If _kykhId = 0 Then Return
        Try
            Dim sql As String =
                "SELECT h.*, " &
                "       lc.lcpt1_nm, kk.kknri1_nm, ko.koza_nm, r1.rsrvk1_nm " &
                "FROM d_kykh h " &
                "LEFT JOIN m_lcpt lc ON lc.lcpt_id = h.lcpt_id " &
                "LEFT JOIN m_kknri kk ON kk.kknri_id = h.kknri_id " &
                "LEFT JOIN m_koza ko ON ko.koza_id = h.koza_id " &
                "LEFT JOIN m_rsrvk1 r1 ON r1.rsrvk1_id = h.rsrvk1_id " &
                "WHERE h.kykh_id = @id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)}
            Dim dt = _crud.GetDataTable(sql, prm)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return
            Dim r = dt.Rows(0)

            txt_KYKH_ID.Text = _kykhId.ToString()
            txt_KYKH_NO.Text = NzStr(r("kykbnj"))
            txt_KYKBNJ.Text = NzStr(r("kykbnj"))
            txt_KYKBNL.Text = NzStr(r("kykbnl"))
            txt_KYAK_NM.Text = NzStr(r("kyak_nm"))
            txt_KYAK_DT.Text = NzDtStr(r("kyak_dt"))
            txt_START_DT.Text = NzDtStr(r("start_dt"))
            txt_SHRI_EN_DT.Text = NzDtStr(r("shri_en_dt"))
            txt_LKIKAN.Text = NzStr(r("lkikan"))
            txt_SHRI_KN.Text = NzStr(r("shri_kn"))
            txt_SHRI_CNT.Text = NzStr(r("shri_cnt"))
            txt_SHRI_DT1.Text = NzDtStr(r("shri_dt1"))
            txt_SHRI_DT2.Text = NzDtStr(r("shri_dt2"))
            txt_SHRI_DT3.Text = NzStr(r("shri_dt3"))  ' smallint(日)
            txt_KNYUKN.Text = NzAmtStr(r("k_knyukn"))
            txt_RYORITU.Text = NzStr(r("ryoritu"))
            txt_SLSRYO.Text = NzAmtStr(r("k_slsryo"))
            txt_GLSRYO.Text = NzAmtStr(r("k_glsryo"))
            txt_KLSRYO.Text = NzAmtStr(r("k_klsryo"))
            txt_MLSRYO.Text = NzAmtStr(r("k_mlsryo"))
            txt_IJIKNR.Text = NzAmtStr(r("k_ijiknr"))
            txt_ZANRYO.Text = NzAmtStr(r("k_zanryo"))
            txt_GZEI.Text = NzAmtStr(r("k_gzei"))
            txt_KZEI.Text = NzAmtStr(r("k_kzei"))
            txt_MZEI.Text = NzAmtStr(r("k_mzei"))
            txt_GLSRYO_ZKOMI.Text = NzAmtStr(r("k_glsryo_zkomi"))
            txt_KLSRYO_ZKOMI.Text = NzAmtStr(r("k_klsryo_zkomi"))
            txt_MLSRYO_ZKOMI.Text = NzAmtStr(r("k_mlsryo_zkomi"))
            txt_SAIKAISU.Text = NzStr(r("saikaisu"))
            txt_MKAISU.Text = NzStr(r("mkaisu"))
            txt_MAE_DT.Text = NzDtStr(r("mae_dt"))
            txt_KYKM_CNT.Text = NzStr(r("kykm_cnt"))
            txt_SUURYO.Text = NzStr(r("k_suuryo"))
            txt_UPDATE_CNT.Text = NzStr(r("update_cnt"))
            txt_K_CREATE_DT.Text = NzDtStr(r("k_create_dt"))
            txt_K_UPDATE_DT.Text = NzDtStr(r("k_update_dt"))
            ' 名称フィールド
            txt_LCPT_NM.Text = NzStr(r("lcpt1_nm"))
            ｔｘｔ_KKNRI1_NM.Text = NzStr(r("kknri1_nm"))
            txt_KOZA_NM.Text = NzStr(r("koza_nm"))
            txt_RSRVK1_NM.Text = NzStr(r("rsrvk1_nm"))
            ' 稟議
            txt_RNG_BANGO.Text = NzStr(r("rng_bango"))
            txt_KIANSHA.Text = NzStr(r("kiansha"))
            txt_SHONIN_DT.Text = NzDtStr(r("shonin_dt"))
            ' 費用計上
            txt_TEKIYO_DT.Text = NzDtStr(r("kj_tekiyo_dt"))
            txt_K_KJYO_ST_DT.Text = NzDtStr(r("k_kjyo_st_dt"))
            txt_K_KJYO_EN_DT.Text = NzDtStr(r("k_kjyo_en_dt"))
            ' 払額（〆）
            txt_SSHRI_KN_M.Text = NzStr(r("sshri_kn_m"))
            txt_SSHRI_KN_1.Text = NzStr(r("sshri_kn_1"))
            txt_SSHRI_KN_2.Text = NzStr(r("sshri_kn_2"))
            txt_SSHRI_KN_3.Text = NzStr(r("sshri_kn_3"))
            ' 増減補正
            txt_SLSRYO_KZOK.Text = NzAmtStr(r("slsryo_kzok"))
            txt_GLSRYO_KZOK.Text = NzAmtStr(r("glsryo_kzok"))
            txt_KLSRYO_KZOK.Text = NzAmtStr(r("klsryo_kzok"))
            txt_MLSRYO_KZOK.Text = NzAmtStr(r("mlsryo_kzok"))
            txt_IJIKNR_KZOK.Text = NzAmtStr(r("ijiknr_kzok"))
            txt_ZANRYO_KZOK.Text = NzAmtStr(r("zanryo_kzok"))
            txt_KNYUKN_KZOK.Text = NzAmtStr(r("knyukn_kzok"))
            txt_KYKM_CNT_KZOK.Text = NzStr(r("kykm_cnt_kzok"))
            txt_SUURYO_KZOK.Text = NzStr(r("suuryo_kzok"))
            txt_HENL_SUM.Text = NzAmtStr(r("k_henl_sum"))
            txt_HENL_SUM_KZOK.Text = NzAmtStr(r("henl_sum_kzok"))
            ' フラグ
            chk_JENCHO_F.Checked = NzBool(r("jencho_f"))
            chk_CKAIYK_F.Checked = NzBool(r("k_ckaiyk_f"))
            chk_K_SEIGOU_F.Checked = NzBool(r("k_seigou_f"))
            chk_K_HENF_F.Checked = NzBool(r("k_henf_f"))
            chk_K_HENL_F.Checked = NzBool(r("k_henl_f"))
            chk_KJKBN_MS_F.Checked = NzBool(r("kjkbn_ms_f"))
            chk_SKYU_KJ_F.Checked = (NzInt(r("skyu_kj_f")) = 1)
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  モード管理
    ' =========================================================
    Private Sub SetReadOnlyMode()
        _isEditMode = False
        txt_Mode.Text = "照会"
        SetControlsEnabled(False)
        cmd_TOUROKU.Enabled = False
        cmd_REVISE.Enabled = (_kykhId > 0)
        cmd_DELETE.Enabled = (_kykhId > 0)
        cmd_MODE_RESET.Enabled = False
    End Sub

    Private Sub SetEditMode()
        _isEditMode = True
        txt_Mode.Text = "修正"
        SetControlsEnabled(True)
        cmd_TOUROKU.Enabled = True
        cmd_REVISE.Enabled = False
        cmd_DELETE.Enabled = False
        cmd_MODE_RESET.Enabled = True
    End Sub

    ''' <summary>入力可能フィールドのEnabled/ReadOnly を切り替える</summary>
    Private Sub SetControlsEnabled(enabled As Boolean)
        ' 常に ReadOnly のフィールド（ID・表示専用）
        Dim readOnlyNames As New HashSet(Of String) From {
            "txt_KYKH_ID", "txt_KYKH_NO", "txt_Mode", "txt_INF",
            "txt_LCPT_NM", "ｔｘｔ_KKNRI1_NM", "txt_KOZA_NM", "txt_RSRVK1_NM",
            "txt_K_CREATE_DT", "txt_K_UPDATE_DT", "txt_KJKBN_NM_DUMMY",
            "txt_KYKM_CNT", "txt_SUURYO", "txt_UPDATE_CNT",
            "txt_SLSRYO", "txt_GLSRYO", "txt_KLSRYO", "txt_MLSRYO",
            "txt_GZEI", "txt_KZEI", "txt_MZEI",
            "txt_GLSRYO_ZKOMI", "txt_KLSRYO_ZKOMI", "txt_MLSRYO_ZKOMI",
            "txt_SLSRYO_KZOK", "txt_GLSRYO_KZOK", "txt_KLSRYO_KZOK", "txt_MLSRYO_KZOK",
            "txt_IJIKNR_KZOK", "txt_ZANRYO_KZOK", "txt_KNYUKN_KZOK",
            "txt_KYKM_CNT_KZOK", "txt_SUURYO_KZOK", "txt_HENL_SUM_KZOK", "txt_HENL_SUM"
        }
        For Each ctl As Control In Me.Controls
            If TypeOf ctl Is TextBox Then
                Dim txt = CType(ctl, TextBox)
                txt.ReadOnly = readOnlyNames.Contains(txt.Name) OrElse Not enabled
            ElseIf TypeOf ctl Is CheckBox Then
                CType(ctl, CheckBox).Enabled = enabled
            End If
        Next
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    ''' <summary>変更登録 (UPDATE d_kykh)</summary>
    Private Sub cmd_TOUROKU_Click(sender As Object, e As EventArgs) Handles cmd_TOUROKU.Click
        If _kykhId = 0 Then Return
        If MessageBox.Show("変更を保存しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

        Try
            Dim val As New Dictionary(Of String, Object)
            val.Add("kykbnj", txt_KYKBNJ.Text.Trim())
            val.Add("kykbnl", txt_KYKBNL.Text.Trim())
            val.Add("kyak_nm", txt_KYAK_NM.Text.Trim())
            val.Add("kyak_dt", ParseDt(txt_KYAK_DT.Text))
            val.Add("start_dt", ParseDt(txt_START_DT.Text))
            val.Add("shri_en_dt", ParseDt(txt_SHRI_EN_DT.Text))
            val.Add("k_rend_dt", ParseDt(txt_SHRI_EN_DT.Text))
            val.Add("lkikan", NzInt(txt_LKIKAN.Text))
            val.Add("shri_kn", NzInt(txt_SHRI_KN.Text))
            val.Add("shri_cnt", NzInt(txt_SHRI_CNT.Text))
            val.Add("shri_dt1", ParseDt(txt_SHRI_DT1.Text))
            val.Add("shri_dt2", ParseDt(txt_SHRI_DT2.Text))
            val.Add("shri_dt3", NzInt(txt_SHRI_DT3.Text))  ' smallint
            val.Add("k_knyukn", NzDbl(txt_KNYUKN.Text))
            val.Add("ryoritu", NzDbl(txt_RYORITU.Text))
            val.Add("k_ijiknr", NzDbl(txt_IJIKNR.Text))
            val.Add("mkaisu", NzInt(txt_MKAISU.Text))
            val.Add("mae_dt", ParseDt(txt_MAE_DT.Text))
            val.Add("rng_bango", txt_RNG_BANGO.Text.Trim())
            val.Add("kiansha", txt_KIANSHA.Text.Trim())
            val.Add("shonin_dt", ParseDt(txt_SHONIN_DT.Text))
            val.Add("jencho_f", chk_JENCHO_F.Checked)
            val.Add("k_ckaiyk_f", chk_CKAIYK_F.Checked)
            val.Add("k_seigou_f", chk_K_SEIGOU_F.Checked)
            val.Add("k_henf_f", chk_K_HENF_F.Checked)
            val.Add("k_henl_f", chk_K_HENL_F.Checked)
            val.Add("kjkbn_ms_f", chk_KJKBN_MS_F.Checked)
            val.Add("skyu_kj_f", If(chk_SKYU_KJ_F.Checked, 1, 0))
            val.Add("k_update_dt", DateTime.Now)

            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kykh_id", _kykhId)
            }
            _crud.Update("d_kykh", val, "kykh_id = @kykh_id", whereParams)

            MessageBox.Show("保存しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadContract()
            SetReadOnlyMode()
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>修正モードに切り替え</summary>
    Private Sub cmd_REVISE_Click(sender As Object, e As EventArgs) Handles cmd_REVISE.Click
        SetEditMode()
    End Sub

    ''' <summary>再リース/返却（未実装）</summary>
    Private Sub cmd_SAILEASE_Click(sender As Object, e As EventArgs) Handles cmd_SAILEASE.Click
        MessageBox.Show("再リース/返却機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>中途解約：物件がある場合 f_KAIYAK を開く</summary>
    Private Sub cmd_KAIYAKU_Click(sender As Object, e As EventArgs) Handles cmd_KAIYAKU.Click
        If _kykhId = 0 Then Return
        Try
            Dim dtCheck = _crud.GetDataTable(
                "SELECT COUNT(*) FROM d_kykm WHERE kykh_id = @id AND b_ckaiyk_f = FALSE",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            If dtCheck IsNot Nothing AndAlso Convert.ToInt32(dtCheck.Rows(0)(0)) > 0 Then
                Dim frm As New Form_f_KAIYAK()
                frm.SetParams(_kykhId)
                frm.ShowDialog(Me)
                LoadContract()
            Else
                MessageBox.Show("解約対象の物件がありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>再リース取消（未実装）</summary>
    Private Sub cmd_ROLLBACK_SAI_Click(sender As Object, e As EventArgs) Handles cmd_ROLLBACK_SAI.Click
        MessageBox.Show("再リース取消機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>削除（d_kykh + d_kykm をトランザクションで削除）</summary>
    Private Sub cmd_DELETE_Click(sender As Object, e As EventArgs) Handles cmd_DELETE.Click
        If _kykhId = 0 Then Return
        If MessageBox.Show("このレコードを削除しますか？削除すると物件データも全て削除されます。",
                           "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        Try
            _crud.BeginTransaction()
            _crud.ExecuteNonQuery(
                "DELETE FROM d_kykm WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            _crud.ExecuteNonQuery(
                "DELETE FROM d_kykh WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            _crud.Commit()
            MessageBox.Show("削除しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("削除エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>モードリセット：編集破棄し照会モードに戻る</summary>
    Private Sub cmd_MODE_RESET_Click(sender As Object, e As EventArgs) Handles cmd_MODE_RESET.Click
        If MessageBox.Show("入力中のデータは失われますがよろしいですか？",
                           "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return
        LoadContract()
        SetReadOnlyMode()
    End Sub

    ''' <summary>取込み（未実装）</summary>
    Private Sub cmd_取込_Click(sender As Object, e As EventArgs) Handles cmd_取込.Click
        MessageBox.Show("取込み機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>クリア：画面の入力値をリセット</summary>
    Private Sub cmd_CLEAR_Click(sender As Object, e As EventArgs) Handles cmd_CLEAR.Click
        If MessageBox.Show("クリアしてよろしいですか？",
                           "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return
        LoadContract()
    End Sub

    ''' <summary>物件画面を開く（Form_f_KYKM）</summary>
    Private Sub cmd_物件画面_Click(sender As Object, e As EventArgs) Handles cmd_物件画面.Click
        If _kykhId = 0 Then Return
        Dim frm As New Form_f_KYKM()
        frm.SetParams(_kykhId)
        frm.ShowDialog(Me)
        LoadContract()  ' 戻ったら集計再読み込み
    End Sub

    ''' <summary>料率マスタ参照（未実装）</summary>
    Private Sub cmd_料率_Click(sender As Object, e As EventArgs) Handles cmd_料率.Click
        MessageBox.Show("料率マスタ参照は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>税率マスタ参照（未実装）</summary>
    Private Sub cmd_税率_Click(sender As Object, e As EventArgs) Handles cmd_税率.Click
        MessageBox.Show("税率マスタ参照は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =========================================================
    '  ヘルパー関数
    ' =========================================================
    Private Function NzStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Return v.ToString()
    End Function

    Private Function NzDtStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Dim dt As DateTime
        If DateTime.TryParse(v.ToString(), dt) Then Return dt.ToString("yyyy/MM/dd")
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

    Private Function NzInt(s As String) As Integer
        Dim v As Integer
        If Integer.TryParse(s.Replace(",", "").Trim(), v) Then Return v
        Return 0
    End Function

    Private Function NzDbl(s As String) As Double
        Dim v As Double
        If Double.TryParse(s.Replace(",", "").Trim(), v) Then Return v
        Return 0.0
    End Function

    Private Function ParseDt(s As String) As Object
        If String.IsNullOrWhiteSpace(s) Then Return DBNull.Value
        Dim dt As DateTime
        If DateTime.TryParse(s, dt) Then Return dt
        Return DBNull.Value
    End Function

    Private Sub Form_f_KYKH_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
