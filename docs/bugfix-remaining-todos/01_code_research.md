# コードベース調査資料: bugfix-remaining-todos (Issue #17)

## 1. プロジェクト概要

- **フレームワーク・言語**: VB.NET (.NET Framework 4.7.2), WinForms
- **DB**: PostgreSQL (Npgsql 6.0.11)
- **OptionStrict**: DataAccess プロジェクトは `Off`（プロジェクト設定）、TestWinForms プロジェクトはソースファイル単位で `Off`（大半のフォーム）。E2E テストファイルのみ `Option Strict On / Option Explicit On` を明示。

### ディレクトリ構成（ソースのみ、bin/obj 除く）

```
c:\kobayashi_LeaseM4BS\
├── LeaseM4BS\
│   └── LeaseM4BS.DataAccess\            # データアクセス層 (DLL)
│       ├── CrudHelper.vb
│       ├── DbConnectionManager.vb
│       ├── KeijoCalculationEngine.vb    # 計上計算エンジン (最大ファイル)
│       ├── KeijoSqlBuilder.vb
│       ├── KeijoTypes.vb
│       ├── KeijoWorkTableManager.vb
│       ├── KlsryoCalculationEngine.vb   # 取引分類計算エンジン
│       ├── KlsryoTypes.vb
│       ├── MonthlyJournalEngine.vb      # 月次仕訳エンジン
│       ├── ChukiCalcEngine.vb
│       ├── AmortizationScheduleBuilder.vb
│       ├── CashScheduleBuilder.vb
│       ├── GsonScheduleBuilder.vb
│       ├── RepaymentScheduleBuilder.vb
│       ├── ScheduleTypes.vb
│       ├── FixedLengthFileWriter.vb
│       ├── KitokuFixedLengthFormats.vb
│       └── UsageExamples.vb
├── LeaseM4BS.TestWinForms\
│   └── LeaseM4BS.TestWinForms\          # 画面層 (EXE) + 557フォーム
│       ├── Form_f_LOGIN_JET.vb          # ログイン画面
│       ├── Form_MAIN.vb                 # メインメニュー
│       ├── Form_f_CHUKI_*.vb            # 注記関連画面
│       ├── Form_f_IDOLST_JOKEN.vb       # 異動一覧条件
│       ├── Form_f_IMPORT_*_FROM_EXCEL.vb # Excelインポート画面 (4画面)
│       ├── Form_BuknEntry.vb            # 物件登録
│       ├── Form_f_BEPPYO2_REP.vb        # 別表2帳票
│       ├── Form_f_CHUKI_SCH.vb          # 注記スケジュール
│       ├── Form_f_flx_YOSAN.vb          # 予算一覧
│       ├── Form_f_flx_TOUGETSU.vb       # 当月一覧
│       └── ...
├── test_e2e_blackbox.vb                 # E2Eテスト (Option Strict On)
├── test_schedule_blackbox.vb            # スケジュールテスト
├── test_keijo_joken_blackbox.vb         # 計上条件テスト
├── test_chuki_idolst_joken_blackbox.vb  # 注記・異動条件テスト
├── test_fixed_length.vb                 # 固定長出力テスト
├── test_type_safety_blackbox.vb         # 型安全性テスト
└── test_diag.vb                         # 診断テスト
```

---

## 2. アーキテクチャ概要

- **パターン**: 2層構造（DataAccess DLL + WinForms EXE）
- **DI**: なし（`New CrudHelper()` を各クラスで直接生成）
- **状態管理**: `LoginSession` クラスで静的プロパティ管理（ユーザー権限等）
- **接続管理**: `DbConnectionManager` → `CrudHelper` の 2 段構成。`CrudHelper` はトランザクション管理を内包する。

