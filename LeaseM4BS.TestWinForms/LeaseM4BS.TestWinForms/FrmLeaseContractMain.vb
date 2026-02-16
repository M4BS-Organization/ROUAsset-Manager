Imports System
Imports System.Drawing
Imports System.Windows.Forms

' =============================================================================
' 新リース会計基準対応 契約管理・判定システム (UI強化・完全統合版)
'
' [今回の修正点]
' Tab1「契約入力」をアップロード画像とExcelシートに基づき大幅リニューアル。
'  - セクション区切り（基本情報、貸主情報、物件情報）の導入
'  - 不動産属性（面積、構造、竣工日）の追加
'  - 貸主詳細（住所、連絡先）の追加
'  - 4列レイアウトによる高密度化
' =============================================================================
Public Class FrmLeaseContractMain
    Inherits Form

    ' ▼ 画面レイアウト用コンテナ
    Private pnlHeader As Panel      ' 上部：ヘッダー
    Private pnlFooter As Panel      ' 下部：ボタンエリア
    Private pnlBody As Panel        ' 中央：タブ表示エリア

    ' ▼ メインコンテナ（タブ）
    Private tabMain As TabControl
    Private pgContract As TabPage
    Private pgInitial As TabPage
    Private pgJudge As TabPage
    Private pgAccounting As TabPage

    ' ▼ 共通ヘッダー項目（全タブ共通）
    Private txtAssetNameHdr As TextBox
    Private txtContractNoHdr As TextBox

    ' -------------------------------------------------------------------------
    ' Tab1: 契約入力用 変数（強化版）
    ' -------------------------------------------------------------------------
    ' 基本情報
    Private txtContractName As TextBox      ' 契約名
    Private txtGroup As TextBox             ' グループ
    Private cmbAssetType As ComboBox        ' 資産内訳（重要：土地建物/車両など）
    Private cmbStatus As ComboBox           ' 契約状況
    Private cmbDeptAdmin As ComboBox        ' 管理部署
    Private cmbDeptCost As ComboBox         ' 費用負担部署

    ' 貸主情報
    Private txtLessorName As TextBox        ' 貸主名
    Private txtLessorCode As TextBox        ' 貸主コード
    Private txtLessorAddress As TextBox     ' 貸主住所
    Private txtLessorTel As TextBox         ' 貸主TEL

    ' 物件・資産詳細
    Private txtPropertyName As TextBox      ' 物件名/資産名
    Private txtLocation As TextBox          ' 設置場所/住所
    Private txtStructure As TextBox         ' 構造/用途
    Private txtArea As TextBox              ' 面積
    Private dtpCompletion As DateTimePicker ' 竣工日/製造日
    Private txtRemarks As TextBox           ' 特記事項

    ' -------------------------------------------------------------------------
    ' Tab2: 初回一時金用 変数
    ' -------------------------------------------------------------------------
    Private dgvInitialPay As DataGridView

    ' -------------------------------------------------------------------------
    ' Tab3: リース判定用 変数 (ASBJ第34号対応版)
    ' -------------------------------------------------------------------------
    ' 識別判定 Q1~Q4
    Private grpQ1 As GroupBox, rbQ1Yes As RadioButton, rbQ1No As RadioButton
    Private txtQ1Memo As TextBox
    Private grpQ2 As GroupBox, rbQ2Yes As RadioButton, rbQ2No As RadioButton
    Private grpQ3 As GroupBox, rbQ3Yes As RadioButton, rbQ3No As RadioButton
    Private grpQ4 As GroupBox, rbQ4Yes As RadioButton, rbQ4No As RadioButton

    ' リース期間・免除規定
    Private dtpJudgeStart As DateTimePicker
    Private dtpJudgeEnd As DateTimePicker
    Private lblTermMonths As Label
    Private lblDateError As Label
    Private chkExtOption As CheckBox
    Private cboExtCertainty As ComboBox
    Private numExtMonths As NumericUpDown
    Private lblShortTermResult As Label
    Private numAssetValue As NumericUpDown
    Private lblLowValueResult As Label
    Private chkApplyExemption As CheckBox

    ' 判定結果パネル
    Private pnlResult As Panel
    Private lblResultText As Label
    Private lblResultBadge As Label
    Private lblResultReason As Label

    ' 定数
    Private Const LOW_VALUE_THRESHOLD As Decimal = 3000000D

    ' 制御用フラグ
    Private _isLoaded As Boolean = False

    ' -------------------------------------------------------------------------
    ' Tab4: 会計入力用 変数
    ' -------------------------------------------------------------------------
    Private dgvSchedule As DataGridView


    ' =========================================================================
    ' コンストラクタ
    ' =========================================================================
    Public Sub New()
        Me.Text = "新リース会計基準 契約管理・判定システム"
        Me.Size = New Size(1280, 900)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Meiryo UI", 9.0F)
        Me.MinimumSize = New Size(1200, 720)

        InitializeComponent()

        _isLoaded = True
        RecalcJudge()
    End Sub

    ' =========================================================================
    ' 全体レイアウト構築
    ' =========================================================================
    Private Sub InitializeComponent()
        Me.SuspendLayout()

        ' 1. Header (共通ヘッダー)
        pnlHeader = New Panel() With {.Dock = DockStyle.Top, .Height = 55, .BackColor = Color.WhiteSmoke, .Padding = New Padding(10, 8, 10, 0)}
        Dim flowH As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False}

        ' 物件名（読み取り専用・連携）
        flowH.Controls.Add(New Label() With {.Text = "対象物件名", .AutoSize = True, .Padding = New Padding(0, 6, 0, 0), .Font = New Font(Me.Font, FontStyle.Bold)})
        txtAssetNameHdr = New TextBox() With {.Width = 350, .ReadOnly = True, .BackColor = Color.White, .Text = "（未入力）"}
        flowH.Controls.Add(txtAssetNameHdr)

        ' 契約番号（読み取り専用・連携）
        flowH.Controls.Add(New Label() With {.Text = "契約番号", .AutoSize = True, .Padding = New Padding(20, 6, 0, 0), .Font = New Font(Me.Font, FontStyle.Bold)})
        txtContractNoHdr = New TextBox() With {.Width = 150, .ReadOnly = True, .BackColor = Color.White, .Text = "（未入力）"}
        flowH.Controls.Add(txtContractNoHdr)
        pnlHeader.Controls.Add(flowH)

        ' 2. Footer (ボタン)
        pnlFooter = New Panel() With {.Dock = DockStyle.Bottom, .Height = 50, .BackColor = Color.Gainsboro, .Padding = New Padding(10)}
        Dim flowF As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft}
        Dim btnSave As New Button() With {.Text = "登録", .Height = 30, .Width = 100, .BackColor = Color.LightSkyBlue, .FlatStyle = FlatStyle.Flat}
        Dim btnClose As New Button() With {.Text = "閉じる", .Height = 30, .Width = 100}
        AddHandler btnClose.Click, Sub(s, e) Me.Close()
        flowF.Controls.Add(btnSave)
        flowF.Controls.Add(btnClose)
        pnlFooter.Controls.Add(flowF)

        ' 3. Body (タブエリア)
        pnlBody = New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(5)}
        tabMain = New TabControl() With {.Dock = DockStyle.Fill}

        pgContract = New TabPage("1. 契約入力")
        pgInitial = New TabPage("2. 初回一時金")
        pgJudge = New TabPage("3. リース判定")
        pgAccounting = New TabPage("4. 会計入力(スケジュール)")

        ' 各タブ構築
        InitTabContract()  ' ★強化版メソッド
        InitTabInitial()
        InitTabJudge_Pro()          ' ★完全移植版メソッド
        InitTabAccounting()

        tabMain.TabPages.AddRange({pgContract, pgInitial, pgJudge, pgAccounting})
        pnlBody.Controls.Add(tabMain)

        ' コントロール追加（順序重要）
        Me.Controls.Add(pnlBody)
        Me.Controls.Add(pnlFooter)
        Me.Controls.Add(pnlHeader)

        Me.ResumeLayout(False)
    End Sub


    ' =========================================================================
    ' [修正版] Tab1: 契約入力用 変数
    ' =========================================================================
    ' --- 左側：契約基本・条件 ---
    ' 期間・条件
    Private dtpContractDate As DateTimePicker   ' 契約締結日
    Private dtpStartTab1 As DateTimePicker      ' 開始日(Tab1用)
    Private dtpEndTab1 As DateTimePicker        ' 終了日(Tab1用)
    Private chkAutoUpdate As CheckBox           ' 自動更新
    Private dtpNoticeDate As DateTimePicker     ' 解約/更新通知期限
    Private numUpdateCount As NumericUpDown     ' 更新見込回数

    ' 貸主
    Private txtBankInfo As TextBox              ' 振込先情報


    ' --- 明細情報（物件・資産リスト） ---
    ' ★修正：テキストボックスではなくグリッドで管理
    Private dgvProperties As DataGridView





    ' =========================================================================
    ' [最終修正版] Tab1: 契約・物件情報
    '  - レイアウト崩れを根絶するため、TableLayoutPanelの行設定を厳密に定義
    '  - ヘルパー関数を使わず、明示的にコントロールを配置して確実性を担保
    ' =========================================================================
    Private Sub InitTabContract()
        Dim pnlScroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(10)}

        ' 左右分割テーブル (左:50% / 右:50%)
        Dim splitTbl As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 2,
            .RowCount = 1
        }
        splitTbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        splitTbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))

        ' ---------------------------------------------------------
        ' [左カラム] 基本情報・貸主・条件
        ' ---------------------------------------------------------
        Dim pnlLeft As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(0, 0, 10, 0), .AutoSize = True}

        ' A. 基本契約情報
        Dim grpBasic As GroupBox = CreateGroupBox("基本契約情報")
        Dim tlpBasic As New TableLayoutPanel() With {.Dock = DockStyle.Top, .AutoSize = True, .ColumnCount = 4, .RowCount = 4}
        SetupTableColumns(tlpBasic)

        txtContractName = New TextBox() With {.Dock = DockStyle.Fill}
        txtGroup = New TextBox() With {.Dock = DockStyle.Fill}
        cmbAssetType = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbAssetType.Items.AddRange({"土地建物普通賃借", "土地建物定期賃借", "車両", "OA機器"})
        cmbStatus = New ComboBox() With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbStatus.Items.AddRange({"契約中", "申請中", "解約済"})
        cmbDeptAdmin = New ComboBox() With {.Dock = DockStyle.Fill}
        cmbDeptCost = New ComboBox() With {.Dock = DockStyle.Fill}

        ' 1行目: 契約名 (横結合)
        AddControlToTable(tlpBasic, 0, "契約名", txtContractName, 3)
        ' 2行目: グループ / 状況
        AddControlToTable(tlpBasic, 1, "グループ", txtGroup)
        AddControlToTable(tlpBasic, 1, "契約状況", cmbStatus, 0, 2) ' 列インデックス指定で配置
        ' 3行目: 資産区分
        AddControlToTable(tlpBasic, 2, "資産区分", cmbAssetType, 3)
        ' 4行目: 部署
        AddControlToTable(tlpBasic, 3, "管理部署", cmbDeptAdmin)
        AddControlToTable(tlpBasic, 3, "負担部署", cmbDeptCost, 0, 2)

        grpBasic.Controls.Add(tlpBasic)
        pnlLeft.Controls.Add(grpBasic)

        ' B. 貸主情報
        Dim grpLessor As GroupBox = CreateGroupBox("貸主・相手先")
        Dim tlpLessor As New TableLayoutPanel() With {.Dock = DockStyle.Top, .AutoSize = True, .ColumnCount = 4, .RowCount = 3}
        SetupTableColumns(tlpLessor)

        txtLessorName = New TextBox() With {.Dock = DockStyle.Fill}
        txtLessorCode = New TextBox() With {.Dock = DockStyle.Fill}
        txtLessorAddress = New TextBox() With {.Dock = DockStyle.Fill}
        txtLessorTel = New TextBox() With {.Dock = DockStyle.Fill}
        txtBankInfo = New TextBox() With {.Dock = DockStyle.Fill}

        AddControlToTable(tlpLessor, 0, "貸主名", txtLessorName)
        AddControlToTable(tlpLessor, 0, "コード", txtLessorCode, 0, 2)
        AddControlToTable(tlpLessor, 1, "住所", txtLessorAddress, 3)
        AddControlToTable(tlpLessor, 2, "電話", txtLessorTel)
        AddControlToTable(tlpLessor, 2, "振込先", txtBankInfo, 0, 2)

        grpLessor.Controls.Add(tlpLessor)
        pnlLeft.Controls.Add(grpLessor)

        ' C. 契約期間・条件
        Dim grpTerm As GroupBox = CreateGroupBox("契約期間・条件")
        Dim tlpTerm As New TableLayoutPanel() With {.Dock = DockStyle.Top, .AutoSize = True, .ColumnCount = 4, .RowCount = 3}
        SetupTableColumns(tlpTerm)

        dtpContractDate = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .ShowCheckBox = True}
        dtpStartTab1 = New DateTimePicker() With {.Format = DateTimePickerFormat.Short}
        dtpEndTab1 = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .ShowCheckBox = True}
        chkAutoUpdate = New CheckBox() With {.Text = "自動更新", .AutoSize = True}
        dtpNoticeDate = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .ShowCheckBox = True}
        numUpdateCount = New NumericUpDown() With {.TextAlign = HorizontalAlignment.Right}

        AddControlToTable(tlpTerm, 0, "締結日", dtpContractDate)
        AddControlToTable(tlpTerm, 1, "開始日", dtpStartTab1)
        AddControlToTable(tlpTerm, 1, "終了日", dtpEndTab1, 0, 2)
        AddControlToTable(tlpTerm, 2, "更新条項", chkAutoUpdate)
        AddControlToTable(tlpTerm, 2, "通知期限", dtpNoticeDate, 0, 2)

        grpTerm.Controls.Add(tlpTerm)
        pnlLeft.Controls.Add(grpTerm)

        ' 順序整列 (下から順に追加したことにならぬよう)
        grpBasic.BringToFront()
        grpLessor.BringToFront()
        grpTerm.BringToFront()


        ' ---------------------------------------------------------
        ' [右カラム] 物件詳細・備考
        ' ---------------------------------------------------------
        Dim pnlRight As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(10, 0, 0, 0), .AutoSize = True}

        ' --- D. 物件明細 (論点整理D対応: ステータス管理を追加) ---
        Dim grpProp As GroupBox = CreateGroupBox("物件・資産明細（複数登録可）")
        grpProp.Height = 350
        grpProp.AutoSize = False

        dgvProperties = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = True
        }

        ' 列定義の強化
        With dgvProperties.Columns
            .Add("Name", "物件名/資産名")
            .Add("Loc", "設置場所")
            .Add("Spec", "構造/摘要")
            .Add("Qty", "数量/面積")

            ' ★追加: 部分解約(論点D)に対応するための列
            Dim colStatus As New DataGridViewComboBoxColumn() With {
                .HeaderText = "状態",
                .Name = "Status"
            }
            colStatus.Items.AddRange("契約中", "返却済", "一部解約")
            .Add(colStatus)

            .Add("ReturnDate", "解約/返却日")
        End With

        ' サンプルデータ (PCの一部返却などを想定)
        dgvProperties.Rows.Add("NotePC Latitude 5320", "本社3F", "リース資産", "3台", "契約中", "")
        dgvProperties.Rows.Add("NotePC Latitude 5320", "大阪支店", "リース資産", "1台", "返却済", "2026/09/30")

        grpProp.Controls.Add(dgvProperties)
        pnlRight.Controls.Add(grpProp)

        ' --- E. 特記事項 ---
        Dim grpMemo As GroupBox = CreateGroupBox("特記事項")
        grpMemo.Height = 150
        grpMemo.AutoSize = False

        txtRemarks = New TextBox() With {.Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Vertical}
        grpMemo.Controls.Add(txtRemarks)
        pnlRight.Controls.Add(grpMemo)

        grpProp.BringToFront()
        grpMemo.BringToFront()

        ' --- 結合 ---
        splitTbl.Controls.Add(pnlLeft, 0, 0)
        splitTbl.Controls.Add(pnlRight, 1, 0)
        pnlScroll.Controls.Add(splitTbl)
        pgContract.Controls.Add(pnlScroll)
    End Sub




    ' 内部テーブル作成 (4列固定)
    Private Function CreateInnerTable() As TableLayoutPanel
        Dim tlp As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 4,
            .RowCount = 0,
            .Padding = New Padding(5)
        }
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        Return tlp
    End Function

    ' 行追加 (列結合対応)
    Private Sub AddRow(tlp As TableLayoutPanel, lbl1 As String, ctrl1 As Control, Optional lbl2 As String = "", Optional ctrl2 As Control = Nothing, Optional span1 As Integer = 1)
        tlp.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
        Dim r = tlp.RowCount

        ' 1列目: ラベル
        tlp.Controls.Add(New Label() With {.Text = lbl1, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill, .ForeColor = Color.DimGray}, 0, r)

        ' 2列目: コントロール
        If ctrl1 IsNot Nothing Then
            tlp.Controls.Add(ctrl1, 1, r)
            If span1 > 1 Then tlp.SetColumnSpan(ctrl1, span1)
        End If

        ' 3-4列目
        If Not String.IsNullOrEmpty(lbl2) Then
            tlp.Controls.Add(New Label() With {.Text = lbl2, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill, .ForeColor = Color.DimGray}, 2, r)
            If ctrl2 IsNot Nothing Then tlp.Controls.Add(ctrl2, 3, r)
        ElseIf ctrl2 IsNot Nothing Then
            ' 右ラベルなしで何か置く場合
            tlp.Controls.Add(ctrl2, 3, r)
        End If

        tlp.RowCount += 1
    End Sub

    ' ★1行に1項目を長く配置するヘルパー (例: 契約名、物件名)
    Private Sub AddRowSingle(tlp As TableLayoutPanel, lbl1 As String, ctrl1 As Control)
        tlp.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
        Dim r = tlp.RowCount

        tlp.Controls.Add(New Label() With {.Text = lbl1, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill, .ForeColor = Color.DimGray}, 0, r)

        If ctrl1 IsNot Nothing Then
            tlp.Controls.Add(ctrl1, 1, r)
            ' 3列分結合して右端まで伸ばす
            tlp.SetColumnSpan(ctrl1, 3)
        End If

        tlp.RowCount += 1
    End Sub
    ' --- 以下、レイアウト崩れを防ぐためのヘルパーメソッド ---




    ' =========================================================================
    ' [修正提案] Tab2: 初回一時金 (新基準対応強化版)
    ' =========================================================================
    ' =========================================================================
    ' [最終修正版] Tab2: 初回一時金
    '  - 論点整理B (遡及修正) 対応: 「状態」列を追加し、確定データの改ざんを防止
    '  - 新基準対応: 「資産計上」フラグ等は維持
    ' =========================================================================
    Private Sub InitTabInitial()
        Dim pnl As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(10)}

        ' 説明ラベル
        Dim lblDesc As New Label() With {
            .Text = "※「状態」が[確定]の行は、既に仕訳連携済みのため変更できません（論点整理B準拠）。" & vbCrLf &
                    "　修正が必要な場合は、赤黒訂正または調整仕訳の処理が必要です。",
            .Dock = DockStyle.Top, .Height = 40, .ForeColor = Color.DarkBlue, .AutoSize = True
        }

        dgvInitialPay = New DataGridView() With {
            .Dock = DockStyle.Fill, .BackgroundColor = Color.White,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        ' 列定義
        With dgvInitialPay.Columns
            ' ★追加: 論点B対応 (ステータス管理)
            Dim colStatus As New DataGridViewComboBoxColumn() With {.HeaderText = "状態", .Name = "Status"}
            colStatus.Items.AddRange("未済", "確定")
            .Add(colStatus)

            Dim colType As New DataGridViewComboBoxColumn() With {.HeaderText = "種別", .Name = "Type"}
            colType.Items.AddRange("敷金", "礼金", "仲介手数料", "前払リース料", "その他")
            .Add(colType)

            .Add("Amount", "支払額(税抜)")
            .Add("Tax", "消費税")

            Dim colInc As New DataGridViewCheckBoxColumn() With {.HeaderText = "資産計上", .Name = "IsAsset", .TrueValue = True, .FalseValue = False}
            .Add(colInc)

            .Add("PayDate", "支払日")
            .Add("Memo", "摘要/返還条件")
        End With

        ' サンプルデータ (確定データが含まれる想定)
        dgvInitialPay.Rows.Add("確定", "敷金", "1,000,000", "0", False, "2026/04/01", "全額返還予定")
        dgvInitialPay.Rows.Add("確定", "仲介手数料", "100,000", "10,000", True, "2026/03/15", "取得原価算入")
        dgvInitialPay.Rows.Add("未済", "礼金", "200,000", "20,000", True, "2026/04/01", "未払計上待ち")

        ' 確定行の背景色を変える処理（疑似的）
        dgvInitialPay.Rows(0).DefaultCellStyle.BackColor = Color.LightGray
        dgvInitialPay.Rows(0).ReadOnly = True
        dgvInitialPay.Rows(1).DefaultCellStyle.BackColor = Color.LightGray
        dgvInitialPay.Rows(1).ReadOnly = True

        pnl.Controls.Add(dgvInitialPay)
        pnl.Controls.Add(lblDesc)
        pgInitial.Controls.Add(pnl)
    End Sub
    ' =========================================================================
    ' [ASBJ第34号対応] Tab3: リース判定
    '  - HTMLモックアップ (lease_checker_gem5.html) のUIとロジックを完全再現
    '  - 識別判定フロー (Q1~Q4) と免除規定判定を実装
    ' =========================================================================
    Private Sub InitTabJudge_Pro()
        Dim clrHeader As Color = ColorTranslator.FromHtml("#fce4d6")
        Dim clrSectionBg As Color = ColorTranslator.FromHtml("#44546a")
        Dim clrBorder As Color = ColorTranslator.FromHtml("#7f7f7f")

        Dim pnlScroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .Padding = New Padding(15)}

        Dim rootLayout As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 1,
            .RowCount = 4,
            .Padding = New Padding(0)
        }
        rootLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        ' =============================================================
        ' セクション1: ASBJ第34号 識別判定
        ' =============================================================
        Dim lblSec1 As New Label() With {
            .Text = "＜ASBJ第34号 識別判定＞",
            .Dock = DockStyle.Fill,
            .BackColor = clrSectionBg,
            .ForeColor = Color.White,
            .Font = New Font(Me.Font, FontStyle.Bold),
            .Padding = New Padding(10, 3, 0, 3),
            .AutoSize = True
        }
        rootLayout.Controls.Add(lblSec1, 0, 0)

        Dim tlpIdent As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 4,
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Margin = New Padding(0, 0, 0, 15)
        }
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 180.0F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))
        tlpIdent.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))

        ' --- Q1. 資産の特定 (2行) ---
        Dim lblQ1 As New Label() With {.Text = "Q1. 資産の特定", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpIdent.Controls.Add(lblQ1, 0, 0)
        tlpIdent.SetRowSpan(lblQ1, 2)

        grpQ1 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 0), .AutoSize = True}
        Dim flowQ1 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ1Yes = New RadioButton() With {.Text = "あり (特定されている)", .AutoSize = True}
        rbQ1No = New RadioButton() With {.Text = "なし", .AutoSize = True}
        AddHandler rbQ1Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ1No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ1.Controls.AddRange({rbQ1Yes, rbQ1No})
        grpQ1.Controls.Add(flowQ1)
        tlpIdent.Controls.Add(grpQ1, 1, 0)
        tlpIdent.SetColumnSpan(grpQ1, 3)

        txtQ1Memo = New TextBox() With {.Dock = DockStyle.Fill, .PlaceholderText = "備考：製造番号、設置場所など、物理的に特定できる情報を入力"}
        tlpIdent.Controls.Add(txtQ1Memo, 1, 1)
        tlpIdent.SetColumnSpan(txtQ1Memo, 3)

        ' --- Q2. 実質的な代替権 ---
        Dim lblQ2 As New Label() With {.Text = "Q2. 実質的な代替権", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpIdent.Controls.Add(lblQ2, 0, 2)

        Dim pnlQ2Cell As New Panel() With {.Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(4)}
        grpQ2 = New GroupBox() With {.Dock = DockStyle.Top, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 0), .AutoSize = True}
        Dim flowQ2 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ2Yes = New RadioButton() With {.Text = "あり (サプライヤーの権利)", .AutoSize = True}
        rbQ2No = New RadioButton() With {.Text = "なし", .AutoSize = True, .Checked = True}
        AddHandler rbQ2Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ2No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ2.Controls.AddRange({rbQ2Yes, rbQ2No})
        grpQ2.Controls.Add(flowQ2)
        Dim lblQ2Note As New Label() With {.Text = "※サプライヤーが経済的利益を得るために、資産を自由に他のものと入れ替える権利を持つ場合は「あり」", .AutoSize = True, .ForeColor = Color.Gray, .Font = New Font(Me.Font.FontFamily, 8.5F), .Dock = DockStyle.Top}
        pnlQ2Cell.Controls.Add(lblQ2Note)
        pnlQ2Cell.Controls.Add(grpQ2)
        tlpIdent.Controls.Add(pnlQ2Cell, 1, 2)
        tlpIdent.SetColumnSpan(pnlQ2Cell, 3)

        ' --- Q3. 経済的利益 & Q4. 使用指図権 (同一行) ---
        Dim lblQ3 As New Label() With {.Text = "Q3. 経済的利益の享受", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpIdent.Controls.Add(lblQ3, 0, 3)

        grpQ3 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 0), .AutoSize = True}
        Dim flowQ3 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ3Yes = New RadioButton() With {.Text = "あり", .AutoSize = True, .Checked = True}
        rbQ3No = New RadioButton() With {.Text = "なし", .AutoSize = True}
        AddHandler rbQ3Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ3No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ3.Controls.AddRange({rbQ3Yes, rbQ3No})
        grpQ3.Controls.Add(flowQ3)
        tlpIdent.Controls.Add(grpQ3, 1, 3)

        Dim lblQ4Hdr As New Label() With {.Text = "Q4. 使用指図権", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpIdent.Controls.Add(lblQ4Hdr, 2, 3)

        grpQ4 = New GroupBox() With {.Dock = DockStyle.Fill, .FlatStyle = FlatStyle.Flat, .Text = "", .Margin = New Padding(0), .Padding = New Padding(4, 0, 0, 0), .AutoSize = True}
        Dim flowQ4 As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        rbQ4Yes = New RadioButton() With {.Text = "あり", .AutoSize = True, .Checked = True}
        rbQ4No = New RadioButton() With {.Text = "なし", .AutoSize = True}
        AddHandler rbQ4Yes.CheckedChanged, AddressOf OnJudgeTrigger
        AddHandler rbQ4No.CheckedChanged, AddressOf OnJudgeTrigger
        flowQ4.Controls.AddRange({rbQ4Yes, rbQ4No})
        grpQ4.Controls.Add(flowQ4)
        tlpIdent.Controls.Add(grpQ4, 3, 3)

        rootLayout.Controls.Add(tlpIdent, 0, 1)

        ' =============================================================
        ' セクション2: リース期間・免除規定
        ' =============================================================
        Dim lblSec2 As New Label() With {
            .Text = "＜リース期間・免除規定＞",
            .Dock = DockStyle.Fill,
            .BackColor = clrSectionBg,
            .ForeColor = Color.White,
            .Font = New Font(Me.Font, FontStyle.Bold),
            .Padding = New Padding(10, 3, 0, 3),
            .AutoSize = True
        }

        Dim tlpExempt As New TableLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .ColumnCount = 6,
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            .Margin = New Padding(0, 0, 0, 10)
        }
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 150.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 110.0F))
        tlpExempt.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))

        ' Row 0: 開始日 / 終了日 / 見積期間
        Dim lblStart As New Label() With {.Text = "開始日", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblStart, 0, 0)
        dtpJudgeStart = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Value = New DateTime(2024, 7, 24)}
        AddHandler dtpJudgeStart.ValueChanged, AddressOf OnJudgeTrigger
        tlpExempt.Controls.Add(dtpJudgeStart, 1, 0)

        Dim lblEnd As New Label() With {.Text = "終了日", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblEnd, 2, 0)
        dtpJudgeEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Fill, .Value = New DateTime(2026, 7, 23)}
        AddHandler dtpJudgeEnd.ValueChanged, AddressOf OnJudgeTrigger
        tlpExempt.Controls.Add(dtpJudgeEnd, 3, 0)

        Dim lblTermHdr As New Label() With {.Text = "見積期間 (月)", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblTermHdr, 4, 0)

        Dim pnlTermCell As New Panel() With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(249, 249, 249), .Padding = New Padding(4)}
        lblTermMonths = New Label() With {.Text = "24", .Font = New Font(Me.Font.FontFamily, 11.0F, FontStyle.Bold), .AutoSize = True, .Dock = DockStyle.Left}
        Dim lblMonthUnit As New Label() With {.Text = " ヶ月", .AutoSize = True, .Dock = DockStyle.Left, .Padding = New Padding(0, 4, 0, 0)}
        lblDateError = New Label() With {.Text = "", .ForeColor = Color.FromArgb(217, 83, 79), .Font = New Font(Me.Font, FontStyle.Bold), .AutoSize = True, .Dock = DockStyle.Bottom}
        pnlTermCell.Controls.Add(lblDateError)
        pnlTermCell.Controls.Add(lblMonthUnit)
        pnlTermCell.Controls.Add(lblTermMonths)
        tlpExempt.Controls.Add(pnlTermCell, 5, 0)

        ' Row 1: 延長オプション / 延長期間 / 短期リース判定
        Dim lblExtHdr As New Label() With {.Text = "延長オプション", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblExtHdr, 0, 1)

        Dim pnlExtCell As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(4), .AutoSize = True}
        Dim flowExt As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        chkExtOption = New CheckBox() With {.Text = "あり", .AutoSize = True}
        Dim lblCert As New Label() With {.Text = "(確実性: ", .AutoSize = True, .Padding = New Padding(5, 4, 0, 0)}
        cboExtCertainty = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Width = 130, .Enabled = False}
        cboExtCertainty.Items.AddRange({"低い(行使しない)", "高い(行使する)"})
        cboExtCertainty.SelectedIndex = 0
        Dim lblCertEnd As New Label() With {.Text = ")", .AutoSize = True, .Padding = New Padding(0, 4, 0, 0)}
        AddHandler chkExtOption.CheckedChanged, AddressOf OnExtOptionChanged
        AddHandler cboExtCertainty.SelectedIndexChanged, AddressOf OnJudgeTrigger
        flowExt.Controls.AddRange({chkExtOption, lblCert, cboExtCertainty, lblCertEnd})
        pnlExtCell.Controls.Add(flowExt)
        tlpExempt.Controls.Add(pnlExtCell, 1, 1)

        Dim lblExtMonthHdr As New Label() With {.Text = "延長期間 (月)", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblExtMonthHdr, 2, 1)
        numExtMonths = New NumericUpDown() With {.Dock = DockStyle.Fill, .Minimum = 0, .Maximum = 600, .Value = 0, .Enabled = False}
        AddHandler numExtMonths.ValueChanged, AddressOf OnJudgeTrigger
        tlpExempt.Controls.Add(numExtMonths, 3, 1)

        Dim lblShortHdr As New Label() With {.Text = "短期リース判定", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblShortHdr, 4, 1)
        lblShortTermResult = New Label() With {.Text = "-", .Dock = DockStyle.Fill, .BackColor = Color.FromArgb(249, 249, 249), .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblShortTermResult, 5, 1)

        ' Row 2: 取得価額 / 少額基準額 / 少額資産判定
        Dim lblAssetHdr As New Label() With {.Text = "取得価額 (新品時)", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblAssetHdr, 0, 2)

        Dim pnlAssetCell As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(4), .AutoSize = True}
        Dim flowAsset As New FlowLayoutPanel() With {.Dock = DockStyle.Top, .AutoSize = True, .WrapContents = False}
        numAssetValue = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 140, .TextAlign = HorizontalAlignment.Right}
        AddHandler numAssetValue.ValueChanged, AddressOf OnJudgeTrigger
        Dim lblYen As New Label() With {.Text = "円", .AutoSize = True, .Padding = New Padding(5, 4, 0, 0)}
        flowAsset.Controls.AddRange({numAssetValue, lblYen})
        Dim lblAssetNote As New Label() With {.Text = "※不明な場合は公正価値等の見積額", .AutoSize = True, .ForeColor = Color.Gray, .Font = New Font(Me.Font.FontFamily, 8.5F), .Dock = DockStyle.Top}
        pnlAssetCell.Controls.Add(lblAssetNote)
        pnlAssetCell.Controls.Add(flowAsset)
        tlpExempt.Controls.Add(pnlAssetCell, 1, 2)

        Dim lblLowHdr As New Label() With {.Text = "少額基準額", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblLowHdr, 2, 2)
        Dim lblLowVal As New Label() With {.Text = "3,000,000 円", .Dock = DockStyle.Fill, .BackColor = Color.FromArgb(249, 249, 249), .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblLowVal, 3, 2)

        Dim lblLowJudgeHdr As New Label() With {.Text = "少額資産判定", .BackColor = clrHeader, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblLowJudgeHdr, 4, 2)
        lblLowValueResult = New Label() With {.Text = "-", .Dock = DockStyle.Fill, .BackColor = Color.FromArgb(249, 249, 249), .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4)}
        tlpExempt.Controls.Add(lblLowValueResult, 5, 2)

        ' Row 3: 免除規定の適用 (ColSpan)
        Dim lblExemptHdr As New Label() With {.Text = "免除規定の適用", .BackColor = Color.FromArgb(255, 204, 204), .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(4), .Font = New Font(Me.Font, FontStyle.Bold)}
        tlpExempt.Controls.Add(lblExemptHdr, 0, 3)

        Dim pnlExemptCell As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(4), .AutoSize = True}
        chkApplyExemption = New CheckBox() With {.Text = "短期または少額の免除規定を適用する (オフバランス処理)", .AutoSize = True, .Font = New Font(Me.Font, FontStyle.Bold), .Enabled = False, .Dock = DockStyle.Top}
        AddHandler chkApplyExemption.CheckedChanged, AddressOf OnJudgeTrigger
        Dim lblExemptNote As New Label() With {.Text = "※適用条件（期間12ヶ月以内 または 少額資産）を満たす場合のみ選択可能です。", .AutoSize = True, .ForeColor = Color.Gray, .Font = New Font(Me.Font.FontFamily, 8.5F), .Dock = DockStyle.Top}
        pnlExemptCell.Controls.Add(lblExemptNote)
        pnlExemptCell.Controls.Add(chkApplyExemption)
        tlpExempt.Controls.Add(pnlExemptCell, 1, 3)
        tlpExempt.SetColumnSpan(pnlExemptCell, 5)

        ' セクション2ヘッダーと免除規定テーブルをまとめるパネル
        Dim pnlSec2 As New Panel() With {.Dock = DockStyle.Top, .AutoSize = True}
        pnlSec2.Controls.Add(tlpExempt)
        pnlSec2.Controls.Add(lblSec2)
        rootLayout.Controls.Add(pnlSec2, 0, 2)

        ' =============================================================
        ' セクション3: 判定結果パネル
        ' =============================================================
        pnlResult = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 80,
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.FromArgb(240, 244, 255),
            .Padding = New Padding(10),
            .Margin = New Padding(0, 10, 0, 0)
        }

        Dim tlpResult As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 2}
        tlpResult.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpResult.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        tlpResult.RowStyles.Add(New RowStyle(SizeType.Percent, 60.0F))
        tlpResult.RowStyles.Add(New RowStyle(SizeType.Percent, 40.0F))

        Dim lblResultTitle As New Label() With {.Text = "現在の判定結果：", .Font = New Font(Me.Font, FontStyle.Bold), .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}
        tlpResult.Controls.Add(lblResultTitle, 0, 0)
        tlpResult.SetRowSpan(lblResultTitle, 2)

        Dim flowResultTop As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .WrapContents = False}
        lblResultText = New Label() With {.Text = "---", .Font = New Font(Me.Font.FontFamily, 16.0F, FontStyle.Bold), .AutoSize = True, .ForeColor = Color.FromArgb(51, 51, 51)}
        lblResultBadge = New Label() With {.Text = "---", .AutoSize = True, .ForeColor = Color.White, .BackColor = Color.FromArgb(204, 204, 204), .Padding = New Padding(6, 3, 6, 3), .Margin = New Padding(15, 4, 0, 0), .Font = New Font(Me.Font.FontFamily, 9.0F)}
        flowResultTop.Controls.AddRange({lblResultText, lblResultBadge})
        tlpResult.Controls.Add(flowResultTop, 1, 0)

        lblResultReason = New Label() With {.Text = "判定条件を入力してください。", .AutoSize = True, .ForeColor = Color.FromArgb(85, 85, 85), .Font = New Font(Me.Font.FontFamily, 8.5F), .Dock = DockStyle.Fill}
        tlpResult.Controls.Add(lblResultReason, 1, 1)

        pnlResult.Controls.Add(tlpResult)
        rootLayout.Controls.Add(pnlResult, 0, 3)

        pnlScroll.Controls.Add(rootLayout)
        pgJudge.Controls.Add(pnlScroll)
    End Sub

    ' =========================================================================
    ' [最終強化版] Tab4: 会計入力 (論点整理A/B/D対応)
    '  - 状態管理(確定/未済)の追加
    '  - 調整額(Catch-up)列の追加
    '  - 条件変更・中途解約ボタンの配置
    ' =========================================================================
    Private Sub InitTabAccounting()
        Dim pnlMain As New Panel() With {.Dock = DockStyle.Fill, .Padding = New Padding(10)}

        ' ---------------------------------------------------------
        ' 1. 条件入力・操作エリア (上部)
        ' ---------------------------------------------------------
        Dim grpCond As New GroupBox() With {
            .Text = "賃料条件・契約操作",
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .Padding = New Padding(5)
        }

        Dim flowCond As New FlowLayoutPanel() With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .Padding = New Padding(0, 5, 0, 5)
        }

        ' 基本入力
        Dim numRent As New NumericUpDown() With {.Maximum = 99999999999D, .ThousandsSeparator = True, .Width = 100, .TextAlign = HorizontalAlignment.Right}
        Dim cboCycle As New ComboBox() : cboCycle.Items.AddRange({"毎月", "3ヶ月", "半年", "年払"}) : cboCycle.SelectedIndex = 0
        Dim cboTiming As New ComboBox() : cboTiming.Items.AddRange({"前払(期首)", "後払(期末)"}) : cboTiming.SelectedIndex = 1
        Dim dtpSchStart As New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = 100}
        Dim dtpSchEnd As New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = 100}

        ' 操作ボタン群 (PDF要件対応)
        Dim btnGen As New Button() With {.Text = "新規作成", .BackColor = Color.LightSkyBlue, .AutoSize = True}
        Dim btnChange As New Button() With {.Text = "条件変更", .BackColor = Color.LightYellow, .AutoSize = True} ' PDF-A対応
        Dim btnTerminate As New Button() With {.Text = "中途解約", .BackColor = Color.LightPink, .AutoSize = True}  ' PDF-D対応

        ' 配置
        flowCond.Controls.Add(New Label() With {.Text = "月額:", .AutoSize = True, .Padding = New Padding(0, 6, 0, 0)})
        flowCond.Controls.Add(numRent)
        flowCond.Controls.Add(New Label() With {.Text = "間隔:", .AutoSize = True, .Padding = New Padding(5, 6, 0, 0)})
        flowCond.Controls.Add(cboCycle)
        flowCond.Controls.Add(New Label() With {.Text = "時期:", .AutoSize = True, .Padding = New Padding(5, 6, 0, 0)})
        flowCond.Controls.Add(cboTiming)
        flowCond.Controls.Add(New Label() With {.Text = "期間:", .AutoSize = True, .Padding = New Padding(5, 6, 0, 0)})
        flowCond.Controls.Add(dtpSchStart)
        flowCond.Controls.Add(New Label() With {.Text = "～", .AutoSize = True, .Padding = New Padding(0, 6, 0, 0)})
        flowCond.Controls.Add(dtpSchEnd)

        ' ボタンエリアを少し離す
        flowCond.Controls.Add(New Label() With {.Width = 20})
        flowCond.Controls.Add(btnGen)
        flowCond.Controls.Add(btnChange)
        flowCond.Controls.Add(btnTerminate)

        grpCond.Controls.Add(flowCond)

        ' メッセージラベル (PDF-B 遡及修正の説明用)
        Dim lblMsg As New Label() With {
            .Text = "※過去の確定済み行は変更できません。修正がある場合は「調整額」にCatch-up仕訳が自動生成されます。",
            .Dock = DockStyle.Bottom, .ForeColor = Color.Red, .AutoSize = True, .Padding = New Padding(5)
        }
        grpCond.Controls.Add(lblMsg)

        pnlMain.Controls.Add(grpCond)

        ' ---------------------------------------------------------
        ' 2. スケジュールグリッド (下部)
        ' ---------------------------------------------------------
        Dim grpSch As New GroupBox() With {.Text = "リース料支払・償却スケジュール (利息法)", .Dock = DockStyle.Fill, .Padding = New Padding(5)}

        dgvSchedule = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .AllowUserToAddRows = False,
            .RowHeadersVisible = False
        }

        ' 列定義 (PDF要件を取り込み)
        With dgvSchedule.Columns
            .Add("Status", "状態")             ' PDF-B: 確定/未済の管理
            .Add("No", "回数")
            .Add("Date", "支払日")
            .Add("Amount", "支払額")
            .Add("Interest", "利息相当額")
            .Add("Principal", "元本返済額")
            .Add("Balance", "リース債務残高")
            .Add("Adjust", "調整額")           ' PDF-B: 遡及修正額 (Catch-up)
            .Add("Memo", "備考")               ' PDF-A: 端数調整などのメモ
        End With

        ' 見た目の調整
        dgvSchedule.Columns("Amount").DefaultCellStyle.Format = "N0"
        dgvSchedule.Columns("Interest").DefaultCellStyle.Format = "N0"
        dgvSchedule.Columns("Principal").DefaultCellStyle.Format = "N0"
        dgvSchedule.Columns("Balance").DefaultCellStyle.Format = "N0"
        dgvSchedule.Columns("Adjust").DefaultCellStyle.Format = "N0"
        dgvSchedule.Columns("Adjust").DefaultCellStyle.ForeColor = Color.Red ' 調整額を目立たせる

        ' サンプルデータ (PDF-A/Bの状況をシミュレーション)
        dgvSchedule.Rows.Add("確定", "1", "2026/04/30", "122,223", "2,000", "120,223", "2,800,000", "0", "")
        dgvSchedule.Rows.Add("未済", "2", "2026/05/31", "122,223", "1,900", "120,323", "2,679,677", "200,000", "遡及修正Catch-up")

        ' 確定行はグレーアウトする処理(疑似)
        dgvSchedule.Rows(0).DefaultCellStyle.BackColor = Color.LightGray
        dgvSchedule.Rows(0).ReadOnly = True

        grpSch.Controls.Add(dgvSchedule)
        pnlMain.Controls.Add(grpSch)

        grpSch.BringToFront()
        pgAccounting.Controls.Add(pnlMain)
    End Sub
    ' =========================================================================
    ' ヘルパー & イベントハンドラ
    ' =========================================================================

    ' =========================================================================
    ' 延長オプションのUI制御
    ' =========================================================================
    Private Sub OnExtOptionChanged(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        Dim isDisabled As Boolean = Not chkExtOption.Checked
        cboExtCertainty.Enabled = Not isDisabled
        numExtMonths.Enabled = Not isDisabled
        If isDisabled Then
            numExtMonths.Value = 0
        End If
        RecalcJudge()
    End Sub

    ' =========================================================================
    ' 判定トリガー (全コントロール共通)
    ' =========================================================================
    Private Sub OnJudgeTrigger(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        RecalcJudge()
    End Sub

    ' =========================================================================
    ' [ASBJ第34号対応] RecalcJudge - calculate() 関数の完全移植
    ' =========================================================================
    Private Sub RecalcJudge(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
        If lblResultText Is Nothing Then Return

        ' --- 1. 値取得 ---
        Dim q1Yes As Boolean = rbQ1Yes.Checked
        Dim q2No As Boolean = rbQ2No.Checked
        Dim q3Yes As Boolean = rbQ3Yes.Checked
        Dim q4Yes As Boolean = rbQ4Yes.Checked

        Dim startDt As DateTime = dtpJudgeStart.Value.Date
        Dim endDt As DateTime = dtpJudgeEnd.Value.Date
        Dim assetVal As Decimal = numAssetValue.Value

        Dim hasExt As Boolean = chkExtOption.Checked
        Dim isExtCertain As Boolean = (cboExtCertainty.SelectedIndex = 1)
        Dim extMonths As Integer = CInt(numExtMonths.Value)

        ' --- 2. リース定義判定 (Q1-Q4) ---
        Dim isLease As Boolean = (q1Yes AndAlso q2No AndAlso q3Yes AndAlso q4Yes)

        ' --- 3. 期間計算 ---
        Dim months As Integer = 0
        Dim isValidDate As Boolean = True

        If endDt < startDt Then
            months = 0
            lblDateError.Text = "終了日が開始日より前です"
            isValidDate = False
        Else
            lblDateError.Text = ""
            months = (endDt.Year - startDt.Year) * 12 + (endDt.Month - startDt.Month)
            If endDt.Day >= startDt.Day Then
                months += 1
            End If
            If hasExt AndAlso isExtCertain Then
                months += extMonths
            End If
        End If

        lblTermMonths.Text = months.ToString()

        ' --- 4. 免除規定要件チェック ---
        Dim isShortTerm As Boolean = (isValidDate AndAlso months > 0 AndAlso months <= 12)
        If isShortTerm Then
            lblShortTermResult.Text = "該当 (12ヶ月以内)"
            lblShortTermResult.ForeColor = Color.FromArgb(0, 123, 255)
            lblShortTermResult.Font = New Font(lblShortTermResult.Font, FontStyle.Bold)
        Else
            lblShortTermResult.Text = "非該当"
            lblShortTermResult.ForeColor = Color.FromArgb(51, 51, 51)
            lblShortTermResult.Font = New Font(lblShortTermResult.Font, FontStyle.Regular)
        End If

        Dim isLowValue As Boolean = (assetVal > 0 AndAlso assetVal <= LOW_VALUE_THRESHOLD)
        If assetVal > 0 Then
            If isLowValue Then
                lblLowValueResult.Text = "該当 (基準額以下)"
                lblLowValueResult.ForeColor = Color.FromArgb(0, 123, 255)
                lblLowValueResult.Font = New Font(lblLowValueResult.Font, FontStyle.Bold)
            Else
                lblLowValueResult.Text = "非該当"
                lblLowValueResult.ForeColor = Color.FromArgb(51, 51, 51)
                lblLowValueResult.Font = New Font(lblLowValueResult.Font, FontStyle.Regular)
            End If
        Else
            lblLowValueResult.Text = "-"
            lblLowValueResult.ForeColor = Color.FromArgb(51, 51, 51)
        End If

        ' --- 5. 免除規定チェックボックスの制御 ---
        If isLease AndAlso (isShortTerm OrElse isLowValue) Then
            chkApplyExemption.Enabled = True
        Else
            chkApplyExemption.Enabled = False
            chkApplyExemption.Checked = False
        End If

        ' --- 6. 最終結果判定 ---
        lblResultText.ForeColor = Color.FromArgb(51, 51, 51)
        lblResultBadge.BackColor = Color.FromArgb(204, 204, 204)

        If Not isLease Then
            lblResultText.Text = "対象外"
            lblResultBadge.Text = "リース資産計上不要"
            lblResultReason.Text = "Q1～Q4の条件を満たさないため、通常の賃貸借処理（オフバランス）となります。"
        Else
            If chkApplyExemption.Checked Then
                lblResultText.Text = "オフバランス処理"
                lblResultText.ForeColor = Color.FromArgb(0, 123, 255)
                lblResultBadge.Text = "免除規定適用"
                lblResultBadge.BackColor = Color.FromArgb(23, 162, 184)
                lblResultReason.Text = "短期または少額資産の免除規定を適用し、賃貸借処理として処理します。"
            Else
                lblResultText.Text = "オンバランス処理"
                lblResultText.ForeColor = Color.FromArgb(217, 83, 79)
                lblResultBadge.Text = "資産計上必須"
                lblResultBadge.BackColor = Color.FromArgb(40, 167, 69)
                lblResultReason.Text = "使用権資産およびリース負債の計上が必要です。"
            End If
        End If
    End Sub
    ' =========================================================================
    ' [統合版] 全タブ共通ヘルパーメソッド群
    ' ※クラスの末尾にある既存のヘルパー関数を全て消して、これに置き換えてください
    ' =========================================================================

    ' ---------------------------------------------------------
    ' 1. Tab1 (契約入力) 用ヘルパー
    ' ---------------------------------------------------------
    ' グループボックス作成
    Private Function CreateGroupBox(title As String) As GroupBox
        Return New GroupBox() With {
            .Text = title,
            .Dock = DockStyle.Top,
            .AutoSize = False,
            .Padding = New Padding(5),
            .Margin = New Padding(0, 0, 0, 15),
            .Height = 160
        }
    End Function

    ' テーブル列設定 (4列固定)
    Private Sub SetupTableColumns(tlp As TableLayoutPanel)
        tlp.ColumnStyles.Clear()
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
    End Sub

    ' テーブルへのコントロール配置 (行・列を明示指定)
    Private Sub AddControlToTable(tlp As TableLayoutPanel, row As Integer, label As String, ctrl As Control, Optional span As Integer = 1, Optional colOffset As Integer = 0)
        If tlp.RowStyles.Count <= row Then
            tlp.RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
        End If

        Dim lbl As New Label() With {.Text = label, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill, .ForeColor = Color.DimGray}
        tlp.Controls.Add(lbl, 0 + colOffset, row)

        If ctrl IsNot Nothing Then
            tlp.Controls.Add(ctrl, 1 + colOffset, row)
            If span > 1 Then tlp.SetColumnSpan(ctrl, span)
        End If
    End Sub

End Class
