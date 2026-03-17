# 要件定義書: chuki-idolst-joken-blackbox

## 1. 機能概要

Access版 LeaseM4BS の中期条件画面（f_CHUKI_JOKEN）と異動リスト条件画面（f_IDOLST_JOKEN）に対して、VB.NET版が同じ入力を与えたときに同じ出力（SQLクエリ条件・ラベルテキスト・パラメータリスト）を返すことをブラックボックステストで検証する。既存テストパターン（test_keijo_joken_blackbox.vb・test_schedule_blackbox.vb）に倣い、コンソールアプリ形式で実装する。

---

## 2. ユーザーストーリー

### US-001: f_CHUKI_JOKEN の条件オブジェクト構築テスト

- **As a** 開発者
- **I want** `Form_f_CHUKI_JOKEN.GenerateWhereClause` が入力パラメータの組み合わせに応じて正しい WHERE 句を生成することを確認したい
- **So that** 財務諸表注記一覧（f_flx_CHUKI）に正しいデータが表示されることを保証できる

#### 受け入れ基準
- [ ] リース区分「移転外のみ」のとき WHERE 句に `kykm.leakbn_id = 1` が含まれる
- [ ] リース区分「オペのみ」のとき WHERE 句に `kykm.leakbn_id = 2` が含まれる
- [ ] リース区分「両方」のとき WHERE 句に `kykm.leakbn_id IN (1, 2)` が含まれる
- [ ] 省略基準「従う」のとき WHERE 句に `kykm.chuum_id = 1` が含まれる
- [ ] 省略基準「省略のみ」のとき WHERE 句に `kykm.chuum_id = 2` が含まれる
- [ ] 省略基準「無視する」のとき WHERE 句に `chuum_id` 条件が含まれない
- [ ] 物件No FROM/TO が入力されたとき WHERE 句に `kykm.kykm_no >= @kyknNoFrom` / `<= @kyknNoTo` が含まれる
- [ ] 物件No FROM/TO が未入力のとき WHERE 句に物件No条件が含まれない
- [ ] 資産科目コンボが選択されたとき WHERE 句に `skmk.skmk_cd = @skmkCd` が含まれる
- [ ] リース会社コンボが選択されたとき WHERE 句に `lcpt.lcpt1_cd = @lcptCd` が含まれる
- [ ] 管理部署コンボが選択されたとき WHERE 句に `b_bcat.bcat_cd = @bcatCd` が含まれる
- [ ] 集計期間パラメータ `@dtFrom` が月初日、`@dtTo` が月末日に正規化される

---

### US-002: f_CHUKI_JOKEN のラベルテキスト生成テスト

- **As a** 開発者
- **I want** `Form_f_CHUKI_JOKEN.GenerateLabelText` が選択条件に応じて正しいラベルテキストを生成することを確認したい
- **So that** f_flx_CHUKI の集計条件表示が Access版と完全一致することを保証できる

#### 受け入れ基準
- [ ] 期間が `2024/04` から `2025/03` のとき `"決算期間：2024/04～2025/03  "` が含まれる
- [ ] 償却方法「定額」のとき `"償却方法：リース定額  "` が含まれる
- [ ] 償却方法「定率」のとき `"償却方法：近似定率  "` が含まれる
- [ ] 利息計算「利息法」のとき `"利息計算：利息法  "` が含まれる
- [ ] 利息計算「利子込法」のとき `"利息計算：利子込法  "` が含まれる

---

### US-003: f_CHUKI_JOKEN のバリデーション検証テスト

- **As a** 開発者
- **I want** `Form_f_CHUKI_JOKEN` の入力バリデーション（必須チェック・リース区分選択チェック）をブラックボックスで検証したい
- **So that** 不正入力で実行ボタンを押下したときに適切なガード処理が行われることを保証できる

