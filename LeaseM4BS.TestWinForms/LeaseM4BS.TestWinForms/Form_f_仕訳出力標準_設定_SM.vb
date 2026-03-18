Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

Partial Public Class Form_f_仕訳出力標準_設定_SM
    Inherits Form

    Private _setteiHelper As SetteiHelper

    Public Sub New()
        InitializeComponent()
        _setteiHelper = New SetteiHelper()
    End Sub

    ''' <summary>
    ''' Form_Load: Access版Form_Openに対応。全FLDNMコンボボックスにRowSource初期化後、ワークテーブルからデータをロード。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComboBoxItems()
        LoadDataFromWorkTable()
    End Sub

    ''' <summary>
    ''' 全FLDNMコンボボックスに選択肢を設定する。
    ''' SM画面にはKEIJO_DT_KINDコンボボックスは無い。
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
    ''' ワークテーブル tw_f_仕訳出力標準_設定_swksm の1行目を読み込み、
    ''' 画面上の全コントロール（txt_SWKSM_*, chk_SWKSM_*, cmb_SWKSM_*）に値を設定する。
    ''' コントロール名→カラム名変換: プレフィックス(txt_/chk_/cmb_)を除去して小文字化。
    ''' </summary>
    Private Sub LoadDataFromWorkTable()
        Try
            Dim data As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksm")
            If data.Count = 0 Then Return

            ' page_1, page_2 の全コントロールに値を設定
            For Each page As TabPage In New TabPage() {page_1, page_2}
                For Each ctrl As Control In page.Controls
                    Dim columnName As String = GetColumnNameFromControl(ctrl)
                    If String.IsNullOrEmpty(columnName) Then Continue For
                    If Not data.ContainsKey(columnName) Then Continue For

                    Dim val As Object = data(columnName)

                    If TypeOf ctrl Is TextBox Then
                        DirectCast(ctrl, TextBox).Text = SafeToString(val)

                    ElseIf TypeOf ctrl Is CheckBox Then
                        DirectCast(ctrl, CheckBox).Checked = SafeToBool(val)

                    ElseIf TypeOf ctrl Is ComboBox Then
                        Dim cmb = DirectCast(ctrl, ComboBox)
                        Dim strVal As String = SafeToString(val)
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
            MessageBox.Show($"ワークテーブルからのデータ読み込みに失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 画面上の全コントロールの値をワークテーブル tw_f_仕訳出力標準_設定_swksm に書き戻す。
    ''' FormClosingイベントで呼び出される。
    ''' </summary>
    Private Sub SaveDataToWorkTable()
        Try
            Dim values As New Dictionary(Of String, Object)

            ' page_1, page_2 の全コントロールから値を収集
            For Each page As TabPage In New TabPage() {page_1, page_2}
                For Each ctrl As Control In page.Controls
                    Dim columnName As String = GetColumnNameFromControl(ctrl)
                    If String.IsNullOrEmpty(columnName) Then Continue For

                    If TypeOf ctrl Is TextBox Then
                        values(columnName) = DirectCast(ctrl, TextBox).Text

                    ElseIf TypeOf ctrl Is CheckBox Then
                        values(columnName) = DirectCast(ctrl, CheckBox).Checked

                    ElseIf TypeOf ctrl Is ComboBox Then
                        Dim cmb = DirectCast(ctrl, ComboBox)
                        If cmb.SelectedIndex >= 0 Then
                            values(columnName) = cmb.SelectedItem.ToString()
                        Else
                            values(columnName) = If(String.IsNullOrEmpty(cmb.Text), CObj(DBNull.Value), cmb.Text)
                        End If
                    End If
                Next
            Next

            If values.Count > 0 Then
                _setteiHelper.UpdateWorkTable("tw_f_仕訳出力標準_設定_swksm", values)
            End If

        Catch ex As Exception
            MessageBox.Show($"ワークテーブルへのデータ保存に失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 戻るボタン: フォームを閉じる。
    ''' </summary>
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' FormClosing: フォームを閉じる前にワークテーブルへ書き戻す。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveDataToWorkTable()
    End Sub

    ''' <summary>
    ''' FormClosed: SetteiHelperを破棄する。
    ''' </summary>
    Private Sub Form_f_仕訳出力標準_設定_SM_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _setteiHelper?.Dispose()
    End Sub

    ' ================================================================
    '  ヘルパーメソッド
    ' ================================================================

    ''' <summary>
    ''' コントロール名からワークテーブルのカラム名を取得する。
    ''' txt_SWKSM_XXX → "swksm_xxx", chk_SWKSM_XXX → "swksm_xxx", cmb_SWKSM_XXX → "swksm_xxx"
    ''' 対象外のコントロールはNothingを返す。
    ''' </summary>
    Private Function GetColumnNameFromControl(ctrl As Control) As String
        Dim name As String = ctrl.Name

        If TypeOf ctrl Is TextBox AndAlso name.StartsWith("txt_SWKSM_") Then
            Return name.Substring(4).ToLower()  ' "txt_" を除去

        ElseIf TypeOf ctrl Is CheckBox AndAlso name.StartsWith("chk_SWKSM_") Then
            Return name.Substring(4).ToLower()  ' "chk_" を除去

        ElseIf TypeOf ctrl Is ComboBox AndAlso name.StartsWith("cmb_SWKSM_") Then
            Return name.Substring(4).ToLower()  ' "cmb_" を除去
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' オブジェクトを安全に文字列に変換する（DBNull/Nothing対応）
    ''' </summary>
    Private Function SafeToString(val As Object) As String
        If val Is Nothing OrElse IsDBNull(val) Then Return ""
        Return Convert.ToString(val)
    End Function

    ''' <summary>
    ''' オブジェクトを安全にBooleanに変換する（DBNull/Nothing対応）
    ''' </summary>
    Private Function SafeToBool(val As Object) As Boolean
        If val Is Nothing OrElse IsDBNull(val) Then Return False

        If TypeOf val Is Boolean Then
            Return CBool(val)
        End If

        ' 数値型: 0以外はTrue
        Try
            Return (Convert.ToDouble(val) <> 0)
        Catch
            Return False
        End Try
    End Function

End Class
