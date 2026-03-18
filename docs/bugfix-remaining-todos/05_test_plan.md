# テスト計画書: bugfix-remaining-todos (Issue #17)

## 1. テスト戦略

- **テストフレームワーク**: vbc.exe (VB.NET コマンドラインコンパイラ) + Console アプリケーション方式
- **テストレベル**: ブラックボックステスト（単体 + 結合）
- **カバレッジ目標**: Issue #17 で修正する 4 機能すべての正常系・異常系をカバー
- **モック戦略**:
  - DB不要テスト: インメモリ DataTable を使って PostgreSQL 接続なしで実行
  - DB必要テスト: DB接続不可時は SKIP（既存パターンに合わせた HandleDbException / Skip ヘルパー活用）
- **回帰テスト方針**: 既存 51 テストを全件 PASS 維持。修正後に 5 つのブラックボックステスト EXE をすべて実行し、FAIL = 0 を確認する。

---

## 2. テスト環境

### 必要なセットアップ手順

```bash
# 1. DataAccess DLL をビルド
cd c:\kobayashi_LeaseM4BS\LeaseM4BS\LeaseM4BS.DataAccess
msbuild LeaseM4BS.DataAccess.vbproj /p:Configuration=Debug

# 2. テスト EXE をコンパイル（新規テストファイル）
cd c:\kobayashi_LeaseM4BS
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll /r:System.dll /r:Npgsql.dll ^
    test_bugfix_blackbox.vb /out:test_bugfix_blackbox.exe

# 3. 回帰テスト EXE を実行（既存）
test_e2e_blackbox.exe
test_keijo_joken_blackbox.exe
test_chuki_idolst_joken_blackbox.exe
test_schedule_blackbox.exe
test_type_safety_blackbox.exe
```

### テストデータの準備方法

- DB不要テストはインメモリ DataTable を使用するため、データ準備不要
- DB接続テストは PostgreSQL が `localhost:5432` で稼働していること

### 外部依存のモック方法

- CrudHelper は直接テストせず、DataAccess DLL の公開 API (エンジンクラス) のみを対象とする
- Form 層 (Form_ContractEntry 等) のテストは実行環境上 WinForms を起動せずに実施するため、
  ロジック部分を直接呼び出すかシミュレーションで代替する

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | Utils.NzInt の戻り値型 (Object → Integer) | 高 | US-004, FR-005 |
| T-002 | Form_ContractEntry の Update 処理 (isNewMode = False) | 高 | US-002, FR-004 |
| T-003 | E2E フロー全体の回帰テスト | 高 | US-001, US-005, FR-001 |
| T-004 | 未実装メニューの状態維持確認 | 中 | US-003, FR-006 |
| T-005 | 型安全性・DBNull ガードの修正確認 | 高 | US-004, NFR-005 |

---

## 4. テストケース

### TC-001: NzInt の戻り値型が Integer であることの確認

- **対象**: `Utils.NzInt`
- **関連要件**: US-004, FR-005
- **種別**: 正常系
- **前提条件**: Utils.NzInt が `Function NzInt(...) As Integer` で定義されている
- **入力**: `NzInt(DBNull.Value)`, `NzInt(42)`, `NzInt("99")`, `NzInt(Nothing)`
- **期待結果**:
  - `NzInt(DBNull.Value)` → `0` (Integer)
  - `NzInt(42)` → `42` (Integer)
  - `NzInt("99")` → `99` (Integer)
  - `NzInt(Nothing)` → `0` (Integer)
- **補足**: 戻り値を Integer 変数に直接代入してコンパイルエラーが出ないことで型安全性を確認

---

### TC-002: NzInt DBNull 境界値テスト

- **対象**: `Utils.NzInt`
- **関連要件**: US-004, FR-005
- **種別**: 境界値
- **前提条件**: TC-001 が PASS していること
- **入力**: `DBNull.Value`, `Nothing`, `Integer.MaxValue`, `Integer.MinValue`, `"0"`, `""`
- **期待結果**:
  - `DBNull.Value` → 0
  - `Nothing` → 0
  - `Integer.MaxValue` → `2147483647`
  - `Integer.MinValue` → `-2147483648`
  - `"0"` → 0
  - `""` → 0（または FormatException なし）