#### 受け入れ基準
- [ ] `txt_DT_FROM` または `txt_DT_TO` が未入力のときメッセージ「必須項目が未入力です。」が表示される（ロジック相当の動作確認）
- [ ] `chk_LEAKBN_ITENGAI_F`・`chk_LEAKBN_OPE_F` が両方 False のときメッセージ「リース区分が設定されていません。」が表示される（ロジック相当の動作確認）
- [ ] FROM > TO の場合に `SwapIf` が正しく日付を入れ替える

---

### US-004: f_IDOLST_JOKEN の条件オブジェクト構築テスト

- **As a** 開発者
- **I want** `Form_f_flx_IDOLST.GetBcatConditions` が選択されたチェックボックスのパターンに応じて正しい SQL 条件を生成することを確認したい
- **So that** 異動リスト（f_flx_IDOLST）に正しいデータが表示されることを保証できる

#### 受け入れ基準
- [ ] 管理部署1のみ True のとき `"AND ( b_bcat.bcat1_cd <> r1_bcat.bcat1_cd OR b_bcat.bcat1_cd IS NULL OR r1_bcat.bcat1_cd IS NULL )"` が生成される
- [ ] 管理部署1・3 が True のとき条件が `OR` で結合される（2条件）
- [ ] 全て False のとき `String.Empty` が返される
- [ ] 全て True のとき5条件が `OR` で結合される
- [ ] 移動日 `@dtFrom`・`@dtTo` パラメータが `frm.DtFrom`・`frm.DtTo` として正しく受け渡される

---

### US-005: f_IDOLST_JOKEN のラベルテキスト生成テスト

- **As a** 開発者
- **I want** `Form_f_IDOLST_JOKEN.GetLabelText` がチェックボックスの状態に応じて正しいラベルテキストを生成することを確認したい
- **So that** f_flx_IDOLST の集計条件表示が Access版と完全一致することを保証できる

#### 受け入れ基準
- [ ] 移動日期間が `"2024/04/01"` から `"2024/06/30"` のとき `"移動日:　2024/04/01～2024/06/30  "` が含まれる
- [ ] 管理部署1のみ True のとき `"管理部署1"` が含まれ `"管理部署2"` が含まれない
- [ ] 管理部署1・2 が True のとき `"管理部署1、管理部署2"` となり末尾が `"、"` で終わらない
- [ ] 全て False のとき `"移動チェックカテゴリ： "` の後に部署名が含まれない（前提: バリデーションにより実行不可だが、ロジックとして Empty 相当）
- [ ] 管理部署5のみ True のとき `"管理部署5"` のみ含まれ末尾が `"、"` で終わらない

---

### US-006: f_IDOLST_JOKEN のバリデーション検証テスト

- **As a** 開発者
- **I want** `Form_f_IDOLST_JOKEN` の入力バリデーション（管理部署チェック必須）をブラックボックスで検証したい
- **So that** 不正入力で実行ボタンを押下したときに適切なガード処理が行われることを保証できる

#### 受け入れ基準
- [ ] `chk_BCAT1_F` から `chk_BCAT5_F` が全て False のときメッセージ「管理部署1～5のうち少なくとも1つにチェックしてください。」が表示される（ロジック相当）
- [ ] FROM > TO の場合に `SwapIf` が移動日を正しく入れ替える

---

## 3. 機能要件

### FR-001: テスト対象ロジックの網羅

- 説明: 以下の全ロジック単位に対してブラックボックステストが存在すること
  - `Form_f_CHUKI_JOKEN.GenerateWhereClause`（WHERE 句生成）
  - `Form_f_CHUKI_JOKEN.GenerateLabelText`（ラベルテキスト生成）
  - `Form_f_IDOLST_JOKEN.GetLabelText`（ラベルテキスト生成）
  - `Form_f_flx_IDOLST.GetBcatConditions`（管理部署 SQL 条件生成）
- 優先度: 必須

### FR-002: ゴールデンデータによる期待値設定

