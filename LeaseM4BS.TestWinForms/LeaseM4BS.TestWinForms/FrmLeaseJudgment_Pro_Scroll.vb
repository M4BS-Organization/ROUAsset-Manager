
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmLeaseJudgment_Pro_Scroll
    Inherits Form

    Private _isShuttingDown As Boolean = False
    Private _isLoaded As Boolean = False


    ' ===== タイトル / ルート =====
    Private lblTitle As Label
    Private root As TableLayoutPanel
    Private top3 As TableLayoutPanel

    ' ===== 左：契約・期間 =====
    Private grpTerm As GroupBox, tlpTerm As TableLayoutPanel
    Private txtContractNo As TextBox
    Private dtpStart As DateTimePicker
    Private numPeriod As NumericUpDown
    Private dtpEnd As DateTimePicker
    Private numNonCancelable As NumericUpDown

    ' ===== 中：判定要素 =====
    Private grpJudge As GroupBox, tlpJudge As TableLayoutPanel
    Private chkTransfer As CheckBox
    Private chkBargain As CheckBox
    Private chkSpecialized As CheckBox
    Private numPVpct As NumericUpDown
    Private numLifePct As NumericUpDown
    Private chkShortTerm As CheckBox
    Private chkLowValue As CheckBox
    Private lblNcNote As Label

    ' ===== 右：支払（原契・代表） =====
    Private grpPay As GroupBox, tlpPay As TableLayoutPanel
    Private numPay As NumericUpDown
    Private dtpFirst As DateTimePicker
    Private numInterval As NumericUpDown
    Private numCount As NumericUpDown
    Private dtpLast As DateTimePicker
    Private cboIndex As ComboBox
    Private numIndexAtContract As NumericUpDown
    Private numIndexCurrent As NumericUpDown

    ' ===== 追加：IFRS16 詳細 =====
    Private grpAdv As GroupBox, tlpAdv As TableLayoutPanel
    ' 割引率
    Private numImplicitRate As NumericUpDown
    Private numIBR As NumericUpDown
    ' オプション（延長/解約）
    Private chkHasExtend As CheckBox, chkRcExtend As CheckBox
    Private numExtendMonths As NumericUpDown
    Private chkHasTerminate As CheckBox, chkRcTerminate As CheckBox
    ' 変動リース料/非リース成分
    Private chkVariableNonIndex As CheckBox
    Private chkPerformanceLinked As CheckBox
    Private numNonLeaseComp As NumericUpDown
    ' GRV
    Private numGRV As NumericUpDown, chkGRVApplicable As CheckBox
    ' 初期調整
    Private numPrepaid As NumericUpDown
    Private numIDC As NumericUpDown
    Private numIncentive As NumericUpDown
    Private numARO As NumericUpDown
    ' 任意フラグ
    Private chkSaleLeaseback As CheckBox, chkSublease As CheckBox

    ' ===== 判定結果 =====
    Private grpResult As GroupBox
    Private lblResult As Label

    ' ===== コマンド =====
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button




    Public Sub New()
        Me.Text = "リース判定（Pro）"
        Me.StartPosition = FormStartPosition.CenterScreen

        ' ★ 変更: DPI スケーリングを有効化（125%/150%でも見切れにくく）
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.AutoScaleDimensions = New SizeF(96.0F, 96.0F)

        Me.MinimumSize = New Size(1500, 950)
        Me.Size = New Size(1500, 950)
        Me.Font = New Font("Meiryo UI", 9.0F)
        Me.KeyPreview = True

        InitializeComponent()

        AddHandler Me.Load, AddressOf OnFormLoaded
        AddHandler Me.FormClosing, AddressOf Frm_FormClosing
    End Sub


    ' 見出しラベルを作る（VB 正しい宣言）
    Private Function SectionLabel(text As String) As Label
        Return New Label() With {
        .Text = text,
        .AutoSize = False,
        .Height = 24,
        .BackColor = Color.FromArgb(245, 245, 245),
        .ForeColor = Color.Black,
        .Font = New Font(Me.Font, FontStyle.Bold),
        .Padding = New Padding(6, 3, 0, 0),
        .Dock = DockStyle.Fill,
        .TextAlign = ContentAlignment.MiddleLeft
    }
    End Function


    Private Sub InitializeComponent()
        SuspendLayout()

        ' ===== タイトル（Row0） =====
        lblTitle = New Label() With {
            .Text = "リース判定（必要項目＋IFRS16実務項目付きレイアウト）",
            .AutoSize = False, .Height = 40,
            .Dock = DockStyle.Fill, .Padding = New Padding(12, 8, 12, 8),
            .Font = New Font(Me.Font, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleLeft
        }


        ' ===== ルート（Row0..Row4） =====
        root = New TableLayoutPanel() With {
    .Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 5, .Padding = New Padding(10)
}
        root.RowStyles.Clear()
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 40.0F)) ' 0: タイトル
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0F))  ' 1: 上段（3カラム） ★ 55→45
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 35.0F))  ' 2: IFRS16 詳細     ★ 25→35
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 20.0F))  ' 3: 判定結果（据置き）
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F)) ' 4: ボタン


        ' ===== 上段3カラム =====
        top3 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 3, .RowCount = 1}
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))

        ' --- 左：契約・期間 ---
        grpTerm = New GroupBox() With {.Text = "契約・期間", .Dock = DockStyle.Fill, .Padding = New Padding(10, 18, 10, 10)}
        tlpTerm = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 6}
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        txtContractNo = New TextBox() With {.Dock = DockStyle.Fill}
        dtpStart = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numPeriod = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 24, .Dock = DockStyle.Left, .Width = 90}
        dtpEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numNonCancelable = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 6, .Dock = DockStyle.Left, .Width = 90}
        tlpTerm.Controls.Add(MakeLabel("契約番号"), 0, 0) : tlpTerm.Controls.Add(txtContractNo, 1, 0)
        tlpTerm.Controls.Add(MakeLabel("契約開始日"), 0, 1) : tlpTerm.Controls.Add(dtpStart, 1, 1)
        tlpTerm.Controls.Add(MakeLabel("契約期間（月）"), 0, 2) : tlpTerm.Controls.Add(numPeriod, 1, 2)
        tlpTerm.Controls.Add(MakeLabel("契約終了日（自動）"), 0, 3) : tlpTerm.Controls.Add(dtpEnd, 1, 3)
        tlpTerm.Controls.Add(MakeLabel("非解約期間（月）"), 0, 4) : tlpTerm.Controls.Add(numNonCancelable, 1, 4)
        AddHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        AddHandler numPeriod.ValueChanged, AddressOf OnTermChanged
        grpTerm.Controls.Add(tlpTerm)

        ' --- 中：判定要素 ---
        grpJudge = New GroupBox() With {.Text = "判定要素（最小＋免除）", .Dock = DockStyle.Fill, .Padding = New Padding(10, 18, 10, 10)}
        tlpJudge = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 7}
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        chkTransfer = New CheckBox() With {.Text = "所有権移転あり", .Dock = DockStyle.Left}
        chkBargain = New CheckBox() With {.Text = "割安購入選択権あり", .Dock = DockStyle.Left}
        chkSpecialized = New CheckBox() With {.Text = "専用性/特定資産", .Dock = DockStyle.Left}
        numPVpct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 90}
        numLifePct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 90}
        chkShortTerm = New CheckBox() With {.Text = "短期免除（12ヶ月以下）", .Dock = DockStyle.Left}
        chkLowValue = New CheckBox() With {.Text = "低価値免除", .Dock = DockStyle.Left}
        lblNcNote = New Label() With {.Text = "", .Dock = DockStyle.Fill, .ForeColor = SystemColors.GrayText, .AutoSize = False, .TextAlign = ContentAlignment.MiddleLeft}
        AddHandler chkTransfer.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkBargain.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkSpecialized.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler numPVpct.ValueChanged, AddressOf OnJudgeChanged
        AddHandler numLifePct.ValueChanged, AddressOf OnJudgeChanged
        AddHandler chkShortTerm.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkLowValue.CheckedChanged, AddressOf OnJudgeChanged
        tlpJudge.Controls.Add(MakeLabel("移転条項"), 0, 0) : tlpJudge.Controls.Add(chkTransfer, 1, 0)
        tlpJudge.Controls.Add(MakeLabel("割安購入"), 0, 1) : tlpJudge.Controls.Add(chkBargain, 1, 1)
        tlpJudge.Controls.Add(MakeLabel("専用性/特定資産"), 0, 2) : tlpJudge.Controls.Add(chkSpecialized, 1, 2)
        tlpJudge.Controls.Add(MakeLabel("現在価値比率（%）"), 0, 3) : tlpJudge.Controls.Add(numPVpct, 1, 3)
        tlpJudge.Controls.Add(MakeLabel("耐用年数比率（%）"), 0, 4) : tlpJudge.Controls.Add(numLifePct, 1, 4)
        tlpJudge.Controls.Add(New Label() With {.Text = "免除"}, 0, 5)
        Dim pnlEx As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlEx.Controls.Add(chkShortTerm) : pnlEx.Controls.Add(chkLowValue)
        tlpJudge.Controls.Add(pnlEx, 1, 5)
        tlpJudge.Controls.Add(lblNcNote, 1, 6)
        grpJudge.Controls.Add(tlpJudge)

        ' --- 右：支払（原契代表） ---
        grpPay = New GroupBox() With {.Text = "支払（原契・代表）", .Dock = DockStyle.Fill, .Padding = New Padding(10, 18, 10, 10)}
        tlpPay = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 7}
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        numPay = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 140}
        dtpFirst = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numInterval = New NumericUpDown() With {.Minimum = 1, .Maximum = 60, .Dock = DockStyle.Left, .Width = 80, .Value = 1}
        numCount = New NumericUpDown() With {.Minimum = 0, .Maximum = 1000, .Dock = DockStyle.Left, .Width = 80, .Value = 24}
        dtpLast = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        cboIndex = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Left, .Width = 120}
        cboIndex.Items.AddRange(New Object() {"CPI"})
        numIndexAtContract = New NumericUpDown() With {.Minimum = 0, .Maximum = 9999, .DecimalPlaces = 2, .Dock = DockStyle.Left, .Width = 100}
        numIndexCurrent = New NumericUpDown() With {.Minimum = 0, .Maximum = 9999, .DecimalPlaces = 2, .Dock = DockStyle.Left, .Width = 100}
        AddHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        AddHandler numInterval.ValueChanged, AddressOf OnPayChanged
        AddHandler numCount.ValueChanged, AddressOf OnPayChanged
        tlpPay.Controls.Add(MakeLabel("1支払額"), 0, 0) : tlpPay.Controls.Add(numPay, 1, 0)
        tlpPay.Controls.Add(MakeLabel("初回支払日"), 0, 1) : tlpPay.Controls.Add(dtpFirst, 1, 1)
        tlpPay.Controls.Add(MakeLabel("支払間隔（月）"), 0, 2) : tlpPay.Controls.Add(numInterval, 1, 2)
        tlpPay.Controls.Add(MakeLabel("総支払回数"), 0, 3) : tlpPay.Controls.Add(numCount, 1, 3)
        tlpPay.Controls.Add(MakeLabel("最終支払日（自動）"), 0, 4) : tlpPay.Controls.Add(dtpLast, 1, 4)
        tlpPay.Controls.Add(MakeLabel("連動指数"), 0, 5) : tlpPay.Controls.Add(cboIndex, 1, 5)
        tlpPay.Controls.Add(MakeLabel("契約時指数/現行"), 0, 6)
        Dim pnlIdx As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlIdx.Controls.Add(numIndexAtContract) : pnlIdx.Controls.Add(New Label() With {.Text = "→"}) : pnlIdx.Controls.Add(numIndexCurrent)
        tlpPay.Controls.Add(pnlIdx, 1, 6)
        grpPay.Controls.Add(tlpPay)

        top3.Controls.Add(grpTerm, 0, 0)
        top3.Controls.Add(grpJudge, 1, 0)
        top3.Controls.Add(grpPay, 2, 0)



        ' ===== 追加：IFRS16 詳細（Row=2）=====
        grpAdv = New GroupBox() With {
    .Text = "IFRS 16 追加項目（割引率・オプション・初期調整 等）",
    .Dock = DockStyle.Fill,
    .Padding = New Padding(10, 18, 10, 10)
}

        ' ★ 追加: スクロール用の中間パネル（小画面/高DPI時のフェイルセーフ）
        Dim pnlAdvScroll As New Panel() With {
    .Dock = DockStyle.Fill,
    .AutoScroll = True
}

        ' ★ 変更: tlpAdv は内容サイズに合わせて伸縮する
        tlpAdv = New TableLayoutPanel() With {
    .Dock = DockStyle.Top,                      ' 高さは内容に依存
    .ColumnCount = 4,
    .RowCount = 12,
    .AutoSize = True,                           ' 重要
    .AutoSizeMode = AutoSizeMode.GrowAndShrink, ' 重要
    .GrowStyle = TableLayoutPanelGrowStyle.AddRows,
    .Margin = New Padding(0)
}

        tlpAdv.ColumnStyles.Clear()
        tlpAdv.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F)) ' L-Label
        tlpAdv.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))   ' L-Value
        tlpAdv.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F)) ' R-Label
        tlpAdv.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 65.0F))   ' R-Value

        ' --- 値コントロールの生成 ---
        numImplicitRate = New NumericUpDown() With {.Minimum = 0, .Maximum = 100, .DecimalPlaces = 2, .Width = 100, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}
        numIBR = New NumericUpDown() With {.Minimum = 0, .Maximum = 100, .DecimalPlaces = 2, .Width = 100, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}

        chkHasExtend = New CheckBox() With {.Text = "延長オプションあり", .Dock = DockStyle.Left}
        chkRcExtend = New CheckBox() With {.Text = "合理的に確実（延長）", .Dock = DockStyle.Left}
        numExtendMonths = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 90, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}

        chkHasTerminate = New CheckBox() With {.Text = "解約オプションあり", .Dock = DockStyle.Left}
        chkRcTerminate = New CheckBox() With {.Text = "合理的に確実（解約しない）", .Dock = DockStyle.Left}

        chkVariableNonIndex = New CheckBox() With {.Text = "指数以外の変動リース料あり", .Dock = DockStyle.Left}
        chkPerformanceLinked = New CheckBox() With {.Text = "業績連動あり", .Dock = DockStyle.Left}
        numNonLeaseComp = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left} ' ★ 140→120で詰め

        chkGRVApplicable = New CheckBox() With {.Text = "残価保証適用", .Dock = DockStyle.Left}
        numGRV = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}

        numPrepaid = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}
        numIDC = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}
        numIncentive = New NumericUpDown() With {.Maximum = 9999999999D, .Minimum = -9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}
        numARO = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 120, .Dock = DockStyle.Left, .Anchor = AnchorStyles.Left}

        chkSaleLeaseback = New CheckBox() With {.Text = "セール&リースバック", .Dock = DockStyle.Left}
        chkSublease = New CheckBox() With {.Text = "サブリース", .Dock = DockStyle.Left}

        ' ===== 見出し：割引率 =====
        Dim hdrRate As Label = SectionLabel("割引率")
        tlpAdv.Controls.Add(hdrRate, 0, 0)
        tlpAdv.SetColumnSpan(hdrRate, 4)

        tlpAdv.Controls.Add(MakeLabel("暗黙利子率(%)"), 0, 1) : tlpAdv.Controls.Add(numImplicitRate, 1, 1)
        tlpAdv.Controls.Add(MakeLabel("追加借入利子率(IBR%)"), 2, 1) : tlpAdv.Controls.Add(numIBR, 3, 1)

        ' ===== 見出し：オプション（延長／解約 と 合理的に確実）=====
        Dim hdrOpt As Label = SectionLabel("オプション（延長／解約 と 合理的に確実）")
        tlpAdv.Controls.Add(hdrOpt, 0, 2)
        tlpAdv.SetColumnSpan(hdrOpt, 4)

        Dim pnlExt As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlExt.Controls.Add(chkHasExtend) : pnlExt.Controls.Add(chkRcExtend)
        pnlExt.Controls.Add(New Label() With {.Text = " 期間(月) ", .AutoSize = True})
        pnlExt.Controls.Add(numExtendMonths)
        tlpAdv.Controls.Add(MakeLabel("延長オプション"), 0, 3)
        tlpAdv.Controls.Add(pnlExt, 1, 3) : tlpAdv.SetColumnSpan(pnlExt, 3)

        Dim pnlTer As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlTer.Controls.Add(chkHasTerminate) : pnlTer.Controls.Add(chkRcTerminate)
        tlpAdv.Controls.Add(MakeLabel("解約オプション"), 0, 4)
        tlpAdv.Controls.Add(pnlTer, 1, 4) : tlpAdv.SetColumnSpan(pnlTer, 3)

        ' ===== 見出し：変動リース料・非リース成分 =====
        Dim hdrVar As Label = SectionLabel("変動リース料・非リース成分")
        tlpAdv.Controls.Add(hdrVar, 0, 5)
        tlpAdv.SetColumnSpan(hdrVar, 4)

        Dim pnlVar As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlVar.Controls.Add(chkVariableNonIndex) : pnlVar.Controls.Add(chkPerformanceLinked)
        tlpAdv.Controls.Add(MakeLabel("変動料の区分"), 0, 6)
        tlpAdv.Controls.Add(pnlVar, 1, 6) : tlpAdv.SetColumnSpan(pnlVar, 3)
        tlpAdv.Controls.Add(MakeLabel("非リース成分（分離額）"), 0, 7) : tlpAdv.Controls.Add(numNonLeaseComp, 1, 7)

        ' ===== 見出し：残価保証（GRV） =====
        Dim hdrGrv As Label = SectionLabel("残価保証（GRV）")
        tlpAdv.Controls.Add(hdrGrv, 0, 8)
        tlpAdv.SetColumnSpan(hdrGrv, 4)

        Dim pnlGrv As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlGrv.Controls.Add(chkGRVApplicable) : pnlGrv.Controls.Add(numGRV)
        tlpAdv.Controls.Add(MakeLabel("GRV"), 0, 9)
        tlpAdv.Controls.Add(pnlGrv, 1, 9) : tlpAdv.SetColumnSpan(pnlGrv, 3)

        ' ===== 見出し：初期調整（+加算／−減額） =====
        Dim hdrInit As Label = SectionLabel("初期調整（+加算／−減額）")
        tlpAdv.Controls.Add(hdrInit, 0, 10)
        tlpAdv.SetColumnSpan(hdrInit, 4)

        Dim pnlInit As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}
        pnlInit.Controls.Add(New Label() With {.Text = "前払", .AutoSize = True}) : pnlInit.Controls.Add(numPrepaid)
        pnlInit.Controls.Add(New Label() With {.Text = "IDC", .AutoSize = True}) : pnlInit.Controls.Add(numIDC)
        pnlInit.Controls.Add(New Label() With {.Text = "インセン", .AutoSize = True}) : pnlInit.Controls.Add(numIncentive)
        pnlInit.Controls.Add(New Label() With {.Text = "除去費用(ARO)", .AutoSize = True}) : pnlInit.Controls.Add(numARO)
        tlpAdv.Controls.Add(MakeLabel("取得原価調整"), 0, 11)
        tlpAdv.Controls.Add(pnlInit, 1, 11) : tlpAdv.SetColumnSpan(pnlInit, 3)

        ' ★ 追加: tlpAdv をスクロールパネルに載せて GroupBox へ
        pnlAdvScroll.Controls.Add(tlpAdv)
        grpAdv.Controls.Clear()
        grpAdv.Controls.Add(pnlAdvScroll)



        ' ===== 判定結果（Row3）=====
        grpResult = New GroupBox() With {.Text = "判定結果", .Dock = DockStyle.Fill, .Padding = New Padding(10, 10, 10, 10)}
        lblResult = New Label() With {.Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter, .Font = New Font(Me.Font.FontFamily, 12.0F, FontStyle.Bold), .AutoSize = False}
        grpResult.Controls.Add(lblResult)

        ' ===== ボタン（Row4）=====
        Dim btnPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(0)}
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        AddHandler btnClose.Click, AddressOf OnCloseClicked
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        ' ===== root に積む =====
        root.Controls.Add(lblTitle, 0, 0)
        root.Controls.Add(top3, 0, 1)
        root.Controls.Add(grpAdv, 0, 2)
        root.Controls.Add(grpResult, 0, 3)
        root.Controls.Add(btnPanel, 0, 4)

        Me.Controls.Add(root)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Shared Function MakeLabel(text As String) As Label
        Return New Label() With {.Text = text, .AutoSize = False, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}
    End Function

    ' ====== イベント ======



    Private Sub OnTermChanged(sender As Object, e As EventArgs)
        If Not _isLoaded OrElse _isShuttingDown OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        If dtpStart Is Nothing OrElse dtpEnd Is Nothing _
       OrElse dtpStart.IsDisposed OrElse dtpEnd.IsDisposed Then Return

        Dim endD = dtpStart.Value.AddMonths(CInt(numPeriod.Value)).AddDays(-1)
        dtpEnd.Value = endD
        SafeRecalc()
    End Sub

    Private Sub OnPayChanged(sender As Object, e As EventArgs)
        If Not _isLoaded OrElse _isShuttingDown OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        If dtpFirst Is Nothing OrElse dtpLast Is Nothing _
       OrElse dtpFirst.IsDisposed OrElse dtpLast.IsDisposed Then Return

        Dim k = Math.Max(1, CInt(numCount.Value))
        Dim m = Math.Max(1, CInt(numInterval.Value))
        dtpLast.Value = dtpFirst.Value.AddMonths(m * (k - 1))
        SafeRecalc()
    End Sub

    Private Sub OnJudgeChanged(sender As Object, e As EventArgs)
        If Not _isLoaded OrElse _isShuttingDown OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        SafeRecalc()
    End Sub



    Private Sub OnCloseClicked(sender As Object, e As EventArgs)
        _isShuttingDown = True
        Me.Close()
    End Sub

    Private Sub FrmLeaseJudgment_Pro_FormClosing(sender As Object, e As FormClosingEventArgs) _
    Handles Me.FormClosing

        _isShuttingDown = True

        ' --- 購読解除（破棄中の再発火を防止） ---
        RemoveHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        RemoveHandler numPeriod.ValueChanged, AddressOf OnTermChanged

        RemoveHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        RemoveHandler numInterval.ValueChanged, AddressOf OnPayChanged
        RemoveHandler numCount.ValueChanged, AddressOf OnPayChanged

        RemoveHandler chkTransfer.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkBargain.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkSpecialized.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler numPVpct.ValueChanged, AddressOf OnJudgeChanged
        RemoveHandler numLifePct.ValueChanged, AddressOf OnJudgeChanged

        ' Pro 追加分
        RemoveHandler chkShortTerm.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkLowValue.CheckedChanged, AddressOf OnJudgeChanged
    End Sub


    Private Sub OnFormLoaded(sender As Object, e As EventArgs)
        _isLoaded = True
        SafeRecalc()
    End Sub

    Private Sub Frm_FormClosing(sender As Object, e As FormClosingEventArgs)
        _isShuttingDown = True

        ' --- 破棄中の再発火を防ぐため、イベント購読解除 ---
        RemoveHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        RemoveHandler numPeriod.ValueChanged, AddressOf OnTermChanged

        RemoveHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        RemoveHandler numInterval.ValueChanged, AddressOf OnPayChanged
        RemoveHandler numCount.ValueChanged, AddressOf OnPayChanged

        RemoveHandler chkTransfer.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkBargain.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkSpecialized.CheckedChanged, AddressOf OnJudgeChanged

        ' Pro 版のみ：免除チェック等も解除
        On Error Resume Next
        RemoveHandler chkShortTerm.CheckedChanged, AddressOf OnJudgeChanged
        RemoveHandler chkLowValue.CheckedChanged, AddressOf OnJudgeChanged
        On Error GoTo 0
    End Sub

    ' ▼ 例外に強いラッパ。ここ経由で RecalcAll を呼ぶ
    Private Sub SafeRecalc()
        If _isShuttingDown OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        Try
            RecalcAll()
        Catch ex As ObjectDisposedException
            ' 破棄レースは無視
        Catch ex As InvalidOperationException
            ' DPI/破棄レースの揺れは無視
        End Try
    End Sub

    ' ▼ 表示更新用の簡易判定（本番の会計ロジックと置換可能）
    Private Sub RecalcAll()
        If lblResult Is Nothing OrElse lblResult.IsDisposed Then Return

        ' --- 免除（Pro） ---
        Dim exempt As Boolean = False
        If TryGetChecked(chkShortTerm) Then exempt = True
        If TryGetChecked(chkLowValue) Then exempt = True

        Dim result As String
        If exempt Then
            result = "判定：免除（短期／低価値）"
        ElseIf TryGetChecked(chkTransfer) OrElse TryGetChecked(chkBargain) Then
            result = "判定：移転FL"
        ElseIf GetNumeric(numPVpct) >= 90D OrElse GetNumeric(numLifePct) >= 75D Then
            result = "判定：フルペイアウトFL"
        Else
            result = "判定：OL（賃借）"
        End If

        Dim nc As Integer = CInt(GetNumeric(numNonCancelable))
        Dim term As Integer = CInt(GetNumeric(numPeriod))

        If lblNcNote IsNot Nothing AndAlso Not lblNcNote.IsDisposed Then
            lblNcNote.Text = $"非解約期間：{nc} ヶ月"
        End If
        lblResult.Text = result & Environment.NewLine &
                         $"（非解約 {nc} ヶ月 / 契約期間 {term} ヶ月）"
    End Sub

    ' ▼ ヘルパー：安全に状態を読む
    Private Shared Function TryGetChecked(chk As CheckBox) As Boolean
        Return (chk IsNot Nothing AndAlso Not chk.IsDisposed AndAlso chk.Checked)
    End Function

    Private Shared Function GetNumeric(num As NumericUpDown) As Decimal
        If num Is Nothing OrElse num.IsDisposed Then Return 0D
        Return num.Value
    End Function



End Class
