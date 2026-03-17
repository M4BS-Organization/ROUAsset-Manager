# テスト計画書: chuki-idolst-joken-blackbox

## 1. テスト戦略

- **テストフレームワーク**: VB.NET コンソールアプリ（DLL 直接参照）
- **テストレベル**: 単体テスト（ロジック単体）＋ WinForms ロジック分離テスト
- **カバレッジ目標**: 条件分岐の全パスカバレッジ（MC/DC 相当）
- **モック戦略**: DB 接続不要。全テスト対象メソッドが純粋な文字列生成ロジックのため、モックなしで入力値を直接渡す

### テスト対象メソッドの可視性変更前提

以下のメソッドを `Private` から `Friend` に変更することをテスト実装の前提とする（仮定事項1、2参照）。

| メソッド | 所在クラス | 変更内容 |
|---|---|---|
| `GenerateWhereClause` | `Form_f_CHUKI_JOKEN` | `Private` → `Friend` |
| `GenerateLabelText` | `Form_f_CHUKI_JOKEN` | `Private` → `Friend` |
| `GetLabelText` | `Form_f_IDOLST_JOKEN` | `Private` → `Friend` |
| `GetBcatConditions` | `Form_f_flx_IDOLST` | `Private` → `Friend` |

ただし、WinForms コントロールへの依存（`txt_DT_FROM.Value`、`chk_LEAKBN_ITENGAI_F.Checked` 等）があるため、テストではフォームをインスタンス化してコントロール値を直接設定してからメソッドを呼び出す方式を採用する。

---

## 2. テスト環境

### 前提条件
- `LeaseM4BS.DataAccess.dll` のビルドが完了していること
- `System.Windows.Forms.dll` が利用可能であること（.NET Framework 4.7.2）

### コンパイルコマンド
```bash
vbc /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll test_chuki_idolst_joken_blackbox.vb
```

### テストデータの準備方法
- DB 接続不要。全テストケースは入力値をコード内で直接定義する
- 期待値は Access版 VBA コードの読解と `Form_f_CHUKI_JOKEN.vb` / `Form_f_IDOLST_JOKEN.vb` のロジック解析により導出

### 外部依存のモック方法
- DB 接続なし
- WinForms フォームをインスタンス化して各コントロールのプロパティ値を直接設定

---

## 3. テスト対象一覧

| ID | 対象 | 優先度 | 関連要件 |
|---|---|---|---|
| T-001 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - リース区分両方 | 高 | US-001 |
| T-002 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - リース区分移転外のみ | 高 | US-001 |
| T-003 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - リース区分オペのみ | 高 | US-001 |
| T-004 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 省略基準「従う」 | 高 | US-001 |
| T-005 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 省略基準「無視する」 | 高 | US-001 |
| T-006 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 省略基準「省略のみ」 | 高 | US-001 |
| T-007 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 物件No FROM/TO あり | 中 | US-001 |
| T-008 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 物件No 未入力 | 中 | US-001 |
| T-009 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 資産科目指定あり | 中 | US-001 |
| T-010 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - リース会社指定あり | 中 | US-001 |
| T-011 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 管理部署指定あり | 中 | US-001 |
| T-012 | `Form_f_CHUKI_JOKEN.GenerateWhereClause` - 日付パラメータ @dtFrom/@dtTo | 高 | US-001 |
| T-013 | `Form_f_CHUKI_JOKEN.GenerateLabelText` - 定額法・利息法 | 高 | US-002 |
| T-014 | `Form_f_CHUKI_JOKEN.GenerateLabelText` - 定率法・利子込法 | 高 | US-002 |
| T-015 | `Form_f_CHUKI_JOKEN.GenerateLabelText` - 期間テキスト | 高 | US-002 |
| T-016 | SwapIf 日付大小入れ替え (CHUKI) | 高 | US-003 |
| T-017 | `Form_f_flx_IDOLST.GetBcatConditions` - 管理部署1のみ True | 高 | US-004 |
| T-018 | `Form_f_flx_IDOLST.GetBcatConditions` - 管理部署1・3 True | 高 | US-004 |
| T-019 | `Form_f_flx_IDOLST.GetBcatConditions` - 全 False | 高 | US-004 |
| T-020 | `Form_f_flx_IDOLST.GetBcatConditions` - 全 True | 高 | US-004 |
| T-021 | `Form_f_IDOLST_JOKEN.GetLabelText` - 移動日期間テキスト | 高 | US-005 |
| T-022 | `Form_f_IDOLST_JOKEN.GetLabelText` - 管理部署1のみ True | 高 | US-005 |
| T-023 | `Form_f_IDOLST_JOKEN.GetLabelText` - 管理部署1・2 True (末尾「、」なし) | 高 | US-005 |
| T-024 | `Form_f_IDOLST_JOKEN.GetLabelText` - 管理部署5のみ True | 高 | US-005 |
| T-025 | `Form_f_IDOLST_JOKEN.GetLabelText` - 管理部署1・2・3・4 True (末尾「、」バグ確認) | 高 | US-005 |
| T-026 | SwapIf 日付大小入れ替え (IDOLST) | 高 | US-006 |

