Imports System.Windows.Forms
Imports LeaseM4BS.DataAccess

' --- スケジュール行編集ダイアログ ---
' Access版 f_HEN_SCH 相当
Partial Public Class Form_f_HEN_SCH
    Inherits Form

    ' 呼び出し元がセットするプロパティ
    Public Property KykmId As Double
    Public Property LineId As Integer
    Public Property ShriDt As String
    Public Property Klsryo As Double
    Public Property Kzei As Double
    Public Property Zritu As Double
    Public Property SshriKn As Integer

    ' 結果プロパティ（DialogResult.OK 時に呼び出し元が参照）
    Public ReadOnly Property ResultShriDt As String
        Get
            Return txt_SHRI_DT.Text.Trim()
        End Get
    End Property

    Public ReadOnly Property ResultKlsryo As Double
        Get
            Dim v As Double
            Double.TryParse(txt_KLSRYO.Text, v)
            Return v
        End Get
    End Property

    Public ReadOnly Property ResultKzei As Double
        Get
            Dim v As Double
            Double.TryParse(txt_KZEI.Text, v)
            Return v
        End Get
    End Property

    Public ReadOnly Property ResultZritu As Double
        Get
            Dim v As Double
            Double.TryParse(txt_ZRITU.Text, v)
            Return v
        End Get
    End Property

    Public ReadOnly Property ResultSshriKn As Integer
        Get
            Dim v As Integer
            Integer.TryParse(txt_SSHRI_KN.Text, v)
            Return v
        End Get
    End Property

    Public Property IsDeleted As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Form_f_HEN_SCH_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_W_KYKM_ID.Text = KykmId.ToString()
        txt_LINE_ID.Text = LineId.ToString()
        txt_SHRI_DT.Text = ShriDt
        txt_KLSRYO.Text = Klsryo.ToString()
        txt_KZEI.Text = Kzei.ToString()
        txt_ZRITU.Text = Zritu.ToString()
        txt_SSHRI_KN.Text = SshriKn.ToString()

        txt_W_KYKM_ID.ReadOnly = True
        txt_LINE_ID.ReadOnly = True
        txt_KLSRYO_ZKOMI.ReadOnly = True
        txt_KLSRYO_SUM.ReadOnly = True
        txt_KZEI_SUM.ReadOnly = True
        txt_KLSRYO_ZKOMI_SUM.ReadOnly = True

        RecalcKzei()
    End Sub

    ' KLSRYO または ZRITU 変更時に消費税自動再計算（FR-004）
    Private Sub txt_KLSRYO_TextChanged(sender As Object, e As EventArgs) Handles txt_KLSRYO.TextChanged
        RecalcKzei()
    End Sub

    Private Sub txt_ZRITU_TextChanged(sender As Object, e As EventArgs) Handles txt_ZRITU.TextChanged
        RecalcKzei()
    End Sub

    Private Sub RecalcKzei()
        Dim klsryoVal As Double
        Dim zrituVal As Double
        If Double.TryParse(txt_KLSRYO.Text, klsryoVal) AndAlso Double.TryParse(txt_ZRITU.Text, zrituVal) Then
            Dim kzei As Long = CLng(Math.Floor(klsryoVal * zrituVal / 100))
            txt_KZEI.Text = kzei.ToString()
            txt_KLSRYO_ZKOMI.Text = (klsryoVal + kzei).ToString()
        End If
    End Sub

    ' [呼出元に反映] ボタン
    Private Sub cmd_呼出元に反映_Click(sender As Object, e As EventArgs) Handles cmd_呼出元に反映.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ' [行削除] ボタン
    Private Sub cmd_削除_Click(sender As Object, e As EventArgs) Handles cmd_削除.Click
        Dim result = MessageBox.Show("この行を削除しますか？", "確認",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            IsDeleted = True
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    ' [キャンセル] ボタン
    Private Sub cmd_閉じる_Click(sender As Object, e As EventArgs) Handles cmd_閉じる.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FormKeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        HandleEnterKeyNavigation(Me, e)
    End Sub
End Class