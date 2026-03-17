# 要件定義書: monthly-journal-engine

## 1. 機能概要

Access版 pc_月次仕訳計上（40,000行超）および pc_SHRI_KEIJO を VB.NET に忠実移植する。
ユーザーが指定した会計期間・対象区分に基づき、契約データからリース料計算を行い、
ワークテーブル（tw_s_chuki_keijo 等）に仕訳計上データを書き込む月次バッチ処理エンジンである。

---

## 2. ユーザーストーリー

### US-001: 計上条件を指定して月次仕訳計上を実行する
- **As a** 経理担当者
- **I want** 期間FROM/TO・対象区分・明細単位・返済方法を指定してエンジンを起動したい
- **So that** 対象期間の仕訳計上データがワークテーブルに生成される

#### 受け入れ基準
- [ ] KeijoJoken（期間FROM、期間TO、対象区分、明細単位、計上タイミング）を引数として受け取れる
- [ ] 期間FROM の月初日が期首日、期間TO の月末日が期末日として正しく計算される
- [ ] 処理完了後に tw_s_chuki_keijo へレコードが書き込まれる
- [ ] エラー発生時は例外をスローし、ワークテーブルへの書き込みはロールバックされる

---

### US-002: 物件単位（KYKM）で仕訳集計する
- **As a** 月次仕訳計上エンジン
- **I want** d_kykh + d_kykm の結合データを物件ごとに集計したい
- **So that** 物件単位の定額・変額・付随費用レコードが tw_s_chuki_keijo に出力される

#### 受け入れ基準
- [ ] ShriMeisai.Kykm 指定時に d_kykh/d_kykm 結合データを取得する
- [ ] 各物件行に対して定額スケジュール（BuildTeigakuSchedule）を生成・集計できる
- [ ] b_henl_f=true の物件に対して変額スケジュール（BuildHengakuSchedule）を生成・集計できる
- [ ] taisho=2 or 3 の場合に付随費用（d_henf）を処理する
- [ ] 計算結果が KlsryoCalculationEngine.Execute の DataTable と同等の内容を持つ

---

### US-003: 配賦単位（HAIF）で仕訳集計する
- **As a** 月次仕訳計上エンジン
- **I want** d_haif の配賦率を適用して物件を複数部署に按分したい
- **So that** 配賦単位の仕訳レコードが tw_s_chuki_keijo に出力される

#### 受け入れ基準
- [ ] ShriMeisai.Haif 指定時に d_haif を JOIN したデータを取得する
- [ ] 同一 kykm_id の複数配賦行に対して haifritu で金額を按分する（Math.Floor で切り捨て）
- [ ] 最終配賦行は残額（合計 - 累積配賦額）を設定する
- [ ] 付随費用（d_henf）についても配賦行ごとに按分する

---

### US-004: ワークテーブル（tw_s_chuki_keijo）にデータを書き込む
- **As a** 月次仕訳計上エンジン
- **I want** 計算結果を tw_s_chuki_keijo テーブルに INSERT したい
- **So that** 後続の仕訳出力処理がワークテーブルを参照できる

#### 受け入れ基準
- [ ] Execute 実行前に tw_s_chuki_keijo の既存データ（同一処理条件）を DELETE する
- [ ] CrudHelper.BeginTransaction / Commit / Rollback を使ってトランザクション管理する
- [ ] 書き込み件数を戻り値として返す（Integer）
- [ ] tw_s_chuki_keijo スキーマが DDL に存在しない場合は DDL を追加定義する（仮定：後述）

---

### US-005: 変額仕訳ワークテーブル（tw_d_henl_keijo）に書き込む
- **As a** 月次仕訳計上エンジン
- **I want** 変額分の仕訳データを tw_d_henl_keijo に書き込みたい
- **So that** 変額仕訳が通常仕訳と分離して管理される

#### 受け入れ基準
- [ ] RecKbn.Hengaku のレコードが tw_d_henl_keijo に INSERT される
- [ ] tw_s_chuki_keijo と同一トランザクション内で処理される

---

### US-006: 減損仕訳ワークテーブル（tw_d_gson_keijo）に書き込む
- **As a** 月次仕訳計上エンジン
- **I want** d_gson テーブルの減損データを tw_d_gson_keijo に書き込みたい
- **So that** 減損仕訳が計上処理と一括して生成される

#### 受け入れ基準
- [ ] d_gson テーブルから対象期間の減損レコードを取得できる
- [ ] tw_d_gson_keijo への INSERT が tw_s_chuki_keijo と同一トランザクション内で行われる

---

### US-007: 計算結果の審査（検証）を実施する
- **As a** 月次仕訳計上エンジン
- **I want** 計算結果の整合性チェックを実行したい
- **So that** 計算エラーや仕訳不整合を事前に検出できる

#### 受け入れ基準
- [ ] 各物件の当期額合計と月別内訳（G01-G12）の合計が一致することを確認する
- [ ] 検証エラー発生時は Exception をスローし、エラー内容（kykm_id、差額）を含むメッセージを設定する
- [ ] 審査処理は WriteToWorkTable 前に実行する（書き込み前バリデーション）

