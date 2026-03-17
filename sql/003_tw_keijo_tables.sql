-- ============================================================
-- LeaseM4BS 計上ワークテーブル DDL
-- Access版 tw_S_CHUKI_KEIJO / tw_D_HENL_KEIJO / tw_D_GSON_KEIJO 相当
-- pc_SHRI_KEIJO.mKEIJO_Sub_SCHtoWK の出力先
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ワークテーブル削除
-- ============================================================
DROP TABLE IF EXISTS public.tw_d_gson_keijo CASCADE;
DROP TABLE IF EXISTS public.tw_d_henl_keijo CASCADE;
DROP TABLE IF EXISTS public.tw_s_chuki_keijo CASCADE;

-- ============================================================
-- tw_s_chuki_keijo: 注記計上結果ワーク（メイン出力先）
-- Access版 pc_SHRI_KEIJO の rslTO (物件単位/配賦単位) 出力先
-- ============================================================
CREATE TABLE public.tw_s_chuki_keijo (
    id                  SERIAL PRIMARY KEY,
    -- 契約情報
    kykh_id             DOUBLE PRECISION,
    kykm_id             DOUBLE PRECISION,
    kykm_no             DOUBLE PRECISION,
    saikaisu_kykm       INTEGER,
    bukn_nm             VARCHAR(200),
    kykbnl_no           VARCHAR(100),
    -- 計上条件
    kkbn_id             INTEGER,
    kjkbn_id            INTEGER,
    leakbn_id           INTEGER,
    lcpt_id             INTEGER,
    -- 計算パラメータ
    hensai_kind         INTEGER,
    rec_kbn             INTEGER,
    -- スケジュール1行分
    shri_dt             DATE,
    sime_dt             DATE,
    lsryo               DOUBLE PRECISION DEFAULT 0,
    zei                 DOUBLE PRECISION DEFAULT 0,
    zritu               DOUBLE PRECISION DEFAULT 0,
    shho_id             INTEGER,
    mae_f               BOOLEAN DEFAULT FALSE,
    ckaiyk_f            BOOLEAN DEFAULT FALSE,
    -- 計上結果
    keijo_f             BOOLEAN DEFAULT FALSE,
    keijo_dt            DATE,
    sumikaisu_zen       INTEGER DEFAULT 0,
    keijo_shri_cnt      INTEGER DEFAULT 0,
    lsryo_total         DOUBLE PRECISION DEFAULT 0,
    lsryo_toki          DOUBLE PRECISION DEFAULT 0,
    zei_total           DOUBLE PRECISION DEFAULT 0,
    zei_toki            DOUBLE PRECISION DEFAULT 0,
    -- 配賦情報
    line_id             INTEGER,
    haifritu            DOUBLE PRECISION,
    hkmk_id             INTEGER,
    h_bcat_id           INTEGER,
    rsrvh1_id           INTEGER,
    h_zokusei1          VARCHAR(200),
    h_zokusei2          VARCHAR(200),
    h_zokusei3          VARCHAR(200),
    h_zokusei4          VARCHAR(200),
    h_zokusei5          VARCHAR(200),
    -- 前払情報
    mae_zou             DOUBLE PRECISION DEFAULT 0,
    mae_gen             DOUBLE PRECISION DEFAULT 0,
    mzei_zou            DOUBLE PRECISION DEFAULT 0,
    mzei_gen            DOUBLE PRECISION DEFAULT 0,
    -- 減損情報
    gson_dt             DATE,
    -- 処理日
    shori_dt            DATE
);

CREATE INDEX idx_tw_s_chuki_keijo_kykm ON public.tw_s_chuki_keijo (kykm_id);
CREATE INDEX idx_tw_s_chuki_keijo_keijo_dt ON public.tw_s_chuki_keijo (keijo_dt);

-- ============================================================
-- tw_d_henl_keijo: 変額リース仕訳ワーク
-- Access版 tw_D_HENL_KEIJO 相当。tw_s_chuki_keijo と同等スキーマ。
-- ============================================================
CREATE TABLE public.tw_d_henl_keijo (
    id                  SERIAL PRIMARY KEY,
    kykh_id             DOUBLE PRECISION,
    kykm_id             DOUBLE PRECISION,
    kykm_no             DOUBLE PRECISION,
    saikaisu_kykm       INTEGER,
    bukn_nm             VARCHAR(200),
    kykbnl_no           VARCHAR(100),
    kkbn_id             INTEGER,
    kjkbn_id            INTEGER,
    leakbn_id           INTEGER,
    lcpt_id             INTEGER,
    hensai_kind         INTEGER,
    rec_kbn             INTEGER,
    shri_dt             DATE,
    sime_dt             DATE,
    lsryo               DOUBLE PRECISION DEFAULT 0,
    zei                 DOUBLE PRECISION DEFAULT 0,
    zritu               DOUBLE PRECISION DEFAULT 0,
    shho_id             INTEGER,
    mae_f               BOOLEAN DEFAULT FALSE,
    ckaiyk_f            BOOLEAN DEFAULT FALSE,
    keijo_f             BOOLEAN DEFAULT FALSE,
    keijo_dt            DATE,
    sumikaisu_zen       INTEGER DEFAULT 0,
    keijo_shri_cnt      INTEGER DEFAULT 0,
    lsryo_total         DOUBLE PRECISION DEFAULT 0,
    lsryo_toki          DOUBLE PRECISION DEFAULT 0,
    zei_total           DOUBLE PRECISION DEFAULT 0,
    zei_toki            DOUBLE PRECISION DEFAULT 0,
    line_id             INTEGER,
    haifritu            DOUBLE PRECISION,
    hkmk_id             INTEGER,
    h_bcat_id           INTEGER,
    rsrvh1_id           INTEGER,
    h_zokusei1          VARCHAR(200),
    h_zokusei2          VARCHAR(200),
    h_zokusei3          VARCHAR(200),
    h_zokusei4          VARCHAR(200),
    h_zokusei5          VARCHAR(200),
    mae_zou             DOUBLE PRECISION DEFAULT 0,
    mae_gen             DOUBLE PRECISION DEFAULT 0,
    mzei_zou            DOUBLE PRECISION DEFAULT 0,
    mzei_gen            DOUBLE PRECISION DEFAULT 0,
    gson_dt             DATE,
    shori_dt            DATE
);

CREATE INDEX idx_tw_d_henl_keijo_kykm ON public.tw_d_henl_keijo (kykm_id);
CREATE INDEX idx_tw_d_henl_keijo_keijo_dt ON public.tw_d_henl_keijo (keijo_dt);

-- ============================================================
-- tw_d_gson_keijo: 減損仕訳ワーク
-- Access版 tw_D_GSON_KEIJO 相当。tw_s_chuki_keijo のサブセット。
-- ============================================================
CREATE TABLE public.tw_d_gson_keijo (
    id                  SERIAL PRIMARY KEY,
    kykh_id             DOUBLE PRECISION,
    kykm_id             DOUBLE PRECISION,
    kykm_no             DOUBLE PRECISION,
    bukn_nm             VARCHAR(200),
    kkbn_id             INTEGER,
    kjkbn_id            INTEGER,
    leakbn_id           INTEGER,
    lcpt_id             INTEGER,
    rec_kbn             INTEGER,
    gson_dt             DATE,
    gson_ryo            DOUBLE PRECISION DEFAULT 0,
    keijo_dt            DATE,
    shori_dt            DATE
);

CREATE INDEX idx_tw_d_gson_keijo_kykm ON public.tw_d_gson_keijo (kykm_id);

COMMIT;
