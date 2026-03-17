# テスト計画書: monthly-journal-engine

## 1. テスト戦略

- **テストフレームワーク**: 不使用（VB.NET/WinForms プロジェクトのため）
- **テストレベル**: 結合テスト（TestWinForms アプリ経由）+ ビルド検証
- **カバレッジ目標**: 受け入れ基準（要件定義 US-001〜US-007）の全項目を手動検証で確認
- **モック戦略**: モックなし。実際の PostgreSQL（開発 DB）に接続してテストする
- **品質ゲート**: MSBuild でビルドエラー/警告ゼロ（CI: `.github/workflows/ci.yml`）

### テスト方針

このプロジェクトは xUnit 等の自動テストフレームワークを持たないため、以下の 2 段階で品質を担保する。

1. **ビルド検証**: MSBuild ビルドが成功すること（コンパイルエラーなし）
2. **TestWinForms 結合テスト**: `LeaseM4BS.TestWinForms` プロジェクトに `Form_f_KEIJO_JOKEN` を介して実際に DB 接続し、計算結果を手動確認する

既存パターン（`Form_f_flx_KEIJO.vb` の `SearchData` / `ApplyGridStyle` 構造）に合わせ、結果は `DataGridView` に表示して目視確認する。

---

## 2. テスト環境

### 必要なセットアップ手順

1. PostgreSQL 開発 DB を起動し、`002_seed_dev.sql` を適用済みであること
2. テスト用契約データ（後述 §5）を投入すること
3. `tw_s_chuki_keijo` / `tw_d_henl_keijo` / `tw_d_gson_keijo` の DDL が `001_ddl.sql` に追加されていること（本 Issue 実装の前提条件）
4. `LeaseM4BS.sln` をビルドし、`LeaseM4BS.DataAccess.dll` が最新であること

### テストデータの準備方法

```sql
-- テスト用データ投入（§5 テストデータ設計を参照）
psql -U lease_m4bs_user -d lease_m4bs_dev -f sql/003_seed_test_monthly_journal.sql
```

### 外部依存のモック方法

モックは使用しない。`CrudHelper` は実際の Npgsql 接続を使用する。
接続文字列は `app.config` / `DbConnectionManager` に従う。

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | `KeijoTypes.vb`（KeijoJoken クラス + Enum 型定義） | 高 | FR-001 |
| T-002 | `KeijoSqlBuilder`（または `GetSourceData` 相当）のSQL生成 | 高 | FR-003 |
| T-003 | `KeijoCalculationEngine.Execute()` の正常終了と戻り値 | 高 | FR-002, US-001 |
| T-004 | 物件単位集計（ShriMeisai.Kykm）の計算動作 | 高 | US-002, FR-006 |
| T-005 | 配賦単位集計（ShriMeisai.Haif）の計算動作 | 高 | US-003, FR-007 |
| T-006 | 付随費用（RecKbn.Fuzui）の処理 | 中 | FR-008, US-002 |
| T-007 | `MonthlyJournalEngine.Execute()` の月別ループ（複数月） | 高 | FR-002, US-001 |
| T-008 | ワークテーブルへの書込・読込 | 高 | US-004, FR-005 |
| T-009 | 計算エラー時のハンドリング（例外・ロールバック） | 中 | NFR-003, NFR-005 |
| T-010 | Access版との計算結果一致性検証 | 高 | NFR-001 |

---

## 4. テストケース

### TC-001: KeijoTypes の型定義が正しくコンパイルされること

- **対象**: `LeaseM4BS.DataAccess.KeijoJoken`（新規クラス）、`KlsryoTypes.vb` の既存 Enum
- **関連要件**: FR-001
- **種別**: ビルド検証
- **前提条件**: `KeijoJoken` クラスが `KlsryoTypes.vb` または `MonthlyJournalTypes.vb` に定義されていること
- **入力**: ビルドコマンド実行
- **期待結果**:
  - `KeijoJoken` がフィールド `KeijoFrom As Date`, `KeijoTo As Date`, `Taisho As Integer`, `Ktmg As ShriKtmg`, `Meisai As ShriMeisai` を持つ
  - MSBuild がエラーなしで完了する
  - `TestWinForms` から `Dim j As New KeijoJoken()` がコンパイルエラーなく使用できる

