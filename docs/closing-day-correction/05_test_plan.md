# テスト計画書: closing-day-correction

## 1. テスト戦略
- **テストフレームワーク**: Console.WriteLineベースのカスタムアサーション（既存ブラックボックステストと同一パターン）
- **テストレベル**: 単体テスト（DBなし）+ 結合テスト（DBあり・スキップ可）
- **カバレッジ目標**: 締日=31（回帰）/ 締日=20,15,1（標準非月末）/ 締日=28,30（2月問題）の全パスをカバー
- **モック戦略**: `KlsryoCalculationEngine` の `GetShimebi()` はDBアクセスを伴うため、単体テストでは `shimeBi` をパラメータとして直接渡すか、テスト用ダミーサブクラスを利用する。DB接続テストはSkipパターンで保護する。

---

## 2. テスト環境
- **テストファイル**: `c:\kobayashi_LeaseM4BS\test_closing_day_correction_blackbox.vb`
- **コンパイル**: `vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_closing_day_correction_blackbox.vb`
- **実行**: `test_closing_day_correction_blackbox.exe`
- **必要なDLL**: `LeaseM4BS.DataAccess.dll`（既存のビルド成果物）
- **テストデータ**: コード内にインラインで定義（DBデータ不要）
- **外部依存のモック**: DB接続テストは `Try/Catch` + `Skip()` パターン（`test_schedule_blackbox.vb:Test_Integration_MonthlyJournal` と同一）

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | kishuDt/kimatDt 算出ロジック（締日=31 回帰） | 高 | US-001, FR-005 |
| T-002 | kishuDt/kimatDt 算出ロジック（締日=20） | 高 | US-001, FR-002, FR-003 |
| T-003 | kishuDt/kimatDt 算出ロジック（締日=15） | 高 | US-001, FR-002, FR-003 |
| T-004 | kishuDt/kimatDt 算出ロジック（締日=1） | 中 | US-001, FR-002, FR-003 |
| T-005 | kishuDt/kimatDt 算出ロジック（締日=28, 2月問題） | 高 | US-001, FR-002, FR-003 |
| T-006 | kishuDt/kimatDt 算出ロジック（締日=30, 2月切り下げ） | 高 | US-001, FR-002, FR-003 |
| T-007 | getudoFrom/getudoTo 月度配列（締日=31 回帰） | 高 | US-002, FR-004, FR-005 |
| T-008 | getudoFrom/getudoTo 月度配列（締日=20） | 高 | US-002, FR-004 |
| T-009 | getudoFrom/getudoTo 月度配列（締日=15） | 中 | US-002, FR-004 |
| T-010 | ynKimatDt 翌期年度末（締日=31 回帰） | 中 | US-003, FR-005 |
| T-011 | ynKimatDt 翌期年度末（締日=20） | 中 | US-003 |
| T-012 | GetShimeBiデフォルト値（DBエラー時 → 31） | 高 | US-004, FR-001 |
| T-013 | kishuDt/kimatDt 月境界（締日=31, dtFrom=2月） | 中 | US-001, FR-002 |
| T-014 | kishuDt/kimatDt 月境界（締日=30, dtTo=2月） | 高 | US-001, FR-003 |

---

## 4. テストケース

---

### TC-001: 締日=31（月末締め）期首/期末日 回帰テスト

- **対象**: `KlsryoCalculationEngine.Execute` 内 kishuDt / kimatDt 算出（締日=31）
- **関連要件**: US-001, FR-005
- **種別**: 正常系（回帰）
- **前提条件**: shimeBi = 31（デフォルト）
- **入力**:
  - `dtFrom = 2024/04/01`
  - `dtTo = 2025/03/31`
  - shimeBi = 31
- **期待結果**:
  - `kishuDt = 2024/04/01`（月初のまま）
  - `kimatDt = 2025/03/31`（月末のまま）

---

### TC-002: 締日=20 期首/期末日補正テスト

