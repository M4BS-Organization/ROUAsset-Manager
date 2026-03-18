Imports System.Windows.Forms

Partial Public Class Form_f_仕訳出力標準_設定_KJ
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Form_Load: Access版Form_Openに対応。全FLDNMコンボボックスにRowSource初期化。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_KJ_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComboBoxItems()
    End Sub

    ''' <summary>
    ''' 全FLDNMコンボボックスに選択肢を設定する。
    ''' </summary>
    Private Sub InitializeComboBoxItems()
        Dim items() As String = {
            "SKMK1", "SKMK2", "SKMK3", "SKMK4", "SKMK5",
            "SKMK6", "SKMK7", "SKMK8", "SKMK9", "SKMK10",
            "HMK1", "HMK2", "HMK3", "HMK4", "HMK5",
            "HMK6", "HMK7", "HMK8", "HMK9", "HMK10",
            "CONST"
        }

        ' page_1 内の全FLDNMコンボボックス
        For Each ctrl As Control In Me.page_1.Controls
            If TypeOf ctrl Is ComboBox AndAlso ctrl.Name.EndsWith("_FLDNM") Then
                Dim cmb = DirectCast(ctrl, ComboBox)
                cmb.Items.Clear()
                cmb.Items.AddRange(items)
            End If
        Next

        ' page_2 内の全FLDNMコンボボックス
        For Each ctrl As Control In Me.page_2.Controls
            If TypeOf ctrl Is ComboBox AndAlso ctrl.Name.EndsWith("_FLDNM") Then
                Dim cmb = DirectCast(ctrl, ComboBox)
                cmb.Items.Clear()
                cmb.Items.AddRange(items)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 戻るボタン: フォームを閉じる。
    ''' </summary>
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

End Class
