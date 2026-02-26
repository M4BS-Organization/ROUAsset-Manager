Imports LeaseM4BS.DataAccess

' todo Module化(絶対おすすめ)(M7Application参照)
Public Class FormHelper
    Private _crud As New CrudHelper()

    Public Sub BindCombo(cmb As ComboBox, sql As String, display As String, value As String)
        cmb.DisplayMember = display
        cmb.ValueMember = value
        cmb.DataSource = _crud.GetDataTable(sql)
    End Sub

    Public Sub BindCombo(cmb As DataGridViewComboBoxColumn, sql As String, display As String, value As String)
        cmb.DisplayMember = display
        cmb.ValueMember = value
        cmb.DataSource = _crud.GetDataTable(sql)
    End Sub

    Public Sub AdjustComboSize(cmb As ComboBox, Optional width As Integer = 600, Optional height As Integer = 12)
        ' ドロップダウンの全体幅 (各列の合計+スクロールバー分)
        cmb.DropDownWidth = width
        cmb.DrawMode = DrawMode.OwnerDrawVariable
        cmb.ItemHeight = height
    End Sub

    ' =========================================================
    '  コンボボックス選択時の連動 (Accessの =Column(x) 再現)
    ' =========================================================
    Public Sub SyncComboToText(form As Form, cmb As ComboBox, columns As String(), textBoxes As String())
        Dim drv = TryCast(cmb.SelectedItem, DataRowView)

        For i = 0 To columns.Count - 1
            Dim text As String = ""

            If drv IsNot Nothing Then
                text = drv(columns(i)).ToString()
            End If

            ' selectedIndex = -1なら""でクリア
            Dim found = form.Controls.Find(textBoxes(i), True)
            If found.Length > 0 Then
                found(0).Text = text
            End If
        Next
    End Sub

    ' 列を隠す
    Public Sub HideColumns(dgv As DataGridView, ParamArray columnNames As String())
        If dgv Is Nothing Then Return
        If columnNames Is Nothing OrElse columnNames.Length = 0 Then Return

        For Each colName In columnNames
            If dgv.Columns.Contains(colName) Then
                dgv.Columns(colName).Visible = False
            End If
        Next
    End Sub

    ' 列フォーマット用ヘルパー
    Public Sub FormatColumn(dgv As DataGridView, colName As String, fmt As String)
        If dgv.Columns.Contains(colName) Then
            dgv.Columns(colName).DefaultCellStyle.Format = fmt
        End If
    End Sub

    ' 選択行を返す
    Public Function GetSelectedRow(dgv As DataGridView) As DataGridViewRow
        ' 1. 「青く選択されている行」を優先して探す
        If dgv.SelectedRows.Count > 0 Then
            Return dgv.SelectedRows(0)
        End If

        ' 2. なければ「カーソルがある行」を探す
        If dgv.CurrentRow IsNot Nothing Then
            Return dgv.CurrentRow
        End If

        ' 3. それでも取れない、または無効な行（新規行やインデックス異常）ならエラー
        MessageBox.Show("行を選択してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return Nothing
    End Function

    ' =========================================================
    '  コンボボックスの3列描画 (Access完全再現・罫線付き)
    ' =========================================================
    Public Sub Combo_DrawItem(sender As Object, e As DrawItemEventArgs, columns As String())
        Dim cmb As ComboBox = CType(sender, ComboBox)

        e.DrawBackground()

        If e.Index < 0 Then Return

        ' データ行を取り出す (DataRowViewとして取得できる)
        Dim drv As DataRowView = CType(cmb.Items(e.Index), DataRowView)

        ' --- 列幅の設定 (AccessのTwipsを参考にPixel換算) ---
        ' 管理単位: 1419;3969;2268 (約 95px, 265px, 150px)
        ' 支払先  : 1421;3402;1701 (約 95px, 225px, 115px)
        ' w2 = 225 : w3 = 115 ' 支払先用の幅調整
        Dim w As Integer = 115

        ' --- 描画ブラシ設定 ---
        Dim textBrush As Brush = If((e.State And DrawItemState.Selected) = DrawItemState.Selected, SystemBrushes.HighlightText, SystemBrushes.WindowText)
        Dim linePen As Pen = SystemPens.ControlDark ' 区切り線の色

        ' 1列目のX座標
        Dim currentX = e.Bounds.X

        For i = 0 To columns.Length - 1
            Dim column = drv(columns(i)).ToString()

            Dim rect As New Rectangle(currentX, e.Bounds.Y, w, e.Bounds.Height)

            e.Graphics.DrawString(column, e.Font, textBrush, rect)

            ' 間の区切り線
            If i < columns.Length - 1 Then
                e.Graphics.DrawLine(linePen, rect.Right, rect.Top, rect.Right, rect.Bottom) ' 縦線
            End If

            currentX = rect.Right + 2
        Next

        e.DrawFocusRectangle()
    End Sub

    Public Sub SyncDgvScroll(dgvMain As DataGridView, dgvTotal As DataGridView)
        ' 横スクロール位置を同期させる（重要）
        dgvTotal.HorizontalScrollingOffset = dgvMain.HorizontalScrollingOffset
    End Sub

    Public Sub SyncDgvColumnWidths(dgvMain As DataGridView, dgvTotal As DataGridView)
        ' 列幅と可視状態を同期させる
        For i As Integer = 0 To dgvMain.ColumnCount - 1
            dgvTotal.Columns(i).Width = dgvMain.Columns(i).Width
            dgvTotal.Columns(i).Visible = dgvMain.Columns(i).Visible
        Next
    End Sub
End Class