# Phase A/B 実装計画書

## 目次

1. [概要](#1-概要)
2. [Phase A: マスタ同期トリガー実装](#2-phase-a-マスタ同期トリガー実装)
3. [Phase B: マイグレーションSP改善 + VB.NET拡張](#3-phase-b-マイグレーションsp改善--vbnet拡張)
4. [推奨実行順序](#4-推奨実行順序)
5. [SQL実装例](#5-sql実装例)
6. [テスト計画](#6-テスト計画)

---

## 1. 概要

### 1.1 目的

SQL1系テーブル（Access移植版: `c_kkbn`, `m_lcpt`, `m_bcat`, `m_corp`, `sec_user`）とSQL2系テーブル（新リース対応版: `m_contract_type`, `m_supplier`, `m_department`, `m_company`, `tm_USER`）の間に存在する5組の重複マスタを、PostgreSQLトリガーによる自動同期で一元管理する。

併せて、既存のワンショットマイグレーションSQL（`501_sp_migrate_to_ctb.sql`）をストアドプロシージャ化し、UPSERT対応・エラーハンドリング・ctb_property自動作成を実現する。

### 1.2 前提条件

- DB統合済み: `001_init.sql` により単一DB `lease_m4bs` / ユーザー `lease_m4bs_user` に統合済み
- 既存の仕訳パイプライン（`KeijoSqlBuilder` → `d_kykh`/`d_kykm` → 仕訳出力）には一切変更を加えない
- VB.NET側は `DbConnectionManager` による単一接続を維持

### 1.3 共通の設計差異（5組共通）

| 項目 | SQL1系 | SQL2系 |
|------|--------|--------|
| PK方式 | 数値サロゲートキー（`*_id`） | 文字列ナチュラルキー（`*_cd`） |
| 履歴管理 | `history_f` boolean フラグ | 物理削除 |
| 監査カラム | `create_id`, `update_id`, `update_cnt` | `create_dt`, `update_dt` のみ |

### 1.4 ファイル配置

```
sql/
  40_views_triggers/
    401_sync_kkbn_contract_type.sql     # Task A-2
    402_sync_lcpt_supplier.sql          # Task A-3
    403_sync_bcat_department.sql        # Task A-4
    404_sync_corp_company.sql           # Task A-5
    405_sync_user_auth.sql              # Task A-6
  50_stored_procedures/
    501_sp_migrate_to_ctb.sql           # Task B-1（既存ファイル置換）
```

---

## 2. Phase A: マスタ同期トリガー実装

### 2.0 循環防止メカニズム（全トリガー共通）

全ての同期トリガーは `pg_catalog.current_setting` のセッション変数を用いた循環防止ガードを実装する。

```
[テーブルA UPDATE] → トリガーA発火 → SET LOCAL sync.in_progress = 'true'
  → テーブルB UPDATE → トリガーB発火 → sync.in_progress = 'true' を検知 → RETURN（スキップ）
  → トリガーA終了 → sync.in_progress リセット（トランザクション終了時に自動リセット）
```

---

### Task A-1: DB接続情報の統一

**状態**: 完了済み

`sql/00_init/001_init.sql` により以下に統合済み:
- DB名: `lease_m4bs`
- ユーザー: `lease_m4bs_user`
- パスワード: `iltex_mega_pass_m4`

追加作業なし。

---

### Task A-2: c_kkbn <-> m_contract_type 同期トリガー

**作成ファイル**: `sql/40_views_triggers/401_sync_kkbn_contract_type.sql`

#### 対象テーブル定義

| カラム | c_kkbn (SQL1) | m_contract_type (SQL2) |
|--------|---------------|----------------------|
| PK | `kkbn_id` (smallint) | `contract_type_cd` (varchar10) |
| 名称 | `kkbn_nm` (varchar50) | `contract_type_nm` (varchar100) |
| 表示順 | - | `sort_order` (smallint) |
| 備考 | - | `remarks` (varchar500) |
| 監査 | - | `create_dt`, `update_dt` |
| サロゲート | - | `id` (SERIAL) |

#### PK変換ロジック

```
SQL1→SQL2: LPAD(CAST(kkbn_id AS VARCHAR), 2, '0')  例: 1 → '01', 12 → '12'
SQL2→SQL1: CAST(LTRIM(contract_type_cd, '0') AS smallint)  例: '01' → 1
           ※ '00' の場合は 0 とする
```

#### 同期方向: 双方向

- **m_contract_type INSERT/UPDATE → c_kkbn**: `contract_type_cd` を `kkbn_id` に変換して反映
- **c_kkbn INSERT/UPDATE → m_contract_type**: `kkbn_id` を `contract_type_cd` に変換して反映

#### VB.NET参照箇所

- `KlsryoCalculationEngine.vb` → `c_kkbn` 参照
- `MasterDataLoader.vb` → `m_contract_type` 参照

#### 優先度: **高**（最もシンプル、パイロットケースとして最適）

---

### Task A-3: m_lcpt <-> m_supplier 同期トリガー

**作成ファイル**: `sql/40_views_triggers/402_sync_lcpt_supplier.sql`

#### 対象テーブル定義

| カラム群 | m_lcpt (SQL1) | m_supplier (SQL2) |
|---------|---------------|-------------------|
| PK | `lcpt_id` (int) | `supplier_cd` (varchar10) |
| コード/名称 | `lcpt1_cd`/`lcpt1_nm`, `lcpt2_cd`/`lcpt2_nm` | `supplier_cd`/`supplier_nm`, `supplier_cd2`/`supplier_nm2` |
| 締日row1 | `shime_day_1`, `sshri_kn1_1`, `shri_day1_1`, `sshri_kn2_1`, `shri_day2_1` | `row1_contract_closing_day`, `row1_first_pay_months`, `row1_first_pay_day`, `row1_second_pay_months`, `row1_second_pay_day` |
| 締日row2 | `shime_day_2`, `sshri_kn1_2`, `shri_day1_2`, `sshri_kn2_2`, `shri_day2_2` | `row2_contract_closing_day`, `row2_first_pay_months`, `row2_first_pay_day`, `row2_second_pay_months`, `row2_second_pay_day` |
| 締日row3 | `shime_day_3`, `sshri_kn1_3`, `shri_day1_3`, `sshri_kn2_3`, `shri_day2_3` | `row3_contract_closing_day`, `row3_first_pay_months`, `row3_first_pay_day`, `row3_second_pay_months`, `row3_second_pay_day` |
| リリースパラメータ | `sai_denomi` | `re_lease_param` |
| SQL1固有 | `sum1-3_cd/nm`, `shri_kn_ini`, `slkikan_s/n`, `shri_kn_s/n`, `sai_denomi_s/n`, `sai_numerator_s/n`, `shri_cnt_s_1-3`, `shho_id_s/n_1-3` | - |

#### PK変換ロジック

```
SQL1→SQL2: m_lcpt.lcpt1_cd → m_supplier.supplier_cd（文字列そのまま）
SQL2→SQL1: m_supplier.supplier_cd → m_lcpt.lcpt1_cd（文字列そのまま）
           lcpt_id はシーケンスで自動採番（INSERTの場合）
```

#### 同期方向: 実質双方向（ただし非対称）

- **m_supplier INSERT/UPDATE → m_lcpt**: 基本カラム（cd, nm, 締日/支払条件row1-3）を反映。SQL1固有カラム（sum系、shho_id系等）はNULLまたはデフォルト値。
- **m_lcpt INSERT/UPDATE → m_supplier UPSERT**: 基本カラムのみ同期。SQL1固有カラムの変更はSQL2には反映しない。

#### 注意事項

- `m_lcpt` には `sum1-3_cd/nm`、`shri_kn_ini`、`slkikan_s/n`、`shho_id_s/n_1-3` 等のSQL1固有カラムが多数存在する。これらはSQL1側のみで管理する。
- `ctb_lease_integrated.supplier_cd` が `m_supplier` へのFKとなっているため、同期の整合性は重要。

#### VB.NET参照箇所

- `KlsryoCalculationEngine.vb` → `m_lcpt` 参照
- `MasterDataLoader.vb` → `m_supplier` 参照

#### 優先度: **高**（ctb_lease_integratedのFK依存あり）

---

### Task A-4: m_bcat <-> m_department 統合ビュー + 同期

**作成ファイル**: `sql/40_views_triggers/403_sync_bcat_department.sql`

#### 対象テーブル定義

| カラム群 | m_bcat (SQL1) | m_department (SQL2) |
|---------|---------------|---------------------|
| PK | `bcat_id` (int) | `dept_cd` (varchar10) |
| コード/名称 1-5 | `bcat1_cd`/`bcat1_nm` 〜 `bcat5_cd`/`bcat5_nm` | `dept_cd`/`dept_nm` 〜 `dept_cd5`/`dept_nm5` |
| 集約カテゴリ | `sum1_cd`/`sum1_nm` 〜 `sum3_cd`/`sum3_nm` | `agg_category1_cd`/`agg_category1_nm` 〜 3 |
| SQL1固有 | `genk_id`(FK→m_genk), `skti_id`(FK→m_skti), `bknri_id`, `kbf_kb/fb/sb`(boolean), `gensonf`(boolean) | - |
| SQL2固有 | - | `cost_category_nm` |

#### PK変換ロジック

```
SQL1→SQL2: m_bcat.bcat1_cd → m_department.dept_cd
SQL2→SQL1: m_department.dept_cd → m_bcat.bcat1_cd
           bcat_id はシーケンスで自動採番（INSERTの場合）
```

#### 間接参照の注意

マイグレーションSP（`501_sp_migrate_to_ctb.sql`）では以下の経路で対応づけている:

```
d_kykh.kknri_id → m_kknri.kknri1_cd → m_department.dept_cd
```

`m_bcat.bcat1_cd` と `m_department.dept_cd` は直接対応するが、粒度が異なる可能性がある（bcatは物件管理部署単位、departmentは組織部門単位）。運用上は `bcat1_cd = dept_cd` となるようマスタデータを揃える前提とする。

#### 同期方向: SQL2 → SQL1 一方向（+ 統合ビュー）

- **m_department INSERT/UPDATE → m_bcat**: dept_cd/nm 1-5、agg_category 1-3 を同期。SQL1固有カラム（genk_id, skti_id, bknri_id, kbf_*, gensonf）はSQL1側のみで管理。
- **m_bcat → m_department**: 直接同期は行わない（m_kknri経由の間接参照が主であり、直接同期すると不整合リスクがある）。
- **v_unified_department ビュー**: 両テーブルの共通カラムを統合したリードオンリービューを提供。

#### VB.NET参照箇所

- `KlsryoCalculationEngine.vb` → `m_bcat` 参照
- `MasterDataLoader.vb` → `m_department` 参照
- `ctb_lease_integrated.mgmt_dept_cd` → FK `m_department`

#### 優先度: **中**（間接参照あり、設計精査が必要）

---

### Task A-5: m_corp <-> m_company 同期トリガー

**作成ファイル**: `sql/40_views_triggers/404_sync_corp_company.sql`

#### 対象テーブル定義

| カラム | m_corp (SQL1) | m_company (SQL2) |
|--------|---------------|------------------|
| PK | `corp_id` (int) | `company_cd` (varchar10) |
| コード/名称1 | `corp1_cd`/`corp1_nm` | `company_cd`/`company_nm` |
| コード/名称2 | `corp2_cd`/`corp2_nm` | `company_cd2`/`company_nm2` |
| コード/名称3 | `corp3_cd`/`corp3_nm` | `company_cd3`/`company_nm3` |
| 備考 | `biko` (varchar100) | `remarks` (varchar500) |
| 監査 | `create_id/dt`, `update_id/dt`, `update_cnt` | `create_dt`, `update_dt` |
| 履歴 | `history_f` (boolean) | - |

#### PK変換ロジック

```
SQL1→SQL2: m_corp.corp1_cd → m_company.company_cd
SQL2→SQL1: m_company.company_cd → m_corp.corp1_cd
           corp_id はシーケンスで自動採番（INSERTの場合）
```

#### 同期方向: 双方向

カラム構造がシンプルで1:1対応するため、双方向同期が安全に実装可能。

- **m_company INSERT/UPDATE → m_corp**: company_cd/nm 1-3 → corp1-3_cd/nm
- **m_corp INSERT/UPDATE → m_company UPSERT**: corp1-3_cd/nm → company_cd/nm 1-3

#### VB.NET参照箇所

- `TestWinForms` 複数フォーム → `m_corp` 参照
- `FrmFlexMaster.vb` → `m_company` 参照

#### 優先度: **高**（シンプル、A-2の次に実装推奨）

---

### Task A-6: sec_user / tw_m_user <-> tm_USER 認証統合

**作成ファイル**: `sql/40_views_triggers/405_sync_user_auth.sql`

#### 対象テーブル定義

| カラム群 | sec_user (SQL1) | tw_m_user (SQL1) | tm_USER (SQL2拡張) |
|---------|-----------------|-------------------|-------------------|
| PK | `user_id` (int) | `user_id` (int) | `user_id` (int) |
| コード | `user_cd` (varchar12) | - | `login_id` (ALTER追加) |
| 名称 | `user_nm` (varchar40) | `user_name` | `user_name` (既存) |
| 認証 | `pwd` (varchar255, **平文**) | - | `password_hash` (PBKDF2-SHA256) |
| 権限 | `kngn_id` (FK→sec_kngn) | - | `role` (admin/accounting/general_affairs/viewer) |
| 状態 | - | - | `is_active`, `failed_login_count`, `locked_until`, `last_login_at` |

#### 認証方式の根本差異

- **sec_user**: パスワードを平文格納（`pwd` カラム）。`sec_kngn` テーブルで権限を分離管理。
- **tm_USER**: PBKDF2-SHA256ハッシュ。ロールを単一カラムで管理。アカウントロック機構あり。

**パスワード同期は不可能**（平文→ハッシュの一方向変換のため、ハッシュ→平文の逆変換は不可）。

#### 同期方向: tm_USER を正（マスター）とする片方向 + マッピング

- **tm_USER INSERT/UPDATE → sec_user**: 基本情報（user_cd=login_id, user_nm=user_name）のみ同期。パスワードは同期しない。
- **tm_USER INSERT/UPDATE → tw_m_user**: user_name, user_kana を同期。
- **sec_user → tm_USER**: 直接同期は行わない。新規ユーザー作成は必ずtm_USER側から行う。

#### kngn_id <-> role マッピングテーブル

権限の対応づけに `t_role_kngn_mapping` テーブルを新規作成する:

```sql
CREATE TABLE t_role_kngn_mapping (
    role        VARCHAR(30) NOT NULL,   -- tm_USER.role
    kngn_id     INTEGER     NOT NULL,   -- sec_kngn.kngn_id
    CONSTRAINT pk_role_kngn_mapping PRIMARY KEY (role)
);

-- 初期データ例
INSERT INTO t_role_kngn_mapping VALUES
    ('admin',            1),  -- 管理者権限
    ('accounting',       2),  -- 経理権限
    ('general_affairs',  3),  -- 総務権限
    ('viewer',           4);  -- 参照のみ
```

#### VB.NET参照箇所

- `Form_f_LOGIN_JET.vb`, `Form_f_SEC_USER_INP.vb` 等 → `sec_user` 参照
- `AuthorizationService.vb` → `tm_USER` 参照

#### 優先度: **低**（最も複雑、認証方式の差異による制約が大きい）

---

## 3. Phase B: マイグレーションSP改善 + VB.NET拡張

### Task B-1: migrate_d_kykh_to_ctb のストアドプロシージャ化

**作成ファイル**: `sql/50_stored_procedures/501_sp_migrate_to_ctb.sql`（既存ファイル置換）

#### 現状の問題点

| 問題 | 影響 |
|------|------|
| `ON CONFLICT DO NOTHING` | 既存レコードの更新不可、データ差分が永久に放置される |
| ストアドプロシージャでない | VB.NETからの呼び出し困難、トランザクション管理が不十分 |
| ctb_propertyへの連携なし | 物件マスタが自動作成されず、EAV属性が欠損 |
| エラーハンドリングなし | 1件のエラーで全件ロールバック、原因特定困難 |
| 配賦は100%固定1レコード | d_haifに複数配賦があっても反映されない |

#### 改善内容

1. **UPSERT化**: `ON CONFLICT DO UPDATE SET` に変更。既存レコードの更新を実現。
2. **ストアドプロシージャ化**: `CREATE OR REPLACE FUNCTION sp_migrate_to_ctb()` として定義。
3. **ctb_property自動作成**: ctb_lease_integrated INSERT時にctb_propertyも連動作成。
4. **処理件数返却**: inserted / updated / skipped のカウントをRETURN。
5. **エラーハンドリング**: `EXCEPTION WHEN` で個別レコードのエラーをキャッチし、ログテーブルに記録して続行。
6. **d_haif複数配賦対応**: d_haifの全レコードをctb_dept_allocationに反映。

#### VB.NETからの呼び出し

```vb
' CtbRepository.vb に追加
Public Function RunMigration() As MigrationResult
    Using conn As NpgsqlConnection = _connMgr.GetConnection()
        Using cmd As New NpgsqlCommand("SELECT * FROM sp_migrate_to_ctb()", conn)
            Using reader = cmd.ExecuteReader()
                ' inserted, updated, skipped, errors を取得
            End Using
        End Using
    End Using
End Function
```

---

### Task B-2: CtbRepository UPDATE/DELETE 追加

**VB.NETファイル**: `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbRepository.vb`

#### 追加メソッド

| メソッド | 概要 |
|---------|------|
| `UpdateByContractNo(contractNo, propertyNo, rec)` | ctb_lease_integrated の既存レコードを更新 |
| `SoftDeleteByContractNo(contractNo, propertyNo)` | 論理削除（`is_deleted = true`, `deleted_at = NOW()`） |
| `UpsertAll(records)` | INSERT or UPDATE（既存InsertAllのUPSERT版） |

#### 論理削除の前提

ctb_lease_integrated に以下のカラム追加が必要:

```sql
ALTER TABLE ctb_lease_integrated
    ADD COLUMN IF NOT EXISTS is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN IF NOT EXISTS deleted_at TIMESTAMP NULL;
```

#### 既存InsertAllとの関係

- `InsertAll` は新規登録のみ（既存動作を維持）
- `UpsertAll` は新規追加メソッドとして、INSERT済みレコードがあればUPDATEする

---

### Task B-3: 新規契約のSQL2系直接投入フロー整備

**対象ファイル**: `FrmLeaseContractMain.vb`, `CtbDataStore.vb`（メモリストア）, `CtbRepository.vb`

#### 現状のフロー

```
FrmLeaseContractMain（画面入力）
  → CtbDataStore（メモリ上のList(Of CtbRecord)）
  → CtbRepository.InsertAll（DBへ一括INSERT）
```

#### 問題点

- メモリストア（CtbDataStore）とDB状態が乖離するリスク
- 画面で編集後にDB反映せずに別画面に遷移すると、メモリとDBの不整合が発生
- エラー時のリカバリ手段がない

#### 改善方針

1. **即時永続化**: 画面で「保存」押下時に即座にDB反映（InsertまたはUpdate）
2. **楽観ロック**: `update_dt` カラムを用いた楽観的排他制御
3. **CtbDataStore のDB連動化**: CtbDataStoreにLoadFromDb / SaveToDbメソッドを追加し、メモリとDBの同期を明示的に管理

---

## 4. 推奨実行順序

### Phase A: マスタ同期トリガー（依存関係考慮）

```
Step 1: Task A-2  c_kkbn <-> m_contract_type
        ├── 理由: 最もシンプル（2カラム対2カラム）
        ├── パイロットケースとして循環防止メカニズムを検証
        └── 所要時間: 0.5日

Step 2: Task A-5  m_corp <-> m_company
        ├── 理由: 次にシンプル（3セットのcd/nm対応）
        ├── A-2で確立したトリガーパターンを再利用
        └── 所要時間: 0.5日

Step 3: Task A-3  m_lcpt <-> m_supplier
        ├── 理由: カラム数は多いが重要度高（ctb_lease_integrated FK依存）
        ├── 締日/支払条件の3行パターンマッピングが核心
        └── 所要時間: 1日

Step 4: Task A-4  m_bcat <-> m_department
        ├── 理由: m_kknri経由の間接参照があり設計精査が必要
        ├── 統合ビュー v_unified_department の作成を含む
        └── 所要時間: 1日

Step 5: Task A-6  sec_user <-> tm_USER
        ├── 理由: 最も複雑、認証方式の根本差異あり
        ├── t_role_kngn_mapping テーブル新規作成を含む
        └── 所要時間: 1.5日
```

**Phase A 合計: 約4.5日**

### Phase B: SP改善 + VB.NET拡張

```
Step 6: Task B-1  SP化（sp_migrate_to_ctb）
        ├── 前提: A-3（m_supplier同期）、A-4（m_department同期）が稼働していること
        ├── UPSERT化 + ctb_property自動作成 + エラーハンドリング
        └── 所要時間: 1.5日

Step 7: Task B-2  CtbRepository拡張
        ├── 前提: B-1のSPが動作すること
        ├── UpdateByContractNo / SoftDeleteByContractNo / UpsertAll 追加
        └── 所要時間: 1日

Step 8: Task B-3  投入フロー整備
        ├── 前提: B-2のリポジトリメソッドが利用可能であること
        ├── FrmLeaseContractMain / CtbDataStore のDB連動化
        └── 所要時間: 1日
```

**Phase B 合計: 約3.5日**

### 全体所要時間: 約8日（Phase A: 4.5日 + Phase B: 3.5日）

---

## 5. SQL実装例

### 5.1 Task A-2: c_kkbn <-> m_contract_type 同期トリガー

```sql
-- ============================================================
-- 401_sync_kkbn_contract_type.sql
-- c_kkbn <-> m_contract_type 双方向同期トリガー
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- 循環防止ヘルパー関数
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION is_sync_in_progress()
RETURNS BOOLEAN
LANGUAGE plpgsql AS $$
BEGIN
    RETURN COALESCE(current_setting('sync.in_progress', TRUE), 'false') = 'true';
END;
$$;

-- ------------------------------------------------------------
-- m_contract_type → c_kkbn 同期トリガー関数
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_contract_type_to_kkbn()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_kkbn_id smallint;
BEGIN
    -- 循環防止
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    -- セッション変数セット（トランザクション終了時に自動リセット）
    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- PK変換: contract_type_cd → kkbn_id
    -- '01' → 1, '12' → 12, '00' → 0
    BEGIN
        v_kkbn_id := CAST(
            CASE
                WHEN LTRIM(NEW.contract_type_cd, '0') = '' THEN '0'
                ELSE LTRIM(NEW.contract_type_cd, '0')
            END AS smallint
        );
    EXCEPTION WHEN OTHERS THEN
        RAISE WARNING 'sync_contract_type_to_kkbn: 変換失敗 cd=%, error=%',
            NEW.contract_type_cd, SQLERRM;
        PERFORM pg_notify('sync_error', json_build_object(
            'source', 'm_contract_type',
            'target', 'c_kkbn',
            'key', NEW.contract_type_cd,
            'error', SQLERRM
        )::text);
        RETURN NEW;
    END;

    IF TG_OP = 'INSERT' THEN
        INSERT INTO c_kkbn (kkbn_id, kkbn_nm)
        VALUES (v_kkbn_id, LEFT(NEW.contract_type_nm, 50))
        ON CONFLICT (kkbn_id) DO UPDATE SET
            kkbn_nm = EXCLUDED.kkbn_nm;

    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE c_kkbn SET
            kkbn_nm = LEFT(NEW.contract_type_nm, 50)
        WHERE kkbn_id = v_kkbn_id;

        -- PKが変更された場合（contract_type_cdの変更）
        IF OLD.contract_type_cd <> NEW.contract_type_cd THEN
            DECLARE
                v_old_kkbn_id smallint;
            BEGIN
                v_old_kkbn_id := CAST(
                    CASE
                        WHEN LTRIM(OLD.contract_type_cd, '0') = '' THEN '0'
                        ELSE LTRIM(OLD.contract_type_cd, '0')
                    END AS smallint
                );
                DELETE FROM c_kkbn WHERE kkbn_id = v_old_kkbn_id;
            END;
        END IF;
    END IF;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- c_kkbn → m_contract_type 同期トリガー関数
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_kkbn_to_contract_type()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_cd VARCHAR(10);
BEGIN
    -- 循環防止
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- PK変換: kkbn_id → contract_type_cd
    v_cd := LPAD(CAST(NEW.kkbn_id AS VARCHAR), 2, '0');

    IF TG_OP = 'INSERT' THEN
        INSERT INTO m_contract_type (contract_type_cd, contract_type_nm)
        VALUES (v_cd, COALESCE(NEW.kkbn_nm, ''))
        ON CONFLICT (contract_type_cd) DO UPDATE SET
            contract_type_nm = EXCLUDED.contract_type_nm,
            update_dt = CURRENT_TIMESTAMP;

    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE m_contract_type SET
            contract_type_nm = COALESCE(NEW.kkbn_nm, ''),
            update_dt = CURRENT_TIMESTAMP
        WHERE contract_type_cd = v_cd;
    END IF;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- トリガー定義
-- ------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_contract_type_to_kkbn ON m_contract_type;
CREATE TRIGGER trg_contract_type_to_kkbn
    AFTER INSERT OR UPDATE ON m_contract_type
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_contract_type_to_kkbn();

DROP TRIGGER IF EXISTS trg_kkbn_to_contract_type ON c_kkbn;
CREATE TRIGGER trg_kkbn_to_contract_type
    AFTER INSERT OR UPDATE ON c_kkbn
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_kkbn_to_contract_type();

COMMIT;
```

### 5.2 Task A-5: m_corp <-> m_company 同期トリガー

```sql
-- ============================================================
-- 404_sync_corp_company.sql
-- m_corp <-> m_company 双方向同期トリガー
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- m_company → m_corp 同期
-- ------------------------------------------------------------
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
        -- corp_id はシーケンスで自動採番
        INSERT INTO m_corp (
            corp_id, corp1_cd, corp1_nm, corp2_cd, corp2_nm,
            corp3_cd, corp3_nm, biko, create_dt, update_dt
        ) VALUES (
            COALESCE(
                (SELECT MAX(corp_id) + 1 FROM m_corp), 1
            ),
            NEW.company_cd,
            LEFT(NEW.company_nm, 40),
            NEW.company_cd2,
            LEFT(NEW.company_nm2, 40),
            NEW.company_cd3,
            LEFT(NEW.company_nm3, 40),
            LEFT(NEW.remarks, 100),
            CURRENT_TIMESTAMP,
            CURRENT_TIMESTAMP
        )
        ON CONFLICT DO NOTHING;  -- corp1_cdに一意制約がない場合

    ELSIF TG_OP = 'UPDATE' THEN
        UPDATE m_corp SET
            corp1_nm = LEFT(NEW.company_nm, 40),
            corp2_cd = NEW.company_cd2,
            corp2_nm = LEFT(NEW.company_nm2, 40),
            corp3_cd = NEW.company_cd3,
            corp3_nm = LEFT(NEW.company_nm3, 40),
            biko     = LEFT(NEW.remarks, 100),
            update_dt = CURRENT_TIMESTAMP
        WHERE corp1_cd = NEW.company_cd;
    END IF;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- m_corp → m_company 同期
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_corp_to_company()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' OR TG_OP = 'UPDATE' THEN
        INSERT INTO m_company (
            company_cd, company_nm, company_cd2, company_nm2,
            company_cd3, company_nm3, remarks
        ) VALUES (
            NEW.corp1_cd,
            COALESCE(NEW.corp1_nm, ''),
            NEW.corp2_cd,
            NEW.corp2_nm,
            NEW.corp3_cd,
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
    END IF;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- トリガー定義
-- ------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_company_to_corp ON m_company;
CREATE TRIGGER trg_company_to_corp
    AFTER INSERT OR UPDATE ON m_company
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_company_to_corp();

DROP TRIGGER IF EXISTS trg_corp_to_company ON m_corp;
CREATE TRIGGER trg_corp_to_company
    AFTER INSERT OR UPDATE ON m_corp
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_corp_to_company();

COMMIT;
```

### 5.3 Task A-3: m_lcpt <-> m_supplier 同期トリガー（抜粋）

```sql
-- ============================================================
-- 402_sync_lcpt_supplier.sql
-- m_lcpt <-> m_supplier 非対称双方向同期
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- m_supplier → m_lcpt 同期（基本カラム + 締日/支払条件）
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_supplier_to_lcpt()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    IF TG_OP = 'INSERT' THEN
        INSERT INTO m_lcpt (
            lcpt_id, lcpt1_cd, lcpt1_nm, lcpt2_cd, lcpt2_nm,
            shime_day_1, sshri_kn1_1, shri_day1_1, sshri_kn2_1, shri_day2_1,
            shime_day_2, sshri_kn1_2, shri_day1_2, sshri_kn2_2, shri_day2_2,
            shime_day_3, sshri_kn1_3, shri_day1_3, sshri_kn2_3, shri_day2_3,
            sai_denomi,
            create_dt, update_dt
        ) VALUES (
            COALESCE((SELECT MAX(lcpt_id) + 1 FROM m_lcpt), 1),
            NEW.supplier_cd,
            LEFT(NEW.supplier_nm, 40),
            NEW.supplier_cd2,
            LEFT(NEW.supplier_nm2, 40),
            -- 締日/支払条件 row1
            NEW.row1_contract_closing_day,
            NEW.row1_first_pay_months,
            NEW.row1_first_pay_day,
            NEW.row1_second_pay_months,
            NEW.row1_second_pay_day,
            -- 締日/支払条件 row2
            NEW.row2_contract_closing_day,
            NEW.row2_first_pay_months,
            NEW.row2_first_pay_day,
            NEW.row2_second_pay_months,
            NEW.row2_second_pay_day,
            -- 締日/支払条件 row3
            NEW.row3_contract_closing_day,
            NEW.row3_first_pay_months,
            NEW.row3_first_pay_day,
            NEW.row3_second_pay_months,
            NEW.row3_second_pay_day,
            -- リリースパラメータ
            NEW.re_lease_param,
            CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
        );

    ELSIF TG_OP = 'UPDATE' THEN
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
            update_dt    = CURRENT_TIMESTAMP
        WHERE lcpt1_cd = NEW.supplier_cd;
    END IF;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- m_lcpt → m_supplier 同期（基本カラムのみ）
-- SQL1固有カラム（sum系、shho_id系）は同期しない
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_lcpt_to_supplier()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    INSERT INTO m_supplier (
        supplier_cd, supplier_nm, supplier_cd2, supplier_nm2,
        row1_contract_closing_day, row1_first_pay_months, row1_first_pay_day,
        row1_second_pay_months, row1_second_pay_day,
        row2_contract_closing_day, row2_first_pay_months, row2_first_pay_day,
        row2_second_pay_months, row2_second_pay_day,
        row3_contract_closing_day, row3_first_pay_months, row3_first_pay_day,
        row3_second_pay_months, row3_second_pay_day,
        re_lease_param
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
        NEW.sai_denomi
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
        update_dt                  = CURRENT_TIMESTAMP;

    RETURN NEW;
END;
$$;

-- ------------------------------------------------------------
-- トリガー定義
-- ------------------------------------------------------------
DROP TRIGGER IF EXISTS trg_supplier_to_lcpt ON m_supplier;
CREATE TRIGGER trg_supplier_to_lcpt
    AFTER INSERT OR UPDATE ON m_supplier
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_supplier_to_lcpt();

DROP TRIGGER IF EXISTS trg_lcpt_to_supplier ON m_lcpt;
CREATE TRIGGER trg_lcpt_to_supplier
    AFTER INSERT OR UPDATE ON m_lcpt
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_lcpt_to_supplier();

COMMIT;
```

### 5.4 Task B-1: sp_migrate_to_ctb ストアドプロシージャ

```sql
-- ============================================================
-- 501_sp_migrate_to_ctb.sql
-- d_kykh → ctb_lease_integrated UPSERT ストアドプロシージャ
-- ============================================================

CREATE OR REPLACE FUNCTION sp_migrate_to_ctb()
RETURNS TABLE (
    total_processed INTEGER,
    inserted_count  INTEGER,
    updated_count   INTEGER,
    skipped_count   INTEGER,
    error_count     INTEGER
)
LANGUAGE plpgsql AS $$
DECLARE
    v_inserted  INTEGER := 0;
    v_updated   INTEGER := 0;
    v_skipped   INTEGER := 0;
    v_errors    INTEGER := 0;
    v_total     INTEGER := 0;
    rec         RECORD;
    v_ctb_id    INTEGER;
    v_exists    BOOLEAN;
BEGIN
    -- エラーログテーブル（セッション内一時テーブル）
    CREATE TEMP TABLE IF NOT EXISTS tmp_migration_errors (
        contract_no VARCHAR(20),
        error_msg   TEXT,
        error_dt    TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    ) ON COMMIT DROP;

    FOR rec IN
        SELECT
            k.kykbnj                                       AS contract_no,
            1                                              AS property_no,
            k.kyak_nm                                      AS contract_name,
            LPAD(CAST(k.kkbn_id AS VARCHAR), 2, '0')      AS contract_type_cd,
            lc.lcpt1_cd                                    AS supplier_cd,
            kn.kknri1_cd                                   AS mgmt_dept_cd,
            CAST(k.start_dt AS DATE)                       AS lease_start_date,
            CAST(k.end_dt AS DATE)                         AS lease_end_date,
            CAST(k.lkikan AS INTEGER)                      AS lease_term_months,
            CAST(k.k_glsryo AS NUMERIC(15,2))             AS monthly_payment,
            CAST(k.k_slsryo AS NUMERIC(15,2))             AS total_payment,
            CONCAT_WS(' | ',
                '相手方番号:' || k.kykbnl,
                '稟議番号:' || k.rng_bango
            )                                              AS remarks,
            k.kykbnj IS NOT NULL                           AS is_valid
        FROM d_kykh k
        LEFT JOIN m_kknri kn ON k.kknri_id = kn.kknri_id
        LEFT JOIN m_lcpt lc ON k.lcpt_id = lc.lcpt_id
        WHERE k.k_history_f IS NOT TRUE
    LOOP
        v_total := v_total + 1;

        BEGIN
            -- 契約番号が空の場合はスキップ
            IF rec.contract_no IS NULL OR TRIM(rec.contract_no) = '' THEN
                v_skipped := v_skipped + 1;
                CONTINUE;
            END IF;

            -- 既存レコード存在チェック
            SELECT EXISTS(
                SELECT 1 FROM ctb_lease_integrated
                WHERE contract_no = rec.contract_no
                  AND property_no = rec.property_no
            ) INTO v_exists;

            -- UPSERT
            INSERT INTO ctb_lease_integrated (
                contract_no, property_no, contract_name,
                contract_type_cd, supplier_cd, mgmt_dept_cd,
                lease_start_date, lease_end_date,
                free_rent_months, lease_term_months,
                asset_name, monthly_payment, total_payment, remarks
            ) VALUES (
                rec.contract_no, rec.property_no, rec.contract_name,
                rec.contract_type_cd, rec.supplier_cd, rec.mgmt_dept_cd,
                rec.lease_start_date, rec.lease_end_date,
                0, rec.lease_term_months,
                rec.contract_name, rec.monthly_payment, rec.total_payment,
                rec.remarks
            )
            ON CONFLICT (contract_no, property_no) DO UPDATE SET
                contract_name    = EXCLUDED.contract_name,
                contract_type_cd = EXCLUDED.contract_type_cd,
                supplier_cd      = EXCLUDED.supplier_cd,
                mgmt_dept_cd     = EXCLUDED.mgmt_dept_cd,
                lease_start_date = EXCLUDED.lease_start_date,
                lease_end_date   = EXCLUDED.lease_end_date,
                lease_term_months= EXCLUDED.lease_term_months,
                monthly_payment  = EXCLUDED.monthly_payment,
                total_payment    = EXCLUDED.total_payment,
                remarks          = EXCLUDED.remarks,
                update_dt        = CURRENT_TIMESTAMP
            RETURNING ctb_id INTO v_ctb_id;

            IF v_exists THEN
                v_updated := v_updated + 1;
            ELSE
                v_inserted := v_inserted + 1;

                -- 新規の場合、ctb_property も自動作成
                INSERT INTO ctb_property (
                    ctb_id, asset_category_cd, property_name
                ) VALUES (
                    v_ctb_id, NULL, rec.contract_name
                )
                ON CONFLICT DO NOTHING;
            END IF;

            -- 配賦レコード（デフォルト100%）
            -- 既存配賦を削除して再作成
            DELETE FROM ctb_dept_allocation WHERE ctb_id = v_ctb_id;

            INSERT INTO ctb_dept_allocation (
                ctb_id, dept_cd, allocation_ratio, payment_amount
            )
            SELECT
                v_ctb_id,
                COALESCE(b.bcat1_cd, rec.mgmt_dept_cd),
                h.haif_ritu,
                rec.monthly_payment * h.haif_ritu / 100.0
            FROM d_haif h
            LEFT JOIN m_bcat b ON h.bcat_id = b.bcat_id
            WHERE h.kykbnj = rec.contract_no
            ;

            -- d_haifにレコードがない場合、管理部署で100%配賦
            IF NOT FOUND AND rec.mgmt_dept_cd IS NOT NULL THEN
                INSERT INTO ctb_dept_allocation (
                    ctb_id, dept_cd, allocation_ratio, payment_amount
                ) VALUES (
                    v_ctb_id, rec.mgmt_dept_cd, 100.00, rec.monthly_payment
                );
            END IF;

        EXCEPTION WHEN OTHERS THEN
            v_errors := v_errors + 1;
            INSERT INTO tmp_migration_errors (contract_no, error_msg)
            VALUES (rec.contract_no, SQLERRM);

            -- エラー通知
            PERFORM pg_notify('migration_error', json_build_object(
                'contract_no', rec.contract_no,
                'error', SQLERRM
            )::text);
        END;
    END LOOP;

    -- 結果返却
    RETURN QUERY SELECT v_total, v_inserted, v_updated, v_skipped, v_errors;
END;
$$;

COMMENT ON FUNCTION sp_migrate_to_ctb() IS
    'd_kykh → ctb_lease_integrated マイグレーション（UPSERT対応）';
```

### 5.5 Task A-4: v_unified_department 統合ビュー

```sql
-- ============================================================
-- 403_sync_bcat_department.sql（ビュー部分）
-- ============================================================

CREATE OR REPLACE VIEW v_unified_department AS
SELECT
    d.dept_cd                          AS dept_cd,
    d.dept_nm                          AS dept_nm,
    d.dept_cd2, d.dept_nm2,
    d.dept_cd3, d.dept_nm3,
    d.dept_cd4, d.dept_nm4,
    d.dept_cd5, d.dept_nm5,
    d.cost_category_nm,
    d.agg_category1_cd, d.agg_category1_nm,
    d.agg_category2_cd, d.agg_category2_nm,
    d.agg_category3_cd, d.agg_category3_nm,
    -- SQL1固有カラム（m_bcatから取得、NULLの場合あり）
    b.bcat_id,
    b.genk_id,
    b.skti_id,
    b.bknri_id,
    b.kbf_kb, b.kbf_fb, b.kbf_sb,
    b.gensonf,
    -- 同期状態
    CASE
        WHEN b.bcat_id IS NOT NULL AND d.dept_cd IS NOT NULL THEN 'synced'
        WHEN b.bcat_id IS NOT NULL THEN 'sql1_only'
        ELSE 'sql2_only'
    END AS sync_status
FROM m_department d
FULL OUTER JOIN m_bcat b ON b.bcat1_cd = d.dept_cd
                         AND (b.history_f IS NOT TRUE OR b.history_f IS NULL);

COMMENT ON VIEW v_unified_department IS
    'm_bcat + m_department 統合ビュー（読み取り専用）';
```

### 5.6 Task A-6: tm_USER → sec_user 同期トリガー（抜粋）

```sql
-- ============================================================
-- 405_sync_user_auth.sql
-- tm_USER → sec_user / tw_m_user 片方向同期
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- kngn_id <-> role マッピングテーブル
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS t_role_kngn_mapping (
    role        VARCHAR(30)  NOT NULL,
    kngn_id     INTEGER      NOT NULL,
    description VARCHAR(100) NULL,
    CONSTRAINT pk_role_kngn_mapping PRIMARY KEY (role)
);

INSERT INTO t_role_kngn_mapping (role, kngn_id, description) VALUES
    ('admin',            1, '管理者 - 全権限'),
    ('accounting',       2, '経理担当 - 仕訳・マスタ更新'),
    ('general_affairs',  3, '総務担当 - 契約・マスタ参照'),
    ('viewer',           4, '参照専用 - 閲覧のみ')
ON CONFLICT (role) DO NOTHING;

-- ------------------------------------------------------------
-- tm_USER → sec_user + tw_m_user 同期
-- パスワードは同期しない（平文 vs ハッシュ、逆変換不可）
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION trg_sync_tm_user_to_sec_user()
RETURNS TRIGGER
LANGUAGE plpgsql AS $$
DECLARE
    v_kngn_id INTEGER;
BEGIN
    IF is_sync_in_progress() THEN
        RETURN NEW;
    END IF;

    PERFORM set_config('sync.in_progress', 'true', TRUE);

    -- role → kngn_id 変換
    SELECT kngn_id INTO v_kngn_id
    FROM t_role_kngn_mapping
    WHERE role = NEW.role;

    -- デフォルト: viewer権限
    IF v_kngn_id IS NULL THEN
        v_kngn_id := 4;
    END IF;

    -- sec_user 同期（パスワードは同期しない）
    INSERT INTO sec_user (
        user_id, user_cd, user_nm, kngn_id,
        create_dt, update_dt
    ) VALUES (
        NEW.user_id,
        NEW.login_id,
        NEW.user_name,
        v_kngn_id,
        CURRENT_TIMESTAMP,
        CURRENT_TIMESTAMP
    )
    ON CONFLICT (user_id) DO UPDATE SET
        user_cd   = EXCLUDED.user_cd,
        user_nm   = EXCLUDED.user_nm,
        kngn_id   = EXCLUDED.kngn_id,
        update_dt = CURRENT_TIMESTAMP;

    -- tw_m_user 同期
    INSERT INTO tw_m_user (user_id, user_name)
    VALUES (NEW.user_id, NEW.user_name)
    ON CONFLICT (user_id) DO UPDATE SET
        user_name = EXCLUDED.user_name;

    RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_tm_user_to_sec_user ON tm_user;
CREATE TRIGGER trg_tm_user_to_sec_user
    AFTER INSERT OR UPDATE ON tm_user
    FOR EACH ROW
    EXECUTE FUNCTION trg_sync_tm_user_to_sec_user();

COMMIT;
```

---

## 6. テスト計画

### 6.1 共通テスト項目（全トリガー共通）

| # | テスト種別 | 内容 | 確認項目 |
|---|-----------|------|---------|
| T-00-1 | 循環防止 | テーブルA INSERT → テーブルB同期 → テーブルAの再トリガーが発火しないこと | `sync.in_progress` 変数が正しく機能 |
| T-00-2 | トランザクション分離 | 同時に2セッションから更新 | セッション変数がセッション間で干渉しないこと |
| T-00-3 | ロールバック | トリガー内でエラー発生時に同期先もロールバック | 両テーブルの整合性が維持される |
| T-00-4 | エラー通知 | PK変換失敗時にpg_notifyが発行される | LISTEN/NOTIFYでエラーを受信可能 |

### 6.2 Task A-2: c_kkbn <-> m_contract_type

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-A2-1 | INSERT同期 (SQL2→SQL1) | `INSERT INTO m_contract_type (contract_type_cd, contract_type_nm) VALUES ('05', '転リース')` | `c_kkbn` に `(5, '転リース')` が追加される |
| T-A2-2 | INSERT同期 (SQL1→SQL2) | `INSERT INTO c_kkbn (kkbn_id, kkbn_nm) VALUES (6, '短期')` | `m_contract_type` に `('06', '短期')` が追加される |
| T-A2-3 | UPDATE同期 (SQL2→SQL1) | `UPDATE m_contract_type SET contract_type_nm = 'ファイナンスリース' WHERE contract_type_cd = '01'` | `c_kkbn` の `kkbn_id=1` の名称が更新される |
| T-A2-4 | UPDATE同期 (SQL1→SQL2) | `UPDATE c_kkbn SET kkbn_nm = 'OPリース' WHERE kkbn_id = 2` | `m_contract_type` の `contract_type_cd='02'` の名称が更新される |
| T-A2-5 | PK変換境界値 | `kkbn_id = 0` → `contract_type_cd = '00'` の双方向変換 | 正しく変換される |
| T-A2-6 | 重複INSERT | 既存レコードと同じPKでINSERT | ON CONFLICTにより名称のみ更新される |
| T-A2-7 | 回帰テスト | `KlsryoCalculationEngine` から `c_kkbn` を参照する既存処理 | 正常動作（影響なし） |

### 6.3 Task A-5: m_corp <-> m_company

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-A5-1 | INSERT同期 (SQL2→SQL1) | `INSERT INTO m_company (company_cd, company_nm) VALUES ('C001', 'テスト法人')` | `m_corp` に `corp1_cd='C001'` レコードが追加 |
| T-A5-2 | INSERT同期 (SQL1→SQL2) | `INSERT INTO m_corp (corp_id, corp1_cd, corp1_nm) VALUES (100, 'C002', 'テスト法人2')` | `m_company` に `company_cd='C002'` レコードが追加 |
| T-A5-3 | UPDATE同期 双方向 | 両テーブルの名称を変更 | 対向テーブルの名称も更新される |
| T-A5-4 | 3セットcd/nm | cd2, cd3 を含むレコードを投入 | 全3セットが正しく同期される |
| T-A5-5 | 文字列長超過 | company_nm に100文字 → corp1_nm(40文字)へ | LEFT()により40文字で切り詰められる |

### 6.4 Task A-3: m_lcpt <-> m_supplier

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-A3-1 | INSERT同期 (SQL2→SQL1) | m_supplier に新規レコード投入（締日row1-3含む） | m_lcpt に対応レコードが作成される（lcpt_id自動採番） |
| T-A3-2 | UPDATE同期 (SQL2→SQL1) | m_supplier の締日条件を変更 | m_lcpt の対応カラムが更新される |
| T-A3-3 | 逆方向同期 | m_lcpt に新規レコード投入 | m_supplier にUPSERTされる（基本カラムのみ） |
| T-A3-4 | SQL1固有カラム維持 | m_supplier を更新 | m_lcpt の sum1-3, shho_id 系カラムが変更されないこと |
| T-A3-5 | FK整合性 | m_supplier 投入後にctb_lease_integrated.supplier_cd で参照 | FK制約違反なし |
| T-A3-6 | NULL締日 | 締日row2, row3がNULLのレコード | NULLが正しく同期される |

### 6.5 Task A-4: m_bcat <-> m_department

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-A4-1 | INSERT同期 (SQL2→SQL1) | m_department に新規部門投入 | m_bcat に対応レコード作成（bcat_id自動採番、SQL1固有カラムはNULL） |
| T-A4-2 | v_unified_department | 統合ビューを参照 | 両テーブルのデータがFULL OUTER JOINで表示、sync_status列が正しい |
| T-A4-3 | 間接参照整合性 | m_kknri.kknri1_cd で参照されるdept_cdが存在するか確認 | 整合性が維持される |
| T-A4-4 | 集約カテゴリ同期 | agg_category1-3 を設定 | m_bcat の sum1-3 に反映される |

### 6.6 Task A-6: sec_user <-> tm_USER

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-A6-1 | INSERT同期 | tm_USER に新規ユーザー投入 | sec_user と tw_m_user に基本情報が同期される |
| T-A6-2 | role→kngn_id変換 | role='admin' のユーザーを投入 | sec_user.kngn_id = 1（マッピングテーブル参照） |
| T-A6-3 | パスワード非同期 | tm_USER のpassword_hashを変更 | sec_user.pwd は変更されないこと（平文のままであること） |
| T-A6-4 | 不明role | role='unknown' のユーザーを投入 | デフォルトkngn_id=4（viewer）で同期 |
| T-A6-5 | 既存ログインテスト | Form_f_LOGIN_JET からログイン | sec_user 参照の既存認証が正常動作 |
| T-A6-6 | AuthorizationService | tm_USER 参照の新認証フロー | 正常動作 |

### 6.7 Task B-1: sp_migrate_to_ctb

| # | テスト種別 | 操作 | 期待結果 |
|---|-----------|------|---------|
| T-B1-1 | 初回実行 | `SELECT * FROM sp_migrate_to_ctb()` | inserted_count > 0, updated_count = 0 |
| T-B1-2 | 再実行（UPSERT） | d_kykh の一部を更新後に再実行 | updated_count > 0, 変更カラムが反映 |
| T-B1-3 | ctb_property自動作成 | 新規契約のマイグレーション | ctb_property にレコードが作成される |
| T-B1-4 | d_haif複数配賦 | 複数部門配賦のある契約をマイグレーション | ctb_dept_allocation に複数レコードが作成される |
| T-B1-5 | エラーハンドリング | 不正データ（NULLの契約番号等）を含むd_kykh | error_count > 0, 他のレコードは正常処理継続 |
| T-B1-6 | エラー通知 | エラー発生時 | pg_notify('migration_error', ...) が発行される |
| T-B1-7 | 空契約番号スキップ | kykbnj が空文字列のレコード | skipped_count に含まれる |

### 6.8 回帰テスト（全Phase共通）

| # | 対象 | 確認内容 |
|---|------|---------|
| T-REG-1 | KeijoSqlBuilder | d_kykh/d_kykm/d_haif の結合クエリが正常動作すること |
| T-REG-2 | KlsryoCalculationEngine | c_kkbn, m_lcpt, m_bcat への参照が正常であること |
| T-REG-3 | MasterDataLoader | m_contract_type, m_supplier, m_department のロードが正常であること |
| T-REG-4 | AuthorizationService | tm_USER 参照のログイン・認証が正常であること |
| T-REG-5 | CtbRepository.InsertAll | 既存の一括INSERTフローが正常動作すること |
| T-REG-6 | 仕訳パイプライン | tw_s_chuki_keijo → 仕訳出力までのE2Eフローが正常であること |

---

## 付録: トリガー循環防止メカニズムの詳細

### PostgreSQLセッション変数方式

```
set_config('sync.in_progress', 'true', TRUE)
                                         ↑ is_local = TRUE
```

`is_local = TRUE` を指定することで、SET LOCAL と同等の動作となる。すなわち:
- 現在のトランザクション内でのみ有効
- トランザクション終了（COMMIT/ROLLBACK）時に自動リセット
- 他セッションに一切影響しない

### 動作フロー

```
Session 1:
  BEGIN;
    UPDATE m_contract_type SET contract_type_nm = 'FA' WHERE contract_type_cd = '01';
    → trg_contract_type_to_kkbn 発火
      → is_sync_in_progress() = false
      → set_config('sync.in_progress', 'true', TRUE)
      → UPDATE c_kkbn SET kkbn_nm = 'FA' WHERE kkbn_id = 1
        → trg_kkbn_to_contract_type 発火
          → is_sync_in_progress() = true  ← ここでガード
          → RETURN NEW (何もせず終了)
      → trg_contract_type_to_kkbn 終了
    → UPDATE 完了
  COMMIT;  ← sync.in_progress 自動リセット

Session 2 (並行):
  → is_sync_in_progress() = false  ← Session 1 の変数は見えない
```

### 注意事項

- SAVEPOINTを使用した部分ロールバック時にもセッション変数はリセットされるため、ネストしたトランザクション内での使用に注意が必要
- 大量データの一括更新時はトリガーを一時無効化（`ALTER TABLE ... DISABLE TRIGGER`）することを検討
