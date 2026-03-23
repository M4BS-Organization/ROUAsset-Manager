# コードベース調査資料: project-documentation

## 調査メモ: docs/project-documentationブランチのGitオブジェクトへのアクセスについて

docs/project-documentationブランチ（コミット: `b5e0bfafef714a4ee67d148982044c7ed6389537`）は確認できましたが、Gitオブジェクトはzlib圧縮バイナリ形式で格納されており、ファイルシステムツールでは直接読み込めません。

以下の情報は Git のログおよびコミットメッセージ、現在チェックアウトされているブランチ（`devin/1772429481-accounting-tab-redesign`）のソースコードから収集しました。

---

## ドキュメントブランチ情報

| 項目 | 内容 |
|---|---|
| ブランチ名 | `docs/project-documentation` |
| 最新コミット | `b5e0bfafef714a4ee67d148982044c7ed6389537` |
| コミットメッセージ | `docs: プロジェクト資料を追加（要件定義書・基本設計書・開発進捗管理表）` |
| コミット日時 | 2025-05-07 (Unix timestamp: 1773041459 +0900) |
| 作成元 | `origin/main` ブランチから作成 |
| ドキュメント一覧（推定） | `docs/01_要件定義書.md`, `docs/02_基本設計書.md`, `docs/03_開発進捗管理表.md` |

---

## 1. プロジェクト概要

### フレームワーク・言語
- **言語**: Visual Basic .NET (VB.NET)
- **フレームワーク**: .NET Framework 4.7.2
- **UIフレームワーク**: Windows Forms (WinForms)
- **データベース**: PostgreSQL
- **データアクセスライブラリ**: Npgsql 6.0.11
- **テストフレームワーク**: MSTest (Microsoft.VisualStudio.TestTools.UnitTesting)

### ディレクトリ構成

```
LeaseM4BS-1/
├── LeaseM4BS/
│   ├── LeaseM4BS.DataAccess/          # データアクセス層
│   │   ├── CrudHelper.vb              # PostgreSQL CRUD汎用ヘルパー
│   │   ├── DbConnectionManager.vb     # DB接続管理クラス
│   │   ├── UsageExamples.vb           # 使用例（移行ガイド）
│   │   └── LeaseM4BS.DataAccess.vbproj
│   └── LeaseM4BS.Tests/               # 単体テスト
│       ├── UserCrudTests.vb           # CRUDテスト（MSTest）
│       └── LeaseM4BS.Tests.vbproj
├── LeaseM4BS.TestWinForms/
│   └── LeaseM4BS.TestWinForms/        # WinFormsアプリケーション本体
│       ├── FrmFlexMenu.vb             # メインメニュー画面
│       ├── FrmFlexContract.vb         # 契約書（フレックス）一覧画面
│       ├── FrmLeaseContractMain.vb    # リース契約詳細入力画面（メイン）
│       ├── FrmAssetDetailEntry.vb     # 資産詳細入力画面
│       ├── FrmFlexROUAsset.vb         # 使用権資産画面（プレースホルダ）
│       ├── FrmFlexMonthlyPayments.vb  # 月次支払画面（プレースホルダ）
│       ├── FrmFlexMonthlyAccounting.vb # 月次会計画面（プレースホルダ）
│       ├── FrmFlexPeriodBalance.vb    # 期間残高画面（プレースホルダ）
│       ├── FrmFlexTaxAdjustment.vb    # 税法調整画面（プレースホルダ）
│       ├── PopupBaseForm.vb           # ポップアップ基底クラス
│       └── LeaseM4BS.TestWinForms.vbproj
└── docs/                              # ドキュメント（docs/project-documentationブランチ上）
    ├── 01_要件定義書.md
    ├── 02_基本設計書.md
    └── 03_開発進捗管理表.md
```

### 主要な依存ライブラリ
- **Npgsql 6.0.11**: PostgreSQL用.NET Dataプロバイダー
- **Microsoft.VisualStudio.TestTools.UnitTesting**: MSTestフレームワーク

---

## 2. アーキテクチャ概要

### アーキテクチャパターン
- **UIパターン**: WinForms + Code-Behind（イベント駆動型）
- **データアクセスパターン**: DAO的Helper Classパターン（`CrudHelper` + `DbConnectionManager`）
- **旧システムとの対応**: Access DAO → PostgreSQL + Npgsql への移行途上

### レイヤー構成と責務

| レイヤー | プロジェクト | 責務 |
|---|---|---|
| プレゼンテーション層 | `LeaseM4BS.TestWinForms` | WinForms画面、ユーザーインタラクション、ビジネスロジックの一部（計算）|
| データアクセス層 | `LeaseM4BS.DataAccess` | PostgreSQL接続管理、CRUD操作の抽象化 |
| テスト層 | `LeaseM4BS.Tests` | MSTestによる単体テスト |

