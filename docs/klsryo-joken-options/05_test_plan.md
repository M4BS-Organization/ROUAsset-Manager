# テスト計画書: klsryo-joken-options

## 1. テスト戦略

- **テストフレームワーク**: Console.WriteLineベースのカスタムアサーション（既存ブラックボックステストと同一パターン）
- **テストレベル**: 単体テスト（WinFormsコントロール値の読み取りロジック） + エンジン統合テスト（DBあり・スキップ可）
- **カバレッジ目標**: 3グループ×全選択肢のデフォルト値確認 + 排他制御 + パラメータ受け渡しの全パスカバレッジ
- **モック戦略**: `Form_f_KLSRYO_JOKEN` をインスタンス化してコントロール値を直接設定する。DB接続が必要なエンジンテストはSkipパターンで保護する。

---

## 2. テスト環境

### 前提条件
- `LeaseM4BS.TestWinForms.exe`（`Form_f_KLSRYO_JOKEN` を含む）のビルドが完了していること
- `LeaseM4BS.DataAccess.dll` のビルドが完了していること
- .NET Framework 4.7.2 環境

### コンパイルコマンド
```bash
vbc /r:LeaseM4BS.TestWinForms.exe /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll test_klsryo_joken_options_blackbox.vb
```

### テストデータの準備方法
- DB接続不要なテスト（TC-001〜TC-012）: コード内でコントロール値を直接設定
- DB接続が必要なテスト（TC-013〜TC-015）: Try/Catch + Skip() パターンで保護

### 外部依存のモック方法
- `Form_f_KLSRYO_JOKEN` をインスタンス化し、各RadioButtonの `Checked` プロパティを設定
- `Form_f_flx_KLSRYO` のプロパティ受け取り確認は `Taisho`/`Ktmg`/`Meisai` の公開プロパティを参照
- `KlsryoCalculationEngine` はDB接続なしではインスタンス化のみ検証、実行はSkipパターンで対応

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | `オプション508`（全部）のデフォルト選択確認 | 高 | US-001, FR-003 |
| T-002 | `chk_SHIME`（締日ベース）のデフォルト選択確認 | 高 | US-002, FR-003 |
| T-003 | `オプション489`（配賦単位）のデフォルト選択確認 | 高 | US-003, FR-003 |
| T-004 | 集計対象グループ 排他制御（リース料選択時に他2つが非選択） | 高 | US-001, FR-002 |
| T-005 | 集計対象グループ 排他制御（保守料選択時に他2つが非選択） | 高 | US-001, FR-002 |
| T-006 | 集計対象グループ 排他制御（全部選択時に他2つが非選択） | 中 | US-001, FR-002 |
| T-007 | タイミンググループ 排他制御（締日ベース選択時に支払日ベースが非選択） | 高 | US-002, FR-002 |
| T-008 | タイミンググループ 排他制御（支払日ベース選択時に締日ベースが非選択） | 高 | US-002, FR-002 |
| T-009 | 明細グループ 排他制御（物件単位選択時に配賦単位が非選択） | 高 | US-003, FR-002 |
| T-010 | 明細グループ 排他制御（配賦単位選択時に物件単位が非選択） | 中 | US-003, FR-002 |
| T-011 | Taisho変換ロジック（リース料選択 → Taisho=1） | 高 | US-001, FR-004 |
| T-012 | Taisho変換ロジック（保守料選択 → Taisho=2） | 高 | US-001, FR-004 |
| T-013 | Taisho変換ロジック（全部選択 → Taisho=3） | 高 | US-001, FR-004 |
| T-014 | Ktmg変換ロジック（締日ベース → ShriKtmg.SimeDtBase） | 高 | US-002, FR-004 |
| T-015 | Ktmg変換ロジック（支払日ベース → ShriKtmg.ShriDtBase） | 高 | US-002, FR-004 |
| T-016 | Meisai変換ロジック（物件単位 → ShriMeisai.Kykm） | 高 | US-003, FR-004 |
| T-017 | Meisai変換ロジック（配賦単位 → ShriMeisai.Haif） | 高 | US-003, FR-004 |
| T-018 | エンジン実行 Taisho=1（リース料のみ）→ 付随費用処理スキップ確認 | 中 | US-001, FR-004 |
| T-019 | エンジン実行 Taisho=3 + Ktmg=SimeDtBase + Meisai=Haif（Access版デフォルト） | 高 | US-001〜US-003, FR-004 |
| T-020 | エンジン実行 Taisho=3 + Ktmg=ShriDtBase + Meisai=Kykm | 中 | US-001〜US-003, FR-004 |

