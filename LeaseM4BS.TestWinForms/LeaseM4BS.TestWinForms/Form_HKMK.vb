Imports LeaseM4BS.DataAccess

Partial Public Class Form_HKMK
    Inherits Form

    Protected _crud As crudHelper = New crudHelper()
    Protected _formHelper As FormHelper = New FormHelper()

    Protected Sub LoadSumCombos(cmbSum1 As ComboBox, cmbSum2 As ComboBox, cmbSum3 As ComboBox)
        Dim sqlSum1 As String = "SELECT DISTINCT sum1_cd, sum1_nm " &
                                    "FROM m_hkmk " &
                                    "WHERE sum1_cd <> '' " &
                                    "ORDER BY sum1_cd"

        Dim sqlSum2 As String = "SELECT DISTINCT sum2_cd, sum2_nm " &
                                    "FROM m_hkmk " &
                                    "WHERE sum1_cd <> '' " &
                                    "ORDER BY sum2_cd"

        Dim sqlSum3 As String = "SELECT DISTINCT sum3_cd, sum3_nm " &
                                    "FROM m_hkmk " &
                                    "WHERE sum3_cd <> '' " &
                                    "ORDER BY sum3_cd"

        _formHelper.BindCombo(cmbSum1, sqlSum1, "sum1_cd", "sum1_cd")
        _formHelper.BindCombo(cmbSum2, sqlSum2, "sum2_cd", "sum2_cd")
        _formHelper.BindCombo(cmbSum3, sqlSum3, "sum3_cd", "sum3_cd")

        For Each cmb In {cmbSum1, cmbSum2, cmbSum3}
            _formHelper.AdjustComboSize(cmb, 600, 16)
            cmb.SelectedIndex = -1
        Next
    End Sub

    Protected Sub LoadPtnCombos(cmbPtn As ComboBox)
        Dim sqlPtn As String = "SELECT DISTINCT hrel_ptn_cd3, hrel_ptn_nm3 " &
                                    "FROM m_hkmk " &
                                    "WHERE hrel_ptn_cd3 <> '' " &
                                    "ORDER BY hrel_ptn_cd3"

        _formHelper.BindCombo(cmbPtn, sqlPtn, "hrel_ptn_cd3", "hrel_ptn_cd3")
        _formHelper.AdjustComboSize(cmbPtn, 600, 16)
        cmbPtn.SelectedIndex = -1
    End Sub
End Class