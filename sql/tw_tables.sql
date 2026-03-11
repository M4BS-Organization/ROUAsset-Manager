-- psql -U manager -d lease_m4bs -f tw_tables.sql
-- 実行順序: master_tables.sql → tw_tables.sql → ctb_tables.sql

-- tw_lease_contract: リース契約テーブル
CREATE TABLE IF NOT EXISTS tw_lease_contract (
    contract_id         SERIAL          PRIMARY KEY,
    contract_no         VARCHAR(20)     NOT NULL,
    contract_name       VARCHAR(200)    NOT NULL,
    asset_breakdown     VARCHAR(50)     NULL,
    mgmt_dept           VARCHAR(100)    NULL,
    cost_dept           VARCHAR(100)    NULL,
    group_name          VARCHAR(100)    NULL,
    structure_usage     VARCHAR(50)     NULL,
    prohibition         TEXT            NULL,
    start_date          DATE            NOT NULL,
    end_date            DATE            NOT NULL,
    contract_months     INTEGER         NOT NULL,
    free_rent_period    VARCHAR(50)     NULL,
    non_cancel_period   VARCHAR(50)     NULL,
    cancel_option       VARCHAR(10)     NULL,
    cancel_condition    TEXT            NULL,
    extend_option       VARCHAR(10)     NULL,
    extend_condition    TEXT            NULL,
    renewal_count       INTEGER         NULL,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP       NULL,
    CONSTRAINT uq_contract_no UNIQUE (contract_no)
);

-- tw_lease_property: 物件属性テーブル
CREATE TABLE IF NOT EXISTS tw_lease_property (
    property_id         SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    property_name       VARCHAR(200)    NOT NULL,
    section             VARCHAR(100)    NULL,
    area                NUMERIC(10,2)   NULL,
    address             VARCHAR(500)    NULL,
    floor_plan          VARCHAR(50)     NULL,
    completion_date     DATE            NULL,
    scale               VARCHAR(100)    NULL,
    useful_life         INTEGER         NOT NULL,
    building_age        INTEGER         NULL
);

-- tw_lease_party: 関係者テーブル（貸主/代理人/仲介/借主/保証人）
CREATE TABLE IF NOT EXISTS tw_lease_party (
    party_id            SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    party_type          VARCHAR(20)     NOT NULL,
    party_name          VARCHAR(200)    NOT NULL,
    party_address       VARCHAR(500)    NULL,
    party_account       VARCHAR(100)    NULL
);

-- tw_lease_schedule: 支払スケジュールテーブル
CREATE TABLE IF NOT EXISTS tw_lease_schedule (
    schedule_id         SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    schedule_type       VARCHAR(10)     NOT NULL,
    payment_amount      NUMERIC(15,2)   NOT NULL,
    first_payment_date  DATE            NOT NULL,
    payment_interval    VARCHAR(20)     NOT NULL,
    total_count         INTEGER         NOT NULL,
    last_payment_date   DATE            NULL,
    rent_total          NUMERIC(15,2)   NULL,
    linked_index        NUMERIC(10,4)   NULL,
    contract_index      NUMERIC(10,4)   NULL,
    maintenance_cost    NUMERIC(15,2)   NULL,
    residual_guarantee  NUMERIC(15,2)   NULL,
    residual_payment_est NUMERIC(15,2)  NULL
);

