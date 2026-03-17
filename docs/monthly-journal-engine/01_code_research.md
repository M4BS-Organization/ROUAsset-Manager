# コードベース調査資料: monthly-journal-engine

## 1. プロジェクト概要

- **フレームワーク・言語**: VB.NET / WinForms / .NET Framework
- **DB**: PostgreSQL (Npgsql ドライバ)
- **移行元**: Access VBA (Jet DB)

### ディレクトリ構成（関連部分）

```
c:\kobayashi_LeaseM4BS\
├── LeaseM4BS\
│   └── LeaseM4BS.DataAccess\
│       ├── KlsryoCalculationEngine.vb   ← 取引分類計算エンジン（既存・移植済み）
│       ├── CashScheduleBuilder.vb       ← キャッシュスケジュール生成（既存・移植済み）
│       ├── KlsryoTypes.vb               ← 型定義 Enum/Class
│       └── CrudHelper.vb                ← PostgreSQL CRUD ヘルパー
└── LeaseM4BS.TestWinForms\
    └── LeaseM4BS.TestWinForms\
        ├── Form_f_KEIJO_JOKEN.vb        ← 仕訳計上条件フォーム（スタブ）
        ├── Form_f_flx_TOUGETSU.vb       ← 当月仕訳一覧フォーム（スタブ）
        └── Form_f_flx_KEIJO.vb          ← 取引分類フレックスフォーム（スタブ）
```

---

## 2. アーキテクチャ概要

- **アーキテクチャパターン**: 手続き型 + DataTable 中心の DataAccess 層
- **レイヤー構成**:
  - `DataAccess` (`.dll`) → 計算ロジック / DB アクセス
  - `TestWinForms` (`.exe`) → UI フォーム（DataAccess.dll を参照）
- **状態管理**: なし（フォームごとの局所変数）
- **エラーハンドリング**: Try/Catch で Exception ラップ、CrudHelper 内でエラーメッセージ生成

---

## 3. 関連する既存コード

| ファイルパス | 役割 | 関連度 |
|---|---|---|
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb` | 取引分類計算エンジン（Access版 pc_SHRI_KLSRYO 移植済み） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/CashScheduleBuilder.vb` | キャッシュスケジュール生成（Access版 pc_SHRI_COM 移植済み） | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/KlsryoTypes.vb` | Enum/Class 型定義 | 高 |
| `LeaseM4BS/LeaseM4BS.DataAccess/CrudHelper.vb` | PostgreSQL CRUD ヘルパー | 高 |
| `LeaseM4BS.TestWinForms/Form_f_KEIJO_JOKEN.vb` | 月次仕訳計上条件入力フォーム（スタブ） | 高 |
| `LeaseM4BS.TestWinForms/Form_f_flx_TOUGETSU.vb` | 当月仕訳一覧フォーム（スタブ） | 高 |
| `LeaseM4BS.TestWinForms/Form_f_flx_KEIJO.vb` | 取引分類フレックスフォーム（参考実装） | 中 |
| `sql/001_ddl.sql` | PostgreSQL テーブル定義 | 高 |

---

## 4. 既存VB.NET実装の詳細分析

### 4-1. KlsryoCalculationEngine.vb（`LeaseM4BS/LeaseM4BS.DataAccess/KlsryoCalculationEngine.vb`）

Access版 `pc_SHRI_KLSRYO` の移植済み実装。月次仕訳計上エンジン（`pc_SHRI_KEIJO`）との関係：
- KLSRYO = 取引分類（期間集計）、KEIJO = 仕訳計上（当月単発）
- 両者は同じ `CashScheduleBuilder` を使い、同じスケジュールから集計期間を変えて値を算出する

#### メソッドシグネチャ一覧

```vb
' メイン (Access版 gKLSRYO_Main)
Public Function Execute(
    dtFrom As Date, dtTo As Date,
    taisho As Integer,        ' 1:リース料 2:保守料 3:全部
    ktmg As ShriKtmg,         ' 1:締日ベース 2:支払日ベース
    meisai As ShriMeisai      ' 1:物件単位 2:配賦単位
) As DataTable

' ソースデータ取得 (Access版 gMOTO_RSETSQL_EDIT)
Private Function GetSourceData(kishuDt, kimatDt, ktmg, meisai, taisho) As DataTable

' 集計対象判定 (Access版 mCHK_集計対象)
Private Function IsTargetRecord(kishuDt, kimatDt, ktmg, row) As Boolean

' 物件単位集計 (Access版 mKLSRYO_Sub_KYKM)
Private Sub ProcessKykm(sourceDt, resultDt, kishuDt, kimatDt, ynKimatDt(), getudoFrom(), getudoTo(), gCnt, ktmg, sekouDt)

' 配賦単位集計 (Access版 mKLSRYO_Sub_HAIF)
Private Sub ProcessHaif(sourceDt, resultDt, kishuDt, kimatDt, ynKimatDt(), getudoFrom(), getudoTo(), gCnt, ktmg, sekouDt)