- **対象**: `KlsryoCalculationEngine.Execute` 内 kishuDt / kimatDt 算出（締日=20）
- **関連要件**: US-001, FR-002, FR-003
- **種別**: 正常系
- **前提条件**: shimeBi = 20
- **入力**:
  - `dtFrom = 2024/04/01`（初期kishuDt = 2024/04/01）
  - `dtTo = 2025/03/31`（初期kimatDt = 2025/03/31）
  - shimeBi = 20
- **期待結果**:
  - `kishuDt = 2024/03/21`（前月20日 + 1日 = 3月21日）
  - `kimatDt = 2025/03/20`（当月dtTo年月に締日20を適用）
- **補足**: Access版算出式 `DateAdd("d",1, CDate("2024/03/" & 20))` = 2024/03/21

---

### TC-003: 締日=15 期首/期末日補正テスト

- **対象**: `KlsryoCalculationEngine.Execute` 内 kishuDt / kimatDt 算出（締日=15）
- **関連要件**: US-001, FR-002, FR-003
- **種別**: 正常系
- **前提条件**: shimeBi = 15
- **入力**:
  - `dtFrom = 2024/04/01`
  - `dtTo = 2025/03/31`
  - shimeBi = 15
- **期待結果**:
  - `kishuDt = 2024/03/16`（前月15日 + 1日 = 3月16日）
  - `kimatDt = 2025/03/15`（dtTo年月に締日15を適用）

---

### TC-004: 締日=1 境界値テスト（最小値）

- **対象**: `KlsryoCalculationEngine.Execute` 内 kishuDt / kimatDt 算出（締日=1）
- **関連要件**: US-001, FR-002, FR-003
- **種別**: 境界値
- **前提条件**: shimeBi = 1
- **入力**:
  - `dtFrom = 2024/04/01`
  - `dtTo = 2025/03/31`
  - shimeBi = 1
- **期待結果**:
  - `kishuDt = 2024/03/02`（前月1日 + 1日 = 3月2日）
  - `kimatDt = 2025/03/01`（dtTo年月に締日1を適用）

---

### TC-005: 締日=28, dtFrom=2月 期首日補正（2月問題）

- **対象**: kishuDt 算出（締日=28, 前月=1月）
- **関連要件**: US-001, FR-002
- **種別**: 境界値
- **前提条件**: shimeBi = 28
- **入力**:
  - `dtFrom = 2024/02/01`（dtFrom月 = 2月）
  - `dtTo = 2024/02/29`（閏年）
  - shimeBi = 28
- **期待結果**:
  - `kishuDt = 2024/01/29`（前月=1月の28日 + 1日 = 1月29日）
  - `kimatDt = 2024/02/28`（dtTo年月2月 → 28日: `Math.Min(28, 29)` = 28）

---

### TC-006: 締日=30, dtTo=2月 期末日切り下げテスト

- **対象**: kimatDt 算出（締日=30, dtTo月=2月）
- **関連要件**: US-001, FR-003
- **種別**: 境界値（月内日数超過の切り下げ）
- **前提条件**: shimeBi = 30
- **入力**:
  - `dtFrom = 2024/02/01`
  - `dtTo = 2024/02/29`（閏年2月末）
  - shimeBi = 30
- **期待結果**:
  - `kimatDt = 2024/02/29`（閏年: `Math.Min(30, 29)` = 29 → 2024/02/29）
- **補足**: 非閏年なら `Math.Min(30, 28)` = 28 → 2月28日

---

### TC-007: 締日=30, dtTo=2月（非閏年）期末日切り下げ

- **対象**: kimatDt 算出（締日=30, dtTo月=2月非閏年）
- **関連要件**: US-001, FR-003
- **種別**: 境界値
- **前提条件**: shimeBi = 30
- **入力**:
  - `dtFrom = 2023/02/01`
  - `dtTo = 2023/02/28`（非閏年2月末）
  - shimeBi = 30
- **期待結果**:
  - `kimatDt = 2023/02/28`（非閏年: `Math.Min(30, 28)` = 28 → 2023/02/28）

