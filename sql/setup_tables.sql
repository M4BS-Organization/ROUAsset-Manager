-- tw_m_user: ユーザーマスタテーブル
CREATE TABLE IF NOT EXISTS tw_m_user (
    user_id       INTEGER PRIMARY KEY,
    user_name     VARCHAR(100),
    user_kana     VARCHAR(100),
    create_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- d_asset: 資産情報テーブル（契約関連カラム統合済み）
-- 旧 td_sisn テーブルの契約カラムを統合
CREATE TABLE IF NOT EXISTS d_asset (
    asset_id        SERIAL PRIMARY KEY,
    -- 資産基本情報
    account_class   VARCHAR(50),
    asset_no        VARCHAR(50),
    quantity        INTEGER DEFAULT 1,
    bukken_nm       VARCHAR(200),
    shozaichi       VARCHAR(500),
    kukaku          VARCHAR(100),
    menseki         VARCHAR(50),
    madori          VARCHAR(100),
    kozo_yoto       VARCHAR(200),
    yoto_seigen     VARCHAR(500),
    taiyo_nensu     INTEGER,
    shunko_dt       DATE,
    kashushi_nm     VARCHAR(200),
    chukai_nm       VARCHAR(200),
    kessai_nm       VARCHAR(200),
    hosho_nm        VARCHAR(200),
    -- 契約関連情報（旧 td_sisn から統合）
    mgmt_unit       VARCHAR(50),
    contract_type   VARCHAR(50),
    acct_target     VARCHAR(50),
    payee           VARCHAR(200),
    contract_no     VARCHAR(50),
    own_mgmt        VARCHAR(50),
    approval_no     VARCHAR(50),
    re_lease_cnt    INTEGER DEFAULT 0,
    contract_nm     VARCHAR(200),
    start_dt        DATE,
    end_dt          DATE,
    contract_period INTEGER,
    cash_price      DECIMAL(18,2),
    monthly_lease   DECIMAL(18,2),
    consistency     VARCHAR(10),
    upd_user_id     VARCHAR(50),
    upd_user_nm     VARCHAR(100),
    -- 共通タイムスタンプ
    create_dt       TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt       TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- d_asset_equipment: 資産付属設備テーブル
CREATE TABLE IF NOT EXISTS d_asset_equipment (
    eq_id       SERIAL PRIMARY KEY,
    asset_id    INTEGER NOT NULL REFERENCES d_asset(asset_id) ON DELETE CASCADE,
    eq_name     VARCHAR(200),
    eq_amount   VARCHAR(50),
    eq_date     VARCHAR(50)
);

-- スキーマ権限付与
GRANT ALL ON SCHEMA public TO lease_m4bs_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO lease_m4bs_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO lease_m4bs_user;
