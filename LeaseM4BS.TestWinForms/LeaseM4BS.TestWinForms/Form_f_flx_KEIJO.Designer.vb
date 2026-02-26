<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class Form_f_flx_KEIJO

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.cmd_閉じる = New System.Windows.Forms.Button()
        Me.cmd_設定 = New System.Windows.Forms.Button()
        Me.cmd_照会 = New System.Windows.Forms.Button()
        Me.cmd_仕訳出力 = New System.Windows.Forms.Button()
        Me.cmd_計上仕訳 = New System.Windows.Forms.Button()
        Me.cmd_振替伝票 = New System.Windows.Forms.Button()
        Me.cmd_FlexSearchDLG = New System.Windows.Forms.Button()
        Me.cmd_FlexSortDLG = New System.Windows.Forms.Button()
        Me.cmd_FlexReportDLG = New System.Windows.Forms.Button()
        Me.cmd_Output = New System.Windows.Forms.Button()
        Me.dgvMain = New System.Windows.Forms.DataGridView()
        Me.txt_JOKEN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KYKM_NO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SAIKAISU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LINE_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KKBN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_REC_KBN_STR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KJKBN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_HREI_KBN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_TRHK_KBN_STR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KYKBNL = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LEAKBN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_BUKN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LCPT1_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_B_BCAT_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_H_BCAT_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_START_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KEIJO_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_CKAIYK_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SUMIKAISU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SYUTOK_ZOU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SYUTOK_GEN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_KARI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_MHRI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LRSOK_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GRUIKEI_ZOU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GSON_RKEI_ZOU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GSON_TK_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_BOKA_ZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_ZZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_ZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_IDO_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SYUTOK_ZOU_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SYUTOK_GEN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_KARI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_MHRI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LRSOK_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GRUIKEI_ZOU_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GSON_RKEI_ZOU_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_GSON_TK_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_BOKA_ZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_ZZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LGNPN_ZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlHeader.SuspendLayout()
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.cmd_Output)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexReportDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexSortDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexSearchDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_振替伝票)
        Me.pnlHeader.Controls.Add(Me.cmd_計上仕訳)
        Me.pnlHeader.Controls.Add(Me.cmd_仕訳出力)
        Me.pnlHeader.Controls.Add(Me.cmd_照会)
        Me.pnlHeader.Controls.Add(Me.cmd_設定)
        Me.pnlHeader.Controls.Add(Me.cmd_閉じる)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1200, 40)
        Me.pnlHeader.TabIndex = 0
        '
        ' cmd_閉じる
        '
        Me.cmd_閉じる.Location = New System.Drawing.Point(4, 4)
        Me.cmd_閉じる.Name = "cmd_閉じる"
        Me.cmd_閉じる.Size = New System.Drawing.Size(104, 30)
        Me.cmd_閉じる.TabIndex = 0
        Me.cmd_閉じる.Text = "閉じる(&C)"
        Me.cmd_閉じる.UseVisualStyleBackColor = True
        '
        ' cmd_設定
        '
        Me.cmd_設定.Location = New System.Drawing.Point(116, 4)
        Me.cmd_設定.Name = "cmd_設定"
        Me.cmd_設定.Size = New System.Drawing.Size(104, 30)
        Me.cmd_設定.TabIndex = 1
        Me.cmd_設定.Text = "再計算(&D)"
        Me.cmd_設定.UseVisualStyleBackColor = True
        '
        ' cmd_照会
        '
        Me.cmd_照会.Location = New System.Drawing.Point(228, 4)
        Me.cmd_照会.Name = "cmd_照会"
        Me.cmd_照会.Size = New System.Drawing.Size(92, 30)
        Me.cmd_照会.TabIndex = 2
        Me.cmd_照会.Text = "照会(&M)"
        Me.cmd_照会.UseVisualStyleBackColor = True
        '
        ' cmd_仕訳出力
        '
        Me.cmd_仕訳出力.Location = New System.Drawing.Point(328, 4)
        Me.cmd_仕訳出力.Name = "cmd_仕訳出力"
        Me.cmd_仕訳出力.Size = New System.Drawing.Size(116, 30)
        Me.cmd_仕訳出力.TabIndex = 3
        Me.cmd_仕訳出力.Text = "仕訳出力(&J)"
        Me.cmd_仕訳出力.UseVisualStyleBackColor = True
        '
        ' cmd_計上仕訳
        '
        Me.cmd_計上仕訳.Location = New System.Drawing.Point(452, 4)
        Me.cmd_計上仕訳.Name = "cmd_計上仕訳"
        Me.cmd_計上仕訳.Size = New System.Drawing.Size(116, 30)
        Me.cmd_計上仕訳.TabIndex = 4
        Me.cmd_計上仕訳.Text = "計上仕訳(&J)"
        Me.cmd_計上仕訳.UseVisualStyleBackColor = True
        '
        ' cmd_振替伝票
        '
        Me.cmd_振替伝票.Location = New System.Drawing.Point(576, 4)
        Me.cmd_振替伝票.Name = "cmd_振替伝票"
        Me.cmd_振替伝票.Size = New System.Drawing.Size(116, 30)
        Me.cmd_振替伝票.TabIndex = 5
        Me.cmd_振替伝票.Text = "振替伝票(&T)"
        Me.cmd_振替伝票.UseVisualStyleBackColor = True
        '
        ' cmd_FlexSearchDLG
        '
        Me.cmd_FlexSearchDLG.Location = New System.Drawing.Point(700, 4)
        Me.cmd_FlexSearchDLG.Name = "cmd_FlexSearchDLG"
        Me.cmd_FlexSearchDLG.Size = New System.Drawing.Size(92, 30)
        Me.cmd_FlexSearchDLG.TabIndex = 6
        Me.cmd_FlexSearchDLG.Text = "検索(&S)"
        Me.cmd_FlexSearchDLG.UseVisualStyleBackColor = True
        '
        ' cmd_FlexSortDLG
        '
        Me.cmd_FlexSortDLG.Location = New System.Drawing.Point(800, 4)
        Me.cmd_FlexSortDLG.Name = "cmd_FlexSortDLG"
        Me.cmd_FlexSortDLG.Size = New System.Drawing.Size(116, 30)
        Me.cmd_FlexSortDLG.TabIndex = 7
        Me.cmd_FlexSortDLG.Text = "並べ替え(&O)"
        Me.cmd_FlexSortDLG.UseVisualStyleBackColor = True
        '
        ' cmd_FlexReportDLG
        '
        Me.cmd_FlexReportDLG.Location = New System.Drawing.Point(924, 4)
        Me.cmd_FlexReportDLG.Name = "cmd_FlexReportDLG"
        Me.cmd_FlexReportDLG.Size = New System.Drawing.Size(92, 30)
        Me.cmd_FlexReportDLG.TabIndex = 8
        Me.cmd_FlexReportDLG.Text = "印刷(&R)"
        Me.cmd_FlexReportDLG.UseVisualStyleBackColor = True
        '
        ' cmd_Output
        '
        Me.cmd_Output.Location = New System.Drawing.Point(1024, 4)
        Me.cmd_Output.Name = "cmd_Output"
        Me.cmd_Output.Size = New System.Drawing.Size(140, 30)
        Me.cmd_Output.TabIndex = 9
        Me.cmd_Output.Text = "ﾌｧｲﾙ出力(&F)"
        Me.cmd_Output.UseVisualStyleBackColor = True
        '
        ' dgvMain
        '
        Me.dgvMain.AllowUserToAddRows = False
        Me.dgvMain.AllowUserToDeleteRows = False
        Me.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {
            Me.txt_JOKEN, Me.txt_KYKM_NO, Me.txt_SAIKAISU, Me.txt_LINE_ID, Me.txt_KKBN_NM, Me.txt_REC_KBN_STR, Me.txt_KJKBN_NM, Me.txt_HREI_KBN, Me.txt_TRHK_KBN_STR, Me.txt_KYKBNL, Me.txt_LEAKBN, Me.txt_BUKN_NM, Me.txt_LCPT1_NM, Me.txt_B_BCAT_NM, Me.txt_H_BCAT_NM, Me.txt_START_DT, Me.txt_KEIJO_DT, Me.txt_CKAIYK_DT, Me.txt_SUMIKAISU, Me.txt_SYUTOK_ZOU, Me.txt_SYUTOK_GEN, Me.txt_ZEI_KARI, Me.txt_ZEI_MHRI, Me.txt_LGNPN_TOKI, Me.txt_LRSOK_TOKI, Me.txt_GRUIKEI_ZOU, Me.txt_GSON_RKEI_ZOU, Me.txt_GSON_TK_TOKI, Me.txt_ZZAN, Me.txt_BOKA_ZAN, Me.txt_LGNPN_ZZAN, Me.txt_LGNPN_ZAN, Me.txt_IDO_DT, Me.txt_SYUTOK_ZOU_SUM, Me.txt_SYUTOK_GEN_SUM, Me.txt_ZEI_KARI_SUM, Me.txt_ZEI_MHRI_SUM, Me.txt_LGNPN_TOKI_SUM, Me.txt_LRSOK_TOKI_SUM, Me.txt_GRUIKEI_ZOU_SUM, Me.txt_GSON_RKEI_ZOU_SUM, Me.txt_GSON_TK_TOKI_SUM, Me.txt_ZZAN_SUM, Me.txt_BOKA_ZAN_SUM, Me.txt_LGNPN_ZZAN_SUM, Me.txt_LGNPN_ZAN_SUM, Me.txt_ID})
        Me.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMain.Location = New System.Drawing.Point(0, 40)
        Me.dgvMain.MultiSelect = False
        Me.dgvMain.Name = "dgvMain"
        Me.dgvMain.ReadOnly = True
        Me.dgvMain.RowTemplate.Height = 21
        Me.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMain.Size = New System.Drawing.Size(1200, 521)
        Me.dgvMain.TabIndex = 10
        '
        ' txt_JOKEN
        '
        Me.txt_JOKEN.DataPropertyName = "JOKEN"
        Me.txt_JOKEN.HeaderText = "JOKEN"
        Me.txt_JOKEN.Name = "txt_JOKEN"
        Me.txt_JOKEN.ReadOnly = True
        Me.txt_JOKEN.Width = 200
        '
        ' txt_KYKM_NO
        '
        Me.txt_KYKM_NO.DataPropertyName = "KYKM_NO"
        Me.txt_KYKM_NO.HeaderText = "物件No"
        Me.txt_KYKM_NO.Name = "txt_KYKM_NO"
        Me.txt_KYKM_NO.ReadOnly = True
        Me.txt_KYKM_NO.Width = 60
        '
        ' txt_SAIKAISU
        '
        Me.txt_SAIKAISU.DataPropertyName = "SAIKAISU"
        Me.txt_SAIKAISU.HeaderText = "再回"
        Me.txt_SAIKAISU.Name = "txt_SAIKAISU"
        Me.txt_SAIKAISU.ReadOnly = True
        Me.txt_SAIKAISU.Width = 60
        '
        ' txt_LINE_ID
        '
        Me.txt_LINE_ID.DataPropertyName = "LINE_ID"
        Me.txt_LINE_ID.HeaderText = "配No"
        Me.txt_LINE_ID.Name = "txt_LINE_ID"
        Me.txt_LINE_ID.ReadOnly = True
        Me.txt_LINE_ID.Width = 60
        '
        ' txt_KKBN_NM
        '
        Me.txt_KKBN_NM.DataPropertyName = "KKBN_NM"
        Me.txt_KKBN_NM.HeaderText = "KKBN_NM"
        Me.txt_KKBN_NM.Name = "txt_KKBN_NM"
        Me.txt_KKBN_NM.ReadOnly = True
        Me.txt_KKBN_NM.Width = 60
        '
        ' txt_REC_KBN_STR
        '
        Me.txt_REC_KBN_STR.DataPropertyName = "REC_KBN_STR"
        Me.txt_REC_KBN_STR.HeaderText = "行区"
        Me.txt_REC_KBN_STR.Name = "txt_REC_KBN_STR"
        Me.txt_REC_KBN_STR.ReadOnly = True
        Me.txt_REC_KBN_STR.Width = 60
        '
        ' txt_KJKBN_NM
        '
        Me.txt_KJKBN_NM.DataPropertyName = "KJKBN_NM"
        Me.txt_KJKBN_NM.HeaderText = "計上区分"
        Me.txt_KJKBN_NM.Name = "txt_KJKBN_NM"
        Me.txt_KJKBN_NM.ReadOnly = True
        Me.txt_KJKBN_NM.Width = 60
        '
        ' txt_HREI_KBN
        '
        Me.txt_HREI_KBN.DataPropertyName = "HREI_KBN"
        Me.txt_HREI_KBN.HeaderText = "法令区分"
        Me.txt_HREI_KBN.Name = "txt_HREI_KBN"
        Me.txt_HREI_KBN.ReadOnly = True
        Me.txt_HREI_KBN.Width = 60
        '
        ' txt_TRHK_KBN_STR
        '
        Me.txt_TRHK_KBN_STR.DataPropertyName = "TRHK_KBN_STR"
        Me.txt_TRHK_KBN_STR.HeaderText = "TRHK_KBN_STR"
        Me.txt_TRHK_KBN_STR.Name = "txt_TRHK_KBN_STR"
        Me.txt_TRHK_KBN_STR.ReadOnly = True
        Me.txt_TRHK_KBN_STR.Width = 60
        '
        ' txt_KYKBNL
        '
        Me.txt_KYKBNL.DataPropertyName = "KYKBNL"
        Me.txt_KYKBNL.HeaderText = "契約番号"
        Me.txt_KYKBNL.Name = "txt_KYKBNL"
        Me.txt_KYKBNL.ReadOnly = True
        Me.txt_KYKBNL.Width = 75
        '
        ' txt_LEAKBN
        '
        Me.txt_LEAKBN.DataPropertyName = "LEAKBN"
        Me.txt_LEAKBN.HeaderText = "リース区分"
        Me.txt_LEAKBN.Name = "txt_LEAKBN"
        Me.txt_LEAKBN.ReadOnly = True
        Me.txt_LEAKBN.Width = 90
        '
        ' txt_BUKN_NM
        '
        Me.txt_BUKN_NM.DataPropertyName = "BUKN_NM"
        Me.txt_BUKN_NM.HeaderText = "物件名"
        Me.txt_BUKN_NM.Name = "txt_BUKN_NM"
        Me.txt_BUKN_NM.ReadOnly = True
        Me.txt_BUKN_NM.Width = 109
        '
        ' txt_LCPT1_NM
        '
        Me.txt_LCPT1_NM.DataPropertyName = "LCPT1_NM"
        Me.txt_LCPT1_NM.HeaderText = "支払先"
        Me.txt_LCPT1_NM.Name = "txt_LCPT1_NM"
        Me.txt_LCPT1_NM.ReadOnly = True
        Me.txt_LCPT1_NM.Width = 109
        '
        ' txt_B_BCAT_NM
        '
        Me.txt_B_BCAT_NM.DataPropertyName = "B_BCAT_NM"
        Me.txt_B_BCAT_NM.HeaderText = "B_BCAT_NM"
        Me.txt_B_BCAT_NM.Name = "txt_B_BCAT_NM"
        Me.txt_B_BCAT_NM.ReadOnly = True
        Me.txt_B_BCAT_NM.Width = 132
        '
        ' txt_H_BCAT_NM
        '
        Me.txt_H_BCAT_NM.DataPropertyName = "H_BCAT_NM"
        Me.txt_H_BCAT_NM.HeaderText = "H_BCAT_NM"
        Me.txt_H_BCAT_NM.Name = "txt_H_BCAT_NM"
        Me.txt_H_BCAT_NM.ReadOnly = True
        Me.txt_H_BCAT_NM.Width = 132
        '
        ' txt_START_DT
        '
        Me.txt_START_DT.DataPropertyName = "START_DT"
        Me.txt_START_DT.HeaderText = "開始日"
        Me.txt_START_DT.Name = "txt_START_DT"
        Me.txt_START_DT.ReadOnly = True
        Me.txt_START_DT.Width = 75
        '
        ' txt_KEIJO_DT
        '
        Me.txt_KEIJO_DT.DataPropertyName = "KEIJO_DT"
        Me.txt_KEIJO_DT.HeaderText = "計上月"
        Me.txt_KEIJO_DT.Name = "txt_KEIJO_DT"
        Me.txt_KEIJO_DT.ReadOnly = True
        Me.txt_KEIJO_DT.Width = 75
        '
        ' txt_CKAIYK_DT
        '
        Me.txt_CKAIYK_DT.DataPropertyName = "CKAIYK_DT"
        Me.txt_CKAIYK_DT.HeaderText = "中途解約日"
        Me.txt_CKAIYK_DT.Name = "txt_CKAIYK_DT"
        Me.txt_CKAIYK_DT.ReadOnly = True
        Me.txt_CKAIYK_DT.Width = 75
        '
        ' txt_SUMIKAISU
        '
        Me.txt_SUMIKAISU.DataPropertyName = "SUMIKAISU"
        Me.txt_SUMIKAISU.HeaderText = "回数済/総"
        Me.txt_SUMIKAISU.Name = "txt_SUMIKAISU"
        Me.txt_SUMIKAISU.ReadOnly = True
        Me.txt_SUMIKAISU.Width = 60
        '
        ' txt_SYUTOK_ZOU
        '
        Me.txt_SYUTOK_ZOU.DataPropertyName = "SYUTOK_ZOU"
        Me.txt_SYUTOK_ZOU.HeaderText = "増加･取得価額"
        Me.txt_SYUTOK_ZOU.Name = "txt_SYUTOK_ZOU"
        Me.txt_SYUTOK_ZOU.ReadOnly = True
        Me.txt_SYUTOK_ZOU.Width = 94
        '
        ' txt_SYUTOK_GEN
        '
        Me.txt_SYUTOK_GEN.DataPropertyName = "SYUTOK_GEN"
        Me.txt_SYUTOK_GEN.HeaderText = "SYUTOK_GEN"
        Me.txt_SYUTOK_GEN.Name = "txt_SYUTOK_GEN"
        Me.txt_SYUTOK_GEN.ReadOnly = True
        Me.txt_SYUTOK_GEN.Width = 94
        '
        ' txt_ZEI_KARI
        '
        Me.txt_ZEI_KARI.DataPropertyName = "ZEI_KARI"
        Me.txt_ZEI_KARI.HeaderText = "仮払消費税"
        Me.txt_ZEI_KARI.Name = "txt_ZEI_KARI"
        Me.txt_ZEI_KARI.ReadOnly = True
        Me.txt_ZEI_KARI.Width = 94
        '
        ' txt_ZEI_MHRI
        '
        Me.txt_ZEI_MHRI.DataPropertyName = "ZEI_MHRI"
        Me.txt_ZEI_MHRI.HeaderText = "ZEI_MHRI"
        Me.txt_ZEI_MHRI.Name = "txt_ZEI_MHRI"
        Me.txt_ZEI_MHRI.ReadOnly = True
        Me.txt_ZEI_MHRI.Width = 94
        '
        ' txt_LGNPN_TOKI
        '
        Me.txt_LGNPN_TOKI.DataPropertyName = "LGNPN_TOKI"
        Me.txt_LGNPN_TOKI.HeaderText = "返済元本"
        Me.txt_LGNPN_TOKI.Name = "txt_LGNPN_TOKI"
        Me.txt_LGNPN_TOKI.ReadOnly = True
        Me.txt_LGNPN_TOKI.Width = 94
        '
        ' txt_LRSOK_TOKI
        '
        Me.txt_LRSOK_TOKI.DataPropertyName = "LRSOK_TOKI"
        Me.txt_LRSOK_TOKI.HeaderText = "支払利息"
        Me.txt_LRSOK_TOKI.Name = "txt_LRSOK_TOKI"
        Me.txt_LRSOK_TOKI.ReadOnly = True
        Me.txt_LRSOK_TOKI.Width = 94
        '
        ' txt_GRUIKEI_ZOU
        '
        Me.txt_GRUIKEI_ZOU.DataPropertyName = "GRUIKEI_ZOU"
        Me.txt_GRUIKEI_ZOU.HeaderText = "減価償却額"
        Me.txt_GRUIKEI_ZOU.Name = "txt_GRUIKEI_ZOU"
        Me.txt_GRUIKEI_ZOU.ReadOnly = True
        Me.txt_GRUIKEI_ZOU.Width = 94
        '
        ' txt_GSON_RKEI_ZOU
        '
        Me.txt_GSON_RKEI_ZOU.DataPropertyName = "GSON_RKEI_ZOU"
        Me.txt_GSON_RKEI_ZOU.HeaderText = "減損損失"
        Me.txt_GSON_RKEI_ZOU.Name = "txt_GSON_RKEI_ZOU"
        Me.txt_GSON_RKEI_ZOU.ReadOnly = True
        Me.txt_GSON_RKEI_ZOU.Width = 94
        '
        ' txt_GSON_TK_TOKI
        '
        Me.txt_GSON_TK_TOKI.DataPropertyName = "GSON_TK_TOKI"
        Me.txt_GSON_TK_TOKI.HeaderText = "GSON_TK_TOKI"
        Me.txt_GSON_TK_TOKI.Name = "txt_GSON_TK_TOKI"
        Me.txt_GSON_TK_TOKI.ReadOnly = True
        Me.txt_GSON_TK_TOKI.Width = 94
        '
        ' txt_ZZAN
        '
        Me.txt_ZZAN.DataPropertyName = "ZZAN"
        Me.txt_ZZAN.HeaderText = "前月末残高"
        Me.txt_ZZAN.Name = "txt_ZZAN"
        Me.txt_ZZAN.ReadOnly = True
        Me.txt_ZZAN.Width = 94
        '
        ' txt_BOKA_ZAN
        '
        Me.txt_BOKA_ZAN.DataPropertyName = "BOKA_ZAN"
        Me.txt_BOKA_ZAN.HeaderText = "当月末残高"
        Me.txt_BOKA_ZAN.Name = "txt_BOKA_ZAN"
        Me.txt_BOKA_ZAN.ReadOnly = True
        Me.txt_BOKA_ZAN.Width = 94
        '
        ' txt_LGNPN_ZZAN
        '
        Me.txt_LGNPN_ZZAN.DataPropertyName = "LGNPN_ZZAN"
        Me.txt_LGNPN_ZZAN.HeaderText = "LGNPN_ZZAN"
        Me.txt_LGNPN_ZZAN.Name = "txt_LGNPN_ZZAN"
        Me.txt_LGNPN_ZZAN.ReadOnly = True
        Me.txt_LGNPN_ZZAN.Width = 94
        '
        ' txt_LGNPN_ZAN
        '
        Me.txt_LGNPN_ZAN.DataPropertyName = "LGNPN_ZAN"
        Me.txt_LGNPN_ZAN.HeaderText = "LGNPN_ZAN"
        Me.txt_LGNPN_ZAN.Name = "txt_LGNPN_ZAN"
        Me.txt_LGNPN_ZAN.ReadOnly = True
        Me.txt_LGNPN_ZAN.Width = 94
        '
        ' txt_IDO_DT
        '
        Me.txt_IDO_DT.DataPropertyName = "IDO_DT"
        Me.txt_IDO_DT.HeaderText = "移動日"
        Me.txt_IDO_DT.Name = "txt_IDO_DT"
        Me.txt_IDO_DT.ReadOnly = True
        Me.txt_IDO_DT.Width = 75
        '
        ' txt_SYUTOK_ZOU_SUM
        '
        Me.txt_SYUTOK_ZOU_SUM.DataPropertyName = "SYUTOK_ZOU_SUM"
        Me.txt_SYUTOK_ZOU_SUM.HeaderText = "SYUTOK_ZOU_SUM"
        Me.txt_SYUTOK_ZOU_SUM.Name = "txt_SYUTOK_ZOU_SUM"
        Me.txt_SYUTOK_ZOU_SUM.ReadOnly = True
        Me.txt_SYUTOK_ZOU_SUM.Width = 94
        '
        ' txt_SYUTOK_GEN_SUM
        '
        Me.txt_SYUTOK_GEN_SUM.DataPropertyName = "SYUTOK_GEN_SUM"
        Me.txt_SYUTOK_GEN_SUM.HeaderText = "SYUTOK_GEN_SUM"
        Me.txt_SYUTOK_GEN_SUM.Name = "txt_SYUTOK_GEN_SUM"
        Me.txt_SYUTOK_GEN_SUM.ReadOnly = True
        Me.txt_SYUTOK_GEN_SUM.Width = 94
        '
        ' txt_ZEI_KARI_SUM
        '
        Me.txt_ZEI_KARI_SUM.DataPropertyName = "ZEI_KARI_SUM"
        Me.txt_ZEI_KARI_SUM.HeaderText = "ZEI_KARI_SUM"
        Me.txt_ZEI_KARI_SUM.Name = "txt_ZEI_KARI_SUM"
        Me.txt_ZEI_KARI_SUM.ReadOnly = True
        Me.txt_ZEI_KARI_SUM.Width = 94
        '
        ' txt_ZEI_MHRI_SUM
        '
        Me.txt_ZEI_MHRI_SUM.DataPropertyName = "ZEI_MHRI_SUM"
        Me.txt_ZEI_MHRI_SUM.HeaderText = "ZEI_MHRI_SUM"
        Me.txt_ZEI_MHRI_SUM.Name = "txt_ZEI_MHRI_SUM"
        Me.txt_ZEI_MHRI_SUM.ReadOnly = True
        Me.txt_ZEI_MHRI_SUM.Width = 94
        '
        ' txt_LGNPN_TOKI_SUM
        '
        Me.txt_LGNPN_TOKI_SUM.DataPropertyName = "LGNPN_TOKI_SUM"
        Me.txt_LGNPN_TOKI_SUM.HeaderText = "LGNPN_TOKI_SUM"
        Me.txt_LGNPN_TOKI_SUM.Name = "txt_LGNPN_TOKI_SUM"
        Me.txt_LGNPN_TOKI_SUM.ReadOnly = True
        Me.txt_LGNPN_TOKI_SUM.Width = 94
        '
        ' txt_LRSOK_TOKI_SUM
        '
        Me.txt_LRSOK_TOKI_SUM.DataPropertyName = "LRSOK_TOKI_SUM"
        Me.txt_LRSOK_TOKI_SUM.HeaderText = "LRSOK_TOKI_SUM"
        Me.txt_LRSOK_TOKI_SUM.Name = "txt_LRSOK_TOKI_SUM"
        Me.txt_LRSOK_TOKI_SUM.ReadOnly = True
        Me.txt_LRSOK_TOKI_SUM.Width = 94
        '
        ' txt_GRUIKEI_ZOU_SUM
        '
        Me.txt_GRUIKEI_ZOU_SUM.DataPropertyName = "GRUIKEI_ZOU_SUM"
        Me.txt_GRUIKEI_ZOU_SUM.HeaderText = "GRUIKEI_ZOU_SUM"
        Me.txt_GRUIKEI_ZOU_SUM.Name = "txt_GRUIKEI_ZOU_SUM"
        Me.txt_GRUIKEI_ZOU_SUM.ReadOnly = True
        Me.txt_GRUIKEI_ZOU_SUM.Width = 94
        '
        ' txt_GSON_RKEI_ZOU_SUM
        '
        Me.txt_GSON_RKEI_ZOU_SUM.DataPropertyName = "GSON_RKEI_ZOU_SUM"
        Me.txt_GSON_RKEI_ZOU_SUM.HeaderText = "GSON_RKEI_ZOU_SUM"
        Me.txt_GSON_RKEI_ZOU_SUM.Name = "txt_GSON_RKEI_ZOU_SUM"
        Me.txt_GSON_RKEI_ZOU_SUM.ReadOnly = True
        Me.txt_GSON_RKEI_ZOU_SUM.Width = 94
        '
        ' txt_GSON_TK_TOKI_SUM
        '
        Me.txt_GSON_TK_TOKI_SUM.DataPropertyName = "GSON_TK_TOKI_SUM"
        Me.txt_GSON_TK_TOKI_SUM.HeaderText = "GSON_TK_TOKI_SUM"
        Me.txt_GSON_TK_TOKI_SUM.Name = "txt_GSON_TK_TOKI_SUM"
        Me.txt_GSON_TK_TOKI_SUM.ReadOnly = True
        Me.txt_GSON_TK_TOKI_SUM.Width = 94
        '
        ' txt_ZZAN_SUM
        '
        Me.txt_ZZAN_SUM.DataPropertyName = "ZZAN_SUM"
        Me.txt_ZZAN_SUM.HeaderText = "ZZAN_SUM"
        Me.txt_ZZAN_SUM.Name = "txt_ZZAN_SUM"
        Me.txt_ZZAN_SUM.ReadOnly = True
        Me.txt_ZZAN_SUM.Width = 94
        '
        ' txt_BOKA_ZAN_SUM
        '
        Me.txt_BOKA_ZAN_SUM.DataPropertyName = "BOKA_ZAN_SUM"
        Me.txt_BOKA_ZAN_SUM.HeaderText = "BOKA_ZAN_SUM"
        Me.txt_BOKA_ZAN_SUM.Name = "txt_BOKA_ZAN_SUM"
        Me.txt_BOKA_ZAN_SUM.ReadOnly = True
        Me.txt_BOKA_ZAN_SUM.Width = 94
        '
        ' txt_LGNPN_ZZAN_SUM
        '
        Me.txt_LGNPN_ZZAN_SUM.DataPropertyName = "LGNPN_ZZAN_SUM"
        Me.txt_LGNPN_ZZAN_SUM.HeaderText = "LGNPN_ZZAN_SUM"
        Me.txt_LGNPN_ZZAN_SUM.Name = "txt_LGNPN_ZZAN_SUM"
        Me.txt_LGNPN_ZZAN_SUM.ReadOnly = True
        Me.txt_LGNPN_ZZAN_SUM.Width = 94
        '
        ' txt_LGNPN_ZAN_SUM
        '
        Me.txt_LGNPN_ZAN_SUM.DataPropertyName = "LGNPN_ZAN_SUM"
        Me.txt_LGNPN_ZAN_SUM.HeaderText = "LGNPN_ZAN_SUM"
        Me.txt_LGNPN_ZAN_SUM.Name = "txt_LGNPN_ZAN_SUM"
        Me.txt_LGNPN_ZAN_SUM.ReadOnly = True
        Me.txt_LGNPN_ZAN_SUM.Width = 94
        '
        ' txt_ID
        '
        Me.txt_ID.DataPropertyName = "ID"
        Me.txt_ID.HeaderText = "ID"
        Me.txt_ID.Name = "txt_ID"
        Me.txt_ID.ReadOnly = True
        Me.txt_ID.Visible = False
        '
        ' Form_f_flx_KEIJO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 561)
        Me.Controls.Add(Me.dgvMain)
        Me.Controls.Add(Me.pnlHeader)
        Me.KeyPreview = True
        Me.Name = "Form_f_flx_KEIJO"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "月次仕訳計上フレックス"
        Me.pnlHeader.ResumeLayout(False)
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents cmd_閉じる As System.Windows.Forms.Button
    Friend WithEvents cmd_設定 As System.Windows.Forms.Button
    Friend WithEvents cmd_照会 As System.Windows.Forms.Button
    Friend WithEvents cmd_仕訳出力 As System.Windows.Forms.Button
    Friend WithEvents cmd_計上仕訳 As System.Windows.Forms.Button
    Friend WithEvents cmd_振替伝票 As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexSearchDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexSortDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexReportDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_Output As System.Windows.Forms.Button
    Friend WithEvents dgvMain As System.Windows.Forms.DataGridView
    Friend WithEvents txt_JOKEN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_KYKM_NO As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SAIKAISU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LINE_ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_KKBN_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_REC_KBN_STR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_KJKBN_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_HREI_KBN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_TRHK_KBN_STR As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_KYKBNL As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LEAKBN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_BUKN_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LCPT1_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_B_BCAT_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_H_BCAT_NM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_START_DT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_KEIJO_DT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_CKAIYK_DT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SUMIKAISU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SYUTOK_ZOU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SYUTOK_GEN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_KARI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_MHRI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_TOKI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LRSOK_TOKI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GRUIKEI_ZOU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GSON_RKEI_ZOU As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GSON_TK_TOKI As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZZAN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_BOKA_ZAN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_ZZAN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_ZAN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_IDO_DT As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SYUTOK_ZOU_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_SYUTOK_GEN_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_KARI_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_MHRI_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_TOKI_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LRSOK_TOKI_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GRUIKEI_ZOU_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GSON_RKEI_ZOU_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_GSON_TK_TOKI_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ZZAN_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_BOKA_ZAN_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_ZZAN_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_LGNPN_ZAN_SUM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txt_ID As System.Windows.Forms.DataGridViewTextBoxColumn

End Class