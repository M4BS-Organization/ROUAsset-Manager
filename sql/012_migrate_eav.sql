-- ============================================================
-- データ移行: ctb_lease_integrated → ctb_property
-- 既存10件のd_kykh移行データをctb_propertyに転写
-- 種別固有属性は空（d_kykhには種別固有情報が存在しないため）
-- ============================================================

BEGIN;

-- Step 1: ctb_property レコード作成
-- asset_category が空の場合は AC05(その他) をデフォルト設定
INSERT INTO ctb_property (
    ctb_id,
    property_no,
    asset_category_cd,
    asset_no,
    asset_name,
    company_name,
    install_location,
    remarks
)
SELECT
    c.ctb_id,
    c.property_no,
    CASE
        WHEN c.asset_category = '不動産' THEN 'AC01'
        WHEN c.asset_category = '構築物' THEN 'AC02'
        WHEN c.asset_category = '機械装置' THEN 'AC03'
        WHEN c.asset_category = '車両'   THEN 'AC04'
        WHEN c.asset_category = 'OA機器' THEN 'AC05'
        WHEN c.asset_category IS NULL OR c.asset_category = '' THEN 'AC05'
        ELSE 'AC05'
    END,
    c.asset_no,
    NULL,           -- asset_name: 契約名とは別。移行時は空
    c.company_name,
    c.install_location,
    c.remarks
FROM ctb_lease_integrated c
ON CONFLICT (ctb_id, property_no) DO NOTHING;

COMMIT;

-- 検証クエリ
-- SELECT p.property_id, p.ctb_id, p.property_no, p.asset_category_cd,
--        ac.asset_category_nm, p.asset_name
-- FROM ctb_property p
-- INNER JOIN m_asset_category ac ON p.asset_category_cd = ac.asset_category_cd
-- ORDER BY p.ctb_id;
