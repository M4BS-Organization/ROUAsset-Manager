# テスト計画書: blackbox-type-safety

## 1. テスト戦略

- **テストフレームワーク**: VB.NET コンソールアプリ（スタンドアロン .exe）
  - 既存パターン（`test_schedule_blackbox.vb`, `test_fixed_length.vb`）に準拠
  - `Module` + `Sub Test_XXX()` / `Function Test_XXX() As Boolean` 形式
  - MSTest / NUnit / xUnit は使用しない
- **テストレベル**: 単体テスト（DB接続不要）+ 統合テスト（DB依存・SKIP扱い）
- **テスト手法**: ゴールデンマスターテスト（Access版VBAコードを手計算して期待値を導出）
- **カバレッジ目標**: 既存テスト（`test_schedule_blackbox.vb`）不足分を網羅
  - 型安全性問題（DBNull, Object型配列, CInt/CDbl直接変換）を重点的に検証
- **モック戦略**: DB接続不要テストは `Nothing` を CrudHelper 引数として渡す。DB依存部分は `Try/Catch` で `SKIP` 扱い
- **合否判定基準**:
  - 数値比較: `Math.Abs(expected - actual) < 0.001`（既存実装に準拠）
  - 整数値・バイト列・Null判定: 等値比較
  - FAIL が1件でもあれば `Environment.ExitCode = 1`

---

## 2. テスト環境

### 必要なセットアップ
- `LeaseM4BS.DataAccess.dll` がビルド済みであること
- `System.Data.dll` が参照可能であること
- コンパイル（DB接続不要テスト）:
  ```
  vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_type_safety_blackbox.vb
  ```
- 実行:
  ```
  test_type_safety_blackbox.exe
  ```

### テストデータの準備方法
- 全テストデータはソースコード内にハードコードする（DBに依存しない）
- 期待値はAccess版VBAコード（`pc_注記.txt`等）を手計算で導出した値を使用

### 外部依存のモック方法
- DB依存クラス（`KeijoCalculationEngine`, `KlsryoCalculationEngine`）は `Try/Catch` で `SKIP`
- `ChukiCalcEngine.Calculate` / `AmortizationScheduleBuilder.Build` は `crud=Nothing` でDB接続不要で動作

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | `ScheduleHelper.GInt` 追加ケース（境界値・特殊値） | 高 | US-001 |
| T-002 | `ScheduleHelper.GKasan` 追加ケース（単一値・全値あり） | 高 | US-001 |
| T-003 | `ScheduleHelper.GetGetuShoNichi` | 中 | FR-001 |
| T-004 | `AmortizationScheduleBuilder` 定額法・減損あり | 高 | US-002 |
| T-005 | `AmortizationScheduleBuilder` 定額法・中途解約フラグ | 高 | US-002 |
| T-006 | `AmortizationScheduleBuilder` 定率法・減損あり | 高 | US-003 |
| T-007 | `RepaymentScheduleBuilder` リースバック繰延損益 | 中 | US-005 |
| T-008 | `ChukiCalcEngine` 中途解約フラグあり | 高 | US-006 |
| T-009 | `ChukiCalcEngine` KessanBi≠31（非月末決算バグ再現） | 中 | US-006 |
| T-010 | `ChukiCalcEngine` MatsubiShuryoKichuMasshoF=True | 中 | US-006 |
| T-011 | `GsonScheduleBuilder` Build（空入力・通常入力） | 中 | FR-001 |
| T-012 | `CashScheduleBuilder.GetMonthEndDate` 境界値 | 中 | FR-001 |
| T-013 | `FixedLengthFileWriter` DBNull数値フィールド | 高 | US-007, US-008 |
| T-014 | `FixedLengthFileWriter` 全角文字バイト境界切り捨て | 高 | US-007 |
| T-015 | 型安全性: CDbl/CInt のDBNull入力（例外発生確認） | 高 | US-008 |
| T-016 | 型安全性: Object型配列への CDbl 累積加算パターン | 高 | US-008 |
| T-017 | 型安全性: GetDbl/GetInt ヘルパー vs 直接変換の挙動比較 | 高 | US-008 |
| T-018 | CLng Banker's Rounding 網羅（0.5/1.5/2.5/−0.5） | 高 | US-002, US-003 |
| T-019 | CInt Banker's Rounding 網羅（0.5/1.5/−0.5） | 高 | US-004 |

