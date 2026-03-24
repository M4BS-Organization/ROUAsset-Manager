-- ============================================================
-- LeaseM4BS 注記計算結果ワークテーブル DDL
-- KeijoWorkTableManager.InsertChukiCalcRow の INSERT文から逆引き
-- Issue #35: tw_系スキーマ設計と管理戦略
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ワークテーブル削除
-- ============================================================
DROP TABLE IF EXISTS public.tw_s_chuki_calc CASCADE;

-- ============================================================
-- tw_s_chuki_calc: 注記計算結果ワーク
-- ChukiCalcEngine の出力先。契約明細単位の注記集計結果を保持。
-- Access版 LM4BSwork.mdb 内のワークテーブル相当。
-- ============================================================
CREATE TABLE public.tw_s_chuki_calc (
    id                      SERIAL PRIMARY KEY,
    -- 契約識別情報
    kykm_id                 DOUBLE PRECISION,
    kykh_id                 DOUBLE PRECISION,
    kykm_no                 DOUBLE PRECISION,
    bukn_nm                 VARCHAR(200),
    kykbnl_no               VARCHAR(100),
    leakbn_id               INTEGER,
    kjkbn_id                INTEGER,
    -- 取得価額（前残・増・減・残）
    syutok_zzan             DOUBLE PRECISION DEFAULT 0,
    syutok_zou              DOUBLE PRECISION DEFAULT 0,
    syutok_gen              DOUBLE PRECISION DEFAULT 0,
    syutok_zan              DOUBLE PRECISION DEFAULT 0,
    -- 減価償却累計額（前残・増・減・残・償却率）
    gruikei_zzan            DOUBLE PRECISION DEFAULT 0,
    gruikei_zou             DOUBLE PRECISION DEFAULT 0,
    gruikei_gen             DOUBLE PRECISION DEFAULT 0,
    gruikei_zan             DOUBLE PRECISION DEFAULT 0,
    skyak_ritu              DOUBLE PRECISION,
    -- 減損損失累計額（前残・増・減・残）
    gson_rkei_zzan          DOUBLE PRECISION DEFAULT 0,
    gson_rkei_zou           DOUBLE PRECISION DEFAULT 0,
    gson_rkei_gen           DOUBLE PRECISION DEFAULT 0,
    gson_rkei_zan           DOUBLE PRECISION DEFAULT 0,
    -- 簿価
    boka_zan                DOUBLE PRECISION DEFAULT 0,
    -- リース料元本（前残・残・1年内/超・2〜5年内/超）
    lgnpn_zzan              DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan               DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_1nai          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_1cho          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_2nai          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_3nai          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_4nai          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_5nai          DOUBLE PRECISION DEFAULT 0,
    lgnpn_zan_5cho          DOUBLE PRECISION DEFAULT 0,
    -- リース料利息（前残・残・1年内/超・2〜5年内/超）
    lrsok_zzan              DOUBLE PRECISION DEFAULT 0,
    lrsok_zan               DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_1nai          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_1cho          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_2nai          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_3nai          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_4nai          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_5nai          DOUBLE PRECISION DEFAULT 0,
    lrsok_zan_5cho          DOUBLE PRECISION DEFAULT 0,
    -- 未払利息
    risoku_mib_zan          DOUBLE PRECISION DEFAULT 0,
    -- 減損勘定（前残・残）
    gson_zzan               DOUBLE PRECISION,
    gson_zan                DOUBLE PRECISION,
    -- 当期発生
    lsryo_toki              DOUBLE PRECISION,
    lgnpn_toki              DOUBLE PRECISION,
    lrsok_toki              DOUBLE PRECISION,
    risoku_hassei_toki      DOUBLE PRECISION DEFAULT 0,
    gson_tk_toki            DOUBLE PRECISION,
    ijiknr_toki             DOUBLE PRECISION,
    lb_soneki_toki          DOUBLE PRECISION,
    lb_soneki_ruikei        DOUBLE PRECISION,
    -- 解約抹消
    lgnpn_kaiyak_gen        DOUBLE PRECISION,
    risoku_mib_kaiyak_gen   DOUBLE PRECISION DEFAULT 0,
    gson_kaiyak_gen         DOUBLE PRECISION
);

CREATE INDEX idx_tw_s_chuki_calc_kykm ON public.tw_s_chuki_calc (kykm_id);

COMMIT;