---

### TC-008: 締日=31 月度配列 回帰テスト

- **対象**: getudoFrom / getudoTo 配列（締日=31）
- **関連要件**: US-002, FR-004, FR-005
- **種別**: 正常系（回帰）
- **前提条件**: shimeBi = 31, kishuDt = 2024/04/01
- **入力**:
  - `dtFrom = 2024/04/01`, `dtTo = 2025/03/31`, shimeBi = 31
- **期待結果**:
  - `getudoFrom(0) = 2024/04/01`
  - `getudoFrom(1) = 2024/05/01`
  - `getudoTo(0) = 2024/04/30`（getudoFrom(1).AddDays(-1)）
  - `getudoTo(11) = 2025/03/31`

---

### TC-009: 締日=20 月度配列補正テスト

- **対象**: getudoFrom / getudoTo 配列（締日=20、kishuDt補正後）
- **関連要件**: US-002, FR-004
- **種別**: 正常系
- **前提条件**: shimeBi = 20, kishuDt補正後 = 2024/03/21
- **入力**:
  - `dtFrom = 2024/04/01`, `dtTo = 2025/03/31`, shimeBi = 20
- **期待結果**:
  - `getudoFrom(0) = 2024/03/21`（補正後kishuDt）
  - `getudoFrom(1) = 2024/04/21`（AddMonths(1)）
  - `getudoTo(0) = 2024/04/20`（getudoFrom(1).AddDays(-1) = 4/21 - 1日 = 4/20）
  - `getudoTo(1) = 2024/05/20`
  - `getudoFrom(12) = 2025/03/21`

---

### TC-010: 締日=15 月度配列補正テスト

- **対象**: getudoFrom / getudoTo 配列（締日=15）
- **関連要件**: US-002, FR-004
- **種別**: 正常系
- **前提条件**: shimeBi = 15, kishuDt補正後 = 2024/03/16
- **入力**:
  - `dtFrom = 2024/04/01`, `dtTo = 2025/03/31`, shimeBi = 15
- **期待結果**:
  - `getudoFrom(0) = 2024/03/16`
  - `getudoTo(0) = 2024/04/15`（getudoFrom(1).AddDays(-1) = 4/16 - 1日 = 4/15）

---

### TC-011: 締日=31 ynKimatDt 回帰テスト

- **対象**: ynKimatDt(0)〜ynKimatDt(4)（締日=31）
- **関連要件**: US-003, FR-005
- **種別**: 正常系（回帰）
- **前提条件**: shimeBi = 31, kimatDt = 2025/03/31
- **入力**:
  - `dtFrom = 2024/04/01`, `dtTo = 2025/03/31`, shimeBi = 31
- **期待結果**:
  - `ynKimatDt(0) = 2026/03/31`（kimatDt + 12M = 月末）
  - `ynKimatDt(1) = 2027/03/31`
  - `ynKimatDt(4) = 2030/03/31`

---

### TC-012: 締日=20 ynKimatDt 補正テスト

- **対象**: ynKimatDt(0)（締日=20、補正後kimatDt基点）
- **関連要件**: US-003
- **種別**: 正常系
- **前提条件**: shimeBi = 20, kimatDt補正後 = 2025/03/20
- **入力**:
  - `dtFrom = 2024/04/01`, `dtTo = 2025/03/31`, shimeBi = 20
- **期待結果（実装方針による）**:
  - 月末ベース維持の場合: `ynKimatDt(0) = 2026/03/31`（既存 `GetMonthEndDate` を流用）
  - 締日ベースに変更した場合: `ynKimatDt(0) = 2026/03/20`（締日=20を翌年同月に適用）
  - **注記**: 要件書US-003仮定事項5に従い実装を確認してからアサーション値を確定する

---

### TC-013: GetShimeBi デフォルト値テスト（DBエラー時）