---

## 4. テストケース

### TC-001: GInt 境界値・特殊値
- **対象**: `ScheduleHelper.GInt(Double) As Double`
- **関連要件**: US-001, FR-001
- **種別**: 境界値
- **前提条件**: なし（DB不要）
- **入力/期待結果**:

| ラベル | 入力 | 期待値 | 根拠 |
|---|---|---|---|
| GInt: 0.0 → 0 | 0.0 | 0.0 | Math.Floor(0) |
| GInt: 1.0 → 1 | 1.0 | 1.0 | Math.Floor(1) |
| GInt: 0.9999999999999 → 1 | 0.9999999999999 | 1.0 | G15変換で1.0になり Floor(1)=1 |
| GInt: 1.0000000000001 → 1 | 1.0000000000001 | 1.0 | G15変換で1.0になり Floor(1)=1 |
| GInt: Double.MaxValue (不変) | 1.7976931348623E+308 | 1.7976931348623E+308 | G15で有効桁維持 |
| GInt: −0.0000000001 → −1 | −0.0000000001 | −1.0 | Math.Floor(−0.0000000001) = −1 |

---

### TC-002: GKasan 追加ケース
- **対象**: `ScheduleHelper.GKasan(Boolean, ParamArray Double?()) As Double?`
- **関連要件**: US-001
- **種別**: 正常系・境界値
- **前提条件**: なし（DB不要）
- **入力/期待結果**:

| ラベル | skipNull | 入力値 | 期待値 |
|---|---|---|---|
| 単一値 | True | 500.0 | 500.0 |
| 単一Null skipTrue | True | Nothing | Nothing |
| skipFalse 単一Null | False | Nothing | 0.0 |
| 全値あり | True | 100.0, 200.0, 300.0 | 600.0 |
| 負値混在 | True | 100.0, −50.0 | 50.0 |
| ゼロ混在 | True | 0.0, Nothing, 100.0 | 100.0 |

---

### TC-003: GetGetuShoNichi
- **対象**: `ScheduleHelper.GetGetuShoNichi(Date) As Date`
- **関連要件**: FR-001
- **種別**: 正常系・境界値
- **前提条件**: なし（DB不要）
- **入力/期待結果**:

| ラベル | 入力 | 期待値 |
|---|---|---|
| 月中の日付 | 2024/04/15 | 2024/04/01 |
| 月初 | 2024/04/01 | 2024/04/01 |
| 月末 | 2024/04/30 | 2024/04/01 |
| 2月末（閏年） | 2024/02/29 | 2024/02/01 |

---

### TC-004: 定額法償却スケジュール・減損あり
- **対象**: `AmortizationScheduleBuilder.Build(ShokyakuHo.Teigaku, ...)`
- **関連要件**: US-002
- **種別**: 正常系
- **前提条件**: DB不要
- **入力**:
  - 取得価額: 1,200,000、期間: 24ヶ月、残価保証: 0
  - 開始日: 2024/04/01、終了日: 2026/03/31
  - 減損スケジュール: M6（2024/09）に減損200,000発生
- **期待結果**:
  - M1〜M5: Skyak = GInt((1200000 − 0) / 24) = 50,000
  - M6: 減損反映後に ZanS が更新される
  - 減損発生月以降で CalcShokyakuRitu が再計算される（定額法なので月額再計算）
  - 最終月 ZanE = 0

---

### TC-005: 定額法・中途解約フラグ
- **対象**: `AmortizationScheduleBuilder.Build(ShokyakuHo.Teigaku, ...)`
- **関連要件**: US-002
- **種別**: 正常系
- **前提条件**: DB不要
- **入力**:
  - 取得価額: 600,000、期間: 12ヶ月、残価保証: 0
  - 支払スケジュールの M6（2024/09）以降に `CkaiykF = True`
