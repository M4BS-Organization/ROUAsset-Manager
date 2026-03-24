# ER図: SQL1/SQL2 テーブル関連図

## 1. SQL1 主要テーブル関連図（マイグレーション版）

```mermaid
erDiagram
    d_kykh ||--o{ d_kykm : "契約ヘッダ → 物件明細"
    d_kykm ||--o{ d_haif : "物件 → 配賦"
    d_kykm ||--o{ d_henf : "物件 → 返済スケジュール"
    d_kykm ||--o{ d_henl : "物件 → 返済履歴"
    d_kykm ||--o{ d_gson : "物件 → 減損"

    d_kykh }o--|| m_lcpt : "リース会社"
    d_kykh }o--|| m_kknri : "契約管理単位"
    d_kykh }o--|| c_kkbn : "契約区分"
    d_kykh }o--|| c_leakbn : "リース区分"
    d_kykh }o--|| c_kjkbn : "計上区分"

    d_haif }o--|| m_bcat : "配賦部門"
    d_haif }o--|| m_hkmk : "補助科目"
    d_haif }o--|| m_rsrvh1 : "予備H1"

    d_henf }o--|| m_shho : "支払方法"

    m_skmk ||--o{ m_swptn : "仕訳科目 → 仕訳パターン"

    d_kykh {
        integer kykh_id PK "契約ヘッダID"
        double_precision kykh_no "契約ヘッダ番号"
        varchar kykbnj "自社契約番号"
        varchar kykbnl "相手方契約番号"
        integer kkbn_id FK "契約区分ID"
        integer lcpt_id FK "リース会社ID"
        integer kknri_id FK "契約管理単位ID"
        integer leakbn_id FK "リース区分ID"
        integer kjkbn_id FK "計上区分ID"
        timestamp start_dt "開始日"
        timestamp end_dt "終了日"
        integer lkikan "リース期間(月)"
        double_precision k_glsryo "月額リース料"
        double_precision k_slsryo "総額リース料"
        varchar kyak_nm "契約名称"
        varchar rng_bango "稟議番号"
    }

    d_kykm {
        integer kykm_id PK "物件明細ID"
        integer kykh_id FK "契約ヘッダID"
        double_precision kykm_no "物件番号"
        varchar bukn_nm "物件名称"
        integer saikaisu "再開回数"
        double_precision klsryo "月額リース料"
        double_precision slsryo "総額リース料"
    }

    d_haif {
        integer kykm_id PK "物件ID"
        smallint line_id PK "行ID"
        double_precision haifritu "配賦率"
        integer hkmk_id FK "補助科目ID"
        integer h_bcat_id FK "配賦部門ID"
    }

    d_henf {
        integer kykm_id PK "物件ID"
        smallint line_id PK "行ID"
        timestamp shri_dt1 "支払日"
        double_precision klsryo "リース料"
        double_precision kzei "消費税"
        integer shho_id FK "支払方法ID"
    }

    d_gson {
        integer kykm_id PK "物件ID"
        smallint line_id PK "行ID"
        timestamp gson_dt "減損日"
        double_precision gson_ryo "減損額"
        double_precision gson_rkei "減損累計"
    }

    m_lcpt {
        integer lcpt_id PK "リース会社ID"
        varchar lcpt1_cd "リース会社コード1"
        varchar lcpt1_nm "リース会社名1"
    }

    m_kknri {
        integer kknri_id PK "管理単位ID"
        varchar kknri1_cd "管理コード1"
        varchar kknri1_nm "管理名称1"
    }

    m_bcat {
        integer bcat_id PK "部門カテゴリID"
        varchar bcat1_cd "部門コード1"
        varchar bcat1_nm "部門名1"
    }

    m_skmk {
        integer skmk_id PK "仕訳科目ID"
        varchar skmk_cd "科目コード"
        varchar skmk_nm "科目名称"
    }

    c_kkbn {
        smallint kkbn_id PK "契約区分ID"
        varchar kkbn_nm "契約区分名"
    }

    c_leakbn {
        smallint leakbn_id PK "リース区分ID"
        varchar leakbn_nm "リース区分名"
    }

    c_kjkbn {
        smallint kjkbn_id PK "計上区分ID"
        varchar kjkbn_nm "計上区分名"
    }

    m_hkmk {
        integer hkmk_id PK "補助科目ID"
        varchar hkmk_cd "補助科目コード"
        varchar hkmk_nm "補助科目名"
    }

    m_shho {
        integer shho_id PK "支払方法ID"
        varchar shho_nm "支払方法名"
    }

    m_rsrvh1 {
        integer rsrvh1_id PK "予備H1 ID"
    }

    m_swptn {
        integer swptn_id PK "仕訳パターンID"
        integer skmk_id FK "仕訳科目ID"
    }
```

