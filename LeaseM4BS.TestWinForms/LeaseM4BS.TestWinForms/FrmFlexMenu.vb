Imports System
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' フレックスメニュー画面
''' 各機能へアクセスするためのポータル画面。
''' メニューバーから子画面（UserControl）を切り替えて表示する。
''' </summary>
Partial Public Class FrmFlexMenu

    Private ReadOnly CLR_HEADER As Color = Color.FromArgb(0, 51, 102)
    Private ReadOnly CLR_ACTIVE As Color = Color.FromArgb(0, 80, 160)
    Private ReadOnly CLR_HOVER As Color = Color.FromArgb(0, 70, 140)

    ''' <summary>
    ''' 現在アクティブなメニューボタン
    ''' </summary>
    Private _activeButton As Button = Nothing

    ''' <summary>
    ''' 現在表示中の子画面（UserControl）
    ''' </summary>
    Private _currentContent As UserControl = Nothing

    Public Sub New()
        InitializeComponent()
        SetupMenuButtons()
        ' デフォルトで契約書（フレックス）を表示
        SwitchContent(btnContract)
    End Sub

    ''' <summary>
    ''' メニューボタンの共通イベントハンドラを設定する
    ''' </summary>
    Private Sub SetupMenuButtons()
        Dim menuButtons() As Button = {
            btnContract, btnROUAsset, btnMonthlyPayments,
            btnMonthlyAccounting, btnPeriodBalance, btnTaxAdjustment,
            btnMaster
        }

        For Each btn As Button In menuButtons
            btn.Cursor = Cursors.Hand
            btn.BackColor = CLR_HEADER
            btn.FlatAppearance.MouseOverBackColor = CLR_HOVER

            AddHandler btn.Click, AddressOf MenuButton_Click
        Next
    End Sub

    ''' <summary>
    ''' メニューボタンクリック時の共通処理
    ''' </summary>
    Private Sub MenuButton_Click(sender As Object, e As EventArgs)
        Dim clickedButton As Button = DirectCast(sender, Button)
        SwitchContent(clickedButton)
    End Sub

    ''' <summary>
    ''' コンテンツパネルの子画面を切り替える
    ''' </summary>
    Private Sub SwitchContent(menuButton As Button)
        ' 同じボタンが再度クリックされた場合は何もしない
        If menuButton Is _activeButton Then Return

        ' アクティブボタンのスタイルを更新
        If _activeButton IsNot Nothing Then
            _activeButton.BackColor = CLR_HEADER
        End If
        menuButton.BackColor = CLR_ACTIVE
        _activeButton = menuButton

        ' 既存のコンテンツを破棄
        If _currentContent IsNot Nothing Then
            pnlContent.Controls.Remove(_currentContent)
            _currentContent.Dispose()
            _currentContent = Nothing
        End If

        ' 新しいコンテンツを作成
        Dim newContent As UserControl = CreateContentForButton(menuButton)
        If newContent IsNot Nothing Then
            newContent.Dock = DockStyle.Fill
            pnlContent.Controls.Add(newContent)
            _currentContent = newContent
        End If
    End Sub

    ''' <summary>
    ''' メニューボタンに対応する子画面（UserControl）を生成する
    ''' </summary>
    Private Function CreateContentForButton(menuButton As Button) As UserControl
        If menuButton Is btnContract Then
            Return New FrmFlexContract()
        ElseIf menuButton Is btnROUAsset Then
            Return New FrmFlexROUAsset()
        ElseIf menuButton Is btnMonthlyPayments Then
            Return New FrmFlexMonthlyPayments()
        ElseIf menuButton Is btnMonthlyAccounting Then
            Return New FrmFlexMonthlyAccounting()
        ElseIf menuButton Is btnPeriodBalance Then
            Return New FrmFlexPeriodBalance()
        ElseIf menuButton Is btnTaxAdjustment Then
            Return New FrmFlexTaxAdjustment()
        ElseIf menuButton Is btnMaster Then
            Return New FrmFlexMaster()
        End If
        Return Nothing
    End Function

End Class
