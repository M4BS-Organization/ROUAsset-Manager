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
        MessageBox.Show("現金購入価額照会は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Function NzDbl(v As Object) As Double
        If IsDBNull(v) OrElse v Is Nothing Then Return 0.0
        Try : Return Convert.ToDouble(v)
        Catch : Return 0.0
        End Try
    End Function

    Private Sub Form_f_KYKH_SUB_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
