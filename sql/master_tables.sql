-- ============================================================
-- リース契約管理システム マスタテーブル DDL
-- 対象DB: PostgreSQL
-- 作成日: 2026/03/04
-- ============================================================


-- ------------------------------------------------------------
-- 1. 会社コードマスタ
-- ------------------------------------------------------------
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

COMMENT ON TABLE  m_company             IS '会社コードマスタ';
COMMENT ON COLUMN m_company.company_cd  IS '会社コード';
COMMENT ON COLUMN m_company.company_nm  IS '会社名';
COMMENT ON COLUMN m_company.company_cd2 IS '会社コード2';
COMMENT ON COLUMN m_company.company_nm2 IS '会社名2';
COMMENT ON COLUMN m_company.company_cd3 IS '会社コード3';
COMMENT ON COLUMN m_company.company_nm3 IS '会社名3';
COMMENT ON COLUMN m_company.remarks     IS '備考';
COMMENT ON COLUMN m_company.create_dt   IS '作成日時';
COMMENT ON COLUMN m_company.update_dt   IS '更新日時';
COMMENT ON COLUMN m_company.id          IS 'ID';


-- ------------------------------------------------------------
-- 2. 契約管理単位マスタ
-- ------------------------------------------------------------
CREATE TABLE m_contract_mgmt_unit (
    contract_mgmt_unit_cd  VARCHAR(10)  NOT NULL,
    contract_mgmt_unit_nm  VARCHAR(100) NOT NULL,
    contract_mgmt_unit_cd2 VARCHAR(10)  NULL,
    contract_mgmt_unit_nm2 VARCHAR(100) NULL,
    contract_mgmt_unit_cd3 VARCHAR(10)  NULL,
    contract_mgmt_unit_nm3 VARCHAR(100) NULL,
    company_nm             VARCHAR(100) NULL,
    cost_item_cd           VARCHAR(20)  NULL,
    cost_item_nm           VARCHAR(100) NULL,
    remarks                VARCHAR(500) NULL,
    create_dt              TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt              TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                     SERIAL,
    CONSTRAINT pk_m_contract_mgmt_unit PRIMARY KEY (contract_mgmt_unit_cd)
);

COMMENT ON TABLE  m_contract_mgmt_unit                        IS '契約管理単位マスタ';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_cd  IS '契約管理単位コード';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_nm  IS '契約管理単位名';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_cd2 IS '契約管理単位コード2';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_nm2 IS '契約管理単位名2';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_cd3 IS '契約管理単位コード3';
COMMENT ON COLUMN m_contract_mgmt_unit.contract_mgmt_unit_nm3 IS '契約管理単位名3';
COMMENT ON COLUMN m_contract_mgmt_unit.company_nm             IS '会社名';
COMMENT ON COLUMN m_contract_mgmt_unit.cost_item_cd           IS '費目決定要素コード';
COMMENT ON COLUMN m_contract_mgmt_unit.cost_item_nm           IS '費目決定要素';
COMMENT ON COLUMN m_contract_mgmt_unit.remarks                IS '備考';
COMMENT ON COLUMN m_contract_mgmt_unit.create_dt              IS '作成日時';
COMMENT ON COLUMN m_contract_mgmt_unit.update_dt              IS '更新日時';
COMMENT ON COLUMN m_contract_mgmt_unit.id                     IS 'ID';


-- ------------------------------------------------------------
-- 3. 支払先マスタ
-- ------------------------------------------------------------
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

