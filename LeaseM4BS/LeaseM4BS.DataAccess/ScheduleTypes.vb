' スケジュール生成用型定義
' Access版 cn_typ_gSch_償却 / cn_typ_gSch_返済 / cn_typ_gSch_減損 / cn_typ_m注記計算_IF に対応

''' <summary>償却方法 (Access版 engSKYAK_HO)</summary>
Public Enum ShokyakuHo
    Teigaku = 1     ' 定額法
    Teiritu = 2     ' 定率法
End Enum

''' <summary>先払/後払 (Access版 engRSOK_TMG)</summary>
Public Enum RsokTmg
    Atobarai = 1            ' 後払
    Sakibarai = 2           ' 先払
    AtobaraiKaishiKojo = 3  ' 後払開始控除
End Enum

''' <summary>計算方法区分 (Access版 cngRCALC_*)</summary>
Public Enum RcalcKind
    RisokuBunri = 1     ' 利子抜法 (利息分離)
    Risikomi = 2        ' 利子込法
End Enum

''' <summary>リース区分 (Access版 engLEAKBN)</summary>
Public Enum LeaseKbn
    Iten = 1        ' 移転ファイナンスリース
    Itengai = 3     ' 移転外ファイナンスリース
    Ope = 4         ' オペレーティングリース
End Enum