---

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS/LeaseM4BS.DataAccess/CrudHelper.vb` | DB CRUD 基盤、トランザクション管理 | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/DbConnectionManager.vb` | 接続生成・ハードコード接続文字列 | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` | 空 Catch ブロック複数あり | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/MonthlyJournalEngine.vb` | 空 Catch ブロックあり | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KeijoCalculationEngine.vb` | NULL 未ガード CDate() 変換あり | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/GsonScheduleBuilder.vb` | NULL 未ガード CDate() あり | 高 |
| `LeaseM4BS.TestWinForms/.../Form_f_CHUKI_RECALC.vb` | todo: 実装不明危険コード | 高 |
| `LeaseM4BS.TestWinForms/.../Form_f_IMPORT_*_FROM_EXCEL.vb` | todo: ファイル入力未実装 (4画面) | 高 |
| `LeaseM4BS.TestWinForms/.../Form_MAIN.vb` | 未実装メニュー多数 (MessageBox.Show のみ) | 高 |
| `LeaseM4BS.TestWinForms/.../Form_BuknEntry.vb` | todo: 削除処理未実装 | 中 |
| `LeaseM4BS.TestWinForms/.../Form_f_BEPPYO2_REP.vb` | todo: 印刷処理未実装 | 中 |
| `LeaseM4BS.TestWinForms/.../Form_f_flx_YOSAN.vb` | todo: グレーアウト条件不明、SQL不明 | 中 |
| `LeaseM4BS.TestWinForms/.../Form_f_flx_TOUGETSU.vb` | todo: 検索条件不完全 | 中 |
| `LeaseM4BS.TestWinForms/.../Form_f_CHUKI_SCH.vb` | todo: SQL 列不完全 (コメントアウト多数) | 中 |
| `LeaseM4BS.TestWinForms/.../Form_f_flx_BUKN.vb` | todo: 項目確認、保守料不明 | 中 |
| `LeaseM4BS.TestWinForms/.../Form_fc_TC_HREL.vb` | todo: 全入れ替え方式の自動採番バグリスク | 中 |

---

## 4. 発見された問題の詳細分類

### 4.1 TODO / 未実装コメント一覧

#### 優先度 高 (業務ロジックへの影響あり)

| ファイル | 行 | 内容 |
|---|---|---|
| `Form_f_CHUKI_RECALC.vb` | 30 | `todo 危険(実装不明、要確認)` — `ResetChukiData()` の全体ロジックが未検証 |
| `Form_f_IMPORT_IDO_FROM_EXCEL.vb` | 36 | `todo ファイル入力` — Excelインポート未実装 |
| `Form_f_IMPORT_GSON_FROM_EXCEL.vb` | 33 | `todo ファイル入力` — Excelインポート未実装 |
| `Form_f_IMPORT_SAILEASE_FROM_EXCEL.vb` | 28 | `todo ファイル入力` — Excelインポート未実装 |
| `Form_f_IMPORT_CONTRACT_FROM_EXCEL.vb` | 44 | `todo ファイル入力` — Excelインポート未実装 |
| `Form_BuknEntry.vb` | 305 | `todo 削除処理` — 削除ボタン押下時に実際の削除SQLが実行されない |
| `Form_f_BEPPYO2_REP.vb` | 20 | `todo 印刷物を作成する` — 別表2帳票の印刷処理未実装 |

#### 優先度 中 (表示/条件ロジックの不完全さ)

| ファイル | 行 | 内容 |
|---|---|---|
| `Form_f_CHUKI_SCH.vb` | 101, 236 | `todo` — SQL に `発生リース料` 等の複数列がコメントアウトのまま |
| `Form_f_flx_YOSAN.vb` | 42, 61, 63, 106, 201 | グレーアウト条件不明、`予想/既存` 区分不明、行区不明 |
| `Form_f_flx_TOUGETSU.vb` | 115 | `集計月/開始日/終了日/中途解約日で条件増えるはず` |
| `Form_f_flx_BUKN.vb` | 22, 48 | 項目正確性未確認、保守料対応列不明 |
| `Form_f_CHUKI_JOKEN.vb` | 160, 266 | `どの条件でもなぜか表示されるテキスト` |
| `Form_f_IDOLST_JOKEN.vb` | 112 | `「、」で終わるパターン` の TrimEnd 動作未確認 |
| `Form_fc_TC_HREL.vb` | 157, 163 | `全入れ替え方式は自動採番の時不具合になる可能性あり` |
| `Form_f_T_KARI_RITU.vb` | 29 | 検索項目未定義 |
| `Form_f_T_ZEI_KAISEI.vb` | 30 | 検索項目未定義 |
| `Form_BuknEntry.vb` | 188 | `todo 適切なメソッド名` |
| `KeijoCalculationEngine.vb` | 1201 | `workRow.KeijoF = False ' 後で設定` — 意図的なフラグ設定箇所 (確認要) |