' 配賦率適用スケジュールコピー
Private Function ApplyHaifritu(schedule As List(Of CashScheduleEntry), haifritu As Double) As List(Of CashScheduleEntry)

' 付随費用処理 (Access版 mKLSRYO_Sub_HENF)
Private Sub ProcessHenf(resultDt, kishuDt, kimatDt, ynKimatDt(), getudoFrom(), getudoTo(), gCnt, ktmg, meisai, sekouDt)

' コア: スケジュールから期間集計 (Access版 mCALC_KLSRYOfromSCH)
Private Function CalcKlsryoFromSchedule(
    ktmg, kishuDt, kimatDt, gCnt, ynKimatDt(), getudoFrom(), getudoTo(),
    vaStartDt, vaRendDt, jenchoF As Boolean,
    schedule As List(Of CashScheduleEntry)
) As KlsryoResult

' 結果DataTable作成
Private Function CreateResultTable(gCnt As Integer) As DataTable

' 結果行追加 (Access版 mKLSRYO_SUB_OUTREC_SET)
Private Sub AddResultRow(resultDt, sourceRow, klsryo As KlsryoResult, recKbn, lcptId, haifInfo As HaifInfo, sekouDt)
Private Sub AddHenfResultRow(resultDt, henfRow, klsryo, haifInfo, sekouDt)

' ヘルパー
Private Function GetSekouDt() As Date
Private Function GetNameFromMaster(sql, id) As Object
Private Shared Function GetDbl(row, colName) As Double
```

#### 結果DataTableのカラム構成

```
kykm_id, kykh_id, 物件No, 再回, 配No, 契区, 行区, 計上区分, 法令区分, 取引区分,
リース区分, 契約番号, 支払先, 物件名, 管理部署, 費用負担部署, 費用区分,
開始日, 終了日, 中途解約日, 請求月, 回数済/総,
現金購入価額_物件, 総支払額, 前期末残高, 当期額,
G01〜G12（月別内訳）,
期末残高, 内1年内, 内2年内, 内3年内, 内4年内, 内5年内, 5年超,
期中増加
```

---

### 4-2. CashScheduleBuilder.vb（`LeaseM4BS/LeaseM4BS.DataAccess/CashScheduleBuilder.vb`）

Access版 `pc_SHRI_COM` の移植済み実装。

#### メソッドシグネチャ一覧

```vb
' 定額キャッシュスケジュール生成 (Access版 gMakeCASH_SCH_T)
Public Shared Function BuildTeigakuSchedule(
    ktmg As ShriKtmg,
    kishuDt As Date, kimatDt As Date, jenchoLastDt As Date,
    jenchoF As Boolean,
    shriKn As Integer, shriCnt As Object,
    sshriKnM, sshriKn1, sshriKn2, sshriKn3 As Integer,
    shhoMId, shho1Id, shho2Id, shho3Id As Object,
    maeDt As Object, shriDt1 As Date, shriDt2 As Object, shriDt3 As Integer,
    zritu As Double,
    klsryo, kzei, mlsryo, mzei As Double,
    ckaiykEsdtT As Object
) As List(Of CashScheduleEntry)

' 変額キャッシュスケジュール生成 (Access版 gMakeCASH_SCH_H)
' D_HENL テーブルから読み取り
Public Shared Function BuildHengakuSchedule(
    crud As CrudHelper, kykmId As Double, ckaiykEsdtH As Object
) As List(Of CashScheduleEntry)

' 汎用スケジュール生成 (Access版 gMakeCASH_SCH_COM)
' 付随費用の単一レコード用
Public Shared Function BuildCommonSchedule(
    ktmg As ShriKtmg,
    shriKn, shriCnt, sshriKn As Integer,
    shhoId As Object, shriDt1 As Date,
    zritu, klsryo, kzei As Double,
    ckaiykEsdt As Object
) As List(Of CashScheduleEntry)

' 次回支払日計算 (Access版 gNEXT_SHRI_DT_CALC_B)
Public Shared Function CalcNextShriDt(baseYY, baseMM, monthOffset, dayOfMonth As Integer) As Date

' 締日計算 (Access版 gSIME_DT_CALC_B)  月末締め前提
Public Shared Function CalcSimeDtB(shriDtYY, shriDtMM, shriDtDD, sshriKn As Integer) As Date

' 月末日取得 (Access版 g末日YMDGet)
Public Shared Function GetMonthEndDate(dt As Date) As Date
```

---

### 4-3. KlsryoTypes.vb（`LeaseM4BS/LeaseM4BS.DataAccess/KlsryoTypes.vb`）

#### Enum 定義

```vb
Enum RecKbn         ' 行区分 (Access版 engSHRI_REC_KBN)
    Teigaku = 1     ' 定額
    Hengaku = 2     ' 変額
    Fuzui = 3       ' 付随費用

Enum ShriKtmg       ' 計上タイミング (Access版 engSHRI_KTMG)
    SimeDtBase = 1  ' 締日ベース
    ShriDtBase = 2  ' 支払日ベース

