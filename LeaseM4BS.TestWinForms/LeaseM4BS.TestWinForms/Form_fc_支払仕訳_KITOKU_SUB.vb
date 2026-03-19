''' <summary>
''' KITOKU支払仕訳出力 消費税率サブフォーム
''' Access版 fc_支払仕訳_KITOKU_SUB 相当
''' 消費税率と税処理コードを入力して親フォームに返す。
''' </summary>
Partial Public Class Form_fc_支払仕訳_KITOKU_SUB
    Inherits Form

    ''' <summary>入力された消費税率</summary>
    Public Property Zritu As Double

    ''' <summary>入力された税処理コード</summary>
    Public Property ZeiCd As String

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_支払仕訳_KITOKU_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_ZRITU.Text = If(Zritu = 0, "", Zritu.ToString())
        txt_ZEI_CD.Text = If(ZeiCd Is Nothing, "", ZeiCd)
    End Sub

    ''' <summary>値を確定して閉じる</summary>
    Public Sub Confirm()
        Dim zrituVal As Double
        If Double.TryParse(txt_ZRITU.Text, zrituVal) Then
            Zritu = zrituVal
        End If
        ZeiCd = txt_ZEI_CD.Text.Trim()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txt_ZEI_CD_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ZEI_CD.KeyDown
        If e.KeyCode = Keys.Return Then
            Confirm()
        End If
    End Sub

    Private Sub txt_ZRITU_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_ZRITU.KeyDown
        If e.KeyCode = Keys.Return Then
            txt_ZEI_CD.Focus()
        End If
    End Sub

End Class