''' <summary>キャッシュフロー区分 (Access版 eng_KIND)</summary>
Public Enum CashFlowKind
    Yakujo = 0      ' 約定
    Seikyu = 1      ' 請求
End Enum

' ======================================================================
'  減損スケジュール (Access版 cn_typ_gSch_減損)
' ======================================================================

''' <summary>減損スケジュール1行 (Access版 cn_typ_gSch_減損)</summary>
Public Class GsonScheduleEntry
    ''' <summary>年度</summary>
    Public Property Nen As Integer
    ''' <summary>月度</summary>
    Public Property Getu As Integer
    ''' <summary>処理タイミング (0:月度末, 1:月度初)</summary>
    Public Property GsonTmg As Integer
    ''' <summary>減損損失・月度初</summary>
    Public Property GsonRyoS As Double
    ''' <summary>減損損失・月度末</summary>
    Public Property GsonRyoE As Double
    ''' <summary>減損損失累計額・月度初</summary>
    Public Property GsonRkeiS As Double
    ''' <summary>減損損失累計額・月度末</summary>
    Public Property GsonRkeiE As Double
End Class

' ======================================================================
'  償却スケジュール (Access版 cn_typ_gSch_償却)
' ======================================================================

''' <summary>償却スケジュール1行 (Access版 cn_typ_gSch_償却)</summary>
Public Class ShokyakuScheduleEntry
    ''' <summary>年度</summary>
    Public Property Nen As Integer
    ''' <summary>月度</summary>
    Public Property Getu As Integer
    ''' <summary>月度開始日</summary>
    Public Property GetuStDt As Date
    ''' <summary>月度終了日</summary>
    Public Property GetuEnDt As Date
    ''' <summary>期間開始日</summary>
    Public Property LkikanStDt As Date
    ''' <summary>期間終了日</summary>
    Public Property LkikanEnDt As Date
    ''' <summary>中途解約フラグ</summary>
    Public Property CkaiykF As Boolean
    ''' <summary>償却率 (定率法の場合のみ。定額法はNothing)</summary>
    Public Property SkyakRitu As Double? = Nothing
    ''' <summary>減価償却累計額・月度初</summary>
    Public Property SkyakRkeiS As Double
    ''' <summary>減価償却累計額・月度末</summary>
    Public Property SkyakRkeiE As Double
    ''' <summary>残高・月度初</summary>
    Public Property ZanS As Double
    ''' <summary>残高・月度末</summary>
    Public Property ZanE As Double
    ''' <summary>9分の10残高・月度初 (定率法のみ)</summary>
    Public Property Zan109S As Double? = Nothing
    ''' <summary>9分の10残高・月度末 (定率法のみ)</summary>
    Public Property Zan109E As Double? = Nothing
    ''' <summary>減価償却費</summary>
    Public Property Skyak As Double
    ''' <summary>減損損失・月度初</summary>
    Public Property GsonRyoS As Double
    ''' <summary>減損損失・月度末</summary>
    Public Property GsonRyoE As Double
    ''' <summary>減損損失累計額・月度初</summary>
    Public Property GsonRkeiS As Double
    ''' <summary>減損損失累計額・月度末</summary>
    Public Property GsonRkeiE As Double
End Class

' ======================================================================
'  返済スケジュール (Access版 cn_typ_gSch_返済)
' ======================================================================

''' <summary>返済スケジュール1行 (Access版 cn_typ_gSch_返済)</summary>
Public Class HensaiScheduleEntry
    ''' <summary>年度</summary>
    Public Property Nen As Integer
    ''' <summary>月度</summary>
    Public Property Getu As Integer
    ''' <summary>月度開始日</summary>
    Public Property GetuStDt As Date
    ''' <summary>月度終了日</summary>
    Public Property GetuEnDt As Date
    ''' <summary>期間開始日</summary>
    Public Property LkikanStDt As Date
    ''' <summary>期間終了日</summary>
    Public Property LkikanEnDt As Date
    ''' <summary>中途解約フラグ</summary>
    Public Property CkaiykF As Boolean

    ' 支払額
    ''' <summary>支払額・月度初</summary>
    Public Property CashS As Double
    ''' <summary>支払額・月度末</summary>
    Public Property CashE As Double
    ''' <summary>支払額残高・月度末</summary>
    Public Property CashZanE As Double

    ' NET支払リース料
    ''' <summary>NET支払リース料・月度初</summary>
    Public Property NetLsryoS As Double
    ''' <summary>NET支払リース料・月度末</summary>
    Public Property NetLsryoE As Double
    ''' <summary>NET支払リース料残高・月度末</summary>
    Public Property NetLsryoZanE As Double

    ' 残価保証
    ''' <summary>残価保証清算額・月度末</summary>
    Public Property ZanryoSeisanE As Double
    ''' <summary>残価保証未清算残高・月度末</summary>
    Public Property ZanryoMsZanE As Double

    ' 維持管理費用
    ''' <summary>維持管理費用・月度初</summary>
    Public Property IjiknrS As Double? = Nothing
    ''' <summary>維持管理費用・月度末</summary>
    Public Property IjiknrE As Double? = Nothing

    ' リースバック損益
    ''' <summary>リースバック繰延損益・月度初</summary>
    Public Property LbSonekiS As Double? = Nothing
    ''' <summary>リースバック繰延損益・月度末</summary>
    Public Property LbSonekiE As Double? = Nothing

    ' 利息
    ''' <summary>発生利息・月度初</summary>
    Public Property RisokuHasseiS As Double
    ''' <summary>発生利息・月度末</summary>
    Public Property RisokuHasseiE As Double
    ''' <summary>支払利息・月度初</summary>
    Public Property RisokuShriS As Double
    ''' <summary>支払利息・月度末</summary>
    Public Property RisokuShriE As Double
    ''' <summary>未払利息残高・月度初</summary>
    Public Property RisokuMibZanS As Double
    ''' <summary>未払利息残高・月度末</summary>
    Public Property RisokuMibZanE As Double
    ''' <summary>利息残高・月度初</summary>
    Public Property RisokuZanS As Double
    ''' <summary>利息残高・月度末</summary>
    Public Property RisokuZanE As Double

    ' 元本
    ''' <summary>返済元本・月度初</summary>
    Public Property GanponS As Double
    ''' <summary>返済元本・月度末</summary>
    Public Property GanponE As Double
    ''' <summary>元本残高・月度初</summary>
    Public Property GanponZanS As Double
    ''' <summary>元本残高・月度末</summary>
    Public Property GanponZanE As Double

    ' 減損
    ''' <summary>減損損失・月度初</summary>
    Public Property GsonRyoS As Double
    ''' <summary>減損損失・月度末</summary>
    Public Property GsonRyoE As Double
    ''' <summary>減損損失累計額・月度初</summary>
    Public Property GsonRkeiS As Double
    ''' <summary>減損損失累計額・月度末</summary>
    Public Property GsonRkeiE As Double
    ''' <summary>リース資産減損勘定の取崩額</summary>
    Public Property GsonTk As Double
    ''' <summary>リース資産減損勘定の残高・月度初</summary>
    Public Property GsonZanS As Double
    ''' <summary>リース資産減損勘定の残高・月度末</summary>
    Public Property GsonZanE As Double
End Class

' ======================================================================
'  注記用支払スケジュール (Access版 cn_typ_gSch_支払_注記用)
' ======================================================================

