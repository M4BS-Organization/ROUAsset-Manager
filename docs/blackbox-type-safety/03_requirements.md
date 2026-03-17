# 要件定義書: blackbox-type-safety

## 1. 機能概要

Access版 LeaseM4BS の各計算モジュール・関数に対して、VB.NET版が同じ入力を与えたときに同じ出力を返すことをブラックボックステストで検証する。型安全性の観点から `Convert.ToDouble` 等の型変換が問題になる箇所を特定し、致命的バグが0件であることを確認する。

既にブラックボックステストの骨格 (`test_schedule_blackbox.vb`, `test_fixed_length.vb`) が存在しており、本要件はその拡充・整備・カバレッジ強化を対象とする。

---

## 2. ユーザーストーリー

### US-001: 数値計算ヘルパーのAccess版互換検証

- **As a** 開発者
- **I want** `ScheduleHelper.GInt` / `GKasan` 等のヘルパー関数がAccess版の挙動と完全に一致することを確認したい
- **So that** 浮動小数点誤差に起因する計算結果のズレをゼロにできる

#### 受け入れ基準
- [ ] `GInt(3744.9999999999996)` が `3745` を返す（Access版 `Int(CStr(...))` と一致）
- [ ] `GInt(-3.7)` が `-4` を返す（切捨て方向の確認）
- [ ] `GKasan(True, Null, Null)` が `Nothing` を返す
- [ ] `GKasan(False, Null, Null)` が `0` を返す
- [ ] `GKasan(True, 100, Nothing, 200)` が `300` を返す
- [ ] `IsMonthEnd` が閏年2月29日・非閏年2月28日を正しく判定する

---

### US-002: 償却スケジュール（定額法）のAccess版互換検証

- **As a** 開発者
- **I want** `AmortizationScheduleBuilder.Build(ShokyakuHo.Teigaku, ...)` の出力がAccess版 `gMake償却_SCH` と完全に一致することを確認したい
- **So that** リース資産の減価償却計算に誤りがないことを保証できる

#### 受け入れ基準
- [ ] 取得価額100万円・12ヶ月・残価保証0のケースで月額償却費が `83333` 円（GInt切捨て後）と一致する
- [ ] 最終月の償却費が端数調整後 `83337` 円（残高=0になる）と一致する
- [ ] 残価保証10万円のケースで月額償却費が `75000` 円・期末残高が `100000` 円と一致する
- [ ] 減損スケジュールを与えた場合、減損発生月以降で償却費が再計算される
- [ ] 中途解約フラグが立った行は `CkaiykF = True` になる
- [ ] 件数（lkikan）と行数が一致する

---

### US-003: 償却スケジュール（定率法）のAccess版互換検証

- **As a** 開発者
- **I want** `AmortizationScheduleBuilder.Build(ShokyakuHo.Teiritu, ...)` の出力がAccess版と完全に一致することを確認したい
- **So that** 定率法を選択した物件の注記計算が正確であることを保証できる

#### 受け入れ基準
- [ ] `CalcShokyakuRitu(60)` が `0.03758` を返す（M4互換モード: `Int(ritu*1000000)` → `Long*0.1` → `Long*0.00001`）
- [ ] 取得価額100万円・60ヶ月のケースで M1 の `Zan109S` が `1111111` 円と一致する
- [ ] 定率法の M1 償却費が `GInt(Zan109S * skyakRitu)` と一致する
- [ ] 最終月の残高が残価保証額（0または指定値）と一致する
- [ ] 減損発生時に `CalcShokyakuRitu(lkikan - i)` で償却率が再計算される

---

### US-004: 返済スケジュール（後払/先払）のAccess版互換検証

- **As a** 開発者
- **I want** `RepaymentScheduleBuilder.BuildYakujoShiharai` の出力がAccess版 `gMake返済_SCH_約定支払用` と完全に一致することを確認したい
- **So that** リース負債の元本・利息・残高の月次推移計算に誤りがないことを保証できる

