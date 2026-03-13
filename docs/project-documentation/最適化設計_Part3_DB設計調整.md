# 最適化設計 Part3: DB設計v5への調整

**ドキュメント番号**: OPT-2026-003
**バージョン**: 1.0
**作成日**: 2026年3月13日
**ステータス**: 確定
**入力ドキュメント**:
- `analysis_ui_db_compatibility.md` — 統合分析レポート
- `constraint_management.md` — 制約管理書
- `research_db_ctb_constraints.md` — DB+CTB調査レポート

---

## 1. 設計方針

### 1.1 基本方針

DB設計v5（37テーブル+7ビュー）をベースとし、以下の原則に従って**最小限の調整**を行う。

| # | 方針 | 詳細 |
|---|------|------|
| P1 | v5構造の維持 | 3NF正規化設計・テーブル層構成・命名規約を変更しない |
| P2 | CTBフラットテーブルとの共存 | CTBの既存22カラムは一切変更せず、NULLカラム追加のみ許容（制約C1） |
| P3 | 会計/税務レンジの二重保持 | 会計基準(ASBJ#34)と税法(法人税法)の計算値を別カラムで保持（制約C4） |
| P4 | 総務/経理分離のためのステータス管理 | lease_contractにステータス・承認情報を追加し、ワークフローをDB層で支援（制約C3） |
| P5 | 22カラム上限の維持 | v5の設計原則「各テーブル最大22カラム」を可能な限り遵守 |

### 1.2 変更スコープの定義

```
v5(37テーブル+7ビュー)
  ├── 既存テーブルへのカラム追加: 3テーブル（計13カラム追加）+ 1制約変更
  ├── 新規テーブル追加: 3テーブル
  ├── 既存ビュー変更: 1ビュー（v_ctb_export再定義）
  └── CTBテーブルへのカラム追加: 1テーブル（8カラム追加）
変更後: 40テーブル + 7ビュー
```

---

## 2. v5への変更サマリー

| # | 変更種別 | 対象 | 内容 | 関連制約 |
|---|----------|------|------|----------|
| 1 | カラム追加 | lease_asset | tax_useful_life_months, tax_depreciation_method, tax_residual_value, segment_code, remarks | C4, C1 |
| 2 | カラム追加+制約変更 | lease_contract | submitted_by, submitted_at, approved_by, approved_at, input_role（+既存statusのCHECK/DEFAULT変更） | C3 |
| 3 | カラム追加 | amortization_schedule | tax_depreciation_amount, tax_accumulated_depreciation, tax_carrying_amount | C4 |
| 4 | テーブル追加 | lease_judgment | リース判定詳細（Q1-Q4回答・免除判定・判定結果） | UI要件 |
| 5 | テーブル追加 | approval_log | 承認ワークフロー履歴（提出/承認/差戻し/再測定） | C3 |
| 6 | テーブル追加 | tax_accounting_diff | 会計税務差異サマリー（期末時点のスナップショット） | C4 |
| 7 | ビュー変更 | v_ctb_export | CTB PDFカラムとのマッピング調整、税務レンジカラム追加 | C1, C4 |
| 8 | カラム追加(CTB) | ctb_lease_integrated | 8カラム追加（#23-30: 会計/税務レンジ） | C1, C4 |

### 2.1 カラム数の変化

| テーブル | v5カラム数 | 追加数 | 変更後 | 上限(22) |
|---------|-----------|--------|--------|----------|
| lease_asset | 15 | +5 | 20 | OK |
| lease_contract | 17 | +5 (注1) | 22 | OK |
| amortization_schedule | 20 | +3 | 23 | 上限超過(注2) |
| ctb_lease_integrated | 22 | +8 | 30 | CTBは上限適用外 |

> **注1**: v5にはすでに`status`カラムが存在するため、新規追加は5カラム（submitted_by, submitted_at, approved_by, approved_at, input_role）。CHECK制約とDEFAULT値の変更のみ。17+5=22列で上限内。
> **注2**: amortization_scheduleの23カラムは上限を1超過するが、税務償却額は会計償却額と同一行で管理すべきであり、テーブル分割は税効果計算のパフォーマンスに悪影響を与えるため許容する。

---

## 3. 新規テーブル詳細DDL

### 3.1 lease_judgment（リース判定詳細）

リース判定の4要件（ASBJ#34準拠のQ1-Q4）と免除規定の適用判断を永続化する。FrmLeaseContractMainのタブ4「リース判定」に対応。

> **Part2 UI対応**: Q1-Q4はPart2 Tab4のRadioButtonバインド先と一致させる（q1_asset_identified, q2_substitution_right, q3_economic_benefit, q4_direction_right）。加えてIFRS16ファイナンスリース分類判定の数値指標（経済的耐用年数比率、PV比率）も保持する。

#### カラム定義表

| # | カラム名 | データ型 | Null | デフォルト | 説明 |
|---|---------|----------|------|-----------|------|
| 1 | judgment_id | SERIAL | N | (自動採番) | PK |
| 2 | asset_id | INTEGER | N | - | FK→lease_asset(asset_id) |
| 3 | judgment_date | DATE | Y | - | 判定実施日 |
| 4 | judgment_type | VARCHAR(20) | Y | - | 'finance' / 'operating' |
| 5 | q1_asset_identified | BOOLEAN | Y | NULL | Q1: 資産の特定（リース該当判定） |
| 6 | q2_substitution_right | BOOLEAN | Y | NULL | Q2: 実質的代替権（なし=リース該当） |
| 7 | q3_economic_benefit | BOOLEAN | Y | NULL | Q3: 経済的利益（あり=リース該当） |
| 8 | q4_direction_right | BOOLEAN | Y | NULL | Q4: 使用指図権（あり=リース該当） |
| 9 | is_lease | BOOLEAN | Y | NULL | 総合判定: リースに該当するか |
| 10 | ownership_transfer | BOOLEAN | Y | FALSE | 所有権移転の有無 |
| 11 | purchase_option_reasonably_certain | BOOLEAN | Y | FALSE | 購入オプション行使が合理的に確実か |
| 12 | economic_life_ratio | NUMERIC(5,2) | Y | - | 経済的耐用年数に対するリース期間比率(%) |
| 13 | pv_ratio | NUMERIC(5,2) | Y | - | 公正価値に対するPV比率(%) |
| 14 | specialized_asset | BOOLEAN | Y | FALSE | 特殊仕様資産フラグ |
| 15 | service_component_flag | BOOLEAN | Y | FALSE | サービス構成要素分離フラグ |
| 16 | is_short_term | BOOLEAN | Y | FALSE | 短期リース該当（12ヶ月以下） |
| 17 | is_low_value | BOOLEAN | Y | FALSE | 少額リース該当 |
| 18 | exemption_applied | BOOLEAN | Y | FALSE | 免除規定適用 |
| 19 | judgment_result | VARCHAR(20) | Y | - | 'non_lease' / 'off_balance' / 'on_balance' |
| 20 | judgment_basis | TEXT | Y | - | 判定根拠（自由記述） |
| 21 | create_dt | TIMESTAMP | N | CURRENT_TIMESTAMP | 作成日時 |
| 22 | update_dt | TIMESTAMP | N | CURRENT_TIMESTAMP | 更新日時 |

#### DDL

```sql
-- =============================================================
-- lease_judgment: リース判定詳細テーブル
-- 層: 契約・資産層（拡張）
-- 用途: リース判定Q1-Q4回答・ファイナンス分類指標・免除判定・判定結果の永続化
-- =============================================================
CREATE TABLE lease_judgment (
    judgment_id                       SERIAL PRIMARY KEY,
    asset_id                          INTEGER NOT NULL
                                      REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    judgment_date                     DATE,
    judgment_type                     VARCHAR(20)
                                      CHECK (judgment_type IN ('finance', 'operating')),

    -- リース該当判定 Q1-Q4（ASBJ#34準拠、Part2 Tab4バインド先）
    q1_asset_identified               BOOLEAN,            -- Q1: 資産の特定
    q2_substitution_right             BOOLEAN,            -- Q2: 実質的代替権
    q3_economic_benefit               BOOLEAN,            -- Q3: 経済的利益
    q4_direction_right                BOOLEAN,            -- Q4: 使用指図権
    is_lease                          BOOLEAN,            -- 総合判定結果

    -- ファイナンスリース分類判定指標（IFRS16）
    ownership_transfer                BOOLEAN DEFAULT FALSE,
    purchase_option_reasonably_certain BOOLEAN DEFAULT FALSE,
    economic_life_ratio               NUMERIC(5,2),       -- 経済的耐用年数比率(%)
    pv_ratio                          NUMERIC(5,2),       -- PV比率(%)
    specialized_asset                 BOOLEAN DEFAULT FALSE,
    service_component_flag            BOOLEAN DEFAULT FALSE,

    -- 免除規定
    is_short_term                     BOOLEAN DEFAULT FALSE,
    is_low_value                      BOOLEAN DEFAULT FALSE,
    exemption_applied                 BOOLEAN DEFAULT FALSE,

    -- 判定結果
    judgment_result                   VARCHAR(20)
                                      CHECK (judgment_result IN ('non_lease', 'off_balance', 'on_balance')),
    judgment_basis                    TEXT,

    -- 監査証跡
    create_dt                         TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                         TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- インデックス
CREATE INDEX idx_lease_judgment_asset_id ON lease_judgment(asset_id);
CREATE INDEX idx_lease_judgment_result ON lease_judgment(judgment_result);

COMMENT ON TABLE lease_judgment IS 'リース判定詳細。Q1-Q4のリース該当判定とIFRS16分類指標・免除判定を保持';
COMMENT ON COLUMN lease_judgment.q1_asset_identified IS 'Q1: 資産が特定されているか。TRUE=特定されている';
COMMENT ON COLUMN lease_judgment.q2_substitution_right IS 'Q2: サプライヤーに実質的代替権があるか。FALSE=リース該当';
COMMENT ON COLUMN lease_judgment.q3_economic_benefit IS 'Q3: 顧客が経済的利益のほぼすべてを享受するか。TRUE=リース該当';
COMMENT ON COLUMN lease_judgment.q4_direction_right IS 'Q4: 顧客が使用を指図する権利を有するか。TRUE=リース該当';
COMMENT ON COLUMN lease_judgment.economic_life_ratio IS 'リース期間 / 経済的耐用年数 の比率(%)。75%以上でファイナンス判定';
COMMENT ON COLUMN lease_judgment.pv_ratio IS 'リース料PV / 公正価値 の比率(%)。90%以上でファイナンス判定';
```

### 3.2 approval_log（承認ワークフロー履歴）

契約のステータス遷移（draft→submitted→approved等）の全履歴を記録する。制約C3（総務/経理分離）のワークフロー実現に必要。

#### カラム定義表

| # | カラム名 | データ型 | Null | デフォルト | 説明 |
|---|---------|----------|------|-----------|------|
| 1 | log_id | SERIAL | N | (自動採番) | PK |
| 2 | contract_id | INTEGER | N | - | FK→lease_contract(contract_id) |
| 3 | action | VARCHAR(20) | N | - | 'submit' / 'approve' / 'reject' / 'remeasure' |
| 4 | action_by | VARCHAR(50) | N | - | 操作者（ユーザーID） |
| 5 | action_date | TIMESTAMP | N | CURRENT_TIMESTAMP | 操作日時 |
| 6 | from_status | VARCHAR(20) | N | - | 遷移前ステータス |
| 7 | to_status | VARCHAR(20) | N | - | 遷移後ステータス |
| 8 | comments | TEXT | Y | - | コメント・差戻し理由 |
| 9 | create_dt | TIMESTAMP | N | CURRENT_TIMESTAMP | 作成日時 |

#### DDL

```sql
-- =============================================================
-- approval_log: 承認ワークフロー履歴テーブル
-- 層: システム管理層
-- 用途: 契約ステータス遷移の全履歴を記録（監査証跡）
-- =============================================================
CREATE TABLE approval_log (
    log_id          SERIAL PRIMARY KEY,
    contract_id     INTEGER NOT NULL
                    REFERENCES lease_contract(contract_id) ON DELETE CASCADE,
    action          VARCHAR(20) NOT NULL
                    CHECK (action IN ('submit', 'approve', 'reject', 'remeasure')),
    action_by       VARCHAR(50) NOT NULL,
    action_date     TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    from_status     VARCHAR(20) NOT NULL,
    to_status       VARCHAR(20) NOT NULL,
    comments        TEXT,
    create_dt       TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- インデックス
CREATE INDEX idx_approval_log_contract_id ON approval_log(contract_id);
CREATE INDEX idx_approval_log_action_date ON approval_log(action_date);
CREATE INDEX idx_approval_log_action_by ON approval_log(action_by);

COMMENT ON TABLE approval_log IS '承認ワークフロー履歴。契約のステータス遷移を全件記録';
COMMENT ON COLUMN approval_log.action IS '操作種別: submit=経理へ提出, approve=承認, reject=差戻し, remeasure=再測定';
```

#### ステータス遷移図

```
                    ┌──────────────────────┐
                    │     draft (下書き)     │
                    └──────────┬───────────┘
                               │ submit（総務→経理）
                               ▼
                    ┌──────────────────────┐
              ┌─────│   submitted (提出済)   │─────┐
              │     └──────────────────────┘     │
              │ reject（差戻し）           approve（承認）
              ▼                                   ▼
    ┌──────────────┐                   ┌──────────────────────┐
    │    draft     │                   │   approved (承認済)    │
    │  (差戻し後)   │                   └──────────┬───────────┘
    └──────────────┘                              │ activate
                                                  ▼
                                       ┌──────────────────────┐
                                       │    active (稼働中)     │
                                       └──────────┬───────────┘
                                                  │ remeasure
                                                  ▼
                                       ┌──────────────────────┐
                                       │ remeasuring (再測定中) │
                                       └──────────┬───────────┘
                                                  │ approve
                                                  ▼
                                       ┌──────────────────────┐
                                       │    active (稼働中)     │
                                       └──────────────────────┘
```

### 3.3 tax_accounting_diff（会計税務差異サマリー）

会計と税務の減価償却差異を期間別に保持し、繰延税金資産/負債の算定根拠とする。制約C4（会計/税務レンジの保持）の実現に不可欠。

#### カラム定義表

| # | カラム名 | データ型 | Null | デフォルト | 説明 |
|---|---------|----------|------|-----------|------|
| 1 | diff_id | SERIAL | N | (自動採番) | PK |
| 2 | asset_id | INTEGER | N | - | FK→lease_asset(asset_id) |
| 3 | fiscal_year | INTEGER | N | - | 会計年度（例: 2026） |
| 4 | period | INTEGER | N | - | 会計期間（1-12） |
| 5 | accounting_depreciation | NUMERIC(15,2) | Y | - | 会計上の減価償却額 |
| 6 | tax_depreciation | NUMERIC(15,2) | Y | - | 税務上の減価償却額 |
| 7 | depreciation_diff | NUMERIC(15,2) | Y | - | 減価償却差異（会計-税務） |
| 8 | accounting_carrying | NUMERIC(15,2) | Y | - | 会計上の帳簿価額 |
| 9 | tax_carrying | NUMERIC(15,2) | Y | - | 税務上の帳簿価額 |
| 10 | carrying_diff | NUMERIC(15,2) | Y | - | 帳簿価額差異（会計-税務）= 一時差異 |
| 11 | cumulative_diff | NUMERIC(15,2) | Y | - | 累積一時差異 |
| 12 | deferred_tax_asset | NUMERIC(15,2) | Y | - | 繰延税金資産 |
| 13 | deferred_tax_liability | NUMERIC(15,2) | Y | - | 繰延税金負債 |
| 14 | create_dt | TIMESTAMP | N | CURRENT_TIMESTAMP | 作成日時 |
| 15 | update_dt | TIMESTAMP | N | CURRENT_TIMESTAMP | 更新日時 |

#### DDL

```sql
-- =============================================================
-- tax_accounting_diff: 会計税務差異サマリーテーブル
-- 層: 会計・測定層（拡張）
-- 用途: 期末時点の会計/税務差異スナップショット。税効果会計の根拠データ
-- =============================================================
CREATE TABLE tax_accounting_diff (
    diff_id                  SERIAL PRIMARY KEY,
    asset_id                 INTEGER NOT NULL
                             REFERENCES lease_asset(asset_id) ON DELETE CASCADE,
    fiscal_year              INTEGER NOT NULL,
    period                   INTEGER NOT NULL
                             CHECK (period BETWEEN 1 AND 12),

    -- 減価償却差異
    accounting_depreciation  NUMERIC(15,2),    -- 会計上の減価償却額
    tax_depreciation         NUMERIC(15,2),    -- 税務上の減価償却額
    depreciation_diff        NUMERIC(15,2),    -- 減価償却差異（会計-税務）

    -- 帳簿価額差異
    accounting_carrying      NUMERIC(15,2),    -- 会計上の帳簿価額
    tax_carrying             NUMERIC(15,2),    -- 税務上の帳簿価額
    carrying_diff            NUMERIC(15,2),    -- 帳簿価額差異 = 一時差異

    -- 税効果
    cumulative_diff          NUMERIC(15,2),    -- 累積一時差異
    deferred_tax_asset       NUMERIC(15,2),    -- 繰延税金資産
    deferred_tax_liability   NUMERIC(15,2),    -- 繰延税金負債

    -- 監査証跡
    create_dt                TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- 一意制約: 1資産・1年度・1期間につき1レコード
    UNIQUE(asset_id, fiscal_year, period)
);

-- インデックス
CREATE INDEX idx_tax_acct_diff_asset_id ON tax_accounting_diff(asset_id);
CREATE INDEX idx_tax_acct_diff_fiscal ON tax_accounting_diff(fiscal_year, period);

COMMENT ON TABLE tax_accounting_diff IS '会計税務差異サマリー。期末時点のスナップショットとして税効果会計の根拠を保持';
COMMENT ON COLUMN tax_accounting_diff.carrying_diff IS '一時差異 = 会計帳簿価額 - 税務帳簿価額。正値は将来減算一時差異（繰延税金資産）';
COMMENT ON COLUMN tax_accounting_diff.cumulative_diff IS '当期末時点の累積一時差異。期首累積 + 当期差異で算出';
```

---

## 4. 既存テーブルへのカラム追加DDL

### 4.1 lease_asset（15列 → 20列）

税務パラメータとCTB連携に必要なカラムを追加。

```sql
-- =============================================================
-- lease_asset: 税務レンジカラム + CTB連携カラムの追加
-- =============================================================

-- 税務耐用年数（月）: 法定耐用年数（耐用年数省令に基づく）
ALTER TABLE lease_asset
    ADD COLUMN tax_useful_life_months INTEGER;
COMMENT ON COLUMN lease_asset.tax_useful_life_months
    IS '税務上の耐用年数（月）。法定耐用年数を月単位で設定。会計上のuseful_life_monthsと対比';

-- 税務償却方法: 定額法(SL)/定率法(DB)
ALTER TABLE lease_asset
    ADD COLUMN tax_depreciation_method VARCHAR(10) DEFAULT 'SL';
COMMENT ON COLUMN lease_asset.tax_depreciation_method
    IS '税務上の減価償却方法。SL=定額法, DB=定率法。会計上のdepreciation_methodと対比';

-- 税務残存簿価（備忘価額）: 通常1円
ALTER TABLE lease_asset
    ADD COLUMN tax_residual_value NUMERIC(15,2) DEFAULT 1;
COMMENT ON COLUMN lease_asset.tax_residual_value
    IS '税務上の残存簿価。備忘価額として通常1円を設定';

-- セグメントCD: CTB連携(segment_cd)に必要
ALTER TABLE lease_asset
    ADD COLUMN segment_code VARCHAR(10);
COMMENT ON COLUMN lease_asset.segment_code
    IS 'セグメントコード。会計分析用区分。CTBのsegment_cdにマッピング';

-- 備考: UI要件
ALTER TABLE lease_asset
    ADD COLUMN remarks VARCHAR(500);
COMMENT ON COLUMN lease_asset.remarks
    IS '備考。資産に関する自由記述フィールド';
```

### 4.2 lease_contract（17列 → 22列）

ステータスのCHECK制約変更と承認ワークフロー情報カラムを追加。

> **注意**: v5にはすでに `status` カラム（#14, VARCHAR(20), DEFAULT='active'）が存在する。
> 本変更ではstatusカラムの追加ではなく、既存CHECK制約の変更とDEFAULT値の変更を行う。
> したがって追加カラムは5つ（submitted_by, submitted_at, approved_by, approved_at, input_role）。
> 変更後のカラム数: 17 + 5 = **22列**（上限内）。

```sql
-- =============================================================
-- lease_contract: ステータスCHECK制約の変更 + 承認ワークフローカラムの追加
-- =============================================================

-- 既存statusカラムのCHECK制約を変更（v5ではactive/terminated/expiredのみ）
ALTER TABLE lease_contract
    DROP CONSTRAINT IF EXISTS chk_lease_contract_status;
ALTER TABLE lease_contract
    ADD CONSTRAINT chk_lease_contract_status
    CHECK (status IN ('draft', 'submitted', 'approved', 'active', 'remeasuring', 'terminated'));
-- DEFAULT値を'draft'に変更（新規契約は下書きから開始）
ALTER TABLE lease_contract
    ALTER COLUMN status SET DEFAULT 'draft';
COMMENT ON COLUMN lease_contract.status
    IS '契約ステータス。draft=下書き, submitted=提出済, approved=承認済, active=稼働中, remeasuring=再測定中, terminated=終了。（v5からCHECK制約を拡張）';

-- 提出者: 経理への提出を行った総務担当者
ALTER TABLE lease_contract
    ADD COLUMN submitted_by VARCHAR(50);
COMMENT ON COLUMN lease_contract.submitted_by
    IS '提出者のユーザーID（総務担当者）';

-- 提出日時
ALTER TABLE lease_contract
    ADD COLUMN submitted_at TIMESTAMP;
COMMENT ON COLUMN lease_contract.submitted_at
    IS '経理への提出日時';

-- 承認者: 承認を行った経理担当者
ALTER TABLE lease_contract
    ADD COLUMN approved_by VARCHAR(50);
COMMENT ON COLUMN lease_contract.approved_by
    IS '承認者のユーザーID（経理担当者）';

-- 承認日時
ALTER TABLE lease_contract
    ADD COLUMN approved_at TIMESTAMP;
COMMENT ON COLUMN lease_contract.approved_at
    IS '経理による承認日時';

-- 入力ロール: 最後に編集したロール
ALTER TABLE lease_contract
    ADD COLUMN input_role VARCHAR(10)
    CHECK (input_role IN ('soumu', 'keiri', 'admin'));
COMMENT ON COLUMN lease_contract.input_role
    IS '最終入力ロール。soumu=総務, keiri=経理, admin=管理者';
```

### 4.3 amortization_schedule（20列 → 23列）

税務償却額と税務帳簿価額を会計値と並行して保持。

```sql
-- =============================================================
-- amortization_schedule: 税務レンジカラムの追加
-- =============================================================

-- 税務上の減価償却額
ALTER TABLE amortization_schedule
    ADD COLUMN tax_depreciation_amount NUMERIC(15,2);
COMMENT ON COLUMN amortization_schedule.tax_depreciation_amount
    IS '税務上の減価償却額。tax_useful_life_monthsとtax_depreciation_methodに基づき算出';

-- 税務上の減価償却累計額
ALTER TABLE amortization_schedule
    ADD COLUMN tax_accumulated_depreciation NUMERIC(15,2);
COMMENT ON COLUMN amortization_schedule.tax_accumulated_depreciation
    IS '税務上の減価償却累計額';

-- 税務上の帳簿価額
ALTER TABLE amortization_schedule
    ADD COLUMN tax_carrying_amount NUMERIC(15,2);
COMMENT ON COLUMN amortization_schedule.tax_carrying_amount
    IS '税務上の帳簿価額 = 取得価額 - 税務減価償却累計額';
```

---

## 5. v_ctb_export ビューの再定義

### 5.1 CTB PDFカラムとv5テーブルのマッピング

CTB PDFの全30カラム（既存22 + 追加8）を、v5正規化テーブルからSELECTする。

| # | CTBカラム | v5ソーステーブル | v5ソースカラム/算出式 | 備考 |
|---|----------|----------------|---------------------|------|
| 1 | ctb_id | lease_asset | asset_id | v5ではasset_idがPK |
| 2 | contract_no | lease_contract | contract_no | 1:1 |
| 3 | property_no | (算出) | ROW_NUMBER() OVER(PARTITION BY contract_id) | 物件枝番を動的算出 |
| 4 | m7_asset_no | external_mapping | external_key WHERE system='M7' | EXT層経由 |
| 5 | jsm10_aro_no | external_mapping | external_key WHERE system='JSM10' | EXT層経由 |
| 6 | lease_start_date | lease_contract | contract_start_date | カラム名変換 |
| 7 | non_cancellable_months | (算出) | age(contract_end_date, contract_start_date) | 月数算出 |
| 8 | is_extension_certain | lease_option | is_reasonably_certain | type='extend' |
| 9 | extension_months | lease_option | option_months | type='extend' |
| 10 | accounting_lease_term | (算出) | non_cancellable + CASE WHEN extend THEN extension END | 条件算出 |
| 11 | periodic_payment_amt | lease_payment_schedule | payment_amount | CAST(NUMERIC(15,0)) |
| 12 | payment_interval_months | lease_contract | payment_interval_months | 1:1（v5にカラム追加済の場合）、算出の場合はサブクエリ |
| 13 | discount_rate | lease_initial_measurement | discount_rate_used | CAST(NUMERIC(5,4)) |
| 14 | residual_value_guarantee | lease_asset | residual_value_guarantee | CAST(NUMERIC(15,0)) |
| 15 | purchase_option_amt | lease_initial_measurement | purchase_option_price | CAST(NUMERIC(15,0)) |
| 16 | aro_present_value | restoration_obligation | pv_amount | CAST(NUMERIC(15,0)) |
| 17 | m7_dept_cd | lease_asset | mgmt_dept_cd | 1:1 |
| 18 | burden_dept_cd | dept_allocation | dept_code | 主配賦先 |
| 19 | asset_class_cd | lease_asset | asset_category_code | 1:1 |
| 20 | segment_cd | lease_asset | segment_code | v5追加カラム |
| 21 | initial_rou_asset | lease_initial_measurement | rou_amount | CAST(NUMERIC(15,0)) |
| 22 | initial_lease_liability | lease_initial_measurement | liability_amount | CAST(NUMERIC(15,0)) |
| 23 | tax_useful_life_months | lease_asset | tax_useful_life_months | v5追加カラム |
| 24 | acct_useful_life_months | lease_asset | useful_life_months | 既存カラム |
| 25 | tax_depreciation_method | lease_asset | tax_depreciation_method | v5追加カラム |
| 26 | acct_depreciation_method | lease_asset | depreciation_method | 既存カラム |
| 27 | tax_depreciation_annual | amortization_schedule | SUM(tax_depreciation_amount) WHERE 直近12ヶ月 | 年間集計 |
| 28 | acct_depreciation_annual | amortization_schedule | SUM(depreciation_amount) WHERE 直近12ヶ月 | 年間集計 |
| 29 | tax_rou_asset | (算出) | 税法ベースの取得価額 | リース料総額等 |
| 30 | tax_acct_diff_annual | (算出) | tax_depreciation_annual - acct_depreciation_annual | 差額算出 |

### 5.2 マテリアライズドビューDDL

```sql
-- =============================================================
-- v_ctb_export: CTB互換マテリアライズドビュー（再定義）
-- CTB PDFの全30カラムに対応
-- =============================================================
DROP MATERIALIZED VIEW IF EXISTS v_ctb_export;

CREATE MATERIALIZED VIEW v_ctb_export AS
SELECT
    -- ==============================
    -- 既存22カラム（CTB PDF定義書準拠）
    -- ==============================
    a.asset_id                                AS ctb_id,
    c.contract_no                             AS contract_no,
    ROW_NUMBER() OVER (
        PARTITION BY c.contract_id
        ORDER BY a.asset_id
    )                                         AS property_no,
    em_m7.external_key                        AS m7_asset_no,
    em_jsm.external_key                       AS jsm10_aro_no,
    c.contract_start_date                     AS lease_start_date,

    -- 解約不能期間（月数算出）
    (EXTRACT(YEAR FROM age(c.contract_end_date, c.contract_start_date)) * 12
     + EXTRACT(MONTH FROM age(c.contract_end_date, c.contract_start_date))
    )::INTEGER                                AS non_cancellable_months,

    lo.is_reasonably_certain                  AS is_extension_certain,
    lo.option_months                          AS extension_months,

    -- 会計リース期間（延長オプション考慮）
    (
        (EXTRACT(YEAR FROM age(c.contract_end_date, c.contract_start_date)) * 12
         + EXTRACT(MONTH FROM age(c.contract_end_date, c.contract_start_date)))
        + CASE
            WHEN lo.is_reasonably_certain = TRUE THEN COALESCE(lo.option_months, 0)
            ELSE 0
          END
    )::INTEGER                                AS accounting_lease_term,

    -- 1支払額
    CAST(COALESCE(
        (SELECT ps.payment_amount
         FROM lease_payment_schedule ps
         WHERE ps.asset_id = a.asset_id
         ORDER BY ps.payment_date
         LIMIT 1), 0
    ) AS NUMERIC(15,0))                       AS periodic_payment_amt,

    -- 支払間隔
    COALESCE(
        (SELECT EXTRACT(MONTH FROM age(ps2.payment_date, ps1.payment_date))::INTEGER
         FROM lease_payment_schedule ps1
         JOIN lease_payment_schedule ps2
           ON ps2.asset_id = ps1.asset_id
          AND ps2.payment_date > ps1.payment_date
         WHERE ps1.asset_id = a.asset_id
         ORDER BY ps1.payment_date
         LIMIT 1),
        1
    )                                         AS payment_interval_months,

    CAST(im.discount_rate_used AS NUMERIC(5,4)) AS discount_rate,

    CAST(a.residual_value_guarantee AS NUMERIC(15,0))
                                              AS residual_value_guarantee,
    CAST(im.purchase_option_price AS NUMERIC(15,0))
                                              AS purchase_option_amt,
    CAST(ro.pv_amount AS NUMERIC(15,0))       AS aro_present_value,

    a.mgmt_dept_cd                            AS m7_dept_cd,
    da_main.dept_code                         AS burden_dept_cd,
    a.asset_category_code                     AS asset_class_cd,
    a.segment_code                            AS segment_cd,

    CAST(im.rou_amount AS NUMERIC(15,0))      AS initial_rou_asset,
    CAST(im.liability_amount AS NUMERIC(15,0)) AS initial_lease_liability,

    -- ==============================
    -- 追加8カラム（会計/税務レンジ）
    -- ==============================
    a.tax_useful_life_months                  AS tax_useful_life_months,
    a.useful_life_months                      AS acct_useful_life_months,
    a.tax_depreciation_method                 AS tax_depreciation_method,
    a.depreciation_method                     AS acct_depreciation_method,

    -- 年間税務償却額（直近12ヶ月の合計）
    CAST(latest_amort.tax_dep_annual AS NUMERIC(15,0))
                                              AS tax_depreciation_annual,
    -- 年間会計償却額（直近12ヶ月の合計）
    CAST(latest_amort.acct_dep_annual AS NUMERIC(15,0))
                                              AS acct_depreciation_annual,

    -- 税法上のROU資産額（リース料総額ベース）
    CAST(
        COALESCE(
            (SELECT SUM(ps.payment_amount)
             FROM lease_payment_schedule ps
             WHERE ps.asset_id = a.asset_id),
            0
        ) AS NUMERIC(15,0)
    )                                         AS tax_rou_asset,

    -- 年間税会差異
    CAST(
        COALESCE(latest_amort.tax_dep_annual, 0)
        - COALESCE(latest_amort.acct_dep_annual, 0)
    AS NUMERIC(15,0))                         AS tax_acct_diff_annual

FROM lease_contract c
INNER JOIN lease_asset a
    ON c.contract_id = a.contract_id

-- M7外部マッピング
LEFT JOIN external_mapping em_m7
    ON a.asset_id = em_m7.asset_id
    AND em_m7.external_system = 'M7'

-- JSM10外部マッピング
LEFT JOIN external_mapping em_jsm
    ON a.asset_id = em_jsm.asset_id
    AND em_jsm.external_system = 'JSM10'

-- 初期測定
LEFT JOIN lease_initial_measurement im
    ON a.asset_id = im.asset_id

-- 主配賦先部署（配賦率最大の部署）
LEFT JOIN LATERAL (
    SELECT dept_code
    FROM dept_allocation
    WHERE asset_id = a.asset_id
    ORDER BY allocation_ratio DESC
    LIMIT 1
) da_main ON TRUE

-- 原状回復義務
LEFT JOIN restoration_obligation ro
    ON a.asset_id = ro.asset_id

-- 延長オプション
LEFT JOIN lease_option lo
    ON a.asset_id = lo.asset_id
    AND lo.option_type = 'extend'

-- 直近12ヶ月の償却額集計
LEFT JOIN LATERAL (
    SELECT
        SUM(ams.tax_depreciation_amount) AS tax_dep_annual,
        SUM(ams.depreciation_amount) AS acct_dep_annual
    FROM amortization_schedule ams
    WHERE ams.asset_id = a.asset_id
      AND ams.schedule_date >= (CURRENT_DATE - INTERVAL '12 months')
      AND ams.schedule_date <= CURRENT_DATE
) latest_amort ON TRUE;

-- ユニークインデックス（CONCURRENTLY REFRESH用）
CREATE UNIQUE INDEX idx_v_ctb_export_ctb_id ON v_ctb_export(ctb_id);

-- 検索用インデックス
CREATE INDEX idx_v_ctb_export_contract_no ON v_ctb_export(contract_no);
CREATE INDEX idx_v_ctb_export_lease_start ON v_ctb_export(lease_start_date);

COMMENT ON MATERIALIZED VIEW v_ctb_export
    IS 'CTB互換マテリアライズドビュー。v5正規化テーブル群をJOINしてCTBフラット構造（30カラム）を再現。REFRESH MATERIALIZED VIEW CONCURRENTLY で更新';
```

---

## 6. CTBフラットテーブルとv5正規化テーブルの関係図

```
┌─────────────────────────────────────────────────────────────────────┐
│                    v5 正規化テーブル群 (40テーブル)                     │
│                                                                     │
│  ┌─────────────────┐  ┌──────────────────────┐  ┌───────────────┐  │
│  │ lease_contract   │  │ lease_asset           │  │ lease_option  │  │
│  │  + status        │  │  + tax_useful_life    │  │               │  │
│  │  + submitted_by  │  │  + tax_depr_method    │  │               │  │
│  │  + approved_by   │  │  + segment_code       │  │               │  │
│  └────────┬────────┘  └──────────┬───────────┘  └───────┬───────┘  │
│           │                      │                      │          │
│           │  ┌───────────────────┼──────────────────────┘          │
│           │  │                   │                                  │
│           ▼  ▼                   ▼                                  │
│  ┌─────────────────────────────────────────────────────┐           │
│  │              lease_initial_measurement               │           │
│  └───────────────────────┬─────────────────────────────┘           │
│                          │                                          │
│           ┌──────────────┼──────────────┐                          │
│           ▼              ▼              ▼                           │
│  ┌─────────────┐ ┌────────────┐ ┌──────────────────┐              │
│  │amortization │ │ dept_      │ │ restoration_     │              │
│  │_schedule    │ │ allocation │ │ obligation       │              │
│  │ +tax_depr   │ │            │ │                  │              │
│  │ +tax_carry  │ │            │ │                  │              │
│  └──────┬──────┘ └────────────┘ └──────────────────┘              │
│         │                                                          │
│         │  ┌──────────────────┐  ┌──────────────────┐             │
│         │  │ external_mapping │  │ lease_payment_   │             │
│         │  │  (M7/JSM10)     │  │ schedule         │             │
│         │  └──────────────────┘  └──────────────────┘             │
│         │                                                          │
│  ┌──────┼──────────────────────────────────┐                      │
│  │      ▼    新規テーブル                    │                      │
│  │  ┌────────────────┐ ┌────────────────┐  │                      │
│  │  │ lease_judgment  │ │ approval_log   │  │                      │
│  │  └────────────────┘ └────────────────┘  │                      │
│  │  ┌────────────────────┐                 │                      │
│  │  │ tax_accounting_diff│                 │                      │
│  │  └────────────────────┘                 │                      │
│  └─────────────────────────────────────────┘                      │
│                                                                     │
└──────────────────────────┬──────────────────────────────────────────┘
                           │
                           │  JOIN + 算出カラム
                           ▼
┌─────────────────────────────────────────────────────────────────────┐
│              v_ctb_export (MATERIALIZED VIEW)                        │
│                                                                     │
│  既存22カラム（CTB PDF定義書準拠）                                      │
│    ctb_id, contract_no, property_no, m7_asset_no, jsm10_aro_no,     │
│    lease_start_date, non_cancellable_months, is_extension_certain,  │
│    extension_months, accounting_lease_term, periodic_payment_amt,   │
│    payment_interval_months, discount_rate, residual_value_guarantee,│
│    purchase_option_amt, aro_present_value, m7_dept_cd,              │
│    burden_dept_cd, asset_class_cd, segment_cd,                      │
│    initial_rou_asset, initial_lease_liability                       │
│  + 追加8カラム（会計/税務レンジ）                                       │
│    tax_useful_life_months, acct_useful_life_months,                  │
│    tax_depreciation_method, acct_depreciation_method,               │
│    tax_depreciation_annual, acct_depreciation_annual,               │
│    tax_rou_asset, tax_acct_diff_annual                              │
│                                                                     │
└──────────────────────────┬──────────────────────────────────────────┘
                           │
                           │  REFRESH MATERIALIZED VIEW CONCURRENTLY
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────────┐
│              外部システム連携                                          │
│                                                                     │
│  ┌──────────┐  ┌──────────┐  ┌──────────────────┐                  │
│  │ シサンM7  │  │  JSM10   │  │ 新基準パッケージ   │                  │
│  │ (固定資産) │  │ (除去債務)│  │ (計算エンジン)    │                  │
│  └──────────┘  └──────────┘  └──────────────────┘                  │
│                                                                     │
│  → v_ctb_export を参照（CTBフラット構造として利用）                     │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### データフローの方向

```
書き込み:  UI → Repository層 → v5正規化テーブル群（唯一の書き込み先）
読み取り:  FrmFlexCtbViewer → v_ctb_export（マテリアライズドビュー）
連携:      外部システム → v_ctb_export（読み取り専用）
```

---

## 7. データ整合性保証

### 7.1 課題

v5正規化テーブルへのINSERT/UPDATEが行われた後、v_ctb_exportマテリアライズドビューの内容が即座には更新されない。外部システムやFrmFlexCtbViewerが古いデータを参照するリスクがある。

### 7.2 リフレッシュ戦略の比較

| # | 戦略 | 方式 | メリット | デメリット | 推奨度 |
|---|------|------|----------|-----------|--------|
| A | トリガーベース | v5テーブルのINSERT/UPDATE/DELETEトリガーでREFRESH実行 | リアルタイム同期 | トリガー内のREFRESHは重い。デッドロックリスク。トランザクション内の可視性問題 | 低 |
| B | バッチ更新 | pg_cronで定期的にREFRESH MATERIALIZED VIEW CONCURRENTLY | 実装シンプル。パフォーマンス予測可能 | リフレッシュ間隔分の遅延 | **高** |
| C | アプリケーション層制御 | 保存ボタン押下後にREFRESHをRepository層から明示呼出 | 必要なタイミングのみ更新 | アプリケーションの全書き込みパスでREFRESH呼出が必要。漏れリスク | 中 |
| D | ハイブリッド(B+C) | 通常はバッチ。契約承認時など重要操作時にはアプリから即時REFRESH | 重要操作はリアルタイム。通常は定期更新 | 実装がやや複雑 | **最高** |

### 7.3 推奨: ハイブリッド方式（D）

```
通常の更新フロー:
  v5テーブルへのINSERT/UPDATE
    → 即時反映なし
    → pg_cronによる定期REFRESH（5分間隔）

重要操作時の即時更新フロー:
  契約承認(approve)時 / 月次締め処理時 / 手動リフレッシュ時
    → アプリケーション層からREFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export
    → FrmFlexCtbViewerの[再読込]ボタンでも手動トリガー可能
```

#### バッチジョブ設定（pg_cron）

```sql
-- pg_cronによる定期リフレッシュ（5分間隔）
SELECT cron.schedule(
    'refresh_v_ctb_export',
    '*/5 * * * *',
    'REFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export'
);
```

#### アプリケーション層からの即時リフレッシュ

```sql
-- 承認処理時などにRepository層から呼び出し
REFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export;
```

> **CONCURRENTLYオプション**: ユニークインデックス（idx_v_ctb_export_ctb_id）が存在するため使用可能。リフレッシュ中もSELECTがブロックされない。

### 7.4 整合性チェック

定期バッチで以下の整合性チェックを実施する。

```sql
-- v5テーブルの件数とv_ctb_exportの件数の一致確認
SELECT
    (SELECT COUNT(*) FROM lease_asset WHERE is_deleted = FALSE) AS v5_count,
    (SELECT COUNT(*) FROM v_ctb_export) AS ctb_count,
    CASE
        WHEN (SELECT COUNT(*) FROM lease_asset WHERE is_deleted = FALSE)
             = (SELECT COUNT(*) FROM v_ctb_export)
        THEN 'OK'
        ELSE 'MISMATCH - REFRESH REQUIRED'
    END AS check_result;
```

---

## 8. インデックス・パフォーマンス設計

### 8.1 追加インデックス一覧

#### 新規テーブルのインデックス（セクション3のDDLに含む）

| テーブル | インデックス名 | カラム | 種別 | 用途 |
|---------|--------------|--------|------|------|
| lease_judgment | idx_lease_judgment_asset_id | asset_id | B-Tree | FK検索 |
| lease_judgment | idx_lease_judgment_result | judgment_result | B-Tree | 判定結果フィルタ |
| approval_log | idx_approval_log_contract_id | contract_id | B-Tree | FK検索 |
| approval_log | idx_approval_log_action_date | action_date | B-Tree | 時系列検索 |
| approval_log | idx_approval_log_action_by | action_by | B-Tree | 担当者検索 |
| tax_accounting_diff | idx_tax_acct_diff_asset_id | asset_id | B-Tree | FK検索 |
| tax_accounting_diff | idx_tax_acct_diff_fiscal | (fiscal_year, period) | B-Tree | 期間検索 |

#### 既存テーブルへの追加インデックス

| テーブル | インデックス名 | カラム | 種別 | 用途 |
|---------|--------------|--------|------|------|
| lease_contract | idx_lease_contract_status | status | B-Tree | ステータスフィルタ（ダッシュボード、一覧画面） |
| lease_contract | idx_lease_contract_submitted | submitted_at | B-Tree | 提出日時での検索（承認待ち一覧） |
| lease_asset | idx_lease_asset_segment | segment_code | B-Tree | セグメント別検索 |

```sql
-- 既存テーブルへの追加インデックス
CREATE INDEX idx_lease_contract_status ON lease_contract(status);
CREATE INDEX idx_lease_contract_submitted ON lease_contract(submitted_at)
    WHERE submitted_at IS NOT NULL;
CREATE INDEX idx_lease_asset_segment ON lease_asset(segment_code)
    WHERE segment_code IS NOT NULL;
```

#### v_ctb_exportのインデックス（セクション5のDDLに含む）

| インデックス名 | カラム | 種別 | 用途 |
|--------------|--------|------|------|
| idx_v_ctb_export_ctb_id | ctb_id | UNIQUE | CONCURRENTLY REFRESH用の必須ユニークインデックス |
| idx_v_ctb_export_contract_no | contract_no | B-Tree | 契約番号検索（FrmFlexCtbViewerのフィルタ） |
| idx_v_ctb_export_lease_start | lease_start_date | B-Tree | 期間検索 |

### 8.2 v_ctb_exportマテリアライズドビューのパフォーマンス考慮

#### リフレッシュ頻度の設計

| 状況 | リフレッシュ頻度 | トリガー |
|------|----------------|---------|
| 通常運用 | 5分間隔 | pg_cronジョブ |
| 月次締め処理 | 即時 | アプリケーション層から呼出 |
| 契約承認時 | 即時 | approval_logへのINSERT後にRepository層から呼出 |
| 手動 | 任意 | FrmFlexCtbViewerの[再読込]ボタン |
| 夜間バッチ | 1日1回（01:00） | pg_cronジョブ（FULL REFRESH） |

#### CONCURRENTLY vs 通常のREFRESH

| 方式 | 速度 | ロック | 用途 |
|------|------|--------|------|
| REFRESH MATERIALIZED VIEW v_ctb_export | 高速 | 排他ロック（SELECT不可） | 夜間バッチ |
| REFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export | やや遅い | 共有ロック（SELECT可） | 日中の5分間隔リフレッシュ |

```sql
-- 夜間バッチ（排他ロック可）: FULL REFRESH
SELECT cron.schedule(
    'refresh_v_ctb_export_nightly',
    '0 1 * * *',
    'REFRESH MATERIALIZED VIEW v_ctb_export'
);

-- 日中リフレッシュ（ユーザーアクセス中）: CONCURRENTLY
SELECT cron.schedule(
    'refresh_v_ctb_export_daytime',
    '*/5 8-20 * * 1-5',
    'REFRESH MATERIALIZED VIEW CONCURRENTLY v_ctb_export'
);
```

### 8.3 想定データ量とパフォーマンス見積

| テーブル | 想定レコード数 | 年間増加率 | リフレッシュ影響 |
|---------|-------------|-----------|----------------|
| lease_contract | 500-2,000件 | +200件/年 | 軽微 |
| lease_asset | 1,000-5,000件 | +500件/年 | 軽微 |
| amortization_schedule | 50,000-200,000件 | +30,000件/年 | 中程度（LATERAL結合のコスト） |
| v_ctb_export | 1,000-5,000行 | +500行/年 | CONCURRENTLY REFRESHで3-10秒 |

> **パフォーマンス推定**: 5,000行規模のv_ctb_exportで、CONCURRENTLY REFRESHの所要時間は3-10秒。5分間隔のリフレッシュでDB負荷は許容範囲内。

---

## 付録A: 変更適用の実行順序

```sql
-- =============================================================
-- マイグレーションスクリプト実行順序
-- =============================================================

-- Step 1: 既存テーブルへのカラム追加（順序不問）
-- 4.1 lease_asset への5カラム追加
-- 4.2 lease_contract への6カラム追加
-- 4.3 amortization_schedule への3カラム追加

-- Step 2: 新規テーブル作成（FK依存順）
-- 3.1 lease_judgment（lease_assetに依存）
-- 3.2 approval_log（lease_contractに依存）
-- 3.3 tax_accounting_diff（lease_assetに依存）

-- Step 3: 追加インデックス作成
-- 8.1 の追加インデックス群

-- Step 4: v_ctb_export マテリアライズドビュー再定義
-- 5.2 のDDL（DROP + CREATE + INDEX）

-- Step 5: pg_cronジョブ登録
-- 7.3 および 8.2 のスケジュール設定
```

## 付録B: 制約適合性の最終チェック

| 制約 | 適合状態 | 根拠 |
|------|---------|------|
| C1: CTB導入、大きな変更なし | **適合** | 既存22カラム維持。8カラムNULL追加のみ。テーブル構造(フラット)維持。v_ctb_exportビューでCTB互換を提供 |
| C2: フレックス画面の存在 | **適合** | FrmFlexCtbViewerは変更なし。v_ctb_exportの30カラムを参照するデータソース変更のみ |
| C3: 総務/経理の明確な分離 | **適合** | lease_contractにstatus/submitted_by/approved_byを追加。approval_logで遷移履歴を完全記録。DBレベルでワークフローを支援 |
| C4: 会計/税務レンジの保持 | **適合** | lease_assetに税務パラメータ3カラム追加。amortization_scheduleに税務償却3カラム追加。tax_accounting_diffテーブルで期間別差異を管理。v_ctb_exportに8カラム追加 |

---

*本ドキュメントの最終更新: 2026-03-13*
*ドキュメント化エージェント Part3 出力*
