<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class Form_f_flx_TOUGETSU

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
        Me.cmd_Output = New System.Windows.Forms.Button()
        Me.cmd_FlexReportDLG = New System.Windows.Forms.Button()
        Me.cmd_FlexSortDLG = New System.Windows.Forms.Button()
        Me.cmd_FlexSearchDLG = New System.Windows.Forms.Button()
        Me.cmd_振替伝票 = New System.Windows.Forms.Button()
        Me.cmd_伝票印刷 = New System.Windows.Forms.Button()
        Me.cmd_品目 = New System.Windows.Forms.Button()
        Me.cmd_仕訳出力 = New System.Windows.Forms.Button()
        Me.cmd_支払照合 = New System.Windows.Forms.Button()
        Me.cmd_照会 = New System.Windows.Forms.Button()
        Me.cmd_設定 = New System.Windows.Forms.Button()
        Me.cmd_閉じる = New System.Windows.Forms.Button()
        Me.dgvMain = New System.Windows.Forms.DataGridView()
        Me.txt_KYKM_NO = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SAIKAISU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LINE_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KKBN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_REC_KBN_STR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_BUKN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_START_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_CKAIYK_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SUMIKAISU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KYKBNL = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LCPT1_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_B_BCAT_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_H_BCAT_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_HKMK_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_B_KNYUKN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_TOTAL = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZKOMI_TOKI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZAN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZAN1NAI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZRITU = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SHRI_DT = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_SHHO_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_KJKBN_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_RSRVH1_NM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LEAKBN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_HREI_KBN = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_計上日 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_TOTAL_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZEI_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ZKOMI_TOKI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZAN_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_LSRYO_ZAN1NAI_SUM = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txt_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlHeader.SuspendLayout()
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.cmd_Output)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexReportDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexSortDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_FlexSearchDLG)
        Me.pnlHeader.Controls.Add(Me.cmd_振替伝票)
        Me.pnlHeader.Controls.Add(Me.cmd_伝票印刷)
        Me.pnlHeader.Controls.Add(Me.cmd_品目)
        Me.pnlHeader.Controls.Add(Me.cmd_仕訳出力)
        Me.pnlHeader.Controls.Add(Me.cmd_支払照合)
        Me.pnlHeader.Controls.Add(Me.cmd_照会)
        Me.pnlHeader.Controls.Add(Me.cmd_設定)
        Me.pnlHeader.Controls.Add(Me.cmd_閉じる)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(2000, 60)
        Me.pnlHeader.TabIndex = 0
        '
        'cmd_Output
        '
        Me.cmd_Output.Location = New System.Drawing.Point(2052, 6)
        Me.cmd_Output.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_Output.Name = "cmd_Output"
        Me.cmd_Output.Size = New System.Drawing.Size(233, 45)
        Me.cmd_Output.TabIndex = 11
        Me.cmd_Output.Text = "ﾌｧｲﾙ出力(&F)"
        Me.cmd_Output.UseVisualStyleBackColor = True
        '
        'cmd_FlexReportDLG
        '
        Me.cmd_FlexReportDLG.Location = New System.Drawing.Point(1885, 6)
        Me.cmd_FlexReportDLG.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_FlexReportDLG.Name = "cmd_FlexReportDLG"
        Me.cmd_FlexReportDLG.Size = New System.Drawing.Size(153, 45)
        Me.cmd_FlexReportDLG.TabIndex = 10
        Me.cmd_FlexReportDLG.Text = "印刷(&R)"
        Me.cmd_FlexReportDLG.UseVisualStyleBackColor = True
        '
        'cmd_FlexSortDLG
        '
        Me.cmd_FlexSortDLG.Location = New System.Drawing.Point(1678, 6)
        Me.cmd_FlexSortDLG.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_FlexSortDLG.Name = "cmd_FlexSortDLG"
        Me.cmd_FlexSortDLG.Size = New System.Drawing.Size(193, 45)
        Me.cmd_FlexSortDLG.TabIndex = 9
        Me.cmd_FlexSortDLG.Text = "並べ替え(&O)"
        Me.cmd_FlexSortDLG.UseVisualStyleBackColor = True
        '
        'cmd_FlexSearchDLG
        '
        Me.cmd_FlexSearchDLG.Location = New System.Drawing.Point(1512, 6)
        Me.cmd_FlexSearchDLG.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_FlexSearchDLG.Name = "cmd_FlexSearchDLG"
        Me.cmd_FlexSearchDLG.Size = New System.Drawing.Size(153, 45)
        Me.cmd_FlexSearchDLG.TabIndex = 8
        Me.cmd_FlexSearchDLG.Text = "検索(&S)"
        Me.cmd_FlexSearchDLG.UseVisualStyleBackColor = True
        '
        'cmd_振替伝票
        '
        Me.cmd_振替伝票.Location = New System.Drawing.Point(1305, 6)
        Me.cmd_振替伝票.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_振替伝票.Name = "cmd_振替伝票"
        Me.cmd_振替伝票.Size = New System.Drawing.Size(193, 45)
        Me.cmd_振替伝票.TabIndex = 7
        Me.cmd_振替伝票.Text = "振替伝票(&T)"
        Me.cmd_振替伝票.UseVisualStyleBackColor = True
        '
        'cmd_伝票印刷
        '
        Me.cmd_伝票印刷.Location = New System.Drawing.Point(1098, 6)
        Me.cmd_伝票印刷.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_伝票印刷.Name = "cmd_伝票印刷"
        Me.cmd_伝票印刷.Size = New System.Drawing.Size(193, 45)
        Me.cmd_伝票印刷.TabIndex = 6
        Me.cmd_伝票印刷.Text = "伝票印刷(&F)"
        Me.cmd_伝票印刷.UseVisualStyleBackColor = True
        '
        'cmd_品目
        '
        Me.cmd_品目.Location = New System.Drawing.Point(960, 6)
        Me.cmd_品目.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_品目.Name = "cmd_品目"
        Me.cmd_品目.Size = New System.Drawing.Size(125, 45)
        Me.cmd_品目.TabIndex = 5
        Me.cmd_品目.Text = "品目"
        Me.cmd_品目.UseVisualStyleBackColor = True
        '
        'cmd_仕訳出力
        '
        Me.cmd_仕訳出力.Location = New System.Drawing.Point(753, 6)
        Me.cmd_仕訳出力.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_仕訳出力.Name = "cmd_仕訳出力"
        Me.cmd_仕訳出力.Size = New System.Drawing.Size(193, 45)
        Me.cmd_仕訳出力.TabIndex = 4
        Me.cmd_仕訳出力.Text = "支払仕訳(&J)"
        Me.cmd_仕訳出力.UseVisualStyleBackColor = True
        '
        'cmd_支払照合
        '
        Me.cmd_支払照合.Location = New System.Drawing.Point(547, 6)
        Me.cmd_支払照合.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_支払照合.Name = "cmd_支払照合"
        Me.cmd_支払照合.Size = New System.Drawing.Size(193, 45)
        Me.cmd_支払照合.TabIndex = 3
        Me.cmd_支払照合.Text = "支払照合(&P)"
        Me.cmd_支払照合.UseVisualStyleBackColor = True
        '
        'cmd_照会
        '
        Me.cmd_照会.Location = New System.Drawing.Point(380, 6)
        Me.cmd_照会.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_照会.Name = "cmd_照会"
        Me.cmd_照会.Size = New System.Drawing.Size(153, 45)
        Me.cmd_照会.TabIndex = 2
        Me.cmd_照会.Text = "照会(&M)"
        Me.cmd_照会.UseVisualStyleBackColor = True
        '
        'cmd_設定
        '
        Me.cmd_設定.Location = New System.Drawing.Point(193, 6)
        Me.cmd_設定.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_設定.Name = "cmd_設定"
        Me.cmd_設定.Size = New System.Drawing.Size(173, 45)
        Me.cmd_設定.TabIndex = 1
        Me.cmd_設定.Text = "再計算(&D)"
        Me.cmd_設定.UseVisualStyleBackColor = True
        '
        'cmd_閉じる
        '
        Me.cmd_閉じる.Location = New System.Drawing.Point(7, 6)
        Me.cmd_閉じる.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.cmd_閉じる.Name = "cmd_閉じる"
        Me.cmd_閉じる.Size = New System.Drawing.Size(173, 45)
        Me.cmd_閉じる.TabIndex = 0
        Me.cmd_閉じる.Text = "閉じる(&C)"
        Me.cmd_閉じる.UseVisualStyleBackColor = True
        '
        'dgvMain
        '
        Me.dgvMain.AllowUserToAddRows = False
        Me.dgvMain.AllowUserToDeleteRows = False
        Me.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.txt_KYKM_NO, Me.txt_SAIKAISU, Me.txt_LINE_ID, Me.txt_KKBN_NM, Me.txt_REC_KBN_STR, Me.txt_BUKN_NM, Me.txt_START_DT, Me.txt_CKAIYK_DT, Me.txt_SUMIKAISU, Me.txt_KYKBNL, Me.txt_LCPT1_NM, Me.txt_B_BCAT_NM, Me.txt_H_BCAT_NM, Me.txt_HKMK_NM, Me.txt_B_KNYUKN, Me.txt_LSRYO_TOTAL, Me.txt_LSRYO_ZZAN, Me.txt_LSRYO_TOKI, Me.txt_ZEI_TOKI, Me.txt_ZKOMI_TOKI, Me.txt_LSRYO_ZAN, Me.txt_LSRYO_ZAN1NAI, Me.txt_ZRITU, Me.txt_SHRI_DT, Me.txt_SHHO_NM, Me.txt_KJKBN_NM, Me.txt_RSRVH1_NM, Me.txt_LEAKBN, Me.txt_HREI_KBN, Me.txt_計上日, Me.txt_LSRYO_TOTAL_SUM, Me.txt_LSRYO_ZZAN_SUM, Me.txt_LSRYO_TOKI_SUM, Me.txt_ZEI_TOKI_SUM, Me.txt_ZKOMI_TOKI_SUM, Me.txt_LSRYO_ZAN_SUM, Me.txt_LSRYO_ZAN1NAI_SUM, Me.txt_ID})
        Me.dgvMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMain.Location = New System.Drawing.Point(0, 60)
        Me.dgvMain.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.dgvMain.MultiSelect = False
        Me.dgvMain.Name = "dgvMain"
        Me.dgvMain.ReadOnly = True
        Me.dgvMain.RowHeadersWidth = 62
        Me.dgvMain.RowTemplate.Height = 21
        Me.dgvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMain.Size = New System.Drawing.Size(2000, 782)
        Me.dgvMain.TabIndex = 12
        '
        'txt_KYKM_NO
        '
        Me.txt_KYKM_NO.DataPropertyName = "KYKM_NO"
        Me.txt_KYKM_NO.HeaderText = "物件No"
        Me.txt_KYKM_NO.MinimumWidth = 8
        Me.txt_KYKM_NO.Name = "txt_KYKM_NO"
        Me.txt_KYKM_NO.ReadOnly = True
        Me.txt_KYKM_NO.Width = 60
        '
        'txt_SAIKAISU
        '
        Me.txt_SAIKAISU.DataPropertyName = "SAIKAISU"
        Me.txt_SAIKAISU.HeaderText = "再回"
        Me.txt_SAIKAISU.MinimumWidth = 8
        Me.txt_SAIKAISU.Name = "txt_SAIKAISU"
        Me.txt_SAIKAISU.ReadOnly = True
        Me.txt_SAIKAISU.Width = 60
        '
        'txt_LINE_ID
        '
        Me.txt_LINE_ID.DataPropertyName = "LINE_ID"
        Me.txt_LINE_ID.HeaderText = "配No"
        Me.txt_LINE_ID.MinimumWidth = 8
        Me.txt_LINE_ID.Name = "txt_LINE_ID"
        Me.txt_LINE_ID.ReadOnly = True
        Me.txt_LINE_ID.Width = 60
        '
        'txt_KKBN_NM
        '
        Me.txt_KKBN_NM.DataPropertyName = "KKBN_NM"
        Me.txt_KKBN_NM.HeaderText = "契区"
        Me.txt_KKBN_NM.MinimumWidth = 8
        Me.txt_KKBN_NM.Name = "txt_KKBN_NM"
        Me.txt_KKBN_NM.ReadOnly = True
        Me.txt_KKBN_NM.Width = 60
        '
        'txt_REC_KBN_STR
        '
        Me.txt_REC_KBN_STR.DataPropertyName = "REC_KBN_STR"
        Me.txt_REC_KBN_STR.HeaderText = "行区"
        Me.txt_REC_KBN_STR.MinimumWidth = 8
        Me.txt_REC_KBN_STR.Name = "txt_REC_KBN_STR"
        Me.txt_REC_KBN_STR.ReadOnly = True
        Me.txt_REC_KBN_STR.Width = 60
        '
        'txt_BUKN_NM
        '
        Me.txt_BUKN_NM.DataPropertyName = "BUKN_NM"
        Me.txt_BUKN_NM.HeaderText = "物件名"
        Me.txt_BUKN_NM.MinimumWidth = 8
        Me.txt_BUKN_NM.Name = "txt_BUKN_NM"
        Me.txt_BUKN_NM.ReadOnly = True
        Me.txt_BUKN_NM.Width = 75
        '
        'txt_START_DT
        '
        Me.txt_START_DT.DataPropertyName = "START_DT"
        Me.txt_START_DT.HeaderText = "開始日"
        Me.txt_START_DT.MinimumWidth = 8
        Me.txt_START_DT.Name = "txt_START_DT"
        Me.txt_START_DT.ReadOnly = True
        Me.txt_START_DT.Width = 75
        '
        'txt_CKAIYK_DT
        '
        Me.txt_CKAIYK_DT.DataPropertyName = "CKAIYK_DT"
        Me.txt_CKAIYK_DT.HeaderText = "中途解約日"
        Me.txt_CKAIYK_DT.MinimumWidth = 8
        Me.txt_CKAIYK_DT.Name = "txt_CKAIYK_DT"
        Me.txt_CKAIYK_DT.ReadOnly = True
        Me.txt_CKAIYK_DT.Width = 75
        '
        'txt_SUMIKAISU
        '
        Me.txt_SUMIKAISU.DataPropertyName = "SUMIKAISU"
        Me.txt_SUMIKAISU.HeaderText = "回数済/総"
        Me.txt_SUMIKAISU.MinimumWidth = 8
        Me.txt_SUMIKAISU.Name = "txt_SUMIKAISU"
        Me.txt_SUMIKAISU.ReadOnly = True
        Me.txt_SUMIKAISU.Width = 60
        '
        'txt_KYKBNL
        '
        Me.txt_KYKBNL.DataPropertyName = "KYKBNL"
        Me.txt_KYKBNL.HeaderText = "契約番号"
        Me.txt_KYKBNL.MinimumWidth = 8
        Me.txt_KYKBNL.Name = "txt_KYKBNL"
        Me.txt_KYKBNL.ReadOnly = True
        Me.txt_KYKBNL.Width = 75
        '
        'txt_LCPT1_NM
        '
        Me.txt_LCPT1_NM.DataPropertyName = "LCPT1_NM"
        Me.txt_LCPT1_NM.HeaderText = "支払先"
        Me.txt_LCPT1_NM.MinimumWidth = 8
        Me.txt_LCPT1_NM.Name = "txt_LCPT1_NM"
        Me.txt_LCPT1_NM.ReadOnly = True
        Me.txt_LCPT1_NM.Width = 75
        '
        'txt_B_BCAT_NM
        '
        Me.txt_B_BCAT_NM.DataPropertyName = "B_BCAT_NM"
        Me.txt_B_BCAT_NM.HeaderText = "B_BCAT_NM"
        Me.txt_B_BCAT_NM.MinimumWidth = 8
        Me.txt_B_BCAT_NM.Name = "txt_B_BCAT_NM"
        Me.txt_B_BCAT_NM.ReadOnly = True
        Me.txt_B_BCAT_NM.Width = 132
        '
        'txt_H_BCAT_NM
        '
        Me.txt_H_BCAT_NM.DataPropertyName = "H_BCAT_NM"
        Me.txt_H_BCAT_NM.HeaderText = "H_BCAT_NM"
        Me.txt_H_BCAT_NM.MinimumWidth = 8
        Me.txt_H_BCAT_NM.Name = "txt_H_BCAT_NM"
        Me.txt_H_BCAT_NM.ReadOnly = True
        Me.txt_H_BCAT_NM.Width = 132
        '
        'txt_HKMK_NM
        '
        Me.txt_HKMK_NM.DataPropertyName = "HKMK_NM"
        Me.txt_HKMK_NM.HeaderText = "費用区分"
        Me.txt_HKMK_NM.MinimumWidth = 8
        Me.txt_HKMK_NM.Name = "txt_HKMK_NM"
        Me.txt_HKMK_NM.ReadOnly = True
        Me.txt_HKMK_NM.Width = 75
        '
        'txt_B_KNYUKN
        '
        Me.txt_B_KNYUKN.DataPropertyName = "B_KNYUKN"
        Me.txt_B_KNYUKN.HeaderText = "現金購入価額(物件)"
        Me.txt_B_KNYUKN.MinimumWidth = 8
        Me.txt_B_KNYUKN.Name = "txt_B_KNYUKN"
        Me.txt_B_KNYUKN.ReadOnly = True
        Me.txt_B_KNYUKN.Width = 94
        '
        'txt_LSRYO_TOTAL
        '
        Me.txt_LSRYO_TOTAL.DataPropertyName = "LSRYO_TOTAL"
        Me.txt_LSRYO_TOTAL.HeaderText = "総支払額"
        Me.txt_LSRYO_TOTAL.MinimumWidth = 8
        Me.txt_LSRYO_TOTAL.Name = "txt_LSRYO_TOTAL"
        Me.txt_LSRYO_TOTAL.ReadOnly = True
        Me.txt_LSRYO_TOTAL.Width = 94
        '
        'txt_LSRYO_ZZAN
        '
        Me.txt_LSRYO_ZZAN.DataPropertyName = "LSRYO_ZZAN"
        Me.txt_LSRYO_ZZAN.HeaderText = "前月末残高"
        Me.txt_LSRYO_ZZAN.MinimumWidth = 8
        Me.txt_LSRYO_ZZAN.Name = "txt_LSRYO_ZZAN"
        Me.txt_LSRYO_ZZAN.ReadOnly = True
        Me.txt_LSRYO_ZZAN.Width = 94
        '
        'txt_LSRYO_TOKI
        '
        Me.txt_LSRYO_TOKI.DataPropertyName = "LSRYO_TOKI"
        Me.txt_LSRYO_TOKI.HeaderText = "税抜き"
        Me.txt_LSRYO_TOKI.MinimumWidth = 8
        Me.txt_LSRYO_TOKI.Name = "txt_LSRYO_TOKI"
        Me.txt_LSRYO_TOKI.ReadOnly = True
        Me.txt_LSRYO_TOKI.Width = 94
        '
        'txt_ZEI_TOKI
        '
        Me.txt_ZEI_TOKI.DataPropertyName = "ZEI_TOKI"
        Me.txt_ZEI_TOKI.HeaderText = "消費税"
        Me.txt_ZEI_TOKI.MinimumWidth = 8
        Me.txt_ZEI_TOKI.Name = "txt_ZEI_TOKI"
        Me.txt_ZEI_TOKI.ReadOnly = True
        Me.txt_ZEI_TOKI.Width = 94
        '
        'txt_ZKOMI_TOKI
        '
        Me.txt_ZKOMI_TOKI.DataPropertyName = "ZKOMI_TOKI"
        Me.txt_ZKOMI_TOKI.HeaderText = "税込み"
        Me.txt_ZKOMI_TOKI.MinimumWidth = 8
        Me.txt_ZKOMI_TOKI.Name = "txt_ZKOMI_TOKI"
        Me.txt_ZKOMI_TOKI.ReadOnly = True
        Me.txt_ZKOMI_TOKI.Width = 94
        '
        'txt_LSRYO_ZAN
        '
        Me.txt_LSRYO_ZAN.DataPropertyName = "LSRYO_ZAN"
        Me.txt_LSRYO_ZAN.HeaderText = "当月末残高"
        Me.txt_LSRYO_ZAN.MinimumWidth = 8
        Me.txt_LSRYO_ZAN.Name = "txt_LSRYO_ZAN"
        Me.txt_LSRYO_ZAN.ReadOnly = True
        Me.txt_LSRYO_ZAN.Width = 94
        '
        'txt_LSRYO_ZAN1NAI
        '
        Me.txt_LSRYO_ZAN1NAI.DataPropertyName = "LSRYO_ZAN1NAI"
        Me.txt_LSRYO_ZAN1NAI.HeaderText = "内１年内"
        Me.txt_LSRYO_ZAN1NAI.MinimumWidth = 8
        Me.txt_LSRYO_ZAN1NAI.Name = "txt_LSRYO_ZAN1NAI"
        Me.txt_LSRYO_ZAN1NAI.ReadOnly = True
        Me.txt_LSRYO_ZAN1NAI.Width = 94
        '
        'txt_ZRITU
        '
        Me.txt_ZRITU.DataPropertyName = "ZRITU"
        Me.txt_ZRITU.HeaderText = "消費税率"
        Me.txt_ZRITU.MinimumWidth = 8
        Me.txt_ZRITU.Name = "txt_ZRITU"
        Me.txt_ZRITU.ReadOnly = True
        Me.txt_ZRITU.Width = 60
        '
        'txt_SHRI_DT
        '
        Me.txt_SHRI_DT.DataPropertyName = "SHRI_DT"
        Me.txt_SHRI_DT.HeaderText = "支払日"
        Me.txt_SHRI_DT.MinimumWidth = 8
        Me.txt_SHRI_DT.Name = "txt_SHRI_DT"
        Me.txt_SHRI_DT.ReadOnly = True
        Me.txt_SHRI_DT.Width = 75
        '
        'txt_SHHO_NM
        '
        Me.txt_SHHO_NM.DataPropertyName = "SHHO_NM"
        Me.txt_SHHO_NM.HeaderText = "支払方法"
        Me.txt_SHHO_NM.MinimumWidth = 8
        Me.txt_SHHO_NM.Name = "txt_SHHO_NM"
        Me.txt_SHHO_NM.ReadOnly = True
        Me.txt_SHHO_NM.Width = 60
        '
        'txt_KJKBN_NM
        '
        Me.txt_KJKBN_NM.DataPropertyName = "KJKBN_NM"
        Me.txt_KJKBN_NM.HeaderText = "計上区分"
        Me.txt_KJKBN_NM.MinimumWidth = 8
        Me.txt_KJKBN_NM.Name = "txt_KJKBN_NM"
        Me.txt_KJKBN_NM.ReadOnly = True
        Me.txt_KJKBN_NM.Width = 60
        '
        'txt_RSRVH1_NM
        '
        Me.txt_RSRVH1_NM.DataPropertyName = "RSRVH1_NM"
        Me.txt_RSRVH1_NM.HeaderText = "RSRVH1_NM"
        Me.txt_RSRVH1_NM.MinimumWidth = 8
        Me.txt_RSRVH1_NM.Name = "txt_RSRVH1_NM"
        Me.txt_RSRVH1_NM.ReadOnly = True
        Me.txt_RSRVH1_NM.Width = 75
        '
        'txt_LEAKBN
        '
        Me.txt_LEAKBN.DataPropertyName = "LEAKBN"
        Me.txt_LEAKBN.HeaderText = "LEAKBN"
        Me.txt_LEAKBN.MinimumWidth = 8
        Me.txt_LEAKBN.Name = "txt_LEAKBN"
        Me.txt_LEAKBN.ReadOnly = True
        Me.txt_LEAKBN.Width = 90
        '
        'txt_HREI_KBN
        '
        Me.txt_HREI_KBN.DataPropertyName = "HREI_KBN"
        Me.txt_HREI_KBN.HeaderText = "HREI_KBN"
        Me.txt_HREI_KBN.MinimumWidth = 8
        Me.txt_HREI_KBN.Name = "txt_HREI_KBN"
        Me.txt_HREI_KBN.ReadOnly = True
        Me.txt_HREI_KBN.Width = 60
        '
        'txt_計上日
        '
        Me.txt_計上日.DataPropertyName = "計上日"
        Me.txt_計上日.HeaderText = "計上日"
        Me.txt_計上日.MinimumWidth = 8
        Me.txt_計上日.Name = "txt_計上日"
        Me.txt_計上日.ReadOnly = True
        Me.txt_計上日.Width = 75
        '
        'txt_LSRYO_TOTAL_SUM
        '
        Me.txt_LSRYO_TOTAL_SUM.DataPropertyName = "LSRYO_TOTAL_SUM"
        Me.txt_LSRYO_TOTAL_SUM.HeaderText = "LSRYO_TOTAL_SUM"
        Me.txt_LSRYO_TOTAL_SUM.MinimumWidth = 8
        Me.txt_LSRYO_TOTAL_SUM.Name = "txt_LSRYO_TOTAL_SUM"
        Me.txt_LSRYO_TOTAL_SUM.ReadOnly = True
        Me.txt_LSRYO_TOTAL_SUM.Width = 94
        '
        'txt_LSRYO_ZZAN_SUM
        '
        Me.txt_LSRYO_ZZAN_SUM.DataPropertyName = "LSRYO_ZZAN_SUM"
        Me.txt_LSRYO_ZZAN_SUM.HeaderText = "LSRYO_ZZAN_SUM"
        Me.txt_LSRYO_ZZAN_SUM.MinimumWidth = 8
        Me.txt_LSRYO_ZZAN_SUM.Name = "txt_LSRYO_ZZAN_SUM"
        Me.txt_LSRYO_ZZAN_SUM.ReadOnly = True
        Me.txt_LSRYO_ZZAN_SUM.Width = 94
        '
        'txt_LSRYO_TOKI_SUM
        '
        Me.txt_LSRYO_TOKI_SUM.DataPropertyName = "LSRYO_TOKI_SUM"
        Me.txt_LSRYO_TOKI_SUM.HeaderText = "LSRYO_TOKI_SUM"
        Me.txt_LSRYO_TOKI_SUM.MinimumWidth = 8
        Me.txt_LSRYO_TOKI_SUM.Name = "txt_LSRYO_TOKI_SUM"
        Me.txt_LSRYO_TOKI_SUM.ReadOnly = True
        Me.txt_LSRYO_TOKI_SUM.Width = 94
        '
        'txt_ZEI_TOKI_SUM
        '
        Me.txt_ZEI_TOKI_SUM.DataPropertyName = "ZEI_TOKI_SUM"
        Me.txt_ZEI_TOKI_SUM.HeaderText = "ZEI_TOKI_SUM"
        Me.txt_ZEI_TOKI_SUM.MinimumWidth = 8
        Me.txt_ZEI_TOKI_SUM.Name = "txt_ZEI_TOKI_SUM"
        Me.txt_ZEI_TOKI_SUM.ReadOnly = True
        Me.txt_ZEI_TOKI_SUM.Width = 94
        '
        'txt_ZKOMI_TOKI_SUM
        '
        Me.txt_ZKOMI_TOKI_SUM.DataPropertyName = "ZKOMI_TOKI_SUM"
        Me.txt_ZKOMI_TOKI_SUM.HeaderText = "ZKOMI_TOKI_SUM"
        Me.txt_ZKOMI_TOKI_SUM.MinimumWidth = 8
        Me.txt_ZKOMI_TOKI_SUM.Name = "txt_ZKOMI_TOKI_SUM"
        Me.txt_ZKOMI_TOKI_SUM.ReadOnly = True
        Me.txt_ZKOMI_TOKI_SUM.Width = 94
        '
        'txt_LSRYO_ZAN_SUM
        '
        Me.txt_LSRYO_ZAN_SUM.DataPropertyName = "LSRYO_ZAN_SUM"
        Me.txt_LSRYO_ZAN_SUM.HeaderText = "LSRYO_ZAN_SUM"
        Me.txt_LSRYO_ZAN_SUM.MinimumWidth = 8
        Me.txt_LSRYO_ZAN_SUM.Name = "txt_LSRYO_ZAN_SUM"
        Me.txt_LSRYO_ZAN_SUM.ReadOnly = True
        Me.txt_LSRYO_ZAN_SUM.Width = 94
        '
        'txt_LSRYO_ZAN1NAI_SUM
        '
        Me.txt_LSRYO_ZAN1NAI_SUM.DataPropertyName = "LSRYO_ZAN1NAI_SUM"
        Me.txt_LSRYO_ZAN1NAI_SUM.HeaderText = "LSRYO_ZAN1NAI_SUM"
        Me.txt_LSRYO_ZAN1NAI_SUM.MinimumWidth = 8
        Me.txt_LSRYO_ZAN1NAI_SUM.Name = "txt_LSRYO_ZAN1NAI_SUM"
        Me.txt_LSRYO_ZAN1NAI_SUM.ReadOnly = True
        Me.txt_LSRYO_ZAN1NAI_SUM.Width = 94
        '
        'txt_ID
        '
        Me.txt_ID.DataPropertyName = "ID"
        Me.txt_ID.HeaderText = "ID"
        Me.txt_ID.MinimumWidth = 8
        Me.txt_ID.Name = "txt_ID"
        Me.txt_ID.ReadOnly = True
        Me.txt_ID.Visible = False
        Me.txt_ID.Width = 150
        '
        'Form_f_flx_TOUGETSU
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(2000, 842)
        Me.Controls.Add(Me.dgvMain)
        Me.Controls.Add(Me.pnlHeader)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(5, 4, 5, 4)
        Me.Name = "Form_f_flx_TOUGETSU"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "月次支払照合フレックス"
        Me.pnlHeader.ResumeLayout(False)
        CType(Me.dgvMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents cmd_閉じる As System.Windows.Forms.Button
    Friend WithEvents cmd_設定 As System.Windows.Forms.Button
    Friend WithEvents cmd_照会 As System.Windows.Forms.Button
    Friend WithEvents cmd_支払照合 As System.Windows.Forms.Button
    Friend WithEvents cmd_仕訳出力 As System.Windows.Forms.Button
    Friend WithEvents cmd_品目 As System.Windows.Forms.Button
    Friend WithEvents cmd_伝票印刷 As System.Windows.Forms.Button
    Friend WithEvents cmd_振替伝票 As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexSearchDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexSortDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_FlexReportDLG As System.Windows.Forms.Button
    Friend WithEvents cmd_Output As System.Windows.Forms.Button
    Friend WithEvents dgvMain As System.Windows.Forms.DataGridView
    Friend WithEvents txt_KYKM_NO As DataGridViewTextBoxColumn
    Friend WithEvents txt_SAIKAISU As DataGridViewTextBoxColumn
    Friend WithEvents txt_LINE_ID As DataGridViewTextBoxColumn
    Friend WithEvents txt_KKBN_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_REC_KBN_STR As DataGridViewTextBoxColumn
    Friend WithEvents txt_BUKN_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_START_DT As DataGridViewTextBoxColumn
    Friend WithEvents txt_CKAIYK_DT As DataGridViewTextBoxColumn
    Friend WithEvents txt_SUMIKAISU As DataGridViewTextBoxColumn
    Friend WithEvents txt_KYKBNL As DataGridViewTextBoxColumn
    Friend WithEvents txt_LCPT1_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_B_BCAT_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_H_BCAT_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_HKMK_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_B_KNYUKN As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_TOTAL As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZZAN As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_TOKI As DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_TOKI As DataGridViewTextBoxColumn
    Friend WithEvents txt_ZKOMI_TOKI As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZAN As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZAN1NAI As DataGridViewTextBoxColumn
    Friend WithEvents txt_ZRITU As DataGridViewTextBoxColumn
    Friend WithEvents txt_SHRI_DT As DataGridViewTextBoxColumn
    Friend WithEvents txt_SHHO_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_KJKBN_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_RSRVH1_NM As DataGridViewTextBoxColumn
    Friend WithEvents txt_LEAKBN As DataGridViewTextBoxColumn
    Friend WithEvents txt_HREI_KBN As DataGridViewTextBoxColumn
    Friend WithEvents txt_計上日 As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_TOTAL_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZZAN_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_TOKI_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_ZEI_TOKI_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_ZKOMI_TOKI_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZAN_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_LSRYO_ZAN1NAI_SUM As DataGridViewTextBoxColumn
    Friend WithEvents txt_ID As DataGridViewTextBoxColumn
End Class