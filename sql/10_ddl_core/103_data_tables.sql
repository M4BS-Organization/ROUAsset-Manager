-- データテーブル (d_*): 契約・物件トランザクション 6テーブル
-- 型統一: timestamp → DATE, double precision → NUMERIC(15,2)
-- ※ ctb_contract_header / ctb_contract_property との型統一のため変更

BEGIN;

SET search_path TO public;

-- ============================================================
-- データテーブル (d_*)
-- ============================================================

DROP TABLE IF EXISTS public.d_gson CASCADE;

CREATE TABLE public.d_gson (
    kykm_id integer NOT NULL,
    line_id smallint NOT NULL,
    kykh_id integer,
    kykh_no integer,
    saikaisu smallint,
    kykm_no integer,
    gson_dt date,
    gson_tmg smallint,
    gson_ryo numeric(15,2),
    gson_rkei numeric(15,2),
    gson_nm character varying(40),
    create_id integer,
    create_dt date,
    update_id integer,
    update_dt date
);

--
-- Name: TABLE d_gson; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_gson IS '減損';


DROP TABLE IF EXISTS public.d_haif CASCADE;

CREATE TABLE public.d_haif (
    kykm_id integer NOT NULL,
    line_id smallint NOT NULL,
    kykh_id integer,
    kykh_no integer,
    saikaisu smallint,
    kykm_no integer,
    haifritu numeric(5,2),
    hkmk_id integer,
    h_bcat_id integer,
    rsrvh1_id integer,
    h_klsryo numeric(15,2),
    h_mlsryo numeric(15,2),
    h_kzei numeric(15,2),
    h_mzei numeric(15,2),
    h_klsryo_zkomi numeric(15,2),
    h_mlsryo_zkomi numeric(15,2),
    h_zokusei1 character varying(100),
    h_zokusei2 character varying(100),
    h_zokusei3 character varying(100),
    h_zokusei4 character varying(100),
    h_zokusei5 character varying(100),
    h_create_id integer,
    h_create_dt date,
    h_update_id integer,
    h_update_dt date
);

--
-- Name: TABLE d_haif; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_haif IS '配賦';


DROP TABLE IF EXISTS public.d_henf CASCADE;

CREATE TABLE public.d_henf (
    kykm_id integer NOT NULL,
    line_id smallint NOT NULL,
    kykh_id integer,
    kykh_no integer,
    saikaisu smallint,
    kykm_no integer,
    shri_kn smallint,
    sshri_kn smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 date,
    klsryo numeric(15,2),
    zritu numeric(5,4),
    kzei numeric(15,2),
    klsryo_zkomi numeric(15,2),
    shri_en_dt date,
    shho_id integer,
    start_dt date,
    end_dt date,
    kikan smallint,
    create_id integer,
    create_dt date,
    update_id integer,
    update_dt date
);

--
-- Name: TABLE d_henf; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_henf IS '変更ファイナンス';


DROP TABLE IF EXISTS public.d_henl CASCADE;

CREATE TABLE public.d_henl (
    kykm_id integer NOT NULL,
    line_id smallint NOT NULL,
    kykh_id integer,
    kykh_no integer,
    saikaisu smallint,
    kykm_no integer,
    shri_kn smallint,
    sshri_kn smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 date,
    klsryo numeric(15,2),
    zritu numeric(5,4),
    kzei numeric(15,2),
    klsryo_zkomi numeric(15,2),
    shri_en_dt date,
    shho_id integer,
    create_id integer,
    create_dt date,
    update_id integer,
    update_dt date
);

--
-- Name: TABLE d_henl; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_henl IS '変更リース';


DROP TABLE IF EXISTS public.d_kykh CASCADE;

