# Issue #11: 標準仕訳出力_KJ Gap分析レポート

## 概要
- 対象: Form_f_仕訳出力標準_KJ.vb（計上仕訳出力処理本体）
- ブランチ: claude/shiwake-kj-verify/4738
- 分析日: 2026-03-16
- 充足率: 88.3%（68/77項目）

## スコープ
- 対象: Form_f_仕訳出力標準_KJ.vb の出力処理本体
- 対象外: 設定画面（Issue #13）、画面遷移

## チェック項目 & Gap分析結果

| No | カテゴリ | チェック項目 | Access版の仕様 | VB.NET実装状況 | 判定 | 備考 |
|----|----------|------------|---------------|---------------|------|------|
| 1 | 画面・UI | コントロール配置 | txt_対象年月, txt_KEIJO_DT, txt_OUTPUT_FOLDER_NM, cmd_実行, cmd_CANCEL, cmd_設定, cmd_選択 | 全コントロール配置済み | ○ | |
| 2 | 画面・UI | RecordSource | tw_f_仕訳出力標準_KJ でバインド | テキストボックスに直接セット | ○ | WinFormsでは妥当な代替 |
| 3 | 画面・UI | フォームタイトル | "月次仕訳計上フレックス 仕訳出力" | Designer.vbで設定済み | ○ | |
| 4 | イベント | Form_Load - 集計条件存在チェック | tw_S_KEIJO_JOKEN RecordCount=0で終了 | SELECT LIMIT 1 + Rows.Count=0チェック | ○ | |
| 5 | イベント | Form_Load - KIKAN_FROM NULLチェック | IsNull(KIKAN_FROM)なら終了 | IsDBNull チェック | ○ | |
| 6 | イベント | Form_Load - 期間1ヶ月チェック | KIKAN_FROM/TO yyyy/mm比較 | yyyy/MM比較 | ○ | |
| 7 | イベント | Form_Load - 適用日チェック | tmSETTEI.TEKIYO_DT比較 | t_setteiからTEKIYO_DT取得比較 | ○ | |
| 8 | イベント | Form_Load - 配賦単位チェック(MEISAI=2) | MEISAI=2以外ならエラー | CInt <> 2チェック | ○ | |
| 9 | イベント | Form_Load - KJ_FLG_1確認(資産計上) | KJ_FLG_1=FalseでYesNo | CBool=FalseでYesNo | ○ | |
| 10 | イベント | Form_Load - KJ_FLG_2確認(費用借入) | KJ_FLG_2=FalseでYesNo | CBool=FalseでYesNo | ○ | |
| 11 | イベント | Form_Load - ワークテーブル初期化 | AddNew/Edit + 対象年月・計上日セット | DELETE後INSERT/UPDATE | ○ | |
| 12 | イベント | Form_Load - 年度末YMD取得 | g年度末YMDGet(KIKAN_FROM) | GetNendoMatsuYMD()で同等 | ○ | |
| 13 | イベント | Form_Load - 共通設定取得 | gGet共通処理設定項目(tmSETTEI) | t_settei直接参照 | △ | 経路異なるが機能同等 |
| 14 | イベント | Form_BeforeUpdate | fmSAVE=Falseならキャンセル | WinFormsでは不要 | ○ | |
| 15 | イベント | cmd_実行 - 計上日必須チェック | IsNull(txt_KEIJO_DT) | IsNullOrWhiteSpace | ○ | |
| 16 | イベント | cmd_実行 - 出力先必須チェック | IsNull(txt_OUTPUT_FOLDER_NM) | IsNullOrWhiteSpace | ○ | |
| 17 | イベント | cmd_実行 - フォルダ存在チェック | gChkDir() | Directory.Exists() | ○ | |
| 18 | イベント | cmd_実行 - 設定テーブル読込 | gSET_T_SETTEI_to_WKTBL + 初期値セット | LoadSettings() + set_swkkj_defaults() | △ | T_SETTEI展開省略 |
| 19 | イベント | cmd_実行 - 実行確認 | gComMsg "0011W" | MessageBox.Show YesNo | ○ | |
| 20 | イベント | cmd_実行 - 仕訳データ作成 | m仕訳データ作成() | m仕訳データ作成() | ○ | |
| 21 | イベント | cmd_実行 - 0件チェック | DCount = 0 | SELECT COUNT(*) = 0 | ○ | |
| 22 | イベント | cmd_実行 - Excel出力 | gEXCEL出力() | ExportToExcel() | ○ | |
| 23 | イベント | cmd_実行 - レポートプレビュー | DoCmd.OpenReport acViewPreview | 未実装 | × | Accessレポート代替なし |
| 24 | イベント | cmd_実行 - ファイル出力メッセージ | パス表示 | 同等メッセージ | ○ | |
| 25 | イベント | cmd_実行 - 砂時計カーソル | gHourGlass | Cursor.WaitCursor | ○ | |
| 26 | イベント | cmd_CANCEL_Click | DoCmd.Close | Me.Close() | ○ | |
| 27 | イベント | cmd_設定_Click | 設定MAIN画面を開く | 設定KJ画面を直接開く | △ | 遷移先差異 |
| 28 | イベント | cmd_選択_Click | p_api_gGetFolderName() | FolderBrowserDialog | ○ | |
| 29 | イベント | txt_KEIJO_DT | gInt()で整数切捨て | TryParse+ToStringで整数化 | ○ | |
| 30 | 仕訳パターン | SSN1: 科目配列構成 | DR2/CR2 | _tmDR(1)/_tmCR(1) | ○ | |
| 31 | 仕訳パターン | SSN1: 科目No統合 | DR・CR両方統合 | DR側のみ統合 | △ | CR側統合欠落 |
| 32 | 仕訳パターン | SSN1: 金額フィールド | SYUTOK_ZOU, ZEI_KHRI | 同一 | ○ | |
| 33 | 仕訳パターン | SSN1: DC分離出力 | DC_BETU_Fで切替 | IsDcBetu()で切替 | ○ | |
| 34 | 仕訳パターン | SSN2: 利息 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 35 | 仕訳パターン | SSN3: 差額 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 36 | 仕訳パターン | SSN4: 科目配列 | DR5/CR1 | _tmDR(4)/_tmCR(0) | ○ | |
| 37 | 仕訳パターン | SSN4: KRZEI_OUT_F条件 | KRZEI_OUT_F + SZEI_KEIJO_TMG=2 | GetSettingBool + HIYOSHZEI_SHRI | ○ | |
| 38 | 仕訳パターン | SSN4: CR合計計算 | 5金額合計 | amt1-5合計 | ○ | |
| 39 | 仕訳パターン | SSN5: 減損 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 40 | 仕訳パターン | SSN6: 直接法ネッティング | 同科目なら相殺 | GCmp()で比較・相殺 | ○ | |
| 41 | 仕訳パターン | SSN6: KAIYK_OUT_F | =0ならCKAIYK_DT IS NULL条件 | excludeKaiykで同等 | ○ | |
| 42 | 仕訳パターン | SSN6: ORDER BY | kykm_no, saikaisu, line_id | 同一 | ○ | |
| 43 | 仕訳パターン | SSN7: 減額/減 | SSN6同構造+CKAIYK_DT IS NOT NULL | ProcessSSN6or7共通化 | ○ | |
| 44 | 仕訳パターン | SSN8: 減額減 | 3DR/1CR | _tmDR(2)/_tmCR(0) | ○ | |
| 45 | 仕訳パターン | HIYO1: 開始費用 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 46 | 仕訳パターン | HIYO2: 科目配列 | DR4/CR1 | _tmDR(3)/_tmCR(0) | ○ | |
| 47 | 仕訳パターン | HIYO2: KKBN_ID分岐 | 保守→維持費/他→リース料 | KKBN_HOSHU分岐 | ○ | |
| 48 | 仕訳パターン | HIYO2: 消費税タイミング | SZEI_KEIJO_TMG=2分岐 | HIYOSHZEI_SHRI分岐 | ○ | |
| 49 | 仕訳パターン | HIYO2: ORDER BY | kykm_no, kykm_id, kkbn_id, rec_kbn, line_id | 同一 | ○ | |
| 50 | 仕訳パターン | HIYO3: 返済 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 51 | 仕訳パターン | HIYO4: 減損 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 52 | 仕訳パターン | HIYO5: 減損特別損失 | 1DR/1CR | ProcessSimplePattern | ○ | |
| 53 | 仕訳パターン | HIYO6: 終了費 | 2DR/1CR + 科目No統合 | _tmDR(1)/_tmCR(0) | ○ | |
| 54 | 共通ロジック | mGET科目 - SKMK1-10 | SKMK_SUM1-10_CD/NM | Select Case完全対応 | ○ | |
| 55 | 共通ロジック | mGET科目 - HMK1-10 | KMK_CD1-10/NM1-10 | Select Case完全対応 | ○ | |
| 56 | 共通ロジック | mGET科目 - CONST | 固定値セット | Case "CONST"対応 | ○ | |
| 57 | 共通ロジック | mGET科目 - 科目名補完 | KMKNM_HOKAN | GetSettingBool対応 | ○ | |
| 58 | 共通ロジック | m科目No統合 - Null比較 | gCmpでNull同士=True | NzStr比較+IsNoInputフィルタ | △ | Null比較挙動差 |
| 59 | 共通ロジック | DC分離出力(別行) | DC_BETU_F=True | OutputDR_Betu/OutputCR_Betu | ○ | |
| 60 | 共通ロジック | DC同行出力 | DC_BETU_F=False | OutputDCR_Same | ○ | |
| 61 | 共通ロジック | 仕訳SEQNo管理 | KYKM_ID変更時+1 | CheckKykmIdChange | ○ | |
| 62 | 共通ロジック | 共通項目SET | 21フィールド | BuildCommonParams | ○ | |
| 63 | 共通ロジック | NzDec/NzStr | Nz(val,0) | 実装済み | ○ | |
| 64 | 共通ロジック | GCmp | Null考慮比較 | 実装済み | ○ | |
| 65 | 共通ロジック | IsNoInput | Null/空文字判定 | 実装済み | ○ | |
| 66 | DB操作 | データソースクエリ | qsel_df_flx_KEIJO WHERE | GetKeijoData() | ○ | |
| 67 | DB操作 | 仕訳DATAクリア | DELETE | DELETE WHERE TRUE | ○ | |
| 68 | DB操作 | 仕訳DATA INSERT | AddNew+Update | InsertSwkRow() | ○ | パラメタライズド |
| 69 | DB操作 | 設定テーブル読込 | OpenRecordset | _crud.GetDataTable() | ○ | |
| 70 | 出力 | Excelファイル名 | YYYYMM_月次仕訳計上フレックス | +タイムスタンプ付き | △ | ファイル名パターン差 |
| 71 | 出力 | Excelデータソース | qsel_s仕訳出力標準_KJ | 同一 | ○ | |
| 72 | 出力 | Excel COM操作 | gEXCEL出力共通関数 | Interop.Excel直接操作 | ○ | |
| 73 | 出力 | レポートプレビュー | r_仕訳出力標準_KJ | 未実装 | × | |
| 74 | エラー | パターンエラー | On Error GoTo L_ERR | Try-Catch | ○ | |
| 75 | エラー | Form_Loadエラー | gComMsg + Cancel=True | Try-Catch + Me.Close() | ○ | |
| 76 | エラー | cmd_実行エラー | gHourGlass False | Finally Cursor.Default | ○ | |
| 77 | 共通ロジック | 共通クラス経由 | pc_仕訳出力標準_COM | 直接実装 | △ | 機能同等 |

