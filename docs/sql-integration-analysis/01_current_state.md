# 現状分析: SQL1/SQL2 テーブル構成とDB接続状況

## 1. SQL1（マイグレーション版）テーブル一覧

### 概要

- **定義ファイル**: `sql/001_ddl.sql` + `sql/003_*` 〜 `sql/009_*`（ワークテーブル群）
- **テーブル総数**: 66テーブル（001_ddl.sql） + 約21ワークテーブル（003〜009系）
- **命名規則**: 日本語略称のプレフィックス方式
- **DB名**: `lease_m4bs_dev` / ユーザー: `lease_m4bs_user`（`sql/000_init.sql`）

### テーブル分類一覧

#### コードテーブル（c_* : 10テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| c_chu_hnti | 注記単位 | 注記の集計単位区分 |
| c_chuum | 注記有無 | 注記対象フラグ |
| c_kjkbn | 計上区分 | 資産/費用区分 |
| c_kjtaisyo | 計上対象 | 計上対象区分 |
| c_kkbn | 契約区分 | FA/OL/割賦/レンタル等 |
| c_leakbn | リース区分 | リース種別（所有権移転/移転外等） |
| c_rcalc | 再計算区分 | 再計算種別 |
| c_settei_idfld | 設定IDフィールド | 設定画面用ID-値対応 |
| c_skyak_ho | 償却方法 | 定額法/定率法等 |
| c_szei_kjkbn | 消費税計上区分 | 消費税処理種別 |

#### データテーブル（d_* : 6テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| d_kykh | 契約ヘッダ | **中核テーブル（約90カラム）**: 契約番号、リース会社、管理部門、期間、金額等 |
| d_kykm | 物件明細 | **最大テーブル（約120カラム）**: 物件ごとのリース料、返済、償却等の詳細 |
| d_haif | 配賦 | 物件ごとの部門配賦（配賦率、科目、部門） |
| d_henf | 返済ファイル | 返済スケジュール（支払金額、税額、支払日） |
| d_henl | 返済履歴 | 返済実績データ |
| d_gson | 減損 | 減損損失情報（減損日、減損額、累計額） |

#### マスタテーブル（m_* : 19テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| m_bcat | 部門カテゴリ | 管理部門（本社、支社等） |
| m_bkind | 物件種別 | 物件の種別分類 |
| m_bknri | 物件管理 | 物件管理単位 |
| m_corp | 法人 | 自社法人情報 |
| m_genk | 原価 | 原価情報 |
| m_gsha | 外注先 | 外注先マスタ |
| m_hkho | 簿価方法 | 簿価計算方法 |
| m_hkmk | 補助科目 | 仕訳補助科目 |
| m_kknri | 契約管理単位 | **重要**: kknri1_cd でm_departmentへマッピング |
| m_koza | 口座 | 銀行口座情報 |
| m_lcpt | リース会社 | **重要**: lcpt1_cd でm_supplierへマッピング |
| m_mcpt | 物件カテゴリ | 物件分類 |
| m_rsrvb1 | 予備B1 | 予備マスタ |
| m_rsrvh1 | 予備H1 | 予備マスタ（配賦で使用） |
| m_rsrvk1 | 予備K1 | 予備マスタ |
| m_shho | 支払方法 | 支払方法区分 |
| m_skmk | 仕訳科目 | **仕訳科目マスタ**: 借方/貸方科目のコードと名称 |
| m_skti | 仕訳摘要 | 仕訳摘要テキスト |
| m_swptn | 仕訳パターン | 仕訳パターン定義（借方/貸方の科目組み合わせ） |

#### セキュリティテーブル（sec_* : 4テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| sec_user | ユーザー | ユーザー認証・権限 |
| sec_kngn | 権限 | 権限定義 |
| sec_kngn_bknri | 権限-物件管理 | 物件管理単位の権限 |
| sec_kngn_kknri | 権限-契約管理 | 契約管理単位の権限 |