---

## 3. 機能要件

### FR-001: KeijoJokenクラス定義
- 説明: 月次仕訳計上処理の入力パラメータを保持するクラス。KlsryoCalculationEngine.Execute の引数を参考に設計する。
- フィールド:
  - KeijoFrom As Date（計上期間FROM）
  - KeijoTo As Date（計上期間TO）
  - Taisho As Integer（対象区分: 1=リース料, 2=保守料, 3=全部）
  - Ktmg As ShriKtmg（計上タイミング: 締日ベース/支払日ベース）
  - Meisai As ShriMeisai（明細単位: 物件単位/配賦単位）
- 優先度: 必須

### FR-002: MonthlyJournalEngineクラス設計
- 説明: KlsryoCalculationEngine と同一クラス設計パターンを採用する。
  - `Private _crud As New CrudHelper()` でDB操作
  - `Public Function Execute(joken As KeijoJoken) As Integer` がメインエントリポイント
  - 内部処理は Private Method に分割（GoSub 相当）
- 優先度: 必須

### FR-003: ソースデータ取得
- 説明: KlsryoCalculationEngine.GetSourceData と同等のSQL構築ロジック。
  - d_kykh + d_kykm JOIN（物件単位時）
  - d_kykh + d_kykm + d_haif JOIN（配賦単位時）
  - k_seigou_f = true でフィルタリング
  - taisho による kkbn_id フィルタ（1=保守以外, 2=保守のみ, 3=全部）
- 優先度: 必須

### FR-004: 集計対象判定
- 説明: KlsryoCalculationEngine.IsTargetRecord と同等ロジック。
  - start_dt / b_rend_dt による期間重複チェック
  - ShriKtmg による締日/支払日ベースの分岐
- 優先度: 必須

### FR-005: ワークテーブルへの書き込み
- 説明: CrudHelper.BeginTransaction を使ったトランザクション内で以下3テーブルに書き込む:
  - `tw_s_chuki_keijo`（主テーブル: 定額・付随費用の仕訳行）
  - `tw_d_henl_keijo`（変額仕訳行）
  - `tw_d_gson_keijo`（減損仕訳行）
- 処理前に同一条件（keijoFrom/To）の既存データを DELETE する
- 優先度: 必須

### FR-006: 定額レコード区分（RecKbn.Teigaku）の処理
- 説明: b_klsryo/b_kzei/b_mlsryo/b_mzei のいずれかが非ゼロかつ shri_cnt > 0 の物件を対象とする
- CashScheduleBuilder.BuildTeigakuSchedule でスケジュール生成
- KlsryoCalculationEngine.CalcKlsryoFromSchedule と同等のコア集計を実施
- 優先度: 必須

### FR-007: 変額レコード区分（RecKbn.Hengaku）の処理
- 説明: b_henl_f = true の物件を対象とする
- CashScheduleBuilder.BuildHengakuSchedule でスケジュール生成
- 優先度: 必須

### FR-008: 付随費用レコード区分（RecKbn.Fuzui）の処理
- 説明: taisho = 2 or 3 かつ d_kykm.b_henf_f = true の物件を対象とする
- d_henf テーブルから付随費用レコードを取得
- CashScheduleBuilder.BuildCommonSchedule でスケジュール生成
- 優先度: 必須

### FR-009: 法令区分判定
- 説明: KlsryoCalculationEngine の法令判定と同一ロジック。
  - t_settei テーブルから SEKOU_DT を取得（デフォルト: 2008-04-01）
  - kyak_dt（なければ start_dt）と SEKOU_DT を比較し「新法」/「旧法」を設定
- 優先度: 必須

### FR-010: マスタ名称解決
- 説明: KlsryoCalculationEngine.GetNameFromMaster と同等のヘルパー関数。
  - c_kkbn, c_kjkbn, c_leakbn, m_lcpt, m_bcat, m_hkmk からの名称取得
  - ID が NULL の場合は DBNull.Value を返す
- 優先度: 必須

---

## 4. 非機能要件

### NFR-001: Access版との計算結果一致性
- 同一入力データに対して Access版 pc_月次仕訳計上 と同一の出力を生成すること
- 金額の端数処理は Math.Floor（切り捨て）で統一する（Access版と同じ）
- 検証方法: 同一テストデータを両版で実行し、全フィールドを比較（Issue #14 E2Eフロー確認で実施）

### NFR-002: 既存パターンへの準拠
- KlsryoCalculationEngine.vb と同一クラス設計パターンを採用する
  - `Private _crud As New CrudHelper()` によるDB操作
  - `IDisposable` は実装しない（KlsryoCalculationEngine に準じる）
  - 内部ヘルパーは同一のシグネチャ（GetDbl, GetNameFromMaster 等）

### NFR-003: トランザクション管理
- ワークテーブル書き込みは1トランザクション内で完結すること
- 例外発生時は必ず Rollback を呼び出すこと
- CrudHelper.BeginTransaction / Commit / Rollback を使用し、直接 NpgsqlTransaction を扱わないこと