---

## 4. テストケース

### TC-001: GenerateWhereClause - リース区分「両方」選択

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `chk_LEAKBN_ITENGAI_F.Checked = True`、`chk_LEAKBN_OPE_F.Checked = True`
- **入力**: dtFrom="2024/04", dtTo="2025/03", 移転外チェック=True, OPEチェック=True, 省略基準=従う
- **期待結果**: WHERE 句に `kykm.leakbn_id IN (1, 2)` が含まれる

---

### TC-002: GenerateWhereClause - リース区分「移転外のみ」

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `chk_LEAKBN_ITENGAI_F.Checked = True`、`chk_LEAKBN_OPE_F.Checked = False`
- **入力**: dtFrom="2024/04", dtTo="2025/03", 移転外チェック=True, OPEチェック=False
- **期待結果**: WHERE 句に `kykm.leakbn_id = 1` が含まれる。`IN (1, 2)` は含まれない

---

### TC-003: GenerateWhereClause - リース区分「オペのみ」

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `chk_LEAKBN_ITENGAI_F.Checked = False`、`chk_LEAKBN_OPE_F.Checked = True`
- **入力**: 移転外チェック=False, OPEチェック=True
- **期待結果**: WHERE 句に `kykm.leakbn_id = 2` が含まれる

---

### TC-004: GenerateWhereClause - 省略基準「従う」

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `radio_FOLLOW.Checked = True`
- **入力**: 省略基準=Follow
- **期待結果**: WHERE 句に `kykm.chuum_id = 1` が含まれる

---

### TC-005: GenerateWhereClause - 省略基準「無視する」

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `radio_IGNORE.Checked = True`（radio_FOLLOW=False, radio_OMISSION=False）
- **入力**: 省略基準=Ignore
- **期待結果**: WHERE 句に `chuum_id` 条件が含まれない（省略基準なし = 全件対象）

---

### TC-006: GenerateWhereClause - 省略基準「省略のみ」

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `radio_OMISSION.Checked = True`
- **入力**: 省略基準=Omission
- **期待結果**: WHERE 句に `kykm.chuum_id = 2` が含まれる

---

### TC-007: GenerateWhereClause - 物件No FROM/TO あり

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001, FR-005
- **種別**: 正常系
- **前提条件**: `txt_KYKM_NO_FROM.Text = "100"`, `txt_KYKM_NO_TO.Text = "200"`
- **入力**: 物件No FROM=100, TO=200
- **期待結果**: WHERE 句に `kykm.kykm_no >= @kyknNoFrom` および `kykm.kykm_no <= @kyknNoTo` が含まれる。パラメータ @kyknNoFrom=100, @kyknNoTo=200

---

### TC-008: GenerateWhereClause - 物件No 未入力

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001, FR-005
- **種別**: 正常系
- **前提条件**: `txt_KYKM_NO_FROM.Text = ""`、`txt_KYKM_NO_TO.Text = ""`
- **入力**: 物件No FROM="", TO=""
- **期待結果**: WHERE 句に `kykm_no` 条件が含まれない

---

### TC-009: GenerateWhereClause - 資産科目指定あり

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `cmb_SKMK_CD.SelectedValue = "1234"`
- **入力**: 資産科目コード="1234"
- **期待結果**: WHERE 句に `skmk.skmk_cd = @skmkCd` が含まれ、パラメータ @skmkCd="1234"

---

### TC-010: GenerateWhereClause - リース会社指定あり

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `cmb_LCPT1_CD.SelectedValue = "LC01"`
- **入力**: リース会社コード="LC01"
- **期待結果**: WHERE 句に `lcpt.lcpt1_cd = @lcptCd` が含まれ、パラメータ @lcptCd="LC01"

---

