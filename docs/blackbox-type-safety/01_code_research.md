# コードベース調査資料: blackbox-type-safety (Issue #15)

## 1. プロジェクト概要

- **フレームワーク・言語**: VB.NET / .NET Framework 4.7.2 / WinForms
- **DB**: PostgreSQL (Npgsql 6.0.11)
- **Access版 (移行元)**: JET (Access MDB) + VBA

### ディレクトリ構成

```
C:\kobayashi_LeaseM4BS\
├── LeaseM4BS/
│   └── LeaseM4BS.DataAccess/          # 業務ロジック・データアクセス層
│       ├── AmortizationScheduleBuilder.vb  # 償却スケジュール (定額/定率法)
│       ├── RepaymentScheduleBuilder.vb     # 返済スケジュール (元本・利息)
│       ├── ChukiCalcEngine.vb              # 注記計算エンジン
│       ├── CashScheduleBuilder.vb          # 支払スケジュール
│       ├── GsonScheduleBuilder.vb          # 減損スケジュール
│       ├── KeijoCalculationEngine.vb       # 計上計算エンジン
│       ├── KlsryoCalculationEngine.vb      # 期間リース料計算エンジン
│       ├── MonthlyJournalEngine.vb         # 月次仕訳エンジン
│       ├── ScheduleTypes.vb               # スケジュール型定義 + ScheduleHelper
│       ├── KlsryoTypes.vb                 # Enum/型定義
│       ├── KeijoTypes.vb                  # 計上エンジン型定義
│       ├── FixedLengthFileWriter.vb        # 固定長ファイル出力
│       ├── KitokuFixedLengthFormats.vb     # 固定長フォーマット定義
│       ├── KeijoSqlBuilder.vb             # SQL組立て
│       ├── KeijoWorkTableManager.vb       # ワークテーブルDB操作
│       ├── CrudHelper.vb                  # PostgreSQL汎用CRUD
│       └── DbConnectionManager.vb         # DB接続管理
├── LeaseM4BS.TestWinForms/              # テスト用WinForms (画面フォーム群)
├── test_schedule_blackbox.vb            # スケジュール系ブラックボックステスト
└── test_fixed_length.vb                 # 固定長出力ブラックボックステスト
```

### 主要な依存ライブラリ
- Npgsql 6.0.11 (PostgreSQL ドライバ)
- System.Data (DataTable, DataRow)
- System.Text.Encoding (Shift-JIS 固定長出力)

---

## 2. アーキテクチャ概要

### アーキテクチャパターン
- **データアクセス層 (LeaseM4BS.DataAccess)**: 業務ロジック + DB操作を担う単一プロジェクト
- **プレゼンテーション層 (LeaseM4BS.TestWinForms)**: WinFormsフォーム群（557フォーム）
- フォームは DataAccess の Engine クラスを呼び出して処理する。

### レイヤー構成と責務

| レイヤー | クラス | 責務 |
|---|---|---|
| 業務ロジック | AmortizationScheduleBuilder, RepaymentScheduleBuilder, ChukiCalcEngine | 精密計算（Access版VBAの忠実移植） |
| 業務ロジック | KeijoCalculationEngine, KlsryoCalculationEngine | 計上・期間リース料計算 |
| データアクセス | CrudHelper, DbConnectionManager | PostgreSQL CRUD |
| 出力 | FixedLengthFileWriter, KitokuFixedLengthFormats | Shift-JIS固定長ファイル生成 |
| 型定義 | ScheduleTypes, KlsryoTypes, KeijoTypes | Enum・DTO定義 |

