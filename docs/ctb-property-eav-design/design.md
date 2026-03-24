# CTB物件テーブル設計書（EAVパターン）

## 1. テーブル構成図（ER図テキスト）

```
┌─────────────────────────────┐
│   m_asset_category          │     ┌───────────────────────────────────┐
│─────────────────────────────│     │   m_property_attribute_def        │
│ PK asset_category_cd        │     │───────────────────────────────────│
│    asset_category_nm        │◄─┐  │ PK attr_def_id (SERIAL)           │
│    (勘定科目列 省略)        │  │  │ FK asset_category_cd              │──► m_asset_category
│    ...                      │  │  │    attr_cd       VARCHAR(30)      │
└─────────────────────────────┘  │  │    attr_nm       VARCHAR(100)     │
                                 │  │    data_type     VARCHAR(20)      │
                                 │  │    display_type  VARCHAR(30)      │
                                 │  │    max_length    INTEGER          │
                                 │  │    is_required   BOOLEAN          │
                                 │  │    sort_order    INTEGER          │
                                 │  │    default_value TEXT             │
                                 │  └───────────────────────────────────┘
                                 │            ▲
                                 │            │ FK
┌─────────────────────────────┐  │  ┌───────────────────────────────────┐
│   ctb_lease_integrated      │  │  │   ctb_property_attribute (EAV)    │
│─────────────────────────────│  │  │───────────────────────────────────│
│ PK ctb_id (SERIAL)          │  │  │ PK prop_attr_id (SERIAL)          │
│    contract_no              │  │  │ FK property_id                    │──► ctb_property
│    property_no              │  │  │ FK attr_def_id                    │──► m_property_attribute_def
│    contract_name            │  │  │    attribute_value TEXT            │
│    contract_type_cd         │  │  │    create_dt / update_dt          │
│    supplier_cd              │  │  └───────────────────────────────────┘
│    mgmt_dept_cd             │  │            ▲
│    lease_start_date         │  │            │ FK
│    lease_end_date           │  │  ┌───────────────────────────────────┐
│    free_rent_months         │  │  │   ctb_property（物件マスタ）      │
│    lease_term_months        │  │  │───────────────────────────────────│
│    asset_no                 │  │  │ PK property_id (SERIAL)           │
│    asset_category_cd ───────│──┘  │ FK ctb_id                         │──► ctb_lease_integrated
│    asset_name               │     │ FK asset_category_cd              │──► m_asset_category
│    company_name             │     │    property_no   INTEGER          │
│    install_location         │     │    asset_no      VARCHAR(20)      │
│    remarks                  │     │    asset_name    VARCHAR(200)     │
│    monthly_payment          │     │    company_name  VARCHAR(100)     │
│    lease_depreciation       │     │    install_location VARCHAR(200)  │
│    total_payment            │     │    remarks       VARCHAR(500)     │
│    split_status             │     │    create_dt / update_dt          │
│   *** re_*, vh_*, oa_* 削除 │     └───────────────────────────────────┘
│    create_dt / update_dt    │              ▲
└─────────────────────────────┘              │ FK
         ▲                         ┌────────────────────┐
         │ FK                      │ ctb_dept_allocation │
         └─────────────────────────│ (既存・変更なし)    │
                                   └────────────────────┘

リレーション:
  ctb_lease_integrated  1 ──── N  ctb_property         (1契約に複数物件)
  ctb_property          1 ──── N  ctb_property_attribute (1物件に複数属性値)
  m_property_attribute_def 1 ── N  ctb_property_attribute (定義→値)
  m_asset_category      1 ──── N  m_property_attribute_def (種別→属性定義)
  m_asset_category      1 ──── N  ctb_property           (種別→物件)
```

---

## 2. 新設テーブルDDL

### 2.1 ctb_property（物件マスタ）

```sql
-- ==============================================
-- ctb_property: 物件マスタ
-- property_no で ctb_lease_integrated 内の一意性を担保
-- ==============================================
CREATE TABLE IF NOT EXISTS ctb_property (
    property_id         SERIAL          NOT NULL,
    ctb_id              INTEGER         NOT NULL,
    property_no         INTEGER         NOT NULL DEFAULT 1,
    asset_category_cd   VARCHAR(10)     NOT NULL,
    asset_no            VARCHAR(20)     NULL,
    asset_name          VARCHAR(200)    NULL,
    company_name        VARCHAR(100)    NULL,
    install_location    VARCHAR(200)    NULL,
    remarks             VARCHAR(500)    NULL,
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_property PRIMARY KEY (property_id),
    CONSTRAINT uq_ctb_property_no UNIQUE (ctb_id, property_no),
    CONSTRAINT fk_property_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE,
    CONSTRAINT fk_property_asset_category FOREIGN KEY (asset_category_cd)
        REFERENCES m_asset_category (asset_category_cd)
);

CREATE INDEX IF NOT EXISTS idx_ctb_property_ctb_id
    ON ctb_property (ctb_id);

CREATE INDEX IF NOT EXISTS idx_ctb_property_asset_no
    ON ctb_property (asset_no);

COMMENT ON TABLE ctb_property IS '物件マスタ: 1契約に対して1〜N物件を管理';
COMMENT ON COLUMN ctb_property.property_id IS '物件ID（サロゲートキー）';
COMMENT ON COLUMN ctb_property.ctb_id IS 'CTB統合テーブルの契約ID';
COMMENT ON COLUMN ctb_property.property_no IS '契約内物件連番（1始まり）';
COMMENT ON COLUMN ctb_property.asset_category_cd IS '資産カテゴリコード（m_asset_category.asset_category_cd）';
```

### 2.2 ctb_property_attribute（物件属性値テーブル - EAV）

```sql
-- ==============================================
-- ctb_property_attribute: EAV汎用テーブル
-- 種別固有情報を key-value で格納
-- ==============================================
CREATE TABLE IF NOT EXISTS ctb_property_attribute (
    prop_attr_id        SERIAL          NOT NULL,
    property_id         INTEGER         NOT NULL,
    attr_def_id         INTEGER         NOT NULL,
    attribute_value     TEXT            NULL,
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_property_attribute PRIMARY KEY (prop_attr_id),
    CONSTRAINT uq_property_attr UNIQUE (property_id, attr_def_id),
    CONSTRAINT fk_prop_attr_property FOREIGN KEY (property_id)
        REFERENCES ctb_property (property_id) ON DELETE CASCADE,
    CONSTRAINT fk_prop_attr_def FOREIGN KEY (attr_def_id)
        REFERENCES m_property_attribute_def (attr_def_id)
);

CREATE INDEX IF NOT EXISTS idx_prop_attr_property_id
    ON ctb_property_attribute (property_id);

CREATE INDEX IF NOT EXISTS idx_prop_attr_def_id
    ON ctb_property_attribute (attr_def_id);

COMMENT ON TABLE ctb_property_attribute IS '物件属性値（EAV）: 種別固有情報をテキストで保持';
COMMENT ON COLUMN ctb_property_attribute.attribute_value IS '属性値（全てTEXT格納。data_typeに基づきアプリ側で変換）';
```

