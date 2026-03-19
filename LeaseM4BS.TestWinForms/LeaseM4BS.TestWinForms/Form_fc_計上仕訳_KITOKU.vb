Imports System.IO
Imports LeaseM4BS.DataAccess

''' <summary>
''' KITOKU顧客固有 計上仕訳出力フォーム
''' Access版 fc_計上仕訳_KITOKU 相当
''' FcJournalOutputBase を継承し、tw_kitoku_cmsw2wrk に計上仕訳を出力する。
''' 支払仕訳と異なり出力ファイルは CMSW2WRK の 1 本のみ。
''' </summary>
Partial Public Class Form_fc_計上仕訳_KITOKU
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "KITOKU"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "計上仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "KEIJO_SLIP_DT"
    Private Const KEY_SLIP_NO As String = "KEIJO_SLIP_NO_START_VAL"
    Private Const KEY_OUTPUT_FOLDER As String = "KEIJO_OUTPUT_FOLDER"
    Private Const KEY_OUTPUT_FILE As String = "KEIJO_OUTPUT_FILE_NM"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("KITOKU")
    End Sub

    Private Sub Form_fc_計上仕訳_KITOKU_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Dim result = ExecuteKeijo(txt_OUTPUT_FOLDER_NM.Text)
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

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_kitoku_cmsw2wrk への INSERT SQL（計上仕訳版）
    ''' Access版 m仕訳データ作成（計上仕訳）相当。
    ''' 3種パターンを rec_kbn で分岐:
    '''   rec_kbn=1: 開始計上（リース資産/リース債務）
    '''   rec_kbn=2: 税一括（仮払消費税/リース未払金）
    '''   rec_kbn=3: 減価償却（減価償却費/減価償却累計額）
    ''' t_haifu_keijo の科目コードは kjkbn_id=2 の行から取得。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_SLIP_DT.Text),
                        Format(Now, "yyyy/MM/dd"), txt_SLIP_DT.Text)
        Dim slipNoStart = CInt(If(String.IsNullOrWhiteSpace(txt_SLIP_NO_START_VAL.Text),
                                  "1", txt_SLIP_NO_START_VAL.Text))

        Return $"
INSERT INTO tw_kitoku_cmsw2wrk (
    sw2_kai_code, sw2_date, sw2_den_no, sw2_gyo_no,
    sw2_dc_kbn, sw2_kmk_code, sw2_hkm_code, sw2_bmn_code,
    sw2_kin, sw2_zei_code, sw2_zei_kbn, sw2_zei_kin,
    sw2_cur_code, sw2_rate_type, sw2_tekiyo1, sw2_tekiyo2,
    sw2_grp_code, sw2_sys_kbn, sw2_den_kbn, sw2_rec_level
)
-- パターン1: 開始計上 (rec_kbn=1) 借方: リース資産 / 貸方: リース債務 / 金額: lsryo
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    1 AS sw2_gyo_no, '1' AS sw2_dc_kbn,
    COALESCE(h.dr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.dr_hkm_cd, '') AS sw2_hkm_code, '',
    k.lsryo, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 1 AND k.keijo_f = TRUE
UNION ALL
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    2 AS sw2_gyo_no, '2' AS sw2_dc_kbn,
    COALESCE(h.cr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.cr_hkm_cd, '') AS sw2_hkm_code, '',
    k.lsryo, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 1 AND k.keijo_f = TRUE
UNION ALL
-- パターン2: 税一括 (rec_kbn=2) 借方: 仮払消費税 / 貸方: リース未払金 / 金額: zei
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    1 AS sw2_gyo_no, '1' AS sw2_dc_kbn,
    COALESCE(h.dr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.dr_hkm_cd, '') AS sw2_hkm_code, '',
    k.zei, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 2 AND k.keijo_f = TRUE AND k.zei > 0
UNION ALL
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    2 AS sw2_gyo_no, '2' AS sw2_dc_kbn,
    COALESCE(h.cr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.cr_hkm_cd, '') AS sw2_hkm_code, '',
    k.zei, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 2 AND k.keijo_f = TRUE AND k.zei > 0
UNION ALL
-- パターン3: 減価償却 (rec_kbn=3) 借方: 減価償却費 / 貸方: 減価償却累計額 / 金額: lsryo
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    1 AS sw2_gyo_no, '1' AS sw2_dc_kbn,
    COALESCE(h.dr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.dr_hkm_cd, '') AS sw2_hkm_code, '',
    k.lsryo, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 3 AND k.keijo_f = TRUE
UNION ALL
SELECT '', '{slipDt}',
    LPAD(({slipNoStart} + ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    2 AS sw2_gyo_no, '2' AS sw2_dc_kbn,
    COALESCE(h.cr_kmk_cd, '') AS sw2_kmk_code,
    COALESCE(h.cr_hkm_cd, '') AS sw2_hkm_code, '',
    k.lsryo, '', '0', 0, 'JPY', '00',
    COALESCE(k.bukn_nm, ''), COALESCE(k.kykbnl_no, ''),
    '00', '01', '1', '0'
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2 AND k.rec_kbn = 3 AND k.keijo_f = TRUE
ORDER BY sw2_den_no, sw2_gyo_no"
    End Function

    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Dim fileName = Path.Combine(outputFolder, txt_OUTPUT_FILE_NM.Text & ".txt")
        FixedLengthFileWriter.WriteFile(fileName, dt, KitokuFixedLengthFormats.GetCMSW2WRKFields())
        Return fileName
    End Function

    Private Function ExecuteKeijo(outputFolder As String) As String
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return Nothing

        _crud.ExecuteNonQuery("DELETE FROM tw_kitoku_cmsw2wrk")
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable("SELECT COUNT(*) FROM tw_kitoku_cmsw2wrk")
        If CInt(dtCount.Rows(0)(0)) = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        Dim dt = _crud.GetDataTable(
            "SELECT * FROM tw_kitoku_cmsw2wrk ORDER BY sw2_den_no, sw2_dc_kbn, sw2_gyo_no")
        Return WriteOutputFile(dt, outputFolder)
    End Function

    Private Sub LoadSettings()
        txt_SLIP_DT.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_SLIP_NO_START_VAL.Text = _settei.GetText(KEY_SLIP_NO, "1")
        txt_OUTPUT_FOLDER_NM.Text = _settei.GetText(KEY_OUTPUT_FOLDER)
        txt_OUTPUT_FILE_NM.Text = _settei.GetText(KEY_OUTPUT_FILE, "CMSW2WRK_KEIJO")
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_SLIP_DT.Text)
        _settei.SetText(KEY_SLIP_NO, txt_SLIP_NO_START_VAL.Text)
        _settei.SetText(KEY_OUTPUT_FOLDER, txt_OUTPUT_FOLDER_NM.Text)
        _settei.SetText(KEY_OUTPUT_FILE, txt_OUTPUT_FILE_NM.Text)
    End Sub

    Private Sub Form_fc_計上仕訳_KITOKU_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
