Imports System.Windows.Forms

Partial Public Class Form_f_flx_D_KYKM
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_D_KYKM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SecurityChecker.ApplyDataUpdateLimit(Me)
    End Sub

End Class