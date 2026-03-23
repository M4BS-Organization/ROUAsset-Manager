# テスト計画書: login-screen

> **作成日:** 2026-03-11
> **対象機能:** ログイン画面実装（ISS-006）
> **対象プロジェクト:** LeaseM4BS（VB.NET .NET Framework 4.7.2 WinForms）

---

## 1. テスト戦略

- **テストフレームワーク:** MSTest 2.2.8（既存プロジェクト LeaseM4BS.Tests に設定済み）
- **テストレベル:** 単体テスト（AuthorizationService / UserInfo / BCrypt）＋ 結合テスト（UI遷移 / 権限制御）
- **カバレッジ目標:** 認証ロジック（AuthorizationService.Authenticate / HasAccess）の正常系・異常系・境界値を網羅
- **モック戦略:**
  - DB依存テスト（Authenticate）はテスト用 PostgreSQL DB に接続して実施（CrudHelper をそのまま使用）
  - テスト実行前に `sql/seed_test_users.sql` でテストユーザーを投入済みであることを前提とする
  - BCrypt のハッシュ化・検証はモックなしで実ライブラリを使用（副作用がなく純粋なロジックのため）

---

## 2. テスト環境

### 必要なセットアップ手順

1. PostgreSQL が起動しており、`lease_m4bs` データベースが存在すること
2. `sql/alter_tm_user.sql` を実行して tm_USER テーブルにカラムが追加済みであること
3. `sql/seed_test_users.sql` を実行してテストユーザー5名が投入済みであること
4. `LeaseM4BS.Tests/App.config` に接続文字列 `LeaseM4BS` が設定されていること（未設定の場合は環境変数 `LEASE_M4BS_CONNECTION_STRING` で代替）
5. `LeaseM4BS.DataAccess` プロジェクトに `BCrypt.Net-Next` NuGet パッケージが追加済みであること

### テストデータの準備方法

テストデータは `sql/seed_test_users.sql` スクリプトで投入する（§5「テストデータ設計」参照）。
各テストメソッドはテストユーザーが投入済みであることを前提とし、テストメソッド内でデータを変更する場合はテスト終了後に元の状態に戻す（`failed_login_count` のリセット等）。

### 外部依存のモック方法

- **CrudHelper / DB接続:** テスト用 DB に直接接続する（モックなし）。テスト用 DB を本番 DB と分離すること
- **BCrypt.Net-Next:** 実ライブラリを使用する（ハッシュ化・検証は副作用なし）
- **AuthorizationService.Current（シングルトン）:** 各テストの `TestInitialize` でシングルトン状態をリセットするため、`AuthorizationService.Current.Logout()` を呼び出す

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | AuthorizationService.Authenticate — 正常系 | 高 | US-002, FR-003, NFR-001 |
| T-002 | AuthorizationService.Authenticate — ユーザー不在 | 高 | US-003, FR-003, NFR-001 |
| T-003 | AuthorizationService.Authenticate — パスワード不一致 | 高 | US-003, FR-003, NFR-001 |
| T-004 | AuthorizationService.Authenticate — アカウント無効 | 高 | US-003, FR-003 |
| T-005 | AuthorizationService.Authenticate — 空文字入力 | 中 | US-003, NFR-001 |
| T-006 | AuthorizationService.Authenticate — CurrentUser のセット | 高 | US-002, FR-003 |
| T-007 | AuthorizationService.HasAccess — admin 全メニュー許可 | 高 | US-005, FR-008 |
| T-008 | AuthorizationService.HasAccess — accounting の制限 | 高 | US-005, FR-008 |
| T-009 | AuthorizationService.HasAccess — general_affairs の制限 | 高 | US-005, FR-008 |
| T-010 | AuthorizationService.HasAccess — viewer の制限 | 高 | US-005, FR-008 |
| T-011 | AuthorizationService.HasAccess — 未ログイン状態 | 中 | FR-003 |
| T-012 | AuthorizationService.Logout — セッションクリア | 中 | US-006, FR-003 |
| T-013 | UserInfo クラス — プロパティの読み書き | 低 | FR-004 |
| T-014 | BCrypt ハッシュ化と検証 — 正常照合 | 高 | FR-003, NFR-001 |
| T-015 | BCrypt ハッシュ化と検証 — 不一致検証 | 高 | FR-003, NFR-001 |
| T-016 | BCrypt — 同一パスワードでもハッシュが異なる（ソルト） | 中 | NFR-001 |
| T-017 | 結合: 起動フォームが FrmLogin であること | 高 | US-001, FR-001 |
| T-018 | 結合: 正常ログイン → FrmFlexMenu 表示 | 高 | US-002 |
| T-019 | 結合: パスワード不一致 → エラーメッセージ表示 | 高 | US-003 |
| T-020 | 結合: 存在しないユーザー → エラーメッセージ表示 | 高 | US-003 |
| T-021 | 結合: 空欄ログイン → バリデーションエラー | 高 | US-003 |
| T-022 | 結合: 無効アカウント → アカウント無効エラー | 高 | US-003 |
| T-023 | 結合: admin 権限 → 全7メニュー有効 | 高 | US-005, FR-008 |
| T-024 | 結合: accounting 権限 → btnMaster 無効 | 高 | US-005, FR-008 |
| T-025 | 結合: general_affairs 権限 → 月次系4ボタン無効 | 高 | US-005, FR-008 |
| T-026 | 結合: viewer 権限 → btnMaster 以外有効 | 高 | US-005, FR-008 |
| T-027 | 結合: FrmFlexMenu クローズ → アプリ終了 | 高 | US-006, FR-001 |
| T-028 | 結合: 終了ボタン → Application.Exit() | 高 | US-004 |
| T-029 | 結合: Esc キー → Application.Exit() | 高 | US-004, NFR-003 |
| T-030 | 結合: Enter キー → ログイン処理実行 | 中 | US-002, NFR-003 |

