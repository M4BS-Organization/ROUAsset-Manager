# UI-DB適合性 統合レポート

**ドキュメント番号**: RPT-2026-005
**作成日**: 2026-03-12
**種別**: 統合分析レポート（最終版）
**入力**: research_ui_db_mapping.md, analysis_ui_db_compatibility.md
**対象**: 既存WinForms UIコード（6画面+DataAccess層4ファイル） vs DB設計書v5（37テーブル+7ビュー）

---

## 1. エグゼクティブサマリー

### 結論: 条件付きYes

DB設計書v5は既存UIに適用可能である。ただし、以下の条件を満たす必要がある:

1. **Repository層の全面改修**（CtbRepository の分割・再実装）
2. **CtbRecord の分割**（41プロパティ → 7クラスへの再設計）
3. **v5 DBへの9項目のカラム/テーブル追加**（カラム追加5件＋テーブル新規作成4件）
4. **3段階の段階的移行**（Phase 1-3、合計30人日）

### 適合性スコア

| 画面 | スコア | 判定 |
|------|--------|------|
| FrmLeaseContractMain | **40%** | データフロー変更必要 |
| FrmAssetDetailEntry | **38%** | データフロー変更必要 |
| FrmFlexCtbViewer | **15%** | 全面書き換え |
| **全体（加重平均）** | **31%** | **大規模改修が必要** |

> スコア基準: 90-100%=カラム名変更のみ / 60-89%=テーブル名・FK変更 / 30-59%=データフロー変更 / 0-29%=全面書き換え

### 必要な改修工数

| Phase | 内容 | 工数 |
|-------|------|------|
| Phase 1 | 読み取り対応（v_ctb_exportビュー活用） | **5人日** |
| Phase 2 | 書き込み対応（Repository層書き換え） | **10人日** |
| Phase 3 | UI最適化（EAV動的UI＋新規画面） | **15人日** |
| **合計** | | **30人日** |

### 推奨アプローチ

Phase 1 でまず読み取り系を v_ctb_export ビュー経由に切り替え、FrmFlexCtbViewer を最小変更で動作させる。Phase 2 で書き込み処理を v5 正規化テーブルに対応させ、Phase 3 で EAV 動的UI化と新規画面を追加する。

---

## 2. 画面別適合性マトリクス

### 全体マトリクス

| 画面 | 対応テーブル | 適合度 | 主な改修内容 | 工数 |
|------|------------|--------|-------------|------|
| FrmLeaseContractMain (Tab1: 契約) | lease_contract, lease_asset, lease_payment_schedule | 45% | INSERT分散、未定義カラム3件対応 | 3人日 |
| FrmLeaseContractMain (Tab2: 初回金) | lease_initial_measurement, lease_deposit | 55% | 会計処理区分カラム追加 | 1人日 |
| FrmLeaseContractMain (Tab3: 会計) | lease_accounting, amortization_schedule, lease_remeasurement | 40% | 4テーブル分散参照、未対応カラム3件 | 3人日 |
| FrmLeaseContractMain (Tab4: 転貸) | sublease_relationship | 25% | カラム2件+テーブル1件追加必須 | 2人日 |
| FrmLeaseContractMain (Tab5: 判定) | lease_option, lease_asset | 35% | 判定結果永続化テーブル新規作成 | 2人日 |
| FrmAssetDetailEntry (基本情報) | lease_asset | 60% | remarks カラム追加 | 0.5人日 |
| FrmAssetDetailEntry (部門配賦) | dept_allocation, department | 40% | 単位変換、有効期間入力欄追加 | 1人日 |
| FrmAssetDetailEntry (種別パネル) | asset_attribute (EAV) | 30% | 16項目のEAV化対応 | 3人日 |
| FrmFlexCtbViewer | v_ctb_export | 15% | 全面書き換え（静的→動的UI） | 3人日 |

---

### 2.1 FrmLeaseContractMain（タブ別詳細）

#### Tab1: 契約基本情報 → lease_contract, lease_asset

| UI要素 | v5対応テーブル.カラム | 適合状況 |
|--------|----------------------|---------|
| txtContractNo | lease_contract.contract_no | OK |
| txtContractName | lease_contract.contract_name | OK |
| cmbContractType | lease_contract.contract_type_code | カラム名変更（contract_type_cd → contract_type_code） |
| cmbSupplier | lease_contract.lessor_company_id | 型変更（VARCHAR→INTEGER）、ID変換必要 |
| cmbMgmtDeptCode | lease_asset.mgmt_dept_cd | OK |
| dtpStartDate / dtpEndDate | lease_contract.contract_start_date / contract_end_date | OK |
| numFreePeriod | lease_incentive (incentive_type='free_rent') | テーブル分離 |
| cmbAssetCategory | lease_asset.asset_category_code | ハードコード→マスタ参照へ変更 |
| dgvMonthlyPayments.MBankAccount | **v5未定義** | bank_accountテーブル追加提案 |
| dgvMonthlyPayments.MPayMethod | **v5未定義** | payment_methodカラム追加提案 |
| dgvMonthlyPayments.MItem | **v5未定義（値域不一致）** | payment_typeの値域拡張提案 |

#### Tab2: 初回金・測定 → lease_initial_measurement, lease_deposit

| UI要素 | v5対応テーブル.カラム | 適合状況 |
|--------|----------------------|---------|
| numInitialDirectCost | lease_initial_measurement.initial_direct_cost | OK |
| numRestorationCost | restoration_obligation.estimated_cost | OK |
| numLeaseIncentive | lease_incentive.incentive_amount | OK |
| dgvInitialCosts（敷金） | lease_deposit.deposit_amount | OK |
| dgvInitialCosts.AcctTreatment | **v5未定義** | accounting_treatmentカラム追加提案 |

