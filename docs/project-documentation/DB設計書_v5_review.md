# DB設計書 v5.0 レビュー結果

**レビュー日**: 2026年3月12日
**対象ドキュメント**: DB設計書_v5.md（3312行）
**参照ドキュメント**: analysis_optimal_db.md（最適DB設計 統合分析レポート）
**レビュアー**: レビューエージェント

---

## エグゼクティブサマリー

DB設計書 v5.0 は、v4の103カラムモノリス(ctb_lease_integrated)を3NF準拠の36テーブル+7ビューに適切に分解した包括的な設計書である。analysis_optimal_db.md の改善提案はほぼ全て反映されており、ASBJ第34号の設例カバレッジも大幅に改善されている。

ただし、3パート結合に起因する構造的不整合（重複ヘッダ、カラム名の不一致）、ビューSQL内のカラム名参照エラー、および一部テーブルのカラム数表記の矛盾が検出された。

### 指摘件数サマリー

| 重要度 | 件数 |
|---|---|
| **Critical（構造的な問題）** | 3 |
| **Major（修正推奨）** | 8 |
| **Minor（改善推奨）** | 7 |

---

## Critical（構造的な問題）

### C-1. FK参照先カラム名の不一致（DDLが実行不可能になる）

以下のFK制約で、参照先テーブルのPKカラム名と異なるカラム名を参照しており、DDL実行時にエラーとなる。

| テーブル | FK制約 | DDL記述 | 正しい参照先 |
|---|---|---|---|
| lease_contract | fk_lease_contract_type | `REFERENCES contract_type(contract_type_code)` | `contract_type(type_code)` ※PKは `type_code` |
| lease_asset | fk_lease_asset_category | `REFERENCES asset_category(asset_category_code)` | `asset_category(category_code)` ※PKは `category_code` |
| lease_asset | fk_lease_asset_dept | `REFERENCES department(dept_cd)` | `department(dept_code)` ※PKは `dept_code` |

**影響**: DDLスクリプトを実行するとFK制約作成時にエラーが発生し、テーブル間の参照整合性が確保できない。

**推奨対応**: 全FK制約のREFERENCES句を参照先テーブルのPKカラム名に合わせる。

### C-2. ビューSQL内のカラム名がテーブル定義と不一致

v_ctb_export、v_disclosure_maturity_analysis、v_lease_summary、v_monthly_journal_pending、v_lease_expiry_alert の各ビューSQLで、テーブル定義に存在しないカラム名を多数参照している。

**v_ctb_export（13.1）の主な不一致**:

| ビュー内の参照 | 実際のテーブル定義 | 問題 |
|---|---|---|
| `c.contract_type_cd` | lease_contract.contract_type_code | カラム名不一致 |
| `c.supplier_cd` | lease_contractにsupplier_cdは未定義 | カラム自体が存在しない（lessor_company_idがFKとして存在） |
| `a.asset_category_cd` | lease_asset.asset_category_code | カラム名不一致 |
| `a.install_location` | lease_asset.location | カラム名不一致 |
| `a.lease_start_date` / `a.lease_end_date` | lease_assetにlease_start/end_dateは未定義 | 契約日はlease_contractに定義（contract_start_date/contract_end_date） |
| `a.mgmt_dept_cd` | lease_asset.mgmt_dept_cd | 一致（OK） |
| `im.lease_classification` | lease_contractに定義（lease_initial_measurementには未定義） | テーブル違い |
| `im.discount_rate` | lease_initial_measurement.discount_rate_used | カラム名不一致 |
| `im.lease_term_months` | lease_initial_measurementに未定義 | カラムが存在しない（3NF化で削除済み） |
| `im.total_lease_payment` | lease_initial_measurement.fixed_payment_total | カラム名不一致 |
| `im.present_value_lease_payment` | lease_initial_measurement.pv_lease_payments | カラム名不一致 |
| `im.initial_rou_amount` | lease_initial_measurement.rou_amount | カラム名不一致 |
| `im.initial_lease_liability` | lease_initial_measurement.liability_amount | カラム名不一致 |
| `im.incentive_received` | lease_initial_measurement.lease_incentive_received | カラム名不一致 |
| `ac.status` | lease_accountingにstatusは未定義 | カラムが存在しない |
| `ac.accumulated_depreciation` | lease_accounting.rou_accumulated_depreciation | カラム名不一致 |

