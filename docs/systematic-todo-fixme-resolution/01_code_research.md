# コードベース調査レポート: systematic-todo-fixme-resolution

## 1. TODO/FIXME 一覧

### CRITICAL

| # | ファイルパス | 行番号 | 内容 | 理由 |
|---|---|---|---|---|
| C1 | `Form_f_CHUKI_RECALC.vb:30` | 30 | `' todo 危険(実装不明、要確認)` | ResetChukiData() は全契約の再計算。誤実行すると全データ書き換え。実装は概ねされているが、コメント付きの変数（`gnzaiKt`, `ksanRitu`, `chuHntiId`, `chuumId`, `kjkbnId`, `szeiKjkbnId`）が未計算のまま除外されている。|
| C2 | `Form_BuknEntry.vb:305` | 305 | `' todo 削除処理` | 物件の削除ボタンを押しても実際には削除されない。確認ダイアログ後にメッセージボックスを表示して閉じるだけ。d_kykm の参照整合性に影響する。 |

### HIGH

| # | ファイルパス | 行番号 | 内容 | 理由 |
|---|---|---|---|---|
| H1 | `Form_f_flx_BUKN.vb:48` | 48 | `' sb.AppendLine(" AS 保守料, ") ' todo 該当項目不明` | 物件フレックス一覧の「保守料」列が未出力。対応するDB列名が不明。`d_kykm` か `d_haif` にあるはずだが特定できていない。 |
| H2 | `Form_f_flx_CHUKI.vb:65-83` | 65-83 | 減価償却累計額相当額・未経過リース料残高相当額 (1年内/1年超/合計) など多数の列がコメントアウト | 注記フレックス一覧に必要な財務諸表注記項目が多数未実装。`ChukiCalcEngine` / `KlsryoCalculationEngine` は実装済みのため、クエリ結果との結合が必要。 |
| H3 | `Form_f_CHUKI_SCH.vb:101,236` | 101, 236 | `' todo` → `@ghassei{i} AS 発生リース料` の計算が `CalcKlsryo` のコピーのみ | `CalcGhassei()` が `Return CalcKlsryo(dt)` と仮実装。発生リース料は支払リース料と異なる定義（利子込法では一致だが、利息法では元本返済額のみになる）。 |
| H4 | `Form_f_flx_TOUGETSU.vb:85,87,88,98,115` | 85-115 | 「行区」「法令区分」「取引区分」「請求月」がコメントアウト、WHERE句の条件も不足 | `Form_f_flx_KEIJO.vb` では法令区分・請求月の実装済み例あり（行:46-65）。同様の実装が TOUGETSU でも必要。WHERE句に集計月・開始日・終了日・中途解約日条件を追加する必要あり。 |
| H5 | `Form_f_flx_YOSAN.vb:42,61,63,94,106,201` | 42-201 | グレーアウト条件不明、「予想/既存」列・「行区」列の値不明、検索条件の`todo` | `Form_f_flx_KHIYO.vb:253-274` にグレーアウト実装例あり（開始日≦DtTo AND 終了日≧DtFrom の範囲外で計上額0のものをグレーに）。「予想/既存」はAccess版のフラグ、「行区」(定額/変額)は `rec_kbn` 相当。 |

### MEDIUM