## 2. SQL2 主要テーブル関連図（新リース対応版）

```mermaid
erDiagram
    tw_lease_contract ||--o| tw_lease_accounting : "契約 → 会計計算"
    tw_lease_contract ||--o| tw_lease_judgment : "契約 → リース判定"
    tw_lease_contract ||--o{ tw_lease_property : "契約 → 物件属性"
    tw_lease_contract ||--o{ tw_lease_party : "契約 → 関係者"
    tw_lease_contract ||--o{ tw_lease_schedule : "契約 → 支払スケジュール"
    tw_lease_contract ||--o{ tw_lease_initial : "契約 → 初回費用"
    tw_lease_contract ||--o| tw_lease_sublease : "契約 → 転貸"
    tw_lease_contract ||--o{ tw_lease_payment_actual : "契約 → 支払実績"
    tw_lease_contract ||--o{ tw_lease_journal : "契約 → 月次仕訳"

    tw_lease_initial }o--|| m_initial_cost_item : "初回費用項目"
    tw_lease_initial }o--|| m_acct_treatment : "会計処理区分"
    tw_lease_payment_actual }o--|| m_payment_method : "支払方法"

    ctb_lease_integrated ||--o{ ctb_dept_allocation : "統合 → 部門配賦"
    ctb_lease_integrated ||--o{ ctb_property : "統合 → 物件"
    ctb_lease_integrated }o--|| m_contract_type : "契約区分"
    ctb_lease_integrated }o--|| m_supplier : "リース会社"
    ctb_lease_integrated }o--|| m_department : "管理部門"

    ctb_dept_allocation }o--|| m_department : "配賦部門"

    ctb_property }o--|| m_asset_category : "資産カテゴリ"
    ctb_property ||--o{ ctb_property_attribute : "物件 → 属性値"
    ctb_property_attribute }o--|| m_property_attribute_def : "属性定義"
    m_property_attribute_def }o--|| m_asset_category : "カテゴリ別定義"

    tw_lease_contract {
        serial contract_id PK "契約ID"
        varchar contract_no UK "契約番号"
        varchar contract_name "契約名"
        date start_date "開始日"
        date end_date "終了日"
        integer contract_months "契約月数"
        varchar cancel_option "解約オプション"
        varchar extend_option "延長オプション"
    }

    tw_lease_judgment {
        serial judgment_id PK "判定ID"
        integer contract_id FK_UK "契約ID"
        boolean q1_result "Q1判定"
        boolean q2_result "Q2判定"
        boolean q3_result "Q3判定"
        boolean q4_result "Q4判定"
        boolean is_exempt_short "短期免除"
        boolean is_exempt_small "少額免除"
        varchar final_result "最終判定結果"
    }

    tw_lease_accounting {
        serial accounting_id PK "会計ID"
        integer contract_id FK_UK "契約ID"
        numeric discount_rate "割引率"
        numeric present_value "現在価値"
        numeric rou_asset "使用権資産"
        numeric lease_liability "リース負債"
        numeric lease_ratio "リース比率"
        numeric non_lease_ratio "非リース比率"
    }

    tw_lease_journal {
        serial journal_id PK "仕訳ID"
        integer contract_id FK "契約ID"
        varchar journal_ym "仕訳年月"
        varchar journal_type "仕訳種別"
        varchar debit_account_cd "借方科目コード"
        numeric debit_amount "借方金額"
        varchar credit_account_cd "貸方科目コード"
        numeric credit_amount "貸方金額"
        varchar asbj_reference "ASBJ参照"
    }

    tw_lease_schedule {
        serial schedule_id PK "スケジュールID"
        integer contract_id FK "契約ID"
        varchar schedule_type "種別"
        numeric payment_amount "支払額"
        date first_payment_date "初回支払日"
        varchar payment_interval "支払間隔"
        integer total_count "総回数"
    }

    ctb_lease_integrated {
        serial ctb_id PK "CTB ID"
        varchar contract_no "契約番号"
        integer property_no "物件番号"
        varchar contract_type_cd FK "契約区分コード"
        varchar supplier_cd FK "リース会社コード"
        varchar mgmt_dept_cd FK "管理部門コード"
        date lease_start_date "開始日"
        date lease_end_date "終了日"
        numeric monthly_payment "月額支払"
        numeric total_payment "総額支払"
        varchar split_status "分割状態"
    }

    ctb_dept_allocation {
        serial allocation_id PK "配賦ID"
        integer ctb_id FK "CTB ID"
        varchar dept_cd FK "部門コード"
        numeric allocation_ratio "配賦率"
        numeric payment_amount "配賦金額"
    }

    ctb_property {
        serial property_id PK "物件ID"
        integer ctb_id FK "CTB ID"
        integer property_no "物件連番"
        varchar asset_category_cd FK "資産カテゴリ"
        varchar asset_no "資産番号"
        varchar asset_name "資産名称"
    }

    ctb_property_attribute {
        serial prop_attr_id PK "属性値ID"
        integer property_id FK "物件ID"
        integer attr_def_id FK "属性定義ID"
        text attribute_value "属性値"
    }

    m_department {
        varchar dept_cd PK "部門コード"
        varchar dept_nm "部門名"
    }

    m_supplier {
        varchar supplier_cd PK "リース会社コード"
        varchar supplier_nm "リース会社名"
    }

    m_contract_type {
        varchar contract_type_cd PK "契約区分コード"
        varchar contract_type_nm "契約区分名"
    }

    m_company {
        varchar company_cd PK "法人コード"
        varchar company_nm "法人名"
    }

    m_asset_category {
        varchar asset_category_cd PK "カテゴリコード"
        varchar asset_category_nm "カテゴリ名"
    }

    m_property_attribute_def {
        serial attr_def_id PK "定義ID"
        varchar asset_category_cd FK "カテゴリコード"
        varchar attr_cd "属性コード"
        varchar attr_nm "属性名"
        varchar data_type "データ型"
        varchar display_type "表示種別"
    }

    m_payment_method {
        varchar payment_method_cd PK "支払方法コード"
        varchar payment_method_nm "支払方法名"
    }

    m_initial_cost_item {
        varchar cost_item_cd PK "費用項目コード"
        varchar cost_item_nm "費用項目名"
    }

    m_acct_treatment {
        varchar acct_treatment_cd PK "会計処理コード"
        varchar acct_treatment_nm "会計処理名"
    }
```