---

## 4. テストケース

### TC-001: 集計対象グループ デフォルト選択確認（全部）

- **対象**: `Form_f_KLSRYO_JOKEN` ロード時の `オプション508`
- **関連要件**: US-001, FR-003
- **種別**: 正常系
- **前提条件**: `New Form_f_KLSRYO_JOKEN()` でフォームをインスタンス化（初期状態）
- **入力**: フォームのデフォルト状態
- **期待結果**: `オプション508.Checked = True`、`オプション504.Checked = False`、`オプション506.Checked = False`

---

### TC-002: タイミンググループ デフォルト選択確認（締日ベース）

- **対象**: `Form_f_KLSRYO_JOKEN` ロード時の `chk_SHIME`
- **関連要件**: US-002, FR-003
- **種別**: 正常系
- **前提条件**: `New Form_f_KLSRYO_JOKEN()` でフォームをインスタンス化（初期状態）
- **入力**: フォームのデフォルト状態
- **期待結果**: `chk_SHIME.Checked = True`、`オプション483.Checked = False`
- **注意**: 現状の Designer.vb では `オプション483`（支払日ベース）が Checked=True になっているが、要件定義（FR-003）では締日ベースがデフォルト。実装後の正しい状態を検証する。

---

### TC-003: 明細グループ デフォルト選択確認（配賦単位）

- **対象**: `Form_f_KLSRYO_JOKEN` ロード時の `オプション489`
- **関連要件**: US-003, FR-003
- **種別**: 正常系
- **前提条件**: `New Form_f_KLSRYO_JOKEN()` でフォームをインスタンス化（初期状態）
- **入力**: フォームのデフォルト状態
- **期待結果**: `オプション489.Checked = True`、`オプション487.Checked = False`

---

### TC-004: 集計対象グループ 排他制御（リース料を選択）

- **対象**: `Panel3`（GroupBox2内）の RadioButton グループ排他制御
- **関連要件**: US-001, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション504.Checked = True` をセット
- **入力**: `オプション504.Checked = True`
- **期待結果**: `オプション504.Checked = True`、`オプション506.Checked = False`、`オプション508.Checked = False`

---

### TC-005: 集計対象グループ 排他制御（保守料を選択）

- **対象**: `Panel3`（GroupBox2内）の RadioButton グループ排他制御
- **関連要件**: US-001, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション506.Checked = True` をセット
- **入力**: `オプション506.Checked = True`
- **期待結果**: `オプション506.Checked = True`、`オプション504.Checked = False`、`オプション508.Checked = False`

---

### TC-006: 集計対象グループ 排他制御（全部を選択）

- **対象**: `Panel3`（GroupBox2内）の RadioButton グループ排他制御
- **関連要件**: US-001, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション504.Checked = True` にしてから `オプション508.Checked = True` をセット
- **入力**: `オプション508.Checked = True`
- **期待結果**: `オプション508.Checked = True`、`オプション504.Checked = False`、`オプション506.Checked = False`

---

### TC-007: タイミンググループ 排他制御（締日ベースを選択）

- **対象**: `Panel1`（GroupBox1内）の RadioButton グループ排他制御
- **関連要件**: US-002, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`chk_SHIME.Checked = True` をセット
- **入力**: `chk_SHIME.Checked = True`
- **期待結果**: `chk_SHIME.Checked = True`、`オプション483.Checked = False`

---

### TC-008: タイミンググループ 排他制御（支払日ベースを選択）

- **対象**: `Panel1`（GroupBox1内）の RadioButton グループ排他制御
- **関連要件**: US-002, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション483.Checked = True` をセット
- **入力**: `オプション483.Checked = True`
- **期待結果**: `オプション483.Checked = True`、`chk_SHIME.Checked = False`

---

### TC-009: 明細グループ 排他制御（物件単位を選択）

