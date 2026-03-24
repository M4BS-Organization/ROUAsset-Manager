-- ============================================================
-- 403_sync_bcat_department.sql
-- m_bcat <-> m_department 双方向同期トリガー + 統合ビュー
--
-- Phase A Task A-4
-- 依存: 10_ddl_core/102_master_tables.sql (m_bcat, m_genk)
--       30_ddl_newlease/301_master_tables.sql (m_department)
--       40_views_triggers/401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--
-- カラムマッピング:
--   m_bcat.bcat1_cd    ↔ m_department.dept_cd
--   m_bcat.bcat1_nm    ↔ m_department.dept_nm
--   m_bcat.bcat2_cd/nm ↔ m_department.dept_cd2/nm2
--   m_bcat.bcat3_cd/nm ↔ m_department.dept_cd3/nm3
--   m_bcat.bcat4_cd/nm ↔ m_department.dept_cd4/nm4
--   m_bcat.bcat5_cd/nm ↔ m_department.dept_cd5/nm5
--   m_bcat.sum1_cd/nm  ↔ m_department.agg_category1_cd/nm
--   m_bcat.sum2_cd/nm  ↔ m_department.agg_category2_cd/nm
--   m_bcat.sum3_cd/nm  ↔ m_department.agg_category3_cd/nm
--   m_bcat.genk_id→m_genk.genk_nm ↔ m_department.cost_category_nm
--   m_bcat.biko        ↔ m_department.remarks
--
-- 同期対象外（SQL1固有）:
--   skti_id, bknri_id, kbf_kb, kbf_fb, kbf_sb, gensonf
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 前提: m_bcat.bcat1_cd にアクティブ行のユニーク制約を追加
-- ============================================================
CREATE UNIQUE INDEX IF NOT EXISTS idx_m_bcat_bcat1_cd_active
    ON m_bcat (bcat1_cd)
    WHERE (history_f IS NOT TRUE);

-- ============================================================
-- 統合ビュー: SQL1/SQL2の部門情報を統一的に参照
-- ============================================================
CREATE OR REPLACE VIEW v_unified_department AS
SELECT
    d.dept_cd,
    d.dept_nm,
    d.dept_cd2,
    d.dept_nm2,
    d.dept_cd3,
    d.dept_nm3,
    d.dept_cd4,
    d.dept_nm4,
    d.dept_cd5,
    d.dept_nm5,
    d.cost_category_nm,
    d.agg_category1_cd,
    d.agg_category1_nm,
    d.agg_category2_cd,
    d.agg_category2_nm,
    d.agg_category3_cd,
    d.agg_category3_nm,
    d.remarks,
    -- SQL1側の追加情報
    b.bcat_id,
    b.genk_id,
    b.skti_id,
    b.bknri_id,
    b.kbf_kb,
    b.kbf_fb,
    b.kbf_sb,
    b.gensonf
FROM m_department d
LEFT JOIN m_bcat b ON b.bcat1_cd = d.dept_cd AND b.history_f IS NOT TRUE;

COMMENT ON VIEW v_unified_department IS 'SQL1(m_bcat)とSQL2(m_department)の統合ビュー';