## 3. 重複マスタ対応関係図

```mermaid
erDiagram
    m_bcat ||--|| m_department : "部門: bcat1_cd ≒ dept_cd"
    m_lcpt ||--|| m_supplier : "リース会社: lcpt1_cd ≒ supplier_cd"
    c_kkbn ||--|| m_contract_type : "契約区分: LPAD(kkbn_id,2) = contract_type_cd"
    m_corp ||--|| m_company : "法人: 構造同等"
    sec_user ||--|| tm_USER : "ユーザー: 権限モデル異なる"
    m_shho ||--|| m_payment_method : "支払方法: コード体系異なる"
    m_koza ||--|| m_bank_account : "口座: 構造簡略化"

    m_bcat {
        integer bcat_id PK "SQL1:部門カテゴリID"
        varchar bcat1_cd "部門コード1"
        varchar bcat1_nm "部門名1"
        varchar bcat2_cd "部門コード2"
        varchar bcat2_nm "部門名2"
        varchar bcat3_cd "部門コード3"
        varchar bcat3_nm "部門名3"
    }

    m_department {
        varchar dept_cd PK "SQL2:部門コード"
        varchar dept_nm "部門名"
        varchar dept_cd2 "部門コード2"
        varchar dept_nm2 "部門名2"
        varchar dept_cd3 "部門コード3"
        varchar dept_nm3 "部門名3"
    }

    m_lcpt {
        integer lcpt_id PK "SQL1:リース会社ID"
        varchar lcpt1_cd "リース会社コード1"
        varchar lcpt1_nm "リース会社名1"
    }

    m_supplier {
        varchar supplier_cd PK "SQL2:リース会社コード"
        varchar supplier_nm "リース会社名"
        smallint row1_contract_closing_day "締日1"
    }

    c_kkbn {
        smallint kkbn_id PK "SQL1:契約区分ID(1,2,3,4)"
        varchar kkbn_nm "契約区分名"
    }

    m_contract_type {
        varchar contract_type_cd PK "SQL2:契約区分コード(01,02,03,04)"
        varchar contract_type_nm "契約区分名"
    }

    m_corp {
        integer corp_id PK "SQL1:法人ID"
    }

    m_company {
        varchar company_cd PK "SQL2:法人コード"
        varchar company_nm "法人名"
    }

    sec_user {
        integer user_id PK "SQL1:ユーザーID"
    }

    tm_USER {
        serial user_id PK "SQL2:ユーザーID"
        varchar login_id "ログインID"
        varchar role "ロール"
    }

    m_shho {
        integer shho_id PK "SQL1:支払方法ID"
        varchar shho_nm "支払方法名"
    }

    m_payment_method {
        varchar payment_method_cd PK "SQL2:支払方法コード"
        varchar payment_method_nm "支払方法名"
    }

    m_koza {
        integer koza_id PK "SQL1:口座ID"
    }

    m_bank_account {
        varchar bank_account_cd PK "SQL2:口座コード"
        varchar bank_account_nm "口座名"
    }
```

