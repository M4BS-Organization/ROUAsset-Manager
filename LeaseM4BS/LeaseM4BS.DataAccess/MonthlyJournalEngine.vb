' 月次仕訳計上エンジン (Access版 pc_月次仕訳計上.g仕訳計算_仕訳to計上テーブル_計上 相当)
' 月別ループで KeijoCalculationEngine を呼び出し、結果をワークテーブルに書き込む。

Imports System
Imports System.Collections.Generic
Imports System.Data

''' <summary>
''' 月次仕訳計上エンジン (Access版 g仕訳計算_仕訳to計上テーブル_計上)
''' 指定期間を月単位に分割し、各月の計上データを算出してワークテーブルに書き込む。
''' </summary>
Public Class MonthlyJournalEngine

    Private _crud As CrudHelper
    Private _keijoEngine As KeijoCalculationEngine
    Private _workTableManager As KeijoWorkTableManager

    ''' <summary>処理進捗コールバック (現在月, 全体月数, メッセージ)</summary>
    Public Event Progress(currentMonth As Integer, totalMonths As Integer, message As String)

    Public Sub New()
        _crud = New CrudHelper()
        _keijoEngine = New KeijoCalculationEngine()
        _workTableManager = New KeijoWorkTableManager(_crud)
    End Sub

    Public Sub New(crud As CrudHelper)
        _crud = crud
        _keijoEngine = New KeijoCalculationEngine()
        _workTableManager = New KeijoWorkTableManager(_crud)
    End Sub

    ' ======================================================================
    '  メイン処理 (Access版 g仕訳計算_仕訳to計上テーブル_計上)
    ' ======================================================================

    ''' <summary>
    ''' 月次仕訳計上メイン処理
    ''' 期間FROM～TOを月単位で分割し、各月ごとに計上計算→ワークテーブル書込を行う。
    ''' </summary>
    ''' <param name="kikanFrom">期間FROM (月初日)</param>
    ''' <param name="kikanTo">期間TO (月末日)</param>
    ''' <param name="joken">計上条件</param>
    ''' <returns>成功: True / 失敗: False</returns>
    Public Function Execute(
        kikanFrom As Date,
        kikanTo As Date,
        joken As KeijoJoken
    ) As Boolean

        Try
            ' トランザクション開始
            _crud.BeginTransaction()

            ' ワークテーブルクリア
            _workTableManager.ClearAll()

            ' 月別ループ
            Dim totalMonths As Integer = GetMonthCount(kikanFrom, kikanTo)
            Dim currentMonth As Integer = 0

            Dim kishuDt As Date = kikanFrom
            Do While kishuDt <= kikanTo

                currentMonth += 1

                ' 当月の期末日 = 月末日
                Dim kimatDt As Date = CashScheduleBuilder.GetMonthEndDate(kishuDt)
                If kimatDt > kikanTo Then
                    kimatDt = kikanTo
                End If

                ' 進捗通知
                RaiseEvent Progress(
                    currentMonth, totalMonths,
                    $"計上計算中... {kishuDt:yyyy/MM} ({currentMonth}/{totalMonths})"
                )

                ' 1ヶ月分の計上計算
                Dim monthlyRows As List(Of KeijoWorkRow) = _keijoEngine.Execute(joken, kishuDt, kimatDt)

                ' ワークテーブルに書込
                _workTableManager.InsertWorkRows(monthlyRows)

                ' 翌月初日へ
                kishuDt = kimatDt.AddDays(1)
            Loop

            ' コミット
            _crud.Commit()

            RaiseEvent Progress(totalMonths, totalMonths, "計上計算完了")

            Return True

        Catch ex As Exception
            ' ロールバック
            Try
                If _crud.IsInTransaction Then
                    _crud.Rollback()
                End If
            Catch
            End Try

            Throw New Exception($"月次仕訳計上処理でエラーが発生しました: {ex.Message}", ex)
        End Try
    End Function

    ' ======================================================================
    '  結果取得
    ' ======================================================================

    ''' <summary>注記計上ワーク全件取得 (結果表示用)</summary>
    Public Function GetChukiKeijoResult() As DataTable
        Return _workTableManager.GetChukiKeijoAll()
    End Function

    ''' <summary>変額仕訳ワーク全件取得</summary>
    Public Function GetHenlKeijoResult() As DataTable
        Return _workTableManager.GetHenlKeijoAll()
    End Function

    ''' <summary>結果件数取得</summary>
    Public Function GetResultCounts() As (ChukiCount As Integer, HenlCount As Integer)
        Return (_workTableManager.GetChukiKeijoCount(), _workTableManager.GetHenlKeijoCount())
    End Function

    ' ======================================================================
    '  ヘルパー
    ' ======================================================================

    ''' <summary>期間の月数を算出</summary>
    Private Shared Function GetMonthCount(fromDt As Date, toDt As Date) As Integer
        Dim months As Integer = (toDt.Year - fromDt.Year) * 12 + (toDt.Month - fromDt.Month) + 1
        If months < 1 Then months = 1
        Return months
    End Function

End Class
