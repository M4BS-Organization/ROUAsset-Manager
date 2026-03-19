# テスト計画書: hen-ido-gson-forms

> Issue #32: 変更・異動・減損処理画面群の移行

---

## 1. テスト戦略

- **テストフレームワーク**: スタンドアローンVB.NETコンソールアプリ（既存の `test_*_blackbox.vb` パターンに準拠）
- **テストレベル**:
  - 単体テスト: DataAccess層の計算ロジック（CashScheduleBuilder、GsonScheduleBuilder等）
  - 結合テスト: DB接続あり（PostgreSQL。接続不可時はSKIP）
- **カバレッジ目標**: 各US受け入れ基準を1対1でカバー
- **モック戦略**:
  - DataTable/DataRow をインメモリで構築してDBなしでロジックをテスト（`test_bugfix_blackbox.vb` パターン）
  - DB接続が必要なテストは例外を `HandleDbException` でSKIPとして扱う

---

## 2. テスト環境

### セットアップ手順

```bat
rem コンパイル
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe" ^
  /target:exe ^
  /out:test_hen_ido_gson_blackbox.exe ^
  /reference:LeaseM4BS/LeaseM4BS.DataAccess/bin/Debug/LeaseM4BS.DataAccess.dll ^
  /reference:LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/bin/Debug/LeaseM4BS.TestWinForms.exe ^
  /reference:Npgsql.dll ^
  /reference:System.Data.dll ^
  /reference:System.Windows.Forms.dll ^
  test_hen_ido_gson_blackbox.vb

rem 実行
test_hen_ido_gson_blackbox.exe
```

### テストデータ準備

- インメモリ DataTable に手動でテストデータを構築（DBなし）
- DB接続テストは `Host=localhost;Port=5432;Database=lease_m4bs` を使用
- 接続できない場合は自動SKIP

### 外部依存のモック

- `CrudHelper` のDB呼び出しが必要なテストはインメモリ DataTable で代替する
- `GsonScheduleBuilder.BuildFromRows(dt.Rows)` / `CashScheduleBuilder.BuildHengakuSchedule` など、DataRowsを直接受け取るAPIはモックなしでテスト可能

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | GsonScheduleBuilder.BuildFromRows — 基本動作 | 高 | US-007, FR-002 |
| T-002 | GsonScheduleBuilder.BuildFromRows — GSON_TMG=0/1 分岐 | 高 | US-007, US-008 |
| T-003 | GsonScheduleBuilder.BuildFromRows — DBNull安全性 | 高 | FR-002 |
| T-004 | 消費税自動計算（KZEI/KLSRYO_ZKOMI） | 高 | FR-004, US-002, US-004 |
| T-005 | 合計行自動更新（SUM列/GOKEI列） | 高 | FR-005, US-001, US-002, US-004 |
| T-006 | 異動処理バリデーション — IDO_DT空欄チェック | 高 | US-005 |
| T-007 | 異動種別選択（管理部署/費用負担部署）による更新カラム切り替え | 高 | FR-003, US-005 |
| T-008 | f_IDO_SUB チェックボックス選択カウント | 中 | US-006 |
| T-009 | CashScheduleBuilder.BuildHengakuSchedule — D_HENL参照 | 高 | FR-001, US-002 |
| T-010 | KeijoWorkTableManager — tw_d_gson_keijo整合性 | 中 | FR-002, NFR-002 |
| T-011 | f_flx_D_GSON 検索SQL — 部分一致/大小文字不問 | 中 | US-007 |
| T-012 | SecurityChecker.ApplyDataUpdateLimit — 権限制御 | 高 | FR-006, US-007 |
| T-013 | Excelインポート 27列構造 — FileHelper連携 | 低 | FR-007, US-010 |
| T-014 | D_HENL 削除時0件警告 | 中 | NFR-002, US-002 |
| T-015 | 異動処理トランザクション — 部分更新防止 | 高 | NFR-002, FR-003 |
| T-016 | LINE_ID採番 — MAX(LINE_ID)+1 | 中 | US-002 仮定 |
| T-017 | GSON_TMG 不正値（0/1以外）例外 | 高 | 技術的制約 7-4 |
| T-018 | f_HEN_SCH 呼出元反映 — ダイアログ結果連携 | 高 | US-001 |