#### 受け入れ基準
- [ ] 後払（`RsokTmg.Atobarai`）のケースで各月 `CashE > 0`、`CashS = 0` となる
- [ ] 先払（`RsokTmg.Sakibarai`）のケースで各月 `CashS > 0`、`CashE = 0` となる
- [ ] 利息合計（`RisokuShriS + RisokuShriE` の全月累計）が `slsryo - ijiknr + zanryo - syutok` と一致する
- [ ] 元本合計（`GanponS + GanponE` の全月累計）が `syutok` と一致する
- [ ] 最終月の `GanponZanE` が `0` になる
- [ ] 最終月の `RisokuZanE` が `0` になる
- [ ] 元本超過調整（2010/06/07コメントのロジック）が正しく機能し、元本が債務残高を超えない

---

### US-005: 維持管理費用・残価保証を含む返済スケジュールの検証

- **As a** 開発者
- **I want** 維持管理費用・残価保証清算がある複合ケースでAccess版と一致することを確認したい
- **So that** 実運用データに近いケースの計算精度を保証できる

#### 受け入れ基準
- [ ] 維持管理費用が `GInt(ijiknr * (cash / slsryo))` で按分され、全月合計が `ijiknr` と一致する（端数調整含む）
- [ ] `NetLsryo = Cash - Ijiknr` が各月正しく計算される
- [ ] 残価保証清算額（`ZanryoSeisanE`）がリース期間末月に正しく配置される
- [ ] `ZanryoMsZanE`（残価保証未清算残高）が月次に正しく引き落とされる
- [ ] リースバック繰延損益（`LbSonekiS`, `LbSonekiE`）が支払額比例で按分される

---

### US-006: 注記計算エンジン（ChukiCalcEngine）の検証

- **As a** 開発者
- **I want** `ChukiCalcEngine.Calculate` の出力がAccess版 `m注記計算_main` と完全に一致することを確認したい
- **So that** 財務諸表注記用の取得価額・減価償却累計額・リース料元本残高等の集計値が正確であることを保証できる

#### 受け入れ基準
- [ ] 移転外ファイナンスリース（`LeaseKbn.Itengai`）で `SyutokZou`・`GruikeiZou`・`BokaZan` が正しく計算される
- [ ] オペレーティングリース（`LeaseKbn.Ope`）で `GsonZzan`・`GsonZan`・`LsryoToki`・`LgnpnToki`・`LrsokToki`・`GsonTkToki`・`IjiknrToki` が `Nothing`（Access版Null）になる
- [ ] 移転ファイナンスリース（`LeaseKbn.Iten`）では資産関連（`SyutokZou`・`GruikeiZou`）が `0` になる
- [ ] `LgnpnZan`・`LrsokZan` の1年内/1年超/2〜5年分類が正しく計算される
- [ ] 中途解約フラグ有りの場合、解約抹消（`LgnpnKaiyakGen`・`RisokuMibKaiyakGen`）が正しく設定される
- [ ] `MatsubiShuryoKichuMasshoF = True/False` で前期末残高・当期減少の判定日が正しく切り替わる
- [ ] `KessanBi` が31以外（非月末決算）の場合のAccess版バグ（コピペバグ）が再現される

---

### US-007: KITOKU固定長出力フォーマットの検証

- **As a** 開発者
- **I want** `FixedLengthFileWriter.BuildRecord` の出力がAccess版 `gStrSizeAdjust` 連結結果と完全一致することを確認したい
- **So that** 外部連携ファイル（CMSW2WRK・APGDHWRK・APGDDWRK・APGDSWRK）の出力フォーマットが正確であることを保証できる

#### 受け入れ基準
- [ ] CMSW2WRK の1レコードが571バイト（Shift-JIS）になる
- [ ] APGDHWRK・APGDDWRK・APGDSWRK の各レコードバイト長がフィールド定義の合計と一致する
- [ ] 数値フォーマット `Format(value, "000000000000000000.000")` が正しく再現される
- [ ] 文字列が右側をスペースでパディングされ、Shift-JISバイト単位で切り捨てられる
- [ ] `Null`（DBNull）の数値フィールドが `0` として扱われ正規化形式で出力される
- [ ] 全角文字列のバイト境界での切り捨てが正しく処理される（半角文字残留なし）
- [ ] ファイル出力E2Eで行数・各行バイト長が正しい

