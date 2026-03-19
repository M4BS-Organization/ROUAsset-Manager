# VB.NET 開発中コード調査レポート — Issue #26 fc_系仕訳出力フォーム共通基盤

調査日: 2026-03-19

---

## 1. FcJournalOutputBase（基底クラス）

**ファイル:** `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FcJournalOutputBase.vb`
**継承元:** `System.Windows.Forms.Form`
**修飾子:** `MustInherit`

### 1.1 フィールド

| フィールド | 型 | 説明 |
|---|---|---|
| `_crud` | `CrudHelper` | DB操作ヘルパー（`Protected`） |

### 1.2 MustOverride（抽象）メンバー

| メンバー | 種別 | シグネチャ | 説明 |
|---|---|---|---|
| `CustomerCode` | ReadOnly Property | `As String` | 顧客コード（KITOKU等） |
| `SwkKbn` | ReadOnly Property | `As String` | 仕訳区分（支払仕訳等） |
| `BuildInsertToWrkSql` | Function | `(kikanFrom As Date) As String` | tw_fc_swk_wrk への INSERT SQL 構築 |
| `WriteOutputFile` | Function | `(dt As DataTable, outputFolder As String) As String` | ファイル出力（固定長/CSV/Excel） |

### 1.3 Overridable プロパティ

| プロパティ | デフォルト値 | 説明 |
|---|---|---|
| `OutputFileNameBase` | `$"fc_{CustomerCode}_{SwkKbn}"` | 出力ファイル名ベース |

### 1.4 実装済みメソッド（Protected/Public）

| メソッド | シグネチャ | 説明 |
|---|---|---|
| `Execute` | `(outputFolder As String) As String` | Template Method メイン処理 |
| `ValidateJokenAndGetKikanFrom` | `(ByRef kikanFrom As Date) As Boolean` | tw_s_keijo_joken 存在確認・kikanFrom 取得 |
| `ClearWorkTable` | `()` | tw_fc_swk_wrk の当顧客データ DELETE |
| `ValidateOutputFolder` | `(outputFolder As String) As Boolean` | 出力フォルダ存在チェック |
| `ConfirmExecute` | `() As Boolean` | 実行確認ダイアログ |
| `Dispose` | `(disposing As Boolean)` | CrudHelper の破棄 |

### 1.5 Template Method の処理フロー（`Execute`）

```
1. ValidateJokenAndGetKikanFrom() → tw_s_keijo_joken から kikanFrom 取得
2. ClearWorkTable()               → tw_fc_swk_wrk の当顧客・当区分を DELETE
3. BuildInsertToWrkSql()          → 【抽象】SQL構築（サブクラスで実装）
4. _crud.ExecuteNonQuery()        → SQL実行（tw_fc_swk_wrk に INSERT）
5. 件数チェック                   → COUNT(*) == 0 なら警告して終了
6. SELECT * FROM tw_fc_swk_wrk    → 出力対象データ取得
7. WriteOutputFile()              → 【抽象】ファイル出力（サブクラスで実装）
```

### 1.6 TODOコメント・未実装箇所

なし。基底クラスは完成状態。

---

## 2. Form_fc_支払仕訳_KITOKU

**ファイル:** `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_fc_支払仕訳_KITOKU.vb`
**継承元:** `FcJournalOutputBase`

### 2.1 UI要素（Designer.vb から）

**ボタン（3個）:**
| コントロール | Text | 機能 |
|---|---|---|
| `cmd_実行` | "実行(&R)" | 仕訳出力実行 |
| `cmd_CANCEL` | "ｷｬﾝｾﾙ(&C)" | フォーム閉じる |
| `cmd_選択` | "選択" | 出力フォルダ選択 |