- **期待結果**:
  - M1〜M5: `CkaiykF = False`
  - M6〜M12: `CkaiykF = True`（中途解約行）
  - M6以降でも ZanE は継続計算される

---

### TC-006: 定率法・減損あり
- **対象**: `AmortizationScheduleBuilder.Build(ShokyakuHo.Teiritu, ...)`
- **関連要件**: US-003
- **種別**: 正常系
- **前提条件**: DB不要
- **入力**:
  - 取得価額: 1,000,000、期間: 60ヶ月、残価保証: 0
  - 減損スケジュール: M12（2021/03）に減損300,000発生
- **期待結果**:
  - M1〜M11: skyakRitu = CalcShokyakuRitu(60) = 0.03758
  - M12: 減損反映後に新残高 = 旧ZanS − 300,000
  - M13以降: skyakRitu = CalcShokyakuRitu(60 − 12) = CalcShokyakuRitu(48)
  - 最終月 ZanE = 0

---

### TC-007: 返済スケジュール・リースバック繰延損益
- **対象**: `RepaymentScheduleBuilder.BuildYakujoShiharai(...)`
- **関連要件**: US-005
- **種別**: 正常系
- **前提条件**: DB不要
- **入力**:
  - 取得価額: 1,000,000、総額: 1,200,000、維持管理: 0、残価保証: 0
  - リースバック損益（BLbSoneki）: 120,000（月次按分対象）
  - 後払、12ヶ月、利率3.5%
- **期待結果**:
  - LbSonekiE の全月合計 = 120,000（支払額比例で按分）
  - 各月 `LbSonekiE = GInt(120000 * (CashE / 1200000))`

---

### TC-008: 注記計算エンジン・中途解約フラグあり
- **対象**: `ChukiCalcEngine.Calculate(...)`
- **関連要件**: US-006
- **種別**: 正常系
- **前提条件**: DB不要（crud=Nothing）
- **入力**:
  - 移転外Fリース、利子抜法、定額法、後払
  - 取得価額: 1,000,000、総額: 1,200,000、利率3.5%
  - BCkaiykF = True（中途解約あり）
  - 解約月: M6（2024/09）以降の ShiharaiSchEntry に CkaiykF=True
- **期待結果**:
  - `LgnpnKaiyakGen` > 0（解約時元本消去が設定される）
  - `RisokuMibKaiyakGen` > 0（解約時未払利息消去が設定される）
  - `LgnpnZan` + `LgnpnKaiyakGen` = 解約月直前のリース負債元本残高

---

### TC-009: 注記計算エンジン・KessanBi≠31（Access版コピペバグ再現）
- **対象**: `ChukiCalcEngine.Calculate(...)`
- **関連要件**: US-006
- **種別**: 異常系（バグ再現テスト）
- **前提条件**: DB不要（crud=Nothing）
- **入力**:
  - KessanBi = 28（月末以外の決算日）
  - 移転外Fリース、利子抜法、定額法、後払
  - 取得価額: 1,000,000、12ヶ月、利率3.5%
- **期待結果**:
  - `y1KimatDt` が `params.KimatDt.AddYears(5)` で上書きされる（Access版バグ再現）
  - `y4KishuDt` / `y5KishuDt` が `#12/30/1899#`（VBA未初期化日付）になる
  - 処理は例外なく完了する（バグの再現であって例外ではない）
  - `LgnpnZan` / `LrsokZan` 等の数値は計算される（Access版と同じ結果になる）

---

### TC-010: 注記計算エンジン・MatsubiShuryoKichuMasshoF=True
- **対象**: `ChukiCalcEngine.Calculate(...)`
- **関連要件**: US-006
- **種別**: 正常系
- **前提条件**: DB不要（crud=Nothing）
- **入力**:
  - 移転外Fリース、利子抜法、定額法、後払
  - MatsubiShuryoKichuMasshoF = True（前期末残高に基づく判定）
  - 取得価額: 1,000,000、24ヶ月（うち半分は前期）、利率3.5%
  - 期首日(KishuDt): 2025/04/01、期末日(KimatDt): 2026/03/31
  - リース開始: 2024/04/01、リース終了: 2026/03/31
