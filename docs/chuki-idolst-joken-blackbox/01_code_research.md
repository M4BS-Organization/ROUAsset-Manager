# コードベース調査資料: chuki-idolst-joken-blackbox (Issue #10)

## 1. プロジェクト概要

- **フレームワーク・言語**: VB.NET / .NET Framework 4.7.2 / WinForms
- **DB**: PostgreSQL (Npgsql 6.0.11)
- **Access版 (移行元)**: JET (Access MDB) + VBA

### ディレクトリ構成

```
C:\kobayashi_LeaseM4BS\
├── LeaseM4BS/
│   └── LeaseM4BS.DataAccess/          # 業務ロジック・データアクセス層
│       ├── ChukiCalcEngine.vb              # 注記計算エンジン (主対象)
│       ├── ScheduleTypes.vb               # ChukiCalcParams / ChukiCalcResult 型定義
│       ├── AmortizationScheduleBuilder.vb  # 償却スケジュール (CHUKI内部で使用)
│       ├── RepaymentScheduleBuilder.vb     # 返済スケジュール (CHUKI内部で使用)
│       ├── GsonScheduleBuilder.vb          # 減損スケジュール
│       ├── KeijoCalculationEngine.vb       # 計上計算エンジン
│       ├── MonthlyJournalEngine.vb         # 月次仕訳エンジン
│       ├── CrudHelper.vb                   # PostgreSQL汎用CRUD
│       └── DbConnectionManager.vb          # DB接続管理
├── LeaseM4BS.TestWinForms/              # テスト用WinForms (画面フォーム群)
│   └── LeaseM4BS.TestWinForms/
│       ├── Form_f_CHUKI_JOKEN.vb           # 財務諸表注記 条件画面 (主対象)
│       ├── Form_f_CHUKI_JOKEN.Designer.vb
│       ├── Form_f_flx_CHUKI.vb             # 財務諸表注記 一覧画面
│       ├── Form_f_CHUKI_SCH.vb             # 注記 返済スケジュール画面
│       ├── Form_f_CHUKI_YOUSHIKI.vb        # 注記様式集計画面
│       ├── Form_f_IDOLST_JOKEN.vb          # 移動物件一覧表 条件画面 (主対象)
│       ├── Form_f_IDOLST_JOKEN.Designer.vb
│       └── Form_f_flx_IDOLST.vb            # 移動物件一覧表 一覧画面
├── test_schedule_blackbox.vb            # スケジュール系ブラックボックステスト
├── test_keijo_joken_blackbox.vb         # 計上条件系ブラックボックステスト
└── test_type_safety_blackbox.vb         # 型安全性ブラックボックステスト
```

### 主要な依存ライブラリ
- Npgsql 6.0.11 (PostgreSQL ドライバ)
- System.Data (DataTable, DataRow)

---

## 2. アーキテクチャ概要

### アーキテクチャパターン
- **データアクセス層 (LeaseM4BS.DataAccess)**: 業務ロジック + DB操作を担う単一プロジェクト
- **プレゼンテーション層 (LeaseM4BS.TestWinForms)**: WinFormsフォーム群（557フォーム）
- 条件フォーム (JOKEN) が WHERE句またはパラメータを構築し、一覧フォーム (flx_) に渡すパターン

### レイヤー構成と責務

| レイヤー | クラス | 責務 |
|---|---|---|
| 条件フォーム | Form_f_CHUKI_JOKEN | 注記計算の条件入力・検証・WHERE句生成 |
| 条件フォーム | Form_f_IDOLST_JOKEN | 異動一覧の条件入力・検証・パラメータ渡し |
| 一覧フォーム | Form_f_flx_CHUKI | 注記計算結果の一覧表示 |
| 一覧フォーム | Form_f_flx_IDOLST | 移動物件一覧表示 |
| 業務ロジック | ChukiCalcEngine | 注記計算メイン (Access版 pc_注記.m注記計算_main 移植) |
| 型定義 | ScheduleTypes.vb | ChukiCalcParams, ChukiCalcResult, ShiharaiSchEntry 等 |

