-- d_asset: 資産情報テーブル
CREATE TABLE IF NOT EXISTS d_asset (
    asset_id      SERIAL PRIMARY KEY,
    account_class VARCHAR(50),
    asset_no      VARCHAR(50),
    quantity      INTEGER DEFAULT 1,
    bukken_nm     VARCHAR(200),
    shozaichi     VARCHAR(500),
    kukaku        VARCHAR(100),
    menseki       VARCHAR(50),
    madori        VARCHAR(100),
    kozo_yoto     VARCHAR(200),
    yoto_seigen   VARCHAR(500),
    taiyo_nensu   INTEGER,
    shunko_dt     DATE,
    kashushi_nm   VARCHAR(200),
    chukai_nm     VARCHAR(200),
    kessai_nm     VARCHAR(200),
    hosho_nm      VARCHAR(200),
    create_dt     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt     TIMESTAMP DEFAULT CURRENT_TIMESTAMP
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