-- tw_lease_accounting: 会計計算テーブル
CREATE TABLE IF NOT EXISTS tw_lease_accounting (
    accounting_id           SERIAL          PRIMARY KEY,
    contract_id             INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    accounting_period_months INTEGER        NULL,
    discount_rate           NUMERIC(8,6)    NULL,
    rent_total              NUMERIC(15,2)   NULL,
    calc_total              NUMERIC(15,2)   NULL,
    lease_ratio             NUMERIC(5,2)    NULL,
    non_lease_ratio         NUMERIC(5,2)    NULL,
    present_value           NUMERIC(15,2)   NULL,
    rou_asset               NUMERIC(15,2)   NULL,
    lease_liability         NUMERIC(15,2)   NULL,
    deposit                 NUMERIC(15,2)   NULL,
    prepaid_rent            NUMERIC(15,2)   NULL,
    equipment_cost          NUMERIC(15,2)   NULL,
    alloc_total             NUMERIC(15,2)   NULL,
    maintenance_cost        NUMERIC(15,2)   NULL,
    distribution_rate       NUMERIC(5,2)    NULL,
    accum_depreciation      NUMERIC(15,2)   NULL DEFAULT 0,
    book_value              NUMERIC(15,2)   NULL DEFAULT 0,
    useful_life             INTEGER         NULL,
    depreciation_method     VARCHAR(20)     NULL,
    initial_direct_cost     NUMERIC(15,2)   NULL DEFAULT 0,
    restoration_cost        NUMERIC(15,2)   NULL DEFAULT 0,
    lease_incentive         NUMERIC(15,2)   NULL DEFAULT 0,
    renewal_forecast_count  INTEGER         NULL,
    renewal_pay_date        DATE            NULL,
    calculated_at           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_accounting_contract UNIQUE (contract_id)
);

-- tw_lease_judgment: リース判定テーブル
CREATE TABLE IF NOT EXISTS tw_lease_judgment (
    judgment_id         SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    q1_result           BOOLEAN         NULL,
    q2_result           BOOLEAN         NULL,
    q3_result           BOOLEAN         NULL,
    q4_result           BOOLEAN         NULL,
    is_exempt_short     BOOLEAN         NULL,
    is_exempt_small     BOOLEAN         NULL,
    final_result        VARCHAR(20)     NULL,
    judged_at           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_judgment_contract UNIQUE (contract_id)
);