- **対象**: `Panel2`（GroupBox2内）の RadioButton グループ排他制御
- **関連要件**: US-003, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション487.Checked = True` をセット
- **入力**: `オプション487.Checked = True`
- **期待結果**: `オプション487.Checked = True`、`オプション489.Checked = False`

---

### TC-010: 明細グループ 排他制御（配賦単位を選択）

- **対象**: `Panel2`（GroupBox2内）の RadioButton グループ排他制御
- **関連要件**: US-003, FR-002
- **種別**: 正常系
- **前提条件**: フォームインスタンス化後、`オプション487.Checked = True` にしてから `オプション489.Checked = True` をセット
- **入力**: `オプション489.Checked = True`
- **期待結果**: `オプション489.Checked = True`、`オプション487.Checked = False`

---

### TC-011: Taisho変換ロジック（リース料選択 → Taisho=1）

- **対象**: `Form_f_KLSRYO_JOKEN.GetTaisho()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-001, FR-004
- **種別**: 正常系
- **前提条件**: `オプション504.Checked = True`（リース料）
- **入力**: `オプション504.Checked = True`、`オプション506.Checked = False`、`オプション508.Checked = False`
- **期待結果**: Taisho = 1（`If(オプション504.Checked, 1, If(オプション506.Checked, 2, 3))` の評価結果）

---

### TC-012: Taisho変換ロジック（保守料選択 → Taisho=2）

- **対象**: `Form_f_KLSRYO_JOKEN.GetTaisho()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-001, FR-004
- **種別**: 正常系
- **前提条件**: `オプション506.Checked = True`（保守料）
- **入力**: `オプション504.Checked = False`、`オプション506.Checked = True`、`オプション508.Checked = False`
- **期待結果**: Taisho = 2

---

### TC-013: Taisho変換ロジック（全部選択 → Taisho=3）

- **対象**: `Form_f_KLSRYO_JOKEN.GetTaisho()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-001, FR-004
- **種別**: 正常系
- **前提条件**: `オプション508.Checked = True`（全部）
- **入力**: `オプション504.Checked = False`、`オプション506.Checked = False`、`オプション508.Checked = True`
- **期待結果**: Taisho = 3

---

### TC-014: Ktmg変換ロジック（締日ベース → ShriKtmg.SimeDtBase）

- **対象**: `Form_f_KLSRYO_JOKEN.GetKtmg()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-002, FR-004
- **種別**: 正常系
- **前提条件**: `chk_SHIME.Checked = True`（締日ベース）
- **入力**: `chk_SHIME.Checked = True`、`オプション483.Checked = False`
- **期待結果**: Ktmg = `ShriKtmg.SimeDtBase`（値=1）

---

### TC-015: Ktmg変換ロジック（支払日ベース → ShriKtmg.ShriDtBase）

- **対象**: `Form_f_KLSRYO_JOKEN.GetKtmg()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-002, FR-004
- **種別**: 正常系
- **前提条件**: `オプション483.Checked = True`（支払日ベース）
- **入力**: `chk_SHIME.Checked = False`、`オプション483.Checked = True`
- **期待結果**: Ktmg = `ShriKtmg.ShriDtBase`（値=2）

---

### TC-016: Meisai変換ロジック（物件単位 → ShriMeisai.Kykm）