**v_disclosure_maturity_analysis（13.2）の不一致**:
- `a.asset_category_cd` → 正: `a.asset_category_code`
- `cat.asset_category_nm` → 正: `cat.category_name`
- `ON a.asset_category_cd = cat.asset_category_cd` → 正: `ON a.asset_category_code = cat.category_code`
- `ac.status` → lease_accountingにstatusは未定義

**v_disclosure_rou_reconciliation（13.3）の不一致**:
- 同上のカラム名問題に加え、`am.period_seq`（未定義、正: `am.period`）、`am.rou_carrying_start`/`am.rou_carrying_end`（未定義、正: `am.rou_carrying_amount`）、`am.impairment_amount`（未定義、正: `am.impairment_in_period`）

**v_disclosure_liability_reconciliation（13.4）の不一致**:
- `am.period_seq` → 正: `am.period`
- `am.liability_start`/`am.liability_end` → 未定義（正: `am.liability_balance`）
- `im.initial_lease_liability` → 正: `im.liability_amount`

**v_lease_summary（13.5）の不一致**:
- `a.asset_category_cd` / `cat.asset_category_nm` / `im.lease_classification` / `a.lease_start_date` / `a.lease_end_date` / `ac.status` / `s.supplier_nm` / `c.supplier_cd` → 全て不一致

**v_monthly_journal_pending（13.6）の不一致**:
- `am.period_seq` → 正: `am.period`
- `am.journal_generated` → 正: `am.journal_generated_flag`
- `ac.status` → 未定義

**v_lease_expiry_alert（13.7）の不一致**:
- `a.lease_end_date` → lease_assetに未定義（contract_end_dateはlease_contractに定義）
- `c.supplier_cd` / `s.supplier_nm` / `ac.status` → 未定義

**影響**: 全7ビューのうち7ビュー全てがDDL実行時にエラーとなる。ビューは一切利用できない。

**推奨対応**: 全ビューSQLをテーブル定義のカラム名に合わせて全面書き直す。

### C-3. lease_assetのカラム数表記の矛盾（14列と記載だが実際は15カラム）

セクション4.2のヘッダでは「14列」と記載されているが、カラム定義表には15行（#1〜#15）が列挙されている。注記で「14属性」と説明しているが、物理カラム数は15であり混乱を招く。

同様に、lease_initial_measurementのヘッダは「22列」だが、カラム定義表には23行（#1〜#23）がある。

lease_accountingのヘッダは「14列」だが、カラム定義表には15行（#1〜#15）がある。

lease_payment_scheduleのヘッダは「17列」だが、カラム定義表には18行（#1〜#18）がある。

**影響**: analysis_optimal_db.mdの最終推奨テーブル構成（セクション7）のカラム数と実際のカラム定義が一致しない。DDL生成やメンテナンス時に混乱が発生する。

**推奨対応**: カラム数を物理カラム数（create_dt/update_dt含む）に統一する。セクションヘッダ・カラム定義表・付録Cのカラム数サマリーを全て一致させる。

---

## Major（修正推奨）

### M-1. 3パート結合による重複ヘッダ・改訂履歴

文書は元々Part1/Part2/Part3として作成され、1ファイルに結合されている。その結果、以下の重複・断絶が発生している。

- **行808**: Part2の独立ヘッダ（タイトル、ドキュメント番号DB-2026-004-P2、改訂履歴）が残存
- **行1594**: Part3の独立ヘッダ（タイトル、ドキュメント番号DB-2026-003-P3、改訂履歴）が残存
  - なお、Part3のドキュメント番号が `DB-2026-003-P3` となっており、Part1/Part2の `DB-2026-004` と不一致
- **行806**: 「Part1 終了」の区切りコメントが残存

**推奨対応**: Part2/Part3のヘッダ・改訂履歴を削除し、冒頭の改訂履歴に統合する。ドキュメント番号を統一する。

### M-2. セクション番号の非連続