#### Tab3: 会計処理 → lease_accounting, amortization_schedule

| UI要素 | v5対応テーブル.カラム | 適合状況 |
|--------|----------------------|---------|
| txtSchDiscountRate | lease_initial_measurement.discount_rate_used | OK |
| txtSchPresentValue | lease_initial_measurement.pv_lease_payments | OK |
| txtSchRou系（期首～期末） | lease_accounting + amortization_schedule | 2テーブル結合必要 |
| txtSchLiab系（期首～期末） | lease_accounting + amortization_schedule | 2テーブル結合必要 |
| txtSchAro系（期首～期末） | restoration_obligation | OK |
| txtSchPayInterval | **v5未定義** | 算出可能（payment_date差分） |
| txtSchRenewalRent | **v5未定義** | 再測定時new_paymentに近いが直接対応なし |
| dgvChangeHistory | lease_remeasurement + lease_modification_assessment | JOIN必要 |

> **注**: Tab3 には上記以外にも約15件の表示系TextBox（txtSchContractDate, txtSchStartDate, txtSchEndDate, txtSchContractPeriod, txtSchFirstPayDate, txtSchPayCount, txtSchFreePeriod, txtSchLastPayDate, txtSchRenewalForecastCount, txtSchAccStartDate, txtSchAccPeriod, txtSchAccEndDate, txtSchRent, txtSchRentTotal 等）が存在する。これらの大半は算出値またはlease_contract/lease_payment_scheduleから直接取得可能であり、適合性への影響は軽微である。詳細は research_ui_db_mapping.md のタブ3セクションを参照。

#### Tab4: 転貸・サブリース → sublease_relationship

| UI要素 | v5対応テーブル.カラム | 適合状況 |
|--------|----------------------|---------|
| chkSublease | sublease_relationship の存在有無 | OK |
| dtpSubleaseStart / End | sublease_relationship.sublease_start_date / end_date | OK |
| txtSublesseeName | **v5未定義** | sublessee_nameカラム追加必須 |
| txtSubleaseArea | **v5未定義** | subleased_areaカラム追加必須 |
| dgvSubleaseIncome | **v5未定義** | sublease_incomeテーブル新規作成必須 |

#### Tab5: リース判定 → lease_option, lease_asset

| UI要素 | v5対応テーブル.カラム | 適合状況 |
|--------|----------------------|---------|
| rbQ1-Q4（判定結果） | **v5未定義** | lease_judgmentテーブル新規作成提案 |
| dtpJudgeStart / dtpJudgeEnd | lease_contract.contract_start_date / contract_end_date | OK |
| lblTermMonths | 算出値 | OK（UI内計算） |
| chkExtOption / cboExtCertainty | lease_option (option_type='extend') | OK |
| chkTerminateOption / cboTerminateCertainty | lease_option (option_type='terminate') | OK |
| numExtMonths | **v5に直接対応なし** | lease_optionから算出 |
| numAssetValue（取得価額） | **v5に直接対応なし** | lease_initial_measurement.rou_amount に近い |
| lblShortTermResult | lease_asset.is_short_term | OK |
| lblLowValueResult | lease_asset.is_low_value | OK |
| chkApplyExemption（免除規定） | lease_asset.is_low_value OR is_short_term | OK |
| chkServiceComponent（構成要素） | **v5に直接対応なし** | practical_expedient 相当が未定義 |
| chkOwnershipTransfer | lease_contract.lease_classification | OK |
| numMonthlyRentJudge（月額リース料） | lease_payment_schedule.payment_amount | OK |
| numDiscountRate（割引率） | lease_initial_measurement.discount_rate_used | OK |
| lblResultText / lblResultBadge / lblResultReason | 算出値 | OK（UI内計算） |

---

### 2.2 FrmAssetDetailEntry

#### 共通パネル → lease_asset

| UI要素 | v5対応 | 適合状況 |
|--------|--------|---------|
| txtAssetNo | lease_asset.asset_no | OK |
| lblAssetCategoryDisplay | lease_asset.asset_category_code | OK |
| txtAssetName | lease_asset.asset_name | OK |
| cmbCompany | lease_contract.lessee_company_id | ハードコード→companyテーブル参照へ |
| txtInstallLocation | lease_asset.location | OK |
| txtRemarks | **v5未定義** | remarks VARCHAR(500) 追加必須 |

#### 不動産パネル → asset_attribute (EAV)

| UI要素 | v4カラム | v5対応 |
|--------|---------|--------|
| txtStructure | re_structure | asset_attribute (field_name='structure') |
| txtArea | re_area | asset_attribute (field_name='area') |
| txtLayout | re_layout | asset_attribute (field_name='layout') |
| dtpCompletion | re_completion_date | asset_attribute (field_name='completion_date') |
| txtLandlordName | re_landlord_name | asset_attribute (field_name='landlord_name') |
| txtBrokerCompany | re_broker_company | asset_attribute (field_name='broker_company') |
| txtUsageRestrictions | re_usage_restrictions | asset_attribute (field_name='usage_restrictions') |

#### 車両パネル → asset_attribute (EAV)

| UI要素 | v4カラム | v5対応 |
|--------|---------|--------|
| txtChassisNo | vh_chassis_no | asset_attribute (field_name='chassis_no') |
| txtRegistrationNo | vh_registration_no | asset_attribute (field_name='registration_no') |
| txtVehicleType | vh_vehicle_type | asset_attribute (field_name='vehicle_type') |
| dtpInspectionDate | vh_inspection_date | asset_attribute (field_name='inspection_date') |
| txtMileageLimit | vh_mileage_limit | asset_attribute (field_name='mileage_limit') |

