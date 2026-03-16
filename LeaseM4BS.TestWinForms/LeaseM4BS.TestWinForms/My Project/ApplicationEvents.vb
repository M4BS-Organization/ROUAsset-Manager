Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    Partial Friend Class MyApplication

        ''' <summary>
        ''' アプリ起動時にログインフォームを表示する（Access版 p_StartUp_gMain 相当）。
        ''' 1. バージョン情報をログ出力
        ''' 2. ログインフォーム表示
        ''' 3. 認証成功(DialogResult.OK)ならセッション情報をセットしメインメニューへ進む
        ''' 4. それ以外はアプリを終了する
        ''' </summary>
        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            ' --- バージョン情報ログ出力（Access版 olStartUp.gSetAppTITLE 相当） ---
            Dim appVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
            Console.WriteLine($"[LeaseM4BS] アプリケーション起動 Version={appVersion} ({DateTime.Now:yyyy/MM/dd HH:mm:ss})")

            ' --- ログインフォーム表示 ---
            Dim loginForm As New Form_f_LOGIN_JET()
            Dim result = loginForm.ShowDialog()

            If result <> System.Windows.Forms.DialogResult.OK Then
                ' ログインキャンセル → アプリ終了
                e.Cancel = True
                Return
            End If

            ' --- ログイン成功後のセッション情報セット（Access版 olSLOG.OutputSLOG 相当） ---
            LoginSession.LoginDateTime = DateTime.Now
            LoginSession.IsSessionActive = True

            ' --- 操作ログ記録: ログイン成功 ---
            LoginSession.WriteAuditLog(LoginSession.OP_LOGIN, "アプリケーション起動")
        End Sub

        ''' <summary>
        ''' アプリ終了時にログアウトログを記録する（Access版 gLOGOFF 相当）
        ''' </summary>
        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown
            If LoginSession.IsSessionActive Then
                ' --- 操作ログ記録: ログアウト ---
                LoginSession.WriteAuditLog(LoginSession.OP_LOGOUT, "アプリケーション終了")

                ' --- セッションクリア ---
                LoginSession.IsSessionActive = False
            End If
        End Sub

    End Class

End Namespace
