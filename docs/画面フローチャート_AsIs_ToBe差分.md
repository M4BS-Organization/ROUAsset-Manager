# 画面フローチャート AsIs / ToBe 差分比較

> **作成日:** 2026-03-11
> **更新日:** 2026-03-11
> **対象プロジェクト:** LeaseM4BS（リース会計管理システム）
> **参照元:** [画面フローチャート_AsIs.md](画面フローチャート_AsIs.md) / [画面フローチャート_ToBe.md](画面フローチャート_ToBe.md)
> **ログイン画面検討:** [ログイン画面_検討資料.md](ログイン画面_検討資料.md)

---

## 1. 差分サマリー

| 項目 | AsIs | ToBe | 変更種別 |
|---|---|---|---|
| ログイン画面 | なし（直接メニュー表示） | FrmLogin 新規実装 | **新規** |
| 契約書画面 | 実装済み | 既存拡張 | 機能追加 |
| 使用権資産画面 | プレースホルダ | 新規実装 | **新規** |
| 月次支払画面 | プレースホルダ | 新規実装 | **新規** |
| 月次会計画面 | プレースホルダ | 新規実装 | **新規** |
| 期間残高画面 | プレースホルダ | 新規実装 | **新規** |
| 税法調整画面 | プレースホルダ | 新規実装 | **新規** |
| マスタ管理画面 | 実装済み | 変更なし | なし |
| 画面間ナビゲーション | なし | 追加 | **新規** |

---

## 2. ログイン画面（ToBe で新規追加）

### 2-0. 起動フローの変更

| 項目 | AsIs | ToBe |
|---|---|---|
| 起動フォーム | FrmFlexMenu（メインメニュー直接表示） | **FrmLogin（ログイン画面）** |
| 認証 | なし（`AuthenticationMode.Windows` 設定のみ） | ユーザーID/パスワード認証（AuthorizationService + tm_USER） |
| 権限制御 | なし（全メニューアクセス可能） | ログインユーザーの権限に応じたメニュー有効/無効制御 |

**起動フロー比較:**

```
AsIs:  App Start → FrmFlexMenu（全機能アクセス可能）

ToBe:  App Start → FrmLogin → 認証成功 → FrmFlexMenu（権限付き）
                            → 認証失敗 → エラー表示（再試行）
                            → 終了ボタン → アプリ終了
```

**背景:**
- 要件定義書 F-BZ-005「認証・権限管理」が優先度 高で定義されているが未実装
- tm_USER テーブルは DB 設計済み（パスワードカラムの追加が必要）
- 詳細は [ログイン画面_検討資料.md](ログイン画面_検討資料.md) を参照

---

## 3. 契約書画面（FrmFlexContract → FrmLeaseContractMain）の差分

### 3-1. ヘッダーボタン

| ボタン | AsIs | ToBe |
|---|---|---|
| 登録 | 実装済み（`OnRegisterClick()` → CTBレコード生成 → DB保存 → `ContractRegistered` イベント発火） | 記載なし（継続と推定） |
| 修正・変更 | 未実装 | 記載なし |
| 更新・満了解約 | 未実装 | 記載なし |
| 中途解約 | 未実装 | 記載なし |
| 削除 | 未実装 | 記載なし |

> **注意:** ToBe ではヘッダーボタンの記載が省略されている。これらの未実装ボタンが ToBe スコープに含まれるかは要確認。

### 3-2. 会計タブ（pgAccounting）

| 項目 | AsIs | ToBe |
|---|---|---|
| タブ名称 | pgAccounting（会計計算） | pgAccounting（会計計算・**返済スケジュール**） |

> **変更点:** ToBe では「返済スケジュール」機能が追加される。

### 3-3. 資産操作ボタン

| ボタン | AsIs | ToBe |
|---|---|---|
| 資産追加（btnAssetNew） | ShowDialog() → `AddAssetRow()` でグリッドに追加 | ShowDialog() → データ反映 |
| 資産参照（btnAssetSearch） | ShowDialog() | ShowDialog() |
| 資産削除（btnDeleteRow） | グリッド行削除 | **記載なし（削除）** |

> **変更点:** AsIs にあった「資産削除」ボタンが ToBe では記載なし。仕様変更か記載漏れか要確認。

### 3-4. 契約管理フォームの動作モード