- **期待結果**:
  - `LgnpnZzan` > 0（前期末残高が設定される）
  - `SyutokZzan` = 1,000,000（前期末取得価額残高）
  - `GruikeiZzan` > 0（前期末減価償却累計）

---

### TC-011: GsonScheduleBuilder Build・空入力
- **対象**: `GsonScheduleBuilder.Build(...)` / `GsonScheduleBuilder.BuildFromRows(...)`
- **関連要件**: FR-001
- **種別**: 正常系・境界値
- **前提条件**: DB不要
- **入力/期待結果**:

| ラベル | 入力 | 期待値 |
|---|---|---|
| 空リスト入力 | 空の DataTable | `Nothing` または空リスト |
| 通常1行 | 減損日: 2024/09/30, 減損額: 200000 | エントリ1件、GsonKin=200000 |
| 複数行 | 2行の減損データ | エントリ2件 |

---

### TC-012: CashScheduleBuilder.GetMonthEndDate 境界値
- **対象**: `CashScheduleBuilder.GetMonthEndDate(Integer, Integer) As Date`
- **関連要件**: FR-001
- **種別**: 境界値
- **前提条件**: DB不要
- **入力/期待結果**:

| ラベル | 入力(年,月) | 期待値 |
|---|---|---|
| 通常月 | 2024, 4 | 2024/04/30 |
| 小の月 | 2024, 6 | 2024/06/30 |
| 12月 | 2024, 12 | 2024/12/31 |
| 閏年2月 | 2024, 2 | 2024/02/29 |
| 非閏年2月 | 2023, 2 | 2023/02/28 |
| 年越し | 2024, 13 | 2025/01/31（13月→翌1月） |

---

### TC-013: FixedLengthFileWriter DBNull数値フィールド
- **対象**: `FixedLengthFileWriter.BuildRecord(DataRow, List(Of FixedLengthFieldDef))`
- **関連要件**: US-007, US-008
- **種別**: 異常系・境界値
- **前提条件**: DB不要
- **入力**:
  - `FixedLengthFieldDef`（数値フィールド、フォーマット `"000000000000000000.000"`、22バイト）
  - 値: `DBNull.Value`
- **期待結果**:
  - 出力: `"000000000000000000.000"`（22バイト）
  - 例外が発生しない

---

### TC-014: 全角文字バイト境界切り捨て（半角文字残留なし）
- **対象**: `FixedLengthFileWriter.PadRightByte(String, Integer) As String`
- **関連要件**: US-007
- **種別**: 境界値
- **前提条件**: DB不要
- **入力/期待結果**:

| ラベル | 入力 | byteWidth | 期待値 | バイト長 |
|---|---|---|---|---|
| 全角3文字→奇数切り捨て | "あいう" (6バイト) | 5 | "あい " (4+1=5バイト) | 5 |
| 全角+半角混在 | "あBC" (4バイト) | 3 | "あ " (2+1=3バイト) | 3 |
| ぴったり | "あい" (4バイト) | 4 | "あい" | 4 |
| 全角1文字+不足 | "あ" (2バイト) | 5 | "あ   " (2+3=5バイト) | 5 |

---

### TC-015: 型安全性 CDbl/CInt のDBNull入力
- **対象**: VB.NETの型変換関数 `CDbl()`, `CInt()` の挙動確認
- **関連要件**: US-008
- **種別**: 異常系
- **前提条件**: DB不要（インラインコードで検証）
- **入力/期待結果**:

