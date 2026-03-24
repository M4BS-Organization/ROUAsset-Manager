# SQL統合分析 - 推奨実装計画書

## 1. 推奨アプローチ概要

### 選定: アプローチ3（ビュー統合 + 段階的マスタ統合）

**選定理由:**

1. **安全性**: 既存パイプライン（`KeijoSqlBuilder` → `d_kykh`/`d_kykm` → 仕訳出力）に一切の変更を加えない
2. **整合性**: マスタ二重管理をビュー＋トリガーで解消し、データ不整合リスクを排除
3. **段階性**: 各フェーズにロールバックポイントを設けた漸進的移行
4. **互換性**: 現在の `DbConnectionManager`（単一DB接続）をそのまま活用
5. **拡張性**: 将来の完全統合（アプローチ2）への移行パスを確保

---

## 2. 推奨アーキテクチャ

### 2.1 現状アーキテクチャ

```
┌─────────────────────────────────────────────────────────────────────────┐
│  VB.NET DataAccess 層                                                   │
│  ┌──────────────────┐  ┌───────────────────┐  ┌─────────────────────┐  │
│  │ KeijoSqlBuilder  │  │ MasterDataLoader  │  │ AuthorizationService│  │
│  │ (d_kykh/d_kykm)  │  │ (m_department等)  │  │ (tm_USER)          │  │
│  └────────┬─────────┘  └────────┬──────────┘  └─────────┬───────────┘  │
│           │                     │                       │              │
│           └─────────────────────┼───────────────────────┘              │
│                                 │                                      │
│                    DbConnectionManager (単一接続)                       │
│                    Host=localhost:5432                                  │
└─────────────────────┬───────────────────────────────────────────────────┘
                      │
    ┌─────────────────┴──────────────────┐
    │                                    │
    ▼                                    ▼
┌──────────────────────┐    ┌──────────────────────────┐
│ SQL1系 (000_init.sql)│    │ SQL2系 (init.sql)        │
│ DB: lease_m4bs_dev   │    │ DB: lease_m4bs           │
│ User: lease_m4bs_user│    │ User: manager            │
├──────────────────────┤    ├──────────────────────────┤
│ d_kykh  (契約ヘッダ) │    │ tw_lease_contract        │
│ d_kykm  (契約明細)   │    │ tw_lease_property        │
│ d_haif  (配賦)       │    │ tw_lease_accounting      │
│ d_gson  (減損)       │    │ tw_lease_judgment        │
│ d_henl  (変更リース) │    │ tw_lease_journal         │
│ m_lcpt  (リース会社) │    │ ctb_lease_integrated     │
│ m_kknri (管理単位)   │    │ ctb_property (EAV)       │
│ m_bcat  (管理部署)   │    │ ctb_dept_allocation      │
│ m_corp  (法人)       │    │ m_department             │
│ c_kkbn  (契約区分)   │    │ m_supplier               │
│ sec_user(ユーザー)   │    │ m_contract_type          │
│ tw_m_user(tm_USER)   │    │ m_company                │
└──────────────────────┘    │ m_asset_category         │
                            └──────────────────────────┘
```

**問題点:**
- マスタが2系統に重複（5箇所）
- 契約データが3系統に分散
- DB接続情報が不統一（DB名・ユーザーが異なる）
- `migrate_d_kykh_to_ctb.sql` がワンショット実行のみ

### 2.2 目標アーキテクチャ（Phase C完了後）

```
┌─────────────────────────────────────────────────────────────────────────┐
│  VB.NET DataAccess 層                                                   │
│  ┌──────────────────┐  ┌───────────────────┐  ┌─────────────────────┐  │
│  │ KeijoSqlBuilder  │  │ MasterDataLoader  │  │ AuthorizationService│  │
│  │ (d_kykh/d_kykm)  │  │ (v_unified_* )   │  │ (v_unified_user)   │  │
│  │   ※変更なし      │  │  ※ビュー参照     │  │  ※ビュー参照       │  │
│  └────────┬─────────┘  └────────┬──────────┘  └─────────┬───────────┘  │
│           └─────────────────────┼───────────────────────┘              │
│                                 │                                      │
│                    DbConnectionManager (単一接続)                       │
│                    Host=localhost:5432                                  │
│                    DB=lease_m4bs (統一)                                 │
└─────────────────────┬───────────────────────────────────────────────────┘
                      │
                      ▼
┌──────────────────────────────────────────────────────────────────────────┐
│  統合DB: lease_m4bs                                                      │
│  User: lease_m4bs_user                                                   │
│                                                                          │
│  ┌───────────────────── 統合ビュー層 ──────────────────────┐             │
│  │  v_unified_department  (m_bcat ∪ m_department)          │             │
│  │  v_unified_supplier    (m_lcpt ∪ m_supplier)            │             │
│  │  v_unified_contract_type (c_kkbn ∪ m_contract_type)     │             │
│  │  v_unified_company     (m_corp ∪ m_company)             │             │
│  │  v_unified_user        (sec_user ∪ tw_m_user)           │             │
│  └──────────────────────────────────────────────────────────┘             │
│                                                                          │
│  ┌─── SQL1系テーブル ──┐  ┌─── SQL2系テーブル ──────────────┐            │
│  │ (既存顧客/仕訳用)   │  │ (新リース対応)                  │            │
│  │                      │  │                                │            │
│  │ d_kykh, d_kykm       │◄─── sync_ctb_to_d_kykh() ────┐  │            │
│  │ d_haif, d_gson       │  │ ctb_lease_integrated ───────┘  │            │
│  │ m_lcpt ◄──┐          │  │ ctb_property (EAV)             │            │
│  │ m_bcat    │  trg_sync │  │ tw_lease_*                     │            │
│  │ m_corp    │          │  │ m_department ──► trg_sync       │            │
│  │ c_kkbn ◄──┘          │  │ m_supplier  ──► trg_sync       │            │
│  │ sec_user ◄──┐        │  │ m_contract_type ► trg_sync     │            │
│  │ tw_m_user ──┘        │  │ m_company   ──► trg_sync       │            │
│  └──────────────────────┘  └────────────────────────────────┘            │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## 3. 実装ロードマップ

### Phase A: マスタ統合基盤（1-2週間）

| 優先度 | タスク | 説明 | 工数目安 |
|--------|--------|------|----------|
| P0 | DB接続情報の統一 | `000_init.sql` と `init.sql` を統合し、単一DB/ユーザーに | 0.5日 |
| P0 | c_kkbn ↔ m_contract_type 双方向同期 | 契約区分の同期トリガー | 1日 |
| P0 | m_lcpt ↔ m_supplier 双方向同期 | リース会社/取引先の同期トリガー | 1日 |
| P1 | m_bcat ↔ m_department 統合ビュー | 管理部署の統合ビュー | 1日 |
| P1 | m_corp ↔ m_company 統合ビュー | 法人/会社の統合ビュー | 0.5日 |
| P1 | sec_user / tw_m_user 認証統合 | ユーザー管理の統合ビュー | 1日 |

**Phase A 完了条件:**
- 全マスタの二重管理が解消されている
- `KeijoSqlBuilder` が変更なしで動作する
- `MasterDataLoader` がビュー経由で正しくデータを返す

### Phase B: データフロー最適化（2-4週間）

| 優先度 | タスク | 説明 | 工数目安 |
|--------|--------|------|----------|
| P0 | migrate_d_kykh_to_ctb ストアドプロシージャ化 | ワンショット→定期実行可能に | 2日 |
| P1 | 新規契約のSQL2系直接投入 | tw_lease_contract + ctb_lease_integrated 経由 | 3日 |
| P1 | MasterDataLoader ビュー参照オプション | 統合ビュー対応の切り替えフラグ | 1日 |
| P2 | 既存契約のSQL1系残存管理 | d_kykh/d_kykm はリードオンリー化の準備 | 2日 |

**Phase B 完了条件:**
- 新規契約がSQL2系に直接投入されている
- 既存契約はSQL1系で引き続き仕訳出力可能
- データ同期が自動化されている

### Phase C: 長期統合（将来 - 数ヶ月～1年）

| 優先度 | タスク | 説明 | 工数目安 |
|--------|--------|------|----------|
| P1 | SQL1テーブルの読み取り専用ビュー化 | d_kykh/d_kykm → ビューに段階変換 | 数週間 |
| P2 | KeijoSqlBuilder の段階的リファクタリング | ctb_lease_integrated ベースに書き換え | 数ヶ月 |
| P3 | 最終統合 | アプローチ2相当の完全統合状態 | - |

---

## 4. 具体的なSQL実装例

### 4.1 DB接続情報の統一（Phase A - P0）

```sql
-- ============================================================
-- 統合初期化スクリプト: 000_init_unified.sql
-- 既存の 000_init.sql と init.sql を統合
-- psql -U postgres -f 000_init_unified.sql
-- ============================================================

