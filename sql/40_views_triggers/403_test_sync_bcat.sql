-- ============================================================
-- 403_test_sync_bcat.sql
-- m_bcat <-> m_department 同期トリガー テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 403_test_sync_bcat.sql
-- 前提: 401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--       403_sync_bcat_department.sql が適用済み
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 準備: テスト用データをクリーン
-- ============================================================
DELETE FROM m_department WHERE dept_cd IN ('TD01', 'TD02', 'TD03');
DELETE FROM m_bcat WHERE bcat1_cd IN ('TD01', 'TD02', 'TD03') AND history_f IS NOT TRUE;
UPDATE m_bcat SET history_f = TRUE WHERE bcat1_cd IN ('TD01', 'TD02', 'TD03');

-- ============================================================
-- テスト1: m_department INSERT → m_bcat 同期
-- ============================================================
\echo '--- Test 1: m_department INSERT → m_bcat sync ---'

INSERT INTO m_department (
    dept_cd, dept_nm,
    dept_cd2, dept_nm2, dept_cd3, dept_nm3,
    dept_cd4, dept_nm4, dept_cd5, dept_nm5,
    cost_category_nm,
    agg_category1_cd, agg_category1_nm,
    agg_category2_cd, agg_category2_nm,
    agg_category3_cd, agg_category3_nm,
    remarks
)
VALUES (
    'TD01', 'テスト部門1',
    'TD01B', 'テスト部門1B', 'TD01C', 'テスト部門1C',
    'TD01D', 'テスト部門1D', 'TD01E', 'テスト部門1E',
    '原価区分1',
    'AG1', '集計1', 'AG2', '集計2', 'AG3', '集計3',
    'テスト備考'
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_bcat
            WHERE bcat1_cd = 'TD01' AND bcat1_nm = 'テスト部門1'
              AND bcat2_cd = 'TD01B' AND bcat3_cd = 'TD01C'
              AND bcat4_cd = 'TD01D' AND bcat5_cd = 'TD01E'
              AND sum1_cd = 'AG1' AND sum1_nm = '集計1'
              AND sum2_cd = 'AG2' AND sum3_cd = 'AG3'
              AND biko = 'テスト備考'
              AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 1 - m_department INSERT → m_bcat sync'
        ELSE 'FAIL: Test 1 - m_bcat(bcat1_cd=TD01) not found'
    END AS result;

-- ============================================================
-- テスト2: m_bcat INSERT → m_department 同期
-- ============================================================
\echo '--- Test 2: m_bcat INSERT → m_department sync ---'

INSERT INTO m_bcat (
    bcat_id, bcat1_cd, bcat1_nm,
    bcat2_cd, bcat2_nm, bcat3_cd, bcat3_nm,
    bcat4_cd, bcat4_nm, bcat5_cd, bcat5_nm,
    sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
    biko, create_dt, update_dt
)
VALUES (
    (SELECT COALESCE(MAX(bcat_id), 0) + 1 FROM m_bcat),
    'TD02', 'テスト部門2',
    'TD02B', 'テスト部門2B', 'TD02C', 'テスト部門2C',
    'TD02D', 'テスト部門2D', 'TD02E', 'テスト部門2E',
    'BG1', '集計B1', 'BG2', '集計B2', 'BG3', '集計B3',
    '備考2', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_department
            WHERE dept_cd = 'TD02' AND dept_nm = 'テスト部門2'
              AND dept_cd2 = 'TD02B' AND dept_cd3 = 'TD02C'
              AND agg_category1_cd = 'BG1' AND agg_category1_nm = '集計B1'
        )
        THEN 'PASS: Test 2 - m_bcat INSERT → m_department sync'
        ELSE 'FAIL: Test 2 - m_department(cd=TD02) not found'
    END AS result;

-- ============================================================
-- テスト3: m_department UPDATE → m_bcat 同期
-- ============================================================
\echo '--- Test 3: m_department UPDATE → m_bcat sync ---'

UPDATE m_department SET dept_nm = 'テスト部門1改', agg_category1_nm = '集計1改'
WHERE dept_cd = 'TD01';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_bcat
            WHERE bcat1_cd = 'TD01' AND bcat1_nm = 'テスト部門1改'
              AND sum1_nm = '集計1改' AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 3 - m_department UPDATE → m_bcat sync'
        ELSE 'FAIL: Test 3 - m_bcat(bcat1_cd=TD01) not updated'
    END AS result;

-- ============================================================
-- テスト4: m_bcat UPDATE → m_department 同期
-- ============================================================
\echo '--- Test 4: m_bcat UPDATE → m_department sync ---'

UPDATE m_bcat SET bcat1_nm = 'テスト部門2改', biko = '備考更新'
WHERE bcat1_cd = 'TD02' AND history_f IS NOT TRUE;

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_department
            WHERE dept_cd = 'TD02' AND dept_nm = 'テスト部門2改'
              AND remarks = '備考更新'
        )
        THEN 'PASS: Test 4 - m_bcat UPDATE → m_department sync'
        ELSE 'FAIL: Test 4 - m_department(cd=TD02) not updated'
    END AS result;

-- ============================================================
-- テスト5: 循環防止テスト
-- ============================================================
\echo '--- Test 5: circular prevention ---'

INSERT INTO m_department (dept_cd, dept_nm)
VALUES ('TD03', 'テスト循環');

SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM m_bcat WHERE bcat1_cd = 'TD03' AND history_f IS NOT TRUE) = 1
         AND (SELECT COUNT(*) FROM m_department WHERE dept_cd = 'TD03') = 1
        THEN 'PASS: Test 5 - No circular trigger loop'
        ELSE 'FAIL: Test 5 - Unexpected record count'
    END AS result;

-- ============================================================
-- テスト6: m_department DELETE → m_bcat 論理削除
-- ============================================================
\echo '--- Test 6: m_department DELETE → m_bcat soft delete ---'

DELETE FROM m_department WHERE dept_cd = 'TD03';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_bcat WHERE bcat1_cd = 'TD03' AND history_f = TRUE
        )
        THEN 'PASS: Test 6 - m_department DELETE → m_bcat soft deleted'
        ELSE 'FAIL: Test 6 - m_bcat(bcat1_cd=TD03) not soft deleted'
    END AS result;

-- ============================================================
-- テスト7: v_unified_department ビュー参照テスト
-- ============================================================
\echo '--- Test 7: v_unified_department view test ---'

-- TD01はまだ存在するので、ビューから参照できるか確認
SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM v_unified_department
            WHERE dept_cd = 'TD01'
        )
        THEN 'PASS: Test 7 - v_unified_department view returns TD01'
        ELSE 'FAIL: Test 7 - TD01 not found in v_unified_department'
    END AS result;

-- ============================================================
-- クリーンアップ
-- ============================================================
DELETE FROM m_department WHERE dept_cd IN ('TD01', 'TD02');
DELETE FROM m_bcat WHERE bcat1_cd IN ('TD01', 'TD02', 'TD03');

\echo '--- All tests completed ---'

COMMIT;