## 4. migrate_d_kykh_to_ctb データフロー図

```mermaid
flowchart TB
    subgraph SQL1_SOURCE ["SQL1: ソーステーブル"]
        d_kykh["d_kykh<br/>契約ヘッダ<br/>(90カラム)"]
        m_kknri["m_kknri<br/>契約管理単位"]
        m_lcpt_s["m_lcpt<br/>リース会社"]
    end

    subgraph MAPPING ["マッピングロジック"]
        MAP1["kknri_id → m_kknri.kknri1_cd<br/>→ m_department.dept_cd"]
        MAP2["kkbn_id → LPAD(kkbn_id, 2, '0')<br/>→ m_contract_type.contract_type_cd"]
        MAP3["lcpt_id → m_lcpt.lcpt1_cd<br/>→ m_supplier.supplier_cd"]
        FILTER["WHERE k_history_f IS NOT TRUE<br/>(履歴レコード除外)"]
        CONFLICT["ON CONFLICT (contract_no, property_no)<br/>DO NOTHING<br/>(重複時スキップ)"]
    end

    subgraph SQL2_TARGET ["SQL2: ターゲットテーブル"]
        ctb["ctb_lease_integrated<br/>契約統合テーブル"]
        ctb_dept["ctb_dept_allocation<br/>部門配賦"]
    end

    subgraph FIELD_MAPPING ["フィールドマッピング"]
        F1["kykbnj → contract_no"]
        F2["kyak_nm → contract_name"]
        F3["start_dt → lease_start_date"]
        F4["end_dt → lease_end_date"]
        F5["lkikan → lease_term_months"]
        F6["k_glsryo → monthly_payment"]
        F7["k_slsryo → total_payment"]
        F8["kykbnl + rng_bango → remarks"]
    end

    d_kykh --> MAP1
    d_kykh --> MAP2
    d_kykh --> MAP3
    m_kknri --> MAP1
    m_lcpt_s --> MAP3

    MAP1 --> FILTER
    MAP2 --> FILTER
    MAP3 --> FILTER

    FILTER --> CONFLICT
    CONFLICT --> ctb

    d_kykh --> FIELD_MAPPING
    FIELD_MAPPING --> ctb

    ctb -->|"mgmt_dept_cd IS NOT NULL<br/>かつ未登録の場合"| ctb_dept
    ctb_dept -->|"allocation_ratio = 100%<br/>payment_amount = monthly_payment"| ctb_dept

    style SQL1_SOURCE fill:#e8f0fe,stroke:#4285f4
    style SQL2_TARGET fill:#e6f4ea,stroke:#34a853
    style MAPPING fill:#fef7e0,stroke:#fbbc04
    style FIELD_MAPPING fill:#fce8e6,stroke:#ea4335
```

