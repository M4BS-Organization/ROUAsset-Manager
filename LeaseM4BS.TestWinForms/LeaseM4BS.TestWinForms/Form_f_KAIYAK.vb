Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 中途解約フォーム (f_KAIYAK)
''' d_kykh の契約情報を表示し、解約実行（d_kykm.b_ckaiyk_f=TRUE）を行う。
''' </summary>
Partial Public Class Form_f_KAIYAK
    Inherits Form

    Private _crud As New CrudHelper()
    Private _kykhId As Double = 0

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
    Private Sub Form_f_KAIYAK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadContract()
    End Sub

    ' =========================================================
    '  契約情報読み込み
    ' =========================================================
    Private Sub LoadContract()
        If _kykhId = 0 Then Return
        Try
            Dim sql As String =
                "SELECT kyak_dt, start_dt, lkikan, shri_kn, shri_cnt, " &
                "       shri_dt1, shri_dt2, shri_dt3, shri_en_dt, jencho_f " &
                "FROM d_kykh WHERE kykh_id = @id"
            Dim prm As New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)}
            Dim dt = _crud.GetDataTable(sql, prm)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return

            Dim r = dt.Rows(0)
            txt_KYAK_DT.Text = NzDtStr(r("kyak_dt"))
            txt_START_DT.Text = NzDtStr(r("start_dt"))
            txt_LKIKAN.Text = NzStr(r("lkikan"))
            txt_SHRI_KN.Text = NzStr(r("shri_kn"))
            txt_SHRI_CNT.Text = NzStr(r("shri_cnt"))
            txt_SHRI_DT1.Text = NzDtStr(r("shri_dt1"))
            txt_SHRI_DT2.Text = NzDtStr(r("shri_dt2"))
            txt_SHRI_DT3.Text = NzStr(r("shri_dt3"))
            txt_SHRI_EN_DT.Text = NzDtStr(r("shri_en_dt"))
            chk_JENCHO_F.Checked = NzBool(r("jencho_f"))
        Catch ex As Exception
            MessageBox.Show("契約情報読み込みエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  ボタンイベント
    ' =========================================================
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If _kykhId = 0 Then
            MessageBox.Show("契約が指定されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- バリデーション ---
        ' 解約対象物件の存在チェック
        Try
            Dim cntDt = _crud.GetDataTable(
                "SELECT COUNT(*) AS cnt FROM d_kykm WHERE kykh_id = @id AND b_ckaiyk_f = FALSE",
                New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", _kykhId)})
            If cntDt Is Nothing OrElse Convert.ToInt32(cntDt.Rows(0)("cnt")) = 0 Then
                MessageBox.Show("解約対象の物件がありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        Catch ex As Exception
            MessageBox.Show("チェックエラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' 解約日の妥当性チェック
        Dim startDt As DateTime
        Dim endDt As DateTime
        Dim hasStartDt = DateTime.TryParse(txt_START_DT.Text, startDt)
        Dim kaiyakDt = DateTime.Now

        If hasStartDt AndAlso kaiyakDt < startDt Then
            MessageBox.Show("解約日が開始日より前です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' 延長契約でない場合: 終了日チェック
        If Not chk_JENCHO_F.Checked Then
            Dim lkikan = ParseIntFromText(txt_LKIKAN.Text)
            If hasStartDt AndAlso lkikan > 0 Then
                endDt = startDt.AddMonths(lkikan).AddDays(-1)
                If kaiyakDt > endDt Then
                    If MessageBox.Show($"解約日が終了日({endDt:yyyy/MM/dd})を超えています。続行しますか？",
                                       "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                        Return
                    End If
                End If
            End If
        End If

        If MessageBox.Show(
            "選択した契約の全物件を解約します。よろしいですか？",
            "解約確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        ' --- 解約実行 ---
        Try
            _crud.BeginTransaction()
            Dim val As New Dictionary(Of String, Object) From {
                {"b_ckaiyk_f", True},
                {"ckaiyk_dt", kaiyakDt}
            }
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kykh_id", _kykhId)
            }
            _crud.Update("d_kykm", val, "kykh_id = @kykh_id AND b_ckaiyk_f = FALSE", whereParams)
            _crud.Commit()

            MessageBox.Show("解約処理が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("解約処理エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_KAIYAK_ALL_Click(sender As Object, e As EventArgs) Handles cmd_KAIYAK_ALL.Click
        Dim frm As New Form_f_KAIYAK_ALL()
        frm.SetParams(_kykhId)
        frm.ShowDialog(Me)
        LoadContract()
    End Sub

    Private Function ParseIntFromText(text As String) As Integer
        Dim result As Integer = 0
        Integer.TryParse(text, result)
        Return result
    End Function

    Private Sub Form_f_KAIYAK_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