---

## 4. テストケース

### TC-001: Authenticate — 正常ログイン成功

- **対象:** `AuthorizationService.Authenticate(loginId As String, password As String) As AuthResult`
- **関連要件:** US-002, FR-003
- **種別:** 正常系
- **前提条件:** テストユーザー `admin`（role='admin', is_active=TRUE, password="password123"）が tm_USER に存在する
- **入力:** `loginId = "admin"`, `password = "password123"`
- **期待結果:**
  - 戻り値が `AuthResult.Success`
  - `AuthorizationService.Current.CurrentUser` が Nothing でない
  - `CurrentUser.LoginId = "admin"`
  - `CurrentUser.Role = "admin"`

---

### TC-002: Authenticate — 存在しないユーザー

- **対象:** `AuthorizationService.Authenticate`
- **関連要件:** US-003, FR-003, NFR-001
- **種別:** 異常系
- **前提条件:** `nonexistent_user` という login_id のユーザーが tm_USER に存在しない
- **入力:** `loginId = "nonexistent_user"`, `password = "anyPassword"`
- **期待結果:**
  - 戻り値が `AuthResult.UserNotFound`
  - `AuthorizationService.Current.CurrentUser` が Nothing のまま

---

### TC-003: Authenticate — パスワード不一致

- **対象:** `AuthorizationService.Authenticate`
- **関連要件:** US-003, FR-003, NFR-001
- **種別:** 異常系
- **前提条件:** テストユーザー `keiri01` が tm_USER に存在する（password="password123"）
- **入力:** `loginId = "keiri01"`, `password = "wrongPassword"`
- **期待結果:**
  - 戻り値が `AuthResult.InvalidPassword`
  - `AuthorizationService.Current.CurrentUser` が Nothing のまま

---

### TC-004: Authenticate — アカウント無効化（is_active=FALSE）

- **対象:** `AuthorizationService.Authenticate`
- **関連要件:** US-003, FR-003
- **種別:** 異常系
- **前提条件:** テストユーザー `disabled01`（is_active=FALSE）が tm_USER に存在する
- **入力:** `loginId = "disabled01"`, `password = "password123"`
- **期待結果:**
  - 戻り値が `AuthResult.AccountDisabled`
  - `AuthorizationService.Current.CurrentUser` が Nothing のまま

---

### TC-005: Authenticate — ログインID が空文字

- **対象:** `AuthorizationService.Authenticate`
- **関連要件:** US-003
- **種別:** 境界値
- **前提条件:** なし
- **入力:** `loginId = ""`, `password = "password123"`
- **期待結果:**
  - 戻り値が `AuthResult.UserNotFound`（空文字では tm_USER に一致しないため）
  - `CurrentUser` が Nothing のまま