---

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS.TestWinForms/Form_f_CHUKI_JOKEN.vb` | 注記条件フォーム (WHERE句生成) | 高 |
| `LeaseM4BS.TestWinForms/Form_f_IDOLST_JOKEN.vb` | 移動一覧条件フォーム | 高 |
| `LeaseM4BS.TestWinForms/Form_f_flx_CHUKI.vb` | 注記一覧 (SQL組立て・表示) | 高 |
| `LeaseM4BS.TestWinForms/Form_f_flx_IDOLST.vb` | 移動物件一覧 (SQL組立て・表示) | 高 |
| `LeaseM4BS.DataAccess/ChukiCalcEngine.vb` | 注記計算エンジン本体 | 高 |
| `LeaseM4BS.DataAccess/ScheduleTypes.vb` | ChukiCalcParams, ChukiCalcResult 型定義 | 高 |
| `LeaseM4BS.DataAccess/RepaymentScheduleBuilder.vb` | 返済スケジュール (CHUKI内部) | 中 |
| `LeaseM4BS.DataAccess/AmortizationScheduleBuilder.vb` | 償却スケジュール (CHUKI内部) | 中 |
| `LeaseM4BS.DataAccess/MonthlyJournalEngine.vb` | 月次仕訳エンジン (CHUKI含む) | 中 |
| `test_schedule_blackbox.vb` | ChukiCalcEngine 既存テスト (Part 5) | 高 |
| `test_keijo_joken_blackbox.vb` | 条件フォーム系テストの参考パターン | 高 |
| `AccessVBA/Form_f_CHUKI_JOKEN.txt` | Access版 VBAコード (移行元) | 高 |
| `AccessVBA/Form_f_IDOLST_JOKEN.txt` | Access版 VBAコード (移行元) | 高 |

---

## 4. 使用パターン

### 状態管理
- 条件フォームは `_prevForm` フィールドで前回集計結果フォームへの参照を保持
- 一覧フォームは `WhereClause` / `Prms` (CHUKI) または `DtFrom`, `DtTo`, `CheckBcatFlags` (IDOLST) をプロパティで受け取る

### ルーティング (フォーム遷移)
```
Form_f_CHUKI_JOKEN
  ├── cmd_EXECUTE_Click → GenerateWhereClause / GenerateLabelText → Form_f_flx_CHUKI.ShowDialog()
  ├── cmd_ZENKAI_Click → _prevForm.ShowDialog()  (前回結果再表示)
  └── cmd_CANCEL_Click → Me.Close()

Form_f_IDOLST_JOKEN
  ├── cmd_EXECUTE_Click → GetLabelText / DtFrom,DtTo,CheckBcatFlags 設定 → Form_f_flx_IDOLST.ShowDialog()
  ├── cmd_ZENKAI_Click → _prevForm.ShowDialog()
  └── cmd_CANCEL_Click → Me.Close()
```

### データアクセス
- **CHUKI一覧**: `Form_f_flx_CHUKI.BuildSql` が WHERE句を組み立て `CrudHelper.GetDataTable` で DataTable 取得
- **IDOLST一覧**: `Form_f_flx_IDOLST.BuildSql` が `@dtFrom`/`@dtTo` を NpgsqlParameter で渡し `CrudHelper.GetDataTable` で取得
- **注記計算**: `ChukiCalcEngine.Calculate(params, shiharaiSch, gsonSchedule, crud)` を呼び出す (MonthlyJournalEngine 経由)

### エラーハンドリング
- MessageBox.Show でユーザー通知
- Try/Catch で DB例外をキャッチ (`Form_f_flx_IDOLST.SearchData` 等)

---

## 5. 既存の類似機能分析

### Form_f_CHUKI_JOKEN (注記条件フォーム) の現在の実装状況

**実装済み**:
- `Form_Load`: コンボボックス (資産科目/リース会社/管理部署) のデータバインド
- `cmd_EXECUTE_Click`: 必須チェック (日付)、リース区分チェック、SwapIf (大小修正)、WHERE句生成、Form_f_flx_CHUKI 呼び出し
- `cmd_CLEAR_Click`: 各コントロールの初期化
- `cmd_ZENKAI_Click`: 前回集計結果表示
- `GenerateWhereClause`: 全条件 (日付/リース区分/省略基準/物件No/資産科目/リース会社/管理部署) を NpgsqlParameter で構築
- `GenerateLabelText`: ラベルテキスト生成

**未実装・TODO**:
- `GenerateLabelText` 内 `' todo どの条件でもなぜか表示されるテキスト` (ライン161): 「所有権移転外ファイナンスリースの計算条件」が常時表示される点がAccess版と一致するか要確認
- `Form_f_flx_CHUKI.BuildSql` 内の多数のコメントアウトされた SELECT カラム (減価償却累計額相当額/期末残高相当額/未経過リース料期末残高/当期支払リース料 等) が未実装。現状は取得価額相当額・現金購入価額・総額リース料・注記判定結果のみ

