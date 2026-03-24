-- データテーブル (d_*): 契約・物件トランザクション 6テーブル

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
    kykh_no double precision,
    saikaisu smallint,
    kykm_no double precision,
    gson_dt timestamp without time zone,
    gson_tmg smallint,
    gson_ryo double precision,
    gson_rkei double precision,
    gson_nm character varying(40),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone
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
    kykh_no double precision,
    saikaisu smallint,
    kykm_no double precision,
    haifritu double precision,
    hkmk_id integer,
    h_bcat_id integer,
    rsrvh1_id integer,
    h_klsryo double precision,
    h_mlsryo double precision,
    h_kzei double precision,
    h_mzei double precision,
    h_klsryo_zkomi double precision,
    h_mlsryo_zkomi double precision,
    h_zokusei1 character varying(100),
    h_zokusei2 character varying(100),
    h_zokusei3 character varying(100),
    h_zokusei4 character varying(100),
    h_zokusei5 character varying(100),
    h_create_id integer,
    h_create_dt timestamp without time zone,
    h_update_id integer,
    h_update_dt timestamp without time zone
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
    kykh_no double precision,
    saikaisu smallint,
    kykm_no double precision,
    shri_kn smallint,
    sshri_kn smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 timestamp without time zone,
    klsryo double precision,
    zritu double precision,
    kzei double precision,
    klsryo_zkomi double precision,
    shri_en_dt timestamp without time zone,
    shho_id integer,
    start_dt timestamp without time zone,
    end_dt timestamp without time zone,
    kikan smallint,
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone
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
    kykh_no double precision,
    saikaisu smallint,
    kykm_no double precision,
    shri_kn smallint,
    sshri_kn smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 timestamp without time zone,
    klsryo double precision,
    zritu double precision,
    kzei double precision,
    klsryo_zkomi double precision,
    shri_en_dt timestamp without time zone,
    shho_id integer,
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone
);

--
-- Name: TABLE d_henl; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_henl IS '変更リース';


DROP TABLE IF EXISTS public.d_kykh CASCADE;

CREATE TABLE public.d_kykh (
    kykh_id integer NOT NULL,
    kykh_no double precision,
    saikaisu smallint,
    update_cnt integer DEFAULT 0,
    k_create_id integer,
    k_create_dt timestamp without time zone,
    k_update_id integer,
    k_update_dt timestamp without time zone,
    kknri_id integer,
    kkbn_id smallint,
    lcpt_id integer,
    kykbnl character varying(30),
    kykbnj character varying(30),
    kyak_dt timestamp without time zone,
    start_dt timestamp without time zone,
    end_dt timestamp without time zone,
    lkikan smallint,
    kykm_cnt integer DEFAULT 0,
    k_suuryo integer,
    k_knyukn double precision,
    ryoritu double precision,
    k_glsryo double precision,
    k_klsryo double precision,
    k_mlsryo double precision,
    k_slsryo double precision,
    zritu double precision,
    k_gzei double precision,
    k_kzei double precision,
    k_mzei double precision,
    k_glsryo_zkomi double precision,
    k_klsryo_zkomi double precision,
    k_mlsryo_zkomi double precision,
    k_ijiknr double precision,
    k_zanryo double precision,
    kykm_cnt_kzok integer,
    suuryo_kzok integer,
    knyukn_kzok double precision,
    glsryo_kzok double precision,
    klsryo_kzok double precision,
    mlsryo_kzok double precision,
    slsryo_kzok double precision,
    ijiknr_kzok double precision,
    zanryo_kzok double precision,
    shri_kn smallint,
    sshri_kn_m smallint,
    sshri_kn_1 smallint,
    sshri_kn_2 smallint,
    sshri_kn_3 smallint,
    shri_cnt smallint DEFAULT 0,
    shri_dt1 timestamp without time zone,
    shri_dt2 timestamp without time zone,
    shri_dt3 smallint,
    shri_en_dt timestamp without time zone,
    mkaisu smallint,
    mae_dt timestamp without time zone,
    jencho_f boolean DEFAULT false,
    koza_id integer,
    shho_m_id integer,
    shho_1_id integer,
    shho_2_id integer,
    shho_3_id integer,
    k_henl_f boolean DEFAULT false,
    k_henl_sum double precision,
    henl_sum_kzok double precision,
    k_henf_f boolean DEFAULT false,
    kyak_end_f boolean DEFAULT false,
    k_ckaiyk_f boolean DEFAULT false,
    k_rend_dt timestamp without time zone,
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
    shonin_dt timestamp without time zone,
    kiansha character varying(20),
    kjkbn_id smallint,
    kjkbn_ms_f boolean DEFAULT false,
    skyu_kj_f smallint,
    kj_tekiyo_dt timestamp without time zone,
    kj_lkikan smallint,
    kj_k_slsryo double precision,
    kj_shri_cnt smallint DEFAULT 0,
    k_kjyo_st_dt timestamp without time zone,
    k_kjyo_en_dt timestamp without time zone
);