---

### TC-002: KeijoSqlBuilder が正しい SQL を生成すること（物件単位/配賦単位）

- **対象**: `MonthlyJournalEngine.GetSourceData()`（Private メソッド、TC-003 経由で間接検証）
- **関連要件**: FR-003, US-002, US-003
- **種別**: 正常系（結合テスト）
- **前提条件**: テスト用契約データが投入済みであること（§5 正常データ参照）
- **入力**:
  - 物件単位: `Meisai = ShriMeisai.Kykm`, `Taisho = 3`（全部）
  - 配賦単位: `Meisai = ShriMeisai.Haif`, `Taisho = 3`（全部）
- **期待結果**:
  - 物件単位: `d_kykh JOIN d_kykm` のレコードが取得される（`k_seigou_f = TRUE` のレコードのみ）
  - 配賦単位: `d_kykh JOIN d_kykm JOIN d_haif` のレコードが取得される
  - `Taisho = 1`（リース料）の場合、`kkbn_id <> 3`（保守以外）でフィルタされる
  - `Taisho = 2`（保守料）の場合、`kkbn_id = 3`（保守のみ）でフィルタされる
- **検証方法**: TestWinForms フォームのデバッグ実行で DataGridView の行数・内容を確認

---

### TC-003: KeijoCalculationEngine.Execute() が正常に完了すること

- **対象**: `MonthlyJournalEngine.Execute(joken As KeijoJoken) As Integer`
- **関連要件**: FR-002, US-001
- **種別**: 正常系（結合テスト）
- **前提条件**:
  - テスト用契約データ（§5 正常データ）が投入済みであること
  - `tw_s_chuki_keijo` テーブルが存在すること
- **入力**:
  ```
  KeijoFrom = 2025/04/01
  KeijoTo   = 2025/04/30
  Taisho    = 3（全部）
  Ktmg      = ShriKtmg.SimeDtBase（締日ベース）
  Meisai    = ShriMeisai.Kykm（物件単位）
  ```
- **期待結果**:
  - 戻り値（書き込み件数）が 0 以上の Integer を返す
  - 例外がスローされない
  - `tw_s_chuki_keijo` に 1 件以上のレコードが存在する

---

### TC-004: 物件単位集計（KYKM）の基本動作

- **対象**: `MonthlyJournalEngine.ProcessKykm()` 相当の内部処理（TC-003 経由で間接検証）
- **関連要件**: US-002, FR-006, FR-007
- **種別**: 正常系（結合テスト）
- **前提条件**: テスト用定額リース契約（TEST-KYKM-001）が投入済みであること
- **入力**:
  ```
  KeijoFrom = 2025/04/01
  KeijoTo   = 2025/04/30
  Meisai    = ShriMeisai.Kykm
  ```
- **期待結果**:
  - `tw_s_chuki_keijo` に物件単位のレコードが INSERT される
  - レコードの `rec_kbn = 1`（定額）である
  - `lsryo_toki` が `d_kykm.b_klsryo` と一致する（月額リース料）
  - `kykm_id` が TEST-KYKM-001 の物件 ID である
  - `shri_dt`（支払日）が 2025/04 内の日付である

---

### TC-005: 配賦単位集計（HAIF）の基本動作

- **対象**: `MonthlyJournalEngine.ProcessHaif()` 相当（TC-003 経由で間接検証）
- **関連要件**: US-003, FR-007
- **種別**: 正常系（結合テスト）
- **前提条件**: テスト用配賦データ（TEST-HAIF-001: 2 行配賦）が投入済みであること
- **入力**:
  ```
  KeijoFrom = 2025/04/01
  KeijoTo   = 2025/04/30
  Meisai    = ShriMeisai.Haif
  ```