### Form_f_IDOLST_JOKEN (移動一覧条件フォーム) の現在の実装状況

**実装済み**:
- `cmd_EXECUTE_Click`: 管理部署チェック (1～5 少なくとも1つ)、SwapIf、Form_f_flx_IDOLST 呼び出し
- `cmd_CANCEL_Click` / `cmd_ZENKAI_Click`
- `GetLabelText`: 移動日範囲・管理部署チェックカテゴリのラベルテキスト生成
- `Form_f_flx_IDOLST.BuildSql`: kykm.ido_dt の範囲条件 + bcat条件 (管理部署コードの不一致・NULL検出)
- `Form_f_flx_IDOLST.GetBcatConditions`: 管理部署チェックフラグ → OR 結合で SQL 生成

**未実装・TODO**:
- `Form_f_IDOLST_JOKEN_Load` が空 (Access版では `tw_S_IDOLST_JOKEN` テーブルから設定を読み込んでいたが、VB.NET版ではフォームに初期値を直接持つ)
- `GetLabelText` 内 `' todo 「、」で終わるパターン` (ライン68): 最後の「、」を TrimEnd で除去しているが、bcat5 のみ末尾「、」なしで直書きしており、4つ選択時に「、」で終わるバグが残る可能性 (要検証)

### Access版との差分 (主要)

#### CHUKI_JOKEN

| 項目 | Access版 | VB.NET版 |
|---|---|---|
| 設定保存 | `tw_S_CHUKI_JOKEN` テーブルに保存 (Form_Load で読み込み) | フォームの初期値のみ (永続化なし) |
| 計算実行 | `pc_注記.g注記計算_纏めtoワークTBL` を呼び出し | `Form_f_flx_CHUKI` に WHERE 句を渡す (計算エンジン呼び出しは MonthlyJournalEngine 経由で別途実装) |
| 省略基準 | `opg_SHORYAK_KIJUN` (1=従う, 2=無視, 3=省略物件のみ) | `radio_FOLLOW` / `radio_IGNORE` / `radio_OMISSION` の3択 (VB.NET版は IGNORE 未実装の可能性あり) |
| 計算月数表示 | `txt_GETU_CNT` フィールドに月数を自動計算 | `txt_DURATION` は存在するが自動更新ロジック未確認 |
| リース区分 | ITENGAI(=3) / OPE(=4) | `leakbn_id IN (1, 2)` ← **要注意: ID値が Access版と異なる** |

**重要な不一致候補**: VB.NET版 `GenerateWhereClause` では `leakbn_id IN (1, 2)` を使用しているが、Access版では `cngLEAKBN_ITENGAI` と `cngLEAKBN_OPE` の定数値が使われており、DB のマスタ値と一致するか確認が必要。`ScheduleTypes.vb` の `LeaseKbn` enum では `Itengai = 3, Ope = 4` と定義されているため、フォームの WHERE 句が `IN (1, 2)` では**誤り**の可能性が高い。

#### IDOLST_JOKEN

| 項目 | Access版 | VB.NET版 |
|---|---|---|
| 設定保存 | `tw_S_IDOLST_JOKEN` テーブルに保存 | 永続化なし |
| 移動履歴 | `IDO_DT_R0` ～ `IDO_DT_R3` を4世代まで追跡 | `kykm.ido_dt` のみ (単一世代) ← **重大な差分** |
| 部署変更検出 | 各世代の `B_BCAT_CDx_Ry` を比較 | `b_bcat.bcatN_cd <> r1_bcat.bcatN_cd OR NULL` のみ (R1 は `b_bcat_id_r1` 経由) |
| カスタマイズ分岐 | `igCUSTM_TYPE` によるメッセージ切り替え | なし |

---

## 6. 技術的制約・注意事項

