Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

Partial Public Class Form_f_仕訳出力標準_設定_SH
    Inherits Form

    Private _setteiHelper As SetteiHelper

    Public Sub New()
        InitializeComponent()
        _setteiHelper = New SetteiHelper()
    End Sub

    ''' <summary>
    ''' Form_Load: Access版Form_Openに対応。全FLDNMコンボボックスにRowSource初期化後、ワークテーブルからデータをロード。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SH_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComboBoxItems()
        LoadDataFromWorkTable()
    End Sub

    ''' <summary>
    ''' 全FLDNMコンボボックスに選択肢を設定する。
    ''' cmb_SWKSH_KEIJO_DT_KINDはDesigner.vbで別途設定済みのためスキップ。
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
                ' cmb_SWKSH_KEIJO_DT_KIND は対象外（Designer.vbで初期化済み）
                If ctrl.Name <> "cmb_SWKSH_KEIJO_DT_KIND" Then
                    Dim cmb = DirectCast(ctrl, ComboBox)
                    cmb.Items.Clear()
                    cmb.Items.AddRange(items)
                End If
            End If
        Next

        ' page_2 内の全FLDNMコンボボックス
        For Each ctrl As Control In Me.page_2.Controls
            If TypeOf ctrl Is ComboBox AndAlso ctrl.Name.EndsWith("_FLDNM") Then
                If ctrl.Name <> "cmb_SWKSH_KEIJO_DT_KIND" Then
                    Dim cmb = DirectCast(ctrl, ComboBox)
                    cmb.Items.Clear()
                    cmb.Items.AddRange(items)
                End If
            End If
        Next
    End Sub

    ' ================================================================
    '  ワークテーブル → コントロール ロード処理
    ' ================================================================

    ''' <summary>
    ''' ワークテーブル tw_f_仕訳出力標準_設定_swksh から1行読み込み、各コントロールに値を設定する。
    ''' Access版 RecordSource="tw_f_仕訳出力標準_設定_SWKSH" に対応。
    ''' </summary>
    Private Sub LoadDataFromWorkTable()
        Dim data = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksh")
        If data Is Nothing OrElse data.Count = 0 Then Return

        ' 全TabPage内コントロールをループ
        For Each page As TabPage In tab_設定.TabPages
            For Each ctrl As Control In page.Controls
                Dim columnName = ctrl.Name.Replace("txt_", "").Replace("chk_", "").Replace("cmb_", "").ToLower()
                If Not data.ContainsKey(columnName) Then Continue For

                If TypeOf ctrl Is TextBox Then
                    DirectCast(ctrl, TextBox).Text = If(data(columnName) IsNot Nothing AndAlso Not IsDBNull(data(columnName)),
                                                        Convert.ToString(data(columnName)), "")
                ElseIf TypeOf ctrl Is CheckBox Then
                    DirectCast(ctrl, CheckBox).Checked = If(data(columnName) IsNot Nothing AndAlso Not IsDBNull(data(columnName)),
                                                            Convert.ToBoolean(data(columnName)), False)
                ElseIf TypeOf ctrl Is ComboBox Then
                    Dim cmb = DirectCast(ctrl, ComboBox)
                    Dim val = If(data(columnName) IsNot Nothing AndAlso Not IsDBNull(data(columnName)),
                               Convert.ToString(data(columnName)), "")
                    If cmb.Items.Contains(val) Then
                        cmb.SelectedItem = val
                    ElseIf val <> "" Then
                        cmb.Text = val
                    End If
                End If
            Next
        Next

        ' KEIJO_DT_KINDは特別処理（integer→SelectedIndex）
        If data.ContainsKey("swksh_keijo_dt_kind") Then
            Dim kind = If(data("swksh_keijo_dt_kind") IsNot Nothing AndAlso Not IsDBNull(data("swksh_keijo_dt_kind")),
                         Convert.ToInt32(data("swksh_keijo_dt_kind")), 1)
            If kind >= 1 AndAlso kind <= cmb_SWKSH_KEIJO_DT_KIND.Items.Count Then
                cmb_SWKSH_KEIJO_DT_KIND.SelectedIndex = kind - 1
            End If
        End If
    End Sub

    ' ================================================================
    '  コントロール → ワークテーブル 保存処理
    ' ================================================================

    ''' <summary>
    ''' 各コントロールの値をワークテーブル tw_f_仕訳出力標準_設定_swksh に書き戻す。
    ''' Access版フォームクローズ時の自動バインディング書き戻しに対応。
    ''' </summary>
    Private Sub SaveDataToWorkTable()
        Dim data As New Dictionary(Of String, Object)

        For Each page As TabPage In tab_設定.TabPages
            For Each ctrl As Control In page.Controls
                Dim columnName = ctrl.Name.Replace("txt_", "").Replace("chk_", "").Replace("cmb_", "").ToLower()

                If TypeOf ctrl Is TextBox Then
                    data(columnName) = DirectCast(ctrl, TextBox).Text
                ElseIf TypeOf ctrl Is CheckBox Then
                    data(columnName) = DirectCast(ctrl, CheckBox).Checked
                ElseIf TypeOf ctrl Is ComboBox Then
                    Dim cmb = DirectCast(ctrl, ComboBox)
                    If cmb.Name = "cmb_SWKSH_KEIJO_DT_KIND" Then
                        ' KEIJO_DT_KINDはinteger保存（SelectedIndex+1）
                        data("swksh_keijo_dt_kind") = cmb.SelectedIndex + 1
                    Else
                        data(columnName) = If(cmb.SelectedItem IsNot Nothing, cmb.SelectedItem.ToString(), cmb.Text)
                    End If
                End If
            Next
        Next

        _setteiHelper.UpdateWorkTable("tw_f_仕訳出力標準_設定_swksh", data)
    End Sub

    ' ================================================================
    '  フォームクローズ / Dispose
    ' ================================================================

    ''' <summary>
    ''' FormClosing: フォームを閉じる前にワークテーブルへ保存する。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SH_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            SaveDataToWorkTable()
        Catch ex As Exception
            ' 保存失敗時はエラーを表示するが、フォームは閉じる
            MessageBox.Show($"設定の保存中にエラーが発生しました: {ex.Message}",
                           "保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' FormClosed: SetteiHelperを破棄する。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SH_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _setteiHelper?.Dispose()
    End Sub

    ''' <summary>
    ''' 戻るボタン: フォームを閉じる。
    ''' </summary>
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

End Class
