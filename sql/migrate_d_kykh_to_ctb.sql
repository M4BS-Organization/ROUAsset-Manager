-- ============================================================
-- データマイグレーション: d_kykh → ctb_lease_integrated
-- 旧版契約ヘッダデータを新版CTBテーブルへ移行
-- ============================================================
-- 前提: seed_data.sql で以下のマスタが投入済みであること
--   m_department:     001=本社, 002=大阪支社, 003=名古屋支社, 004=福岡支社
--   m_contract_type:  01=FA, 02=OL, 03=割賦購入, 04=レンタル
--   m_supplier:       LC001=オリックス, LC002=三井住友FAL, LC003=東京センチュリー, LC004=みずほリース, LC005=NTTファイナンス
-- ============================================================

BEGIN;

-- ============================================================
-- 1. d_kykh → ctb_lease_integrated
-- ============================================================
-- マッピング:
--   kknri_id  → m_kknri.kknri1_cd → m_department.dept_cd
--   kkbn_id   → LPAD(kkbn_id, 2, '0') → m_contract_type.contract_type_cd
--   lcpt_id   → m_lcpt.lcpt1_cd → m_supplier.supplier_cd

INSERT INTO ctb_lease_integrated (
    contract_no,
    property_no,
    contract_name,
    contract_type_cd,
    supplier_cd,
    mgmt_dept_cd,
    lease_start_date,
    lease_end_date,
    free_rent_months,
    lease_term_months,
    asset_name,
    monthly_payment,
    total_payment,
    remarks
)
SELECT
    k.kykbnj,                                       -- contract_no ← 自社契約番号
    1,                                               -- property_no (デフォルト)
    k.kyak_nm,                                       -- contract_name ← 契約名称
    LPAD(CAST(k.kkbn_id AS VARCHAR), 2, '0'),       -- contract_type_cd ← kkbn_id → '01','02','03','04'
    lc.lcpt1_cd,                                     -- supplier_cd ← m_lcpt.lcpt1_cd
    kn.kknri1_cd,                                    -- mgmt_dept_cd ← m_kknri.kknri1_cd
    CAST(k.start_dt AS DATE),                        -- lease_start_date
    CAST(k.end_dt AS DATE),                          -- lease_end_date
    0,                                               -- free_rent_months (旧版に該当なし)
    CAST(k.lkikan AS INTEGER),                       -- lease_term_months
    k.kyak_nm,                                       -- asset_name ← 契約名で仮設定
    CAST(k.k_glsryo AS NUMERIC(15,2)),              -- monthly_payment
    CAST(k.k_slsryo AS NUMERIC(15,2)),              -- total_payment
    CONCAT_WS(' | ',
        '相手方番号:' || k.kykbnl,
        '稟議番号:' || k.rng_bango
    )                                                -- remarks ← 相手方契約番号 + 稟議番号
FROM d_kykh k
LEFT JOIN m_kknri kn ON k.kknri_id = kn.kknri_id
LEFT JOIN m_lcpt lc ON k.lcpt_id = lc.lcpt_id
WHERE k.k_history_f IS NOT TRUE                      -- 履歴レコード除外
ON CONFLICT (contract_no, property_no) DO NOTHING;   -- 重複時はスキップ

-- ============================================================
-- 2. ctb_dept_allocation（配賦部門：100%で1レコード）
-- ============================================================

INSERT INTO ctb_dept_allocation (
    ctb_id,
    dept_cd,
    allocation_ratio,
    payment_amount
)
SELECT
    c.ctb_id,
    c.mgmt_dept_cd,                                  -- 管理部署 = 配賦部門
    100.00,                                          -- 100%配賦
    c.monthly_payment                                -- 月額支払額
FROM ctb_lease_integrated c
WHERE c.mgmt_dept_cd IS NOT NULL
  AND NOT EXISTS (
    SELECT 1 FROM ctb_dept_allocation d
    WHERE d.ctb_id = c.ctb_id AND d.dept_cd = c.mgmt_dept_cd
  );

COMMIT;

-- ============================================================
-- 確認クエリ
-- ============================================================
-- SELECT
--     c.ctb_id, c.contract_no, c.contract_name,
--     ct.contract_type_nm, s.supplier_nm, md.dept_nm,
--     c.lease_start_date, c.lease_end_date, c.lease_term_months,
--     c.monthly_payment, c.total_payment
-- FROM ctb_lease_integrated c
-- LEFT JOIN m_contract_type ct ON c.contract_type_cd = ct.contract_type_cd
-- LEFT JOIN m_supplier s ON c.supplier_cd = s.supplier_cd
-- LEFT JOIN m_department md ON c.mgmt_dept_cd = md.dept_cd
-- ORDER BY c.contract_no;