---

### TC-006: Authenticate — パスワードが空文字

- **対象:** `AuthorizationService.Authenticate`
- **関連要件:** US-003
- **種別:** 境界値
- **前提条件:** テストユーザー `admin` が存在する
- **入力:** `loginId = "admin"`, `password = ""`
- **期待結果:**
  - 戻り値が `AuthResult.InvalidPassword`
  - `CurrentUser` が Nothing のまま

---

### TC-007: Authenticate — 成功後の CurrentUser 内容確認

- **対象:** `AuthorizationService.Authenticate`, `AuthorizationService.CurrentUser`
- **関連要件:** US-002, FR-003, FR-004
- **種別:** 正常系
- **前提条件:** テストユーザー `soumu01`（role='general_affairs', is_active=TRUE）が存在する
- **入力:** `loginId = "soumu01"`, `password = "password123"`
- **期待結果:**
  - 戻り値が `AuthResult.Success`
  - `CurrentUser.LoginId = "soumu01"`
  - `CurrentUser.Role = "general_affairs"`
  - `CurrentUser.IsActive = True`
  - `CurrentUser.UserId` が 0 より大きい整数値

---

### TC-008: HasAccess — admin は全メニューにアクセス可能

- **対象:** `AuthorizationService.HasAccess(menuId As String) As Boolean`
- **関連要件:** US-005, FR-008
- **種別:** 正常系
- **前提条件:** `admin` ユーザーでログイン済み（`Authenticate("admin", "password123")` 呼び出し後）
- **入力:** menuId = "btnContract", "btnROUAsset", "btnMonthlyPayments", "btnMonthlyAccounting", "btnPeriodBalance", "btnTaxAdjustment", "btnMaster" の各値
- **期待結果:** 全ての menuId に対して戻り値が `True`

---

### TC-009: HasAccess — accounting は btnMaster にアクセス不可

- **対象:** `AuthorizationService.HasAccess`
- **関連要件:** US-005, FR-008
- **種別:** 正常系
- **前提条件:** `keiri01`（role='accounting'）でログイン済み
- **入力（アクセス可）:** "btnContract", "btnROUAsset", "btnMonthlyPayments", "btnMonthlyAccounting", "btnPeriodBalance", "btnTaxAdjustment"
- **入力（アクセス不可）:** "btnMaster"
- **期待結果:**
  - 前者6メニューはすべて `True`
  - `"btnMaster"` は `False`

---

### TC-010: HasAccess — general_affairs は月次系4メニューにアクセス不可

- **対象:** `AuthorizationService.HasAccess`
- **関連要件:** US-005, FR-008
- **種別:** 正常系
- **前提条件:** `soumu01`（role='general_affairs'）でログイン済み
- **入力（アクセス可）:** "btnContract", "btnROUAsset"
- **入力（アクセス不可）:** "btnMonthlyPayments", "btnMonthlyAccounting", "btnPeriodBalance", "btnTaxAdjustment", "btnMaster"
- **期待結果:**
  - "btnContract", "btnROUAsset" は `True`
  - 月次系4メニューおよび "btnMaster" は `False`

---

### TC-011: HasAccess — viewer は btnMaster にアクセス不可

- **対象:** `AuthorizationService.HasAccess`
- **関連要件:** US-005, FR-008
- **種別:** 正常系
- **前提条件:** `viewer01`（role='viewer'）でログイン済み
- **入力（アクセス可）:** "btnContract", "btnROUAsset", "btnMonthlyPayments", "btnMonthlyAccounting", "btnPeriodBalance", "btnTaxAdjustment"
- **入力（アクセス不可）:** "btnMaster"
- **期待結果:**
  - 前者6メニューはすべて `True`
  - `"btnMaster"` は `False`

---

### TC-012: HasAccess — 未ログイン状態では全メニューにアクセス不可

- **対象:** `AuthorizationService.HasAccess`
- **関連要件:** FR-003
- **種別:** 異常系
- **前提条件:** `Logout()` を呼び出した後（CurrentUser = Nothing）
- **入力:** "btnContract"
- **期待結果:** 戻り値が `False`（または例外をスローせず False を返す）

---

### TC-013: HasAccess — 存在しない menuId