COMMENT ON TABLE  m_supplier                           IS '支払先マスタ';
COMMENT ON COLUMN m_supplier.supplier_cd               IS '支払先コード';
COMMENT ON COLUMN m_supplier.supplier_nm               IS '支払先名';
COMMENT ON COLUMN m_supplier.supplier_cd2              IS '支払先コード2';
COMMENT ON COLUMN m_supplier.supplier_nm2              IS '支払先名2';
COMMENT ON COLUMN m_supplier.row1_contract_closing_day IS '1行目_契約締結_日締め';
COMMENT ON COLUMN m_supplier.row1_first_pay_months     IS '1行目_初回支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row1_first_pay_day        IS '1行目_初回支払_日';
COMMENT ON COLUMN m_supplier.row1_second_pay_months    IS '1行目_2回目支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row1_second_pay_day       IS '1行目_2回目支払_日';
COMMENT ON COLUMN m_supplier.row2_contract_closing_day IS '2行目_契約締結_日締め';
COMMENT ON COLUMN m_supplier.row2_first_pay_months     IS '2行目_初回支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row2_first_pay_day        IS '2行目_初回支払_日';
COMMENT ON COLUMN m_supplier.row2_second_pay_months    IS '2行目_2回目支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row2_second_pay_day       IS '2行目_2回目支払_日';
COMMENT ON COLUMN m_supplier.row3_contract_closing_day IS '3行目_契約締結_日締め';
COMMENT ON COLUMN m_supplier.row3_first_pay_months     IS '3行目_初回支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row3_first_pay_day        IS '3行目_初回支払_日';
COMMENT ON COLUMN m_supplier.row3_second_pay_months    IS '3行目_2回目支払_ヶ月後';
COMMENT ON COLUMN m_supplier.row3_second_pay_day       IS '3行目_2回目支払_日';
COMMENT ON COLUMN m_supplier.re_lease_param            IS '再リースパラメータ';
COMMENT ON COLUMN m_supplier.remarks                   IS '備考';
COMMENT ON COLUMN m_supplier.create_dt                 IS '作成日時';
COMMENT ON COLUMN m_supplier.update_dt                 IS '更新日時';
COMMENT ON COLUMN m_supplier.id                        IS 'ID';


-- ------------------------------------------------------------
-- 4. 支払方法マスタ
-- ------------------------------------------------------------
CREATE TABLE m_payment_method (
    payment_method_cd  VARCHAR(10)  NOT NULL,
    payment_method_nm  VARCHAR(100) NOT NULL,
    remarks            VARCHAR(500) NULL,
    create_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt          TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                 SERIAL,
    CONSTRAINT pk_m_payment_method PRIMARY KEY (payment_method_cd)
);

COMMENT ON TABLE  m_payment_method                    IS '支払方法マスタ';
COMMENT ON COLUMN m_payment_method.payment_method_cd  IS '支払方法コード';
COMMENT ON COLUMN m_payment_method.payment_method_nm  IS '支払方法名';
COMMENT ON COLUMN m_payment_method.remarks            IS '備考';
COMMENT ON COLUMN m_payment_method.create_dt          IS '作成日時';
COMMENT ON COLUMN m_payment_method.update_dt          IS '更新日時';
COMMENT ON COLUMN m_payment_method.id                 IS 'ID';


-- ------------------------------------------------------------
-- 5. 原価区分マスタ
-- ------------------------------------------------------------
CREATE TABLE m_cost_category (
    cost_category_cd  VARCHAR(10)  NOT NULL,
    cost_category_nm  VARCHAR(100) NOT NULL,
    remarks           VARCHAR(500) NULL,
    create_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                SERIAL,
    CONSTRAINT pk_m_cost_category PRIMARY KEY (cost_category_cd)
);

COMMENT ON TABLE  m_cost_category                   IS '原価区分マスタ';
COMMENT ON COLUMN m_cost_category.cost_category_cd  IS '原価区分コード';
COMMENT ON COLUMN m_cost_category.cost_category_nm  IS '原価区分名';
COMMENT ON COLUMN m_cost_category.remarks           IS '備考';
COMMENT ON COLUMN m_cost_category.create_dt         IS '作成日時';
COMMENT ON COLUMN m_cost_category.update_dt         IS '更新日時';
COMMENT ON COLUMN m_cost_category.id                IS 'ID';


-- ------------------------------------------------------------
-- 6. 部署マスタ
-- ------------------------------------------------------------
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
    property_mgmt_unit   VARCHAR(100) NULL,
    remarks              VARCHAR(500) NULL,
    create_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                   SERIAL,
    CONSTRAINT pk_m_department PRIMARY KEY (dept_cd)
);

