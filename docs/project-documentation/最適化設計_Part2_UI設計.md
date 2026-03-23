# 最適化設計 Part2: UI設計書（全画面レイアウト・コントロール配置）

**ドキュメント番号**: UI-2026-002
**バージョン**: 1.0
**作成日**: 2026-03-13
**入力ドキュメント**: 統合分析レポート / 既存UI棚卸し / 制約管理書

---

## 1. 設計方針

### 1.1 カラーテーマ

| 領域 | 背景色 | タブヘッダ色 | 対象ユーザー |
|------|--------|-------------|-------------|
| 総務タブ | #E8F5E9（淡緑） | #4CAF50（緑） | 総務担当 |
| 経理タブ | #E3F2FD（淡青） | #1976D2（青） | 経理担当 |
| 共同タブ | #FFF8E1（淡黄） | #FFA000（琥珀） | 総務+経理 |
| 横断参照 | #F3E5F5（淡紫） | #7B1FA2（紫） | 全ユーザー |

> **注記（Part1との差異）**: Part1の画面遷移図ではMermaid凡例で「青=総務メイン / オレンジ=経理メイン」と記載しているが、これは画面遷移図のノード色であり、タブ背景色とは異なる。本Part2のカラーテーマ（総務=淡緑, 経理=淡青）がFrmLeaseContractMain内のタブ色分けの正式定義である。Part1の記載はフロー図上の色分けとして読み替えること。

### 1.2 コントロール配置原則

- **グリッド配置**: TableLayoutPanel による行列配置（Label左 + Input右）
- **グループ化**: GroupBox でセクション単位にまとめる
- **金額フィールド**: 右寄せ、N0書式、NUMERIC(15,2)対応
- **ReadOnlyフィールド**: BackColor = SystemColors.Control（グレー）
- **必須フィールド**: Label末尾に「*」、BackColor = #FFFDE7（淡黄）

### 1.3 制約対応方針

| 制約 | UI設計での対応 |
|------|---------------|
| C1: CTB維持 | FrmFlexCtbViewerで30カラム表示。v_ctb_exportをデータソースとする |
| C2: フレックス画面 | FrmFlexCtbViewerを維持・強化。EAV動的パネルを下部に追加 |
| C3: 総務/経理分離 | タブ色分け + ステータスベース権限制御 |
| C4: 会計/税務レンジ | タブ6で並列表示UI。差異自動計算 |

### 1.4 EAV動的フォーム生成

`asset_class_field`テーブルの以下のカラムを参照し、実行時にコントロールを動的生成する:

| カラム | 用途 |
|--------|------|
| ui_control_type | TextBox / ComboBox / DateTimePicker / NumericUpDown / CheckBox |
| ui_group | GroupBox名（セクション分け） |
| ui_row / ui_col | TableLayoutPanel内の配置座標 |
| ui_width | コントロール幅 |
| ui_options | ComboBoxの選択肢（JSON配列） |
| is_readonly | 読取専用フラグ |
| is_required | 必須フラグ |
| display_format | 表示書式（N0, yyyy/MM/dd等） |

---

## 2. FrmFlexDashboard（ダッシュボード）— 新規

### 2.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | 親コンテナに合わせてDock=Fill |
| 対象ユーザー | 全ユーザー |
| データソース | lease_contract, approval_log, amortization_schedule |

### 2.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [リース資産管理システム ダッシュボード]          [ユーザー名] [ロール] │
├───────────────────┬─────────────────────────────────────────────┤
│                   │                                             │
│  ┌─────────────┐  │  承認待ち一覧                                │
│  │ 契約件数     │  │  ┌──────────────────────────────────────┐   │
│  │    125      │  │  │ 契約番号 │ 契約名   │ステータス│提出日 │   │
│  └─────────────┘  │  │ LC-0042  │ 本社ビル │submitted│03/10 │   │
│  ┌─────────────┐  │  │ LC-0043  │ 社用車A  │submitted│03/11 │   │
│  │ 資産総額     │  │  │ LC-0045  │ 複合機   │submitted│03/12 │   │
│  │  ¥1.2B     │  │  └──────────────────────────────────────┘   │
│  └─────────────┘  │                                             │
│  ┌─────────────┐  ├─────────────────────────────────────────────┤
│  │ 月次処理     │  │                                             │
│  │  未処理: 8  │  │  最近の操作ログ                               │
│  └─────────────┘  │  ┌──────────────────────────────────────┐   │
│  ┌─────────────┐  │  │ 日時     │ 操作者 │ 操作   │ 対象    │   │
│  │ 承認待ち     │  │  │ 03/12 14:20│ 田中 │ 承認  │ LC-0040│   │
│  │    3件      │  │  │ 03/12 10:05│ 佐藤 │ 提出  │ LC-0042│   │
│  └─────────────┘  │  │ 03/11 16:30│ 鈴木 │ 登録  │ LC-0043│   │
├───────────────────┤  └──────────────────────────────────────┘   │
│ 期限アラート       │                                             │
│ ! LC-0038 満了30日前│                                             │
│ ! LC-0039 車検期限  │                                             │
│ ! 月次締め 残3日    │                                             │
└───────────────────┴─────────────────────────────────────────────┘
```

### 2.3 コントロール定義

| Name | Type | Label | データソース | 入力/表示 |
|------|------|-------|-------------|----------|
| pnlHeader | Panel | ヘッダー | — | 表示 |
| lblTitle | Label | リース資産管理システム ダッシュボード | — | 表示 |
| lblUserInfo | Label | ユーザー名 / ロール | tw_m_user | 表示 |
| pnlKpi | FlowLayoutPanel | KPIカード領域 | — | 表示 |
| lblKpiContractCount | Label | 契約件数 | lease_contract COUNT | 表示 |
| lblKpiAssetTotal | Label | 資産総額 | lease_accounting SUM(rou_carrying_amount) | 表示 |
| lblKpiMonthlyPending | Label | 月次未処理 | amortization_schedule (未処理件数) | 表示 |
| lblKpiApprovalPending | Label | 承認待ち | lease_contract WHERE status='submitted' COUNT | 表示 |
| dgvApprovalQueue | DataGridView | 承認待ち一覧 | lease_contract WHERE status='submitted' | 表示 |
| dgvRecentLog | DataGridView | 最近の操作ログ | approval_log ORDER BY action_date DESC LIMIT 20 | 表示 |
| pnlAlerts | Panel | 期限アラート | lease_contract (満了日近接), lease_asset (点検期限) | 表示 |

### 2.4 承認待ち一覧グリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先 |
|--------|-----------|-----|-----|-----------|
| colContractNo | 契約番号 | TextBox | 120 | lease_contract.contract_no |
| colContractName | 契約名 | TextBox | 200 | lease_contract.contract_name |
| colStatus | ステータス | TextBox | 100 | lease_contract.status |
| colSubmittedBy | 提出者 | TextBox | 100 | lease_contract.submitted_by |
| colSubmittedAt | 提出日 | TextBox | 100 | lease_contract.submitted_at |

### 2.5 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Load | 画面表示時 | KPI集計クエリ実行、承認待ち一覧ロード、ログロード、アラート生成 |
| dgvApprovalQueue_CellDoubleClick | 行ダブルクリック | FrmLeaseContractMain を該当contract_idで起動 |
| pnlAlerts_LinkClicked | アラートリンククリック | 対象画面へ遷移 |
| tmrRefresh_Tick | 5分間隔 | KPI・承認待ち・ログを自動リフレッシュ |

---

## 3. FrmFlexContract（契約一覧）— 既存改修

### 3.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | Dock=Fill |
| 対象ユーザー | 全ユーザー |
| データソース | lease_contract JOIN lease_asset JOIN lease_accounting |

### 3.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [契約一覧]                                                       │
├─────────────────────────────────────────────────────────────────┤
│ フィルター:                                                       │
│ ステータス [▼ 全て    ] 分類 [▼ 全て     ] 期間 [    ]～[    ]    │
│ 契約番号/名称 [                    ] [検索]  [クリア]             │
├─────────────────────────────────────────────────────────────────┤
│ │契約番号│契約名称  │貸主    │分類      │ステータス│開始日  │終了日  │
│ │LC-0001 │本社ビル  │XX不動産│on_balance│active   │2025/04│2035/03│
│ │LC-0002 │社用車A   │YYリース│on_balance│active   │2025/06│2030/05│
│ │LC-0003 │複合機    │ZZファイ│off_balance│draft   │2025/07│2028/06│
│ │        │          │        │          │         │       │       │
│ │        │          │        │          │         │       │       │
├─────────────────────────────────────────────────────────────────┤
│ 件数: 125件                     [新規登録] [詳細表示] [CTB一覧]   │
└─────────────────────────────────────────────────────────────────┘
```

### 3.3 コントロール定義

| Name | Type | Label | バインド先 | 入力/表示 | バリデーション |
|------|------|-------|-----------|----------|--------------|
| cmbFilterStatus | ComboBox | ステータス | — | 入力(DropDownList) | — |
| cmbFilterClassification | ComboBox | 分類 | — | 入力(DropDownList) | — |
| dtpFilterFrom | DateTimePicker | 期間From | — | 入力 | From <= To |
| dtpFilterTo | DateTimePicker | 期間To | — | 入力 | — |
| txtSearch | TextBox | 契約番号/名称 | — | 入力 | — |
| btnSearch | Button | 検索 | — | — | — |
| btnClear | Button | クリア | — | — | — |
| dgvContracts | DataGridView | 契約一覧 | (下記グリッド定義) | 表示(ReadOnly) | — |
| lblCount | Label | 件数 | — | 表示 | — |
| btnNew | Button | 新規登録 | — | — | 総務ロールのみEnabled |
| btnDetail | Button | 詳細表示 | — | — | 行選択時のみEnabled |
| btnCtbList | Button | CTB一覧 | — | — | — |

