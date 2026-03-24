-- ============================================================
-- 405_test_sync_user.sql
-- sec_user <-> tw_m_user (tm_USER) 同期トリガー テストスクリプト
--
-- 実行方法: psql -d lease_m4bs -f 405_test_sync_user.sql
-- 前提: 401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--       305_alter_tm_user.sql が適用済み
--       405_sync_user.sql が適用済み
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 準備: テスト用データをクリーン
-- ============================================================
DELETE FROM sec_user WHERE user_id IN (9901, 9902, 9903);
DELETE FROM tw_m_user WHERE user_id IN (9901, 9902, 9903);

-- ============================================================
-- テスト1: tw_m_user INSERT → sec_user 同期
-- ============================================================
\echo '--- Test 1: tw_m_user INSERT → sec_user sync ---'

INSERT INTO tw_m_user (
    user_id, user_name, user_kana,
    login_id, password_hash, role, is_active,
    failed_login_count
)
VALUES (
    9901, 'テストユーザー1', 'テストユーザー1カナ',
    'test_user_9901', 'hash_dummy_9901', 'admin', TRUE,
    0
);

-- 検証: sec_userに user_id=9901 が作成され、role='admin'→kngn_id=1
SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM sec_user
            WHERE user_id = 9901 AND user_nm = 'テストユーザー1'
              AND kngn_id = 1 AND history_f IS NOT TRUE
        )
        THEN 'PASS: Test 1 - tw_m_user INSERT → sec_user sync (admin→kngn_id=1)'
        ELSE 'FAIL: Test 1 - sec_user(user_id=9901) not found or kngn_id wrong'
    END AS result;

-- ============================================================
-- テスト2: sec_user INSERT → tw_m_user 同期
-- ============================================================
\echo '--- Test 2: sec_user INSERT → tw_m_user sync ---'

INSERT INTO sec_user (user_id, user_cd, user_nm, kngn_id, history_f)
VALUES (9902, 'TU02', 'テストユーザー2', 2, FALSE);

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM tw_m_user
            WHERE user_id = 9902 AND user_name = 'テストユーザー2'
        )
        THEN 'PASS: Test 2 - sec_user INSERT → tw_m_user sync'
        ELSE 'FAIL: Test 2 - tw_m_user(user_id=9902) not found'
    END AS result;

-- ============================================================
-- テスト3: tw_m_user UPDATE → sec_user 同期
--   role変更 admin→viewer, is_active=FALSE→history_f=TRUE
-- ============================================================
\echo '--- Test 3: tw_m_user UPDATE → sec_user sync ---'

UPDATE tw_m_user SET role = 'viewer', is_active = FALSE
WHERE user_id = 9901;

SELECT
    CASE
        WHEN EXISTS (
            SELECT 1 FROM sec_user
            WHERE user_id = 9901
              AND kngn_id = (SELECT kngn_id FROM t_role_kngn_mapping WHERE role = 'viewer')
              AND history_f = TRUE
        )
        THEN 'PASS: Test 3 - tw_m_user UPDATE → sec_user sync (viewer + history_f=TRUE)'
        ELSE 'FAIL: Test 3 - sec_user(user_id=9901) role or history_f not updated'
    END AS result;

-- ============================================================
-- テスト4: 循環防止テスト
-- ============================================================
\echo '--- Test 4: circular prevention ---'

INSERT INTO tw_m_user (
    user_id, user_name, user_kana,
    login_id, password_hash, role, is_active,
    failed_login_count
)
VALUES (
    9903, 'テスト循環ユーザー', 'テストジュンカン',
    'test_user_9903', 'hash_dummy_9903', 'editor', TRUE,
    0
);

SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM sec_user WHERE user_id = 9903) = 1
         AND (SELECT COUNT(*) FROM tw_m_user WHERE user_id = 9903) = 1
        THEN 'PASS: Test 4 - No circular trigger loop'
        ELSE 'FAIL: Test 4 - Unexpected record count'
    END AS result;

-- ============================================================
-- テスト5: t_role_kngn_mapping マッピング確認（4ロール全て）
-- ============================================================
\echo '--- Test 5: t_role_kngn_mapping all 4 roles ---'

SELECT
    CASE
        WHEN (SELECT COUNT(*) FROM t_role_kngn_mapping) >= 4
         AND EXISTS (SELECT 1 FROM t_role_kngn_mapping WHERE role = 'admin')
         AND EXISTS (SELECT 1 FROM t_role_kngn_mapping WHERE role = 'editor')
         AND EXISTS (SELECT 1 FROM t_role_kngn_mapping WHERE role = 'viewer')
         AND EXISTS (SELECT 1 FROM t_role_kngn_mapping WHERE role = 'operator')
        THEN 'PASS: Test 5 - All 4 roles exist in t_role_kngn_mapping'
        ELSE 'FAIL: Test 5 - Missing roles in t_role_kngn_mapping'
    END AS result;

-- ============================================================
-- テスト6: conv_role_to_kngn_id / conv_kngn_id_to_role ヘルパー関数
-- ============================================================
\echo '--- Test 6: helper function conv_role_to_kngn_id / conv_kngn_id_to_role ---'

SELECT
    CASE
        WHEN conv_role_to_kngn_id('admin') = 1
         AND conv_role_to_kngn_id('editor') IS NOT NULL
         AND conv_role_to_kngn_id('viewer') IS NOT NULL
         AND conv_role_to_kngn_id('operator') IS NOT NULL
         AND conv_kngn_id_to_role(1) = 'admin'
         AND conv_kngn_id_to_role(conv_role_to_kngn_id('editor')) = 'editor'
         AND conv_kngn_id_to_role(conv_role_to_kngn_id('viewer')) = 'viewer'
         AND conv_kngn_id_to_role(conv_role_to_kngn_id('operator')) = 'operator'
        THEN 'PASS: Test 6 - Helper functions roundtrip correct'
        ELSE 'FAIL: Test 6 - Helper function conversion mismatch'
    END AS result;

-- ============================================================
-- クリーンアップ
-- ============================================================
DELETE FROM sec_user WHERE user_id IN (9901, 9902, 9903);
DELETE FROM tw_m_user WHERE user_id IN (9901, 9902, 9903);

\echo '--- All tests completed ---'

COMMIT;