| ラベル | コード | 期待結果 |
|---|---|---|
| CDbl(DBNull.Value) → 例外 | `CDbl(DBNull.Value)` | `InvalidCastException` が発生する |
| CInt(DBNull.Value) → 例外 | `CInt(DBNull.Value)` | `InvalidCastException` が発生する |
| CDbl(Nothing) → 0 | `CDbl(Nothing)` | `0.0`（VB.NET: Nothing は数値型で0） |
| CInt(Nothing) → 0 | `CInt(Nothing)` | `0` |
| CDbl("123.45") → 123.45 | `CDbl("123.45")` | `123.45` |
| Convert.ToDouble(DBNull.Value) → 例外 | `Convert.ToDouble(DBNull.Value)` | `InvalidCastException` が発生する |

- **検証観点**: 既存コードの「危険パターン」（`CDbl(row("col"))` でDBNull非考慮）が実際に例外を投げることを確認し、`GetDbl`/`GetInt` ヘルパー使用の必要性を明示する

---

### TC-016: Object型配列へのCDbl累積加算パターン
- **対象**: `KlsryoCalculationEngine` の `CDbl(lsryoTokig(j)) + entry.Lsryo` パターン相当
- **関連要件**: US-008
- **種別**: 異常系
- **前提条件**: DB不要（インラインコードで検証）
- **入力/期待結果**:

| ラベル | シナリオ | 入力 | 期待結果 |
|---|---|---|---|
| 正常累積 | Object配列の初期値0.0に加算 | `arr(0) = 0.0`, `CDbl(arr(0)) + 100.0` | `100.0`（正常） |
| Nothing初期化 | Object配列の初期値なしに加算 | `arr(0)` 未設定（DBNull相当）, `CDbl(arr(0))` | `InvalidCastException` が発生する |
| 安全パターン | If IsDBNull チェックあり | `If IsDBNull(arr(0)) Then 0.0 Else CDbl(arr(0))` + 100.0 | `100.0`（例外なし） |

---

### TC-017: GetDbl/GetInt ヘルパー vs 直接変換の挙動比較
- **対象**: `KeijoCalculationEngine.GetDbl(DataRow, String)` / `GetInt(DataRow, String)` 相当ロジック
- **関連要件**: US-008
- **種別**: 正常系・異常系
- **前提条件**: DB不要（DataRowをコード内で構築）
- **入力/期待結果**:

| ラベル | 変換方法 | 入力値 | 期待結果 |
|---|---|---|---|
| GetDbl: 通常値 | `Convert.ToDouble(row(col))` | `123.45` | `123.45` |
| GetDbl: DBNull | `If IsDBNull(row(col)) Then 0.0 Else Convert.ToDouble(row(col))` | `DBNull.Value` | `0.0` |
| 直接CDbl: DBNull | `CDbl(row(col))` | `DBNull.Value` | `InvalidCastException` |
| GetInt: 通常値 | `Convert.ToInt32(row(col))` | `42` | `42` |
| GetInt: DBNull | `If IsDBNull(row(col)) Then 0 Else Convert.ToInt32(row(col))` | `DBNull.Value` | `0` |

---

### TC-018: CLng Banker's Rounding 網羅
- **対象**: VB.NETの `CLng()` 関数（Banker's Rounding）
- **関連要件**: US-002, US-003
- **種別**: 境界値
- **前提条件**: なし（DB不要）
- **入力/期待結果**:

| ラベル | 入力 | 期待値 | 備考 |
|---|---|---|---|
| CLng(0.5) → 0 | 0.5 | 0 | 偶数丸め（0は偶数） |
| CLng(1.5) → 2 | 1.5 | 2 | 偶数丸め（2は偶数） |
| CLng(2.5) → 2 | 2.5 | 2 | 偶数丸め（2は偶数） |
| CLng(3.5) → 4 | 3.5 | 4 | 偶数丸め（4は偶数） |
| CLng(−0.5) → 0 | −0.5 | 0 | 偶数丸め |
| CLng(−1.5) → −2 | −1.5 | −2 | 偶数丸め |
| CLng(37584.4 * 0.1) | 3758.44 | 3758 | 定率法配賦計算の実ケース |

---

### TC-019: CInt Banker's Rounding 網羅
- **対象**: VB.NETの `CInt()` 関数（Banker's Rounding）
- **関連要件**: US-004
- **種別**: 境界値
- **前提条件**: なし（DB不要）
- **入力/期待結果**:

