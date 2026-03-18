Imports System.Data
Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess
Imports Npgsql

''' <summary>
''' 月次支払照合フレックス 仕訳出力（支払照合仕訳）
''' Access版 Form_f_仕訳出力標準_SH を完全再現
''' 7パターン: 資産(SSN1-3) + 費用(HIYO1-4)
''' 計上日は3種(画面入力/支払日/記帳支払日)からラジオボタンで選択
''' </summary>
Partial Public Class Form_f_仕訳出力標準_SH
    Inherits Form

    ' ─── 定数 ───
    Private Const KJKBN_HIYO As Integer = 1     ' 計上区分=費用
    Private Const KJKBN_SISAN As Integer = 2    ' 計上区分=資産
    Private Const KKBN_HOSHU As Integer = 3     ' 契約区分=保守
    Private Const HIYOSHZEI_SHRI As Integer = 2 ' 消費税計上タイミング=支払の都度

    ' ─── フィールド ───
    Private _crud As New CrudHelper()
    Private _仕訳SEQNo As Integer
    Private _settingSH As DataRow    ' tw_f_仕訳出力標準_設定_SWKSH
    Private _settingKY As DataRow    ' tw_f_仕訳出力標準_設定_SWKKY
    Private _keijoDtKind As Integer  ' 計上日選択種別 (1=画面入力, 2=支払日, 3=記帳支払日)

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
    Private Sub Form_f_仕訳出力標準_SH_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' 月次支払照合フレックスの集計条件チェック
            Dim dtJoken = _crud.GetDataTable("SELECT * FROM tw_s_tougetsu_joken LIMIT 1")
            If dtJoken.Rows.Count = 0 Then
                MessageBox.Show("月次支払照合フレックスの集計条件が実行されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
            End If

            Dim jokenRow = dtJoken.Rows(0)
            If IsDBNull(jokenRow("kikan_from")) Then
                MessageBox.Show("月次支払照合フレックスの集計条件が実行されていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
                Return
            End If

            ' 計算対象・明細チェック
            If Not IsDBNull(jokenRow("meisai")) Then
                If CInt(jokenRow("meisai")) <> 2 Then
                    MessageBox.Show("月次支払照合フレックスの明細が「配賦単位」でないと実行できません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Me.Close()
                    Return
                End If
            End If

            ' 集計対象チェック (全件でない場合は警告)
            If Not IsDBNull(jokenRow("taisho")) Then
                If CInt(jokenRow("taisho")) <> 3 Then
                    If MessageBox.Show("月次支払照合フレックスの集計対象が「全件」ではありません。処理を中止しますか？",
                                       "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                        Me.Close()
                        Return
                    End If
                End If
            End If

            Dim kikanFrom As DateTime = CDate(jokenRow("kikan_from"))

            ' ワークテーブル初期化 + 対象年月・計上日セット
            _crud.ExecuteNonQuery("DELETE FROM tw_f_仕訳出力標準_sh WHERE TRUE")

            ' 年度末YMD取得 → 計上日初期値
            Dim keijoDt = GetNendoMatsuYMD(kikanFrom)

            ' 計上日選択種別(SWKSH_KEIJO_DT_KIND)を取得
            Dim keijoDtKind As Integer = 1  ' デフォルト: 画面入力
            Dim dtSHSetting = _crud.GetDataTable("SELECT swksh_keijo_dt_kind FROM tw_f_仕訳出力標準_設定_swksh LIMIT 1")
            If dtSHSetting.Rows.Count > 0 AndAlso Not IsDBNull(dtSHSetting.Rows(0)(0)) Then
                keijoDtKind = CInt(dtSHSetting.Rows(0)(0))
            Else
                ' デフォルト値テーブルから取得
                Dim dtDefault = _crud.GetDataTable("SELECT val_number FROM t_settei_仕訳出力標準_デフォルト値 WHERE settei_nm = 'SWKSH_KEIJO_DT_KIND'")
                If dtDefault.Rows.Count > 0 AndAlso Not IsDBNull(dtDefault.Rows(0)(0)) Then
                    keijoDtKind = CInt(dtDefault.Rows(0)(0))
                End If
            End If

            _crud.ExecuteNonQuery("INSERT INTO tw_f_仕訳出力標準_sh (対象年月, keijo_dt, swksh_keijo_dt_kind) VALUES (@p1, @p2, @p3)",
                New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@p1", kikanFrom),
                    New NpgsqlParameter("@p2", keijoDt),
                    New NpgsqlParameter("@p3", keijoDtKind)
                })

            ' フォームに表示
            txt_対象年月.Text = kikanFrom.ToString("yyyy/MM")
            txt_KEIJO_DT.Text = keijoDt.ToString("yyyyMMdd")

            ' ラジオボタン初期設定
            _keijoDtKind = keijoDtKind
            Select Case keijoDtKind
                Case 1 : オプション481.Checked = True
                Case 2 : オプション483.Checked = True
                Case 3 : オプション521.Checked = True
                Case Else : オプション481.Checked = True
            End Select
            UpdateKeijoDtLock()

        Catch ex As Exception
            MessageBox.Show("初期化エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    ' ================================================================
    '  ラジオボタン変更時 (Access版 opg_SWKSH_KEIJO_DT_KIND_AfterUpdate 相当)
    ' ================================================================
    Private Sub オプション481_CheckedChanged(sender As Object, e As EventArgs) Handles オプション481.CheckedChanged
        If オプション481.Checked Then _keijoDtKind = 1
        UpdateKeijoDtLock()
    End Sub

    Private Sub オプション483_CheckedChanged(sender As Object, e As EventArgs) Handles オプション483.CheckedChanged
        If オプション483.Checked Then _keijoDtKind = 2
        UpdateKeijoDtLock()
    End Sub

    Private Sub オプション521_CheckedChanged(sender As Object, e As EventArgs) Handles オプション521.CheckedChanged
        If オプション521.Checked Then _keijoDtKind = 3
        UpdateKeijoDtLock()
    End Sub

    Private Sub UpdateKeijoDtLock()
        txt_KEIJO_DT.ReadOnly = (_keijoDtKind <> 1)
    End Sub

    ' ================================================================
    '  cmd_実行_Click (Access版 cmd_実行_Click 相当)
    ' ================================================================
    Private Sub cmd_実行_Click(sender As Object, e As EventArgs) Handles cmd_実行.Click
        Try
            ' 計上日選択チェック
            If _keijoDtKind < 1 OrElse _keijoDtKind > 3 Then
                MessageBox.Show("計上日の選択が不正です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 計上日入力チェック (画面入力の場合のみ)
            If _keijoDtKind = 1 Then
                If String.IsNullOrWhiteSpace(txt_KEIJO_DT.Text) Then
                    MessageBox.Show("計上日が未入力です。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txt_KEIJO_DT.Focus()
                    Return
                End If
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

            ' SSN1チェック: 復帰・降級計算チェック
            If GetSettingBool(_settingSH, "swksh_ssn1_out_f") Then
                Dim dtJoken = _crud.GetDataTable("SELECT sw_rsok FROM tw_s_tougetsu_joken LIMIT 1")
                If dtJoken.Rows.Count > 0 Then
                    Dim swRsok = dtJoken.Rows(0)("sw_rsok")
                    If Not IsDBNull(swRsok) AndAlso CBool(swRsok) = False Then
                        If MessageBox.Show("月次支払照合フレックスの「復帰・降級計算」がOFFのため「資産・版1(決済)」の仕訳が出力されませんがよろしいですか？",
                                           "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                            Return
                        End If
                    End If
                End If
            End If

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
                Dim dtCount = _crud.GetDataTable("SELECT COUNT(*) FROM tw_f_仕訳出力標準_sh_仕訳data")
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
    '  txt_KEIJO_DT_Leave (計上日の数値切捨て)
    ' ================================================================
    Private Sub txt_KEIJO_DT_Leave(sender As Object, e As EventArgs) Handles txt_KEIJO_DT.Leave
        Dim val As Integer
        If Integer.TryParse(txt_KEIJO_DT.Text, val) Then
            txt_KEIJO_DT.Text = val.ToString()
        End If
    End Sub

    ' ================================================================
    '  設定テーブル読込
    ' ================================================================
    Private Function LoadSettings() As Boolean
        Try
            Dim dtSH = _crud.GetDataTable("SELECT * FROM tw_f_仕訳出力標準_設定_swksh LIMIT 1")
            If dtSH.Rows.Count = 0 Then
                SetDefaultSettings()
                dtSH = _crud.GetDataTable("SELECT * FROM tw_f_仕訳出力標準_設定_swksh LIMIT 1")
                If dtSH.Rows.Count = 0 Then
                    MessageBox.Show("仕訳出力設定（支払照合仕訳）が取得できません。設定画面で設定してください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
            End If
            _settingSH = dtSH.Rows(0)

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
            _crud.ExecuteNonQuery("SELECT set_swksh_defaults()")
        Catch
            ' ストアドがない場合は無視
        End Try
    End Sub

    ' ================================================================
    '  年度末YMD取得 (Access版 g年度末YMDGet 相当)
    ' ================================================================
    Private Function GetNendoMatsuYMD(kikanFrom As DateTime) As DateTime
        Try
            Dim dtSettei = _crud.GetDataTable("SELECT val_int FROM t_settei WHERE settei_nm = 'NENDO_KAISHI_TSUKI'")
            Dim nendoStartMonth As Integer = 4
            If dtSettei.Rows.Count > 0 AndAlso Not IsDBNull(dtSettei.Rows(0)(0)) Then
                nendoStartMonth = CInt(dtSettei.Rows(0)(0))
            End If

            Dim year = kikanFrom.Year
            Dim month = kikanFrom.Month
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
            Return New DateTime(kikanFrom.Year, kikanFrom.Month, DateTime.DaysInMonth(kikanFrom.Year, kikanFrom.Month))
        End Try
    End Function

    ' ================================================================
    '  Excel出力 (Access版 gEXCEL出力 相当)
    ' ================================================================
    Private Function ExportToExcel() As String
        Try
            Dim dt = _crud.GetDataTable("SELECT * FROM qsel_s仕訳出力標準_sh ORDER BY 仕訳seqno, 仕訳枝no")
            If dt.Rows.Count = 0 Then Return ""

            Dim timestamp = DateTime.Now.ToString("yyyyMMddHHmm")
            Dim fileName = txt_対象年月.Text.Replace("/", "") & "_月次支払照合フレックス_仕訳_" & timestamp & ".xlsx"
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
    '  SH版: HMKフィールドは hrel_kmk_cdN / hrel_kmk_nmN を参照
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
            Case "HMK1" : kamokuCD = dataRow("hrel_kmk_cd1") : kamokuNM = dataRow("hrel_kmk_nm1")
            Case "HMK2" : kamokuCD = dataRow("hrel_kmk_cd2") : kamokuNM = dataRow("hrel_kmk_nm2")
            Case "HMK3" : kamokuCD = dataRow("hrel_kmk_cd3") : kamokuNM = dataRow("hrel_kmk_nm3")
            Case "HMK4" : kamokuCD = dataRow("hrel_kmk_cd4") : kamokuNM = dataRow("hrel_kmk_nm4")
            Case "HMK5" : kamokuCD = dataRow("hrel_kmk_cd5") : kamokuNM = dataRow("hrel_kmk_nm5")
            Case "HMK6" : kamokuCD = dataRow("hrel_kmk_cd6") : kamokuNM = dataRow("hrel_kmk_nm6")
            Case "HMK7" : kamokuCD = dataRow("hrel_kmk_cd7") : kamokuNM = dataRow("hrel_kmk_nm7")
            Case "HMK8" : kamokuCD = dataRow("hrel_kmk_cd8") : kamokuNM = dataRow("hrel_kmk_nm8")
            Case "HMK9" : kamokuCD = dataRow("hrel_kmk_cd9") : kamokuNM = dataRow("hrel_kmk_nm9")
            Case "HMK10" : kamokuCD = dataRow("hrel_kmk_cd10") : kamokuNM = dataRow("hrel_kmk_nm10")
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
    '  計上日取得 (Access版 m仕訳データ作成_SUB_共通項目SET の計上日部分)
    '  SH版: ラジオボタン選択に応じて3種の計上日を返す
    ' ================================================================
    Private Function GetKeijoDt(dataRow As DataRow) As String
        Select Case _keijoDtKind
            Case 1  ' 画面の計上日
                Return txt_KEIJO_DT.Text
            Case 2  ' 支払日
                Return NzStr(dataRow("shri_dt"))
            Case 3  ' 記帳支払日
                Return NzStr(dataRow("記帳支払日"))
            Case Else
                Return txt_KEIJO_DT.Text
        End Select
    End Function

    ' ================================================================
    '  共通項目SET (Access版 m仕訳データ作成_SUB_共通項目SET 相当)
    '  SH版: KJと同様の追加フィールド + SH固有(SHHO_ID, 支払日, 記帳支払日)
    ' ================================================================
    Private Function BuildCommonParams(dataRow As DataRow, seqNo As Integer, edaNo As Integer,
                                        kjkbnId As Integer, ptnNo As Integer, ptnName As String) As List(Of NpgsqlParameter)
        Return New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@仕訳seqno", seqNo),
            New NpgsqlParameter("@仕訳枝no", edaNo),
            New NpgsqlParameter("@kjkbn_id", kjkbnId),
            New NpgsqlParameter("@仕訳ptn_no", ptnNo),
            New NpgsqlParameter("@仕訳ptn", ptnName),
            New NpgsqlParameter("@対象年月", If(IsDBNull(dataRow("対象年月")), DBNull.Value, dataRow("対象年月"))),
            New NpgsqlParameter("@計上日", GetKeijoDt(dataRow)),
            New NpgsqlParameter("@kykbnl", If(IsDBNull(dataRow("kykbnl")), DBNull.Value, dataRow("kykbnl"))),
            New NpgsqlParameter("@kykh_no", If(IsDBNull(dataRow("kykh_no")), DBNull.Value, dataRow("kykh_no"))),
            New NpgsqlParameter("@saikaisu", If(IsDBNull(dataRow("saikaisu")), DBNull.Value, dataRow("saikaisu"))),
            New NpgsqlParameter("@kykm_no", If(IsDBNull(dataRow("kykm_no")), DBNull.Value, dataRow("kykm_no"))),
            New NpgsqlParameter("@bukn_bango1", If(IsDBNull(dataRow("bukn_bango1")), DBNull.Value, dataRow("bukn_bango1"))),
            New NpgsqlParameter("@bukn_bango2", If(IsDBNull(dataRow("bukn_bango2")), DBNull.Value, dataRow("bukn_bango2"))),
            New NpgsqlParameter("@bukn_bango3", If(IsDBNull(dataRow("bukn_bango3")), DBNull.Value, dataRow("bukn_bango3"))),
            New NpgsqlParameter("@bukn_nm", If(IsDBNull(dataRow("bukn_nm")), DBNull.Value, dataRow("bukn_nm"))),
            New NpgsqlParameter("@lsryo_total", If(IsDBNull(dataRow("lsryo_total")), DBNull.Value, dataRow("lsryo_total"))),
            New NpgsqlParameter("@zritu", If(IsDBNull(dataRow("zritu")), DBNull.Value, dataRow("zritu"))),
            New NpgsqlParameter("@lcpt_id", If(IsDBNull(dataRow("lcpt_id")), DBNull.Value, dataRow("lcpt_id"))),
            New NpgsqlParameter("@kknri_id", If(IsDBNull(dataRow("kknri_id")), DBNull.Value, dataRow("kknri_id"))),
            New NpgsqlParameter("@b_bcat_id", If(IsDBNull(dataRow("b_bcat_id")), DBNull.Value, dataRow("b_bcat_id"))),
            New NpgsqlParameter("@h_bcat_id", If(IsDBNull(dataRow("h_bcat_id")), DBNull.Value, dataRow("h_bcat_id"))),
            New NpgsqlParameter("@skmk_id", If(IsDBNull(dataRow("skmk_id")), DBNull.Value, dataRow("skmk_id"))),
            New NpgsqlParameter("@shho_id", If(IsDBNull(dataRow("shho_id")), DBNull.Value, dataRow("shho_id"))),
            New NpgsqlParameter("@支払日", If(IsDBNull(dataRow("shri_dt")), DBNull.Value, dataRow("shri_dt"))),
            New NpgsqlParameter("@記帳支払日", If(IsDBNull(dataRow("記帳支払日")), DBNull.Value, dataRow("記帳支払日")))
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
            "INSERT INTO tw_f_仕訳出力標準_sh_仕訳data " &
            "(仕訳seqno, 仕訳枝no, kjkbn_id, 仕訳ptn_no, 仕訳ptn, " &
            "対象年月, 計上日, kykbnl, kykh_no, saikaisu, kykm_no, " &
            "bukn_bango1, bukn_bango2, bukn_bango3, bukn_nm, " &
            "lsryo_total, zritu, lcpt_id, kknri_id, b_bcat_id, h_bcat_id, skmk_id, " &
            "shho_id, 支払日, 記帳支払日, " &
            "貸借区分id, 借方科目no, 借方科目cd, 借方科目, 借方金額, " &
            "貸方科目no, 貸方科目cd, 貸方科目, 貸方金額) " &
            "VALUES (@仕訳seqno, @仕訳枝no, @kjkbn_id, @仕訳ptn_no, @仕訳ptn, " &
            "@対象年月, @計上日, @kykbnl, @kykh_no, @saikaisu, @kykm_no, " &
            "@bukn_bango1, @bukn_bango2, @bukn_bango3, @bukn_nm, " &
            "@lsryo_total, @zritu, @lcpt_id, @kknri_id, @b_bcat_id, @h_bcat_id, @skmk_id, " &
            "@shho_id, @支払日, @記帳支払日, " &
            "@貸借区分id, @借方科目no, @借方科目cd, @借方科目, @借方金額, " &
            "@貸方科目no, @貸方科目cd, @貸方科目, @貸方金額)",
            prms)
    End Sub

    ' ================================================================
    '  仕訳データ作成 メインディスパッチャ
    ' ================================================================
    Private Function m仕訳データ作成() As Boolean
        Try
            _crud.ExecuteNonQuery("DELETE FROM tw_f_仕訳出力標準_sh_仕訳data WHERE TRUE")
            _仕訳SEQNo = 1

            ' === 資産 (SSN1-3) ===
            If GetSettingBool(_settingSH, "swksh_ssn1_out_f") Then
                ' SSN1は復帰・降級計算がONの場合のみ実行
                Dim dtJoken = _crud.GetDataTable("SELECT sw_rsok FROM tw_s_tougetsu_joken LIMIT 1")
                If dtJoken.Rows.Count > 0 AndAlso Not IsDBNull(dtJoken.Rows(0)("sw_rsok")) AndAlso CBool(dtJoken.Rows(0)("sw_rsok")) Then
                    If Not m仕訳データ作成_SUB_資産_1版1決済() Then Return False
                End If
            End If
            If GetSettingBool(_settingSH, "swksh_ssn2_out_f") Then
                If Not m仕訳データ作成_SUB_資産_2版2() Then Return False
            End If
            If GetSettingBool(_settingSH, "swksh_ssn3_out_f") Then
                If Not m仕訳データ作成_SUB_資産_3版3消費税() Then Return False
            End If

            ' === 費用 (HIYO1-4) ===
            If GetSettingBool(_settingSH, "swksh_hiyo1_out_f") Then
                If Not m仕訳データ作成_SUB_費用_1版1() Then Return False
            End If
            If GetSettingBool(_settingSH, "swksh_hiyo2_out_f") Then
                If Not m仕訳データ作成_SUB_費用_2版2月間() Then Return False
            End If
            If GetSettingBool(_settingSH, "swksh_hiyo3_out_f") Then
                If Not m仕訳データ作成_SUB_費用_3版3一括() Then Return False
            End If
            If GetSettingBool(_settingSH, "swksh_hiyo4_out_f") Then
                If Not m仕訳データ作成_SUB_費用_4版4一括リース() Then Return False
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("仕訳データ作成エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ─── データ取得・出力共通 ───

    Private Function GetTougetsuData(kjkbnId As Integer, Optional extraWhere As String = "",
                                      Optional orderBy As String = "kykm_no, kykm_id, kkbn_id, rec_kbn, line_id") As DataTable
        Dim sql = "SELECT * FROM qsel_df_flx_tougetsu WHERE kjkbn_id = @kjkbn_id"
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
                               kamoku As KamokuInfo, kingaku As Decimal, ByRef flOut As Boolean)
        If kingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName)
        InsertSwkRow(prms, 1, kamoku.KamokuNo, kamoku.KamokuCD, kamoku.KamokuNM, kingaku,
                     DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value)
        edaNo += 1
        flOut = True
    End Sub

    Private Sub OutputCR_Betu(dataRow As DataRow, ByRef edaNo As Integer, kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                               kamoku As KamokuInfo, kingaku As Decimal, ByRef flOut As Boolean)
        If kingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName)
        InsertSwkRow(prms, 2, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                     kamoku.KamokuNo, kamoku.KamokuCD, kamoku.KamokuNM, kingaku)
        edaNo += 1
        flOut = True
    End Sub

    Private Sub OutputDCR_Same(dataRow As DataRow, ByRef edaNo As Integer, kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                                drKamoku As KamokuInfo, drKingaku As Decimal,
                                crKamoku As KamokuInfo, ByRef flOut As Boolean)
        If drKingaku = 0 Then Return
        Dim prms = BuildCommonParams(dataRow, _仕訳SEQNo, edaNo, kjkbnId, ptnNo, ptnName)
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
    '  SSN1: 資産_版1(決済) — 5DR/1CR 複合パターン
    '  DR1: LGNPN_TOKI(リース元本), DR2: LRSOK_TOKI(復帰削減),
    '  DR3: IJIKNR_TOKI(維持管理), DR4: ZEI_TOKI(一括算出), DR5: ZEI_TOKI(月次算出)
    '  CR1: 合計金額
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_1版1決済() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing

            Dim dt = GetTougetsuData(KJKBN_SISAN)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ReDim _tmDR(4) ' 5 DR accounts
                ReDim _tmCR(0) ' 1 CR account

                ' DR1: リース元本
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_1d1_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_1d1_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR2: 復帰削減
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_1d2_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_1d2_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR3: 維持管理費用
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_1d3_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_1d3_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_1d3_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(2) = New KamokuInfo With {.KamokuNo = 3, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR4: 消費税(一括算出)
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_1d4_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_1d4_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_1d4_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(3) = New KamokuInfo With {.KamokuNo = 4, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR5: 消費税(月次算出)
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_1d5_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_1d5_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_1d5_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(4) = New KamokuInfo With {.KamokuNo = 5, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSH, "swksh_ssn1_kno_togo_f"))

                ' CR1: 預金/買掛/未払金
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn1_2c1_fldnm"), GetSettingStr(_settingSH, "swksh_ssn1_2c1_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn1_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                Dim amtLgnpn = NzDec(row("lgnpn_toki"))
                Dim amtLrsok = NzDec(row("lrsok_toki"))
                Dim amtIjiknr = NzDec(row("ijiknr_toki"))
                Dim amtZei = NzDec(row("zei_toki"))
                Dim szeiKeijoTmg = If(IsDBNull(row("szei_keijo_tmg")), 0, CInt(row("szei_keijo_tmg")))

                If IsDcBetu() Then
                    ' 貸借別行出力
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(0), amtLgnpn, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(1), amtLrsok, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(2), amtIjiknr, flOut)
                    If amtZei <> 0 Then
                        If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                            ' 月次算出 → DR5
                            OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(4), amtZei, flOut)
                        Else
                            ' 一括算出 → DR4
                            OutputDR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(3), amtZei, flOut)
                        End If
                    End If
                    ' CR: 合計
                    Dim crTotal = amtLgnpn + amtLrsok + amtIjiknr + amtZei
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmCR(0), crTotal, flOut)
                Else
                    ' 貸借同行出力
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(0), amtLgnpn, _tmCR(0), flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(1), amtLrsok, _tmCR(0), flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(2), amtIjiknr, _tmCR(0), flOut)
                    If amtZei <> 0 Then
                        If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                            OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(4), amtZei, _tmCR(0), flOut)
                        Else
                            OutputDCR_Same(row, edaNo, KJKBN_SISAN, 1, "版1(決済)", _tmDR(3), amtZei, _tmCR(0), flOut)
                        End If
                    End If
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("SSN1(版1決済)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  SSN2: 資産_版2 — 2DR/1CR
    '  月次算出: DR1=LSRYO_TOKI, DR2=ZEI_TOKI
    '  一括算出: DR1=LSRYO_TOKI+ZEI_TOKI (合算)
    '  CR1: LSRYO_TOKI + ZEI_TOKI
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_2版2() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing

            Dim dt = GetTougetsuData(KJKBN_SISAN)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ReDim _tmDR(1) ' 2 DR accounts
                ReDim _tmCR(0) ' 1 CR account

                ' DR1: 預金/買掛/未払金
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn2_1d1_fldnm"), GetSettingStr(_settingSH, "swksh_ssn2_1d1_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn2_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR2: 消費税
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn2_1d2_fldnm"), GetSettingStr(_settingSH, "swksh_ssn2_1d2_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn2_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSH, "swksh_ssn2_kno_togo_f"))

                ' CR1
                mGET科目(GetSettingStr(_settingSH, "swksh_ssn2_2c1_fldnm"), GetSettingStr(_settingSH, "swksh_ssn2_2c1_cnstcd"), GetSettingStr(_settingSH, "swksh_ssn2_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                Dim amtLsryo = NzDec(row("lsryo_toki"))
                Dim amtZei = NzDec(row("zei_toki"))
                Dim szeiKeijoTmg = If(IsDBNull(row("szei_keijo_tmg")), 0, CInt(row("szei_keijo_tmg")))
                Dim crTotal = amtLsryo + amtZei

                If IsDcBetu() Then
                    If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                        ' 月次算出: DR1=LSRYO_TOKI, DR2=ZEI_TOKI
                        OutputDR_Betu(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(0), amtLsryo, flOut)
                        OutputDR_Betu(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(1), amtZei, flOut)
                    Else
                        ' 一括算出: DR1=合算
                        OutputDR_Betu(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(0), crTotal, flOut)
                    End If
                    OutputCR_Betu(row, edaNo, KJKBN_SISAN, 2, "版2", _tmCR(0), crTotal, flOut)
                Else
                    If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                        OutputDCR_Same(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(0), amtLsryo, _tmCR(0), flOut)
                        OutputDCR_Same(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(1), amtZei, _tmCR(0), flOut)
                    Else
                        OutputDCR_Same(row, edaNo, KJKBN_SISAN, 2, "版2", _tmDR(0), crTotal, _tmCR(0), flOut)
                    End If
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("SSN2(版2)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  SSN3: 資産_版3(月次算出・消費税のみ) — 1DR/1CR シンプル
    '  条件: SZEI_KEIJO_TMG = SHRI(月次算出)のみ対象
    ' ================================================================
    Private Function m仕訳データ作成_SUB_資産_3版3消費税() As Boolean
        Return ProcessSimplePattern(KJKBN_SISAN, 3, "版3(月次算出消費税のみ)", "swksh_ssn3", "zei_toki",
                                    "szei_keijo_tmg = " & HIYOSHZEI_SHRI.ToString())
    End Function

    ' ================================================================
    '  HIYO1: 費用_版1 — 4DR/1CR 複合パターン (HOSHU分岐あり)
    '  DR1: LSRYO_TOKI(リース料/非保守), DR2: LSRYO_TOKI(保守料/保守のみ)
    '  DR3: ZEI_TOKI(月次算出), DR4: ZEI_TOKI(一括算出)
    '  CR1: LSRYO_TOKI + ZEI_TOKI
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_1版1() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing

            Dim dt = GetTougetsuData(KJKBN_HIYO)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ReDim _tmDR(3) ' 4 DR accounts
                ReDim _tmCR(0) ' 1 CR account

                ' DR1: リース料
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo1_1d1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo1_1d1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo1_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR2: 保守料
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo1_1d2_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo1_1d2_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo1_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR3: 消費税(月次算出)
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo1_1d3_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo1_1d3_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo1_1d3_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(2) = New KamokuInfo With {.KamokuNo = 3, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR4: 消費税(一括算出)
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo1_1d4_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo1_1d4_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo1_1d4_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(3) = New KamokuInfo With {.KamokuNo = 4, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSH, "swksh_hiyo1_kno_togo_f"))

                ' CR1
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo1_2c1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo1_2c1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo1_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                Dim amtLsryo = NzDec(row("lsryo_toki"))
                Dim amtZei = NzDec(row("zei_toki"))
                Dim kkbnId = If(IsDBNull(row("kkbn_id")), 0, CInt(row("kkbn_id")))
                Dim szeiKeijoTmg = If(IsDBNull(row("szei_keijo_tmg")), 0, CInt(row("szei_keijo_tmg")))

                If IsDcBetu() Then
                    ' 貸借別行出力
                    ' リース料/保守料の振り分け
                    If amtLsryo <> 0 Then
                        If kkbnId = KKBN_HOSHU Then
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(1), amtLsryo, flOut)
                        Else
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(0), amtLsryo, flOut)
                        End If
                    End If
                    ' 消費税の振り分け
                    If amtZei <> 0 Then
                        If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(2), amtZei, flOut)
                        Else
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(3), amtZei, flOut)
                        End If
                    End If
                    ' CR: 合計
                    Dim crTotal = amtLsryo + amtZei
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 1, "版1", _tmCR(0), crTotal, flOut)
                Else
                    ' 貸借同行出力
                    If amtLsryo <> 0 Then
                        If kkbnId = KKBN_HOSHU Then
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(1), amtLsryo, _tmCR(0), flOut)
                        Else
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(0), amtLsryo, _tmCR(0), flOut)
                        End If
                    End If
                    If amtZei <> 0 Then
                        If szeiKeijoTmg = HIYOSHZEI_SHRI Then
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(2), amtZei, _tmCR(0), flOut)
                        Else
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 1, "版1", _tmDR(3), amtZei, _tmCR(0), flOut)
                        End If
                    End If
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("HIYO1(版1)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  HIYO2: 費用_版2(月間計上) — 3DR/1CR (HOSHU分岐あり)
    '  条件: SZEI_KEIJO_TMG = SHRI(月次算出)のみ対象
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_2版2月間() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing

            Dim dt = GetTougetsuData(KJKBN_HIYO, "szei_keijo_tmg = " & HIYOSHZEI_SHRI.ToString())
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ReDim _tmDR(2) ' 3 DR accounts
                ReDim _tmCR(0) ' 1 CR account

                ' DR1: リース料
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo2_1d1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo2_1d1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo2_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR2: 保守料
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo2_1d2_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo2_1d2_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo2_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR3: 消費税
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo2_1d3_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo2_1d3_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo2_1d3_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(2) = New KamokuInfo With {.KamokuNo = 3, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSH, "swksh_hiyo2_kno_togo_f"))

                ' CR1
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo2_2c1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo2_2c1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo2_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                Dim amtLsryo = NzDec(row("lsryo_toki"))
                Dim amtZei = NzDec(row("zei_toki"))
                Dim kkbnId = If(IsDBNull(row("kkbn_id")), 0, CInt(row("kkbn_id")))

                If IsDcBetu() Then
                    If amtLsryo <> 0 Then
                        If kkbnId = KKBN_HOSHU Then
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(1), amtLsryo, flOut)
                        Else
                            OutputDR_Betu(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(0), amtLsryo, flOut)
                        End If
                    End If
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(2), amtZei, flOut)
                    Dim crTotal = amtLsryo + amtZei
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmCR(0), crTotal, flOut)
                Else
                    If amtLsryo <> 0 Then
                        If kkbnId = KKBN_HOSHU Then
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(1), amtLsryo, _tmCR(0), flOut)
                        Else
                            OutputDCR_Same(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(0), amtLsryo, _tmCR(0), flOut)
                        End If
                    End If
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 2, "版2(月間計上)", _tmDR(2), amtZei, _tmCR(0), flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("HIYO2(版2月間)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  HIYO3: 費用_版3(一括算出) — 2DR/1CR
    '  条件: SZEI_KEIJO_TMG <> SHRI(一括算出)のみ対象
    '  DR1: LSRYO_TOKI, DR2: ZEI_TOKI
    '  CR1: LSRYO_TOKI + ZEI_TOKI
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_3版3一括() As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing

            Dim dt = GetTougetsuData(KJKBN_HIYO, "szei_keijo_tmg <> " & HIYOSHZEI_SHRI.ToString())
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                ReDim _tmDR(1) ' 2 DR accounts
                ReDim _tmCR(0) ' 1 CR account

                ' DR1: リース料
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo3_1d1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo3_1d1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo3_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                ' DR2: 消費税
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo3_1d2_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo3_1d2_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo3_1d2_cnstnm"), row, kamokuCD, kamokuNM)
                _tmDR(1) = New KamokuInfo With {.KamokuNo = 2, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}
                m科目No統合(_tmDR, GetSettingBool(_settingSH, "swksh_hiyo3_kno_togo_f"))

                ' CR1
                mGET科目(GetSettingStr(_settingSH, "swksh_hiyo3_2c1_fldnm"), GetSettingStr(_settingSH, "swksh_hiyo3_2c1_cnstcd"), GetSettingStr(_settingSH, "swksh_hiyo3_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                _tmCR(0) = New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                Dim amtLsryo = NzDec(row("lsryo_toki"))
                Dim amtZei = NzDec(row("zei_toki"))

                If IsDcBetu() Then
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 3, "版3(一括算出)", _tmDR(0), amtLsryo, flOut)
                    OutputDR_Betu(row, edaNo, KJKBN_HIYO, 3, "版3(一括算出)", _tmDR(1), amtZei, flOut)
                    Dim crTotal = amtLsryo + amtZei
                    OutputCR_Betu(row, edaNo, KJKBN_HIYO, 3, "版3(一括算出)", _tmCR(0), crTotal, flOut)
                Else
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 3, "版3(一括算出)", _tmDR(0), amtLsryo, _tmCR(0), flOut)
                    OutputDCR_Same(row, edaNo, KJKBN_HIYO, 3, "版3(一括算出)", _tmDR(1), amtZei, _tmCR(0), flOut)
                End If
            Next
            If flOut Then _仕訳SEQNo += 1
            Return True
        Catch ex As Exception
            MessageBox.Show("HIYO3(版3一括)エラー: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' ================================================================
    '  HIYO4: 費用_版4(一括算出・リースのみ) — 1DR/1CR シンプル
    '  条件: SZEI_KEIJO_TMG <> SHRI かつ LSRYO_TOKI <> 0
    ' ================================================================
    Private Function m仕訳データ作成_SUB_費用_4版4一括リース() As Boolean
        Return ProcessSimplePattern(KJKBN_HIYO, 4, "版4(一括算出リースのみ)", "swksh_hiyo4", "lsryo_toki",
                                    "szei_keijo_tmg <> " & HIYOSHZEI_SHRI.ToString())
    End Function

    ''' <summary>
    ''' 1DR/1CR のシンプルパターン共通処理 (SSN3, HIYO4)
    ''' </summary>
    Private Function ProcessSimplePattern(kjkbnId As Integer, ptnNo As Integer, ptnName As String,
                                           settingPrefix As String, amountColumn As String,
                                           Optional extraWhere As String = "") As Boolean
        Try
            Dim kamokuCD As Object = Nothing, kamokuNM As Object = Nothing
            Dim dt = GetTougetsuData(kjkbnId, extraWhere)
            Dim prevKykmId As Object = DBNull.Value
            Dim flOut As Boolean = False
            Dim edaNo As Integer = 1

            For Each row As DataRow In dt.Rows
                CheckKykmIdChange(row, prevKykmId, edaNo, flOut)

                Dim amt = NzDec(row(amountColumn))
                If amt <> 0 Then
                    mGET科目(GetSettingStr(_settingSH, settingPrefix & "_1d1_fldnm"), GetSettingStr(_settingSH, settingPrefix & "_1d1_cnstcd"), GetSettingStr(_settingSH, settingPrefix & "_1d1_cnstnm"), row, kamokuCD, kamokuNM)
                    Dim drK As New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                    mGET科目(GetSettingStr(_settingSH, settingPrefix & "_2c1_fldnm"), GetSettingStr(_settingSH, settingPrefix & "_2c1_cnstcd"), GetSettingStr(_settingSH, settingPrefix & "_2c1_cnstnm"), row, kamokuCD, kamokuNM)
                    Dim crK As New KamokuInfo With {.KamokuNo = 1, .KamokuCD = kamokuCD, .KamokuNM = kamokuNM}

                    If IsDcBetu() Then
                        OutputDR_Betu(row, edaNo, kjkbnId, ptnNo, ptnName, drK, amt, flOut)
                        OutputCR_Betu(row, edaNo, kjkbnId, ptnNo, ptnName, crK, amt, flOut)
                    Else
                        OutputDCR_Same(row, edaNo, kjkbnId, ptnNo, ptnName, drK, amt, crK, flOut)
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
