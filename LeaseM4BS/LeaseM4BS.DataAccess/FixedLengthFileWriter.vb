Imports System.Data
Imports System.IO
Imports System.Text

Namespace LeaseM4BS.DataAccess

    ''' <summary>
    ''' DataTableから固定長テキストファイルを出力するライター
    ''' Access版 pc_仕訳出力 の gStrSizeAdjust + Format(Nz()) と同等ロジック
    ''' </summary>
    Public Class FixedLengthFileWriter

        Private Shared ReadOnly Sjis As Encoding = Encoding.GetEncoding("Shift_JIS")

        ''' <summary>
        ''' DataTableからフォーマット定義に従って固定長ファイルを出力する
        ''' </summary>
        Public Shared Sub WriteFile(filePath As String, dt As DataTable, fields As List(Of FixedLengthFieldDef))
            Using sw As New StreamWriter(filePath, False, Sjis)
                For Each row As DataRow In dt.Rows
                    sw.WriteLine(BuildRecord(row, fields))
                Next
            End Using
        End Sub

        ''' <summary>
        ''' 1行分のレコード文字列を生成する（テスト用にPublic）
        ''' </summary>
        Public Shared Function BuildRecord(row As DataRow, fields As List(Of FixedLengthFieldDef)) As String
            Dim sb As New StringBuilder()
            For Each field As FixedLengthFieldDef In fields
                Dim rawValue As Object = Nothing
                If row.Table.Columns.Contains(field.Name) Then
                    rawValue = row(field.Name)
                End If
                sb.Append(FormatField(rawValue, field))
            Next
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' フィールド値をフォーマットしてパディングする
        ''' </summary>
        Private Shared Function FormatField(value As Object, field As FixedLengthFieldDef) As String
            Dim text As String = FormatValue(value, field)
            Return PadRightByte(text, field.ByteWidth)
        End Function

        ''' <summary>
        ''' 値にフォーマットを適用する
        ''' Access版: Format(Nz(value, 0), formatString) または gStrSizeAdjust(value, byteWidth)
        ''' </summary>
        Private Shared Function FormatValue(value As Object, field As FixedLengthFieldDef) As String
            If field.FormatString IsNot Nothing Then
                ' 数値フォーマット: Null/DBNull → 0 として扱う
                Dim numVal As Double = 0
                If value IsNot Nothing AndAlso Not IsDBNull(value) Then
                    Try
                        numVal = Convert.ToDouble(value)
                    Catch
                        numVal = 0
                    End Try
                End If
                Return numVal.ToString(field.FormatString)
            Else
                ' 文字列: Null/DBNull → 空文字列
                If value Is Nothing OrElse IsDBNull(value) Then
                    Return ""
                End If
                Return value.ToString()
            End If
        End Function

        ''' <summary>
        ''' Shift-JISバイト単位で右パディング（半角スペース）または切り捨て
        ''' Access版 gStrSizeAdjust() と同等
        ''' </summary>
        Public Shared Function PadRightByte(text As String, byteLen As Integer) As String
            Dim val As String = If(text, "")
            Dim currentBytes As Byte() = Sjis.GetBytes(val)

            If currentBytes.Length >= byteLen Then
                ' 指定サイズ以上の場合は切り捨て
                Return Sjis.GetString(currentBytes, 0, byteLen)
            Else
                ' 足りない分を半角スペースで埋める
                Return val & New String(" "c, byteLen - currentBytes.Length)
            End If
        End Function

    End Class

End Namespace
