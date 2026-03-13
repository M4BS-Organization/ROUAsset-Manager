# DB設計書 v5.0 — リース資産管理システム (LeaseM4BS)

**ドキュメント番号**: DB-2026-004
**バージョン**: 5.0
**作成日**: 2026年3月12日
**ステータス**: 確定
**情報ソース**: DB設計書v4.0、最適DB設計統合分析レポート(ANALYSIS-2026-001)、リース会計基準設例

---

## 改訂履歴

| 日付 | バージョン | 内容 |
|---|---|---|
| 2026-03-09 | 1.0 | 初版作成 |
| 2026-03-10 | 2.0 | SQL実装との完全同期 |
| 2026-03-11 | 2.1 | 共有ドライブ資料に基づく更新 |
| 2026-03-12 | 3.0 | ハイブリッドEAV設計、設例対応カラム追加 |
| 2026-03-12 | 4.0 | サブリース、仕訳自動生成、開示注記ビュー追加 |
| 2026-03-12 | 5.0 | **本版**: 103カラムモノリス廃止、36テーブル+7ビューに完全正規化。3NF厳密化、ASBJ#34全設例カバー |

---

## 1. 設計方針

### 1.1 設計原則

- **第3正規形(3NF)厳密準拠**: 全テーブルが3NFを満たす。推移的関数従属・部分関数従属を排除
- **EAVハイブリッド**: 資産種別固有属性のみEAV管理、共通属性は固定カラム
- **各テーブル最大22カラム**: 可読性・保守性を確保。CrudHelper Dictionary(Of String, Object) パターンとの親和性維持
- **NUMERIC(15,2)統一（金額系）**: 全金額カラムで統一。利率系のみNUMERIC(8,6)またはNUMERIC(15,6)
- **全テーブルにcreate_dt/update_dt**: 監査証跡として必須（schema_version、audit_logを除く）
- **プレフィックスなし命名**: PostgreSQLスキーマで層分離（v4の `m_`/`ctb_`/`tw_` プレフィックスを廃止）

### 1.2 テーブル層構成

| 層 | テーブル数 | 役割 |
|---|---|---|
| マスタ層 | 11 | 会社・勘定科目・資産区分等の参照データ |
| 契約・資産層 | 3 | リース契約・資産・オプション |
| 会計・測定層 | 4 | 当初測定・会計・支払・変動リース料 |
| スケジュール層 | 1 | 償却スケジュール |
| イベント層 | 4 | 再測定・条件変更・経過措置・セールリースバック |
| 貸手・サブリース層 | 2 | 貸手・サブリース関係 |
| 付帯層 | 4 | インセンティブ・原状回復・敷金・部門配賦 |
| 仕訳層 | 4 | 仕訳ヘッダ/明細・テンプレート |
| 外部連携・開示層 | 2 | 外部マッピング・開示スナップショット |
| システム管理層 | 2 | スキーマバージョン・監査ログ |
| **合計** | **37** | **EAV層2テーブル含む（36テーブル + audit_log = 37テーブル）** |

### 1.3 データフロー図

```
┌──────────────────────────────────────────────────────────────┐
│                   画面入力（WinForms）                         │
│  FrmLeaseContractMain / FrmAssetDetailEntry                   │
│  FrmFlexCtbViewer (BuildUI()で動的UI生成)                      │
└──────────────┬──────────────────────────────────┬────────────┘
               │ 保存時リアルタイム変換              │
               ▼                                   ▼
┌──────────────────────────┐    ┌──────────────────────────────┐
│   契約・資産層             │    │   マスタ層（参照のみ）         │
│   lease_contract          │    │   company                    │
│   lease_asset             │    │   supplier                   │
│   lease_option            │    │   department                 │
│                           │    │   asset_category             │
│   EAV層                   │    │   contract_type              │
│   asset_class_field       │    │   gl_account                 │
│   asset_attribute         │    │   index_master               │
└──────────┬───────────────┘    │   index_history              │
           │ 会計計算エンジン     │   borrowing_rate_history     │
           ▼                     └──────────────────────────────┘
┌──────────────────────────────────────────────────────────────┐
│              会計・測定層                                       │
│   lease_initial_measurement                                    │
│   lease_accounting                                             │
│   lease_payment_schedule                                       │
│   lease_variable_payment                                       │
└──────────┬───────────────────────────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────────────────────────┐
│              スケジュール層                                     │
│   amortization_schedule                                        │
└──────────┬───────────────────────────────────────────────────┘
           │
           ├──────────────────────────────────────┐
           ▼                                      ▼
┌────────────────────────┐    ┌────────────────────────────────┐
│   イベント層             │    │   仕訳層                        │
│   lease_remeasurement   │    │   journal_header               │
│   lease_modification    │    │   journal_detail               │
│   lease_transition      │    │   journal_template_header      │
│   sale_leaseback        │    │   journal_template_line        │
└────────────────────────┘    └──────────┬─────────────────────┘
                                         │
┌────────────────────────┐               ▼
│  貸手・サブリース層      │    ┌────────────────────────────────┐
│  lease_lessor           │    │   外部連携・開示層               │
│  sublease_relationship  │    │   external_mapping             │
│                         │    │   disclosure_snapshot          │
└────────────────────────┘    └────────────────────────────────┘

┌────────────────────────┐    ┌────────────────────────────────┐
│   付帯層                 │    │   システム管理層                 │
│   lease_incentive       │    │   schema_version               │
│   restoration_obligation│    │   audit_log                    │
│   lease_deposit         │    └────────────────────────────────┘
│   dept_allocation       │
└────────────────────────┘
```

### 1.4 ER図（テーブル関連図）

```
                            company
                              │
                    ┌─────────┤ 1:N
                    │         │
                    │     lease_contract ──────── contract_type
                    │         │
              supplier        │ 1:N
                              │
                        lease_asset ──────────── asset_category
                         │  │  │  │                    │
           ┌─────────────┘  │  │  └────────────┐      │
           │                │  │               │      │ 1:N
           ▼                │  ▼               ▼      ▼
    lease_option            │  lease_accounting    asset_class_field
                            │       │                  │
                            ▼       │                  │ 1:N
               lease_initial_       │                  ▼
               measurement          │            asset_attribute
                                    │
           ┌────────────────────────┤
           │                        │
           ▼                        ▼
    lease_payment_          amortization_schedule
    schedule                     │
           │                     ▼
           ▼              journal_header ──→ journal_detail
    lease_variable_              │
    payment                      ▼
                           gl_account

    lease_remeasurement ←── lease_asset
    lease_modification  ←── lease_asset
    lease_transition    ←── lease_asset
    sale_leaseback      ←── lease_asset
    lease_lessor        ←── lease_asset
    sublease_relationship ←─ lease_asset
    lease_incentive     ←── lease_asset
    restoration_obligation ←─ lease_asset
    lease_deposit       ←── lease_asset
    dept_allocation     ←── lease_asset

    index_master ←── index_history
    company ←── borrowing_rate_history

    journal_template_header ←── journal_template_line
```

---

## 2. マスタ層テーブル定義

### 2.1 company（会社マスタ）

会社情報を管理するマスタテーブル。リース契約の借手側・親会社等を登録する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | company_id | SERIAL | NOT NULL | (自動採番) | 会社ID（PK） |
| 2 | company_name | VARCHAR(100) | NOT NULL | - | 会社名称 |
| 3 | company_type | VARCHAR(20) | NULL | - | 会社区分（parent/subsidiary/affiliate等） |
| 4 | tax_id | VARCHAR(20) | NULL | - | 法人番号 |
| 5 | address | VARCHAR(200) | NULL | - | 所在地 |
| 6 | contact | VARCHAR(100) | NULL | - | 連絡先（電話番号・メール等） |
| 7 | is_active | BOOLEAN | NOT NULL | TRUE | 有効フラグ |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_company | (company_id) |
| INDEX | idx_company_name | (company_name) |
| INDEX | idx_company_active | (is_active) |

#### DDL

```sql
-- ============================================================
-- 2.1 company（会社マスタ）
-- ============================================================
CREATE TABLE company (
    company_id        SERIAL        NOT NULL,
    company_name      VARCHAR(100)  NOT NULL,
    company_type      VARCHAR(20),
    tax_id            VARCHAR(20),
    address           VARCHAR(200),
    contact           VARCHAR(100),
    is_active         BOOLEAN       NOT NULL DEFAULT TRUE,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_company PRIMARY KEY (company_id)
);

CREATE INDEX idx_company_name   ON company (company_name);
CREATE INDEX idx_company_active ON company (is_active);
```

---

### 2.2 supplier（取引先マスタ）

リース貸主・サプライヤー等の取引先情報を管理する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | supplier_id | SERIAL | NOT NULL | (自動採番) | 取引先ID（PK） |
| 2 | supplier_name | VARCHAR(100) | NOT NULL | - | 取引先名称 |
| 3 | supplier_type | VARCHAR(20) | NULL | - | 取引先区分（lessor/vendor/other等） |
| 4 | tax_id | VARCHAR(20) | NULL | - | 法人番号 |
| 5 | address | VARCHAR(200) | NULL | - | 所在地 |
| 6 | contact | VARCHAR(100) | NULL | - | 連絡先 |
| 7 | is_active | BOOLEAN | NOT NULL | TRUE | 有効フラグ |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_supplier | (supplier_id) |
| INDEX | idx_supplier_name | (supplier_name) |
| INDEX | idx_supplier_active | (is_active) |

#### DDL

```sql
-- ============================================================
-- 2.2 supplier（取引先マスタ）
-- ============================================================
CREATE TABLE supplier (
    supplier_id       SERIAL        NOT NULL,
    supplier_name     VARCHAR(100)  NOT NULL,
    supplier_type     VARCHAR(20),
    tax_id            VARCHAR(20),
    address           VARCHAR(200),
    contact           VARCHAR(100),
    is_active         BOOLEAN       NOT NULL DEFAULT TRUE,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_supplier PRIMARY KEY (supplier_id)
);

CREATE INDEX idx_supplier_name   ON supplier (supplier_name);
CREATE INDEX idx_supplier_active ON supplier (is_active);
```

---

### 2.3 department（部署マスタ）

組織の部署情報を管理する。階層構造をparent_deptによる自己参照で表現する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | dept_code | VARCHAR(10) | NOT NULL | - | 部署コード（PK） |
| 2 | dept_name | VARCHAR(100) | NOT NULL | - | 部署名称 |
| 3 | parent_dept | VARCHAR(10) | NULL | - | 親部署コード（FK→自テーブル） |
| 4 | cost_center | VARCHAR(20) | NULL | - | コストセンターコード |
| 5 | manager | VARCHAR(50) | NULL | - | 部署責任者名 |
| 6 | is_active | BOOLEAN | NOT NULL | TRUE | 有効フラグ |
| 7 | display_order | INTEGER | NULL | - | 表示順序 |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_department | (dept_code) |
| FK | fk_dept_parent | parent_dept → department(dept_code) |
| INDEX | idx_dept_parent | (parent_dept) |
| INDEX | idx_dept_active | (is_active) |
| INDEX | idx_dept_order | (display_order) |

#### DDL

```sql
-- ============================================================
-- 2.3 department（部署マスタ）
-- ============================================================
CREATE TABLE department (
    dept_code         VARCHAR(10)   NOT NULL,
    dept_name         VARCHAR(100)  NOT NULL,
    parent_dept       VARCHAR(10),
    cost_center       VARCHAR(20),
    manager           VARCHAR(50),
    is_active         BOOLEAN       NOT NULL DEFAULT TRUE,
    display_order     INTEGER,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_department PRIMARY KEY (dept_code),
    CONSTRAINT fk_dept_parent FOREIGN KEY (parent_dept)
        REFERENCES department (dept_code)
);

CREATE INDEX idx_dept_parent ON department (parent_dept);
CREATE INDEX idx_dept_active ON department (is_active);
CREATE INDEX idx_dept_order  ON department (display_order);
```

---

### 2.4 contract_type（契約種別マスタ）

リース契約の種別（ファイナンスリース、オペレーティングリース等）を管理する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | type_code | VARCHAR(10) | NOT NULL | - | 契約種別コード（PK） |
| 2 | type_name | VARCHAR(50) | NOT NULL | - | 契約種別名称 |
| 3 | description | VARCHAR(200) | NULL | - | 説明 |
| 4 | is_active | BOOLEAN | NOT NULL | TRUE | 有効フラグ |
| 5 | display_order | INTEGER | NULL | - | 表示順序 |
| 6 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 7 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 8 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_contract_type | (type_code) |
| INDEX | idx_contract_type_active | (is_active) |
| INDEX | idx_contract_type_order | (display_order) |

#### DDL

```sql
-- ============================================================
-- 2.4 contract_type（契約種別マスタ）
-- ============================================================
CREATE TABLE contract_type (
    type_code         VARCHAR(10)   NOT NULL,
    type_name         VARCHAR(50)   NOT NULL,
    description       VARCHAR(200),
    is_active         BOOLEAN       NOT NULL DEFAULT TRUE,
    display_order     INTEGER,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_contract_type PRIMARY KEY (type_code)
);

CREATE INDEX idx_contract_type_active ON contract_type (is_active);
CREATE INDEX idx_contract_type_order  ON contract_type (display_order);
```

---

### 2.5 gl_account（勘定科目マスタ）

勘定科目を一元管理するマスタテーブル。仕訳自動生成・開示注記・資産区分の勘定科目参照先として使用される。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | account_code | VARCHAR(10) | NOT NULL | - | 勘定科目コード（PK） |
| 2 | account_name | VARCHAR(100) | NOT NULL | - | 勘定科目名称 |
| 3 | account_category | VARCHAR(20) | NULL | - | 勘定科目区分（asset/liability/equity/revenue/expense） |
| 4 | normal_balance | VARCHAR(10) | NULL | - | 通常残高方向（CHECK: debit/credit） |
| 5 | is_active | BOOLEAN | NOT NULL | TRUE | 有効フラグ |
| 6 | display_order | INTEGER | NULL | - | 表示順序 |
| 7 | parent_account_code | VARCHAR(10) | NULL | - | 親勘定科目コード（階層構造用） |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_gl_account | (account_code) |
| CHECK | chk_gl_normal_balance | normal_balance IN ('debit', 'credit') |
| INDEX | idx_gl_account_category | (account_category) |
| INDEX | idx_gl_account_active | (is_active) |
| INDEX | idx_gl_account_parent | (parent_account_code) |

#### DDL

```sql
-- ============================================================
-- 2.5 gl_account（勘定科目マスタ）
-- ============================================================
CREATE TABLE gl_account (
    account_code        VARCHAR(10)   NOT NULL,
    account_name        VARCHAR(100)  NOT NULL,
    account_category    VARCHAR(20),
    normal_balance      VARCHAR(10),
    is_active           BOOLEAN       NOT NULL DEFAULT TRUE,
    display_order       INTEGER,
    parent_account_code VARCHAR(10),
    remarks             VARCHAR(500),
    create_dt           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_gl_account PRIMARY KEY (account_code),
    CONSTRAINT chk_gl_normal_balance CHECK (normal_balance IN ('debit', 'credit'))
);

CREATE INDEX idx_gl_account_category ON gl_account (account_category);
CREATE INDEX idx_gl_account_active   ON gl_account (is_active);
CREATE INDEX idx_gl_account_parent   ON gl_account (parent_account_code);
```

---

### 2.6 asset_category（資産区分マスタ）

リース資産の分類を管理する。EAVアンカーテーブルとして、asset_class_fieldの属性定義セットの起点となる。また、分類ごとのデフォルト勘定科目を保持する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | category_code | VARCHAR(10) | NOT NULL | - | 資産区分コード（PK） |
| 2 | category_name | VARCHAR(100) | NOT NULL | - | 資産区分名称 |
| 3 | parent_category | VARCHAR(10) | NULL | - | 親区分コード（階層構造用、FK→自テーブル） |
| 4 | depreciation_method_default | VARCHAR(10) | NULL | 'SL' | デフォルト償却方法（SL=定額法/DB=定率法等） |
| 5 | useful_life_default | INTEGER | NULL | - | デフォルト耐用年数（月） |
| 6 | account_rou | VARCHAR(10) | NULL | - | 使用権資産勘定コード |
| 7 | account_depreciation | VARCHAR(10) | NULL | - | 減価償却費勘定コード |
| 8 | account_liability | VARCHAR(10) | NULL | - | リース負債勘定コード |
| 9 | account_interest | VARCHAR(10) | NULL | - | 支払利息勘定コード |
| 10 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 11 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 12 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_asset_category | (category_code) |
| FK | fk_asset_cat_parent | parent_category → asset_category(category_code) |
| INDEX | idx_asset_cat_parent | (parent_category) |

> **注記**: account_rou, account_depreciation, account_liability, account_interest の各カラムにはgl_accountへのFK制約を設けない。gl_accountとの整合性はアプリケーション層で保証する（v4踏襲方針）。

#### DDL

```sql
-- ============================================================
-- 2.6 asset_category（資産区分マスタ）
-- ============================================================
CREATE TABLE asset_category (
    category_code                VARCHAR(10)   NOT NULL,
    category_name                VARCHAR(100)  NOT NULL,
    parent_category              VARCHAR(10),
    depreciation_method_default  VARCHAR(10)   DEFAULT 'SL',
    useful_life_default          INTEGER,
    account_rou                  VARCHAR(10),
    account_depreciation         VARCHAR(10),
    account_liability            VARCHAR(10),
    account_interest             VARCHAR(10),
    remarks                      VARCHAR(500),
    create_dt                    TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                    TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_asset_category PRIMARY KEY (category_code),
    CONSTRAINT fk_asset_cat_parent FOREIGN KEY (parent_category)
        REFERENCES asset_category (category_code)
);

CREATE INDEX idx_asset_cat_parent ON asset_category (parent_category);
```

---

