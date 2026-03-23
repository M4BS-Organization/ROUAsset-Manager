# 新旧UIの導線確保 — 作業調査・Issue分解資料

> **作成日:** 2026-03-23
> **対象期間:** 2026/3/23（月）〜 3/27（金）
> **対象プロジェクト:** LeaseM4BS（リース会計管理システム）

---

## 1. 目的

既存の「契約フレックス画面（リスト）」から、新規作成した「4タブ画面（契約管理フォーム）」への導線を実装し、実データを使ったリース判定ロジック構築の準備を整える。

---

## 2. 現状分析（AsIs）

### 2.1 画面構成

| 画面 | ファイル | 状態 |
|---|---|---|
| 契約フレックス画面（リスト） | `FrmFlexContract.vb` (270行) | **実装済み** |
| 4タブ画面（契約管理） | `FrmLeaseContractMain.vb` (2478行) | **実装済み** |
| メインメニュー | `FrmFlexMenu.vb` | **実装済み** |
| ログイン | `FrmLogin.vb` | **実装済み** |

### 2.2 画面遷移の現状

```
FrmFlexContract（リスト画面）
  ├─ [新規登録] → FrmLeaseContractMain (NewEntry) ← 契約番号のみ自動採番セット
  ├─ [変更]     → FrmLeaseContractMain (Edit)     ← Tag に契約番号セットのみ
  ├─ [照会]     → FrmLeaseContractMain (Inquiry)  ← Tag に契約番号セットのみ
  └─ [ダブルクリック] → FrmLeaseContractMain (Edit) ← 同上
```

### 2.3 既に実装されている部分

- **画面遷移自体は動作する**: `OpenContractMain()` メソッドで `FrmLeaseContractMain` を `Show()` で開く処理は完成
- **新規登録時の契約番号自動採番**: `GetNextContractNo()` → `InitContractNo` プロパティ経由で反映
- **登録完了イベント**: `ContractRegistered` イベントで一覧リロード
- **3つの動作モード**: NewEntry / Edit / Inquiry の `ContractOpenMode` 列挙型

### 2.4 未実装（GAP）の部分

| # | GAP | 影響 | 優先度 |
|---|---|---|---|
| G1 | **Edit/Inquiryモードでの既存データ読込がない** | 変更・照会ボタンを押してもフォームは空白のまま表示される | **最重要** |
| G2 | **リスト選択データ→契約入力タブへの初期表示がない** | リストで選択した契約の詳細が4タブ画面に反映されない | **最重要** |
| G3 | **契約入力タブ→リース判定タブへのデータ連携がない** | 判定タブの入力項目（期間・金額等）が契約データと独立している | 重要 |
| G4 | **ブランチ分離されていない** | 既存コードへの影響リスクがある | 前提作業 |

---

## 3. データ構造の整理

### 3.1 リスト画面のグリッドカラム（FrmFlexContract）

```
colCtbId, colContractNo, colPropertyNo, colContractName,
colAssetNo, colAssetName, colAssetCategory,
colStartDate, colEndDate, colContractPeriod,
colDeptName, colAllocationRatio, colTotalPayment, colSplitStatus
```

### 3.2 4タブ画面で必要なデータ（FrmLeaseContractMain）

**タブ1: 契約基本情報**
- 契約種類、契約番号、契約名称、管理部署、取引先
- リース開始日・終了日、無償期間、リース期間
- 資産グリッド（AssetNo, AssetName, AssetCategory, InstallLocation 等）

**タブ2: 初期費用**
- 初期費用グリッド、直接費用、原状復帰費用、リース優遇措置

**タブ3: 会計計算・返済スケジュール**
- 契約期間情報（読み取り専用）、支払情報、会計期間・金額
- 月次支払スケジュールグリッド、返済スケジュールマトリックス

**タブ4: リース判定**
- Q1〜Q4判定（ラジオボタン）、定量判定（資産時価・月額賃料・割引率）
- 適用除外判定（各種チェックボックス）、判定結果

### 3.3 データソース

| レイヤー | クラス | 用途 |
|---|---|---|
| DB永続化 | `CtbRepository` | `ctb_lease_integrated` + `ctb_dept_allocation` テーブル操作 |
| メモリストア | `CtbDataStore` | DB未接続時のフォールバック |
| マスタ読込 | `MasterDataLoader` | 部署・取引先・契約種類等のComboBox用データ |
| データモデル | `CtbRecord` | 1契約×1資産×1配賦部門のレコード構造 |