#### 優先度 低 (機能プレースホルダー)

| ファイル | 行 | 内容 |
|---|---|---|
| `Form_MAIN.vb` | 810〜 | システムタブ「データ保存/復元/EXCEL取込/キャッシュクリア/最適化/DB作成/DB削除/ロック解除」がすべて `MessageBox.Show("未実装")` のみ |

---

### 4.2 空の Catch ブロック (例外握りつぶし)

`Catch\n    End Try` パターンが以下のファイルに存在:

| ファイル | 行 | 文脈 |
|---|---|---|
| `CrudHelper.vb` | 439–440 | `Dispose()` 中の `_activeTransaction.Rollback()` 失敗を握りつぶし |
| `KlsryoCalculationEngine.vb` | 360–361 | `haifDt` 取得 (配賦情報取得失敗を無視) |
| `KlsryoCalculationEngine.vb` | 993–994 | `GetSekouDt()` — 施行日取得失敗でデフォルト値 `2008/4/1` を返す |
| `KlsryoCalculationEngine.vb` | 1005–1006 | `GetNameFromMaster()` — マスタ名称取得失敗を無視 |
| `MonthlyJournalEngine.vb` | 102–103 | 月次仕訳計上の rollback 失敗を握りつぶし |
| `MonthlyJournalEngine.vb` | 153–154 | 注記計算の rollback 失敗を握りつぶし |
| `test_e2e_blackbox.vb` | 181–182 | テストコード内 (テストのみ、本番影響なし) |

**注意**: `CrudHelper.Dispose()` の空 Catch はロールバック時例外を握りつぶすため、ロールバック失敗が検知できない。

---

### 4.3 Option Strict Off に起因する型安全リスク

**プロジェクト設定**:
- `LeaseM4BS.DataAccess.vbproj`: `<OptionStrict>Off</OptionStrict>` (行 44)
- `LeaseM4BS.TestWinForms.vbproj`: ソースファイル単位で未宣言 → `Off`
- 警告抑制: `NoWarn` 42016,41999,42017,42018,42019,42032,42036,42020,42021,42022

**DataAccess DLL 内の `CDate()` 直呼びで NULL 未ガード箇所 (InvalidCastException リスク)**:

| ファイル | 行 | 列名 | NULL 可能性 |
|---|---|---|---|
| `KeijoCalculationEngine.vb` | 127 | `row("start_dt")` | NULL 可 (チェックなし) |
| `KeijoCalculationEngine.vb` | 129 | `row("b_rend_dt")` | NULL 可 (チェックなし) |
| `GsonScheduleBuilder.vb` | 41, 85 | `row("gson_dt")` | NULL 可 (チェックなし) |
| `KlsryoCalculationEngine.vb` | 198, 262 | `row("shri_dt1")` | NULL 可 |
| `KlsryoCalculationEngine.vb` | 366, 387 | `henfRow("shri_dt1")` | NULL 可 |
| `CashScheduleBuilder.vb` | 189 | `row("shri_dt1")` | NULL 可 |

**比較**: `KlsryoCalculationEngine.vb` の `IsTargetRecord()` (行 138) は `startDt/bRendDt` を `IsDBNull` チェック後に `CDate()` — 正しいパターン。`KeijoCalculationEngine.vb` の同等箇所 (行 127,129) では IsDBNull チェックなしの直変換がある。

