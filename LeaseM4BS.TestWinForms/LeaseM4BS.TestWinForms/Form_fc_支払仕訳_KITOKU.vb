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

    Private _settei As FcSetteiHelper

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
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_SLIP_DT.Text),
                        Format(Now, "yyyy/MM/dd"),
                        txt_SLIP_DT.Text)
        Dim slipNoStart = CInt(If(String.IsNullOrWhiteSpace(txt_SLIP_NO_START_VAL.Text), "1",
                                  txt_SLIP_NO_START_VAL.Text))
        Dim kamokuCd = txt_KAMOKU_CD.Text
        Dim bshoCd = txt_BSHO_CD.Text

        ' TODO: Access版 m仕訳データ作成（支払仕訳）と照合して正確な列マッピングに更新する
        ' 借方行（リース費用）と貸方行（未払金）を UNION ALL で生成
        Return $"
INSERT INTO tw_kitoku_cmsw2wrk (
    sw2_kai_code,
    sw2_date,
    sw2_den_no,
    sw2_gyo_no,
    sw2_dc_kbn,
    sw2_kmk_code,
    sw2_hkm_code,
    sw2_bmn_code,
    sw2_kin,
    sw2_zei_code,
    sw2_zei_kbn,
    sw2_zei_kin,
    sw2_cur_code,
    sw2_rate_type,
    sw2_tekiyo1,
    sw2_tekiyo2,
    sw2_grp_code,
    sw2_sys_kbn,
    sw2_den_kbn,
    sw2_rec_level,
    sw2_tori_kbn,
    sw2_tori_code
)
SELECT
    '',
    '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    1 AS sw2_gyo_no,
    '1' AS sw2_dc_kbn,
    COALESCE(h.dr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.dr_hkm_cd, '') AS sw2_hkm_code,
    '{bshoCd}' AS sw2_bmn_code,
    k.lsryo AS sw2_kin,
    '' AS sw2_zei_code,
    '0' AS sw2_zei_kbn,
    k.zei AS sw2_zei_kin,
    'JPY' AS sw2_cur_code,
    '00' AS sw2_rate_type,
    COALESCE(k.bukn_nm, '') AS sw2_tekiyo1,
    COALESCE(k.kykbnl_no, '') AS sw2_tekiyo2,
    '00' AS sw2_grp_code,
    '01' AS sw2_sys_kbn,
    '1' AS sw2_den_kbn,
    '0' AS sw2_rec_level,
    '' AS sw2_tori_kbn,
    '' AS sw2_tori_code
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1
  AND k.rec_kbn IN (1, 3)
  AND k.keijo_f = TRUE
UNION ALL
SELECT
    '',
    '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    2 AS sw2_gyo_no,
    '2' AS sw2_dc_kbn,
    '{kamokuCd}' AS sw2_kmk_code,
    '' AS sw2_hkm_code,
    '{bshoCd}' AS sw2_bmn_code,
    k.lsryo + k.zei AS sw2_kin,
    '' AS sw2_zei_code,
    '0' AS sw2_zei_kbn,
    0 AS sw2_zei_kin,
    'JPY' AS sw2_cur_code,
    '00' AS sw2_rate_type,
    COALESCE(k.bukn_nm, '') AS sw2_tekiyo1,
    COALESCE(k.kykbnl_no, '') AS sw2_tekiyo2,
    '00' AS sw2_grp_code,
    '01' AS sw2_sys_kbn,
    '1' AS sw2_den_kbn,
    '0' AS sw2_rec_level,
    '' AS sw2_tori_kbn,
    '' AS sw2_tori_code
FROM tw_s_chuki_keijo k
WHERE k.kjkbn_id = 1
  AND k.rec_kbn IN (1, 3)
  AND k.keijo_f = TRUE
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
    End Sub

    Private Sub Form_fc_支払仕訳_KITOKU_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