### CHUKI_JOKEN

1. **leakbn_id の値マッピング**
   - `Form_f_CHUKI_JOKEN.vb:108` の `IN (1, 2)` と `ScheduleTypes.vb` の `LeaseKbn.Itengai=3, Ope=4` に不一致がある。`c_leakbn` テーブルの実際の ID 値を確認する必要がある。

2. **注記計算結果カラム未実装**
   - `Form_f_flx_CHUKI.BuildSql` のコメントアウト部分 (ライン65-79) が未実装。ブラックボックステストでは ChukiCalcEngine の出力値 (ChukiCalcResult プロパティ) の正確性を検証することが優先。

3. **ChukiCalcEngine は実装済み**
   - `ChukiCalcEngine.Calculate` は `ScheduleTypes.ChukiCalcParams` を引数に取り `ChukiCalcResult` を返す。Access版の `m注記計算_main` + `SUB_資産関連` + `SUB_返済関連` を忠実移植済み。
   - `test_schedule_blackbox.vb` の Part 5 (Test_Chuki_* 4件) で既に基本テスト済み。

4. **KessanBi≠31 バグ再現**
   - `ChukiCalcEngine.vb:58-67` に Access版のコピペバグを意図的に再現した分岐あり。`ig決算日=31` 以外ではy4/y5変数が未初期化のまま。

### IDOLST_JOKEN

1. **移動履歴世代の差分**
   - Access版 `mIDOLST_Main` は `IDO_DT_R0` ～ `IDO_DT_R3` の4世代ループ処理を行うが、VB.NET版 `Form_f_flx_IDOLST.BuildSql` は現在の移動日 (`kykm.ido_dt`) と直前の部署ID (`kykm.b_bcat_id_r1`) のみを参照している。複数世代移動ロジックは未移植。

2. **NULL安全性**
   - `Form_f_flx_IDOLST.GetBcatConditions` (ライン183): `b_bcat.bcatN_cd IS NULL OR r1_bcat.bcatN_cd IS NULL` を条件に含めており、部署コードが NULL の行も出力する。Access版準拠。

3. **GetLabelText の末尾「、」バグ**
   - `Form_f_IDOLST_JOKEN.vb:86` で bcat5 のみ末尾「、」なし。bcat4 を最後に選択した場合「管理部署4、」で終わり TrimEnd が機能しないケースが残る可能性あり。

---

## 7. 既存テストパターンの分析

### 既存テストファイル構造

| ファイル | モジュール名 | 対象 | スタイル |
|---|---|---|---|
| `test_schedule_blackbox.vb` | `TestScheduleBlackBox` | スケジュール系 | Sub/Function、passCount/failCount/skipCount カウンタ |
| `test_keijo_joken_blackbox.vb` | `TestKeijoJokenBlackBox` | KEIJO条件系 | Function戻りBoolean、allPassed フラグ |
| `test_type_safety_blackbox.vb` | `TestTypeSafetyBlackBox` | 型安全性系 | Sub/Function、passCount/failCount/skipCount カウンタ |

### CHUKI ブラックボックステストの既存カバレッジ

`test_schedule_blackbox.vb` Part 5 で以下がカバー済み:
- `Test_Chuki_Itengai_Basic`: 移転外F・12M・定額法・利子抜法の基本計算
- `Test_Chuki_Ope_NullFields`: OPEリースの Null フィールド検証
- `Test_Chuki_Iten_NoShisan`: 移転リースの資産関連スキップ
- `Test_Chuki_NonMonthEnd_KessanBug`: KessanBi=28 のコピペバグ再現

### 未カバー領域 (新テストで追加すべき)

#### CHUKI_JOKEN 条件ロジック
- WHERE句生成の検証: GenerateWhereClause の各条件組み合わせ (leakbn_id, chuum_id, kykm_no, skmk_cd, lcpt1_cd, bcat_cd)
- GenerateLabelText の出力値検証
- 日付の大小入れ替え (SwapIf) の検証
- 必須チェック (日付未入力) の検証
- リース区分未選択バリデーション

#### IDOLST_JOKEN 条件ロジック
- GetBcatConditions の SQL 生成検証 (フラグ組み合わせ: 全OFF/1つ/複数/全ON)
- GetLabelText の出力値検証 (末尾「、」バグ含む)
- SwapIf の検証 (日付大小入れ替え)
- 管理部署全OFF のバリデーション

