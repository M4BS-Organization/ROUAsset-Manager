-- ============================================================
-- 501_sp_migrate_to_ctb.sql
-- d_kykh → ctb_lease_integrated + ctb_property + ctb_dept_allocation
-- ストアドプロシージャ版（UPSERT対応・差分同期可能）
--
-- Phase B Task B-1
-- 依存: 10_ddl_core/103_data_tables.sql (d_kykh)
--       10_ddl_core/102_master_tables.sql (m_kknri, m_lcpt)
--       30_ddl_newlease/303_ctb_tables.sql (ctb_lease_integrated, ctb_dept_allocation)
--       30_ddl_newlease/304_ctb_property_eav.sql (ctb_property)
--       40_views_triggers/402_sync_lcpt_supplier.sql (m_supplier同期済み前提)
--       40_views_triggers/403_sync_bcat_department.sql (m_department同期済み前提)
--
-- 使用方法:
--   SELECT * FROM sp_migrate_d_kykh_to_ctb();
--   SELECT * FROM sp_migrate_d_kykh_to_ctb('K00001');  -- 特定契約のみ
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- メイン関数
-- ============================================================
CREATE OR REPLACE FUNCTION sp_migrate_d_kykh_to_ctb(
    p_contract_no VARCHAR DEFAULT NULL  -- NULLの場合は全件処理
)
RETURNS TABLE (
    inserted_count  integer,
    updated_count   integer,
    skipped_count   integer,
    property_count  integer,
    alloc_count     integer,
    error_count     integer,
    errors          text[]
)
LANGUAGE plpgsql AS $$
DECLARE
    v_inserted   integer := 0;
    v_updated    integer := 0;
    v_skipped    integer := 0;
    v_prop_count integer := 0;
    v_alloc_count integer := 0;
    v_err_count  integer := 0;
    v_errors     text[] := '{}';
    v_rec        RECORD;
    v_ctb_id     integer;
    v_is_new     boolean;
