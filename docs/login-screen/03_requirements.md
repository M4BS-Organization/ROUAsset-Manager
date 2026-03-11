# 要件定義書: ログイン画面（login-screen）

> **作成日:** 2026-03-11
> **対象プロジェクト:** LeaseM4BS（リース会計管理システム）VB.NET .NET Framework 4.7.2 WinForms
> **採用方式:** 方式A（アプリケーション独自認証）
> **関連課題:** ISS-006（認証・権限管理が未実装）
> **ベース資料:** ログイン画面_検討資料.md、ログイン画面_実装計画.md

---

## 1. 機能概要

リース会計管理システム LeaseM4BS に対し、起動時にログイン画面（FrmLogin）を提示し、tm_USER テーブルに登録されたユーザーID／パスワード（BCryptハッシュ）で認証を行う。認証成功後はユーザーに割り当てられた権限ロール（admin / accounting / general_affairs / viewer）に従い、FrmFlexMenu のメニューボタンの有効／無効を制御することで、部門別のアクセス制御を実現する。

---

## 2. ユーザーストーリー

### US-001: アプリ起動時にログイン画面が表示される

- **As a** システム利用者
- **I want** アプリケーション起動時にログイン画面が最初に表示されること
- **So that** 認証を受けていないユーザーがメインメニューに直接アクセスできないようにする

#### 受け入れ基準
- [ ] アプリケーション起動時、FrmFlexMenu ではなく FrmLogin が最初に表示される
- [ ] FrmLogin はタスクバーに表示される
- [ ] FrmLogin のサイズは 400 x 350 px で固定（リサイズ不可）
- [ ] フォームのタイトルに "LeaseM4BS ログイン" が表示される
- [ ] 起動フォームの変更後、既存の Application.Designer.vb で `ShutDownStyle = AfterMainFormCloses` が維持される

---

### US-002: ログインID／パスワードで認証する

- **As a** システム利用者
- **I want** ログインID とパスワードを入力してログインボタンを押すことで認証できること
- **So that** 正規のユーザーのみがシステムを利用できる

#### 受け入れ基準
- [ ] ログインID 入力欄（txtLoginId）が表示され、最大50文字まで入力できる
- [ ] パスワード入力欄（txtPassword）が表示され、入力文字が "*" でマスクされる
- [ ] パスワード欄の最大入力文字数は 100 文字である
- [ ] Enter キーを押すとログイン処理が実行される（AcceptButton = btnLogin）
- [ ] フォーム表示時に txtLoginId に初期フォーカスが当たる
- [ ] tm_USER.login_id と一致するユーザーが存在し、かつ BCrypt.Verify でパスワードが一致する場合、認証成功となる
- [ ] 認証成功後、FrmFlexMenu が表示され、FrmLogin は非表示になる

---

### US-003: 認証失敗時にエラーメッセージを表示する

- **As a** システム利用者
- **I want** ログインに失敗した際に分かりやすいエラーメッセージが表示されること
- **So that** 入力内容を確認して再試行できる

#### 受け入れ基準
- [ ] ログインIDまたはパスワードが誤っている場合、"ログインIDまたはパスワードが正しくありません" と表示される
- [ ] ログインIDが存在しない場合も同一のエラーメッセージを表示する（ユーザー存在の推測を防止）
- [ ] アカウントが無効化（is_active = FALSE）の場合、"このアカウントは無効化されています" と表示される
- [ ] エラーメッセージは赤色（ForeColor = Red）のラベル（lblError）に表示される
- [ ] 認証失敗後、パスワード入力欄がクリアされ、txtLoginId にフォーカスが移る
- [ ] 入力欄が空の状態でログインボタンを押した場合、バリデーションエラーが表示される

---

### US-004: ログイン画面からアプリケーションを終了する

- **As a** システム利用者
- **I want** ログイン画面で「終了」ボタンまたは Esc キーを押してアプリケーションを終了できること
- **So that** ログインしない場合にアプリを終了できる

#### 受け入れ基準
- [ ] 「終了」ボタン（btnExit）が表示される
- [ ] btnExit をクリックすると Application.Exit() が実行される
- [ ] Esc キーを押すと Application.Exit() が実行される（CancelButton = btnExit）

---

### US-005: 権限ロールに応じたメニュー制御