| 範囲 | 内容 | 問題 |
|---|---|---|
| 1.1〜1.4 | 設計方針 | OK |
| 2.1〜2.9 | マスタ層 | OK |
| 3.1〜3.2 | EAV層 | OK |
| 4.1〜4.3 | 契約・資産層 | OK |
| 5.1〜5.4 | 会計・測定層 | OK |
| 6.1 | スケジュール層 | OK |
| 7.1〜7.4 | イベント層 | OK |
| 8.1〜8.2 | 貸手・サブリース層 | OK |
| 9.1〜9.4 | 付帯層 | OK |
| 10.1〜10.4 | 仕訳層 | OK |
| 11.1〜11.2 | 外部連携・開示層 | OK |
| 12.1〜12.2 | システム管理層 | OK |
| 13.1〜13.7 | ビュー定義 | OK |
| 14.1〜14.2 | 設例カバレッジ | OK |
| 15.1〜15.3 | データ移行計画 | OK |
| 付録A〜B（Part2） | テーブル作成順序・関連図・カラム数 | **重複**: 本体の付録A〜Bと内容が異なる |

セクション番号自体は連続しているが、**Part2末尾の付録A/B/C（行1536〜1592）と本体の付録A/B（行3208〜3312）が重複**している。Part2の付録はPart2スコープに限定された内容であり、統合後の文書では混乱を招く。

**推奨対応**: Part2の付録A/B/Cを削除するか、本体の付録に統合する。

### M-3. テーブル層構成の合計数の矛盾

セクション1.2のテーブル層構成では合計37テーブル（+EAV層2テーブル = 合計39）と記載されているが、改訂履歴や全体通じて「36テーブル+7ビュー」と記載されている。

実際のテーブル定義を数えると:
- マスタ層: 9（company, supplier, department, contract_type, gl_account, asset_category, index_master, index_history, borrowing_rate_history）
- EAV層: 2（asset_class_field, asset_attribute）
- 契約・資産層: 3（lease_contract, lease_asset, lease_option）
- 会計・測定層: 4（lease_initial_measurement, lease_accounting, lease_payment_schedule, lease_variable_payment）
- スケジュール層: 1（amortization_schedule）
- イベント層: 4（lease_remeasurement, lease_modification_assessment, lease_transition, sale_leaseback）
- 貸手・サブリース層: 2（lease_lessor, sublease_relationship）
- 付帯層: 4（lease_incentive, restoration_obligation, lease_deposit, dept_allocation）
- 仕訳層: 4（journal_header, journal_detail, journal_template_header, journal_template_line）
- 外部連携・開示層: 2（external_mapping, disclosure_snapshot）
- システム管理層: 2（schema_version, audit_log）

合計: **37テーブル**（audit_logを含む）

しかしセクション1.2の表では各層の合計が37（マスタ11+契約資産3+会計測定4+スケジュール1+イベント4+貸手サブリース2+付帯4+仕訳4+外部連携開示2+システム管理2 = 37）であり、「+EAV層2テーブル = 合計39」は誤り（EAV層はマスタ層11に含まれているのか別枠なのか不明確）。

analysis_optimal_db.md の最終推奨（セクション7）では audit_log を「-」行として別扱いにし36テーブルとしている（schema_version=36番目、audit_log=番号なし）。この扱いがv5本体と齟齬がある。

**推奨対応**: テーブル層構成表の合計を正確に修正し、「36テーブル」「37テーブル」の定義を明確化する。

### M-4. schema_versionテーブルにcreate_dt/update_dtが欠如

設計方針（セクション1.1）で「全テーブルにcreate_dt/update_dt」と明記されているが、schema_version（12.1）にはcreate_dt/update_dtが定義されていない。付録B.2でも「schema_version、audit_logを除く」と例外扱いしているが、設計方針と矛盾する。

audit_logはchanged_atカラムがcreate_dtの役割を果たしており合理的だが、schema_versionにはapplied_dateしかなくupdate_dtに相当するカラムがない。

**推奨対応**: 設計方針の記述を「schema_version、audit_logを除く全テーブル」に修正するか、schema_versionにもcreate_dt/update_dtを追加する。

### M-5. audit_logのトリガー関数でNEW.idを参照しているが、各テーブルのPKカラム名はidではない

fn_audit_trigger()関数内で `NEW.id` / `OLD.id` を参照しているが、実際のテーブルPKはcontract_id, asset_id, journal_id等であり、`id`という名前のカラムは存在しない。このトリガーは全テーブルに適用不可能。

