# Access版調査レポート — fc_計上仕訳_KITOKU / fc_支払仕訳_KITOKU

調査日: 2026-03-19
調査対象: `C:\access_LeaseM4BS\drive-download\AccessGUI\`

---

## 1. fc_支払仕訳_KITOKU

### 1.1 UIコントロール

| コントロール名 | 種別 | 内容 |
|---|---|---|
| `txt_SLIP_DT` | TextBox | 伝票日付 |
| `txt_SLIP_NO_START_VAL` | TextBox | 伝票番号開始値 |
| `txt_OUTPUT_FOLDER_NM` | TextBox | 出力先フォルダ |
| `txt_OUTPUT_FILE_NM` | TextBox | 出力ファイル名 |
| `txt_KAMOKU_CD` | TextBox | **科目コード**（例: 4160, または 4160-001 形式） |
| `txt_KAMOKU_NM` | TextBox | **科目名**（例: 未払金） |
| `txt_BSHO_CD` | TextBox | **補助科目コード** |
| `cmd_実行` | Button | 実行 |
| `cmd_CANCEL` | Button | キャンセル |
| `cmd_選択` | Button | フォルダ選択 |

**デフォルト値（初期設定時）:**
- KAMOKU_CD: `4160`
- KAMOKU_NM: `未払金`
- OUTPUT_FILE1_NM: `CMSW2WRK.tmp`
- OUTPUT_FILE2_NM: `APGDHWRK.tmp`
- OUTPUT_FILE3_NM: `APGDDWRK.tmp`
- OUTPUT_FILE4_NM: `APGDSWRK.tmp`

### 1.2 出力ファイル（4本）

| ファイル番号 | デフォルトファイル名 | 説明 |
|---|---|---|
| 1 | CMSW2WRK.tmp | 仕訳コアデータ |
| 2 | APGDHWRK.tmp | 支払AP ヘッダ |
| 3 | APGDDWRK.tmp | 支払AP 明細 |
| 4 | APGDSWRK.tmp | 支払AP 消費税 |

### 1.3 主要ビジネスロジック（関数一覧）

```
cmd_実行_Click()
  ├── m仕訳作成元データCHK()         ← データ前処理チェック（資産科目、費目決定要素CD等）
  ├── m仕訳()                        ← 仕訳データ作成メイン
  │   ├── m仕訳_売買現預金()          ← 売買処理（現預金）
  │   ├── m仕訳_売買未払金()          ← 売買処理（未払金）
  │   ├── m仕訳_賃貸消費税一括有現預金() ← 賃貸借（消費税一括有、計上月=支払月）
  │   ├── m仕訳_賃貸消費税一括有未払金() ← 賃貸借（消費税一括有、計上月<支払月）
  │   ├── m仕訳_賃貸消費税一括無現預金() ← 賃貸借（消費税一括無、計上月=支払月）
  │   └── m仕訳_賃貸消費税一括無未払金() ← 賃貸借（消費税一括無、計上月<支払月）
  ├── m仕訳データ出力(slFPATH1)      ← CMSW2WRK 出力
  ├── om仕訳出力.gAPGDHWRK出力(slFPATH2) ← APGDHWRK 出力
  ├── om仕訳出力.gAPGDDWRK出力(slFPATH3) ← APGDDWRK 出力
  └── om仕訳出力.gAPGDSWRK出力(slFPATH4) ← APGDSWRK 出力