- **対象**: `GetShimeBi()` のフォールバック動作
- **関連要件**: US-004, FR-001, NFR-002
- **種別**: 異常系
- **前提条件**: `t_settei` テーブルに `SHIMEBI` レコードが存在しない、またはDB接続不可
- **入力**: DB接続失敗状況（テストでは Skip パターンを適用）
- **期待結果**: shimeBi = 31（デフォルト値にフォールバック）、エラーログが記録される
- **テスト実装方法**: DB接続テストとして実行し、接続不可時は `Skip()` で保護

---

### TC-014: 締日=20, dtFrom=1月（前月=12月）期首日補正テスト

- **対象**: kishuDt 算出（年跨ぎ）
- **関連要件**: US-001, FR-002
- **種別**: 正常系（年跨ぎケース）
- **前提条件**: shimeBi = 20
- **入力**:
  - `dtFrom = 2025/01/01`（初期kishuDt = 2025/01/01）
  - `dtTo = 2025/12/31`
  - shimeBi = 20
- **期待結果**:
  - `kishuDt = 2024/12/21`（前月=2024/12の20日 + 1日 = 2024/12/21）
  - `kimatDt = 2025/12/20`

---

### TC-015: 締日=28, dtFrom=3月（前月=2月）期首日 閏年テスト

- **対象**: kishuDt 算出（締日=28, 前月=閏年2月）
- **関連要件**: US-001, FR-002
- **種別**: 境界値（閏年2月）
- **前提条件**: shimeBi = 28, 閏年
- **入力**:
  - `dtFrom = 2024/03/01`（初期kishuDt = 2024/03/01）
  - `dtTo = 2024/03/31`
  - shimeBi = 28
- **期待結果**:
  - `kishuDt = 2024/02/29`（前月=2024/02の28日 + 1日 = 2024/02/29: 閏年なので有効日）
  - `kimatDt = 2024/03/28`

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| 標準期間（4月〜3月末） | dtFrom=2024/04/01, dtTo=2025/03/31 | TC-001〜003, TC-008〜011 |
| 標準期間（1月〜12月末） | dtFrom=2025/01/01, dtTo=2025/12/31 | TC-014（年跨ぎ） |
| 締日20 | shimeBi=20 | TC-002, TC-009, TC-012, TC-014 |
| 締日15 | shimeBi=15 | TC-003, TC-010 |
| 締日1 | shimeBi=1 | TC-004 |

### 異常データ

| データ名 | 値 | 期待エラー/動作 |
|---|---|---|
| DBエラー時の締日取得 | t_settei接続失敗 | デフォルト値31にフォールバック |
| 締日30で2月（閏年） | shimeBi=30, dtTo=2024/02/29 | kimatDt=2024/02/29（29日に切り下げ） |
| 締日30で2月（非閏年） | shimeBi=30, dtTo=2023/02/28 | kimatDt=2023/02/28（28日に切り下げ） |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| 締日=1（最小値） | shimeBi=1 | 最小締日での期首/期末算出 |
| 締日=28 | shimeBi=28 | 2月境界での自然な締日 |
| 締日=30, 2月（閏年） | shimeBi=30, month=2, 閏年 | Math.Min(30,29)=29 切り下げ |
| 締日=30, 2月（非閏年） | shimeBi=30, month=2, 非閏年 | Math.Min(30,28)=28 切り下げ |
| 前月=12月（年跨ぎ） | dtFrom=2025/01/01, shimeBi=20 | 年をまたぐAddMonths(-1)の動作 |
| 前月=閏年2月 | dtFrom=2024/03/01, shimeBi=28 | 2024/02/28 + 1日 = 2024/02/29 |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_closing_day_correction_blackbox.vb` | KlsryoCalculationEngine 締日補正ロジック | 15 |

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

- **Moduleトップレベル**: `Module TestClosingDayCorrectionBlackBox` として定義（`test_schedule_blackbox.vb` と同一構造）
- **カウンタ変数**: `passCount`, `failCount`, `skipCount` をモジュールレベルで宣言
- **サマリ出力**: `=== 結果: PASS={passCount}, FAIL={failCount}, SKIP={skipCount} ===`
- **ExitCode制御**: failCount > 0 のとき `Environment.ExitCode = 1`

### アサーションヘルパー

既存の `test_schedule_blackbox.vb` と同一のヘルパーをコピーして使用する:

```vb
Sub AssertEqual(label As String, expected As Date, actual As Date)
    Console.Write($"  {label} ... ")
    If expected = actual Then
        Pass(label)
    Else
        Fail(label, expected.ToString("yyyy/MM/dd"), actual.ToString("yyyy/MM/dd"))
    End If