### 2.7 index_master（指数マスタ）

CPI・LIBOR・TIBOR等の外部指数を管理するマスタテーブル。変動リース料の指数連動計算（設例13）や再測定時の指数参照に使用する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | index_id | SERIAL | NOT NULL | (自動採番) | 指数ID（PK） |
| 2 | index_name | VARCHAR(100) | NOT NULL | - | 指数名称 |
| 3 | index_type | VARCHAR(20) | NULL | - | 指数種別（CPI/LIBOR/TIBOR等） |
| 4 | base_value | NUMERIC(15,6) | NULL | - | 基準値 |
| 5 | base_date | DATE | NULL | - | 基準日 |
| 6 | current_value | NUMERIC(15,6) | NULL | - | 現在値 |
| 7 | current_date_val | DATE | NULL | - | 現在値取得日 |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_index_master | (index_id) |
| INDEX | idx_index_master_type | (index_type) |
| INDEX | idx_index_master_name | (index_name) |

#### DDL

```sql
-- ============================================================
-- 2.7 index_master（指数マスタ）
-- ============================================================
CREATE TABLE index_master (
    index_id          SERIAL          NOT NULL,
    index_name        VARCHAR(100)    NOT NULL,
    index_type        VARCHAR(20),
    base_value        NUMERIC(15,6),
    base_date         DATE,
    current_value     NUMERIC(15,6),
    current_date_val  DATE,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_index_master PRIMARY KEY (index_id)
);

CREATE INDEX idx_index_master_type ON index_master (index_type);
CREATE INDEX idx_index_master_name ON index_master (index_name);
```

---

### 2.8 index_history（指数履歴）

index_masterの時系列データを管理する。指数の過去値を日次で記録し、変動リース料の再計算や再測定時に参照する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | history_id | SERIAL | NOT NULL | (自動採番) | 履歴ID（PK） |
| 2 | index_id | INTEGER | NOT NULL | - | 指数ID（FK→index_master） |
| 3 | record_date | DATE | NOT NULL | - | 記録日 |
| 4 | index_value | NUMERIC(15,6) | NOT NULL | - | 指数値 |
| 5 | source | VARCHAR(100) | NULL | - | データソース（取得元） |
| 6 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 7 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 8 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_index_history | (history_id) |
| FK | fk_idxhist_index | index_id → index_master(index_id) ON DELETE CASCADE |
| UNIQUE | uq_index_history_date | (index_id, record_date) |
| INDEX | idx_idxhist_index | (index_id) |
| INDEX | idx_idxhist_date | (record_date) |

#### DDL

```sql
-- ============================================================
-- 2.8 index_history（指数履歴）
-- ============================================================
CREATE TABLE index_history (
    history_id        SERIAL          NOT NULL,
    index_id          INTEGER         NOT NULL,
    record_date       DATE            NOT NULL,
    index_value       NUMERIC(15,6)   NOT NULL,
    source            VARCHAR(100),
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_index_history PRIMARY KEY (history_id),
    CONSTRAINT fk_idxhist_index FOREIGN KEY (index_id)
        REFERENCES index_master (index_id) ON DELETE CASCADE,
    CONSTRAINT uq_index_history_date UNIQUE (index_id, record_date)
);

CREATE INDEX idx_idxhist_index ON index_history (index_id);
CREATE INDEX idx_idxhist_date  ON index_history (record_date);
```

---

### 2.9 borrowing_rate_history（追加借入利子率履歴）

会社・通貨・適用開始日ごとの追加借入利子率を管理する。割引率の自動決定（ASBJ第34号における追加借入利子率）に使用する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | rate_id | SERIAL | NOT NULL | (自動採番) | 利率ID（PK） |
| 2 | company_id | INTEGER | NOT NULL | - | 会社ID（FK→company） |
| 3 | effective_date | DATE | NOT NULL | - | 適用開始日 |
| 4 | rate_value | NUMERIC(8,6) | NOT NULL | - | 利率値（例: 0.025000 = 2.5%） |
| 5 | rate_type | VARCHAR(30) | NULL | - | 利率種別（追加借入利子率/基準金利等） |
| 6 | currency_code | VARCHAR(3) | NOT NULL | 'JPY' | 通貨コード |
| 7 | source | VARCHAR(100) | NULL | - | 利率ソース（主要取引銀行名等） |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_borrowing_rate_history | (rate_id) |
| FK | fk_brh_company | company_id → company(company_id) |
| UNIQUE | uq_brh_company_date_type | (company_id, effective_date, rate_type, currency_code) |
| INDEX | idx_brh_company | (company_id) |
| INDEX | idx_brh_effective | (effective_date) |

#### DDL

```sql
-- ============================================================
-- 2.9 borrowing_rate_history（追加借入利子率履歴）
-- ============================================================
CREATE TABLE borrowing_rate_history (
    rate_id           SERIAL          NOT NULL,
    company_id        INTEGER         NOT NULL,
    effective_date    DATE            NOT NULL,
    rate_value        NUMERIC(8,6)    NOT NULL,
    rate_type         VARCHAR(30),
    currency_code     VARCHAR(3)      NOT NULL DEFAULT 'JPY',
    source            VARCHAR(100),
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_borrowing_rate_history PRIMARY KEY (rate_id),
    CONSTRAINT fk_brh_company FOREIGN KEY (company_id)
        REFERENCES company (company_id),
    CONSTRAINT uq_brh_company_date_type
        UNIQUE (company_id, effective_date, rate_type, currency_code)
);

CREATE INDEX idx_brh_company   ON borrowing_rate_history (company_id);
CREATE INDEX idx_brh_effective ON borrowing_rate_history (effective_date);
```

---

## 3. EAV層テーブル定義

### 3.1 asset_class_field（属性定義マスタ）

資産区分ごとの動的属性フィールドを定義するEAV定義テーブル。FrmFlexCtbViewerのBuildUI()パターンにより、本テーブルの定義に基づいて動的にUI（テキストボックス、コンボボックス等）を生成する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | field_id | SERIAL | NOT NULL | (自動採番) | フィールドID（PK） |
| 2 | category_code | VARCHAR(10) | NOT NULL | - | 資産区分コード（FK→asset_category） |
| 3 | field_name | VARCHAR(50) | NOT NULL | - | フィールド名（英字、プログラム参照用） |
| 4 | field_label | VARCHAR(100) | NOT NULL | - | フィールドラベル（日本語、画面表示用） |
| 5 | data_type | VARCHAR(20) | NOT NULL | - | データ型（text/numeric/date/boolean） |
| 6 | is_required | BOOLEAN | NOT NULL | FALSE | 必須フラグ |
| 7 | display_order | INTEGER | NULL | - | 表示順序 |
| 8 | ui_control_type | VARCHAR(30) | NULL | 'textbox' | UIコントロール種別（textbox/combobox/datepicker/checkbox等） |
| 9 | ui_group | VARCHAR(50) | NULL | - | UIグループ名（画面のグループボックス単位） |
| 10 | ui_width | INTEGER | NULL | - | UIコントロール幅（ピクセル） |
| 11 | ui_row | INTEGER | NULL | - | UI配置行番号 |
| 12 | ui_col | INTEGER | NULL | - | UI配置列番号 |
| 13 | default_value | VARCHAR(200) | NULL | - | デフォルト値（文字列表現） |
| 14 | combobox_source | VARCHAR(200) | NULL | - | コンボボックスデータソース（テーブル名.カラム名 or CSV値リスト） |
| 15 | is_visible | BOOLEAN | NOT NULL | TRUE | 表示フラグ |
| 16 | is_readonly | BOOLEAN | NOT NULL | FALSE | 読取専用フラグ |
| 17 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 18 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_asset_class_field | (field_id) |
| FK | fk_acf_category | category_code → asset_category(category_code) ON DELETE CASCADE |
| UNIQUE | uq_acf_category_field | (category_code, field_name) |
| INDEX | idx_acf_category | (category_code) |
| INDEX | idx_acf_order | (category_code, display_order) |

#### DDL

```sql
-- ============================================================
-- 3.1 asset_class_field（属性定義マスタ）
-- ============================================================
CREATE TABLE asset_class_field (
    field_id          SERIAL        NOT NULL,
    category_code     VARCHAR(10)   NOT NULL,
    field_name        VARCHAR(50)   NOT NULL,
    field_label       VARCHAR(100)  NOT NULL,
    data_type         VARCHAR(20)   NOT NULL,
    is_required       BOOLEAN       NOT NULL DEFAULT FALSE,
    display_order     INTEGER,
    ui_control_type   VARCHAR(30)   DEFAULT 'textbox',
    ui_group          VARCHAR(50),
    ui_width          INTEGER,
    ui_row            INTEGER,
    ui_col            INTEGER,
    default_value     VARCHAR(200),
    combobox_source   VARCHAR(200),
    is_visible        BOOLEAN       NOT NULL DEFAULT TRUE,
    is_readonly       BOOLEAN       NOT NULL DEFAULT FALSE,
    create_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_asset_class_field PRIMARY KEY (field_id),
    CONSTRAINT fk_acf_category FOREIGN KEY (category_code)
        REFERENCES asset_category (category_code) ON DELETE CASCADE,
    CONSTRAINT uq_acf_category_field UNIQUE (category_code, field_name)
);

CREATE INDEX idx_acf_category ON asset_class_field (category_code);
CREATE INDEX idx_acf_order    ON asset_class_field (category_code, display_order);
```

---

### 3.2 asset_attribute（属性値テーブル）

lease_assetに紐づく動的属性の値を格納するEAV値テーブル。data_typeに応じてvalue_text/value_numeric/value_date/value_booleanのいずれか1つに値を格納する。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | attr_id | SERIAL | NOT NULL | (自動採番) | 属性値ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset） |
| 3 | field_id | INTEGER | NOT NULL | - | フィールドID（FK→asset_class_field） |
| 4 | value_text | VARCHAR(500) | NULL | - | テキスト値（data_type='text'の場合） |
| 5 | value_numeric | NUMERIC(15,2) | NULL | - | 数値値（data_type='numeric'の場合） |
| 6 | value_date | DATE | NULL | - | 日付値（data_type='date'の場合） |
| 7 | value_boolean | BOOLEAN | NULL | - | 真偽値（data_type='boolean'の場合） |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_asset_attribute | (attr_id) |
| FK | fk_attr_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| FK | fk_attr_field | field_id → asset_class_field(field_id) ON DELETE CASCADE |
| UNIQUE | uq_attr_asset_field | (asset_id, field_id) |
| INDEX | idx_attr_asset | (asset_id) |
| INDEX | idx_attr_field | (field_id) |

> **注記**: asset_idのFK参照先 `lease_asset` は契約・資産層（Part2で定義）のテーブル。スキーマ作成時はテーブル作成順序に注意し、FK制約はALTER TABLEで後付けすること。

#### DDL

```sql
-- ============================================================
-- 3.2 asset_attribute（属性値テーブル）
-- ============================================================
CREATE TABLE asset_attribute (
    attr_id           SERIAL          NOT NULL,
    asset_id          INTEGER         NOT NULL,
    field_id          INTEGER         NOT NULL,
    value_text        VARCHAR(500),
    value_numeric     NUMERIC(15,2),
    value_date        DATE,
    value_boolean     BOOLEAN,
    remarks           VARCHAR(500),
    create_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_asset_attribute PRIMARY KEY (attr_id),
    CONSTRAINT uq_attr_asset_field UNIQUE (asset_id, field_id)
);

-- FK制約はlease_asset作成後にALTER TABLEで追加
-- ALTER TABLE asset_attribute
--     ADD CONSTRAINT fk_attr_asset FOREIGN KEY (asset_id)
--         REFERENCES lease_asset (asset_id) ON DELETE CASCADE;

ALTER TABLE asset_attribute
    ADD CONSTRAINT fk_attr_field FOREIGN KEY (field_id)
        REFERENCES asset_class_field (field_id) ON DELETE CASCADE;

CREATE INDEX idx_attr_asset ON asset_attribute (asset_id);
CREATE INDEX idx_attr_field ON asset_attribute (field_id);
```

---

## 4. 契約・資産層テーブル定義

### 4.1 lease_contract（リース契約テーブル -- 17列）

v4の ctb_lease_integrated から契約属性を分離した正規化テーブル。1契約に対し複数の資産（lease_asset）が紐づく。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | contract_id | SERIAL | YES | - | 契約ID（PK） |
| 2 | company_id | INTEGER | YES | - | 会社ID（FK → company） |
| 3 | supplier_cd | VARCHAR(10) | YES | - | 取引先コード（FK → supplier） |
| 4 | contract_type_cd | VARCHAR(10) | YES | - | 契約種別コード（FK → contract_type） |
| 5 | contract_no | VARCHAR(50) | YES | - | 契約番号（UNIQUE） |
| 6 | contract_name | VARCHAR(200) | YES | - | 契約名称 |
| 7 | contract_date | DATE | NO | - | 契約締結日 |
| 8 | contract_start_date | DATE | YES | - | 契約開始日 |
| 9 | contract_end_date | DATE | YES | - | 契約終了日 |
| 10 | lease_classification | VARCHAR(20) | YES | - | リース分類（finance_transfer / finance_non_transfer / operating） |
| 11 | has_cancel_option | BOOLEAN | - | FALSE | 解約オプション有無 |
| 12 | auto_renewal | BOOLEAN | - | FALSE | 自動更新フラグ |
| 13 | renewal_notice_months | INTEGER | - | - | 更新通知期間（月数） |
| 14 | status | VARCHAR(20) | - | 'active' | 契約ステータス（active / terminated / expired） |
| 15 | remarks | VARCHAR(500) | - | - | 備考 |
| 16 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 17 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_contract | (contract_id) |
| UNIQUE | uq_lease_contract_no | (contract_no) |
| FK | fk_lease_contract_lessor | (lessor_company_id) REFERENCES company(company_id) |
| FK | fk_lease_contract_lessee | (lessee_company_id) REFERENCES company(company_id) |
| FK | fk_lease_contract_type | (contract_type_code) REFERENCES contract_type(type_code) |
| INDEX | idx_lease_contract_classification | (lease_classification) |
| INDEX | idx_lease_contract_status | (status) |
| INDEX | idx_lease_contract_dates | (contract_start_date, contract_end_date) |

#### DDL

```sql
-- ============================================================
-- 4.1 lease_contract（リース契約テーブル）
-- ============================================================
CREATE TABLE lease_contract (
    contract_id            SERIAL         PRIMARY KEY,
    contract_no            VARCHAR(20)    NOT NULL,
    contract_name          VARCHAR(200),
    lessor_company_id      INTEGER,
    lessee_company_id      INTEGER,
    contract_type_code     VARCHAR(10),
    lease_classification   VARCHAR(20)    NOT NULL,
    contract_date          DATE,
    contract_start_date    DATE           NOT NULL,
    contract_end_date      DATE           NOT NULL,
    has_cancel_option      BOOLEAN        DEFAULT FALSE,
    auto_renewal           BOOLEAN        DEFAULT FALSE,
    renewal_notice_months  INTEGER,
    status                 VARCHAR(20)    DEFAULT 'active',
    remarks                VARCHAR(500),
    create_dt              TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt              TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT uq_lease_contract_no
        UNIQUE (contract_no),

    CONSTRAINT fk_lease_contract_lessor
        FOREIGN KEY (lessor_company_id) REFERENCES company(company_id),

    CONSTRAINT fk_lease_contract_lessee
        FOREIGN KEY (lessee_company_id) REFERENCES company(company_id),

    CONSTRAINT fk_lease_contract_type
        FOREIGN KEY (contract_type_code) REFERENCES contract_type(type_code),

    CONSTRAINT chk_lease_contract_classification
        CHECK (lease_classification IN ('finance_transfer', 'finance_non_transfer', 'operating')),

    CONSTRAINT chk_lease_contract_dates
        CHECK (contract_end_date >= contract_start_date)
);

CREATE INDEX idx_lease_contract_classification ON lease_contract(lease_classification);
CREATE INDEX idx_lease_contract_status ON lease_contract(status);
CREATE INDEX idx_lease_contract_dates ON lease_contract(contract_start_date, contract_end_date);

COMMENT ON TABLE lease_contract IS 'リース契約テーブル — 契約の基本情報を管理';
COMMENT ON COLUMN lease_contract.lease_classification IS 'finance_transfer=所有権移転FL / finance_non_transfer=所有権移転外FL / operating=OL';
COMMENT ON COLUMN lease_contract.contract_date IS '契約締結日（リース開始日とは異なる場合がある）';
COMMENT ON COLUMN lease_contract.has_cancel_option IS 'v4 cancellable_flag から改名（用語統一）';
```

---

### 4.2 lease_asset（リース資産テーブル — 15列）

