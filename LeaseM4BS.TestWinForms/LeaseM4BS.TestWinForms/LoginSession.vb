' =========================================================
' LoginSession - グローバルユーザーセッション
' Access版 typ_gLogin 構造体に相当
' ログイン成功時にセットし、アプリ全体で参照する
' =========================================================
Imports System.Data
Imports System.Linq
Imports LeaseM4BS.DataAccess
Imports Npgsql


''' <summary>
''' 顧客タイプ識別子 (Access版 engCUSTM_TYPE Enum に相当)
''' p_Customize.txt 定義準拠。26種類の顧客固有カスタマイズを識別する。
''' </summary>
Public Enum igCUSTM_TYPE As Integer
    STD = 0             ' 標準
    DKO = 1             ' DKO
    DNS = 2             ' DNS
    VTC = 3             ' VTC
    MYCOM = 4           ' マイコム
    NIFS = 5            ' NIFS
    SNKO = 6            ' 三光
    RISO = 7            ' リソ
    ' 8 は欠番
    SANKO_AIR = 9       ' 三光エアー
    SAKURA_IS = 10      ' 桜インターナショナル
    YAMASHIN_F = 11     ' 山信F
    KITOKU = 12         ' 木徳
    FUJISASH = 13       ' 富士佐
    TCCB = 14           ' TCCB
    NIDEC_SHIBA = 15    ' 日電柴
    JOT = 16            ' JOT
    KYOTO = 17          ' 京都
    MARUZEN = 18        ' 丸善
    NKSOL = 19          ' NK-SOL
    TSYSCOM = 20        ' TSYSCOM
    KINTETSU_IS = 21    ' 近鉄IS
    VALQUA = 22         ' VALQUA
    NIPPAN_R = 23       ' 日版R
    CACMARUHA = 24      ' キャッチ丸葉
    KINTETSU_RE = 25    ' 近鉄RE
    STD_SWK = 26        ' 標準_仕訳
