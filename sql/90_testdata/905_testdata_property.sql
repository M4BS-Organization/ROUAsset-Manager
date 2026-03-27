-- ============================================================
-- テストデータ: ctb_contract_property + ctb_property_attribute
-- 3件の物件にEAV属性データを投入（建物/車両/OA機器）
--
-- 前提: 906_testdata_d_kykm.sql + SP実行済み
--       → ctb_contract_property にデータが存在すること
-- ============================================================

-- EAV属性: 建物(AC01) の属性値 — 福岡支社 倉庫 (kykh_id=9009)
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'RC'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_structure'
WHERE c.kykh_id = 9009 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, '150 sqm'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_area'
WHERE c.kykh_id = 9009 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

-- EAV属性: 車両(AC04) の属性値 — トヨタ カローラ (kykh_id=9003)
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'NRE210-1234567'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_chassis_no'
WHERE c.kykh_id = 9003 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, '500-1234'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_registration_no'
WHERE c.kykh_id = 9003 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

-- EAV属性: OA機器(AC03) の属性値 — Cisco Catalyst (kykh_id=9010)
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'C9300-48T-A'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_model_no'
WHERE c.kykh_id = 9010 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'FCW2345L0AB'
FROM ctb_contract_property p
INNER JOIN ctb_lease_integrated c ON p.ctb_id = c.ctb_id
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_serial_no'
WHERE c.kykh_id = 9010 AND p.property_no = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;
