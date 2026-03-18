Imports System.Windows.Forms

' =========================================================
' 契約管理単位 アクセス権選択サブフォーム
' Access版 Form_f_SEC_KNGN_INP_SUB 相当
' =========================================================
Partial Public Class Form_f_SEC_KNGN_INP_SUB
    Inherits Form

    ''' <summary>
    ''' 対象の契約管理単位ID
    ''' </summary>
    Public Property KknriId As Integer = 0

    ''' <summary>
    ''' 表示テキスト（コード - 名称）
    ''' </summary>
    Public Property DisplayText As String = ""

    ''' <summary>
    ''' 選択されたアクセス種別（1=変更, 2=参照）
    ''' </summary>
    Public Property SelectedAccessKind As Integer = 1

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_SEC_KNGN_INP_SUB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 管理単位情報を表示
        txt_KKNRI_ID.Text = KknriId.ToString()
        txt_KKNRI_ID.ReadOnly = True

        If DisplayText.Contains(" - ") Then
            Dim parts = DisplayText.Split(New String() {" - "}, 2, StringSplitOptions.None)
            txt_KKNRI1_CD.Text = parts(0)
            txt_KKNRI1_NM.Text = If(parts.Length > 1, parts(1), "")
        Else
            txt_KKNRI1_CD.Text = DisplayText
            txt_KKNRI1_NM.Text = ""
        End If
        txt_KKNRI1_CD.ReadOnly = True
        txt_KKNRI1_NM.ReadOnly = True

        ' デフォルトは「変更」
        オプション16.Checked = True
    End Sub

    ' OK — 選択して閉じる（ラベルの「変更」「参照」RadioButtonで確定）
    ' Designer.vbにOKボタンがないため、フォーム自体をダイアログとして使用
    ' RadioButton選択後、フォームを閉じる際にDialogResult.OKを返す
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.DialogResult = DialogResult.None Then
            ' ×ボタンで閉じた場合はCancel
            Me.DialogResult = DialogResult.Cancel
        End If
        MyBase.OnFormClosing(e)
    End Sub

    ' フォームのAcceptButton相当：Enterキーで確定
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
