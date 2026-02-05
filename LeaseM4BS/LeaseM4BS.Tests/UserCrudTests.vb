Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Data
Imports Npgsql
Imports LeaseM4BS.DataAccess

''' <summary>
''' tw_m_user テーブルに対する CRUD 操作のテストクラス
''' Form1.vb の btnCrudTest_Click メソッドのロジックを MSTest に移植
''' </summary>
<TestClass>
Public Class UserCrudTests

    Private Const TestUserId As Integer = 99999
    Private Const TestUserName As String = "テストユーザー"
    Private Const UpdatedUserName As String = "更新後ユーザー"

    ''' <summary>
    ''' 各テストの前にテストデータをクリーンアップ
    ''' </summary>
    <TestInitialize>
    Public Sub TestInitialize()
        Using helper As New CrudHelper()
            Try
                helper.Delete("tw_m_user", "user_id = @userId", New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@userId", TestUserId)
                })
            Catch
                ' テストデータが存在しない場合は無視
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' 各テストの後にテストデータをクリーンアップ
    ''' </summary>
    <TestCleanup>
    Public Sub TestCleanup()
        Using helper As New CrudHelper()
            Try
                helper.Delete("tw_m_user", "user_id = @userId", New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@userId", TestUserId)
                })
            Catch
                ' テストデータが存在しない場合は無視
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' INSERT操作のテスト - レコードを挿入できることを確認
    ''' </summary>
    <TestMethod>
    Public Sub Insert_ShouldInsertRecord()
        Using helper As New CrudHelper()
            ' Arrange
            Dim insertValues As New Dictionary(Of String, Object) From {
                {"user_id", TestUserId},
                {"user_name", TestUserName},
                {"user_kana", "テストユーザー"},
                {"create_date", DateTime.Now},
                {"update_date", DateTime.Now}
            }

            ' Act
            Dim insertedRows As Integer = helper.Insert("tw_m_user", insertValues)

            ' Assert
            Assert.AreEqual(1, insertedRows)
        End Using
    End Sub

    ''' <summary>
    ''' SELECT操作のテスト - 挿入したレコードを取得できることを確認
    ''' </summary>
    <TestMethod>
    Public Sub Select_ShouldRetrieveInsertedRecord()
        Using helper As New CrudHelper()
            ' Arrange - まずレコードを挿入
            Dim insertValues As New Dictionary(Of String, Object) From {
                {"user_id", TestUserId},
                {"user_name", TestUserName},
                {"user_kana", "テストユーザー"},
                {"create_date", DateTime.Now},
                {"update_date", DateTime.Now}
            }
            helper.Insert("tw_m_user", insertValues)

            ' Act
            Dim selectParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim dt As DataTable = helper.GetDataTable("SELECT user_id, user_name, user_kana FROM tw_m_user WHERE user_id = @userId", selectParams)

            ' Assert
            Assert.AreEqual(1, dt.Rows.Count)
            Assert.AreEqual(TestUserId, Convert.ToInt32(dt.Rows(0)("user_id")))
            Assert.AreEqual(TestUserName, dt.Rows(0)("user_name").ToString())
            Assert.AreEqual("テストユーザー", dt.Rows(0)("user_kana").ToString())
        End Using
    End Sub

    ''' <summary>
    ''' UPDATE操作のテスト - レコードを更新できることを確認
    ''' </summary>
    <TestMethod>
    Public Sub Update_ShouldUpdateRecord()
        Using helper As New CrudHelper()
            ' Arrange - まずレコードを挿入
            Dim insertValues As New Dictionary(Of String, Object) From {
                {"user_id", TestUserId},
                {"user_name", TestUserName},
                {"user_kana", "テストユーザー"},
                {"create_date", DateTime.Now},
                {"update_date", DateTime.Now}
            }
            helper.Insert("tw_m_user", insertValues)

            ' Act
            Dim updateValues As New Dictionary(Of String, Object) From {
                {"user_name", UpdatedUserName},
                {"update_date", DateTime.Now}
            }
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim updatedRows As Integer = helper.Update("tw_m_user", updateValues, "user_id = @userId", whereParams)

            ' Assert
            Assert.AreEqual(1, updatedRows)

            ' 更新後の値を確認
            Dim selectParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim dt As DataTable = helper.GetDataTable("SELECT user_name FROM tw_m_user WHERE user_id = @userId", selectParams)
            Assert.AreEqual(1, dt.Rows.Count)
            Assert.AreEqual(UpdatedUserName, dt.Rows(0)("user_name").ToString())
        End Using
    End Sub

    ''' <summary>
    ''' DELETE操作のテスト - レコードを削除できることを確認
    ''' </summary>
    <TestMethod>
    Public Sub Delete_ShouldDeleteRecord()
        Using helper As New CrudHelper()
            ' Arrange - まずレコードを挿入
            Dim insertValues As New Dictionary(Of String, Object) From {
                {"user_id", TestUserId},
                {"user_name", TestUserName},
                {"user_kana", "テストユーザー"},
                {"create_date", DateTime.Now},
                {"update_date", DateTime.Now}
            }
            helper.Insert("tw_m_user", insertValues)

            ' Act
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim deletedRows As Integer = helper.Delete("tw_m_user", "user_id = @userId", whereParams)

            ' Assert
            Assert.AreEqual(1, deletedRows)

            ' 削除後にレコードが存在しないことを確認
            Dim selectParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim dt As DataTable = helper.GetDataTable("SELECT user_id FROM tw_m_user WHERE user_id = @userId", selectParams)
            Assert.AreEqual(0, dt.Rows.Count)
        End Using
    End Sub

    ''' <summary>
    ''' 完全なCRUDサイクルのテスト - INSERT, SELECT, UPDATE, DELETE の一連の操作を確認
    ''' </summary>
    <TestMethod>
    Public Sub FullCrudCycle_ShouldCompleteSuccessfully()
        Using helper As New CrudHelper()
            ' Step 1: INSERT
            Dim insertValues As New Dictionary(Of String, Object) From {
                {"user_id", TestUserId},
                {"user_name", TestUserName},
                {"user_kana", "テストユーザー"},
                {"create_date", DateTime.Now},
                {"update_date", DateTime.Now}
            }
            Dim insertedRows As Integer = helper.Insert("tw_m_user", insertValues)
            Assert.AreEqual(1, insertedRows, "INSERT should affect 1 row")

            ' Step 2: SELECT - 挿入確認
            Dim selectParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim dt As DataTable = helper.GetDataTable("SELECT user_id, user_name, user_kana FROM tw_m_user WHERE user_id = @userId", selectParams)
            Assert.AreEqual(1, dt.Rows.Count, "SELECT should return 1 row after INSERT")
            Assert.AreEqual(TestUserName, dt.Rows(0)("user_name").ToString())

            ' Step 3: UPDATE
            Dim updateValues As New Dictionary(Of String, Object) From {
                {"user_name", UpdatedUserName},
                {"update_date", DateTime.Now}
            }
            Dim whereParams As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@userId", TestUserId)
            }
            Dim updatedRows As Integer = helper.Update("tw_m_user", updateValues, "user_id = @userId", whereParams)
            Assert.AreEqual(1, updatedRows, "UPDATE should affect 1 row")

            ' Step 4: SELECT - 更新確認
            dt = helper.GetDataTable("SELECT user_id, user_name, user_kana FROM tw_m_user WHERE user_id = @userId", selectParams)
            Assert.AreEqual(1, dt.Rows.Count, "SELECT should return 1 row after UPDATE")
            Assert.AreEqual(UpdatedUserName, dt.Rows(0)("user_name").ToString(), "user_name should be updated")

            ' Step 5: DELETE
            Dim deletedRows As Integer = helper.Delete("tw_m_user", "user_id = @userId", whereParams)
            Assert.AreEqual(1, deletedRows, "DELETE should affect 1 row")

            ' Step 6: SELECT - 削除確認
            dt = helper.GetDataTable("SELECT user_id, user_name FROM tw_m_user WHERE user_id = @userId", selectParams)
            Assert.AreEqual(0, dt.Rows.Count, "SELECT should return 0 rows after DELETE")
        End Using
    End Sub

End Class