### 2.3 m_property_attribute_def（物件属性定義マスタ）

```sql
-- ==============================================
-- m_property_attribute_def: 属性定義マスタ
-- ユーザーがカスタマイズ可能な項目定義
-- ==============================================
CREATE TABLE IF NOT EXISTS m_property_attribute_def (
    attr_def_id         SERIAL          NOT NULL,
    asset_category_cd   VARCHAR(10)     NOT NULL,
    attr_cd             VARCHAR(30)     NOT NULL,
    attr_nm             VARCHAR(100)    NOT NULL,
    data_type           VARCHAR(20)     NOT NULL DEFAULT 'TEXT',
    display_type        VARCHAR(30)     NOT NULL DEFAULT 'TEXTBOX',
    max_length          INTEGER         NULL,
    is_required         BOOLEAN         NOT NULL DEFAULT FALSE,
    sort_order          INTEGER         NOT NULL DEFAULT 0,
    default_value       TEXT            NULL,
    remarks             VARCHAR(500)    NULL,
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_m_property_attribute_def PRIMARY KEY (attr_def_id),
    CONSTRAINT uq_attr_def_category_cd UNIQUE (asset_category_cd, attr_cd),
    CONSTRAINT fk_attr_def_asset_category FOREIGN KEY (asset_category_cd)
        REFERENCES m_asset_category (asset_category_cd),
    CONSTRAINT chk_data_type CHECK (data_type IN ('TEXT', 'DATE', 'NUMERIC', 'BOOLEAN', 'INTEGER')),
    CONSTRAINT chk_display_type CHECK (display_type IN ('TEXTBOX', 'DATEPICKER', 'NUMERICUPDOWN', 'CHECKBOX', 'COMBOBOX', 'TEXTAREA'))
);

CREATE INDEX IF NOT EXISTS idx_attr_def_category
    ON m_property_attribute_def (asset_category_cd, sort_order);

COMMENT ON TABLE m_property_attribute_def IS '物件属性定義マスタ: 資産カテゴリごとの動的項目定義';
COMMENT ON COLUMN m_property_attribute_def.data_type IS 'データ型: TEXT/DATE/NUMERIC/BOOLEAN/INTEGER';
COMMENT ON COLUMN m_property_attribute_def.display_type IS '表示コントロール: TEXTBOX/DATEPICKER/NUMERICUPDOWN/CHECKBOX/COMBOBOX/TEXTAREA';
COMMENT ON COLUMN m_property_attribute_def.sort_order IS '画面表示順（小さい方が上）';
```

> **DDL実行順序**: `m_property_attribute_def` → `ctb_property` → `ctb_property_attribute`
> （m_property_attribute_defへのFK参照があるため先に作成する）

---

## 3. 既存テーブル改修

### 3.1 ctb_lease_integrated の変更点

**削除するカラム（16列）**: 種別固有情報はctb_property + ctb_property_attributeへ移行

```sql
-- ctb_lease_integrated から種別固有カラムを削除
ALTER TABLE ctb_lease_integrated
    DROP COLUMN IF EXISTS re_structure,
    DROP COLUMN IF EXISTS re_area,
    DROP COLUMN IF EXISTS re_layout,
    DROP COLUMN IF EXISTS re_completion_date,
    DROP COLUMN IF EXISTS re_landlord_name,
    DROP COLUMN IF EXISTS re_broker_company,
    DROP COLUMN IF EXISTS re_usage_restrictions,
    DROP COLUMN IF EXISTS vh_chassis_no,
    DROP COLUMN IF EXISTS vh_registration_no,
    DROP COLUMN IF EXISTS vh_vehicle_type,
    DROP COLUMN IF EXISTS vh_inspection_date,
    DROP COLUMN IF EXISTS vh_mileage_limit,
    DROP COLUMN IF EXISTS oa_model_no,
    DROP COLUMN IF EXISTS oa_serial_no,
    DROP COLUMN IF EXISTS oa_maintenance_date,
    DROP COLUMN IF EXISTS oa_maintenance_contract;
```

**変更するカラム**: `asset_category` → `asset_category_cd`（FK追加）

```sql
-- asset_category を asset_category_cd に改名し、FKを付与
ALTER TABLE ctb_lease_integrated
    RENAME COLUMN asset_category TO asset_category_cd;

ALTER TABLE ctb_lease_integrated
    ALTER COLUMN asset_category_cd TYPE VARCHAR(10);

ALTER TABLE ctb_lease_integrated
    ADD CONSTRAINT fk_ctb_asset_category
    FOREIGN KEY (asset_category_cd) REFERENCES m_asset_category (asset_category_cd);
```

**残すカラム（変更後のctb_lease_integrated全体）**:

| カラム | 型 | 備考 |
|--------|-----|------|
| ctb_id | SERIAL PK | |
| contract_no | VARCHAR(15) | |
| property_no | INTEGER | ※将来的にctb_propertyのproperty_noと統合検討 |
| contract_name | VARCHAR(200) | 画面表示用に残す |
| contract_type_cd | VARCHAR(10) | FK → m_contract_type |
| supplier_cd | VARCHAR(10) | FK → m_supplier |
| mgmt_dept_cd | VARCHAR(10) | FK → m_department |
| lease_start_date | DATE | |
| lease_end_date | DATE | |
| free_rent_months | INTEGER | |
| lease_term_months | INTEGER | |
| asset_no | VARCHAR(20) | 画面一覧表示用に残す |
| asset_category_cd | VARCHAR(10) | FK → m_asset_category（改名） |
| asset_name | VARCHAR(200) | 画面一覧表示用に残す |
| company_name | VARCHAR(100) | |
| install_location | VARCHAR(200) | |
| remarks | VARCHAR(500) | |
| monthly_payment | NUMERIC(15,2) | |
| lease_depreciation | NUMERIC(15,2) | |
| total_payment | NUMERIC(15,2) | |
| split_status | VARCHAR(10) | |
| create_dt | TIMESTAMP | |
| update_dt | TIMESTAMP | |

### 3.2 m_asset_category のシードデータ変更

既存のシードデータはそのまま利用可能。変更なし。

```
AC01=建物, AC02=構築物, AC03=機械装置, AC04=車両運搬具, AC05=その他
```

画面側では `asset_category_cd` + `asset_category_nm` をマスタからロードして表示する（ハードコード廃止）。

---

## 4. 初期属性定義データ（シード）

