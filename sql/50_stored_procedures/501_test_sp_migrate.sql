-- ============================================================
-- 501_test_sp_migrate.sql
-- sp_migrate_d_kykh_to_ctb テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 501_test_sp_migrate.sql
-- 前提: 501_sp_migrate_to_ctb.sql が適用済み
--       テストデータ（90_testdata/902_testdata_d_kykh.sql, 906_testdata_d_kykm.sql）投入済み
-- ============================================================

SET search_path TO public;

-- ============================================================
-- テスト1: 全件マイグレーション実行
-- ============================================================
\echo '--- Test 1: Full migration ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb();

-- ============================================================
-- テスト2: ctb_lease_integrated 結果確認
-- ============================================================
\echo '--- Test 2: Verify ctb_lease_integrated records ---'

SELECT
    c.ctb_id,
    c.contract_no,
    c.property_no,
    c.lease_start_date,
    c.periodic_payment_amt,
    c.kykh_id
FROM ctb_lease_integrated c
ORDER BY c.contract_no, c.property_no;

-- ============================================================
-- テスト3: ctb_contract_property 自動作成確認
-- ============================================================
\echo '--- Test 3: Verify ctb_contract_property ---'

SELECT
    p.property_id,
    p.ctb_id,
    p.property_no,
    p.bukn_bango1,
    p.bukn_nm,
    p.asset_category_cd,
    bk.bkind_nm
FROM ctb_contract_property p
LEFT JOIN m_bkind bk ON p.bkind_id = bk.bkind_id
ORDER BY p.ctb_id, p.property_no;

-- ============================================================
-- テスト4: ctb_contract_header 確認
-- ============================================================
\echo '--- Test 4: Verify ctb_contract_header ---'

SELECT
    h.ctb_id,
    h.kykbnj,
    h.kyak_nm,
    h.start_dt,
    h.end_dt,
    h.lkikan
FROM ctb_contract_header h
ORDER BY h.ctb_id;

-- ============================================================
-- テスト5: ctb_dept_allocation 確認
-- ============================================================
\echo '--- Test 5: Verify ctb_dept_allocation ---'

SELECT
    a.ctb_id,
    c.property_no,
    a.dept_cd,
    a.haifritu,
    d.dept_nm
FROM ctb_dept_allocation a
JOIN ctb_lease_integrated c ON a.ctb_id = c.ctb_id
LEFT JOIN m_department d ON a.dept_cd = d.dept_cd
ORDER BY a.ctb_id, a.haifritu DESC;

-- ============================================================
-- テスト6: 差分同期（再実行でUPDATE）
-- ============================================================
\echo '--- Test 6: Re-run should UPDATE existing records ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb();

-- updated_count > 0, inserted_count = 0 であること

-- ============================================================
-- テスト7: 特定契約のみマイグレーション
-- ============================================================
\echo '--- Test 7: Single contract migration ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb(
    (SELECT kykbnj FROM d_kykh WHERE k_history_f IS NOT TRUE LIMIT 1)
);

\echo '--- All tests completed ---'
