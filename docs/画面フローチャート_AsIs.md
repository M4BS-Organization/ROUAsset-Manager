# 画面フローチャート AsIs（現状の実装）

> **作成日:** 2026-03-11
> **対象プロジェクト:** LeaseM4BS（リース会計管理システム）
> **対象:** LeaseM4BS.TestWinForms
> **ToBe版:** [画面フローチャート_ToBe.md](画面フローチャート_ToBe.md)

---

## 1. 全体画面遷移図

```
┌─────────────────────────────────────────────────────────────────────┐
│                     アプリケーション起動                              │
└───────────────────────────┬─────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    FrmFlexMenu（メインメニュー）                      │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │  メニューボタン群                                               │  │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐        │  │
│  │  │ 契約書    │ │使用権資産│ │ 月次支払  │ │ 月次会計  │        │  │
│  │  │ ★実装済  │ │ 未実装   │ │ 未実装    │ │ 未実装    │        │  │
│  │  └────┬─────┘ └──────────┘ └──────────┘ └──────────┘        │  │
│  │  ┌──────────┐ ┌──────────┐ ┌──────────┐                      │  │
│  │  │ 期間残高  │ │ 税法調整  │ │ マスタ   │                      │  │
│  │  │ 未実装    │ │ 未実装    │ │ ★実装済 │                      │  │
│  │  └──────────┘ └──────────┘ └────┬─────┘                      │  │
│  └─────────────────────────────────┼────────────────────────────┘  │
│                                    ▼                                │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │  pnlContent（コンテンツ表示エリア）                             │  │
│  │  ※ 選択されたメニューに応じてUserControlを動的に切り替え         │  │
│  └───────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 2. メニュー → 各画面の対応表

| メニューボタン | クラス名 | 実装状態 |
|---|---|---|
| 契約書 | `FrmFlexContract` | **実装済み** |
| 使用権資産 | `FrmFlexROUAsset` | プレースホルダ |
| 月次支払 | `FrmFlexMonthlyPayments` | プレースホルダ |
| 月次会計 | `FrmFlexMonthlyAccounting` | プレースホルダ |
| 期間残高 | `FrmFlexPeriodBalance` | プレースホルダ |
| 税法調整 | `FrmFlexTaxAdjustment` | プレースホルダ |
| マスタ | `FrmFlexMaster` | **実装済み** |

> **動作仕様:** メニューボタン押下時、`SwitchContent()` メソッドにより `pnlContent` 内の UserControl を切り替える。同時に表示されるのは1画面のみ（前の画面は Dispose される）。

---

## 3. 契約書画面の詳細フロー（実装済み）

```
FrmFlexContract（契約書一覧 UserControl）
│
├─ [新規登録] ボタン ──────────────────┐
│                                       ▼
├─ [編集] ボタン ──────────────► FrmLeaseContractMain（契約管理フォーム）
│                                 │  ※ Show() で非モーダル表示
├─ [照会] ボタン ──────────────► │  モード: NewEntry / Edit / Inquiry
│                                 │
├─ DataGridView ダブルクリック ──► │
│                                 │
│                                 ├─ タブ構成
│                                 │   ├─ pgContract（契約基本情報）
│                                 │   ├─ pgInitial（初期費用）
│                                 │   ├─ pgAccounting（会計計算）
│                                 │   ├─ pgSublease（転貸情報）
│                                 │   └─ pgJudgment（Q1-Q4 リース判定）
│                                 │
│                                 ├─ ヘッダーボタン
│                                 │   ├─ [登録] ──► OnRegisterClick()
│                                 │   │              CTBレコード生成 → DB保存
│                                 │   │              ContractRegistered イベント発火
│                                 │   ├─ [修正・変更]    ※未実装
│                                 │   ├─ [更新・満了解約] ※未実装
│                                 │   ├─ [中途解約]      ※未実装
│                                 │   └─ [削除]          ※未実装
│                                 │
│                                 ├─ [資産追加] ボタン ──────┐
│                                 │                           ▼
│                                 ├─ [資産参照] ボタン ► FrmAssetDetailEntry
│                                 │                     （資産詳細ダイアログ）
│                                 ├─ [資産削除] ボタン    ※ ShowDialog() でモーダル表示
│                                 │   └─ グリッド行削除    ※ PopupBaseForm を継承
│                                 │                           │
│                                 │                     ┌─────┴─────┐
│                                 │                     │           │
│                                 │                   [OK]      [Cancel]
│                                 │                     │           │
│                                 │                     ▼           ▼
│                                 │               AddAssetRow()  キャンセル
│                                 │               グリッドに追加  (親画面へ)
│                                 │
│                                 ├─ [保存] ボタン → 契約データ保存
│                                 └─ [閉じる] → フォームを閉じる
│
└─ 契約一覧表示（DataGridView）
```

---

## 4. 契約管理フォームの動作モード

```
FrmLeaseContractMain
├── NewEntry モード（新規登録）
│   ├── 契約番号: 自動採番
│   ├── 全フィールド: 入力可能
│   └── 保存時: 新規レコード作成
│
├── Edit モード（編集）
│   ├── 契約番号: 読み取り専用
│   ├── その他フィールド: 入力可能
│   └── 保存時: 既存レコード更新
│
└── Inquiry モード（照会）
    ├── 全フィールド: 読み取り専用
    └── 保存ボタン: 非表示 / 無効