---

### TC-003: NzDec の戻り値型確認（変更不要の確認）

- **対象**: `Utils.NzDec`
- **関連要件**: US-004
- **種別**: 正常系
- **前提条件**: NzDec が Decimal を返すと仮定されている
- **入力**: `NzDec(DBNull.Value)`, `NzDec(1234.56D)`
- **期待結果**:
  - `NzDec(DBNull.Value)` → `0D` (Decimal)
  - `NzDec(1234.56D)` → `1234.56D`

---

### TC-004: NzDate の戻り値型確認

- **対象**: `Utils.NzDate`
- **関連要件**: US-004
- **種別**: 正常系
- **入力**: `NzDate(DBNull.Value)`, `NzDate(New Date(2024, 4, 1))`
- **期待結果**:
  - `NzDate(DBNull.Value)` → `Date.MinValue` または `Nothing` (実装依存)
  - `NzDate(New Date(2024, 4, 1))` → `New Date(2024, 4, 1)`

---

### TC-005: Form_ContractEntry — 修正モード（isNewMode = False）の UPDATE 確認

- **対象**: `Form_ContractEntry.SaveButton_Click` または `SaveData()`
- **関連要件**: US-002, FR-004
- **種別**: 正常系
- **前提条件**:
  - `isNewMode = False` に設定されたインスタンス
  - DB に d_kykh テーブルのレコードが 1 件以上存在
- **入力**: 既存契約 ID を指定し、保存ボタン押下相当の処理を呼び出す
- **期待結果**:
  - `CrudHelper.Update` が呼び出される (INSERT ではない)
  - 「修正モードですが、Update 処理が未実装…」警告ダイアログが表示されない
  - INSERT 件数が増えない (DB 観点)
- **DB確認クエリ**:
  ```sql
  SELECT COUNT(*) FROM d_kykh WHERE kykh_kykh_id = <テスト対象ID>;
  -- 修正前後でカウントが変化しないこと
  ```

---

### TC-006: Form_ContractEntry — 新規モード（isNewMode = True）の INSERT が壊れていないことの確認

- **対象**: `Form_ContractEntry.SaveData()`
- **関連要件**: US-002, FR-004
- **種別**: 回帰テスト / 正常系
- **前提条件**: `isNewMode = True`
- **入力**: 新規契約データ（必須項目を入力済み）で保存
- **期待結果**:
  - INSERT が実行される（UPDATE ではない）
  - d_kykh テーブルに新規レコードが 1 件増える

---

### TC-007: Form_ContractEntry — UPDATE 失敗時のエラーハンドリング

- **対象**: `Form_ContractEntry.SaveData()` — UPDATE 失敗ケース
- **関連要件**: US-002, FR-004, NFR-003
- **種別**: 異常系
- **前提条件**: `isNewMode = False`、DB 接続を意図的に遮断、またはモック UPDATE が失敗
- **入力**: 保存ボタン押下
- **期待結果**:
  - 未処理例外ダイアログが表示されない
  - 「エラーの種類とメッセージ」を含む MessageBox が表示される
  - データ破損が発生しない

---

### TC-008: 回帰テスト — test_e2e_blackbox.exe 全 PASS

- **対象**: `test_e2e_blackbox.exe`
- **関連要件**: US-005, FR-001
- **種別**: 回帰テスト
- **前提条件**: DataAccess.dll がビルドされ最新であること
- **入力**: EXE を引数なしで実行
- **期待結果**: 出力の最終行が `全テスト PASS` または `PASS=N, FAIL=0, SKIP=M`
- **コマンド**:
  ```bash
  test_e2e_blackbox.exe
  echo Exit code: %ERRORLEVEL%
  # ERRORLEVEL = 0 であること
  ```

---

### TC-009: 回帰テスト — test_keijo_joken_blackbox.exe 全 PASS

- **対象**: `test_keijo_joken_blackbox.exe`
- **関連要件**: US-005
- **種別**: 回帰テスト
- **前提条件**: TC-008 と同様
- **入力**: EXE を引数なしで実行
- **期待結果**: `FAIL=0` かつ Exit code = 0