### 3.4 CtbRecord → 4タブ画面フィールドのマッピング

| CtbRecordプロパティ | タブ1コントロール | 備考 |
|---|---|---|
| ContractNo | txtContractNo | 編集/照会時は読取専用 |
| ContractName | txtContractName | |
| ContractTypeCd | cmbContractType | マスタ逆引き必要 |
| MgmtDeptCd | cmbMgmtDeptCode | マスタ逆引き必要 |
| SupplierCd | cmbSupplier | マスタ逆引き必要 |
| LeaseStartDate | dtpStartDate | |
| LeaseEndDate | dtpEndDate | |
| FreeRentMonths | numFreePeriod | |
| LeaseTermMonths | lblLeaseMonths | 自動計算表示 |
| AssetNo, AssetName, AssetCategory... | dgvAssets（行） | 複数資産対応 |
| DeptAllocations | （配賦部門グリッド） | 複数部門対応 |

| CtbRecordプロパティ | タブ4コントロール | 備考 |
|---|---|---|
| LeaseStartDate | dtpJudgeStart | タブ1と連動 |
| LeaseEndDate | dtpJudgeEnd | タブ1と連動 |
| LeaseTermMonths | lblTermMonths | 自動計算 |
| MonthlyPayment | numMonthlyRentJudge相当 | 金額連動 |

---

## 4. 作業項目の整理

### 作業0: ブランチ作成・環境準備

| 項目 | 詳細 |
|---|---|
| 作業内容 | 開発用ブランチを作成し、既存mainブランチを保護する |
| 具体的手順 | `git checkout -b feature/ui-navigation-integration` で新ブランチ作成 |
| 完了条件 | 新ブランチ上で開発を開始できる状態 |
| 想定工数 | 小 |

### 作業1: Edit/Inquiryモードでの既存データ読込

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmLeaseContractMain.vb` |
| 作業内容 | 契約番号（Tag プロパティ）をキーにDB/メモリストアからCtbRecordを取得し、各タブのコントロールに反映する `LoadContractData(contractNo)` メソッドを実装 |
| 主要タスク | (a) CtbRepository/CtbDataStoreから契約番号でレコード取得するメソッド追加<br>(b) FrmLeaseContractMain に `LoadContractData()` を実装<br>(c) `ApplyInitialValues()` 内で Tag が空でない場合にデータ読込を呼び出し<br>(d) Edit/Inquiryモードの制御（Inquiryは全フィールド ReadOnly） |
| 完了条件 | リスト画面で契約を選択→変更/照会ボタン→4タブ画面に既存データが表示される |
| 依存 | 作業0 |

### 作業1-a: CtbRepository に契約番号検索を追加

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `CtbRepository.vb`, `CtbDataStore.vb` |
| 作業内容 | `SelectByContractNo(contractNo As String) As List(Of CtbRecord)` メソッドを追加。同一契約番号で複数レコード（複数資産・複数配賦部門）が返る |
| SQLイメージ | `SELECT * FROM ctb_lease_integrated WHERE contract_no = @contractNo` + 配賦テーブルJOIN |
| 完了条件 | 単体テストで契約番号指定のレコード取得が確認できる |

### 作業1-b: タブ1（契約基本情報）へのデータバインディング

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmLeaseContractMain.vb` |
| 作業内容 | CtbRecord のプロパティを各コントロールにマッピング。ComboBoxはコード値→表示名の逆引きが必要 |
| 主要処理 | txtContractNo, txtContractName, cmbContractType, cmbMgmtDeptCode, cmbSupplier, dtpStartDate, dtpEndDate, numFreePeriod への値セット。dgvAssets への資産行追加 |
| 注意点 | ComboBoxの `SelectedValue` セットはマスタデータロード完了後に行う必要がある |
| 完了条件 | Edit/Inquiryモードでタブ1の全フィールドに既存データが表示される |

### 作業1-c: タブ2（初期費用）へのデータバインディング

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmLeaseContractMain.vb` |
| 作業内容 | 初期費用データの読込と表示。現状のCtbRecordには初期費用フィールドがないため、DB側の拡張が必要かを確認 |
| 注意点 | 初期費用は別テーブル管理の可能性あり（tw_initial_cost等）。テーブル構造確認が必要 |
| 完了条件 | 初期費用データがある場合はタブ2に表示される（データがない場合は空表示） |

### 作業1-d: Inquiryモードの制御実装

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmLeaseContractMain.vb` |
| 作業内容 | Inquiryモード時に全入力コントロールを `ReadOnly = True` / `Enabled = False` に設定。登録ボタンを非表示/無効化 |
| 実装方針 | `SetReadOnlyMode()` メソッドを作成し、再帰的にフォーム内の全コントロールを無効化 |
| 完了条件 | 照会モードで開いた画面が完全に読み取り専用になる |

