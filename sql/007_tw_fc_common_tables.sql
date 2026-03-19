-- ============================================================
-- LeaseM4BS fc_系顧客固有仕訳出力 共通ワークテーブル DDL
-- Access版 tw_fc_支払仕訳 / tw_fc_計上仕訳 等に相当
-- FcJournalOutputBase が tw_s_chuki_keijo を変換して書き込む中間ワーク
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ワークテーブル削除
-- ============================================================
DROP TABLE IF EXISTS public.tw_fc_swk_wrk CASCADE;

-- ============================================================
-- tw_fc_swk_wrk: fc_系共通仕訳出力ワークテーブル
-- Access版 tw_fc_支払仕訳 / tw_fc_計上仕訳 等の共通スキーマ
-- tw_s_chuki_keijo → 顧客固有SQL → このワークテーブル → 出力
-- ============================================================
CREATE TABLE public.tw_fc_swk_wrk (
    id                  SERIAL PRIMARY KEY,

    -- 顧客識別
    customer_cd         VARCHAR(20) NOT NULL,       -- 顧客コード (KITOKU, TSYSCOM 等)
    swk_kbn             VARCHAR(20) NOT NULL,       -- 仕訳区分 (支払仕訳, 計上仕訳, 経費仕訳 等)

    -- 伝票情報
    den_no              VARCHAR(20),                -- 伝票番号
    den_date            VARCHAR(10),                -- 伝票日付 (YYYY/MM/DD)
    gyo_no              INTEGER DEFAULT 0,          -- 行番号

    -- 借方科目
    dr_kmk_cd           VARCHAR(20),               -- 借方科目コード
    dr_kmk_nm           VARCHAR(100),              -- 借方科目名称
    dr_hkm_cd           VARCHAR(20),               -- 借方補助科目コード
    dr_bmn_cd           VARCHAR(20),               -- 借方部門コード
    dr_kin              DOUBLE PRECISION DEFAULT 0, -- 借方金額
    dr_zei_kin          DOUBLE PRECISION DEFAULT 0, -- 借方消費税額

    -- 貸方科目
    cr_kmk_cd           VARCHAR(20),               -- 貸方科目コード
    cr_kmk_nm           VARCHAR(100),              -- 貸方科目名称
    cr_hkm_cd           VARCHAR(20),               -- 貸方補助科目コード
    cr_bmn_cd           VARCHAR(20),               -- 貸方部門コード
    cr_kin              DOUBLE PRECISION DEFAULT 0, -- 貸方金額
    cr_zei_kin          DOUBLE PRECISION DEFAULT 0, -- 貸方消費税額

    -- 摘要・補足
    tekiyo              VARCHAR(200),              -- 摘要
    zei_kbn             VARCHAR(4),                -- 消費税区分
    zritu               DOUBLE PRECISION DEFAULT 0, -- 消費税率

    -- 元データ参照 (tw_s_chuki_keijo)
    kykm_id             DOUBLE PRECISION,           -- 物件明細ID
    kykh_id             DOUBLE PRECISION,           -- 契約ヘッダID
    bukn_nm             VARCHAR(200),              -- 物件名
    kykbnl_no           VARCHAR(100),              -- 契約番号
    shri_dt             DATE,                       -- 支払日
    lcpt_id             INTEGER,                    -- 支払先ID
    lsryo               DOUBLE PRECISION DEFAULT 0, -- リース料（税抜）
    zei                 DOUBLE PRECISION DEFAULT 0, -- 消費税額
    rec_kbn             INTEGER,                    -- レコード区分 (1:定額 2:変額 3:付随費用)
    kjkbn_id            INTEGER,                    -- 計上区分 (1:費用 2:資産)

    -- 顧客固有拡張カラム（各顧客が必要に応じて使用）
    ext_char1           VARCHAR(100),
    ext_char2           VARCHAR(100),
    ext_char3           VARCHAR(100),
    ext_char4           VARCHAR(100),
    ext_num1            DOUBLE PRECISION DEFAULT 0,
    ext_num2            DOUBLE PRECISION DEFAULT 0,

    -- 処理管理
    shori_dt            DATE,                       -- 処理日
    output_f            BOOLEAN DEFAULT FALSE,       -- 出力済みフラグ
    created_at          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_tw_fc_swk_wrk_customer ON public.tw_fc_swk_wrk (customer_cd, swk_kbn);
CREATE INDEX idx_tw_fc_swk_wrk_den ON public.tw_fc_swk_wrk (den_no, gyo_no);
CREATE INDEX idx_tw_fc_swk_wrk_kykm ON public.tw_fc_swk_wrk (kykm_id);

COMMENT ON TABLE public.tw_fc_swk_wrk IS 'fc_系顧客固有仕訳出力共通ワークテーブル。FcJournalOutputBase.BuildOutputSql()がtw_s_chuki_keijoから変換して書き込む。';
COMMENT ON COLUMN public.tw_fc_swk_wrk.customer_cd IS '顧客コード: KITOKU, TSYSCOM, KYOTO, YAMASHIN 等';
COMMENT ON COLUMN public.tw_fc_swk_wrk.swk_kbn IS '仕訳区分: 支払仕訳, 計上仕訳, 経費仕訳, 移動仕訳, 長短振替仕訳';

COMMIT;