**テキストボックス（10個）:**
| コントロール | 説明 |
|---|---|
| `txt_SLIP_DT` | 伝票日付 |
| `txt_SLIP_NO_START_VAL` | 伝票番号開始値 |
| `txt_KAMOKU_CD` | 科目コード |
| `txt_KAMOKU_NM` | 科目名称 |
| `txt_BSHO_CD` | 部署コード（一括） |
| `txt_OUTPUT_FOLDER_NM` | 出力先フォルダ |
| `txt_OUTPUT_FILE1_NM` | 出力ファイル1（CMSW2WRK） |
| `txt_OUTPUT_FILE2_NM` | 出力ファイル2（APGDHWRK） |
| `txt_OUTPUT_FILE3_NM` | 出力ファイル3（APGDDWRK） |
| `txt_OUTPUT_FILE4_NM` | 出力ファイル4（APGDSWRK） |

**ラベル（17個）:**
- lbl_SLIP_DT（伝票日付）、lbl_SLIP_NO_START_VAL（伝票番号開始値）
- lbl_KAMOKU（科目）、lbl_KAMOKU_CD（ｺｰﾄﾞ）、lbl_KAMOKU_NM（名称）
- txt_KAMOKU（未払金 — 固定テキストラベル）
- lbl_BSHO_CD（部署ｺｰﾄﾞ(一括)）
- lbl_OUTPUT_FOLDER_NM（出力先ﾌｫﾙﾀﾞ）
- lbl_EXPLANATION1〜5（説明文）
- lbl_OUTPUT_FILE1_NM〜lbl_OUTPUT_FILE4_NM（ファイル説明）
- ラベル25（＜税処理ｺｰﾄﾞ＞）

**フォーム設定:** 555x563、CenterParent、タイトル「支払仕訳出力画面」

### 2.2 抽象プロパティ実装

| プロパティ | 値 |
|---|---|
| `CustomerCode` | `"KITOKU"` |
| `SwkKbn` | `"支払仕訳"` |

### 2.3 設定キー定数

```
SLIP_DT, SLIP_NO_START_VAL, KAMOKU_CD, KAMOKU_NM, BSHO_CD,
OUTPUT_FOLDER, OUTPUT_FILE1_NM, OUTPUT_FILE2_NM, OUTPUT_FILE3_NM, OUTPUT_FILE4_NM
```

### 2.4 実装済み機能

| 機能 | メソッド | 状態 |
|---|---|---|
| フォームLoad | `Form_fc_支払仕訳_KITOKU_Load` | 実装済み（joken検証 + 設定読込） |
| 実行ボタン | `cmd_実行_Click` | 実装済み（バリデーション → ExecuteKitoku → 設定保存） |
| キャンセル | `cmd_CANCEL_Click` | 実装済み |
| フォルダ選択 | `cmd_選択_Click` | 実装済み（FolderBrowserDialog） |
| 設定読み込み | `LoadSettings` | 実装済み（10項目） |
| 設定保存 | `SaveSettings` | 実装済み（10項目） |
| FormClosed | `Form_fc_支払仕訳_KITOKU_FormClosed` | 実装済み（_settei?.Dispose()） |

### 2.5 BuildInsertToWrkSql の SQL 内容

**INSERT先テーブル:** `tw_kitoku_cmsw2wrk`（基底クラスの tw_fc_swk_wrk ではない）

**SQL構造:** UNION ALL で借方行・貸方行を生成

**借方行（sw2_dc_kbn='1'）:**
- ソース: `tw_s_chuki_keijo k LEFT JOIN t_haifu_keijo h ON h.lcpt_id = k.lcpt_id AND h.kjkbn_id = k.kjkbn_id`
- 条件: `k.kjkbn_id = 1 AND k.rec_kbn IN (1, 3) AND k.keijo_f = TRUE`
- 金額: `k.lsryo`（税抜）、税額: `k.zei`
- 科目: `h.dr_kmk_cd` / `h.dr_hkm_cd`

**貸方行（sw2_dc_kbn='2'）:**
- 同一ソース・条件
- 金額: `k.lsryo + k.zei`（税込）
- 科目: 入力値 `kamokuCd`（未払金科目コード）

**パラメータ:**
- `slipDt`: txt_SLIP_DT.Text（デフォルト: 今日）
- `slipNoStart`: txt_SLIP_NO_START_VAL.Text（デフォルト: "1"）
- `kamokuCd`: txt_KAMOKU_CD.Text
- `bshoCd`: txt_BSHO_CD.Text

