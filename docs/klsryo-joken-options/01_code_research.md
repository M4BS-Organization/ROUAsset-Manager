# コードベース調査資料: klsryo-joken-options

## 1. プロジェクト概要
- フレームワーク・言語: VB.NET 4.7.2 / WinForms
- データアクセス: Npgsql (PostgreSQL)
- 主要な依存ライブラリ: Npgsql, System.Data

### ディレクトリ構成（対象機能周辺）
```
C:\kobayashi_LeaseM4BS\
├── LeaseM4BS\LeaseM4BS.DataAccess\
│   ├── KlsryoCalculationEngine.vb   # 計算エンジン本体
│   └── KlsryoTypes.vb               # 列挙型・型定義
└── LeaseM4BS.TestWinForms\LeaseM4BS.TestWinForms\
    ├── Form_f_KLSRYO_JOKEN.vb        # 条件入力フォーム (本Issue対象)
    ├── Form_f_KLSRYO_JOKEN.Designer.vb
    ├── Form_f_flx_KLSRYO.vb          # 集計結果表示フォーム
    ├── Form_f_KEIJO_JOKEN.vb          # 参考: 月次仕訳計上条件フォーム
    └── Form_f_KEIJO_JOKEN.Designer.vb
```

## 2. アーキテクチャ概要
- アーキテクチャパターン: Layered (WinForms UI + DataAccess層)
- JOKEN フォームが条件を受け取り、flx フォームへパラメータを渡す → flx フォームが計算エンジンを呼び出す構造
- 計算ロジックはすべて DataAccess 層の Engine クラスに集約

### レイヤー構成
| レイヤー | 責務 |
|---|---|
| `Form_f_KLSRYO_JOKEN` | ユーザー条件入力UI、パラメータを `Form_f_flx_KLSRYO` へ渡す |
| `Form_f_flx_KLSRYO` | 計算エンジン呼び出し、結果をDataGridViewに表示 |
| `KlsryoCalculationEngine` | SQL組み立て・実行・集計演算 |
| `KlsryoTypes.vb` | 列挙型・データクラス定義 |

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_KLSRYO_JOKEN.vb` | 条件入力フォーム（本Issue対象） | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_KLSRYO_JOKEN.Designer.vb` | フォームデザイナー（UI定義） | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_flx_KLSRYO.vb` | Taisho/Ktmg/Meisai プロパティ受取・エンジン呼び出し | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` | taisho/ktmg/meisai を実際に使用する計算エンジン | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoTypes.vb` | ShriKtmg・ShriMeisai 列挙型定義 | 高 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_KEIJO_JOKEN.vb` | 参考: ラジオボタン実装パターン（radio_BUKN/radio_HAIF） | 中 |
| `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/Form_f_KEIJO_JOKEN.Designer.vb` | 参考: GroupBox + Panel + RadioButton 構造 | 中 |

## 4. 使用パターン

### 状態管理
- JOKEN フォームはプロパティ (`DtFrom`, `DtTo`, `Taisho`, `Ktmg`, `Meisai`) で状態を保持
- `Form_f_flx_KLSRYO.vb:13-15` でデフォルト値を定義、JOKEN フォームから上書き

### ラジオボタン実装パターン (Form_f_KEIJO_JOKEN 参考例)

**Designer.vb での定義方法:**
- `GroupBox` の中に `Panel` を配置、`Panel` 内に `RadioButton` を複数配置
- グループ化は Panel への格納で自動管理（同一 Panel 内の RadioButton はグループ化）
- デフォルト選択は `Checked = True` + `TabStop = True` で設定

```vb
' Form_f_KEIJO_JOKEN.Designer.vb:158-178
Me.radio_BUKN = New System.Windows.Forms.RadioButton()
Me.radio_HAIF = New System.Windows.Forms.RadioButton()

' radio_BUKN (物件単位)
Me.radio_BUKN.AutoSize = True
Me.radio_BUKN.Location = New System.Drawing.Point(14, 20)
Me.radio_BUKN.Name = "radio_BUKN"
Me.radio_BUKN.Text = "物件単位"

' radio_HAIF (配賦単位) - デフォルト選択
Me.radio_HAIF.AutoSize = True
Me.radio_HAIF.Checked = True
Me.radio_HAIF.TabStop = True
Me.radio_HAIF.Text = "配賦単位"
```