---

### TC-010: 回帰テスト — test_chuki_idolst_joken_blackbox.exe 全 PASS (51 テスト)

- **対象**: `test_chuki_idolst_joken_blackbox.exe`
- **関連要件**: US-005
- **種別**: 回帰テスト
- **前提条件**: TC-008 と同様
- **入力**: EXE を引数なしで実行
- **期待結果**: `PASS=51, FAIL=0` かつ Exit code = 0

---

### TC-011: 回帰テスト — test_schedule_blackbox.exe 全 PASS

- **対象**: `test_schedule_blackbox.exe`
- **関連要件**: US-005
- **種別**: 回帰テスト
- **前提条件**: TC-008 と同様
- **入力**: EXE を引数なしで実行
- **期待結果**: `FAIL=0` かつ Exit code = 0

---

### TC-012: 回帰テスト — test_type_safety_blackbox.exe 全 PASS

- **対象**: `test_type_safety_blackbox.exe`
- **関連要件**: US-005, US-004
- **種別**: 回帰テスト
- **前提条件**: TC-008 と同様
- **入力**: EXE を引数なしで実行
- **期待結果**: `FAIL=0` かつ Exit code = 0

---

### TC-013: NzInt を Option Strict On 下でコンパイルが通ること

- **対象**: `Utils.NzInt` 呼び出し箇所（Form_ContractEntry.vb 等）
- **関連要件**: US-004, FR-005, NFR-005
- **種別**: コンパイルテスト（静的検査）
- **前提条件**: 修正後の Utils.vb と呼び出し元ファイルに `Option Strict On` を追加
- **入力**: `vbc /optionstrict+ /r:LeaseM4BS.DataAccess.dll <対象ファイル>.vb`
- **期待結果**: コンパイルエラー 0 件

---

### TC-014: KeijoCalculationEngine — NULL ガード後の CDate 変換が InvalidCastException を起こさないこと

- **対象**: `KeijoCalculationEngine.vb` 行 127, 129 (start_dt, b_rend_dt)
- **関連要件**: US-001, FR-001
- **種別**: 異常系
- **前提条件**: `row("start_dt") = DBNull.Value` のインメモリ DataRow を用意
- **入力**: start_dt が DBNull の行を KeijoCalculationEngine に渡す
- **期待結果**: InvalidCastException が発生しない（ガード後スキップまたはデフォルト値が使われる）

---

### TC-015: GsonScheduleBuilder — gson_dt が DBNull の行を渡した場合の動作確認

- **対象**: `GsonScheduleBuilder.BuildFromRows`
- **関連要件**: US-001, FR-001
- **種別**: 異常系
- **前提条件**: `gson_dt = DBNull.Value` の DataRow を含む DataTable を用意
- **入力**: DBNull の gson_dt を含む DataRowCollection
- **期待結果**: InvalidCastException が発生しない（既存 test_type_safety_blackbox の TC-005 に対応）

---

### TC-016: 未実装メニュー — MessageBox が表示されること（スモークテスト）

- **対象**: `Form_MAIN.vb` — システムタブ未実装メニュー項目
- **関連要件**: US-003, FR-006
- **種別**: 正常系（スモーク）
- **前提条件**: Form_MAIN が起動した状態（ログイン済み）
- **入力**: 「データ保存」「DB削除」等の未実装メニュー項目をクリック
- **期待結果**:
  - 「未実装です」の MessageBox が表示される（クラッシュしない）
  - 例外ダイアログが表示されない
- **備考**: 自動化が困難なため目視確認でよい。CI からは除外。

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| NzInt_ValidInt | `42` | NzInt 正常系 |
| NzInt_ValidString | `"99"` | 文字列からの変換確認 |
| NzInt_ZeroString | `"0"` | ゼロ変換確認 |
| ContractEntry_NewRecord | isNewMode=True, 全必須フィールド入力 | INSERT 確認 |
| ContractEntry_ExistRecord | isNewMode=False, 既存 kykh_kykh_id | UPDATE 確認 |
| GsonDt_Valid | `New Date(2024, 6, 30)` | GsonScheduleBuilder 正常系 |
| StartDt_Valid | `New Date(2024, 4, 1)` | KeijoCalculationEngine 正常系 |