**注意:** SQL内の日付・コードはパラメータ化されておらず、文字列補間で直接埋め込み（SQLインジェクションリスクあり、ただし内部用途）。

### 2.6 4ファイル出力の実装状況

**ExecuteKitoku メソッド（独自メイン処理）:**

```
1. ValidateJokenAndGetKikanFrom()  → kikanFrom 取得
2. ClearKitokuWorkTables()         → 4テーブル DELETE
3. BuildInsertToWrkSql()           → tw_kitoku_cmsw2wrk へ INSERT
4. 件数チェック                    → COUNT(*) == 0 なら終了
5. BuildApgdhWrk()                 → tw_kitoku_apgdhwrk へ INSERT（集計）
6. BuildApgddWrk()                 → tw_kitoku_apgddwrk へ INSERT（借方明細）
7. BuildApgdsWrk()                 → tw_kitoku_apgdswrk へ INSERT（支払情報）
8. 4テーブルそれぞれ SELECT → 4ファイル出力
```

| ファイル | ワークテーブル | 出力メソッド | フォーマット定義 |
|---|---|---|---|
| File1 (.txt) | tw_kitoku_cmsw2wrk | `WriteOutputFile` | `GetCMSW2WRKFields()` (52フィールド) |
| File2 (.txt) | tw_kitoku_apgdhwrk | `WriteApgdhFile` | `GetAPGDHWRKFields()` (72フィールド) |
| File3 (.txt) | tw_kitoku_apgddwrk | `WriteApgddFile` | `GetAPGDDWRKFields()` (31フィールド) |
| File4 (.txt) | tw_kitoku_apgdswrk | `WriteApgdsFile` | `GetAPGDSWRKFields()` (62フィールド) |

**APGD系ワークテーブル生成SQL:**

| メソッド | INSERT SQL | 説明 |
|---|---|---|
| `BuildApgdhWrk` | `INSERT INTO tw_kitoku_apgdhwrk (gdh_den_no, gdh_den_date, gdh_kei_kin) SELECT sw2_den_no, sw2_date, SUM(sw2_kin) FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '2' GROUP BY sw2_den_no, sw2_date` | 貸方行を伝票番号で集計 |
| `BuildApgddWrk` | `INSERT INTO tw_kitoku_apgddwrk (gdd_den_no, gdd_den_date, gdd_gyo_no, gdd_kmk_code, gdd_nuki_kin, gdd_zei_kin) SELECT sw2_den_no, sw2_date, sw2_gyo_no, sw2_kmk_code, sw2_kin, sw2_zei_kin FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '1'` | 借方行をそのまま転記 |
| `BuildApgdsWrk` | `INSERT INTO tw_kitoku_apgdswrk (gds_den_no, gds_den_date, gds_gyo_no, gds_sh_kin) SELECT sw2_den_no, sw2_date, sw2_gyo_no, sw2_kin FROM tw_kitoku_cmsw2wrk WHERE sw2_dc_kbn = '2'` | 貸方行の支払情報を転記 |

**注意:** 基底クラスの `Execute()` Template Method は直接使わず、独自の `ExecuteKitoku()` を使用している（4テーブル×4ファイルの特殊パターンのため）。

### 2.7 TODOコメント

1. **L117:** `TODO: Access版 m仕訳データ作成（支払仕訳）と照合して正確な列マッピングに更新する`
2. **L270:** `TODO: Access版 gAPGDHWRK作成 相当。tw_kitoku_cmsw2wrk を集計して INSERT する。`
3. **L279:** `TODO: Access版 gAPGDDWRK作成 相当。借方明細行を INSERT する。`
4. **L287:** `TODO: Access版 gAPGDSWRK作成 相当。支払情報を INSERT する。`

---

## 3. Form_fc_計上仕訳_KITOKU

**ファイル:** `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_fc_計上仕訳_KITOKU.vb`
**継承元:** `FcJournalOutputBase`

### 3.1 UI要素（Designer.vb から）

**ボタン（3個）:** cmd_実行、cmd_CANCEL、cmd_選択