End Enum
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

    ' --- 顧客タイプ (Access版 igCUSTM_TYPE に相当) ---
    Public CustomerType As igCUSTM_TYPE = igCUSTM_TYPE.STD

    ' --- DB環境情報 (Access版 sgDB_NAME に相当) ---
    Public DatabaseName As String = ""

    ' --- セッション状態 ---
    Public LoginDateTime As DateTime = DateTime.MinValue
    Public IsSessionActive As Boolean = False

    ' --- パスワード関連 (Access版 gPWD_KIGEN チェックに相当) ---
    Public PasswordExpireDate As DateTime = DateTime.MinValue
    Public IsFirstLogin As Boolean = False

    ' --- 契約管理単位別権限 (Access版 tgKKNRI_LIST に相当) ---
    Public KknriList As New List(Of KknriAccessEntry)()
    ' --- 部門管理単位別権限 (Access版 tgBKNRI_LIST に相当) ---
    Public BknriList As New List(Of BknriAccessEntry)()

    ' --- パスワードポリシー (Access版 typ_gLogin のパスワード関連フィールド) ---
    Public PasswordMinLength As Integer = 0       ' PWD_MIN
    Public PwdMojiChk As Boolean = False          ' PWD_MOJI_CHK
    Public PwdAlphChk As Boolean = False          ' PWD_ALPH_CHK
    Public PwdNumChk As Boolean = False           ' PWD_NUM_CHK
    Public PwdSymbolChk As Boolean = False        ' PWD_SYMBOL_CHK
    Public PwdLifeTime As Integer = 0             ' PWD_LIFE_TIME
    Public PwdGraceTime As Integer = 0            ' PWD_GRACE_TIME
    Public PwdUpdDt As DateTime? = Nothing        ' PWD_UPD_DT

    ' --- 管理単位別権限の構造体 ---
    Public Structure KknriAccessEntry
        Public KknriId As Integer
        Public AccessKind As Integer  ' 1=更新, 2=参照
    End Structure

    Public Structure BknriAccessEntry
        Public BknriId As Integer
        Public AccessKind As Integer  ' 1=更新, 2=参照
    End Structure

    ' --- 操作ログ種別定数 (Access版 p_Com_Const.txt engOP_KBN 準拠) ---
    ' Access版は3桁ゼロパディング数値（Format(value, "000")）
    ' --- 契約・データ操作系 ---
    Public Const OP_KBN_KYKH As String = "001"              ' リース契約
    Public Const OP_KBN_KYKM As String = "002"              ' リース期間
    Public Const OP_KBN_KYKM_HAIF As String = "003"         ' リース期間(配分案1)
    Public Const OP_KBN_HENF As String = "004"              ' 変更計画情報
    Public Const OP_KBN_GSON As String = "005"              ' 月額
    Public Const OP_KBN_KYKH_ADD As String = "006"          ' リース契約新規完成
    Public Const OP_KBN_KYKM_BKN As String = "007"          ' リース期間(事業部管理用)
    ' --- 計算・処理系 ---
    Public Const OP_KBN_TOUGETSU As String = "011"          ' 元利均等計算
    Public Const OP_KBN_KEIJO As String = "012"             ' 計上/決定
    ' --- 帳票・参照系 ---
    Public Const OP_KBN_TANA As String = "021"              ' 棚卸資産表示
    Public Const OP_KBN_KLSRYO As String = "022"            ' 賃借料収入内訳表
    Public Const OP_KBN_IDOLST As String = "023"            ' 異動リスト表示
    Public Const OP_KBN_KHIYO As String = "024"             ' 当期利用料費用明細表
    Public Const OP_KBN_YOSAN As String = "025"             ' 予算シミュレーション
    Public Const OP_KBN_KEIHIM As String = "028"            ' 費用内訳表
    Public Const OP_KBN_CHUKI As String = "031"             ' 長期継続事業リース判定一覧
    Public Const OP_KBN_ZANDAKA As String = "032"           ' リース料残高リスト表示
    Public Const OP_KBN_SAIMU As String = "033"             ' リース債務内訳リスト表
    Public Const OP_KBN_BEPPYO As String = "034"            ' 別表16(4)2
    Public Const OP_KBN_BEPPYO_FLX As String = "035"        ' 別表16(4)2(フレックス)
    ' --- マスタメンテナンス系 ---
    Public Const OP_KBN_HOLIDAY As String = "251"           ' 祝日設定
    Public Const OP_KBN_KARI_RITU As String = "252"         ' 追加利率情報管理
    Public Const OP_KBN_ZEI_KAISEI As String = "253"        ' 税制改正情報
    Public Const OP_KBN_HREL As String = "254"              ' 割引率設定
    Public Const OP_KBN_KYKBNJ_SEQ As String = "255"        ' 契約種類管理番号
    Public Const OP_KBN_TC_SWK_DEF_COM As String = "256"    ' 割当配賦値設定
    ' --- バッチ・一括処理系 ---
    Public Const OP_KBN_CHUKI_RECALC As String = "301"      ' 長期継続再計算
    Public Const OP_KBN_IDO As String = "302"               ' リース異動
    Public Const OP_KBN_LEASE_TORIKOMI As String = "303"    ' レース取込
    Public Const OP_KBN_KEIYAKU_IDO_TORIKOMI As String = "304" ' 契約異動取込
    ' --- バックアップ・復元系 ---
    Public Const OP_KBN_BACKUP As String = "401"            ' 操作バックアップ
    Public Const OP_KBN_RESTORE As String = "402"           ' バックアップ復元
    Public Const OP_KBN_EXCELIMP_ADD As String = "403"      ' EXCEL追加取込
    Public Const OP_KBN_EXCELIMP_NEW As String = "404"      ' EXCEL取込(新規作成)
    Public Const OP_KBN_FLEX_RESTOR As String = "407"       ' フレックスリスト登録データ復元
    Public Const OP_KBN_USERPASSWORD As String = "408"      ' システム利用者パスワード変更
    Public Const OP_KBN_GASSAN As String = "411"            ' 統計データ合算
    Public Const OP_KBN_ZENSAKUJO As String = "412"         ' 全リース契約削除
    Public Const OP_KBN_DATACONVERT As String = "421"       ' データ移行時点データ変換
    Public Const OP_KBN_DATAPASSWORD As String = "422"      ' データパスワード設定
    ' --- 設定系 ---
    Public Const OP_KBN_SETTEI As String = "601"            ' 利用設定
    Public Const OP_KBN_TOUSEIOPT As String = "602"         ' 統制オプション設定
    Public Const OP_KBN_SWK_DEF_COM As String = "611"       ' 割当配賦値設定
    ' --- ログ参照系 ---
    Public Const OP_KBN_SLOG As String = "621"              ' 操作ログ
    Public Const OP_KBN_ULOG As String = "622"              ' 更新ログ
    Public Const OP_KBN_BKLOG As String = "623"             ' 保存/復元ログ
    Public Const OP_KBN_LOGDEL As String = "624"            ' 操作/更新ログ削除
    ' --- グループ参照系 ---
    Public Const OP_KBN_KYKH_G As String = "801"            ' リース契約一覧
    Public Const OP_KBN_KYKM_G As String = "802"            ' リース期間一覧
    Public Const OP_KBN_KYKM_CHUKI As String = "803"        ' 中期判断リース
    Public Const OP_KBN_CHUKI_SCH As String = "804"         ' 計数スクリーン
    Public Const OP_KBN_ZANDAKA_SCH As String = "805"       ' 計数スクリーン
    Public Const OP_KBN_SAIMU_SCH As String = "806"         ' 計数スクリーン
    ' --- システム系 ---
    Public Const OP_KBN_SYSSTART As String = "901"          ' システム開始
    Public Const OP_KBN_SYSEND As String = "902"            ' システム終了
    Public Const OP_KBN_LOGIN As String = "911"             ' ログイン
    Public Const OP_KBN_LOGOFF As String = "912"            ' ログオフ
    Public Const OP_KBN_LOGINERR As String = "913"          ' ログインエラー
    Public Const OP_KBN_SONOTA As String = "999"            ' その他

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

        ' --- 追加: 管理単位別権限の読込 (Access版 gSetPublic_KNGN 行860-894 相当) ---
        Try
            LoadKknriPermissions(kngnIdParam, crud)
            LoadBknriPermissions(kngnIdParam, crud)
        Catch ex As Exception
            ' 失敗しても認証フローは止めない（空リストで続行）
            KknriList = New List(Of KknriAccessEntry)()
            BknriList = New List(Of BknriAccessEntry)()
        End Try
    End Sub

    ''' <summary>
    ''' sec_kngn_kknri テーブルから契約管理単位別権限を読み込む
    ''' Access版 pc_StartUp.txt 行861-876 相当
    ''' </summary>
    Private Sub LoadKknriPermissions(kngnIdParam As Integer, crud As CrudHelper)
        KknriList = New List(Of KknriAccessEntry)()
        Dim sql As String = "SELECT kknri_id, access_kind FROM sec_kngn_kknri WHERE kngn_id = @kngn_id"
        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@kngn_id", kngnIdParam)
        }
        Dim dt = crud.GetDataTable(sql, prms)
        For Each r As DataRow In dt.Rows
            Dim entry As New KknriAccessEntry()
            entry.KknriId = If(r("kknri_id") IsNot DBNull.Value, CInt(r("kknri_id")), 0)
            entry.AccessKind = If(r("access_kind") IsNot DBNull.Value, CInt(r("access_kind")), 0)
            KknriList.Add(entry)
        Next
    End Sub

    ''' <summary>
    ''' sec_kngn_bknri テーブルから部門管理単位別権限を読み込む
    ''' Access版 pc_StartUp.txt 行879-894 相当
    ''' </summary>
    Private Sub LoadBknriPermissions(kngnIdParam As Integer, crud As CrudHelper)
        BknriList = New List(Of BknriAccessEntry)()
        Dim sql As String = "SELECT bknri_id, access_kind FROM sec_kngn_bknri WHERE kngn_id = @kngn_id"
        Dim prms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@kngn_id", kngnIdParam)
        }
        Dim dt = crud.GetDataTable(sql, prms)
        For Each r As DataRow In dt.Rows
            Dim entry As New BknriAccessEntry()
            entry.BknriId = If(r("bknri_id") IsNot DBNull.Value, CInt(r("bknri_id")), 0)
            entry.AccessKind = If(r("access_kind") IsNot DBNull.Value, CInt(r("access_kind")), 0)
            BknriList.Add(entry)
        Next
    End Sub

    ''' <summary>
    ''' sec_user テーブルからパスワードポリシー情報を読み込む
    ''' Access版 gSetPublic_KNGN 行803-840 相当
    ''' </summary>
    Public Sub LoadPasswordPolicy(userIdParam As Integer)
        Try
            Dim crud As New CrudHelper()
            Dim sql As String = "SELECT pwd_min, pwd_moji_chk, pwd_alph_chk, pwd_num_chk, pwd_symbol_chk, " &
                                "pwd_life_time, pwd_grace_time, pwd_upd_dt " &
                                "FROM sec_user WHERE user_id = @user_id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@user_id", userIdParam)
            }
            Dim dt = crud.GetDataTable(sql, prms)
            If dt.Rows.Count = 0 Then Return

            Dim row = dt.Rows(0)
            PasswordMinLength = If(row("pwd_min") IsNot DBNull.Value, CInt(row("pwd_min")), 0)
            PwdMojiChk = If(row("pwd_moji_chk") IsNot DBNull.Value, CBool(row("pwd_moji_chk")), False)
            PwdAlphChk = If(row("pwd_alph_chk") IsNot DBNull.Value, CBool(row("pwd_alph_chk")), False)
            PwdNumChk = If(row("pwd_num_chk") IsNot DBNull.Value, CBool(row("pwd_num_chk")), False)
            PwdSymbolChk = If(row("pwd_symbol_chk") IsNot DBNull.Value, CBool(row("pwd_symbol_chk")), False)
            PwdLifeTime = If(row("pwd_life_time") IsNot DBNull.Value, CInt(row("pwd_life_time")), 0)
            PwdGraceTime = If(row("pwd_grace_time") IsNot DBNull.Value, CInt(row("pwd_grace_time")), 0)
            PwdUpdDt = If(row("pwd_upd_dt") IsNot DBNull.Value, CDate(row("pwd_upd_dt")), CType(Nothing, DateTime?))
        Catch ex As Exception
            ' パスワードポリシー読込失敗はスキップ
            Console.WriteLine($"[LoadPasswordPolicy] 読込失敗: {ex.Message}")
        End Try
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
        ' 管理単位別権限
        KknriList = New List(Of KknriAccessEntry)()
        BknriList = New List(Of BknriAccessEntry)()
        CustomerType = igCUSTM_TYPE.STD
        ' パスワードポリシー
        PasswordMinLength = 0
        PwdMojiChk = False
        PwdAlphChk = False
        PwdNumChk = False
        PwdSymbolChk = False
        PwdLifeTime = 0
        PwdGraceTime = 0
        PwdUpdDt = Nothing
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


    Public Sub LoadCustomerType()
        Try
            Dim crud As New CrudHelper()
            Dim sql As String = "SELECT custm_type FROM t_customize LIMIT 1"
            Dim dt = crud.GetDataTable(sql)
            If dt.Rows.Count = 0 Then
                CustomerType = igCUSTM_TYPE.STD
            Else
                Dim row = dt.Rows(0)
                Dim rawValue As Integer = If(row("custm_type") IsNot DBNull.Value, CInt(row("custm_type")), 0)
                If [Enum].IsDefined(GetType(igCUSTM_TYPE), rawValue) Then
                    CustomerType = CType(rawValue, igCUSTM_TYPE)
                Else
                    CustomerType = igCUSTM_TYPE.STD
                End If
            End If
        Catch ex As Exception
            CustomerType = igCUSTM_TYPE.STD
        End Try
    End Sub
    ''' <summary>
    ''' 操作ログをDBに記録する（Access版 olSLOG.OutputSLOG 完全準拠）
    ''' fgNT_SECF(IsSessionActive) および fgNT_SLOGOUT(EnableSystemLog) チェック付き
    ''' </summary>
    ''' <param name="opKbn">操作種別コード（3桁ゼロパディング: OP_KBN_* 定数）</param>
    ''' <param name="detail">操作詳細（op_detail1 に記録、最大255バイト）</param>
    ''' <param name="opNm">操作名（op_nm に記録）。省略時は空文字</param>
    ''' <param name="opS">操作内容（op_s に記録: "新規","更新","削除" 等）。省略時は空文字</param>
    ''' <param name="detail2">操作詳細2（op_detail2 に記録、長文対応）。省略時は空文字</param>
    ''' <param name="updSbt">更新小計（upd_sbt に記録）。省略時は空文字</param>
    ''' <returns>採番された slog_no。失敗時は -1</returns>

    Public Function WriteAuditLog(opKbn As String,
                                  detail As String,
                                  Optional opNm As String = "",
                                  Optional opS As String = "",
                                  Optional detail2 As String = "",
                                  Optional updSbt As String = "") As Integer
        Try
            ' Access版 OutputSLOG 準拠: fgNT_SLOGOUT チェック
            ' ただしシステム開始/終了・ログイン/ログオフはフラグに関係なく記録
            Dim isSystemOp As Boolean = (opKbn = OP_KBN_SYSSTART OrElse opKbn = OP_KBN_SYSEND OrElse
                                         opKbn = OP_KBN_LOGIN OrElse opKbn = OP_KBN_LOGOFF OrElse
                                         opKbn = OP_KBN_LOGINERR)
            If Not isSystemOp AndAlso Not EnableSystemLog Then
                Return -1
            End If

            Dim now As DateTime = DateTime.Now
            Dim crud As New CrudHelper()
            Dim sql As String = "INSERT INTO l_slog (op_st_dt, op_en_dt, op_kbn, op_nm, op_s, " &
                                "op_user_cd, op_user_nm, pc_name, ip_adr, win_user, " &
                                "op_detail1, op_detail2, upd_sbt) " &
                                "VALUES (@op_st_dt, @op_en_dt, @op_kbn, @op_nm, @op_s, " &
                                "@op_user_cd, @op_user_nm, @pc_name, @ip_adr, @win_user, " &
                                "@op_detail1, @op_detail2, @upd_sbt) " &
                                "RETURNING slog_no"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@op_st_dt", now),
                New NpgsqlParameter("@op_en_dt", now),
                New NpgsqlParameter("@op_kbn", opKbn),
                New NpgsqlParameter("@op_nm", If(opNm, "")),
                New NpgsqlParameter("@op_s", If(opS, "")),
                New NpgsqlParameter("@op_user_cd", If(LoggedInUserCd, "")),
                New NpgsqlParameter("@op_user_nm", If(LoggedInUserNm, "")),
                New NpgsqlParameter("@pc_name", Environment.MachineName),
                New NpgsqlParameter("@ip_adr", GetLocalIpAddress()),
                New NpgsqlParameter("@win_user", Environment.UserName),
                New NpgsqlParameter("@op_detail1", If(detail, "")),
                New NpgsqlParameter("@op_detail2", If(detail2, "")),
                New NpgsqlParameter("@upd_sbt", If(updSbt, ""))
            }
            Dim slogNo As Integer = crud.ExecuteScalar(Of Integer)(sql, prms)
            Return slogNo
        Catch ex As Exception
            ' ログ記録失敗は握りつぶす（業務処理に影響させない）
            Console.WriteLine($"[WriteAuditLog] ログ記録失敗: {ex.Message}")
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' ローカルIPアドレスを取得する（Access版 vmIP_ADR 相当、複数の場合カンマ区切り）
    ''' </summary>
    Public Function GetLocalIpAddress() As String
        Try
            Dim host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
            Dim ips = host.AddressList.
                Where(Function(a) a.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork).
                Select(Function(a) a.ToString())
            Dim result = String.Join(",", ips)
            ' Access版: 100バイト制限
            If result.Length > 100 Then result = result.Substring(0, 100)
            Return result
        Catch
            Return ""
        End Try
    End Function

End Module
