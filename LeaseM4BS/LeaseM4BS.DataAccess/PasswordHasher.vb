Imports System
Imports System.Security.Cryptography

''' <summary>PBKDF2-SHA256 によるパスワードハッシュユーティリティ（外部依存なし）</summary>
Public NotInheritable Class PasswordHasher

    Private Const SaltSize As Integer = 16
    Private Const HashSize As Integer = 32
    Private Const Iterations As Integer = 100000

    Private Sub New()
    End Sub

    ''' <summary>パスワードからハッシュ文字列を生成する</summary>
    ''' <returns>Base64エンコードされた "iterations:salt:hash" 形式の文字列</returns>
    Public Shared Function HashPassword(password As String) As String
        Dim salt(SaltSize - 1) As Byte
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using

        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256)
            Dim hash As Byte() = pbkdf2.GetBytes(HashSize)
            Return Iterations.ToString() & ":" &
                   Convert.ToBase64String(salt) & ":" &
                   Convert.ToBase64String(hash)
        End Using
    End Function

    ''' <summary>パスワードがハッシュと一致するか検証する</summary>
    Public Shared Function Verify(password As String, storedHash As String) As Boolean
        Try
            Dim parts As String() = storedHash.Split(":"c)
            If parts.Length <> 3 Then Return False

            Dim iterations As Integer = Integer.Parse(parts(0))
            Dim salt As Byte() = Convert.FromBase64String(parts(1))
            Dim expectedHash As Byte() = Convert.FromBase64String(parts(2))

            Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256)
                Dim actualHash As Byte() = pbkdf2.GetBytes(expectedHash.Length)
                Return SlowEquals(expectedHash, actualHash)
            End Using
        Catch ex As FormatException
            Return False
        End Try
    End Function

    ''' <summary>タイミング攻撃を防ぐ定数時間比較</summary>
    Private Shared Function SlowEquals(a As Byte(), b As Byte()) As Boolean
        Dim diff As UInteger = CUInt(a.Length Xor b.Length)
        Dim len As Integer = Math.Min(a.Length, b.Length)
        For i As Integer = 0 To len - 1
            diff = diff Or CUInt(a(i) Xor b(i))
        Next
        Return diff = 0
    End Function

End Class