- **対象**: `Form_f_KLSRYO_JOKEN.GetMeisai()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-003, FR-004
- **種別**: 正常系
- **前提条件**: `オプション487.Checked = True`（物件単位）
- **入力**: `オプション487.Checked = True`、`オプション489.Checked = False`
- **期待結果**: Meisai = `ShriMeisai.Kykm`

---

### TC-017: Meisai変換ロジック（配賦単位 → ShriMeisai.Haif）

- **対象**: `Form_f_KLSRYO_JOKEN.GetMeisai()` または `cmd_EXECUTE_Click` の変換ロジック
- **関連要件**: US-003, FR-004
- **種別**: 正常系
- **前提条件**: `オプション489.Checked = True`（配賦単位）
- **入力**: `オプション487.Checked = False`、`オプション489.Checked = True`
- **期待結果**: Meisai = `ShriMeisai.Haif`

---

### TC-018: エンジン実行 Taisho=1（リース料のみ）付随費用処理スキップ確認

- **対象**: `KlsryoCalculationEngine.Execute` の `taisho=1` 時の分岐
- **関連要件**: US-001, FR-004
- **種別**: 正常系（境界値）
- **前提条件**: `taisho=1`、DB接続可能な環境
- **入力**:
  - `taisho = 1`（リース料のみ）
  - `ktmg = ShriKtmg.SimeDtBase`
  - `meisai = ShriMeisai.Haif`
  - `dtFrom = 2024/04/01`、`dtTo = 2024/04/30`
- **期待結果**: 実行が成功（例外なし）し、付随費用処理（`ProcessHenf`）が呼ばれない（`taisho <> 2 AndAlso taisho <> 3` のため）。DB未接続の場合は SKIP。
- **注意**: `KlsryoCalculationEngine.Execute:87` にある `If taisho = 2 OrElse taisho = 3 Then ProcessHenf(...)` の条件をテストする

---

### TC-019: エンジン実行 Access版デフォルト条件（Taisho=3 + SimeDtBase + Haif）

- **対象**: `KlsryoCalculationEngine.Execute` の全パラメータ組み合わせ
- **関連要件**: US-001〜US-003, FR-004
- **種別**: 正常系（回帰）
- **前提条件**: DB接続可能な環境
- **入力**:
  - `taisho = 3`（全部）
  - `ktmg = ShriKtmg.SimeDtBase`（締日ベース）
  - `meisai = ShriMeisai.Haif`（配賦単位）
  - `dtFrom = 2024/04/01`、`dtTo = 2024/04/30`
- **期待結果**: 結果リストが `Nothing` でなく、0件以上が返される。DB未接続の場合は SKIP。

---

### TC-020: エンジン実行 Taisho=3 + ShriDtBase + Kykm

- **対象**: `KlsryoCalculationEngine.Execute` のパラメータ組み合わせ
- **関連要件**: US-001〜US-003, FR-004
- **種別**: 正常系
- **前提条件**: DB接続可能な環境
- **入力**:
  - `taisho = 3`（全部）
  - `ktmg = ShriKtmg.ShriDtBase`（支払日ベース）
  - `meisai = ShriMeisai.Kykm`（物件単位）
  - `dtFrom = 2024/04/01`、`dtTo = 2024/04/30`
- **期待結果**: 実行が成功し、結果リストが `Nothing` でない。DB未接続の場合は SKIP。

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| 標準集計期間 FROM | 2024/04/01 | エンジン統合テストの期間開始日 |
| 標準集計期間 TO | 2024/04/30 | エンジン統合テストの期間終了日 |
| Taisho=1（リース料） | 整数値 1 | Access版 `opg_TAISHO=1` に対応 |
| Taisho=2（保守料） | 整数値 2 | Access版 `opg_TAISHO=2` に対応 |
| Taisho=3（全部） | 整数値 3 | Access版 `opg_TAISHO=3` に対応、デフォルト |

### 異常データ

| データ名 | 値 | 期待エラー/動作 |
|---|---|---|
| 全ラジオボタン False（コントロール未初期化） | Checked=False（3グループ全て） | デフォルト値ロジックによる Taisho=3、Ktmg=SimeDtBase、Meisai=Haif が設定される（If の else 側が評価される） |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| Taisho=1（リース料のみ） | taisho=1 | `If taisho = 2 OrElse taisho = 3` 条件が False になり付随費用スキップ |
| Taisho=2（保守料のみ） | taisho=2 | 付随費用処理が実行される |
| chk_SHIME（締日ベース）と Designer デフォルトの不整合 | chk_SHIME.Checked=True | 実装前はオプション483=Trueがデザイナーデフォルト。実装後はchk_SHIMEがデフォルト |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_klsryo_joken_options_blackbox.vb` | デフォルト値確認、排他制御、Taisho/Ktmg/Meisai変換ロジック、エンジン統合 | 20件 |

### テストモジュール構成

