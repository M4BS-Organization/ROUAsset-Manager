-- ============================================================
-- 402_sync_lcpt_supplier.sql
-- m_lcpt <-> m_supplier 双方向同期トリガー
--
-- Phase A Task A-3
-- 依存: 10_ddl_core/102_master_tables.sql (m_lcpt)
--       30_ddl_newlease/301_master_tables.sql (m_supplier)
--       40_views_triggers/401_sync_kkbn_contract_type.sql (is_sync_in_progress)
--
-- カラムマッピング:
--   m_lcpt.lcpt1_cd       ↔ m_supplier.supplier_cd
--   m_lcpt.lcpt1_nm       ↔ m_supplier.supplier_nm
--   m_lcpt.lcpt2_cd       ↔ m_supplier.supplier_cd2
--   m_lcpt.lcpt2_nm       ↔ m_supplier.supplier_nm2
--   m_lcpt.shime_day_1    ↔ m_supplier.row1_contract_closing_day
--   m_lcpt.sshri_kn1_1    ↔ m_supplier.row1_first_pay_months
--   m_lcpt.shri_day1_1    ↔ m_supplier.row1_first_pay_day
--   m_lcpt.sshri_kn2_1    ↔ m_supplier.row1_second_pay_months
--   m_lcpt.shri_day2_1    ↔ m_supplier.row1_second_pay_day
--   (row2, row3 も同パターン)
--   m_lcpt.sai_denomi     ↔ m_supplier.re_lease_param
--   m_lcpt.biko           ↔ m_supplier.remarks
--
-- 同期対象外（SQL1固有）:
--   sum1-3_cd/nm, shri_kn_ini, slkikan_s/n, shri_kn_s/n,
--   sai_denomi_s/n, sai_numerator_s/n, shri_cnt_s_1-3, shho_id_s/n_1-3
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 前提: m_lcpt.lcpt1_cd にアクティブ行のユニーク制約を追加
-- ============================================================
CREATE UNIQUE INDEX IF NOT EXISTS idx_m_lcpt_lcpt1_cd_active
    ON m_lcpt (lcpt1_cd)
    WHERE (history_f IS NOT TRUE);

