# システム設計書: tw-schema-design

## 1. 設計方針

| 項目 | 決定 | 根拠 |
|------|------|------|
| テーブル種別 | 通常テーブル（CREATE TABLE） | 既存パターンとの一貫性 |
| スキーマ配置 | `public` スキーマ + `tw_` 接頭辞 | 既存コード互換 |
| データクリア | `DELETE FROM` | CrudHelper トランザクション互換 |
| インデックス | B-tree | 既存パターン踏襲 |

## 2. ワークテーブル全体マップ（全21テーブル）

### 2.1 月次計算ワーク（tw_s_*）— Access版 LM4BSwork.mdb 相当

| # | テーブル名 | DDLファイル | カラム数 | 用途 |
|---|---|---|---|---|
| 1 | tw_s_chuki_keijo | 003_tw_keijo_tables.sql | 43+id | 注記計上結果 |
| 2 | tw_s_chuki_calc | **007_tw_chuki_calc.sql** | 55+id | 注記計算結果 |
| 3 | tw_s_keijo_joken | **008_tw_joken_tables.sql** | 10+id | 計上条件 |
| 4 | tw_s_tougetsu_joken | **008_tw_joken_tables.sql** | 4+id | 当月条件 |
| 5 | tw_s_saimu_joken | **008_tw_joken_tables.sql** | 3+id | 債務条件 |

### 2.2 変動/減損ワーク（tw_d_*）

| # | テーブル名 | DDLファイル | カラム数 | 用途 |
|---|---|---|---|---|
| 6 | tw_d_henl_keijo | 003_tw_keijo_tables.sql | 43+id | 変額リース仕訳 |
| 7 | tw_d_gson_keijo | 003_tw_keijo_tables.sql | 12+id | 減損仕訳 |

### 2.3 フォーム連動ワーク（tw_f_*）

| # | テーブル名 | DDLファイル | カラム数 | 用途 |
|---|---|---|---|---|
| 8 | tw_f_仕訳出力標準_設定_swksh | 004_tw_settei_tables.sql | 多数 | SH設定 |
| 9 | tw_f_仕訳出力標準_設定_swkkj | 004_tw_settei_tables.sql | 多数 | KJ設定 |
| 10 | tw_f_仕訳出力標準_設定_swksm | 004_tw_settei_tables.sql | 多数 | SM設定 |
| 11 | tw_f_仕訳出力標準_設定_swkky | 004_tw_settei_tables.sql | 3+id | 共通設定 |
| 12 | tw_f_仕訳出力標準_kj | **009_tw_shiwake_output_tables.sql** | 2+id | KJ出力制御 |
| 13 | tw_f_仕訳出力標準_kj_仕訳data | **009_tw_shiwake_output_tables.sql** | 29+id | KJ仕訳データ |
| 14 | tw_f_仕訳出力標準_sh | **009_tw_shiwake_output_tables.sql** | 3+id | SH出力制御 |
| 15 | tw_f_仕訳出力標準_sh_仕訳data | **009_tw_shiwake_output_tables.sql** | 32+id | SH仕訳データ |
| 16 | tw_f_仕訳出力標準_sm | **009_tw_shiwake_output_tables.sql** | 3+id | SM出力制御 |
| 17 | tw_f_仕訳出力標準_sm_仕訳data | **009_tw_shiwake_output_tables.sql** | 25+id | SM仕訳データ |

### 2.4 KITOKU固定長出力ワーク（tw_kitoku_*）

| # | テーブル名 | DDLファイル | カラム数 | 用途 |
|---|---|---|---|---|
| 18 | tw_kitoku_cmsw2wrk | 004_tw_kitoku_tables.sql | 52+id | 伝票ワーク |
| 19 | tw_kitoku_apgdhwrk | 004_tw_kitoku_tables.sql | 72+id | 金額概要ワーク |
| 20 | tw_kitoku_apgddwrk | 004_tw_kitoku_tables.sql | 31+id | 金額詳細ワーク |
| 21 | tw_kitoku_apgdswrk | 004_tw_kitoku_tables.sql | 62+id | 支払ワーク |

### 2.5 マスター系ワーク

| # | テーブル名 | DDLファイル | 備考 |
|---|---|---|---|
| - | tw_m_user | 001_ddl.sql | テスト用。ワークテーブルではないが tw_ プレフィックスを持つ |

## 3. DDLファイル構成

### 実行順序