- **対象:** `AuthorizationService.HasAccess`
- **関連要件:** FR-003
- **種別:** 境界値
- **前提条件:** `admin` でログイン済み
- **入力:** `menuId = "btnNonExistent"`
- **期待結果:** 戻り値が `False`（定義外の menuId はアクセス不可として扱う）

---

### TC-014: Logout — セッションがクリアされる

- **対象:** `AuthorizationService.Logout()`
- **関連要件:** US-006, FR-003
- **種別:** 正常系
- **前提条件:** `admin` でログイン済み（`CurrentUser` が設定されている状態）
- **入力:** `AuthorizationService.Current.Logout()` 呼び出し
- **期待結果:** `AuthorizationService.Current.CurrentUser` が Nothing になる

---

### TC-015: UserInfo — プロパティの設定・取得

- **対象:** `UserInfo` クラス
- **関連要件:** FR-004
- **種別:** 正常系
- **前提条件:** なし
- **入力:** `New UserInfo()` を生成し、各プロパティに値をセット
  - `UserId = 1`, `LoginId = "admin"`, `UserName = "管理者"`, `Role = "admin"`, `IsActive = True`
- **期待結果:** 設定した値がそのまま取得できる

---

### TC-016: BCrypt — ハッシュ化と正しいパスワードでの検証成功

- **対象:** `BCrypt.Net.BCrypt.HashPassword` / `BCrypt.Net.BCrypt.Verify`
- **関連要件:** FR-003, NFR-001
- **種別:** 正常系
- **前提条件:** BCrypt.Net-Next が参照済み
- **入力:** `plainPassword = "password123"`
- **処理:**
  1. `hash = BCryptNet.HashPassword(plainPassword)`
  2. `result = BCryptNet.Verify(plainPassword, hash)`
- **期待結果:** `result = True`

---

### TC-017: BCrypt — 誤ったパスワードでの検証失敗

- **対象:** `BCrypt.Net.BCrypt.Verify`
- **関連要件:** FR-003, NFR-001
- **種別:** 異常系
- **前提条件:** "password123" のハッシュが生成済み
- **入力:** `BCryptNet.Verify("wrongPassword", hash)`
- **期待結果:** 戻り値が `False`

---

### TC-018: BCrypt — 同一平文から異なるハッシュが生成される

- **対象:** `BCrypt.Net.BCrypt.HashPassword`
- **関連要件:** NFR-001（ソルトの一意性）
- **種別:** 正常系
- **前提条件:** なし
- **入力:** `plainPassword = "password123"` で2回ハッシュ化
- **期待結果:** `hash1 <> hash2`（ソルトが異なるため）、かつ両方とも `BCryptNet.Verify("password123", hashN)` が `True`

---

### TC-019: 結合: 起動フォームが FrmLogin であること

- **対象:** `Application.Designer.vb` の `OnCreateMainForm`
- **関連要件:** US-001, FR-001
- **種別:** 結合テスト（手動）
- **前提条件:** `Application.Designer.vb` の `MainForm` が `FrmLogin` に変更済み
- **手順:** アプリケーションを起動する
- **期待結果:**
  - FrmLogin が最初に表示される
  - FrmFlexMenu は起動直後には表示されない
  - タスクバーに FrmLogin が表示される

---

### TC-020: 結合: 正常ログイン → FrmFlexMenu 表示

- **対象:** `FrmLogin.btnLogin_Click` → `AuthorizationService.Authenticate` → `FrmFlexMenu.New(UserInfo)`
- **関連要件:** US-002, FR-006, FR-007
- **種別:** 結合テスト（手動）
- **前提条件:** テストユーザー `admin` が DB に存在する
- **手順:**
  1. FrmLogin を表示する
  2. `txtLoginId` に "admin" を入力する
  3. `txtPassword` に "password123" を入力する
  4. `btnLogin` をクリックする
- **期待結果:**
  - FrmFlexMenu が表示される
  - FrmLogin は非表示になる（Visible=False）
  - FrmFlexMenu のタイトルが表示される

---

### TC-021: 結合: パスワード不一致エラー表示

- **対象:** `FrmLogin.btnLogin_Click` → エラーメッセージ表示
- **関連要件:** US-003, FR-006
- **種別:** 結合テスト（手動）
- **前提条件:** テストユーザー `admin` が DB に存在する
- **手順:**
  1. `txtLoginId` に "admin"、`txtPassword` に "wrongPassword" を入力して `btnLogin` クリック