#### OA機器パネル → asset_attribute (EAV)

| UI要素 | v4カラム | v5対応 |
|--------|---------|--------|
| txtModelNo | oa_model_no | asset_attribute (field_name='model_no') |
| txtSerialNo | oa_serial_no | asset_attribute (field_name='serial_no') |
| dtpMaintenanceDate | oa_maintenance_date | asset_attribute (field_name='maintenance_date') |
| txtMaintenanceContract | oa_maintenance_contract | asset_attribute (field_name='maintenance_contract') |

**EAV化の影響**: 全16項目が固定カラムから EAV パターンに移行。Phase 1 では既存固定パネルを維持しアダプタ層で変換、Phase 3 で asset_class_field ベースの動的UI生成に移行。

---

### 2.3 FrmFlexCtbViewer

| 項目 | 現状 | v5対応 |
|------|------|--------|
| データソース | ctb_lease_integrated (単一テーブル) | v_ctb_export ビュー or v_lease_summary ビュー |
| カラム構成 | 30列ハードコード | 固定列 + EAV PIVOT 動的列 |
| 主キー | ctb_id (単一) | contract_id + asset_id (複合) |
| 資産種別列 | re_*/vh_*/oa_* 固定カラム | asset_attribute PIVOT |

v_ctb_export マテリアライズドビューを活用することで、Phase 1 では SELECT 元の変更のみで対応可能。全30列の詳細マッピングは research_ui_db_mapping.md のセクション1.3を参照。主な廃止列は colCtbId, colPropertyNo（v5複合キーに移行）、v5未定義列は colSegmentCd/Nm, colNonCancellableMonths, colExtensionMonths, colAccountingLeaseTerm, colPaymentIntervalMonths（いずれも算出値化または segment テーブル追加で対応）。

---

## 3. データフロー変更図

### 旧データフロー（v4）

```
FrmLeaseContractMain
  |
  +-- OnRegisterClick()
  |     |
  |     +-- CtbRecord (41プロパティのフラットオブジェクト) を生成
  |           |
  |           +-- CtbDataStore.Add(record)  [メモリストア]
  |           |
  |           +-- CtbRepository.InsertAll(records)  [DB永続化]
  |                 |
  |                 +-- InsertLeaseIntegrated()
  |                 |     ctb_lease_integrated に36カラム一括INSERT
  |                 |
  |                 +-- InsertDeptAllocation()
  |                       ctb_dept_allocation に4カラムINSERT
  |
FrmFlexCtbViewer
  |
  +-- LoadData()
        |
        +-- CtbRepository.SelectAll()
              |
              +-- ctb_lease_integrated
                  LEFT JOIN ctb_dept_allocation
                  LEFT JOIN m_department
                  --> CtbRecord リスト --> DataGridView表示
```

**特徴**: 1テーブルへの1回のINSERT/SELECTで完結。

### 新データフロー（v5）

```
FrmLeaseContractMain
  |
  +-- OnRegisterClick()
  |     |
  |     +-- LeaseContractUnit (複合オブジェクト) を生成
  |     |     +-- ContractData    : LeaseContractRecord
  |     |     +-- Assets[]        : LeaseAssetRecord[]
  |     |     |     +-- Attributes[] : AssetAttributeRecord[]  (EAV)
  |     |     +-- Options[]       : LeaseOptionRecord[]
  |     |     +-- Allocations[]   : DeptAllocationRecord[]
  |     |     +-- Incentives[]    : LeaseIncentiveRecord[]
  |     |     +-- InitialMeas     : LeaseInitialMeasurementRecord
  |     |     +-- PaySchedules[]  : LeasePaymentScheduleRecord[]
  |     |
  |     +-- LeaseContractUnitOfWork.Save(unit)  [新Repository]
  |           |
  |           |  BEGIN TRANSACTION
  |           +-- (1) lease_contract INSERT       --> contract_id 取得
  |           +-- (2) lease_asset INSERT (xN)     --> asset_id 取得
  |           +-- (3) asset_attribute INSERT (xM)   <-- EAV
  |           +-- (4) lease_option INSERT
  |           +-- (5) dept_allocation INSERT
  |           +-- (6) lease_incentive INSERT
  |           +-- (7) lease_initial_measurement INSERT
  |           +-- (8) lease_payment_schedule INSERT (x支払回数)
  |           +-- (9) restoration_obligation INSERT
  |           |  COMMIT
  |           +-- POST: REFRESH MATERIALIZED VIEW v_ctb_export
  |
FrmFlexCtbViewer
  |
  +-- LoadData()
        |
        +-- LeaseViewRepository.SelectAll()
              |
              +-- SELECT * FROM v_ctb_export  [マテリアライズドビュー]
                  +-- 固定カラム    --> DataGridView基本列
                  +-- EAV PIVOT    --> 動的カラム追加
```

### トランザクション設計

```
+-------------------------------------------------------------+
| Transaction Scope (1保存ボタン = 1トランザクション)             |
|                                                              |
|  INSERT lease_contract  --> contract_id (RETURNING)          |
|    |                                                         |
|    +-- INSERT lease_asset xN --> asset_id[] (RETURNING)      |
|    |     |                                                   |
|    |     +-- INSERT asset_attribute xM (EAV)                 |
|    |     +-- INSERT dept_allocation xK                       |
|    |     +-- INSERT lease_option xL                          |
|    |     +-- INSERT lease_incentive xP                       |
|    |     +-- INSERT lease_initial_measurement                |
|    |     +-- INSERT lease_payment_schedule xQ                |
|    |     +-- INSERT restoration_obligation                   |
|    |                                                         |
|  COMMIT                                                      |
|  POST-COMMIT: REFRESH MATERIALIZED VIEW v_ctb_export         |
+-------------------------------------------------------------+
```

