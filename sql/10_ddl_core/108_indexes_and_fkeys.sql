-- インデックス・外部キー制約: PK, INDEX, FK

BEGIN;

SET search_path TO public;

-- ============================================================
-- PRIMARY KEY 制約
-- ============================================================

--
-- Name: c_chu_hnti c_chu_hnti_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_chu_hnti
    ADD CONSTRAINT c_chu_hnti_pkey PRIMARY KEY (chu_hnti_id);


--
-- Name: c_chuum c_chuum_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_chuum
    ADD CONSTRAINT c_chuum_pkey PRIMARY KEY (chuum_id);


--
-- Name: c_kjkbn c_kjkbn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_kjkbn
    ADD CONSTRAINT c_kjkbn_pkey PRIMARY KEY (kjkbn_id);


--
-- Name: c_kjtaisyo c_kjtaisyo_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_kjtaisyo
    ADD CONSTRAINT c_kjtaisyo_pkey PRIMARY KEY (kjtaisyo_id);


--
-- Name: c_kkbn c_kkbn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_kkbn
    ADD CONSTRAINT c_kkbn_pkey PRIMARY KEY (kkbn_id);


--
-- Name: c_leakbn c_leakbn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_leakbn
    ADD CONSTRAINT c_leakbn_pkey PRIMARY KEY (leakbn_id);


--
-- Name: c_rcalc c_rcalc_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_rcalc
    ADD CONSTRAINT c_rcalc_pkey PRIMARY KEY (rcalc_id);


--
-- Name: c_settei_idfld c_settei_idfld_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_settei_idfld
    ADD CONSTRAINT c_settei_idfld_pkey PRIMARY KEY (settei_id, val_id);


--
-- Name: c_skyak_ho c_skyak_ho_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_skyak_ho
    ADD CONSTRAINT c_skyak_ho_pkey PRIMARY KEY (skyak_ho_id);


--
-- Name: c_szei_kjkbn c_szei_kjkbn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.c_szei_kjkbn
    ADD CONSTRAINT c_szei_kjkbn_pkey PRIMARY KEY (szei_kjkbn_id);


--
-- Name: d_gson d_gson_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_gson
    ADD CONSTRAINT d_gson_pkey PRIMARY KEY (kykm_id, line_id);


--
-- Name: d_haif d_haif_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT d_haif_pkey PRIMARY KEY (kykm_id, line_id);


--
-- Name: d_henf d_henf_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henf
    ADD CONSTRAINT d_henf_pkey PRIMARY KEY (kykm_id, line_id);


--
-- Name: d_henl d_henl_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henl
    ADD CONSTRAINT d_henl_pkey PRIMARY KEY (kykm_id, line_id);


--
-- Name: d_kykh d_kykh_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT d_kykh_pkey PRIMARY KEY (kykh_id);


--
-- Name: d_kykm d_kykm_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT d_kykm_pkey PRIMARY KEY (kykm_id);


--
-- Name: l_bklog l_bklog_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.l_bklog
    ADD CONSTRAINT l_bklog_pkey PRIMARY KEY (op_dt);


--
-- Name: l_slog l_slog_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.l_slog
    ADD CONSTRAINT l_slog_pkey PRIMARY KEY (slog_no);


--
-- Name: l_ulog l_ulog_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.l_ulog
    ADD CONSTRAINT l_ulog_pkey PRIMARY KEY (slog_no, ulog_no);


--
-- Name: m_bcat m_bcat_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bcat
    ADD CONSTRAINT m_bcat_pkey PRIMARY KEY (bcat_id);


--
-- Name: m_bkind m_bkind_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bkind
    ADD CONSTRAINT m_bkind_pkey PRIMARY KEY (bkind_id);


--
-- Name: m_bknri m_bknri_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bknri
    ADD CONSTRAINT m_bknri_pkey PRIMARY KEY (bknri_id);


--
-- Name: m_corp m_corp_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_corp
    ADD CONSTRAINT m_corp_pkey PRIMARY KEY (corp_id);


--
-- Name: m_genk m_genk_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_genk
    ADD CONSTRAINT m_genk_pkey PRIMARY KEY (genk_id);


--
-- Name: m_gsha m_gsha_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_gsha
    ADD CONSTRAINT m_gsha_pkey PRIMARY KEY (gsha_id);


--
-- Name: m_hkho m_hkho_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_hkho
    ADD CONSTRAINT m_hkho_pkey PRIMARY KEY (hkho_id);


--
-- Name: m_hkmk m_hkmk_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_hkmk
    ADD CONSTRAINT m_hkmk_pkey PRIMARY KEY (hkmk_id);


--
-- Name: m_kknri m_kknri_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_kknri
    ADD CONSTRAINT m_kknri_pkey PRIMARY KEY (kknri_id);


--
-- Name: m_koza m_koza_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_koza
    ADD CONSTRAINT m_koza_pkey PRIMARY KEY (koza_id);


--
-- Name: m_lcpt m_lcpt_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_lcpt
    ADD CONSTRAINT m_lcpt_pkey PRIMARY KEY (lcpt_id);


--
-- Name: m_mcpt m_mcpt_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_mcpt
    ADD CONSTRAINT m_mcpt_pkey PRIMARY KEY (mcpt_id);


--
-- Name: m_rsrvb1 m_rsrvb1_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_rsrvb1
    ADD CONSTRAINT m_rsrvb1_pkey PRIMARY KEY (rsrvb1_id);


