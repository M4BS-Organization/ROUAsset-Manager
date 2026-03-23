# ドキュメント調査結果

> **作成日:** 2026-03-11
> **調査対象:** ログイン画面実装に必要な外部技術情報

---

## 1. BCrypt.Net-Next NuGet パッケージ

### バージョン・互換性
- **最新安定版: 4.1.0**（2026年2月16日リリース）
- **.NET Framework 4.7.2 に直接対応**（明示的ターゲットに含まれている）
- .NET Standard 2.0 / 2.1 にも対応

### VB.NET での使い方

**インストール:**
```
Install-Package BCrypt.Net-Next
```

**Imports:**
```vb
Imports BCryptNet = BCrypt.Net.BCrypt
```

**パスワードのハッシュ化:**
```vb
Dim passwordHash As String = BCryptNet.HashPassword(plainTextPassword)
```

**パスワードの検証:**
```vb
Dim isValid As Boolean = BCryptNet.Verify(plainTextPassword, storedHash)
```

**ワークファクターを指定する場合:**
```vb
Dim passwordHash As String = BCryptNet.HashPassword(plainTextPassword, workFactor:=12)
```

**注意事項:**
- ソルトは自動生成に任せること（手動生成は非推奨）
- デフォルトワークファクターは 11
- BCrypt のバリアント $2a$, $2b$, $2x$, $2y$ すべて対応

---

## 2. VB.NET WinForms ログイン画面の実装パターン

### パターン A: Application Events を使う方法（推奨・既存構造に最小変更）

`My Project\Application.vb` に Startup イベントハンドラーを追加:

```vb
Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) _
            Handles Me.Startup
            Dim loginForm As New FrmLogin()
            If loginForm.ShowDialog() <> DialogResult.OK Then
                e.Cancel = True
            End If
        End Sub
    End Class
End Namespace
```

FrmFlexMenu が MainForm のまま、ログイン成功後に表示される。

### パターン B: MainForm を FrmLogin に変更する方法（実装計画書の方式）

Application.Designer.vb で `Me.MainForm = FrmLogin` に変更し、ログイン成功後に FrmFlexMenu を Show する。

### ベストプラクティス
- ログインフォームは `ShowDialog()` で表示し、`DialogResult.OK` で認証成功を返す
- ユーザー情報はシングルトンクラスで保持

---

## 3. NpgsqlParameter による SQL インジェクション対策

### 既存プロジェクトのパターン（CrudHelper.GetDataTable）

```vb
Dim params As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@login_id", loginId)
}
Dim sql As String = "SELECT * FROM tm_USER WHERE login_id = @login_id"
Dim dt As DataTable = db.GetDataTable(sql, params)
```

- パラメータデータは SQL とは別に送信されるため、SQL インジェクションは構造的に不可能
- パスワード検証は DB からハッシュ取得 → アプリ側で `BCrypt.Verify()` 呼び出し

---

## まとめ・推奨事項

| 項目 | 推奨 |
|------|------|
| BCrypt パッケージ | BCrypt.Net-Next 4.1.0（.NET 4.7.2 直接対応） |
| ハッシュ関数 | `BCrypt.HashPassword()` デフォルトワークファクター 11 |
| ログイン画面実装 | 実装計画書の方式（MainForm を FrmLogin に変更）を採用 |
| SQL インジェクション対策 | 既存の `CrudHelper.GetDataTable(sql, params)` パターンをそのまま使用 |
| パスワード検証 | DB からハッシュ取得 → アプリ側で `BCrypt.Verify()` |
