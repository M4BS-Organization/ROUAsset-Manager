
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmLeaseJudgment_Lite
    Inherits Form

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
    Private chkTransfer As CheckBox              ' 所有権移転
    Private chkBargain As CheckBox               ' 割安購入
    Private chkSpecialized As CheckBox           ' 専用性/特定資産
    Private numPVpct As NumericUpDown            ' 現在価値比率%
    Private numLifePct As NumericUpDown          ' 耐用年数比率%
    Private lblNcNote As Label                   ' 非解約期間の補足（任意表示）

    ' ===== 右：支払（原契・代表） =====
    Private grpPay As GroupBox, tlpPay As TableLayoutPanel
    Private numPay As NumericUpDown
    Private dtpFirst As DateTimePicker
    Private numInterval As NumericUpDown
    Private numCount As NumericUpDown
    Private dtpLast As DateTimePicker
    Private cboIndex As ComboBox

    ' ===== 判定結果 =====
    Private grpResult As GroupBox
    Private lblResult As Label

    ' ===== コマンド =====
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button


    Public Sub New()
        Me.Text = "リース判定（軽量版）"
        Me.StartPosition = FormStartPosition.CenterScreen

        ' ★ これを追記（または元の行を上書き）
        Me.AutoScaleMode = AutoScaleMode.None

        ' ★ フォームサイズをDPI対応の推奨値へ変更
        Me.MinimumSize = New Size(1450, 900)
        Me.Size = New Size(1450, 900)

        Me.Font = New Font("Meiryo UI", 9.0F)
        Me.KeyPreview = True
        InitializeComponent()
        RecalcAll()
    End Sub



    Private Sub InitializeComponent()
        SuspendLayout()

        ' ===== タイトル（★ Dock=Top は使わない） =====
        lblTitle = New Label() With {
        .Text = "リース判定（必要項目のみ・最適化レイアウト）",
        .AutoSize = False,
        .Height = 40,                          ' タイトル行の高さを固定
        .Dock = DockStyle.Fill,                ' root のセル内で Fill
        .Padding = New Padding(12, 8, 12, 8),
        .Font = New Font(Me.Font, FontStyle.Bold),
        .TextAlign = ContentAlignment.MiddleLeft
    }

        ' ===== ルート（縦）— タイトルを 0 行目に入れる =====
        root = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 1,
        .RowCount = 4,                         ' ここはいったん 4 で初期化（後でクリアして再定義）
        .BackColor = SystemColors.Control,
        .Padding = New Padding(10, 10, 10, 10)
    }

        ' RowStyles を組み替え：0=タイトル / 1=上段 / 2=判定結果 / 3=ボタン
        root.RowStyles.Clear()
        root.RowCount = 4
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 40.0F))  ' ★ タイトル行（固定 40px）
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 75.0F))   ' 上段（契約・判定要素・支払）
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0F))   ' 判定結果（縮小）
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))  ' 下部ボタン列

        ' ===== 上段 3カラム =====
        top3 = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 3,
        .RowCount = 1,
        .Padding = New Padding(0)             ' 余白は各 GroupBox 側で管理
    }
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F)) ' 左：契約・期間
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F)) ' 中：判定要素
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F)) ' 右：支払

        ' === 左：契約・期間 ===
        grpTerm = New GroupBox() With {
        .Text = "契約・期間",
        .Dock = DockStyle.Fill,
        .Padding = New Padding(10, 18, 10, 10)   ' タイトル行の食い込み回避で上だけ広め
    }
        tlpTerm = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .RowCount = 6
    }
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))  ' ラベル列
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))   ' 値列

        txtContractNo = New TextBox() With {.Dock = DockStyle.Fill}
        dtpStart = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numPeriod = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Dock = DockStyle.Left, .Width = 90, .Value = 24}
        dtpEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numNonCancelable = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Dock = DockStyle.Left, .Width = 90, .Value = 6}

        tlpTerm.Controls.Add(MakeLabel("契約番号"), 0, 0) : tlpTerm.Controls.Add(txtContractNo, 1, 0)
        tlpTerm.Controls.Add(MakeLabel("契約開始日"), 0, 1) : tlpTerm.Controls.Add(dtpStart, 1, 1)
        tlpTerm.Controls.Add(MakeLabel("契約期間（月）"), 0, 2) : tlpTerm.Controls.Add(numPeriod, 1, 2)
        tlpTerm.Controls.Add(MakeLabel("契約終了日（自動）"), 0, 3) : tlpTerm.Controls.Add(dtpEnd, 1, 3)
        tlpTerm.Controls.Add(MakeLabel("非解約期間（月）"), 0, 4) : tlpTerm.Controls.Add(numNonCancelable, 1, 4)

        AddHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        AddHandler numPeriod.ValueChanged, AddressOf OnTermChanged

        grpTerm.Controls.Add(tlpTerm)

        ' === 中：判定要素 ===
        grpJudge = New GroupBox() With {
        .Text = "判定要素（最小セット）",
        .Dock = DockStyle.Fill,
        .Padding = New Padding(10, 18, 10, 10)
    }
        tlpJudge = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .RowCount = 6
    }
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        chkTransfer = New CheckBox() With {.Text = "所有権移転あり", .Dock = DockStyle.Left}
        chkBargain = New CheckBox() With {.Text = "割安購入選択権あり", .Dock = DockStyle.Left}
        chkSpecialized = New CheckBox() With {.Text = "専用性/特定資産", .Dock = DockStyle.Left}
        numPVpct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 90}
        numLifePct = New NumericUpDown() With {.Minimum = 0, .Maximum = 200, .DecimalPlaces = 1, .Increment = 0.1D, .Dock = DockStyle.Left, .Width = 90}
        lblNcNote = New Label() With {.Text = "", .Dock = DockStyle.Fill, .ForeColor = SystemColors.GrayText, .AutoSize = False, .TextAlign = ContentAlignment.MiddleLeft}

        AddHandler chkTransfer.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkBargain.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler chkSpecialized.CheckedChanged, AddressOf OnJudgeChanged
        AddHandler numPVpct.ValueChanged, AddressOf OnJudgeChanged
        AddHandler numLifePct.ValueChanged, AddressOf OnJudgeChanged

        tlpJudge.Controls.Add(MakeLabel("移転条項"), 0, 0) : tlpJudge.Controls.Add(chkTransfer, 1, 0)
        tlpJudge.Controls.Add(MakeLabel("割安購入"), 0, 1) : tlpJudge.Controls.Add(chkBargain, 1, 1)
        tlpJudge.Controls.Add(MakeLabel("専用性/特定資産"), 0, 2) : tlpJudge.Controls.Add(chkSpecialized, 1, 2)
        tlpJudge.Controls.Add(MakeLabel("現在価値比率（%）"), 0, 3) : tlpJudge.Controls.Add(numPVpct, 1, 3)
        tlpJudge.Controls.Add(MakeLabel("耐用年数比率（%）"), 0, 4) : tlpJudge.Controls.Add(numLifePct, 1, 4)
        tlpJudge.Controls.Add(New Label() With {.Text = "", .AutoSize = False}, 0, 5) : tlpJudge.Controls.Add(lblNcNote, 1, 5)

        grpJudge.Controls.Add(tlpJudge)

        ' === 右：支払（原契・代表） ===
        grpPay = New GroupBox() With {
        .Text = "支払（原契・代表）",
        .Dock = DockStyle.Fill,
        .Padding = New Padding(10, 18, 10, 10)
    }
        tlpPay = New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .RowCount = 6
    }
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        numPay = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 140}
        dtpFirst = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        numInterval = New NumericUpDown() With {.Minimum = 1, .Maximum = 60, .Dock = DockStyle.Left, .Width = 80, .Value = 1}
        numCount = New NumericUpDown() With {.Minimum = 0, .Maximum = 1000, .Dock = DockStyle.Left, .Width = 80, .Value = 24}
        dtpLast = New DateTimePicker() With {.Format = DateTimePickerFormat.[Short], .Dock = DockStyle.Left, .Width = 130}
        cboIndex = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Left, .Width = 120}
        cboIndex.Items.AddRange(New Object() {"CPI"})

        AddHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        AddHandler numInterval.ValueChanged, AddressOf OnPayChanged
        AddHandler numCount.ValueChanged, AddressOf OnPayChanged

        tlpPay.Controls.Add(MakeLabel("1支払額"), 0, 0) : tlpPay.Controls.Add(numPay, 1, 0)
        tlpPay.Controls.Add(MakeLabel("初回支払日"), 0, 1) : tlpPay.Controls.Add(dtpFirst, 1, 1)
        tlpPay.Controls.Add(MakeLabel("支払間隔（月）"), 0, 2) : tlpPay.Controls.Add(numInterval, 1, 2)
        tlpPay.Controls.Add(MakeLabel("総支払回数"), 0, 3) : tlpPay.Controls.Add(numCount, 1, 3)
        tlpPay.Controls.Add(MakeLabel("最終支払日（自動）"), 0, 4) : tlpPay.Controls.Add(dtpLast, 1, 4)
        tlpPay.Controls.Add(MakeLabel("連動指数"), 0, 5) : tlpPay.Controls.Add(cboIndex, 1, 5)

        grpPay.Controls.Add(tlpPay)

        ' 3カラム配置（Row=1）
        top3.Controls.Add(grpTerm, 0, 0)
        top3.Controls.Add(grpJudge, 1, 0)
        top3.Controls.Add(grpPay, 2, 0)

        ' ===== 判定結果（Row=2） =====
        grpResult = New GroupBox() With {
        .Text = "判定結果",
        .Dock = DockStyle.Fill,
        .Padding = New Padding(10, 10, 10, 10),
        .MinimumSize = New Size(0, 150)
    }
        lblResult = New Label() With {
        .Dock = DockStyle.Fill,
        .TextAlign = ContentAlignment.MiddleCenter,
        .Font = New Font(Me.Font.FontFamily, 12.0F, FontStyle.Bold),
        .AutoSize = False
    }
        grpResult.Controls.Add(lblResult)

        ' ===== ボタン（Row=3） =====
        Dim btnPanel = New FlowLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .FlowDirection = FlowDirection.RightToLeft,
        .Padding = New Padding(0)
    }
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        AddHandler btnClose.Click, AddressOf OnCloseClicked
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        ' ===== root に積む（重要：タイトルは Row=0） =====
        root.Controls.Add(lblTitle, 0, 0)  ' ★ タイトル
        root.Controls.Add(top3, 0, 1)  ' 上段3カラム
        root.Controls.Add(grpResult, 0, 2)
        root.Controls.Add(btnPanel, 0, 3)

        ' フォームへ反映（タイトルは root の中に入れたので、ここで lblTitle を Add しない）
        Me.Controls.Add(root)

        ResumeLayout(False)
        PerformLayout()
    End Sub


    ' ラベルを統一仕様で生成
    Private Shared Function MakeLabel(text As String) As Label
        Return New Label() With {
            .Text = text,
            .AutoSize = False,
            .TextAlign = ContentAlignment.MiddleRight,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(3)
        }
    End Function

    ' ====== イベント ======
    Private Sub OnCloseClicked(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub OnTermChanged(sender As Object, e As EventArgs)
        ' 開始日 + 契約期間(月) → 終了日（簡便法：同日-1日）
        Dim months As Integer = CInt(numPeriod.Value)
        Dim startD = dtpStart.Value
        Dim endD = startD.AddMonths(months).AddDays(-1)
        dtpEnd.Value = endD
        RecalcAll()
    End Sub

    Private Sub OnPayChanged(sender As Object, e As EventArgs)
        ' 初回 + (間隔 * (回数-1)) → 最終支払日（単純化）
        Dim first = dtpFirst.Value
        Dim k = Math.Max(1, CInt(numCount.Value))
        Dim m = Math.Max(1, CInt(numInterval.Value))
        dtpLast.Value = first.AddMonths(m * (k - 1))
        RecalcAll()
    End Sub

    Private Sub OnJudgeChanged(sender As Object, e As EventArgs)
        RecalcAll()
    End Sub

    ' ====== 判定ロジック（簡易） ======
    Private Sub RecalcAll()
        ' 1) 移転/割安購入のいずれか True → 移転FL
        ' 2) 上記以外で PV>=90% または Life>=75% → フルペイアウトFL
        ' 3) それ以外 → OL
        Dim result As String
        If chkTransfer.Checked Or chkBargain.Checked Then
            result = "判定：移転FL"
        ElseIf numPVpct.Value >= 90D Or numLifePct.Value >= 75D Then
            result = "判定：フルペイアウトFL"
        Else
            result = "判定：OL（賃借）"
        End If

        Dim nc As Integer = CInt(numNonCancelable.Value)
        Dim term As Integer = CInt(numPeriod.Value)
        lblNcNote.Text = $"非解約期間：{nc} ヶ月"

        lblResult.Text = result & Environment.NewLine & $"（非解約 {nc} ヶ月 / 契約期間 {term} ヶ月）"
    End Sub
End Class