- 説明: 各テストケースの期待値は Access版コードの読解と手計算（VBA相当ロジックの追跡）により求めた値を使用する。「妥当に見える値」ではなく「Access版が実際に生成するクエリ・テキスト」を期待値として設定する
- 優先度: 必須

### FR-003: テスト結果の PASS/FAIL/SKIP レポート

- 説明: 既存の `passCount`/`failCount`/`skipCount` 形式を継続使用し、全テスト完了後にサマリーを出力する。`FAIL` が1件でもあれば終了コード1を返す
- 優先度: 必須

### FR-004: DB 接続不要のユニットテスト

- 説明: `GenerateWhereClause`・`GenerateLabelText`・`GetLabelText`・`GetBcatConditions` はすべて純粋な文字列生成ロジックのためDB接続なしで実行可能であること。ただし、VB.NETのWinForms依存がある場合は対象メソッドをロジック分離して検証する
- 優先度: 必須

### FR-005: テストケースの網羅基準

- 説明: 各入力パラメータの組み合わせについて以下を網羅すること
  - 正常系: 全入力が最大/最小範囲内の典型値
  - 境界値: FROM=TO（同一月）・FROM が TO より大きい（SwapIf テスト）・物件No=0
  - 異常系: 必須入力未設定・チェックボックス全未選択
- 優先度: 必須

### FR-006: 既存テストパターンとのスタイル統一

- 説明: テストコードは `test_keijo_joken_blackbox.vb` のスタイル（`Module` + `Sub Test_XXX()` + `Console.Write` PASS/FAIL出力）に従う。ファイル先頭にコンパイルコマンドを記載する
- 優先度: 必須

---

## 4. 非機能要件

### NFR-001: パフォーマンス

- DB接続不要テスト群の実行時間は5秒以内とする
- WinForms ロジック分離テストはフォームのインスタンス化を伴わず純粋関数として実行する

### NFR-002: 保守性

- テストコードは既存の `test_keijo_joken_blackbox.vb` / `test_schedule_blackbox.vb` のスタイルに準拠する（`Module` + `Sub Test_XXX()` 形式）
- コンパイルコマンドをファイル先頭コメントに記載する
- 各テストは `Try/Catch` で囲み、予期しない例外を FAIL として捕捉する

### NFR-003: 再現性

- テストは冪等であること（同じ入力で何度実行しても同じ結果になること）
- テスト間の状態共有なし（各テストは独立して実行可能）

### NFR-004: 文字エンコーディング

- コンソール出力は `Console.OutputEncoding = System.Text.Encoding.UTF8` を設定する

---

## 5. 前提条件・制約

- テストは `LeaseM4BS.DataAccess.dll` に依存する。DLLのビルドが完了していることが前提
- コンパイルは `vbc /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll` で行う（.NET Framework 4.7.2）
- `Form_f_CHUKI_JOKEN.GenerateWhereClause` および `Form_f_CHUKI_JOKEN.GenerateLabelText` は `Private` メソッドのため、テスト用に `Friend` または別クラスへの切り出しが必要（仮定事項1参照）
- `Form_f_IDOLST_JOKEN.GetLabelText` および `Form_f_flx_IDOLST.GetBcatConditions` も同様に可視性の調整が必要な場合がある
- `GetMonthStart` / `GetMonthEnd` ヘルパー関数が `Form_f_CHUKI_JOKEN` 内で使用されており、月初日・月末日への正規化仕様が前提
- Access版 (`C:\access_LeaseM4BS`) はバイナリ形式のため、期待値はVBAコードの読解と手計算で確認する

---

## 6. スコープ外