### TC-011: GenerateWhereClause - 管理部署指定あり

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: `cmb_BCAT1_CD.SelectedValue = "BC01"`
- **入力**: 管理部署コード="BC01"
- **期待結果**: WHERE 句に `b_bcat.bcat_cd = @bcatCd` が含まれ、パラメータ @bcatCd="BC01"

---

### TC-012: GenerateWhereClause - 日付パラメータ正規化

- **対象**: `Form_f_CHUKI_JOKEN.GenerateWhereClause`
- **関連要件**: US-001, FR-001
- **種別**: 正常系
- **前提条件**: dtFrom=2024/04（月中の任意日）, dtTo=2025/03（月中の任意日）
- **入力**: txt_DT_FROM.Value = #2024/04/15#, txt_DT_TO.Value = #2025/03/20#
- **期待結果**: パラメータ @dtFrom = 2024/04/01（月初）, @dtTo = 2025/03/31（月末）。WHERE句に `kykh.start_dt <= @dtTo AND kykh.end_dt >= @dtFrom` が含まれる

---

### TC-013: GenerateLabelText - 定額法・利息法の組み合わせ

- **対象**: `Form_f_CHUKI_JOKEN.GenerateLabelText`
- **関連要件**: US-002, FR-001
- **種別**: 正常系
- **前提条件**: `radio_TEIGAKU.Checked = True`、`radio_RISOKU.Checked = True`、dtFrom=2024/04, dtTo=2025/03
- **入力**: 償却方法=定額、利息計算=利息法
- **期待結果**: 戻り値に `"決算期間：2024/04～2025/03  "` が含まれる。`"償却方法：リース定額  "` が含まれる。`"利息計算：利息法  "` が含まれる。`"所有権移転外ファイナンスリースの計算条件  "` が常時含まれる（todo確認済み動作）

---

### TC-014: GenerateLabelText - 定率法・利子込法の組み合わせ

- **対象**: `Form_f_CHUKI_JOKEN.GenerateLabelText`
- **関連要件**: US-002, FR-001
- **種別**: 正常系
- **前提条件**: `radio_TEIGAKU.Checked = False`、`radio_RISOKU.Checked = False`
- **入力**: 償却方法=定率（radio_TEIGAKU=False）、利息計算=利子込法（radio_RISOKU=False）
- **期待結果**: `"償却方法：近似定率  "` が含まれる。`"利息計算：利子込法  "` が含まれる

---

### TC-015: GenerateLabelText - 期間テキスト同一月

- **対象**: `Form_f_CHUKI_JOKEN.GenerateLabelText`
- **関連要件**: US-002, FR-001, FR-005
- **種別**: 境界値
- **前提条件**: FROM=TO の同一月
- **入力**: dtFrom=2024/04, dtTo=2024/04
- **期待結果**: `"決算期間：2024/04～2024/04  "` が含まれる（FROM=TOの同一月でもエラーなし）

---

### TC-016: SwapIf 日付大小入れ替え (CHUKI)

- **対象**: `Form_f_CHUKI_JOKEN.cmd_EXECUTE_Click` 内の SwapIf ロジック
- **関連要件**: US-003, FR-001, FR-005
- **種別**: 境界値
- **前提条件**: FROM > TO の逆転入力
- **入力**: txt_DT_FROM.Value = #2025/03/31#, txt_DT_TO.Value = #2024/04/01#（FROM > TO）
- **期待結果**: SwapIf により DT_FROM が 2024/04/01、DT_TO が 2025/03/31 に入れ替わる

**注意**: `cmd_EXECUTE_Click` 内で `SwapIf` は MessageBox.Show の確認ダイアログより前に呼ばれるため、WinForms インスタンスのテストでは確認ダイアログをキャンセルしてから値を確認する。

---

### TC-017: GetBcatConditions - 管理部署1のみ True

- **対象**: `Form_f_flx_IDOLST.GetBcatConditions`
- **関連要件**: US-004, FR-001
- **種別**: 正常系
- **前提条件**: `CheckBcatFlags = {True, False, False, False, False}`
- **入力**: CheckBcatFlags[0]=True, [1..4]=False
- **期待結果**: 戻り値 = `"AND ( b_bcat.bcat1_cd <> r1_bcat.bcat1_cd OR b_bcat.bcat1_cd IS NULL OR r1_bcat.bcat1_cd IS NULL )"`

---

### TC-018: GetBcatConditions - 管理部署1・3 True（OR 結合）

