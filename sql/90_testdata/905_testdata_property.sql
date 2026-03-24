-- ============================================================
-- テストデータ: ctb_property + ctb_property_attribute
-- 3件の物件にデータを投入（建物/車両/機械装置）
-- ============================================================

-- J-2025-0004 (ctb_id=1) 機械装置
UPDATE ctb_property SET
    asset_no = 'ASSET-0001',
    asset_name = 'Cisco Catalyst 9300',
    asset_category_cd = 'AC03',
    company_name = 'NTT',
    install_location = 'Server Room 1F'
WHERE ctb_id = 1;

-- J-2025-0003 (ctb_id=10) 建物
UPDATE ctb_property SET
    asset_no = 'ASSET-0002',
    asset_name = 'Warehouse A',
    asset_category_cd = 'AC01',
    company_name = 'Fukuoka',
    install_location = 'Fukuoka Hakozaki'
WHERE ctb_id = 10;

-- J-2024-0003 (ctb_id=5) 車両
UPDATE ctb_property SET
    asset_no = 'ASSET-0003',
    asset_name = 'Toyota Corolla NRE210',
    asset_category_cd = 'AC04',
    company_name = 'Osaka',
    install_location = 'Osaka Parking'
WHERE ctb_id = 5;

-- EAV属性: 建物(AC01) の属性値
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'RC'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_structure'
WHERE p.ctb_id = 10
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, '500 sqm'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 're_area'
WHERE p.ctb_id = 10
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

-- EAV属性: 車両(AC04) の属性値
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'NRE210-1234567'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_chassis_no'
WHERE p.ctb_id = 5
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, '500-1234'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'vh_registration_no'
WHERE p.ctb_id = 5
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

-- EAV属性: 機械装置(AC03) の属性値
INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'C9300-48T-A'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_model_no'
WHERE p.ctb_id = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;

INSERT INTO ctb_property_attribute (property_id, attr_def_id, attribute_value)
SELECT p.property_id, d.attr_def_id, 'FCW2345L0AB'
FROM ctb_property p
INNER JOIN m_property_attribute_def d ON d.asset_category_cd = p.asset_category_cd AND d.attr_cd = 'oa_serial_no'
WHERE p.ctb_id = 1
ON CONFLICT (property_id, attr_def_id) DO UPDATE SET attribute_value = EXCLUDED.attribute_value;
