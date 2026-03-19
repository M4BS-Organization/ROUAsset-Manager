# 要件定義書: hen-ido-gson-forms

> Issue #32: 変更・異動・減損処理画面群の移行（f_HEN_SCH/f_HENF/f_HENL/f_IDO系/f_GSON系）

---

## 1. 機能概要

本機能群は、リース契約における変更処理（返済スケジュール変更・保守料変更・変額リース料管理）、物件異動処理（管理部署・費用負担部署の変更）、および減損データ管理（減損損失の登録・照会・Excelインポート）を VB.NET/WinForms に移行するものである。

各フォームのDesignerファイルおよびロジック基盤（`GsonScheduleBuilder`、`CashScheduleBuilder.BuildHengakuSchedule`）は既にコードベースに存在するが、ビジネスロジック（イベントハンドラ・DB操作・再計算処理）が未実装の状態にある。本要件定義はその実装範囲を確定することを目的とする。

---

## 2. ユーザーストーリー

### US-001: 返済スケジュール変更（f_HEN_SCH）

- **As a** リース担当者
- **I want** 物件単位の返済スケジュールを変更できる
- **So that** 契約条件変更後の支払計画を正確に管理できる

#### 受け入れ基準

- [ ] 変更対象の支払日（SHRI_DT）、支払額（KLSRYO）、消費税（KZEI）、消費税率（ZRITU）、〆支払間隔（SSHRI_KN）を編集できる
- [ ] 「呼出元に反映（&R）」ボタン押下時、変更内容が呼び出し元フォームのコンテキストに反映される
- [ ] 「行削除（&D）」ボタン押下時、選択行が削除される
- [ ] 合計行（KLSRYO_SUM / KZEI_SUM / KLSRYO_ZKOMI_SUM）が自動再計算される
- [ ] キャンセルボタン押下時、変更が破棄されフォームが閉じる

---

### US-002: 変額リース料管理（f_HENL）

- **As a** リース担当者
- **I want** 変額リース料の履歴を確認・編集できる
- **So that** 変額スケジュールに基づいた正確な計上計算が行える

#### 受け入れ基準

- [ ] D_HENLテーブルの各行（LINE_ID単位）を一覧表示する
- [ ] 初回支払日（SHRI_DT1）、支払額（KLSRYO）、消費税（KZEI）、税込み額（KLSRYO_ZKOMI）、支払間隔（SHRI_KN）、〆支払間隔（SSHRI_KN）、支払回数（SHRI_CNT）、最終支払日（SHRI_EN_DT）、支払額合計（KLSRYO_GOKEI）、消費税合計（KZEI_GOKEI）、税込み合計（KLSRYO_ZKOMI_GOKEI）を表示する
- [ ] 「スケジュール展開（&T）」ボタン押下時、f_HEN_SCHを子フォームとして開き、選択行のスケジュール明細を展開する
- [ ] 「行削除（&D）」ボタン押下時、選択行をD_HENLから削除する
- [ ] 合計行（SUM列）が全行合算で表示される
- [ ] D_HENLの変更後、CashScheduleBuilder.BuildHengakuScheduleが参照するデータが整合する

---

### US-003: 変額リース料参照（f_REF_D_HENL）

- **As a** リース参照ユーザー
- **I want** 変額リース料履歴を読み取り専用で閲覧できる
- **So that** 変更を加えずにスケジュール内容を確認できる

#### 受け入れ基準

- [ ] f_HENLと同一の項目を表示するが、全フィールドが編集不可（ReadOnly）である
- [ ] 「戻る（&C）」ボタンのみ操作可能である

---

### US-004: 保守料管理（f_HENF）

- **As a** リース担当者
- **I want** 物件に付随する保守料の支払スケジュールを管理できる
- **So that** 保守料の会計処理を正確に行える

#### 受け入れ基準

- [ ] 支払先（F_LCPT1_NM）、費用区分（F_HKMK_NM）、契約先（F_GSHA_NM）、契約番号（KYKBNF）、銀行口座（KOZA_NM）を表示する
- [ ] 初回支払日、支払額、消費税、消費税率、支払間隔、〆支払間隔、支払回数、最終支払日、支払額合計、消費税合計、税込み合計を表示・編集できる
- [ ] 消費税計上区分（HSZEI_KJKBN_ID）の表示と自動設定禁止フラグ（chk_HSZEI_KJKBN_ID_MS_F）を設定できる
- [ ] 「スケジュール展開（&T）」ボタンが機能する
- [ ] 「行削除（&D）」ボタンが機能する

---

