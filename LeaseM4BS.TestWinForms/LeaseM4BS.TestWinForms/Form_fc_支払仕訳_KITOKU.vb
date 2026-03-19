Imports System.IO
Imports LeaseM4BS.DataAccess

''' <summary>
''' KITOKU顧客固有 支払仕訳出力フォーム
''' Access版 fc_支払仕訳_KITOKU 相当
''' FcJournalOutputBase を継承し、tw_kitoku_cmsw2wrk / apgdhwrk / apgddwrk / apgdswrk に出力する。
''' </summary>
Partial Public Class Form_fc_支払仕訳_KITOKU
    Inherits FcJournalOutputBase

    ' ================================================================
    '  FcJournalOutputBase 抽象プロパティ実装
    ' ================================================================

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "KITOKU"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "支払仕訳"
        End Get
    End Property

    ' ================================================================
    '  設定キー定数
    ' ================================================================

    Private Const KEY_SLIP_DT As String = "SLIP_DT"
    Private Const KEY_SLIP_NO As String = "SLIP_NO_START_VAL"
    Private Const KEY_KAMOKU_CD As String = "KAMOKU_CD"
    Private Const KEY_KAMOKU_NM As String = "KAMOKU_NM"
    Private Const KEY_BSHO_CD As String = "BSHO_CD"
    Private Const KEY_OUTPUT_FOLDER As String = "OUTPUT_FOLDER"
    Private Const KEY_OUTPUT_FILE1 As String = "OUTPUT_FILE1_NM"
    Private Const KEY_OUTPUT_FILE2 As String = "OUTPUT_FILE2_NM"
    Private Const KEY_OUTPUT_FILE3 As String = "OUTPUT_FILE3_NM"
    Private Const KEY_OUTPUT_FILE4 As String = "OUTPUT_FILE4_NM"
    Private Const KEY_ZRITU As String = "ZRITU"
    Private Const KEY_ZEI_CD As String = "ZEI_CD"

    Private _settei As FcSetteiHelper
    Private _zritu As Double = 0
    Private _zeiCd As String = ""

    ' ================================================================
    '  コンストラクタ
    ' ================================================================

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("KITOKU")
    End Sub

    ' ================================================================
    '  フォームイベント
    ' ================================================================

    Private Sub Form_fc_支払仕訳_KITOKU_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 前提条件チェック（tw_s_keijo_joken 存在確認）
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            Me.Close()
            Return
        End If

        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ValidateOutputFolder(txt_OUTPUT_FOLDER_NM.Text) Then Return
        If Not ConfirmExecute() Then Return

        Dim outputFolder = txt_OUTPUT_FOLDER_NM.Text
        Dim result = ExecuteKitoku(outputFolder)

        If result IsNot Nothing Then
            SaveSettings()
            MessageBox.Show($"出力完了しました。{Environment.NewLine}出力先: {result}",
                            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    ''' <summary>消費税率・税処理コードをSUBフォームで設定する（Access版 SUBフォーム呼び出し相当）</summary>
    Private Sub cmd_消費税設定_Click(sender As Object, e As EventArgs) Handles cmd_消費税設定.Click
        Using frm As New Form_fc_支払仕訳_KITOKU_SUB()
            frm.Zritu = _zritu
            frm.ZeiCd = _zeiCd
            If frm.ShowDialog(Me) = DialogResult.OK Then
                _zritu = frm.Zritu
                _zeiCd = frm.ZeiCd
            End If
        End Using
    End Sub

    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "出力先フォルダを選択してください"
            If Not String.IsNullOrWhiteSpace(txt_OUTPUT_FOLDER_NM.Text) Then
                dlg.SelectedPath = txt_OUTPUT_FOLDER_NM.Text
            End If
            If dlg.ShowDialog() = DialogResult.OK Then
                txt_OUTPUT_FOLDER_NM.Text = dlg.SelectedPath
            End If
        End Using
    End Sub

    ' ================================================================
    '  FcJournalOutputBase 抽象メソッド実装
    ' ================================================================

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_kitoku_cmsw2wrk への INSERT SQL を返す。
    ''' Access版 m仕訳データ作成（支払仕訳）相当。
    ''' kjkbn_id=1(費用), rec_kbn IN(1,3)(定額・付随費用), keijo_f=TRUE が対象。
    ''' 6種仕訳パターン（売買/賃貸 × 現預金/未払金 × 消費税一括有無）を CASE WHEN で実装。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_SLIP_DT.Text),
                        Format(Now, "yyyy/MM/dd"),
                        txt_SLIP_DT.Text)
        Dim slipNoStart = CInt(If(String.IsNullOrWhiteSpace(txt_SLIP_NO_START_VAL.Text), "1",
                                  txt_SLIP_NO_START_VAL.Text))
        Dim bshoCd = txt_BSHO_CD.Text

        ' Access版 pc_仕訳出力.gDivKMK_CD 相当: "4160-001" → kmkCd="4160", hkmCd="001"
        Dim kmkCd As String = ""
        Dim hkmCd As String = ""
        KitokuJournalHelper.DivKamokuCd(txt_KAMOKU_CD.Text, kmkCd, hkmCd)

        ' 計上期間開始月（現預金 vs 未払金 判定に使用）
        Dim kikanFromStr = Format(kikanFrom, "yyyy-MM-dd")

        '
        ' 6種仕訳パターン説明:
        '   leakbn_id=1          → 売買（移転ファイナンスリース）
        '   leakbn_id IN (3,4)   → 賃貸（移転外/オペレーティングリース）
        '   shri_dt 同月          → 現預金支払い（貸方=現預金科目 h.cr_kmk_cd）
        '   shri_dt 翌月以降      → 未払金計上（貸方=入力未払金科目 kmkCd）
        '   zei = 0 かつ 賃貸     → 消費税一括無（消費税ゼロ扱い）
        '
        Dim todayStr = Format(Now, "yyyy/MM/dd")
        Return $"
INSERT INTO tw_kitoku_cmsw2wrk (
    sw2_kai_code, sw2_date, sw2_den_no, sw2_gyo_no, sw2_dc_kbn,
    sw2_kmk_code, sw2_hkm_code, sw2_bmn_code,
    sw2_code1, sw2_code2, sw2_code3, sw2_code4,
    sw2_kin, sw2_zei_code, sw2_zei_kbn, sw2_zei_kin,
    sw2_cur_code, sw2_rate_type, sw2_rate, sw2_cur_kin,
    sw2_tekiyo1, sw2_tekiyo2, sw2_grp_code, sw2_sys_kbn,
    sw2_sys_den_no, sw2_sys_sys_kbn, sw2_sys_grp_code,
    sw2_ait_kmk_code, sw2_ait_hkm_code,
    sw2_usr_id1, sw2_sts_no1, sw2_sys_date1,
    sw2_usr_id2, sw2_sts_no2, sw2_sys_date2,
    sw2_shonin_kbn, sw2_den_kbn, sw2_haifu_kbn, sw2_rec_level,
    sw2_tori_kbn, sw2_tori_code,
    sw2_yobi_char1, sw2_yobi_char2, sw2_yobi_char3, sw2_yobi_char4,
    sw2_yobi_char5, sw2_yobi_char6, sw2_yobi_char7, sw2_yobi_char8,
    sw2_yobi_num1, sw2_yobi_num2, sw2_yobi_num3
)
-- 借方行: リース費用（全6パターン共通）
SELECT
    '' AS sw2_kai_code,
    '{slipDt}' AS sw2_date,
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0') AS sw2_den_no,
    1 AS sw2_gyo_no, '1' AS sw2_dc_kbn,
    COALESCE(h.dr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.dr_hkm_cd, '') AS sw2_hkm_code,
    '{bshoCd}' AS sw2_bmn_code,
    '' AS sw2_code1, '' AS sw2_code2, '' AS sw2_code3, '' AS sw2_code4,
    k.lsryo AS sw2_kin,
    '{_zeiCd}' AS sw2_zei_code, '0' AS sw2_zei_kbn,
    CASE WHEN k.leakbn_id IN (3, 4) AND k.zei = 0 THEN 0 ELSE k.zei END AS sw2_zei_kin,
    'JPY' AS sw2_cur_code, '00' AS sw2_rate_type, 1 AS sw2_rate, k.lsryo AS sw2_cur_kin,
    COALESCE(k.bukn_nm, '') AS sw2_tekiyo1,
    COALESCE(k.kykbnl_no, '') AS sw2_tekiyo2,
    '00' AS sw2_grp_code, '01' AS sw2_sys_kbn,
    '' AS sw2_sys_den_no, '' AS sw2_sys_sys_kbn, '' AS sw2_sys_grp_code,
    '' AS sw2_ait_kmk_code, '' AS sw2_ait_hkm_code,
    '' AS sw2_usr_id1, '' AS sw2_sts_no1, '{todayStr}' AS sw2_sys_date1,
    '' AS sw2_usr_id2, '' AS sw2_sts_no2, '' AS sw2_sys_date2,
    '0' AS sw2_shonin_kbn, '1' AS sw2_den_kbn, '0' AS sw2_haifu_kbn, '0' AS sw2_rec_level,
    '' AS sw2_tori_kbn, '' AS sw2_tori_code,
    '' AS sw2_yobi_char1, '' AS sw2_yobi_char2, '' AS sw2_yobi_char3, '' AS sw2_yobi_char4,
    '' AS sw2_yobi_char5, '' AS sw2_yobi_char6, '' AS sw2_yobi_char7, '' AS sw2_yobi_char8,
    0 AS sw2_yobi_num1, 0 AS sw2_yobi_num2, 0 AS sw2_yobi_num3
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
UNION ALL
-- 貸方行: 現預金（shri_dt同月）または未払金（翌月以降）、消費税一括有無で金額分岐
SELECT
    '' AS sw2_kai_code,
    '{slipDt}' AS sw2_date,
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0') AS sw2_den_no,
    2 AS sw2_gyo_no, '2' AS sw2_dc_kbn,
    CASE
        WHEN DATE_TRUNC('month', k.shri_dt) = DATE_TRUNC('month', '{kikanFromStr}'::date)
        THEN COALESCE(h.cr_kmk_cd, '') ELSE '{kmkCd}'
    END AS sw2_kmk_code,
    CASE
        WHEN DATE_TRUNC('month', k.shri_dt) = DATE_TRUNC('month', '{kikanFromStr}'::date)
        THEN COALESCE(h.cr_hkm_cd, '') ELSE '{hkmCd}'
    END AS sw2_hkm_code,
    '{bshoCd}' AS sw2_bmn_code,
    '' AS sw2_code1, '' AS sw2_code2, '' AS sw2_code3, '' AS sw2_code4,
    CASE WHEN k.leakbn_id IN (3, 4) AND k.zei = 0 THEN k.lsryo ELSE k.lsryo + k.zei END AS sw2_kin,
    '{_zeiCd}' AS sw2_zei_code, '0' AS sw2_zei_kbn, 0 AS sw2_zei_kin,
    'JPY' AS sw2_cur_code, '00' AS sw2_rate_type, 1 AS sw2_rate,
    CASE WHEN k.leakbn_id IN (3, 4) AND k.zei = 0 THEN k.lsryo ELSE k.lsryo + k.zei END AS sw2_cur_kin,
    COALESCE(k.bukn_nm, '') AS sw2_tekiyo1,
    COALESCE(k.kykbnl_no, '') AS sw2_tekiyo2,
    '00' AS sw2_grp_code, '01' AS sw2_sys_kbn,
    '' AS sw2_sys_den_no, '' AS sw2_sys_sys_kbn, '' AS sw2_sys_grp_code,
    '' AS sw2_ait_kmk_code, '' AS sw2_ait_hkm_code,
    '' AS sw2_usr_id1, '' AS sw2_sts_no1, '{todayStr}' AS sw2_sys_date1,
    '' AS sw2_usr_id2, '' AS sw2_sts_no2, '' AS sw2_sys_date2,
    '0' AS sw2_shonin_kbn, '1' AS sw2_den_kbn, '0' AS sw2_haifu_kbn, '0' AS sw2_rec_level,
    '' AS sw2_tori_kbn, '' AS sw2_tori_code,
    '' AS sw2_yobi_char1, '' AS sw2_yobi_char2, '' AS sw2_yobi_char3, '' AS sw2_yobi_char4,
    '' AS sw2_yobi_char5, '' AS sw2_yobi_char6, '' AS sw2_yobi_char7, '' AS sw2_yobi_char8,
    0 AS sw2_yobi_num1, 0 AS sw2_yobi_num2, 0 AS sw2_yobi_num3
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY sw2_den_no, sw2_gyo_no"
    End Function

    ''' <summary>
    ''' tw_kitoku_cmsw2wrk をShift-JIS固定長ファイルに出力する。
    ''' </summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, txt_OUTPUT_FILE1_NM.Text & ".txt")
        FixedLengthFileWriter.WriteFile(fileName, dt, KitokuFixedLengthFormats.GetCMSW2WRKFields())
        Return fileName
    End Function

    ' ================================================================
    '  KITOKU固有 実行フロー（4ワークテーブル / 4ファイル出力）
    ' ================================================================

    ''' <summary>
    ''' KITOKU支払仕訳出力のメイン処理。
    ''' 4つのワークテーブルを生成して4ファイルを出力する。
    ''' </summary>
    Private Function ExecuteKitoku(outputFolder As String) As String
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return Nothing

        ' ワークテーブルクリア（4テーブル）
        ClearKitokuWorkTables()

        ' CMSW2WRK データ生成
        Dim insertSql = BuildInsertToWrkSql(kikanFrom)
        _crud.ExecuteNonQuery(insertSql)

        ' 件数チェック
        Dim dtCount = _crud.GetDataTable("SELECT COUNT(*) FROM tw_kitoku_cmsw2wrk")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        ' APGD系ワークテーブル生成
        BuildApgdhWrk()
        BuildApgddWrk()
        BuildApgdsWrk()

        ' 各ワークテーブルデータ取得
        Dim dtCmsw2 = _crud.GetDataTable(
            "SELECT * FROM tw_kitoku_cmsw2wrk ORDER BY sw2_den_no, sw2_dc_kbn, sw2_gyo_no")
        Dim dtApgdh = _crud.GetDataTable(
            "SELECT * FROM tw_kitoku_apgdhwrk ORDER BY gdh_den_no")
        Dim dtApgdd = _crud.GetDataTable(
            "SELECT * FROM tw_kitoku_apgddwrk ORDER BY gdd_den_no, gdd_gyo_no")
        Dim dtApgds = _crud.GetDataTable(
            "SELECT * FROM tw_kitoku_apgdswrk ORDER BY gds_den_no, gds_gyo_no")

        ' ファイル出力（4ファイル）
        WriteOutputFile(dtCmsw2, outputFolder)
        WriteApgdhFile(dtApgdh, outputFolder)
        WriteApgddFile(dtApgdd, outputFolder)
        WriteApgdsFile(dtApgds, outputFolder)

        Return outputFolder
    End Function

    Private Sub ClearKitokuWorkTables()
        _crud.ExecuteNonQuery("DELETE FROM tw_kitoku_cmsw2wrk")
        _crud.ExecuteNonQuery("DELETE FROM tw_kitoku_apgdhwrk")
        _crud.ExecuteNonQuery("DELETE FROM tw_kitoku_apgddwrk")
        _crud.ExecuteNonQuery("DELETE FROM tw_kitoku_apgdswrk")
    End Sub

    ' TODO: Access版 gAPGDHWRK作成 相当。tw_kitoku_cmsw2wrk を集計して INSERT する。
    Private Sub BuildApgdhWrk()
        _crud.ExecuteNonQuery(
            "INSERT INTO tw_kitoku_apgdhwrk (gdh_den_no, gdh_den_date, gdh_kei_kin) " &
            "SELECT sw2_den_no, sw2_date, SUM(sw2_kin) " &
            "FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '2' " &
            "GROUP BY sw2_den_no, sw2_date")
    End Sub

    ' TODO: Access版 gAPGDDWRK作成 相当。借方明細行を INSERT する。
    Private Sub BuildApgddWrk()
        _crud.ExecuteNonQuery(
            "INSERT INTO tw_kitoku_apgddwrk (gdd_den_no, gdd_den_date, gdd_gyo_no, gdd_kmk_code, gdd_nuki_kin, gdd_zei_kin) " &
            "SELECT sw2_den_no, sw2_date, sw2_gyo_no, sw2_kmk_code, sw2_kin, sw2_zei_kin " &
            "FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '1'")
    End Sub

    ' TODO: Access版 gAPGDSWRK作成 相当。支払情報を INSERT する。
    Private Sub BuildApgdsWrk()
        _crud.ExecuteNonQuery(
            "INSERT INTO tw_kitoku_apgdswrk (gds_den_no, gds_den_date, gds_gyo_no, gds_sh_kin) " &
            "SELECT sw2_den_no, sw2_date, sw2_gyo_no, sw2_kin " &
            "FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '2'")
    End Sub

    Private Sub WriteApgdhFile(dt As DataTable, outputFolder As String)
        Dim fileName = Path.Combine(outputFolder, txt_OUTPUT_FILE2_NM.Text & ".txt")
        FixedLengthFileWriter.WriteFile(fileName, dt, KitokuFixedLengthFormats.GetAPGDHWRKFields())
    End Sub

    Private Sub WriteApgddFile(dt As DataTable, outputFolder As String)
        Dim fileName = Path.Combine(outputFolder, txt_OUTPUT_FILE3_NM.Text & ".txt")
        FixedLengthFileWriter.WriteFile(fileName, dt, KitokuFixedLengthFormats.GetAPGDDWRKFields())
    End Sub

    Private Sub WriteApgdsFile(dt As DataTable, outputFolder As String)
        Dim fileName = Path.Combine(outputFolder, txt_OUTPUT_FILE4_NM.Text & ".txt")
        FixedLengthFileWriter.WriteFile(fileName, dt, KitokuFixedLengthFormats.GetAPGDSWRKFields())
    End Sub

    ' ================================================================
    '  設定の読み書き
    ' ================================================================

    Private Sub LoadSettings()
        txt_SLIP_DT.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_SLIP_NO_START_VAL.Text = _settei.GetText(KEY_SLIP_NO, "1")
        txt_KAMOKU_CD.Text = _settei.GetText(KEY_KAMOKU_CD)
        txt_KAMOKU_NM.Text = _settei.GetText(KEY_KAMOKU_NM)
        txt_BSHO_CD.Text = _settei.GetText(KEY_BSHO_CD)
        txt_OUTPUT_FOLDER_NM.Text = _settei.GetText(KEY_OUTPUT_FOLDER)
        txt_OUTPUT_FILE1_NM.Text = _settei.GetText(KEY_OUTPUT_FILE1, "CMSW2WRK")
        txt_OUTPUT_FILE2_NM.Text = _settei.GetText(KEY_OUTPUT_FILE2, "APGDHWRK")
        txt_OUTPUT_FILE3_NM.Text = _settei.GetText(KEY_OUTPUT_FILE3, "APGDDWRK")
        txt_OUTPUT_FILE4_NM.Text = _settei.GetText(KEY_OUTPUT_FILE4, "APGDSWRK")
        Double.TryParse(_settei.GetText(KEY_ZRITU, "0"), _zritu)
        _zeiCd = _settei.GetText(KEY_ZEI_CD, "")
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_SLIP_DT.Text)
        _settei.SetText(KEY_SLIP_NO, txt_SLIP_NO_START_VAL.Text)
        _settei.SetText(KEY_KAMOKU_CD, txt_KAMOKU_CD.Text)
        _settei.SetText(KEY_KAMOKU_NM, txt_KAMOKU_NM.Text)
        _settei.SetText(KEY_BSHO_CD, txt_BSHO_CD.Text)
        _settei.SetText(KEY_OUTPUT_FOLDER, txt_OUTPUT_FOLDER_NM.Text)
        _settei.SetText(KEY_OUTPUT_FILE1, txt_OUTPUT_FILE1_NM.Text)
        _settei.SetText(KEY_OUTPUT_FILE2, txt_OUTPUT_FILE2_NM.Text)
        _settei.SetText(KEY_OUTPUT_FILE3, txt_OUTPUT_FILE3_NM.Text)
        _settei.SetText(KEY_OUTPUT_FILE4, txt_OUTPUT_FILE4_NM.Text)
        _settei.SetText(KEY_ZRITU, _zritu.ToString())
        _settei.SetText(KEY_ZEI_CD, _zeiCd)
    End Sub

    Private Sub Form_fc_支払仕訳_KITOKU_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
