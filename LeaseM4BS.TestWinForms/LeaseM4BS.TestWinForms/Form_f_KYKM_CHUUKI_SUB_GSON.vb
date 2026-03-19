Imports System.Windows.Forms
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 減損注記サブ（編集モード） ---
' Access版 f_KYKM_CHUUKI_SUB_GSON 相当
Partial Public Class Form_f_KYKM_CHUUKI_SUB_GSON
    Inherits Form

    Public Property KykmId As Double

    Private _crud As New CrudHelper()
    Private _rows As New List(Of GsonRow)
    Private _currentRowIndex As Integer = 0

    Public Structure GsonRow
        Public LineId As Integer
        Public GsonDt As String
        Public GsonTmg As Integer
        Public GsonRyo As Double
        Public GsonRkei As Double
    End Structure

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_W_KYKM_ID.Text = KykmId.ToString()
        txt_W_KYKM_ID.ReadOnly = True
        txt_LINE_ID.ReadOnly = True
        txt_GSON_RYO_SUM.ReadOnly = True
        txt_COUNT.ReadOnly = True

        LoadGsonData()
    End Sub

    Public Sub LoadGsonData()
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@kykm_id", KykmId))
            Dim dt = _crud.GetDataTable(
                "SELECT line_id, gson_dt, gson_tmg, gson_ryo, gson_rkei " &
                "FROM d_gson WHERE kykm_id = @kykm_id ORDER BY gson_dt", prms)

            _rows.Clear()
            For Each row As DataRow In dt.Rows
                Dim r As New GsonRow()
                r.LineId = CInt(row("line_id"))
                r.GsonDt = ToDateStr(row("gson_dt"), "yyyy/MM")
                r.GsonTmg = If(IsDBNull(row("gson_tmg")), 0, CInt(row("gson_tmg")))
                r.GsonRyo = If(IsDBNull(row("gson_ryo")), 0, CDbl(row("gson_ryo")))
                r.GsonRkei = If(IsDBNull(row("gson_rkei")), 0, CDbl(row("gson_rkei")))
                _rows.Add(r)
            Next

            If _rows.Count > 0 Then
                _currentRowIndex = 0
                RenderCurrentRow()
            End If
            UpdateTotals()
        Catch ex As Exception
            MessageBox.Show("減損データ取得エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RenderCurrentRow()
        If _currentRowIndex < 0 OrElse _currentRowIndex >= _rows.Count Then Return
        Dim r = _rows(_currentRowIndex)
        txt_LINE_ID.Text = r.LineId.ToString()
        txt_GSON_DT.Text = r.GsonDt
        txt_GSON_RYO.Text = r.GsonRyo.ToString()
        txt_GSON_RKEI.Text = r.GsonRkei.ToString()
    End Sub

    Public Sub UpdateTotals()
        txt_GSON_RYO_SUM.Text = _rows.Sum(Function(r) r.GsonRyo).ToString()
        txt_COUNT.Text = _rows.Count.ToString()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class