| 項目 | AsIs | ToBe |
|---|---|---|
| NewEntry モード | 自動採番、全フィールド入力可能、新規レコード作成 | 記載あり（同等） |
| Edit モード | 契約番号読取専用、他フィールド入力可能、既存レコード更新 | 記載あり（同等） |
| Inquiry モード | 全フィールド読取専用、保存ボタン非表示/無効 | 記載あり（同等） |

> 動作モードに変更なし。

---

## 4. 新規画面の詳細（ToBe で追加）

AsIs ではすべてプレースホルダだった以下5画面に、ToBe で具体的な画面フローが定義された。

### 4-1. 使用権資産画面（FrmFlexROUAsset）

**最も機能が豊富な新規画面。**

| 機能 | 遷移方式 | 遷移先 |
|---|---|---|
| 行ダブルクリック | `ShowDialog()` | 使用権資産詳細（モーダルダイアログ） |
| 元契約へボタン | `Show()` | FrmLeaseContractMain (Edit) |
| 期間残高へボタン | `SwitchContent()` | FrmFlexPeriodBalance |
| 減価償却実行 | 画面内処理 | 月次減価償却 → 一覧更新 |
| 減損テスト | 画面内処理 | 減損テスト → 一覧更新 |
| Excel出力 | 画面内処理 | Excel エクスポート |

**使用権資産詳細ダイアログからの遷移:**
- 元契約へ → `Show()` → FrmLeaseContractMain (Edit)
- 閉じる → 一覧へ戻る

### 4-2. 月次支払画面（FrmFlexMonthlyPayments）

| 機能 | 遷移方式 | 遷移先 |
|---|---|---|
| 支払登録 | `ShowDialog()` | 支払実績入力（モーダルダイアログ） |
| 元契約へボタン | `Show()` | FrmLeaseContractMain (Edit) |

### 4-3. 月次会計画面（FrmFlexMonthlyAccounting）

| 機能 | 遷移方式 | 遷移先 |
|---|---|---|
| 仕訳詳細 | `ShowDialog()` | 仕訳詳細（モーダルダイアログ） |
| 元契約へボタン | `Show()` | FrmLeaseContractMain (Edit) |

### 4-4. 期間残高画面（FrmFlexPeriodBalance）

| 機能 | 遷移方式 | 遷移先 |
|---|---|---|
| 明細照会 | `ShowDialog()` | 残高明細（モーダルダイアログ） |
| 税法調整へボタン | `SwitchContent()` | FrmFlexTaxAdjustment |

### 4-5. 税法調整画面（FrmFlexTaxAdjustment）

| 機能 | 遷移方式 | 遷移先 |
|---|---|---|
| 調整詳細 | `ShowDialog()` | 税法調整詳細（モーダルダイアログ） |

---

## 5. 画面間ナビゲーションの追加（ToBe 新規）

AsIs では各画面が独立していたが、ToBe では画面間の相互遷移が追加される。

### 5-1. 元契約へ（Show() による非モーダル遷移）

以下の画面から `FrmLeaseContractMain` (Edit) へ遷移可能になる:

| 遷移元 | 遷移方式 |
|---|---|
| FrmFlexROUAsset | `Show()` ボタン |
| FrmFlexROUAsset 詳細ダイアログ | `Show()` ボタン |
| FrmFlexMonthlyPayments | `Show()` ボタン |
| FrmFlexMonthlyAccounting | `Show()` ボタン |

> AsIs では `FrmFlexContract` からのみ `FrmLeaseContractMain` へ遷移可能だったが、ToBe では4画面から直接遷移可能になる。

### 5-2. SwitchContent() による画面内切替

| 遷移元 | 遷移先 | 方向 |
|---|---|---|
| FrmFlexROUAsset | FrmFlexPeriodBalance | 片方向 |
| FrmFlexPeriodBalance | FrmFlexTaxAdjustment | 片方向 |

> メニューボタン以外からの `SwitchContent()` 呼び出しが追加される。

### 5-3. ナビゲーション全体図

```
AsIs:
  FrmFlexContract ──Show()──► FrmLeaseContractMain
  （他の画面間遷移なし）

ToBe:
  FrmFlexContract ──────────Show()──────────► FrmLeaseContractMain
  FrmFlexROUAsset ──────────Show()──────────►       ▲
  FrmFlexMonthlyPayments ──Show()──────────►       │
  FrmFlexMonthlyAccounting ─Show()─────────►       │
                                                     │
  FrmFlexROUAsset ──SwitchContent()──► FrmFlexPeriodBalance
                                            │
                                            ▼ SwitchContent()
                                       FrmFlexTaxAdjustment
```