```sql
-- ==============================================
-- m_property_attribute_def: 初期属性定義
-- 旧 re_*7列、vh_*5列、oa_*4列 をEAV定義に移行
-- ==============================================

-- ---- 不動産系（AC01=建物, AC02=構築物 共通） ----
-- AC01: 建物
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC01', 're_structure',          '構造',         'TEXT',    'TEXTBOX',        100, FALSE, 10),
    ('AC01', 're_area',               '面積',         'TEXT',    'TEXTBOX',         50, FALSE, 20),
    ('AC01', 're_layout',             '間取り',       'TEXT',    'TEXTBOX',         50, FALSE, 30),
    ('AC01', 're_completion_date',    '竣工日',       'DATE',    'DATEPICKER',    NULL, FALSE, 40),
    ('AC01', 're_landlord_name',      '貸主名',       'TEXT',    'TEXTBOX',        100, FALSE, 50),
    ('AC01', 're_broker_company',     '仲介会社',     'TEXT',    'TEXTBOX',        100, FALSE, 60),
    ('AC01', 're_usage_restrictions', '用途制限',     'TEXT',    'TEXTAREA',       200, FALSE, 70);

-- AC02: 構築物（不動産系なので同じ属性を持つ）
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC02', 're_structure',          '構造',         'TEXT',    'TEXTBOX',        100, FALSE, 10),
    ('AC02', 're_area',               '面積',         'TEXT',    'TEXTBOX',         50, FALSE, 20),
    ('AC02', 're_layout',             '間取り',       'TEXT',    'TEXTBOX',         50, FALSE, 30),
    ('AC02', 're_completion_date',    '竣工日',       'DATE',    'DATEPICKER',    NULL, FALSE, 40),
    ('AC02', 're_landlord_name',      '貸主名',       'TEXT',    'TEXTBOX',        100, FALSE, 50),
    ('AC02', 're_broker_company',     '仲介会社',     'TEXT',    'TEXTBOX',        100, FALSE, 60),
    ('AC02', 're_usage_restrictions', '用途制限',     'TEXT',    'TEXTAREA',       200, FALSE, 70);

-- ---- 機械装置系（AC03） ----
-- AC03: 機械装置（OA機器系と同じ属性を初期設定）
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC03', 'oa_model_no',           '型番',           'TEXT',    'TEXTBOX',       50, FALSE, 10),
    ('AC03', 'oa_serial_no',          'シリアル番号',   'TEXT',    'TEXTBOX',       50, FALSE, 20),
    ('AC03', 'oa_maintenance_date',   '保守期限',       'DATE',    'DATEPICKER',  NULL, FALSE, 30),
    ('AC03', 'oa_maintenance_contract','保守契約',       'TEXT',    'TEXTBOX',      200, FALSE, 40);

-- ---- 車両系（AC04=車両運搬具） ----
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC04', 'vh_chassis_no',         '車台番号',       'TEXT',    'TEXTBOX',       50, FALSE, 10),
    ('AC04', 'vh_registration_no',    '登録番号',       'TEXT',    'TEXTBOX',       50, FALSE, 20),
    ('AC04', 'vh_vehicle_type',       '車種',           'TEXT',    'TEXTBOX',      100, FALSE, 30),
    ('AC04', 'vh_inspection_date',    '車検期限',       'DATE',    'DATEPICKER',  NULL, FALSE, 40),
    ('AC04', 'vh_mileage_limit',      '走行距離制限',   'TEXT',    'TEXTBOX',       50, FALSE, 50);

-- ---- その他（AC05） ----
-- AC05: その他（初期は属性なし。ユーザーが自由に追加可能）
-- 必要に応じて管理画面から追加する想定
```

**属性定義サマリ**:

| asset_category_cd | カテゴリ名 | 属性数 | 属性一覧 |
|-------------------|-----------|--------|---------|
| AC01 | 建物 | 7 | 構造, 面積, 間取り, 竣工日, 貸主名, 仲介会社, 用途制限 |
| AC02 | 構築物 | 7 | (建物と同一) |
| AC03 | 機械装置 | 4 | 型番, シリアル番号, 保守期限, 保守契約 |
| AC04 | 車両運搬具 | 5 | 車台番号, 登録番号, 車種, 車検期限, 走行距離制限 |
| AC05 | その他 | 0 | (ユーザーが随時追加) |

---

## 5. 統合VIEW定義

### 5.1 v_ctb_property_full（画面用統合ビュー）

EAVデータを行→列変換（ピボット）して画面での取り扱いを容易にするVIEW。

```sql
-- ==============================================
-- v_ctb_property_full: 画面用統合ビュー
-- ctb_property + EAV属性を動的取得するベースビュー
-- ==============================================
CREATE OR REPLACE VIEW v_ctb_property_full AS
SELECT
    p.property_id,
    p.ctb_id,
    p.property_no,
    p.asset_category_cd,
    ac.asset_category_nm,
    p.asset_no,
    p.asset_name,
    p.company_name,
    p.install_location,
    p.remarks,
    c.contract_no,
    c.contract_name,
    c.lease_start_date,
    c.lease_end_date,
    c.monthly_payment
FROM ctb_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_asset_category ac ON p.asset_category_cd = ac.asset_category_cd;

COMMENT ON VIEW v_ctb_property_full IS '物件統合ビュー: 一覧画面用。EAV属性はアプリ側でproperty_idを使って別途取得する。';
```

**EAV属性の取得SQL（アプリ側で使用）**:

```sql
-- 特定物件の全属性値を属性定義と結合して取得
SELECT
    d.attr_def_id,
    d.attr_cd,
    d.attr_nm,
    d.data_type,
    d.display_type,
    d.max_length,
    d.is_required,
    d.sort_order,
    d.default_value,
    a.attribute_value
FROM m_property_attribute_def d
LEFT JOIN ctb_property_attribute a
    ON d.attr_def_id = a.attr_def_id
   AND a.property_id = @property_id
WHERE d.asset_category_cd = @asset_category_cd
ORDER BY d.sort_order;
```

> **設計判断**: ピボットVIEW（crosstab）は属性が動的に増減するため不適切。
> 代わりに、一覧画面ではv_ctb_property_fullで基本情報を表示し、
> 詳細画面では上記SQLで属性定義+値をセットで取得し、動的にコントロールを生成する。

---

## 6. データ移行SQL

### 6.1 既存ctb_lease_integrated → ctb_property + ctb_property_attribute

