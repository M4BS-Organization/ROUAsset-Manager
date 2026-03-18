# 要件定義書: closing-day-correction

## 1. 機能概要

`KlsryoCalculationEngine.Execute` において、現在は月末締め（ig締日=31）を前提として期首日を月初(1日)固定、期末日を月末固定で算出している。本機能は、DBまたは設定から取得した締日（ig締日）が31以外の場合に、期首日・期末日・月度配列を補正するロジックを追加し、Access版 `gKLSRYO_Main`（行145-152）との完全互換性を実現する。

---

## 2. ユーザーストーリー

### US-001: 締日非31の期首日・期末日補正

- **As a** 経理担当者
- **I want** 月末以外の締日（例: 15日締め、25日締め）を設定した場合でも、リース料期間集計の期首日・期末日が締日に基づいて正しく算出される
- **So that** Access版と同じ集計結果が得られ、会計処理の誤りが発生しない

#### 受け入れ基準
- [ ] ig締日=31（または未設定）の場合、現行動作と同一: 期首日=月初(1日)、期末日=月末
- [ ] ig締日=15 の場合、Access版 gKLSRYO_Main 行145-152 の算出式と同一の期首日・期末日が得られる
- [ ] ig締日=25 の場合、Access版の算出式と同一の期首日・期末日が得られる
- [ ] 期首日・期末日の補正は `Execute` 内の kishuDt / kimatDt の算出箇所に閉じている

### US-002: 月度配列への補正反映

- **As a** 経理担当者
- **I want** 締日非31の場合、月度開始日配列 (getudoFrom) および月度終了日配列 (getudoTo) も補正後の kishuDt に基づいて正しく再構築される
- **So that** 当期月別内訳（G01〜G12）が締日ベースで正しく振り分けられる

#### 受け入れ基準
- [ ] getudoFrom(0) が補正後 kishuDt と一致する
- [ ] getudoTo(i) = getudoFrom(i+1).AddDays(-1) の関係が締日補正後も維持される
- [ ] 締日=31 の場合の getudoFrom/getudoTo は現行と同一

### US-003: 翌期以降年度末の締日対応

- **As a** 経理担当者
- **I want** 翌期以降の年度末（ynKimatDt）も締日非31の場合に補正された kimatDt を基点として正しく5年分算出される
- **So that** 翌期以降1年内〜5年超の内訳金額が締日ベースで正確に区切られる

#### 受け入れ基準
- [ ] 締日=31 の場合の ynKimatDt は現行と同一（月末）
- [ ] 締日=15 の場合、ynKimatDt(0) は補正後 kimatDt の翌年同月15日相当（月内日数を超える場合は月末日）

### US-004: 締日値の取得

- **As a** システム管理者
- **I want** ig締日の値が `t_settei` テーブルから取得される
- **So that** 締日設定を変更した際に再ビルドなしで即時反映される

#### 受け入れ基準
- [ ] `t_settei` テーブルに `settei_nm = 'SHIMEBI'`（仮定: 後述「仮定事項」参照）のレコードが存在する場合、その `val_number` を締日(Integer)として使用する
- [ ] レコードが存在しない場合、または val_number が NULL の場合は 31 をデフォルト値として使用する
- [ ] 締日値の取得失敗（DBアクセス例外）時は 31 をフォールバックとして使用し、エラーをログに記録する（既存 `GetSekouDt` の実装パターンに準拠）

---

## 3. 機能要件

### FR-001: 締日値の取得処理

- 説明: `Execute` 開始時に `t_settei` テーブルから締日（整数値: 1〜31）を取得するプライベートメソッド `GetShimebi()` を追加する。既存の `GetSekouDt()` と同様のパターン（try/catch + デフォルト値 + エラーログ）で実装する
- 優先度: 必須

### FR-002: 期首日補正ロジック

- 説明: 取得した締日 shimeDayInt が 31 以外の場合、kishuDt を以下の Access版算出式で再計算する
  ```
  dte_lKISHU_DT = DateAdd("d", 1, CDate(Format(DateAdd("m",-1,dte_lKISHU_DT),"yyyy/mm") & "/" & ig締日))
  ```
  VB.NET 等価: `kishuDt = New Date(dtFrom.AddMonths(-1).Year, dtFrom.AddMonths(-1).Month, 1).AddMonths(1).AddDays(-1 + shimeDayInt)` （ただし月末を超える日数に対する丸め処理を含む）
- 優先度: 必須

### FR-003: 期末日補正ロジック

- 説明: 取得した締日 shimeDayInt が 31 以外の場合、kimatDt を以下の Access版算出式で再計算する
  ```
  dte_lKIMAT_DT = CDate(Format(dte_lKIMAT_DT,"yyyy/mm") & "/" & ig締日)
  ```
  VB.NET 等価: 締日 shimeDayInt を dtTo と同年月に適用し、月内に存在する日として設定する（2月の場合は28日または29日に切り下げ）
- 優先度: 必須

### FR-004: 月度配列の再構築

- 説明: kishuDt の補正後、getudoFrom/getudoTo の算出は既存ロジックをそのまま流用する（kishuDt.AddMonths(i) による月度区間は補正後 kishuDt を起点とするため、修正不要）
- 優先度: 必須