**テキストボックス（4個）:**
| コントロール | 説明 |
|---|---|
| `txt_SLIP_DT` | 伝票日付 |
| `txt_SLIP_NO_START_VAL` | 伝票番号開始値 |
| `txt_OUTPUT_FOLDER_NM` | 出力先フォルダ |
| `txt_OUTPUT_FILE_NM` | 出力ファイル名（1ファイルのみ） |

**ラベル（6個）:**
- lbl_SLIP_DT（伝票日付）、lbl_SLIP_NO_START_VAL（伝票番号開始値）
- lbl_OUTPUT_FOLDER_NM（出力先ﾌｫﾙﾀﾞ）
- lbl_EXPLANATION3（ﾌｧｲﾙ名）、lbl_EXPLANATION4（CORE）
- lbl_OUTPUT_FILE_NM（その他システム用仕訳ワーク）

**フォーム設定:** 555x245、CenterParent、タイトル「計上仕訳出力画面」

### 3.2 抽象プロパティ実装

| プロパティ | 値 |
|---|---|
| `CustomerCode` | `"KITOKU"` |
| `SwkKbn` | `"計上仕訳"` |

### 3.3 設定キー定数

```
KEIJO_SLIP_DT, KEIJO_SLIP_NO_START_VAL, KEIJO_OUTPUT_FOLDER, KEIJO_OUTPUT_FILE_NM
```

### 3.4 実装済み機能

支払仕訳と同様の構造。主な違い:
- 出力ファイルは CMSW2WRK の **1本のみ**（APGD系不要）
- 独自メソッド `ExecuteKeijo()` で直接 tw_kitoku_cmsw2wrk を操作

### 3.5 BuildInsertToWrkSql の SQL 内容

**INSERT先テーブル:** `tw_kitoku_cmsw2wrk`

**SQL構造:** UNION ALL で借方行・貸方行を生成

**条件の違い（支払仕訳との比較）:**
| 項目 | 支払仕訳 | 計上仕訳 |
|---|---|---|
| kjkbn_id | 1（費用） | 2（資産） |
| rec_kbn | IN (1, 3) | IN (1, 2, 3) |
| 貸方科目 | 入力値（未払金） | `h.cr_kmk_cd` / `h.cr_hkm_cd`（t_haifu_keijo から） |
| INSERT列数 | 22列（sw2_tori_kbn, sw2_tori_code 含む） | 20列（sw2_tori_kbn, sw2_tori_code なし） |

### 3.6 TODOコメント

1. **L77:** `TODO: Access版 m仕訳データ作成（計上仕訳）と照合して正確な列マッピングに更新する`

---

## 4. Form_fc_支払仕訳_KITOKU_SUB

**ファイル:** `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_fc_支払仕訳_KITOKU_SUB.vb`
**継承元:** `System.Windows.Forms.Form`（FcJournalOutputBase ではない）

### 4.1 UI要素

| コントロール | 型 | 説明 |
|---|---|---|
| `txt_ZRITU` | TextBox | 消費税率入力 |
| `txt_ZEI_CD` | TextBox | 税処理コード入力 |
| `テキスト48` | Label | "消費税率" |
| `テキスト63` | Label | "税処理ｺｰﾄﾞ" |

**フォーム設定:** 300x200、CenterParent、タイトル「消費税率」

### 4.2 機能

| プロパティ/メソッド | 説明 |
|---|---|
| `Zritu As Double` | 入力された消費税率（Public Property） |
| `ZeiCd As String` | 入力された税処理コード（Public Property） |
| `Confirm()` | 値確定して DialogResult.OK で閉じる |
| `txt_ZEI_CD_KeyDown` | Enter で Confirm() 呼び出し |
| `txt_ZRITU_KeyDown` | Enter で txt_ZEI_CD にフォーカス移動 |

**注意:** 親フォーム（Form_fc_支払仕訳_KITOKU）からの呼び出しコードは現時点で未実装。SUB フォームの表示トリガーが見当たらない。

---

## 5. FcSetteiHelper

**ファイル:** `LeaseM4BS/LeaseM4BS.DataAccess/FcSetteiHelper.vb`
**インターフェース:** `IDisposable`