---

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS.DataAccess/AmortizationScheduleBuilder.vb` | 定額法/定率法の償却スケジュール生成。CLng, Math.Floor, Math.Round 使用 | 高 |
| `LeaseM4BS.DataAccess/RepaymentScheduleBuilder.vb` | 返済スケジュール生成 (元本・利息)。CInt使用 | 高 |
| `LeaseM4BS.DataAccess/ChukiCalcEngine.vb` | 注記計算エンジン。CInt(enum)比較パターン多数 | 高 |
| `LeaseM4BS.DataAccess/KeijoCalculationEngine.vb` | 計上計算エンジン。CDbl, CInt, CLng, CStr を多数使用 | 高 |
| `LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` | 期間リース料計算。CDbl, CInt パターン | 高 |
| `LeaseM4BS.DataAccess/CashScheduleBuilder.vb` | 支払スケジュール生成。CInt, CDbl 使用 | 高 |
| `LeaseM4BS.DataAccess/ScheduleTypes.vb` | GInt/GKasan ヘルパー（Access版VBA互換丸め関数） | 高 |
| `LeaseM4BS.DataAccess/FixedLengthFileWriter.vb` | 固定長出力。Convert.ToDouble + try/catch | 中 |
| `LeaseM4BS.DataAccess/KeijoWorkTableManager.vb` | ワークテーブルINSERT。CInt(enum) | 中 |
| `LeaseM4BS.DataAccess/CrudHelper.vb` | DB汎用CRUD。Convert.ToInt32 | 低 |
| `test_schedule_blackbox.vb` | スケジュール系ブラックボックステスト (GInt, GKasan, 償却, 返済, 注記) | 高 |
| `test_fixed_length.vb` | 固定長出力ブラックボックステスト | 高 |

---

## 4. 使用パターン

### 4.1 型変換パターン一覧

#### (A) CDbl — DBからのDouble値取得（最多使用）

`KeijoCalculationEngine.vb` での使用例：
```vb
' :113
Dim kykmId As Double = CDbl(row("kykm_kykm_id"))
' :141
p.BLbSoneki = If(IsDBNull(row("b_lb_soneki")), CType(Nothing, Double?), CDbl(row("b_lb_soneki")))
' :162-163
resultRow.KykhId = CDbl(row("kykh_kykh_id"))
resultRow.KykmNo = CDbl(row("kykm_kykm_no"))
```

`KlsryoCalculationEngine.vb` での使用例：
```vb
' :213
Dim schH = CashScheduleBuilder.BuildHengakuSchedule(_crud, CDbl(row("kykm_kykm_id")), ...)
' :581-582 (Object型のArrayにAccumulateするパターン)
lsryoTokig(j) = CDbl(lsryoTokig(j)) + entry.Lsryo
zeiTokig(j)   = CDbl(zeiTokig(j))   + entry.Zei
```

#### (B) CInt — Enum比較・整数変換

`KeijoCalculationEngine.vb`、`ChukiCalcEngine.vb` での使用例（最頻出パターン）：
```vb
' KeijoCalculationEngine.vb :121
If kjkbnId <> CInt(Kjkbn.Sisan) Then Continue For
' ChukiCalcEngine.vb :30
If params.RcalcId = CInt(RcalcKind.Risikomi) OrElse params.LeakbnId = CInt(LeaseKbn.Ope) Then
```

`RepaymentScheduleBuilder.vb` での使用例：
```vb
' :297
endNen += CInt(Math.Floor((endGetu - 1) / 12.0))
```

`CashScheduleBuilder.vb` での使用例：
```vb
' :121
Dim cnt As Integer = If(shriCnt Is Nothing OrElse IsDBNull(shriCnt), 0, CInt(shriCnt))
' :188-191
Dim shriCnt As Integer = CInt(row("shri_cnt"))
Dim shriKn  As Integer = CInt(row("shri_kn"))
Dim sshriKn As Integer = CInt(row("sshri_kn"))
```

#### (C) CLng — 配賦按分（整数丸め）

`KeijoCalculationEngine.vb` での使用例：
```vb
' :622-623 (配賦按分計算)
entry.Lsryo = CLng(schT(j).Lsryo * hi.Haifritu / 100)
entry.Zei   = CLng(schT(j).Zei   * hi.Haifritu / 100)
' :726-727
entry.Lsryo = CLng(schH(j).Lsryo * hi.Haifritu / 100)
entry.Zei   = CLng(schH(j).Zei   * hi.Haifritu / 100)
```

`AmortizationScheduleBuilder.vb` での使用例（重要！M4互換モード）：
```vb
' :28-29
Dim llWK As Long = CLng(Math.Floor(ritu * 1000000))
llWK = CLng(Math.Round(llWK * 0.1, MidpointRounding.ToEven))
```

#### (D) CStr — DB値の文字列化

`KeijoCalculationEngine.vb` での使用例：
```vb
' :164-165
resultRow.BuknNm   = If(IsDBNull(row("bukn_nm")), "", CStr(row("bukn_nm")))
resultRow.KykbnlNo = If(IsDBNull(row("kykbnl")), "", CStr(row("kykbnl")))
```

#### (E) Convert.ToDouble / Convert.ToInt32 — DB値変換

`FixedLengthFileWriter.vb`:
```vb
' :59 (try/catch付き - NULLやオブジェクト型に対応)
numVal = Convert.ToDouble(value)
```

`KeijoCalculationEngine.vb`:
```vb
' :1491 (GetDbl内部)
Return Convert.ToDouble(row(colName))
' :1505 (GetInt内部)
Return Convert.ToInt32(row(colName))
```

`CrudHelper.vb`:
```vb
' :347
Return Convert.ToInt32(result) > 0
```

---

## 5. 計算ロジックの特定（精度が重要な業務ロジック）

### 5.1 GInt — Access版VBA互換の切り捨て丸め（最重要）

**場所**: `ScheduleTypes.vb:351-356`

```vb
Public Shared Function GInt(value As Double) As Double
    ' VBA CStr(Double) は G15 相当 → 15桁有効数字で丸め
    Dim s As String = value.ToString("G15")
    Dim cleaned As Double = Double.Parse(s)
    Return Math.Floor(cleaned)
