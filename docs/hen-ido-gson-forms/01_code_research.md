# コードベース調査資料: hen-ido-gson-forms

## 1. プロジェクト概要

- **フレームワーク・言語**: VB.NET 4.7.2 / WinForms
- **DB**: PostgreSQL（Npgsql 経由）
- **対象ブランチ**: `claude/keihi-meisai-form`

### ディレクトリ構成（関連部分）

```
C:\kobayashi_LeaseM4BS\
├── LeaseM4BS\
│   └── LeaseM4BS.DataAccess\          # データアクセス層
│       ├── DbConnectionManager.vb     # Npgsql接続管理
│       ├── CrudHelper.vb              # 汎用CRUD (DAO.Recordset代替)
│       ├── GsonScheduleBuilder.vb     # 減損スケジュール生成
│       ├── CashScheduleBuilder.vb     # 変額キャッシュスケジュール(D_HENL読み込み)
│       ├── RepaymentScheduleBuilder.vb # 返済スケジュール生成
│       ├── KeijoWorkTableManager.vb   # 計上ワークテーブル(tw_d_henl_keijo/tw_d_gson_keijo)
│       ├── KeijoSqlBuilder.vb         # 計上SQL生成(d_gson参照含む)
│       ├── KeijoTypes.vb              # HenlKeijo/GsonKeijo enum定義
│       └── ScheduleTypes.vb           # GsonScheduleEntry, HensaiScheduleEntry等
└── LeaseM4BS.TestWinForms\
    └── LeaseM4BS.TestWinForms\
        ├── Form_f_HEN_SCH.vb/.Designer.vb   # 返済スケジュール変更フォーム
        ├── Form_f_HENF.vb/.Designer.vb       # 保守料(返金管理)フォーム
        ├── Form_f_HENL.vb/.Designer.vb       # 変額リース料フォーム
        ├── Form_f_IDO.vb/.Designer.vb         # 物件移動フォーム
        ├── Form_f_IDO_SUB.vb/.Designer.vb    # 物件移動サブフォーム
        ├── Form_f_IDOLST_JOKEN.vb/.Designer.vb # 移動物件一覧条件フォーム(実装済み)
        ├── Form_f_flx_D_GSON.vb/.Designer.vb  # 減損フレックス一覧(実装済み)
        ├── Form_f_flx_D_HENF.vb/.Designer.vb  # 保守フレックス一覧(実装済み)
        ├── Form_f_flx_IDOLST.vb/.Designer.vb  # 移動物件一覧(実装済み)
        └── FormHelper.vb                      # 共通拡張メソッド
```

### 主要な依存ライブラリ

- `Npgsql` (PostgreSQL接続)
- `System.Windows.Forms` (WinForms)
- `DocumentFormat.OpenXml` (ファイル出力)

---

## 2. アーキテクチャ概要

- **アーキテクチャパターン**: 2層構造（WinForms UI ＋ DataAccessクラスライブラリ）
- **レイヤー構成**:
  | レイヤー | 場所 | 責務 |
  |---|---|---|
  | UI層 | LeaseM4BS.TestWinForms | フォーム表示・イベント処理 |
  | データアクセス層 | LeaseM4BS.DataAccess | SQL実行・計算ロジック |

---

## 3. 関連する既存コード

### フォーム・UI（LeaseM4BS.TestWinForms）

| ファイルパス | 役割 | 実装状況 |
|---|---|---|
| `Form_f_HEN_SCH.vb` | 返済スケジュール変更（ダイアログ） | **スタブ** (コード空) |
| `Form_f_HEN_SCH.Designer.vb` | 同上・レイアウト定義済み | Designer完成済み |
| `Form_f_HENF.vb` | 保守料/返金管理（ダイアログ） | **スタブ** (コード空) |
| `Form_f_HENF.Designer.vb` | 同上・レイアウト定義済み | Designer完成済み |
| `Form_f_HENL.vb` | 変額リース料（ダイアログ） | **スタブ** (コード空) |
| `Form_f_HENL.Designer.vb` | 同上・レイアウト定義済み | Designer完成済み |
| `Form_f_IDO.vb` | 物件移動処理 | **スタブ** (コード空) |
| `Form_f_IDO.Designer.vb` | 同上・レイアウト定義済み | Designer完成済み |
| `Form_f_IDO_SUB.vb` | 物件移動サブ（行データ） | **スタブ** (コード空) |
| `Form_f_IDO_SUB.Designer.vb` | 同上・レイアウト定義済み | Designer完成済み |
| `Form_f_IDOLST_JOKEN.vb` | 移動物件一覧条件フォーム | **実装済み** |
| `Form_f_flx_D_GSON.vb` | 減損フレックス一覧 | **実装済み** |
| `Form_f_flx_D_HENF.vb` | 保守フレックス一覧 | **実装済み** |
| `Form_f_flx_IDOLST.vb` | 移動物件一覧 | **実装済み** |

