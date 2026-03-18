' =========================================================
' SecurityChecker - アクセス権チェックモジュール
' Access版 p_SEC.txt のVB.NET移植
' =========================================================
Imports System.Windows.Forms

Public Module SecurityChecker

    ''' <summary>
    ''' Access版 engACCESS_KIND に対応
    ''' </summary>
    Public Enum AccessKindEnum
        Update = 1      ' cngACCESS_KIND_UPD — 全データ変更
        Reference = 2   ' cngACCESS_KIND_REF — 全データ参照
        UnitBased = 3   ' cngACCESS_KIND_OTH — 管理単位限定
    End Enum

    ' =========================================================
    '  権限チェック関数
    ' =========================================================

    ''' <summary>
    ''' 更新可否を判定する (Access版 p_SEC.gSECCHK_UPD 相当)
    ''' </summary>
    Public Function CanUpdate() As Boolean
        Try
            Select Case LoginSession.AccessKind
                Case AccessKindEnum.Update
                    ' 全データ変更 → 更新可
                    Return True

                Case AccessKindEnum.Reference
                    ' 全データ参照 → 更新不可
                    Return False

                Case AccessKindEnum.UnitBased
                    ' 管理単位限定 → KknriListに更新権限を持つエントリがあれば更新可
                    For Each entry In LoginSession.KknriList
                        If entry.AccessKind = AccessKindEnum.Update Then
                            Return True
                        End If
                    Next
                    Return False

                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 特定の契約管理単位に対して更新権限があるか (Access版 gSECCHK_KKNRI_UPD 相当)
    ''' </summary>
    Public Function CanUpdateKknri(kknriId As Integer) As Boolean
        Try
            Select Case LoginSession.AccessKind
                Case AccessKindEnum.Update
                    Return True
                Case AccessKindEnum.Reference
                    Return False
                Case AccessKindEnum.UnitBased
                    For Each entry In LoginSession.KknriList
                        If entry.KknriId = kknriId Then
                            Return (entry.AccessKind = AccessKindEnum.Update)
                        End If
                    Next
                    Return False
                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 特定の契約管理単位に対して参照権限があるか (Access版 gSECCHK_KKNRI_REF 相当)
    ''' </summary>
    Public Function CanRefKknri(kknriId As Integer) As Boolean
        Try
            Select Case LoginSession.AccessKind
                Case AccessKindEnum.Update, AccessKindEnum.Reference
                    ' 全データ変更 or 全データ参照 → 参照可
                    Return True
                Case AccessKindEnum.UnitBased
                    For Each entry In LoginSession.KknriList
                        If entry.KknriId = kknriId Then
                            Return (entry.AccessKind = AccessKindEnum.Update OrElse
                                    entry.AccessKind = AccessKindEnum.Reference)
                        End If
                    Next
                    Return False
                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 部門管理単位に対して更新権限があるか (Access版 gSECCHK_UPD_BKN 相当)
    ''' </summary>
    Public Function CanUpdateBknri(bknriId As Integer) As Boolean
        Try
            Select Case LoginSession.AccessKindB
                Case AccessKindEnum.Update
                    Return True
                Case AccessKindEnum.Reference
                    Return False
                Case AccessKindEnum.UnitBased
                    For Each entry In LoginSession.BknriList
                        If entry.BknriId = bknriId Then
                            Return (entry.AccessKind = AccessKindEnum.Update)
                        End If
                    Next
                    Return False
                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 部門管理単位に対して参照権限があるか
    ''' </summary>
    Public Function CanRefBknri(bknriId As Integer) As Boolean
        Try
            Select Case LoginSession.AccessKindB
                Case AccessKindEnum.Update, AccessKindEnum.Reference
                    Return True
                Case AccessKindEnum.UnitBased
                    For Each entry In LoginSession.BknriList
                        If entry.BknriId = bknriId Then
                            Return (entry.AccessKind = AccessKindEnum.Update OrElse
                                    entry.AccessKind = AccessKindEnum.Reference)
                        End If
                    Next
                    Return False
                Case Else
                    Return False
            End Select
        Catch
            Return False
        End Try
    End Function

    ' =========================================================
    '  UI制御関数
    ' =========================================================

    ''' <summary>
    ''' マスタ画面のボタン制御 (Access版 gMstUpdLimitChk 相当)
    ''' マスタ更新権限・ファイル出力権限に基づきボタンのEnabled制御
    ''' </summary>
    Public Sub ApplyMasterUpdateLimit(form As Form)
        Try
            ' マスタ更新権限
            Dim canMaster As Boolean = LoginSession.CanMasterUpdate
            SetControlEnabled(form, "cmd_新規", canMaster)
            SetControlEnabled(form, "cmd_変更", canMaster)
            SetControlEnabled(form, "cmd_Touroku", canMaster)
            SetControlEnabled(form, "cmd_Del", canMaster)

            ' ファイル出力権限
            SetControlEnabled(form, "cmd_Output", LoginSession.CanFileOutput)

            ' 印刷権限
            SetControlEnabled(form, "cmd_FlexReportDLG", LoginSession.CanPrint)
        Catch
            ' UI制御失敗は無視
        End Try
    End Sub

    ''' <summary>
    ''' データ画面のボタン制御 (Access版 gDataUpdLimitChk 相当)
    ''' AccessKind + 契約管理単位に基づき更新/参照制御
    ''' </summary>
    Public Sub ApplyDataUpdateLimit(form As Form, Optional kknriId As Integer = 0)
        Try
            ' 更新権限判定
            Dim canUpd As Boolean
            If kknriId > 0 Then
                canUpd = CanUpdateKknri(kknriId)
            Else
                canUpd = CanUpdate()
            End If

            SetControlEnabled(form, "cmd_新規", canUpd)
            SetControlEnabled(form, "cmd_変更", canUpd)
            SetControlEnabled(form, "cmd_Touroku", canUpd)
            SetControlEnabled(form, "cmd_Del", canUpd)

            ' ファイル出力権限
            SetControlEnabled(form, "cmd_Output", LoginSession.CanFileOutput)

            ' 印刷権限
            SetControlEnabled(form, "cmd_FlexReportDLG", LoginSession.CanPrint)
        Catch
            ' UI制御失敗は無視
        End Try
    End Sub

    ''' <summary>
    ''' 一覧画面のボタン制御 (Access版 gListLimitChk 相当)
    ''' ファイル出力・印刷のみ制御
    ''' </summary>
    Public Sub ApplyListLimit(form As Form)
        Try
            SetControlEnabled(form, "cmd_Output", LoginSession.CanFileOutput)
            SetControlEnabled(form, "cmd_FlexReportDLG", LoginSession.CanPrint)
        Catch
            ' UI制御失敗は無視
        End Try
    End Sub

    ' =========================================================
    '  セキュリティフィルタSQL生成
    ' =========================================================

    ''' <summary>
    ''' AccessKind=3（管理単位限定）時の契約管理単位フィルタWHERE句を生成
    ''' Access版 pc_StartUp.gSET_FlexSecSQL 相当
    ''' </summary>
    ''' <param name="columnName">フィルタ対象のカラム名（例: "kknri_id"）</param>
    ''' <returns>WHERE句の条件文字列。AccessKind≠3の場合は空文字</returns>
    Public Function GetKknriFilterSql(columnName As String) As String
        If LoginSession.AccessKind <> AccessKindEnum.UnitBased Then
            Return ""
        End If

        If LoginSession.KknriList.Count = 0 Then
            ' 管理単位限定だがリストが空 → データなし
            Return $"{columnName} IN (-1)"
        End If

        Dim ids As New List(Of String)()
        For Each entry In LoginSession.KknriList
            ids.Add(entry.KknriId.ToString())
        Next
        Return $"{columnName} IN ({String.Join(",", ids)})"
    End Function

    ''' <summary>
    ''' AccessKindB=3（管理単位限定）時の部門管理単位フィルタWHERE句を生成
    ''' </summary>
    Public Function GetBknriFilterSql(columnName As String) As String
        If LoginSession.AccessKindB <> AccessKindEnum.UnitBased Then
            Return ""
        End If

        If LoginSession.BknriList.Count = 0 Then
            Return $"{columnName} IN (-1)"
        End If

        Dim ids As New List(Of String)()
        For Each entry In LoginSession.BknriList
            ids.Add(entry.BknriId.ToString())
        Next
        Return $"{columnName} IN ({String.Join(",", ids)})"
    End Function

    ' =========================================================
    '  ヘルパー
    ' =========================================================

    ''' <summary>
    ''' フォーム内のコントロールを名前で検索してEnabled設定する
    ''' コントロールが見つからない場合は何もしない (Access版 gIsFoundControl + gControlLockSet 相当)
    ''' </summary>
    Private Sub SetControlEnabled(form As Form, controlName As String, enabled As Boolean)
        Dim controls = form.Controls.Find(controlName, True)
        If controls.Length > 0 Then
            controls(0).Enabled = enabled
        End If
    End Sub

End Module
