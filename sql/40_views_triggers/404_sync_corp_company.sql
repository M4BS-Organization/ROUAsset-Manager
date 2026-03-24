-- ============================================================
-- 404_sync_corp_company.sql
-- m_corp <-> m_company 双方向同期トリガー
--
-- Phase A Task A-5
-- 依存: 10_ddl_core/102_master_tables.sql (m_corp)
--       30_ddl_newlease/301_master_tables.sql (m_company)
--       40_views_triggers/401_sync_kkbn_contract_type.sql (is_sync_in_progress)
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 前提: m_corp.corp1_cd にユニーク制約を追加
-- アクティブレコード（history_f = false or NULL）で一意性を担保
-- ============================================================
CREATE UNIQUE INDEX IF NOT EXISTS idx_m_corp_corp1_cd_active
    ON m_corp (corp1_cd)
    WHERE (history_f IS NOT TRUE);

-- ============================================================
-- m_company → m_corp 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_company_to_corp()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_corp_id integer;
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' THEN
        -- corp_idはシーケンスなし、MAX+1方式
        SELECT COALESCE(MAX(corp_id), 0) + 1 INTO v_corp_id FROM m_corp;

        -- corp1_cdが既存（history_fレコード含む）の場合はUPDATE
        IF EXISTS (SELECT 1 FROM m_corp WHERE corp1_cd = NEW.company_cd AND history_f IS NOT TRUE) THEN
            UPDATE m_corp SET
                corp1_nm  = LEFT(NEW.company_nm, 40),
                corp2_cd  = LEFT(NEW.company_cd2, 12),
                corp2_nm  = LEFT(NEW.company_nm2, 40),
                corp3_cd  = LEFT(NEW.company_cd3, 12),
                corp3_nm  = LEFT(NEW.company_nm3, 40),
                biko      = LEFT(NEW.remarks, 100),
                update_dt = CURRENT_TIMESTAMP
            WHERE corp1_cd = NEW.company_cd AND history_f IS NOT TRUE;
        ELSE
            INSERT INTO m_corp (
                corp_id, corp1_cd, corp1_nm, corp2_cd, corp2_nm,
                corp3_cd, corp3_nm, biko, create_dt, update_dt
            ) VALUES (
                v_corp_id,
                LEFT(NEW.company_cd, 12),
                LEFT(NEW.company_nm, 40),
                LEFT(NEW.company_cd2, 12),
                LEFT(NEW.company_nm2, 40),
                LEFT(NEW.company_cd3, 12),
                LEFT(NEW.company_nm3, 40),
                LEFT(NEW.remarks, 100),
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP
            );
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        -- company_cd（PK）変更時: 旧corp1_cdを論理削除、新corp1_cdを挿入
        IF OLD.company_cd <> NEW.company_cd THEN
            UPDATE m_corp SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
            WHERE corp1_cd = OLD.company_cd AND history_f IS NOT TRUE;

            SELECT COALESCE(MAX(corp_id), 0) + 1 INTO v_corp_id FROM m_corp;

            INSERT INTO m_corp (
                corp_id, corp1_cd, corp1_nm, corp2_cd, corp2_nm,
                corp3_cd, corp3_nm, biko, create_dt, update_dt
            ) VALUES (
                v_corp_id,
                LEFT(NEW.company_cd, 12),
                LEFT(NEW.company_nm, 40),
                LEFT(NEW.company_cd2, 12),
                LEFT(NEW.company_nm2, 40),
                LEFT(NEW.company_cd3, 12),
                LEFT(NEW.company_nm3, 40),
                LEFT(NEW.remarks, 100),
                CURRENT_TIMESTAMP,
                CURRENT_TIMESTAMP
            );
        ELSE
            UPDATE m_corp SET
                corp1_nm  = LEFT(NEW.company_nm, 40),
                corp2_cd  = LEFT(NEW.company_cd2, 12),
                corp2_nm  = LEFT(NEW.company_nm2, 40),
                corp3_cd  = LEFT(NEW.company_cd3, 12),
                corp3_nm  = LEFT(NEW.company_nm3, 40),
                biko      = LEFT(NEW.remarks, 100),
                update_dt = CURRENT_TIMESTAMP
            WHERE corp1_cd = NEW.company_cd AND history_f IS NOT TRUE;
        END IF;

    ELSIF TG_OP = 'DELETE' THEN
        -- m_company削除 → m_corpは論理削除（m_kknri.corp_idがFK参照するため物理削除不可）
        UPDATE m_corp SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
        WHERE corp1_cd = OLD.company_cd AND history_f IS NOT TRUE;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN OLD;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- m_corp → m_company 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_corp_to_company()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- history_f = TRUE（論理削除）のレコードは同期しない
    IF NEW.history_f IS TRUE THEN
        -- m_company側を削除
        IF NEW.corp1_cd IS NOT NULL THEN
            DELETE FROM m_company WHERE company_cd = LEFT(NEW.corp1_cd, 10);
        END IF;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    -- corp1_cdがNULLの場合は同期スキップ
    IF NEW.corp1_cd IS NULL THEN
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_company (
            company_cd, company_nm, company_cd2, company_nm2,
            company_cd3, company_nm3, remarks
        ) VALUES (
            LEFT(NEW.corp1_cd, 10),
            COALESCE(NEW.corp1_nm, ''),
            LEFT(NEW.corp2_cd, 10),
            NEW.corp2_nm,
            LEFT(NEW.corp3_cd, 10),
            NEW.corp3_nm,
            NEW.biko
        )
        ON CONFLICT (company_cd) DO UPDATE SET
            company_nm  = EXCLUDED.company_nm,
            company_cd2 = EXCLUDED.company_cd2,
            company_nm2 = EXCLUDED.company_nm2,
            company_cd3 = EXCLUDED.company_cd3,
            company_nm3 = EXCLUDED.company_nm3,
            remarks     = EXCLUDED.remarks,
            update_dt   = CURRENT_TIMESTAMP;

        -- PK変更時（corp1_cdが変わった場合）: 旧company削除
        IF TG_OP = 'UPDATE' AND OLD.corp1_cd IS NOT NULL AND OLD.corp1_cd <> NEW.corp1_cd THEN
            DELETE FROM m_company WHERE company_cd = LEFT(OLD.corp1_cd, 10);
        END IF;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- トリガー定義
-- ============================================================

-- m_company → m_corp
DROP TRIGGER IF EXISTS trg_company_to_corp ON m_company;
CREATE TRIGGER trg_company_to_corp
    AFTER INSERT OR UPDATE OR DELETE ON m_company
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_company_to_corp();

COMMENT ON TRIGGER trg_company_to_corp ON m_company
    IS 'm_company変更時にm_corpへ自動同期';

-- m_corp → m_company
DROP TRIGGER IF EXISTS trg_corp_to_company ON m_corp;
CREATE TRIGGER trg_corp_to_company
    AFTER INSERT OR UPDATE ON m_corp
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_corp_to_company();

COMMENT ON TRIGGER trg_corp_to_company ON m_corp
    IS 'm_corp変更時にm_companyへ自動同期';

COMMIT;