- **As a** システム管理者 / 各部門ユーザー
- **I want** 自分の権限に応じてアクセスできるメニューだけが有効化されること
- **So that** 自部門の業務範囲外の機能へのアクセスが制限される

#### 受け入れ基準
- [ ] admin でログインした場合、全7メニューボタンが Enabled=True になる
- [ ] accounting でログインした場合、btnMaster が Enabled=False になる
- [ ] general_affairs でログインした場合、btnMonthlyPayments / btnMonthlyAccounting / btnPeriodBalance / btnTaxAdjustment が Enabled=False になる
- [ ] viewer でログインした場合、btnMaster が Enabled=False になる（その他は Enabled=True）
- [ ] 権限制御は FrmFlexMenu の初期化時（ApplyPermissions メソッド）に適用される
- [ ] general_affairs がアクセス可能な最初のボタン（btnContract）がデフォルト表示される

---

### US-006: ログイン後にメインメニューを閉じるとアプリが終了する

- **As a** システム利用者
- **I want** FrmFlexMenu を閉じたときにアプリケーションが正常終了すること
- **So that** ログアウト後に不要なプロセスが残らない

#### 受け入れ基準
- [ ] FrmFlexMenu を閉じた際に FrmLogin も閉じられる
- [ ] FrmLogin.Close() の呼び出しにより `ShutDownStyle = AfterMainFormCloses` に従いプロセスが終了する
- [ ] FrmFlexMenu.FormClosed イベントで FrmLogin.Close() が呼び出される

---

## 3. 機能要件

### FR-001: 起動フォームの切り替え
- **説明:** Application.Designer.vb の `OnCreateMainForm` で `Me.MainForm` を `FrmFlexMenu` から `FrmLogin` に変更する
- **優先度:** 必須

### FR-002: tm_USER テーブル拡張
- **説明:** 既存の tm_USER テーブルに認証用カラムを追加する
  - `login_id VARCHAR(50) NOT NULL UNIQUE` — ログイン画面で入力するID
  - `password_hash VARCHAR(256) NOT NULL` — BCryptハッシュ化パスワード
  - `role VARCHAR(20) NOT NULL DEFAULT 'viewer'` — 権限ロール（check制約付き）
  - `is_active BOOLEAN NOT NULL DEFAULT TRUE` — アカウント有効フラグ
  - `last_login_at TIMESTAMP` — 最終ログイン日時（認証成功時に更新）
  - `failed_login_count INTEGER NOT NULL DEFAULT 0` — 連続ログイン失敗回数（Phase 2のロック機能で利用）
  - `locked_until TIMESTAMP` — アカウントロック解除日時（Phase 2で利用）
- **優先度:** 必須

### FR-003: AuthorizationService クラス実装
- **説明:** `LeaseM4BS.DataAccess/AuthorizationService.vb` にシングルトンの認証・権限チェックサービスを実装する
  - `Authenticate(loginId, password) As AuthResult` — 認証実行
  - `HasAccess(menuId As String) As Boolean` — メニューアクセス権チェック
  - `Logout()` — セッションクリア
  - CrudHelper パターン（パラメータ化クエリ）を使用する
  - BCrypt.Net-Next NuGet パッケージでパスワード照合を行う
- **優先度:** 必須

### FR-004: UserInfo クラス実装
- **説明:** `LeaseM4BS.DataAccess/UserInfo.vb` にログインユーザー情報を保持するクラスを実装する
  - `UserId As Integer`、`LoginId As String`、`UserName As String`、`Role As String`、`IsActive As Boolean`
- **優先度:** 必須

### FR-005: AuthResult 列挙型実装
- **説明:** `LeaseM4BS.DataAccess/AuthResult.vb` に認証結果を表す列挙型を実装する
  - `Success`、`UserNotFound`、`InvalidPassword`、`AccountDisabled`、`AccountLocked`（Phase 2用）
- **優先度:** 必須

### FR-006: FrmLogin フォーム実装
- **説明:** `LeaseM4BS.TestWinForms/FrmLogin.vb` + `FrmLogin.Designer.vb` として新規フォームを実装する
  - コントロール一覧は「4. UI要件」を参照
  - ログイン成功時に FrmFlexMenu(UserInfo) を生成して表示する