--
-- Name: m_rsrvh1 m_rsrvh1_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_rsrvh1
    ADD CONSTRAINT m_rsrvh1_pkey PRIMARY KEY (rsrvh1_id);


--
-- Name: m_rsrvk1 m_rsrvk1_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_rsrvk1
    ADD CONSTRAINT m_rsrvk1_pkey PRIMARY KEY (rsrvk1_id);


--
-- Name: m_shho m_shho_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_shho
    ADD CONSTRAINT m_shho_pkey PRIMARY KEY (shho_id);


--
-- Name: m_skmk m_skmk_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_skmk
    ADD CONSTRAINT m_skmk_pkey PRIMARY KEY (skmk_id);


--
-- Name: m_skti m_skti_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_skti
    ADD CONSTRAINT m_skti_pkey PRIMARY KEY (skti_id);


--
-- Name: m_swptn m_swptn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_swptn
    ADD CONSTRAINT m_swptn_pkey PRIMARY KEY (swptn_id);


--
-- Name: sec_kngn_bknri sec_kngn_bknri_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_bknri
    ADD CONSTRAINT sec_kngn_bknri_pkey PRIMARY KEY (kngn_id, bknri_id);


--
-- Name: sec_kngn_kknri sec_kngn_kknri_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_kknri
    ADD CONSTRAINT sec_kngn_kknri_pkey PRIMARY KEY (kngn_id, kknri_id);


--
-- Name: sec_kngn sec_kngn_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn
    ADD CONSTRAINT sec_kngn_pkey PRIMARY KEY (kngn_id);


--
-- Name: sec_user sec_user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_user
    ADD CONSTRAINT sec_user_pkey PRIMARY KEY (user_id);


--
-- Name: t_accounting_unit t_accounting_unit_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_accounting_unit
    ADD CONSTRAINT t_accounting_unit_pkey PRIMARY KEY (act_unit_id);


--
-- Name: t_amortization_schedule t_amortization_schedule_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_amortization_schedule
    ADD CONSTRAINT t_amortization_schedule_pkey PRIMARY KEY (schedule_id);


--
-- Name: t_audit_log t_audit_log_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_audit_log
    ADD CONSTRAINT t_audit_log_pkey PRIMARY KEY (log_id);


--
-- Name: t_db_version t_db_version_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_db_version
    ADD CONSTRAINT t_db_version_pkey PRIMARY KEY (db_version);


--
-- Name: t_holiday t_holiday_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_holiday
    ADD CONSTRAINT t_holiday_pkey PRIMARY KEY (id);


--
-- Name: t_journal_setting t_journal_setting_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_journal_setting
    ADD CONSTRAINT t_journal_setting_pkey PRIMARY KEY (setting_key);


--
-- Name: t_kari_ritu t_kari_ritu_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_kari_ritu
    ADD CONSTRAINT t_kari_ritu_pkey PRIMARY KEY (kari_ritu_id);


--
-- Name: t_kykbnj_seq t_kykbnj_seq_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_kykbnj_seq
    ADD CONSTRAINT t_kykbnj_seq_pkey PRIMARY KEY (key);


--
-- Name: t_mstk t_mstk_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_mstk
    ADD CONSTRAINT t_mstk_pkey PRIMARY KEY (mstk_id);


--
-- Name: t_opt t_opt_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_opt
    ADD CONSTRAINT t_opt_pkey PRIMARY KEY (opt_id);


--
-- Name: t_req t_req_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_req
    ADD CONSTRAINT t_req_pkey PRIMARY KEY (req_id);


--
-- Name: t_seq t_seq_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_seq
    ADD CONSTRAINT t_seq_pkey PRIMARY KEY (field_nm);


--
-- Name: t_settei t_settei_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_settei
    ADD CONSTRAINT t_settei_pkey PRIMARY KEY (settei_id);


--
-- Name: t_shwak_d t_shwak_d_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_shwak_d
    ADD CONSTRAINT t_shwak_d_pkey PRIMARY KEY (shwak_id);


--
-- Name: t_swk_nm t_swk_nm_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_swk_nm
    ADD CONSTRAINT t_swk_nm_pkey PRIMARY KEY (swk_kbn);


--
-- Name: t_system t_system_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_system
    ADD CONSTRAINT t_system_pkey PRIMARY KEY (ap_version);


--
-- Name: t_szei_kmk t_szei_kmk_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_szei_kmk
    ADD CONSTRAINT t_szei_kmk_pkey PRIMARY KEY (zritu, kind, hreikbn, leakbn_id, kjkbn_id, szei_kjkbn_id);


--
-- Name: t_zei_kaisei t_zei_kaisei_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_zei_kaisei
    ADD CONSTRAINT t_zei_kaisei_pkey PRIMARY KEY (zei_kaisei_id);


--
-- Name: tc_hrel tc_hrel_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_hrel
    ADD CONSTRAINT tc_hrel_pkey PRIMARY KEY (ptn_cd1, ptn_cd2);


--
-- Name: tc_rec_shri tc_rec_shri_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_rec_shri
    ADD CONSTRAINT tc_rec_shri_pkey PRIMARY KEY (rec_shri_id);


--
-- Name: tc_reg_report tc_reg_report_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_reg_report
    ADD CONSTRAINT tc_reg_report_pkey PRIMARY KEY (line_id);


--
-- Name: tc_swk_def_com tc_swk_def_com_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_swk_def_com
    ADD CONSTRAINT tc_swk_def_com_pkey PRIMARY KEY (swk_kbn);