---

### US-008: 型安全性問題の特定と修正

- **As a** 開発者
- **I want** `Convert.ToDouble` 等の型変換が `DBNull`・`Nothing`・`Object` 型の値に対して安全に機能することを確認したい
- **So that** 実行時例外（`InvalidCastException`・`NullReferenceException`）が発生しないことを保証できる

#### 受け入れ基準
- [ ] `GsonScheduleBuilder.SafeConv(Of Double)(DBNull.Value, 0)` が `0.0` を返す
- [ ] `KlsryoResult.LsryoZzan`（`Object` 型 Null可）を数値演算に使う箇所でガード処理が実施されている
- [ ] `KeijoWorkRow.ShhoId`（`Object` 型）が `DBNull.Value` の場合に例外なく処理される
- [ ] `CashScheduleEntry.GsonDt`（`Object` 型）の `Nothing` / `DBNull.Value` 判定が正しく機能する
- [ ] Access版の `Nz(value, 0)` 相当のNull安全変換が全ての `Object` 型フィールドに適用されている

---

## 3. 機能要件

### FR-001: テスト対象クラスの網羅
- 説明: 以下の全クラスに対してブラックボックステストが存在すること
  - `ScheduleHelper`（GInt, GKasan, GetGetuYYYYMM, GetGetuShoNichi, IsMonthEnd）
  - `AmortizationScheduleBuilder`（定額法・定率法・CalcShokyakuRitu）
  - `RepaymentScheduleBuilder`（BuildYakujoShiharai + SetKisobubu 内部ロジック）
  - `ChukiCalcEngine`（Calculate + CalcShisanRelated + CalcHensaiRelated）
  - `GsonScheduleBuilder`（Build + BuildFromRows）
  - `CashScheduleBuilder`（GetMonthEndDate）
  - `FixedLengthFileWriter`（BuildRecord, PadRightByte, WriteFile）
  - `KitokuFixedLengthFormats`（全4フォーマット）
- 優先度: 必須

### FR-002: ゴールデンデータによる期待値設定
- 説明: 各テストケースの期待値はAccess版コードを手計算または実機確認により求めた値を使用する。「妥当に見える値」ではなく「Access版が実際に返す値」を設定する
- 優先度: 必須

### FR-003: テスト結果のPASS/FAIL/SKIPレポート
- 説明: 既存の `passCount`/`failCount`/`skipCount` 形式を継続使用し、全テスト完了後にサマリーを出力する。`FAIL` が1件でもあれば終了コード1を返す
- 優先度: 必須

### FR-004: DB接続不要のユニットテスト分離
- 説明: `ScheduleHelper`・`AmortizationScheduleBuilder`・`RepaymentScheduleBuilder`・`ChukiCalcEngine`（crud=Nothingパターン）の各テストはDB接続なしで実行可能であること。DB依存テスト（MonthlyJournalEngine等）は `SKIP` で適切に扱う
- 優先度: 必須

### FR-005: 許容誤差の明示
- 説明: 数値比較は `Math.Abs(expected - actual) < 0.001`（既存実装）を標準とする。完全一致が求められるケース（バイト列・整数値）は等値比較を使用する
- 優先度: 必須

---

## 4. 非機能要件

### NFR-001: パフォーマンス
- DB接続不要テスト群（Part 1〜5）の実行時間は5秒以内とする
- DB接続テスト（Part 6）は接続タイムアウトを含めて30秒以内とする

### NFR-002: 保守性
- テストコードは既存の `test_schedule_blackbox.vb` / `test_fixed_length.vb` のスタイルに準拠する（`Module` + `Sub Test_XXX()` 形式）
- コンパイルコマンドをファイル先頭コメントに記載する

### NFR-003: 再現性
- テストは冪等であること（同じ入力で何度実行しても同じ結果になること）
- テスト間の状態共有なし（各テストは独立して実行可能）

