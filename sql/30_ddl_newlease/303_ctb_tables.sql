-- ============================================================
-- 303_ctb_tables.sql
-- 共通基盤テーブル (CTB) 定義
--
-- 構成:
--   1. ctb_lease_integrated    - 定義書「09_共通基盤テーブル(CTB) 詳細定義書」準拠
--   2. ctb_dept_allocation     - 定義書準拠
--   3. ctb_remeasurement_history - 定義書準拠
--   4. ctb_contract_header     - 新規: d_kykh と型統一した契約基本情報
--   5. ctb_contract_property   - 新規: d_kykm と型統一した物件明細
--
-- 依存:
--   10_ddl_core/101_code_tables.sql   (c_kkbn)
--   10_ddl_core/102_master_tables.sql (m_kknri, m_lcpt, m_bkind)
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 1. 新基準統合メインテーブル (ctb_lease_integrated)
--    定義書「09_共通基盤テーブル(CTB) 詳細定義書_20260304」準拠
--    M4BS, M7, JSM10の全要素を統合。
--    新基準（ASBJ 34/33）に基づく計算エンジンが参照する基幹テーブル。
-- ============================================================

DROP TABLE IF EXISTS ctb_lease_integrated CASCADE;

CREATE TABLE IF NOT EXISTS ctb_lease_integrated (
    -- === PK ===
    ctb_id                      SERIAL          NOT NULL,

    -- === 契約・物件識別 ===
    contract_no                 VARCHAR(15)     NOT NULL,       -- リース契約の管理番号 (M4BS P.21)
    property_no                 INTEGER         NULL,           -- 1契約内の物件枝番 (M4BS P.21)

    -- === 外部システム連携 ===
    m7_asset_no                 VARCHAR(15)     NULL,           -- 固定資産管理上の番号 (M7 P.38)
    jsm10_aro_no                VARCHAR(15)     NULL,           -- ARO管理番号 (JSM10 P.2)

    -- === リース期間・延長 ===
    lease_start_date            DATE            NOT NULL,       -- 貸手が借手に資産を提供した日 (M4BS P.21)
    non_cancellable_months      INTEGER         NOT NULL,       -- 契約上の解約不能月数 (M4BS P.23)
    is_extension_certain        BOOLEAN         NULL,           -- 延長行使が合理的に確実か (新基準第34号 P.6)
    extension_months            INTEGER         NULL,           -- 延長オプションの対象期間 (新基準第34号 P.6)
    accounting_lease_term       INTEGER         NOT NULL,       -- 償却期間となる合計月数 (新基準第34号 P.6)

    -- === 支払・割引 ===
    periodic_payment_amt        NUMERIC(15,0)   NOT NULL,       -- 支払予定明細の単価 (M4BS P.23)
    payment_interval_months     INTEGER         NOT NULL,       -- 支払頻度(1/12ヶ月等) (M4BS P.22)
    discount_rate               NUMERIC(5,4)    NOT NULL,       -- NPV算出用割引率 (M4BS P.22)
    residual_value_guarantee    NUMERIC(15,0)   NULL,           -- リース終了時の保証額 (新基準第34号 P.7)
    purchase_option_amt         NUMERIC(15,0)   NULL,           -- 行使が確実な場合の価額 (新基準第34号 P.9)
    aro_present_value           NUMERIC(15,0)   NULL,           -- 使用権資産への加算額 (JSM10 P.2)

    -- === M7連携（部署・科目） ===
    m7_dept_cd                  VARCHAR(12)     NULL,           -- 資産管理責任部門 (M7 P.52)
    burden_dept_cd              VARCHAR(12)     NULL,           -- 費用計上先部門 (M7 P.52)
    asset_class_cd              VARCHAR(12)     NULL,           -- 使用権資産の勘定科目 (M7 P.50)
    segment_cd                  VARCHAR(12)     NULL,           -- 会計分析用区分 (M7 P.12)

    -- === 計算結果 ===
    initial_rou_asset           NUMERIC(15,0)   NULL,           -- NPV＋除去費用等の算定額 (新基準第34号 P.8)
    initial_lease_liability     NUMERIC(15,0)   NULL,           -- リース料現在価値の合計 (新基準第34号 P.8)

    -- === d_kykh逆参照（実装追加） ===
    kykh_id                     INTEGER         NULL,           -- d_kykh.kykh_id への逆参照

    -- === システム ===
    create_dt                   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt                   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_lease_integrated PRIMARY KEY (ctb_id),
    CONSTRAINT uq_ctb_contract_property UNIQUE (contract_no, property_no)
);

