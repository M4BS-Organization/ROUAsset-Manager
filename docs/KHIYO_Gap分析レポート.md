# f_flx_KHIYO（期間費用計上明細表）Gap分析レポート

> **作成日**: 2026-03-16
> **対象Issue**: #7 f_flx_KHIYO 期間計算・グレーアウト条件
> **ブランチ**: claude/khiyo-period-calc/9752
> **分析対象**: Access版 → VB.NET/PostgreSQL 移植の完成度

---

## サマリー

| 判定 | 件数 | 割合 |
|------|------|------|
| 実装済 | 34件 | 74% |
| 部分実装 | 5件 | 11% |
| 未実装 | 6件 | 13% |
| デッドコード | 1件 | 2% |
| **合計** | **46件** | **100%** |

---

## A. 費用行区（7種類）

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-001 | 支払額 (kjkbn_id=1) のSQL条件 | **実装済** | UNION ALL内 sql1。`kykm.kjkbn_id = 1` 条件一致 |
| CHK-002 | 保守料 (b_henf_f) のSQL条件 | **実装済** | UNION ALL内 sql2。d_henfテーブルJOIN、`b_henf_f = True` 条件一致 |
| CHK-003 | 償却費（月割按分）のSQL条件 | **実装済** | UNION ALL内 sql3。`b_syutok / (taiyo_nen * 12) * periodMonths` |
| CHK-004 | 支払利息（月割按分）のSQL条件 | **実装済** | UNION ALL内 sql4。`(b_slsryo - b_knyukn) / lkikan * periodMonths` |
| CHK-005 | 維持管理費用（月割按分）のSQL条件 | **実装済** | UNION ALL内 sql5。`b_ijiknr / lkikan * periodMonths` |
| CHK-006 | 減損損失のSQL条件 | **部分実装** | UNION ALL内 sql6。金額が固定 `0`（Access版では計算ロジックがある可能性あり） |
| CHK-007 | 減損勘定取崩額のSQL条件 | **部分実装** | UNION ALL内 sql7。金額が固定 `0`（同上） |

---

## B. 期間月数計算

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-008 | 契約期間と集計期間の交差計算 | **実装済** | PostgreSQL `AGE()` + `GREATEST`/`LEAST` で正しく再現 |
| CHK-009 | 月割按分（periodMonths変数） | **実装済** | SQL文字列内で構築。償却費/支払利息/維持管理費用に適用 |
| CHK-010 | 期間月数の表示（条件フォーム） | **実装済** | `UtilDate.GetDuration()` で `DateDiff + 1` 計算 |

---

## C. グレーアウト

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-011 | 期間外の行をグレー表示 | **実装済** | `ApplyGrayOut()` で startDt/endDt と DtFrom/DtTo を比較 |
| CHK-012 | 金額0でも期間内なら通常表示 | **実装済** | `keijoAmount <> 0` で対応 |
| CHK-013 | グレーアウトの色設定 | **実装済** | `ForeColor=Gray`, `BackColor=FromArgb(240,240,240)` |

---

## D. 条件入力フォーム (Form_f_KHIYO_JOKEN)

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-014 | 集計期間FROM/TO入力 | **実装済** | DateTimePicker (`yyyy/MM`) |
| CHK-015 | 期間月数の自動計算表示 | **実装済** | `DATE_ValueChanged` で `GetDuration()` 呼び出し |
| CHK-016 | 計上タイミング（〆日/約定支払日）ラジオボタン | **実装済** | UIあり。RecBaseプロパティ設定済（修正-002） |
| CHK-017 | 費用行区ON/OFFチェックボックス（7種） | **実装済** | `chk_REC_KBN_1`〜`7` |
| CHK-018 | 金額符号（マイナス/プラス）ラジオボタン | **部分実装** | ラベル表示のみ。SQL条件に符号フィルタ未反映 |
| CHK-019 | 出力対象未選択時のバリデーション | **実装済** | `cmd_EXECUTE_Click` 内 |
| CHK-020 | 実行確認ダイアログ | **実装済** | `MessageBox.Show` |
| CHK-021 | 日付の順序自動修正 | **実装済** | `SwapIf()` |
| CHK-022 | NextDtTo パラメータ設定 | **実装済** | `txt_DT_TO.Value.AddMonths(1)` 設定済（修正-001） |
| CHK-023 | RecBase（計上タイミング）パラメータ設定 | **実装済** | `radio_SHIME.Checked` による分岐設定済（修正-002） |
| CHK-024 | 前回集計結果ボタン | **実装済** | `cmd_ZENKAI` で実装 |

---