```

---

## 5. マスタ管理画面の構成（実装済み）

```
FrmFlexMaster（マスタデータ管理 UserControl）
│
├── マスタ種別選択（9テーブル）
│   ├── m_company（会社）
│   ├── m_supplier（取引先）
│   ├── m_payment_method（支払方法）
│   ├── m_department（部門）
│   ├── m_asset_category（資産種別）
│   ├── m_bank_account（銀行口座）
│   ├── m_contract_type（契約種別）
│   ├── m_initial_cost_item（初回費目）
│   └── m_acct_treatment（会計処理）
│
└── CRUD 操作
    ├── 一覧表示（DataGridView）
    ├── 新規登録
    ├── 編集
    └── 削除
```

---

## 6. フォーム階層構造

```
System.Windows.Forms.Form
├── FrmFlexMenu                    メインウィンドウ（起動フォーム）
├── FrmLeaseContractMain           契約管理フォーム（非モーダル）
└── PopupBaseForm                  ポップアップ基底クラス
    └── FrmAssetDetailEntry        資産詳細ダイアログ（モーダル）

System.Windows.Forms.UserControl
├── FrmFlexContract                契約書画面        【実装済み】
├── FrmFlexROUAsset                使用権資産画面    【プレースホルダ】
├── FrmFlexMonthlyPayments         月次支払画面      【プレースホルダ】
├── FrmFlexMonthlyAccounting       月次会計画面      【プレースホルダ】
├── FrmFlexPeriodBalance           期間残高画面      【プレースホルダ】
├── FrmFlexTaxAdjustment           税法調整画面      【プレースホルダ】
└── FrmFlexMaster                  マスタ管理画面    【実装済み】
```

---

## 7. 画面遷移のパターン

| パターン | 表示方式 | 用途 | 例 |
|---|---|---|---|
| メニュー切替 | `SwitchContent()` | メイン画面内のコンテンツ切替 | メニューボタン → UserControl |
| 非モーダル表示 | `Form.Show()` | 独立したウィンドウを開く | 契約一覧 → 契約管理 |
| モーダルダイアログ | `Form.ShowDialog()` | 親画面をブロックして入力を受ける | 契約管理 → 資産詳細 |

---

## 8. 未実装画面（プレースホルダ）一覧

以下の5画面は UserControl として存在するが、内部ロジックは未実装：

1. **FrmFlexROUAsset** — 使用権資産管理
2. **FrmFlexMonthlyPayments** — 月次リース料支払管理
3. **FrmFlexMonthlyAccounting** — 月次会計仕訳処理
4. **FrmFlexPeriodBalance** — 期間残高照会・管理
5. **FrmFlexTaxAdjustment** — 税法調整（税務・会計差異処理）

---

## フロー図（mermaid）

```mermaid
graph TD
    START(["App Start"]) --> MENU

    subgraph FrmFlexMenu["FrmFlexMenu メインメニュー"]
        MENU{"Menu Button\nSwitchContent()"}
        MENU -->|"契約書"| CONTRACT["FrmFlexContract\n契約書一覧\nIMPLEMENTED"]
        MENU -->|"使用権資産"| ROU["FrmFlexROUAsset\nPLACEHOLDER"]
        MENU -->|"月次支払"| PAY["FrmFlexMonthlyPayments\nPLACEHOLDER"]
        MENU -->|"月次会計"| ACC["FrmFlexMonthlyAccounting\nPLACEHOLDER"]
        MENU -->|"期間残高"| BAL["FrmFlexPeriodBalance\nPLACEHOLDER"]
        MENU -->|"税法調整"| TAX["FrmFlexTaxAdjustment\nPLACEHOLDER"]
        MENU -->|"マスタ"| MASTER["FrmFlexMaster\nマスタデータ\nIMPLEMENTED"]
    end

    CONTRACT -->|"新規登録 btnNewEntry\nShow()"| MAIN_NEW["FrmLeaseContractMain\nNewEntry Mode"]
    CONTRACT -->|"変更 btnEdit\nShow()"| MAIN_EDIT["FrmLeaseContractMain\nEdit Mode"]
    CONTRACT -->|"照会 btnInquiry\nShow()"| MAIN_INQ["FrmLeaseContractMain\nInquiry Mode\nReadOnly"]
    CONTRACT -->|"DGV ダブルクリック\nShow()"| MAIN_EDIT

    MAIN_NEW --> TABS{"タブ切替"}
    MAIN_EDIT --> TABS
    MAIN_INQ --> TABS

    TABS -->|"契約"| TAB_CT["pgContract\n契約基本情報"]
    TABS -->|"初回金"| TAB_IN["pgInitial\n初期費用"]
    TABS -->|"会計"| TAB_AC["pgAccounting\n会計計算"]
    TABS -->|"転貸"| TAB_SB["pgSublease\n転貸情報"]
    TABS -->|"リース判定"| TAB_JD["pgJudgment\nQ1-Q4判定"]

    TABS --> HDR{"ヘッダーボタン"}
    HDR -->|"登録"| REG["OnRegisterClick()\nCTBレコード生成\nDB保存\nIMPLEMENTED"]
    HDR -->|"修正・変更"| TODO1["未実装"]
    HDR -->|"更新・満了解約"| TODO2["未実装"]
    HDR -->|"中途解約"| TODO3["未実装"]
    HDR -->|"削除"| TODO4["未実装"]
    REG -->|"ContractRegistered\nイベント発火"| CONTRACT

    TAB_CT --> ASSET_BTN{"資産操作"}
    ASSET_BTN -->|"追加 btnAssetNew\nShowDialog()"| ASSET["FrmAssetDetailEntry\nPopupBaseForm\nモーダル"]
    ASSET_BTN -->|"参照 btnAssetSearch\nShowDialog()"| ASSET
    ASSET_BTN -->|"削除 btnDeleteRow"| DEL_ROW["グリッド行削除"]

    ASSET -->|"OK\nDialogResult.OK"| ASSET_OK["AddAssetRow()\nグリッドに追加"]
    ASSET -->|"Cancel"| TAB_CT

    MASTER --> MASTER_SEL{"マスタ種別\n9テーブル"}
    MASTER_SEL --> M1["m_company\n会社"]
    MASTER_SEL --> M2["m_supplier\n取引先"]
    MASTER_SEL --> M3["m_payment_method\n支払方法"]
    MASTER_SEL --> M4["m_department\n部門"]
    MASTER_SEL --> M5["m_asset_category\n資産種別"]
    MASTER_SEL --> M6["m_bank_account\n銀行口座"]
    MASTER_SEL --> M7["m_contract_type\n契約種別"]
    MASTER_SEL --> M8["m_initial_cost_item\n初回費目"]
    MASTER_SEL --> M9["m_acct_treatment\n会計処理"]

    classDef implemented fill:#d4edda,stroke:#28a745,stroke-width:2px,color:#155724
    classDef placeholder fill:#fff3cd,stroke:#ffc107,stroke-width:2px,color:#856404
    classDef dialog fill:#cce5ff,stroke:#007bff,stroke-width:2px,color:#004085
    classDef notimpl fill:#f8d7da,stroke:#dc3545,stroke-width:2px,color:#721c24

    class CONTRACT,MASTER,REG implemented
    class ROU,PAY,ACC,BAL,TAX placeholder
    class ASSET,MAIN_NEW,MAIN_EDIT,MAIN_INQ dialog
    class TODO1,TODO2,TODO3,TODO4 notimpl
```