**VB の暗黙変換が残っている箇所**:
- `KeijoCalculationEngine.vb` 行 162-163: `CDbl(row("kykh_kykh_id"))`, `CDbl(row("kykm_kykm_no"))` — IsDBNull なし
- `Form_f_LOGIN_JET.vb` 内の `CDate()/CInt()` が 8 箇所 — ログイン時エラーリスク

---

### 4.4 Npgsql 接続管理

**設計上の問題点**:

1. **`GetConnection()` の呼び出し元が Dispose 管理を担う設計**
   `DbConnectionManager.GetConnection()` は Open 済み `NpgsqlConnection` を返す。呼び出し元が `Using` で囲まなければ接続リークする。
   - `CrudHelper.GetDataTable()` (行 61), `ExecuteNonQuery()` (行 113), `ExecuteScalar()` (行 164) は `useInternalConnection` フラグで `conn.Dispose()` を手動呼び出し — ただし Catch ブロック到達時は Dispose されない可能性あり (Catch 内で conn を dispose しないまま `Throw`)

2. **ハードコードされた接続文字列のフォールバック**
   `DbConnectionManager.GetConnectionString()` 行 76:
   ```
   Return "Host=localhost;Port=5432;Database=lease_m4bs;Username=lease_m4bs_user;Password=iltex_mega_pass_m4"
   ```
   本番環境で App.config が欠けた場合、開発用認証情報でフォールバックする。

3. **`WriteError()` のログファイルパスが相対パス**
   `DbConnectionManager.WriteError()` 行 162: `Dim logFile = "error.log"` — カレントディレクトリへの書き込みで、運用環境では予測不可能な場所に出力される。

4. **`CrudHelper.GetDataTable()` の接続漏れパターン**:
   ```vb
   Try
       conn = _connectionManager.GetConnection()
       ' ...操作...
       If useInternalConnection Then conn.Dispose()  ' 成功パスのみ
   Catch ex As Exception
       ' conn は Dispose されないまま例外再 Throw
       Throw New Exception(...)
   End Try
   ```
   `GetDataTable`, `ExecuteNonQuery`, `ExecuteScalar` の 3 メソッドすべてに同じパターン。

---

### 4.5 Access 版との互換性問題

| カテゴリ | 箇所 | 問題 |
|---|---|---|
| 未実装 DB 管理 | `Form_MAIN.vb` 行 814〜 | Access 版 Case 401-424 (データ保存/復元/最適化等) が MessageBox のみ |
| Excelインポート | `Form_f_IMPORT_*_FROM_EXCEL.vb` 4画面 | ファイル読み込み処理が全未実装 |
| 注記スケジュール SQL | `Form_f_CHUKI_SCH.vb` | 発生利息相当額/支払利息相当額/変換元本等の列がコメントアウト |
| 予算一覧 | `Form_f_flx_YOSAN.vb` | '既存' AS 予想 / '定額' AS 行区 がハードコード (Access 版は動的) |
| 当月一覧検索条件 | `Form_f_flx_TOUGETSU.vb` | 集計月・中途解約日等の条件が欠落 |
| 物件一覧 | `Form_f_flx_BUKN.vb` | 保守料列が対応不明でコメントアウト |
| 施行日デフォルト | `KlsryoCalculationEngine.vb` 行 995 | DB 取得失敗時に `2008/4/1` をハードコードで返す |
| 注記再計算 | `Form_f_CHUKI_RECALC.vb` | `ResetChukiData()` の実装が「危険・要確認」とコメントされており、Access 版との等価性未検証 |

---

### 4.6 その他指摘事項

