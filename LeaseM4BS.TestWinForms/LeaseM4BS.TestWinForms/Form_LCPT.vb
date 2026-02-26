Imports System.Windows
Imports LeaseM4BS.DataAccess
Imports Microsoft.SqlServer.Server

Partial Public Class Form_LCPT
    Inherits Form

    Protected _crud As crudHelper = New crudHelper()
    Protected _formHelper As FormHelper = New FormHelper()

    ' -------------------------------------------------------------------------
    ' マスタデータのロード
    ' -------------------------------------------------------------------------
    Protected Sub LoadLcptCombo(cmbLcpt2 As ComboBox)
        ' 支払先
        Dim sqlLcpt2 As String = "SELECT DISTINCT lcpt2_cd, lcpt2_nm " &
                                    "FROM m_lcpt " &
                                    "WHERE lcpt2_cd <> '' " &
                                    "ORDER BY lcpt2_cd"

        _formHelper.BindCombo(cmbLcpt2, sqlLcpt2, "lcpt2_cd", "lcpt2_cd")
        _formHelper.AdjustComboSize(cmbLcpt2, 600, 16)
        cmbLcpt2.SelectedIndex = -1
    End Sub

    Protected Sub LoadSumCombos(cmbSum1 As ComboBox, cmbSum2 As ComboBox, cmbSum3 As ComboBox)
        ' 集計区分
        Dim sqlSum1 As String = "SELECT DISTINCT sum1_cd, sum1_nm " &
                                    "FROM m_lcpt " &
                                    "WHERE sum1_cd <> '' " &
                                    "ORDER BY sum1_cd"

        Dim sqlSum2 As String = "SELECT DISTINCT sum2_cd, sum2_nm " &
                                    "FROM m_lcpt " &
                                    "WHERE sum2_cd <> '' " &
                                    "ORDER BY sum2_cd"

        Dim sqlSum3 As String = "SELECT DISTINCT sum3_cd, sum3_nm " &
                                    "FROM m_lcpt " &
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
End Class