| 順番 | ファイル | 内容 | 状態 |
|------|----------|------|------|
| 1 | 000_init.sql | 初期化 | 既存 |
| 2 | 001_ddl.sql | メインテーブル（66テーブル） | 既存 |
| 3 | 002_seed_dev.sql | 開発用シードデータ | 既存 |
| 4 | 003_tw_keijo_tables.sql | 計上ワーク（3テーブル） | 既存 |
| 5 | 004_tw_kitoku_tables.sql | KITOKU出力ワーク（4テーブル） | 既存 |
| 6 | 004_tw_settei_tables.sql | 設定ワーク（4テーブル） | 既存 |
| 7 | 005_seed_test_equivalence.sql | テストシード | 既存 |
| 8 | 006_alter_log_tables.sql | ログテーブル変更 | 既存 |
| 9 | **007_tw_chuki_calc.sql** | 注記計算結果ワーク | **新規** |
| 10 | **008_tw_joken_tables.sql** | 条件ワーク（3テーブル） | **新規** |
| 11 | **009_tw_shiwake_output_tables.sql** | 仕訳出力ワーク（6テーブル） | **新規** |

## 4. ライフサイクル管理

### 4.1 クリアタイミング

| カテゴリ | クリアタイミング | 方法 |
|----------|-----------------|------|
| 計上ワーク（tw_s_chuki_keijo/tw_d_*） | 月次計上処理開始時 | KeijoWorkTableManager.ClearAll() |
| 注記計算結果（tw_s_chuki_calc） | 注記計算処理開始時 | KeijoWorkTableManager.ClearChukiCalc() |
| 条件ワーク（tw_s_*_joken） | 条件設定フォーム実行時 | 各フォームが DELETE → INSERT |
| 仕訳出力制御（tw_f_仕訳出力標準_*） | 仕訳出力フォーム開始時 | DELETE FROM → INSERT |
| 仕訳データ（tw_f_*_仕訳data） | 仕訳データ作成処理開始時 | DELETE FROM WHERE TRUE |
| 設定ワーク（tw_f_*_設定_swk*） | 設定画面開始時 | SetteiHelper.ClearAllWorkTables() |
| KITOKU出力（tw_kitoku_*） | KITOKU出力処理開始時 | DELETE FROM |

### 4.2 トランザクション戦略

```
計上処理:
  BeginTransaction
  ├─ ClearAll()                    ← DELETE FROM 3テーブル
  ├─ 月別ループ:
  │   ├─ KeijoCalculationEngine.Execute()
  │   └─ InsertWorkRows(rows)     ← INSERT ループ
  └─ Commit / Rollback

仕訳出力:
  DELETE FROM tw_f_*_仕訳data
  ├─ パターン別 INSERT ループ
  └─ Excel出力 / KITOKU出力
```

## 5. インデックス設計

### 既存インデックス

| テーブル | インデックス | カラム |
|----------|------------|--------|
| tw_s_chuki_keijo | idx_tw_s_chuki_keijo_kykm | kykm_id |
| tw_s_chuki_keijo | idx_tw_s_chuki_keijo_keijo_dt | keijo_dt |
| tw_d_henl_keijo | idx_tw_d_henl_keijo_kykm | kykm_id |
| tw_d_henl_keijo | idx_tw_d_henl_keijo_keijo_dt | keijo_dt |
| tw_d_gson_keijo | idx_tw_d_gson_keijo_kykm | kykm_id |
| tw_kitoku_cmsw2wrk | idx_tw_kitoku_cmsw2wrk_den | sw2_den_no, sw2_dc_kbn, sw2_gyo_no |
| tw_kitoku_apgdhwrk | idx_tw_kitoku_apgdhwrk_den | gdh_den_no |
| tw_kitoku_apgddwrk | idx_tw_kitoku_apgddwrk_den | gdd_den_no, gdd_gyo_no |
| tw_kitoku_apgdswrk | idx_tw_kitoku_apgdswrk_den | gds_den_no, gds_gyo_no |

### 新規インデックス

| テーブル | インデックス | カラム |
|----------|------------|--------|
| tw_s_chuki_calc | idx_tw_s_chuki_calc_kykm | kykm_id |
| tw_f_仕訳出力標準_kj_仕訳data | idx_tw_f_kj_仕訳data_seq | 仕訳seqno |
| tw_f_仕訳出力標準_sh_仕訳data | idx_tw_f_sh_仕訳data_seq | 仕訳seqno |
| tw_f_仕訳出力標準_sm_仕訳data | idx_tw_f_sm_仕訳data_seq | 仕訳seqno |

## 6. 将来の最適化候補

| 最適化 | 効果 | リスク |
|--------|------|--------|
| UNLOGGED TABLE化 | 2-3倍の書込高速化 | クラッシュ時データ消失（ワークなので問題なし） |
| TRUNCATE化 | クリアのO(1)化 | CrudHelperトランザクションとの互換確認必要 |
| COPY API利用 | 大量INSERT高速化 | Npgsql BeginBinaryImport の導入コスト |
| バルクINSERT | INSERT文の統合 | コード変更量が大きい |