### US-005: 物件異動処理（f_IDO）

- **As a** リース担当者
- **I want** 物件の管理部署または費用負担部署を一括変更できる
- **So that** 組織変更時の物件割り当てを効率的に更新できる

#### 受け入れ基準

- [ ] 異動種別（管理部署 / 費用負担部署）をラジオボタンで選択できる
- [ ] 移動元の部署分類（BCAT1〜BCAT5）を5列で表示する
- [ ] 移動先の部署分類（BCAT1〜BCAT5）を5列で入力できる
- [ ] 異動日（IDO_DT）を入力できる
- [ ] 「移動元の照会（&M）」ボタンで現在の部署構成を確認できる
- [ ] 「すべて選択（&A）」/「すべて選択しない（&E）」ボタンで対象物件を一括選択/解除できる
- [ ] 「解除（&K）」ボタンで個別の選択状態を解除できる
- [ ] 「実行（&R）」ボタン押下時、選択された物件のD_KYKMのBCAT情報を一括更新する
- [ ] 異動日が空欄の場合、実行できない（バリデーションエラーを表示する）

---

### US-006: 物件異動明細（f_IDO_SUB）

- **As a** リース担当者
- **I want** 異動対象の物件一覧と選択状態を確認できる
- **So that** 異動処理の対象範囲を把握できる

#### 受け入れ基準

- [ ] 物件No（KYKM_NO）、再リース回数（SAIKAISU）、資産番号1（BUKN_BANGO1）、物件名称（BUKN_NM）、開始日（START_DT）、中途解約日（CKAIYK_DT）、1支払額（KLSRYO）を表示する
- [ ] 異動フラグ（chk_IDO_F）チェックボックスで個別選択ができる
- [ ] 移動件数カウント（COUNT / DCOUNT）が選択状態に応じてリアルタイム更新される

---

### US-007: 減損データ一覧（f_flx_D_GSON）

- **As a** リース担当者
- **I want** 減損データの一覧を検索・閲覧・変更できる
- **So that** 減損損失の計上状況を一元管理できる

#### 受け入れ基準

- [ ] 物件No（KYKM_NO）、計上区分（KJKBN_NM）、減損行No（LINE_ID）、物件名（BUKN_NM）、処理年月（GSON_DT）、処理タイミング（GSON_TMG_NM）、減損損失（GSON_RYO）、減損損失累計額相当額（GSON_RKEI）、管理単位、管理部署、契約区分、支払先、リース契約番号、再リース回数、リース開始日を一覧表示する
- [ ] テキストボックスによる契約番号等の横断検索ができる（部分一致、大文字小文字不問）
- [ ] 「照会（&M）」ボタンで選択行の詳細参照フォーム（f_REF_D_KYKM_CHUUKI_SUB_GSON）を開く
- [ ] 「変更（&U）」ボタンで選択行の減損データを編集できる
- [ ] 「ファイル出力（&F）」ボタンでDataGridViewの内容をCSV/固定長ファイルとして出力できる
- [ ] SecurityChecker.ApplyDataUpdateLimitによりデータ更新権限がないユーザーは変更ボタンを使用できない

---

### US-008: 減損注記サブ（f_KYKM_CHUUKI_SUB_GSON）

- **As a** リース担当者
- **I want** 物件の減損注記情報（年月・処理タイミング・減損損失・累計額）を確認できる
- **So that** 注記計算の基礎データを確認できる

#### 受け入れ基準

- [ ] 処理年月（GSON_DT）、処理タイミング（GSON_TMG）、減損損失（GSON_RYO）、減損損失累計額相当額（GSON_RKEI）、件数（COUNT）を表示する
- [ ] 減損損失合計（GSON_RYO_SUM）を全行合算で表示する

---

### US-009: 減損参照サブ（f_REF_D_KYKM_CHUUKI_SUB_GSON）

- **As a** 参照ユーザー
- **I want** 減損注記情報を読み取り専用で閲覧できる
- **So that** 変更を加えずに確認できる

#### 受け入れ基準

- [ ] f_KYKM_CHUUKI_SUB_GSONと同一の項目を表示するが全フィールドが読み取り専用である

---

### US-010: 減損データExcelインポート（f_IMPORT_GSON_FROM_EXCEL）

- **As a** リース担当者
- **I want** Excelから減損損失データを一括取り込みできる
- **So that** 外部システムからの減損データを効率的に登録できる

#### 受け入れ基準

