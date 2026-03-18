# テスト計画書: TODO/FIXME体系的解消

## 1. テスト戦略

### テストフレームワーク
- **NUnit等の標準フレームワーク**: 不使用（既存プロジェクトに導入なし）
- **採用方式**: ブラックボックステスト（コンソール exe）+ 手動確認手順
- **ビルド**: MSBuild (vbc.exe) でコンパイル、コマンドラインで実行

### テストレベル
| レベル | 手法 | 対象 |
|---|---|---|
| ユニットテスト相当 | ブラックボックステスト（DB不要） | Pure関数、型安全性、ロジック分岐 |
| 結合テスト相当 | ブラックボックステスト（DB接続あり） | SQL正確性、CRUD、データ整合性 |
| 手動確認 | 画面操作による目視確認 | UI（グレーアウト、表示制御） |

### カバレッジ目標
- CRITICAL修正: 100%（全受入条件に対応するテストケース）
- HIGH修正: 80%以上（主要な正常系・異常系）
- MEDIUM/LOW修正: 任意（回帰防止を優先）

### モック戦略
- DB接続が必要なテストは **SKIPパターン**（DB接続不可時は `Skip(label & " (DB接続不可)")` として続行）
- DB不要なテストはインメモリ `DataTable` を使用
- 既存パターン（`HandleDbException` サブルーチン）を流用

---

## 2. 既存テスト環境の調査結果

### 2.1 テストファイル構成（現行）

| ファイルパス | 種別 | 対象機能 |
|---|---|---|
| `test_bugfix_blackbox.vb` | ブラックボックス（DB不要） | NzInt/NzDec/NzDate型安全性、DBNull NULLガード |
| `test_chuki_idolst_joken_blackbox.vb` | ブラックボックス（DB不要） | CHUKI_JOKEN・IDOLST_JOKENのPure関数 |
| `test_keijo_joken_blackbox.vb` | ブラックボックス（DB接続あり） | KeijoCalculationEngine |
| `test_schedule_blackbox.vb` | ブラックボックス（DB接続あり） | スケジュールビルダー |
| `test_e2e_blackbox.vb` | E2Eテスト（DB接続あり） | 一連の業務フロー |
| `test_type_safety_blackbox.vb` | ブラックボックス（DB不要） | 型安全性全般 |
| `test_diag.vb` | 診断ツール | DB接続診断 |
| `test_npgsql_diag.vb` | 診断ツール | Npgsql接続診断 |
| `test_fixed_length.vb` | ブラックボックス（DB不要） | 固定長ファイル出力 |

### 2.2 採用する既存パターン

**テストモジュール構造:**
```vb
Option Strict On
Option Explicit On

Module TestXxxBlackBox
    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== <テスト名> ブラックボックステスト ===")
        ' テストケース呼び出し
        Console.WriteLine($"=== 結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount} ===")
        If failCount > 0 Then Environment.ExitCode = 1
    End Sub
End Module
```

**アサーションパターン:**
```vb
Sub AssertEqual(label As String, expected As Integer, actual As Integer)
Sub AssertContains(label As String, text As String, substr As String)
Sub AssertTrue(label As String, condition As Boolean)
Sub Pass(label As String)
Sub Fail(label As String, expected As String, actual As String)
Sub Skip(label As String)
```

**DB例外ハンドリング（既存パターン流用）:**
```vb
Sub HandleDbException(label As String, ex As Exception)
    Dim rootMsg As String = GetRootMessage(ex)
    If rootMsg.Contains("does not exist") Then
        Skip(label & " (DBスキーマ未完了: ...)")
    ElseIf rootMsg.Contains("refused") Then
        Skip(label & " (DB接続不可)")
    Else
        Fail(label, "success", "Exception: " & rootMsg)
    End If
End Sub
```

**コンパイルコマンド雛形:**
```bash
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe" /target:exe /out:<テスト名>.exe \
  /reference:LeaseM4BS/LeaseM4BS.DataAccess/bin/Debug/LeaseM4BS.DataAccess.dll \
  /reference:LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/bin/Debug/LeaseM4BS.TestWinForms.exe \
  /reference:"C:\Program Files (x86)\Npgsql\net461\Npgsql.dll" \
  <テスト名>.vb
```

