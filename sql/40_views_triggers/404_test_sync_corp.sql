-- ============================================================
-- 404_test_sync_corp.sql
-- m_corp <-> m_company 同期トリガー テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 404_test_sync_corp.sql
-- 前提: 401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--       404_sync_corp_company.sql が適用済み
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 準備: テスト用データをクリーン
-- ============================================================
DELETE FROM m_company WHERE company_cd IN ('T01', 'T02', 'T03', 'T04');
DELETE FROM m_corp WHERE corp1_cd IN ('T01', 'T02', 'T03', 'T04') AND history_f IS NOT TRUE;
UPDATE m_corp SET history_f = TRUE WHERE corp1_cd IN ('T01', 'T02', 'T03', 'T04');

-- ============================================================
-- テスト1: m_company INSERT → m_corp 同期
-- ============================================================
\echo '--- Test 1: m_company INSERT → m_corp sync ---'

INSERT INTO m_company (company_cd, company_nm, company_cd2, company_nm2, company_cd3, company_nm3, remarks)
VALUES ('T01', 'テスト法人1', 'T01B', 'テスト法人1B', 'T01C', 'テスト法人1C', 'テスト備考');

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_corp
            WHERE corp1_cd = 'T01' AND corp1_nm = 'テスト法人1'
              AND corp2_cd = 'T01B' AND corp3_cd = 'T01C'
              AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 1 - m_company INSERT → m_corp sync'
        ELSE 'FAIL: Test 1 - m_corp(corp1_cd=T01) not found'
    END AS result;

-- ============================================================
-- テスト2: m_corp INSERT → m_company 同期
-- ============================================================
\echo '--- Test 2: m_corp INSERT → m_company sync ---'

INSERT INTO m_corp (corp_id, corp1_cd, corp1_nm, corp2_cd, corp2_nm, create_dt, update_dt)
VALUES (
    (SELECT COALESCE(MAX(corp_id), 0) + 1 FROM m_corp),
    'T02', 'テスト法人2', 'T02B', 'テスト法人2B', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_company
            WHERE company_cd = 'T02' AND company_nm = 'テスト法人2'
              AND company_cd2 = 'T02B'
        )
        THEN 'PASS: Test 2 - m_corp INSERT → m_company sync'
        ELSE 'FAIL: Test 2 - m_company(cd=T02) not found'
    END AS result;

-- ============================================================
-- テスト3: m_company UPDATE → m_corp 同期
-- ============================================================
\echo '--- Test 3: m_company UPDATE → m_corp sync ---'

UPDATE m_company SET company_nm = 'テスト法人1改', remarks = '更新済み'
WHERE company_cd = 'T01';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_corp
            WHERE corp1_cd = 'T01' AND corp1_nm = 'テスト法人1改'
              AND biko = '更新済み' AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 3 - m_company UPDATE → m_corp sync'
        ELSE 'FAIL: Test 3 - m_corp(corp1_cd=T01) not updated'
    END AS result;

-- ============================================================
-- テスト4: m_corp UPDATE → m_company 同期
-- ============================================================
\echo '--- Test 4: m_corp UPDATE → m_company sync ---'

UPDATE m_corp SET corp1_nm = 'テスト法人2改', biko = '備考更新'
WHERE corp1_cd = 'T02' AND history_f IS NOT TRUE;

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_company
            WHERE company_cd = 'T02' AND company_nm = 'テスト法人2改'
              AND remarks = '備考更新'
        )
        THEN 'PASS: Test 4 - m_corp UPDATE → m_company sync'
        ELSE 'FAIL: Test 4 - m_company(cd=T02) not updated'
    END AS result;

-- ============================================================
-- テスト5: 循環防止テスト
-- ============================================================
\echo '--- Test 5: circular prevention ---'

INSERT INTO m_company (company_cd, company_nm)
VALUES ('T03', 'テスト循環');

SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM m_corp WHERE corp1_cd = 'T03' AND history_f IS NOT TRUE) = 1
         AND (SELECT COUNT(*) FROM m_company WHERE company_cd = 'T03') = 1
        THEN 'PASS: Test 5 - No circular trigger loop'
        ELSE 'FAIL: Test 5 - Unexpected record count'
    END AS result;

-- ============================================================
-- テスト6: m_company DELETE → m_corp 論理削除
-- ============================================================
\echo '--- Test 6: m_company DELETE → m_corp soft delete ---'

DELETE FROM m_company WHERE company_cd = 'T03';

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_corp WHERE corp1_cd = 'T03' AND history_f = TRUE
        )
        THEN 'PASS: Test 6 - m_company DELETE → m_corp soft deleted'
        ELSE 'FAIL: Test 6 - m_corp(corp1_cd=T03) not soft deleted'
    END AS result;

-- ============================================================
-- テスト7: m_corp history_f=TRUE → m_company 削除
-- ============================================================
\echo '--- Test 7: m_corp logical delete → m_company delete ---'

-- T02をm_corp側で論理削除
UPDATE m_corp SET history_f = TRUE WHERE corp1_cd = 'T02' AND history_f IS NOT TRUE;

SELECT
    CASE
        WHEN NOT EXISTS (SELECT 1 FROM m_company WHERE company_cd = 'T02')
        THEN 'PASS: Test 7 - m_corp logical delete → m_company deleted'
        ELSE 'FAIL: Test 7 - m_company(cd=T02) still exists'
    END AS result;

-- ============================================================
-- テスト8: varchar長の切り詰めテスト
-- ============================================================
\echo '--- Test 8: varchar truncation ---'

INSERT INTO m_company (company_cd, company_nm)
VALUES ('T04', '12345678901234567890123456789012345678901234567890あいうえおかきくけこ');

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM m_corp
            WHERE corp1_cd = 'T04' AND LENGTH(corp1_nm) <= 40
              AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 8 - VARCHAR truncation works (company_nm→corp1_nm)'
        ELSE 'FAIL: Test 8 - truncation failed'
    END AS result;

-- ============================================================
-- クリーンアップ
-- ============================================================
DELETE FROM m_company WHERE company_cd IN ('T01', 'T04');
DELETE FROM m_corp WHERE corp1_cd IN ('T01', 'T02', 'T03', 'T04');

\echo '--- All tests completed ---'

COMMIT;