- [ ] Excelファイルを選択するダイアログが表示される
- [ ] 取り込んだデータがDataGridView（dgv_LIST）にプレビュー表示される（ヘッダは青背景・白文字）
- [ ] 「Excelをワークテーブルに取り込む」ボタン押下時、FileHelperを使用してExcelを読み込みワークテーブルに格納する（現状 `todo ファイル入力` として未実装）
- [ ] 「前回本登録ログ（&Z）」ボタンで前回インポートログ（Form_f_ZENKAI_LOG）を表示できる
- [ ] インポートは27列（インデックス0〜26）の構造に対応する

---

## 3. 機能要件

### FR-001: D_HENLテーブルの読み書き

- 説明: f_HENL / f_HEN_SCH は `d_henl` テーブルに対してSELECT/INSERT/UPDATE/DELETEを行う。CashScheduleBuilder.BuildHengakuSchedule（`CashScheduleBuilder.vb:182`）が同テーブルを参照するため、変更後のデータ整合性を保証する。
- 優先度: 必須

### FR-002: D_GSONテーブルの読み書き

- 説明: f_flx_D_GSON / f_KYKM_CHUUKI_SUB_GSON は `d_gson` テーブルを対象とする。GsonScheduleBuilder.Build（`GsonScheduleBuilder.vb:20`）が同テーブルを参照するため、INSERT/UPDATE/DELETE時は計上ワーク（tw_D_GSON_KEIJO）の整合性を考慮する。
- 優先度: 必須

### FR-003: 異動処理によるD_KYKMのBCAT更新

- 説明: f_IDO の「実行」ボタン処理は、選択された物件のD_KYKMのBCAT1_ID〜BCAT5_ID（管理部署）またはHKBCAT1_ID〜HKBCAT5_ID（費用負担部署）を異動日（IDO_DT）付きで更新する。
- 優先度: 必須

### FR-004: 消費税計算の自動反映

- 説明: f_HENL / f_HENF において、KLSRYO（支払額）またはZRITU（消費税率）が変更された場合、KZEI（消費税）およびKLSRYO_ZKOMI（税込み額）を自動再計算する。計算式: KZEI = KLSRYO × ZRITU / 100（端数処理は切捨て）、KLSRYO_ZKOMI = KLSRYO + KZEI。
- 優先度: 必須

### FR-005: 合計行の自動更新

- 説明: f_HEN_SCH / f_HENL / f_HENF において、データ行の追加・変更・削除のたびに合計行（SUM列およびGOKEI列）をリアルタイムで再計算・表示する。
- 優先度: 必須

### FR-006: セキュリティチェックの適用

- 説明: f_flx_D_GSON および f_flx_D_HENF のフォームロード時に SecurityChecker.ApplyDataUpdateLimit を呼び出し、データ更新権限に応じてボタンの有効/無効を制御する。
- 優先度: 必須

### FR-007: Excelインポートの完全実装

- 説明: f_IMPORT_GSON_FROM_EXCEL の「Excelをワークテーブルに取り込む」処理（現状 `todo ファイル入力`）をFileHelperを使用して実装する。取り込み後は本登録確認画面へ遷移する。
- 優先度: 推奨

### FR-008: 異動一覧フォーム（f_flx_IDOLST）との連携

- 説明: f_IDOLST_JOKEN（既に一部実装済み）で設定した条件に基づき、f_flx_IDOLST に異動履歴を一覧表示する。f_IDO による異動実行後に履歴が反映されることを確認する。
- 優先度: 推奨

---

## 4. 非機能要件

### NFR-001: パフォーマンス

- f_flx_D_GSON の初回表示は3秒以内に完了すること
- 異動処理（f_IDO 実行）は処理対象が1,000物件以下の場合、10秒以内に完了すること
- D_HENL / D_GSON の合計行再計算は操作後500ミリ秒以内に反映されること

### NFR-002: データ整合性

- f_HENL でのレコード削除時、当該 KYKM_ID に対するD_HENLレコードが0件になる場合、呼び出し元（f_KYKM等）に対して警告を表示すること（仮定: 変額スケジュールが空になると計算エラーになる）
- f_IDO の実行はトランザクション内で行い、部分更新が発生しないこと
- D_GSONのINSERT/UPDATE/DELETEはトランザクション内で行い、tw_D_GSON_KEIJO（ワーク）との不整合が発生しないこと

### NFR-003: ユーザビリティ

- エンターキー押下で次のコントロールへフォーカス移動すること（HandleEnterKeyNavigation使用。f_flx_D_HENF:29 に実装例あり）
- 各フォームはCenterParentで表示すること

---

## 5. 前提条件・制約

