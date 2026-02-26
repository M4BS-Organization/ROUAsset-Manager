Imports LeaseM4BS.DataAccess

Partial Public Class Form_SKMK
    Inherits Form

    Protected _crud As crudHelper = New crudHelper()
    Protected _formHelper As FormHelper = New FormHelper()

    Protected Sub LoadSumCombos(cmbSums As ComboBox())
        ' 集計区分1から15まで
        For i As Integer = 1 To 15
            Dim sqlSum As String = $"SELECT DISTINCT sum{i}_cd, sum{i}_nm " &
                                        $"FROM m_skmk " &
                                        $"WHERE sum{i}_cd <> '' " &
                                        $"ORDER BY sum{i}_cd"

            _formHelper.BindCombo(cmbSums(i - 1), sqlSum, $"sum{i}_cd", $"sum{i}_cd")

            _formHelper.AdjustComboSize(cmbSums(i - 1), 600, 16)
            cmbSums(i - 1).SelectedIndex = -1
        Next
    End Sub

    Protected Sub LoadPtnCombo(cmbPtn As ComboBox)
        Dim sqlPtn As String = "SELECT hrel_ptn_cd1, hrel_ptn_nm1 " &
                                    "FROM m_skmk " &
                                    "WHERE hrel_ptn_cd1 <> '' " &
                                    "ORDER BY hrel_ptn_cd1"

        _formHelper.BindCombo(cmbPtn, sqlPtn, "hrel_ptn_cd1", "hrel_ptn_nm1")

        _formHelper.AdjustComboSize(cmbPtn, 600, 16)
        cmbPtn.SelectedIndex = -1
    End Sub
End Class