**コードビハインドでの読み取りパターン:**
```vb
' Form_f_KEIJO_JOKEN.vb:52-53
Dim joken As New KeijoJoken() With {
    .Meisai = If(radio_BUKN.Checked, ShriMeisai.Kykm, ShriMeisai.Haif),
    ...
}
```

### データアクセス
- `KlsryoCalculationEngine.Execute()` が `taisho (Integer)`, `ktmg (ShriKtmg)`, `meisai (ShriMeisai)` を受け取る
- `GetSourceData()` 内でSQLのWHERE句を動的に組み立て (taisho で `kkbn_id` フィルタ)
- `Execute()` の Select Case で meisai に応じて処理メソッドを分岐

### エラーハンドリング
- `Form_f_flx_KLSRYO.SearchData()` が Try-Catch で `MessageBox.Show("一覧取得エラー: " & ex.Message)` を表示

## 5. 既存の類似機能分析

### Form_f_KEIJO_JOKEN.vb の明細ラジオボタン実装（完成例）

`Form_f_KEIJO_JOKEN` は Meisai（物件/配賦）に対してラジオボタンを実装済みで、最も直接的な参考実装。

| 項目 | Form_f_KEIJO_JOKEN | Form_f_KLSRYO_JOKEN（現状） |
|---|---|---|
| 明細ラジオ (Meisai) | `radio_BUKN` / `radio_HAIF` で実装済み | `オプション487`（物件）/ `オプション489`（配賦）がDesignerに存在するが未接続 |
| 集計対象 (Taisho) | `Taisho = 3` にハードコード | `オプション504`（リース）/ `オプション506`（保守）/ `オプション508`（全部）がDesignerに存在するが未接続 |
| タイミング (Ktmg) | 未実装 | `chk_SHIME`（締支払）/ `オプション483`（支払日）がDesignerに存在するが未接続 |

### 重要な発見: Designer.vb にラジオボタンが既に定義されている

`Form_f_KLSRYO_JOKEN.Designer.vb` にはラジオボタンがすでに宣言・配置されているが、
`Form_f_KLSRYO_JOKEN.vb` の実行ボタンハンドラでは読み取っておらず、ハードコード値を使っている。

**既存のラジオボタン制御一覧 (Designer.vb):**

| コントロール名 | 表示テキスト | 対応するパラメータ | 初期選択 | 配置 |
|---|---|---|---|---|
| `chk_SHIME` | 〆支払ベース | Ktmg=? (未明: 「締日ベース」か?) | False | Panel1 (GroupBox1「計算方法」) |
| `オプション483` | 支払日ベース | Ktmg=ShriDtBase | True (Checked) | Panel1 |
| `オプション504` | リース料 | Taisho=1 | False | Panel3 (GroupBox2「計算対象」) |
| `オプション506` | 保守料 | Taisho=2 | False | Panel3 |
| `オプション508` | 全部 | Taisho=3 | True (Checked) | Panel3 |
| `オプション487` | 物件単位 | Meisai=ShriMeisai.Kykm | False | Panel2 (GroupBox2「計算対象」) |
| `オプション489` | 配賦単位 | Meisai=ShriMeisai.Haif | True (Checked) | Panel2 |

**注意:** `chk_SHIME` は `RadioButton` として宣言されているが名前が「chk_」プレフィックスで統一されていない。
また `オプション483` が「支払日ベース」で Checked=True になっているが、現在のハードコードでは `SimeDtBase`（締日ベース）が設定されているため、UIと実際の動作が逆転している可能性がある。

### 再利用可能なパターン
- `GetDuration()`: 期間計算ヘルパー（BaseForm系のユーティリティ）
- `SwapIf()`: FROM/TO 順序補正ユーティリティ
- `HandleEnterKeyNavigation()`: エンターキーナビゲーション

## 6. 技術的制約・注意事項

