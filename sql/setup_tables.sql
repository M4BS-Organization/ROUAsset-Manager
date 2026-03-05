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
-- ※ d_asset より先に定義すること（d_asset が FK 参照するため）
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

-- ============================================================
-- 資産・契約トランザクションテーブル
-- 旧テーブル (tw_m_user, 旧 d_asset) を廃止し、
-- マスタFK参照型に再設計
-- ============================================================

-- d_asset: 資産・契約管理テーブル
-- 旧 free-text カラム → マスタFK参照カラムへの対応:
--   旧 account_class  → asset_category_cd  (m_asset_category)
--   旧 mgmt_unit      → contract_mgmt_unit_cd (m_contract_mgmt_unit)
--   旧 contract_type  → property_type_cd   (m_property_type)
--   旧 payee          → supplier_cd        (m_supplier)
--   旧 kashushi_nm    → kashushi_vendor_cd (m_vendor: 貸主)
--   旧 chukai_nm      → chukai_vendor_cd   (m_vendor: 仲介業者)
--   旧 kessai_nm      → payment_method_cd  (m_payment_method: 決済方法)
--   旧 hosho_nm       → hosho_vendor_cd    (m_vendor: 保証会社)
CREATE TABLE IF NOT EXISTS d_asset (
    asset_id                SERIAL          PRIMARY KEY,
    -- CTB連携FK (NULL=CTB未連携、値設定=新基準計算エンジンと連携済み)
    ctb_id                  INTEGER         REFERENCES ctb_lease_integrated(ctb_id) ON DELETE SET NULL,
    -- マスタFK参照カラム
    asset_category_cd       VARCHAR(10)     REFERENCES m_asset_category(asset_category_cd),       -- 資産区分
    contract_mgmt_unit_cd   VARCHAR(10)     REFERENCES m_contract_mgmt_unit(contract_mgmt_unit_cd), -- 契約管理単位
    property_type_cd        VARCHAR(10)     REFERENCES m_property_type(property_type_cd),          -- 物件種別/契約種別
    supplier_cd             VARCHAR(10)     REFERENCES m_supplier(supplier_cd),                    -- 支払先
    kashushi_vendor_cd      VARCHAR(10)     REFERENCES m_vendor(vendor_cd),                        -- 貸主
    chukai_vendor_cd        VARCHAR(10)     REFERENCES m_vendor(vendor_cd),                        -- 仲介業者
    payment_method_cd       VARCHAR(10)     REFERENCES m_payment_method(payment_method_cd),        -- 決済方法
    hosho_vendor_cd         VARCHAR(10)     REFERENCES m_vendor(vendor_cd),                        -- 保証会社
    -- 資産基本情報 (直接入力)
    asset_no                VARCHAR(50),
    quantity                INTEGER         DEFAULT 1,
    bukken_nm               VARCHAR(200),
    shozaichi               VARCHAR(500),
    kukaku                  VARCHAR(100),
    menseki                 VARCHAR(50),
    madori                  VARCHAR(100),
    kozo_yoto               VARCHAR(200),
    yoto_seigen             VARCHAR(500),
    taiyo_nensu             INTEGER,
    shunko_dt               DATE,
    -- 契約関連情報 (直接入力)
    acct_target             VARCHAR(50),
    contract_no             VARCHAR(50),
    own_mgmt                VARCHAR(50),
    approval_no             VARCHAR(50),
    re_lease_cnt            INTEGER         DEFAULT 0,
    contract_nm             VARCHAR(200),
    start_dt                DATE,
    end_dt                  DATE,
    contract_period         INTEGER,
    cash_price              DECIMAL(18,2),
    monthly_lease           DECIMAL(18,2),
    consistency             VARCHAR(10),
    -- 更新者情報
    upd_user_id             VARCHAR(50),
    upd_user_nm             VARCHAR(100),
    -- タイムスタンプ
    create_dt               TIMESTAMP       DEFAULT CURRENT_TIMESTAMP,
    update_dt               TIMESTAMP       DEFAULT CURRENT_TIMESTAMP
);

-- d_asset_equipment: 資産付属設備テーブル
CREATE TABLE IF NOT EXISTS d_asset_equipment (
    eq_id       SERIAL PRIMARY KEY,
    asset_id    INTEGER NOT NULL REFERENCES d_asset(asset_id) ON DELETE CASCADE,
    eq_name     VARCHAR(200),
    eq_amount   VARCHAR(50),
    eq_date     VARCHAR(50)
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
