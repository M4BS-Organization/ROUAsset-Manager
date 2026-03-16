' Access版 pc_SHRI_KLSRYO を VB.NET に忠実に移植
' gKLSRYO_Main + mCALC_KLSRYOfromSCH + mCHK_集計対象 + mKLSRYO_SUB_OUTREC_SET

Imports System.Data
Imports Npgsql

Public Class KlsryoCalculationEngine

    Private _crud As New CrudHelper()

    ''' <summary>
    ''' メイン計算処理 (Access版 gKLSRYO_Main)
    ''' 期間FROM/TO と各パラメータを受けて DataTable を返す
    ''' </summary>
    Public Function Execute(
        dtFrom As Date, dtTo As Date,
        taisho As Integer,
        ktmg As ShriKtmg,
        meisai As ShriMeisai
    ) As DataTable

        ' *** 期首日/期末日の算出 (月末締め前提: ig締日=31)
        Dim kishuDt As Date = New Date(dtFrom.Year, dtFrom.Month, 1)
        Dim kimatDt As Date = CashScheduleBuilder.GetMonthEndDate(dtTo)

        ' *** 翌期以降年度末(5年分)を算出
        Dim ynKimatDt(4) As Date
        For i As Integer = 0 To 4
            Dim wk As Date
            If i = 0 Then
                wk = kimatDt
            Else
                wk = ynKimatDt(i - 1)
            End If
            wk = CashScheduleBuilder.GetMonthEndDate(wk)
            wk = wk.AddMonths(12)
            ynKimatDt(i) = CashScheduleBuilder.GetMonthEndDate(wk)
        Next

        ' *** 月度開始/終了日を算出
        Dim getudoFrom(12) As Date
        Dim getudoTo(11) As Date
        For i As Integer = 0 To 12
            getudoFrom(i) = kishuDt.AddMonths(i)
        Next
        For i As Integer = 0 To 11
            getudoTo(i) = getudoFrom(i + 1).AddDays(-1)
        Next

        ' *** 集計期間月数を算出
        Dim gCnt As Integer = 0
        Do
            If kishuDt.AddMonths(gCnt) > kimatDt Then Exit Do
            gCnt += 1
        Loop

        ' *** ソースSQL構築 + データ取得
        Dim sourceDt As DataTable = GetSourceData(kishuDt, kimatDt, ktmg, meisai, taisho)

        ' *** 結果DataTable作成
        Dim resultDt As DataTable = CreateResultTable(gCnt)

        ' *** 施行日を事前取得 (法令区分判定用)
        Dim sekouDt As Date = GetSekouDt()

        ' *** 各レコードをループして計算
        Select Case meisai
            Case ShriMeisai.Kykm
                ProcessKykm(sourceDt, resultDt, kishuDt, kimatDt, ynKimatDt, getudoFrom, getudoTo, gCnt, ktmg, sekouDt)
            Case ShriMeisai.Haif
                ProcessHaif(sourceDt, resultDt, kishuDt, kimatDt, ynKimatDt, getudoFrom, getudoTo, gCnt, ktmg, sekouDt)
        End Select

        ' *** 付随費用処理 (対象=保守 or 全部)
        If taisho = 2 OrElse taisho = 3 Then
            ProcessHenf(resultDt, kishuDt, kimatDt, ynKimatDt, getudoFrom, getudoTo, gCnt, ktmg, meisai, sekouDt)
        End If

        Return resultDt
    End Function

    ' --------------------------------------------------
    ' ソースデータ取得 (Access版 gMOTO_RSETSQL_EDIT)
    ' --------------------------------------------------
    Private Function GetSourceData(kishuDt As Date, kimatDt As Date, ktmg As ShriKtmg, meisai As ShriMeisai, taisho As Integer) As DataTable
        Dim sb As New System.Text.StringBuilder()
        Dim prms As New List(Of NpgsqlParameter)

        sb.Append("SELECT d_kykh.*, d_kykm.*")
        sb.Append(", d_kykh.kykh_id AS kykh_kykh_id")
        sb.Append(", d_kykh.kykh_no AS kykh_kykh_no")
        sb.Append(", d_kykh.saikaisu AS kykh_saikaisu")
        sb.Append(", d_kykm.kykm_id AS kykm_kykm_id")
        sb.Append(", d_kykm.kykm_no AS kykm_kykm_no")
        sb.Append(", d_kykm.saikaisu AS kykm_saikaisu")
        sb.Append(", d_kykh.kjkbn_id AS kykh_kjkbn_id")
        sb.Append(", d_kykh.shri_cnt AS kykh_shri_cnt")
        sb.Append(", d_kykh.mkaisu AS kykh_mkaisu")
        sb.Append(", d_kykm.kjkbn_id AS kykm_kjkbn_id")

        If meisai = ShriMeisai.Haif Then
            sb.Append(", d_haif.*")
        End If

        sb.Append(" FROM d_kykh")
        sb.Append(" INNER JOIN d_kykm ON d_kykh.kykh_id = d_kykm.kykh_id")
        If meisai = ShriMeisai.Haif Then
            sb.Append(" INNER JOIN d_haif ON d_kykm.kykm_id = d_haif.kykm_id")
        End If

        sb.Append(" WHERE d_kykh.k_seigou_f = true")

        ' 契約区分条件 (Access版 slJoken)
        Select Case taisho
            Case 1 ' リース料 → 保守以外
                sb.Append(" AND d_kykh.kkbn_id <> @hoshu")
                prms.Add(New NpgsqlParameter("@hoshu", Kkbn.Hoshu))
            Case 2 ' 保守料
                sb.Append(" AND d_kykh.kkbn_id = @hoshu")
                prms.Add(New NpgsqlParameter("@hoshu", Kkbn.Hoshu))
            Case 3 ' 全部 → 条件なし
        End Select

        sb.Append(" ORDER BY d_kykm.kykm_id")
        If meisai = ShriMeisai.Haif Then
            sb.Append(", d_haif.line_id DESC")
        End If

        Return _crud.GetDataTable(sb.ToString(), prms)
    End Function

    ' --------------------------------------------------
    ' 集計対象判定 (Access版 mCHK_集計対象)
    ' --------------------------------------------------
    Private Function IsTargetRecord(kishuDt As Date, kimatDt As Date, ktmg As ShriKtmg, row As DataRow) As Boolean
        Dim startDt As Object = row("start_dt")
        Dim bRendDt As Object = row("b_rend_dt")
        If IsDBNull(startDt) OrElse IsDBNull(bRendDt) Then Return True

        ' A: 契約期間が集計期間と重なる
        If CDate(startDt) <= kimatDt AndAlso CDate(bRendDt) >= kishuDt Then Return True

        Select Case ktmg
            Case ShriKtmg.SimeDtBase
                Dim bSmdtFstSum As Object = row("b_smdt_fst_sum")
                Dim bShdtLstSum As Object = row("b_shdt_lst_sum")
                If IsDBNull(bSmdtFstSum) Then Return True
                If IsDBNull(bShdtLstSum) Then Return True
                ' B: 支払計上期間が重なる
                If CDate(bSmdtFstSum) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then Return True
                ' C
                If CDate(startDt) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then Return True
                ' D
                If CDate(bSmdtFstSum) <= kimatDt AndAlso CDate(bRendDt) >= kishuDt Then Return True

            Case ShriKtmg.ShriDtBase
                Dim bShdtFstSum As Object = row("b_shdt_fst_sum")
                Dim bShdtLstSum As Object = row("b_shdt_lst_sum")
                If IsDBNull(bShdtFstSum) Then Return True
                If IsDBNull(bShdtLstSum) Then Return True
                ' B
                If CDate(bShdtFstSum) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then Return True
                ' C
                If CDate(startDt) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then Return True
                ' D
                If CDate(bShdtFstSum) <= kimatDt AndAlso CDate(bRendDt) >= kishuDt Then Return True
        End Select

        Return False
    End Function

    ' --------------------------------------------------
    ' 物件単位集計 (Access版 mKLSRYO_Sub_KYKM)
    ' --------------------------------------------------
    Private Sub ProcessKykm(sourceDt As DataTable, resultDt As DataTable,
        kishuDt As Date, kimatDt As Date, ynKimatDt() As Date,
        getudoFrom() As Date, getudoTo() As Date, gCnt As Integer,
        ktmg As ShriKtmg, sekouDt As Date)

        For Each row As DataRow In sourceDt.Rows
            If Not IsTargetRecord(kishuDt, kimatDt, ktmg, row) Then Continue For

            ' *** 定額 ***
            Dim bKlsryo = GetDbl(row, "b_klsryo")
            Dim bKzei = GetDbl(row, "b_kzei")
            Dim bMlsryo = GetDbl(row, "b_mlsryo")
            Dim bMzei = GetDbl(row, "b_mzei")

            If (bKlsryo <> 0 OrElse bKzei <> 0 OrElse bMlsryo <> 0 OrElse bMzei <> 0) Then
                Dim shriCnt = row("shri_cnt")
                If Not IsDBNull(shriCnt) AndAlso CInt(shriCnt) > 0 Then
                    Dim schedule = CashScheduleBuilder.BuildTeigakuSchedule(
                        ktmg, kishuDt, kimatDt, ynKimatDt(4),
                        CBool(row("jencho_f")),
                        CInt(row("shri_kn")), row("shri_cnt"),
                        CInt(row("sshri_kn_m")), CInt(row("sshri_kn_1")), CInt(row("sshri_kn_2")), CInt(row("sshri_kn_3")),
                        row("shho_m_id"), row("shho_1_id"), row("shho_2_id"), row("shho_3_id"),
                        row("mae_dt"), CDate(row("shri_dt1")), row("shri_dt2"), CInt(row("shri_dt3")),
                        GetDbl(row, "zritu"),
                        bKlsryo, bKzei, bMlsryo, bMzei,
                        row("ckaiyk_esdt_t"))

                    Dim klsryoResult = CalcKlsryoFromSchedule(
                        ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                        row("start_dt"), row("b_rend_dt"), CBool(row("jencho_f")), schedule)

                    AddResultRow(resultDt, row, klsryoResult, RecKbn.Teigaku, row("lcpt_id"), Nothing, sekouDt)
                End If
            End If

            ' *** 変額 ***
            If CBool(row("b_henl_f")) Then
                Dim schH = CashScheduleBuilder.BuildHengakuSchedule(_crud, CDbl(row("kykm_kykm_id")), row("ckaiyk_esdt_h"))
                If schH.Count > 0 Then
                    Dim klsryoResultH = CalcKlsryoFromSchedule(
                        ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                        row("start_dt"), row("b_rend_dt"), False, schH)

                    AddResultRow(resultDt, row, klsryoResultH, RecKbn.Hengaku, row("lcpt_id"), Nothing, sekouDt)
                End If
            End If
        Next
    End Sub

    ' --------------------------------------------------
    ' 配賦単位集計 (Access版 mKLSRYO_Sub_HAIF)
    ' --------------------------------------------------
    Private Sub ProcessHaif(sourceDt As DataTable, resultDt As DataTable,
        kishuDt As Date, kimatDt As Date, ynKimatDt() As Date,
        getudoFrom() As Date, getudoTo() As Date, gCnt As Integer,
        ktmg As ShriKtmg, sekouDt As Date)

        Dim prevKykmId As Double = -1
        Dim teigakuSchedule As List(Of CashScheduleEntry) = Nothing
        Dim hengakuSchedule As List(Of CashScheduleEntry) = Nothing

        For Each row As DataRow In sourceDt.Rows
            If Not IsTargetRecord(kishuDt, kimatDt, ktmg, row) Then Continue For

            Dim kykmId = CDbl(row("kykm_kykm_id"))

            ' 物件が変わったらスケジュールを再構築
            If kykmId <> prevKykmId Then
                prevKykmId = kykmId
                teigakuSchedule = Nothing
                hengakuSchedule = Nothing

                Dim bKlsryo = GetDbl(row, "h_klsryo")
                Dim bKzei = GetDbl(row, "h_kzei")
                Dim bMlsryo = GetDbl(row, "h_mlsryo")
                Dim bMzei = GetDbl(row, "h_mzei")

                If (bKlsryo <> 0 OrElse bKzei <> 0 OrElse bMlsryo <> 0 OrElse bMzei <> 0) Then
                    Dim shriCnt = row("shri_cnt")
                    If Not IsDBNull(shriCnt) AndAlso CInt(shriCnt) > 0 Then
                        teigakuSchedule = CashScheduleBuilder.BuildTeigakuSchedule(
                            ktmg, kishuDt, kimatDt, ynKimatDt(4),
                            CBool(row("jencho_f")),
                            CInt(row("shri_kn")), row("shri_cnt"),
                            CInt(row("sshri_kn_m")), CInt(row("sshri_kn_1")), CInt(row("sshri_kn_2")), CInt(row("sshri_kn_3")),
                            row("shho_m_id"), row("shho_1_id"), row("shho_2_id"), row("shho_3_id"),
                            row("mae_dt"), CDate(row("shri_dt1")), row("shri_dt2"), CInt(row("shri_dt3")),
                            GetDbl(row, "zritu"),
                            bKlsryo, bKzei, bMlsryo, bMzei,
                            row("ckaiyk_esdt_t"))
                    End If
                End If

                If CBool(row("b_henl_f")) Then
                    hengakuSchedule = CashScheduleBuilder.BuildHengakuSchedule(_crud, kykmId, row("ckaiyk_esdt_h"))
                End If
            End If

            ' 配賦率で按分してスケジュールから集計
            Dim haifritu As Double = GetDbl(row, "haifritu")
            Dim haifInfo As New HaifInfo()
            haifInfo.LineId = CInt(row("line_id"))
            haifInfo.Haifritu = haifritu
            haifInfo.HBcatId = row("h_bcat_id")
            haifInfo.HkmkId = row("hkmk_id")

            ' 定額
            If teigakuSchedule IsNot Nothing AndAlso teigakuSchedule.Count > 0 Then
                Dim haifSchedule = ApplyHaifritu(teigakuSchedule, haifritu)
                Dim klsResult = CalcKlsryoFromSchedule(
                    ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                    row("start_dt"), row("b_rend_dt"), CBool(row("jencho_f")), haifSchedule)
                AddResultRow(resultDt, row, klsResult, RecKbn.Teigaku, row("lcpt_id"), haifInfo, sekouDt)
            End If

            ' 変額
            If hengakuSchedule IsNot Nothing AndAlso hengakuSchedule.Count > 0 Then
                Dim haifScheduleH = ApplyHaifritu(hengakuSchedule, haifritu)
                Dim klsResultH = CalcKlsryoFromSchedule(
                    ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                    row("start_dt"), row("b_rend_dt"), False, haifScheduleH)
                AddResultRow(resultDt, row, klsResultH, RecKbn.Hengaku, row("lcpt_id"), haifInfo, sekouDt)
            End If
        Next
    End Sub

    ''' <summary>配賦率を適用したスケジュールのコピーを作成</summary>
    Private Function ApplyHaifritu(schedule As List(Of CashScheduleEntry), haifritu As Double) As List(Of CashScheduleEntry)
        Dim result As New List(Of CashScheduleEntry)
        For Each entry In schedule
            Dim copy As New CashScheduleEntry()
            copy.ShriDt = entry.ShriDt
            copy.SimeDt = entry.SimeDt
            copy.Lsryo = Math.Floor(entry.Lsryo * haifritu / 100)
            copy.Zritu = entry.Zritu
            copy.Zei = Math.Floor(entry.Zei * haifritu / 100)
            copy.MaeF = entry.MaeF
            copy.CkaiykF = entry.CkaiykF
            copy.ShhoId = entry.ShhoId
            copy.SshriKn = entry.SshriKn
            copy.LsryoHsum = 0
            copy.ZeiHsum = 0
            result.Add(copy)
        Next
        Return result
    End Function

    ' --------------------------------------------------
    ' 付随費用処理 (Access版 mKLSRYO_Sub_HENF)
    ' --------------------------------------------------
    Private Sub ProcessHenf(resultDt As DataTable,
        kishuDt As Date, kimatDt As Date, ynKimatDt() As Date,
        getudoFrom() As Date, getudoTo() As Date, gCnt As Integer,
        ktmg As ShriKtmg, meisai As ShriMeisai, sekouDt As Date)

        ' 付随費用レコード取得
        Dim henfSql = "SELECT d_kykh.*, d_kykm.*, d_henf.*" &
            ", d_kykh.kykh_id AS kykh_kykh_id, d_kykh.kykh_no AS kykh_kykh_no" &
            ", d_kykh.saikaisu AS kykh_saikaisu" &
            ", d_kykm.kykm_id AS kykm_kykm_id, d_kykm.kykm_no AS kykm_kykm_no" &
            ", d_kykm.saikaisu AS kykm_saikaisu" &
            ", d_kykm.kjkbn_id AS kykm_kjkbn_id, d_kykh.kjkbn_id AS kykh_kjkbn_id" &
            " FROM d_henf" &
            " INNER JOIN d_kykm ON d_henf.kykm_id = d_kykm.kykm_id" &
            " INNER JOIN d_kykh ON d_kykm.kykh_id = d_kykh.kykh_id" &
            " WHERE d_kykm.b_henf_f = true" &
            " ORDER BY d_henf.kykm_id, d_henf.line_id"

        Dim henfDt As DataTable
        Try
            henfDt = _crud.GetDataTable(henfSql)
        Catch
            Return ' テーブルが存在しない場合はスキップ
        End Try

        ' 配賦情報(配賦単位の場合)
        Dim haifDt As DataTable = Nothing
        If meisai = ShriMeisai.Haif Then
            Try
                Dim haifSql = "SELECT d_haif.* FROM d_haif" &
                    " INNER JOIN d_kykm ON d_haif.kykm_id = d_kykm.kykm_id" &
                    " WHERE d_kykm.b_henf_f = true" &
                    " ORDER BY d_haif.kykm_id, d_haif.line_id"
                haifDt = _crud.GetDataTable(haifSql)
            Catch
            End Try
        End If

        For Each henfRow As DataRow In henfDt.Rows
            ' 事前フィルタ: 集計期間外の付随費用をスキップ (Access版 mKLSRYO_Sub_HENF 準拠)
            Dim henfShriDt1 As Date = CDate(henfRow("shri_dt1"))
            Dim henfShriKn As Integer = CInt(henfRow("shri_kn"))
            Dim henfShriCnt As Integer = CInt(henfRow("shri_cnt"))
            Dim henfSshriKn As Integer = CInt(henfRow("sshri_kn"))
            Dim henfIlDay As Integer = If(henfShriDt1.Day >= DateTime.DaysInMonth(henfShriDt1.Year, henfShriDt1.Month), 31, henfShriDt1.Day)
            Dim lastShriDt As Date = CashScheduleBuilder.CalcNextShriDt(
                henfShriDt1.Year, henfShriDt1.Month, (henfShriCnt - 1) * henfShriKn, henfIlDay)
            If ktmg = ShriKtmg.ShriDtBase Then
                If lastShriDt < kishuDt AndAlso henfShriDt1 < kishuDt Then Continue For
                If henfShriDt1 > kimatDt Then Continue For
            Else
                Dim lastSimeDt As Date = CashScheduleBuilder.CalcSimeDtB(lastShriDt.Year, lastShriDt.Month, lastShriDt.Day, henfSshriKn)
                If lastSimeDt < kishuDt Then Continue For
                Dim firstSimeDt As Date = CashScheduleBuilder.CalcSimeDtB(henfShriDt1.Year, henfShriDt1.Month, henfShriDt1.Day, henfSshriKn)
                If firstSimeDt > kimatDt Then Continue For
            End If

            ' スケジュール生成
            Dim schedule = CashScheduleBuilder.BuildCommonSchedule(
                ktmg,
                CInt(henfRow("shri_kn")), CInt(henfRow("shri_cnt")), CInt(henfRow("sshri_kn")),
                henfRow("shho_id"), CDate(henfRow("shri_dt1")),
                GetDbl(henfRow, "zritu"), GetDbl(henfRow, "klsryo"), GetDbl(henfRow, "kzei"),
                Nothing)

            If schedule.Count = 0 Then Continue For

            If meisai = ShriMeisai.Kykm Then
                ' 物件単位
                Dim klsResult = CalcKlsryoFromSchedule(
                    ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                    henfRow("start_dt"), henfRow("end_dt"), False, schedule)
                AddHenfResultRow(resultDt, henfRow, klsResult, Nothing, sekouDt)
            Else
                ' 配賦単位: 配賦行ごとに按分
                Dim kykmId = CDbl(henfRow("kykm_id"))
                Dim haifRows = GetHaifRows(haifDt, kykmId)
                If haifRows.Count = 0 Then
                    Dim klsResult = CalcKlsryoFromSchedule(
                        ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                        henfRow("start_dt"), henfRow("end_dt"), False, schedule)
                    AddHenfResultRow(resultDt, henfRow, klsResult, Nothing, sekouDt)
                Else
                    For idx As Integer = 0 To haifRows.Count - 1
                        Dim haifRow = haifRows(idx)
                        Dim haifSchedule As List(Of CashScheduleEntry)
                        If idx = haifRows.Count - 1 Then
                            ' 最終行: 残額
                            haifSchedule = New List(Of CashScheduleEntry)
                            For Each entry In schedule
                                Dim copy As New CashScheduleEntry()
                                copy.ShriDt = entry.ShriDt
                                copy.SimeDt = entry.SimeDt
                                copy.Lsryo = entry.Lsryo - entry.LsryoHsum
                                copy.Zei = entry.Zei - entry.ZeiHsum
                                copy.Zritu = entry.Zritu
                                copy.MaeF = entry.MaeF
                                copy.CkaiykF = entry.CkaiykF
                                copy.ShhoId = entry.ShhoId
                                copy.SshriKn = entry.SshriKn
                                haifSchedule.Add(copy)
                            Next
                        Else
                            Dim hr = CDbl(haifRow("haifritu"))
                            haifSchedule = New List(Of CashScheduleEntry)
                            For Each entry In schedule
                                Dim copy As New CashScheduleEntry()
                                copy.ShriDt = entry.ShriDt
                                copy.SimeDt = entry.SimeDt
                                copy.Lsryo = Math.Floor(entry.Lsryo * hr / 100)
                                copy.Zei = Math.Floor(entry.Zei * hr / 100)
                                copy.Zritu = entry.Zritu
                                copy.MaeF = entry.MaeF
                                copy.CkaiykF = entry.CkaiykF
                                copy.ShhoId = entry.ShhoId
                                copy.SshriKn = entry.SshriKn
                                entry.LsryoHsum += copy.Lsryo
                                entry.ZeiHsum += copy.Zei
                                haifSchedule.Add(copy)
                            Next
                        End If

                        Dim hi As New HaifInfo()
                        hi.LineId = CInt(haifRow("line_id"))
                        hi.Haifritu = CDbl(haifRow("haifritu"))
                        hi.HBcatId = haifRow("h_bcat_id")

                        Dim klsResult = CalcKlsryoFromSchedule(
                            ktmg, kishuDt, kimatDt, gCnt, ynKimatDt, getudoFrom, getudoTo,
                            henfRow("start_dt"), henfRow("end_dt"), False, haifSchedule)
                        AddHenfResultRow(resultDt, henfRow, klsResult, hi, sekouDt)
                    Next
                End If
            End If
        Next
    End Sub

    Private Function GetHaifRows(haifDt As DataTable, kykmId As Double) As List(Of DataRow)
        Dim result As New List(Of DataRow)
        If haifDt Is Nothing Then Return result
        For Each row As DataRow In haifDt.Rows
            If CDbl(row("kykm_id")) = kykmId Then
                result.Add(row)
            End If
        Next
        Return result
    End Function

    ' --------------------------------------------------
    ' コア: スケジュールから期間集計 (Access版 mCALC_KLSRYOfromSCH)
    ' --------------------------------------------------
    Private Function CalcKlsryoFromSchedule(
        ktmg As ShriKtmg,
        kishuDt As Date, kimatDt As Date,
        gCnt As Integer,
        ynKimatDt() As Date,
        getudoFrom() As Date,
        getudoTo() As Date,
        vaStartDt As Object, vaRendDt As Object,
        jenchoF As Boolean,
        schedule As List(Of CashScheduleEntry)
    ) As KlsryoResult

        ' 開始日/終了日のNull補完
        Dim startDt As Date
        If vaStartDt Is Nothing OrElse IsDBNull(vaStartDt) Then
            If schedule.Count > 0 Then startDt = schedule(0).ShriDt Else startDt = kishuDt
        Else
            startDt = CDate(vaStartDt)
        End If
        Dim rendDt As Date
        If vaRendDt Is Nothing OrElse IsDBNull(vaRendDt) Then
            If schedule.Count > 0 Then rendDt = schedule(schedule.Count - 1).ShriDt Else rendDt = kimatDt
        Else
            rendDt = CDate(vaRendDt)
        End If

        ' 変数リセット
        Dim souKaisuCf As Integer = 0
        Dim sumiKaisu As Integer = 0
        Dim lsryoTotalCf As Double = 0
        Dim lsryoZen As Double = 0 : Dim lsryoZenCf As Double = 0
        Dim lsryoToki As Double = 0 : Dim lsryoTokiCf As Double = 0
        Dim lsryoYokuCf As Double = 0
        Dim lsryoYoku1NaiCf As Double = 0 : Dim lsryoYoku2NaiCf As Double = 0
        Dim lsryoYoku3NaiCf As Double = 0 : Dim lsryoYoku4NaiCf As Double = 0
        Dim lsryoYoku5NaiCf As Double = 0 : Dim lsryoYoku5ChoCf As Double = 0
        Dim lsryoTokig(11) As Object

        Dim zeiTotalCf As Double = 0
        Dim zeiZen As Double = 0 : Dim zeiZenCf As Double = 0
        Dim zeiToki As Double = 0 : Dim zeiTokiCf As Double = 0
        Dim zeiYokuCf As Double = 0
        Dim zeiYoku1NaiCf As Double = 0 : Dim zeiYoku2NaiCf As Double = 0
        Dim zeiYoku3NaiCf As Double = 0 : Dim zeiYoku4NaiCf As Double = 0
        Dim zeiYoku5NaiCf As Double = 0 : Dim zeiYoku5ChoCf As Double = 0
        Dim zeiTokig(11) As Object

        ' 月別配列の初期化
        For i As Integer = 0 To 11
            If i + 1 > gCnt Then
                lsryoTokig(i) = Nothing
                zeiTokig(i) = Nothing
            Else
                lsryoTokig(i) = 0.0
                zeiTokig(i) = 0.0
            End If
        Next

        Dim flHasseiZen As Boolean = False : Dim flHasseiZenCf As Boolean = False
        Dim flHasseiToki As Boolean = False : Dim flHasseiTokiCf As Boolean = False
        Dim flHasseiYokuki As Boolean = False : Dim flHasseiYokukiCf As Boolean = False

        ' *** 各金額を求める ***
        For Each entry In schedule
            ' 比較基準日
            Dim compareDate As Date
            If ktmg = ShriKtmg.SimeDtBase Then
                compareDate = entry.SimeDt
            Else
                compareDate = entry.ShriDt
            End If

            If Not entry.MaeF Then
                souKaisuCf += 1
            End If
            lsryoTotalCf += entry.Lsryo
            zeiTotalCf += entry.Zei

            If compareDate < kishuDt Then
                ' *** 前期以前 ***
                flHasseiZenCf = True
                lsryoZenCf += entry.Lsryo
                zeiZenCf += entry.Zei
                If Not entry.CkaiykF Then
                    flHasseiZen = True
                    lsryoZen += entry.Lsryo
                    zeiZen += entry.Zei
                    If Not entry.MaeF Then sumiKaisu += 1
                End If

            ElseIf compareDate <= kimatDt Then
                ' *** 当期 ***
                flHasseiTokiCf = True
                lsryoTokiCf += entry.Lsryo
                zeiTokiCf += entry.Zei
                If Not entry.CkaiykF Then
                    flHasseiToki = True
                    lsryoToki += entry.Lsryo
                    zeiToki += entry.Zei
                    If Not entry.MaeF Then sumiKaisu += 1
                    ' 月別内訳
                    For j As Integer = 0 To 11
                        If j + 1 <= gCnt Then
                            If compareDate >= getudoFrom(j) AndAlso compareDate <= getudoTo(j) Then
                                lsryoTokig(j) = CDbl(lsryoTokig(j)) + entry.Lsryo
                                zeiTokig(j) = CDbl(zeiTokig(j)) + entry.Zei
                            End If
                        End If
                    Next
                End If

            Else
                ' *** 翌期以降 ***
                flHasseiYokukiCf = True
                lsryoYokuCf += entry.Lsryo
                zeiYokuCf += entry.Zei
                If Not entry.CkaiykF Then
                    flHasseiYokuki = True
                End If

                ' 1〜5年超 振り分け
                If compareDate <= ynKimatDt(0) Then
                    lsryoYoku1NaiCf += entry.Lsryo : zeiYoku1NaiCf += entry.Zei
                ElseIf compareDate <= ynKimatDt(1) Then
                    lsryoYoku2NaiCf += entry.Lsryo : zeiYoku2NaiCf += entry.Zei
                ElseIf compareDate <= ynKimatDt(2) Then
                    lsryoYoku3NaiCf += entry.Lsryo : zeiYoku3NaiCf += entry.Zei
                ElseIf compareDate <= ynKimatDt(3) Then
                    lsryoYoku4NaiCf += entry.Lsryo : zeiYoku4NaiCf += entry.Zei
                ElseIf compareDate <= ynKimatDt(4) Then
                    lsryoYoku5NaiCf += entry.Lsryo : zeiYoku5NaiCf += entry.Zei
                Else
                    lsryoYoku5ChoCf += entry.Lsryo : zeiYoku5ChoCf += entry.Zei
                End If
            End If
        Next

        ' *** 結果を組み立て ***
        Dim result As New KlsryoResult()

        ' 総回数/済回数
        If jenchoF Then
            result.Soukaisu = Nothing
        Else
            result.Soukaisu = souKaisuCf
        End If
        result.Sumikaisu = sumiKaisu

        ' 期中増加
        If jenchoF Then
            result.LsryoZou = Nothing
        Else
            result.LsryoZou = 0.0
            If (Not flHasseiZenCf) AndAlso flHasseiTokiCf Then
                If startDt >= kishuDt Then
                    result.LsryoZou = lsryoTotalCf
                End If
            End If
            If startDt >= kishuDt AndAlso startDt <= kimatDt Then
                If Not flHasseiZenCf Then
                    result.LsryoZou = lsryoTotalCf
                End If
            End If
        End If

        ' 前期末残高
        If jenchoF Then
            result.LsryoZzan = Nothing
            result.ZeiZzan = Nothing
        Else
            result.LsryoZzan = lsryoTotalCf - lsryoZenCf
            result.ZeiZzan = zeiTotalCf - zeiZenCf
            If Not flHasseiZenCf Then
                If startDt >= kishuDt Then
                    result.LsryoZzan = 0.0
                    result.ZeiZzan = 0.0
                End If
            End If
            If (Not flHasseiToki) AndAlso (Not flHasseiYokuki) Then
                result.LsryoZzan = 0.0
                result.ZeiZzan = 0.0
            End If
        End If

        ' 期末残高 + 1〜5年超内訳
        If jenchoF Then
            result.LsryoZan = Nothing
            result.ZeiZan = Nothing
            If flHasseiYokuki Then
                result.LsryoZan1Nai = lsryoYoku1NaiCf
                result.LsryoZan2Nai = lsryoYoku2NaiCf
                result.LsryoZan3Nai = lsryoYoku3NaiCf
                result.LsryoZan4Nai = lsryoYoku4NaiCf
                result.LsryoZan5Nai = lsryoYoku5NaiCf
                result.LsryoZan5Cho = lsryoYoku5ChoCf
                result.ZeiZan1Nai = zeiYoku1NaiCf
                result.ZeiZan2Nai = zeiYoku2NaiCf
                result.ZeiZan3Nai = zeiYoku3NaiCf
                result.ZeiZan4Nai = zeiYoku4NaiCf
                result.ZeiZan5Nai = zeiYoku5NaiCf
                result.ZeiZan5Cho = zeiYoku5ChoCf
                If (Not flHasseiZenCf) AndAlso (Not flHasseiTokiCf) Then
                    If startDt > kimatDt Then
                        result.LsryoZan1Nai = 0 : result.LsryoZan2Nai = 0 : result.LsryoZan3Nai = 0
                        result.LsryoZan4Nai = 0 : result.LsryoZan5Nai = 0 : result.LsryoZan5Cho = 0
                        result.ZeiZan1Nai = 0 : result.ZeiZan2Nai = 0 : result.ZeiZan3Nai = 0
                        result.ZeiZan4Nai = 0 : result.ZeiZan5Nai = 0 : result.ZeiZan5Cho = 0
                    End If
                End If
            End If
        Else
            If flHasseiYokuki Then
                result.LsryoZan = lsryoYokuCf
                result.ZeiZan = zeiYokuCf
                result.LsryoZan1Nai = lsryoYoku1NaiCf
                result.LsryoZan2Nai = lsryoYoku2NaiCf
                result.LsryoZan3Nai = lsryoYoku3NaiCf
                result.LsryoZan4Nai = lsryoYoku4NaiCf
                result.LsryoZan5Nai = lsryoYoku5NaiCf
                result.LsryoZan5Cho = lsryoYoku5ChoCf
                result.ZeiZan1Nai = zeiYoku1NaiCf
                result.ZeiZan2Nai = zeiYoku2NaiCf
                result.ZeiZan3Nai = zeiYoku3NaiCf
                result.ZeiZan4Nai = zeiYoku4NaiCf
                result.ZeiZan5Nai = zeiYoku5NaiCf
                result.ZeiZan5Cho = zeiYoku5ChoCf
                If (Not flHasseiZenCf) AndAlso (Not flHasseiTokiCf) Then
                    If startDt > kimatDt Then
                        result.LsryoZan = 0.0 : result.ZeiZan = 0.0
                        result.LsryoZan1Nai = 0 : result.LsryoZan2Nai = 0 : result.LsryoZan3Nai = 0
                        result.LsryoZan4Nai = 0 : result.LsryoZan5Nai = 0 : result.LsryoZan5Cho = 0
                        result.ZeiZan1Nai = 0 : result.ZeiZan2Nai = 0 : result.ZeiZan3Nai = 0
                        result.ZeiZan4Nai = 0 : result.ZeiZan5Nai = 0 : result.ZeiZan5Cho = 0
                    End If
                End If
            Else
                result.LsryoZan = 0.0
                result.ZeiZan = 0.0
            End If
        End If

        ' 当期額・総額・月別
        result.LsryoToki = lsryoToki
        result.LsryoTotal = lsryoTotalCf
        result.ZeiToki = zeiToki
        result.ZeiTotal = zeiTotalCf
        result.LsryoTokig = lsryoTokig
        result.ZeiTokig = zeiTokig

        ' TAISHO_F (一覧表示対象フラグ)
        result.TaishoF = False
        If startDt <= kimatDt AndAlso rendDt >= kishuDt Then
            result.TaishoF = True
        Else
            If flHasseiToki Then
                result.TaishoF = True
            Else
                If rendDt < kishuDt Then
                    If flHasseiYokuki Then result.TaishoF = True
                ElseIf startDt > kimatDt Then
                    If flHasseiZen Then result.TaishoF = True
                End If
            End If
        End If

        Return result
    End Function

    ' --------------------------------------------------
    ' 結果DataTable作成
    ' --------------------------------------------------
    Private Function CreateResultTable(gCnt As Integer) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("kykm_id", GetType(Double))
        dt.Columns.Add("kykh_id", GetType(Double))
        dt.Columns.Add("物件No", GetType(Double))
        dt.Columns.Add("再回", GetType(Integer))
        dt.Columns.Add("配No", GetType(Integer))
        dt.Columns.Add("契区", GetType(String))
        dt.Columns.Add("行区", GetType(String))
        dt.Columns.Add("計上区分", GetType(String))
        dt.Columns.Add("法令区分", GetType(String))
        dt.Columns.Add("取引区分", GetType(String))
        dt.Columns.Add("リース区分", GetType(String))
        dt.Columns.Add("契約番号", GetType(String))
        dt.Columns.Add("支払先", GetType(String))
        dt.Columns.Add("物件名", GetType(String))
        dt.Columns.Add("管理部署", GetType(String))
        dt.Columns.Add("費用負担部署", GetType(String))
        dt.Columns.Add("費用区分", GetType(String))
        dt.Columns.Add("開始日", GetType(Date))
        dt.Columns.Add("終了日", GetType(Date))
        dt.Columns.Add("中途解約日", GetType(Date))
        dt.Columns.Add("請求月", GetType(String))
        dt.Columns.Add("回数済/総", GetType(String))
        dt.Columns.Add("現金購入価額_物件", GetType(Double))
        dt.Columns.Add("総支払額", GetType(Double))
        dt.Columns.Add("前期末残高", GetType(Double))
        dt.Columns.Add("当期額", GetType(Double))
        ' 月別列 G01〜G12
        For i As Integer = 1 To 12
            dt.Columns.Add($"G{i:D2}", GetType(Double))
        Next
        dt.Columns.Add("期末残高", GetType(Double))
        dt.Columns.Add("内1年内", GetType(Double))
        dt.Columns.Add("内2年内", GetType(Double))
        dt.Columns.Add("内3年内", GetType(Double))
        dt.Columns.Add("内4年内", GetType(Double))
        dt.Columns.Add("内5年内", GetType(Double))
        dt.Columns.Add("5年超", GetType(Double))
        dt.Columns.Add("期中増加", GetType(Double))
        Return dt
    End Function

    ' --------------------------------------------------
    ' 結果行追加 (Access版 mKLSRYO_SUB_OUTREC_SET)
    ' --------------------------------------------------
    Private Sub AddResultRow(resultDt As DataTable, sourceRow As DataRow,
        klsryo As KlsryoResult, recKbn As RecKbn, lcptId As Object,
        haifInfo As HaifInfo, sekouDt As Date)

        If Not klsryo.TaishoF Then Return

        Dim r = resultDt.NewRow()

        r("kykm_id") = sourceRow("kykm_kykm_id")
        r("kykh_id") = sourceRow("kykh_kykh_id")
        r("物件No") = sourceRow("kykm_kykm_no")
        r("再回") = If(IsDBNull(sourceRow("kykm_saikaisu")), DBNull.Value, sourceRow("kykm_saikaisu"))

        ' 配No
        If haifInfo IsNot Nothing Then
            r("配No") = haifInfo.LineId
        Else
            r("配No") = DBNull.Value
        End If

        ' 契区 (名称取得)
        r("契区") = GetNameFromMaster("SELECT kkbn_nm FROM c_kkbn WHERE kkbn_id = @id", sourceRow("kkbn_id"))

        ' 行区 (REC_KBN_STR)
        Select Case recKbn
            Case RecKbn.Teigaku : r("行区") = "定額"
            Case RecKbn.Hengaku : r("行区") = "変額"
            Case RecKbn.Fuzui : r("行区") = "付随費用"
        End Select

        ' 計上区分
        Dim kjkbnId As Object
        If recKbn <> RecKbn.Fuzui Then
            kjkbnId = sourceRow("kykm_kjkbn_id")
        Else
            kjkbnId = Kjkbn.Hiyo
        End If
        r("計上区分") = GetNameFromMaster("SELECT kjkbn_nm FROM c_kjkbn WHERE kjkbn_id = @id", kjkbnId)

        ' 法令区分 (Access版 gCalc法令判定 準拠)
        Dim refDt As Object = sourceRow("kyak_dt")
        If IsDBNull(refDt) Then
            refDt = sourceRow("start_dt")
        ElseIf Not IsDBNull(sourceRow("start_dt")) Then
            If CDate(sourceRow("start_dt")) < CDate(refDt) Then
                refDt = sourceRow("start_dt")
            End If
        End If
        If Not IsDBNull(refDt) Then
            If CDate(refDt) >= sekouDt Then
                r("法令区分") = "新法"
            Else
                r("法令区分") = "旧法"
            End If
        End If

        ' 取引区分
        If Not IsDBNull(kjkbnId) Then
            Select Case CInt(kjkbnId)
                Case Kjkbn.Hiyo : r("取引区分") = "費用計上"
                Case Kjkbn.Sisan : r("取引区分") = "資産計上"
            End Select
        End If

        ' リース区分
        r("リース区分") = GetNameFromMaster("SELECT leakbn_nm FROM c_leakbn WHERE leakbn_id = @id", sourceRow("leakbn_id"))

        r("契約番号") = If(IsDBNull(sourceRow("kykbnl")), DBNull.Value, sourceRow("kykbnl"))
        r("支払先") = GetNameFromMaster("SELECT lcpt1_nm FROM m_lcpt WHERE lcpt_id = @id", lcptId)
        r("物件名") = If(IsDBNull(sourceRow("bukn_nm")), DBNull.Value, sourceRow("bukn_nm"))
        r("管理部署") = GetNameFromMaster("SELECT bcat1_nm FROM m_bcat WHERE bcat_id = @id", sourceRow("b_bcat_id"))

        ' 費用負担部署
        If haifInfo IsNot Nothing Then
            r("費用負担部署") = GetNameFromMaster("SELECT bcat1_nm FROM m_bcat WHERE bcat_id = @id", haifInfo.HBcatId)
            r("費用区分") = GetNameFromMaster("SELECT hkmk_nm FROM m_hkmk WHERE hkmk_id = @id", haifInfo.HkmkId)
        End If

        r("開始日") = If(IsDBNull(sourceRow("start_dt")), DBNull.Value, sourceRow("start_dt"))
        r("終了日") = If(IsDBNull(sourceRow("end_dt")), DBNull.Value, sourceRow("end_dt"))
        r("中途解約日") = If(IsDBNull(sourceRow("ckaiyk_dt")), DBNull.Value, sourceRow("ckaiyk_dt"))

        ' 回数済/総
        Dim souStr = If(klsryo.Soukaisu Is Nothing, "?", klsryo.Soukaisu.ToString())
        Dim sumiStr = If(klsryo.Sumikaisu Is Nothing, "0", klsryo.Sumikaisu.ToString())
        r("回数済/総") = $"{sumiStr}/{souStr}"

        r("現金購入価額_物件") = GetDbl(sourceRow, "b_knyukn")
        r("総支払額") = klsryo.LsryoTotal

        ' 計算結果
        r("前期末残高") = If(klsryo.LsryoZzan Is Nothing, DBNull.Value, klsryo.LsryoZzan)
        r("当期額") = klsryo.LsryoToki

        ' 月別
        For i As Integer = 0 To 11
            If klsryo.LsryoTokig(i) Is Nothing Then
                r($"G{i + 1:D2}") = DBNull.Value
            Else
                r($"G{i + 1:D2}") = CDbl(klsryo.LsryoTokig(i))
            End If
        Next

        r("期末残高") = If(klsryo.LsryoZan Is Nothing, DBNull.Value, klsryo.LsryoZan)
        r("内1年内") = klsryo.LsryoZan1Nai
        r("内2年内") = klsryo.LsryoZan2Nai
        r("内3年内") = klsryo.LsryoZan3Nai
        r("内4年内") = klsryo.LsryoZan4Nai
        r("内5年内") = klsryo.LsryoZan5Nai
        r("5年超") = klsryo.LsryoZan5Cho
        r("期中増加") = If(klsryo.LsryoZou Is Nothing, DBNull.Value, klsryo.LsryoZou)

        resultDt.Rows.Add(r)
    End Sub

    ''' <summary>付随費用用の結果行追加</summary>
    Private Sub AddHenfResultRow(resultDt As DataTable, henfRow As DataRow,
        klsryo As KlsryoResult, haifInfo As HaifInfo, sekouDt As Date)

        If Not klsryo.TaishoF Then Return

        Dim r = resultDt.NewRow()

        r("kykm_id") = henfRow("kykm_id")
        r("kykh_id") = henfRow("kykh_kykh_id")
        r("物件No") = henfRow("kykm_kykm_no")
        r("再回") = If(IsDBNull(henfRow("kykm_saikaisu")), DBNull.Value, henfRow("kykm_saikaisu"))
        r("配No") = If(haifInfo IsNot Nothing, CObj(haifInfo.LineId), DBNull.Value)
        r("契区") = "保守"
        r("行区") = "付随費用"
        r("計上区分") = GetNameFromMaster("SELECT kjkbn_nm FROM c_kjkbn WHERE kjkbn_id = @id", Kjkbn.Hiyo)

        ' 法令区分
        Dim refDt As Object = henfRow("kyak_dt")
        If IsDBNull(refDt) Then
            refDt = henfRow("start_dt")
        ElseIf Not IsDBNull(henfRow("start_dt")) Then
            If CDate(henfRow("start_dt")) < CDate(refDt) Then
                refDt = henfRow("start_dt")
            End If
        End If
        If Not IsDBNull(refDt) Then
            r("法令区分") = If(CDate(refDt) >= sekouDt, "新法", "旧法")
        End If
        r("取引区分") = "費用計上"

        r("リース区分") = GetNameFromMaster("SELECT leakbn_nm FROM c_leakbn WHERE leakbn_id = @id", henfRow("leakbn_id"))
        r("契約番号") = If(IsDBNull(henfRow("kykbnf")), DBNull.Value, henfRow("kykbnf"))
        r("支払先") = GetNameFromMaster("SELECT lcpt1_nm FROM m_lcpt WHERE lcpt_id = @id", henfRow("f_lcpt_id"))
        r("物件名") = If(IsDBNull(henfRow("bukn_nm")), DBNull.Value, henfRow("bukn_nm"))
        r("管理部署") = GetNameFromMaster("SELECT bcat1_nm FROM m_bcat WHERE bcat_id = @id", henfRow("b_bcat_id"))

        If haifInfo IsNot Nothing Then
            r("費用負担部署") = GetNameFromMaster("SELECT bcat1_nm FROM m_bcat WHERE bcat_id = @id", haifInfo.HBcatId)
            r("費用区分") = GetNameFromMaster("SELECT hkmk_nm FROM c_hkmk WHERE hkmk_id = @id", haifInfo.HkmkId)
        Else
            r("費用区分") = GetNameFromMaster("SELECT hkmk_nm FROM c_hkmk WHERE hkmk_id = @id", henfRow("f_hkmk_id"))
        End If

        r("開始日") = If(IsDBNull(henfRow("start_dt")), DBNull.Value, henfRow("start_dt"))
        r("終了日") = If(IsDBNull(henfRow("end_dt")), DBNull.Value, henfRow("end_dt"))
        r("中途解約日") = If(IsDBNull(henfRow("ckaiyk_dt")), DBNull.Value, henfRow("ckaiyk_dt"))

        Dim souStr = If(klsryo.Soukaisu Is Nothing, "?", klsryo.Soukaisu.ToString())
        Dim sumiStr = If(klsryo.Sumikaisu Is Nothing, "0", klsryo.Sumikaisu.ToString())
        r("回数済/総") = $"{sumiStr}/{souStr}"

        r("現金購入価額_物件") = GetDbl(henfRow, "b_knyukn")
        r("総支払額") = klsryo.LsryoTotal
        r("前期末残高") = If(klsryo.LsryoZzan Is Nothing, DBNull.Value, klsryo.LsryoZzan)
        r("当期額") = klsryo.LsryoToki

        For i As Integer = 0 To 11
            If klsryo.LsryoTokig(i) Is Nothing Then
                r($"G{i + 1:D2}") = DBNull.Value
            Else
                r($"G{i + 1:D2}") = CDbl(klsryo.LsryoTokig(i))
            End If
        Next

        r("期末残高") = If(klsryo.LsryoZan Is Nothing, DBNull.Value, klsryo.LsryoZan)
        r("内1年内") = klsryo.LsryoZan1Nai
        r("内2年内") = klsryo.LsryoZan2Nai
        r("内3年内") = klsryo.LsryoZan3Nai
        r("内4年内") = klsryo.LsryoZan4Nai
        r("内5年内") = klsryo.LsryoZan5Nai
        r("5年超") = klsryo.LsryoZan5Cho
        r("期中増加") = If(klsryo.LsryoZou Is Nothing, DBNull.Value, klsryo.LsryoZou)

        resultDt.Rows.Add(r)
    End Sub

    ' --------------------------------------------------
    ' ヘルパー
    ' --------------------------------------------------
    Private Function GetSekouDt() As Date
        Try
            Dim dt = _crud.GetDataTable("SELECT val_datetime FROM t_settei WHERE settei_nm = 'SEKOU_DT'")
            If dt.Rows.Count > 0 Then Return CDate(dt.Rows(0)("val_datetime"))
        Catch
        End Try
        Return New Date(2008, 4, 1) ' デフォルト施行日
    End Function

    Private Function GetNameFromMaster(sql As String, id As Object) As Object
        If id Is Nothing OrElse IsDBNull(id) Then Return DBNull.Value
        Try
            Dim prms As New List(Of NpgsqlParameter)
            prms.Add(New NpgsqlParameter("@id", id))
            Dim dt = _crud.GetDataTable(sql, prms)
            If dt.Rows.Count > 0 Then Return dt.Rows(0)(0)
        Catch
        End Try
        Return DBNull.Value
    End Function

    Private Shared Function GetDbl(row As DataRow, colName As String) As Double
        If IsDBNull(row(colName)) Then Return 0.0
        Return CDbl(row(colName))
    End Function

End Class
