Imports System.Math

''' <summary>
''' 契約書・物件の計算ロジック共通ヘルパー
''' Access版 pc_f_KYKH の計算関数を移植
''' </summary>
Public NotInheritable Class ContractCalcHelper

        Private Sub New()
        End Sub

        ' =========================================================
        '  終了日計算 (Access版: pc_f_KYKH L5568 gSetCTL_END_DT)
        '  = 開始日 + リース期間(月) - 1日
        ' =========================================================
        ''' <summary>終了日を算出する</summary>
        ''' <param name="startDt">開始日</param>
        ''' <param name="lkikan">リース期間(月数)</param>
        ''' <returns>終了日。入力不正時はNothing</returns>
        Public Shared Function CalcEndDt(startDt As Date?, lkikan As Integer) As Date?
            If startDt Is Nothing OrElse lkikan <= 0 Then Return Nothing
            Return startDt.Value.AddMonths(lkikan).AddDays(-1)
        End Function

        ' =========================================================
        '  最終支払日計算 (Access版: pc_f_KYKH L5526 gCALC_SHRI_EN_DT)
        '  支払回数=1 → DT1, =2 → DT2, >=3 → DT2+(回数-2)*間隔月 の日をDT3の日に置換
        ' =========================================================
        ''' <summary>最終支払日を算出する</summary>
        Public Shared Function CalcShriEnDt(shriCnt As Integer, shriKn As Integer,
                                            shriDt1 As Date?, shriDt2 As Date?,
                                            shriDt3Day As Integer) As Date?
            If shriCnt <= 0 Then Return Nothing

            If shriCnt = 1 Then
                Return shriDt1
            ElseIf shriCnt = 2 Then
                Return shriDt2
            Else
                ' 第3回以降
                If shriDt2 Is Nothing OrElse shriKn <= 0 Then Return Nothing
                Dim baseDate = shriDt2.Value.AddMonths((shriCnt - 2) * shriKn)
                ' 日をDT3の日に置換（月末超過時は月末に丸め）
                Dim targetDay = Math.Min(shriDt3Day, DateTime.DaysInMonth(baseDate.Year, baseDate.Month))
                Return New Date(baseDate.Year, baseDate.Month, targetDay)
            End If
        End Function

        ' =========================================================
        '  支払回数計算 (Access版: Form_f_KYKH L823 mSetCTL_SHRI_CNT)
        '  = INT((リース期間 - 前払回数) / 支払間隔)
        ' =========================================================
        ''' <summary>支払回数を算出する</summary>
        ''' <param name="lkikan">リース期間(月)</param>
        ''' <param name="mkaisu">前払回数</param>
        ''' <param name="shriKn">支払間隔(月)</param>
        ''' <returns>支払回数。計算不能時は-1</returns>
        Public Shared Function CalcShriCnt(lkikan As Integer, mkaisu As Integer, shriKn As Integer) As Integer
            If shriKn <= 0 Then Return -1
            Dim result = CInt(Math.Truncate(CDbl(lkikan - mkaisu) / shriKn))
            If result < 0 Then Return -1
            Return result
        End Function

        ' =========================================================
        '  総額計算 (Access版: pc_f_KYKH L5868 gCALC_SLSRYO)
        '  = 1回額 × 支払回数 + 前払額 + 変額合計
        ' =========================================================
        ''' <summary>リース料総額を算出する</summary>
        Public Shared Function CalcSlsryo(klsryo As Double, shriCnt As Integer,
                                          mlsryo As Double, henlSum As Double) As Double
            Return GAdd(klsryo * shriCnt, mlsryo, henlSum)
        End Function

        ' =========================================================
        '  税額計算 (Access版: pc_f_KYKH L5582 gSetCTL_ZGAKU)
        '  = INT(税抜額 × 税率)    ※切り捨て
        ' =========================================================
        ''' <summary>消費税額を算出する (切り捨て)</summary>
        Public Shared Function CalcZei(znuki As Double, zritu As Double) As Double
            Return Math.Truncate(znuki * zritu)
        End Function

        ' =========================================================
        '  税込額計算
        ' =========================================================
        ''' <summary>税込額 = 税抜額 + 税額</summary>
        Public Shared Function CalcZkomi(znuki As Double, zei As Double) As Double
            Return znuki + zei
        End Function

        ' =========================================================
        '  税込→税抜逆算 (Access版: pc_f_KYKH L5597)
        '  = INT(税込額 / (1 + 税率))
        ' =========================================================
        ''' <summary>税込額から税抜額を逆算する</summary>
        Public Shared Function CalcZnuki(zkomi As Double, zritu As Double) As Double
            If zritu <= 0 Then Return zkomi
            Return Math.Truncate(zkomi / (1 + zritu))
        End Function

        ' =========================================================
        '  配賦率から金額算出
        '  = INT(物件金額 × 配賦率 / 100)
        ' =========================================================
        ''' <summary>配賦率から配賦行金額を算出する</summary>
        Public Shared Function CalcHaifKingaku(bukkenKingaku As Double, haifritu As Double) As Double
            Return Math.Truncate(bukkenKingaku * haifritu / 100.0)
        End Function

        ' =========================================================
        '  配賦率算出 (金額から)
        '  = 配賦行金額 / 物件金額 × 100
        ' =========================================================
        ''' <summary>金額比率から配賦率を算出する</summary>
        Public Shared Function CalcHaifritu(haifKingaku As Double, bukkenKingaku As Double) As Double
            If bukkenKingaku = 0 Then Return 0.0
            Return Math.Round(haifKingaku / bukkenKingaku * 100.0, 2)
        End Function

        ' =========================================================
        '  gInt (Access版: p_Com.txt L910)
        '  型安全な整数変換 (切り捨て)
        ' =========================================================
        ''' <summary>Access版 gInt の再現。数値を整数に切り捨て。</summary>
        Public Shared Function GInt(v As Object) As Object
            If v Is Nothing OrElse IsDBNull(v) Then Return DBNull.Value
            Try
                Dim d = Convert.ToDouble(v)
                Return CInt(Math.Truncate(d))
            Catch
                Return DBNull.Value
            End Try
        End Function

        ' =========================================================
        '  g加算 (Access版: p_Com.txt L725)
        '  NULL 対応の加算
        ' =========================================================
        ''' <summary>複数値のNULL安全な加算</summary>
        Public Shared Function GAdd(ParamArray values() As Double) As Double
            Dim result As Double = 0
            For Each v In values
                result += v
            Next
            Return result
        End Function

End Class
