-- ============================================================
-- 402_test_sync_lcpt.sql
-- m_lcpt <-> m_supplier 同期トリガー テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 402_test_sync_lcpt.sql
-- 前提: 401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--       402_sync_lcpt_supplier.sql が適用済み
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 準備: テスト用データをクリーン
-- ============================================================
DELETE FROM m_supplier WHERE supplier_cd IN ('TS01', 'TS02', 'TS03', 'TS04');
DELETE FROM m_lcpt WHERE lcpt1_cd IN ('TS01', 'TS02', 'TS03', 'TS04') AND history_f IS NOT TRUE;
UPDATE m_lcpt SET history_f = TRUE WHERE lcpt1_cd IN ('TS01', 'TS02', 'TS03', 'TS04');

-- ============================================================
-- テスト1: m_supplier INSERT → m_lcpt 同期
-- ============================================================
\echo '--- Test 1: m_supplier INSERT → m_lcpt sync ---'

INSERT INTO m_supplier (
    supplier_cd, supplier_nm,
    row1_contract_closing_day, row1_first_pay_months, row1_first_pay_day,
    row1_second_pay_months, row1_second_pay_day,
    re_lease_param, remarks
)
VALUES (
    'TS01', 'テスト取引先1',
    20, 1, 10, 2, 25,
    'パラメータ1', 'テスト備考'
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_lcpt
            WHERE lcpt1_cd = 'TS01' AND lcpt1_nm = 'テスト取引先1'
              AND shime_day_1 = 20 AND sshri_kn1_1 = 1 AND shri_day1_1 = 10
              AND sshri_kn2_1 = 2 AND shri_day2_1 = 25
              AND sai_denomi = 'パラメータ1' AND biko = 'テスト備考'
              AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 1 - m_supplier INSERT → m_lcpt sync'
        ELSE 'FAIL: Test 1 - m_lcpt(lcpt1_cd=TS01) not found'
    END AS result;

-- ============================================================
-- テスト2: m_lcpt INSERT → m_supplier 同期
-- ============================================================
\echo '--- Test 2: m_lcpt INSERT → m_supplier sync ---'

INSERT INTO m_lcpt (
    lcpt_id, lcpt1_cd, lcpt1_nm,
    shime_day_1, sshri_kn1_1, shri_day1_1, sshri_kn2_1, shri_day2_1,
    sai_denomi, biko, create_dt, update_dt
)
VALUES (
    (SELECT COALESCE(MAX(lcpt_id), 0) + 1 FROM m_lcpt),
    'TS02', 'テスト取引先2',
    15, 2, 20, 3, 30,
    'パラメータ2', '備考2', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_supplier
            WHERE supplier_cd = 'TS02' AND supplier_nm = 'テスト取引先2'
              AND row1_contract_closing_day = 15 AND row1_first_pay_months = 2
              AND row1_first_pay_day = 20
        )
        THEN 'PASS: Test 2 - m_lcpt INSERT → m_supplier sync'
        ELSE 'FAIL: Test 2 - m_supplier(cd=TS02) not found'
    END AS result;

-- ============================================================
-- テスト3: m_supplier UPDATE → m_lcpt 同期
-- ============================================================
\echo '--- Test 3: m_supplier UPDATE → m_lcpt sync ---'

UPDATE m_supplier SET supplier_nm = 'テスト取引先1改', row1_contract_closing_day = 25
WHERE supplier_cd = 'TS01';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_lcpt
            WHERE lcpt1_cd = 'TS01' AND lcpt1_nm = 'テスト取引先1改'
              AND shime_day_1 = 25 AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 3 - m_supplier UPDATE → m_lcpt sync'
        ELSE 'FAIL: Test 3 - m_lcpt(lcpt1_cd=TS01) not updated'
    END AS result;

-- ============================================================
-- テスト4: m_lcpt UPDATE → m_supplier 同期
-- ============================================================
\echo '--- Test 4: m_lcpt UPDATE → m_supplier sync ---'

UPDATE m_lcpt SET lcpt1_nm = 'テスト取引先2改', biko = '備考更新'
WHERE lcpt1_cd = 'TS02' AND history_f IS NOT TRUE;

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_supplier
            WHERE supplier_cd = 'TS02' AND supplier_nm = 'テスト取引先2改'
              AND remarks = '備考更新'
        )
        THEN 'PASS: Test 4 - m_lcpt UPDATE → m_supplier sync'
        ELSE 'FAIL: Test 4 - m_supplier(cd=TS02) not updated'
    END AS result;

