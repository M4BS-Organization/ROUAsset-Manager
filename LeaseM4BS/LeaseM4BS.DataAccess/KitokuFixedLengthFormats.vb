Imports System.Collections.Generic

''' <summary>
''' 固定長フィールド定義
''' </summary>
Public Class FixedLengthFieldDef
        ''' <summary>DB列名（DataTableのカラム名と一致）</summary>
        Public Property Name As String
        ''' <summary>Shift-JISバイト幅</summary>
        Public Property ByteWidth As Integer
        ''' <summary>数値フォーマット文字列（Nothingの場合は文字列としてパディング）</summary>
        Public Property FormatString As String

        Public Sub New(name As String, byteWidth As Integer, Optional formatString As String = Nothing)
            Me.Name = name
            Me.ByteWidth = byteWidth
            Me.FormatString = formatString
        End Sub
    End Class

    ''' <summary>
    ''' KITOKU顧客向け固定長出力フォーマット定義
    ''' Access版 pc_仕訳出力.txt から正確に移植
    ''' </summary>
    Public Module KitokuFixedLengthFormats

        Private Const FMT_KIN As String = "000000000000000000.000"
        Private Const FMT_RATE As String = "00000.000000000000"
        Private Const FMT_GYONO As String = "00000"
        Private Const FMT_ZEI_SPECIAL As String = "0.0"

        ''' <summary>
        ''' CMSW2WRK (伝票ワーク) フィールド定義 — 52フィールド
        ''' Access版: gCMSW2WRK出力() Lines 146-197
        ''' </summary>
        Public Function GetCMSW2WRKFields() As List(Of FixedLengthFieldDef)
            Return New List(Of FixedLengthFieldDef) From {
                New FixedLengthFieldDef("SW2_KAI_CODE", 5),
                New FixedLengthFieldDef("SW2_DATE", 10),
                New FixedLengthFieldDef("SW2_DEN_NO", 8),
                New FixedLengthFieldDef("SW2_GYO_NO", 5, FMT_GYONO),
                New FixedLengthFieldDef("SW2_DC_KBN", 1),
                New FixedLengthFieldDef("SW2_KMK_CODE", 10),
                New FixedLengthFieldDef("SW2_HKM_CODE", 10),
                New FixedLengthFieldDef("SW2_BMN_CODE", 10),
                New FixedLengthFieldDef("SW2_CODE1", 10),
                New FixedLengthFieldDef("SW2_CODE2", 10),
                New FixedLengthFieldDef("SW2_CODE3", 10),
                New FixedLengthFieldDef("SW2_CODE4", 10),
                New FixedLengthFieldDef("SW2_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("SW2_ZEI_CODE", 4),
                New FixedLengthFieldDef("SW2_ZEI_KBN", 1),
                New FixedLengthFieldDef("SW2_ZEI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("SW2_CUR_CODE", 3),
                New FixedLengthFieldDef("SW2_RATE_TYPE", 2),
                New FixedLengthFieldDef("SW2_RATE", 18, FMT_RATE),
                New FixedLengthFieldDef("SW2_CUR_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("SW2_TEKIYO1", 40),
                New FixedLengthFieldDef("SW2_TEKIYO2", 40),
                New FixedLengthFieldDef("SW2_GRP_CODE", 2),
                New FixedLengthFieldDef("SW2_SYS_KBN", 2),
                New FixedLengthFieldDef("SW2_SYS_DEN_NO", 8),
                New FixedLengthFieldDef("SW2_SYS_SYS_KBN", 2),
                New FixedLengthFieldDef("SW2_SYS_GRP_CODE", 2),
                New FixedLengthFieldDef("SW2_AIT_KMK_CODE", 10),
                New FixedLengthFieldDef("SW2_AIT_HKM_CODE", 10),
                New FixedLengthFieldDef("SW2_USR_ID1", 10),
                New FixedLengthFieldDef("SW2_STS_NO1", 3),
                New FixedLengthFieldDef("SW2_SYS_DATE1", 10),
                New FixedLengthFieldDef("SW2_USR_ID2", 10),
                New FixedLengthFieldDef("SW2_STS_NO2", 3),
                New FixedLengthFieldDef("SW2_SYS_DATE2", 10),
                New FixedLengthFieldDef("SW2_SHONIN_KBN", 1),
                New FixedLengthFieldDef("SW2_DEN_KBN", 1),
                New FixedLengthFieldDef("SW2_HAIFU_KBN", 1),
                New FixedLengthFieldDef("SW2_REC_LEVEL", 1),
                New FixedLengthFieldDef("SW2_TORI_KBN", 1),
                New FixedLengthFieldDef("SW2_TORI_CODE", 20),
                New FixedLengthFieldDef("SW2_YOBI_CHAR1", 10),
                New FixedLengthFieldDef("SW2_YOBI_CHAR2", 10),
                New FixedLengthFieldDef("SW2_YOBI_CHAR3", 10),
                New FixedLengthFieldDef("SW2_YOBI_CHAR4", 10),
                New FixedLengthFieldDef("SW2_YOBI_CHAR5", 20),
                New FixedLengthFieldDef("SW2_YOBI_CHAR6", 20),
                New FixedLengthFieldDef("SW2_YOBI_CHAR7", 20),
                New FixedLengthFieldDef("SW2_YOBI_CHAR8", 20),
                New FixedLengthFieldDef("SW2_YOBI_NUM1", 22, FMT_KIN),
                New FixedLengthFieldDef("SW2_YOBI_NUM2", 22, FMT_KIN),
                New FixedLengthFieldDef("SW2_YOBI_NUM3", 22, FMT_KIN)
            }
        End Function

        ''' <summary>
        ''' APGDHWRK (金額概要ワーク) フィールド定義 — 72フィールド
        ''' Access版: gAPGDHWRK出力() Lines 266-346
        ''' </summary>
        Public Function GetAPGDHWRKFields() As List(Of FixedLengthFieldDef)
            Return New List(Of FixedLengthFieldDef) From {
                New FixedLengthFieldDef("GDH_KAI_CODE", 5),
                New FixedLengthFieldDef("GDH_DEN_KBN", 2),
                New FixedLengthFieldDef("GDH_GRP_CODE", 2),
                New FixedLengthFieldDef("GDH_DEN_NO", 8),
                New FixedLengthFieldDef("GDH_DEN_DATE", 10),
                New FixedLengthFieldDef("GDH_SH_SAKI_KBN", 1),
                New FixedLengthFieldDef("GDH_SIR_CODE", 20),
                New FixedLengthFieldDef("GDH_KEI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_KABU_KBN", 1),
                New FixedLengthFieldDef("GDH_SAI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_SAI_NUKI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_SAI_ZEI_KIN", 22, FMT_ZEI_SPECIAL),
                New FixedLengthFieldDef("GDH_TEKIYO", 40),
                New FixedLengthFieldDef("GDH_SEI_NO", 15),
                New FixedLengthFieldDef("GDH_SEI_DATE", 10),
                New FixedLengthFieldDef("GDH_CYCLE_KBN", 1),
                New FixedLengthFieldDef("GDH_DEN_SYURUI", 1),
                New FixedLengthFieldDef("GDH_DC_HANTEN", 1),
                New FixedLengthFieldDef("GDH_SHZ_DEN_KBN", 1),
                New FixedLengthFieldDef("GDH_SOSAI_KBN", 1),
                New FixedLengthFieldDef("GDH_UTIBARAI_KBN", 1),
                New FixedLengthFieldDef("GDH_SAI_KMK_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_HKM_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_BMN_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_FC1_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_FC2_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_FC3_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_FC4_CODE", 10),
                New FixedLengthFieldDef("GDH_SAI_ZEI_CODE", 4),
                New FixedLengthFieldDef("GDH_SAI_ZEI_KBN", 1),
                New FixedLengthFieldDef("GDH_SAI_TEKIYO", 40),
                New FixedLengthFieldDef("GDH_CUR_CODE", 3),
                New FixedLengthFieldDef("GDH_RATE_TYPE", 2),
                New FixedLengthFieldDef("GDH_RATE", 18, FMT_RATE),
                New FixedLengthFieldDef("GDH_CUR_KIN", 22),
                New FixedLengthFieldDef("GDH_RPL_USR_ID", 10),
                New FixedLengthFieldDef("GDH_RPL_STS_NO", 3),
                New FixedLengthFieldDef("GDH_RPL_DATE", 10),
                New FixedLengthFieldDef("GDH_SPOT_KBN", 1),
                New FixedLengthFieldDef("GDH_KIJITU_KBN", 1),
                New FixedLengthFieldDef("GDH_FURI_KMK_CODE", 10),
                New FixedLengthFieldDef("GDH_FURI_HKM_CODE", 10),
                New FixedLengthFieldDef("GDH_FURI_FC1_CODE", 10),
                New FixedLengthFieldDef("GDH_FURI_FC2_CODE", 10),
                New FixedLengthFieldDef("GDH_FURI_FC3_CODE", 10),
                New FixedLengthFieldDef("GDH_FURI_FC4_CODE", 10),
                New FixedLengthFieldDef("GDH_REC_LEVEL", 1),
                New FixedLengthFieldDef("GDH_KARI_KMK_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_HKM_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_BMN_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_FC1_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_FC2_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_FC3_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_FC4_CODE", 10),
                New FixedLengthFieldDef("GDH_KARI_ZEI_CODE", 4),
                New FixedLengthFieldDef("GDH_KARI_ZEI_KBN", 1),
                New FixedLengthFieldDef("GDH_KARI_TEKIYO", 40),
                New FixedLengthFieldDef("GDH_KARI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_KARI_NUKI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_KARI_ZEI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_KARI_CUR_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_FURI_KMK_TORI_KBN", 1),
                New FixedLengthFieldDef("GDH_FURI_KMK_TORI_CODE", 20),
                New FixedLengthFieldDef("GDH_KARI_KMK_TORI_KBN", 1),
                New FixedLengthFieldDef("GDH_KARI_KMK_TORI_CODE", 20),
                New FixedLengthFieldDef("GDH_SAI_KMK_TORI_KBN", 1),
                New FixedLengthFieldDef("GDH_SAI_KMK_TORI_CODE", 20),
                New FixedLengthFieldDef("GDH_YOBI_CHAR1", 10),
                New FixedLengthFieldDef("GDH_YOBI_CHAR2", 10),
                New FixedLengthFieldDef("GDH_YOBI_CHAR3", 10),
                New FixedLengthFieldDef("GDH_YOBI_CHAR4", 10),
                New FixedLengthFieldDef("GDH_YOBI_CHAR5", 20),
                New FixedLengthFieldDef("GDH_YOBI_CHAR6", 20),
                New FixedLengthFieldDef("GDH_YOBI_CHAR7", 20),
                New FixedLengthFieldDef("GDH_YOBI_CHAR8", 20),
                New FixedLengthFieldDef("GDH_YOBI_NUM1", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_YOBI_NUM2", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_YOBI_NUM3", 22, FMT_KIN),
                New FixedLengthFieldDef("GDH_SOSAI_RATE_TYPE", 2),
                New FixedLengthFieldDef("GDH_SOSAI_RATE", 18, FMT_RATE),
                New FixedLengthFieldDef("GDH_SOSAI_KIN", 22)
            }
        End Function

        ''' <summary>
        ''' APGDDWRK (金額詳細ワーク) フィールド定義 — 31フィールド
        ''' Access版: gAPGDDWRK出力() Lines 417-451
        ''' </summary>
        Public Function GetAPGDDWRKFields() As List(Of FixedLengthFieldDef)
            Return New List(Of FixedLengthFieldDef) From {
                New FixedLengthFieldDef("GDD_KAI_CODE", 5),
                New FixedLengthFieldDef("GDD_DEN_KBN", 2),
                New FixedLengthFieldDef("GDD_GRP_CODE", 2),
                New FixedLengthFieldDef("GDD_DEN_NO", 8),
                New FixedLengthFieldDef("GDD_DEN_DATE", 10),
                New FixedLengthFieldDef("GDD_GYO_NO", 5, FMT_GYONO),
                New FixedLengthFieldDef("GDD_KMK_CODE", 10),
                New FixedLengthFieldDef("GDD_HKM_CODE", 10),
                New FixedLengthFieldDef("GDD_BMN_CODE", 10),
                New FixedLengthFieldDef("GDD_FC1_CODE", 10),
                New FixedLengthFieldDef("GDD_FC2_CODE", 10),
                New FixedLengthFieldDef("GDD_FC3_CODE", 10),
                New FixedLengthFieldDef("GDD_FC4_CODE", 10),
                New FixedLengthFieldDef("GDD_NUKI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDD_ZEI_CODE", 4),
                New FixedLengthFieldDef("GDD_ZEI_KBN", 1),
                New FixedLengthFieldDef("GDD_ZEI_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDD_CUR_KIN", 22),
                New FixedLengthFieldDef("GDD_TEKIYO", 40),
                New FixedLengthFieldDef("GDD_TORI_KBN", 1),
                New FixedLengthFieldDef("GDD_TORI_CODE", 20),
                New FixedLengthFieldDef("GDD_YOBI_CHAR1", 10),
                New FixedLengthFieldDef("GDD_YOBI_CHAR2", 10),
                New FixedLengthFieldDef("GDD_YOBI_CHAR3", 10),
                New FixedLengthFieldDef("GDD_YOBI_CHAR4", 10),
                New FixedLengthFieldDef("GDD_YOBI_CHAR5", 20),
                New FixedLengthFieldDef("GDD_YOBI_CHAR6", 20),
                New FixedLengthFieldDef("GDD_YOBI_CHAR7", 20),
                New FixedLengthFieldDef("GDD_YOBI_CHAR8", 20),
                New FixedLengthFieldDef("GDD_YOBI_NUM1", 22, FMT_KIN),
                New FixedLengthFieldDef("GDD_YOBI_NUM2", 22, FMT_KIN),
                New FixedLengthFieldDef("GDD_YOBI_NUM3", 22, FMT_KIN),
                New FixedLengthFieldDef("GDD_RATE_TYPE", 2),
                New FixedLengthFieldDef("GDD_RATE", 18, FMT_RATE)
            }
        End Function

        ''' <summary>
        ''' APGDSWRK (支払ワーク) フィールド定義 — 62フィールド
        ''' Access版: gAPGDSWRK出力() Lines 519-581
        ''' </summary>
        Public Function GetAPGDSWRKFields() As List(Of FixedLengthFieldDef)
            Return New List(Of FixedLengthFieldDef) From {
                New FixedLengthFieldDef("GDS_KAI_CODE", 5),
                New FixedLengthFieldDef("GDS_DEN_KBN", 2),
                New FixedLengthFieldDef("GDS_GRP_CODE", 2),
                New FixedLengthFieldDef("GDS_DEN_NO", 8),
                New FixedLengthFieldDef("GDS_DEN_DATE", 10),
                New FixedLengthFieldDef("GDS_GYO_NO", 5, FMT_GYONO),
                New FixedLengthFieldDef("GDS_SH_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDS_SHY_DATE", 10),
                New FixedLengthFieldDef("GDS_SH_HOHO", 2),
                New FixedLengthFieldDef("GDS_SH_TEKIYO", 40),
                New FixedLengthFieldDef("GDS_KGG_CODE", 10),
                New FixedLengthFieldDef("GDS_CUR_KIN", 22),
                New FixedLengthFieldDef("GDS_KOZ_KNR_CODE", 4),
                New FixedLengthFieldDef("GDS_FRJ_CODE", 4),
                New FixedLengthFieldDef("GDS_FSAKI_BNK_CODE", 4),
                New FixedLengthFieldDef("GDS_FSAKI_BRH_CODE", 3),
                New FixedLengthFieldDef("GDS_FSAKI_KOZ_SYUBETSU", 2),
                New FixedLengthFieldDef("GDS_FSAKI_KOZ_NO", 7),
                New FixedLengthFieldDef("GDS_FSAKI_KOZ_NAME", 40),
                New FixedLengthFieldDef("GDS_FSAKI_KOZ_NAME_FB", 30),
                New FixedLengthFieldDef("GDS_TESU_KBN", 1),
                New FixedLengthFieldDef("GDS_KYUJITSU_KBN", 1),
                New FixedLengthFieldDef("GDS_DSI_KBN", 1),
                New FixedLengthFieldDef("GDS_TEGA_KOZ_CODE", 4),
                New FixedLengthFieldDef("GDS_TEGA_WATASI_CODE", 10),
                New FixedLengthFieldDef("GDS_TEGA_NO", 10),
                New FixedLengthFieldDef("GDS_TEGA_SYURUI", 1),
                New FixedLengthFieldDef("GDS_FDASI_KBN", 1),
                New FixedLengthFieldDef("GDS_FDASI_DATE", 10),
                New FixedLengthFieldDef("GDS_TEGA_MANKIBI", 10),
                New FixedLengthFieldDef("GDS_TEGA_HAKKO_KBN", 1),
                New FixedLengthFieldDef("GDS_KIJITU_YOTEI_DATE", 10),
                New FixedLengthFieldDef("GDS_TEGA_BMN_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_KBN", 1),
                New FixedLengthFieldDef("GDS_AZU_KMK_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_HKM_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_BMN_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_FC1_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_FC2_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_FC3_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_FC4_CODE", 10),
                New FixedLengthFieldDef("GDS_AZU_ZEI_CODE", 4),
                New FixedLengthFieldDef("GDS_AZU_ZEI_KBN", 1),
                New FixedLengthFieldDef("GDS_AZU_TEKIYO", 40),
                New FixedLengthFieldDef("GDS_AZU_KIN", 22),
                New FixedLengthFieldDef("GDS_AZU_NUKI_KIN", 22),
                New FixedLengthFieldDef("GDS_AZU_ZEI_KIN", 22),
                New FixedLengthFieldDef("GDS_AZU_CUR_KIN", 22),
                New FixedLengthFieldDef("GDS_TYO_SH_KIN", 22, FMT_KIN),
                New FixedLengthFieldDef("GDS_AZU_KMK_TORI_KBN", 1),
                New FixedLengthFieldDef("GDS_AZU_KMK_TORI_CODE", 20),
                New FixedLengthFieldDef("GDS_YOBI_CHAR1", 10),
                New FixedLengthFieldDef("GDS_YOBI_CHAR2", 10),
                New FixedLengthFieldDef("GDS_YOBI_CHAR3", 10),
                New FixedLengthFieldDef("GDS_YOBI_CHAR4", 10),
                New FixedLengthFieldDef("GDS_YOBI_CHAR5", 20),
                New FixedLengthFieldDef("GDS_YOBI_CHAR6", 20),
                New FixedLengthFieldDef("GDS_YOBI_CHAR7", 20),
                New FixedLengthFieldDef("GDS_YOBI_CHAR8", 20),
                New FixedLengthFieldDef("GDS_YOBI_NUM1", 22, FMT_KIN),
                New FixedLengthFieldDef("GDS_YOBI_NUM2", 22, FMT_KIN),
                New FixedLengthFieldDef("GDS_YOBI_NUM3", 22, FMT_KIN),
                New FixedLengthFieldDef("GDS_TEGA_NO_EDA", 5)
            }
        End Function

End Module