#### 設定テーブル（t_* : 18テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| t_accounting_unit | 会計単位 | 会計単位設定 |
| t_amortization_schedule | 償却スケジュール | 償却計算結果 |
| t_audit_log | 監査ログ | 操作監査 |
| t_db_version | DBバージョン | スキーマバージョン管理 |
| t_holiday | 休日 | 休日カレンダー |
| t_journal_setting | 仕訳設定 | 仕訳出力設定 |
| t_kari_ritu | 借入利率 | 割引率設定 |
| t_kykbnj_seq | 契約番号採番 | 自社契約番号シーケンス |
| t_mstk | マスタ区分 | マスタ管理区分 |
| t_opt | オプション | オプション設定 |
| t_req | リクエスト | 処理リクエスト管理 |
| t_seq | シーケンス | 汎用シーケンス |
| t_settei | 設定 | システム設定（多数の設定フラグ） |
| t_shwak_d | 仕訳出力定義 | 仕訳出力フォーマット定義 |
| t_swk_nm | 仕訳名称 | 仕訳名称マスタ |
| t_system | システム | システム基本情報 |
| t_szei_kmk | 消費税科目 | 消費税科目設定 |
| t_zei_kaisei | 税改正 | 消費税改正履歴 |

#### 計算テーブル（tc_* : 5テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| tc_hrel | 返済履歴 | 返済計算結果 |
| tc_rec_shri | 返済支払 | 支払計算結果 |
| tc_reg_report | 帳票 | 帳票出力結果（非常に多カラム） |
| tc_swk_def_com | 仕訳定義共通 | 仕訳定義の共通設定 |
| tc_swk_settei | 仕訳設定 | 仕訳出力設定（最大のカラム数） |

#### ログテーブル（l_* : 3テーブル）

| テーブル名 | 日本語名 | 概要 |
|---|---|---|
| l_bklog | バックアップログ | バックアップ操作ログ |
| l_slog | システムログ | システム操作ログ |
| l_ulog | ユーザーログ | ユーザー操作ログ |

#### ワークテーブル（tw_* : 計約23テーブル、003〜009系DDLで定義）

| ファイル | テーブル名 | 概要 |
|---|---|---|
| 001_ddl.sql | tw_m_user | ユーザーワーク |
| 003_tw_keijo_tables.sql | tw_s_chuki_keijo | **注記計上結果ワーク（メインパイプライン）** |
| 003_tw_keijo_tables.sql | tw_d_henl_keijo | 返済履歴計上ワーク |
| 003_tw_keijo_tables.sql | tw_d_gson_keijo | 減損計上ワーク |
| 004_tw_kitoku_tables.sql | tw_kitoku_cmsw2wrk | KITOKU伝票ワーク（52フィールド、571バイト固定長） |
| 004_tw_kitoku_tables.sql | tw_kitoku_apgdhwrk | KITOKUヘッダワーク |
| 004_tw_kitoku_tables.sql | tw_kitoku_apgddwrk | KITOKU明細ワーク |
| 004_tw_kitoku_tables.sql | tw_kitoku_apgdswrk | KITOKUサマリワーク |
| 004_tw_settei_tables.sql | tw_f_仕訳出力標準_設定_swksh | 支払照合フレックス設定 |
| 004_tw_settei_tables.sql | tw_f_仕訳出力標準_設定_swkkj | 月次仕訳計上フレックス設定 |
| 004_tw_settei_tables.sql | tw_f_仕訳出力標準_設定_swksm | 月次仕訳サマリ設定 |
| 004_tw_settei_tables.sql | tw_f_仕訳出力標準_設定_swkky | 月次仕訳共用設定 |
| 007_tw_chuki_calc.sql | tw_s_chuki_calc | 注記計算結果ワーク |
| 007_tw_fc_common_tables.sql | tw_fc_swk_wrk | fc系顧客固有仕訳出力共通ワーク |
| 008_tw_joken_tables.sql | tw_s_keijo_joken | 計上条件ワーク |
| 008_tw_joken_tables.sql | tw_s_tougetsu_joken | 当月条件ワーク |
| 008_tw_joken_tables.sql | tw_s_saimu_joken | 債務条件ワーク |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_kj | KJ出力制御ワーク |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_kj_仕訳data | **KJ仕訳データ出力先** |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_sh | SH出力制御ワーク |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_sh_仕訳data | SH仕訳データ出力先 |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_sm | SM出力制御ワーク |
| 009_tw_shiwake_output.sql | tw_f_仕訳出力標準_sm_仕訳data | SM仕訳データ出力先 |

### 仕訳パイプライン

