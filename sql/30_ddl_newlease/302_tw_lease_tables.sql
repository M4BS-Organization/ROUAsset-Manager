-- 新リース会計ワークテーブル定義
-- 契約マスタとして d_kykh(kykh_id) を参照する。
-- tw_lease_contract / tw_lease_property / tw_lease_party は d_kykh 側に統合済みのため本ファイルには含まない。
-- 実行順序: master_tables.sql → tw_tables.sql → ctb_tables.sql

-- tw_lease_schedule: 支払スケジュールテーブル
CREATE TABLE IF NOT EXISTS tw_lease_schedule (
    schedule_id         SERIAL          PRIMARY KEY,
    kykh_id             INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
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
    kykh_id                 INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
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
    renewal_forecast_count  INTEGER         NULL,
    renewal_pay_date        DATE            NULL,
    calculated_at           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_accounting_contract UNIQUE (kykh_id)
);

-- tw_lease_judgment: リース判定テーブル
CREATE TABLE IF NOT EXISTS tw_lease_judgment (
    judgment_id         SERIAL          PRIMARY KEY,
    kykh_id             INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
    q1_result           BOOLEAN         NULL,
    q2_result           BOOLEAN         NULL,
    q3_result           BOOLEAN         NULL,
    q4_result           BOOLEAN         NULL,
    is_exempt_short     BOOLEAN         NULL,
    is_exempt_small     BOOLEAN         NULL,
    final_result        VARCHAR(20)     NULL,
    judged_at           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_judgment_contract UNIQUE (kykh_id)
);

-- tw_lease_initial: 初回費用テーブル
CREATE TABLE IF NOT EXISTS tw_lease_initial (
    initial_id              SERIAL          PRIMARY KEY,
    kykh_id                 INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
    cost_item_cd            VARCHAR(10)     NULL REFERENCES m_initial_cost_item(cost_item_cd),
    acct_treatment_cd       VARCHAR(10)     NULL REFERENCES m_acct_treatment(acct_treatment_cd),
    amount                  NUMERIC(15,2)   NULL DEFAULT 0,
    remarks                 VARCHAR(500)    NULL,
    created_at              TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_sublease: 転貸情報テーブル
CREATE TABLE IF NOT EXISTS tw_lease_sublease (
    sublease_id         SERIAL          PRIMARY KEY,
    kykh_id             INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
    subtenant_name      VARCHAR(200)    NULL,
    subtenant_address   VARCHAR(500)    NULL,
    sublease_income     NUMERIC(15,2)   NULL DEFAULT 0,
    remarks             VARCHAR(500)    NULL,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- tw_lease_payment_actual: 月次支払実績テーブル
CREATE TABLE IF NOT EXISTS tw_lease_payment_actual (
    payment_actual_id   SERIAL          PRIMARY KEY,
    kykh_id             INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
    payment_ym          VARCHAR(7)      NOT NULL,
    payment_type        VARCHAR(20)     NOT NULL,
    scheduled_amount    NUMERIC(15,2)   NULL DEFAULT 0,
    actual_amount       NUMERIC(15,2)   NULL DEFAULT 0,
    difference          NUMERIC(15,2)   NULL DEFAULT 0,
    payment_date        DATE            NULL,
    payment_method_cd   VARCHAR(10)     NULL REFERENCES m_payment_method(payment_method_cd),
    remarks             VARCHAR(500)    NULL,
    created_at          TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at          TIMESTAMP       NULL
);

-- tw_lease_journal: 月次仕訳テーブル
CREATE TABLE IF NOT EXISTS tw_lease_journal (
    journal_id          SERIAL          PRIMARY KEY,
    kykh_id             INTEGER         NOT NULL REFERENCES d_kykh(kykh_id),
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
    generated_at        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- インデックス
CREATE INDEX IF NOT EXISTS idx_tw_schedule_kykh ON tw_lease_schedule(kykh_id);
CREATE INDEX IF NOT EXISTS idx_tw_initial_kykh ON tw_lease_initial(kykh_id);
CREATE INDEX IF NOT EXISTS idx_tw_sublease_kykh ON tw_lease_sublease(kykh_id);
CREATE INDEX IF NOT EXISTS idx_tw_payment_actual_kykh ON tw_lease_payment_actual(kykh_id);
CREATE INDEX IF NOT EXISTS idx_tw_payment_actual_ym ON tw_lease_payment_actual(payment_ym);
CREATE INDEX IF NOT EXISTS idx_tw_journal_kykh ON tw_lease_journal(kykh_id);
CREATE INDEX IF NOT EXISTS idx_tw_journal_ym ON tw_lease_journal(journal_ym);
