# コードベース調査報告: 固定長出力フォーマット

## 1. Access版実装

### ソースファイル
- `C:\Users\SAP1\Downloads\リースM4_開発用-20260313T010343Z-3-001\リースM4_開発用\AccessVBA\pc_仕訳出力.txt`

### 出力関数
| 関数名 | 行範囲 | ワークテーブル | ソート順 |
|--------|--------|---------------|---------|
| `gCMSW2WRK出力()` | 110-218 | tw_KITOKU_CMSW2WRK | SW2_DEN_NO, SW2_DC_KBN, SW2_GYO_NO |
| `gAPGDHWRK出力()` | 230-369 | tw_KITOKU_APGDHWRK | GDH_DEN_NO |
| `gAPGDDWRK出力()` | 381-471 | tw_KITOKU_APGDDWRK | GDD_DEN_NO, GDD_GYO_NO |
| `gAPGDSWRK出力()` | 483-602 | tw_KITOKU_APGDSWRK | GDS_DEN_NO, GDS_GYO_NO |

### 共通パターン
- パディング関数: `gStrSizeAdjust(value, byteWidth)` — Shift-JISバイト単位で右パディング（半角スペース）
- Null処理: `Nz(value, 0)` で数値Null→0変換
- 数値フォーマット: `Format(Nz(value, 0), "000000000000000000.000")` 等
- ファイル出力: `Open ... For Output` → `Print #ilFNum, slOutRec` → `Close`

## 2. VB.NET版既存実装

### FileHelper.vb
- パス: `LeaseM4BS.TestWinForms/LeaseM4BS.TestWinForms/FileHelper.vb`
- `ToFixedLengthFile()` (L129-162): DataGridView→固定長ファイル出力（汎用）
- `PadRightByte()` (L187-199): Shift-JISバイト単位パディング（Access版gStrSizeAdjust相当）
- `GetColumnByteWidth()` (L167-182): 列幅取得（Dictionary→Tag→デフォルト）

### 課題
- DataGridView依存（UI層）→ DataAccess層にDataTable対応版が必要
- フォーマット固有のフィールド定義が未定義
- 数値フォーマット処理が未実装

## 3. 関連テーブル

### PostgreSQL（既存）
- `tw_s_chuki_keijo` — 注記計上結果ワーク
- `tw_d_henl_keijo` — 変額リース仕訳ワーク
- `tw_d_gson_keijo` — 減損仕訳ワーク

### PostgreSQL（未作成）
- `tw_kitoku_cmsw2wrk` — KITOKU伝票ワーク
- `tw_kitoku_apgdhwrk` — KITOKU金額概要ワーク
- `tw_kitoku_apgddwrk` — KITOKU金額詳細ワーク
- `tw_kitoku_apgdswrk` — KITOKU支払ワーク

## 4. 既存テストパターン
- コンソールベースのブラックボックステスト（test_keijo_blackbox.vb）
- `vbc /r:LeaseM4BS.DataAccess.dll` でコンパイル
- Sub Main() で各テスト関数を順次実行、Boolean結果でPASS/FAIL判定
