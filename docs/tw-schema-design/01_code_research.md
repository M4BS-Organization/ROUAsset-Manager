# コードベース調査資料: tw-schema-design

## 1. プロジェクト概要

- **フレームワーク**: VB.NET 4.7.2 / WinForms
- **DBドライバ**: Npgsql (PostgreSQL)
- **アーキテクチャ**: レイヤードアーキテクチャ（DataAccess層 + WinForms UI層）

## 2. 既存 tw_ テーブル一覧

### DDL管理済み（sql/ 配下に定義あり）

| テーブル名 | DDLファイル | カテゴリ |
|---|---|---|
| tw_s_chuki_keijo | 003_tw_keijo_tables.sql | 注記計上ワーク |
| tw_d_henl_keijo | 003_tw_keijo_tables.sql | 変額仕訳ワーク |
| tw_d_gson_keijo | 003_tw_keijo_tables.sql | 減損仕訳ワーク |
| tw_kitoku_cmsw2wrk | 004_tw_kitoku_tables.sql | KITOKU伝票ワーク |
| tw_kitoku_apgdhwrk | 004_tw_kitoku_tables.sql | KITOKU金額概要ワーク |
| tw_kitoku_apgddwrk | 004_tw_kitoku_tables.sql | KITOKU金額詳細ワーク |
| tw_kitoku_apgdswrk | 004_tw_kitoku_tables.sql | KITOKU支払ワーク |
| tw_f_仕訳出力標準_設定_swksh | 004_tw_settei_tables.sql | SH設定ワーク |
| tw_f_仕訳出力標準_設定_swkkj | 004_tw_settei_tables.sql | KJ設定ワーク |
| tw_f_仕訳出力標準_設定_swksm | 004_tw_settei_tables.sql | SM設定ワーク |
| tw_f_仕訳出力標準_設定_swkky | 004_tw_settei_tables.sql | 共通設定ワーク |

### DDL未管理（コード上で参照されているがDDLなし）

| テーブル名 | 参照元 | 用途 |
|---|---|---|
| tw_s_chuki_calc | KeijoWorkTableManager.vb | 注記計算結果 |
| tw_s_keijo_joken | Form_f_仕訳出力標準_KJ.vb | 計上条件 |
| tw_s_tougetsu_joken | Form_f_仕訳出力標準_SH.vb | 当月条件 |
| tw_s_saimu_joken | Form_f_仕訳出力標準_SM.vb | 債務条件 |
| tw_f_仕訳出力標準_kj | Form_f_仕訳出力標準_KJ.vb | KJ出力制御 |
| tw_f_仕訳出力標準_kj_仕訳data | Form_f_仕訳出力標準_KJ.vb | KJ仕訳データ |
| tw_f_仕訳出力標準_sh | Form_f_仕訳出力標準_SH.vb | SH出力制御 |
| tw_f_仕訳出力標準_sh_仕訳data | Form_f_仕訳出力標準_SH.vb | SH仕訳データ |
| tw_f_仕訳出力標準_sm | Form_f_仕訳出力標準_SM.vb | SM出力制御 |
| tw_f_仕訳出力標準_sm_仕訳data | Form_f_仕訳出力標準_SM.vb | SM仕訳データ |

## 3. データアクセスパターン

### パターンA: KeijoWorkTableManager（計上ワーク専用）
```vb
Dim mgr As New KeijoWorkTableManager(crud)
mgr.ClearAll()               ' DELETE FROM 3テーブル
mgr.InsertWorkRows(rows)     ' TargetTable に応じて振り分けINSERT
mgr.GetChukiKeijoAll()       ' SELECT *
```

### パターンB: CrudHelper 直接呼び出し
```vb
_crud.GetDataTable("SELECT * FROM tw_s_keijo_joken LIMIT 1")
_crud.ExecuteNonQuery("DELETE FROM tw_f_仕訳出力標準_kj WHERE TRUE")
_crud.ExecuteNonQuery("INSERT INTO tw_f_仕訳出力標準_kj (...) VALUES (...)", params)
```

## 4. トランザクション管理

- CrudHelper が `_activeConnection` / `_activeTransaction` で状態管理
- MonthlyJournalEngine: 月次計上処理全体を1トランザクションで囲む
- パターン: BeginTransaction → ClearAll → ループ(Calculate → InsertWorkRows) → Commit

## 5. 接続管理

- DbConnectionManager: App.config → 環境変数 → デフォルトの順で接続文字列取得
- Npgsql コネクションプーリング有効（デフォルト）

## 6. 命名規則

| プレフィックス | 意味 | 例 |
|---|---|---|
| tw_s_ | 月次計算系ワーク | tw_s_chuki_keijo |
| tw_d_ | 差分/変動系ワーク | tw_d_henl_keijo |
| tw_f_ | フォーム連動ワーク | tw_f_仕訳出力標準_kj |
| tw_kitoku_ | KITOKU固定長出力ワーク | tw_kitoku_cmsw2wrk |
| tw_m_ | マスター系ワーク | tw_m_user |

## 7. 既存テストパターン

- ブラックボックステスト: ルートの `test_*_blackbox.vb` → コンパイル済み `.exe`
- DB実接続で検証（モック不使用）
- `Console.WriteLine` で結果出力するコンソールアプリスタイル
- Issue番号・テストケースIDでトレーサビリティ確保
