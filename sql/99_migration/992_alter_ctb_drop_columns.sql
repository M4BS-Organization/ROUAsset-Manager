-- ============================================================
-- ctb_lease_integrated: 種別固有カラム削除 + asset_category → asset_category_cd 改名
-- ============================================================

-- Step 1: 種別固有カラム削除（16列）
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

-- Step 2: asset_category を asset_category_cd にリネーム + FK付与
-- まずデータをコード値に変換
UPDATE ctb_lease_integrated
SET asset_category = CASE asset_category
    WHEN '不動産' THEN 'AC01'
    WHEN '構築物' THEN 'AC02'
    WHEN '機械装置' THEN 'AC03'
    WHEN '車両'   THEN 'AC04'
    WHEN 'OA機器' THEN 'AC05'
    ELSE COALESCE(asset_category, 'AC05')
END;

-- リネーム
ALTER TABLE ctb_lease_integrated
    RENAME COLUMN asset_category TO asset_category_cd;

-- 型調整（VARCHAR(20) → VARCHAR(10)）
ALTER TABLE ctb_lease_integrated
    ALTER COLUMN asset_category_cd TYPE VARCHAR(10);

-- FK追加
ALTER TABLE ctb_lease_integrated
    ADD CONSTRAINT fk_ctb_asset_category
    FOREIGN KEY (asset_category_cd) REFERENCES m_asset_category (asset_category_cd);
