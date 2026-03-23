Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 物件サブ画面 (f_KYKM_SUB)
''' d_haif (配賦行) の一覧を表示し、配賦率合計・金額合計を算出する。
''' </summary>
Partial Public Class Form_f_KYKM_SUB
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykmId As Integer = 0
    Private _rows As New List(Of System.Data.DataRow)
    Private _currentIndex As Integer = 0

    ''' <summary>ダイアログ終了時に選択中の line_id を返す（-1 = 未選択）</summary>
    Public Property SelectedLineId As Integer = -1

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
    Private Sub Form_f_KYKM_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    ' =========================================================
    '  データ読み込み
    ' =========================================================
    Public Sub LoadData()
        Try
            Dim sql As String =
                "SELECT h.*, " &
                "       b.bcat1_nm, k.hkmk_nm, r.rsrvh1_nm " &
                "FROM d_haif h " &
                "LEFT JOIN m_bcat b ON b.bcat_id = h.h_bcat_id " &
                "LEFT JOIN m_hkmk k ON k.hkmk_id = h.hkmk_id " &
                "LEFT JOIN m_rsrvh1 r ON r.rsrvh1_id = h.rsrvh1_id " &
                "WHERE h.kykm_id = @id " &
                "ORDER BY h.line_id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)}
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
            txt_COUNT.Text = "0"
            Return
        End If

        Dim r = _rows(_currentIndex)
        Dim lineIdVal As Integer = 0
        Integer.TryParse(NzStr(r("line_id")), lineIdVal)
        SelectedLineId = lineIdVal
        txt_W_KYKM_ID.Text = NzStr(r("kykm_id"))
        txt_LINE_ID.Text = NzStr(r("line_id"))
        txt_HAIFRITU.Text = NzStr(r("haifritu"))
        txt_BCAT1_NM.Text = NzStr(r("bcat1_nm"))
        txt_HKMK_NM.Text = NzStr(r("hkmk_nm"))
        txt_RSRVH1_NM.Text = NzStr(r("rsrvh1_nm"))
        txt_H_KLSRYO.Text = NzAmtStr(r("h_klsryo"))
        txt_H_MLSRYO.Text = NzAmtStr(r("h_mlsryo"))
        txt_H_KZEI.Text = NzAmtStr(r("h_kzei"))
        txt_H_MZEI.Text = NzAmtStr(r("h_mzei"))
        txt_H_KLSRYO_ZKOMI.Text = NzAmtStr(r("h_klsryo_zkomi"))
        txt_H_MLSRYO_ZKOMI.Text = NzAmtStr(r("h_mlsryo_zkomi"))
        txt_H_ZOKUSEI1.Text = NzStr(r("h_zokusei1"))
        txt_COUNT.Text = _rows.Count.ToString()
    End Sub

    Private Sub ClearFields()
        Dim fields() As TextBox = {
            txt_W_KYKM_ID, txt_LINE_ID, txt_HAIFRITU,
            txt_BCAT1_NM, txt_HKMK_NM, txt_RSRVH1_NM,
            txt_H_KLSRYO, txt_H_MLSRYO, txt_H_KZEI, txt_H_MZEI,
            txt_H_KLSRYO_ZKOMI, txt_H_MLSRYO_ZKOMI, txt_H_ZOKUSEI1
        }
        For Each t In fields
            t.Text = ""
        Next
    End Sub

    ' =========================================================
    '  合計表示 + 配賦率100%チェック
    ' =========================================================
    Private Sub ShowTotals()
        Dim sumHaifritu As Double = 0
        Dim sumKlsryo As Double = 0, sumMlsryo As Double = 0
        Dim sumKzei As Double = 0, sumMzei As Double = 0

        For Each r In _rows
            sumHaifritu += NzDbl(r("haifritu"))
            sumKlsryo += NzDbl(r("h_klsryo"))
            sumMlsryo += NzDbl(r("h_mlsryo"))
            sumKzei += NzDbl(r("h_kzei"))
            sumMzei += NzDbl(r("h_mzei"))
        Next

        txt_HAIFRITU_SUM.Text = sumHaifritu.ToString("N2")
        txt_H_KLSRYO_SUM.Text = sumKlsryo.ToString("N0")
        txt_H_MLSRYO_SUM.Text = sumMlsryo.ToString("N0")
        txt_H_KZEI_SUM.Text = sumKzei.ToString("N0")
        txt_H_MZEI_SUM.Text = sumMzei.ToString("N0")

        ' 配賦率合計が100%でない場合は警告表示
        If _rows.Count > 0 AndAlso Math.Abs(sumHaifritu - 100.0) > 0.001 Then
            txt_HAIFRITU_SUM.BackColor = System.Drawing.Color.LightYellow
        Else
            txt_HAIFRITU_SUM.BackColor = System.Drawing.SystemColors.Window
        End If

        ' 1回額合計 > 物件1回額チェック
        Try
            Dim bkDt = _crud.GetDataTable(
                "SELECT COALESCE(b_klsryo, 0) AS b_klsryo FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            If bkDt IsNot Nothing AndAlso bkDt.Rows.Count > 0 Then
                Dim bKlsryo = Convert.ToDouble(bkDt.Rows(0)("b_klsryo"))
                If sumKlsryo > bKlsryo AndAlso bKlsryo > 0 Then
                    txt_H_KLSRYO_SUM.BackColor = System.Drawing.Color.LightPink
                Else
                    txt_H_KLSRYO_SUM.BackColor = System.Drawing.SystemColors.Window
                End If
            End If
        Catch
        End Try
    End Sub

    ''' <summary>配賦率合計が100%かチェック。Trueなら問題なし。</summary>
    Public Function ValidateHaifRitu() As Boolean
        Dim sumHaifritu As Double = 0
        For Each r In _rows
            sumHaifritu += NzDbl(r("haifritu"))
        Next
        If _rows.Count > 0 AndAlso Math.Abs(sumHaifritu - 100.0) > 0.001 Then
            MessageBox.Show(
                $"配賦率の合計が100%になっていません（現在: {sumHaifritu:N2}%）。",
                "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_配賦率_Click(sender As Object, e As EventArgs) Handles cmd_配賦率.Click
        If _kykmId = 0 OrElse _rows.Count = 0 Then Return
        Try
            ' 物件の1回額を取得
            Dim bkDt = _crud.GetDataTable(
                "SELECT COALESCE(b_klsryo, 0) AS b_klsryo FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            If bkDt Is Nothing OrElse bkDt.Rows.Count = 0 Then Return
            Dim bKlsryo = Convert.ToDouble(bkDt.Rows(0)("b_klsryo"))
            If bKlsryo = 0 Then
                MessageBox.Show("物件の1回額が0のため配賦率を計算できません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 各行の配賦率を算出
            Dim totalRitu As Double = 0
            Dim updates As New List(Of Tuple(Of Integer, Double))  ' (line_id, haifritu)
            For i = 0 To _rows.Count - 1
                Dim r = _rows(i)
                Dim lineId = Convert.ToInt32(r("line_id"))
                Dim hKlsryo = NzDbl(r("h_klsryo"))
                Dim ritu = Math.Round(hKlsryo / bKlsryo * 100.0, 2)
                If i = _rows.Count - 1 Then
                    ' 最終行で端数調整
                    ritu = Math.Round(100.0 - totalRitu, 2)
                End If
                totalRitu += ritu
                updates.Add(Tuple.Create(lineId, ritu))
            Next

            ' 一括UPDATE
            For Each upd In updates
                _crud.ExecuteNonQuery(
                    "UPDATE d_haif SET haifritu = @ritu WHERE kykm_id = @id AND line_id = @lid",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@ritu", upd.Item2),
                        New NpgsqlParameter("@id", _kykmId),
                        New NpgsqlParameter("@lid", upd.Item1)
                    })
            Next

            MessageBox.Show("配賦率を再計算しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadData()
        Catch ex As Exception
            MessageBox.Show("配賦率計算エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_期間額_Click(sender As Object, e As EventArgs) Handles cmd_期間額.Click
        If _kykmId = 0 OrElse _rows.Count = 0 Then Return
        Try
            ' 物件の金額を取得
            Dim bkDt = _crud.GetDataTable(
                "SELECT COALESCE(b_klsryo,0) AS b_klsryo, COALESCE(b_kzei,0) AS b_kzei, " &
                "       COALESCE(b_mlsryo,0) AS b_mlsryo, COALESCE(b_mzei,0) AS b_mzei " &
                "FROM d_kykm WHERE kykm_id = @id",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykmId)})
            If bkDt Is Nothing OrElse bkDt.Rows.Count = 0 Then Return
            Dim bk = bkDt.Rows(0)
            Dim bKlsryo = Convert.ToDouble(bk("b_klsryo"))
            Dim bKzei = Convert.ToDouble(bk("b_kzei"))
            Dim bMlsryo = Convert.ToDouble(bk("b_mlsryo"))
            Dim bMzei = Convert.ToDouble(bk("b_mzei"))

            Dim sumK As Double = 0, sumKz As Double = 0, sumM As Double = 0, sumMz As Double = 0

            For i = 0 To _rows.Count - 1
                Dim r = _rows(i)
                Dim lineId = Convert.ToInt32(r("line_id"))
                Dim ritu = NzDbl(r("haifritu"))

                Dim hK, hKz, hM, hMz As Double
                If i = _rows.Count - 1 Then
                    ' 最終行: 端数調整
                    hK = bKlsryo - sumK
                    hKz = bKzei - sumKz
                    hM = bMlsryo - sumM
                    hMz = bMzei - sumMz
                Else
                    hK = Math.Truncate(bKlsryo * ritu / 100.0)
                    hKz = Math.Truncate(bKzei * ritu / 100.0)
                    hM = Math.Truncate(bMlsryo * ritu / 100.0)
                    hMz = Math.Truncate(bMzei * ritu / 100.0)
                End If

                sumK += hK : sumKz += hKz : sumM += hM : sumMz += hMz

                _crud.ExecuteNonQuery(
                    "UPDATE d_haif SET h_klsryo=@k, h_kzei=@kz, h_klsryo_zkomi=@kzk, " &
                    "h_mlsryo=@m, h_mzei=@mz, h_mlsryo_zkomi=@mzk " &
                    "WHERE kykm_id=@id AND line_id=@lid",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@k", hK),
                        New NpgsqlParameter("@kz", hKz),
                        New NpgsqlParameter("@kzk", hK + hKz),
                        New NpgsqlParameter("@m", hM),
                        New NpgsqlParameter("@mz", hMz),
                        New NpgsqlParameter("@mzk", hM + hMz),
                        New NpgsqlParameter("@id", _kykmId),
                        New NpgsqlParameter("@lid", lineId)
                    })
            Next

            MessageBox.Show("期間額を再配分しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadData()
        Catch ex As Exception
            MessageBox.Show("期間額計算エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_f_KYKM_SUB_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