| # | ファイルパス | 行番号 | 内容 | 理由 |
|---|---|---|---|---|
| M1 | `Form_f_CHUKI_JOKEN.vb:160,266` | 160, 266 | `' todo どの条件でもなぜか表示されるテキスト` (「所有権移転外ファイナンスリースの計算条件」) | 所有権移転外ファイナンスリースの条件チェックが実装されていない。本来はリース区分チェックの状態に応じて表示/非表示を切り替えるべき。 |
| M2 | `Form_f_IDOLST_JOKEN.vb:62,112` | 62, 112 | `TrimEnd("、"c)` の動作確認 (bcat4=T, bcat5=F のケース) と「、で終わるパターン」 | `GetLabelText()` の実装と `GetLabelTextPure()` の実装で、bcat4=T・bcat5=F の時は `TrimEnd("、"c)` が正常動作する。ただし Issue #10 でのテスト未確認。 |
| M3 | `Form_fc_TC_HREL.vb:78,157,163` | 78-163 | 「全入れ替え方式は自動採番の時不具合になる可能性あり」「どの主キーが重複しているか出力したい」 | 配賦明細の全件削除→再登録方式はシーケンス採番と相性が悪い。現在 `kykm_id` は MAX+1 方式のため問題は顕在化しにくいが、変更時のリスクがある。 |
| M4 | `Form_fc_TC_HREL_YOBI.vb:129,251,291` | 129-291 | 「重複するデータが入力されています」のメッセージが重複箇所を特定しない | UI改善レベル。現状は動作するが、どの行が重複しているか表示できると良い。 |
| M5 | `Form_f_BEPPYO2_REP.vb:20` | 20 | `' todo 印刷物を作成する(FiscalYear, DtFrom, DtToも使う)` | 別表2レポートの印刷実装が完全に空。PrintDocument の PrintPage ハンドラが未実装。 |
| M6 | `Form_f_flx_BUKN.vb:22` | 22 | `' todo: 項目が正しいか確認` | 物件フレックスSQL全体の項目確認。現在は必要そうな項目は揃っているが、Access版との突き合わせ確認が未完了。 |
| M7 | `Form_f_T_KARI_RITU.vb:29` | 29 | `' todo 検索項目考える` | 借入利率マスタの検索条件が単に `kari_ritu_id <> 0` のみ。適切な検索条件（利率名、日付範囲等）の追加検討。 |
| M8 | `Form_f_T_ZEI_KAISEI.vb:30` | 30 | `' todo 検索項目考える` | 税率改正マスタの検索条件が `zei_kaisei_id <> 0` のみ。 |

### LOW

| # | ファイルパス | 行番号 | 内容 | 理由 |
|---|---|---|---|---|
| L1 | `Form_f_flx_YOSAN.vb:94` | 94 | `' todo シンプルな比較文にしたい` | `m0 + m1 + ... + m23 > 0` の条件式。機能的に問題なし。 |
| L2 | `Form_f_flx_YOSAN.vb:201` | 201 | `'todo AS yyyy/MM` | YOSAN の月別列ヘッダーを `yyyy/MM` 形式にしたい。現状は `m0`, `m1` 等。 |
| L3 | `Form_BuknEntry.vb:188` | 188 | `' todo 適切なメソッド名` | メソッド名の命名改善。動作に影響なし。 |
| L4 | `Form_fc_TC_HREL.vb:78` | 78 | `' todo 適切なメソッド名` | 同上。 |
| L5 | `Form_fc_TC_HREL_YOBI.vb:129` | 129 | `' todo 適切なメソッド名` | 同上。 |
| L6 | `Form_f_CHUKI_SCH.vb:265` | 265 | `' 描画処理本体 todo印刷` | 印刷プレビュー実装は存在する (`PrintDocument1_PrintPage`) が、内容の完成度確認が必要。機能的にはほぼ動作する形になっている。 |
| L7 | `Form_f_IMPORT_CONTRACT_FROM_EXCEL.vb:44` など | 33-44 | `'todo ファイル入力` | EXCEL取込系フォーム4種が未実装。`Form_MAIN.vb` でも「EXCEL取込機能は未実装です」と表示している。スコープ外の可能性大。 |
| L8 | `Form_MAIN.vb:810-906` | 810-906 | `MessageBox.Show("XXX機能は未実装です。")` が多数 | システムタブの各種管理機能（データ保存/復元/エクスポート、DB最適化/作成/削除、ログ管理等）が未実装。いずれもスコープ外の可能性が高い。 |

---

## 2. Access版対応コード

Access版 (`C:\access_LeaseM4BS`) は `.mdb` / `.accdr` 形式のバイナリのみ存在し、VBA ソースは直接参照不可。

ただし、VB.NET側のコメントやクラス名から、以下の対応関係が判明している:

| VB.NET クラス/フォーム | Access版 モジュール/プロシージャ |
|---|---|
| `KeijoCalculationEngine` | `pc_SHRI_KEIJO.gKEIJO_Main` |
| `MonthlyJournalEngine` | `pc_月次仕訳計上.g仕訳計算_仕訳to計上テーブル_計上` |
| `KlsryoCalculationEngine` | `pc_SHRI_KLSRYO.gKLSRYO_Main` |
| `ChukiCalcEngine` | `pc_注記.m注記計算_main` |
| `GsonScheduleBuilder` | `pc_注記.gMake減損_SCH` |
| `CashScheduleBuilder` | `pc_SHRI_COM.gMake_CASH_SCH` |
| `RepaymentScheduleBuilder` | `pc_返済_SCH` 相当 |
| `AmortizationScheduleBuilder` | `pc_償却_SCH` 相当 |
| `DbConnectionManager` | `DAO.Database` (JET Engine) |
| `CrudHelper` | `DAO.Recordset` |

### 重要な注意点

- **leakbn_id の値不一致**: `Form_f_CHUKI_JOKEN.vb:183-184` のコメントより、`c_leakbn` テーブルの実ID (1=所有権移転外, 2=オペレーティング) が、`ScheduleTypes.LeaseKbn` enum (Itengai=3, Ope=4) と異なる。WHERE句では実テーブルIDを使用する必要あり。
- **ChukiCalcEngine の月末日決算以外の分岐**: `ChukiCalcEngine.vb:58` のコメントに「Access版にコピペバグあり。Access版と完全一致させるため同じ動作を再現する」とあり、意図的な再現バグを含む。
- **SEKOU_DT**: 法令区分の新法/旧法判定に使用する施行日は `t_settei` テーブルの `settei_nm = 'SEKOU_DT'` から取得 (2008/04/01)。`Form_f_flx_KEIJO.vb:51` に実装例あり。

---

## 3. コードアーキテクチャ

### 3.1 ディレクトリ構成（主要ファイル）

```
c:\kobayashi_LeaseM4BS\
├── LeaseM4BS\
│   └── LeaseM4BS.DataAccess\          # データアクセス・計算エンジン層
│       ├── DbConnectionManager.vb     # PostgreSQL接続管理 (DAO.Database代替)
│       ├── CrudHelper.vb              # CRUD汎用ヘルパー (DAO.Recordset代替)
│       ├── KeijoCalculationEngine.vb  # 計上計算エンジン (pc_SHRI_KEIJO相当)
│       ├── MonthlyJournalEngine.vb    # 月次仕訳計上エンジン
│       ├── KlsryoCalculationEngine.vb # 期間リース料計算エンジン (pc_SHRI_KLSRYO相当)
│       ├── ChukiCalcEngine.vb         # 注記計算エンジン (pc_注記相当)
│       ├── CashScheduleBuilder.vb     # キャッシュスケジュール生成
│       ├── RepaymentScheduleBuilder.vb # 返済スケジュール生成
│       ├── AmortizationScheduleBuilder.vb # 償却スケジュール生成
│       ├── GsonScheduleBuilder.vb     # 減損スケジュール生成
│       ├── KeijoWorkTableManager.vb   # 計上ワークテーブル管理
│       ├── KeijoTypes.vb              # 計上エンジン型定義 (KeijoJoken, KeijoResult等)
│       ├── KlsryoTypes.vb             # KLSRYO型定義 (RecKbn, KlsryoResult等)
│       ├── ScheduleTypes.vb           # スケジュール共通型定義
│       ├── KeijoSqlBuilder.vb         # 計上SQL生成
│       ├── KitokuFixedLengthFormats.vb # 既得固定長ファイル形式
│       ├── FixedLengthFileWriter.vb   # 固定長ファイル出力
│       ├── SetteiHelper.vb            # 設定値ヘルパー (t_settei操作)
│       └── UsageExamples.vb           # 使用例
│
└── LeaseM4BS.TestWinForms\
    └── LeaseM4BS.TestWinForms\        # WinForms UIレイヤー
        ├── Form_MAIN.vb               # メインメニュー
        ├── Form_ContractEntry.vb      # 契約書入力 (d_kykh/d_kykm)
        ├── Form_BuknEntry.vb          # 物件入力 (d_kykm)
        ├── Form_f_flx_KEIJO.vb        # 計上フレックス一覧
        ├── Form_f_flx_KHIYO.vb        # 費用フレックス一覧
        ├── Form_f_flx_KLSRYO.vb       # 期間リース料フレックス一覧
        ├── Form_f_flx_CHUKI.vb        # 注記フレックス一覧
        ├── Form_f_flx_BUKN.vb         # 物件フレックス一覧
        ├── Form_f_flx_TOUGETSU.vb     # 当月フレックス一覧
        ├── Form_f_flx_YOSAN.vb        # 予算フレックス一覧
        ├── Form_f_CHUKI_RECALC.vb     # 注記再計算
        ├── Form_f_CHUKI_SCH.vb        # 注記スケジュール
        ├── Form_f_CHUKI_JOKEN.vb      # 注記条件設定
        └── Form_f_IDOLST_JOKEN.vb     # 移動物件一覧条件
```

