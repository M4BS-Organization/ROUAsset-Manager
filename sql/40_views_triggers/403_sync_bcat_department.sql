-- ============================================================
-- 403_sync_bcat_department.sql
-- m_bcat <-> m_department 双方向同期トリガー + 統合ビュー
--
-- 依存: 10_ddl_core/102_master_tables.sql (m_bcat)
--       30_ddl_newlease/301_master_tables.sql (m_department)
--       40_views_triggers/401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--
-- カラムマッピング（1:1 直接対応）:
--   m_bcat.bcat1_cd/nm  ↔ m_department.dept_cd/nm
--   m_bcat.bcat2_cd/nm  ↔ m_department.dept_cd2/nm2
--   m_bcat.bcat3_cd/nm  ↔ m_department.dept_cd3/nm3
--   m_bcat.bcat4_cd/nm  ↔ m_department.dept_cd4/nm4
--   m_bcat.bcat5_cd/nm  ↔ m_department.dept_cd5/nm5
--   m_bcat.genk_id      ↔ m_department.genk_id
--   m_bcat.skti_id      ↔ m_department.skti_id
--   m_bcat.sum1_cd/nm   ↔ m_department.sum1_cd/nm
--   m_bcat.sum2_cd/nm   ↔ m_department.sum2_cd/nm
--   m_bcat.sum3_cd/nm   ↔ m_department.sum3_cd/nm
--   m_bcat.bknri_id     ↔ m_department.bknri_id
--   m_bcat.kbf_kb/fb/sb ↔ m_department.kbf_kb/fb/sb
--   m_bcat.gensonf      ↔ m_department.gensonf
--   m_bcat.biko         ↔ m_department.biko
--   m_bcat.update_cnt   ↔ m_department.update_cnt
--   m_bcat.history_f    ↔ m_department.history_f
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
-- 統合ビュー: m_bcat と m_department の統合参照
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
    d.genk_id,
    d.skti_id,
    d.sum1_cd,
    d.sum1_nm,
    d.sum2_cd,
    d.sum2_nm,
    d.sum3_cd,
    d.sum3_nm,
    d.bknri_id,
    d.kbf_kb,
    d.kbf_fb,
    d.kbf_sb,
    d.gensonf,
    d.biko,
    d.update_cnt,
    d.history_f,
    -- m_bcat側の追加情報
    b.bcat_id
FROM m_department d
LEFT JOIN m_bcat b ON b.bcat1_cd = d.dept_cd AND b.history_f IS NOT TRUE;

