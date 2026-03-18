Imports System.Windows.Forms
Imports System.Collections.Generic
Imports LeaseM4BS.DataAccess

Partial Public Class Form_f_仕訳出力標準_設定_KJ
    Inherits Form

    Private _setteiHelper As SetteiHelper

    Private Const WORK_TABLE As String = "tw_f_仕訳出力標準_設定_swkkj"

    Public Sub New()
        InitializeComponent()
        _setteiHelper = New SetteiHelper()
    End Sub

    ''' <summary>
    ''' Form_Load: Access版Form_Openに対応。全FLDNMコンボボックスにRowSource初期化後、ワークテーブルからデータをロード。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_KJ_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComboBoxItems()
        LoadDataFromWorkTable()
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

    ' ================================================================
    '  LoadDataFromWorkTable - ワークテーブルから全コントロールへ値をロード
    ' ================================================================

    ''' <summary>
    ''' SetteiHelper.LoadFromWorkTable でワークテーブルの1行目をDictionary取得し、
    ''' 各コントロール（txt_/chk_/cmb_）に値を設定する。
    ''' </summary>
    Private Sub LoadDataFromWorkTable()
        Try
            Dim data As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable(WORK_TABLE)
            If data.Count = 0 Then Return

            ' page_1, page_2 の全コントロールを走査
            For Each page As TabPage In New TabPage() {Me.page_1, Me.page_2}
                For Each ctrl As Control In page.Controls
                    Dim columnName As String = ControlNameToColumnName(ctrl)
                    If columnName Is Nothing Then Continue For
                    If Not data.ContainsKey(columnName) Then Continue For

                    Dim val As Object = data(columnName)

                    If TypeOf ctrl Is TextBox Then
                        DirectCast(ctrl, TextBox).Text = SafeToString(val)

                    ElseIf TypeOf ctrl Is CheckBox Then
                        DirectCast(ctrl, CheckBox).Checked = SafeToBool(val)

                    ElseIf TypeOf ctrl Is ComboBox Then
                        Dim strVal As String = SafeToString(val)
                        Dim cmb = DirectCast(ctrl, ComboBox)
                        If String.IsNullOrEmpty(strVal) Then
                            cmb.SelectedIndex = -1
                        Else
                            Dim idx As Integer = cmb.Items.IndexOf(strVal)
                            If idx >= 0 Then
                                cmb.SelectedIndex = idx
                            Else
                                cmb.Text = strVal
                            End If
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            MessageBox.Show($"ワークテーブルからのデータ読込に失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  SaveDataToWorkTable - 全コントロールの値をワークテーブルへ書き戻し
    ' ================================================================

    ''' <summary>
    ''' 全コントロール（txt_/chk_/cmb_）の値をDictionaryに集約し、
    ''' SetteiHelper.UpdateWorkTable でワークテーブルを更新する。
    ''' </summary>
    Private Sub SaveDataToWorkTable()
        Try
            Dim values As New Dictionary(Of String, Object)

            ' page_1, page_2 の全コントロールを走査
            For Each page As TabPage In New TabPage() {Me.page_1, Me.page_2}
                For Each ctrl As Control In page.Controls
                    Dim columnName As String = ControlNameToColumnName(ctrl)
                    If columnName Is Nothing Then Continue For

                    If TypeOf ctrl Is TextBox Then
                        values(columnName) = DirectCast(ctrl, TextBox).Text

                    ElseIf TypeOf ctrl Is CheckBox Then
                        values(columnName) = DirectCast(ctrl, CheckBox).Checked

                    ElseIf TypeOf ctrl Is ComboBox Then
                        Dim cmb = DirectCast(ctrl, ComboBox)
                        If cmb.SelectedIndex >= 0 Then
                            values(columnName) = cmb.SelectedItem.ToString()
                        Else
                            values(columnName) = If(String.IsNullOrEmpty(cmb.Text), "", cmb.Text)
                        End If
                    End If
                Next
            Next

            If values.Count > 0 Then
                _setteiHelper.UpdateWorkTable(WORK_TABLE, values)
            End If

        Catch ex As Exception
            MessageBox.Show($"ワークテーブルへのデータ保存に失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  FormClosing - フォームを閉じる前にワークテーブルへ保存
    ' ================================================================

    Private Sub Form_f_仕訳出力標準_設定_KJ_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveDataToWorkTable()
    End Sub

    ' ================================================================
    '  FormClosed - IDisposable対応
    ' ================================================================

    Private Sub Form_f_仕訳出力標準_設定_KJ_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _setteiHelper?.Dispose()
    End Sub

    ''' <summary>
    ''' 戻るボタン: フォームを閉じる。
    ''' </summary>
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' ================================================================
    '  ヘルパーメソッド
    ' ================================================================

    ''' <summary>
    ''' コントロール名からワークテーブルのカラム名を導出する。
    ''' txt_SWKKJ_SSN1_1D1_CNSTCD → "swkkj_ssn1_1d1_cnstcd"
    ''' chk_SWKKJ_SSN1_OUT_F → "swkkj_ssn1_out_f"
    ''' cmb_SWKKJ_SSN1_1D1_FLDNM → "swkkj_ssn1_1d1_fldnm"
    ''' 対象外コントロールの場合はNothingを返す。
    ''' </summary>
    Private Function ControlNameToColumnName(ctrl As Control) As String
        Dim name As String = ctrl.Name

        If TypeOf ctrl Is TextBox AndAlso name.StartsWith("txt_SWKKJ_") Then
            Return name.Substring(4).ToLower()  ' "txt_" を除去
        ElseIf TypeOf ctrl Is CheckBox AndAlso name.StartsWith("chk_SWKKJ_") Then
            Return name.Substring(4).ToLower()  ' "chk_" を除去
        ElseIf TypeOf ctrl Is ComboBox AndAlso name.StartsWith("cmb_SWKKJ_") Then
            Return name.Substring(4).ToLower()  ' "cmb_" を除去
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' DBNull/Nothing対応の文字列変換
    ''' </summary>
    Private Function SafeToString(val As Object) As String
        If val Is Nothing OrElse IsDBNull(val) Then Return ""
        Return Convert.ToString(val)
    End Function

    ''' <summary>
    ''' DBNull/Nothing対応のBoolean変換
    ''' </summary>
    Private Function SafeToBool(val As Object) As Boolean
        If val Is Nothing OrElse IsDBNull(val) Then Return False
        If TypeOf val Is Boolean Then Return CBool(val)
        Try
            Return (Convert.ToDouble(val) <> 0)
        Catch
            Return False
        End Try
    End Function

End Class