''' <summary>注記用支払スケジュール1行 (Access版 cn_typ_gSch_支払_注記用)</summary>
Public Class ShiharaiSchEntry
    ''' <summary>支払年月日</summary>
    Public Property ShriDt As Date
    ''' <summary>締日</summary>
    Public Property SimeDt As Date
    ''' <summary>計上日 (請求ベース=締日, 約定=支払日)</summary>
    Public Property KeijDt As Date
    ''' <summary>支払リース料</summary>
    Public Property Cash As Double
    ''' <summary>中途解約フラグ</summary>
    Public Property CkaiykF As Boolean
    ''' <summary>年度</summary>
    Public Property Nen As Integer
    ''' <summary>月度</summary>
    Public Property Getu As Integer
    ''' <summary>計上年度</summary>
    Public Property KeijNen As Integer
    ''' <summary>計上月度</summary>
    Public Property KeijGetu As Integer
End Class

' ======================================================================
'  注記計算パラメータ / 結果 (Access版 cn_typ_m注記計算_IF)
' ======================================================================

''' <summary>注記計算入力パラメータ (Access版 cn_typ_m注記計算_IF の入力部分)</summary>
Public Class ChukiCalcParams
    ' 期間
    Public Property KishuDt As Date             ' 期首日
    Public Property KimatDt As Date             ' 期末日

    ' 契約情報
    Public Property StartDt As Date             ' 開始日
    Public Property Lkikan As Integer           ' リース期間月数
    Public Property BRendDt As Date             ' 実際終了日
    Public Property BCkaiykF As Boolean         ' 中途解約フラグ

    ' 計算方法
    Public Property RcalcId As Integer          ' 計算方法区分 (1:利子抜, 2:利子込)
    Public Property SkyakHoId As Integer        ' 償却方法ID
    Public Property LeakbnId As Integer         ' リース区分
    Public Property HensaiKind As HensaiKind    ' 返済方法
    Public Property RsokTmg As RsokTmg         ' 先払/後払
    Public Property SkyuKjF As Boolean          ' 遡及計上フラグ

    ' 金額
    Public Property BSlsryo As Double           ' 総額リース料
    Public Property BIjiknr As Double           ' 維持管理費用
    Public Property BZanryo As Double           ' 残価保証額
    Public Property BSyutok As Double           ' 取得価額相当額
    Public Property KsanRitu As Double          ' 計算利子率
    Public Property BLbSoneki As Double?        ' リースバック売却損益
    Public Property LbChukiF As Boolean         ' リースバック注記フラグ

    ' 末日終了モード
    Public Property MatsubiShuryoKichuMasshoF As Boolean = True  ' True=期中抹消(新), False=翌期抹消(従来)

    ' 決算日 (締日)
    Public Property KessanBi As Integer = 31    ' 決算日 (31=月末)
End Class

''' <summary>注記計算結果 (Access版 cn_typ_m注記計算_IF の出力部分)</summary>
Public Class ChukiCalcResult
    ' 取得価額
    Public Property SyutokZzan As Double        ' 前期末残高
    Public Property SyutokZou As Double         ' 当期増加
    Public Property SyutokGen As Double         ' 当期減少
    Public Property SyutokZan As Double         ' 期末残高

    ' 減価償却累計額
    Public Property GruikeiZzan As Double       ' 前期末残高
    Public Property GruikeiZou As Double        ' 当期増加 (=当期減価償却費)
    Public Property GruikeiGen As Double        ' 当期減少
    Public Property GruikeiZan As Double        ' 期末残高
    Public Property SkyakRitu As Double?        ' 償却率

    ' 減損損失累計額
    Public Property GsonRkeiZzan As Double      ' 前期末残高
    Public Property GsonRkeiZou As Double       ' 当期増加
    Public Property GsonRkeiGen As Double       ' 当期減少
    Public Property GsonRkeiZan As Double       ' 期末残高

    ' 簿価
    Public Property BokaZan As Double           ' 期末残高

    ' リース料元本 (LGNPN)
    Public Property LgnpnZzan As Double         ' 前期末残高
    Public Property LgnpnZan As Double          ' 期末残高
    Public Property LgnpnZan1Nai As Double      ' 1年内
    Public Property LgnpnZan1Cho As Double      ' 1年超
    Public Property LgnpnZan2Nai As Double      ' 2年内
    Public Property LgnpnZan3Nai As Double      ' 3年内
    Public Property LgnpnZan4Nai As Double      ' 4年内
    Public Property LgnpnZan5Nai As Double      ' 5年内
    Public Property LgnpnZan5Cho As Double      ' 5年超

    ' リース料利息 (LRSOK)
    Public Property LrsokZzan As Double         ' 前期末残高
    Public Property LrsokZan As Double          ' 期末残高
    Public Property LrsokZan1Nai As Double      ' 1年内
    Public Property LrsokZan1Cho As Double      ' 1年超
    Public Property LrsokZan2Nai As Double      ' 2年内
    Public Property LrsokZan3Nai As Double      ' 3年内
    Public Property LrsokZan4Nai As Double      ' 4年内
    Public Property LrsokZan5Nai As Double      ' 5年内
    Public Property LrsokZan5Cho As Double      ' 5年超

    ' 未払利息
    Public Property RisokuMibZan As Double       ' 期末残高

    ' リース資産減損勘定 (OPEリースではNothing=Access版Null)
    Public Property GsonZzan As Double?         ' 前期末残高
    Public Property GsonZan As Double?          ' 期末残高

    ' 当期発生 (OPEリースではNothing=Access版Null)
    Public Property LsryoToki As Double?        ' リース料
    Public Property LgnpnToki As Double?        ' 元本
    Public Property LrsokToki As Double?        ' 利息
    Public Property RisokuHasseiToki As Double   ' 発生利息
    Public Property GsonTkToki As Double?       ' 減損勘定取崩額
    Public Property IjiknrToki As Double?       ' 維持管理費用
    Public Property LbSonekiToki As Double?     ' リースバック損益
    Public Property LbSonekiRuikei As Double?   ' リースバック損益累計

    ' 解約抹消 (OPEリースではNothing=Access版Null)
    Public Property LgnpnKaiyakGen As Double?   ' 債務
    Public Property RisokuMibKaiyakGen As Double ' 未払利息
    Public Property GsonKaiyakGen As Double?    ' 減損勘定
