
-- ============================================================
-- 401_sync_kkbn_contract_type.sql
-- c_kkbn <-> m_contract_type 双方向同期トリガー
--
-- Phase A Task A-2: パイロットケース
-- 依存: 10_ddl_core/101_code_tables.sql (c_kkbn)
--       30_ddl_newlease/301_master_tables.sql (m_contract_type)
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 循環防止ヘルパー関数（全同期トリガー共通）
-- セッション変数 sync.in_progress で再帰呼び出しを防ぐ
-- ============================================================
CREATE OR REPLACE FUNCTION is_sync_in_progress()
RETURNS BOOLEAN
LANGUAGE plpgsql
STABLE
AS $$
BEGIN
    RETURN COALESCE(current_setting('sync.in_progress', TRUE), 'false') = 'true';
END;
$$;

COMMENT ON FUNCTION is_sync_in_progress() IS '同期トリガーの循環防止チェック関数';

-- ============================================================
-- PK変換ヘルパー関数
-- ============================================================

-- contract_type_cd ('01','02',...) → kkbn_id (1,2,...)
CREATE OR REPLACE FUNCTION conv_contract_type_cd_to_kkbn_id(p_cd VARCHAR)
RETURNS smallint
LANGUAGE plpgsql
IMMUTABLE
AS $$
BEGIN
    RETURN CAST(
        CASE
            WHEN LTRIM(p_cd, '0') = '' THEN '0'
            ELSE LTRIM(p_cd, '0')
        END AS smallint
    );
END;
$$;

-- kkbn_id (1,2,...) → contract_type_cd ('01','02',...)
CREATE OR REPLACE FUNCTION conv_kkbn_id_to_contract_type_cd(p_id smallint)
RETURNS VARCHAR(10)
LANGUAGE plpgsql
IMMUTABLE
AS $$
BEGIN
    RETURN LPAD(CAST(p_id AS VARCHAR), 2, '0');
END;
$$;

-- ============================================================
-- m_contract_type → c_kkbn 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_contract_type_to_kkbn()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_kkbn_id smallint;
    v_old_kkbn_id smallint;
BEGIN
    -- 循環防止
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    -- セッション変数セット（トランザクション終了時に自動リセット）
    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- PK変換
    BEGIN
        v_kkbn_id := conv_contract_type_cd_to_kkbn_id(NEW.contract_type_cd);
    EXCEPTION WHEN OTHERS THEN
        RAISE WARNING '[sync] contract_type→kkbn PK変換失敗: cd=%, error=%',
            NEW.contract_type_cd, SQLERRM;
        PERFORM pg_notify('sync_error', json_build_object(
            'source', 'm_contract_type',
            'target', 'c_kkbn',
            'key', NEW.contract_type_cd,
            'error', SQLERRM,
            'ts', CURRENT_TIMESTAMP
        )::text);
        RETURN NEW;
    END;

    IF TG_OP = 'INSERT' THEN
        INSERT INTO c_kkbn (kkbn_id, kkbn_nm)
        VALUES (v_kkbn_id, LEFT(NEW.contract_type_nm, 50))
        ON CONFLICT (kkbn_id) DO UPDATE SET
            kkbn_nm = EXCLUDED.kkbn_nm;

    ELSIF TG_OP = 'UPDATE' THEN
        -- PK変更チェック
        IF OLD.contract_type_cd <> NEW.contract_type_cd THEN
            v_old_kkbn_id := conv_contract_type_cd_to_kkbn_id(OLD.contract_type_cd);
            -- 旧レコード削除 + 新レコード挿入
            DELETE FROM c_kkbn WHERE kkbn_id = v_old_kkbn_id;
            INSERT INTO c_kkbn (kkbn_id, kkbn_nm)
            VALUES (v_kkbn_id, LEFT(NEW.contract_type_nm, 50))
            ON CONFLICT (kkbn_id) DO UPDATE SET
                kkbn_nm = EXCLUDED.kkbn_nm;
        ELSE
            UPDATE c_kkbn SET
                kkbn_nm = LEFT(NEW.contract_type_nm, 50)
            WHERE kkbn_id = v_kkbn_id;
        END IF;

    ELSIF TG_OP = 'DELETE' THEN
        v_kkbn_id := conv_contract_type_cd_to_kkbn_id(OLD.contract_type_cd);
        -- FK参照がある場合はDELETEしない（d_kykh.kkbn_id→c_kkbn）
        DELETE FROM c_kkbn WHERE kkbn_id = v_kkbn_id;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN OLD;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- c_kkbn → m_contract_type 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_kkbn_to_contract_type()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_cd VARCHAR(10);
    v_old_cd VARCHAR(10);
BEGIN
    -- 循環防止
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- PK変換
    v_cd := conv_kkbn_id_to_contract_type_cd(NEW.kkbn_id);

    IF TG_OP = 'INSERT' THEN
        INSERT INTO m_contract_type (contract_type_cd, contract_type_nm)
        VALUES (v_cd, COALESCE(NEW.kkbn_nm, ''))
        ON CONFLICT (contract_type_cd) DO UPDATE SET
            contract_type_nm = EXCLUDED.contract_type_nm,
            update_dt = CURRENT_TIMESTAMP;

    ELSIF TG_OP = 'UPDATE' THEN
        IF OLD.kkbn_id <> NEW.kkbn_id THEN
            v_old_cd := conv_kkbn_id_to_contract_type_cd(OLD.kkbn_id);
            DELETE FROM m_contract_type WHERE contract_type_cd = v_old_cd;
            INSERT INTO m_contract_type (contract_type_cd, contract_type_nm)
            VALUES (v_cd, COALESCE(NEW.kkbn_nm, ''))
            ON CONFLICT (contract_type_cd) DO UPDATE SET
                contract_type_nm = EXCLUDED.contract_type_nm,
                update_dt = CURRENT_TIMESTAMP;
        ELSE
            UPDATE m_contract_type SET
                contract_type_nm = COALESCE(NEW.kkbn_nm, ''),
                update_dt = CURRENT_TIMESTAMP
            WHERE contract_type_cd = v_cd;
        END IF;

    ELSIF TG_OP = 'DELETE' THEN
        v_cd := conv_kkbn_id_to_contract_type_cd(OLD.kkbn_id);
        -- FK参照がある場合はDELETEしない（ctb_lease_integrated.contract_type_cd→m_contract_type）
        DELETE FROM m_contract_type WHERE contract_type_cd = v_cd;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN OLD;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- トリガー定義
-- ============================================================

-- m_contract_type → c_kkbn
DROP TRIGGER IF EXISTS trg_contract_type_to_kkbn ON m_contract_type;
CREATE TRIGGER trg_contract_type_to_kkbn
    AFTER INSERT OR UPDATE OR DELETE ON m_contract_type
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_contract_type_to_kkbn();

COMMENT ON TRIGGER trg_contract_type_to_kkbn ON m_contract_type
    IS 'm_contract_type変更時にc_kkbnへ自動同期';

-- c_kkbn → m_contract_type
DROP TRIGGER IF EXISTS trg_kkbn_to_contract_type ON c_kkbn;
CREATE TRIGGER trg_kkbn_to_contract_type
    AFTER INSERT OR UPDATE OR DELETE ON c_kkbn
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_kkbn_to_contract_type();

COMMENT ON TRIGGER trg_kkbn_to_contract_type ON c_kkbn
    IS 'c_kkbn変更時にm_contract_typeへ自動同期';

COMMIT;