- **対象**: `Form_f_flx_IDOLST.GetBcatConditions`
- **関連要件**: US-004, FR-001
- **種別**: 正常系
- **前提条件**: `CheckBcatFlags = {True, False, True, False, False}`
- **入力**: CheckBcatFlags[0]=True, [1]=False, [2]=True, [3..4]=False
- **期待結果**: 戻り値に `bcat1_cd` 条件と `bcat3_cd` 条件が `OR` で結合される（2条件）。具体的には `"AND ( b_bcat.bcat1_cd <> r1_bcat.bcat1_cd OR b_bcat.bcat1_cd IS NULL OR r1_bcat.bcat1_cd IS NULL OR b_bcat.bcat3_cd <> r1_bcat.bcat3_cd OR b_bcat.bcat3_cd IS NULL OR r1_bcat.bcat3_cd IS NULL )"`

---

### TC-019: GetBcatConditions - 全て False

- **対象**: `Form_f_flx_IDOLST.GetBcatConditions`
- **関連要件**: US-004, FR-001, FR-005
- **種別**: 異常系（バリデーション後の想定外ケース）
- **前提条件**: `CheckBcatFlags = {False, False, False, False, False}`
- **入力**: 全フラグ False
- **期待結果**: 戻り値 = `String.Empty`（空文字列）

---

### TC-020: GetBcatConditions - 全て True

- **対象**: `Form_f_flx_IDOLST.GetBcatConditions`
- **関連要件**: US-004, FR-001
- **種別**: 正常系
- **前提条件**: `CheckBcatFlags = {True, True, True, True, True}`
- **入力**: 全フラグ True
- **期待結果**: 戻り値に bcat1 から bcat5 の5条件が `OR` で結合される。`"AND ( "` で始まり `" )"` で終わる。`bcat1_cd`、`bcat2_cd`、`bcat3_cd`、`bcat4_cd`、`bcat5_cd` の各条件が含まれる

---

### TC-021: GetLabelText - 移動日期間テキスト

- **対象**: `Form_f_IDOLST_JOKEN.GetLabelText`
- **関連要件**: US-005, FR-001
- **種別**: 正常系
- **前提条件**: `txt_IDO_DT_FROM.Text = "2024/04/01"`、`txt_IDO_DT_TO.Text = "2024/06/30"`、管理部署1=True
- **入力**: 移動日FROM="2024/04/01", TO="2024/06/30", chk_BCAT1_F=True
- **期待結果**: 戻り値に `"移動日:　2024/04/01～2024/06/30  "` が含まれる

---

### TC-022: GetLabelText - 管理部署1のみ True

- **対象**: `Form_f_IDOLST_JOKEN.GetLabelText`
- **関連要件**: US-005, FR-001
- **種別**: 正常系
- **前提条件**: `chk_BCAT1_F.Checked = True`、他は False
- **入力**: BCAT1=True, BCAT2..5=False
- **期待結果**: `"管理部署1"` が含まれる。`"管理部署2"` が含まれない。末尾が `"、"` で終わらない

---

### TC-023: GetLabelText - 管理部署1・2 True（末尾「、」なし）

- **対象**: `Form_f_IDOLST_JOKEN.GetLabelText`
- **関連要件**: US-005, FR-001
- **種別**: 正常系（境界値）
- **前提条件**: `chk_BCAT1_F.Checked = True`、`chk_BCAT2_F.Checked = True`、他は False
- **入力**: BCAT1=True, BCAT2=True, BCAT3..5=False
- **期待結果**: `"管理部署1、管理部署2"` となり末尾が `"、"` で終わらない。（bcat2 が追加され bcat5 なしのため TrimEnd が機能するか確認）

---

### TC-024: GetLabelText - 管理部署5のみ True

- **対象**: `Form_f_IDOLST_JOKEN.GetLabelText`
- **関連要件**: US-005, FR-001
- **種別**: 正常系（境界値）
- **前提条件**: `chk_BCAT5_F.Checked = True`、他は False
- **入力**: BCAT1..4=False, BCAT5=True
- **期待結果**: `"管理部署5"` のみ含まれる。末尾が `"、"` で終わらない（bcat5 は直書きで「、」なし）

---

### TC-025: GetLabelText - 管理部署1・2・3・4 True（末尾「、」バグ確認）