**推奨対応**: record_idの取得ロジックを動的に変更するか（例: TG_ARGVでPKカラム名を渡す）、各テーブル専用のトリガー関数を作成する。

### M-6. lease_accountingにstatusカラムが未定義だがビューで参照

v_ctb_export、v_disclosure_maturity_analysis、v_lease_summary、v_monthly_journal_pending、v_lease_expiry_alertの各ビューで `ac.status` を参照しているが、lease_accountingテーブルの定義にstatusカラムは存在しない。

lease_contractにはstatusカラム（active/terminated/expired）が定義されているが、ビューのJOIN構造では lease_accounting をエイリアス `ac` としており不整合。

**推奨対応**: statusフィールドをlease_contractから取得するようビューSQLを修正するか、lease_accountingにもstatusカラムを追加する。

### M-7. lease_assetにlease_start_date/lease_end_dateが未定義

複数のビュー（v_ctb_export, v_lease_summary, v_lease_expiry_alert）で `a.lease_start_date` / `a.lease_end_date` を参照しているが、lease_assetテーブルにはこれらのカラムは定義されていない。リース開始日/終了日は lease_contract の contract_start_date/contract_end_date として定義されている。

**推奨対応**: ビューSQLで lease_contract 経由で日付を取得するか、lease_assetにリース開始日/終了日を追加する。

### M-8. journal_headerのFK（fk_journal_template）の作成順序問題

journal_headerのDDLで `REFERENCES journal_template_header(template_id)` を参照しているが、DDLの記載順序ではjournal_header（セクション10.1）がjournal_template_header（セクション10.3）より先に定義されている。FK制約はテーブル作成後にALTER TABLEで追加する必要があるが、DDL内にインラインで記述されており、実行順序によってはエラーとなる。

**推奨対応**: journal_headerのDDLからFK制約をインライン定義から外し、ALTER TABLE形式で後付けするコメントを追加する。またはDDL一括実行スクリプト（付録A）にjournal_template_headerをjournal_headerの前に記載する。

---

## Minor（改善推奨）

### m-1. Part1/Part2/Part3間でのカラム定義表のヘッダ表記が不統一

- Part1（マスタ層・EAV層）: `| NOT NULL | DEFAULT |`
- Part2（契約・資産層〜スケジュール層）: `| NOT NULL | DEFAULT |` （ただし値の表記が「-」と空欄が混在）
- Part3（イベント層以降）: `| Null | Default |` （ヘッダの英語表記が変更）

Part3では「NOT NULL」列が「Null」列に変わり、NOT NULLの場合は「NOT NULL」、NULLの場合は「NULL」と記載する形式に変わっている。

**推奨対応**: 全セクションでカラム定義表のヘッダを統一する。

### m-2. DDLスタイルの不統一

- Part1/Part2: `CONSTRAINT pk_xxx PRIMARY KEY (yyy)` 形式と `xxx SERIAL PRIMARY KEY` 形式が混在
- Part2以降: 概ね `xxx SERIAL PRIMARY KEY` のインラインPK形式
- Part1: `CONSTRAINT pk_xxx PRIMARY KEY (yyy)` の明示的PK制約名形式

**推奨対応**: PK制約の定義形式を統一する。制約名の明示を全テーブルで統一することを推奨。

### m-3. lease_contractのsupplier参照が欠如

v4ではctb_lease_integratedにsupplier_cdがあったが、v5のlease_contractにはsupplier（貸主）への直接参照がない。lessor_company_id（→company）はあるが、supplier（取引先マスタ）テーブルへのFKはない。ビューSQL内では `c.supplier_cd` を参照しており、テーブル設計との乖離がある。

**推奨対応**: 貸主の管理をcompanyテーブルで行うのかsupplierテーブルで行うのか方針を明確化し、ビューSQLを整合させる。

### m-4. amortization_scheduleのperiod_seqカラムの不在

v_disclosure_rou_reconciliation、v_disclosure_liability_reconciliation、v_monthly_journal_pendingで `am.period_seq` を参照しているが、amortization_scheduleの定義ではperiod_seqではなく `period`（会計期間: 1-12）として定義されている。period_seqは連番（累計の回数）を想定しているように見えるが、periodは月を表す1-12の値であり、意味が異なる可能性がある。

