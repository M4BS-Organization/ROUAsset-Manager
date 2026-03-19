# 要件定義書: tw-schema-design

## 1. 機能概要

Access版で3つの別MDB（LM4BSwork.mdb / LM4BSmdld.mdb / LM4BSImpWkDB.mdb）に分散していた tw_ 系ワークテーブルを、PostgreSQL の単一データベース内で再設計・統合管理する。

## 2. 機能要件

### FR-001: 不足DDLの整備
- tw_s_chuki_calc（注記計算結果）のDDLを作成する
- tw_s_keijo_joken / tw_s_tougetsu_joken / tw_s_saimu_joken（条件ワーク）のDDLを作成する
- tw_f_仕訳出力標準_kj/sh/sm および _仕訳data（仕訳出力ワーク）のDDLを作成する

### FR-002: テーブルネーミング規則
- すべてのワークテーブルは `tw_` プレフィックスを持つ
- サブプレフィックス: `tw_s_`（月次計算）、`tw_d_`（変動/減損）、`tw_f_`（フォーム連動）、`tw_kitoku_`（KITOKU出力）

### FR-003: スキーマ配置
- 全ワークテーブルを `public` スキーマに配置（既存コードとの互換性）

### FR-004: クリア操作
- 各ワークテーブルは `DELETE FROM <table>` で全件クリア（既存パターン維持）

### FR-005: 42P01エラーハンドリング削除
- DDL整備に伴い、tw_s_chuki_calc の undefined_table サイレント無視を削除

## 3. 非機能要件

### NFR-001: パフォーマンス
- ワークテーブルのクリアが1秒以内に完了すること
- kykm_id 指定のSELECTが全件スキャンを回避すること（インデックス）

### NFR-002: トランザクション整合性
- CrudHelper のトランザクション管理と互換であること
- 部分書き込み失敗時にロールバック可能であること

### NFR-003: 同時実行
- シングルユーザー運用を前提（排他制御は不要）

## 4. 完了条件（Issue #35）

- [x] ワークテーブル設計書が完成
- [x] PostgreSQLスキーマ（DDL）が作成済み
- [x] クリア・管理手順が文書化

## 5. スコープ外

- UNLOGGED TABLE / TEMPORARY TABLE の活用（将来最適化として別Issue化）
- 専用スキーマへの分離
- Access版 LM4BSmdld.mdb / LM4BSImpWkDB.mdb 相当のテーブル（コード上の参照なし）