- **期待結果**:
  - `tw_s_chuki_keijo` に配賦行ごとのレコードが INSERT される（2 行）
  - 各行の `line_id` が `d_haif.line_id` と一致する
  - 1 行目: `lsryo_toki = FLOOR(月額 × 0.6)` （配賦率 60%、切り捨て）
  - 2 行目: `lsryo_toki = 月額 - 1行目の金額`（残額）
  - 2 行の `lsryo_toki` の合計が物件の月額リース料 `d_kykm.b_klsryo` と一致する

---

### TC-006: 付随費用（HENF）の処理

- **対象**: `MonthlyJournalEngine.ProcessHenf()` 相当（TC-003 経由で間接検証）
- **関連要件**: FR-008, US-002
- **種別**: 正常系（結合テスト）
- **前提条件**: テスト用付随費用データ（TEST-HENF-001）が投入済みであること（`d_kykm.b_henf_f = TRUE`）
- **入力**:
  ```
  KeijoFrom = 2025/04/01
  KeijoTo   = 2025/04/30
  Taisho    = 2（保守料）または 3（全部）
  Meisai    = ShriMeisai.Kykm
  ```
- **期待結果**:
  - `tw_s_chuki_keijo` に `rec_kbn = 3`（付随費用）のレコードが INSERT される
  - `lsryo_toki` が `d_henf.klsryo` と一致する
  - `Taisho = 1`（リース料のみ）の場合は付随費用レコードが存在しない

---

### TC-007: MonthlyJournalEngine.Execute() の月別ループ

- **対象**: `MonthlyJournalEngine.Execute()` の複数月処理
- **関連要件**: FR-002, US-001
- **種別**: 正常系（結合テスト）
- **前提条件**: テスト用契約データが投入済みであること
- **入力**:
  ```
  KeijoFrom = 2025/04/01
  KeijoTo   = 2025/06/30（3ヶ月間）
  Meisai    = ShriMeisai.Kykm
  ```
- **期待結果**:
  - 3 ヶ月分（2025/04, 05, 06）のレコードが `tw_s_chuki_keijo` に INSERT される
  - 各月の `shri_dt`（支払日）が各月内の日付であること
  - 単月実行（TC-003）の 3 回分と同等の件数になること
  - Execute の戻り値が 3 ヶ月分の合計書き込み件数を返すこと

---

### TC-008: ワークテーブルへの書込・読込

- **対象**: `MonthlyJournalEngine.WriteToWorkTable()`、`CrudHelper.BeginTransaction/Commit/Rollback`
- **関連要件**: US-004, FR-005, NFR-003
- **種別**: 正常系 + 異常系（結合テスト）
- **前提条件**:
  - `tw_s_chuki_keijo` テーブルが存在すること
  - テスト用契約データが投入済みであること
- **入力A（正常系）**: TC-003 と同一入力
- **入力B（異常系）**: 存在しないワークテーブル名を指定（テスト専用の異常データ）
- **期待結果（正常系A）**:
  - `Execute()` 実行後、`SELECT COUNT(*) FROM tw_s_chuki_keijo WHERE ...` の件数と戻り値が一致する
  - 同一条件で再実行した場合、既存レコードが DELETE されて新規 INSERT される（二重登録なし）
  - `CrudHelper.IsInTransaction` が Commit 後に `False` であること
- **期待結果（異常系B）**:
  - 例外がスローされる
  - `tw_s_chuki_keijo` にデータが残っていない（ロールバック確認）

---

### TC-009: 計算エラー時のハンドリング

