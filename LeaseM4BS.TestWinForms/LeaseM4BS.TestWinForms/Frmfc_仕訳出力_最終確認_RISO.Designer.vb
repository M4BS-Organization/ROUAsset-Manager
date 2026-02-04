<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frmfc_仕訳出力_最終確認_RISO
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.unnamed_Label_1917970269568 = New System.Windows.Forms.Label()
        Me.unnamed_Rectangle_1917970272448 = New System.Windows.Forms.Panel()
        Me.unnamed_Line_1917970274176 = New System.Windows.Forms.Label()
        Me.unnamed_Image_1917970271104 = New System.Windows.Forms.PictureBox()
        Me.unnamed_CommandButton_1917970275712 = New System.Windows.Forms.Button()
        Me.unnamed_OptionButton_1917970277248 = New System.Windows.Forms.RadioButton()
        Me.unnamed_CheckBox_1917970279040 = New System.Windows.Forms.CheckBox()
        Me.unnamed_OptionGroup_1917970280896 = New System.Windows.Forms.GroupBox()
        Me.unnamed_TextBox_1917977379840 = New System.Windows.Forms.TextBox()
        Me.unnamed_ListBox_1917977381376 = New System.Windows.Forms.ListBox()
        Me.unnamed_ComboBox_1917977382976 = New System.Windows.Forms.ComboBox()
        Me.unnamed_Subform_1917977385856 = New System.Windows.Forms.Panel()
        Me.unnamed_UnboundObjectFrame_1917977387584 = New System.Windows.Forms.Panel()
        Me.unnamed_Tab_1917977390720 = New System.Windows.Forms.TabControl()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.入力番号_ラベル = New System.Windows.Forms.Label()
        Me.細目コード識別区分_ラベル = New System.Windows.Forms.Label()
        Me.勘定科目コード_ラベル = New System.Windows.Forms.Label()
        Me.会計部門コード_ラベル = New System.Windows.Forms.Label()
        Me.取引先コード_ラベル = New System.Windows.Forms.Label()
        Me.履歴物件コード_ラベル = New System.Windows.Forms.Label()
        Me.セグメントコード_ラベル = New System.Windows.Forms.Label()
        Me.取引通貨コード_ラベル = New System.Windows.Forms.Label()
        Me.資金コード_ラベル = New System.Windows.Forms.Label()
        Me.基本通貨発生金額_ラベル = New System.Windows.Forms.Label()
        Me.消費税区分コード_ラベル = New System.Windows.Forms.Label()
        Me.参考消費税金額_ラベル = New System.Windows.Forms.Label()
        Me.文字摘要１_ラベル = New System.Windows.Forms.Label()
        Me.cmd_実行 = New System.Windows.Forms.Button()
        Me.cmd_CANCEL = New System.Windows.Forms.Button()
        Me.txt_処理年月 = New System.Windows.Forms.TextBox()
        Me.ラベル521 = New System.Windows.Forms.Label()
        Me.txt_計上日_曜日 = New System.Windows.Forms.TextBox()
        Me.cmd_FlexReportDLG = New System.Windows.Forms.Button()
        Me.txt_OUTPUT_FPATH = New System.Windows.Forms.TextBox()
        Me.ラベル523 = New System.Windows.Forms.Label()
        Me.cmd_選択 = New System.Windows.Forms.Button()
        Me.txt_仕訳_ID = New System.Windows.Forms.TextBox()
        Me.会社コード_ラベル = New System.Windows.Forms.Label()
        Me.起票社員コード_ラベル = New System.Windows.Forms.Label()
        Me.起票部門コード_ラベル = New System.Windows.Forms.Label()
        Me.承認社員コード_ラベル = New System.Windows.Forms.Label()
        Me.承認日付_ラベル = New System.Windows.Forms.Label()
        Me.承認状態区分_ラベル = New System.Windows.Forms.Label()
        Me.仕訳種別区分_ラベル = New System.Windows.Forms.Label()
        Me.伝票日付_ラベル = New System.Windows.Forms.Label()
        Me.伝票番号_ラベル = New System.Windows.Forms.Label()
        Me.伝票操作禁止区分_ラベル = New System.Windows.Forms.Label()
        Me.伝票備考_ラベル = New System.Windows.Forms.Label()
        Me.行番号_ラベル = New System.Windows.Forms.Label()
        Me.伝票明細貸借区分_ラベル = New System.Windows.Forms.Label()
        Me.細目コード_ラベル = New System.Windows.Forms.Label()
        Me.集計拡張コード１識別区分_ラベル = New System.Windows.Forms.Label()
        Me.集計拡張コード１_ラベル = New System.Windows.Forms.Label()
        Me.課税区分_ラベル = New System.Windows.Forms.Label()
        Me.税率区分_ラベル = New System.Windows.Forms.Label()
        Me.取引通貨発生金額_ラベル = New System.Windows.Forms.Label()
        Me.入力システム区分_ラベル = New System.Windows.Forms.Label()
        Me.pnlDetail = New System.Windows.Forms.Panel()
        Me.入力番号 = New System.Windows.Forms.TextBox()
        Me.細目コード識別区分 = New System.Windows.Forms.TextBox()
        Me.勘定科目コード = New System.Windows.Forms.TextBox()
        Me.会計部門コード = New System.Windows.Forms.TextBox()
        Me.取引先コード = New System.Windows.Forms.TextBox()
        Me.履歴物件コード = New System.Windows.Forms.TextBox()
        Me.セグメントコード = New System.Windows.Forms.TextBox()
        Me.取引通貨コード = New System.Windows.Forms.TextBox()
        Me.資金コード = New System.Windows.Forms.TextBox()
        Me.基本通貨発生金額 = New System.Windows.Forms.TextBox()
        Me.消費税区分コード = New System.Windows.Forms.TextBox()
        Me.参考消費税金額 = New System.Windows.Forms.TextBox()
        Me.文字摘要１ = New System.Windows.Forms.TextBox()
        Me.会社コード = New System.Windows.Forms.TextBox()
        Me.起票社員コード = New System.Windows.Forms.TextBox()
        Me.起票部門コード = New System.Windows.Forms.TextBox()
        Me.承認社員コード = New System.Windows.Forms.TextBox()
        Me.承認日付 = New System.Windows.Forms.TextBox()
        Me.承認状態区分 = New System.Windows.Forms.TextBox()
        Me.仕訳種別区分 = New System.Windows.Forms.TextBox()
        Me.伝票日付 = New System.Windows.Forms.TextBox()
        Me.伝票番号 = New System.Windows.Forms.TextBox()
        Me.伝票操作禁止区分 = New System.Windows.Forms.TextBox()
        Me.伝票備考 = New System.Windows.Forms.TextBox()
        Me.行番号 = New System.Windows.Forms.TextBox()
        Me.伝票明細貸借区分 = New System.Windows.Forms.TextBox()
        Me.細目コード = New System.Windows.Forms.TextBox()
        Me.集計拡張コード１識別区分 = New System.Windows.Forms.TextBox()
        Me.集計拡張コード１ = New System.Windows.Forms.TextBox()
        Me.課税区分 = New System.Windows.Forms.TextBox()
        Me.税率区分 = New System.Windows.Forms.TextBox()
        Me.取引通貨発生金額 = New System.Windows.Forms.TextBox()
        Me.入力システム区分 = New System.Windows.Forms.TextBox()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.ラベル34 = New System.Windows.Forms.Label()
        Me.ラベル35 = New System.Windows.Forms.Label()
        Me.txt_借方金額_SUM = New System.Windows.Forms.TextBox()
        Me.txt_貸方金額_SUM = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        ' Frmfc_仕訳出力_最終確認_RISO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(2105, 800)
        Me.Controls.Add(Me.unnamed_Label_1917970269568)
        Me.Controls.Add(Me.unnamed_Rectangle_1917970272448)
        Me.Controls.Add(Me.unnamed_Line_1917970274176)
        Me.Controls.Add(Me.unnamed_Image_1917970271104)
        Me.Controls.Add(Me.unnamed_CommandButton_1917970275712)
        Me.Controls.Add(Me.unnamed_OptionButton_1917970277248)
        Me.Controls.Add(Me.unnamed_CheckBox_1917970279040)
        Me.Controls.Add(Me.unnamed_OptionGroup_1917970280896)
        Me.Controls.Add(Me.unnamed_TextBox_1917977379840)
        Me.Controls.Add(Me.unnamed_ListBox_1917977381376)
        Me.Controls.Add(Me.unnamed_ComboBox_1917977382976)
        Me.Controls.Add(Me.unnamed_Subform_1917977385856)
        Me.Controls.Add(Me.unnamed_UnboundObjectFrame_1917977387584)
        Me.Controls.Add(Me.unnamed_Tab_1917977390720)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlDetail)
        Me.Controls.Add(Me.pnlFooter)
        '
        ' Properties
        '
        ' unnamed_Label_1917970269568
        Me.unnamed_Label_1917970269568.Name = "unnamed_Label_1917970269568"
        Me.unnamed_Label_1917970269568.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Label_1917970269568.Size = New System.Drawing.Size(133, 26)

        ' unnamed_Rectangle_1917970272448
        Me.unnamed_Rectangle_1917970272448.Name = "unnamed_Rectangle_1917970272448"
        Me.unnamed_Rectangle_1917970272448.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Rectangle_1917970272448.Size = New System.Drawing.Size(56, 56)

        ' unnamed_Line_1917970274176
        Me.unnamed_Line_1917970274176.Name = "unnamed_Line_1917970274176"
        Me.unnamed_Line_1917970274176.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Line_1917970274176.Size = New System.Drawing.Size(113, 26)
        Me.unnamed_Line_1917970274176.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle

        ' unnamed_Image_1917970271104
        Me.unnamed_Image_1917970271104.Name = "unnamed_Image_1917970271104"
        Me.unnamed_Image_1917970271104.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Image_1917970271104.Size = New System.Drawing.Size(113, 113)

        ' unnamed_CommandButton_1917970275712
        Me.unnamed_CommandButton_1917970275712.Name = "unnamed_CommandButton_1917970275712"
        Me.unnamed_CommandButton_1917970275712.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CommandButton_1917970275712.Size = New System.Drawing.Size(113, 26)

        ' unnamed_OptionButton_1917970277248
        Me.unnamed_OptionButton_1917970277248.Name = "unnamed_OptionButton_1917970277248"
        Me.unnamed_OptionButton_1917970277248.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionButton_1917970277248.Size = New System.Drawing.Size(133, 26)

        ' unnamed_CheckBox_1917970279040
        Me.unnamed_CheckBox_1917970279040.Name = "unnamed_CheckBox_1917970279040"
        Me.unnamed_CheckBox_1917970279040.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_CheckBox_1917970279040.Size = New System.Drawing.Size(133, 26)

        ' unnamed_OptionGroup_1917970280896
        Me.unnamed_OptionGroup_1917970280896.Name = "unnamed_OptionGroup_1917970280896"
        Me.unnamed_OptionGroup_1917970280896.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_OptionGroup_1917970280896.Size = New System.Drawing.Size(113, 113)

        ' unnamed_TextBox_1917977379840
        Me.unnamed_TextBox_1917977379840.Name = "unnamed_TextBox_1917977379840"
        Me.unnamed_TextBox_1917977379840.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_TextBox_1917977379840.Size = New System.Drawing.Size(113, 26)

        ' unnamed_ListBox_1917977381376
        Me.unnamed_ListBox_1917977381376.Name = "unnamed_ListBox_1917977381376"
        Me.unnamed_ListBox_1917977381376.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ListBox_1917977381376.Size = New System.Drawing.Size(113, 94)

        ' unnamed_ComboBox_1917977382976
        Me.unnamed_ComboBox_1917977382976.Name = "unnamed_ComboBox_1917977382976"
        Me.unnamed_ComboBox_1917977382976.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_ComboBox_1917977382976.Size = New System.Drawing.Size(113, 26)

        ' unnamed_Subform_1917977385856
        Me.unnamed_Subform_1917977385856.Name = "unnamed_Subform_1917977385856"
        Me.unnamed_Subform_1917977385856.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Subform_1917977385856.Size = New System.Drawing.Size(113, 113)

        ' unnamed_UnboundObjectFrame_1917977387584
        Me.unnamed_UnboundObjectFrame_1917977387584.Name = "unnamed_UnboundObjectFrame_1917977387584"
        Me.unnamed_UnboundObjectFrame_1917977387584.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_UnboundObjectFrame_1917977387584.Size = New System.Drawing.Size(302, 189)

        ' unnamed_Tab_1917977390720
        Me.unnamed_Tab_1917977390720.Name = "unnamed_Tab_1917977390720"
        Me.unnamed_Tab_1917977390720.Location = New System.Drawing.Point(0, 0)
        Me.unnamed_Tab_1917977390720.Size = New System.Drawing.Size(340, 226)

        ' pnlHeader
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Size = New System.Drawing.Size(2105, 124)

        ' 入力番号_ラベル
        Me.入力番号_ラベル.Name = "入力番号_ラベル"
        Me.入力番号_ラベル.Location = New System.Drawing.Point(3, 83)
        Me.入力番号_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.入力番号_ラベル.Text = "入力番号"
        Me.pnlHeader.Controls.Add(Me.入力番号_ラベル)

        ' 細目コード識別区分_ラベル
        Me.細目コード識別区分_ラベル.Name = "細目コード識別区分_ラベル"
        Me.細目コード識別区分_ラベル.Location = New System.Drawing.Point(934, 83)
        Me.細目コード識別区分_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.細目コード識別区分_ラベル.Text = "細目\015\012コード\015\012識別区分"
        Me.pnlHeader.Controls.Add(Me.細目コード識別区分_ラベル)

        ' 勘定科目コード_ラベル
        Me.勘定科目コード_ラベル.Name = "勘定科目コード_ラベル"
        Me.勘定科目コード_ラベル.Location = New System.Drawing.Point(820, 83)
        Me.勘定科目コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.勘定科目コード_ラベル.Text = "勘定科目コード"
        Me.pnlHeader.Controls.Add(Me.勘定科目コード_ラベル)

        ' 会計部門コード_ラベル
        Me.会計部門コード_ラベル.Name = "会計部門コード_ラベル"
        Me.会計部門コード_ラベル.Location = New System.Drawing.Point(877, 83)
        Me.会計部門コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.会計部門コード_ラベル.Text = "会計部門コード"
        Me.pnlHeader.Controls.Add(Me.会計部門コード_ラベル)

        ' 取引先コード_ラベル
        Me.取引先コード_ラベル.Name = "取引先コード_ラベル"
        Me.取引先コード_ラベル.Location = New System.Drawing.Point(1160, 83)
        Me.取引先コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.取引先コード_ラベル.Text = "取引先\015\012コード"
        Me.pnlHeader.Controls.Add(Me.取引先コード_ラベル)

        ' 履歴物件コード_ラベル
        Me.履歴物件コード_ラベル.Name = "履歴物件コード_ラベル"
        Me.履歴物件コード_ラベル.Location = New System.Drawing.Point(1792, 83)
        Me.履歴物件コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.履歴物件コード_ラベル.Text = "履歴物件コード"
        Me.pnlHeader.Controls.Add(Me.履歴物件コード_ラベル)

        ' セグメントコード_ラベル
        Me.セグメントコード_ラベル.Name = "セグメントコード_ラベル"
        Me.セグメントコード_ラベル.Location = New System.Drawing.Point(1217, 83)
        Me.セグメントコード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.セグメントコード_ラベル.Text = "ｾｸﾞﾒﾝﾄ\015\012ｺｰﾄﾞ"
        Me.pnlHeader.Controls.Add(Me.セグメントコード_ラベル)

        ' 取引通貨コード_ラベル
        Me.取引通貨コード_ラベル.Name = "取引通貨コード_ラベル"
        Me.取引通貨コード_ラベル.Location = New System.Drawing.Point(1274, 83)
        Me.取引通貨コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.取引通貨コード_ラベル.Text = "取引通貨コード"
        Me.pnlHeader.Controls.Add(Me.取引通貨コード_ラベル)

        ' 資金コード_ラベル
        Me.資金コード_ラベル.Name = "資金コード_ラベル"
        Me.資金コード_ラベル.Location = New System.Drawing.Point(1331, 83)
        Me.資金コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.資金コード_ラベル.Text = "資金\015\012コード"
        Me.pnlHeader.Controls.Add(Me.資金コード_ラベル)

        ' 基本通貨発生金額_ラベル
        Me.基本通貨発生金額_ラベル.Name = "基本通貨発生金額_ラベル"
        Me.基本通貨発生金額_ラベル.Location = New System.Drawing.Point(1471, 83)
        Me.基本通貨発生金額_ラベル.Size = New System.Drawing.Size(94, 37)
        Me.基本通貨発生金額_ラベル.Text = "基本通貨発生金額"
        Me.pnlHeader.Controls.Add(Me.基本通貨発生金額_ラベル)

        ' 消費税区分コード_ラベル
        Me.消費税区分コード_ラベル.Name = "消費税区分コード_ラベル"
        Me.消費税区分コード_ラベル.Location = New System.Drawing.Point(1387, 83)
        Me.消費税区分コード_ラベル.Size = New System.Drawing.Size(45, 37)
        Me.消費税区分コード_ラベル.Text = "消費税\015\012区分\015\012コード\015\012CD"
        Me.pnlHeader.Controls.Add(Me.消費税区分コード_ラベル)

        ' 参考消費税金額_ラベル
        Me.参考消費税金額_ラベル.Name = "参考消費税金額_ラベル"
        Me.参考消費税金額_ラベル.Location = New System.Drawing.Point(1660, 83)
        Me.参考消費税金額_ラベル.Size = New System.Drawing.Size(94, 37)
        Me.参考消費税金額_ラベル.Text = "参考消費税\015\012金額"
        Me.pnlHeader.Controls.Add(Me.参考消費税金額_ラベル)

        ' 文字摘要１_ラベル
        Me.文字摘要１_ラベル.Name = "文字摘要１_ラベル"
        Me.文字摘要１_ラベル.Location = New System.Drawing.Point(1849, 83)
        Me.文字摘要１_ラベル.Size = New System.Drawing.Size(253, 37)
        Me.文字摘要１_ラベル.Text = "文字摘要１"
        Me.pnlHeader.Controls.Add(Me.文字摘要１_ラベル)

        ' cmd_実行
        Me.cmd_実行.Name = "cmd_実行"
        Me.cmd_実行.Location = New System.Drawing.Point(7, 7)
        Me.cmd_実行.Size = New System.Drawing.Size(75, 26)
        Me.cmd_実行.Text = "実行(&R)"
        Me.pnlHeader.Controls.Add(Me.cmd_実行)

        ' cmd_CANCEL
        Me.cmd_CANCEL.Name = "cmd_CANCEL"
        Me.cmd_CANCEL.Location = New System.Drawing.Point(90, 7)
        Me.cmd_CANCEL.Size = New System.Drawing.Size(75, 26)
        Me.cmd_CANCEL.Text = "ｷｬﾝｾﾙ(&C)"
        Me.cmd_CANCEL.TabIndex = 1
        Me.pnlHeader.Controls.Add(Me.cmd_CANCEL)

        ' txt_処理年月
        Me.txt_処理年月.Name = "txt_処理年月"
        Me.txt_処理年月.Location = New System.Drawing.Point(113, 56)
        Me.txt_処理年月.Size = New System.Drawing.Size(75, 18)
        Me.txt_処理年月.TabIndex = 2
        Me.pnlHeader.Controls.Add(Me.txt_処理年月)

        ' ラベル521
        Me.ラベル521.Name = "ラベル521"
        Me.ラベル521.Location = New System.Drawing.Point(18, 56)
        Me.ラベル521.Size = New System.Drawing.Size(94, 18)
        Me.ラベル521.Text = "処理年月"
        Me.pnlHeader.Controls.Add(Me.ラベル521)

        ' txt_計上日_曜日
        Me.txt_計上日_曜日.Name = "txt_計上日_曜日"
        Me.txt_計上日_曜日.Location = New System.Drawing.Point(188, 56)
        Me.txt_計上日_曜日.Size = New System.Drawing.Size(22, 18)
        Me.txt_計上日_曜日.Visible = False
        Me.txt_計上日_曜日.TabIndex = 6
        Me.pnlHeader.Controls.Add(Me.txt_計上日_曜日)

        ' cmd_FlexReportDLG
        Me.cmd_FlexReportDLG.Name = "cmd_FlexReportDLG"
        Me.cmd_FlexReportDLG.Location = New System.Drawing.Point(264, 7)
        Me.cmd_FlexReportDLG.Size = New System.Drawing.Size(75, 26)
        Me.cmd_FlexReportDLG.Text = "印刷(&P)"
        Me.cmd_FlexReportDLG.Visible = False
        Me.cmd_FlexReportDLG.TabIndex = 5
        Me.pnlHeader.Controls.Add(Me.cmd_FlexReportDLG)

        ' txt_OUTPUT_FPATH
        Me.txt_OUTPUT_FPATH.Name = "txt_OUTPUT_FPATH"
        Me.txt_OUTPUT_FPATH.Location = New System.Drawing.Point(321, 56)
        Me.txt_OUTPUT_FPATH.Size = New System.Drawing.Size(378, 18)
        Me.txt_OUTPUT_FPATH.TabIndex = 3
        Me.pnlHeader.Controls.Add(Me.txt_OUTPUT_FPATH)

        ' ラベル523
        Me.ラベル523.Name = "ラベル523"
        Me.ラベル523.Location = New System.Drawing.Point(226, 56)
        Me.ラベル523.Size = New System.Drawing.Size(94, 18)
        Me.ラベル523.Text = "出力先ﾌｧｲﾙ名"
        Me.pnlHeader.Controls.Add(Me.ラベル523)

        ' cmd_選択
        Me.cmd_選択.Name = "cmd_選択"
        Me.cmd_選択.Location = New System.Drawing.Point(702, 56)
        Me.cmd_選択.Size = New System.Drawing.Size(56, 18)
        Me.cmd_選択.Text = "選択(&S)"
        Me.cmd_選択.TabIndex = 4
        Me.pnlHeader.Controls.Add(Me.cmd_選択)

        ' txt_仕訳_ID
        Me.txt_仕訳_ID.Name = "txt_仕訳_ID"
        Me.txt_仕訳_ID.Location = New System.Drawing.Point(1020, 56)
        Me.txt_仕訳_ID.Size = New System.Drawing.Size(75, 18)
        Me.txt_仕訳_ID.Visible = False
        Me.txt_仕訳_ID.TabIndex = 7
        Me.pnlHeader.Controls.Add(Me.txt_仕訳_ID)

        ' 会社コード_ラベル
        Me.会社コード_ラベル.Name = "会社コード_ラベル"
        Me.会社コード_ラベル.Location = New System.Drawing.Point(98, 83)
        Me.会社コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.会社コード_ラベル.Text = "会社\015\012コード"
        Me.pnlHeader.Controls.Add(Me.会社コード_ラベル)

        ' 起票社員コード_ラベル
        Me.起票社員コード_ラベル.Name = "起票社員コード_ラベル"
        Me.起票社員コード_ラベル.Location = New System.Drawing.Point(155, 83)
        Me.起票社員コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.起票社員コード_ラベル.Text = "起票社員コード"
        Me.pnlHeader.Controls.Add(Me.起票社員コード_ラベル)

        ' 起票部門コード_ラベル
        Me.起票部門コード_ラベル.Name = "起票部門コード_ラベル"
        Me.起票部門コード_ラベル.Location = New System.Drawing.Point(211, 83)
        Me.起票部門コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.起票部門コード_ラベル.Text = "起票部門コード"
        Me.pnlHeader.Controls.Add(Me.起票部門コード_ラベル)

        ' 承認社員コード_ラベル
        Me.承認社員コード_ラベル.Name = "承認社員コード_ラベル"
        Me.承認社員コード_ラベル.Location = New System.Drawing.Point(268, 83)
        Me.承認社員コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.承認社員コード_ラベル.Text = "承認社員コード"
        Me.pnlHeader.Controls.Add(Me.承認社員コード_ラベル)

        ' 承認日付_ラベル
        Me.承認日付_ラベル.Name = "承認日付_ラベル"
        Me.承認日付_ラベル.Location = New System.Drawing.Point(325, 83)
        Me.承認日付_ラベル.Size = New System.Drawing.Size(68, 37)
        Me.承認日付_ラベル.Text = "承認日付"
        Me.pnlHeader.Controls.Add(Me.承認日付_ラベル)

        ' 承認状態区分_ラベル
        Me.承認状態区分_ラベル.Name = "承認状態区分_ラベル"
        Me.承認状態区分_ラベル.Location = New System.Drawing.Point(393, 83)
        Me.承認状態区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.承認状態区分_ラベル.Text = "承認状態区分"
        Me.pnlHeader.Controls.Add(Me.承認状態区分_ラベル)

        ' 仕訳種別区分_ラベル
        Me.仕訳種別区分_ラベル.Name = "仕訳種別区分_ラベル"
        Me.仕訳種別区分_ラベル.Location = New System.Drawing.Point(431, 83)
        Me.仕訳種別区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.仕訳種別区分_ラベル.Text = "仕訳種別区分"
        Me.pnlHeader.Controls.Add(Me.仕訳種別区分_ラベル)

        ' 伝票日付_ラベル
        Me.伝票日付_ラベル.Name = "伝票日付_ラベル"
        Me.伝票日付_ラベル.Location = New System.Drawing.Point(468, 83)
        Me.伝票日付_ラベル.Size = New System.Drawing.Size(68, 37)
        Me.伝票日付_ラベル.Text = "伝票日付"
        Me.pnlHeader.Controls.Add(Me.伝票日付_ラベル)

        ' 伝票番号_ラベル
        Me.伝票番号_ラベル.Name = "伝票番号_ラベル"
        Me.伝票番号_ラベル.Location = New System.Drawing.Point(536, 83)
        Me.伝票番号_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.伝票番号_ラベル.Text = "伝票番号"
        Me.pnlHeader.Controls.Add(Me.伝票番号_ラベル)

        ' 伝票操作禁止区分_ラベル
        Me.伝票操作禁止区分_ラベル.Name = "伝票操作禁止区分_ラベル"
        Me.伝票操作禁止区分_ラベル.Location = New System.Drawing.Point(593, 83)
        Me.伝票操作禁止区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.伝票操作禁止区分_ラベル.Text = "伝票操作禁止区分"
        Me.pnlHeader.Controls.Add(Me.伝票操作禁止区分_ラベル)

        ' 伝票備考_ラベル
        Me.伝票備考_ラベル.Name = "伝票備考_ラベル"
        Me.伝票備考_ラベル.Location = New System.Drawing.Point(631, 83)
        Me.伝票備考_ラベル.Size = New System.Drawing.Size(75, 37)
        Me.伝票備考_ラベル.Text = "伝票備考"
        Me.pnlHeader.Controls.Add(Me.伝票備考_ラベル)

        ' 行番号_ラベル
        Me.行番号_ラベル.Name = "行番号_ラベル"
        Me.行番号_ラベル.Location = New System.Drawing.Point(707, 83)
        Me.行番号_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.行番号_ラベル.Text = "行番号"
        Me.pnlHeader.Controls.Add(Me.行番号_ラベル)

        ' 伝票明細貸借区分_ラベル
        Me.伝票明細貸借区分_ラベル.Name = "伝票明細貸借区分_ラベル"
        Me.伝票明細貸借区分_ラベル.Location = New System.Drawing.Point(763, 83)
        Me.伝票明細貸借区分_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.伝票明細貸借区分_ラベル.Text = "伝票明細貸借区分"
        Me.pnlHeader.Controls.Add(Me.伝票明細貸借区分_ラベル)

        ' 細目コード_ラベル
        Me.細目コード_ラベル.Name = "細目コード_ラベル"
        Me.細目コード_ラベル.Location = New System.Drawing.Point(990, 83)
        Me.細目コード_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.細目コード_ラベル.Text = "細目\015\012コード"
        Me.pnlHeader.Controls.Add(Me.細目コード_ラベル)

        ' 集計拡張コード１識別区分_ラベル
        Me.集計拡張コード１識別区分_ラベル.Name = "集計拡張コード１識別区分_ラベル"
        Me.集計拡張コード１識別区分_ラベル.Location = New System.Drawing.Point(1047, 83)
        Me.集計拡張コード１識別区分_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.集計拡張コード１識別区分_ラベル.Text = "集計拡張ｺｰﾄﾞ1識別区分"
        Me.pnlHeader.Controls.Add(Me.集計拡張コード１識別区分_ラベル)

        ' 集計拡張コード１_ラベル
        Me.集計拡張コード１_ラベル.Name = "集計拡張コード１_ラベル"
        Me.集計拡張コード１_ラベル.Location = New System.Drawing.Point(1104, 83)
        Me.集計拡張コード１_ラベル.Size = New System.Drawing.Size(56, 37)
        Me.集計拡張コード１_ラベル.Text = "集計拡張\015\012ｺｰﾄﾞ1"
        Me.pnlHeader.Controls.Add(Me.集計拡張コード１_ラベル)

        ' 課税区分_ラベル
        Me.課税区分_ラベル.Name = "課税区分_ラベル"
        Me.課税区分_ラベル.Location = New System.Drawing.Point(1754, 83)
        Me.課税区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.課税区分_ラベル.Text = "課税区分"
        Me.pnlHeader.Controls.Add(Me.課税区分_ラベル)

        ' 税率区分_ラベル
        Me.税率区分_ラベル.Name = "税率区分_ラベル"
        Me.税率区分_ラベル.Location = New System.Drawing.Point(1433, 83)
        Me.税率区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.税率区分_ラベル.Text = "税率区分"
        Me.pnlHeader.Controls.Add(Me.税率区分_ラベル)

        ' 取引通貨発生金額_ラベル
        Me.取引通貨発生金額_ラベル.Name = "取引通貨発生金額_ラベル"
        Me.取引通貨発生金額_ラベル.Location = New System.Drawing.Point(1565, 83)
        Me.取引通貨発生金額_ラベル.Size = New System.Drawing.Size(94, 37)
        Me.取引通貨発生金額_ラベル.Text = "取引通貨発生金額"
        Me.pnlHeader.Controls.Add(Me.取引通貨発生金額_ラベル)

        ' 入力システム区分_ラベル
        Me.入力システム区分_ラベル.Name = "入力システム区分_ラベル"
        Me.入力システム区分_ラベル.Location = New System.Drawing.Point(60, 83)
        Me.入力システム区分_ラベル.Size = New System.Drawing.Size(37, 37)
        Me.入力システム区分_ラベル.Text = "入力\015\012ｼｽﾃﾑ\015\012区分"
        Me.pnlHeader.Controls.Add(Me.入力システム区分_ラベル)

        ' pnlDetail
        Me.pnlDetail.Name = "pnlDetail"
        Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill

        ' 入力番号
        Me.入力番号.Name = "入力番号"
        Me.入力番号.Location = New System.Drawing.Point(3, 0)
        Me.入力番号.Size = New System.Drawing.Size(56, 18)
        Me.pnlDetail.Controls.Add(Me.入力番号)

        ' 細目コード識別区分
        Me.細目コード識別区分.Name = "細目コード識別区分"
        Me.細目コード識別区分.Location = New System.Drawing.Point(934, 0)
        Me.細目コード識別区分.Size = New System.Drawing.Size(56, 18)
        Me.細目コード識別区分.TabIndex = 17
        Me.pnlDetail.Controls.Add(Me.細目コード識別区分)

        ' 勘定科目コード
        Me.勘定科目コード.Name = "勘定科目コード"
        Me.勘定科目コード.Location = New System.Drawing.Point(820, 0)
        Me.勘定科目コード.Size = New System.Drawing.Size(56, 18)
        Me.勘定科目コード.TabIndex = 15
        Me.pnlDetail.Controls.Add(Me.勘定科目コード)

        ' 会計部門コード
        Me.会計部門コード.Name = "会計部門コード"
        Me.会計部門コード.Location = New System.Drawing.Point(877, 0)
        Me.会計部門コード.Size = New System.Drawing.Size(56, 18)
        Me.会計部門コード.TabIndex = 16
        Me.pnlDetail.Controls.Add(Me.会計部門コード)

        ' 取引先コード
        Me.取引先コード.Name = "取引先コード"
        Me.取引先コード.Location = New System.Drawing.Point(1160, 0)
        Me.取引先コード.Size = New System.Drawing.Size(56, 18)
        Me.取引先コード.TabIndex = 21
        Me.pnlDetail.Controls.Add(Me.取引先コード)

        ' 履歴物件コード
        Me.履歴物件コード.Name = "履歴物件コード"
        Me.履歴物件コード.Location = New System.Drawing.Point(1792, 0)
        Me.履歴物件コード.Size = New System.Drawing.Size(56, 18)
        Me.履歴物件コード.TabIndex = 31
        Me.pnlDetail.Controls.Add(Me.履歴物件コード)

        ' セグメントコード
        Me.セグメントコード.Name = "セグメントコード"
        Me.セグメントコード.Location = New System.Drawing.Point(1217, 0)
        Me.セグメントコード.Size = New System.Drawing.Size(56, 18)
        Me.セグメントコード.TabIndex = 22
        Me.pnlDetail.Controls.Add(Me.セグメントコード)

        ' 取引通貨コード
        Me.取引通貨コード.Name = "取引通貨コード"
        Me.取引通貨コード.Location = New System.Drawing.Point(1274, 0)
        Me.取引通貨コード.Size = New System.Drawing.Size(56, 18)
        Me.取引通貨コード.TabIndex = 23
        Me.pnlDetail.Controls.Add(Me.取引通貨コード)

        ' 資金コード
        Me.資金コード.Name = "資金コード"
        Me.資金コード.Location = New System.Drawing.Point(1331, 0)
        Me.資金コード.Size = New System.Drawing.Size(56, 18)
        Me.資金コード.TabIndex = 24
        Me.pnlDetail.Controls.Add(Me.資金コード)

        ' 基本通貨発生金額
        Me.基本通貨発生金額.Name = "基本通貨発生金額"
        Me.基本通貨発生金額.Location = New System.Drawing.Point(1471, 0)
        Me.基本通貨発生金額.Size = New System.Drawing.Size(94, 18)
        Me.基本通貨発生金額.TabIndex = 27
        Me.pnlDetail.Controls.Add(Me.基本通貨発生金額)

        ' 消費税区分コード
        Me.消費税区分コード.Name = "消費税区分コード"
        Me.消費税区分コード.Location = New System.Drawing.Point(1387, 0)
        Me.消費税区分コード.Size = New System.Drawing.Size(45, 18)
        Me.消費税区分コード.TabIndex = 25
        Me.pnlDetail.Controls.Add(Me.消費税区分コード)

        ' 参考消費税金額
        Me.参考消費税金額.Name = "参考消費税金額"
        Me.参考消費税金額.Location = New System.Drawing.Point(1660, 0)
        Me.参考消費税金額.Size = New System.Drawing.Size(94, 18)
        Me.参考消費税金額.TabIndex = 29
        Me.pnlDetail.Controls.Add(Me.参考消費税金額)

        ' 文字摘要１
        Me.文字摘要１.Name = "文字摘要１"
        Me.文字摘要１.Location = New System.Drawing.Point(1849, 0)
        Me.文字摘要１.Size = New System.Drawing.Size(253, 18)
        Me.文字摘要１.TabIndex = 32
        Me.pnlDetail.Controls.Add(Me.文字摘要１)

        ' 会社コード
        Me.会社コード.Name = "会社コード"
        Me.会社コード.Location = New System.Drawing.Point(98, 0)
        Me.会社コード.Size = New System.Drawing.Size(56, 18)
        Me.会社コード.TabIndex = 2
        Me.pnlDetail.Controls.Add(Me.会社コード)

        ' 起票社員コード
        Me.起票社員コード.Name = "起票社員コード"
        Me.起票社員コード.Location = New System.Drawing.Point(155, 0)
        Me.起票社員コード.Size = New System.Drawing.Size(56, 18)
        Me.起票社員コード.TabIndex = 3
        Me.pnlDetail.Controls.Add(Me.起票社員コード)

        ' 起票部門コード
        Me.起票部門コード.Name = "起票部門コード"
        Me.起票部門コード.Location = New System.Drawing.Point(211, 0)
        Me.起票部門コード.Size = New System.Drawing.Size(56, 18)
        Me.起票部門コード.TabIndex = 4
        Me.pnlDetail.Controls.Add(Me.起票部門コード)

        ' 承認社員コード
        Me.承認社員コード.Name = "承認社員コード"
        Me.承認社員コード.Location = New System.Drawing.Point(268, 0)
        Me.承認社員コード.Size = New System.Drawing.Size(56, 18)
        Me.承認社員コード.TabIndex = 5
        Me.pnlDetail.Controls.Add(Me.承認社員コード)

        ' 承認日付
        Me.承認日付.Name = "承認日付"
        Me.承認日付.Location = New System.Drawing.Point(325, 0)
        Me.承認日付.Size = New System.Drawing.Size(68, 18)
        Me.承認日付.TabIndex = 6
        Me.pnlDetail.Controls.Add(Me.承認日付)

        ' 承認状態区分
        Me.承認状態区分.Name = "承認状態区分"
        Me.承認状態区分.Location = New System.Drawing.Point(393, 0)
        Me.承認状態区分.Size = New System.Drawing.Size(37, 18)
        Me.承認状態区分.TabIndex = 7
        Me.pnlDetail.Controls.Add(Me.承認状態区分)

        ' 仕訳種別区分
        Me.仕訳種別区分.Name = "仕訳種別区分"
        Me.仕訳種別区分.Location = New System.Drawing.Point(431, 0)
        Me.仕訳種別区分.Size = New System.Drawing.Size(37, 18)
        Me.仕訳種別区分.TabIndex = 8
        Me.pnlDetail.Controls.Add(Me.仕訳種別区分)

        ' 伝票日付
        Me.伝票日付.Name = "伝票日付"
        Me.伝票日付.Location = New System.Drawing.Point(468, 0)
        Me.伝票日付.Size = New System.Drawing.Size(68, 18)
        Me.伝票日付.TabIndex = 9
        Me.pnlDetail.Controls.Add(Me.伝票日付)

        ' 伝票番号
        Me.伝票番号.Name = "伝票番号"
        Me.伝票番号.Location = New System.Drawing.Point(536, 0)
        Me.伝票番号.Size = New System.Drawing.Size(56, 18)
        Me.伝票番号.TabIndex = 10
        Me.pnlDetail.Controls.Add(Me.伝票番号)

        ' 伝票操作禁止区分
        Me.伝票操作禁止区分.Name = "伝票操作禁止区分"
        Me.伝票操作禁止区分.Location = New System.Drawing.Point(593, 0)
        Me.伝票操作禁止区分.Size = New System.Drawing.Size(37, 18)
        Me.伝票操作禁止区分.TabIndex = 11
        Me.pnlDetail.Controls.Add(Me.伝票操作禁止区分)

        ' 伝票備考
        Me.伝票備考.Name = "伝票備考"
        Me.伝票備考.Location = New System.Drawing.Point(631, 0)
        Me.伝票備考.Size = New System.Drawing.Size(75, 18)
        Me.伝票備考.TabIndex = 12
        Me.pnlDetail.Controls.Add(Me.伝票備考)

        ' 行番号
        Me.行番号.Name = "行番号"
        Me.行番号.Location = New System.Drawing.Point(707, 0)
        Me.行番号.Size = New System.Drawing.Size(56, 18)
        Me.行番号.TabIndex = 13
        Me.pnlDetail.Controls.Add(Me.行番号)

        ' 伝票明細貸借区分
        Me.伝票明細貸借区分.Name = "伝票明細貸借区分"
        Me.伝票明細貸借区分.Location = New System.Drawing.Point(763, 0)
        Me.伝票明細貸借区分.Size = New System.Drawing.Size(56, 18)
        Me.伝票明細貸借区分.TabIndex = 14
        Me.pnlDetail.Controls.Add(Me.伝票明細貸借区分)

        ' 細目コード
        Me.細目コード.Name = "細目コード"
        Me.細目コード.Location = New System.Drawing.Point(990, 0)
        Me.細目コード.Size = New System.Drawing.Size(56, 18)
        Me.細目コード.TabIndex = 18
        Me.pnlDetail.Controls.Add(Me.細目コード)

        ' 集計拡張コード１識別区分
        Me.集計拡張コード１識別区分.Name = "集計拡張コード１識別区分"
        Me.集計拡張コード１識別区分.Location = New System.Drawing.Point(1047, 0)
        Me.集計拡張コード１識別区分.Size = New System.Drawing.Size(56, 18)
        Me.集計拡張コード１識別区分.TabIndex = 19
        Me.pnlDetail.Controls.Add(Me.集計拡張コード１識別区分)

        ' 集計拡張コード１
        Me.集計拡張コード１.Name = "集計拡張コード１"
        Me.集計拡張コード１.Location = New System.Drawing.Point(1104, 0)
        Me.集計拡張コード１.Size = New System.Drawing.Size(56, 18)
        Me.集計拡張コード１.TabIndex = 20
        Me.pnlDetail.Controls.Add(Me.集計拡張コード１)

        ' 課税区分
        Me.課税区分.Name = "課税区分"
        Me.課税区分.Location = New System.Drawing.Point(1754, 0)
        Me.課税区分.Size = New System.Drawing.Size(37, 18)
        Me.課税区分.TabIndex = 30
        Me.pnlDetail.Controls.Add(Me.課税区分)

        ' 税率区分
        Me.税率区分.Name = "税率区分"
        Me.税率区分.Location = New System.Drawing.Point(1433, 0)
        Me.税率区分.Size = New System.Drawing.Size(37, 18)
        Me.税率区分.TabIndex = 26
        Me.pnlDetail.Controls.Add(Me.税率区分)

        ' 取引通貨発生金額
        Me.取引通貨発生金額.Name = "取引通貨発生金額"
        Me.取引通貨発生金額.Location = New System.Drawing.Point(1565, 0)
        Me.取引通貨発生金額.Size = New System.Drawing.Size(94, 18)
        Me.取引通貨発生金額.TabIndex = 28
        Me.pnlDetail.Controls.Add(Me.取引通貨発生金額)

        ' 入力システム区分
        Me.入力システム区分.Name = "入力システム区分"
        Me.入力システム区分.Location = New System.Drawing.Point(60, 0)
        Me.入力システム区分.Size = New System.Drawing.Size(37, 18)
        Me.入力システム区分.TabIndex = 1
        Me.pnlDetail.Controls.Add(Me.入力システム区分)

        ' pnlFooter
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Size = New System.Drawing.Size(2105, 45)

        ' ラベル34
        Me.ラベル34.Name = "ラベル34"
        Me.ラベル34.Location = New System.Drawing.Point(1409, 0)
        Me.ラベル34.Size = New System.Drawing.Size(56, 18)
        Me.ラベル34.Text = "借方合計"
        Me.pnlFooter.Controls.Add(Me.ラベル34)

        ' ラベル35
        Me.ラベル35.Name = "ラベル35"
        Me.ラベル35.Location = New System.Drawing.Point(1409, 18)
        Me.ラベル35.Size = New System.Drawing.Size(56, 18)
        Me.ラベル35.Text = "貸方合計"
        Me.pnlFooter.Controls.Add(Me.ラベル35)

        ' txt_借方金額_SUM
        Me.txt_借方金額_SUM.Name = "txt_借方金額_SUM"
        Me.txt_借方金額_SUM.Location = New System.Drawing.Point(1471, 0)
        Me.txt_借方金額_SUM.Size = New System.Drawing.Size(94, 18)
        Me.pnlFooter.Controls.Add(Me.txt_借方金額_SUM)

        ' txt_貸方金額_SUM
        Me.txt_貸方金額_SUM.Name = "txt_貸方金額_SUM"
        Me.txt_貸方金額_SUM.Location = New System.Drawing.Point(1471, 18)
        Me.txt_貸方金額_SUM.Size = New System.Drawing.Size(94, 18)
        Me.txt_貸方金額_SUM.TabIndex = 1
        Me.pnlFooter.Controls.Add(Me.txt_貸方金額_SUM)

        Me.Name = "Frmfc_仕訳出力_最終確認_RISO"
        Me.Text = "仕訳出力　最終確認"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents unnamed_Label_1917970269568 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Rectangle_1917970272448 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_Line_1917970274176 As System.Windows.Forms.Label
    Friend WithEvents unnamed_Image_1917970271104 As System.Windows.Forms.PictureBox
    Friend WithEvents unnamed_CommandButton_1917970275712 As System.Windows.Forms.Button
    Friend WithEvents unnamed_OptionButton_1917970277248 As System.Windows.Forms.RadioButton
    Friend WithEvents unnamed_CheckBox_1917970279040 As System.Windows.Forms.CheckBox
    Friend WithEvents unnamed_OptionGroup_1917970280896 As System.Windows.Forms.GroupBox
    Friend WithEvents unnamed_TextBox_1917977379840 As System.Windows.Forms.TextBox
    Friend WithEvents unnamed_ListBox_1917977381376 As System.Windows.Forms.ListBox
    Friend WithEvents unnamed_ComboBox_1917977382976 As System.Windows.Forms.ComboBox
    Friend WithEvents unnamed_Subform_1917977385856 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_UnboundObjectFrame_1917977387584 As System.Windows.Forms.Panel
    Friend WithEvents unnamed_Tab_1917977390720 As System.Windows.Forms.TabControl
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents 入力番号_ラベル As System.Windows.Forms.Label
    Friend WithEvents 細目コード識別区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 勘定科目コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 会計部門コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 取引先コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 履歴物件コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents セグメントコード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 取引通貨コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 資金コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 基本通貨発生金額_ラベル As System.Windows.Forms.Label
    Friend WithEvents 消費税区分コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 参考消費税金額_ラベル As System.Windows.Forms.Label
    Friend WithEvents 文字摘要１_ラベル As System.Windows.Forms.Label
    Friend WithEvents cmd_実行 As System.Windows.Forms.Button
    Friend WithEvents cmd_CANCEL As System.Windows.Forms.Button
    Friend WithEvents txt_処理年月 As System.Windows.Forms.TextBox
    Friend WithEvents ラベル521 As System.Windows.Forms.Label
    Friend WithEvents txt_計上日_曜日 As System.Windows.Forms.TextBox
    Friend WithEvents cmd_FlexReportDLG As System.Windows.Forms.Button
    Friend WithEvents txt_OUTPUT_FPATH As System.Windows.Forms.TextBox
    Friend WithEvents ラベル523 As System.Windows.Forms.Label
    Friend WithEvents cmd_選択 As System.Windows.Forms.Button
    Friend WithEvents txt_仕訳_ID As System.Windows.Forms.TextBox
    Friend WithEvents 会社コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 起票社員コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 起票部門コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 承認社員コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 承認日付_ラベル As System.Windows.Forms.Label
    Friend WithEvents 承認状態区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 仕訳種別区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 伝票日付_ラベル As System.Windows.Forms.Label
    Friend WithEvents 伝票番号_ラベル As System.Windows.Forms.Label
    Friend WithEvents 伝票操作禁止区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 伝票備考_ラベル As System.Windows.Forms.Label
    Friend WithEvents 行番号_ラベル As System.Windows.Forms.Label
    Friend WithEvents 伝票明細貸借区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 細目コード_ラベル As System.Windows.Forms.Label
    Friend WithEvents 集計拡張コード１識別区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 集計拡張コード１_ラベル As System.Windows.Forms.Label
    Friend WithEvents 課税区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 税率区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents 取引通貨発生金額_ラベル As System.Windows.Forms.Label
    Friend WithEvents 入力システム区分_ラベル As System.Windows.Forms.Label
    Friend WithEvents pnlDetail As System.Windows.Forms.Panel
    Friend WithEvents 入力番号 As System.Windows.Forms.TextBox
    Friend WithEvents 細目コード識別区分 As System.Windows.Forms.TextBox
    Friend WithEvents 勘定科目コード As System.Windows.Forms.TextBox
    Friend WithEvents 会計部門コード As System.Windows.Forms.TextBox
    Friend WithEvents 取引先コード As System.Windows.Forms.TextBox
    Friend WithEvents 履歴物件コード As System.Windows.Forms.TextBox
    Friend WithEvents セグメントコード As System.Windows.Forms.TextBox
    Friend WithEvents 取引通貨コード As System.Windows.Forms.TextBox
    Friend WithEvents 資金コード As System.Windows.Forms.TextBox
    Friend WithEvents 基本通貨発生金額 As System.Windows.Forms.TextBox
    Friend WithEvents 消費税区分コード As System.Windows.Forms.TextBox
    Friend WithEvents 参考消費税金額 As System.Windows.Forms.TextBox
    Friend WithEvents 文字摘要１ As System.Windows.Forms.TextBox
    Friend WithEvents 会社コード As System.Windows.Forms.TextBox
    Friend WithEvents 起票社員コード As System.Windows.Forms.TextBox
    Friend WithEvents 起票部門コード As System.Windows.Forms.TextBox
    Friend WithEvents 承認社員コード As System.Windows.Forms.TextBox
    Friend WithEvents 承認日付 As System.Windows.Forms.TextBox
    Friend WithEvents 承認状態区分 As System.Windows.Forms.TextBox
    Friend WithEvents 仕訳種別区分 As System.Windows.Forms.TextBox
    Friend WithEvents 伝票日付 As System.Windows.Forms.TextBox
    Friend WithEvents 伝票番号 As System.Windows.Forms.TextBox
    Friend WithEvents 伝票操作禁止区分 As System.Windows.Forms.TextBox
    Friend WithEvents 伝票備考 As System.Windows.Forms.TextBox
    Friend WithEvents 行番号 As System.Windows.Forms.TextBox
    Friend WithEvents 伝票明細貸借区分 As System.Windows.Forms.TextBox
    Friend WithEvents 細目コード As System.Windows.Forms.TextBox
    Friend WithEvents 集計拡張コード１識別区分 As System.Windows.Forms.TextBox
    Friend WithEvents 集計拡張コード１ As System.Windows.Forms.TextBox
    Friend WithEvents 課税区分 As System.Windows.Forms.TextBox
    Friend WithEvents 税率区分 As System.Windows.Forms.TextBox
    Friend WithEvents 取引通貨発生金額 As System.Windows.Forms.TextBox
    Friend WithEvents 入力システム区分 As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents ラベル34 As System.Windows.Forms.Label
    Friend WithEvents ラベル35 As System.Windows.Forms.Label
    Friend WithEvents txt_借方金額_SUM As System.Windows.Forms.TextBox
    Friend WithEvents txt_貸方金額_SUM As System.Windows.Forms.TextBox

End Class
