' =========================================================
' PasswordValidator - パスワード文字種チェック
' Access版 f_SEC_USER_INP.mPASS_CHK のVB.NET移植
' =========================================================

Public Class PasswordValidator

    ''' <summary>
    ''' パスワードポリシー設定
    ''' </summary>
    Public Class PasswordPolicy
        Public Property PwdMin As Integer = 0           ' 最小文字数（0=制限なし）
        Public Property PwdMojiChk As Boolean = False   ' 文字種制限の有効/無効
        Public Property PwdAlphChk As Boolean = False   ' 英字(a-z/A-Z)必須
        Public Property PwdNumChk As Boolean = False    ' 数字(0-9)必須
        Public Property PwdSymbolChk As Boolean = False ' 記号必須
    End Class

    ''' <summary>
    ''' 検証結果
    ''' </summary>
    Public Class ValidationResult
        Public Property IsValid As Boolean = True
        Public Property ErrorMessage As String = ""
    End Class

    ' パスワード最大文字数 (Access版と同じ上限)
    Private Const MAX_PASSWORD_LENGTH As Integer = 20

    ''' <summary>
    ''' LoginSessionの現在のポリシーからPasswordPolicyを生成する
    ''' </summary>
    Public Shared Function GetCurrentPolicy() As PasswordPolicy
        Dim policy As New PasswordPolicy()
        policy.PwdMin = LoginSession.PasswordMinLength
        policy.PwdMojiChk = LoginSession.PwdMojiChk
        policy.PwdAlphChk = LoginSession.PwdAlphChk
        policy.PwdNumChk = LoginSession.PwdNumChk
        policy.PwdSymbolChk = LoginSession.PwdSymbolChk
        Return policy
    End Function

    ''' <summary>
    ''' パスワードを検証する (Access版 mPASS_CHK に完全準拠)
    ''' </summary>
    Public Shared Function Validate(password As String, policy As PasswordPolicy) As ValidationResult
        Dim result As New ValidationResult()

        ' NULLまたは空チェック
        If String.IsNullOrEmpty(password) Then
            result.IsValid = False
            result.ErrorMessage = "ﾊﾟｽﾜｰﾄﾞを入力してください。"
            Return result
        End If

        ' 最大文字数チェック (Access版: 20文字以内)
        If password.Length > MAX_PASSWORD_LENGTH Then
            result.IsValid = False
            result.ErrorMessage = "ﾊﾟｽﾜｰﾄﾞは20文字以内で入力して下さい。"
            Return result
        End If

        ' 最小文字数チェック (Access版: txt_PWD_MIN)
        If policy.PwdMin > 0 AndAlso password.Length < policy.PwdMin Then
            result.IsValid = False
            result.ErrorMessage = $"ﾊﾟｽﾜｰﾄﾞは{policy.PwdMin}文字以上、20文字以内で入力して下さい。"
            Return result
        End If

        ' 文字種チェック (Access版: chk_PWD_MOJI_CHK が ON の場合のみ)
        If policy.PwdMojiChk Then
            ' 必須文字種の説明メッセージを構築
            Dim requiredTypes As New List(Of String)()
            If policy.PwdAlphChk Then requiredTypes.Add("英字")
            If policy.PwdNumChk Then requiredTypes.Add("数字")
            If policy.PwdSymbolChk Then requiredTypes.Add("記号")

            Dim baseMsg As String = "ﾊﾟｽﾜｰﾄﾞが、文字種の規則に違反しています。"
            If requiredTypes.Count > 0 Then
                baseMsg &= vbCrLf & "ﾊﾟｽﾜｰﾄﾞに " & String.Join(" ", requiredTypes) & " を含めて下さい。"
            End If

            ' 英字チェック (Access版: a-z の範囲で検索, StrConv で小文字変換)
            If policy.PwdAlphChk Then
                Dim found As Boolean = False
                For Each c As Char In password
                    If Char.IsLetter(c) Then
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    result.IsValid = False
                    result.ErrorMessage = baseMsg
                    Return result
                End If
            End If

            ' 数字チェック (Access版: IsNumeric で判定)
            If policy.PwdNumChk Then
                Dim found As Boolean = False
                For Each c As Char In password
                    If Char.IsDigit(c) Then
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    result.IsValid = False
                    result.ErrorMessage = baseMsg
                    Return result
                End If
            End If

            ' 記号チェック (Access版: 英字でも数字でもない印字可能文字)
            If policy.PwdSymbolChk Then
                Dim found As Boolean = False
                For Each c As Char In password
                    If Not Char.IsLetterOrDigit(c) AndAlso Not Char.IsWhiteSpace(c) Then
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    result.IsValid = False
                    result.ErrorMessage = baseMsg
                    Return result
                End If
            End If
        End If

        ' すべてのチェックをパス
        result.IsValid = True
        result.ErrorMessage = ""
        Return result
    End Function

    ''' <summary>
    ''' 現在のLoginSessionポリシーで検証するショートカット
    ''' </summary>
    Public Shared Function ValidateWithCurrentPolicy(password As String) As ValidationResult
        Return Validate(password, GetCurrentPolicy())
    End Function

End Class
