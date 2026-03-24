-- ============================================================
-- 501_test_sp_migrate.sql
-- sp_migrate_d_kykh_to_ctb テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 501_test_sp_migrate.sql
-- 前提: 501_sp_migrate_to_ctb.sql が適用済み
--       テストデータ（90_testdata/902_testdata_d_kykh.sql）投入済み
-- ============================================================

SET search_path TO public;

-- ============================================================
-- テスト1: 全件マイグレーション実行
-- ============================================================
\echo '--- Test 1: Full migration ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb();

-- ============================================================
-- テスト2: 結果確認
-- ============================================================
\echo '--- Test 2: Verify ctb_lease_integrated records ---'

SELECT
    c.ctb_id,
    c.contract_no,
    c.contract_name,
    c.contract_type_cd,
    c.supplier_cd,
    c.mgmt_dept_cd,
    c.lease_start_date,
    c.monthly_payment
FROM ctb_lease_integrated c
WHERE c.contract_no IN (SELECT kykbnj FROM d_kykh WHERE k_history_f IS NOT TRUE)
ORDER BY c.contract_no;

-- ============================================================
-- テスト3: ctb_property 自動作成確認
-- ============================================================
\echo '--- Test 3: Verify ctb_property auto-creation ---'

SELECT
    p.property_id,
    p.ctb_id,
    p.property_no,
    p.asset_category_cd,
    p.asset_name,
    p.remarks
FROM ctb_property p
WHERE p.remarks = 'マイグレーションにより自動作成'
ORDER BY p.ctb_id;

-- ============================================================
-- テスト4: ctb_dept_allocation 確認
-- ============================================================
\echo '--- Test 4: Verify ctb_dept_allocation ---'

SELECT
    d.allocation_id,
    d.ctb_id,
    d.dept_cd,
    d.allocation_ratio,
    d.payment_amount
FROM ctb_dept_allocation d
ORDER BY d.ctb_id;

-- ============================================================
-- テスト5: 差分同期（再実行でUPDATE）
-- ============================================================
\echo '--- Test 5: Re-run should UPDATE existing records ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb();

-- updated_count > 0, inserted_count = 0 であること

-- ============================================================
-- テスト6: 特定契約のみマイグレーション
-- ============================================================
\echo '--- Test 6: Single contract migration ---'

SELECT * FROM sp_migrate_d_kykh_to_ctb(
    (SELECT kykbnj FROM d_kykh WHERE k_history_f IS NOT TRUE LIMIT 1)
);

\echo '--- All tests completed ---'
