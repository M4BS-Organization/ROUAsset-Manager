Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

''' <summary>
''' fc_系顧客固有仕訳出力設定管理ヘルパー
''' SetteiHelper の fc_系版。顧客コードをプレフィックスとして t_settei テーブルで設定を管理する。
''' Access版 fc_系フォームの gSET_* 系関数群に相当。
''' </summary>
Public Class FcSetteiHelper
    Implements IDisposable

    Private _crud As CrudHelper
    Private _disposed As Boolean = False

    ''' <summary>
    ''' t_settei における顧客設定のプレフィックス形式。
    ''' 例: "FC_KITOKU_" → settei_nm = "FC_KITOKU_OUTPUT_FOLDER" 等
    ''' </summary>
    Private ReadOnly _prefix As String

    ' ================================================================
    '  コンストラクタ
    ' ================================================================

    ''' <summary>
    ''' 顧客コードを指定して生成。内部で CrudHelper を生成する。
    ''' </summary>
    ''' <param name="customerCode">顧客コード (KITOKU, TSYSCOM, KYOTO 等)</param>
    Public Sub New(customerCode As String)
        If String.IsNullOrWhiteSpace(customerCode) Then
            Throw New ArgumentNullException(NameOf(customerCode))
        End If
        _prefix = $"FC_{customerCode.ToUpper()}_"
        _crud = New CrudHelper()
    End Sub

    ''' <summary>
    ''' CrudHelper 注入コンストラクタ（テスト用）。
    ''' </summary>
    Public Sub New(customerCode As String, crud As CrudHelper)
        If String.IsNullOrWhiteSpace(customerCode) Then
            Throw New ArgumentNullException(NameOf(customerCode))
        End If
        If crud Is Nothing Then
            Throw New ArgumentNullException(NameOf(crud))
        End If
        _prefix = $"FC_{customerCode.ToUpper()}_"
        _crud = crud
    End Sub

    ' ================================================================
    '  プロパティ
    ' ================================================================

    ''' <summary>設定プレフィックス (例: "FC_KITOKU_")</summary>
    Public ReadOnly Property Prefix As String
        Get
            Return _prefix
        End Get
    End Property

    ' ================================================================
    '  設定値の読み取り
    ' ================================================================

    ''' <summary>
    ''' 指定キーの設定値（テキスト）を取得する。
    ''' settei_nm = Prefix + key で t_settei を検索する。
    ''' </summary>
    ''' <param name="key">設定キー (例: "OUTPUT_FOLDER", "KMK_CD_DR")</param>
    ''' <param name="defaultValue">レコードがない場合のデフォルト値</param>
    Public Function GetText(key As String, Optional defaultValue As String = "") As String
        Dim row = GetSetteiRow(key)
        If row Is Nothing Then Return defaultValue
        If IsDBNull(row("val_text")) Then Return defaultValue
        Return CStr(row("val_text"))
    End Function

    ''' <summary>
    ''' 指定キーの設定値（数値）を取得する。
    ''' </summary>
    Public Function GetNumber(key As String, Optional defaultValue As Double = 0) As Double
        Dim row = GetSetteiRow(key)
        If row Is Nothing Then Return defaultValue
        If IsDBNull(row("val_number")) Then Return defaultValue
        Return CDbl(row("val_number"))
    End Function

    ''' <summary>
    ''' 当顧客の全設定レコードを取得する。
    ''' </summary>
    Public Function LoadAll() As List(Of SetteiRecord)
        Dim sql = "SELECT * FROM t_settei WHERE settei_nm LIKE @p1 ORDER BY settei_id"
        Dim dt = _crud.GetDataTable(sql,
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@p1", _prefix & "%")
            })

        Dim records As New List(Of SetteiRecord)
        For Each row As DataRow In dt.Rows
            records.Add(RowToRecord(row))
        Next
        Return records
    End Function

    ' ================================================================
    '  設定値の保存
    ' ================================================================

    ''' <summary>
    ''' 指定キーにテキスト値を保存する（UPSERT）。
    ''' </summary>
    Public Sub SetText(key As String, value As String, Optional nameJpn As String = "")
        UpsertSettei(key, nameJpn, 0, value, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' 指定キーに数値を保存する（UPSERT）。
    ''' </summary>
    Public Sub SetNumber(key As String, value As Double, Optional nameJpn As String = "")
        UpsertSettei(key, nameJpn, 1, Nothing, value, Nothing)
    End Sub

    ''' <summary>
    ''' 設定レコードリストを一括保存する（DELETE + INSERT）。
    ''' </summary>
    Public Sub SaveAll(records As List(Of SetteiRecord))
        If records Is Nothing Then Throw New ArgumentNullException(NameOf(records))

        Dim ownTx = Not _crud.IsInTransaction
        Try
            If ownTx Then _crud.BeginTransaction()

            _crud.ExecuteNonQuery(
                "DELETE FROM t_settei WHERE settei_nm LIKE @p1",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@p1", _prefix & "%")
                })

            For Each rec In records
                InsertSetteiRecord(rec)
            Next

            If ownTx Then _crud.Commit()
        Catch
            If ownTx Then _crud.Rollback()
            Throw
        End Try
    End Sub

    ' ================================================================
    '  Private ヘルパー
    ' ================================================================

    Private Function GetSetteiRow(key As String) As DataRow
        Dim setteiNm = _prefix & key.ToUpper()
        Dim dt = _crud.GetDataTable(
            "SELECT * FROM t_settei WHERE settei_nm = @p1 LIMIT 1",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@p1", setteiNm)
            })
        If dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Private Sub UpsertSettei(key As String, nameJpn As String, setteiType As Integer,
                              valText As String, valNumber As Double?, valDatetime As DateTime?)
        Dim setteiNm = _prefix & key.ToUpper()
        _crud.ExecuteNonQuery(
            "INSERT INTO t_settei (settei_nm, settei_nm_jpn, settei_type, val_text, val_number, val_datetime) " &
            "VALUES (@nm, @nmj, @type, @text, @num, @dt) " &
            "ON CONFLICT (settei_nm) DO UPDATE SET " &
            "  settei_nm_jpn = EXCLUDED.settei_nm_jpn, " &
            "  settei_type = EXCLUDED.settei_type, " &
            "  val_text = EXCLUDED.val_text, " &
            "  val_number = EXCLUDED.val_number, " &
            "  val_datetime = EXCLUDED.val_datetime",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nm", setteiNm),
                New NpgsqlParameter("@nmj", If(CObj(nameJpn), DBNull.Value)),
                New NpgsqlParameter("@type", setteiType),
                New NpgsqlParameter("@text", If(CObj(valText), DBNull.Value)),
                New NpgsqlParameter("@num", If(valNumber.HasValue, CObj(valNumber.Value), DBNull.Value)),
                New NpgsqlParameter("@dt", If(valDatetime.HasValue, CObj(valDatetime.Value), DBNull.Value))
            })
    End Sub

    Private Sub InsertSetteiRecord(rec As SetteiRecord)
        _crud.ExecuteNonQuery(
            "INSERT INTO t_settei (settei_id, settei_nm, settei_nm_jpn, settei_type, val_text, val_number, val_datetime, biko) " &
            "VALUES (@id, @nm, @nmj, @type, @text, @num, @dt, @biko)",
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@id", rec.SetteiId),
                New NpgsqlParameter("@nm", If(CObj(rec.SetteiNm), DBNull.Value)),
                New NpgsqlParameter("@nmj", If(CObj(rec.SetteiNmJpn), DBNull.Value)),
                New NpgsqlParameter("@type", rec.SetteiType),
                New NpgsqlParameter("@text", If(CObj(rec.ValText), DBNull.Value)),
                New NpgsqlParameter("@num", If(rec.ValNumber.HasValue, CObj(rec.ValNumber.Value), DBNull.Value)),
                New NpgsqlParameter("@dt", If(rec.ValDatetime.HasValue, CObj(rec.ValDatetime.Value), DBNull.Value)),
                New NpgsqlParameter("@biko", If(CObj(rec.Biko), DBNull.Value))
            })
    End Sub

    Private Shared Function RowToRecord(row As DataRow) As SetteiRecord
        Return New SetteiRecord() With {
            .SetteiId = CInt(row("settei_id")),
            .SetteiNm = If(IsDBNull(row("settei_nm")), "", CStr(row("settei_nm"))),
            .SetteiNmJpn = If(IsDBNull(row("settei_nm_jpn")), "", CStr(row("settei_nm_jpn"))),
            .SetteiType = If(IsDBNull(row("settei_type")), 0, CInt(row("settei_type"))),
            .ValText = If(IsDBNull(row("val_text")), "", CStr(row("val_text"))),
            .ValNumber = If(IsDBNull(row("val_number")), CType(Nothing, Double?), CDbl(row("val_number"))),
            .ValDatetime = If(IsDBNull(row("val_datetime")), CType(Nothing, DateTime?), CDate(row("val_datetime"))),
            .Biko = If(IsDBNull(row("biko")), "", CStr(row("biko")))
        }
    End Function

    ' ================================================================
    '  IDisposable
    ' ================================================================

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _disposed Then
            _crud?.Dispose()
            _disposed = True
        End If
    End Sub

End Class