COMMENT ON TABLE  m_department                    IS '部署マスタ';
COMMENT ON COLUMN m_department.dept_cd            IS '部署コード';
COMMENT ON COLUMN m_department.dept_nm            IS '部署名';
COMMENT ON COLUMN m_department.dept_cd2           IS '部署コード2';
COMMENT ON COLUMN m_department.dept_nm2           IS '部署名2';
COMMENT ON COLUMN m_department.dept_cd3           IS '部署コード3';
COMMENT ON COLUMN m_department.dept_nm3           IS '部署名3';
COMMENT ON COLUMN m_department.dept_cd4           IS '部署コード4';
COMMENT ON COLUMN m_department.dept_nm4           IS '部署名4';
COMMENT ON COLUMN m_department.dept_cd5           IS '部署コード5';
COMMENT ON COLUMN m_department.dept_nm5           IS '部署名5';
COMMENT ON COLUMN m_department.cost_category_nm   IS '原価区分';
COMMENT ON COLUMN m_department.agg_category1_cd   IS '集計区分1コード';
COMMENT ON COLUMN m_department.agg_category1_nm   IS '集計区分1';
COMMENT ON COLUMN m_department.agg_category2_cd   IS '集計区分2コード';
COMMENT ON COLUMN m_department.agg_category2_nm   IS '集計区分2';
COMMENT ON COLUMN m_department.agg_category3_cd   IS '集計区分3コード';
COMMENT ON COLUMN m_department.agg_category3_nm   IS '集計区分3';
COMMENT ON COLUMN m_department.property_mgmt_unit IS '物件管理単位';
COMMENT ON COLUMN m_department.remarks            IS '備考';
COMMENT ON COLUMN m_department.create_dt          IS '作成日時';
COMMENT ON COLUMN m_department.update_dt          IS '更新日時';
COMMENT ON COLUMN m_department.id                 IS 'ID';


-- ------------------------------------------------------------
-- 7. 物件管理単位マスタ
-- ------------------------------------------------------------
CREATE TABLE m_property_mgmt_unit (
    property_mgmt_unit_cd  VARCHAR(10)  NOT NULL,
    property_mgmt_unit_nm  VARCHAR(100) NOT NULL,
    property_mgmt_unit_cd2 VARCHAR(10)  NULL,
    property_mgmt_unit_nm2 VARCHAR(100) NULL,
    property_mgmt_unit_cd3 VARCHAR(10)  NULL,
    property_mgmt_unit_nm3 VARCHAR(100) NULL,
    remarks                VARCHAR(500) NULL,
    create_dt              TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt              TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                     SERIAL,
    CONSTRAINT pk_m_property_mgmt_unit PRIMARY KEY (property_mgmt_unit_cd)
);

COMMENT ON TABLE  m_property_mgmt_unit                         IS '物件管理単位マスタ';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_cd  IS '物件管理単位コード';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_nm  IS '物件管理単位名';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_cd2 IS '物件管理単位コード2';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_nm2 IS '物件管理単位名2';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_cd3 IS '物件管理単位コード3';
COMMENT ON COLUMN m_property_mgmt_unit.property_mgmt_unit_nm3 IS '物件管理単位名3';
COMMENT ON COLUMN m_property_mgmt_unit.remarks                IS '備考';
COMMENT ON COLUMN m_property_mgmt_unit.create_dt              IS '作成日時';
COMMENT ON COLUMN m_property_mgmt_unit.update_dt              IS '更新日時';
COMMENT ON COLUMN m_property_mgmt_unit.id                     IS 'ID';


-- ------------------------------------------------------------
-- 8. 費用区分マスタ
-- ------------------------------------------------------------
CREATE TABLE m_expense_category (
    expense_category_cd  VARCHAR(10)  NOT NULL,
    expense_category_nm  VARCHAR(100) NOT NULL,
    agg_category1_cd     VARCHAR(10)  NULL,
    agg_category1_nm     VARCHAR(100) NULL,
    agg_category2_cd     VARCHAR(10)  NULL,
    agg_category2_nm     VARCHAR(100) NULL,
    agg_category3_cd     VARCHAR(10)  NULL,
    agg_category3_nm     VARCHAR(100) NULL,
    cost_item_cd         VARCHAR(20)  NULL,
    cost_item_nm         VARCHAR(100) NULL,
    remarks              VARCHAR(500) NULL,
    create_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt            TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                   SERIAL,
    CONSTRAINT pk_m_expense_category PRIMARY KEY (expense_category_cd)
);

