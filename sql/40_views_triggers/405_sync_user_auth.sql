-- ============================================================
-- 405_sync_user_auth.sql
-- sec_user + tw_m_user <-> tm_USER 同期トリガー
--
-- Phase A Task A-6
-- 依存: 10_ddl_core/104_security_tables.sql (sec_user, tw_m_user)
--       30_ddl_newlease/305_alter_tm_user.sql (tm_USER = tw_m_user + ALTER)
--       40_views_triggers/401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--
-- 設計方針:
--   - tm_USER（=tw_m_user+拡張）を認証のマスターとする
--   - パスワードは同期しない（sec_user=平文/旧形式, tm_USER=PBKDF2）
--   - ユーザー基本情報（名前、アクティブ状態）のみ同期
--   - role↔kngn_id はマッピングテーブルで変換
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- ロール↔権限IDマッピングテーブル
-- ============================================================
CREATE TABLE IF NOT EXISTS t_role_kngn_mapping (
    role         VARCHAR(20)  NOT NULL,
    kngn_id      integer      NOT NULL,
    description  VARCHAR(100),
    CONSTRAINT pk_role_kngn_mapping PRIMARY KEY (role)
);

COMMENT ON TABLE t_role_kngn_mapping IS 'tm_USER.role と sec_kngn.kngn_id のマッピング';

-- デフォルトマッピング（sec_kngn の seed データに合わせる）
-- sec_kngn: 1=システム管理者, 2=一般ユーザー, 3=閲覧のみ
INSERT INTO t_role_kngn_mapping (role, kngn_id, description)
VALUES
    ('admin',           1, 'システム管理者 → kngn_id=1'),
    ('accounting',      2, '経理担当 → kngn_id=2'),
    ('general_affairs', 2, '総務担当 → kngn_id=2'),
    ('viewer',          3, '閲覧のみ → kngn_id=3')
ON CONFLICT (role) DO NOTHING;

-- ============================================================
-- ヘルパー関数: role → kngn_id
-- ============================================================
CREATE OR REPLACE FUNCTION conv_role_to_kngn_id(p_role VARCHAR)
RETURNS integer
LANGUAGE plpgsql
STABLE
AS $$
DECLARE
    v_kngn_id integer;
BEGIN
    SELECT kngn_id INTO v_kngn_id FROM t_role_kngn_mapping WHERE role = p_role;
    RETURN COALESCE(v_kngn_id, 3); -- デフォルトは閲覧のみ
END;
$$;

-- ============================================================
-- ヘルパー関数: kngn_id → role
-- ============================================================
CREATE OR REPLACE FUNCTION conv_kngn_id_to_role(p_kngn_id integer)
RETURNS VARCHAR(20)
LANGUAGE plpgsql
STABLE
AS $$
DECLARE
    v_role VARCHAR(20);
BEGIN
    SELECT role INTO v_role FROM t_role_kngn_mapping WHERE kngn_id = p_kngn_id LIMIT 1;
    RETURN COALESCE(v_role, 'viewer');
END;
$$;

-- ============================================================
-- tm_USER (tw_m_user) → sec_user 同期トリガー関数
-- マスター: tm_USER → スレーブ: sec_user
-- パスワードは同期しない
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_tm_user_to_sec_user()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_kngn_id integer;
    v_sec_user_id integer;
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- role → kngn_id 変換
    v_kngn_id := conv_role_to_kngn_id(NEW.role);

    IF TG_OP = 'INSERT' THEN
        -- sec_userに同一user_idが存在するかチェック
        IF NOT EXISTS (SELECT 1 FROM sec_user WHERE user_id = NEW.user_id) THEN
            INSERT INTO sec_user (
                user_id, user_cd, user_nm, kngn_id,
                history_f, create_dt, update_dt
            ) VALUES (
                NEW.user_id,
                LEFT(NEW.login_id, 12),
                LEFT(NEW.user_name, 40),
                v_kngn_id,
                NOT NEW.is_active,  -- is_active=TRUE → history_f=FALSE
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP
            );
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE sec_user SET
            user_cd   = LEFT(NEW.login_id, 12),
            user_nm   = LEFT(NEW.user_name, 40),
            kngn_id   = v_kngn_id,
            history_f = NOT NEW.is_active,
            err_ct    = NEW.failed_login_count,
            update_dt = CURRENT_TIMESTAMP
        WHERE user_id = NEW.user_id;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- sec_user → tw_m_user (tm_USER) 同期トリガー関数
-- 基本情報のみ（名前、カナ）。パスワード・ロールは同期しない
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_sec_user_to_tm_user()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' THEN
        -- tw_m_user（=tm_USER）に同一user_idが存在するかチェック
        IF NOT EXISTS (SELECT 1 FROM tw_m_user WHERE user_id = NEW.user_id) THEN
            INSERT INTO tw_m_user (
                user_id, user_name, create_date, update_date,
                login_id, password_hash, role, is_active
            ) VALUES (
                NEW.user_id,
                NEW.user_nm,
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP,
                COALESCE(NEW.user_cd, 'user_' || NEW.user_id),
                'SYNC_FROM_SEC_USER',  -- プレースホルダ（sec_userのpwdは旧形式のため変換不可）
                conv_kngn_id_to_role(NEW.kngn_id),
                NOT COALESCE(NEW.history_f, FALSE)
            );
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE tw_m_user SET
            user_name   = NEW.user_nm,
            update_date = CURRENT_TIMESTAMP
        WHERE user_id = NEW.user_id;

        -- is_active の逆同期（history_f → is_active）
        -- tm_USERのALTERカラムに直接アクセス
        UPDATE tw_m_user SET
            update_date = CURRENT_TIMESTAMP
        WHERE user_id = NEW.user_id;

        -- is_activeフラグの同期（history_fの反転）
        -- 注: tw_m_userテーブルにis_activeカラムが存在する前提（305_alter_tm_user.sql適用後）
        BEGIN
            EXECUTE format(
                'UPDATE tw_m_user SET is_active = $1 WHERE user_id = $2'
            ) USING (NOT NEW.history_f), NEW.user_id;
        EXCEPTION WHEN undefined_column THEN
            -- is_activeカラムが未追加の場合は無視
            NULL;
        END;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- トリガー定義
-- ============================================================

-- tm_USER (tw_m_user) → sec_user
DROP TRIGGER IF EXISTS trg_tm_user_to_sec_user ON tw_m_user;
CREATE TRIGGER trg_tm_user_to_sec_user
    AFTER INSERT OR UPDATE ON tw_m_user
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_tm_user_to_sec_user();

COMMENT ON TRIGGER trg_tm_user_to_sec_user ON tw_m_user
    IS 'tm_USER変更時にsec_userへ基本情報を自動同期（パスワード除外）';

-- sec_user → tw_m_user (tm_USER)
DROP TRIGGER IF EXISTS trg_sec_user_to_tm_user ON sec_user;
CREATE TRIGGER trg_sec_user_to_tm_user
    AFTER INSERT OR UPDATE ON sec_user
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_sec_user_to_tm_user();

COMMENT ON TRIGGER trg_sec_user_to_tm_user ON sec_user
    IS 'sec_user変更時にtm_USERへ基本情報を自動同期（パスワード除外）';

COMMIT;