### 作業2: リスト選択データ→契約入力タブへの初期表示

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmFlexContract.vb`, `FrmLeaseContractMain.vb` |
| 作業内容 | 作業1と統合して実現。`OpenContractMain()` で渡す契約番号をキーにデータを読み込む |
| 現状コードとの関係 | 現在 `frm.Tag = contractNo` でセットされている契約番号を、`LoadContractData()` のトリガーとして利用 |
| 完了条件 | リスト画面でデータ行を選択→変更ボタン→4タブ画面にそのデータが表示される |
| 依存 | 作業1 |
| 備考 | 作業1の完了により自動的に達成される（追加実装は不要の見込み） |

### 作業3: 契約入力タブ→リース判定タブへのデータ連携

| 項目 | 詳細 |
|---|---|
| 対象ファイル | `FrmLeaseContractMain.vb` |
| 作業内容 | タブ1で入力/表示されたデータをタブ4（リース判定）の対応フィールドに自動反映する |
| 連携項目 | (a) リース開始日 → dtpJudgeStart<br>(b) リース終了日 → dtpJudgeEnd<br>(c) リース期間 → lblTermMonths（自動計算）<br>(d) 月額賃料 → 判定用月額賃料フィールド |
| 実装方針 | タブ切替イベント (`tabMain.SelectedIndexChanged`) でデータを同期、またはタブ1の各コントロールの `ValueChanged` イベントでリアルタイム同期 |
| 完了条件 | タブ1で契約データを入力/変更→タブ4に自動反映される |
| 依存 | 作業1（データ読込が先） |

### 作業3-a: 日付・期間の連動

| 項目 | 詳細 |
|---|---|
| 対象コントロール | dtpStartDate ↔ dtpJudgeStart, dtpEndDate ↔ dtpJudgeEnd |
| 実装方法 | `dtpStartDate.ValueChanged` イベントで `dtpJudgeStart.Value = dtpStartDate.Value` をセット |
| 注意点 | 双方向同期は避け、タブ1→タブ4の片方向にする（タブ4は判定用の参照値） |

### 作業3-b: 金額データの連動

| 項目 | 詳細 |
|---|---|
| 連携元 | タブ3の月額賃料 (`txtSchRent`) またはタブ1の支払情報 |
| 連携先 | タブ4の `numMonthlyRentJudge` 相当フィールド |
| 注意点 | 判定用の金額は手動上書き可能にすべきか要確認 |

### 作業4（任意）: テスト・動作検証

| 項目 | 詳細 |
|---|---|
| 作業内容 | 実装した導線のE2E動作確認 |
| 検証シナリオ | (a) 新規登録→リスト反映→変更で再表示<br>(b) リスト選択→照会で読取専用表示<br>(c) タブ1入力→タブ4連動確認<br>(d) DB接続時/未接続時（フォールバック）の動作 |
| 完了条件 | 全シナリオが問題なく動作する |

---

## 5. Issue分解案

以下の粒度でGitHub Issueに分解することを推奨:

### Issue #1: ブランチ作成・開発環境準備
- **ラベル**: `setup`
- **内容**: `feature/ui-navigation-integration` ブランチ作成
- **作業量**: XS

### Issue #2: CtbRepository に契約番号検索メソッドを追加
- **ラベル**: `backend`, `data-access`
- **内容**: 作業1-a。`SelectByContractNo()` メソッド追加 + `CtbDataStore` にも同等メソッド追加
- **作業量**: S

### Issue #3: Edit/Inquiryモードでの契約データ読込（タブ1: 契約基本情報）
- **ラベル**: `frontend`, `data-binding`
- **内容**: 作業1-b。`LoadContractData()` メソッド実装。ComboBox逆引き含む
- **依存**: Issue #2
- **作業量**: M

### Issue #4: Edit/Inquiryモードでの契約データ読込（タブ2: 初期費用）
- **ラベル**: `frontend`, `data-binding`
- **内容**: 作業1-c。初期費用テーブルの構造確認→データバインディング
- **依存**: Issue #2
- **作業量**: S〜M

### Issue #5: Inquiryモード（読み取り専用）の制御実装
- **ラベル**: `frontend`, `ux`
- **内容**: 作業1-d。`SetReadOnlyMode()` メソッドで全コントロールを無効化
- **依存**: Issue #3
- **作業量**: S

### Issue #6: タブ1→タブ4（リース判定）へのデータ連動
- **ラベル**: `frontend`, `business-logic`
- **内容**: 作業3。日付・期間・金額の片方向同期実装
- **依存**: Issue #3
- **作業量**: M

### Issue #7: E2E動作検証・バグ修正
- **ラベル**: `testing`, `qa`
- **内容**: 作業4。全導線シナリオの手動テスト + 発見バグの修正
- **依存**: Issue #3〜#6
- **作業量**: S

---

## 6. 推奨実装順序

```
Day 1 (月): Issue #1 → Issue #2
Day 2 (火): Issue #3（メイン作業・最重要）
Day 3 (水): Issue #4 + Issue #5
Day 4 (木): Issue #6（リース判定連動）
Day 5 (金): Issue #7（テスト・バグ修正・PR作成）
```

---

## 7. 技術的な注意事項

### 7.1 ComboBoxのデータバインディング

Edit/Inquiryモードでは、MasterDataLoader によるマスタデータのロードが完了してから ComboBox の `SelectedValue` をセットする必要がある。ロード順序：

```
1. MasterDataLoader.LoadContractTypes() → cmbContractType にバインド
2. MasterDataLoader.LoadDepartments()   → cmbMgmtDeptCode にバインド
3. MasterDataLoader.LoadSuppliers()     → cmbSupplier にバインド
4. LoadContractData(contractNo)          → 各コントロールに値セット
```

### 7.2 複数資産レコードの扱い

`CtbRecord` は「1契約×1資産×1配賦部門」単位。同一契約番号で複数レコードが返る場合、以下のグルーピングが必要:

```
契約番号でグループ化
  ├─ 共通項目（契約名、種類、期間等）→ タブ1上部に表示
  ├─ 資産グループ → dgvAssets の各行に表示
  └─ 配賦部門グループ → 配賦部門グリッドに表示