---

## 3. 画面・機能一覧

### 実装済み画面

| クラス名 | 画面名 | 実装状況 | 説明 |
|---|---|---|---|
| `FrmFlexMenu` | フレックスメニュー画面 | 実装済み | ポータル画面。メニューバーから子画面を切り替え |
| `FrmFlexContract` | 契約書（フレックス）一覧 | 実装済み | 契約一覧の検索・表示、契約詳細への遷移 |
| `FrmLeaseContractMain` | リース契約詳細入力画面 | 実装済み（5タブ） | リース契約の詳細情報入力・計算のメイン画面 |
| `FrmAssetDetailEntry` | 資産詳細入力画面 | 実装済み | 資産情報の詳細入力（FrmLeaseContractMainから呼び出し）|

### プレースホルダ画面（未実装）

| クラス名 | 画面名 | 実装状況 | 説明 |
|---|---|---|---|
| `FrmFlexROUAsset` | 使用権資産画面 | プレースホルダのみ | `InitializeComponent()`のみ実装 |
| `FrmFlexMonthlyPayments` | 月次支払画面 | プレースホルダのみ | `InitializeComponent()`のみ実装 |
| `FrmFlexMonthlyAccounting` | 月次会計画面 | プレースホルダのみ | `InitializeComponent()`のみ実装 |
| `FrmFlexPeriodBalance` | 期間残高画面 | プレースホルダのみ | `InitializeComponent()`のみ実装 |
| `FrmFlexTaxAdjustment` | 税法調整画面 | プレースホルダのみ | `InitializeComponent()`のみ実装 |

### FrmLeaseContractMain のタブ構成

| タブ名 | 定数名 | 実装状況 | 主な内容 |
|---|---|---|---|
| 契約 | `pgContract` | 実装済み | 基本・管理情報、資産情報（DataGridView）、契約条件（更新・解約・購入オプション） |
| 初回金 | `pgInitial` | 実装済み | 初期直接費用、原状回復費用見積、リース・インセンティブ |
| 会計 | `pgAccounting` | 実装済み（今回改修対象） | 現契約期間、現支払情報、会計期間、返済スケジュールマトリックス、変更履歴 |
| 転貸 | `pgSublease` | 実装済み | サブリース情報、転借人情報、転貸収入グリッド |
| リース判定 | `pgJudgment` | 実装済み | Q1-Q4判定、免除規定、結果表示 |

### FrmFlexMenu のメニューボタン

| ボタン名 | 遷移先 | 実装状況 |
|---|---|---|
| `btnContract` | `FrmFlexContract` | 実装済み |
| `btnROUAsset` | `FrmFlexROUAsset` | プレースホルダ |
| `btnMonthlyPayments` | `FrmFlexMonthlyPayments` | プレースホルダ |
| `btnMonthlyAccounting` | `FrmFlexMonthlyAccounting` | プレースホルダ |
| `btnPeriodBalance` | `FrmFlexPeriodBalance` | プレースホルダ |
| `btnTaxAdjustment` | `FrmFlexTaxAdjustment` | プレースホルダ |

---

## 4. 使用パターン

### 状態管理
- WinForms標準のイベント駆動型
- `_isLoaded` フラグで初期化完了を管理（不要なイベント発火を防止）
- `Private Shared _xxxCounter As Integer` で採番カウンタを管理（静的変数）

### ルーティング（画面遷移）
- `FrmFlexMenu.SwitchContent(menuButton)` で子画面（UserControl）を動的に切り替え
- `FrmLeaseContractMain` は `FrmFlexContract` 上のボタンから新規インスタンスとして起動
- `FrmAssetDetailEntry` は `FrmLeaseContractMain` からポップアップとして起動（`PopupBaseForm` 継承）

### データアクセス
```vb
' CrudHelperパターン
Using helper As New CrudHelper()
    Dim dt As DataTable = helper.GetDataTable("SELECT ...", parameters)
    helper.Insert("table_name", values)
    helper.ExecuteNonQuery("UPDATE ...", parameters)
End Using

' DbConnectionManagerパターン（低レベル）
Dim connMgr As New DbConnectionManager()
Using conn As NpgsqlConnection = connMgr.GetConnection()
    Dim cmd As New NpgsqlCommand(sql, conn)
    ' ...
End Using
```

接続文字列の優先順位:
1. `App.config` の `connectionStrings["LeaseM4BS"]`
2. 環境変数 `LEASE_M4BS_CONNECTION_STRING`
3. デフォルト値（localhost:5432, lease_m4bs DB）