---

## 3. テストケース一覧

### 3.1 CRITICAL修正のテスト

#### FR-C-001: Form_f_CHUKI_RECALC の危険フラグ解消（C1）

##### TC-C1-001: gnzaiKt 計算式の確認
- **対象**: `Form_f_CHUKI_RECALC.vb:30`、`ResetChukiData()` 内のコメントアウト計算式
- **関連要件**: FR-C-001
- **種別**: 手動確認
- **前提条件**: DBにテスト契約データが存在すること（`d_kykm.kykm_id` が存在する行）
- **入力**: b_knyukn=1000000, b_gnzai_kt=800000 のテストデータ
- **期待結果**: `gnzaiKt = LEAST(b_knyukn, b_gnzai_kt) = 800000` が設定されること
- **確認手順**: `ResetChukiData()` 実行後に `d_kykm` を SELECT し、更新後の値を目視確認

##### TC-C1-002: コメントアウト項目（chuumId, kjkbnId, szeiKjkbnId）の更新確認
- **対象**: `Form_f_CHUKI_RECALC.vb:30`
- **関連要件**: FR-C-001
- **種別**: 手動確認
- **前提条件**: テスト用 d_kykm レコードが存在すること
- **入力**: 実行前の chuumId, kjkbnId, szeiKjkbnId の初期値を記録
- **期待結果**: `ResetChukiData()` 実行後、各項目が期待値に更新されるか、または「実装不可」コメントに変更されていること
- **確認手順**: 実装後コードのコメントを確認し、TODOコメントが消えていることを確認

##### TC-C1-003: 危険フラグコメントの除去確認（静的確認）
- **対象**: `Form_f_CHUKI_RECALC.vb:30`
- **関連要件**: FR-C-001
- **種別**: 静的コード確認（DB不要）
- **確認手順**: `grep -n "todo 危険" LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\Form_f_CHUKI_RECALC.vb` の結果が0件であること

---

#### FR-C-002: Form_BuknEntry の削除処理（C2）

##### TC-C2-001: 削除処理実行 - 正常系（子レコード含む）
- **対象**: `Form_BuknEntry.vb:305`、`cmd_DELETE_Click` イベントハンドラ
- **関連要件**: FR-C-002
- **種別**: 結合テスト（DB接続あり）
- **前提条件**:
  - テスト用 `d_kykm` レコードを INSERT しておく（`kykm_id=9999`）
  - 関連する `d_haif` レコードも INSERT しておく（`kykm_id=9999`）
- **入力**: `kykm_id=9999` のテストレコード
- **期待結果**:
  1. `d_haif WHERE kykm_id=9999` のレコード数が 0 になること
  2. `d_kykm WHERE kykm_id=9999` のレコード数が 0 になること
- **確認クエリ**:
  ```sql
  SELECT COUNT(*) FROM d_haif WHERE kykm_id = 9999;
  SELECT COUNT(*) FROM d_kykm WHERE kykm_id = 9999;
  ```

##### TC-C2-002: 削除処理 - 確認ダイアログ後にキャンセル
- **対象**: `Form_BuknEntry.vb`、確認ダイアログ処理
- **関連要件**: FR-C-002
- **種別**: 手動確認
- **前提条件**: テスト用 d_kykm レコードが存在すること
- **入力**: [削除]ボタン押下後、確認ダイアログで「いいえ」を選択
- **期待結果**: レコードが削除されず、画面が閉じないこと

##### TC-C2-003: 削除後の画面クローズ確認
- **対象**: `Form_BuknEntry.vb`、`Me.Close()` 呼び出し
- **関連要件**: FR-C-002
- **種別**: 手動確認
- **前提条件**: テスト用 d_kykm レコードが存在すること
- **入力**: [削除]ボタン押下後、確認ダイアログで「はい」を選択
- **期待結果**: 削除成功後に画面が閉じること

##### TC-C2-004: 外部キー制約エラーなし確認
- **対象**: `Form_BuknEntry.vb`、削除SQL実行
- **関連要件**: FR-C-002
- **種別**: 結合テスト（DB接続あり）
- **前提条件**: `d_kykm` に対して `d_haif` が外部キー参照している場合
- **入力**: `d_haif` に子レコードが存在する `kykm_id`
- **期待結果**: 子レコードを先に削除してから親レコードを削除し、`PostgresException(23503)` が発生しないこと