-- ============================================================
-- テスト5: 循環防止テスト
-- ============================================================
\echo '--- Test 5: circular prevention ---'

INSERT INTO m_supplier (supplier_cd, supplier_nm)
VALUES ('TS03', 'テスト循環');

SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM m_lcpt WHERE lcpt1_cd = 'TS03' AND history_f IS NOT TRUE) = 1
         AND (SELECT COUNT(*) FROM m_supplier WHERE supplier_cd = 'TS03') = 1
        THEN 'PASS: Test 5 - No circular trigger loop'
        ELSE 'FAIL: Test 5 - Unexpected record count'
    END AS result;

-- ============================================================
-- テスト6: m_supplier DELETE → m_lcpt 論理削除
-- ============================================================
\echo '--- Test 6: m_supplier DELETE → m_lcpt soft delete ---'

DELETE FROM m_supplier WHERE supplier_cd = 'TS03';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_lcpt WHERE lcpt1_cd = 'TS03' AND history_f = TRUE
        )
        THEN 'PASS: Test 6 - m_supplier DELETE → m_lcpt soft deleted'
        ELSE 'FAIL: Test 6 - m_lcpt(lcpt1_cd=TS03) not soft deleted'
    END AS result;

-- ============================================================
-- テスト7: m_lcpt history_f=TRUE → m_supplier 削除
-- ============================================================
\echo '--- Test 7: m_lcpt logical delete → m_supplier delete ---'

UPDATE m_lcpt SET history_f = TRUE WHERE lcpt1_cd = 'TS02' AND history_f IS NOT TRUE;

SELECT
    CASE
        WHEN NOT EXISTS (SELECT 1 FROM m_supplier WHERE supplier_cd = 'TS02')
        THEN 'PASS: Test 7 - m_lcpt logical delete → m_supplier deleted'
        ELSE 'FAIL: Test 7 - m_supplier(cd=TS02) still exists'
    END AS result;

-- ============================================================
-- テスト8: 支払条件3行マッピングテスト（row1/row2/row3）
-- ============================================================
\echo '--- Test 8: payment terms 3-row mapping ---'

INSERT INTO m_supplier (
    supplier_cd, supplier_nm,
    row1_contract_closing_day, row1_first_pay_months, row1_first_pay_day,
    row1_second_pay_months, row1_second_pay_day,
    row2_contract_closing_day, row2_first_pay_months, row2_first_pay_day,
    row2_second_pay_months, row2_second_pay_day,
    row3_contract_closing_day, row3_first_pay_months, row3_first_pay_day,
    row3_second_pay_months, row3_second_pay_day
)
VALUES (
    'TS04', 'テスト3行取引先',
    20, 1, 10, 2, 25,
    15, 2, 20, 3, 5,
    10, 1, 15, 2, 28
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_lcpt
            WHERE lcpt1_cd = 'TS04' AND lcpt1_nm = 'テスト3行取引先'
              -- row1
              AND shime_day_1 = 20 AND sshri_kn1_1 = 1 AND shri_day1_1 = 10
              AND sshri_kn2_1 = 2 AND shri_day2_1 = 25
              -- row2
              AND shime_day_2 = 15 AND sshri_kn1_2 = 2 AND shri_day1_2 = 20
              AND sshri_kn2_2 = 3 AND shri_day2_2 = 5
              -- row3
              AND shime_day_3 = 10 AND sshri_kn1_3 = 1 AND shri_day1_3 = 15
              AND sshri_kn2_3 = 2 AND shri_day2_3 = 28
              AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 8 - 3-row payment terms mapped correctly'
        ELSE 'FAIL: Test 8 - payment terms row2/row3 not mapped'
    END AS result;

-- ============================================================
-- クリーンアップ
-- ============================================================
DELETE FROM m_supplier WHERE supplier_cd IN ('TS01', 'TS04');
DELETE FROM m_lcpt WHERE lcpt1_cd IN ('TS01', 'TS02', 'TS03', 'TS04');

\echo '--- All tests completed ---'

COMMIT;