```sql
-- ==============================================
-- データ移行: 既存種別固有カラム → EAVテーブル
-- 前提: m_property_attribute_def のシードデータ投入済み
-- ==============================================

BEGIN;

-- ---- Step 1: ctb_property レコード作成 ----
-- 既存ctb_lease_integratedの各行をctb_propertyに転写
INSERT INTO ctb_property (
    ctb_id,
    property_no,
    asset_category_cd,
    asset_no,
    asset_name,
    company_name,
    install_location,
    remarks
)
SELECT
    c.ctb_id,
    c.property_no,
    -- 旧 asset_category(日本語名) → asset_category_cd へのマッピング
    CASE c.asset_category
        WHEN '不動産' THEN 'AC01'
        WHEN '構築物' THEN 'AC02'
        WHEN '機械装置' THEN 'AC03'
        WHEN '車両'   THEN 'AC04'
        WHEN 'OA機器' THEN 'AC05'
        ELSE 'AC05'  -- デフォルト: その他
    END,
    c.asset_no,
    c.asset_name,
    c.company_name,
    c.install_location,
    c.remarks
FROM ctb_lease_integrated c
ON CONFLICT (ctb_id, property_no) DO NOTHING;

-- ---- Step 2: 不動産属性の移行（asset_category = '不動産' → AC01） ----
-- re_structure
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_structure
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_structure'
WHERE c.re_structure IS NOT NULL AND c.re_structure <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_area
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_area
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_area'
WHERE c.re_area IS NOT NULL AND c.re_area <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_layout
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_layout
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_layout'
WHERE c.re_layout IS NOT NULL AND c.re_layout <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_completion_date (DATE → TEXT変換)
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, TO_CHAR(c.re_completion_date, 'YYYY-MM-DD')
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_completion_date'
WHERE c.re_completion_date IS NOT NULL
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_landlord_name
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_landlord_name
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_landlord_name'
WHERE c.re_landlord_name IS NOT NULL AND c.re_landlord_name <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_broker_company
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_broker_company
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_broker_company'
WHERE c.re_broker_company IS NOT NULL AND c.re_broker_company <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- re_usage_restrictions
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.re_usage_restrictions
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_usage_restrictions'
WHERE c.re_usage_restrictions IS NOT NULL AND c.re_usage_restrictions <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- ---- Step 3: 車両属性の移行（asset_category = '車両' → AC04） ----
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.vh_chassis_no
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_chassis_no'
WHERE c.vh_chassis_no IS NOT NULL AND c.vh_chassis_no <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.vh_registration_no
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_registration_no'
WHERE c.vh_registration_no IS NOT NULL AND c.vh_registration_no <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.vh_vehicle_type
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_vehicle_type'
WHERE c.vh_vehicle_type IS NOT NULL AND c.vh_vehicle_type <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, TO_CHAR(c.vh_inspection_date, 'YYYY-MM-DD')
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_inspection_date'
WHERE c.vh_inspection_date IS NOT NULL
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.vh_mileage_limit
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_mileage_limit'
WHERE c.vh_mileage_limit IS NOT NULL AND c.vh_mileage_limit <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- ---- Step 4: OA機器属性の移行（asset_category = 'OA機器' → AC05相当 → AC03で管理） ----
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.oa_model_no
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_model_no'
WHERE c.oa_model_no IS NOT NULL AND c.oa_model_no <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.oa_serial_no
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_serial_no'
WHERE c.oa_serial_no IS NOT NULL AND c.oa_serial_no <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, TO_CHAR(c.oa_maintenance_date, 'YYYY-MM-DD')
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_maintenance_date'
WHERE c.oa_maintenance_date IS NOT NULL
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, c.oa_maintenance_contract
FROM ctb_lease_integrated c
INNER JOIN ctb_property p ON c.ctb_id = p.ctb_id AND c.property_no = p.property_no
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_maintenance_contract'
WHERE c.oa_maintenance_contract IS NOT NULL AND c.oa_maintenance_contract <> ''
ON CONFLICT (property_id, attr_def_id) DO NOTHING;

-- ---- Step 5: ctb_lease_integrated の asset_category を asset_category_cd に変換 ----
UPDATE ctb_lease_integrated
SET asset_category = CASE asset_category
    WHEN '不動産' THEN 'AC01'
    WHEN '構築物' THEN 'AC02'
    WHEN '機械装置' THEN 'AC03'
    WHEN '車両'   THEN 'AC04'
    WHEN 'OA機器' THEN 'AC05'
    ELSE asset_category  -- 既にコード値の場合はそのまま
END
WHERE asset_category IN ('不動産', '構築物', '機械装置', '車両', 'OA機器');

COMMIT;

-- ---- 検証クエリ ----
-- SELECT p.property_id, p.ctb_id, p.property_no, p.asset_category_cd,
--        ac.asset_category_nm, p.asset_name,
--        d.attr_nm, a.attribute_value
-- FROM ctb_property p
-- INNER JOIN m_asset_category ac ON p.asset_category_cd = ac.asset_category_cd
-- LEFT JOIN ctb_property_attribute a ON p.property_id = a.property_id
-- LEFT JOIN m_property_attribute_def d ON a.attr_def_id = d.attr_def_id
-- ORDER BY p.ctb_id, p.property_no, d.sort_order;
```

---

## 7. 作業手順

### Step 1: DDLスクリプト作成・実行

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `sql/010_ctb_property_eav.sql`（新規作成） |
| **作業内容** | セクション2のDDL3テーブル + セクション3のALTER TABLE + セクション5のVIEWを1ファイルにまとめ、psqlで実行 |
| **完了条件** | `\dt ctb_property*` で2テーブル、`\dt m_property*` で1テーブルが表示される。`\dv v_ctb_property_full` でVIEWが存在する |

### Step 2: 属性定義シードデータ投入

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `sql/011_seed_property_attribute_def.sql`（新規作成） |
| **作業内容** | セクション4のINSERT文を実行 |
| **完了条件** | `SELECT count(*) FROM m_property_attribute_def` が 23件（AC01:7 + AC02:7 + AC03:4 + AC04:5 + AC05:0） |

### Step 3: データ移行実行

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `sql/012_migrate_eav.sql`（新規作成） |
| **作業内容** | セクション6の移行SQLを実行。既存10件のd_kykhデータをctb_property + ctb_property_attributeに移行 |
| **完了条件** | `SELECT count(*) FROM ctb_property` が既存ctb_lease_integratedの行数と一致。検証クエリで属性値が正しく表示される |

### Step 4: ctb_lease_integrated カラム削除

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `sql/013_alter_ctb_drop_type_columns.sql`（新規作成） |
| **作業内容** | セクション3.1のALTER TABLE DROP COLUMN + RENAME COLUMNを実行 |
| **完了条件** | `\d ctb_lease_integrated` で re_*, vh_*, oa_* カラムが存在しない。asset_category_cd カラムにFKが付与されている |

### Step 5: PropertyAttributeDefRepository 新規作成

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/PropertyAttributeDefRepository.vb`（新規作成） |
| **作業内容** | m_property_attribute_defからasset_category_cd指定で属性定義リストを取得するリポジトリ。セクション8.1参照 |
| **完了条件** | `LoadByAssetCategory("AC01")` で7件の属性定義が取得できる |

### Step 6: PropertyRepository 新規作成

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/PropertyRepository.vb`（新規作成） |
| **作業内容** | ctb_property + ctb_property_attribute のCRUD。セクション8.2参照 |
| **完了条件** | INSERT/SELECT/UPDATEの各メソッドが動作する |

### Step 7: CtbDataStore.vb 改修

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbDataStore.vb` |
| **作業内容** | CtbRecordからre_*/vh_*/oa_*プロパティを削除。代わりにPropertyAttributesディクショナリを追加。セクション8.1参照 |
| **完了条件** | CtbRecordがコンパイルエラーなく、EAV属性をDictionary(Of String, String)で保持できる |

### Step 8: CtbRepository.vb 改修

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbRepository.vb` |
| **作業内容** | INSERT/SELECT SQLからre_*/vh_*/oa_*カラムを削除。ctb_property + ctb_property_attributeへのINSERT/SELECTを追加。セクション8.2参照 |
| **完了条件** | InsertAll/SelectAll/SelectByContractNoが新スキーマで動作する |