```
[入力]
  d_kykh + d_kykm + d_haif + d_henf
    ↓ KeijoSqlBuilder.BuildSourceSql()
    ↓ （meisai=物件/配賦, taisho=リース/保守/全部）
  tw_s_chuki_keijo（注記計上結果ワーク）
    ↓ KeijoCalculationEngine
    ↓
  tw_f_仕訳出力標準_kj_仕訳data（KJ: 月次仕訳計上）
  tw_f_仕訳出力標準_sh_仕訳data（SH: 月次支払照合）
  tw_f_仕訳出力標準_sm_仕訳data（SM: 月次仕訳サマリ）
    ↓
[出力形式]
  - 標準CSV/固定長テキスト
  - KITOKU固定長出力（tw_kitoku_*）
  - fc系顧客固有出力（tw_fc_swk_wrk）
```

### VB.NET 参照コンポーネント

| クラス | ファイル | 役割 |
|---|---|---|
| KeijoSqlBuilder | `KeijoSqlBuilder.vb` | 計上用SQL生成（d_kykh/d_kykm/d_haif結合、約250行） |
| KlsryoCalculationEngine | `KlsryoCalculationEngine.vb` | リース料計算エンジン |
| ChukiCalcEngine | `ChukiCalcEngine.vb` | 注記計算エンジン（tw_s_chuki_calc書き込み） |

---

## 2. SQL2（新リース対応版）テーブル一覧

### 概要

- **定義ファイル**: `sql/master_tables.sql` + `sql/tw_tables.sql` + `sql/ctb_tables.sql` + `sql/010_ctb_property_eav.sql`
- **テーブル総数**: 25テーブル（マスタ11 + ctb 4 + tw_lease 10）+ ビュー1
- **命名規則**: 英語名（スネークケース）
- **DB名**: `lease_m4bs` / ユーザー: `manager`（`sql/init.sql`）
- **設計思想**: ASBJ第16号（使用権資産）対応、EAV拡張パターン

### マスタテーブル（m_* : 11テーブル）

| テーブル名 | 概要 |
|---|---|
| m_company | 法人マスタ（company_cd/nm、最大3セット） |
| m_supplier | リース会社マスタ（締日・支払日の3行分パターン） |
| m_payment_method | 支払方法マスタ |
| m_department | 部門マスタ（dept_cd/nm 5セット + 集約カテゴリ3セット） |
| m_asset_category | 資産カテゴリ（勘定科目12種設定） |
| m_bank_account | 銀行口座マスタ |
| m_contract_type | 契約区分マスタ（FA/OL/割賦/レンタル） |
| m_initial_cost_item | 初回費用項目マスタ |
| m_acct_treatment | 会計処理区分マスタ |
| m_monthly_item | 月次項目マスタ |
| m_property_attribute_def | **属性定義マスタ（EAV）**: 資産カテゴリ別の動的項目定義 |

### CTBテーブル（ctb_* : 4テーブル）

| テーブル名 | 概要 |
|---|---|
| ctb_lease_integrated | **契約統合テーブル**: contract_no + property_no で一意。不動産/車両/OA属性カラム含む |
| ctb_dept_allocation | 部門配賦テーブル（ctb_idに対する配賦率・金額） |
| ctb_property | **物件マスタ（EAV）**: ctb_id配下の物件管理。asset_category_cdでカテゴリ紐づけ |
| ctb_property_attribute | **属性値テーブル（EAV）**: property_id + attr_def_id → attribute_value（TEXT格納） |

### ワークテーブル（tw_lease_* : 10テーブル）

| テーブル名 | 概要 |
|---|---|
| tw_lease_contract | リース契約（contract_no一意、契約名、期間、オプション情報） |
| tw_lease_property | 物件属性（面積、住所、間取り、耐用年数） |
| tw_lease_party | 関係者（貸主/代理人/仲介/借主/保証人） |
| tw_lease_schedule | 支払スケジュール（支払額、間隔、回数） |
| tw_lease_accounting | **会計計算**（割引率、現在価値、使用権資産、リース負債） |
| tw_lease_judgment | **リース判定**（ASBJ第16号 Q1-Q4判定、短期/少額免除） |
| tw_lease_initial | 初回費用（費用項目・会計処理区分ごとの金額） |
| tw_lease_sublease | 転貸情報 |
| tw_lease_payment_actual | 月次支払実績（予定額 vs 実績額の差異管理） |
| tw_lease_journal | **月次仕訳**（借方/貸方科目・金額、ASBJ参照情報） |

### ビュー

| ビュー名 | 概要 |
|---|---|
| v_ctb_property_full | 物件統合ビュー（ctb_property + ctb_lease_integrated + m_asset_category結合） |