End Class

' ======================================================================
'  ヘルパー関数
' ======================================================================

''' <summary>スケジュール計算用ヘルパー (Access版 gInt, gNvl, g加算, g月度 等)</summary>
Public Class ScheduleHelper

    ''' <summary>
    ''' Access版 gInt 相当: Int(CStr(value))
    ''' VBA の CStr は15桁有効数字で文字列化し、Int() で切捨てる。
    ''' CStr往復により浮動小数点誤差 (例: 3744.9999999999996→3745) が除去される。
    ''' </summary>
    Public Shared Function GInt(value As Double) As Double
        ' VBA CStr(Double) は G15 相当 → 15桁有効数字で丸め
        Dim s As String = value.ToString("G15")
        Dim cleaned As Double = Double.Parse(s)
        Return Math.Floor(cleaned)
    End Function

    ''' <summary>
    ''' Null安全加算 (Access版 g加算)
    ''' skipNull=True かつ全項がNothing(Null)の場合は Nothing を返す。
    ''' skipNull=False の場合はNothingを0として加算し常にDoubleを返す。
    ''' </summary>
    Public Shared Function GKasan(skipNull As Boolean, ParamArray values() As Double?) As Double?
        Dim result As Double = 0
        Dim allNull As Boolean = True
        For Each v In values
            If v.HasValue Then
                allNull = False
                result += v.Value
            End If
        Next
        If skipNull AndAlso allNull Then Return Nothing
        Return result
    End Function

    ''' <summary>
    ''' 月度の年・月を求める (Access版 g月度YYYYandMMGet)
    ''' ig決算日=31(月末)前提: 日付のYear/Monthがそのまま年度/月度
    ''' </summary>
    Public Shared Sub GetGetuYYYYMM(dt As Date, ByRef nen As Integer, ByRef getu As Integer)
        nen = dt.Year
        getu = dt.Month
    End Sub

    ''' <summary>
    ''' 月度の初日を求める (Access版 g月度初YMDGet)
    ''' ig決算日=31(月末)前提: 月初1日
    ''' </summary>
    Public Shared Function GetGetuShoNichi(dt As Date) As Date
        Return New Date(dt.Year, dt.Month, 1)
    End Function

    ''' <summary>
    ''' 月末日チェック (Access版 g月末日Check)
    ''' </summary>
    Public Shared Function IsMonthEnd(dt As Date) As Boolean
        Return dt.Day = DateTime.DaysInMonth(dt.Year, dt.Month)
    End Function

End Class