**設計ポイント**:
- INSERT順序はFK依存関係に従う: contract -> asset -> (attribute, allocation, option, ...) -> measurement -> schedule
- v_ctb_export リフレッシュはトランザクション外（CONCURRENTLY オプション）
- 書き込みは正規化テーブルに対して行い、読み取りはビュー経由

---

## 4. CtbRecord分割マッピング表

CtbDataStore.vb の CtbRecord クラスの全41プロパティの行き先:

| # | プロパティ名 | 旧テーブル | 新テーブル | 新カラム名 | 変換の有無 |
|---|-------------|-----------|-----------|-----------|-----------|
| 1 | CtbId | ctb_lease_integrated.ctb_id | **廃止** | contract_id + asset_id に分離 | 削除 |
| 2 | ContractNo | ctb_lease_integrated.contract_no | lease_contract | contract_no | なし |
| 3 | PropertyNo | ctb_lease_integrated.property_no | **廃止** | asset_id に置換 | 削除 |
| 4 | ContractName | ctb_lease_integrated.contract_name | lease_contract | contract_name | なし |
| 5 | ContractTypeCd | ctb_lease_integrated.contract_type_cd | lease_contract | contract_type_code | カラム名変更 |
| 6 | SupplierCd | ctb_lease_integrated.supplier_cd | lease_contract | lessor_company_id | 型変更(VARCHAR->INTEGER) + ID変換 |
| 7 | MgmtDeptCd | ctb_lease_integrated.mgmt_dept_cd | lease_asset | mgmt_dept_cd | テーブル移動 |
| 8 | LeaseStartDate | ctb_lease_integrated.lease_start_date | lease_contract | contract_start_date | カラム名変更 |
| 9 | LeaseEndDate | ctb_lease_integrated.lease_end_date | lease_contract | contract_end_date | カラム名変更 |
| 10 | FreeRentMonths | ctb_lease_integrated.free_rent_months | lease_incentive | incentive_type='free_rent' | テーブル分離 + 構造変更 |
| 11 | LeaseTermMonths | ctb_lease_integrated.lease_term_months | **廃止** | 算出値（非保存） | 削除 |
| 12 | AssetNo | ctb_lease_integrated.asset_no | lease_asset | asset_no | テーブル移動 |
| 13 | AssetCategory | ctb_lease_integrated.asset_category | lease_asset | asset_category_code | テーブル移動 + カラム名変更 |
| 14 | AssetName | ctb_lease_integrated.asset_name | lease_asset | asset_name | テーブル移動 |
| 15 | CompanyName | ctb_lease_integrated.company_name | **廃止** | company.company_name (JOIN) | JOIN取得 |
| 16 | InstallLocation | ctb_lease_integrated.install_location | lease_asset | location | テーブル移動 + カラム名変更 |
| 17 | Remarks | ctb_lease_integrated.remarks | lease_asset | remarks | テーブル移動（**v5カラム追加必要**） |
| 18 | ReStructure | ctb_lease_integrated.re_structure | asset_attribute | field_name='structure' | EAV化 |
| 19 | ReArea | ctb_lease_integrated.re_area | asset_attribute | field_name='area' | EAV化 |
| 20 | ReLayout | ctb_lease_integrated.re_layout | asset_attribute | field_name='layout' | EAV化 |
| 21 | ReCompletionDate | ctb_lease_integrated.re_completion_date | asset_attribute | field_name='completion_date' | EAV化 |
| 22 | ReLandlordName | ctb_lease_integrated.re_landlord_name | asset_attribute | field_name='landlord_name' | EAV化 |
| 23 | ReBrokerCompany | ctb_lease_integrated.re_broker_company | asset_attribute | field_name='broker_company' | EAV化 |
| 24 | ReUsageRestrictions | ctb_lease_integrated.re_usage_restrictions | asset_attribute | field_name='usage_restrictions' | EAV化 |
| 25 | VhChassisNo | ctb_lease_integrated.vh_chassis_no | asset_attribute | field_name='chassis_no' | EAV化 |
| 26 | VhRegistrationNo | ctb_lease_integrated.vh_registration_no | asset_attribute | field_name='registration_no' | EAV化 |
| 27 | VhVehicleType | ctb_lease_integrated.vh_vehicle_type | asset_attribute | field_name='vehicle_type' | EAV化 |
| 28 | VhInspectionDate | ctb_lease_integrated.vh_inspection_date | asset_attribute | field_name='inspection_date' | EAV化 |
| 29 | VhMileageLimit | ctb_lease_integrated.vh_mileage_limit | asset_attribute | field_name='mileage_limit' | EAV化 |
| 30 | OaModelNo | ctb_lease_integrated.oa_model_no | asset_attribute | field_name='model_no' | EAV化 |
| 31 | OaSerialNo | ctb_lease_integrated.oa_serial_no | asset_attribute | field_name='serial_no' | EAV化 |
| 32 | OaMaintenanceDate | ctb_lease_integrated.oa_maintenance_date | asset_attribute | field_name='maintenance_date' | EAV化 |
| 33 | OaMaintenanceContract | ctb_lease_integrated.oa_maintenance_contract | asset_attribute | field_name='maintenance_contract' | EAV化 |
| 34 | DeptCd | ctb_dept_allocation.dept_cd | dept_allocation | dept_code | テーブル名+カラム名変更 |
| 35 | DeptName | m_department.dept_name (JOIN) | department | dept_name (JOIN) | テーブル名変更 |
| 36 | AllocationRatio | ctb_dept_allocation.allocation_ratio | dept_allocation | allocation_ratio | 百分率(0-100) -> 比率(0-1) 変換 |
| 37 | MonthlyPayment | ctb_lease_integrated.monthly_payment | lease_payment_schedule | payment_amount | テーブル分離 |
| 38 | LeaseDepreciation | ctb_lease_integrated.lease_depreciation | amortization_schedule | depreciation_amount | テーブル分離（**廃止候補**） |
| 39 | TotalPayment | ctb_lease_integrated.total_payment | lease_initial_measurement | fixed_payment_total | テーブル分離 |
| 40 | SplitStatus | ctb_lease_integrated.split_status | **廃止** | - | 削除 |
| 41 | DeptAllocations | (子テーブル集約) | dept_allocation | 1:Nリレーション | テーブル名変更 |