リース対象の個別資産を管理する。lease_contract に対して N:1 の関係。v4の ctb_lease_integrated から資産属性を分離。償却方法(depreciation_method)は分析レポートの指摘により lease_accounting から本テーブルに移動（資産固有の属性のため）。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | asset_id | SERIAL | NOT NULL | (自動採番) | 資産ID（PK） |
| 2 | contract_id | INTEGER | - | - | 契約ID（FK→lease_contract ON DELETE CASCADE） |
| 3 | asset_no | VARCHAR(20) | NOT NULL | - | 資産番号（UNIQUE） |
| 4 | asset_name | VARCHAR(200) | NOT NULL | - | 資産名称 |
| 5 | asset_category_code | VARCHAR(10) | - | - | 資産分類コード（FK→asset_category） |
| 6 | location | VARCHAR(200) | - | - | 設置場所 |
| 7 | quantity | INTEGER | - | 1 | 数量 |
| 8 | useful_life_months | INTEGER | - | - | 経済的耐用年数（月） |
| 9 | residual_value_guarantee | NUMERIC(15,2) | - | 0 | 残価保証額 |
| 10 | is_low_value | BOOLEAN | - | FALSE | 少額リース判定フラグ |
| 11 | is_short_term | BOOLEAN | - | FALSE | 短期リース判定フラグ |
| 12 | mgmt_dept_cd | VARCHAR(10) | - | - | 管理部署コード（FK→department） |
| 13 | depreciation_method | VARCHAR(10) | - | 'SL' | 償却方法（SL=定額法 / DB=定率法） |
| 14 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 15 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **注**: unit列は削除。quantity の INTEGER 型で数量管理に十分対応可能。カラム数は create_dt/update_dt を含めて15列。

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_asset | (asset_id) |
| UNIQUE | uq_lease_asset_no | (asset_no) |
| FK | fk_lease_asset_contract | (contract_id) REFERENCES lease_contract(contract_id) ON DELETE CASCADE |
| FK | fk_lease_asset_category | (asset_category_code) REFERENCES asset_category(category_code) |
| FK | fk_lease_asset_dept | (mgmt_dept_cd) REFERENCES department(dept_code) |
| INDEX | idx_lease_asset_contract | (contract_id) |
| INDEX | idx_lease_asset_category | (asset_category_code) |
| INDEX | idx_lease_asset_flags | (is_low_value, is_short_term) |

#### DDL

```sql
-- ============================================================
-- 4.2 lease_asset（リース資産テーブル）
-- ============================================================
CREATE TABLE lease_asset (
    asset_id                  SERIAL          PRIMARY KEY,
    contract_id               INTEGER,
    asset_no                  VARCHAR(20)     NOT NULL,
    asset_name                VARCHAR(200)    NOT NULL,
    asset_category_code       VARCHAR(10),
    location                  VARCHAR(200),
    quantity                  INTEGER         DEFAULT 1,
    useful_life_months        INTEGER,
    residual_value_guarantee  NUMERIC(15,2)   DEFAULT 0,
    is_low_value              BOOLEAN         DEFAULT FALSE,
    is_short_term             BOOLEAN         DEFAULT FALSE,
    mgmt_dept_cd              VARCHAR(10),
    depreciation_method       VARCHAR(10)     DEFAULT 'SL',
    create_dt                 TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                 TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT uq_lease_asset_no
        UNIQUE (asset_no),

    CONSTRAINT fk_lease_asset_contract
        FOREIGN KEY (contract_id) REFERENCES lease_contract(contract_id) ON DELETE CASCADE,

    CONSTRAINT fk_lease_asset_category
        FOREIGN KEY (asset_category_code) REFERENCES asset_category(category_code),

    CONSTRAINT fk_lease_asset_dept
        FOREIGN KEY (mgmt_dept_cd) REFERENCES department(dept_code),

    CONSTRAINT chk_lease_asset_quantity
        CHECK (quantity > 0),

    CONSTRAINT chk_lease_asset_depreciation
        CHECK (depreciation_method IN ('SL', 'DB'))
);

CREATE INDEX idx_lease_asset_contract ON lease_asset(contract_id);
CREATE INDEX idx_lease_asset_category ON lease_asset(asset_category_code);
CREATE INDEX idx_lease_asset_flags ON lease_asset(is_low_value, is_short_term);

COMMENT ON TABLE lease_asset IS 'リース資産テーブル — リース対象の個別資産を管理';
COMMENT ON COLUMN lease_asset.depreciation_method IS 'SL=Straight-Line(定額法) / DB=Declining-Balance(定率法)。分析レポートによりlease_accountingから移動';
COMMENT ON COLUMN lease_asset.mgmt_dept_cd IS 'v4 m_asset.mgmt_dept_cd 相当。部門配賦の基点';
COMMENT ON COLUMN lease_asset.is_low_value IS 'ASBJ第34号 少額リース判定。TRUEの場合、使用権資産・リース負債を認識しない簡便処理';
COMMENT ON COLUMN lease_asset.is_short_term IS 'ASBJ第34号 短期リース判定。リース期間12ヶ月以内の場合TRUE';
```

---

### 4.3 lease_option（リースオプションテーブル — 12列）

購入オプション・延長オプション・解約オプションを管理する。ASBJ第34号 設例8（リース期間の決定）・設例10（所有権移転）で使用。合理的確実性の評価履歴を保持。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | option_id | SERIAL | NOT NULL | (自動採番) | オプションID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | option_type | VARCHAR(20) | NOT NULL | - | オプション種別（purchase / extend / terminate） |
| 4 | exercise_date | DATE | - | - | 行使予定日 |
| 5 | option_price | NUMERIC(15,2) | - | - | オプション行使価格 |
| 6 | is_reasonably_certain | BOOLEAN | - | FALSE | 合理的確実性フラグ |
| 7 | assessment_date | DATE | - | - | 評価日 |
| 8 | assessment_basis | TEXT | - | - | 評価根拠（テキスト記述） |
| 9 | exercised_flag | BOOLEAN | - | FALSE | 行使済フラグ |
| 10 | remarks | VARCHAR(500) | - | - | 備考 |
| 11 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 12 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_option | (option_id) |
| FK | fk_lease_option_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| INDEX | idx_lease_option_asset | (asset_id) |
| INDEX | idx_lease_option_type | (option_type) |
| INDEX | idx_lease_option_exercise | (exercise_date, exercised_flag) |

#### DDL

```sql
-- ============================================================
-- 4.3 lease_option（リースオプションテーブル）
-- ============================================================
CREATE TABLE lease_option (
    option_id              SERIAL          PRIMARY KEY,
    asset_id               INTEGER,
    option_type            VARCHAR(20)     NOT NULL,
    exercise_date          DATE,
    option_price           NUMERIC(15,2),
    is_reasonably_certain  BOOLEAN         DEFAULT FALSE,
    assessment_date        DATE,
    assessment_basis       TEXT,
    exercised_flag         BOOLEAN         DEFAULT FALSE,
    remarks                VARCHAR(500),
    create_dt              TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt              TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lease_option_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT chk_lease_option_type
        CHECK (option_type IN ('purchase', 'extend', 'terminate'))
);

CREATE INDEX idx_lease_option_asset ON lease_option(asset_id);
CREATE INDEX idx_lease_option_type ON lease_option(option_type);
CREATE INDEX idx_lease_option_exercise ON lease_option(exercise_date, exercised_flag);

COMMENT ON TABLE lease_option IS 'リースオプションテーブル — 購入・延長・解約オプションを管理';
COMMENT ON COLUMN lease_option.option_type IS 'purchase=購入オプション / extend=延長オプション / terminate=解約オプション';
COMMENT ON COLUMN lease_option.is_reasonably_certain IS 'ASBJ第34号「合理的に確実」の判定結果。リース期間・PV計算に影響';
COMMENT ON COLUMN lease_option.assessment_basis IS '合理的確実性の評価根拠。監査対応用テキスト';
```

---

## 5. 会計・測定層テーブル定義

### 5.1 lease_initial_measurement（当初測定テーブル — 23列）

リース開始日時点の当初測定結果を保持する。PV計算の構成要素（固定支払額・変動支払見積・残価保証額・購入オプション価格・解約ペナルティ・原状回復費用）を個別管理。ASBJ第34号 設例7（リース/非リース配分）・設例9（PV計算）・設例11（残価保証）に対応。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | measurement_id | SERIAL | NOT NULL | (自動採番) | 測定ID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | measurement_date | DATE | NOT NULL | - | 測定基準日 |
| 4 | fixed_payment_total | NUMERIC(15,2) | - | - | 固定リース料総額 |
| 5 | variable_payment_estimate | NUMERIC(15,2) | - | 0 | 変動リース料見積額 |
| 6 | residual_guarantee_amount | NUMERIC(15,2) | - | 0 | 残価保証額 |
| 7 | purchase_option_price | NUMERIC(15,2) | - | 0 | 購入オプション行使価格 |
| 8 | termination_penalty | NUMERIC(15,2) | - | 0 | 解約ペナルティ額 |
| 9 | restoration_cost | NUMERIC(15,2) | - | 0 | 原状回復費用 |
| 10 | initial_direct_cost | NUMERIC(15,2) | - | 0 | 初期直接費用 |
| 11 | lease_incentive_received | NUMERIC(15,2) | - | 0 | 受領リースインセンティブ |
| 12 | prepaid_lease_payment | NUMERIC(15,2) | - | 0 | 前払リース料 |
| 13 | discount_rate_used | NUMERIC(8,6) | NOT NULL | - | 使用割引率 |
| 14 | discount_rate_type | VARCHAR(30) | - | - | 割引率種別（calculated_rate / incremental_borrowing_rate） |
| 15 | pv_lease_payments | NUMERIC(15,2) | NOT NULL | - | リース料総額の現在価値 |
| 16 | rou_amount | NUMERIC(15,2) | NOT NULL | - | 使用権資産の当初測定額 |
| 17 | liability_amount | NUMERIC(15,2) | NOT NULL | - | リース負債の当初測定額 |
| 18 | payment_timing | VARCHAR(10) | - | 'end' | 支払タイミング（begin=期首払い / end=期末払い） |
| 19 | lease_standalone_price | NUMERIC(15,2) | - | - | リース構成要素の独立価格（設例7対応） |
| 20 | non_lease_standalone_price | NUMERIC(15,2) | - | - | 非リース構成要素の独立価格（設例7対応） |
| 21 | calculation_memo | TEXT | - | - | 計算メモ（根拠記録用） |
| 22 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 23 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **注**: v4からの変更点 — approved_by を削除し23列（create_dt/update_dt含む）。設例7対応で lease_standalone_price / non_lease_standalone_price を追加。設例9-2対応で payment_timing を追加。

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_initial_measurement | (measurement_id) |
| FK | fk_lease_initial_meas_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| INDEX | idx_lease_initial_meas_asset | (asset_id) |
| INDEX | idx_lease_initial_meas_date | (measurement_date) |

#### DDL

```sql
-- ============================================================
-- 5.1 lease_initial_measurement（当初測定テーブル）
-- ============================================================
CREATE TABLE lease_initial_measurement (
    measurement_id             SERIAL          PRIMARY KEY,
    asset_id                   INTEGER,
    measurement_date           DATE            NOT NULL,
    fixed_payment_total        NUMERIC(15,2),
    variable_payment_estimate  NUMERIC(15,2)   DEFAULT 0,
    residual_guarantee_amount  NUMERIC(15,2)   DEFAULT 0,
    purchase_option_price      NUMERIC(15,2)   DEFAULT 0,
    termination_penalty        NUMERIC(15,2)   DEFAULT 0,
    restoration_cost           NUMERIC(15,2)   DEFAULT 0,
    initial_direct_cost        NUMERIC(15,2)   DEFAULT 0,
    lease_incentive_received   NUMERIC(15,2)   DEFAULT 0,
    prepaid_lease_payment      NUMERIC(15,2)   DEFAULT 0,
    discount_rate_used         NUMERIC(8,6)    NOT NULL,
    discount_rate_type         VARCHAR(30),
    pv_lease_payments          NUMERIC(15,2)   NOT NULL,
    rou_amount                 NUMERIC(15,2)   NOT NULL,
    liability_amount           NUMERIC(15,2)   NOT NULL,
    payment_timing             VARCHAR(10)     DEFAULT 'end',
    lease_standalone_price     NUMERIC(15,2),
    non_lease_standalone_price NUMERIC(15,2),
    calculation_memo           TEXT,
    create_dt                  TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                  TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lease_initial_meas_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT chk_lease_initial_meas_timing
        CHECK (payment_timing IN ('begin', 'end')),

    CONSTRAINT chk_lease_initial_meas_rate_type
        CHECK (discount_rate_type IN ('calculated_rate', 'incremental_borrowing_rate'))
);

CREATE INDEX idx_lease_initial_meas_asset ON lease_initial_measurement(asset_id);
CREATE INDEX idx_lease_initial_meas_date ON lease_initial_measurement(measurement_date);

COMMENT ON TABLE lease_initial_measurement IS '当初測定テーブル — リース開始日時点のPV計算構成要素と測定結果を保持';
COMMENT ON COLUMN lease_initial_measurement.discount_rate_used IS '使用割引率。NUMERIC(8,6)で小数点以下6桁まで保持（例: 0.030000 = 3.0%）';
COMMENT ON COLUMN lease_initial_measurement.payment_timing IS 'begin=期首払い（annuity-due） / end=期末払い（ordinary annuity）。設例9-2対応';
COMMENT ON COLUMN lease_initial_measurement.rou_amount IS '使用権資産 = PV + 初期直接費用 + 前払リース料 + 原状回復費用 - リースインセンティブ';
COMMENT ON COLUMN lease_initial_measurement.lease_standalone_price IS '設例7: リース/非リース構成要素の配分計算に使用する独立価格';
COMMENT ON COLUMN lease_initial_measurement.non_lease_standalone_price IS '設例7: 非リース構成要素（保守サービス等）の独立価格';
```

---

### 5.2 lease_accounting（リース会計テーブル — 15列）

期ごとの使用権資産・リース負債の残高を管理する。v4の ctb_lease_integrated から会計属性を分離し、3NF厳密化のため6カラムを削除（discount_rate, lease_term_months, payment_amount_monthly は lease_initial_measurement で管理、depreciation_method は lease_asset に移動、transition_method/transition_date は lease_transition で管理）。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | accounting_id | SERIAL | NOT NULL | (自動採番) | 会計ID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | accounting_date | DATE | NOT NULL | - | 会計基準日 |
| 4 | rou_initial_amount | NUMERIC(15,2) | - | - | 使用権資産 取得原価 |
| 5 | rou_accumulated_depreciation | NUMERIC(15,2) | - | - | 使用権資産 減価償却累計額 |
| 6 | rou_carrying_amount | NUMERIC(15,2) | - | - | 使用権資産 帳簿価額 |
| 7 | lease_liability_initial | NUMERIC(15,2) | - | - | リース負債 当初額 |
| 8 | lease_liability_balance | NUMERIC(15,2) | - | - | リース負債 残高 |
| 9 | impairment_loss | NUMERIC(15,2) | - | 0 | 減損損失累計額 |
| 10 | modified_flag | BOOLEAN | - | FALSE | 条件変更フラグ |
| 11 | remeasurement_count | INTEGER | - | 0 | 再測定回数 |
| 12 | fiscal_year | INTEGER | NOT NULL | - | 会計年度 |
| 13 | period | INTEGER | NOT NULL | - | 会計期間（1〜12） |
| 14 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 15 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **注**: 3NF厳密化により以下を削除（15列にはcreate_dt/update_dtを含む）:
> - discount_rate → lease_initial_measurement.discount_rate_used を参照
> - lease_term_months → lease_contract の contract_start_date / contract_end_date から算出
> - payment_amount_monthly → lease_payment_schedule から算出
> - depreciation_method, depreciation_start_date → lease_asset.depreciation_method に移動
> - transition_method, transition_date → lease_transition テーブルで管理

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_accounting | (accounting_id) |
| FK | fk_lease_accounting_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| UNIQUE | uq_lease_accounting_period | (asset_id, fiscal_year, period) |
| INDEX | idx_lease_accounting_asset | (asset_id) |
| INDEX | idx_lease_accounting_fy | (fiscal_year, period) |
| INDEX | idx_lease_accounting_date | (accounting_date) |

#### DDL

```sql
-- ============================================================
-- 5.2 lease_accounting（リース会計テーブル）
-- ============================================================
CREATE TABLE lease_accounting (
    accounting_id                SERIAL          PRIMARY KEY,
    asset_id                     INTEGER,
    accounting_date              DATE            NOT NULL,
    rou_initial_amount           NUMERIC(15,2),
    rou_accumulated_depreciation NUMERIC(15,2),
    rou_carrying_amount          NUMERIC(15,2),
    lease_liability_initial      NUMERIC(15,2),
    lease_liability_balance      NUMERIC(15,2),
    impairment_loss              NUMERIC(15,2)   DEFAULT 0,
    modified_flag                BOOLEAN         DEFAULT FALSE,
    remeasurement_count          INTEGER         DEFAULT 0,
    fiscal_year                  INTEGER         NOT NULL,
    period                       INTEGER         NOT NULL,
    create_dt                    TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                    TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lease_accounting_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT uq_lease_accounting_period
        UNIQUE (asset_id, fiscal_year, period),

    CONSTRAINT chk_lease_accounting_period
        CHECK (period BETWEEN 1 AND 12)
);

CREATE INDEX idx_lease_accounting_asset ON lease_accounting(asset_id);
CREATE INDEX idx_lease_accounting_fy ON lease_accounting(fiscal_year, period);
CREATE INDEX idx_lease_accounting_date ON lease_accounting(accounting_date);

COMMENT ON TABLE lease_accounting IS 'リース会計テーブル — 期ごとのROU資産・リース負債残高を管理';
COMMENT ON COLUMN lease_accounting.rou_carrying_amount IS '帳簿価額 = 取得原価 - 減価償却累計額 - 減損損失累計額';
COMMENT ON COLUMN lease_accounting.modified_flag IS '条件変更（lease_remeasurement）による再測定が行われた場合TRUE';
COMMENT ON COLUMN lease_accounting.remeasurement_count IS '当該資産の累計再測定回数。監査対応用';
COMMENT ON COLUMN lease_accounting.period IS '会計期間（1=4月〜12=3月、3月決算の場合）';
```

---

### 5.3 lease_payment_schedule（リース支払スケジュールテーブル — 18列）

