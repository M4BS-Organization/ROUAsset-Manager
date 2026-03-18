# 要件定義書: klsryo-joken-options

## 1. 機能概要

期間リース料支払明細表の条件画面（`Form_f_KLSRYO_JOKEN`）において、集計対象（対象）・計上タイミング（タイミング）・明細単位（明細）の3つのオプションをユーザーが選択できるよう実装する。現状はこれら3つのパラメータがハードコード固定値で渡されており、Access版との機能差異が存在する。本機能はその差異を解消し、Access版と同等の柔軟な集計条件選択を提供する。

---

## 2. ユーザーストーリー

### US-001: 集計対象の選択
- **As a** 経理担当者
- **I want** 期間リース料支払明細の集計対象をリース料・保守料・全部から選択したい
- **So that** 必要に応じてリース料のみ、または保守料のみに絞り込んだ集計が行える

#### 受け入れ基準
- [ ] 条件画面に「集計対象」として「リース料」「保守料」「全部」の3択ラジオボタンが表示される
- [ ] デフォルト選択値は「全部」（Access版 `opg_TAISHO = 3` と同等）である
- [ ] 「実行」ボタン押下時、選択された値（1=リース料、2=保守料、3=全部）が `Form_f_flx_KLSRYO.Taisho` に渡される
- [ ] 渡された `Taisho` 値が `KlsryoCalculationEngine.Execute` に正しく反映される

### US-002: 計上タイミングの選択
- **As a** 経理担当者
- **I want** 計上タイミングを締日ベースまたは支払日ベースから選択したい
- **So that** 会計処理方針に合わせた集計基準でリース料明細を出力できる

#### 受け入れ基準
- [ ] 条件画面に「計上タイミング」として「締日ベース」「支払日ベース」の2択ラジオボタンが表示される
- [ ] デフォルト選択値は「締日ベース」（Access版 `opg_KTMG = 1` と同等）である
- [ ] 「実行」ボタン押下時、選択された値（`ShriKtmg.SimeDtBase` または `ShriKtmg.ShriDtBase`）が `Form_f_flx_KLSRYO.Ktmg` に渡される
- [ ] 渡された `Ktmg` 値が `KlsryoCalculationEngine.Execute` に正しく反映される

### US-003: 明細単位の選択
- **As a** 経理担当者
- **I want** 明細の集計単位を物件単位または配賦単位から選択したい
- **So that** 配賦情報を含めた詳細な内訳、または物件単位での集計を選んで出力できる

#### 受け入れ基準
- [ ] 条件画面に「明細」として「物件単位」「配賦単位」の2択ラジオボタンが表示される
- [ ] デフォルト選択値は「配賦単位」（Access版 `opg_MEISAI = 2` と同等）である
- [ ] 「実行」ボタン押下時、選択された値（`ShriMeisai.Kykm` または `ShriMeisai.Haif`）が `Form_f_flx_KLSRYO.Meisai` に渡される
- [ ] 渡された `Meisai` 値が `KlsryoCalculationEngine.Execute` に正しく反映される

### US-004: 選択条件のラベル表示
- **As a** 経理担当者
- **I want** 集計結果画面に選択した条件が表示されること
- **So that** 現在表示中の集計結果がどの条件で出力されたかを一目で確認できる

#### 受け入れ基準
- [ ] `Form_f_flx_KLSRYO` のヘッダラベル（`lbl_CONDITION` 相当）に、選択された集計対象・計上タイミング・明細単位が日本語で表示される
- [ ] ラベルに表示される条件文字列は集計期間と合わせて「集計期間: yyyy/MM ～ yyyy/MM / 対象: 全部 / タイミング: 締日ベース / 明細: 配賦単位」のような形式とする（仮定、実装時に調整可）

---

## 3. 機能要件

### FR-001: ラジオボタンの追加（Designer.vb）
- 説明: `Form_f_KLSRYO_JOKEN.Designer.vb` に以下のコントロールを追加する
  - **集計対象グループ**（GroupBox2 内 Panel3）: 既存の `オプション504`（リース料）・`オプション506`（保守料）・`オプション508`（全部）をそのまま活用し、適切な命名で紐付ける
  - **計上タイミンググループ**（GroupBox1 内 Panel1）: 既存の `chk_SHIME`（締日ベース）・`オプション483`（支払日ベース）をそのまま活用し、適切な命名で紐付ける
  - **明細単位グループ**（GroupBox2 内 Panel2）: 既存の `オプション487`（物件単位）・`オプション489`（配賦単位）をそのまま活用し、適切な命名で紐付ける
- 優先度: 必須

### FR-002: 各グループ内のラジオボタン排他制御
- 説明: 3つの選択グループがそれぞれ独立した GroupBox または Panel 内に配置され、同グループ内のラジオボタンが排他選択となること
- 優先度: 必須

### FR-003: デフォルト値の設定
- 説明: フォームロード時に以下のデフォルト値が選択済みとなること
  - 集計対象: 全部（`オプション508.Checked = True`）
  - 計上タイミング: 締日ベース（`chk_SHIME.Checked = True`）
  - 明細単位: 配賦単位（`オプション489.Checked = True`）
- 優先度: 必須

