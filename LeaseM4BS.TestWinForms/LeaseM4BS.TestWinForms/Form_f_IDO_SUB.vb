Imports System.Windows.Forms

' --- 物件移動サブ行 ---
' Access版 f_IDO_SUB 相当
' Form_f_IDO 内で UserControl 的に使用される1行分のデータ表示
Partial Public Class Form_f_IDO_SUB
    Inherits Form

    ' 行データプロパティ
    Public Property KykmId As Double
    Public Property KykmNo As String
    Public Property Saikaisu As Integer
    Public Property BuknBango1 As String
    Public Property BuknNm As String
    Public Property StartDt As String
    Public Property CkaiykDt As String
    Public Property Klsryo As Double

    Public Sub New()
        InitializeComponent()
    End Sub

    ' 行データをコントロールに反映
    Public Sub RenderRow()
        txt_KYKM_NO.Text = KykmNo
        txt_SAIKAISU.Text = Saikaisu.ToString()
        txt_BUKN_BANGO1.Text = BuknBango1
        txt_BUKN_NM.Text = BuknNm
        txt_START_DT.Text = StartDt
        txt_CKAIYK_DT.Text = CkaiykDt
        txt_KLSRYO.Text = Klsryo.ToString()

        ' データ行のテキストは読み取り専用
        txt_KYKM_NO.ReadOnly = True
        txt_SAIKAISU.ReadOnly = True
        txt_BUKN_BANGO1.ReadOnly = True
        txt_BUKN_NM.ReadOnly = True
        txt_START_DT.ReadOnly = True
        txt_CKAIYK_DT.ReadOnly = True
        txt_KLSRYO.ReadOnly = True
    End Sub

    Public ReadOnly Property IsChecked As Boolean
        Get
            Return chk_IDO_F.Checked
        End Get
    End Property
End Class