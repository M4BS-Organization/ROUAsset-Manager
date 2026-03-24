-- ============================================================
-- CTB物件テーブル（EAVパターン）DDL
-- 実行順: 1. m_property_attribute_def → 2. ctb_property → 3. ctb_property_attribute → 4. VIEW
-- psql -U lease_m4bs_user -d lease_m4bs -f 010_ctb_property_eav.sql
-- ============================================================

-- ==============================================
-- 1. m_property_attribute_def: 属性定義マスタ
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

-- ==============================================
-- 2. ctb_property: 物件マスタ
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

-- ==============================================
-- 3. ctb_property_attribute: EAV汎用テーブル
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

-- ==============================================
-- 4. v_ctb_property_full: 画面用統合ビュー
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