### 3.2 アーキテクチャパターン

**2層構成** (Access版の継続設計):
- **DataAccess層** (`LeaseM4BS.DataAccess.dll`): PostgreSQL接続、CRUD操作、計算エンジン
- **WinForms UI層** (`LeaseM4BS.TestWinForms.exe`): 画面描画、SQL構築、DataAccess層呼び出し

### 3.3 CrudHelper の主要 API

```vb
' データ取得
_crud.GetDataTable(sql, params)         ' → DataTable

' スカラ値取得
_crud.ExecuteScalar(Of T)(sql, params)  ' → T

' 更新系
_crud.ExecuteNonQuery(sql, params)      ' → Integer (影響行数)
_crud.Insert("テーブル名", Dictionary)  ' キー/値辞書でINSERT
_crud.Update("テーブル名", Dictionary, "WHERE句") ' UPDATE

' トランザクション
_crud.BeginTransaction()
_crud.Commit()
_crud.Rollback()
```

### 3.4 DB接続

- **エンジン**: PostgreSQL (Npgsql 4.x)
- **接続設定**: `App.config` の `connectionStrings["LeaseM4BS"]` → 環境変数 `LEASE_M4BS_CONNECTION_STRING` → ハードコードデフォルト
- **接続先 (デフォルト)**: `localhost:5432/lease_m4bs`

### 3.5 主要なデータテーブル

| テーブル | 内容 |
|---|---|
| `d_kykh` | 契約ヘッダ (1契約 = 1行) |
| `d_kykm` | 契約明細/物件 (1物件 = 1行) |
| `d_haif` | 配賦明細 |
| `d_gson` | 減損情報 |
| `t_settei` | システム設定値 (SEKOU_DT, leakbn_id 等) |
| `c_leakbn` | リース区分コード (1=所有権移転外, 2=OPE) |
| `c_kjkbn` | 計上区分コード |
| `c_kkbn` | 契約区分コード |
| `m_lcpt` | 支払先マスタ |
| `m_bcat` | 管理部署マスタ |
| `tw_s_chuki_keijo` | 注記計上ワークテーブル |
| `tw_d_henl_keijo` | 変額仕訳ワークテーブル |

---

## 4. 依存関係と推奨実装順序

### 優先度マトリクス

```
依存なし ─────────────────────────── 依存あり
LOW影響                              HIGH影響
    │                                    │
    L7(EXCEL取込)                         C2(BuknEntry削除)
    L8(MAIN未実装メニュー)                H1(保守料項目特定)
    L1-6(軽微TODO)                        H2(CHUKI列追加) ← ChukiCalcEngine済み
    M7,M8(検索条件)                       H3(発生リース料定義)
    M3,M4(UI改善)                         H4(TOUGETSU列・WHERE条件)
    M1(CHUKI_JOKEN表示)                   H5(YOSAN グレーアウト)
    M2(IDOLST TrimEnd確認)                C1(CHUKI_RECALC全体確認)
```

### 推奨実装順

