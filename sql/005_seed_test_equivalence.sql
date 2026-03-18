-- ============================================================
-- LeaseM4BS 等価性検証テスト用データ投入 (Issue #29)
--
-- 実行方法:
--   psql -U lease_m4bs_user -d lease_m4bs_dev -f 005_seed_test_equivalence.sql
--
-- 前提: 001_ddl.sql, 002_seed_dev.sql が実行済みであること
-- ============================================================

BEGIN;

-- ==========================================================
-- コードマスタ (テストに必要な最小限)
-- ==========================================================

-- 契約区分
INSERT INTO c_kkbn (kkbn_id, kkbn_nm) VALUES
    (1, 'ファイナンスリース'),
    (2, 'オペレーティングリース')
ON CONFLICT (kkbn_id) DO NOTHING;

-- 計上区分
INSERT INTO c_kjkbn (kjkbn_id, kjkbn_nm) VALUES
    (1, '費用'),
    (2, '資産')
ON CONFLICT (kjkbn_id) DO NOTHING;

-- リース区分
INSERT INTO c_leakbn (leakbn_id, leakbn_nm) VALUES
    (1, '移転ファイナンスリース'),
    (3, '移転外ファイナンスリース'),
    (4, 'オペレーティングリース')
ON CONFLICT (leakbn_id) DO NOTHING;

-- 計算方法区分
INSERT INTO c_rcalc (rcalc_id, rcalc_nm) VALUES
    (1, '利子抜法'),
    (2, '利子込法')
ON CONFLICT (rcalc_id) DO NOTHING;

-- 償却方法
INSERT INTO c_skyak_ho (skyak_ho_id, skyak_ho_nm) VALUES
    (1, '定額法'),
    (2, '定率法')
ON CONFLICT (skyak_ho_id) DO NOTHING;

-- ==========================================================
-- 法人マスタ
-- ==========================================================
INSERT INTO m_corp (corp_id, corp_cd, corp_nm, kessan_bi, kishu_getu)
VALUES (1, 'TEST-CORP', 'テスト法人', 31, 4)
ON CONFLICT (corp_id) DO NOTHING;

-- ==========================================================
-- EQ-001/EQ-005: 定額法12ヶ月リース（標準・後払・利子抜法）
-- ==========================================================

-- 契約ヘッダ
INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90001, 90001, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2025-03-31', 12,
    1, 1050000, 0, 0,
    1, 12, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

-- 契約明細（物件）
INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90001, 90001, 90001, 1, 1,
    1050000, 0, 0, 1000000,
    0.025, 1, FALSE, '2025-03-31',
    TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

-- ==========================================================
-- EQ-002: 定額法24ヶ月リース（残価保証あり）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90002, 90002, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2026-03-31', 24,
    1, 2640000, 0, 240000,
    1, 24, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90002, 90002, 90002, 1, 1,
    2640000, 0, 240000, 2400000,
    0.03, 1, FALSE, '2026-03-31',
    TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

-- ==========================================================
-- EQ-004: 定額法12ヶ月（減損あり）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90004, 90004, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2025-03-31', 12,
    1, 1320000, 0, 0,
    1, 12, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, genson_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90004, 90004, 90004, 1, 1,
    1320000, 0, 0, 1200000,
    0.025, 1, FALSE, '2025-03-31',
    TRUE, TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

-- 減損データ（2024年7月末に200,000円の減損）
INSERT INTO d_gson (
    kykm_id, line_id, kykh_id, kykh_no, saikaisu, kykm_no,
    gson_dt, gson_tmg, gson_ryo, gson_rkei
) VALUES (
    90004, 1, 90004, 90004, 1, 1,
    '2024-07-31', 0, 200000, 200000
) ON CONFLICT (kykm_id, line_id) DO NOTHING;

-- ==========================================================
-- EQ-006: 先払24ヶ月（維持管理費・残価保証あり）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90006, 90006, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2026-03-31', 24,
    1, 2200000, 50000, 100000,
    1, 24, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90006, 90006, 90006, 1, 1,
    2200000, 50000, 100000, 2000000,
    0.03, 1, FALSE, '2026-03-31',
    TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

-- ==========================================================
-- EQ-007: リースバック損益あり（後払12ヶ月）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90007, 90007, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2025-03-31', 12,
    1, 1100000, 0, 0,
    1, 12, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90007, 90007, 90007, 1, 1,
    1100000, 0, 0, 1000000,
    0.02, 1, FALSE, '2025-03-31',
    TRUE, 120000, TRUE
) ON CONFLICT (kykm_id) DO NOTHING;

-- ==========================================================
-- EQ-008: 注記計算（移転FA・定額法・利子抜法・5年）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f
) VALUES (
    90008, 90008, 1, 1, 1, NULL,
    '2024-03-15', '2024-04-01', '2029-03-31', 60,
    1, 6000000, 0, 0,
    1, 60, 2,
    TRUE, FALSE
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90008, 90008, 90008, 1, 1,
    6000000, 0, 0, 5000000,
    0.025, 1, FALSE, '2029-03-31',
    TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

-- ==========================================================
-- EQ-009: 注記計算（中途解約）
-- ==========================================================

INSERT INTO d_kykh (
    kykh_id, kykh_no, saikaisu, kknri_id, kkbn_id, lcpt_id,
    kyak_dt, start_dt, end_dt, lkikan,
    kykm_cnt, k_slsryo, k_ijiknr, k_zanryo,
    shri_kn, shri_cnt, kjkbn_id,
    k_seigou_f, k_history_f, k_ckaiyk_f, k_rend_dt
) VALUES (
    90009, 90009, 1, 1, 1, NULL,
    '2023-03-15', '2023-04-01', '2026-03-31', 36,
    1, 3600000, 0, 0,
    1, 36, 2,
    TRUE, FALSE, TRUE, '2024-09-30'
) ON CONFLICT (kykh_id) DO NOTHING;

INSERT INTO d_kykm (
    kykm_id, kykh_id, kykh_no, kykm_no, saikaisu,
    b_slsryo, b_ijiknr, b_zanryo, b_syutok,
    ksan_ritu, leakbn_id, b_ckaiyk_f, b_rend_dt,
    ckaiyk_dt, b_seigou_f, b_lb_soneki, lb_chuki_f
) VALUES (
    90009, 90009, 90009, 1, 1,
    3600000, 0, 0, 3000000,
    0.025, 1, TRUE, '2024-09-30',
    '2024-09-30', TRUE, NULL, FALSE
) ON CONFLICT (kykm_id) DO NOTHING;

COMMIT;

-- ============================================================
-- 投入データ確認
-- ============================================================
\echo ''
\echo '=== 等価性テスト用データ確認 ==='
\echo ''
\echo '--- 契約ヘッダ (d_kykh) ---'
SELECT kykh_id, kykh_no, start_dt::date, end_dt::date, lkikan, k_slsryo, k_ijiknr, k_zanryo
FROM d_kykh WHERE kykh_id >= 90000 ORDER BY kykh_id;

\echo ''
\echo '--- 契約明細 (d_kykm) ---'
SELECT kykm_id, kykh_id, b_slsryo, b_ijiknr, b_zanryo, b_syutok, ksan_ritu, leakbn_id, b_ckaiyk_f
FROM d_kykm WHERE kykm_id >= 90000 ORDER BY kykm_id;

\echo ''
\echo '--- 減損データ (d_gson) ---'
SELECT kykm_id, line_id, gson_dt::date, gson_tmg, gson_ryo, gson_rkei
FROM d_gson WHERE kykm_id >= 90000 ORDER BY kykm_id, line_id;
