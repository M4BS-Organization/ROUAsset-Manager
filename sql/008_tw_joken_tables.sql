-- ============================================================
-- LeaseM4BS 条件ワークテーブル DDL
-- 仕訳出力フォーム（KJ/SH/SM）が処理前に参照する条件テーブル
-- f_KEIJO_JOKEN / f_flx_TOUGETSU / f_CHUKI_JOKEN 等が書き込み
-- Issue #35: tw_系スキーマ設計と管理戦略
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ワークテーブル削除
-- ============================================================
DROP TABLE IF EXISTS public.tw_s_saimu_joken CASCADE;
DROP TABLE IF EXISTS public.tw_s_tougetsu_joken CASCADE;
DROP TABLE IF EXISTS public.tw_s_keijo_joken CASCADE;

-- ============================================================
-- tw_s_keijo_joken: 計上条件ワーク
-- Form_f_仕訳出力標準_KJ が SELECT * LIMIT 1 で存在チェック
-- f_KEIJO_JOKEN が処理実行時に書き込む
-- ============================================================
CREATE TABLE public.tw_s_keijo_joken (
    id              SERIAL PRIMARY KEY,
    -- 計上条件パラメータ（f_KEIJO_JOKEN の画面入力値）
    meisai          INTEGER,            -- 明細単位 (1:物件 / 2:配賦)
    taisho          INTEGER,            -- 対象区分 (1:リース / 2:保守 / 3:全部)
    kjkbn_sisan     BOOLEAN DEFAULT FALSE, -- 計上区分: 資産を含む
    kjkbn_hiyo      BOOLEAN DEFAULT FALSE, -- 計上区分: 費用を含む
    sa_joken        VARCHAR(500),       -- ユーザー指定フィルタ条件
    hensai_kind     INTEGER,            -- 新法費用の返済方法
    shori_end_f     BOOLEAN DEFAULT FALSE, -- 処理完了残高当月埋めフラグ
    skyak_ho_id     INTEGER,            -- 償却方法ID
    kikan_from      DATE,               -- 対象期間FROM
    kikan_to        DATE,               -- 対象期間TO
    exec_dt         DATE                -- 実行日時
);

-- ============================================================
-- tw_s_tougetsu_joken: 当月条件ワーク
-- Form_f_仕訳出力標準_SH が SELECT * LIMIT 1 で存在チェック
-- Form_f_仕訳出力標準_SH が SELECT sw_rsok LIMIT 1 で利息返済フラグを参照
-- f_flx_TOUGETSU が月次処理実行時に書き込む
-- ============================================================
CREATE TABLE public.tw_s_tougetsu_joken (
    id              SERIAL PRIMARY KEY,
    -- 当月処理条件
    sw_rsok         BOOLEAN DEFAULT FALSE, -- 利息返済計算済みフラグ
    kikan_from      DATE,               -- 対象期間FROM
    kikan_to        DATE,               -- 対象期間TO
    exec_dt         DATE                -- 実行日時
);

-- ============================================================
-- tw_s_saimu_joken: 債務条件ワーク
-- Form_f_仕訳出力標準_SM が SELECT * LIMIT 1 で存在チェック
-- 債務返済明細表の集計条件実行時に書き込む
-- ============================================================
CREATE TABLE public.tw_s_saimu_joken (
    id              SERIAL PRIMARY KEY,
    -- 債務条件パラメータ
    kikan_from      DATE,               -- 対象期間FROM
    kikan_to        DATE,               -- 対象期間TO
    exec_dt         DATE                -- 実行日時
);

COMMIT;