-- ============================================================
-- m_supplier → m_lcpt 同期トリガー関数
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_supplier_to_lcpt()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_lcpt_id integer;
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' THEN
        -- lcpt1_cdが既存（アクティブ）の場合はUPDATE
        IF EXISTS (SELECT 1 FROM m_lcpt WHERE lcpt1_cd = NEW.supplier_cd AND history_f IS NOT TRUE) THEN
            UPDATE m_lcpt SET
                lcpt1_nm    = LEFT(NEW.supplier_nm, 40),
                lcpt2_cd    = LEFT(NEW.supplier_cd2, 12),
                lcpt2_nm    = LEFT(NEW.supplier_nm2, 40),
                shime_day_1 = NEW.row1_contract_closing_day,
                sshri_kn1_1 = NEW.row1_first_pay_months,
                shri_day1_1 = NEW.row1_first_pay_day,
                sshri_kn2_1 = NEW.row1_second_pay_months,
                shri_day2_1 = NEW.row1_second_pay_day,
                shime_day_2 = NEW.row2_contract_closing_day,
                sshri_kn1_2 = NEW.row2_first_pay_months,
                shri_day1_2 = NEW.row2_first_pay_day,
                sshri_kn2_2 = NEW.row2_second_pay_months,
                shri_day2_2 = NEW.row2_second_pay_day,
                shime_day_3 = NEW.row3_contract_closing_day,
                sshri_kn1_3 = NEW.row3_first_pay_months,
                shri_day1_3 = NEW.row3_first_pay_day,
                sshri_kn2_3 = NEW.row3_second_pay_months,
                shri_day2_3 = NEW.row3_second_pay_day,
                sai_denomi  = NEW.re_lease_param,
                biko        = LEFT(NEW.remarks, 200),
                update_dt   = CURRENT_TIMESTAMP
            WHERE lcpt1_cd = NEW.supplier_cd AND history_f IS NOT TRUE;
        ELSE
            SELECT COALESCE(MAX(lcpt_id), 0) + 1 INTO v_lcpt_id FROM m_lcpt;

            INSERT INTO m_lcpt (
                lcpt_id, lcpt1_cd, lcpt1_nm, lcpt2_cd, lcpt2_nm,
                shime_day_1, sshri_kn1_1, shri_day1_1, sshri_kn2_1, shri_day2_1,
                shime_day_2, sshri_kn1_2, shri_day1_2, sshri_kn2_2, shri_day2_2,
                shime_day_3, sshri_kn1_3, shri_day1_3, sshri_kn2_3, shri_day2_3,
                sai_denomi, biko, create_dt, update_dt
            ) VALUES (
                v_lcpt_id,
                LEFT(NEW.supplier_cd, 12),
                LEFT(NEW.supplier_nm, 40),
                LEFT(NEW.supplier_cd2, 12),
                LEFT(NEW.supplier_nm2, 40),
                NEW.row1_contract_closing_day, NEW.row1_first_pay_months,
                NEW.row1_first_pay_day, NEW.row1_second_pay_months, NEW.row1_second_pay_day,
                NEW.row2_contract_closing_day, NEW.row2_first_pay_months,
                NEW.row2_first_pay_day, NEW.row2_second_pay_months, NEW.row2_second_pay_day,
                NEW.row3_contract_closing_day, NEW.row3_first_pay_months,
                NEW.row3_first_pay_day, NEW.row3_second_pay_months, NEW.row3_second_pay_day,
                NEW.re_lease_param,
                LEFT(NEW.remarks, 200),
                CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            );
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        IF OLD.supplier_cd <> NEW.supplier_cd THEN
            -- PK変更: 旧レコード論理削除 + 新レコード作成
            UPDATE m_lcpt SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
            WHERE lcpt1_cd = OLD.supplier_cd AND history_f IS NOT TRUE;

            SELECT COALESCE(MAX(lcpt_id), 0) + 1 INTO v_lcpt_id FROM m_lcpt;

            INSERT INTO m_lcpt (
                lcpt_id, lcpt1_cd, lcpt1_nm, lcpt2_cd, lcpt2_nm,
                shime_day_1, sshri_kn1_1, shri_day1_1, sshri_kn2_1, shri_day2_1,
                shime_day_2, sshri_kn1_2, shri_day1_2, sshri_kn2_2, shri_day2_2,
                shime_day_3, sshri_kn1_3, shri_day1_3, sshri_kn2_3, shri_day2_3,
                sai_denomi, biko, create_dt, update_dt
            ) VALUES (
                v_lcpt_id,
                LEFT(NEW.supplier_cd, 12), LEFT(NEW.supplier_nm, 40),
                LEFT(NEW.supplier_cd2, 12), LEFT(NEW.supplier_nm2, 40),
                NEW.row1_contract_closing_day, NEW.row1_first_pay_months,
                NEW.row1_first_pay_day, NEW.row1_second_pay_months, NEW.row1_second_pay_day,
                NEW.row2_contract_closing_day, NEW.row2_first_pay_months,
                NEW.row2_first_pay_day, NEW.row2_second_pay_months, NEW.row2_second_pay_day,
                NEW.row3_contract_closing_day, NEW.row3_first_pay_months,
                NEW.row3_first_pay_day, NEW.row3_second_pay_months, NEW.row3_second_pay_day,
                NEW.re_lease_param, LEFT(NEW.remarks, 200),
                CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            );
        ELSE
            UPDATE m_lcpt SET
                lcpt1_nm    = LEFT(NEW.supplier_nm, 40),
                lcpt2_cd    = LEFT(NEW.supplier_cd2, 12),
                lcpt2_nm    = LEFT(NEW.supplier_nm2, 40),
                shime_day_1 = NEW.row1_contract_closing_day,
                sshri_kn1_1 = NEW.row1_first_pay_months,
                shri_day1_1 = NEW.row1_first_pay_day,
                sshri_kn2_1 = NEW.row1_second_pay_months,
                shri_day2_1 = NEW.row1_second_pay_day,
                shime_day_2 = NEW.row2_contract_closing_day,
                sshri_kn1_2 = NEW.row2_first_pay_months,
                shri_day1_2 = NEW.row2_first_pay_day,
                sshri_kn2_2 = NEW.row2_second_pay_months,
                shri_day2_2 = NEW.row2_second_pay_day,
                shime_day_3 = NEW.row3_contract_closing_day,
                sshri_kn1_3 = NEW.row3_first_pay_months,
                shri_day1_3 = NEW.row3_first_pay_day,
                sshri_kn2_3 = NEW.row3_second_pay_months,
                shri_day2_3 = NEW.row3_second_pay_day,
                sai_denomi  = NEW.re_lease_param,
                biko        = LEFT(NEW.remarks, 200),
                update_dt   = CURRENT_TIMESTAMP
            WHERE lcpt1_cd = NEW.supplier_cd AND history_f IS NOT TRUE;
        END IF;

    ELSIF TG_OP = 'DELETE' THEN
        -- m_supplier削除 → m_lcptは論理削除（d_kykh.lcpt_idがFK参照）
        UPDATE m_lcpt SET history_f = TRUE, update_dt = CURRENT_TIMESTAMP
        WHERE lcpt1_cd = OLD.supplier_cd AND history_f IS NOT TRUE;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN OLD;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- m_lcpt → m_supplier 同期トリガー関数
