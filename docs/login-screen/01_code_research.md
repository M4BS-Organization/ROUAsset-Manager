# コードベース調査資料: login-screen

**作成日:** 2026-03-11
**調査対象:** LeaseM4BS ログイン画面実装（ISS-006）
**ベース計画書:** docs/ログイン画面_実装計画.md

---

## 1. プロジェクト概要

### フレームワーク・言語

| 項目 | 値 |
|---|---|
| 言語 | VB.NET |
| フレームワーク | .NET Framework 4.7.2 |
| DB | PostgreSQL |
| ドライバ | Npgsql 6.0.11 |
| テストフレームワーク | MSTest 2.2.8 |
| IDE想定 | Visual Studio（Designer 自動生成あり） |

### ディレクトリ構成

```
LeaseM4BS-1/
├── LeaseM4BS/
│   ├── LeaseM4BS.DataAccess/         クラスライブラリ（DAL + Business層）
│   │   ├── CrudHelper.vb             汎用 CRUD ヘルパー（IDisposable）
│   │   ├── DbConnectionManager.vb    接続管理（IDisposable）
│   │   ├── MasterDataLoader.vb       マスタデータ読み込みユーティリティ
│   │   ├── UsageExamples.vb          使用例（参照用）
│   │   ├── LeaseM4BS.DataAccess.vbproj
│   │   └── packages.config
│   └── LeaseM4BS.Tests/              MSTest テストプロジェクト
│       ├── Placeholder.vb            現在は空（ビルドエラー防止用）
│       ├── LeaseM4BS.Tests.vbproj
│       └── packages.config
├── LeaseM4BS.TestWinForms/
│   └── LeaseM4BS.TestWinForms/       WinForms 実行プロジェクト
│       ├── FrmFlexMenu.vb            メインメニュー（現在の起動フォーム）
│       ├── FrmFlexMenu.Designer.vb
│       ├── FrmFlexContract.vb        各画面（UserControl）
│       ├── FrmFlexROUAsset.vb
│       ├── FrmFlexMonthlyPayments.vb
│       ├── FrmFlexMonthlyAccounting.vb
│       ├── FrmFlexPeriodBalance.vb
│       ├── FrmFlexTaxAdjustment.vb
│       ├── FrmFlexMaster.vb
│       ├── FrmAssetDetailEntry.vb    ポップアップ画面（PopupBaseForm継承）
│       ├── FrmAssetDetailEntry.Designer.vb
│       ├── FrmLeaseContractMain.vb
│       ├── PopupBaseForm.vb          ポップアップ基底クラス
│       ├── CtbDataStore.vb
│       ├── CtbRepository.vb
│       ├── LeaseM4BS.TestWinForms.vbproj
│       ├── packages.config
│       └── My Project/
│           ├── Application.Designer.vb  起動フォーム設定
│           └── AssemblyInfo.vb
├── sql/
│   ├── init.sql              DB・ユーザー作成
│   ├── master_tables.sql     マスタテーブル（m_company 等）
│   ├── tw_tables.sql         トランザクションテーブル（tw_lease_contract 等）
│   ├── ctb_tables.sql        連結テーブル（ctb_lease_integrated 等）
│   └── seed_data.sql         初期データ投入
└── docs/
    ├── ログイン画面_実装計画.md
    ├── ログイン画面_検討資料.md
    └── project-documentation/
        ├── 05_DB設計書.md
        ├── 09_コーディング規約書.md
        └── ...
```

### 主要な依存ライブラリ

| パッケージ | バージョン | 用途 |
|---|---|---|
| Npgsql | 6.0.11 | PostgreSQL ドライバ |
| MSTest.TestFramework | 2.2.8 | テストフレームワーク（Testsプロジェクトのみ） |
| System.Text.Json | 6.0.x | JSON処理 |
| BCrypt.Net-Next | 未追加 | **ログイン実装で追加が必要** |

---

## 2. アーキテクチャ概要

### アーキテクチャパターン

