## H2-9: カスタマイズ体系

リースM4BSは複数の顧客に導入されており、顧客ごとの業務要件に対応するため、カスタマイズ体系が整備されている。

### 顧客タイプ一覧（26種類）

起動時に `T_Customize` テーブルから読み込まれた値が `igCUSTM_TYPE` グローバル変数に格納され、顧客タイプに応じた処理分岐が行われる。

| タイプNo | 顧客識別子 | 備考 |
|---------|-----------|------|
| 0 | STD（標準） | デフォルト動作 |
| 1 | DKO | |
| 2 | DNS | |
| 3 | VTC | 一部検証コメント化（柔軟対応） |
| 4 | MYCOM | 資産/負債二重構造、子会社/親会社別、支払伝票印刷統合 |
| 5 | NIFS | |
| 6 | SNKO | 最終確認画面統合、異動データ検証 |
| 7 | RISO | TAISHO=3, MEISAI=2 の厳格チェック |
| 9 | SANKO_AIR | |
| 10 | SAKURA_IS | |
| 11 | YAMASHIN_F | |
| 12 | KITOKU | 固定長テキスト出力（CMSW2WRK/APGDHWRK 等） |
| 13 | FUJISASH | |
| 14 | TCCB | |
| 15 | NIDEC_SHIBA | |
| 16 | JOT | pc_JOT_仕訳出力_COM で共通化 |
| 17 | KYOTO | |
| 18 | MARUZEN | |
| 19 | NKSOL | SW_RSOK 検証（残高差額計算済み確認） |
| 20 | TSYSCOM | 月初/月末両対応、仕訳No シーケンス 4000 |
| 21 | KINTETSU_IS | |
| 22 | VALQUA | 長短振替仕訳あり |
| 23 | NIPPAN_R | |
| 24 | CACMARUHA | |
| 25 | KINTETSU_RE | |
| 26 | STD_SWK | |

> タイプNo 8 は欠番。

### T_Customize テーブルのフラグ体系

顧客タイプ（`igCUSTM_TYPE`）に加え、`T_Customize` テーブルには細粒度の動作制御フラグが格納されている。

| フラグ項目 | 概要 |
|-----------|------|
| ドラッシュリース捨却禁止 | ドライフリースの除却操作を禁止する |
| 更新予定額計算 | リース更新時の予定額を自動計算する |
| サーバーDBはOracle | バックエンドDB が Oracle の場合に有効化 |
| APログイン必須 | AP 経由でのログインを必須とする |
| タイプ50に切替 | 特定処理を代替ロジック（タイプ50）に切り替える |
| 期間別フロー表示 | 期間ごとのキャッシュフロー表示を有効化 |
| 使用禁止フラックス | 特定フラグの使用を禁止する |
| システム構成の保持あり | システム設定を保持モードで管理する |
| 計上メニュー表示 | 計上仕訳メニューを表示する |
| 仕訳出力 | 仕訳出力機能を有効化する |

### 顧客固有フォーム（fc_ 系）のパターン

顧客固有の仕訳出力フォームは `fc_` プレフィックスで管理されており、標準フォームとは独立して保守される。全240画面のうち52画面が `fc_` フォームである。

主な用途は仕訳出力のカスタマイズであり、以下の出力形式が存在する。

| 出力形式 | 代表顧客 |
|---------|---------|
| Excel | MYCOM, SNKO, JOT, RISO, NKSOL, VTC, VALQUA |
| CSV | TSYSCOM |
| 固定長テキスト | KITOKU（CMSW2WRK/APGDHWRK/APGDDWRK/APGDSWRK） |
| テキスト/標準 | 標準仕訳（KITOKU ベース共通フォーマット） |

---

## H2-10: セットアップ・配布構成（AI_Upload）

`AI_Upload` フォルダはエンドユーザー向けのセットアップ一式を収めた配布スイートである。

### setup.inf 設定内容

```ini
# 主要設定項目（抜粋）
対象ランタイム : Access 2016 64bit Runtime
インストール先 : C:\LeaseM4BS\
```

Access Runtime のバージョンと配置先パスが固定されており、インストーラーが `setup.inf` を参照してセットアップを進める。

### 配布ファイル一覧

| ファイル名 | 種別 | 役割 |
|-----------|------|------|
| LM4BS.accdr | Access ランタイム形式 DB | メインアプリケーション |
| LM4BS.mdb | Access MDB | フォーム・VBA 本体 |
| LM4BSCustomize.mdb | Access MDB | 顧客別カスタマイズ定義 |
| LM4BSImpWkDB.mdb | Access MDB | Excel 取込ワーク用 |
| LM4BSmdld.mdb | Access MDB | モデルデータ用 |
| LM4BSwork.mdb | Access MDB | 一般ワーク用 |
| SecSet2016.vbs | VBScript | Access 信頼できる場所のレジストリ登録 |
| LM4_delete.wsf | WSF スクリプト | アンインストール処理 |
| pc_IniFile.vbs | VBScript クラス | INI ファイル読み書き |
| p_Com.vbs | VBScript | 共通ユーティリティ（40 関数以上） |
| _MessageForm_.exe | .NET 実行ファイル | メッセージダイアログ（.NET 2.0/4.0） |
| _MessageForm_.exe.config | .NET 設定ファイル | .NET ランタイム設定 |

