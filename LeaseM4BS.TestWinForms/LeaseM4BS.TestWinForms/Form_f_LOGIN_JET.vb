Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class Form_f_LOGIN_JET
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    ' デフォルトのログイン試行上限 (sec_user.login_attempts が NULL の場合)
    Private Const DEFAULT_LOGIN_ATTEMPTS As Integer = 5

    ' パスワード有効期限のデフォルト日数
    Private Const DEFAULT_PWD_EXPIRE_DAYS As Integer = 90

    ' パスワード期限切迫の警告日数（残り30日以内で警告）
    Private Const PWD_EXPIRE_WARNING_DAYS As Integer = 30

    ' 期待するDBバージョン（app_version テーブルと比較）
    Private Const EXPECTED_DB_VERSION As String = "1.0.0"

    Public Sub New()
        InitializeComponent()
    End Sub

    ' =========================================================
    '  フォーム初期化
    ' =========================================================
    Private Sub Form_f_LOGIN_JET_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' パスワードマスク
        txt_PWD.PasswordChar = "*"c
        txt_PWD.MaxLength = 255

        ' ユーザーコード入力欄
        txt_USER_CD.MaxLength = 12

        ' 保存用フィールドは非表示
        txt_USER_CD_SAVE.Visible = False

        ' 前回ログインしたユーザーコードを復元
        If txt_USER_CD_SAVE.Text <> "" Then
            txt_USER_CD.Text = txt_USER_CD_SAVE.Text
            txt_PWD.Focus()
        Else
            txt_USER_CD.Focus()
        End If

        ' 接続先パスを表示
        Try
            Dim dbMgr As New DbConnectionManager()
            txt_PATH.Text = dbMgr.GetMaskedConnectionString()
        Catch
            txt_PATH.Text = "(接続情報取得エラー)"
        End Try
        txt_PATH.ReadOnly = True

        ' Enterキーでログイン実行
        Me.AcceptButton = cmd_Jikko
        Me.CancelButton = cmd_Cancel
    End Sub

    ' =========================================================
    '  実行ボタン (認証処理)
    ' =========================================================
    Private Sub cmd_Jikko_Click(sender As Object, e As EventArgs) Handles cmd_Jikko.Click
        ' --- 入力チェック ---
        Dim userCd As String = txt_USER_CD.Text.Trim()
        Dim pwd As String = txt_PWD.Text

        If String.IsNullOrEmpty(userCd) Then
            MessageBox.Show("利用者コードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_USER_CD.Focus()
            Return
        End If

        If String.IsNullOrEmpty(pwd) Then
            MessageBox.Show("パスワードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_PWD.Focus()
            Return
        End If

        ' --- ユーザー検索 (history_f を含めて取得し、ロック状態も判定する) ---
        Dim sql As String = "SELECT user_id, user_cd, user_nm, pwd, kngn_id, " &
                            "err_ct, login_attempts, d_first_login, history_f, pwd_upd_dt " &
                            "FROM sec_user " &
                            "WHERE user_cd = @user_cd"
        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@user_cd", userCd)
        }

        Dim dt = _crud.GetDataTable(sql, prms)

        ' ユーザーが見つからない場合も「コードまたはパスワードが正しくありません」と表示
        ' （ユーザー存在有無の情報漏洩を防止）
        If dt.Rows.Count = 0 Then
            MessageBox.Show("利用者コードまたはパスワードが正しくありません。", "認証エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            txt_USER_CD.Focus()
            Return
        End If

        Dim row = dt.Rows(0)
        Dim userId As Integer = CInt(row("user_id"))
        Dim userNm As String = If(row("user_nm") IsNot DBNull.Value, row("user_nm").ToString(), "")
        Dim storedPwd As String = If(row("pwd") IsNot DBNull.Value, row("pwd").ToString(), "")
        Dim kngnId As Integer = CInt(row("kngn_id"))
        Dim errCt As Integer = If(row("err_ct") IsNot DBNull.Value, CInt(row("err_ct")), 0)
        Dim loginAttempts As Integer = If(row("login_attempts") IsNot DBNull.Value,
                                          CInt(row("login_attempts")), DEFAULT_LOGIN_ATTEMPTS)
        Dim historyF As Boolean = If(row("history_f") IsNot DBNull.Value, CBool(row("history_f")), False)

        ' --- アカウントロック判定 ---
        ' history_f = TRUE の場合はアカウントが無効化されている
        ' ユーザー存在有無の情報漏洩を防ぐため、未存在時と同じメッセージを表示
        If historyF Then
            MessageBox.Show("利用者コードまたはパスワードが正しくありません。", "認証エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Access版と同じ動作: ERR_CT > LOGIN_ATTEMPTS でロック（最後の1回は試行を許可し、
        ' パスワード不正時にHISTORY_F=TRUEを設定する）
        If loginAttempts > 0 AndAlso errCt > loginAttempts Then
            MessageBox.Show("ログイン試行回数の上限に達しました。" & vbCrLf &
                            "システム管理者に連絡してください。",
                            "アカウントロック",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' --- パスワード照合 (Gap 11: SHA256ハッシュ比較 + 平文フォールバック) ---
        If Not VerifyPassword(pwd, storedPwd) Then
            ' 監査ログ: ログイン失敗 (Access版 cngOP_KBN_LOGINERR 相当)
            LoginSession.WriteAuditLogAs(LoginSession.OP_KBN_LOGINERR, "パスワード不正 ユーザー:" & userCd, userCd, "")

            ' 失敗: err_ct をインクリメント + last_err_dt を更新
            errCt += 1

            ' --- アカウント無効化判定 (Gap 12: Access版 HISTORY_F=True に相当) ---
            ' Access版: imTryCnt = ERR_CT+1 → If LOGIN_ATTEMPTS < imTryCnt Then HISTORY_F=True
            ' つまり errCt(増分後) > loginAttempts でロック
            If loginAttempts > 0 AndAlso errCt > loginAttempts Then
                ' 試行上限超過: アカウントを論理削除して終了
                Dim lockSql As String = "UPDATE sec_user SET err_ct = @err_ct, last_err_dt = @now, history_f = TRUE WHERE user_id = @user_id"
                Dim lockPrms As New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@err_ct", errCt),
                    New NpgsqlParameter("@now", DateTime.Now),
                    New NpgsqlParameter("@user_id", userId)
                }
                _crud.ExecuteNonQuery(lockSql, lockPrms)

                MessageBox.Show("ログインの再試行回数が許容の回数を超えました。" & vbCrLf &
                                "利用者コード:" & userCd & " は使用できません。" & vbCrLf &
                                "アカウントがロックされました。管理者に連絡してください。",
                                "アカウントロック",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
                Return
            End If

            ' 試行上限未達: err_ct 更新のみ
            Dim updateSql As String = "UPDATE sec_user SET err_ct = @err_ct, last_err_dt = @now WHERE user_id = @user_id"
            Dim updatePrms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@err_ct", errCt),
                New NpgsqlParameter("@now", DateTime.Now),
                New NpgsqlParameter("@user_id", userId)
            }
            _crud.ExecuteNonQuery(updateSql, updatePrms)

            ' ユーザー存在有無の情報漏洩を防ぐため、すべてのケースで同一メッセージを表示
            If loginAttempts > 0 Then
                Dim remaining As Integer = loginAttempts - errCt
                If remaining > 0 Then
                    MessageBox.Show("利用者コードまたはパスワードが正しくありません。" & vbCrLf &
                                    $"残り試行回数: {remaining} 回",
                                    "認証エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show("利用者コードまたはパスワードが正しくありません。" & vbCrLf &
                                    "ログイン試行回数の上限に達しました。" & vbCrLf &
                                    "システム管理者に連絡してください。",
                                    "アカウントロック",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                ' loginAttempts = 0 は試行回数無制限
                MessageBox.Show("利用者コードまたはパスワードが正しくありません。",
                                "認証エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            txt_PWD.Clear()
            txt_PWD.Focus()
            Return
        End If

        ' --- 認証成功 ---

        ' err_ct リセット + last_err_dt クリア + 初回ログイン記録
        Dim successSql As String
        Dim successPrms As New List(Of NpgsqlParameter)

        Dim isFirstLogin As Boolean = (row("d_first_login") Is DBNull.Value)
        If isFirstLogin Then
            successSql = "UPDATE sec_user SET err_ct = 0, last_err_dt = NULL, d_first_login = @now WHERE user_id = @user_id"
            successPrms.Add(New NpgsqlParameter("@now", DateTime.Now))
        Else
            successSql = "UPDATE sec_user SET err_ct = 0, last_err_dt = NULL WHERE user_id = @user_id"
        End If
        successPrms.Add(New NpgsqlParameter("@user_id", userId))
        _crud.ExecuteNonQuery(successSql, successPrms)

        ' グローバルセッションにユーザー情報をセット
        LoginSession.LoggedInUserId = userId
        LoginSession.LoggedInUserCd = userCd
        LoginSession.LoggedInUserNm = userNm

        ' --- 認証後の初期化処理 ---
        ' 各ステップで例外が発生してもセッションが中途半端にならないよう保護
        Try
            ' 権限情報をロード
            LoginSession.LoadPermissions(kngnId)

            ' パスワードポリシーをロード
            LoginSession.LoadPasswordPolicy(userId)

            ' --- Gap 4: DBバージョン＆整合性チェック ---
            If Not CheckDatabaseVersion() Then
                ' バージョン不一致でアプリ終了
                LoginSession.Clear()
                Application.Exit()
                Return
            End If

            ' --- Gap 5: ユーザーセット初期化 (Access版 gInitUserSet に相当) ---
            LoginSession.InitUserSet()
            If Not LoginSession.CurrentUserSetLoaded Then
                MessageBox.Show("ユーザーセットの初期化に失敗しました。" & vbCrLf &
                                "システム管理者に連絡してください。",
                                "初期化エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' 警告のみ表示して続行（致命的エラーではない場合）
            End If

            ' --- Gap 6: 月次オプション読込 (Access版 GetTousei_OPT に相当) ---
            LoginSession.LoadTouseiOptions()
            LoginSession.LoadCustomerType()

            ' --- Gap 11: パスワード有効期限チェック (Access版 gPWD_KIGEN に相当) ---
            CheckPasswordExpiry(row)

        Catch ex As Exception
            ' 初期化処理の失敗時はセッションをクリアしてログイン不可とする
            LoginSession.Clear()
            MessageBox.Show("ログイン後の初期化処理でエラーが発生しました。" & vbCrLf &
                            ex.Message & vbCrLf &
                            "システム管理者に連絡してください。",
                            "初期化エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' セッション状態を有効化（初期化がすべて成功した後）
        LoginSession.LoginDateTime = DateTime.Now
        LoginSession.IsSessionActive = True

        ' 監査ログ: ログイン成功 (Access版 olSLOG.OutputSLOG 相当)
        LoginSession.WriteAuditLog(LoginSession.OP_KBN_LOGIN, "ログイン成功")

        ' 次回用にユーザーコードを保存
        txt_USER_CD_SAVE.Text = userCd

        ' ダイアログ結果をOKにしてフォームを閉じる
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' =========================================================
    '  キャンセルボタン
    ' =========================================================
    Private Sub cmd_Cancel_Click(sender As Object, e As EventArgs) Handles cmd_Cancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' =========================================================
    '  Gap 4: DBバージョンチェック
    '  Access版 gDB_VERSION_CHK() + gDB_CHK() に相当
    '  app_version テーブルからバージョンを取得し、期待値と比較
    ' =========================================================
    Private Function CheckDatabaseVersion() As Boolean
        Try
            ' app_version テーブルが存在するか確認し、バージョンを取得
            Dim sql As String = "SELECT version_no FROM app_version ORDER BY updated_at DESC LIMIT 1"
            Dim dt = _crud.GetDataTable(sql)

            If dt.Rows.Count = 0 Then
                ' バージョンレコードなし: 警告のみで続行
                ' （初回導入時はテーブルが空の可能性あり）
                LoginSession.DbVersion = "(未設定)"
                Return True
            End If

            Dim dbVersion As String = If(dt.Rows(0)("version_no") IsNot DBNull.Value,
                                         dt.Rows(0)("version_no").ToString(), "")
            LoginSession.DbVersion = dbVersion

            If dbVersion <> EXPECTED_DB_VERSION Then
                MessageBox.Show("データベースバージョンが一致しません。" & vbCrLf &
                                $"期待値: {EXPECTED_DB_VERSION}" & vbCrLf &
                                $"実際値: {dbVersion}" & vbCrLf &
                                "システムを終了します。",
                                "DBバージョンエラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return True

        Catch ex As Exception
            ' app_version テーブルが存在しない場合はスキップ（警告のみ）
            LoginSession.DbVersion = "(チェック不可)"
            Return True
        End Try
    End Function

    ' =========================================================
    '  Gap 11: パスワード暗号化比較
    '  SHA256ハッシュ比較 + 平文フォールバック
    '  Access版 pc_Encrypt の暗号化は独自方式だが、PostgreSQL版は
    '  SHA256に移行。既存の平文パスワードにも対応するフォールバック。
    ' =========================================================

    ''' <summary>
    ''' パスワードを検証する
    ''' 1. SHA256ハッシュとして比較
    ''' 2. 一致しなければ平文として比較（フォールバック）
    ''' </summary>
    Private Function VerifyPassword(inputPwd As String, storedPwd As String) As Boolean
        If String.IsNullOrEmpty(storedPwd) AndAlso String.IsNullOrEmpty(inputPwd) Then
            Return True
        End If

        If String.IsNullOrEmpty(storedPwd) OrElse String.IsNullOrEmpty(inputPwd) Then
            Return False
        End If

        ' まずSHA256ハッシュとして比較
        Dim inputHash As String = ComputeSha256Hash(inputPwd)
        If String.Equals(inputHash, storedPwd, StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        ' フォールバック: 平文として比較（既存DBのパスワードが平文の場合）
        If storedPwd = inputPwd Then
            Return True
        End If

        ' Access版の独自暗号化方式にも対応（pc_Encrypt互換の復号を試みる）
        Try
            Dim decrypted As String = DecryptAccessPassword(storedPwd)
            If decrypted IsNot Nothing AndAlso decrypted = inputPwd Then
                Return True
            End If
        Catch
            ' 復号失敗は無視
        End Try

        Return False
    End Function

    ''' <summary>
    ''' SHA256ハッシュを計算する
    ''' </summary>
    Private Shared Function ComputeSha256Hash(input As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function

    ''' <summary>
    ''' Access版 pc_Encrypt.gDecrypt 互換の復号処理
    ''' 暗号化キー: "ILTEX KUBOKI&amp;TANI"
    ''' 形式: 先頭4文字がシード値、以降4文字ごとに16進数の暗号化データ
    ''' </summary>
    Private Shared Function DecryptAccessPassword(encryptedStr As String) As String
        Const ENCRYPT_KEY As String = "ILTEX KUBOKI&TANI"

        If String.IsNullOrEmpty(encryptedStr) OrElse encryptedStr.Length < 8 Then
            Return Nothing
        End If

        ' 全体が4文字ごとの16進数であるか確認（seed prefix なしのパターン）
        ' Access版 pc_Encrypt の形式: 先頭4文字がシード値、以降が暗号データ
        ' ただしシード値が16進数のみで構成される場合もあるため、両パターンを試行する

        ' --- パターン1: 先頭4文字がseed（10進数）、残りがデータ ---
        Dim result As String = TryDecryptWithSeed(encryptedStr, ENCRYPT_KEY)
        If result IsNot Nothing Then Return result

        ' --- パターン2: seed = 0 として全体をデータとして復号 ---
        If encryptedStr.Length Mod 4 = 0 Then
            result = TryDecryptData(encryptedStr, ENCRYPT_KEY, 0)
            If result IsNot Nothing Then Return result
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' 先頭4文字をseed、残りをデータとして復号を試みる
    ''' </summary>
    Private Shared Function TryDecryptWithSeed(encryptedStr As String, encryptKey As String) As String
        Dim seedStr As String = encryptedStr.Substring(0, 4)
        Dim seed As Long
        If Not Long.TryParse(seedStr, seed) Then
            Return Nothing
        End If

        Dim dataStr As String = encryptedStr.Substring(4)
        If dataStr.Length = 0 OrElse dataStr.Length Mod 4 <> 0 Then
            Return Nothing
        End If

        Return TryDecryptData(dataStr, encryptKey, seed)
    End Function

    ''' <summary>
    ''' 指定されたseed値でデータ文字列を復号する。
    ''' 復号結果が印字可能文字のみで構成される場合に成功と判定する。
    ''' </summary>
    Private Shared Function TryDecryptData(dataStr As String, encryptKey As String, seed As Long) As String
        If dataStr.Length Mod 4 <> 0 Then Return Nothing

        Dim inLen As Integer = dataStr.Length \ 4
        Dim keyLen As Integer = encryptKey.Length
        Dim result As New StringBuilder()

        For i As Integer = 1 To inLen
            Dim hexStr As String = dataStr.Substring((i - 1) * 4, 4)
            Dim wk1 As Long
            Try
                wk1 = Convert.ToInt64(hexStr, 16)
            Catch
                Return Nothing
            End Try

            Dim wk2 As Long = 0
            If i <= keyLen Then
                wk2 = AscW(encryptKey(i - 1))
            End If

            Dim wk3 As Long = wk1 - wk2 - seed
            ' 復号結果が印字可能ASCII範囲外なら失敗
            If wk3 < 32 OrElse wk3 > 126 Then
                Return Nothing
            End If

            result.Append(ChrW(CInt(wk3)))
        Next

        If result.Length > 0 Then
            Return result.ToString()
        End If

        Return Nothing
    End Function

    ' =========================================================
    '  Gap 11: パスワード有効期限チェック
    '  Access版 gPWD_KIGEN() に相当
    '  戻り値の意味（Access版に準拠）:
    '    0: 問題なし
    '    1: パスワード未設定
    '    2: パスワード有効期限切れ
    '    3: パスワード変更猶予期間終了間近
    ' =========================================================
    Private Sub CheckPasswordExpiry(userRow As DataRow)
        Try
            ' パスワード有効期限の取得
            ' 優先順位: pwd_upd_dt（パスワード更新日）> d_first_login（初回ログイン日）
            Dim expireDate As DateTime? = Nothing

            ' pwd_upd_dt カラムがあれば最優先で使用（パスワード変更日 + 有効期限日数）
            ' PwdLifeTime が設定済みならそれを使用、未設定ならデフォルト値
            Dim expireDays As Integer = If(LoginSession.PwdLifeTime > 0, LoginSession.PwdLifeTime, DEFAULT_PWD_EXPIRE_DAYS)
            If userRow.Table.Columns.Contains("pwd_upd_dt") AndAlso userRow("pwd_upd_dt") IsNot DBNull.Value Then
                Dim pwdUpdDt As DateTime = CDate(userRow("pwd_upd_dt"))
                expireDate = pwdUpdDt.AddDays(expireDays)
            ElseIf userRow.Table.Columns.Contains("pwd_expire_dt") AndAlso userRow("pwd_expire_dt") IsNot DBNull.Value Then
                expireDate = CDate(userRow("pwd_expire_dt"))
            ElseIf userRow("d_first_login") IsNot DBNull.Value Then
                ' d_first_login からの経過日数で判定（デフォルト90日）
                Dim firstLogin As DateTime = CDate(userRow("d_first_login"))
                expireDate = firstLogin.AddDays(expireDays)
            Else
                ' 初回ログイン: パスワード設定を推奨
                Dim result As DialogResult = MessageBox.Show(
                    "パスワードが設定されていません。" & vbCrLf & "パスワードを設定しますか？",
                    "パスワード設定",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim frmPwd As New Form_f_CHANGE_PASSWORD()
                    frmPwd.ShowDialog()
                End If
                Return
            End If

            If expireDate.HasValue Then
                Dim today As DateTime = DateTime.Today
                Dim daysRemaining As Integer = CInt((expireDate.Value - today).TotalDays)

                If daysRemaining < 0 Then
                    ' 期限切れ
                    Dim result As DialogResult = MessageBox.Show(
                        "パスワードの有効期限が切れました。" & vbCrLf & "パスワードを変更しますか？",
                        "パスワード期限切れ",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        Dim frmPwd As New Form_f_CHANGE_PASSWORD()
                        frmPwd.ShowDialog()
                    End If
                ElseIf daysRemaining <= PWD_EXPIRE_WARNING_DAYS Then
                    ' 期限切迫（30日以内）: 警告メッセージ
                    MessageBox.Show(
                        $"パスワード変更猶予期間が終了間近です。" & vbCrLf &
                        $"残り {daysRemaining} 日です。" & vbCrLf &
                        "パスワードを変更しないと次回からログインできなくなります。",
                        "パスワード期限警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                ' daysRemaining > PWD_EXPIRE_WARNING_DAYS の場合は何もしない（正常）
            End If

        Catch ex As Exception
            ' パスワード有効期限チェックに失敗した場合はスキップ（ログイン自体は許可）
        End Try
    End Sub

End Class