## E. UI構造（Form_f_flx_KHIYO）

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-025 | 閉じるボタン | **実装済** | `cmd_CLOSE_Click` |
| CHK-026 | 再計算ボタン | **部分実装** | `Me.Close()` のみ。条件フォームへの戻り処理は呼び出し元依存 |
| CHK-027 | 照会ボタン | **実装済** | `Form_BuknEntry` / `Form_ContractEntry` 呼び出し |
| CHK-028 | 印刷ボタン | **未実装** | ボタンはDesigner上にあるがClickハンドラ未実装 |
| CHK-029 | ファイル出力ボタン | **実装済** | `Form_f_FlexOutputDLG` へ委譲 |
| CHK-030 | 検索ボタン | **実装済** | `cmd_SEARCH_Click` |
| CHK-031 | 条件ラベル表示 | **実装済** | `GetLabelText()` |
| CHK-032 | ダブルクリックで照会 | **実装済** | `dgv_LIST_CellDoubleClick` |
| CHK-033 | Enterキーナビゲーション | **実装済** | `FormKeyDown` / `HandleEnterKeyNavigation` |

---

## F. データグリッド

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-034 | 基本情報列（物件No, 配No, 計上区分, 契約番号 等） | **実装済** | SQLで取得 |
| CHK-035 | 12ヶ月分月別計上額列 | **未実装** | Designer列は定義済だが、SQLが `期間計上額` 1列のみ返却 |
| CHK-036 | 期間合計列 | **未実装** | Designer列は定義済だがSQLに含まれず |
| CHK-037 | 累計列 | **未実装** | Designer列は定義済だがSQLに含まれず |
| CHK-038 | 行区列 | **実装済** | SQLで `'支払額' AS 行区` 等で返却 |
| CHK-039 | kykm_id / kykh_id の非表示 | **実装済** | `HideColumns()` |
| CHK-040 | 金額フォーマット (#,##0) | **実装済** | `FormatColumn()` で期間計上額に適用 |
| CHK-041 | AutoGenerateColumns = True | **実装済** | `SearchData()` 内。Designer定義列はクリアされ自動生成列に置換 |

---

## G. ファイル出力

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-042 | CSV出力 | **実装済** | `Form_f_FlexOutputDLG` 経由 |
| CHK-043 | Excel出力 | **実装済** | `Form_f_FlexOutputDLG` 経由 |
| CHK-044 | 固定長出力 | **実装済** | `Form_f_FlexOutputDLG` 経由 |
| CHK-045 | CSV区切り文字選択UI | **未実装** | ハードコーディングされた区切り文字で出力 |

---

## H. その他

| ID | チェック項目 | 判定 | 備考 |
|---|---|---|---|
| CHK-046 | AddRecConditions() の活用 | **デッドコード** | 定義あり・呼び出しなし。UNION ALL方式への移行で不要化 |

---

## 修正対応状況

### 今回対応済み（本ブランチで修正）

| 修正ID | 優先度 | 対象 | 内容 | 状態 |
|---|---|---|---|---|
| 修正-001 | 高 | CHK-022 | `NextDtTo` パラメータ設定追加（`txt_DT_TO.Value.AddMonths(1)`） | **修正済** |
| 修正-002 | 高 | CHK-023 | `RecBase` パラメータ設定追加（`radio_SHIME.Checked` による分岐） | **修正済** |

### 次フェーズ送り

| 修正ID | 優先度 | 対象 | 内容 | 理由 |
|---|---|---|---|---|
| 修正-003 | 中 | CHK-018 | 金額符号フィルタをSQLに反映 | Access版仕様の詳細確認が必要 |
| 修正-004 | 中 | CHK-035〜037 | 12ヶ月分月別列のSQL拡張 | 設計レベルの作業が必要。別Issue起票推奨 |
| 修正-005 | 低 | CHK-028 | 印刷ボタンのイベントハンドラ実装 | 印刷機能全体のスコープ外（開発スケジュール参照） |
| 修正-006 | 低 | CHK-046 | デッドコード `AddRecConditions()` の削除 | 影響なし。リファクタリング時に対応 |
| 修正-007 | 低 | CHK-045 | CSV区切り文字選択UIの実装 | FileHelper側の修正と合わせて対応 |

---

## 対象ファイル一覧

| ファイル | 役割 |
|---|---|
| `Form_f_flx_KHIYO.vb` | メイン明細表フォーム（SQL構築・グレーアウト・UI操作） |
| `Form_f_flx_KHIYO.Designer.vb` | Designer（41列定義・ボタン配置） |
| `Form_f_KHIYO_JOKEN.vb` | 条件入力フォーム（パラメータ受け渡し） |
| `Form_f_KHIYO_JOKEN.Designer.vb` | 条件入力Designer |
| `FormHelper.vb` | 拡張メソッド集（16メソッド） |
| `UtilDate.vb` | 日付ユーティリティ（4メソッド） |
| `CrudHelper.vb` | DB操作ヘルパー（12メソッド） |
| `Form_f_FlexOutputDLG.vb` | 出力ダイアログ |