```

### 1.4 pc_仕訳出力クラス（共通モジュール）

Access版では `pc_仕訳出力` クラス（`om仕訳出力`）が以下を担当:
- `gDivKMK_CD()` - 科目コード分割（KMK_CODE / HKM_CODE）
- `gOutputFile_Check()` - 出力ファイルパスの確認
- `gAPGDHWRK出力()` - APGDHWRK ファイル生成
- `gAPGDDWRK出力()` - APGDDWRK ファイル生成
- `gAPGDSWRK出力()` - APGDSWRK ファイル生成

### 1.5 データバリデーション（m仕訳作成元データCHK）

- 資産科目マスタの費目決定要素CD 未入力チェック
- HREL（費目関連テーブル）との結合チェック
- 科目コード未設定データの有無確認

### 1.6 伝票番号開始値の取得

- `Form_Load` で `tw_s_joken` または序列テーブルから `NEXTVAL` を取得
- 未払金科目コードは `-` 区切り（例: `4160-001` → KMK_CD=`4160`, HKM_CD=`001`）

---

## 2. fc_計上仕訳_KITOKU

### 2.1 UIコントロール

| コントロール名 | 種別 | 内容 |
|---|---|---|
| `txt_SLIP_DT` | TextBox | 伝票日付 |
| `txt_SLIP_NO_START_VAL` | TextBox | 伝票番号開始値 |
| `txt_OUTPUT_FOLDER_NM` | TextBox | 出力先フォルダ |
| `txt_OUTPUT_FILE_NM` | TextBox | 出力ファイル名 |
| `cmd_実行` | Button | 実行 |
| `cmd_CANCEL` | Button | キャンセル |
| `cmd_選択` | Button | フォルダ選択 |

> ※ 支払仕訳と異なり KAMOKU_CD / BSHO_CD フィールドは**存在しない**

### 2.2 出力ファイル（4本）

支払仕訳と**同じ4ファイル**:
- CMSW2WRK.tmp / APGDHWRK.tmp / APGDDWRK.tmp / APGDSWRK.tmp

### 2.3 主要ビジネスロジック（関数一覧）

```
cmd_実行_Click()
  ├── m仕訳作成元データCHK()         ← データ前処理チェック
  ├── m仕訳データ作成()              ← 計上仕訳データ作成メイン
  │   ├── m仕訳データ作成_開始計上() ← リース資産/リース債務 + 仮払消費税/リース未払金
  │   │   ├── m仕訳データ作成_開始計上1D() ← 借方（リース資産）
  │   │   ├── m仕訳データ作成_開始計上1C() ← 貸方（リース債務）
  │   │   ├── m仕訳データ作成_開始計上2D() ← 借方（仮払消費税）
  │   │   └── m仕訳データ作成_開始計上2C() ← 貸方（リース未払金）
  │   ├── m仕訳データ作成_税一括()    ← 消費税税一括（仮払消費税/リース未払金）
  │   │   ├── m仕訳データ作成_税一括D()
  │   │   └── m仕訳データ作成_税一括C()
  │   └── m仕訳データ作成_減価償却() ← 減価償却費/減価償却累計額
  ├── m仕訳データ出力(slFPATH1)      ← CMSW2WRK 出力
  ├── om仕訳出力.gAPGDHWRK出力(slFPATH2)
  ├── om仕訳出力.gAPGDDWRK出力(slFPATH3)
  └── om仕訳出力.gAPGDSWRK出力(slFPATH4)
```

### 2.4 科目コード取得ロジック

`om仕訳出力.gDivKMK_CD` を使い各レコードの科目CD・補助CDを動的に決定:
- SKMK_SUM2_CD（開始計上 借方）
- SKMK_SUM5_CD（開始計上 貸方）
- KMK_CD9（仮払消費税）
- SKMK_SUM6_CD（リース未払金）

---

## 3. fc_支払仕訳_KITOKU_SUB

### 3.1 UIコントロール

| コントロール名 | 内容 |
|---|---|
| `txt_ZRITU` | 消費税率 |
| `txt_ZEI_CD` | 税処理コード（ValidationRule: `gLenCheck() = True`） |

### 3.2 特記事項

- RecordSource: `"twc_fc_支払仕訳_KITOKU_SUB"` — 専用ワークテーブルを使用
- `txt_ZEI_CD` に `gLenCheck()` 関数バリデーションが適用
- Access版では親フォームからサブフォームとして呼び出し

---

## 4. Access版 全体特記事項

1. **設定テーブル**: `tw_s_joken` でフォームごとの設定を永続化（VB.NET の `t_settei` に相当）
2. **pc_仕訳出力 共通クラス**: APGD系出力は全て pc_仕訳出力 クラスの共通メソッドで実行。VB.NET には未実装
3. **固定長フォーマット**: .tmp 拡張子でテキスト出力（VB.NET は .txt で統一）
4. **伝票番号採番**: NEXTVAL 方式（シーケンス/採番テーブル使用）