### Step 9: FrmAssetDetailEntry.vb 改修（動的パネル生成）

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmAssetDetailEntry.vb` + `FrmAssetDetailEntry.Designer.vb` |
| **作業内容** | 固定パネル(pnlRealEstate/pnlVehicle/pnlOfficeEquip)を廃止し、m_property_attribute_defから動的にコントロールを生成。セクション8.3参照 |
| **完了条件** | 資産カテゴリ変更時に種別固有ブロックの入力項目が動的に切り替わる |

### Step 10: FrmLeaseContractMain.vb 改修

| 項目 | 内容 |
|------|------|
| **対象ファイル** | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmLeaseContractMain.vb` |
| **作業内容** | cmbAssetCategoryのハードコード廃止 → m_asset_categoryマスタからロード。CtbRecordの種別プロパティ参照をEAV形式に変更。セクション8.4参照 |
| **完了条件** | コンボボックスがマスタデータ駆動になり、5種別が表示される |

### Step 11: 結合テスト

| 項目 | 内容 |
|------|------|
| **対象ファイル** | 全改修ファイル |
| **作業内容** | (1) 新規契約登録 → 資産入力 → 種別固有情報入力 → DB保存 → 再読込で値が保持されていることを確認。(2) 各資産カテゴリ（5種）で動的パネルが正しく生成されることを確認 |
| **完了条件** | 全5カテゴリで登録・照会が正常動作。既存移行データも正しく表示される |

---

## 8. アプリケーション層の変更

### 8.1 CtbDataStore.vb の変更

**CtbRecord の変更概要**:

```vb
' ★削除: 種別固有プロパティ（31行目〜52行目すべて）
' ReStructure, ReArea, ReLayout, ReCompletionDate, ReLandlordName, ReBrokerCompany, ReUsageRestrictions
' VhChassisNo, VhRegistrationNo, VhVehicleType, VhInspectionDate, VhMileageLimit
' OaModelNo, OaSerialNo, OaMaintenanceDate, OaMaintenanceContract

' ★追加: EAV属性の汎用保持
''' <summary>
''' 種別固有属性 (attr_cd → attribute_value)
''' m_property_attribute_defのattr_cdをキーに、値をテキストで保持
''' </summary>
Public Property PropertyAttributes As New Dictionary(Of String, String)

''' <summary>
''' 物件テーブルのproperty_id（DB保存後に設定される）
''' </summary>
Public Property PropertyId As Integer = 0

''' <summary>
''' 資産カテゴリコード（AC01〜AC05）。旧AssetCategoryは日本語名だったがコード値に変更。
''' </summary>
Public Property AssetCategoryCd As String = ""

' ★残す: AssetCategory プロパティは表示用に ReadOnly で維持（マスタから名称解決）
```

**属性アクセスのヘルパー**:

```vb
''' <summary>
''' EAV属性値を取得する（キー不在時は空文字列）
''' </summary>
Public Function GetAttribute(attrCd As String) As String
    If PropertyAttributes.ContainsKey(attrCd) Then
        Return PropertyAttributes(attrCd)
    End If
    Return ""
End Function

''' <summary>
''' EAV属性値を設定する
''' </summary>
Public Sub SetAttribute(attrCd As String, value As String)
    PropertyAttributes(attrCd) = If(value, "")
End Sub
```

### 8.2 CtbRepository.vb の変更

**INSERT処理の変更**:

```vb
''' <summary>
''' CTBレコード群をDBに一括INSERT（1トランザクション）
''' ctb_lease_integrated → ctb_property → ctb_property_attribute の順にINSERT
''' </summary>
Public Sub InsertAll(records As List(Of CtbRecord))
    If records Is Nothing OrElse records.Count = 0 Then Return

    Using conn As NpgsqlConnection = _connMgr.GetConnection()
        Using txn As NpgsqlTransaction = conn.BeginTransaction()
            Try
                For Each rec In records
                    ' 1. ctb_lease_integrated INSERT（re_*/vh_*/oa_* なし）
                    Dim ctbId As Integer = InsertLeaseIntegrated(conn, txn, rec)

                    ' 2. ctb_property INSERT
                    Dim propertyId As Integer = InsertProperty(conn, txn, ctbId, rec)

                    ' 3. ctb_property_attribute INSERT（EAV）
                    InsertPropertyAttributes(conn, txn, propertyId, rec.AssetCategoryCd, rec.PropertyAttributes)

                    ' 4. ctb_dept_allocation INSERT
                    InsertDeptAllocation(conn, txn, ctbId, rec.DeptCd, rec.AllocationRatio, rec.MonthlyPayment)
                Next
                txn.Commit()
            Catch
                txn.Rollback()
                Throw
            End Try
        End Using
    End Using
End Sub
```

**新メソッド InsertProperty**:

```vb
Private Function InsertProperty(conn As NpgsqlConnection, txn As NpgsqlTransaction,
                                 ctbId As Integer, rec As CtbRecord) As Integer
    Const sql As String =
        "INSERT INTO ctb_property " &
        "(ctb_id, property_no, asset_category_cd, asset_no, asset_name, company_name, install_location, remarks) " &
        "VALUES (@ctb_id, @property_no, @asset_category_cd, @asset_no, @asset_name, @company_name, @install_location, @remarks) " &
        "RETURNING property_id"

    Using cmd As New NpgsqlCommand(sql, conn, txn)
        cmd.Parameters.AddWithValue("@ctb_id", ctbId)
        cmd.Parameters.AddWithValue("@property_no", rec.PropertyNo)
        cmd.Parameters.AddWithValue("@asset_category_cd", rec.AssetCategoryCd)
        cmd.Parameters.AddWithValue("@asset_no", NullIfEmpty(rec.AssetNo))
        cmd.Parameters.AddWithValue("@asset_name", NullIfEmpty(rec.AssetName))
        cmd.Parameters.AddWithValue("@company_name", NullIfEmpty(rec.CompanyName))
        cmd.Parameters.AddWithValue("@install_location", NullIfEmpty(rec.InstallLocation))
        cmd.Parameters.AddWithValue("@remarks", NullIfEmpty(rec.Remarks))
        Return CInt(cmd.ExecuteScalar())
    End Using
End Function
```

**新メソッド InsertPropertyAttributes**:

