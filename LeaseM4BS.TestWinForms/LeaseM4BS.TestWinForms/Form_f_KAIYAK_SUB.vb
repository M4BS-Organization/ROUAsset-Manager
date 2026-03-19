Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 解約サブフォーム (f_KAIYAK_SUB)
''' d_kykm の物件一覧を解約/継続に分けて表示し合計を算出する。
''' chk_CKAIYK_F_NEW + 解約日/最終支払日/違約金を入力して SaveCancellations() で保存。
''' </summary>
Partial Public Class Form_f_KAIYAK_SUB
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0
    Private _rows As New List(Of System.Data.DataRow)
    Private _currentIndex As Integer = 0
    Private _isDirty As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>外部からパラメータをセット</summary>
    Public Sub SetParams(kykhId As Double)
        _kykhId = kykhId
    End Sub

    ' =========================================================
    '  フォームロード
    ' =========================================================
    Private Sub Form_f_KAIYAK_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String =
                "SELECT m.kykm_id, m.kykm_no, m.bukn_nm, " &
                "       m.b_suuryo, m.b_klsryo, m.b_slsryo, m.b_knyukn, " &
                "       m.b_glsryo, m.b_mlsryo, m.b_ijiknr, m.b_zanryo, m.b_henl_sum, " &
                "       m.b_henl_sedt, m.b_henf_sedt, " &
                "       m.b_ckaiyk_f, m.ckaiyk_dt, m.ckaiyk_esdt_t, m.ckaiyk_esdt_h, " &
                "       m.iyaku_kin, m.b_henl_f, m.b_henf_f, " &
                "       b.bcat1_nm " &
                "FROM d_kykm m " &
                "LEFT JOIN m_bcat b ON b.bcat_id = m.b_bcat_id " &
                "WHERE m.kykh_id = @id ORDER BY m.kykm_no"
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

    ' =========================================================
    '  現在レコード表示
    ' =========================================================
    Private Sub ShowCurrentRecord()
        If _rows.Count = 0 Then
            ClearFields()
            Return
        End If

        _isDirty = False
        Dim r = _rows(_currentIndex)
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_KYKM_NO.Text = NzStr(r("kykm_no"))
        txt_BUKN_NM.Text = NzStr(r("bukn_nm"))
        txt_BCAT_NM.Text = NzStr(r("bcat1_nm"))
        txt_SUURYO.Text = NzStr(r("b_suuryo"))
        txt_KLSRYO.Text = NzAmtStr(r("b_klsryo"))
        txt_SLSRYO.Text = NzAmtStr(r("b_slsryo"))
        txt_KNYUKN.Text = NzAmtStr(r("b_knyukn"))
        txt_GLSRYO.Text = NzAmtStr(r("b_glsryo"))
        txt_MLSRYO.Text = NzAmtStr(r("b_mlsryo"))
        txt_IJIKNR.Text = NzAmtStr(r("b_ijiknr"))
        txt_ZANRYO.Text = NzAmtStr(r("b_zanryo"))
        txt_HENL_SUM.Text = NzAmtStr(r("b_henl_sum"))
        txt_HENL_SEDT.Text = NzDtStr(r("b_henl_sedt"))
        txt_HENF_SEDT.Text = NzDtStr(r("b_henf_sedt"))

        ' 既存の解約情報
        txt_CKAIYK_F.Text = If(NzBool(r("b_ckaiyk_f")), "解約", "継続")
        txt_CKAIYK_DT.Text = NzDtStr(r("ckaiyk_dt"))
        txt_CKAIYK_ESDT_T.Text = NzDtStr(r("ckaiyk_esdt_t"))
        txt_CKAIYK_ESDT_H.Text = NzDtStr(r("ckaiyk_esdt_h"))
        txt_IYAKU_KIN.Text = NzAmtStr(r("iyaku_kin"))
        txt_HENL_F.Text = If(NzBool(r("b_henl_f")), "有", "")
        txt_HENF_F.Text = If(NzBool(r("b_henf_f")), "有", "")

        ' 新規入力フィールドをリセット（既存値を引き継ぐ）
        chk_CKAIYK_F_NEW.Checked = NzBool(r("b_ckaiyk_f"))
        txt_CKAIYK_DT_NEW.Text = NzDtStr(r("ckaiyk_dt"))
        txt_CKAIYK_ESDT_T_NEW.Text = NzDtStr(r("ckaiyk_esdt_t"))
        txt_IYAKU_KIN_NEW.Text = NzAmtStr(r("iyaku_kin"))
        _isDirty = False
    End Sub

    Private Sub ClearFields()
        For Each t In New TextBox() {
            txt_W_KYKM_ID, txt_KYKM_NO, txt_BUKN_NM, txt_BCAT_NM,
            txt_SUURYO, txt_KLSRYO, txt_SLSRYO, txt_KNYUKN,
            txt_GLSRYO, txt_MLSRYO, txt_IJIKNR, txt_ZANRYO, txt_HENL_SUM,
            txt_HENL_SEDT, txt_HENF_SEDT, txt_CKAIYK_F,
            txt_CKAIYK_DT, txt_CKAIYK_ESDT_T, txt_CKAIYK_ESDT_H,
            txt_IYAKU_KIN, txt_HENL_F, txt_HENF_F,
            txt_CKAIYK_DT_NEW, txt_CKAIYK_ESDT_T_NEW, txt_IYAKU_KIN_NEW
        }
            t.Text = ""
        Next
        chk_CKAIYK_F_NEW.Checked = False
    End Sub

    ' =========================================================
    '  合計表示（継続/解約別）
    ' =========================================================
    Private Sub ShowTotals()
        Dim cntKzok As Integer = 0, cntKyak As Integer = 0
        Dim suuryoKzok As Double = 0, suuryoKyak As Double = 0
        Dim klsryoKzok As Double = 0, klsryoKyak As Double = 0
        Dim slsryoKzok As Double = 0, slsryoKyak As Double = 0
        Dim knyuknKzok As Double = 0, knyuknKyak As Double = 0
        Dim glsryoKzok As Double = 0, glsryoKyak As Double = 0
        Dim mlsryoKzok As Double = 0, mlsryoKyak As Double = 0
        Dim ijiknrKzok As Double = 0, ijiknrKyak As Double = 0
        Dim zanryoKzok As Double = 0, zanryoKyak As Double = 0
        Dim henlSumKzok As Double = 0, henlSumKyak As Double = 0

        For Each r In _rows
            If NzBool(r("b_ckaiyk_f")) Then
                cntKyak += 1
                suuryoKyak += NzDbl(r("b_suuryo"))
                klsryoKyak += NzDbl(r("b_klsryo"))
                slsryoKyak += NzDbl(r("b_slsryo"))
                knyuknKyak += NzDbl(r("b_knyukn"))
                glsryoKyak += NzDbl(r("b_glsryo"))
                mlsryoKyak += NzDbl(r("b_mlsryo"))
                ijiknrKyak += NzDbl(r("b_ijiknr"))
                zanryoKyak += NzDbl(r("b_zanryo"))
                henlSumKyak += NzDbl(r("b_henl_sum"))
            Else
                cntKzok += 1
                suuryoKzok += NzDbl(r("b_suuryo"))
                klsryoKzok += NzDbl(r("b_klsryo"))
                slsryoKzok += NzDbl(r("b_slsryo"))
                knyuknKzok += NzDbl(r("b_knyukn"))
                glsryoKzok += NzDbl(r("b_glsryo"))
                mlsryoKzok += NzDbl(r("b_mlsryo"))
                ijiknrKzok += NzDbl(r("b_ijiknr"))
                zanryoKzok += NzDbl(r("b_zanryo"))
                henlSumKzok += NzDbl(r("b_henl_sum"))
            End If
        Next

        ' 継続合計
        txt_COUNT_KZOK.Text = cntKzok.ToString()
        txt_SUURYO_SUM_KZOK.Text = suuryoKzok.ToString("N0")
        txt_KLSRYO_SUM_KZOK.Text = klsryoKzok.ToString("N0")
        txt_SLSRYO_SUM_KZOK.Text = slsryoKzok.ToString("N0")
        txt_KNYUKN_SUM_KZOK.Text = knyuknKzok.ToString("N0")
        txt_GLSRYO_SUM_KZOK.Text = glsryoKzok.ToString("N0")
        txt_MLSRYO_SUM_KZOK.Text = mlsryoKzok.ToString("N0")
        txt_IJIKNR_SUM_KZOK.Text = ijiknrKzok.ToString("N0")
        txt_ZANRYO_SUM_KZOK.Text = zanryoKzok.ToString("N0")
        txt_HENL_SUM_SUM_KZOK.Text = henlSumKzok.ToString("N0")

        ' 解約合計
        txt_COUNT_KYAK.Text = cntKyak.ToString()
        txt_SUURYO_SUM_KYAK.Text = suuryoKyak.ToString("N0")
        txt_KLSRYO_SUM_KYAK.Text = klsryoKyak.ToString("N0")
        txt_SLSRYO_SUM_KYAK.Text = slsryoKyak.ToString("N0")
        txt_KNYUKN_SUM_KYAK.Text = knyuknKyak.ToString("N0")
        txt_GLSRYO_SUM_KYAK.Text = glsryoKyak.ToString("N0")
        txt_MLSRYO_SUM_KYAK.Text = mlsryoKyak.ToString("N0")
        txt_IJIKNR_SUM_KYAK.Text = ijiknrKyak.ToString("N0")
        txt_ZANRYO_SUM_KYAK.Text = zanryoKyak.ToString("N0")
        txt_HENL_SUM_SUM_KYAK.Text = henlSumKyak.ToString("N0")
    End Sub

    ' =========================================================
    '  ダーティフラグ管理
    ' =========================================================
    Private Sub SetDirty(sender As Object, e As EventArgs) _
        Handles txt_CKAIYK_DT_NEW.TextChanged,
                txt_CKAIYK_ESDT_T_NEW.TextChanged,
                txt_IYAKU_KIN_NEW.TextChanged
        _isDirty = True
    End Sub

    Private Sub chk_CKAIYK_F_NEW_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chk_CKAIYK_F_NEW.CheckedChanged
        _isDirty = True
    End Sub

    ' =========================================================
    '  現在レコードの保存
    ' =========================================================
    Private Sub SaveCurrentRecord()
        If _rows.Count = 0 Then Return
        Dim kykmId = CInt(txt_W_KYKM_ID.Text)
        If kykmId = 0 Then Return

        Dim newCkaiykDt As Object = DBNull.Value
        If Not String.IsNullOrWhiteSpace(txt_CKAIYK_DT_NEW.Text) Then
            Dim dt As DateTime
            If DateTime.TryParse(txt_CKAIYK_DT_NEW.Text, dt) Then newCkaiykDt = dt
        End If

        Dim newEsdtT As Object = DBNull.Value
        If Not String.IsNullOrWhiteSpace(txt_CKAIYK_ESDT_T_NEW.Text) Then
            Dim dt As DateTime
            If DateTime.TryParse(txt_CKAIYK_ESDT_T_NEW.Text, dt) Then newEsdtT = dt
        End If

        Dim newIyakuKin As Double = 0
        Double.TryParse(txt_IYAKU_KIN_NEW.Text.Replace(",", ""), newIyakuKin)

        Try
            Dim val As New Dictionary(Of String, Object) From {
                {"b_ckaiyk_f", chk_CKAIYK_F_NEW.Checked},
                {"ckaiyk_dt", newCkaiykDt},
                {"ckaiyk_esdt_t", newEsdtT},
                {"iyaku_kin", newIyakuKin}
            }
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kykm_id", kykmId)
            }
            _crud.Update("d_kykm", val, "kykm_id = @kykm_id", whereParams)
            _isDirty = False
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>全物件の解約フラグを一括保存（Form_f_KAIYAKから呼び出し可）</summary>
    Public Sub SaveCancellations()
        If _isDirty Then SaveCurrentRecord()
        LoadData()
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

    Private Sub Form_f_KAIYAK_SUB_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