### セキュリティ設定（SecSet2016.vbs）

Access は信頼できる場所として登録されていないフォルダ内の MDB を開くとマクロが無効化される。`SecSet2016.vbs` はこの制約に対応するため、インストール先フォルダを Windows レジストリの「信頼できる場所」に登録する。UAC（ユーザーアカウント制御）に対応した昇格処理も含まれている。

```
レジストリ登録先（例）:
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Office\16.0\Access\Security\Trusted Locations\...
```

### インストール / アンインストール

**インストール手順（概略）:**
1. `setup.inf` に基づき Access 2016 64bit Runtime を展開
2. `SecSet2016.vbs` を実行してレジストリへ信頼できる場所を登録
3. 配布 MDB 群を `C:\LeaseM4BS\` に配置
4. `_MessageForm_.exe` を配置（.NET ランタイム依存）

**アンインストール（LM4_delete.wsf）:**
- 配置ファイルの削除
- レジストリエントリの削除
- バックアップデータの処理

---

## H2-11: VB.NET版移行状況

Access 版からのモダン化を目的として VB.NET（Windows Forms）への移行プロジェクトが進行中である。

### プロジェクト構成

```
LeaseM4BS.slnx
├── LeaseM4BS.DataAccess   # PostgreSQL データアクセス層
└── LeaseM4BS.TestWinForms # WinForms テストプロジェクト（約557フォーム）
```

- ターゲットフレームワーク: .NET Framework 4.7.2
- 言語: VB.NET
- RDBMS: PostgreSQL（Npgsql 6.0.11）

### データアクセス層（完成）

データアクセス層は **完成済み** であり、Access VBA の DAO.Recordset を代替する汎用クラスが実装されている。

#### DbConnectionManager

| 機能 | 詳細 |
|------|------|
| 接続文字列取得 | `App.config` または環境変数から取得 |
| デフォルト接続先 | `Host=localhost;Port=5432;Database=lease_m4bs` |
| 接続テスト | 起動時に疎通確認 |
| パスワード表示 | ログ出力時にマスク処理 |

#### CrudHelper（DAO.Recordset 代替）

| メソッド | 役割 |
|---------|------|
| `GetDataTable` | SELECT 結果を DataTable で取得 |
| `ExecuteNonQuery` | INSERT / UPDATE / DELETE 実行 |
| `ExecuteScalar<T>` | 単一値の取得 |
| `Insert` | レコード挿入 |
| `Update` | レコード更新 |
| `Delete` | レコード削除 |
| `Exists` | レコード存在確認 |
| `Begin/Commit/Rollback` | トランザクション管理 |

### UI 基盤（完成）

#### FormHelper

Access フォームの動作をWinForms 上で再現するためのヘルパークラス。

| 機能カテゴリ | 主なメソッド / 機能 |
|------------|------------------|
| ComboBox | `Bind(SQL)` でSQL バインド、`AdjustSize`、複数列表示（Access 風罫線付き） |
| 自動同期 | `SyncTo`: ComboBox → TextBox 自動同期（`=Column(x)` の再現） |
| DataGridView | `HideColumns`、`FormatColumn`、`GetSelectedRow` |
| 複数 DGV 同期 | `SyncDgvScroll` / `SyncDgvColumnWidths` |

#### FileHelper

| メソッド | 状態 | 詳細 |
|---------|------|------|
| `ToExcelFile` | 完成 | Excel 出力（Interop 使用） |
| `ToCsvFile` | 完成 | CSV 出力（Shift_JIS エンコード） |
| `ToFixedLengthFile` | **未完成** | 固定長ファイル出力（KITOKU 顧客等で必要） |

#### CalendarColumn

DataGridView 内のセルで `DateTimePicker` を利用できるカスタムカラム。Access のグリッド上での日付入力を再現する。

### 移行済み画面一覧

| カテゴリ | 移行済み画面（主要） |
|---------|-----------------|
| マスタ | BCAT, BKNRI, KKNRI, SKMK, HKMK 等 |
| 契約入力 | ContractEntry（59KB）, BuknEntry（17KB） |
| システム | 0F_SYSTEM, 0F_SYSTEM管理, ログ画面群 |
| 検索・出力 | FlexSearchDLG 系, FlexOutputDLG 系, FlexReportDLG 系 |
| 計算 | CHUKI_RECALC, CHUKI_SCH, 年金現価計算 |

TestWinForms プロジェクトには約 **557 フォーム** が存在し、Access 版の 240 画面を大幅に上回る規模で移行・テスト用画面が実装されている。

### 未完成項目

| 項目 | 詳細 |
|-----|------|
| 固定長ファイル出力 | `FileHelper.ToFixedLengthFile` が未実装。KITOKU 顧客など固定長テキスト形式が必要な顧客に影響 |
| 複雑な業務計算処理 | 月次仕訳計上・債務スケジュール等の一部ロジックが未移行 |
| 顧客固有仕訳出力 | `fc_` 系フォーム 52 画面の移行が未着手または途中 |

### 完成度サマリ

| レイヤ | 完成度 | 備考 |
|-------|-------|------|
| データアクセス層 | **完成** | DbConnectionManager, CrudHelper |
| UI 基盤（FormHelper/CalendarColumn） | **完成** | Access の主要 UI パターンを網羅 |
| FileHelper | **一部未完** | 固定長出力のみ未実装 |
| マスタ・システム系画面 | **概ね完成** | 主要マスタおよびシステム管理画面は移行済み |
| 業務計算（仕訳計上等） | **一部未完** | 複雑な計算ロジックが残存 |
| 顧客固有仕訳出力（fc_ 系） | **未完成** | 26 顧客分の固有ロジックが残課題 |

---

## H2-12: まとめ・今後の課題

### Access 版の規模感

リースM4BSのAccess版は、長年にわたる顧客対応を経て大規模なシステムに成長している。

| 指標 | 数値 |
|-----|------|
| フォーム数 | 240 画面（うち顧客固有 fc_ 系 52 画面） |
| VBA モジュール数 | 373 モジュール |
| 対応顧客数 | 26 社（タイプ 0〜26） |
| 仕訳出力バリエーション | Excel / CSV / 固定長テキスト / 標準テキスト |
| セキュリティ | 3 段階アクセス制御 + パスワードポリシー |

### VB.NET 移行の進捗

移行プロジェクトの基盤部分（データアクセス層・UI 基盤）は完成しており、主要なマスタ画面と契約入力画面も移行済みである。一方で、業務の中核となる仕訳計算・仕訳出力の顧客固有ロジックは大部分が未完成であり、移行プロジェクトの後半フェーズに残されている。

```
移行完了レイヤ（基盤）:
  [完成] データアクセス層（PostgreSQL / Npgsql）
  [完成] UI 基盤（FormHelper, CalendarColumn）
  [完成] マスタ・システム管理系画面