- **対象**: `MonthlyJournalEngine.Execute()`、例外スロー・ロールバック処理
- **関連要件**: NFR-003, NFR-005, US-007
- **種別**: 異常系（結合テスト）
- **前提条件**: 異常データ（§5 異常データ参照）が投入済みであること
- **入力A（DB 接続切断）**: Execute 実行中に DB 接続が切れる状態をシミュレート（接続文字列を不正に変更）
- **入力B（不整合データ）**: `shri_cnt = 0` かつ `b_klsryo > 0` の矛盾データ
- **期待結果（入力A）**:
  - `Exception`（または `NpgsqlException`）がスローされる
  - `tw_s_chuki_keijo` にデータが残っていない
- **期待結果（入力B）**:
  - 集計対象外と判定されてスキップされる（または仕様に応じて `InvalidOperationException` をスロー）
  - エラーメッセージに `kykm_id` が含まれること（仕様に応じて）

---

### TC-010: Access版との計算結果一致性検証

- **対象**: `MonthlyJournalEngine.Execute()` の出力全フィールド
- **関連要件**: NFR-001
- **種別**: 正常系（E2E 検証 — Issue #14 と連携）
- **前提条件**:
  - Access版 pc_月次仕訳計上 と VB.NET版の両方で同一の契約データを使用すること
  - Access版の出力を CSV またはスプレッドシートにエクスポートしておくこと
- **入力**: Access版と同一の契約データ（TEST-KYKM-001〜003 を使用）と同一期間
- **期待結果**:
  - `lsryo_total`（税抜支払総額）が一致する
  - `lsryo_toki`（当月計上額）が一致する
  - `zei_total`（税額総計）が一致する
  - `soukaisu`（総回数）が一致する
  - `sumikaisu_zen`（前期以前済回数）が一致する
  - `keijo_shri_cnt`（計上仕訳回数）が一致する
  - 端数処理: `Math.Floor`（切り捨て）で統一されていること
- **検証方法**: 両版の出力を Excel で比較。差異があれば差額・kykm_id を記録する

---

## 5. テストデータ設計

### 正常データ

以下を `sql/003_seed_test_monthly_journal.sql` として追加する。

| データ名 | 値 | 用途 |
|---|---|---|
| TEST-KYKH-001 | リース契約（kkbn_id=1, kjkbn_id=1:費用, k_seigou_f=TRUE） | 基本動作検証（TC-003, TC-004） |
| TEST-KYKM-001 | 物件（b_klsryo=100000, b_kzei=10000, shri_cnt=60, start_dt='2022-04-01', end_dt='2027-03-31'） | 定額リース月額計算検証 |
| TEST-KYKM-002 | 物件（b_henl_f=TRUE: 変額リース） | 変額スケジュール検証（TC-004 変額分） |
| TEST-KYKM-003 | 物件（b_henf_f=TRUE: 付随費用あり） | 付随費用処理検証（TC-006） |
| TEST-HAIF-001 | 配賦2行（line_id=1: haifritu=0.6, line_id=2: haifritu=0.4） | 配賦単位集計検証（TC-005） |
| TEST-HENF-001 | 付随費用（klsryo=5000, kzei=500, shri_cnt=60） | 付随費用金額検証（TC-006） |
| TEST-HENL-001 | 変額リース（klsryo=120000, shri_cnt=12） | 変額スケジュール検証 |

**サンプル INSERT（TEST-KYKH-001 / TEST-KYKM-001）**:

```sql
-- 支払先マスタ
INSERT INTO m_lcpt (lcpt_id, lcpt1_cd, lcpt1_nm) VALUES (901, 'TEST-L01', 'テスト支払先01')
ON CONFLICT (lcpt_id) DO NOTHING;

-- 管理部署マスタ
INSERT INTO m_bcat (bcat_id, bcat_cd, bcat1_nm) VALUES (901, 'TEST-B01', 'テスト管理部署01')
ON CONFLICT (bcat_id) DO NOTHING;

-- 契約ヘッダ
INSERT INTO d_kykh (
    kykh_id, kykh_no, kkbn_id, lcpt_id, kykbnl,
    start_dt, end_dt, kjkbn_id,
    shri_kn, sshri_kn_m, sshri_kn1, sshri_kn2, sshri_kn3,
    shri_cnt, shri_dt1, shri_dt3,
    mkaisu, jencho_f, k_seigou_f,
    b_klsryo, b_kzei, b_mlsryo, b_mzei, zritu,
    shho_3_id, k_slsryo, k_knyukn,
    kj_shri_cnt, skyu_kj_f
) VALUES (
    9001, 9001.0, 1, 901, 'TEST-9001',
    '2022-04-01', '2027-03-31', 1,
    1, 1, 1, 1, 1,
    60, '2022-04-25', 25,
    60, FALSE, TRUE,
    100000, 10000, 0, 0, 0.10,
    NULL, 6000000, 5500000,
    36, 0
) ON CONFLICT (kykh_id) DO NOTHING;

-- 物件明細
INSERT INTO d_kykm (
    kykm_id, kykh_id, kykm_no, bukn_nm, b_bcat_id,
    b_klsryo, b_kzei, b_mlsryo, b_mzei,
    b_henl_f, b_henf_f,
    b_rend_dt,
    b_smdt_fst_sum, b_smdt_lst_sum,
    b_shdt_fst_sum, b_shdt_lst_sum,
    kjkbn_id, leakbn_id, hensai_kind, b_gson_f, saikaisu
) VALUES (
    9001, 9001, 1, 'テスト物件01', 901,
    100000, 10000, 0, 0,
    FALSE, FALSE,
    '2027-03-31',
    '2022-04-25', '2027-03-25',
    '2022-04-25', '2027-03-25',
    1, 1, 1, FALSE, 0
) ON CONFLICT (kykm_id) DO NOTHING;
```

### 異常データ

| データ名 | 値 | 期待エラー |
|---|---|---|
| TEST-ERR-001 | `shri_cnt = 0` かつ `b_klsryo = 50000`（矛盾データ） | 集計対象外スキップ または InvalidOperationException |
| TEST-ERR-002 | `k_seigou_f = FALSE` の契約（集計対象外） | ソースデータ取得時にフィルタされレコード数 0 |
| TEST-ERR-003 | 不正な接続文字列（TC-009 用） | NpgsqlException スロー、ロールバック確認 |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| TEST-BND-001 | `start_dt = KeijoFrom` と同日（期間境界・開始日一致） | 集計対象に含まれること |
| TEST-BND-002 | `end_dt = KeijoTo` と同日（期間境界・終了日一致） | 集計対象に含まれること |
| TEST-BND-003 | `end_dt < KeijoFrom`（期間前に終了した契約） | 集計対象外となること |
| TEST-BND-004 | `start_dt > KeijoTo`（期間後に開始する契約） | 集計対象外となること |
| TEST-BND-005 | 配賦率合計が 0.9999（端数あり） | 最終配賦行が残額方式で正しく計算されること |
| TEST-BND-006 | `mkaisu = 1`（支払回数 1 回） | 最終回のみ計上（済回数 = 総回数 後は対象外） |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `LeaseM4BS.TestWinForms/Form_f_KEIJO_JOKEN.vb`（既存スタブ → 要接続実装） | TC-001, TC-003〜TC-007 のフォーム起点 | — |
| `LeaseM4BS.TestWinForms/Form_f_flx_TOUGETSU.vb`（既存スタブ → 要結果表示実装） | TC-003〜TC-007 の結果確認 | — |
| `sql/003_seed_test_monthly_journal.sql`（新規作成） | TC-004〜TC-006, TC-008〜TC-010 のテストデータ | — |
| `LeaseM4BS.DataAccess/MonthlyJournalEngine.vb`（新規 — 実装対象） | TC-003〜TC-010 | 8 |
| `LeaseM4BS.DataAccess/KlsryoTypes.vb` または `MonthlyJournalTypes.vb`（既存改修） | TC-001 | 1 |