---

#### FR-C-003: CalcGhassei の発生リース料計算（C3）

##### TC-C3-001: CalcGhassei の戻り値が CalcKlsryo と同値でないこと（利息法）
- **対象**: `Form_f_CHUKI_SCH.vb:101`、`CalcGhassei()` メソッド
- **関連要件**: FR-C-003
- **種別**: ユニットテスト相当（DB不要）
- **前提条件**: ファイナンスリース（利息法）のテストデータを用意
- **入力**: 支払リース料=100000, 利息額=5000 のスケジュールデータ
- **期待結果**: 発生リース料 = 元本返済額 = 95000（利息法の場合）
- **確認手順**: ブラックボックステストで `CalcGhassei()` の戻り値を `CalcKlsryo()` と比較

##### TC-C3-002: CalcGhassei の戻り値（利子込法）
- **対象**: `Form_f_CHUKI_SCH.vb:101`
- **関連要件**: FR-C-003
- **種別**: ユニットテスト相当（DB不要）
- **前提条件**: ファイナンスリース（利子込法）のテストデータ
- **入力**: 支払リース料=100000 のデータ
- **期待結果**: 発生リース料 = 支払リース料 = 100000（利子込法では等値）

---

### 3.2 HIGH修正のテスト

#### FR-H-002: Form_f_flx_TOUGETSU の法令区分・取引区分・請求月（H4）

##### TC-H4-001: 法令区分 - SEKOU_DT以後は新法
- **対象**: `Form_f_flx_TOUGETSU.vb:87`、BuildSql() 内の法令区分 CASE 文
- **関連要件**: FR-H-002
- **種別**: DB接続あり（SEKOU_DT を t_settei から取得）
- **前提条件**: `t_settei` に `settei_nm='SEKOU_DT'` で値='2008/04/01' が設定されていること
- **入力**: 契約開始日 2009/01/01 のデータ
- **期待結果**: 法令区分列が「新法」を示す値になること
- **参照実装**: `Form_f_flx_KEIJO.vb:51` の CASE 文

##### TC-H4-002: 法令区分 - SEKOU_DT前は旧法
- **対象**: `Form_f_flx_TOUGETSU.vb:87`
- **関連要件**: FR-H-002
- **種別**: DB接続あり
- **入力**: 契約開始日 2007/01/01 のデータ
- **期待結果**: 法令区分列が「旧法」を示す値になること

##### TC-H4-003: WHERE句 - 集計期間フィルタ（DtFrom/DtTo）
- **対象**: `Form_f_flx_TOUGETSU.vb:115`
- **関連要件**: FR-H-004
- **種別**: DB接続あり
- **入力**: DtFrom=2024/04/01, DtTo=2025/03/31
- **期待結果**: `start_dt <= '2025-03-31' AND end_dt >= '2024-04-01'` の条件が SQL に含まれること
- **確認手順**: 生成SQL文字列を出力して AssertContains で確認

---

#### FR-H-003 / FR-H-005: Form_f_flx_YOSAN のグレーアウトと検索条件（H3, H5）

##### TC-H3-001: グレーアウト - 期間外かつ計上額0の行がグレー表示
- **対象**: `Form_f_flx_YOSAN.vb:42`、グレーアウト処理
- **関連要件**: FR-H-003
- **種別**: 手動確認（UI確認）
- **前提条件**: 集計期間外（start_dt > DtTo）かつ計上額=0 のテストデータを用意
- **期待結果**: 該当行の `ForeColor = Color.Gray, BackColor = RGB(240,240,240)` になること
- **参照実装**: `Form_f_flx_KHIYO.vb:253-274`

##### TC-H3-002: グレーアウト - 期間内の行は通常表示
- **対象**: `Form_f_flx_YOSAN.vb:42`
- **関連要件**: FR-H-003
- **種別**: 手動確認（UI確認）
- **前提条件**: 集計期間内（start_dt <= DtTo AND end_dt >= DtFrom）のテストデータ
- **期待結果**: 該当行が通常色（白背景・黒文字）で表示されること