### 異常データ

| データ名 | 値 | 期待エラー |
|---|---|---|
| NzInt_DBNull | `DBNull.Value` | エラーなし (0 を返す) |
| NzInt_Nothing | `Nothing` | エラーなし (0 を返す) |
| GsonDt_DBNull | `gson_dt = DBNull.Value` の DataRow | InvalidCastException が発生しないこと |
| StartDt_DBNull | `start_dt = DBNull.Value` の DataRow | InvalidCastException が発生しないこと |
| ContractEntry_UpdateFail | DB 接続遮断後に isNewMode=False で保存 | 適切なエラーメッセージ MessageBox |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| NzInt_IntMax | `Integer.MaxValue` (2147483647) | オーバーフロー確認 |
| NzInt_IntMin | `Integer.MinValue` (-2147483648) | アンダーフロー確認 |
| NzInt_EmptyString | `""` | 空文字列の変換 |
| CDate_MinDate | `Date.MinValue` | 最小日付変換 |
| CDate_MaxDate | `Date.MaxValue` | 最大日付変換 |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_bugfix_blackbox.vb` (新規作成) | NzInt/NzDec/NzDate 型安全性, KeijoCalculationEngine NULL ガード, GsonScheduleBuilder NULL ガード | 13 件 (TC-001〜TC-004, TC-013〜TC-015 + 追加ケース) |
| `c:\kobayashi_LeaseM4BS\test_e2e_blackbox.vb` (既存・変更なし) | E2E フロー全体 | 既存 |
| `c:\kobayashi_LeaseM4BS\test_keijo_joken_blackbox.vb` (既存・変更なし) | 計上条件 | 既存 |
| `c:\kobayashi_LeaseM4BS\test_chuki_idolst_joken_blackbox.vb` (既存・変更なし) | 注記・異動条件 (51 件) | 既存 |
| `c:\kobayashi_LeaseM4BS\test_schedule_blackbox.vb` (既存・変更なし) | スケジュール計算 | 既存 |
| `c:\kobayashi_LeaseM4BS\test_type_safety_blackbox.vb` (既存・変更なし) | 型安全性全般 | 既存 |

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

**モジュール構造**:
```vb
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports LeaseM4BS.DataAccess

Module TestBugfixBlackBox
    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== バグ修正確認 ブラックボックステスト ===")
        ' 各テストメソッドを呼ぶ
        Console.WriteLine(String.Format("=== 結果: PASS={0}, FAIL={1}, SKIP={2} ===", passCount, failCount, skipCount))
        If failCount > 0 Then Environment.ExitCode = 1
    End Sub
End Module
```

**アサーションヘルパー** (test_e2e_blackbox.vb と同一シグネチャを使用):
```vb
Sub Pass(label As String)
    Console.WriteLine("PASS")
    passCount += 1
End Sub

Sub Fail(label As String, expected As String, actual As String)
    Console.WriteLine("FAIL (expected=" & expected & ", actual=" & actual & ")")
    failCount += 1
End Sub

Sub Skip(label As String)
    Console.WriteLine("SKIP (" & label & ")")
    skipCount += 1
End Sub
```

**DB 例外ハンドリング** (test_e2e_blackbox.vb の HandleDbException と同一):
```vb
Sub HandleDbException(label As String, ex As Exception)
    Dim rootMsg As String = ex.Message
    Dim inner As Exception = ex.InnerException
    While inner IsNot Nothing
        rootMsg = inner.Message
        inner = inner.InnerException
    End While
    If rootMsg.Contains("Connection") OrElse rootMsg.Contains("refused") Then
        Skip(label & " (DB接続不可)")
    Else
        Fail(label, "success", "Exception: " & rootMsg)
    End If