-- ユーザー統一: lease_m4bs_user を正とする
DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'lease_m4bs_user') THEN
        CREATE ROLE lease_m4bs_user WITH
            LOGIN
            PASSWORD 'iltex_mega_pass_m4'
            NOSUPERUSER NOCREATEDB NOCREATEROLE INHERIT;
    END IF;
END
$$;

-- 旧 manager ユーザーが存在する場合、権限を移譲
DO $$
BEGIN
    IF EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'manager') THEN
        GRANT manager TO lease_m4bs_user;
    END IF;
END
$$;

-- 統合データベース: lease_m4bs を使用（init.sql 側に合わせる）
SELECT 'CREATE DATABASE lease_m4bs OWNER lease_m4bs_user ENCODING ''UTF8'''
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'lease_m4bs')
\gexec

GRANT ALL PRIVILEGES ON DATABASE lease_m4bs TO lease_m4bs_user;

\connect lease_m4bs

-- 全テーブルの所有権を移譲
DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN SELECT tablename FROM pg_tables WHERE schemaname = 'public' LOOP
        EXECUTE 'ALTER TABLE public.' || quote_ident(r.tablename) || ' OWNER TO lease_m4bs_user';
    END LOOP;
END
$$;

GRANT ALL ON SCHEMA public TO lease_m4bs_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO lease_m4bs_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO lease_m4bs_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON FUNCTIONS TO lease_m4bs_user;
```

**VB.NET側の変更（`DbConnectionManager.vb` のデフォルト接続文字列）:**

```
変更前: "Host=localhost;Port=5432;Database=lease_m4bs;Username=lease_m4bs_user;Password=iltex_mega_pass_m4"
変更後: 変更不要（既に lease_m4bs を指定しているため、DB統合側で合わせる）
```

> 注: `000_init.sql` の `lease_m4bs_dev` を開発環境として残す場合は、`App.config` の接続文字列名で環境を切り替える方式とする。

### 4.2 c_kkbn ↔ m_contract_type 双方向同期トリガー（Phase A - P0）

```sql
-- ============================================================
-- c_kkbn ↔ m_contract_type 双方向同期トリガー
-- ============================================================
-- カラムマッピング:
--   c_kkbn.kkbn_id (smallint)  ←→  m_contract_type.contract_type_cd (varchar(10))
--   c_kkbn.kkbn_nm (varchar50) ←→  m_contract_type.contract_type_nm (varchar100)
--   変換: kkbn_id → LPAD(kkbn_id::text, 2, '0') = contract_type_cd
-- ============================================================

-- ■ m_contract_type → c_kkbn 方向の同期
CREATE OR REPLACE FUNCTION trg_sync_contract_type_to_kkbn()
RETURNS TRIGGER AS $$
DECLARE
    v_kkbn_id SMALLINT;
BEGIN
    -- contract_type_cd ('01','02',...) → kkbn_id (1,2,...)
    BEGIN
        v_kkbn_id := CAST(NEW.contract_type_cd AS SMALLINT);
    EXCEPTION WHEN OTHERS THEN
        -- 数値変換できない場合はスキップ
        RAISE NOTICE 'trg_sync_contract_type_to_kkbn: 変換不能 contract_type_cd=%', NEW.contract_type_cd;
        RETURN NEW;
    END;

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO c_kkbn (kkbn_id, kkbn_nm)
        VALUES (v_kkbn_id, NEW.contract_type_nm)
        ON CONFLICT (kkbn_id) DO UPDATE
            SET kkbn_nm = EXCLUDED.kkbn_nm;
    ELSIF TG_OP = 'DELETE' THEN
        -- 削除は安全のため行わない（仕訳パイプラインが参照する可能性）
        RAISE NOTICE 'trg_sync_contract_type_to_kkbn: DELETE は同期対象外（kkbn_id=%）', v_kkbn_id;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_m_contract_type_sync_to_kkbn
    AFTER INSERT OR UPDATE ON m_contract_type
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_contract_type_to_kkbn();