### 5.1 対応テーブル・プレフィックス

- **テーブル:** `t_settei`
- **プレフィックス形式:** `FC_{CustomerCode}_` （例: `FC_KITOKU_OUTPUT_FOLDER`）

### 5.2 実装済みメソッド一覧

| メソッド | シグネチャ | 説明 |
|---|---|---|
| **コンストラクタ** | `New(customerCode As String)` | CrudHelper 自動生成版 |
| **コンストラクタ** | `New(customerCode As String, crud As CrudHelper)` | テスト用 DI 版 |
| `GetText` | `(key, defaultValue) As String` | テキスト設定値取得 |
| `GetNumber` | `(key, defaultValue) As Double` | 数値設定値取得 |
| `LoadAll` | `() As List(Of SetteiRecord)` | 全設定レコード取得 |
| `SetText` | `(key, value, nameJpn)` | テキスト値 UPSERT |
| `SetNumber` | `(key, value, nameJpn)` | 数値 UPSERT |
| `SaveAll` | `(records As List(Of SetteiRecord))` | 一括保存（DELETE + INSERT、トランザクション対応） |
| `Dispose` | `()` | CrudHelper 破棄 |

**Private ヘルパー:**
- `GetSetteiRow(key)` → `settei_nm = PREFIX + KEY` で LIMIT 1 取得
- `UpsertSettei(...)` → `INSERT ... ON CONFLICT (settei_nm) DO UPDATE SET ...`
- `InsertSetteiRecord(rec)` → 個別 INSERT（SaveAll 内で使用）
- `RowToRecord(row)` → DataRow → SetteiRecord 変換

### 5.3 TODOコメント

なし。完成状態。

---

## 6. KitokuFixedLengthFormats

**ファイル:** `LeaseM4BS/LeaseM4BS.DataAccess/KitokuFixedLengthFormats.vb`
**種別:** `Module`（Public Module）

### 6.1 フォーマット定数

| 定数 | 値 | 用途 |
|---|---|---|
| `FMT_KIN` | `"000000000000000000.000"` | 金額（22バイト） |
| `FMT_RATE` | `"00000.000000000000"` | レート（18バイト） |
| `FMT_GYONO` | `"00000"` | 行番号（5バイト） |
| `FMT_ZEI_SPECIAL` | `"0.0"` | 税額特殊（3バイト） |

### 6.2 各ワークテーブルのフィールド定義

#### GetCMSW2WRKFields() — 52フィールド
Access版 gCMSW2WRK出力() Lines 146-197 準拠

| # | フィールド名 | バイト幅 | フォーマット |
|---|---|---|---|
| 1 | SW2_KAI_CODE | 5 | 文字列 |
| 2 | SW2_DATE | 10 | 文字列 |
| 3 | SW2_DEN_NO | 8 | 文字列 |
| 4 | SW2_GYO_NO | 5 | 00000 |
| 5 | SW2_DC_KBN | 1 | 文字列 |
| 6-8 | SW2_KMK_CODE / HKM_CODE / BMN_CODE | 各10 | 文字列 |
| 9-12 | SW2_CODE1〜CODE4 | 各10 | 文字列 |
| 13 | SW2_KIN | 22 | FMT_KIN |
| 14 | SW2_ZEI_CODE | 4 | 文字列 |
| 15 | SW2_ZEI_KBN | 1 | 文字列 |
| 16 | SW2_ZEI_KIN | 22 | FMT_KIN |
| 17 | SW2_CUR_CODE | 3 | 文字列 |
| 18 | SW2_RATE_TYPE | 2 | 文字列 |
| 19 | SW2_RATE | 18 | FMT_RATE |
| 20 | SW2_CUR_KIN | 22 | FMT_KIN |
| 21-22 | SW2_TEKIYO1 / TEKIYO2 | 各40 | 文字列 |
| 23-27 | SW2_GRP_CODE〜SYS_GRP_CODE | 2,2,8,2,2 | 文字列 |
| 28-29 | SW2_AIT_KMK_CODE / AIT_HKM_CODE | 各10 | 文字列 |
| 30-35 | SW2_USR_ID1〜SYS_DATE2 | 10,3,10,10,3,10 | 文字列 |
| 36-41 | SW2_SHONIN_KBN〜TORI_CODE | 1,1,1,1,1,20 | 文字列 |
| 42-49 | SW2_YOBI_CHAR1〜CHAR8 | 10,10,10,10,20,20,20,20 | 文字列 |
| 50-52 | SW2_YOBI_NUM1〜NUM3 | 各22 | FMT_KIN |