--
-- Name: TABLE d_kykh; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.d_kykh IS '契約ヘッダ';


DROP TABLE IF EXISTS public.d_kykm CASCADE;

CREATE TABLE public.d_kykm (
    kykm_id integer NOT NULL,
    kykh_id integer,
    kykh_no double precision,
    kykm_no double precision,
    kykm_no_mae double precision,
    saikaisu smallint,
    b_kedaban character varying(10),
    bukn_bango1 character varying(30),
    bukn_bango2 character varying(30),
    bukn_bango3 character varying(30),
    b_create_id integer,
    b_create_dt timestamp without time zone,
    b_update_id integer,
    b_update_dt timestamp without time zone,
    b_suuryo integer,
    suuryo_sum_f boolean DEFAULT false,
    b_knyukn double precision,
    b_glsryo double precision,
    b_klsryo double precision,
    b_mlsryo double precision,
    b_slsryo double precision,
    b_gzei double precision,
    b_kzei double precision,
    b_mzei double precision,
    b_glsryo_zkomi double precision,
    b_klsryo_zkomi double precision,
    b_mlsryo_zkomi double precision,
    b_ijiknr double precision,
    b_zanryo double precision,
    b_ghassei double precision,
    b_gnzai_kt double precision,
    b_syutok double precision,
    kari_ritu double precision,
    kari_ritu_ms_f boolean DEFAULT false,
    ksan_ritu double precision,
    taiyo_nen smallint,
    taiyo_nen_ms_f boolean DEFAULT false,
    rslt90p double precision,
    rslt90p_str character varying(10),
    rslt75p double precision,
    rslt75p_str character varying(10),
    leakbn_id smallint,
    leakbn_id_ms_f boolean DEFAULT false,
    chuum_id smallint,
    chuum_id_ms_f boolean DEFAULT false,
    chu_hnti_id smallint,
    b_lb_soneki double precision,
    lb_chuki_f boolean DEFAULT false,
    hkho_id integer,
    hk_dt timestamp without time zone,
    hk_gsha_id integer,
    b_ckaiyk_f boolean DEFAULT false,
    ckaiyk_dt timestamp without time zone,
    ckaiyk_esdt_t timestamp without time zone,
    ckaiyk_esdt_h timestamp without time zone,
    iyaku_kin double precision,
    b_henl_f boolean DEFAULT false,
    b_henl_sum double precision,
    b_henl_sedt timestamp without time zone,
    b_henf_f boolean DEFAULT false,
    b_henf_sedt timestamp without time zone,
    b_henf_klsryo_new double precision,
    f_lcpt_id integer,
    f_hkmk_id integer,
    f_gsha_id integer,
    kykbnf character varying(30),
    genson_f boolean DEFAULT false,
    b_rend_dt timestamp without time zone,
    b_seigou_f boolean DEFAULT false,
    skmk_id integer,
    b_bcat_id integer,
    ido_dt timestamp without time zone,
    b_bcat_id_r1 integer,
    ido_dt_r1 timestamp without time zone,
    b_bcat_id_r2 integer,
    ido_dt_r2 timestamp without time zone,
    b_bcat_id_r3 integer,
    ido_dt_r3 timestamp without time zone,
    k_gsha_id integer,
    bkind_id integer,
    mcpt_id integer,
    rsrvb1_id integer,
    b_smdt_fst_sum timestamp without time zone,
    b_smdt_lst_sum timestamp without time zone,
    b_shdt_fst_sum timestamp without time zone,
    b_shdt_lst_sum timestamp without time zone,
    setti_dt timestamp without time zone,
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
    kj_kkakaku double precision,
    kj_ksan_ritu double precision,
    kj_ho smallint,
    kj_b_slsryo double precision,
    kj_b_ijiknr double precision,
    kjkbn_ms_f boolean DEFAULT false,
    b_kjyo_en_dt timestamp without time zone,
    b_bcat_cd_k character varying(12),
    b_bcat_nm_k character varying(40),
    b_sairyo double precision,
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
