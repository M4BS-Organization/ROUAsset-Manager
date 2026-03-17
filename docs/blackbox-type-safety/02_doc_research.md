# ドキュメント調査結果: VB.NET ブラックボックステスト & 型安全性

## 1. VB.NET でのブラックボックステストのベストプラクティス（Access VBA移行時）

**ゴールデンマスターテスト（Characterization Testing）が最適**

レガシーシステム移行時のブラックボックステストには「ゴールデンマスターテスト」パターンが推奨される：

- Access版の既存の動作（出力）を「ゴールデンマスター」として記録
- VB.NET版で同じ入力を与え、出力をゴールデンマスターと比較
- 差分があれば移行バグとして検出

**.NETでの実装ツール**: ApprovalTests.NET や Verify ライブラリが利用可能。

**移行テスト戦略**:
1. Access版で代表的な入力パターンの出力を記録
2. VB.NET版で同じ入力を処理
3. 出力ファイル（固定長、CSV等）をバイト単位で比較
4. 差分箇所を特定し、型変換・丸め誤差等を調査

## 2. VB.NET の型安全性に関する一般的な問題

**CDbl vs Convert.ToDouble の挙動差異**:
- `CDbl()` はシステムの地域設定（カルチャ）を考慮する
- `Convert.ToDouble()` はより厳格
- `Val()` はピリオドのみを小数点として認識（カルチャ非依存）

**CInt のString解析の違い**:
- `CInt("10.5")` → 正常動作（小数を解析して丸める）→ 結果: `10`（銀行家丸め）
- `Convert.ToInt32("10.5")` → **例外発生**（FormatException）

**推奨**: VB固有の型変換関数（`CInt`, `CDbl`, `CDec`等）を優先使用すべき（Microsoft公式推奨）

## 3. Access VBA と VB.NET の数値計算における精度差異

### データ型のサイズ変更

| 型名 | VBA/VB6 | VB.NET | 注意点 |
|------|---------|--------|--------|
| `Integer` | 16-bit | **32-bit** | VBAのIntegerはVB.NETではShort相当 |
| `Long` | 32-bit | **64-bit** | VBAのLongはVB.NETではInteger相当 |
| `Currency` | 64-bit整数÷10000（小数4桁） | **存在しない** | `Decimal`で代替 |
| `Double` | 64-bit浮動小数点 | 64-bit浮動小数点（同一） | 精度は同等 |
| `Decimal` | VBAではVariant/Decimalのみ | ネイティブ128-bit | 最大29桁の有効数字 |

### Currency型 → Decimal型の移行リスク
- VBA `Currency`: 小数点以下 **4桁** 固定精度
- VB.NET `Decimal`: 小数点以下 **最大28桁**
- 移行後に精度が上がるため、丸め処理の結果が変わる可能性がある

### Double型の注意点
- 両環境とも IEEE 754 準拠の64-bit浮動小数点
- 金融計算では `Decimal` を使うべき（Microsoft公式推奨）

## 4. 丸め処理の差異（重要）

**VBA `Round()` と VB.NET `Math.Round()` の比較**:
- **両方とも銀行家丸め（Banker's Rounding）がデフォルト**
- VB.NETの追加オプション: `MidpointRounding` 列挙型で制御可能

**VB.NETの整数変換関数（CInt, CLng等）も銀行家丸めを使用**:
- `CInt(0.5)` → `0`（偶数へ）
- `CInt(1.5)` → `2`（偶数へ）

## 5. MSTest / NUnit / xUnit での VB.NET ブラックボックステストパターン

**レガシー移行向け推奨**: .NET Framework 4.x であれば **MSTest** が最も摩擦が少ない。

**ゴールデンマスターパターンの実装例**:
```vb
<TestMethod>
Public Sub Test_ScheduleOutput_MatchesAccessVersion()
    Dim input As New TestInput With {
        .LeaseAmount = 1000000D,
        .Rate = 0.05D,
        .Term = 60
    }
    Dim actualOutput = GenerateSchedule(input)
    Dim expectedOutput = File.ReadAllText("golden_master/schedule_001.txt")
    Assert.AreEqual(expectedOutput, actualOutput)
End Sub
```

## 6. 移行時の主要リスクと対策

| リスク | 影響 | 対策 |
|--------|------|------|
| Integer/Long サイズ変更 | オーバーフロー挙動の変化 | VBAのInteger→Short、Long→Integerにマッピング |
| Currency型の廃止 | 精度変化（4桁→28桁） | Decimalで代替し、丸め処理を明示的に追加 |
| 丸め処理 | 中間値の丸め方向 | 銀行家丸めがデフォルトであることを確認 |
| 文字列→数値変換 | カルチャ依存の挙動差 | CDbl/CInt等のVB関数を使用 |
| 浮動小数点精度 | 微小な計算誤差 | 金融計算にはDecimal型を使用 |
