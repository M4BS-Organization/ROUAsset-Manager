# コードベース調査: f_flx_KLSRYO 計算ロジック

## 現状の VB.NET 実装

### Form_f_flx_KLSRYO.vb (116行)
- 基本的なSQLクエリ + DataGridView表示の骨格のみ
- 6つの計算項目がTODOコメントアウト: 行区, 法令区分, 取引区分, 請求月, 回数済/総, リース料残高
- cmd_RECALCULATE_Click は空実装

### Form_f_KLSRYO_JOKEN.vb (60行)
- 日付FROM/TOの入力 + 期間計算
- Form_f_flx_KLSRYO に日付パラメータを渡していない

### 参照パターン
- **Form_f_flx_KEIJO.vb**: 法令区分CASE式, 残高計算, FormatColumn
- **Form_f_flx_KHIYO.vb**: 行区(REC_KBN)分類, DtFrom/DtTo Property, UNION ALL

## Access版 計算エンジン構造

### pc_SHRI_KLSRYO (メイン計算クラス)
- `gKLSRYO_Main`: オーケストレーター
- `mKLSRYO_Sub_KYKM`: 物件単位集計
- `mKLSRYO_Sub_HAIF`: 配賦単位集計
- `mKLSRYO_Sub_HENF`: 付随費用集計
- `mCALC_KLSRYOfromSCH`: キャッシュスケジュールから期間集計 (コア)
- `mCHK_集計対象`: 集計対象判定 (4パターン)
- `mKLSRYO_SUB_OUTREC_SET`: 結果レコード書き出し

### pc_SHRI_COM (共通ライブラリ)
- `gMOTO_RSETSQL_EDIT`: ソースSQL構築
- `gMakeCASH_SCH_T`: 定額キャッシュスケジュール生成
- `gMakeCASH_SCH_H`: 変額キャッシュスケジュール生成

### 主要定数
- engSHRI_REC_KBN: 1=定額, 2=変額, 3=付随費用
- engSHRI_KTMG: 1=締日ベース, 2=支払日ベース
- engSHRI_MEISAI: 1=物件単位, 2=配賦単位
- engKKBN: 1=リース, 2=レンタル, 3=保守
- engKJKBN: 1=費用, 2=資産

## データベーステーブル
- D_KYKH: 契約ヘッダ (start_dt, end_dt, mkaisu, shri_cnt, k_slsryo, zritu, ...)
- D_KYKM: 契約明細 (b_klsryo, b_kzei, b_mlsryo, b_mzei, b_slsryo, b_henl_f, ...)
- D_HAIF: 配賦 (line_id, haifritu, h_klsryo, h_bcat_id, hkmk_id)
- D_HENL: 変額リース料
- C_KKBN, C_KJKBN, C_LEAKBN: 分類マスタ
- M_LCPT, M_BCAT, M_HKMK: 名称マスタ
- t_settei: 設定テーブル (SEKOU_DT=施行日)
