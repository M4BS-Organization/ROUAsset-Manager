Imports LeaseM4BS.DataAccess

Partial Public Class Form_BCAT
    Inherits Form

    Protected _crud As crudHelper = New crudHelper()
    Protected _formHelper As FormHelper = New FormHelper()

    ' -------------------------------------------------------------------------
    ' マスタデータのロード
    ' -------------------------------------------------------------------------
    Protected Sub LoadBcatCombos(cmbBcat2 As ComboBox, cmbBcat3 As ComboBox, cmbBcat4 As ComboBox, cmbBcat5 As ComboBox)
        Dim sqlBcat2 As String = "SELECT DISTINCT bcat2_cd, bcat2_nm " &
                                    "FROM m_bcat " &
                                    "WHERE bcat2_cd <> '' " &
                                    "ORDER BY bcat2_cd;"

        Dim sqlBcat3 As String = "SELECT DISTINCT bcat3_cd, bcat3_nm " &
                                    "FROM m_bcat " &
                                    "WHERE bcat3_cd <> '' " &
                                    "ORDER BY bcat3_cd;"

        Dim sqlBcat4 As String = "SELECT DISTINCT bcat4_cd, bcat4_nm " &
                                    "FROM m_bcat " &
                                    "WHERE bcat4_cd <> '' " &
                                    "ORDER BY bcat4_cd;"

        Dim sqlBcat5 As String = "SELECT DISTINCT bcat5_cd, bcat5_nm " &
                                    "FROM m_bcat " &
                                    "WHERE bcat5_cd <> '' " &
                                    "ORDER BY bcat5_cd;"

        _formHelper.BindCombo(cmbBcat2, sqlBcat2, "bcat2_cd", "bcat2_cd")
        _formHelper.BindCombo(cmbBcat3, sqlBcat3, "bcat3_cd", "bcat3_cd")
        _formHelper.BindCombo(cmbBcat4, sqlBcat4, "bcat4_cd", "bcat4_cd")
        _formHelper.BindCombo(cmbBcat5, sqlBcat5, "bcat5_cd", "bcat5_cd")

        For Each cmb In {cmbBcat2, cmbBcat3, cmbBcat4, cmbBcat5}
            _formHelper.AdjustComboSize(cmb, 600, 16)
            cmb.SelectedIndex = -1
        Next

    End Sub

    Protected Sub LoadGenkCombo(cmbGenk As ComboBox)
        Dim sqlGenk = "SELECT DISTINCT genk_cd, genk_nm " &
                        "FROM m_genk " &
                        "WHERE genk_cd <> '' " &
                        "ORDER BY genk_cd;"

        _formHelper.BindCombo(cmbGenk, sqlGenk, "genk_cd", "genk_cd")

        _formHelper.AdjustComboSize(cmbGenk, 600, 16)
        cmbGenk.SelectedIndex = -1
    End Sub

    Protected Sub LoadSumCombos(cmbSum1 As ComboBox, cmbSum2 As ComboBox, cmbSum3 As ComboBox)
        Dim sqlSum1 = "SELECT DISTINCT sum1_cd, sum1_nm " &
                        "FROM m_bcat " &
                        "WHERE sum1_cd <> '' " &
                        "ORDER BY sum1_cd;"

        Dim sqlSum2 = "SELECT DISTINCT sum2_cd, sum2_nm " &
                        "FROM m_bcat " &
                        "WHERE sum1_cd <> '' " &
                        "ORDER BY sum2_cd;"

        Dim sqlSum3 = "SELECT DISTINCT sum3_cd, sum3_nm " &
                        "FROM m_bcat " &
                        "WHERE sum1_cd <> '' " &
                        "ORDER BY sum3_cd;"

        _formHelper.BindCombo(cmbSum1, sqlSum1, "sum1_cd", "sum1_cd")
        _formHelper.BindCombo(cmbSum2, sqlSum2, "sum2_cd", "sum2_cd")
        _formHelper.BindCombo(cmbSum3, sqlSum3, "sum3_cd", "sum3_cd")

        For Each cmb In {cmbSum1, cmbSum2, cmbSum3}
            _formHelper.AdjustComboSize(cmb, 600, 16)
            cmb.SelectedIndex = -1
        Next
    End Sub

    Protected Sub LoadBknriCombo(cmbBknri As ComboBox)
        Dim sqlBknri As String = "SELECT bknri1_cd, bknri1_nm " &
                                    "FROM m_bknri " &
                                    "WHERE bknri1_cd <> '' " &
                                    "ORDER BY bknri1_cd;"

        _formHelper.BindCombo(cmbBknri, sqlBknri, "bknri1_cd", "bknri1_cd")
        _formHelper.AdjustComboSize(cmbBknri, 600, 16)
        cmbBknri.SelectedIndex = -1
    End Sub
End Class