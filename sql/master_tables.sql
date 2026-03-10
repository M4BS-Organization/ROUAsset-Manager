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


CREATE TABLE m_department (
    dept_cd              VARCHAR(10)  NOT NULL,
    dept_nm              VARCHAR(100) NOT NULL,
    dept_cd2             VARCHAR(10)  NULL,
    dept_nm2             VARCHAR(100) NULL,
    dept_cd3             VARCHAR(10)  NULL,
    dept_nm3             VARCHAR(100) NULL,
    dept_cd4             VARCHAR(10)  NULL,
    dept_nm4             VARCHAR(100) NULL,
    dept_cd5             VARCHAR(10)  NULL,
    dept_nm5             VARCHAR(100) NULL,
    cost_category_nm     VARCHAR(100) NULL,
    agg_category1_cd     VARCHAR(10)  NULL,
    agg_category1_nm     VARCHAR(100) NULL,
    agg_category2_cd     VARCHAR(10)  NULL,
    agg_category2_nm     VARCHAR(100) NULL,
    agg_category3_cd     VARCHAR(10)  NULL,
    agg_category3_nm     VARCHAR(100) NULL,
    remarks              VARCHAR(500) NULL,
    create_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                   SERIAL,
    CONSTRAINT pk_m_department PRIMARY KEY (dept_cd)
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