- VB.NET / .NET Framework 4.7.2 / WinForms で実装すること
- データベースは PostgreSQL を使用し、Npgsqlライブラリを経由してアクセスすること
- 既存のCrudHelperクラスを使用してDB操作を行うこと
- f_HEN_SCH / f_HENL / f_HENF / f_IDO / f_IDO_SUB のDesignerファイルは実装済みであり、変更しないこと
- f_flx_D_GSON の Designer実装（DataGridView列定義・ボタン配置）は完成しており、ロジック実装のみが対象
- 権限チェックにはSecurityCheckerクラスを使用すること
- Excelインポートは既存のFileHelperクラスを使用すること（f_IMPORT_GSON_FROM_EXCEL:31 参照）

---

## 6. スコープ外

- D_KYKHテーブルへの直接変更（契約書ヘッダの変更は本機能群のスコープ外）
- 変更登録後の月次仕訳の自動再計算（月次計上処理は別機能 `pc_月次仕訳計上` のスコープ）
- 異動処理の承認フロー（Access版では単一ユーザー確認のみ。承認ワークフローは実装しない）
- f_IDOを通じたBCAT変更に伴う配賦データ（D_HAIF）の自動更新
- f_IMPORT_GSON_FROM_EXCEL における本登録後の仕訳再計算トリガー
- 顧客固有カスタマイズ（fc_ 系フォーム）

---

## 7. 用語定義

| 用語 | 定義 |
|---|---|
| D_HENL | 変額リース料テーブル。物件(KYKM_ID)ごとにLINE_ID単位のスケジュール行を持つ |
| D_GSON | 減損データテーブル。物件(KYKM_ID)ごとに処理年月(GSON_DT)・処理タイミング(GSON_TMG)・減損損失額(GSON_RYO)・累計額(GSON_RKEI)を記録 |
| KLSRYO | リース料・支払額（税抜き）|
| KZEI | 消費税額 |
| KLSRYO_ZKOMI | 税込み支払額（KLSRYO + KZEI）|
| ZRITU | 消費税率（% 単位）|
| SHRI_DT | 支払日（単発） |
| SHRI_DT1 | 初回支払日 |
| SHRI_KN | 支払間隔（ヶ月）|
| SSHRI_KN | 〆支払間隔（ヶ月）|
| SHRI_CNT | 支払回数 |
| SHRI_EN_DT | 最終支払日 |
| GSON_TMG | 処理タイミング（0=月度末, 1=月度初）|
| GSON_RYO | 減損損失額（当期発生分）|
| GSON_RKEI | 減損損失累計額相当額 |
| LINE_ID | 明細行番号（D_HENLおよびD_GSONの行識別子）|
| BCAT1〜BCAT5 | 物件分類1〜5（部署階層に対応）|
| IDO_DT | 異動日 |
| SAIKAISU | 再リース回数 |
| tw_D_HENL_KEIJO | 変更償却仕訳ワークテーブル |
| tw_D_GSON_KEIJO | 減損仕訳ワークテーブル |

---

## 8. 仮定事項

1. **D_HENLのLINE_ID採番**: 新規行追加時のLINE_IDはMAX(LINE_ID)+1で採番する（仮定: Access版と同様の採番方式）。実装前に採番方式を確認すること。

2. **f_HEN_SCHの単一行 vs. 複数行モード**: Designerファイルの構造上、f_HEN_SCHは単一のスケジュール行（1件）を編集するモーダルダイアログとして機能すると推定される（合計行はサマリ用）。複数行グリッド方式の場合は設計変更が必要。

3. **f_IDOのBCAT変更の即時反映**: 異動処理はD_KYKMを直接更新し、変更履歴テーブルへの記録は行わないと仮定する（Access版のf_IDOにD_HAIF更新ロジックがあった場合は要確認）。

4. **減損データの更新権限**: SecurityChecker.ApplyDataUpdateLimitがデータ更新ボタンの有効/無効を制御することで対応する。行レベルの権限制御は不要と仮定する。

5. **f_IMPORT_GSON_FROM_EXCELの27列構造**: インポートExcelの列定義（物件No、処理年月、減損損失額等の列順）はAccess版のf_減損損失取込に準拠すると仮定する。実装前にテンプレートExcelの列定義を確認すること。

6. **異動処理のBCAT列対応**: f_IDOのラジオボタンが「管理部署」の場合はBCAT1_ID〜BCAT5_ID、「費用負担部署」の場合はHKBCAT1_ID〜HKBCAT5_IDを更新すると仮定する（Access版コードの確認が必要）。
