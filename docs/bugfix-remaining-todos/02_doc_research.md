# ドキュメント調査: Issue #17 バグ修正 + 残TODO消化

## 1. VB.NET WinForms Option Strict On / Option Explicit On のバグパターンとベストプラクティス

**推奨設定**: Option Strict On, Option Explicit On, Option Infer On

**Option Strict On の主な効果**:
- 暗黙の縮小変換（narrowing conversion）をコンパイル時にエラーとして検出（ランタイムでのデータロスを防止）
- Late Binding を禁止（タイポがコンパイル時に検出される）
- メソッド呼び出しが Early Bound になりパフォーマンス向上
- ランタイム例外ではなくコンパイルエラーとして検出されるため、バグの早期発見が可能

**Option Explicit On の効果**:
- 変数の明示的宣言を強制（未宣言変数の使用を防止）
- Off にする正当な理由は存在しない

**よくあるバグパターン**:
- `Object` 型から具体的な型への暗黙キャスト（Option Strict On で検出可能）
- DBNull.Value と Nothing の混同
- 文字列結合での暗黙型変換

**WinForms固有の注意点**: .NET 5以降ではプロジェクト設定の Option Strict が無視される場合があり、コードファイルに直接記述が必要な場合がある。ただし .NET Framework 4.7.2 では問題なし。

## 2. Npgsql (PostgreSQL .NET ドライバ) の接続管理ベストプラクティス

**接続プーリング**:
- Npgsql はデフォルトで接続プーリングが有効。Close/Dispose しても物理接続は閉じられず、内部プールに返却される
- 物理接続の開閉は高コストなので、プーリングを活用すべき

**接続のライフサイクル管理**:
- 接続は可能な限り短時間だけオープンする
- 必ず `Using` ブロックで Dispose する（例外発生時もリソースリークを防止）
- Dispose しないと接続リークが発生し、プログラムがクラッシュする可能性がある

**エラーハンドリングとリトライ**:
- NpgsqlException をキャッチし、SQLSTATE を検査してリトライ判断
- SELECT（読み取り専用）は安全にリトライ可能
- INSERT/UPDATE/DELETE はべき等性を考慮する必要あり

## 3. Access から VB.NET/PostgreSQL への移行時のよくある問題

**NULL処理の違い（最重要）**:
- Access: `'Hello' & Null = 'Hello'`（Null が無視される）
- PostgreSQL: `'Hello' || NULL = NULL`（Null が伝播する）
- VBA の `Nz()` 関数は .NET には存在しない。カスタム実装が必要

**DBNull vs Nothing の区別**:
- `DBNull.Value` はDB側のNULL、`Nothing` はVB.NET側のnull参照
- DataReader から値を読む前に必ず `IsDBNull()` チェックが必要
- Nullable型（`Integer?`, `DateTime?`）を活用して DB の NULL を自然に表現

**日付処理**:
- ISO-8601 フォーマットが PostgreSQL で最も安全
- Access の日付リテラル `#2024/01/01#` は VB.NET/PostgreSQL では使えない

**文字列比較**:
- Access はデフォルトで大文字小文字を区別しない
- PostgreSQL はデフォルトで大文字小文字を区別する（`ILIKE` や `LOWER()` で対応）

**パラメータの NULL 渡し**:
- SQL パラメータに NULL を渡す場合、`Nothing` ではなく `DBNull.Value` を使用する必要がある

## 4. VB.NET の例外処理・エラーハンドリングのベストプラクティス

**ベストプラクティス**:
1. Catch ブロックでは派生度の高い例外から順に記述
2. 発生頻度の高い条件は事前チェックで回避
3. 通常のフロー制御に例外を使わない
4. 処理できない例外は上位に伝播させる
5. リソース解放は必ず Finally または Using で行う

**Access VBA との違い**:
- VBA: `On Error GoTo` / `On Error Resume Next`（非構造化）
- VB.NET: `Try/Catch/Finally`（構造化例外処理）
- VBA の `On Error Resume Next` は VB.NET では絶対に使わない

## 5. Issue #17 で特に注意すべきポイント

1. **NULL処理**: Access の `Nz()` が正しく VB.NET に移行されているか確認。`DBNull.Value` チェック漏れがバグの最大原因
2. **型安全性**: Option Strict On で検出される暗黙変換を全て明示的キャストに修正
3. **接続管理**: Npgsql 接続が全て `Using` ブロック内で管理されているか確認（リーク防止）
4. **文字列比較**: PostgreSQL での大文字小文字区別に注意
5. **例外処理**: VBA の `On Error Resume Next` パターンが残っていないか確認