1. **Phase 1 - CRITICAL** (データ破損リスク回避)
   - `C2`: `Form_BuknEntry` の削除処理実装
     - `d_kykm` の DELETE SQL を `Form_ContractEntry.cmd_DELETE_Click` と同様のパターンで実装
   - `C1`: `Form_f_CHUKI_RECALC` のコメント変数の洗い出しと確認
     - コメント中の `gnzaiKt`, `ksanRitu`, `kjkbnId` 等が本当に不要なのかを確認

2. **Phase 2 - HIGH (独立実装可能なもの)**
   - `H4`: `Form_f_flx_TOUGETSU` の未実装列追加とWHERE句補完
     - `Form_f_flx_KEIJO.vb` の法令区分・請求月実装 (行46-65) を参考に実装可能
   - `H5`: `Form_f_flx_YOSAN` のグレーアウト条件
     - `Form_f_flx_KHIYO.vb:253-274` のパターンを移植
   - `H1`: `Form_f_flx_BUKN` の保守料列
     - `d_haif` テーブルの `h_ijiknr` か `d_kykm.b_ijiknr` が相当する可能性が高い

3. **Phase 3 - HIGH (計算エンジン連携)**
   - `H2`: `Form_f_flx_CHUKI` の未実装列追加
     - `ChukiCalcEngine` / `KlsryoCalculationEngine` の出力結果と JOIN する
   - `H3`: `Form_f_CHUKI_SCH` の発生リース料計算実装
     - 利子込法 → 支払リース料と同値、利息法 → 元本返済相当額 (スケジュールから取得)

4. **Phase 4 - MEDIUM/LOW**
   - `M1`, `M2` のUI確認
   - `L1`-`L6` の軽微なリファクタリング

---

## 5. 補足メモ

### 5.1 Form_f_flx_KEIJO の前月末残高・当月末残高

`Form_f_flx_KEIJO.vb:75-99` に実装済み:
- **前月末残高**: `ROUND(k_slsryo - (k_slsryo / mkaisu) * kj_shri_cnt, 0)` (均等割)
- **当月末残高**: `ROUND(k_slsryo - (k_slsryo / mkaisu) * (kj_shri_cnt + 1), 0)` (均等割)
- 注意: 均等割近似のため、スケジュールベースの厳密計算とは誤差が出る可能性あり。

### 5.2 Form_f_flx_KHIYO のグレーアウト実装済み

`Form_f_flx_KHIYO.vb:253-274` に実装済み:
```vb
' 期間内(start_dt <= DtTo AND end_dt >= DtFrom)、または計上額 != 0 → 通常表示
' それ以外 → グレー (ForeColor=Gray, BackColor=RGB(240,240,240))
```
`Form_f_flx_YOSAN` でも同パターンを適用できる。

### 5.3 Form_f_flx_KLSRYO の行区・法令区分・取引区分

`Form_f_flx_KLSRYO.vb` は `KlsryoCalculationEngine.Execute()` を直接呼び出して結果を表示しており、Engine内部で `RecKbn` (Teigaku/Hengaku/Fuzui) を計算して返す。法令区分は `sekouDt` (t_settei から取得) との比較で判定 (`Form_f_flx_KEIJO` の実装を参照)。

### 5.4 Form_f_flx_TOUGETSU の WHERE句不足について

`Form_f_flx_TOUGETSU.vb:115` のコメント通り、「集計月、開始日、終了日、中途解約日で条件増えるはず」。
`Form_f_KEIJO_JOKEN.vb` の `GenerateWhereClause` で生成する条件 (日付範囲、leakbn_id 等) をベースに同様の WHERE 句を追加するのが適切。

### 5.5 ContractEntry の Update 処理

`Form_ContractEntry.vb:570-726` に Update (修正) 処理は実装済み（洗い替え方式: 明細全DELETE → 全INSERT）。Issue記載の「Update処理が未実装」は解消済み。

### 5.6 Access版バイナリのみ

`C:\access_LeaseM4BS` に `.mdb` / `.accdr` のみ存在。VBA ソースファイル (.bas/.cls/.frm) は含まれない。参照不可。