End Sub

Sub AssertEqual(label As String, expected As Integer, actual As Integer)
    ' 既存パターンと同一
End Sub

Sub Pass(label As String)
    Console.WriteLine("PASS")
    passCount += 1
End Sub

Sub Fail(label As String, expected As String, actual As String)
    Console.WriteLine($"FAIL (expected={expected}, actual={actual})")
    failCount += 1
End Sub

Sub Skip(label As String)
    Console.WriteLine($"SKIP ({label})")
    skipCount += 1
End Sub
```

### 日付補正ロジックの直接テスト方法

`KlsryoCalculationEngine.Execute` 全体を呼び出すとDB接続が必要になるため、以下のいずれかの方法で単体テストを実現する:

1. **推奨**: 補正ロジックをテスト用にPublicなSharedメソッド（例: `KlsryoCalculationEngine.CalcKishuDt`, `CalcKimatDt`）として切り出し、テストから直接呼び出す
2. **代替**: テスト専用の入力値で `Execute` を呼び出し、結果DataTableのメタデータ（列値）から間接的に期首/期末日を検証する（DB接続が必要な場合はSkip）
3. **最小変更案**: テストファイル内にAccess版算出式を再実装したヘルパー関数を定義し、VB.NET実装と比較検証する

### DB接続テストの保護パターン

```vb
Sub Test_GetShimeBi_DbError()
    Dim label As String = "GetShimeBi: DBエラー → デフォルト31"
    Try
        ' DB接続が必要な実装をテスト
        ' ...
    Catch ex As Exception
        Dim msg As String = ex.Message
        If msg.Contains("Connection") OrElse msg.Contains("接続") OrElse msg.Contains("refused") Then
            Skip(label & " (DB接続不可)")
        ElseIf msg.Contains("は存在しません") OrElse msg.Contains("does not exist") Then
            Skip(label & " (DBスキーマ未完了)")
        Else
            Fail(label, "shimeBi=31", $"Exception: {msg}")
        End If
    End Try
End Sub
```

### モック/スタブの使い方

- DB依存の `GetShimeBi()` はテスト時に `shimeBi` パラメータとして直接渡せるよう、実装時に `Execute(dtFrom, dtTo, taisho, ktmg, meisai, Optional shimeBi As Integer = -1)` のシグネチャを選択することを推奨する（-1 の場合のみ `GetShimeBi()` をDB呼び出し）

---

## 8. テスト実行順序

```
Part 1: 期首/期末日 単体補正テスト
  TC-001: 締日=31 回帰
  TC-002: 締日=20 標準
  TC-003: 締日=15 標準
  TC-004: 締日=1 境界値（最小）
  TC-005: 締日=28, 2月（閏年）
  TC-006: 締日=30, 2月（閏年）切り下げ
  TC-007: 締日=30, 2月（非閏年）切り下げ
  TC-014: 締日=20, 年跨ぎ（1月→前年12月）
  TC-015: 締日=28, 閏年前月=2月

Part 2: 月度配列テスト
  TC-008: 締日=31 月度配列 回帰
  TC-009: 締日=20 月度配列
  TC-010: 締日=15 月度配列

Part 3: 翌期年度末テスト
  TC-011: 締日=31 ynKimatDt 回帰
  TC-012: 締日=20 ynKimatDt

Part 4: DB接続テスト（Skipパターン保護）
  TC-013: GetShimeBi DBエラー → デフォルト31
```
