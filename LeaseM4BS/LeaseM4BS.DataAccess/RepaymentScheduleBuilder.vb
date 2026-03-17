' 返済スケジュール生成 (Access版 pc_注記.gMake返済_SCH_約定支払用 + SET基礎部分 の忠実移植)
' 負債返済スケジュール (元本・利息・残高の月次推移) を生成する

Imports System.Collections.Generic

''' <summary>
''' 返済スケジュール生成 (Access版 pc_注記.gMake返済_SCH_約定支払用)
''' </summary>
Public Class RepaymentScheduleBuilder

    ' ======================================================================
    '  メイン: 約定支払用返済スケジュール生成
    ' ======================================================================

    ''' <summary>
    ''' 約定支払用返済スケジュールを生成する (Access版 gMake返済_SCH_約定支払用)
    ''' </summary>
    ''' <param name="startDt">開始日</param>
    ''' <param name="lkikan">リース期間月数</param>
    ''' <param name="rendDt">実際終了日</param>
    ''' <param name="syutok">取得価額相当額</param>
    ''' <param name="slsryo">総額リース料</param>
    ''' <param name="ijiknr">維持管理費用</param>
    ''' <param name="zanryo">残価保証額</param>
    ''' <param name="lbSoneki">リースバック売却損益</param>
    ''' <param name="ckaiykF">中途解約フラグ</param>
    ''' <param name="ksanRitu">計算利子率</param>
    ''' <param name="rsokTmg">先払/後払</param>
    ''' <param name="shiharaiSch">支払スケジュール</param>
    ''' <param name="gsonSchedule">減損スケジュール</param>
    ''' <param name="hensaiKind">返済方法</param>
    ''' <param name="warningMsg">ワーニングメッセージ (Out)</param>
    ''' <returns>返済スケジュールリスト。エラー時はNothing</returns>
    Public Shared Function BuildYakujoShiharai(
        startDt As Date,
        lkikan As Integer,
        rendDt As Date,
        syutok As Double,
        slsryo As Double,
        ijiknr As Double,
        zanryo As Double,
        lbSoneki As Double?,
        ckaiykF As Boolean,
        ksanRitu As Double,
        rsokTmg As RsokTmg,
        shiharaiSch As List(Of ShiharaiSchEntry),
        gsonSchedule As List(Of GsonScheduleEntry),
        hensaiKind As HensaiKind,
        ByRef warningMsg As String
    ) As List(Of HensaiScheduleEntry)

        warningMsg = Nothing

        ' --------------------------------------------------
        ' 基礎部分を作成 (SET基礎部分)
        ' --------------------------------------------------
        Dim schedule As List(Of HensaiScheduleEntry) = SetKisobubu(
            startDt, lkikan, rendDt, slsryo, ijiknr, zanryo, lbSoneki,
            ckaiykF, rsokTmg, shiharaiSch, hensaiKind)

        If schedule Is Nothing OrElse schedule.Count = 0 Then Return schedule

        Dim schCnt As Integer = schedule.Count

        ' -- 利息総額 = 総額リース料 - 維持管理費用 + 残価保証額 - 取得価額相当額
        Dim risokuTotal As Double = ScheduleHelper.GKasan(True, slsryo, -ijiknr, zanryo, -syutok)
        Dim risokuZan As Double = risokuTotal

        ' --------------------------------------------------
        ' 利息・元本の計算
        ' --------------------------------------------------
        For i As Integer = 0 To schCnt - 1
            Dim entry = schedule(i)

            ' -- 発生利息・月度初
            entry.RisokuHasseiS = 0

            If i = 0 Then
                ' 初回月度
                ' 支払利息・月度初
                If entry.RisokuHasseiS > entry.NetLsryoS Then
                    entry.RisokuShriS = entry.NetLsryoS
                Else
                    entry.RisokuShriS = entry.RisokuHasseiS
                End If
                ' 返済元本・月度初
                entry.GanponS = entry.NetLsryoS - entry.RisokuShriS
                ' 未払利息残高・月度初
                entry.RisokuMibZanS = entry.RisokuHasseiS - entry.RisokuShriS
                ' 利息残高・月度初
                entry.RisokuZanS = risokuTotal - entry.RisokuShriS
                ' 元本残高・月度初
                entry.GanponZanS = syutok - entry.GanponS
            Else
                ' 2ヶ月目以降
                Dim prev = schedule(i - 1)
                ' 支払利息・月度初
                Dim wk As Double = prev.RisokuMibZanE + entry.RisokuHasseiS
                If wk > entry.NetLsryoS Then
                    entry.RisokuShriS = entry.NetLsryoS
                Else
                    entry.RisokuShriS = wk
                End If

                ' 返済元本・月度初
                entry.GanponS = entry.NetLsryoS - entry.RisokuShriS

                ' ★2010/06/07 元本が債務残高を超える場合の調整
                If entry.GanponS > prev.GanponZanE Then
                    Dim excess As Double = entry.GanponS - prev.GanponZanE
                    entry.GanponS = prev.GanponZanE
                    entry.RisokuHasseiS += excess
                    entry.RisokuShriS += excess
                End If

                ' 未払利息残高・月度初
                entry.RisokuMibZanS = prev.RisokuMibZanE + entry.RisokuHasseiS - entry.RisokuShriS
                ' 利息残高・月度初
                entry.RisokuZanS = prev.RisokuZanE - entry.RisokuShriS
                ' 元本残高・月度初
                entry.GanponZanS = prev.GanponZanE - entry.GanponS
            End If

            risokuZan -= entry.RisokuHasseiS

            ' -- 発生利息・月度末
            If entry.NetLsryoZanE = 0 AndAlso entry.ZanryoMsZanE = 0 Then
                ' 当該月より後に支払/残価保証清算がない
                If entry.NetLsryoE > 0 OrElse entry.ZanryoSeisanE > 0 Then
                    entry.RisokuHasseiE = risokuZan
                    risokuZan = 0
                Else
                    entry.RisokuHasseiE = 0
                End If
            Else
                ' 翌月チェック
                If i + 1 < schCnt Then
                    Dim nxt = schedule(i + 1)
                    If nxt.NetLsryoZanE = 0 AndAlso nxt.ZanryoMsZanE = 0 AndAlso
                       nxt.NetLsryoE = 0 AndAlso nxt.ZanryoSeisanE = 0 Then
                        ' 翌月が最終支払で、翌月の月度末にも支払がない場合
                        entry.RisokuHasseiE = risokuZan
                        risokuZan = 0
                    Else
                        ' 通常ケース: (元本残高+未払利息) × 計算利子率 / 12
                        entry.RisokuHasseiE = ScheduleHelper.GInt((entry.GanponZanS + entry.RisokuMibZanS) * ksanRitu / 12)
                    End If
                Else
                    entry.RisokuHasseiE = risokuZan
                    risokuZan = 0
                End If
            End If

            ' -- 支払利息・月度末
            Dim wk1 As Double = entry.RisokuMibZanS + entry.RisokuHasseiE
            Dim wk2 As Double = entry.NetLsryoE + entry.ZanryoSeisanE
            If wk1 > wk2 Then
                entry.RisokuShriE = wk2
            Else
                entry.RisokuShriE = wk1
            End If

            ' -- 返済元本・月度末
            entry.GanponE = entry.NetLsryoE + entry.ZanryoSeisanE - entry.RisokuShriE

            ' ★2010/06/07 元本が債務残高を超える場合の調整
            If entry.GanponE > entry.GanponZanS Then
                Dim excess As Double = entry.GanponE - entry.GanponZanS
                entry.GanponE = entry.GanponZanS
                entry.RisokuHasseiE += excess
                entry.RisokuShriE += excess
            End If

            ' -- 未払利息残高・月度末
            entry.RisokuMibZanE = entry.RisokuMibZanS + entry.RisokuHasseiE - entry.RisokuShriE
            ' -- 利息残高・月度末
            entry.RisokuZanE = entry.RisokuZanS - entry.RisokuShriE
            ' -- 元本残高・月度末
            entry.GanponZanE = entry.GanponZanS - entry.GanponE

            risokuZan -= entry.RisokuHasseiE
        Next

        ' --------------------------------------------------
        ' 減損処理
        ' --------------------------------------------------
        ' 初期化
        For i As Integer = 0 To schCnt - 1
            schedule(i).GsonRyoS = 0
            schedule(i).GsonRyoE = 0
            schedule(i).GsonRkeiS = 0
            schedule(i).GsonRkeiE = 0
        Next

        ' 減損スケジュール反映
        If gsonSchedule IsNot Nothing Then
            For Each gson In gsonSchedule
                For j As Integer = 0 To schCnt - 1
                    If schedule(j).Nen = gson.Nen AndAlso schedule(j).Getu = gson.Getu Then
                        For k As Integer = j To schCnt - 1
                            If k = j Then
                                schedule(k).GsonRyoS = gson.GsonRyoS
                                schedule(k).GsonRyoE = gson.GsonRyoE
                                schedule(k).GsonRkeiS = gson.GsonRkeiS
                            Else
                                schedule(k).GsonRyoS = 0
                                schedule(k).GsonRyoE = 0
                                schedule(k).GsonRkeiS = gson.GsonRkeiE
                            End If
                            schedule(k).GsonRkeiE = gson.GsonRkeiE
                        Next
                        Exit For
                    End If
                Next
            Next
        End If

        ' --------------------------------------------------
        ' 減損勘定の残高・取崩額
        ' --------------------------------------------------
        For i As Integer = 0 To schCnt - 1
            Dim entry = schedule(i)

            ' 減損勘定残高・月度初
            If i = 0 Then
                entry.GsonZanS = entry.GsonRyoS
            Else
                entry.GsonZanS = schedule(i - 1).GsonZanE + entry.GsonRyoS
            End If

            ' 減損勘定取崩額
            If entry.CashZanE = 0 Then
                ' 最終支払
                entry.GsonTk = entry.GsonZanS
            Else
                If i = 0 Then
                    entry.GsonTk = ScheduleHelper.GInt(entry.GsonZanS * ((entry.CashS + entry.CashE) / slsryo))
                Else
                    If schedule(i - 1).CashZanE <> 0 Then
                        entry.GsonTk = ScheduleHelper.GInt(entry.GsonZanS * ((entry.CashS + entry.CashE) / schedule(i - 1).CashZanE))
                    Else
                        entry.GsonTk = entry.GsonZanS
                    End If
                End If
            End If

            ' 減損勘定残高・月度末
            entry.GsonZanE = entry.GsonZanS + entry.GsonRyoE - entry.GsonTk
        Next

        Return schedule
    End Function

    ' ======================================================================
    '  基礎部分セット (Access版 gMake返済_SCH_約定支払用_SET基礎部分)
    ' ======================================================================

    Private Shared Function SetKisobubu(
        startDt As Date,
        lkikan As Integer,
        rendDt As Date,
        slsryo As Double,
        ijiknr As Double,
        zanryo As Double,
        lbSoneki As Double?,
        ckaiykF As Boolean,
        rsokTmg As RsokTmg,
        shiharaiSch As List(Of ShiharaiSchEntry),
        hensaiKind As HensaiKind
    ) As List(Of HensaiScheduleEntry)

        If shiharaiSch Is Nothing OrElse shiharaiSch.Count = 0 Then
            Return New List(Of HensaiScheduleEntry)()
        End If

        ' --------------------------------------------------
        ' 最終支払月度を求める
        ' --------------------------------------------------
        Dim startNen As Integer = 0, startGetu As Integer = 0
        ScheduleHelper.GetGetuYYYYMM(startDt, startNen, startGetu)

        Dim shriEnDt As Date? = Nothing
        For Each s In shiharaiSch
            If Not shriEnDt.HasValue OrElse s.KeijDt > shriEnDt.Value Then
                shriEnDt = s.KeijDt
            End If
        Next

        Dim shriEnNen As Integer = 0, shriEnGetu As Integer = 0
        If shriEnDt.HasValue Then
            ScheduleHelper.GetGetuYYYYMM(shriEnDt.Value, shriEnNen, shriEnGetu)
        End If

        ' 最終発生月度
        Dim endNen As Integer = startNen
        Dim endGetu As Integer = startGetu + lkikan - 1
        endNen += CInt(Math.Floor((endGetu - 1) / 12.0))
        endGetu = ((endGetu - 1) Mod 12) + 1

        ' 返済スケジュール最終月度
        Dim schLastNen As Integer, schLastGetu As Integer
        If Not shriEnDt.HasValue Then
            schLastNen = endNen : schLastGetu = endGetu
        Else
            If New Date(endNen, endGetu, 1) > New Date(shriEnNen, shriEnGetu, 1) Then
                schLastNen = endNen : schLastGetu = endGetu
            Else
                schLastNen = shriEnNen : schLastGetu = shriEnGetu
            End If
        End If

        Dim startGetuSho As Date = ScheduleHelper.GetGetuShoNichi(startDt)

        ' --------------------------------------------------
        ' 返済スケジュール初期化
        ' --------------------------------------------------
        Dim schCnt As Integer = (schLastNen - startNen) * 12 + schLastGetu - startGetu + 1
        Dim schedule As New List(Of HensaiScheduleEntry)(schCnt)

        Dim curNen As Integer = startNen
        Dim curGetu As Integer = startGetu

        For i As Integer = 0 To schCnt - 1
            Dim entry As New HensaiScheduleEntry()
            entry.Nen = curNen
            entry.Getu = curGetu
            entry.GetuStDt = startGetuSho.AddMonths(i)
            entry.GetuEnDt = startGetuSho.AddMonths(i + 1).AddDays(-1)
            entry.LkikanStDt = startDt.AddMonths(i)
            entry.LkikanEnDt = startDt.AddMonths(i + 1).AddDays(-1)
            ' 全項目 0/Nothing にリセット (デフォルト値でOK)
            ' 中途解約フラグ
            If ckaiykF Then
                If entry.LkikanStDt > rendDt Then
                    entry.CkaiykF = True
                End If
            End If
            schedule.Add(entry)

            curGetu += 1
            If curGetu > 12 Then
                curNen += 1
                curGetu = 1
            End If
        Next

        ' --------------------------------------------------
        ' 支払スケジュール → 月度マッピング
        ' --------------------------------------------------
        For Each s In shiharaiSch
            Dim idx As Integer

            If hensaiKind = HensaiKind.HendoRiritsu Then
                ' 返済方法＝請求ベースの場合 (Access版 cngHENSAI_KIND_請求支払)
                If s.KeijDt.ToString("yyyy/MM") < startDt.ToString("yyyy/MM") Then
                    ' 計上日が開始日より前の月度 → 先頭行の月度初
                    schedule(0).CashS += s.Cash
                Else
                    ' 計上日が開始日以降の月度
                    idx = (s.KeijNen - startNen) * 12 + s.KeijGetu - startGetu
                    If idx >= 0 AndAlso idx < schCnt Then
                        Select Case rsokTmg
                            Case RsokTmg.Atobarai, RsokTmg.AtobaraiKaishiKojo
                                schedule(idx).CashE += s.Cash
                            Case RsokTmg.Sakibarai
                                schedule(idx).CashS += s.Cash
                        End Select
                    End If
                End If
            Else
                ' 返済方法＝約定支払の場合 (Access版 cngHENSAI_KIND_約定支払 / 均等支払)
                If s.KeijDt <= startDt Then
                    If rsokTmg = RsokTmg.AtobaraiKaishiKojo AndAlso
                       s.KeijDt.ToString("yyyyMM") = startDt.ToString("yyyyMM") Then
                        ' 開始月と同月なら利息控除
                        idx = (s.KeijNen - startNen) * 12 + s.KeijGetu - startGetu
                        If idx >= 0 AndAlso idx < schCnt Then
                            schedule(idx).CashE += s.Cash
                        End If
                    Else
                        ' 開始日以前 → 先頭行の月度初に加算
                        schedule(0).CashS += s.Cash
                    End If
                Else
                    ' 開始日より後
                    idx = (s.KeijNen - startNen) * 12 + s.KeijGetu - startGetu
                    If idx >= 0 AndAlso idx < schCnt Then
                        Select Case rsokTmg
                            Case RsokTmg.Atobarai, RsokTmg.AtobaraiKaishiKojo
                                schedule(idx).CashE += s.Cash
                            Case RsokTmg.Sakibarai
                                schedule(idx).CashS += s.Cash
                        End Select
                    End If
                End If
            End If
        Next

        ' --------------------------------------------------
        ' 残価保証清算額・残価保証未清算残高
        ' --------------------------------------------------
        If zanryo <> 0 Then
            Dim endIdx As Integer = (endNen - startNen) * 12 + endGetu - startGetu
            If endIdx >= 0 AndAlso endIdx < schCnt Then
                schedule(endIdx).ZanryoSeisanE = zanryo
            End If

            For i As Integer = 0 To schCnt - 1
                If i = 0 Then
                    schedule(i).ZanryoMsZanE = zanryo - schedule(i).ZanryoSeisanE
                Else
                    schedule(i).ZanryoMsZanE = schedule(i - 1).ZanryoMsZanE - schedule(i).ZanryoSeisanE
                End If
            Next
        End If

        ' --------------------------------------------------
        ' 支払額残高・月度末
        ' --------------------------------------------------
        For i As Integer = 0 To schCnt - 1
            If i = 0 Then
                schedule(i).CashZanE = slsryo - schedule(i).CashS - schedule(i).CashE
            Else
                schedule(i).CashZanE = schedule(i - 1).CashZanE - schedule(i).CashS - schedule(i).CashE
            End If
        Next

        ' --------------------------------------------------
        ' 維持管理費用 (支払タイミングで按分)
        ' --------------------------------------------------
        If ijiknr <> 0 Then
            Dim ijiknrRkei As Double = 0
            For i As Integer = 0 To schCnt - 1
                Dim entry = schedule(i)
                ' 月度初
                If entry.CashS > 0 Then
                    entry.IjiknrS = ScheduleHelper.GInt(ijiknr * (entry.CashS / slsryo))
                    ijiknrRkei += entry.IjiknrS.Value
                End If
                ' 月度末
                If entry.CashE > 0 Then
                    entry.IjiknrE = ScheduleHelper.GInt(ijiknr * (entry.CashE / slsryo))
                    ijiknrRkei += entry.IjiknrE.Value
                End If
                ' 端数処理: 最終支払月
                If (entry.CashS > 0 OrElse entry.CashE > 0) AndAlso entry.CashZanE = 0 Then
                    If entry.CashE > 0 Then
                        entry.IjiknrE = If(entry.IjiknrE, 0) + (ijiknr - ijiknrRkei)
                    ElseIf entry.CashS > 0 Then
                        entry.IjiknrS = If(entry.IjiknrS, 0) + (ijiknr - ijiknrRkei)
                    End If
                End If
            Next
        End If

        ' --------------------------------------------------
        ' NET支払リース料 (支払額 - 維持管理費用)
        ' --------------------------------------------------
        For i As Integer = 0 To schCnt - 1
            Dim entry = schedule(i)
            entry.NetLsryoS = ScheduleHelper.GKasan(False, entry.CashS, -If(entry.IjiknrS, 0))
            entry.NetLsryoE = ScheduleHelper.GKasan(False, entry.CashE, -If(entry.IjiknrE, 0))

            ' NET支払リース料残高・月度末
            If i = 0 Then
                entry.NetLsryoZanE = ScheduleHelper.GKasan(False, slsryo, -ijiknr, -entry.NetLsryoS, -entry.NetLsryoE)
            Else
                entry.NetLsryoZanE = schedule(i - 1).NetLsryoZanE - entry.NetLsryoS - entry.NetLsryoE
            End If
        Next

        ' --------------------------------------------------
        ' リースバック繰延損益 (支払タイミングで按分)
        ' --------------------------------------------------
        If lbSoneki.HasValue AndAlso lbSoneki.Value <> 0 Then
            Dim lbVal As Double = lbSoneki.Value
            Dim lbRkei As Double = 0
            For i As Integer = 0 To schCnt - 1
                Dim entry = schedule(i)
                ' 月度初
                If entry.CashS > 0 Then
                    If lbVal < 0 Then
                        entry.LbSonekiS = -ScheduleHelper.GInt(-lbVal * (entry.CashS / slsryo))
                    Else
                        entry.LbSonekiS = ScheduleHelper.GInt(lbVal * (entry.CashS / slsryo))
                    End If
                    lbRkei += entry.LbSonekiS.Value
                End If
                ' 月度末
                If entry.CashE > 0 Then
                    If lbVal < 0 Then
                        entry.LbSonekiE = -ScheduleHelper.GInt(-lbVal * (entry.CashE / slsryo))
                    Else
                        entry.LbSonekiE = ScheduleHelper.GInt(lbVal * (entry.CashE / slsryo))
                    End If
                    lbRkei += entry.LbSonekiE.Value
                End If
                ' 端数処理
                If (entry.CashS > 0 OrElse entry.CashE > 0) AndAlso entry.CashZanE = 0 Then
                    If entry.CashE > 0 Then
                        entry.LbSonekiE = If(entry.LbSonekiE, 0) + (lbVal - lbRkei)
                    ElseIf entry.CashS > 0 Then
                        entry.LbSonekiS = If(entry.LbSonekiS, 0) + (lbVal - lbRkei)
                    End If
                End If
            Next
        End If

        Return schedule
    End Function

End Class
