Imports System.Windows.Forms
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 変額リース料管理 ---
' Access版 f_HENL 相当
Partial Public Class Form_f_HENL
    Inherits Form

    ' 呼び出し元がセットするプロパティ
    Public Property KykmId As Double

    Private _crud As New CrudHelper()
    Private _rows As New List(Of HenlRow)
    Private _currentRowIndex As Integer = 0

    ' 行データ構造
    Public Structure HenlRow
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

    ' 動的生成ナビゲーションボタン (Access版レコードナビゲーション相当)
    Private WithEvents cmd_前 As Button
    Private WithEvents cmd_次 As Button
    Private lbl_RowPos As Label

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_HENL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_W_KYKM_ID.Text = KykmId.ToString()
        txt_W_KYKM_ID.ReadOnly = True
        txt_LINE_ID.ReadOnly = True

        ' 合計行は読み取り専用
        txt_KLSRYO_SUM.ReadOnly = True
        txt_KZEI_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI_SUM.ReadOnly = True
        txt_KLSRYO_GOKEI_SUM.ReadOnly = True
        txt_KZEI_GOKEI_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI_GOKEI_SUM.ReadOnly = True
        txt_SHRI_CNT_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI.ReadOnly = True

        ' ナビゲーションボタンを動的生成 (Designer.vb に存在しないため)
        CreateNavigationButtons()

        LoadHenlData()
    End Sub

    Private Sub CreateNavigationButtons()
        cmd_前 = New Button()
        cmd_前.Text = "前(&P)"
        cmd_前.Size = New Drawing.Size(60, 26)
        cmd_前.Location = New Drawing.Point(260, 3)
        Me.Controls.Add(cmd_前)

        cmd_次 = New Button()
        cmd_次.Text = "次(&N)"
        cmd_次.Size = New Drawing.Size(60, 26)
        cmd_次.Location = New Drawing.Point(325, 3)
        Me.Controls.Add(cmd_次)

        lbl_RowPos = New Label()
        lbl_RowPos.AutoSize = True
        lbl_RowPos.Location = New Drawing.Point(395, 8)
        Me.Controls.Add(lbl_RowPos)
    End Sub

    Private Sub LoadHenlData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            Dim dt = _crud.GetDataTable(
                "SELECT * FROM d_henl WHERE kykm_id = @kykm_id ORDER BY line_id", prms)

            _rows.Clear()
            For Each row As DataRow In dt.Rows
                Dim r As New HenlRow()
                r.LineId = CInt(row("line_id"))
                r.ShriDt1 = ToDateStr(row("shri_dt1"))
                r.Klsryo = If(IsDBNull(row("klsryo")), 0, CDbl(row("klsryo")))
                r.Kzei = If(IsDBNull(row("kzei")), 0, CDbl(row("kzei")))
                r.Zritu = If(IsDBNull(row("zritu")), 0, CDbl(row("zritu")))
                r.ShriKn = If(IsDBNull(row("shri_kn")), 0, CInt(row("shri_kn")))
                r.SshriKn = If(IsDBNull(row("sshri_kn")), 0, CInt(row("sshri_kn")))
                r.ShriCnt = If(IsDBNull(row("shri_cnt")), 0, CInt(row("shri_cnt")))
                ' 最終支払日を計算 (開始日 + 支払間隔 × 支払回数)
                If Not String.IsNullOrEmpty(r.ShriDt1) AndAlso r.ShriKn > 0 AndAlso r.ShriCnt > 0 Then
                    Dim startDt As Date
                    If Date.TryParse(r.ShriDt1, startDt) Then
                        r.ShriEnDt = startDt.AddMonths(r.ShriKn * (r.ShriCnt - 1)).ToString("yyyy/MM/dd")
                    End If
                End If
                ' 合計は計算
                r.KlsryoGokei = r.Klsryo * r.ShriCnt
                r.KzeiGokei = r.Kzei * r.ShriCnt
                r.KlsryoZkomiGokei = (r.Klsryo + r.Kzei) * r.ShriCnt
                _rows.Add(r)
            Next

            If _rows.Count > 0 Then
                If _currentRowIndex >= _rows.Count Then _currentRowIndex = _rows.Count - 1
                If _currentRowIndex < 0 Then _currentRowIndex = 0
                RenderCurrentRow()
            Else
                ClearFields()
            End If

            UpdateSumFields()
            UpdateRowPosition()
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

    ' 合計行の自動更新（FR-005）
    Private Sub UpdateSumFields()
        txt_KLSRYO_SUM.Text = _rows.Sum(Function(r) r.Klsryo).ToString()
        txt_KZEI_SUM.Text = _rows.Sum(Function(r) r.Kzei).ToString()
        txt_KLSRYO_ZKOMI_SUM.Text = _rows.Sum(Function(r) r.Klsryo + r.Kzei).ToString()
        txt_KLSRYO_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KlsryoGokei).ToString()
        txt_KZEI_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KzeiGokei).ToString()
        txt_KLSRYO_ZKOMI_GOKEI_SUM.Text = _rows.Sum(Function(r) r.KlsryoZkomiGokei).ToString()
        txt_SHRI_CNT_SUM.Text = _rows.Sum(Function(r) r.ShriCnt).ToString()
    End Sub

    ' [スケジュール展開] ボタン → Form_f_HEN_SCH をモーダルで開く
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
                ' 行削除
                DeleteHenlRow(r.LineId)
            Else
                ' 値を反映
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
                PersistHenlRow(updated)
                RenderCurrentRow()
                UpdateSumFields()
            End If
        End If
    End Sub

    ' [行削除] ボタン
    Private Sub cmd_削除_Click(sender As Object, e As EventArgs) Handles cmd_削除.Click
        If _rows.Count = 0 Then Return
        Dim r = _rows(_currentRowIndex)

        ' 最後の1行の場合は警告（NFR-002）
        If _rows.Count = 1 Then
            Dim warn = MessageBox.Show("変額リース料が0件になります。削除しますか？", "警告",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If warn = DialogResult.No Then Return
        Else
            Dim confirm = MessageBox.Show("行 " & r.LineId & " を削除しますか？", "確認",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = DialogResult.No Then Return
        End If

        DeleteHenlRow(r.LineId)
    End Sub

    Private Sub DeleteHenlRow(lineId As Integer)
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            prms.Add(New NpgsqlParameter("@line_id", lineId))
            _crud.ExecuteNonQuery(
                "DELETE FROM d_henl WHERE kykm_id = @kykm_id AND line_id = @line_id", prms)

            LoadHenlData()
            MessageBox.Show("削除しました。")
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [前] ボタン — Access版レコードナビゲーション相当
    Private Sub cmd_前_Click(sender As Object, e As EventArgs) Handles cmd_前.Click
        If _currentRowIndex > 0 Then
            _currentRowIndex -= 1
            RenderCurrentRow()
            UpdateRowPosition()
        End If
    End Sub

    ' [次] ボタン
    Private Sub cmd_次_Click(sender As Object, e As EventArgs) Handles cmd_次.Click
        If _currentRowIndex < _rows.Count - 1 Then
            _currentRowIndex += 1
            RenderCurrentRow()
            UpdateRowPosition()
        End If
    End Sub

    Private Sub UpdateRowPosition()
        If lbl_RowPos IsNot Nothing Then
            If _rows.Count = 0 Then
                lbl_RowPos.Text = "0 / 0"
            Else
                lbl_RowPos.Text = (_currentRowIndex + 1).ToString() & " / " & _rows.Count.ToString()
            End If
        End If
    End Sub

    ''' <summary>
    ''' スケジュール展開後の編集結果をDBに永続化する (Access版の動作を再現)
    ''' </summary>
    Private Sub PersistHenlRow(r As HenlRow)
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            prms.Add(New NpgsqlParameter("@line_id", r.LineId))
            prms.Add(New NpgsqlParameter("@klsryo", r.Klsryo))
            prms.Add(New NpgsqlParameter("@kzei", r.Kzei))
            prms.Add(New NpgsqlParameter("@zritu", r.Zritu))
            prms.Add(New NpgsqlParameter("@sshri_kn", r.SshriKn))
            prms.Add(New NpgsqlParameter("@shri_kn", r.ShriKn))

            Dim shriDt As Object = DBNull.Value
            If Not String.IsNullOrEmpty(r.ShriDt1) Then
                Dim dt As Date
                If Date.TryParse(r.ShriDt1, dt) Then shriDt = dt
            End If
            prms.Add(New NpgsqlParameter("@shri_dt1", shriDt))

            _crud.ExecuteNonQuery(
                "UPDATE d_henl SET klsryo = @klsryo, kzei = @kzei, zritu = @zritu, " &
                "sshri_kn = @sshri_kn, shri_kn = @shri_kn, shri_dt1 = @shri_dt1 " &
                "WHERE kykm_id = @kykm_id AND line_id = @line_id", prms)
        Catch ex As Exception
            MessageBox.Show("保存エラー: " & ex.Message, "エラー",
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