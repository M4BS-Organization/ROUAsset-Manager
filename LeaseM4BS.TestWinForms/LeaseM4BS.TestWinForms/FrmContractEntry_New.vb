
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class FrmContractEntry_New
    Inherits Form

    Private lblTitle As Label
    Private root As TableLayoutPanel
    Private top2 As TableLayoutPanel
    Private grpHeader As GroupBox, tlpHeader As TableLayoutPanel
    Private grpParties As GroupBox, tlpParties As TableLayoutPanel
    Private grpTerms As GroupBox, tlpTerms As TableLayoutPanel
    Private grpPayment As GroupBox, tlpPayment As TableLayoutPanel
    Private grpOptions As GroupBox, tlpOpt As TableLayoutPanel
    Private btnRegister As Button, btnEdit As Button, btnUpdate As Button, btnDelete As Button, btnClose As Button

    Public Sub New()
        Me.Text = "契約入力"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1400, 900)
        Me.Font = New Font("Meiryo UI", 9.0F)
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        lblTitle = New Label() With {.Text = "契約入力（Sheet4）", .Dock = DockStyle.Top, .Padding = New Padding(12), .Font = New Font(Me.Font, FontStyle.Bold)}

        root = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 4}
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 0))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 65.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 35.0F))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 56.0F))

        top2 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .Padding = New Padding(8)}
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 48.0F))
        top2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 52.0F))

        ' 左：ヘッダー＋当事者
        Dim leftStack As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2}
        leftStack.RowStyles.Add(New RowStyle(SizeType.Percent, 42.0F))
        leftStack.RowStyles.Add(New RowStyle(SizeType.Percent, 58.0F))

        grpHeader = New GroupBox() With {.Text = "契約ヘッダー", .Dock = DockStyle.Fill}
        tlpHeader = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 6, .Padding = New Padding(8)}
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

        grpParties = New GroupBox() With {.Text = "契約当事者", .Dock = DockStyle.Fill}
        tlpParties = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 6, .Padding = New Padding(8)}
        tlpParties.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpParties.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlpParties.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpParties.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Dim txtLessorAddr As New TextBox() With {.Dock = DockStyle.Fill, .Text = "渋谷区宇田川町"}
        Dim txtAgentAddr As New TextBox() With {.Dock = DockStyle.Fill}
        Dim txtLesseeAddr As New TextBox() With {.Dock = DockStyle.Fill, .Text = "渋谷区渋谷2-12-8"}
        Dim txtLessorName As New TextBox() With {.Dock = DockStyle.Fill, .Text = "大成開発"}
        Dim txtAgentName As New TextBox() With {.Dock = DockStyle.Fill, .Text = "フォーシーズ"}
        Dim txtLesseeName As New TextBox() With {.Dock = DockStyle.Fill, .Text = "株式会社イルテックス 代表取締役 照井康太"}
        Dim txtLessorAcct As New TextBox() With {.Dock = DockStyle.Fill, .Text = "北陸銀行 渋谷支店"}
        Dim txtAgentAcct As New TextBox() With {.Dock = DockStyle.Fill, .Text = "三井住友 中央支店"}
        Dim txtLesseeAcct As New TextBox() With {.Dock = DockStyle.Fill, .Text = "城南信金 用賀支店"}
        Dim txtG1 As New TextBox() With {.Dock = DockStyle.Fill, .Text = "世田谷区下馬2-6-13"}
        Dim txtG2 As New TextBox() With {.Dock = DockStyle.Fill, .Text = "照井 康太"}

        tlpParties.Controls.Add(New Label() With {.Text = "貸主住所", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpParties.Controls.Add(txtLessorAddr, 1, 0)
        tlpParties.Controls.Add(New Label() With {.Text = "代行先住所", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpParties.Controls.Add(txtAgentAddr, 3, 0)
        tlpParties.Controls.Add(New Label() With {.Text = "借主住所", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpParties.Controls.Add(txtLesseeAddr, 1, 1)
        tlpParties.Controls.Add(New Label() With {.Text = "貸主名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpParties.Controls.Add(txtLessorName, 1, 2)
        tlpParties.Controls.Add(New Label() With {.Text = "代行先名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 2)
        tlpParties.Controls.Add(txtAgentName, 3, 2)
        tlpParties.Controls.Add(New Label() With {.Text = "借主名", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpParties.Controls.Add(txtLesseeName, 1, 3)
        tlpParties.Controls.Add(New Label() With {.Text = "貸主口座", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpParties.Controls.Add(txtLessorAcct, 1, 4)
        tlpParties.Controls.Add(New Label() With {.Text = "口座（代行）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 4)
        tlpParties.Controls.Add(txtAgentAcct, 3, 4)
        tlpParties.Controls.Add(New Label() With {.Text = "口座（借主）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 5)
        tlpParties.Controls.Add(txtLesseeAcct, 1, 5)
        tlpParties.Controls.Add(New Label() With {.Text = "連帯保証人1", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 5)
        tlpParties.Controls.Add(txtG1, 3, 5)
        ' （連帯保証人2は必要に応じて行を1つ追加）

        grpParties.Controls.Add(tlpParties)
        leftStack.Controls.Add(grpHeader, 0, 0)
        leftStack.Controls.Add(grpParties, 0, 1)

        ' 右：契約条項＋約定支払
        Dim rightStack As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2}
        rightStack.RowStyles.Add(New RowStyle(SizeType.Percent, 60.0F))
        rightStack.RowStyles.Add(New RowStyle(SizeType.Percent, 40.0F))

        grpTerms = New GroupBox() With {.Text = "契約条項", .Dock = DockStyle.Fill}
        tlpTerms = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 6, .Padding = New Padding(8)}
        tlpTerms.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpTerms.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        tlpTerms.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpTerms.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))
        Dim dtpStart As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numPeriod As New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 100, .Value = 24}
        Dim dtpEnd As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numMushou As New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 100}
        Dim numNC As New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 100, .Value = 6}
        Dim dtpNotice As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim dtpFirst As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim cboApply As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cboApply.Items.AddRange(New Object() {"当月分", "翌月分", "8月分"})
        Dim numInterval As New NumericUpDown() With {.Minimum = 1, .Maximum = 60, .Width = 100, .Value = 1}
        Dim numCount As New NumericUpDown() With {.Minimum = 0, .Maximum = 1000, .Width = 100}
        Dim dtpLast As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim dtpLastApply As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}

        tlpTerms.Controls.Add(New Label() With {.Text = "契約開始日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpTerms.Controls.Add(dtpStart, 1, 0)
        tlpTerms.Controls.Add(New Label() With {.Text = "契約期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpTerms.Controls.Add(numPeriod, 3, 0)
        tlpTerms.Controls.Add(New Label() With {.Text = "契約終了日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpTerms.Controls.Add(dtpEnd, 1, 1)
        tlpTerms.Controls.Add(New Label() With {.Text = "無償期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 1)
        tlpTerms.Controls.Add(numMushou, 3, 1)
        tlpTerms.Controls.Add(New Label() With {.Text = "解約不能期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpTerms.Controls.Add(numNC, 1, 2)
        tlpTerms.Controls.Add(New Label() With {.Text = "告知期日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 2)
        tlpTerms.Controls.Add(dtpNotice, 3, 2)
        tlpTerms.Controls.Add(New Label() With {.Text = "初回支払日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpTerms.Controls.Add(dtpFirst, 1, 3)
        tlpTerms.Controls.Add(New Label() With {.Text = "充当期間", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 3)
        tlpTerms.Controls.Add(cboApply, 3, 3)
        tlpTerms.Controls.Add(New Label() With {.Text = "支払間隔（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpTerms.Controls.Add(numInterval, 1, 4)
        tlpTerms.Controls.Add(New Label() With {.Text = "総支払回数", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 4)
        tlpTerms.Controls.Add(numCount, 3, 4)
        tlpTerms.Controls.Add(New Label() With {.Text = "最終支払日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 5)
        tlpTerms.Controls.Add(dtpLast, 1, 5)
        tlpTerms.Controls.Add(New Label() With {.Text = "最終充当日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 5)
        tlpTerms.Controls.Add(dtpLastApply, 3, 5)

        grpTerms.Controls.Add(tlpTerms)

        grpPayment = New GroupBox() With {.Text = "約定支払", .Dock = DockStyle.Fill}
        tlpPayment = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 4, .Padding = New Padding(8)}
        tlpPayment.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpPayment.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
        tlpPayment.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 140.0F))
        tlpPayment.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 65.0F))
        Dim numRentTotal As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180, .ThousandsSeparator = True, .Value = 2933352D}
        Dim numTax As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180, .ThousandsSeparator = True, .Value = 293328D}
        Dim numGrossRent As New NumericUpDown() With {.Maximum = 99999999999D, .Dock = DockStyle.Left, .Width = 180, .ThousandsSeparator = True, .Value = 3226680D}
        Dim cboIndex As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList}
        cboIndex.Items.AddRange(New Object() {"CPI"})
        Dim txtIndexContract As New TextBox() With {.Dock = DockStyle.Left, .Width = 120}

        tlpPayment.Controls.Add(New Label() With {.Text = "賃料合計", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 0)
        tlpPayment.Controls.Add(numRentTotal, 1, 0)
        tlpPayment.Controls.Add(New Label() With {.Text = "消費税", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 0)
        tlpPayment.Controls.Add(numTax, 3, 0)
        tlpPayment.Controls.Add(New Label() With {.Text = "税込賃料総額", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpPayment.Controls.Add(numGrossRent, 1, 1)
        tlpPayment.Controls.Add(New Label() With {.Text = "連動指数", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 1)
        tlpPayment.Controls.Add(cboIndex, 3, 1)
        tlpPayment.Controls.Add(New Label() With {.Text = "契約時指数", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 2)
        tlpPayment.Controls.Add(txtIndexContract, 3, 2)

        grpPayment.Controls.Add(tlpPayment)

        rightStack.Controls.Add(grpTerms, 0, 0)
        rightStack.Controls.Add(grpPayment, 0, 1)

        top2.Controls.Add(leftStack, 0, 0)
        top2.Controls.Add(rightStack, 1, 0)

        ' 下段：オプション（延長／解約）
        grpOptions = New GroupBox() With {.Text = "オプション（延長／解約）", .Dock = DockStyle.Fill}
        tlpOpt = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 4, .RowCount = 4, .Padding = New Padding(8)}
        tlpOpt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpOpt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 35.0F))
        tlpOpt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tlpOpt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 65.0F))
        Dim chkExtend As New CheckBox() With {.Text = "延長規定あり", .Dock = DockStyle.Left, .Checked = True}
        Dim chkCancel As New CheckBox() With {.Text = "解約規定あり", .Dock = DockStyle.Left, .Checked = True}
        Dim txtExtCond1 As New TextBox() With {.Dock = DockStyle.Fill, .Text = "解約不能期間に応ずる賃借料"}
        Dim txtCanCond1 As New TextBox() With {.Dock = DockStyle.Fill, .Text = "解約不能期間に応ずる賃借料"}
        Dim dtpExtStart As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numExtPeriod As New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 100, .Value = 24}
        Dim dtpExtEnd As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numExtNC As New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Width = 100, .Value = 6}
        Dim dtpExtNotice As New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        Dim numExtCount As New NumericUpDown() With {.Minimum = 0, .Maximum = 100, .Width = 100, .Value = 4}

        tlpOpt.Controls.Add(chkExtend, 0, 0) : tlpOpt.SetColumnSpan(chkExtend, 2)
        tlpOpt.Controls.Add(chkCancel, 2, 0) : tlpOpt.SetColumnSpan(chkCancel, 2)
        tlpOpt.Controls.Add(New Label() With {.Text = "延長条件1", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 1)
        tlpOpt.Controls.Add(txtExtCond1, 1, 1)
        tlpOpt.Controls.Add(New Label() With {.Text = "解約条件1", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 1)
        tlpOpt.Controls.Add(txtCanCond1, 3, 1)
        tlpOpt.Controls.Add(New Label() With {.Text = "延長開始日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 2)
        tlpOpt.Controls.Add(dtpExtStart, 1, 2)
        tlpOpt.Controls.Add(New Label() With {.Text = "延長期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 2)
        tlpOpt.Controls.Add(numExtPeriod, 3, 2)
        tlpOpt.Controls.Add(New Label() With {.Text = "延長終了日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 3)
        tlpOpt.Controls.Add(dtpExtEnd, 1, 3)
        tlpOpt.Controls.Add(New Label() With {.Text = "解約不能期間（月）", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 3)
        tlpOpt.Controls.Add(numExtNC, 3, 3)
        ' 告知＆更新見込回数を追加行に（行数増やすときは RowCount と RowStyles を調整してください）
        tlpOpt.RowCount += 1
        tlpOpt.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpOpt.Controls.Add(New Label() With {.Text = "告知期日", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 0, 4)
        tlpOpt.Controls.Add(dtpExtNotice, 1, 4)
        tlpOpt.Controls.Add(New Label() With {.Text = "更新見込回数", .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}, 2, 4)
        tlpOpt.Controls.Add(numExtCount, 3, 4)

        grpOptions.Controls.Add(tlpOpt)

        ' 配置
        top2.Controls.Add(leftStack, 0, 0)
        top2.Controls.Add(rightStack, 1, 0)

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
        root.Controls.Add(grpOptions, 0, 2)
        root.Controls.Add(btnPanel, 0, 3)

        Me.Controls.Add(root)
        Me.Controls.Add(lblTitle)
    End Sub
End Class
