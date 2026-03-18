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

            ' 4. バージョン確認 (Access版: gCHK_VERSION_SH/KJ/SM/KY)
            If Not _setteiHelper.CheckVersion("SWKSH_VER", "1.0") OrElse
               Not _setteiHelper.CheckVersion("SWKKJ_VER", "1.0") OrElse
               Not _setteiHelper.CheckVersion("SWKSM_VER", "1.0") OrElse
               Not _setteiHelper.CheckVersion("SWKKY_VER", "1.0") Then
                MessageBox.Show("仕訳出力設定のバージョンが異なるため、処理が実行できません。管理者に連絡してください。",
                                "バージョンエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

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

            ' NOTE: Access版では登録後、SH画面が開いている場合にKEIJO_DT_KINDを更新していた
            '       (gIsLoadedFrm("f_仕訳出力標準_SH") → Form_f_仕訳出力標準_SH.gSET_KEIJO_DT_KIND)
            '       WinForms版ではShowDialog()でモーダル表示するため、MAIN登録時にSH画面は閉じている。
            '       そのため、この処理はWinForms版では不要。

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
        If Not CheckRequiredFields(shData, "月次支払照合フレックス(SH)") Then
            ' Access版: 必須チェック失敗時にSH画面を開く
            Using frm As New Form_f_仕訳出力標準_設定_SH()
                frm.ShowDialog(Me)
            End Using
            Return False
        End If

        ' KJ ワークテーブルチェック
        Dim kjData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkkj")
        If Not CheckRequiredFields(kjData, "月次仕訳計上フレックス(KJ)") Then
            ' Access版: 必須チェック失敗時にKJ画面を開く
            Using frm As New Form_f_仕訳出力標準_設定_KJ()
                frm.ShowDialog(Me)
            End Using
            Return False
        End If

        ' SM ワークテーブルチェック
        Dim smData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksm")
        If Not CheckRequiredFields(smData, "リース債務返済明細表(SM)") Then
            ' Access版: 必須チェック失敗時にSM画面を開く
            Using frm As New Form_f_仕訳出力標準_設定_SM()
                frm.ShowDialog(Me)
            End Using
            Return False
        End If

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
    '  mCHK_組合わせ - Access版の項目間チェック (23パターン)
    '  各パターンに固有メッセージとフォーム遷移を実装
    ' ================================================================

    Private Function mCHK_組合わせ() As Boolean
        Dim shData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksh")
        Dim kjData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swkkj")
        Dim smData As Dictionary(Of String, Object) = _setteiHelper.LoadFromWorkTable("tw_f_仕訳出力標準_設定_swksm")
        Dim msg As String

        ' ==============================================================
        ' SH内チェック（7パターン）: 失敗時→SH画面を開く
        ' ==============================================================

        ' --- SH 資産リース ---
        ' SSN1 && SSN2
        If GetBoolFromWorkTable(shData, "swksh_ssn1_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_ssn2_out_f") Then
            msg = "月次支払照合フレックス　資産リース「1.貸出1(組合)」「2.貸出2」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SSN1 && SSN3
        If GetBoolFromWorkTable(shData, "swksh_ssn1_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_ssn3_out_f") Then
            msg = "月次支払照合フレックス　資産リース「1.貸出1(組合)」「3.貸出3(リース料引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SSN2 && SSN3
        If GetBoolFromWorkTable(shData, "swksh_ssn2_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_ssn3_out_f") Then
            msg = "月次支払照合フレックス　資産リース「2.貸出2」「3.貸出3(リース料引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' --- SH 費用リース ---
        ' HIYO1 && HIYO2
        If GetBoolFromWorkTable(shData, "swksh_hiyo1_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_hiyo2_out_f") Then
            msg = "月次支払照合フレックス　費用リース「1.貸出1」「2.貸出2(リース料引落)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO1 && HIYO3
        If GetBoolFromWorkTable(shData, "swksh_hiyo1_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_hiyo3_out_f") Then
            msg = "月次支払照合フレックス　費用リース「1.貸出1」「3.貸出3(一括引落)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO1 && HIYO4
        If GetBoolFromWorkTable(shData, "swksh_hiyo1_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_hiyo4_out_f") Then
            msg = "月次支払照合フレックス　費用リース「1.貸出1」「4.貸出4(一括引落リース料のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO3 && HIYO4
        If GetBoolFromWorkTable(shData, "swksh_hiyo3_out_f") AndAlso GetBoolFromWorkTable(shData, "swksh_hiyo4_out_f") Then
            msg = "月次支払照合フレックス　費用リース「3.貸出3(一括引落)」「4.貸出4(一括引落リース料のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' ==============================================================
        ' KJ内チェック（1パターン）: 失敗時→KJ画面を開く
        ' ==============================================================

        ' SSN6 && SSN7 && SSN6_KAIYK_OUT_F
        If GetBoolFromWorkTable(kjData, "swkkj_ssn6_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn7_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn6_kaiyk_out_f") Then
            msg = "月次仕訳計上フレックス　資産リース「6.終了(資産) 解約分出力:ON」「7.解約末償(資産)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_KJ()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' ==============================================================
        ' SM内チェック（8パターン）: 失敗時→SM画面を開く
        ' ==============================================================

        ' --- SM 資産リース ---
        ' SSN1 && SSN3
        If GetBoolFromWorkTable(smData, "swksm_ssn1_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_ssn3_out_f") Then
            msg = "リース債務返済明細表　資産リース「1.返済振替」「3.返済振替2(リース料のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SSN1 && SSN5
        If GetBoolFromWorkTable(smData, "swksm_ssn1_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_ssn5_out_f") Then
            msg = "リース債務返済明細表　資産リース「1.返済振替」「5.返済振替3(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SSN2 && SSN4
        If GetBoolFromWorkTable(smData, "swksm_ssn2_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_ssn4_out_f") Then
            msg = "リース債務返済明細表　資産リース「2.返済振替戻し」「4.返済振替戻し2(リース料のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SSN2 && SSN6
        If GetBoolFromWorkTable(smData, "swksm_ssn2_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_ssn6_out_f") Then
            msg = "リース債務返済明細表　資産リース「2.返済振替戻し」「6.返済振替戻し3(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' --- SM 費用リース ---
        ' HIYO1 && HIYO3
        If GetBoolFromWorkTable(smData, "swksm_hiyo1_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_hiyo3_out_f") Then
            msg = "リース債務返済明細表　費用リース「1.返済振替」「3.返済振替2(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO1 && HIYO5
        If GetBoolFromWorkTable(smData, "swksm_hiyo1_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_hiyo5_out_f") Then
            msg = "リース債務返済明細表　費用リース「1.返済振替」「5.返済振替3(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO2 && HIYO4
        If GetBoolFromWorkTable(smData, "swksm_hiyo2_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_hiyo4_out_f") Then
            msg = "リース債務返済明細表　費用リース「2.返済振替戻し」「4.返済振替戻し2(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' HIYO2 && HIYO6
        If GetBoolFromWorkTable(smData, "swksm_hiyo2_out_f") AndAlso GetBoolFromWorkTable(smData, "swksm_hiyo6_out_f") Then
            msg = "リース債務返済明細表　費用リース「2.返済振替戻し」「6.返済振替戻し3(口座引落のみ)」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SM()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' ==============================================================
        ' SH⇔KJクロスチェック（7パターン）: 失敗時→SH画面を開く
        ' ==============================================================

        ' --- 資産リース ---
        ' SH.SSN1 && KJ.SSN4
        If GetBoolFromWorkTable(shData, "swksh_ssn1_out_f") AndAlso GetBoolFromWorkTable(kjData, "swkkj_ssn4_out_f") Then
            msg = "月次支払照合フレックス　資産リース「1.貸出1(組合)」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　資産リース「4.組合」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SH.SSN2 && KJ.SSN4 && KJ.SSN4_KRZEI_OUT_F
        If GetBoolFromWorkTable(shData, "swksh_ssn2_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_krzei_out_f") Then
            msg = "月次支払照合フレックス　資産リース「2.貸出2」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　資産リース「4.組合 繰延リース料出力:ON」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SH.SSN3 && KJ.SSN4 && KJ.SSN4_KRZEI_OUT_F
        If GetBoolFromWorkTable(shData, "swksh_ssn3_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_out_f") AndAlso
           GetBoolFromWorkTable(kjData, "swkkj_ssn4_krzei_out_f") Then
            msg = "月次支払照合フレックス　資産リース「3.貸出3(リース料引落のみ)」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　資産リース「4.組合 繰延リース料出力:ON」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' --- 費用リース (クロスチェック) ---
        ' SH.HIYO1 && KJ.HIYO2
        If GetBoolFromWorkTable(shData, "swksh_hiyo1_out_f") AndAlso GetBoolFromWorkTable(kjData, "swkkj_hiyo2_out_f") Then
            msg = "月次支払照合フレックス　費用リース「1.貸出1」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　費用リース「2.費用計上」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SH.HIYO2 && KJ.HIYO2
        If GetBoolFromWorkTable(shData, "swksh_hiyo2_out_f") AndAlso GetBoolFromWorkTable(kjData, "swkkj_hiyo2_out_f") Then
            msg = "月次支払照合フレックス　費用リース「2.貸出2(リース料引落)」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　費用リース「2.費用計上」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SH.HIYO3 && KJ.HIYO2
        If GetBoolFromWorkTable(shData, "swksh_hiyo3_out_f") AndAlso GetBoolFromWorkTable(kjData, "swkkj_hiyo2_out_f") Then
            msg = "月次支払照合フレックス　費用リース「3.貸出3(一括引落)」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　費用リース「2.費用計上」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        ' SH.HIYO4 && KJ.HIYO2
        If GetBoolFromWorkTable(shData, "swksh_hiyo4_out_f") AndAlso GetBoolFromWorkTable(kjData, "swkkj_hiyo2_out_f") Then
            msg = "月次支払照合フレックス　費用リース「4.貸出4(一括引落リース料のみ)」" & vbCrLf & vbCrLf &
                  "月次仕訳計上フレックス　費用リース「2.費用計上」" & vbCrLf & vbCrLf &
                  "を同時出力すると２重仕訳になる可能性がありますがよろしいですか？"
            If MessageBox.Show(msg, "組合わせ確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                Using frm As New Form_f_仕訳出力標準_設定_SH()
                    frm.tab_設定.SelectedTab = frm.page_2  ' タブを費用に切替
                    frm.ShowDialog(Me)
                End Using
                Return False
            End If
        End If

        Return True
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