- **期待結果:**
  - `lblError` に "ログインIDまたはパスワードが正しくありません" が表示される
  - `lblError.ForeColor = Color.Red`
  - `txtPassword.Text` が空になる
  - `txtLoginId` にフォーカスが移る
  - FrmLogin が引き続き表示される

---

### TC-022: 結合: 存在しないユーザーのエラーメッセージが TC-021 と同一

- **対象:** `FrmLogin.btnLogin_Click`
- **関連要件:** US-003, NFR-001（エラーメッセージ統一）
- **種別:** 結合テスト（手動）
- **前提条件:** `nonexistent_user` が DB に存在しない
- **手順:**
  1. `txtLoginId` に "nonexistent_user"、`txtPassword` に "anyPassword" を入力して `btnLogin` クリック
- **期待結果:**
  - `lblError` に "ログインIDまたはパスワードが正しくありません" が表示される（TC-021 と同一文言）

---

### TC-023: 結合: 空欄ログイン → バリデーションエラー

- **対象:** `FrmLogin.btnLogin_Click` — 入力バリデーション
- **関連要件:** US-003, FR-006
- **種別:** 結合テスト（手動）
- **前提条件:** なし
- **手順:**
  1. `txtLoginId` と `txtPassword` を空のままにして `btnLogin` クリック
- **期待結果:**
  - `lblError` にバリデーションエラーメッセージが表示される
  - DB へのアクセスは発生しない

---

### TC-024: 結合: 無効アカウントログイン

- **対象:** `FrmLogin.btnLogin_Click`
- **関連要件:** US-003, FR-006
- **種別:** 結合テスト（手動）
- **前提条件:** テストユーザー `disabled01`（is_active=FALSE）が DB に存在する
- **手順:**
  1. `txtLoginId` に "disabled01"、`txtPassword` に "password123" を入力して `btnLogin` クリック
- **期待結果:**
  - `lblError` に "このアカウントは無効化されています" が表示される
  - FrmLogin が引き続き表示される

---

### TC-025: 結合: admin 権限 — 全7メニューボタンが有効

- **対象:** `FrmFlexMenu.ApplyPermissions()` — admin ロール
- **関連要件:** US-005, FR-007, FR-008
- **種別:** 結合テスト（手動）
- **前提条件:** `admin`（role='admin'）でログイン済み
- **手順:** FrmFlexMenu を表示する
- **期待結果:**
  - btnContract.Enabled = True
  - btnROUAsset.Enabled = True
  - btnMonthlyPayments.Enabled = True
  - btnMonthlyAccounting.Enabled = True
  - btnPeriodBalance.Enabled = True
  - btnTaxAdjustment.Enabled = True
  - btnMaster.Enabled = True

---

### TC-026: 結合: accounting 権限 — btnMaster が無効

- **対象:** `FrmFlexMenu.ApplyPermissions()` — accounting ロール
- **関連要件:** US-005, FR-007, FR-008
- **種別:** 結合テスト（手動）
- **前提条件:** `keiri01`（role='accounting'）でログイン済み
- **期待結果:**
  - btnContract, btnROUAsset, btnMonthlyPayments, btnMonthlyAccounting, btnPeriodBalance, btnTaxAdjustment の Enabled = True
  - btnMaster.Enabled = False

---

### TC-027: 結合: general_affairs 権限 — 月次系4ボタンが無効

- **対象:** `FrmFlexMenu.ApplyPermissions()` — general_affairs ロール
- **関連要件:** US-005, FR-007, FR-008
- **種別:** 結合テスト（手動）
- **前提条件:** `soumu01`（role='general_affairs'）でログイン済み
- **期待結果:**
  - btnContract.Enabled = True
  - btnROUAsset.Enabled = True
  - btnMonthlyPayments.Enabled = False
  - btnMonthlyAccounting.Enabled = False
  - btnPeriodBalance.Enabled = False
  - btnTaxAdjustment.Enabled = False
  - btnMaster.Enabled = False

---

### TC-028: 結合: viewer 権限 — btnMaster 以外有効