**合計レコード長: 571バイト**

#### GetAPGDHWRKFields() — 72フィールド
Access版 gAPGDHWRK出力() Lines 266-346 準拠

主要フィールド:
- GDH_KAI_CODE(5), GDH_DEN_KBN(2), GDH_GRP_CODE(2), GDH_DEN_NO(8), GDH_DEN_DATE(10)
- GDH_KEI_KIN(22, FMT_KIN), GDH_SAI_KIN(22, FMT_KIN) 等
- GDH_SAI_ZEI_KIN(22, FMT_ZEI_SPECIAL) — 特殊フォーマット "0.0"
- 予備フィールド YOBI_CHAR1-8, YOBI_NUM1-3
- GDH_SOSAI_RATE_TYPE(2), GDH_SOSAI_RATE(18, FMT_RATE), GDH_SOSAI_KIN(22)

#### GetAPGDDWRKFields() — 34フィールド（コメントでは31と記載だが実際は34）
Access版 gAPGDDWRK出力() Lines 417-451 準拠

主要フィールド:
- GDD_KAI_CODE(5), GDD_DEN_KBN(2), GDD_GRP_CODE(2), GDD_DEN_NO(8), GDD_DEN_DATE(10)
- GDD_GYO_NO(5, FMT_GYONO)
- GDD_NUKI_KIN(22, FMT_KIN), GDD_ZEI_KIN(22, FMT_KIN)
- GDD_RATE_TYPE(2), GDD_RATE(18, FMT_RATE) — 末尾に追加

#### GetAPGDSWRKFields() — 63フィールド（コメントでは62と記載だが実際は63）
Access版 gAPGDSWRK出力() Lines 519-581 準拠

主要フィールド:
- GDS_KAI_CODE(5), GDS_DEN_KBN(2), GDS_GRP_CODE(2), GDS_DEN_NO(8), GDS_DEN_DATE(10)
- GDS_GYO_NO(5, FMT_GYONO), GDS_SH_KIN(22, FMT_KIN)
- 振込先情報（BNK_CODE, BRH_CODE, KOZ_SYUBETSU, KOZ_NO, KOZ_NAME 等）
- 手形情報（TEGA系）
- 預り情報（AZU系）
- GDS_TEGA_NO_EDA(5) — 末尾

### 6.3 TODOコメント

なし。完成状態。

---

## 7. FixedLengthFileWriter

**ファイル:** `LeaseM4BS/LeaseM4BS.DataAccess/FixedLengthFileWriter.vb`

### 7.1 WriteFile の実装内容

```vb
Public Shared Sub WriteFile(filePath, dt, fields)
  Using sw As New StreamWriter(filePath, False, Sjis)  ' Shift-JIS エンコーディング
    For Each row In dt.Rows
      sw.WriteLine(BuildRecord(row, fields))
    Next
  End Using
End Sub
```

**エンコーディング:** Shift-JIS（`Encoding.GetEncoding("Shift_JIS")`）

### 7.2 実装済みメソッド

| メソッド | アクセス | 説明 |
|---|---|---|
| `WriteFile` | Public Shared | DataTable → 固定長ファイル出力 |
| `BuildRecord` | Public Shared | 1行分のレコード文字列生成（テスト用に Public） |
| `FormatField` | Private Shared | フィールド値のフォーマット + パディング |
| `FormatValue` | Private Shared | 値にフォーマット適用（Access版 `Format(Nz())` 相当） |
| `PadRightByte` | Public Shared | Shift-JIS バイト単位の右パディング（Access版 `gStrSizeAdjust()` 相当） |

