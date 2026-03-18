Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

' =========================================================
' システム利用権限 入力画面
' Access版 Form_f_SEC_KNGN_INP + pc_f_SEC_KNGN_INP 相当
' =========================================================
Partial Public Class Form_f_SEC_KNGN_INP
    Inherits Form

    Private ReadOnly _crud As New CrudHelper()

    Public Property EditMode As String = "NEW"
    Public Property TargetKngnId As Integer = 0

    ' 契約管理単位の選択済みリスト (kknri_id, access_kind)
    Private _selectedKknriList As New List(Of KknriEntry)()
    ' 部門管理単位の選択済みリスト (bknri_id, access_kind)
    Private _selectedBknriList As New List(Of BknriEntry)()

    Private Structure KknriEntry
        Public KknriId As Integer
        Public KknriCd As String
        Public KknriNm As String
        Public AccessKind As Integer  ' 1=変更, 2=参照
    End Structure

    Private Structure BknriEntry
        Public BknriId As Integer
        Public BknriCd As String
        Public BknriNm As String
        Public AccessKind As Integer
    End Structure

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_SEC_KNGN_INP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_Mode.Visible = False
        txt_ID.Visible = False
        txt_CREATE_DT.ReadOnly = True
        txt_UPDATE_DT.ReadOnly = True

        txt_Mode.Text = EditMode

        If EditMode = "EDIT" Then
            LoadKngnData(TargetKngnId)
            txt_CD.ReadOnly = True
        Else
            ' 新規: デフォルト
            オプション16.Checked = True  ' ACCESS_KIND=1(全データ変更)
            オプション53.Checked = True  ' ACCESS_KIND_B=1(全データ変更)
        End If

        ' 候補リストをロード
        LoadKknriKoho()
        LoadBknriKoho()

        ' ACCESS_KINDに応じたUI制御
        UpdateKknriAreaEnabled()
        UpdateBknriAreaEnabled()
    End Sub

    ''' <summary>
    ''' 既存権限グループデータを読み込む
    ''' </summary>
    Private Sub LoadKngnData(kngnId As Integer)
        Try
            Dim sql As String = "SELECT * FROM sec_kngn WHERE kngn_id = @kngn_id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kngn_id", kngnId)
            }
            Dim dt = _crud.GetDataTable(sql, prms)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("権限グループが見つかりません。", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            Dim row = dt.Rows(0)
            txt_ID.Text = row("kngn_id").ToString()
            txt_CD.Text = If(row("kngn_cd") IsNot DBNull.Value, row("kngn_cd").ToString(), "")
            txt_NM.Text = If(row("kngn_nm") IsNot DBNull.Value, row("kngn_nm").ToString(), "")
            txt_BIKO.Text = If(row("biko") IsNot DBNull.Value, row("biko").ToString(), "")

            ' ACCESS_KIND (契約管理単位用)
            Dim ak As Integer = If(row("access_kind") IsNot DBNull.Value, CInt(row("access_kind")), 1)
            Select Case ak
                Case 1 : オプション16.Checked = True
                Case 2 : オプション18.Checked = True
                Case 3 : オプション23.Checked = True
            End Select

            ' ACCESS_KIND_B (物件管理単位用)
            Dim akb As Integer = If(row("access_kind_b") IsNot DBNull.Value, CInt(row("access_kind_b")), 1)
            Select Case akb
                Case 1 : オプション53.Checked = True
                Case 2 : オプション55.Checked = True
                Case 3 : オプション57.Checked = True
            End Select

            ' 権限チェックボックス
            chk_Admin.Checked = If(row("admin") IsNot DBNull.Value, CBool(row("admin")), False)
            chk_Master_Update.Checked = If(row("master_update") IsNot DBNull.Value, CBool(row("master_update")), False)
            chk_Approval.Checked = If(row("approval") IsNot DBNull.Value, CBool(row("approval")), False)
            chk_File_Output.Checked = If(row("file_output") IsNot DBNull.Value, CBool(row("file_output")), False)
            chk_Print.Checked = If(row("print") IsNot DBNull.Value, CBool(row("print")), False)
            chk_Log_Ref.Checked = If(row("log_ref") IsNot DBNull.Value, CBool(row("log_ref")), False)

            ' 監査情報
            txt_CREATE_DT.Text = If(row("create_dt") IsNot DBNull.Value, CDate(row("create_dt")).ToString("yyyy/MM/dd HH:mm:ss"), "")
            txt_UPDATE_DT.Text = If(row("update_dt") IsNot DBNull.Value, CDate(row("update_dt")).ToString("yyyy/MM/dd HH:mm:ss"), "")

            ' 選択済み管理単位を読み込む
            LoadSelectedKknri(kngnId)
            LoadSelectedBknri(kngnId)

        Catch ex As Exception
            MessageBox.Show("データ読込エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================
    '  契約管理単位
    ' =========================================================
    Private Sub LoadKknriKoho()
        Try
            Dim sql As String = "SELECT kknri_id, kknri1_cd, kknri1_nm FROM m_kknri ORDER BY kknri1_cd"
            Dim dt = _crud.GetDataTable(sql)
            lst_KKNRI_ID_KOHO.Items.Clear()
            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("kknri_id"))
                ' 既に選択済みのものは候補から除外
                If Not _selectedKknriList.Exists(Function(x) x.KknriId = id) Then
                    lst_KKNRI_ID_KOHO.Items.Add(New ListItem(id, $"{row("kknri1_cd")} - {row("kknri1_nm")}"))
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadSelectedKknri(kngnId As Integer)
        _selectedKknriList.Clear()
        Try
            Dim sql As String =
                "SELECT kk.kknri_id, kk.access_kind, m.kknri1_cd, m.kknri1_nm " &
                "FROM sec_kngn_kknri kk " &
                "JOIN m_kknri m ON kk.kknri_id = m.kknri_id " &
                "WHERE kk.kngn_id = @kngn_id ORDER BY m.kknri1_cd"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kngn_id", kngnId)
            }
            Dim dt = _crud.GetDataTable(sql, prms)
            For Each row As DataRow In dt.Rows
                Dim entry As New KknriEntry()
                entry.KknriId = CInt(row("kknri_id"))
                entry.KknriCd = row("kknri1_cd").ToString()
                entry.KknriNm = row("kknri1_nm").ToString()
                entry.AccessKind = CInt(row("access_kind"))
                _selectedKknriList.Add(entry)
            Next
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' 契約管理単位 追加ボタン (＞)
    ''' </summary>
    Private Sub cmd_Add_KKNRI_Click(sender As Object, e As EventArgs) Handles cmd_Add_KKNRI.Click
        If lst_KKNRI_ID_KOHO.SelectedItem Is Nothing Then Return
        Dim item = DirectCast(lst_KKNRI_ID_KOHO.SelectedItem, ListItem)

        ' サブフォームで権限種別を選択
        Dim subForm As New Form_f_SEC_KNGN_INP_SUB()
        subForm.KknriId = item.Id
        subForm.DisplayText = item.DisplayText
        If subForm.ShowDialog() = DialogResult.OK Then
            Dim entry As New KknriEntry()
            entry.KknriId = item.Id
            entry.KknriCd = item.DisplayText.Split({" - "}, StringSplitOptions.None)(0)
            entry.KknriNm = If(item.DisplayText.Contains(" - "), item.DisplayText.Substring(item.DisplayText.IndexOf(" - ") + 3), "")
            entry.AccessKind = subForm.SelectedAccessKind
            _selectedKknriList.Add(entry)
            LoadKknriKoho()  ' 候補を更新
        End If
    End Sub

    ''' <summary>
    ''' 契約管理単位 削除ボタン (＜)
    ''' </summary>
    Private Sub cmd_Del_KKNRI_Click(sender As Object, e As EventArgs) Handles cmd_Del_KKNRI.Click
        ' 最後に追加したものを削除（簡易実装）
        If _selectedKknriList.Count > 0 Then
            _selectedKknriList.RemoveAt(_selectedKknriList.Count - 1)
            LoadKknriKoho()
        End If
    End Sub

    ' =========================================================
    '  部門管理単位 (BKNRI) — 同構造
    ' =========================================================
    Private Sub LoadBknriKoho()
        Try
            Dim sql As String = "SELECT bknri_id, bknri1_cd, bknri1_nm FROM m_bknri ORDER BY bknri1_cd"
            Dim dt = _crud.GetDataTable(sql)
            lst_BKNRI_ID_KOHO.Items.Clear()
            For Each row As DataRow In dt.Rows
                Dim id As Integer = CInt(row("bknri_id"))
                If Not _selectedBknriList.Exists(Function(x) x.BknriId = id) Then
                    lst_BKNRI_ID_KOHO.Items.Add(New ListItem(id, $"{row("bknri1_cd")} - {row("bknri1_nm")}"))
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadSelectedBknri(kngnId As Integer)
        _selectedBknriList.Clear()
        Try
            Dim sql As String =
                "SELECT bk.bknri_id, bk.access_kind, m.bknri1_cd, m.bknri1_nm " &
                "FROM sec_kngn_bknri bk " &
                "JOIN m_bknri m ON bk.bknri_id = m.bknri_id " &
                "WHERE bk.kngn_id = @kngn_id ORDER BY m.bknri1_cd"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@kngn_id", kngnId)
            }
            Dim dt = _crud.GetDataTable(sql, prms)
            For Each row As DataRow In dt.Rows
                Dim entry As New BknriEntry()
                entry.BknriId = CInt(row("bknri_id"))
                entry.BknriCd = row("bknri1_cd").ToString()
                entry.BknriNm = row("bknri1_nm").ToString()
                entry.AccessKind = CInt(row("access_kind"))
                _selectedBknriList.Add(entry)
            Next
        Catch
        End Try
    End Sub

    Private Sub cmd_Add_BKNRI_Click(sender As Object, e As EventArgs) Handles cmd_Add_BKNRI.Click
        If lst_BKNRI_ID_KOHO.SelectedItem Is Nothing Then Return
        Dim item = DirectCast(lst_BKNRI_ID_KOHO.SelectedItem, ListItem)

        Dim subForm As New Form_f_SEC_KNGN_INP_B_SUB()
        subForm.BknriId = item.Id
        subForm.DisplayText = item.DisplayText
        If subForm.ShowDialog() = DialogResult.OK Then
            Dim entry As New BknriEntry()
            entry.BknriId = item.Id
            entry.BknriCd = item.DisplayText.Split({" - "}, StringSplitOptions.None)(0)
            entry.BknriNm = If(item.DisplayText.Contains(" - "), item.DisplayText.Substring(item.DisplayText.IndexOf(" - ") + 3), "")
            entry.AccessKind = subForm.SelectedAccessKind
            _selectedBknriList.Add(entry)
            LoadBknriKoho()
        End If
    End Sub

    Private Sub cmd_Del_BKNRI_Click(sender As Object, e As EventArgs) Handles cmd_Del_BKNRI.Click
        If _selectedBknriList.Count > 0 Then
            _selectedBknriList.RemoveAt(_selectedBknriList.Count - 1)
            LoadBknriKoho()
        End If
    End Sub

    ' =========================================================
    '  ACCESS_KIND RadioButton変更時のUI制御
    ' =========================================================
    Private Sub オプション16_CheckedChanged(sender As Object, e As EventArgs) Handles オプション16.CheckedChanged
        UpdateKknriAreaEnabled()
    End Sub
    Private Sub オプション18_CheckedChanged(sender As Object, e As EventArgs) Handles オプション18.CheckedChanged
        UpdateKknriAreaEnabled()
    End Sub
    Private Sub オプション23_CheckedChanged(sender As Object, e As EventArgs) Handles オプション23.CheckedChanged
        UpdateKknriAreaEnabled()
    End Sub
    Private Sub オプション53_CheckedChanged(sender As Object, e As EventArgs) Handles オプション53.CheckedChanged
        UpdateBknriAreaEnabled()
    End Sub
    Private Sub オプション55_CheckedChanged(sender As Object, e As EventArgs) Handles オプション55.CheckedChanged
        UpdateBknriAreaEnabled()
    End Sub
    Private Sub オプション57_CheckedChanged(sender As Object, e As EventArgs) Handles オプション57.CheckedChanged
        UpdateBknriAreaEnabled()
    End Sub

    Private Sub UpdateKknriAreaEnabled()
        Dim enabled As Boolean = オプション23.Checked  ' ACCESS_KIND=3 のとき有効
        lst_KKNRI_ID_KOHO.Enabled = enabled
        cmd_Add_KKNRI.Enabled = enabled
        cmd_Del_KKNRI.Enabled = enabled
    End Sub

    Private Sub UpdateBknriAreaEnabled()
        Dim enabled As Boolean = オプション57.Checked  ' ACCESS_KIND_B=3 のとき有効
        lst_BKNRI_ID_KOHO.Enabled = enabled
        cmd_Add_BKNRI.Enabled = enabled
        cmd_Del_BKNRI.Enabled = enabled
    End Sub

    ' =========================================================
    '  ACCESS_KIND値の取得
    ' =========================================================
    Private Function GetAccessKind() As Integer
        If オプション16.Checked Then Return 1
        If オプション18.Checked Then Return 2
        If オプション23.Checked Then Return 3
        Return 1
    End Function

    Private Function GetAccessKindB() As Integer
        If オプション53.Checked Then Return 1
        If オプション55.Checked Then Return 2
        If オプション57.Checked Then Return 3
        Return 1
    End Function

    ' =========================================================
    '  登録ボタン
    ' =========================================================
    Private Sub cmd_Touroku_Click(sender As Object, e As EventArgs) Handles cmd_Touroku.Click
        If String.IsNullOrWhiteSpace(txt_CD.Text) Then
            MessageBox.Show("権限コードを入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_CD.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(txt_NM.Text) Then
            MessageBox.Show("権限名を入力してください。", "入力エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txt_NM.Focus()
            Return
        End If

        Try
            ' トランザクション内で3テーブル同時更新
            Dim connStr = New DbConnectionManager().GetConnectionString()
            Using conn As New NpgsqlConnection(connStr)
                conn.Open()
                Using tran = conn.BeginTransaction()
                    Try
                        If EditMode = "NEW" Then
                            InsertKngn(conn, tran)
                        Else
                            UpdateKngn(conn, tran)
                        End If
                        tran.Commit()
                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            MessageBox.Show("登録しました。", "完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("登録エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InsertKngn(conn As NpgsqlConnection, tran As NpgsqlTransaction)
        ' sec_kngn INSERT
        Dim sql As String =
            "INSERT INTO sec_kngn (kngn_cd, kngn_nm, access_kind, access_kind_b, " &
            "admin, master_update, approval, file_output, print, log_ref, " &
            "biko, history_f, create_dt, update_dt) " &
            "VALUES (@cd, @nm, @ak, @akb, @admin, @mu, @appr, @fo, @pr, @lr, " &
            "@biko, FALSE, @now, @now) RETURNING kngn_id"

        Dim kngnId As Integer
        Using cmd As New NpgsqlCommand(sql, conn, tran)
            cmd.Parameters.AddWithValue("@cd", txt_CD.Text.Trim())
            cmd.Parameters.AddWithValue("@nm", txt_NM.Text.Trim())
            cmd.Parameters.AddWithValue("@ak", GetAccessKind())
            cmd.Parameters.AddWithValue("@akb", GetAccessKindB())
            cmd.Parameters.AddWithValue("@admin", chk_Admin.Checked)
            cmd.Parameters.AddWithValue("@mu", chk_Master_Update.Checked)
            cmd.Parameters.AddWithValue("@appr", chk_Approval.Checked)
            cmd.Parameters.AddWithValue("@fo", chk_File_Output.Checked)
            cmd.Parameters.AddWithValue("@pr", chk_Print.Checked)
            cmd.Parameters.AddWithValue("@lr", chk_Log_Ref.Checked)
            cmd.Parameters.AddWithValue("@biko", txt_BIKO.Text.Trim())
            cmd.Parameters.AddWithValue("@now", DateTime.Now)
            kngnId = CInt(cmd.ExecuteScalar())
        End Using

        ' sec_kngn_kknri / sec_kngn_bknri INSERT
        InsertKknriEntries(conn, tran, kngnId)
        InsertBknriEntries(conn, tran, kngnId)
    End Sub

    Private Sub UpdateKngn(conn As NpgsqlConnection, tran As NpgsqlTransaction)
        ' sec_kngn UPDATE
        Dim sql As String =
            "UPDATE sec_kngn SET kngn_nm = @nm, access_kind = @ak, access_kind_b = @akb, " &
            "admin = @admin, master_update = @mu, approval = @appr, " &
            "file_output = @fo, print = @pr, log_ref = @lr, " &
            "biko = @biko, update_dt = @now " &
            "WHERE kngn_id = @id"
        Using cmd As New NpgsqlCommand(sql, conn, tran)
            cmd.Parameters.AddWithValue("@nm", txt_NM.Text.Trim())
            cmd.Parameters.AddWithValue("@ak", GetAccessKind())
            cmd.Parameters.AddWithValue("@akb", GetAccessKindB())
            cmd.Parameters.AddWithValue("@admin", chk_Admin.Checked)
            cmd.Parameters.AddWithValue("@mu", chk_Master_Update.Checked)
            cmd.Parameters.AddWithValue("@appr", chk_Approval.Checked)
            cmd.Parameters.AddWithValue("@fo", chk_File_Output.Checked)
            cmd.Parameters.AddWithValue("@pr", chk_Print.Checked)
            cmd.Parameters.AddWithValue("@lr", chk_Log_Ref.Checked)
            cmd.Parameters.AddWithValue("@biko", txt_BIKO.Text.Trim())
            cmd.Parameters.AddWithValue("@now", DateTime.Now)
            cmd.Parameters.AddWithValue("@id", TargetKngnId)
            cmd.ExecuteNonQuery()
        End Using

        ' sec_kngn_kknri DELETE → INSERT (全件洗い替え)
        Using cmd As New NpgsqlCommand("DELETE FROM sec_kngn_kknri WHERE kngn_id = @id", conn, tran)
            cmd.Parameters.AddWithValue("@id", TargetKngnId)
            cmd.ExecuteNonQuery()
        End Using
        InsertKknriEntries(conn, tran, TargetKngnId)

        ' sec_kngn_bknri DELETE → INSERT
        Using cmd As New NpgsqlCommand("DELETE FROM sec_kngn_bknri WHERE kngn_id = @id", conn, tran)
            cmd.Parameters.AddWithValue("@id", TargetKngnId)
            cmd.ExecuteNonQuery()
        End Using
        InsertBknriEntries(conn, tran, TargetKngnId)
    End Sub

    Private Sub InsertKknriEntries(conn As NpgsqlConnection, tran As NpgsqlTransaction, kngnId As Integer)
        For Each entry In _selectedKknriList
            Dim sql As String = "INSERT INTO sec_kngn_kknri (kngn_id, kknri_id, access_kind) VALUES (@kngn_id, @kknri_id, @ak)"
            Using cmd As New NpgsqlCommand(sql, conn, tran)
                cmd.Parameters.AddWithValue("@kngn_id", kngnId)
                cmd.Parameters.AddWithValue("@kknri_id", entry.KknriId)
                cmd.Parameters.AddWithValue("@ak", entry.AccessKind)
                cmd.ExecuteNonQuery()
            End Using
        Next
    End Sub

    Private Sub InsertBknriEntries(conn As NpgsqlConnection, tran As NpgsqlTransaction, kngnId As Integer)
        For Each entry In _selectedBknriList
            Dim sql As String = "INSERT INTO sec_kngn_bknri (kngn_id, bknri_id, access_kind) VALUES (@kngn_id, @bknri_id, @ak)"
            Using cmd As New NpgsqlCommand(sql, conn, tran)
                cmd.Parameters.AddWithValue("@kngn_id", kngnId)
                cmd.Parameters.AddWithValue("@bknri_id", entry.BknriId)
                cmd.Parameters.AddWithValue("@ak", entry.AccessKind)
                cmd.ExecuteNonQuery()
            End Using
        Next
    End Sub

    ' =========================================================
    '  削除ボタン（論理削除）
    ' =========================================================
    Private Sub cmd_Del_Click(sender As Object, e As EventArgs) Handles cmd_Del.Click
        If EditMode <> "EDIT" Then Return

        ' ユーザーが割り当てられていないか確認
        Dim chkSql As String = "SELECT COUNT(*) FROM sec_user WHERE kngn_id = @id AND history_f = FALSE"
        Dim chkPrms As New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@id", TargetKngnId)
        }
        Dim userCount As Integer = _crud.ExecuteScalar(Of Integer)(chkSql, chkPrms)
        If userCount > 0 Then
            MessageBox.Show($"この権限グループには {userCount} 名のユーザーが割り当てられているため削除できません。", "制限",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result = MessageBox.Show("この権限グループを使用不可にしますか？", "確認",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result <> DialogResult.Yes Then Return

        Try
            Dim sql As String = "UPDATE sec_kngn SET history_f = TRUE, update_dt = @now WHERE kngn_id = @id"
            Dim prms As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@now", DateTime.Now),
                New NpgsqlParameter("@id", TargetKngnId)
            }
            _crud.ExecuteNonQuery(sql, prms)
            MessageBox.Show("使用不可に設定しました。", "完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("削除エラー: " & ex.Message, "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmd_Close_Click(sender As Object, e As EventArgs) Handles cmd_Close.Click
        Me.Close()
    End Sub

    ' =========================================================
    '  ListBox用ヘルパークラス
    ' =========================================================
    Private Class ListItem
        Public ReadOnly Property Id As Integer
        Public ReadOnly Property DisplayText As String

        Public Sub New(id As Integer, displayText As String)
            Me.Id = id
            Me.DisplayText = displayText
        End Sub

        Public Overrides Function ToString() As String
            Return DisplayText
        End Function
    End Class

End Class