移行途中レイヤ（業務ロジック）:
  [一部] 業務計算（月次仕訳計上・債務スケジュール等）
  [未完] 顧客固有仕訳出力（fc_ 系 52 画面・26 顧客分）
  [未完] 固定長ファイル出力（FileHelper.ToFixedLengthFile）
```

### 残課題の整理

移行プロジェクトを完遂するにあたり、以下の課題を優先度別に整理する。

#### 優先度: 高

| 課題 | 内容 | 影響範囲 |
|-----|------|---------|
| 顧客固有仕訳出力の移行 | fc_ 系 52 画面の VB.NET 実装。26 顧客それぞれの出力形式（Excel/CSV/固定長）に対応する必要がある | 全顧客の月次業務に直結 |
| 固定長ファイル出力の実装 | `FileHelper.ToFixedLengthFile` を完成させる。KITOKU など固定長形式必須の顧客が存在する | 特定顧客の仕訳連携に直結 |
| 月次仕訳計上ロジックの移行 | Access VBA の `pc_月次仕訳計上` に相当する複雑な業務計算を VB.NET に移植する | 全顧客の月次処理に直結 |

#### 優先度: 中

| 課題 | 内容 | 影響範囲 |
|-----|------|---------|
| 返済スケジュール・償却スケジュール移行 | `gMake返済_SCH`・`mMake償却_SCH` 等の数値計算ロジックの精度検証と移植 | 計算結果の正確性に影響 |
| 顧客タイプ別メニュー制御 | `igCUSTM_TYPE` に応じたメニュー表示/非表示ロジックの VB.NET 再現 | 顧客ごとの操作性に影響 |
| セキュリティモデルの完全移植 | 3 段階アクセス制御（ACCESS_KIND 1/2/3）および契約管理単位別権限の実装 | セキュリティ要件 |

#### 優先度: 低

| 課題 | 内容 | 影響範囲 |
|-----|------|---------|
| 帳票・レポート条件画面の移行 | 棚卸一覧・リース債務一覧等の各種レポート条件画面（f_*_JOKEN 系） | レポート出力機能 |
| データ取込機能の移行 | Excel インポート・各種取込画面（f_IMPORT 系）の移行 | 初期データ投入・定期取込 |
| ツール画面の移行 | 消費税改正・中途解約ツール等の補助ツール画面 | 保守運用 |

### 今後の開発方針

1. **業務計算ロジックの先行移行**: 仕訳計上・スケジュール計算はシステムの根幹であり、早期に移行・テスト検証を完了させることが全体スケジュールの鍵となる。
2. **顧客固有実装の標準化**: 26 顧客分の `fc_` 系フォームを個別に移植するのではなく、仕訳出力の共通インターフェースを設計し、顧客ごとの差分を設定・プラグインとして管理する構造が望ましい。
3. **Access 版との並行稼働期間の設定**: 移行完了までの間、Access 版と VB.NET 版の計算結果を定期的に突き合わせ、ロジックの等価性を確認するテスト体制を整備する。
4. **固定長出力の早期完成**: KITOKU 顧客のような既存連携に依存している顧客は、`ToFixedLengthFile` の完成なしには VB.NET 版に切り替えられないため、優先度を上げて対処する。
