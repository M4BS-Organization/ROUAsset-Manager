-- ============================================================
-- LeaseM4BS 仕訳出力ワークテーブル DDL
-- Form_f_仕訳出力標準_KJ / SH / SM が使用する出力先テーブル
-- Issue #35: tw_系スキーマ設計と管理戦略
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ワークテーブル削除
-- ============================================================
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_sm_仕訳data" CASCADE;
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_sm" CASCADE;
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_sh_仕訳data" CASCADE;
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_sh" CASCADE;
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_kj_仕訳data" CASCADE;
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_kj" CASCADE;

-- ============================================================
-- tw_f_仕訳出力標準_kj: 月次仕訳計上フレックス 出力制御ワーク
-- Form_f_仕訳出力標準_KJ:110-119 で DELETE→INSERT
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_kj" (
    id              SERIAL PRIMARY KEY,
    "対象年月"       DATE,
    keijo_dt        DATE
);

-- ============================================================
-- tw_f_仕訳出力標準_kj_仕訳data: KJ仕訳データ出力先
-- Form_f_仕訳出力標準_KJ:543-556 のINSERT文から逆引き
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_kj_仕訳data" (
    id              SERIAL PRIMARY KEY,
    "仕訳seqno"     INTEGER,
    "仕訳枝no"      INTEGER,
    kjkbn_id        INTEGER,
    "仕訳ptn_no"    INTEGER,
    "仕訳ptn"       VARCHAR(100),
    "対象年月"       DATE,
    "計上日"         DATE,
    kykbnl          VARCHAR(100),
    kykh_no         DOUBLE PRECISION,
    saikaisu        INTEGER,
    kykm_no         DOUBLE PRECISION,
    bukn_bango1     VARCHAR(100),
    bukn_bango2     VARCHAR(100),
    bukn_bango3     VARCHAR(100),
    bukn_nm         VARCHAR(200),
    lsryo_total     DOUBLE PRECISION DEFAULT 0,
    zritu           DOUBLE PRECISION DEFAULT 0,
    lcpt_id         INTEGER,
    kknri_id        INTEGER,
    b_bcat_id       INTEGER,
    h_bcat_id       INTEGER,
    skmk_id         INTEGER,
    "貸借区分id"     INTEGER,
    "借方科目no"     INTEGER,
    "借方科目cd"     VARCHAR(50),
    "借方科目"       VARCHAR(100),
    "借方金額"       DOUBLE PRECISION DEFAULT 0,
    "貸方科目no"     INTEGER,
    "貸方科目cd"     VARCHAR(50),
    "貸方科目"       VARCHAR(100),
    "貸方金額"       DOUBLE PRECISION DEFAULT 0
);

CREATE INDEX "idx_tw_f_kj_仕訳data_seq" ON "tw_f_仕訳出力標準_kj_仕訳data" ("仕訳seqno");

-- ============================================================
-- tw_f_仕訳出力標準_sh: 月次支払照合フレックス 出力制御ワーク
-- Form_f_仕訳出力標準_SH:86-109 で DELETE→INSERT
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_sh" (
    id                      SERIAL PRIMARY KEY,
    "対象年月"               DATE,
    keijo_dt                DATE,
    swksh_keijo_dt_kind     INTEGER DEFAULT 1
);

-- ============================================================
-- tw_f_仕訳出力標準_sh_仕訳data: SH仕訳データ出力先
-- Form_f_仕訳出力標準_SH:606-621 のINSERT文から逆引き
-- KJと同じ構造 + shho_id, 支払日, 記帳支払日
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_sh_仕訳data" (
    id              SERIAL PRIMARY KEY,
    "仕訳seqno"     INTEGER,
    "仕訳枝no"      INTEGER,
    kjkbn_id        INTEGER,
    "仕訳ptn_no"    INTEGER,
    "仕訳ptn"       VARCHAR(100),
    "対象年月"       DATE,
    "計上日"         DATE,
    kykbnl          VARCHAR(100),
    kykh_no         DOUBLE PRECISION,
    saikaisu        INTEGER,
    kykm_no         DOUBLE PRECISION,
    bukn_bango1     VARCHAR(100),
    bukn_bango2     VARCHAR(100),
    bukn_bango3     VARCHAR(100),
    bukn_nm         VARCHAR(200),
    lsryo_total     DOUBLE PRECISION DEFAULT 0,
    zritu           DOUBLE PRECISION DEFAULT 0,
    lcpt_id         INTEGER,
    kknri_id        INTEGER,
    b_bcat_id       INTEGER,
    h_bcat_id       INTEGER,
    skmk_id         INTEGER,
    shho_id         INTEGER,
    "支払日"         DATE,
    "記帳支払日"     DATE,
    "貸借区分id"     INTEGER,
    "借方科目no"     INTEGER,
    "借方科目cd"     VARCHAR(50),
    "借方科目"       VARCHAR(100),
    "借方金額"       DOUBLE PRECISION DEFAULT 0,
    "貸方科目no"     INTEGER,
    "貸方科目cd"     VARCHAR(50),
    "貸方科目"       VARCHAR(100),
    "貸方金額"       DOUBLE PRECISION DEFAULT 0
);

CREATE INDEX "idx_tw_f_sh_仕訳data_seq" ON "tw_f_仕訳出力標準_sh_仕訳data" ("仕訳seqno");

-- ============================================================
-- tw_f_仕訳出力標準_sm: リース債務返済明細表 出力制御ワーク
-- Form_f_仕訳出力標準_SM:91-103 で DELETE→INSERT
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_sm" (
    id              SERIAL PRIMARY KEY,
    "対象年月"       DATE,
    keijo1_dt       DATE,
    keijo2_dt       DATE
);

-- ============================================================
-- tw_f_仕訳出力標準_sm_仕訳data: SM仕訳データ出力先
-- Form_f_仕訳出力標準_SM:536-549 のINSERT文から逆引き
-- KJより少ないカラム（saikaisu無し、lsryo_total/zritu無し等）
-- ============================================================
CREATE TABLE "tw_f_仕訳出力標準_sm_仕訳data" (
    id              SERIAL PRIMARY KEY,
    "仕訳seqno"     INTEGER,
    "仕訳枝no"      INTEGER,
    kjkbn_id        INTEGER,
    "仕訳ptn_no"    INTEGER,
    "仕訳ptn"       VARCHAR(100),
    "対象年月"       DATE,
    "計上日"         DATE,
    kykbnl          VARCHAR(100),
    kykh_no         DOUBLE PRECISION,
    kykm_no         DOUBLE PRECISION,
    bukn_bango1     VARCHAR(100),
    bukn_bango2     VARCHAR(100),
    bukn_bango3     VARCHAR(100),
    bukn_nm         VARCHAR(200),
    lcpt_id         INTEGER,
    kknri_id        INTEGER,
    b_bcat_id       INTEGER,
    skmk_id         INTEGER,
    "貸借区分id"     INTEGER,
    "借方科目no"     INTEGER,
    "借方科目cd"     VARCHAR(50),
    "借方科目"       VARCHAR(100),
    "借方金額"       DOUBLE PRECISION DEFAULT 0,
    "貸方科目no"     INTEGER,
    "貸方科目cd"     VARCHAR(50),
    "貸方科目"       VARCHAR(100),
    "貸方金額"       DOUBLE PRECISION DEFAULT 0
);

CREATE INDEX "idx_tw_f_sm_仕訳data_seq" ON "tw_f_仕訳出力標準_sm_仕訳data" ("仕訳seqno");

COMMIT;
