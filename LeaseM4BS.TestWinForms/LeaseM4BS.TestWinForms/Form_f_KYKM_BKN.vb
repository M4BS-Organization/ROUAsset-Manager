Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 物件管理画面（分割管理） (f_KYKM_BKN)
''' kykm_id を受け取り、d_kykm + d_kykh + 各マスターを JOIN して物件情報を表示する。
''' </summary>
Partial Public Class Form_f_KYKM_BKN
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
    Private Sub Form_f_KYKM_BKN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Private Sub LoadData()
        If _kykmId = 0 Then Return
        Try
            Dim sql As String =
                "SELECT m.*, " &
                "       h.start_dt, h.kykbnl, h.k_knyukn, " &
                "       kk.kkbn_nm, " &
                "       sk.skmk_nm, bk.bkind_nm, " &
                "       gs.gsha_nm  AS k_gsha_nm, " &
                "       mc.mcpt_nm, rv.rsrvb1_nm, " &
                "       bc.bcat1_nm,  bc.bcat2_nm,  bc.bcat3_nm,  bc.bcat4_nm,  bc.bcat5_nm, " &
                "       r1.bcat1_nm AS bcat1_nm_r1, r1.bcat2_nm AS bcat2_nm_r1, " &
                "       r1.bcat3_nm AS bcat3_nm_r1, r1.bcat4_nm AS bcat4_nm_r1, " &
                "       r1.bcat5_nm AS bcat5_nm_r1, " &
                "       r2.bcat1_nm AS bcat1_nm_r2, r2.bcat2_nm AS bcat2_nm_r2, " &
                "       r2.bcat3_nm AS bcat3_nm_r2, r2.bcat4_nm AS bcat4_nm_r2, " &
                "       r2.bcat5_nm AS bcat5_nm_r2, " &
                "       r3.bcat1_nm AS bcat1_nm_r3, r3.bcat2_nm AS bcat2_nm_r3, " &
                "       r3.bcat3_nm AS bcat3_nm_r3, r3.bcat4_nm AS bcat4_nm_r3, " &
                "       r3.bcat5_nm AS bcat5_nm_r3, " &
                "       hk.hkho_nm, " &
                "       hkgs.gsha_nm AS hk_gsha_nm " &
                "FROM d_kykm m " &
                "LEFT JOIN d_kykh    h    ON h.kykh_id    = m.kykh_id " &
                "LEFT JOIN c_kkbn    kk   ON kk.kkbn_id   = h.kkbn_id " &
                "LEFT JOIN m_skmk    sk   ON sk.skmk_id   = m.skmk_id " &
                "LEFT JOIN m_bkind   bk   ON bk.bkind_id  = m.bkind_id " &
                "LEFT JOIN m_gsha    gs   ON gs.gsha_id   = m.k_gsha_id " &
                "LEFT JOIN m_mcpt    mc   ON mc.mcpt_id   = m.mcpt_id " &
                "LEFT JOIN m_rsrvb1  rv   ON rv.rsrvb1_id = m.rsrvb1_id " &
                "LEFT JOIN m_bcat    bc   ON bc.bcat_id   = m.b_bcat_id " &
                "LEFT JOIN m_bcat    r1   ON r1.bcat_id   = m.b_bcat_id_r1 " &
                "LEFT JOIN m_bcat    r2   ON r2.bcat_id   = m.b_bcat_id_r2 " &
                "LEFT JOIN m_bcat    r3   ON r3.bcat_id   = m.b_bcat_id_r3 " &
                "LEFT JOIN m_hkho    hk   ON hk.hkho_id   = m.hkho_id " &
                "LEFT JOIN m_gsha    hkgs ON hkgs.gsha_id = m.hk_gsha_id " &
                "WHERE m.kykm_id = @id"
            Dim prm As New List(Of NpgsqlParameter)
            prm.Add(New NpgsqlParameter("@id", _kykmId))
            Dim dt = _crud.GetDataTable(sql, prm)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim r = dt.Rows(0)
            PopulateFields(r)
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  フィールド表示
    ' =========================================================
    Private Sub PopulateFields(r As System.Data.DataRow)
        ' IDs
        txt_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))

        ' 契約情報
        txt_START_DT.Text = NzDtStr(r("start_dt"))
        txt_KYKBNL.Text = NzStr(r("kykbnl"))
        txt_KKBN_NM.Text = NzStr(r("kkbn_nm"))
        txt_KYKBNF.Text = NzStr(r("kykbnf"))
        txt_KYKM_NO.Text = NzStr(r("kykm_no"))
        txt_KYKM_NO_MAE.Text = NzStr(r("kykm_no_mae"))
        txt_KJKBN.Text = NzStr(r("kjkbn"))

        ' 物件情報
        txt_BUKN_NM.Text = NzStr(r("bukn_nm"))
        txt_SUURYO.Text = NzStr(r("suuryo"))
        txt_B_KEDABAN.Text = NzStr(r("b_kedaban"))
        txt_KNYUKN.Text = NzAmtStr(r("knyukn"))
        txt_K_KNYUKN.Text = NzAmtStr(r("k_knyukn"))
        txt_B_TANKA.Text = NzAmtStr(r("b_tanka"))
        txt_B_SAIRYO.Text = NzAmtStr(r("b_sairyo"))
        txt_B_KJYO_EN_DT.Text = NzDtStr(r("b_kjyo_en_dt"))

        ' リース料
        txt_SLSRYO.Text = NzAmtStr(r("slsryo"))
        txt_GLSRYO.Text = NzAmtStr(r("glsryo"))
        txt_KLSRYO.Text = NzAmtStr(r("klsryo"))
        txt_MLSRYO.Text = NzAmtStr(r("mlsryo"))
        txt_IJIKNR.Text = NzAmtStr(r("ijiknr"))
        txt_ZANRYO.Text = NzAmtStr(r("zanryo"))

        ' 消費税
        txt_GZEI.Text = NzAmtStr(r("gzei"))
        txt_KZEI.Text = NzAmtStr(r("kzei"))
        txt_MZEI.Text = NzAmtStr(r("mzei"))
        txt_GLSRYO_ZKOMI.Text = NzAmtStr(r("glsryo_zkomi"))
        txt_KLSRYO_ZKOMI.Text = NzAmtStr(r("klsryo_zkomi"))
        txt_MLSRYO_ZKOMI.Text = NzAmtStr(r("mlsryo_zkomi"))

        ' 変動・解約
        txt_HENL_SUM.Text = NzAmtStr(r("henl_sum"))
        txt_B_HENF_KLSRYO_NEW.Text = NzAmtStr(r("b_henf_klsryo_new"))
        txt_B_HENL_F.Text = NzStr(r("b_henl_f"))
        txt_B_HENF_F.Text = NzStr(r("b_henf_f"))
        txt_HENL_SEDT.Text = NzDtStr(r("henl_sedt"))
        txt_HENF_SEDT.Text = NzDtStr(r("henf_sedt"))
        txt_CKAIYK_DT.Text = NzDtStr(r("ckaiyk_dt"))
        txt_CKAIYK_ESDT_T.Text = NzDtStr(r("ckaiyk_esdt_t"))
        txt_CKAIYK_ESDT_H.Text = NzDtStr(r("ckaiyk_esdt_h"))
        txt_IYAKU_KIN.Text = NzAmtStr(r("iyaku_kin"))

        ' 管理部署（bcat）
        txt_BCAT1_NM.Text = NzStr(r("bcat1_nm"))
        txt_BCAT2_NM.Text = NzStr(r("bcat2_nm"))
        txt_BCAT3_NM.Text = NzStr(r("bcat3_nm"))
        txt_BCAT4_NM.Text = NzStr(r("bcat4_nm"))
        txt_BCAT5_NM.Text = NzStr(r("bcat5_nm"))
        txt_IDO_DT.Text = NzDtStr(r("ido_dt"))
        txt_B_BCAT_NM_K.Text = NzStr(r("b_bcat_nm_k"))

        ' 移動先R1
        txt_BCAT1_NM_R1.Text = NzStr(r("bcat1_nm_r1"))
        txt_BCAT2_NM_R1.Text = NzStr(r("bcat2_nm_r1"))
        txt_BCAT3_NM_R1.Text = NzStr(r("bcat3_nm_r1"))
        txt_BCAT4_NM_R1.Text = NzStr(r("bcat4_nm_r1"))
        txt_BCAT5_NM_R1.Text = NzStr(r("bcat5_nm_r1"))
        txt_IDO_DT_R1.Text = NzDtStr(r("ido_dt_r1"))

        ' 移動先R2
        txt_BCAT1_NM_R2.Text = NzStr(r("bcat1_nm_r2"))
        txt_BCAT2_NM_R2.Text = NzStr(r("bcat2_nm_r2"))
        txt_BCAT3_NM_R2.Text = NzStr(r("bcat3_nm_r2"))
        txt_BCAT4_NM_R2.Text = NzStr(r("bcat4_nm_r2"))
        txt_BCAT5_NM_R2.Text = NzStr(r("bcat5_nm_r2"))
        txt_IDO_DT_R2.Text = NzDtStr(r("ido_dt_r2"))

        ' 移動先R3
        txt_BCAT1_NM_R3.Text = NzStr(r("bcat1_nm_r3"))
        txt_BCAT2_NM_R3.Text = NzStr(r("bcat2_nm_r3"))
        txt_BCAT3_NM_R3.Text = NzStr(r("bcat3_nm_r3"))
        txt_BCAT4_NM_R3.Text = NzStr(r("bcat4_nm_r3"))
        txt_BCAT5_NM_R3.Text = NzStr(r("bcat5_nm_r3"))
        txt_IDO_DT_R3.Text = NzDtStr(r("ido_dt_r3"))

        ' 名称参照
        txt_SKMK_NM.Text = NzStr(r("skmk_nm"))
        txt_BKIND_NM.Text = NzStr(r("bkind_nm"))
        txt_K_GSHA_NM.Text = NzStr(r("k_gsha_nm"))
        txt_MCPT_NM.Text = NzStr(r("mcpt_nm"))
        txt_RSRV_NM.Text = NzStr(r("rsrvb1_nm"))

        ' 廃棄
        txt_HKHO_NM.Text = NzStr(r("hkho_nm"))
        txt_HK_GSHA_NM.Text = NzStr(r("hk_gsha_nm"))
        txt_HK_DT.Text = NzDtStr(r("hk_dt"))

        ' 物件番号
        txt_BUKN_BANGO1.Text = NzStr(r("bukn_bango1"))
        txt_BUKN_BANGO2.Text = NzStr(r("bukn_bango2"))
        txt_BUKN_BANGO3.Text = NzStr(r("bukn_bango3"))
        txt_SETTI_DT.Text = NzDtStr(r("setti_dt"))

        ' 属性
        txt_B_ZOKUSEI1.Text = NzStr(r("b_zokusei1"))
        txt_B_ZOKUSEI2.Text = NzStr(r("b_zokusei2"))
        txt_B_ZOKUSEI3.Text = NzStr(r("b_zokusei3"))
        txt_B_ZOKUSEI4.Text = NzStr(r("b_zokusei4"))
        txt_B_ZOKUSEI5.Text = NzStr(r("b_zokusei5"))

        ' その他
        txt_B_GSON_F.Text = NzStr(r("b_gson_f"))
        txt_F_LCPT_ID.Text = NzStr(r("f_lcpt_id"))
        txt_F_HKMK_ID.Text = NzStr(r("f_hkmk_id"))
        txt_F_GSHA_ID.Text = NzStr(r("f_gsha_id"))
        txt_INF.Text = NzStr(r("inf"))
        txt_B_CREATE_DT.Text = NzDtStr(r("b_create_dt"))
        txt_B_UPDATE_DT.Text = NzDtStr(r("b_update_dt"))
        txt_DMY.Text = NzStr(r("dmy"))
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    Private Sub cmd_TOUROKU_Click(sender As Object, e As EventArgs) Handles cmd_TOUROKU.Click
        If _kykmId = 0 Then Return
        If MessageBox.Show("変更を保存しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return
        Try
            Dim val As New Dictionary(Of String, Object)
            val.Add("bukn_nm", txt_BUKN_NM.Text.Trim())
            val.Add("suuryo", ParseIntFromText(txt_SUURYO.Text))
            val.Add("b_kedaban", txt_B_KEDABAN.Text.Trim())
            val.Add("b_update_dt", DateTime.Now)
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kykm_id", _kykmId)
            }
            _crud.Update("d_kykm", val, "kykm_id = @kykm_id", whereParams)
            MessageBox.Show("保存しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_HAIF_ADD_Click(sender As Object, e As EventArgs) Handles cmd_HAIF_ADD.Click
        If _kykmId = 0 Then Return
        Try
            ' 親物件の情報を取得
            Dim parentDt = _crud.GetDataTable(
                "SELECT kykh_id, kykh_no, saikaisu, kykm_no, " &
                "       COALESCE(b_klsryo,0) AS b_klsryo, COALESCE(b_kzei,0) AS b_kzei, " &
                "       COALESCE(b_mlsryo,0) AS b_mlsryo, COALESCE(b_mzei,0) AS b_mzei " &
                "FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            If parentDt Is Nothing OrElse parentDt.Rows.Count = 0 Then Return
            Dim pr = parentDt.Rows(0)

            ' 次のline_idを採番
            Dim maxLineObj = _crud.ExecuteScalar(Of Object)(
                "SELECT COALESCE(MAX(line_id), 0) FROM d_haif WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            Dim newLineId As Integer = Convert.ToInt32(maxLineObj) + 1

            ' 残配賦率を算出
            Dim sumObj = _crud.ExecuteScalar(Of Object)(
                "SELECT COALESCE(SUM(haifritu), 0) FROM d_haif WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            Dim newHaifritu As Double = Math.Max(100.0 - Convert.ToDouble(sumObj), 0.0)

            ' 金額 = 親物件金額 × 配賦率 / 100
            Dim bKlsryo = Convert.ToDouble(pr("b_klsryo"))
            Dim bKzei = Convert.ToDouble(pr("b_kzei"))
            Dim bMlsryo = Convert.ToDouble(pr("b_mlsryo"))
            Dim bMzei = Convert.ToDouble(pr("b_mzei"))
            Dim hKlsryo = Math.Floor(bKlsryo * newHaifritu / 100.0)
            Dim hKzei = Math.Floor(bKzei * newHaifritu / 100.0)
            Dim hMlsryo = Math.Floor(bMlsryo * newHaifritu / 100.0)
            Dim hMzei = Math.Floor(bMzei * newHaifritu / 100.0)

            Dim sql As String =
                "INSERT INTO d_haif (kykm_id, line_id, kykh_id, kykh_no, saikaisu, kykm_no, " &
                "  haifritu, h_klsryo, h_kzei, h_klsryo_zkomi, " &
                "  h_mlsryo, h_mzei, h_mlsryo_zkomi, " &
                "  h_create_id, h_create_dt, h_update_id, h_update_dt) " &
                "VALUES (@kykm_id, @line_id, @kykh_id, @kykh_no, @saikaisu, @kykm_no, " &
                "  @haifritu, @h_klsryo, @h_kzei, @h_klsryo_zkomi, " &
                "  @h_mlsryo, @h_mzei, @h_mlsryo_zkomi, " &
                "  0, NOW(), 0, NOW())"
            Dim prm As New List(Of NpgsqlParameter)
            prm.Add(New NpgsqlParameter("@kykm_id", _kykmId))
            prm.Add(New NpgsqlParameter("@line_id", CShort(newLineId)))
            prm.Add(New NpgsqlParameter("@kykh_id", If(IsDBNull(pr("kykh_id")), CObj(DBNull.Value), pr("kykh_id"))))
            prm.Add(New NpgsqlParameter("@kykh_no", If(IsDBNull(pr("kykh_no")), CObj(DBNull.Value), pr("kykh_no"))))
            prm.Add(New NpgsqlParameter("@saikaisu", If(IsDBNull(pr("saikaisu")), CObj(DBNull.Value), pr("saikaisu"))))
            prm.Add(New NpgsqlParameter("@kykm_no", If(IsDBNull(pr("kykm_no")), CObj(DBNull.Value), pr("kykm_no"))))
            prm.Add(New NpgsqlParameter("@haifritu", newHaifritu))
            prm.Add(New NpgsqlParameter("@h_klsryo", hKlsryo))
            prm.Add(New NpgsqlParameter("@h_kzei", hKzei))
            prm.Add(New NpgsqlParameter("@h_klsryo_zkomi", hKlsryo + hKzei))
            prm.Add(New NpgsqlParameter("@h_mlsryo", hMlsryo))
            prm.Add(New NpgsqlParameter("@h_mzei", hMzei))
            prm.Add(New NpgsqlParameter("@h_mlsryo_zkomi", hMlsryo + hMzei))
            _crud.ExecuteNonQuery(sql, prm)

            MessageBox.Show("配賦行を追加しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadData()  ' データ再読み込み
        Catch ex As Exception
            MessageBox.Show("配賦行追加エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_HAIF_DEL_Click(sender As Object, e As EventArgs) Handles cmd_HAIF_DEL.Click
        If _kykmId = 0 Then Return
        Dim frm As New Form_f_KYKM_SUB_BKN()
        frm.SetParams(_kykmId)
        frm.ShowDialog(Me)
        Dim lineId = frm.SelectedLineId
        If lineId <= 0 Then Return
        If MessageBox.Show($"配賦行 (LINE_ID={lineId}) を削除しますか？",
                "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return
        Try
            Dim delPrms As New List(Of NpgsqlParameter)
            delPrms.Add(New NpgsqlParameter("@kykm_id", _kykmId))
            delPrms.Add(New NpgsqlParameter("@line_id", lineId))
            _crud.ExecuteNonQuery(
                "DELETE FROM d_haif WHERE kykm_id = @kykm_id AND line_id = @line_id",
                delPrms)
            MessageBox.Show("配賦行を削除しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_f_KYKM_BKN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