---

## 4. テストケース

### TC-001: GsonScheduleBuilder — 空DataTable入力

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: US-007, FR-002
- **種別**: 正常系
- **前提条件**: d_gson相当のカラムを持つ空のDataTableを用意
- **入力**: 行数=0のDataTable（列: gson_dt, gson_tmg, gson_ryo, gson_rkei）
- **期待結果**: 空の`List(Of GsonScheduleEntry)`が返る（Count=0、例外なし）

---

### TC-002: GsonScheduleBuilder — GSON_TMG=0（月度末）

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: US-007, US-008
- **種別**: 正常系
- **前提条件**: gson_tmg=0のDataRowを1件用意
- **入力**:
  - gson_dt = 2024/09/30
  - gson_tmg = 0（月度末）
  - gson_ryo = 200000
  - gson_rkei = 200000
- **期待結果**:
  - Count=1
  - GsonRyoS=0.0（月度初=0）
  - GsonRyoE=200000.0（月度末=gsonRyo）
  - GsonRkeiS=0.0（gsonRkei - gsonRyo = 0）
  - GsonRkeiE=200000.0

---

### TC-003: GsonScheduleBuilder — GSON_TMG=1（月度初）

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: US-007, US-008
- **種別**: 正常系
- **前提条件**: gson_tmg=1のDataRowを1件用意
- **入力**:
  - gson_dt = 2024/09/30
  - gson_tmg = 1（月度初）
  - gson_ryo = 150000
  - gson_rkei = 350000（前回分含む累計）
- **期待結果**:
  - GsonRyoS=150000.0（月度初=gsonRyo）
  - GsonRyoE=0.0（月度末=0）
  - GsonRkeiS=350000.0（月度初時点で既に累計に含まれる）
  - GsonRkeiE=350000.0

---

### TC-004: GsonScheduleBuilder — gson_dt=DBNull行スキップ

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: FR-002
- **種別**: 異常系
- **前提条件**: gson_dt=DBNull.Valueの行を含むDataTable
- **入力**: gson_dt=DBNull, gson_tmg=0, gson_ryo=100000, gson_rkei=100000
- **期待結果**: InvalidCastException が発生しない、Count=0（DBNull行はスキップ）

---

### TC-005: GsonScheduleBuilder — DBNull行と有効行の混在

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: FR-002
- **種別**: 異常系
- **前提条件**: DBNull行1件 + 有効行1件
- **入力**:
  - 行1: gson_dt=DBNull, gson_ryo=50000
  - 行2: gson_dt=2024/09/30, gson_ryo=200000
- **期待結果**: Count=1（有効行のみ処理）、GsonRyoE=200000.0

---

### TC-006: GsonScheduleBuilder — GSON_TMG不正値（0/1以外）

- **対象**: `GsonScheduleBuilder.BuildFromRows`（`GsonScheduleBuilder.vb:62`）
- **関連要件**: 技術的制約 7-4
- **種別**: 異常系
- **前提条件**: gson_tmg=2（不正値）のDataRowを用意
- **入力**: gson_dt=2024/09/30, gson_tmg=2, gson_ryo=100000, gson_rkei=100000
- **期待結果**: Exception がスローされる（ArgumentException または同等）

---

### TC-007: 消費税自動計算 — 標準税率10%

