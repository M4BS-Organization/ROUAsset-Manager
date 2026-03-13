# LeaseM4BS Migration - チーム運用ルール

## プロジェクト概要
Access版 LeaseM4BS を VB.NET/WinForms へマイグレーションするプロジェクト。

- マイグレーション元: `C:\access_LeaseM4BS`（Access版）
- マイグレーション先: `C:\kobayashi_LeaseM4BS`（VB.NET版、本リポジトリ）
- リポジトリ: `M4BS-Organization/M4BS`

## タスク管理（GitHub Issues）

### タスクの確認
```bash
# 自分にアサインされたタスクを確認
gh issue list --repo M4BS-Organization/M4BS --assignee @me

# 未アサインのタスクを確認
gh issue list --repo M4BS-Organization/M4BS --no-assignee

# ラベルで絞り込み
gh issue list --repo M4BS-Organization/M4BS --label "form-migration"
gh issue list --repo M4BS-Organization/M4BS --label "priority:high"
```

### 作業開始
1. Issueを自分にアサイン
   ```bash
   gh issue edit <ISSUE番号> --repo M4BS-Organization/M4BS --add-assignee @me
   ```
2. ブランチを作成（命名規則に従う）
3. 作業開始

### 完了時
1. PRを作成し、Issueを紐づけ
   ```bash
   gh pr create --repo M4BS-Organization/M4BS --title "タイトル" --body "Closes #<ISSUE番号>"
   ```
2. レビュー依頼

## ブランチ命名規則
```
feature/<Issue番号>-<簡潔な説明>
例: feature/12-form-bcat-migration

bugfix/<Issue番号>-<簡潔な説明>
例: bugfix/15-fix-date-calculation
```

## ラベル一覧

### 分類
| ラベル | 用途 |
|--------|------|
| `form-migration` | フォーム移植 |
| `data-access` | DB/データアクセス層 |
| `business-logic` | 計算・業務ロジック |
| `ui` | 画面レイアウト・UX |
| `testing` | テスト |

### 優先度
| ラベル | 意味 |
|--------|------|
| `priority:high` | 優先度:高 |
| `priority:medium` | 優先度:中 |
| `priority:low` | 優先度:低 |

## 開発スケジュール（4.5日間: 3/13 PM 〜 3/19）

担当: 中村(`nkshn1726fgu`) / 小谷(`kodani-t`)

### Phase 1: 基盤修正（3/13 PM 〜 3/16）
- #2 FileHelper.ToFixedLengthFile 完成（中村）
- #3 FileHelper.ToCsvFile デリミタ修正（中村）
- #4 f_flx_KEIJO 計算ロジック（中村）
- #5 ログイン画面 JET 基本実装（小谷）
- #6 メインメニュー遷移確認（小谷）
- #7 f_flx_KHIYO 期間計算（小谷）

### Phase 2: 仕訳出力検証 + 計算TODO消化（3/17 〜 3/18）
- #8 f_flx_KLSRYO 取引分類（中村）
- #9 f_KEIJO_JOKEN 条件ロジック（中村）
- #10 f_CHUKI_JOKEN + f_IDOLST_JOKEN（中村）
- #11 標準仕訳出力_KJ 検証（小谷）
- #12 標準仕訳出力_SM/SH 検証（小谷）
- #13 仕訳出力設定画面 検証（小谷）

### Phase 3: 動作確認 + バグ修正（3/19）
- #14 E2Eフロー確認（中村）
- #15 バグ修正 + 型安全性（中村）
- #16 出力結果検証（小谷）
- #17 バグ修正 + 残TODO（小谷）

## 進捗報告
- Slackチャンネル `#m4bs-progress` に毎朝自動投稿
- 手動で進捗確認したい場合: Claudeに「進捗教えて」と聞く

## ディレクトリ構成
```
C:\kobayashi_LeaseM4BS/
├── LeaseM4BS/                    # メインソリューション
│   ├── LeaseM4BS.DataAccess/     # データアクセス層
│   └── LeaseM4BS.slnx            # ソリューションファイル
└── LeaseM4BS.TestWinForms/       # テスト用WinFormsプロジェクト
```
