Imports System.Windows.Forms
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 減損注記サブ（参照モード） ---
' Access版 f_REF_D_KYKM_CHUUKI_SUB_GSON 相当
' Form_f_KYKM_CHUUKI_SUB_GSON と同じデータを表示するが全フィールド読み取り専用
Partial Public Class Form_f_REF_D_KYKM_CHUUKI_SUB_GSON
    Inherits Form

    Public Property KykmId As Double

    Private _crud As New CrudHelper()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 全コントロールを読み取り専用にする
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).ReadOnly = True
            End If
        Next

        ' データ読み込み（Form_f_KYKM_CHUUKI_SUB_GSON と同じロジック）
        LoadGsonData()
    End Sub

    Private Sub LoadGsonData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            Dim dt = _crud.GetDataTable(
                "SELECT line_id, gson_dt, gson_tmg, gson_ryo, gson_rkei " &
                "FROM d_gson WHERE kykm_id = @kykm_id ORDER BY gson_dt", prms)

            If dt.Rows.Count > 0 Then
                Dim row = dt.Rows(0)
                txt_LINE_ID.Text = If(IsDBNull(row("line_id")), "", row("line_id").ToString())
                txt_GSON_DT.Text = If(IsDBNull(row("gson_dt")), "", CDate(row("gson_dt")).ToString("yyyy/MM"))
                txt_GSON_RYO.Text = If(IsDBNull(row("gson_ryo")), "", row("gson_ryo").ToString())
                txt_GSON_RKEI.Text = If(IsDBNull(row("gson_rkei")), "", row("gson_rkei").ToString())
            End If

            ' 合計
            Dim total As Double = 0
            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("gson_ryo")) Then total += CDbl(row("gson_ryo"))
            Next
            txt_GSON_RYO_SUM.Text = total.ToString()
            txt_COUNT.Text = dt.Rows.Count.ToString()
            txt_W_KYKM_ID.Text = KykmId.ToString()
        Catch ex As Exception
            MessageBox.Show("減損データ取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class