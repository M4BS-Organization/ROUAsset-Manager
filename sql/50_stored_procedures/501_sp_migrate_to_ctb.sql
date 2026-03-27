-- ============================================================
-- 501_sp_migrate_to_ctb.sql
-- d_kykh → ctb_lease_integrated + ctb_contract_header
--        + ctb_contract_property + ctb_dept_allocation
-- ストアドプロシージャ版（UPSERT対応・差分同期可能）
--
-- Phase B Task B-1
-- 依存: 10_ddl_core/103_data_tables.sql (d_kykh, d_kykm)
--       10_ddl_core/102_master_tables.sql (m_kknri, m_bkind)
--       30_ddl_newlease/303_ctb_tables.sql (ctb_lease_integrated, ctb_contract_header,
--                                           ctb_contract_property, ctb_dept_allocation)
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
    header_count    integer,
    alloc_count     integer,
    error_count     integer,
    errors          text[]
)
LANGUAGE plpgsql AS $$
DECLARE
    v_inserted   integer := 0;
    v_updated    integer := 0;
    v_skipped    integer := 0;
    v_hdr_count  integer := 0;
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
            k.kykbnj,
            k.kykbnl,
            k.kyak_nm,
            k.kkbn_id,
            k.lcpt_id,
            k.kknri_id,
            k.kyak_dt,
            k.start_dt,
            k.end_dt,
            k.lkikan,
            k.rng_bango,
            k.shonin_dt,
            k.kiansha,
            k.jencho_f,
            k.k_glsryo,
            k.k_klsryo,
            k.k_slsryo,
            k.zritu,
            k.saikaisu,
            k.kyak_end_f,
            COALESCE(CAST(k.shri_kn AS INTEGER), 1)  AS payment_interval_months,
            kn.kknri1_cd                              AS mgmt_dept_cd,
            -- d_kykm（物件明細）から各種カラムを取得（最初の1物件）
            m.kykm_id,
            m.bukn_nm,
            m.bukn_bango1,
            m.bkind_id,
            m.skmk_id,
            m.b_suuryo,
            m.b_knyukn,
            m.b_glsryo,
            m.b_klsryo,
            m.b_bcat_id,
            m.kari_ritu,
            COALESCE(bk.asset_category_cd, 'AC05')    AS asset_category_cd
        FROM d_kykh k
        LEFT JOIN m_kknri kn ON k.kknri_id = kn.kknri_id
        LEFT JOIN LATERAL (
            SELECT kykm.kykm_id,
                   kykm.bukn_nm, kykm.bkind_id, kykm.bukn_bango1,
                   kykm.skmk_id, kykm.b_suuryo, kykm.b_knyukn,
                   kykm.b_glsryo, kykm.b_klsryo, kykm.b_bcat_id,
                   kykm.kari_ritu,
                   kykm.setti_dt, kykm.b_kedaban
            FROM d_kykm kykm
            WHERE kykm.kykh_id = k.kykh_id
            ORDER BY kykm.kykm_no
            LIMIT 1
        ) m ON TRUE
        LEFT JOIN m_bkind bk ON m.bkind_id = bk.bkind_id
        WHERE k.k_history_f IS NOT TRUE
          AND k.kykbnj IS NOT NULL
          AND (p_contract_no IS NULL OR k.kykbnj = p_contract_no)
    LOOP
        BEGIN
            -- ============================================================
            -- 1. ctb_lease_integrated UPSERT
            --    定義書準拠カラムのみ
            -- ============================================================
            INSERT INTO ctb_lease_integrated (
                contract_no,
                property_no,
                lease_start_date,
                non_cancellable_months,
                accounting_lease_term,
                periodic_payment_amt,
                payment_interval_months,
                discount_rate,
                kykh_id
            ) VALUES (
                v_rec.kykbnj,
                1,
                v_rec.start_dt,
                CAST(v_rec.lkikan AS INTEGER),
                CAST(v_rec.lkikan AS INTEGER),
                CAST(v_rec.k_glsryo AS NUMERIC(15,0)),
                v_rec.payment_interval_months,
                COALESCE(v_rec.kari_ritu, 0.0300),
                v_rec.kykh_id
            )
            ON CONFLICT (contract_no, property_no) DO UPDATE SET
                lease_start_date        = EXCLUDED.lease_start_date,
                non_cancellable_months  = EXCLUDED.non_cancellable_months,
                accounting_lease_term   = EXCLUDED.accounting_lease_term,
                periodic_payment_amt    = EXCLUDED.periodic_payment_amt,
                payment_interval_months = EXCLUDED.payment_interval_months,
                discount_rate           = EXCLUDED.discount_rate,
                kykh_id                 = EXCLUDED.kykh_id,
                update_dt               = CURRENT_TIMESTAMP
            RETURNING ctb_id,
                (xmax = 0) AS is_new  -- xmax=0 → INSERT, otherwise UPDATE
            INTO v_ctb_id, v_is_new;

            IF v_is_new THEN
                v_inserted := v_inserted + 1;
            ELSE
                v_updated := v_updated + 1;
            END IF;

            -- ============================================================
            -- 2. ctb_contract_header UPSERT
            -- ============================================================
            INSERT INTO ctb_contract_header (
                ctb_id,
                kykh_id,
                kykbnj,
                kykbnl,
                kyak_nm,
                kkbn_id,
                lcpt_id,
                kknri_id,
                kyak_dt,
                start_dt,
                end_dt,
                lkikan,
                rng_bango,
                shonin_dt,
                kiansha,
                jencho_f,
                k_glsryo,
                k_klsryo,
                k_slsryo,
                zritu,
                saikaisu,
                kyak_end_f
            ) VALUES (
                v_ctb_id,
                v_rec.kykh_id,
                v_rec.kykbnj,
                v_rec.kykbnl,
                v_rec.kyak_nm,
                v_rec.kkbn_id,
                v_rec.lcpt_id,
                v_rec.kknri_id,
                v_rec.kyak_dt,
                v_rec.start_dt,
                v_rec.end_dt,
                v_rec.lkikan,
                v_rec.rng_bango,
                v_rec.shonin_dt,
                v_rec.kiansha,
                v_rec.jencho_f,
                v_rec.k_glsryo,
                v_rec.k_klsryo,
                v_rec.k_slsryo,
                v_rec.zritu,
                v_rec.saikaisu,
                v_rec.kyak_end_f
            )
            ON CONFLICT (ctb_id) DO UPDATE SET
                kykh_id    = EXCLUDED.kykh_id,
                kykbnj     = EXCLUDED.kykbnj,
                kykbnl     = EXCLUDED.kykbnl,
                kyak_nm    = EXCLUDED.kyak_nm,
                kkbn_id    = EXCLUDED.kkbn_id,
                lcpt_id    = EXCLUDED.lcpt_id,
                kknri_id   = EXCLUDED.kknri_id,
                kyak_dt    = EXCLUDED.kyak_dt,
                start_dt   = EXCLUDED.start_dt,
                end_dt     = EXCLUDED.end_dt,
                lkikan     = EXCLUDED.lkikan,
                rng_bango  = EXCLUDED.rng_bango,
                shonin_dt  = EXCLUDED.shonin_dt,
                kiansha    = EXCLUDED.kiansha,
                jencho_f   = EXCLUDED.jencho_f,
                k_glsryo   = EXCLUDED.k_glsryo,
                k_klsryo   = EXCLUDED.k_klsryo,
                k_slsryo   = EXCLUDED.k_slsryo,
                zritu      = EXCLUDED.zritu,
                saikaisu   = EXCLUDED.saikaisu,
                kyak_end_f = EXCLUDED.kyak_end_f,
                update_dt  = CURRENT_TIMESTAMP;

            v_hdr_count := v_hdr_count + 1;

            -- ============================================================
            -- 3. ctb_contract_property UPSERT
            -- ============================================================
            INSERT INTO ctb_contract_property (
                ctb_id,
                property_no,
                bukn_nm,
                bukn_bango1,
                bkind_id,
                skmk_id,
                b_suuryo,
                b_knyukn,
                b_glsryo,
                b_klsryo,
                b_bcat_id,
                asset_category_cd,
                setti_dt,
                b_kedaban
            ) VALUES (
                v_ctb_id,
                1,
                v_rec.bukn_nm,
                v_rec.bukn_bango1,
                v_rec.bkind_id,
                v_rec.skmk_id,
                v_rec.b_suuryo,
                v_rec.b_knyukn,
                v_rec.b_glsryo,
                v_rec.b_klsryo,
                v_rec.b_bcat_id,
                COALESCE(v_rec.asset_category_cd, 'AC01'),
                v_rec.setti_dt,
                v_rec.b_kedaban
            )
            ON CONFLICT (ctb_id, property_no) DO UPDATE SET
                bukn_nm           = EXCLUDED.bukn_nm,
                bukn_bango1       = EXCLUDED.bukn_bango1,
                bkind_id          = EXCLUDED.bkind_id,
                skmk_id           = EXCLUDED.skmk_id,
                b_suuryo          = EXCLUDED.b_suuryo,
                b_knyukn          = EXCLUDED.b_knyukn,
                b_glsryo          = EXCLUDED.b_glsryo,
                b_klsryo          = EXCLUDED.b_klsryo,
                b_bcat_id         = EXCLUDED.b_bcat_id,
                asset_category_cd = EXCLUDED.asset_category_cd,
                setti_dt          = EXCLUDED.setti_dt,
                b_kedaban         = EXCLUDED.b_kedaban,
                update_dt         = CURRENT_TIMESTAMP;

            -- ============================================================
            -- 4. ctb_dept_allocation（d_haif → 物件単位の配賦明細同期）
            -- ============================================================
            DELETE FROM ctb_dept_allocation WHERE ctb_id = v_ctb_id;

            INSERT INTO ctb_dept_allocation (
                ctb_id, line_id, h_bcat_id, dept_cd, haifritu, hkmk_id,
                h_klsryo, h_mlsryo, h_kzei, h_mzei, h_klsryo_zkomi, h_mlsryo_zkomi,
                rsrvh1_id
            )
            SELECT
                v_ctb_id,
                h.line_id,
                h.h_bcat_id,
                COALESCE(b.bcat1_cd, '000'),
                COALESCE(h.haifritu, 100.00),
                h.hkmk_id,
                h.h_klsryo, h.h_mlsryo, h.h_kzei, h.h_mzei,
                h.h_klsryo_zkomi, h.h_mlsryo_zkomi,
                h.rsrvh1_id
            FROM d_haif h
            LEFT JOIN m_bcat b ON h.h_bcat_id = b.bcat_id AND b.history_f IS NOT TRUE
            WHERE h.kykh_id = v_rec.kykh_id
              AND h.kykm_id = v_rec.kykm_id;

            GET DIAGNOSTICS v_alloc_count = v_alloc_count + ROW_COUNT;

            -- d_haif にデータがない場合は管理部門から1行作成
            IF NOT FOUND AND v_rec.mgmt_dept_cd IS NOT NULL THEN
                INSERT INTO ctb_dept_allocation (
                    ctb_id, line_id, dept_cd, haifritu
                ) VALUES (
                    v_ctb_id, 1, v_rec.mgmt_dept_cd, 100.00
                );
                v_alloc_count := v_alloc_count + 1;
            END IF;

        EXCEPTION WHEN OTHERS THEN
            -- 個別レコードのエラーはログに記録して続行
            v_err_count := v_err_count + 1;
            v_errors := array_append(v_errors,
                format('contract_no=%s, kykh_id=%s: %s',
                    v_rec.kykbnj, v_rec.kykh_id, SQLERRM));

            -- pg_notifyでエラー通知
            PERFORM pg_notify('migration_error', json_build_object(
                'contract_no', v_rec.kykbnj,
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
        v_hdr_count, v_alloc_count,
        v_err_count, v_errors;
END;
$$;

COMMENT ON FUNCTION sp_migrate_d_kykh_to_ctb(VARCHAR)
    IS 'd_kykh→ctb_lease_integrated/ctb_contract_header/ctb_contract_property/ctb_dept_allocation差分同期SP';

COMMIT;