-- 共通カラムのみ同期（SQL1固有カラムは無視）
-- ============================================================
CREATE OR REPLACE FUNCTION trg_sync_lcpt_to_supplier()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- history_f = TRUE → m_supplier側を削除
    IF NEW.history_f IS TRUE THEN
        IF NEW.lcpt1_cd IS NOT NULL THEN
            DELETE FROM m_supplier WHERE supplier_cd = LEFT(NEW.lcpt1_cd, 10);
        END IF;
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    -- lcpt1_cdがNULLの場合はスキップ
    IF NEW.lcpt1_cd IS NULL THEN
        PERFORM set_config('sync.in_progress', 'false', TRUE);
        RETURN NEW;
    END IF;

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_supplier (
            supplier_cd, supplier_nm, supplier_cd2, supplier_nm2,
            row1_contract_closing_day, row1_first_pay_months, row1_first_pay_day,
            row1_second_pay_months, row1_second_pay_day,
            row2_contract_closing_day, row2_first_pay_months, row2_first_pay_day,
            row2_second_pay_months, row2_second_pay_day,
            row3_contract_closing_day, row3_first_pay_months, row3_first_pay_day,
            row3_second_pay_months, row3_second_pay_day,
            re_lease_param, remarks
        ) VALUES (
            LEFT(NEW.lcpt1_cd, 10),
            COALESCE(NEW.lcpt1_nm, ''),
            LEFT(NEW.lcpt2_cd, 10),
            NEW.lcpt2_nm,
            NEW.shime_day_1, NEW.sshri_kn1_1, NEW.shri_day1_1,
            NEW.sshri_kn2_1, NEW.shri_day2_1,
            NEW.shime_day_2, NEW.sshri_kn1_2, NEW.shri_day1_2,
            NEW.sshri_kn2_2, NEW.shri_day2_2,
            NEW.shime_day_3, NEW.sshri_kn1_3, NEW.shri_day1_3,
            NEW.sshri_kn2_3, NEW.shri_day2_3,
            NEW.sai_denomi,
            LEFT(NEW.biko, 500)
        )
        ON CONFLICT (supplier_cd) DO UPDATE SET
            supplier_nm               = EXCLUDED.supplier_nm,
            supplier_cd2              = EXCLUDED.supplier_cd2,
            supplier_nm2              = EXCLUDED.supplier_nm2,
            row1_contract_closing_day = EXCLUDED.row1_contract_closing_day,
            row1_first_pay_months     = EXCLUDED.row1_first_pay_months,
            row1_first_pay_day        = EXCLUDED.row1_first_pay_day,
            row1_second_pay_months    = EXCLUDED.row1_second_pay_months,
            row1_second_pay_day       = EXCLUDED.row1_second_pay_day,
            row2_contract_closing_day = EXCLUDED.row2_contract_closing_day,
            row2_first_pay_months     = EXCLUDED.row2_first_pay_months,
            row2_first_pay_day        = EXCLUDED.row2_first_pay_day,
            row2_second_pay_months    = EXCLUDED.row2_second_pay_months,
            row2_second_pay_day       = EXCLUDED.row2_second_pay_day,
            row3_contract_closing_day = EXCLUDED.row3_contract_closing_day,
            row3_first_pay_months     = EXCLUDED.row3_first_pay_months,
            row3_first_pay_day        = EXCLUDED.row3_first_pay_day,
            row3_second_pay_months    = EXCLUDED.row3_second_pay_months,
            row3_second_pay_day       = EXCLUDED.row3_second_pay_day,
            re_lease_param            = EXCLUDED.re_lease_param,
            remarks                   = EXCLUDED.remarks,
            update_dt                 = CURRENT_TIMESTAMP;

        -- PK変更時: 旧supplier削除
        IF TG_OP = 'UPDATE' AND OLD.lcpt1_cd IS NOT NULL AND OLD.lcpt1_cd <> NEW.lcpt1_cd THEN
            DELETE FROM m_supplier WHERE supplier_cd = LEFT(OLD.lcpt1_cd, 10);
        END IF;
    END IF;

    PERFORM set_config('sync.in_progress', 'false', TRUE);
    RETURN NEW;
END;
$$;

-- ============================================================
-- トリガー定義
-- ============================================================

-- m_supplier → m_lcpt
DROP TRIGGER IF EXISTS trg_supplier_to_lcpt ON m_supplier;
CREATE TRIGGER trg_supplier_to_lcpt
    AFTER INSERT OR UPDATE OR DELETE ON m_supplier
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_supplier_to_lcpt();

COMMENT ON TRIGGER trg_supplier_to_lcpt ON m_supplier
    IS 'm_supplier変更時にm_lcptへ自動同期';

-- m_lcpt → m_supplier
DROP TRIGGER IF EXISTS trg_lcpt_to_supplier ON m_lcpt;
CREATE TRIGGER trg_lcpt_to_supplier
    AFTER INSERT OR UPDATE ON m_lcpt
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_lcpt_to_supplier();

COMMENT ON TRIGGER trg_lcpt_to_supplier ON m_lcpt
    IS 'm_lcpt変更時にm_supplierへ自動同期';

COMMIT;