--
-- Name: tc_swk_settei tc_swk_settei_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_swk_settei
    ADD CONSTRAINT tc_swk_settei_pkey PRIMARY KEY (swk_settei_id);


--
-- Name: tw_m_user tw_m_user_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tw_m_user
    ADD CONSTRAINT tw_m_user_pkey PRIMARY KEY (user_id);


-- ============================================================
-- INDEX
-- ============================================================

--
-- Name: idx_c_chu_hnti_chu_hnti_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_chu_hnti_chu_hnti_nm ON public.c_chu_hnti USING btree (chu_hnti_nm);


--
-- Name: idx_c_chuum_chuum_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_chuum_chuum_nm ON public.c_chuum USING btree (chuum_nm);


--
-- Name: idx_c_kkbn_kkbn_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_kkbn_kkbn_nm ON public.c_kkbn USING btree (kkbn_nm);


--
-- Name: idx_c_leakbn_leakbn_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_leakbn_leakbn_nm ON public.c_leakbn USING btree (leakbn_nm);


--
-- Name: idx_c_rcalc_rcalc_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_rcalc_rcalc_nm ON public.c_rcalc USING btree (rcalc_nm);


--
-- Name: idx_c_settei_idfld_settei_id_val_id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_settei_idfld_settei_id_val_id ON public.c_settei_idfld USING btree (settei_id, val_id);


--
-- Name: idx_c_skyak_ho_skyak_ho_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_c_skyak_ho_skyak_ho_nm ON public.c_skyak_ho USING btree (skyak_ho_nm);


--
-- Name: idx_c_szei_kjkbn_d_order; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_c_szei_kjkbn_d_order ON public.c_szei_kjkbn USING btree (d_order);


--
-- Name: idx_d_gson_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_gson_create_id ON public.d_gson USING btree (create_id);


--
-- Name: idx_d_gson_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_gson_kykh_id ON public.d_gson USING btree (kykh_id);


--
-- Name: idx_d_gson_kykh_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_gson_kykh_no ON public.d_gson USING btree (kykh_no);


--
-- Name: idx_d_gson_kykm_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_gson_kykm_no ON public.d_gson USING btree (kykm_no);


--
-- Name: idx_d_gson_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_gson_update_id ON public.d_gson USING btree (update_id);


--
-- Name: idx_d_haif_h_bcat_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_h_bcat_id ON public.d_haif USING btree (h_bcat_id);


--
-- Name: idx_d_haif_h_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_h_create_id ON public.d_haif USING btree (h_create_id);


--
-- Name: idx_d_haif_h_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_h_update_id ON public.d_haif USING btree (h_update_id);


--
-- Name: idx_d_haif_hkmk_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_hkmk_id ON public.d_haif USING btree (hkmk_id);


--
-- Name: idx_d_haif_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_kykh_id ON public.d_haif USING btree (kykh_id);


--
-- Name: idx_d_haif_kykh_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_kykh_no ON public.d_haif USING btree (kykh_no);


--
-- Name: idx_d_haif_kykm_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_kykm_no ON public.d_haif USING btree (kykm_no);


--
-- Name: idx_d_haif_rsrvh1_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_rsrvh1_id ON public.d_haif USING btree (rsrvh1_id);


--
-- Name: idx_d_haif_saikaisu; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_haif_saikaisu ON public.d_haif USING btree (saikaisu);


--
-- Name: idx_d_henf_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_create_id ON public.d_henf USING btree (create_id);


--
-- Name: idx_d_henf_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_kykh_id ON public.d_henf USING btree (kykh_id);


--
-- Name: idx_d_henf_kykh_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_kykh_no ON public.d_henf USING btree (kykh_no);


--
-- Name: idx_d_henf_kykm_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_kykm_no ON public.d_henf USING btree (kykm_no);


--
-- Name: idx_d_henf_shho_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_shho_id ON public.d_henf USING btree (shho_id);


--
-- Name: idx_d_henf_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henf_update_id ON public.d_henf USING btree (update_id);


--
-- Name: idx_d_henl_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_create_id ON public.d_henl USING btree (create_id);


--
-- Name: idx_d_henl_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_kykh_id ON public.d_henl USING btree (kykh_id);


--
-- Name: idx_d_henl_kykh_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_kykh_no ON public.d_henl USING btree (kykh_no);


--
-- Name: idx_d_henl_kykm_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_kykm_no ON public.d_henl USING btree (kykm_no);


--
-- Name: idx_d_henl_shho_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_shho_id ON public.d_henl USING btree (shho_id);


--
-- Name: idx_d_henl_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_henl_update_id ON public.d_henl USING btree (update_id);


--
-- Name: idx_d_kykh_k_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_k_create_id ON public.d_kykh USING btree (k_create_id);


--
-- Name: idx_d_kykh_k_history_f; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_k_history_f ON public.d_kykh USING btree (k_history_f);


--
-- Name: idx_d_kykh_k_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_k_update_id ON public.d_kykh USING btree (k_update_id);


--
-- Name: idx_d_kykh_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_kjkbn_id ON public.d_kykh USING btree (kjkbn_id);


--
-- Name: idx_d_kykh_kkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_kkbn_id ON public.d_kykh USING btree (kkbn_id);


--
-- Name: idx_d_kykh_kknri_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_kknri_id ON public.d_kykh USING btree (kknri_id);


--
-- Name: idx_d_kykh_koza_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_koza_id ON public.d_kykh USING btree (koza_id);


--
-- Name: idx_d_kykh_kyak_end_f; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_kyak_end_f ON public.d_kykh USING btree (kyak_end_f);


