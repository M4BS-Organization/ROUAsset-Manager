-- ============================================================
-- m_property_attribute_def: 初期属性定義シードデータ
-- 旧 re_*7列、vh_*5列、oa_*4列 をEAV定義に移行
-- ============================================================

-- AC01: 建物
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC01', 're_structure',          '構造',       'TEXT',  'TEXTBOX',      100, FALSE, 10),
    ('AC01', 're_area',               '面積',       'TEXT',  'TEXTBOX',       50, FALSE, 20),
    ('AC01', 're_layout',             '間取り',     'TEXT',  'TEXTBOX',       50, FALSE, 30),
    ('AC01', 're_completion_date',    '竣工日',     'DATE',  'DATEPICKER',  NULL, FALSE, 40),
    ('AC01', 're_landlord_name',      '貸主名',     'TEXT',  'TEXTBOX',      100, FALSE, 50),
    ('AC01', 're_broker_company',     '仲介会社',   'TEXT',  'TEXTBOX',      100, FALSE, 60),
    ('AC01', 're_usage_restrictions', '用途制限',   'TEXT',  'TEXTAREA',     200, FALSE, 70)
ON CONFLICT (asset_category_cd, attr_cd) DO NOTHING;

-- AC02: 構築物（建物と同じ属性セット）
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC02', 're_structure',          '構造',       'TEXT',  'TEXTBOX',      100, FALSE, 10),
    ('AC02', 're_area',               '面積',       'TEXT',  'TEXTBOX',       50, FALSE, 20),
    ('AC02', 're_layout',             '間取り',     'TEXT',  'TEXTBOX',       50, FALSE, 30),
    ('AC02', 're_completion_date',    '竣工日',     'DATE',  'DATEPICKER',  NULL, FALSE, 40),
    ('AC02', 're_landlord_name',      '貸主名',     'TEXT',  'TEXTBOX',      100, FALSE, 50),
    ('AC02', 're_broker_company',     '仲介会社',   'TEXT',  'TEXTBOX',      100, FALSE, 60),
    ('AC02', 're_usage_restrictions', '用途制限',   'TEXT',  'TEXTAREA',     200, FALSE, 70)
ON CONFLICT (asset_category_cd, attr_cd) DO NOTHING;

-- AC03: 機械装置
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC03', 'oa_model_no',            '型番',           'TEXT',  'TEXTBOX',       50, FALSE, 10),
    ('AC03', 'oa_serial_no',           'シリアル番号',   'TEXT',  'TEXTBOX',       50, FALSE, 20),
    ('AC03', 'oa_maintenance_date',    '保守期限',       'DATE',  'DATEPICKER',  NULL, FALSE, 30),
    ('AC03', 'oa_maintenance_contract','保守契約',       'TEXT',  'TEXTBOX',      200, FALSE, 40)
ON CONFLICT (asset_category_cd, attr_cd) DO NOTHING;

-- AC04: 車両運搬具
INSERT INTO m_property_attribute_def
    (asset_category_cd, attr_cd, attr_nm, data_type, display_type, max_length, is_required, sort_order)
VALUES
    ('AC04', 'vh_chassis_no',      '車台番号',       'TEXT',  'TEXTBOX',       50, FALSE, 10),
    ('AC04', 'vh_registration_no', '登録番号',       'TEXT',  'TEXTBOX',       50, FALSE, 20),
    ('AC04', 'vh_vehicle_type',    '車種',           'TEXT',  'TEXTBOX',      100, FALSE, 30),
    ('AC04', 'vh_inspection_date', '車検期限',       'DATE',  'DATEPICKER',  NULL, FALSE, 40),
    ('AC04', 'vh_mileage_limit',   '走行距離制限',   'TEXT',  'TEXTBOX',       50, FALSE, 50)
ON CONFLICT (asset_category_cd, attr_cd) DO NOTHING;

-- AC05: その他（初期は属性なし。ユーザーが管理画面から随時追加）
