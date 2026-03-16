' Access版 pc_SHRI_COM の gMakeCASH_SCH_T / gMakeCASH_SCH_H / gMakeCASH_SCH_COM を移植
' キャッシュスケジュール（支払予定配列）を生成するクラス

Imports Npgsql

Public Class CashScheduleBuilder

    ' 各月の最終日テーブル (Access版 imMonthLastDay_TBL)
    Private Shared ReadOnly MonthLastDay As Integer() = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

    ''' <summary>
    ''' 定額キャッシュスケジュール生成 (Access版 gMakeCASH_SCH_T)
    ''' 前払→第1回→第2回→第3回以降(支払間隔で生成)
    ''' </summary>
    Public Shared Function BuildTeigakuSchedule(
        ktmg As ShriKtmg,
        kishuDt As Date, kimatDt As Date, jenchoLastDt As Date,
        jenchoF As Boolean,
        shriKn As Integer, shriCnt As Object,
        sshriKnM As Integer, sshriKn1 As Integer, sshriKn2 As Integer, sshriKn3 As Integer,
        shhoMId As Object, shho1Id As Object, shho2Id As Object, shho3Id As Object,
        maeDt As Object, shriDt1 As Date, shriDt2 As Object, shriDt3 As Integer,
        zritu As Double,
        klsryo As Double, kzei As Double, mlsryo As Double, mzei As Double,
        ckaiykEsdtT As Object
    ) As List(Of CashScheduleEntry)

        Dim schedule As New List(Of CashScheduleEntry)

        ' 第2回支払日の年月を保持
        Dim shriDt2YY As Integer = 0
        Dim shriDt2MM As Integer = 0
        If shriDt2 IsNot Nothing AndAlso Not IsDBNull(shriDt2) Then
            Dim dt2 = CDate(shriDt2)
            shriDt2YY = dt2.Year
            shriDt2MM = dt2.Month
        End If

        ' --------------------------------------------------
        ' 前払パート
        ' --------------------------------------------------
        If maeDt IsNot Nothing AndAlso Not IsDBNull(maeDt) AndAlso (mlsryo <> 0 OrElse mzei <> 0) Then
            Dim entry As New CashScheduleEntry()
            entry.ShriDt = CDate(maeDt)
            entry.SimeDt = CalcSimeDtFromShriDt(CDate(maeDt), sshriKnM)
            entry.Lsryo = mlsryo
            entry.Zritu = zritu
            entry.Zei = mzei
            entry.MaeF = True
            entry.CkaiykF = False
            entry.ShhoId = shhoMId
            entry.SshriKn = sshriKnM
            entry.LsryoHsum = 0
            entry.ZeiHsum = 0
            schedule.Add(entry)
        End If

        ' --------------------------------------------------
        ' 第1回以降支払パート
        ' --------------------------------------------------
        If jenchoF AndAlso (shriCnt Is Nothing OrElse IsDBNull(shriCnt)) Then
            ' *** 自動更新パターン ***
            Dim i As Integer = 1
            Do
                Dim vlShriDt As Date
                Dim vlSimeDt As Date
                Dim vlShhoId As Object
                Dim vlSshriKn As Integer

                If i = 1 Then
                    vlShriDt = shriDt1
                    vlSimeDt = CalcSimeDtFromShriDt(vlShriDt, sshriKn1)
                    vlShhoId = shho1Id
                    vlSshriKn = sshriKn1
                ElseIf i = 2 Then
                    vlShriDt = CDate(shriDt2)
                    vlSimeDt = CalcSimeDtFromShriDt(vlShriDt, sshriKn2)
                    vlShhoId = shho2Id
                    vlSshriKn = sshriKn2
                Else
                    vlShriDt = CalcNextShriDt(shriDt2YY, shriDt2MM, (i - 2) * shriKn, shriDt3)
                    If ktmg = ShriKtmg.SimeDtBase Then
                        vlSimeDt = CalcSimeDtB(vlShriDt.Year, vlShriDt.Month, vlShriDt.Day, sshriKn3)
                    Else
                        vlSimeDt = vlShriDt
                    End If
                    vlShhoId = shho3Id
                    vlSshriKn = sshriKn3
                End If

                ' LOOPの終了判定
                If ktmg = ShriKtmg.ShriDtBase Then
                    If vlShriDt > jenchoLastDt Then Exit Do
                Else
                    If vlSimeDt > jenchoLastDt Then Exit Do
                End If

                Dim entry As New CashScheduleEntry()
                entry.ShriDt = vlShriDt
                entry.SimeDt = vlSimeDt
                entry.Lsryo = klsryo
                entry.Zritu = zritu
                entry.Zei = kzei
                entry.MaeF = False
                entry.CkaiykF = False
                If ckaiykEsdtT IsNot Nothing AndAlso Not IsDBNull(ckaiykEsdtT) Then
                    If entry.ShriDt > CDate(ckaiykEsdtT) Then
                        entry.CkaiykF = True
                    End If
                End If
                entry.ShhoId = vlShhoId
                entry.SshriKn = vlSshriKn
                entry.LsryoHsum = 0
                entry.ZeiHsum = 0
                schedule.Add(entry)

                i += 1
            Loop
        Else
            ' *** 非自動更新パターン ***
            Dim cnt As Integer = If(shriCnt Is Nothing OrElse IsDBNull(shriCnt), 0, CInt(shriCnt))
            For i As Integer = 1 To cnt
                Dim entry As New CashScheduleEntry()

                If i = 1 Then
                    entry.ShriDt = shriDt1
                    If ktmg = ShriKtmg.SimeDtBase Then
                        entry.SimeDt = CalcSimeDtFromShriDt(entry.ShriDt, sshriKn1)
                    Else
                        entry.SimeDt = entry.ShriDt
                    End If
                    entry.ShhoId = shho1Id
                    entry.SshriKn = sshriKn1
                ElseIf i = 2 Then
                    entry.ShriDt = CDate(shriDt2)
                    If ktmg = ShriKtmg.SimeDtBase Then
                        entry.SimeDt = CalcSimeDtFromShriDt(entry.ShriDt, sshriKn2)
                    Else
                        entry.SimeDt = entry.ShriDt
                    End If
                    entry.ShhoId = shho2Id
                    entry.SshriKn = sshriKn2
                Else
                    entry.ShriDt = CalcNextShriDt(shriDt2YY, shriDt2MM, (i - 2) * shriKn, shriDt3)
                    If ktmg = ShriKtmg.SimeDtBase Then
                        entry.SimeDt = CalcSimeDtB(entry.ShriDt.Year, entry.ShriDt.Month, entry.ShriDt.Day, sshriKn3)
                    Else
                        entry.SimeDt = entry.ShriDt
                    End If
                    entry.ShhoId = shho3Id
                    entry.SshriKn = sshriKn3
                End If

                entry.Lsryo = klsryo
                entry.Zritu = zritu
                entry.Zei = kzei
                entry.MaeF = False
                entry.CkaiykF = False
                If ckaiykEsdtT IsNot Nothing AndAlso Not IsDBNull(ckaiykEsdtT) Then
                    If entry.ShriDt > CDate(ckaiykEsdtT) Then
                        entry.CkaiykF = True
                    End If
                End If
                entry.LsryoHsum = 0
                entry.ZeiHsum = 0
                schedule.Add(entry)
            Next
        End If

        Return schedule
    End Function

    ''' <summary>
    ''' 変額キャッシュスケジュール生成 (Access版 gMakeCASH_SCH_H)
    ''' D_HENL テーブルから読み取り
    ''' </summary>
    Public Shared Function BuildHengakuSchedule(
        crud As CrudHelper, kykmId As Double, ckaiykEsdtH As Object
    ) As List(Of CashScheduleEntry)

        Dim schedule As New List(Of CashScheduleEntry)
        Dim sql = "SELECT * FROM d_henl WHERE kykm_id = @kykm_id ORDER BY line_id"
        Dim prms As New List(Of NpgsqlParameter)
        prms.Add(New NpgsqlParameter("@kykm_id", kykmId))

        Dim dt = crud.GetDataTable(sql, prms)
        For Each row As System.Data.DataRow In dt.Rows
            Dim shriCnt As Integer = CInt(row("shri_cnt"))
            Dim shriDt1 As Date = CDate(row("shri_dt1"))
            Dim shriKn As Integer = CInt(row("shri_kn"))
            Dim sshriKn As Integer = CInt(row("sshri_kn"))

            ' 初回支払日の日を取得 (月末判定)
            Dim ilDay As Integer
            If shriDt1.Day >= DateTime.DaysInMonth(shriDt1.Year, shriDt1.Month) Then
                ilDay = 31
            Else
                ilDay = shriDt1.Day
            End If

            For i As Integer = 1 To shriCnt
                Dim entry As New CashScheduleEntry()
                entry.ShriDt = CalcNextShriDt(shriDt1.Year, shriDt1.Month, (i - 1) * shriKn, ilDay)
                entry.SimeDt = CalcSimeDtB(entry.ShriDt.Year, entry.ShriDt.Month, entry.ShriDt.Day, sshriKn)
                entry.Lsryo = If(IsDBNull(row("klsryo")), 0.0, CDbl(row("klsryo")))
                entry.Zritu = If(IsDBNull(row("zritu")), 0.0, CDbl(row("zritu")))
                entry.Zei = If(IsDBNull(row("kzei")), 0.0, CDbl(row("kzei")))
                entry.MaeF = False
                entry.CkaiykF = False
                If ckaiykEsdtH IsNot Nothing AndAlso Not IsDBNull(ckaiykEsdtH) Then
                    If entry.ShriDt > CDate(ckaiykEsdtH) Then
                        entry.CkaiykF = True
                    End If
                End If
                entry.ShhoId = If(IsDBNull(row("shho_id")), Nothing, row("shho_id"))
                entry.SshriKn = sshriKn
                entry.LsryoHsum = 0
                entry.ZeiHsum = 0
                schedule.Add(entry)
            Next
        Next

        Return schedule
    End Function

    ''' <summary>
    ''' 汎用キャッシュスケジュール生成 (Access版 gMakeCASH_SCH_COM)
    ''' 付随費用の単一レコード用
    ''' </summary>
    Public Shared Function BuildCommonSchedule(
        ktmg As ShriKtmg,
        shriKn As Integer, shriCnt As Integer, sshriKn As Integer,
        shhoId As Object, shriDt1 As Date,
        zritu As Double, klsryo As Double, kzei As Double,
        ckaiykEsdt As Object
    ) As List(Of CashScheduleEntry)

        Dim schedule As New List(Of CashScheduleEntry)
        Dim ilDay As Integer
        If shriDt1.Day >= DateTime.DaysInMonth(shriDt1.Year, shriDt1.Month) Then
            ilDay = 31
        Else
            ilDay = shriDt1.Day
        End If

        For i As Integer = 1 To shriCnt
            Dim entry As New CashScheduleEntry()
            entry.ShriDt = CalcNextShriDt(shriDt1.Year, shriDt1.Month, (i - 1) * shriKn, ilDay)
            If ktmg = ShriKtmg.SimeDtBase Then
                entry.SimeDt = CalcSimeDtB(entry.ShriDt.Year, entry.ShriDt.Month, entry.ShriDt.Day, sshriKn)
            Else
                entry.SimeDt = entry.ShriDt
            End If
            entry.Lsryo = klsryo
            entry.Zritu = zritu
            entry.Zei = kzei
            entry.MaeF = False
            entry.CkaiykF = False
            If ckaiykEsdt IsNot Nothing AndAlso Not IsDBNull(ckaiykEsdt) Then
                If entry.ShriDt > CDate(ckaiykEsdt) Then
                    entry.CkaiykF = True
                End If
            End If
            entry.ShhoId = shhoId
            entry.SshriKn = sshriKn
            entry.LsryoHsum = 0
            entry.ZeiHsum = 0
            schedule.Add(entry)
        Next

        Return schedule
    End Function

    ' --------------------------------------------------
    ' ヘルパー関数
    ' --------------------------------------------------

    ''' <summary>
    ''' 次回支払日計算 (Access版 gNEXT_SHRI_DT_CALC_B)
    ''' 指定年月の指定月数後の年月日を求める
    ''' </summary>
    Public Shared Function CalcNextShriDt(baseYY As Integer, baseMM As Integer, monthOffset As Integer, dayOfMonth As Integer) As Date
        Dim yy As Integer = baseYY
        Dim mm As Integer = baseMM + monthOffset
        If mm > 12 Then
            yy = yy + CInt(Math.Floor((mm - 1) / 12.0))
            mm = ((mm - 1) Mod 12) + 1
        End If

        Dim dd As Integer
        If mm = 2 Then
            If (yy Mod 4 = 0 AndAlso yy Mod 100 <> 0) OrElse (yy Mod 400 = 0) Then
                dd = 29
            Else
                dd = 28
            End If
        Else
            dd = MonthLastDay(mm)
        End If
        If dayOfMonth < dd Then
            dd = dayOfMonth
        End If

        Return New Date(yy, mm, dd)
    End Function

    ''' <summary>
    ''' 締日計算 (Access版 gSIME_DT_CALC_B)
    ''' 締日=月末(ig締日=31)前提で計算
    ''' </summary>
    Public Shared Function CalcSimeDtB(shriDtYY As Integer, shriDtMM As Integer, shriDtDD As Integer, sshriKn As Integer) As Date
        Dim yy As Integer = shriDtYY
        Dim mm As Integer = shriDtMM - sshriKn

        ' ig締日=31 (月末締め) を前提とする
        If mm <= 0 Then
            yy = yy + CInt(Math.Floor(mm / 12.0)) - 1
            mm = (mm Mod 12) + 12
            If mm = 0 Then mm = 12
        End If

        Dim dd As Integer
        If mm = 2 Then
            If (yy Mod 4 = 0 AndAlso yy Mod 100 <> 0) OrElse (yy Mod 400 = 0) Then
                dd = 29
            Else
                dd = 28
            End If
        Else
            dd = MonthLastDay(mm)
        End If

        Return New Date(yy, mm, dd)
    End Function

    ''' <summary>
    ''' 締日取得 (Access版 g締日Get に相当): 支払日と締支払間隔から締日を算出
    ''' </summary>
    Private Shared Function CalcSimeDtFromShriDt(shriDt As Date, sshriKn As Integer) As Date
        Return CalcSimeDtB(shriDt.Year, shriDt.Month, shriDt.Day, sshriKn)
    End Function

    ''' <summary>月末日取得 (Access版 g末日YMDGet に相当)</summary>
    Public Shared Function GetMonthEndDate(dt As Date) As Date
        Return New Date(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month))
    End Function

End Class