### エラーハンドリング
- `Try/Catch ex As Exception` による例外補足
- エラーメッセージは `MessageBox.Show()` でユーザーに表示
- デバッグ出力は `System.Diagnostics.Debug.WriteLine()` を使用
- データアクセス層では詳細メッセージを構築して再スロー

### テストパターン
- MSTest (`<TestClass>`, `<TestMethod>` 属性)
- `<TestInitialize>` / `<TestCleanup>` でテストデータのクリーンアップ
- テスト対象: `tw_m_user` テーブルへのCRUD操作
- 現時点でUI画面のテストはなし（DataAccessのみ）

---

## 5. 会計タブ（今回の改修対象）詳細

### 会計タブの構成セクション

| セクション | メソッド | 内容 |
|---|---|---|
| 現契約期間 + 現支払情報 | `BuildAccSchTopRowSection()` | 読み取り専用表示（契約タブから自動転記） |
| 会計期間 | `BuildAccSchAccountingSection()` | 会計期間・金額の表形式入力、返済スケジュールマトリックス |
| 変更履歴 | `BuildAccChangeHistorySection()` | DataGridViewによる変更履歴表示 |

### 会計タブの自動計算ロジック（`UpdateAccountingTabValues`）

企業会計基準第34号（ASBJ第34号）に基づく計算:

| 計算項目 | 根拠条文 | 計算式 |
|---|---|---|
| 会計期間 | 第34号§17（延長オプション合理的確実性） | 基本期間(月) + 更新予想回数 × 更新月数 |
| 賃料総額 | - | 月額賃料 × 基本期間月数 |
| 算定総額 | 第34号§22 | 月額賃料 × 会計期間月数 |
| リース・非リース配分 | 第34号§13（構成要素の区分） | 算定総額 × リース割合 / (リース割合 + 非リース割合) |
| 現在価値（PV） | 第34号§22（年金現価公式） | 月額支払 × (1-(1+r)^-n)/r |
| 使用権資産 | 第34号§27 | PV + 初期直接費用 + 原状回復費用 - リース・インセンティブ |
| リース負債 | 第34号§22 | PV |

### 返済スケジュールマトリックス項目

- **使用権資産（ROU）**: 期首・増加・変更増減・減少・期末
- **リース負債（Liab）**: 期首・増加・変更増減・減少・期末
- **原状回復引当（ARO）**: 期首・増加・変更増減・減少・期末

---

## 6. 既存の類似機能分析

### 再利用可能なヘルパーメソッド（FrmLeaseContractMain内）

| メソッド名 | 用途 |
|---|---|
| `CreateSection(text)` | セクション用GroupBox生成 |
| `CreateFieldLabel(text)` | フィールドラベル生成 |
| `CreateGridLabel(text)` | グリッドラベル生成 |
| `AddFieldRow(tbl, lbl1, ctrl1, lbl2, ctrl2)` | TableLayoutPanelへのフィールド行追加 |
| `CalcMonthsBetween(startDt, endDt)` | 2日付間の月数計算（端数補正あり） |
| `RecalcAll()` | 全タブの再計算トリガー |

### データ採番パターン（自動採番）
```vb
' 静的カウンタによる採番
Private Shared _contractCounter As Integer = 0
_contractCounter += 1
txtContractNo.Text = $"LC-{DateTime.Now.Year}-{_contractCounter:D4}"
```

---

## 7. 技術的制約・注意事項

### 依存関係の制約
- .NET Framework 4.7.2（.NET Core / .NET 5+ ではない）
- Npgsql 6.0.11 固定（NuGet packages.config 管理）
- Access DBからの移行途上（旧コードへの参照コメントあり: "DAO.Recordset の代替"）

### 実装上の注意点
- `FrmFlexROUAsset`, `FrmFlexMonthlyPayments`, `FrmFlexMonthlyAccounting`, `FrmFlexPeriodBalance`, `FrmFlexTaxAdjustment` はプレースホルダのみで**未実装**
- `FrmLeaseContractMain` のヘッダーボタン（登録・修正・更新・中途解約・削除）は`"この機能は現在実装中です。"` メッセージのみ（**未実装**）
- サンプルデータはハードコーディングされており、DBとの実際のCRUDは未接続（`LoadSampleData()`）
- ARO（原状回復引当）計算ロジックは返済スケジュールマトリックスに列はあるが詳細計算は未実装

### パフォーマンス上の考慮点
- `FrmLeaseContractMain` はコードオンリーでUIを構築（Designerなし）、ファイルサイズが大きい（38000トークン超）
- `RecalcAll()` は複数タブにわたる再計算を行うため、入力イベントのたびに発火する設計

---

## 8. 命名規則・コーディング規約