```

### 7.3 DB未接続時のフォールバック

現在の `LoadCtbData()` と同様、DB接続失敗時は `CtbDataStore`（メモリストア）にフォールバックする設計を維持:

```vb
Try
    records = repo.SelectByContractNo(contractNo)
Catch
    records = CtbDataStore.Instance.GetByContractNo(contractNo)
End Try
```

### 7.4 イベント循環の回避

タブ1→タブ4の連動実装時、`ValueChanged` イベントの循環を防止するフラグが必要:

```vb
Private _isSyncingData As Boolean = False

Private Sub dtpStartDate_ValueChanged(...)
    If _isSyncingData Then Return
    _isSyncingData = True
    dtpJudgeStart.Value = dtpStartDate.Value
    _isSyncingData = False
End Sub
```

---

## 8. 参照ドキュメント

| ドキュメント | パス |
|---|---|
| 画面フローチャート（AsIs） | `docs/画面フローチャート_AsIs.md` |
| 画面フローチャート（ToBe） | `docs/画面フローチャート_ToBe.md` |
| 画面フローチャート（差分） | `docs/画面フローチャート_AsIs_ToBe差分.md` |
| DB設計書 v5 | `docs/project-documentation/DB設計書_v5.md` |
| 基本設計書 v2 | `docs/project-documentation/02_基本設計書_v2.md` |
| 要件定義書 v2 | `docs/project-documentation/01_要件定義書_v2.md` |
| UI最適化設計 Part1 | `docs/project-documentation/最適化設計_Part1_UIフロー.md` |
| UI最適化設計 Part2 | `docs/project-documentation/最適化設計_Part2_UI設計.md` |

---

## 9. 主要ソースファイル

| ファイル | パス | 行数 |
|---|---|---|
| 契約リスト画面 | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmFlexContract.vb` | 270 |
| 4タブ契約管理 | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FrmLeaseContractMain.vb` | 2478 |
| CTBリポジトリ | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbRepository.vb` | 150+ |
| CTBメモリストア | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbDataStore.vb` | 140 |
| CTBレコード | `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/CtbDataStore.vb` 内 | — |
| マスタ読込 | `LeaseM4BS/LeaseM4BS.DataAccess/MasterDataLoader.vb` | 135 |
| DB接続管理 | `LeaseM4BS/LeaseM4BS.DataAccess/DbConnectionManager.vb` | 200 |
| 汎用CRUD | `LeaseM4BS/LeaseM4BS.DataAccess/CrudHelper.vb` | 17912 |