--
-- Name: idx_d_kykh_kykh_no_saikaisu; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_d_kykh_kykh_no_saikaisu ON public.d_kykh USING btree (kykh_no, saikaisu);


--
-- Name: idx_d_kykh_lcpt_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_lcpt_id ON public.d_kykh USING btree (lcpt_id);


--
-- Name: idx_d_kykh_rsrvk1_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_rsrvk1_id ON public.d_kykh USING btree (rsrvk1_id);


--
-- Name: idx_d_kykh_shho_1_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_shho_1_id ON public.d_kykh USING btree (shho_1_id);


--
-- Name: idx_d_kykh_shho_2_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_shho_2_id ON public.d_kykh USING btree (shho_2_id);


--
-- Name: idx_d_kykh_shho_3_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_shho_3_id ON public.d_kykh USING btree (shho_3_id);


--
-- Name: idx_d_kykh_shho_m_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykh_shho_m_id ON public.d_kykh USING btree (shho_m_id);


--
-- Name: idx_d_kykm_b_bcat_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_b_bcat_id ON public.d_kykm USING btree (b_bcat_id);


--
-- Name: idx_d_kykm_b_bcat_id_r2; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_b_bcat_id_r2 ON public.d_kykm USING btree (b_bcat_id_r2);


--
-- Name: idx_d_kykm_b_ckaiyk_f; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_b_ckaiyk_f ON public.d_kykm USING btree (b_ckaiyk_f);


--
-- Name: idx_d_kykm_b_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_b_create_id ON public.d_kykm USING btree (b_create_id);


--
-- Name: idx_d_kykm_b_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_b_update_id ON public.d_kykm USING btree (b_update_id);


--
-- Name: idx_d_kykm_bkind_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_bkind_id ON public.d_kykm USING btree (bkind_id);


--
-- Name: idx_d_kykm_chu_hnti_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_chu_hnti_id ON public.d_kykm USING btree (chu_hnti_id);


--
-- Name: idx_d_kykm_chuum_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_chuum_id ON public.d_kykm USING btree (chuum_id);


--
-- Name: idx_d_kykm_f_gsha_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_f_gsha_id ON public.d_kykm USING btree (f_gsha_id);


--
-- Name: idx_d_kykm_f_hkmk_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_f_hkmk_id ON public.d_kykm USING btree (f_hkmk_id);


--
-- Name: idx_d_kykm_f_lcpt_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_f_lcpt_id ON public.d_kykm USING btree (f_lcpt_id);


--
-- Name: idx_d_kykm_hk_gsha_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_hk_gsha_id ON public.d_kykm USING btree (hk_gsha_id);


--
-- Name: idx_d_kykm_hkho_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_hkho_id ON public.d_kykm USING btree (hkho_id);


--
-- Name: idx_d_kykm_hszei_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_hszei_kjkbn_id ON public.d_kykm USING btree (hszei_kjkbn_id);


--
-- Name: idx_d_kykm_ido_dt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_ido_dt ON public.d_kykm USING btree (ido_dt);


--
-- Name: idx_d_kykm_ido_dt_r1; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_ido_dt_r1 ON public.d_kykm USING btree (ido_dt_r1);


--
-- Name: idx_d_kykm_ido_dt_r2; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_ido_dt_r2 ON public.d_kykm USING btree (ido_dt_r2);


--
-- Name: idx_d_kykm_ido_dt_r3; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_ido_dt_r3 ON public.d_kykm USING btree (ido_dt_r3);


--
-- Name: idx_d_kykm_k_gsha_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_k_gsha_id ON public.d_kykm USING btree (k_gsha_id);


--
-- Name: idx_d_kykm_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_kjkbn_id ON public.d_kykm USING btree (kjkbn_id);


--
-- Name: idx_d_kykm_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_kykh_id ON public.d_kykm USING btree (kykh_id);


--
-- Name: idx_d_kykm_kykh_no; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_kykh_no ON public.d_kykm USING btree (kykh_no);


--
-- Name: idx_d_kykm_kykm_no_saikaisu; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_d_kykm_kykm_no_saikaisu ON public.d_kykm USING btree (kykm_no, saikaisu);


--
-- Name: idx_d_kykm_leakbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_leakbn_id ON public.d_kykm USING btree (leakbn_id);


--
-- Name: idx_d_kykm_mcpt_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_mcpt_id ON public.d_kykm USING btree (mcpt_id);


--
-- Name: idx_d_kykm_rsrvb1_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_rsrvb1_id ON public.d_kykm USING btree (rsrvb1_id);


--
-- Name: idx_d_kykm_saikaisu; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_saikaisu ON public.d_kykm USING btree (saikaisu);


--
-- Name: idx_d_kykm_skmk_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_skmk_id ON public.d_kykm USING btree (skmk_id);


--
-- Name: idx_d_kykm_skyak_ho_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_skyak_ho_id ON public.d_kykm USING btree (skyak_ho_id);


--
-- Name: idx_d_kykm_szei_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_d_kykm_szei_kjkbn_id ON public.d_kykm USING btree (szei_kjkbn_id);


--
-- Name: idx_l_bklog_op_dt; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_l_bklog_op_dt ON public.l_bklog USING btree (op_dt);


--
-- Name: idx_l_ulog_slog_no_ulog_no; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_l_ulog_slog_no_ulog_no ON public.l_ulog USING btree (slog_no, ulog_no);


--
-- Name: idx_m_bcat_bknri_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bcat_bknri_id ON public.m_bcat USING btree (bknri_id);


