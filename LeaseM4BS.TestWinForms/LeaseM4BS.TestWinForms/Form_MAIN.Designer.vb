<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_MAIN
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.台帳ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_CONTRACT_LIST = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_BUKN_LIST = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_HAIF = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_HENF = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_GSON = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_NEW_CONTRACT = New System.Windows.Forms.ToolStripMenuItem()
        Me.月次ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_TOUGETSU_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KEIJO_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.期間ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_TANA_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KLSRYO_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_IDOLST_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KHIYO_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_YOSAN_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.決算ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_CHUKI_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_ZANDAKA_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SAIMU_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_BEPPYO2_JOKEN = New System.Windows.Forms.ToolStripMenuItem()
        Me.マスタToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_CORP = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KKNRI = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LCPT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SHHO = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_GENK = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_BCAT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_BKNRI = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_HKMK = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SKMK = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_BKIND = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KOZA = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_GSHA = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_MCPT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_HKHO = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_RSRVH1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_RSRVB1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_KARI_RITU = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_ZEI_KAISEI = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_HREL = New System.Windows.Forms.ToolStripMenuItem()
        Me.一括更新ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.システムToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DB管理ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_SAVE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_RESTORE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_EXCEL_IMPORT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_CACHE_CLEAR = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_COMPACT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_CREATE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_DROP = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOCK_CLEAR = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_MIGRATE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_MNT_BKNRI = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_DB_EXPORT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_ENV_SETTING = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_TOUSEI_OPT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_PWD_CHANGE = New System.Windows.Forms.ToolStripMenuItem()
        Me.ログ管理ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOG_REF = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOG_UPD = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOG_SAVE_RESTORE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOG_DELETE = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SWKK_FIXED = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SEC_USER = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_SEC_KNGN = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_VERSION_INFO = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_LOGOUT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_EXIT = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_CHUKI_RECALC = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_IMPORT_CONTRACT_FROM_EXCEL = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_IMPORT_IDO_FROM_EXCEL = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_IMPORT_SAILEASE_FROM_EXCEL = New System.Windows.Forms.ToolStripMenuItem()
        Me.menu_IMPORT_GSON_FROM_EXCEL = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tsslUserInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.台帳ToolStripMenuItem, Me.月次ToolStripMenuItem, Me.期間ToolStripMenuItem, Me.決算ToolStripMenuItem, Me.マスタToolStripMenuItem, Me.一括更新ToolStripMenuItem, Me.システムToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1375, 33)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '台帳ToolStripMenuItem
        '
        Me.台帳ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_CONTRACT_LIST, Me.menu_BUKN_LIST, Me.menu_HAIF, Me.menu_HENF, Me.menu_GSON, Me.menu_NEW_CONTRACT})
        Me.台帳ToolStripMenuItem.Name = "台帳ToolStripMenuItem"
        Me.台帳ToolStripMenuItem.Size = New System.Drawing.Size(64, 29)
        Me.台帳ToolStripMenuItem.Text = "台帳"
        '
        'menu_CONTRACT_LIST
        '
        Me.menu_CONTRACT_LIST.Name = "menu_CONTRACT_LIST"
        Me.menu_CONTRACT_LIST.Size = New System.Drawing.Size(359, 34)
        Me.menu_CONTRACT_LIST.Text = "契約書フレックス"
        '
        'menu_BUKN_LIST
        '
        Me.menu_BUKN_LIST.Name = "menu_BUKN_LIST"
        Me.menu_BUKN_LIST.Size = New System.Drawing.Size(359, 34)
        Me.menu_BUKN_LIST.Text = "物件フレックス"
        '
        'menu_HAIF
        '
        Me.menu_HAIF.Name = "menu_HAIF"
        Me.menu_HAIF.Size = New System.Drawing.Size(359, 34)
        Me.menu_HAIF.Text = "物件フレックス（配賦行単位）"
        '
        'menu_HENF
        '
        Me.menu_HENF.Name = "menu_HENF"
        Me.menu_HENF.Size = New System.Drawing.Size(359, 34)
        Me.menu_HENF.Text = "保守フレックス（物件付随保守）"
        '
        'menu_GSON
        '
        Me.menu_GSON.Name = "menu_GSON"
        Me.menu_GSON.Size = New System.Drawing.Size(359, 34)
        Me.menu_GSON.Text = "減損フレックス"
        '
        'menu_NEW_CONTRACT
        '
        Me.menu_NEW_CONTRACT.Name = "menu_NEW_CONTRACT"
        Me.menu_NEW_CONTRACT.Size = New System.Drawing.Size(359, 34)
        Me.menu_NEW_CONTRACT.Text = "新規入力"
        '
        '月次ToolStripMenuItem
        '
        Me.月次ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_TOUGETSU_JOKEN, Me.menu_KEIJO_JOKEN})
        Me.月次ToolStripMenuItem.Name = "月次ToolStripMenuItem"
        Me.月次ToolStripMenuItem.Size = New System.Drawing.Size(64, 29)
        Me.月次ToolStripMenuItem.Text = "月次"
        '
        'menu_TOUGETSU_JOKEN
        '
        Me.menu_TOUGETSU_JOKEN.Name = "menu_TOUGETSU_JOKEN"
        Me.menu_TOUGETSU_JOKEN.Size = New System.Drawing.Size(287, 34)
        Me.menu_TOUGETSU_JOKEN.Text = "月次支払照合フレックス"
        '
        'menu_KEIJO_JOKEN
        '
        Me.menu_KEIJO_JOKEN.Name = "menu_KEIJO_JOKEN"
        Me.menu_KEIJO_JOKEN.Size = New System.Drawing.Size(287, 34)
        Me.menu_KEIJO_JOKEN.Text = "月次仕訳計上フレックス"
        '
        '期間ToolStripMenuItem
        '
        Me.期間ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_TANA_JOKEN, Me.menu_KLSRYO_JOKEN, Me.menu_IDOLST_JOKEN, Me.menu_KHIYO_JOKEN, Me.menu_YOSAN_JOKEN})
        Me.期間ToolStripMenuItem.Name = "期間ToolStripMenuItem"
        Me.期間ToolStripMenuItem.Size = New System.Drawing.Size(64, 29)
        Me.期間ToolStripMenuItem.Text = "期間"
        '
        'menu_TANA_JOKEN
        '
        Me.menu_TANA_JOKEN.Name = "menu_TANA_JOKEN"
        Me.menu_TANA_JOKEN.Size = New System.Drawing.Size(297, 34)
        Me.menu_TANA_JOKEN.Text = "棚卸明細表"
        '
        'menu_KLSRYO_JOKEN
        '
        Me.menu_KLSRYO_JOKEN.Name = "menu_KLSRYO_JOKEN"
        Me.menu_KLSRYO_JOKEN.Size = New System.Drawing.Size(297, 34)
        Me.menu_KLSRYO_JOKEN.Text = "期間リース料支払明細表"
        '
        'menu_IDOLST_JOKEN
        '
        Me.menu_IDOLST_JOKEN.Name = "menu_IDOLST_JOKEN"
        Me.menu_IDOLST_JOKEN.Size = New System.Drawing.Size(297, 34)
        Me.menu_IDOLST_JOKEN.Text = "移動物件一覧表"
        '
        'menu_KHIYO_JOKEN
        '
        Me.menu_KHIYO_JOKEN.Name = "menu_KHIYO_JOKEN"
        Me.menu_KHIYO_JOKEN.Size = New System.Drawing.Size(297, 34)
        Me.menu_KHIYO_JOKEN.Text = "期間費用計上明細表"
        '
        'menu_YOSAN_JOKEN
        '
        Me.menu_YOSAN_JOKEN.Name = "menu_YOSAN_JOKEN"
        Me.menu_YOSAN_JOKEN.Size = New System.Drawing.Size(297, 34)
        Me.menu_YOSAN_JOKEN.Text = "予算実績集計"
        '
        '決算ToolStripMenuItem
        '
        Me.決算ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_CHUKI_JOKEN, Me.menu_ZANDAKA_JOKEN, Me.menu_SAIMU_JOKEN, Me.menu_BEPPYO2_JOKEN})
        Me.決算ToolStripMenuItem.Name = "決算ToolStripMenuItem"
        Me.決算ToolStripMenuItem.Size = New System.Drawing.Size(64, 29)
        Me.決算ToolStripMenuItem.Text = "決算"
        '
        'menu_CHUKI_JOKEN
        '
        Me.menu_CHUKI_JOKEN.Name = "menu_CHUKI_JOKEN"
        Me.menu_CHUKI_JOKEN.Size = New System.Drawing.Size(279, 34)
        Me.menu_CHUKI_JOKEN.Text = "財務諸表注記"
        '
        'menu_ZANDAKA_JOKEN
        '
        Me.menu_ZANDAKA_JOKEN.Name = "menu_ZANDAKA_JOKEN"
        Me.menu_ZANDAKA_JOKEN.Size = New System.Drawing.Size(279, 34)
        Me.menu_ZANDAKA_JOKEN.Text = "リース資産残高一覧表"
        '
        'menu_SAIMU_JOKEN
        '
        Me.menu_SAIMU_JOKEN.Name = "menu_SAIMU_JOKEN"
        Me.menu_SAIMU_JOKEN.Size = New System.Drawing.Size(279, 34)
        Me.menu_SAIMU_JOKEN.Text = "リース債務返済明細表"
        '
        'menu_BEPPYO2_JOKEN
        '
        Me.menu_BEPPYO2_JOKEN.Name = "menu_BEPPYO2_JOKEN"
        Me.menu_BEPPYO2_JOKEN.Size = New System.Drawing.Size(279, 34)
        Me.menu_BEPPYO2_JOKEN.Text = "別表16（4）"
        '
        'マスタToolStripMenuItem
        '
        Me.マスタToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_CORP, Me.menu_KKNRI, Me.menu_LCPT, Me.menu_SHHO, Me.menu_GENK, Me.menu_BCAT, Me.menu_BKNRI, Me.menu_HKMK, Me.menu_SKMK, Me.menu_BKIND, Me.menu_KOZA, Me.menu_GSHA, Me.menu_MCPT, Me.menu_HKHO, Me.menu_RSRVH1, Me.menu_RSRVB1, Me.menu_KARI_RITU, Me.menu_ZEI_KAISEI, Me.menu_HREL})
        Me.マスタToolStripMenuItem.Name = "マスタToolStripMenuItem"
        Me.マスタToolStripMenuItem.Size = New System.Drawing.Size(70, 29)
        Me.マスタToolStripMenuItem.Text = "マスタ"
        '
        'menu_CORP
        '
        Me.menu_CORP.Name = "menu_CORP"
        Me.menu_CORP.Size = New System.Drawing.Size(292, 34)
        Me.menu_CORP.Text = "会社"
        '
        'menu_KKNRI
        '
        Me.menu_KKNRI.Name = "menu_KKNRI"
        Me.menu_KKNRI.Size = New System.Drawing.Size(292, 34)
        Me.menu_KKNRI.Text = "契約管理単位"
        '
        'menu_LCPT
        '
        Me.menu_LCPT.Name = "menu_LCPT"
        Me.menu_LCPT.Size = New System.Drawing.Size(292, 34)
        Me.menu_LCPT.Text = "支払先"
        '
        'menu_SHHO
        '
        Me.menu_SHHO.Name = "menu_SHHO"
        Me.menu_SHHO.Size = New System.Drawing.Size(292, 34)
        Me.menu_SHHO.Text = "支払方法"
        '
        'menu_GENK
        '
        Me.menu_GENK.Name = "menu_GENK"
        Me.menu_GENK.Size = New System.Drawing.Size(292, 34)
        Me.menu_GENK.Text = "原価区分"
        '
        'menu_BCAT
        '
        Me.menu_BCAT.Name = "menu_BCAT"
        Me.menu_BCAT.Size = New System.Drawing.Size(292, 34)
        Me.menu_BCAT.Text = "部署"
        '
        'menu_BKNRI
        '
        Me.menu_BKNRI.Name = "menu_BKNRI"
        Me.menu_BKNRI.Size = New System.Drawing.Size(292, 34)
        Me.menu_BKNRI.Text = "物件管理単位"
        '
        'menu_HKMK
        '
        Me.menu_HKMK.Name = "menu_HKMK"
        Me.menu_HKMK.Size = New System.Drawing.Size(292, 34)
        Me.menu_HKMK.Text = "費用区分"
        '
        'menu_SKMK
        '
        Me.menu_SKMK.Name = "menu_SKMK"
        Me.menu_SKMK.Size = New System.Drawing.Size(292, 34)
        Me.menu_SKMK.Text = "資産区分"
        '
        'menu_BKIND
        '
        Me.menu_BKIND.Name = "menu_BKIND"
        Me.menu_BKIND.Size = New System.Drawing.Size(292, 34)
        Me.menu_BKIND.Text = "物件種別"
        '
        'menu_KOZA
        '
        Me.menu_KOZA.Name = "menu_KOZA"
        Me.menu_KOZA.Size = New System.Drawing.Size(292, 34)
        Me.menu_KOZA.Text = "銀行口座"
        '
        'menu_GSHA
        '
        Me.menu_GSHA.Name = "menu_GSHA"
        Me.menu_GSHA.Size = New System.Drawing.Size(292, 34)
        Me.menu_GSHA.Text = "業者"
        '
        'menu_MCPT
        '
        Me.menu_MCPT.Name = "menu_MCPT"
        Me.menu_MCPT.Size = New System.Drawing.Size(292, 34)
        Me.menu_MCPT.Text = "メーカー"
        '
        'menu_HKHO
        '
        Me.menu_HKHO.Name = "menu_HKHO"
        Me.menu_HKHO.Size = New System.Drawing.Size(292, 34)
        Me.menu_HKHO.Text = "廃棄方法"
        '
        'menu_RSRVH1
        '
        Me.menu_RSRVH1.Name = "menu_RSRVH1"
        Me.menu_RSRVH1.Size = New System.Drawing.Size(292, 34)
        Me.menu_RSRVH1.Text = "予備（契約者用）"
        '
        'menu_RSRVB1
        '
        Me.menu_RSRVB1.Name = "menu_RSRVB1"
        Me.menu_RSRVB1.Size = New System.Drawing.Size(292, 34)
        Me.menu_RSRVB1.Text = "予備（物件用）"
        '
        'menu_KARI_RITU
        '
        Me.menu_KARI_RITU.Name = "menu_KARI_RITU"
        Me.menu_KARI_RITU.Size = New System.Drawing.Size(292, 34)
        Me.menu_KARI_RITU.Text = "追加借入利子率テーブル"
        '
        'menu_ZEI_KAISEI
        '
        Me.menu_ZEI_KAISEI.Name = "menu_ZEI_KAISEI"
        Me.menu_ZEI_KAISEI.Size = New System.Drawing.Size(292, 34)
        Me.menu_ZEI_KAISEI.Text = "消費税率テーブル"
        '
        'menu_HREL
        '
        Me.menu_HREL.Name = "menu_HREL"
        Me.menu_HREL.Size = New System.Drawing.Size(292, 34)
        Me.menu_HREL.Text = "費用関連テーブル"
        '
        '一括更新ToolStripMenuItem
        '
        Me.一括更新ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_CHUKI_RECALC, Me.menu_IMPORT_CONTRACT_FROM_EXCEL, Me.menu_IMPORT_IDO_FROM_EXCEL, Me.menu_IMPORT_SAILEASE_FROM_EXCEL, Me.menu_IMPORT_GSON_FROM_EXCEL})
        Me.一括更新ToolStripMenuItem.Name = "一括更新ToolStripMenuItem"
        Me.一括更新ToolStripMenuItem.Size = New System.Drawing.Size(100, 29)
        Me.一括更新ToolStripMenuItem.Text = "一括更新"
        '
        'menu_CHUKI_RECALC
        '
        Me.menu_CHUKI_RECALC.Name = "menu_CHUKI_RECALC"
        Me.menu_CHUKI_RECALC.Size = New System.Drawing.Size(314, 34)
        Me.menu_CHUKI_RECALC.Text = "注記判定再計算"
        '
        'menu_IMPORT_CONTRACT_FROM_EXCEL
        '
        Me.menu_IMPORT_CONTRACT_FROM_EXCEL.Name = "menu_IMPORT_CONTRACT_FROM_EXCEL"
        Me.menu_IMPORT_CONTRACT_FROM_EXCEL.Size = New System.Drawing.Size(314, 34)
        Me.menu_IMPORT_CONTRACT_FROM_EXCEL.Text = "契約書変更情報Excel取込"
        '
        'menu_IMPORT_IDO_FROM_EXCEL
        '
        Me.menu_IMPORT_IDO_FROM_EXCEL.Name = "menu_IMPORT_IDO_FROM_EXCEL"
        Me.menu_IMPORT_IDO_FROM_EXCEL.Size = New System.Drawing.Size(314, 34)
        Me.menu_IMPORT_IDO_FROM_EXCEL.Text = "物件移動"
        '
        'menu_IMPORT_SAILEASE_FROM_EXCEL
        '
        Me.menu_IMPORT_SAILEASE_FROM_EXCEL.Name = "menu_IMPORT_SAILEASE_FROM_EXCEL"
        Me.menu_IMPORT_SAILEASE_FROM_EXCEL.Size = New System.Drawing.Size(314, 34)
        Me.menu_IMPORT_SAILEASE_FROM_EXCEL.Text = "再リース/返却"
        '
        'menu_IMPORT_GSON_FROM_EXCEL
        '
        Me.menu_IMPORT_GSON_FROM_EXCEL.Name = "menu_IMPORT_GSON_FROM_EXCEL"
        Me.menu_IMPORT_GSON_FROM_EXCEL.Size = New System.Drawing.Size(314, 34)
        Me.menu_IMPORT_GSON_FROM_EXCEL.Text = "減損損失の取り込み"
        '
        'システムToolStripMenuItem
        '
        Me.システムToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_SEC_USER, Me.menu_SEC_KNGN, Me.DB管理ToolStripMenuItem, Me.menu_ENV_SETTING, Me.menu_TOUSEI_OPT, Me.menu_PWD_CHANGE, Me.ログ管理ToolStripMenuItem, Me.menu_SWKK_FIXED, Me.menu_VERSION_INFO, Me.menu_LOGOUT, Me.menu_EXIT})
        Me.システムToolStripMenuItem.Name = "システムToolStripMenuItem"
        Me.システムToolStripMenuItem.Size = New System.Drawing.Size(82, 29)
        Me.システムToolStripMenuItem.Text = "システム"
        '
        'DB管理ToolStripMenuItem
        '
        Me.DB管理ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_DB_SAVE, Me.menu_DB_RESTORE, Me.menu_EXCEL_IMPORT, Me.menu_CACHE_CLEAR, Me.menu_DB_COMPACT, Me.menu_DB_CREATE, Me.menu_DB_DROP, Me.menu_LOCK_CLEAR, Me.menu_DB_MIGRATE, Me.menu_DB_MNT_BKNRI, Me.menu_DB_EXPORT})
        Me.DB管理ToolStripMenuItem.Name = "DB管理ToolStripMenuItem"
        Me.DB管理ToolStripMenuItem.Size = New System.Drawing.Size(270, 34)
        Me.DB管理ToolStripMenuItem.Text = "DB管理"
        '
        'menu_DB_SAVE
        '
        Me.menu_DB_SAVE.Name = "menu_DB_SAVE"
        Me.menu_DB_SAVE.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_SAVE.Text = "データ保存"
        '
        'menu_DB_RESTORE
        '
        Me.menu_DB_RESTORE.Name = "menu_DB_RESTORE"
        Me.menu_DB_RESTORE.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_RESTORE.Text = "データ復元"
        '
        'menu_EXCEL_IMPORT
        '
        Me.menu_EXCEL_IMPORT.Name = "menu_EXCEL_IMPORT"
        Me.menu_EXCEL_IMPORT.Size = New System.Drawing.Size(280, 34)
        Me.menu_EXCEL_IMPORT.Text = "EXCEL取込"
        '
        'menu_CACHE_CLEAR
        '
        Me.menu_CACHE_CLEAR.Name = "menu_CACHE_CLEAR"
        Me.menu_CACHE_CLEAR.Size = New System.Drawing.Size(280, 34)
        Me.menu_CACHE_CLEAR.Text = "キャッシュクリア"
        '
        'menu_DB_COMPACT
        '
        Me.menu_DB_COMPACT.Name = "menu_DB_COMPACT"
        Me.menu_DB_COMPACT.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_COMPACT.Text = "最適化"
        '
        'menu_DB_CREATE
        '
        Me.menu_DB_CREATE.Name = "menu_DB_CREATE"
        Me.menu_DB_CREATE.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_CREATE.Text = "DB作成"
        '
        'menu_DB_DROP
        '
        Me.menu_DB_DROP.Name = "menu_DB_DROP"
        Me.menu_DB_DROP.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_DROP.Text = "DB削除"
        '
        'menu_LOCK_CLEAR
        '
        Me.menu_LOCK_CLEAR.Name = "menu_LOCK_CLEAR"
        Me.menu_LOCK_CLEAR.Size = New System.Drawing.Size(280, 34)
        Me.menu_LOCK_CLEAR.Text = "ロック解除"
        '
        'menu_DB_MIGRATE
        '
        Me.menu_DB_MIGRATE.Name = "menu_DB_MIGRATE"
        Me.menu_DB_MIGRATE.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_MIGRATE.Text = "DBマイグレーション"
        '
        'menu_DB_MNT_BKNRI
        '
        Me.menu_DB_MNT_BKNRI.Name = "menu_DB_MNT_BKNRI"
        Me.menu_DB_MNT_BKNRI.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_MNT_BKNRI.Text = "物件管理単位メンテナンス"
        '
        'menu_DB_EXPORT
        '
        Me.menu_DB_EXPORT.Name = "menu_DB_EXPORT"
        Me.menu_DB_EXPORT.Size = New System.Drawing.Size(280, 34)
        Me.menu_DB_EXPORT.Text = "データエクスポート"
        '
        'menu_ENV_SETTING
        '
        Me.menu_ENV_SETTING.Name = "menu_ENV_SETTING"
        Me.menu_ENV_SETTING.Size = New System.Drawing.Size(270, 34)
        Me.menu_ENV_SETTING.Text = "環境設定"
        '
        'menu_TOUSEI_OPT
        '
        Me.menu_TOUSEI_OPT.Name = "menu_TOUSEI_OPT"
        Me.menu_TOUSEI_OPT.Size = New System.Drawing.Size(270, 34)
        Me.menu_TOUSEI_OPT.Text = "統制オプション"
        '
        'menu_SEC_USER
        '
        Me.menu_SEC_USER.Name = "menu_SEC_USER"
        Me.menu_SEC_USER.Size = New System.Drawing.Size(270, 34)
        Me.menu_SEC_USER.Text = "システム利用者"
        '
        'menu_SEC_KNGN
        '
        Me.menu_SEC_KNGN.Name = "menu_SEC_KNGN"
        Me.menu_SEC_KNGN.Size = New System.Drawing.Size(270, 34)
        Me.menu_SEC_KNGN.Text = "システム利用権限"
        '
        'menu_PWD_CHANGE
        '
        Me.menu_PWD_CHANGE.Name = "menu_PWD_CHANGE"
        Me.menu_PWD_CHANGE.Size = New System.Drawing.Size(270, 34)
        Me.menu_PWD_CHANGE.Text = "パスワード変更"
        '
        'ログ管理ToolStripMenuItem
        '
        Me.ログ管理ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menu_LOG_REF, Me.menu_LOG_UPD, Me.menu_LOG_SAVE_RESTORE, Me.menu_LOG_DELETE})
        Me.ログ管理ToolStripMenuItem.Name = "ログ管理ToolStripMenuItem"
        Me.ログ管理ToolStripMenuItem.Size = New System.Drawing.Size(270, 34)
        Me.ログ管理ToolStripMenuItem.Text = "ログ管理"
        '
        'menu_LOG_REF
        '
        Me.menu_LOG_REF.Name = "menu_LOG_REF"
        Me.menu_LOG_REF.Size = New System.Drawing.Size(250, 34)
        Me.menu_LOG_REF.Text = "参照ログ"
        '
        'menu_LOG_UPD
        '
        Me.menu_LOG_UPD.Name = "menu_LOG_UPD"
        Me.menu_LOG_UPD.Size = New System.Drawing.Size(250, 34)
        Me.menu_LOG_UPD.Text = "更新ログ"
        '
        'menu_LOG_SAVE_RESTORE
        '
        Me.menu_LOG_SAVE_RESTORE.Name = "menu_LOG_SAVE_RESTORE"
        Me.menu_LOG_SAVE_RESTORE.Size = New System.Drawing.Size(250, 34)
        Me.menu_LOG_SAVE_RESTORE.Text = "保存復元ログ"
        '
        'menu_LOG_DELETE
        '
        Me.menu_LOG_DELETE.Name = "menu_LOG_DELETE"
        Me.menu_LOG_DELETE.Size = New System.Drawing.Size(250, 34)
        Me.menu_LOG_DELETE.Text = "ログ削除"
        '
        'menu_SWKK_FIXED
        '
        Me.menu_SWKK_FIXED.Name = "menu_SWKK_FIXED"
        Me.menu_SWKK_FIXED.Size = New System.Drawing.Size(270, 34)
        Me.menu_SWKK_FIXED.Text = "仕訳固定値設定"
        '
        'menu_VERSION_INFO
        '
        Me.menu_VERSION_INFO.Name = "menu_VERSION_INFO"
        Me.menu_VERSION_INFO.Size = New System.Drawing.Size(270, 34)
        Me.menu_VERSION_INFO.Text = "バージョン情報"
        '
        'menu_LOGOUT
        '
        Me.menu_LOGOUT.Name = "menu_LOGOUT"
        Me.menu_LOGOUT.Size = New System.Drawing.Size(270, 34)
        Me.menu_LOGOUT.Text = "ログオフ"
        '
        'menu_EXIT
        '
        Me.menu_EXIT.Name = "menu_EXIT"
        Me.menu_EXIT.Size = New System.Drawing.Size(270, 34)
        Me.menu_EXIT.Text = "業務の終了"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsslUserInfo})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 145)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1375, 22)
        Me.StatusStrip1.TabIndex = 4
        '
        'tsslUserInfo
        '
        Me.tsslUserInfo.Name = "tsslUserInfo"
        Me.tsslUserInfo.Size = New System.Drawing.Size(0, 17)
        Me.tsslUserInfo.Text = ""
        '
        'Form_MAIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1375, 167)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Name = "Form_MAIN"
        Me.Text = "メイン画面"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents システムToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DB管理ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menu_DB_SAVE As ToolStripMenuItem
    Friend WithEvents menu_DB_RESTORE As ToolStripMenuItem
    Friend WithEvents menu_EXCEL_IMPORT As ToolStripMenuItem
    Friend WithEvents menu_CACHE_CLEAR As ToolStripMenuItem
    Friend WithEvents menu_DB_COMPACT As ToolStripMenuItem
    Friend WithEvents menu_DB_CREATE As ToolStripMenuItem
    Friend WithEvents menu_DB_DROP As ToolStripMenuItem
    Friend WithEvents menu_LOCK_CLEAR As ToolStripMenuItem
    Friend WithEvents menu_DB_MIGRATE As ToolStripMenuItem
    Friend WithEvents menu_DB_MNT_BKNRI As ToolStripMenuItem
    Friend WithEvents menu_DB_EXPORT As ToolStripMenuItem
    Friend WithEvents menu_ENV_SETTING As ToolStripMenuItem
    Friend WithEvents menu_TOUSEI_OPT As ToolStripMenuItem
    Friend WithEvents menu_SEC_USER As ToolStripMenuItem
    Friend WithEvents menu_SEC_KNGN As ToolStripMenuItem
    Friend WithEvents menu_PWD_CHANGE As ToolStripMenuItem
    Friend WithEvents ログ管理ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menu_LOG_REF As ToolStripMenuItem
    Friend WithEvents menu_LOG_UPD As ToolStripMenuItem
    Friend WithEvents menu_LOG_SAVE_RESTORE As ToolStripMenuItem
    Friend WithEvents menu_LOG_DELETE As ToolStripMenuItem
    Friend WithEvents menu_SWKK_FIXED As ToolStripMenuItem
    Friend WithEvents menu_VERSION_INFO As ToolStripMenuItem
    Friend WithEvents menu_LOGOUT As ToolStripMenuItem
    Friend WithEvents menu_EXIT As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents tsslUserInfo As ToolStripStatusLabel
    Friend WithEvents 台帳ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menu_CONTRACT_LIST As ToolStripMenuItem
    Friend WithEvents menu_BUKN_LIST As ToolStripMenuItem
    Friend WithEvents menu_HAIF As ToolStripMenuItem
    Friend WithEvents menu_HENF As ToolStripMenuItem
    Friend WithEvents menu_GSON As ToolStripMenuItem
    Friend WithEvents menu_NEW_CONTRACT As ToolStripMenuItem
    Friend WithEvents 月次ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 期間ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 決算ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents マスタToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 一括更新ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents menu_TOUGETSU_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_KEIJO_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_TANA_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_KLSRYO_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_IDOLST_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_KHIYO_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_YOSAN_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_CHUKI_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_ZANDAKA_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_SAIMU_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_BEPPYO2_JOKEN As ToolStripMenuItem
    Friend WithEvents menu_CORP As ToolStripMenuItem
    Friend WithEvents menu_KKNRI As ToolStripMenuItem
    Friend WithEvents menu_LCPT As ToolStripMenuItem
    Friend WithEvents menu_GENK As ToolStripMenuItem
    Friend WithEvents menu_BCAT As ToolStripMenuItem
    Friend WithEvents menu_BKNRI As ToolStripMenuItem
    Friend WithEvents menu_HKMK As ToolStripMenuItem
    Friend WithEvents menu_SKMK As ToolStripMenuItem
    Friend WithEvents menu_BKIND As ToolStripMenuItem
    Friend WithEvents menu_KOZA As ToolStripMenuItem
    Friend WithEvents menu_GSHA As ToolStripMenuItem
    Friend WithEvents menu_MCPT As ToolStripMenuItem
    Friend WithEvents menu_HKHO As ToolStripMenuItem
    Friend WithEvents menu_RSRVH1 As ToolStripMenuItem
    Friend WithEvents menu_RSRVB1 As ToolStripMenuItem
    Friend WithEvents menu_KARI_RITU As ToolStripMenuItem
    Friend WithEvents menu_ZEI_KAISEI As ToolStripMenuItem
    Friend WithEvents menu_HREL As ToolStripMenuItem
    Friend WithEvents menu_CHUKI_RECALC As ToolStripMenuItem
    Friend WithEvents menu_IMPORT_CONTRACT_FROM_EXCEL As ToolStripMenuItem
    Friend WithEvents menu_IMPORT_IDO_FROM_EXCEL As ToolStripMenuItem
    Friend WithEvents menu_IMPORT_SAILEASE_FROM_EXCEL As ToolStripMenuItem
    Friend WithEvents menu_IMPORT_GSON_FROM_EXCEL As ToolStripMenuItem
    Friend WithEvents menu_SHHO As ToolStripMenuItem
End Class