CREATE TABLE public.d_kykh (
    kykh_id integer NOT NULL,
    kykh_no integer,
    saikaisu smallint,
    update_cnt integer DEFAULT 0,
    k_create_id integer,
    k_create_dt date,
    k_update_id integer,
    k_update_dt date,
    kknri_id integer,
    kkbn_id smallint,
    lcpt_id integer,
    kykbnl character varying(30),
    kykbnj character varying(30),
    kyak_dt date,
    start_dt date,
    end_dt date,
    lkikan smallint,
    kykm_cnt integer DEFAULT 0,
    k_suuryo integer,
    k_knyukn numeric(15,2),
    ryoritu numeric(10,6),
    k_glsryo numeric(15,2),
    k_klsryo numeric(15,2),
    k_mlsryo numeric(15,2),
    k_slsryo numeric(15,2),
    zritu numeric(5,4),
    k_gzei numeric(15,2),
    k_kzei numeric(15,2),
    k_mzei numeric(15,2),
    k_glsryo_zkomi numeric(15,2),
    k_klsryo_zkomi numeric(15,2),
    k_mlsryo_zkomi numeric(15,2),
    k_ijiknr numeric(15,2),
    k_zanryo numeric(15,2),
    kykm_cnt_kzok integer,
    suuryo_kzok integer,
    knyukn_kzok numeric(15,2),
    glsryo_kzok numeric(15,2),
    klsryo_kzok numeric(15,2),
    mlsryo_kzok numeric(15,2),
    slsryo_kzok numeric(15,2),
    ijiknr_kzok numeric(15,2),
    zanryo_kzok numeric(15,2),
    shri_kn smallint,
    sshri_kn_m smallint,
    sshri_kn_1 smallint,
    sshri_kn_2 smallint,
    sshri_kn_3 smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 date,
    shri_dt2 date,
    shri_dt3 smallint,
    shri_en_dt date,
    mkaisu smallint,
    mae_dt date,
    jencho_f boolean DEFAULT false,
    koza_id integer,
    shho_m_id integer,
    shho_1_id integer,
    shho_2_id integer,
    shho_3_id integer,
    k_henl_f boolean DEFAULT false,
    k_henl_sum numeric(15,2),
    henl_sum_kzok numeric(15,2),
    k_henf_f boolean DEFAULT false,
    kyak_end_f boolean DEFAULT false,
    k_ckaiyk_f boolean DEFAULT false,
    k_rend_dt date,
    k_history_f boolean DEFAULT false,
    k_seigou_f boolean DEFAULT false,
    rsrvk1_id integer,
    kyak_nm character varying(100),
    k_zokusei1 character varying(100),
    k_zokusei2 character varying(100),
    k_zokusei3 character varying(100),
    k_zokusei4 character varying(100),
    k_zokusei5 character varying(100),
    rng_bango character varying(30),
    shonin_dt date,
    kiansha character varying(20),
    kjkbn_id smallint,
    kjkbn_ms_f boolean DEFAULT false,
    skyu_kj_f smallint,
    kj_tekiyo_dt date,
    kj_lkikan smallint,
    kj_k_slsryo numeric(15,2),
    kj_shri_cnt smallint DEFAULT 0,
    k_kjyo_st_dt date,
    k_kjyo_en_dt date
);

--
-- Name: TABLE d_kykh; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_kykh IS '契約ヘッダ';


DROP TABLE IF EXISTS public.d_kykm CASCADE;