- WinForms フォームの描画・UI レイアウト検証
- `Form_f_flx_CHUKI.BuildSql` の完全な SQL 文検証（`todo` コメントの未実装列を含む SELECT 句は対象外）
- `Form_f_flx_CHUKI.SearchData` の DB 接続 E2E テスト（DBデータ依存のため対象外）
- `Form_f_flx_IDOLST.SearchData` の DB 接続 E2E テスト（同上）
- `Form_f_CHUKI_JOKEN` のコンボボックス描画ロジック（`Combo_DrawItem`）のテスト
- `Form_f_CHUKI_YOUSHIKI` / `Form_f_CHUKI_RECALC` / `Form_f_CHUKI_SCH` の検証（別 Issue 対象）
- `Form_f_flx_CHUKI.cmd_SCH_Click` / `cmd_REF_Click` 等のボタン動作 E2E テスト

---

## 7. 用語定義

| 用語 | 定義 |
|---|---|
| CHUKI | 財務諸表注記（中期）の略称。`f_CHUKI_JOKEN` はその検索条件入力フォーム |
| IDOLST | 移動物件一覧表（異動リスト）の略称。`f_IDOLST_JOKEN` はその検索条件入力フォーム |
| WhereClause | `f_flx_CHUKI` に渡す SQL の WHERE 句文字列。`@dtFrom`・`@dtTo` 等のパラメータプレースホルダを含む |
| CheckBcatFlags | `Form_f_flx_IDOLST` が受け取る管理部署チェック状態の Boolean 配列（長さ5） |
| GetBcatConditions | CheckBcatFlags の True 要素に対応する SQL 条件を OR 結合して返すメソッド |
| leakbn_id | リース区分 ID。1=移転外ファイナンスリース、2=オペレーティングリース |
| chuum_id | 省略基準 ID。1=従う（注記）、2=省略物件のみ |
| SwapIf | FROM > TO の場合に日付の順序を正しく入れ替えるユーティリティメソッド |
| GetMonthStart / GetMonthEnd | 日付から月初日・月末日を返すユーティリティメソッド |
| bcat | 管理部署コード。bcat1〜bcat5 の階層構造を持つ |

---

## 8. 仮定事項

1. **`Private` メソッドのテスト可否**: `GenerateWhereClause`・`GenerateLabelText`（CHUKI_JOKEN）および `GetLabelText`（IDOLST_JOKEN）は現在 `Private` 宣言されている。ブラックボックステストのためには `Friend` への変更またはロジック分離クラスへの切り出しが必要。実装担当者がこの変更を事前に行うことを前提とする。変更を行わない場合、テストはフォームを実際にインスタンス化して行動を観察する間接テストに切り替える（その場合はDB接続なしのテスト実施が困難になる点に注意）。

2. **`GetBcatConditions` の Public 化**: `Form_f_flx_IDOLST.GetBcatConditions` は現在 `Private` であるが、テスト対象として `Friend` または `Public` に変更することを仮定する。

3. **`GetMonthStart` / `GetMonthEnd` の実装**: `Form_f_CHUKI_JOKEN` 内で参照されている `GetMonthStart` / `GetMonthEnd` は共通ユーティリティとして定義されていると仮定する（未確認）。実装場所の確認が必要。

4. **WinForms 依存の回避**: コンソールアプリ形式のテストで WinForms の `DateTimePicker` 等を直接操作することは困難なため、テスト対象メソッドはロジック部分のみを Pure Function として抽出した等価ロジック関数をテストする（例: `GenerateWhereClause` 相当の静的メソッドを作成）。Access版との一致検証が主目的のため、WinForms コントロールへの依存はモック化せず入力値を直接 `String`・`Boolean`・`Date` 型で渡す形式を推奨する。

5. **Access版 WHERE 句の再現**: Access版の VBA クエリロジックはバイナリ形式のため、本要件書の期待値は VB.NET 版ソースコード（`Form_f_CHUKI_JOKEN.vb`・`Form_f_IDOLST_JOKEN.vb`）の読解から導出した。実機確認が必要な場合は Access を起動して実際の SQL パラメータ値を検証すること。
