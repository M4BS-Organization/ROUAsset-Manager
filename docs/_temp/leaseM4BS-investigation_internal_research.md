# リースM4BS 内部調査結果（5エージェント統合）

## 1. AccessGUI フォーム構成（240画面）

### 分類

| 分類 | プレフィックス | 件数 | 用途 |
|------|-------------|------|------|
| メインフォーム | f_ | 183 | 業務画面（契約書、物件、仕訳等） |
| 顧客固有フォーム | fc_ | 52 | 顧客別カスタマイズ（仕訳出力等） |
| フレックスリスト | f_flx_ | 40 | 一覧表示・グリッド画面 |
| マスタ入力 | f_M_ | 22 | マスタデータ保守 |
| 参照画面 | f_REF_ | 9 | 読み取り専用参照 |
| システム管理 | 0F_/0f_ | 4 | システム設定・管理 |

### 主要画面

**契約書（KYKH）関連:**
- f_KYKH: 契約書主画面
- f_KYKH_SUB: サブ情報
- f_KAIYAK: 解約画面
- f_REF_D_KYKH: 参照画面
- f_flx_D_KYKH: 一覧

**物件（KYKM）関連:**
- f_KYKM: 物件主画面（タブ構成: 基本/分割/中途）
- f_KYKM_SUB: サブ情報
- f_KYKM_BKN: 分割管理
- f_KYKM_CHUUKI: 注記情報
- f_KYKM_CHUUKI_SUB_GSON: 減損サブ
- f_REF_D_KYKM系: 参照画面群
- f_flx_D_KYKM: 物件一覧

**変更・異動関連:**
- f_HEN_SCH: 返済スケジュール変更
- f_HENF: 返金（変更支払情報）
- f_HENL: 変更履歴
- f_IDO / f_IDO_SUB: 異動処理

**仕訳出力関連:**
- f_仕訳出力標準_SM: 支払仕訳出力（支払管理）
- f_仕訳出力標準_SH: 支払仕訳出力（支払処理）
- f_仕訳出力標準_KJ: 計上仕訳出力
- f_仕訳出力標準_設定_MAIN/SM/SH/KJ: 設定画面
- fc_系: 顧客別仕訳出力フォーム多数

**マスタメンテナンス:**
- f_M_CORP_INP: 会社 / f_M_GSHA_INP: リース会社 / f_M_BCAT_INP: 物件分類
- f_M_BKIND_INP: 物件種別 / f_M_BKNRI_INP: 物件管理
- f_M_LCPT_INP: リース料金パターン / f_M_MCPT_INP: 月額料金
- f_M_GENK_INP: 原価 / f_M_HKHO_INP: 配賦方法 / f_M_HKMK_INP: 配賦目的
- f_M_KKNRI_INP: 契約管理単位 / f_M_KOZA_INP: 口座
- f_M_SWPTN_INP: 仕訳パターン / f_M_SKMK_INP: 仕訳科目
- f_M_SHHO_INP: 支払方法
- f_M_RSRV系: 予備マスタ（SANKO等の顧客固有）
- f_SEC_USER_INP: ユーザー / f_SEC_KNGN_INP: 権限グループ
- 各マスタに対応するf_flx_M_*一覧画面あり

**レポート・帳票条件:**
- f_TANA_JOKEN: 棚卸一覧表 / f_KLSRYO_JOKEN: リース料一覧表
- f_TOUGETSU_JOKEN: 当月支払 / f_KEIJO_JOKEN: 計上仕訳
- f_CHUKI_JOKEN: 注記対象 / f_ZANDAKA_JOKEN: 残高一覧
- f_SAIMU_JOKEN: リース債務 / f_YOSAN_JOKEN: 予算
- f_BEPPYO2_JOKEN: 別表16(4)
- f_KHIYO_JOKEN: 費用 / f_経費明細表_JOKEN: 経費明細
- f_IDOLST_JOKEN: 異動一覧
- 各条件画面に対応するf_flx_*一覧・スケジュール画面あり

