# SQL ディレクトリ構成

## 3層アーキテクチャ

| 層 | ディレクトリ | 役割 | テーブル例 |
|----|------------|------|-----------|
| **マイグレーション側** | `10_ddl_core/`, `20_ddl_work/` | Access移植の契約・物件データ（業務の原本） | d_kykh, d_kykm, c_kkbn, m_lcpt |
| **共通基盤** | `40_views_triggers/`, `50_stored_procedures/` | マスタ同期トリガー、マッピングビュー、移行SP | v_contract_full, sp_migrate_d_kykh_to_ctb |
| **新リース対応側** | `30_ddl_newlease/` | ASBJ 34/33 会計計算 + d_kykhと型統一した契約情報 | ctb_lease_integrated, ctb_contract_header |

### テーブル間の関係

```
d_kykh（マイグレーション側）         ctb_lease_integrated（定義書準拠・会計計算）
  契約の「What/Who/When」              会計の「How much/How long」
       │                                     │
       │ kykh_id                              │ ctb_id
       ▼                                     ▼
ctb_contract_header ─── ctb_id FK ──→ ctb_lease_integrated
  契約基本情報(d_kykh型統一)             会計パラメータ+計算結果
       │                                     │
       │ ctb_id                              │ ctb_id
       ▼                                     ▼
ctb_contract_property              ctb_dept_allocation
  物件明細(d_kykm型統一)              多部門配賦(定義書準拠)

                                   ctb_remeasurement_history
                                     再測定履歴(定義書準拠)

tw_lease_schedule〜journal (7テーブル)
  支払・会計・判定 ワークテーブル
  ※kykh_id で d_kykh を参照
```

## 実行順序

本番環境・新規セットアップ時は以下の順序で実行してください。

```
00_init/001_init.sql              ← DB・ユーザー作成（postgresで実行）
  ↓
10_ddl_core/101_code_tables.sql   ← コードテーブル (c_*) 10テーブル
10_ddl_core/102_master_tables.sql ← マスタテーブル (m_*) 19テーブル
10_ddl_core/103_data_tables.sql   ← データテーブル (d_*) 6テーブル ※型統一済(DATE/NUMERIC)
10_ddl_core/104_security_tables.sql ← セキュリティ (sec_* + tw_m_user) 5テーブル
10_ddl_core/105_system_tables.sql ← システム/設定 (t_*) 18テーブル
10_ddl_core/106_calc_tables.sql   ← 計算テーブル (tc_*) 5テーブル
10_ddl_core/107_log_tables.sql    ← ログテーブル (l_*) 3テーブル
10_ddl_core/108_indexes_and_fkeys.sql ← PK・INDEX・FK制約（※必ず最後）
  ↓
20_ddl_work/201〜207              ← ワークテーブル（10_ddl_coreに依存なし、順不同）
  ↓
30_ddl_newlease/301_master_tables.sql   ← 新リース用マスタ
30_ddl_newlease/302_tw_lease_tables.sql ← 新リース用ワーク 7テーブル（d_kykh参照）
30_ddl_newlease/303_ctb_tables.sql      ← CTB定義書準拠 + ctb_contract_header/property
30_ddl_newlease/304_ctb_property_eav.sql ← EAV物件属性（303に依存）
30_ddl_newlease/305_alter_tm_user.sql   ← ユーザー拡張（104に依存）
  ↓
40_views_triggers/401_sync_kkbn_contract_type.sql  ← c_kkbn↔m_contract_type同期（※必ず最初: 共通関数定義）
40_views_triggers/402_sync_lcpt_supplier.sql       ← m_lcpt↔m_supplier同期
40_views_triggers/403_sync_bcat_department.sql     ← m_bcat↔m_department同期 + v_unified_department
40_views_triggers/404_sync_corp_company.sql        ← m_corp↔m_company同期
40_views_triggers/405_sync_user_auth.sql           ← sec_user↔tm_USER同期 + t_role_kngn_mapping
  ↓
30_ddl_newlease/306_v_contract_mapping.sql         ← d_kykh↔ctb統合ビュー（マスタ同期後に実行）
  ↓
50_stored_procedures/501_sp_migrate_to_ctb.sql     ← d_kykh→ctb差分同期SP
  ↓
80_seed/801_seed_core.sql              ← 権限・オプション初期データ
80_seed/802_seed_newlease_master.sql   ← 新リース用マスタ初期データ（※トリガーによりSQL1側にも自動同期）
```

## テスト環境のみ（追加）

```
90_testdata/901_testdata_equivalence.sql   ← 等価性テスト用データ
90_testdata/902_testdata_d_kykh.sql        ← 契約テストデータ
90_testdata/903_testdata_users.sql         ← テストユーザー
90_testdata/904_seed_property_attr_def.sql ← 属性定義マスタ
90_testdata/905_testdata_property.sql      ← 物件テストデータ
```

## ワンショット移行スクリプト

```
99_migration/991_migrate_eav.sql           ← ctb→EAV構造移行
99_migration/992_alter_ctb_drop_columns.sql ← 種別固有カラム削除
```

## ディレクトリ番号体系

| 番号帯 | 用途 | 備考 |
|--------|------|------|
| 00 | DB初期化 | postgresスーパーユーザーで実行 |
| 10 | マイグレーション側DDL | d_*, c_*, m_* (型統一済: DATE/NUMERIC) |
| 20 | マイグレーション側ワーク | 仕訳出力・計上条件等 |
| 30 | 新リース対応側DDL | CTB定義書準拠 + d_kykh型統一テーブル + ワーク |
| 40 | 共通基盤: マスタ同期 | 401が共通関数定義のため必ず最初に実行 |
| 50 | 共通基盤: SP | d_kykh→ctb同期 |
| 80 | 本番用初期データ | |
| 90 | テスト用データ | 本番環境では実行しない |
| 99 | ワンショット移行 | 1回のみ実行 |

## 型統一ルール

d_kykh/d_kykm（マイグレーション側）と ctb_contract_header/ctb_contract_property（新リース側）で
項目名・型を統一しています。

| 項目種別 | 統一型 | 旧型（Access移植時） |
|---------|--------|-------------------|
| 日付 | `DATE` | timestamp without time zone |
| 金額 | `NUMERIC(15,2)` | double precision |
| 料率 | `NUMERIC(10,6)` or `NUMERIC(5,4)` | double precision |
| 連番 | `INTEGER` | double precision |
| 文字列 | `VARCHAR(N)` 同一桁数 | 変更なし |

## DB接続情報

| 項目 | 値 |
|------|-----|
| DB名（本番） | `lease_m4bs` |
| DB名（開発） | `lease_m4bs_dev` |
| DB名（テスト） | `lease_m4bs_test` |
| ユーザー | `lease_m4bs_user` |
| スキーマ | `public` |
