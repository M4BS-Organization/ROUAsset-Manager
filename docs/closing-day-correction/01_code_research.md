# コードベース調査資料: closing-day-correction

## 1. プロジェクト概要
- フレームワーク: .NET Framework 4.7.2 / VB.NET / WinForms
- ソリューション: `LeaseM4BS/LeaseM4BS.slnx`
- DBアクセス: PostgreSQL (Npgsql 6.0.11) / `LeaseM4BS.DataAccess` プロジェクト

## 2. アーキテクチャ概要
- 計算エンジン層 (`LeaseM4BS.DataAccess`) + UIフォーム層 (`LeaseM4BS.TestWinForms`)
- 計算エンジンはすべてStatefulクラス（`_crud As CrudHelper` を保持）
- Access版 `pc_SHRI_KLSRYO` → `KlsryoCalculationEngine.vb` に対応

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` | 期間リース料計算エンジン本体（対象） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoTypes.vb` | 型定義（KlsryoResult, CashScheduleEntry 等） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/CashScheduleBuilder.vb` | キャッシュスケジュール生成・締日計算 | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/ScheduleTypes.vb` | `ChukiCalcParams.KessanBi` 定義 | 中 |
| `LeaseM4BS/LeaseM4BS.DataAccess/ChukiCalcEngine.vb` | 注記計算エンジン（締日非31対応済みの参考実装） | 中 |
| `LeaseM4BS/LeaseM4BS.DataAccess/SetteiHelper.vb` | `t_settei` テーブルアクセス（設定値取得） | 中 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_KLSRYO_JOKEN.vb` | 条件入力フォーム（`Execute`の呼び出し元） | 中 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_flx_KLSRYO.vb` | 一覧表示フォーム（`Execute`の直接呼び出し） | 中 |

## 4. 現行実装の詳細分析

### KlsryoCalculationEngine.Execute の期首/期末日算出（対象コード）

`KlsryoCalculationEngine.vb:22-24`:
```vb
' *** 期首日/期末日の算出 (月末締め前提: ig締日=31)
Dim kishuDt As Date = New Date(dtFrom.Year, dtFrom.Month, 1)
Dim kimatDt As Date = CashScheduleBuilder.GetMonthEndDate(dtTo)
```

**問題点**: 期首日を常に月初1日、期末日を月末日に固定している。締日が31以外の場合の補正ロジックが欠落している。

### 月度配列(getudoFrom/getudoTo)の算出（KlsryoCalculationEngine.vb:41-48）

```vb
Dim getudoFrom(12) As Date
Dim getudoTo(11) As Date
For i As Integer = 0 To 12
    getudoFrom(i) = kishuDt.AddMonths(i)
Next
For i As Integer = 0 To 11
    getudoTo(i) = getudoFrom(i + 1).AddDays(-1)
Next
```

kishuDtをベースにAddMonthsで計算しているため、kishuDtの補正後にはこの算出も自動的に正しく動く。
ただし、締日非31の場合は `getudoTo(i) = getudoFrom(i+1).AddDays(-1)` では正確に `締日` にならない（締日の1日前ではなく当月締日になるべき）。

### 翌期年度末(ynKimatDt)の算出（KlsryoCalculationEngine.vb:27-38）

```vb
wk = CashScheduleBuilder.GetMonthEndDate(wk)
wk = wk.AddMonths(12)
ynKimatDt(i) = CashScheduleBuilder.GetMonthEndDate(wk)
```

`GetMonthEndDate` を使用しているため月末固定。締日非31の場合は締日を使った計算に変更が必要。

### CalcSimeDtB での締日コメント（CashScheduleBuilder.vb:315）

```vb
' ig締日=31 (月末締め) を前提とする
```

明示的に月末前提と記載されており、締日非31対応は未実装。

### GetMonthEndDate（CashScheduleBuilder.vb:344-346）

```vb
Public Shared Function GetMonthEndDate(dt As Date) As Date
    Return New Date(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month))
End Function
```

月末日を返す汎用関数。締日補正後の期末日計算には `Math.Min(shimeBi, DateTime.DaysInMonth(...))` が必要。

## 5. Access版との差異（主要）

### Access版 p_PublicVariable.txt:672

```vba
Public Const ig締日 = 31
```

Access版では `ig締日` はグローバル定数として定義されており、**常に31に固定されている**。
ただし `pc_SHRI_KLSRYO.txt:147-152` では `If ig締日 <> 31 Then` の補正ブロックが存在する。

### Access版 gKLSRYO_Main 補正ロジック（pc_SHRI_KLSRYO.txt:145-152）