### ファイル命名規則
- フォームファイル: `Frm[機能名].vb` + `Frm[機能名].Designer.vb`
- クラスファイル: `[クラス名].vb`
- デザイナーファイル: `[クラス名].Designer.vb`

### コントロール命名規則
| プレフィックス | コントロール種別 |
|---|---|
| `txt` | TextBox |
| `lbl` | Label |
| `btn` | Button |
| `cmb` | ComboBox |
| `dtp` | DateTimePicker |
| `num` | NumericUpDown |
| `dgv` | DataGridView |
| `grp` | GroupBox |
| `chk` | CheckBox |
| `rb` | RadioButton |
| `pnl` | Panel |
| `pgXxx` | TabPage |
| `tbl` | TableLayoutPanel |
| `sch` | スケジュール（会計タブ内コントロールのプレフィックス） |

### クラス・メソッド命名規則
- クラス名: PascalCase（例: `CrudHelper`, `DbConnectionManager`）
- メソッド名: PascalCase（例: `GetDataTable`, `ExecuteNonQuery`）
- プライベートフィールド: アンダースコアプレフィックス + camelCase（例: `_isLoaded`, `_activeButton`）
- イベントハンドラ: `On[動詞][対象]` 形式（例: `OnAssetSearchClick`）、または `[対象]_[イベント名]`

### コメント・ドキュメント規約
- クラス・Public/Privateメソッドに XML コメント (`'''`) を使用
- インラインコメントは日本語
- 設計根拠は条文番号付きでコメント記載（例: `' 第34号§22 リース負債の当初測定`）

---

## 9. Git・ブランチ戦略

### 確認されたブランチ一覧（リモート含む）

| ブランチ名 | 用途 |
|---|---|
| `main` | メインブランチ |
| `docs/project-documentation` | ドキュメントブランチ（要件定義書等） |
| `devin/1772429481-accounting-tab-redesign` | 会計タブ改修（現在作業中） |
| `feature/accounting-tab-revision` | 会計タブ改修（別ブランチ） |
| `feature/add-test-tab` | テストタブ追加 |
| `feature/remove-lease-judgment-items` | リース判定項目削除 |
| `devin/1771914520-asbj34-judgment-migrate` | ASBJ34号判定画面移行 |
| `devin/1771302100-chuki-recalc-update` | 中途解約再計算更新 |

---

## 10. docs/project-documentationブランチのドキュメント分析結果（間接情報）

### Gitログからの確認事項
- コミット: `b5e0bfafef714a4ee67d148982044c7ed6389537`
- コミット日時: 2025年5月7日
- コミットメッセージ: `docs: プロジェクト資料を追加（要件定義書・基本設計書・開発進捗管理表）`
- 対象ドキュメント3件が一括追加された（単一コミット）

### アクセス制約について
docs/project-documentationブランチのGitオブジェクト（コミット・ツリー・ブロブ）はzlib圧縮バイナリ形式で格納されており、本調査ツール（ファイルシステム読み込み）では直接内容を取得できませんでした。

ドキュメントの内容を確認するには、以下のコマンドを実行する必要があります：

```bash
git show "docs/project-documentation:docs/01_要件定義書.md"
git show "docs/project-documentation:docs/02_基本設計書.md"
git show "docs/project-documentation:docs/03_開発進捗管理表.md"
```

### ソースコードから推定されるドキュメントのカバー範囲

コードベースの状況から、ドキュメントには以下が記載されていると推定されます：

**01_要件定義書**（推定カバー範囲）
- 企業会計基準第34号（ASBJ第34号）への対応要件
- リース契約管理の業務要件
- リース判定（Q1-Q4判定ロジック）の要件
- 免除規定（短期・少額）の要件
- 使用権資産・リース負債の当初認識計算要件

**02_基本設計書**（推定カバー範囲）
- WinForms画面設計
- タブ構成（契約・初回金・会計・転貸・リース判定）
- データベース設計（PostgreSQL、tw_M_USERテーブル等）
- データアクセス層の設計（CrudHelper・DbConnectionManager）

**03_開発進捗管理表**（推定カバー範囲）
- 各画面・機能の実装状況（プレースホルダ5画面を含む未完成機能）
- ブランチ別の開発タスク進捗

**未実装・未記載の可能性が高い項目**（コードベースから判断）
- `FrmFlexROUAsset`（使用権資産画面）の詳細設計
- `FrmFlexMonthlyPayments`（月次支払画面）の詳細設計
- `FrmFlexMonthlyAccounting`（月次会計画面）の詳細設計
- `FrmFlexPeriodBalance`（期間残高画面）の詳細設計
- `FrmFlexTaxAdjustment`（税法調整画面）の詳細設計
- DBへの実際のCRUD接続実装
- ヘッダーボタン機能（登録・修正・更新・中途解約・削除）の実装