### プロパティ分散サマリ

| 行き先 | プロパティ数 | 内訳 |
|--------|------------|------|
| lease_contract | 6 | #2,4,5,6,8,9 |
| lease_asset | 6 | #7,12,13,14,16,17 |
| asset_attribute (EAV) | 16 | #18-33 |
| dept_allocation | 3 | #34,35,36 |
| lease_initial_measurement | 1 | #39 |
| lease_payment_schedule | 1 | #37 |
| amortization_schedule | 1 | #38 |
| lease_incentive | 1 | #10 |
| dept_allocation (子テーブル) | 1 | #41 |
| **廃止** | 5 | #1,3,11,15,40 |
| **合計** | **41** | |

---

## 5. DB設計v5への追加提案

### 5.1 カラム追加提案

| # | テーブル | カラム | 型 | 用途 | 優先度 | DDL |
|---|---------|--------|---|------|--------|-----|
| 1 | lease_asset | remarks | VARCHAR(500) | 資産備考（FrmAssetDetailEntry.txtRemarks） | **Must** | `ALTER TABLE lease_asset ADD COLUMN remarks VARCHAR(500);` |
| 2 | lease_payment_schedule | payment_method | VARCHAR(20) | 支払方法（dgvMonthlyPayments.MPayMethod） | **Should** | `ALTER TABLE lease_payment_schedule ADD COLUMN payment_method VARCHAR(20);` |
| 3 | sublease_relationship | sublessee_name | VARCHAR(100) | 転貸先名称（txtSublesseeName） | **Should** | `ALTER TABLE sublease_relationship ADD COLUMN sublessee_name VARCHAR(100);` |
| 4 | sublease_relationship | subleased_area | NUMERIC(10,2) | 転貸面積（txtSubleaseArea） | **Should** | `ALTER TABLE sublease_relationship ADD COLUMN subleased_area NUMERIC(10,2);` |
| 5 | lease_deposit | accounting_treatment | VARCHAR(30) | 会計処理区分（dgvInitialCosts.AcctTreatment） | **Could** | `ALTER TABLE lease_deposit ADD COLUMN accounting_treatment VARCHAR(30);` |

### 5.2 テーブル新規作成提案

#### (1) bank_account — 銀行口座マスタ **Should**

dgvMonthlyPayments の振込先口座列の格納先。

```sql
CREATE TABLE bank_account (
    account_id       SERIAL PRIMARY KEY,
    company_id       INTEGER REFERENCES company(company_id),
    bank_name        VARCHAR(100) NOT NULL,
    branch_name      VARCHAR(100),
    account_type     VARCHAR(20),          -- 'ordinary' / 'current'
    account_number   VARCHAR(20) NOT NULL,
    account_holder   VARCHAR(100),
    create_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- lease_payment_schedule へのFK追加
ALTER TABLE lease_payment_schedule
    ADD COLUMN bank_account_id INTEGER REFERENCES bank_account(account_id);
```

#### (2) sublease_income — サブリース収入明細 **Should**

dgvSubleaseIncome の格納先。

