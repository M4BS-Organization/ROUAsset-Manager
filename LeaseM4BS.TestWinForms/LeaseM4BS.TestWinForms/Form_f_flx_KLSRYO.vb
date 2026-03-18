Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

Partial Public Class Form_f_flx_KLSRYO
    Inherits Form

    Private Const FMT_CURRENCY As String = "#,##0"
    Private Const FMT_DATE As String = "yyyy/MM/dd"

    ' Access版 JOKEN から受け取るパラメータ
    Public Property DtFrom As Date
    Public Property DtTo As Date
    Public Property Taisho As Integer = 3                      ' 1:リース料, 2:保守料, 3:全部
    Public Property Ktmg As ShriKtmg = ShriKtmg.SimeDtBase    ' 締日ベース
    Public Property Meisai As ShriMeisai = ShriMeisai.Haif     ' 配賦単位
    Public Property LabelText As String

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_KLSRYO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SearchData()
        SecurityChecker.ApplyListLimit(Me)
    End Sub

    Private Sub SearchData()
        Try
            ' 計算エンジンを呼び出し (Access版 pc_SHRI_KLSRYO.gKLSRYO_Main 相当)
            Dim engine As New KlsryoCalculationEngine()
            Dim dt As System.Data.DataTable = engine.Execute(DtFrom, DtTo, Taisho, Ktmg, Meisai)

            ' 検索条件でフィルタ (数値のみ許可)
            If txt_SEARCH.Text.Trim() <> "" Then
                Dim searchVal = txt_SEARCH.Text.Trim()
                Dim numVal As Integer
                If Integer.TryParse(searchVal, numVal) Then
                    dt.DefaultView.RowFilter = $"物件No = {numVal}"
                    dt = dt.DefaultView.ToTable()
                End If
            End If

            dgv_LIST.Columns.Clear()
            dgv_LIST.AutoGenerateColumns = True
            dgv_LIST.DataSource = dt

            ApplyGridStyle()

        Catch ex As Exception
            MessageBox.Show("一覧取得エラー: " & ex.Message)
        End Try
    End Sub

    ' --- グリッドの見た目調整 ---
    Private Sub ApplyGridStyle()
        dgv_LIST.HideColumns("kykm_id", "kykh_id")

        ' 通貨フォーマット
        Dim currencyCols = {"現金購入価額_物件", "総支払額", "前期末残高", "当期額",
            "期末残高", "期中増加", "内1年内", "内2年内", "内3年内", "内4年内", "内5年内", "5年超"}
        For Each col In currencyCols
            dgv_LIST.FormatColumn(col, FMT_CURRENCY)
        Next

        ' 月別列 G01〜G12
        For i As Integer = 1 To 12
            dgv_LIST.FormatColumn($"G{i:D2}", FMT_CURRENCY)
        Next

        ' 日付フォーマット
        Dim dateCols = {"開始日", "終了日", "中途解約日"}
        For Each col In dateCols
            dgv_LIST.FormatColumn(col, FMT_DATE)
        Next
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' [再計算]ボタン
    Private Sub cmd_RECALCULATE_Click(sender As Object, e As EventArgs) Handles cmd_RECALCULATE.Click
        SearchData()
    End Sub

    ' [ファイル出力]ボタン
    Private Sub cmd_OUTPUT_FILE_Click(sender As Object, e As EventArgs) Handles cmd_OUTPUT_FILE.Click
        Dim frm As New Form_f_FlexOutputDLG
        frm.Dgv = dgv_LIST

        frm.ShowDialog()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class