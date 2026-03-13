# DB実接続（CRUD）実装計画書: LeaseM4BS

**ドキュメント番号**: IMPL-2026-001
**バージョン**: 1.0
**作成日**: 2026年3月12日
**対象読者**: LeaseM4BS-1 リポジトリの開発担当者（VB.NET・PostgreSQL 知識を前提とする）

**前提ドキュメント**:
- 01_要件定義書_v2.md（REQ-2026-002）
- 02_基本設計書_v2.md（BD-2026-002）
- 05_DB設計書.md（DB-2026-001）
- 09_コーディング規約書.md（CS-2026-001）

---

## 目次

1. [目的・対象読者](#1-目的対象読者)
2. [現状分析](#2-現状分析)
3. [DB設計と画面のマッピング](#3-db設計と画面のマッピング)
4. [実装方針](#4-実装方針)
5. [画面別実装計画（優先度順）](#5-画面別実装計画優先度順)
6. [段階的移行戦略](#6-段階的移行戦略)
7. [テスト計画](#7-テスト計画)
8. [スケジュール・マイルストーン](#8-スケジュールマイルストーン)
9. [コーディング規約・命名規則](#9-コーディング規約命名規則)
10. [付録](#10-付録)

---

## 1. 目的・対象読者

### 1.1 本ドキュメントの目的

本ドキュメントは、LeaseM4BS プロジェクトにおいて PostgreSQL への実接続（CRUD）を安全かつ秩序立てて実装するための、**実装者向け技術ガイド**である。

開発進捗管理表（`03_開発進捗管理表_v2.md`）は DB 実接続を「ISS-001（未着手）」と位置づけているが、実際のコードベースを調査した結果、**一覧画面の読み取り（SELECT）処理は既に CrudHelper 経由で DB 実接続済み**であることが判明した。本書はこの実態を踏まえ、真に未実装である機能—すなわち **FrmLeaseContractMain における既存データ読み込み（編集・照会モード）、ヘッダーボタンの DB 書き込み処理（INSERT/UPDATE/DELETE）、および 3 つの未定義テーブルの DDL 整備**—を対象として、実装の指針を提供する。

本書が示す内容は以下の 3 点である。

1. **何を実装するか**: 未完成の機能を画面・テーブル単位で明確化する（現状分析・画面マッピング）
2. **どの順番で実装するか**: 依存関係と優先度に基づく実装フェーズを定める（画面別実装計画）
3. **どのように実装するか**: 既存コードパターン（CrudHelper、Using 句、トランザクション）への準拠方法を具体的なコード例で示す（実装方針）

本書は「何を作るか」を定める設計書（05_DB設計書.md 等）とは性格が異なり、実装者が手を動かす際の**手順書・照合チェックリスト**として機能することを目的とする。

### 1.2 対象読者と前提知識

**対象読者**: LeaseM4BS-1 リポジトリの開発担当者

**前提知識**:

| 技術領域 | バージョン・詳細 |
|---|---|
| VB.NET | .NET Framework 4.7.2 |
| Windows Forms | .NET Framework 付属 WinForms |
| PostgreSQL / Npgsql | Npgsql 6.0.11（packages.config 記載） |
| MSTest | v2.2.8（テストプロジェクト参照） |
| CrudHelper | プロジェクト固有の汎用 CRUD ヘルパークラス（LeaseM4BS.DataAccess） |

**参照すべき先行ドキュメント**:

| ドキュメント | 番号 | ファイルパス |
|---|---|---|
| 要件定義書 | REQ-2026-002 | `docs/project-documentation/01_要件定義書_v2.md` |
| 基本設計書 | BD-2026-002 | `docs/project-documentation/02_基本設計書_v2.md` |
| DB 設計書 | DB-2026-001 | `docs/project-documentation/05_DB設計書.md` |
| コーディング規約書 | CS-2026-001 | `docs/project-documentation/09_コーディング規約書.md` |
| 開発進捗管理表 | PM-2026-002 | `docs/project-documentation/03_開発進捗管理表_v2.md` |

### 1.3 ドキュメント構成

本書は以下の流れで構成されており、実装フェーズに応じて参照するセクションを選択できる。

```text
1. 目的・対象読者          ← 本セクション（位置づけの確認）
2. 現状分析                ← 実装前に必読（何が済みで何が残っているか）
3. DB設計と画面のマッピング ← 実装対象テーブルの確認
4. 実装方針                ← CrudHelper 使用パターン・トランザクション・エラー処理
5. 画面別実装計画          ← フェーズ別の具体的な実装手順
6. 段階的移行戦略          ← Feature Flag を用いた安全な切り替え方法
7. テスト計画              ← MSTest によるユニットテストと手動結合テスト
8. スケジュール・マイルストーン ← 目安期間とリスク
9. コーディング規約・命名規則 ← 命名・SQL 記述・削除タイミング
10. 付録                   ← CrudHelper 早見表・PostgreSQL エラーコード
```

---

## 2. 現状分析

### 2.1 「LoadSampleData()」に関する重要な発見

開発進捗管理表（`ISS-001`）および `03_開発進捗管理表_v2.md §2.5` では、DB 実接続の未着手状態を「`LoadSampleData()` のハードコード状態」と表現している。しかし実際のコードベースを全画面調査した結果、**`LoadSampleData()` という名称のメソッドはいずれのファイルにも存在しない**ことが判明した。

実態は以下の 2 層に整理される。

**【一覧画面（参照系）— 既に DB 実接続済み】**

`FrmFlexContract`、`FrmFlexROUAsset`、`FrmFlexMonthlyPayments`、`FrmFlexMonthlyAccounting`、`FrmFlexPeriodBalance`、`FrmFlexTaxAdjustment` の各一覧画面は、すでに `CrudHelper.GetDataTable()` を直接呼び出す形で PostgreSQL への SELECT が実装されている。各画面の `LoadData()` または `LoadCtbData()` メソッドが該当する（詳細は §2.2 参照）。

**【契約詳細画面（CRUD）— 未実装が残る】**

`FrmLeaseContractMain`（リース契約詳細・5タブ構成）は新規登録（INSERT）の一部が実装されているが、以下が未実装である。

- 既存データの読み込み（編集・照会モード時のデータ復元）
- ヘッダーボタン全般（登録・修正・変更・更新・満了解約・中途解約・削除）の DB 書き込み
- `FrmAssetDetailEntry`（資産詳細ポップアップ）の DB 保存

加えて、3 つの一覧画面が参照するテーブルが DB 設計書（v2.1）に未定義であり、DDL の整備も必要である（§2.3 参照）。

現在の採番は `FrmFlexContract.GetNextContractNo()` によるメモリ内 CTB ストア参照で行われており、DB シーケンスとの同期が保証されない問題がある。この採番も DB シーケンスへの移行対象である（§4.6 参照）。

### 2.2 既存 DataAccess 基盤の棚卸し

`LeaseM4BS.DataAccess` プロジェクトには、以下のクラスが完成済みであり、本実装で即座に再利用できる。

#### CrudHelper.vb — 汎用 CRUD ヘルパー（完成済み）

DAO.Recordset の代替として設計された汎用クラス。`IDisposable` を実装しており、`Using` ブロックでの使用を前提とする。

| メソッド | シグネチャ | 役割 |
|---|---|---|
| GetDataTable | `(sql As String, Optional parameters As List(Of NpgsqlParameter)) As DataTable` | SELECT → DataTable |
| ExecuteNonQuery | `(sql As String, Optional parameters As List(Of NpgsqlParameter)) As Integer` | INSERT/UPDATE/DELETE |
| ExecuteScalar(Of T) | `(sql As String, Optional parameters As List(Of NpgsqlParameter)) As T` | 単一値取得（NULL 安全） |
| SafeConvert(Of T) | `(value As Object, Optional defaultValue As T) As T` | DB 値の安全型変換 |
| Insert | `(tableName As String, columnValues As Dictionary(Of String, Object)) As Integer` | 辞書型 INSERT |
| Update | `(tableName As String, columnValues As Dictionary(Of String, Object), whereClause As String, Optional whereParameters As List(Of NpgsqlParameter)) As Integer` | 辞書型 UPDATE（WHERE 必須） |
| Delete | `(tableName As String, whereClause As String, Optional whereParameters As List(Of NpgsqlParameter)) As Integer` | DELETE（WHERE 必須） |
| Exists | `(tableName As String, whereClause As String, Optional whereParameters As List(Of NpgsqlParameter)) As Boolean` | レコード存在確認 |
| BeginTransaction | `()` | トランザクション開始 |
| Commit | `()` | コミット |
| Rollback | `()` | ロールバック |

> **シグネチャ注意**: `GetDataTable` の第 2 引数は `List(Of NpgsqlParameter)` であり、`Dictionary(Of String, Object)` ではない。既存コードの呼び出しパターンを必ず確認すること。

**実装上の重要な特性**:

- WHERE 句なしの UPDATE / DELETE は引数バリデーションで防止されている（全テーブル誤更新の防護）
- パラメータは内部で `Clone()` されるため、同一パラメータリストを複数回渡しても安全
- トランザクション中は既存の接続を再利用するため、`BeginTransaction` 後の同一 `CrudHelper` インスタンスに対して複数の INSERT/UPDATE を発行できる

#### DbConnectionManager.vb — 接続文字列管理（完成済み）

接続文字列の優先順位:

1. `App.config` の `connectionStrings["LeaseM4BS"]`（現在 `Database=lease_new` が設定されている）
2. 環境変数 `LEASE_M4BS_CONNECTION_STRING`
3. デフォルトフォールバック: `Host=localhost;Port=5432;Database=lease_m4bs;Username=manager;Password=pass`

> **注意**: `App.config` の DB 名（`lease_new`）と DB 設計書・`init.sql` の DB 名（`lease_m4bs`）が一致していない。開発環境の実際の DB 名を確認し、必要に応じて `App.config` を修正すること。

#### その他の再利用可能コンポーネント

| クラス | ファイル | 再利用可能な機能 |
|---|---|---|
| MasterDataLoader | `LeaseM4BS.DataAccess/MasterDataLoader.vb` | コンボボックス用マスタデータ取得（10 マスタ対応済み） |
| LeaseContractRepository | `LeaseM4BS.DataAccess/LeaseContractRepository.vb` | `tw_lease_contract` の GetAll / GetByContractNo / Insert / Update / Delete |
| LeaseAccountingRepository | `LeaseM4BS.DataAccess/LeaseAccountingRepository.vb` | `tw_lease_accounting` の GetByContractId / Upsert |
| CtbRepository | `LeaseM4BS.TestWinForms/CtbRepository.vb` | CTB レコード一括 INSERT・全件取得（RETURNING ctb_id 使用） |
| AuthorizationService | `LeaseM4BS.DataAccess/AuthorizationService.vb` | CrudHelper を使用した DB 認証の実装済み参考例 |

### 2.3 DB テーブルの整備状況

`lease_m4bs` データベースのテーブル構成は以下の通りである。

| 種別 | プレフィックス | テーブル数 | 主要テーブル |
|---|---|---|---|
| マスタ | `m_` | 10 | m_company, m_supplier, m_department, m_asset_category, m_contract_type, m_initial_cost_item, m_acct_treatment, m_monthly_item, m_payment_method, m_bank_account |
| トランザクション | `tw_` | 11 | tw_lease_contract, tw_lease_property, tw_lease_party, tw_lease_schedule, tw_lease_accounting, tw_lease_judgment, tw_lease_initial, tw_lease_sublease, tw_lease_payment_actual, tw_lease_journal + 1 |
| 連結 | `ctb_` | 3 | ctb_lease_integrated, ctb_dept_allocation, ctb_remeasurement_history |
| Access 移行 | — | 2 | d_asset, tw_m_user（SQL スクリプト管理外） |
| **合計** | | **26** | |

SQL スクリプトの実行順序（DB 構築時に遵守すること）:

```text
1. sql/init.sql            -- DB・ユーザー作成（psql -U postgres で実行）
2. sql/master_tables.sql   -- マスタテーブル 10 件
3. sql/tw_tables.sql       -- トランザクションテーブル 11 件 + インデックス
4. sql/ctb_tables.sql      -- 連結テーブル 3 件 + インデックス
5. sql/seed_data.sql       -- 初期シードデータ
6. sql/seed_test_users.sql -- テストユーザーデータ（必要な場合）
```

**画面実装に必要なテーブルの構築確認チェックリスト**:

- [ ] `tw_lease_contract` — 契約詳細 CRUD の最重要テーブル
- [ ] `tw_lease_accounting` — 会計計算結果の保存先（UNIQUE 制約あり）
- [ ] `tw_lease_judgment` — リース判定結果の保存先
- [ ] `tw_lease_initial` — 初回費用（費用項目ごとに複数行の正規化設計）
- [ ] `tw_lease_sublease` — 転貸情報
- [ ] `m_` 系 10 テーブル — コンボボックス選択肢の供給元
- [ ] `d_asset` — Access 移行テーブル（`sql/tw_tables.sql` 管理外のため要別途確認）

**3 つの未定義テーブルへの対応が必要（重要）**:

以下のテーブルは画面コードで参照されているが、`05_DB設計書.md`（v2.1）に定義が存在しない。`sql/tw_tables.sql` を確認し、DDL が含まれていない場合は本実装計画の一環として DDL を作成する必要がある。

| テーブル | 参照画面 | 状態 |
|---|---|---|
| `tw_lease_period_balance` | FrmFlexPeriodBalance.vb（LoadData 内） | DB 設計書未定義 |
| `tw_lease_balance_breakdown` | FrmFlexPeriodBalance.vb（LoadContractBreakdown 内） | DB 設計書未定義 |
| `tw_lease_tax_adjustment` | FrmFlexTaxAdjustment.vb（LoadData 内） | DB 設計書未定義 |

### 2.4 未着手機能の整理（進捗管理表との照合）

| 課題 ID | 表の記述 | 実態（コードベース調査結果） | 本計画での対応 |
|---|---|---|---|
| ISS-001 | DB 実接続（全画面）「未着手」 | 一覧画面の SELECT は実装済み。詳細画面の INSERT/UPDATE/DELETE が未実装 | Phase A〜C で対応 |
| ISS-002 | ヘッダーボタン全般「実装中」メッセージのみ | 登録・修正・変更・更新・満了解約・中途解約・削除の全ボタンで `MessageBox.Show("実装中")` のみ | Phase C で対応（ISS-001 完了後） |
| ISS-003 | `grpParty` / `pnlResult` VB コード未実装 | pgJudgment タブ内の 2 コンポーネントが UI 未実装 | Phase B の前提条件（本計画スコープ外） |
| ISS-004 | ARO 詳細計算未実装 | マトリックス列はあるが計算ロジック未実装 | Phase B（pgAccounting タブ）で部分対応 |
| ISS-005〜007 | プレースホルダ画面 5 件 | `InitializeComponent()` のみ（LoadData は実装済みのケースあり） | Phase D（別途計画） |

**優先度の根拠**: DB 実接続（ISS-001）はヘッダーボタン実装（ISS-002）の前提条件であり、どちらも「高」優先度と位置づけられている。また `RSK-004`（ハードコードされたサンプルデータの本番移行漏れ）リスクへの直接対処として、本実装計画が必要である。

---

## 3. DB設計と画面のマッピング

### 3.1 テーブル-画面対応マップ

以下に LeaseM4BS の全テーブルと対応画面・操作種別の対応関係を示す。「操作」欄は C=INSERT、R=SELECT、U=UPDATE、D=DELETE を表す。

| テーブル | 種別 | 主キー | 主な対応画面 | 対応タブ/セクション | 操作 | 備考 |
|---|---|---|---|---|---|---|
| d_asset | Access 移行 | (Access 管理) | （現行コードで直接参照なし） | — | R | DB 設計書記載。LeaseContractRepository.SearchContracts() が参照想定。現行 FrmFlexContract は CTB テーブル経由 |
| tw_m_user | Access 移行 | user_id SERIAL | 認証系 | FrmLogin | CRUD | UserCrudTests.vb で実装済みパターンあり |
| **tw_lease_contract** | tw_ | contract_id SERIAL | FrmFlexContract / FrmLeaseContractMain | pgContract | **CRUD** | **最重要テーブル。UNIQUE(contract_no)** |
| tw_lease_property | tw_ | property_id SERIAL | FrmLeaseContractMain | pgContract（物件属性） | CRUD | FK: contract_id |
| tw_lease_party | tw_ | party_id SERIAL | FrmLeaseContractMain | pgJudgment（grpParty）※ | CRUD | FK: contract_id ※grpParty 未実装 |
| tw_lease_schedule | tw_ | schedule_id SERIAL | FrmLeaseContractMain | pgJudgment（dgvSchedule） | CRUD | FK: contract_id |
| **tw_lease_accounting** | tw_ | accounting_id SERIAL | FrmLeaseContractMain | pgAccounting | **CRUD** | FK: contract_id、UNIQUE(contract_id) |
| tw_lease_judgment | tw_ | judgment_id SERIAL | FrmLeaseContractMain | pgJudgment（Q1-Q4） | CRUD | FK: contract_id、UNIQUE(contract_id) |
| tw_lease_initial | tw_ | initial_id SERIAL | FrmLeaseContractMain | pgInitial | CRUD | FK: contract_id、費用項目ごとに複数行 |
| tw_lease_sublease | tw_ | sublease_id SERIAL | FrmLeaseContractMain | pgSublease | CRUD | FK: contract_id |
| tw_lease_payment_actual | tw_ | payment_actual_id SERIAL | FrmFlexMonthlyPayments | 一覧 | CRUD | FK: contract_id（Phase D） |
| tw_lease_journal | tw_ | journal_id SERIAL | FrmFlexMonthlyAccounting | 一覧 | CRUD | FK: contract_id（Phase D） |
| tw_lease_period_balance | tw_ | (未定義) | FrmFlexPeriodBalance | 一覧 | CRUD | **DB 設計書未定義・DDL 要確認** |
| tw_lease_balance_breakdown | tw_ | (未定義) | FrmFlexPeriodBalance | 内訳 | CRUD | **DB 設計書未定義・DDL 要確認** |
| tw_lease_tax_adjustment | tw_ | (未定義) | FrmFlexTaxAdjustment | 一覧 | CRUD | **DB 設計書未定義・DDL 要確認** |
| m_company | m_ | company_cd | FrmLeaseContractMain | pgContract | R | ComboBox 選択肢 |
| m_supplier | m_ | supplier_cd | FrmLeaseContractMain | pgContract | R | ComboBox 選択肢 |
| m_department | m_ | dept_cd | FrmLeaseContractMain | pgContract | R | ComboBox 選択肢 |
| m_asset_category | m_ | asset_category_cd | FrmAssetDetailEntry | - | R | ComboBox 選択肢 |
| m_contract_type | m_ | contract_type_cd | FrmFlexContract / FrmLeaseContractMain | - | R | ComboBox 選択肢 |
| m_payment_method | m_ | payment_method_cd | FrmLeaseContractMain | pgJudgment | R | ComboBox 選択肢 |
| m_initial_cost_item | m_ | cost_item_cd | FrmLeaseContractMain | pgInitial | R | ComboBox 選択肢 |
| m_acct_treatment | m_ | acct_treatment_cd | FrmLeaseContractMain | pgInitial | R | ComboBox 選択肢 |
| m_monthly_item | m_ | monthly_item_cd | FrmLeaseContractMain | - | R | ComboBox 選択肢 |
| m_bank_account | m_ | bank_account_cd | FrmLeaseContractMain | - | R | ComboBox 選択肢 |
| ctb_lease_integrated | ctb_ | ctb_id SERIAL | FrmFlexContract | 一覧 | CRUD | M4BS-M7 新基準統合（実装済み） |
| ctb_dept_allocation | ctb_ | (ctb_id FK) | FrmFlexContract | 一覧 | CRUD | 部署配分（実装済み） |
| ctb_remeasurement_history | ctb_ | remeasurement_id SERIAL | 将来: 再測定画面 | - | CRUD | 将来対応 |

### 3.2 FrmFlexContract（契約一覧）のデータフロー

FrmFlexContract は現在、CTB テーブル（`CtbRepository.SelectAll()` 経由、実装済み）を唯一のデータソースとして使用している。`d_asset` テーブルへのアクセスは現行コードに存在せず、将来の検索機能拡張時に追加する想定である。

**FrmLeaseContractMain への遷移設計**: 「新規」ボタン押下時は `contract_id = 0` を引数として新規モードで起動。既存契約行のダブルクリック時は `contract_no` を `Tag` プロパティ経由で渡し、既存データ読み込みモードで起動する。

### 3.3 FrmLeaseContractMain（契約詳細 5 タブ）のデータフロー

**テーブル親子関係**:

```text
tw_lease_contract（親）
├── tw_lease_property    (property_id, FK: contract_id)  ← pgContract
├── tw_lease_party       (party_id, FK: contract_id)     ← pgJudgment/grpParty
├── tw_lease_schedule    (schedule_id, FK: contract_id)  ← pgJudgment/dgvSchedule
├── tw_lease_accounting  (accounting_id, FK: contract_id, UNIQUE) ← pgAccounting
├── tw_lease_judgment    (judgment_id, FK: contract_id, UNIQUE)   ← pgJudgment
├── tw_lease_initial     (initial_id, FK: contract_id)   ← pgInitial（複数行）
└── tw_lease_sublease    (sublease_id, FK: contract_id)  ← pgSublease
```

**RecalcAll() と DB 保存のタイミング**: 会計タブの計算結果は「登録」または「更新」ボタン押下時に `tw_lease_accounting` に保存する設計とする（ユーザーが明示的に確定するまで DB には書き込まない）。

### 3.4 FrmAssetDetailEntry（資産詳細）のデータフロー

`PopupBaseForm` を継承したポップアップダイアログ。OK 時のみ親画面 DataGridView を更新し、Cancel 時は変更を破棄する。

---

## 4. 実装方針

### 4.1 アーキテクチャ方針（既存設計との整合）

LeaseM4BS は「**WinForms Code-Behind + CrudHelper（DAO パターン）**」を採用。以下を遵守する。

**採用しないパターン**:
- シサンM7 の Entity-Repository / MVP パターン
- `RepositoryBase` クラス
- Dependency Injection コンテナ
- 非同期 DB 操作（`async/await`）

**維持するパターン**:
- `Using crud As New CrudHelper()` ブロックによるリソース確実解放
- `DataGridView.Rows.Add()` → `Cells["列名"].Value` による手動バインド

大規模な機能追加は `ContractDataHelper.vb` 等の別ヘルパークラスに分離することを推奨する（§8.3 参照）。

### 4.2 CrudHelper 使用パターンの標準化

**パターン 1 — SELECT（GetDataTable）**:

```vb
Dim sql As String =
    "SELECT c.contract_id, c.contract_no, c.contract_name, " &
    "c.start_date, c.end_date " &
    "FROM tw_lease_contract c " &
    "WHERE c.contract_no = @contract_no"

Dim params As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@contract_no", contractNo)
}

Using crud As New CrudHelper()
    Dim dt As DataTable = crud.GetDataTable(sql, params)
    If dt.Rows.Count > 0 Then
        Dim row As DataRow = dt.Rows(0)
        txtContractName.Text = crud.SafeConvert(Of String)(row("contract_name"), "")
    End If
End Using
```

**パターン 2 — INSERT（辞書型）**:

```vb
Dim columnValues As New Dictionary(Of String, Object) From {
    {"contract_no", txtContractNo.Text.Trim()},
    {"contract_name", txtContractName.Text.Trim()},
    {"start_date", dtpStartDate.Value.Date},
    {"end_date", dtpEndDate.Value.Date},
    {"contract_months", CInt(txtContractMonths.Text)},
    {"created_at", DateTime.Now}
}

Using crud As New CrudHelper()
    Dim affected As Integer = crud.Insert("tw_lease_contract", columnValues)
End Using
```

**パターン 3 — UPDATE（辞書型）**:

```vb
Dim columnValues As New Dictionary(Of String, Object) From {
    {"contract_name", txtContractName.Text.Trim()},
    {"updated_at", DateTime.Now}
}
Dim whereParams As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@where_contract_id", _currentContractId)
}

Using crud As New CrudHelper()
    crud.Update("tw_lease_contract", columnValues,
        "contract_id = @where_contract_id", whereParams)
End Using
```

**パターン 4 — DELETE**:

```vb
Dim whereParams As New List(Of NpgsqlParameter) From {
    New NpgsqlParameter("@where_contract_id", contractId)
}
Using crud As New CrudHelper()
    crud.Delete("tw_lease_contract", "contract_id = @where_contract_id", whereParams)
End Using
```

**パターン 5 — EXISTS（重複確認）**:

```vb
Using crud As New CrudHelper()
    If crud.Exists("tw_lease_contract", "contract_no = @contract_no",
        New List(Of NpgsqlParameter) From {New NpgsqlParameter("@contract_no", contractNo)}) Then
        MessageBox.Show("この契約番号は既に登録されています。", "重複エラー",
            MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return
    End If
End Using
```

**パターン 6 — ExecuteScalar（RETURNING で主キー取得）**:

```vb
Dim sql As String =
    "INSERT INTO tw_lease_contract (contract_no, contract_name, start_date, end_date, contract_months) " &
    "VALUES (@contract_no, @contract_name, @start_date, @end_date, @contract_months) " &
    "RETURNING contract_id"

Using crud As New CrudHelper()
    Dim newId As Integer = crud.ExecuteScalar(Of Integer)(sql, params)
    _currentContractId = newId
End Using
```

### 4.3 トランザクション管理方針

**原則: ヘッダーボタン 1 回のクリック = 1 トランザクション**

```vb
Using crud As New CrudHelper()
    crud.BeginTransaction()
    Try
        Dim contractId As Integer = InsertContract(crud)
        InsertAccounting(crud, contractId)
        InsertJudgment(crud, contractId)
        crud.Commit()
    Catch ex As Exception
        crud.Rollback()
        HandleDbError(ex)
    End Try
End Using
```

### 4.4 エラーハンドリング方針

LeaseM4BS には `DataAccessException` は存在しない。`NpgsqlException.SqlState` で PostgreSQL エラーコードを判別する。

```vb
Catch ex As Exception
    Dim pgEx As Npgsql.NpgsqlException = TryCast(ex.InnerException, Npgsql.NpgsqlException)
    If pgEx IsNot Nothing Then
        Select Case pgEx.SqlState
            Case "23505" : MessageBox.Show("この契約番号は既に登録されています。", "重複エラー", ...)
            Case "23502" : MessageBox.Show("必須項目が入力されていません。", "入力エラー", ...)
            Case "23503" : MessageBox.Show("参照先のデータが存在しません。", "参照エラー", ...)
            Case Else    : MessageBox.Show($"DBエラー: {pgEx.SqlState}", "エラー", ...)
        End Select
    End If
End Try
```

### 4.5 セキュリティ要件の遵守

- **パラメータ化クエリの徹底**: 文字列連結による SQL 構築は**絶対に禁止**
- **動的 WHERE 句**: カラム名をホワイトリストで検証してから SQL に埋め込む
- **接続文字列**: `App.config` を `.gitignore` に追加するか `App.config.example` 方式を採用

### 4.6 採番方針（静的カウンタから DB シーケンスへ）

**方式 A — PostgreSQL シーケンス使用（推奨）**: `SELECT nextval('seq_contract_{year}')`
**方式 B — MAX + 1 パターン（フォールバック）**: `SELECT COALESCE(MAX(...), 0) + 1`

### 4.7 `_isLoaded` フラグと DB 読み込みタイミング

DB 読み込みは必ず `_isLoaded = True` 設定後（`Shown` イベント）に呼び出す。Feature Flag との組み合わせで安全に切り替える。

---

## 5. 画面別実装計画（優先度順）

### 5.1 優先度一覧

| 優先度 | 画面・機能 | 対象テーブル（主） | 実装フェーズ | 優先理由 |
|---|---|---|---|---|
| P1 | FrmLeaseContractMain — 既存データ読み込み | tw_lease_contract + ctb_lease_integrated | Phase A | INSERT は済んでいるが READ が未実装 |
| P2 | FrmLeaseContractMain — tw_lease_contract 直接保存 | tw_lease_contract | Phase A | 現状は CTB 経由のみ |
| P3 | FrmLeaseContractMain — ヘッダーボタン「修正・変更」 | tw_lease_contract、関連 tw_ テーブル | Phase B | P1・P2 完了後に実装可能 |
| P4 | FrmLeaseContractMain — ヘッダーボタン「更新・満了解約・中途解約・削除」 | tw_lease_contract（ステータス変更） | Phase B | P3 と並行実装 |
| P5 | 各タブ（pgInitial / pgAccounting / pgSublease / pgJudgment）の DB 読み書き | tw_lease_initial 等 | Phase B | Phase A で契約ID 確定後 |
| P6 | 未定義テーブル 3 件の DDL 確認・追加 | tw_lease_period_balance 等 | Phase A 準備 | sql/tw_tables.sql に定義済み。実 DB 適用確認が必要 |
| P7 | FrmFlexMonthlyPayments / FrmFlexMonthlyAccounting — カラム不整合修正 | tw_lease_payment_actual 等 | Phase C | 設計書との不整合解消 |
| P8 | FrmFlexPeriodBalance / FrmFlexTaxAdjustment — データ投入と動作確認 | tw_lease_period_balance 等 | Phase C | LoadData() は実装済み |

### 5.2 Phase A: FrmLeaseContractMain — 既存データ読み込み

`LoadFromDb(contractNo As String)` メソッドを新設し、`Shown` イベントから呼び出す。

```vb
Private Sub LoadFromDb(contractNo As String)
    If String.IsNullOrWhiteSpace(contractNo) Then Return
    Try
        Dim sql As String =
            "SELECT c.contract_id, c.contract_no, c.contract_name, " &
            "c.start_date, c.end_date, c.contract_months " &
            "FROM tw_lease_contract c WHERE c.contract_no = @contract_no"
        Dim params As New List(Of Npgsql.NpgsqlParameter) From {
            New Npgsql.NpgsqlParameter("@contract_no", contractNo)
        }
        Using crud As New CrudHelper()
            Dim dt As DataTable = crud.GetDataTable(sql, params)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("契約データが見つかりません。", "データなし", ...)
                Return
            End If
            Dim row As DataRow = dt.Rows(0)
            _contractId = CInt(row("contract_id"))
            txtContractNo.Text = row("contract_no").ToString()
            ' ... 各コントロールへの値セット ...
            LoadCtbAssetGrid(contractNo, crud)
        End Using
        RecalcAll()
    Catch ex As Exception
        ' エラーハンドリング（§4.4 参照）
    End Try
End Sub
```

### 5.3 Phase A: tw_lease_contract への直接 INSERT

`OnRegisterClick()` に `tw_lease_contract` への INSERT を追加。`RETURNING contract_id` で主キー取得。

### 5.4 Phase B: ヘッダーボタン（修正・変更・削除）

- **修正・変更**: `CrudHelper.Update()` で `tw_lease_contract` を UPDATE
- **削除**: トランザクション内で子テーブルを削除順序に従って明示的に DELETE した後、親テーブルを DELETE

### 5.5 Phase B: 各タブの DB 読み書き

- **pgAccounting**: `LeaseAccountingRepository.Upsert()` を使用（既実装）
- **pgJudgment**: `ON CONFLICT (contract_id) DO UPDATE` パターンで Upsert
- **pgInitial / pgSublease**: 「全削除→再 INSERT」パターン

### 5.6 Phase A 準備: 未定義テーブル 3 件の DDL 確認

`sql/tw_tables.sql` を直接確認したところ、3 件全てのテーブル DDL が既に定義済みであることが確認された（DB 設計書の記述漏れ）。実 DB への適用確認が必要。

```bash
psql -U manager -d lease_new -c "\dt tw_lease_period_balance"
```

### 5.7 Phase C: 月次画面の動作確認

FrmFlexMonthlyPayments / FrmFlexMonthlyAccounting の `LoadData()` は DB 実接続済み。テストデータ投入後に表示を目視確認する。

### 5.8 Phase D: プレースホルダ画面の位置づけ

Phase D は本計画スコープ外。CTB 連携テーブルおよび M7 統合機能は別途策定する。

---

## 6. 段階的移行戦略

### 6.1 移行の基本方針

1. **画面単位・ボタン単位の段階的置換**
2. **Feature Flag による安全な切り替え**（`App.config` の設定値で制御）
3. **後戻り手順の明示**（各ステップでフラグを `false` に戻す手順を事前確認）
4. **テスト DB での先行検証**

### 6.2 Feature Flag 設計

```xml
<appSettings>
    <add key="UseDatabase_ContractWrite"   value="false" />
    <add key="UseDatabase_ContractRead"    value="false" />
    <add key="UseDatabase_HeaderButtons"   value="false" />
    <add key="UseDatabase_TabData"         value="false" />
</appSettings>
```

`DbFeatureFlag.vb` を `LeaseM4BS.DataAccess` プロジェクトに新規作成し、各フラグを `ConfigurationManager.AppSettings` から取得する静的プロパティとして実装する。

### 6.3 ステップ別移行手順

| Step | 内容 | Feature Flag | 後戻り手順 |
|---|---|---|---|
| 0 | 準備（DB名確認・DDL適用・DbFeatureFlag作成・接続テスト） | — | — |
| 1 | FrmLeaseContractMain 既存データ読み込み | `ContractRead=true` | フラグを `false` に戻す |
| 2 | tw_lease_contract INSERT | `ContractWrite=true` | フラグを `false` + 不要レコード DELETE |
| 3 | ヘッダーボタン実装 | `HeaderButtons=true` | フラグを `false` で「実装中」メッセージに戻る |
| 4 | 各タブの DB 読み書き | `TabData=true` | フラグを `false` に戻す |
| 5 | 全 Flag 有効化 + コードクリーンアップ | 全 `true` → キー削除 | — |

### 6.4 データ整合性確保

- `App.config` の DB 名不一致（`lease_new` vs `lease_m4bs`）を実装開始前に解消
- `App.config.example` 方式でバージョン管理
- 開発用シードデータの管理
- 移行後の目視確認チェックリスト（8項目）

---

## 7. テスト計画

### 7.1 テスト戦略

- テスト DB: `lease_m4bs_test`（`LeaseM4BS_Test` 接続文字列が App.config に既存）
- データ管理: `<TestInitialize>` で INSERT、`<TestCleanup>` で DELETE
- 既存テスト（`AccountingCalcTests.vb`、`LeaseJudgmentTests.vb`）は変更しない

### 7.2 単体テスト（MSTest v2.2.8）

追加するテストファイル:

| ファイル名 | テスト対象 |
|---|---|
| `LeaseContractRepositoryTests.vb` | `LeaseContractRepository` |
| `LeaseAccountingRepositoryTests.vb` | `LeaseAccountingRepository` |
| `CrudHelperIntegrationTests.vb` | `CrudHelper`（統合） |
| `MasterDataLoaderTests.vb` | `MasterDataLoader` |

主要テストケース一覧:

| テスト対象 | テストメソッド名 | 検証内容 |
|---|---|---|
| `GetAll()` | `GetAll_ReturnsDataTable_NotNull` | DataTable が null でないこと |
| `GetByContractNo()` | `GetByContractNo_ExistingNo_ReturnsRow` | 既存レコードが1行返ること |
| `GetByContractNo()` | `GetByContractNo_NotFound_ReturnsEmptyTable` | 存在しない番号で0行 |
| `Insert()` | `Insert_ValidData_ReturnsOne` | 影響行数 1 が返ること（主キー取得が必要な場合はパターン6の ExecuteScalar を使用） |
| `Insert()` | `Insert_DuplicateContractNo_ThrowsException` | 重複で例外 |
| `Update()` | `Update_ExistingRecord_UpdatesName` | 更新後の値がDBに反映 |
| `Delete()` | `Delete_ExistingRecord_RemovesFromDb` | 削除後にレコード消失 |
| `Upsert()` | `Upsert_NewRecord_Inserts` | 新規はINSERT |
| `Upsert()` | `Upsert_ExistingRecord_Updates` | 既存はUPDATE |

### 7.3 結合テスト（手動）

| シナリオ | 確認内容 |
|---|---|
| 契約一覧表示 | FrmFlexContract に DB データが表示されること |
| 新規登録 | 「登録」→ `tw_lease_contract` にレコード作成 |
| 既存照会・更新 | 既存契約を開いて値が復元 → 修正 → DB に反映 |
| 資産詳細 | ポップアップで入力 → OK → 親画面に反映 |
| ComboBox | 各 ComboBox に DB マスタの選択肢が表示 |
| 会計タブ保存 | `RecalcAll()` 結果が `tw_lease_accounting` に保存 |

### 7.4 テスト環境セットアップ手順

```sql
CREATE DATABASE lease_m4bs_test OWNER lease_m4bs_user;
```

```bash
psql -U lease_m4bs_user -d lease_m4bs_test -f sql/master_tables.sql
psql -U lease_m4bs_user -d lease_m4bs_test -f sql/tw_tables.sql
psql -U lease_m4bs_user -d lease_m4bs_test -f sql/ctb_tables.sql
psql -U lease_m4bs_user -d lease_m4bs_test -f sql/seed_data.sql
```

---

## 8. スケジュール・マイルストーン

### 8.1 フェーズ定義とマイルストーン

| フェーズ | 内容 | マイルストーン条件 | 目安期間 |
|---|---|---|---|
| **準備** | テストDB構築 / Feature Flag 実装 / 接続文字列整備 | 接続確認ログが出力される | 0.5日 |
| **Phase A** | FrmLeaseContractMain pgContract CRUD + マスタ ComboBox | 契約の新規登録・修正・削除が動作 | 1週間 |
| **Phase B** | pgInitial / pgAccounting / pgSublease / pgJudgment CRUD | 全5タブの Read/Write が動作 | 1週間 |
| **Phase C** | ヘッダーボタン全機能実装 | 全7ボタンの動作確認完了、ISS-002 クローズ | 1週間 |
| **Phase D** | プレースホルダ画面（別計画） | — | 別途計画 |

### 8.2 依存関係と前提条件

**ブロッカー**:

| ブロッカー | 影響 | 対処 |
|---|---|---|
| ISS-003: grpParty 未実装 | Phase B pgJudgment | pgJudgment のみ後回し |
| 会計タブ改修ブランチ未マージ | Phase B pgAccounting | マージ完了を待つ |
| DB名不一致 | 全フェーズ | 実装前に統一 |

### 8.3 リスクと対策

| リスク | 影響度 | 対策 |
|---|---|---|
| FrmLeaseContractMain（45,000行）への CRUD 追加で保守困難化 | 高 | `ContractDataHelper.vb` に分離 |
| Feature Flag 削除忘れ | 高 | Phase C 完了チェックリストに必須項目追加 |
| DB設計書と実装コードの乖離 | 中 | `\d テーブル名` で実際のカラムを確認してから実装 |
| 接続文字列の Git 混入 | 高 | `.gitignore` 追加 or `App.config.example` 方式 |
| テストDB未構築で本番DB破壊 | 高 | `TestInitialize` で接続文字列名をアサーション確認 |

---

## 9. コーディング規約・命名規則

### 9.1 新設クラス・メソッドの命名規則

| 種別 | 命名パターン | 例 |
|---|---|---|
| リポジトリクラス | `XxxRepository` | `LeasePropertyRepository` |
| フォーム用CRUDヘルパー | `XxxDataHelper` | `ContractDataHelper` |
| Feature Flag クラス | `DbFeatureFlag` | — |
| テストクラス | `XxxTests` | `LeaseContractRepositoryTests` |
| DB読み込みメソッド | `LoadXxxData(key)` | `LoadContractData(contractNo)` |
| DB保存メソッド | `SaveXxxData()` | `SaveContractData()` |

**SQLパラメータ命名**: 通常 `@column_name`、WHERE句 `@where_xxx`、SET句 `@set_xxx`

### 9.2 ハードコード・仮実装コードのコメント規約

§2.1 で確認した通り、`LoadSampleData()` というメソッドは現行コードに存在しない。しかしハードコードされた仮実装（`MessageBox.Show("実装中")` 等）の削除タイミングを示すコメント規約として以下を適用する。

| 状態 | コメント形式 |
|---|---|
| DB未接続・仮実装状態 | `' HACK: DB接続実装後に置換する` |
| Feature Flag 切り替え済み | `' TODO: Phase A 完了後に削除する` |
| DB接続完了・削除対象確定 | `' FIXME: 仮実装コード削除対象` |

全 Phase のテスト Pass + コードレビュー承認後に削除する。

### 9.3 SQL 記述規則

- SQL キーワード大文字、テーブル名・カラム名は小文字スネークケース
- 複数行は `&` 演算子で連結
- 動的 WHERE 句はホワイトリスト方式
- 会計条文番号コメント（`' 第34号§22`）を CRUD メソッド内でも維持

---

## 10. 付録

### 10.1 CrudHelper メソッド早見表

| メソッド | 主な用途 | 戻り値 |
|---|---|---|
| `GetDataTable` | SELECT → DataTable 取得 | DataTable |
| `ExecuteScalar(Of T)` | 単一値取得（採番等） | 型 T の値 |
| `Insert` | 辞書型 INSERT | 影響行数（Integer）。主キー取得が必要な場合は `ExecuteScalar(Of Integer)` + `RETURNING` を使用する |
| `Update` | 辞書型 UPDATE（WHERE 必須） | 更新行数 |
| `Delete` | DELETE（WHERE 必須） | 削除行数 |
| `ExecuteNonQuery` | 直接 SQL 実行 | 影響行数 |
| `Exists` | レコード存在確認 | Boolean |
| `BeginTransaction` | トランザクション開始 | なし |
| `Commit` | コミット | なし |
| `Rollback` | ロールバック | なし |

### 10.2 PostgreSQL エラーコード対応表

| SQLSTATE | エラー名 | ユーザーメッセージ例 |
|---|---|---|
| `23505` | unique_violation | 「この契約番号は既に登録されています。」 |
| `23502` | not_null_violation | 「必須項目が入力されていません。」 |
| `23503` | foreign_key_violation | 「参照先のデータが存在しません。」 |
| `23514` | check_violation | 「入力値が許可された範囲を超えています。」 |
| `42P01` | undefined_table | 「システムエラーが発生しました。管理者に連絡してください。」 |
| `42703` | undefined_column | 「システムエラーが発生しました。管理者に連絡してください。」 |
| `08006` | connection_failure | 「データベースへの接続が切断されました。」 |
| `08001` | sqlclient_unable_to_establish | 「データベースに接続できません。」 |
| `40001` | serialization_failure | 自動リトライ（最大3回） |
| `40P01` | deadlock_detected | 「処理が競合しました。再試行してください。」 |

### 10.3 関連ドキュメント索引

| ドキュメント名 | ファイルパス | 参照箇所 |
|---|---|---|
| 要件定義書 v2 | `docs/project-documentation/01_要件定義書_v2.md` | DB接続要件（§2.3） |
| 基本設計書 v2 | `docs/project-documentation/02_基本設計書_v2.md` | アーキテクチャ方針（§1.1） |
| DB設計書 v2.1 | `docs/project-documentation/05_DB設計書.md` | テーブル定義・FK制約 |
| 進捗管理表 v2 | `docs/project-documentation/03_開発進捗管理表_v2.md` | ISS-001〜007 |
| コーディング規約書 | `docs/project-documentation/09_コーディング規約書.md` | SQL規則・エラー処理・命名 |
| 開発ロードマップ | `docs/project-documentation/15_開発ロードマップ・工程表.md` | フェーズ定義 |

---

*作成日: 2026年3月12日*
*作成ツール: Claude Code（マルチエージェント調査・分割執筆・統合レビュー）*