```vb
Private Sub InsertPropertyAttributes(conn As NpgsqlConnection, txn As NpgsqlTransaction,
                                      propertyId As Integer, assetCategoryCd As String,
                                      attributes As Dictionary(Of String, String))
    If attributes Is Nothing OrElse attributes.Count = 0 Then Return

    ' attr_cd → attr_def_id のマッピングを取得
    Const lookupSql As String =
        "SELECT attr_def_id, attr_cd FROM m_property_attribute_def WHERE asset_category_cd = @cat"

    Dim attrMap As New Dictionary(Of String, Integer)
    Using cmd As New NpgsqlCommand(lookupSql, conn, txn)
        cmd.Parameters.AddWithValue("@cat", assetCategoryCd)
        Using reader = cmd.ExecuteReader()
            While reader.Read()
                attrMap(reader.GetString(1)) = reader.GetInt32(0)
            End While
        End Using
    End Using

    ' 属性値を INSERT
    Const insertSql As String =
        "INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value) " &
        "VALUES (@pid, @did, @val) " &
        "ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value, update_dt = CURRENT_TIMESTAMP"

    For Each kvp In attributes
        Dim defId As Integer
        If attrMap.TryGetValue(kvp.Key, defId) AndAlso Not String.IsNullOrEmpty(kvp.Value) Then
            Using cmd As New NpgsqlCommand(insertSql, conn, txn)
                cmd.Parameters.AddWithValue("@pid", propertyId)
                cmd.Parameters.AddWithValue("@did", defId)
                cmd.Parameters.AddWithValue("@val", kvp.Value)
                cmd.ExecuteNonQuery()
            End Using
        End If
    Next
End Sub
```

**SELECT処理の変更**:

```vb
''' <summary>
''' DBからCTBレコードを全件取得
''' ctb_lease_integrated + ctb_property + ctb_property_attribute を結合
''' </summary>
Public Function SelectAll() As List(Of CtbRecord)
    ' Step 1: ctb_lease_integrated + ctb_property + dept を取得
    Const sql As String =
        "SELECT c.ctb_id, c.contract_no, c.property_no, " &
        "c.contract_name, c.contract_type_cd, c.supplier_cd, c.mgmt_dept_cd, " &
        "c.lease_start_date, c.lease_end_date, c.free_rent_months, c.lease_term_months, " &
        "c.asset_category_cd, " &
        "p.property_id, p.asset_no, p.asset_name, p.company_name, p.install_location, p.remarks AS property_remarks, " &
        "c.monthly_payment, c.lease_depreciation, c.total_payment, c.split_status, " &
        "d.dept_cd, md.dept_nm, d.allocation_ratio, " &
        "ac.asset_category_nm " &
        "FROM ctb_lease_integrated c " &
        "LEFT JOIN ctb_property p ON c.ctb_id = p.ctb_id " &
        "LEFT JOIN m_asset_category ac ON c.asset_category_cd = ac.asset_category_cd " &
        "LEFT JOIN ctb_dept_allocation d ON c.ctb_id = d.ctb_id " &
        "LEFT JOIN m_department md ON d.dept_cd = md.dept_cd " &
        "ORDER BY c.contract_no, c.property_no"

    Dim results As New List(Of CtbRecord)

    Using conn As NpgsqlConnection = _connMgr.GetConnection()
        Using cmd As New NpgsqlCommand(sql, conn)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    Dim rec As New CtbRecord()
                    ' (基本カラムのマッピング - 種別固有は含まない)
                    rec.PropertyId = SafeGetInt(reader, "property_id").GetValueOrDefault(0)
                    rec.AssetCategoryCd = SafeGetString(reader, "asset_category_cd")
                    rec.AssetCategory = SafeGetString(reader, "asset_category_nm")
                    ' ... (他の基本カラム)
                    results.Add(rec)
                End While
            End Using
        End Using

        ' Step 2: 各レコードのEAV属性を取得
        Const attrSql As String =
            "SELECT d.attr_cd, a.attribute_value " &
            "FROM ctb_property_attribute a " &
            "INNER JOIN m_property_attribute_def d ON a.attr_def_id = d.attr_def_id " &
            "WHERE a.property_id = @pid"

        For Each rec In results
            If rec.PropertyId > 0 Then
                Using cmd As New NpgsqlCommand(attrSql, conn)
                    cmd.Parameters.AddWithValue("@pid", rec.PropertyId)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            rec.PropertyAttributes(reader.GetString(0)) = If(reader.IsDBNull(1), "", reader.GetString(1))
                        End While
                    End Using
                End Using
            End If
        Next
    End Using

    Return results
End Function
```

### 8.3 FrmAssetDetailEntry.vb の変更（動的パネル生成）

**変更の核心**: 固定3パネル（pnlRealEstate/pnlVehicle/pnlOfficeEquip）を廃止し、`m_property_attribute_def`の定義に基づいて動的にコントロールを生成する。

**Designer.vb から削除するコントロール**:
- `pnlRealEstate` および配下の全コントロール（txtStructure, txtArea, txtLayout, dtpCompletion, txtLandlordName, txtBrokerCompany, txtUsageRestrictions）
- `pnlVehicle` および配下の全コントロール（txtChassisNo, txtRegistrationNo, txtVehicleType, dtpInspectionDate, txtMileageLimit）
- `pnlOfficeEquip` および配下の全コントロール（txtModelNo, txtSerialNo, dtpMaintenanceDate, txtMaintenanceContract）

**Designer.vb に追加するコントロール**:
- `pnlDynamicAttributes` (FlowLayoutPanel) - 動的属性コントロールのコンテナ

**コードビハインドの主要変更**:

```vb
' ★新規: 属性定義リスト（DBから取得）
Private _attrDefs As List(Of PropertyAttributeDef)

' ★新規: 動的生成されたコントロールへの参照（attr_cd → Control）
Private _dynamicControls As New Dictionary(Of String, Control)

' ★変更: AssetCategory → AssetCategoryCd（コード値で受け取る）
Public Property AssetCategoryCd As String = ""

''' <summary>
''' m_property_attribute_def を読み込み、動的にコントロールを生成する
''' </summary>
Private Sub BuildDynamicAttributePanel()
    pnlDynamicAttributes.Controls.Clear()
    _dynamicControls.Clear()

    If String.IsNullOrEmpty(AssetCategoryCd) Then Return

    ' DBから属性定義を取得
    Dim repo As New PropertyAttributeDefRepository()
    _attrDefs = repo.LoadByAssetCategory(AssetCategoryCd)

    For Each attrDef In _attrDefs
        ' ラベル + 入力コントロール を1行として追加
        Dim rowPanel As New Panel() With {
            .Width = pnlDynamicAttributes.Width - 30,
            .Height = 35,
            .Margin = New Padding(0, 2, 0, 2)
        }

        Dim lbl As New Label() With {
            .Text = attrDef.AttrNm,
            .Location = New Point(0, 8),
            .Width = 120,
            .Font = FNT_LABEL,
            .ForeColor = CLR_LABEL
        }
        rowPanel.Controls.Add(lbl)

        Dim ctl As Control = CreateControlForAttribute(attrDef)
        ctl.Location = New Point(130, 3)
        ctl.Width = rowPanel.Width - 140
        rowPanel.Controls.Add(ctl)

        _dynamicControls(attrDef.AttrCd) = ctl
        pnlDynamicAttributes.Controls.Add(rowPanel)
    Next
End Sub

''' <summary>
''' 属性定義の display_type に応じてコントロールを生成する
''' </summary>
Private Function CreateControlForAttribute(attrDef As PropertyAttributeDef) As Control
    Select Case attrDef.DisplayType
        Case "DATEPICKER"
            Dim dtp As New DateTimePicker() With {
                .Format = DateTimePickerFormat.Short,
                .Tag = attrDef.AttrCd
            }
            Return dtp

        Case "NUMERICUPDOWN"
            Dim nud As New NumericUpDown() With {
                .DecimalPlaces = 2,
                .Maximum = Decimal.MaxValue,
                .Tag = attrDef.AttrCd
            }
            Return nud

        Case "CHECKBOX"
            Dim chk As New CheckBox() With {
                .Tag = attrDef.AttrCd
            }
            Return chk

        Case "TEXTAREA"
            Dim txt As New TextBox() With {
                .Multiline = True,
                .Height = 60,
                .ScrollBars = ScrollBars.Vertical,
                .Tag = attrDef.AttrCd
            }
            If attrDef.MaxLength.HasValue Then txt.MaxLength = attrDef.MaxLength.Value
            Return txt

        Case Else ' "TEXTBOX" (default)
            Dim txt As New TextBox() With {
                .Tag = attrDef.AttrCd
            }
            If attrDef.MaxLength.HasValue Then txt.MaxLength = attrDef.MaxLength.Value
            Return txt
    End Select
End Function
```