### 3.4 グリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先テーブル.カラム |
|--------|-----------|-----|-----|-------------------------|
| colContractNo | 契約番号 | TextBox | 120 | lease_contract.contract_no |
| colContractName | 契約名称 | TextBox | 200 | lease_contract.contract_name |
| colLessor | 貸主 | TextBox | 150 | supplier.supplier_name |
| colClassification | 分類 | TextBox | 100 | lease_contract.lease_classification |
| colStatus | ステータス | TextBox | 100 | lease_contract.status |
| colStartDate | 開始日 | TextBox | 100 | lease_contract.contract_start_date |
| colEndDate | 終了日 | TextBox | 100 | lease_contract.contract_end_date |
| colRouCarrying | ROU帳簿価額 | TextBox | 130 | lease_accounting.rou_carrying_amount |
| colLiabilityBalance | 負債残高 | TextBox | 130 | lease_accounting.lease_liability_balance |

### 3.5 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Load | 画面表示時 | フィルタComboBoxにマスタ値ロード、dgvContracts全件ロード |
| btnSearch_Click | 検索ボタン | フィルター条件でSELECT実行、dgvContractsリバインド |
| btnClear_Click | クリアボタン | フィルターリセット、全件再表示 |
| btnNew_Click | 新規登録ボタン | FrmLeaseContractMain(新規モード)を起動 |
| btnDetail_Click | 詳細表示ボタン | 選択行のcontract_idでFrmLeaseContractMain(編集モード)を起動 |
| dgvContracts_CellDoubleClick | 行ダブルクリック | btnDetail_Clickと同じ |
| btnCtbList_Click | CTB一覧ボタン | FrmFlexCtbViewerタブへ切替 |

---

## 4. FrmLeaseContractMain（契約詳細）— 改修（5タブ→6タブ）

### 4.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | Form（モードレス） |
| サイズ | 1400x950（最小: 1280x800） |
| 対象ユーザー | 総務+経理（ステータスベース権限制御） |

