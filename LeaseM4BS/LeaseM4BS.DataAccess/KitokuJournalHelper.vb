''' <summary>
''' KITOKU顧客固有の仕訳出力ヘルパー
''' Access版 pc_仕訳出力.gDivKMK_CD 相当
''' </summary>
Public Module KitokuJournalHelper

    ''' <summary>
    ''' 科目コード文字列を KMK_CD / HKM_CD に分割する。
    ''' "4160-001" → kmkCd="4160", hkmCd="001"
    ''' "4160"     → kmkCd="4160", hkmCd=""
    ''' Access版 pc_仕訳出力.gDivKMK_CD 相当。
    ''' </summary>
    Public Sub DivKamokuCd(kamokuCd As String, ByRef kmkCd As String, ByRef hkmCd As String)
        If String.IsNullOrWhiteSpace(kamokuCd) Then
            kmkCd = ""
            hkmCd = ""
            Return
        End If
        Dim idx = kamokuCd.IndexOf("-"c)
        If idx >= 0 Then
            kmkCd = kamokuCd.Substring(0, idx)
            hkmCd = kamokuCd.Substring(idx + 1)
        Else
            kmkCd = kamokuCd
            hkmCd = ""
        End If
    End Sub

End Module
