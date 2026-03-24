-- psql -U lease_m4bs_user -d lease_m4bs -f 303_ctb_tables.sql

CREATE TABLE IF NOT EXISTS ctb_lease_integrated (
    ctb_id              SERIAL          NOT NULL,
    contract_no         VARCHAR(15)     NOT NULL,
    property_no         INTEGER         NOT NULL DEFAULT 1,

    contract_name       VARCHAR(200)    NULL,
    contract_type_cd    VARCHAR(10)     NULL,
    supplier_cd         VARCHAR(10)     NULL,
    mgmt_dept_cd        VARCHAR(10)     NULL,
    lease_start_date    DATE            NULL,
    lease_end_date      DATE            NULL,
    free_rent_months    INTEGER         NULL DEFAULT 0,
    lease_term_months   INTEGER         NULL,

    asset_no            VARCHAR(20)     NULL,
    asset_category_cd   VARCHAR(10)     NULL,
    asset_name          VARCHAR(200)    NULL,
    company_name        VARCHAR(100)    NULL,
    install_location    VARCHAR(200)    NULL,
    remarks             VARCHAR(500)    NULL,

    monthly_payment     NUMERIC(15,2)   NULL DEFAULT 0,
    lease_depreciation  NUMERIC(15,2)   NULL DEFAULT 0,
    total_payment       NUMERIC(15,2)   NULL DEFAULT 0,
    split_status        VARCHAR(10)     NOT NULL DEFAULT 'unsplit',

    re_structure        VARCHAR(100)    NULL,
    re_area             VARCHAR(50)     NULL,
    re_layout           VARCHAR(50)     NULL,
    re_completion_date  DATE            NULL,
    re_landlord_name    VARCHAR(100)    NULL,
    re_broker_company   VARCHAR(100)    NULL,
    re_usage_restrictions VARCHAR(200)  NULL,

    vh_chassis_no       VARCHAR(50)     NULL,
    vh_registration_no  VARCHAR(50)     NULL,
    vh_vehicle_type     VARCHAR(100)    NULL,
    vh_inspection_date  DATE            NULL,
    vh_mileage_limit    VARCHAR(50)     NULL,

    oa_model_no         VARCHAR(50)     NULL,
    oa_serial_no        VARCHAR(50)     NULL,
    oa_maintenance_date DATE            NULL,
    oa_maintenance_contract VARCHAR(200) NULL,

    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_lease_integrated PRIMARY KEY (ctb_id),
    CONSTRAINT uq_ctb_contract_property UNIQUE (contract_no, property_no),
    CONSTRAINT fk_ctb_contract_type FOREIGN KEY (contract_type_cd)
        REFERENCES m_contract_type (contract_type_cd),
    CONSTRAINT fk_ctb_supplier FOREIGN KEY (supplier_cd)
        REFERENCES m_supplier (supplier_cd),
    CONSTRAINT fk_ctb_mgmt_dept FOREIGN KEY (mgmt_dept_cd)
        REFERENCES m_department (dept_cd)
);

CREATE INDEX IF NOT EXISTS idx_ctb_contract_no
    ON ctb_lease_integrated (contract_no);

CREATE INDEX IF NOT EXISTS idx_ctb_asset_no
    ON ctb_lease_integrated (asset_no);


CREATE TABLE IF NOT EXISTS ctb_dept_allocation (
    allocation_id    SERIAL          NOT NULL,
    ctb_id           INTEGER         NOT NULL,
    dept_cd          VARCHAR(10)     NOT NULL,
    allocation_ratio NUMERIC(5,2)    NOT NULL DEFAULT 100.00,
    payment_amount   NUMERIC(15,2)   NULL DEFAULT 0,
    remarks          VARCHAR(500)    NULL,
    create_dt        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_ctb_dept_allocation PRIMARY KEY (allocation_id),
    CONSTRAINT fk_dept_allocation_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE,
    CONSTRAINT fk_dept_allocation_dept FOREIGN KEY (dept_cd)
        REFERENCES m_department (dept_cd)
);

CREATE UNIQUE INDEX IF NOT EXISTS uq_dept_allocation_ctb_dept
    ON ctb_dept_allocation (ctb_id, dept_cd);

CREATE INDEX IF NOT EXISTS idx_dept_allocation_ctb_id
    ON ctb_dept_allocation (ctb_id);