End Function
```

**背景**: Access版VBAの `Int(CStr(value))` パターンを再現。`CStr` が G15 形式で文字列化することで浮動小数点誤差（例: `3744.9999999999996 → 3745`）を除去してから切り捨てる。VB.NET の `Math.Floor` 単独では再現できない。

**使用箇所**（主なもの）:
- `AmortizationScheduleBuilder.vb`: `BuildTeigaku`/`BuildTeiritu` 内の `monthSkyak` 計算
- `RepaymentScheduleBuilder.vb`: `RisokuHasseiE` の計算
- `ChukiCalcEngine.vb`: 注記計算全般

### 5.2 CLng の Banker's Rounding（定率法償却率計算）

**場所**: `AmortizationScheduleBuilder.vb:28-29`

```vb
Dim llWK As Long = CLng(Math.Floor(ritu * 1000000))
llWK = CLng(Math.Round(llWK * 0.1, MidpointRounding.ToEven))
```

Access版VBAの `CLng()` は銀行家丸め（Banker's Rounding: 0.5の場合は偶数に丸め）を行う。VB.NETでは `MidpointRounding.ToEven` を明示して再現している。

### 5.3 配賦按分の整数切り捨て（CLng）

**場所**: `KeijoCalculationEngine.vb:622-623, 726-727, 877-878`

```vb
entry.Lsryo = CLng(schT(j).Lsryo * hi.Haifritu / 100)
```

`CLng` は VBA互換のBanker's Roundingを行う。`CInt` と異なり Long 型に変換するが、Lsryo は Double なので実質的に「整数に丸める」意図。Access版との一致のために `CLng` を使用している。

### 5.4 Object型配列への累積加算（CDbl往復）

**場所**: `KlsryoCalculationEngine.vb:581-582`

```vb
lsryoTokig(j) = CDbl(lsryoTokig(j)) + entry.Lsryo
```

`lsryoTokig` は `Object()` 型配列。DBNull の可能性があるため `CDbl` で都度変換してから加算する。`IsDBNull` チェックが省略されているため、DBNull の場合は実行時例外になりうる。**要確認の箇所**。

### 5.5 利息計算式

**場所**: `RepaymentScheduleBuilder.vb:146`

```vb
entry.RisokuHasseiE = ScheduleHelper.GInt((entry.GanponZanS + entry.RisokuMibZanS) * ksanRitu / 12)
```

(元本残高 + 未払利息残高) × 計算利子率 ÷ 12 の結果を GInt で切り捨て。GInt の精度が利息計算全体の正確性に直結する。

---

## 6. Access版との比較ポイント

### 6.1 Access版VBAソース

Access版のVBAソースは以下に存在する：
- `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_注記.txt`
  - `gInt` 関数（= `ScheduleHelper.GInt` の移植元）
  - `gMake償却_SCH`（= `AmortizationScheduleBuilder.Build` の移植元）
  - `gMake返済_SCH_約定支払用`（= `RepaymentScheduleBuilder.BuildYakujoShiharai` の移植元）
  - `m注記計算_main`（= `ChukiCalcEngine.Calculate` の移植元）

### 6.2 主要な対応関係

| Access版 VBA | VB.NET クラス/メソッド |
|---|---|
| `pc_注記.gInt(value)` | `ScheduleHelper.GInt(Double)` |
| `pc_注記.g加算(skipNull, ...)` | `ScheduleHelper.GKasan(Boolean, Double?())` |
| `pc_注記.gCalc償却率(taiyoKikan)` | `AmortizationScheduleBuilder.CalcShokyakuRitu(Integer)` |
| `pc_注記.gMake償却_SCH(...)` | `AmortizationScheduleBuilder.Build(...)` |
| `pc_注記.gMake返済_SCH_約定支払用(...)` | `RepaymentScheduleBuilder.BuildYakujoShiharai(...)` |
| `pc_注記.m注記計算_main(...)` | `ChukiCalcEngine.Calculate(...)` |
| `pc_SHRI_KEIJO.gKEIJO_Main(...)` | `KeijoCalculationEngine.Execute(...)` |
| `pc_仕訳出力.gStrSizeAdjust(...)` | `FixedLengthFileWriter.PadRightByte(...)` |
| `cn_typ_m注記計算_IF` (Type) | `ChukiCalcParams` + `ChukiCalcResult` |
| `cn_typ_gSch_償却` (Type) | `ShokyakuScheduleEntry` |
| `cn_typ_gSch_返済` (Type) | `HensaiScheduleEntry` |

### 6.3 注目すべき移植上のコメント

`ChukiCalcEngine.vb:58-66` に Access版コピペバグの意図的再現がある：

```vb
' ※Access版にコピペバグあり: Y4/Y5変数を設定せずY3/Y1KIMATを上書きしている。
'   Access版と完全一致させるため同じ動作を再現する。
'   実運用では ig決算日=31 が標準のためこの分岐はほぼ通らない。
y1KimatDt = params.KimatDt.AddYears(5)               ' Access版バグ: 最後に5年後で上書き
y4KishuDt = #12/30/1899#                             ' Access版: VBA未初期化Date
y5KishuDt = #12/30/1899#                             ' Access版: VBA未初期化Date
```

---

## 7. 既存テストの状況

### 7.1 ブラックボックステスト（現状）

#### `test_schedule_blackbox.vb`（`c:\kobayashi_LeaseM4BS\test_schedule_blackbox.vb`）

コンパイル: `vbc /r:LeaseM4BS.DataAccess.dll /r:System.Data.dll test_schedule_blackbox.vb`

カバー範囲:
| Part | 対象 | テスト数 |
|---|---|---|
| Part 1 | `ScheduleHelper` (GInt, GKasan, GetGetuYYYYMM, IsMonthEnd) | 8 |
| Part 2 | `AmortizationScheduleBuilder` 定額法 | 2 |
| Part 3 | `AmortizationScheduleBuilder` 定率法 + 償却率計算 | 2 |
| Part 4 | `RepaymentScheduleBuilder` (後払/先払/維持管理費用/残価保証) | 4 |
| Part 5 | `ChukiCalcEngine` (移転外Fリース, オペ, 移転) | 3 |
| Part 6 | 統合テスト (DB接続あり) | 1（要DBあり） |

#### `test_fixed_length.vb`（`c:\kobayashi_LeaseM4BS\test_fixed_length.vb`）

カバー範囲:
| テスト番号 | 内容 |
|---|---|
| Test1-4 | 固定長フォーマット4種のバイト長検証 |
| Test5 | 数値フォーマット (`#,##0` 等) |
| Test6 | 文字列パディング (Shift-JIS) |
| Test7 | バイト超過時の切り捨て |
| Test8-11 | 各フォーマットでの完全レコード生成 |
| Test12 | E2Eファイル出力 |

