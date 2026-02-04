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
    ' Tab3: リース判定用 変数 (FrmLeaseJudgment_Pro 完全移植)
    ' -------------------------------------------------------------------------
    Private rootJudge As TableLayoutPanel
    Private top3 As TableLayoutPanel

    ' 左：契約・期間
    Private grpTerm As GroupBox, tlpTerm As TableLayoutPanel
    Private txtContractNo As TextBox
    Private dtpStart As DateTimePicker
    Private numPeriod As NumericUpDown
    Private dtpEnd As DateTimePicker
    Private numNonCancelable As NumericUpDown

    ' 中：判定要素
    Private grpJudge As GroupBox, tlpJudge As TableLayoutPanel
    Private chkTransfer As CheckBox
    Private chkBargain As CheckBox
    Private numExercisePrice As NumericUpDown
    Private chkSpecialized As CheckBox
    Private chkShortTerm As CheckBox
    Private chkLowValue As CheckBox
    Private numPVpct As NumericUpDown
    Private numLifePct As NumericUpDown
    Private lblNcNote As Label

    ' 右：支払
    Private grpPay As GroupBox, tlpPay As TableLayoutPanel
    Private numPay As NumericUpDown
    Private dtpFirst As DateTimePicker
    Private numInterval As NumericUpDown
    Private numCount As NumericUpDown
    Private dtpLast As DateTimePicker
    Private cboIndex As ComboBox
    Private numIndexAtContract As NumericUpDown
    Private numIndexCurrent As NumericUpDown

    ' 下段：IFRS詳細
    Private grpAcc As GroupBox, adv3 As TableLayoutPanel
    Private numImplicitRate As NumericUpDown, numIBR As NumericUpDown
    Private chkHasExtend As CheckBox, chkRcExtend As CheckBox
    Private numExtendMonths As NumericUpDown, numExtendPay As NumericUpDown
    Private chkHasTerminate As CheckBox, chkRcTerminate As CheckBox
    Private chkVariableNonIndex As CheckBox, chkPerformanceLinked As CheckBox, numNonLeaseComp As NumericUpDown
    Private numGRV As NumericUpDown, chkGRVApplicable As CheckBox, numGRVPayment As NumericUpDown
    Private numPrepaid As NumericUpDown, numIDC As NumericUpDown, numIncentive As NumericUpDown, numARO As NumericUpDown
    Private chkSaleLeaseback As CheckBox, chkSublease As CheckBox

    ' 判定結果
    Private grpResult As GroupBox
    Private lblResult As Label

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
    ' [修正版] Tab3: リース判定 (レイアウト崩れ防止版)
    '  - パーセント指定を廃止し、中身に合わせて高さが自動調整されるように修正
    '  - 最大化時や低解像度時でも見切れず、必要に応じてスクロールバーが出るように変更
    ' =========================================================================
    Private Sub InitTabJudge_Pro()
        ' 1. 全体をスクロール可能にするパネル (画面が小さくても切れないようにする)
        Dim pnlScroll As New Panel() With {.Dock = DockStyle.Fill, .AutoScroll = True}

        ' 2. メインレイアウト (上から順に積み上げる設定)
        rootJudge = New TableLayoutPanel() With {
            .Dock = DockStyle.Top,  ' FillではなくTopにして、上から順に配置
            .AutoSize = True,       ' 中身に合わせて高さを自動で伸ばす
            .ColumnCount = 1,
            .RowCount = 3,
            .Padding = New Padding(4)
        }
        ' 全行を AutoSize に設定 (中身の高さに合わせる)
        rootJudge.RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' 上段
        rootJudge.RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' 中段
        rootJudge.RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' 下段

        ' ---------------------------------------------------------
        ' [上段] 3カラム (契約・判定・支払)
        ' ---------------------------------------------------------
        top3 = New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,       ' ★重要: 中身に合わせて広げる
            .ColumnCount = 3,
            .RowCount = 1
        }
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 34.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))
        top3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0F))

        ' 左: 契約・期間
        grpTerm = New GroupBox() With {.Text = "契約・期間", .Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(6), .BackColor = Color.FromArgb(250, 250, 250)}
        tlpTerm = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 2}
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpTerm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        txtContractNo = New TextBox() With {.Width = 140, .Dock = DockStyle.Left}
        AddHandler txtContractNo.TextChanged, Sub(s, e) txtContractNoHdr.Text = txtContractNo.Text
        dtpStart = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Left, .Width = 120}
        numPeriod = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 24, .Dock = DockStyle.Left, .Width = 80}
        dtpEnd = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Dock = DockStyle.Left, .Width = 120, .Enabled = False}
        numNonCancelable = New NumericUpDown() With {.Minimum = 0, .Maximum = 1200, .Value = 6, .Dock = DockStyle.Left, .Width = 80}

        AddHandler dtpStart.ValueChanged, AddressOf OnTermChanged
        AddHandler numPeriod.ValueChanged, AddressOf OnTermChanged

        AddTermRow("契約番号", txtContractNo)
        AddTermRow("契約開始日", dtpStart)
        AddTermRow("契約期間(月)", numPeriod)
        AddTermRow("契約終了日", dtpEnd)
        AddTermRow("非解約期間(月)", numNonCancelable)
        grpTerm.Controls.Add(tlpTerm)

        ' 中: 判定要素 (論点C対応)
        grpJudge = New GroupBox() With {.Text = "判定要素・重要性基準(論点C)", .Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(6), .BackColor = Color.FromArgb(250, 250, 250)}
        tlpJudge = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 2}
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpJudge.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        Dim lblTotalCost As New Label() With {.Text = "4,200,000", .TextAlign = ContentAlignment.MiddleRight, .Font = New Font(Me.Font, FontStyle.Bold), .AutoSize = True}
        Dim lblThreshold As New Label() With {.Text = "(基準: 3,000,000)", .TextAlign = ContentAlignment.MiddleLeft, .ForeColor = Color.Gray, .AutoSize = True, .Padding = New Padding(5, 0, 0, 0)}
        Dim pnlCost As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlCost.Controls.Add(lblTotalCost) : pnlCost.Controls.Add(lblThreshold)
        AddJudgeRow("取得価額計", pnlCost)

        chkTransfer = New CheckBox() With {.Text = "所有権移転", .AutoSize = True}
        chkBargain = New CheckBox() With {.Text = "割安購入権", .AutoSize = True}
        numExercisePrice = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 90, .Enabled = False}
        AddHandler chkBargain.CheckedChanged, Sub(s, e) numExercisePrice.Enabled = chkBargain.Checked
        Dim pnlBargain As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlBargain.Controls.Add(chkBargain) : pnlBargain.Controls.Add(numExercisePrice)

        chkSpecialized = New CheckBox() With {.Text = "専用性/特定資産", .AutoSize = True}
        chkShortTerm = New CheckBox() With {.Text = "短期(≦12M)", .AutoSize = True}
        chkLowValue = New CheckBox() With {.Text = "少額", .AutoSize = True}
        Dim pnlEx As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlEx.Controls.Add(chkShortTerm) : pnlEx.Controls.Add(chkLowValue)

        lblNcNote = New Label() With {.Text = "", .ForeColor = SystemColors.GrayText, .AutoSize = True}
        numPVpct = New NumericUpDown() With {.DecimalPlaces = 1, .Width = 70}
        numLifePct = New NumericUpDown() With {.DecimalPlaces = 1, .Width = 70}

        AddHandler chkTransfer.CheckedChanged, AddressOf RecalcJudge
        AddHandler chkBargain.CheckedChanged, AddressOf RecalcJudge
        AddHandler chkSpecialized.CheckedChanged, AddressOf RecalcJudge
        AddHandler chkShortTerm.CheckedChanged, AddressOf RecalcJudge
        AddHandler chkLowValue.CheckedChanged, AddressOf RecalcJudge
        AddHandler numPVpct.ValueChanged, AddressOf RecalcJudge
        AddHandler numLifePct.ValueChanged, AddressOf RecalcJudge

        AddJudgeRow("移転条項", chkTransfer)
        AddJudgeRow("購入権利(行使額)", pnlBargain)
        AddJudgeRow("専用性", chkSpecialized)
        AddJudgeRow("免除規定", pnlEx)
        AddJudgeRow("（参考）", lblNcNote)
        AddJudgeRow("現在価値比率(%)", numPVpct)
        AddJudgeRow("耐用年数比率(%)", numLifePct)
        grpJudge.Controls.Add(tlpJudge)

        ' 右: 支払
        grpPay = New GroupBox() With {.Text = "支払（原契・代表）", .Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(6), .BackColor = Color.FromArgb(250, 250, 250)}
        tlpPay = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 2}
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        tlpPay.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        numPay = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 110}
        dtpFirst = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = 110}
        numInterval = New NumericUpDown() With {.Minimum = 1, .Value = 1, .Width = 60}
        numCount = New NumericUpDown() With {.Minimum = 1, .Value = 24, .Width = 60}
        dtpLast = New DateTimePicker() With {.Format = DateTimePickerFormat.Short, .Width = 110, .Enabled = False}
        cboIndex = New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Width = 80}
        cboIndex.Items.AddRange({"-", "CPI"})
        numIndexAtContract = New NumericUpDown() With {.DecimalPlaces = 2, .Width = 60, .Margin = New Padding(3, 4, 3, 3)}
        numIndexCurrent = New NumericUpDown() With {.DecimalPlaces = 2, .Width = 60, .Margin = New Padding(3, 4, 3, 3)}

        AddHandler dtpFirst.ValueChanged, AddressOf OnPayChanged
        AddHandler numInterval.ValueChanged, AddressOf OnPayChanged
        AddHandler numCount.ValueChanged, AddressOf OnPayChanged

        Dim pnlIdx As New FlowLayoutPanel() With {.AutoSize = True, .WrapContents = False, .Margin = New Padding(0)}
        pnlIdx.Controls.Add(numIndexAtContract)
        pnlIdx.Controls.Add(New Label() With {.Text = "→", .AutoSize = True, .Padding = New Padding(0, 8, 0, 0)})
        pnlIdx.Controls.Add(numIndexCurrent)

        AddPayRow("1回支払額(税抜)", numPay)
        AddPayRow("初回支払日", dtpFirst)
        AddPayRow("支払間隔(月)", numInterval)
        AddPayRow("総支払回数", numCount)
        AddPayRow("最終支払日", dtpLast)
        AddPayRow("連動指数", cboIndex)
        AddPayRow("契約時→現在", pnlIdx)
        grpPay.Controls.Add(tlpPay)

        top3.Controls.Add(grpTerm, 0, 0)
        top3.Controls.Add(grpJudge, 1, 0)
        top3.Controls.Add(grpPay, 2, 0)
        rootJudge.Controls.Add(top3, 0, 0)

        ' ---------------------------------------------------------
        ' [中段] 詳細パラメータ (新基準)
        ' ---------------------------------------------------------
        grpAcc = New GroupBox() With {.Text = "（経理入力）新リース会計基準 詳細パラメータ", .Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(6), .BackColor = Color.FromArgb(255, 247, 230)}
        adv3 = New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 3}
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))
        adv3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3F))

        ' L: 割引率
        Dim L As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 2}
        L.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        L.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        Dim hdrRate As Label = CreateHeader("割引率")
        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F)) : L.Controls.Add(hdrRate, 0, 0) : L.SetColumnSpan(hdrRate, 2)

        numImplicitRate = New NumericUpDown() With {.DecimalPlaces = 2, .Width = 60}
        numIBR = New NumericUpDown() With {.DecimalPlaces = 2, .Width = 60}
        Dim pnlRates As New FlowLayoutPanel() With {.AutoSize = True}
        pnlRates.Controls.AddRange({New Label() With {.Text = "暗黙", .AutoSize = True}, numImplicitRate, New Label() With {.Text = "IBR", .AutoSize = True}, numIBR})
        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F)) : L.Controls.Add(MakeLabel("年率(%)"), 0, 1) : L.Controls.Add(pnlRates, 1, 1)

        Dim hdrOpt As Label = CreateHeader("オプション")
        L.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F)) : L.Controls.Add(hdrOpt, 0, 2) : L.SetColumnSpan(hdrOpt, 2)

        chkHasExtend = New CheckBox() With {.Text = "延長OP", .AutoSize = True}
        chkRcExtend = New CheckBox() With {.Text = "確実", .AutoSize = True}
        numExtendMonths = New NumericUpDown() With {.Width = 50}
        numExtendPay = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 85}
        Dim pnlExt As New FlowLayoutPanel() With {.AutoSize = True, .FlowDirection = FlowDirection.TopDown}
        Dim pnlExtChk As New FlowLayoutPanel() With {.AutoSize = True} : pnlExtChk.Controls.AddRange({chkHasExtend, chkRcExtend})
        Dim pnlExtVal As New FlowLayoutPanel() With {.AutoSize = True} : pnlExtVal.Controls.AddRange({New Label() With {.Text = "月数", .AutoSize = True}, numExtendMonths, New Label() With {.Text = "月額", .AutoSize = True}, numExtendPay})
        pnlExt.Controls.AddRange({pnlExtChk, pnlExtVal})
        L.RowStyles.Add(New RowStyle(SizeType.AutoSize)) : L.Controls.Add(MakeLabel("延長"), 0, 3) : L.Controls.Add(pnlExt, 1, 3)

        chkHasTerminate = New CheckBox() With {.Text = "解約OP", .AutoSize = True}
        chkRcTerminate = New CheckBox() With {.Text = "行使しない", .AutoSize = True}
        Dim pnlTer As New FlowLayoutPanel() With {.AutoSize = True} : pnlTer.Controls.AddRange({chkHasTerminate, chkRcTerminate})
        L.RowStyles.Add(New RowStyle(SizeType.AutoSize)) : L.Controls.Add(MakeLabel("解約"), 0, 4) : L.Controls.Add(pnlTer, 1, 4)

        ' M: 変動/GRV
        Dim M As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 2}
        M.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F)) : M.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        Dim hdrVar As Label = CreateHeader("変動/非リース")
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F)) : M.Controls.Add(hdrVar, 0, 0) : M.SetColumnSpan(hdrVar, 2)
        chkVariableNonIndex = New CheckBox() With {.Text = "指数以外", .AutoSize = True}
        chkPerformanceLinked = New CheckBox() With {.Text = "業績連動", .AutoSize = True}
        Dim pnlVar As New FlowLayoutPanel() With {.AutoSize = True} : pnlVar.Controls.AddRange({chkVariableNonIndex, chkPerformanceLinked})
        numNonLeaseComp = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 100}
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F)) : M.Controls.Add(MakeLabel("区分"), 0, 1) : M.Controls.Add(pnlVar, 1, 1)
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F)) : M.Controls.Add(MakeLabel("非リース額"), 0, 2) : M.Controls.Add(numNonLeaseComp, 1, 2)

        Dim hdrGrv As Label = CreateHeader("残価保証(GRV)")
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F)) : M.Controls.Add(hdrGrv, 0, 3) : M.SetColumnSpan(hdrGrv, 2)
        chkGRVApplicable = New CheckBox() With {.Text = "適用", .AutoSize = True}
        numGRV = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 90}
        Dim pnlGrv1 As New FlowLayoutPanel() With {.AutoSize = True} : pnlGrv1.Controls.AddRange({chkGRVApplicable, numGRV})
        numGRVPayment = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 90}
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F)) : M.Controls.Add(MakeLabel("保証額"), 0, 4) : M.Controls.Add(pnlGrv1, 1, 4)
        M.RowStyles.Add(New RowStyle(SizeType.Absolute, 28.0F)) : M.Controls.Add(MakeLabel("支払見込"), 0, 5) : M.Controls.Add(numGRVPayment, 1, 5)

        ' R: 初期調整
        Dim R As New TableLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True, .ColumnCount = 4}
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 90.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20.0F))
        R.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        numPrepaid = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 95, .Dock = DockStyle.Left}
        numIDC = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 95, .Dock = DockStyle.Left}
        numIncentive = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 95, .Dock = DockStyle.Left}
        numARO = New NumericUpDown() With {.Maximum = 9999999999D, .ThousandsSeparator = True, .Width = 95, .Dock = DockStyle.Left}
        chkSaleLeaseback = New CheckBox() With {.Text = "S&LB", .AutoSize = True}
        chkSublease = New CheckBox() With {.Text = "転リース", .AutoSize = True}

        Dim hdrInit As Label = CreateHeader("初期調整")
        Dim hdrOptR As Label = CreateHeader("特例")
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0F))
        R.Controls.Add(hdrInit, 0, 0) : R.SetColumnSpan(hdrInit, 2) : R.Controls.Add(hdrOptR, 3, 0)
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F)) : R.Controls.Add(MakeLabel("前払"), 0, 1) : R.Controls.Add(numPrepaid, 1, 1) : R.Controls.Add(chkSaleLeaseback, 3, 1)
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F)) : R.Controls.Add(MakeLabel("IDC"), 0, 2) : R.Controls.Add(numIDC, 1, 2) : R.Controls.Add(chkSublease, 3, 2)
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F)) : R.Controls.Add(MakeLabel("インセン"), 0, 3) : R.Controls.Add(numIncentive, 1, 3)
        R.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F)) : R.Controls.Add(MakeLabel("除去債務"), 0, 4) : R.Controls.Add(numARO, 1, 4)

        adv3.Controls.Add(L, 0, 0) : adv3.Controls.Add(M, 1, 0) : adv3.Controls.Add(R, 2, 0)
        grpAcc.Controls.Add(adv3)
        rootJudge.Controls.Add(grpAcc, 1, 0)

        ' ---------------------------------------------------------
        ' [下段] 結果 (論点B対応)
        ' ---------------------------------------------------------
        grpResult = New GroupBox() With {.Text = "判定結果", .Dock = DockStyle.Fill, .AutoSize = True, .Padding = New Padding(10)}
        Dim pnlRes As New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoSize = True}

        lblResult = New Label() With {.AutoSize = True, .Font = New Font(Me.Font.FontFamily, 12.0F, FontStyle.Bold), .BackColor = Color.AliceBlue, .Text = "---"}
        Dim lblStatus As New Label() With {.Text = "【状態: 確定済】(変更不可)", .ForeColor = Color.Red, .Font = New Font(Me.Font, FontStyle.Bold), .AutoSize = True, .Margin = New Padding(20, 0, 0, 0), .Visible = True}

        pnlRes.Controls.Add(lblResult)
        pnlRes.Controls.Add(lblStatus)
        grpResult.Controls.Add(pnlRes)

        rootJudge.Controls.Add(grpResult, 2, 0)

        ' スクロールパネルにレイアウトを追加
        pnlScroll.Controls.Add(rootJudge)
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

    Private Sub OnTermChanged(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        dtpEnd.Value = dtpStart.Value.AddMonths(CInt(numPeriod.Value)).AddDays(-1)
        RecalcJudge()
    End Sub

    Private Sub OnPayChanged(sender As Object, e As EventArgs)
        If Not _isLoaded Then Return
        Dim k = Math.Max(1, CInt(numCount.Value))
        Dim m = Math.Max(1, CInt(numInterval.Value))
        dtpLast.Value = dtpFirst.Value.AddMonths(m * (k - 1))
        RecalcJudge()
    End Sub

    ' 簡易判定ロジック
    Private Sub RecalcJudge(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
        If lblResult Is Nothing Then Return

        ' 免除
        If chkShortTerm.Checked OrElse chkLowValue.Checked Then
            lblResult.Text = "判定：オフバランス（免除規定適用）"
            lblResult.ForeColor = Color.Green
            lblNcNote.Text = ""
            Return
        End If

        ' FL判定
        Dim isFL As Boolean = False
        Dim reason As String = ""
        If chkTransfer.Checked Then : isFL = True : reason = "移転"
        ElseIf chkBargain.Checked Then : isFL = True : reason = "割安購入"
        ElseIf chkSpecialized.Checked Then : isFL = True : reason = "専用性"
        ElseIf numPVpct.Value >= 90D Then : isFL = True : reason = "PV基準"
        ElseIf numLifePct.Value >= 75D Then : isFL = True : reason = "期間基準"
        End If

        Dim nc As Integer = CInt(numNonCancelable.Value)
        lblNcNote.Text = $"非解約: {nc}ヶ月"

        If isFL Then
            lblResult.Text = $"判定：ファイナンス・リース ({reason})" & vbCrLf & "資産・負債計上が必要です"
            lblResult.ForeColor = Color.Red
        Else
            lblResult.Text = "判定：オペレーティング・リース" & vbCrLf & "（IFRS16では原則オンバランス）"
            lblResult.ForeColor = Color.Blue
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

    ' ---------------------------------------------------------
    ' 2. Tab3 (リース判定) 用ヘルパー
    ' ---------------------------------------------------------
    ' 2列のテーブル作成 (左:ラベル, 右:入力)
    Private Function MakeSubTable() As TableLayoutPanel
        Dim t As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .AutoSize = True
        }
        t.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120.0F))
        t.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        Return t
    End Function

    ' ラベル作成 (右寄せ)
    Private Function MakeLabel(text As String) As Label
        Return New Label() With {
            .Text = text,
            .AutoSize = False,
            .TextAlign = ContentAlignment.MiddleRight,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(0, 5, 2, 5)
        }
    End Function

    ' 見出しラベル作成 (グレー背景)
    Private Function CreateHeader(text As String) As Label
        Return New Label() With {
            .Text = text,
            .Height = 22,
            .BackColor = Color.FromArgb(240, 240, 240),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font(Me.Font, FontStyle.Bold)
        }
    End Function

    ' 複数のコントロールを横に並べるパネル作成
    Private Function CreateFlow(ParamArray ctrls() As Control) As FlowLayoutPanel
        Dim f As New FlowLayoutPanel() With {
            .AutoSize = True,
            .Dock = DockStyle.Fill,
            .WrapContents = False,
            .Margin = New Padding(0)
        }
        f.Controls.AddRange(ctrls)
        Return f
    End Function

    ' 汎用: テーブルへの行追加
    Private Sub AddSubRow(t As TableLayoutPanel, lbl As String, c As Control)
        t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Dim r As Integer = t.RowCount

        t.Controls.Add(MakeLabel(lbl), 0, r)
        If c IsNot Nothing Then
            t.Controls.Add(c, 1, r)
        End If

        t.RowCount += 1
    End Sub

    ' --- 以下、各列専用ラッパー ---
    Private Sub AddTermRow(lbl As String, ctrl As Control)
        AddSubRow(tlpTerm, lbl, ctrl)
    End Sub
    Private Sub AddJudgeRow(lbl As String, ctrl As Control)
        AddSubRow(tlpJudge, lbl, ctrl)
    End Sub
    Private Sub AddPayRow(lbl As String, ctrl As Control)
        AddSubRow(tlpPay, lbl, ctrl)
    End Sub
End Class