**属性値の読み書き**:

```vb
''' <summary>
''' 動的コントロールから属性値を Dictionary に収集する
''' </summary>
Public Function CollectAttributeValues() As Dictionary(Of String, String)
    Dim result As New Dictionary(Of String, String)
    For Each kvp In _dynamicControls
        Dim attrCd As String = kvp.Key
        Dim ctl As Control = kvp.Value

        If TypeOf ctl Is DateTimePicker Then
            result(attrCd) = DirectCast(ctl, DateTimePicker).Value.ToString("yyyy-MM-dd")
        ElseIf TypeOf ctl Is NumericUpDown Then
            result(attrCd) = DirectCast(ctl, NumericUpDown).Value.ToString()
        ElseIf TypeOf ctl Is CheckBox Then
            result(attrCd) = If(DirectCast(ctl, CheckBox).Checked, "true", "false")
        Else
            result(attrCd) = ctl.Text
        End If
    Next
    Return result
End Function

''' <summary>
''' Dictionary から動的コントロールに値を設定する（照会モード用）
''' </summary>
Public Sub LoadAttributeValues(attributes As Dictionary(Of String, String))
    If attributes Is Nothing Then Return
    For Each kvp In attributes
        Dim ctl As Control = Nothing
        If _dynamicControls.TryGetValue(kvp.Key, ctl) Then
            If TypeOf ctl Is DateTimePicker Then
                Dim dt As Date
                If Date.TryParse(kvp.Value, dt) Then
                    DirectCast(ctl, DateTimePicker).Value = dt
                End If
            ElseIf TypeOf ctl Is NumericUpDown Then
                Dim d As Decimal
                If Decimal.TryParse(kvp.Value, d) Then
                    DirectCast(ctl, NumericUpDown).Value = d
                End If
            ElseIf TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Checked = (kvp.Value = "true")
            Else
                ctl.Text = If(kvp.Value, "")
            End If
        End If
    Next
End Sub
```

**削除するプロパティ（戻り値用の種別固有ReadOnlyプロパティ）**:
- `ReStructure`, `ReArea`, `ReLayout`, `ReCompletionDate`, `ReLandlordName`, `ReBrokerCompany`, `ReUsageRestrictions`
- `VhChassisNo`, `VhRegistrationNo`, `VhVehicleType`, `VhInspectionDate`, `VhMileageLimit`
- `OaModelNo`, `OaSerialNo`, `OaMaintenanceDate`, `OaMaintenanceContract`

**削除するメソッド**:
- `SwitchCategoryPanel()` - 動的パネルに置き換わるため不要

### 8.4 FrmLeaseContractMain.vb の変更

**変更点1: cmbAssetCategory をマスタ駆動に**

```vb
' ★変更前（848-854行目）:
' cmbAssetCategory.Items.AddRange(New String() {"不動産", "車両", "OA機器"})
' cmbAssetCategory.SelectedIndex = 0

' ★変更後:
Private _assetCategoryTable As Data.DataTable

Private Sub InitAssetCategoryCombo()
    _assetCategoryTable = _masterData.LoadAssetCategories()
    cmbAssetCategory.Items.Clear()
    cmbAssetCategory.DisplayMember = "asset_category_nm"
    cmbAssetCategory.ValueMember = "asset_category_cd"
    For Each row As Data.DataRow In _assetCategoryTable.Rows
        cmbAssetCategory.Items.Add(row("asset_category_nm").ToString())
    Next
    If cmbAssetCategory.Items.Count > 0 Then cmbAssetCategory.SelectedIndex = 0
End Sub

''' <summary>
''' 選択中の資産カテゴリコードを取得
''' </summary>
Private Function GetSelectedAssetCategoryCd() As String
    If cmbAssetCategory.SelectedIndex < 0 Then Return ""
    Return _assetCategoryTable.Rows(cmbAssetCategory.SelectedIndex)("asset_category_cd").ToString()
End Function
```

**変更点2: 資産入力画面の呼び出し**

```vb
' ★変更前:
' frm.AssetCategory = cmbAssetCategory.SelectedItem.ToString()

' ★変更後:
frm.AssetCategoryCd = GetSelectedAssetCategoryCd()
```

**変更点3: CtbRecordへの種別固有値の設定**

```vb
' ★変更前（2506行目〜のSelect Case）:
' Select Case frm.AssetCategory
'     Case "不動産"
'         ctb.ReStructure = frm.ReStructure
'         ctb.ReArea = frm.ReArea
'         ...
'     Case "車両"
'         ctb.VhChassisNo = frm.VhChassisNo
'         ...
' End Select

' ★変更後:
ctb.AssetCategoryCd = frm.AssetCategoryCd
ctb.PropertyAttributes = frm.CollectAttributeValues()
```

**変更点4: MasterDataLoader への LoadAssetCategories メソッド追加**

MasterDataLoaderに以下を追加する必要がある:

```vb
''' <summary>
''' m_asset_category テーブルを読み込む
''' </summary>
Public Function LoadAssetCategories() As DataTable
    Const sql As String = "SELECT asset_category_cd, asset_category_nm FROM m_asset_category ORDER BY asset_category_cd"
    Dim dt As New DataTable()
    Using conn = GetConnection()
        Using cmd As New NpgsqlCommand(sql, conn)
            Using adapter As New NpgsqlDataAdapter(cmd)
                adapter.Fill(dt)
            End Using
        End Using
    End Using
    Return dt
End Function
```

---

## 9. 画面イメージ（テキスト）