### 7.2 テストが不足している箇所

- `KeijoCalculationEngine` の型変換（DB行 → Double への変換）
- `KlsryoCalculationEngine` の Object型配列累積（`CDbl(lsryoTokig(j))`）
- `CashScheduleBuilder` の CInt 変換と NULL ハンドリング
- 減損スケジュール反映後の償却/返済スケジュール整合性

---

## 8. TODO/FIXME（型安全性関連）

直接の TODO/FIXME コメントはコードベース内に見当たらなかったが、以下の箇所が型安全性の観点で注意が必要：

### 要注意箇所

| ファイル | 行 | 問題点 |
|---|---|---|
| `KlsryoCalculationEngine.vb` | 581-582 | `Object()` 型配列に `CDbl()` なしで累積すると DBNull 時に例外 |
| `KeijoCalculationEngine.vb` | 113 | `CDbl(row("kykm_kykm_id"))` — DBNull チェックなし |
| `CashScheduleBuilder.vb` | 188-191 | `CInt(row(...))` — DBNull チェックなし（外側でフィルタされているはずだが要確認） |
| `KeijoCalculationEngine.vb` | 1578 | `CDbl(row("kykm_kykm_id"))` — GetDbl を使わず直接 CDbl |
| `ChukiCalcEngine.vb` | 30, 72, 74 | `CInt(RcalcKind.Risikomi)` パターン — Enum から Integer への明示的変換が多数 |