COMMENT ON TABLE  m_expense_category                      IS '費用区分マスタ';
COMMENT ON COLUMN m_expense_category.expense_category_cd  IS '費用区分コード';
COMMENT ON COLUMN m_expense_category.expense_category_nm  IS '費用区分名';
COMMENT ON COLUMN m_expense_category.agg_category1_cd     IS '集計区分1コード';
COMMENT ON COLUMN m_expense_category.agg_category1_nm     IS '集計区分1';
COMMENT ON COLUMN m_expense_category.agg_category2_cd     IS '集計区分2コード';
COMMENT ON COLUMN m_expense_category.agg_category2_nm     IS '集計区分2';
COMMENT ON COLUMN m_expense_category.agg_category3_cd     IS '集計区分3コード';
COMMENT ON COLUMN m_expense_category.agg_category3_nm     IS '集計区分3';
COMMENT ON COLUMN m_expense_category.cost_item_cd         IS '費目決定要素コード';
COMMENT ON COLUMN m_expense_category.cost_item_nm         IS '費目決定要素';
COMMENT ON COLUMN m_expense_category.remarks              IS '備考';
COMMENT ON COLUMN m_expense_category.create_dt            IS '作成日時';
COMMENT ON COLUMN m_expense_category.update_dt            IS '更新日時';
COMMENT ON COLUMN m_expense_category.id                   IS 'ID';


-- ------------------------------------------------------------
-- 9. 資産区分マスタ
-- ------------------------------------------------------------
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

COMMENT ON TABLE  m_asset_category                          IS '資産区分マスタ';
COMMENT ON COLUMN m_asset_category.asset_category_cd        IS '資産区分コード';
COMMENT ON COLUMN m_asset_category.asset_category_nm        IS '資産区分名';
COMMENT ON COLUMN m_asset_category.note_asset_account_cd    IS '注記資産科目CD';
COMMENT ON COLUMN m_asset_category.asset_account_cd         IS '資産科目CD';
COMMENT ON COLUMN m_asset_category.accum_account_cd         IS '累計科目CD';
COMMENT ON COLUMN m_asset_category.impair_accum_account_cd  IS '減損累計科目CD';
COMMENT ON COLUMN m_asset_category.liability_account_cd     IS '負債科目CD';
COMMENT ON COLUMN m_asset_category.unpaid_tax_account_cd    IS '未払消費税科目CD';
COMMENT ON COLUMN m_asset_category.impair_account_cd        IS '減損勘定科目CD';
COMMENT ON COLUMN m_asset_category.liability_account_1y_cd  IS '負債科目CD（1年以内）';
COMMENT ON COLUMN m_asset_category.unpaid_tax_account_1y_cd IS '未払消費税科目CD（1年以内）';
COMMENT ON COLUMN m_asset_category.impair_account_1y_cd     IS '減損勘定科目CD（1年以内）';
COMMENT ON COLUMN m_asset_category.cost_item_cd             IS '費目決定要素CD';
COMMENT ON COLUMN m_asset_category.remarks                  IS '備考';
COMMENT ON COLUMN m_asset_category.create_dt                IS '作成日時';
COMMENT ON COLUMN m_asset_category.update_dt                IS '更新日時';
COMMENT ON COLUMN m_asset_category.id                       IS 'ID';


-- ------------------------------------------------------------
-- 10. 物件種別マスタ
-- ------------------------------------------------------------
CREATE TABLE m_property_type (
    property_type_cd  VARCHAR(10)  NOT NULL,
    property_type_nm  VARCHAR(100) NOT NULL,
    remarks           VARCHAR(500) NULL,
    create_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt         TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                SERIAL,
    CONSTRAINT pk_m_property_type PRIMARY KEY (property_type_cd)
);

COMMENT ON TABLE  m_property_type                  IS '物件種別マスタ';
COMMENT ON COLUMN m_property_type.property_type_cd IS '物件種別コード';
COMMENT ON COLUMN m_property_type.property_type_nm IS '物件種別名';
COMMENT ON COLUMN m_property_type.remarks          IS '備考';
COMMENT ON COLUMN m_property_type.create_dt        IS '作成日時';
COMMENT ON COLUMN m_property_type.update_dt        IS '更新日時';
COMMENT ON COLUMN m_property_type.id               IS 'ID';


