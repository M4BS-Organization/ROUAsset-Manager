Imports System.Data
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' リース債務返済明細表 仕訳出力（支払管理仕訳）
''' Access版 Form_f_仕訳出力標準_SM を完全再現
''' 12パターン: 資産(SSN1-6) + 費用(HIYO1-6)
''' 奇数パターン=長短振替(KEIJO1_DT)、偶数パターン=長短振替戻し(KEIJO2_DT)
''' </summary>
Partial Public Class Form_f_仕訳出力標準_SM
    Inherits Form

    ' ─── 定数 ───
    Private Const KJKBN_HIYO As Integer = 1     ' 計上区分=費用
    Private Const KJKBN_SISAN As Integer = 2    ' 計上区分=資産

    ' ─── フィールド ───
    Private _crud As New CrudHelper()
    Private _仕訳SEQNo As Integer
    Private _settingSM As DataRow    ' tw_f_仕訳出力標準_設定_SWKSM
    Private _settingKY As DataRow    ' tw_f_仕訳出力標準_設定_SWKKY

    ' 科目情報構造体 (Access版 cn_typ_mSWK 相当)
    Private Structure KamokuInfo
        Public KamokuNo As Integer
        Public KamokuCD As Object
        Public KamokuNM As Object
    End Structure

    ' 借方/貸方科目配列
    Private _tmDR() As KamokuInfo
    Private _tmCR() As KamokuInfo

    Public Sub New()
        InitializeComponent()
    End Sub

    ' ================================================================
    '  Form_Load (Access版 Form_Open 相当)
    ' ================================================================
    Private Sub Form_f_仕訳出力標準_SM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' リース債務返済明細表の集計条件チェック
            Dim dtJoken = _crud.GetDataTable("SELECT * FROM tw_s_saimu_joken LIMIT 1")
            If dtJoken.Rows.Count = 0 Then
                MessageBox.Show("リース債務返済明細表の集計条件が実行されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
            End If

            Dim jokenRow = dtJoken.Rows(0)
            If IsDBNull(jokenRow("kikan_from")) Then
                MessageBox.Show("リース債務返済明細表の集計条件が実行されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
            End If

            Dim kikanFrom As DateTime = CDate(jokenRow("kikan_from"))
            Dim kikanTo As DateTime = CDate(jokenRow("kikan_to"))

            ' 集計期間は1ヶ月以内
            If kikanFrom.ToString("yyyy/MM") <> kikanTo.ToString("yyyy/MM") Then
                MessageBox.Show("リース債務返済明細表の集計期間は、1ヶ月で指定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
            End If

            ' 適用日チェック
            Dim dtSettei = _crud.GetDataTable("SELECT val_datetime FROM t_settei WHERE settei_nm = 'TEKIYO_DT'")
            If dtSettei.Rows.Count > 0 AndAlso Not IsDBNull(dtSettei.Rows(0)(0)) Then
                Dim tekiyoDt As DateTime = CDate(dtSettei.Rows(0)(0))
                If kikanFrom.ToString("yyyyMM") < tekiyoDt.ToString("yyyyMM") Then
                    MessageBox.Show("集計条件が適用日以降でないと実行できません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Me.Close()
                    Return
                End If
            End If

            ' 計算対象・明細チェック
            If Not IsDBNull(jokenRow("meisai")) Then
                If CInt(jokenRow("meisai")) <> 2 Then
                    MessageBox.Show("リース債務返済明細表が「配賦単位」でないと実行できません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Me.Close()
                    Return
                End If
            End If

            ' ワークテーブル初期化 + 対象年月・計上日セット
            _crud.ExecuteNonQuery("DELETE FROM tw_f_仕訳出力標準_sm WHERE TRUE")

            ' 年度末YMD取得 → 長短振替日(KEIJO1_DT)
            Dim keijo1Dt = GetNendoMatsuYMD(kikanTo)
            ' 長短振替戻し日(KEIJO2_DT) = 長短振替日の翌日
            Dim keijo2Dt = keijo1Dt.AddDays(1)

            _crud.ExecuteNonQuery("INSERT INTO tw_f_仕訳出力標準_sm (対象年月, keijo1_dt, keijo2_dt) VALUES (@p1, @p2, @p3)",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@p1", kikanFrom),
                    New NpgsqlParameter("@p2", keijo1Dt),
                    New NpgsqlParameter("@p3", keijo2Dt)
                })

            ' フォームに表示
            txt_対象年月.Text = kikanFrom.ToString("yyyy/MM")
            txt_KEIJO1_DT.Text = keijo1Dt.ToString("yyyyMMdd")
            txt_KEIJO2_DT.Text = keijo2Dt.ToString("yyyyMMdd")

        Catch ex As Exception
            MessageBox.Show("初期化エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    ' ================================================================
    '  cmd_実行_Click (Access版 cmd_実行_Click 相当)
    ' ================================================================
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        Try
            ' 必須項目チェック
            If String.IsNullOrWhiteSpace(txt_KEIJO1_DT.Text) Then
                MessageBox.Show("長短振替日が未入力です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_KEIJO1_DT.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(txt_KEIJO2_DT.Text) Then
                MessageBox.Show("長短振替戻し日が未入力です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_KEIJO2_DT.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(txt_OUTPUT_FOLDER_NM.Text) Then
                MessageBox.Show("出力先フォルダ名が未入力です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txt_OUTPUT_FOLDER_NM.Focus()
                Return
            End If

            If Not System.IO.Directory.Exists(txt_OUTPUT_FOLDER_NM.Text) Then
                MessageBox.Show("指定したフォルダが存在しません。存在するフォルダを指定して再度実行してください。",
                                "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txt_OUTPUT_FOLDER_NM.Focus()
                Return
            End If

            ' 設定テーブル読込
            If Not LoadSettings() Then Return

            ' 実行確認
            If MessageBox.Show("実行してよろしいですか？", "確認",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return
            End If

            ' 仕訳データ作成
            Cursor = Cursors.WaitCursor
            Try
                If Not m仕訳データ作成() Then
                    Return
                End If

                ' データ件数チェック
                Dim dtCount = _crud.GetDataTable("SELECT COUNT(*) FROM tw_f_仕訳出力標準_sm_仕訳data")
                If CInt(dtCount.Rows(0)(0)) = 0 Then
                    MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Excel出力
                Dim filePath = ExportToExcel()

                If Not String.IsNullOrEmpty(filePath) Then
                    MessageBox.Show("以下のファイルを出力しました。" & vbCrLf & vbCrLf & filePath,
                                    "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                Me.Close()

            Finally
                Cursor = Cursors.Default
            End Try

        Catch ex As Exception
            MessageBox.Show("実行エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  cmd_CANCEL_Click
    ' ================================================================
    Private Sub cmd_CANCEL_Click(sender As Object, e As EventArgs) Handles cmd_CANCEL.Click
        Me.Close()
    End Sub

    ' ================================================================
    '  cmd_設定_Click
    ' ================================================================
    Private Sub cmd_設定_Click(sender As Object, e As EventArgs) Handles cmd_設定.Click
        Try
            Using frm As New Form_f_仕訳出力標準_設定_MAIN()
                frm.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show("設定画面エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ================================================================
    '  cmd_選択_Click (フォルダ選択)
    ' ================================================================
    Private Sub cmd_選択_Click(sender As Object, e As EventArgs) Handles cmd_選択.Click
        Using fbd As New FolderBrowserDialog()
            fbd.Description = "出力先フォルダの設定"
            If fbd.ShowDialog() = DialogResult.OK Then
                txt_OUTPUT_FOLDER_NM.Text = fbd.SelectedPath
            End If
        End Using
    End Sub

    ' ================================================================
    '  txt_KEIJO1_DT_Leave / txt_KEIJO2_DT_Leave (計上日の数値切捨て)
    ' ================================================================
    Private Sub txt_KEIJO1_DT_Leave(sender As Object, e As EventArgs) Handles txt_KEIJO1_DT.Leave
        Dim val As Integer
        If Integer.TryParse(txt_KEIJO1_DT.Text, val) Then
            txt_KEIJO1_DT.Text = val.ToString()
        End If
    End Sub

    Private Sub txt_KEIJO2_DT_Leave(sender As Object, e As EventArgs) Handles txt_KEIJO2_DT.Leave
        Dim val As Integer
        If Integer.TryParse(txt_KEIJO2_DT.Text, val) Then
            txt_KEIJO2_DT.Text = val.ToString()
        End If
    End Sub

    ' ================================================================
    '  設定テーブル読込
    ' ================================================================
    Private Function LoadSettings() As Boolean
        Try
            Dim dtSM = _crud.GetDataTable("SELECT * FROM tw_f_仕訳出力標準_設定_swksm LIMIT 1")
            If dtSM.Rows.Count = 0 Then
                SetDefaultSettings()
                dtSM = _crud.GetDataTable("SELECT * FROM tw_f_仕訳出力標準_設定_swksm LIMIT 1")
                If dtSM.Rows.Count = 0 Then
                    MessageBox.Show("仕訳出力設定（支払管理仕訳）が取得できません。設定画面で設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
            End If
            _settingSM = dtSM.Rows(0)

            Dim dtKY = _crud.GetDataTable("SELECT * FROM tw_f_仕訳出力標準_設定_swkky LIMIT 1")
            If dtKY.Rows.Count = 0 Then
                MessageBox.Show("仕訳出力共通設定が取得できません。設定画面で設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
            _settingKY = dtKY.Rows(0)

            Return True
        Catch ex As Exception
            MessageBox.Show("設定読込エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  初期値セット
    ' ================================================================
    Private Sub SetDefaultSettings()
        Try
            _crud.ExecuteNonQuery("SELECT set_swksm_defaults()")
        Catch
            ' ストアドがない場合は無視
        End Try
    End Sub

    ' ================================================================
    '  年度末YMD取得 (Access版 g年度末YMDGet 相当)
    ' ================================================================
    Private Function GetNendoMatsuYMD(kikanTo As DateTime) As DateTime
        Try
            Dim dtSettei = _crud.GetDataTable("SELECT val_int FROM t_settei WHERE settei_nm = 'NENDO_KAISHI_TSUKI'")
            Dim nendoStartMonth As Integer = 4
            If dtSettei.Rows.Count > 0 AndAlso Not IsDBNull(dtSettei.Rows(0)(0)) Then
                nendoStartMonth = CInt(dtSettei.Rows(0)(0))
            End If

            Dim year = kikanTo.Year
            Dim month = kikanTo.Month
            Dim nendoEndMonth = If(nendoStartMonth = 1, 12, nendoStartMonth - 1)
            Dim nendoEndYear = year
            If month >= nendoStartMonth Then
                nendoEndYear = year + 1
            End If
            If nendoStartMonth = 1 Then
                nendoEndYear = year
            End If

            Return New DateTime(nendoEndYear, nendoEndMonth, DateTime.DaysInMonth(nendoEndYear, nendoEndMonth))
        Catch
            Return New DateTime(kikanTo.Year, kikanTo.Month, DateTime.DaysInMonth(kikanTo.Year, kikanTo.Month))
        End Try
    End Function

    ' ================================================================
    '  Excel出力 (Access版 gEXCEL出力 相当)
    ' ================================================================
    Private Function ExportToExcel() As String
        Try
            Dim dt = _crud.GetDataTable("SELECT * FROM qsel_s仕訳出力標準_sm ORDER BY 仕訳seqno, 仕訳枝no")
            If dt.Rows.Count = 0 Then Return ""

            Dim timestamp = DateTime.Now.ToString("yyyyMMddHHmm")
            Dim fileName = txt_対象年月.Text.Replace("/", "") & "_リース債務返済明細表_仕訳_" & timestamp & ".xlsx"
            Dim filePath = System.IO.Path.Combine(txt_OUTPUT_FOLDER_NM.Text, fileName)

            ExportDataTableToExcel(dt, filePath)

            Return filePath
        Catch ex As Exception
            MessageBox.Show("Excel出力エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function

    Private Sub ExportDataTableToExcel(dt As DataTable, filePath As String)
        Dim xlApp As Object = Nothing
        Dim xlBooks As Object = Nothing
        Dim xlBook As Object = Nothing
        Dim xlSheet As Object = Nothing

        Try
            xlApp = CreateObject("Excel.Application")
            xlApp.Visible = False
            xlBooks = xlApp.Workbooks
            xlBook = xlBooks.Add()
            xlSheet = xlBook.Worksheets(1)

            For c As Integer = 0 To dt.Columns.Count - 1
                xlSheet.Cells(1, c + 1).Value = dt.Columns(c).ColumnName
            Next

            For r As Integer = 0 To dt.Rows.Count - 1
                For c As Integer = 0 To dt.Columns.Count - 1
                    Dim val = dt.Rows(r)(c)
                    If Not IsDBNull(val) Then
                        xlSheet.Cells(r + 2, c + 1).Value = val
                    End If
                Next
            Next

            xlBook.SaveAs(filePath)
        Finally
            If xlBook IsNot Nothing Then
                Try : xlBook.Close(False) : Catch : End Try
            End If
            If xlApp IsNot Nothing Then
                Try : xlApp.Quit() : Catch : End Try
            End If
            If xlSheet IsNot Nothing Then System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet)
            If xlBook IsNot Nothing Then System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBook)
            If xlBooks IsNot Nothing Then System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBooks)
            If xlApp IsNot Nothing Then System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)
        End Try
    End Sub

    ' ─── ヘルパー関数群 ───

    Private Function NzDec(value As Object) As Decimal
        If value Is Nothing OrElse IsDBNull(value) Then Return 0D
        Dim result As Decimal
        If Decimal.TryParse(value.ToString(), result) Then Return result
        Return 0D
    End Function

    Private Function NzStr(value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then Return ""
        Return value.ToString()
    End Function

    Private Function IsNoInput(value As Object) As Boolean
        Return value Is Nothing OrElse IsDBNull(value) OrElse String.IsNullOrWhiteSpace(value.ToString())
    End Function

    Private Function GCmp(val1 As Object, val2 As Object) As Boolean
        If IsNoInput(val1) AndAlso IsNoInput(val2) Then Return True
        If IsNoInput(val1) OrElse IsNoInput(val2) Then Return False
        Return val1.ToString() = val2.ToString()
    End Function

    Private Function GetSettingBool(row As DataRow, columnName As String) As Boolean
        If Not row.Table.Columns.Contains(columnName) Then Return False
        Dim val = row(columnName)
        If val Is Nothing OrElse IsDBNull(val) Then Return False
        If TypeOf val Is Boolean Then Return CBool(val)
        Return val.ToString() = "True" OrElse val.ToString() = "1" OrElse val.ToString() = "-1"
    End Function

    Private Function GetSettingStr(row As DataRow, columnName As String) As String
        If Not row.Table.Columns.Contains(columnName) Then Return ""
        Return NzStr(row(columnName))
    End Function

    ' ================================================================
    '  科目コード・名称取得 (Access版 mGET科目 相当)
    ' ================================================================
    Private Sub mGET科目(fldNm As String, cnstCD As String, cnstNM As String,
                         dataRow As DataRow, ByRef kamokuCD As Object, ByRef kamokuNM As Object)
        kamokuCD = DBNull.Value
        kamokuNM = DBNull.Value

        Select Case NzStr(fldNm)
            Case "SKMK1" : kamokuCD = dataRow("skmk_sum1_cd") : kamokuNM = dataRow("skmk_sum1_nm")
            Case "SKMK2" : kamokuCD = dataRow("skmk_sum2_cd") : kamokuNM = dataRow("skmk_sum2_nm")
            Case "SKMK3" : kamokuCD = dataRow("skmk_sum3_cd") : kamokuNM = dataRow("skmk_sum3_nm")
            Case "SKMK4" : kamokuCD = dataRow("skmk_sum4_cd") : kamokuNM = dataRow("skmk_sum4_nm")
            Case "SKMK5" : kamokuCD = dataRow("skmk_sum5_cd") : kamokuNM = dataRow("skmk_sum5_nm")
            Case "SKMK6" : kamokuCD = dataRow("skmk_sum6_cd") : kamokuNM = dataRow("skmk_sum6_nm")
            Case "SKMK7" : kamokuCD = dataRow("skmk_sum7_cd") : kamokuNM = dataRow("skmk_sum7_nm")
            Case "SKMK8" : kamokuCD = dataRow("skmk_sum8_cd") : kamokuNM = dataRow("skmk_sum8_nm")
            Case "SKMK9" : kamokuCD = dataRow("skmk_sum9_cd") : kamokuNM = dataRow("skmk_sum9_nm")
            Case "SKMK10" : kamokuCD = dataRow("skmk_sum10_cd") : kamokuNM = dataRow("skmk_sum10_nm")
            Case "HMK1" : kamokuCD = dataRow("kmk_cd1") : kamokuNM = dataRow("kmk_nm1")
            Case "HMK2" : kamokuCD = dataRow("kmk_cd2") : kamokuNM = dataRow("kmk_nm2")
            Case "HMK3" : kamokuCD = dataRow("kmk_cd3") : kamokuNM = dataRow("kmk_nm3")
            Case "HMK4" : kamokuCD = dataRow("kmk_cd4") : kamokuNM = dataRow("kmk_nm4")
            Case "HMK5" : kamokuCD = dataRow("kmk_cd5") : kamokuNM = dataRow("kmk_nm5")
            Case "HMK6" : kamokuCD = dataRow("kmk_cd6") : kamokuNM = dataRow("kmk_nm6")
            Case "HMK7" : kamokuCD = dataRow("kmk_cd7") : kamokuNM = dataRow("kmk_nm7")
            Case "HMK8" : kamokuCD = dataRow("kmk_cd8") : kamokuNM = dataRow("kmk_nm8")
            Case "HMK9" : kamokuCD = dataRow("kmk_cd9") : kamokuNM = dataRow("kmk_nm9")
            Case "HMK10" : kamokuCD = dataRow("kmk_cd10") : kamokuNM = dataRow("kmk_nm10")
            Case "CONST"
                kamokuCD = If(String.IsNullOrEmpty(cnstCD), DBNull.Value, CObj(cnstCD))
                kamokuNM = If(String.IsNullOrEmpty(cnstNM), DBNull.Value, CObj(cnstNM))
        End Select

        ' SWKKY_KMKNM_HOKAN: 科目コード・名称がどちらもブランクの場合、名称にデフォルトの値をセット
        If GetSettingBool(_settingKY, "swkky_kmknm_hokan") Then
            If IsNoInput(kamokuCD) AndAlso IsNoInput(kamokuNM) Then
                kamokuNM = If(String.IsNullOrEmpty(cnstNM), DBNull.Value, CObj(cnstNM))
            End If
        End If
    End Sub

    ' ================================================================
    '  科目No統合 (Access版 m仕訳データ作成_SUB_科目No統合 相当)
    ' ================================================================
    Private Sub m科目No統合(ByRef kamokuArr() As KamokuInfo, knoTogoF As Boolean)
        If Not knoTogoF Then Return

        For i = 0 To kamokuArr.Length - 1
            For j = i + 1 To kamokuArr.Length - 1
                If GCmp(kamokuArr(i).KamokuCD, kamokuArr(j).KamokuCD) AndAlso
                   GCmp(kamokuArr(i).KamokuNM, kamokuArr(j).KamokuNM) Then
                    kamokuArr(j).KamokuNo = kamokuArr(i).KamokuNo
                End If
            Next
        Next
    End Sub

    ' ================================================================
    '  共通項目SET (Access版 m仕訳データ作成_SUB_共通項目SET 相当)
    '  SM版: keijoDt パラメータで長短振替/長短振替戻しの計上日を指定
    ' ================================================================
    Private Function BuildCommonParams(dataRow As DataRow, seqNo As Integer, edaNo As Integer,
                                        kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                                        keijoDt As String) As List(Of NpgsqlParameter)
        Return New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@仕訳seqno", seqNo),
            New NpgsqlParameter("@仕訳枝no", edaNo),
            New NpgsqlParameter("@kjkbn_id", kjkbnId),
            New NpgsqlParameter("@仕訳ptn_no", ptnNo),
            New NpgsqlParameter("@仕訳ptn", ptnName),
            New NpgsqlParameter("@対象年月", If(IsDBNull(dataRow("対象年月")), DBNull.Value, dataRow("対象年月"))),
            New NpgsqlParameter("@計上日", keijoDt),
            New NpgsqlParameter("@kykbnl", If(IsDBNull(dataRow("kykbnl")), DBNull.Value, dataRow("kykbnl"))),
            New NpgsqlParameter("@kykh_no", If(IsDBNull(dataRow("kykh_no")), DBNull.Value, dataRow("kykh_no"))),
            New NpgsqlParameter("@kykm_no", If(IsDBNull(dataRow("kykm_no")), DBNull.Value, dataRow("kykm_no"))),
            New NpgsqlParameter("@bukn_bango1", If(IsDBNull(dataRow("bukn_bango1")), DBNull.Value, dataRow("bukn_bango1"))),
            New NpgsqlParameter("@bukn_bango2", If(IsDBNull(dataRow("bukn_bango2")), DBNull.Value, dataRow("bukn_bango2"))),
            New NpgsqlParameter("@bukn_bango3", If(IsDBNull(dataRow("bukn_bango3")), DBNull.Value, dataRow("bukn_bango3"))),
            New NpgsqlParameter("@bukn_nm", If(IsDBNull(dataRow("bukn_nm")), DBNull.Value, dataRow("bukn_nm"))),
            New NpgsqlParameter("@lcpt_id", If(IsDBNull(dataRow("lcpt_id")), DBNull.Value, dataRow("lcpt_id"))),
            New NpgsqlParameter("@kknri_id", If(IsDBNull(dataRow("kknri_id")), DBNull.Value, dataRow("kknri_id"))),
            New NpgsqlParameter("@b_bcat_id", If(IsDBNull(dataRow("b_bcat_id")), DBNull.Value, dataRow("b_bcat_id"))),
            New NpgsqlParameter("@skmk_id", If(IsDBNull(dataRow("skmk_id")), DBNull.Value, dataRow("skmk_id")))
        }
    End Function

    ' ================================================================
    '  仕訳DATAへ1行INSERT
    ' ================================================================
    Private Sub InsertSwkRow(prms As List(Of NpgsqlParameter),
                              dcKbnId As Integer,
                              drKamokuNo As Object, drKamokuCD As Object, drKamokuNM As Object, drKingaku As Object,
                              crKamokuNo As Object, crKamokuCD As Object, crKamokuNM As Object, crKingaku As Object)

        prms.Add(New NpgsqlParameter("@貸借区分id", dcKbnId))
        prms.Add(New NpgsqlParameter("@借方科目no", If(drKamokuNo Is Nothing, DBNull.Value, drKamokuNo)))
        prms.Add(New NpgsqlParameter("@借方科目cd", If(drKamokuCD Is Nothing OrElse IsDBNull(drKamokuCD), DBNull.Value, drKamokuCD)))
        prms.Add(New NpgsqlParameter("@借方科目", If(drKamokuNM Is Nothing OrElse IsDBNull(drKamokuNM), DBNull.Value, drKamokuNM)))
        prms.Add(New NpgsqlParameter("@借方金額", If(drKingaku Is Nothing OrElse IsDBNull(drKingaku), DBNull.Value, drKingaku)))
        prms.Add(New NpgsqlParameter("@貸方科目no", If(crKamokuNo Is Nothing, DBNull.Value, crKamokuNo)))
        prms.Add(New NpgsqlParameter("@貸方科目cd", If(crKamokuCD Is Nothing OrElse IsDBNull(crKamokuCD), DBNull.Value, crKamokuCD)))
        prms.Add(New NpgsqlParameter("@貸方科目", If(crKamokuNM Is Nothing OrElse IsDBNull(crKamokuNM), DBNull.Value, crKamokuNM)))
        prms.Add(New NpgsqlParameter("@貸方金額", If(crKingaku Is Nothing OrElse IsDBNull(crKingaku), DBNull.Value, crKingaku)))

        _crud.ExecuteNonQuery(
            "INSERT INTO tw_f_仕訳出力標準_sm_仕訳data " &
            "(仕訳seqno, 仕訳枝no, kjkbn_id, 仕訳ptn_no, 仕訳ptn, " &
            "対象年月, 計上日, kykbnl, kykh_no, kykm_no, " &
            "bukn_bango1, bukn_bango2, bukn_bango3, bukn_nm, " &
            "lcpt_id, kknri_id, b_bcat_id, skmk_id, " &
            "貸借区分id, 借方科目no, 借方科目cd, 借方科目, 借方金額, " &
            "貸方科目no, 貸方科目cd, 貸方科目, 貸方金額) " &
            "VALUES (@仕訳seqno, @仕訳枝no, @kjkbn_id, @仕訳ptn_no, @仕訳ptn, " &
            "@対象年月, @計上日, @kykbnl, @kykh_no, @kykm_no, " &
            "@bukn_bango1, @bukn_bango2, @bukn_bango3, @bukn_nm, " &
            "@lcpt_id, @kknri_id, @b_bcat_id, @skmk_id, " &
            "@貸借区分id, @借方科目no, @借方科目cd, @借方科目, @借方金額, " &
            "@貸方科目no, @貸方科目cd, @貸方科目, @貸方金額)",
            prms)
    End Sub

    ' ================================================================
    '  仕訳データ作成 メインディスパッチャ
    ' ================================================================
    Private Function m仕訳データ作成() As Boolean
        Try
            _crud.ExecuteNonQuery("DELETE FROM tw_f_仕訳出力標準_sm_仕訳data WHERE TRUE")
            _仕訳SEQNo = 1

            ' === 資産 (SSN1-6) ===
            If GetSettingBool(_settingSM, "swksm_ssn1_out_f") Then
                If Not m仕訳データ作成_SUB_資産_1長短振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_ssn2_out_f") Then
                If Not m仕訳データ作成_SUB_資産_2長短振替戻() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_ssn3_out_f") Then
                If Not m仕訳データ作成_SUB_資産_3元本振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_ssn4_out_f") Then
                If Not m仕訳データ作成_SUB_資産_4元本振替戻() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_ssn5_out_f") Then
                If Not m仕訳データ作成_SUB_資産_5消費税振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_ssn6_out_f") Then
                If Not m仕訳データ作成_SUB_資産_6消費税振替戻() Then Return False
            End If

            ' === 費用 (HIYO1-6) ===
            If GetSettingBool(_settingSM, "swksm_hiyo1_out_f") Then
                If Not m仕訳データ作成_SUB_費用_1長短振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_hiyo2_out_f") Then
                If Not m仕訳データ作成_SUB_費用_2長短振替戻() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_hiyo3_out_f") Then
                If Not m仕訳データ作成_SUB_費用_3消費税振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_hiyo4_out_f") Then
                If Not m仕訳データ作成_SUB_費用_4消費税振替戻() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_hiyo5_out_f") Then
                If Not m仕訳データ作成_SUB_費用_5減損振替() Then Return False
            End If
            If GetSettingBool(_settingSM, "swksm_hiyo6_out_f") Then
                If Not m仕訳データ作成_SUB_費用_6減損振替戻() Then Return False
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("仕訳データ作成エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ─── データ取得・出力共通 ───

    Private Function GetSaimuData(kjkbnId As Integer, Optional extraWhere As String = "", Optional orderBy As String = "kykm_no, kykm_id, line_id") As DataTable
        Dim sql = "SELECT * FROM qsel_df_flx_saimu WHERE kjkbn_id = @kjkbn_id"
        If Not String.IsNullOrEmpty(extraWhere) Then
            sql &= " AND " & extraWhere
        End If
        sql &= " ORDER BY " & orderBy
        Return _crud.GetDataTable(sql, New List(Of NpgsqlParameter) From {New NpgsqlParameter("@kjkbn_id", kjkbnId)})
    End Function

    Private Function IsDcBetu() As Boolean
        Return GetSettingBool(_settingKY, "swkky_dc_betu_f")
    End Function

    Private Sub OutputDR_Betu(dataRow As DataRow, ByRef edaNo As Integer, kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                               kamoku As KamokuInfo, kingaku As Decimal, keijoDt As String, ByRef flOut As Boolean)
        If kingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName, keijoDt)
        InsertSwkRow(prms, 1, kamoku.KamokuNo, kamoku.KamokuCD, kamoku.KamokuNM, kingaku,
                     DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value)
        edaNo += 1
        flOut = True
    End Sub

    Private Sub OutputCR_Betu(dataRow As DataRow, ByRef edaNo As Integer, kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                               kamoku As KamokuInfo, kingaku As Decimal, keijoDt As String, ByRef flOut As Boolean)
        If kingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName, keijoDt)
        InsertSwkRow(prms, 2, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                     kamoku.KamokuNo, kamoku.KamokuCD, kamoku.KamokuNM, kingaku)
        edaNo += 1
        flOut = True
    End Sub

    Private Sub OutputDCR_Same(dataRow As DataRow, ByRef edaNo As Integer, kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                                drKamoku As KamokuInfo, drKingaku As Decimal,
                                crKamoku As KamokuInfo, keijoDt As String, ByRef flOut As Boolean)
        If drKingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName, keijoDt)
        InsertSwkRow(prms, 0, drKamoku.KamokuNo, drKamoku.KamokuCD, drKamoku.KamokuNM, drKingaku,
                     crKamoku.KamokuNo, crKamoku.KamokuCD, crKamoku.KamokuNM, drKingaku)
        edaNo += 1
        flOut = True
    End Sub

    Private Sub CheckKykmIdChange(dataRow As DataRow, ByRef prevKykmId As Object, ByRef edaNo As Integer, ByRef flOut As Boolean)
        Dim currentKykmId = dataRow("kykm_id")
        Dim changed As Boolean
        If IsDBNull(prevKykmId) Then
            changed = True
        ElseIf IsDBNull(currentKykmId) Then
            changed = True
        Else
            changed = (CDbl(currentKykmId) <> CDbl(prevKykmId))
        End If
        If changed Then
            If flOut Then _仕訳SEQNo += 1
            edaNo = 1
            prevKykmId = currentKykmId
            flOut = False
        End If
    End Sub

    ' ================================================================
    '  SSN1: 資産_長短振替 (KEIJO1_DT)
    '  DR: リース元本残高(LGNPN_ZAN1NAI), 消費税残高(ZEI_ZAN1NAI)
    '  CR: リース元本残高(LGNPN_ZAN1NAI), 消費税残高(ZEI_ZAN1NAI)
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_1長短振替() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            ReDim _tmDR(1)
            ReDim _tmCR(1)

            Dim dt = GetSaimuData(KJKBN_SISAN)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                mGET科目(GetSettingStr(_settingSM, "swksm_ssn1_1d1_fldnm"), GetSettingStr(_settingSM, "swksm_ssn1_1d1_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn1_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_ssn1_1d2_fldnm"), GetSettingStr(_settingSM, "swksm_ssn1_1d2_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn1_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSM, "swksm_ssn1_kno_togo_f"))

                mGET科目(GetSettingStr(_settingSM, "swksm_ssn1_2c1_fldnm"), GetSettingStr(_settingSM, "swksm_ssn1_2c1_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn1_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_ssn1_2c2_fldnm"), GetSettingStr(_settingSM, "swksm_ssn1_2c2_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn1_2c2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmCR, GetSettingBool(_settingSM, "swksm_ssn1_kno_togo_f"))

                Dim amt1 = NzDec(row("lgnpn_zan1nai"))
                Dim amt2 = NzDec(row("zei_zan1nai"))

                If IsDcBetu() Then
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmDR(0), amt1, txt_KEIJO1_DT.Text, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmDR(1), amt2, txt_KEIJO1_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmCR(0), amt1, txt_KEIJO1_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmCR(1), amt2, txt_KEIJO1_DT.Text, flOut)
                Else
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmDR(0), amt1, _tmCR(0), txt_KEIJO1_DT.Text, flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "長短振替", _tmDR(1), amt2, _tmCR(1), txt_KEIJO1_DT.Text, flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("SSN1(長短振替)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  SSN2: 資産_長短振替戻し (KEIJO2_DT)
    '  SSN1の逆仕訳（同じ金額でDR/CR逆転、計上日=KEIJO2_DT）
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_2長短振替戻() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            ReDim _tmDR(1)
            ReDim _tmCR(1)

            Dim dt = GetSaimuData(KJKBN_SISAN)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ' 振替戻し: SSN1のCR科目をDRに、SSN1のDR科目をCRに
                mGET科目(GetSettingStr(_settingSM, "swksm_ssn2_1d1_fldnm"), GetSettingStr(_settingSM, "swksm_ssn2_1d1_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn2_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_ssn2_1d2_fldnm"), GetSettingStr(_settingSM, "swksm_ssn2_1d2_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn2_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSM, "swksm_ssn2_kno_togo_f"))

                mGET科目(GetSettingStr(_settingSM, "swksm_ssn2_2c1_fldnm"), GetSettingStr(_settingSM, "swksm_ssn2_2c1_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn2_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_ssn2_2c2_fldnm"), GetSettingStr(_settingSM, "swksm_ssn2_2c2_cnstcd"), GetSettingStr(_settingSM, "swksm_ssn2_2c2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmCR, GetSettingBool(_settingSM, "swksm_ssn2_kno_togo_f"))

                Dim amt1 = NzDec(row("lgnpn_zan1nai"))
                Dim amt2 = NzDec(row("zei_zan1nai"))

                If IsDcBetu() Then
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmDR(0), amt1, txt_KEIJO2_DT.Text, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmDR(1), amt2, txt_KEIJO2_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmCR(0), amt1, txt_KEIJO2_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmCR(1), amt2, txt_KEIJO2_DT.Text, flOut)
                Else
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmDR(0), amt1, _tmCR(0), txt_KEIJO2_DT.Text, flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 2, "長短振替戻", _tmDR(1), amt2, _tmCR(1), txt_KEIJO2_DT.Text, flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("SSN2(長短振替戻)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  SSN3: 資産_元本振替 (KEIJO1_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_3元本振替() As Boolean
        Return ProcessSimplePattern(KJKBN_SISAN, 3, "元本振替", "swksm_ssn3", "lgnpn_zan1nai", txt_KEIJO1_DT.Text)
    End Function

    ' ================================================================
    '  SSN4: 資産_元本振替戻し (KEIJO2_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_4元本振替戻() As Boolean
        Return ProcessSimplePattern(KJKBN_SISAN, 4, "元本振替戻", "swksm_ssn4", "lgnpn_zan1nai", txt_KEIJO2_DT.Text)
    End Function

    ' ================================================================
    '  SSN5: 資産_消費税振替 (KEIJO1_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_5消費税振替() As Boolean
        Return ProcessSimplePattern(KJKBN_SISAN, 5, "消費税振替", "swksm_ssn5", "zei_zan1nai", txt_KEIJO1_DT.Text)
    End Function

    ' ================================================================
    '  SSN6: 資産_消費税振替戻し (KEIJO2_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_6消費税振替戻() As Boolean
        Return ProcessSimplePattern(KJKBN_SISAN, 6, "消費税振替戻", "swksm_ssn6", "zei_zan1nai", txt_KEIJO2_DT.Text)
    End Function

    ' ================================================================
    '  HIYO1: 費用_長短振替 (KEIJO1_DT)
    '  DR: 消費税残高(ZEI_ZAN1NAI), 減損残高(GSON_ZAN1NAI)
    '  CR: 消費税残高(ZEI_ZAN1NAI), 減損残高(GSON_ZAN1NAI)
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_1長短振替() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            ReDim _tmDR(1)
            ReDim _tmCR(1)

            Dim dt = GetSaimuData(KJKBN_HIYO)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo1_1d1_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo1_1d1_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo1_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo1_1d2_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo1_1d2_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo1_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSM, "swksm_hiyo1_kno_togo_f"))

                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo1_2c1_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo1_2c1_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo1_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo1_2c2_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo1_2c2_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo1_2c2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmCR, GetSettingBool(_settingSM, "swksm_hiyo1_kno_togo_f"))

                Dim amt1 = NzDec(row("zei_zan1nai"))
                Dim amt2 = NzDec(row("gson_zan1nai"))

                If IsDcBetu() Then
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmDR(0), amt1, txt_KEIJO1_DT.Text, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmDR(1), amt2, txt_KEIJO1_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmCR(0), amt1, txt_KEIJO1_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmCR(1), amt2, txt_KEIJO1_DT.Text, flOut)
                Else
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmDR(0), amt1, _tmCR(0), txt_KEIJO1_DT.Text, flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "長短振替", _tmDR(1), amt2, _tmCR(1), txt_KEIJO1_DT.Text, flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("HIYO1(長短振替)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  HIYO2: 費用_長短振替戻し (KEIJO2_DT)
    '  HIYO1の逆仕訳
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_2長短振替戻() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            ReDim _tmDR(1)
            ReDim _tmCR(1)

            Dim dt = GetSaimuData(KJKBN_HIYO)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo2_1d1_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo2_1d1_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo2_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo2_1d2_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo2_1d2_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo2_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSM, "swksm_hiyo2_kno_togo_f"))

                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo2_2c1_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo2_2c1_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo2_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                mGET科目(GetSettingStr(_settingSM, "swksm_hiyo2_2c2_fldnm"), GetSettingStr(_settingSM, "swksm_hiyo2_2c2_cnstcd"), GetSettingStr(_settingSM, "swksm_hiyo2_2c2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmCR, GetSettingBool(_settingSM, "swksm_hiyo2_kno_togo_f"))

                Dim amt1 = NzDec(row("zei_zan1nai"))
                Dim amt2 = NzDec(row("gson_zan1nai"))

                If IsDcBetu() Then
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmDR(0), amt1, txt_KEIJO2_DT.Text, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmDR(1), amt2, txt_KEIJO2_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmCR(0), amt1, txt_KEIJO2_DT.Text, flOut)
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmCR(1), amt2, txt_KEIJO2_DT.Text, flOut)
                Else
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmDR(0), amt1, _tmCR(0), txt_KEIJO2_DT.Text, flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 2, "長短振替戻", _tmDR(1), amt2, _tmCR(1), txt_KEIJO2_DT.Text, flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("HIYO2(長短振替戻)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  HIYO3: 費用_消費税振替 (KEIJO1_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_3消費税振替() As Boolean
        Return ProcessSimplePattern(KJKBN_HIYO, 3, "消費税振替", "swksm_hiyo3", "zei_zan1nai", txt_KEIJO1_DT.Text)
    End Function

    ' ================================================================
    '  HIYO4: 費用_消費税振替戻し (KEIJO2_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_4消費税振替戻() As Boolean
        Return ProcessSimplePattern(KJKBN_HIYO, 4, "消費税振替戻", "swksm_hiyo4", "zei_zan1nai", txt_KEIJO2_DT.Text)
    End Function

    ' ================================================================
    '  HIYO5: 費用_減損振替 (KEIJO1_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_5減損振替() As Boolean
        Return ProcessSimplePattern(KJKBN_HIYO, 5, "減損振替", "swksm_hiyo5", "gson_zan1nai", txt_KEIJO1_DT.Text)
    End Function

    ' ================================================================
    '  HIYO6: 費用_減損振替戻し (KEIJO2_DT) 1DR/1CR シンプル
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_6減損振替戻() As Boolean
        Return ProcessSimplePattern(KJKBN_HIYO, 6, "減損振替戻", "swksm_hiyo6", "gson_zan1nai", txt_KEIJO2_DT.Text)
    End Function

    ''' <summary>
    ''' 1DR/1CR のシンプルパターン共通処理 (SSN3-6, HIYO3-6)
    ''' </summary>
    Private Function ProcessSimplePattern(kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                                           settingPrefix As String, amountColumn As String,
                                           keijoDt As String, Optional extraWhere As String = "") As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            Dim dt = GetSaimuData(kjkbnId, extraWhere)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                Dim amt = NzDec(row(amountColumn))
                If amt <> 0 Then
                    mGET科目(GetSettingStr(_settingSM, settingPrefix & "_1d1_fldnm"), GetSettingStr(_settingSM, settingPrefix & "_1d1_cnstcd"), GetSettingStr(_settingSM, settingPrefix & "_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                    Dim drK As New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                    mGET科目(GetSettingStr(_settingSM, settingPrefix & "_2c1_fldnm"), GetSettingStr(_settingSM, settingPrefix & "_2c1_cnstcd"), GetSettingStr(_settingSM, settingPrefix & "_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                    Dim crK As New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                    If IsDcBetu() Then
                        OutputDR_Betu(row, edaNo, kjkbnId, ptnNo, ptnName, drK, amt, keijoDt, flOut)
                        OutputCR_Betu(row, edaNo, kjkbnId, ptnNo, ptnName, crK, amt, keijoDt, flOut)
                    Else
                        OutputDCR_Same(row, edaNo, kjkbnId, ptnNo, ptnName, drK, amt, crK, keijoDt, flOut)
                    End If
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show($"{settingPrefix}({ptnName})エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class
