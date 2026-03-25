-- ============================================================
-- 306_v_contract_mapping.sql
-- 契約統合ビュー (v_contract_full)
--
-- マイグレーション側 (d_kykh) と新リース側 (ctb_lease_integrated,
-- ctb_contract_header) を結合し、契約の全体像を単一ビューで提供する。
--
-- 結合構成:
--   d_kykh                 (契約ヘッダ - マイグレーション側)
--   ctb_lease_integrated   (会計計算 - 仕様側)
--   ctb_contract_header    (契約基本情報 - 新リース側)
--   c_kkbn                 (契約区分マスタ)
--   m_lcpt                 (リース会社マスタ)
--   m_kknri                (管理部門マスタ)
--
-- 依存:
--   10_ddl_core/101_code_tables.sql   (c_kkbn)
--   10_ddl_core/102_master_tables.sql (m_lcpt, m_kknri)
--   10_ddl_core/103_data_tables.sql   (d_kykh)
--   30_ddl_newlease/303_ctb_tables.sql (ctb_lease_integrated, ctb_contract_header)
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- 契約統合ビュー (v_contract_full)
-- d_kykh を起点に、CTB テーブル群およびマスタを LEFT JOIN し、
-- 現行契約（履歴でないもの）の全情報を一覧できるようにする。
-- ============================================================

DROP VIEW IF EXISTS v_contract_full;

CREATE OR REPLACE VIEW v_contract_full AS
SELECT
    -- === d_kykh: 契約ヘッダ（マイグレーション側） ===
    k.kykh_id,
    k.kykbnj                    AS contract_no,
    k.kykbnl                    AS contract_no_lessor,
    k.kyak_nm                   AS contract_name,
    k.kyak_dt                   AS contract_date,
    k.start_dt,
    k.end_dt,
    k.lkikan                    AS lease_months,
    k.rng_bango                 AS approval_no,
    k.shonin_dt                 AS approval_date,
    k.kiansha                   AS applicant,
    k.jencho_f                  AS is_extended,
    k.k_glsryo                  AS monthly_payment,

    -- === マスタテーブル: 名称展開 ===
    kb.kkbn_nm                  AS contract_type_name,
    lc.lcpt1_nm                 AS supplier_name,
    kn.kknri1_nm                AS mgmt_dept_name,

    -- === ctb_lease_integrated: 会計計算（仕様側） ===
    c.ctb_id,
    c.non_cancellable_months,
    c.accounting_lease_term,
    c.discount_rate,
    c.initial_rou_asset,
    c.initial_lease_liability,

    -- === ctb_contract_header: 新リース固有項目 ===
    h.free_rent_months

FROM
    d_kykh k

    -- CTB 統合テーブル (kykh_id で逆参照)
    LEFT JOIN ctb_lease_integrated c
        ON c.kykh_id = k.kykh_id

    -- CTB 契約基本情報 (ctb_id で結合)
    LEFT JOIN ctb_contract_header h
        ON h.ctb_id = c.ctb_id

    -- 契約区分マスタ
    LEFT JOIN c_kkbn kb
        ON kb.kkbn_id = k.kkbn_id

    -- リース会社マスタ
    LEFT JOIN m_lcpt lc
        ON lc.lcpt_id = k.lcpt_id

    -- 管理部門マスタ
    LEFT JOIN m_kknri kn
        ON kn.kknri_id = k.kknri_id

WHERE
    k.k_history_f IS NOT TRUE
;

COMMENT ON VIEW v_contract_full IS '契約統合ビュー: d_kykh + CTB + マスタを結合した現行契約一覧';

COMMIT;
