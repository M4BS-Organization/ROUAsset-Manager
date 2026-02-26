Imports System.Runtime.Remoting.Metadata.W3cXsd2001
Imports LeaseM4BS.DataAccess
Imports Npgsql

Public Class Form_f_M_SHHO_CHANGE
    Public Property ShhoId As Double = 0
    Private _crud As crudHelper = New crudHelper()

    Private Sub Form_f_M_SHHO_CHANGE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' --- ヘッダ取得 (ID指定) ---
            Dim sqlHead As String = "SELECT * FROM m_shho WHERE shho_id = @id"

            Dim prmHead As New List(Of Npgsql.NpgsqlParameter) From {
                New Npgsql.NpgsqlParameter("@id", ShhoId)
            }

            Dim dtHead As DataTable = _crud.GetDataTable(sqlHead, prmHead)

            If dtHead.Rows.Count > 0 Then
                Dim row As DataRow = dtHead.Rows(0)

                ' 画面項目にセット（以前のコードと同じロジック）
                txt_SHHO_CD.Text = row("shho_cd").ToString()
                txt_SHHO_NM.Text = row("shho_nm").ToString()
                txt_BIKO.Text = row("biko").ToString()
                txt_CREATE_DT.Text = row("create_dt").ToString()
                txt_UPDATE_DT.Text = row("update_dt").ToString()
                txt_SHHO_ID.Text = row("shho_id").ToString()
            End If
        Catch ex As Exception
            MessageBox.Show("詳細読込エラー: " & ex.Message)
        End Try
    End Sub

    ' [閉じる]ボタン
    Private Sub cmd_CLOSE_Click(sender As Object, e As EventArgs) Handles cmd_CLOSE.Click
        Me.Close()
    End Sub

    ' [変更登録] ボタン
    Private Sub cmd_CREATE_Click(sender As Object, e As EventArgs) Handles cmd_CREATE.Click
        ' 必須項目が未入力
        If txt_SHHO_CD.Text = "" Or txt_SHHO_NM.Text = "" Then
            MessageBox.Show("必須項目が未入力です", "登録不可", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If
        If MessageBox.Show("登録してもよろしいですか？", "登録確認", MessageBoxButtons.YesNo) = DialogResult.No Then
            Return
        End If

        Dim shho As New Dictionary(Of String, Object)
        shho("shho_cd") = txt_SHHO_CD.Text
        shho("shho_nm") = txt_SHHO_NM.Text
        shho("biko") = txt_BIKO.Text
        shho("update_dt") = DateTime.Now

        Dim currentCnt As Integer = _crud.ExecuteScalar(Of Integer)("SELECT update_cnt FROM m_shho WHERE shho_id = @id",
                                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", CInt(txt_SHHO_ID.Text))})
        shho("update_cnt") = currentCnt + 1

        ' パラメータ設定
        Dim prms As New List(Of NpgsqlParameter)
        prms.Add(New NpgsqlParameter("@id", Integer.Parse(txt_SHHO_ID.Text)))

        ' 行を更新
        _crud.Update("m_shho", shho, "shho_id = @id", prms)

        Me.Close()
    End Sub

    ' [削除] ボタン
    Private Sub cmd_DELETE_Click(sender As Object, e As EventArgs) Handles cmd_DELETE.Click
        If String.IsNullOrWhiteSpace(txt_SHHO_ID.Text) Then
            MessageBox.Show("削除対象が選択されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("削除してもよろしいですか？", "削除確認", MessageBoxButtons.YesNo) = DialogResult.No Then
            Return
        End If

        ' パラメータ設定
        Dim prms As New List(Of NpgsqlParameter)
        prms.Add(New NpgsqlParameter("@id", Integer.Parse(txt_SHHO_ID.Text)))

        ' 行を削除
        _crud.Delete("m_shho", "shho_id = @id", prms)

        Me.Close()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' エンターキーが押されたら次のコントロールへ移動
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class