-- ■ c_kkbn → m_contract_type 方向の同期
CREATE OR REPLACE FUNCTION trg_sync_kkbn_to_contract_type()
RETURNS TRIGGER AS $$
DECLARE
    v_contract_type_cd VARCHAR(10);
BEGIN
    v_contract_type_cd := LPAD(NEW.kkbn_id::TEXT, 2, '0');

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_contract_type (contract_type_cd, contract_type_nm, sort_order)
        VALUES (v_contract_type_cd, NEW.kkbn_nm, NEW.kkbn_id)
        ON CONFLICT (contract_type_cd) DO UPDATE
            SET contract_type_nm = EXCLUDED.contract_type_nm;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_c_kkbn_sync_to_contract_type
    AFTER INSERT OR UPDATE ON c_kkbn
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_kkbn_to_contract_type();

-- ■ 統合ビュー（読み取り用）
CREATE OR REPLACE VIEW v_unified_contract_type AS
SELECT
    ct.contract_type_cd,
    ct.contract_type_nm,
    ct.sort_order,
    ck.kkbn_id,
    ck.kkbn_nm AS kkbn_nm_original,
    CASE
        WHEN ck.kkbn_id IS NOT NULL THEN TRUE
        ELSE FALSE
    END AS has_legacy_mapping
FROM m_contract_type ct
LEFT JOIN c_kkbn ck ON LPAD(ck.kkbn_id::TEXT, 2, '0') = ct.contract_type_cd;

COMMENT ON VIEW v_unified_contract_type IS '契約区分統合ビュー: m_contract_type を正とし、c_kkbn とのマッピングを含む';
```

### 4.3 m_lcpt ↔ m_supplier 双方向同期トリガー（Phase A - P0）

```sql
-- ============================================================
-- m_lcpt ↔ m_supplier 双方向同期トリガー
-- ============================================================
-- カラムマッピング:
--   m_lcpt.lcpt_id (integer)           ←  内部ID（自動採番）
--   m_lcpt.lcpt1_cd (varchar12)        ←→ m_supplier.supplier_cd (varchar10)
--   m_lcpt.lcpt1_nm (varchar40)        ←→ m_supplier.supplier_nm (varchar100)
--   m_lcpt.lcpt2_cd (varchar12)        ←→ m_supplier.supplier_cd2 (varchar10)
--   m_lcpt.lcpt2_nm (varchar40)        ←→ m_supplier.supplier_nm2 (varchar100)
--   m_lcpt.shime_day_1 (smallint)      ←→ m_supplier.row1_contract_closing_day
--   m_lcpt.sshri_kn1_1 (smallint)      ←→ m_supplier.row1_first_pay_months
--   m_lcpt.shri_day1_1 (smallint)      ←→ m_supplier.row1_first_pay_day
--   m_lcpt.sshri_kn2_1 (smallint)      ←→ m_supplier.row1_second_pay_months
--   m_lcpt.shri_day2_1 (smallint)      ←→ m_supplier.row1_second_pay_day
--   （row2, row3 も同様にshime_day_2/3系とマッピング）
--   m_lcpt.sai_denomi (smallint)       ←→ m_supplier.re_lease_param
--   m_lcpt.biko (varchar200)           ←→ m_supplier.remarks (varchar500)
-- ============================================================

-- ■ m_supplier → m_lcpt 方向の同期
CREATE OR REPLACE FUNCTION trg_sync_supplier_to_lcpt()
RETURNS TRIGGER AS $$
DECLARE
    v_lcpt_id INTEGER;
BEGIN
    -- supplier_cd = lcpt1_cd でマッチング
    SELECT lcpt_id INTO v_lcpt_id FROM m_lcpt WHERE lcpt1_cd = NEW.supplier_cd;

    IF v_lcpt_id IS NOT NULL THEN
        -- 既存レコード更新
        UPDATE m_lcpt SET
            lcpt1_nm     = LEFT(NEW.supplier_nm, 40),
            lcpt2_cd     = NEW.supplier_cd2,
            lcpt2_nm     = LEFT(NEW.supplier_nm2, 40),
            shime_day_1  = NEW.row1_contract_closing_day,
            sshri_kn1_1  = NEW.row1_first_pay_months,
            shri_day1_1  = NEW.row1_first_pay_day,
            sshri_kn2_1  = NEW.row1_second_pay_months,
            shri_day2_1  = NEW.row1_second_pay_day,
            shime_day_2  = NEW.row2_contract_closing_day,
            sshri_kn1_2  = NEW.row2_first_pay_months,
            shri_day1_2  = NEW.row2_first_pay_day,
            sshri_kn2_2  = NEW.row2_second_pay_months,
            shri_day2_2  = NEW.row2_second_pay_day,
            shime_day_3  = NEW.row3_contract_closing_day,
            sshri_kn1_3  = NEW.row3_first_pay_months,
            shri_day1_3  = NEW.row3_first_pay_day,
            sshri_kn2_3  = NEW.row3_second_pay_months,
            shri_day2_3  = NEW.row3_second_pay_day,
            sai_denomi   = NEW.re_lease_param,
            biko         = LEFT(NEW.remarks, 200),
            update_dt    = CURRENT_TIMESTAMP
        WHERE lcpt_id = v_lcpt_id;
    ELSE
        -- 新規レコード挿入（lcpt_id は既存の最大値+1）
        INSERT INTO m_lcpt (
            lcpt_id, lcpt1_cd, lcpt1_nm, lcpt2_cd, lcpt2_nm,
            shime_day_1, sshri_kn1_1, shri_day1_1, sshri_kn2_1, shri_day2_1,
            shime_day_2, sshri_kn1_2, shri_day1_2, sshri_kn2_2, shri_day2_2,
            shime_day_3, sshri_kn1_3, shri_day1_3, sshri_kn2_3, shri_day2_3,
            sai_denomi, biko, create_dt, update_dt
        ) VALUES (
            COALESCE((SELECT MAX(lcpt_id) FROM m_lcpt), 0) + 1,
            NEW.supplier_cd,
            LEFT(NEW.supplier_nm, 40),
            NEW.supplier_cd2,
            LEFT(NEW.supplier_nm2, 40),
            NEW.row1_contract_closing_day, NEW.row1_first_pay_months,
            NEW.row1_first_pay_day, NEW.row1_second_pay_months, NEW.row1_second_pay_day,
            NEW.row2_contract_closing_day, NEW.row2_first_pay_months,
            NEW.row2_first_pay_day, NEW.row2_second_pay_months, NEW.row2_second_pay_day,
            NEW.row3_contract_closing_day, NEW.row3_first_pay_months,
            NEW.row3_first_pay_day, NEW.row3_second_pay_months, NEW.row3_second_pay_day,
            NEW.re_lease_param,
            LEFT(NEW.remarks, 200),
            CURRENT_TIMESTAMP,
            CURRENT_TIMESTAMP
        );
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_m_supplier_sync_to_lcpt
    AFTER INSERT OR UPDATE ON m_supplier
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_supplier_to_lcpt();