```sql
CREATE TABLE sublease_income (
    income_id        SERIAL PRIMARY KEY,
    sublease_id      INTEGER NOT NULL REFERENCES sublease_relationship(sublease_id),
    income_date      DATE NOT NULL,
    income_amount    NUMERIC(15,2) NOT NULL,
    tax_amount       NUMERIC(15,2) DEFAULT 0,
    remarks          VARCHAR(200),
    create_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### (3) segment — セグメントマスタ **Should**

FrmFlexCtbViewer の colSegmentCd/Nm の格納先。

```sql
CREATE TABLE segment (
    segment_code     VARCHAR(10) PRIMARY KEY,
    segment_name     VARCHAR(100) NOT NULL,
    is_active        BOOLEAN DEFAULT TRUE,
    create_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- lease_asset へのFK追加
ALTER TABLE lease_asset
    ADD COLUMN segment_code VARCHAR(10) REFERENCES segment(segment_code);
```

#### (4) lease_judgment — リース判定結果 **Must**

Tab5（リース判定）のQ1-Q4結果の永続化先。

```sql
CREATE TABLE lease_judgment (
    judgment_id      SERIAL PRIMARY KEY,
    asset_id         INTEGER NOT NULL REFERENCES lease_asset(asset_id),
    judgment_date    DATE NOT NULL,
    q1_asset_identified      BOOLEAN,   -- 資産の特定
    q2_substantive_right     BOOLEAN,   -- 実質的代替権
    q3_economic_benefit      BOOLEAN,   -- 経済的利益
    q4_right_to_direct       BOOLEAN,   -- 使用指図権
    is_lease         BOOLEAN NOT NULL,   -- リース該当判定
    is_short_term    BOOLEAN DEFAULT FALSE,
    is_low_value     BOOLEAN DEFAULT FALSE,
    exemption_applied BOOLEAN DEFAULT FALSE,
    judgment_result  VARCHAR(30),        -- 'finance_transfer' / 'finance_non_transfer' / 'operating' / 'exempt'
    judgment_reason  TEXT,
    create_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_dt        TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

---

## 6. 改修ロードマップ

### Phase 1: 読み取り対応（5人日）

**目標**: v5 DBスキーマを適用しつつ、既存画面を最小変更で表示動作させる

**前提条件**:
- DB設計書v5のDDLが適用済み
- v_ctb_export マテリアライズドビューが作成済み

| # | タスク | 対象ファイル | 工数 |
|---|--------|------------|------|
| 1-1 | v_ctb_export ビューをSELECT元とする LeaseViewRepository を新規作成 | LeaseViewRepository.vb (新規) | 1人日 |
| 1-2 | FrmFlexCtbViewer の LoadData() を LeaseViewRepository.SelectAll() に切り替え | FrmFlexCtbViewer.vb | 0.5人日 |
| 1-3 | CtbRecord をアダプタとして維持（v_ctb_export行 → CtbRecord変換） | CtbDataStore.vb | 0.5人日 |
| 1-4 | LeaseContractRepository / LeaseAccountingRepository のテーブル名・カラム名をv5に修正 | LeaseContractRepository.vb, LeaseAccountingRepository.vb | 1人日 |
| 1-5 | MasterDataLoader のテーブル名をv5に修正（m_department → department 等） | MasterDataLoader.vb | 0.5人日 |
| 1-6 | v_ctb_export ビュー + 必要DB追加カラムのマイグレーションSQL作成 | sql/migration_v5_phase1.sql (新規) | 1人日 |
| 1-7 | 結合テスト（既存画面の表示・基本操作確認） | - | 0.5人日 |

**Phase 1完了時の状態**:
- FrmFlexCtbViewer: v_ctb_export 経由で読み取り可能
- FrmLeaseContractMain: 表示は可能だが、保存はまだ旧構造のまま
- 既存の CtbRepository は読み取り専用として残存

---

### Phase 2: 書き込み対応（10人日）

**目標**: 書き込み処理をv5正規化テーブルに対応させる

**前提条件**:
- Phase 1 完了
- 新Record/DTOクラスの設計レビュー完了

| # | タスク | 対象ファイル | 工数 |
|---|--------|------------|------|
| 2-1 | 新Record/DTOクラス群を作成（7クラス） | Records/ フォルダ (新規) | 1人日 |
| 2-2 | テーブル単位のRepository群を作成（6クラス） | Repositories/ フォルダ (新規) | 3人日 |
| 2-3 | LeaseContractUnitOfWork を作成（集約保存ロジック） | LeaseContractUnitOfWork.vb (新規) | 2人日 |
| 2-4 | FrmLeaseContractMain.OnRegisterClick() を書き換え | FrmLeaseContractMain.vb | 2人日 |
| 2-5 | FrmAssetDetailEntry の戻り値を新Record型に変更 | FrmAssetDetailEntry.vb | 1人日 |
| 2-6 | EAVアダプタ実装（固定パネル入力 → AssetAttributeRecord[]） | EavAdapter.vb (新規) | 0.5人日 |
| 2-7 | DeptAllocation 単位変換 + effective_from デフォルト設定 | DeptAllocationRepository.vb | 0.5人日 |

**新Record/DTOクラス（2-1で作成）**:

```vb
' LeaseContractRecord — lease_contract テーブル対応
Public Class LeaseContractRecord
    Public Property ContractId As Integer
    Public Property ContractNo As String
    Public Property ContractName As String
    Public Property ContractTypeCode As String
    Public Property LessorCompanyId As Integer?
    Public Property LesseeCompanyId As Integer?
    Public Property LeaseClassification As String
    Public Property ContractDate As Date?
    Public Property ContractStartDate As Date?
    Public Property ContractEndDate As Date?
    Public Property HasCancelOption As Boolean
    Public Property AutoRenewal As Boolean
    Public Property RenewalNoticeMonths As Integer?
    Public Property Status As String = "active"
    Public Property Remarks As String
End Class
```

```vb
' LeaseAssetRecord — lease_asset テーブル対応
Public Class LeaseAssetRecord
    Public Property AssetId As Integer
    Public Property ContractId As Integer
    Public Property AssetNo As String
    Public Property AssetName As String
    Public Property AssetCategoryCode As String
    Public Property Location As String
    Public Property Quantity As Integer = 1
    Public Property UsefulLifeMonths As Integer?
    Public Property ResidualValueGuarantee As Decimal
    Public Property IsLowValue As Boolean
    Public Property IsShortTerm As Boolean
    Public Property MgmtDeptCd As String
    Public Property DepreciationMethod As String = "SL"
    Public Property Remarks As String
End Class
```

```vb
' AssetAttributeRecord — asset_attribute テーブル対応 (EAV)
Public Class AssetAttributeRecord
    Public Property AttrId As Integer
    Public Property AssetId As Integer
    Public Property FieldId As Integer
    Public Property ValueText As String
    Public Property ValueNumeric As Decimal?
    Public Property ValueDate As Date?
    Public Property ValueBoolean As Boolean?
End Class
```

```vb
' DeptAllocationRecord — dept_allocation テーブル対応
Public Class DeptAllocationRecord
    Public Property AllocationId As Integer
    Public Property AssetId As Integer
    Public Property DeptCode As String
    Public Property AllocationRatio As Decimal    ' 0-1 (UIでは0-100入力、保存時に/100)
    Public Property EffectiveFrom As Date         ' デフォルト: contract_start_date
    Public Property EffectiveTo As Date?          ' Nothing = 無期限
    Public Property CostCenter As String
End Class
```

**UnitOfWork パターン（2-3で作成）**:

```vb
' LeaseContractUnitOfWork — 集約保存ロジック
Public Class LeaseContractUnitOfWork
    Public Function Save(unit As LeaseContractUnit) As Integer
        Using conn = DbHelper.GetConnection()
            conn.Open()
            Using tx = conn.BeginTransaction()
                Try
                    ' (1) lease_contract INSERT
                    Dim contractId = _contractRepo.Insert(unit.ContractData, conn, tx)

                    For Each asset In unit.Assets
                        asset.ContractId = contractId
                        ' (2) lease_asset INSERT
                        Dim assetId = _assetRepo.Insert(asset, conn, tx)

                        ' (3) asset_attribute INSERT (EAV)
                        _attrRepo.InsertBatch(assetId, asset.Attributes, conn, tx)

                        ' (4) dept_allocation INSERT
                        _allocRepo.InsertBatch(assetId, unit.Allocations, conn, tx)

                        ' (5) lease_option INSERT
                        _optionRepo.InsertBatch(assetId, unit.Options, conn, tx)

                        ' (6) lease_incentive INSERT
                        _incentiveRepo.InsertBatch(assetId, unit.Incentives, conn, tx)

                        ' (7) lease_initial_measurement INSERT
                        unit.InitialMeas.AssetId = assetId
                        _measRepo.Insert(unit.InitialMeas, conn, tx)

                        ' (8) lease_payment_schedule INSERT
                        _schedRepo.InsertBatch(assetId, unit.PaySchedules, conn, tx)

                        ' (9) restoration_obligation INSERT (該当時のみ)
                        If unit.HasRestoration Then
                            _restoRepo.Insert(assetId, unit.Restoration, conn, tx)
                        End If
                    Next

                    tx.Commit()
                    Return contractId
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using

        ' POST: ビューリフレッシュ（トランザクション外）
        _viewRepo.RefreshView()
    End Function
End Class
```

**Phase 2完了時の状態**:
- 保存ボタンで v5 正規化テーブルに正しく書き込み可能
- CtbRepository は廃止可能（LeaseContractUnitOfWork が代替）
- CtbRecord は廃止可能（新Record群が代替）

---

### Phase 3: UI最適化（15人日）

**目標**: v5テーブル構造に合わせたUI改善（EAV動的UI化、新規画面追加）

**前提条件**:
- Phase 2 完了
- v5追加テーブル（bank_account, sublease_income, segment, lease_judgment）のDDL適用済み

| # | タスク | 対象ファイル | 工数 |
|---|--------|------------|------|
| 3-1 | FrmAssetDetailEntry の種別固有パネルを asset_class_field ベース動的UI生成に変更 | FrmAssetDetailEntry.vb, DynamicFieldPanel.vb (新規) | 3人日 |
| 3-2 | FrmFlexCtbViewer を v_ctb_export + 動的カラム(EAV PIVOT)に対応 | FrmFlexCtbViewer.vb | 2人日 |
| 3-3 | dgvDeptAllocation に effective_from/effective_to 列を追加 | FrmAssetDetailEntry.vb | 0.5人日 |
| 3-4 | dgvMonthlyPayments に振込先口座/支払方法列のDB連携を追加 | FrmLeaseContractMain.vb | 1人日 |
| 3-5 | リース判定結果の永続化（lease_judgment テーブル + 保存ロジック） | FrmLeaseContractMain.vb, LeaseJudgmentRepository.vb (新規) | 1.5人日 |
| 3-6 | 転貸タブの不足項目対応（sublessee_name, subleased_area, sublease_income） | FrmLeaseContractMain.vb, SubleaseRepository.vb (新規) | 2人日 |
| 3-7 | 仕訳一覧・照会画面の新規作成 | FrmJournalViewer.vb (新規) | 3人日 |
| 3-8 | セグメント情報対応（segment テーブル + UI追加） | - | 1人日 |
| 3-9 | 結合テスト + リグレッションテスト | - | 1人日 |

**Phase 3完了時の状態**:
- EAV 動的UI で資産種別の追加がコード変更なしで可能
- 仕訳照会画面で journal_header/detail の閲覧が可能
- 転貸タブの全項目がDB連携済み

---

### Phaseサマリ

| Phase | 工数 | 影響範囲 | 完了条件 |
|-------|------|---------|---------|
| Phase 1 | 5人日 | Repository読み取り + マスタ名変更 | FrmFlexCtbViewer がv5 DBから表示可能 |
| Phase 2 | 10人日 | Repository書き込み + Record構造 | 保存ボタンでv5正規化テーブルに書き込み可能 |
| Phase 3 | 15人日 | UI最適化 + 新規画面 | EAV動的UI + 仕訳照会画面 |
| **合計** | **30人日** | | |

### ファイル変更影響マトリクス

| ファイル | Phase 1 | Phase 2 | Phase 3 | 変更種別 |
|---------|---------|---------|---------|---------|
| CtbRepository.vb | 読取専用化 | 廃止 | - | 廃止 |
| CtbDataStore.vb | アダプタ追加 | 廃止 | - | 廃止 |
| FrmFlexCtbViewer.vb | SELECT元変更 | - | 動的カラム | 改修 |
| FrmLeaseContractMain.vb | - | OnRegisterClick書換 | 転貸/判定タブ改修 | 大規模改修 |
| FrmAssetDetailEntry.vb | - | 戻り値型変更 | 動的UI化 | 大規模改修 |
| LeaseContractRepository.vb | テーブル名修正 | CrudHelper対応確認 | - | 改修 |
| LeaseAccountingRepository.vb | テーブル名修正 | Upsert条件修正 | - | 改修 |
| MasterDataLoader.vb | テーブル名修正 | - | - | 軽微改修 |
| LeaseViewRepository.vb | **新規作成** | - | - | 新規 |
| Records/ (7ファイル) | - | **新規作成** | - | 新規 |
| Repositories/ (6ファイル) | - | **新規作成** | - | 新規 |
| LeaseContractUnitOfWork.vb | - | **新規作成** | - | 新規 |
| EavAdapter.vb | - | **新規作成** | - | 新規 |
| DynamicFieldPanel.vb | - | - | **新規作成** | 新規 |
| FrmJournalViewer.vb | - | - | **新規作成** | 新規 |
| SubleaseRepository.vb | - | - | **新規作成** | 新規 |
| LeaseJudgmentRepository.vb | - | - | **新規作成** | 新規 |

---

## 7. リスクと対策

| # | リスク | 影響度 | 発生確率 | 対策 |
|---|--------|--------|---------|------|
| R1 | **トランザクション複雑化によるデータ不整合** — v4の1テーブルINSERTから最大9テーブルINSERTに増加。途中でエラーが発生した場合のロールバック | 高 | 中 | UnitOfWork パターンで1トランザクションに集約。全INSERT/UPDATEを同一トランザクション内で実行し、例外時はRollback。Phase 2 の結合テストで異常系を重点的に検証 |
| R2 | **EAV化によるパフォーマンス劣化** — 16固定カラムから16行のEAV行に変換されることで、PIVOT SELECT のコストが増加 | 中 | 中 | v_ctb_export マテリアライズドビューで読み取りパフォーマンスを担保。書き込み時のバッチINSERTで INSERT 回数を最小化。asset_attribute に (asset_id, field_id) の複合インデックスを設定 |
| R3 | **マテリアライズドビューの更新遅延** — REFRESH MATERIALIZED VIEW CONCURRENTLY の実行中にユーザーが FrmFlexCtbViewer を開くと古いデータが表示される | 低 | 高 | ビューリフレッシュ完了までFrmFlexCtbViewer に「データ更新中」表示。または、リフレッシュをCONCURRENTLYで実行し、読み取りをブロックしない設計にする |
| R4 | **配賦率の単位変換バグ** — UI(0-100) と DB(0-1) の変換が二重適用される、または適用漏れが発生する | 中 | 中 | DeptAllocationRepository 内で変換を一元化。UI側では常に百分率で表示・入力し、Repository層でのみ変換を行う。単体テストで境界値(0, 50, 100)を検証 |
| R5 | **supplier_cd から supplier_id への型変更** — VARCHAR の supplier_cd を INTEGER の supplier_id に変換するマッピングが必要。既存データの移行時にマッピングミスが発生する可能性 | 高 | 中 | supplier テーブルに supplier_cd カラムを残し（後方互換）、UI ではsupplier_cd で検索 → supplier_id を内部取得するルックアップメソッドを実装。データ移行SQL でマッピングテーブルを作成して検証 |
| R6 | **effective_from/effective_to のデフォルト値設定ミス** — dept_allocation のUIに有効期間入力欄がないため、デフォルト値が不適切だと過去・未来の配賦が誤計算される | 中 | 中 | Phase 1 では effective_from = 契約開始日、effective_to = NULL（無期限）をデフォルトとして自動設定。Phase 3 でUIに有効期間入力欄を追加 |
| R7 | **既存データの移行リスク** — ctb_lease_integrated の103カラムモノリスデータを v5 の37テーブルに分割移行する際のデータロス | 高 | 低 | マイグレーションSQL を Phase 1 で作成。移行前後のレコード件数・金額合計のクロスチェックスクリプトを用意。テスト環境で移行リハーサルを最低2回実施 |
| R8 | **v5に未定義のUIデータの消失** — 振込先口座、支払方法、セグメント情報等、v5に格納先がないデータが移行時に失われる | 中 | 高 | Phase 1 のマイグレーションSQL で追加提案テーブル/カラムを同時に作成し、データの受け皿を確保してから移行を実行。未対応データは一時テーブルに退避 |
| R9 | **Phase間の依存関係によるスケジュール遅延** — Phase 2 は Phase 1 の完了が前提。Phase 1 が遅延すると全体に影響 | 中 | 中 | Phase 1 の各タスクに明確な完了基準を設定。Phase 1-2 間で1日のバッファを確保。Phase 3 の一部タスク（DDL作成等）は Phase 2 と並行作業可能 |
| R10 | **CrudHelper の互換性問題** — v5テーブルのカラム型変更（VARCHAR→INTEGER等）により、CrudHelper の Dictionary パラメータ生成でDbTypeミスマッチが発生する可能性 | 低 | 中 | CrudHelper に型推論ロジックがある場合はv5カラム型に合わせて修正。型推論がない場合は、Repository側で明示的にDbType指定。単体テストで全テーブルのINSERT/SELECTを検証 |

---

**以上**
