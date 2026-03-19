Imports System.Windows.Forms

''' <summary>
''' MARUZEN計上仕訳 税率設定サブフォーム
''' 税率(ZRITU)と税区分(ZEI_KBN)を設定するダイアログ
''' </summary>
Partial Public Class Form_fc_計上仕訳_MARUZEN_SUB
    Inherits Form

    ''' <summary>税率（例: 0.1 = 10%）</summary>
    Public Property Zritu As Decimal = 0.1D

    ''' <summary>税区分コード</summary>
    Public Property ZeiKbn As String = "10"

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_fc_計上仕訳_MARUZEN_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_ZRITU.Text = Zritu.ToString("0.##")
        txt_ZEI_KBN.Text = ZeiKbn
    End Sub

End Class