Access DAO からの移行プロジェクト。明確な Clean Architecture / MVC ではなく、以下の2層構成：

- **DataAccess層（LeaseM4BS.DataAccess）**: DB接続・CRUD操作・マスタ読み込み・（AuthorizationService を追加予定）
- **UI層（LeaseM4BS.TestWinForms）**: WinForms フォーム群、UserControl 群

### プロジェクト参照関係

```
LeaseM4BS.TestWinForms
  └─ ProjectReference → LeaseM4BS.DataAccess（{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}）

LeaseM4BS.Tests
  └─ ProjectReference → LeaseM4BS.DataAccess（同上）
```

`LeaseM4BS.TestWinForms.vbproj:243` に `ProjectReference` として定義されている。

---

## 3. 関連する既存コード

### 3-1. 直接関連ファイル

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/My Project/Application.Designer.vb` | 起動フォーム設定（FrmFlexMenu → FrmLogin に変更） | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmFlexMenu.vb` | メインメニュー（コンストラクタ追加・ApplyPermissions追加） | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmFlexMenu.Designer.vb` | メニューボタン定義（ボタン名の確認） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/CrudHelper.vb` | 認証クエリに使用するGetDataTable/ExecuteNonQuery | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/DbConnectionManager.vb` | DB接続（接続文字列取得） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/LeaseM4BS.DataAccess.vbproj` | BCrypt.Net-Next パッケージ参照追加が必要 | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms.vbproj` | FrmLogin の Compile Include 追加が必要 | 高 |

### 3-2. 参照（パターン学習）ファイル

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmAssetDetailEntry.Designer.vb` | Designer パターン・色定数・レイアウト規約 | 中 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/PopupBaseForm.vb` | フォーム基底クラスの実装例 | 中 |
| `LeaseM4BS/LeaseM4BS.DataAccess/MasterDataLoader.vb` | CrudHelper の利用パターン | 中 |
| `LeaseM4BS/LeaseM4BS.DataAccess/UsageExamples.vb` | NpgsqlParameter の使い方参照例 | 中 |
| `LeaseM4BS/LeaseM4BS.Tests/Placeholder.vb` | テストプロジェクト現状（空）の確認 | 低 |

---

## 4. 使用パターン

### 4-1. CrudHelper パターン（GetDataTable）

`CrudHelper.vb:46` の `GetDataTable` シグネチャ:

```vb
Public Function GetDataTable(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As DataTable
```

**パラメータの渡し方（推奨パターン）:**

```vb
' CrudHelper を使ったパラメータ付きクエリ
Dim params As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@login_id", loginId)
}
Dim dt As DataTable = crud.GetDataTable(
    "SELECT * FROM tm_USER WHERE login_id = @login_id", params)
```

- `parameters` は `List(Of NpgsqlParameter)` で渡す
- 内部で `param.Clone()` してから `cmd.Parameters.Add` するため、同じパラメータリストを再利用可能
- 例外発生時は SQL・パラメータ情報付きの詳細メッセージで `Throw New Exception(...)` される

### 4-2. CrudHelper パターン（ExecuteNonQuery）

`CrudHelper.vb:98` の `ExecuteNonQuery` シグネチャ:

```vb
Public Function ExecuteNonQuery(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As Integer
```

**UPDATE の典型パターン（last_login_at 更新等）:**

```vb
Dim params As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@set_last_login_at", DateTime.Now),
    New NpgsqlParameter("@user_id", userId)
}
crud.ExecuteNonQuery(
    "UPDATE tm_USER SET last_login_at = @set_last_login_at WHERE user_id = @user_id",
    params)
