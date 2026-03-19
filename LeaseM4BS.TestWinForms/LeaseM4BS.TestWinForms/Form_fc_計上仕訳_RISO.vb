Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' RISO顧客固有 計上仕訳出力フォーム
''' Access版 fc_計上仕訳_RISO 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に計上仕訳を出力する。
''' kjkbn_id=2（資産）、rec_kbn IN(1,3) および rec_kbn=2（zei>0）を対象。
''' ファイル出力なし（データ登録のみ）。
''' </summary>
Partial Public Class Form_fc_計上仕訳_RISO
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "RISO"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "計上仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "RISO_KEIJOSWK_SLIP_DT"
    Private Const KEY_BUMON As String = "RISO_KEIJOSWK_BUMON_CD"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("RISO")
    End Sub

    Private Sub Form_fc_計上仕訳_RISO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            Me.Close()
            Return
        End If
        txt_出力年月.Text = Format(kikanFrom, "yyyy/MM")
        txt_出力年月.ReadOnly = True
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'RISO' AND swk_kbn = '計上仕訳'")
        Dim cnt = CInt(dtCount.Rows(0)(0))
        If cnt = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SaveSettings()
        MessageBox.Show($"データを登録しました。（{cnt}件）",
                        "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（RISO計上仕訳版）
    ''' kjkbn_id=2（資産）、rec_kbn IN(1,3) UNION ALL rec_kbn=2（zei>0）。
    ''' txt_部署コード_一括用 を dr_bmn_cd に使用。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = If(String.IsNullOrWhiteSpace(txt_伝票日付.Text),
                        Format(Now, "yyyy/MM/dd"), txt_伝票日付.Text)
        Dim bumonCd = txt_部署コード_一括用.Text.Trim()

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'RISO', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id, k.rec_kbn) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    CASE WHEN k.rec_kbn = 2 THEN k.zei ELSE k.lsryo END AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    CASE WHEN k.rec_kbn = 2 THEN k.zei ELSE k.lsryo END AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2
  AND k.rec_kbn IN (1, 3)
  AND k.keijo_f = TRUE
UNION ALL
SELECT
    'RISO', '計上仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '{bumonCd}' AS dr_bmn_cd,
    k.zei AS dr_kin,
    0 AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 2
  AND k.rec_kbn = 2
  AND k.keijo_f = TRUE
  AND k.zei > 0
ORDER BY kykm_id, rec_kbn"
    End Function

    ''' <summary>ファイル出力なし（データ登録のみ）。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Return Nothing
    End Function

    Private Sub LoadSettings()
        txt_伝票日付.Text = _settei.GetText(KEY_SLIP_DT, Format(Now, "yyyy/MM/dd"))
        txt_部署コード_一括用.Text = _settei.GetText(KEY_BUMON)
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_伝票日付.Text)
        _settei.SetText(KEY_BUMON, txt_部署コード_一括用.Text)
    End Sub

    Private Sub Form_fc_計上仕訳_RISO_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
