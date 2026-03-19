Imports System.IO
Imports System.Text
Imports LeaseM4BS.DataAccess

''' <summary>
''' VTC顧客固有 支払仕訳出力フォーム
''' Access版 fc_支払仕訳_VTC 相当
''' FcJournalOutputBase を継承し、tw_fc_swk_wrk に支払仕訳を出力する。
''' kjkbn_id=1（費用）、rec_kbn IN(1,3) を対象。
''' ファイル出力なし（データ作成のみ）。
''' </summary>
Partial Public Class Form_fc_支払仕訳_VTC
    Inherits FcJournalOutputBase

    Protected Overrides ReadOnly Property CustomerCode As String
        Get
            Return "VTC"
        End Get
    End Property

    Protected Overrides ReadOnly Property SwkKbn As String
        Get
            Return "支払仕訳"
        End Get
    End Property

    Private Const KEY_SLIP_DT As String = "VTC_HARAISWK_SLIP_DT"
    Private Const KEY_KMKCD_1D As String = "VTC_HARAISWK_KMKCD_1D"
    Private Const KEY_KMKCD_1C As String = "VTC_HARAISWK_KMKCD_1C"
    Private Const KEY_KMKCD_2D As String = "VTC_HARAISWK_KMKCD_2D"
    Private Const KEY_KMKCD_2C As String = "VTC_HARAISWK_KMKCD_2C"
    Private Const KEY_KMKCD_2C2 As String = "VTC_HARAISWK_KMKCD_2C2"
    Private Const KEY_KMKCD_3C As String = "VTC_HARAISWK_KMKCD_3C"
    Private Const KEY_KMKCD_4C As String = "VTC_HARAISWK_KMKCD_4C"

    Private _settei As FcSetteiHelper

    Public Sub New()
        InitializeComponent()
        _settei = New FcSetteiHelper("VTC")
    End Sub

    Private Sub Form_fc_支払仕訳_VTC_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then
            Me.Close()
            Return
        End If
        txt_SLIP_DT.Text = Format(kikanFrom, "yyyy/MM")
        txt_SLIP_DT.ReadOnly = True
        LoadSettings()
    End Sub

    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        If Not ConfirmExecute() Then Return

        Dim kikanFrom As Date
        If Not ValidateJokenAndGetKikanFrom(kikanFrom) Then Return

        ClearWorkTable()
        _crud.ExecuteNonQuery(BuildInsertToWrkSql(kikanFrom))

        Dim dtCount = _crud.GetDataTable(
            "SELECT COUNT(*) FROM tw_fc_swk_wrk WHERE customer_cd = 'VTC' AND swk_kbn = '支払仕訳'")
        Dim cnt = CInt(dtCount.Rows(0)(0))

        If cnt = 0 Then
            MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SaveSettings()
        MessageBox.Show($"仕訳データを {cnt} 件作成しました。",
                        "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    Private Sub cmd_支払日確認_Click(sender As Object, e As EventArgs) Handles cmd_支払日確認.Click
        MessageBox.Show("支払日確認機能は未実装です。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' tw_s_chuki_keijo → tw_fc_swk_wrk への INSERT SQL（VTC支払仕訳版）
    ''' kjkbn_id=1（費用）、rec_kbn IN(1,3)。勘定科目は t_haifu_keijo から取得。
    ''' </summary>
    Protected Overrides Function BuildInsertToWrkSql(kikanFrom As Date) As String
        Dim slipDt = Format(kikanFrom, "yyyy/MM/dd")

        Return $"
INSERT INTO tw_fc_swk_wrk (
    customer_cd, swk_kbn, den_no, den_date, gyo_no,
    dr_kmk_cd, dr_hkm_cd, dr_bmn_cd, dr_kin, dr_zei_kin,
    cr_kmk_cd, cr_hkm_cd, cr_bmn_cd, cr_kin,
    tekiyo, kykm_id, lsryo, zei, rec_kbn, kjkbn_id, shori_dt
)
SELECT
    'VTC', '支払仕訳',
    LPAD((ROW_NUMBER() OVER (ORDER BY k.kykm_id) - 1)::TEXT, 8, '0'),
    '{slipDt}', 1,
    COALESCE(h.dr_kmk_cd, '') AS dr_kmk_cd,
    COALESCE(h.dr_hkm_cd, '') AS dr_hkm_cd,
    '' AS dr_bmn_cd,
    k.lsryo AS dr_kin,
    k.zei AS dr_zei_kin,
    COALESCE(h.cr_kmk_cd, '') AS cr_kmk_cd,
    COALESCE(h.cr_hkm_cd, '') AS cr_hkm_cd,
    '' AS cr_bmn_cd,
    k.lsryo + k.zei AS cr_kin,
    COALESCE(k.bukn_nm, '') AS tekiyo,
    k.kykm_id, k.lsryo, k.zei, k.rec_kbn, k.kjkbn_id, CURRENT_DATE
FROM tw_s_chuki_keijo k
LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id
WHERE k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE
ORDER BY k.kykm_id"
    End Function

    ''' <summary>VTCはファイル出力なし。</summary>
    Protected Overrides Function WriteOutputFile(dt As DataTable, outputFolder As String) As String
        Return Nothing
    End Function

    Private Sub LoadSettings()
        txt_1D_リース未払金.Text = _settei.GetText(KEY_KMKCD_1D)
        txt_1C_支払リース仮勘定.Text = _settei.GetText(KEY_KMKCD_1C)
        txt_2D_未払費用.Text = _settei.GetText(KEY_KMKCD_2D)
        txt_2C_支払リース仮勘定.Text = _settei.GetText(KEY_KMKCD_2C)
        txt_2C_前払費用.Text = _settei.GetText(KEY_KMKCD_2C2)
        txt_3C_支払リース仮勘定.Text = _settei.GetText(KEY_KMKCD_3C)
        txt_4C_支払リース仮勘定.Text = _settei.GetText(KEY_KMKCD_4C)
    End Sub

    Private Sub SaveSettings()
        _settei.SetText(KEY_SLIP_DT, txt_SLIP_DT.Text)
        _settei.SetText(KEY_KMKCD_1D, txt_1D_リース未払金.Text)
        _settei.SetText(KEY_KMKCD_1C, txt_1C_支払リース仮勘定.Text)
        _settei.SetText(KEY_KMKCD_2D, txt_2D_未払費用.Text)
        _settei.SetText(KEY_KMKCD_2C, txt_2C_支払リース仮勘定.Text)
        _settei.SetText(KEY_KMKCD_2C2, txt_2C_前払費用.Text)
        _settei.SetText(KEY_KMKCD_3C, txt_3C_支払リース仮勘定.Text)
        _settei.SetText(KEY_KMKCD_4C, txt_4C_支払リース仮勘定.Text)
    End Sub

    Private Sub Form_fc_支払仕訳_VTC_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        _settei?.Dispose()
    End Sub

End Class
