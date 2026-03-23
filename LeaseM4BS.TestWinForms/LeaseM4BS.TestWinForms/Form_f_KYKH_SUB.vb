Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 契約書サブ画面 (f_KYKH_SUB)
''' Access版 sub_f_KYKH_SUB サブフォーム相当。
''' d_kykm の物件一覧を表示し、合計行を算出する。
''' ShowAll=False: 解約済み除外 (b_ckaiyk_f=FALSE)
''' ShowAll=True : 全件表示
''' </summary>
Partial Public Class Form_f_KYKH_SUB
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0
    Private _showAll As Boolean = False
    Private _rows As New List(Of System.Data.DataRow)
    Private _currentIndex As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部からパラメータをセット</summary>
    Public Sub SetParams(kykhId As Double, Optional showAll As Boolean = False)
        _kykhId = kykhId
        _showAll = showAll
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KYKH_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim whereClause As String
            If _showAll Then
                whereClause = "WHERE m.kykh_id = @id"
            Else
                whereClause = "WHERE m.kykh_id = @id AND m.b_ckaiyk_f = FALSE"
            End If

            Dim sql As String =
                "SELECT m.*, b.bcat_nm " &
                "FROM d_kykm m " &
                "LEFT JOIN m_bcat b ON b.bcat_id = m.b_bcat_id " &
                whereClause & " ORDER BY m.kykm_no"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)}
            Dim dt = _crud.GetDataTable(sql, prm)

            _rows.Clear()
            If dt IsNot Nothing Then
                For Each row As System.Data.DataRow In dt.Rows
                    _rows.Add(row)
                Next
            End If

            _currentIndex = 0
            ShowCurrentRecord()
            ShowTotals()
        Catch ex As Exception
            MessageBox.Show("データ読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>現在レコードを画面に反映</summary>
    Private Sub ShowCurrentRecord()
        If _rows.Count = 0 Then
            ClearRecordFields()
            txt_COUNT.Text = "0"
            Return
        End If

        Dim r = _rows(_currentIndex)
        txt_KYKM_NO.Text = NzStr(r("kykm_no"))
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_BUKN_NM.Text = NzStr(r("bukn_nm"))
        txt_BCAT_NM.Text = NzStr(r("bcat_nm"))
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
        txt_KJKBN.Text = NzStr(r("kjkbn_id"))
        chk_CKAIYK_F.Checked = NzBool(r("b_ckaiyk_f"))
        txt_COUNT.Text = _rows.Count.ToString()
    End Sub

    Private Sub ClearRecordFields()
        Dim fields() As TextBox = {
            txt_KYKM_NO, txt_W_KYKM_ID, txt_KYKM_ID, txt_BUKN_NM, txt_BCAT_NM,
            txt_SUURYO, txt_KNYUKN, txt_SLSRYO, txt_GLSRYO, txt_KLSRYO, txt_MLSRYO,
            txt_IJIKNR, txt_ZANRYO, txt_GZEI, txt_KZEI, txt_MZEI,
            txt_GLSRYO_ZKOMI, txt_KLSRYO_ZKOMI, txt_MLSRYO_ZKOMI,
            txt_HENL_SUM, txt_HENL_SEDT, txt_HENF_SEDT, txt_KJKBN
        }
        For Each t In fields
            t.Text = ""
        Next
        chk_CKAIYK_F.Checked = False
    End Sub

    ''' <summary>合計行を集計して表示</summary>
    Private Sub ShowTotals()
        Dim sumKnyukn As Double = 0, sumSuuryo As Double = 0
        Dim sumSlsryo As Double = 0, sumGlsryo As Double = 0
        Dim sumKlsryo As Double = 0, sumMlsryo As Double = 0
        Dim sumIjiknr As Double = 0, sumZanryo As Double = 0
        Dim sumGzei As Double = 0, sumKzei As Double = 0
        Dim sumMzei As Double = 0, sumHenlSum As Double = 0

        For Each r In _rows
            sumKnyukn += NzDbl(r("b_knyukn"))
            sumSuuryo += NzDbl(r("b_suuryo"))
            sumSlsryo += NzDbl(r("b_slsryo"))
            sumGlsryo += NzDbl(r("b_glsryo"))
            sumKlsryo += NzDbl(r("b_klsryo"))
            sumMlsryo += NzDbl(r("b_mlsryo"))
            sumIjiknr += NzDbl(r("b_ijiknr"))
            sumZanryo += NzDbl(r("b_zanryo"))
            sumGzei += NzDbl(r("b_gzei"))
            sumKzei += NzDbl(r("b_kzei"))
            sumMzei += NzDbl(r("b_mzei"))
            sumHenlSum += NzDbl(r("b_henl_sum"))
        Next

        txt_B_KNYUKN_SUM.Text = sumKnyukn.ToString("N0")
        txt_SUURYO_SUM.Text = sumSuuryo.ToString("N0")
        txt_B_SLSRYO_SUM.Text = sumSlsryo.ToString("N0")
        txt_B_GLSRYO_SUM.Text = sumGlsryo.ToString("N0")
        txt_B_KLSRYO_SUM.Text = sumKlsryo.ToString("N0")
        txt_B_MLSRYO_SUM.Text = sumMlsryo.ToString("N0")
        txt_B_IJIKNR_SUM.Text = sumIjiknr.ToString("N0")
        txt_B_ZANRYO_SUM.Text = sumZanryo.ToString("N0")
        txt_B_GZEI_SUM.Text = sumGzei.ToString("N0")
        txt_B_KZEI_SUM.Text = sumKzei.ToString("N0")
        txt_B_MZEI_SUM.Text = sumMzei.ToString("N0")
        txt_HENL_SUM_SUM.Text = sumHenlSum.ToString("N0")
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_現金購入価額_Click(sender As Object, e As EventArgs) Handles cmd_現金購入価額.Click
        If _kykhId = 0 OrElse _rows.Count < 2 Then
            MessageBox.Show("按分対象の物件が2件以上必要です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' 契約書の金額を取得
            Dim khDt = _crud.GetDataTable(
                "SELECT COALESCE(k_klsryo,0) AS k_klsryo, COALESCE(k_glsryo,0) AS k_glsryo, " &
                "       COALESCE(k_mlsryo,0) AS k_mlsryo, COALESCE(k_ijiknr,0) AS k_ijiknr, " &
                "       COALESCE(k_zanryo,0) AS k_zanryo " &
                "FROM d_kykh WHERE kykh_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            If khDt Is Nothing OrElse khDt.Rows.Count = 0 Then Return
            Dim kh = khDt.Rows(0)
            Dim kKlsryo = Convert.ToDouble(kh("k_klsryo"))
            Dim kGlsryo = Convert.ToDouble(kh("k_glsryo"))
            Dim kMlsryo = Convert.ToDouble(kh("k_mlsryo"))
            Dim kIjiknr = Convert.ToDouble(kh("k_ijiknr"))
            Dim kZanryo = Convert.ToDouble(kh("k_zanryo"))

            ' 全物件の現金購入価額合計を取得
            Dim totalKnyukn As Double = 0
            For Each r In _rows
                totalKnyukn += NzDbl(r("b_knyukn"))
            Next

            If totalKnyukn = 0 Then
                MessageBox.Show("現金購入価額の合計が0のため按分できません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If MessageBox.Show("現金購入価額の比率でリース料を按分します。よろしいですか？",
                               "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

            ' 各物件に按分 (端数は最終物件で調整)
            Dim sumK As Double = 0, sumG As Double = 0, sumM As Double = 0
            Dim sumIj As Double = 0, sumZ As Double = 0

            For i = 0 To _rows.Count - 1
                Dim r = _rows(i)
                Dim kykmId = Convert.ToInt32(r("kykm_id"))
                Dim bKnyukn = NzDbl(r("b_knyukn"))
                Dim ratio = bKnyukn / totalKnyukn

                Dim bK, bG, bM, bIj, bZn As Double
                If i = _rows.Count - 1 Then
                    ' 最終行: 端数調整
                    bK = kKlsryo - sumK
                    bG = kGlsryo - sumG
                    bM = kMlsryo - sumM
                    bIj = kIjiknr - sumIj
                    bZn = kZanryo - sumZ
                Else
                    bK = Math.Truncate(kKlsryo * ratio)
                    bG = Math.Truncate(kGlsryo * ratio)
                    bM = Math.Truncate(kMlsryo * ratio)
                    bIj = Math.Truncate(kIjiknr * ratio)
                    bZn = Math.Truncate(kZanryo * ratio)
                End If

                sumK += bK : sumG += bG : sumM += bM : sumIj += bIj : sumZ += bZn

                _crud.ExecuteNonQuery(
                    "UPDATE d_kykm SET b_klsryo=@k, b_glsryo=@g, b_mlsryo=@m, " &
                    "b_ijiknr=@ij, b_zanryo=@z, b_update_dt=NOW() " &
                    "WHERE kykm_id=@id",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@k", bK),
                        New NpgsqlParameter("@g", bG),
                        New NpgsqlParameter("@m", bM),
                        New NpgsqlParameter("@ij", bIj),
                        New NpgsqlParameter("@z", bZn),
                        New NpgsqlParameter("@id", kykmId)
                    })
            Next

            MessageBox.Show("現金購入価額比率で按分しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadData()
        Catch ex As Exception
            MessageBox.Show("按分エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_f_KYKH_SUB_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
