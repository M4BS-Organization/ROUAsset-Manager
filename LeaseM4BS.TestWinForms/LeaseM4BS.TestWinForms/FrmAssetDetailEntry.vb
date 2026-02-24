Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

Partial Public Class FrmAssetDetailEntry

    Public Property AssetId As Integer = 0
    Public Property IsReadOnly As Boolean = False

    Public ReadOnly Property PropertyName As String
        Get
            Return txtPropertyName.Text
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