End Sub
```

### モック / スタブの使い方

- DB 不要テストでは `New DataTable()` を使ったインメモリデータで完結させる
- DB 必要テストは既存パターン通り SKIP ロジックで対応する
- Form 層テスト (TC-005〜TC-007) は DB あり環境でのみ実行可能であるため、CI では SKIP 扱い

### テストヘルパー / ユーティリティの活用

- `AssertEqual(label, expected As Double, actual As Double)` — 浮動小数点比較 (誤差 < 0.001)
- `AssertEqual(label, expected As Integer, actual As Integer)` — 整数比較
- `AssertEqual(label, expected As Boolean, actual As Boolean)` — 真偽値比較
- `AssertEqualDate(label, expected As Date, actual As Date)` — 日付比較
- これらは test_type_safety_blackbox.vb / test_e2e_blackbox.vb から同一実装をコピーして使う

---

## 8. テスト実行方法（vbc.exe コンパイル・実行手順）

### 手順 1: DataAccess DLL のビルド

```bash
cd c:\kobayashi_LeaseM4BS\LeaseM4BS\LeaseM4BS.DataAccess
msbuild LeaseM4BS.DataAccess.vbproj /p:Configuration=Debug /t:Build
```

### 手順 2: 新規テストファイルのコンパイル

```bash
cd c:\kobayashi_LeaseM4BS
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll /r:System.dll /r:Npgsql.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll ^
    test_bugfix_blackbox.vb /out:test_bugfix_blackbox.exe
```

### 手順 3: 新規テストの実行

```bash
test_bugfix_blackbox.exe
```

### 手順 4: 既存回帰テストの全実行

```bash
test_e2e_blackbox.exe
echo E2E: %ERRORLEVEL%

test_keijo_joken_blackbox.exe
echo KEIJO: %ERRORLEVEL%

test_chuki_idolst_joken_blackbox.exe
echo CHUKI: %ERRORLEVEL%

test_schedule_blackbox.exe
echo SCHEDULE: %ERRORLEVEL%

test_type_safety_blackbox.exe
echo TYPE_SAFETY: %ERRORLEVEL%
```

全 EXE の Exit code が 0 であることを確認する。

### 手順 5: Option Strict On コンパイルチェック

```bash
' 修正対象ファイルの型安全性コンパイル確認 (例: Utils.vb + Form_ContractEntry.vb)
vbc /optionstrict+ /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Windows.Forms.dll ^
    Utils.vb Form_ContractEntry.vb /out:NUL
' エラー 0 件であること
```

---

## 9. テスト成功基準

| 基準 | 判定条件 |
|---|---|
| 新規テスト (TC-001〜TC-015) | `test_bugfix_blackbox.exe` の FAIL = 0 かつ Exit code = 0 |
| E2E 回帰テスト | `test_e2e_blackbox.exe` の FAIL = 0 かつ Exit code = 0 |
| 計上条件回帰テスト | `test_keijo_joken_blackbox.exe` の FAIL = 0 かつ Exit code = 0 |
| 注記・異動回帰テスト | `test_chuki_idolst_joken_blackbox.exe` の PASS = 51, FAIL = 0 かつ Exit code = 0 |
| スケジュール回帰テスト | `test_schedule_blackbox.exe` の FAIL = 0 かつ Exit code = 0 |
| 型安全性回帰テスト | `test_type_safety_blackbox.exe` の FAIL = 0 かつ Exit code = 0 |
| Option Strict On コンパイル | 修正対象ファイルでコンパイルエラー 0 件 |
| Form_ContractEntry UPDATE | isNewMode=False 保存時に UPDATE が実行され INSERT が発生しないこと (DB 観点) |
| 未実装メニュー | クリック時に「未実装です」MessageBox が表示され例外ダイアログが出ないこと (目視確認) |

---

## 10. リスクと注意事項

| リスク | 対応方針 |
|---|---|
| Form_ContractEntry の WinForms テストは自動化が困難 | DB 直接確認 (INSERT/UPDATE 件数) + 目視確認を組み合わせる |
| KeijoCalculationEngine の NULL ガード修正が計算結果に影響する可能性 | TC-008〜TC-012 の回帰テストで確認。FAIL が出た場合は修正を見直す |
| NzInt の型変更が既存呼び出し元でコンパイルエラーを起こす | 手順 5 の Option Strict On コンパイルチェックで全件確認する |
| PostgreSQL が稼働していない環境での DB 必要テスト | SKIP 扱いとし、CI 環境 (GitHub Actions) で実行することで担保する |
