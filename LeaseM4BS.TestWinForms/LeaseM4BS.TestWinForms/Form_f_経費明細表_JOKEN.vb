Imports System.Windows.Forms
Imports UtilDate

' --- 経費明細表 ---
Partial Public Class Form_f_経費明細表_JOKEN
    Inherits Form

    Private _prevForm As Form_f_flx_経費明細表

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_経費明細表_JOKEN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' デフォルト: 全部
        オプション508.Checked = True
    End Sub

    ' [実行]ボタン
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        ' yyyy/mm 形式の入力チェック
        Dim dtFrom As Date
        Dim dtTo As Date

        If Not TryParseYearMonth(txt_KIKAN_FROM.Text, dtFrom) Then
            MessageBox.Show("集計期間(開始)をyyyy/mmの形式で入力してください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_KIKAN_FROM.Focus()
            Return
        End If

        If Not TryParseYearMonth(txt_KIKAN_TO.Text, dtTo) Then
            MessageBox.Show("集計期間(終了)をyyyy/mmの形式で入力してください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_KIKAN_TO.Focus()
            Return
        End If

        ' 日付の順番を正しくする
        If dtFrom > dtTo Then
            Dim tmp = txt_KIKAN_FROM.Text
            txt_KIKAN_FROM.Text = txt_KIKAN_TO.Text
            txt_KIKAN_TO.Text = tmp
            Dim tmpDt = dtFrom
            dtFrom = dtTo
            dtTo = tmpDt
        End If

        If MessageBox.Show("実行してもよろしいですか？", "実行確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        ' 前回フォームが残っていれば解放
        If _prevForm IsNot Nothing Then
            _prevForm.Dispose()
            _prevForm = Nothing
        End If

        Dim frm As New Form_f_flx_経費明細表()
        frm.DtFrom = GetMonthStart(dtFrom)
        frm.DtTo = GetMonthEnd(dtTo)
        frm.Taisho = GetTaisho()
        frm.LabelText = BuildLabelText(frm.DtFrom, frm.DtTo, frm.Taisho)

        frm.ShowDialog()

        _prevForm = frm
    End Sub

    ' [キャンセル]ボタン
    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        If _prevForm IsNot Nothing Then
            _prevForm.Dispose()
            _prevForm = Nothing
        End If
        Me.Close()
    End Sub

    ' [前回集計結果]ボタン
    Private Sub cmd_ZEN_Click(sender As Object, e As EventArgs) Handles cmd_ZEN.Click
        If _prevForm IsNot Nothing Then
            _prevForm.ShowDialog()
        End If
    End Sub

    Private Sub txt_KIKAN_Changed(sender As Object, e As EventArgs) Handles txt_KIKAN_FROM.TextChanged, txt_KIKAN_TO.TextChanged
        ' 期間計算(ヶ月)
        Dim dtFrom As Date
        Dim dtTo As Date

        If TryParseYearMonth(txt_KIKAN_FROM.Text, dtFrom) AndAlso TryParseYearMonth(txt_KIKAN_TO.Text, dtTo) Then
            Dim duration As Integer = GetDuration(dtFrom, dtTo)
            txt_GETU_CNT.Text = If(duration = 0, "", duration.ToString())
        Else
            txt_GETU_CNT.Text = ""
        End If
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub

    ''' <summary>
    ''' 集計対象を取得 (1=リース料, 2=保守料, 3=全部)
    ''' </summary>
    Friend Function GetTaisho() As Integer
        Return If(オプション504.Checked, 1, If(オプション506.Checked, 2, 3))
    End Function

    ''' <summary>
    ''' yyyy/mm または yyyy/MM 形式の文字列をDateにパース
    ''' </summary>
    Private Shared Function TryParseYearMonth(text As String, ByRef result As Date) As Boolean
        If String.IsNullOrWhiteSpace(text) Then Return False

        Dim parts = text.Trim().Split("/"c)
        If parts.Length <> 2 Then Return False

        Dim year As Integer
        Dim month As Integer

        If Not Integer.TryParse(parts(0), year) Then Return False
        If Not Integer.TryParse(parts(1), month) Then Return False

        If year < 1 OrElse year > 9999 OrElse month < 1 OrElse month > 12 Then Return False

        result = New Date(year, month, 1)
        Return True
    End Function

    Private Function BuildLabelText(dtFrom As Date, dtTo As Date, taisho As Integer) As String
        Dim taishoStr = If(taisho = 1, "リース料", If(taisho = 2, "保守料", "全部"))
        Return $"集計期間: {dtFrom:yyyy/MM} ～ {dtTo:yyyy/MM} / 対象: {taishoStr}"
    End Function
End Class