- **使われていない import**: `Form_f_CHUKI_RECALC.vb` が `Npgsql.Replication.PgOutput.Messages` を import — 不要な依存
- **`（没）LeaseM4BS.DataAccess.vbproj`**: ルートに削除候補ファイルが残存
- **`UsageExamples.vb`**: DataAccess DLL にサンプルコードが含まれている — 本番ビルドに不要
- **`KeijoCalculationEngine.vb` 行 1201**: `workRow.KeijoF = False ' 後で設定` — 設定タイミングが後続ロジックに依存するが、設定漏れが発生した場合のデフォルト False が計上フラグとして問題になりうる

---

## 5. 既存テストのカバレッジ状況

| テストファイル | 対象 | テスト方式 | 状態 |
|---|---|---|---|
| `test_schedule_blackbox.vb` | ScheduleHelper, AmortizationScheduleBuilder, RepaymentScheduleBuilder, ChukiCalcEngine, GsonScheduleBuilder | ブラックボックス (DB不要) | PASS |
| `test_keijo_joken_blackbox.vb` | KeijoJoken 条件構築, KeijoCalculationEngine, MonthlyJournalEngine | ブラックボックス (DB必要) | PASS |
| `test_chuki_idolst_joken_blackbox.vb` | Form_f_CHUKI_JOKEN, Form_f_IDOLST_JOKEN, Form_f_flx_IDOLST | ブラックボックス (DB必要) | PASS (51テスト) |
| `test_fixed_length.vb` | FixedLengthFileWriter, KitokuFixedLengthFormats | ブラックボックス (DB不要) | PASS |
| `test_type_safety_blackbox.vb` | ScheduleHelper, AmortizationScheduleBuilder, RepaymentScheduleBuilder, ChukiCalcEngine, GsonScheduleBuilder, CashScheduleBuilder, 型安全性 | ブラックボックス (DB不要) | PASS |
| `test_e2e_blackbox.vb` | ログイン認証, LoginSession, KlsryoCalculationEngine, KeijoJoken, KeijoCalculationEngine, MonthlyJournalEngine, ChukiCalcEngine, FixedLengthFileWriter | E2E (DB必要) | PASS |
| `test_diag.vb` | DB接続診断 | 診断 (DB必要) | — |

**テストがないエリア**:
- Form 層の UI ロジック (Form_f_CHUKI_RECALC の `ResetChukiData` 等)
- Excelインポート処理 (未実装のため)
- 物件削除処理 (未実装のため)
- 別表2帳票出力 (未実装のため)
- Form_f_flx_YOSAN / Form_f_flx_TOUGETSU の SQL 条件ロジック

---

## 6. 技術的制約・注意事項

- **Option Strict Off** がプロジェクトレベルで設定されており、暗黙型変換が多用されている。`Option Strict On` にすると CDate(Object)/CInt(Object) 等が大量にコンパイルエラーになる。
- **CrudHelper の接続リークパターン** は DataAccess DLL の 3 メソッド (`GetDataTable`, `ExecuteNonQuery`, `ExecuteScalar`) に共通して存在する。修正時は 3 箇所を同時に修正する必要がある。
- **空の Catch** は `KlsryoCalculationEngine.GetSekouDt()` と `GetNameFromMaster()` においてデフォルト値返却パターンとして使われており、意図的な設計の可能性がある。修正時は `KlsryoCalculationEngine.IsTargetRecord()` の `Return True` デフォルト動作と整合させること。
- `GsonScheduleBuilder.vb` 行 41 の `CDate(row("gson_dt"))` は NULL ガードなし。`d_gson.gson_dt` が NULL の場合 `InvalidCastException` が発生する。

---

## 7. 命名規則・コーディング規約

- クラス名: PascalCase (`KeijoCalculationEngine`, `CrudHelper`)
- メソッド名: PascalCase (`GetDataTable`, `ExecuteNonQuery`)
- プライベートフィールド: `_` prefix + camelCase (`_connectionManager`, `_disposed`)
- ファイル名: クラス名と一致 (例外: `Form_f_*.vb` はフォーム名を含む)
- コメント: 日本語が多用される（業務用語はアルファベット略語が主）
- テストファイル: `test_<機能名>_blackbox.vb` のスネークケース命名