**データ取込:**
- f_IMPORT: Excelインポート / f_IMPORT_LOG: インポートログ
- f_IMPORT_最終確認: インポート確認 / f_IMPORT_最終確認_SUB_KYKH/MST
- f_更新解約取込 / f_減損損失取込 / f_部署取込 / f_契約書変更情報取込

**ツール・その他:**
- f_T_HOLIDAY: 祝日設定 / f_T_KARI_RITU: 仮利率（追加借入利子率）
- f_T_ZEI_KAISEI: 消費税改正 / f_T_KYKBNJ_SEQ: 契約番号シーケンス
- f_KIRIKAE: 画面切替 / f_StatusMeter: 進捗表示
- f_SAILEASE: セールアンドリースバック
- f_合算: 合算処理 / f_支払照合: 支払照合
- f_中途解約ツール / f_税率変更ツール

### GUI定義ファイル構造
- Access Form定義言語 Version 21
- Begin Form ... End構造
- RecordSourceでデータソース指定（tw_プレフィックスのテーブル/ビュー）
- イベントハンドラ（OnOpen, OnClose等）= [Event Procedure]でVBA連携
- NameMap: コントロール名のUnicode/16進エンコード
- PrtDevMode: プリンター設定（16進数バイナリ）

---

## 2. AccessVBA コアロジック（373モジュール）

### システム起動フロー
```
p_StartUp_gMain()
  ├─ Accessバージョン確認（2000～2016対応）
  ├─ 和暦配列 tgEras() 初期化（明治～令和）
  ├─ インストールフォルダチェック（ProgramFiles警告）
  ├─ ACCESS最大化
  ├─ ワークDB初期化
  │   ├─ LM4BSwork.mdb（ワーク用）
  │   ├─ LM4BSmdld.mdb（モデル用）
  │   └─ LM4BSImpWkDB.mdb（Excel取込用）
  ├─ カスタマイズ設定読込（T_Customize → igCUSTM_TYPE）
  ├─ メニューバー生成（tcon_MenuMatrix → 5階層メニュー）
  ├─ セキュリティ設定（権限グループ別メニュー制御）
  └─ ログ初期化（Tempフォルダに LM42log.mdb）
```

### 主要テーブル体系

**業務データ:**
- D_KYKH: 契約書ヘッダ（KYKH_ID, 契約者名等）
- D_KYKM: 契約書明細（物件情報）
- D_HAIF: 配分データ
- D_HENL: 変更履歴
- D_GSON: 減損データ

**計算結果:**
- tw_S_CHUKI_KEIJO: 注記計上結果
- tw_D_HENL_KEIJO: 変更償却仕訳
- tw_D_GSON_KEIJO: 減損仕訳

**マスタ:**
- M_CORP: 企業 / M_KKNRI: 契約管理単位 / M_LCPT: リース料金パターン
- M_SHHO: 支払方法 / M_GENK: 原価 / M_BCAT: 物件分類
- M_HKMK: 配賦目的 / M_SKMK: 仕訳科目 / M_BKIND: 物件種別
- M_KOZA: 口座 / M_GSHA: リース会社 / M_MCPT: 月額料金
- M_HKHO: 配賦方法 / M_SWPTN: 仕訳パターン

**システム:**
- T_SYSTEM: システム設定 / T_Customize: カスタマイズフラグ
- tcon_MenuMatrix: メニュー定義 / tcon_MDBList: MDBパス管理
- tcon_Message: メッセージマスタ / SEC_USER: ユーザー
- L_SLOG: 操作ログ / L_ULOG: 更新ログ / L_BKLOG: バックアップログ

### リース計算ロジック

