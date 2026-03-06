-- ============================================================
-- トランザクション・CTBテーブル DDL
-- 対象DB: PostgreSQL
-- ※ 実行前に master_tables.sql を先に実行すること
--   (m_asset_category, m_contract_mgmt_unit, m_property_type,
--    m_supplier, m_vendor, m_payment_method 等を参照するため)
-- ============================================================

-- ============================================================
-- CTB (共通基盤テーブル) 新リース会計基準対応 (ASBJ 34/33)
-- 定義書: 共通基盤テーブル(CTB)詳細定義書_20260304
-- ============================================================

-- 1. 新基準統合メインテーブル (ctb_lease_integrated)
-- M4BS, M7, JSM10 の全要素を統合。新基準計算エンジンが参照する基幹テーブル。
CREATE TABLE IF NOT EXISTS ctb_lease_integrated (
    ctb_id                      SERIAL          PRIMARY KEY,
    -- 契約識別
    contract_no                 VARCHAR(15)     NOT NULL,   -- M4BS P.21: リース契約の管理番号
    property_no                 INTEGER,                    -- M4BS P.21: 1契約内の物件枝番
    m7_asset_no                 VARCHAR(15),               -- M7 P.38:   固定資産管理上の番号
    jsm10_aro_no                VARCHAR(15),               -- JSM10 P.2: ARO管理番号
    -- リース期間 (新基準第34号 P.6)
    lease_start_date            DATE            NOT NULL,   -- M4BS P.21: 貸手が借手に資産を提供した日
    non_cancellable_months      INTEGER         NOT NULL,   -- M4BS P.23: 契約上の解約不能月数
    is_extension_certain        BOOLEAN,                   -- 延長行使が合理的に確実か
    extension_months            INTEGER,                   -- 延長オプションの対象期間
    accounting_lease_term       INTEGER         NOT NULL,   -- 償却期間となる合計月数
    -- 支払条件 (M4BS P.22-23)
    periodic_payment_amt        NUMERIC(15,0)   NOT NULL,   -- 1支払額(税抜)
    payment_interval_months     INTEGER         NOT NULL,   -- 支払間隔(1/12ヶ月等)
    discount_rate               NUMERIC(5,4)    NOT NULL,   -- 追加借入利子率(NPV算出用割引率)
    -- オプション・保証
    residual_value_guarantee    NUMERIC(15,0),             -- 新基準第34号 P.7: リース終了時の保証額
    purchase_option_amt         NUMERIC(15,0),             -- 新基準第34号 P.9: 購入選択権価額(行使確実時)
    aro_present_value           NUMERIC(15,0),             -- JSM10 P.2: 除去債務現在価値(使用権資産加算額)
    -- 部署・科目 (M7連携)
    m7_dept_cd                  VARCHAR(12),               -- M7 P.52: 資産管理責任部門
    burden_dept_cd              VARCHAR(12),               -- M7 P.52: 費用計上先部門
    asset_class_cd              VARCHAR(12),               -- M7 P.50: 使用権資産の勘定科目
    segment_cd                  VARCHAR(12),               -- M7 P.12: 会計分析用区分
    -- 算定結果 (新基準第34号 P.8)
    initial_rou_asset           NUMERIC(15,0),             -- 使用権資産当初額(NPV＋除去費用等)
    initial_lease_liability     NUMERIC(15,0),             -- リース負債当初額(リース料現在価値合計)
    -- タイムスタンプ
    create_dt                   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt                   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    -- contract_no + property_no の組み合わせで一意
    UNIQUE (contract_no, property_no)
);


-- 2. 多部門配賦テーブル (ctb_dept_allocation)
-- シサンM7の配賦ロジック(P.70)を継承。1物件の費用を複数部門へ按分する。
CREATE TABLE IF NOT EXISTS ctb_dept_allocation (
    allocation_id    SERIAL          PRIMARY KEY,
    ctb_id           INTEGER         NOT NULL REFERENCES ctb_lease_integrated(ctb_id) ON DELETE CASCADE,
    dept_cd          VARCHAR(12)     NOT NULL,   -- M7 P.70: 費用負担する部署コード
    allocation_ratio NUMERIC(5,2)    NOT NULL    -- M7 P.70: 各部門への按分比率(%)
);


-- 3. 再測定履歴テーブル (ctb_remeasurement_history)
-- 条件変更や見積りの見直し（新基準第34号 P.10）に伴う履歴を保持。
CREATE TABLE IF NOT EXISTS ctb_remeasurement_history (
    history_id          SERIAL          PRIMARY KEY,
    ctb_id              INTEGER         NOT NULL REFERENCES ctb_lease_integrated(ctb_id) ON DELETE CASCADE,
    measurement_date    DATE            NOT NULL,   -- 新基準第34号 P.10: 修正計上の基準日
    reason_type         VARCHAR(20),               -- 条件変更/期間見直し等
    revised_liability   NUMERIC(15,0),             -- 再算定後の負債残高
    revised_rou_asset   NUMERIC(15,0)              -- 再算定後の資産簿価
);


-- スキーマ権限付与
GRANT ALL ON SCHEMA public TO lease_m4bs_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO lease_m4bs_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO lease_m4bs_user;