- **対象:** `FrmFlexMenu.ApplyPermissions()` — viewer ロール
- **関連要件:** US-005, FR-007, FR-008
- **種別:** 結合テスト（手動）
- **前提条件:** `viewer01`（role='viewer'）でログイン済み
- **期待結果:**
  - btnContract, btnROUAsset, btnMonthlyPayments, btnMonthlyAccounting, btnPeriodBalance, btnTaxAdjustment の Enabled = True
  - btnMaster.Enabled = False

---

### TC-029: 結合: FrmFlexMenu クローズ → FrmLogin クローズ → プロセス終了

- **対象:** `FrmLogin.ShowMainMenu` の `FormClosed` ハンドラ → `FrmLogin.Close()`
- **関連要件:** US-006, FR-001
- **種別:** 結合テスト（手動）
- **前提条件:** `admin` でログイン済み。FrmFlexMenu が表示されている
- **手順:** FrmFlexMenu の閉じるボタン（×）をクリックする
- **期待結果:**
  - FrmLogin も閉じられる
  - アプリケーションプロセスが終了する（タスクマネージャーで確認）

---

### TC-030: 結合: 終了ボタン → Application.Exit()

- **対象:** `FrmLogin.btnExit_Click`
- **関連要件:** US-004, FR-006
- **種別:** 結合テスト（手動）
- **前提条件:** FrmLogin が表示されている
- **手順:** `btnExit` をクリックする
- **期待結果:** アプリケーションが終了する

---

### TC-031: 結合: Esc キー → Application.Exit()

- **対象:** `FrmLogin` の `CancelButton = btnExit`
- **関連要件:** US-004, NFR-003
- **種別:** 結合テスト（手動）
- **前提条件:** FrmLogin が表示されており、いずれかのコントロールにフォーカスがある
- **手順:** Esc キーを押す
- **期待結果:** TC-030 と同じ（アプリケーションが終了する）

---

### TC-032: 結合: Enter キー → ログイン処理実行

- **対象:** `FrmLogin` の `AcceptButton = btnLogin`
- **関連要件:** US-002, NFR-003
- **種別:** 結合テスト（手動）
- **前提条件:** FrmLogin が表示されており、`txtPassword` にフォーカスがある
- **手順:**
  1. `txtLoginId` に "admin"、`txtPassword` に "password123" を入力する
  2. Enter キーを押す
- **期待結果:** `btnLogin_Click` と同じ動作（FrmFlexMenu が表示される）

---

### TC-033: 結合: フォーム表示時の初期フォーカス確認

- **対象:** `FrmLogin` — 初期フォーカス
- **関連要件:** NFR-003
- **種別:** 結合テスト（手動）
- **前提条件:** なし
- **手順:** FrmLogin を起動する
- **期待結果:** `txtLoginId` にフォーカスが当たっている状態で表示される

---

## 5. テストデータ設計

### 正常データ

| データ名 | login_id | password（平文） | role | is_active | 用途 |
|---|---|---|---|---|---|
| 管理者ユーザー | admin | password123 | admin | TRUE | admin 権限テスト全般 |
| 経理ユーザー | keiri01 | password123 | accounting | TRUE | accounting 権限テスト |
| 総務ユーザー | soumu01 | password123 | general_affairs | TRUE | general_affairs 権限テスト |
| 監査ユーザー | viewer01 | password123 | viewer | TRUE | viewer 権限テスト |

### 異常データ

| データ名 | login_id | password（平文） | role | is_active | 期待エラー |
|---|---|---|---|---|---|
| 無効ユーザー | disabled01 | password123 | viewer | FALSE | AuthResult.AccountDisabled |
| 存在しないユーザー | nonexistent_user | — | — | — | AuthResult.UserNotFound |
| 誤パスワード | admin | wrongPassword | — | — | AuthResult.InvalidPassword |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| 空のログインID | loginId = "" | 空文字は UserNotFound になること |
| 空のパスワード | password = "" | 空文字は InvalidPassword になること |
| 最大長ログインID | loginId = "a" * 50 文字 | MaxLength=50 の上限値（FrmLogin の txtLoginId.MaxLength） |
| 最大長パスワード | password = "p" * 100 文字 | MaxLength=100 の上限値（txtPassword.MaxLength） |
| 存在しない menuId | menuId = "btnNonExistent" | HasAccess が False を返すこと |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `LeaseM4BS/LeaseM4BS.Tests/AuthorizationServiceTests.vb` | AuthorizationService.Authenticate, HasAccess, Logout | 14（TC-001 〜 TC-014） |
| `LeaseM4BS/LeaseM4BS.Tests/UserInfoTests.vb` | UserInfo クラス | 1（TC-015） |
| `LeaseM4BS/LeaseM4BS.Tests/BCryptTests.vb` | BCrypt ハッシュ化・検証 | 3（TC-016 〜 TC-018） |

