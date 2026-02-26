Imports LeaseM4BS.DataAccess

Partial Public Class Form_KKNRI
    Inherits Form

    Protected _crud As crudHelper = New crudHelper()
    Protected _formHelper As FormHelper = New FormHelper()

    ' -------------------------------------------------------------------------
    ' マスタデータのロード
    ' -------------------------------------------------------------------------
    Protected Sub LoadKknriCombos(cmbKknri2 As ComboBox, cmbKknri3 As ComboBox)
        ' 契約管理単位 (Kknri2, kknri3)
        Dim sqlKknri2 As String = "SELECT DISTINCT kknri2_cd, kknri2_nm " &
                                    "FROM m_kknri " &
                                    "WHERE kknri2_cd <> '' " &
                                    "ORDER BY kknri2_cd"

        Dim sqlKknri3 As String = "SELECT DISTINCT kknri3_cd, kknri3_nm " &
                                    "FROM m_kknri " &
                                    "WHERE kknri3_cd <> '' " &
                                    "ORDER BY kknri3_cd"

        _formHelper.BindCombo(cmbKknri2, sqlKknri2, "kknri2_cd", "kknri2_cd")
        _formHelper.BindCombo(cmbKknri3, sqlKknri3, "kknri3_cd", "kknri3_cd")

        For Each cmb In {cmbKknri2, cmbKknri3}
            _formHelper.AdjustComboSize(cmb, 600, 16)
            cmb.SelectedIndex = -1
        Next
    End Sub

    Protected Sub LoadCorpCombo(cmbCorp As ComboBox)
        ' 会社
        Dim sqlCorp As String = "SELECT corp_id, corp1_cd, corp1_nm " &
                                        "FROM m_corp " &
                                        "WHERE corp_id <> 0 " &
                                        "ORDER BY corp_id"

        _formHelper.BindCombo(cmbCorp, sqlCorp, "corp1_cd", "corp1_cd")
        _formHelper.AdjustComboSize(cmbCorp, 600, 16)
        cmbCorp.SelectedIndex = -1
    End Sub

    Protected Sub LoadPtnCombo(cmbPtn As ComboBox)
        ' 費用決定要素
        Dim sqlPtn As String = "SELECT DISTINCT hrel_ptn_cd4, hrel_ptn_nm4 " &
                                        "FROM m_kknri " &
                                        "WHERE hrel_ptn_cd4 <> '' " &
                                        "ORDER BY hrel_ptn_cd4"

        _formHelper.BindCombo(cmbPtn, sqlPtn, "hrel_ptn_cd4", "hrel_ptn_cd4")
        _formHelper.AdjustComboSize(cmbPtn, 600, 16)
        cmbPtn.SelectedIndex = -1
    End Sub
End Class