## 5. 仕訳パイプライン全体フロー図

```mermaid
flowchart TB
    subgraph INPUT ["入力データ（SQL1）"]
        d_kykh2["d_kykh 契約ヘッダ"]
        d_kykm2["d_kykm 物件明細"]
        d_haif2["d_haif 配賦"]
        d_henf2["d_henf 返済"]
        d_gson2["d_gson 減損"]
    end

    subgraph JOKEN ["条件ワーク"]
        tw_keijo["tw_s_keijo_joken<br/>計上条件"]
        tw_tougetsu["tw_s_tougetsu_joken<br/>当月条件"]
        tw_saimu["tw_s_saimu_joken<br/>債務条件"]
    end

    subgraph ENGINE ["VB.NET 計算エンジン"]
        KSB["KeijoSqlBuilder<br/>計上用SQL生成"]
        KCE["KeijoCalculationEngine<br/>計上計算"]
        CCE["ChukiCalcEngine<br/>注記計算"]
        KLE["KlsryoCalculationEngine<br/>リース料計算"]
    end

    subgraph WORK ["中間ワーク"]
        tw_chuki["tw_s_chuki_keijo<br/>注記計上結果"]
        tw_henl["tw_d_henl_keijo<br/>返済履歴計上"]
        tw_gson_k["tw_d_gson_keijo<br/>減損計上"]
        tw_calc["tw_s_chuki_calc<br/>注記計算結果"]
    end

    subgraph OUTPUT ["仕訳出力ワーク"]
        tw_kj["tw_f_仕訳出力標準_kj_仕訳data<br/>KJ: 月次仕訳計上"]
        tw_sh["tw_f_仕訳出力標準_sh_仕訳data<br/>SH: 月次支払照合"]
        tw_sm["tw_f_仕訳出力標準_sm_仕訳data<br/>SM: 月次仕訳サマリ"]
    end

    subgraph FILE_OUTPUT ["ファイル出力"]
        CSV["標準CSV/固定長"]
        KITOKU["KITOKU固定長<br/>tw_kitoku_cmsw2wrk等"]
        FC["fc系顧客固有<br/>tw_fc_swk_wrk"]
    end

    subgraph NEW_LEASE ["新リース対応（SQL2）"]
        tw_judgment["tw_lease_judgment<br/>リース判定（ASBJ16号）"]
        tw_accounting["tw_lease_accounting<br/>会計計算（使用権資産）"]
        tw_journal["tw_lease_journal<br/>月次仕訳（新基準）"]
    end

    d_kykh2 --> KSB
    d_kykm2 --> KSB
    d_haif2 --> KSB
    tw_keijo --> KCE

    KSB --> tw_chuki
    d_henf2 --> tw_henl
    d_gson2 --> tw_gson_k

    KCE --> tw_chuki
    CCE --> tw_calc
    KLE --> tw_chuki

    tw_chuki --> tw_kj
    tw_chuki --> tw_sh
    tw_chuki --> tw_sm

    tw_kj --> CSV
    tw_sh --> CSV
    tw_sm --> CSV
    tw_chuki --> KITOKU
    tw_chuki --> FC

    tw_judgment -.->|"将来統合"| tw_journal
    tw_accounting -.->|"将来統合"| tw_journal

    style INPUT fill:#e8f0fe,stroke:#4285f4
    style ENGINE fill:#f3e8fd,stroke:#a142f4
    style WORK fill:#fef7e0,stroke:#fbbc04
    style OUTPUT fill:#e6f4ea,stroke:#34a853
    style FILE_OUTPUT fill:#fce8e6,stroke:#ea4335
    style NEW_LEASE fill:#e0f7fa,stroke:#00acc1
    style JOKEN fill:#fff3e0,stroke:#ff9800
```