### Access版バグの意図的再現

`ChukiCalcEngine.vb:57-67` — 非月末決算（ig決算日≠31）の場合のコピペバグを意図的に再現。ブラックボックステストではこの分岐の検証が必要。

---

## 9. データアクセス層の構造とDB接続方法

### CrudHelper（`LeaseM4BS.DataAccess/CrudHelper.vb`）

- `GetDataTable(sql, params)` → `DataTable` を返す
- `ExecuteNonQuery(sql, params)` → 更新
- トランザクション管理（BeginTransaction/Commit/Rollback）あり
- パラメータは `List(Of NpgsqlParameter)` を渡す

### DbConnectionManager（`LeaseM4BS.DataAccess/DbConnectionManager.vb`）

- 接続文字列管理
- デフォルト接続文字列はソース内ハードコードまたはapp.config参照（要確認）

### DBからの値取得パターン（型変換観点）

```vb
' ① 安全パターン (GetDbl/GetInt ヘルパー利用)
Dim val As Double = GetDbl(row, "column_name")   ' DBNull → 0.0
Dim val As Integer = GetInt(row, "column_name")  ' DBNull → 0

' ② Ifによるインラインチェック
Dim val As Double = If(IsDBNull(row("col")), 0.0, CDbl(row("col")))

' ③ 直接変換（DBNull非考慮、型不一致でInvalidCastException）
Dim val As Double = CDbl(row("col"))    ' ← 危険パターン
Dim val As Integer = CInt(row("col"))  ' ← 危険パターン
```

