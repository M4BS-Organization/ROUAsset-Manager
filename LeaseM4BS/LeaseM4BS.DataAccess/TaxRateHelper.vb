Imports Npgsql

''' <summary>
''' 消費税率取得ヘルパー
''' Access版 pc_改正消費税.gGetZRITUForKYKH を移植
''' </summary>
Public Class TaxRateHelper
        Implements IDisposable

        Private _crud As New CrudHelper()
        Private _disposed As Boolean = False

        ''' <summary>
        ''' 契約書向け消費税率を取得する
        ''' Access版: pc_改正消費税.gGetZRITUForKYKH (L266)
        ''' </summary>
        ''' <param name="kkbnId">契約区分ID</param>
        ''' <param name="kyakDt">契約日</param>
        ''' <param name="startDt">開始日 (必須)</param>
        ''' <param name="endDt">終了日</param>
        ''' <param name="saikaisu">再リース回数 (0=初契約)</param>
        ''' <returns>税率 (例: 0.10)。取得不能時はNothing</returns>
        Public Function GetZrituForKykh(kkbnId As Integer,
                                        kyakDt As Date?,
                                        startDt As Date?,
                                        endDt As Date?,
                                        saikaisu As Integer) As Double?

            If startDt Is Nothing Then Return Nothing

            ' 消費税改正テーブルから適用税率を取得
            ' 開始日以前で最も新しい適用開始日の税率を返す
            Dim sql As String =
                "SELECT zritu FROM t_zei_kaisei " &
                "WHERE teki_dt_from <= @dt " &
                "ORDER BY teki_dt_from DESC " &
                "LIMIT 1"

            ' 再リースの場合: 終了日基準で税率判定
            ' 初契約の場合: 開始日基準
            Dim refDate As Date
            If saikaisu > 0 AndAlso endDt IsNot Nothing Then
                refDate = endDt.Value
            Else
                refDate = startDt.Value
            End If

            Dim prm As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@dt", refDate)
            }

            Dim dt = _crud.GetDataTable(sql, prm)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Try
                    Return Convert.ToDouble(dt.Rows(0)("zritu"))
                Catch
                    Return Nothing
                End Try
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' 消費税率一覧を取得 (ComboBoxバインド用)
        ''' </summary>
        Public Function GetZrituList() As System.Data.DataTable
            Return _crud.GetDataTable(
                "SELECT zritu_id, zritu, teki_dt_from " &
                "FROM t_zei_kaisei " &
                "ORDER BY teki_dt_from DESC")
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not _disposed Then
                _crud?.Dispose()
                _disposed = True
            End If
        End Sub

End Class
