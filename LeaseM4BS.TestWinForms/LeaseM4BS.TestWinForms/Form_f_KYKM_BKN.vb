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
        txt_START_DT.Text = NzDateStr(r("start_dt"))
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
        txt_B_KJYO_EN_DT.Text = NzDateStr(r("b_kjyo_en_dt"))

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
        txt_HENL_SEDT.Text = NzDateStr(r("henl_sedt"))
        txt_HENF_SEDT.Text = NzDateStr(r("henf_sedt"))
        txt_CKAIYK_DT.Text = NzDateStr(r("ckaiyk_dt"))
        txt_CKAIYK_ESDT_T.Text = NzDateStr(r("ckaiyk_esdt_t"))
        txt_CKAIYK_ESDT_H.Text = NzDateStr(r("ckaiyk_esdt_h"))
        txt_IYAKU_KIN.Text = NzAmtStr(r("iyaku_kin"))

        ' 管理部署（bcat）
        txt_BCAT1_NM.Text = NzStr(r("bcat1_nm"))
        txt_BCAT2_NM.Text = NzStr(r("bcat2_nm"))
        txt_BCAT3_NM.Text = NzStr(r("bcat3_nm"))
        txt_BCAT4_NM.Text = NzStr(r("bcat4_nm"))
        txt_BCAT5_NM.Text = NzStr(r("bcat5_nm"))
        txt_IDO_DT.Text = NzDateStr(r("ido_dt"))
        txt_B_BCAT_NM_K.Text = NzStr(r("b_bcat_nm_k"))

        ' 移動先R1
        txt_BCAT1_NM_R1.Text = NzStr(r("bcat1_nm_r1"))
        txt_BCAT2_NM_R1.Text = NzStr(r("bcat2_nm_r1"))
        txt_BCAT3_NM_R1.Text = NzStr(r("bcat3_nm_r1"))
        txt_BCAT4_NM_R1.Text = NzStr(r("bcat4_nm_r1"))
        txt_BCAT5_NM_R1.Text = NzStr(r("bcat5_nm_r1"))
        txt_IDO_DT_R1.Text = NzDateStr(r("ido_dt_r1"))

        ' 移動先R2
        txt_BCAT1_NM_R2.Text = NzStr(r("bcat1_nm_r2"))
        txt_BCAT2_NM_R2.Text = NzStr(r("bcat2_nm_r2"))
        txt_BCAT3_NM_R2.Text = NzStr(r("bcat3_nm_r2"))
        txt_BCAT4_NM_R2.Text = NzStr(r("bcat4_nm_r2"))
        txt_BCAT5_NM_R2.Text = NzStr(r("bcat5_nm_r2"))
        txt_IDO_DT_R2.Text = NzDateStr(r("ido_dt_r2"))

        ' 移動先R3
        txt_BCAT1_NM_R3.Text = NzStr(r("bcat1_nm_r3"))
        txt_BCAT2_NM_R3.Text = NzStr(r("bcat2_nm_r3"))
        txt_BCAT3_NM_R3.Text = NzStr(r("bcat3_nm_r3"))
        txt_BCAT4_NM_R3.Text = NzStr(r("bcat4_nm_r3"))
        txt_BCAT5_NM_R3.Text = NzStr(r("bcat5_nm_r3"))
        txt_IDO_DT_R3.Text = NzDateStr(r("ido_dt_r3"))

        ' 名称参照
        txt_SKMK_NM.Text = NzStr(r("skmk_nm"))
        txt_BKIND_NM.Text = NzStr(r("bkind_nm"))
        txt_K_GSHA_NM.Text = NzStr(r("k_gsha_nm"))
        txt_MCPT_NM.Text = NzStr(r("mcpt_nm"))
        txt_RSRV_NM.Text = NzStr(r("rsrvb1_nm"))

        ' 廃棄
        txt_HKHO_NM.Text = NzStr(r("hkho_nm"))
        txt_HK_GSHA_NM.Text = NzStr(r("hk_gsha_nm"))
        txt_HK_DT.Text = NzDateStr(r("hk_dt"))

        ' 物件番号
        txt_BUKN_BANGO1.Text = NzStr(r("bukn_bango1"))
        txt_BUKN_BANGO2.Text = NzStr(r("bukn_bango2"))
        txt_BUKN_BANGO3.Text = NzStr(r("bukn_bango3"))
        txt_SETTI_DT.Text = NzDateStr(r("setti_dt"))

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
        txt_B_CREATE_DT.Text = NzDateStr(r("b_create_dt"))
        txt_B_UPDATE_DT.Text = NzDateStr(r("b_update_dt"))
        txt_DMY.Text = NzStr(r("dmy"))
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    Private Sub cmd_TOUROKU_Click(sender As Object, e As EventArgs) Handles cmd_TOUROKU.Click
        MessageBox.Show("分割登録機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_HAIF_ADD_Click(sender As Object, e As EventArgs) Handles cmd_HAIF_ADD.Click
        MessageBox.Show("配賦追加機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Function NzDateStr(v As Object) As String
        If IsDBNull(v) OrElse v Is Nothing Then Return ""
        Try : Return Convert.ToDateTime(v).ToString("yyyy/MM/dd")
        Catch : Return v.ToString()
        End Try
    End Function

    Private Sub Form_f_KYKM_BKN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
