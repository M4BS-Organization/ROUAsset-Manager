# SQL ディレクトリ構成

## 実行順序

本番環境・新規セットアップ時は以下の順序で実行してください。

```
00_init/001_init.sql              ← DB・ユーザー作成（postgresで実行）
  ↓
10_ddl_core/101_code_tables.sql   ← コードテーブル (c_*) 10テーブル
10_ddl_core/102_master_tables.sql ← マスタテーブル (m_*) 19テーブル
10_ddl_core/103_data_tables.sql   ← データテーブル (d_*) 6テーブル
10_ddl_core/104_security_tables.sql ← セキュリティ (sec_* + tw_m_user) 5テーブル
10_ddl_core/105_system_tables.sql ← システム/設定 (t_*) 18テーブル
10_ddl_core/106_calc_tables.sql   ← 計算テーブル (tc_*) 5テーブル
10_ddl_core/107_log_tables.sql    ← ログテーブル (l_*) 3テーブル
10_ddl_core/108_indexes_and_fkeys.sql ← PK・INDEX・FK制約（※必ず最後）
  ↓
20_ddl_work/201〜207              ← ワークテーブル（10_ddl_coreに依存なし、順不同）
  ↓
30_ddl_newlease/301_master_tables.sql   ← 新リース用マスタ
30_ddl_newlease/302_tw_lease_tables.sql ← 新リース用トランザクション（301に依存）
30_ddl_newlease/303_ctb_tables.sql      ← CTB統合テーブル（301に依存）
30_ddl_newlease/304_ctb_property_eav.sql ← EAV物件管理（301,303に依存）
30_ddl_newlease/305_alter_tm_user.sql   ← ユーザー拡張（104に依存）
  ↓
40_views_triggers/401_sync_kkbn_contract_type.sql  ← c_kkbn↔m_contract_type同期（※必ず最初: 共通関数定義）
40_views_triggers/402_sync_lcpt_supplier.sql       ← m_lcpt↔m_supplier同期
40_views_triggers/403_sync_bcat_department.sql     ← m_bcat↔m_department同期 + v_unified_department
40_views_triggers/404_sync_corp_company.sql        ← m_corp↔m_company同期
40_views_triggers/405_sync_user_auth.sql           ← sec_user↔tm_USER同期 + t_role_kngn_mapping
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

## ストアドプロシージャ

```
50_stored_procedures/501_sp_migrate_to_ctb.sql ← d_kykh→ctb_lease_integrated移行
```

## ディレクトリ番号体系

| 番号帯 | 用途 | 備考 |
|--------|------|------|
| 00 | DB初期化 | postgresスーパーユーザーで実行 |
| 10 | Access移植DDL (SQL1) | 旧001_ddl.sqlを分割 |
| 20 | ワークテーブルDDL (SQL1) | 仕訳出力・計上条件等 |
| 30 | 新リースDDL (SQL2) | ASBJ対応・EAV |
| 40 | マスタ同期トリガー・統合ビュー | 401が共通関数定義のため必ず最初に実行 |
| 50 | ストアドプロシージャ | Phase B実装時に拡充 |
| 80 | 本番用初期データ | |
| 90 | テスト用データ | 本番環境では実行しない |
| 99 | ワンショット移行 | 1回のみ実行 |

## DB接続情報

| 項目 | 値 |
|------|-----|
| DB名（本番） | `lease_m4bs` |
| DB名（開発） | `lease_m4bs_dev` |
| DB名（テスト） | `lease_m4bs_test` |
| ユーザー | `lease_m4bs_user` |
| スキーマ | `public` |

## テーブル系統

| 系統 | テーブル数 | 命名規則 | 用途 |
|------|-----------|---------|------|
| SQL1（Access移植） | 66+23ワーク | 日本語略称 (d_kykh, m_lcpt) | 既存会計機能 |
| SQL2（新リース） | 25+ビュー | 英語 (tw_lease_contract, m_department) | 新リース基準対応 |
