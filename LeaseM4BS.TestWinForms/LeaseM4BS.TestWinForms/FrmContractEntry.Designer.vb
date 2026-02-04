<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmContractEntry
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.cmd_閉じる = New System.Windows.Forms.Button()
        Me.cmd_TOUROKU = New System.Windows.Forms.Button()
        Me.cmd_REVISE = New System.Windows.Forms.Button()
        Me.cmd_SAILEASE = New System.Windows.Forms.Button()
        Me.cmd_KAIYAKU = New System.Windows.Forms.Button()
        Me.cmd_ROLLBACK_SAI = New System.Windows.Forms.Button()
        Me.cmd_DELETE = New System.Windows.Forms.Button()
        Me.cmd_MODE_RESET = New System.Windows.Forms.Button()
        Me.cmd_取込 = New System.Windows.Forms.Button()
        Me.cmd_CLEAR = New System.Windows.Forms.Button()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.txt_CORP_NM = New System.Windows.Forms.TextBox()
        Me.Label_M1 = New System.Windows.Forms.Label()
        Me.txt_Mode = New System.Windows.Forms.TextBox()
        Me.lbl_計上区分 = New System.Windows.Forms.Label()
        Me.txt_KJKBN_NM = New System.Windows.Forms.TextBox()
        Me.lbl_適用日 = New System.Windows.Forms.Label()
        Me.txt_TEKIYO_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_管理単位 = New System.Windows.Forms.Label()
        Me.cmb_KKNRI_ID = New System.Windows.Forms.ComboBox()
        Me.txt_KKNRI_NM = New System.Windows.Forms.TextBox()
        Me.lbl_契約区分 = New System.Windows.Forms.Label()
        Me.cmb_KKBN_ID = New System.Windows.Forms.ComboBox()
        Me.lbl_支払先 = New System.Windows.Forms.Label()
        Me.cmb_LCPT_ID = New System.Windows.Forms.ComboBox()
        Me.txt_LCPT_NM = New System.Windows.Forms.TextBox()
        Me.lbl_契約番号 = New System.Windows.Forms.Label()
        Me.txt_KYAK_NO = New System.Windows.Forms.TextBox()
        Me.lbl_契約日 = New System.Windows.Forms.Label()
        Me.dtp_KYAK_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_再リース回数 = New System.Windows.Forms.Label()
        Me.txt_SAIKAISU = New System.Windows.Forms.TextBox()
        Me.lbl_契約終了 = New System.Windows.Forms.Label()
        Me.chk_KYAK_END_F = New System.Windows.Forms.CheckBox()
        Me.lbl_自社管理用 = New System.Windows.Forms.Label()
        Me.txt_JISH_KYAK_NO = New System.Windows.Forms.TextBox()
        Me.lbl_開始日 = New System.Windows.Forms.Label()
        Me.dtp_START_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_契約期間 = New System.Windows.Forms.Label()
        Me.txt_LKIKAN = New System.Windows.Forms.TextBox()
        Me.Label_M2 = New System.Windows.Forms.Label()
        Me.lbl_稟議番号 = New System.Windows.Forms.Label()
        Me.txt_RNG_BANGO = New System.Windows.Forms.TextBox()
        Me.lbl_終了日 = New System.Windows.Forms.Label()
        Me.dtp_END_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_遡及計上 = New System.Windows.Forms.Label()
        Me.chk_SKYU_KJ_F = New System.Windows.Forms.CheckBox()
        Me.lbl_支払間隔 = New System.Windows.Forms.Label()
        Me.txt_SHRI_KN = New System.Windows.Forms.TextBox()
        Me.Label_M3 = New System.Windows.Forms.Label()
        Me.lbl_前払回数 = New System.Windows.Forms.Label()
        Me.txt_MKAISU = New System.Windows.Forms.TextBox()
        Me.Label_M4 = New System.Windows.Forms.Label()
        Me.lbl_支払回数 = New System.Windows.Forms.Label()
        Me.txt_SHRI_CNT = New System.Windows.Forms.TextBox()
        Me.Label_M5 = New System.Windows.Forms.Label()
        Me.lbl_前払日 = New System.Windows.Forms.Label()
        Me.txt_MAE_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_第1回支払日 = New System.Windows.Forms.Label()
        Me.txt_SHRI_DT1 = New System.Windows.Forms.DateTimePicker()
        Me.lbl_第2回支払日 = New System.Windows.Forms.Label()
        Me.txt_SHRI_DT2 = New System.Windows.Forms.DateTimePicker()
        Me.lbl_第3回以降 = New System.Windows.Forms.Label()
        Me.txt_SHRI_DT3 = New System.Windows.Forms.TextBox()
        Me.Label_M6 = New System.Windows.Forms.Label()
        Me.lbl_最終支払日 = New System.Windows.Forms.Label()
        Me.txt_SHRI_EN_DT = New System.Windows.Forms.DateTimePicker()
        Me.lbl_現金購入価額 = New System.Windows.Forms.Label()
        Me.txt_KNYUKN = New System.Windows.Forms.TextBox()
        Me.cmd_料率 = New System.Windows.Forms.Button()
        Me.txt_RYORITU = New System.Windows.Forms.TextBox()
        Me.cmd_税率 = New System.Windows.Forms.Button()
        Me.cmb_ZRITU = New System.Windows.Forms.ComboBox()
        Me.lbl_支払方法 = New System.Windows.Forms.Label()
        Me.cmb_SHHO_M_ID = New System.Windows.Forms.ComboBox()
        Me.cmb_SHHO_1_ID = New System.Windows.Forms.ComboBox()
        Me.cmb_SHHO_2_ID = New System.Windows.Forms.ComboBox()
        Me.cmb_SHHO_3_ID = New System.Windows.Forms.ComboBox()
        Me.lbl_月額 = New System.Windows.Forms.Label()
        Me.txt_GLSRYO = New System.Windows.Forms.TextBox()
        Me.lbl_期間額 = New System.Windows.Forms.Label()
        Me.txt_KLSRYO = New System.Windows.Forms.TextBox()
        Me.lbl_前払 = New System.Windows.Forms.Label()
        Me.txt_MLSRYO = New System.Windows.Forms.TextBox()
        Me.lbl_総額 = New System.Windows.Forms.Label()
        Me.txt_SLSRYO = New System.Windows.Forms.TextBox()
        Me.lbl_税抜き = New System.Windows.Forms.Label()
        Me.lbl_消費税 = New System.Windows.Forms.Label()
        Me.txt_GZEI = New System.Windows.Forms.TextBox()
        Me.txt_KZEI = New System.Windows.Forms.TextBox()
        Me.txt_MZEI = New System.Windows.Forms.TextBox()
        Me.lbl_税込み = New System.Windows.Forms.Label()
        Me.txt_GLSRYO_ZEIKOMI = New System.Windows.Forms.TextBox()
        Me.txt_KLSRYO_ZKOMI = New System.Windows.Forms.TextBox()
        Me.txt_MLSRYO_ZKOMI = New System.Windows.Forms.TextBox()
        Me.lbl_維持管理費用 = New System.Windows.Forms.Label()
        Me.txt_IJIKNR = New System.Windows.Forms.TextBox()
        Me.lbl_残価保証額 = New System.Windows.Forms.Label()
        Me.txt_ZANRYO = New System.Windows.Forms.TextBox()
        Me.lbl_銀行口座 = New System.Windows.Forms.Label()
        Me.cmb_KOZA_ID = New System.Windows.Forms.ComboBox()
        Me.txt_KOZA_NM = New System.Windows.Forms.TextBox()
        Me.lbl_契約予備 = New System.Windows.Forms.Label()
        Me.cmb_RSRVK1_ID = New System.Windows.Forms.ComboBox()
        Me.txt_RSRVK1_NM = New System.Windows.Forms.TextBox()
        Me.lbl_備考 = New System.Windows.Forms.Label()
        Me.txt_K_ZOKUSEI1 = New System.Windows.Forms.TextBox()
        Me.lbl_契約名 = New System.Windows.Forms.Label()
        Me.txt_KYAK_NM = New System.Windows.Forms.TextBox()
        Me.cmd_物件画面 = New System.Windows.Forms.Button()
        Me.tab_f_KYKH_SUB = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.dgv_DETAILS = New System.Windows.Forms.DataGridView()
        Me.txt_KYKH_ID = New System.Windows.Forms.TextBox()
        Me.txt_KYKH_NO = New System.Windows.Forms.TextBox()
        Me.txt_UPDATE_CNT = New System.Windows.Forms.TextBox()
        Me.txt_K_CREATE_DT = New System.Windows.Forms.TextBox()
        Me.txt_K_UPDATE_DT = New System.Windows.Forms.TextBox()
        Me.txt_REND_DT = New System.Windows.Forms.TextBox()
        Me.txt_K_KJYO_ST_DT = New System.Windows.Forms.TextBox()
        Me.txt_K_KJYO_EN_DT = New System.Windows.Forms.TextBox()
        Me.chk_CKAIYK_F = New System.Windows.Forms.CheckBox()
        Me.chk_K_SEIGOU_F = New System.Windows.Forms.CheckBox()
        Me.chk_K_HENF_F = New System.Windows.Forms.CheckBox()
        Me.chk_K_HENL_F = New System.Windows.Forms.CheckBox()
        Me.chk_JENCHO_F = New System.Windows.Forms.CheckBox()
        Me.col_KYKM_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_BUKN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_SUURYO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_KNYUKN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.col_GLSRYO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlHeader.SuspendLayout()
        Me.pnlDetail.SuspendLayout()
        Me.tab_f_KYKH_SUB.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.dgv_DETAILS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.cmd_閉じる)
        Me.pnlHeader.Controls.Add(Me.cmd_TOUROKU)
        Me.pnlHeader.Controls.Add(Me.cmd_REVISE)
        Me.pnlHeader.Controls.Add(Me.cmd_SAILEASE)
        Me.pnlHeader.Controls.Add(Me.cmd_KAIYAKU)
        Me.pnlHeader.Controls.Add(Me.cmd_ROLLBACK_SAI)
        Me.pnlHeader.Controls.Add(Me.cmd_DELETE)
        Me.pnlHeader.Controls.Add(Me.cmd_MODE_RESET)
        Me.pnlHeader.Controls.Add(Me.cmd_取込)
        Me.pnlHeader.Controls.Add(Me.cmd_CLEAR)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(900, 40)
        Me.pnlHeader.TabIndex = 0
        '
        'cmd_閉じる
        '
        Me.cmd_閉じる.Location = New System.Drawing.Point(5, 5)
        Me.cmd_閉じる.Name = "cmd_閉じる"
        Me.cmd_閉じる.Size = New System.Drawing.Size(70, 30)
        Me.cmd_閉じる.TabIndex = 0
        Me.cmd_閉じる.Text = "閉じる(&C)"
        '
        'cmd_TOUROKU
        '
        Me.cmd_TOUROKU.Location = New System.Drawing.Point(80, 5)
        Me.cmd_TOUROKU.Name = "cmd_TOUROKU"
        Me.cmd_TOUROKU.Size = New System.Drawing.Size(80, 30)
        Me.cmd_TOUROKU.TabIndex = 1
        Me.cmd_TOUROKU.Text = "登録(&S)"
        '
        'cmd_REVISE
        '
        Me.cmd_REVISE.Location = New System.Drawing.Point(165, 5)
        Me.cmd_REVISE.Name = "cmd_REVISE"
        Me.cmd_REVISE.Size = New System.Drawing.Size(70, 30)
        Me.cmd_REVISE.TabIndex = 2
        Me.cmd_REVISE.Text = "修正(&V)"
        '
        'cmd_SAILEASE
        '
        Me.cmd_SAILEASE.Location = New System.Drawing.Point(240, 5)
        Me.cmd_SAILEASE.Name = "cmd_SAILEASE"
        Me.cmd_SAILEASE.Size = New System.Drawing.Size(100, 30)
        Me.cmd_SAILEASE.TabIndex = 3
        Me.cmd_SAILEASE.Text = "再ﾘｰｽ/返却(&H)"
        '
        'cmd_KAIYAKU
        '
        Me.cmd_KAIYAKU.Location = New System.Drawing.Point(345, 5)
        Me.cmd_KAIYAKU.Name = "cmd_KAIYAKU"
        Me.cmd_KAIYAKU.Size = New System.Drawing.Size(90, 30)
        Me.cmd_KAIYAKU.TabIndex = 4
        Me.cmd_KAIYAKU.Text = "中途解約(&K)"
        '
        'cmd_ROLLBACK_SAI
        '
        Me.cmd_ROLLBACK_SAI.Location = New System.Drawing.Point(440, 5)
        Me.cmd_ROLLBACK_SAI.Name = "cmd_ROLLBACK_SAI"
        Me.cmd_ROLLBACK_SAI.Size = New System.Drawing.Size(100, 30)
        Me.cmd_ROLLBACK_SAI.TabIndex = 5
        Me.cmd_ROLLBACK_SAI.Text = "再ﾘｰｽ取消(&T)"
        '
        'cmd_DELETE
        '
        Me.cmd_DELETE.Location = New System.Drawing.Point(545, 5)
        Me.cmd_DELETE.Name = "cmd_DELETE"
        Me.cmd_DELETE.Size = New System.Drawing.Size(70, 30)
        Me.cmd_DELETE.TabIndex = 6
        Me.cmd_DELETE.Text = "削除(&D)"
        '
        'cmd_MODE_RESET
        '
        Me.cmd_MODE_RESET.Location = New System.Drawing.Point(620, 5)
        Me.cmd_MODE_RESET.Name = "cmd_MODE_RESET"
        Me.cmd_MODE_RESET.Size = New System.Drawing.Size(90, 30)
        Me.cmd_MODE_RESET.TabIndex = 7
        Me.cmd_MODE_RESET.Text = "ﾓｰﾄﾞﾘｾｯﾄ(&M)"
        '
        'cmd_取込
        '
        Me.cmd_取込.Location = New System.Drawing.Point(715, 5)
        Me.cmd_取込.Name = "cmd_取込"
        Me.cmd_取込.Size = New System.Drawing.Size(70, 30)
        Me.cmd_取込.TabIndex = 8
        Me.cmd_取込.Text = "取込み(&I)"
        '
        'cmd_CLEAR
        '
        Me.cmd_CLEAR.Location = New System.Drawing.Point(790, 5)
        Me.cmd_CLEAR.Name = "cmd_CLEAR"
        Me.cmd_CLEAR.Size = New System.Drawing.Size(70, 30)
        Me.cmd_CLEAR.TabIndex = 9
        Me.cmd_CLEAR.Text = "クリア(&L)"
        '
        'pnlDetail
        '
        Me.pnlDetail.AutoScroll = True
        Me.pnlDetail.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlDetail.Controls.Add(Me.txt_CORP_NM)
        Me.pnlDetail.Controls.Add(Me.Label_M1)
        Me.pnlDetail.Controls.Add(Me.txt_Mode)
        Me.pnlDetail.Controls.Add(Me.lbl_計上区分)
        Me.pnlDetail.Controls.Add(Me.txt_KJKBN_NM)
        Me.pnlDetail.Controls.Add(Me.lbl_適用日)
        Me.pnlDetail.Controls.Add(Me.txt_TEKIYO_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_管理単位)
        Me.pnlDetail.Controls.Add(Me.cmb_KKNRI_ID)
        Me.pnlDetail.Controls.Add(Me.txt_KKNRI_NM)
        Me.pnlDetail.Controls.Add(Me.lbl_契約区分)
        Me.pnlDetail.Controls.Add(Me.cmb_KKBN_ID)
        Me.pnlDetail.Controls.Add(Me.lbl_支払先)
        Me.pnlDetail.Controls.Add(Me.cmb_LCPT_ID)
        Me.pnlDetail.Controls.Add(Me.txt_LCPT_NM)
        Me.pnlDetail.Controls.Add(Me.lbl_契約番号)
        Me.pnlDetail.Controls.Add(Me.txt_KYAK_NO)
        Me.pnlDetail.Controls.Add(Me.lbl_契約日)
        Me.pnlDetail.Controls.Add(Me.dtp_KYAK_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_再リース回数)
        Me.pnlDetail.Controls.Add(Me.txt_SAIKAISU)
        Me.pnlDetail.Controls.Add(Me.lbl_契約終了)
        Me.pnlDetail.Controls.Add(Me.chk_KYAK_END_F)
        Me.pnlDetail.Controls.Add(Me.lbl_自社管理用)
        Me.pnlDetail.Controls.Add(Me.txt_JISH_KYAK_NO)
        Me.pnlDetail.Controls.Add(Me.lbl_開始日)
        Me.pnlDetail.Controls.Add(Me.dtp_START_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_契約期間)
        Me.pnlDetail.Controls.Add(Me.txt_LKIKAN)
        Me.pnlDetail.Controls.Add(Me.Label_M2)
        Me.pnlDetail.Controls.Add(Me.lbl_稟議番号)
        Me.pnlDetail.Controls.Add(Me.txt_RNG_BANGO)
        Me.pnlDetail.Controls.Add(Me.lbl_終了日)
        Me.pnlDetail.Controls.Add(Me.dtp_END_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_遡及計上)
        Me.pnlDetail.Controls.Add(Me.chk_SKYU_KJ_F)
        Me.pnlDetail.Controls.Add(Me.lbl_支払間隔)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_KN)
        Me.pnlDetail.Controls.Add(Me.Label_M3)
        Me.pnlDetail.Controls.Add(Me.lbl_前払回数)
        Me.pnlDetail.Controls.Add(Me.txt_MKAISU)
        Me.pnlDetail.Controls.Add(Me.Label_M4)
        Me.pnlDetail.Controls.Add(Me.lbl_支払回数)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_CNT)
        Me.pnlDetail.Controls.Add(Me.Label_M5)
        Me.pnlDetail.Controls.Add(Me.lbl_前払日)
        Me.pnlDetail.Controls.Add(Me.txt_MAE_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_第1回支払日)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_DT1)
        Me.pnlDetail.Controls.Add(Me.lbl_第2回支払日)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_DT2)
        Me.pnlDetail.Controls.Add(Me.lbl_第3回以降)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_DT3)
        Me.pnlDetail.Controls.Add(Me.Label_M6)
        Me.pnlDetail.Controls.Add(Me.lbl_最終支払日)
        Me.pnlDetail.Controls.Add(Me.txt_SHRI_EN_DT)
        Me.pnlDetail.Controls.Add(Me.lbl_現金購入価額)
        Me.pnlDetail.Controls.Add(Me.txt_KNYUKN)
        Me.pnlDetail.Controls.Add(Me.cmd_料率)
        Me.pnlDetail.Controls.Add(Me.txt_RYORITU)
        Me.pnlDetail.Controls.Add(Me.cmd_税率)
        Me.pnlDetail.Controls.Add(Me.cmb_ZRITU)
        Me.pnlDetail.Controls.Add(Me.lbl_支払方法)
        Me.pnlDetail.Controls.Add(Me.cmb_SHHO_M_ID)
        Me.pnlDetail.Controls.Add(Me.cmb_SHHO_1_ID)
        Me.pnlDetail.Controls.Add(Me.cmb_SHHO_2_ID)
        Me.pnlDetail.Controls.Add(Me.cmb_SHHO_3_ID)
        Me.pnlDetail.Controls.Add(Me.lbl_月額)
        Me.pnlDetail.Controls.Add(Me.txt_GLSRYO)
        Me.pnlDetail.Controls.Add(Me.lbl_期間額)
        Me.pnlDetail.Controls.Add(Me.txt_KLSRYO)
        Me.pnlDetail.Controls.Add(Me.lbl_前払)
        Me.pnlDetail.Controls.Add(Me.txt_MLSRYO)
        Me.pnlDetail.Controls.Add(Me.lbl_総額)
        Me.pnlDetail.Controls.Add(Me.txt_SLSRYO)
        Me.pnlDetail.Controls.Add(Me.lbl_税抜き)
        Me.pnlDetail.Controls.Add(Me.lbl_消費税)
        Me.pnlDetail.Controls.Add(Me.txt_GZEI)
        Me.pnlDetail.Controls.Add(Me.txt_KZEI)
        Me.pnlDetail.Controls.Add(Me.txt_MZEI)
        Me.pnlDetail.Controls.Add(Me.lbl_税込み)
        Me.pnlDetail.Controls.Add(Me.txt_GLSRYO_ZEIKOMI)
        Me.pnlDetail.Controls.Add(Me.txt_KLSRYO_ZKOMI)
        Me.pnlDetail.Controls.Add(Me.txt_MLSRYO_ZKOMI)
        Me.pnlDetail.Controls.Add(Me.lbl_維持管理費用)
        Me.pnlDetail.Controls.Add(Me.txt_IJIKNR)
        Me.pnlDetail.Controls.Add(Me.lbl_残価保証額)
        Me.pnlDetail.Controls.Add(Me.txt_ZANRYO)
        Me.pnlDetail.Controls.Add(Me.lbl_銀行口座)
        Me.pnlDetail.Controls.Add(Me.cmb_KOZA_ID)
        Me.pnlDetail.Controls.Add(Me.txt_KOZA_NM)
        Me.pnlDetail.Controls.Add(Me.lbl_契約予備)
        Me.pnlDetail.Controls.Add(Me.cmb_RSRVK1_ID)
        Me.pnlDetail.Controls.Add(Me.txt_RSRVK1_NM)
        Me.pnlDetail.Controls.Add(Me.lbl_備考)
        Me.pnlDetail.Controls.Add(Me.txt_K_ZOKUSEI1)
        Me.pnlDetail.Controls.Add(Me.lbl_契約名)
        Me.pnlDetail.Controls.Add(Me.txt_KYAK_NM)
        Me.pnlDetail.Controls.Add(Me.cmd_物件画面)
        Me.pnlDetail.Controls.Add(Me.tab_f_KYKH_SUB)
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDetail.Location = New System.Drawing.Point(0, 40)
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Size = New System.Drawing.Size(900, 560)
        Me.pnlDetail.TabIndex = 1
        '
        'txt_CORP_NM
        '
        Me.txt_CORP_NM.Location = New System.Drawing.Point(509, 23)
        Me.txt_CORP_NM.Name = "txt_CORP_NM"
        Me.txt_CORP_NM.Size = New System.Drawing.Size(98, 19)
        Me.txt_CORP_NM.TabIndex = 99
        '
        'Label_M1
        '
        Me.Label_M1.Location = New System.Drawing.Point(618, 80)
        Me.Label_M1.Name = "Label_M1"
        Me.Label_M1.Size = New System.Drawing.Size(30, 15)
        Me.Label_M1.TabIndex = 0
        Me.Label_M1.Text = "ヶ月"
        '
        'txt_Mode
        '
        Me.txt_Mode.BackColor = System.Drawing.Color.Red
        Me.txt_Mode.ForeColor = System.Drawing.Color.White
        Me.txt_Mode.Location = New System.Drawing.Point(4, 4)
        Me.txt_Mode.Name = "txt_Mode"
        Me.txt_Mode.Size = New System.Drawing.Size(56, 19)
        Me.txt_Mode.TabIndex = 1
        Me.txt_Mode.Text = "新規"
        '
        'lbl_計上区分
        '
        Me.lbl_計上区分.Location = New System.Drawing.Point(76, 4)
        Me.lbl_計上区分.Name = "lbl_計上区分"
        Me.lbl_計上区分.Size = New System.Drawing.Size(76, 15)
        Me.lbl_計上区分.TabIndex = 2
        Me.lbl_計上区分.Text = "計上対象"
        '
        'txt_KJKBN_NM
        '
        Me.txt_KJKBN_NM.Location = New System.Drawing.Point(151, 4)
        Me.txt_KJKBN_NM.Name = "txt_KJKBN_NM"
        Me.txt_KJKBN_NM.Size = New System.Drawing.Size(42, 19)
        Me.txt_KJKBN_NM.TabIndex = 3
        '
        'lbl_適用日
        '
        Me.lbl_適用日.Location = New System.Drawing.Point(196, 4)
        Me.lbl_適用日.Name = "lbl_適用日"
        Me.lbl_適用日.Size = New System.Drawing.Size(50, 15)
        Me.lbl_適用日.TabIndex = 4
        Me.lbl_適用日.Text = "適用日"
        '
        'txt_TEKIYO_DT
        '
        Me.txt_TEKIYO_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txt_TEKIYO_DT.Location = New System.Drawing.Point(247, 2)
        Me.txt_TEKIYO_DT.Name = "txt_TEKIYO_DT"
        Me.txt_TEKIYO_DT.Size = New System.Drawing.Size(113, 19)
        Me.txt_TEKIYO_DT.TabIndex = 5
        '
        'lbl_管理単位
        '
        Me.lbl_管理単位.Location = New System.Drawing.Point(76, 23)
        Me.lbl_管理単位.Name = "lbl_管理単位"
        Me.lbl_管理単位.Size = New System.Drawing.Size(76, 15)
        Me.lbl_管理単位.TabIndex = 6
        Me.lbl_管理単位.Text = "管理単位"
        '
        'cmb_KKNRI_ID
        '
        Me.cmb_KKNRI_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmb_KKNRI_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmb_KKNRI_ID.Location = New System.Drawing.Point(151, 23)
        Me.cmb_KKNRI_ID.Name = "cmb_KKNRI_ID"
        Me.cmb_KKNRI_ID.Size = New System.Drawing.Size(95, 20)
        Me.cmb_KKNRI_ID.TabIndex = 7
        '
        'txt_KKNRI_NM
        '
        Me.txt_KKNRI_NM.Location = New System.Drawing.Point(246, 23)
        Me.txt_KKNRI_NM.Name = "txt_KKNRI_NM"
        Me.txt_KKNRI_NM.Size = New System.Drawing.Size(264, 19)
        Me.txt_KKNRI_NM.TabIndex = 8
        '
        'lbl_契約区分
        '
        Me.lbl_契約区分.Location = New System.Drawing.Point(76, 42)
        Me.lbl_契約区分.Name = "lbl_契約区分"
        Me.lbl_契約区分.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約区分.TabIndex = 9
        Me.lbl_契約区分.Text = "契約区分"
        '
        'cmb_KKBN_ID
        '
        Me.cmb_KKBN_ID.Location = New System.Drawing.Point(151, 42)
        Me.cmb_KKBN_ID.Name = "cmb_KKBN_ID"
        Me.cmb_KKBN_ID.Size = New System.Drawing.Size(114, 20)
        Me.cmb_KKBN_ID.TabIndex = 10
        '
        'lbl_支払先
        '
        Me.lbl_支払先.Location = New System.Drawing.Point(283, 42)
        Me.lbl_支払先.Name = "lbl_支払先"
        Me.lbl_支払先.Size = New System.Drawing.Size(76, 15)
        Me.lbl_支払先.TabIndex = 11
        Me.lbl_支払先.Text = "支払先"
        '
        'cmb_LCPT_ID
        '
        Me.cmb_LCPT_ID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmb_LCPT_ID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmb_LCPT_ID.Location = New System.Drawing.Point(359, 42)
        Me.cmb_LCPT_ID.Name = "cmb_LCPT_ID"
        Me.cmb_LCPT_ID.Size = New System.Drawing.Size(95, 20)
        Me.cmb_LCPT_ID.TabIndex = 12
        '
        'txt_LCPT_NM
        '
        Me.txt_LCPT_NM.Location = New System.Drawing.Point(454, 42)
        Me.txt_LCPT_NM.Name = "txt_LCPT_NM"
        Me.txt_LCPT_NM.Size = New System.Drawing.Size(226, 19)
        Me.txt_LCPT_NM.TabIndex = 13
        '
        'lbl_契約番号
        '
        Me.lbl_契約番号.Location = New System.Drawing.Point(75, 61)
        Me.lbl_契約番号.Name = "lbl_契約番号"
        Me.lbl_契約番号.Size = New System.Drawing.Size(83, 15)
        Me.lbl_契約番号.TabIndex = 14
        Me.lbl_契約番号.Text = "契約番号"
        '
        'txt_KYAK_NO
        '
        Me.txt_KYAK_NO.Location = New System.Drawing.Point(158, 61)
        Me.txt_KYAK_NO.Name = "txt_KYAK_NO"
        Me.txt_KYAK_NO.Size = New System.Drawing.Size(113, 19)
        Me.txt_KYAK_NO.TabIndex = 15
        '
        'lbl_契約日
        '
        Me.lbl_契約日.Location = New System.Drawing.Point(283, 61)
        Me.lbl_契約日.Name = "lbl_契約日"
        Me.lbl_契約日.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約日.TabIndex = 16
        Me.lbl_契約日.Text = "契約日"
        '
        'dtp_KYAK_DT
        '
        Me.dtp_KYAK_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_KYAK_DT.Location = New System.Drawing.Point(359, 61)
        Me.dtp_KYAK_DT.Name = "dtp_KYAK_DT"
        Me.dtp_KYAK_DT.Size = New System.Drawing.Size(130, 19)
        Me.dtp_KYAK_DT.TabIndex = 17
        '
        'lbl_再リース回数
        '
        Me.lbl_再リース回数.Location = New System.Drawing.Point(505, 63)
        Me.lbl_再リース回数.Name = "lbl_再リース回数"
        Me.lbl_再リース回数.Size = New System.Drawing.Size(76, 15)
        Me.lbl_再リース回数.TabIndex = 18
        Me.lbl_再リース回数.Text = "再リース回数"
        '
        'txt_SAIKAISU
        '
        Me.txt_SAIKAISU.Location = New System.Drawing.Point(580, 61)
        Me.txt_SAIKAISU.Name = "txt_SAIKAISU"
        Me.txt_SAIKAISU.Size = New System.Drawing.Size(68, 19)
        Me.txt_SAIKAISU.TabIndex = 19
        Me.txt_SAIKAISU.Text = "0"
        Me.txt_SAIKAISU.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_契約終了
        '
        Me.lbl_契約終了.Location = New System.Drawing.Point(656, 63)
        Me.lbl_契約終了.Name = "lbl_契約終了"
        Me.lbl_契約終了.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約終了.TabIndex = 20
        Me.lbl_契約終了.Text = "契約終了"
        '
        'chk_KYAK_END_F
        '
        Me.chk_KYAK_END_F.Location = New System.Drawing.Point(739, 63)
        Me.chk_KYAK_END_F.Name = "chk_KYAK_END_F"
        Me.chk_KYAK_END_F.Size = New System.Drawing.Size(15, 14)
        Me.chk_KYAK_END_F.TabIndex = 21
        '
        'lbl_自社管理用
        '
        Me.lbl_自社管理用.Location = New System.Drawing.Point(75, 76)
        Me.lbl_自社管理用.Name = "lbl_自社管理用"
        Me.lbl_自社管理用.Size = New System.Drawing.Size(83, 15)
        Me.lbl_自社管理用.TabIndex = 22
        Me.lbl_自社管理用.Text = "自社管理用"
        '
        'txt_JISH_KYAK_NO
        '
        Me.txt_JISH_KYAK_NO.Location = New System.Drawing.Point(158, 76)
        Me.txt_JISH_KYAK_NO.Name = "txt_JISH_KYAK_NO"
        Me.txt_JISH_KYAK_NO.Size = New System.Drawing.Size(113, 19)
        Me.txt_JISH_KYAK_NO.TabIndex = 23
        '
        'lbl_開始日
        '
        Me.lbl_開始日.Location = New System.Drawing.Point(283, 76)
        Me.lbl_開始日.Name = "lbl_開始日"
        Me.lbl_開始日.Size = New System.Drawing.Size(76, 15)
        Me.lbl_開始日.TabIndex = 24
        Me.lbl_開始日.Text = "開始日"
        '
        'dtp_START_DT
        '
        Me.dtp_START_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_START_DT.Location = New System.Drawing.Point(359, 76)
        Me.dtp_START_DT.Name = "dtp_START_DT"
        Me.dtp_START_DT.Size = New System.Drawing.Size(130, 19)
        Me.dtp_START_DT.TabIndex = 25
        '
        'lbl_契約期間
        '
        Me.lbl_契約期間.Location = New System.Drawing.Point(505, 80)
        Me.lbl_契約期間.Name = "lbl_契約期間"
        Me.lbl_契約期間.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約期間.TabIndex = 26
        Me.lbl_契約期間.Text = "契約期間"
        '
        'txt_LKIKAN
        '
        Me.txt_LKIKAN.Location = New System.Drawing.Point(580, 76)
        Me.txt_LKIKAN.Name = "txt_LKIKAN"
        Me.txt_LKIKAN.Size = New System.Drawing.Size(38, 19)
        Me.txt_LKIKAN.TabIndex = 27
        Me.txt_LKIKAN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_M2
        '
        Me.Label_M2.Location = New System.Drawing.Point(0, 0)
        Me.Label_M2.Name = "Label_M2"
        Me.Label_M2.Size = New System.Drawing.Size(100, 23)
        Me.Label_M2.TabIndex = 28
        '
        'lbl_稟議番号
        '
        Me.lbl_稟議番号.Location = New System.Drawing.Point(75, 91)
        Me.lbl_稟議番号.Name = "lbl_稟議番号"
        Me.lbl_稟議番号.Size = New System.Drawing.Size(83, 15)
        Me.lbl_稟議番号.TabIndex = 29
        Me.lbl_稟議番号.Text = "稟議番号"
        '
        'txt_RNG_BANGO
        '
        Me.txt_RNG_BANGO.Location = New System.Drawing.Point(158, 91)
        Me.txt_RNG_BANGO.Name = "txt_RNG_BANGO"
        Me.txt_RNG_BANGO.Size = New System.Drawing.Size(113, 19)
        Me.txt_RNG_BANGO.TabIndex = 30
        '
        'lbl_終了日
        '
        Me.lbl_終了日.Location = New System.Drawing.Point(283, 91)
        Me.lbl_終了日.Name = "lbl_終了日"
        Me.lbl_終了日.Size = New System.Drawing.Size(76, 15)
        Me.lbl_終了日.TabIndex = 31
        Me.lbl_終了日.Text = "終了日"
        '
        'dtp_END_DT
        '
        Me.dtp_END_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_END_DT.Location = New System.Drawing.Point(359, 91)
        Me.dtp_END_DT.Name = "dtp_END_DT"
        Me.dtp_END_DT.Size = New System.Drawing.Size(130, 19)
        Me.dtp_END_DT.TabIndex = 32
        '
        'lbl_遡及計上
        '
        Me.lbl_遡及計上.Location = New System.Drawing.Point(656, 80)
        Me.lbl_遡及計上.Name = "lbl_遡及計上"
        Me.lbl_遡及計上.Size = New System.Drawing.Size(76, 15)
        Me.lbl_遡及計上.TabIndex = 33
        Me.lbl_遡及計上.Text = "遡及計上"
        '
        'chk_SKYU_KJ_F
        '
        Me.chk_SKYU_KJ_F.Location = New System.Drawing.Point(739, 81)
        Me.chk_SKYU_KJ_F.Name = "chk_SKYU_KJ_F"
        Me.chk_SKYU_KJ_F.Size = New System.Drawing.Size(15, 14)
        Me.chk_SKYU_KJ_F.TabIndex = 34
        '
        'lbl_支払間隔
        '
        Me.lbl_支払間隔.Location = New System.Drawing.Point(75, 109)
        Me.lbl_支払間隔.Name = "lbl_支払間隔"
        Me.lbl_支払間隔.Size = New System.Drawing.Size(76, 15)
        Me.lbl_支払間隔.TabIndex = 35
        Me.lbl_支払間隔.Text = "支払間隔"
        '
        'txt_SHRI_KN
        '
        Me.txt_SHRI_KN.Location = New System.Drawing.Point(75, 124)
        Me.txt_SHRI_KN.Name = "txt_SHRI_KN"
        Me.txt_SHRI_KN.Size = New System.Drawing.Size(30, 19)
        Me.txt_SHRI_KN.TabIndex = 36
        Me.txt_SHRI_KN.Text = "1"
        Me.txt_SHRI_KN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_M3
        '
        Me.Label_M3.Location = New System.Drawing.Point(105, 124)
        Me.Label_M3.Name = "Label_M3"
        Me.Label_M3.Size = New System.Drawing.Size(45, 15)
        Me.Label_M3.TabIndex = 37
        Me.Label_M3.Text = "ヶ月"
        '
        'lbl_前払回数
        '
        Me.lbl_前払回数.Location = New System.Drawing.Point(151, 109)
        Me.lbl_前払回数.Name = "lbl_前払回数"
        Me.lbl_前払回数.Size = New System.Drawing.Size(76, 15)
        Me.lbl_前払回数.TabIndex = 38
        Me.lbl_前払回数.Text = "前払回数"
        '
        'txt_MKAISU
        '
        Me.txt_MKAISU.Location = New System.Drawing.Point(151, 124)
        Me.txt_MKAISU.Name = "txt_MKAISU"
        Me.txt_MKAISU.Size = New System.Drawing.Size(30, 19)
        Me.txt_MKAISU.TabIndex = 39
        Me.txt_MKAISU.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_M4
        '
        Me.Label_M4.Location = New System.Drawing.Point(181, 124)
        Me.Label_M4.Name = "Label_M4"
        Me.Label_M4.Size = New System.Drawing.Size(45, 15)
        Me.Label_M4.TabIndex = 40
        Me.Label_M4.Text = "回"
        '
        'lbl_支払回数
        '
        Me.lbl_支払回数.Location = New System.Drawing.Point(226, 109)
        Me.lbl_支払回数.Name = "lbl_支払回数"
        Me.lbl_支払回数.Size = New System.Drawing.Size(76, 15)
        Me.lbl_支払回数.TabIndex = 41
        Me.lbl_支払回数.Text = "支払回数"
        '
        'txt_SHRI_CNT
        '
        Me.txt_SHRI_CNT.Location = New System.Drawing.Point(226, 124)
        Me.txt_SHRI_CNT.Name = "txt_SHRI_CNT"
        Me.txt_SHRI_CNT.Size = New System.Drawing.Size(30, 19)
        Me.txt_SHRI_CNT.TabIndex = 42
        Me.txt_SHRI_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_M5
        '
        Me.Label_M5.Location = New System.Drawing.Point(256, 124)
        Me.Label_M5.Name = "Label_M5"
        Me.Label_M5.Size = New System.Drawing.Size(45, 15)
        Me.Label_M5.TabIndex = 43
        Me.Label_M5.Text = "回"
        '
        'lbl_前払日
        '
        Me.lbl_前払日.Location = New System.Drawing.Point(302, 109)
        Me.lbl_前払日.Name = "lbl_前払日"
        Me.lbl_前払日.Size = New System.Drawing.Size(76, 15)
        Me.lbl_前払日.TabIndex = 44
        Me.lbl_前払日.Text = "前払日"
        '
        'txt_MAE_DT
        '
        Me.txt_MAE_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txt_MAE_DT.Location = New System.Drawing.Point(302, 124)
        Me.txt_MAE_DT.Name = "txt_MAE_DT"
        Me.txt_MAE_DT.ShowCheckBox = True
        Me.txt_MAE_DT.Size = New System.Drawing.Size(113, 19)
        Me.txt_MAE_DT.TabIndex = 45
        '
        'lbl_第1回支払日
        '
        Me.lbl_第1回支払日.Location = New System.Drawing.Point(418, 109)
        Me.lbl_第1回支払日.Name = "lbl_第1回支払日"
        Me.lbl_第1回支払日.Size = New System.Drawing.Size(79, 15)
        Me.lbl_第1回支払日.TabIndex = 46
        Me.lbl_第1回支払日.Text = "第1回支払日"
        '
        'txt_SHRI_DT1
        '
        Me.txt_SHRI_DT1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txt_SHRI_DT1.Location = New System.Drawing.Point(420, 123)
        Me.txt_SHRI_DT1.Name = "txt_SHRI_DT1"
        Me.txt_SHRI_DT1.Size = New System.Drawing.Size(99, 19)
        Me.txt_SHRI_DT1.TabIndex = 47
        '
        'lbl_第2回支払日
        '
        Me.lbl_第2回支払日.Location = New System.Drawing.Point(528, 109)
        Me.lbl_第2回支払日.Name = "lbl_第2回支払日"
        Me.lbl_第2回支払日.Size = New System.Drawing.Size(79, 15)
        Me.lbl_第2回支払日.TabIndex = 48
        Me.lbl_第2回支払日.Text = "第2回支払日"
        '
        'txt_SHRI_DT2
        '
        Me.txt_SHRI_DT2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txt_SHRI_DT2.Location = New System.Drawing.Point(528, 122)
        Me.txt_SHRI_DT2.Name = "txt_SHRI_DT2"
        Me.txt_SHRI_DT2.Size = New System.Drawing.Size(97, 19)
        Me.txt_SHRI_DT2.TabIndex = 49
        '
        'lbl_第3回以降
        '
        Me.lbl_第3回以降.Location = New System.Drawing.Point(630, 109)
        Me.lbl_第3回以降.Name = "lbl_第3回以降"
        Me.lbl_第3回以降.Size = New System.Drawing.Size(64, 15)
        Me.lbl_第3回以降.TabIndex = 50
        Me.lbl_第3回以降.Text = "第3回以降"
        '
        'txt_SHRI_DT3
        '
        Me.txt_SHRI_DT3.Location = New System.Drawing.Point(630, 122)
        Me.txt_SHRI_DT3.Name = "txt_SHRI_DT3"
        Me.txt_SHRI_DT3.Size = New System.Drawing.Size(30, 19)
        Me.txt_SHRI_DT3.TabIndex = 51
        Me.txt_SHRI_DT3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label_M6
        '
        Me.Label_M6.Location = New System.Drawing.Point(660, 126)
        Me.Label_M6.Name = "Label_M6"
        Me.Label_M6.Size = New System.Drawing.Size(34, 19)
        Me.Label_M6.TabIndex = 52
        Me.Label_M6.Text = "日"
        '
        'lbl_最終支払日
        '
        Me.lbl_最終支払日.Location = New System.Drawing.Point(691, 106)
        Me.lbl_最終支払日.Name = "lbl_最終支払日"
        Me.lbl_最終支払日.Size = New System.Drawing.Size(76, 15)
        Me.lbl_最終支払日.TabIndex = 53
        Me.lbl_最終支払日.Text = "最終支払日"
        '
        'txt_SHRI_EN_DT
        '
        Me.txt_SHRI_EN_DT.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txt_SHRI_EN_DT.Location = New System.Drawing.Point(691, 123)
        Me.txt_SHRI_EN_DT.Name = "txt_SHRI_EN_DT"
        Me.txt_SHRI_EN_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_SHRI_EN_DT.TabIndex = 54
        '
        'lbl_現金購入価額
        '
        Me.lbl_現金購入価額.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lbl_現金購入価額.ForeColor = System.Drawing.Color.White
        Me.lbl_現金購入価額.Location = New System.Drawing.Point(7, 144)
        Me.lbl_現金購入価額.Name = "lbl_現金購入価額"
        Me.lbl_現金購入価額.Size = New System.Drawing.Size(95, 15)
        Me.lbl_現金購入価額.TabIndex = 55
        Me.lbl_現金購入価額.Text = "現金購入価額"
        '
        'txt_KNYUKN
        '
        Me.txt_KNYUKN.Location = New System.Drawing.Point(102, 144)
        Me.txt_KNYUKN.Name = "txt_KNYUKN"
        Me.txt_KNYUKN.Size = New System.Drawing.Size(95, 19)
        Me.txt_KNYUKN.TabIndex = 56
        Me.txt_KNYUKN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmd_料率
        '
        Me.cmd_料率.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmd_料率.ForeColor = System.Drawing.Color.Yellow
        Me.cmd_料率.Location = New System.Drawing.Point(7, 166)
        Me.cmd_料率.Name = "cmd_料率"
        Me.cmd_料率.Size = New System.Drawing.Size(95, 23)
        Me.cmd_料率.TabIndex = 57
        Me.cmd_料率.Text = "料率(&R)"
        Me.cmd_料率.UseVisualStyleBackColor = False
        '
        'txt_RYORITU
        '
        Me.txt_RYORITU.Location = New System.Drawing.Point(102, 166)
        Me.txt_RYORITU.Name = "txt_RYORITU"
        Me.txt_RYORITU.Size = New System.Drawing.Size(76, 19)
        Me.txt_RYORITU.TabIndex = 58
        Me.txt_RYORITU.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmd_税率
        '
        Me.cmd_税率.Location = New System.Drawing.Point(7, 185)
        Me.cmd_税率.Name = "cmd_税率"
        Me.cmd_税率.Size = New System.Drawing.Size(95, 23)
        Me.cmd_税率.TabIndex = 59
        Me.cmd_税率.Text = "消費税率(&Z)"
        '
        'cmb_ZRITU
        '
        Me.cmb_ZRITU.Location = New System.Drawing.Point(102, 184)
        Me.cmb_ZRITU.Name = "cmb_ZRITU"
        Me.cmb_ZRITU.Size = New System.Drawing.Size(76, 20)
        Me.cmb_ZRITU.TabIndex = 60
        '
        'lbl_支払方法
        '
        Me.lbl_支払方法.Location = New System.Drawing.Point(279, 143)
        Me.lbl_支払方法.Name = "lbl_支払方法"
        Me.lbl_支払方法.Size = New System.Drawing.Size(76, 15)
        Me.lbl_支払方法.TabIndex = 61
        Me.lbl_支払方法.Text = "支払方法"
        '
        'cmb_SHHO_M_ID
        '
        Me.cmb_SHHO_M_ID.Location = New System.Drawing.Point(279, 158)
        Me.cmb_SHHO_M_ID.Name = "cmb_SHHO_M_ID"
        Me.cmb_SHHO_M_ID.Size = New System.Drawing.Size(76, 20)
        Me.cmb_SHHO_M_ID.TabIndex = 62
        '
        'cmb_SHHO_1_ID
        '
        Me.cmb_SHHO_1_ID.Location = New System.Drawing.Point(279, 173)
        Me.cmb_SHHO_1_ID.Name = "cmb_SHHO_1_ID"
        Me.cmb_SHHO_1_ID.Size = New System.Drawing.Size(76, 20)
        Me.cmb_SHHO_1_ID.TabIndex = 63
        '
        'cmb_SHHO_2_ID
        '
        Me.cmb_SHHO_2_ID.Location = New System.Drawing.Point(279, 189)
        Me.cmb_SHHO_2_ID.Name = "cmb_SHHO_2_ID"
        Me.cmb_SHHO_2_ID.Size = New System.Drawing.Size(76, 20)
        Me.cmb_SHHO_2_ID.TabIndex = 64
        '
        'cmb_SHHO_3_ID
        '
        Me.cmb_SHHO_3_ID.Location = New System.Drawing.Point(279, 204)
        Me.cmb_SHHO_3_ID.Name = "cmb_SHHO_3_ID"
        Me.cmb_SHHO_3_ID.Size = New System.Drawing.Size(76, 20)
        Me.cmb_SHHO_3_ID.TabIndex = 65
        '
        'lbl_月額
        '
        Me.lbl_月額.BackColor = System.Drawing.Color.Blue
        Me.lbl_月額.ForeColor = System.Drawing.Color.White
        Me.lbl_月額.Location = New System.Drawing.Point(434, 165)
        Me.lbl_月額.Name = "lbl_月額"
        Me.lbl_月額.Size = New System.Drawing.Size(95, 15)
        Me.lbl_月額.TabIndex = 66
        Me.lbl_月額.Text = "月額"
        '
        'txt_GLSRYO
        '
        Me.txt_GLSRYO.Location = New System.Drawing.Point(529, 165)
        Me.txt_GLSRYO.Name = "txt_GLSRYO"
        Me.txt_GLSRYO.Size = New System.Drawing.Size(95, 19)
        Me.txt_GLSRYO.TabIndex = 67
        Me.txt_GLSRYO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_期間額
        '
        Me.lbl_期間額.BackColor = System.Drawing.Color.Blue
        Me.lbl_期間額.ForeColor = System.Drawing.Color.White
        Me.lbl_期間額.Location = New System.Drawing.Point(434, 180)
        Me.lbl_期間額.Name = "lbl_期間額"
        Me.lbl_期間額.Size = New System.Drawing.Size(95, 15)
        Me.lbl_期間額.TabIndex = 68
        Me.lbl_期間額.Text = "１支払額"
        '
        'txt_KLSRYO
        '
        Me.txt_KLSRYO.Location = New System.Drawing.Point(529, 180)
        Me.txt_KLSRYO.Name = "txt_KLSRYO"
        Me.txt_KLSRYO.Size = New System.Drawing.Size(95, 19)
        Me.txt_KLSRYO.TabIndex = 69
        Me.txt_KLSRYO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_前払
        '
        Me.lbl_前払.BackColor = System.Drawing.Color.Blue
        Me.lbl_前払.ForeColor = System.Drawing.Color.White
        Me.lbl_前払.Location = New System.Drawing.Point(434, 196)
        Me.lbl_前払.Name = "lbl_前払"
        Me.lbl_前払.Size = New System.Drawing.Size(95, 15)
        Me.lbl_前払.TabIndex = 70
        Me.lbl_前払.Text = "前払"
        '
        'txt_MLSRYO
        '
        Me.txt_MLSRYO.Location = New System.Drawing.Point(529, 196)
        Me.txt_MLSRYO.Name = "txt_MLSRYO"
        Me.txt_MLSRYO.Size = New System.Drawing.Size(95, 19)
        Me.txt_MLSRYO.TabIndex = 71
        Me.txt_MLSRYO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_総額
        '
        Me.lbl_総額.BackColor = System.Drawing.Color.Blue
        Me.lbl_総額.ForeColor = System.Drawing.Color.White
        Me.lbl_総額.Location = New System.Drawing.Point(434, 211)
        Me.lbl_総額.Name = "lbl_総額"
        Me.lbl_総額.Size = New System.Drawing.Size(95, 15)
        Me.lbl_総額.TabIndex = 72
        Me.lbl_総額.Text = "総額"
        '
        'txt_SLSRYO
        '
        Me.txt_SLSRYO.Location = New System.Drawing.Point(529, 211)
        Me.txt_SLSRYO.Name = "txt_SLSRYO"
        Me.txt_SLSRYO.Size = New System.Drawing.Size(95, 19)
        Me.txt_SLSRYO.TabIndex = 73
        Me.txt_SLSRYO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_税抜き
        '
        Me.lbl_税抜き.Location = New System.Drawing.Point(529, 150)
        Me.lbl_税抜き.Name = "lbl_税抜き"
        Me.lbl_税抜き.Size = New System.Drawing.Size(95, 15)
        Me.lbl_税抜き.TabIndex = 74
        Me.lbl_税抜き.Text = "税抜き"
        '
        'lbl_消費税
        '
        Me.lbl_消費税.Location = New System.Drawing.Point(624, 150)
        Me.lbl_消費税.Name = "lbl_消費税"
        Me.lbl_消費税.Size = New System.Drawing.Size(95, 15)
        Me.lbl_消費税.TabIndex = 75
        Me.lbl_消費税.Text = "消費税"
        '
        'txt_GZEI
        '
        Me.txt_GZEI.Location = New System.Drawing.Point(623, 165)
        Me.txt_GZEI.Name = "txt_GZEI"
        Me.txt_GZEI.Size = New System.Drawing.Size(95, 19)
        Me.txt_GZEI.TabIndex = 76
        Me.txt_GZEI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_KZEI
        '
        Me.txt_KZEI.Location = New System.Drawing.Point(623, 180)
        Me.txt_KZEI.Name = "txt_KZEI"
        Me.txt_KZEI.Size = New System.Drawing.Size(95, 19)
        Me.txt_KZEI.TabIndex = 77
        Me.txt_KZEI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_MZEI
        '
        Me.txt_MZEI.Location = New System.Drawing.Point(623, 196)
        Me.txt_MZEI.Name = "txt_MZEI"
        Me.txt_MZEI.Size = New System.Drawing.Size(95, 19)
        Me.txt_MZEI.TabIndex = 78
        Me.txt_MZEI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_税込み
        '
        Me.lbl_税込み.Location = New System.Drawing.Point(718, 150)
        Me.lbl_税込み.Name = "lbl_税込み"
        Me.lbl_税込み.Size = New System.Drawing.Size(95, 15)
        Me.lbl_税込み.TabIndex = 79
        Me.lbl_税込み.Text = "税込み"
        '
        'txt_GLSRYO_ZEIKOMI
        '
        Me.txt_GLSRYO_ZEIKOMI.Location = New System.Drawing.Point(718, 165)
        Me.txt_GLSRYO_ZEIKOMI.Name = "txt_GLSRYO_ZEIKOMI"
        Me.txt_GLSRYO_ZEIKOMI.Size = New System.Drawing.Size(95, 19)
        Me.txt_GLSRYO_ZEIKOMI.TabIndex = 80
        Me.txt_GLSRYO_ZEIKOMI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_KLSRYO_ZKOMI
        '
        Me.txt_KLSRYO_ZKOMI.Location = New System.Drawing.Point(718, 180)
        Me.txt_KLSRYO_ZKOMI.Name = "txt_KLSRYO_ZKOMI"
        Me.txt_KLSRYO_ZKOMI.Size = New System.Drawing.Size(95, 19)
        Me.txt_KLSRYO_ZKOMI.TabIndex = 81
        Me.txt_KLSRYO_ZKOMI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txt_MLSRYO_ZKOMI
        '
        Me.txt_MLSRYO_ZKOMI.Location = New System.Drawing.Point(718, 196)
        Me.txt_MLSRYO_ZKOMI.Name = "txt_MLSRYO_ZKOMI"
        Me.txt_MLSRYO_ZKOMI.Size = New System.Drawing.Size(95, 19)
        Me.txt_MLSRYO_ZKOMI.TabIndex = 82
        Me.txt_MLSRYO_ZKOMI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_維持管理費用
        '
        Me.lbl_維持管理費用.BackColor = System.Drawing.Color.Blue
        Me.lbl_維持管理費用.ForeColor = System.Drawing.Color.White
        Me.lbl_維持管理費用.Location = New System.Drawing.Point(434, 226)
        Me.lbl_維持管理費用.Name = "lbl_維持管理費用"
        Me.lbl_維持管理費用.Size = New System.Drawing.Size(95, 15)
        Me.lbl_維持管理費用.TabIndex = 83
        Me.lbl_維持管理費用.Text = "内維持管理費用"
        '
        'txt_IJIKNR
        '
        Me.txt_IJIKNR.Location = New System.Drawing.Point(529, 226)
        Me.txt_IJIKNR.Name = "txt_IJIKNR"
        Me.txt_IJIKNR.Size = New System.Drawing.Size(95, 19)
        Me.txt_IJIKNR.TabIndex = 84
        Me.txt_IJIKNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_残価保証額
        '
        Me.lbl_残価保証額.BackColor = System.Drawing.Color.Blue
        Me.lbl_残価保証額.ForeColor = System.Drawing.Color.White
        Me.lbl_残価保証額.Location = New System.Drawing.Point(434, 241)
        Me.lbl_残価保証額.Name = "lbl_残価保証額"
        Me.lbl_残価保証額.Size = New System.Drawing.Size(95, 15)
        Me.lbl_残価保証額.TabIndex = 85
        Me.lbl_残価保証額.Text = "残価保証額"
        '
        'txt_ZANRYO
        '
        Me.txt_ZANRYO.Location = New System.Drawing.Point(529, 241)
        Me.txt_ZANRYO.Name = "txt_ZANRYO"
        Me.txt_ZANRYO.Size = New System.Drawing.Size(95, 19)
        Me.txt_ZANRYO.TabIndex = 86
        Me.txt_ZANRYO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lbl_銀行口座
        '
        Me.lbl_銀行口座.Location = New System.Drawing.Point(7, 226)
        Me.lbl_銀行口座.Name = "lbl_銀行口座"
        Me.lbl_銀行口座.Size = New System.Drawing.Size(76, 15)
        Me.lbl_銀行口座.TabIndex = 87
        Me.lbl_銀行口座.Text = "銀行口座"
        '
        'cmb_KOZA_ID
        '
        Me.cmb_KOZA_ID.Location = New System.Drawing.Point(83, 226)
        Me.cmb_KOZA_ID.Name = "cmb_KOZA_ID"
        Me.cmb_KOZA_ID.Size = New System.Drawing.Size(95, 20)
        Me.cmb_KOZA_ID.TabIndex = 88
        '
        'txt_KOZA_NM
        '
        Me.txt_KOZA_NM.Location = New System.Drawing.Point(177, 226)
        Me.txt_KOZA_NM.Name = "txt_KOZA_NM"
        Me.txt_KOZA_NM.ReadOnly = True
        Me.txt_KOZA_NM.Size = New System.Drawing.Size(151, 19)
        Me.txt_KOZA_NM.TabIndex = 89
        '
        'lbl_契約予備
        '
        Me.lbl_契約予備.Location = New System.Drawing.Point(7, 245)
        Me.lbl_契約予備.Name = "lbl_契約予備"
        Me.lbl_契約予備.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約予備.TabIndex = 90
        Me.lbl_契約予備.Text = "契約予備"
        '
        'cmb_RSRVK1_ID
        '
        Me.cmb_RSRVK1_ID.Location = New System.Drawing.Point(83, 245)
        Me.cmb_RSRVK1_ID.Name = "cmb_RSRVK1_ID"
        Me.cmb_RSRVK1_ID.Size = New System.Drawing.Size(95, 20)
        Me.cmb_RSRVK1_ID.TabIndex = 91
        '
        'txt_RSRVK1_NM
        '
        Me.txt_RSRVK1_NM.Location = New System.Drawing.Point(177, 245)
        Me.txt_RSRVK1_NM.Name = "txt_RSRVK1_NM"
        Me.txt_RSRVK1_NM.ReadOnly = True
        Me.txt_RSRVK1_NM.Size = New System.Drawing.Size(151, 19)
        Me.txt_RSRVK1_NM.TabIndex = 92
        '
        'lbl_備考
        '
        Me.lbl_備考.Location = New System.Drawing.Point(149, 268)
        Me.lbl_備考.Name = "lbl_備考"
        Me.lbl_備考.Size = New System.Drawing.Size(76, 15)
        Me.lbl_備考.TabIndex = 93
        Me.lbl_備考.Text = "備考"
        '
        'txt_K_ZOKUSEI1
        '
        Me.txt_K_ZOKUSEI1.Location = New System.Drawing.Point(226, 266)
        Me.txt_K_ZOKUSEI1.Name = "txt_K_ZOKUSEI1"
        Me.txt_K_ZOKUSEI1.Size = New System.Drawing.Size(461, 19)
        Me.txt_K_ZOKUSEI1.TabIndex = 94
        '
        'lbl_契約名
        '
        Me.lbl_契約名.Location = New System.Drawing.Point(149, 287)
        Me.lbl_契約名.Name = "lbl_契約名"
        Me.lbl_契約名.Size = New System.Drawing.Size(76, 15)
        Me.lbl_契約名.TabIndex = 95
        Me.lbl_契約名.Text = "契約名"
        '
        'txt_KYAK_NM
        '
        Me.txt_KYAK_NM.Location = New System.Drawing.Point(226, 287)
        Me.txt_KYAK_NM.Name = "txt_KYAK_NM"
        Me.txt_KYAK_NM.Size = New System.Drawing.Size(461, 19)
        Me.txt_KYAK_NM.TabIndex = 96
        '
        'cmd_物件画面
        '
        Me.cmd_物件画面.Location = New System.Drawing.Point(30, 287)
        Me.cmd_物件画面.Name = "cmd_物件画面"
        Me.cmd_物件画面.Size = New System.Drawing.Size(95, 30)
        Me.cmd_物件画面.TabIndex = 97
        Me.cmd_物件画面.Text = "物件画面(&B)"
        '
        'tab_f_KYKH_SUB
        '
        Me.tab_f_KYKH_SUB.Controls.Add(Me.TabPage1)
        Me.tab_f_KYKH_SUB.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tab_f_KYKH_SUB.Location = New System.Drawing.Point(0, 317)
        Me.tab_f_KYKH_SUB.Name = "tab_f_KYKH_SUB"
        Me.tab_f_KYKH_SUB.SelectedIndex = 0
        Me.tab_f_KYKH_SUB.Size = New System.Drawing.Size(883, 250)
        Me.tab_f_KYKH_SUB.TabIndex = 98
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.dgv_DETAILS)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(875, 224)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "現在の物件(1)"
        '
        'dgv_DETAILS
        '
        Me.dgv_DETAILS.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_KYKM_ID, Me.col_BUKN_NM, Me.col_SUURYO, Me.col_KNYUKN, Me.col_GLSRYO})
        Me.dgv_DETAILS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv_DETAILS.Location = New System.Drawing.Point(0, 0)
        Me.dgv_DETAILS.Name = "dgv_DETAILS"
        Me.dgv_DETAILS.Size = New System.Drawing.Size(875, 224)
        Me.dgv_DETAILS.TabIndex = 0
        '
        'txt_KYKH_ID
        '
        Me.txt_KYKH_ID.Location = New System.Drawing.Point(0, 0)
        Me.txt_KYKH_ID.Name = "txt_KYKH_ID"
        Me.txt_KYKH_ID.Size = New System.Drawing.Size(50, 19)
        Me.txt_KYKH_ID.TabIndex = 2
        Me.txt_KYKH_ID.Text = "0"
        Me.txt_KYKH_ID.Visible = False
        '
        'txt_KYKH_NO
        '
        Me.txt_KYKH_NO.Location = New System.Drawing.Point(0, 0)
        Me.txt_KYKH_NO.Name = "txt_KYKH_NO"
        Me.txt_KYKH_NO.Size = New System.Drawing.Size(100, 19)
        Me.txt_KYKH_NO.TabIndex = 3
        Me.txt_KYKH_NO.Text = "0"
        Me.txt_KYKH_NO.Visible = False
        '
        'txt_UPDATE_CNT
        '
        Me.txt_UPDATE_CNT.Location = New System.Drawing.Point(0, 0)
        Me.txt_UPDATE_CNT.Name = "txt_UPDATE_CNT"
        Me.txt_UPDATE_CNT.Size = New System.Drawing.Size(100, 19)
        Me.txt_UPDATE_CNT.TabIndex = 4
        Me.txt_UPDATE_CNT.Text = "0"
        Me.txt_UPDATE_CNT.Visible = False
        '
        'txt_K_CREATE_DT
        '
        Me.txt_K_CREATE_DT.Location = New System.Drawing.Point(0, 0)
        Me.txt_K_CREATE_DT.Name = "txt_K_CREATE_DT"
        Me.txt_K_CREATE_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_K_CREATE_DT.TabIndex = 5
        Me.txt_K_CREATE_DT.Visible = False
        '
        'txt_K_UPDATE_DT
        '
        Me.txt_K_UPDATE_DT.Location = New System.Drawing.Point(0, 0)
        Me.txt_K_UPDATE_DT.Name = "txt_K_UPDATE_DT"
        Me.txt_K_UPDATE_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_K_UPDATE_DT.TabIndex = 6
        Me.txt_K_UPDATE_DT.Visible = False
        '
        'txt_REND_DT
        '
        Me.txt_REND_DT.Location = New System.Drawing.Point(0, 0)
        Me.txt_REND_DT.Name = "txt_REND_DT"
        Me.txt_REND_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_REND_DT.TabIndex = 7
        Me.txt_REND_DT.Visible = False
        '
        'txt_K_KJYO_ST_DT
        '
        Me.txt_K_KJYO_ST_DT.Location = New System.Drawing.Point(0, 0)
        Me.txt_K_KJYO_ST_DT.Name = "txt_K_KJYO_ST_DT"
        Me.txt_K_KJYO_ST_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_K_KJYO_ST_DT.TabIndex = 8
        Me.txt_K_KJYO_ST_DT.Visible = False
        '
        'txt_K_KJYO_EN_DT
        '
        Me.txt_K_KJYO_EN_DT.Location = New System.Drawing.Point(0, 0)
        Me.txt_K_KJYO_EN_DT.Name = "txt_K_KJYO_EN_DT"
        Me.txt_K_KJYO_EN_DT.Size = New System.Drawing.Size(100, 19)
        Me.txt_K_KJYO_EN_DT.TabIndex = 9
        Me.txt_K_KJYO_EN_DT.Visible = False
        '
        'chk_CKAIYK_F
        '
        Me.chk_CKAIYK_F.Location = New System.Drawing.Point(0, 0)
        Me.chk_CKAIYK_F.Name = "chk_CKAIYK_F"
        Me.chk_CKAIYK_F.Size = New System.Drawing.Size(104, 24)
        Me.chk_CKAIYK_F.TabIndex = 10
        Me.chk_CKAIYK_F.Visible = False
        '
        'chk_K_SEIGOU_F
        '
        Me.chk_K_SEIGOU_F.Location = New System.Drawing.Point(0, 0)
        Me.chk_K_SEIGOU_F.Name = "chk_K_SEIGOU_F"
        Me.chk_K_SEIGOU_F.Size = New System.Drawing.Size(104, 24)
        Me.chk_K_SEIGOU_F.TabIndex = 11
        Me.chk_K_SEIGOU_F.Visible = False
        '
        'chk_K_HENF_F
        '
        Me.chk_K_HENF_F.Location = New System.Drawing.Point(0, 0)
        Me.chk_K_HENF_F.Name = "chk_K_HENF_F"
        Me.chk_K_HENF_F.Size = New System.Drawing.Size(104, 24)
        Me.chk_K_HENF_F.TabIndex = 12
        Me.chk_K_HENF_F.Visible = False
        '
        'chk_K_HENL_F
        '
        Me.chk_K_HENL_F.Location = New System.Drawing.Point(0, 0)
        Me.chk_K_HENL_F.Name = "chk_K_HENL_F"
        Me.chk_K_HENL_F.Size = New System.Drawing.Size(104, 24)
        Me.chk_K_HENL_F.TabIndex = 13
        Me.chk_K_HENL_F.Visible = False
        '
        'chk_JENCHO_F
        '
        Me.chk_JENCHO_F.Location = New System.Drawing.Point(0, 0)
        Me.chk_JENCHO_F.Name = "chk_JENCHO_F"
        Me.chk_JENCHO_F.Size = New System.Drawing.Size(104, 24)
        Me.chk_JENCHO_F.TabIndex = 14
        Me.chk_JENCHO_F.Visible = False
        '
        'col_KYKM_ID
        '
        Me.col_KYKM_ID.HeaderText = "物件ID"
        Me.col_KYKM_ID.Name = "col_KYKM_ID"
        Me.col_KYKM_ID.Visible = False
        '
        'col_BUKN_NM
        '
        Me.col_BUKN_NM.HeaderText = "物件名称"
        Me.col_BUKN_NM.Name = "col_BUKN_NM"
        '
        'col_SUURYO
        '
        Me.col_SUURYO.HeaderText = "数量"
        Me.col_SUURYO.Name = "col_SUURYO"
        '
        'col_KNYUKN
        '
        Me.col_KNYUKN.HeaderText = "現金購入価額"
        Me.col_KNYUKN.Name = "col_KNYUKN"
        '
        'col_GLSRYO
        '
        Me.col_GLSRYO.HeaderText = "月額リース料"
        Me.col_GLSRYO.Name = "col_GLSRYO"
        '
        'FrmContractEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 600)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.txt_KYKH_ID)
        Me.Controls.Add(Me.txt_KYKH_NO)
        Me.Controls.Add(Me.txt_UPDATE_CNT)
        Me.Controls.Add(Me.txt_K_CREATE_DT)
        Me.Controls.Add(Me.txt_K_UPDATE_DT)
        Me.Controls.Add(Me.txt_REND_DT)
        Me.Controls.Add(Me.txt_K_KJYO_ST_DT)
        Me.Controls.Add(Me.txt_K_KJYO_EN_DT)
        Me.Controls.Add(Me.chk_CKAIYK_F)
        Me.Controls.Add(Me.chk_K_SEIGOU_F)
        Me.Controls.Add(Me.chk_K_HENF_F)
        Me.Controls.Add(Me.chk_K_HENL_F)
        Me.Controls.Add(Me.chk_JENCHO_F)
        Me.KeyPreview = True
        Me.Name = "FrmContractEntry"
        Me.Text = "契約書"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlDetail.ResumeLayout(False)
        Me.pnlDetail.PerformLayout()
        Me.tab_f_KYKH_SUB.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.dgv_DETAILS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel

    ' Header Buttons
    Friend WithEvents cmd_閉じる As System.Windows.Forms.Button
    Friend WithEvents cmd_TOUROKU As System.Windows.Forms.Button
    Friend WithEvents cmd_REVISE As System.Windows.Forms.Button
    Friend WithEvents cmd_SAILEASE As System.Windows.Forms.Button
    Friend WithEvents cmd_KAIYAKU As System.Windows.Forms.Button
    Friend WithEvents cmd_ROLLBACK_SAI As System.Windows.Forms.Button
    Friend WithEvents cmd_DELETE As System.Windows.Forms.Button
    Friend WithEvents cmd_MODE_RESET As System.Windows.Forms.Button
    Friend WithEvents cmd_取込 As System.Windows.Forms.Button
    Friend WithEvents cmd_CLEAR As System.Windows.Forms.Button

    ' Detail Controls (Access Names)
    Friend WithEvents txt_Mode As System.Windows.Forms.TextBox
    Friend WithEvents Label_M1 As System.Windows.Forms.Label
    Friend WithEvents lbl_計上区分 As System.Windows.Forms.Label
    Friend WithEvents txt_KJKBN_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_適用日 As System.Windows.Forms.Label
    Friend WithEvents txt_TEKIYO_DT As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_管理単位 As System.Windows.Forms.Label
    Friend WithEvents cmb_KKNRI_ID As System.Windows.Forms.ComboBox
    Friend WithEvents txt_KKNRI_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約区分 As System.Windows.Forms.Label
    Friend WithEvents cmb_KKBN_ID As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_支払先 As System.Windows.Forms.Label
    Friend WithEvents cmb_LCPT_ID As System.Windows.Forms.ComboBox
    Friend WithEvents txt_LCPT_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約番号 As System.Windows.Forms.Label
    Friend WithEvents txt_KYAK_NO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約日 As System.Windows.Forms.Label
    Friend WithEvents dtp_KYAK_DT As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_再リース回数 As System.Windows.Forms.Label
    Friend WithEvents txt_SAIKAISU As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約終了 As System.Windows.Forms.Label
    Friend WithEvents chk_KYAK_END_F As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_自社管理用 As System.Windows.Forms.Label
    Friend WithEvents txt_JISH_KYAK_NO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_開始日 As System.Windows.Forms.Label
    Friend WithEvents dtp_START_DT As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_契約期間 As System.Windows.Forms.Label
    Friend WithEvents txt_LKIKAN As System.Windows.Forms.TextBox
    Friend WithEvents Label_M2 As System.Windows.Forms.Label
    Friend WithEvents lbl_稟議番号 As System.Windows.Forms.Label
    Friend WithEvents txt_RNG_BANGO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_終了日 As System.Windows.Forms.Label
    Friend WithEvents dtp_END_DT As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_遡及計上 As System.Windows.Forms.Label
    Friend WithEvents chk_SKYU_KJ_F As System.Windows.Forms.CheckBox

    ' Payment Condition
    Friend WithEvents lbl_支払間隔 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_KN As System.Windows.Forms.TextBox
    Friend WithEvents Label_M3 As System.Windows.Forms.Label
    Friend WithEvents lbl_前払回数 As System.Windows.Forms.Label
    Friend WithEvents txt_MKAISU As System.Windows.Forms.TextBox
    Friend WithEvents Label_M4 As System.Windows.Forms.Label
    Friend WithEvents lbl_支払回数 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_CNT As System.Windows.Forms.TextBox
    Friend WithEvents Label_M5 As System.Windows.Forms.Label
    Friend WithEvents lbl_前払日 As System.Windows.Forms.Label
    Friend WithEvents txt_MAE_DT As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_第1回支払日 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_DT1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_第2回支払日 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_DT2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbl_第3回以降 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_DT3 As System.Windows.Forms.TextBox
    Friend WithEvents Label_M6 As System.Windows.Forms.Label
    Friend WithEvents lbl_最終支払日 As System.Windows.Forms.Label
    Friend WithEvents txt_SHRI_EN_DT As System.Windows.Forms.DateTimePicker

    ' Amount
    Friend WithEvents lbl_現金購入価額 As System.Windows.Forms.Label
    Friend WithEvents txt_KNYUKN As System.Windows.Forms.TextBox
    Friend WithEvents cmd_料率 As System.Windows.Forms.Button
    Friend WithEvents txt_RYORITU As System.Windows.Forms.TextBox
    Friend WithEvents cmd_税率 As System.Windows.Forms.Button
    Friend WithEvents cmb_ZRITU As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_支払方法 As System.Windows.Forms.Label
    Friend WithEvents cmb_SHHO_M_ID As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_SHHO_1_ID As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_SHHO_2_ID As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_SHHO_3_ID As System.Windows.Forms.ComboBox
    Friend WithEvents lbl_月額 As System.Windows.Forms.Label
    Friend WithEvents txt_GLSRYO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_期間額 As System.Windows.Forms.Label
    Friend WithEvents txt_KLSRYO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_前払 As System.Windows.Forms.Label
    Friend WithEvents txt_MLSRYO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_総額 As System.Windows.Forms.Label
    Friend WithEvents txt_SLSRYO As System.Windows.Forms.TextBox
    Friend WithEvents lbl_税抜き As System.Windows.Forms.Label
    Friend WithEvents lbl_消費税 As System.Windows.Forms.Label
    Friend WithEvents txt_GZEI As System.Windows.Forms.TextBox
    Friend WithEvents txt_KZEI As System.Windows.Forms.TextBox
    Friend WithEvents txt_MZEI As System.Windows.Forms.TextBox
    Friend WithEvents lbl_税込み As System.Windows.Forms.Label
    Friend WithEvents txt_GLSRYO_ZEIKOMI As System.Windows.Forms.TextBox
    Friend WithEvents txt_KLSRYO_ZKOMI As System.Windows.Forms.TextBox
    Friend WithEvents txt_MLSRYO_ZKOMI As System.Windows.Forms.TextBox
    Friend WithEvents lbl_維持管理費用 As System.Windows.Forms.Label
    Friend WithEvents txt_IJIKNR As System.Windows.Forms.TextBox
    Friend WithEvents lbl_残価保証額 As System.Windows.Forms.Label
    Friend WithEvents txt_ZANRYO As System.Windows.Forms.TextBox

    ' Others
    Friend WithEvents lbl_銀行口座 As System.Windows.Forms.Label
    Friend WithEvents cmb_KOZA_ID As System.Windows.Forms.ComboBox
    Friend WithEvents txt_KOZA_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約予備 As System.Windows.Forms.Label
    Friend WithEvents cmb_RSRVK1_ID As System.Windows.Forms.ComboBox
    Friend WithEvents txt_RSRVK1_NM As System.Windows.Forms.TextBox
    Friend WithEvents lbl_備考 As System.Windows.Forms.Label
    Friend WithEvents txt_K_ZOKUSEI1 As System.Windows.Forms.TextBox
    Friend WithEvents lbl_契約名 As System.Windows.Forms.Label
    Friend WithEvents txt_KYAK_NM As System.Windows.Forms.TextBox

    ' Subform Area
    Friend WithEvents cmd_物件画面 As System.Windows.Forms.Button
    Friend WithEvents tab_f_KYKH_SUB As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents dgv_DETAILS As System.Windows.Forms.DataGridView
    Friend WithEvents txt_CORP_NM As TextBox

    ' -------------------------------------------------------------
    '  Access定義に基づく 隠しコントロール (ID, フラグ, システム項目)
    ' -------------------------------------------------------------
    Friend WithEvents txt_KYKH_ID As System.Windows.Forms.TextBox
    Friend WithEvents txt_KYKH_NO As System.Windows.Forms.TextBox
    Friend WithEvents txt_UPDATE_CNT As System.Windows.Forms.TextBox
    Friend WithEvents txt_K_CREATE_DT As System.Windows.Forms.TextBox
    Friend WithEvents txt_K_UPDATE_DT As System.Windows.Forms.TextBox
    Friend WithEvents txt_REND_DT As System.Windows.Forms.TextBox
    Friend WithEvents txt_K_KJYO_ST_DT As System.Windows.Forms.TextBox
    Friend WithEvents txt_K_KJYO_EN_DT As System.Windows.Forms.TextBox

    Friend WithEvents chk_CKAIYK_F As System.Windows.Forms.CheckBox
    Friend WithEvents chk_K_SEIGOU_F As System.Windows.Forms.CheckBox
    Friend WithEvents chk_K_HENF_F As System.Windows.Forms.CheckBox
    Friend WithEvents chk_K_HENL_F As System.Windows.Forms.CheckBox
    Friend WithEvents chk_JENCHO_F As System.Windows.Forms.CheckBox
    Friend WithEvents col_KYKM_ID As DataGridViewTextBoxColumn
    Friend WithEvents col_BUKN_NM As DataGridViewTextBoxColumn
    Friend WithEvents col_SUURYO As DataGridViewTextBoxColumn
    Friend WithEvents col_KNYUKN As DataGridViewTextBoxColumn
    Friend WithEvents col_GLSRYO As DataGridViewTextBoxColumn
End Class