CREATE INDEX IF NOT EXISTS idx_ctb_contract_no ON ctb_lease_integrated (contract_no);
CREATE INDEX IF NOT EXISTS idx_ctb_kykh_id ON ctb_lease_integrated (kykh_id);

COMMENT ON TABLE ctb_lease_integrated IS '新基準統合メインテーブル（定義書準拠）';
COMMENT ON COLUMN ctb_lease_integrated.kykh_id IS 'd_kykh.kykh_id への逆参照（マイグレーション元の契約ID）';


-- ============================================================
-- 2. 多部門配賦テーブル (ctb_dept_allocation)
--    定義書準拠: シサンM7の配賦ロジック（70ページ）を継承
-- ============================================================

DROP TABLE IF EXISTS ctb_dept_allocation CASCADE;

CREATE TABLE IF NOT EXISTS ctb_dept_allocation (
    -- === PK ===
    allocation_id       SERIAL          NOT NULL,
    ctb_id              INTEGER         NOT NULL,           -- FK → ctb_lease_integrated
    line_id             SMALLINT        NOT NULL DEFAULT 1, -- 配賦行連番          ← d_haif.line_id

    -- === 配賦先（d_haif統一） ===
    h_bcat_id           INTEGER         NULL,               -- 部門ID              ← d_haif.h_bcat_id → m_bcat
    dept_cd             VARCHAR(12)     NOT NULL,           -- 部門コード (M7 P.70) ← m_bcat.bcat1_cd 経由
    haifritu            NUMERIC(5,2)    NOT NULL,           -- 配賦比率(%)         ← d_haif.haifritu
    hkmk_id             INTEGER         NULL,               -- 費目科目ID          ← d_haif.hkmk_id

    -- === 金額（d_haif統一） ===
    h_klsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦リース料        ← d_haif.h_klsryo
    h_mlsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦メンテナンス料  ← d_haif.h_mlsryo
    h_kzei              NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦リース消費税    ← d_haif.h_kzei
    h_mzei              NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦メンテ消費税    ← d_haif.h_mzei
    h_klsryo_zkomi      NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦リース税込      ← d_haif.h_klsryo_zkomi
    h_mlsryo_zkomi      NUMERIC(15,2)   NULL DEFAULT 0,     -- 配賦メンテ税込      ← d_haif.h_mlsryo_zkomi

    -- === 予備（d_haif統一） ===
    rsrvh1_id           INTEGER         NULL,               -- 配賦予備1 ID        ← d_haif.rsrvh1_id

    -- === システム ===
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_dept_allocation PRIMARY KEY (allocation_id),
    CONSTRAINT fk_dept_allocation_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS uq_dept_allocation_ctb_dept
    ON ctb_dept_allocation (ctb_id, dept_cd, line_id);

CREATE INDEX IF NOT EXISTS idx_dept_allocation_ctb_id
    ON ctb_dept_allocation (ctb_id);

COMMENT ON TABLE ctb_dept_allocation IS '多部門配賦テーブル（d_haifと型統一・定義書準拠）';


-- ============================================================
-- 3. 再測定履歴テーブル (ctb_remeasurement_history)
--    定義書準拠: 条件変更や見積りの見直し（新基準第34号 10ページ）
-- ============================================================

DROP TABLE IF EXISTS ctb_remeasurement_history CASCADE;

CREATE TABLE IF NOT EXISTS ctb_remeasurement_history (
    history_id          SERIAL          NOT NULL,
    ctb_id              INTEGER         NOT NULL,           -- メインへの外部キー
    measurement_date    DATE            NOT NULL,           -- 修正計上の基準日 (新基準第34号 P.10)
    reason_type         VARCHAR(20)     NULL,               -- 条件変更/期間見直し等 (新基準第34号 P.10)
    revised_liability   NUMERIC(15,0)   NULL,               -- 再算定後の負債残高 (新基準第34号 P.10)
    revised_rou_asset   NUMERIC(15,0)   NULL,               -- 再算定後の資産簿価 (新基準第34号 P.10)

    CONSTRAINT pk_ctb_remeasurement PRIMARY KEY (history_id),
    CONSTRAINT fk_remeasurement_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE
);

COMMENT ON TABLE ctb_remeasurement_history IS '再測定履歴テーブル（定義書準拠）';


-- ============================================================
-- 4. 契約基本情報テーブル (ctb_contract_header)
--    d_kykh と項目名・型を統一した新リース対応版の契約ヘッダ
--    ctb_lease_integrated の ctb_id を主キーとして参照
-- ============================================================

DROP TABLE IF EXISTS ctb_contract_header CASCADE;

CREATE TABLE IF NOT EXISTS ctb_contract_header (
    -- === PK / 結合キー ===
    ctb_id              INTEGER         NOT NULL,           -- FK → ctb_lease_integrated
    kykh_id             INTEGER         NULL,               -- FK → d_kykh（マイグレーション元）

    -- === 基本情報（d_kykh統一） ===
    kykbnj              VARCHAR(30)     NOT NULL,           -- 契約番号（自社）       ← d_kykh.kykbnj
    kykbnl              VARCHAR(30)     NULL,               -- 契約番号（リース会社） ← d_kykh.kykbnl
    kyak_nm             VARCHAR(100)    NULL,               -- 契約名               ← d_kykh.kyak_nm
    kkbn_id             SMALLINT        NULL,               -- 契約区分ID           ← d_kykh.kkbn_id → c_kkbn
    lcpt_id             INTEGER         NULL,               -- リース会社ID         ← d_kykh.lcpt_id → m_lcpt
    kknri_id            INTEGER         NULL,               -- 管理部門ID           ← d_kykh.kknri_id → m_kknri

    -- === 契約期間（d_kykh統一） ===
    kyak_dt             DATE            NULL,               -- 契約締結日           ← d_kykh.kyak_dt
    start_dt            DATE            NOT NULL,           -- リース開始日         ← d_kykh.start_dt
    end_dt              DATE            NOT NULL,           -- リース終了日         ← d_kykh.end_dt
    lkikan              SMALLINT        NULL,               -- リース期間（月）     ← d_kykh.lkikan
    free_rent_months    INTEGER         NULL DEFAULT 0,     -- 無償期間（月）       ※新リース固有

    -- === 社内管理（d_kykh統一） ===
    rng_bango           VARCHAR(30)     NULL,               -- 稟議番号             ← d_kykh.rng_bango
    shonin_dt           DATE            NULL,               -- 承認日               ← d_kykh.shonin_dt
    kiansha             VARCHAR(20)     NULL,               -- 起案者               ← d_kykh.kiansha
    jencho_f            BOOLEAN         NOT NULL DEFAULT FALSE, -- 延長フラグ       ← d_kykh.jencho_f

    -- === 金額（d_kykh統一） ===
    k_glsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 月額リース料         ← d_kykh.k_glsryo
    k_klsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 1支払額              ← d_kykh.k_klsryo
    k_slsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 総額リース料         ← d_kykh.k_slsryo
    zritu               NUMERIC(5,4)    NULL,               -- 消費税率             ← d_kykh.zritu

    -- === 状態管理（d_kykh統一） ===
    saikaisu            SMALLINT        NULL DEFAULT 0,     -- 再リース回数         ← d_kykh.saikaisu
    kyak_end_f          BOOLEAN         NOT NULL DEFAULT FALSE, -- 契約終了フラグ   ← d_kykh.kyak_end_f

    -- === システム ===
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_contract_header PRIMARY KEY (ctb_id),
    CONSTRAINT fk_contract_header_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE,
    CONSTRAINT fk_contract_header_kkbn FOREIGN KEY (kkbn_id)
        REFERENCES c_kkbn (kkbn_id),
    CONSTRAINT fk_contract_header_lcpt FOREIGN KEY (lcpt_id)
        REFERENCES m_lcpt (lcpt_id),
    CONSTRAINT fk_contract_header_kknri FOREIGN KEY (kknri_id)
        REFERENCES m_kknri (kknri_id)
);

CREATE INDEX IF NOT EXISTS idx_contract_header_kykbnj ON ctb_contract_header (kykbnj);
CREATE INDEX IF NOT EXISTS idx_contract_header_kykh ON ctb_contract_header (kykh_id);

COMMENT ON TABLE ctb_contract_header IS '契約基本情報テーブル（d_kykhと型統一）';


-- ============================================================
-- 5. 物件明細テーブル (ctb_contract_property)
--    d_kykm と項目名・型を統一した新リース対応版の物件明細
-- ============================================================

DROP TABLE IF EXISTS ctb_contract_property CASCADE;

CREATE TABLE IF NOT EXISTS ctb_contract_property (
    -- === PK / 結合キー ===
    property_id         SERIAL          NOT NULL,           -- 内部ID（d_kykm.kykm_id 相当）
    ctb_id              INTEGER         NOT NULL,           -- FK → ctb_lease_integrated
    property_no         INTEGER         NOT NULL DEFAULT 1, -- 1契約内の資産枝番（ctb_lease_integrated.property_no と対応）
    kykm_no             INTEGER         NULL,               -- 物件一覧の通し連番   ← d_kykm.kykm_no

    -- === 物件情報（d_kykm統一） ===
    bukn_bango1         VARCHAR(30)     NULL,               -- 資産番号             ← d_kykm.bukn_bango1
    bukn_nm             VARCHAR(100)    NULL,               -- 資産名               ← d_kykm.bukn_nm
    bukn_bango2         VARCHAR(30)     NULL,               -- 物件番号2            ← d_kykm.bukn_bango2
    bkind_id            INTEGER         NULL,               -- 物件種別ID           ← d_kykm.bkind_id → m_bkind
    skmk_id             INTEGER         NULL,               -- 資産区分ID           ← d_kykm.skmk_id
    b_kedaban           VARCHAR(10)     NULL,               -- 枝番                 ← d_kykm.b_kedaban
    setti_dt            DATE            NULL,               -- 設置日               ← d_kykm.setti_dt

    -- === 数量・金額（d_kykm統一） ===
    b_suuryo            INTEGER         NULL DEFAULT 1,     -- 数量                 ← d_kykm.b_suuryo
    b_knyukn            NUMERIC(15,2)   NULL DEFAULT 0,     -- 現金購入価額         ← d_kykm.b_knyukn
    b_glsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 月額リース料         ← d_kykm.b_glsryo
    b_klsryo            NUMERIC(15,2)   NULL DEFAULT 0,     -- 1支払額              ← d_kykm.b_klsryo

    -- === 配賦（d_kykm統一） ===
    b_bcat_id           INTEGER         NULL,               -- 部門ID               ← d_kykm.b_bcat_id
    b_bcat_nm           VARCHAR(40)     NULL,               -- 部門名               ← d_kykm.b_bcat_nm

    -- === 新リース固有 ===
    asset_category_cd   VARCHAR(10)     NULL,               -- 資産カテゴリ（AC01〜AC05）
    company_name        VARCHAR(100)    NULL,               -- 会社名
    install_location    VARCHAR(200)    NULL,               -- 設置場所
    remarks             VARCHAR(500)    NULL,               -- 備考

    -- === システム ===
    create_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    update_dt           TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT pk_ctb_contract_property PRIMARY KEY (property_id),
    CONSTRAINT uq_ctb_property_no UNIQUE (ctb_id, property_no),
    CONSTRAINT fk_contract_property_ctb FOREIGN KEY (ctb_id)
        REFERENCES ctb_lease_integrated (ctb_id) ON DELETE CASCADE,
    CONSTRAINT fk_contract_property_bkind FOREIGN KEY (bkind_id)
        REFERENCES m_bkind (bkind_id)
);

CREATE INDEX IF NOT EXISTS idx_contract_property_ctb ON ctb_contract_property (ctb_id);
CREATE INDEX IF NOT EXISTS idx_contract_property_bukn ON ctb_contract_property (bukn_bango1);

COMMENT ON TABLE ctb_contract_property IS '物件明細テーブル（d_kykmと型統一）';

COMMIT;
