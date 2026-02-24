Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class FrmAssetDetailEntry

    Public Property AssetId As Integer = 0
    Public Property IsReadOnly As Boolean = False

    ' 月次明細タブ用コントロール
    Private dgvMonthlyPayments As DataGridView
    Private lblMonthlyTotalExTax As Label
    Private lblMonthlyTotalTax As Label
    Private lblMonthlyTotalIncTax As Label
    Private numFairValue As NumericUpDown
    Private numEconomicLife As NumericUpDown
    Private numImplicitRate As NumericUpDown
    Private numIBR As NumericUpDown
    Private lblAppliedRate As Label

    Public ReadOnly Property PropertyName As String
        Get
            Return txtPropertyName.Text
        End Get
    End Property

    Public ReadOnly Property AccountClass As String
        Get
            If cmbAccountClass.SelectedItem IsNot Nothing Then
                Return cmbAccountClass.SelectedItem.ToString()
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property AssetNo As String
        Get
            Return txtAssetNo.Text
        End Get
    End Property

    Public ReadOnly Property Quantity As Integer
        Get
            Return CInt(numQuantity.Value)
        End Get
    End Property

    Public ReadOnly Property CashPrice As String
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property MonthlyLease As String
        Get
            Return ""
        End Get
    End Property

    Private Sub FrmAssetDetailEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 計上区分コンボボックスの初期化
        cmbAccountClass.Items.AddRange(New String() {"オンバランス", "オフバランス"})
        If cmbAccountClass.Items.Count > 0 Then
            cmbAccountClass.SelectedIndex = 0
        End If

        ' 資産番号が設定済みなら表示
        If AssetId > 0 Then
            txtAssetNo.Text = AssetId.ToString()
        End If

        If AssetId > 0 Then
            LoadAssetData()
        End If
        If IsReadOnly Then
            ApplyReadOnlyMode()
        End If

        Dim selfEquipRows As Integer = dgvSelfEquipment.Rows.Count
        If dgvSelfEquipment.AllowUserToAddRows Then selfEquipRows -= 1
        If selfEquipRows < 3 Then
            For i As Integer = 0 To 2 - selfEquipRows
                dgvSelfEquipment.Rows.Add()
            Next
        End If

        Dim allocRows As Integer = dgvAllocations.Rows.Count
        If dgvAllocations.AllowUserToAddRows Then allocRows -= 1
        If allocRows < 7 Then
            For i As Integer = 0 To 6 - allocRows
                dgvAllocations.Rows.Add()
            Next
        End If

        CalcBuildingAge()
        InitTabMonthlyDetail()
    End Sub

    Private Sub InitTabMonthlyDetail()
        Dim CLR_CARD As Color = Color.White
        Dim CLR_BORDER As Color = Color.FromArgb(222, 226, 230)
        Dim CLR_TEXT As Color = Color.FromArgb(33, 37, 41)
        Dim CLR_LABEL As Color = Color.FromArgb(73, 80, 87)
        Dim CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
        Dim CLR_READONLY As Color = Color.FromArgb(233, 236, 239)
        Dim FNT_INPUT As New Font("Meiryo", 9.75F)
        Dim FNT_LABEL As New Font("Meiryo", 9.0F, FontStyle.Bold)

        Dim scroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(6)}

        Dim mainTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 1, .RowCount = 2
        }
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        mainTbl.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        ' === 月額支払明細 ===
        Dim grpMonthly As New GroupBox() With {
            .Text = "月額支払明細", .Dock = DockStyle.Top,
            .BackColor = Color.White, .Height = 260, .AutoSize = False,
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER,
            .Padding = New Padding(6, 12, 6, 6)
        }
        Dim pnlGrid As New Panel() With {.Dock = DockStyle.Fill}

        dgvMonthlyPayments = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = CLR_CARD,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True,
            .RowHeadersVisible = False,
            .BorderStyle = BorderStyle.None,
            .GridColor = CLR_BORDER,
            .DefaultCellStyle = New DataGridViewCellStyle() With {.Font = FNT_INPUT, .ForeColor = CLR_TEXT},
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle() With {
                .BackColor = Color.FromArgb(240, 244, 248),
                .Font = FNT_LABEL,
                .ForeColor = CLR_LABEL,
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            },
            .EnableHeadersVisualStyles = False
        }

        Dim colMItem As New DataGridViewComboBoxColumn() With {
            .HeaderText = "科目", .Name = "MItem", .FillWeight = 14
        }
        colMItem.Items.AddRange("賃料", "管理費", "共益費")
        dgvMonthlyPayments.Columns.Add(colMItem)
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "支払額（税抜）", .Name = "MAmountExTax", .FillWeight = 14,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0"
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "消費税", .Name = "MTax", .FillWeight = 12, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "税込合計", .Name = "MTotalIncTax", .FillWeight = 14, .ReadOnly = True,
            .DefaultCellStyle = New DataGridViewCellStyle() With {
                .Alignment = DataGridViewContentAlignment.MiddleRight, .Format = "N0",
                .BackColor = CLR_READONLY
            }
        })
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "振込先口座", .Name = "MBankAccount", .FillWeight = 18
        })
        Dim colPayMethod As New DataGridViewComboBoxColumn() With {
            .HeaderText = "支払方法", .Name = "MPayMethod", .FillWeight = 12
        }
        colPayMethod.Items.AddRange("振込", "口座振替", "手形", "現金")
        dgvMonthlyPayments.Columns.Add(colPayMethod)
        dgvMonthlyPayments.Columns.Add(New DataGridViewTextBoxColumn() With {
            .HeaderText = "支払日", .Name = "MPayDate", .FillWeight = 10
        })

        AddHandler dgvMonthlyPayments.CellValueChanged, AddressOf OnMonthlyPaymentChanged
        AddHandler dgvMonthlyPayments.CellEndEdit, AddressOf OnMonthlyPaymentCellEndEdit

        Dim pnlTotal As New Panel() With {
            .Dock = DockStyle.Bottom, .Height = 30,
            .BackColor = Color.FromArgb(230, 240, 250)
        }
        Dim flowTotal As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.LeftToRight,
            .WrapContents = False,
            .Padding = New Padding(4, 4, 0, 0)
        }
        flowTotal.Controls.Add(New Label() With {
            .Text = "月額合計:", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 10, 0)
        })
        lblMonthlyTotalExTax = New Label() With {
            .Text = "税抜: 0", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 20, 0)
        }
        lblMonthlyTotalTax = New Label() With {
            .Text = "税: 0", .Font = FNT_LABEL, .AutoSize = True,
            .Padding = New Padding(0, 2, 20, 0)
        }
        lblMonthlyTotalIncTax = New Label() With {
            .Text = "税込: 0",
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold),
            .AutoSize = True, .ForeColor = CLR_HEADER,
            .Padding = New Padding(0, 1, 0, 0)
        }
        flowTotal.Controls.Add(lblMonthlyTotalExTax)
        flowTotal.Controls.Add(lblMonthlyTotalTax)
        flowTotal.Controls.Add(lblMonthlyTotalIncTax)
        pnlTotal.Controls.Add(flowTotal)

        pnlGrid.Controls.Add(dgvMonthlyPayments)
        pnlGrid.Controls.Add(pnlTotal)
        grpMonthly.Controls.Add(pnlGrid)

        ' === 財務パラメータ ===
        Dim grpFinancial As New GroupBox() With {
            .Text = "財務パラメータ", .Dock = DockStyle.Top, .AutoSize = True,
            .BackColor = Color.White,
            .Font = New Font("Meiryo", 10.0F, FontStyle.Bold),
            .ForeColor = CLR_HEADER,
            .Padding = New Padding(6, 12, 6, 6)
        }
        Dim tblFin As New TableLayoutPanel() With {
            .Dock = DockStyle.Top, .AutoSize = True,
            .ColumnCount = 4, .Padding = New Padding(8)
        }
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 160.0F))
        tblFin.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        numFairValue = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 99999999999D,
            .ThousandsSeparator = True, .TextAlign = HorizontalAlignment.Right,
            .Value = 0
        }
        numEconomicLife = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .Maximum = 100,
            .TextAlign = HorizontalAlignment.Right, .Value = 47
        }
        numImplicitRate = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .DecimalPlaces = 2, .Maximum = 100,
            .Increment = 0.01D, .TextAlign = HorizontalAlignment.Right
        }
        numIBR = New NumericUpDown() With {
            .Dock = DockStyle.Fill, .DecimalPlaces = 2, .Maximum = 100,
            .Increment = 0.01D, .TextAlign = HorizontalAlignment.Right,
            .Value = 2.5D
        }

        lblAppliedRate = New Label() With {
            .Dock = DockStyle.Fill, .Text = "適用割引率: IBR 2.50%",
            .BackColor = Color.FromArgb(255, 248, 220),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Meiryo", 9.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(133, 100, 4)
        }

        AddHandler numImplicitRate.ValueChanged, Sub(s, e) UpdateAppliedRate()
        AddHandler numIBR.ValueChanged, Sub(s, e) UpdateAppliedRate()

        ' Row 0
        Dim lblFairValue As New Label() With {
            .Text = "原資産見積公正価値", .Dock = DockStyle.Fill,
            .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
            .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 4, 0)
        }
        Dim lblEconomicLife As New Label() With {
            .Text = "経済的耐用年数(年)", .Dock = DockStyle.Fill,
            .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
            .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 4, 0)
        }
        tblFin.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblFin.Controls.Add(lblFairValue, 0, 0)
        tblFin.Controls.Add(numFairValue, 1, 0)
        tblFin.Controls.Add(lblEconomicLife, 2, 0)
        tblFin.Controls.Add(numEconomicLife, 3, 0)
        tblFin.RowCount = 1

        ' Row 1
        Dim lblImplicitRate As New Label() With {
            .Text = "リース計算利子率(%)", .Dock = DockStyle.Fill,
            .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
            .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 4, 0)
        }
        Dim lblIBR As New Label() With {
            .Text = "追加借入利子率IBR(%)", .Dock = DockStyle.Fill,
            .Font = FNT_LABEL, .ForeColor = CLR_LABEL,
            .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 4, 0)
        }
        tblFin.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblFin.Controls.Add(lblImplicitRate, 0, 1)
        tblFin.Controls.Add(numImplicitRate, 1, 1)
        tblFin.Controls.Add(lblIBR, 2, 1)
        tblFin.Controls.Add(numIBR, 3, 1)
        tblFin.RowCount = 2

        ' Row 2: 適用割引率
        tblFin.RowStyles.Add(New RowStyle(SizeType.Absolute, 32.0F))
        tblFin.Controls.Add(lblAppliedRate, 0, 2)
        tblFin.SetColumnSpan(lblAppliedRate, 4)
        tblFin.RowCount = 3

        grpFinancial.Controls.Add(tblFin)

        mainTbl.Controls.Add(grpMonthly, 0, 0)
        mainTbl.Controls.Add(grpFinancial, 0, 1)

        scroll.Controls.Add(mainTbl)
        tabMonthlyDetail.Controls.Add(scroll)
    End Sub

    Private Sub UpdateAppliedRate()
        If numImplicitRate.Value > 0 Then
            lblAppliedRate.Text = "適用割引率: 計算利子率 " & numImplicitRate.Value.ToString("F2") & "%"
        Else
            lblAppliedRate.Text = "適用割引率: IBR " & numIBR.Value.ToString("F2") & "%"
        End If
    End Sub

    Private Sub OnMonthlyPaymentChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim grid As DataGridView = DirectCast(sender, DataGridView)
        Dim row As DataGridViewRow = grid.Rows(e.RowIndex)

        If e.ColumnIndex = grid.Columns("MAmountExTax").Index Then
            Dim amountExTax As Decimal = 0
            If row.Cells("MAmountExTax").Value IsNot Nothing Then
                Decimal.TryParse(row.Cells("MAmountExTax").Value.ToString(), amountExTax)
            End If
            Dim tax As Decimal = Math.Floor(amountExTax * 0.1D)
            row.Cells("MTax").Value = tax
            row.Cells("MTotalIncTax").Value = amountExTax + tax
        End If

        RecalcMonthlyTotals()
    End Sub

    Private Sub OnMonthlyPaymentCellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Dim grid As DataGridView = DirectCast(sender, DataGridView)
        grid.InvalidateCell(e.ColumnIndex, e.RowIndex)
    End Sub

    Private Sub RecalcMonthlyTotals()
        Dim totalExTax As Decimal = 0
        Dim totalTax As Decimal = 0
        Dim totalIncTax As Decimal = 0
        For Each row As DataGridViewRow In dgvMonthlyPayments.Rows
            If row.IsNewRow Then Continue For
            Dim ex As Decimal = 0
            Dim tx As Decimal = 0
            Dim inc As Decimal = 0
            If row.Cells("MAmountExTax").Value IsNot Nothing Then Decimal.TryParse(row.Cells("MAmountExTax").Value.ToString(), ex)
            If row.Cells("MTax").Value IsNot Nothing Then Decimal.TryParse(row.Cells("MTax").Value.ToString(), tx)
            If row.Cells("MTotalIncTax").Value IsNot Nothing Then Decimal.TryParse(row.Cells("MTotalIncTax").Value.ToString(), inc)
            totalExTax += ex
            totalTax += tx
            totalIncTax += inc
        Next
        lblMonthlyTotalExTax.Text = "税抜: " & totalExTax.ToString("N0")
        lblMonthlyTotalTax.Text = "税: " & totalTax.ToString("N0")
        lblMonthlyTotalIncTax.Text = "税込: " & totalIncTax.ToString("N0")
    End Sub

    Private Sub LoadAssetData()
        Try
            Using db As New CrudHelper()
                Dim params As New List(Of NpgsqlParameter)()
                params.Add(New NpgsqlParameter("@asset_id", AssetId))
                Dim dt As DataTable = db.GetDataTable(
                    "SELECT * FROM d_asset WHERE asset_id = @asset_id", params)
                If dt.Rows.Count > 0 Then
                    Dim row As DataRow = dt.Rows(0)
                    ' 新規3項目の読み込み
                    Dim accountClassVal As String = db.SafeConvert(Of String)(row("account_class"), "")
                    If Not String.IsNullOrEmpty(accountClassVal) Then
                        Dim idx As Integer = cmbAccountClass.FindStringExact(accountClassVal)
                        If idx >= 0 Then cmbAccountClass.SelectedIndex = idx
                    End If
                    txtAssetNo.Text = db.SafeConvert(Of String)(row("asset_no"), AssetId.ToString())
                    Dim qty As Integer = db.SafeConvert(Of Integer)(row("quantity"), 1)
                    If qty >= numQuantity.Minimum AndAlso qty <= numQuantity.Maximum Then
                        numQuantity.Value = qty
                    End If

                    txtPropertyName.Text = db.SafeConvert(Of String)(row("bukken_nm"), "")
                    txtLocation.Text = db.SafeConvert(Of String)(row("shozaichi"), "")
                    txtSection.Text = db.SafeConvert(Of String)(row("kukaku"), "")
                    txtArea.Text = db.SafeConvert(Of String)(row("menseki"), "")
                    txtLayout.Text = db.SafeConvert(Of String)(row("madori"), "")
                    txtStructure.Text = db.SafeConvert(Of String)(row("kozo_yoto"), "")
                    txtUsageRestrictions.Text = db.SafeConvert(Of String)(row("yoto_seigen"), "")
                    Dim usefulLife As Integer = db.SafeConvert(Of Integer)(row("taiyo_nensu"), 0)
                    If usefulLife >= numUsefulLife.Minimum AndAlso usefulLife <= numUsefulLife.Maximum Then
                        numUsefulLife.Value = usefulLife
                    End If
                    If row("shunko_dt") IsNot DBNull.Value Then
                        dtpCompletion.Checked = True
                        dtpCompletion.Value = CDate(row("shunko_dt"))
                    Else
                        dtpCompletion.Checked = False
                    End If
                    txtLandlordName.Text = db.SafeConvert(Of String)(row("kashushi_nm"), "")
                    txtBrokerCompany.Text = db.SafeConvert(Of String)(row("chukai_nm"), "")
                    txtPaymentAgent.Text = db.SafeConvert(Of String)(row("kessai_nm"), "")
                    txtGuarantor.Text = db.SafeConvert(Of String)(row("hosho_nm"), "")
                End If

                Dim eqParams As New List(Of NpgsqlParameter)()
                eqParams.Add(New NpgsqlParameter("@asset_id", AssetId))
                Dim eqDt As DataTable = db.GetDataTable(
                    "SELECT * FROM d_asset_equipment WHERE asset_id = @asset_id ORDER BY eq_id", eqParams)
                For Each eqRow As DataRow In eqDt.Rows
                    dgvSelfEquipment.Rows.Add(
                        db.SafeConvert(Of String)(eqRow("eq_name"), ""),
                        db.SafeConvert(Of String)(eqRow("eq_date"), ""),
                        db.SafeConvert(Of String)(eqRow("eq_amount"), ""))
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show("資産データの読み込みに失敗しました。" & vbCrLf & ex.Message,
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyReadOnlyMode()
        cmbAccountClass.Enabled = False
        numQuantity.Enabled = False
        txtPropertyName.ReadOnly = True
        txtLocation.ReadOnly = True
        txtSection.ReadOnly = True
        txtArea.ReadOnly = True
        txtLayout.ReadOnly = True
        txtStructure.ReadOnly = True
        txtUsageRestrictions.ReadOnly = True
        numUsefulLife.Enabled = False
        dtpCompletion.Enabled = False
        txtLandlordName.ReadOnly = True
        txtBrokerCompany.ReadOnly = True
        txtPaymentAgent.ReadOnly = True
        txtGuarantor.ReadOnly = True
        dgvSelfEquipment.ReadOnly = True
        dgvSelfEquipment.AllowUserToAddRows = False
        dgvAllocations.ReadOnly = True
        dgvAllocations.AllowUserToAddRows = False
        If dgvMonthlyPayments IsNot Nothing Then
            dgvMonthlyPayments.ReadOnly = True
            dgvMonthlyPayments.AllowUserToAddRows = False
        End If
        If numFairValue IsNot Nothing Then numFairValue.Enabled = False
        If numEconomicLife IsNot Nothing Then numEconomicLife.Enabled = False
        If numImplicitRate IsNot Nothing Then numImplicitRate.Enabled = False
        If numIBR IsNot Nothing Then numIBR.Enabled = False
        txtShikikin.ReadOnly = True
        txtHoshokin.ReadOnly = True
        txtReikin.ReadOnly = True
        txtBrokerFee.ReadOnly = True
        txtPrepaidRent.ReadOnly = True
        btnSave.Visible = False

        Dim readOnlyColor As Color = Color.FromArgb(233, 236, 239)
        txtPropertyName.BackColor = readOnlyColor
        txtLocation.BackColor = readOnlyColor
        txtSection.BackColor = readOnlyColor
        txtArea.BackColor = readOnlyColor
        txtLayout.BackColor = readOnlyColor
        txtStructure.BackColor = readOnlyColor
        txtUsageRestrictions.BackColor = readOnlyColor
        txtLandlordName.BackColor = readOnlyColor
        txtBrokerCompany.BackColor = readOnlyColor
        txtPaymentAgent.BackColor = readOnlyColor
        txtGuarantor.BackColor = readOnlyColor
        txtShikikin.BackColor = readOnlyColor
        txtHoshokin.BackColor = readOnlyColor
        txtReikin.BackColor = readOnlyColor
        txtBrokerFee.BackColor = readOnlyColor
        txtPrepaidRent.BackColor = readOnlyColor

        Me.Text = "資産詳細（照会）"
    End Sub

    Private Sub CalcBuildingAge()
        Try
            If dtpCompletion.Checked Then
                Dim age As Integer = DateTime.Now.Year - dtpCompletion.Value.Year
                lblBuildingAge.Text = age.ToString() & "年"
            Else
                lblBuildingAge.Text = "---年"
            End If
        Catch ex As Exception
            lblBuildingAge.Text = "---年"
        End Try
    End Sub

    Private Sub dtpCompletion_ValueChanged(sender As Object, e As EventArgs) Handles dtpCompletion.ValueChanged
        CalcBuildingAge()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If String.IsNullOrWhiteSpace(txtPropertyName.Text) Then
            MessageBox.Show("物件名を入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPropertyName.Focus()
            Return
        End If

        Try
            Using db As New CrudHelper()
                db.BeginTransaction()
                Try
                    Dim columnValues As New Dictionary(Of String, Object)()
                    columnValues.Add("account_class", AccountClass)
                    columnValues.Add("asset_no", txtAssetNo.Text)
                    columnValues.Add("quantity", CInt(numQuantity.Value))
                    columnValues.Add("bukken_nm", txtPropertyName.Text)
                    columnValues.Add("shozaichi", txtLocation.Text)
                    columnValues.Add("kukaku", txtSection.Text)
                    columnValues.Add("menseki", txtArea.Text)
                    columnValues.Add("madori", txtLayout.Text)
                    columnValues.Add("kozo_yoto", txtStructure.Text)
                    columnValues.Add("yoto_seigen", txtUsageRestrictions.Text)
                    columnValues.Add("taiyo_nensu", CInt(numUsefulLife.Value))
                    If dtpCompletion.Checked Then
                        columnValues.Add("shunko_dt", dtpCompletion.Value.Date)
                    Else
                        columnValues.Add("shunko_dt", DBNull.Value)
                    End If
                    columnValues.Add("kashushi_nm", txtLandlordName.Text)
                    columnValues.Add("chukai_nm", txtBrokerCompany.Text)
                    columnValues.Add("kessai_nm", txtPaymentAgent.Text)
                    columnValues.Add("hosho_nm", txtGuarantor.Text)
                    columnValues.Add("update_dt", DateTime.Now)

                    If AssetId > 0 Then
                        Dim whereParams As New List(Of NpgsqlParameter)()
                        whereParams.Add(New NpgsqlParameter("@w_asset_id", AssetId))
                        db.Update("d_asset", columnValues, "asset_id = @w_asset_id", whereParams)
                    Else
                        columnValues.Add("create_dt", DateTime.Now)
                        db.Insert("d_asset", columnValues)
                        AssetId = db.ExecuteScalar(Of Integer)(
                            "SELECT currval('d_asset_asset_id_seq')")
                    End If

                    Dim delParams As New List(Of NpgsqlParameter)()
                    delParams.Add(New NpgsqlParameter("@d_asset_id", AssetId))
                    db.Delete("d_asset_equipment", "asset_id = @d_asset_id", delParams)

                    For Each row As DataGridViewRow In dgvSelfEquipment.Rows
                        If row.IsNewRow Then Continue For
                        Dim eqName As String = If(row.Cells("SelfEquipName").Value IsNot Nothing,
                                                  row.Cells("SelfEquipName").Value.ToString(), "")
                        Dim eqDate As String = If(row.Cells("SelfEquipDate").Value IsNot Nothing,
                                                  row.Cells("SelfEquipDate").Value.ToString(), "")
                        Dim eqAmount As String = If(row.Cells("SelfEquipAmount").Value IsNot Nothing,
                                                    row.Cells("SelfEquipAmount").Value.ToString(), "")
                        If String.IsNullOrWhiteSpace(eqName) AndAlso
                           String.IsNullOrWhiteSpace(eqAmount) AndAlso
                           String.IsNullOrWhiteSpace(eqDate) Then Continue For

                        Dim eqValues As New Dictionary(Of String, Object)()
                        eqValues.Add("asset_id", AssetId)
                        eqValues.Add("eq_name", eqName)
                        eqValues.Add("eq_amount", eqAmount)
                        eqValues.Add("eq_date", eqDate)
                        db.Insert("d_asset_equipment", eqValues)
                    Next

                    db.Commit()
                Catch ex As Exception
                    db.Rollback()
                    Throw
                End Try
            End Using

            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("保存に失敗しました。" & vbCrLf & ex.Message,
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