リース料の支払スケジュールを管理する。元本・利息の按分、支払実績（paid_flag / paid_date）、消費税計算を保持。ASBJ第34号 設例9（利息法による按分）の計算結果を格納。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | schedule_id | SERIAL | NOT NULL | (自動採番) | スケジュールID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | payment_seq | INTEGER | NOT NULL | - | 支払回数（連番） |
| 4 | payment_date | DATE | NOT NULL | - | 支払予定日 |
| 5 | payment_amount | NUMERIC(15,2) | NOT NULL | - | 支払金額（税抜） |
| 6 | principal_portion | NUMERIC(15,2) | - | - | 元本返済額 |
| 7 | interest_portion | NUMERIC(15,2) | - | - | 利息費用額 |
| 8 | balance_after | NUMERIC(15,2) | - | - | 支払後リース負債残高 |
| 9 | payment_type | VARCHAR(20) | - | - | 支払種別（fixed / variable / residual_guarantee） |
| 10 | payment_timing | VARCHAR(10) | - | 'end' | 支払タイミング（begin=期首 / end=期末） |
| 11 | paid_flag | BOOLEAN | - | FALSE | 支払済フラグ |
| 12 | paid_date | DATE | - | - | 実際支払日 |
| 13 | invoice_no | VARCHAR(30) | - | - | 請求書番号 |
| 14 | tax_amount | NUMERIC(15,2) | - | 0 | 消費税額 |
| 15 | tax_rate | NUMERIC(5,4) | - | - | 消費税率（例: 0.1000 = 10%） |
| 16 | remarks | VARCHAR(500) | - | - | 備考 |
| 17 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 18 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **注**: 18列にcreate_dt/update_dt含む。

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_payment_schedule | (schedule_id) |
| FK | fk_lease_payment_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| UNIQUE | uq_lease_payment_seq | (asset_id, payment_seq) |
| INDEX | idx_lease_payment_asset | (asset_id) |
| INDEX | idx_lease_payment_date | (payment_date) |
| INDEX | idx_lease_payment_unpaid | (paid_flag, payment_date) WHERE paid_flag = FALSE |

#### DDL

```sql
-- ============================================================
-- 5.3 lease_payment_schedule（リース支払スケジュールテーブル）
-- ============================================================
CREATE TABLE lease_payment_schedule (
    schedule_id      SERIAL          PRIMARY KEY,
    asset_id         INTEGER,
    payment_seq      INTEGER         NOT NULL,
    payment_date     DATE            NOT NULL,
    payment_amount   NUMERIC(15,2)   NOT NULL,
    principal_portion NUMERIC(15,2),
    interest_portion NUMERIC(15,2),
    balance_after    NUMERIC(15,2),
    payment_type     VARCHAR(20),
    payment_timing   VARCHAR(10)     DEFAULT 'end',
    paid_flag        BOOLEAN         DEFAULT FALSE,
    paid_date        DATE,
    invoice_no       VARCHAR(30),
    tax_amount       NUMERIC(15,2)   DEFAULT 0,
    tax_rate         NUMERIC(5,4),
    remarks          VARCHAR(500),
    create_dt        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lease_payment_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT uq_lease_payment_seq
        UNIQUE (asset_id, payment_seq),

    CONSTRAINT chk_lease_payment_type
        CHECK (payment_type IN ('fixed', 'variable', 'residual_guarantee')),

    CONSTRAINT chk_lease_payment_timing
        CHECK (payment_timing IN ('begin', 'end'))
);

CREATE INDEX idx_lease_payment_asset ON lease_payment_schedule(asset_id);
CREATE INDEX idx_lease_payment_date ON lease_payment_schedule(payment_date);
CREATE INDEX idx_lease_payment_unpaid ON lease_payment_schedule(paid_flag, payment_date)
    WHERE paid_flag = FALSE;

COMMENT ON TABLE lease_payment_schedule IS 'リース支払スケジュールテーブル — リース料の元本・利息按分と支払実績を管理';
COMMENT ON COLUMN lease_payment_schedule.principal_portion IS '利息法により計算された元本返済額';
COMMENT ON COLUMN lease_payment_schedule.interest_portion IS '利息法により計算された利息費用額。期首残高 * 割引率で算出';
COMMENT ON COLUMN lease_payment_schedule.balance_after IS '当該支払後のリース負債残高。前回残高 - 元本返済額';
COMMENT ON COLUMN lease_payment_schedule.payment_type IS 'fixed=固定リース料 / variable=変動リース料 / residual_guarantee=残価保証';
COMMENT ON COLUMN lease_payment_schedule.tax_rate IS '消費税率。NUMERIC(5,4)で小数点以下4桁（例: 0.1000 = 10%）';
```

---

### 5.4 lease_variable_payment（変動リース料テーブル — 10列）

変動リース料の実績値を管理する。指数連動（CPI等）・業績連動・使用量連動の3種別に対応。ASBJ第34号 設例13（変動リース料）・設例16（指数変動による再測定）に対応。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | variable_id | SERIAL | NOT NULL | (自動採番) | 変動リース料ID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | payment_date | DATE | NOT NULL | - | 支払対象日 |
| 4 | variable_type | VARCHAR(30) | NOT NULL | - | 変動種別（index_linked / performance_linked / usage_linked） |
| 5 | base_index_value | NUMERIC(15,6) | - | - | 基準指数値 |
| 6 | current_index_value | NUMERIC(15,6) | - | - | 当期指数値 |
| 7 | calculated_amount | NUMERIC(15,2) | NOT NULL | - | 算出金額 |
| 8 | remarks | VARCHAR(500) | - | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_variable_payment | (variable_id) |
| FK | fk_lease_variable_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| INDEX | idx_lease_variable_asset | (asset_id) |
| INDEX | idx_lease_variable_date | (payment_date) |
| INDEX | idx_lease_variable_type | (variable_type) |

#### DDL

```sql
-- ============================================================
-- 5.4 lease_variable_payment（変動リース料テーブル）
-- ============================================================
CREATE TABLE lease_variable_payment (
    variable_id          SERIAL          PRIMARY KEY,
    asset_id             INTEGER,
    payment_date         DATE            NOT NULL,
    variable_type        VARCHAR(30)     NOT NULL,
    base_index_value     NUMERIC(15,6),
    current_index_value  NUMERIC(15,6),
    calculated_amount    NUMERIC(15,2)   NOT NULL,
    remarks              VARCHAR(500),
    create_dt            TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt            TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lease_variable_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT chk_lease_variable_type
        CHECK (variable_type IN ('index_linked', 'performance_linked', 'usage_linked'))
);

CREATE INDEX idx_lease_variable_asset ON lease_variable_payment(asset_id);
CREATE INDEX idx_lease_variable_date ON lease_variable_payment(payment_date);
CREATE INDEX idx_lease_variable_type ON lease_variable_payment(variable_type);

COMMENT ON TABLE lease_variable_payment IS '変動リース料テーブル — 指数連動・業績連動・使用量連動のリース料実績を管理';
COMMENT ON COLUMN lease_variable_payment.variable_type IS 'index_linked=指数連動(CPI等) / performance_linked=業績連動 / usage_linked=使用量連動';
COMMENT ON COLUMN lease_variable_payment.base_index_value IS '契約開始時の基準指数値。NUMERIC(15,6)で精度保持';
COMMENT ON COLUMN lease_variable_payment.current_index_value IS '当期の指数値。base_index_value との比率でcalculated_amountを算出';
COMMENT ON COLUMN lease_variable_payment.calculated_amount IS '変動リース料算出額 = 基本リース料 * (current_index_value / base_index_value)';
```

---

## 6. スケジュール層テーブル定義

### 6.1 amortization_schedule（償却スケジュールテーブル — 20列）

月次/期次の使用権資産償却・リース負債返済の明細を管理する。減価償却額・利息費用・元本返済を期ごとに記録し、仕訳生成のソースとなる。減損・追加・除却・為替調整の発生額も保持（性能上の理由による許容された非正規化）。

#### カラム定義

| # | カラム名 | データ型 | NOT NULL | DEFAULT | 説明 |
|---|---|---|---|---|---|
| 1 | schedule_id | SERIAL | NOT NULL | (自動採番) | スケジュールID（PK） |
| 2 | asset_id | INTEGER | - | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | fiscal_year | INTEGER | NOT NULL | - | 会計年度 |
| 4 | period | INTEGER | NOT NULL | - | 会計期間（1〜12） |
| 5 | schedule_date | DATE | NOT NULL | - | スケジュール基準日 |
| 6 | depreciation_amount | NUMERIC(15,2) | - | - | 当期減価償却額 |
| 7 | accumulated_depreciation | NUMERIC(15,2) | - | - | 減価償却累計額 |
| 8 | rou_carrying_amount | NUMERIC(15,2) | - | - | 使用権資産帳簿価額 |
| 9 | interest_expense | NUMERIC(15,2) | - | - | 当期利息費用 |
| 10 | principal_repayment | NUMERIC(15,2) | - | - | 当期元本返済額 |
| 11 | liability_balance | NUMERIC(15,2) | - | - | リース負債残高 |
| 12 | payment_amount | NUMERIC(15,2) | - | - | 当期支払額 |
| 13 | impairment_in_period | NUMERIC(15,2) | - | 0 | 当期減損損失 |
| 14 | rou_addition | NUMERIC(15,2) | - | 0 | 当期ROU追加額（再測定増加分） |
| 15 | rou_disposal | NUMERIC(15,2) | - | 0 | 当期ROU除却額 |
| 16 | fx_adjustment | NUMERIC(15,2) | - | 0 | 為替換算調整額 |
| 17 | journal_generated_flag | BOOLEAN | - | FALSE | 仕訳生成済フラグ |
| 18 | remarks | VARCHAR(500) | - | - | 備考 |
| 19 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 20 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **注**: rou_addition / rou_disposal は lease_remeasurement から導出可能だが、月次集計・開示注記生成の利便性を考慮し許容された非正規化（分析レポート判定済み）。