### NFR-004: パフォーマンス
- 標準的なデータ量（1,000物件以下）で Execute 完了まで 30秒以内であること
- 仮定: 現状の Access版と同等の処理時間を基準とする

### NFR-005: エラーハンドリング
- DB接続エラー・SQL実行エラーは Exception をキャッチせずに呼び出し元に伝播させること（CrudHelper が詳細メッセージを付加する）
- 計算エラー（NFR-001 検証失敗等）は InvalidOperationException をスローする

---

## 5. 前提条件・制約

- PostgreSQL（Npgsql経由）を使用する。Access MDB への接続は行わない。
- 既存の CrudHelper / DbConnectionManager / CashScheduleBuilder / KlsryoCalculationEngine を再利用する。コードの重複は避ける。
- .NET Framework 4.7.2（既存プロジェクトに準拠）。
- WinForms から呼び出されるが、エンジン自体は UI 非依存の DataAccess プロジェクトに配置する。
- KlsryoTypes.vb の列挙型（RecKbn, ShriKtmg, ShriMeisai, Kkbn, Kjkbn）をそのまま使用する。

---

## 6. スコープ外

- 仕訳出力（tc_swk_def_com への書き込み）はスコープ外。出力は別 Issue（#11-#13）で対応する。
- WinForms の条件入力画面（f_CHUKI_JOKEN に相当するフォーム）の実装はスコープ外。
- 消費税の科目マッピング（t_szei_kmk 参照）はスコープ外。
- バックアップ・リストア機能はスコープ外。
- 並列実行・マルチスレッド対応はスコープ外。

---

## 7. 用語定義

| 用語 | 定義 |
|---|---|
| KYKM | 物件（d_kykm テーブル。契約ヘッダ d_kykh に紐づく） |
| HAIF | 配賦（d_haif テーブル。1物件を複数部署に按分する） |
| HENF | 付随費用（d_henf テーブル。物件に付随する費用） |
| GSON | 減損（d_gson テーブル。減損損失の記録） |
| 期首日 | 期間FROMの月の1日 |
| 期末日 | 期間TOの月の末日（CashScheduleBuilder.GetMonthEndDate で算出） |
| 計上タイミング (Ktmg) | 締日ベース（締日で期間判定）または支払日ベース（支払日で期間判定） |
| 明細単位 (Meisai) | 物件単位（KYKM）または配賦単位（HAIF） |
| 行区分 (RecKbn) | 定額 / 変額 / 付随費用の3種 |
| 計上区分 (Kjkbn) | 費用計上 または 資産計上 |
| tw_s_chuki_keijo | 月次仕訳計上メインワークテーブル（現 DDL に未定義） |
| tw_d_henl_keijo | 変額仕訳ワークテーブル（現 DDL に未定義） |
| tw_d_gson_keijo | 減損仕訳ワークテーブル（現 DDL に未定義） |
| SEKOU_DT | リース会計新法の施行日（t_settei テーブルに保存、デフォルト 2008-04-01） |

---

## 8. 仮定事項

**仮定1: ワークテーブル（tw_s_chuki_keijo 等）のスキーマ**
- 現在の `sql/001_ddl.sql` には tw_s_chuki_keijo / tw_d_henl_keijo / tw_d_gson_keijo が存在しない。
- これらは Access版の一時テーブルに相当すると推定する。
- 実装前に Access版の当該テーブル定義を確認し、DDL に追加が必要。
- **要確認**: 実装者は Access版 pc_月次仕訳計上 内の「tw_s_chuki_keijo 作成」箇所を参照すること。

**仮定2: KeijoJokenクラスの分離**
- KeijoJoken は KlsryoTypes.vb に追加するか、新規ファイル MonthlyJournalTypes.vb に定義するか、どちらでも実装可能。
- KlsryoTypes.vb への追加を推奨（既存型と同一ファイルで管理）。
- **要確認**: チームリードに命名規則の確認を求める。

**仮定3: CalcKlsryoFromSchedule の再利用方針**
- KlsryoCalculationEngine.CalcKlsryoFromSchedule は Private であるため直接呼び出せない。
- MonthlyJournalEngine はこのロジックを以下のいずれかで参照する:
  - (A) KlsryoCalculationEngine を Friend/Public に変更して呼び出す
  - (B) 共通処理を LeaseM4BS.DataAccess 内の Static ヘルパーに抽出する
  - (C) MonthlyJournalEngine 内に同一ロジックを複製する（Access版の GoSub 相当）
- **推奨**: (A) KlsryoCalculationEngine の CalcKlsryoFromSchedule を Friend に変更して再利用する。
- **要確認**: 実装担当者の判断に委ねる。

**仮定4: 減損仕訳（tw_d_gson_keijo）の処理詳細**
- d_gson テーブルの構造は判明しているが（gson_dt, gson_tmg, gson_ryo 等）、月次仕訳計上での具体的な処理ロジックは Access版を参照要。
- 本要件では「d_gson から対象期間データを取得し tw_d_gson_keijo に書き込む」として定義する。
- **要確認**: Access版 pc_月次仕訳計上の減損処理ブロックを確認すること。