**月次仕訳計上フロー（pc_月次仕訳計上）:**
```
g月次_v計_契約書to料金TBL_計上()
  ├─ 対象期間の契約データ抽出
  ├─ ループ処理（各契約ごと）
  │  ├─ m月次_v計_SQLMAKE() - SQL生成
  │  ├─ m月次_v計() - 利息計算
  │  │  ├─ m月次_v計_SUB_償却関連()
  │  │  ├─ mMake償却_SCH() - 償却スケジュール生成
  │  │  └─ gMake返済_SCH() - 返済スケジュール生成
  │  └─ mMakeFUSAI_SCH() - 債務スケジュール生成
  ├─ m利用_結果テーブル入力()
  └─ m登録処理()
```

**主要計算:**
- 年金現価計算（0f_MNT_tcon_年金現価の計算式）
- 返済スケジュール生成（gMake返済_SCH）
- 利息計算（利息 = リース価額 × 利率 / 12 × 月数）
- 償却スケジュール（定額法・利息法）
- 債務スケジュール（長短振替含む）

### セキュリティモデル

**認証体系:**
- typ_gLogin構造体: UserCD, PWD, KngnCd, boAdmin, boMaster, boAPPROVAL, boLOG, boFOutKNGN, boPriKNGN
- パスワードポリシー: 有効期限, 猶予期間, 最小文字数, 英字/数字/記号チェック
- ログイン試行制限（cn_gLOGIN_ATTEMPTS_SYS = 5）
- 暗号化: pc_Encrypt使用

**アクセス権限体系:**
- ACCESS_KIND: 全データ更新(1) / 全データ参照(2) / 管理単位限定(3)
- 契約管理単位(tgKKNRI_LIST) / 部門管理単位(tgBKNRI_LIST)ごとの権限
- メニュー表示/非表示制御（mMENUBAR_ENABLE_CTL）

**予約ユーザー:**
- system (ID=1): システム管理者
- iltex (ID=0): イルテックス保守用

### カスタマイズ体系

**顧客タイプ（igCUSTM_TYPE, 26種類）:**
0=標準, 1=DKO, 2=DNS, 3=VTC, 4=MYCOM, 5=NIFS, 6=SNKO, 7=RISO,
9=SANKO_AIR, 10=SAKURA_IS, 11=YAMASHIN_F, 12=KITOKU, 13=FUJISASH,
14=TCCB, 15=NIDEC_SHIBA, 16=JOT, 17=KYOTO, 18=MARUZEN, 19=NKSOL,
20=TSYSCOM, 21=KINTETSU_IS, 22=VALQUA, 23=NIPPAN_R, 24=CACMARUHA,
25=KINTETSU_RE, 26=STD_SWK

**カスタマイズフラグ（T_Customizeテーブル）:**
- ドラッシュリース捨却禁止, 更新予定額計算, サーバーDBはOracle
- APログイン必須, タイプ50に切替, 期間別フロー表示
- 使用禁止フラックス, システム構成の保持あり, 計上メニュー表示, 仕訳出力

---

## 3. 顧客カスタム仕訳フォーム

### 仕訳の3種類
1. **計上仕訳（KEIJO）**: 月次費用・資産のリース→会計仕訳
2. **支払仕訳（TOUGETSU）**: 毎月リース料支払の仕訳
3. **経費仕訳**: 経費計上

### 共通構造（Form_Openパターン）
1. 仕訳条件テーブル検証（tw_S_TOUGETSU_JOKEN / tw_S_KEIJO_JOKEN）
2. 期間チェック（KIKAN_FROM/KIKAN_TO）
3. 計算パラメータ検証（KTMG, TAISHO, MEISAI）
4. ワークテーブル初期化

### 顧客別カスタマイズポイント

| 顧客 | 出力形式 | 特徴 |
|------|---------|------|
| MYCOM | Excel | 資産/負債二重構造、子会社/親会社別、支払伝票印刷統合 |
| TSYSCOM | CSV | 月初/月末両対応、仕訳Noシーケンス4000 |
| SNKO | Excel | 最終確認画面統合、異動データ検証 |
| JOT | Excel/標準 | pc_JOT_仕訳出力_COMで共通化 |
| RISO | Excel | TAISHO=3, MEISAI=2の厳格チェック |
| NKSOL | Excel | SW_RSOK検証（残高差額計算済み確認） |
| VTC | Excel | 一部検証コメント化（柔軟対応） |
| VALQUA | Excel | 長短振替仕訳あり |
| 標準仕訳 | テキスト/Excel | CMSW2WRK/APGDHWRK/APGDDWRK/APGDSWRK形式 |

