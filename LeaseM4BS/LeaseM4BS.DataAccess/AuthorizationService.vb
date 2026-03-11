Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Npgsql

''' <summary>認証・権限チェックサービス（シングルトン）</summary>
Public Class AuthorizationService

    Private Shared ReadOnly _instance As New AuthorizationService()
    Public Shared ReadOnly Property Current As AuthorizationService
        Get
            Return _instance
        End Get
    End Property

    Private _currentUser As UserInfo = Nothing
    Public ReadOnly Property CurrentUser As UserInfo
        Get
            Return _currentUser
        End Get
    End Property

    ''' <summary>メニュー権限マップ（キー = FrmFlexMenu のボタン Name プロパティ）</summary>
    ''' <remarks>将来的に抽象的な権限名に置き換え、UI層でマッピングする設計も検討</remarks>
    Private Shared ReadOnly _menuPermissions As New Dictionary(Of String, String()) From {
        {"btnContract",          New String() {"admin", "accounting", "general_affairs", "viewer"}},
        {"btnROUAsset",          New String() {"admin", "accounting", "general_affairs", "viewer"}},
        {"btnMonthlyPayments",   New String() {"admin", "accounting", "viewer"}},
        {"btnMonthlyAccounting", New String() {"admin", "accounting", "viewer"}},
        {"btnPeriodBalance",     New String() {"admin", "accounting", "viewer"}},
        {"btnTaxAdjustment",     New String() {"admin", "accounting", "viewer"}},
        {"btnMaster",            New String() {"admin"}}
    }

    Private Sub New()
    End Sub

    Public Function Authenticate(loginId As String, password As String) As AuthResult
        Dim sql As String = "SELECT user_id, login_id, user_name, role, is_active, password_hash " &
                            "FROM tm_USER WHERE login_id = @login_id"
        Dim params As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@login_id", loginId)
        }

        Using crud As New CrudHelper()
            Dim dt As DataTable = crud.GetDataTable(sql, params)

            If dt.Rows.Count = 0 Then
                Return AuthResult.UserNotFound
            End If

            Dim row As DataRow = dt.Rows(0)
            Dim storedHash As String = CStr(row("password_hash"))
            If Not PasswordHasher.Verify(password, storedHash) Then
                crud.ExecuteNonQuery(
                    "UPDATE tm_USER SET failed_login_count = failed_login_count + 1 " &
                    "WHERE login_id = @login_id",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@login_id", loginId)
                    })
                Return AuthResult.InvalidPassword
            End If

            Dim isActive As Boolean = CType(row("is_active"), Boolean)
            If Not isActive Then
                Return AuthResult.AccountDisabled
            End If

            crud.ExecuteNonQuery(
                "UPDATE tm_USER SET last_login_at = @last_login_at, failed_login_count = 0 " &
                "WHERE login_id = @login_id",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@last_login_at", DateTime.Now),
                    New NpgsqlParameter("@login_id", loginId)
                })

            _currentUser = New UserInfo With {
                .UserId   = CInt(row("user_id")),
                .LoginId  = CStr(row("login_id")),
                .UserName = CStr(row("user_name")),
                .Role     = CStr(row("role")),
                .IsActive = isActive
            }
            Return AuthResult.Success
        End Using
    End Function

    Public Function HasAccess(menuId As String) As Boolean
        If _currentUser Is Nothing Then Return False
        If Not _menuPermissions.ContainsKey(menuId) Then Return False
        Return Array.IndexOf(_menuPermissions(menuId), _currentUser.Role) >= 0
    End Function

    Public Sub Logout()
        _currentUser = Nothing
    End Sub

End Class
