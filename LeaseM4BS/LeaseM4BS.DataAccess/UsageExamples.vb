Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Npgsql
Imports LeaseM4BS.DataAccess

''' <summary>
''' DbConnectionManager の使用例を示すクラス
''' DAO コードから Npgsql への移行パターンを実装しています
''' </summary>
Public Class UsageExamples

    ''' <summary>
    ''' 例1: 基本的なデータ取得（SELECT）
    ''' DAO.Recordset の代替
    ''' </summary>
    Public Sub Example1_BasicSelect()
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT USER_ID, USER_NAME, USER_KANA FROM tw_M_USER ORDER BY USER_ID"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    Using reader As NpgsqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim userId As Integer = Convert.ToInt32(reader("USER_ID"))
                            Dim userName As String = reader("USER_NAME").ToString()
                            Dim userKana As String = reader("USER_KANA").ToString()
                            
                            Console.WriteLine($"ID: {userId}, 名前: {userName}, カナ: {userKana}")
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' 例2: パラメータ付きクエリ
    ''' SQL インジェクション対策
    ''' </summary>
    Public Function Example2_ParameterizedQuery(userId As Integer) As String
        Dim connMgr As New DbConnectionManager()
        Dim userName As String = String.Empty

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT USER_NAME FROM tw_M_USER WHERE USER_ID = @userId"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@userId", userId)
                    
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        userName = result.ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return userName
    End Function

    ''' <summary>
    ''' 例3: データの挿入（INSERT）
    ''' DAO.Recordset.AddNew の代替
    ''' </summary>
    Public Function Example3_InsertData(userName As String, userKana As String) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "INSERT INTO tw_M_USER (USER_NAME, USER_KANA, CREATE_DATE) VALUES (@userName, @userKana, @createDate)"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@userName", userName)
                    cmd.Parameters.AddWithValue("@userKana", userKana)
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now)
                    
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    Return rowsAffected > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ挿入エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 例4: データの更新（UPDATE）
    ''' DAO.Recordset.Edit / Update の代替
    ''' </summary>
    Public Function Example4_UpdateData(userId As Integer, userName As String) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "UPDATE tw_M_USER SET USER_NAME = @userName, UPDATE_DATE = @updateDate WHERE USER_ID = @userId"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@userName", userName)
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now)
                    cmd.Parameters.AddWithValue("@userId", userId)
                    
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    Return rowsAffected > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ更新エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 例5: データの削除（DELETE）
    ''' DAO.Recordset.Delete の代替
    ''' </summary>
    Public Function Example5_DeleteData(userId As Integer) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "DELETE FROM tw_M_USER WHERE USER_ID = @userId"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@userId", userId)
                    
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    Return rowsAffected > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ削除エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 例6: トランザクション処理
    ''' DAO.Workspace.BeginTrans / CommitTrans / Rollback の代替
    ''' </summary>
    Public Function Example6_Transaction(user1Name As String, user2Name As String) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Using transaction As NpgsqlTransaction = conn.BeginTransaction()
                    Try
                        ' 1つ目の挿入
                        Using cmd1 As New NpgsqlCommand("INSERT INTO tw_M_USER (USER_NAME, CREATE_DATE) VALUES (@userName, @createDate)", conn, transaction)
                            cmd1.Parameters.AddWithValue("@userName", user1Name)
                            cmd1.Parameters.AddWithValue("@createDate", DateTime.Now)
                            cmd1.ExecuteNonQuery()
                        End Using

                        ' 2つ目の挿入
                        Using cmd2 As New NpgsqlCommand("INSERT INTO tw_M_USER (USER_NAME, CREATE_DATE) VALUES (@userName, @createDate)", conn, transaction)
                            cmd2.Parameters.AddWithValue("@userName", user2Name)
                            cmd2.Parameters.AddWithValue("@createDate", DateTime.Now)
                            cmd2.ExecuteNonQuery()
                        End Using

                        ' コミット
                        transaction.Commit()
                        Return True

                    Catch ex As Exception
                        ' ロールバック
                        transaction.Rollback()
                        MessageBox.Show($"トランザクションエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "トランザクションエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 例7: DataTable への読み込み
    ''' DataGridView へのバインディング用
    ''' </summary>
    Public Function Example7_LoadDataTable() As DataTable
        Dim connMgr As New DbConnectionManager()
        Dim dataTable As New DataTable()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT USER_ID, USER_NAME, USER_KANA, CREATE_DATE FROM tw_M_USER ORDER BY USER_ID"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    Using adapter As New NpgsqlDataAdapter(cmd)
                        adapter.Fill(dataTable)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "データ読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' 例8: DataGridView へのバインディング
    ''' </summary>
    Public Sub Example8_BindToDataGridView(dgv As DataGridView)
        Dim dataTable As DataTable = Example7_LoadDataTable()
        dgv.DataSource = dataTable
    End Sub

    ''' <summary>
    ''' 例9: レコード数の取得
    ''' DAO.Recordset.RecordCount の代替
    ''' </summary>
    Public Function Example9_GetRecordCount() As Integer
        Dim connMgr As New DbConnectionManager()
        Dim count As Integer = 0

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT COUNT(*) FROM tw_M_USER"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        count = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "カウント取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return count
    End Function

    ''' <summary>
    ''' 例10: 条件付きレコード数の取得
    ''' </summary>
    Public Function Example10_GetRecordCountWithCondition(searchName As String) As Integer
        Dim connMgr As New DbConnectionManager()
        Dim count As Integer = 0

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT COUNT(*) FROM tw_M_USER WHERE USER_NAME LIKE @searchName"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@searchName", $"%{searchName}%")
                    
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        count = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "カウント取得エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return count
    End Function

    ''' <summary>
    ''' 例11: レコードの存在チェック
    ''' DAO.Recordset.EOF の代替
    ''' </summary>
    Public Function Example11_RecordExists(userId As Integer) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT COUNT(*) FROM tw_M_USER WHERE USER_ID = @userId"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@userId", userId)
                    
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result) > 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "存在チェックエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return False
    End Function

    ''' <summary>
    ''' 例12: 複数テーブルの JOIN クエリ
    ''' </summary>
    Public Function Example12_JoinQuery() As DataTable
        Dim connMgr As New DbConnectionManager()
        Dim dataTable As New DataTable()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Dim sql As String = "SELECT u.USER_ID, u.USER_NAME, d.DEPT_NAME " &
                                   "FROM tw_M_USER u " &
                                   "INNER JOIN tw_M_DEPT d ON u.DEPT_ID = d.DEPT_ID " &
                                   "ORDER BY u.USER_ID"
                
                Using cmd As New NpgsqlCommand(sql, conn)
                    Using adapter As New NpgsqlDataAdapter(cmd)
                        adapter.Fill(dataTable)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "JOIN クエリエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' 例13: ストアドプロシージャの呼び出し
    ''' </summary>
    Public Function Example13_CallStoredProcedure(userId As Integer) As DataTable
        Dim connMgr As New DbConnectionManager()
        Dim dataTable As New DataTable()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Using cmd As New NpgsqlCommand("get_user_details", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("p_user_id", userId)
                    
                    Using adapter As New NpgsqlDataAdapter(cmd)
                        adapter.Fill(dataTable)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "ストアドプロシージャエラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' 例14: バッチ挿入（複数レコードの一括挿入）
    ''' </summary>
    Public Function Example14_BatchInsert(userNames As List(Of String)) As Boolean
        Dim connMgr As New DbConnectionManager()

        Try
            Using conn As NpgsqlConnection = connMgr.GetConnection()
                Using transaction As NpgsqlTransaction = conn.BeginTransaction()
                    Try
                        For Each userName As String In userNames
                            Using cmd As New NpgsqlCommand("INSERT INTO tw_M_USER (USER_NAME, CREATE_DATE) VALUES (@userName, @createDate)", conn, transaction)
                                cmd.Parameters.AddWithValue("@userName", userName)
                                cmd.Parameters.AddWithValue("@createDate", DateTime.Now)
                                cmd.ExecuteNonQuery()
                            End Using
                        Next

                        transaction.Commit()
                        Return True

                    Catch ex As Exception
                        transaction.Rollback()
                        MessageBox.Show($"バッチ挿入エラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End Try
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show($"エラー: {ex.Message}", "バッチ挿入エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 例15: 接続テスト
    ''' </summary>
    Public Sub Example15_TestConnection()
        Dim connMgr As New DbConnectionManager()
        Dim errorMessage As String = String.Empty

        If connMgr.TestConnection(errorMessage) Then
            MessageBox.Show("データベース接続に成功しました。", "接続テスト", MessageBoxButtons.OK, MessageBoxIcon.Information)
            
            ' 接続文字列の表示（パスワードはマスクされます）
            Dim maskedConnStr As String = connMgr.GetMaskedConnectionString()
            Console.WriteLine($"接続文字列: {maskedConnStr}")
        Else
            MessageBox.Show($"データベース接続に失敗しました: {errorMessage}", "接続テスト", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

End Class