- **対象**: `Form_f_IDOLST_JOKEN.GetLabelText`
- **関連要件**: US-005, FR-001
- **種別**: 異常系（既知バグ確認）
- **前提条件**: `chk_BCAT1_F` ～ `chk_BCAT4_F` = True、`chk_BCAT5_F` = False
- **入力**: BCAT1=True, BCAT2=True, BCAT3=True, BCAT4=True, BCAT5=False
- **期待結果（現状動作確認）**: コード上 bcat4 の後に `"、"` が付くが、bcat5 が False のため TrimEnd が呼ばれる。`"管理部署4、"` となるか `TrimEnd("、"c)` が正しく除去するか確認。期待値: `"管理部署1、管理部署2、管理部署3、管理部署4"` で末尾「、」なし
- **注記**: `TrimEnd("、"c)` は .NET において `"、"` が複数バイト文字のため `Char` 型として機能しない可能性あり（潜在バグ）。実際の動作を PASS/FAIL で記録し、Access版との不一致であれば FAIL とする

---

### TC-026: SwapIf 日付大小入れ替え (IDOLST)

- **対象**: `Form_f_IDOLST_JOKEN.cmd_EXECUTE_Click` 内の SwapIf ロジック
- **関連要件**: US-006, FR-001, FR-005
- **種別**: 境界値
- **前提条件**: FROM > TO の逆転入力、管理部署チェックあり
- **入力**: txt_IDO_DT_FROM.Value = #2024/06/30#, txt_IDO_DT_TO.Value = #2024/04/01#（FROM > TO）
- **期待結果**: SwapIf により IDO_DT_FROM が 2024/04/01、IDO_DT_TO が 2024/06/30 に入れ替わる

---

## 5. テストデータ設計

### 正常データ

| データ名 | 値 | 用途 |
|---|---|---|
| 標準集計期間 FROM | 2024/04/01 | 注記計算の期間開始日（月初） |
| 標準集計期間 TO | 2025/03/31 | 注記計算の期間終了日（月末） |
| 月中日付 FROM | 2024/04/15 | GetMonthStart 正規化確認 |
| 月中日付 TO | 2025/03/20 | GetMonthEnd 正規化確認 |
| 移動日 FROM | 2024/04/01 | 移動物件一覧の期間開始 |
| 移動日 TO | 2024/06/30 | 移動物件一覧の期間終了 |
| 物件No FROM | 100 | 物件No範囲条件下限 |
| 物件No TO | 200 | 物件No範囲条件上限 |
| 資産科目コード | "1234" | 資産科目フィルタ |
| リース会社コード | "LC01" | リース会社フィルタ |
| 管理部署コード | "BC01" | 管理部署フィルタ |

### 異常データ

| データ名 | 値 | 期待エラー/動作 |
|---|---|---|
| 日付未入力 (FROM) | Nothing | バリデーションメッセージ「必須項目が未入力です。」 |
| 日付未入力 (TO) | Nothing | バリデーションメッセージ「必須項目が未入力です。」 |
| リース区分全未選択 | ITENGAI=False, OPE=False | バリデーションメッセージ「リース区分が設定されていません。」 |
| 管理部署全未選択 | BCAT1..5=False | バリデーションメッセージ「管理部署1～5のうち少なくとも1つにチェックしてください。」 |
| CheckBcatFlags=Nothing | Nothing | GetBcatConditions が String.Empty を返す |

### 境界値データ

| データ名 | 値 | テスト観点 |
|---|---|---|
| FROM=TO（同一月） | 2024/04 ～ 2024/04 | 同一月の集計が正常動作するか |
| FROM > TO（逆転） | FROM=2025/03, TO=2024/04 | SwapIf が入れ替えるか |
| 物件No=0 | FROM=0, TO=0 | 最小値の境界確認 |
| 管理部署 BCAT4 のみ（末尾「、」） | BCAT4=True, BCAT5=False | TrimEnd の動作確認 |
| 管理部署 BCAT5 のみ | BCAT5=True, BCAT1..4=False | bcat5 が直書きの「、」なし形式になるか |
| 管理部署 全5つ True | BCAT1..5=True | 全条件 OR 結合が正しいか |

---

## 6. テストファイル構成

| テストファイルパス | テスト対象 | テストケース数 |
|---|---|---|
| `c:\kobayashi_LeaseM4BS\test_chuki_idolst_joken_blackbox.vb` | GenerateWhereClause, GenerateLabelText, GetBcatConditions, GetLabelText, SwapIf | 26 件 |

### テストモジュール構成