### 仕訳出力先テーブル
- tw_fc_支払仕訳 / tw_fc_計上仕訳: 作業テーブル
- tw_KITOKU_CMSW2WRK: 伝票ワーク（固定長テキスト出力）
- tw_KITOKU_APGDHWRK: 金額概要ワーク
- tw_KITOKU_APGDDWRK: 金額詳細ワーク
- tw_KITOKU_APGDSWRK: 支払ワーク

---

## 4. AI_Upload（セットアップスイート）

### ファイル構成
- setup.inf: インストール設定（Access 2016 64bit Runtime, C:\LeaseM4BS\）
- SecSet2016.vbs: Access信頼できる場所のレジストリ登録（UAC対応）
- LM4_delete.wsf: アンインストール（ファイル削除+レジストリ削除+バックアップ）
- pc_IniFile.vbs: INIファイル読み書きクラス
- p_Com.vbs: 共通ユーティリティ（40個以上の関数）
- _MessageForm_.exe: .NET 2.0/4.0メッセージフォーム

### 配布ファイル
LM4BS.accdr, LM4BS.mdb, LM4BSCustomize.mdb, LM4BSImpWkDB.mdb, LM4BSmdld.mdb, LM4BSwork.mdb, SecSet2016.vbs, _MessageForm_.exe, _MessageForm_.exe.config

---

## 5. VB.NET版プロジェクト現状

### プロジェクト構成
- LeaseM4BS.slnx (.NET Framework 4.7.2, VB.NET)
  - LeaseM4BS.DataAccess: PostgreSQLデータアクセス層
  - LeaseM4BS.TestWinForms: WinFormsテストプロジェクト（約557フォーム）

### データアクセス層（完成）
- DbConnectionManager: PostgreSQL接続管理（Npgsql 6.0.11）
  - App.config/環境変数から接続文字列取得
  - デフォルト: Host=localhost;Port=5432;Database=lease_m4bs
  - 接続テスト、パスワードマスク表示
- CrudHelper: 汎用CRUD操作（DAO.Recordset代替）
  - GetDataTable, ExecuteNonQuery, ExecuteScalar<T>
  - Insert, Update, Delete, Exists
  - トランザクション管理（Begin/Commit/Rollback）

### UI基盤（完成）
- FormHelper:
  - ComboBox: Bind(SQL), AdjustSize, 複数列表示(Access風罫線付き)
  - SyncTo: ComboBox→TextBox自動同期（=Column(x)再現）
  - DGV: HideColumns, FormatColumn, GetSelectedRow
  - SyncDgvScroll/SyncDgvColumnWidths: 複数DGV同期
- FileHelper:
  - ToExcelFile: Excel出力（Interop使用）
  - ToCsvFile: CSV出力（Shift_JIS）
  - ToFixedLengthFile: 固定長（未完成）
- CalendarColumn: DGV内DateTimePicker

### 移行済み画面（主要なもの）
- マスタ: BCAT, BKNRI, KKNRI, SKMK, HKMK等
- 契約入力: ContractEntry(59KB), BuknEntry(17KB)
- システム: 0F_SYSTEM, 0F_SYSTEM管理, ログ画面群
- 検索: FlexSearchDLG系, FlexOutputDLG系, FlexReportDLG系
- 計算: CHUKI_RECALC, CHUKI_SCH, 年金現価計算

### 未完成項目
- 固定長ファイル出力（ToFixedLengthFile）
- 複雑な業務計算処理の一部
- 顧客固有仕訳出力の移行
