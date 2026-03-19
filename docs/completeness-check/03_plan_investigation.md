# 開発計画・Issue・要件調査レポート — Issue #26

調査日: 2026-03-19

---

## 1. Issue #26 詳細

**タイトル:** 顧客固有仕訳出力フォーム共通基盤の設計・実装（fc_系52画面）
**状態:** OPEN（未クローズ）
**担当者:** kodani-t
**優先度:** priority:high
**ラベル:** form-migration, priority:high

### 1.1 完了条件（Acceptance Criteria）

```
- [ ] 共通インターフェースの設計完了
- [ ] 設定駆動による出力形式切り替えが動作
- [ ] 最低2顧客の固有仕訳出力が共通基盤上で動作
```

### 1.2 対象スコープ（Issue本文より）

- 対象: fc_ プレフィックス付き**52画面**
- 対象顧客: MYCOM, TSYSCOM, SNKO, JOT, RISO, NKSOL, VTC, VALQUA 等
- 各顧客カスタマイズポイント:
  - MYCOM: 二重構造
  - TSYSCOM: CSV形式
  - JOT: 共通化
  - RISO: 厳格チェック
  - NKSOL: 残高検証
  - VALQUA: 長短振替

### 1.3 背景

- Access版に fc_系フォームが**52画面**存在
- 個別実装では非効率 → 共通基盤で効率化
- VB.NET 側での実装画面数: 現在 53 ファイル（Designer除く）確認済み

---

## 2. PR #85 の実装サマリー

**タイトル:** feat: fc_系顧客固有仕訳出力フォーム共通基盤の実装 (Issue #26)

### 2.1 変更ファイル

| ファイル | 種別 |
|---|---|
| `FcJournalOutputBase.vb` | 新規 — Template Method 基底クラス |
| `FcSetteiHelper.vb` | 新規 — 設定永続化ヘルパー |
| `sql/007_tw_fc_common_tables.sql` | 新規 — tw_fc_swk_wrk テーブル |
| `Form_fc_支払仕訳_KITOKU.vb` + Designer | 新規 |
| `Form_fc_支払仕訳_KITOKU_SUB.vb` + Designer | 新規 |
| `Form_fc_計上仕訳_KITOKU.vb` + Designer | 新規 |
| `KitokuFixedLengthFormats.vb` | 修正（名前空間二重ネスト解消） |
| `FixedLengthFileWriter.vb` | 修正（名前空間二重ネスト解消） |
| 両 .vbproj | 修正（新規ファイル登録） |

### 2.2 コミット: fae5741

10ファイル変更、1109行追加、96行削除

---

## 3. fc_系フォームの全体スコープ

### 3.1 Access版 fc_系ファイル一覧（drive-downloadより）

```
fc_JOT_支払仕訳.txt
fc_JOT_計上仕訳.txt
fc_MYCOM_仕訳出力.txt
fc_MYCOM_仕訳出力Sub.txt
fc_MYCOM_仕訳出力_会社MNT.txt
fc_MYCOM_支払伝票印刷.txt
fc_MYCOM_支払伝票印刷Sub.txt
fc_MYCOM_支払伝票印刷_一括設定.txt
fc_SANKO_AIR_振替伝票_支払用_出力指示.txt
fc_SANKO_AIR_振替伝票_支払用_出力指示_SUB.txt
fc_SANKO_AIR_振替伝票_支払用_出力指示_修正.txt
fc_SANKO_AIR_振替伝票_支払用_出力指示_預金.txt
fc_SANKO_AIR_振替伝票_計上用_出力指示.txt
fc_SANKO_AIR_異動届_JOKEN.txt
fc_SANKO_AIR_登録変更願_JOKEN.txt
fc_SANKO_AIR_登録届_JOKEN.txt
fc_SNKO_仕訳出力_JOKEN.txt
fc_SNKO_仕訳出力_JOKEN_SUB.txt
fc_SNKO_仕訳出力_最終確認.txt
fc_SNKO_計上仕訳出力_JOKEN.txt
fc_SNKO_計上仕訳出力_最終確認.txt
fc_TC_HREL.txt
fc_TC_SWK_DEF_COM.txt
fc_TSYSCOM_支払仕訳.txt
fc_TSYSCOM_移動仕訳.txt
fc_TSYSCOM_計上仕訳.txt
fc_VALQUA_支払仕訳.txt
fc_VALQUA_計上仕訳.txt
fc_VALQUA_長短振替仕訳.txt
fc_仕訳出力_VTC_明細.txt
fc_仕訳出力_最終確認_RISO.txt
fc_支払仕訳_JOT.txt
fc_支払仕訳_JOT_伝票番号.txt
fc_支払仕訳_KITOKU.txt
fc_支払仕訳_KITOKU_SUB.txt  ← 今回実装済み
fc_支払仕訳_KYOTO.txt
fc_支払仕訳_NKSOL.txt
fc_支払仕訳_RISO.txt
fc_計上仕訳_KITOKU.txt      ← 今回実装済み
... 他多数
```

### 3.2 KITOKU以外の顧客対応状況

| 顧客 | Access版ファイル数 | VB.NET実装状況 |
|---|---|---|
| KITOKU | 3 | **部分実装**（今回のPR#85） |
| MYCOM | 6 | 未実装（スケルトンのみ） |
| TSYSCOM | 3 | 未実装（スケルトンのみ） |
| SNKO | 5 | 未実装（スケルトンのみ） |
| JOT | 4 | 未実装（スケルトンのみ） |
| RISO | 3 | 未実装（スケルトンのみ） |
| NKSOL | 1 | 未実装（スケルトンのみ） |
| VALQUA | 3 | 未実装（スケルトンのみ） |
| SANKO_AIR | 6 | 未実装（スケルトンのみ） |
| VTC | 1 | 未実装（スケルトンのみ） |
| KYOTO | 1 | 未実装（スケルトンのみ） |

---

## 4. Issue #26 完了条件の達成状況

| 完了条件 | 状態 | 備考 |
|---|---|---|
| 共通インターフェースの設計完了 | **達成** | FcJournalOutputBase 実装済み |
| 設定駆動による出力形式切り替えが動作 | **部分達成** | FcSetteiHelper実装済み。設定駆動の出力形式切り替えは未実装 |
| 最低2顧客の固有仕訳出力が共通基盤上で動作 | **部分達成** | KITOKU 2画面実装済みだが、基底クラスのExecute()を使わず独自フロー |

---

## 5. マイグレーション全体における位置づけ

```
Phase 1 (基盤修正)      ← 完了
Phase 2 (仕訳出力検証)  ← Issue #26 はここ（実装中）
Phase 3 (動作確認)      ← 未着手
```

Issue #26 は「fc_系52画面の共通基盤」として最重要タスクの一つ。
KITOKU 2画面の基盤実装は完了しているが、残り50画面（10顧客）は未実装。

---

## 6. 開発スケジュール上の状況

- 開発期間: 3/13 PM 〜 3/19（本日が最終日）
- Issue #26 は未クローズ（OPEN状態継続中）
- 完了条件の「最低2顧客」は KITOKU 1顧客で支払仕訳・計上仕訳の2種類として満たしているが、要件上は「2顧客」（KITOKU + 別顧客）の可能性もある