### DataAccess層（LeaseM4BS.DataAccess）

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `DbConnectionManager.vb` | Npgsql接続管理・接続文字列取得 | 高 |
| `CrudHelper.vb` | GetDataTable/Insert/Update/Delete/Transaction | 高 |
| `GsonScheduleBuilder.vb` | `d_gson`から減損スケジュール生成 | 高 |
| `CashScheduleBuilder.vb` | `d_henl`から変額スケジュール生成 | 高 |
| `RepaymentScheduleBuilder.vb` | 返済スケジュール生成（GsonSchedule連携） | 高 |
| `KeijoWorkTableManager.vb` | `tw_d_henl_keijo`/`tw_d_gson_keijo`管理 | 高 |
| `KeijoSqlBuilder.vb` | 計上SQL生成（`d_gson`参照） | 中 |
| `KeijoTypes.vb` | `HenlKeijo`/`GsonKeijo` enum | 中 |
| `ScheduleTypes.vb` | `GsonScheduleEntry`/`HensaiScheduleEntry` | 中 |
| `FormHelper.vb` | ComboBox.Bind/DataGridView拡張 | 中 |

---

## 4. 使用パターン

### 4-1. DataAccess パターン

```vb
' 標準パターン: CrudHelper でクエリ実行
Private _crud As New CrudHelper()

' SELECT: GetDataTable
Dim prms As New List(Of NpgsqlParameter)
prms.Add(New NpgsqlParameter("@kykm_id", kykmId))
Dim dt As DataTable = _crud.GetDataTable(sql, prms)

' INSERT: Insert メソッド
Dim vals As New Dictionary(Of String, Object)
vals("kykm_id") = kykmId
_crud.Insert("d_henl", vals)

' トランザクション
_crud.BeginTransaction()
Try
    _crud.ExecuteNonQuery(sql)
    _crud.Commit()
Catch ex As Exception
    _crud.Rollback()
End Try
```

### 4-2. フォームデータバインディング

```vb
' DataGridView に DataTable をバインド
dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)

' ComboBox バインド (FormHelper 拡張メソッド)
cmb.Bind("SELECT ...", "display_col", "value_col")
cmb.AdjustSize()
```

### 4-3. フォームロードパターン（実装済みフォームの例）

```vb
Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    lbl_CONDITION.Text = LabelText      ' 検索条件ラベル設定
    SearchData()                         ' データ取得
    SecurityChecker.ApplyListLimit(Me)   ' セキュリティ制限
End Sub

Private Sub SearchData()
    Try
        Dim prms As New List(Of NpgsqlParameter)
        Dim sql = BuildSql(txt_SEARCH.Text.Trim(), prms)
        dgv_LIST.Columns.Clear()
        dgv_LIST.AutoGenerateColumns = True
        dgv_LIST.DataSource = _crud.GetDataTable(sql, prms)
        ApplyGridStyle()
    Catch ex As Exception
        MessageBox.Show("一覧取得エラー: " & ex.Message)
    End Try
End Sub
```

### 4-4. エンターキーナビゲーション

```vb
Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    HandleEnterKeyNavigation(Me, e)
End Sub
```

### 4-5. ダイアログパターン（JOKEN → flx）

```vb
' 条件フォーム (JOKEN) からリスト/処理フォームを ShowDialog で開く
Dim frm As New Form_f_flx_IDOLST
frm.LabelText = GetLabelText()
frm.DtFrom = txt_IDO_DT_FROM.Value
frm.DtTo = txt_IDO_DT_TO.Value
frm.ShowDialog()
```

---

## 5. 既存の類似機能分析

### 5-1. f_IDOLST_JOKEN（移動物件一覧条件）

最も参考になる実装済みフォーム。構成:

- **JOKEN フォーム** (`Form_f_IDOLST_JOKEN.vb`): 期間・管理部署チェックボックスを受け取り、`Form_f_flx_IDOLST` を `ShowDialog` で呼び出す
- **フレックスフォーム** (`Form_f_flx_IDOLST.vb`): `BuildSql()` で条件付きSQLを組み立て、`dgv_LIST` に表示
- **前回結果**: `_prevForm As Form_f_flx_IDOLST` に保持し「前回集計結果」ボタンで再表示

### 5-2. f_flx_D_GSON（減損フレックス一覧）

Designer.vb に完全な `dgv_LIST` カラム定義あり。表示対象テーブル:
- `d_gson` (gson_dt, gson_tmg, gson_ryo, gson_rkei)
- `d_kykm`, `d_kykh`, `m_bcat`, `m_lcpt`, `c_kjkbn`, `c_kkbn`

