# ドキュメント調査: 締日非31の期首/期末補正ロジック

## 1. Access版元ソース仕様

### gKLSRYO_Main (行145-152)
```vba
If ig締日 <> 31 Then
    dte_lKISHU_DT = DateAdd("d", 1, CDate(Format(DateAdd("m",-1,dte_lKISHU_DT),"yyyy/mm") & "/" & ig締日))
    dte_lKIMAT_DT = CDate(Format(dte_lKIMAT_DT,"yyyy/mm") & "/" & ig締日)
End If
```

#### 動作詳細
- **ig締日=31（月末締め）**: 期首=月初1日、期末=月末日（補正なし）
- **ig締日=20 の例**:
  - 期首日: 前月の締日翌日（例: 前月20日+1 → 前月21日）
  - 期末日: 当月の締日（例: 当月20日）
  - つまり「21日〜翌月20日」が1ヶ月度となる

### ig締日の取得元
Access版では `ig締日` はグローバル変数で、ログイン時にDB（顧客マスタ等）から取得される。
VB.NET版では `KlsryoCalculationEngine.Execute` のパラメータとして渡すか、CrudHelperでDB取得する設計が必要。

## 2. VB.NET 日付処理の注意点

### DateAdd vs .AddMonths/.AddDays
- Access版 `DateAdd("m", -1, dt)` → VB.NET `dt.AddMonths(-1)`
- Access版 `DateAdd("d", 1, dt)` → VB.NET `dt.AddDays(1)`

### 月末日の扱い
- 締日が28/29/30で該当月に存在しない場合（例: 2月の30日）の対処が必要
- `DateTime.DaysInMonth(year, month)` で月の最終日を取得
- 締日 > 月の日数の場合は月末日に切り下げる（Access版の暗黙動作と同様）

### CDate(Format(...)) パターンの移植
Access版:
```vba
CDate(Format(dt, "yyyy/mm") & "/" & ig締日)
```
VB.NET等価:
```vb
Dim targetDay As Integer = Math.Min(shimeBi, DateTime.DaysInMonth(dt.Year, dt.Month))
New Date(dt.Year, dt.Month, targetDay)
```

## 3. 既存コードベースでの締日関連実装

### ChukiCalcEngine での KessanBi 対応
- `ChukiCalcEngine.vb` では既に `params.KessanBi` を受け取り、非月末決算の分岐処理を実装済み
- `ScheduleTypes.vb` の `ChukiCalcParams.KessanBi` プロパティ（デフォルト31）

### CashScheduleBuilder.CalcSimeDtB
- 締日計算は月末前提で実装済み
- 非月末締日対応は未実装

## 4. 月度配列 (getudoFrom/getudoTo) への影響

締日が月末以外の場合:
- `getudoFrom(i)` = 前月締日+1日 (i月度の開始日)
- `getudoTo(i)` = 当月締日 (i月度の終了日)
- 月末締めの場合と月度の区切りが変わるため、集計結果に直接影響する

## 5. エッジケース

| ケース | 期待動作 |
|--------|----------|
| 締日=31 | 補正なし（現行通り） |
| 締日=28, 2月 | 2月28日が期末（閏年でも28日） |
| 締日=30, 2月 | 2月28日（閏年29日）に切り下げ |
| 締日=15 | 16日〜翌月15日が1ヶ月度 |
| 締日=1 | 2日〜翌月1日が1ヶ月度 |
