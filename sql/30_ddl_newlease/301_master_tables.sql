CREATE TABLE m_company (
    company_cd   VARCHAR(10)  NOT NULL,
    company_nm   VARCHAR(100) NOT NULL,
    company_cd2  VARCHAR(10)  NULL,
    company_nm2  VARCHAR(100) NULL,
    company_cd3  VARCHAR(10)  NULL,
    company_nm3  VARCHAR(100) NULL,
    remarks      VARCHAR(500) NULL,
    create_dt    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt    TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id           SERIAL,
    CONSTRAINT pk_m_company PRIMARY KEY (company_cd)
);


CREATE TABLE m_supplier (
    supplier_cd                VARCHAR(10)  NOT NULL,
    supplier_nm                VARCHAR(100) NOT NULL,
    supplier_cd2               VARCHAR(10)  NULL,
    supplier_nm2               VARCHAR(100) NULL,
    row1_contract_closing_day  SMALLINT     NULL,
    row1_first_pay_months      SMALLINT     NULL,
    row1_first_pay_day         SMALLINT     NULL,
    row1_second_pay_months     SMALLINT     NULL,
    row1_second_pay_day        SMALLINT     NULL,
    row2_contract_closing_day  SMALLINT     NULL,
    row2_first_pay_months      SMALLINT     NULL,
    row2_first_pay_day         SMALLINT     NULL,
    row2_second_pay_months     SMALLINT     NULL,
    row2_second_pay_day        SMALLINT     NULL,
    row3_contract_closing_day  SMALLINT     NULL,
    row3_first_pay_months      SMALLINT     NULL,
    row3_first_pay_day         SMALLINT     NULL,
    row3_second_pay_months     SMALLINT     NULL,
    row3_second_pay_day        SMALLINT     NULL,
    re_lease_param             SMALLINT     NULL,
    remarks                    VARCHAR(500) NULL,
    create_dt                  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                         SERIAL,
    CONSTRAINT pk_m_supplier PRIMARY KEY (supplier_cd)
);


CREATE TABLE m_payment_method (
    payment_method_cd  VARCHAR(10)  NOT NULL,
    payment_method_nm  VARCHAR(100) NOT NULL,
    remarks            VARCHAR(500) NULL,
    create_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                 SERIAL,
    CONSTRAINT pk_m_payment_method PRIMARY KEY (payment_method_cd)
);


-- m_department: 部門マスタ（m_bcatとカラム名・型を統一）
CREATE TABLE m_department (
    -- === PK ===
    dept_id              SERIAL       NOT NULL,           -- サロゲートPK
    dept_cd              VARCHAR(12)  NOT NULL,           -- 部門コード         ← m_bcat.bcat1_cd

    -- === 部門階層（m_bcat統一） ===
    dept_nm              VARCHAR(80)  NOT NULL,           -- 部門名             ← m_bcat.bcat1_nm
    dept_cd2             VARCHAR(12)  NULL,               -- 部門コード2        ← m_bcat.bcat2_cd
    dept_nm2             VARCHAR(40)  NULL,               -- 部門名2            ← m_bcat.bcat2_nm
    dept_cd3             VARCHAR(12)  NULL,               -- 部門コード3        ← m_bcat.bcat3_cd
    dept_nm3             VARCHAR(40)  NULL,               -- 部門名3            ← m_bcat.bcat3_nm
    dept_cd4             VARCHAR(12)  NULL,               -- 部門コード4        ← m_bcat.bcat4_cd
    dept_nm4             VARCHAR(40)  NULL,               -- 部門名4            ← m_bcat.bcat4_nm
    dept_cd5             VARCHAR(12)  NULL,               -- 部門コード5        ← m_bcat.bcat5_cd
    dept_nm5             VARCHAR(40)  NULL,               -- 部門名5            ← m_bcat.bcat5_nm

    -- === 原価・集計区分（m_bcat統一） ===
    genk_id              INTEGER      NULL,               -- 原価区分ID         ← m_bcat.genk_id → m_genk
    skti_id              INTEGER      NULL,               -- 資産計上ID         ← m_bcat.skti_id
    sum1_cd              VARCHAR(12)  NULL,               -- 集計区分1コード    ← m_bcat.sum1_cd
    sum1_nm              VARCHAR(40)  NULL,               -- 集計区分1名        ← m_bcat.sum1_nm
    sum2_cd              VARCHAR(12)  NULL,               -- 集計区分2コード    ← m_bcat.sum2_cd
    sum2_nm              VARCHAR(40)  NULL,               -- 集計区分2名        ← m_bcat.sum2_nm
    sum3_cd              VARCHAR(12)  NULL,               -- 集計区分3コード    ← m_bcat.sum3_cd
    sum3_nm              VARCHAR(40)  NULL,               -- 集計区分3名        ← m_bcat.sum3_nm

    -- === フラグ（m_bcat統一） ===
    bknri_id             INTEGER      NULL,               -- 物件管理ID         ← m_bcat.bknri_id
    kbf_kb               BOOLEAN      NOT NULL DEFAULT FALSE, -- 科目配賦フラグ ← m_bcat.kbf_kb
    kbf_fb               BOOLEAN      NOT NULL DEFAULT FALSE, -- FB配賦フラグ   ← m_bcat.kbf_fb
    kbf_sb               BOOLEAN      NOT NULL DEFAULT FALSE, -- SB配賦フラグ   ← m_bcat.kbf_sb
    gensonf              BOOLEAN      NOT NULL DEFAULT FALSE, -- 減損フラグ     ← m_bcat.gensonf

    -- === 備考・システム（m_bcat統一） ===
    biko                 VARCHAR(100) NULL,               -- 備考               ← m_bcat.biko
    create_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_cnt           INTEGER      NOT NULL DEFAULT 0, --                    ← m_bcat.update_cnt
    history_f            BOOLEAN      NOT NULL DEFAULT FALSE, -- 履歴フラグ     ← m_bcat.history_f

    CONSTRAINT pk_m_department PRIMARY KEY (dept_cd),
    CONSTRAINT uq_m_department_id UNIQUE (dept_id)
);


