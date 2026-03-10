-- psql -U manager -d lease_new -f seed_data.sql

INSERT INTO m_company (company_cd, company_nm) VALUES
    ('0001', '本社'),
    ('0002', '東日本支社'),
    ('0003', '西日本支社')
ON CONFLICT (company_cd) DO NOTHING;

INSERT INTO m_department (dept_cd, dept_nm) VALUES
    ('001', '総務部'),
    ('002', '経理部'),
    ('003', '営業部'),
    ('004', '情報システム部'),
    ('005', '人事部')
ON CONFLICT (dept_cd) DO NOTHING;

INSERT INTO m_supplier (supplier_cd, supplier_nm) VALUES
    ('S001', '三井不動産株式会社'),
    ('S002', '三菱地所株式会社'),
    ('S003', '住友不動産株式会社'),
    ('S004', '東急不動産株式会社'),
    ('S005', '野村不動産株式会社')
ON CONFLICT (supplier_cd) DO NOTHING;

INSERT INTO m_bank_account (bank_account_cd, bank_account_nm) VALUES
    ('B001', '三菱UFJ銀行 本店 普通 1234567'),
    ('B002', 'みずほ銀行 丸の内支店 普通 2345678'),
    ('B003', '三井住友銀行 東京営業部 普通 3456789')
ON CONFLICT (bank_account_cd) DO NOTHING;

INSERT INTO m_asset_category (asset_category_cd, asset_category_nm) VALUES
    ('AC01', '建物'),
    ('AC02', '構築物'),
    ('AC03', '機械装置'),
    ('AC04', '車両運搬具'),
    ('AC05', 'その他')
ON CONFLICT (asset_category_cd) DO NOTHING;

INSERT INTO m_contract_type (contract_type_cd, contract_type_nm, sort_order) VALUES
    ('01', '普通賃貸', 1),
    ('02', '定期賃貸', 2)
ON CONFLICT (contract_type_cd) DO NOTHING;

INSERT INTO m_initial_cost_item (cost_item_cd, cost_item_nm, sort_order) VALUES
    ('01', '敷金', 1),
    ('02', '敷金償却額（返還不能分）', 2),
    ('03', '礼金', 3),
    ('04', '仲介手数料', 4)
ON CONFLICT (cost_item_cd) DO NOTHING;

INSERT INTO m_acct_treatment (acct_treatment_cd, acct_treatment_nm, sort_order) VALUES
    ('01', '資産計上', 1),
    ('02', '費用処理', 2),
    ('03', '繰延処理', 3),
    ('04', '預り金処理', 4)
ON CONFLICT (acct_treatment_cd) DO NOTHING;

INSERT INTO m_monthly_item (monthly_item_cd, monthly_item_nm, sort_order) VALUES
    ('01', '賃料', 1),
    ('02', '管理費', 2),
    ('03', '共益費', 3)
ON CONFLICT (monthly_item_cd) DO NOTHING;

INSERT INTO m_payment_method (payment_method_cd, payment_method_nm) VALUES
    ('01', '振込'),
    ('02', '口座振替'),
    ('03', '手形'),
    ('04', '現金')
ON CONFLICT (payment_method_cd) DO NOTHING;