- **対象**: f_HENL / f_HENF の消費税計算ロジック（FR-004）
- **関連要件**: FR-004, US-002, US-004
- **種別**: 正常系
- **前提条件**: KLSRYO変更イベントのロジックを純粋関数として抽出してテスト
- **入力**: KLSRYO=100000, ZRITU=10
- **期待結果**:
  - KZEI = Int(100000 * 10 / 100) = 10000（切捨て）
  - KLSRYO_ZKOMI = 100000 + 10000 = 110000

---

### TC-008: 消費税自動計算 — 端数切捨て（8%）

- **対象**: 消費税計算ロジック
- **関連要件**: FR-004
- **種別**: 境界値
- **前提条件**: ZRITU=8
- **入力**: KLSRYO=12345, ZRITU=8
- **期待結果**:
  - KZEI = Int(12345 * 8 / 100) = Int(987.6) = 987（切捨て）
  - KLSRYO_ZKOMI = 12345 + 987 = 13332

---

### TC-009: 消費税自動計算 — 税率0%

- **対象**: 消費税計算ロジック
- **関連要件**: FR-004
- **種別**: 境界値
- **入力**: KLSRYO=100000, ZRITU=0
- **期待結果**: KZEI=0, KLSRYO_ZKOMI=100000

---

### TC-010: 合計行自動更新 — 3行の合計

- **対象**: f_HEN_SCH / f_HENL / f_HENF の合計計算ロジック（FR-005）
- **関連要件**: FR-005, US-001, US-002, US-004
- **種別**: 正常系
- **入力**:
  | 行 | KLSRYO | KZEI | KLSRYO_ZKOMI |
  |---|---|---|---|
  | 1 | 100000 | 10000 | 110000 |
  | 2 | 200000 | 20000 | 220000 |
  | 3 | 150000 | 15000 | 165000 |
- **期待結果**:
  - KLSRYO_SUM = 450000
  - KZEI_SUM = 45000
  - KLSRYO_ZKOMI_SUM = 495000

---

### TC-011: 合計行自動更新 — 行削除後の再計算

- **対象**: 合計計算ロジック（FR-005）
- **関連要件**: FR-005
- **種別**: 正常系
- **前提条件**: TC-010の3行から1行削除
- **入力**: 行2を削除
- **期待結果**:
  - KLSRYO_SUM = 250000（行1+行3）
  - KZEI_SUM = 25000
  - KLSRYO_ZKOMI_SUM = 275000

---

### TC-012: 合計行自動更新 — 行なし（0件）

- **対象**: 合計計算ロジック（FR-005）
- **関連要件**: FR-005
- **種別**: 境界値
- **入力**: データ行なし
- **期待結果**: 全SUM列=0

---

### TC-013: f_IDO バリデーション — IDO_DT空欄チェック

- **対象**: `Form_f_IDO` の実行バリデーションロジック
- **関連要件**: US-005
- **種別**: 異常系
- **前提条件**: 異動フォームに物件が選択されているが IDO_DT が空欄
- **入力**: IDO_DT = 空文字 / Nothing
- **期待結果**: 実行できない（バリデーションエラーメッセージを返す、または False を返す）

---

### TC-014: f_IDO バリデーション — IDO_DT正常

- **対象**: `Form_f_IDO` のバリデーションロジック
- **関連要件**: US-005
- **種別**: 正常系
- **入力**: IDO_DT = 2024/04/01（有効な日付）
- **期待結果**: バリデーション通過（True を返す）

---

### TC-015: 異動種別 — 管理部署カラム更新対象

- **対象**: f_IDO の異動種別ラジオボタンによるカラム切り替えロジック（FR-003）
- **関連要件**: FR-003, US-005
- **種別**: 正常系
- **入力**: 異動種別 = 管理部署（オプション416）
- **期待結果**: 更新対象カラムが BCAT1_ID〜BCAT5_ID（HKBCAT1_ID〜HKBCAT5_ID ではない）

---

### TC-016: 異動種別 — 費用負担部署カラム更新対象