- **優先度:** 必須

### FR-007: FrmFlexMenu への権限制御追加
- **説明:** FrmFlexMenu に `New(currentUser As UserInfo)` コンストラクタオーバーロードを追加し、`ApplyPermissions()` メソッドで各メニューボタンの Enabled を制御する
  - 既存のパラメータなしコンストラクタ `New()` は Designer 用に維持する
  - メニューアクセス制御は Phase 1 ではハードコードで実装する
- **優先度:** 必須

### FR-008: メニューアクセス制御マッピング
- **説明:** AuthorizationService 内にメニューIDとアクセス可能ロールの対応を定義する（Phase 1はハードコード）

| メニューボタン名 | 表示テキスト | admin | accounting | general_affairs | viewer |
|---|---|---|---|---|---|
| btnContract | 契約書(フレックス) | ○ | ○ | ○ | ○ |
| btnROUAsset | 使用権資産(フレックス) | ○ | ○ | ○ | ○ |
| btnMonthlyPayments | 月次支払(フレックス) | ○ | ○ | × | ○ |
| btnMonthlyAccounting | 月次会計(フレックス) | ○ | ○ | × | ○ |
| btnPeriodBalance | 期間残高(フレックス) | ○ | ○ | × | ○ |
| btnTaxAdjustment | 税法調整(フレックス) | ○ | ○ | × | ○ |
| btnMaster | マスタ | ○ | × | × | × |

- **優先度:** 必須

### FR-009: BCrypt.Net-Next パッケージ追加
- **説明:** `LeaseM4BS.DataAccess.vbproj` に `BCrypt.Net-Next` NuGet パッケージ（最新安定版）を追加する。.NET Framework 4.7.2 との互換性が確認されている。
- **優先度:** 必須

---

## 4. UI要件

### FrmLogin フォーム仕様

| 項目 | 仕様 |
|---|---|
| フォームサイズ | 400 x 350 px（固定） |
| StartPosition | CenterScreen |
| FormBorderStyle | FixedDialog |
| MaximizeBox | False |
| MinimizeBox | False |
| AcceptButton | btnLogin |
| CancelButton | btnExit |
| ShowInTaskbar | True |

### コントロール一覧

| コントロール名 | 種別 | 仕様 |
|---|---|---|
| `lblTitle` | Label | テキスト: "リース会計管理システム"、フォントスタイル: Bold |
| `lblLoginId` | Label | テキスト: "ログインID:" |
| `txtLoginId` | TextBox | MaxLength=50、フォーム表示時に初期フォーカス |
| `lblPassword` | Label | テキスト: "パスワード:" |
| `txtPassword` | TextBox | PasswordChar="*"、UseSystemPasswordChar=True、MaxLength=100 |
| `lblError` | Label | ForeColor=Red、初期状態: Visible=False（または Text=""） |
| `btnLogin` | Button | テキスト: "ログイン"、AcceptButton として設定 |
| `btnExit` | Button | テキスト: "終了"、CancelButton として設定 |
| `lblVersion` | Label | アプリケーションバージョン表示（右下） |

---

## 5. 非機能要件

### NFR-001: セキュリティ

- **パスワードハッシュ化:** BCrypt（WorkFactor=12）でハッシュ化して tm_USER.password_hash に格納する。平文パスワードは DB に保存しない
- **エラーメッセージ統一:** ユーザーが存在しない場合とパスワード不一致の場合のエラーメッセージは同一文言とし、ユーザーの存在有無を外部から推測できないようにする
- **パスワードマスク:** txtPassword は PasswordChar と UseSystemPasswordChar を両方設定し、画面上に平文が表示されないようにする
- **認証失敗後のクリア:** 認証失敗時にパスワード欄の内容をプログラム的にクリアする
- **パラメータ化クエリ:** AuthorizationService での DB 操作は CrudHelper を通じてパラメータ化クエリで実装し、SQL インジェクションを防止する

### NFR-002: パフォーマンス

- **認証応答時間:** BCrypt WorkFactor=12 でのハッシュ照合は通常 200〜400ms 程度。ユーザーが体感できる遅延が生じるが、セキュリティ上許容する（仮定：最大2秒以内に認証処理が完了すること）
- **DB接続:** 既存の CrudHelper / DbConnectionManager パターンに従い、接続の取得・解放を適切に行う