-- ■ m_lcpt → m_supplier 方向の同期
CREATE OR REPLACE FUNCTION trg_sync_lcpt_to_supplier()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.lcpt1_cd IS NULL OR NEW.lcpt1_cd = '' THEN
        RETURN NEW;
    END IF;

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
        NEW.lcpt1_cd,
        COALESCE(NEW.lcpt1_nm, ''),
        NEW.lcpt2_cd,
        NEW.lcpt2_nm,
        NEW.shime_day_1, NEW.sshri_kn1_1, NEW.shri_day1_1,
        NEW.sshri_kn2_1, NEW.shri_day2_1,
        NEW.shime_day_2, NEW.sshri_kn1_2, NEW.shri_day1_2,
        NEW.sshri_kn2_2, NEW.shri_day2_2,
        NEW.shime_day_3, NEW.sshri_kn1_3, NEW.shri_day1_3,
        NEW.sshri_kn2_3, NEW.shri_day2_3,
        NEW.sai_denomi,
        NEW.biko
    )
    ON CONFLICT (supplier_cd) DO UPDATE SET
        supplier_nm                = EXCLUDED.supplier_nm,
        supplier_cd2               = EXCLUDED.supplier_cd2,
        supplier_nm2               = EXCLUDED.supplier_nm2,
        row1_contract_closing_day  = EXCLUDED.row1_contract_closing_day,
        row1_first_pay_months      = EXCLUDED.row1_first_pay_months,
        row1_first_pay_day         = EXCLUDED.row1_first_pay_day,
        row1_second_pay_months     = EXCLUDED.row1_second_pay_months,
        row1_second_pay_day        = EXCLUDED.row1_second_pay_day,
        row2_contract_closing_day  = EXCLUDED.row2_contract_closing_day,
        row2_first_pay_months      = EXCLUDED.row2_first_pay_months,
        row2_first_pay_day         = EXCLUDED.row2_first_pay_day,
        row2_second_pay_months     = EXCLUDED.row2_second_pay_months,
        row2_second_pay_day        = EXCLUDED.row2_second_pay_day,
        row3_contract_closing_day  = EXCLUDED.row3_contract_closing_day,
        row3_first_pay_months      = EXCLUDED.row3_first_pay_months,
        row3_first_pay_day         = EXCLUDED.row3_first_pay_day,
        row3_second_pay_months     = EXCLUDED.row3_second_pay_months,
        row3_second_pay_day        = EXCLUDED.row3_second_pay_day,
        re_lease_param             = EXCLUDED.re_lease_param,
        remarks                    = EXCLUDED.remarks,
        update_dt                  = CURRENT_TIMESTAMP;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_m_lcpt_sync_to_supplier
    AFTER INSERT OR UPDATE ON m_lcpt
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_lcpt_to_supplier();

-- ■ 統合ビュー
CREATE OR REPLACE VIEW v_unified_supplier AS
SELECT
    s.supplier_cd,
    s.supplier_nm,
    s.supplier_cd2,
    s.supplier_nm2,
    s.row1_contract_closing_day,
    s.row1_first_pay_months,
    s.row1_first_pay_day,
    s.row1_second_pay_months,
    s.row1_second_pay_day,
    s.row2_contract_closing_day,
    s.row2_first_pay_months,
    s.row2_first_pay_day,
    s.row2_second_pay_months,
    s.row2_second_pay_day,
    s.row3_contract_closing_day,
    s.row3_first_pay_months,
    s.row3_first_pay_day,
    s.row3_second_pay_months,
    s.row3_second_pay_day,
    s.re_lease_param,
    s.remarks,
    lc.lcpt_id,
    CASE
        WHEN lc.lcpt_id IS NOT NULL THEN TRUE
        ELSE FALSE
    END AS has_legacy_mapping
FROM m_supplier s
LEFT JOIN m_lcpt lc ON lc.lcpt1_cd = s.supplier_cd;

COMMENT ON VIEW v_unified_supplier IS '取引先統合ビュー: m_supplier を正とし、m_lcpt とのマッピングを含む';
```

### 4.4 v_unified_department ビュー定義（Phase A - P1）

```sql
-- ============================================================
-- m_bcat ↔ m_department 統合ビュー
-- ============================================================
-- カラムマッピング:
--   m_bcat.bcat_id (integer)     ←  内部ID
--   m_bcat.bcat1_cd (varchar12)  ←→ m_department.dept_cd (varchar10)
--   m_bcat.bcat1_nm (varchar80)  ←→ m_department.dept_nm (varchar100)
--   m_bcat.bcat2_cd (varchar12)  ←→ m_department.dept_cd2 (varchar10)
--   m_bcat.bcat2_nm (varchar40)  ←→ m_department.dept_nm2 (varchar100)
--   m_bcat.bcat3_cd ~ bcat5_cd   ←→ m_department.dept_cd3 ~ dept_cd5
--   m_bcat.sum1_cd/sum1_nm       ←→ m_department.agg_category1_cd/nm
--   m_bcat.sum2_cd/sum2_nm       ←→ m_department.agg_category2_cd/nm
--   m_bcat.sum3_cd/sum3_nm       ←→ m_department.agg_category3_cd/nm
--   m_bcat.genk_id               ←→ m_department.cost_category_nm (間接参照)
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
    -- レガシー系フィールド
    bc.bcat_id,
    bc.genk_id,
    bc.skti_id,
    bc.bknri_id,
    bc.kbf_kb,
    bc.kbf_fb,
    bc.kbf_sb,
    bc.gensonf,
    CASE
        WHEN bc.bcat_id IS NOT NULL THEN TRUE
        ELSE FALSE
    END AS has_legacy_mapping,
    -- データソース判定
    CASE
        WHEN bc.bcat_id IS NOT NULL AND d.dept_cd IS NOT NULL THEN 'BOTH'
        WHEN bc.bcat_id IS NOT NULL THEN 'LEGACY_ONLY'
        ELSE 'NEW_ONLY'
    END AS data_source
