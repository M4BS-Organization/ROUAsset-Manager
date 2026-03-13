# LeaseM4BS Migration - チーム運用ルール

## プロジェクト概要
Access版 LeaseM4BS を VB.NET/WinForms へマイグレーションするプロジェクト。

- マイグレーション元: `C:\access_LeaseM4BS`（Access版）
- マイグレーション先: `C:\kobayashi_LeaseM4BS`（VB.NET版、本リポジトリ）
- リポジトリ: `KobayashiManato/M4BS`

## タスク管理（GitHub Issues）

### タスクの確認
```bash
# 自分にアサインされたタスクを確認
gh issue list --repo KobayashiManato/M4BS --assignee @me

# 未アサインのタスクを確認
gh issue list --repo KobayashiManato/M4BS --no-assignee

# ラベルで絞り込み
gh issue list --repo KobayashiManato/M4BS --label "form-migration"
gh issue list --repo KobayashiManato/M4BS --label "priority:high"
```

### 作業開始
1. Issueを自分にアサイン
   ```bash
   gh issue edit <ISSUE番号> --repo KobayashiManato/M4BS --add-assignee @me
   ```
2. ブランチを作成（命名規則に従う）
3. 作業開始

### 完了時
1. PRを作成し、Issueを紐づけ
   ```bash
   gh pr create --repo KobayashiManato/M4BS --title "タイトル" --body "Closes #<ISSUE番号>"
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