-- ------------------------------------------------------------
-- 11. 銀行口座マスタ
-- ------------------------------------------------------------
CREATE TABLE m_bank_account (
    bank_account_cd  VARCHAR(10)  NOT NULL,
    bank_account_nm  VARCHAR(100) NOT NULL,
    remarks          VARCHAR(500) NULL,
    create_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id               SERIAL,
    CONSTRAINT pk_m_bank_account PRIMARY KEY (bank_account_cd)
);

COMMENT ON TABLE  m_bank_account                  IS '銀行口座マスタ';
COMMENT ON COLUMN m_bank_account.bank_account_cd  IS '銀行口座コード';
COMMENT ON COLUMN m_bank_account.bank_account_nm  IS '銀行口座名';
COMMENT ON COLUMN m_bank_account.remarks          IS '備考';
COMMENT ON COLUMN m_bank_account.create_dt        IS '作成日時';
COMMENT ON COLUMN m_bank_account.update_dt        IS '更新日時';
COMMENT ON COLUMN m_bank_account.id               IS 'ID';


-- ------------------------------------------------------------
-- 12. 業者マスタ
-- ------------------------------------------------------------
CREATE TABLE m_vendor (
    vendor_cd  VARCHAR(10)  NOT NULL,
    vendor_nm  VARCHAR(100) NOT NULL,
    remarks    VARCHAR(500) NULL,
    create_dt  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt  TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id         SERIAL,
    CONSTRAINT pk_m_vendor PRIMARY KEY (vendor_cd)
);

COMMENT ON TABLE  m_vendor           IS '業者マスタ';
COMMENT ON COLUMN m_vendor.vendor_cd IS '業者コード';
COMMENT ON COLUMN m_vendor.vendor_nm IS '業者名';
COMMENT ON COLUMN m_vendor.remarks   IS '備考';
COMMENT ON COLUMN m_vendor.create_dt IS '作成日時';
COMMENT ON COLUMN m_vendor.update_dt IS '更新日時';
COMMENT ON COLUMN m_vendor.id        IS 'ID';


-- ------------------------------------------------------------
-- 13. メーカーマスタ
-- ------------------------------------------------------------
CREATE TABLE m_manufacturer (
    manufacturer_cd  VARCHAR(10)  NOT NULL,
    manufacturer_nm  VARCHAR(100) NOT NULL,
    remarks          VARCHAR(500) NULL,
    create_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id               SERIAL,
    CONSTRAINT pk_m_manufacturer PRIMARY KEY (manufacturer_cd)
);

COMMENT ON TABLE  m_manufacturer                 IS 'メーカーマスタ';
COMMENT ON COLUMN m_manufacturer.manufacturer_cd IS 'メーカーコード';
COMMENT ON COLUMN m_manufacturer.manufacturer_nm IS 'メーカー名';
COMMENT ON COLUMN m_manufacturer.remarks         IS '備考';
COMMENT ON COLUMN m_manufacturer.create_dt       IS '作成日時';
COMMENT ON COLUMN m_manufacturer.update_dt       IS '更新日時';
COMMENT ON COLUMN m_manufacturer.id              IS 'ID';


-- ------------------------------------------------------------
-- 14. 廃棄方法マスタ
-- ------------------------------------------------------------
CREATE TABLE m_disposal_method (
    disposal_method_cd  VARCHAR(10)  NOT NULL,
    disposal_method_nm  VARCHAR(100) NOT NULL,
    remarks             VARCHAR(500) NULL,
    create_dt           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id                  SERIAL,
    CONSTRAINT pk_m_disposal_method PRIMARY KEY (disposal_method_cd)
);

COMMENT ON TABLE  m_disposal_method                     IS '廃棄方法マスタ';
COMMENT ON COLUMN m_disposal_method.disposal_method_cd  IS '廃棄方法コード';
COMMENT ON COLUMN m_disposal_method.disposal_method_nm  IS '廃棄方法';
COMMENT ON COLUMN m_disposal_method.remarks             IS '備考';
COMMENT ON COLUMN m_disposal_method.create_dt           IS '作成日時';
COMMENT ON COLUMN m_disposal_method.update_dt           IS '更新日時';
COMMENT ON COLUMN m_disposal_method.id                  IS 'ID';


-- ============================================================
-- END OF DDL
-- ============================================================
