-- tw_m_user: ユーザーマスタテーブル
CREATE TABLE IF NOT EXISTS tw_m_user (
    user_id       INTEGER PRIMARY KEY,
    user_name     VARCHAR(100),
    user_kana     VARCHAR(100),
    create_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_date   TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- td_sisn: 契約（リース試算）トランザクションテーブル
CREATE TABLE IF NOT EXISTS td_sisn (
    sisn_id       SERIAL PRIMARY KEY,
    knrtni        VARCHAR(50),
    kykkbn        VARCHAR(50),
    kjotis        VARCHAR(50),
    shrsk         VARCHAR(200),
    kykno         VARCHAR(50) UNIQUE,
    jshknr        VARCHAR(50),
    rngno         VARCHAR(50),
    srsks         INTEGER DEFAULT 0,
    kyknm         VARCHAR(200),
    kisymd        DATE,
    syrymd        DATE,
    kykkk         INTEGER,
    gnknkngk      DECIMAL(18,2),
    gtkrsry       DECIMAL(18,2),
    ssnsry        INTEGER,
    sigo          VARCHAR(10),
    updusrid      VARCHAR(50),
    updusrnm      VARCHAR(100),
    insid         VARCHAR(50),
    insnm         VARCHAR(100),
    kosnid        VARCHAR(50),
    kosnnm        VARCHAR(100),
    insdt         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    kosndt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

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
