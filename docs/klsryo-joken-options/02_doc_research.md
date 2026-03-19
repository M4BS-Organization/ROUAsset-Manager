# ドキュメント調査: f_KLSRYO_JOKEN ラジオボタン追加

## 1. VB.NET WinForms RadioButton + GroupBox のベストプラクティス

### コンテナベースのグルーピング
- 同一コンテナ（Form, GroupBox, Panel）内のRadioButtonは自動的に1つのグループとして機能
- 複数のラジオボタングループが必要な場合、**別々のGroupBoxに配置**する
- GroupBox内のRadioButtonのみが相互排他的

### GroupBox vs Panel
- **GroupBox**: キャプション表示可、常にボーダー表示 → 論理グループの視覚的表現に最適
- **Panel**: キャプションなし、BorderStyle制御可能

### 実装ポイント
- RadioButtonを**まずGroupBoxに追加**し、**GroupBoxのみをフォームに追加**
- `Controls.Add()` の順序: RadioButton → GroupBox → Form

## 2. Access VBA OptionGroup → WinForms RadioButton 移植パターン

### マッピング

| Access (OptionGroup) | WinForms |
|---|---|
| `OptionGroup` コンテナ | `GroupBox` コンテナ |
| `OptionButton` | `RadioButton` |
| `OptionGroup.Value` (整数) | 各 `RadioButton.Checked` をチェック or ヘルパー関数 |
| `OptionGroup_AfterUpdate` | 各 `RadioButton.CheckedChanged` イベント |

### 推奨移植パターン（Tag プロパティ活用）

```vb
' RadioButton.Tag に Access の OptionValue を格納
rb1.Tag = 1
rb2.Tag = 2
rb3.Tag = 3

Private Function GetOptionGroupValue(grp As GroupBox) As Integer
    For Each ctrl As Control In grp.Controls
        If TypeOf ctrl Is RadioButton AndAlso DirectCast(ctrl, RadioButton).Checked Then
            Return CInt(ctrl.Tag)
        End If
    Next
    Return 0
End Function
```

## 3. Designer.vb での RadioButton 配置

### 典型的なコード構造

```vb
' フィールド宣言
Friend WithEvents grp_TAISHO As GroupBox
Friend WithEvents rdo_TAISHO_1 As RadioButton
Friend WithEvents rdo_TAISHO_2 As RadioButton

' InitializeComponent() 内
Me.grp_TAISHO = New GroupBox()
Me.rdo_TAISHO_1 = New RadioButton()

' SuspendLayout
Me.grp_TAISHO.SuspendLayout()

' RadioButton プロパティ設定
Me.rdo_TAISHO_1.AutoSize = True
Me.rdo_TAISHO_1.Location = New Point(10, 20)
Me.rdo_TAISHO_1.Text = "選択肢1"

' GroupBoxにRadioButtonを追加（子→親の順）
Me.grp_TAISHO.Controls.Add(Me.rdo_TAISHO_1)

' FormにGroupBoxを追加
Me.Controls.Add(Me.grp_TAISHO)

' ResumeLayout
Me.grp_TAISHO.ResumeLayout(False)
Me.grp_TAISHO.PerformLayout()
```

### 注意点
- `SuspendLayout()` / `ResumeLayout()` の対応を維持
- デフォルトで1つのRadioButtonに `Checked = True` を設定
- 縦並び: Y座標を20-25pxピッチ

## 4. 移植指針まとめ

1. 各OptionGroupに対応するGroupBoxを作成: `grp_TAISHO`, `grp_KTMG`, `grp_MEISAI`
2. 各OptionButtonに対応するRadioButtonを作成: `rdo_グループ名_番号`
3. Designer.vb でRadioButtonをGroupBox.Controls に追加
4. Tag プロパティまたはヘルパー関数で整数値取得
5. CheckedChanged イベントで AfterUpdate 相当の処理を実装
