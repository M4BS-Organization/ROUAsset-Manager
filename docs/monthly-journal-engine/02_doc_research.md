# ドキュメント調査: 月次仕訳計上エンジン

## 1. Access版 VBA モジュール構成

### メインモジュール: pc_月次仕訳計上
- **場所**: `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_月次仕訳計上.txt`
- **規模**: 40,000行超
- **エントリポイント**: `g仕訳計算_仕訳to計上テーブル_計上()`
  - 引数: dte_aKIKAN_FROM, dte_aKIKAN_TO, iaSKYAK_HO_ID, saJoken, fa処理完了残高当月埋_F, iaMEISAI, ia返済方法_新法費用
  - 戻値: Boolean
- **主要サブルーチン**:
  - `m仕訳計算_仕訳to計上TBL_SUB()` - 月別ループの実行部
  - `m仕訳計算_SQLMAKE()` - 抽出SQLの組立
  - `m仕訳計算_配賦補正()` - 配賦単位の端数調整
  - `m仕訳計算_配賦計算()` - 配賦率に基づく金額配分
  - `m仕訳計算_配賦合計加算()` - 配賦合計の累計
- **内部構造体**: `cn_typ_mSch_FUSAI`（債務スケジュール）, `cn_typ_g資産_IF`（計算パラメータ）

### 契約レベル計上: pc_SHRI_KEIJO
- **場所**: `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_SHRI_KEIJO.txt`
- **エントリポイント**: `gKEIJO_Main()`
  - 引数: dte_aKIKAN_FROM, dte_aKIKAN_TO, iaTAISHO(1:リース/2:保守/3:全部), iaMEISAI(1:物件/2:配賦), saSTBL(出力テーブル名), ia返済方法_新法費用
  - 戻値: Boolean
- **主要サブルーチン**:
  - `mKEIJO_Sub_KYKM()` - 物件単位集計（メインループ）
  - `mKEIJO_Sub_HAIF()` - 配賦単位集計
  - `mKEIJO_Sub_HENF()` - 付随費用（保守）処理
  - `mCALC_KEIJOfromSCH()` - スケジュールから計上金額算出
  - `mKEIJO_Sub_SCHtoWK()` - スケジュール→ワークテーブル書込
  - `mKEIJO_Sub_SCHtoWK_ADD()` - 不要行の補完出力
  - `mUPD_CASHSCH_SOKYU()` - 速給処理
  - `mGET_法令区分()` - 旧法/新法判定
  - `mCALL_CHUUKI()` - 注記計算呼出
- **内部構造体**:
  - `cn_typ_mKEIJO` - 計上レコード（総回数、済回数、当期額、税額、支払先ID、科目ID、配賦情報等）
  - `cn_typ_mHAIF` - 配賦情報（配賦率、科目、属性等）
  - `cn_typ_m不要行補完管理` - 不要行補完用

### 共通処理: pc_SHRI_COM
- **場所**: `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_SHRI_COM.txt`
- **主要関数**:
  - `gMOTO_RSETSQL_EDIT_KEIJO()` - 計上用ソースデータSQLの構築
  - `gMOTO_RSETSQL_EDIT()` - 汎用ソースデータSQL構築
  - `gMakeCASH_SCH_T()` - 定額キャッシュスケジュール生成（VB.NETに移植済）
  - `gMakeCASH_SCH_H()` - 変額キャッシュスケジュール生成（VB.NETに移植済）
  - `gCALC_CASH_SOKYU()` - 速給処理
  - `g集計対象外CHK()` - 集計対象外チェック

### 当月処理: pc_SHRI_TOUGETSU
- **場所**: `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_SHRI_TOUGETSU.txt`
- 当月の支払仕訳処理。計上エンジンとは別フローだが参考になる

### 共通型定義: p_PublicVariable
- **場所**: `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\p_PublicVariable.txt`
- 全Enum定義（engSHRI_KTMG, engSHRI_MEISAI, engKJKBN, engKKBN, engHENSAI_KIND, engHOREI等）
- 共通Type定義（cn_typ_gSch_CASH, cn_typ_g資産_IF, cn_typ_g共通設定等）

## 2. VB.NET 移植済みコンポーネント

### KlsryoCalculationEngine（参考パターン）
- 設計パターン: 単一クラスに Execute() エントリポイント、Private メソッドで処理分割
- DB アクセス: CrudHelper.GetDataTable() でパラメータ化クエリ
- 結果: DataTable として返却
- 期間計算: GetMonthEndDate(), AddMonths() ベース

### CashScheduleBuilder（直接再利用）
- BuildTeigakuSchedule() → 定額スケジュール（pc_SHRI_COM.gMakeCASH_SCH_T 移植）
- BuildHengakuSchedule() → 変額スケジュール（pc_SHRI_COM.gMakeCASH_SCH_H 移植）
- CalcNextShriDt() → 次回支払日算出
- GetMonthEndDate() → 月末日算出

### KlsryoTypes（直接再利用）
- RecKbn (1:定額, 2:変額, 3:付随費用)
- ShriKtmg (1:締日ベース, 2:支払日ベース)
- ShriMeisai (1:物件単位, 2:配賦単位)
- Kkbn (1:リース, 2:レンタル, 3:保守)
- Kjkbn (1:費用, 2:資産)
- CashScheduleEntry（支払スケジュール1行）
- KlsryoResult（期間リース料結果）
- HaifInfo（配賦情報）

## 3. 処理フロー詳細