--
-- Name: idx_m_bcat_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bcat_create_id ON public.m_bcat USING btree (create_id);


--
-- Name: idx_m_bcat_genk_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bcat_genk_id ON public.m_bcat USING btree (genk_id);


--
-- Name: idx_m_bcat_skti_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bcat_skti_id ON public.m_bcat USING btree (skti_id);


--
-- Name: idx_m_bcat_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bcat_update_id ON public.m_bcat USING btree (update_id);


--
-- Name: idx_m_bkind_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bkind_create_id ON public.m_bkind USING btree (create_id);


--
-- Name: idx_m_bkind_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bkind_update_id ON public.m_bkind USING btree (update_id);


--
-- Name: idx_m_bknri_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bknri_create_id ON public.m_bknri USING btree (create_id);


--
-- Name: idx_m_bknri_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_bknri_update_id ON public.m_bknri USING btree (update_id);


--
-- Name: idx_m_corp_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_corp_create_id ON public.m_corp USING btree (create_id);


--
-- Name: idx_m_corp_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_corp_update_id ON public.m_corp USING btree (update_id);


--
-- Name: idx_m_genk_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_genk_create_id ON public.m_genk USING btree (create_id);


--
-- Name: idx_m_genk_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_genk_update_id ON public.m_genk USING btree (update_id);


--
-- Name: idx_m_gsha_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_gsha_create_id ON public.m_gsha USING btree (create_id);


--
-- Name: idx_m_gsha_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_gsha_update_id ON public.m_gsha USING btree (update_id);


--
-- Name: idx_m_hkho_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_hkho_create_id ON public.m_hkho USING btree (create_id);


--
-- Name: idx_m_hkho_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_hkho_update_id ON public.m_hkho USING btree (update_id);


--
-- Name: idx_m_hkmk_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_hkmk_create_id ON public.m_hkmk USING btree (create_id);


--
-- Name: idx_m_hkmk_knjkb_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_hkmk_knjkb_id ON public.m_hkmk USING btree (knjkb_id);


--
-- Name: idx_m_hkmk_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_hkmk_update_id ON public.m_hkmk USING btree (update_id);


--
-- Name: idx_m_kknri_corp_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_kknri_corp_id ON public.m_kknri USING btree (corp_id);


--
-- Name: idx_m_kknri_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_kknri_create_id ON public.m_kknri USING btree (create_id);


--
-- Name: idx_m_kknri_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_kknri_update_id ON public.m_kknri USING btree (update_id);


--
-- Name: idx_m_koza_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_koza_create_id ON public.m_koza USING btree (create_id);


--
-- Name: idx_m_koza_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_koza_update_id ON public.m_koza USING btree (update_id);


--
-- Name: idx_m_lcpt_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_create_id ON public.m_lcpt USING btree (create_id);


--
-- Name: idx_m_lcpt_shho_id_n_1; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_n_1 ON public.m_lcpt USING btree (shho_id_n_1);


--
-- Name: idx_m_lcpt_shho_id_n_2; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_n_2 ON public.m_lcpt USING btree (shho_id_n_2);


--
-- Name: idx_m_lcpt_shho_id_n_3; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_n_3 ON public.m_lcpt USING btree (shho_id_n_3);


--
-- Name: idx_m_lcpt_shho_id_s_1; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_s_1 ON public.m_lcpt USING btree (shho_id_s_1);


--
-- Name: idx_m_lcpt_shho_id_s_2; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_s_2 ON public.m_lcpt USING btree (shho_id_s_2);


--
-- Name: idx_m_lcpt_shho_id_s_3; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_shho_id_s_3 ON public.m_lcpt USING btree (shho_id_s_3);


--
-- Name: idx_m_lcpt_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_lcpt_update_id ON public.m_lcpt USING btree (update_id);


--
-- Name: idx_m_mcpt_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_mcpt_create_id ON public.m_mcpt USING btree (create_id);


--
-- Name: idx_m_mcpt_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_mcpt_update_id ON public.m_mcpt USING btree (update_id);


--
-- Name: idx_m_rsrvb1_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_rsrvb1_create_id ON public.m_rsrvb1 USING btree (create_id);


--
-- Name: idx_m_rsrvb1_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_rsrvb1_update_id ON public.m_rsrvb1 USING btree (update_id);


--
-- Name: idx_m_rsrvh1_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_rsrvh1_create_id ON public.m_rsrvh1 USING btree (create_id);


--
-- Name: idx_m_rsrvh1_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_rsrvh1_update_id ON public.m_rsrvh1 USING btree (update_id);


--
-- Name: idx_m_shho_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_shho_create_id ON public.m_shho USING btree (create_id);


--
-- Name: idx_m_shho_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_shho_update_id ON public.m_shho USING btree (update_id);


--
-- Name: idx_m_skmk_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_skmk_create_id ON public.m_skmk USING btree (create_id);


--
-- Name: idx_m_skmk_knjkb_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_skmk_knjkb_id ON public.m_skmk USING btree (knjkb_id);


--
-- Name: idx_m_skmk_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_skmk_update_id ON public.m_skmk USING btree (update_id);


--
-- Name: idx_m_skti_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_skti_create_id ON public.m_skti USING btree (create_id);


--
-- Name: idx_m_skti_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_skti_update_id ON public.m_skti USING btree (update_id);


--
-- Name: idx_m_swptn_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_swptn_create_id ON public.m_swptn USING btree (create_id);


--
-- Name: idx_m_swptn_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_m_swptn_update_id ON public.m_swptn USING btree (update_id);