BEGIN
    -- ============================================================
    -- d_kykh から対象レコードをループ処理
    -- ============================================================
    FOR v_rec IN
        SELECT
            k.kykh_id,
            k.kykbnj                                         AS contract_no,
            k.kyak_nm                                        AS contract_name,
            LPAD(CAST(k.kkbn_id AS VARCHAR), 2, '0')        AS contract_type_cd,
            lc.lcpt1_cd                                      AS supplier_cd,
            kn.kknri1_cd                                     AS mgmt_dept_cd,
            CAST(k.start_dt AS DATE)                         AS lease_start_date,
            CAST(k.end_dt AS DATE)                           AS lease_end_date,
            CAST(k.lkikan AS INTEGER)                        AS lease_term_months,
            CAST(k.k_glsryo AS NUMERIC(15,2))               AS monthly_payment,
            CAST(k.k_slsryo AS NUMERIC(15,2))               AS total_payment,
            k.kykbnl,
            k.rng_bango
        FROM d_kykh k
        LEFT JOIN m_kknri kn ON k.kknri_id = kn.kknri_id
        LEFT JOIN m_lcpt lc ON k.lcpt_id = lc.lcpt_id
        WHERE k.k_history_f IS NOT TRUE
          AND k.kykbnj IS NOT NULL
          AND (p_contract_no IS NULL OR k.kykbnj = p_contract_no)
    LOOP
        BEGIN
            -- ============================================================
            -- 1. ctb_lease_integrated UPSERT
            -- ============================================================
            INSERT INTO ctb_lease_integrated (
                contract_no,
                property_no,
                contract_name,
                contract_type_cd,
                supplier_cd,
                mgmt_dept_cd,
                lease_start_date,
                lease_end_date,
                free_rent_months,
                lease_term_months,
                asset_name,
                monthly_payment,
                total_payment,
                remarks
            ) VALUES (
                v_rec.contract_no,
                1,
                v_rec.contract_name,
                v_rec.contract_type_cd,
                v_rec.supplier_cd,
                v_rec.mgmt_dept_cd,
                v_rec.lease_start_date,
                v_rec.lease_end_date,
                0,
                v_rec.lease_term_months,
                v_rec.contract_name,  -- asset_name: 契約名で仮設定
                v_rec.monthly_payment,
                v_rec.total_payment,
                CONCAT_WS(' | ',
                    CASE WHEN v_rec.kykbnl IS NOT NULL THEN '相手方番号:' || v_rec.kykbnl END,
                    CASE WHEN v_rec.rng_bango IS NOT NULL THEN '稟議番号:' || v_rec.rng_bango END
                )
            )
            ON CONFLICT (contract_no, property_no) DO UPDATE SET
                contract_name    = EXCLUDED.contract_name,
                contract_type_cd = EXCLUDED.contract_type_cd,
                supplier_cd      = EXCLUDED.supplier_cd,
                mgmt_dept_cd     = EXCLUDED.mgmt_dept_cd,
                lease_start_date = EXCLUDED.lease_start_date,
                lease_end_date   = EXCLUDED.lease_end_date,
                lease_term_months = EXCLUDED.lease_term_months,
                asset_name       = EXCLUDED.asset_name,
                monthly_payment  = EXCLUDED.monthly_payment,
                total_payment    = EXCLUDED.total_payment,
                remarks          = EXCLUDED.remarks,
                update_dt        = CURRENT_TIMESTAMP
            RETURNING ctb_id,
                (xmax = 0) AS is_new  -- xmax=0 → INSERT, otherwise UPDATE
            INTO v_ctb_id, v_is_new;

            IF v_is_new THEN
                v_inserted := v_inserted + 1;
            ELSE
                v_updated := v_updated + 1;
            END IF;

            -- ============================================================
            -- 2. ctb_property 自動作成（新規INSERTの場合のみ）
            -- ============================================================
            IF v_is_new THEN
                INSERT INTO ctb_property (
                    ctb_id,
                    property_no,
                    asset_category_cd,
                    asset_name,
                    remarks
                ) VALUES (
                    v_ctb_id,
                    1,
                    'AC01',  -- デフォルト: 建物（後からUI変更可能）
                    NULL,    -- 資産名は未設定（資産詳細入力画面から入力）
                    '移行データ: 資産詳細入力画面から編集してください'
                )
                ON CONFLICT (ctb_id, property_no) DO NOTHING;

                v_prop_count := v_prop_count + 1;
            END IF;

            -- ============================================================
            -- 3. ctb_dept_allocation UPSERT
            -- ============================================================
            IF v_rec.mgmt_dept_cd IS NOT NULL THEN
                INSERT INTO ctb_dept_allocation (
                    ctb_id,
                    dept_cd,
                    allocation_ratio,
                    payment_amount,
                    remarks
                ) VALUES (
                    v_ctb_id,
                    v_rec.mgmt_dept_cd,
                    100.00,
                    v_rec.monthly_payment,
                    CASE WHEN v_is_new THEN 'マイグレーション自動作成' ELSE '差分同期更新' END
                )
                ON CONFLICT (ctb_id, dept_cd) DO UPDATE SET
                    allocation_ratio = EXCLUDED.allocation_ratio,
                    payment_amount   = EXCLUDED.payment_amount,
                    remarks          = EXCLUDED.remarks,
                    update_dt        = CURRENT_TIMESTAMP;

                v_alloc_count := v_alloc_count + 1;
            END IF;

        EXCEPTION WHEN OTHERS THEN
            -- 個別レコードのエラーはログに記録して続行
            v_err_count := v_err_count + 1;
            v_errors := array_append(v_errors,
                format('contract_no=%s, kykh_id=%s: %s',
                    v_rec.contract_no, v_rec.kykh_id, SQLERRM));

            -- pg_notifyでエラー通知
            PERFORM pg_notify('migration_error', json_build_object(
                'contract_no', v_rec.contract_no,
                'kykh_id', v_rec.kykh_id,
                'error', SQLERRM,
                'ts', CURRENT_TIMESTAMP
            )::text);
        END;
    END LOOP;

    -- ============================================================
    -- 結果返却
    -- ============================================================
    RETURN QUERY SELECT
        v_inserted, v_updated, v_skipped,
        v_prop_count, v_alloc_count,
        v_err_count, v_errors;
END;
$$;

COMMENT ON FUNCTION sp_migrate_d_kykh_to_ctb(VARCHAR)
    IS 'd_kykh→ctb_lease_integrated/ctb_property/ctb_dept_allocation差分同期SP';

COMMIT;