**フォーマットロジック:**
- `FormatString` あり → 数値として扱い（Null→0）、`numVal.ToString(formatString)` で整形
- `FormatString` なし → 文字列として扱い（Null→""）
- バイト幅超過時 → Shift-JIS バイト単位で切り捨て
- バイト幅不足時 → 半角スペースで右パディング

### 7.3 TODOコメント

なし。完成状態。

---

## 8. SQLテーブル定義

### 8.1 tw_fc_swk_wrk（共通ワークテーブル）

**ファイル:** `sql/007_tw_fc_common_tables.sql`

汎用的な仕訳ワーク。customer_cd + swk_kbn で顧客・区分を識別。
FcJournalOutputBase の Template Method から使用される想定。
**ただし、KITOKU フォームはこのテーブルを使わず、tw_kitoku_cmsw2wrk を直接使用している。**

### 8.2 KITOKU固有ワークテーブル

**ファイル:** `sql/004_tw_kitoku_tables.sql`

| テーブル | フィールド数 | 説明 |
|---|---|---|
| `tw_kitoku_cmsw2wrk` | 52列（+id） | 伝票ワーク（レコード長571バイト） |
| `tw_kitoku_apgdhwrk` | 72列（+id） | 金額概要ワーク |
| `tw_kitoku_apgddwrk` | 34列（+id） | 金額詳細ワーク |
| `tw_kitoku_apgdswrk` | 63列（+id） | 支払ワーク |

---

## 9. 全体的なTODO/未実装サマリー

### 9.1 TODOコメント一覧

| # | ファイル | 行 | TODO内容 | 重要度 |
|---|---|---|---|---|
| 1 | Form_fc_支払仕訳_KITOKU.vb | L117 | Access版 m仕訳データ作成（支払仕訳）と照合して正確な列マッピングに更新する | **高** |
| 2 | Form_fc_支払仕訳_KITOKU.vb | L270 | Access版 gAPGDHWRK作成 相当。tw_kitoku_cmsw2wrk を集計して INSERT する。 | 中（実装済みだが照合未完） |
| 3 | Form_fc_支払仕訳_KITOKU.vb | L279 | Access版 gAPGDDWRK作成 相当。借方明細行を INSERT する。 | 中（実装済みだが照合未完） |
| 4 | Form_fc_支払仕訳_KITOKU.vb | L287 | Access版 gAPGDSWRK作成 相当。支払情報を INSERT する。 | 中（実装済みだが照合未完） |
| 5 | Form_fc_計上仕訳_KITOKU.vb | L77 | Access版 m仕訳データ作成（計上仕訳）と照合して正確な列マッピングに更新する | **高** |

### 9.2 構造的な課題・未実装箇所

1. **基底クラスと KITOKU の関係:** FcJournalOutputBase は `tw_fc_swk_wrk` を使う Template Method を提供するが、KITOKU は `tw_kitoku_cmsw2wrk` を使う独自フロー（`ExecuteKitoku`/`ExecuteKeijo`）を採用。基底クラスの `Execute()` メソッドは KITOKU からは呼ばれない。

2. **SUB フォームの呼び出し未実装:** `Form_fc_支払仕訳_KITOKU_SUB` は消費税率・税処理コード入力用だが、親フォーム（Form_fc_支払仕訳_KITOKU）からの呼び出しコードが存在しない。

3. **SQL パラメータ化不足:** `BuildInsertToWrkSql` 内で `slipDt`, `kamokuCd`, `bshoCd`, `slipNoStart` が文字列補間で直接 SQL に埋め込まれている（`NpgsqlParameter` 未使用）。

4. **APGD系テーブルの列不足:** `BuildApgdhWrk`, `BuildApgddWrk`, `BuildApgdsWrk` では各テーブルの一部の列しか INSERT していない。72列中3列、34列中6列、63列中4列のみ。残りは NULL/デフォルト値。Access版との照合が必要。

5. **フィールド数の不一致（コメント vs 実際）:**
   - APGDDWRK: コメント「31フィールド」 → 実際は 34 フィールド（rate_type, rate 追加）
   - APGDSWRK: コメント「62フィールド」 → 実際は 63 フィールド（tega_no_eda 追加）
