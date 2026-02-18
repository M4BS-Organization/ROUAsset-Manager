
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmInitialLumpSum
    Inherits Form

    Private lblTitle As Label
    Private root As TableLayoutPanel
    Private grpHeader As GroupBox, tlpHeader As TableLayoutPanel
    Private grpSummary As GroupBox, tlpSummary As TableLayoutPanel
    Private dgvInit As DataGridView
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button

    Public Sub New()
        Me.Text = "初回一時金"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1280, 800)
        Me.Font = New Font("Meiryo UI", 9.0F)
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        lblTitle = New Label() With {.Text = "初回一時金（Sheet3）", .Dock = DockStyle.Top, .Padding = New Padding(12), .Font = New Font(Me.Font, FontStyle.Bold)}

        root = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 4}
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 32.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 18.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))

        ' ヘッダー
        grpHeader = New GroupBox() With {.Text = "契約ヘッダー", .Dock = DockStyle.Fill}
        tlpHeader = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 4, .Padding = New Padding(8)}
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 65.0F))
        Dim dtpStart As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numPeriod As New NumericUpDown() With {.Maximum = 1200, .Width = 90, .Value = 24}
        Dim dtpEnd As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numNC As New NumericUpDown() With {.Maximum = 1200, .Width = 90, .Value = 6}

        tlpHeader.Controls.Add(New Label() With {.Text = "契約開始日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpHeader.Controls.Add(dtpStart, 1, 0)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpHeader.Controls.Add(numPeriod, 3, 0)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約終了日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpHeader.Controls.Add(dtpEnd, 1, 1)
        tlpHeader.Controls.Add(New Label() With {.Text = "解約不能期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 1)
        tlpHeader.Controls.Add(numNC, 3, 1)

        grpHeader.Controls.Add(tlpHeader)

        ' 約定支払（概要）
        grpSummary = New GroupBox() With {.Text = "約定支払（概要）", .Dock = DockStyle.Fill}
        tlpSummary = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 6, .RowCount = 2, .Padding = New Padding(8)}
        For i = 0 To 5
            tlpSummary.ColumnStyles.Add(If(i Mod 2 = 0, New ColumnStyle(SizeType.Absolute, 120.0F), New ColumnStyle(SizeType.Percent, 20.0F)))
        Next
        Dim numPay As New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 140, .Value = 122223D}
        Dim numTax As New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 140, .Value = 12222D}
        Dim numGross As New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Dock = DockStyle.Left, .Width = 140, .Value = 134445D}

        tlpSummary.Controls.Add(New Label() With {.Text = "1支払額", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpSummary.Controls.Add(numPay, 1, 0)
        tlpSummary.Controls.Add(New Label() With {.Text = "消費税", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpSummary.Controls.Add(numTax, 3, 0)
        tlpSummary.Controls.Add(New Label() With {.Text = "税込", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 4, 0)
        tlpSummary.Controls.Add(numGross, 5, 0)

        grpSummary.Controls.Add(tlpSummary)

        ' 初回一時金グリッド
        dgvInit = New DataGridView() With {.Dock = DockStyle.Fill, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, .AllowUserToAddRows = True, .AllowUserToDeleteRows = True}
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "科目内訳"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "初回額"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "消費税"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "税込"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "振込口座"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "口座合計"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "初回支払日"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "充当期間"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払間隔"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払回数"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "最終支払日"})
        dgvInit.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "償却額"})

        ' 代表行をExcel例に合わせて幾つか投入
        dgvInit.Rows.Add("手付金", "100000", "0", "100000", "株式会社マイレコ", "100000", "2024/07/20", "1", "", "", "2024/07/20", "0")
        dgvInit.Rows.Add("敷金", "488892", "0", "488892", "大成開発 北陸銀行 渋谷支店", "0", "2024/07/24", "", "", "", "", "")
        dgvInit.Rows.Add("日割家賃", "31541", "3154", "34695", "大成開発 北陸銀行 渋谷支店", "0", "2024/07/24", "7/24～7/31", "1", "", "2024/07/24", "0")
        dgvInit.Rows.Add("礼金", "658032", "0", "658032", "大成開発 北陸銀行 渋谷支店", "0", "2024/07/24", "", "", "", "", "")
        dgvInit.Rows.Add("保証料", "122223", "12222", "134445", "フォーシーズ 三井住友 中央支店", "134445", "2024/07/24", "", "", "", "", "")
        dgvInit.Rows.Add("仲介手数料", "18540", "1854", "20394", "株式会社マイレコ", "0", "", "", "", "", "", "")
        dgvInit.Rows.Add("火災保険", "250000", "25000", "275000", "株式会社イルテックス工業", "275000", "2024/08/31", "", "", "", "", "")
        dgvInit.Rows.Add("手付金相殺", "-100000", "0", "-100000", "株式会社マイレコ", "0", "", "", "", "", "", "")
        ' 必要に応じて追加

        ' ボタン
        Dim btnPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(10)}
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        root.Controls.Add(grpHeader, 0, 0)
        root.Controls.Add(grpSummary, 0, 1)
        root.Controls.Add(dgvInit, 0, 2)
        root.Controls.Add(btnPanel, 0, 3)

        Me.Controls.Add(root)
        Me.Controls.Add(lblTitle)
    End Sub
End Class