- **対象**: f_IDO の異動種別ラジオボタンによるカラム切り替えロジック
- **関連要件**: FR-003, US-005
- **種別**: 正常系
- **入力**: 異動種別 = 費用負担部署（オプション418）
- **期待結果**: 更新対象カラムが HKBCAT1_ID〜HKBCAT5_ID

---

### TC-017: f_IDO_SUB — chk_IDO_F選択カウント更新

- **対象**: `Form_f_IDO_SUB` のチェックボックス変更イベント
- **関連要件**: US-006
- **種別**: 正常系
- **前提条件**: 5件の物件が表示されている
- **入力**: 3件チェック
- **期待結果**: COUNT = 3, DCOUNT = 5（全件数）

---

### TC-018: f_IDO_SUB — すべて選択ボタン

- **対象**: `Form_f_IDO` の「すべて選択（&A）」ボタン
- **関連要件**: US-005
- **種別**: 正常系
- **前提条件**: 一部チェックされていない状態
- **入力**: 「すべて選択」ボタン押下
- **期待結果**: 全 chk_IDO_F = True, COUNT = DCOUNT

---

### TC-019: f_IDO_SUB — すべて選択しないボタン

- **対象**: `Form_f_IDO` の「すべて選択しない（&E）」ボタン
- **関連要件**: US-005
- **種別**: 正常系
- **入力**: 「すべて選択しない」ボタン押下
- **期待結果**: 全 chk_IDO_F = False, COUNT = 0

---

### TC-020: D_HENL LINE_ID採番 — MAX+1

- **対象**: f_HENL の行追加時LINE_ID採番ロジック
- **関連要件**: FR-001, 仮定 8-1
- **種別**: 正常系
- **前提条件**: 既存行 LINE_ID = {1, 2, 5}
- **入力**: 新規行追加
- **期待結果**: 新規行の LINE_ID = 6（MAX(LINE_ID)+1）

---

### TC-021: D_HENL LINE_ID採番 — 0件時

- **対象**: f_HENL の行追加時LINE_ID採番ロジック
- **関連要件**: FR-001
- **種別**: 境界値
- **前提条件**: 対象KYKM_IDのD_HENLレコードが0件
- **入力**: 新規行追加
- **期待結果**: 新規行の LINE_ID = 1

---

### TC-022: D_HENL 全削除時の警告

- **対象**: f_HENL の行削除ロジック
- **関連要件**: NFR-002, US-002
- **種別**: 異常系
- **前提条件**: D_HENLレコードが1件（最後の1件）
- **入力**: 行削除実行
- **期待結果**: 警告メッセージが表示される（「変額スケジュールが空になる」等）

---

### TC-023: f_flx_D_GSON 検索SQL — 部分一致（ILIKE）

- **対象**: `Form_f_flx_D_GSON` の検索SQL生成ロジック
- **関連要件**: US-007
- **種別**: 正常系
- **入力**: 検索テキスト = "ABC"
- **期待結果**: SQLに `ILIKE '%ABC%'` または同等の大文字小文字不問・部分一致条件が含まれる

---

### TC-024: f_flx_D_GSON 検索SQL — 空文字（全件）

- **対象**: `Form_f_flx_D_GSON` の検索SQL生成ロジック
- **関連要件**: US-007
- **種別**: 境界値
- **入力**: 検索テキスト = ""（空文字）
- **期待結果**: SQLにWHERE条件なし（または全件検索）、エラーなし

---

### TC-025: SecurityChecker — DataUpdateLimit 権限あり

- **対象**: `SecurityChecker.ApplyDataUpdateLimit`
- **関連要件**: FR-006, US-007
- **種別**: 正常系
- **前提条件**: データ更新権限があるユーザー
- **入力**: ApplyDataUpdateLimit(Me)
- **期待結果**: 変更ボタン・削除ボタンが有効（Enabled=True）

---

### TC-026: SecurityChecker — DataUpdateLimit 権限なし

