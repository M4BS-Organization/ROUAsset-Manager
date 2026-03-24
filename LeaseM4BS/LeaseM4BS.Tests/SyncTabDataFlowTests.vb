Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Reflection
Imports System.Windows.Forms
Imports System.Drawing

Namespace LeaseM4BS.Tests

    ''' <summary>
    ''' タブ間データ連携テスト: MECE 分類
    '''
    ''' 【カテゴリA】 Tab1→Tab2 (SyncTab1ToInitialCost) - 契約参照情報の転記
    '''   A1: 正常系（全フィールド設定済み）
    '''   A2: 空文字（契約番号・名称が空）
    '''   A3: 長文（契約名称が最大長）
    '''   A4: リース期間の各表示パターン
    '''   A5: 値変更後の再同期
    '''
    ''' 【カテゴリB】 Tab1→Tab4 (SyncTab1ToSublease) - 元契約情報の参照表示
    '''   B1: 正常系（全フィールド設定済み）
    '''   B2: 日付フォーマット確認
    '''   B3: 空文字・デフォルト値
    '''   B4: 値変更後の再同期
    '''   B5: 年跨ぎ日付
    '''
    ''' 【カテゴリC】 Tab5→Tab3 (SyncJudgeToAccounting) - 判定結果連動
    '''   C1: オンバランス処理 → 赤バナー + マトリックス有効
    '''   C2: オフバランス処理 → シアンバナー + マトリックス無効
    '''   C3: 対象外 → グレーバナー + マトリックス無効
    '''   C4: 未判定("---") → デフォルトバナー + マトリックス無効
    '''   C5: 判定変更（オンバランス→オフバランス切替）
    '''   C6: 判定変更（オフバランス→オンバランス切替）
    '''
    ''' 【カテゴリD】 SetAccountingMatrixEnabled - マトリックス有効/無効
    '''   D1: enabled=True → 全16フィールドが Enabled + CLR_CARD
    '''   D2: enabled=False → 全16フィールドが Disabled + CLR_READONLY
    '''   D3: True→False切替
    '''   D4: False→True切替
    '''
    ''' 【カテゴリE】 イベント循環回避ガード
    '''   E1: _isLoaded=False → SyncTab1ToInitialCost スキップ
    '''   E2: _isLoaded=False → SyncTab1ToSublease スキップ
    '''   E3: _isSyncingData=True → SyncTab1ToInitialCost スキップ
    '''   E4: _isSyncingData=True → SyncTab1ToSublease スキップ
    '''   E5: _isLoaded=False → SyncJudgeToAccounting スキップ
    '''   E6: _isSyncingData は SyncJudgeToAccounting をブロックしない
    '''
    ''' 【カテゴリF】 タブ切替トリガー（統合テスト）
    '''   F1: Tab2(初回金)選択 → SyncTab1ToInitialCost 発火
    '''   F2: Tab3(会計)選択 → SyncJudgeToAccounting 発火
    '''   F3: Tab4(転貸)選択 → SyncTab1ToSublease 発火
    '''   F4: Tab5(判定)選択 → SyncTab1ToJudge 発火
    '''   F5: Tab1(契約)選択 → 同期メソッド未発火
    '''
    ''' 【カテゴリG】 RecalcJudge連鎖テスト
    '''   G1: RecalcJudge完了後にSyncJudgeToAccountingが呼ばれる
    '''
    ''' 【カテゴリH】 境界値・エッジケース
    '''   H1: フォーム初期化直後（_isLoaded=False）で全同期がスキップ
    '''   H2: 同一値で再同期（冪等性）
    '''   H3: 特殊文字を含む契約名称
    ''' </summary>
    <TestClass>
    Public Class SyncTabDataFlowTests

        Private _form As FrmLeaseContractMain

        Private Shared ReadOnly CLR_CARD As Color = Color.White
        Private Shared ReadOnly CLR_READONLY As Color = Color.FromArgb(233, 236, 239)
        Private Shared ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)

        ' リフレクション用ヘルパー
        Private Shared ReadOnly BF As BindingFlags =
            BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public

        <TestInitialize>
        Public Sub Setup()
            _form = New FrmLeaseContractMain()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            If _form IsNot Nothing Then
                _form.Dispose()
                _form = Nothing
            End If
        End Sub

        ' === リフレクション ヘルパー ===

        Private Function GetField(Of T)(name As String) As T
            Dim fi = _form.GetType().GetField(name, BF)
            Assert.IsNotNull(fi, $"フィールド '{name}' が見つかりません")
            Return CType(fi.GetValue(_form), T)
        End Function

        Private Sub SetField(name As String, value As Object)
            Dim fi = _form.GetType().GetField(name, BF)
            Assert.IsNotNull(fi, $"フィールド '{name}' が見つかりません")
            fi.SetValue(_form, value)
        End Sub

        Private Sub InvokeMethod(name As String, Optional args As Object() = Nothing)
            Dim mi = _form.GetType().GetMethod(name, BF)
            Assert.IsNotNull(mi, $"メソッド '{name}' が見つかりません")
            mi.Invoke(_form, args)
        End Sub

        ''' <summary>
        ''' _isLoaded=True にして各同期メソッドが動作するようにする
        ''' </summary>
        Private Sub ActivateForm()
            SetField("_isLoaded", True)
        End Sub

        ' =============================================================
        '  A. Tab1→Tab2 (SyncTab1ToInitialCost)
        ' =============================================================

        <TestMethod>
        Public Sub A01_正常系_全フィールド転記()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-2024-001"
            GetField(Of TextBox)("txtContractName").Text = "本社ビル賃貸借契約"
            GetField(Of Label)("lblLeaseMonths").Text = "60ヶ月"

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual("C-2024-001", GetField(Of Label)("lblInitContractNo").Text)
            Assert.AreEqual("本社ビル賃貸借契約", GetField(Of Label)("lblInitContractName").Text)
            Assert.AreEqual("60ヶ月", GetField(Of Label)("lblInitLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub A02_空文字_契約番号と名称が空()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = ""
            GetField(Of TextBox)("txtContractName").Text = ""
            GetField(Of Label)("lblLeaseMonths").Text = ""

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual("", GetField(Of Label)("lblInitContractNo").Text)
            Assert.AreEqual("", GetField(Of Label)("lblInitContractName").Text)
            Assert.AreEqual("", GetField(Of Label)("lblInitLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub A03_長文_契約名称100文字()
            ActivateForm()
            Dim longName As String = New String("あ"c, 100)
            GetField(Of TextBox)("txtContractName").Text = longName

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual(longName, GetField(Of Label)("lblInitContractName").Text)
        End Sub

        <TestMethod>
        Public Sub A04_リース期間_1ヶ月()
            ActivateForm()
            GetField(Of Label)("lblLeaseMonths").Text = "1ヶ月"

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual("1ヶ月", GetField(Of Label)("lblInitLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub A05_リース期間_600ヶ月()
            ActivateForm()
            GetField(Of Label)("lblLeaseMonths").Text = "600ヶ月"

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual("600ヶ月", GetField(Of Label)("lblInitLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub A06_値変更後の再同期()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-001"
            InvokeMethod("SyncTab1ToInitialCost")
            Assert.AreEqual("C-001", GetField(Of Label)("lblInitContractNo").Text)

            ' 値を変更して再同期
            GetField(Of TextBox)("txtContractNo").Text = "C-002"
            InvokeMethod("SyncTab1ToInitialCost")
            Assert.AreEqual("C-002", GetField(Of Label)("lblInitContractNo").Text)
        End Sub

        ' =============================================================
        '  B. Tab1→Tab4 (SyncTab1ToSublease)
        ' =============================================================

        <TestMethod>
        Public Sub B01_正常系_全フィールド転記()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-2024-001"
            GetField(Of TextBox)("txtContractName").Text = "本社ビル賃貸借契約"
            GetField(Of DateTimePicker)("dtpStartDate").Value = New DateTime(2024, 4, 1)
            GetField(Of DateTimePicker)("dtpEndDate").Value = New DateTime(2029, 3, 31)
            GetField(Of Label)("lblLeaseMonths").Text = "59ヶ月"

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual("C-2024-001", GetField(Of Label)("lblSubContractNo").Text)
            Assert.AreEqual("本社ビル賃貸借契約", GetField(Of Label)("lblSubContractName").Text)
            Assert.AreEqual("2024/04/01", GetField(Of Label)("lblSubStartDate").Text)
            Assert.AreEqual("2029/03/31", GetField(Of Label)("lblSubEndDate").Text)
            Assert.AreEqual("59ヶ月", GetField(Of Label)("lblSubLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub B02_日付フォーマット_yyyyMMdd()
            ActivateForm()
            GetField(Of DateTimePicker)("dtpStartDate").Value = New DateTime(2025, 1, 15)
            GetField(Of DateTimePicker)("dtpEndDate").Value = New DateTime(2025, 12, 31)

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual("2025/01/15", GetField(Of Label)("lblSubStartDate").Text)
            Assert.AreEqual("2025/12/31", GetField(Of Label)("lblSubEndDate").Text)
        End Sub

        <TestMethod>
        Public Sub B03_空文字とデフォルト値()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = ""
            GetField(Of TextBox)("txtContractName").Text = ""
            GetField(Of Label)("lblLeaseMonths").Text = "---ヶ月"

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual("", GetField(Of Label)("lblSubContractNo").Text)
            Assert.AreEqual("", GetField(Of Label)("lblSubContractName").Text)
            Assert.AreEqual("---ヶ月", GetField(Of Label)("lblSubLeasePeriod").Text)
        End Sub

        <TestMethod>
        Public Sub B04_値変更後の再同期()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-001"
            InvokeMethod("SyncTab1ToSublease")
            Assert.AreEqual("C-001", GetField(Of Label)("lblSubContractNo").Text)

            GetField(Of TextBox)("txtContractNo").Text = "C-999"
            InvokeMethod("SyncTab1ToSublease")
            Assert.AreEqual("C-999", GetField(Of Label)("lblSubContractNo").Text)
        End Sub

        <TestMethod>
        Public Sub B05_年跨ぎ日付()
            ActivateForm()
            GetField(Of DateTimePicker)("dtpStartDate").Value = New DateTime(2024, 12, 1)
            GetField(Of DateTimePicker)("dtpEndDate").Value = New DateTime(2025, 1, 31)

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual("2024/12/01", GetField(Of Label)("lblSubStartDate").Text)
            Assert.AreEqual("2025/01/31", GetField(Of Label)("lblSubEndDate").Text)
        End Sub

        ' =============================================================
        '  C. Tab5→Tab3 (SyncJudgeToAccounting) - 判定結果連動
        ' =============================================================

        <TestMethod>
        Public Sub C01_オンバランス処理_赤バナーとマトリックス有効()
            ActivateForm()
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"

            InvokeMethod("SyncJudgeToAccounting")

            Dim banner = GetField(Of Label)("lblAcctJudgeResult")
            Assert.AreEqual("リース判定: オンバランス処理", banner.Text)
            Assert.AreEqual(Color.White, banner.ForeColor)
            Assert.AreEqual(Color.FromArgb(220, 53, 69), banner.BackColor)

            ' マトリックスが有効であること
            Assert.IsTrue(GetField(Of TextBox)("txtSchPresentValue").Enabled)
            Assert.IsTrue(GetField(Of TextBox)("txtSchRouBegin").Enabled)
            Assert.IsTrue(GetField(Of TextBox)("txtSchLiabBegin").Enabled)
            Assert.IsTrue(GetField(Of TextBox)("txtSchAroBegin").Enabled)
            Assert.AreEqual(CLR_CARD, GetField(Of TextBox)("txtSchPresentValue").BackColor)
        End Sub

        <TestMethod>
        Public Sub C02_オフバランス処理_シアンバナーとマトリックス無効()
            ActivateForm()
            GetField(Of Label)("lblResultText").Text = "オフバランス処理"

            InvokeMethod("SyncJudgeToAccounting")

            Dim banner = GetField(Of Label)("lblAcctJudgeResult")
            Assert.AreEqual("リース判定: オフバランス処理", banner.Text)
            Assert.AreEqual(Color.White, banner.ForeColor)
            Assert.AreEqual(Color.FromArgb(23, 162, 184), banner.BackColor)

            ' マトリックスが無効であること
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)
            Assert.IsFalse(GetField(Of TextBox)("txtSchRouBegin").Enabled)
            Assert.IsFalse(GetField(Of TextBox)("txtSchLiabBegin").Enabled)
            Assert.IsFalse(GetField(Of TextBox)("txtSchAroBegin").Enabled)
            Assert.AreEqual(CLR_READONLY, GetField(Of TextBox)("txtSchPresentValue").BackColor)
        End Sub

        <TestMethod>
        Public Sub C03_対象外_グレーバナーとマトリックス無効()
            ActivateForm()
            GetField(Of Label)("lblResultText").Text = "対象外"

            InvokeMethod("SyncJudgeToAccounting")

            Dim banner = GetField(Of Label)("lblAcctJudgeResult")
            Assert.AreEqual("リース判定: 対象外", banner.Text)
            Assert.AreEqual(Color.FromArgb(33, 37, 41), banner.ForeColor) ' CLR_TEXT
            Assert.AreEqual(Color.FromArgb(204, 204, 204), banner.BackColor)
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)
        End Sub

        <TestMethod>
        Public Sub C04_未判定_デフォルトバナーとマトリックス無効()
            ActivateForm()
            GetField(Of Label)("lblResultText").Text = "---"

            InvokeMethod("SyncJudgeToAccounting")

            Dim banner = GetField(Of Label)("lblAcctJudgeResult")
            Assert.AreEqual("リース判定: ---", banner.Text)
            Assert.AreEqual(CLR_HEADER, banner.ForeColor)
            Assert.AreEqual(Color.FromArgb(230, 240, 250), banner.BackColor)
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)
        End Sub

        <TestMethod>
        Public Sub C05_判定変更_オンバランスからオフバランスへ()
            ActivateForm()
            ' まずオンバランス
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"
            InvokeMethod("SyncJudgeToAccounting")
            Assert.IsTrue(GetField(Of TextBox)("txtSchPresentValue").Enabled)

            ' オフバランスに変更
            GetField(Of Label)("lblResultText").Text = "オフバランス処理"
            InvokeMethod("SyncJudgeToAccounting")
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)
            Assert.AreEqual(CLR_READONLY, GetField(Of TextBox)("txtSchPresentValue").BackColor)
        End Sub

        <TestMethod>
        Public Sub C06_判定変更_オフバランスからオンバランスへ()
            ActivateForm()
            ' まずオフバランス
            GetField(Of Label)("lblResultText").Text = "オフバランス処理"
            InvokeMethod("SyncJudgeToAccounting")
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)

            ' オンバランスに変更
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"
            InvokeMethod("SyncJudgeToAccounting")
            Assert.IsTrue(GetField(Of TextBox)("txtSchPresentValue").Enabled)
            Assert.AreEqual(CLR_CARD, GetField(Of TextBox)("txtSchPresentValue").BackColor)
        End Sub

        ' =============================================================
        '  D. SetAccountingMatrixEnabled - マトリックス有効/無効
        ' =============================================================

        Private Shared ReadOnly MatrixFieldNames() As String = {
            "txtSchPresentValue",
            "txtSchRouBegin", "txtSchRouIncrease", "txtSchRouChange", "txtSchRouDecrease", "txtSchRouEnd",
            "txtSchLiabBegin", "txtSchLiabIncrease", "txtSchLiabChange", "txtSchLiabDecrease", "txtSchLiabEnd",
            "txtSchAroBegin", "txtSchAroIncrease", "txtSchAroChange", "txtSchAroDecrease", "txtSchAroEnd"
        }

        <TestMethod>
        Public Sub D01_有効化_全16フィールドがEnabled()
            ActivateForm()
            InvokeMethod("SetAccountingMatrixEnabled", New Object() {True})

            For Each fieldName In MatrixFieldNames
                Dim txt = GetField(Of TextBox)(fieldName)
                Assert.IsTrue(txt.Enabled, $"{fieldName} が Enabled でない")
                Assert.AreEqual(CLR_CARD, txt.BackColor, $"{fieldName} の BackColor が CLR_CARD でない")
            Next
        End Sub

        <TestMethod>
        Public Sub D02_無効化_全16フィールドがDisabled()
            ActivateForm()
            InvokeMethod("SetAccountingMatrixEnabled", New Object() {False})

            For Each fieldName In MatrixFieldNames
                Dim txt = GetField(Of TextBox)(fieldName)
                Assert.IsFalse(txt.Enabled, $"{fieldName} が Disabled でない")
                Assert.AreEqual(CLR_READONLY, txt.BackColor, $"{fieldName} の BackColor が CLR_READONLY でない")
            Next
        End Sub

        <TestMethod>
        Public Sub D03_TrueからFalse切替()
            ActivateForm()
            InvokeMethod("SetAccountingMatrixEnabled", New Object() {True})
            Assert.IsTrue(GetField(Of TextBox)("txtSchPresentValue").Enabled)

            InvokeMethod("SetAccountingMatrixEnabled", New Object() {False})
            Assert.IsFalse(GetField(Of TextBox)("txtSchPresentValue").Enabled)
        End Sub

        <TestMethod>
        Public Sub D04_FalseからTrue切替()
            ActivateForm()
            InvokeMethod("SetAccountingMatrixEnabled", New Object() {False})
            Assert.IsFalse(GetField(Of TextBox)("txtSchRouEnd").Enabled)

            InvokeMethod("SetAccountingMatrixEnabled", New Object() {True})
            Assert.IsTrue(GetField(Of TextBox)("txtSchRouEnd").Enabled)
        End Sub

        ' =============================================================
        '  E. イベント循環回避ガード
        ' =============================================================

        ''' <summary>_isLoaded を明示的に False にしてガードを検証するヘルパー</summary>
        Private Sub DeactivateForm()
            SetField("_isLoaded", False)
        End Sub

        <TestMethod>
        Public Sub E01_isLoadedFalse_SyncTab1ToInitialCostスキップ()
            DeactivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-001"
            Dim original = GetField(Of Label)("lblInitContractNo").Text

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual(original, GetField(Of Label)("lblInitContractNo").Text,
                            "_isLoaded=False の時は転記されないこと")
        End Sub

        <TestMethod>
        Public Sub E02_isLoadedFalse_SyncTab1ToSubleaseスキップ()
            DeactivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-001"
            Dim original = GetField(Of Label)("lblSubContractNo").Text

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual(original, GetField(Of Label)("lblSubContractNo").Text,
                            "_isLoaded=False の時は転記されないこと")
        End Sub

        <TestMethod>
        Public Sub E03_isSyncingDataTrue_SyncTab1ToInitialCostスキップ()
            ActivateForm()
            SetField("_isSyncingData", True)
            GetField(Of TextBox)("txtContractNo").Text = "C-BLOCKED"
            Dim original = GetField(Of Label)("lblInitContractNo").Text

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual(original, GetField(Of Label)("lblInitContractNo").Text,
                            "_isSyncingData=True の時は転記されないこと")
            ' フラグを元に戻す
            SetField("_isSyncingData", False)
        End Sub

        <TestMethod>
        Public Sub E04_isSyncingDataTrue_SyncTab1ToSubleaseスキップ()
            ActivateForm()
            SetField("_isSyncingData", True)
            GetField(Of TextBox)("txtContractNo").Text = "C-BLOCKED"
            Dim original = GetField(Of Label)("lblSubContractNo").Text

            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual(original, GetField(Of Label)("lblSubContractNo").Text,
                            "_isSyncingData=True の時は転記されないこと")
            SetField("_isSyncingData", False)
        End Sub

        <TestMethod>
        Public Sub E05_isLoadedFalse_SyncJudgeToAccountingスキップ()
            DeactivateForm()
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"
            Dim original = GetField(Of Label)("lblAcctJudgeResult").Text

            InvokeMethod("SyncJudgeToAccounting")

            Assert.AreEqual(original, GetField(Of Label)("lblAcctJudgeResult").Text,
                            "_isLoaded=False の時は反映されないこと")
        End Sub

        <TestMethod>
        Public Sub E06_isSyncingData_SyncJudgeToAccountingはブロックされない()
            ' SyncJudgeToAccounting は _isSyncingData ガードが除去されている
            ActivateForm()
            SetField("_isSyncingData", True)
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"

            InvokeMethod("SyncJudgeToAccounting")

            Assert.AreEqual("リース判定: オンバランス処理",
                            GetField(Of Label)("lblAcctJudgeResult").Text,
                            "_isSyncingData=True でもSyncJudgeToAccountingは実行されること")
            SetField("_isSyncingData", False)
        End Sub

        ' =============================================================
        '  F. タブ切替トリガー（統合テスト）
        ' =============================================================

        <TestMethod>
        Public Sub F01_Tab2同期で契約情報が反映される()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-TAB2"
            GetField(Of TextBox)("txtContractName").Text = "テスト契約"

            ' タブ2向け同期を実行
            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual("C-TAB2", GetField(Of Label)("lblInitContractNo").Text)
            Assert.AreEqual("テスト契約", GetField(Of Label)("lblInitContractName").Text)
        End Sub

        <TestMethod>
        Public Sub F02_Tab3同期で判定結果が反映される()
            ActivateForm()
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"

            ' タブ3向け同期を実行
            InvokeMethod("SyncJudgeToAccounting")

            Assert.AreEqual("リース判定: オンバランス処理",
                            GetField(Of Label)("lblAcctJudgeResult").Text)
        End Sub

        <TestMethod>
        Public Sub F03_Tab4同期で元契約情報が反映される()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-TAB4"
            GetField(Of DateTimePicker)("dtpStartDate").Value = New DateTime(2024, 4, 1)
            GetField(Of DateTimePicker)("dtpEndDate").Value = New DateTime(2029, 3, 31)

            ' タブ4向け同期を実行
            InvokeMethod("SyncTab1ToSublease")

            Assert.AreEqual("C-TAB4", GetField(Of Label)("lblSubContractNo").Text)
            Assert.AreEqual("2024/04/01", GetField(Of Label)("lblSubStartDate").Text)
            Assert.AreEqual("2029/03/31", GetField(Of Label)("lblSubEndDate").Text)
        End Sub

        <TestMethod>
        Public Sub F05_Tab2同期はTab4に影響しない()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-BEFORE"

            ' Tab2のみ同期
            InvokeMethod("SyncTab1ToInitialCost")
            Assert.AreEqual("C-BEFORE", GetField(Of Label)("lblInitContractNo").Text)

            ' Tab4のラベルは変更されていないこと
            Dim subLabel = GetField(Of Label)("lblSubContractNo").Text
            Assert.AreNotEqual("C-BEFORE", subLabel,
                               "Tab2同期はTab4に影響しないこと")
        End Sub

        ' =============================================================
        '  H. 境界値・エッジケース
        ' =============================================================

        <TestMethod>
        Public Sub H01_isLoadedFalseで全同期スキップ()
            DeactivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "SKIP"
            GetField(Of Label)("lblResultText").Text = "オンバランス処理"
            Dim origInit = GetField(Of Label)("lblInitContractNo").Text
            Dim origSub = GetField(Of Label)("lblSubContractNo").Text
            Dim origAcct = GetField(Of Label)("lblAcctJudgeResult").Text

            InvokeMethod("SyncTab1ToInitialCost")
            InvokeMethod("SyncTab1ToSublease")
            InvokeMethod("SyncJudgeToAccounting")

            ' 全同期がスキップされ、ラベルが変更されていないこと
            Assert.AreEqual(origInit, GetField(Of Label)("lblInitContractNo").Text)
            Assert.AreEqual(origSub, GetField(Of Label)("lblSubContractNo").Text)
            Assert.AreEqual(origAcct, GetField(Of Label)("lblAcctJudgeResult").Text)
        End Sub

        <TestMethod>
        Public Sub H02_冪等性_同一値で再同期()
            ActivateForm()
            GetField(Of TextBox)("txtContractNo").Text = "C-IDEM"
            GetField(Of TextBox)("txtContractName").Text = "冪等テスト"

            InvokeMethod("SyncTab1ToInitialCost")
            Dim first = GetField(Of Label)("lblInitContractNo").Text

            InvokeMethod("SyncTab1ToInitialCost")
            Dim second = GetField(Of Label)("lblInitContractNo").Text

            Assert.AreEqual(first, second, "同一値での再同期で結果が変わらないこと")
        End Sub

        <TestMethod>
        Public Sub H03_特殊文字を含む契約名称()
            ActivateForm()
            Dim specialName As String = "契約<>&""'テスト　全角スペース" & vbTab & "タブ"
            GetField(Of TextBox)("txtContractName").Text = specialName

            InvokeMethod("SyncTab1ToInitialCost")

            Assert.AreEqual(specialName, GetField(Of Label)("lblInitContractName").Text)
        End Sub

        <TestMethod>
        Public Sub H04_isSyncingDataフラグが例外時にもリセットされる()
            ActivateForm()
            ' 正常系の同期実行
            GetField(Of TextBox)("txtContractNo").Text = "C-FLAG"
            InvokeMethod("SyncTab1ToInitialCost")

            ' 同期完了後にフラグがFalseに戻っていること
            Assert.IsFalse(GetField(Of Boolean)("_isSyncingData"),
                           "同期完了後に_isSyncingDataがFalseに戻ること")
        End Sub

        <TestMethod>
        Public Sub H05_マトリックス全16フィールドの存在確認()
            ' SetAccountingMatrixEnabled で操作する16フィールドがすべて存在すること
            For Each fieldName In MatrixFieldNames
                Dim txt = GetField(Of TextBox)(fieldName)
                Assert.IsNotNull(txt, $"マトリックスフィールド '{fieldName}' が存在しないか null")
            Next
        End Sub

    End Class

End Namespace