### Taisho パラメータの型
- `Form_f_flx_KLSRYO.vb:13`: `Public Property Taisho As Integer = 3`
- `KlsryoCalculationEngine.Execute()`: `taisho As Integer`
- 列挙型ではなく `Integer` のまま（1=リース料, 2=保守料, 3=全部）
- Access版 `opg_TAISHO` の値（1/2/3）と対応

### Ktmg パラメータ
- `ShriKtmg` 列挙型: `Nothing_=0`, `SimeDtBase=1`, `ShriDtBase=2`, `ShriRDtBase=3`
- Designer の `chk_SHIME`「〆支払ベース」と `オプション483`「支払日ベース」の 2 択のみ
- Access版 `opg_KTMG`: 1=締日ベース, 2=支払日ベース
- `ShriRDtBase=3`（前月支払日ベース）はUI上での選択肢なし

### UIとロジックの不整合（要確認）
- `オプション483`（支払日ベース）が `Checked=True` (デフォルト選択)
- しかし `Form_f_KLSRYO_JOKEN.vb:41` でハードコード: `frm.Ktmg = ShriKtmg.SimeDtBase`（締日ベース）
- ラジオボタンを有効化する際に、デフォルト選択を `chk_SHIME`（締日ベース）に変更する必要がある

### Designer.vb コントロール命名の問題
- `オプション483`, `オプション487` 等のJapanese名は Access版のデザイナー名をそのまま移植した可能性
- `Form_f_KEIJO_JOKEN` の `radio_BUKN`, `radio_HAIF` のような英語名に統一するか検討が必要

### 付随費用処理 (Taisho との関連)
- `KlsryoCalculationEngine.Execute():87`: `If taisho = 2 OrElse taisho = 3 Then ProcessHenf(...)`
- Taisho=1（リース料のみ）の場合は付随費用処理がスキップされる
- この動作は意図的であり、ラジオボタン有効化後も保持する必要がある

### Access版ソースの参照
- Access版ソースは `.mdb` バイナリ形式のみ（`c:\access_LeaseM4BS\LM4BS.mdb`）
- VBAソース直接参照不可。`docs/klsryo-classification/01_code_research.md` に Access版情報が既に調査済み
- Access版定数: `engSHRI_KTMG: 1=締日ベース, 2=支払日ベース` / `opg_TAISHO: 1=リース料, 2=保守料, 3=全部`

## 7. 命名規則・コーディング規約

### コントロール命名規則
- ラジオボタン: `radio_` プレフィックス（Form_f_KEIJO_JOKEN: `radio_BUKN`, `radio_HAIF`）
- チェックボックス: `chk_` プレフィックス
- テキストボックス: `txt_` プレフィックス
- ボタン: `cmd_` プレフィックス
- **現状の問題**: Designer.vb の `オプション483` 等は日本語命名で規約違反

### 推奨する新コントロール名（追加実装時）
| 対応する Access opg | 推奨名 | 意味 |
|---|---|---|
| opg_TAISHO=1 | `radio_LSRYO` | リース料 |
| opg_TAISHO=2 | `radio_HOSHU` | 保守料 |
| opg_TAISHO=3 | `radio_ZENBU` | 全部 |
| opg_KTMG=1 | `radio_SIME` | 締日ベース |
| opg_KTMG=2 | `radio_SHRI` | 支払日ベース |
| opg_MEISAI=1 | `radio_BUKN` | 物件単位 |
| opg_MEISAI=2 | `radio_HAIF` | 配賦単位 |

### プロパティ・変数命名
- PascalCase: `Taisho`, `Ktmg`, `Meisai`, `DtFrom`, `DtTo`
- Private フィールド: `_prevForm` (`_` プレフィックス + camelCase)

### コードビハインドパターン
```vb
' cmd_EXECUTE_Click での読み取りパターン (Form_f_KEIJO_JOKEN 参照)
frm.Meisai = If(radio_BUKN.Checked, ShriMeisai.Kykm, ShriMeisai.Haif)

' Taisho の場合 (Integer型)
frm.Taisho = If(radio_LSRYO.Checked, 1, If(radio_HOSHU.Checked, 2, 3))

' Ktmg の場合
frm.Ktmg = If(radio_SIME.Checked, ShriKtmg.SimeDtBase, ShriKtmg.ShriDtBase)
```