### NFR-003: ユーザビリティ

- **キーボード操作:** Enter キーでログイン（AcceptButton）、Esc キーで終了（CancelButton）に対応する
- **初期フォーカス:** フォーム表示直後に txtLoginId にフォーカスが当たること
- **エラー復帰:** 認証失敗後はパスワード欄クリア後に txtLoginId へフォーカスが移り、再入力しやすい状態にする

### NFR-004: 保守性

- **Phase 2 への移行性:** AuthorizationService のメニューアクセス制御は、Phase 2 で ts_MENU テーブルとの連携に切り替えられるよう、ハードコード部分を `_menuPermissions` ディクショナリとして局所化する
- **コーディング規約:** 既存の `CrudHelper` パターン（パラメータ化クエリ、IDisposable 実装）に準拠する

---

## 6. 前提条件・制約

- VB.NET / .NET Framework 4.7.2 / Windows Forms で実装する
- PostgreSQL を使用しており、DB 操作は既存の CrudHelper クラスを経由する
- tm_USER テーブルが既に PostgreSQL DB に存在する（05_DB設計書.md に定義済み）
- FrmFlexMenu の Designer は自動生成ファイルのため、`New()` コンストラクタは変更せず、オーバーロードを追加する
- Application.Designer.vb は自動生成ファイルであり、変更は `OnCreateMainForm` のオーバーライドメソッド内のみに留める
- BCrypt.Net-Next NuGet パッケージが .NET Framework 4.7.2 に対応していることを前提とする（実装計画で確認済み）

---

## 7. スコープ外（Phase 1 では実装しない）

以下は Phase 2 以降の実装とし、Phase 1 では意図的に対象外とする。

| 項目 | Phase 2 での対応方針 |
|---|---|
| ユーザー管理画面 | btnMaster の管理画面にユーザー管理タブを追加 |
| パスワード変更機能 | FrmLogin にリンク追加、または初回ログイン時に強制変更ダイアログ |
| アカウントロック機能 | failed_login_count >= 5 で locked_until を設定（カラムは Phase 1 で追加済み） |
| 画面内の編集制御（viewer ロール） | 各画面の保存・編集ボタンを Enabled=False にする |
| ts_MENU テーブルとの連携 | ハードコードの _menuPermissions を ts_MENU.ITEM_ENABLED と連携させる |
| 監査ログ | ログイン/ログアウト/主要操作を別テーブルに記録 |
| 自動ログアウト | 無操作タイマーによるセッションタイムアウト |
| 初回ログイン時のパスワード変更強制 | — |

---

## 8. DB要件

### tm_USER テーブル拡張

追加するカラムの定義は実装計画（Step 1）を参照。

**ロール定義（role カラムの許容値）**

| ロール値 | 対象部門 | 権限範囲 |
|---|---|---|
| `admin` | IT部門・システム管理者 | 全メニュー有効 |
| `accounting` | 経理部 | btnMaster 以外全て有効 |
| `general_affairs` | 総務部 | btnContract と btnROUAsset のみ有効 |
| `viewer` | 監査・管理職 | btnMaster 以外全て有効（編集制限は Phase 2） |

**チェック制約:** `role IN ('admin', 'accounting', 'general_affairs', 'viewer')`

**初期データ:** 管理者ユーザー（login_id='admin', role='admin'）を含む5種類のテストユーザーを投入する（詳細は sql/seed_test_users.sql）

---

## 9. 画面遷移要件

```
アプリケーション起動
  ↓
FrmLogin（ログイン画面）← MainForm
  ├─ [ログイン] or Enter
  │     ↓ AuthorizationService.Authenticate()
  │   成功 → FrmLogin.Hide() → New FrmFlexMenu(currentUser).Show()
  │              FrmFlexMenu.FormClosed → FrmLogin.Close() → アプリ終了
  │   失敗 → lblError にメッセージ表示（FrmLogin のまま）
  └─ [終了] or Esc → Application.Exit() → アプリ終了
```

---

## 10. テスト要件

### 結合テスト項目