COMMENT ON VIEW v_unified_department IS 'm_bcat と m_department の統合ビュー（カラム1:1対応）';

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
                bcat1_nm  = NEW.dept_nm,
                bcat2_cd  = NEW.dept_cd2,  bcat2_nm = NEW.dept_nm2,
                bcat3_cd  = NEW.dept_cd3,  bcat3_nm = NEW.dept_nm3,
                bcat4_cd  = NEW.dept_cd4,  bcat4_nm = NEW.dept_nm4,
                bcat5_cd  = NEW.dept_cd5,  bcat5_nm = NEW.dept_nm5,
                genk_id   = NEW.genk_id,   skti_id  = NEW.skti_id,
                sum1_cd   = NEW.sum1_cd,   sum1_nm  = NEW.sum1_nm,
                sum2_cd   = NEW.sum2_cd,   sum2_nm  = NEW.sum2_nm,
                sum3_cd   = NEW.sum3_cd,   sum3_nm  = NEW.sum3_nm,
                bknri_id  = NEW.bknri_id,
                kbf_kb    = NEW.kbf_kb,    kbf_fb   = NEW.kbf_fb,
                kbf_sb    = NEW.kbf_sb,    gensonf  = NEW.gensonf,
                biko      = NEW.biko,
                update_cnt = NEW.update_cnt,
                update_dt = CURRENT_TIMESTAMP
            WHERE bcat1_cd = NEW.dept_cd AND history_f IS NOT TRUE;
        ELSE
            SELECT COALESCE(MAX(bcat_id), 0) + 1 INTO v_bcat_id FROM m_bcat;
            INSERT INTO m_bcat (
                bcat_id, bcat1_cd, bcat1_nm,
                bcat2_cd, bcat2_nm, bcat3_cd, bcat3_nm,
                bcat4_cd, bcat4_nm, bcat5_cd, bcat5_nm,
                genk_id, skti_id,
                sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
                bknri_id, kbf_kb, kbf_fb, kbf_sb, gensonf,
                biko, update_cnt, create_dt, update_dt
            ) VALUES (
                v_bcat_id, NEW.dept_cd, NEW.dept_nm,
                NEW.dept_cd2, NEW.dept_nm2, NEW.dept_cd3, NEW.dept_nm3,
                NEW.dept_cd4, NEW.dept_nm4, NEW.dept_cd5, NEW.dept_nm5,
                NEW.genk_id, NEW.skti_id,
                NEW.sum1_cd, NEW.sum1_nm, NEW.sum2_cd, NEW.sum2_nm, NEW.sum3_cd, NEW.sum3_nm,
                NEW.bknri_id, NEW.kbf_kb, NEW.kbf_fb, NEW.kbf_sb, NEW.gensonf,
                NEW.biko, NEW.update_cnt, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
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
                genk_id, skti_id,
                sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
                bknri_id, kbf_kb, kbf_fb, kbf_sb, gensonf,
                biko, update_cnt, create_dt, update_dt
            ) VALUES (
                v_bcat_id, NEW.dept_cd, NEW.dept_nm,
                NEW.dept_cd2, NEW.dept_nm2, NEW.dept_cd3, NEW.dept_nm3,
                NEW.dept_cd4, NEW.dept_nm4, NEW.dept_cd5, NEW.dept_nm5,
                NEW.genk_id, NEW.skti_id,
                NEW.sum1_cd, NEW.sum1_nm, NEW.sum2_cd, NEW.sum2_nm, NEW.sum3_cd, NEW.sum3_nm,
                NEW.bknri_id, NEW.kbf_kb, NEW.kbf_fb, NEW.kbf_sb, NEW.gensonf,
                NEW.biko, NEW.update_cnt, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            );
        ELSE
            UPDATE m_bcat SET
                bcat1_nm  = NEW.dept_nm,
                bcat2_cd  = NEW.dept_cd2,  bcat2_nm = NEW.dept_nm2,
                bcat3_cd  = NEW.dept_cd3,  bcat3_nm = NEW.dept_nm3,
                bcat4_cd  = NEW.dept_cd4,  bcat4_nm = NEW.dept_nm4,
                bcat5_cd  = NEW.dept_cd5,  bcat5_nm = NEW.dept_nm5,
                genk_id   = NEW.genk_id,   skti_id  = NEW.skti_id,
                sum1_cd   = NEW.sum1_cd,   sum1_nm  = NEW.sum1_nm,
                sum2_cd   = NEW.sum2_cd,   sum2_nm  = NEW.sum2_nm,
                sum3_cd   = NEW.sum3_cd,   sum3_nm  = NEW.sum3_nm,
                bknri_id  = NEW.bknri_id,
                kbf_kb    = NEW.kbf_kb,    kbf_fb   = NEW.kbf_fb,
                kbf_sb    = NEW.kbf_sb,    gensonf  = NEW.gensonf,
                biko      = NEW.biko,
                update_cnt = NEW.update_cnt,
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
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- history_f = TRUE → m_department側を削除
    IF NEW.history_f IS TRUE THEN
        IF NEW.bcat1_cd IS NOT NULL THEN
            DELETE FROM m_department WHERE dept_cd = NEW.bcat1_cd;
        END IF;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    IF NEW.bcat1_cd IS NULL THEN
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_department (
            dept_cd, dept_nm,
            dept_cd2, dept_nm2, dept_cd3, dept_nm3,
            dept_cd4, dept_nm4, dept_cd5, dept_nm5,
            genk_id, skti_id,
            sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
            bknri_id, kbf_kb, kbf_fb, kbf_sb, gensonf,
            biko, update_cnt, history_f
        ) VALUES (
            NEW.bcat1_cd, COALESCE(NEW.bcat1_nm, ''),
            NEW.bcat2_cd, NEW.bcat2_nm, NEW.bcat3_cd, NEW.bcat3_nm,
            NEW.bcat4_cd, NEW.bcat4_nm, NEW.bcat5_cd, NEW.bcat5_nm,
            NEW.genk_id, NEW.skti_id,
            NEW.sum1_cd, NEW.sum1_nm, NEW.sum2_cd, NEW.sum2_nm, NEW.sum3_cd, NEW.sum3_nm,
            NEW.bknri_id, NEW.kbf_kb, NEW.kbf_fb, NEW.kbf_sb, NEW.gensonf,
            NEW.biko, NEW.update_cnt, NEW.history_f
        )
        ON CONFLICT (dept_cd) DO UPDATE SET
            dept_nm    = EXCLUDED.dept_nm,
            dept_cd2   = EXCLUDED.dept_cd2,   dept_nm2  = EXCLUDED.dept_nm2,
            dept_cd3   = EXCLUDED.dept_cd3,   dept_nm3  = EXCLUDED.dept_nm3,
            dept_cd4   = EXCLUDED.dept_cd4,   dept_nm4  = EXCLUDED.dept_nm4,
            dept_cd5   = EXCLUDED.dept_cd5,   dept_nm5  = EXCLUDED.dept_nm5,
            genk_id    = EXCLUDED.genk_id,    skti_id   = EXCLUDED.skti_id,
            sum1_cd    = EXCLUDED.sum1_cd,    sum1_nm   = EXCLUDED.sum1_nm,
            sum2_cd    = EXCLUDED.sum2_cd,    sum2_nm   = EXCLUDED.sum2_nm,
            sum3_cd    = EXCLUDED.sum3_cd,    sum3_nm   = EXCLUDED.sum3_nm,
            bknri_id   = EXCLUDED.bknri_id,
            kbf_kb     = EXCLUDED.kbf_kb,     kbf_fb    = EXCLUDED.kbf_fb,
            kbf_sb     = EXCLUDED.kbf_sb,     gensonf   = EXCLUDED.gensonf,
            biko       = EXCLUDED.biko,
            update_cnt = EXCLUDED.update_cnt,
            history_f  = EXCLUDED.history_f,
            update_dt  = CURRENT_TIMESTAMP;

        -- PK変更時
        IF TG_OP = 'UPDATE' AND OLD.bcat1_cd IS NOT NULL AND OLD.bcat1_cd <> NEW.bcat1_cd THEN
            DELETE FROM m_department WHERE dept_cd = OLD.bcat1_cd;
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
