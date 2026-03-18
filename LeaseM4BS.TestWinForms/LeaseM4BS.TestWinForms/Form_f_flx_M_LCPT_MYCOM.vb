Imports System.Windows.Forms

Partial Public Class Form_f_flx_M_LCPT_MYCOM
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_flx_M_LCPT_MYCOM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SecurityChecker.ApplyMasterUpdateLimit(Me)
    End Sub

End Class