### tm_USERテーブル（拡張）

`sql/alter_tm_user.sql` により、既存の `tm_USER` テーブルにログイン認証カラムを追加:
- `login_id`, `password_hash`（PBKDF2-SHA256）
- `role`（admin / accounting / general_affairs / viewer）
- `is_active`, `last_login_at`, `failed_login_count`, `locked_until`

---

## 3. 機能的重複マッピング表

| SQL1（旧・日本語略称） | SQL2（新・英語命名） | 重複度 | 備考 |
|---|---|---|---|
| m_bcat（部門カテゴリ） | m_department（部門） | 高 | m_bcat.bcat1_cd 〜 bcat5_cd に相当するdept_cd 5セット |
| m_lcpt（リース会社） | m_supplier（リース会社） | 高 | m_lcpt.lcpt1_cd → m_supplier.supplier_cd でマッピング |
| c_kkbn（契約区分） | m_contract_type（契約区分） | 完全重複 | kkbn_id → LPAD(kkbn_id,2,'0') で変換 |
| m_corp（法人） | m_company（法人） | 高 | company_cd の3セット構成は同等 |
| sec_user（ユーザー） | tm_USER（ユーザー） | 高 | 旧: 権限テーブル分離、新: 単一テーブルにロール統合 |
| m_shho（支払方法） | m_payment_method（支払方法） | 高 | コード体系は異なるが機能同一 |
| m_hkmk（補助科目） | m_asset_category（資産カテゴリ） | 中 | 新版は勘定科目12種を直接保持 |
| m_koza（口座） | m_bank_account（口座） | 高 | 構造簡略化 |
| d_kykh + d_kykm | tw_lease_contract + ctb_lease_integrated | 高（3系統） | 旧: ヘッダ+明細の2テーブル、新: 契約情報+統合テーブルの2テーブル。migrate_d_kykh_to_ctb.sql で変換ロジック定義済み |
| d_haif（配賦） | ctb_dept_allocation（部門配賦） | 高 | 旧: 配賦率+科目+5属性、新: 配賦率+金額 |
| d_henf（返済） | tw_lease_schedule（支払スケジュール） | 中 | 新版はスケジュール種別・間隔の概念追加 |
| d_henl（返済履歴） | tw_lease_payment_actual（支払実績） | 中 | 新版は予定額 vs 実績額の差異管理 |
| d_gson（減損） | ※該当なし | - | 新版ではtw_lease_accountingに統合検討中 |
| ※該当なし | tw_lease_judgment（リース判定） | - | ASBJ第16号対応の新規テーブル |
| ※該当なし | tw_lease_accounting（会計計算） | - | 使用権資産・リース負債の計算結果 |
| ※該当なし | tw_lease_journal（月次仕訳） | - | 新基準対応の仕訳出力 |

---

## 4. DB接続の不整合

### 現状の接続設定

| 系統 | DB名 | ユーザー | 定義ファイル |
|---|---|---|---|
| SQL1（マイグレーション版） | `lease_m4bs_dev` | `lease_m4bs_user` | `sql/000_init.sql` |
| SQL2（新リース対応版） | `lease_m4bs` | `manager` | `sql/init.sql` |
| VB.NET アプリケーション | App.config参照（単一DB） | App.config参照 | `DbConnectionManager.vb` |

### 問題点

1. **DB名の不一致**: SQL1は `lease_m4bs_dev`、SQL2は `lease_m4bs` と別データベースを想定
2. **ユーザーの不一致**: SQL1は `lease_m4bs_user`（権限制限あり）、SQL2は `manager`（オーナー権限）
3. **VB.NET側の単一接続**: `DbConnectionManager.vb` は App.config の `"LeaseM4BS"` 接続文字列1つのみを参照。複数DB接続の仕組みがない
4. **テスト環境**: SQL1は `lease_m4bs_test` も定義（000_init.sql）、SQL2にはテスト環境定義なし
5. **パスワード管理**: SQL1は `iltex_mega_pass_m4`、SQL2は `pass` とセキュリティレベルが異なる

### 影響

- SQL1とSQL2のテーブルが同一DBに存在しない場合、`migrate_d_kykh_to_ctb.sql` のクロステーブル参照が機能しない
- VB.NETアプリケーションから両系統のテーブルにアクセスするには、接続設定の統一またはマルチDB対応が必要