#### インデックス・制約

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_amortization_schedule | (schedule_id) |
| FK | fk_amortization_asset | (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE |
| UNIQUE | uq_amortization_period | (asset_id, fiscal_year, period) |
| INDEX | idx_amortization_asset | (asset_id) |
| INDEX | idx_amortization_fy | (fiscal_year, period) |
| INDEX | idx_amortization_date | (schedule_date) |
| INDEX | idx_amortization_journal | (journal_generated_flag) WHERE journal_generated_flag = FALSE |

#### DDL

```sql
-- ============================================================
-- 6.1 amortization_schedule（償却スケジュールテーブル）
-- ============================================================
CREATE TABLE amortization_schedule (
    schedule_id              SERIAL          PRIMARY KEY,
    asset_id                 INTEGER,
    fiscal_year              INTEGER         NOT NULL,
    period                   INTEGER         NOT NULL,
    schedule_date            DATE            NOT NULL,
    depreciation_amount      NUMERIC(15,2),
    accumulated_depreciation NUMERIC(15,2),
    rou_carrying_amount      NUMERIC(15,2),
    interest_expense         NUMERIC(15,2),
    principal_repayment      NUMERIC(15,2),
    liability_balance        NUMERIC(15,2),
    payment_amount           NUMERIC(15,2),
    impairment_in_period     NUMERIC(15,2)   DEFAULT 0,
    rou_addition             NUMERIC(15,2)   DEFAULT 0,
    rou_disposal             NUMERIC(15,2)   DEFAULT 0,
    fx_adjustment            NUMERIC(15,2)   DEFAULT 0,
    journal_generated_flag   BOOLEAN         DEFAULT FALSE,
    remarks                  VARCHAR(500),
    create_dt                TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_amortization_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,

    CONSTRAINT uq_amortization_period
        UNIQUE (asset_id, fiscal_year, period),

    CONSTRAINT chk_amortization_period
        CHECK (period BETWEEN 1 AND 12)
);

CREATE INDEX idx_amortization_asset ON amortization_schedule(asset_id);
CREATE INDEX idx_amortization_fy ON amortization_schedule(fiscal_year, period);
CREATE INDEX idx_amortization_date ON amortization_schedule(schedule_date);
CREATE INDEX idx_amortization_journal ON amortization_schedule(journal_generated_flag)
    WHERE journal_generated_flag = FALSE;

COMMENT ON TABLE amortization_schedule IS '償却スケジュールテーブル — 月次/期次のROU償却・リース負債返済明細';
COMMENT ON COLUMN amortization_schedule.depreciation_amount IS '当期減価償却額。定額法: ROU取得原価 / リース期間月数';
COMMENT ON COLUMN amortization_schedule.interest_expense IS '当期利息費用。利息法: 期首リース負債残高 * 割引率 / 12';
COMMENT ON COLUMN amortization_schedule.principal_repayment IS '当期元本返済額 = 支払額 - 利息費用';
COMMENT ON COLUMN amortization_schedule.rou_addition IS '再測定による使用権資産増加額（許容された非正規化）';
COMMENT ON COLUMN amortization_schedule.rou_disposal IS '除却・解約による使用権資産減少額（許容された非正規化）';
COMMENT ON COLUMN amortization_schedule.fx_adjustment IS '外貨建てリースの為替換算調整額（将来拡張用）';
COMMENT ON COLUMN amortization_schedule.journal_generated_flag IS '仕訳生成済フラグ。v_monthly_journal_pending ビューのフィルタ条件';
```

---

## 7. イベント層テーブル定義

イベント層は、リース契約のライフサイクルにおいて発生するイベント（再測定、条件変更評価、経過措置適用、セール・アンド・リースバック）を記録する。各イベントは lease_asset への FK を持ち、ON DELETE CASCADE で資産削除時に連動削除される。

---

### 7.1 lease_remeasurement（再測定履歴）— 18列

リース負債の再測定イベントを記録する。ASBJ第34号 設例15-2～15-5（条件変更に伴う再測定）、設例16（指数変動による再測定）に対応。前後値を両方保持することで監査証跡を確保する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | remeasurement_id | SERIAL | NOT NULL | - | 再測定ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | remeasurement_date | DATE | NOT NULL | - | 再測定日 |
| 4 | trigger_type | VARCHAR(30) | NOT NULL | - | トリガー種別（term_change / purchase_option_change / index_change / residual_guarantee_change / scope_change） |
| 5 | old_lease_term | INTEGER | NULL | - | 変更前リース期間（月） |
| 6 | new_lease_term | INTEGER | NULL | - | 変更後リース期間（月） |
| 7 | old_discount_rate | NUMERIC(8,6) | NULL | - | 変更前割引率 |
| 8 | new_discount_rate | NUMERIC(8,6) | NULL | - | 変更後割引率 |
| 9 | old_liability | NUMERIC(15,2) | NULL | - | 変更前リース負債残高 |
| 10 | new_liability | NUMERIC(15,2) | NULL | - | 変更後リース負債残高 |
| 11 | liability_adjustment | NUMERIC(15,2) | NULL | - | リース負債調整額（new - old） |
| 12 | rou_adjustment | NUMERIC(15,2) | NULL | - | 使用権資産調整額 |
| 13 | old_payment | NUMERIC(15,2) | NULL | - | 変更前リース料（月額） |
| 14 | new_payment | NUMERIC(15,2) | NULL | - | 変更後リース料（月額） |
| 15 | approved_by | VARCHAR(50) | NULL | - | 承認者 |
| 16 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 17 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 18 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_remeasurement | (remeasurement_id) |
| FK | fk_remeasurement_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| CHECK | ck_remeasurement_trigger | trigger_type IN ('term_change', 'purchase_option_change', 'index_change', 'residual_guarantee_change', 'scope_change') |
| INDEX | idx_remeasurement_asset | (asset_id) |
| INDEX | idx_remeasurement_date | (remeasurement_date) |
| INDEX | idx_remeasurement_trigger | (trigger_type) |

#### DDL

```sql
CREATE TABLE lease_remeasurement (
    remeasurement_id    SERIAL          PRIMARY KEY,
    asset_id            INTEGER         NOT NULL,
    remeasurement_date  DATE            NOT NULL,
    trigger_type        VARCHAR(30)     NOT NULL,
    old_lease_term      INTEGER,
    new_lease_term      INTEGER,
    old_discount_rate   NUMERIC(8,6),
    new_discount_rate   NUMERIC(8,6),
    old_liability       NUMERIC(15,2),
    new_liability       NUMERIC(15,2),
    liability_adjustment NUMERIC(15,2),
    rou_adjustment      NUMERIC(15,2),
    old_payment         NUMERIC(15,2),
    new_payment         NUMERIC(15,2),
    approved_by         VARCHAR(50),
    remarks             VARCHAR(500),
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_remeasurement_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT ck_remeasurement_trigger
        CHECK (trigger_type IN ('term_change', 'purchase_option_change', 'index_change',
                                'residual_guarantee_change', 'scope_change'))
);

CREATE INDEX idx_remeasurement_asset   ON lease_remeasurement(asset_id);
CREATE INDEX idx_remeasurement_date    ON lease_remeasurement(remeasurement_date);
CREATE INDEX idx_remeasurement_trigger ON lease_remeasurement(trigger_type);

COMMENT ON TABLE lease_remeasurement IS 'リース負債再測定履歴（設例15-2～16対応）';
```

---

### 7.2 lease_modification_assessment（条件変更評価）— 10列

リース条件変更時の「独立リース判定」を記録する。ASBJ第34号 設例15-1に対応。条件変更が独立リースとして扱われるか否かの評価結果と根拠を保持する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | assessment_id | SERIAL | NOT NULL | - | 評価ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | assessment_date | DATE | NOT NULL | - | 評価日 |
| 4 | modification_description | TEXT | NULL | - | 変更内容の説明 |
| 5 | is_separate_lease | BOOLEAN | NOT NULL | - | 独立リース判定結果（TRUE=独立リース） |
| 6 | separate_lease_asset_id | INTEGER | NULL | - | 独立リースの新資産ID（FK→lease_asset） |
| 7 | assessment_basis | TEXT | NULL | - | 判定根拠 |
| 8 | approved_by | VARCHAR(50) | NULL | - | 承認者 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_mod_assessment | (assessment_id) |
| FK | fk_mod_assessment_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| FK | fk_mod_assessment_separate | separate_lease_asset_id → lease_asset(asset_id) |
| INDEX | idx_mod_assessment_asset | (asset_id) |
| INDEX | idx_mod_assessment_date | (assessment_date) |

#### DDL

```sql
CREATE TABLE lease_modification_assessment (
    assessment_id           SERIAL          PRIMARY KEY,
    asset_id                INTEGER         NOT NULL,
    assessment_date         DATE            NOT NULL,
    modification_description TEXT,
    is_separate_lease       BOOLEAN         NOT NULL,
    separate_lease_asset_id INTEGER,
    assessment_basis        TEXT,
    approved_by             VARCHAR(50),
    create_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_mod_assessment_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT fk_mod_assessment_separate
        FOREIGN KEY (separate_lease_asset_id) REFERENCES lease_asset(asset_id)
);

CREATE INDEX idx_mod_assessment_asset ON lease_modification_assessment(asset_id);
CREATE INDEX idx_mod_assessment_date  ON lease_modification_assessment(assessment_date);

COMMENT ON TABLE lease_modification_assessment IS 'リース条件変更評価（設例15-1: 独立リース判定）';
```

---

### 7.3 lease_transition（経過措置）— 14列

ASBJ第34号の適用開始時における経過措置の適用方法と調整額を記録する。設例20に対応。完全遡及法、修正遡及法、簡便法の3方式をサポートする。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | transition_id | SERIAL | NOT NULL | - | 経過措置ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | transition_method | VARCHAR(20) | NOT NULL | - | 経過措置方式（full_retrospective / modified_retrospective / simplified） |
| 4 | previous_standard_classification | VARCHAR(30) | NULL | - | 旧基準での分類（finance_lease / operating_lease 等） |
| 5 | previous_lease_liability | NUMERIC(15,2) | NULL | - | 旧基準でのリース負債残高 |
| 6 | previous_rou_amount | NUMERIC(15,2) | NULL | - | 旧基準での使用権資産相当額 |
| 7 | transition_adjustment_equity | NUMERIC(15,2) | NULL | - | 利益剰余金調整額 |
| 8 | transition_date | DATE | NOT NULL | - | 経過措置適用日 |
| 9 | cumulative_effect | NUMERIC(15,2) | NULL | - | 累積的影響額 |
| 10 | practical_expedient_used | BOOLEAN | NULL | FALSE | 実務上の便法使用フラグ |
| 11 | grandfathered_flag | BOOLEAN | NULL | FALSE | 経過措置適用フラグ（旧基準適用リース） |
| 12 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 13 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 14 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_transition | (transition_id) |
| FK | fk_transition_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| CHECK | ck_transition_method | transition_method IN ('full_retrospective', 'modified_retrospective', 'simplified') |
| INDEX | idx_transition_asset | (asset_id) |
| INDEX | idx_transition_date | (transition_date) |

#### DDL

```sql
CREATE TABLE lease_transition (
    transition_id                   SERIAL          PRIMARY KEY,
    asset_id                        INTEGER         NOT NULL,
    transition_method               VARCHAR(20)     NOT NULL,
    previous_standard_classification VARCHAR(30),
    previous_lease_liability        NUMERIC(15,2),
    previous_rou_amount             NUMERIC(15,2),
    transition_adjustment_equity    NUMERIC(15,2),
    transition_date                 DATE            NOT NULL,
    cumulative_effect               NUMERIC(15,2),
    practical_expedient_used        BOOLEAN         DEFAULT FALSE,
    grandfathered_flag              BOOLEAN         DEFAULT FALSE,
    remarks                         VARCHAR(500),
    create_dt                       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_transition_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT ck_transition_method
        CHECK (transition_method IN ('full_retrospective', 'modified_retrospective', 'simplified'))
);

CREATE INDEX idx_transition_asset ON lease_transition(asset_id);
CREATE INDEX idx_transition_date  ON lease_transition(transition_date);

COMMENT ON TABLE lease_transition IS '経過措置適用記録（設例20対応）';
```

---

### 7.4 sale_leaseback（セール・アンド・リースバック）— 14列

セール・アンド・リースバック取引の売却条件とリースバック条件を記録する。売却損益の認識・繰延の判定情報を保持する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | slb_id | SERIAL | NOT NULL | - | セール・リースバックID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | sale_date | DATE | NOT NULL | - | 売却日 |
| 4 | sale_price | NUMERIC(15,2) | NOT NULL | - | 売却価格 |
| 5 | carrying_amount_at_sale | NUMERIC(15,2) | NOT NULL | - | 売却時帳簿価額 |
| 6 | gain_on_sale | NUMERIC(15,2) | NULL | - | 売却益 |
| 7 | deferred_gain | NUMERIC(15,2) | NULL | - | 繰延売却益 |
| 8 | is_market_terms | BOOLEAN | NULL | - | 市場条件判定フラグ |
| 9 | buyer_lessor_id | INTEGER | NULL | - | 買主・貸主ID（FK→company） |
| 10 | leaseback_term | INTEGER | NULL | - | リースバック期間（月） |
| 11 | leaseback_payment | NUMERIC(15,2) | NULL | - | リースバック月額リース料 |
| 12 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 13 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 14 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_sale_leaseback | (slb_id) |
| FK | fk_slb_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| FK | fk_slb_buyer_lessor | buyer_lessor_id → company(company_id) |
| INDEX | idx_slb_asset | (asset_id) |
| INDEX | idx_slb_sale_date | (sale_date) |

#### DDL

```sql
CREATE TABLE sale_leaseback (
    slb_id                  SERIAL          PRIMARY KEY,
    asset_id                INTEGER         NOT NULL,
    sale_date               DATE            NOT NULL,
    sale_price              NUMERIC(15,2)   NOT NULL,
    carrying_amount_at_sale NUMERIC(15,2)   NOT NULL,
    gain_on_sale            NUMERIC(15,2),
    deferred_gain           NUMERIC(15,2),
    is_market_terms         BOOLEAN,
    buyer_lessor_id         INTEGER,
    leaseback_term          INTEGER,
    leaseback_payment       NUMERIC(15,2),
    remarks                 VARCHAR(500),
    create_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_slb_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT fk_slb_buyer_lessor
        FOREIGN KEY (buyer_lessor_id) REFERENCES company(company_id)
);

CREATE INDEX idx_slb_asset     ON sale_leaseback(asset_id);
CREATE INDEX idx_slb_sale_date ON sale_leaseback(sale_date);

COMMENT ON TABLE sale_leaseback IS 'セール・アンド・リースバック取引';
```

---

## 8. 貸手・サブリース層テーブル定義

貸手・サブリース層は、貸手としてのリース会計処理（設例9-3、設例12: 製造販売型リース）およびサブリース関係（設例18-1/18-2、設例19: 転リース）を管理する。

---

### 8.1 lease_lessor（貸手リース）— 12列

貸手としてのリース会計情報を管理する。ファイナンス・リース（リース投資資産・リース料総額・未実現利息等）とオペレーティング・リースの双方に対応。設例9-3（見積残存価額）、設例12（製造販売型リース）をカバーする。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | lessor_id | SERIAL | NOT NULL | - | 貸手リースID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | lessor_type | VARCHAR(20) | NOT NULL | - | 貸手リース分類（finance / operating） |
| 4 | gross_investment | NUMERIC(15,2) | NULL | - | リース投資資産総額（リース料総額 + 無保証残存価額） |
| 5 | unearned_finance_income | NUMERIC(15,2) | NULL | - | 未実現利息収益 |
| 6 | net_investment | NUMERIC(15,2) | NULL | - | リース投資資産純額（= gross - unearned） |
| 7 | residual_value_unguaranteed | NUMERIC(15,2) | NULL | 0 | 無保証残存価額（設例9-3対応） |
| 8 | selling_profit | NUMERIC(15,2) | NULL | 0 | 販売利益（設例12: 製造販売型リース） |
| 9 | initial_direct_cost_lessor | NUMERIC(15,2) | NULL | 0 | 貸手の初期直接コスト |
| 10 | cost_of_asset_sold | NUMERIC(15,2) | NULL | 0 | 売上原価（設例12: 製造販売型リース） |
| 11 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 12 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_lessor | (lessor_id) |
| FK | fk_lessor_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| CHECK | ck_lessor_type | lessor_type IN ('finance', 'operating') |
| INDEX | idx_lessor_asset | (asset_id) |
| INDEX | idx_lessor_type | (lessor_type) |

#### DDL

```sql
CREATE TABLE lease_lessor (
    lessor_id                   SERIAL          PRIMARY KEY,
    asset_id                    INTEGER         NOT NULL,
    lessor_type                 VARCHAR(20)     NOT NULL,
    gross_investment            NUMERIC(15,2),
    unearned_finance_income     NUMERIC(15,2),
    net_investment              NUMERIC(15,2),
    residual_value_unguaranteed NUMERIC(15,2)   DEFAULT 0,
    selling_profit              NUMERIC(15,2)   DEFAULT 0,
    initial_direct_cost_lessor  NUMERIC(15,2)   DEFAULT 0,
    cost_of_asset_sold          NUMERIC(15,2)   DEFAULT 0,
    create_dt                   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_lessor_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT ck_lessor_type
        CHECK (lessor_type IN ('finance', 'operating'))
);

CREATE INDEX idx_lessor_asset ON lease_lessor(asset_id);
CREATE INDEX idx_lessor_type  ON lease_lessor(lessor_type);

COMMENT ON TABLE lease_lessor IS '貸手リース会計情報（設例9-3, 12対応）';
```

---

### 8.2 sublease_relationship（サブリース関係）— 11列

ヘッドリースとサブリースの関係を管理する。中間リース会社（intermediate lessor）として、ヘッドリースの使用権資産を原資産としたサブリースを記録する。設例18-1（ファイナンス・サブリース）、設例18-2（オペレーティング・サブリース）、設例19（転リース）に対応。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | sublease_id | SERIAL | NOT NULL | - | サブリースID（PK） |
| 2 | head_lease_asset_id | INTEGER | NOT NULL | - | ヘッドリース資産ID（FK→lease_asset） |
| 3 | sublease_asset_id | INTEGER | NOT NULL | - | サブリース資産ID（FK→lease_asset） |
| 4 | sublease_classification | VARCHAR(20) | NOT NULL | - | サブリース分類（finance / operating） |
| 5 | sublease_start_date | DATE | NOT NULL | - | サブリース開始日 |
| 6 | sublease_end_date | DATE | NOT NULL | - | サブリース終了日 |
| 7 | intermediate_lessor_flag | BOOLEAN | NULL | TRUE | 中間リース会社フラグ |
| 8 | net_investment_sublease | NUMERIC(15,2) | NULL | - | サブリース投資純額（ファイナンス分類の場合） |
| 9 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 10 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 11 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_sublease_relationship | (sublease_id) |
| FK | fk_sublease_head | head_lease_asset_id → lease_asset(asset_id) |
| FK | fk_sublease_sub | sublease_asset_id → lease_asset(asset_id) |
| CHECK | ck_sublease_classification | sublease_classification IN ('finance', 'operating') |
| INDEX | idx_sublease_head | (head_lease_asset_id) |
| INDEX | idx_sublease_sub | (sublease_asset_id) |

#### DDL

```sql
CREATE TABLE sublease_relationship (
    sublease_id             SERIAL          PRIMARY KEY,
    head_lease_asset_id     INTEGER         NOT NULL,
    sublease_asset_id       INTEGER         NOT NULL,
    sublease_classification VARCHAR(20)     NOT NULL,
    sublease_start_date     DATE            NOT NULL,
    sublease_end_date       DATE            NOT NULL,
    intermediate_lessor_flag BOOLEAN        DEFAULT TRUE,
    net_investment_sublease NUMERIC(15,2),
    remarks                 VARCHAR(500),
    create_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt               TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_sublease_head
        FOREIGN KEY (head_lease_asset_id) REFERENCES lease_asset(asset_id),
    CONSTRAINT fk_sublease_sub
        FOREIGN KEY (sublease_asset_id) REFERENCES lease_asset(asset_id),
    CONSTRAINT ck_sublease_classification
        CHECK (sublease_classification IN ('finance', 'operating'))
);

CREATE INDEX idx_sublease_head ON sublease_relationship(head_lease_asset_id);
CREATE INDEX idx_sublease_sub  ON sublease_relationship(sublease_asset_id);

COMMENT ON TABLE sublease_relationship IS 'サブリース関係（設例18-1/18-2, 19対応）';
```

---

## 9. 付帯層テーブル定義

付帯層は、リース契約に付随する付帯要素（インセンティブ、原状回復義務、敷金・建設協力金、部門配賦）を管理する。これらはリース負債・使用権資産の測定に影響を与えるが、リース契約の主要要素とは独立して管理される。

---

### 9.1 lease_incentive（リース・インセンティブ）— 10列

貸手から借手に提供されるリース・インセンティブ（フリーレント、改装費補助、移転費補助等）を管理する。インセンティブ額は使用権資産の測定時に控除される。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | incentive_id | SERIAL | NOT NULL | - | インセンティブID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | incentive_type | VARCHAR(30) | NOT NULL | - | インセンティブ種別（free_rent / improvement_allowance / relocation_allowance） |
| 4 | incentive_amount | NUMERIC(15,2) | NOT NULL | - | インセンティブ額 |
| 5 | received_date | DATE | NULL | - | 受領日 |
| 6 | amortization_period | INTEGER | NULL | - | 償却期間（月） |
| 7 | amortized_amount | NUMERIC(15,2) | NULL | 0 | 償却済額 |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_incentive | (incentive_id) |
| FK | fk_incentive_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| CHECK | ck_incentive_type | incentive_type IN ('free_rent', 'improvement_allowance', 'relocation_allowance') |
| INDEX | idx_incentive_asset | (asset_id) |

#### DDL

```sql
CREATE TABLE lease_incentive (
    incentive_id        SERIAL          PRIMARY KEY,
    asset_id            INTEGER         NOT NULL,
    incentive_type      VARCHAR(30)     NOT NULL,
    incentive_amount    NUMERIC(15,2)   NOT NULL,
    received_date       DATE,
    amortization_period INTEGER,
    amortized_amount    NUMERIC(15,2)   DEFAULT 0,
    remarks             VARCHAR(500),
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_incentive_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT ck_incentive_type
        CHECK (incentive_type IN ('free_rent', 'improvement_allowance', 'relocation_allowance'))
);

CREATE INDEX idx_incentive_asset ON lease_incentive(asset_id);

COMMENT ON TABLE lease_incentive IS 'リース・インセンティブ';
```

---

### 9.2 restoration_obligation（原状回復義務）— 10列

リース終了時の原状回復義務に関する見積額・現在価値を管理する。原状回復義務の現在価値は使用権資産の当初測定額に加算される。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | obligation_id | SERIAL | NOT NULL | - | 原状回復義務ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | estimated_cost | NUMERIC(15,2) | NOT NULL | - | 見積原状回復コスト |
| 4 | discount_rate | NUMERIC(8,6) | NULL | - | 割引率 |
| 5 | pv_amount | NUMERIC(15,2) | NULL | - | 現在価値 |
| 6 | recognition_date | DATE | NOT NULL | - | 認識日 |
| 7 | settlement_date | DATE | NULL | - | 決済予定日 |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_restoration_obligation | (obligation_id) |
| FK | fk_restoration_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| INDEX | idx_restoration_asset | (asset_id) |
| INDEX | idx_restoration_settlement | (settlement_date) |

#### DDL

```sql
CREATE TABLE restoration_obligation (
    obligation_id   SERIAL          PRIMARY KEY,
    asset_id        INTEGER         NOT NULL,
    estimated_cost  NUMERIC(15,2)   NOT NULL,
    discount_rate   NUMERIC(8,6),
    pv_amount       NUMERIC(15,2),
    recognition_date DATE           NOT NULL,
    settlement_date DATE,
    remarks         VARCHAR(500),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_restoration_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE
);

CREATE INDEX idx_restoration_asset      ON restoration_obligation(asset_id);
CREATE INDEX idx_restoration_settlement ON restoration_obligation(settlement_date);

COMMENT ON TABLE restoration_obligation IS '原状回復義務';
```

---

### 9.3 lease_deposit（敷金・建設協力金）— 11列

敷金、建設協力金、権利金等の保証金的な預け金を管理する。ASBJ第34号 設例14（建設協力金）に対応。建設協力金はリース負債とは別にPV計算が必要であり、専用テーブルで管理する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | deposit_id | SERIAL | NOT NULL | - | 預け金ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | deposit_type | VARCHAR(30) | NOT NULL | - | 預け金種別（security_deposit / construction_cooperation / key_money） |
| 4 | deposit_amount | NUMERIC(15,2) | NOT NULL | - | 預け金額（名目額） |
| 5 | deposit_pv | NUMERIC(15,2) | NULL | - | 現在価値 |
| 6 | return_amount | NUMERIC(15,2) | NULL | - | 返還予定額 |
| 7 | return_date | DATE | NULL | - | 返還予定日 |
| 8 | amortization_method | VARCHAR(20) | NULL | - | 償却方法（straight_line / effective_interest 等） |
| 9 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 10 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 11 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_lease_deposit | (deposit_id) |
| FK | fk_deposit_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| CHECK | ck_deposit_type | deposit_type IN ('security_deposit', 'construction_cooperation', 'key_money') |
| INDEX | idx_deposit_asset | (asset_id) |
| INDEX | idx_deposit_type | (deposit_type) |

#### DDL

```sql
CREATE TABLE lease_deposit (
    deposit_id          SERIAL          PRIMARY KEY,
    asset_id            INTEGER         NOT NULL,
    deposit_type        VARCHAR(30)     NOT NULL,
    deposit_amount      NUMERIC(15,2)   NOT NULL,
    deposit_pv          NUMERIC(15,2),
    return_amount       NUMERIC(15,2),
    return_date         DATE,
    amortization_method VARCHAR(20),
    remarks             VARCHAR(500),
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_deposit_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT ck_deposit_type
        CHECK (deposit_type IN ('security_deposit', 'construction_cooperation', 'key_money'))
);

CREATE INDEX idx_deposit_asset ON lease_deposit(asset_id);
CREATE INDEX idx_deposit_type  ON lease_deposit(deposit_type);

COMMENT ON TABLE lease_deposit IS '敷金・建設協力金（設例14対応）';
```

---

### 9.4 dept_allocation（部門配賦）— 10列

1つのリース資産を複数部門で共用する場合の費用配賦比率を管理する。配賦比率は期間別に有効期間を持ち、組織変更時の配賦比率変更に対応できる。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | allocation_id | SERIAL | NOT NULL | - | 配賦ID（PK） |
| 2 | asset_id | INTEGER | NOT NULL | - | 資産ID（FK→lease_asset ON DELETE CASCADE） |
| 3 | dept_code | VARCHAR(10) | NOT NULL | - | 部署コード（FK→department） |
| 4 | allocation_ratio | NUMERIC(5,4) | NOT NULL | - | 配賦比率（0 < ratio <= 1） |
| 5 | effective_from | DATE | NOT NULL | - | 有効開始日 |
| 6 | effective_to | DATE | NULL | - | 有効終了日（NULL=無期限） |
| 7 | cost_center | VARCHAR(20) | NULL | - | コストセンター |
| 8 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 9 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 10 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_dept_allocation | (allocation_id) |
| FK | fk_allocation_asset | asset_id → lease_asset(asset_id) ON DELETE CASCADE |
| FK | fk_allocation_dept | dept_code → department(dept_code) |
| CHECK | ck_allocation_ratio | allocation_ratio > 0 AND allocation_ratio <= 1 |
| INDEX | idx_allocation_asset | (asset_id) |
| INDEX | idx_allocation_dept | (dept_code) |
| INDEX | idx_allocation_effective | (effective_from, effective_to) |

#### DDL

```sql
CREATE TABLE dept_allocation (
    allocation_id   SERIAL          PRIMARY KEY,
    asset_id        INTEGER         NOT NULL,
    dept_code       VARCHAR(10)     NOT NULL,
    allocation_ratio NUMERIC(5,4)   NOT NULL,
    effective_from  DATE            NOT NULL,
    effective_to    DATE,
    cost_center     VARCHAR(20),
    remarks         VARCHAR(500),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_allocation_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    CONSTRAINT fk_allocation_dept
        FOREIGN KEY (dept_code) REFERENCES department(dept_code),
    CONSTRAINT ck_allocation_ratio
        CHECK (allocation_ratio > 0 AND allocation_ratio <= 1)
);

CREATE INDEX idx_allocation_asset     ON dept_allocation(asset_id);
CREATE INDEX idx_allocation_dept      ON dept_allocation(dept_code);
CREATE INDEX idx_allocation_effective ON dept_allocation(effective_from, effective_to);

COMMENT ON TABLE dept_allocation IS '部門配賦';
```

---

## 10. 仕訳層テーブル定義

仕訳層は、リース会計に関連する全25種の仕訳パターン（当初認識、減価償却、利息費用、支払、再測定、減損、条件変更、経過措置、除却、サブリース認識、貸手当初、貸手収益、セール・リースバック、敷金償却、インセンティブ償却、原状回復認識、原状回復決済、為替評価替、振替、期末調整、開示計算、年度末締め、戻入、修正、その他）の仕訳実績を管理する。テンプレートヘッダ/明細の分離設計により、仕訳自動生成の柔軟性を確保する。

---

### 10.1 journal_header（仕訳ヘッダ）— 14列

仕訳伝票の基本情報を管理する。1仕訳ヘッダに対し、複数の仕訳明細（journal_detail）が紐づく。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | journal_id | SERIAL | NOT NULL | - | 仕訳ID（PK） |
| 2 | asset_id | INTEGER | NULL | - | 資産ID（FK→lease_asset）※開示計算等は資産紐づけなし |
| 3 | journal_date | DATE | NOT NULL | - | 仕訳日 |
| 4 | journal_type | VARCHAR(30) | NOT NULL | - | 仕訳種別（25種、下記参照） |
| 5 | fiscal_year | INTEGER | NOT NULL | - | 会計年度 |
| 6 | period | INTEGER | NOT NULL | - | 会計期間（月: 1-12） |
| 7 | template_id | INTEGER | NULL | - | テンプレートID（FK→journal_template_header） |
| 8 | total_debit | NUMERIC(15,2) | NULL | - | 借方合計 |
| 9 | total_credit | NUMERIC(15,2) | NULL | - | 貸方合計 |
| 10 | posted_flag | BOOLEAN | NULL | FALSE | 転記済フラグ |
| 11 | posted_date | DATE | NULL | - | 転記日 |
| 12 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 13 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 14 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **journal_type 25種一覧**: initial_recognition / depreciation / interest / payment / remeasurement / impairment / modification / transition / disposal / sublease_recognition / lessor_initial / lessor_income / sale_leaseback / deposit_amortization / incentive_amortization / restoration_recognition / restoration_settlement / fx_revaluation / reclassification / period_end_adjustment / disclosure_calc / year_end_closing / reversal / correction / other

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_journal_header | (journal_id) |
| FK | fk_journal_asset | asset_id → lease_asset(asset_id) |
| FK | fk_journal_template | template_id → journal_template_header(template_id) |
| INDEX | idx_journal_asset | (asset_id) |
| INDEX | idx_journal_date | (journal_date) |
| INDEX | idx_journal_type | (journal_type) |
| INDEX | idx_journal_fiscal | (fiscal_year, period) |
| INDEX | idx_journal_posted | (posted_flag) |

#### DDL

```sql
CREATE TABLE journal_header (
    journal_id      SERIAL          PRIMARY KEY,
    asset_id        INTEGER,
    journal_date    DATE            NOT NULL,
    journal_type    VARCHAR(30)     NOT NULL,
    fiscal_year     INTEGER         NOT NULL,
    period          INTEGER         NOT NULL,
    template_id     INTEGER,
    total_debit     NUMERIC(15,2),
    total_credit    NUMERIC(15,2),
    posted_flag     BOOLEAN         DEFAULT FALSE,
    posted_date     DATE,
    remarks         VARCHAR(500),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_journal_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id),
    CONSTRAINT fk_journal_template
        FOREIGN KEY (template_id) REFERENCES journal_template_header(template_id)
);

CREATE INDEX idx_journal_asset  ON journal_header(asset_id);
CREATE INDEX idx_journal_date   ON journal_header(journal_date);
CREATE INDEX idx_journal_type   ON journal_header(journal_type);
CREATE INDEX idx_journal_fiscal ON journal_header(fiscal_year, period);
CREATE INDEX idx_journal_posted ON journal_header(posted_flag);

COMMENT ON TABLE journal_header IS '仕訳ヘッダ（25種仕訳パターン対応）';
```

---

### 10.2 journal_detail（仕訳明細）— 13列

仕訳伝票の借方/貸方明細行を管理する。1仕訳ヘッダに対し複数行で構成される。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | detail_id | SERIAL | NOT NULL | - | 仕訳明細ID（PK） |
| 2 | journal_id | INTEGER | NOT NULL | - | 仕訳ID（FK→journal_header ON DELETE CASCADE） |
| 3 | line_no | INTEGER | NOT NULL | - | 行番号 |
| 4 | account_code | VARCHAR(10) | NOT NULL | - | 勘定科目コード（FK→gl_account） |
| 5 | account_name | VARCHAR(100) | NULL | - | 勘定科目名称（非正規化: 帳票出力用） |
| 6 | sub_account | VARCHAR(20) | NULL | - | 補助科目 |
| 7 | debit_amount | NUMERIC(15,2) | NULL | 0 | 借方金額 |
| 8 | credit_amount | NUMERIC(15,2) | NULL | 0 | 貸方金額 |
| 9 | dept_code | VARCHAR(10) | NULL | - | 部署コード |
| 10 | tax_code | VARCHAR(10) | NULL | - | 税コード |
| 11 | description | VARCHAR(200) | NULL | - | 摘要 |
| 12 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 13 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_journal_detail | (detail_id) |
| FK | fk_jd_header | journal_id → journal_header(journal_id) ON DELETE CASCADE |
| FK | fk_jd_account | account_code → gl_account(account_code) |
| UNIQUE | uq_jd_journal_line | (journal_id, line_no) |
| INDEX | idx_jd_journal | (journal_id) |
| INDEX | idx_jd_account | (account_code) |

#### DDL

```sql
CREATE TABLE journal_detail (
    detail_id       SERIAL          PRIMARY KEY,
    journal_id      INTEGER         NOT NULL,
    line_no         INTEGER         NOT NULL,
    account_code    VARCHAR(10)     NOT NULL,
    account_name    VARCHAR(100),
    sub_account     VARCHAR(20),
    debit_amount    NUMERIC(15,2)   DEFAULT 0,
    credit_amount   NUMERIC(15,2)   DEFAULT 0,
    dept_code       VARCHAR(10),
    tax_code        VARCHAR(10),
    description     VARCHAR(200),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_jd_header
        FOREIGN KEY (journal_id) REFERENCES journal_header(journal_id) ON DELETE CASCADE,
    CONSTRAINT fk_jd_account
        FOREIGN KEY (account_code) REFERENCES gl_account(account_code),
    CONSTRAINT uq_jd_journal_line
        UNIQUE (journal_id, line_no)
);

CREATE INDEX idx_jd_journal ON journal_detail(journal_id);
CREATE INDEX idx_jd_account ON journal_detail(account_code);

COMMENT ON TABLE journal_detail IS '仕訳明細';
```

---

### 10.3 journal_template_header（仕訳テンプレートヘッダ）— 9列

仕訳自動生成のテンプレートヘッダ。v4の journal_template を3NF準拠のためヘッダ/明細に分離した設計。25種の仕訳イベントに対応するテンプレートを定義する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | template_id | SERIAL | NOT NULL | - | テンプレートID（PK） |
| 2 | journal_type | VARCHAR(30) | NOT NULL | - | 仕訳種別（journal_header.journal_type と対応） |
| 3 | template_name | VARCHAR(100) | NOT NULL | - | テンプレート名称 |
| 4 | auto_generate_flag | BOOLEAN | NULL | TRUE | 自動生成フラグ |
| 5 | priority | INTEGER | NULL | 0 | 優先度（同一種別に複数テンプレートがある場合） |
| 6 | effective_from | DATE | NULL | - | 有効開始日 |
| 7 | effective_to | DATE | NULL | - | 有効終了日（NULL=無期限） |
| 8 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 9 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_journal_template_header | (template_id) |
| INDEX | idx_jth_type | (journal_type) |
| INDEX | idx_jth_effective | (effective_from, effective_to) |

#### DDL

```sql
CREATE TABLE journal_template_header (
    template_id         SERIAL          PRIMARY KEY,
    journal_type        VARCHAR(30)     NOT NULL,
    template_name       VARCHAR(100)    NOT NULL,
    auto_generate_flag  BOOLEAN         DEFAULT TRUE,
    priority            INTEGER         DEFAULT 0,
    effective_from      DATE,
    effective_to        DATE,
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_jth_type      ON journal_template_header(journal_type);
CREATE INDEX idx_jth_effective ON journal_template_header(effective_from, effective_to);

COMMENT ON TABLE journal_template_header IS '仕訳テンプレートヘッダ（25種対応）';
```

---

### 10.4 journal_template_line（仕訳テンプレート明細）— 11列

仕訳テンプレートの借方/貸方の各明細行。account_source により勘定科目の動的解決を行い、amount_source により金額の算出元を指定する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | line_id | SERIAL | NOT NULL | - | テンプレート明細ID（PK） |
| 2 | template_id | INTEGER | NOT NULL | - | テンプレートID（FK→journal_template_header ON DELETE CASCADE） |
| 3 | line_no | INTEGER | NOT NULL | - | 行番号 |
| 4 | debit_credit | VARCHAR(10) | NOT NULL | - | 借方/貸方区分（debit / credit） |
| 5 | account_source | VARCHAR(50) | NOT NULL | - | 勘定科目参照先（例: `category:asset_account_cd`, `fixed:1100`） |
| 6 | amount_source | VARCHAR(50) | NOT NULL | - | 金額算出元（例: `schedule:interest_expense`, `ctb:initial_rou_asset`） |
| 7 | amount_sign | INTEGER | NULL | 1 | 金額符号（1=正, -1=反転） |
| 8 | description_pattern | VARCHAR(200) | NULL | - | 摘要パターン（テンプレート変数展開対応） |
| 9 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 10 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 11 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

> **account_source の仕組み**: `category:asset_account_cd` の場合、対象リース資産の asset_category から asset_category テーブルの asset_account_cd カラムの値を勘定科目として使用する。`fixed:1100` の場合は固定で勘定科目コード '1100' を使用する。

> **amount_source の仕組み**: `schedule:interest_expense` の場合、amortization_schedule テーブルの当該期間の interest_expense カラムの値を使用する。`measurement:initial_rou_amount` の場合は lease_initial_measurement テーブルの値を参照する。

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_journal_template_line | (line_id) |
| FK | fk_jtl_header | template_id → journal_template_header(template_id) ON DELETE CASCADE |
| UNIQUE | uq_jtl_template_line | (template_id, line_no) |
| CHECK | ck_jtl_debit_credit | debit_credit IN ('debit', 'credit') |
| CHECK | ck_jtl_amount_sign | amount_sign IN (1, -1) |
| INDEX | idx_jtl_template | (template_id) |

#### DDL

```sql
CREATE TABLE journal_template_line (
    line_id             SERIAL          PRIMARY KEY,
    template_id         INTEGER         NOT NULL,
    line_no             INTEGER         NOT NULL,
    debit_credit        VARCHAR(10)     NOT NULL,
    account_source      VARCHAR(50)     NOT NULL,
    amount_source       VARCHAR(50)     NOT NULL,
    amount_sign         INTEGER         DEFAULT 1,
    description_pattern VARCHAR(200),
    remarks             VARCHAR(500),
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_jtl_header
        FOREIGN KEY (template_id) REFERENCES journal_template_header(template_id) ON DELETE CASCADE,
    CONSTRAINT uq_jtl_template_line
        UNIQUE (template_id, line_no),
    CONSTRAINT ck_jtl_debit_credit
        CHECK (debit_credit IN ('debit', 'credit')),
    CONSTRAINT ck_jtl_amount_sign
        CHECK (amount_sign IN (1, -1))
);

CREATE INDEX idx_jtl_template ON journal_template_line(template_id);

COMMENT ON TABLE journal_template_line IS '仕訳テンプレート明細';
```

---

## 11. 外部連携・開示層テーブル定義

外部連携・開示層は、外部システム（シサンM7、JSM10等）とのデータ連携マッピングおよび開示注記用のスナップショットデータを管理する。

---

### 11.1 external_mapping（外部システムマッピング）— 12列

外部システムとの連携キー・連携フィールドのマッピング情報を管理する。シサンM7、JSM10等との双方向データ連携を実現する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | mapping_id | SERIAL | NOT NULL | - | マッピングID（PK） |
| 2 | asset_id | INTEGER | NULL | - | 資産ID（FK→lease_asset） |
| 3 | external_system | VARCHAR(30) | NOT NULL | - | 外部システム識別子（M7 / JSM10 / other） |
| 4 | external_key | VARCHAR(50) | NOT NULL | - | 外部システムキー |
| 5 | external_field | VARCHAR(50) | NULL | - | 外部システムフィールド名 |
| 6 | mapped_value | VARCHAR(200) | NULL | - | マッピング値 |
| 7 | sync_direction | VARCHAR(10) | NULL | 'export' | 同期方向（import / export / both） |
| 8 | last_sync_date | TIMESTAMP | NULL | - | 最終同期日時 |
| 9 | sync_status | VARCHAR(20) | NULL | 'pending' | 同期状態（pending / synced / error） |
| 10 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 11 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 12 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_external_mapping | (mapping_id) |
| FK | fk_extmap_asset | asset_id → lease_asset(asset_id) |
| CHECK | ck_extmap_direction | sync_direction IN ('import', 'export', 'both') |
| CHECK | ck_extmap_status | sync_status IN ('pending', 'synced', 'error') |
| INDEX | idx_extmap_asset | (asset_id) |
| INDEX | idx_extmap_system | (external_system) |
| INDEX | idx_extmap_key | (external_system, external_key) |

#### DDL

```sql
CREATE TABLE external_mapping (
    mapping_id      SERIAL          PRIMARY KEY,
    asset_id        INTEGER,
    external_system VARCHAR(30)     NOT NULL,
    external_key    VARCHAR(50)     NOT NULL,
    external_field  VARCHAR(50),
    mapped_value    VARCHAR(200),
    sync_direction  VARCHAR(10)     DEFAULT 'export',
    last_sync_date  TIMESTAMP,
    sync_status     VARCHAR(20)     DEFAULT 'pending',
    remarks         VARCHAR(500),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_extmap_asset
        FOREIGN KEY (asset_id) REFERENCES lease_asset(asset_id),
    CONSTRAINT ck_extmap_direction
        CHECK (sync_direction IN ('import', 'export', 'both')),
    CONSTRAINT ck_extmap_status
        CHECK (sync_status IN ('pending', 'synced', 'error'))
);

CREATE INDEX idx_extmap_asset  ON external_mapping(asset_id);
CREATE INDEX idx_extmap_system ON external_mapping(external_system);
CREATE INDEX idx_extmap_key    ON external_mapping(external_system, external_key);

COMMENT ON TABLE external_mapping IS '外部システムマッピング（M7/JSM10連携）';
```

---

### 11.2 disclosure_snapshot（開示注記スナップショット）— 17列

開示注記用の集計結果スナップショットを管理する。満期分析、使用権資産増減、リース負債増減、免除開示の各開示類型に対応し、年度別バケット（1年以内〜5年超）で金額を保持する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | snapshot_id | SERIAL | NOT NULL | - | スナップショットID（PK） |
| 2 | fiscal_year | INTEGER | NOT NULL | - | 会計年度 |
| 3 | period | INTEGER | NOT NULL | - | 会計期間（月） |
| 4 | disclosure_type | VARCHAR(30) | NOT NULL | - | 開示類型（maturity_analysis / rou_reconciliation / liability_reconciliation / exemptions） |
| 5 | category_code | VARCHAR(10) | NULL | - | 分類コード（資産分類等） |
| 6 | amount_current | NUMERIC(15,2) | NULL | 0 | 当期金額 |
| 7 | amount_1year | NUMERIC(15,2) | NULL | 0 | 1年以内 |
| 8 | amount_2year | NUMERIC(15,2) | NULL | 0 | 1年超2年以内 |
| 9 | amount_3year | NUMERIC(15,2) | NULL | 0 | 2年超3年以内 |
| 10 | amount_4year | NUMERIC(15,2) | NULL | 0 | 3年超4年以内 |
| 11 | amount_5plus | NUMERIC(15,2) | NULL | 0 | 5年超 |
| 12 | total_amount | NUMERIC(15,2) | NULL | 0 | 合計金額 |
| 13 | generated_date | TIMESTAMP | NULL | - | 生成日時 |
| 14 | approved_flag | BOOLEAN | NULL | FALSE | 承認済フラグ |
| 15 | remarks | VARCHAR(500) | NULL | - | 備考 |
| 16 | create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 作成日時 |
| 17 | update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 更新日時 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_disclosure_snapshot | (snapshot_id) |
| CHECK | ck_disclosure_type | disclosure_type IN ('maturity_analysis', 'rou_reconciliation', 'liability_reconciliation', 'exemptions') |
| INDEX | idx_disclosure_fiscal | (fiscal_year, period) |
| INDEX | idx_disclosure_type | (disclosure_type) |
| INDEX | idx_disclosure_approved | (approved_flag) |

#### DDL

```sql
CREATE TABLE disclosure_snapshot (
    snapshot_id     SERIAL          PRIMARY KEY,
    fiscal_year     INTEGER         NOT NULL,
    period          INTEGER         NOT NULL,
    disclosure_type VARCHAR(30)     NOT NULL,
    category_code   VARCHAR(10),
    amount_current  NUMERIC(15,2)   DEFAULT 0,
    amount_1year    NUMERIC(15,2)   DEFAULT 0,
    amount_2year    NUMERIC(15,2)   DEFAULT 0,
    amount_3year    NUMERIC(15,2)   DEFAULT 0,
    amount_4year    NUMERIC(15,2)   DEFAULT 0,
    amount_5plus    NUMERIC(15,2)   DEFAULT 0,
    total_amount    NUMERIC(15,2)   DEFAULT 0,
    generated_date  TIMESTAMP,
    approved_flag   BOOLEAN         DEFAULT FALSE,
    remarks         VARCHAR(500),
    create_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT ck_disclosure_type
        CHECK (disclosure_type IN ('maturity_analysis', 'rou_reconciliation',
                                   'liability_reconciliation', 'exemptions'))
);

CREATE INDEX idx_disclosure_fiscal   ON disclosure_snapshot(fiscal_year, period);
CREATE INDEX idx_disclosure_type     ON disclosure_snapshot(disclosure_type);
CREATE INDEX idx_disclosure_approved ON disclosure_snapshot(approved_flag);

COMMENT ON TABLE disclosure_snapshot IS '開示注記スナップショット';
```

---

## 12. システム管理層テーブル定義

システム管理層は、データベーススキーマのバージョン管理と監査ログを管理する。これらのテーブルはアプリケーション・テーブルとのFK関係を持たず独立して動作する。

---

### 12.1 schema_version（スキーマバージョン管理）— 6列

データベーススキーマの変更履歴を管理する。マイグレーションスクリプトの適用状況を追跡し、ロールバック可能性を担保する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | version_id | SERIAL | NOT NULL | - | バージョンID（PK） |
| 2 | version_no | VARCHAR(10) | NOT NULL | - | バージョン番号（例: '5.0.0'） |
| 3 | applied_date | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 適用日時 |
| 4 | description | VARCHAR(200) | NULL | - | 変更概要 |
| 5 | script_name | VARCHAR(100) | NULL | - | 適用スクリプト名 |
| 6 | rollback_script | VARCHAR(100) | NULL | - | ロールバックスクリプト名 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_schema_version | (version_id) |
| INDEX | idx_schema_version_no | (version_no) |

#### DDL

```sql
CREATE TABLE schema_version (
    version_id      SERIAL          PRIMARY KEY,
    version_no      VARCHAR(10)     NOT NULL,
    applied_date    TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    description     VARCHAR(200),
    script_name     VARCHAR(100),
    rollback_script VARCHAR(100)
);

CREATE INDEX idx_schema_version_no ON schema_version(version_no);

COMMENT ON TABLE schema_version IS 'スキーマバージョン管理';

-- 初期バージョン挿入
INSERT INTO schema_version (version_no, description, script_name)
VALUES ('5.0.0', 'v5.0 初期スキーマ（36テーブル + 7ビュー）', 'v5_initial_schema.sql');
```

---

### 12.2 audit_log（監査ログ）— 10列

全テーブルのデータ変更履歴を記録する。PostgreSQLのトリガー関数により自動挿入される。old_values / new_values は JSONB 型で変更前後のレコード内容を保持する。

#### カラム定義

| # | カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|---|
| 1 | log_id | SERIAL | NOT NULL | - | ログID（PK） |
| 2 | table_name | VARCHAR(50) | NOT NULL | - | 対象テーブル名 |
| 3 | record_id | INTEGER | NOT NULL | - | 対象レコードID |
| 4 | action | VARCHAR(10) | NOT NULL | - | 操作種別（INSERT / UPDATE / DELETE） |
| 5 | old_values | JSONB | NULL | - | 変更前の値 |
| 6 | new_values | JSONB | NULL | - | 変更後の値 |
| 7 | changed_by | VARCHAR(50) | NULL | - | 変更者 |
| 8 | changed_at | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | 変更日時 |
| 9 | ip_address | VARCHAR(45) | NULL | - | IPアドレス（IPv6対応） |
| 10 | remarks | VARCHAR(500) | NULL | - | 備考 |

#### 制約・インデックス

| 種別 | 名称 | 定義 |
|---|---|---|
| PK | pk_audit_log | (log_id) |
| CHECK | ck_audit_action | action IN ('INSERT', 'UPDATE', 'DELETE') |
| INDEX | idx_audit_table | (table_name) |
| INDEX | idx_audit_record | (table_name, record_id) |
| INDEX | idx_audit_changed_at | (changed_at) |
| INDEX | idx_audit_changed_by | (changed_by) |

#### DDL

```sql
CREATE TABLE audit_log (
    log_id      SERIAL          PRIMARY KEY,
    table_name  VARCHAR(50)     NOT NULL,
    record_id   INTEGER         NOT NULL,
    action      VARCHAR(10)     NOT NULL,
    old_values  JSONB,
    new_values  JSONB,
    changed_by  VARCHAR(50),
    changed_at  TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ip_address  VARCHAR(45),
    remarks     VARCHAR(500),

    CONSTRAINT ck_audit_action
        CHECK (action IN ('INSERT', 'UPDATE', 'DELETE'))
);

CREATE INDEX idx_audit_table      ON audit_log(table_name);
CREATE INDEX idx_audit_record     ON audit_log(table_name, record_id);
CREATE INDEX idx_audit_changed_at ON audit_log(changed_at);
CREATE INDEX idx_audit_changed_by ON audit_log(changed_by);

COMMENT ON TABLE audit_log IS '監査ログ（全テーブル変更追跡）';

-- 汎用監査トリガー関数
CREATE OR REPLACE FUNCTION fn_audit_trigger()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        INSERT INTO audit_log (table_name, record_id, action, new_values, changed_at)
        VALUES (TG_TABLE_NAME, NEW.id, 'INSERT', to_jsonb(NEW), CURRENT_TIMESTAMP);
        RETURN NEW;
    ELSIF TG_OP = 'UPDATE' THEN
        INSERT INTO audit_log (table_name, record_id, action, old_values, new_values, changed_at)
        VALUES (TG_TABLE_NAME, NEW.id, 'UPDATE', to_jsonb(OLD), to_jsonb(NEW), CURRENT_TIMESTAMP);
        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        INSERT INTO audit_log (table_name, record_id, action, old_values, changed_at)
        VALUES (TG_TABLE_NAME, OLD.id, 'DELETE', to_jsonb(OLD), CURRENT_TIMESTAMP);
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;
```

---

## 13. ビュー定義

7ビューの SQL 定義を以下に示す。v_ctb_export は MATERIALIZED VIEW として定義し、他のビューは通常のビューとする。

---

### 13.1 v_ctb_export（旧CTBフラットテーブル互換ビュー）— MATERIALIZED VIEW

旧 ctb_lease_integrated（103カラム）との後方互換性を提供するマテリアライズドビュー。lease_contract、lease_asset、lease_initial_measurement、lease_accounting、lease_lessor、sublease_relationship、lease_transition の LEFT JOIN で構成する。定期リフレッシュにより性能を確保する。

```sql
CREATE MATERIALIZED VIEW v_ctb_export AS
SELECT
    -- 契約基本情報
    c.contract_id,
    c.contract_no,
    c.contract_name,
    c.contract_type_code,
    c.lessor_company_id,
    c.lessee_company_id,
    c.lease_classification,
    c.contract_date,
    c.contract_start_date,
    c.contract_end_date,
    c.has_cancel_option,
    c.auto_renewal,
    c.renewal_notice_months,
    c.status,

    -- 資産基本情報
    a.asset_id,
    a.asset_no,
    a.asset_name,
    a.asset_category_code,
    a.location,
    a.quantity,
    a.useful_life_months,
    a.residual_value_guarantee,
    a.is_low_value,
    a.is_short_term,
    a.mgmt_dept_cd,
    a.depreciation_method,

    -- 当初測定情報
    im.measurement_id,
    im.measurement_date,
    im.fixed_payment_total,
    im.variable_payment_estimate,
    im.residual_guarantee_amount,
    im.purchase_option_price,
    im.termination_penalty,
    im.restoration_cost,
    im.initial_direct_cost,
    im.lease_incentive_received,
    im.prepaid_lease_payment,
    im.discount_rate_used,
    im.discount_rate_type,
    im.pv_lease_payments,
    im.rou_amount,
    im.liability_amount,
    im.payment_timing,
    im.lease_standalone_price,
    im.non_lease_standalone_price,
    im.calculation_memo,

    -- 会計情報
    ac.accounting_id,
    ac.accounting_date,
    ac.rou_initial_amount,
    ac.rou_accumulated_depreciation,
    ac.rou_carrying_amount,
    ac.lease_liability_initial,
    ac.lease_liability_balance,
    ac.impairment_loss,
    ac.modified_flag,
    ac.remeasurement_count,
    ac.fiscal_year,
    ac.period,

    -- 貸手情報
    ll.lessor_id,
    ll.lessor_type,
    ll.gross_investment,
    ll.unearned_finance_income,
    ll.net_investment,
    ll.residual_value_unguaranteed,
    ll.selling_profit,
    ll.initial_direct_cost_lessor,
    ll.cost_of_asset_sold,

    -- サブリース情報
    sr.sublease_id,
    sr.head_lease_asset_id,
    sr.sublease_asset_id,
    sr.sublease_classification,
    sr.sublease_start_date,
    sr.sublease_end_date,
    sr.intermediate_lessor_flag,
    sr.net_investment_sublease,

    -- 経過措置情報
    lt.transition_id,
    lt.transition_method,
    lt.previous_standard_classification,
    lt.previous_lease_liability,
    lt.previous_rou_amount,
    lt.transition_adjustment_equity,
    lt.transition_date,
    lt.cumulative_effect,
    lt.practical_expedient_used,
    lt.grandfathered_flag

FROM lease_contract c
INNER JOIN lease_asset a ON c.contract_id = a.contract_id
LEFT JOIN lease_initial_measurement im ON a.asset_id = im.asset_id
LEFT JOIN lease_accounting ac ON a.asset_id = ac.asset_id
LEFT JOIN lease_lessor ll ON a.asset_id = ll.asset_id
LEFT JOIN sublease_relationship sr ON a.asset_id = sr.head_lease_asset_id
LEFT JOIN lease_transition lt ON a.asset_id = lt.asset_id;

-- リフレッシュ用インデックス
CREATE UNIQUE INDEX idx_ctb_export_asset ON v_ctb_export(asset_id);
CREATE INDEX idx_ctb_export_contract ON v_ctb_export(contract_no);
CREATE INDEX idx_ctb_export_category ON v_ctb_export(asset_category_code);

-- リフレッシュコマンド（定期実行 or 保存時実行）
-- REFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export;

COMMENT ON MATERIALIZED VIEW v_ctb_export IS '旧CTBフラットテーブル互換（MATERIALIZED VIEW）';
```

---

### 13.2 v_disclosure_maturity_analysis（満期分析ビュー）

リース料支払の満期分析を、1年以内 / 1-2年 / 2-3年 / 3-4年 / 4-5年 / 5年超のバケット別に集計する。

```sql
CREATE OR REPLACE VIEW v_disclosure_maturity_analysis AS
SELECT
    a.asset_category_code,
    cat.category_name,
    SUM(CASE WHEN ps.payment_date < (CURRENT_DATE + INTERVAL '1 year')
             THEN ps.payment_amount ELSE 0 END) AS amount_within_1year,
    SUM(CASE WHEN ps.payment_date >= (CURRENT_DATE + INTERVAL '1 year')
              AND ps.payment_date < (CURRENT_DATE + INTERVAL '2 years')
             THEN ps.payment_amount ELSE 0 END) AS amount_1_2year,
    SUM(CASE WHEN ps.payment_date >= (CURRENT_DATE + INTERVAL '2 years')
              AND ps.payment_date < (CURRENT_DATE + INTERVAL '3 years')
             THEN ps.payment_amount ELSE 0 END) AS amount_2_3year,
    SUM(CASE WHEN ps.payment_date >= (CURRENT_DATE + INTERVAL '3 years')
              AND ps.payment_date < (CURRENT_DATE + INTERVAL '4 years')
             THEN ps.payment_amount ELSE 0 END) AS amount_3_4year,
    SUM(CASE WHEN ps.payment_date >= (CURRENT_DATE + INTERVAL '4 years')
              AND ps.payment_date < (CURRENT_DATE + INTERVAL '5 years')
             THEN ps.payment_amount ELSE 0 END) AS amount_4_5year,
    SUM(CASE WHEN ps.payment_date >= (CURRENT_DATE + INTERVAL '5 years')
             THEN ps.payment_amount ELSE 0 END) AS amount_over_5year,
    SUM(ps.payment_amount) AS total_amount
FROM lease_asset a
INNER JOIN asset_category cat ON a.asset_category_code = cat.category_code
INNER JOIN lease_payment_schedule ps ON a.asset_id = ps.asset_id
INNER JOIN lease_contract c ON a.contract_id = c.contract_id
WHERE c.status = 'active'
  AND ps.payment_date >= CURRENT_DATE
GROUP BY a.asset_category_code, cat.category_name;

COMMENT ON VIEW v_disclosure_maturity_analysis IS '満期分析（開示注記用）';
```

---

### 13.3 v_disclosure_rou_reconciliation（使用権資産増減明細ビュー）

使用権資産の期首残高→取得→減価償却→減損→その他増減→期末残高の増減明細を集計する。

```sql
CREATE OR REPLACE VIEW v_disclosure_rou_reconciliation AS
SELECT
    a.asset_category_code,
    cat.category_name,
    am.fiscal_year,
    -- 期首残高（期首月の帳簿価額 + 当期減価償却費で逆算）
    SUM(CASE WHEN am.period = 1
             THEN am.rou_carrying_amount + am.depreciation_amount
                  + COALESCE(am.impairment_in_period, 0)
             ELSE 0 END) AS opening_balance,
    -- 当期取得
    SUM(CASE WHEN am.period = 1 THEN COALESCE(im.rou_amount, 0) ELSE 0 END) AS additions,
    -- 減価償却費
    SUM(am.depreciation_amount) AS depreciation,
    -- 減損損失
    SUM(COALESCE(am.impairment_in_period, 0)) AS impairment,
    -- その他増減（再測定による調整 + 為替調整）
    SUM(COALESCE(am.rou_addition, 0) - COALESCE(am.rou_disposal, 0)
        + COALESCE(am.fx_adjustment, 0)) AS other_changes,
    -- 期末残高
    SUM(CASE WHEN am.period = (
        SELECT MAX(am2.period) FROM amortization_schedule am2
        WHERE am2.asset_id = am.asset_id
          AND am2.fiscal_year = am.fiscal_year
    ) THEN am.rou_carrying_amount ELSE 0 END) AS closing_balance
FROM lease_asset a
INNER JOIN asset_category cat ON a.asset_category_code = cat.category_code
INNER JOIN amortization_schedule am ON a.asset_id = am.asset_id
LEFT JOIN lease_initial_measurement im ON a.asset_id = im.asset_id
GROUP BY a.asset_category_code, cat.category_name, am.fiscal_year;

COMMENT ON VIEW v_disclosure_rou_reconciliation IS '使用権資産増減明細（開示注記用）';
```

---

### 13.4 v_disclosure_liability_reconciliation（リース負債増減明細ビュー）

リース負債の期首残高→新規認識→利息費用→支払→再測定→その他増減→期末残高の増減明細を集計する。

```sql
CREATE OR REPLACE VIEW v_disclosure_liability_reconciliation AS
SELECT
    a.asset_category_code,
    cat.category_name,
    am.fiscal_year,
    -- 期首残高（期首月の負債残高 + 当期元本返済 - 当期利息で逆算）
    SUM(CASE WHEN am.period = 1
             THEN am.liability_balance + am.principal_repayment - am.interest_expense
             ELSE 0 END) AS opening_balance,
    -- 新規認識
    SUM(CASE WHEN am.period = 1 THEN COALESCE(im.liability_amount, 0) ELSE 0 END) AS new_recognition,
    -- 利息費用
    SUM(am.interest_expense) AS interest_expense,
    -- 支払額
    SUM(am.payment_amount) AS payments,
    -- 再測定調整
    COALESCE((
        SELECT SUM(r.liability_adjustment)
        FROM lease_remeasurement r
        WHERE r.asset_id = a.asset_id
          AND EXTRACT(YEAR FROM r.remeasurement_date) = am.fiscal_year
    ), 0) AS remeasurement_adjustment,
    -- 期末残高
    SUM(CASE WHEN am.period = (
        SELECT MAX(am2.period) FROM amortization_schedule am2
        WHERE am2.asset_id = am.asset_id
          AND am2.fiscal_year = am.fiscal_year
    ) THEN am.liability_balance ELSE 0 END) AS closing_balance
FROM lease_asset a
INNER JOIN asset_category cat ON a.asset_category_code = cat.category_code
INNER JOIN amortization_schedule am ON a.asset_id = am.asset_id
LEFT JOIN lease_initial_measurement im ON a.asset_id = im.asset_id
GROUP BY a.asset_category_code, cat.category_name, a.asset_id, am.fiscal_year;

COMMENT ON VIEW v_disclosure_liability_reconciliation IS 'リース負債増減明細（開示注記用）';
```

---

### 13.5 v_lease_summary（リース一覧サマリービュー）

一覧画面用のサマリービュー。契約番号、資産名称、分類、期間、使用権資産簿価、リース負債残高、ステータスを集約する。

```sql
CREATE OR REPLACE VIEW v_lease_summary AS
SELECT
    c.contract_no,
    c.contract_name,
    a.asset_id,
    a.asset_no,
    a.asset_name,
    a.asset_category_code,
    cat.category_name,
    c.lease_classification,
    c.contract_start_date,
    c.contract_end_date,
    ac.rou_carrying_amount,
    ac.lease_liability_balance,
    c.status,
    co.company_name AS lessor_name
FROM lease_contract c
INNER JOIN lease_asset a ON c.contract_id = a.contract_id
LEFT JOIN asset_category cat ON a.asset_category_code = cat.category_code
LEFT JOIN lease_accounting ac ON a.asset_id = ac.asset_id
LEFT JOIN company co ON c.lessor_company_id = co.company_id;

COMMENT ON VIEW v_lease_summary IS 'リース一覧サマリー（一覧画面用）';
```

---

### 13.6 v_monthly_journal_pending（仕訳未生成スケジュールビュー）

月次償却スケジュールのうち、仕訳が未生成のレコードを一覧する。月次バッチ処理の対象抽出に使用する。

```sql
CREATE OR REPLACE VIEW v_monthly_journal_pending AS
SELECT
    am.schedule_id,
    am.asset_id,
    a.asset_no,
    a.asset_name,
    am.fiscal_year,
    am.period,
    am.schedule_date,
    am.depreciation_amount,
    am.interest_expense,
    am.principal_repayment,
    am.payment_amount,
    am.journal_generated_flag
FROM amortization_schedule am
INNER JOIN lease_asset a ON am.asset_id = a.asset_id
INNER JOIN lease_contract c ON a.contract_id = c.contract_id
WHERE am.journal_generated_flag = FALSE
  AND c.status = 'active'
  AND am.schedule_date <= CURRENT_DATE
ORDER BY am.schedule_date, am.asset_id;

COMMENT ON VIEW v_monthly_journal_pending IS '仕訳未生成月次スケジュール一覧';
```

---

### 13.7 v_lease_expiry_alert（リース期限アラートビュー）

リース終了日が90日以内に到来する契約を一覧する。更新判断・再契約準備のアラートとして使用する。

```sql
CREATE OR REPLACE VIEW v_lease_expiry_alert AS
SELECT
    c.contract_no,
    c.contract_name,
    a.asset_id,
    a.asset_no,
    a.asset_name,
    c.contract_end_date,
    (c.contract_end_date - CURRENT_DATE) AS days_remaining,
    ac.rou_carrying_amount,
    ac.lease_liability_balance,
    c.status,
    c.auto_renewal,
    c.renewal_notice_months,
    co.company_name AS lessor_name
FROM lease_contract c
INNER JOIN lease_asset a ON c.contract_id = a.contract_id
LEFT JOIN lease_accounting ac ON a.asset_id = ac.asset_id
LEFT JOIN company co ON c.lessor_company_id = co.company_id
WHERE c.contract_end_date IS NOT NULL
  AND c.contract_end_date BETWEEN CURRENT_DATE AND (CURRENT_DATE + INTERVAL '90 days')
  AND c.status = 'active'
ORDER BY c.contract_end_date;

COMMENT ON VIEW v_lease_expiry_alert IS 'リース期限アラート（90日以内）';
```

---

## 14. ASBJ第34号 設例カバレッジマトリクス

以下は、analysis_optimal_db.md セクション2.1 のカバレッジ分析を整形したものである。

### 14.1 設例別対応状況

| 設例 | 内容 | カバー状況 | 対応テーブル | 備考 |
|---|---|---|---|---|
| 設例1 | リース識別フローチャート | 対象外 | - | アプリ層判定ロジック |
| 設例2 | 鉄道車両（特定資産） | 対象外 | - | アプリ層判定ロジック |
| 設例3 | 小売区画（特定資産） | 対象外 | - | アプリ層判定ロジック |
| 設例4 | ガス貯蔵タンク | 対象外 | - | アプリ層判定ロジック |
| 設例5 | ネットワークサービス | 対象外 | - | アプリ層判定ロジック |
| 設例6 | 電力 | 対象外 | - | アプリ層判定ロジック |
| 設例7 | リース/非リース配分 | **対応済** | lease_initial_measurement | lease_standalone_price, non_lease_standalone_price 追加 |
| 設例8 | 借手リース期間 | **対応済** | lease_contract, lease_option | 延長/解約オプションの合理的確実性判定 |
| 設例9 | 借手/貸手 所有権移転外FL | **対応済** | lease_initial_measurement, amortization_schedule, lease_lessor | PV計算、利息法、リース投資資産 |
| 設例9-2 | 前払い/後払い | **対応済** | lease_initial_measurement | payment_timing (begin/end) 追加 |
| 設例9-3 | 貸手の見積残存価額 | **対応済** | lease_lessor | residual_value_unguaranteed |
| 設例10 | 所有権移転FL | **対応済** | lease_contract, lease_asset | lease_classification で間接対応 |
| 設例11 | 残価保証 | **対応済** | lease_initial_measurement, lease_asset | residual_guarantee_amount |
| 設例12 | 製造販売型リース | **対応済** | lease_lessor | selling_profit, cost_of_asset_sold |
| 設例13 | 変動リース料 | **対応済** | lease_variable_payment, index_master | 指数連動/業績連動/使用量連動 |
| 設例14 | 建設協力金 | **対応済** | **lease_deposit** | v5で新規追加 |
| 設例15-1 | 独立リースとしての変更 | **対応済** | **lease_modification_assessment** | v5で新規追加 |
| 設例15-2 | 範囲縮小+単価増額 | **対応済** | lease_remeasurement | trigger_type = scope_change |
| 設例15-3 | 範囲拡大+縮小 | **対応済** | lease_remeasurement | 複数レコードで対応 |
| 設例15-4 | 期間延長 | **対応済** | lease_remeasurement | old_lease_term / new_lease_term |
| 設例15-5 | リース料のみ変更 | **対応済** | lease_remeasurement | old_payment / new_payment |
| 設例16 | 条件変更なし再測定 | **対応済** | lease_remeasurement | trigger_type = index_change |
| 設例17 | 重要性再評価 | 部分対応 | lease_asset | is_low_value フラグあり。再評価履歴は audit_log で追跡 |
| 設例18-1 | サブリースFL | **対応済** | sublease_relationship | sublease_classification = finance |
| 設例18-2 | サブリースOL | **対応済** | sublease_relationship | sublease_classification = operating |
| 設例19 | 転リース | 部分対応 | sublease_relationship | 転リース特殊処理は audit_log + remarks で追跡 |
| 設例20 | 経過措置 | **対応済** | lease_transition | full_retrospective / modified_retrospective / simplified |

### 14.2 カバレッジ統計

| 区分 | 件数 | 割合 |
|---|---|---|
| DB設計対象外（設例1-6: アプリ層判定） | 6 | - |
| 完全対応 | 18 | 90% |
| 部分対応（audit_log等で補完可能） | 2 | 10% |
| **対応不足** | **0** | **0%** |

> v4からの改善: lease_deposit（設例14）、lease_modification_assessment（設例15-1）の追加により、v4で不足していた2設例が完全対応となった。

---

## 15. データ移行計画（v4 → v5）

analysis_optimal_db.md セクション5.2 の移行パスに基づく。

### 15.1 移行ステップ

| # | ステップ | 内容 | 難易度 | 推定工数 |
|---|---|---|---|---|
| 1 | マスタテーブル移行 | m_company → company, m_supplier → supplier 等。カラム名の変換のみ | 低 | 0.5人日 |
| 2 | 契約・資産分離 | ctb_lease_integrated → lease_contract + lease_asset。contract_no で契約を統合、property_no で資産を分離 | 中 | 2人日 |
| 3 | 会計情報分離 | ctb_lease_integrated の会計カラム → lease_initial_measurement + lease_accounting | 中 | 2人日 |
| 4 | スケジュール移行 | tw_lease_amortization_schedule → amortization_schedule。カラム名変換 | 低 | 0.5人日 |
| 5 | 仕訳移行 | tw_journal_header/detail → journal_header/detail。構造同等 | 低 | 0.5人日 |
| 6 | EAV移行 | m_asset_class_field/m_asset_attr → asset_class_field/asset_attribute | 低 | 0.5人日 |
| 7 | 再測定履歴移行 | ctb_remeasurement_history → lease_remeasurement | 中 | 1人日 |
| 8 | ビュー作成 | v_ctb_export 等の互換ビュー7本作成 | 高 | 3人日 |
| - | **合計** | - | - | **10人日** |

### 15.2 移行リスクと対策

| リスク | 影響度 | 対策 |
|---|---|---|
| ctb_id → asset_id の参照キー変更 | 高 | external_mapping テーブルで旧キーマッピングを保持。外部連携先にキー変更を通知 |
| 103カラムの分散による既存クエリ破壊 | 高 | v_ctb_export マテリアライズドビューで互換性確保。段階的に直接テーブル参照に移行 |
| EAV PIVOTの性能問題 | 中 | v_ctb_export をマテリアライズドビュー化し定期リフレッシュ |
| トランザクション複雑化 | 中 | Repository パターンで SaveLeaseContract() メソッドにカプセル化 |
| 移行中のデータ不整合 | 中 | 移行スクリプトをトランザクションで囲み、検証SQLで件数・合計額を照合 |

### 15.3 移行検証チェックリスト

| # | 検証項目 | 検証方法 |
|---|---|---|
| 1 | レコード件数一致 | v4各テーブルのCOUNT(*) = v5対応テーブルのCOUNT(*) |
| 2 | 金額合計一致 | 主要金額カラムのSUM比較（初期ROU、リース負債、月額リース料等） |
| 3 | FK整合性 | 全FK制約が有効であること確認（SET CONSTRAINTS ALL IMMEDIATE） |
| 4 | v_ctb_export整合性 | v_ctb_export の全カラムが旧 ctb_lease_integrated と値一致 |
| 5 | 仕訳整合性 | journal_header の total_debit = total_credit（貸借一致） |
| 6 | 開示注記整合性 | disclosure_snapshot の total_amount = 各バケット合計 |

---

## 付録A. 命名規約

### A.1 テーブル命名

| 規則 | 説明 | 例 |
|---|---|---|
| スネークケース | 全テーブル名・カラム名にスネークケース（小文字+アンダースコア）を使用 | lease_contract, asset_id |
| プレフィックスなし | v4の層別プレフィックス（m_, ctb_, tw_）を廃止。PostgreSQLのスキーマ機能で層分離を推奨 | company (旧 m_company) |
| 単数形 | テーブル名は単数形を使用 | lease_asset (非 lease_assets) |
| 意味のある名称 | 略語を避け、意味の明確な名称を使用 | amortization_schedule (非 amort_sch) |

### A.2 カラム命名

| 規則 | 説明 | 例 |
|---|---|---|
| PK | テーブル名の単数概念 + `_id` | contract_id, asset_id |
| FK | 参照先テーブルのPKカラム名をそのまま使用 | asset_id (→ lease_asset.asset_id) |
| フラグ | `is_` + 形容詞/状態 | is_separate_lease, is_market_terms |
| 日付 | `_date` サフィックス | remeasurement_date, sale_date |
| 日時 | `_dt` サフィックス（タイムスタンプ） | create_dt, update_dt |
| 金額 | 内容を示す名称（`_amount` は任意） | deposit_amount, gain_on_sale |
| コード | `_code` または `_cd` サフィックス | account_code, dept_code |

### A.3 インデックス命名

| 規則 | パターン | 例 |
|---|---|---|
| PRIMARY KEY | pk_{テーブル名} | pk_lease_remeasurement |
| FOREIGN KEY | fk_{略称}_{参照先} | fk_remeasurement_asset |
| UNIQUE | uq_{テーブル略称}_{カラム} | uq_jd_journal_line |
| INDEX | idx_{テーブル略称}_{カラム} | idx_remeasurement_date |
| CHECK | ck_{テーブル略称}_{項目} | ck_remeasurement_trigger |

### A.4 ビュー命名

| 規則 | 説明 | 例 |
|---|---|---|
| `v_` プレフィックス | 通常ビューに付与 | v_lease_summary |
| `v_disclosure_` | 開示注記ビューに付与 | v_disclosure_maturity_analysis |
| MATERIALIZED VIEW | 通常ビューと同じ `v_` プレフィックス | v_ctb_export |

---

## 付録B. データ型ガイドライン

### B.1 データ型選定基準

| 用途 | データ型 | 精度 | 備考 |
|---|---|---|---|
| 主キー（自動連番） | SERIAL | - | PostgreSQL固有。将来的に GENERATED ALWAYS AS IDENTITY への移行を検討 |
| 外部キー（整数） | INTEGER | - | 参照先のSERIAL PKに対応 |
| 金額 | NUMERIC(15,2) | 小数2桁 | 最大13桁の整数部 + 2桁の小数部。兆円単位まで対応 |
| 利率・割引率 | NUMERIC(8,6) | 小数6桁 | 例: 0.035000 = 3.5%。小数6桁で0.0001%単位まで表現 |
| 配賦比率 | NUMERIC(5,4) | 小数4桁 | 0.0000 〜 1.0000。100.00%を0.01%単位で表現 |
| 短いコード | VARCHAR(10) | - | 部署コード、勘定科目コード等 |
| 中程度のコード | VARCHAR(20-30) | - | ステータスコード、分類コード等 |
| 名称（短） | VARCHAR(50-100) | - | 勘定科目名称、属性ラベル等 |
| 名称（長） | VARCHAR(200) | - | 契約名称、資産名称等 |
| 備考 | VARCHAR(500) | - | 全テーブル共通 |
| 長文テキスト | TEXT | - | 判定根拠、変更内容説明等。上限なし |
| 日付 | DATE | - | 年月日。時刻不要の場合 |
| タイムスタンプ | TIMESTAMP | - | 年月日時分秒。create_dt / update_dt / 同期日時等 |
| フラグ | BOOLEAN | - | TRUE/FALSE。NOT NULL推奨、DEFAULTを設定 |
| 月数・期間 | INTEGER | - | リース期間（月）、償却期間（月）等 |
| JSON | JSONB | - | 監査ログの変更前後値のみ。アプリテーブルではJSONB不使用（CrudHelper互換性） |

### B.2 全テーブル共通カラム

以下のカラムは全テーブル（schema_version、audit_log を除く）に必須とする。

| カラム名 | データ型 | Null | Default | 説明 |
|---|---|---|---|---|
| create_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | レコード作成日時。INSERT時に自動設定 |
| update_dt | TIMESTAMP | NOT NULL | CURRENT_TIMESTAMP | レコード更新日時。UPDATE時にトリガーで自動更新 |

```sql
-- update_dt 自動更新トリガー関数
CREATE OR REPLACE FUNCTION fn_update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW.update_dt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- 各テーブルへのトリガー適用例
CREATE TRIGGER trg_lease_remeasurement_update
    BEFORE UPDATE ON lease_remeasurement
    FOR EACH ROW
    EXECUTE FUNCTION fn_update_timestamp();
```

### B.3 NULL許容方針

| 区分 | 方針 | 例 |
|---|---|---|
| 主キー | NOT NULL（必須） | remeasurement_id |
| 外部キー（必須参照） | NOT NULL | asset_id（イベントは必ず資産に紐づく） |
| 外部キー（任意参照） | NULL許容 | separate_lease_asset_id（独立リース判定の場合のみ） |
| 業務必須項目 | NOT NULL | remeasurement_date, trigger_type |
| 前後値ペア | NULL許容 | old_lease_term / new_lease_term（トリガー種別によっては不要） |
| 金額（デフォルト0） | NULL許容 + DEFAULT 0 | amortized_amount, residual_value_unguaranteed |
| フラグ（デフォルト値あり） | NULL許容 + DEFAULT | posted_flag DEFAULT FALSE |
| 備考 | NULL許容 | remarks |
| create_dt / update_dt | NOT NULL + DEFAULT | 全テーブル共通 |
