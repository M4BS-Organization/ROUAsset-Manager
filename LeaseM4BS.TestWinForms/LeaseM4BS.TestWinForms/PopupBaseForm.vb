Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' ポップアップ画面の共通基本クラス。
''' 画面表示時にデスクトップの作業領域内に収まるようサイズ・位置を自動補正する。
''' </summary>
Public Class PopupBaseForm
    Inherits Form

    ''' <summary>
    ''' デザイナー用コンストラクタ。直接インスタンス化を防止（派生クラスのみ使用可）。
    ''' </summary>
    Public Sub New()
        If Me.GetType() Is GetType(PopupBaseForm) AndAlso
           LicenseManager.UsageMode <> LicenseUsageMode.Designtime Then
            Throw New InvalidOperationException(
                "PopupBaseForm は直接インスタンス化できません。派生クラスを使用してください。")
        End If
    End Sub

    ''' <summary>
    ''' フォーム表示時に作業領域内に収まるようサイズ・位置を補正する。
    ''' </summary>
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        AdjustToWorkingArea()
    End Sub

    ''' <summary>
    ''' 画面のサイズと位置をデスクトップ作業領域内に収める。
    ''' 高さが作業領域を超える場合は自動縮小する。
    ''' 上端がマイナス座標にならないことを保証する。
    ''' </summary>
    Private Sub AdjustToWorkingArea()
        Dim workArea As Rectangle = Screen.FromControl(Me).WorkingArea

        ' 画面サイズが作業領域を超える場合は縮小
        Dim newWidth As Integer = Math.Min(Me.Width, workArea.Width)
        Dim newHeight As Integer = Math.Min(Me.Height, workArea.Height)

        If newWidth <> Me.Width OrElse newHeight <> Me.Height Then
            Me.Size = New Size(newWidth, newHeight)
        End If

        ' 画面位置を作業領域内に補正
        Dim newLeft As Integer = Me.Left
        Dim newTop As Integer = Me.Top

        ' 左端が作業領域外にならないよう補正
        If newLeft < workArea.Left Then
            newLeft = workArea.Left
        End If

        ' 上端が作業領域外（マイナス座標等）にならないよう補正
        If newTop < workArea.Top Then
            newTop = workArea.Top
        End If

        ' 右端が作業領域外にならないよう補正
        If newLeft + newWidth > workArea.Right Then
            newLeft = workArea.Right - newWidth
        End If

        ' 下端が作業領域外にならないよう補正
        If newTop + newHeight > workArea.Bottom Then
            newTop = workArea.Bottom - newHeight
        End If

        If newLeft <> Me.Left OrElse newTop <> Me.Top Then
            Me.Location = New Point(newLeft, newTop)
        End If
    End Sub

End Class