**総テストケース数**: 10

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

1. **クラス設計**: `KlsryoCalculationEngine.vb` と同一パターン
   - `Private _crud As New CrudHelper()` で DB アクセス
   - `Public Function Execute(...) As Integer` をエントリポイントとする
   - 内部処理は `Private Sub ProcessKykm / ProcessHaif / ProcessHenf` に分割

2. **SQL 構築**: `Form_f_flx_KEIJO.vb` の `BuildSql()` パターン
   - `System.Text.StringBuilder` で動的 SQL を構築
   - `NpgsqlParameter` でパラメータ化する

3. **トランザクション管理**: `Form_JournalEntry.vb` の `btnSave_Click` パターン
   - `_crud.BeginTransaction()` → 処理 → `_crud.Commit()` / `_crud.Rollback()`
   - `Try/Catch` で必ず `Rollback()` を呼ぶ

4. **TestWinForms フォーム結果表示**: `Form_f_flx_KEIJO.vb` の `SearchData` / `ApplyGridStyle` パターン
   - `DataGridView` に `_crud.GetDataTable(sql)` の結果を表示
   - `HideColumns` / `FormatColumn` で書式設定

### モック/スタブの使い方

- モックは使用しない（実 DB 必須）
- `Form_f_KEIJO_JOKEN.vb` の実行ボタンから `MonthlyJournalEngine.Execute()` を直接呼び出し、戻り値をメッセージボックスまたはラベルに表示して確認する

### テストヘルパー/ユーティリティの活用

- `CrudHelper.GetDataTable()` — ワークテーブルの確認 SELECT に使用
- `CrudHelper.ExecuteScalar(Of Integer)()` — 件数確認に使用
- `FormHelper.vb` の `SwapIf`, `GetMonthStart`, `GetMonthEnd`, `HandleEnterKeyNavigation` — フォーム側で再利用

---

## 8. ビルド検証手順

```bash
# ソリューション全体のビルド（CI と同等）
cd C:\kobayashi_LeaseM4BS
MSBuild LeaseM4BS\LeaseM4BS.slnx /p:Configuration=Debug /p:Platform="Any CPU" /t:Rebuild

# 期待結果
# Build succeeded.
# 0 Error(s)
# 0 Warning(s) が理想（既存 Warning は許容範囲内）
```

### 確認事項

| チェック項目 | 確認方法 | 期待結果 |
|---|---|---|
| `KeijoJoken` クラスのコンパイル | MSBuild | エラーなし |
| `MonthlyJournalEngine` クラスのコンパイル | MSBuild | エラーなし |
| `TestWinForms` からの参照解決 | MSBuild | エラーなし |
| `tw_s_chuki_keijo` DDL 追加後の DB 接続 | TestWinForms 起動 | 接続エラーなし |

---

## 9. テスト実施順序

1. `TC-001` (ビルド検証) → MSBuild 成功を確認
2. `sql/003_seed_test_monthly_journal.sql` を開発 DB に投入
3. `TC-002` (SQL 生成) → TestWinForms デバッグ実行で DataGridView を確認
4. `TC-003` (Execute 正常終了) → 戻り値と `tw_s_chuki_keijo` レコード数を確認
5. `TC-004` (物件単位) → ワークテーブルの `lsryo_toki` を確認
6. `TC-005` (配賦単位) → 配賦2行の金額配分を確認
7. `TC-006` (付随費用) → `rec_kbn = 3` レコードを確認
8. `TC-007` (月別ループ) → 3ヶ月分レコード件数を確認
9. `TC-008` (書込・読込) → 二重実行で件数が変わらないことを確認
10. `TC-009` (エラーハンドリング) → 異常データでのロールバックを確認
11. `TC-010` (Access版一致性) → Issue #14 の E2E フロー確認と合わせて実施
