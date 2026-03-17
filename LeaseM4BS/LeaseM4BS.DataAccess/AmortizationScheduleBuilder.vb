' 償却スケジュール生成 (Access版 pc_注記.gMake償却_SCH + gCalc償却率 の忠実移植)
' 定額法 / 定率法 の資産減価償却スケジュールを生成する

Imports System.Collections.Generic

''' <summary>
''' 償却スケジュール生成 (Access版 pc_注記.gMake償却_SCH)
''' </summary>
Public Class AmortizationScheduleBuilder

    ' ======================================================================
    '  定率法の償却率計算 (Access版 gCalc償却率)
    ' ======================================================================

    ''' <summary>
    ''' 定率法の減価償却率を算出する (Access版 gCalc償却率)
    ''' M4互換モード: Int(ritu*1000000) → Long*0.1 → Long*0.00001
    ''' 計算式: 1 - n√0.1 = 1 - 0.1^(1/n)
    ''' </summary>
    ''' <param name="taiyoKikan">耐用月数 (リース期間)</param>
    ''' <returns>償却率 (月率)</returns>
    Public Shared Function CalcShokyakuRitu(taiyoKikan As Integer) As Double
        Dim ritu As Double = 1 - (0.1 ^ (1.0 / taiyoKikan))
        ' Access版 fgM4互換=True (定数) なので M4互換モードで計算
        ' llWK = Int(ritu * 1000000)  → Long
        ' llWK = llWK * 0.1           → Long (VBA Banker's Rounding)
        ' result = llWK * 0.00001
        Dim llWK As Long = CLng(Math.Floor(ritu * 1000000))
        llWK = CLng(Math.Round(llWK * 0.1, MidpointRounding.ToEven))
        Return llWK * 0.00001
    End Function

    ' ======================================================================
    '  メイン: 償却スケジュール生成 (Access版 gMake償却_SCH)
    ' ======================================================================

    ''' <summary>
    ''' 償却スケジュールを生成する (Access版 gMake償却_SCH)
    ''' </summary>
    ''' <param name="skyakHo">償却方法 (定額法/定率法)</param>
    ''' <param name="skyakRitu">償却率 (定率法の場合のみ。定額法はNothing)</param>
    ''' <param name="startDt">開始日</param>
    ''' <param name="lkikan">リース期間月数</param>
    ''' <param name="rendDt">実際終了日 (終了日または中途解約日の前日)</param>
    ''' <param name="syutok">取得価額相当額</param>
    ''' <param name="zanryo">残価保証額 (なしの場合0)</param>
    ''' <param name="gsonSchedule">減損スケジュール</param>
    ''' <param name="warningMsg">ワーニングメッセージ (Out)</param>
    ''' <returns>償却スケジュールリスト</returns>
    Public Shared Function Build(
        skyakHo As ShokyakuHo,
        skyakRitu As Double?,
        startDt As Date,
        lkikan As Integer,
        rendDt As Date,
        syutok As Double,
        zanryo As Double,
        gsonSchedule As List(Of GsonScheduleEntry),
        ByRef warningMsg As String
    ) As List(Of ShokyakuScheduleEntry)

        warningMsg = Nothing

        ' --------------------------------------------------
        ' 開始年月を求める
        ' --------------------------------------------------
        Dim nen As Integer = 0
        Dim getu As Integer = 0
        ScheduleHelper.GetGetuYYYYMM(startDt, nen, getu)
        Dim startGetuSho As Date = ScheduleHelper.GetGetuShoNichi(startDt)

        ' --------------------------------------------------
        ' 償却スケジュール初期化 (配列動的獲得 & 日付等セット)
        ' --------------------------------------------------
        Dim schedule As New List(Of ShokyakuScheduleEntry)(lkikan)
        Dim curNen As Integer = nen
        Dim curGetu As Integer = getu

        For i As Integer = 0 To lkikan - 1
            Dim entry As New ShokyakuScheduleEntry()
            entry.Nen = curNen
            entry.Getu = curGetu
            ' 月度開始日/終了日
            entry.GetuStDt = startGetuSho.AddMonths(i)
            entry.GetuEnDt = startGetuSho.AddMonths(i + 1).AddDays(-1)
            ' 期間開始日/終了日
            entry.LkikanStDt = startDt.AddMonths(i)
            entry.LkikanEnDt = startDt.AddMonths(i + 1).AddDays(-1)
            ' クリア
            entry.GsonRyoS = 0
            entry.GsonRyoE = 0
            entry.GsonRkeiS = 0
            entry.GsonRkeiE = 0
            ' 中途解約フラグ
            If entry.LkikanStDt > rendDt Then
                entry.CkaiykF = True
            End If
            schedule.Add(entry)

            ' 年度/月度カウントアップ
            curGetu += 1
            If curGetu > 12 Then
                curGetu = 1
                curNen += 1
            End If
        Next

        ' --------------------------------------------------
        ' 減損スケジュール反映
        ' --------------------------------------------------
        If gsonSchedule IsNot Nothing Then
            For Each gson In gsonSchedule
                Dim hit As Boolean = False
                For j As Integer = 0 To lkikan - 1
                    If schedule(j).Nen = gson.Nen AndAlso schedule(j).Getu = gson.Getu Then
                        hit = True
                        If schedule(j).CkaiykF Then
                            warningMsg = "減損処理年月が不正です。契約期間の範囲でなければなりません。"
                            Return Nothing
                        End If
                        For k As Integer = j To lkikan - 1
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
                If Not hit Then
                    warningMsg = "減損処理年月が不正です。契約期間の範囲でなければなりません。"
                    Return Nothing
                End If
            Next
        End If

        ' --------------------------------------------------
        ' 償却スケジュールに各値をセット
        ' --------------------------------------------------
        Dim gsonRkeiPrev As Double = 0  ' 前月の減損損失累計額・月度初

        Select Case skyakHo
            Case ShokyakuHo.Teigaku
                BuildTeigaku(schedule, lkikan, syutok, zanryo, gsonRkeiPrev, warningMsg)

            Case ShokyakuHo.Teiritu
                If Not skyakRitu.HasValue Then
                    skyakRitu = CalcShokyakuRitu(lkikan)
                End If
                BuildTeiritu(schedule, lkikan, syutok, zanryo, skyakRitu.Value, gsonRkeiPrev, warningMsg)

            Case Else
                ' 定額法でも定率法でもない場合: 何もしない
        End Select

        Return schedule
    End Function

    ' ======================================================================
    '  定額法 (Access版 gMake償却_SCH の cngSKYAK_HO_TEIGAKU ブロック)
    ' ======================================================================

    Private Shared Sub BuildTeigaku(
        schedule As List(Of ShokyakuScheduleEntry),
        lkikan As Integer,
        syutok As Double,
        zanryo As Double,
        ByRef gsonRkeiPrev As Double,
        ByRef warningMsg As String
    )
        Dim monthSkyak As Double = ScheduleHelper.GInt((syutok - zanryo) / lkikan)

        For i As Integer = 0 To lkikan - 1
            Dim entry = schedule(i)

            ' 償却率: 定額法は Nothing
            entry.SkyakRitu = Nothing

            ' 減価償却累計額・月度初
            If i = 0 Then
                entry.SkyakRkeiS = 0
            Else
                entry.SkyakRkeiS = schedule(i - 1).SkyakRkeiE
            End If

            ' 残高・月度初 = 取得価額 - 減価償却累計額・月度初 - 減損損失累計額・月度初
            entry.ZanS = syutok - entry.SkyakRkeiS - entry.GsonRkeiS

            ' 9分の10残高: 定額法は Nothing
            entry.Zan109S = Nothing

            ' 減価償却費
            If entry.ZanS > zanryo Then
                ' 残高が残価保証額を上回る
                If i = lkikan - 1 Then
                    ' 最終月: 残高 - 残価保証額
                    entry.Skyak = entry.ZanS - zanryo
                    monthSkyak = entry.Skyak
                Else
                    ' 最終月でない
                    If entry.GsonRkeiS > gsonRkeiPrev Then
                        ' 減損損失累計額が増えた: 再計算
                        entry.Skyak = ScheduleHelper.GInt((entry.ZanS - zanryo) / (lkikan - i))
                        monthSkyak = entry.Skyak
                    Else
                        entry.Skyak = monthSkyak
                    End If
                End If
            Else
                ' 残高が残価保証額以下: 償却費0
                entry.Skyak = 0
            End If

            ' 減価償却累計額・月度末
            entry.SkyakRkeiE = entry.SkyakRkeiS + entry.Skyak

            ' 残高・月度末
            entry.ZanE = syutok - entry.SkyakRkeiE - entry.GsonRkeiE

            ' 9分の10残高・月度末: 定額法は Nothing
            entry.Zan109E = Nothing

            gsonRkeiPrev = entry.GsonRkeiS

            ' ワーニング
            If entry.ZanE < zanryo Then
                If zanryo > 0 Then
                    warningMsg = "「減損損失」が『「残高相当額」−「残価保証額」』を超えているため「残高相当額」が「残価保証額」より小さくなります。"
                Else
                    warningMsg = "「減損損失」が「残高相当額」を超えているため「残高相当額」がマイナスになります。"
                End If
            End If
        Next
    End Sub

    ' ======================================================================
    '  定率法 (Access版 gMake償却_SCH の cngSKYAK_HO_TEIRITU ブロック)
    ' ======================================================================

    Private Shared Sub BuildTeiritu(
        schedule As List(Of ShokyakuScheduleEntry),
        lkikan As Integer,
        syutok As Double,
        zanryo As Double,
        skyakRitu As Double,
        ByRef gsonRkeiPrev As Double,
        ByRef warningMsg As String
    )
        Dim currentRitu As Double = skyakRitu

        For i As Integer = 0 To lkikan - 1
            Dim entry = schedule(i)

            ' 償却率
            If entry.GsonRkeiS > gsonRkeiPrev Then
                ' 減損増加時: 償却率再計算
                currentRitu = CalcShokyakuRitu(lkikan - i)
            End If
            entry.SkyakRitu = currentRitu

            ' 減価償却累計額・月度初
            If i = 0 Then
                entry.SkyakRkeiS = 0
            Else
                entry.SkyakRkeiS = schedule(i - 1).SkyakRkeiE
            End If

            ' 残高・月度初
            entry.ZanS = syutok - entry.SkyakRkeiS - entry.GsonRkeiS

            ' 減価償却費
            If entry.ZanS > zanryo Then
                ' 9分の10残高・月度初
                If entry.GsonRyoS > 0 Then
                    ' 減損損失・月度初 発生時
                    entry.Zan109S = ScheduleHelper.GInt((entry.ZanS - zanryo) * 10.0 / 9.0)
                Else
                    ' 減損未発生時
                    If i = 0 Then
                        entry.Zan109S = ScheduleHelper.GInt((syutok - zanryo) * 10.0 / 9.0)
                    Else
                        entry.Zan109S = schedule(i - 1).Zan109E
                    End If
                End If

                ' 減価償却費
                If i = lkikan - 1 Then
                    ' 最終月: 残高 - 残価保証額
                    entry.Skyak = entry.ZanS - zanryo
                Else
                    ' 9分の10残高 × 償却率
                    entry.Skyak = ScheduleHelper.GInt(entry.Zan109S.Value * currentRitu)
                End If
            Else
                ' 残高が残価保証額以下
                entry.Skyak = 0
            End If

            ' 減価償却累計額・月度末
            entry.SkyakRkeiE = entry.SkyakRkeiS + entry.Skyak

            ' 残高・月度末
            entry.ZanE = syutok - entry.SkyakRkeiE - entry.GsonRkeiE

            ' 9分の10残高・月度末
            If entry.GsonRyoE > 0 Then
                ' 減損損失・月度末 発生時
                entry.Zan109E = ScheduleHelper.GInt((entry.ZanE - zanryo) * 10.0 / 9.0)
            Else
                ' 減損未発生時
                If entry.Zan109S.HasValue Then
                    entry.Zan109E = entry.Zan109S.Value - entry.Skyak
                Else
                    entry.Zan109E = Nothing
                End If
            End If

            gsonRkeiPrev = entry.GsonRkeiS

            ' ワーニング
            If entry.ZanE < zanryo Then
                If zanryo > 0 Then
                    warningMsg = "「減損損失」が『「残高相当額」−「残価保証額」』を超えているため「残高相当額」が「残価保証額」より小さくなります。"
                Else
                    warningMsg = "「減損損失」が「残高相当額」を超えているため「残高相当額」がマイナスになります。"
                End If
            End If
        Next
    End Sub

End Class