CREATE TABLE public.d_kykm (
    kykm_id integer NOT NULL,
    kykh_id integer,
    kykh_no integer,
    kykm_no integer,
    kykm_no_mae integer,
    saikaisu smallint,
    b_kedaban character varying(10),
    bukn_bango1 character varying(30),
    bukn_bango2 character varying(30),
    bukn_bango3 character varying(30),
    b_create_id integer,
    b_create_dt date,
    b_update_id integer,
    b_update_dt date,
    b_suuryo integer,
    suuryo_sum_f boolean DEFAULT false,
    b_knyukn numeric(15,2),
    b_glsryo numeric(15,2),
    b_klsryo numeric(15,2),
    b_mlsryo numeric(15,2),
    b_slsryo numeric(15,2),
    b_gzei numeric(15,2),
    b_kzei numeric(15,2),
    b_mzei numeric(15,2),
    b_glsryo_zkomi numeric(15,2),
    b_klsryo_zkomi numeric(15,2),
    b_mlsryo_zkomi numeric(15,2),
    b_ijiknr numeric(15,2),
    b_zanryo numeric(15,2),
    b_ghassei numeric(15,2),
    b_gnzai_kt numeric(15,2),
    b_syutok numeric(15,2),
    kari_ritu numeric(10,6),
    kari_ritu_ms_f boolean DEFAULT false,
    ksan_ritu numeric(10,6),
    taiyo_nen smallint,
    taiyo_nen_ms_f boolean DEFAULT false,
    rslt90p numeric(15,2),
    rslt90p_str character varying(10),
    rslt75p numeric(15,2),
    rslt75p_str character varying(10),
    leakbn_id smallint,
    leakbn_id_ms_f boolean DEFAULT false,
    chuum_id smallint,
    chuum_id_ms_f boolean DEFAULT false,
    chu_hnti_id smallint,
    b_lb_soneki numeric(15,2),
    lb_chuki_f boolean DEFAULT false,
    hkho_id integer,
    hk_dt date,
    hk_gsha_id integer,
    b_ckaiyk_f boolean DEFAULT false,
    ckaiyk_dt date,
    ckaiyk_esdt_t date,
    ckaiyk_esdt_h date,
    iyaku_kin numeric(15,2),
    b_henl_f boolean DEFAULT false,
    b_henl_sum numeric(15,2),
    b_henl_sedt date,
    b_henf_f boolean DEFAULT false,
    b_henf_sedt date,
    b_henf_klsryo_new numeric(15,2),
    f_lcpt_id integer,
    f_hkmk_id integer,
    f_gsha_id integer,
    kykbnf character varying(30),
    genson_f boolean DEFAULT false,
    b_rend_dt date,
    b_seigou_f boolean DEFAULT false,
    skmk_id integer,
    b_bcat_id integer,
    ido_dt date,
    b_bcat_id_r1 integer,
    ido_dt_r1 date,
    b_bcat_id_r2 integer,
    ido_dt_r2 date,
    b_bcat_id_r3 integer,
    ido_dt_r3 date,
    k_gsha_id integer,
    bkind_id integer,
    mcpt_id integer,
    rsrvb1_id integer,
    b_smdt_fst_sum date,
    b_smdt_lst_sum date,
    b_shdt_fst_sum date,
    b_shdt_lst_sum date,
    setti_dt date,
    bukn_nm character varying(100),
    b_zokusei1 character varying(100),
    b_zokusei2 character varying(100),
    b_zokusei3 character varying(100),
    b_zokusei4 character varying(100),
    b_zokusei5 character varying(100),
    b_gson_f boolean DEFAULT false,
    rsok_tmg smallint,
    gk_calc_kind smallint,
    hensai_kind smallint,
    ij_kjyo_kind smallint,
    gson_tk_kind smallint,
    lb_kjyo_kind smallint,
    rsok_tmg_ms_f boolean DEFAULT false,
    gk_calc_kind_ms_f boolean DEFAULT false,
    hensai_kind_ms_f boolean DEFAULT false,
    ij_kjyo_kind_ms_f boolean DEFAULT false,
    gson_tk_kind_ms_f boolean DEFAULT false,
    lb_kjyo_kind_ms_f boolean DEFAULT false,
    kjkbn_id smallint,
    skyak_ho_id smallint,
    kj_flg smallint,
    syk_kikan smallint,
    kj_kkakaku numeric(15,2),
    kj_ksan_ritu numeric(10,6),
    kj_ho smallint,
    kj_b_slsryo numeric(15,2),
    kj_b_ijiknr numeric(15,2),
    kjkbn_ms_f boolean DEFAULT false,
    b_kjyo_en_dt date,
    b_bcat_cd_k character varying(12),
    b_bcat_nm_k character varying(40),
    b_sairyo numeric(15,2),
    szei_kjkbn_id smallint,
    szei_kjkbn_id_ms_f boolean DEFAULT false,
    hszei_kjkbn_id smallint,
    hszei_kjkbn_id_ms_f boolean DEFAULT false
);

--
-- Name: TABLE d_kykm; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_kykm IS '物件明細';

COMMIT;
