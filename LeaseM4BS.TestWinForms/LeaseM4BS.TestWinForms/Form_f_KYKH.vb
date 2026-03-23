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
            val.Add("k_knyukn", ParseDblFromText(txt_KNYUKN.Text))
            val.Add("ryoritu", ParseDblFromText(txt_RYORITU.Text))
            val.Add("k_ijiknr", ParseDblFromText(txt_IJIKNR.Text))
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

    ''' <summary>再リース処理</summary>
    Private Sub cmd_SAILEASE_Click(sender As Object, e As EventArgs) Handles cmd_SAILEASE.Click
        If _kykhId = 0 Then Return
        If MessageBox.Show("再リース処理を実行しますか？" & vbCrLf &
                           "再リース回数が+1されます。",
                           "再リース確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            ' 現在の再リース回数を取得
            Dim currentSaikaisu = NzInt(txt_SAIKAISU.Text)
            Dim newSaikaisu = currentSaikaisu + 1

            _crud.BeginTransaction()

            ' d_kykh.saikaisu を更新
            _crud.ExecuteNonQuery(
                "UPDATE d_kykh SET saikaisu = @saikaisu, k_update_dt = NOW() WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@saikaisu", newSaikaisu),
                    New NpgsqlParameter("@id", _kykhId)
                })

            ' d_haif.saikaisu も更新
            _crud.ExecuteNonQuery(
                "UPDATE d_haif SET saikaisu = @saikaisu WHERE kykm_id IN (SELECT kykm_id FROM d_kykm WHERE kykh_id = @id)",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@saikaisu", newSaikaisu),
                    New NpgsqlParameter("@id", _kykhId)
                })

            ' 解約フラグをリセット (再リース開始)
            _crud.ExecuteNonQuery(
                "UPDATE d_kykm SET b_ckaiyk_f = FALSE, ckaiyk_dt = NULL, " &
                "  ckaiyk_esdt_t = NULL, ckaiyk_esdt_h = NULL, iyaku_kin = NULL " &
                "WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})

            _crud.Commit()

            MessageBox.Show($"再リース処理が完了しました。(再リース回数: {newSaikaisu})",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadContract()
            SetReadOnlyMode()
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("再リースエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

    ''' <summary>再リース取消</summary>
    Private Sub cmd_ROLLBACK_SAI_Click(sender As Object, e As EventArgs) Handles cmd_ROLLBACK_SAI.Click
        If _kykhId = 0 Then Return
        Dim currentSaikaisu = NzInt(txt_SAIKAISU.Text)
        If currentSaikaisu <= 0 Then
            MessageBox.Show("再リース回数が0のため取消できません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("再リースを取り消しますか？" & vbCrLf &
                           $"再リース回数が {currentSaikaisu} → {currentSaikaisu - 1} に戻ります。",
                           "再リース取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
            Return
        End If

        Try
            Dim newSaikaisu = currentSaikaisu - 1

            _crud.BeginTransaction()

            ' d_kykh.saikaisu を更新
            _crud.ExecuteNonQuery(
                "UPDATE d_kykh SET saikaisu = @saikaisu, k_update_dt = NOW() WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@saikaisu", newSaikaisu),
                    New NpgsqlParameter("@id", _kykhId)
                })

            ' d_haif.saikaisu も更新
            _crud.ExecuteNonQuery(
                "UPDATE d_haif SET saikaisu = @saikaisu WHERE kykm_id IN (SELECT kykm_id FROM d_kykm WHERE kykh_id = @id)",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@saikaisu", newSaikaisu),
                    New NpgsqlParameter("@id", _kykhId)
                })

            _crud.Commit()

            MessageBox.Show($"再リースを取り消しました。(再リース回数: {newSaikaisu})",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadContract()
            SetReadOnlyMode()
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("再リース取消エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

    ''' <summary>料率マスタ参照</summary>
    Private Sub cmd_料率_Click(sender As Object, e As EventArgs) Handles cmd_料率.Click
        Try
            Dim dt = _crud.GetDataTable("SELECT * FROM m_ryoritu ORDER BY ryoritu_id")
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("料率データがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Using frm As New Form()
                frm.Text = "料率マスタ一覧"
                frm.Size = New System.Drawing.Size(600, 400)
                frm.StartPosition = FormStartPosition.CenterParent
                Dim dgv As New DataGridView()
                dgv.Dock = DockStyle.Fill
                dgv.ReadOnly = True
                dgv.AllowUserToAddRows = False
                dgv.AllowUserToDeleteRows = False
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                dgv.DataSource = dt
                frm.Controls.Add(dgv)
                frm.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show("料率マスタ取得エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>税率マスタ参照</summary>
    Private Sub cmd_税率_Click(sender As Object, e As EventArgs) Handles cmd_税率.Click
        Try
            Dim dt = _crud.GetDataTable("SELECT zei_kaisei_id, teki_dt_from, teki_dt_to, zritu, kkyak_dt_from, kkyak_dt_to FROM t_zei_kaisei ORDER BY teki_dt_from")
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MessageBox.Show("税率データがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Using frm As New Form()
                frm.Text = "税率改正マスタ一覧"
                frm.Size = New System.Drawing.Size(700, 400)
                frm.StartPosition = FormStartPosition.CenterParent
                Dim dgv As New DataGridView()
                dgv.Dock = DockStyle.Fill
                dgv.ReadOnly = True
                dgv.AllowUserToAddRows = False
                dgv.AllowUserToDeleteRows = False
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                dgv.DataSource = dt
                frm.Controls.Add(dgv)
                frm.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show("税率マスタ取得エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  テキスト→数値ヘルパー (カンマ除去付き、TextBox専用)
    ' =========================================================
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

    ' =========================================================
    '  計算ロジック (Access版 pc_f_KYKH / Form_f_KYKH 移植)
    ' =========================================================

    ''' <summary>終了日を算出してセット (Access版: gSetCTL_END_DT)</summary>
    Private Sub CalcAndSetEndDt()
        Dim startDt As Date?
        Dim d As DateTime
        If DateTime.TryParse(txt_START_DT.Text, d) Then startDt = d
        Dim lkikan = NzInt(txt_LKIKAN.Text)
        Dim endDt = ContractCalcHelper.CalcEndDt(startDt, lkikan)
        txt_SHRI_EN_DT.Text = If(endDt IsNot Nothing, endDt.Value.ToString("yyyy/MM/dd"), "")
    End Sub

    ''' <summary>支払回数を算出してセット (Access版: mSetCTL_SHRI_CNT)</summary>
    Private Sub CalcAndSetShriCnt()
        If chk_JENCHO_F.Checked Then Return  ' 延長フラグON時は計算しない
        Dim lkikan = NzInt(txt_LKIKAN.Text)
        Dim mkaisu = NzInt(txt_MKAISU.Text)
        Dim shriKn = NzInt(txt_SHRI_KN.Text)
        Dim cnt = ContractCalcHelper.CalcShriCnt(lkikan, mkaisu, shriKn)
        If cnt >= 0 Then txt_SHRI_CNT.Text = cnt.ToString()
    End Sub

    ''' <summary>総額を算出してセット (Access版: gCALC_SLSRYO)</summary>
    Private Sub CalcAndSetSlsryo()
        Dim klsryo = NzDbl(txt_KLSRYO.Text)
        Dim shriCnt = NzInt(txt_SHRI_CNT.Text)
        Dim mlsryo = NzDbl(txt_MLSRYO.Text)
        Dim henlSum = NzDbl(txt_HENL_SUM.Text)
        txt_SLSRYO.Text = ContractCalcHelper.CalcSlsryo(klsryo, shriCnt, mlsryo, henlSum).ToString("N0")
    End Sub

    ''' <summary>月額の税額・税込を連動計算</summary>
    Private Sub CalcGlsryoZei()
        Dim glsryo = NzDbl(txt_GLSRYO.Text)
        Dim zritu = GetCurrentZritu()
        Dim gzei = ContractCalcHelper.CalcZei(glsryo, zritu)
        txt_GZEI.Text = gzei.ToString("N0")
        txt_GLSRYO_ZKOMI.Text = ContractCalcHelper.CalcZkomi(glsryo, gzei).ToString("N0")
    End Sub

    ''' <summary>1回額の税額・税込を連動計算</summary>
    Private Sub CalcKlsryoZei()
        Dim klsryo = NzDbl(txt_KLSRYO.Text)
        Dim zritu = GetCurrentZritu()
        Dim kzei = ContractCalcHelper.CalcZei(klsryo, zritu)
        txt_KZEI.Text = kzei.ToString("N0")
        txt_KLSRYO_ZKOMI.Text = ContractCalcHelper.CalcZkomi(klsryo, kzei).ToString("N0")
    End Sub

    ''' <summary>前払額の税額・税込を連動計算</summary>
    Private Sub CalcMlsryoZei()
        Dim mlsryo = NzDbl(txt_MLSRYO.Text)
        Dim zritu = GetCurrentZritu()
        Dim mzei = ContractCalcHelper.CalcZei(mlsryo, zritu)
        txt_MZEI.Text = mzei.ToString("N0")
        txt_MLSRYO_ZKOMI.Text = ContractCalcHelper.CalcZkomi(mlsryo, mzei).ToString("N0")
    End Sub

    ''' <summary>現在の消費税率を取得 (画面上のテキストから)</summary>
    Private Function GetCurrentZritu() As Double
        ' TODO: ComboBox化後はSelectedValueから取得
        Dim v As Double
        If Double.TryParse(txt_K_ZOKUSEI1.Tag?.ToString(), v) Then Return v
        ' フォールバック: 10%
        Return 0.1
    End Function

    ''' <summary>消費税率を自動セット (Access版: mSetCTL_ZRITU)</summary>
    Private Sub AutoSetZritu()
        Try
            Dim startDt As Date? = Nothing
            Dim endDt As Date? = Nothing
            Dim d As DateTime
            If DateTime.TryParse(txt_START_DT.Text, d) Then startDt = d
            If DateTime.TryParse(txt_SHRI_EN_DT.Text, d) Then endDt = d
            Dim saikaisu = NzInt(txt_SAIKAISU.Text)

            Using helper As New TaxRateHelper()
                Dim rate = helper.GetZrituForKykh(0, Nothing, startDt, endDt, saikaisu)
                If rate IsNot Nothing Then
                    txt_K_ZOKUSEI1.Tag = rate.Value  ' 税率を保持
                End If
            End Using
        Catch
            ' 税率取得失敗時は無視
        End Try
    End Sub

    ' =========================================================
    '  AfterUpdate イベント連動 (Access版の連鎖再計算を再現)
    ' =========================================================

    Private Sub txt_START_DT_Leave(sender As Object, e As EventArgs) Handles txt_START_DT.Leave
        If Not _isEditMode Then Return
        CalcAndSetEndDt()
        AutoSetZritu()
    End Sub

    Private Sub txt_LKIKAN_Leave(sender As Object, e As EventArgs) Handles txt_LKIKAN.Leave
        If Not _isEditMode Then Return
        CalcAndSetEndDt()
        CalcAndSetShriCnt()
        AutoSetZritu()
    End Sub

    Private Sub txt_SHRI_KN_Leave(sender As Object, e As EventArgs) Handles txt_SHRI_KN.Leave
        If Not _isEditMode Then Return
        ' 支払間隔変更確認
        If NzInt(txt_SHRI_KN.Text) > 0 AndAlso NzDbl(txt_KLSRYO.Text) > 0 Then
            If MessageBox.Show("支払間隔を変更するとリース料が再計算されます。よろしいですか？",
                               "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return
            End If
        End If
        CalcAndSetShriCnt()
    End Sub

    Private Sub txt_SHRI_CNT_Leave(sender As Object, e As EventArgs) Handles txt_SHRI_CNT.Leave
        If Not _isEditMode Then Return
        CalcAndSetSlsryo()
    End Sub

    Private Sub txt_MKAISU_Leave(sender As Object, e As EventArgs) Handles txt_MKAISU.Leave
        If Not _isEditMode Then Return
        CalcAndSetShriCnt()
    End Sub

    Private Sub txt_GLSRYO_Leave(sender As Object, e As EventArgs) Handles txt_GLSRYO.Leave
        If Not _isEditMode Then Return
        CalcGlsryoZei()
    End Sub

    Private Sub txt_KLSRYO_Leave(sender As Object, e As EventArgs) Handles txt_KLSRYO.Leave
        If Not _isEditMode Then Return
        CalcKlsryoZei()
        CalcAndSetSlsryo()
    End Sub

    Private Sub txt_MLSRYO_Leave(sender As Object, e As EventArgs) Handles txt_MLSRYO.Leave
        If Not _isEditMode Then Return
        CalcMlsryoZei()
        CalcAndSetSlsryo()
    End Sub

    Private Sub chk_JENCHO_F_CheckedChanged(sender As Object, e As EventArgs) Handles chk_JENCHO_F.CheckedChanged
        If Not _isEditMode Then Return
        CalcAndSetShriCnt()
        CalcAndSetSlsryo()
    End Sub

    Private Sub txt_KYAK_DT_Leave(sender As Object, e As EventArgs) Handles txt_KYAK_DT.Leave
        If Not _isEditMode Then Return
        ' 契約日→開始日デフォルト (開始日が空なら契約日をコピー)
        If String.IsNullOrWhiteSpace(txt_START_DT.Text) Then
            txt_START_DT.Text = txt_KYAK_DT.Text
            CalcAndSetEndDt()
        End If
        AutoSetZritu()
    End Sub

    Private Sub Form_f_KYKH_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
