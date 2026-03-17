' 計上エンジン用型定義 (Access版 cn_typ_mKEIJO / p_PublicVariable 相当)
' KeijoCalculationEngine / MonthlyJournalEngine で使用する型定義

''' <summary>返済方法 (Access版 engHENSAI_KIND)</summary>
Public Enum HensaiKind
    Teigaku = 1         ' 定額
    Kinto = 2           ' 均等
    HendoRiritsu = 3    ' 変動利率(リースベース)
End Enum

''' <summary>計上出力先テーブル区分</summary>
Public Enum KeijoTargetTable
    ChukiKeijo = 1   ' tw_s_chuki_keijo (注記計上ワーク)
    HenlKeijo = 2    ' tw_d_henl_keijo (変額仕訳ワーク)
End Enum

''' <summary>
''' 計上条件（画面入力パラメータ）
''' Access版 gKEIJO_Main の引数に相当
''' </summary>
Public Class KeijoJoken
    ''' <summary>明細単位 (1:物件 / 2:配賦)</summary>
    Public Property Meisai As ShriMeisai

    ''' <summary>対象区分 (1:リース / 2:保守 / 3:全部)</summary>
    Public Property Taisho As Integer

    ''' <summary>計上区分: 資産を含む</summary>
    Public Property KjkbnSisan As Boolean

    ''' <summary>計上区分: 費用を含む</summary>
    Public Property KjkbnHiyo As Boolean

    ''' <summary>ユーザー指定フィルタ条件 (SQL WHERE句断片)</summary>
    Public Property SaJoken As String

    ''' <summary>新法費用の返済方法 (画面指定)</summary>
    Public Property HensaiKindShinhoHiyo As HensaiKind

    ''' <summary>処理完了残高当月埋めフラグ</summary>
    Public Property ShoriEndF As Boolean

    ''' <summary>償却方法ID</summary>
    Public Property SkyakHoId As Integer
End Class

''' <summary>
''' 計上計算結果 (Access版 cn_typ_mKEIJO の計算結果部分)
''' CalcKeijoFromSchedule の出力。
''' </summary>
Public Class KeijoResult
    Public Property Soukaisu As Object = DBNull.Value
    Public Property SumikaisuZen As Integer = 0
    Public Property KeijoShriCnt As Integer = 0
    Public Property LsryoTotal As Double = 0
    Public Property LsryoToki As Double = 0
    Public Property ZeiTotal As Double = 0
    Public Property ZeiToki As Double = 0
    Public Property TaishoF As Boolean = False
    Public Property Zritu As Double = 0

    ' 配賦・科目情報 (計算後に設定)
    Public Property LcptId As Object = DBNull.Value
    Public Property HkmkId As Object = DBNull.Value
    Public Property LineId As Object = DBNull.Value
    Public Property Haifritu As Object = DBNull.Value
    Public Property HBcatId As Object = DBNull.Value
    Public Property Rsrvh1Id As Object = DBNull.Value
    Public Property HZokusei1 As Object = DBNull.Value
    Public Property HZokusei2 As Object = DBNull.Value
    Public Property HZokusei3 As Object = DBNull.Value
    Public Property HZokusei4 As Object = DBNull.Value
    Public Property HZokusei5 As Object = DBNull.Value

    ' 行区分・返済方法
    Public Property RecKbn As RecKbn = RecKbn.Teigaku
    Public Property HensaiKind As HensaiKind = HensaiKind.Teigaku
End Class

''' <summary>
''' 計上ワーク1行 (Access版 tw_S_CHUKI_KEIJO / tw_D_HENL_KEIJO の1レコードに相当)
''' KeijoCalculationEngine.ScheduleToWorkRows の出力。
''' </summary>
Public Class KeijoWorkRow
    ' 契約情報
    Public Property KykmId As Double
    Public Property KykhId As Double
    Public Property KykmNo As Double
    Public Property SaikaisuKykm As Object = DBNull.Value
    Public Property BuknNm As String = ""
    Public Property KykbnlNo As String = ""

    ' 計上条件
    Public Property KkbnId As Object = DBNull.Value
    Public Property KjkbnId As Object = DBNull.Value
    Public Property LeakbnId As Object = DBNull.Value
    Public Property LcptId As Object = DBNull.Value

    ' 計算パラメータ
    Public Property HensaiKind As HensaiKind = HensaiKind.Teigaku
    Public Property RecKbn As RecKbn = RecKbn.Teigaku

    ' スケジュール1行分
    Public Property ShriDt As Date
    Public Property SimeDt As Date
    Public Property Lsryo As Double = 0
    Public Property Zei As Double = 0
    Public Property Zritu As Double = 0
    Public Property ShhoId As Object = DBNull.Value
    Public Property MaeF As Boolean = False
    Public Property CkaiykF As Boolean = False

    ' 計上結果
    Public Property KeijoF As Boolean = False
    Public Property KejoDt As Object = DBNull.Value
    Public Property SumikaisuZen As Integer = 0
    Public Property KeijoShriCnt As Integer = 0
    Public Property LsryoTotal As Double = 0
    Public Property LsryoToki As Double = 0
    Public Property ZeiTotal As Double = 0
    Public Property ZeiToki As Double = 0

    ' 配賦情報
    Public Property LineId As Object = DBNull.Value
    Public Property Haifritu As Object = DBNull.Value
    Public Property HkmkId As Object = DBNull.Value
    Public Property HBcatId As Object = DBNull.Value
    Public Property Rsrvh1Id As Object = DBNull.Value
    Public Property HZokusei1 As Object = DBNull.Value
    Public Property HZokusei2 As Object = DBNull.Value
    Public Property HZokusei3 As Object = DBNull.Value
    Public Property HZokusei4 As Object = DBNull.Value
    Public Property HZokusei5 As Object = DBNull.Value

    ' 前払情報
    Public Property MaeZou As Double = 0
    Public Property MaeGen As Double = 0
    Public Property MzeiZou As Double = 0
    Public Property MzeiGen As Double = 0

    ' 減損情報
    Public Property GsonDt As Object = DBNull.Value

    ' 処理日
    Public Property ShoriDt As Date

    ' 出力先テーブル
    Public Property TargetTable As KeijoTargetTable = KeijoTargetTable.ChukiKeijo
End Class
