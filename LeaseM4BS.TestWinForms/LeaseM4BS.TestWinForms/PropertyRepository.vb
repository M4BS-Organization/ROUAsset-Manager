Imports System.Collections.Generic
Imports Npgsql
Imports LeaseM4BS.DataAccess

''' <summary>
''' ctb_property / ctb_property_attribute / m_property_attribute_def のDB操作
''' </summary>
Public Class PropertyRepository

    Private ReadOnly _connMgr As New DbConnectionManager()

    ' ------------------------------------------------------------------
    ' 属性定義マスタ取得
    ' ------------------------------------------------------------------

    ''' <summary>
    ''' 指定した資産カテゴリの属性定義一覧を取得する
    ''' </summary>
    Public Function GetAttributeDefs(assetCategoryCd As String) As List(Of PropertyAttributeDef)
        Const sql As String =
            "SELECT attr_def_id, asset_category_cd, attr_cd, attr_nm, " &
            "data_type, display_type, max_length, is_required, sort_order, default_value " &
            "FROM m_property_attribute_def " &
            "WHERE asset_category_cd = @asset_category_cd " &
            "ORDER BY sort_order"

        Dim results As New List(Of PropertyAttributeDef)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@asset_category_cd", assetCategoryCd)
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim def As New PropertyAttributeDef()
                        def.AttrDefId = reader.GetInt32(reader.GetOrdinal("attr_def_id"))
                        def.AssetCategoryCd = reader.GetString(reader.GetOrdinal("asset_category_cd"))
                        def.AttrCd = reader.GetString(reader.GetOrdinal("attr_cd"))
                        def.AttrNm = reader.GetString(reader.GetOrdinal("attr_nm"))
                        def.DataType = reader.GetString(reader.GetOrdinal("data_type"))
                        def.DisplayType = reader.GetString(reader.GetOrdinal("display_type"))
                        Dim mlOrd As Integer = reader.GetOrdinal("max_length")
                        def.MaxLength = If(reader.IsDBNull(mlOrd), Nothing, CType(reader.GetInt32(mlOrd), Integer?))
                        def.IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required"))
                        def.SortOrder = reader.GetInt32(reader.GetOrdinal("sort_order"))
                        Dim dvOrd As Integer = reader.GetOrdinal("default_value")
                        def.DefaultValue = If(reader.IsDBNull(dvOrd), "", reader.GetString(dvOrd))
                        results.Add(def)
                    End While
                End Using
            End Using
        End Using

        Return results
    End Function

    ' ------------------------------------------------------------------
    ' 物件マスタ CRUD
    ' ------------------------------------------------------------------

    ''' <summary>
    ''' 物件レコードをINSERTし、property_idを返す
    ''' </summary>
    Public Function InsertProperty(conn As NpgsqlConnection, txn As NpgsqlTransaction,
                                    rec As PropertyRecord) As Integer
        Const sql As String =
            "INSERT INTO ctb_property " &
            "(ctb_id, property_no, asset_category_cd, asset_no, asset_name, " &
            " company_name, install_location, remarks) " &
            "VALUES (@ctb_id, @property_no, @asset_category_cd, @asset_no, @asset_name, " &
            " @company_name, @install_location, @remarks) " &
            "RETURNING property_id"

        Using cmd As New NpgsqlCommand(sql, conn, txn)
            cmd.Parameters.AddWithValue("@ctb_id", rec.CtbId)
            cmd.Parameters.AddWithValue("@property_no", rec.PropertyNo)
            cmd.Parameters.AddWithValue("@asset_category_cd", rec.AssetCategoryCd)
            cmd.Parameters.AddWithValue("@asset_no", NullIfEmpty(rec.AssetNo))
            cmd.Parameters.AddWithValue("@asset_name", NullIfEmpty(rec.AssetName))
            cmd.Parameters.AddWithValue("@company_name", NullIfEmpty(rec.CompanyName))
            cmd.Parameters.AddWithValue("@install_location", NullIfEmpty(rec.InstallLocation))
            cmd.Parameters.AddWithValue("@remarks", NullIfEmpty(rec.Remarks))
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    ''' <summary>
    ''' EAV属性値を一括INSERT
    ''' </summary>
    Public Sub InsertPropertyAttributes(conn As NpgsqlConnection, txn As NpgsqlTransaction,
                                         propertyId As Integer, attributes As Dictionary(Of Integer, String))
        If attributes Is Nothing OrElse attributes.Count = 0 Then Return

        Const sql As String =
            "INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value) " &
            "VALUES (@property_id, @attr_def_id, @attribute_value) " &
            "ON CONFLICT (property_id, attr_def_id) DO UPDATE SET " &
            "attribute_value = EXCLUDED.attribute_value, update_dt = CURRENT_TIMESTAMP"

        For Each kvp In attributes
            If String.IsNullOrEmpty(kvp.Value) Then Continue For
            Using cmd As New NpgsqlCommand(sql, conn, txn)
                cmd.Parameters.AddWithValue("@property_id", propertyId)
                cmd.Parameters.AddWithValue("@attr_def_id", kvp.Key)
                cmd.Parameters.AddWithValue("@attribute_value", kvp.Value)
                cmd.ExecuteNonQuery()
            End Using
        Next
    End Sub

    ''' <summary>
    ''' 指定した物件のEAV属性値を定義付きで取得する
    ''' </summary>
    Public Function GetPropertyAttributes(propertyId As Integer, assetCategoryCd As String) As List(Of PropertyAttributeValue)
        Const sql As String =
            "SELECT d.attr_def_id, d.attr_cd, d.attr_nm, d.data_type, d.display_type, " &
            "d.max_length, d.is_required, d.sort_order, d.default_value, " &
            "a.attribute_value " &
            "FROM m_property_attribute_def d " &
            "LEFT JOIN ctb_property_attribute a " &
            "  ON d.attr_def_id = a.attr_def_id AND a.property_id = @property_id " &
            "WHERE d.asset_category_cd = @asset_category_cd " &
            "ORDER BY d.sort_order"

        Dim results As New List(Of PropertyAttributeValue)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@property_id", propertyId)
                cmd.Parameters.AddWithValue("@asset_category_cd", assetCategoryCd)
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim av As New PropertyAttributeValue()
                        av.AttrDefId = reader.GetInt32(reader.GetOrdinal("attr_def_id"))
                        av.AttrCd = reader.GetString(reader.GetOrdinal("attr_cd"))
                        av.AttrNm = reader.GetString(reader.GetOrdinal("attr_nm"))
                        av.DataType = reader.GetString(reader.GetOrdinal("data_type"))
                        av.DisplayType = reader.GetString(reader.GetOrdinal("display_type"))
                        Dim mlOrd As Integer = reader.GetOrdinal("max_length")
                        av.MaxLength = If(reader.IsDBNull(mlOrd), Nothing, CType(reader.GetInt32(mlOrd), Integer?))
                        av.IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required"))
                        av.SortOrder = reader.GetInt32(reader.GetOrdinal("sort_order"))
                        Dim dvOrd As Integer = reader.GetOrdinal("default_value")
                        av.DefaultValue = If(reader.IsDBNull(dvOrd), "", reader.GetString(dvOrd))
                        Dim valOrd As Integer = reader.GetOrdinal("attribute_value")
                        av.Value = If(reader.IsDBNull(valOrd), "", reader.GetString(valOrd))
                        results.Add(av)
                    End While
                End Using
            End Using
        End Using

        Return results
    End Function

    ''' <summary>
    ''' ctb_idから物件レコードを取得
    ''' </summary>
    Public Function GetByCtbId(ctbId As Integer) As List(Of PropertyRecord)
        Const sql As String =
            "SELECT property_id, ctb_id, property_no, asset_category_cd, " &
            "asset_no, asset_name, company_name, install_location, remarks " &
            "FROM ctb_property WHERE ctb_id = @ctb_id ORDER BY property_no"

        Dim results As New List(Of PropertyRecord)

        Using conn As NpgsqlConnection = _connMgr.GetConnection()
            Using cmd As New NpgsqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@ctb_id", ctbId)
                Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        results.Add(MapReaderToPropertyRecord(reader))
                    End While
                End Using
            End Using
        End Using

        Return results
    End Function

    Private Shared Function MapReaderToPropertyRecord(reader As NpgsqlDataReader) As PropertyRecord
        Dim rec As New PropertyRecord()
        rec.PropertyId = reader.GetInt32(reader.GetOrdinal("property_id"))
        rec.CtbId = reader.GetInt32(reader.GetOrdinal("ctb_id"))
        rec.PropertyNo = reader.GetInt32(reader.GetOrdinal("property_no"))
        rec.AssetCategoryCd = reader.GetString(reader.GetOrdinal("asset_category_cd"))
        Dim anOrd As Integer = reader.GetOrdinal("asset_no")
        rec.AssetNo = If(reader.IsDBNull(anOrd), "", reader.GetString(anOrd))
        Dim nmOrd As Integer = reader.GetOrdinal("asset_name")
        rec.AssetName = If(reader.IsDBNull(nmOrd), "", reader.GetString(nmOrd))
        Dim coOrd As Integer = reader.GetOrdinal("company_name")
        rec.CompanyName = If(reader.IsDBNull(coOrd), "", reader.GetString(coOrd))
        Dim ilOrd As Integer = reader.GetOrdinal("install_location")
        rec.InstallLocation = If(reader.IsDBNull(ilOrd), "", reader.GetString(ilOrd))
        Dim rmOrd As Integer = reader.GetOrdinal("remarks")
        rec.Remarks = If(reader.IsDBNull(rmOrd), "", reader.GetString(rmOrd))
        Return rec
    End Function

    Private Shared Function NullIfEmpty(value As String) As Object
        If String.IsNullOrEmpty(value) Then Return DBNull.Value
        Return value
    End Function