セキュリティ: `SecurityChecker.ApplyDataUpdateLimit(Me)` 適用。

### 5-3. f_flx_D_HENF（保守フレックス一覧）

`d_henf` テーブルの保守料データを表示。`cmd_展開` でスケジュール展開機能。Designer に `shri_dt1`, `shri_cnt`, `klsryo`, `shri_kn`, `sshri_kn`, `shri_en_dt` 等のカラム定義あり。

### 5-4. GsonScheduleBuilder / CashScheduleBuilder

- `GsonScheduleBuilder.Build(crud, kykmId)`: `d_gson` から `GsonScheduleEntry` リストを生成
- `CashScheduleBuilder.BuildHengakuSchedule(crud, kykmId, ckaiykEsdtH)`: `d_henl` から支払スケジュールを展開

---

## 6. データテーブル・フィールド詳細

### d_henl（変額リース料テーブル）

`CashScheduleBuilder.vb:182` より:
```sql
SELECT * FROM d_henl WHERE kykm_id = @kykm_id ORDER BY line_id
```
使用フィールド: `shri_cnt`, `shri_dt1`, `shri_kn`, `sshri_kn`, `klsryo`, `zritu`, `kzei`, `shho_id`, `line_id`

### d_gson（減損テーブル）

`GsonScheduleBuilder.vb:28` より:
```sql
SELECT gson_dt, gson_tmg, gson_ryo, gson_rkei FROM d_gson WHERE kykm_id = @kykm_id ORDER BY gson_dt
```
- `gson_dt`: 減損処理年月
- `gson_tmg`: 処理タイミング (0=月度末, 1=月度初)
- `gson_ryo`: 減損損失
- `gson_rkei`: 減損損失累計額

### tw_d_henl_keijo / tw_d_gson_keijo（計上ワークテーブル）

`KeijoWorkTableManager.vb` が管理:
- `ClearHenlKeijo()` → `DELETE FROM tw_d_henl_keijo`
- `ClearGsonKeijo()` → `DELETE FROM tw_d_gson_keijo`
- `InsertHenlKeijo(row)` / `InsertGsonKeijo(row)` でワーク書き込み

### Form_f_HEN_SCH で使用されるフィールド（Designer.vb より）

| コントロール名 | 役割 |
|---|---|
| `txt_SHRI_DT` | 支払日 |
| `txt_KLSRYO` | 支払額 |
| `txt_KZEI` | 消費税 |
| `txt_ZRITU` | 消費税率 |
| `txt_LINE_ID` | 行ID |
| `txt_KLSRYO_ZKOMI` | 税込み額 |
| `txt_W_KYKM_ID` | 物件ID（ワーク） |
| `txt_SSHRI_KN` | 〆支払間隔 |
| `txt_KLSRYO_SUM` / `txt_KZEI_SUM` / `txt_KLSRYO_ZKOMI_SUM` | 合計行 |

### Form_f_HENF で使用されるフィールド（Designer.vb より）

| コントロール名 | 役割 |
|---|---|
| `txt_F_HKMK_NM` | 費用区分名 |
| `txt_F_LCPT1_NM` | 支払先名 |
| `txt_F_GSHA_NM` | 契約先名 |
| `txt_KOZA_NM` | 銀行口座名 |
| `txt_START_DT` / `txt_LKIKAN` | 開始日・契約期間 |
| `txt_SAIKAISU` | 再リース回数 |
| `chk_HSZEI_KJKBN_ID_MS_F` | 自動設定禁止フラグ |

### Form_f_IDO で使用されるフィールド（Designer.vb より）

| コントロール名 | 役割 |
|---|---|
| `txt_BCAT1_NM_From` 〜 `txt_BCAT5_NM_From` | 移動元管理部署1〜5 |
| `txt_BCAT1_NM_To` 〜 `テキスト474` | 移動先管理部署1〜5 |
| `txt_IDO_DT` | 移動日 |
| `オプション416` / `オプション418` | 管理部署1/費用負担部署2 の選択 |
| `cmd_照会` / `cmd_実行` / `cmd_解除` | 照会・実行・解除ボタン |

---

## 7. 技術的制約・注意事項

### 7-1. Access版ソースへのアクセス不可

`C:\access_LeaseM4BS` はMDBファイルのみ（`.mdb`, `.accdr`）。VBAソースは直接参照不可。
Designer.vb のコントロール名と既存DataAccess層コメント（Access版関数名記載）が唯一の手がかり。

### 7-2. Designer.vb は完成済み・コードは空スタブ

対象フォーム（f_HEN_SCH, f_HENF, f_HENL, f_IDO, f_IDO_SUB）はいずれも:
- **Designer.vb**: コントロール配置・名称・フォームタイトルが定義済み
- **.vb (コード)**: `New()` → `InitializeComponent()` のみのスタブ状態