**推奨対応**: period_seqの意図を明確化し、必要であればamortization_scheduleにperiod_seqカラムを追加する。

### m-5. disclosure_snapshotに4年超5年以内のバケットが欠如

カラム定義では amount_1year / amount_2year / amount_3year / amount_4year / amount_5plus の5バケットだが、analysis_optimal_db.mdでは「1年以内/1-2年/2-3年/3-4年/4-5年/5年超」の6バケットを想定している。amount_4yearは「3年超4年以内」であり、「4年超5年以内」に相当するカラムがない。

**推奨対応**: amount_5year（4年超5年以内）カラムを追加し、6バケット構成にする。

### m-6. v_ctb_exportのカラム数が103カラムに達していない

v_ctb_exportは「旧CTBフラットテーブル（103カラム）との後方互換性を提供する」と説明されているが、実際のSELECT句は約50カラム程度しか定義されていない。EAV PIVOTによる種別固有カラム（re_*, vh_*, oa_*の16カラム）も含まれていない。

**推奨対応**: v_ctb_exportの完全なカラムリストを定義するか、部分的な互換ビューであることを明記する。

### m-7. 日本語の品質は概ね良好だが、一部技術用語の表記揺れあり

- 「勘定科目コード」と「勘定科目コード（FK→gl_account）」は統一されている
- 「償却方法」と「減価償却方法」が混在（depreciation_method の説明）
- セクション見出しの括弧の使い方: 全角括弧「（）」と半角括弧「()」が混在

---

## テーブル完全性チェックリスト

### テーブル（目標: 36テーブル ※analysis_optimal_db.md最終推奨）

| # | テーブル名 | 定義セクション | 定義有無 | カラム数（記載/実物理） | 備考 |
|---|---|---|---|---|---|
| 1 | company | 2.1 | OK | 10/10 | |
| 2 | supplier | 2.2 | OK | 10/10 | |
| 3 | department | 2.3 | OK | 10/10 | |
| 4 | contract_type | 2.4 | OK | 8/8 | |
| 5 | gl_account | 2.5 | OK | 10/10 | |
| 6 | asset_category | 2.6 | OK | 12/12 | |
| 7 | index_master | 2.7 | OK | 10/10 | |
| 8 | index_history | 2.8 | OK | 8/8 | |
| 9 | borrowing_rate_history | 2.9 | OK | 10/10 | |
| 10 | asset_class_field | 3.1 | OK | 18/18 | |
| 11 | asset_attribute | 3.2 | OK | 10/10 | |
| 12 | lease_contract | 4.1 | OK | 17/17 | |
| 13 | lease_asset | 4.2 | OK | **14/15** | **C-3**: create_dt/update_dtで15物理カラム |
| 14 | lease_option | 4.3 | OK | 12/12 | |
| 15 | lease_initial_measurement | 5.1 | OK | **22/23** | **C-3**: create_dt/update_dtで23物理カラム |
| 16 | lease_accounting | 5.2 | OK | **14/15** | **C-3**: create_dt/update_dtで15物理カラム |
| 17 | lease_payment_schedule | 5.3 | OK | **17/18** | **C-3**: create_dt/update_dtで18物理カラム |
| 18 | lease_variable_payment | 5.4 | OK | 10/10 | |
| 19 | amortization_schedule | 6.1 | OK | 20/20 | |
| 20 | lease_remeasurement | 7.1 | OK | 18/18 | |
| 21 | lease_modification_assessment | 7.2 | OK | 10/10 | 新規追加 |
| 22 | lease_transition | 7.3 | OK | 14/14 | |
| 23 | sale_leaseback | 7.4 | OK | 14/14 | |
| 24 | lease_lessor | 8.1 | OK | 12/12 | |
| 25 | sublease_relationship | 8.2 | OK | 11/11 | |
| 26 | lease_incentive | 9.1 | OK | 10/10 | |
| 27 | restoration_obligation | 9.2 | OK | 10/10 | |
| 28 | lease_deposit | 9.3 | OK | 11/11 | 新規追加 |
| 29 | dept_allocation | 9.4 | OK | 10/10 | |
| 30 | journal_header | 10.1 | OK | 14/14 | |
| 31 | journal_detail | 10.2 | OK | 13/13 | |
| 32 | journal_template_header | 10.3 | OK | 9/9 | |
| 33 | journal_template_line | 10.4 | OK | 11/11 | |
| 34 | external_mapping | 11.1 | OK | 12/12 | |
| 35 | disclosure_snapshot | 11.2 | OK | 17/17 | |
| 36 | schema_version | 12.1 | OK | 6/6 | create_dt/update_dt欠如（M-4） |
| (+) | audit_log | 12.2 | OK | 10/10 | 番号なし扱い |