##### TC-H5-001: 検索条件 - TryParse 失敗時の例外なし
- **対象**: `Form_f_flx_YOSAN.vb:106`、検索テキスト入力処理
- **関連要件**: FR-H-005
- **種別**: ユニットテスト相当（DB不要）
- **前提条件**: なし
- **入力**: テキストボックスに "abc"（整数変換不可の文字列）を入力
- **期待結果**: `FormatException` や `OverflowException` が発生せず、画面が正常に動作すること

##### TC-H5-002: 検索条件 - 有効な整数で WHERE句が生成される
- **対象**: `Form_f_flx_YOSAN.vb:106`
- **関連要件**: FR-H-005
- **種別**: DB接続あり
- **入力**: テキストボックスに "12345" を入力して検索
- **期待結果**: `WHERE kykm_no = @search` 相当の条件が SQL に含まれ、結果が絞り込まれること

---

#### FR-H-001: Form_f_flx_BUKN の保守料列（H1）

##### TC-H1-001: 保守料列が SQL に追加されていること
- **対象**: `Form_f_flx_BUKN.vb:48`
- **関連要件**: FR-H-001
- **種別**: 静的コード確認（DB不要）
- **前提条件**: 実装完了後
- **確認手順**: `grep -n "todo 該当項目不明" Form_f_flx_BUKN.vb` の結果が0件であること

##### TC-H1-002: 保守料列がグリッドに表示される
- **対象**: `Form_f_flx_BUKN.vb:48`
- **関連要件**: FR-H-001
- **種別**: 手動確認（DB接続あり）
- **前提条件**: d_henf テーブルに保守料データが存在すること
- **期待結果**: 物件フレックス一覧に「保守料」列が表示され、値が正しいこと

---

### 3.3 MEDIUM修正のテスト

#### FR-M-002: Form_BuknEntry メソッド名変更（M2, L3, L4, L5）

##### TC-M2-001: メソッドリネーム後のビルド確認
- **対象**: `Form_BuknEntry.vb:188`、`Form_fc_TC_HREL.vb:78`, `Form_fc_TC_HREL_YOBI.vb:129`
- **関連要件**: FR-M-002
- **種別**: ビルドテスト（コンパイル確認）
- **確認手順**: `msbuild LeaseM4BS\LeaseM4BS.slnx` がエラーなく完了すること
- **期待結果**: ビルドエラー0件

#### FR-M-003: YOSAN 列ヘッダの年月表示（M3）

##### TC-M3-001: 列ヘッダが yyyy/MM 形式で表示される
- **対象**: `Form_f_flx_YOSAN.vb:201`
- **関連要件**: FR-M-003
- **種別**: 手動確認（UI確認）
- **前提条件**: 集計開始月=2024/04
- **期待結果**: グリッド列ヘッダが `2024/04`, `2024/05`, ... と表示されること（`m0`, `m1` ではなく）

#### FR-L-003: Form_f_IDOLST_JOKEN TrimEnd 確認（L3）

##### TC-L3-001: TrimEnd("、") - bcat4=True, bcat5=False
- **対象**: `Form_f_IDOLST_JOKEN.vb:62`、`GetLabelTextPure()`
- **関連要件**: FR-L-003
- **種別**: ユニットテスト相当（DB不要）
- **前提条件**: なし
- **入力**: bcat1=False, bcat2=False, bcat3=False, bcat4=True, bcat5=False
- **期待結果**: 返却文字列が「、」で終わらないこと（TrimEnd が正しく機能すること）
- **確認手順**: 既存の `test_chuki_idolst_joken_blackbox.vb` に `Test_25_IdolstBcat4Only_TrailingComma` として追加確認

---

### 3.4 LOW修正のテスト

#### FR-L-002: c_leakbn テーブルID値確認（L2）

##### TC-L2-001: c_leakbn テーブルの ID 値確認
- **対象**: `Form_f_CHUKI_JOKEN.vb:184`
- **関連要件**: FR-L-002
- **種別**: DB接続あり（確認のみ）
- **確認クエリ**:
  ```sql
  SELECT leakbn_id, leakbn_nm FROM c_leakbn ORDER BY leakbn_id;
  ```
