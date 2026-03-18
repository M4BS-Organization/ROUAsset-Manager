Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.Text
Imports LeaseM4BS.DataAccess
Imports Npgsql

' =========================================================
' システム利用者 入力画面
' Access版 Form_f_SEC_USER_INP 相当
' =========================================================
Partial Public Class Form_f_SEC_USER_INP
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    ''' <summary>
    ''' "NEW" = 新規, "EDIT" = 編集
    ''' </summary>
    Public Property EditMode As String = "NEW"

    ''' <summary>
    ''' 編集対象のユーザーID（EditMode="EDIT"の場合に設定）
    ''' </summary>
    Public Property TargetUserId As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    ' =========================================================
    '  フォーム初期化
    ' =========================================================
    Private Sub Form_f_SEC_USER_INP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' パスワードマスク
        txt_PWD_INP.PasswordChar = "*"c
        txt_PWD_INP_RETRY.PasswordChar = "*"c

        ' 非表示フィールド
        txt_Mode.Visible = False
        txt_ID.Visible = False
        txt_PWD.Visible = False
        txt_PWD_SAVE.Visible = False
        txt_HISTORY_F_SAVE.Visible = False
        txt_KNGN_ID_SAVE.Visible = False
        txt_KNGN_NM.Visible = False
        txt_PWD_UPD_DT.Visible = False
        txt_D_FIRST_LOGIN.Visible = False

        ' 読取専用フィールド
        txt_CREATE_DT.ReadOnly = True
        txt_UPDATE_DT.ReadOnly = True
        txt_ERR_CT.ReadOnly = True
        txt_LAST_ERR_DT.ReadOnly = True

        ' 権限グループComboBoxをバインド
        LoadKngnComboBox()

        txt_Mode.Text = EditMode

        If EditMode = "EDIT" Then
            LoadUserData(TargetUserId)
            txt_CD.ReadOnly = True  ' 編集時はユーザーコード変更不可
        Else
            ' 新規: デフォルト値
            txt_LOGIN_ATTEMPTS.Text = "5"
            txt_PWD_LIFE_TIME.Text = "90"
            txt_PWD_GRACE_TIME.Text = "30"
            txt_PWD_MIN.Text = "8"
        End If
    End Sub

    ''' <summary>
    ''' 権限グループComboBoxにデータをバインド
    ''' </summary>
    Private Sub LoadKngnComboBox()
        Try
            Dim sql As String = "SELECT kngn_id, kngn_nm FROM sec_kngn WHERE history_f = FALSE ORDER BY kngn_cd"
            Dim dt = _crud.GetDataTable(sql)
            cmb_KNGN_ID.DataSource = dt
            cmb_KNGN_ID.ValueMember = "kngn_id"
            cmb_KNGN_ID.DisplayMember = "kngn_nm"
            cmb_KNGN_ID.SelectedIndex = -1
        Catch ex As Exception
            MessageBox.Show("権限グループ読込エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 編集モード: 既存ユーザーデータを読み込んでフォームにセット
    ''' </summary>
    Private Sub LoadUserData(userId As Integer)
        Try
            Dim sql As String = "SELECT * FROM sec_user WHERE user_id = @user_id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@user_id", userId)
            }
            Dim dt = _crud.GetDataTable(sql, prms)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("ユーザーが見つかりません。", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            Dim row = dt.Rows(0)
            txt_ID.Text = row("user_id").ToString()
            txt_CD.Text = If(row("user_cd") IsNot DBNull.Value, row("user_cd").ToString(), "")
            txt_NM.Text = If(row("user_nm") IsNot DBNull.Value, row("user_nm").ToString(), "")
            txt_BIKO.Text = If(row("biko") IsNot DBNull.Value, row("biko").ToString(), "")

            ' 権限グループ選択
            If row("kngn_id") IsNot DBNull.Value Then
                cmb_KNGN_ID.SelectedValue = CInt(row("kngn_id"))
                txt_KNGN_ID_SAVE.Text = row("kngn_id").ToString()
            End If

            ' パスワード関連
            txt_PWD_SAVE.Text = If(row("pwd") IsNot DBNull.Value, row("pwd").ToString(), "")
            txt_LOGIN_ATTEMPTS.Text = If(row("login_attempts") IsNot DBNull.Value, row("login_attempts").ToString(), "5")
            txt_PWD_LIFE_TIME.Text = If(row("pwd_life_time") IsNot DBNull.Value, row("pwd_life_time").ToString(), "90")
            txt_PWD_GRACE_TIME.Text = If(row("pwd_grace_time") IsNot DBNull.Value, row("pwd_grace_time").ToString(), "30")
            txt_PWD_MIN.Text = If(row("pwd_min") IsNot DBNull.Value, row("pwd_min").ToString(), "0")

            ' 文字種チェック
            chk_PWD_MOJI_CHK.Checked = If(row("pwd_moji_chk") IsNot DBNull.Value, CBool(row("pwd_moji_chk")), False)
            chk_PWD_ALPH_CHK.Checked = If(row("pwd_alph_chk") IsNot DBNull.Value, CBool(row("pwd_alph_chk")), False)
            chk_PWD_NUM_CHK.Checked = If(row("pwd_num_chk") IsNot DBNull.Value, CBool(row("pwd_num_chk")), False)
            chk_PWD_SYMBOL_CHK.Checked = If(row("pwd_symbol_chk") IsNot DBNull.Value, CBool(row("pwd_symbol_chk")), False)

            ' 使用不可
            chk_HISTORY_F.Checked = If(row("history_f") IsNot DBNull.Value, CBool(row("history_f")), False)
            txt_HISTORY_F_SAVE.Text = chk_HISTORY_F.Checked.ToString()

            ' エラー情報（読取専用）
            txt_ERR_CT.Text = If(row("err_ct") IsNot DBNull.Value, row("err_ct").ToString(), "0")
            txt_LAST_ERR_DT.Text = If(row("last_err_dt") IsNot DBNull.Value, CDate(row("last_err_dt")).ToString("yyyy/MM/dd HH:mm:ss"), "")

            ' 監査情報（読取専用）
            txt_CREATE_DT.Text = If(row("create_dt") IsNot DBNull.Value, CDate(row("create_dt")).ToString("yyyy/MM/dd HH:mm:ss"), "")
            txt_UPDATE_DT.Text = If(row("update_dt") IsNot DBNull.Value, CDate(row("update_dt")).ToString("yyyy/MM/dd HH:mm:ss"), "")

        Catch ex As Exception
            MessageBox.Show("データ読込エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  登録ボタン
    ' =========================================================
    Private Sub cmd_Touroku_Click(sender As Object, e As EventArgs) Handles cmd_Touroku.Click
        ' --- 入力チェック ---
        If String.IsNullOrWhiteSpace(txt_CD.Text) Then
            MessageBox.Show("利用者コードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_CD.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txt_NM.Text) Then
            MessageBox.Show("利用者名を入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NM.Focus()
            Return
        End If

        If cmb_KNGN_ID.SelectedValue Is Nothing Then
            MessageBox.Show("利用権限を選択してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cmb_KNGN_ID.Focus()
            Return
        End If

        ' --- 自分自身の権限変更防止 (Access版 line 97-101) ---
        If EditMode = "EDIT" AndAlso TargetUserId = LoginSession.LoggedInUserId Then
            If cmb_KNGN_ID.SelectedValue IsNot Nothing AndAlso
               CInt(cmb_KNGN_ID.SelectedValue) <> LoginSession.KngnId Then
                MessageBox.Show("自分自身の権限グループは変更できません。", "制限",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        ' --- パスワード入力がある場合の検証 ---
        Dim hasNewPassword As Boolean = Not String.IsNullOrEmpty(txt_PWD_INP.Text)
        If hasNewPassword Then
            ' 確認入力一致チェック
            If txt_PWD_INP.Text <> txt_PWD_INP_RETRY.Text Then
                MessageBox.Show("パスワードとパスワード(確認)が一致しません。", "入力エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_PWD_INP_RETRY.Focus()
                Return
            End If

            ' PasswordValidator で文字種チェック
            Dim policy As New PasswordValidator.PasswordPolicy()
            policy.PwdMin = If(Integer.TryParse(txt_PWD_MIN.Text, Nothing), CInt(txt_PWD_MIN.Text), 0)
            policy.PwdMojiChk = chk_PWD_MOJI_CHK.Checked
            policy.PwdAlphChk = chk_PWD_ALPH_CHK.Checked
            policy.PwdNumChk = chk_PWD_NUM_CHK.Checked
            policy.PwdSymbolChk = chk_PWD_SYMBOL_CHK.Checked

            Dim valResult = PasswordValidator.Validate(txt_PWD_INP.Text, policy)
            If Not valResult.IsValid Then
                MessageBox.Show(valResult.ErrorMessage, "パスワードエラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_PWD_INP.Focus()
                Return
            End If
        End If

        ' --- DB登録 ---
        Try
            If EditMode = "NEW" Then
                InsertUser(hasNewPassword)
            Else
                UpdateUser(hasNewPassword)
            End If

            MessageBox.Show("登録しました。", "完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("登録エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InsertUser(hasNewPassword As Boolean)
        ' ユーザーコード重複チェック
        Dim chkSql As String = "SELECT COUNT(*) FROM sec_user WHERE user_cd = @user_cd"
        Dim chkPrms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@user_cd", txt_CD.Text.Trim())
        }
        Dim cnt As Integer = _crud.ExecuteScalar(Of Integer)(chkSql, chkPrms)
        If cnt > 0 Then
            Throw New Exception("利用者コード '" & txt_CD.Text.Trim() & "' は既に登録されています。")
        End If

        Dim sql As String =
            "INSERT INTO sec_user (user_cd, user_nm, kngn_id, pwd, biko, " &
            "login_attempts, pwd_life_time, pwd_grace_time, pwd_min, " &
            "pwd_moji_chk, pwd_alph_chk, pwd_num_chk, pwd_symbol_chk, " &
            "history_f, err_ct, create_dt, update_dt) " &
            "VALUES (@user_cd, @user_nm, @kngn_id, @pwd, @biko, " &
            "@login_attempts, @pwd_life_time, @pwd_grace_time, @pwd_min, " &
            "@pwd_moji_chk, @pwd_alph_chk, @pwd_num_chk, @pwd_symbol_chk, " &
            "@history_f, 0, @now, @now)"

        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@user_cd", txt_CD.Text.Trim()),
            New NpgsqlParameter("@user_nm", txt_NM.Text.Trim()),
            New NpgsqlParameter("@kngn_id", CInt(cmb_KNGN_ID.SelectedValue)),
            New NpgsqlParameter("@pwd", If(hasNewPassword, ComputeSha256Hash(txt_PWD_INP.Text), CObj(DBNull.Value))),
            New NpgsqlParameter("@biko", txt_BIKO.Text.Trim()),
            New NpgsqlParameter("@login_attempts", ParseIntOrDefault(txt_LOGIN_ATTEMPTS.Text, 5)),
            New NpgsqlParameter("@pwd_life_time", ParseIntOrDefault(txt_PWD_LIFE_TIME.Text, 90)),
            New NpgsqlParameter("@pwd_grace_time", ParseIntOrDefault(txt_PWD_GRACE_TIME.Text, 30)),
            New NpgsqlParameter("@pwd_min", ParseIntOrDefault(txt_PWD_MIN.Text, 0)),
            New NpgsqlParameter("@pwd_moji_chk", chk_PWD_MOJI_CHK.Checked),
            New NpgsqlParameter("@pwd_alph_chk", chk_PWD_ALPH_CHK.Checked),
            New NpgsqlParameter("@pwd_num_chk", chk_PWD_NUM_CHK.Checked),
            New NpgsqlParameter("@pwd_symbol_chk", chk_PWD_SYMBOL_CHK.Checked),
            New NpgsqlParameter("@history_f", chk_HISTORY_F.Checked),
            New NpgsqlParameter("@now", DateTime.Now)
        }
        _crud.ExecuteNonQuery(sql, prms)
    End Sub

    Private Sub UpdateUser(hasNewPassword As Boolean)
        Dim sb As New StringBuilder()
        sb.Append("UPDATE sec_user SET user_nm = @user_nm, kngn_id = @kngn_id, ")
        sb.Append("biko = @biko, login_attempts = @login_attempts, ")
        sb.Append("pwd_life_time = @pwd_life_time, pwd_grace_time = @pwd_grace_time, ")
        sb.Append("pwd_min = @pwd_min, ")
        sb.Append("pwd_moji_chk = @pwd_moji_chk, pwd_alph_chk = @pwd_alph_chk, ")
        sb.Append("pwd_num_chk = @pwd_num_chk, pwd_symbol_chk = @pwd_symbol_chk, ")
        sb.Append("history_f = @history_f, update_dt = @now")
        If hasNewPassword Then
            sb.Append(", pwd = @pwd, pwd_upd_dt = @now")
        End If
        sb.Append(" WHERE user_id = @user_id")

        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@user_nm", txt_NM.Text.Trim()),
            New NpgsqlParameter("@kngn_id", CInt(cmb_KNGN_ID.SelectedValue)),
            New NpgsqlParameter("@biko", txt_BIKO.Text.Trim()),
            New NpgsqlParameter("@login_attempts", ParseIntOrDefault(txt_LOGIN_ATTEMPTS.Text, 5)),
            New NpgsqlParameter("@pwd_life_time", ParseIntOrDefault(txt_PWD_LIFE_TIME.Text, 90)),
            New NpgsqlParameter("@pwd_grace_time", ParseIntOrDefault(txt_PWD_GRACE_TIME.Text, 30)),
            New NpgsqlParameter("@pwd_min", ParseIntOrDefault(txt_PWD_MIN.Text, 0)),
            New NpgsqlParameter("@pwd_moji_chk", chk_PWD_MOJI_CHK.Checked),
            New NpgsqlParameter("@pwd_alph_chk", chk_PWD_ALPH_CHK.Checked),
            New NpgsqlParameter("@pwd_num_chk", chk_PWD_NUM_CHK.Checked),
            New NpgsqlParameter("@pwd_symbol_chk", chk_PWD_SYMBOL_CHK.Checked),
            New NpgsqlParameter("@history_f", chk_HISTORY_F.Checked),
            New NpgsqlParameter("@now", DateTime.Now),
            New NpgsqlParameter("@user_id", TargetUserId)
        }
        If hasNewPassword Then
            prms.Add(New NpgsqlParameter("@pwd", ComputeSha256Hash(txt_PWD_INP.Text)))
        End If

        _crud.ExecuteNonQuery(sb.ToString(), prms)
    End Sub

    ' =========================================================
    '  削除ボタン（論理削除）
    ' =========================================================
    Private Sub cmd_Del_Click(sender As Object, e As EventArgs) Handles cmd_Del.Click
        If EditMode <> "EDIT" Then Return

        ' 自分自身の削除防止
        If TargetUserId = LoginSession.LoggedInUserId Then
            MessageBox.Show("自分自身は削除できません。", "制限",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result = MessageBox.Show("このユーザーを使用不可にしますか？", "確認",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result <> DialogResult.Yes Then Return

        Try
            Dim sql As String = "UPDATE sec_user SET history_f = TRUE, update_dt = @now WHERE user_id = @user_id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@now", DateTime.Now),
                New NpgsqlParameter("@user_id", TargetUserId)
            }
            _crud.ExecuteNonQuery(sql, prms)

            MessageBox.Show("使用不可に設定しました。", "完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  閉じるボタン
    ' =========================================================
    Private Sub cmd_Close_Click(sender As Object, e As EventArgs) Handles cmd_Close.Click
        Me.Close()
    End Sub

    ' =========================================================
    '  ヘルパー
    ' =========================================================
    Private Shared Function ComputeSha256Hash(input As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function

    Private Shared Function ParseIntOrDefault(text As String, defaultValue As Integer) As Integer
        Dim result As Integer
        If Integer.TryParse(text, result) Then
            Return result
        End If
        Return defaultValue
    End Function

End Class
