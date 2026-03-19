Imports System.Windows.Forms
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 保守料管理 ---
' Access版 f_HENF 相当
Partial Public Class Form_f_HENF
    Inherits Form

    ' 呼び出し元がセットするプロパティ
    Public Property KykmId As Double
    Public Property HenfLineId As Integer

    Private _crud As New CrudHelper()
    Private _rows As New List(Of HenfRow)
    Private _currentRowIndex As Integer = 0

    Public Structure HenfRow
        Public LineId As Integer
        Public ShriDt1 As String
        Public Klsryo As Double
        Public Kzei As Double
        Public Zritu As Double
        Public ShriKn As Integer
        Public SshriKn As Integer
        Public ShriCnt As Integer
        Public ShriEnDt As String
        Public KlsryoGokei As Double
        Public KzeiGokei As Double
        Public KlsryoZkomiGokei As Double
    End Structure

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_HENF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_W_KYKM_ID.Text = KykmId.ToString()
        txt_W_KYKM_ID.ReadOnly = True
        txt_LINE_ID.ReadOnly = True
        txt_KLSRYO_ZKOMI.ReadOnly = True

        ' 合計行は読み取り専用
        txt_KLSRYO_SUM.ReadOnly = True
        txt_KZEI_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI_SUM.ReadOnly = True
        txt_KLSRYO_GOKEI_SUM.ReadOnly = True
        txt_KZEI_GOKEI_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI_GOKEI_SUM.ReadOnly = True
        txt_SHRI_CNT_SUM.ReadOnly = True

        ' ヘッダ情報読み込み
        LoadHeaderInfo()
        LoadHenfData()
    End Sub

    Private Sub LoadHeaderInfo()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            Dim dt = _crud.GetDataTable(
                "SELECT dh.*, mh.hkmk_nm AS f_hkmk_nm, ml.lcpt_nm AS f_lcpt1_nm, " &
                "mg.gsha_nm AS f_gsha_nm, mk.koza_nm " &
                "FROM d_henf dh " &
                "LEFT JOIN m_hkmk mh ON dh.f_hkmk_id = mh.hkmk_id " &
                "LEFT JOIN m_lcpt ml ON dh.f_lcpt1_id = ml.lcpt_id " &
                "LEFT JOIN m_gsha mg ON dh.f_gsha_id = mg.gsha_id " &
                "LEFT JOIN m_koza mk ON dh.koza_id = mk.koza_id " &
                "WHERE dh.kykm_id = @kykm_id LIMIT 1", prms)

            If dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_F_HKMK_NM.Text = If(IsDBNull(row("f_hkmk_nm")), "", row("f_hkmk_nm").ToString())
                txt_F_LCPT1_NM.Text = If(IsDBNull(row("f_lcpt1_nm")), "", row("f_lcpt1_nm").ToString())
                txt_F_GSHA_NM.Text = If(IsDBNull(row("f_gsha_nm")), "", row("f_gsha_nm").ToString())
                txt_KYKBNF.Text = If(IsDBNull(row("kykbnf")), "", row("kykbnf").ToString())
                txt_KOZA_NM.Text = If(IsDBNull(row("koza_nm")), "", row("koza_nm").ToString())
                txt_START_DT.Text = If(IsDBNull(row("start_dt")), "", CDate(row("start_dt")).ToString("yyyy/MM/dd"))
                txt_LKIKAN.Text = If(IsDBNull(row("lkikan")), "", row("lkikan").ToString())
                txt_SAIKAISU.Text = If(IsDBNull(row("saikaisu")), "", row("saikaisu").ToString())
                chk_HSZEI_KJKBN_ID_MS_F.Checked = If(IsDBNull(row("hszei_kjkbn_id_ms_f")), False,
                    CBool(row("hszei_kjkbn_id_ms_f")))
            End If
        Catch ex As Exception
            ' ヘッダ情報取得失敗は警告のみ
        End Try
    End Sub

    Private Sub LoadHenfData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            Dim dt = _crud.GetDataTable(
                "SELECT * FROM d_henf WHERE kykm_id = @kykm_id ORDER BY line_id", prms)

            _rows.Clear()
            For Each row As DataRow In dt.Rows
                Dim r As New HenfRow()
                r.LineId = CInt(row("line_id"))
                r.ShriDt1 = If(IsDBNull(row("shri_dt1")), "", CDate(row("shri_dt1")).ToString("yyyy/MM/dd"))
                r.Klsryo = If(IsDBNull(row("klsryo")), 0, CDbl(row("klsryo")))
                r.Kzei = If(IsDBNull(row("kzei")), 0, CDbl(row("kzei")))
                r.Zritu = If(IsDBNull(row("zritu")), 0, CDbl(row("zritu")))
                r.ShriKn = If(IsDBNull(row("shri_kn")), 0, CInt(row("shri_kn")))
                r.SshriKn = If(IsDBNull(row("sshri_kn")), 0, CInt(row("sshri_kn")))
                r.ShriCnt = If(IsDBNull(row("shri_cnt")), 0, CInt(row("shri_cnt")))
                r.KlsryoGokei = r.Klsryo * r.ShriCnt
                r.KzeiGokei = r.Kzei * r.ShriCnt
                r.KlsryoZkomiGokei = (r.Klsryo + r.Kzei) * r.ShriCnt
                _rows.Add(r)
            Next

            If _rows.Count > 0 Then
                _currentRowIndex = 0
                RenderCurrentRow()
            Else
                ClearFields()
            End If
            UpdateSumFields()
        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RenderCurrentRow()
        If _currentRowIndex < 0 OrElse _currentRowIndex >= _rows.Count Then Return
        Dim r = _rows(_currentRowIndex)
        txt_LINE_ID.Text = r.LineId.ToString()
        txt_SHRI_DT1.Text = r.ShriDt1
        txt_KLSRYO.Text = r.Klsryo.ToString()
        txt_KZEI.Text = r.Kzei.ToString()
        txt_KLSRYO_ZKOMI.Text = (r.Klsryo + r.Kzei).ToString()
        txt_ZRITU.Text = r.Zritu.ToString()
        txt_SHRI_KN.Text = r.ShriKn.ToString()
        txt_SSHRI_KN.Text = r.SshriKn.ToString()
        txt_SHRI_CNT.Text = r.ShriCnt.ToString()
        txt_SHRI_EN_DT.Text = r.ShriEnDt
        txt_KLSRYO_GOKEI.Text = r.KlsryoGokei.ToString()
        txt_KZEI_GOKEI.Text = r.KzeiGokei.ToString()
        txt_KLSRYO_ZKOMI_GOKEI.Text = r.KlsryoZkomiGokei.ToString()
    End Sub

    Private Sub ClearFields()
        txt_LINE_ID.Text = ""
        txt_SHRI_DT1.Text = ""
        txt_KLSRYO.Text = ""
        txt_KZEI.Text = ""
        txt_KLSRYO_ZKOMI.Text = ""
        txt_ZRITU.Text = ""
        txt_SHRI_KN.Text = ""
        txt_SSHRI_KN.Text = ""
        txt_SHRI_CNT.Text = ""
        txt_SHRI_EN_DT.Text = ""
        txt_KLSRYO_GOKEI.Text = ""
        txt_KZEI_GOKEI.Text = ""
        txt_KLSRYO_ZKOMI_GOKEI.Text = ""
    End Sub

    Private Sub UpdateSumFields()
        txt_KLSRYO_SUM.Text = _rows.Sum(Function(r) r.Klsryo).ToString()
        txt_KZEI_SUM.Text = _rows.Sum(Function(r) r.Kzei).ToString()
        txt_KLSRYO_ZKOMI_SUM.Text = _rows.Sum(Function(r) r.Klsryo + r.Kzei).ToString()
        txt_KLSRYO_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KlsryoGokei).ToString()
        txt_KZEI_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KzeiGokei).ToString()
        txt_KLSRYO_ZKOMI_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KlsryoZkomiGokei).ToString()
        txt_SHRI_CNT_SUM.Text = _rows.Sum(Function(r) r.ShriCnt).ToString()
    End Sub

    ' [スケジュール展開] ボタン
    Private Sub cmd_展開_Click(sender As Object, e As EventArgs) Handles cmd_展開.Click
        If _rows.Count = 0 Then Return
        Dim r = _rows(_currentRowIndex)

        Dim frm As New Form_f_HEN_SCH()
        frm.KykmId = KykmId
        frm.LineId = r.LineId
        frm.ShriDt = r.ShriDt1
        frm.Klsryo = r.Klsryo
        frm.Kzei = r.Kzei
        frm.Zritu = r.Zritu
        frm.SshriKn = r.SshriKn

        If frm.ShowDialog() = DialogResult.OK Then
            If frm.IsDeleted Then
                DeleteHenfRow(r.LineId)
            Else
                Dim updated = r
                updated.ShriDt1 = frm.ResultShriDt
                updated.Klsryo = frm.ResultKlsryo
                updated.Kzei = frm.ResultKzei
                updated.Zritu = frm.ResultZritu
                updated.SshriKn = frm.ResultSshriKn
                updated.KlsryoGokei = updated.Klsryo * updated.ShriCnt
                updated.KzeiGokei = updated.Kzei * updated.ShriCnt
                updated.KlsryoZkomiGokei = (updated.Klsryo + updated.Kzei) * updated.ShriCnt
                _rows(_currentRowIndex) = updated
                RenderCurrentRow()
                UpdateSumFields()
            End If
        End If
    End Sub

    ' [行削除] ボタン
    Private Sub cmd_削除_Click(sender As Object, e As EventArgs) Handles cmd_削除.Click
        If _rows.Count = 0 Then Return
        Dim r = _rows(_currentRowIndex)
        Dim confirm = MessageBox.Show("行 " & r.LineId & " を削除しますか？", "確認",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.No Then Return
        DeleteHenfRow(r.LineId)
    End Sub

    Private Sub DeleteHenfRow(lineId As Integer)
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            prms.Add(New NpgsqlParameter("@line_id", lineId))
            _crud.ExecuteNonQuery(
                "DELETE FROM d_henf WHERE kykm_id = @kykm_id AND line_id = @line_id", prms)
            LoadHenfData()
            MessageBox.Show("削除しました。")
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [戻る] ボタン
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.Close()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class