| ラベル | 入力 | 期待値 | 備考 |
|---|---|---|---|
| CInt(0.5) → 0 | 0.5 | 0 | 偶数丸め |
| CInt(1.5) → 2 | 1.5 | 2 | 偶数丸め |
| CInt(2.5) → 2 | 2.5 | 2 | 偶数丸め |
| CInt(3.5) → 4 | 3.5 | 4 | 偶数丸め |
| CInt(−0.5) → 0 | −0.5 | 0 | 偶数丸め |
| CInt("10") → 10 | "10"（文字列） | 10 | 文字列からの変換 |
| CInt("10.5") → 10 | "10.5"（文字列） | 10 | 文字列の小数丸め（銀行家） |

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| 基本取得価額 | 1,000,000 | 償却・返済スケジュール標準ケース |
| 基本期間 | 12ヶ月 | 標準リース期間 |
| 基本利率 | 3.5% (0.035) | 返済スケジュール |
| 定率法期間 | 60ヶ月 | CalcShokyakuRitu(60) = 0.03758 |
| 標準支払額 | 100,000/月 | ShiharaiSchEntry.Cash |
| 標準開始日 | 2024/04/01 | 年度開始 |
| 標準終了日 | 2025/03/31 | 12ヶ月後年度末 |
| 決算日(月末) | 31 | KessanBi 標準値 |

### 異常データ

| データ名 | 値 | 期待エラー/挙動 |
|---|---|---|
| DBNull数値 | `DBNull.Value` | CDbl直接変換で InvalidCastException |
| Nothing（Object型配列） | 初期化なし | CDbl累積加算で InvalidCastException |
| 文字列数値 | "123.45" | CDbl成功、Convert.ToDouble成功 |
| 0除算ケース | 総額リース料 = 0 | GInt内で / 0 → NaN or Exception（要確認） |
| 負値 | GInt(−3.7) | −4（切捨て） |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| 浮動小数点誤差 | 3744.9999999999996 | GInt: G15変換で3745になる |
| 直下整数 | 1.0000000000001 | GInt: G15変換で1.0になり1を返す |
| Banker's境界 | 0.5, 1.5, 2.5, 3.5 | CLng/CInt 偶数丸め確認 |
| 全角バイト境界 | 奇数バイト幅での全角文字切り捨て | PadRightByte: 半角残留なし確認 |
| 閏年2月 | 2024/02/29 | IsMonthEnd=True, GetMonthEndDate |
| 非閏年2月 | 2023/02/28 | IsMonthEnd=True |
| 非月末 | 2024/02/28 (閏年) | IsMonthEnd=False |
| KessanBi非月末 | 28 | ChukiCalcEngine: コピペバグ分岐 |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_type_safety_blackbox.vb` | 型安全性・未カバー領域 | 約60 |

### `test_type_safety_blackbox.vb` の内部構成

| Part | 対象 | テストケース数（概算） |
|---|---|---|
| Part 1 | `ScheduleHelper` 追加（GInt境界値, GKasan追加, GetGetuShoNichi） | 10 |
| Part 2 | `AmortizationScheduleBuilder` 追加（減損あり, 中途解約フラグ） | 8 |
| Part 3 | `RepaymentScheduleBuilder` 追加（リースバック損益） | 5 |
| Part 4 | `ChukiCalcEngine` 追加（中途解約, KessanBi≠31, MatsubiShuryoKichuMasshoF） | 12 |
| Part 5 | `GsonScheduleBuilder` Build（空入力, 通常入力） | 6 |
| Part 6 | `CashScheduleBuilder.GetMonthEndDate` 境界値 | 6 |
| Part 7 | 型安全性（CDbl/CInt DBNull, Object配列, GetDbl比較） | 10 |
| Part 8 | Banker's Rounding（CLng, CInt 網羅） | 12 |
| Part 9 | 統合テスト (DB接続) — SKIP扱い | 1 |
| **合計** | | **約70** |

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

1. **ファイル構造**: `Module TestXxx` + `Sub Main()` + `Sub Test_XXX()` / `Function Test_XXX() As Boolean`
2. **カウンター**: `Dim passCount As Integer = 0` / `Dim failCount As Integer = 0` / `Dim skipCount As Integer = 0`
3. **出力**: `Console.OutputEncoding = System.Text.Encoding.UTF8`
4. **Part構成**: `Console.WriteLine("--- Part N: 説明 ---")` で区切る
5. **例外ハンドリング**: `Try / Catch ex As Exception` → `Fail(label, "success", $"Exception: {ex.GetType().Name}: {ex.Message}")`
6. **DB接続不可時のSKIP**: `rootMsg.Contains("refused")` 等で判定して `Skip()` を呼ぶ

### モック/スタブの使い方

- DB不要クラス（`ScheduleHelper`, `AmortizationScheduleBuilder`, `RepaymentScheduleBuilder`, `ChukiCalcEngine`）は `crud=Nothing` で直接呼び出す
- DB依存クラス（`KeijoCalculationEngine`, `KlsryoCalculationEngine`, `MonthlyJournalEngine`）は `Try/Catch` で保護し接続失敗時は `SKIP`

### アサーションヘルパー

`test_schedule_blackbox.vb` の以下をそのまま流用:
- `AssertEqual(label, Double, Double)` — 許容誤差 0.001
- `AssertEqual(label, Integer, Integer)` — 等値比較
- `AssertEqual(label, Boolean, Boolean)` — 等値比較
- `AssertEqual(label, Double?, Double?)` — Null考慮の数値比較
- `Pass(label)`, `Fail(label, expected, actual)`, `Skip(label)`

### コンパイルコマンド

新テストファイル先頭コメントに以下を記載する:
```
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_type_safety_blackbox.vb
' 実行: test_type_safety_blackbox.exe
```

---

## 8. 実行方法

### 前提
1. `LeaseM4BS.DataAccess.dll` が `c:\kobayashi_LeaseM4BS\` に存在すること（ビルド後コピー）
2. .NET Framework 4.7.2 の `vbc.exe` が PATH に含まれていること

### コンパイル・実行手順

```bash
# 作業ディレクトリに移動
cd c:\kobayashi_LeaseM4BS

