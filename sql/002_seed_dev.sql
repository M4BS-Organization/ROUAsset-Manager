-- ============================================================
-- LeaseM4BS 開発環境用 初期データ投入
--
-- 実行方法:
--   psql -U lease_m4bs_user -d lease_m4bs_dev -f 002_seed_dev.sql
--
-- 注意: 開発・テスト環境専用です。本番環境には使用しないでください。
-- ============================================================

BEGIN;

-- ==========================================================
-- 権限グループ (sec_kngn)
-- ==========================================================
INSERT INTO sec_kngn (kngn_id, kngn_cd, kngn_nm, access_kind, access_kind_b,
    biko, create_id, create_dt, update_id, update_dt, update_cnt, history_f,
    admin, master_update, file_output, print, log_ref, approval)
VALUES
    -- 管理者権限（全権限付与）
    (1, 'ADMIN', '管理者', 1, 1,
     '全機能へのアクセス権限', 0, NOW(), 0, NOW(), 0, FALSE,
     TRUE, TRUE, TRUE, TRUE, TRUE, TRUE),
    -- 一般ユーザー権限
    (2, 'USER', '一般ユーザー', 1, 1,
     '基本機能のみ', 0, NOW(), 0, NOW(), 0, FALSE,
     FALSE, FALSE, TRUE, TRUE, FALSE, FALSE),
    -- 閲覧専用権限
    (3, 'VIEWER', '閲覧専用', 0, 0,
     '参照のみ', 0, NOW(), 0, NOW(), 0, FALSE,
     FALSE, FALSE, FALSE, FALSE, FALSE, FALSE)
ON CONFLICT (kngn_id) DO NOTHING;

-- ==========================================================
-- ユーザー (sec_user)
-- ==========================================================
INSERT INTO sec_user (user_id, user_cd, user_nm, pwd, kngn_id,
    biko, create_id, create_dt, update_id, update_dt, update_cnt, history_f,
    login_attempts, pwd_life_time, pwd_grace_time, pwd_min,
    pwd_moji_chk, pwd_alph_chk, pwd_num_chk, pwd_symbol_chk,
    pwd_upd_dt, d_first_login, err_ct, last_err_dt)
VALUES
    -- 管理者ユーザー (admin / admin123) ※SHA256ハッシュで格納
    (1, 'admin', '管理者', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 1,
     '開発用管理者アカウント', 0, NOW(), 0, NOW(), 0, FALSE,
     10, NULL, NULL, NULL,
     FALSE, FALSE, FALSE, FALSE,
     NOW(), NULL, 0, NULL),
    -- 一般ユーザー (user01 / pass01) ※SHA256ハッシュで格納
    (2, 'user01', 'テストユーザー01', 'fa66ed652b77f7a4bbc9e07201ea3e37cdef4e8e130890b137aa5f55a65af1d0', 2,
     '開発用一般アカウント', 0, NOW(), 0, NOW(), 0, FALSE,
     5, NULL, NULL, NULL,
     FALSE, FALSE, FALSE, FALSE,
     NOW(), NULL, 0, NULL),
    -- 閲覧専用ユーザー (viewer / view01) ※SHA256ハッシュで格納
    (3, 'viewer', '閲覧ユーザー', '971509e8489927e6a7fd28735da035cbe455e10d8a3d305e0e7454c7a3ac506f', 3,
     '開発用閲覧専用アカウント', 0, NOW(), 0, NOW(), 0, FALSE,
     5, NULL, NULL, NULL,
     FALSE, FALSE, FALSE, FALSE,
     NOW(), NULL, 0, NULL)
ON CONFLICT (user_id) DO NOTHING;

-- ==========================================================
-- 統制オプション (t_opt) — ログ出力フラグ初期値
-- Access版 fgNT_SLOGOUT / fgNT_ULOGOUT / fgNT_RECOUT / fgNT_DTCNVLOG に相当
-- ==========================================================
INSERT INTO t_opt (slog, ulog, recopt, cnvlog)
SELECT true, true, true, false
WHERE NOT EXISTS (SELECT 1 FROM t_opt LIMIT 1);

COMMIT;

-- ============================================================
-- 投入データ確認
-- ============================================================
\echo '--- 権限グループ ---'
SELECT kngn_id, kngn_cd, kngn_nm, admin, master_update, file_output, print FROM sec_kngn ORDER BY kngn_id;

\echo ''
\echo '--- ユーザー一覧 ---'
SELECT user_id, user_cd, user_nm, kngn_id, biko FROM sec_user ORDER BY user_id;

\echo ''
\echo '--- ログイン情報 ---'
\echo '  admin   / admin123  (管理者権限) ※パスワードはSHA256ハッシュで格納'
\echo '  user01  / pass01    (一般ユーザー権限)'
\echo '  viewer  / view01    (閲覧専用権限)'