CREATE TABLE m_asset_category (
    asset_category_cd         VARCHAR(10)  NOT NULL,
    asset_category_nm         VARCHAR(100) NOT NULL,
    note_asset_account_cd     VARCHAR(10)  NULL,
    asset_account_cd          VARCHAR(10)  NULL,
    accum_account_cd          VARCHAR(10)  NULL,
    impair_accum_account_cd   VARCHAR(10)  NULL,
    liability_account_cd      VARCHAR(10)  NULL,
    unpaid_tax_account_cd     VARCHAR(10)  NULL,
    impair_account_cd         VARCHAR(10)  NULL,
    liability_account_1y_cd   VARCHAR(10)  NULL,
    unpaid_tax_account_1y_cd  VARCHAR(10)  NULL,
    impair_account_1y_cd      VARCHAR(10)  NULL,
    cost_item_cd              VARCHAR(10)  NULL,
    remarks                   VARCHAR(500) NULL,
    create_dt                 TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                 TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                        SERIAL,
    CONSTRAINT pk_m_asset_category PRIMARY KEY (asset_category_cd)
);


CREATE TABLE m_bank_account (
    bank_account_cd  VARCHAR(10)  NOT NULL,
    bank_account_nm  VARCHAR(100) NOT NULL,
    remarks          VARCHAR(500) NULL,
    create_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id               SERIAL,
    CONSTRAINT pk_m_bank_account PRIMARY KEY (bank_account_cd)
);


CREATE TABLE m_contract_type (
    contract_type_cd  VARCHAR(10)  NOT NULL,
    contract_type_nm  VARCHAR(100) NOT NULL,
    sort_order        SMALLINT     NOT NULL DEFAULT 0,
    remarks           VARCHAR(500) NULL,
    create_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                SERIAL,
    CONSTRAINT pk_m_contract_type PRIMARY KEY (contract_type_cd)
);


CREATE TABLE m_initial_cost_item (
    cost_item_cd  VARCHAR(10)  NOT NULL,
    cost_item_nm  VARCHAR(100) NOT NULL,
    sort_order    SMALLINT     NOT NULL DEFAULT 0,
    remarks       VARCHAR(500) NULL,
    create_dt     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id            SERIAL,
    CONSTRAINT pk_m_initial_cost_item PRIMARY KEY (cost_item_cd)
);


CREATE TABLE m_acct_treatment (
    acct_treatment_cd  VARCHAR(10)  NOT NULL,
    acct_treatment_nm  VARCHAR(100) NOT NULL,
    sort_order         SMALLINT     NOT NULL DEFAULT 0,
    remarks            VARCHAR(500) NULL,
    create_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                 SERIAL,
    CONSTRAINT pk_m_acct_treatment PRIMARY KEY (acct_treatment_cd)
);


CREATE TABLE m_monthly_item (
    monthly_item_cd  VARCHAR(10)  NOT NULL,
    monthly_item_nm  VARCHAR(100) NOT NULL,
    sort_order       SMALLINT     NOT NULL DEFAULT 0,
    remarks          VARCHAR(500) NULL,
    create_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id               SERIAL,
    CONSTRAINT pk_m_monthly_item PRIMARY KEY (monthly_item_cd)
);