```

`CrudHelper.Update()` ヘルパーメソッドも利用可能（`CrudHelper.vb:283`）:

```vb
crud.Update("tm_USER",
    New Dictionary(Of String, Object) From {
        {"last_login_at", DateTime.Now},
        {"failed_login_count", 0}
    },
    "user_id = @user_id",
    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@user_id", userId)})
```

### 4-3. DbConnectionManager 接続文字列の取得方法

`DbConnectionManager.vb:59` の `GetConnectionString()`:

接続文字列の優先順位（`DbConnectionManager.vb:62-76`）:

```
1. App.config の connectionStrings["LeaseM4BS"]
2. 環境変数 LEASE_M4BS_CONNECTION_STRING
3. デフォルト（ローカル開発用ハードコード）
   "Host=localhost;Port=5432;Database=lease_new;Username=manager;Password=pass;Timeout=3;Command Timeout=5"
```

`AuthorizationService` での使用パターン:

```vb
' CrudHelper はデフォルトコンストラクタで App.config から接続文字列を読む
Using crud As New CrudHelper()
    Dim dt As DataTable = crud.GetDataTable(sql, params)
End Using
```

### 4-4. FrmFlexMenu コンストラクタ・メニューボタン

`FrmFlexMenu.vb:26-31` 現在のコンストラクタ:

```vb
Public Sub New()
    InitializeComponent()
    SetupMenuButtons()
    ' デフォルトで契約書（フレックス）を表示
    SwitchContent(btnContract)
End Sub
```

`SetupMenuButtons()` が処理するボタン配列（`FrmFlexMenu.vb:37-41`）:

```vb
Dim menuButtons() As Button = {
    btnContract, btnROUAsset, btnMonthlyPayments,
    btnMonthlyAccounting, btnPeriodBalance, btnTaxAdjustment,
    btnMaster
}
```

`SwitchContent(menuButton As Button)` は `FrmFlexMenu.vb:63` に定義。メニューボタンに応じて対応する UserControl を生成して `pnlContent` に表示する。

色定数（`FrmFlexMenu.vb:12-14`）:

```vb
Private ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)    ' 濃紺（通常）
Private ReadOnly CLR_ACTIVE As Color = Color.FromArgb(0, 80, 160)    ' やや明るい青（選択中）
Private ReadOnly CLR_HOVER  As Color = Color.FromArgb(0, 70, 140)    ' ホバー
```

### 4-5. Application.Designer.vb 現在の起動フォーム設定

`Application.Designer.vb:34-36` の `OnCreateMainForm`:

```vb
Protected Overrides Sub OnCreateMainForm()
    Me.MainForm = Global.LeaseM4BS.TestWinForms.FrmFlexMenu
End Sub
```

ShutdownStyle は `AfterMainFormCloses`（`Application.Designer.vb:30`）。つまり、**MainForm が閉じるとアプリ終了**。FrmLogin を MainForm にした場合、FrmFlexMenu 閉じ時に FrmLogin.Close() を呼ぶ必要がある。

`StartupObject` は `LeaseM4BS.TestWinForms.My.MyApplication`（`LeaseM4BS.TestWinForms.vbproj:9`）。

---

## 5. 既存の類似機能分析

### 5-1. Designer パターン（FrmAssetDetailEntry.Designer.vb）

色定数はローカル変数として `InitializeComponent()` 内で定義されている（フィールドではない）:

```vb
' FrmAssetDetailEntry.Designer.vb:83-92
Dim CLR_HEADER   As Color = Color.FromArgb(0, 51, 102)
Dim CLR_CARD     As Color = Color.White
Dim CLR_LABEL    As Color = Color.FromArgb(73, 80, 87)
Dim CLR_TEXT     As Color = Color.FromArgb(33, 37, 41)
Dim CLR_BORDER   As Color = Color.FromArgb(222, 226, 230)
Dim CLR_ACCENT   As Color = Color.FromArgb(40, 167, 69)
Dim CLR_READONLY As Color = Color.FromArgb(233, 236, 239)
Dim FNT_LABEL    As New Font("Meiryo", 9.0F, FontStyle.Bold)
Dim FNT_INPUT    As New Font("Meiryo", 9.75F, FontStyle.Regular)
Dim FNT_SECTION  As New Font("Meiryo", 10.0F, FontStyle.Bold)
```

FrmAssetDetailEntry は `PopupBaseForm` を継承（`FrmAssetDetailEntry.Designer.vb:3`）。FrmLogin は `Form` を直接継承する（ポップアップではないため）。

### 5-2. PopupBaseForm

`PopupBaseForm.vb` は画面位置・サイズをデスクトップ作業領域内に収める基底クラス。FrmLogin では不要（FixedDialog + CenterScreen で制御）。

### 5-3. vbproj へのフォーム登録パターン

`LeaseM4BS.TestWinForms.vbproj:150-155`:

```xml
<!-- Form の登録パターン -->
<Compile Include="FrmFlexMenu.vb">
  <SubType>Form</SubType>