Enum ShriMeisai     ' 明細単位 (Access版 engSHRI_MEISAI)
    Kykm = 1        ' 物件単位
    Haif = 2        ' 配賦単位

Enum Kkbn           ' 契約区分 (Access版 engKKBN)
    Lease = 1       ' リース
    Rental = 2      ' レンタル
    Hoshu = 3       ' 保守

Enum Kjkbn          ' 計上区分 (Access版 engKJKBN)
    Hiyo = 1        ' 費用
    Sisan = 2       ' 資産
```

#### Class 定義

```vb
Class CashScheduleEntry    ' (Access版 cn_typ_gSch_CASH)
    ShriDt As Date         ' 支払日
    SimeDt As Date         ' 締日
    Lsryo As Double        ' 支払額(税抜)
    Zritu As Double        ' 消費税率
    Zei As Double          ' 税額
    MaeF As Boolean        ' 前払フラグ
    CkaiykF As Boolean     ' 中途解約フラグ
    ShhoId As Object       ' 支払方法ID
    SshriKn As Integer     ' 締支払間隔
    LsryoHsum As Double    ' 配賦用累計(税抜)
    ZeiHsum As Double      ' 配賦用累計(税額)

Class KlsryoResult         ' (Access版 cn_typ_mKLSRYO)
    Soukaisu As Object     ' 総回数(Null可)
    Sumikaisu As Object    ' 済回数(Null可)
    LsryoTotal As Double   ' 税抜総額
    LsryoZzan As Object    ' 前期末残高(Null可)
    LsryoToki As Double    ' 当期額
    LsryoTokig As Object() ' 月別内訳(12ヶ月、要素Null可)
    LsryoZan As Object     ' 期末残高(Null可)
    LsryoZan1Nai〜5Cho As Double  ' 翌期以降年度別残高
    LsryoZou As Object     ' 期中増加(Null可)
    ZeiTotal/ZeiZzan/ZeiToki/ZeiTokig/ZeiZan... (税額版同構造)
    TaishoF As Boolean     ' 一覧表示対象フラグ
    RecKbn As RecKbn       ' 行区分

Class HaifInfo             ' (Access版 cn_typ_mHAIF)
    LineId As Integer      ' 配賦行ID
    Haifritu As Double     ' 配賦率
    HkmkId As Object       ' 費用科目ID
    HBcatId As Object      ' 費用負担部署ID
    Rsrvh1Id As Object     ' 予備ID(配賦)
    HZokusei1〜5 As Object ' 属性値1〜5
```

---

### 4-4. CrudHelper.vb（`LeaseM4BS/LeaseM4BS.DataAccess/CrudHelper.vb`）

#### 主要メソッドシグネチャ

```vb
' コンストラクタ
Public Sub New()
Public Sub New(connectionString As String)
Public Sub New(connectionManager As DbConnectionManager)