実装者はビジネスロジックとイベントハンドラのみ追加すれば良い。

### 7-3. f_IDO_SUB の二重コントロール問題

`Form_f_IDO_SUB.Designer.vb` では、ヘッダ行用（ラベル）とデータ行用（TextBox）で同じ位置 (0,0) にコントロールが配置されている。実装時は ScrollableControl/Panel に複数行を動的生成するパターンが必要と推測。

### 7-4. GsonScheduleEntry の GSON_TMG 値制約

`GsonScheduleBuilder.vb:62` で GSON_TMG が 0/1 以外の場合に例外をスロー。d_gson のデータ品質に注意。

### 7-5. セキュリティチェック

一覧フォームには `SecurityChecker.ApplyListLimit(Me)` または `SecurityChecker.ApplyDataUpdateLimit(Me)` を呼び出すパターンが必須（`Form_f_flx_D_GSON`, `Form_f_flx_D_HENF` 参照）。

### 7-6. 接続文字列

`DbConnectionManager.vb:76` にデフォルト接続文字列:
```
Host=localhost;Port=5432;Database=lease_m4bs;Username=lease_m4bs_user;Password=iltex_mega_pass_m4
```
App.config の `connectionStrings["LeaseM4BS"]` または環境変数 `LEASE_M4BS_CONNECTION_STRING` で上書き可能。

---

## 8. 命名規則・コーディング規約

### ファイル命名規則
- フォームファイル: `Form_<Access版フォーム名>.vb` / `Form_<Access版フォーム名>.Designer.vb`
- 例: `Form_f_IDO.vb`, `Form_f_flx_IDOLST.vb`

### コントロール命名規則
- ボタン: `cmd_<動詞>` (例: `cmd_実行`, `cmd_CANCEL`, `cmd_CLOSE`)
- テキストボックス: `txt_<フィールド名大文字>` (例: `txt_KLSRYO`, `txt_IDO_DT`)
- DataGridView: `dgv_LIST`
- ラベル: 日本語名そのまま (例: `ラベル318`, `テキスト13`) または意味のある名前 (`lbl_CONDITION`)
- チェックボックス: `chk_<フィールド名>` (例: `chk_IDO_F`, `chk_BCAT1_F`)
- ラジオボタン: `オプション<番号>` (Access踏襲) または意味のある名前

### クラス命名規則
- フォームクラス: `Form_<フォーム名>` (Partial Class)
- DataAccessクラス: PascalCase + 機能名 (例: `GsonScheduleBuilder`, `CrudHelper`)

### SQLパターン
- パラメータプレフィックス: `@` (例: `@kykm_id`)
- テーブル名: PostgreSQL小文字スネークケース (例: `d_kykm`, `m_bcat`)

### コメント規約
- Access版との対応を明記: `' Access版 gMake減損_SCH 相当`
- DataPropertyName は大文字スネークケース (例: `"GSON_DT"`, `"KYKM_NO"`)

---

## 9. 実装時の推奨アプローチ

### f_HEN_SCH（返済スケジュール変更）
- Designer.vb にある `cmd_呼出元に反映` / `cmd_削除` / `cmd_閉じる` イベントを実装
- `txt_SHRI_DT`, `txt_KLSRYO`, `txt_KZEI`, `txt_ZRITU` 等の入力値を読み取り
- `CrudHelper.Update("d_haif", ...)` でスケジュール行を更新、または新規 INSERT
- 合計行 (`txt_KLSRYO_SUM` 等) は計算後に更新

### f_HENF（保守料/返金管理）
- `Form_f_flx_D_HENF` (一覧画面) と対で動作するエントリ画面
- `cmd_展開` は `d_henf.shri_cnt` / `shri_kn` からスケジュールを自動展開
- `CashScheduleBuilder.BuildHengakuSchedule` と同様のロジックを参照

### f_HENL（変額リース料）
- `d_henl` テーブルへの CRUD
- `CashScheduleBuilder.BuildHengakuSchedule` (`CashScheduleBuilder.vb:177`) が直接参照
- `cmd_展開` ボタンで `CashScheduleEntry` リストを生成して一覧に表示する想定

### f_IDO / f_IDO_SUB（物件移動）
- `f_IDOLST_JOKEN` → `f_flx_IDOLST` の対がすでに実装済みなので参考にする
- `d_kykm.b_bcat_id` (管理部署ID) / `d_kykm.b_bcat_id_r1` (移動前管理部署ID) を更新
- `d_kykm.ido_dt` に移動日をセット
- `cmd_実行` でバルク更新（`chk_IDO_F` がチェックされた行のみ）
