Imports System.Windows.Forms

Partial Public Class Form_f_flx_M_SWPTN
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_M_SWPTN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SecurityChecker.ApplyMasterUpdateLimit(Me)
    End Sub

End Class