Imports System.Collections.Generic
Imports Npgsql
Imports LeaseM4BS.DataAccess

''' <summary>
''' CTB DB persistence: ctb_lease_integrated / ctb_dept_allocation / ctb_property
''' </summary>
Public Class CtbRepository

    Private ReadOnly _connMgr As New DbConnectionManager()
    Private ReadOnly _propRepo As New PropertyRepository()

    ''' <summary>
    ''' CTBレコード群をDBに一括INSERT（1トランザクション）
    ''' ctb_lease_integrated + ctb_property + ctb_property_attribute + ctb_dept_allocation
    ''' </summary>
    Public Sub InsertAll(records As List(Of CtbRecord))
        If records Is Nothing OrElse records.Count = 0 Then Return

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using txn As NpgsqlTransaction = conn.BeginTransaction()
                Try
                    For Each rec In records
                        Dim ctbId As Integer = InsertLeaseIntegrated(conn, txn, rec)

                        ' 物件マスタ + EAV属性
                        If rec.PropertyRec IsNot Nothing Then
                            rec.PropertyRec.CtbId = ctbId
                            Dim propId As Integer = _propRepo.InsertProperty(conn, txn, rec.PropertyRec)
                            _propRepo.InsertPropertyAttributes(conn, txn, propId, rec.PropertyRec.Attributes)
                        End If

                        ' 配賦
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

    ''' <summary>
    ''' ctb_lease_integrated INSERT（資産情報はctb_propertyに移行済みのため含まない）
    ''' </summary>
    Private Function InsertLeaseIntegrated(conn As NpgsqlConnection, txn As NpgsqlTransaction, rec As CtbRecord) As Integer
        Const sql As String =
            "INSERT INTO ctb_lease_integrated (" &
            "contract_no, property_no, contract_name, contract_type_cd, supplier_cd, mgmt_dept_cd, " &
            "lease_start_date, lease_end_date, free_rent_months, lease_term_months, " &
            "asset_category_cd, " &
            "monthly_payment, lease_depreciation, total_payment, split_status" &
            ") VALUES (" &
            "@contract_no, @property_no, @contract_name, @contract_type_cd, @supplier_cd, @mgmt_dept_cd, " &
            "@lease_start_date, @lease_end_date, @free_rent_months, @lease_term_months, " &
            "@asset_category_cd, " &
            "@monthly_payment, @lease_depreciation, @total_payment, @split_status" &
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
            cmd.Parameters.AddWithValue("@asset_category_cd", NullIfEmpty(rec.AssetCategoryCd))
            cmd.Parameters.AddWithValue("@monthly_payment", rec.MonthlyPayment)
            cmd.Parameters.AddWithValue("@lease_depreciation", rec.LeaseDepreciation)
            cmd.Parameters.AddWithValue("@total_payment", rec.TotalPayment)
            cmd.Parameters.AddWithValue("@split_status", rec.SplitStatus)

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
    ''' DBからCTBレコードを全件取得（ctb_property + ctb_dept_allocation JOIN）
    ''' 資産情報はctb_propertyから取得する
    ''' </summary>
    Public Function SelectAll() As List(Of CtbRecord)
        Const sql As String =
            "SELECT c.ctb_id, c.contract_no, c.property_no, " &
            "c.contract_name, c.contract_type_cd, c.supplier_cd, c.mgmt_dept_cd, " &
            "c.lease_start_date, c.lease_end_date, c.free_rent_months, c.lease_term_months, " &
            "COALESCE(p.asset_category_cd, c.asset_category_cd) AS asset_category_cd, " &
            "c.monthly_payment, c.lease_depreciation, c.total_payment, c.split_status, " &
            "p.asset_no, p.asset_name, p.company_name, p.install_location, p.remarks AS property_remarks, " &
            "d.dept_cd, md.dept_nm, d.allocation_ratio " &
            "FROM ctb_lease_integrated c " &
            "LEFT JOIN ctb_property p ON c.ctb_id = p.ctb_id " &
            "LEFT JOIN ctb_dept_allocation d ON c.ctb_id = d.ctb_id " &
            "LEFT JOIN m_department md ON d.dept_cd = md.dept_cd " &
            "ORDER BY c.contract_no, c.property_no"

        Dim results As New List(Of CtbRecord)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
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
    ''' 契約番号を指定してCTBレコードを取得する
    ''' </summary>
    Public Function SelectByContractNo(contractNo As String) As List(Of CtbRecord)
        If String.IsNullOrEmpty(contractNo) Then Return New List(Of CtbRecord)

        Const sql As String =
            "SELECT c.ctb_id, c.contract_no, c.property_no, " &
            "c.contract_name, c.contract_type_cd, c.supplier_cd, c.mgmt_dept_cd, " &
            "c.lease_start_date, c.lease_end_date, c.free_rent_months, c.lease_term_months, " &
            "COALESCE(p.asset_category_cd, c.asset_category_cd) AS asset_category_cd, " &
            "c.monthly_payment, c.lease_depreciation, c.total_payment, c.split_status, " &
            "p.asset_no, p.asset_name, p.company_name, p.install_location, p.remarks AS property_remarks, " &
            "d.dept_cd, md.dept_nm, d.allocation_ratio " &
            "FROM ctb_lease_integrated c " &
            "LEFT JOIN ctb_property p ON c.ctb_id = p.ctb_id " &
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
    ''' NpgsqlDataReader → CtbRecord マッピング
    ''' 資産情報はctb_property（p.）から取得
    ''' </summary>
    Private Shared Function MapReaderToCtbRecord(reader As NpgsqlDataReader) As CtbRecord
        Dim rec As New CtbRecord()
        ' --- ctb_lease_integrated ---
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
        rec.AssetCategoryCd = SafeGetString(reader, "asset_category_cd")
        rec.MonthlyPayment = SafeGetDecimal(reader, "monthly_payment")
        rec.LeaseDepreciation = SafeGetDecimal(reader, "lease_depreciation")
        rec.TotalPayment = SafeGetDecimal(reader, "total_payment")
        rec.SplitStatus = SafeGetString(reader, "split_status")
        ' --- ctb_property (p.) ---
        rec.AssetNo = SafeGetString(reader, "asset_no")
        rec.AssetName = SafeGetString(reader, "asset_name")
        rec.CompanyName = SafeGetString(reader, "company_name")
        rec.InstallLocation = SafeGetString(reader, "install_location")
        rec.Remarks = SafeGetString(reader, "property_remarks")
        ' --- ctb_dept_allocation ---
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