```vba
dte_lKISHU_DT = CDate(Format(dte_aKIKAN_FROM, "yyyy/mm") & "/01")
dte_lKIMAT_DT = g末日YMDGet(dte_aKIKAN_TO)
If ig締日 <> 31 Then
    ' 期首日補正: 前月締日+1日
    dte_lKISHU_DT = CDate(Format(DateAdd("m", -1, dte_lKISHU_DT), "yyyy/mm") & "/" & ig締日)
    dte_lKISHU_DT = DateAdd("d", 1, dte_lKISHU_DT)
    ' 期末日補正: 当月締日
    dte_lKIMAT_DT = CDate(Format(dte_lKIMAT_DT, "yyyy/mm") & "/" & ig締日)
End If
```

VB.NET版ではこのif文が丸ごと欠落している。

### ChukiCalcEngine.vb での参考実装（KessanBi対応）

`ChukiCalcEngine.vb:44-67` では `params.KessanBi` パラメータを受け取り、31/非31で分岐する実装が既にある。
ただし注記計算での用途であり、翌期以降年度末の算出ロジックが異なる（Access版バグ再現のための特殊実装）。

## 6. ig締日の取得元

### Access版
- `p_PublicVariable.txt:672`: `Public Const ig締日 = 31` — グローバル定数として常に31に固定

### VB.NET版
- **現状**: 取得処理なし。`ig締日=31` として暗黙固定
- **想定される取得先**: `t_settei` テーブル（`SetteiHelper.GetSettingValue` 経由）
  - `KlsryoCalculationEngine.GetSekouDt` が `t_settei` から施行日を取得する同パターン
  - `t_settei.settei_nm = 'SIME_BI'` または類似キーの追加が必要（現時点では未定義）
- **代替案**: `Execute` メソッドのパラメータとして `shimeBi As Integer = 31` を追加する方法

## 7. 使用パターン

### Execute呼び出しパターン（Form_f_flx_KLSRYO.vb:29-30）

```vb
Dim engine As New KlsryoCalculationEngine()
Dim dt As System.Data.DataTable = engine.Execute(DtFrom, DtTo, Taisho, Ktmg, Meisai)
```

現在は `shimeBi` パラメータなし。追加する場合は呼び出し元(Form_f_flx_KLSRYO, Form_f_KLSRYO_JOKEN)も修正が必要。

### t_settei取得パターン（KlsryoCalculationEngine.vb:991-1000）

```vb
Private Function GetSekouDt() As Date
    Try
        Dim dt = _crud.GetDataTable("SELECT val_datetime FROM t_settei WHERE settei_nm = 'SEKOU_DT'")
        If dt.Rows.Count > 0 Then Return CDate(dt.Rows(0)("val_datetime"))
    Catch ex As Exception
        DbConnectionManager.WriteError("設定値取得失敗(GetSekouDt)", ex)
    End Try
    Return New Date(2008, 4, 1) ' デフォルト値
End Function
```

同パターンで `GetShimeBi()` ヘルパーを追加可能。デフォルト値は 31。

## 8. 技術的制約・注意事項

1. **月境界の扱い**: 締日が29/30/31のとき2月や小の月では `DateTime.DaysInMonth` で切り下げが必要
2. **ynKimatDt の影響**: `getudoFrom/getudoTo` に加え、翌期以降5年分の年度末(`ynKimatDt`)も締日に合わせた計算が必要
3. **CalcSimeDtB への影響**: `CashScheduleBuilder.CalcSimeDtB` が月末固定のため、締日非31の場合は呼び出し側での対処が必要かどうか別途確認が必要（CalcSimeDtBはスケジュール生成時の締日計算に使用）
4. **Access版では常に31**: `ig締日 = 31` の定数のため、Access版でも `ig締日 <> 31` のパスは実際には通らない可能性が高い。実運用時のテストデータが必要

## 9. 命名規則・コーディング規約

- メソッド名: PascalCase (例: `GetShimeBi`, `GetSekouDt`)
- ローカル変数: camelCase (例: `shimeBi`, `kishuDt`)
- コメント: Access版メソッド名を括弧内に記述 (例: `' Access版 gKLSRYO_Main`)
- 締日パラメータ名の候補: `shimeBi`（ChukiCalcParamsの `KessanBi` に相当）

## 10. 推奨実装方針

### 方針A: パラメータ追加方式
`Execute(dtFrom, dtTo, taisho, ktmg, meisai, shimeBi As Integer = 31)` として引数追加。
- 呼び出し元2箇所（Form_f_flx_KLSRYO, Form_f_KLSRYO_JOKEN）も修正が必要
- テスト時に明示的に締日を指定できる利点あり

### 方針B: DB取得方式
`Private Function GetShimeBi() As Integer` を `GetSekouDt` と同パターンで追加。
- `t_settei` に `SIME_BI` キーの追加が必要（スキーマ変更を伴う）
- 呼び出し元の変更不要

**推奨**: 方針Aでデフォルト値31を設定し、後方互換性を維持しつつ拡張する。