**結合テスト（手動）:** TC-019 〜 TC-033（15件）は手動実施。MSTest プロジェクトへの自動化は Phase 2 以降で検討する（WinForms UI の自動テストには Coded UI Test または UI Automation が必要なため）。

**合計:** 単体テスト 18件 + 結合テスト（手動）15件 = 33件

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

`LeaseM4BS.Tests` プロジェクトは現時点では `Placeholder.vb`（空の Namespace 定義のみ）だが、vbproj には MSTest 2.2.8 が設定済みのため、標準の MSTest パターンをそのまま採用する。

**基本的なテストクラス構造（VB.NET MSTest）:**

```vb
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace LeaseM4BS.Tests

    <TestClass>
    Public Class AuthorizationServiceTests

        <TestInitialize>
        Public Sub Setup()
            ' シングルトン状態をリセット
            AuthorizationService.Current.Logout()
        End Sub

        <TestMethod>
        Public Sub Authenticate_ValidCredentials_ReturnsSuccess()
            ' Arrange
            Dim sut = AuthorizationService.Current

            ' Act
            Dim result = sut.Authenticate("admin", "password123")

            ' Assert
            Assert.AreEqual(AuthResult.Success, result)
            Assert.IsNotNull(sut.CurrentUser)
            Assert.AreEqual("admin", sut.CurrentUser.LoginId)
        End Sub

    End Class

End Namespace
```

### モック/スタブの使い方

- **CrudHelper / DB:** モックなし。テスト用 DB に直接接続する
  - 理由: `CrudHelper` は `IDisposable` を実装しているが、インターフェースを持たないため、テストプロジェクト側からモックの差し込みが困難
  - テスト実行前に `sql/seed_test_users.sql` を投入し、テスト後は不変であることを確認する
- **BCrypt:** モックなし。ライブラリを直接使用する

### テストヘルパー/ユーティリティの活用

- `TestInitialize` 属性で `AuthorizationService.Current.Logout()` を呼び出し、シングルトン状態を各テスト前にリセットする
- `TestCleanup` 属性で認証成功テスト後に `failed_login_count` が変更された場合は DB を元の状態に戻す（必要に応じて UPDATE を実行）

### vbproj への追加が必要な `<Compile>` エントリ

実装時に `LeaseM4BS.Tests.vbproj` の `<ItemGroup>` に以下を追加する:

```xml
<Compile Include="AuthorizationServiceTests.vb" />
<Compile Include="UserInfoTests.vb" />
<Compile Include="BCryptTests.vb" />
```

また、BCrypt.Net-Next が `LeaseM4BS.DataAccess` に追加された後、`LeaseM4BS.Tests` は `ProjectReference` 経由でアセンブリを参照するため、`LeaseM4BS.Tests.vbproj` への BCrypt の直接参照は不要。

---

## 8. DB依存テストの方針

### 採用方針: 実 DB 接続（モックなし）

`CrudHelper` はインターフェース（IDisposable のみ）を持たず、内部で直接 `NpgsqlConnection` を生成するため、単体テストでのモック化は困難である。そのため、DB依存テスト（TC-001〜TC-014）は実際の PostgreSQL DB に接続して実施する。

### 前提条件と環境分離

- テスト実行環境には PostgreSQL が起動済みであること
- テスト用 DB（`lease_m4bs`）は本番 DB と同一インスタンスであっても、テーブルデータを管理することで問題ない
- `LeaseM4BS.Tests/App.config` に接続文字列を設定する。未設定の場合は `DbConnectionManager` のデフォルト値（`Host=localhost;Port=5432;Database=lease_new`）が使用されるが、実際の DB 名 `lease_m4bs` と異なるため要注意

### 今後の改善（Phase 2 以降）

Phase 2 以降で `AuthorizationService` を実装する際に `ICrudHelper` インターフェースを導入することで、モックを使った真の単体テストへの移行が可能になる。現 Phase 1 では実 DB 接続を前提とする。
