' 注記計算エンジン (Access版 pc_注記.m注記計算_main + SUB_資産関連 + SUB_返済関連 の忠実移植)
' 償却/返済スケジュールから注記計算結果を集計する

Imports System.Collections.Generic

''' <summary>
''' 注記計算エンジン (Access版 pc_注記.m注記計算_main)
''' 償却・返済スケジュールの結果から注記用の各金額を算出する
''' </summary>
Public Class ChukiCalcEngine

    ''' <summary>
    ''' 注記計算メイン (Access版 m注記計算_main)
    ''' </summary>
    Public Shared Function Calculate(
        params As ChukiCalcParams,
        shiharaiSch As List(Of ShiharaiSchEntry),
        gsonSchedule As List(Of GsonScheduleEntry),
        crud As CrudHelper
    ) As ChukiCalcResult

        Dim result As New ChukiCalcResult()

        ' --------------------------------------------------
        ' 前準備
        ' --------------------------------------------------
        Dim syutok As Double
        Dim ksanRitu As Double

        If params.RcalcId = CInt(RcalcKind.Risikomi) OrElse params.LeakbnId = CInt(LeaseKbn.Ope) Then
            ' 利子込法またはオペレーティングリース
            syutok = ScheduleHelper.GKasan(True, params.BSlsryo, -params.BIjiknr, params.BZanryo)
            ksanRitu = 0
        Else
            syutok = params.BSyutok
            ksanRitu = params.KsanRitu
        End If

        ' 前期末日
        Dim zKimatDt As Date = params.KishuDt.AddDays(-1)

        ' 翌期以降の期首/期末日
        Dim y1KishuDt As Date = params.KimatDt.AddDays(1)
        Dim kessanBi As Integer = params.KessanBi

        Dim y1KimatDt, y2KishuDt, y3KishuDt, y4KishuDt, y5KishuDt, y5KimatDt As Date

        If kessanBi = 31 Then
            ' 月末日決算
            y1KimatDt = y1KishuDt.AddYears(1).AddDays(-1)
            y2KishuDt = y1KishuDt.AddYears(1)
            y3KishuDt = y1KishuDt.AddYears(2)
            y4KishuDt = y1KishuDt.AddYears(3)
            y5KishuDt = y1KishuDt.AddYears(4)
            y5KimatDt = y1KishuDt.AddYears(5).AddDays(-1)
        Else
            ' 月末日決算でない場合 (1-28日)
            ' ※Access版にコピペバグあり: Y4/Y5変数を設定せずY3/Y1KIMATを上書きしている。
            '   Access版と完全一致させるため同じ動作を再現する。
            '   実運用では ig決算日=31 が標準のためこの分岐はほぼ通らない。
            y1KimatDt = params.KimatDt.AddYears(5)               ' Access版バグ: 最後に5年後で上書き
            y2KishuDt = params.KimatDt.AddYears(1).AddDays(1)
            y3KishuDt = params.KimatDt.AddYears(4).AddDays(1)   ' Access版バグ: 最後に4年後+1日で上書き
            y4KishuDt = #12/30/1899#                             ' Access版: VBA未初期化Date
            y5KishuDt = #12/30/1899#                             ' Access版: VBA未初期化Date
            y5KimatDt = #12/30/1899#                             ' Access版: VBA未初期化Date
        End If

        ' --------------------------------------------------
        ' 資産関連 (移転外Fリースのみ)
        ' --------------------------------------------------
        If params.LeakbnId = CInt(LeaseKbn.Itengai) Then
            Dim skyakRitu As Double? = Nothing
            If params.SkyakHoId = CInt(ShokyakuHo.Teiritu) Then
                skyakRitu = AmortizationScheduleBuilder.CalcShokyakuRitu(params.Lkikan)
            End If
            result.SkyakRitu = skyakRitu

            ' 償却スケジュール作成
            Dim warningMsg As String = Nothing
            Dim shokyakuSch = AmortizationScheduleBuilder.Build(
                CType(params.SkyakHoId, ShokyakuHo), skyakRitu,
                params.StartDt, params.Lkikan, params.BRendDt,
                syutok, params.BZanryo,
                gsonSchedule, warningMsg)

            If shokyakuSch IsNot Nothing Then
                CalcShisanRelated(result, params, syutok, zKimatDt, shokyakuSch)
            End If
        End If

        ' --------------------------------------------------
        ' 返済関連
        ' --------------------------------------------------
        Dim hWarningMsg As String = Nothing
        Dim hensaiSch = RepaymentScheduleBuilder.BuildYakujoShiharai(
            params.StartDt, params.Lkikan, params.BRendDt,
            syutok, params.BSlsryo, params.BIjiknr, params.BZanryo,
            If(params.LbChukiF, params.BLbSoneki, CType(Nothing, Double?)),
            params.BCkaiykF, ksanRitu, params.RsokTmg,
            shiharaiSch, gsonSchedule, params.HensaiKind, hWarningMsg)

        If hensaiSch IsNot Nothing AndAlso hensaiSch.Count > 0 Then
            CalcHensaiRelated(result, params, hensaiSch, syutok, zKimatDt,
                              y1KishuDt, y1KimatDt, y2KishuDt, y3KishuDt,
                              y4KishuDt, y5KishuDt, y5KimatDt)
        End If

        Return result
    End Function

    ' ======================================================================
    '  資産関連集計 (Access版 m注記計算_SUB_資産関連)
    ' ======================================================================

    Private Shared Sub CalcShisanRelated(
        result As ChukiCalcResult,
        params As ChukiCalcParams,
        syutok As Double,
        zKimatDt As Date,
        shokyakuSch As List(Of ShokyakuScheduleEntry)
    )
        Dim matsubiF As Boolean = params.MatsubiShuryoKichuMasshoF

        ' -- 取得価額・前期末残高
        If matsubiF Then
            If params.StartDt < params.KishuDt AndAlso params.BRendDt >= params.KishuDt Then
                result.SyutokZzan = syutok
            Else
                result.SyutokZzan = 0 : result.GruikeiZzan = 0 : result.GsonRkeiZzan = 0
            End If
        Else
            If params.StartDt < params.KishuDt AndAlso params.BRendDt >= zKimatDt Then
                result.SyutokZzan = syutok
            Else
                result.SyutokZzan = 0 : result.GruikeiZzan = 0 : result.GsonRkeiZzan = 0
            End If
        End If

        ' -- 取得価額・当期増加
        If params.StartDt >= params.KishuDt AndAlso params.StartDt <= params.KimatDt Then
            result.SyutokZou = syutok
        Else
            result.SyutokZou = 0
        End If

        ' -- 取得価額・当期減少
        If matsubiF Then
            If params.BRendDt >= params.KishuDt AndAlso params.BRendDt <= params.KimatDt Then
                result.SyutokGen = syutok
            Else
                result.SyutokGen = 0
            End If
        Else
            If params.BRendDt >= zKimatDt AndAlso params.BRendDt < params.KimatDt Then
                result.SyutokGen = syutok
            Else
                result.SyutokGen = 0
            End If
        End If

        ' -- 取得価額・期末残高
        result.SyutokZan = result.SyutokZzan + result.SyutokZou - result.SyutokGen

        ' -- 減価償却累計額/減損損失累計額の集計
        result.GruikeiZzan = 0 : result.GruikeiZou = 0
        result.GsonRkeiZzan = 0 : result.GsonRkeiZou = 0

        For Each s In shokyakuSch
            ' 前期末残高
            If s.GetuEnDt = zKimatDt Then
                result.GruikeiZzan = s.SkyakRkeiE
                result.GsonRkeiZzan = s.GsonRkeiE
            End If
            ' 当期増加
            If s.GetuStDt >= params.KishuDt AndAlso s.GetuStDt <= params.KimatDt Then
                If params.BRendDt >= s.LkikanStDt Then
                    result.GruikeiZou += s.Skyak
                    result.GsonRkeiZou += s.GsonRyoS + s.GsonRyoE
                End If
            End If
        Next

        ' 前期末残高調整
        If matsubiF Then
            If params.StartDt >= params.KishuDt OrElse params.BRendDt < params.KishuDt Then
                result.GruikeiZzan = 0 : result.GsonRkeiZzan = 0
            End If
        Else
            If params.StartDt >= params.KishuDt OrElse params.BRendDt < zKimatDt Then
                result.GruikeiZzan = 0 : result.GsonRkeiZzan = 0
            End If
        End If

        ' 当期減少
        If matsubiF Then
            If params.BRendDt >= params.KishuDt AndAlso params.BRendDt <= params.KimatDt Then
                result.GruikeiGen = result.GruikeiZzan + result.GruikeiZou
                result.GsonRkeiGen = result.GsonRkeiZzan + result.GsonRkeiZou
            Else
                result.GruikeiGen = 0 : result.GsonRkeiGen = 0
            End If
        Else
            If params.BRendDt >= zKimatDt AndAlso params.BRendDt < params.KimatDt Then
                result.GruikeiGen = result.GruikeiZzan + result.GruikeiZou
                result.GsonRkeiGen = result.GsonRkeiZzan + result.GsonRkeiZou
            Else
                result.GruikeiGen = 0 : result.GsonRkeiGen = 0
            End If
        End If

        ' 期末残高
        result.GruikeiZan = result.GruikeiZzan + result.GruikeiZou - result.GruikeiGen
        result.GsonRkeiZan = result.GsonRkeiZzan + result.GsonRkeiZou - result.GsonRkeiGen

        ' 簿価期末残高
        result.BokaZan = result.SyutokZan - result.GruikeiZan - result.GsonRkeiZan
    End Sub

    ' ======================================================================
    '  返済関連集計 (Access版 m注記計算_SUB_返済関連)
    ' ======================================================================

    Private Shared Sub CalcHensaiRelated(
        result As ChukiCalcResult,
        params As ChukiCalcParams,
        hensaiSch As List(Of HensaiScheduleEntry),
        syutok As Double,
        zKimatDt As Date,
        y1KishuDt As Date, y1KimatDt As Date,
        y2KishuDt As Date, y3KishuDt As Date,
        y4KishuDt As Date, y5KishuDt As Date,
        y5KimatDt As Date
    )
        Dim matsubiF As Boolean = params.MatsubiShuryoKichuMasshoF
        Dim kaiyakSaimu As Double = 0
        Dim kaiyakMibRisoku As Double = 0
        Dim kaiyakGson As Double = 0

        For Each entry In hensaiSch
            ' 中途解約されていない行のみ解約抹消値を更新
            If Not entry.CkaiykF Then
                kaiyakSaimu = entry.GanponZanE
                kaiyakMibRisoku = entry.RisokuMibZanE
                kaiyakGson = entry.GsonZanE
            End If

            ' 期末残高設定
            If entry.GetuEnDt = zKimatDt Then
                ' 前期末
                result.LgnpnZzan = entry.GanponZanE
                result.LrsokZzan = entry.RisokuZanE
                result.GsonZzan = entry.GsonZanE
            ElseIf entry.GetuEnDt = params.KimatDt Then
                ' 期末
                result.LgnpnZan = entry.GanponZanE
                result.LrsokZan = entry.RisokuZanE
                result.RisokuMibZan = entry.RisokuMibZanE
                result.GsonZan = entry.GsonZanE
            ElseIf entry.GetuEnDt = y1KimatDt Then
                ' 翌期以降1年目期末
                result.LgnpnZan1Cho = entry.GanponZanE
                result.LrsokZan1Cho = entry.RisokuZanE
            ElseIf entry.GetuEnDt = y5KimatDt Then
                ' 翌期以降5年目期末
                result.LgnpnZan5Cho = entry.GanponZanE
                result.LrsokZan5Cho = entry.RisokuZanE
            End If

            ' 各期の発生額
            If entry.GetuStDt < params.KishuDt Then
                ' 前期以前
                If Not entry.CkaiykF Then
                    result.LbSonekiRuikei = ScheduleHelper.GKasan(True, result.LbSonekiRuikei, entry.LbSonekiS, entry.LbSonekiE)
                End If

            ElseIf entry.GetuStDt >= params.KishuDt AndAlso entry.GetuStDt <= params.KimatDt Then
                ' 当期
                If Not entry.CkaiykF Then
                    result.LsryoToki += entry.NetLsryoS + entry.NetLsryoE + entry.ZanryoSeisanE
                    result.LgnpnToki += entry.GanponS + entry.GanponE
                    result.LrsokToki += entry.RisokuShriS + entry.RisokuShriE
                    result.RisokuHasseiToki += entry.RisokuHasseiS + entry.RisokuHasseiE
                    result.GsonTkToki += entry.GsonTk
                    result.IjiknrToki = ScheduleHelper.GKasan(True, result.IjiknrToki, entry.IjiknrS, entry.IjiknrE)
                    result.LbSonekiToki = ScheduleHelper.GKasan(True, result.LbSonekiToki, entry.LbSonekiS, entry.LbSonekiE)
                    result.LbSonekiRuikei = ScheduleHelper.GKasan(True, result.LbSonekiRuikei, entry.LbSonekiS, entry.LbSonekiE)
                End If

            ElseIf entry.GetuStDt >= y1KishuDt AndAlso entry.GetuStDt < y2KishuDt Then
                ' 1年内
                result.LgnpnZan1Nai += entry.GanponS + entry.GanponE
                result.LrsokZan1Nai += entry.RisokuShriS + entry.RisokuShriE

            ElseIf entry.GetuStDt >= y2KishuDt AndAlso entry.GetuStDt < y3KishuDt Then
                ' 2年内
                result.LgnpnZan2Nai += entry.GanponS + entry.GanponE
                result.LrsokZan2Nai += entry.RisokuShriS + entry.RisokuShriE

            ElseIf entry.GetuStDt >= y3KishuDt AndAlso entry.GetuStDt < y4KishuDt Then
                ' 3年内
                result.LgnpnZan3Nai += entry.GanponS + entry.GanponE
                result.LrsokZan3Nai += entry.RisokuShriS + entry.RisokuShriE

            ElseIf entry.GetuStDt >= y4KishuDt AndAlso entry.GetuStDt < y5KishuDt Then
                ' 4年内
                result.LgnpnZan4Nai += entry.GanponS + entry.GanponE
                result.LrsokZan4Nai += entry.RisokuShriS + entry.RisokuShriE

            ElseIf entry.GetuStDt >= y5KishuDt AndAlso entry.GetuStDt <= y5KimatDt Then
                ' 5年内
                result.LgnpnZan5Nai += entry.GanponS + entry.GanponE
                result.LrsokZan5Nai += entry.RisokuShriS + entry.RisokuShriE
            End If
        Next

        ' -- 前期末残高調整
        If params.StartDt > zKimatDt Then
            result.LgnpnZzan = 0 : result.LrsokZzan = 0 : result.GsonZzan = 0
        End If

        If params.BCkaiykF Then
            If matsubiF Then
                If params.BRendDt < params.KishuDt Then
                    result.LgnpnZzan = 0 : result.LrsokZzan = 0 : result.GsonZzan = 0
                End If
            Else
                If params.BRendDt < zKimatDt Then
                    result.LgnpnZzan = 0 : result.LrsokZzan = 0 : result.GsonZzan = 0
                End If
            End If
        End If

        ' -- 期末残高調整
        If params.StartDt > params.KimatDt Then
            ClearAllPeriodBalances(result)
        End If

        If params.BCkaiykF Then
            If matsubiF Then
                If params.BRendDt <= params.KimatDt Then
                    ClearAllPeriodBalances(result)
                End If
            Else
                If params.BRendDt < params.KimatDt Then
                    ClearAllPeriodBalances(result)
                End If
            End If
        End If

        ' -- 解約抹消
        If params.BCkaiykF Then
            If matsubiF Then
                If params.BRendDt >= params.KishuDt AndAlso params.BRendDt <= params.KimatDt Then
                    result.LgnpnKaiyakGen = kaiyakSaimu
                    result.RisokuMibKaiyakGen = kaiyakMibRisoku
                    result.GsonKaiyakGen = kaiyakGson
                End If
            Else
                If params.BRendDt >= zKimatDt AndAlso params.BRendDt < params.KimatDt Then
                    result.LgnpnKaiyakGen = kaiyakSaimu
                    result.RisokuMibKaiyakGen = kaiyakMibRisoku
                    result.GsonKaiyakGen = kaiyakGson
                End If
            End If
        End If

        ' -- オペレーティングリース調整 (Access版: これらのフィールドをNullに設定)
        If params.LeakbnId = CInt(LeaseKbn.Ope) Then
            result.GsonZzan = Nothing : result.GsonZan = Nothing
            result.GsonTkToki = Nothing : result.GsonKaiyakGen = Nothing
            result.LsryoToki = Nothing : result.LgnpnToki = Nothing
            result.LrsokToki = Nothing : result.IjiknrToki = Nothing
            result.LbSonekiToki = Nothing
        End If
    End Sub

    ''' <summary>期末残高と翌期以降の全残高を0クリアする</summary>
    Private Shared Sub ClearAllPeriodBalances(result As ChukiCalcResult)
        result.LgnpnZan = 0
        result.LgnpnZan1Nai = 0 : result.LgnpnZan1Cho = 0
        result.LgnpnZan2Nai = 0 : result.LgnpnZan3Nai = 0
        result.LgnpnZan4Nai = 0 : result.LgnpnZan5Nai = 0
        result.LgnpnZan5Cho = 0
        result.LrsokZan = 0
        result.LrsokZan1Nai = 0 : result.LrsokZan1Cho = 0
        result.LrsokZan2Nai = 0 : result.LrsokZan3Nai = 0
        result.LrsokZan4Nai = 0 : result.LrsokZan5Nai = 0
        result.LrsokZan5Cho = 0
        result.RisokuMibZan = 0 : result.GsonZan = 0
    End Sub

End Class