- **対象**: `SecurityChecker.ApplyDataUpdateLimit`
- **関連要件**: FR-006, US-007
- **種別**: 正常系
- **前提条件**: データ更新権限がないユーザー
- **入力**: ApplyDataUpdateLimit(Me)
- **期待結果**: 変更ボタン・削除ボタンが無効（Enabled=False）

---

### TC-027: f_HEN_SCH — ダイアログ呼出元反映

- **対象**: `Form_f_HEN_SCH` の「呼出元に反映（&R）」ボタン
- **関連要件**: US-001
- **種別**: 正常系
- **前提条件**: f_HENL から f_HEN_SCH を ShowDialog で開く
- **入力**: SHRI_DT / KLSRYO / KZEI / ZRITU / SSHRI_KN を編集後、「呼出元に反映」押下
- **期待結果**: DialogResult = OK、呼び出し元に変更内容が渡される

---

### TC-028: f_HEN_SCH — キャンセル時の変更破棄

- **対象**: `Form_f_HEN_SCH` のキャンセルボタン
- **関連要件**: US-001
- **種別**: 正常系
- **入力**: 値を変更後にキャンセルボタン押下
- **期待結果**: DialogResult = Cancel、元の値が保持される（変更なし）

---

### TC-029: f_REF_D_HENL — ReadOnlyモード確認

- **対象**: `Form_f_REF_D_HENL`（参照フォーム）
- **関連要件**: US-003
- **種別**: 正常系
- **入力**: フォームロード
- **期待結果**: 全入力フィールドが ReadOnly=True、「戻る（&C）」ボタンのみ有効

---

### TC-030: 異動処理トランザクション — 部分更新防止

- **対象**: f_IDO の「実行（&R）」ボタン処理（NFR-002）
- **関連要件**: NFR-002, FR-003
- **種別**: 異常系
- **前提条件**: DB接続ありのテスト環境。3件選択中に2件目更新後にDBエラーを発生させる
- **入力**: 更新途中にDB接続を切断（シミュレーション）
- **期待結果**: トランザクションがロールバックされ、1件目の変更も元に戻る

---

### TC-031: CashScheduleBuilder — D_HENL参照整合性

- **対象**: `CashScheduleBuilder.BuildHengakuSchedule`
- **関連要件**: FR-001, US-002
- **種別**: 結合テスト（DB接続必要）
- **前提条件**: DB接続あり、特定KYKM_IDのD_HENLデータが存在する
- **入力**: CashScheduleBuilder.BuildHengakuSchedule(crud, kykmId, ckaiykEsdtH)
- **期待結果**: 例外なし、D_HENLの行数とスケジュールエントリ数が整合する

---

### TC-032: KeijoWorkTable — tw_d_gson_keijo整合性

- **対象**: `KeijoWorkTableManager.ClearGsonKeijo` / `InsertGsonKeijo`
- **関連要件**: FR-002, NFR-002
- **種別**: 結合テスト（DB接続必要）
- **前提条件**: DB接続あり
- **入力**: GsonKeijo行を3件Insert後にClearを呼ぶ
- **期待結果**: Clear後のtw_d_gson_keijo件数=0

---

### TC-033: f_IMPORT_GSON_FROM_EXCEL — 27列構造チェック

- **対象**: `Form_f_IMPORT_GSON_FROM_EXCEL` のExcelインポートロジック
- **関連要件**: FR-007, US-010
- **種別**: 異常系（境界値）
- **前提条件**: 列数が26の不完全なExcelファイル
- **入力**: 26列のExcelファイルを選択
- **期待結果**: エラーメッセージを表示（27列必要のメッセージ）

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| 標準KYKM_ID | 1001 | 各テストの基準物件ID |
| 標準GSON_DT | 2024/09/30 | 月末日 |
| 標準GSON_RYO | 200000 | 減損損失額 |
| 標準GSON_RKEI | 200000 | 減損累計額（初回）|
| 標準KLSRYO | 100000 | リース料（税抜）|
| 標準ZRITU | 10 | 消費税率10% |
| 標準KZEI | 10000 | 消費税額 |
| 標準IDO_DT | 2024/04/01 | 異動日 |