FROM m_department d
FULL OUTER JOIN m_bcat bc ON bc.bcat1_cd = d.dept_cd
                          AND (bc.history_f IS NOT TRUE);

COMMENT ON VIEW v_unified_department IS
    '管理部署統合ビュー: m_department を正とし、m_bcat（レガシー）とのFULL OUTER JOINで全レコードを網羅';

-- ■ m_department → m_bcat 同期トリガー
CREATE OR REPLACE FUNCTION trg_sync_department_to_bcat()
RETURNS TRIGGER AS $$
DECLARE
    v_bcat_id INTEGER;
BEGIN
    SELECT bcat_id INTO v_bcat_id FROM m_bcat WHERE bcat1_cd = NEW.dept_cd AND history_f IS NOT TRUE;

    IF v_bcat_id IS NOT NULL THEN
        UPDATE m_bcat SET
            bcat1_nm   = LEFT(NEW.dept_nm, 80),
            bcat2_cd   = NEW.dept_cd2,
            bcat2_nm   = LEFT(NEW.dept_nm2, 40),
            bcat3_cd   = NEW.dept_cd3,
            bcat3_nm   = LEFT(NEW.dept_nm3, 40),
            bcat4_cd   = NEW.dept_cd4,
            bcat4_nm   = LEFT(NEW.dept_nm4, 40),
            bcat5_cd   = NEW.dept_cd5,
            bcat5_nm   = LEFT(NEW.dept_nm5, 40),
            sum1_cd    = NEW.agg_category1_cd,
            sum1_nm    = LEFT(NEW.agg_category1_nm, 40),
            sum2_cd    = NEW.agg_category2_cd,
            sum2_nm    = LEFT(NEW.agg_category2_nm, 40),
            sum3_cd    = NEW.agg_category3_cd,
            sum3_nm    = LEFT(NEW.agg_category3_nm, 40),
            biko       = LEFT(NEW.remarks, 100),
            update_dt  = CURRENT_TIMESTAMP
        WHERE bcat_id = v_bcat_id;
    ELSE
        INSERT INTO m_bcat (
            bcat_id, bcat1_cd, bcat1_nm, bcat2_cd, bcat2_nm,
            bcat3_cd, bcat3_nm, bcat4_cd, bcat4_nm, bcat5_cd, bcat5_nm,
            sum1_cd, sum1_nm, sum2_cd, sum2_nm, sum3_cd, sum3_nm,
            biko, create_dt, update_dt
        ) VALUES (
            COALESCE((SELECT MAX(bcat_id) FROM m_bcat), 0) + 1,
            NEW.dept_cd, LEFT(NEW.dept_nm, 80),
            NEW.dept_cd2, LEFT(NEW.dept_nm2, 40),
            NEW.dept_cd3, LEFT(NEW.dept_nm3, 40),
            NEW.dept_cd4, LEFT(NEW.dept_nm4, 40),
            NEW.dept_cd5, LEFT(NEW.dept_nm5, 40),
            NEW.agg_category1_cd, LEFT(NEW.agg_category1_nm, 40),
            NEW.agg_category2_cd, LEFT(NEW.agg_category2_nm, 40),
            NEW.agg_category3_cd, LEFT(NEW.agg_category3_nm, 40),
            LEFT(NEW.remarks, 100),
            CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
        );
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_m_department_sync_to_bcat
    AFTER INSERT OR UPDATE ON m_department
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_department_to_bcat();
```

### 4.5 m_corp ↔ m_company 統合ビュー（Phase A - P1）

```sql
-- ============================================================
-- m_corp ↔ m_company 統合ビュー
-- ============================================================
-- カラムマッピング:
--   m_corp.corp_id (integer)     ←  内部ID
--   m_corp.corp1_cd (varchar12)  ←→ m_company.company_cd (varchar10)
--   m_corp.corp1_nm (varchar40)  ←→ m_company.company_nm (varchar100)
--   m_corp.corp2_cd (varchar12)  ←→ m_company.company_cd2 (varchar10)
--   m_corp.corp2_nm (varchar40)  ←→ m_company.company_nm2 (varchar100)
--   m_corp.corp3_cd              ←→ m_company.company_cd3
--   m_corp.corp3_nm              ←→ m_company.company_nm3
--   m_corp.biko (varchar100)     ←→ m_company.remarks (varchar500)
-- ============================================================

CREATE OR REPLACE VIEW v_unified_company AS
SELECT
    c.company_cd,
    c.company_nm,
    c.company_cd2,
    c.company_nm2,
    c.company_cd3,
    c.company_nm3,
    c.remarks,
    cr.corp_id,
    CASE
        WHEN cr.corp_id IS NOT NULL THEN TRUE
        ELSE FALSE
    END AS has_legacy_mapping
FROM m_company c
LEFT JOIN m_corp cr ON cr.corp1_cd = c.company_cd
                   AND (cr.history_f IS NOT TRUE);

COMMENT ON VIEW v_unified_company IS '法人統合ビュー: m_company を正とし、m_corp とのマッピングを含む';

-- ■ 同期トリガー（m_company → m_corp）
CREATE OR REPLACE FUNCTION trg_sync_company_to_corp()
RETURNS TRIGGER AS $$
DECLARE
    v_corp_id INTEGER;
BEGIN
    SELECT corp_id INTO v_corp_id FROM m_corp WHERE corp1_cd = NEW.company_cd AND history_f IS NOT TRUE;

    IF v_corp_id IS NOT NULL THEN
        UPDATE m_corp SET
            corp1_nm   = LEFT(NEW.company_nm, 40),
            corp2_cd   = NEW.company_cd2,
            corp2_nm   = LEFT(NEW.company_nm2, 40),
            corp3_cd   = NEW.company_cd3,
            corp3_nm   = LEFT(NEW.company_nm3, 40),
            biko       = LEFT(NEW.remarks, 100),
            update_dt  = CURRENT_TIMESTAMP
        WHERE corp_id = v_corp_id;
    ELSE
        INSERT INTO m_corp (
            corp_id, corp1_cd, corp1_nm, corp2_cd, corp2_nm,
            corp3_cd, corp3_nm, biko, create_dt, update_dt
        ) VALUES (
            COALESCE((SELECT MAX(corp_id) FROM m_corp), 0) + 1,
            NEW.company_cd, LEFT(NEW.company_nm, 40),
            NEW.company_cd2, LEFT(NEW.company_nm2, 40),
            NEW.company_cd3, LEFT(NEW.company_nm3, 40),
            LEFT(NEW.remarks, 100),
            CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
        );
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_m_company_sync_to_corp
    AFTER INSERT OR UPDATE ON m_company
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_company_to_corp();
```

### 4.6 sec_user / tw_m_user 認証統合ビュー（Phase A - P1）

```sql
-- ============================================================
-- sec_user ↔ tw_m_user (tm_USER) 認証統合ビュー
-- ============================================================
-- カラムマッピング:
--   sec_user.user_id (integer)       ←→ tw_m_user.user_id (integer)
--   sec_user.user_cd (varchar12)     ←→ tw_m_user.login_id (varchar50) ※alter後
--   sec_user.user_nm (varchar40)     ←→ tw_m_user.user_name (varchar100)
--   sec_user.pwd (varchar255)        ←→ tw_m_user.password_hash (varchar256) ※alter後
--   sec_user.kngn_id                  →  tw_m_user.role ※間接マッピング
-- ============================================================

