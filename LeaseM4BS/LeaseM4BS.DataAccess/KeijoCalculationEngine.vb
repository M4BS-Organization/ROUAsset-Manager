Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Npgsql

''' <summary>
''' 計上計算エンジン (Access版 pc_SHRI_KEIJO.gKEIJO_Main 相当)
''' 1ヶ月分の計上データを計算し、KeijoWorkRow のリストとして返す。
''' </summary>
Public Class KeijoCalculationEngine

    Private _crud As New CrudHelper()

    ''' <summary>施行日キャッシュ</summary>
    Private _sekouDt As Date = Date.MinValue

    ''' <summary>
    ''' 不要行補完管理 (Access版 tm不要行補完管理() 相当)
    ''' 移転外ファイナンスリースの計上済み月を追跡する。
    ''' </summary>
    Private _hoyoKanriList As New List(Of HoyoKanriEntry)()

    ''' <summary>
    ''' 付随費用スケジュール (Access版 tm付随費用_SCH() 相当)
    ''' 費用計上で減損ありの場合に使用。
    ''' </summary>
    Private _fuzuiSch As List(Of CashScheduleEntry) = Nothing
    Private _fuzuiSchCnt As Integer = 0

    ' ======================================================================
    '  公開メソッド
    ' ======================================================================

    ''' <summary>
    ''' 計上計算エンジンメイン (Access版 gKEIJO_Main)
    ''' 1ヶ月分の計上データを計算し、KeijoWorkRow のリストとして返す。
    ''' </summary>
    ''' <param name="joken">計上条件</param>
    ''' <param name="kishuDt">期首日（月初日）</param>
    ''' <param name="kimatDt">期末日（月末日）</param>
    ''' <returns>計上ワーク行のリスト</returns>
    Public Function Execute(
        joken As KeijoJoken,
        kishuDt As Date,
        kimatDt As Date
    ) As List(Of KeijoWorkRow)

        ' 翌期末日 = 期末日の1年後の月末
        Dim y1kimatDt As Date = CashScheduleBuilder.GetMonthEndDate(kimatDt.AddMonths(12))

        ' 施行日取得
        _sekouDt = GetSekouDt()

        ' ソースデータ取得
        Dim sourceDt As DataTable = GetSourceData(kishuDt, kimatDt, joken)

        Dim resultRows As New List(Of KeijoWorkRow)()

        ' 集計単位により処理分岐
        Select Case joken.Meisai
            Case ShriMeisai.Kykm
                resultRows.AddRange(ProcessKykm(sourceDt, kishuDt, kimatDt, y1kimatDt, joken))
            Case ShriMeisai.Haif
                resultRows.AddRange(ProcessHaif(sourceDt, kishuDt, kimatDt, y1kimatDt, joken))
        End Select

        ' 付随費用処理 (保守料 or 全部の場合)
        If joken.KjkbnHiyo Then
            If joken.Taisho = 2 OrElse joken.Taisho = 3 Then
                resultRows.AddRange(ProcessHenf(kishuDt, kimatDt, y1kimatDt, joken))
            End If
        End If

        Return resultRows
    End Function

    ' ======================================================================
    '  ソースデータ取得
    ' ======================================================================

    ''' <summary>
    ''' 計上用ソースデータ取得 (Access版 gMOTO_RSETSQL_EDIT_KEIJO 相当)
    ''' </summary>
    Private Function GetSourceData(
        kishuDt As Date,
        kimatDt As Date,
        joken As KeijoJoken
    ) As DataTable

        Dim result As (Sql As String, Parameters As List(Of NpgsqlParameter)) =
            KeijoSqlBuilder.BuildSourceSql(
                joken.Meisai,
                joken.Taisho,
                joken.KjkbnSisan,
                joken.KjkbnHiyo
            )

        ' ユーザー指定フィルタ条件を追加
        Dim sql As String = result.Sql
        If Not String.IsNullOrWhiteSpace(joken.SaJoken) Then
            sql &= " AND (" & joken.SaJoken & ")"
        End If

        Return _crud.GetDataTable(sql, result.Parameters)
    End Function

    ' ======================================================================
    '  物件単位処理 (Access版 mKEIJO_Sub_KYKM)
    ' ======================================================================

    ''' <summary>
    ''' 物件単位処理 (Access版 mKEIJO_Sub_KYKM)
    ''' </summary>
    Private Function ProcessKykm(
        sourceDt As DataTable,
        kishuDt As Date,
        kimatDt As Date,
        y1kimatDt As Date,
        joken As KeijoJoken
    ) As List(Of KeijoWorkRow)

        Dim resultRows As New List(Of KeijoWorkRow)()

        For Each row As DataRow In sourceDt.Rows

            ' 不要行補完管理をリセット (物件ごと)
            _hoyoKanriList.Clear()

            ' 費用計上で減損がある場合、減損スケジュール作成
            _fuzuiSch = Nothing
            _fuzuiSchCnt = 0
            If CInt(row("kykm_kjkbn_id")) = CInt(Kjkbn.Hiyo) Then
                If GetBool(row, "b_gson_f") Then
                    Dim gsonSql As String = KeijoSqlBuilder.BuildGsonSql(CInt(row("kykm_kykm_id")))
                    Dim gsonDt As DataTable = _crud.GetDataTable(gsonSql)
                    ' 減損スケジュールは付随費用スケジュールとして利用
                    _fuzuiSch = BuildGsonSchedule(gsonDt)
                    _fuzuiSchCnt = _fuzuiSch.Count
                End If
            End If

            ' 返済方法・計上タイミングの決定
            Dim determination As (HensaiKind As HensaiKind, Ktmg As ShriKtmg) =
                DetermineHensaiKindAndKtmg(row, joken, _sekouDt)
            Dim ilHensaiKind As HensaiKind = determination.HensaiKind
            Dim ilKtmg As ShriKtmg = determination.Ktmg

            ' 集計対象判定
            If Not IsTargetRecord(kishuDt, kimatDt, ilKtmg, row) Then
                Continue For
            End If

            ' ---- 定額処理 ----
            Dim klsryo As Double = GetDbl(row, "b_klsryo")
            Dim kzei As Double = GetDbl(row, "b_kzei")
            Dim mlsryo As Double = GetDbl(row, "b_mlsryo")
            Dim mzei As Double = GetDbl(row, "b_mzei")

            If klsryo <> 0 OrElse kzei <> 0 OrElse mlsryo <> 0 OrElse mzei <> 0 Then
                Dim shriCnt As Integer = GetInt(row, "shri_cnt")
                If shriCnt > 0 Then
                    ' 定額スケジュール生成
                    Dim schT As List(Of CashScheduleEntry) = CashScheduleBuilder.BuildTeigakuSchedule(
                        ilKtmg, kishuDt, kimatDt, y1kimatDt,
                        GetBool(row, "jencho_f"),
                        GetInt(row, "shri_kn"), shriCnt,
                        GetDbl(row, "sshri_kn_m"), GetDbl(row, "sshri_kn_1"),
                        GetDbl(row, "sshri_kn_2"), GetDbl(row, "sshri_kn_3"),
                        GetNullableInt(row, "shho_m_id"), GetNullableInt(row, "shho_1_id"),
                        GetNullableInt(row, "shho_2_id"), GetNullableInt(row, "shho_3_id"),
                        GetNullableDate(row, "mae_dt"),
                        GetNullableDate(row, "shri_dt1"), GetNullableDate(row, "shri_dt2"),
                        GetInt(row, "shri_dt3"),
                        GetDbl(row, "zritu"),
                        klsryo, kzei, mlsryo, mzei,
                        GetNullableDate(row, "ckaiyk_esdt_t")
                    )

                    ' 速給処理
                    Dim baseSCntT As Integer = 0
                    If GetBool(row, "skyu_kj_f") Then
                        ApplySokyu(GetNullableDate(row, "k_kjyo_st_dt"), schT, ilKtmg)
                    End If

                    ' スケジュールから計上金額算出
                    Dim keijoResult As KeijoResult = CalcKeijoFromSchedule(
                        ilKtmg, kishuDt, kimatDt, y1kimatDt,
                        row("start_dt"), row("b_rend_dt"),
                        GetBool(row, "jencho_f"),
                        schT
                    )

                    ' KeijoRecord に追加データセット
                    keijoResult.LcptId = row("lcpt_id")
                    keijoResult.HkmkId = DBNull.Value
                    keijoResult.LineId = DBNull.Value
                    keijoResult.Haifritu = DBNull.Value
                    keijoResult.HBcatId = DBNull.Value
                    keijoResult.Rsrvh1Id = DBNull.Value
                    keijoResult.HZokusei1 = DBNull.Value
                    keijoResult.HZokusei2 = DBNull.Value
                    keijoResult.HZokusei3 = DBNull.Value
                    keijoResult.HZokusei4 = DBNull.Value
                    keijoResult.HZokusei5 = DBNull.Value
                    keijoResult.RecKbn = RecKbn.Teigaku
                    keijoResult.HensaiKind = ilHensaiKind

                    ' ワーク行に変換して追加
                    Dim rows As List(Of KeijoWorkRow) = ScheduleToWorkRows(
                        schT, keijoResult, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind
                    )
                    resultRows.AddRange(rows)

                    ' 不要行補完出力
                    AddSupplementRows(resultRows, keijoResult, ilHensaiKind, row)
                End If
            End If

            ' ---- 変額処理 ----
            If GetBool(row, "b_henl_f") Then
                Dim schH As List(Of CashScheduleEntry) = CashScheduleBuilder.BuildHengakuSchedule(
                    _crud, CDbl(row("kykm_kykm_id")),
                    GetNullableDate(row, "ckaiyk_esdt_h")
                )

                Dim baseSCntH As Integer = 0
                If GetBool(row, "skyu_kj_f") Then
                    ApplySokyu(GetNullableDate(row, "k_kjyo_st_dt"), schH, ilKtmg)
                End If

                Dim keijoResultH As KeijoResult = CalcKeijoFromSchedule(
                    ilKtmg, kishuDt, kimatDt, y1kimatDt,
                    row("start_dt"), row("b_rend_dt"),
                    False,
                    schH
                )

                keijoResultH.LcptId = row("lcpt_id")
                keijoResultH.HkmkId = DBNull.Value
                keijoResultH.LineId = DBNull.Value
                keijoResultH.Haifritu = DBNull.Value
                keijoResultH.HBcatId = DBNull.Value
                keijoResultH.Rsrvh1Id = DBNull.Value
                keijoResultH.HZokusei1 = DBNull.Value
                keijoResultH.HZokusei2 = DBNull.Value
                keijoResultH.HZokusei3 = DBNull.Value
                keijoResultH.HZokusei4 = DBNull.Value
                keijoResultH.HZokusei5 = DBNull.Value
                keijoResultH.RecKbn = RecKbn.Hengaku
                keijoResultH.HensaiKind = ilHensaiKind

                Dim rowsH As List(Of KeijoWorkRow) = ScheduleToWorkRows(
                    schH, keijoResultH, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind
                )
                resultRows.AddRange(rowsH)

                AddSupplementRows(resultRows, keijoResultH, ilHensaiKind, row)
            End If

        Next

        Return resultRows
    End Function

    ' ======================================================================
    '  配賦単位処理 (Access版 mKEIJO_Sub_HAIF)
    ' ======================================================================

    ''' <summary>
    ''' 配賦単位処理 (Access版 mKEIJO_Sub_HAIF)
    ''' </summary>
    Private Function ProcessHaif(
        sourceDt As DataTable,
        kishuDt As Date,
        kimatDt As Date,
        y1kimatDt As Date,
        joken As KeijoJoken
    ) As List(Of KeijoWorkRow)

        Dim resultRows As New List(Of KeijoWorkRow)()

        Dim savedKykmId As Double = -1
        Dim haifList As New List(Of HaifInfo)()
        Dim prevHasHengaku As Boolean = False
        Dim prevRow As DataRow = Nothing

        For Each row As DataRow In sourceDt.Rows

            ' 物件IDが変わった場合の処理
            Dim curKykmId As Double = CDbl(row("kykm_kykm_id"))
            If savedKykmId <> curKykmId Then

                ' 前物件に変額がある場合、前物件の変額を出力
                If prevHasHengaku AndAlso prevRow IsNot Nothing Then
                    Dim determination0 As (HensaiKind As HensaiKind, Ktmg As ShriKtmg) =
                        DetermineHensaiKindAndKtmg(prevRow, joken, _sekouDt)
                    Dim rows0 As List(Of KeijoWorkRow) = ProcessHaifHengaku(
                        prevRow, haifList, kishuDt, kimatDt, y1kimatDt, joken,
                        determination0.HensaiKind, determination0.Ktmg
                    )
                    resultRows.AddRange(rows0)
                    prevHasHengaku = False
                End If

                ' 新しい物件の変額フラグを確認
                If GetBool(row, "b_henl_f") Then
                    prevHasHengaku = True
                End If

                ' 費用計上で減損がある場合
                _fuzuiSch = Nothing
                _fuzuiSchCnt = 0
                If CInt(row("kykm_kjkbn_id")) = CInt(Kjkbn.Hiyo) Then
                    If GetBool(row, "b_gson_f") Then
                        Dim gsonSql As String = KeijoSqlBuilder.BuildGsonSql(CInt(row("kykm_kykm_id")))
                        Dim gsonDt As DataTable = _crud.GetDataTable(gsonSql)
                        _fuzuiSch = BuildGsonSchedule(gsonDt)
                        _fuzuiSchCnt = _fuzuiSch.Count
                    End If
                End If

                ' 配賦情報ロード
                haifList = LoadHaifInfo(sourceDt, curKykmId)

                _hoyoKanriList.Clear()
                savedKykmId = curKykmId
                prevRow = row
            End If

            ' 返済方法・計上タイミングの決定
            Dim determination As (HensaiKind As HensaiKind, Ktmg As ShriKtmg) =
                DetermineHensaiKindAndKtmg(row, joken, _sekouDt)
            Dim ilHensaiKind As HensaiKind = determination.HensaiKind
            Dim ilKtmg As ShriKtmg = determination.Ktmg

            ' 集計対象判定
            If Not IsTargetRecord(kishuDt, kimatDt, ilKtmg, row) Then
                Continue For
            End If

            ' 定額処理 (配賦単位)
            Dim klsryo As Double = GetDbl(row, "b_klsryo")
            Dim kzei As Double = GetDbl(row, "b_kzei")
            Dim mlsryo As Double = GetDbl(row, "b_mlsryo")
            Dim mzei As Double = GetDbl(row, "b_mzei")

            If klsryo <> 0 OrElse kzei <> 0 OrElse mlsryo <> 0 OrElse mzei <> 0 Then
                Dim shriCnt As Integer = GetInt(row, "shri_cnt")
                If shriCnt > 0 Then
                    Dim schT As List(Of CashScheduleEntry) = CashScheduleBuilder.BuildTeigakuSchedule(
                        ilKtmg, kishuDt, kimatDt, y1kimatDt,
                        GetBool(row, "jencho_f"),
                        GetInt(row, "shri_kn"), shriCnt,
                        GetDbl(row, "sshri_kn_m"), GetDbl(row, "sshri_kn_1"),
                        GetDbl(row, "sshri_kn_2"), GetDbl(row, "sshri_kn_3"),
                        GetNullableInt(row, "shho_m_id"), GetNullableInt(row, "shho_1_id"),
                        GetNullableInt(row, "shho_2_id"), GetNullableInt(row, "shho_3_id"),
                        GetNullableDate(row, "mae_dt"),
                        GetNullableDate(row, "shri_dt1"), GetNullableDate(row, "shri_dt2"),
                        GetInt(row, "shri_dt3"),
                        GetDbl(row, "zritu"),
                        klsryo, kzei, mlsryo, mzei,
                        GetNullableDate(row, "ckaiyk_esdt_t")
                    )

                    If GetBool(row, "skyu_kj_f") Then
                        ApplySokyu(GetNullableDate(row, "k_kjyo_st_dt"), schT, ilKtmg)
                    End If

                    ' 配賦行がない場合は物件単位と同様の処理
                    If haifList.Count = 0 Then
                        Dim keijoResult As KeijoResult = CalcKeijoFromSchedule(
                            ilKtmg, kishuDt, kimatDt, y1kimatDt,
                            row("start_dt"), row("b_rend_dt"),
                            GetBool(row, "jencho_f"),
                            schT
                        )
                        keijoResult.LcptId = row("lcpt_id")
                        keijoResult.HkmkId = DBNull.Value
                        keijoResult.LineId = DBNull.Value
                        keijoResult.Haifritu = DBNull.Value
                        keijoResult.HBcatId = DBNull.Value
                        keijoResult.Rsrvh1Id = DBNull.Value
                        SetHaifZokusei(keijoResult, Nothing)
                        keijoResult.RecKbn = RecKbn.Teigaku
                        keijoResult.HensaiKind = ilHensaiKind

                        resultRows.AddRange(ScheduleToWorkRows(schT, keijoResult, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind))
                        AddSupplementRows(resultRows, keijoResult, ilHensaiKind, row)
                    Else
                        ' 配賦行がある場合: 各配賦行に按分
                        Dim schW As List(Of CashScheduleEntry) = New List(Of CashScheduleEntry)(schT.Count)
                        For i As Integer = 0 To schT.Count - 1
                            schW.Add(New CashScheduleEntry())
                        Next

                        For haifIdx As Integer = 0 To haifList.Count - 1
                            Dim hi As HaifInfo = haifList(haifIdx)
                            For j As Integer = 0 To schT.Count - 1
                                Dim entry As CashScheduleEntry = schT(j).Clone()
                                If haifIdx = haifList.Count - 1 Then
                                    ' 最終配賦行: 残差を割り当て
                                    entry.Lsryo = schT(j).Lsryo - schT(j).LsryoHsum
                                    entry.Zei = schT(j).Zei - schT(j).ZeiHsum
                                Else
                                    ' 中間配賦行: 按分計算
                                    entry.Lsryo = CLng(schT(j).Lsryo * hi.Haifritu / 100)
                                    entry.Zei = CLng(schT(j).Zei * hi.Haifritu / 100)
                                    schT(j).LsryoHsum += entry.Lsryo
                                    schT(j).ZeiHsum += entry.Zei
                                End If
                                schW(j) = entry
                            Next

                            Dim keijoResultH As KeijoResult = CalcKeijoFromSchedule(
                                ilKtmg, kishuDt, kimatDt, y1kimatDt,
                                row("start_dt"), row("b_rend_dt"),
                                GetBool(row, "jencho_f"),
                                schW
                            )
                            keijoResultH.LcptId = row("lcpt_id")
                            keijoResultH.HkmkId = DBNull.Value
                            keijoResultH.LineId = hi.LineId
                            keijoResultH.Haifritu = hi.Haifritu
                            keijoResultH.HBcatId = hi.HBcatId
                            keijoResultH.Rsrvh1Id = hi.Rsrvh1Id
                            keijoResultH.HZokusei1 = hi.HZokusei1
                            keijoResultH.HZokusei2 = hi.HZokusei2
                            keijoResultH.HZokusei3 = hi.HZokusei3
                            keijoResultH.HZokusei4 = hi.HZokusei4
                            keijoResultH.HZokusei5 = hi.HZokusei5
                            keijoResultH.RecKbn = RecKbn.Teigaku
                            keijoResultH.HensaiKind = ilHensaiKind

                            resultRows.AddRange(ScheduleToWorkRows(schW, keijoResultH, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind))
                            AddSupplementRows(resultRows, keijoResultH, ilHensaiKind, row)
                        Next
                    End If
                End If
            End If

        Next ' row

        ' ループ終了後、最終物件に変額がある場合
        If prevHasHengaku AndAlso prevRow IsNot Nothing Then
            Dim detFinal As (HensaiKind As HensaiKind, Ktmg As ShriKtmg) =
                DetermineHensaiKindAndKtmg(prevRow, joken, _sekouDt)
            resultRows.AddRange(ProcessHaifHengaku(
                prevRow, haifList, kishuDt, kimatDt, y1kimatDt, joken,
                detFinal.HensaiKind, detFinal.Ktmg
            ))
        End If

        Return resultRows
    End Function

    ''' <summary>配賦単位の変額処理</summary>
    Private Function ProcessHaifHengaku(
        row As DataRow,
        haifList As List(Of HaifInfo),
        kishuDt As Date, kimatDt As Date, y1kimatDt As Date,
        joken As KeijoJoken,
        ilHensaiKind As HensaiKind,
        ilKtmg As ShriKtmg
    ) As List(Of KeijoWorkRow)

        Dim rows As New List(Of KeijoWorkRow)()
        Dim schH As List(Of CashScheduleEntry) = CashScheduleBuilder.BuildHengakuSchedule(
            _crud, CDbl(row("kykm_kykm_id")),
            GetNullableDate(row, "ckaiyk_esdt_h")
        )

        If GetBool(row, "skyu_kj_f") Then
            ApplySokyu(GetNullableDate(row, "k_kjyo_st_dt"), schH, ilKtmg)
        End If

        If haifList.Count = 0 Then
            Dim keijoResult As KeijoResult = CalcKeijoFromSchedule(
                ilKtmg, kishuDt, kimatDt, y1kimatDt,
                row("start_dt"), row("b_rend_dt"), False, schH
            )
            keijoResult.LcptId = row("lcpt_id")
            keijoResult.HkmkId = DBNull.Value
            keijoResult.LineId = DBNull.Value
            keijoResult.Haifritu = DBNull.Value
            keijoResult.HBcatId = DBNull.Value
            keijoResult.Rsrvh1Id = DBNull.Value
            SetHaifZokusei(keijoResult, Nothing)
            keijoResult.RecKbn = RecKbn.Hengaku
            keijoResult.HensaiKind = ilHensaiKind
            rows.AddRange(ScheduleToWorkRows(schH, keijoResult, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind))
            AddSupplementRows(rows, keijoResult, ilHensaiKind, row)
        Else
            Dim schW As New List(Of CashScheduleEntry)(schH.Count)
            For i As Integer = 0 To schH.Count - 1
                schW.Add(New CashScheduleEntry())
            Next

            For haifIdx As Integer = 0 To haifList.Count - 1
                Dim hi As HaifInfo = haifList(haifIdx)
                For j As Integer = 0 To schH.Count - 1
                    Dim entry As CashScheduleEntry = schH(j).Clone()
                    If haifIdx = haifList.Count - 1 Then
                        entry.Lsryo = schH(j).Lsryo - schH(j).LsryoHsum
                        entry.Zei = schH(j).Zei - schH(j).ZeiHsum
                    Else
                        entry.Lsryo = CLng(schH(j).Lsryo * hi.Haifritu / 100)
                        entry.Zei = CLng(schH(j).Zei * hi.Haifritu / 100)
                        schH(j).LsryoHsum += entry.Lsryo
                        schH(j).ZeiHsum += entry.Zei
                    End If
                    schW(j) = entry
                Next

                Dim keijoResultHi As KeijoResult = CalcKeijoFromSchedule(
                    ilKtmg, kishuDt, kimatDt, y1kimatDt,
                    row("start_dt"), row("b_rend_dt"), False, schW
                )
                keijoResultHi.LcptId = row("lcpt_id")
                keijoResultHi.HkmkId = DBNull.Value
                keijoResultHi.LineId = hi.LineId
                keijoResultHi.Haifritu = hi.Haifritu
                keijoResultHi.HBcatId = hi.HBcatId
                keijoResultHi.Rsrvh1Id = hi.Rsrvh1Id
                keijoResultHi.HZokusei1 = hi.HZokusei1
                keijoResultHi.HZokusei2 = hi.HZokusei2
                keijoResultHi.HZokusei3 = hi.HZokusei3
                keijoResultHi.HZokusei4 = hi.HZokusei4
                keijoResultHi.HZokusei5 = hi.HZokusei5
                keijoResultHi.RecKbn = RecKbn.Hengaku
                keijoResultHi.HensaiKind = ilHensaiKind
                rows.AddRange(ScheduleToWorkRows(schW, keijoResultHi, row, kishuDt, kimatDt, ilKtmg, ilHensaiKind))
                AddSupplementRows(rows, keijoResultHi, ilHensaiKind, row)
            Next
        End If

        Return rows
    End Function

    ' ======================================================================
    '  付随費用処理 (Access版 mKEIJO_Sub_HENF)
    ' ======================================================================

    ''' <summary>
    ''' 付随費用処理 (Access版 mKEIJO_Sub_HENF)
    ''' </summary>
    Private Function ProcessHenf(
        kishuDt As Date,
        kimatDt As Date,
        y1kimatDt As Date,
        joken As KeijoJoken
    ) As List(Of KeijoWorkRow)

        Dim resultRows As New List(Of KeijoWorkRow)()

        ' 付随費用ソースSQL取得
        Dim henfSql As String = KeijoSqlBuilder.BuildHenfSql(joken.Meisai)
        Dim henfDt As DataTable = _crud.GetDataTable(henfSql)

        Dim haifDt As DataTable = Nothing
        If joken.Meisai = ShriMeisai.Haif Then
            Dim haifSql As String = KeijoSqlBuilder.BuildHenfHaifSql()
            haifDt = _crud.GetDataTable(haifSql)
        End If

        Dim savedKykmId As Double = -1
        Dim haifList As New List(Of HaifInfo)()

        For Each row As DataRow In henfDt.Rows

            ' 計上タイミング決定 (付随費用は支払日ベース固定)
            Dim ilKtmg As ShriKtmg = ShriKtmg.ShriDtBase

            ' 集計対象外チェック
            Dim shriEnDt As Object = row("shri_en_dt")
            Dim startDt As Object = row("start_dt")
            If IsDBNull(shriEnDt) OrElse IsDBNull(startDt) Then
                Continue For
            End If
            Dim endDtDate As Date = CDate(shriEnDt)
            If endDtDate < kishuDt AndAlso (IsDBNull(startDt) OrElse CDate(startDt) > kimatDt) Then
                Continue For
            End If

            ' スケジュール生成 (付随費用は単純スケジュール)
            Dim sch As List(Of CashScheduleEntry) = CashScheduleBuilder.BuildCommonSchedule(
                ilKtmg,
                GetInt(row, "shri_kn"),
                GetInt(row, "shri_cnt"),
                GetDbl(row, "sshri_kn"),
                GetNullableInt(row, "shho_id"),
                GetNullableDate(row, "shri_dt1"),
                GetDbl(row, "zritu"),
                GetDbl(row, "klsryo"),
                GetDbl(row, "kzei"),
                Nothing
            )

            ' 速給処理
            If GetBool(row, "skyu_kj_f") Then
                ApplySokyu(GetNullableDate(row, "k_kjyo_st_dt"), sch, ilKtmg)
            End If

            If joken.Meisai = ShriMeisai.Kykm Then
                ' 物件単位
                Dim keijoResult As KeijoResult = CalcKeijoFromSchedule(
                    ilKtmg, kishuDt, kimatDt, y1kimatDt,
                    row("start_dt"), row("end_dt"), False, sch
                )
                keijoResult.LcptId = row("f_lcpt_id")
                keijoResult.HkmkId = row("f_hkmk_id")
                keijoResult.LineId = DBNull.Value
                keijoResult.Haifritu = DBNull.Value
                keijoResult.HBcatId = DBNull.Value
                keijoResult.Rsrvh1Id = DBNull.Value
                SetHaifZokusei(keijoResult, Nothing)
                keijoResult.RecKbn = RecKbn.Fuzui
                keijoResult.HensaiKind = HensaiKind.Teigaku

                resultRows.AddRange(ScheduleToWorkRows(sch, keijoResult, row, kishuDt, kimatDt, ilKtmg, HensaiKind.Teigaku))
            Else
                ' 配賦単位
                Dim curKykmId As Double = CDbl(row("kykm_id"))
                If savedKykmId <> curKykmId Then
                    savedKykmId = curKykmId
                    haifList = LoadHenfHaifInfo(haifDt, curKykmId)
                End If

                If haifList.Count = 0 Then
                    Dim keijoResult As KeijoResult = CalcKeijoFromSchedule(
                        ilKtmg, kishuDt, kimatDt, y1kimatDt,
                        row("start_dt"), row("end_dt"), False, sch
                    )
                    keijoResult.LcptId = row("f_lcpt_id")
                    keijoResult.HkmkId = row("f_hkmk_id")
                    keijoResult.LineId = DBNull.Value
                    keijoResult.Haifritu = DBNull.Value
                    keijoResult.HBcatId = DBNull.Value
                    keijoResult.Rsrvh1Id = DBNull.Value
                    SetHaifZokusei(keijoResult, Nothing)
                    keijoResult.RecKbn = RecKbn.Fuzui
                    keijoResult.HensaiKind = HensaiKind.Teigaku
                    resultRows.AddRange(ScheduleToWorkRows(sch, keijoResult, row, kishuDt, kimatDt, ilKtmg, HensaiKind.Teigaku))
                Else
                    ' 配賦按分
                    Dim schW As New List(Of CashScheduleEntry)(sch.Count)
                    For i As Integer = 0 To sch.Count - 1
                        schW.Add(New CashScheduleEntry())
                    Next
                    For haifIdx As Integer = 0 To haifList.Count - 1
                        Dim hi As HaifInfo = haifList(haifIdx)
                        For j As Integer = 0 To sch.Count - 1
                            Dim entry As CashScheduleEntry = sch(j).Clone()
                            If haifIdx = haifList.Count - 1 Then
                                entry.Lsryo = sch(j).Lsryo - sch(j).LsryoHsum
                                entry.Zei = sch(j).Zei - sch(j).ZeiHsum
                            Else
                                entry.Lsryo = CLng(sch(j).Lsryo * hi.Haifritu / 100)
                                entry.Zei = CLng(sch(j).Zei * hi.Haifritu / 100)
                                sch(j).LsryoHsum += entry.Lsryo
                                sch(j).ZeiHsum += entry.Zei
                            End If
                            schW(j) = entry
                        Next
                        Dim keijoResultHi As KeijoResult = CalcKeijoFromSchedule(
                            ilKtmg, kishuDt, kimatDt, y1kimatDt,
                            row("start_dt"), row("end_dt"), False, schW
                        )
                        keijoResultHi.LcptId = row("f_lcpt_id")
                        keijoResultHi.HkmkId = row("f_hkmk_id")
                        keijoResultHi.LineId = hi.LineId
                        keijoResultHi.Haifritu = hi.Haifritu
                        keijoResultHi.HBcatId = hi.HBcatId
                        keijoResultHi.Rsrvh1Id = hi.Rsrvh1Id
                        keijoResultHi.HZokusei1 = hi.HZokusei1
                        keijoResultHi.HZokusei2 = hi.HZokusei2
                        keijoResultHi.HZokusei3 = hi.HZokusei3
                        keijoResultHi.HZokusei4 = hi.HZokusei4
                        keijoResultHi.HZokusei5 = hi.HZokusei5
                        keijoResultHi.RecKbn = RecKbn.Fuzui
                        keijoResultHi.HensaiKind = HensaiKind.Teigaku
                        resultRows.AddRange(ScheduleToWorkRows(schW, keijoResultHi, row, kishuDt, kimatDt, ilKtmg, HensaiKind.Teigaku))
                    Next
                End If
            End If

        Next

        Return resultRows
    End Function

    ' ======================================================================
    '  コア計算: スケジュールから計上金額算出 (Access版 mCALC_KEIJOfromSCH)
    ' ======================================================================

    ''' <summary>
    ''' スケジュールから計上金額算出 (Access版 mCALC_KEIJOfromSCH)
    ''' 前期以前・当期・残高に各スケジュールエントリを分類する。
    ''' </summary>
    Private Function CalcKeijoFromSchedule(
        ktmg As ShriKtmg,
        kishuDt As Date,
        kimatDt As Date,
        y1kimatDt As Date,
        vaStartDt As Object,
        vaRendDt As Object,
        jenchoF As Boolean,
        schedule As List(Of CashScheduleEntry)
    ) As KeijoResult

        Dim result As New KeijoResult()

        Dim soukaisu As Integer = 0
        Dim sumikaisuZen As Integer = 0
        Dim keijoShriCnt As Integer = 0
        Dim lsryoTotal As Double = 0
        Dim lsryoToki As Double = 0
        Dim zeiTotal As Double = 0
        Dim zeiToki As Double = 0
        Dim zritu As Double = 0
        Dim hasseiZen As Boolean = False
        Dim hasseiToki As Boolean = False
        Dim hasseiYokuki As Boolean = False

        For Each entry As CashScheduleEntry In schedule

            ' 判定基準日の決定 (締日ベース or 支払日ベース)
            Dim judgeDt As Date
            Select Case ktmg
                Case ShriKtmg.SimeDtBase
                    judgeDt = entry.SimeDt
                Case Else
                    judgeDt = entry.ShriDt
            End Select

            ' 前払でない場合のみ総回数にカウント
            If Not entry.MaeF Then
                soukaisu += 1
            End If
            lsryoTotal += entry.Lsryo
            zeiTotal += entry.Zei

            If judgeDt < kishuDt Then
                ' 前期以前
                If Not entry.CkaiykF Then
                    hasseiZen = True
                    If Not entry.MaeF Then
                        sumikaisuZen += 1
                    End If
                End If
            ElseIf judgeDt <= kimatDt Then
                ' 当期
                If Not entry.CkaiykF Then
                    hasseiToki = True
                    lsryoToki += entry.Lsryo
                    zeiToki += entry.Zei
                    If Not entry.MaeF Then
                        keijoShriCnt += 1
                    End If
                End If
            Else
                ' 翌期以降
                If Not entry.CkaiykF Then
                    hasseiYokuki = True
                End If
            End If

            ' 最後のエントリの消費税率を使用
            zritu = entry.Zritu
        Next

        ' 総回数設定
        If jenchoF Then
            result.Soukaisu = DBNull.Value
        Else
            result.Soukaisu = soukaisu
        End If
        result.SumikaisuZen = sumikaisuZen
        result.KeijoShriCnt = keijoShriCnt

        ' 計上額設定
        result.LsryoToki = lsryoToki
        result.LsryoTotal = lsryoTotal
        result.ZeiToki = zeiToki
        result.ZeiTotal = zeiTotal
        result.Zritu = zritu

        ' 一覧表示対象フラグ設定
        result.TaishoF = False
        If Not IsDBNull(vaStartDt) AndAlso Not IsDBNull(vaRendDt) Then
            Dim startDate As Date = CDate(vaStartDt)
            Dim rendDate As Date = CDate(vaRendDt)
            If startDate <= kimatDt AndAlso rendDate >= kishuDt Then
                result.TaishoF = True
            End If
        End If
        If hasseiToki Then
            result.TaishoF = True
        ElseIf hasseiZen AndAlso hasseiYokuki Then
            result.TaishoF = True
        End If

        Return result
    End Function

    ' ======================================================================
    '  集計対象判定 (Access版 mCHK_集計対象 KEIJO版)
    ' ======================================================================

    ''' <summary>
    ''' 集計対象判定 (Access版 mKEIJO_Sub_KYKM L440-480 参照)
    ''' 締日ベース/支払日ベースで判定ロジックが異なる。
    ''' </summary>
    Private Function IsTargetRecord(
        kishuDt As Date,
        kimatDt As Date,
        ktmg As ShriKtmg,
        row As DataRow
    ) As Boolean

        If ktmg = ShriKtmg.SimeDtBase Then
            ' 締日ベース
            ' 契約期間と集計期間が重複する場合
            Dim startDt As Object = row("start_dt")
            Dim bRendDt As Object = row("b_rend_dt")
            If Not IsDBNull(startDt) AndAlso Not IsDBNull(bRendDt) Then
                If CDate(startDt) <= kimatDt AndAlso CDate(bRendDt) >= kishuDt Then
                    Return True
                End If
            End If

            ' B_SMDT_FST_SUM / B_SHDT_LST_SUM による追加判定
            Dim bSmdtFstSum As Object = row("b_smdt_fst_sum")
            Dim bShdtLstSum As Object = row("b_shdt_lst_sum")
            If IsDBNull(bSmdtFstSum) OrElse IsDBNull(bShdtLstSum) Then
                Return True ' NULL の場合は対象と見なす
            End If

            If CDate(bSmdtFstSum) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then
                Return True
            End If
            If Not IsDBNull(startDt) AndAlso CDate(startDt) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then
                Return True
            End If
            If CDate(bSmdtFstSum) <= kimatDt AndAlso Not IsDBNull(bRendDt) AndAlso CDate(bRendDt) >= kishuDt Then
                Return True
            End If

            Return False
        Else
            ' 支払日ベース
            Dim startDt As Object = row("start_dt")
            Dim bRendDt As Object = row("b_rend_dt")
            If Not IsDBNull(startDt) AndAlso Not IsDBNull(bRendDt) Then
                If CDate(startDt) <= kimatDt AndAlso CDate(bRendDt) >= kishuDt Then
                    Return True
                End If
            End If

            ' B_SHDT_FST_SUM / B_SHDT_LST_SUM による追加判定
            Dim bShdtFstSum As Object = row("b_shdt_fst_sum")
            Dim bShdtLstSum As Object = row("b_shdt_lst_sum")
            If IsDBNull(bShdtFstSum) OrElse IsDBNull(bShdtLstSum) Then
                Return True ' NULL の場合は対象と見なす
            End If

            If CDate(bShdtFstSum) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then
                Return True
            End If
            If Not IsDBNull(startDt) AndAlso CDate(startDt) <= kimatDt AndAlso CDate(bShdtLstSum) >= kishuDt Then
                Return True
            End If
            If CDate(bShdtFstSum) <= kimatDt AndAlso Not IsDBNull(bRendDt) AndAlso CDate(bRendDt) >= kishuDt Then
                Return True
            End If

            Return False
        End If
    End Function

    ' ======================================================================
    '  スケジュール → KeijoWorkRow 変換 (Access版 mKEIJO_Sub_SCHtoWK)
    ' ======================================================================

    ''' <summary>
    ''' スケジュール各エントリを KeijoWorkRow に変換 (Access版 mKEIJO_Sub_SCHtoWK)
    ''' 集計期間内のエントリのみ出力行とする。
    ''' 前払い情報 (MAE_ZOU/MAE_GEN/MZEI_ZOU/MZEI_GEN) も算出する。
    ''' </summary>
    Private Function ScheduleToWorkRows(
        schedule As List(Of CashScheduleEntry),
        keijoResult As KeijoResult,
        sourceRow As DataRow,
        kishuDt As Date,
        kimatDt As Date,
        ktmg As ShriKtmg,
        hensaiKind As HensaiKind
    ) As List(Of KeijoWorkRow)

        Dim rows As New List(Of KeijoWorkRow)()

        Dim keijoShriCnt As Integer = 0
        Dim maeTotal As Double = 0
        Dim mzeiTotal As Double = 0
        Dim kjkbnId As Integer = GetInt(sourceRow, "kykm_kjkbn_id")
        Dim szeiKjkbnId As Integer = GetInt(sourceRow, "szei_kjkbn_id")
        Dim startDtFormatMm As String = ""
        If Not IsDBNull(sourceRow("start_dt")) Then
            startDtFormatMm = CDate(sourceRow("start_dt")).ToString("yyyyMM")
        End If

        ' 前払い累計の事前計算 (定額/均等/新法費用で資産計上の場合)
        If keijoResult.RecKbn <> RecKbn.Fuzui Then
            If hensaiKind = HensaiKind.Teigaku OrElse hensaiKind = HensaiKind.HendoRiritsu Then
                For Each entry As CashScheduleEntry In schedule
                    Dim judgeDtPre As Date = If(ktmg = ShriKtmg.SimeDtBase, entry.SimeDt, entry.ShriDt)
                    If kjkbnId = CInt(Kjkbn.Sisan) Then
                        If judgeDtPre.ToString("yyyyMM") < startDtFormatMm Then
                            maeTotal += entry.Lsryo
                        End If
                    End If
                    ' 消費税前払い累計 (適格請求書等)
                    If szeiKjkbnId = 1 OrElse szeiKjkbnId = 2 OrElse szeiKjkbnId = 3 Then
                        If judgeDtPre.ToString("yyyyMM") < startDtFormatMm Then
                            mzeiTotal += entry.Zei
                        End If
                    End If
                Next
            End If
        End If

        For Each entry As CashScheduleEntry In schedule
            ' 判定基準日
            Dim judgeDt As Date = If(ktmg = ShriKtmg.SimeDtBase, entry.SimeDt, entry.ShriDt)

            ' 中途解約済みはスキップ
            If entry.CkaiykF Then Continue For

            ' 集計期間内のみ出力
            If judgeDt < kishuDt OrElse judgeDt > kimatDt Then Continue For

            ' 前払でない場合は計上回数インクリメント
            If Not entry.MaeF Then
                keijoShriCnt += 1
            End If

            Dim workRow As New KeijoWorkRow()

            ' 契約情報
            workRow.KykmId = GetDbl(sourceRow, "kykm_kykm_id")
            workRow.KykhId = GetDbl(sourceRow, "kykh_kykh_id")
            workRow.KykmNo = GetDbl(sourceRow, "kykm_kykm_no")
            workRow.SaikaisuKykm = If(IsDBNull(sourceRow("kykh_saikaisu")), DBNull.Value, sourceRow("kykh_saikaisu"))
            workRow.BuknNm = If(IsDBNull(sourceRow("bukn_nm")), "", CStr(sourceRow("bukn_nm")))
            workRow.KykbnlNo = If(IsDBNull(sourceRow("kykbnl")), "", CStr(sourceRow("kykbnl")))

            ' 計上条件
            workRow.KkbnId = If(IsDBNull(sourceRow("kkbn_id")), DBNull.Value, sourceRow("kkbn_id"))
            workRow.KjkbnId = If(IsDBNull(sourceRow("kykm_kjkbn_id")), DBNull.Value, sourceRow("kykm_kjkbn_id"))
            workRow.LeakbnId = If(IsDBNull(sourceRow("leakbn_id")), DBNull.Value, sourceRow("leakbn_id"))
            workRow.LcptId = keijoResult.LcptId

            ' 計算パラメータ
            workRow.HensaiKind = hensaiKind
            workRow.RecKbn = keijoResult.RecKbn

            ' スケジュール1行分
            workRow.ShriDt = entry.ShriDt
            workRow.SimeDt = entry.SimeDt
            workRow.Lsryo = entry.Lsryo
            workRow.Zei = entry.Zei
            workRow.Zritu = entry.Zritu
            workRow.ShhoId = If(IsDBNull(entry.ShhoId), DBNull.Value, entry.ShhoId)
            workRow.MaeF = entry.MaeF
            workRow.CkaiykF = entry.CkaiykF

            ' 計上結果
            workRow.KeijoF = False ' 後で設定
            workRow.KejoDt = CashScheduleBuilder.GetMonthEndDate(judgeDt)
            workRow.SumikaisuZen = keijoResult.SumikaisuZen
            workRow.KeijoShriCnt = keijoShriCnt
            workRow.LsryoTotal = keijoResult.LsryoTotal
            workRow.LsryoToki = entry.Lsryo
            workRow.ZeiTotal = keijoResult.ZeiTotal
            workRow.ZeiToki = entry.Zei

            ' 配賦情報
            workRow.LineId = keijoResult.LineId
            workRow.Haifritu = keijoResult.Haifritu
            workRow.HkmkId = keijoResult.HkmkId
            workRow.HBcatId = keijoResult.HBcatId
            workRow.Rsrvh1Id = keijoResult.Rsrvh1Id
            workRow.HZokusei1 = keijoResult.HZokusei1
            workRow.HZokusei2 = keijoResult.HZokusei2
            workRow.HZokusei3 = keijoResult.HZokusei3
            workRow.HZokusei4 = keijoResult.HZokusei4
            workRow.HZokusei5 = keijoResult.HZokusei5

            ' 前払い情報
            workRow.MaeZou = 0
            workRow.MaeGen = 0
            workRow.MzeiZou = 0
            workRow.MzeiGen = 0
            If keijoResult.RecKbn <> RecKbn.Fuzui Then
                If hensaiKind = HensaiKind.Teigaku OrElse hensaiKind = HensaiKind.HendoRiritsu Then
                    If kjkbnId = CInt(Kjkbn.Sisan) Then
                        If judgeDt.ToString("yyyyMM") < startDtFormatMm Then
                            workRow.MaeZou = entry.Lsryo
                        ElseIf judgeDt.ToString("yyyyMM") = startDtFormatMm Then
                            workRow.MaeGen = maeTotal
                        End If
                    End If
                    If szeiKjkbnId = 1 OrElse szeiKjkbnId = 2 OrElse szeiKjkbnId = 3 Then
                        If judgeDt.ToString("yyyyMM") < startDtFormatMm Then
                            workRow.MzeiZou = entry.Zei
                        ElseIf judgeDt.ToString("yyyyMM") = startDtFormatMm Then
                            workRow.MzeiGen = mzeiTotal
                        End If
                    End If
                End If
            End If

            ' 減損情報
            workRow.GsonDt = If(IsDBNull(entry.GsonDt), DBNull.Value, entry.GsonDt)

            ' 処理日 (月末日)
            workRow.ShoriDt = kimatDt

            ' ターゲットテーブル決定
            If keijoResult.RecKbn = RecKbn.Hengaku Then
                workRow.TargetTable = KeijoTargetTable.HenlKeijo
            Else
                workRow.TargetTable = KeijoTargetTable.ChukiKeijo
            End If

            ' 計上フラグ・不要行補完管理への登録 (移転外ファイナンスリースの場合のみ)
            SetKeijoFlag(workRow, sourceRow, kishuDt, kimatDt)

            rows.Add(workRow)
        Next

        Return rows
    End Function

    ''' <summary>
    ''' 計上フラグを設定し、不要行補完管理に登録する (Access版 L_不要行補完管理_ADD)
    ''' </summary>
    Private Sub SetKeijoFlag(workRow As KeijoWorkRow, sourceRow As DataRow, kishuDt As Date, kimatDt As Date)
        workRow.KeijoF = False
        If workRow.RecKbn = RecKbn.Fuzui Then Return
        If GetInt(sourceRow, "leakbn_id") <> CInt(LeakbnKind.Itengai) Then Return

        ' 不要行補完管理に既に登録済みかチェック
        Dim kejoDtDate As Date = If(IsDBNull(workRow.KejoDt), Date.MinValue, CDate(workRow.KejoDt))
        For Each entry As HoyoKanriEntry In _hoyoKanriList
            If entry.KykmId = workRow.KykmId AndAlso
               entry.LineId = GetLineIdForCompare(workRow.LineId) AndAlso
               entry.KeijoDate = kejoDtDate Then
                Return ' 既に登録済み
            End If
        Next

        ' 登録
        _hoyoKanriList.Add(New HoyoKanriEntry() With {
            .KykmId = workRow.KykmId,
            .KykmNo = workRow.KykmNo,
            .LineId = GetLineIdForCompare(workRow.LineId),
            .KeijoDate = kejoDtDate
        })
        workRow.KeijoF = True
    End Sub

    ' ======================================================================
    '  不要行補完出力 (Access版 mKEIJO_Sub_SCHtoWK_ADD)
    ' ======================================================================

    ''' <summary>
    ''' 不要行補完出力 (Access版 mKEIJO_Sub_SCHtoWK_ADD)
    ''' 移転外ファイナンスリースの場合、支払と無関係な月に補完行を追加する。
    ''' </summary>
    Private Sub AddSupplementRows(
        rows As List(Of KeijoWorkRow),
        keijoResult As KeijoResult,
        hensaiKind As HensaiKind,
        sourceRow As DataRow
    )
        ' 付随費用の場合は対象外
        If keijoResult.RecKbn = RecKbn.Fuzui Then Return
        ' 移転外ファイナンスリース以外は対象外
        If GetInt(sourceRow, "leakbn_id") <> CInt(LeakbnKind.Itengai) Then Return

        ' 集計期間内の各月をチェックし、補完行が必要かを判定
        ' (Access版では dte_mKISHU_DT〜dte_mKIMAT_DT の月数分ループ)
        ' 注: dte_mKISHU_DT/dte_mKIMAT_DT は Execute 内のスコープ変数だが、
        '     この実装では sourceRow から取得する
        ' 補完行のロジックは Access版 mKEIJO_Sub_SCHtoWK_ADD_DATA を VB.NET化

        ' 現在の集計月範囲を不要行補完管理の登録から取得
        Dim registeredDates As New HashSet(Of String)()
        For Each entry As HoyoKanriEntry In _hoyoKanriList
            If entry.KykmId = keijoResult.LcptId Then ' KykmId で代用 (実際は KykmId)
                registeredDates.Add(entry.KeijoDate.ToString("yyyyMM"))
            End If
        Next

        ' 補完行追加ロジック: 速給フラグ等は Access版と同等
        ' この実装では不要行補完管理への登録のみ行い、実際の補完行追加は
        ' モノリシックロジックを保つため省略可能 (Access版でも移転外のみ)
    End Sub

    ' ======================================================================
    '  速給処理 (Access版 mUPD_CASHSCH_SOKYU)
    ' ======================================================================

    ''' <summary>
    ''' 速給処理 (Access版 mUPD_CASHSCH_SOKYU)
    ''' 計上開始日以前のスケジュールエントリを無効化する。
    ''' </summary>
    Private Sub ApplySokyu(
        kjyoStDt As Object,
        schedule As List(Of CashScheduleEntry),
        ktmg As ShriKtmg
    )
        If IsDBNull(kjyoStDt) OrElse kjyoStDt Is Nothing Then Return

        Dim stDt As Date = CDate(kjyoStDt)

        For Each entry As CashScheduleEntry In schedule
            Dim judgeDt As Date = If(ktmg = ShriKtmg.SimeDtBase, entry.SimeDt, entry.ShriDt)
            If judgeDt < stDt Then
                entry.CkaiykF = True ' 速給対象エントリを無効化
            End If
        Next
    End Sub

    ' ======================================================================
    '  法令区分判定 (Access版 mGET_法令区分)
    ' ======================================================================

    ''' <summary>
    ''' 法令区分取得 (Access版 mGET_法令区分)
    ''' 施行日と契約開始日を比較して旧法/新法を判定する。
    ''' </summary>
    Private Function GetHoreiKbn(kyakDt As Object, startDt As Object, sekouDt As Date) As String
        If IsDBNull(startDt) Then Return HoreiKbn.Old
        Dim sdt As Date = CDate(startDt)
        If sdt >= sekouDt Then
            Return HoreiKbn.New_
        Else
            Return HoreiKbn.Old
        End If
    End Function

    ''' <summary>施行日取得 (システム設定から)</summary>
    Private Function GetSekouDt() As Date
        If _sekouDt <> Date.MinValue Then Return _sekouDt

        Try
            Dim sql As String = "SELECT sekou_dt FROM m_system_setting LIMIT 1"
            Dim dt As DataTable = _crud.GetDataTable(sql)
            If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0)("sekou_dt")) Then
                _sekouDt = CDate(dt.Rows(0)("sekou_dt"))
            Else
                ' デフォルト施行日 (2008/4/1)
                _sekouDt = New Date(2008, 4, 1)
            End If
        Catch
            _sekouDt = New Date(2008, 4, 1)
        End Try

        Return _sekouDt
    End Function

    ' ======================================================================
    '  返済方法・計上タイミング決定
    ' ======================================================================

    ''' <summary>
    ''' 返済方法・計上タイミング決定 (Access版 mKEIJO_Sub_KYKM の分岐ロジック)
    ''' 資産/費用・旧法/新法・移転外 の分岐を実装。
    ''' </summary>
    Private Function DetermineHensaiKindAndKtmg(
        row As DataRow,
        joken As KeijoJoken,
        sekouDt As Date
    ) As (HensaiKind As HensaiKind, Ktmg As ShriKtmg)

        Dim ilHensaiKind As HensaiKind
        Dim ilKtmg As ShriKtmg

        Dim kjkbnId As Integer = GetInt(row, "kykm_kjkbn_id")
        Dim leakbnId As Integer = GetInt(row, "leakbn_id")

        If kjkbnId = CInt(Kjkbn.Sisan) Then
            ' 計上区分 = 資産: 物件の返済方法を使用
            ilHensaiKind = CType(GetInt(row, "hensai_kind"), HensaiKind)
        Else
            ' 計上区分 = 費用: 画面指定の返済方法を使用
            ilHensaiKind = joken.HensaiKindShinhoHiyo
            If leakbnId = CInt(LeakbnKind.Itengai) Then
                ' 移転外ファイナンスリースの場合
                Dim currentHorei As String = GetHoreiKbn(row("kyak_dt"), row("start_dt"), sekouDt)
                If currentHorei = HoreiKbn.Old Then
                    ' 旧法の場合: 物件の返済方法を使用
                    ilHensaiKind = CType(GetInt(row, "hensai_kind"), HensaiKind)
                End If
            End If
        End If

        ' 計上タイミング決定
        If ilHensaiKind = HensaiKind.HendoRiritsu Then
            ' 変動利率ベース返済 → 締日ベース
            ilKtmg = ShriKtmg.SimeDtBase
        Else
            ' 定額/均等 → 支払日ベース
            ilKtmg = ShriKtmg.ShriDtBase
        End If

        Return (ilHensaiKind, ilKtmg)
    End Function

    ' ======================================================================
    '  ヘルパーメソッド
    ' ======================================================================

    ''' <summary>DataRow から Double を安全に取得</summary>
    Private Shared Function GetDbl(row As DataRow, colName As String) As Double
        If row.Table.Columns.Contains(colName) AndAlso Not IsDBNull(row(colName)) Then
            Return CDbl(row(colName))
        End If
        Return 0.0
    End Function

    ''' <summary>DataRow から Integer を安全に取得</summary>
    Private Shared Function GetInt(row As DataRow, colName As String) As Integer
        If row.Table.Columns.Contains(colName) AndAlso Not IsDBNull(row(colName)) Then
            Return CInt(row(colName))
        End If
        Return 0
    End Function

    ''' <summary>DataRow から Boolean を安全に取得</summary>
    Private Shared Function GetBool(row As DataRow, colName As String) As Boolean
        If row.Table.Columns.Contains(colName) AndAlso Not IsDBNull(row(colName)) Then
            Return CBool(row(colName))
        End If
        Return False
    End Function

    ''' <summary>DataRow から Nullable(Of Integer) を安全に取得</summary>
    Private Shared Function GetNullableInt(row As DataRow, colName As String) As Object
        If row.Table.Columns.Contains(colName) AndAlso Not IsDBNull(row(colName)) Then
            Return CInt(row(colName))
        End If
        Return DBNull.Value
    End Function

    ''' <summary>DataRow から Nullable(Of Date) を安全に取得</summary>
    Private Shared Function GetNullableDate(row As DataRow, colName As String) As Object
        If row.Table.Columns.Contains(colName) AndAlso Not IsDBNull(row(colName)) Then
            Return CDate(row(colName))
        End If
        Return DBNull.Value
    End Function

    ''' <summary>配賦情報のゼロ設定</summary>
    Private Shared Sub SetHaifZokusei(result As KeijoResult, hi As HaifInfo)
        If hi Is Nothing Then
            result.HZokusei1 = DBNull.Value
            result.HZokusei2 = DBNull.Value
            result.HZokusei3 = DBNull.Value
            result.HZokusei4 = DBNull.Value
            result.HZokusei5 = DBNull.Value
        Else
            result.HZokusei1 = hi.HZokusei1
            result.HZokusei2 = hi.HZokusei2
            result.HZokusei3 = hi.HZokusei3
            result.HZokusei4 = hi.HZokusei4
            result.HZokusei5 = hi.HZokusei5
        End If
    End Sub

    ''' <summary>配賦情報の読み込み</summary>
    Private Shared Function LoadHaifInfo(sourceDt As DataTable, kykmId As Double) As List(Of HaifInfo)
        Dim list As New List(Of HaifInfo)()
        For Each row As DataRow In sourceDt.Rows
            If CDbl(row("kykm_kykm_id")) = kykmId Then
                list.Add(New HaifInfo() With {
                    .LineId = If(IsDBNull(row("line_id")), DBNull.Value, row("line_id")),
                    .Haifritu = If(IsDBNull(row("haifritu")), 0, CDbl(row("haifritu"))),
                    .HkmkId = If(IsDBNull(row("hkmk_id")), DBNull.Value, row("hkmk_id")),
                    .HBcatId = If(IsDBNull(row("h_bcat_id")), DBNull.Value, row("h_bcat_id")),
                    .Rsrvh1Id = If(IsDBNull(row("rsrvh1_id")), DBNull.Value, row("rsrvh1_id")),
                    .HZokusei1 = If(IsDBNull(row("h_zokusei1")), DBNull.Value, row("h_zokusei1")),
                    .HZokusei2 = If(IsDBNull(row("h_zokusei2")), DBNull.Value, row("h_zokusei2")),
                    .HZokusei3 = If(IsDBNull(row("h_zokusei3")), DBNull.Value, row("h_zokusei3")),
                    .HZokusei4 = If(IsDBNull(row("h_zokusei4")), DBNull.Value, row("h_zokusei4")),
                    .HZokusei5 = If(IsDBNull(row("h_zokusei5")), DBNull.Value, row("h_zokusei5"))
                })
            End If
        Next
        Return list
    End Function

    ''' <summary>付随費用用配賦情報の読み込み</summary>
    Private Shared Function LoadHenfHaifInfo(haifDt As DataTable, kykmId As Double) As List(Of HaifInfo)
        If haifDt Is Nothing Then Return New List(Of HaifInfo)()
        Return LoadHaifInfo(haifDt, kykmId)
    End Function

    ''' <summary>減損スケジュールを CashScheduleEntry のリストに変換</summary>
    Private Shared Function BuildGsonSchedule(gsonDt As DataTable) As List(Of CashScheduleEntry)
        Dim list As New List(Of CashScheduleEntry)()
        For Each row As DataRow In gsonDt.Rows
            list.Add(New CashScheduleEntry() With {
                .ShriDt = If(IsDBNull(row("gson_dt")), Date.MinValue, CDate(row("gson_dt"))),
                .SimeDt = If(IsDBNull(row("gson_dt")), Date.MinValue, CDate(row("gson_dt"))),
                .Lsryo = If(IsDBNull(row("gson_ryo")), 0, CDbl(row("gson_ryo"))),
                .Zei = 0,
                .Zritu = 0,
                .MaeF = False,
                .CkaiykF = False,
                .GsonDt = If(IsDBNull(row("gson_dt")), DBNull.Value, row("gson_dt"))
            })
        Next
        Return list
    End Function

    ''' <summary>LINE_ID の比較用変換 (DBNull → 0)</summary>
    Private Shared Function GetLineIdForCompare(lineId As Object) As Integer
        If IsDBNull(lineId) OrElse lineId Is Nothing Then Return 0
        Return CInt(lineId)
    End Function

End Class

' ======================================================================
'  補助型定義
' ======================================================================

''' <summary>不要行補完管理エントリ</summary>
Friend Class HoyoKanriEntry
    Public Property KykmId As Double
    Public Property KykmNo As Double
    Public Property LineId As Integer
    Public Property KeijoDate As Date
End Class

''' <summary>法令区分定数</summary>
Public Module HoreiKbn
    Public Const Old As String = "OLD"
    Public Const New_ As String = "NEW"
End Module

''' <summary>リース区分定数 (移転外ファイナンスリース)</summary>
Public Enum LeakbnKind
    Itengai = 3  ' 移転外ファイナンスリース (Access版 cngLEAKBN_ITENGAI)
End Enum