End Class

' ------------------------------------------------------------------
' データクラス
' ------------------------------------------------------------------

''' <summary>
''' 属性定義マスタレコード
''' </summary>
Public Class PropertyAttributeDef
    Public Property AttrDefId As Integer
    Public Property AssetCategoryCd As String = ""
    Public Property AttrCd As String = ""
    Public Property AttrNm As String = ""
    Public Property DataType As String = "TEXT"
    Public Property DisplayType As String = "TEXTBOX"
    Public Property MaxLength As Integer? = Nothing
    Public Property IsRequired As Boolean = False
    Public Property SortOrder As Integer = 0
    Public Property DefaultValue As String = ""
End Class

''' <summary>
''' 属性値（定義 + 値のペア）: 画面表示用
''' </summary>
Public Class PropertyAttributeValue
    Inherits PropertyAttributeDef
    Public Property Value As String = ""
End Class

''' <summary>
''' 物件マスタレコード
''' </summary>
Public Class PropertyRecord
    Public Property PropertyId As Integer
    Public Property CtbId As Integer
    Public Property PropertyNo As Integer = 1
    Public Property AssetCategoryCd As String = ""
    Public Property AssetNo As String = ""
    Public Property AssetName As String = ""
    Public Property CompanyName As String = ""
    Public Property InstallLocation As String = ""
    Public Property Remarks As String = ""

    ''' <summary>EAV属性値: key=attr_def_id, value=属性値テキスト</summary>
    Public Property Attributes As New Dictionary(Of Integer, String)

    ''' <summary>配賦情報</summary>
    Public Property DeptAllocations As New List(Of CtbDeptAllocation)
End Class
