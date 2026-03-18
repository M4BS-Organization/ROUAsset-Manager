Imports System.Windows.Forms
Imports System.Collections.Generic
Imports LeaseM4BS.DataAccess

Partial Public Class Form_f_仕訳出力標準_設定_MAIN
    Inherits Form

    Private _setteiHelper As SetteiHelper

    Public Sub New()
        InitializeComponent()
        _setteiHelper = New SetteiHelper()
    End Sub

    ' ================================================================
    '  Form_Load - Access版 Form_Open 相当
    ' ================================================================

    Private Sub Form_f_仕訳出力標準_設定_MAIN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' 1. T_SETTEI → ワークテーブル展開
            _setteiHelper.LoadSettingsToWorkTables()

            ' 2. ワークテーブルが空(=T_SETTEIに設定なし)の場合、デフォルト値登録して再ロード
            Dim kyData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkky")
            If kyData.Count = 0 OrElse (kyData.Count = 1 AndAlso kyData.ContainsKey("id")) Then
                _setteiHelper.InitializeDefaultSettings()
                _setteiHelper.LoadSettingsToWorkTables()
                kyData = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkky")
            End If

            ' 3. KYワークテーブルからチェックボックスの値を読込・設定
            chk_SWKKY_KMKNM_HOKAN.Checked = GetBoolFromWorkTable(kyData, "swkky_kmknm_hokan")
            chk_SWKKY_DC_BETU_F.Checked = GetBoolFromWorkTable(kyData, "swkky_dc_betu_f")

        Catch ex As Exception
            MessageBox.Show($"設定の読み込みに失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  ボタン: SH/KJ/SM 設定画面を開く
    ' ================================================================

    Private Sub cmd_SWKSH_Click(sender As Object, e As EventArgs) Handles cmd_SWKSH.Click
        Using frm As New Form_f_仕訳出力標準_設定_SH()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub cmd_SWKKJ_Click(sender As Object, e As EventArgs) Handles cmd_SWKKJ.Click
        Using frm As New Form_f_仕訳出力標準_設定_KJ()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub cmd_SWKSM_Click(sender As Object, e As EventArgs) Handles cmd_SWKSM.Click
        Using frm As New Form_f_仕訳出力標準_設定_SM()
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' ================================================================
    '  ボタン: 登録
    ' ================================================================

    Private Sub cmd_TOUROKU_Click(sender As Object, e As EventArgs) Handles cmd_TOUROKU.Click
        Try
            ' 1. KYワークテーブル更新: チェックボックスの値を書き戻し
            Dim kyValues As New Dictionary(Of String, Object) From {
                {"swkky_kmknm_hokan", chk_SWKKY_KMKNM_HOKAN.Checked},
                {"swkky_dc_betu_f", chk_SWKKY_DC_BETU_F.Checked}
            }
            _setteiHelper.UpdateWorkTable("tw_f_仕訳出力標準_設定_swkky", kyValues)

            ' 2. 必須フィールドチェック: SH/KJ/SMの各ワークテーブルの_FLDNM/_CNSTNMフィールドを確認
            If Not mCHK_必須フィールド() Then
                Return
            End If

            ' 3. 組合わせチェック
            If Not mCHK_組合わせ() Then
                Return
            End If

            ' 4. 登録確認
            Dim result As DialogResult = MessageBox.Show("登録してよろしいですか？",
                                                         "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result <> DialogResult.Yes Then
                Return
            End If

            ' 5. DB登録
            _setteiHelper.SaveWorkTablesToTSettei()

            ' 6. フォームクローズ
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"登録処理に失敗しました。{vbCrLf}{ex.Message}",
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  ボタン: 閉じる
    ' ================================================================

    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' ================================================================
    '  FormClosed - IDisposable対応
    ' ================================================================

    Private Sub Form_f_仕訳出力標準_設定_MAIN_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _setteiHelper?.Dispose()
    End Sub

    ' ================================================================
    '  mCHK_必須フィールド - SH/KJ/SMの_FLDNM/_CNSTNMチェック
    ' ================================================================

    Private Function mCHK_必須フィールド() As Boolean
        ' SH ワークテーブルチェック
        Dim shData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksh")
        If Not CheckRequiredFields(shData, "月次支払照合フレックス(SH)") Then Return False

        ' KJ ワークテーブルチェック
        Dim kjData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkkj")
        If Not CheckRequiredFields(kjData, "月次仕訳計上フレックス(KJ)") Then Return False

        ' SM ワークテーブルチェック
        Dim smData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksm")
        If Not CheckRequiredFields(smData, "リース債務返済明細表(SM)") Then Return False

        Return True
    End Function

    ''' <summary>
    ''' ワークテーブルの_FLDNM/_CNSTNMフィールドの必須チェック。
    ''' OUT_Fがtrueのレコードで、対応する_FLDNMまたは_CNSTNMが空の場合に警告。
    ''' </summary>
    Private Function CheckRequiredFields(data As Dictionary(Of String, Object), sectionName As String) As Boolean
        If data Is Nothing OrElse data.Count = 0 Then Return True

        For Each kvp In data
            Dim key As String = kvp.Key.ToLower()

            ' _out_f が True のフィールドを探す
            If Not key.EndsWith("_out_f") Then Continue For
            If Not GetBoolFromWorkTable(data, key) Then Continue For

            ' 対応する _fldnm / _cnstnm を確認
            Dim baseKey As String = key.Replace("_out_f", "")
            Dim fldnmKey As String = baseKey & "_fldnm"
            Dim cnstnmKey As String = baseKey & "_cnstnm"

            Dim hasFldnm As Boolean = data.ContainsKey(fldnmKey) AndAlso
                                       Not IsNullOrEmpty(data(fldnmKey))
            Dim hasCnstnm As Boolean = data.ContainsKey(cnstnmKey) AndAlso
                                        Not IsNullOrEmpty(data(cnstnmKey))

            If Not hasFldnm AndAlso Not hasCnstnm Then
                ' _fldnm も _cnstnm も両方空の場合は警告（出力フラグONなのにフィールド未設定）
                MessageBox.Show($"{sectionName}の出力設定で、フィールド名または定数名が未設定の項目があります。",
                                "入力チェック", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        Next

        Return True
    End Function

    ' ================================================================
    '  mCHK_組合わせ - Access版の組合わせ検証 (20パターン)
    ' ================================================================

    Private Function mCHK_組合わせ() As Boolean
        Dim shData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksh")
        Dim kjData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkkj")
        Dim smData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksm")

        ' --- SH内チェック（7パターン）---
        If Not CheckCombination(shData, "swksh_ssn1_out_f", shData, "swksh_ssn2_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_ssn1_out_f", shData, "swksh_ssn3_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_ssn2_out_f", shData, "swksh_ssn3_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo1_out_f", shData, "swksh_hiyo2_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo1_out_f", shData, "swksh_hiyo3_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo1_out_f", shData, "swksh_hiyo4_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo3_out_f", shData, "swksh_hiyo4_out_f") Then Return False

        ' --- KJ内チェック（1パターン: 3つ全てONの場合）---
        If GetBoolFromWorkTable(kjData, "swkkj_ssn6_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn7_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn6_kaiyk_out_f") Then
            If Not ShowCombinationWarning() Then Return False
        End If

        ' --- SM内チェック（8パターン）---
        If Not CheckCombination(smData, "swksm_ssn1_out_f", smData, "swksm_ssn3_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_ssn1_out_f", smData, "swksm_ssn5_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_ssn2_out_f", smData, "swksm_ssn4_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_ssn2_out_f", smData, "swksm_ssn6_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_hiyo1_out_f", smData, "swksm_hiyo3_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_hiyo1_out_f", smData, "swksm_hiyo5_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_hiyo2_out_f", smData, "swksm_hiyo4_out_f") Then Return False
        If Not CheckCombination(smData, "swksm_hiyo2_out_f", smData, "swksm_hiyo6_out_f") Then Return False

        ' --- SH⇔KJクロスチェック（7パターン）---
        If Not CheckCombination(shData, "swksh_ssn1_out_f", kjData, "swkkj_ssn4_out_f") Then Return False

        ' SH.SSN2 && KJ.SSN4 && KJ.SSN4_KRZEI
        If GetBoolFromWorkTable(shData, "swksh_ssn2_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_krzei_out_f") Then
            If Not ShowCombinationWarning() Then Return False
        End If

        ' SH.SSN3 && KJ.SSN4 && KJ.SSN4_KRZEI
        If GetBoolFromWorkTable(shData, "swksh_ssn3_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_krzei_out_f") Then
            If Not ShowCombinationWarning() Then Return False
        End If

        If Not CheckCombination(shData, "swksh_hiyo1_out_f", kjData, "swkkj_hiyo2_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo2_out_f", kjData, "swkkj_hiyo2_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo3_out_f", kjData, "swkkj_hiyo2_out_f") Then Return False
        If Not CheckCombination(shData, "swksh_hiyo4_out_f", kjData, "swkkj_hiyo2_out_f") Then Return False

        Return True
    End Function

    ''' <summary>
    ''' 2つのフラグが両方ONの場合に二重仕訳警告を表示する。
    ''' ユーザーがNoを選んだ場合Falseを返す。
    ''' </summary>
    Private Function CheckCombination(data1 As Dictionary(Of String, Object), key1 As String,
                                       data2 As Dictionary(Of String, Object), key2 As String) As Boolean
        If GetBoolFromWorkTable(data1, key1) AndAlso GetBoolFromWorkTable(data2, key2) Then
            Return ShowCombinationWarning()
        End If
        Return True
    End Function

    ''' <summary>
    ''' 仕訳二重警告を表示する。Yesで続行(True)、Noで中断(False)。
    ''' </summary>
    Private Function ShowCombinationWarning() As Boolean
        Dim result As DialogResult = MessageBox.Show(
            "仕訳二重になる可能性があります。よろしいですか？",
            "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        Return (result = DialogResult.Yes)
    End Function

    ' ================================================================
    '  ヘルパーメソッド
    ' ================================================================

    ''' <summary>
    ''' DictionaryからBooleanを安全に取得する（DBNull/Nothing対応）
    ''' </summary>
    Private Function GetBoolFromWorkTable(dict As Dictionary(Of String, Object), key As String) As Boolean
        If dict Is Nothing OrElse Not dict.ContainsKey(key) Then
            Return False
        End If

        Dim val As Object = dict(key)

        If val Is Nothing OrElse IsDBNull(val) Then
            Return False
        End If

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

    ''' <summary>
    ''' オブジェクトがNull/DBNull/空文字列かどうかを判定する
    ''' </summary>
    Private Function IsNullOrEmpty(val As Object) As Boolean
        If val Is Nothing OrElse IsDBNull(val) Then Return True
        Return String.IsNullOrWhiteSpace(Convert.ToString(val))
    End Function

End Class