### FR-005: 締日=31 の場合の後方互換性維持

- 説明: shimeDayInt = 31 の場合は現行ロジック（kishuDt = 月初, kimatDt = 月末）を変更しない。既存の全ユニットテストが引き続き通過すること
- 優先度: 必須

---

## 4. 非機能要件

### NFR-001: パフォーマンス

- `GetShimebi()` による `t_settei` クエリは `Execute` 呼び出し1回につき最大1回のDBアクセスとする
- 追加のDB呼び出しによって既存の `Execute` 実行時間（対象件数1,000件）が 50ms 以上増加しないこと（仮定: 現行の SEKOU_DT 取得と同等のオーバーヘッド）

### NFR-002: 信頼性

- `t_settei` の `SHIMEBI` レコード不在・NULL・型不正（整数以外）のすべての場合に、例外を伝播させずデフォルト値 31 で処理を継続する
- エラーは `DbConnectionManager.WriteError` でログ記録する（既存パターンに準拠）

### NFR-003: 保守性

- 締日補正ロジックは `Execute` 内の既存の「期首日/期末日の算出」コメントブロックを拡張する形で実装し、新たなクラス/モジュールは作成しない
- 補正条件は `If shimeDayInt <> 31 Then ... End If` の単一ブロックに集約する

### NFR-004: テスト互換性

- 既存の単体テスト（締日=31 前提のもの）がすべてパスすること
- 新規テストとして締日=15 および締日=25 の境界値テストを追加すること

---

## 5. 前提条件・制約

- 対象コードは `c:/kobayashi_LeaseM4BS/LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` の `Execute` メソッドのみ
- `CashScheduleBuilder.GetMonthEndDate` および `CashScheduleBuilder.CalcSimeDtB` は変更しない（締日=31 固定の用途を分離するため）
- PostgreSQL `t_settei` テーブルの構造は既存の `SetteiHelper.vb` で参照されているスキーマ（`settei_nm VARCHAR, val_number DOUBLE PRECISION, settei_type INTEGER`）に依存する
- Access版 gKLSRYO_Main の実装言語はVBAであり、`DateAdd`/`Format` 関数の境界値動作（月末日の切り下げ等）をVB.NETで厳密に再現すること

---

## 6. スコープ外

- `CashScheduleBuilder.CalcSimeDtB` の締日パラメータ化（同関数は支払スケジュール計算専用であり、別 Issue #8 で管理）
- `KeijoCalculationEngine` および `MonthlyJournalEngine` への締日補正の適用（本Issueの対象外）
- 締日設定のUIからの変更機能（`t_settei` への直接書き込みは既存 `SetteiHelper` が担う）
- 締日が 1〜31 の範囲外の値（0, 負数, 32以上）への対応（バリデーションは呼び出し元の責務とし、本エンジンはデフォルト値 31 にフォールバックするのみ）

---

## 7. 用語定義

| 用語 | 定義 |
|------|------|
| ig締日 | Access版グローバル変数。月次締め処理の基準日（日数: 1〜31、31=月末） |
| 期首日 (kishuDt) | 集計期間の開始日。月末締め時は月初(1日)、非月末締め時はAccess版算出式に従う |
| 期末日 (kimatDt) | 集計期間の終了日。月末締め時は月末日、非月末締め時は締日当日 |
| 月度配列 (getudoFrom/getudoTo) | 各月の開始日・終了日配列（最大13/12要素）。当期月別内訳の振り分けに使用 |
| 翌期年度末 (ynKimatDt) | 翌期以降5年分の期末日配列。翌期以降の残高内訳算出に使用 |
| SHIMEBI | t_settei テーブルにおける締日設定のsettei_nm（仮定値。実際のキー名は要確認） |

---

## 8. 仮定事項

1. **`t_settei` における締日設定キー名**: Access版での `ig締日` に対応する `t_settei.settei_nm` のキー名を `'SHIMEBI'` と仮定している。実際のキー名（例: `'IG_SHIMEBI'`、`'CLOSING_DAY'` 等）は、DBスキーマ定義またはAccess版の `gSET_*` 系初期化コードを確認して特定すること。**実装前にDBまたはAccess版コードで確認が必要**。

2. **締日の設定型**: `t_settei.val_number` に数値型（settei_type=1）として格納されることを仮定している。

3. **Access版 `DateAdd("m",-1, kishuDt)` の境界値動作**: VBAの `DateAdd` は月末日を自動調整する（例: 3/31 の1ヶ月前 = 2/28）。VB.NET の `AddMonths` も同様の動作をするため等価と仮定している。

4. **kishuDt の初期値**: 補正前の kishuDt = `New Date(dtFrom.Year, dtFrom.Month, 1)` であることを前提とする（現行コード通り）。

5. **ynKimatDt の締日対応**: Access版 gKLSRYO_Main の行145-152は期首日・期末日のみを対象としており、ynKimatDt の算出ロジック（`GetMonthEndDate` + `AddMonths(12)`）は変更対象に含まれない可能性がある。補正後の kimatDt を基点として `GetMonthEndDate` で月末を取得する現行ロジックは、締日非31の場合には不正確になる可能性があるため、**実装時に Access版の ynKimatDt 算出ロジックを再確認すること**。
