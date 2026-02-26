Module Utils
    ' オブジェクトを安全にIntegerに変換する
    Public Function NzInt(val As Object) As Integer
        If val Is Nothing OrElse IsDBNull(val) OrElse String.IsNullOrEmpty(val.ToString()) Then
            Return 0
        End If

        Dim result As Integer
        If Integer.TryParse(val.ToString(), result) Then
            Return result
        Else
            Return 0
        End If
    End Function
End Module