```vb
' コンパイル: vbc /r:LeaseM4BS.TestWinForms.exe /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll test_klsryo_joken_options_blackbox.vb
' 実行: test_klsryo_joken_options_blackbox.exe

Module TestKlsryoJokenOptionsBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    <STAThread>
    Sub Main()
        Console.OutputEncoding = System.Text.Encoding.UTF8
        Console.WriteLine("=== f_KLSRYO_JOKEN ラジオボタンオプション ブラックボックステスト ===")
        Console.WriteLine()

        ' ---- Part 1: デフォルト値確認 ----
        Console.WriteLine("--- Part 1: デフォルト値確認 ---")
        Test_TC001_Default_Taisho_Zenbu()      ' TC-001
        Test_TC002_Default_Ktmg_SimeDt()       ' TC-002
        Test_TC003_Default_Meisai_Haif()       ' TC-003
        Console.WriteLine()

        ' ---- Part 2: 集計対象グループ 排他制御 ----
        Console.WriteLine("--- Part 2: 集計対象グループ 排他制御 ---")
        Test_TC004_Taisho_Lsryo_Exclusive()    ' TC-004
        Test_TC005_Taisho_Hoshu_Exclusive()    ' TC-005
        Test_TC006_Taisho_Zenbu_Exclusive()    ' TC-006
        Console.WriteLine()

        ' ---- Part 3: タイミンググループ 排他制御 ----
        Console.WriteLine("--- Part 3: タイミンググループ 排他制御 ---")
        Test_TC007_Ktmg_Sime_Exclusive()       ' TC-007
        Test_TC008_Ktmg_Shri_Exclusive()       ' TC-008
        Console.WriteLine()

        ' ---- Part 4: 明細グループ 排他制御 ----
        Console.WriteLine("--- Part 4: 明細グループ 排他制御 ---")
        Test_TC009_Meisai_Bukn_Exclusive()     ' TC-009
        Test_TC010_Meisai_Haif_Exclusive()     ' TC-010
        Console.WriteLine()

        ' ---- Part 5: Taisho変換ロジック ----
        Console.WriteLine("--- Part 5: Taisho変換ロジック ---")
        Test_TC011_Taisho_Lsryo_Returns1()     ' TC-011
        Test_TC012_Taisho_Hoshu_Returns2()     ' TC-012
        Test_TC013_Taisho_Zenbu_Returns3()     ' TC-013
        Console.WriteLine()

        ' ---- Part 6: Ktmg変換ロジック ----
        Console.WriteLine("--- Part 6: Ktmg変換ロジック ---")
        Test_TC014_Ktmg_Sime_ReturnsSimeDtBase()   ' TC-014
        Test_TC015_Ktmg_Shri_ReturnsShriDtBase()   ' TC-015
        Console.WriteLine()

        ' ---- Part 7: Meisai変換ロジック ----
        Console.WriteLine("--- Part 7: Meisai変換ロジック ---")
        Test_TC016_Meisai_Bukn_ReturnsKykm()   ' TC-016
        Test_TC017_Meisai_Haif_ReturnsHaif()   ' TC-017
        Console.WriteLine()

        ' ---- Part 8: エンジン統合テスト ----
        Console.WriteLine("--- Part 8: エンジン統合テスト (DBあり/SKIP可) ---")
        Test_TC018_Engine_Taisho1_NoHenf()     ' TC-018
        Test_TC019_Engine_Default_Combo()      ' TC-019
        Test_TC020_Engine_ShriDt_Kykm()        ' TC-020
        Console.WriteLine()

        ' ---- 結果集計 ----
        Console.WriteLine($"=== 結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount} ===")
        If failCount > 0 Then
            Environment.Exit(1)
        End If
    End Sub

    ' ヘルパー
    Sub Pass(label As String)
        passCount += 1
        Console.WriteLine($"  PASS: {label}")
    End Sub

    Sub Fail(label As String, expected As String, actual As String)
        failCount += 1
        Console.WriteLine($"  FAIL: {label}")
        Console.WriteLine($"    Expected: {expected}")
        Console.WriteLine($"    Actual:   {actual}")
    End Sub

    Sub Skip(label As String, reason As String)
        skipCount += 1
        Console.WriteLine($"  SKIP: {label} ({reason})")
    End Sub

    Sub AssertTrue(label As String, condition As Boolean)
        If condition Then Pass(label) Else Fail(label, "True", "False")
    End Sub

    Sub AssertFalse(label As String, condition As Boolean)
        If Not condition Then Pass(label) Else Fail(label, "False", "True")
    End Sub

    Sub AssertEqual(Of T)(label As String, expected As T, actual As T)
        If Object.Equals(expected, actual) Then
            Pass(label)
        Else
            Fail(label, expected.ToString(), actual.ToString())
        End If
    End Sub

End Module
```

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