### FR-004: 選択値の Form_f_flx_KLSRYO への受け渡し
- 説明: `cmd_EXECUTE_Click` において、以下の変換ロジックを実装する
  - `Taisho`: `オプション504.Checked` なら 1、`オプション506.Checked` なら 2、それ以外（全部）なら 3
  - `Ktmg`: `chk_SHIME.Checked` なら `ShriKtmg.SimeDtBase`、それ以外なら `ShriKtmg.ShriDtBase`
  - `Meisai`: `オプション487.Checked` なら `ShriMeisai.Kykm`、それ以外なら `ShriMeisai.Haif`
- 優先度: 必須

### FR-005: ラベルテキストの生成と受け渡し
- 説明: `Form_f_flx_KLSRYO.LabelText` プロパティに選択条件を含む文字列を設定し、集計結果画面のヘッダに表示する
- 優先度: 推奨

---

## 4. 非機能要件

### NFR-001: パフォーマンス
- 条件選択操作（ラジオボタン切り替え）はユーザーの操作に即時反応し、遅延が生じないこと（操作から 100ms 以内に UI が更新される）

### NFR-002: ユーザビリティ
- 各選択グループにはラベルを付し、何を選択しているかが一目でわかること
- 現状の Designer.vb に既にラベル（`ラベル485`「計上タイミング」、`Label5`「集計対象」、`Label4`「明細」）が定義済みであるため、これらを活用すること

### NFR-003: 後方互換性
- `Form_f_flx_KLSRYO` の `Taisho`・`Ktmg`・`Meisai` プロパティのインターフェースは変更しない
- `KlsryoCalculationEngine.Execute` のシグネチャは変更しない

### NFR-004: コーディング規約
- 既存コードベースの命名規則（`cmd_`、`txt_`、`オプション`、`chk_` プレフィックス）に準拠する
- `Designer.vb` の変更は手動で行うか、IDE の Designer を使用すること（CLAUDE.md の PreToolUse フック注意）

---

## 5. 前提条件・制約

- `Form_f_KLSRYO_JOKEN.Designer.vb` には既に3グループのラジオボタンが定義済みだが、 `Form_f_KLSRYO_JOKEN.vb` の `cmd_EXECUTE_Click` でそれらの値が読み取られずハードコード値が使用されている
- `KlsryoTypes.vb` に `ShriKtmg` および `ShriMeisai` の Enum が定義済みであり、そのまま利用可能
- `KlsryoCalculationEngine.Execute` は `taisho As Integer`、`ktmg As ShriKtmg`、`meisai As ShriMeisai` を引数として受け付ける設計になっている
- Designer.vb を直接編集する場合、CLAUDE.md の PreToolUse フックによる確認プロンプトに注意すること

---

## 6. スコープ外

- `ShriKtmg.ShriRDtBase`（前月支払日ベース）の UI 選択肢への追加（Access版でも opg_KTMG は1か2のみのため）
- `KlsryoCalculationEngine` 内部の計算ロジック変更
- `Form_f_flx_KLSRYO` の表示レイアウト変更（列追加・削除等）
- 条件設定の保存・復元機能（ユーザー設定の永続化）
- 条件変更に伴う `Form_f_flx_KLSRYO` のリアルタイム再計算

---

## 7. 用語定義

| 用語 | 定義 |
|---|---|
| 集計対象 (Taisho) | リース料・保守料・全部のいずれを集計するかを示す区分。Access版 `opg_TAISHO` に相当 |
| 計上タイミング (Ktmg) | 計上基準日を締日ベースと支払日ベースで切り替えるパラメータ。Access版 `opg_KTMG` に相当。`ShriKtmg` Enum で管理 |
| 明細単位 (Meisai) | 集計結果を物件単位（契約単位）で出力するか配賦単位で出力するかを示す区分。Access版 `opg_MEISAI` に相当。`ShriMeisai` Enum で管理 |
| 締日ベース | 各月の締日を基準に計上を行う方式（`ShriKtmg.SimeDtBase`） |
| 支払日ベース | 実際の支払日を基準に計上を行う方式（`ShriKtmg.ShriDtBase`） |
| 物件単位 | 契約物件（kykm）単位で明細を集計する方式（`ShriMeisai.Kykm`） |
| 配賦単位 | 配賦情報（haif）単位で明細を集計する方式（`ShriMeisai.Haif`） |

---

## 8. 仮定事項

1. **既存ラジオボタンの再利用**: Designer.vb を調査した結果、既に `chk_SHIME`・`オプション483`（計上タイミング）、`オプション504`・`オプション506`・`オプション508`（集計対象）、`オプション487`・`オプション489`（明細単位）が定義済みである。本実装では「.vb ファイルの `cmd_EXECUTE_Click` を修正してこれら既存コントロールの値を読み取る」ことを主要作業と位置付ける。Designer.vb の大幅な変更は不要と判断している。実装前に各ラジオボタンが適切な Panel 内に配置され排他制御が機能しているかを動作確認すること。

2. **LabelText の形式**: US-004 におけるラベル文字列のフォーマットは暫定案であり、`Form_f_flx_KLSRYO` の `lbl_CONDITION` ラベルの表示スペースに合わせて実装時に調整すること。

3. **AccessのOpg値とEnumのマッピング**: Access版 `opg_KTMG = 1` を `ShriKtmg.SimeDtBase`（値 = 1）、`opg_KTMG = 2` を `ShriKtmg.ShriDtBase`（値 = 2）と対応付ける。`KlsryoTypes.vb` の Enum 定義と一致していることを確認済みだが、実装前に再確認すること。