**結果**: 36テーブル + audit_log = 37テーブル全て定義済み

### ビュー（目標: 7ビュー）

| # | ビュー名 | 定義セクション | 定義有無 | 備考 |
|---|---|---|---|---|
| 1 | v_ctb_export | 13.1 | OK | MATERIALIZED VIEW。カラム名不一致多数（C-2） |
| 2 | v_disclosure_maturity_analysis | 13.2 | OK | カラム名不一致あり（C-2） |
| 3 | v_disclosure_rou_reconciliation | 13.3 | OK | カラム名不一致あり（C-2） |
| 4 | v_disclosure_liability_reconciliation | 13.4 | OK | カラム名不一致あり（C-2） |
| 5 | v_lease_summary | 13.5 | OK | カラム名不一致あり（C-2） |
| 6 | v_monthly_journal_pending | 13.6 | OK | カラム名不一致あり（C-2） |
| 7 | v_lease_expiry_alert | 13.7 | OK | カラム名不一致あり（C-2） |

**結果**: 7ビュー全て定義済み（ただし全ビューのSQLにカラム名不一致あり）

---

## 分析レポート（analysis_optimal_db.md）との整合性チェック

| 改善提案 | 反映状況 | 備考 |
|---|---|---|
| lease_accounting: 20→14列に削減 | **反映済** | 6カラム削除の根拠も明記。ただし物理カラムは15（C-3） |
| journal_template: ヘッダ/明細分離 | **反映済** | journal_template_header(9列) + journal_template_line(11列) |
| asset_class_field: UI制御カラム追加 | **反映済** | 8→18列に拡張（ui_control_type, ui_group, ui_width等追加） |
| lease_deposit: 新規追加 | **反映済** | セクション9.3で定義。設例14対応 |
| lease_modification_assessment: 新規追加 | **反映済** | セクション7.2で定義。設例15-1対応 |
| 全テーブルcreate_dt/update_dt追加 | **ほぼ反映済** | schema_versionのみ欠如（M-4） |
| lease_contract: +4カラム | **反映済** | contract_date, auto_renewal, renewal_notice_months, has_cancel_option |
| lease_asset: +2カラム | **反映済** | mgmt_dept_cd, depreciation_method |
| lease_initial_measurement: +3カラム | **反映済** | payment_timing, lease_standalone_price, non_lease_standalone_price |

**結果**: analysis_optimal_db.md の主要改善提案は全て反映されている。

---

## 推奨アクション

### 優先度: 高（Critical対応）

1. **C-1 FK参照先カラム名の修正**: lease_contract, lease_assetの3箇所のFK制約REFERENCES句を修正
2. **C-2 ビューSQLの全面書き直し**: 全7ビューのSQLをテーブル定義のカラム名に合わせて修正。v4のカラム名（旧命名）が混入している原因を特定し排除
3. **C-3 カラム数表記の統一**: 「14列」等の記載をcreate_dt/update_dt含む物理カラム数に統一

### 優先度: 中（Major対応）

4. **M-1 3パート結合の整理**: Part2/Part3の重複ヘッダ・改訂履歴を削除し単一文書化
5. **M-2 付録の統合**: Part2の付録A/B/Cを本体付録に統合
6. **M-3 テーブル合計数の修正**: 1.2の層構成表の合計を修正
7. **M-5 audit_logトリガー関数の修正**: NEW.idをPKカラム名に動的対応させる
8. **M-6〜M-8 その他**: statusカラムの整理、日付カラムの整理、DDL作成順序の明確化

### 優先度: 低（Minor対応）

9. カラム定義表ヘッダの統一、DDLスタイルの統一、技術用語の表記揺れ修正等
