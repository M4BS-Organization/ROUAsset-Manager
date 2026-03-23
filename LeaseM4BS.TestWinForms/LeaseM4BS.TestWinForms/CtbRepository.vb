Imports System.Collections.Generic
Imports Npgsql
Imports LeaseM4BS.DataAccess

''' <summary>
''' CTB DB persistence: ctb_lease_integrated / ctb_dept_allocation
''' </summary>
Public Class CtbRepository

    Private ReadOnly _connMgr As New DbConnectionManager()

    ''' <summary>
    ''' CTBレコード群をDBに一括INSERT（1トランザクション）
    ''' </summary>
    Public Sub InsertAll(records As List(Of CtbRecord))
        If records Is Nothing OrElse records.Count = 0 Then Return

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using txn As NpgsqlTransaction = conn.BeginTransaction()
                Try
                    For Each rec In records
                        Dim ctbId As Integer = InsertLeaseIntegrated(conn, txn, rec)
                        InsertDeptAllocation(conn, txn, ctbId, rec.DeptCd, rec.AllocationRatio, rec.MonthlyPayment)
                    Next
                    txn.Commit()
                Catch
                    txn.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Private Function InsertLeaseIntegrated(conn As NpgsqlConnection, txn As NpgsqlTransaction, rec As CtbRecord) As Integer
        Const sql As String =
            "INSERT INTO ctb_lease_integrated (" &
            "contract_no, property_no, contract_name, contract_type_cd, supplier_cd, mgmt_dept_cd, " &
            "lease_start_date, lease_end_date, free_rent_months, lease_term_months, " &
            "asset_no, asset_category, asset_name, company_name, install_location, remarks, " &
            "monthly_payment, lease_depreciation, total_payment, split_status, " &
            "re_structure, re_area, re_layout, re_completion_date, re_landlord_name, re_broker_company, re_usage_restrictions, " &
            "vh_chassis_no, vh_registration_no, vh_vehicle_type, vh_inspection_date, vh_mileage_limit, " &
            "oa_model_no, oa_serial_no, oa_maintenance_date, oa_maintenance_contract" &
            ") VALUES (" &
            "@contract_no, @property_no, @contract_name, @contract_type_cd, @supplier_cd, @mgmt_dept_cd, " &
            "@lease_start_date, @lease_end_date, @free_rent_months, @lease_term_months, " &
            "@asset_no, @asset_category, @asset_name, @company_name, @install_location, @remarks, " &
            "@monthly_payment, @lease_depreciation, @total_payment, @split_status, " &
            "@re_structure, @re_area, @re_layout, @re_completion_date, @re_landlord_name, @re_broker_company, @re_usage_restrictions, " &
            "@vh_chassis_no, @vh_registration_no, @vh_vehicle_type, @vh_inspection_date, @vh_mileage_limit, " &
            "@oa_model_no, @oa_serial_no, @oa_maintenance_date, @oa_maintenance_contract" &
            ") RETURNING ctb_id"

        Using cmd As New NpgsqlCommand(sql, conn, txn)
            cmd.Parameters.AddWithValue("@contract_no", rec.ContractNo)
            cmd.Parameters.AddWithValue("@property_no", rec.PropertyNo)
            cmd.Parameters.AddWithValue("@contract_name", NullIfEmpty(rec.ContractName))
            cmd.Parameters.AddWithValue("@contract_type_cd", NullIfEmpty(rec.ContractTypeCd))
            cmd.Parameters.AddWithValue("@supplier_cd", NullIfEmpty(rec.SupplierCd))
            cmd.Parameters.AddWithValue("@mgmt_dept_cd", NullIfEmpty(rec.MgmtDeptCd))
            cmd.Parameters.AddWithValue("@lease_start_date", NullIfNothing(rec.LeaseStartDate))
            cmd.Parameters.AddWithValue("@lease_end_date", NullIfNothing(rec.LeaseEndDate))
            cmd.Parameters.AddWithValue("@free_rent_months", rec.FreeRentMonths)
            cmd.Parameters.AddWithValue("@lease_term_months", NullIfNothing(rec.LeaseTermMonths))
            cmd.Parameters.AddWithValue("@asset_no", NullIfEmpty(rec.AssetNo))
            cmd.Parameters.AddWithValue("@asset_category", NullIfEmpty(rec.AssetCategory))
            cmd.Parameters.AddWithValue("@asset_name", NullIfEmpty(rec.AssetName))
            cmd.Parameters.AddWithValue("@company_name", NullIfEmpty(rec.CompanyName))
            cmd.Parameters.AddWithValue("@install_location", NullIfEmpty(rec.InstallLocation))
            cmd.Parameters.AddWithValue("@remarks", NullIfEmpty(rec.Remarks))
            cmd.Parameters.AddWithValue("@monthly_payment", rec.MonthlyPayment)
            cmd.Parameters.AddWithValue("@lease_depreciation", rec.LeaseDepreciation)
            cmd.Parameters.AddWithValue("@total_payment", rec.TotalPayment)
            cmd.Parameters.AddWithValue("@split_status", rec.SplitStatus)
            cmd.Parameters.AddWithValue("@re_structure", NullIfEmpty(rec.ReStructure))
            cmd.Parameters.AddWithValue("@re_area", NullIfEmpty(rec.ReArea))
            cmd.Parameters.AddWithValue("@re_layout", NullIfEmpty(rec.ReLayout))
            cmd.Parameters.AddWithValue("@re_completion_date", NullIfNothing(rec.ReCompletionDate))
            cmd.Parameters.AddWithValue("@re_landlord_name", NullIfEmpty(rec.ReLandlordName))
            cmd.Parameters.AddWithValue("@re_broker_company", NullIfEmpty(rec.ReBrokerCompany))
            cmd.Parameters.AddWithValue("@re_usage_restrictions", NullIfEmpty(rec.ReUsageRestrictions))
            cmd.Parameters.AddWithValue("@vh_chassis_no", NullIfEmpty(rec.VhChassisNo))
            cmd.Parameters.AddWithValue("@vh_registration_no", NullIfEmpty(rec.VhRegistrationNo))
            cmd.Parameters.AddWithValue("@vh_vehicle_type", NullIfEmpty(rec.VhVehicleType))
            cmd.Parameters.AddWithValue("@vh_inspection_date", NullIfNothing(rec.VhInspectionDate))
            cmd.Parameters.AddWithValue("@vh_mileage_limit", NullIfEmpty(rec.VhMileageLimit))
            cmd.Parameters.AddWithValue("@oa_model_no", NullIfEmpty(rec.OaModelNo))
            cmd.Parameters.AddWithValue("@oa_serial_no", NullIfEmpty(rec.OaSerialNo))
            cmd.Parameters.AddWithValue("@oa_maintenance_date", NullIfNothing(rec.OaMaintenanceDate))
            cmd.Parameters.AddWithValue("@oa_maintenance_contract", NullIfEmpty(rec.OaMaintenanceContract))

            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Private Sub InsertDeptAllocation(conn As NpgsqlConnection, txn As NpgsqlTransaction,
                                     ctbId As Integer, deptCd As String, ratio As Decimal, paymentAmount As Decimal)
        If String.IsNullOrEmpty(deptCd) Then Return

        Const sql As String =
            "INSERT INTO ctb_dept_allocation (ctb_id, dept_cd, allocation_ratio, payment_amount) " &
            "VALUES (@ctb_id, @dept_cd, @allocation_ratio, @payment_amount)"

        Using cmd As New NpgsqlCommand(sql, conn, txn)
            cmd.Parameters.AddWithValue("@ctb_id", ctbId)
            cmd.Parameters.AddWithValue("@dept_cd", deptCd)
            cmd.Parameters.AddWithValue("@allocation_ratio", ratio)
            cmd.Parameters.AddWithValue("@payment_amount", paymentAmount)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ''' <summary>
    ''' DBからCTBレコードを全件取得（ctb_dept_allocation JOIN）
    ''' </summary>
    Public Function SelectAll() As List(Of CtbRecord)
        Const sql As String =
            "SELECT c.ctb_id, c.contract_no, c.property_no, " &
            "c.contract_name, c.contract_type_cd, c.supplier_cd, c.mgmt_dept_cd, " &
            "c.lease_start_date, c.lease_end_date, c.free_rent_months, c.lease_term_months, " &
            "c.asset_no, c.asset_category, c.asset_name, c.company_name, c.install_location, c.remarks, " &
            "c.monthly_payment, c.lease_depreciation, c.total_payment, c.split_status, " &
            "c.re_structure, c.re_area, c.re_layout, c.re_completion_date, c.re_landlord_name, c.re_broker_company, c.re_usage_restrictions, " &
            "c.vh_chassis_no, c.vh_registration_no, c.vh_vehicle_type, c.vh_inspection_date, c.vh_mileage_limit, " &
            "c.oa_model_no, c.oa_serial_no, c.oa_maintenance_date, c.oa_maintenance_contract, " &
            "d.dept_cd, md.dept_nm, d.allocation_ratio " &
            "FROM ctb_lease_integrated c " &
            "LEFT JOIN ctb_dept_allocation d ON c.ctb_id = d.ctb_id " &
            "LEFT JOIN m_department md ON d.dept_cd = md.dept_cd " &
            "ORDER BY c.contract_no, c.property_no"

        Dim results As New List(Of CtbRecord)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim rec As New CtbRecord()
                        rec.CtbId = reader.GetInt32(reader.GetOrdinal("ctb_id"))
                        rec.ContractNo = SafeGetString(reader, "contract_no")
                        rec.PropertyNo = If(reader.IsDBNull(reader.GetOrdinal("property_no")), 1, reader.GetInt32(reader.GetOrdinal("property_no")))
                        rec.ContractName = SafeGetString(reader, "contract_name")
                        rec.ContractTypeCd = SafeGetString(reader, "contract_type_cd")
                        rec.SupplierCd = SafeGetString(reader, "supplier_cd")
                        rec.MgmtDeptCd = SafeGetString(reader, "mgmt_dept_cd")
                        rec.LeaseStartDate = SafeGetDate(reader, "lease_start_date")
                        rec.LeaseEndDate = SafeGetDate(reader, "lease_end_date")
                        rec.FreeRentMonths = If(reader.IsDBNull(reader.GetOrdinal("free_rent_months")), 0, reader.GetInt32(reader.GetOrdinal("free_rent_months")))
                        rec.LeaseTermMonths = SafeGetInt(reader, "lease_term_months")
                        rec.AssetNo = SafeGetString(reader, "asset_no")
                        rec.AssetCategory = SafeGetString(reader, "asset_category")
                        rec.AssetName = SafeGetString(reader, "asset_name")
                        rec.CompanyName = SafeGetString(reader, "company_name")
                        rec.InstallLocation = SafeGetString(reader, "install_location")
                        rec.Remarks = SafeGetString(reader, "remarks")
                        rec.MonthlyPayment = SafeGetDecimal(reader, "monthly_payment")
                        rec.LeaseDepreciation = SafeGetDecimal(reader, "lease_depreciation")
                        rec.TotalPayment = SafeGetDecimal(reader, "total_payment")
                        rec.SplitStatus = SafeGetString(reader, "split_status")
                        rec.ReStructure = SafeGetString(reader, "re_structure")
                        rec.ReArea = SafeGetString(reader, "re_area")
                        rec.ReLayout = SafeGetString(reader, "re_layout")
                        rec.ReCompletionDate = SafeGetDate(reader, "re_completion_date")
                        rec.ReLandlordName = SafeGetString(reader, "re_landlord_name")
                        rec.ReBrokerCompany = SafeGetString(reader, "re_broker_company")
                        rec.ReUsageRestrictions = SafeGetString(reader, "re_usage_restrictions")
                        rec.VhChassisNo = SafeGetString(reader, "vh_chassis_no")
                        rec.VhRegistrationNo = SafeGetString(reader, "vh_registration_no")
                        rec.VhVehicleType = SafeGetString(reader, "vh_vehicle_type")
                        rec.VhInspectionDate = SafeGetDate(reader, "vh_inspection_date")
                        rec.VhMileageLimit = SafeGetString(reader, "vh_mileage_limit")
                        rec.OaModelNo = SafeGetString(reader, "oa_model_no")
                        rec.OaSerialNo = SafeGetString(reader, "oa_serial_no")
                        rec.OaMaintenanceDate = SafeGetDate(reader, "oa_maintenance_date")
                        rec.OaMaintenanceContract = SafeGetString(reader, "oa_maintenance_contract")
                        rec.DeptCd = SafeGetString(reader, "dept_cd")
                        rec.DeptName = SafeGetString(reader, "dept_nm")
                        rec.AllocationRatio = If(reader.IsDBNull(reader.GetOrdinal("allocation_ratio")), 0D, reader.GetDecimal(reader.GetOrdinal("allocation_ratio")))
                        results.Add(rec)
                    End While
                End Using
            End Using
        End Using

        Return results
    End Function

    ''' <summary>
    ''' 契約番号を指定してCTBレコードを取得する
    ''' </summary>
    Public Function SelectByContractNo(contractNo As String) As List(Of CtbRecord)
        If String.IsNullOrEmpty(contractNo) Then Return New List(Of CtbRecord)

        Const sql As String =
            "SELECT c.ctb_id, c.contract_no, c.property_no, " &
            "c.contract_name, c.contract_type_cd, c.supplier_cd, c.mgmt_dept_cd, " &
            "c.lease_start_date, c.lease_end_date, c.free_rent_months, c.lease_term_months, " &
            "c.asset_no, c.asset_category, c.asset_name, c.company_name, c.install_location, c.remarks, " &
            "c.monthly_payment, c.lease_depreciation, c.total_payment, c.split_status, " &
            "c.re_structure, c.re_area, c.re_layout, c.re_completion_date, c.re_landlord_name, c.re_broker_company, c.re_usage_restrictions, " &
            "c.vh_chassis_no, c.vh_registration_no, c.vh_vehicle_type, c.vh_inspection_date, c.vh_mileage_limit, " &
            "c.oa_model_no, c.oa_serial_no, c.oa_maintenance_date, c.oa_maintenance_contract, " &
            "d.dept_cd, md.dept_nm, d.allocation_ratio " &
            "FROM ctb_lease_integrated c " &
            "LEFT JOIN ctb_dept_allocation d ON c.ctb_id = d.ctb_id " &
            "LEFT JOIN m_department md ON d.dept_cd = md.dept_cd " &
            "WHERE c.contract_no = @contract_no " &
            "ORDER BY c.property_no"

        Dim results As New List(Of CtbRecord)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@contract_no", contractNo)
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        results.Add(MapReaderToCtbRecord(reader))
                    End While
                End Using
            End Using
        End Using

        Return results
    End Function

    ''' <summary>
    ''' NpgsqlDataReader から CtbRecord へのマッピング
    ''' </summary>
    Private Shared Function MapReaderToCtbRecord(reader As NpgsqlDataReader) As CtbRecord
        Dim rec As New CtbRecord()
        rec.CtbId = reader.GetInt32(reader.GetOrdinal("ctb_id"))
        rec.ContractNo = SafeGetString(reader, "contract_no")
        rec.PropertyNo = If(reader.IsDBNull(reader.GetOrdinal("property_no")), 1, reader.GetInt32(reader.GetOrdinal("property_no")))
        rec.ContractName = SafeGetString(reader, "contract_name")
        rec.ContractTypeCd = SafeGetString(reader, "contract_type_cd")
        rec.SupplierCd = SafeGetString(reader, "supplier_cd")
        rec.MgmtDeptCd = SafeGetString(reader, "mgmt_dept_cd")
        rec.LeaseStartDate = SafeGetDate(reader, "lease_start_date")
        rec.LeaseEndDate = SafeGetDate(reader, "lease_end_date")
        rec.FreeRentMonths = If(reader.IsDBNull(reader.GetOrdinal("free_rent_months")), 0, reader.GetInt32(reader.GetOrdinal("free_rent_months")))
        rec.LeaseTermMonths = SafeGetInt(reader, "lease_term_months")
        rec.AssetNo = SafeGetString(reader, "asset_no")
        rec.AssetCategory = SafeGetString(reader, "asset_category")
        rec.AssetName = SafeGetString(reader, "asset_name")
        rec.CompanyName = SafeGetString(reader, "company_name")
        rec.InstallLocation = SafeGetString(reader, "install_location")
        rec.Remarks = SafeGetString(reader, "remarks")
        rec.MonthlyPayment = SafeGetDecimal(reader, "monthly_payment")
        rec.LeaseDepreciation = SafeGetDecimal(reader, "lease_depreciation")
        rec.TotalPayment = SafeGetDecimal(reader, "total_payment")
        rec.SplitStatus = SafeGetString(reader, "split_status")
        rec.ReStructure = SafeGetString(reader, "re_structure")
        rec.ReArea = SafeGetString(reader, "re_area")
        rec.ReLayout = SafeGetString(reader, "re_layout")
        rec.ReCompletionDate = SafeGetDate(reader, "re_completion_date")
        rec.ReLandlordName = SafeGetString(reader, "re_landlord_name")
        rec.ReBrokerCompany = SafeGetString(reader, "re_broker_company")
        rec.ReUsageRestrictions = SafeGetString(reader, "re_usage_restrictions")
        rec.VhChassisNo = SafeGetString(reader, "vh_chassis_no")
        rec.VhRegistrationNo = SafeGetString(reader, "vh_registration_no")
        rec.VhVehicleType = SafeGetString(reader, "vh_vehicle_type")
        rec.VhInspectionDate = SafeGetDate(reader, "vh_inspection_date")
        rec.VhMileageLimit = SafeGetString(reader, "vh_mileage_limit")
        rec.OaModelNo = SafeGetString(reader, "oa_model_no")
        rec.OaSerialNo = SafeGetString(reader, "oa_serial_no")
        rec.OaMaintenanceDate = SafeGetDate(reader, "oa_maintenance_date")
        rec.OaMaintenanceContract = SafeGetString(reader, "oa_maintenance_contract")
        rec.DeptCd = SafeGetString(reader, "dept_cd")
        rec.DeptName = SafeGetString(reader, "dept_nm")
        rec.AllocationRatio = If(reader.IsDBNull(reader.GetOrdinal("allocation_ratio")), 0D, reader.GetDecimal(reader.GetOrdinal("allocation_ratio")))
        Return rec
    End Function

    Private Shared Function NullIfEmpty(value As String) As Object
        If String.IsNullOrEmpty(value) Then Return DBNull.Value
        Return value
    End Function

    Private Shared Function NullIfNothing(Of T As Structure)(value As T?) As Object
        If value.HasValue Then Return value.Value
        Return DBNull.Value
    End Function

    Private Shared Function SafeGetString(reader As NpgsqlDataReader, column As String) As String
        Dim ordinal As Integer = reader.GetOrdinal(column)
        If reader.IsDBNull(ordinal) Then Return ""
        Return reader.GetString(ordinal)
    End Function

    Private Shared Function SafeGetDate(reader As NpgsqlDataReader, column As String) As Date?
        Dim ordinal As Integer = reader.GetOrdinal(column)
        If reader.IsDBNull(ordinal) Then Return Nothing
        Return reader.GetDateTime(ordinal)
    End Function

    Private Shared Function SafeGetInt(reader As NpgsqlDataReader, column As String) As Integer?
        Dim ordinal As Integer = reader.GetOrdinal(column)
        If reader.IsDBNull(ordinal) Then Return Nothing
        Return reader.GetInt32(ordinal)
    End Function

    Private Shared Function SafeGetDecimal(reader As NpgsqlDataReader, column As String) As Decimal
        Dim ordinal As Integer = reader.GetOrdinal(column)
        If reader.IsDBNull(ordinal) Then Return 0D
        Return reader.GetDecimal(ordinal)
    End Function

End Class