### 異常データ

| データ名 | 値 | 期待エラー |
|---|---|---|
| gson_dt=NULL | DBNull.Value | スキップ（例外なし）|
| gson_tmg=不正値 | 2 | ArgumentException |
| IDO_DT=空欄 | "" | バリデーションエラー |
| Excel=26列 | 26列のデータ | 列数不足エラー |
| D_HENL最終行削除 | 1件→0件 | 警告メッセージ |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| ZRITU=0 | 0% | 税率ゼロでの除算回避 |
| KLSRYO=0 | 0円 | 支払額ゼロ |
| SHRI_CNT=1 | 1回払い | 最小支払回数 |
| LINE_ID=0件 | MAX=None | 初回採番 |
| GSON_RKEI=GSON_RYO | 累計=当期額 | 初回減損（GsonRkeiS=0）|
| GSON_TMG=0 月初日 | 2024/04/01 | 月初日付での月度末処理 |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_hen_ido_gson_blackbox.vb` | GsonScheduleBuilder / 消費税計算 / 合計行 / IDOバリデーション / LINE_ID採番 | TC-001〜TC-026 (26件) |
| `c:\kobayashi_LeaseM4BS\test_hensch_dialog_blackbox.vb` | f_HEN_SCH ダイアログ動作 / f_REF_D_HENL ReadOnly確認 | TC-027〜TC-029 (3件) |
| `c:\kobayashi_LeaseM4BS\test_gson_integration.vb` | CashScheduleBuilder / KeijoWorkTable / トランザクション（DB接続）| TC-030〜TC-032 (3件) |

**合計テストケース: 33件（ファイル3本）**

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

既存の `test_*_blackbox.vb` ファイルで確立された以下のパターンを踏襲する。

**ファイル構造パターン** (`test_schedule_blackbox.vb` / `test_bugfix_blackbox.vb` 準拠):
```vb
Option Strict On
Option Explicit On

' ブラックボックステスト: <機能名>
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll ...
' 実行: test_xxx.exe

Imports System
Imports LeaseM4BS.DataAccess

Module TestXxxBlackBox
    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        ' Partごとにテストを呼び出す
        PrintSummary()
    End Sub
    ' ... テストメソッド、アサーションヘルパー
End Module
```

**モック/スタブの使い方**:
- DBが必要なテストは `DataTable` をインメモリで構築してDLL公開APIを直接呼ぶ（`test_bugfix_blackbox.vb:228-250` 参照）
- DB接続テストは `Try/Catch` でDB例外をSKIPとして扱う（`test_schedule_blackbox.vb:811-858` 参照）

**アサーションパターン**:
- `AssertEqual(label, expected, actual)` — Double / Integer / Boolean のオーバーロード
- `AssertContains(label, actual, expected)` — SQL文字列検証（`test_chuki_idolst_joken_blackbox.vb` 参照）
- `Pass(label)` / `Fail(label, expected, actual)` / `Skip(label)` — カウンタ更新と出力

**Pure Function テストパターン** (`test_chuki_idolst_joken_blackbox.vb` 参照):
- フォームのロジックを `XxxPure` / `GetXxxConditions` 等の静的メソッドとして公開し、WinFormsイベントと分離してテスト
- f_IDO の異動種別判定ロジック、f_HENL の消費税計算ロジックも同様に純粋関数として実装・テストすること

**テストヘルパー/ユーティリティの活用**:
- `HandleDbException(label, ex)` パターンでDB例外を一元処理（`test_bugfix_blackbox.vb:459-470` 参照）
- `FindParam(prms, name)` パターンでNpgsqlParameterを検索（`test_chuki_idolst_joken_blackbox.vb:592-597` 参照）