### gKEIJO_Main() の処理フロー
```
1. 期首日/期末日/翌期末日の算出
2. READ ONLY トランザクション開始
3. 不整合データチェック (g集計中不整合データCHK)
4. 転記テーブルダウンロード (TC_HREL → twc_TC_HREL)
5. 計上区分フィルタ条件組立 (資産/費用/両方)
6. 対象区分フィルタ条件組立 (リース/保守/全部)
7. SQL構築 (gMOTO_RSETSQL_EDIT_KEIJO)
8. レコード件数取得
9. レコードセットオープン
10. 出力ワークテーブルクリア
11. メインループ:
    Case 物件単位: mKEIJO_Sub_KYKM()
    Case 配賦単位: mKEIJO_Sub_HAIF()
12. 付随費用処理: mKEIJO_Sub_HENF()
13. READ ONLY トランザクション終了
14. 注記計算呼出: mCALL_CHUUKI()
15. 支払照合チェック
```

### mKEIJO_Sub_KYKM() の処理フロー
```
For Each レコード:
  1. 不要行補完管理初期化
  2. 費用計上で減損ありの場合、減損スケジュール作成
  3. 返済方法/計上タイミングの決定
     - 資産: 物件の返済方法を使用
     - 費用(新法): 画面指定の返済方法を使用
     - 費用(旧法移行外): 物件の返済方法を使用
  4. 集計対象判定（期間重複チェック）
     - 締日ベース: 契約期間 vs 集計期間の重複判定
     - 支払日ベース: 支払日範囲 vs 集計期間の重複判定
  5. 定額スケジュール生成 (gMakeCASH_SCH_T)
  6. 速給処理 (mUPD_CASHSCH_SOKYU)
  7. スケジュール→計上金額算出 (mCALC_KEIJOfromSCH)
  8. 結果をワークテーブルに出力 (mKEIJO_Sub_SCHtoWK)
  9. 不要行の補完出力 (mKEIJO_Sub_SCHtoWK_ADD)
  10. 変額処理 (B_HENL_F=True の場合)
      - 変額スケジュール生成 (gMakeCASH_SCH_H)
      - 同様に計上金額算出→出力
Next
```

### mCALC_KEIJOfromSCH() の核心ロジック
```
入力: スケジュール配列, 期首日, 期末日, 翌期末日, 契約開始日, 契約終了日
出力: KeijoRecord (前期以前/当期/残高 の各金額)

For Each スケジュールエントリ:
  締日ベースの場合:
    期間判定 = 締日(SimeDt)で判定
  支払日ベースの場合:
    期間判定 = 支払日(ShriDt)で判定

  If 判定日 < 期首日 Then
    前期以前に加算（SUMIKAISU_ZEN++, 前払額累計）
  ElseIf 判定日 <= 期末日 Then
    当期に加算（KEIJO_SHRI_CNT++, 当期額累計）
  ElseIf 判定日 <= 翌期末日 Then
    1年内残高に加算
  Else
    1年超残高に加算
  End If
Next
```

## 4. ワークテーブル定義（Access版から導出）

### tw_S_CHUKI_KEIJO（注記計上結果ワーク）
Access版の rslTO への書き込みフィールドから導出:
- 契約情報: KYKH_ID, KYKH_NO, KYKM_ID, KYKM_NO, BUKN_NM, LCPT_NM, SHHO_NM
- 期間情報: START_DT, B_REND_DT, KYAK_DT
- 計上条件: KKBN_ID, KJKBN_ID, LEAKBN_ID, CHU_HNTI_ID
- 計算パラメータ: RCALC_ID, SKYAK_HO_ID, HENSAI_KIND
- 計上結果: SOUKAISU, SUMIKAISU_ZEN, KEIJO_SHRI_CNT, LSRYO_TOTAL, LSRYO_TOKI, ZEI_TOTAL, ZEI_TOKI
- 表示制御: TAISHO_F, KEIJO_F, REC_KBN
- 配賦情報: LINE_ID, HAIFRITU, H_BCAT_ID, RSRVH1_ID, HKMK_ID
- 属性: H_ZOKUSEI1-5, ZRITU, SHHO_ID, SHRI_DT, LCPT_ID
- 前払情報: MAE_ZOU, MAE_GEN, MZEI_ZOU, MZEI_GEN
- 減損情報: GSON_DT
- 計上日: KEIJO_DT, SHORI_DT

### tw_D_HENL_KEIJO（変額仕訳ワーク）
- 変額リース（D_HENL）の計上結果を格納
- tw_S_CHUKI_KEIJO と同等スキーマ

### tw_D_GSON_KEIJO（減損仕訳ワーク）
- 減損（D_GSON）の計上結果を格納
- tw_S_CHUKI_KEIJO のサブセット

## 5. 業務ルール要約

### 法令区分判定
- 施行日(SEKOU_DT)と契約開始日(START_DT)を比較
- 旧法(cngHOREI_OLD): START_DT < SEKOU_DT
- 新法(cngHOREI_NEW): START_DT >= SEKOU_DT

### 返済方法の決定
- 資産計上: 物件の返済方法(HENSAI_KIND)をそのまま使用
- 費用計上(新法): 画面指定の返済方法を使用
- 費用計上(旧法・移行外リース): 物件の返済方法を使用

### 計上タイミング
- 返済方法=リースベース(3)の場合: 締日ベース(KTMG=1)
- 返済方法=定額(1)or均等(2)の場合: 支払日ベース(KTMG=2)

### 速給処理
- SKYU_KJ_F=True の場合、計上開始日(K_KJYO_ST_DT)以前のスケジュールを無効化
