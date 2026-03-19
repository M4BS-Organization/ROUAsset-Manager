Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 物件主画面 (f_KYKM)
''' Access版 f_KYKM / pc_f_KYKM 相当。
''' d_kykm の物件明細 CRUD + レコードナビゲーション。
''' </summary>
Partial Public Class Form_f_KYKM
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0
    Private _records As New List(Of System.Data.DataRow)
    Private _currentIndex As Integer = -1
    Private _isDirty As Boolean = False
    Private _isNewRecord As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部から契約ID をセット</summary>
    Public Sub SetParams(kykhId As Double)
        _kykhId = kykhId
    End Sub

    ' =========================================================
    '  フォームロード / クローズ
    ' =========================================================
    Private Sub Form_f_KYKM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAllRecords()
        If _records.Count > 0 Then
            ShowRecord(0)
        Else
            ClearFields()
            UpdateNavButtons()
        End If
    End Sub

    Private Sub Form_f_KYKM_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If _isDirty Then
            Dim res = MessageBox.Show("変更を保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If res = DialogResult.Cancel Then
                e.Cancel = True
                Return
            ElseIf res = DialogResult.Yes Then
                If Not SaveCurrentRecord() Then
                    e.Cancel = True
                    Return
                End If
            End If
        End If
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Private Sub LoadAllRecords()
        Try
            Dim sql As String =
                "SELECT m.*, " &
                "       sk.skmk_nm, bk.bkind_nm, gs.gsha_nm AS k_gsha_nm, " &
                "       mc.mcpt_nm, rv.rsrvb1_nm, " &
                "       bc.bcat_nm AS bcat1_nm " &
                "FROM d_kykm m " &
                "LEFT JOIN m_skmk sk ON sk.skmk_id = m.skmk_id " &
                "LEFT JOIN m_bkind bk ON bk.bkind_id = m.bkind_id " &
                "LEFT JOIN m_gsha gs ON gs.gsha_id = m.k_gsha_id " &
                "LEFT JOIN m_mcpt mc ON mc.mcpt_id = m.mcpt_id " &
                "LEFT JOIN m_rsrvb1 rv ON rv.rsrvb1_id = m.rsrvb1_id " &
                "LEFT JOIN m_bcat bc ON bc.bcat_id = m.b_bcat_id " &
                "WHERE m.kykh_id = @id ORDER BY m.kykm_no"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)}
            Dim dt = _crud.GetDataTable(sql, prm)
            _records.Clear()
            If dt IsNot Nothing Then
                For Each row As System.Data.DataRow In dt.Rows
                    _records.Add(row)
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  レコード表示
    ' =========================================================
    Private Sub ShowRecord(index As Integer)
        If index < 0 OrElse index >= _records.Count Then Return
        _currentIndex = index
        _isNewRecord = False
        _isDirty = False

        Dim r = _records(index)
        txt_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_KYKM_NO.Text = NzStr(r("kykm_no"))
        txt_KYKM_NO_MAE.Text = NzStr(r("kykm_no_mae"))
        txt_BUKN_NM.Text = NzStr(r("bukn_nm"))
        txt_B_KEDABAN.Text = NzStr(r("b_kedaban"))
        txt_SUURYO.Text = NzStr(r("b_suuryo"))
        txt_KNYUKN.Text = NzAmtStr(r("b_knyukn"))
        txt_SLSRYO.Text = NzAmtStr(r("b_slsryo"))
        txt_GLSRYO.Text = NzAmtStr(r("b_glsryo"))
        txt_KLSRYO.Text = NzAmtStr(r("b_klsryo"))
        txt_MLSRYO.Text = NzAmtStr(r("b_mlsryo"))
        txt_IJIKNR.Text = NzAmtStr(r("b_ijiknr"))
        txt_ZANRYO.Text = NzAmtStr(r("b_zanryo"))
        txt_GZEI.Text = NzAmtStr(r("b_gzei"))
        txt_KZEI.Text = NzAmtStr(r("b_kzei"))
        txt_MZEI.Text = NzAmtStr(r("b_mzei"))
        txt_GLSRYO_ZKOMI.Text = NzAmtStr(r("b_glsryo_zkomi"))
        txt_KLSRYO_ZKOMI.Text = NzAmtStr(r("b_klsryo_zkomi"))
        txt_MLSRYO_ZKOMI.Text = NzAmtStr(r("b_mlsryo_zkomi"))
        txt_HENL_SUM.Text = NzAmtStr(r("b_henl_sum"))
        txt_HENL_SEDT.Text = NzDtStr(r("b_henl_sedt"))
        txt_HENF_SEDT.Text = NzDtStr(r("b_henf_sedt"))
        txt_B_HENF_KLSRYO_NEW.Text = NzAmtStr(r("b_henf_klsryo_new"))
        txt_B_HENL_F.Text = If(NzBool(r("b_henl_f")), "1", "0")
        txt_B_HENF_F.Text = If(NzBool(r("b_henf_f")), "1", "0")
        ' 解約
        chk_CKAIYK_F.Checked = NzBool(r("b_ckaiyk_f"))
        txt_CKAIYK_DT.Text = NzDtStr(r("ckaiyk_dt"))
        txt_CKAIYK_ESDT_T.Text = NzDtStr(r("ckaiyk_esdt_t"))
        txt_CKAIYK_ESDT_H.Text = NzDtStr(r("ckaiyk_esdt_h"))
        txt_IYAKU_KIN.Text = NzAmtStr(r("iyaku_kin"))
        ' 名称参照
        txt_SKMK_NM.Text = NzStr(r("skmk_nm"))
        txt_BKIND_NM.Text = NzStr(r("bkind_nm"))
        txt_K_GSHA_NM.Text = NzStr(r("k_gsha_nm"))
        txt_MCPT_NM.Text = NzStr(r("mcpt_nm"))
        txt_RSRV_NM.Text = NzStr(r("rsrvb1_nm"))
        txt_BCAT1_NM.Text = NzStr(r("bcat1_nm"))
        txt_BUKN_BANGO1.Text = NzStr(r("bukn_bango1"))
        txt_BUKN_BANGO2.Text = NzStr(r("bukn_bango2"))
        txt_BUKN_BANGO3.Text = NzStr(r("bukn_bango3"))
        txt_SETTI_DT.Text = NzDtStr(r("b_rend_dt"))
        txt_IDO_DT.Text = NzDtStr(r("ido_dt"))
        txt_B_ZOKUSEI1.Text = NzStr(r("b_zokusei1"))
        txt_KJKBN.Text = NzStr(r("kjkbn_id"))
        txt_B_CREATE_DT.Text = NzDtStr(r("b_create_dt"))
        txt_B_UPDATE_DT.Text = NzDtStr(r("b_update_dt"))
        ' フラグ
        chk_SUURYO_SUM_F.Checked = NzBool(r("suuryo_sum_f"))
        UpdateNavButtons()
        UpdateRecordCount()
    End Sub

    Private Sub ClearFields()
        Dim textboxes() As String = {
            "txt_KYKM_ID", "txt_W_KYKM_ID", "txt_KYKM_NO", "txt_KYKM_NO_MAE",
            "txt_BUKN_NM", "txt_B_KEDABAN", "txt_SUURYO",
            "txt_KNYUKN", "txt_SLSRYO", "txt_GLSRYO", "txt_KLSRYO", "txt_MLSRYO",
            "txt_IJIKNR", "txt_ZANRYO", "txt_GZEI", "txt_KZEI", "txt_MZEI",
            "txt_GLSRYO_ZKOMI", "txt_KLSRYO_ZKOMI", "txt_MLSRYO_ZKOMI",
            "txt_HENL_SUM", "txt_HENL_SEDT", "txt_HENF_SEDT", "txt_B_HENF_KLSRYO_NEW",
            "txt_B_HENL_F", "txt_B_HENF_F", "txt_CKAIYK_DT",
            "txt_CKAIYK_ESDT_T", "txt_CKAIYK_ESDT_H", "txt_IYAKU_KIN",
            "txt_SKMK_NM", "txt_BKIND_NM", "txt_K_GSHA_NM", "txt_MCPT_NM",
            "txt_RSRV_NM", "txt_BCAT1_NM", "txt_BUKN_BANGO1", "txt_BUKN_BANGO2",
            "txt_BUKN_BANGO3", "txt_SETTI_DT", "txt_IDO_DT", "txt_B_ZOKUSEI1",
            "txt_KJKBN", "txt_B_CREATE_DT", "txt_B_UPDATE_DT", "txt_INF"
        }
        For Each nm In textboxes
            Dim ctl = Me.Controls.Find(nm, True)
            If ctl.Length > 0 Then CType(ctl(0), TextBox).Text = ""
        Next
        chk_CKAIYK_F.Checked = False
        chk_SUURYO_SUM_F.Checked = False
    End Sub

    Private Sub UpdateNavButtons()
        cmd_REC_FIRST.Enabled = (_currentIndex > 0)
        cmd_REC_PREVIOUS.Enabled = (_currentIndex > 0)
        cmd_REC_NEXT.Enabled = (_currentIndex < _records.Count - 1)
        cmd_REC_LAST.Enabled = (_currentIndex < _records.Count - 1)
    End Sub

    Private Sub UpdateRecordCount()
        txt_INF.Text = $"{_currentIndex + 1} / {_records.Count}"
    End Sub

    ' =========================================================
    '  レコード保存
    ' =========================================================
    Private Function SaveCurrentRecord() As Boolean
        If _kykhId = 0 Then Return True
        Try
            Dim val As New Dictionary(Of String, Object)
            val.Add("bukn_nm", txt_BUKN_NM.Text.Trim())
            val.Add("b_kedaban", txt_B_KEDABAN.Text.Trim())
            val.Add("b_suuryo", NzInt(txt_SUURYO.Text))
            val.Add("b_knyukn", NzDbl(txt_KNYUKN.Text))
            val.Add("b_glsryo", NzDbl(txt_GLSRYO.Text))
            val.Add("b_klsryo", NzDbl(txt_KLSRYO.Text))
            val.Add("b_mlsryo", NzDbl(txt_MLSRYO.Text))
            val.Add("b_slsryo", NzDbl(txt_SLSRYO.Text))
            val.Add("b_ijiknr", NzDbl(txt_IJIKNR.Text))
            val.Add("b_zanryo", NzDbl(txt_ZANRYO.Text))
            val.Add("b_gzei", NzDbl(txt_GZEI.Text))
            val.Add("b_kzei", NzDbl(txt_KZEI.Text))
            val.Add("b_mzei", NzDbl(txt_MZEI.Text))
            val.Add("b_glsryo_zkomi", NzDbl(txt_GLSRYO_ZKOMI.Text))
            val.Add("b_klsryo_zkomi", NzDbl(txt_KLSRYO_ZKOMI.Text))
            val.Add("b_mlsryo_zkomi", NzDbl(txt_MLSRYO_ZKOMI.Text))
            val.Add("b_henl_sum", NzDbl(txt_HENL_SUM.Text))
            val.Add("b_henl_sedt", ParseDt(txt_HENL_SEDT.Text))
            val.Add("b_henf_sedt", ParseDt(txt_HENF_SEDT.Text))
            val.Add("b_henf_klsryo_new", NzDbl(txt_B_HENF_KLSRYO_NEW.Text))
            val.Add("b_henl_f", (txt_B_HENL_F.Text = "1"))
            val.Add("b_henf_f", (txt_B_HENF_F.Text = "1"))
            val.Add("b_ckaiyk_f", chk_CKAIYK_F.Checked)
            val.Add("ckaiyk_dt", ParseDt(txt_CKAIYK_DT.Text))
            val.Add("ckaiyk_esdt_t", ParseDt(txt_CKAIYK_ESDT_T.Text))
            val.Add("ckaiyk_esdt_h", ParseDt(txt_CKAIYK_ESDT_H.Text))
            val.Add("iyaku_kin", NzDbl(txt_IYAKU_KIN.Text))
            val.Add("bukn_bango1", txt_BUKN_BANGO1.Text.Trim())
            val.Add("bukn_bango2", txt_BUKN_BANGO2.Text.Trim())
            val.Add("bukn_bango3", txt_BUKN_BANGO3.Text.Trim())
            val.Add("b_zokusei1", txt_B_ZOKUSEI1.Text.Trim())
            val.Add("suuryo_sum_f", chk_SUURYO_SUM_F.Checked)
            val.Add("b_update_dt", DateTime.Now)

            If _isNewRecord Then
                ' 新規: MAX(kykm_id)+1 で採番
                Dim maxIdObj = _crud.ExecuteScalar(Of Object)(
                    "SELECT COALESCE(MAX(kykm_id), 0) FROM d_kykm")
                Dim newId As Integer = Convert.ToInt32(maxIdObj) + 1
                Dim maxNoObj = _crud.ExecuteScalar(Of Object)(
                    "SELECT COALESCE(MAX(kykm_no), 0) FROM d_kykm WHERE kykh_id = @id",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
                Dim newNo As Integer = Convert.ToInt32(maxNoObj) + 1

                val.Add("kykm_id", newId)
                val.Add("kykh_id", _kykhId)
                val.Add("kykm_no", newNo)
                val.Add("saikaisu", 0)
                val.Add("b_create_id", 0)
                val.Add("b_create_dt", DateTime.Now)
                val.Add("b_update_id", 0)
                ' 必須フラグ群
                For Each flagCol In {"kari_ritu_ms_f", "taiyo_nen_ms_f", "leakbn_id_ms_f",
                                     "chuum_id_ms_f", "lb_chuki_f", "genson_f",
                                     "b_seigou_f", "b_gson_f", "rsok_tmg_ms_f",
                                     "gk_calc_kind_ms_f", "hensai_kind_ms_f",
                                     "ij_kjyo_kind_ms_f", "gson_tk_kind_ms_f",
                                     "lb_kjyo_kind_ms_f", "kjkbn_ms_f",
                                     "szei_kjkbn_id_ms_f", "hszei_kjkbn_id_ms_f"}
                    val.Add(flagCol, False)
                Next
                For Each intCol In {"leakbn_id", "chuum_id", "chu_hnti_id", "hkho_id",
                                    "hk_gsha_id", "f_lcpt_id", "f_hkmk_id", "f_gsha_id",
                                    "skmk_id", "b_bcat_id", "b_bcat_id_r1", "b_bcat_id_r2",
                                    "b_bcat_id_r3", "k_gsha_id", "bkind_id", "mcpt_id",
                                    "rsrvb1_id", "rsok_tmg", "gk_calc_kind", "hensai_kind",
                                    "ij_kjyo_kind", "gson_tk_kind", "lb_kjyo_kind",
                                    "kjkbn_id", "skyak_ho_id", "kj_flg"}
                    val.Add(intCol, 0)
                Next
                _crud.Insert("d_kykm", val)
                _isNewRecord = False
            Else
                ' 更新
                Dim kykmId As Integer = Convert.ToInt32(txt_KYKM_ID.Text)
                Dim whereP As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@kykm_id", kykmId)
                }
                _crud.Update("d_kykm", val, "kykm_id = @kykm_id", whereP)
            End If

            _isDirty = False
            LoadAllRecords()
            Return True
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' =========================================================
    '  ナビゲーションボタン
    ' =========================================================
    Private Function NavigateWithSaveCheck(newIndex As Integer) As Boolean
        If _isDirty Then
            Dim res = MessageBox.Show("変更を保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If res = DialogResult.Cancel Then Return False
            If res = DialogResult.Yes Then
                If Not SaveCurrentRecord() Then Return False
            Else
                _isDirty = False
            End If
        End If
        ShowRecord(newIndex)
        Return True
    End Function

    Private Sub cmd_REC_FIRST_Click(sender As Object, e As EventArgs) Handles cmd_REC_FIRST.Click
        NavigateWithSaveCheck(0)
    End Sub

    Private Sub cmd_REC_PREVIOUS_Click(sender As Object, e As EventArgs) Handles cmd_REC_PREVIOUS.Click
        If _currentIndex > 0 Then NavigateWithSaveCheck(_currentIndex - 1)
    End Sub

    Private Sub cmd_REC_NEXT_Click(sender As Object, e As EventArgs) Handles cmd_REC_NEXT.Click
        If _currentIndex < _records.Count - 1 Then NavigateWithSaveCheck(_currentIndex + 1)
    End Sub

    Private Sub cmd_REC_LAST_Click(sender As Object, e As EventArgs) Handles cmd_REC_LAST.Click
        If _records.Count > 0 Then NavigateWithSaveCheck(_records.Count - 1)
    End Sub

    Private Sub cmd_REC_NEW_Click(sender As Object, e As EventArgs) Handles cmd_REC_NEW.Click
        If _isDirty Then
            Dim res = MessageBox.Show("変更を保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If res = DialogResult.Cancel Then Return
            If res = DialogResult.Yes Then
                If Not SaveCurrentRecord() Then Return
            End If
        End If
        ClearFields()
        _currentIndex = _records.Count  ' 仮インデックス
        _isNewRecord = True
        _isDirty = False
        txt_INF.Text = $"新規 / {_records.Count}"
        UpdateNavButtons()
    End Sub

    ' =========================================================
    '  CRUD ボタン
    ' =========================================================
    Private Sub cmd_削除_Click(sender As Object, e As EventArgs) Handles cmd_削除.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        If MessageBox.Show("この物件を削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return

        Try
            Dim kykmId As Integer = Convert.ToInt32(txt_KYKM_ID.Text)
            _crud.ExecuteNonQuery(
                "DELETE FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", kykmId)})
            _isDirty = False
            LoadAllRecords()
            If _records.Count > 0 Then
                ShowRecord(Math.Min(_currentIndex, _records.Count - 1))
            Else
                ClearFields()
                _currentIndex = -1
                UpdateNavButtons()
            End If
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_同一複写_Click(sender As Object, e As EventArgs) Handles cmd_同一複写.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        ' 現在レコードを保存してから複写
        If _isDirty Then
            If Not SaveCurrentRecord() Then Return
        End If
        Try
            ' 現在レコードの内容を複写して INSERT
            Dim maxIdObj = _crud.ExecuteScalar(Of Object)(
                "SELECT COALESCE(MAX(kykm_id), 0) FROM d_kykm")
            Dim newId As Integer = Convert.ToInt32(maxIdObj) + 1
            Dim maxNoObj = _crud.ExecuteScalar(Of Object)(
                "SELECT COALESCE(MAX(kykm_no), 0) FROM d_kykm WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            Dim newNo As Integer = Convert.ToInt32(maxNoObj) + 1

            Dim kykmId As Integer = Convert.ToInt32(txt_KYKM_ID.Text)
            _crud.ExecuteNonQuery(
                "INSERT INTO d_kykm SELECT " & newId & ", kykh_id, kykh_no, " & newNo &
                ", kykm_no, saikaisu, b_kedaban, bukn_bango1, bukn_bango2, bukn_bango3, " &
                "0, now(), 0, now(), b_suuryo, suuryo_sum_f, b_knyukn, b_glsryo, " &
                "b_klsryo, b_mlsryo, b_slsryo, b_gzei, b_kzei, b_mzei, " &
                "b_glsryo_zkomi, b_klsryo_zkomi, b_mlsryo_zkomi, b_ijiknr, b_zanryo, " &
                "b_ghassei, b_gnzai_kt, b_syutok, kari_ritu, kari_ritu_ms_f, ksan_ritu, " &
                "taiyo_nen, taiyo_nen_ms_f, rslt90p, rslt90p_str, rslt75p, rslt75p_str, " &
                "leakbn_id, leakbn_id_ms_f, chuum_id, chuum_id_ms_f, chu_hnti_id, " &
                "b_lb_soneki, lb_chuki_f, hkho_id, hk_dt, hk_gsha_id, " &
                "FALSE, NULL, NULL, NULL, 0, FALSE, NULL, NULL, FALSE, NULL, 0, " &
                "f_lcpt_id, f_hkmk_id, f_gsha_id, kykbnf, genson_f, NULL, FALSE, " &
                "skmk_id, b_bcat_id, NULL, b_bcat_id_r1, NULL, b_bcat_id_r2, NULL, " &
                "b_bcat_id_r3, NULL, k_gsha_id, bkind_id, mcpt_id, rsrvb1_id, " &
                "rsok_tmg, rsok_tmg_ms_f, gk_calc_kind, gk_calc_kind_ms_f, hensai_kind, " &
                "hensai_kind_ms_f, ij_kjyo_kind, ij_kjyo_kind_ms_f, gson_tk_kind, " &
                "gson_tk_kind_ms_f, lb_kjyo_kind, lb_kjyo_kind_ms_f, kjkbn_id, " &
                "kjkbn_ms_f, szei_kjkbn_id_ms_f, hszei_kjkbn_id_ms_f, skyak_ho_id, " &
                "kj_flg, b_zokusei1, b_zokusei2, b_zokusei3, b_zokusei4, b_zokusei5, " &
                "b_rend_dt, b_seigou_f, b_gson_f, bukn_nm, ido_dt, ido_dt_r1, " &
                "ido_dt_r2, ido_dt_r3, b_tanka, b_sairyo, b_kjyo_en_dt " &
                "FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", kykmId)})

            LoadAllRecords()
            ShowRecord(_records.Count - 1)
        Catch ex As Exception
            MessageBox.Show("複写エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_残額複写_Click(sender As Object, e As EventArgs) Handles cmd_残額複写.Click
        MessageBox.Show("残額複写機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_HENL_Click(sender As Object, e As EventArgs) Handles cmd_HENL.Click
        MessageBox.Show("返却(HENL)機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_HENF_Click(sender As Object, e As EventArgs) Handles cmd_HENF.Click
        MessageBox.Show("変更(HENF)機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_CHUUKI_Click(sender As Object, e As EventArgs) Handles cmd_CHUUKI.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        Dim kykmId As Integer = 0
        Integer.TryParse(txt_KYKM_ID.Text, kykmId)
        If kykmId = 0 Then Return
        Dim frm As New Form_f_KYKM_CHUUKI()
        frm.SetParams(kykmId)
        frm.ShowDialog(Me)
    End Sub

    Private Sub cmd_分割_Click(sender As Object, e As EventArgs) Handles cmd_分割.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        Dim kykmId As Integer = 0
        Integer.TryParse(txt_KYKM_ID.Text, kykmId)
        If kykmId = 0 Then Return
        Dim frm As New Form_f_KYKM_BKN()
        frm.SetParams(kykmId)
        frm.ShowDialog(Me)
    End Sub

    Private Sub cmd_HAIF_ADD_Click(sender As Object, e As EventArgs) Handles cmd_HAIF_ADD.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        Dim frm As New Form_f_KYKM_SUB()
        frm.SetParams(Convert.ToDouble(txt_KYKM_ID.Text))
        frm.ShowDialog(Me)
    End Sub

    Private Sub cmd_HAIF_DEL_Click(sender As Object, e As EventArgs) Handles cmd_HAIF_DEL.Click
        If _currentIndex < 0 OrElse _currentIndex >= _records.Count Then Return
        Dim kykmId As Integer = 0
        Integer.TryParse(txt_KYKM_ID.Text, kykmId)
        If kykmId = 0 Then Return

        ' f_KYKM_SUB を開いて削除対象行を選択させる
        Dim frm As New Form_f_KYKM_SUB()
        frm.SetParams(kykmId)
        frm.ShowDialog(Me)

        Dim lineId = frm.SelectedLineId
        If lineId <= 0 Then Return

        If MessageBox.Show(
            $"配賦行 (LINE_ID={lineId}) を削除しますか？",
            "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Return

        Try
            Dim delPrms As New List(Of NpgsqlParameter)
            delPrms.Add(New NpgsqlParameter("@kykm_id", kykmId))
            delPrms.Add(New NpgsqlParameter("@line_id", lineId))
            _crud.ExecuteNonQuery(
                "DELETE FROM d_haif WHERE kykm_id = @kykm_id AND line_id = @line_id",
                delPrms)
            MessageBox.Show("配賦行を削除しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        If _isDirty Then
            Dim res = MessageBox.Show("変更を保存して閉じますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            If res = DialogResult.Cancel Then Return
            If res = DialogResult.Yes AndAlso Not SaveCurrentRecord() Then Return
        End If
        Me.Close()
    End Sub

    ' =========================================================
    '  変更フラグ管理（主要テキストボックスのTextChanged）
    ' =========================================================
    Private Sub SetDirty(sender As Object, e As EventArgs) _
        Handles txt_BUKN_NM.TextChanged, txt_SUURYO.TextChanged, txt_KNYUKN.TextChanged,
                txt_GLSRYO.TextChanged, txt_KLSRYO.TextChanged, txt_MLSRYO.TextChanged,
                txt_SLSRYO.TextChanged, txt_IJIKNR.TextChanged, txt_ZANRYO.TextChanged
        _isDirty = True
    End Sub

    ' =========================================================
    '  ヘルパー
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

    Private Sub Form_f_KYKM_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