--
-- Name: idx_sec_kngn_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_kngn_create_id ON public.sec_kngn USING btree (create_id);


--
-- Name: idx_sec_kngn_kknri_kknri_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_kngn_kknri_kknri_id ON public.sec_kngn_kknri USING btree (kknri_id);


--
-- Name: idx_sec_kngn_kknri_kngn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_kngn_kknri_kngn_id ON public.sec_kngn_kknri USING btree (kngn_id);


--
-- Name: idx_sec_kngn_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_kngn_update_id ON public.sec_kngn USING btree (update_id);


--
-- Name: idx_sec_user_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_user_create_id ON public.sec_user USING btree (create_id);


--
-- Name: idx_sec_user_kngn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_user_kngn_id ON public.sec_user USING btree (kngn_id);


--
-- Name: idx_sec_user_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_sec_user_update_id ON public.sec_user USING btree (update_id);


--
-- Name: idx_shwak_d_entry_type; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_shwak_d_entry_type ON public.t_shwak_d USING btree (entry_type);


--
-- Name: idx_shwak_d_kykh_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_shwak_d_kykh_id ON public.t_shwak_d USING btree (kykh_id);


--
-- Name: idx_shwak_d_process_date; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_shwak_d_process_date ON public.t_shwak_d USING btree (process_date);


--
-- Name: idx_t_db_version_db_version; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_db_version_db_version ON public.t_db_version USING btree (db_version);


--
-- Name: idx_t_holiday_id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_holiday_id ON public.t_holiday USING btree (id);


--
-- Name: idx_t_kari_ritu_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_kari_ritu_create_id ON public.t_kari_ritu USING btree (create_id);


--
-- Name: idx_t_kari_ritu_kari_ritu_id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_kari_ritu_kari_ritu_id ON public.t_kari_ritu USING btree (kari_ritu_id);


--
-- Name: idx_t_kari_ritu_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_kari_ritu_update_id ON public.t_kari_ritu USING btree (update_id);


--
-- Name: idx_t_kykbnj_seq_key; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_kykbnj_seq_key ON public.t_kykbnj_seq USING btree (key);


--
-- Name: idx_t_mstk_mstk_id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_mstk_mstk_id ON public.t_mstk USING btree (mstk_id);


--
-- Name: idx_t_opt_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_opt_create_id ON public.t_opt USING btree (create_id);


--
-- Name: idx_t_opt_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_opt_update_id ON public.t_opt USING btree (update_id);


--
-- Name: idx_t_seq_field_nm; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_seq_field_nm ON public.t_seq USING btree (field_nm);


--
-- Name: idx_t_swk_nm_swk_kbn; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_swk_nm_swk_kbn ON public.t_swk_nm USING btree (swk_kbn);


--
-- Name: idx_t_system_ap_version; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_system_ap_version ON public.t_system USING btree (ap_version);


--
-- Name: idx_t_szei_kmk_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_szei_kmk_kjkbn_id ON public.t_szei_kmk USING btree (kjkbn_id);


--
-- Name: idx_t_szei_kmk_leakbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_szei_kmk_leakbn_id ON public.t_szei_kmk USING btree (leakbn_id);


--
-- Name: idx_t_szei_kmk_szei_kjkbn_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_szei_kmk_szei_kjkbn_id ON public.t_szei_kmk USING btree (szei_kjkbn_id);


--
-- Name: idx_t_zei_kaisei_create_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_zei_kaisei_create_id ON public.t_zei_kaisei USING btree (create_id);


--
-- Name: idx_t_zei_kaisei_update_id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_t_zei_kaisei_update_id ON public.t_zei_kaisei USING btree (update_id);


--
-- Name: idx_t_zei_kaisei_zei_kaisei_id; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX idx_t_zei_kaisei_zei_kaisei_id ON public.t_zei_kaisei USING btree (zei_kaisei_id);


--
-- Name: idx_tc_hrel_ptn_cd1_ptn_cd2_ptn_cd3_ptn_cd4; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_hrel_ptn_cd1_ptn_cd2_ptn_cd3_ptn_cd4 ON public.tc_hrel USING btree (ptn_cd1, ptn_cd2, ptn_cd3, ptn_cd4);


--
-- Name: idx_tc_rec_shri_xxxx1id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_rec_shri_xxxx1id ON public.tc_rec_shri USING btree (xxxx1id);


--
-- Name: idx_tc_rec_shri_xxxx2id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_rec_shri_xxxx2id ON public.tc_rec_shri USING btree (xxxx2id);


--
-- Name: idx_tc_rec_shri_xxxx3id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_rec_shri_xxxx3id ON public.tc_rec_shri USING btree (xxxx3id);


--
-- Name: idx_tc_rec_shri_xxxx4id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_rec_shri_xxxx4id ON public.tc_rec_shri USING btree (xxxx4id);


--
-- Name: idx_tc_rec_shri_xxxx5id; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX idx_tc_rec_shri_xxxx5id ON public.tc_rec_shri USING btree (xxxx5id);


--
-- Name: tc_reg_report_line_id_idx; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX tc_reg_report_line_id_idx ON public.tc_reg_report USING btree (line_id);


-- ============================================================
-- FOREIGN KEY 制約
-- ============================================================

--
-- Name: d_haif fk_d_haif_h_bcat; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT fk_d_haif_h_bcat FOREIGN KEY (h_bcat_id) REFERENCES public.m_bcat(bcat_id);