### 変更前（固定3パネル切替）

```
┌─ FrmAssetDetailEntry ──────────────────────────────────┐
│ 資産番号: [A00001]   資産種類: 不動産                   │
│                                                         │
│ ┌─ 基本情報 ─────────────────────────────────────────┐ │
│ │ 資産名:    [                    ]                    │ │
│ │ 会社:      [▼ 本社         ]                        │ │
│ │ 設置場所:  [                    ]                    │ │
│ │ 備考:      [                    ]                    │ │
│ └─────────────────────────────────────────────────────┘ │
│                                                         │
│ ┌─ 不動産固有情報 (pnlRealEstate) ★固定 ────────────┐ │
│ │ 構造:       [鉄筋コンクリート    ]                   │ │
│ │ 面積:       [120.5㎡             ]                   │ │
│ │ 間取り:     [3LDK                ]                   │ │
│ │ 竣工日:     [2020/04/01          ]                   │ │
│ │ 貸主名:     [○○不動産           ]                   │ │
│ │ 仲介会社:   [△△リアルティ       ]                   │ │
│ │ 用途制限:   [事務所のみ          ]                   │ │
│ └─────────────────────────────────────────────────────┘ │
│   ★ pnlVehicle, pnlOfficeEquip は hidden              │
│                                                         │
│ ┌─ 配賦部門 ─────────────────────────────────────────┐ │
│ │ [部門▼ ] [配賦率(%)] [+追加] [-削除]                │ │
│ │  本社     100.00                                     │ │
│ │ 配賦率合計: 100.00%                                  │ │
│ └─────────────────────────────────────────────────────┘ │
│              [追加] [キャンセル]                         │
└─────────────────────────────────────────────────────────┘
```

### 変更後（EAV動的パネル生成）

```
┌─ FrmAssetDetailEntry ──────────────────────────────────┐
│ 資産番号: [A00001]   資産種類: 建物 (AC01)              │
│                                                         │
│ ┌─ 基本情報 ─────────────────────────────────────────┐ │
│ │ 資産名:    [                    ]                    │ │
│ │ 会社:      [▼ 本社         ]                        │ │
│ │ 設置場所:  [                    ]                    │ │
│ │ 備考:      [                    ]                    │ │
│ └─────────────────────────────────────────────────────┘ │
│                                                         │
│ ┌─ 種別固有情報 (pnlDynamicAttributes) ★動的生成 ──┐ │
│ │  ┌────────────────────────────────────────────────┐ │ │
│ │  │ 構造        [鉄筋コンクリート    ] ← TEXTBOX   │ │ │
│ │  │ 面積        [120.5㎡             ] ← TEXTBOX   │ │ │
│ │  │ 間取り      [3LDK                ] ← TEXTBOX   │ │ │
│ │  │ 竣工日      [2020/04/01       ▼ ] ← DATEPICKER│ │ │
│ │  │ 貸主名      [○○不動産           ] ← TEXTBOX   │ │ │
│ │  │ 仲介会社    [△△リアルティ       ] ← TEXTBOX   │ │ │
│ │  │ 用途制限    [事務所のみ          ] ← TEXTAREA  │ │ │
│ │  │             [                    ]              │ │ │
│ │  └────────────────────────────────────────────────┘ │ │
│ │  ↑ m_property_attribute_def の定義に基づき自動生成  │ │
│ │    sort_order 順にラベル+コントロールを配置          │ │
│ └─────────────────────────────────────────────────────┘ │
│                                                         │
│ ┌─ 配賦部門 ─────────────────────────────────────────┐ │
│ │ [部門▼ ] [配賦率(%)] [+追加] [-削除]                │ │
│ │  本社     100.00                                     │ │
│ │ 配賦率合計: 100.00%                                  │ │
│ └─────────────────────────────────────────────────────┘ │
│              [追加] [キャンセル]                         │
└─────────────────────────────────────────────────────────┘

--- 資産種類を「車両運搬具(AC04)」に変更した場合 ---

│ ┌─ 種別固有情報 (pnlDynamicAttributes) ★動的生成 ──┐ │
│ │  ┌────────────────────────────────────────────────┐ │ │
│ │  │ 車台番号    [ABC-1234567         ] ← TEXTBOX   │ │ │
│ │  │ 登録番号    [品川300さ1234       ] ← TEXTBOX   │ │ │
│ │  │ 車種        [トヨタ プリウス     ] ← TEXTBOX   │ │ │
│ │  │ 車検期限    [2026/09/30       ▼ ] ← DATEPICKER│ │ │
│ │  │ 走行距離制限[30,000km            ] ← TEXTBOX   │ │ │
│ │  └────────────────────────────────────────────────┘ │ │
│ └─────────────────────────────────────────────────────┘ │

--- 資産種類を「その他(AC05)」に変更した場合 ---

│ ┌─ 種別固有情報 (pnlDynamicAttributes) ★動的生成 ──┐ │
│ │  （属性定義が0件のため、空表示）                     │ │
│ │  「この資産カテゴリには固有項目が未定義です」         │ │
│ └─────────────────────────────────────────────────────┘ │
```

### 動的生成の処理フロー

```
[画面ロード / 資産カテゴリ変更]
         │
         ▼
[m_property_attribute_def を asset_category_cd で SELECT]
         │
         ▼  結果: List(Of PropertyAttributeDef)
         │
         ▼
[pnlDynamicAttributes.Controls.Clear()]
         │
         ▼
[For Each attrDef In definitions]
    │
    ├─ display_type = "TEXTBOX"      → New TextBox()
    ├─ display_type = "DATEPICKER"   → New DateTimePicker()
    ├─ display_type = "NUMERICUPDOWN"→ New NumericUpDown()
    ├─ display_type = "CHECKBOX"     → New CheckBox()
    ├─ display_type = "TEXTAREA"     → New TextBox() {Multiline=True}
    └─ display_type = "COMBOBOX"     → New ComboBox() (将来拡張)
         │
         ▼
[ラベル(attr_nm) + コントロール を Panel に追加]
[_dynamicControls(attr_cd) = ctl でマップ保持]
         │
         ▼
[既存データがある場合: LoadAttributeValues(rec.PropertyAttributes)]
         │
         ▼
[追加ボタン押下時: CollectAttributeValues() → rec.PropertyAttributes]
```

### PropertyAttributeDef クラス定義

```vb
''' <summary>
''' 物件属性定義（m_property_attribute_def の1行に対応）
''' </summary>
Public Class PropertyAttributeDef
    Public Property AttrDefId As Integer
    Public Property AssetCategoryCd As String = ""
    Public Property AttrCd As String = ""
    Public Property AttrNm As String = ""
    Public Property DataType As String = "TEXT"
    Public Property DisplayType As String = "TEXTBOX"
    Public Property MaxLength As Integer? = Nothing
    Public Property IsRequired As Boolean = False
    Public Property SortOrder As Integer = 0
    Public Property DefaultValue As String = ""
End Class
```
