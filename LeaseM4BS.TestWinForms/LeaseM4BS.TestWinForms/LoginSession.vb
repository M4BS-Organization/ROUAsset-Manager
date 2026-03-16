' =========================================================
' LoginSession - グローバルユーザーセッション
' Access版 typ_gLogin 構造体に相当
' ログイン成功時にセットし、アプリ全体で参照する
' =========================================================
Imports LeaseM4BS.DataAccess
Imports Npgsql

Public Module LoginSession

    ' --- ユーザー基本情報 ---
    Public LoggedInUserId As Integer = 0
    Public LoggedInUserCd As String = ""
    Public LoggedInUserNm As String = ""

    ' --- 権限グループ情報 (sec_kngn) ---
    Public KngnId As Integer = 0
    Public KngnCd As String = ""
    Public KngnNm As String = ""

    ' --- 権限フラグ (Access版 typ_gLogin の boXxx に相当) ---
    Public AccessKind As Integer = 0       ' 1:全データ変更 2:全データ参照 3:管理単位限定
    Public AccessKindB As Integer = 0      ' 物件管理単位のアクセス種別
    Public IsAdmin As Boolean = False      ' システム管理者権限
    Public CanMasterUpdate As Boolean = False  ' マスタ更新権限
    Public CanFileOutput As Boolean = False    ' ファイル出力権限
    Public CanPrint As Boolean = False         ' 印刷権限
    Public CanLogRef As Boolean = False        ' ログ参照権限
    Public CanApproval As Boolean = False      ' 承認権限

    ' --- ユーザーセット初期化フラグ (Gap 5: Access版 gInitUserSet に相当) ---
    Public CurrentUserSetLoaded As Boolean = False

    ' --- 月次オプション (Gap 6: Access版 GetTousei_OPT に相当) ---
    Public EnableSystemLog As Boolean = False   ' Access版 fgNT_SLOGOUT
    Public EnableUserLog As Boolean = False     ' Access版 fgNT_ULOGOUT
    Public EnableRecordLog As Boolean = False   ' Access版 fgNT_RECOUT
    Public EnableConversionLog As Boolean = False ' Access版 fgNT_DTCNVLOG

    ' --- DBバージョン情報 (Gap 4) ---
    Public DbVersion As String = ""

    ' --- DB環境情報 (Access版 sgDB_NAME に相当) ---
    Public DatabaseName As String = ""

    ' --- セッション状態 ---
    Public LoginDateTime As DateTime = DateTime.MinValue
    Public IsSessionActive As Boolean = False

    ' --- パスワード関連 (Access版 gPWD_KIGEN チェックに相当) ---
    Public PasswordExpireDate As DateTime = DateTime.MinValue
    Public IsFirstLogin As Boolean = False

    ' --- 操作ログ種別定数 (Access版 engOP_KBN に相当) ---
    Public Const OP_LOGIN As String = "LOGIN"            ' ログイン成功
    Public Const OP_LOGIN_ERR As String = "LOGIN_ERR"    ' ログイン失敗
    Public Const OP_LOGOUT As String = "LOGOUT"          ' ログアウト
    Public Const OP_PWD_CHANGE As String = "PWD_CHANGE"  ' パスワード変更

    ''' <summary>
    ''' ログイン成功時に sec_kngn から権限情報を取得してセットする
    ''' </summary>
    Public Sub LoadPermissions(kngnIdParam As Integer)
        Dim crud As New CrudHelper()
        Dim sql As String = "SELECT kngn_id, kngn_cd, kngn_nm, access_kind, access_kind_b, " &
                            "admin, master_update, file_output, print, log_ref, approval " &
                            "FROM sec_kngn WHERE kngn_id = @kngn_id AND history_f = FALSE"
        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@kngn_id", kngnIdParam)
        }

        Dim dt = crud.GetDataTable(sql, prms)
        If dt.Rows.Count = 0 Then Return

        Dim row = dt.Rows(0)
        LoginSession.KngnId = kngnIdParam
        LoginSession.KngnCd = If(row("kngn_cd") IsNot DBNull.Value, row("kngn_cd").ToString(), "")
        LoginSession.KngnNm = If(row("kngn_nm") IsNot DBNull.Value, row("kngn_nm").ToString(), "")
        LoginSession.AccessKind = If(row("access_kind") IsNot DBNull.Value, CInt(row("access_kind")), 0)
        LoginSession.AccessKindB = If(row("access_kind_b") IsNot DBNull.Value, CInt(row("access_kind_b")), 0)
        LoginSession.IsAdmin = If(row("admin") IsNot DBNull.Value, CBool(row("admin")), False)
        LoginSession.CanMasterUpdate = If(row("master_update") IsNot DBNull.Value, CBool(row("master_update")), False)
        LoginSession.CanFileOutput = If(row("file_output") IsNot DBNull.Value, CBool(row("file_output")), False)
        LoginSession.CanPrint = If(row("print") IsNot DBNull.Value, CBool(row("print")), False)
        LoginSession.CanLogRef = If(row("log_ref") IsNot DBNull.Value, CBool(row("log_ref")), False)
        LoginSession.CanApproval = If(row("approval") IsNot DBNull.Value, CBool(row("approval")), False)
    End Sub

    ''' <summary>
    ''' ログアウト時にセッション情報をクリアする
    ''' </summary>
    Public Sub Clear()
        LoggedInUserId = 0
        LoggedInUserCd = ""
        LoggedInUserNm = ""
        KngnId = 0
        KngnCd = ""
        KngnNm = ""
        AccessKind = 0
        AccessKindB = 0
        IsAdmin = False
        CanMasterUpdate = False
        CanFileOutput = False
        CanPrint = False
        CanLogRef = False
        CanApproval = False
        ' Gap 5: ユーザーセット初期化フラグ
        CurrentUserSetLoaded = False
        ' Gap 6: 月次オプション
        EnableSystemLog = False
        EnableUserLog = False
        EnableRecordLog = False
        EnableConversionLog = False
        ' Gap 4: DBバージョン
        DbVersion = ""
        ' DB環境情報
        DatabaseName = ""
        ' セッション状態
        LoginDateTime = DateTime.MinValue
        IsSessionActive = False
        ' パスワード関連
        PasswordExpireDate = DateTime.MinValue
        IsFirstLogin = False
    End Sub

    ''' <summary>
    ''' ユーザーセット初期化 (Gap 5: Access版 gInitUserSet に相当)
    ''' sec_kngn と sec_user のマスタをメモリにロードし、フラグをセットする
    ''' </summary>
    Public Sub InitUserSet()
        Try
            Dim crud As New CrudHelper()

            ' sec_kngn マスタの存在確認（権限情報はLoadPermissionsで既にロード済み）
            ' ここでは追加のユーザーセット情報をロードする
            Dim sql As String = "SELECT COUNT(*) FROM sec_kngn WHERE history_f = FALSE"
            Dim kngnCount As Integer = crud.ExecuteScalar(Of Integer)(sql)

            Dim sql2 As String = "SELECT COUNT(*) FROM sec_user WHERE history_f = FALSE"
            Dim userCount As Integer = crud.ExecuteScalar(Of Integer)(sql2)

            ' マスタが正常にロードできた場合はフラグをセット
            If kngnCount > 0 AndAlso userCount > 0 Then
                CurrentUserSetLoaded = True
            Else
                CurrentUserSetLoaded = False
            End If
        Catch ex As Exception
            ' テーブルが存在しない等の場合はスキップ
            CurrentUserSetLoaded = False
        End Try
    End Sub

    ''' <summary>
    ''' 月次オプション読込 (Gap 6: Access版 GetTousei_OPT に相当)
    ''' T_OPT テーブルからログ出力フラグ等を読み込む
    ''' </summary>
    Public Sub LoadTouseiOptions()
        Try
            Dim crud As New CrudHelper()
            Dim sql As String = "SELECT slog, ulog, recopt, cnvlog FROM t_opt LIMIT 1"
            Dim dt = crud.GetDataTable(sql)

            If dt.Rows.Count = 0 Then
                ' レコードなし: デフォルト値（全てFalse）のまま
                EnableSystemLog = False
                EnableUserLog = False
                EnableRecordLog = False
                EnableConversionLog = False
            Else
                Dim row = dt.Rows(0)
                EnableSystemLog = If(row("slog") IsNot DBNull.Value, CBool(row("slog")), False)
                EnableUserLog = If(row("ulog") IsNot DBNull.Value, CBool(row("ulog")), False)
                EnableRecordLog = If(row("recopt") IsNot DBNull.Value, CBool(row("recopt")), False)
                EnableConversionLog = If(row("cnvlog") IsNot DBNull.Value, CBool(row("cnvlog")), False)
            End If
        Catch ex As Exception
            ' T_OPT テーブルが存在しない場合はスキップ（デフォルト値のまま）
            EnableSystemLog = False
            EnableUserLog = False
            EnableRecordLog = False
            EnableConversionLog = False
        End Try
    End Sub

    ''' <summary>
    ''' 操作ログをDBに記録する（Access版 olSLOG.OutputSLOG 相当）
    ''' </summary>
    ''' <param name="operationType">操作種別（OP_LOGIN, OP_LOGIN_ERR, OP_LOGOUT, OP_PWD_CHANGE）</param>
    ''' <param name="detail">操作詳細</param>
    Public Sub WriteAuditLog(operationType As String, detail As String)
        Try
            Dim crud As New CrudHelper()
            Dim sql As String = "INSERT INTO sec_slog (user_id, user_cd, op_kbn, op_detail, op_dt) " &
                                "VALUES (@user_id, @user_cd, @op_kbn, @op_detail, @op_dt)"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@user_id", LoggedInUserId),
                New NpgsqlParameter("@user_cd", If(LoggedInUserCd, "")),
                New NpgsqlParameter("@op_kbn", operationType),
                New NpgsqlParameter("@op_detail", detail),
                New NpgsqlParameter("@op_dt", DateTime.Now)
            }
            crud.ExecuteNonQuery(sql, prms)
        Catch ex As Exception
            ' ログ記録失敗は握りつぶす（業務処理に影響させない）
            ' デバッグ時はConsoleに出力
            Console.WriteLine($"[WriteAuditLog] ログ記録失敗: {ex.Message}")
        End Try
    End Sub

End Module
