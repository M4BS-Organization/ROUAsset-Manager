
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmAccountingInput
    Inherits Form

    Private lblTitle As Label
    Private root As TableLayoutPanel
    Private top2 As TableLayoutPanel
    Private grpHeader As GroupBox, tlpHeader As TableLayoutPanel
    Private grpAcct As GroupBox, dgvAcct As DataGridView
    Private grpPV As GroupBox, tlpPV As TableLayoutPanel
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button

    Public Sub New()
        Me.Text = "会計入力"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1280, 800)
        Me.Font = New Font("Meiryo UI", 9.0F)
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        lblTitle = New Label() With {.Text = "会計入力（Sheet2）", .Dock = DockStyle.Top, .Padding = New Padding(12), .Font = New Font(Me.Font, FontStyle.Bold)}

        root = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 4}
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 0))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 55.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))

        top2 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .Padding = New Padding(8)}
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))

        ' ヘッダー
        grpHeader = New GroupBox() With {.Text = "契約ヘッダー", .Dock = DockStyle.Fill}
        tlpHeader = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 7, .Padding = New Padding(8)}
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpHeader.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Dim cboAsset As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboAsset.Items.AddRange(New Object() {"土地建物普通賃借"})
        Dim cboMgmt As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboMgmt.Items.AddRange(New Object() {"渋谷本社"})
        Dim cboCost As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Fill}
        cboCost.Items.AddRange(New Object() {"配賦"})
        Dim txtNo As New TextBox() With {.Dock = DockStyle.Fill, .Text = "123456789"}
        Dim txtName As New TextBox() With {.Dock = DockStyle.Fill}

        tlpHeader.Controls.Add(New Label() With {.Text = "資産内訳", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpHeader.Controls.Add(cboAsset, 1, 0)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約管理部署", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpHeader.Controls.Add(cboMgmt, 1, 1)
        tlpHeader.Controls.Add(New Label() With {.Text = "費用負担部署", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpHeader.Controls.Add(cboCost, 1, 2)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約番号", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpHeader.Controls.Add(txtNo, 1, 3)
        tlpHeader.Controls.Add(New Label() With {.Text = "契約名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpHeader.Controls.Add(txtName, 1, 4)

        grpHeader.Controls.Add(tlpHeader)

        ' 会計リース料（原契＋延長）
        grpAcct = New GroupBox() With {.Text = "会計リース料", .Dock = DockStyle.Fill}
        dgvAcct = New DataGridView() With {.Dock = DockStyle.Fill, .AllowUserToAddRows = True, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill}
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "種別"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "1支払額"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "初回支払日"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払間隔"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "総支払回数"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "最終支払日"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "賃料合計"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "連動指数"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "契約時指数"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "内維持管理費用"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "残価保証額"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "残価保証（有無）"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "支払見込額"})
        dgvAcct.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "賃借料総額"})
        dgvAcct.Rows.Add("原契", "122223", "2024/07/27", "1", "24", "2026/06/27", "2933352", "CPI", "1.7", "0", "0", "なし", "0", "2933352")
        dgvAcct.Rows.Add("延長", "129412", "2026/07/27", "1", "24", "2028/06/27", "3105888", "CPI", "1.8", "0", "0", "なし", "0", "3105888")
        grpAcct.Controls.Add(dgvAcct)

        top2.Controls.Add(grpHeader, 0, 0)
        top2.Controls.Add(grpAcct, 1, 0)

        ' 下段：現在価値等
        grpPV = New GroupBox() With {.Text = "現在価値・使用権資産・リース負債", .Dock = DockStyle.Fill}
        tlpPV = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 3, .Padding = New Padding(8)}
        tlpPV.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpPV.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        tlpPV.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpPV.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))
        Dim numPV As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180}
        Dim numROU As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180}
        Dim numLeaseLiab As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180}
        Dim numRate As New NumericUpDown() With {.Maximum = 100D, .DecimalPlaces = 2, .Dock = DockStyle.Left, .Width = 100}
        numRate.Value = 6D

        tlpPV.Controls.Add(New Label() With {.Text = "現在価値", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpPV.Controls.Add(numPV, 1, 0)
        tlpPV.Controls.Add(New Label() With {.Text = "使用権資産", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpPV.Controls.Add(numROU, 3, 0)
        tlpPV.Controls.Add(New Label() With {.Text = "リース負債", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpPV.Controls.Add(numLeaseLiab, 1, 1)
        tlpPV.Controls.Add(New Label() With {.Text = "追加借入利子率(%)", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 1)
        tlpPV.Controls.Add(numRate, 3, 1)

        grpPV.Controls.Add(tlpPV)

        ' ボタン
        Dim btnPanel = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(10)}
        btnRegister = New Button() With {.Text = "登録(&R)", .Width = 100}
        btnUpdate = New Button() With {.Text = "更新(&U)", .Width = 100}
        btnEdit = New Button() With {.Text = "修正(&E)", .Width = 100}
        btnDelete = New Button() With {.Text = "削除(&D)", .Width = 100}
        btnClose = New Button() With {.Text = "閉じる(&X)", .Width = 100}
        btnPanel.Controls.AddRange(New Control() {btnClose, btnUpdate, btnEdit, btnDelete, btnRegister})

        root.Controls.Add(New Panel(), 0, 0)
        root.Controls.Add(top2, 0, 1)
        root.Controls.Add(grpPV, 0, 2)
        root.Controls.Add(btnPanel, 0, 3)

        Me.Controls.Add(root)
        Me.Controls.Add(lblTitle)
    End Sub
End Class

