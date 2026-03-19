Imports System.Globalization
Imports LeaseM4BS.DataAccess

''' <summary>
''' VTC顧客 支払仕訳 支払先確認フォーム
''' Access版 fc_支払仕訳_VTC_支払先確認 相当
''' tw_fc_swk_wrk（VTC 支払仕訳データ）の1件を表示し、予定月計上フラグを管理する。
''' 予定月計上F: ext_char1 = '1'(ON) / '0'(OFF)
''' 全行予定月計上: 全行をON、全行予定月計上解除: 全行をOFF
''' 翌月にずれる場合に予定月計上: 支払日の月 != 処理月の場合にON
''' </summary>
Partial Public Class Form_fc_支払仕訳_VTC_支払先確認
    Inherits Form

    Private _crud As New CrudHelper()
    Private _currentRowId As Integer = -1

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_支払仕訳_VTC_支払先確認_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCurrentRow()
    End Sub

    Private Sub LoadCurrentRow()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT id, den_date, shri_dt, tekiyo, lsryo, zei, ext_char1 " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳' " &
                "ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                _currentRowId = CInt(row("id"))

                Dim shriDt = If(IsDBNull(row("shri_dt")), CType(Nothing, DateTime?), CType(row("shri_dt"), DateTime))
                If shriDt.HasValue Then
                    txt_予定支払日.Text = shriDt.Value.ToString("yyyy/MM/dd")
                    txt_予定支払日_曜日.Text = shriDt.Value.ToString("ddd", New CultureInfo("ja-JP"))
                    txt_実際支払日.Text = shriDt.Value.ToString("yyyy/MM/dd")
                    txt_実際支払日_曜日.Text = shriDt.Value.ToString("ddd", New CultureInfo("ja-JP"))
                End If

                txt_LCPT1_NM.Text = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())
                txt_KKBN_NM.Text = ""
                txt_SHHO_NM.Text = ""

                Dim lsryo = If(IsDBNull(row("lsryo")), 0.0, CType(row("lsryo"), Double))
                Dim zei = If(IsDBNull(row("zei")), 0.0, CType(row("zei"), Double))
                txt_税抜き額.Text = lsryo.ToString("N0")
                txt_消費税額.Text = zei.ToString("N0")

                Dim isYotei = If(IsDBNull(row("ext_char1")), False, row("ext_char1").ToString() = "1")
                chk_予定月計上F.Checked = isYotei
            End If

            ' 合計を計算して表示
            Dim dtSum = _crud.GetDataTable(
                "SELECT COALESCE(SUM(lsryo), 0) AS lsryo_sum, COALESCE(SUM(zei), 0) AS zei_sum " &
                "FROM tw_fc_swk_wrk WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳'")
            If dtSum IsNot Nothing AndAlso dtSum.Rows.Count > 0 Then
                txt_税抜き額_SUM.Text = CType(dtSum.Rows(0)("lsryo_sum"), Double).ToString("N0")
                txt_消費税額_SUM.Text = CType(dtSum.Rows(0)("zei_sum"), Double).ToString("N0")
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_戻る_Click(sender As Object, e As EventArgs) Handles cmd_戻る.Click
        Me.Close()
    End Sub

    Private Sub cmd_全行予定月計上_Click(sender As Object, e As EventArgs) Handles cmd_全行予定月計上.Click
        Try
            _crud.ExecuteNonQuery(
                "UPDATE tw_fc_swk_wrk SET ext_char1 = '1' " &
                "WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳'")
            LoadCurrentRow()
        Catch ex As Exception
            MessageBox.Show($"更新エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_全行予定月計上解除_Click(sender As Object, e As EventArgs) Handles cmd_全行予定月計上解除.Click
        Try
            _crud.ExecuteNonQuery(
                "UPDATE tw_fc_swk_wrk SET ext_char1 = '0' " &
                "WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳'")
            LoadCurrentRow()
        Catch ex As Exception
            MessageBox.Show($"更新エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 支払日の月が処理月（den_date）と異なる行（翌月にずれる）を予定月計上ON
    ''' </summary>
    Private Sub cmd_翌月にずれる場合に予定月計上_Click(sender As Object, e As EventArgs) Handles cmd_翌月にずれる場合に予定月計上.Click
        Try
            ' shri_dtの月 != den_dateの月 の行をON
            _crud.ExecuteNonQuery(
                "UPDATE tw_fc_swk_wrk SET ext_char1 = '1' " &
                "WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳' " &
                "  AND DATE_TRUNC('month', shri_dt) != DATE_TRUNC('month', den_date::date)")
            LoadCurrentRow()
        Catch ex As Exception
            MessageBox.Show($"更新エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_fc_支払仕訳_VTC_支払先確認_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
