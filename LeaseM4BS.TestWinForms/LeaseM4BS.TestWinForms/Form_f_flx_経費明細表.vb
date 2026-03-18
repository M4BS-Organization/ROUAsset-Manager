Imports System.Windows.Forms

Partial Public Class Form_f_flx_経費明細表
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_経費明細表_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SecurityChecker.ApplyListLimit(Me)
    End Sub

End Class