```vb
' コンパイル: vbc /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll /r:System.Windows.Forms.dll test_chuki_idolst_joken_blackbox.vb
' 実行: test_chuki_idolst_joken_blackbox.exe

Module TestChukiIdolstJokenBlackBox

    Dim passCount As Integer = 0
    Dim failCount As Integer = 0
    Dim skipCount As Integer = 0

    Sub Main()
        ' Part 1: CHUKI_JOKEN - GenerateWhereClause (TC-001 ～ TC-012)
        ' Part 2: CHUKI_JOKEN - GenerateLabelText   (TC-013 ～ TC-015)
        ' Part 3: CHUKI_JOKEN - SwapIf              (TC-016)
        ' Part 4: IDOLST - GetBcatConditions        (TC-017 ～ TC-020)
        ' Part 5: IDOLST_JOKEN - GetLabelText       (TC-021 ～ TC-025)
        ' Part 6: IDOLST_JOKEN - SwapIf             (TC-026)
    End Sub

    ' ヘルパー
    Sub Pass(label As String)       ' passCount++、PASS出力
    Sub Fail(label As String, ...)  ' failCount++、FAIL出力
    Sub Skip(label As String, ...)  ' skipCount++、SKIP出力
    Sub AssertContains(label, expected, actual) ' Contains判定ヘルパー
    Sub AssertNotContains(label, notExpected, actual)
    Sub AssertEqual(label, expected, actual)
End Module
```

---

## 7. 既存テストパターンとの整合性

### 採用する既存パターン

| パターン | 参照元 | 本テストでの適用 |
|---|---|---|
| `passCount`/`failCount`/`skipCount` カウンタ | `test_schedule_blackbox.vb` | 同形式で採用 |
| `Sub Test_XXX()` 形式 | `test_schedule_blackbox.vb` | 同形式で採用 |
| `Try/Catch` で FAIL 捕捉 | `test_keijo_joken_blackbox.vb` | 各テスト関数内で採用 |
| `FAIL が1件でも失敗時 ExitCode=1` | `test_schedule_blackbox.vb` | 採用 |
| `SKIP` パターン（DBスキーマ未完了時） | `test_schedule_blackbox.vb` | WinForms インスタンス化失敗時に適用 |
| `Console.OutputEncoding = UTF8` | 両既存ファイル | 先頭で設定 |

### WinForms 依存メソッドのテスト方式

`GenerateWhereClause`、`GenerateLabelText`、`GetLabelText` はフォームコントロールから値を読む設計のため、以下の方式でテストする。

1. フォームをインスタンス化する（`New Form_f_CHUKI_JOKEN()`）
2. コントロールのプロパティ値を直接設定する（例: `frm.chk_LEAKBN_ITENGAI_F.Checked = True`）
3. `Friend` に変更したメソッドを直接呼び出す
4. 戻り値の文字列に対して `Contains` または完全一致で検証する

WinForms の初期化が失敗する場合（STA スレッド要件等）は SKIP として記録する。

### モック/スタブの使い方

- DB 接続は不要（全テストが文字列生成ロジックのみ）
- `CrudHelper` のインスタンスはフォームがフィールドとして持つが、`GenerateWhereClause` / `GenerateLabelText` の実行パスでは呼ばれないため影響なし

### テストヘルパーの実装方針

```vb
Sub AssertContains(label As String, expected As String, actual As String)
    If actual.Contains(expected) Then
        Pass(label)
    Else
        Fail(label, $"Expected to contain [{expected}]", $"Actual: [{actual}]")
    End If
End Sub

Sub AssertNotContains(label As String, notExpected As String, actual As String)
    If Not actual.Contains(notExpected) Then
        Pass(label)
    Else
        Fail(label, $"Expected NOT to contain [{notExpected}]", $"Actual: [{actual}]")
    End If
End Sub

Sub AssertEqual(label As String, expected As String, actual As String)
    If actual = expected Then
        Pass(label)
    Else
        Fail(label, $"[{expected}]", $"[{actual}]")
    End If
End Sub
```

---

## 8. 既知の問題と SKIP 基準

| 問題 | 対応 |
|---|---|
| `leakbn_id IN (1, 2)` と `LeaseKbn.Itengai=3, Ope=4` の不一致 | 現行コードの動作（IN(1,2)）を期待値として設定し PASS/FAIL を記録。コード調査結果メモに「要確認」を記載する |
| `GetLabelText` の末尾「、」バグ（TC-025） | 現状動作を記録。Access版と不一致の場合は FAIL として ExitCode=1 に寄与させる |
| WinForms STA スレッド要件 | コンパイル時に `/main:TestChukiIdolstJokenBlackBox` を指定し STAThread 属性を Main に付与する |
| `GenerateWhereClause` / `GetLabelText` が `Private` | `Friend` への変更を前提。変更前はフォームインスタンス経由の間接テストで代替 |
