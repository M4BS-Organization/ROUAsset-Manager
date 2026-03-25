Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' d_kykh（マイグレーション側）と ctb_contract_header（新リース側）間の
''' ID↔コード変換、およびマスタ逆引きを行うユーティリティクラス。
''' </summary>
Public Class ContractDataMapper

    Private ReadOnly _crud As New CrudHelper()

    ' =========================================================
    '  d_kykh → 画面表示用（IDからマスタ情報を取得）
    ' =========================================================

    ''' <summary>
    ''' kkbn_id から契約区分名を取得する。
    ''' </summary>
    Public Function GetKkbnName(kkbnId As Short) As String
        Try
            Dim sql As String = "SELECT kkbn_nm FROM c_kkbn WHERE kkbn_id = @id"
            Dim result = _crud.ExecuteScalar(Of Object)(sql, New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", kkbnId)})
            Return If(result IsNot Nothing, result.ToString(), "")
        Catch
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' lcpt_id からリース会社情報（コード, 名称）を取得する。
    ''' </summary>
    Public Function GetLcptInfo(lcptId As Integer) As (Code As String, Name As String)
        Try
            Dim sql As String = "SELECT lcpt1_cd, lcpt1_nm FROM m_lcpt WHERE lcpt_id = @id"
            Dim dt = _crud.GetDataTable(sql, New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@id", lcptId)
            })
            If dt.Rows.Count > 0 Then
                Return (dt.Rows(0)("lcpt1_cd").ToString(), dt.Rows(0)("lcpt1_nm").ToString())
            End If
        Catch
        End Try
        Return ("", "")
    End Function

    ''' <summary>
    ''' kknri_id から管理部門情報（コード, 名称）を取得する。
    ''' </summary>
    Public Function GetKknriInfo(kknriId As Integer) As (Code As String, Name As String)
        Try
            Dim sql As String = "SELECT kknri1_cd, kknri1_nm FROM m_kknri WHERE kknri_id = @id"
            Dim dt = _crud.GetDataTable(sql, New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@id", kknriId)
            })
            If dt.Rows.Count > 0 Then
                Return (dt.Rows(0)("kknri1_cd").ToString(), dt.Rows(0)("kknri1_nm").ToString())
            End If
        Catch
        End Try
        Return ("", "")
    End Function

    ' =========================================================
    '  画面選択 → d_kykh 保存用（ComboBoxインデックスからIDを取得）
    ' =========================================================

    ''' <summary>
    ''' 契約区分ComboBoxの選択テキストから kkbn_id を取得する。
    ''' </summary>
    Public Function GetKkbnId(contractTypeName As String) As Short
        Try
            Dim sql As String = "SELECT kkbn_id FROM c_kkbn WHERE kkbn_nm = @nm"
            Dim result = _crud.ExecuteScalar(Of Object)(sql, New List(Of NpgsqlParameter) From {New NpgsqlParameter("@nm", contractTypeName)})
            If result IsNot Nothing Then Return Convert.ToInt16(result)
        Catch
        End Try
        Return 1 ' デフォルト: リース
    End Function

    ''' <summary>
    ''' DataTable + 選択インデックスから lcpt_id を取得する。
    ''' </summary>
    Public Shared Function GetLcptId(supplierTable As DataTable, selectedIndex As Integer) As Integer
        If supplierTable IsNot Nothing AndAlso
           selectedIndex >= 0 AndAlso selectedIndex < supplierTable.Rows.Count Then
            Dim val = supplierTable.Rows(selectedIndex)("lcpt_id")
            If val IsNot Nothing AndAlso Not IsDBNull(val) Then Return Convert.ToInt32(val)
        End If
        Return 0
    End Function

    ''' <summary>
    ''' DataTable + 選択インデックスから kknri_id を取得する。
    ''' </summary>
    Public Shared Function GetKknriId(deptTable As DataTable, selectedIndex As Integer) As Integer
        If deptTable IsNot Nothing AndAlso
           selectedIndex >= 0 AndAlso selectedIndex < deptTable.Rows.Count Then
            Dim val = deptTable.Rows(selectedIndex)("kknri_id")
            If val IsNot Nothing AndAlso Not IsDBNull(val) Then Return Convert.ToInt32(val)
        End If
        Return 0
    End Function

    ' =========================================================
    '  ComboBox逆引き（d_kykhのID → ComboBoxのインデックスを特定）
    ' =========================================================

    ''' <summary>
    ''' kkbn_id から契約区分ComboBox内のインデックスを特定する。
    ''' </summary>
    Public Function FindKkbnIndex(cmb As ComboBox, kkbnId As Short) As Integer
        Dim name As String = GetKkbnName(kkbnId)
        If String.IsNullOrEmpty(name) Then Return -1
        For i As Integer = 0 To cmb.Items.Count - 1
            If cmb.Items(i).ToString().Contains(name) Then Return i
        Next
        Return -1
    End Function

    ''' <summary>
    ''' lcpt_id からリース会社ComboBox内のインデックスを特定する。
    ''' </summary>
    Public Shared Function FindLcptIndex(supplierTable As DataTable, lcptId As Integer) As Integer
        If supplierTable Is Nothing Then Return -1
        For i As Integer = 0 To supplierTable.Rows.Count - 1
            Dim val = supplierTable.Rows(i)("lcpt_id")
            If val IsNot Nothing AndAlso Not IsDBNull(val) AndAlso Convert.ToInt32(val) = lcptId Then
                Return i
            End If
        Next
        Return -1
    End Function

    ''' <summary>
    ''' kknri_id から管理部門ComboBox内のインデックスを特定する。
    ''' </summary>
    Public Shared Function FindKknriIndex(deptTable As DataTable, kknriId As Integer) As Integer
        If deptTable Is Nothing Then Return -1
        For i As Integer = 0 To deptTable.Rows.Count - 1
            Dim val = deptTable.Rows(i)("kknri_id")
            If val IsNot Nothing AndAlso Not IsDBNull(val) AndAlso Convert.ToInt32(val) = kknriId Then
                Return i
            End If
        Next
        Return -1
    End Function

    ' =========================================================
    '  d_kykh 採番
    ' =========================================================

    ''' <summary>
    ''' d_kykh の次の kykh_id を採番する（MAX+1）。
    ''' </summary>
    Public Function GetNextKykhId() As Integer
        Try
            Dim sql As String = "SELECT COALESCE(MAX(kykh_id), 0) FROM d_kykh"
            Dim result = _crud.ExecuteScalar(Of Object)(sql)
            Return Convert.ToInt32(result) + 1
        Catch
            Return 1
        End Try
    End Function

End Class
