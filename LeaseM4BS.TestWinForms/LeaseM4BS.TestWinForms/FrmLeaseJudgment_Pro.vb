
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmLeaseJudgment_Pro
    Inherits Form

    ' ===== 追加：IFRS16 詳細 =====
    ' GRV
    Private numGRV As NumericUpDown, chkGRVApplicable As CheckBox
    Private numGRVPayment As NumericUpDown  ' ★追加: 支払見込額

    ' ▼ 判定画面の参照ヘッダー（読み取り専用）
    Private txtAssetName As TextBox        ' 対象物件名（参照）
    Private txtContractNoHdr As TextBox    ' 契約番号（参照）
    Private txtContractNameHdr As TextBox  ' 契約名（参照）

    ' ▼ 経理入力のコンテナ（IFRS16 詳細を内包）
    Private grpAcc As GroupBox

    ' ▼ IFRS16 詳細（3カラム）用の親テーブル
    Private adv3 As TableLayoutPanel

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

        ' 1366x768 基準のため、最小サイズを下げる
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.AutoScaleDimensions = New SizeF(96.0F, 96.0F)

        Me.MinimumSize = New Size(1200, 720)  ' ★ 1500x950 → 1200x720
        Me.Size = New Size(1280, 760)         ' 実行時に最大化する運用でもOK
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

        ' ===== 0) タイトル =====
        lblTitle = New Label() With {
        .Text = "リース判定（必要項目＋IFRS16実務項目付きレイアウト）",
        .AutoSize = False, .Height = 36,
        .Dock = DockStyle.Fill, .Padding = New Padding(12, 8, 12, 8),
        .Font = New Font(Me.Font, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleLeft
    }

        ' ===== 1) root を最初に New（Add より前に作るのが絶対条件）=====
        root = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 6, .Padding = New Padding(10)
    }
        root.ColumnStyles.Clear()
        root.RowStyles.Clear()
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 36.0F))   ' 0: タイトル
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))   ' 1: 参照ヘッダー

        root.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0F))    ' 2: 総務入力（上段3カラム）
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 40.0F))    ' 3: 経理入力（IFRS16 詳細）
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0F))    ' 4: 判定結果
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))   ' 5: ボタン
        root.Controls.Add(lblTitle, 0, 0)

        ' ===== 2) 参照ヘッダー（対象物件名／契約番号／契約名：読み取り専用）=====
        Dim hdr As New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 6,
        .BackColor = Color.FromArgb(245, 245, 245) ' 総務系参照帯
    }
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F)) ' ラベル
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))   ' 物件名
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F)) ' ラベル
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20.0F))   ' 契約番号
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F)) ' ラベル
        hdr.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))   ' 契約名

        txtAssetName = New TextBox() With {.ReadOnly = True, .Dock = DockStyle.Fill, .BackColor = Color.White}
        txtContractNoHdr = New TextBox() With {.ReadOnly = True, .Dock = DockStyle.Fill, .BackColor = Color.White}
        txtContractNameHdr = New TextBox() With {.ReadOnly = True, .Dock = DockStyle.Fill, .BackColor = Color.White}

        hdr.Controls.Add(New Label() With {.Text = "対象物件名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        hdr.Controls.Add(txtAssetName, 1, 0)
        hdr.Controls.Add(New Label() With {.Text = "契約番号", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        hdr.Controls.Add(txtContractNoHdr, 3, 0)
        hdr.Controls.Add(New Label() With {.Text = "契約名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 4, 0)
        hdr.Controls.Add(txtContractNameHdr, 5, 0)

        root.Controls.Add(hdr, 0, 1)

        ' ===== 3) 総務エリア（上段3カラム：契約・判定要素・支払）=====
        top3 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 3, .RowCount = 1}
        top3.ColumnStyles.Clear()
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))

        ' --- 左：契約・期間 ---
        grpTerm = New GroupBox() With {.Text = "契約・期間", .Dock = DockStyle.Fill, .Padding = New Padding(10, 14, 10, 8), .BackColor = Color.FromArgb(245, 245, 245)}
        tlpTerm = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 6}
        tlpTerm.SuspendLayout()
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        txtContractNo = New TextBox() With {.Width = 160, .Dock = DockStyle.Left}
        dtpStart = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numPeriod = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 24, .Dock = DockStyle.Left, .Width = 80}
        dtpEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130, .Enabled = False}
        numNonCancelable = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 6, .Dock = DockStyle.Left, .Width = 80}

        AddHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        AddHandler numPeriod.ValueChanged, AddressOf OnTermChanged

        tlpTerm.Controls.Add(MakeLabel("契約番号"), 0, 0) : tlpTerm.Controls.Add(txtContractNo, 1, 0)
        tlpTerm.Controls.Add(MakeLabel("契約開始日"), 0, 1) : tlpTerm.Controls.Add(dtpStart, 1, 1)
        tlpTerm.Controls.Add(MakeLabel("契約期間（月）"), 0, 2) : tlpTerm.Controls.Add(numPeriod, 1, 2)
        tlpTerm.Controls.Add(MakeLabel("契約終了日"), 0, 3) : tlpTerm.Controls.Add(dtpEnd, 1, 3)
        tlpTerm.Controls.Add(MakeLabel("非解約期間（月）"), 0, 4) : tlpTerm.Controls.Add(numNonCancelable, 1, 4)
        tlpTerm.ResumeLayout(False)
        grpTerm.Controls.Add(tlpTerm)

        ' --- 中：判定要素 ---
        grpJudge = New GroupBox() With {.Text = "判定要素", .Dock = DockStyle.Fill, .Padding = New Padding(10, 14, 10, 8), .BackColor = Color.FromArgb(245, 245, 245)}
        tlpJudge = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 7}
        tlpJudge.SuspendLayout()
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        ' コントロール初期化
        chkTransfer = New CheckBox() With {.Text = "所有権移転", .Dock = DockStyle.Left, .AutoSize = True}

        chkBargain = New CheckBox() With {.Text = "割安購入権", .Dock = DockStyle.Left, .AutoSize = True}
        Dim numExercisePrice As New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 100, .Dock = DockStyle.Left, .Enabled = False}
        AddHandler chkBargain.CheckedChanged, Sub(s, ev) numExercisePrice.Enabled = chkBargain.Checked
        Dim pnlBargain As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlBargain.Controls.Add(chkBargain) : pnlBargain.Controls.Add(numExercisePrice)

        chkSpecialized = New CheckBox() With {.Text = "専用性/特定資産", .Dock = DockStyle.Left, .AutoSize = True}

        ' 免除規定（短期・少額）パネル
        chkShortTerm = New CheckBox() With {.Text = "短期(≦12M)", .AutoSize = True}
        chkLowValue = New CheckBox() With {.Text = "少額", .AutoSize = True}
        Dim pnlEx As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlEx.Controls.Add(chkShortTerm) : pnlEx.Controls.Add(chkLowValue)

        ' 参考情報（非解約期間）
        lblNcNote = New Label() With {.Text = "", .Dock = DockStyle.Fill, .ForeColor = SystemColors.GrayText, .AutoSize = False, .TextAlign = ContentAlignment.MiddleLeft}

        numPVpct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 80}
        numLifePct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 80}

        ' イベントハンドラ登録
        AddHandler chkTransfer.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkBargain.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkSpecialized.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkShortTerm.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkLowValue.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler numPVpct.ValueChanged, AddressOf OnJudgeChanged
        AddHandler numLifePct.ValueChanged, AddressOf OnJudgeChanged

        ' ★修正：行配置の並び替え（免除を上部に移動）
        ' 0: 移転
        tlpJudge.Controls.Add(MakeLabel("移転条項"), 0, 0) : tlpJudge.Controls.Add(chkTransfer, 1, 0)

        ' 1: 割安購入
        tlpJudge.Controls.Add(MakeLabel("購入権利(行使額)"), 0, 1) : tlpJudge.Controls.Add(pnlBargain, 1, 1)

        ' 2: 専用性
        tlpJudge.Controls.Add(MakeLabel("専用性"), 0, 2) : tlpJudge.Controls.Add(chkSpecialized, 1, 2)

        ' 3: 免除規定（★ここへ移動）
        tlpJudge.Controls.Add(MakeLabel("免除規定"), 0, 3) : tlpJudge.Controls.Add(pnlEx, 1, 3)

        ' 4: 非解約期間注記
        tlpJudge.Controls.Add(MakeLabel("（参考）"), 0, 4) : tlpJudge.Controls.Add(lblNcNote, 1, 4)

        ' 5: PV比率
        tlpJudge.Controls.Add(MakeLabel("現在価値比率(%)"), 0, 5) : tlpJudge.Controls.Add(numPVpct, 1, 5)

        ' 6: 耐用年数比率
        tlpJudge.Controls.Add(MakeLabel("耐用年数比率(%)"), 0, 6) : tlpJudge.Controls.Add(numLifePct, 1, 6)

        tlpJudge.ResumeLayout(False)
        grpJudge.Controls.Add(tlpJudge)

        ' --- 右：支払（原契・代表） ---
        grpPay = New GroupBox() With {.Text = "支払（原契・代表）", .Dock = DockStyle.Fill, .Padding = New Padding(10, 14, 10, 8), .BackColor = Color.FromArgb(245, 245, 245)}
        tlpPay = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 7}
        tlpPay.SuspendLayout()
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 130.0F))
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        numPay = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 120}
        dtpFirst = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 120}
        numInterval = New NumericUpDown() With {.Minimum = 1, .Maximum = 60, .Dock = DockStyle.Left, .Width = 60, .Value = 1}
        numCount = New NumericUpDown() With {.Minimum = 0, .Maximum = 1000, .Dock = DockStyle.Left, .Width = 60, .Value = 24}
        dtpLast = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 120, .Enabled = False}
        cboIndex = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Left, .Width = 80}
        If cboIndex.Items.Count = 0 Then cboIndex.Items.AddRange(New Object() {"-", "CPI"})

        numIndexAtContract = New NumericUpDown() With {.Minimum = 0, .Maximum = 9999, .DecimalPlaces = 2, .Width = 60}
        numIndexCurrent = New NumericUpDown() With {.Minimum = 0, .Maximum = 9999, .DecimalPlaces = 2, .Width = 60}

        AddHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        AddHandler numInterval.ValueChanged, AddressOf OnPayChanged
        AddHandler numCount.ValueChanged, AddressOf OnPayChanged

        tlpPay.Controls.Add(MakeLabel("1支払額(税抜)"), 0, 0) : tlpPay.Controls.Add(numPay, 1, 0)
        tlpPay.Controls.Add(MakeLabel("初回支払日"), 0, 1) : tlpPay.Controls.Add(dtpFirst, 1, 1)
        tlpPay.Controls.Add(MakeLabel("支払間隔(月)"), 0, 2) : tlpPay.Controls.Add(numInterval, 1, 2)
        tlpPay.Controls.Add(MakeLabel("総支払回数"), 0, 3) : tlpPay.Controls.Add(numCount, 1, 3)
        tlpPay.Controls.Add(MakeLabel("最終支払日"), 0, 4) : tlpPay.Controls.Add(dtpLast, 1, 4)
        tlpPay.Controls.Add(MakeLabel("連動指数"), 0, 5) : tlpPay.Controls.Add(cboIndex, 1, 5)

        ' ★修正：指数パネル内の位置ずれ修正（Margin Top で垂直位置を下げる）
        Dim pnlIdx As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}

        ' 上のマージンを 3px ほど追加してラベルのセンターと合わせる
        numIndexAtContract.Margin = New Padding(3, 4, 3, 3)
        numIndexCurrent.Margin = New Padding(3, 4, 3, 3)

        pnlIdx.Controls.Add(numIndexAtContract)
        pnlIdx.Controls.Add(New Label() With {.Text = "→", .AutoSize = True, .Padding = New Padding(0, 8, 0, 0)}) ' 矢印も少し下げる
        pnlIdx.Controls.Add(numIndexCurrent)

        tlpPay.Controls.Add(MakeLabel("契約時→現在"), 0, 6)
        tlpPay.Controls.Add(pnlIdx, 1, 6)

        tlpPay.ResumeLayout(False)
        grpPay.Controls.Add(tlpPay)

        top3.Controls.Add(grpTerm, 0, 0)
        top3.Controls.Add(grpJudge, 1, 0)
        top3.Controls.Add(grpPay, 2, 0)
        root.Controls.Add(top3, 0, 2)

        ' ===== 4) 経理エリア（IFRS 16 会計判断：3カラム）=====
        grpAcc = New GroupBox() With {
        .Text = "（経理入力）IFRS 16 会計判断",
        .Dock = DockStyle.Fill,
        .Padding = New Padding(10, 14, 10, 8),
        .BackColor = Color.FromArgb(255, 247, 230)
    }

        adv3 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 3, .RowCount = 1}
        adv3.ColumnStyles.Clear()
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))

        ' --- 左列 L ---
        Dim L As TableLayoutPanel = CreateSubTable() : L.SuspendLayout()

        ' 1. 割引率（★修正：2行を1行にまとめて高さを節約）
        Dim hdrRate As Label = CreateHeader("割引率", 22)
        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        L.Controls.Add(hdrRate, 0, 0) : L.SetColumnSpan(hdrRate, 2)

        numImplicitRate = If(numImplicitRate, New NumericUpDown() With {.Minimum = 0, .Maximum = 100, .DecimalPlaces = 2, .Width = 65, .ThousandsSeparator = True, .Dock = DockStyle.Left})
        numIBR = If(numIBR, New NumericUpDown() With {.Minimum = 0, .Maximum = 100, .DecimalPlaces = 2, .Width = 65, .ThousandsSeparator = True, .Dock = DockStyle.Left})

        ' 横並びパネル
        Dim pnlRates As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlRates.Controls.Add(New Label() With {.Text = "暗黙", .AutoSize = True, .Padding = New Padding(0, 4, 0, 0)})
        pnlRates.Controls.Add(numImplicitRate)
        pnlRates.Controls.Add(New Label() With {.Text = "IBR", .AutoSize = True, .Padding = New Padding(6, 4, 0, 0)})
        pnlRates.Controls.Add(numIBR)

        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F))
        L.Controls.Add(MakeLabel("年率(%)"), 0, 1) : L.Controls.Add(pnlRates, 1, 1)

        ' 2. オプション（延長／解約）
        Dim hdrOpt As Label = CreateHeader("オプション", 22)
        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        L.Controls.Add(hdrOpt, 0, 2) : L.SetColumnSpan(hdrOpt, 2)

        ' --- 延長OP ---
        chkHasExtend = If(chkHasExtend, New CheckBox() With {.Text = "延長OP", .AutoSize = True})
        chkRcExtend = If(chkRcExtend, New CheckBox() With {.Text = "確実", .AutoSize = True})
        numExtendMonths = If(numExtendMonths, New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 50})
        Dim numExtendPay As New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 85}

        Dim pnlExt As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .FlowDirection = FlowDirection.TopDown, .WrapContents = False, .Margin = New Padding(0)}

        ' 上段：チェックボックス
        Dim pnlExtChk As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlExtChk.Controls.Add(chkHasExtend)
        pnlExtChk.Controls.Add(chkRcExtend)

        ' 下段：数値（省スペース化）
        Dim pnlExtVal As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlExtVal.Controls.Add(New Label() With {.Text = "月数", .AutoSize = True, .Padding = New Padding(0, 5, 0, 0)})
        pnlExtVal.Controls.Add(numExtendMonths)
        pnlExtVal.Controls.Add(New Label() With {.Text = "月額", .AutoSize = True, .Padding = New Padding(4, 5, 0, 0)})
        pnlExtVal.Controls.Add(numExtendPay)

        pnlExt.Controls.Add(pnlExtChk)
        pnlExt.Controls.Add(pnlExtVal)

        ' 行追加：延長（高さ自動）
        L.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        L.Controls.Add(MakeLabel("延長"), 0, 3)
        L.Controls.Add(pnlExt, 1, 3)

        ' --- 解約OP（★ここが表示されるようになります） ---
        chkHasTerminate = If(chkHasTerminate, New CheckBox() With {.Text = "解約OP", .AutoSize = True})
        chkRcTerminate = If(chkRcTerminate, New CheckBox() With {.Text = "確実(行使しない)", .AutoSize = True})

        Dim pnlTer As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0, 4, 0, 0)}
        pnlTer.Controls.Add(chkHasTerminate)
        pnlTer.Controls.Add(chkRcTerminate)

        ' 行追加：解約
        L.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        L.Controls.Add(MakeLabel("解約"), 0, 4)
        L.Controls.Add(pnlTer, 1, 4)

        L.ResumeLayout(False)

        ' --- 中列 M ---
        Dim M As TableLayoutPanel = CreateSubTable() : M.SuspendLayout()
        chkVariableNonIndex = If(chkVariableNonIndex, New CheckBox() With {.Text = "指数以外の変動料", .AutoSize = True})
        chkPerformanceLinked = If(chkPerformanceLinked, New CheckBox() With {.Text = "業績連動", .AutoSize = True})
        numNonLeaseComp = If(numNonLeaseComp, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 110, .Dock = DockStyle.Left})

        Dim hdrVar As Label = CreateHeader("変動/非リース", 22)
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F)) : M.Controls.Add(hdrVar, 0, 0) : M.SetColumnSpan(hdrVar, 2)

        Dim pnlVar As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        pnlVar.Controls.Add(chkVariableNonIndex) : pnlVar.Controls.Add(chkPerformanceLinked)

        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        M.Controls.Add(MakeLabel("区分"), 0, 1) : M.Controls.Add(pnlVar, 1, 1)
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        M.Controls.Add(MakeLabel("非リース額"), 0, 2) : M.Controls.Add(numNonLeaseComp, 1, 2)


        ' ===== ここから置き換え：残価保証（GRV） 見出し＋行 =====

        ' --- GRV 見出し ---
        Dim hdrGrv As Label = CreateHeader("残価保証（GRV）", 22)
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        M.Controls.Add(hdrGrv, 0, 3)
        M.SetColumnSpan(hdrGrv, 2)

        ' --- 1段目：保証額（適用チェック付き） ---
        chkGRVApplicable = If(chkGRVApplicable, New CheckBox() With {.Text = "適用", .AutoSize = True, .Margin = New Padding(0, 3, 3, 0)})
        numGRV = If(numGRV, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 100, .Dock = DockStyle.Left})

        Dim pnlGrv1 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlGrv1.Controls.Add(chkGRVApplicable)
        pnlGrv1.Controls.Add(numGRV)

        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        M.Controls.Add(MakeLabel("保証額"), 0, 4)
        M.Controls.Add(pnlGrv1, 1, 4)

        ' --- 2段目：支払見込額（★ここを追加） ---
        ' Excelの「残価保証支払見込額」に対応
        numGRVPayment = If(numGRVPayment, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 100, .Dock = DockStyle.Left})

        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        M.Controls.Add(MakeLabel("支払見込"), 0, 5)
        M.Controls.Add(numGRVPayment, 1, 5)

        M.ResumeLayout(False)

        ' ===== ここまで置き換え =====
        ' --- 右列 R ---
        ' 縦長による見切れを防ぐため、4列構成にして「初期調整」と「特例」を横に並べる
        Dim R As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .RowCount = 0, .ColumnCount = 4}
        R.SuspendLayout()

        ' 列定義： [ラベル 100px] [数値 110px] [余白 20px] [特例エリア(残り)]
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        ' コントロール初期化（数値入力）
        numPrepaid = If(numPrepaid, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 105, .Dock = DockStyle.Left})
        numIDC = If(numIDC, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 105, .Dock = DockStyle.Left})
        numIncentive = If(numIncentive, New NumericUpDown() With {.Maximum = 9999999999D, .Minimum = -9999999999D, .ThousandsSeparator = True, .Width = 105, .Dock = DockStyle.Left})
        numARO = If(numARO, New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 105, .Dock = DockStyle.Left})

        ' コントロール初期化（チェックボックス）
        chkSaleLeaseback = If(chkSaleLeaseback, New CheckBox() With {.Text = "S&LB", .AutoSize = True})
        chkSublease = If(chkSublease, New CheckBox() With {.Text = "転リース", .AutoSize = True})

        ' 1行目：見出し（左右に分割配置）
        Dim hdrInit As Label = CreateHeader("初期調整", 22)
        Dim hdrOptR As Label = CreateHeader("特例", 22)

        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        R.Controls.Add(hdrInit, 0, 0) : R.SetColumnSpan(hdrInit, 2) ' 左側見出し
        R.Controls.Add(hdrOptR, 3, 0)                               ' 右側見出し

        ' 2行目：前払 ／ S&LB
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        R.Controls.Add(MakeLabel("前払リース料"), 0, 1)
        R.Controls.Add(numPrepaid, 1, 1)
        R.Controls.Add(chkSaleLeaseback, 3, 1)

        ' 3行目：IDC ／ 転リース
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        R.Controls.Add(MakeLabel("IDC(初期費)"), 0, 2)
        R.Controls.Add(numIDC, 1, 2)
        R.Controls.Add(chkSublease, 3, 2)

        ' 4行目：インセンティブ
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        R.Controls.Add(MakeLabel("インセンティブ"), 0, 3)
        R.Controls.Add(numIncentive, 1, 3)

        ' 5行目：除去債務
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        R.Controls.Add(MakeLabel("除去債務"), 0, 4)
        R.Controls.Add(numARO, 1, 4)

        R.ResumeLayout(False)

        adv3.Controls.Add(L, 0, 0)
        adv3.Controls.Add(M, 1, 0)
        adv3.Controls.Add(R, 2, 0)
        grpAcc.Controls.Add(adv3)
        root.Controls.Add(grpAcc, 0, 3)

        ' ===== 5) 判定結果＆ボタン =====
        grpResult = New GroupBox() With {.Text = "判定結果", .Dock = DockStyle.Fill, .Padding = New Padding(8, 6, 8, 6)}
        lblResult = New Label() With {.Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter, .Font = New Font(Me.Font.FontFamily, 10.5F, FontStyle.Bold), .AutoSize = False}
        grpResult.Controls.Add(lblResult)

        Dim btnPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(0)}
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        AddHandler btnClose.Click, AddressOf OnCloseClicked
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        root.Controls.Add(grpResult, 0, 4)
        root.Controls.Add(btnPanel, 0, 5)

        ' ===== 6) 最後にフォームに root を載せる =====
        Me.Controls.Clear()
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





    Private Function CreateHeader(text As String, Optional height As Integer = 22) As Label
        Return New Label() With {
        .Text = text,
        .AutoSize = False,
        .Height = height,
        .BackColor = Color.FromArgb(245, 245, 245),
        .ForeColor = Color.Black,
        .Font = New Font(Me.Font, FontStyle.Bold),
        .Padding = New Padding(6, 3, 0, 0),
        .Dock = DockStyle.Fill,
        .TextAlign = ContentAlignment.MiddleLeft
    }
    End Function

    Private Function CreateSubTable() As TableLayoutPanel
        Dim t As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 0}
        t.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F)) ' ラベル列
        t.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))  ' 値列
        Return t
    End Function


End Class
