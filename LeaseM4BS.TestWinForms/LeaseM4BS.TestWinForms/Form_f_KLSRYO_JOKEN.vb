Imports System.Windows.Forms
Imports UtilDate

' --- 期間リース料支払い明細表 ---
Partial Public Class Form_f_KLSRYO_JOKEN
    Inherits Form

    Private _prevForm As Form_f_flx_KLSRYO

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_KLSRYO_JOKEN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    ' [実行]ボタン
    Private Sub cmd_EXECUTE_Click(sender As Object, e As EventArgs) Handles cmd_EXECUTE.Click
        If txt_DATE_FROM.Text = "" OrElse txt_DATE_TO.Text = "" Then
            MessageBox.Show("必須項目が未入力です。")
            Return
        End If

        ' 集計期間の指定を正しくする
        SwapIf(txt_DATE_FROM, txt_DATE_TO)

        If MessageBox.Show("実行してもよろしいですか？", "実行確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        ' 前回フォームが残っていれば解放
        If _prevForm IsNot Nothing Then
            _prevForm.Dispose()
            _prevForm = Nothing
        End If

        Dim frm As New Form_f_flx_KLSRYO
        frm.DtFrom = CDate(txt_DATE_FROM.Value)
        frm.DtTo = CDate(txt_DATE_TO.Value)
        frm.Taisho = GetTaisho()
        frm.Ktmg = GetKtmg()
        frm.Meisai = GetMeisai()
        frm.LabelText = BuildLabelText(frm.DtFrom, frm.DtTo, frm.Taisho, frm.Ktmg, frm.Meisai)
        frm.ShowDialog()

        _prevForm = frm
    End Sub

    ' [キャンセル]ボタン
    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub DATE_ValueChanged(sender As Object, e As EventArgs) Handles txt_DATE_FROM.ValueChanged, txt_DATE_TO.ValueChanged
        ' 期間計算(ヶ月)
        Dim duration As Integer = GetDuration(txt_DATE_FROM.Value, txt_DATE_TO.Value)

        txt_DURATION.Text = If(duration = 0, "", duration.ToString())
    End Sub

    ' [前回集計結果]ボタン
    Private Sub cmd_ZENKAI_Click(sender As Object, e As EventArgs) Handles cmd_ZENKAI.Click
        If _prevForm IsNot Nothing Then
            _prevForm.ShowDialog()
        End If
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub

    Friend Function GetTaisho() As Integer
        Return If(radio_LSRYO.Checked, 1, If(radio_HOSHU.Checked, 2, 3))
    End Function

    Friend Function GetKtmg() As LeaseM4BS.DataAccess.ShriKtmg
        Return If(radio_SIME.Checked, LeaseM4BS.DataAccess.ShriKtmg.SimeDtBase, LeaseM4BS.DataAccess.ShriKtmg.ShriDtBase)
    End Function

    Friend Function GetMeisai() As LeaseM4BS.DataAccess.ShriMeisai
        Return If(radio_BUKN.Checked, LeaseM4BS.DataAccess.ShriMeisai.Kykm, LeaseM4BS.DataAccess.ShriMeisai.Haif)
    End Function

    Private Function BuildLabelText(dtFrom As Date, dtTo As Date, taisho As Integer, ktmg As LeaseM4BS.DataAccess.ShriKtmg, meisai As LeaseM4BS.DataAccess.ShriMeisai) As String
        Dim taishoStr = If(taisho = 1, "リース料", If(taisho = 2, "保守料", "全部"))
        Dim ktmgStr = If(ktmg = LeaseM4BS.DataAccess.ShriKtmg.SimeDtBase, "締日ベース", "支払日ベース")
        Dim meisaiStr = If(meisai = LeaseM4BS.DataAccess.ShriMeisai.Kykm, "物件単位", "配賦単位")
        Return $"集計期間: {dtFrom:yyyy/MM} ～ {dtTo:yyyy/MM} / 対象: {taishoStr} / タイミング: {ktmgStr} / 明細: {meisaiStr}"
    End Function
End Class