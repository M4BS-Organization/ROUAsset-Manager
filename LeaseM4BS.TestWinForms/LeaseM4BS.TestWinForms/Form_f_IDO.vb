Imports System.Windows.Forms
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

' --- 物件移動処理 ---
' Access版 f_IDO 相当
Partial Public Class Form_f_IDO
    Inherits Form

    Private _crud As New CrudHelper()
    Private _items As New List(Of IdoItem)

    Private Structure IdoItem
        Public KykmId As Double
        Public KykmNo As String
        Public Saikaisu As Integer
        Public BuknBango1 As String
        Public BuknNm As String
        Public StartDt As String
        Public CkaiykDt As String
        Public Klsryo As Double
        Public IsChecked As Boolean
    End Structure

    ' DataGridView で表示するための DataTable
    Private _dgvItems As DataGridView

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_IDO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        オプション416.Checked = True

        ' 移動元・移動先は読み取り専用表示
        txt_BCAT1_NM_From.ReadOnly = True
        txt_BCAT2_NM_From.ReadOnly = True
        txt_BCAT3_NM_From.ReadOnly = True
        txt_BCAT4_NM_From.ReadOnly = True
        txt_BCAT5_NM_From.ReadOnly = True
    End Sub

    ' [移動元の照会] ボタン
    Private Sub cmd_照会_Click(sender As Object, e As EventArgs) Handles cmd_照会.Click
        LoadIdobjects()
    End Sub

    Private Sub LoadIdobjects()
        Try
            Dim bcatCol As String
            If オプション416.Checked Then
                bcatCol = "bcat1_id"
            Else
                bcatCol = "hkbcat1_id"
            End If

            ' 移動元の部署名を取得するため、テキストボックスから部署IDを読み取る
            ' ここではFrom TextBoxにセットされた値をベースに検索
            Dim sql = "SELECT dk.kykm_id, dk.kykm_no, dk.saikaisu, " &
                      "dk.bukn_bango1, dk.bukn_nm, " &
                      "dkh.kishu_dt AS start_dt, dk.ckaiyk_dt, dkh.klsryo " &
                      "FROM d_kykm dk " &
                      "INNER JOIN d_kykh dkh ON dk.kykm_id = dkh.kykm_id " &
                      "WHERE dk." & bcatCol & " IS NOT NULL " &
                      "ORDER BY dk.kykm_no"

            Dim dt = _crud.GetDataTable(sql)

            _items.Clear()
            For Each row As DataRow In dt.Rows
                Dim item As New IdoItem()
                item.KykmId = CDbl(row("kykm_id"))
                item.KykmNo = If(IsDBNull(row("kykm_no")), "", row("kykm_no").ToString())
                item.Saikaisu = If(IsDBNull(row("saikaisu")), 0, CInt(row("saikaisu")))
                item.BuknBango1 = If(IsDBNull(row("bukn_bango1")), "", row("bukn_bango1").ToString())
                item.BuknNm = If(IsDBNull(row("bukn_nm")), "", row("bukn_nm").ToString())
                item.StartDt = If(IsDBNull(row("start_dt")), "", CDate(row("start_dt")).ToString("yyyy/MM/dd"))
                item.CkaiykDt = If(IsDBNull(row("ckaiyk_dt")), "", CDate(row("ckaiyk_dt")).ToString("yyyy/MM/dd"))
                item.Klsryo = If(IsDBNull(row("klsryo")), 0, CDbl(row("klsryo")))
                item.IsChecked = False
                _items.Add(item)
            Next

            MessageBox.Show(_items.Count & " 件の物件が見つかりました。")
        Catch ex As Exception
            MessageBox.Show("照会エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [すべて選択] ボタン
    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        For i = 0 To _items.Count - 1
            Dim item = _items(i)
            item.IsChecked = True
            _items(i) = item
        Next
    End Sub

    ' [すべて選択しない] ボタン
    Private Sub cmd_非選択_Click(sender As Object, e As EventArgs) Handles cmd_非選択.Click
        For i = 0 To _items.Count - 1
            Dim item = _items(i)
            item.IsChecked = False
            _items(i) = item
        Next
    End Sub

    ' [解除] ボタン
    Private Sub cmd_解除_Click(sender As Object, e As EventArgs) Handles cmd_解除.Click
        cmd_非選択_Click(sender, e)
    End Sub

    ' [実行] ボタン — トランザクション内バルク UPDATE（NFR-002）
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        ' バリデーション: 異動日が空欄の場合はエラー
        If String.IsNullOrWhiteSpace(txt_IDO_DT.Text) Then
            MessageBox.Show("移動日を入力してください。", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            txt_IDO_DT.Focus()
            Return
        End If

        Dim checkedItems = _items.Where(Function(x) x.IsChecked).ToList()
        If checkedItems.Count = 0 Then
            MessageBox.Show("移動対象の物件を選択してください。", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim confirm = MessageBox.Show(checkedItems.Count & " 件の物件を異動しますか？", "確認",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.No Then Return

        Try
            _crud.BeginTransaction()

            Dim isBcat = オプション416.Checked
            Dim bcatPrefix = If(isBcat, "bcat", "hkbcat")

            For Each item In checkedItems
                Dim sql = String.Format(
                    "UPDATE d_kykm SET {0}1_id = @to1, {0}2_id = @to2, {0}3_id = @to3, " &
                    "{0}4_id = @to4, {0}5_id = @to5, ido_dt = @ido_dt " &
                    "WHERE kykm_id = @kykm_id", bcatPrefix)

                Dim prms As New List(Of NpgsqlParameter)
                prms.Add(New NpgsqlParameter("@to1", GetBcatToValue(1)))
                prms.Add(New NpgsqlParameter("@to2", GetBcatToValue(2)))
                prms.Add(New NpgsqlParameter("@to3", GetBcatToValue(3)))
                prms.Add(New NpgsqlParameter("@to4", GetBcatToValue(4)))
                prms.Add(New NpgsqlParameter("@to5", GetBcatToValue(5)))
                prms.Add(New NpgsqlParameter("@ido_dt", CDate(txt_IDO_DT.Text)))
                prms.Add(New NpgsqlParameter("@kykm_id", item.KykmId))

                _crud.ExecuteNonQuery(sql, prms)
            Next

            _crud.Commit()
            MessageBox.Show(checkedItems.Count & " 件の異動が完了しました。")
        Catch ex As Exception
            _crud.Rollback()
            MessageBox.Show("異動処理エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GetBcatToValue(index As Integer) As Object
        Dim txt As TextBox = Nothing
        Select Case index
            Case 1 : txt = txt_BCAT1_NM_To
            Case 2 : txt = テキスト471
            Case 3 : txt = テキスト472
            Case 4 : txt = テキスト473
            Case 5 : txt = テキスト474
        End Select

        If txt Is Nothing OrElse String.IsNullOrWhiteSpace(txt.Text) Then
            Return DBNull.Value
        End If

        Dim v As Double
        If Double.TryParse(txt.Text, v) Then Return v
        Return txt.Text
    End Function

    ' [キャンセル] ボタン
    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class