' SELECT → DataTable
Public Function GetDataTable(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As DataTable

' INSERT/UPDATE/DELETE
Public Function ExecuteNonQuery(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As Integer

' スカラー取得 (ジェネリック)
Public Function ExecuteScalar(Of T)(sql As String, Optional parameters As List(Of NpgsqlParameter) = Nothing) As T

' NULL安全型変換
Public Function SafeConvert(Of T)(value As Object, Optional defaultValue As T = Nothing) As T

' テーブル INSERT (カラム→値Dictionary)
Public Function Insert(tableName As String, columnValues As Dictionary(Of String, Object)) As Integer

' テーブル UPDATE
Public Function Update(tableName As String, columnValues As Dictionary(Of String, Object), whereClause As String, ...) As Integer

' テーブル DELETE
Public Function Delete(tableName As String, whereClause As String, ...) As Integer

' レコード存在確認
Public Function Exists(tableName As String, whereClause As String, ...) As Boolean

' トランザクション制御
Public Sub BeginTransaction()
Public Sub Commit()
Public Sub Rollback()
Public ReadOnly Property IsInTransaction As Boolean
```

---

## 5. Access VBAの処理フロー分析

### 5-1. pc_SHRI_KEIJO (月次仕訳計上エンジン)

Access版の主関数: `gKEIJO_Main()`

#### 関数シグネチャ（Shift-JIS文字化けあり）

```vb
Public Function gKEIJO_Main(
    dte_aKIKAN_FROM As Date,     ' 集計期間FROM
    dte_aKIKAN_TO As Date,       ' 集計期間TO
    iaTAISHO As Integer,         ' 集計対象 1:リース料 2:保守料 3:全部
    iaMEISAI As engSHRI_MEISAI,  ' 明細 1:物件単位 2:配賦単位
    saSTBL As String,            ' 集計結果保存ワークテーブル名
    ia返戻方法_新旧対応 As Integer  ' 新旧リースの返戻方法 1:定額 2:変動利率 3:均等
) As Boolean
```

#### 処理フロー

```
gKEIJO_Main()
├── 期首日/期末日算出 (dte_mKISHU_DT, dte_mKIMAT_DT)
│   └── dte_mY1KIMAT_DT (翌期末 = 1年後月末)
├── gSVBeginTransREADONLY (読み取りトランザクション開始)
├── omSHRI_COM.g集計前処理確認CHK() (業務チェック)
├── g転記_SVtoWK("TC_HREL", "twc_TC_HREL")  (関連テーブルダウンロード)
├── WHERE条件構築 (slJoken: 契約区分・計上区分フィルタ)
│   └── bgKJ_FLG_SISAN/bgKJ_FLG_HIYO (資産/費用フラグ)
├── omSHRI_COM.gMOTO_RSETSQL_EDIT_KEIJO() (ソースSQL生成)
├── レコード件数取得 → プログレス初期化
├── ワークテーブルクリア (DELETE * FROM saSTBL)
├── [分岐] iaMEISAI
│   ├── Case 1: mKEIJO_Sub_KYKM(rslFROM, llRECCNT, rslTO) 物件単位
│   └── Case 2: mKEIJO_Sub_HAIF(rslFROM, llRECCNT, rslTO) 配賦単位
├── [条件] iaTAISHO=2 or 3: mKEIJO_Sub_HENF(rslTO) 付随費用
├── gSVRollback (読み取りトランザクション終了)
└── [後処理] mCALL_CHUUKI() (中期計算) + gCHK_仕訳FLX_の支払と合計()
```

#### cn_typ_mKEIJO 構造体（仕訳計上結果型）

Access版の仕訳計上専用結果型。KlsryoResult と異なり、追加フィールドあり:

```
SOUKAISU          総回数
SUMIKAISU_ZEN     済回数・前期以前
KEIJO_SHRI_CNT    計上仕訳回数
LSRYO_TOTAL       税抜支払総額
LSRYO_TOKI        当期額
ZEI_TOTAL/ZEI_TOKI
TAISHO_F          一覧表示対象フラグ
LCPT_ID           支払先ID
HKMK_ID           費用科目ID
SHRI_DT           支払日
ZRITU             消費税率
SHHO_ID           支払方法
LINE_ID           配賦行ID
HAIFRITU          配賦率
H_BCAT_ID         費用負担部署ID
RSRVH1_ID         予備ID(配賦)
H_ZOKUSEI1〜5     属性値
GSON_DT           減損日
REC_KBN           レコード区分(1:定額, 2:変額, 3:付随費用)
KEIJO_DT          計上日
KEIJO_F           計上表示対象フラグ
HENSAI_KIND       返戻方法 1:定額 2:変動利率 3:均等
MAE_ZOU           前払・増加
MAE_GEN           前払・減少
MZEI_ZOU          前払(消費税)・増加
MZEI_GEN          前払(消費税)・減少
```

**KlsryoResult との主な差分**:
- KEIJO では月別内訳(G01〜G12)・翌期年度別残高(1〜5年超)・期末残高・前期末残高は**不要**
- 代わりに `KEIJO_DT`(計上日)・`KEIJO_F`(計上フラグ)・`HENSAI_KIND`(返戻方法)・`GSON_DT`(減損日)・`MAE_ZOU/GEN`(前払増減) が追加

#### mKEIJO_Sub_KYKM フロー詳細（物件単位処理）

```
mKEIJO_Sub_KYKM(rslFROM, laRecCnt, rslTO)
│
└── FOR 各レコード (rslFROM)
    ├── [B_GSON_F=true && KJKBN=費用] lm保守系_SCH_CNT = gMake減損_SCH()
    ├── 計上区分/返戻方法判定 → ilKTMG (締日/支払日ベース) 設定
    ├── 集計対象チェック (期間重複判定4条件 A/B/C/D)
    │
    ├── [定額処理]
    │   ├── B_KLSRYO/B_KZEI/B_MLSRYO/B_MZEI != 0 かつ SHRI_CNT > 0
    │   ├── omSHRI_COM.gMakeCASH_SCH_T() → tlSCH_T[]
    │   ├── [SKYU_KJ_F=true] mUPD_CASHSCH_SOKYU() (遡及補正)
    │   ├── mCALC_KEIJOfromSCH() → tlKEIJO (計上結果算出)
    │   ├── tlKEIJO に LCPT_ID/LINE_ID等セット
    │   ├── mKEIJO_Sub_SCHtoWK()  (ワークテーブルへ出力)
    │   └── mKEIJO_Sub_SCHtoWK_ADD()  (不払い行等の補完行出力)
    │
    └── [変額処理]
        ├── B_HENL_F=true
        ├── omSHRI_COM.gMakeCASH_SCH_H() → tlSCH_H[]
        ├── [SKYU_KJ_F=true] mUPD_CASHSCH_SOKYU()
        ├── mCALC_KEIJOfromSCH() → tlKEIJO
        └── mKEIJO_Sub_SCHtoWK() + mKEIJO_Sub_SCHtoWK_ADD()
```

#### mCALC_KEIJOfromSCH（仕訳計上コア計算）

KlsryoCalculationEngine の `CalcKlsryoFromSchedule` と対応するが、出力が異なる:

```
mCALC_KEIJOfromSCH(ilKTMG, dte_mKISHU_DT, dte_mKIMAT_DT, dte_mY1KIMAT_DT,
    start_dt, b_rend_dt, jencho_f, llSCH_CNT, tlSCH[], tlKEIJO, ilBASE_SHRI_CNT)

→ tlKEIJO に設定する値:
  - LSRYO_TOTAL     : 総支払額(スケジュール全回合計)
  - LSRYO_TOKI      : 当月計上額(比較基準日が当期内のエントリ合計)
  - ZEI_TOTAL/TOKI  : 同 税額版
  - SOUKAISU        : 総回数(前払除く)
  - SUMIKAISU_ZEN   : 前期以前済回数
  - KEIJO_SHRI_CNT  : 当月計上回数
  - SHRI_DT         : 当月支払日(当月内最初のエントリ)
  - TAISHO_F        : 一覧表示フラグ
  - KEIJO_F         : 計上フラグ(KEIJO_DT が非NULL)
```

#### mKEIJO_Sub_SCHtoWK（ワークテーブル書き込み）

```
→ ワークテーブル tw_S_KEIJO への INSERT
  スケジュール各エントリを1行として出力:
  - KYKM_ID, KYKH_ID, LINE_ID(配賦行)
  - SHRI_DT (支払日/締日)
  - LSRYO, ZEI, ZRITU
  - SHHO_ID (支払方法)
  - REC_KBN (定額/変額/付随費用)
  - MAE_F (前払フラグ)
  - CKAIYK_F (中途解約フラグ)
  - HENSAI_KIND (返戻方法)
  - KEIJO_F (計上済フラグ → 既に計上されていれば 0 セット)
  - KEIJO_DT (計上日 = 支払日)
```

---

### 5-2. pc_月次仕訳計上（月次仕訳計上メインエンジン）

主関数: `g月次計算_集計to集計TBL_本番()`

**KlsryoとKEIJOの違い**:
- `KLSRYO` (取引分類): 期間FROM〜TOで複数月分を集計した DataTable を返す
- `KEIJO` (月次仕訳計上): 1ヶ月単位でスケジュール各回を個別行としてワークテーブルに書き込む

#### 関数シグネチャ

```vb
Public Function g月次計算_集計to集計TBL_本番(
    dte_aKIKAN_FROM As Date,     ' 集計開始日
    dte_aKIKAN_TO As Date,       ' 集計終了日
    iaSKYAK_HO_ID As engSKYAK_HO, ' 新旧リース移転前テナントの利率計算方法
    saJoken As String,           ' ユーザー指定抽出条件データフィルタ
    fa残高翌期繰越処理_F As Boolean, ' 残高翌期繰越処理の残高を当期に繰越するフラグ
    iaMEISAI As engSHRI_MEISAI,
    ia返戻方法_新旧対応 As Integer
) As Boolean
```

#### 処理フロー

```
g月次計算_集計to集計TBL_本番()
├── 期首日/期末日算出
├── gSVBeginTransREADONLY
├── ワークテーブルクリア
│   ├── DELETE tw_S_CHUKI_KEIJO  (中途解約仕訳集計)
│   ├── DELETE tw_D_HENL_KEIJO   (変額リース計上)
│   └── DELETE tw_D_GSON_KEIJO   (減損計上)
├── m月次計算_集計to集計TBL_SUB() ← コア処理
│   ├── m月次計算_SQLMAKE()
│   ├── DO 各レコードループ
│   │   ├── m集計項目を集計TBL に反映()
│   │   ├── m月次計算()
│   │   │   ├── m月次計算_SUB_損益関連()
│   │   │   │   ├── mCalc減価償却()
│   │   │   │   └── mMake減価_SCH()
│   │   │   ├── mMakeFUSAI_SCH()
│   │   │   └── gMake支払_SCH()
│   │   └── m計算結果を集計TBL に反映()
│   └── m繰越処理()
├── マスタダウンロード
└── m繰越処理()
```

#### ワークテーブル（Access版）

- `tw_S_KEIJO`: 仕訳計上結果スプールテーブル（件数 × スケジュール回数行）
- `tw_S_CHUKI_KEIJO`: 中途解約仕訳用
- `tw_D_HENL_KEIJO`: 変額リース計上用
- `tw_D_GSON_KEIJO`: 減損計上用
- `tw_S_TOUGETSU`: 当月仕訳計上結果

**注意**: 現在のVB.NET版 DDL には `tw_*` ワークテーブルは存在しない（要新設）

---

### 5-3. pc_SHRI_COM（共通SQL生成）

```vb
' ソースデータSQLを生成 (KLSRYO/TOUGETSU共通)
Public Sub gMOTO_RSETSQL_EDIT(
    dte_aKISHU_DT, dte_aKIMAT_DT As Date,
    iaKTMG As engSHRI_KTMG,
    iaMEISAI As engSHRI_MEISAI,
    saJOKEN_OTH As String,
    saSQL As String,          ' OUT: SQL文
    saSQLCNT As String        ' OUT: 件数取得用SQL
)

' KEIJO用 (上記と列構成が若干異なる)
Public Sub gMOTO_RSETSQL_EDIT_KEIJO(
    iaMEISAI, slJoken, slSQL, slSQLCNT
)
```

→ VB.NET版では `KlsryoCalculationEngine.GetSourceData()` が同等処理を実装済み。KEIJO用は「計上区分フィルタ (bgKJ_FLG_SISAN/bgKJ_FLG_HIYO)」が追加される。

---

### 5-4. pc_SHRI_TOUGETSU（当月仕訳処理）

```vb
' 当月処理メイン
Public Function gTOUGETSU_Main(
    dte_aKIKAN_FROM As Date,
    dte_aKIKAN_TO As Date,
    iaTAISHO As Integer,       ' 1:リース料 2:保守料 3:全部
    iaKTMG As engSHRI_KTMG,    ' 計上タイミング
    iaMEISAI As engSHRI_MEISAI,
    saSTBL As String,          ' ワークテーブル名
    faRSOK As Boolean          ' 元本・利息計算フラグ
) As Boolean
```

- KLSRYO (取引分類) が「複数月集計 → DataTable」の実装に対し、TOUGETSU は「当月1回分 → ワークテーブルINSERT」の実装
- 処理サブルーチンは `mTOUGETSU_Sub_KYKM`, `mTOUGETSU_Sub_HAIF`, `mTOUGETSU_Sub_HENF` の3系統

---

### 5-5. p_PublicVariable（グローバル変数・Enum定義）

主要 Enum:

```vb
Enum engSHRI_KTMG    ' 計上タイミング
    cngSHRI_KTMG_SIME_DT = 1  ' 締日ベース
    cngSHRI_KTMG_SHRI_DT = 2  ' 支払日ベース
    cngSHRI_KTMG_SHRI_DT_3 = 3 ' 管理支払日ベース(TOUGETSU専用)

Enum engSHRI_MEISAI  ' 明細単位
    cngSHRI_MEISAI_KYKM = 1   ' 物件単位
    cngSHRI_MEISAI_HAIF = 2   ' 配賦単位

Enum engHENSAI_KIND  ' 返戻方法
    cngHENSAI_KIND_定額 = 1
    cngHENSAI_KIND_変動利率 = 2
    cngHENSAI_KIND_均等 = 3

Enum engKKBN  ' 契約区分
    cngKKBN_HOSHU = 3  ' 保守

Enum engKJKBN  ' 計上区分
    cngKJKBN_HIYO = 1   ' 費用
    cngKJKBN_SISAN = 2  ' 資産
```

---

## 6. DBスキーマ分析（主要テーブル）

### d_kykh（契約ヘッダ）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:341`

| カラム | 型 | 備考 |
|---|---|---|
| kykh_id | integer PK | 契約ID |
| kykh_no | double precision | 契約No |
| kkbn_id | smallint | 契約区分(c_kkbn FK) |
| lcpt_id | integer | 支払先ID(m_lcpt FK) |
| kykbnl | varchar(30) | 契約番号(ローカル) |
| kyak_dt | timestamp | 契約日 |
| start_dt | timestamp | 開始日 |
| end_dt | timestamp | 終了日 |
| kjkbn_id | smallint | 計上区分(c_kjkbn FK) |
| shri_kn | smallint | 支払間隔 |
| sshri_kn_m/1/2/3 | smallint | 締支払間隔(前払/第1/2/3回以降) |
| shri_cnt | smallint | 支払回数 |
| shri_dt1 | timestamp | 第1回支払日 |
| shri_dt2 | timestamp | 第2回支払日 |
| shri_dt3 | smallint | 第3回以降支払日(日) |
| mkaisu | smallint | 総回数 |
| mae_dt | timestamp | 前払日 |
| jencho_f | boolean | 自動更新フラグ |
| k_seigou_f | boolean | 整合フラグ(集計対象) |
| b_klsryo | double precision | 月額リース料(税抜) |
| b_kzei | double precision | 月額消費税 |
| b_mlsryo | double precision | 前払リース料 |
| b_mzei | double precision | 前払消費税 |
| zritu | double precision | 消費税率 |
| shho_m/1/2/3_id | integer | 支払方法ID(前払/第1/2/3回) |
| k_slsryo | double precision | 総支払額 |
| k_knyukn | double precision | 現金購入価額 |
| ckaiyk_esdt_t | timestamp | 中途解約有効終日(定額) |
| ckaiyk_esdt_h | timestamp | 中途解約有効終日(変額) |
| kj_shri_cnt | smallint | 計上済支払回数 |
| skyu_kj_f | smallint | 遡及計上フラグ |
| k_kjyo_st_dt | timestamp | 計上開始日(遡及用) |
| hensai_kind | smallint | 返戻方法(d_kykmにも) |

### d_kykm（物件明細）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:440`

| カラム | 型 | 備考 |
|---|---|---|
| kykm_id | integer PK | 物件明細ID |
| kykh_id | integer FK | 契約ヘッダID |
| kykm_no | double precision | 物件No |
| bukn_nm | varchar(100) | 物件名 |
| b_bcat_id | integer | 管理部署ID(m_bcat FK) |
| b_klsryo | double precision | 物件月額リース料(税抜) |
| b_kzei | double precision | 物件消費税 |
| b_mlsryo | double precision | 物件前払リース料 |
| b_mzei | double precision | 物件前払消費税 |
| b_henl_f | boolean | 変額リースフラグ |
| b_henf_f | boolean | 付随費用フラグ |
| b_rend_dt | timestamp | 実際終了日(延長後) |
| b_smdt_fst_sum / b_smdt_lst_sum | timestamp | 締日ベース集計期間(最初/最後) |
| b_shdt_fst_sum / b_shdt_lst_sum | timestamp | 支払日ベース集計期間(最初/最後) |
| ckaiyk_dt | timestamp | 中途解約日 |
| ckaiyk_esdt_t/h | timestamp | 中途解約有効終日 |
| kjkbn_id | smallint | 計上区分(物件レベル) |
| leakbn_id | smallint | リース区分 |
| hensai_kind | smallint | 返戻方法 |
| b_gson_f | boolean | 減損フラグ |
| saikaisu | smallint | 再リース回数 |

### d_haif（配賦）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:243`

| カラム | 型 | 備考 |
|---|---|---|
| kykm_id | integer PK | 物件明細ID |
| line_id | smallint PK | 配賦行ID |
| haifritu | double precision | 配賦率 |
| hkmk_id | integer | 費用科目ID |
| h_bcat_id | integer | 費用負担部署ID |
| rsrvh1_id | integer | 予備ID(配賦) |
| h_klsryo/h_mlsryo/h_kzei/h_mzei | double precision | 配賦後金額 |
| h_zokusei1〜5 | varchar(100) | 属性値1〜5 |

### d_henl（変額リース）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:311`

| カラム | 型 | 備考 |
|---|---|---|
| kykm_id | integer PK | 物件明細ID |
| line_id | smallint PK | 変額行ID |
| shri_kn | smallint | 支払間隔 |
| sshri_kn | smallint | 締支払間隔 |
| shri_cnt | smallint | 支払回数 |
| shri_dt1 | timestamp | 第1回支払日 |
| klsryo | double precision | 月額リース料(税抜) |
| zritu | double precision | 消費税率 |
| kzei | double precision | 消費税 |
| shho_id | integer | 支払方法ID |

### d_henf（付随費用）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:278`

| カラム | 型 | 備考 |
|---|---|---|
| kykm_id | integer PK | 物件明細ID |
| line_id | smallint PK | 付随費用行ID |
| shri_kn | smallint | 支払間隔 |
| sshri_kn | smallint | 締支払間隔 |
| shri_cnt | smallint | 支払回数 |
| shri_dt1 | timestamp | 第1回支払日 |
| klsryo | double precision | 費用額(税抜) |
| kzei | double precision | 消費税 |
| shho_id | integer | 支払方法ID |
| start_dt / end_dt | timestamp | 有効期間 |

### d_gson（減損）— `c:\kobayashi_LeaseM4BS\sql\001_ddl.sql:218`

| カラム | 型 | 備考 |
|---|---|---|
| kykm_id | integer PK | 物件明細ID |
| line_id | smallint PK | 減損行ID |
| gson_dt | timestamp | 減損日 |
| gson_tmg | smallint | 減損タイミング |
| gson_ryo | double precision | 減損額 |
| gson_nm | varchar(40) | 減損名称 |

### tw_* ワークテーブル（未存在 → 要新設）

現在のDDLには `tw_S_KEIJO`, `tw_S_TOUGETSU`, `tw_S_CHUKI_KEIJO`, `tw_D_HENL_KEIJO`, `tw_D_GSON_KEIJO` は定義されていない。月次仕訳計上エンジン実装には新設が必要。

---

## 7. 既存フォームの状態

### Form_f_KEIJO_JOKEN.vb（`LeaseM4BS.TestWinForms/Form_f_KEIJO_JOKEN.vb`）

月次仕訳計上の条件入力フォーム。**スタブ実装**。

- 集計期間FROM/TO の DatePicker (`txt_DATE_FROM`, `txt_DATE_TO`)
- 期間月数表示 (`txt_DURATION`)
- 明細区分ラジオボタン (`radio_BUKN`:物件単位, もう一方:配賦単位)
- 計上区分チェックボックス (`chk_KEIJO`:資産, `chk_SHORI`:費用)
- 設定コンボ (`cmb_SETTEI`: c_settei_idfld.settei_id=19)
- [実行]ボタン → `Form_f_flx_TOUGETSU` を表示するのみ（計算エンジン未接続）
- **未実装**: `iaTAISHO`(対象区分)・`iaKTMG`(計上タイミング)・実際の計算エンジン呼び出し

### Form_f_flx_TOUGETSU.vb（`LeaseM4BS.TestWinForms/Form_f_flx_TOUGETSU.vb`）

当月仕訳一覧フォーム。**スタブ実装**。

- `DataGridView` (`dgv_LIST`) に直接SQLクエリ結果を表示
- BuildSql() は単純な d_kykm JOIN クエリ（スケジュール計算なし）
- 多数の TODO コメントあり（行区・法令区分・請求月・回数済/総 等が未実装）
- [再計算]ボタン → `Form_f_KEIJO_JOKEN` を再表示するのみ
- **未実装**: `KlsryoCalculationEngine` または `KeijoCalculationEngine` の呼び出し

### Form_f_flx_KEIJO.vb（`LeaseM4BS.TestWinForms/Form_f_flx_KEIJO.vb`）

取引分類一覧フォーム（KLSRYO）。**Form_f_flx_TOUGETSU と類似のスタブ**だが、より多くのカラムが実装済み（法令区分・前月末残高・当月計上額等）。TOUGETSU実装の参考になる。

---

## 8. 再利用可能なコンポーネント一覧

| コンポーネント | 場所 | 再利用方法 |
|---|---|---|
| `KlsryoCalculationEngine` | `DataAccess/KlsryoCalculationEngine.vb` | `CalcKlsryoFromSchedule()` を KEIJO用にラップ可能 |
| `CashScheduleBuilder` | `DataAccess/CashScheduleBuilder.vb` | そのまま再利用（定額/変額/付随費用スケジュール生成） |
| `CrudHelper` | `DataAccess/CrudHelper.vb` | DB アクセス全般 |
| `KlsryoTypes` (Enum) | `DataAccess/KlsryoTypes.vb` | ShriKtmg, ShriMeisai, Kkbn, Kjkbn, RecKbn, HaifInfo |
| `Form_f_flx_KEIJO.vb` | TestWinForms | UI パターンの参考 (ApplyGridStyle, BuildSql 構造) |

---

## 9. 技術的制約・注意事項

### KlsryoEngine と KeijoEngine の関係

| 項目 | KLSRYO (取引分類) | KEIJO (月次仕訳計上) |
|---|---|---|
| 集計期間 | 複数ヶ月（FROM〜TO） | 1ヶ月 |
| 出力形式 | DataTable（UI表示用） | ワークテーブルINSERT（仕訳作成用） |
| 月別内訳 | G01〜G12 あり | なし |
| 翌期年度別残高 | 1〜5年超 あり | なし |
| 追加フィールド | なし | KEIJO_DT, KEIJO_F, HENSAI_KIND, MAE_ZOU/GEN |
| 遡及補正 | なし | mUPD_CASHSCH_SOKYU あり |
| 減損処理 | なし | mMake減損_SCH あり |
| 中期計算 | なし | mCALL_CHUUKI あり |

### ワークテーブル戦略の選択

Access版は Access MDB 内のワークテーブルに書き込む。VB.NET版での実装選択肢:
1. **PostgreSQL ワークテーブル方式**: tw_S_KEIJO 等を新規 DDL に追加
2. **インメモリ DataTable 方式**: DB に書かず DataTable を返す（KlsryoEngine と同パターン）

現在の KlsryoEngine は方式2（DataTable 返却）を採用しており、一貫性から同方式が推奨。

### 返戻方法 (HENSAI_KIND) の扱い

- SISAN (資産計上) の場合: `d_kykm.hensai_kind` の値を使用
- HIYO (費用計上) の場合: 引数 `ia返戻方法_新旧対応` (= VB.NET の `ia返戻方法` パラメータ) を使用
- HIYO で LEAKBN = 移転外ファイナンスリース かつ 旧法の場合: `d_kykm.hensai_kind` を使用

### 法令区分判定

```
参照日 = COALESCE(kyak_dt, start_dt)
参照日 >= SEKOU_DT (t_settei より取得、デフォルト 2008/04/01) → "新法"
参照日 < SEKOU_DT → "旧法"
```

→ VB.NET版 `GetSekouDt()` で取得済み。

---

## 10. 命名規則・コーディング規約

- ファイル名: `PascalCase.vb`（例: `KlsryoCalculationEngine.vb`）
- クラス名: `PascalCase`
- メソッド名: `PascalCase`（公開）/ `camelCase`（内部なし、すべて PascalCase）
- プライベートフィールド: `_camelCase`（例: `_crud`）
- ローカル変数: 型略語を先頭に付ける慣習なし（ Access版の `il/sl/fl/dl` 型プレフィックスは廃止）
- SQL: StringBuilder で動的構築、NpgsqlParameter でパラメタライズ
- コメント: XML ドキュメントコメント (`''' <summary>`) は公開メンバのみ
- DBNull チェック: `IsDBNull()` 関数を使用
