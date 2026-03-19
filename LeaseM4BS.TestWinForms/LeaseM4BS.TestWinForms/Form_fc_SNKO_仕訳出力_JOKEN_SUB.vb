Imports System.Globalization
Imports LeaseM4BS.DataAccess

''' <summary>
''' SNKO顧客 支払仕訳出力 明細リストフォーム
''' Access版 fc_SNKO_仕訳出力_JOKEN_SUB 相当
''' tw_fc_swk_wrk（SNKO 支払仕訳データ）の最初の1件を表示する。
''' 3種類の仕訳パターン（費用/現金・費用/未払・未払/現金）の出力指示チェックと
''' 計上日・税込額・前回出力状況を表示する。
''' </summary>
Partial Public Class Form_fc_SNKO_仕訳出力_JOKEN_SUB
    Inherits Form

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_SNKO_仕訳出力_JOKEN_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadFirstRow()
    End Sub

    Private Sub LoadFirstRow()
        Try
            Dim dt = _crud.GetDataTable(
                "SELECT id, den_date, shri_dt, tekiyo, lsryo, zei, " &
                "       ext_char1, ext_char2, ext_char3, ext_char4 " &
                "FROM tw_fc_swk_wrk " &
                "WHERE customer_cd = 'SNKO' AND swk_kbn = '支払仕訳' " &
                "ORDER BY den_no, gyo_no LIMIT 1")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)

                ' 支払先・銀行口座
                txt_LCPT1_NM.Text = If(IsDBNull(row("tekiyo")), "", row("tekiyo").ToString())

                ' 支払予定日
                Dim shriDt = If(IsDBNull(row("shri_dt")), CType(Nothing, DateTime?), CType(row("shri_dt"), DateTime))
                If shriDt.HasValue Then
                    txt_支払予定日.Text = shriDt.Value.ToString("yyyy/MM/dd")
                    txt_支払予定日_曜日.Text = shriDt.Value.ToString("ddd", New CultureInfo("ja-JP"))
                End If

                ' 税込額（lsryo + zei）
                Dim lsryo = If(IsDBNull(row("lsryo")), 0.0, CType(row("lsryo"), Double))
                Dim zei = If(IsDBNull(row("zei")), 0.0, CType(row("zei"), Double))
                Dim zeikomi = lsryo + zei
                txt_税込額.Text = zeikomi.ToString("N0")
                テキスト86.Text = zeikomi.ToString("N0")

                ' 出力指示チェック (ext_char1=費用現金F, ext_char2=費用未払F, ext_char3=未払現金F)
                chk_費用_現金_F.Checked = (Not IsDBNull(row("ext_char1"))) AndAlso row("ext_char1").ToString() = "1"
                chk_費用_未払_F.Checked = (Not IsDBNull(row("ext_char2"))) AndAlso row("ext_char2").ToString() = "1"
                chk_未払_現金_F.Checked = (Not IsDBNull(row("ext_char3"))) AndAlso row("ext_char3").ToString() = "1"

                ' 前回出力状況（output_f 列から判定）
                ' 費用/現金: ext_char4から前回情報を取得（簡易実装）
                Dim prevF = If(IsDBNull(row("ext_char4")), "", row("ext_char4").ToString())
                chk_前回_費用_現金_F.Checked = prevF.Contains("1")
                chk_前回_費用_未払_F.Checked = prevF.Contains("2")
                chk_前回_未払_現金_F.Checked = prevF.Contains("3")

                ' 計上日は今日を初期値として設定
                Dim today = DateTime.Today
                txt_計上日_費用_現金.Text = today.ToString("yyyy/MM/dd")
                txt_計上日_費用_現金_曜日.Text = today.ToString("ddd", New CultureInfo("ja-JP"))
                txt_計上日_費用_未払.Text = today.ToString("yyyy/MM/dd")
                txt_計上日_費用_未払_曜日.Text = today.ToString("ddd", New CultureInfo("ja-JP"))
                txt_計上日_未払_現金.Text = today.ToString("yyyy/MM/dd")
                txt_計上日_未払_現金_曜日.Text = today.ToString("ddd", New CultureInfo("ja-JP"))

                txt_税込額_費用_現金.Text = zeikomi.ToString("N0")
                txt_税込額_費用_未払.Text = zeikomi.ToString("N0")
                txt_税込額_未払_現金.Text = zeikomi.ToString("N0")
            End If

            ' 合計
            Dim dtSum = _crud.GetDataTable(
                "SELECT COALESCE(SUM(lsryo + zei), 0) AS zeikomi_sum " &
                "FROM tw_fc_swk_wrk WHERE customer_cd = 'SNKO' AND swk_kbn = '支払仕訳'")
            If dtSum IsNot Nothing AndAlso dtSum.Rows.Count > 0 Then
                Dim s = CType(dtSum.Rows(0)("zeikomi_sum"), Double).ToString("N0")
                txt_税込額_費用_現金_SUM.Text = s
                txt_税込額_費用_未払_SUM.Text = s
                txt_税込額_未払_現金_SUM.Text = s
            End If
        Catch ex As Exception
            MessageBox.Show($"データ読み込みエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form_fc_SNKO_仕訳出力_JOKEN_SUB_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _crud?.Dispose()
    End Sub

End Class
