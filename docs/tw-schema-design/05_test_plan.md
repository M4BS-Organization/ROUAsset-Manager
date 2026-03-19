# テスト計画書: tw-schema-design

## 1. テスト対象

- 新規DDL: 007_tw_chuki_calc.sql, 008_tw_joken_tables.sql, 009_tw_shiwake_output_tables.sql
- VB.NET修正: KeijoWorkTableManager.vb の 42P01 サイレント無視削除
- 全21 tw_ テーブルの存在確認

## 2. テスト方針

- DB実接続によるブラックボックステスト（既存パターン準拠）
- Console.WriteLine による結果出力
- テストケースID付きでトレーサビリティ確保

## 3. テストケース

### TC-TW-001: 全tw_テーブル存在確認
- information_schema.tables で全21テーブルの存在を検証
- PASS条件: 全テーブルが見つかること

### TC-TW-002: tw_s_chuki_calc CRUD
- ClearChukiCalc → INSERT → GetChukiCalcAll → COUNT
- PASS条件: INSERT/SELECT/DELETE が正常動作、42P01エラーが出ないこと

### TC-TW-003: 条件ワークテーブル CRUD
- tw_s_keijo_joken / tw_s_tougetsu_joken / tw_s_saimu_joken
- DELETE FROM → INSERT → SELECT * LIMIT 1
- sw_rsok の BOOLEAN 読み取り確認

### TC-TW-004: 仕訳出力ワークテーブル CRUD
- tw_f_仕訳出力標準_kj/sh/sm と _仕訳data
- 日本語テーブル名のクォート処理確認
- DELETE FROM → INSERT → COUNT

### TC-TW-005: カラム数検証
- 各テーブルの information_schema.columns でカラム数を検証
- コードのINSERT文と一致すること

### TC-TW-006: インデックス存在確認
- pg_indexes で新規インデックスの存在を検証

## 4. テスト実行方法

```bash
# コンパイル
vbc /out:test_tw_schema_blackbox.exe /r:Npgsql.dll test_tw_schema_blackbox.vb

# 実行
./test_tw_schema_blackbox.exe
```
