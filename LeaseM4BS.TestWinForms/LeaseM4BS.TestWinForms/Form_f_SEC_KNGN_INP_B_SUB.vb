Imports System.Windows.Forms

' =========================================================
' 部門管理単位 アクセス権選択サブフォーム
' Access版 Form_f_SEC_KNGN_INP_B_SUB 相当
' =========================================================
Partial Public Class Form_f_SEC_KNGN_INP_B_SUB
    Inherits Form

    Public Property BknriId As Integer = 0
    Public Property DisplayText As String = ""
    Public Property SelectedAccessKind As Integer = 1

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_SEC_KNGN_INP_B_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_BKNRI_ID.Text = BknriId.ToString()
        txt_BKNRI_ID.ReadOnly = True

        If DisplayText.Contains(" - ") Then
            Dim parts = DisplayText.Split(New String() {" - "}, 2, StringSplitOptions.None)
            txt_BKNRI1_CD.Text = parts(0)
            txt_BKNRI1_NM.Text = If(parts.Length > 1, parts(1), "")
        Else
            txt_BKNRI1_CD.Text = DisplayText
            txt_BKNRI1_NM.Text = ""
        End If
        txt_BKNRI1_CD.ReadOnly = True
        txt_BKNRI1_NM.ReadOnly = True

        オプション16.Checked = True
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.DialogResult = DialogResult.None Then
            Me.DialogResult = DialogResult.Cancel
        End If
        MyBase.OnFormClosing(e)
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectedAccessKind = If(オプション16.Checked, 1, 2)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub

End Class
