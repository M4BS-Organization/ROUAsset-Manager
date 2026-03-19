Imports System.Data
Imports System.IO
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' fc_系顧客固有仕訳出力フォームの共通基底クラス
''' Access版 fc_系フォームの共通処理を抽象化
''' Form_f_仕訳出力標準_KJ/SM/SH の実装パターンを踏襲
''' </summary>
Public MustInherit Class FcJournalOutputBase
    Inherits Form

    ' ================================================================
    '  フィールド
    ' ================================================================
    Protected _crud As New CrudHelper()

    ''' <summary>顧客コード (KITOKU, TSYSCOM, KYOTO 等)</summary>
    Protected MustOverride ReadOnly Property CustomerCode As String

    ''' <summary>仕訳区分 (支払仕訳, 計上仕訳, 経費仕訳 等)</summary>
    Protected MustOverride ReadOnly Property SwkKbn As String

    ''' <summary>出力ファイル名ベース (拡張子なし)</summary>
    Protected Overridable ReadOnly Property OutputFileNameBase As String
        Get
            Return $"fc_{CustomerCode}_{SwkKbn}"
        End Get
    End Property

    ' ================================================================
    '  抽象メソッド（各顧客フォームで実装必須）
    ' ================================================================

    ''' <summary>
    ''' 仕訳出力データを tw_fc_swk_wrk に書き込む SQL を構築する。
    ''' tw_s_chuki_keijo を SELECT して tw_fc_swk_wrk に INSERT するSQL。
    ''' Access版 m仕訳データ作成 / gSWK_DATA_MAKE 相当。
    ''' </summary>
    Protected MustOverride Function BuildInsertToWrkSql(kikanFrom As Date) As String

    ''' <summary>
    ''' tw_fc_swk_wrk のデータをファイルに出力する。
    ''' 固定長 / CSV / Excel を顧客ごとに選択して実装する。
    ''' </summary>
    ''' <param name="dt">tw_fc_swk_wrk の出力対象データ</param>
    ''' <param name="outputFolder">出力先フォルダパス</param>
    ''' <returns>出力ファイルパス（失敗時は Nothing）</returns>
    Protected MustOverride Function WriteOutputFile(dt As DataTable, outputFolder As String) As String

    ' ================================================================
    '  テンプレートメソッド（共通処理フロー）
    ' ================================================================

    ''' <summary>
    ''' 仕訳出力処理メイン。
    ''' 1. 前提条件チェック（tw_s_keijo_joken の存在確認）
    ''' 2. ワークテーブルクリア
    ''' 3. BuildInsertToWrkSql() で tw_fc_swk_wrk に書き込み
    ''' 4. WriteOutputFile() でファイル出力
    ''' </summary>
    ''' <param name="outputFolder">出力先フォルダパス</param>
    ''' <returns>出力ファイルパス（件数0またはエラー時は Nothing）</returns>
    Protected Function Execute(outputFolder As String) As String
        ' 前提条件チェック
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return Nothing

        ' ワークテーブルクリア
        ClearWorkTable()

        ' データ生成
        Dim insertSql = BuildInsertToWrkSql(kikanFrom)
        _crud.ExecuteNonQuery(insertSql)

        ' 件数チェック
        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = @p1 AND swk_kbn = @p2",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@p1", CustomerCode),
                New NpgsqlParameter("@p2", SwkKbn)
            })
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        ' 出力データ取得
        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_fc_swk_wrk WHERE customer_cd = @p1 AND swk_kbn = @p2 ORDER BY den_no, gyo_no",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@p1", CustomerCode),
                New NpgsqlParameter("@p2", SwkKbn)
            })

        ' ファイル出力
        Return WriteOutputFile(dt, outputFolder)
    End Function

    ''' <summary>
    ''' tw_s_keijo_joken の存在確認と kikanFrom の取得。
    ''' Access版 Form_Open の集計条件チェック相当。
    ''' </summary>
    Protected Function ValidateJokenAndGetKikanFrom(ByRef kikanFrom As Date) As Boolean
        Dim dtJoken = _crud.GetDataTable("SELECT * FROM tw_s_keijo_joken LIMIT 1")
        If dtJoken.Rows.Count = 0 OrElse IsDBNull(dtJoken.Rows(0)("kikan_from")) Then
            MessageBox.Show("月次仕訳計上フレックスの集計条件が実行されていません。",
                            "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        kikanFrom = CDate(dtJoken.Rows(0)("kikan_from"))
        Return True
    End Function

    ''' <summary>
    ''' tw_fc_swk_wrk の当顧客データをクリアする。
    ''' </summary>
    Protected Sub ClearWorkTable()
        _crud.ExecuteNonQuery(
            "DELETE FROM tw_fc_swk_wrk WHERE customer_cd = @p1 AND swk_kbn = @p2",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@p1", CustomerCode),
                New NpgsqlParameter("@p2", SwkKbn)
            })
    End Sub

    ''' <summary>
    ''' 出力フォルダのバリデーション（共通）。
    ''' </summary>
    Protected Function ValidateOutputFolder(outputFolder As String) As Boolean
        If String.IsNullOrWhiteSpace(outputFolder) Then
            MessageBox.Show("出力フォルダを指定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If Not Directory.Exists(outputFolder) Then
            MessageBox.Show("指定したフォルダが存在しません。存在するフォルダを指定して再度実行してください。",
                            "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 実行確認ダイアログ（共通）。
    ''' </summary>
    Protected Function ConfirmExecute() As Boolean
        Return MessageBox.Show("実行してよろしいですか？", "確認",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
    End Function

    ' ================================================================
    '  IDisposable
    ' ================================================================
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _crud?.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

End Class