--
-- Name: d_haif fk_d_haif_hkmk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT fk_d_haif_hkmk FOREIGN KEY (hkmk_id) REFERENCES public.m_hkmk(hkmk_id);


--
-- Name: d_haif fk_d_haif_rsrvh1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT fk_d_haif_rsrvh1 FOREIGN KEY (rsrvh1_id) REFERENCES public.m_rsrvh1(rsrvh1_id);


--
-- Name: d_henf fk_d_henf_shho; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henf
    ADD CONSTRAINT fk_d_henf_shho FOREIGN KEY (shho_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_henl fk_d_henl_shho; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henl
    ADD CONSTRAINT fk_d_henl_shho FOREIGN KEY (shho_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_kykh fk_d_kykh_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_kjkbn FOREIGN KEY (kjkbn_id) REFERENCES public.c_kjkbn(kjkbn_id);


--
-- Name: d_kykh fk_d_kykh_kkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_kkbn FOREIGN KEY (kkbn_id) REFERENCES public.c_kkbn(kkbn_id);


--
-- Name: d_kykh fk_d_kykh_kknri; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_kknri FOREIGN KEY (kknri_id) REFERENCES public.m_kknri(kknri_id);


--
-- Name: d_kykh fk_d_kykh_koza; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_koza FOREIGN KEY (koza_id) REFERENCES public.m_koza(koza_id);


--
-- Name: d_kykh fk_d_kykh_lcpt; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_lcpt FOREIGN KEY (lcpt_id) REFERENCES public.m_lcpt(lcpt_id);


--
-- Name: d_kykh fk_d_kykh_rsrvk1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_rsrvk1 FOREIGN KEY (rsrvk1_id) REFERENCES public.m_rsrvk1(rsrvk1_id);


--
-- Name: d_kykh fk_d_kykh_shho_1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_shho_1 FOREIGN KEY (shho_1_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_kykh fk_d_kykh_shho_2; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_shho_2 FOREIGN KEY (shho_2_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_kykh fk_d_kykh_shho_3; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_shho_3 FOREIGN KEY (shho_3_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_kykh fk_d_kykh_shho_m; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykh
    ADD CONSTRAINT fk_d_kykh_shho_m FOREIGN KEY (shho_m_id) REFERENCES public.m_shho(shho_id);


--
-- Name: d_kykm fk_d_kykm_b_bcat; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_b_bcat FOREIGN KEY (b_bcat_id) REFERENCES public.m_bcat(bcat_id);


--
-- Name: d_kykm fk_d_kykm_bkind; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_bkind FOREIGN KEY (bkind_id) REFERENCES public.m_bkind(bkind_id);


--
-- Name: d_kykm fk_d_kykm_chu_hnti; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_chu_hnti FOREIGN KEY (chu_hnti_id) REFERENCES public.c_chu_hnti(chu_hnti_id);


--
-- Name: d_kykm fk_d_kykm_chuum; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_chuum FOREIGN KEY (chuum_id) REFERENCES public.c_chuum(chuum_id);


--
-- Name: d_kykm fk_d_kykm_f_gsha; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_f_gsha FOREIGN KEY (f_gsha_id) REFERENCES public.m_gsha(gsha_id);


--
-- Name: d_kykm fk_d_kykm_f_hkmk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_f_hkmk FOREIGN KEY (f_hkmk_id) REFERENCES public.m_hkmk(hkmk_id);


--
-- Name: d_kykm fk_d_kykm_f_lcpt; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_f_lcpt FOREIGN KEY (f_lcpt_id) REFERENCES public.m_lcpt(lcpt_id);


--
-- Name: d_kykm fk_d_kykm_hk_gsha; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_hk_gsha FOREIGN KEY (hk_gsha_id) REFERENCES public.m_gsha(gsha_id);


--
-- Name: d_kykm fk_d_kykm_hkho; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_hkho FOREIGN KEY (hkho_id) REFERENCES public.m_hkho(hkho_id);


--
-- Name: d_kykm fk_d_kykm_hszei_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_hszei_kjkbn FOREIGN KEY (hszei_kjkbn_id) REFERENCES public.c_szei_kjkbn(szei_kjkbn_id);


--
-- Name: d_kykm fk_d_kykm_k_gsha; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_k_gsha FOREIGN KEY (k_gsha_id) REFERENCES public.m_gsha(gsha_id);


--
-- Name: d_kykm fk_d_kykm_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_kjkbn FOREIGN KEY (kjkbn_id) REFERENCES public.c_kjkbn(kjkbn_id);


--
-- Name: d_kykm fk_d_kykm_leakbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_leakbn FOREIGN KEY (leakbn_id) REFERENCES public.c_leakbn(leakbn_id);


--
-- Name: d_kykm fk_d_kykm_mcpt; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_mcpt FOREIGN KEY (mcpt_id) REFERENCES public.m_mcpt(mcpt_id);


--
-- Name: d_kykm fk_d_kykm_rsrvb1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_rsrvb1 FOREIGN KEY (rsrvb1_id) REFERENCES public.m_rsrvb1(rsrvb1_id);


--
-- Name: d_kykm fk_d_kykm_skmk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_skmk FOREIGN KEY (skmk_id) REFERENCES public.m_skmk(skmk_id);


--
-- Name: d_kykm fk_d_kykm_skyak_ho; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_skyak_ho FOREIGN KEY (skyak_ho_id) REFERENCES public.c_skyak_ho(skyak_ho_id);


--
-- Name: d_kykm fk_d_kykm_szei_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_d_kykm_szei_kjkbn FOREIGN KEY (szei_kjkbn_id) REFERENCES public.c_szei_kjkbn(szei_kjkbn_id);


--
-- Name: d_gson fk_gson_kykh; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_gson
    ADD CONSTRAINT fk_gson_kykh FOREIGN KEY (kykh_id) REFERENCES public.d_kykh(kykh_id);


--
-- Name: d_gson fk_gson_kykm; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_gson
    ADD CONSTRAINT fk_gson_kykm FOREIGN KEY (kykm_id) REFERENCES public.d_kykm(kykm_id);


--
-- Name: d_haif fk_haif_kykh; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT fk_haif_kykh FOREIGN KEY (kykh_id) REFERENCES public.d_kykh(kykh_id);


--
-- Name: d_haif fk_haif_kykm; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_haif
    ADD CONSTRAINT fk_haif_kykm FOREIGN KEY (kykm_id) REFERENCES public.d_kykm(kykm_id);


--
-- Name: d_henf fk_henf_kykh; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henf
    ADD CONSTRAINT fk_henf_kykh FOREIGN KEY (kykh_id) REFERENCES public.d_kykh(kykh_id);


--
-- Name: d_henf fk_henf_kykm; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henf
    ADD CONSTRAINT fk_henf_kykm FOREIGN KEY (kykm_id) REFERENCES public.d_kykm(kykm_id);


--
-- Name: d_henl fk_henl_kykh; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henl
    ADD CONSTRAINT fk_henl_kykh FOREIGN KEY (kykh_id) REFERENCES public.d_kykh(kykh_id);


--
-- Name: d_henl fk_henl_kykm; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_henl
    ADD CONSTRAINT fk_henl_kykm FOREIGN KEY (kykm_id) REFERENCES public.d_kykm(kykm_id);


--
-- Name: d_kykm fk_kykm_kykh; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.d_kykm
    ADD CONSTRAINT fk_kykm_kykh FOREIGN KEY (kykh_id) REFERENCES public.d_kykh(kykh_id);


--
-- Name: l_ulog fk_l_ulog_slog; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.l_ulog
    ADD CONSTRAINT fk_l_ulog_slog FOREIGN KEY (slog_no) REFERENCES public.l_slog(slog_no);


--
-- Name: m_bcat fk_m_bcat_bknri; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bcat
    ADD CONSTRAINT fk_m_bcat_bknri FOREIGN KEY (bknri_id) REFERENCES public.m_bknri(bknri_id);


--
-- Name: m_bcat fk_m_bcat_genk; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bcat
    ADD CONSTRAINT fk_m_bcat_genk FOREIGN KEY (genk_id) REFERENCES public.m_genk(genk_id);


--
-- Name: m_bcat fk_m_bcat_skti; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_bcat
    ADD CONSTRAINT fk_m_bcat_skti FOREIGN KEY (skti_id) REFERENCES public.m_skti(skti_id);


--
-- Name: m_kknri fk_m_kknri_corp; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.m_kknri
    ADD CONSTRAINT fk_m_kknri_corp FOREIGN KEY (corp_id) REFERENCES public.m_corp(corp_id);


--
-- Name: t_amortization_schedule fk_schedule_act_unit; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_amortization_schedule
    ADD CONSTRAINT fk_schedule_act_unit FOREIGN KEY (act_unit_id) REFERENCES public.t_accounting_unit(act_unit_id) ON DELETE CASCADE;


--
-- Name: sec_kngn_bknri fk_sec_kngn_bknri_bknri; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_bknri
    ADD CONSTRAINT fk_sec_kngn_bknri_bknri FOREIGN KEY (bknri_id) REFERENCES public.m_bknri(bknri_id);


--
-- Name: sec_kngn_bknri fk_sec_kngn_bknri_kngn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_bknri
    ADD CONSTRAINT fk_sec_kngn_bknri_kngn FOREIGN KEY (kngn_id) REFERENCES public.sec_kngn(kngn_id);


--
-- Name: sec_kngn_kknri fk_sec_kngn_kknri_kknri; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_kknri
    ADD CONSTRAINT fk_sec_kngn_kknri_kknri FOREIGN KEY (kknri_id) REFERENCES public.m_kknri(kknri_id);


--
-- Name: sec_kngn_kknri fk_sec_kngn_kknri_kngn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_kngn_kknri
    ADD CONSTRAINT fk_sec_kngn_kknri_kngn FOREIGN KEY (kngn_id) REFERENCES public.sec_kngn(kngn_id);


--
-- Name: sec_user fk_sec_user_kngn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.sec_user
    ADD CONSTRAINT fk_sec_user_kngn FOREIGN KEY (kngn_id) REFERENCES public.sec_kngn(kngn_id);


--
-- Name: t_szei_kmk fk_t_szei_kmk_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_szei_kmk
    ADD CONSTRAINT fk_t_szei_kmk_kjkbn FOREIGN KEY (kjkbn_id) REFERENCES public.c_kjkbn(kjkbn_id);


--
-- Name: t_szei_kmk fk_t_szei_kmk_leakbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_szei_kmk
    ADD CONSTRAINT fk_t_szei_kmk_leakbn FOREIGN KEY (leakbn_id) REFERENCES public.c_leakbn(leakbn_id);


--
-- Name: t_szei_kmk fk_t_szei_kmk_szei_kjkbn; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_szei_kmk
    ADD CONSTRAINT fk_t_szei_kmk_szei_kjkbn FOREIGN KEY (szei_kjkbn_id) REFERENCES public.c_szei_kjkbn(szei_kjkbn_id);

COMMIT;