CREATE OR REPLACE VIEW v_unified_user AS
SELECT
    -- tw_m_user（認証拡張後）を正とする
    tu.user_id,
    tu.user_name,
    tu.user_kana,
    tu.login_id,
    tu.password_hash,
    tu.role,
    tu.is_active,
    tu.last_login_at,
    tu.failed_login_count,
    tu.locked_until,
    -- レガシーsec_userフィールド
    su.user_cd AS legacy_user_cd,
    su.kngn_id AS legacy_kngn_id,
    su.pwd_life_time,
    su.pwd_grace_time,
    su.pwd_min,
    su.pwd_moji_chk,
    su.pwd_alph_chk,
    su.pwd_num_chk,
    su.pwd_symbol_chk,
    CASE
        WHEN su.user_id IS NOT NULL THEN TRUE
        ELSE FALSE
    END AS has_legacy_mapping
FROM tw_m_user tu
LEFT JOIN sec_user su ON su.user_cd = tu.login_id
                     AND (su.history_f IS NOT TRUE);

COMMENT ON VIEW v_unified_user IS
    'ユーザー統合ビュー: tw_m_user（alter_tm_user適用済）を正とし、sec_user のパスワードポリシー情報を含む';
```

### 4.7 migrate_d_kykh_to_ctb ストアドプロシージャ版（Phase B - P0）

```sql
-- ============================================================
-- migrate_d_kykh_to_ctb ストアドプロシージャ版
-- ワンショット実行 → 定期実行（差分同期）対応
-- ============================================================

CREATE OR REPLACE FUNCTION sp_sync_d_kykh_to_ctb(
    p_full_sync BOOLEAN DEFAULT FALSE
)
RETURNS TABLE (
    inserted_count INTEGER,
    updated_count  INTEGER,
    skipped_count  INTEGER,
    error_count    INTEGER
) AS $$
DECLARE
    v_inserted INTEGER := 0;
    v_updated  INTEGER := 0;
    v_skipped  INTEGER := 0;
    v_errors   INTEGER := 0;
    v_last_sync TIMESTAMP;
    r RECORD;