-- tw_lease_initial: 初回費用テーブル
CREATE TABLE IF NOT EXISTS tw_lease_initial (
    initial_id              SERIAL          PRIMARY KEY,
    contract_id             INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    cost_item_cd            VARCHAR(10)     NULL REFERENCES m_initial_cost_item(cost_item_cd),
    acct_treatment_cd       VARCHAR(10)     NULL REFERENCES m_acct_treatment(acct_treatment_cd),
    amount                  NUMERIC(15,2)   NULL DEFAULT 0,
    remarks                 VARCHAR(500)    NULL,
    created_at              TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_sublease: 転貸情報テーブル
CREATE TABLE IF NOT EXISTS tw_lease_sublease (
    sublease_id         SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    subtenant_name      VARCHAR(200)    NULL,
    subtenant_address   VARCHAR(500)    NULL,
    sublease_income     NUMERIC(15,2)   NULL DEFAULT 0,
    remarks             VARCHAR(500)    NULL,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_payment_actual: 月次支払実績テーブル
CREATE TABLE IF NOT EXISTS tw_lease_payment_actual (
    payment_actual_id   SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    payment_ym          VARCHAR(7)      NOT NULL,
    payment_type        VARCHAR(20)     NOT NULL,
    scheduled_amount    NUMERIC(15,2)   NULL DEFAULT 0,
    actual_amount       NUMERIC(15,2)   NULL DEFAULT 0,
    difference          NUMERIC(15,2)   NULL DEFAULT 0,
    payment_date        DATE            NULL,
    payment_method_cd   VARCHAR(10)     NULL REFERENCES m_payment_method(payment_method_cd),
    remarks             VARCHAR(500)    NULL,
    tax_amount          NUMERIC(15,2)   NULL DEFAULT 0,
    total_amount        NUMERIC(15,2)   NULL DEFAULT 0,
    supplier_name       VARCHAR(200)    NULL,
    status              VARCHAR(20)     NULL DEFAULT '未処理',
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP       NULL
);

-- tw_lease_journal: 月次仕訳テーブル
CREATE TABLE IF NOT EXISTS tw_lease_journal (
    journal_id          SERIAL          PRIMARY KEY,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    journal_ym          VARCHAR(7)      NOT NULL,
    journal_type        VARCHAR(20)     NOT NULL,
    debit_account_cd    VARCHAR(10)     NULL,
    debit_account_name  VARCHAR(100)    NULL,
    debit_amount        NUMERIC(15,2)   NOT NULL DEFAULT 0,
    credit_account_cd   VARCHAR(10)     NULL,
    credit_account_name VARCHAR(100)    NULL,
    credit_amount       NUMERIC(15,2)   NOT NULL DEFAULT 0,
    description         VARCHAR(500)    NULL,
    asbj_reference      VARCHAR(100)    NULL,
    status              VARCHAR(20)     NULL DEFAULT '未処理',
    generated_at        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_period_balance: 期間残高テーブル（FrmFlexPeriodBalance画面用）
CREATE TABLE IF NOT EXISTS tw_lease_period_balance (
    balance_id          SERIAL          PRIMARY KEY,
    fiscal_year         VARCHAR(4)      NOT NULL,
    quarter             VARCHAR(4)      NULL,
    account_item        VARCHAR(100)    NOT NULL,
    opening_balance     NUMERIC(15,2)   NULL DEFAULT 0,
    increase_amount     NUMERIC(15,2)   NULL DEFAULT 0,
    change_adjustment   NUMERIC(15,2)   NULL DEFAULT 0,
    decrease_amount     NUMERIC(15,2)   NULL DEFAULT 0,
    closing_balance     NUMERIC(15,2)   NULL DEFAULT 0,
    display_order       INTEGER         NULL DEFAULT 0,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_balance_breakdown: 残高内訳テーブル（FrmFlexPeriodBalance画面用）
CREATE TABLE IF NOT EXISTS tw_lease_balance_breakdown (
    breakdown_id        SERIAL          PRIMARY KEY,
    fiscal_year         VARCHAR(4)      NOT NULL,
    account_item        VARCHAR(100)    NOT NULL,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    opening_balance     NUMERIC(15,2)   NULL DEFAULT 0,
    increase_amount     NUMERIC(15,2)   NULL DEFAULT 0,
    decrease_amount     NUMERIC(15,2)   NULL DEFAULT 0,
    closing_balance     NUMERIC(15,2)   NULL DEFAULT 0,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_tax_adjustment: 税務調整テーブル（FrmFlexTaxAdjustment画面用）
CREATE TABLE IF NOT EXISTS tw_lease_tax_adjustment (
    tax_adj_id          SERIAL          PRIMARY KEY,
    fiscal_year         VARCHAR(4)      NOT NULL,
    contract_id         INTEGER         NOT NULL REFERENCES tw_lease_contract(contract_id),
    adjustment_type     VARCHAR(50)     NOT NULL,
    accounting_amount   NUMERIC(15,2)   NULL DEFAULT 0,
    tax_amount          NUMERIC(15,2)   NULL DEFAULT 0,
    difference_amount   NUMERIC(15,2)   NULL DEFAULT 0,
    temp_or_permanent   VARCHAR(10)     NULL,
    adjustment_reason   VARCHAR(500)    NULL,
    status              VARCHAR(20)     NULL DEFAULT '未処理',
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- インデックス
CREATE INDEX IF NOT EXISTS idx_tw_property_contract ON tw_lease_property(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_party_contract ON tw_lease_party(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_schedule_contract ON tw_lease_schedule(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_initial_contract ON tw_lease_initial(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_sublease_contract ON tw_lease_sublease(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_payment_actual_contract ON tw_lease_payment_actual(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_payment_actual_ym ON tw_lease_payment_actual(payment_ym);
CREATE INDEX IF NOT EXISTS idx_tw_journal_contract ON tw_lease_journal(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_journal_ym ON tw_lease_journal(journal_ym);
CREATE INDEX IF NOT EXISTS idx_tw_period_balance_year ON tw_lease_period_balance(fiscal_year);
CREATE INDEX IF NOT EXISTS idx_tw_balance_breakdown_year ON tw_lease_balance_breakdown(fiscal_year);
CREATE INDEX IF NOT EXISTS idx_tw_balance_breakdown_contract ON tw_lease_balance_breakdown(contract_id);
CREATE INDEX IF NOT EXISTS idx_tw_tax_adjustment_year ON tw_lease_tax_adjustment(fiscal_year);
CREATE INDEX IF NOT EXISTS idx_tw_tax_adjustment_contract ON tw_lease_tax_adjustment(contract_id);