### NFR-004: 文字エンコーディング
- コンソール出力は `Console.OutputEncoding = Encoding.UTF8` を設定する
- 固定長出力テストの文字列比較はShift-JISバイト単位で行う

---

## 5. 前提条件・制約

- テストは `LeaseM4BS.DataAccess.dll` に依存する。DLLのビルドが完了していることが前提
- コンパイルは `vbc /r:LeaseM4BS.DataAccess.dll` で行う（.NET Framework 4.7.2）
- Access版 (`C:\access_LeaseM4BS`) はバイナリ形式のため、ソースコード上の挙動はコードレビューと手計算で確認する
- `ig決算日 = 31`（月末締め）を標準前提とする。月末以外の決算日パターンはAccess版コピペバグを含むため再現テストのみ行う
- DB依存テスト（`MonthlyJournalEngine` 統合テスト）はローカルPostgreSQLが起動していない環境ではSKIPとする

---

## 6. スコープ外

- Access版 MDB ファイルを直接読み込んでの自動ゴールデンデータ生成（Access版はバイナリ形式のため不可）
- UI・WinFormsフォームのテスト
- `KeijoCalculationEngine.Execute`・`KlsryoCalculationEngine.Execute` のE2Eテスト（DBデータ依存のためSKIP扱い）
- パフォーマンステスト（大量データ処理速度）
- 並列実行・スレッドセーフ性の検証

---

## 7. 用語定義

| 用語 | 定義 |
|---|---|
| GInt | Access版 `Int(CStr(value))` の VB.NET 移植。浮動小数点誤差を `G15` フォーマット経由で除去し `Math.Floor` で切捨て |
| ゴールデンデータ | Access版が実際に出力する値。テストの期待値として使用 |
| 月度初 / 月度末 | 1ヶ月のスケジュール行における「月初の支払」と「月末の支払」を区別するサフィックス（S = 初, E = 末）|
| 中途解約フラグ | `CkaiykF = True` の行。リース期間終了前に解約された月以降の行に設定される |
| 利子抜法 | リース料に含まれる利息を分離して計上する方法（`RcalcKind.RisokuBunri`）|
| 利子込法 | 利息を分離せずリース料そのものを費用計上する方法（`RcalcKind.Risikomi`）|
| M4互換モード | `gCalc償却率` において `Int(ritu*1000000)` → `Long*0.1`（銀行丸め）→ `Long*0.00001` の手順で定率を計算するモード |
| KITOKU | 財務会計システム「奉行」の外部連携固定長フォーマット（CMSW2WRK・APGDHWRK・APGDDWRK・APGDSWRK）|
| DBNull / Nothing | Access版 Null の VB.NET 対応。数値型フィールドは `Nothing`（`Double?`）、Object型フィールドは `DBNull.Value` を使用 |

---

## 8. 仮定事項

1. **Access版の挙動はVBAコードから手計算で確認可能**: Access版 `C:\access_LeaseM4BS` はバイナリ（.mdb）形式のため、Access版の計算結果を直接実行して検証することは現時点でスコープ外とした。期待値はVBAコードの読解と手計算から導出する。実装前に必要であれば Access を起動して実機確認を行うこと。

2. **既存テストファイルとの共存**: 既存の `test_schedule_blackbox.vb` / `test_fixed_length.vb` は拡充対象であり置き換えない。新規テストはこれらのファイルに追加するか、新ファイル `test_type_safety_blackbox.vb` に分離する（担当者判断）。

3. **型安全性の問題は主に Object 型フィールドに集中**: `KlsryoResult`・`KeijoWorkRow`・`CashScheduleEntry` の `Object` 型プロパティが `DBNull.Value` を保持した状態で算術演算が行われた場合に `InvalidCastException` が発生するリスクがある。テスト対象の特定は実コードの `Convert.ToDouble(value)` 呼び出し箇所のGrep調査で行う。

4. **数値許容誤差は 0.001 を標準**: 既存実装に合わせる。GInt後の整数比較は 0.001 以内で完全一致と判断できる。