BEGIN
    -- 最終同期日時を取得（初回はNULL→全件同期）
    IF NOT p_full_sync THEN
        SELECT MAX(update_dt) INTO v_last_sync
        FROM ctb_lease_integrated
        WHERE remarks LIKE '%sync_from_d_kykh%';
    END IF;

    -- d_kykh の対象レコードをループ
    FOR r IN
        SELECT
            k.kykh_id,
            k.kykbnj,
            k.kyak_nm,
            LPAD(CAST(k.kkbn_id AS VARCHAR), 2, '0') AS contract_type_cd,
            lc.lcpt1_cd AS supplier_cd,
            kn.kknri1_cd AS mgmt_dept_cd,
            CAST(k.start_dt AS DATE) AS lease_start_date,
            CAST(k.end_dt AS DATE) AS lease_end_date,
            CAST(k.lkikan AS INTEGER) AS lease_term_months,
            CAST(k.k_glsryo AS NUMERIC(15,2)) AS monthly_payment,
            CAST(k.k_slsryo AS NUMERIC(15,2)) AS total_payment,
            CONCAT_WS(' | ',
                '相手方番号:' || k.kykbnl,
                '稟議番号:' || k.rng_bango,
                'sync_from_d_kykh'
            ) AS remarks,
            k.k_update_dt
        FROM d_kykh k
        LEFT JOIN m_kknri kn ON k.kknri_id = kn.kknri_id
        LEFT JOIN m_lcpt lc ON k.lcpt_id = lc.lcpt_id
        WHERE k.k_history_f IS NOT TRUE
          AND (p_full_sync OR v_last_sync IS NULL OR k.k_update_dt > v_last_sync)
    LOOP
        BEGIN
            -- UPSERT
            INSERT INTO ctb_lease_integrated (
                contract_no, property_no, contract_name,
                contract_type_cd, supplier_cd, mgmt_dept_cd,
                lease_start_date, lease_end_date, free_rent_months,
                lease_term_months, asset_name, monthly_payment,
                total_payment, remarks
            ) VALUES (
                r.kykbnj, 1, r.kyak_nm,
                r.contract_type_cd, r.supplier_cd, r.mgmt_dept_cd,
                r.lease_start_date, r.lease_end_date, 0,
                r.lease_term_months, r.kyak_nm, r.monthly_payment,
                r.total_payment, r.remarks
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
                update_dt        = CURRENT_TIMESTAMP;

            IF FOUND THEN
                -- INSERT or UPDATE が成功
                GET DIAGNOSTICS v_inserted = ROW_COUNT;
                -- 簡易判定: 挿入 or 更新
                v_inserted := v_inserted + 1;
            ELSE
                v_skipped := v_skipped + 1;
            END IF;

        EXCEPTION WHEN OTHERS THEN
            v_errors := v_errors + 1;
            RAISE NOTICE 'sync error for kykh_id=%: %', r.kykh_id, SQLERRM;
        END;
    END LOOP;

    -- 配賦データの同期
    INSERT INTO ctb_dept_allocation (ctb_id, dept_cd, allocation_ratio, payment_amount)
    SELECT
        c.ctb_id,
        c.mgmt_dept_cd,
        100.00,
        c.monthly_payment
    FROM ctb_lease_integrated c
    WHERE c.mgmt_dept_cd IS NOT NULL
      AND NOT EXISTS (
        SELECT 1 FROM ctb_dept_allocation d
        WHERE d.ctb_id = c.ctb_id AND d.dept_cd = c.mgmt_dept_cd
    );

    -- 同期ログ記録
    RAISE NOTICE 'sp_sync_d_kykh_to_ctb 完了: inserted=%, updated=%, skipped=%, errors=%',
        v_inserted, v_updated, v_skipped, v_errors;

    RETURN QUERY SELECT v_inserted, v_updated, v_skipped, v_errors;
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION sp_sync_d_kykh_to_ctb IS
    'd_kykh → ctb_lease_integrated 差分同期プロシージャ。p_full_sync=TRUE で全件再同期。';

-- ■ 定期実行用（pg_cron 等で毎日実行）
-- SELECT * FROM sp_sync_d_kykh_to_ctb(FALSE);  -- 差分同期
-- SELECT * FROM sp_sync_d_kykh_to_ctb(TRUE);   -- 全件再同期
```

---

## 5. リスク軽減策

### 5.1 トリガーの信頼性テスト計画

| テスト項目 | テスト内容 | 期待結果 |
|-----------|-----------|---------|
| 正常系: INSERT同期 | m_supplierにINSERT → m_lcptに反映 | lcpt1_cd/lcpt1_nm が一致 |
| 正常系: UPDATE同期 | m_contract_typeをUPDATE → c_kkbnに反映 | kkbn_nm が更新済 |
| 異常系: 循環防止 | m_supplier→m_lcpt→m_supplierの循環が発生しないこと | トリガーが無限ループしない |
| 異常系: NULL処理 | NULLカラムの同期でエラーが発生しないこと | NULLが安全に伝播 |
| 異常系: 文字列長超過 | varchar(100)→varchar(40)への同期で切り捨て | LEFT()で安全に切り捨て |
| 性能: 一括INSERT | 1000件一括INSERTでトリガー性能劣化がないこと | 10秒以内に完了 |

**循環防止の実装:**

```sql
-- セッション変数で循環防止
CREATE OR REPLACE FUNCTION trg_sync_supplier_to_lcpt()
RETURNS TRIGGER AS $$
BEGIN
    -- 循環防止: 他のトリガーからの呼び出しかチェック
    IF current_setting('app.sync_in_progress', TRUE) = 'true' THEN
        RETURN NEW;
    END IF;

    -- フラグ設定
    PERFORM set_config('app.sync_in_progress', 'true', TRUE);

    -- ...同期ロジック...

    -- フラグ解除
    PERFORM set_config('app.sync_in_progress', 'false', TRUE);

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;
```

### 5.2 pg_notify による同期失敗検知

```sql
-- 同期失敗時の通知チャネル
CREATE OR REPLACE FUNCTION notify_sync_failure()
RETURNS TRIGGER AS $$
BEGIN
    PERFORM pg_notify('sync_failure', json_build_object(
        'table', TG_TABLE_NAME,
        'operation', TG_OP,
        'timestamp', CURRENT_TIMESTAMP,
        'detail', SQLERRM
    )::text);
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

-- VB.NET側のリスナー実装例:
-- Using conn As NpgsqlConnection = connMgr.GetConnection()
--     conn.Notification += Sub(sender, e)
--         Logger.Error("同期失敗検知: " & e.Payload)
--     End Sub
--     Using cmd As New NpgsqlCommand("LISTEN sync_failure", conn)
--         cmd.ExecuteNonQuery()
--     End Using
-- End Using
```

### 5.3 ロールバック手順

#### Phase A ロールバック

```sql
-- Phase A で作成したオブジェクトを全て削除
-- ビュー
DROP VIEW IF EXISTS v_unified_department CASCADE;
DROP VIEW IF EXISTS v_unified_supplier CASCADE;
DROP VIEW IF EXISTS v_unified_contract_type CASCADE;
DROP VIEW IF EXISTS v_unified_company CASCADE;
DROP VIEW IF EXISTS v_unified_user CASCADE;

-- トリガー
DROP TRIGGER IF EXISTS trg_m_contract_type_sync_to_kkbn ON m_contract_type;
DROP TRIGGER IF EXISTS trg_c_kkbn_sync_to_contract_type ON c_kkbn;
DROP TRIGGER IF EXISTS trg_m_supplier_sync_to_lcpt ON m_supplier;
DROP TRIGGER IF EXISTS trg_m_lcpt_sync_to_supplier ON m_lcpt;
DROP TRIGGER IF EXISTS trg_m_department_sync_to_bcat ON m_department;
DROP TRIGGER IF EXISTS trg_m_company_sync_to_corp ON m_company;

-- 関数
DROP FUNCTION IF EXISTS trg_sync_contract_type_to_kkbn() CASCADE;
DROP FUNCTION IF EXISTS trg_sync_kkbn_to_contract_type() CASCADE;
DROP FUNCTION IF EXISTS trg_sync_supplier_to_lcpt() CASCADE;
DROP FUNCTION IF EXISTS trg_sync_lcpt_to_supplier() CASCADE;
DROP FUNCTION IF EXISTS trg_sync_department_to_bcat() CASCADE;
DROP FUNCTION IF EXISTS trg_sync_company_to_corp() CASCADE;

-- 影響: SQL1系テーブルは一切変更されていないため、ロールバック後も仕訳パイプラインは動作する
```

#### Phase B ロールバック

```sql
-- ストアドプロシージャ削除
DROP FUNCTION IF EXISTS sp_sync_d_kykh_to_ctb(BOOLEAN) CASCADE;

-- pg_cron ジョブ削除（設定済みの場合）
-- SELECT cron.unschedule('sync_d_kykh_to_ctb');

-- 影響: 差分同期が停止するが、既存データは保持される
-- migrate_d_kykh_to_ctb.sql の手動実行で代替可能
```

### 5.4 回帰テスト計画

| テスト分類 | テスト内容 | 検証ポイント |
|-----------|-----------|-------------|
| 仕訳出力E2E | KeijoSqlBuilder → d_kykh/d_kykm → 仕訳CSV | 出力結果がPhase A前と完全一致 |
| ログイン認証 | AuthorizationService → tm_USER | ログイン成功/失敗が正常動作 |
| マスタ一覧表示 | MasterDataLoader → 各マスタ | コンボボックス・リストの内容が正常 |
| 契約一覧表示 | ctb_lease_integrated → 一覧画面 | 契約データが正しく表示 |
| 新規契約登録 | SQL2系への新規投入 | ctb + マスタ参照が正常 |

**自動化推奨テスト:**

```sql
-- テストスクリプト: 同期整合性チェック
DO $$
DECLARE
    v_mismatch_count INTEGER;
BEGIN
    -- c_kkbn と m_contract_type の整合性
    SELECT COUNT(*) INTO v_mismatch_count
    FROM c_kkbn ck
    LEFT JOIN m_contract_type ct ON LPAD(ck.kkbn_id::TEXT, 2, '0') = ct.contract_type_cd
    WHERE ct.contract_type_cd IS NULL;

    IF v_mismatch_count > 0 THEN
        RAISE WARNING '整合性エラー: c_kkbn に対応する m_contract_type がない件数: %', v_mismatch_count;
    END IF;

    -- m_lcpt と m_supplier の整合性
    SELECT COUNT(*) INTO v_mismatch_count
    FROM m_lcpt lc
    LEFT JOIN m_supplier s ON lc.lcpt1_cd = s.supplier_cd
    WHERE s.supplier_cd IS NULL AND lc.lcpt1_cd IS NOT NULL AND lc.history_f IS NOT TRUE;

    IF v_mismatch_count > 0 THEN
        RAISE WARNING '整合性エラー: m_lcpt に対応する m_supplier がない件数: %', v_mismatch_count;
    END IF;

    -- m_bcat と m_department の整合性
    SELECT COUNT(*) INTO v_mismatch_count
    FROM m_bcat bc
    LEFT JOIN m_department d ON bc.bcat1_cd = d.dept_cd
    WHERE d.dept_cd IS NULL AND bc.bcat1_cd IS NOT NULL AND bc.history_f IS NOT TRUE;

    IF v_mismatch_count > 0 THEN
        RAISE WARNING '整合性エラー: m_bcat に対応する m_department がない件数: %', v_mismatch_count;
    END IF;

    RAISE NOTICE '整合性チェック完了';
END
$$;
```

---

## 6. 要件との整合性確認

### 要件1: 既存顧客がそのまま会計システムを使用

| 確認項目 | 対応方法 | 状態 |
|---------|---------|------|
| SQL1テーブル（d_kykh/d_kykm）の構造変更なし | トリガーはSQL2側に配置。SQL1テーブルはトリガーの **書き込み先** としてのみ使用 | 達成 |
| KeijoSqlBuilder の変更なし | Phase A/B を通じて KeijoSqlBuilder は一切変更しない | 達成 |
| 仕訳出力結果の同一性 | 回帰テストで Phase A 前後の出力を diff 比較 | テストで検証 |
| DbConnectionManager の互換性 | 単一接続（`lease_m4bs`）を維持 | 達成 |

### 要件2: 新リース対応顧客が追加データを保持

| 確認項目 | 対応方法 | 状態 |
|---------|---------|------|
| SQL2テーブル（tw_lease_*, ctb_*）への新規投入 | Phase B で新規契約をSQL2系に直接投入 | Phase B で実装 |
| EAV属性（ctb_property_attribute）の利用 | 010_ctb_property_eav.sql で定義済。変更不要 | 既存で達成 |
| マスタの一元管理 | 統合ビュー（v_unified_*）経由で参照 | Phase A で実装 |
| 契約データの整合性 | sp_sync_d_kykh_to_ctb で差分同期を自動化 | Phase B で実装 |

### マスタ統合による両要件の同時充足

```
既存顧客フロー（変更なし）:
  m_lcpt/m_bcat/c_kkbn → d_kykh/d_kykm → KeijoSqlBuilder → 仕訳CSV

新リース対応フロー（Phase B以降）:
  m_supplier/m_department/m_contract_type → ctb_lease_integrated → 新仕訳エンジン

マスタ同期（Phase A で実現）:
  m_supplier ──trg──► m_lcpt     （SQL2→SQL1方向）
  m_department ──trg──► m_bcat   （SQL2→SQL1方向）
  m_contract_type ──trg──► c_kkbn（SQL2→SQL1方向）
  ※逆方向も対応（レガシー画面からの更新にも追従）
```

---

## 7. 実装チェックリスト

### Phase A チェックリスト

- [ ] `000_init_unified.sql` の作成とテスト
- [ ] `DbConnectionManager.vb` のデフォルト接続先確認
- [ ] `trg_sync_contract_type_to_kkbn` / `trg_sync_kkbn_to_contract_type` の作成
- [ ] `trg_sync_supplier_to_lcpt` / `trg_sync_lcpt_to_supplier` の作成
- [ ] `trg_sync_department_to_bcat` の作成
- [ ] `trg_sync_company_to_corp` の作成
- [ ] `v_unified_department` / `v_unified_supplier` / `v_unified_contract_type` / `v_unified_company` / `v_unified_user` の作成
- [ ] 循環防止（`app.sync_in_progress`）の実装
- [ ] 全トリガーの単体テスト
- [ ] 仕訳出力の回帰テスト（Phase A前後 diff 比較）
- [ ] ロールバックスクリプトの動作確認

### Phase B チェックリスト

- [ ] `sp_sync_d_kykh_to_ctb` の作成とテスト
- [ ] 差分同期の正常動作確認
- [ ] 全件再同期の正常動作確認
- [ ] `MasterDataLoader.vb` の統合ビュー参照オプション追加
- [ ] 新規契約投入フローのE2Eテスト
- [ ] pg_cron（または外部スケジューラ）による定期実行設定
- [ ] pg_notify による同期失敗検知の設定

---

## 8. 補足: ファイル配置計画

```
sql/
├── 000_init_unified.sql          ← NEW: 統合初期化スクリプト
├── 000_init.sql                  ← 既存（参考保存）
├── init.sql                      ← 既存（参考保存）
├── 001_ddl.sql                   ← 既存（変更なし）
├── 015_unified_views.sql         ← NEW: Phase A 統合ビュー定義
├── 016_sync_triggers.sql         ← NEW: Phase A 同期トリガー
├── 017_sync_loop_prevention.sql  ← NEW: Phase A 循環防止ユーティリティ
├── 020_sp_sync_d_kykh_to_ctb.sql ← NEW: Phase B ストアドプロシージャ
├── 099_rollback_phase_a.sql      ← NEW: Phase A ロールバック
├── 100_rollback_phase_b.sql      ← NEW: Phase B ロールバック
├── migrate_d_kykh_to_ctb.sql     ← 既存（参考保存、sp版で代替）
└── ...
```
