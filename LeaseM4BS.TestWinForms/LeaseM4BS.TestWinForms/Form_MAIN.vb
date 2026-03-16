Imports LeaseM4BS.DataAccess

Public Class Form_MAIN

    ' =========================================================
    ' フォームロード - ユーザー情報表示・権限制御・タイトル設定
    ' =========================================================
    Private Sub Form_MAIN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ステータスバーにログインユーザー情報を表示
        tsslUserInfo.Text = "ログイン者: " & LoginSession.LoggedInUserNm &
                            "（権限: " & LoginSession.KngnNm & "）"

        ' タイトルバーにDB環境名を表示（Access版 Form_f_LOGIN_JET:289-298 再現）
        SetTitleWithDbName()

        ' 権限制御を適用（Access版 mMENUBAR_ENABLE_CTL 再現）
        ApplyMenuPermissions()
    End Sub

    ' =========================================================
    ' タイトルバーにDB環境名を表示
    ' Access版(Form_f_LOGIN_JET.txt:289-298)の再現
    ' =========================================================
    Private Sub SetTitleWithDbName()
        Try
            Dim connMgr As New DbConnectionManager()
            ' GetMaskedConnectionString は "Host=xxx;Port=xxx;Database=xxx" 形式を返す
            Dim maskedStr As String = connMgr.GetMaskedConnectionString()
            Dim dbName As String = ""
            For Each part As String In maskedStr.Split(";"c)
                Dim trimmed As String = part.Trim()
                If trimmed.StartsWith("Database=", StringComparison.OrdinalIgnoreCase) Then
                    dbName = trimmed.Substring("Database=".Length)
                    Exit For
                End If
            Next
            If Not String.IsNullOrEmpty(dbName) Then
                Me.Text = "LeaseM4BS - DB[" & dbName & "]"
            Else
                Me.Text = "LeaseM4BS"
            End If
        Catch ex As Exception
            Me.Text = "LeaseM4BS"
        End Try
    End Sub

    ' =========================================================
    ' メニュー権限制御（Access版 mMENUBAR_ENABLE_CTL + p_menu2 権限チェック再現）
    ' LoginSessionの全権限フラグに基づいてメニューの有効/無効を制御する
    ' =========================================================
    Private Sub ApplyMenuPermissions()
        ' --- まず全メニューを有効化（リセット） ---
        ResetAllMenuEnabled()

        ' ---------------------------------------------------------
        ' 1. IsAdmin=False → 一括更新タブ無効化
        '    Access版: Case 201,202 (ユーザ管理) は boAdmin 必須
        '    Access版: Case 401,402,422,602 (DB管理系) は boAdmin 必須
        ' ---------------------------------------------------------
        If LoginSession.IsAdmin = False Then
            一括更新ToolStripMenuItem.Enabled = False
        End If

        ' ---------------------------------------------------------
        ' 2. CanMasterUpdate=False → マスタタブ無効化
        '    Access版: mMENUBAR_ENABLE_CTL の boPriKNGN チェック
        '    （マスタメニュー表示/非表示はマスタ更新権限に連動）
        ' ---------------------------------------------------------
        If LoginSession.CanMasterUpdate = False Then
            マスタToolStripMenuItem.Enabled = False
        End If

        ' ---------------------------------------------------------
        ' 3. CanFileOutput=False → ファイル出力関連メニュー無効化
        '    台帳タブの各フレックスは表示するが、出力機能を持つ
        '    期間タブ・決算タブの帳票出力系を無効化
        ' ---------------------------------------------------------
        If LoginSession.CanFileOutput = False Then
            ' 期間タブ（帳票出力系）
            menu_TANA_JOKEN.Enabled = False          ' 棚卸明細表
            menu_KLSRYO_JOKEN.Enabled = False        ' 期間リース料支払明細表
            menu_IDOLST_JOKEN.Enabled = False        ' 移動物件一覧表
            menu_KHIYO_JOKEN.Enabled = False         ' 期間費用計上明細表
            menu_YOSAN_JOKEN.Enabled = False         ' 予算実績集計

            ' 決算タブ（帳票出力系）
            menu_CHUKI_JOKEN.Enabled = False         ' 財務諸表注記
            menu_ZANDAKA_JOKEN.Enabled = False       ' リース資産残高一覧表
            menu_SAIMU_JOKEN.Enabled = False         ' リース債務返済明細表
            menu_BEPPYO2_JOKEN.Enabled = False       ' 別表16（4）
        End If

        ' ---------------------------------------------------------
        ' 4. CanPrint=False → 印刷関連メニュー無効化
        '    決算タブの帳票は印刷を伴うため無効化
        ' ---------------------------------------------------------
        If LoginSession.CanPrint = False Then
            menu_CHUKI_JOKEN.Enabled = False         ' 財務諸表注記
            menu_ZANDAKA_JOKEN.Enabled = False       ' リース資産残高一覧表
            menu_SAIMU_JOKEN.Enabled = False         ' リース債務返済明細表
            menu_BEPPYO2_JOKEN.Enabled = False       ' 別表16（4）
        End If

        ' ---------------------------------------------------------
        ' 5. CanLogRef=False → ログ管理サブメニュー無効化
        '    Access版: Case 621,622,623 (ログ管理) は boLOG 必須
        ' ---------------------------------------------------------
        If LoginSession.CanLogRef = False Then
            ログ管理ToolStripMenuItem.Enabled = False
        End If

        ' ---------------------------------------------------------
        ' 5b. IsAdmin=False → システムタブ内のDB管理・統制オプション無効化
        '     Access版: Case 401,402,422,602 (DB管理系) は boAdmin 必須
        ' ---------------------------------------------------------
        If LoginSession.IsAdmin = False Then
            DB管理ToolStripMenuItem.Enabled = False
            menu_TOUSEI_OPT.Enabled = False
            menu_ENV_SETTING.Enabled = False
            menu_SWKK_FIXED.Enabled = False
        End If

        ' ---------------------------------------------------------
        ' 6. AccessKind=2（参照のみ）→ データ変更系メニュー無効化
        '    Access版: cngACCESS_KIND_REF
        '    - 台帳タブの「新規入力」無効化
        '    - 一括更新タブ全体無効化
        '    Access版 p_menu2: Case 301 (注記再計算) は AccessKind=UPD(1)のみ
        '    Access版 p_menu2: Case 302,303,304 (物件移動/再リース/減損取込) は gSECCHK_UPD
        '    Access版 p_menu2: Case 403 (Excel取込) は gSECCHK_UPD
        ' ---------------------------------------------------------
        If LoginSession.AccessKind = 2 Then
            ' 台帳タブの新規入力を無効化
            menu_NEW_CONTRACT.Enabled = False

            ' 一括更新タブ全体を無効化
            一括更新ToolStripMenuItem.Enabled = False

            ' 月次タブの仕訳計上も変更操作を伴うため無効化
            menu_KEIJO_JOKEN.Enabled = False
        End If

        ' ---------------------------------------------------------
        ' 7. AccessKind=3（管理単位限定）→ AccessKind=2と同じ制御 + 追加制限
        '    Access版: cngACCESS_KIND_OTH
        '    参照のみと同じ制限に加え、管理単位外のデータへのアクセスを制限
        ' ---------------------------------------------------------
        If LoginSession.AccessKind = 3 Then
            ' AccessKind=2と同じ制御
            menu_NEW_CONTRACT.Enabled = False
            一括更新ToolStripMenuItem.Enabled = False
            menu_KEIJO_JOKEN.Enabled = False

            ' 追加制限: 管理単位限定では全社横断の帳票出力も制限
            ' Access版 p_menu2 の各フォームで AccessKind=OTH チェックがある項目
            menu_CHUKI_JOKEN.Enabled = False         ' 財務諸表注記 (f_CHUKI_JOKEN:151)
            menu_SAIMU_JOKEN.Enabled = False         ' リース債務返済明細表 (f_SAIMU_JOKEN:146)
            menu_BEPPYO2_JOKEN.Enabled = False       ' 別表16（4）(f_BEPPYO2_JOKEN:934)
            menu_IDOLST_JOKEN.Enabled = False        ' 移動物件一覧表 (f_IDOLST_JOKEN:518)
        End If
    End Sub

    ' =========================================================
    ' 全メニューの有効状態をリセット（再ログイン時に使用）
    ' =========================================================
    Private Sub ResetAllMenuEnabled()
        ' システムタブ
        システムToolStripMenuItem.Enabled = True
        DB管理ToolStripMenuItem.Enabled = True
        menu_DB_SAVE.Enabled = True
        menu_DB_RESTORE.Enabled = True
        menu_EXCEL_IMPORT.Enabled = True
        menu_CACHE_CLEAR.Enabled = True
        menu_DB_COMPACT.Enabled = True
        menu_DB_CREATE.Enabled = True
        menu_DB_DROP.Enabled = True
        menu_LOCK_CLEAR.Enabled = True
        menu_DB_MIGRATE.Enabled = True
        menu_DB_MNT_BKNRI.Enabled = True
        menu_DB_EXPORT.Enabled = True
        menu_ENV_SETTING.Enabled = True
        menu_TOUSEI_OPT.Enabled = True
        menu_PWD_CHANGE.Enabled = True
        ログ管理ToolStripMenuItem.Enabled = True
        menu_LOG_REF.Enabled = True
        menu_LOG_UPD.Enabled = True
        menu_LOG_SAVE_RESTORE.Enabled = True
        menu_LOG_DELETE.Enabled = True
        menu_SWKK_FIXED.Enabled = True
        menu_VERSION_INFO.Enabled = True
        menu_LOGOUT.Enabled = True
        menu_EXIT.Enabled = True

        ' 台帳タブ
        台帳ToolStripMenuItem.Enabled = True
        menu_CONTRACT_LIST.Enabled = True
        menu_BUKN_LIST.Enabled = True
        menu_HAIF.Enabled = True
        menu_HENF.Enabled = True
        menu_GSON.Enabled = True
        menu_NEW_CONTRACT.Enabled = True

        ' 月次タブ
        月次ToolStripMenuItem.Enabled = True
        menu_TOUGETSU_JOKEN.Enabled = True
        menu_KEIJO_JOKEN.Enabled = True

        ' 期間タブ
        期間ToolStripMenuItem.Enabled = True
        menu_TANA_JOKEN.Enabled = True
        menu_KLSRYO_JOKEN.Enabled = True
        menu_IDOLST_JOKEN.Enabled = True
        menu_KHIYO_JOKEN.Enabled = True
        menu_YOSAN_JOKEN.Enabled = True

        ' 決算タブ
        決算ToolStripMenuItem.Enabled = True
        menu_CHUKI_JOKEN.Enabled = True
        menu_ZANDAKA_JOKEN.Enabled = True
        menu_SAIMU_JOKEN.Enabled = True
        menu_BEPPYO2_JOKEN.Enabled = True

        ' マスタタブ
        マスタToolStripMenuItem.Enabled = True
        menu_CORP.Enabled = True
        menu_KKNRI.Enabled = True
        menu_LCPT.Enabled = True
        menu_SHHO.Enabled = True
        menu_GENK.Enabled = True
        menu_BCAT.Enabled = True
        menu_BKNRI.Enabled = True
        menu_HKMK.Enabled = True
        menu_SKMK.Enabled = True
        menu_BKIND.Enabled = True
        menu_KOZA.Enabled = True
        menu_GSHA.Enabled = True
        menu_MCPT.Enabled = True
        menu_HKHO.Enabled = True
        menu_RSRVH1.Enabled = True
        menu_RSRVB1.Enabled = True
        menu_KARI_RITU.Enabled = True
        menu_ZEI_KAISEI.Enabled = True
        menu_HREL.Enabled = True

        ' 一括更新タブ
        一括更新ToolStripMenuItem.Enabled = True
        menu_CHUKI_RECALC.Enabled = True
        menu_IMPORT_CONTRACT_FROM_EXCEL.Enabled = True
        menu_IMPORT_IDO_FROM_EXCEL.Enabled = True
        menu_IMPORT_SAILEASE_FROM_EXCEL.Enabled = True
        menu_IMPORT_GSON_FROM_EXCEL.Enabled = True
    End Sub

    ' =========================================================
    ' システムタブ - ログオフ (Access版 Case 613)
    ' Access版 gLOGOFF (p_StartUp.txt:535-570) の再現
    ' =========================================================
    Private Sub menu_LOGOUT_Click(sender As Object, e As EventArgs) Handles menu_LOGOUT.Click
        ' --- Access版 p_menu2 Case 613 再現: ログオフ確認ダイアログ ---
        Dim confirmResult As DialogResult = MessageBox.Show(
            "ログオフしますか？",
            "ログオフ確認",
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)

        If confirmResult <> DialogResult.Yes Then
            Return
        End If

        ' --- Access版 p_menu2 Case 613: gCloseFORMandREPORT 再現 ---
        ' 開いている子フォームをすべて閉じる（旧セッションのフォームが残存しないようにする）
        CloseAllChildForms()

        ' --- Access版 gLOGOFF (p_StartUp.txt:535-570) 再現 ---
        ' 1. ログアウト時のログ記録（Access版 gLOGOFF: olSLOG.OutputSLOG 再現）
        RecordLogoutLog()

        ' 2. セッションをクリア（Access版: vgUSER_ID=Null, vgUSER_NM=Null, vgKNGN_ID=Null, sgDB_NAME="" 等）
        LoginSession.Clear()

        ' 3. タイトルバー更新（Access版: olStartUp.gSetAppTITLE）
        Me.Text = "LeaseM4BS"

        ' 4. メイン画面を非表示にしてログイン画面を再表示
        Me.Hide()

        Using loginForm As New Form_f_LOGIN_JET()
            Dim result As DialogResult = loginForm.ShowDialog()

            If result = DialogResult.OK Then
                ' ログイン成功: ユーザー情報を再表示
                tsslUserInfo.Text = "ログイン者: " & LoginSession.LoggedInUserNm &
                                    "（権限: " & LoginSession.KngnNm & "）"

                ' タイトルバーにDB環境名を再設定
                SetTitleWithDbName()

                ' 権限制御を再適用（全フラグをリセットして再適用）
                ApplyMenuPermissions()

                Me.Show()
            Else
                ' キャンセル: アプリケーション終了
                Me.Close()
            End If
        End Using
    End Sub

    ' =========================================================
    ' 全子フォーム閉じ処理
    ' Access版 gCloseFORMandREPORT (p_menu2.txt) の再現
    ' ログアウト時に開いている子フォームをすべて閉じ、
    ' 旧セッションのフォームが新セッションに残存しないようにする
    ' =========================================================
    Private Sub CloseAllChildForms()
        ' OpenFormsコレクションは閉じると変化するためToListでコピーしてから処理
        For Each frm As Form In Application.OpenForms.Cast(Of Form).Where(Function(f) f IsNot Me).ToList()
            Try
                frm.Close()
            Catch ex As Exception
                ' フォーム閉じ失敗はスキップ（ログアウト処理を阻害しない）
            End Try
        Next
    End Sub

    ' =========================================================
    ' ログアウト時のログ記録
    ' Access版 gLOGOFF (p_StartUp.txt:539-544) の再現
    ' sec_slog テーブルに LOGOUT 操作を記録する
    ' テーブルが存在しない場合は Try-Catch でスキップ
    ' =========================================================
    Private Sub RecordLogoutLog()
        Try
            Dim crud As New CrudHelper()
            Dim columnValues As New Dictionary(Of String, Object) From {
                {"user_id", LoginSession.LoggedInUserId},
                {"op_kbn", "LOGOUT"},
                {"op_st_dt", DateTime.Now},
                {"op_detail1", Me.Text},
                {"upd_sbt", "その他"}
            }
            crud.Insert("sec_slog", columnValues)
        Catch ex As Exception
            ' テーブルが存在しない場合やDB接続エラーはスキップ
            ' 本体処理（ログアウト）に影響させない
        End Try
    End Sub

    ' =========================================================
    ' 台帳タブ
    ' =========================================================
    ' [契約書フレックス]
    Private Sub menu_CONTRACT_LIST_Click(sender As Object, e As EventArgs) Handles menu_CONTRACT_LIST.Click
        Try
            Dim frm As New Form_f_flx_CONTRACT()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [物件フレックス]
    Private Sub menu_BUKN_LIST_Click(sender As Object, e As EventArgs) Handles menu_BUKN_LIST.Click
        Try
            Dim frm As New Form_f_flx_BUKN()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [物件フレックス（配賦行単位）]
    Private Sub menu_HAIF_Click(sender As Object, e As EventArgs) Handles menu_HAIF.Click
        Try
            Dim frm As New Form_f_flx_D_HAIF()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [保守フレックス（物件付随保守料）]
    Private Sub menu_HENF_Click(sender As Object, e As EventArgs) Handles menu_HENF.Click
        Try
            Dim frm As New Form_f_flx_D_HENF()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [減損フレックス]
    Private Sub menu_GSON_Click(sender As Object, e As EventArgs) Handles menu_GSON.Click
        Try
            Dim frm As New Form_f_flx_D_GSON()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [新規入力]
    Private Sub menu_NEW_CONTRACT_Click(sender As Object, e As EventArgs) Handles menu_NEW_CONTRACT.Click
        Try
            Dim frm As New Form_ContractEntry()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 月次タブ
    ' =========================================================
    ' [月次支払照合フレックス]
    Private Sub menu_TOUGETSU_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_TOUGETSU_JOKEN.Click
        Try
            Dim frm As New Form_f_TOUGETSU_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [月次仕訳計上フレックス]
    Private Sub menu_KEIJO_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_KEIJO_JOKEN.Click
        Try
            Dim frm As New Form_f_KEIJO_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 期間タブ
    ' =========================================================
    ' [棚卸明細表]
    Private Sub menu_TANA_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_TANA_JOKEN.Click
        Try
            Dim frm As New Form_f_TANA_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [期間リース料支払い明細表]
    Private Sub menu_KLSRYO_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_KLSRYO_JOKEN.Click
        Try
            Dim frm As New Form_f_KLSRYO_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [移動物件一覧表]
    Private Sub menu_IDOLST_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_IDOLST_JOKEN.Click
        Try
            Dim frm As New Form_f_IDOLST_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [期間費用計上明細表]
    Private Sub menu_KHIYO_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_KHIYO_JOKEN.Click
        Try
            Dim frm As New Form_f_KHIYO_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [予算実績集計]
    Private Sub menu_YOSAN_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_YOSAN_JOKEN.Click
        Try
            Dim frm As New Form_f_YOSAN_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 決算タブ
    ' =========================================================
    ' [財務諸表注記]
    Private Sub menu_CHUKI_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_CHUKI_JOKEN.Click
        Try
            Dim frm As New Form_f_CHUKI_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [リース残高一覧表]
    Private Sub menu_ZANDAKA_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_ZANDAKA_JOKEN.Click
        Try
            Dim frm As New Form_f_ZANDAKA_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [リース債務返済明細一覧]
    Private Sub menu_SAIMU_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_SAIMU_JOKEN.Click
        Try
            Dim frm As New Form_f_SAIMU_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [別表16（4）]
    Private Sub menu_BEPPYO2_JOKEN_Click(sender As Object, e As EventArgs) Handles menu_BEPPYO2_JOKEN.Click
        Try
            Dim frm As New Form_f_BEPPYO2_JOKEN()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' マスタタブ
    ' =========================================================
    ' [会社]
    Private Sub menu_CORP_Click(sender As Object, e As EventArgs) Handles menu_CORP.Click
        Try
            Dim frm As New Form_f_flx_M_CORP()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [契約管理単位]
    Private Sub menu_KKNRI_Click(sender As Object, e As EventArgs) Handles menu_KKNRI.Click
        Try
            Dim frm As New Form_f_flx_M_KKNRI()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [支払先]
    Private Sub menu_LCPT_Click(sender As Object, e As EventArgs) Handles menu_LCPT.Click
        Try
            Dim frm As New Form_f_flx_M_LCPT()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [支払方法]
    Private Sub menu_SHHO_Click(sender As Object, e As EventArgs) Handles menu_SHHO.Click
        Try
            Dim frm As New Form_f_flx_M_SHHO()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [原価区分]
    Private Sub menu_GENK_Click(sender As Object, e As EventArgs) Handles menu_GENK.Click
        Try
            Dim frm As New Form_f_flx_M_GENK()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [部署]
    Private Sub menu_BCAT_Click(sender As Object, e As EventArgs) Handles menu_BCAT.Click
        Try
            Dim frm As New Form_f_flx_M_BCAT()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [物件管理単位]
    Private Sub menu_BKNRI_Click(sender As Object, e As EventArgs) Handles menu_BKNRI.Click
        Try
            Dim frm As New Form_f_flx_M_BKNRI()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [費用区分]
    Private Sub menu_HKMK_Click(sender As Object, e As EventArgs) Handles menu_HKMK.Click
        Try
            Dim frm As New Form_f_flx_M_HKMK()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [資産区分]
    Private Sub menu_SKMK_Click(sender As Object, e As EventArgs) Handles menu_SKMK.Click
        Try
            Dim frm As New Form_f_flx_M_SKMK()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [物件種別]
    Private Sub menu_BKIND_Click(sender As Object, e As EventArgs) Handles menu_BKIND.Click
        Try
            Dim frm As New Form_f_flx_M_BKIND()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [銀行口座]
    Private Sub menu_KOZA_Click(sender As Object, e As EventArgs) Handles menu_KOZA.Click
        Try
            Dim frm As New Form_f_flx_M_KOZA()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [業者]
    Private Sub menu_GSHA_Click(sender As Object, e As EventArgs) Handles menu_GSHA.Click
        Try
            Dim frm As New Form_f_flx_M_GSHA()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [メーカー]
    Private Sub menu_MCPT_Click(sender As Object, e As EventArgs) Handles menu_MCPT.Click
        Try
            Dim frm As New Form_f_flx_M_MCPT()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [廃棄方法]
    Private Sub menu_HKHO_Click(sender As Object, e As EventArgs) Handles menu_HKHO.Click
        Try
            Dim frm As New Form_f_flx_M_HKHO()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [予備（契約書用）]
    Private Sub menu_RSRVH1_Click(sender As Object, e As EventArgs) Handles menu_RSRVH1.Click
        Try
            Dim frm As New Form_f_flx_M_RSRVK1()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [予備（物件用）]
    Private Sub menu_RSRVB1_Click(sender As Object, e As EventArgs) Handles menu_RSRVB1.Click
        Try
            Dim frm As New Form_f_flx_M_RSRVB1()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [追加借入利子率テーブル]
    Private Sub menu_KARI_RITU_Click(sender As Object, e As EventArgs) Handles menu_KARI_RITU.Click
        Try
            Dim frm As New Form_f_T_KARI_RITU()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [消費税率テーブル]
    Private Sub menu_ZEI_KAISEI_Click(sender As Object, e As EventArgs) Handles menu_ZEI_KAISEI.Click
        Try
            Dim frm As New Form_f_T_ZEI_KAISEI()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [費用関連テーブル]
    Private Sub menu_HREL_Click(sender As Object, e As EventArgs) Handles menu_HREL.Click
        Try
            Dim frm As New Form_fc_TC_HREL()
            frm.Show()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    ' 一括更新タブ
    ' =========================================================
    ' [注記判定再計算]
    Private Sub menu_CHUKI_RECALC_Click(sender As Object, e As EventArgs) Handles menu_CHUKI_RECALC.Click
        Try
            Dim frm As New Form_f_CHUKI_RECALC()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [契約書変更情報Excel取込]
    Private Sub menu_IMPORT_CONTRACT_FROM_EXCEL_Click(sender As Object, e As EventArgs) Handles menu_IMPORT_CONTRACT_FROM_EXCEL.Click
        Try
            Dim frm As New Form_f_IMPORT_CONTRACT_FROM_EXCEL()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [物件移動]
    Private Sub menu_IMPORT_IDO_FROM_EXCEL_Click(sender As Object, e As EventArgs) Handles menu_IMPORT_IDO_FROM_EXCEL.Click
        Try
            Dim frm As New Form_f_IMPORT_IDO_FROM_EXCEL()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [再リース/返却]
    Private Sub menu_IMPORT_SAILEASE_FROM_EXCEL_Click(sender As Object, e As EventArgs) Handles menu_IMPORT_SAILEASE_FROM_EXCEL.Click
        Try
            Dim frm As New Form_f_IMPORT_SAILEASE_FROM_EXCEL()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' [減損損失の取り込み]
    Private Sub menu_IMPORT_GSON_FROM_EXCEL_Click(sender As Object, e As EventArgs) Handles menu_IMPORT_GSON_FROM_EXCEL.Click
        Try
            Dim frm As New Form_f_IMPORT_GSON_FROM_EXCEL()
            frm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("フォームの起動に失敗しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ' =========================================================
    ' システムタブ - 業務の終了
    ' Access版 p_menu2 Case 625 相当
    ' =========================================================
    Private Sub menu_EXIT_Click(sender As Object, e As EventArgs) Handles menu_EXIT.Click
        Dim confirmResult As DialogResult = MessageBox.Show(
            "業務を終了しますか？",
            "終了確認",
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)

        If confirmResult <> DialogResult.Yes Then
            Return
        End If

        ' ログアウト処理を経由して終了
        CloseAllChildForms()
        RecordLogoutLog()
        LoginSession.Clear()
        Me.Close()
    End Sub

    ' =========================================================
    ' システムタブ - バージョン情報 (Access版 Case 612)
    ' =========================================================
    Private Sub menu_VERSION_INFO_Click(sender As Object, e As EventArgs) Handles menu_VERSION_INFO.Click
        Dim appVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        MessageBox.Show(
            "LeaseM4BS" & vbCrLf &
            "Version: " & appVersion & vbCrLf &
            "DB Version: " & LoginSession.DbVersion,
            "バージョン情報",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =========================================================
    ' システムタブ - 未実装メニュー項目のプレースホルダー
    ' 各機能は別Issue/別フェーズで実装予定
    ' =========================================================

    ' [DB管理 - データ保存] Access版 Case 401
    Private Sub menu_DB_SAVE_Click(sender As Object, e As EventArgs) Handles menu_DB_SAVE.Click
        MessageBox.Show("データ保存機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - データ復元] Access版 Case 402
    Private Sub menu_DB_RESTORE_Click(sender As Object, e As EventArgs) Handles menu_DB_RESTORE.Click
        MessageBox.Show("データ復元機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - EXCEL取込] Access版 Case 403
    Private Sub menu_EXCEL_IMPORT_Click(sender As Object, e As EventArgs) Handles menu_EXCEL_IMPORT.Click
        MessageBox.Show("EXCEL取込機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - キャッシュクリア] Access版 Case 404
    Private Sub menu_CACHE_CLEAR_Click(sender As Object, e As EventArgs) Handles menu_CACHE_CLEAR.Click
        MessageBox.Show("キャッシュクリア機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - 最適化] Access版 Case 421
    Private Sub menu_DB_COMPACT_Click(sender As Object, e As EventArgs) Handles menu_DB_COMPACT.Click
        MessageBox.Show("DB最適化機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - DB作成] Access版 Case 422
    Private Sub menu_DB_CREATE_Click(sender As Object, e As EventArgs) Handles menu_DB_CREATE.Click
        MessageBox.Show("DB作成機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - DB削除] Access版 Case 423
    Private Sub menu_DB_DROP_Click(sender As Object, e As EventArgs) Handles menu_DB_DROP.Click
        MessageBox.Show("DB削除機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - ロック解除] Access版 Case 424
    Private Sub menu_LOCK_CLEAR_Click(sender As Object, e As EventArgs) Handles menu_LOCK_CLEAR.Click
        MessageBox.Show("ロック解除機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - DBマイグレーション] Access版 Case 425
    Private Sub menu_DB_MIGRATE_Click(sender As Object, e As EventArgs) Handles menu_DB_MIGRATE.Click
        MessageBox.Show("DBマイグレーション機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - 物件管理単位メンテナンス] Access版 Case 426
    Private Sub menu_DB_MNT_BKNRI_Click(sender As Object, e As EventArgs) Handles menu_DB_MNT_BKNRI.Click
        MessageBox.Show("物件管理単位メンテナンス機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [DB管理 - データエクスポート] Access版 Case 427
    Private Sub menu_DB_EXPORT_Click(sender As Object, e As EventArgs) Handles menu_DB_EXPORT.Click
        MessageBox.Show("データエクスポート機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [環境設定] Access版 Case 601
    Private Sub menu_ENV_SETTING_Click(sender As Object, e As EventArgs) Handles menu_ENV_SETTING.Click
        MessageBox.Show("環境設定機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [統制オプション] Access版 Case 602
    Private Sub menu_TOUSEI_OPT_Click(sender As Object, e As EventArgs) Handles menu_TOUSEI_OPT.Click
        MessageBox.Show("統制オプション機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [パスワード変更] Access版 Case 408
    Private Sub menu_PWD_CHANGE_Click(sender As Object, e As EventArgs) Handles menu_PWD_CHANGE.Click
        MessageBox.Show("パスワード変更機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [ログ管理 - 参照ログ] Access版 Case 621
    Private Sub menu_LOG_REF_Click(sender As Object, e As EventArgs) Handles menu_LOG_REF.Click
        MessageBox.Show("参照ログ機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [ログ管理 - 更新ログ] Access版 Case 622
    Private Sub menu_LOG_UPD_Click(sender As Object, e As EventArgs) Handles menu_LOG_UPD.Click
        MessageBox.Show("更新ログ機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [ログ管理 - 保存復元ログ] Access版 Case 623
    Private Sub menu_LOG_SAVE_RESTORE_Click(sender As Object, e As EventArgs) Handles menu_LOG_SAVE_RESTORE.Click
        MessageBox.Show("保存復元ログ機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [ログ管理 - ログ削除] Access版 Case 624
    Private Sub menu_LOG_DELETE_Click(sender As Object, e As EventArgs) Handles menu_LOG_DELETE.Click
        MessageBox.Show("ログ削除機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' [仕訳固定値設定] Access版 Case 611
    Private Sub menu_SWKK_FIXED_Click(sender As Object, e As EventArgs) Handles menu_SWKK_FIXED.Click
        MessageBox.Show("仕訳固定値設定機能は未実装です。", "未実装", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