`KeijoCalculationEngine.vb` と `KlsryoCalculationEngine.vb` には `GetDbl`/`GetInt` ヘルパーが定義されているが、一部箇所では直接 `CDbl`/`CInt` が使われている不一致が存在する。

---

## 10. 入出力ポイント（外部ファイル出力）

### 固定長出力（KITOKU仕訳）

- **書き込み**: `FixedLengthFileWriter.WriteFile(filePath, dt, fields)`
  - エンコーディング: Shift-JIS
  - フォーマット定義: `KitokuFixedLengthFormats.vb`（CMSW2WRK: 571バイト, APGDHWRK, APGDDWRK, APGDSWRK）
  - 数値 → `Convert.ToDouble(value)` → `numVal.ToString(field.FormatString)`
  - 文字列 → `PadRightByte(text, byteLen)`（Shift-JISバイト単位でパディング）

- **型変換リスク**: `FixedLengthFileWriter.vb:59` の `Convert.ToDouble(value)` は try/catch で保護されているが、想定外の型（Decimal等）が来ると精度が失われる可能性がある。

### CSV出力

- `CrudHelper.vb` 内に `ToCsvFile` メソッドの参照あり（実装は `FileHelper` クラスと思われる）。ソース未確認。

---

## 11. 命名規則・コーディング規約

### ファイル命名規則
- ビジネスロジック: `<機能名>Builder.vb`, `<機能名>Engine.vb`, `<機能名>Manager.vb`
- 型定義: `<機能名>Types.vb`
- SQL組立て: `<機能名>SqlBuilder.vb`

### 変数/プロパティ命名規則
- Access版の日本語または英略語をそのまま踏襲: `Lsryo`（リース料）, `Zei`（税額）, `Kyakh`（契約書）, `Kykm`（物件）, `GInt`, `GKasan` 等
- DBカラム名は Access版のスネークケースをそのまま使用: `kykm_kykm_id`, `shri_cnt`

### コメント規約
- クラス/メソッドには `'''<summary>` XML ドキュメントコメントあり
- Access版対応関係は `' Access版 XXX 相当` または `' Access版 XXX の忠実移植` と明記
- Access版のバグを意図的に再現した箇所は `' Access版バグ:` コメントあり

---

## 12. 技術的制約・注意事項

### 12.1 浮動小数点精度
- 業務計算には `Double` のみ使用（`Decimal` は未使用）。Access版VBAが `Double` のため。
- `GInt` による G15 文字列往復が精度の要。Access版と数値が一致しない場合はここが原因になりやすい。

### 12.2 VBA互換丸め
- `CLng` による Banker's Rounding は `MidpointRounding.ToEven` で再現。
- `CInt` による Banker's Rounding も同様（VB.NETの `CInt` はデフォルトで Banker's Rounding を使用するため自然に互換）。

### 12.3 DBNull 取り扱い
- DB値は `DataRow` 経由で `Object` 型として来る。`IsDBNull` チェック漏れが InvalidCastException の原因になる。
- 安全なパターンは `GetDbl`/`GetInt` ヘルパー経由だが、一部コードで直接変換が残存。

### 12.4 Access版の既知バグの再現
- `ChukiCalcEngine.vb` の非月末決算分岐（ig決算日≠31）はAccess版のコピペバグを意図的に再現している。実運用では通らない分岐だが、ブラックボックステストでは検証対象外とする（またはAccess版と一致することを確認する）。

### 12.5 Shift-JIS 固定長出力
- `FixedLengthFileWriter.PadRightByte` はバイト単位でパディングするため、全角文字（2バイト）と半角（1バイト）が混在する文字列のサイズ計算に注意が必要。
