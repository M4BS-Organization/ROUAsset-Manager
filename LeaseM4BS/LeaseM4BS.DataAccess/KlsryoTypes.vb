' Access版 p_PublicVariable.txt の Enum / Type 定義を VB.NET に移植
' pc_SHRI_KLSRYO / pc_SHRI_COM で使用する型定義

''' <summary>行区分 (Access版 engSHRI_REC_KBN)</summary>
Public Enum RecKbn
    Teigaku = 1   ' 定額
    Hengaku = 2   ' 変額
    Fuzui = 3     ' 付随費用
End Enum

''' <summary>計上タイミング (Access版 engSHRI_KTMG)</summary>
Public Enum ShriKtmg
    Nothing_ = 0     ' 未設定
    SimeDtBase = 1   ' 締日ベース
    ShriDtBase = 2   ' 支払日ベース
    ShriRDtBase = 3  ' 前月支払日ベース
End Enum

''' <summary>明細単位 (Access版 engSHRI_MEISAI)</summary>
Public Enum ShriMeisai
    Kykm = 1    ' 物件単位
    Haif = 2    ' 配賦単位
End Enum

''' <summary>契約区分 (Access版 engKKBN)</summary>
Public Enum Kkbn
    Nothing_ = 0  ' 未設定
    Lease = 1     ' リース
    Rental = 2    ' レンタル
    Hoshu = 3     ' 保守
    Iten = 4      ' 移転リース
    Other = 99    ' その他
End Enum

''' <summary>計上区分 (Access版 engKJKBN)</summary>
Public Enum Kjkbn
    None = 0    ' 未設定
    Hiyo = 1    ' 費用
    Sisan = 2   ' 資産
End Enum

''' <summary>キャッシュスケジュール1エントリ (Access版 cn_typ_gSch_CASH)</summary>
Public Class CashScheduleEntry
    Public Property ShriDt As Date        ' 支払日
    Public Property SimeDt As Date        ' 締日
    Public Property Lsryo As Double       ' 支払額(税抜)
    Public Property Zritu As Double       ' 消費税率
    Public Property Zei As Double         ' 税額
    Public Property MaeF As Boolean       ' 前払フラグ
    Public Property CkaiykF As Boolean    ' 中途解約フラグ
    Public Property ShhoId As Object      ' 支払方法ID
    Public Property SshriKn As Integer    ' 締支払間隔
    Public Property LsryoHsum As Double   ' 配賦用累計(税抜)
    Public Property ZeiHsum As Double     ' 配賦用累計(税額)
End Class

''' <summary>期間リース料計算結果 (Access版 cn_typ_mKLSRYO)</summary>
Public Class KlsryoResult
    Public Property Soukaisu As Object      ' 総回数 (Null可)
    Public Property Sumikaisu As Object     ' 済回数 (Null可)
    Public Property LsryoTotal As Double    ' 税抜総額
    Public Property LsryoZzan As Object     ' 前期末残高 (Null可)
    Public Property LsryoToki As Double     ' 当期額
    Public Property LsryoTokig As Object()  ' 月別内訳(12ヶ月) (要素がNull可)
    Public Property LsryoZan As Object      ' 期末残高 (Null可)
    Public Property LsryoZan1Nai As Double  ' 翌期以降1年内
    Public Property LsryoZan2Nai As Double  ' 1〜2年
    Public Property LsryoZan3Nai As Double  ' 2〜3年
    Public Property LsryoZan4Nai As Double  ' 3〜4年
    Public Property LsryoZan5Nai As Double  ' 4〜5年
    Public Property LsryoZan5Cho As Double  ' 5年超
    Public Property LsryoZou As Object      ' 期中増加 (Null可)
    Public Property ZeiTotal As Double      ' 税額総額
    Public Property ZeiZzan As Object       ' 税額前期末残高 (Null可)
    Public Property ZeiToki As Double       ' 税額当期
    Public Property ZeiTokig As Object()    ' 税額月別(12ヶ月) (要素がNull可)
    Public Property ZeiZan As Object        ' 税額期末残高 (Null可)
    Public Property ZeiZan1Nai As Double
    Public Property ZeiZan2Nai As Double
    Public Property ZeiZan3Nai As Double
    Public Property ZeiZan4Nai As Double
    Public Property ZeiZan5Nai As Double
    Public Property ZeiZan5Cho As Double
    Public Property TaishoF As Boolean      ' 一覧表示対象フラグ
    Public Property RecKbn As RecKbn        ' 行区分

    Public Sub New()
        LsryoTokig = New Object(11) {}
        ZeiTokig = New Object(11) {}
    End Sub
End Class

''' <summary>配賦情報 (Access版 cn_typ_mHAIF)</summary>
Public Class HaifInfo
    Public Property LineId As Integer
    Public Property Haifritu As Double
    Public Property HkmkId As Object
    Public Property HBcatId As Object
    Public Property Rsrvh1Id As Object
    Public Property HZokusei1 As Object
    Public Property HZokusei2 As Object
    Public Property HZokusei3 As Object
    Public Property HZokusei4 As Object
    Public Property HZokusei5 As Object
End Class
