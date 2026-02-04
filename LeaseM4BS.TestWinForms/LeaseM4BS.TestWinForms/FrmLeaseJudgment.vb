
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmLeaseJudgment
    Inherits Form

    Private lblTitle As Label
    Private root As TableLayoutPanel
    Private top2 As TableLayoutPanel
    Private grpHeader As GroupBox, tlpHeader As TableLayoutPanel
    Private grpAsset As GroupBox, tlpAsset As TableLayoutPanel
    Private dgvSchedule As DataGridView
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button

    Public Sub New()
        Me.Text = "リース判定"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1280, 800)
        Me.Font = New Font("Meiryo UI", 9.0F)
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        ' タイトル
        lblTitle = New Label() With {.Text = "リース判定（Sheet1）", .Dock = DockStyle.Top, .Padding = New Padding(12), .Font = New Font(Me.Font, FontStyle.Bold)}

        ' ルート
        root = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 4}
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 0))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 55.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))

        ' 上段 2カラム
        top2 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .Padding = New Padding(8)}
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 48.0F))
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 52.0F))

        ' 契約／管理
        grpHeader = New GroupBox() With {.Text = "契約／管理", .Dock = DockStyle.Fill}
        tlpHeader = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 12, .Padding = New Padding(8)}
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        Dim cboAssetBreakdown As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboAssetBreakdown.Items.AddRange(New Object() {"土地建物普通賃借", "土地建物定期賃借", "車両リース", "その他施設利用"})
        Dim cboMgmt As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboMgmt.Items.AddRange(New Object() {"渋谷本社"})
        Dim cboCost As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboCost.Items.AddRange(New Object() {"配賦"})
        Dim txtNo As New TextBox() With {.Dock = DockStyle.Fill, .Text = "123456789"}
        Dim txtName As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtGroup As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtKinsi As New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .Height = 60, .Text = "税法の確認 20260113 ほか"}
        Dim cboYoto As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}

        tlpHeader.Controls.Add(New Label() With {.Text = "資産内訳", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpHeader.Controls.Add(cboAssetBreakdown, 1, 0)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約管理部署", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpHeader.Controls.Add(cboMgmt, 1, 1)
        tlpHeader.Controls.Add(New Label() With {.Text = "費用負担部署", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpHeader.Controls.Add(cboCost, 1, 2)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約番号", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpHeader.Controls.Add(txtNo, 1, 3)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpHeader.Controls.Add(txtName, 1, 4)
        tlpHeader.Controls.Add(New Label() With {.Text = "グループ", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 5)
        tlpHeader.Controls.Add(txtGroup, 1, 5)
        tlpHeader.Controls.Add(New Label() With {.Text = "構造用途", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 6)
        tlpHeader.Controls.Add(cboYoto, 1, 6)
        tlpHeader.Controls.Add(New Label() With {.Text = "禁止事項その他", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 7)
        tlpHeader.Controls.Add(txtKinsi, 1, 7)

        grpHeader.Controls.Add(tlpHeader)

        ' 物件属性
        grpAsset = New GroupBox() With {.Text = "物件属性", .Dock = DockStyle.Fill}
        tlpAsset = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 12, .Padding = New Padding(8)}
        tlpAsset.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpAsset.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Dim txtBukken As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtKuaku As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtMens As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtAddr As New TextBox() With {.Dock = DockStyle.Fill, .Text = "渋谷区渋谷2-12-8"}
        Dim txtMadori As New TextBox() With {.Dock = DockStyle.Fill}
        Dim dtpShunko As New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Left}
        Dim txtKibo As New TextBox() With {.Dock = DockStyle.Fill}
        Dim numTaiyo As New NumericUpDown() With {.Maximum = 600, .Dock = DockStyle.Left, .Width = 100}
        Dim numChikun As New NumericUpDown() With {.Maximum = 200, .Dock = DockStyle.Left, .Width = 100}

        tlpAsset.Controls.Add(New Label() With {.Text = "物件名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpAsset.Controls.Add(txtBukken, 1, 0)
        tlpAsset.Controls.Add(New Label() With {.Text = "区画", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpAsset.Controls.Add(txtKuaku, 1, 1)
        tlpAsset.Controls.Add(New Label() With {.Text = "面積", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpAsset.Controls.Add(txtMens, 1, 2)
        tlpAsset.Controls.Add(New Label() With {.Text = "住所", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpAsset.Controls.Add(txtAddr, 1, 3)
        tlpAsset.Controls.Add(New Label() With {.Text = "間取り", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpAsset.Controls.Add(txtMadori, 1, 4)
        tlpAsset.Controls.Add(New Label() With {.Text = "竣工", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 5)
        tlpAsset.Controls.Add(dtpShunko, 1, 5)
        tlpAsset.Controls.Add(New Label() With {.Text = "規模", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 6)
        tlpAsset.Controls.Add(txtKibo, 1, 6)
        tlpAsset.Controls.Add(New Label() With {.Text = "耐用年数", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 7)
        tlpAsset.Controls.Add(numTaiyo, 1, 7)
        tlpAsset.Controls.Add(New Label() With {.Text = "築年", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 8)
        tlpAsset.Controls.Add(numChikun, 1, 8)

        grpAsset.Controls.Add(tlpAsset)

        top2.Controls.Add(grpHeader, 0, 0)
        top2.Controls.Add(grpAsset, 1, 0)

        ' 下段：支払スケジュール（原契・延長…）
        dgvSchedule = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .AllowUserToAddRows = True,
            .AllowUserToDeleteRows = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "種別（原契/延長）"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "1支払額"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "初回支払日"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払間隔"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "総支払回数"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "最終支払日"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "賃料合計"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "連動指数"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "契約時指数"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "内維持管理費用"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "残価保証額"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "残価保証（有無）"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払見込額"})
        dgvSchedule.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "賃借料総額"})

        ' シートの例に合わせ既定行（原契1行＋延長数行）を軽く追加
        dgvSchedule.Rows.Add("原契", "122223", "2024/07/27", "1", "24", "2026/06/27", "2933352", "CPI", "1.7", "0", "0", "なし", "0", "2933352")
        dgvSchedule.Rows.Add("延長", "129412", "2026/07/27", "1", "24", "2028/06/27", "3105888", "CPI", "1.8", "0", "0", "なし", "0", "3105888")
        dgvSchedule.Rows.Add("延長", "127989", "2028/07/27", "1", "24", "2030/06/27", "3071736", "CPI", "1.82", "0", "0", "なし", "0", "3071736")
        ' （必要に応じて追加）

        ' 下部ボタン
        Dim btnPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(10)}
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        ' ルートへ
        root.Controls.Add(New Panel(), 0, 0)
        root.Controls.Add(top2, 0, 1)
        root.Controls.Add(dgvSchedule, 0, 2)
        root.Controls.Add(btnPanel, 0, 3)

        Me.Controls.Add(root)
        Me.Controls.Add(lblTitle)
    End Sub
End Class