## サマリー

| 判定 | 件数 | 割合 |
|------|------|------|
| ○ 完全実装 | 68 | 88.3% |
| △ 一部/代替 | 7 | 9.1% |
| × 未実装 | 2 | 2.6% |
| **合計** | **77** | **100%** |

**充足率: 88.3%（○のみ）/ 97.4%（○+△）**

## 未実装項目の詳細

### × 未実装（2件）
1. **No.23/73: レポートプレビュー** — Access版は `DoCmd.OpenReport "r_仕訳出力標準_KJ"` でプレビュー表示。VB.NETではAccessレポートの直接代替がないため未実装。Excel出力は完了しているため実害は限定的。

### △ 要注意（7件）
1. **No.13: 共通設定取得経路** — gGet共通処理設定項目を経由せずt_settei直接参照。機能同等。
2. **No.18: 設定テーブル展開** — T_SETTEI→ワークテーブル展開プロセス省略。ストアド依存。
3. **No.27: 設定画面遷移先** — _設定_MAINではなく_設定_KJを直接開く。
4. **No.31: SSN1 CR側科目No統合欠落** — DR側のみ統合、CR側の統合が漏れている。**要修正**
5. **No.58: m科目No統合 Null比較差異** — gCmpはNull同士をTrueとするが、VB.NET版はIsNoInputでNullペアを除外。**要検証**
6. **No.70: Excelファイル名パターン差** — タイムスタンプ追加による差異。運用上は問題なし。
7. **No.77: 共通クラス不使用** — 直接実装で代替。機能同等。

## 推奨アクション
1. **No.31修正**: SSN1のCR側にもm科目No統合を追加（低リスク・数行の修正）
2. **No.58検証**: m科目No統合のNull比較挙動をテストデータで確認
3. **No.23/73**: レポートプレビューは将来的にRDLC等で対応検討（現時点では不要と判断可）
