-- ============================================================
-- 401_test_sync_kkbn.sql
-- c_kkbn <-> m_contract_type 同期トリガー テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 401_test_sync_kkbn.sql
-- 前提: 401_sync_kkbn_contract_type.sql が適用済みであること
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 準備: テスト用データをクリーン
-- ============================================================
DELETE FROM m_contract_type WHERE contract_type_cd IN ('90', '91', '92');
DELETE FROM c_kkbn WHERE kkbn_id IN (90, 91, 92);

-- ============================================================
-- テスト1: m_contract_type INSERT → c_kkbn 同期
-- ============================================================
\echo '--- Test 1: m_contract_type INSERT → c_kkbn sync ---'

INSERT INTO m_contract_type (contract_type_cd, contract_type_nm)
VALUES ('90', 'テスト契約区分90');

-- 検証: c_kkbnに kkbn_id=90 が作成されているか
SELECT
    CASE
        WHEN EXISTS (SELECT 1 FROM c_kkbn WHERE kkbn_id = 90 AND kkbn_nm = 'テスト契約区分90')
        THEN 'PASS: Test 1 - m_contract_type INSERT → c_kkbn sync'
        ELSE 'FAIL: Test 1 - c_kkbn(kkbn_id=90) not found'
    END AS result;

-- ============================================================
-- テスト2: c_kkbn INSERT → m_contract_type 同期
-- ============================================================
\echo '--- Test 2: c_kkbn INSERT → m_contract_type sync ---'

INSERT INTO c_kkbn (kkbn_id, kkbn_nm)
VALUES (91, 'テスト区分91');

-- 検証: m_contract_typeに contract_type_cd='91' が作成されているか
SELECT
    CASE
        WHEN EXISTS (SELECT 1 FROM m_contract_type WHERE contract_type_cd = '91' AND contract_type_nm = 'テスト区分91')
        THEN 'PASS: Test 2 - c_kkbn INSERT → m_contract_type sync'
        ELSE 'FAIL: Test 2 - m_contract_type(cd=91) not found'
    END AS result;

-- ============================================================
-- テスト3: m_contract_type UPDATE → c_kkbn 同期
-- ============================================================
\echo '--- Test 3: m_contract_type UPDATE → c_kkbn sync ---'

UPDATE m_contract_type SET contract_type_nm = 'テスト契約区分90改' WHERE contract_type_cd = '90';

SELECT
    CASE
        WHEN EXISTS (SELECT 1 FROM c_kkbn WHERE kkbn_id = 90 AND kkbn_nm = 'テスト契約区分90改')
        THEN 'PASS: Test 3 - m_contract_type UPDATE → c_kkbn sync'
        ELSE 'FAIL: Test 3 - c_kkbn(kkbn_id=90) name not updated'
    END AS result;

-- ============================================================
-- テスト4: c_kkbn UPDATE → m_contract_type 同期
-- ============================================================
\echo '--- Test 4: c_kkbn UPDATE → m_contract_type sync ---'

UPDATE c_kkbn SET kkbn_nm = 'テスト区分91改' WHERE kkbn_id = 91;

SELECT
    CASE
        WHEN EXISTS (SELECT 1 FROM m_contract_type WHERE contract_type_cd = '91' AND contract_type_nm = 'テスト区分91改')
        THEN 'PASS: Test 4 - c_kkbn UPDATE → m_contract_type sync'
        ELSE 'FAIL: Test 4 - m_contract_type(cd=91) name not updated'
    END AS result;

-- ============================================================
-- テスト5: 循環防止テスト
-- 双方向トリガーが無限ループしないことを確認
-- ============================================================
\echo '--- Test 5: circular prevention ---'

INSERT INTO m_contract_type (contract_type_cd, contract_type_nm)
VALUES ('92', 'テスト循環92');

-- 両テーブルにレコードが1件ずつ存在することを確認
SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM c_kkbn WHERE kkbn_id = 92) = 1
         AND (SELECT COUNT(*) FROM m_contract_type WHERE contract_type_cd = '92') = 1
        THEN 'PASS: Test 5 - No circular trigger loop'
        ELSE 'FAIL: Test 5 - Unexpected record count (circular loop?)'
    END AS result;

-- ============================================================
-- テスト6: DELETE同期（m_contract_type → c_kkbn）
-- ============================================================
\echo '--- Test 6: m_contract_type DELETE → c_kkbn sync ---'

DELETE FROM m_contract_type WHERE contract_type_cd = '92';

SELECT
    CASE
        WHEN NOT EXISTS (SELECT 1 FROM c_kkbn WHERE kkbn_id = 92)
        THEN 'PASS: Test 6 - m_contract_type DELETE → c_kkbn deleted'
        ELSE 'FAIL: Test 6 - c_kkbn(kkbn_id=92) still exists'
    END AS result;

-- ============================================================
-- テスト7: DELETE同期（c_kkbn → m_contract_type）
-- ============================================================
\echo '--- Test 7: c_kkbn DELETE → m_contract_type sync ---'

DELETE FROM c_kkbn WHERE kkbn_id = 91;

SELECT
    CASE
        WHEN NOT EXISTS (SELECT 1 FROM m_contract_type WHERE contract_type_cd = '91')
        THEN 'PASS: Test 7 - c_kkbn DELETE → m_contract_type deleted'
        ELSE 'FAIL: Test 7 - m_contract_type(cd=91) still exists'
    END AS result;

-- ============================================================
-- テスト8: PK変換の境界値テスト
-- ============================================================
\echo '--- Test 8: PK conversion edge cases ---'

-- kkbn_id=1 → '01', kkbn_id=9 → '09'
SELECT
    CASE
        WHEN conv_kkbn_id_to_contract_type_cd(1::smallint) = '01'
         AND conv_kkbn_id_to_contract_type_cd(9::smallint) = '09'
         AND conv_kkbn_id_to_contract_type_cd(12::smallint) = '12'
         AND conv_contract_type_cd_to_kkbn_id('01') = 1
         AND conv_contract_type_cd_to_kkbn_id('09') = 9
         AND conv_contract_type_cd_to_kkbn_id('12') = 12
        THEN 'PASS: Test 8 - PK conversion functions correct'
        ELSE 'FAIL: Test 8 - PK conversion mismatch'
    END AS result;

-- ============================================================
-- クリーンアップ
-- ============================================================
DELETE FROM m_contract_type WHERE contract_type_cd = '90';
DELETE FROM c_kkbn WHERE kkbn_id = 90;

\echo '--- All tests completed ---'

COMMIT;