# コンパイル
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_type_safety_blackbox.vb

# 実行
test_type_safety_blackbox.exe
```

### 既存テストと合わせた一括実行

```bash
# 既存テストのコンパイル・実行
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_schedule_blackbox.vb && test_schedule_blackbox.exe
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_fixed_length.vb    && test_fixed_length.exe

# 新テストのコンパイル・実行
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_type_safety_blackbox.vb && test_type_safety_blackbox.exe
```

---

## 9. 合否判定基準

| 判定 | 基準 |
|---|---|
| PASS | 数値: `Math.Abs(expected − actual) < 0.001` / 整数・文字列・Boolean: 完全一致 |
| FAIL | 上記基準を満たさない場合。`failCount` に加算し終了コード 1 |
| SKIP | DB接続不可・スキーマ未完成・アセンブリ参照不一致の場合。`skipCount` に加算 |
| 全体PASS | `failCount = 0`（SKIPは許容） |

### 型安全性テスト（TC-015〜017）の特殊判定

「例外が発生すること」を期待するテストは以下のパターンで検証する:
```vb
Sub Test_CDbl_DBNull_ThrowsException()
    Dim label As String = "CDbl(DBNull) → InvalidCastException"
    Try
        Dim row As New DataTable()
        row.Columns.Add("col", GetType(Object))
        Dim r = row.NewRow()
        r("col") = DBNull.Value
        row.Rows.Add(r)
        Dim v As Double = CDbl(row.Rows(0)("col"))
        Fail(label, "InvalidCastException", $"No exception, got {v}")
    Catch ex As InvalidCastException
        Pass(label)
    Catch ex As Exception
        Fail(label, "InvalidCastException", ex.GetType().Name)
    End Try
End Sub
```