---

## 6. マスタ管理画面の差分

| 項目 | AsIs | ToBe |
|---|---|---|
| 表示粒度 | 9テーブル個別表示 | 4カテゴリに集約 |
| 機能 | CRUD操作 | CRUD操作（変更なし） |

**AsIs（9テーブル）:**
m_company, m_supplier, m_payment_method, m_department, m_asset_category, m_bank_account, m_contract_type, m_initial_cost_item, m_acct_treatment

**ToBe（4カテゴリ）:**
勘定科目マスタ, 部門マスタ, 取引先マスタ, その他マスタ

> **注意:** ToBe の mermaid 図では4カテゴリに簡略化されているが、実テーブル構成が変わるのか、表示のグルーピングのみかは要確認。

---

## 7. 新規モーダルダイアログ一覧（ToBe で追加）

AsIs では `FrmAssetDetailEntry` のみだったモーダルダイアログが、ToBe で5つ追加される。

| ダイアログ名 | 呼び出し元 | 操作 |
|---|---|---|
| 使用権資産詳細 | FrmFlexROUAsset | 行ダブルクリック |
| 支払実績入力 | FrmFlexMonthlyPayments | 支払登録ボタン |
| 仕訳詳細 | FrmFlexMonthlyAccounting | 仕訳詳細ボタン |
| 残高明細 | FrmFlexPeriodBalance | 明細照会ボタン |
| 税法調整詳細 | FrmFlexTaxAdjustment | 調整詳細ボタン |

> すべて `ShowDialog()` によるモーダル表示。`PopupBaseForm` を継承するかどうかは ToBe では未定義。

---

## 8. フォーム階層構造の差分

```
AsIs:                                    ToBe:
System.Windows.Forms.Form                System.Windows.Forms.Form
├── FrmFlexMenu                          ├── FrmLogin ★新規（起動フォーム）
├── FrmLeaseContractMain                 ├── FrmFlexMenu
└── PopupBaseForm                        ├── FrmLeaseContractMain
    └── FrmAssetDetailEntry              └── PopupBaseForm
                                             └── FrmAssetDetailEntry
                                             └── ★5つの新規ダイアログ（継承元未定義）

UserControl                              UserControl
├── FrmFlexContract    【実装済み】       ├── FrmFlexContract    【既存拡張】
├── FrmFlexROUAsset    【プレースホルダ】  ├── FrmFlexROUAsset    【★新規実装】
├── FrmFlexMonthlyPayments 【同上】      ├── FrmFlexMonthlyPayments 【★新規実装】
├── FrmFlexMonthlyAccounting 【同上】    ├── FrmFlexMonthlyAccounting【★新規実装】
├── FrmFlexPeriodBalance   【同上】      ├── FrmFlexPeriodBalance  【★新規実装】
├── FrmFlexTaxAdjustment   【同上】      ├── FrmFlexTaxAdjustment  【★新規実装】
└── FrmFlexMaster      【実装済み】       └── FrmFlexMaster      【変更なし】
```

---

## 9. 要確認事項

| # | 項目 | 詳細 |
|---|---|---|
| 1 | **認証方式の決定** | アプリ独自認証（ID/パスワード）/ Windows認証 / ハイブリッドのいずれを採用するか（[検討資料](ログイン画面_検討資料.md) 参照） |
| 2 | **権限レベルの設計** | 管理者/経理担当/総務担当/照会専用の4段階で十分か、メニュー単位の細粒度制御が必要か |
| 3 | **tm_USER テーブル拡張** | password_hash, role, is_active 等のカラム追加の承認 |
| 4 | ヘッダーボタンのスコープ | ToBe で「修正・変更」「更新・満了解約」「中途解約」「削除」ボタンの実装が含まれるか |
| 5 | 資産削除ボタン | AsIs にある `btnDeleteRow` が ToBe で削除されたのは仕様変更か記載漏れか |
| 6 | マスタテーブル構成 | ToBe で4カテゴリに集約されたのは表示上のグルーピングか、テーブル統廃合か |
| 7 | 新規ダイアログの継承元 | 5つの新規モーダルダイアログが `PopupBaseForm` を継承するか |
| 8 | ContractRegistered イベント | ToBe の各画面から契約管理へ遷移した際のイベント連携はどうなるか |