| パターン | 参照元 | 本テストでの適用 |
|---|---|---|
| `passCount`/`failCount`/`skipCount` カウンタ | `test_chuki_idolst_joken_blackbox.vb` | 同形式で採用 |
| `Sub Test_XXX()` 形式（スネークケース + 機能名） | `test_closing_day_correction_blackbox.vb` | 同形式で採用 |
| `Try/Catch` で FAIL 捕捉 | `test_keijo_joken_blackbox.vb` | 各テスト関数内で採用 |
| `ExitCode=1`（FAIL時） | `test_chuki_idolst_joken_blackbox.vb` | `Environment.Exit(1)` で採用 |
| `SKIP` パターン（DB未接続時） | `test_schedule_blackbox.vb` | エンジン統合テスト（TC-018〜TC-020）に適用 |
| `Console.OutputEncoding = UTF8` | 全既存ファイル | 先頭で設定 |
| `<STAThread>` 属性 + WinForms インスタンス化 | `test_chuki_idolst_joken_blackbox.vb` | `Main()` に付与。RadioButton の排他制御は UI スレッドで動作するため必須 |

### WinForms 依存コントロールのテスト方式

`Form_f_KLSRYO_JOKEN` の RadioButton は WinForms コントロールであるため、以下の方式でテストする。

1. `New Form_f_KLSRYO_JOKEN()` でフォームをインスタンス化する
2. `frm.オプション504.Checked = True` のようにコントロールのプロパティを直接設定する
3. 同一 Panel 内の RadioButton が自動で排他制御されることを `Checked` プロパティで確認する
4. 変換ロジックは `frm.cmd_EXECUTE_Click` を直接呼べないため、`GetTaisho()` / `GetKtmg()` / `GetMeisai()` を `Friend` に変更したメソッドを呼び出すか、または変換ロジックをテスト内でインライン再実装して検証する

### モック/スタブの使い方

- DB 接続なしのテスト（TC-001〜TC-017）: `Form_f_KLSRYO_JOKEN` のみインスタンス化、DB接続コードは呼ばれない
- エンジン統合テスト（TC-018〜TC-020）: `KlsryoCalculationEngine.Execute()` を実行。DB接続エラー時は `Try/Catch` で捕捉し `Skip()` を呼ぶ

### テスト変換ロジックの実装方針

`cmd_EXECUTE_Click` 内の変換ロジックをテストするため、`Form_f_KLSRYO_JOKEN` に以下の `Friend` メソッドを追加することを前提とする（実装時に対応）。

```vb
Friend Function GetTaisho() As Integer
    Return If(オプション504.Checked, 1, If(オプション506.Checked, 2, 3))
End Function

Friend Function GetKtmg() As ShriKtmg
    Return If(chk_SHIME.Checked, ShriKtmg.SimeDtBase, ShriKtmg.ShriDtBase)
End Function

Friend Function GetMeisai() As ShriMeisai
    Return If(オプション487.Checked, ShriMeisai.Kykm, ShriMeisai.Haif)
End Function
```

これらメソッドを公開できない場合は、テスト内で同等のロジックをインライン実装してコントロールの `Checked` 状態から期待値を算出し検証する。

---

## 8. 既知の問題と SKIP 基準

| 問題 | 対応 |
|---|---|
| Designer.vb の `オプション483`（支払日ベース）が `Checked=True`（TC-002 の課題） | FR-003 の実装で `chk_SHIME.Checked=True` / `オプション483.Checked=False` に修正されることを前提として PASS/FAIL を記録。実装前の状態で実行すると TC-002 が FAIL になることを想定し、その場合 SKIP ではなく FAIL として ExitCode=1 に寄与させる |
| `コントロール名が日本語`（`オプション483`等） | テストコードでも日本語コントロール名を使用する。VB.NETは識別子に全角文字を許容するため技術的問題なし |
| WinForms STA スレッド要件 | `Main()` に `<STAThread>` 属性を付与。WinForms インスタンス化失敗時は SKIP として記録 |
| エンジン統合テストの DB接続失敗 | `ex.Message.Contains("does not exist")` または接続文字列未設定を判定して SKIP |