| # | テスト項目 | 入力条件 | 期待結果 |
|---|---|---|---|
| TC-001 | 起動フォーム確認 | アプリ起動 | FrmLogin が最初に表示される（FrmFlexMenu が起動しない） |
| TC-002 | 正常ログイン | 有効な login_id とパスワード | FrmFlexMenu が表示される |
| TC-003 | パスワード不一致 | 有効な login_id + 誤ったパスワード | "ログインIDまたはパスワードが正しくありません" 表示、パスワード欄クリア |
| TC-004 | 存在しないユーザー | 存在しない login_id | TC-003 と同一メッセージ表示 |
| TC-005 | 空欄ログイン | 入力欄を空にして [ログイン] | バリデーションエラーメッセージ表示 |
| TC-006 | 無効アカウント | is_active=FALSE のユーザー | "このアカウントは無効化されています" 表示 |
| TC-007 | admin 権限 | role='admin' でログイン | 全7メニューボタンが Enabled=True |
| TC-008 | accounting 権限 | role='accounting' でログイン | btnMaster.Enabled=False、他は True |
| TC-009 | general_affairs 権限 | role='general_affairs' でログイン | btnMonthlyPayments / btnMonthlyAccounting / btnPeriodBalance / btnTaxAdjustment が Enabled=False |
| TC-010 | viewer 権限 | role='viewer' でログイン | btnMaster.Enabled=False、他は True |
| TC-011 | FrmFlexMenu クローズ | FrmFlexMenu の閉じるボタン | FrmLogin も閉じられ、プロセスが終了する |
| TC-012 | 終了ボタン | FrmLogin で [終了] ボタン | Application.Exit() が呼ばれアプリが終了する |
| TC-013 | Esc キー | FrmLogin で Esc キー | TC-012 と同様 |
| TC-014 | Enter キー | FrmLogin で Enter キー | ログイン処理が実行される |

---

## 11. 用語定義

| 用語 | 定義 |
|---|---|
| login_id | ログイン画面で入力するユーザー識別子（tm_USER.login_id カラム）。内部キーの user_id（SERIAL）とは別 |
| user_id | tm_USER の SERIAL 主キー。内部参照に使用 |
| password_hash | BCrypt でハッシュ化されたパスワード文字列 |
| role | ユーザーに付与された権限ロール（admin / accounting / general_affairs / viewer） |
| is_active | アカウントの有効・無効フラグ（FALSE のユーザーはログイン不可） |
| AuthorizationService | 認証処理・権限チェック・セッション管理を担うサービスクラス |
| UserInfo | ログイン成功後にセッションで保持するユーザー情報オブジェクト |
| AuthResult | 認証結果を表す列挙型（Success, UserNotFound, InvalidPassword, AccountDisabled 等） |
| MainForm | WinForms アプリケーションのメインフォーム。このフォームを閉じるとアプリが終了する |
| BCrypt | パスワードハッシュ化アルゴリズム。WorkFactor=12 を使用 |

---

## 12. 仮定事項

以下は資料から明確に確認できなかった点について合理的な仮定を置いたもの。実装前に確認が必要な場合はその旨記載する。

1. **BCrypt 認証応答時間（要確認）:** WorkFactor=12 での認証処理は通常 200〜400ms だが、実行環境（サーバースペック）によっては数秒になる場合がある。パフォーマンス要件として「最大2秒以内」と設定したが、実際の環境で計測・調整が必要。

2. **viewer ロールの編集制御（Phase 1 スコープ外）:** viewer ロールは全業務メニューにアクセス可能（Enabled=True）だが、各画面内の編集・保存ボタンの制御は Phase 2 とする。Phase 1 では viewer と accounting は同じメニュー表示となる（btnMaster のみ除外）。

3. **tm_USER の既存データ:** 資料より、現時点では tm_USER には本番データが存在しない想定で DDL を設計している（既存行がある場合のマイグレーション手順は sql/alter_tm_user.sql に含む）。

4. **general_affairs のデフォルト表示画面:** 月次系4ボタンが無効のため、デフォルト表示は btnContract（契約書）とする。資料に明示的な記述はなかったが、アクセス可能な最初のボタンとして合理的と判断した。

5. **FrmLogin のウィンドウアイコン:** 資料に指定がないため、既存フォームのデフォルトアイコンを使用すると仮定する。ブランドロゴ画像が必要な場合は別途確認が必要。