- **期待結果**: `leakbn_id=1` が「所有権移転外ファイナンスリース」、`leakbn_id=2` が「オペレーティングリース」であること
- **後処理**: `test_chuki_idolst_joken_blackbox.vb` の期待値（`IN (1, 2)` 等）と整合していることを確認

---

## 4. テストデータ準備

### 4.1 正常データ（DB接続テスト用）

| データ名 | テーブル | 値 | 用途 |
|---|---|---|---|
| テスト物件（削除用） | d_kykm | kykm_id=9999, kykm_no=99999, leakbn_id=1 | TC-C2-001〜004 |
| テスト配賦（削除用） | d_haif | kykm_id=9999 の配賦行 | TC-C2-001, TC-C2-004 |
| 新法テスト契約 | d_kykm | start_dt='2009-01-01', leakbn_id=1 | TC-H4-001 |
| 旧法テスト契約 | d_kykm | start_dt='2007-01-01', leakbn_id=1 | TC-H4-002 |
| YOSAN期間外データ | d_kykm | end_dt < DtFrom のデータ | TC-H3-001 |

**テストデータ投入 SQL 雛形（実行前に既存データ確認を必須とする）:**
```sql
-- TC-C2 用テストデータ
INSERT INTO d_kykm (kykm_id, kykm_no, leakbn_id, ...)
VALUES (9999, 99999, 1, ...)
ON CONFLICT (kykm_id) DO NOTHING;

-- クリーンアップ（テスト後）
DELETE FROM d_haif WHERE kykm_id = 9999;
DELETE FROM d_kykm WHERE kykm_id = 9999;
```

### 4.2 異常データ

| データ名 | 値 | 期待エラー |
|---|---|---|
| 検索条件 - 非数値 | テキスト="abc" | FormatException が発生しないこと（TryParse使用） |
| 削除対象なし | kykm_id=99998（存在しない） | 0件削除（エラーなし）、または適切なメッセージ表示 |
| 子レコード付き削除 | kykm_id に d_haif が存在する | 親子順での削除成功（PostgresException 23503 なし） |

### 4.3 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| SEKOU_DT ちょうど | start_dt='2008-04-01' | 新法/旧法の境界（新法に含まれるか確認） |
| bcat4=True, bcat5=False | GetLabelTextPure 入力 | TrimEnd が末尾「、」を除去するか |
| 検索条件=Integer.MaxValue | テキスト="2147483647" | Integer.TryParse 境界値 |
| 検索条件=0 | テキスト="0" | 0件になる可能性（正常動作確認） |

---

## 5. 回帰テスト計画

### 5.1 既存テストの再実行

実装変更後は以下の既存テストが全て PASS であることを確認する:

| テストファイル | 確認コマンド | 確認目的 |
|---|---|---|
| `test_bugfix_blackbox.exe` | `.\test_bugfix_blackbox.exe` | NzInt/NzDec/NzDate 型安全性が損なわれていないこと |
| `test_chuki_idolst_joken_blackbox.exe` | `.\test_chuki_idolst_joken_blackbox.exe` | CHUKI_JOKEN/IDOLST_JOKEN Pure関数が正常動作すること |
| `test_type_safety_blackbox.exe` | `.\test_type_safety_blackbox.exe` | 型安全性全般が維持されること |
| `test_schedule_blackbox.exe` | `.\test_schedule_blackbox.exe` | スケジュール計算が正常動作すること |
| `test_e2e_blackbox.exe` | `.\test_e2e_blackbox.exe` | E2Eフロー全体が正常動作すること |

### 5.2 ビルド回帰確認

```bash
# ソリューション全体のビルド確認
msbuild LeaseM4BS\LeaseM4BS.slnx /p:Configuration=Debug

# 期待結果: Build succeeded. 0 Error(s)
```

### 5.3 Form_f_flx_KEIJO との整合性確認

`Form_f_flx_TOUGETSU` に法令区分を追加した際は、`Form_f_flx_KEIJO` の法令区分ロジックと同一 SQL（CASE 文）が使われていることを確認する:

```bash
# Form_f_flx_KEIJO.vb の法令区分 CASE 文を抽出して比較
grep -A 5 "SEKOU_DT" LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_flx_KEIJO.vb
grep -A 5 "SEKOU_DT" LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_flx_TOUGETSU.vb
```

---

## 6. テスト実行手順

### 6.1 DB不要テスト（ブラックボックス）

```bash
# 1. テストファイルを作成（ルートに test_todo_fixme_blackbox.vb として追加）
# 2. コンパイル
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\vbc.exe" /target:exe /out:test_todo_fixme_blackbox.exe ^
  /reference:LeaseM4BS/LeaseM4BS.DataAccess/bin/Debug/LeaseM4BS.DataAccess.dll ^
  /reference:LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/bin/Debug/LeaseM4BS.TestWinForms.exe ^
  /reference:"C:\Program Files (x86)\Npgsql\net461\Npgsql.dll" ^
  test_todo_fixme_blackbox.vb

# 3. 実行
.\test_todo_fixme_blackbox.exe
```

### 6.2 DB接続テスト

```bash
# 事前: PostgreSQL DB への接続確認
.\test_diag.exe

# テストデータ投入（テスト用レコードを INSERT）
psql -h localhost -U postgres -d lease_m4bs -f test_data_setup.sql

# テスト実行
.\test_todo_fixme_blackbox.exe

# テストデータクリーンアップ
psql -h localhost -U postgres -d lease_m4bs -f test_data_cleanup.sql
```

### 6.3 手動確認テスト

1. `LeaseM4BS.TestWinForms.exe` を起動
2. 対象フォームを開く
3. テストケースに記載の入力値を投入し、期待結果を目視確認
4. 結果をスクリーンショットまたはメモで記録

### 6.4 完了判定

以下の全条件を満たすことでテスト完了とする:

- [ ] `test_todo_fixme_blackbox.exe` が FAIL=0 で完了すること
- [ ] 既存テスト（`test_bugfix_blackbox.exe` 等）が全て PASS または SKIP のみであること
- [ ] ソリューションのビルドが `0 Error(s)` で完了すること
- [ ] TC-C2-001（BuknEntry削除）の DB 確認クエリが 0件を返すこと
- [ ] TC-H5-001（YOSAN検索条件の例外なし）が手動確認または自動テストでPASSすること
- [ ] `Form_f_CHUKI_RECALC.vb` に `todo 危険` コメントが残っていないこと

---

## 7. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数（予定） |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_todo_fixme_blackbox.vb` | FR-C-001〜003, FR-H-002〜005, FR-L-003 | 約20件 |
| 既存: `test_bugfix_blackbox.vb` | NzInt型安全性、NULLガード（回帰） | 17件（既存） |
| 既存: `test_chuki_idolst_joken_blackbox.vb` | CHUKI/IDOLST Pure関数（回帰） | 31件（既存） |
| 既存: `test_e2e_blackbox.vb` | E2Eフロー（回帰） | 既存 |
| 手動確認手順書（本ドキュメント §6.3） | TC-C1-001〜003, TC-H1-002, TC-H3-001〜002, TC-M3-001 | 7件 |

---

## 8. 既存テストパターンとの整合性

### 採用する既存パターン
- **モジュール構造**: `Module TestXxxBlackBox` + `passCount/failCount/skipCount` カウンター
- **アサーション**: `AssertEqual`, `AssertContains`, `AssertNotContains`, `AssertTrue` を `test_chuki_idolst_joken_blackbox.vb` から流用
- **DBエラーハンドリング**: `HandleDbException` サブルーチン（`test_bugfix_blackbox.vb` の実装パターン）
- **スキップ判定**: DB接続不可・スキーマ未完了の場合は `Skip()` で処理継続（テスト全体が止まらないようにする）
- **終了コード**: `failCount > 0` の場合 `Environment.ExitCode = 1`（CI 連携を想定）

### テストヘルパーの活用
- Pure関数（DB不要）のテストは `Option Strict On` でコンパイル確認を兼ねる
- DB接続テストは `HandleDbException` で接続不可時に自動SKIP
- インメモリ `DataTable` + `DataRow` でロジック部分の単体確認（`test_bugfix_blackbox.vb` TC-014, TC-015 パターン）