### 4.2 全体レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [リース契約管理]  契約番号: LC-2025-0042   ステータス: [draft ▼]  │
├─────────────────────────────────────────────────────────────────┤
│ [登録] [修正] [更新・満了] [中途解約] [削除]  [CTB確認] [印刷]    │
├─────────────────────────────────────────────────────────────────┤
│┌────────┬────────┬────────┬──────────┬────────┬─────────┐       │
││契約基本 │資産    │ 転貸   │リース判定│会計処理 │税務・差異│       │
││(総務)  │初回金  │(総務)  │(共同)   │(経理)  │(経理)   │       │
││ 緑     │(総務)  │ 緑     │ 黄      │ 青     │ 青      │       │
│├────────┤ 緑     ├────────┴──────────┴────────┴─────────┤       │
││                  │        │                              │       │
││  (タブコンテンツ)  │        │                              │       │
││                  │        │                              │       │
│└─────────────────┴────────┴──────────────────────────────┘       │
├─────────────────────────────────────────────────────────────────┤
│ リース判定: オンバランス処理（資産計上必須）                        │
│                              [下書き保存] [経理へ提出] [承認] [差戻]│
└─────────────────────────────────────────────────────────────────┘
```

### 4.3 ヘッダー・フッター コントロール

| Name | Type | Label | バインド先 | 入力/表示 |
|------|------|-------|-----------|----------|
| lblTitle | Label | リース契約管理 | — | 表示 |
| txtContractNoHeader | TextBox | 契約番号 | lease_contract.contract_no | 表示(ReadOnly) |
| lblStatusBadge | Label | ステータス | lease_contract.status | 表示（色分け） |
| btnRegister | Button | 登録 | — | — |
| btnModify | Button | 修正 | — | — |
| btnRenewExpire | Button | 更新・満了 | — | — |
| btnTerminate | Button | 中途解約 | — | — |
| btnDelete | Button | 削除 | — | — |
| btnCtbCheck | Button | CTB確認 | — | FrmFlexCtbViewerを当該契約フィルタで起動 |
| btnPrint | Button | 印刷 | — | — |
| lblJudgmentPreview | Label | リース判定結果プレビュー | — | 表示 |
| btnSaveDraft | Button | 下書き保存 | — | status=draft時のみ表示 |
| btnSubmit | Button | 経理へ提出 | — | 総務+status=draft時のみ表示 |
| btnApprove | Button | 承認 | — | 経理+status=submitted時のみ表示 |
| btnReject | Button | 差戻し | — | 経理+status=submitted時のみ表示 |

---

### 4.4 Tab1: 契約基本情報（総務）— 背景色 #E8F5E9

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ 基本・管理情報                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 契約番号*  [LC-2025-0042  ]  契約名称*  [              ]  │    │
│ │ 契約種類*  [▼ 不動産賃貸   ]  取引先*    [▼ XX不動産    ]  │    │
│ │ 管理部署*  [▼ 総務部      ]  部署名     [総務部        ]  │    │
│ │ 開始日*    [2025/04/01    ]  終了日*    [2035/03/31    ]  │    │
│ │ 無償期間   [  0 ] 月         リース期間  120 月           │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 資産一覧                                                       │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 資産種類 [▼ 不動産] 資産番号 [      ] [検索] [+新規]       │    │
│ │                                                          │    │
│ │ │□│資産番号 │資産種類│資産名      │設置場所│配賦部門      │    │
│ │ │ │ASSET-001│不動産  │本社ビル5F   │東京   │総務(50%),経理│    │
│ │ │ │ASSET-002│不動産  │本社ビル6F   │東京   │営業(100%)   │    │
│ │                                         [行削除]          │    │
│ │ 資産件数: 2件                                              │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 月額支払明細                                                   │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │科目    │支払額(税抜)│消費税  │税込合計│振込先  │支払方法│支払日│ │
│ │ │賃料    │  500,000  │ 50,000│550,000│みずほ  │振込   │25日 │ │
│ │ │管理費  │   50,000  │  5,000│ 55,000│みずほ  │振込   │25日 │ │
│ │ │共益費  │   30,000  │  3,000│ 33,000│みずほ  │振込   │25日 │ │
│ ├──────────────────────────────────────────────────────────┤    │
│ │ 合計(税抜): 580,000  消費税: 58,000  合計(税込): 638,000    │    │
│ └──────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab1 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| txtContractNo | TextBox | 契約番号* | lease_contract.contract_no | 表示(ReadOnly) | 自動採番 |
| txtContractName | TextBox | 契約名称* | lease_contract.contract_name | 入力 | 必須 |
| cmbContractType | ComboBox | 契約種類* | lease_contract.contract_type_cd | 入力(DropDownList) | 必須 |
| cmbSupplier | ComboBox | 取引先* | lease_contract.supplier_cd | 入力(DropDownList) | 必須 |
| txtSupplierName | TextBox | 取引先名称 | supplier.supplier_name | 表示(ReadOnly) | — |
| cmbMgmtDeptCode | ComboBox | 管理部署* | lease_asset.mgmt_dept_cd (注: v5ではlease_assetに配置。契約レベルの管理部署はUI上の便宜的表示) | 入力(DropDown) | 必須 |
| txtMgmtDeptName | TextBox | 部署名 | department.dept_name | 表示(ReadOnly) | — |
| dtpStartDate | DateTimePicker | 開始日* | lease_contract.contract_start_date | 入力(Short) | 必須, < 終了日 |
| dtpEndDate | DateTimePicker | 終了日* | lease_contract.contract_end_date | 入力(Short) | 必須, > 開始日 |
| numFreePeriod | NumericUpDown | 無償期間 | lease_contract.free_rent_months | 入力(0-60) | — |
| lblLeaseMonths | Label | リース期間 | (算出値) | 表示 | — |
| cmbAssetCategory | ComboBox | 資産種類 | — | 入力(DropDownList) | — |
| txtAssetNo | TextBox | 資産番号 | — | 入力 | — |
| btnAssetSearch | Button | 検索 | — | — | — |
| btnAssetNew | Button | +新規登録 | — | — | — |
| dgvAssets | DataGridView | 資産一覧 | lease_asset | 表示 | — |
| btnDeleteRow | Button | 行削除 | — | — | — |
| lblAssetCount | Label | 資産件数 | — | 表示 | — |
| dgvMonthlyPayments | DataGridView | 月額支払明細 | lease_payment_schedule | 入力 | — |
| lblMonthlyTotalExTax | Label | 合計(税抜) | (算出値) | 表示 | — |
| lblMonthlyTotalTax | Label | 消費税 | (算出値) | 表示 | — |
| lblMonthlyTotalIncTax | Label | 合計(税込) | (算出値) | 表示 | — |

#### Tab1 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| cmbSupplier_SelectedIndexChanged | 取引先変更 | txtSupplierNameに名称を表示 |
| cmbMgmtDeptCode_SelectedIndexChanged | 管理部署変更 | txtMgmtDeptNameに名称を表示 |
| dtpStartDate_ValueChanged | 開始日変更 | リース期間再計算、全再計算(RecalcAll) |
| dtpEndDate_ValueChanged | 終了日変更 | リース期間再計算、全再計算(RecalcAll) |
| numFreePeriod_ValueChanged | 無償期間変更 | リース期間再計算、全再計算(RecalcAll) |
| btnAssetSearch_Click | 検索ボタン | FrmAssetDetailEntry(照会モード)をShowDialog |
| btnAssetNew_Click | 新規登録ボタン | 自動採番→FrmAssetDetailEntry(登録モード)をShowDialog |
| btnDeleteRow_Click | 行削除ボタン | チェック済み行を削除 |
| dgvMonthlyPayments_CellValueChanged | 支払額変更 | 消費税・税込合計を自動計算、合計ラベル更新 |

---

### 4.5 Tab2: 資産・初回金（総務）— 背景色 #E8F5E9

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ 初回費用明細                                                   │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │費目          │金額(税抜) │消費税  │金額(税込) │会計処理   │    │
│ │ │敷金          │2,000,000 │      0│2,000,000│預り金処理 │    │
│ │ │敷金償却額    │  500,000 │ 50,000│  550,000│費用処理   │    │
│ │ │礼金          │  300,000 │ 30,000│  330,000│費用処理   │    │
│ │ │仲介手数料    │  200,000 │ 20,000│  220,000│費用処理   │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ その他初回費用                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 初期直接費用     [    150,000] 円                          │    │
│ │ 原状回復費用見積 [    500,000] 円                          │    │
│ │ リース・インセンティブ [  200,000] 円                       │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 部門配賦サマリ                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 資産: ASSET-001 本社ビル5F                                 │    │
│ │ │部門名   │配賦率(%)│月額配賦額│                             │    │
│ │ │総務部   │  50.00 │ 290,000│                             │    │
│ │ │経理部   │  50.00 │ 290,000│                             │    │
│ │ 配賦率合計: 100.00%                                        │    │
│ └──────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab2 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| dgvInitialCosts | DataGridView | 初回費用明細 | lease_deposit, lease_incentive | 入力 | — |
| numInitialDirectCost | NumericUpDown | 初期直接費用 | lease_initial_measurement.initial_direct_cost | 入力 | Max=9,999,999,999 |
| numRestorationCost | NumericUpDown | 原状回復費用見積 | restoration_obligation.estimated_cost | 入力 | Max=9,999,999,999 |
| numLeaseIncentive | NumericUpDown | リース・インセンティブ | lease_incentive.incentive_amount | 入力 | Max=9,999,999,999 |
| dgvDeptAllocationSummary | DataGridView | 部門配賦サマリ | dept_allocation | 表示(ReadOnly) | — |
| lblAllocationTotal | Label | 配賦率合計 | (算出値) | 表示 | 100%で緑、それ以外で赤 |

#### Tab2 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| dgvInitialCosts_CellValueChanged | 費用金額変更 | 消費税・税込合計を自動計算 |
| numInitialDirectCost_ValueChanged | 初期直接費用変更 | RecalcAll（ROU資産額に影響） |
| numRestorationCost_ValueChanged | 原状回復費用変更 | RecalcAll（ROU資産額に影響） |
| numLeaseIncentive_ValueChanged | インセンティブ変更 | RecalcAll（ROU資産額に影響） |

---

### 4.6 Tab3: 転貸・リース条件（総務）— 背景色 #E8F5E9

> **注記**: Part1の画面一覧ではTab3=「転貸」として定義。本設計では転貸情報に加えオプション・敷金・原状回復も本タブに含め「転貸・リース条件」として統合する。Part1のTab2「資産・初回金」から初回費用系をTab2に残しつつ、オプション・敷金系をTab3に配置する構成とした。

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ オプション情報                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ □ 購入オプションあり                                       │    │
│ │   購入価額 [          ] 円  行使確実性 [▼ 高い(行使する)]   │    │
│ │                                                          │    │
│ │ □ 延長オプションあり                                       │    │
│ │   延長期間 [    ] 月  行使確実性 [▼ 低い(行使しない)]       │    │
│ │                                                          │    │
│ │ □ 解約オプションあり                                       │    │
│ │   解約可能日 [          ]  行使確実性 [▼ 行使しない]        │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ インセンティブ詳細                                             │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │種別          │金額       │受取時期    │会計処理         │    │
│ │ │フリーレント  │  600,000 │契約時     │ROU資産から控除  │    │
│ │ │内装補助      │  200,000 │入居時     │ROU資産から控除  │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 原状回復義務                                                   │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ □ 原状回復義務あり                                         │    │
│ │   見積費用 [    500,000] 円  割引率 [  2.00]%              │    │
│ │   現在価値 [    453,515] 円  (自動計算)                     │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 敷金・建設協力金                                               │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │種別       │金額       │返還予定額│返還不能分│償却方法   │    │
│ │ │敷金       │2,000,000 │1,500,000│  500,000│定額法    │    │
│ └──────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab3 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| chkPurchaseOption | CheckBox | 購入オプションあり | lease_option (type='purchase') | 入力 | — |
| numPurchasePrice | NumericUpDown | 購入価額 | lease_option.option_amount | 入力 | chkPurchaseOption=True時有効 |
| cboPurchaseCertainty | ComboBox | 行使確実性 | lease_option.is_reasonably_certain | 入力(DropDownList) | — |
| chkExtOption | CheckBox | 延長オプションあり | lease_option (type='extend') | 入力 | — |
| numExtMonths | NumericUpDown | 延長期間 | lease_option.option_months | 入力(0-600) | chkExtOption=True時有効 |
| cboExtCertainty | ComboBox | 行使確実性 | lease_option.is_reasonably_certain | 入力(DropDownList) | — |
| chkTerminateOption | CheckBox | 解約オプションあり | lease_option (type='terminate') | 入力 | — |
| dtpTerminateDate | DateTimePicker | 解約可能日 | lease_option.option_exercise_date | 入力 | chkTerminateOption=True時有効 |
| cboTerminateCertainty | ComboBox | 行使確実性 | lease_option.is_reasonably_certain | 入力(DropDownList) | — |
| dgvIncentives | DataGridView | インセンティブ詳細 | lease_incentive | 入力 | — |
| chkRestoration | CheckBox | 原状回復義務あり | restoration_obligation有無 | 入力 | — |
| numRestorationEstimate | NumericUpDown | 見積費用 | restoration_obligation.estimated_cost | 入力 | chkRestoration=True時有効 |
| numRestorationRate | NumericUpDown | 割引率 | restoration_obligation.discount_rate | 入力(0-20, 小数2桁) | — |
| lblRestorationPV | Label | 現在価値 | restoration_obligation.pv_amount | 表示(自動計算) | — |
| dgvDeposits | DataGridView | 敷金・建設協力金 | lease_deposit | 入力 | — |

#### Tab3 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| chkPurchaseOption_CheckedChanged | チェック変更 | 購入オプション入力欄のEnabled切替 |
| chkExtOption_CheckedChanged | チェック変更 | 延長オプション入力欄のEnabled切替、RecalcAll |
| chkTerminateOption_CheckedChanged | チェック変更 | 解約オプション入力欄のEnabled切替 |
| numExtMonths_ValueChanged | 延長期間変更 | 会計リース期間再計算、RecalcAll |
| chkRestoration_CheckedChanged | チェック変更 | 原状回復入力欄のEnabled切替 |
| numRestorationEstimate_ValueChanged | 見積費用変更 | PV自動計算、RecalcAll |
| numRestorationRate_ValueChanged | 割引率変更 | PV自動計算 |

---

### 4.7 Tab4: リース判定（共同/総務+経理）— 背景色 #FFF8E1

> **注記**: 本タブにはPart1「Tab4: リース判定」に対応するリース判定Q1-Q4、免除判定、および初期測定パラメータ（割引率・PV計算）を含む。初期測定パラメータはTab4で入力し、結果の詳細表示（返済スケジュール等）はTab5で行う構成とした。転貸（サブリース）情報はTab3に移動が望ましいが、現状はTab4に暫定配置している。

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ リース判定（総務入力）                                         │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ Q1 資産の特定:    ○あり(特定されている)  ○なし             │    │
│ │ Q2 実質的代替権:  ○あり(サプライヤー)    ○なし             │    │
│ │ Q3 経済的利益:    ○あり                  ○なし             │    │
│ │ Q4 使用指図権:    ○あり                  ○なし             │    │
│ │ ──────────────────────────────────────────                │    │
│ │ リース該当判定:  [リースに該当する]  (緑バッジ)              │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 期間・免除判定（総務入力）                                     │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 見積期間   開始 [2025/04/01] 終了 [2035/03/31]  = 120月   │    │
│ │ 短期リース判定: 非該当（12ヶ月超）                          │    │
│ │ 取得価額   [  12,000,000] 円                               │    │
│ │ 少額リース判定: 非該当（300万円超）                          │    │
│ │ □ 免除規定を適用する （短期or少額該当時のみ有効）            │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 初期測定パラメータ（経理入力）                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 月額リース料*    [    580,000] 円                          │    │
│ │ 割引率(IBR)*     [      2.50] %                           │    │
│ │ □ 所有権移転条項あり                                       │    │
│ │ □ 非リース構成要素を分離しない                              │    │
│ │ ──────────────────────────────────────────                │    │
│ │ PV(リース料総額の現在価値): [  63,847,216] 円               │    │
│ │ ROU資産額: PV + 直接費用 + 原状回復 - インセンティブ         │    │
│ │          = 63,847,216 + 150,000 + 453,515 - 200,000       │    │
│ │          = [  64,250,731] 円                               │    │
│ │ リース負債: [  63,847,216] 円                               │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 判定結果                                                       │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │        【オンバランス処理】  資産計上必須                    │    │
│ │  判定根拠: リースに該当。短期免除・少額免除いずれも非該当。   │    │
│ │           ASBJ第34号に基づき使用権資産・リース負債を計上。   │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ サブリース関係（該当時のみ表示）                                │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ □ 転貸（サブリース）あり                                    │    │
│ │ 転貸先: [              ]  転貸面積: [        ]             │    │
│ │ 転貸期間: [          ] ～ [          ]                      │    │
│ │ │科目  │月額受取額(税抜)│消費税│税込合計│                    │    │
│ │ │転貸料│     200,000  │20,000│220,000│                    │    │
│ └──────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab4 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| rbQ1Yes / rbQ1No | RadioButton | Q1 資産の特定 | lease_judgment.q1_asset_identified | 入力(総務) | — |
| rbQ2Yes / rbQ2No | RadioButton | Q2 実質的代替権 | lease_judgment.q2_substitution_right | 入力(総務) | — |
| rbQ3Yes / rbQ3No | RadioButton | Q3 経済的利益 | lease_judgment.q3_economic_benefit | 入力(総務) | — |
| rbQ4Yes / rbQ4No | RadioButton | Q4 使用指図権 | lease_judgment.q4_direction_right | 入力(総務) | — |
| lblLeaseResult | Label | リース該当判定 | lease_judgment.is_lease | 表示(自動) | — |
| dtpJudgeStart | DateTimePicker | 見積期間開始 | — | 入力(総務) | — |
| dtpJudgeEnd | DateTimePicker | 見積期間終了 | — | 入力(総務) | > 開始日 |
| lblTermMonths | Label | 見積期間(月) | (算出値) | 表示 | — |
| lblShortTermResult | Label | 短期リース判定 | lease_judgment.is_short_term | 表示(自動) | — |
| numAssetValue | NumericUpDown | 取得価額 | — | 入力(総務) | Max=9,999,999,999 |
| lblLowValueResult | Label | 少額リース判定 | lease_judgment.is_low_value | 表示(自動) | — |
| chkApplyExemption | CheckBox | 免除規定適用 | lease_judgment.exemption_applied | 入力(総務) | 短期or少額該当時のみEnabled |
| numMonthlyRentJudge | NumericUpDown | 月額リース料* | — | 入力(経理) | Max=9,900,000,000 |
| numDiscountRate | NumericUpDown | 割引率(%)* | lease_initial_measurement.discount_rate_used | 入力(経理) | 0-20, 小数2桁 |
| chkOwnershipTransfer | CheckBox | 所有権移転条項 | — | 入力(経理) | — |
| chkServiceComponent | CheckBox | 非リース構成要素分離しない | — | 入力(経理) | — |
| txtPresentValue | TextBox | PV | lease_initial_measurement.pv_lease_payments | 表示(ReadOnly) | — |
| txtRouAmount | TextBox | ROU資産額 | lease_initial_measurement.rou_amount | 表示(ReadOnly) | — |
| txtLiabilityAmount | TextBox | リース負債 | lease_initial_measurement.liability_amount | 表示(ReadOnly) | — |
| lblResultText | Label | 判定結果テキスト | lease_judgment.judgment_result | 表示(18pt Bold) | — |
| lblResultBadge | Label | 判定バッジ | — | 表示 | — |
| lblResultReason | Label | 判定根拠 | — | 表示(複数行) | — |
| chkSublease | CheckBox | 転貸あり | sublease_relationship有無 | 入力(総務) | — |
| txtSublesseeName | TextBox | 転貸先名称 | sublease_relationship.sublessee_name | 入力(総務) | chkSublease=True時有効 |
| txtSubleaseArea | TextBox | 転貸面積 | sublease_relationship.subleased_area | 入力(総務) | — |
| dtpSubleaseStart | DateTimePicker | 転貸開始日 | sublease_relationship.sublease_start_date | 入力(総務) | — |
| dtpSubleaseEnd | DateTimePicker | 転貸終了日 | sublease_relationship.sublease_end_date | 入力(総務) | — |
| dgvSubleaseIncome | DataGridView | 転貸料受取 | sublease_relationship | 入力(総務) | — |

#### Tab4 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| rbQ1-Q4_CheckedChanged | 判定回答変更 | リース該当判定を再計算、lblLeaseResult更新 |
| dtpJudgeStart/End_ValueChanged | 見積期間変更 | 短期リース判定を再計算 |
| numAssetValue_ValueChanged | 取得価額変更 | 少額リース判定を再計算 |
| numMonthlyRentJudge_ValueChanged | 月額リース料変更 | PV再計算、ROU・負債再計算、RecalcAll |
| numDiscountRate_ValueChanged | 割引率変更 | PV再計算、ROU・負債再計算、RecalcAll |
| chkSublease_CheckedChanged | 転貸チェック変更 | 転貸入力パネルの表示/非表示切替 |

---

### 4.8 Tab5: 会計処理（経理）— 背景色 #E3F2FD

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ 現契約期間                                                     │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 契約日 [2025/04/01]  開始日 [2025/04/01]                   │    │
│ │ 契約期間 [120月]      終了日 [2035/03/31]                   │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 会計期間・支払情報                                             │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 初回支払日 [2025/05/01]   支払間隔 [1ヶ月]                  │    │
│ │ 支払回数   [120]          無償期間 [0月]                    │    │
│ │ 最終支払日 [2035/02/28]                                    │    │
│ │ 更新予想回数 [  0 ]  会計期間 [120月]  会計終了日 [2035/03] │    │
│ │ 賃料 [580,000]  賃料総額 [69,600,000]  算定総額 [69,600,000]│   │
│ │ リース割合 [100.00]%  配分総額 [69,600,000]  割引率 [2.50]% │    │
│ │ 維持管理費用 [      0]  非リース割合 [  0.00]%              │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 返済スケジュール                                               │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │          │現在価値  │期首      │増加      │変更増減│減少│期末    │
│ │ 使用権資産│63,847,216│        0│64,250,731│      0│  0│64,250,731│
│ │ リース負債│         │        0│63,847,216│      0│  0│63,847,216│
│ │ 除去債務  │         │        0│   453,515│      0│  0│  453,515│
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 償却スケジュール                                               │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │期間    │期首残高   │償却額  │利息費用│元本返済│期末残高   │    │
│ │ │2025/04 │64,250,731│535,423│132,600│447,400│63,803,331│    │
│ │ │2025/05 │63,803,331│535,423│131,676│448,324│63,355,007│    │
│ │ │...     │          │       │       │       │          │    │
│ │ 表示期間: [▼ 全期間] [先頭へ] [前月] [次月] [末尾へ]       │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 仕訳プレビュー                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 対象月: [2025/04 ▼]                                        │    │
│ │ │日付      │借方科目    │借方金額│貸方科目    │貸方金額  │    │
│ │ │2025/04/01│使用権資産  │64,250,731│リース負債│63,847,216│   │
│ │ │          │            │         │除去債務  │  453,515│    │
│ │ │2025/04/30│減価償却費  │  535,423│減価償却累計│ 535,423│    │
│ │ │2025/04/30│支払利息    │  132,600│リース負債  │ 447,400│    │
│ │ │          │リース負債  │  580,000│普通預金    │ 580,000│    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 再測定履歴                                                     │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │変更No│変更計上日│事由      │変更額    │変更後資産│変更後負債│   │
│ │ │(履歴なし)                                              │    │
│ │                                            [再測定実行]   │    │
│ └──────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab5 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| txtSchContractDate | TextBox | 契約日 | lease_contract.contract_start_date | 表示(ReadOnly) | — |
| txtSchStartDate | TextBox | 開始日 | lease_contract.contract_start_date | 表示(ReadOnly) | — |
| txtSchContractPeriod | TextBox | 契約期間 | (算出値) | 表示(ReadOnly) | — |
| txtSchEndDate | TextBox | 終了日 | lease_contract.contract_end_date | 表示(ReadOnly) | — |
| txtSchFirstPayDate | TextBox | 初回支払日 | (算出値) | 表示(ReadOnly) | — |
| txtSchPayInterval | TextBox | 支払間隔 | lease_contract.payment_interval_months | 表示(ReadOnly) | — |
| txtSchPayCount | TextBox | 支払回数 | (算出値) | 表示(ReadOnly) | — |
| txtSchFreePeriod | TextBox | 無償期間 | lease_contract.free_rent_months | 表示(ReadOnly) | — |
| txtSchLastPayDate | TextBox | 最終支払日 | (算出値) | 表示(ReadOnly) | — |
| txtSchRenewalForecastCount | TextBox | 更新予想回数 | — | 入力(経理) | — |
| txtSchAccPeriod | TextBox | 会計期間 | (算出値) | 表示(ReadOnly) | — |
| txtSchAccEndDate | TextBox | 会計終了日 | (算出値) | 表示(ReadOnly) | — |
| txtSchRent | TextBox | 賃料 | — | 表示(ReadOnly) | — |
| txtSchRentTotal | TextBox | 賃料総額 | (算出値) | 表示(ReadOnly) | — |
| txtSchCalcTotal | TextBox | 算定総額 | (算出値) | 表示(ReadOnly) | — |
| txtSchLeaseRatio | TextBox | リース割合 | — | 入力(経理) | Leave→RecalcAll |
| txtSchAllocTotal | TextBox | 配分総額 | (算出値) | 表示(ReadOnly) | — |
| txtSchDiscountRate | TextBox | 割引率 | — | 表示(ReadOnly) | — |
| txtSchMaintenanceCost | TextBox | 維持管理費用 | — | 入力(経理) | Leave→RecalcAll |
| txtSchNonLeaseRatio | TextBox | 非リース割合 | — | 入力(経理) | Leave→RecalcAll |
| txtSchPresentValue | TextBox | 現在価値(PV) | lease_initial_measurement.pv_lease_payments | 表示(ReadOnly) | — |
| txtSchRouBegin | TextBox | ROU期首 | — | 表示(ReadOnly) | — |
| txtSchRouIncrease | TextBox | ROU増加 | — | 表示(ReadOnly) | — |
| txtSchRouChange | TextBox | ROU変更増減 | — | 表示(ReadOnly) | — |
| txtSchRouDecrease | TextBox | ROU減少 | — | 表示(ReadOnly) | — |
| txtSchRouEnd | TextBox | ROU期末 | — | 表示(ReadOnly) | — |
| txtSchLiabBegin | TextBox | 負債期首 | — | 表示(ReadOnly) | — |
| txtSchLiabIncrease | TextBox | 負債増加 | — | 表示(ReadOnly) | — |
| txtSchLiabChange | TextBox | 負債変更増減 | — | 表示(ReadOnly) | — |
| txtSchLiabDecrease | TextBox | 負債減少 | — | 表示(ReadOnly) | — |
| txtSchLiabEnd | TextBox | 負債期末 | — | 表示(ReadOnly) | — |
| txtSchAroBegin | TextBox | ARO期首 | — | 入力(経理) | — |
| txtSchAroIncrease | TextBox | ARO増加 | — | 入力(経理) | — |
| txtSchAroChange | TextBox | ARO変更増減 | — | 入力(経理) | — |
| txtSchAroDecrease | TextBox | ARO減少 | — | 入力(経理) | — |
| txtSchAroEnd | TextBox | ARO期末 | — | 入力(経理) | — |
| dgvAmortization | DataGridView | 償却スケジュール | amortization_schedule | 表示(ReadOnly) | — |
| cmbAmortPeriod | ComboBox | 表示期間 | — | 入力(DropDownList) | — |
| cmbJournalMonth | ComboBox | 仕訳プレビュー対象月 | — | 入力(DropDownList) | — |
| dgvJournalPreview | DataGridView | 仕訳プレビュー | journal_header/detail | 表示(ReadOnly) | — |
| dgvChangeHistory | DataGridView | 再測定履歴 | lease_remeasurement | 表示(ReadOnly) | — |
| btnRemeasure | Button | 再測定実行 | — | — | status=active時のみEnabled |

#### Tab5 償却スケジュールグリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先 |
|--------|-----------|-----|-----|-----------|
| colAmortPeriod | 期間 | TextBox | 80 | amortization_schedule.period_start_date |
| colAmortBeginBalance | 期首残高 | TextBox | 120 | amortization_schedule.beginning_balance |
| colAmortDepreciation | 償却額 | TextBox | 100 | amortization_schedule.depreciation_amount |
| colAmortInterest | 利息費用 | TextBox | 100 | amortization_schedule.interest_expense |
| colAmortPrincipal | 元本返済 | TextBox | 100 | amortization_schedule.principal_payment |
| colAmortEndBalance | 期末残高 | TextBox | 120 | amortization_schedule.ending_balance |

#### Tab5 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Tab5_Enter | タブ切替時 | UpdateAccountingTabValues()で全自動計算値を更新 |
| txtSchLeaseRatio_Leave | リース割合変更 | RecalcAll |
| txtSchMaintenanceCost_Leave | 維持管理費用変更 | RecalcAll |
| txtSchNonLeaseRatio_Leave | 非リース割合変更 | RecalcAll |
| cmbAmortPeriod_SelectedIndexChanged | 表示期間変更 | 償却スケジュールグリッドのフィルタ更新 |
| cmbJournalMonth_SelectedIndexChanged | 仕訳月変更 | 仕訳プレビューグリッドの再ロード |
| btnRemeasure_Click | 再測定実行 | FrmRemeasurementをモーダル起動 |

---

### 4.9 Tab6: 税務・差異（経理）— 新設 — 背景色 #E3F2FD

```
┌─────────────────────────────────────────────────────────────────┐
│ ■ 税務パラメータ                                                 │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ 税務耐用年数    [  360] 月 (= 30年)                        │    │
│ │ 税務償却方法    [▼ 定額法(SL)  ]                            │    │
│ │ 税務ROU資産額   [ 69,600,000] 円（リース料総額ベース）       │    │
│ │ 税務残存簿価    [          1] 円（備忘価額）                 │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 会計 vs 税務 比較                                              │
│ ┌──────────────────┬──────────────────┬──────────────┐          │
│ │  項目             │ 会計(ASBJ#34)    │ 税務(法人税法) │ 差異    │
│ ├──────────────────┼──────────────────┼──────────────┤          │
│ │ 耐用年数(月)      │           120    │          360 │   -240  │
│ │ 償却方法          │        定額法    │       定額法  │    -    │
│ │ ROU資産額         │    64,250,731    │  69,600,000  │★差異   │
│ │ 年間償却額        │     6,425,073    │    2,320,000 │★差異   │
│ │ リース負債        │    63,847,216    │           -  │★差異   │
│ ├──────────────────┴──────────────────┴──────────────┤          │
│ │ ★ 一時差異合計:                       △4,105,073   │          │
│ │   繰延税金資産(税率30.62%):            1,257,373    │          │
│ └──────────────────────────────────────────────────────┘          │
│                                                                 │
│ ■ 期間別税会差異推移                                             │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │年度  │期間│会計償却 │税務償却 │当期差異   │累積差異   │繰延税金│
│ │ │2025  │ 1  │ 535,423│ 193,333│  342,090│   342,090│104,744│
│ │ │2025  │ 2  │ 535,423│ 193,333│  342,090│   684,180│209,487│
│ │ │...   │    │        │        │         │          │       │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│                     [税務パラメータ保存]  [差異レポート出力]       │
└─────────────────────────────────────────────────────────────────┘
```

#### Tab6 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| numTaxUsefulLife | NumericUpDown | 税務耐用年数(月) | lease_asset.tax_useful_life_months | 入力(経理) | 1-600 |
| lblTaxUsefulLifeYears | Label | (=N年) | (算出値) | 表示 | — |
| cmbTaxDepMethod | ComboBox | 税務償却方法 | lease_asset.tax_depreciation_method | 入力(経理, DropDownList) | SL/DB |
| numTaxRouAmount | NumericUpDown | 税務ROU資産額 | tax_accounting_diff (参照) | 入力(経理) | Max=9,999,999,999 |
| numTaxResidualValue | NumericUpDown | 税務残存簿価 | lease_asset.tax_residual_value | 入力(経理) | DEFAULT=1 |
| dgvComparison | DataGridView | 会計vs税務比較 | (算出値) | 表示(ReadOnly) | — |
| lblTempDiffTotal | Label | 一時差異合計 | (算出値) | 表示 | — |
| lblDeferredTax | Label | 繰延税金資産 | (算出値) | 表示 | — |
| dgvTaxDiff | DataGridView | 期間別税会差異 | tax_accounting_diff | 表示(ReadOnly) | — |
| btnSaveTaxParams | Button | 税務パラメータ保存 | — | — | — |
| btnExportDiffReport | Button | 差異レポート出力 | — | — | — |

#### Tab6 比較グリッド列定義

| 列Name | HeaderText | 型 | 幅 |
|--------|-----------|-----|-----|
| colCompItem | 項目 | TextBox | 150 |
| colCompAccounting | 会計(ASBJ#34) | TextBox | 150 |
| colCompTax | 税務(法人税法) | TextBox | 150 |
| colCompDiff | 差異 | TextBox | 120 |

#### Tab6 期間別差異グリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先 |
|--------|-----------|-----|-----|-----------|
| colDiffYear | 年度 | TextBox | 60 | tax_accounting_diff.fiscal_year |
| colDiffPeriod | 期間 | TextBox | 50 | tax_accounting_diff.period |
| colDiffAcctDepr | 会計償却 | TextBox | 110 | tax_accounting_diff.accounting_depreciation |
| colDiffTaxDepr | 税務償却 | TextBox | 110 | tax_accounting_diff.tax_depreciation |
| colDiffTemp | 当期差異 | TextBox | 110 | tax_accounting_diff.depreciation_diff |
| colDiffCumulative | 累積差異 | TextBox | 110 | tax_accounting_diff.cumulative_diff |
| colDiffDeferred | 繰延税金 | TextBox | 110 | tax_accounting_diff.deferred_tax_asset |

#### Tab6 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Tab6_Enter | タブ切替時 | 会計値をTab5から取得、比較テーブル更新 |
| numTaxUsefulLife_ValueChanged | 税務耐用年数変更 | 年表示更新、差異再計算 |
| cmbTaxDepMethod_SelectedIndexChanged | 税務償却方法変更 | 差異再計算 |
| numTaxRouAmount_ValueChanged | 税務ROU資産額変更 | 差異再計算 |
| btnSaveTaxParams_Click | 保存ボタン | lease_asset税務カラム更新、tax_accounting_diff再生成 |
| btnExportDiffReport_Click | レポート出力 | CSV/Excelで差異レポートを出力 |

---

## 5. FrmAssetDetailEntry（資産詳細）— 改修

### 5.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | Form（モーダル, ShowDialog） |
| サイズ | 850x700（現状850x650から拡張） |
| 基底クラス | PopupBaseForm |
| 対象ユーザー | 総務 |

### 5.2 レイアウト

```
┌─────────────────────────────────────────────────────────────┐
│ [資産入力]                                        [×]       │
├─────────────────────────────────────────────────────────────┤
│ ■ 基本情報（共通）                                           │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ 資産番号 [ASSET-0042  ]  資産種類 [不動産          ] │     │
│ │ 資産名*  [本社ビル5F                              ] │     │
│ │ 管理会社 [▼ 本社      ]  設置場所  [東京都千代田区 ] │     │
│ │ 備考     [                                        ] │     │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│ ■ 部門配賦                                                   │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ │部門        │配賦率(%) │                [行追加][行削除]│  │
│ │ │総務部      │   50.00 │                              │  │
│ │ │経理部      │   50.00 │                              │  │
│ │ 配賦率合計: 100.00% (緑)                               │  │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│ ■ 種別固有情報（動的生成: asset_class_field参照）             │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ [不動産の場合]                                       │     │
│ │ 構造 [RC造           ]  面積  [150.00 m2  ]         │     │
│ │ 間取り [3LDK         ]  築年月 [2010/03    ]         │     │
│ │ 貸主名 [XX不動産     ]  仲介業者 [YY仲介   ]         │     │
│ │ 用途制限 [事務所用途のみ                    ]         │     │
│ │                                                     │     │
│ │ [EAV動的フィールド（asset_class_field定義に基づく）]   │     │
│ │ 建物グレード [▼ Aグレード]  耐震等級 [▼ 等級1    ]   │     │
│ │ 省エネ基準   [▼ 適合     ]                           │     │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│                              [追加]  [キャンセル]            │
└─────────────────────────────────────────────────────────────┘
```

### 5.3 共通パネル コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| txtAssetNo | TextBox | 資産番号 | lease_asset.asset_no | 表示(ReadOnly) | 自動採番 |
| lblAssetCategoryDisplay | Label | 資産種類 | lease_asset.asset_category_code | 表示(ReadOnly) | — |
| txtAssetName | TextBox | 資産名* | lease_asset.asset_name | 入力 | 必須 |
| cmbCompany | ComboBox | 管理会社 | lease_asset.company_cd | 入力(DropDownList) | — |
| txtInstallLocation | TextBox | 設置場所 | lease_asset.install_location | 入力 | — |
| txtRemarks | TextBox | 備考 | lease_asset.remarks | 入力(Multiline) | — |
| dgvDeptAllocation | DataGridView | 部門配賦 | dept_allocation | 入力 | 配賦率合計=100% |
| btnAddDept | Button | 行追加 | — | — | — |
| btnRemoveDept | Button | 行削除 | — | — | — |
| lblAllocationTotal | Label | 配賦率合計 | (算出値) | 表示 | 100%で緑、他は赤 |

### 5.4 種別固有パネル（静的定義: 既存互換）

#### 不動産パネル (pnlRealEstate)

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 |
|------|------|-------|-------------------------|----------|
| txtStructure | TextBox | 構造 | asset_attribute (field='structure') | 入力 |
| txtArea | TextBox | 面積 | asset_attribute (field='area') | 入力 |
| txtLayout | TextBox | 間取り | asset_attribute (field='layout') | 入力 |
| dtpCompletion | DateTimePicker | 築年月 | asset_attribute (field='completion_date') | 入力 |
| txtLandlordName | TextBox | 貸主名 | asset_attribute (field='landlord_name') | 入力 |
| txtBrokerCompany | TextBox | 仲介業者 | asset_attribute (field='broker_company') | 入力 |
| txtUsageRestrictions | TextBox | 用途制限 | asset_attribute (field='usage_restrictions') | 入力 |

#### 車両パネル (pnlVehicle)

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 |
|------|------|-------|-------------------------|----------|
| txtChassisNo | TextBox | 車台番号 | asset_attribute (field='chassis_no') | 入力 |
| txtRegistrationNo | TextBox | 登録番号 | asset_attribute (field='registration_no') | 入力 |
| txtVehicleType | TextBox | 車種 | asset_attribute (field='vehicle_type') | 入力 |
| dtpInspectionDate | DateTimePicker | 車検期限 | asset_attribute (field='inspection_date') | 入力 |
| txtMileageLimit | TextBox | 走行距離制限 | asset_attribute (field='mileage_limit') | 入力 |

#### OA機器パネル (pnlOfficeEquip)

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 |
|------|------|-------|-------------------------|----------|
| txtModelNo | TextBox | 型番 | asset_attribute (field='model_no') | 入力 |
| txtSerialNo | TextBox | シリアルNo | asset_attribute (field='serial_no') | 入力 |
| dtpMaintenanceDate | DateTimePicker | 保守期限 | asset_attribute (field='maintenance_date') | 入力 |
| txtMaintenanceContract | TextBox | 保守契約 | asset_attribute (field='maintenance_contract') | 入力 |

### 5.5 EAV動的パネル生成ロジック

```
pnlDynamic (Panel, Dock=Bottom, AutoScroll=True)
│
├── BuildDynamicPanel(assetCategoryCode) 起動時に呼出
│   ├── SELECT * FROM asset_class_field
│   │   WHERE category_code = @categoryCode
│   │   ORDER BY ui_group, ui_row, ui_col
│   │
│   ├── ui_groupごとにGroupBox生成
│   │   └── 内部にTableLayoutPanelを配置
│   │       └── 各fieldのui_row, ui_colに応じてコントロール配置
│   │
│   └── コントロール生成ルール:
│       ├── ui_control_type = 'TextBox'     → TextBox生成
│       ├── ui_control_type = 'ComboBox'    → ComboBox生成 (ui_optionsからItems設定)
│       ├── ui_control_type = 'DatePicker'  → DateTimePicker生成
│       ├── ui_control_type = 'Numeric'     → NumericUpDown生成
│       ├── ui_control_type = 'CheckBox'    → CheckBox生成
│       └── is_required = True              → バリデーション対象に追加
│
├── LoadDynamicValues(assetId) データロード
│   └── SELECT * FROM asset_attribute WHERE asset_id = @assetId
│       → 各コントロールに値をセット
│
└── SaveDynamicValues(assetId) データ保存
    └── 各コントロールの値をasset_attributeにUPSERT
```

### 5.6 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Form_Load | 画面表示時 | SwitchCategoryPanel + BuildDynamicPanel + LoadDynamicValues |
| btnAddDept_Click | 行追加 | dgvDeptAllocationに空行追加 |
| btnRemoveDept_Click | 行削除 | 選択行削除、配賦率合計再計算 |
| dgvDeptAllocation_CellValueChanged | 配賦率変更 | 配賦率合計再計算、色更新 |
| btnAdd_Click | 追加ボタン | バリデーション(資産名必須, 配賦率100%) → SaveDynamicValues → DialogResult.OK |
| btnCancel_Click | キャンセル | DialogResult.Cancel |

---

## 6. FrmFlexCtbViewer（フレックスCTB）— 改修

### 6.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | Dock=Fill |
| データソース | v_ctb_export マテリアライズドビュー（30カラム） |
| 表示モード | ReadOnly |

### 6.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [CTBデータビューア]  データソース: v_ctb_export                    │
├─────────────────────────────────────────────────────────────────┤
│ フィルター:                                                       │
│ 契約番号 [        ] 期間 [    ]～[    ] 資産種別 [▼全て]          │
│ ステータス [▼全て  ]                    [検索] [クリア]           │
├─────────────────────────────────────────────────────────────────┤
│ │CTB_ID│契約番号│物件No│M7資産番号│...│初期ROU│初期負債│税務耐用│会計耐用│税会差異│
│ │    1 │LC-0001│    1│M7-00001 │...│64,250│63,847│   360│   120│-4,105│
│ │    2 │LC-0001│    2│M7-00002 │...│32,100│31,900│   360│   120│-2,050│
│ │    3 │LC-0002│    1│M7-00003 │...│ 8,500│ 8,200│   120│    60│  -900│
│ │      │       │     │         │...│      │      │      │      │      │
│ │ (差異≠0のセルは黄色背景+赤文字)                                 │
├─────────────────────────────────────────────────────────────────┤
│ ■ 種別固有属性（動的表示: asset_class_field参照）                  │
│ ┌─────────────────────────────────────────────────────────┐    │
│ │ 選択: CTB_ID=1 / LC-0001 / ASSET-001 / 不動産            │    │
│ │ 構造: RC造  面積: 150.00m2  間取り: 3LDK  築年月: 2010/03 │    │
│ │ 貸主名: XX不動産  建物グレード: Aグレード  耐震等級: 等級1  │    │
│ └─────────────────────────────────────────────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│ 件数: 125件                     [再読込] [CSV出力] [Excel出力]   │
└─────────────────────────────────────────────────────────────────┘
```

### 6.3 グリッド列定義（全30カラム）

| # | 列Name | HeaderText | 幅 | 揃え | カテゴリ |
|---|--------|-----------|-----|------|---------|
| 1 | colCtbId | CTB_ID | 60 | 右 | 識別キー |
| 2 | colContractNo | 契約番号 | 120 | 左 | 識別キー |
| 3 | colPropertyNo | 物件No | 60 | 右 | 識別キー |
| 4 | colM7AssetNo | M7資産番号 | 110 | 左 | 識別キー |
| 5 | colJsm10AroNo | JSM10除去債務No | 130 | 左 | 識別キー |
| 6 | colLeaseStartDate | リース開始日 | 100 | 中央 | リース条件 |
| 7 | colNonCancellableMonths | 解約不能期間(月) | 110 | 右 | リース条件 |
| 8 | colIsExtensionCertain | 延長確実性 | 80 | 中央 | リース条件 |
| 9 | colExtensionMonths | 延長期間(月) | 90 | 右 | リース条件 |
| 10 | colAccountingLeaseTerm | 会計リース期間(月) | 120 | 右 | リース条件 |
| 11 | colPeriodicPaymentAmt | 定期支払額 | 100 | 右 | 支払条件 |
| 12 | colPaymentIntervalMonths | 支払間隔(月) | 90 | 右 | 支払条件 |
| 13 | colDiscountRate | 割引率 | 70 | 右 | 支払条件 |
| 14 | colResidualValueGuarantee | 残価保証額 | 100 | 右 | オプション |
| 15 | colPurchaseOptionAmt | 購入OP額 | 110 | 右 | オプション |
| 16 | colAroPresentValue | ARO現在価値 | 100 | 右 | オプション |
| 17 | colM7DeptCd | M7部門CD | 90 | 左 | M7連携 |
| 18 | colBurdenDeptCd | 負担部門CD | 90 | 左 | M7連携 |
| 19 | colAssetClassCd | 資産科目CD | 90 | 左 | M7連携 |
| 20 | colSegmentCd | セグメントCD | 100 | 左 | M7連携 |
| 21 | colInitialRouAsset | 初期ROU資産 | 120 | 右 | 会計初期値 |
| 22 | colInitialLeaseLiability | 初期リース負債 | 120 | 右 | 会計初期値 |
| 23 | colTaxUsefulLife | 税務耐用年数(月) | 100 | 右 | 税務レンジ |
| 24 | colAcctUsefulLife | 会計耐用年数(月) | 100 | 右 | 会計レンジ |
| 25 | colTaxDepMethod | 税務償却方法 | 90 | 中央 | 税務レンジ |
| 26 | colAcctDepMethod | 会計償却方法 | 90 | 中央 | 会計レンジ |
| 27 | colTaxDepAnnual | 税務年間償却額 | 110 | 右 | 税務レンジ |
| 28 | colAcctDepAnnual | 会計年間償却額 | 110 | 右 | 会計レンジ |
| 29 | colTaxRouAsset | 税務ROU資産額 | 110 | 右 | 税務レンジ |
| 30 | colTaxAcctDiff | 税会差異(年) | 110 | 右 | 差異 |

**条件付き書式**: 列#30(税会差異)の値が0でない場合、セルのBackColor=#FFFDE7（淡黄）、ForeColor=#D32F2F（赤）

### 6.4 フィルター・操作コントロール

| Name | Type | Label | 入力/表示 |
|------|------|-------|----------|
| txtFilterContractNo | TextBox | 契約番号 | 入力 |
| dtpFilterFrom | DateTimePicker | 期間From | 入力 |
| dtpFilterTo | DateTimePicker | 期間To | 入力 |
| cmbFilterAssetClass | ComboBox | 資産種別 | 入力(DropDownList) |
| cmbFilterStatus | ComboBox | ステータス | 入力(DropDownList) |
| btnSearch | Button | 検索 | — |
| btnClear | Button | クリア | — |
| btnReload | Button | 再読込 | — |
| btnExportCsv | Button | CSV出力 | — |
| btnExportExcel | Button | Excel出力 | — |
| lblTotalCount | Label | 件数 | 表示 |
| pnlDynamicDetail | Panel | 種別固有属性 | 表示(動的生成) |

### 6.5 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Load | 画面表示時 | LoadData()でv_ctb_exportから全件ロード |
| btnSearch_Click | 検索ボタン | フィルター条件でSELECT、グリッドリバインド |
| btnClear_Click | クリアボタン | フィルターリセット、全件再表示 |
| btnReload_Click | 再読込ボタン | REFRESH MATERIALIZED VIEW CONCURRENTLY → LoadData() |
| dgvCtbData_SelectionChanged | 行選択変更 | pnlDynamicDetailを選択行のasset_class_cdで動的再構築 |
| btnExportCsv_Click | CSV出力 | DataGridViewの全データをCSVファイルに出力 |
| btnExportExcel_Click | Excel出力 | DataGridViewの全データをExcelファイルに出力 |
| dgvCtbData_CellFormatting | セル描画時 | 税会差異列の条件付き書式適用 |

---

## 7. FrmRemeasurement（再測定）— 新規

### 7.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | Form（モーダル, ShowDialog） |
| サイズ | 900x700 |
| 対象ユーザー | 経理 |
| 起動元 | FrmLeaseContractMain Tab5 [再測定実行]ボタン |

### 7.2 レイアウト

```
┌─────────────────────────────────────────────────────────────┐
│ [再測定]  契約: LC-2025-0042 本社ビル              [×]       │
├─────────────────────────────────────────────────────────────┤
│ ■ 再測定情報                                                 │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ 再測定日*     [2026/04/01    ]                       │     │
│ │ 再測定理由*   [▼ リース期間の見直し ]                 │     │
│ │ 説明          [                                    ] │     │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│ ■ 変更前 → 変更後                                           │
│ ┌────────────────────┬────────────────────┐                 │
│ │ 【変更前】          │ 【変更後】          │                 │
│ │ リース期間: 120月   │ リース期間: [144]月 │                 │
│ │ 月額リース料: 580,000│ 月額リース料:[580,000]│               │
│ │ 割引率: 2.50%      │ 割引率: [2.50]%     │                 │
│ │ ROU資産: 64,250,731│ ROU資産: (自動計算)  │                 │
│ │ リース負債: 63,847,216│ リース負債: (自動計算)│               │
│ └────────────────────┴────────────────────┘                 │
│                                                             │
│ ■ 再測定計算結果                                             │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ 修正後リース負債:   [  72,500,000] 円                │     │
│ │ 負債増減額:         [   8,652,784] 円                │     │
│ │ 修正後ROU資産:      [  72,903,515] 円                │     │
│ │ 資産増減額:         [   8,652,784] 円                │     │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│ ■ 仕訳プレビュー                                             │
│ ┌─────────────────────────────────────────────────────┐     │
│ │ │借方科目    │借方金額  │貸方科目    │貸方金額    │     │
│ │ │使用権資産  │8,652,784│リース負債  │8,652,784  │     │
│ └─────────────────────────────────────────────────────┘     │
│                                                             │
│                   [計算実行]  [確定]  [キャンセル]            │
└─────────────────────────────────────────────────────────────┘
```

### 7.3 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 | バリデーション |
|------|------|-------|-------------------------|----------|--------------|
| lblContractInfo | Label | 契約情報 | lease_contract | 表示 | — |
| dtpRemeasureDate | DateTimePicker | 再測定日* | lease_remeasurement.remeasurement_date | 入力 | 必須 |
| cmbReason | ComboBox | 再測定理由* | lease_remeasurement.reason_type | 入力(DropDownList) | 必須 |
| txtDescription | TextBox | 説明 | lease_remeasurement.description | 入力(Multiline) | — |
| lblBeforeTerm | Label | 変更前リース期間 | (現在値) | 表示 | — |
| lblBeforeRent | Label | 変更前月額 | (現在値) | 表示 | — |
| lblBeforeRate | Label | 変更前割引率 | (現在値) | 表示 | — |
| lblBeforeRou | Label | 変更前ROU | (現在値) | 表示 | — |
| lblBeforeLiab | Label | 変更前負債 | (現在値) | 表示 | — |
| numAfterTerm | NumericUpDown | 変更後リース期間 | lease_remeasurement.revised_lease_term | 入力 | 1-600 |
| numAfterRent | NumericUpDown | 変更後月額 | lease_remeasurement.revised_payment | 入力 | — |
| numAfterRate | NumericUpDown | 変更後割引率 | lease_remeasurement.revised_discount_rate | 入力 | 0-20 |
| txtRevisedLiability | TextBox | 修正後負債 | lease_remeasurement.revised_liability | 表示(ReadOnly) | — |
| txtLiabilityDiff | TextBox | 負債増減額 | (算出値) | 表示(ReadOnly) | — |
| txtRevisedRou | TextBox | 修正後ROU | lease_remeasurement.revised_rou_asset | 表示(ReadOnly) | — |
| txtRouDiff | TextBox | 資産増減額 | (算出値) | 表示(ReadOnly) | — |
| dgvJournalPreview | DataGridView | 仕訳プレビュー | (算出値) | 表示(ReadOnly) | — |
| btnCalc | Button | 計算実行 | — | — | — |
| btnConfirm | Button | 確定 | — | — | 計算実行後のみEnabled |
| btnCancel | Button | キャンセル | — | — | — |

### 7.4 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Form_Load | 画面表示時 | 変更前の値をロード、変更後に初期値コピー |
| cmbReason_SelectedIndexChanged | 理由変更 | 理由に応じて変更可能項目を制御 |
| btnCalc_Click | 計算実行 | PV再計算、修正後ROU/負債算出、仕訳プレビュー生成 |
| btnConfirm_Click | 確定 | lease_remeasurement INSERT、amortization_schedule再生成、DialogResult.OK |
| btnCancel_Click | キャンセル | DialogResult.Cancel |

---

## 8. FrmJournalViewer（仕訳照会）— 新規

### 8.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | Dock=Fill |
| 対象ユーザー | 経理 |
| データソース | journal_header, journal_detail |

### 8.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [仕訳照会]                                                       │
├─────────────────────────────────────────────────────────────────┤
│ フィルター:                                                       │
│ 期間 [2025/04]～[2025/04] 契約番号 [        ] 仕訳種別 [▼全て]   │
│ ステータス [▼全て]                             [検索] [クリア]    │
├─────────────────────────────────────────────────────────────────┤
│ ■ 仕訳ヘッダ一覧                                                 │
│ │仕訳No│計上日   │契約番号│仕訳種別│ステータス│借方合計 │貸方合計 │
│ │JE-001│2025/04/01│LC-0001│初期認識│confirmed│64,250,731│64,250,731│
│ │JE-002│2025/04/30│LC-0001│月次償却│pending  │ 1,247,423│ 1,247,423│
│ │JE-003│2025/04/30│LC-0001│月次支払│pending  │   580,000│   580,000│
├─────────────────────────────────────────────────────────────────┤
│ ■ 仕訳明細（選択ヘッダの明細）                                    │
│ │行No│借方科目    │借方金額   │貸方科目    │貸方金額   │摘要    │
│ │  1 │使用権資産  │64,250,731│            │          │初期認識│
│ │  2 │            │          │リース負債  │63,847,216│       │
│ │  3 │            │          │除去債務    │   453,515│       │
├─────────────────────────────────────────────────────────────────┤
│ 件数: 45件  借方合計: 130,156,308  貸方合計: 130,156,308         │
│                                            [承認] [CSV出力]      │
└─────────────────────────────────────────────────────────────────┘
```

### 8.3 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 |
|------|------|-------|-------------------------|----------|
| dtpFilterFrom | DateTimePicker | 期間From | — | 入力 |
| dtpFilterTo | DateTimePicker | 期間To | — | 入力 |
| txtFilterContractNo | TextBox | 契約番号 | — | 入力 |
| cmbFilterType | ComboBox | 仕訳種別 | — | 入力(DropDownList) |
| cmbFilterStatus | ComboBox | ステータス | — | 入力(DropDownList) |
| btnSearch | Button | 検索 | — | — |
| btnClear | Button | クリア | — | — |
| dgvJournalHeader | DataGridView | 仕訳ヘッダ一覧 | journal_header | 表示(ReadOnly) |
| dgvJournalDetail | DataGridView | 仕訳明細 | journal_detail | 表示(ReadOnly) |
| lblCount | Label | 件数 | — | 表示 |
| lblDebitTotal | Label | 借方合計 | (算出値) | 表示 |
| lblCreditTotal | Label | 貸方合計 | (算出値) | 表示 |
| btnApprove | Button | 承認 | — | — |
| btnExportCsv | Button | CSV出力 | — | — |

### 8.4 仕訳ヘッダグリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先 |
|--------|-----------|-----|-----|-----------|
| colJournalNo | 仕訳No | TextBox | 80 | journal_header.journal_no |
| colPostingDate | 計上日 | TextBox | 100 | journal_header.posting_date |
| colContractNo | 契約番号 | TextBox | 120 | journal_header.contract_no |
| colJournalType | 仕訳種別 | TextBox | 100 | journal_header.journal_type |
| colJournalStatus | ステータス | TextBox | 100 | journal_header.status |
| colDebitTotal | 借方合計 | TextBox | 120 | (算出値) |
| colCreditTotal | 貸方合計 | TextBox | 120 | (算出値) |

### 8.5 仕訳明細グリッド列定義

| 列Name | HeaderText | 型 | 幅 | バインド先 |
|--------|-----------|-----|-----|-----------|
| colLineNo | 行No | TextBox | 50 | journal_detail.line_no |
| colDebitAccount | 借方科目 | TextBox | 150 | journal_detail.debit_account_cd + gl_account.account_name |
| colDebitAmount | 借方金額 | TextBox | 120 | journal_detail.debit_amount |
| colCreditAccount | 貸方科目 | TextBox | 150 | journal_detail.credit_account_cd + gl_account.account_name |
| colCreditAmount | 貸方金額 | TextBox | 120 | journal_detail.credit_amount |
| colDescription | 摘要 | TextBox | 200 | journal_detail.description |

### 8.6 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| Load | 画面表示時 | 当月の仕訳一覧をロード |
| btnSearch_Click | 検索 | フィルター条件で仕訳ヘッダ一覧をリロード |
| dgvJournalHeader_SelectionChanged | ヘッダ行選択変更 | 選択ヘッダの明細をdgvJournalDetailにロード |
| btnApprove_Click | 承認 | 選択仕訳のステータスをconfirmedに更新 |
| btnExportCsv_Click | CSV出力 | ヘッダ+明細をCSV出力 |

---

## 9. FrmDisclosure（開示注記）— 新規

### 9.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | Dock=Fill |
| 対象ユーザー | 経理 |
| データソース | disclosure_snapshot, lease_accounting, amortization_schedule |

### 9.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [開示注記生成]                                                    │
├─────────────────────────────────────────────────────────────────┤
│ 対象期間: 会計年度 [▼ 2025] 期間 [▼ 通期]  [注記生成]            │
├─────────────────────────────────────────────────────────────────┤
│ ■ 使用権資産の増減表（ASBJ#34 §53）                              │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │種類    │期首残高  │増加    │減少    │償却累計│期末残高  │    │
│ │ │不動産  │64,250,731│      0│      0│5,354,230│58,896,501│   │
│ │ │車両    │ 8,500,000│      0│      0│1,700,000│ 6,800,000│   │
│ │ │合計    │72,750,731│      0│      0│7,054,230│65,696,501│   │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ リース負債の満期分析（ASBJ#34 §55）                            │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │期間         │金額        │                              │    │
│ │ │1年以内       │  6,960,000│                              │    │
│ │ │1年超5年以内  │ 27,840,000│                              │    │
│ │ │5年超         │ 34,800,000│                              │    │
│ │ │合計         │ 69,600,000│                              │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│ ■ 費用の内訳（ASBJ#34 §56）                                     │
│ ┌──────────────────────────────────────────────────────────┐    │
│ │ │項目           │金額       │                             │    │
│ │ │減価償却費     │ 7,054,230│                             │    │
│ │ │利息費用       │ 1,580,400│                             │    │
│ │ │短期リース費用 │   240,000│                             │    │
│ │ │少額リース費用 │   120,000│                             │    │
│ └──────────────────────────────────────────────────────────┘    │
│                                                                 │
│                          [プレビュー] [Excel出力] [PDF出力]       │
└─────────────────────────────────────────────────────────────────┘
```

### 9.3 コントロール定義

| Name | Type | Label | バインド先テーブル.カラム | 入力/表示 |
|------|------|-------|-------------------------|----------|
| cmbFiscalYear | ComboBox | 会計年度 | — | 入力(DropDownList) |
| cmbPeriod | ComboBox | 期間 | — | 入力(DropDownList) |
| btnGenerate | Button | 注記生成 | — | — |
| dgvRouMovement | DataGridView | 使用権資産増減表 | disclosure_snapshot + lease_accounting | 表示(ReadOnly) |
| dgvMaturity | DataGridView | リース負債満期分析 | amortization_schedule | 表示(ReadOnly) |
| dgvExpenseBreakdown | DataGridView | 費用内訳 | journal_detail (集計) | 表示(ReadOnly) |
| btnPreview | Button | プレビュー | — | — |
| btnExportExcel | Button | Excel出力 | — | — |
| btnExportPdf | Button | PDF出力 | — | — |

### 9.4 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| btnGenerate_Click | 注記生成 | 選択年度・期間のデータを集計し各グリッドに表示、disclosure_snapshotに保存 |
| btnPreview_Click | プレビュー | 開示注記の印刷プレビュー表示 |
| btnExportExcel_Click | Excel出力 | 開示注記をExcelファイルに出力 |
| btnExportPdf_Click | PDF出力 | 開示注記をPDFファイルに出力 |

---

## 10. FrmFlexMaster（マスタメンテナンス）— 既存改修

### 10.1 画面概要

| 項目 | 値 |
|------|-----|
| 実装方式 | UserControl（FrmFlexMenu内埋め込み） |
| サイズ | Dock=Fill |
| 対象ユーザー | 管理者 |

### 10.2 レイアウト

```
┌─────────────────────────────────────────────────────────────────┐
│ [マスタメンテナンス]                                              │
├──────────┬──────────────────────────────────────────────────────┤
│ マスタ選択 │                                                     │
│           │  ■ 部門マスタ (department)                           │
│ ○ 会社    │  ┌──────────────────────────────────────────────┐   │
│ ● 部門    │  │ │部門CD │部門名     │上位部門│有効  │         │   │
│ ○ 取引先  │  │ │D001   │総務部     │       │ ✓   │         │   │
│ ○ 契約種別│  │ │D002   │経理部     │       │ ✓   │         │   │
│ ○ 資産区分│  │ │D003   │営業部     │       │ ✓   │         │   │
│ ○ 勘定科目│  │ │D004   │IT部       │       │ ✓   │         │   │
│ ○ 指標    │  │ │       │           │       │      │         │   │
│ ○ 借入金利│  │  └──────────────────────────────────────────────┘   │
│ ○ ユーザー│  │                                                     │
│           │  │                                                     │
│           │  │  [新規追加] [編集] [削除] [CSV取込] [CSV出力]       │
└──────────┴──────────────────────────────────────────────────────┘
```

### 10.3 コントロール定義

| Name | Type | Label | 入力/表示 |
|------|------|-------|----------|
| lstMasterType | ListBox | マスタ選択 | 入力 |
| dgvMasterData | DataGridView | マスタデータ | 入力/表示 |
| btnAdd | Button | 新規追加 | — |
| btnEdit | Button | 編集 | — |
| btnDelete | Button | 削除 | — |
| btnImportCsv | Button | CSV取込 | — |
| btnExportCsv | Button | CSV出力 | — |

### 10.4 マスタ種別とテーブルマッピング

| マスタ名 | テーブル | 主な列 |
|---------|---------|--------|
| 会社 | company | company_cd, company_name, is_active |
| 部門 | department | dept_cd, dept_name, parent_dept_cd, is_active |
| 取引先 | supplier | supplier_cd, supplier_name, is_active |
| 契約種別 | contract_type | type_cd, type_name, is_active |
| 資産区分 | asset_category | category_code, category_name, is_active |
| 勘定科目 | gl_account | account_cd, account_name, account_type, is_active |
| 指標 | index_master | index_cd, index_name, index_type |
| 借入金利 | borrowing_rate_history | rate_id, effective_date, rate |
| ユーザー | tw_m_user | user_id, user_name, role, is_active |

### 10.5 イベント一覧

| イベント | トリガー | 処理内容 |
|---------|---------|---------|
| lstMasterType_SelectedIndexChanged | マスタ種別変更 | 選択マスタのデータをdgvMasterDataにロード |
| btnAdd_Click | 新規追加 | 空行追加 or 入力ダイアログ表示 |
| btnEdit_Click | 編集 | 選択行を編集可能にする |
| btnDelete_Click | 削除 | 選択行を論理削除(is_active=False) |
| btnImportCsv_Click | CSV取込 | OpenFileDialogでCSV選択 → 一括取込 |
| btnExportCsv_Click | CSV出力 | SaveFileDialogでCSV出力 |

---

## 11. ステータスベース権限制御マトリクス

### 11.1 ボタン表示/非表示ルール

| ステータス | 総務が見えるボタン | 経理が見えるボタン |
|-----------|-------------------|-------------------|
| draft | [登録] [下書き保存] [経理へ提出] [削除] | (閲覧のみ、操作ボタンなし) |
| submitted | (操作ボタンなし) | [承認] [差戻し] |
| approved | (操作ボタンなし) | (操作ボタンなし) |
| active | [修正](限定) | [再測定実行] |
| remeasuring | (操作ボタンなし) | [再測定承認] |

### 11.2 タブ編集権限ルール

| ステータス | Tab1-3(総務) | Tab4(共同) | Tab5-6(経理) |
|-----------|-------------|-----------|-------------|
| draft(総務) | 編集可 | Q1-Q4: 編集可 / 割引率等: ReadOnly | 非表示 |
| draft(経理) | ReadOnly | ReadOnly | 非表示 |
| submitted(総務) | ReadOnly | ReadOnly | ReadOnly |
| submitted(経理) | ReadOnly | 割引率・免除: 編集可 / Q1-Q4: ReadOnly | 編集可 |
| approved | ReadOnly | ReadOnly | ReadOnly |
| active(総務) | 限定編集 | ReadOnly | ReadOnly |
| active(経理) | ReadOnly | ReadOnly | ReadOnly(再測定は別画面) |

---

## 12. 画面遷移図サマリ

```
FrmFlexMenu (メインメニュー: 10ボタン)
├── [ダッシュボード] → FrmFlexDashboard
│   └── 承認待ちクリック → FrmLeaseContractMain(該当契約)
├── [契約管理] → FrmFlexContract
│   ├── [新規登録] → FrmLeaseContractMain(新規)
│   ├── [詳細/ダブルクリック] → FrmLeaseContractMain(編集)
│   │   ├── Tab1 [+新規登録/検索] → FrmAssetDetailEntry(モーダル)
│   │   ├── Tab5 [再測定実行] → FrmRemeasurement(モーダル)
│   │   └── ツールバー [CTB確認] → FrmFlexCtbViewer(フィルタ付)
│   └── [CTB一覧] → FrmFlexCtbViewer
├── [使用権資産] → FrmFlexROUAsset
├── [月次支払] → FrmFlexMonthlyPayments
├── [月次会計] → FrmFlexMonthlyAccounting → FrmJournalViewer
├── [期間残高] → FrmFlexPeriodBalance
├── [税務調整] → FrmFlexTaxAdjustment
├── [開示注記] → FrmDisclosure
├── [マスタ管理] → FrmFlexMaster
└── [CTBビューア] → FrmFlexCtbViewer
```

---

*本ドキュメントの最終更新: 2026-03-13*
*ドキュメント化エージェント Part2 出力*