-- ============================================================
-- m_department → m_bcat 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_department_to_bcat()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_bcat_id integer;
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' THEN
        IF EXISTS (SELECT 1 FROM m_bcat WHERE bcat1_cd = NEW.dept_cd AND history_f IS NOT TRUE) THEN
            UPDATE m_bcat SET
                bcat1_nm = LEFT(NEW.dept_nm, 80),
                bcat2_cd = LEFT(NEW.dept_cd2, 12),
                bcat2_nm = LEFT(NEW.dept_nm2, 40),
                bcat3_cd = LEFT(NEW.dept_cd3, 12),
                bcat3_nm = LEFT(NEW.dept_nm3, 40),
                bcat4_cd = LEFT(NEW.dept_cd4, 12),
                bcat4_nm = LEFT(NEW.dept_nm4, 40),
                bcat5_cd = LEFT(NEW.dept_cd5, 12),
                bcat5_nm = LEFT(NEW.dept_nm5, 40),
                sum1_cd  = LEFT(NEW.agg_category1_cd, 12),
                sum1_nm  = LEFT(NEW.agg_category1_nm, 40),
                sum2_cd  = LEFT(NEW.agg_category2_cd, 12),
                sum2_nm  = LEFT(NEW.agg_category2_nm, 40),
                sum3_cd  = LEFT(NEW.agg_category3_cd, 12),
                sum3_nm  = LEFT(NEW.agg_category3_nm, 40),
                biko     = LEFT(NEW.remarks, 100),
                update_dt = CURRENT_TIMESTAMP
            WHERE bcat1_cd = NEW.dept_cd AND history_f IS NOT TRUE;
        ELSE
            SELECT COALESCE(MAX(bcat_id), 0) + 1 INTO v_bcat_id FROM m_bcat;

            INSERT INTO m_bcat (
                bcat_id, bcat1_cd, bcat1_nm,
                bcat2_cd, bcat2_nm, bcat3_cd, bcat3_nm,
                bcat4_cd, bcat4_nm, bcat5_cd, bcat5_nm,
                sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
                biko, create_dt, update_dt
            ) VALUES (
                v_bcat_id,
                LEFT(NEW.dept_cd, 12), LEFT(NEW.dept_nm, 80),
                LEFT(NEW.dept_cd2, 12), LEFT(NEW.dept_nm2, 40),
                LEFT(NEW.dept_cd3, 12), LEFT(NEW.dept_nm3, 40),
                LEFT(NEW.dept_cd4, 12), LEFT(NEW.dept_nm4, 40),
                LEFT(NEW.dept_cd5, 12), LEFT(NEW.dept_nm5, 40),
                LEFT(NEW.agg_category1_cd, 12), LEFT(NEW.agg_category1_nm, 40),
                LEFT(NEW.agg_category2_cd, 12), LEFT(NEW.agg_category2_nm, 40),
                LEFT(NEW.agg_category3_cd, 12), LEFT(NEW.agg_category3_nm, 40),
                LEFT(NEW.remarks, 100),
                CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            );
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        IF OLD.dept_cd <> NEW.dept_cd THEN
            UPDATE m_bcat SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
            WHERE bcat1_cd = OLD.dept_cd AND history_f IS NOT TRUE;

            SELECT COALESCE(MAX(bcat_id), 0) + 1 INTO v_bcat_id FROM m_bcat;

            INSERT INTO m_bcat (
                bcat_id, bcat1_cd, bcat1_nm,
                bcat2_cd, bcat2_nm, bcat3_cd, bcat3_nm,
                bcat4_cd, bcat4_nm, bcat5_cd, bcat5_nm,
                sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
                biko, create_dt, update_dt
            ) VALUES (
                v_bcat_id,
                LEFT(NEW.dept_cd, 12), LEFT(NEW.dept_nm, 80),
                LEFT(NEW.dept_cd2, 12), LEFT(NEW.dept_nm2, 40),
                LEFT(NEW.dept_cd3, 12), LEFT(NEW.dept_nm3, 40),
                LEFT(NEW.dept_cd4, 12), LEFT(NEW.dept_nm4, 40),
                LEFT(NEW.dept_cd5, 12), LEFT(NEW.dept_nm5, 40),
                LEFT(NEW.agg_category1_cd, 12), LEFT(NEW.agg_category1_nm, 40),
                LEFT(NEW.agg_category2_cd, 12), LEFT(NEW.agg_category2_nm, 40),
                LEFT(NEW.agg_category3_cd, 12), LEFT(NEW.agg_category3_nm, 40),
                LEFT(NEW.remarks, 100),
                CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            );
        ELSE
            UPDATE m_bcat SET
                bcat1_nm = LEFT(NEW.dept_nm, 80),
                bcat2_cd = LEFT(NEW.dept_cd2, 12),
                bcat2_nm = LEFT(NEW.dept_nm2, 40),
                bcat3_cd = LEFT(NEW.dept_cd3, 12),
                bcat3_nm = LEFT(NEW.dept_nm3, 40),
                bcat4_cd = LEFT(NEW.dept_cd4, 12),
                bcat4_nm = LEFT(NEW.dept_nm4, 40),
                bcat5_cd = LEFT(NEW.dept_cd5, 12),
                bcat5_nm = LEFT(NEW.dept_nm5, 40),
                sum1_cd  = LEFT(NEW.agg_category1_cd, 12),
                sum1_nm  = LEFT(NEW.agg_category1_nm, 40),
                sum2_cd  = LEFT(NEW.agg_category2_cd, 12),
                sum2_nm  = LEFT(NEW.agg_category2_nm, 40),
                sum3_cd  = LEFT(NEW.agg_category3_cd, 12),
                sum3_nm  = LEFT(NEW.agg_category3_nm, 40),
                biko     = LEFT(NEW.remarks, 100),
                update_dt = CURRENT_TIMESTAMP
            WHERE bcat1_cd = NEW.dept_cd AND history_f IS NOT TRUE;
        END IF;

    ELSIF TG_OP = 'DELETE' THEN
        UPDATE m_bcat SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
        WHERE bcat1_cd = OLD.dept_cd AND history_f IS NOT TRUE;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN OLD;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- m_bcat → m_department 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_bcat_to_department()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_cost_nm VARCHAR(100);
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- history_f = TRUE → m_department側を削除
    IF NEW.history_f IS TRUE THEN
        IF NEW.bcat1_cd IS NOT NULL THEN
            DELETE FROM m_department WHERE dept_cd = LEFT(NEW.bcat1_cd, 10);
        END IF;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    IF NEW.bcat1_cd IS NULL THEN
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    -- genk_id → cost_category_nm（m_genkから名称を取得）
    IF NEW.genk_id IS NOT NULL THEN
        SELECT genk_nm INTO v_cost_nm FROM m_genk WHERE genk_id = NEW.genk_id;
    END IF;

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_department (
            dept_cd, dept_nm, dept_cd2, dept_nm2, dept_cd3, dept_nm3,
            dept_cd4, dept_nm4, dept_cd5, dept_nm5,
            cost_category_nm,
            agg_category1_cd, agg_category1_nm,
            agg_category2_cd, agg_category2_nm,
            agg_category3_cd, agg_category3_nm,
            remarks
        ) VALUES (
            LEFT(NEW.bcat1_cd, 10),
            COALESCE(NEW.bcat1_nm, ''),
            LEFT(NEW.bcat2_cd, 10), NEW.bcat2_nm,
            LEFT(NEW.bcat3_cd, 10), NEW.bcat3_nm,
            LEFT(NEW.bcat4_cd, 10), NEW.bcat4_nm,
            LEFT(NEW.bcat5_cd, 10), NEW.bcat5_nm,
            v_cost_nm,
            LEFT(NEW.sum1_cd, 10), NEW.sum1_nm,
            LEFT(NEW.sum2_cd, 10), NEW.sum2_nm,
            LEFT(NEW.sum3_cd, 10), NEW.sum3_nm,
            NEW.biko
        )
        ON CONFLICT (dept_cd) DO UPDATE SET
            dept_nm          = EXCLUDED.dept_nm,
            dept_cd2         = EXCLUDED.dept_cd2,
            dept_nm2         = EXCLUDED.dept_nm2,
            dept_cd3         = EXCLUDED.dept_cd3,
            dept_nm3         = EXCLUDED.dept_nm3,
            dept_cd4         = EXCLUDED.dept_cd4,
            dept_nm4         = EXCLUDED.dept_nm4,
            dept_cd5         = EXCLUDED.dept_cd5,
            dept_nm5         = EXCLUDED.dept_nm5,
            cost_category_nm = EXCLUDED.cost_category_nm,
            agg_category1_cd = EXCLUDED.agg_category1_cd,
            agg_category1_nm = EXCLUDED.agg_category1_nm,
            agg_category2_cd = EXCLUDED.agg_category2_cd,
            agg_category2_nm = EXCLUDED.agg_category2_nm,
            agg_category3_cd = EXCLUDED.agg_category3_cd,
            agg_category3_nm = EXCLUDED.agg_category3_nm,
            remarks          = EXCLUDED.remarks,
            update_dt        = CURRENT_TIMESTAMP;

        -- PK変更時
        IF TG_OP = 'UPDATE' AND OLD.bcat1_cd IS NOT NULL AND OLD.bcat1_cd <> NEW.bcat1_cd THEN
            DELETE FROM m_department WHERE dept_cd = LEFT(OLD.bcat1_cd, 10);
        END IF;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- トリガー定義
-- ============================================================

-- m_department → m_bcat
DROP TRIGGER IF EXISTS trg_department_to_bcat ON m_department;
CREATE TRIGGER trg_department_to_bcat
    AFTER INSERT OR UPDATE OR DELETE ON m_department
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_department_to_bcat();

COMMENT ON TRIGGER trg_department_to_bcat ON m_department
    IS 'm_department変更時にm_bcatへ自動同期';

-- m_bcat → m_department
DROP TRIGGER IF EXISTS trg_bcat_to_department ON m_bcat;
CREATE TRIGGER trg_bcat_to_department
    AFTER INSERT OR UPDATE ON m_bcat
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_bcat_to_department();

COMMENT ON TRIGGER trg_bcat_to_department ON m_bcat
    IS 'm_bcat変更時にm_departmentへ自動同期';

COMMIT;