</Compile>
<Compile Include="FrmFlexMenu.Designer.vb">
  <DependentUpon>FrmFlexMenu.vb</DependentUpon>
</Compile>
```

FrmLogin 追加時は同様のパターンで2エントリが必要。

### 5-4. DataAccess.vbproj へのクラス登録パターン

`LeaseM4BS.DataAccess.vbproj:113-115`:

```xml
<Compile Include="CrudHelper.vb" />
<Compile Include="DbConnectionManager.vb" />
<Compile Include="MasterDataLoader.vb" />
```

`AuthorizationService.vb`、`UserInfo.vb`、`AuthResult.vb` も同様のパターンで追加する。

---

## 6. 技術的制約・注意事項

### 6-1. BCrypt.Net-Next の追加先

**重要:** BCrypt.Net-Next は `LeaseM4BS.DataAccess` プロジェクトに追加する（AuthorizationService.vb がそこに配置されるため）。`LeaseM4BS.TestWinForms` には不要。

packages.config（`LeaseM4BS/LeaseM4BS.DataAccess/packages.config`）に追記:
```xml
<package id="BCrypt.Net-Next" version="4.0.3" targetFramework="net472" />
```

vbproj の `<ItemGroup>` にも `<Reference>` エントリを追加する必要がある。

### 6-2. VB.NET の Designer 制約

**重要:** WinForms Designer はパラメータなしコンストラクタを必要とする。実装計画書 §6 の注意通り、FrmFlexMenu のパラメータなしコンストラクタを残したまま、オーバーロードを追加する。

### 6-3. OptionStrict Off

両プロジェクトとも `<OptionStrict>Off</OptionStrict>`（DataAccess.vbproj:45, TestWinForms.vbproj:63）。型の暗黙変換が許可されているが、新規コードでは明示的な型変換を推奨（コーディング規約書§2.2）。

### 6-4. tm_USER テーブルの現状

`sql/` ディレクトリに `tm_USER` のDDLファイルは存在しない（DB設計書によると m7_db の tm_USER がシサンM7 側のもの）。LeaseM4BS 側の tm_USER は `sql/master_tables.sql` 等には未定義であり、**実装計画書の DDL（alter_tm_user.sql）で新規定義する**形が正しい。

DB設計書（`docs/project-documentation/05_DB設計書.md:164-166`）:
```
#### tm_USER（ユーザー）
ユーザー認証・権限管理に使用。AuthorizationServiceが参照。
```
（詳細カラム定義はドキュメントに記載なし → 実装計画書の DDL が初出定義）

### 6-5. NpgsqlParameter のクローン挙動

`CrudHelper.vb:73` で `param.Clone()` を使用。パラメータオブジェクトは Clone されるため、同一リストを使い回しても問題ないが、`@set_` プレフィックスは `Update()` ヘルパーが自動付与する（`CrudHelper.vb:301`）。直接 SQL を書く場合はプレフィックス不要。

### 6-6. データベース名の変更点

`DbConnectionManager.vb:76` のデフォルト接続文字列は `Database=lease_new` になっているが、実際の開発環境 DB名は `lease_m4bs`（DB設計書§1.2）。App.config で上書きされている前提。

---

## 7. 命名規則・コーディング規約

コーディング規約書（`docs/project-documentation/09_コーディング規約書.md`）から抜粋:

### ファイル命名規則

| 種別 | 規則 | ログイン画面での適用例 |
|---|---|---|
| フォーム | `Frm[機能名].vb` | `FrmLogin.vb` |
| フォームデザイナー | `Frm[機能名].Designer.vb` | `FrmLogin.Designer.vb` |
| クラス | `[クラス名].vb` | `AuthorizationService.vb`, `UserInfo.vb`, `AuthResult.vb` |

### クラス・メンバー命名規則

| 対象 | 規則 | 例 |
|---|---|---|
| クラス名 | PascalCase | `AuthorizationService`, `UserInfo` |
| メソッド名 | PascalCase | `Authenticate`, `HasAccess`, `Logout` |
| プロパティ名 | PascalCase | `Current`, `CurrentUser`, `Role` |
| プライベートフィールド | `_camelCase` | `_currentUser`, `_menuPermissions` |
| 列挙型 | PascalCase | `AuthResult` |
| 列挙値 | PascalCase | `Success`, `UserNotFound`, `InvalidPassword` |

### コントロール命名規則（FrmLogin に適用）

| プレフィックス | コントロール | FrmLogin での例 |
|---|---|---|
| `txt` | TextBox | `txtLoginId`, `txtPassword` |
| `lbl` | Label | `lblTitle`, `lblLoginId`, `lblPassword`, `lblError`, `lblVersion` |
| `btn` | Button | `btnLogin`, `btnExit` |

### データベース命名規則

| 対象 | 規則 | ログイン実装での例 |
|---|---|---|
| マスタテーブル | `tm_[名前]` | `tm_USER` |
| ユニーク制約 | `uq_[テーブル]_[カラム]` | `uq_user_login_id` |
| チェック制約 | `chk_[テーブル]_[カラム]` | `chk_user_role` |

---

## 8. 新規ファイル実装時の具体的留意点

### FrmLogin.vb（LeaseM4BS.TestWinForms）

- 直接 `System.Windows.Forms.Form` を継承（PopupBaseForm は不使用）
- `InitializeComponent()` で基本プロパティを設定
- `AcceptButton = btnLogin`、`CancelButton = btnExit` を Designer.vb で設定
- Font は Meiryo（既存フォームに合わせる）

### AuthorizationService.vb（LeaseM4BS.DataAccess）

- `Shared` プロパティ `Current` でシングルトン管理
- 認証処理では `CrudHelper.GetDataTable()` でユーザー取得、`CrudHelper.ExecuteNonQuery()` でログイン日時・失敗回数を更新
- BCrypt.Net-Next の名前空間: `BCrypt.Net.BCrypt.Verify(password, hash)` / `BCrypt.Net.BCrypt.HashPassword(password)`

### vbproj 変更のチェックリスト

**LeaseM4BS.DataAccess.vbproj:**
- `packages.config` に `BCrypt.Net-Next` を追記
- `<ItemGroup>` の `<Reference>` に BCrypt DLL の HintPath を追加
- `<ItemGroup>` の `<Compile>` に `AuthorizationService.vb`、`UserInfo.vb`、`AuthResult.vb` を追加

**LeaseM4BS.TestWinForms.vbproj:**
- `<Compile>` に `FrmLogin.vb`（`SubType=Form`）と `FrmLogin.Designer.vb`（`DependentUpon`）を追加

### Application.Designer.vb 変更

`LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/My Project/Application.Designer.vb:35` を以下に変更:

```vb
' 変更前
Me.MainForm = Global.LeaseM4BS.TestWinForms.FrmFlexMenu
' 変更後
Me.MainForm = Global.LeaseM4BS.TestWinForms.FrmLogin
```

> 注意: このファイルは `<AutoGen>True</AutoGen>` フラグが付いているが、実態はプロジェクトプロパティから生成されるため、Visual Studio のプロジェクトプロパティ「スタートアップオブジェクト」から変更するか、直接ファイル編集する。直接編集の場合、VS のデザイナーによる上書きに注意。
