-- ============================================================
-- CTB物件 EAV属性テーブル DDL
--
-- ctb_property は ctb_contract_property（303_ctb_tables.sql）に統合済みのため削除。
-- ctb_property_attribute の FK 参照先を ctb_contract_property に変更。
-- v_ctb_property_full は旧カラム参照のため削除。
--
-- 残すテーブル:
--   1. m_property_attribute_def  - 属性定義マスタ（EAV）
--   2. ctb_property_attribute    - 属性値テーブル（FK → ctb_contract_property）
--
-- 依存: 303_ctb_tables.sql (ctb_contract_property)
--       301_master_tables.sql (m_asset_category)
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
-- 2. ctb_property_attribute: EAV汎用テーブル
-- 種別固有情報を key-value で格納
-- FK 参照先: ctb_contract_property.property_id（303_ctb_tables.sqlで定義）
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
        REFERENCES ctb_contract_property (property_id) ON DELETE CASCADE,
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
-- 旧テーブル・ビューの削除（ctb_contract_propertyに統合済み）
-- ==============================================
-- DROP TABLE IF EXISTS ctb_property CASCADE;
-- DROP VIEW IF EXISTS v_ctb_property_full;