#### ChukiCalcEngine 追加テスト
- 中途解約フラグ (BCkaiykF=True) の期末残高クリア検証
- MatsubiShuryoKichuMasshoF=False (翌期抹消モード) の分岐検証
- 前期末残高 (LgnpnZzan) と当期末残高 (LgnpnZan) の正確性
- リースバック損益 (LbSoneki) あり/なしの検証

### テストファイルのコンパイル方法

```bash
# 既存テストのコンパイル例
vbc /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll test_keijo_joken_blackbox.vb
vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_schedule_blackbox.vb

# 新テストのコンパイル (予定)
vbc /r:LeaseM4BS.DataAccess.dll /r:Npgsql.dll /r:System.Data.dll test_chuki_idolst_joken_blackbox.vb
```

---

## 8. Access版 VBA コードの要点

### Form_f_CHUKI_JOKEN (Access版)

- **RecordSource**: `tw_S_CHUKI_JOKEN` (フォームのデータソース = 設定保存テーブル)
- **Form_Load**: `tw_S_CHUKI_JOKEN` にレコードがなければデフォルト値 (`RCALC_ID=RisokuHo`, `SKYAK_HO_ID=Teigaku`) を INSERT して初期化
- **実行ボタン**: `pc_注記.g注記計算_纏めtoワークTBL` を呼び出して計算後、`f_flx_CHUKI` を開く
- **省略基準**: `opg_SHORYAK_KIJUN` (1=従う, 2=無視, 3=省略物件のみ) ← VB.NET版は3段階ラジオボタンで対応済み
- **月数表示**: `mCALC_GETU_CNT` で期間月数を `txt_GETU_CNT` に自動更新

### Form_f_IDOLST_JOKEN (Access版)

- **RecordSource**: `tw_S_IDOLST_JOKEN`
- **Form_Load**: `BCAT1_F=True` のデフォルトレコードを初期化
- **実行処理 `mIDOLST_Main`**:
  - `tw_S_IDOLST` ワークテーブルを DELETE → INSERT で作成
  - メインSQL: `D_KYKM` × `D_KYKH` × `M_BCAT` (R0～R3 の4世代LEFT JOIN) で全移動履歴を取得
  - `IDO_DT_R0` ～ `IDO_DT_R3` の4世代ループで移動日範囲チェック
  - 部署変更検出: `B_BCAT[1-5]_CD_R[i]` と `B_BCAT[1-5]_CD_R[i+1]` を比較
  - 最終世代 (cnlRCNT=3) は移動前部署が NULL の場合も対象
- **後処理 `m後処理`**: `tw_S_IDOLST` に `KKNRI_NM`, `KKBN_NM`, `LCPT_NM` 等を UPDATE で補完

**Access版の移動履歴追跡の重要点**:
```
R0 = 現在の部署 (kykm.B_BCAT_ID)
R1 = 1つ前の部署 (kykm.B_BCAT_ID_R1)
R2 = 2つ前の部署 (kykm.B_BCAT_ID_R2)
R3 = 3つ前の部署 (kykm.B_BCAT_ID_R3)
```
VB.NET版は R0/R1 のみ実装。R2/R3 を扱う複数世代移動は未移植。
ブラックボックステストでは現行実装 (R0/R1) の正確性を確認し、R2/R3 は SKIP 扱いとする。

---

## 9. 命名規則・コーディング規約

- ファイル命名: `Form_f_<機能名>.vb` (条件フォーム=`_JOKEN`, 一覧=`f_flx_`)
- フォームクラス: `Partial Public Class Form_f_CHUKI_JOKEN`
- コントロール命名: Access版と同一 (`txt_`, `chk_`, `cmb_`, `cmd_`, `radio_`, `lbl_`)
- パラメータ: `@camelCase` (NpgsqlParameter)
- ヘルパーメソッド: フォーム内 Private Function / Sub (例: `GenerateWhereClause`, `GetBcatConditions`)
- テストモジュール: `Module Test<機能名>BlackBox` / `passCount`, `failCount`, `skipCount` カウンタ + `AssertEqual` / `Pass` / `Fail` / `Skip` ヘルパー
