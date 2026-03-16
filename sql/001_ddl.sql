-- ============================================================
-- LeaseM4BS PostgreSQL DDL
-- Access DB -> PostgreSQL 移行用テーブル定義
-- Generated from pg_dump output: 2026-03-16
-- Tables: 66, Indexes: 150+, FK Constraints: 56
-- ============================================================

BEGIN;

SET search_path TO public;

-- ============================================================
-- テーブル削除 (DROP TABLE) - FK依存関係の逆順
-- ============================================================

DROP TABLE IF EXISTS public.tw_m_user CASCADE;
DROP TABLE IF EXISTS public.tc_swk_settei CASCADE;
DROP TABLE IF EXISTS public.tc_swk_def_com CASCADE;
DROP TABLE IF EXISTS public.tc_reg_report CASCADE;
DROP TABLE IF EXISTS public.tc_rec_shri CASCADE;
DROP TABLE IF EXISTS public.tc_hrel CASCADE;
DROP TABLE IF EXISTS public.t_zei_kaisei CASCADE;
DROP TABLE IF EXISTS public.t_szei_kmk CASCADE;
DROP TABLE IF EXISTS public.t_system CASCADE;
DROP TABLE IF EXISTS public.t_swk_nm CASCADE;
DROP TABLE IF EXISTS public.t_shwak_d CASCADE;
DROP TABLE IF EXISTS public.t_settei CASCADE;
DROP TABLE IF EXISTS public.t_seq CASCADE;
DROP TABLE IF EXISTS public.t_req CASCADE;
DROP TABLE IF EXISTS public.t_opt CASCADE;
DROP TABLE IF EXISTS public.t_mstk CASCADE;
DROP TABLE IF EXISTS public.t_kykbnj_seq CASCADE;
DROP TABLE IF EXISTS public.t_kari_ritu CASCADE;
DROP TABLE IF EXISTS public.t_journal_setting CASCADE;
DROP TABLE IF EXISTS public.t_holiday CASCADE;
DROP TABLE IF EXISTS public.t_db_version CASCADE;
DROP TABLE IF EXISTS public.t_audit_log CASCADE;
DROP TABLE IF EXISTS public.t_amortization_schedule CASCADE;
DROP TABLE IF EXISTS public.t_accounting_unit CASCADE;
DROP TABLE IF EXISTS public.sec_user CASCADE;
DROP TABLE IF EXISTS public.sec_kngn_kknri CASCADE;
DROP TABLE IF EXISTS public.sec_kngn_bknri CASCADE;
DROP TABLE IF EXISTS public.sec_kngn CASCADE;
DROP TABLE IF EXISTS public.m_swptn CASCADE;
DROP TABLE IF EXISTS public.l_ulog CASCADE;
DROP TABLE IF EXISTS public.l_slog CASCADE;
DROP TABLE IF EXISTS public.l_bklog CASCADE;
DROP TABLE IF EXISTS public.d_henl CASCADE;
DROP TABLE IF EXISTS public.d_henf CASCADE;
DROP TABLE IF EXISTS public.d_haif CASCADE;
DROP TABLE IF EXISTS public.m_rsrvh1 CASCADE;
DROP TABLE IF EXISTS public.d_gson CASCADE;
DROP TABLE IF EXISTS public.d_kykm CASCADE;
DROP TABLE IF EXISTS public.m_skmk CASCADE;
DROP TABLE IF EXISTS public.m_rsrvb1 CASCADE;
DROP TABLE IF EXISTS public.m_mcpt CASCADE;
DROP TABLE IF EXISTS public.m_hkho CASCADE;
DROP TABLE IF EXISTS public.m_hkmk CASCADE;
DROP TABLE IF EXISTS public.m_gsha CASCADE;
DROP TABLE IF EXISTS public.m_bkind CASCADE;
DROP TABLE IF EXISTS public.m_bcat CASCADE;
DROP TABLE IF EXISTS public.m_skti CASCADE;
DROP TABLE IF EXISTS public.m_genk CASCADE;
DROP TABLE IF EXISTS public.m_bknri CASCADE;
DROP TABLE IF EXISTS public.d_kykh CASCADE;
DROP TABLE IF EXISTS public.m_shho CASCADE;
DROP TABLE IF EXISTS public.m_rsrvk1 CASCADE;
DROP TABLE IF EXISTS public.m_lcpt CASCADE;
DROP TABLE IF EXISTS public.m_koza CASCADE;
DROP TABLE IF EXISTS public.m_kknri CASCADE;
DROP TABLE IF EXISTS public.m_corp CASCADE;
DROP TABLE IF EXISTS public.c_szei_kjkbn CASCADE;
DROP TABLE IF EXISTS public.c_skyak_ho CASCADE;
DROP TABLE IF EXISTS public.c_settei_idfld CASCADE;
DROP TABLE IF EXISTS public.c_rcalc CASCADE;
DROP TABLE IF EXISTS public.c_leakbn CASCADE;
DROP TABLE IF EXISTS public.c_kkbn CASCADE;
DROP TABLE IF EXISTS public.c_kjtaisyo CASCADE;
DROP TABLE IF EXISTS public.c_kjkbn CASCADE;
DROP TABLE IF EXISTS public.c_chuum CASCADE;
DROP TABLE IF EXISTS public.c_chu_hnti CASCADE;

-- ============================================================
-- コードテーブル (c_*)
-- ============================================================

CREATE TABLE public.c_chu_hnti (
    chu_hnti_id smallint NOT NULL,
    chu_hnti_nm character varying(100)
);

--
-- Name: TABLE c_chu_hnti; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_chu_hnti IS '注記単位';


CREATE TABLE public.c_chuum (
    chuum_id smallint NOT NULL,
    chuum_nm character varying(10)
);

--
-- Name: TABLE c_chuum; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_chuum IS '注記有無';


CREATE TABLE public.c_kjkbn (
    kjkbn_id smallint NOT NULL,
    kjkbn_nm character varying(10)
);

--
-- Name: TABLE c_kjkbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kjkbn IS '計上区分';


CREATE TABLE public.c_kjtaisyo (
    kjtaisyo_id smallint CONSTRAINT c_kjtaisyo_kjkbn_id_not_null NOT NULL,
    kjtaisyo_nm character varying(10)
);

--
-- Name: TABLE c_kjtaisyo; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kjtaisyo IS '計上対象';


CREATE TABLE public.c_kkbn (
    kkbn_id smallint NOT NULL,
    kkbn_nm character varying(50)
);

--
-- Name: TABLE c_kkbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kkbn IS '契約区分';


CREATE TABLE public.c_leakbn (
    leakbn_id smallint NOT NULL,
    leakbn_nm character varying(100)
);

--
-- Name: TABLE c_leakbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_leakbn IS 'リース区分';


CREATE TABLE public.c_rcalc (
    rcalc_id smallint NOT NULL,
    rcalc_nm character varying(10)
);

--
-- Name: TABLE c_rcalc; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_rcalc IS '再計算区分';


CREATE TABLE public.c_settei_idfld (
    settei_id integer NOT NULL,
    val_id integer NOT NULL,
    val_short_nm character varying(100),
    val_nm character varying(100)
);

--
-- Name: TABLE c_settei_idfld; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_settei_idfld IS '設定IDフィールド';


CREATE TABLE public.c_skyak_ho (
    skyak_ho_id smallint NOT NULL,
    skyak_ho_nm character varying(10)
);

--
-- Name: TABLE c_skyak_ho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_skyak_ho IS '償却方法';


CREATE TABLE public.c_szei_kjkbn (
    szei_kjkbn_id smallint NOT NULL,
    szei_kjkbn_nm character varying(50),
    szei_keijo_tmg smallint,
    kojo_taisyo smallint,
    kgai_tmg smallint,
    hosyu_f smallint,
    d_order smallint
);

--
-- Name: TABLE c_szei_kjkbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_szei_kjkbn IS '消費税計上区分';


-- ============================================================
-- データテーブル (d_*)
-- ============================================================

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


-- ============================================================
-- ログテーブル (l_*)
-- ============================================================

CREATE TABLE public.l_bklog (
    op_dt timestamp without time zone NOT NULL,
    op_nm character varying(8),
    op_s character varying(20),
    op_user_cd character varying(12),
    op_user_nm character varying(40),
    pc_name character varying(40),
    ip_adr character varying(100),
    win_user character varying(40),
    file_name character varying(40),
    folder character varying(255),
    pwd character varying(255),
    db_version character varying(30)
);

--
-- Name: TABLE l_bklog; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.l_bklog IS 'バックアップログ';


CREATE TABLE public.l_slog (
    slog_no integer NOT NULL,
    op_st_dt timestamp without time zone,
    op_en_dt timestamp without time zone,
    op_kbn character varying(3),
    op_nm character varying(50),
    op_s character varying(40),
    op_user_cd character varying(12),
    op_user_nm character varying(40),
    pc_name character varying(40),
    ip_adr character varying(100),
    win_user character varying(40),
    op_detail1 character varying(255),
    op_detail2 text,
    upd_sbt character varying(6),
    yobi character varying(40)
);

--
-- Name: TABLE l_slog; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.l_slog IS 'セッションログ';


CREATE TABLE public.l_ulog (
    slog_no integer NOT NULL,
    ulog_no integer NOT NULL,
    tbl_nm character varying(40),
    upd_nm character varying(4),
    key_nm1 character varying(50),
    key_val1 character varying(30),
    key_nm2 character varying(50),
    key_val2 character varying(30),
    rec1 text,
    rec2 text,
    db_version character varying(30),
    recf character varying(1),
    yobi character varying(40)
);

--
-- Name: TABLE l_ulog; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.l_ulog IS '更新ログ';


-- ============================================================
-- マスタテーブル (m_*)
-- ============================================================

CREATE TABLE public.m_bcat (
    bcat_id integer NOT NULL,
    bcat1_cd character varying(12),
    bcat1_nm character varying(80),
    bcat2_cd character varying(12),
    bcat2_nm character varying(40),
    bcat3_cd character varying(12),
    bcat3_nm character varying(40),
    bcat4_cd character varying(12),
    bcat4_nm character varying(40),
    bcat5_cd character varying(12),
    bcat5_nm character varying(40),
    genk_id integer,
    skti_id integer,
    sum1_cd character varying(12),
    sum1_nm character varying(40),
    sum2_cd character varying(12),
    sum2_nm character varying(40),
    sum3_cd character varying(12),
    sum3_nm character varying(40),
    bknri_id integer,
    kbf_kb boolean DEFAULT false,
    kbf_fb boolean DEFAULT false,
    kbf_sb boolean DEFAULT false,
    gensonf boolean DEFAULT false,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_bcat; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_bcat IS '管理部署';


CREATE TABLE public.m_bkind (
    bkind_id integer NOT NULL,
    bkind_cd character varying(12),
    bkind_nm character varying(40),
    bkind2_cd character varying(12),
    bkind2_nm character varying(40),
    bkind3_cd character varying(12),
    bkind3_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_bkind; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_bkind IS '物件種別';


CREATE TABLE public.m_bknri (
    bknri_id integer NOT NULL,
    bknri1_cd character varying(12),
    bknri1_nm character varying(80),
    bknri2_cd character varying(12),
    bknri2_nm character varying(40),
    bknri3_cd character varying(12),
    bknri3_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_bknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_bknri IS '物件分類';


CREATE TABLE public.m_corp (
    corp_id integer NOT NULL,
    corp1_cd character varying(12),
    corp1_nm character varying(40),
    corp2_cd character varying(12),
    corp2_nm character varying(40),
    corp3_cd character varying(12),
    corp3_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_corp; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_corp IS '法人';


CREATE TABLE public.m_genk (
    genk_id integer NOT NULL,
    genk_cd character varying(12),
    genk_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_genk; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_genk IS '原価分類';


CREATE TABLE public.m_gsha (
    gsha_id integer NOT NULL,
    gsha_cd character varying(12),
    gsha_nm character varying(40),
    biko character varying(200),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_gsha; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_gsha IS '購入先・業者';


CREATE TABLE public.m_hkho (
    hkho_id integer NOT NULL,
    hkho_cd character varying(12),
    hkho_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_hkho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_hkho IS '返却方法';


CREATE TABLE public.m_hkmk (
    hkmk_id integer NOT NULL,
    hkmk_cd character varying(12),
    hkmk_nm character varying(40),
    knjkb_id integer,
    sum1_cd character varying(12),
    sum1_nm character varying(40),
    sum2_cd character varying(12),
    sum2_nm character varying(40),
    sum3_cd character varying(12),
    sum3_nm character varying(40),
    hrel_ptn_cd3 character varying(12),
    hrel_ptn_nm3 character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_hkmk; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_hkmk IS '費用区分';


CREATE TABLE public.m_kknri (
    kknri_id integer NOT NULL,
    kknri1_cd character varying(12),
    kknri1_nm character varying(40),
    kknri2_cd character varying(12),
    kknri2_nm character varying(40),
    kknri3_cd character varying(12),
    kknri3_nm character varying(40),
    corp_id integer,
    hrel_ptn_cd4 character varying(12),
    hrel_ptn_nm4 character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_kknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_kknri IS '契約管理単位';


CREATE TABLE public.m_koza (
    koza_id integer NOT NULL,
    koza_cd character varying(12),
    koza_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_koza; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_koza IS '口座';


CREATE TABLE public.m_lcpt (
    lcpt_id integer NOT NULL,
    lcpt1_cd character varying(12),
    lcpt1_nm character varying(40),
    lcpt2_cd character varying(12),
    lcpt2_nm character varying(40),
    shime_day_1 smallint,
    sshri_kn1_1 smallint,
    shri_day1_1 smallint,
    sshri_kn2_1 smallint,
    shri_day2_1 smallint,
    shime_day_2 smallint,
    sshri_kn1_2 smallint,
    shri_day1_2 smallint,
    sshri_kn2_2 smallint,
    shri_day2_2 smallint,
    shime_day_3 smallint,
    sshri_kn1_3 smallint,
    shri_day1_3 smallint,
    sshri_kn2_3 smallint,
    shri_day2_3 smallint,
    sai_denomi smallint,
    biko character varying(200),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false,
    sum1_cd character varying(12),
    sum1_nm character varying(40),
    sum2_cd character varying(12),
    sum2_nm character varying(40),
    sum3_cd character varying(12),
    sum3_nm character varying(40),
    shri_kn_ini smallint,
    slkikan_s smallint,
    shri_kn_s smallint,
    sai_denomi_s smallint,
    sai_numerator_s smallint,
    slkikan_n smallint,
    shri_kn_n smallint,
    sai_denomi_n smallint,
    sai_numerator_n smallint,
    shri_cnt_s_1 smallint,
    shho_id_s_1 integer,
    shho_id_n_1 integer,
    shri_cnt_s_2 smallint,
    shho_id_s_2 integer,
    shho_id_n_2 integer,
    shri_cnt_s_3 smallint,
    shho_id_s_3 integer,
    shho_id_n_3 integer
);

--
-- Name: TABLE m_lcpt; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_lcpt IS 'リース会社・支払先';


CREATE TABLE public.m_mcpt (
    mcpt_id integer NOT NULL,
    mcpt_cd character varying(12),
    mcpt_nm character varying(40),
    biko character varying(200),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_mcpt; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_mcpt IS 'メーカー';


CREATE TABLE public.m_rsrvb1 (
    rsrvb1_id integer NOT NULL,
    rsrvb1_cd character varying(12),
    rsrvb1_nm character varying(40),
    num double precision,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_rsrvb1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_rsrvb1 IS '物件予備1';


CREATE TABLE public.m_rsrvh1 (
    rsrvh1_id integer NOT NULL,
    rsrvh1_cd character varying(12),
    rsrvh1_nm character varying(40),
    num double precision,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_rsrvh1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_rsrvh1 IS '配賦予備1';


CREATE TABLE public.m_rsrvk1 (
    rsrvk1_id integer NOT NULL,
    rsrvk1_cd character varying(12),
    rsrvk1_nm character varying(40),
    num double precision,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_rsrvk1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_rsrvk1 IS '契約予備1';


CREATE TABLE public.m_shho (
    shho_id integer NOT NULL,
    shho_cd character varying(12),
    shho_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_shho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_shho IS '支払方法';


CREATE TABLE public.m_skmk (
    skmk_id integer NOT NULL,
    skmk_cd character varying(12),
    skmk_nm character varying(40),
    knjkb_id integer,
    sum1_cd character varying(20),
    sum1_nm character varying(80),
    sum2_cd character varying(20),
    sum2_nm character varying(80),
    sum3_cd character varying(20),
    sum3_nm character varying(80),
    sum4_cd character varying(20),
    sum4_nm character varying(80),
    sum5_cd character varying(20),
    sum5_nm character varying(80),
    sum6_cd character varying(20),
    sum6_nm character varying(80),
    sum7_cd character varying(20),
    sum7_nm character varying(80),
    sum8_cd character varying(20),
    sum8_nm character varying(80),
    sum9_cd character varying(20),
    sum9_nm character varying(80),
    sum10_cd character varying(20),
    sum10_nm character varying(80),
    sum11_cd character varying(20),
    sum11_nm character varying(80),
    sum12_cd character varying(20),
    sum12_nm character varying(80),
    sum13_cd character varying(20),
    sum13_nm character varying(80),
    sum14_cd character varying(20),
    sum14_nm character varying(80),
    sum15_cd character varying(20),
    sum15_nm character varying(80),
    hrel_ptn_cd1 character varying(12),
    hrel_ptn_nm1 character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_skmk; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_skmk IS '集計区分';


CREATE TABLE public.m_skti (
    skti_id integer NOT NULL,
    skti_cd character varying(12),
    skti_nm character varying(40),
    sktsyt character varying(40),
    jgsyonm character varying(40),
    jgsyopst character varying(10),
    jgsyoadr character varying(100),
    jgsyotel character varying(20),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_skti; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_skti IS '事業体';


CREATE TABLE public.m_swptn (
    swptn_id integer NOT NULL,
    swptn_cd character varying(12),
    swptn_nm character varying(40),
    kmk1_cd character varying(12),
    kmk1_nm character varying(40),
    kmk2_cd character varying(12),
    kmk2_nm character varying(40),
    kmk3_cd character varying(12),
    kmk3_nm character varying(40),
    kmk4_cd character varying(12),
    kmk4_nm character varying(40),
    kmk5_cd character varying(12),
    kmk5_nm character varying(40),
    kmk6_cd character varying(12),
    kmk6_nm character varying(40),
    kmk7_cd character varying(12),
    kmk7_nm character varying(40),
    kmk8_cd character varying(12),
    kmk8_nm character varying(40),
    kmk9_cd character varying(12),
    kmk9_nm character varying(40),
    kmk10_cd character varying(12),
    kmk10_nm character varying(40),
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE m_swptn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.m_swptn IS '仕訳パターン';


-- ============================================================
-- セキュリティテーブル (sec_*)
-- ============================================================

CREATE TABLE public.sec_kngn (
    kngn_id integer NOT NULL,
    kngn_cd character varying(12),
    kngn_nm character varying(40),
    access_kind smallint,
    access_kind_b smallint,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false,
    admin boolean DEFAULT false,
    master_update boolean DEFAULT false,
    file_output boolean DEFAULT false,
    print boolean DEFAULT false,
    log_ref boolean DEFAULT false,
    approval boolean DEFAULT false
);

--
-- Name: TABLE sec_kngn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_kngn IS '権限';


CREATE TABLE public.sec_kngn_bknri (
    kngn_id integer NOT NULL,
    bknri_id integer NOT NULL,
    access_kind smallint
);

--
-- Name: TABLE sec_kngn_bknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_kngn_bknri IS '権限別物件分類';


CREATE TABLE public.sec_kngn_kknri (
    kngn_id integer NOT NULL,
    kknri_id integer NOT NULL,
    access_kind smallint
);

--
-- Name: TABLE sec_kngn_kknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_kngn_kknri IS '権限別契約管理単位';


CREATE TABLE public.sec_user (
    user_id integer NOT NULL,
    user_cd character varying(12),
    user_nm character varying(40),
    pwd character varying(255),
    kngn_id integer,
    biko character varying(100),
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false,
    login_attempts smallint,
    pwd_life_time smallint,
    pwd_grace_time smallint,
    pwd_min smallint,
    pwd_moji_chk boolean DEFAULT false,
    pwd_alph_chk boolean DEFAULT false,
    pwd_num_chk boolean DEFAULT false,
    pwd_symbol_chk boolean DEFAULT false,
    pwd_upd_dt timestamp without time zone,
    d_first_login timestamp without time zone,
    err_ct smallint,
    last_err_dt timestamp without time zone
);

--
-- Name: TABLE sec_user; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_user IS 'ユーザー';


-- ============================================================
-- トランザクション/設定テーブル (t_*)
-- ============================================================

CREATE TABLE public.t_accounting_unit (
    act_unit_id integer NOT NULL,
    kykh_id integer NOT NULL,
    lease_type character varying(20) DEFAULT 'FINANCE'::character varying NOT NULL,
    on_balance_flag boolean DEFAULT true,
    impairment_flag boolean DEFAULT false,
    currency_code character varying(3) DEFAULT 'JPY'::character varying,
    discount_rate numeric(5,4) NOT NULL,
    rou_asset_value numeric(18,2) NOT NULL,
    lease_liability_value numeric(18,2) NOT NULL,
    residual_value numeric(18,2) DEFAULT 0,
    start_date date,
    end_date date,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    created_by character varying(50),
    termination_date date,
    termination_penalty numeric(18,0) DEFAULT 0,
    status character varying(20) DEFAULT 'ACTIVE'::character varying,
    is_determination_fixed boolean DEFAULT false
);

--
-- Name: TABLE t_accounting_unit; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_accounting_unit IS '拡張: 会計単位ヘッダ（新リース会計用）';

--
-- Name: COLUMN t_accounting_unit.termination_date; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_accounting_unit.termination_date IS '中途解約日';

--
-- Name: COLUMN t_accounting_unit.termination_penalty; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_accounting_unit.termination_penalty IS '中途解約違約金 (リース解約損)';

--
-- Name: COLUMN t_accounting_unit.status; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_accounting_unit.status IS '契約ステータス (ACTIVE/TERMINATED)';

--
-- Name: COLUMN t_accounting_unit.is_determination_fixed; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_accounting_unit.is_determination_fixed IS '判定結果固定フラグ: TRUEの場合、自動再判定による上書きを防止(照井様指摘対応)';


CREATE TABLE public.t_amortization_schedule (
    schedule_id integer NOT NULL,
    act_unit_id integer NOT NULL,
    payment_count integer NOT NULL,
    payment_date date NOT NULL,
    payment_amount numeric(18,2) NOT NULL,
    interest_expense numeric(18,2) NOT NULL,
    principal_repayment numeric(18,2) NOT NULL,
    liability_balance numeric(18,2) NOT NULL,
    rou_depreciation numeric(18,2) DEFAULT 0,
    rou_book_value numeric(18,2) DEFAULT 0
);

--
-- Name: TABLE t_amortization_schedule; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_amortization_schedule IS '拡張: 利息法償却予定表';


CREATE TABLE public.t_audit_log (
    log_id bigint NOT NULL,
    table_name character varying(50),
    record_id character varying(50),
    operation_type character varying(10),
    operated_by character varying(50),
    operated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    old_value jsonb,
    new_value jsonb
);

--
-- Name: TABLE t_audit_log; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_audit_log IS '拡張: 操作監査ログ';


CREATE TABLE public.t_db_version (
    db_version character varying(30) NOT NULL
);

--
-- Name: TABLE t_db_version; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_db_version IS 'DBバージョン';


CREATE TABLE public.t_holiday (
    id smallint NOT NULL,
    h_date timestamp without time zone,
    biko character varying(255)
);

--
-- Name: TABLE t_holiday; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_holiday IS '休日';


CREATE TABLE public.t_journal_setting (
    setting_key character varying(50) NOT NULL,
    setting_value character varying(100) NOT NULL,
    description character varying(200)
);

--
-- Name: TABLE t_journal_setting; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_journal_setting IS '拡張: 仕訳生成用設定マスタ';


CREATE TABLE public.t_kari_ritu (
    kari_ritu_id integer NOT NULL,
    start_dt timestamp without time zone,
    kari_ritu double precision,
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE t_kari_ritu; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_kari_ritu IS '仮リース料率';


CREATE TABLE public.t_kykbnj_seq (
    key character varying(30) NOT NULL,
    nextval double precision,
    biko character varying(50)
);

--
-- Name: TABLE t_kykbnj_seq; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_kykbnj_seq IS '契約番号採番';


CREATE TABLE public.t_mstk (
    mstk_id integer NOT NULL,
    mst_name character varying(50),
    update_dt timestamp without time zone,
    kind smallint,
    local_name character varying(50),
    pkeys character varying(255),
    compfld character varying(30),
    biko character varying(30)
);

--
-- Name: TABLE t_mstk; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_mstk IS 'マスタチェック';


CREATE TABLE public.t_opt (
    slog boolean DEFAULT false,
    ulog boolean DEFAULT false,
    recopt boolean DEFAULT false,
    cnvlog boolean DEFAULT false,
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    opt_id integer NOT NULL
);

--
-- Name: TABLE t_opt; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_opt IS 'オプション設定';


CREATE TABLE public.t_req (
    req_id integer NOT NULL,
    req_nm character varying(40),
    req_dt timestamp without time zone,
    biko character varying(30)
);

--
-- Name: TABLE t_req; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_req IS 'その他のデータ：処理要求ﾃｰﾌﾞﾙ';

--
-- Name: COLUMN t_req.req_id; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_req.req_id IS 'N(9) 処理ID';

--
-- Name: COLUMN t_req.req_nm; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_req.req_nm IS 'V2(40) 処理名称';

--
-- Name: COLUMN t_req.req_dt; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_req.req_dt IS 'D 要求日時';

--
-- Name: COLUMN t_req.biko; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_req.biko IS 'V2(30) 備考';


CREATE TABLE public.t_seq (
    field_nm character varying(30) NOT NULL,
    table_nm character varying(30),
    nextval double precision
);

--
-- Name: TABLE t_seq; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_seq IS '採番管理';


CREATE TABLE public.t_settei (
    settei_id integer NOT NULL,
    settei_nm character varying(30) NOT NULL,
    settei_nm_jpn character varying(50) NOT NULL,
    settei_type integer NOT NULL,
    val_text character varying(100),
    val_number double precision,
    val_datetime timestamp without time zone,
    biko character varying(100)
);

--
-- Name: TABLE t_settei; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_settei IS 'その他のデータ：共有初期設定ﾃｰﾌﾞﾙ★2005/02/14';

--
-- Name: COLUMN t_settei.settei_id; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.settei_id IS 'N(9) 設定項目ID';

--
-- Name: COLUMN t_settei.settei_nm; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.settei_nm IS 'V2(30) 設定項目名';

--
-- Name: COLUMN t_settei.settei_nm_jpn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.settei_nm_jpn IS 'V2(50) 設定項目名(日本語)';

--
-- Name: COLUMN t_settei.settei_type; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.settei_type IS 'N(2) ﾀｲﾌﾟ';

--
-- Name: COLUMN t_settei.val_text; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.val_text IS 'V2(100) 設定値(文字列)';

--
-- Name: COLUMN t_settei.val_number; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.val_number IS 'N(15,8) 設定値(数値)';

--
-- Name: COLUMN t_settei.val_datetime; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.val_datetime IS 'D 設定値(日付時刻)';

--
-- Name: COLUMN t_settei.biko; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_settei.biko IS 'V2(100) 備考';


CREATE TABLE public.t_shwak_d (
    shwak_id integer NOT NULL,
    process_date date NOT NULL,
    kykh_id integer NOT NULL,
    act_unit_id integer,
    debit_acct_cd character varying(20),
    debit_acct_nm character varying(100),
    debit_tax_cd character varying(10),
    debit_amount numeric(18,2) DEFAULT 0,
    credit_acct_cd character varying(20),
    credit_acct_nm character varying(100),
    credit_tax_cd character varying(10),
    credit_amount numeric(18,2) DEFAULT 0,
    description character varying(255),
    entry_type character varying(20) DEFAULT 'NORMAL'::character varying,
    is_exported boolean DEFAULT false,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);

--
-- Name: TABLE t_shwak_d; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_shwak_d IS '仕訳生成ワーク: 月次処理および遡及修正(Catch-up)仕訳の一時保管場所';

--
-- Name: COLUMN t_shwak_d.entry_type; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.t_shwak_d.entry_type IS '仕訳区分: NORMAL=通常仕訳, CATCHUP=遡及修正仕訳(過年度修正損益など)';


CREATE TABLE public.t_swk_nm (
    swk_kbn smallint NOT NULL,
    xxxxcd character varying(12),
    swk_nm character varying(50),
    swk_ryk character varying(20),
    xxxxnm character varying(50)
);

--
-- Name: TABLE t_swk_nm; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_swk_nm IS '仕訳名称';


CREATE TABLE public.t_system (
    ap_version character varying(30) NOT NULL,
    customizetype character varying(20),
    customizeno smallint,
    url character varying(50),
    mail character varying(50),
    shipday timestamp without time zone,
    tcomment character varying(255),
    file_version_mdld character varying(30),
    file_version_work character varying(30),
    file_version_excel character varying(30)
);

--
-- Name: TABLE t_system; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_system IS 'システム情報';


CREATE TABLE public.t_szei_kmk (
    zritu double precision NOT NULL,
    kind smallint NOT NULL,
    hreikbn smallint NOT NULL,
    leakbn_id smallint NOT NULL,
    kjkbn_id smallint NOT NULL,
    szei_kjkbn_id smallint NOT NULL,
    ptn_cd1 character varying(12),
    ptn_nm1 character varying(40),
    ptn_cd2 character varying(12),
    ptn_nm2 character varying(40),
    ptn_cd3 character varying(12),
    ptn_nm3 character varying(40),
    ptn_cd4 character varying(12),
    ptn_nm4 character varying(40),
    zkmk_cd1 character varying(20),
    zhkmk_nm1 character varying(40),
    zkmk_cd2 character varying(20),
    zhkmk_nm2 character varying(40),
    zkmk_cd3 character varying(20),
    zhkmk_nm3 character varying(40)
);

--
-- Name: TABLE t_szei_kmk; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_szei_kmk IS '消費税科目';


CREATE TABLE public.t_zei_kaisei (
    zei_kaisei_id integer NOT NULL,
    teki_dt_from timestamp without time zone,
    teki_dt_to timestamp without time zone,
    zritu double precision,
    kkyak_dt_from timestamp without time zone,
    kkyak_dt_to timestamp without time zone,
    create_id integer,
    create_dt timestamp without time zone,
    update_id integer,
    update_dt timestamp without time zone,
    update_cnt integer DEFAULT 0,
    history_f boolean DEFAULT false
);

--
-- Name: TABLE t_zei_kaisei; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_zei_kaisei IS '税制改正';


-- ============================================================
-- 計算テーブル (tc_*)
-- ============================================================

CREATE TABLE public.tc_hrel (
    ptn_cd1 character varying(12) NOT NULL,
    ptn_nm1 character varying(40),
    ptn_cd2 character varying(12) NOT NULL,
    ptn_nm2 character varying(40),
    ptn_cd3 character varying(12),
    ptn_nm3 character varying(40),
    ptn_cd4 character varying(12),
    ptn_nm4 character varying(40),
    kmk_cd1 character varying(20),
    kmk_nm1 character varying(80),
    kmk_cd2 character varying(20),
    kmk_nm2 character varying(80),
    kmk_cd3 character varying(20),
    kmk_nm3 character varying(80),
    kmk_cd4 character varying(20),
    kmk_nm4 character varying(80),
    kmk_cd5 character varying(20),
    kmk_nm5 character varying(80),
    kmk_cd6 character varying(20),
    kmk_nm6 character varying(80),
    kmk_cd7 character varying(20),
    kmk_nm7 character varying(80),
    kmk_cd8 character varying(20),
    kmk_nm8 character varying(80),
    kmk_cd9 character varying(20),
    kmk_nm9 character varying(80),
    kmk_cd10 character varying(20),
    kmk_nm10 character varying(80),
    kmk_cd11 character varying(20),
    kmk_nm11 character varying(80),
    kmk_cd12 character varying(20),
    kmk_nm12 character varying(80),
    kmk_cd13 character varying(20),
    kmk_nm13 character varying(80),
    kmk_cd14 character varying(20),
    kmk_nm14 character varying(80),
    kmk_cd15 character varying(20),
    kmk_nm15 character varying(80)
);

--
-- Name: TABLE tc_hrel; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.tc_hrel IS '配賦連動';


CREATE TABLE public.tc_rec_shri (
    ktmg smallint,
    shri_tuki timestamp without time zone,
    shri_dt timestamp without time zone,
    shri_r_dt timestamp without time zone,
    xxxx1id integer,
    xxxx1cd character varying(12),
    xxxx1nm character varying(80),
    xxxx2id integer,
    xxxx2cd character varying(12),
    xxxx2nm character varying(80),
    xxxx3id integer,
    xxxx3cd character varying(12),
    xxxx3nm character varying(80),
    xxxx4id integer,
    xxxx4cd character varying(12),
    xxxx4nm character varying(80),
    xxxx5id integer,
    xxxx5cd character varying(12),
    xxxx5nm character varying(80),
    siwakekbn smallint,
    sum_zkomi_toki double precision,
    sum_znuki_toki double precision,
    sum_zei_toki double precision,
    cnt_reccnt integer,
    kihyo_dt timestamp without time zone,
    output_dt timestamp without time zone,
    sum_zkomi_toki_hiyo double precision,
    sum_zkomi_toki_sisan double precision,
    "計上日" timestamp without time zone,
    rec_shri_id integer NOT NULL
);

--
-- Name: TABLE tc_rec_shri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.tc_rec_shri IS '支払実績';


CREATE TABLE public.tc_reg_report (
    line_id integer NOT NULL,
    address1 character varying(40),
    address2 character varying(40),
    address3 character varying(40),
    address4 character varying(40),
    address5 character varying(40),
    attachment1 character varying(40),
    attachment2 character varying(40),
    attachment3 character varying(40),
    attachment4 character varying(40),
    attachment5 character varying(40),
    attachment6 character varying(40),
    attachment7 character varying(40),
    attachment8 character varying(40),
    attachment9 character varying(40),
    attachment10 character varying(40),
    attachment11 character varying(40),
    attachment12 character varying(40),
    attachment13 character varying(40),
    attachment14 character varying(40),
    attachment15 character varying(40),
    attachment16 character varying(40),
    attachment17 character varying(40),
    attachment18 character varying(40),
    attachment19 character varying(40),
    attachment20 character varying(40)
);

--
-- Name: TABLE tc_reg_report; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.tc_reg_report IS 'その他のデータ：登録届指示ﾃｰﾌﾞﾙ◆2008-10-17';

--
-- Name: COLUMN tc_reg_report.line_id; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.line_id IS 'N(1) 行ID';

--
-- Name: COLUMN tc_reg_report.address1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.address1 IS 'V2(40) 届先１';

--
-- Name: COLUMN tc_reg_report.address2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.address2 IS 'V2(40) 届先２';

--
-- Name: COLUMN tc_reg_report.address3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.address3 IS 'V2(40) 届先３ ◆予備';

--
-- Name: COLUMN tc_reg_report.address4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.address4 IS 'V2(40) 届先４ ◆予備';

--
-- Name: COLUMN tc_reg_report.address5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.address5 IS 'V2(40) 届先５ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment1 IS 'V2(40) 添付書類１';

--
-- Name: COLUMN tc_reg_report.attachment2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment2 IS 'V2(40) 添付書類２';

--
-- Name: COLUMN tc_reg_report.attachment3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment3 IS 'V2(40) 添付書類３';

--
-- Name: COLUMN tc_reg_report.attachment4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment4 IS 'V2(40) 添付書類４';

--
-- Name: COLUMN tc_reg_report.attachment5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment5 IS 'V2(40) 添付書類５';

--
-- Name: COLUMN tc_reg_report.attachment6; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment6 IS 'V2(40) 添付書類６';

--
-- Name: COLUMN tc_reg_report.attachment7; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment7 IS 'V2(40) 添付書類７';

--
-- Name: COLUMN tc_reg_report.attachment8; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment8 IS 'V2(40) 添付書類８';

--
-- Name: COLUMN tc_reg_report.attachment9; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment9 IS 'V2(40) 添付書類９';

--
-- Name: COLUMN tc_reg_report.attachment10; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment10 IS 'V2(40) 添付書類１０';

--
-- Name: COLUMN tc_reg_report.attachment11; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment11 IS 'V2(40) 添付書類１１';

--
-- Name: COLUMN tc_reg_report.attachment12; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment12 IS 'V2(40) 添付書類１２';

--
-- Name: COLUMN tc_reg_report.attachment13; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment13 IS 'V2(40) 添付書類１３';

--
-- Name: COLUMN tc_reg_report.attachment14; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment14 IS 'V2(40) 添付書類１４';

--
-- Name: COLUMN tc_reg_report.attachment15; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment15 IS 'V2(40) 添付書類１５ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment16; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment16 IS 'V2(40) 添付書類１６ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment17; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment17 IS 'V2(40) 添付書類１７ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment18; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment18 IS 'V2(40) 添付書類１８ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment19; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment19 IS 'V2(40) 添付書類１９ ◆予備';

--
-- Name: COLUMN tc_reg_report.attachment20; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_reg_report.attachment20 IS 'V2(40) 添付書類２０ ◆予備';


CREATE TABLE public.tc_swk_def_com (
    swk_kbn integer NOT NULL,
    xxxxcd character varying(12),
    getsudo timestamp without time zone,
    seikyu_no integer,
    data_kbn character varying(2),
    corp_cd character varying(3),
    hassei_basho character varying(2),
    kanjo_basho character varying(2),
    kessai_kbn character varying(1),
    keijo_dt timestamp without time zone,
    d_kamoku character varying(4),
    d_kessai_basho character varying(2),
    d_xx1cd character varying(12),
    d_xx2cd character varying(12),
    d_xx3cd character varying(12),
    d_xx4cd character varying(12),
    d_xx5cd character varying(12),
    d_kbn1 character varying(2),
    d_kbn2 character varying(2),
    d_kbn3 character varying(2),
    d_kbn4 character varying(2),
    d_kbn5 character varying(2),
    d_kingaku double precision,
    c_kamoku character varying(4),
    c_kessai_basho character varying(2),
    c_xx1cd character varying(12),
    c_xx2cd character varying(12),
    c_xx3cd character varying(12),
    c_xx4cd character varying(12),
    c_xx5cd character varying(12),
    c_kbn1 character varying(2),
    c_kbn2 character varying(2),
    c_kbn3 character varying(2),
    c_kbn4 character varying(2),
    c_kbn5 character varying(2),
    c_kingaku double precision,
    tekiyo_cd1 character varying(3),
    tekiyo_cd2 character varying(3),
    tekiyo_cd3 character varying(3),
    zei double precision,
    zritu double precision,
    input_kbn character varying(12)
);

--
-- Name: TABLE tc_swk_def_com; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.tc_swk_def_com IS 'その他のデータ：仕訳固定値設定ﾃｰﾌﾞﾙ';

--
-- Name: COLUMN tc_swk_def_com.swk_kbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.swk_kbn IS 'N(2) 仕訳区分 1:支払(費用/預金) 2:未払 3:支払(未払金/預金) 4:振替 5:減損損失 6:減損勘定取崩';

--
-- Name: COLUMN tc_swk_def_com.xxxxcd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.xxxxcd IS 'V2(12) 予備コード';

--
-- Name: COLUMN tc_swk_def_com.getsudo; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.getsudo IS 'D 処理年月';

--
-- Name: COLUMN tc_swk_def_com.seikyu_no; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.seikyu_no IS 'N(7) 請求NO';

--
-- Name: COLUMN tc_swk_def_com.data_kbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.data_kbn IS 'V2(2) ﾃﾞｰﾀ区分';

--
-- Name: COLUMN tc_swk_def_com.corp_cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.corp_cd IS 'V2(3) 会社ｺｰﾄﾞ';

--
-- Name: COLUMN tc_swk_def_com.hassei_basho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.hassei_basho IS 'V2(2) 発生場所';

--
-- Name: COLUMN tc_swk_def_com.kanjo_basho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.kanjo_basho IS 'V2(2) 勘定場所';

--
-- Name: COLUMN tc_swk_def_com.kessai_kbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.kessai_kbn IS 'V2(1) 決済区分';

--
-- Name: COLUMN tc_swk_def_com.keijo_dt; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.keijo_dt IS 'D 処理年月日';

--
-- Name: COLUMN tc_swk_def_com.d_kamoku; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kamoku IS 'V2(4) 借)勘定科目';

--
-- Name: COLUMN tc_swk_def_com.d_kessai_basho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kessai_basho IS 'V2(2) 借)決済場所';

--
-- Name: COLUMN tc_swk_def_com.d_xx1cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_xx1cd IS 'V2(12) 借)コード１';

--
-- Name: COLUMN tc_swk_def_com.d_xx2cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_xx2cd IS 'V2(12) 借)コード２';

--
-- Name: COLUMN tc_swk_def_com.d_xx3cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_xx3cd IS 'V2(12) 借)コード３';

--
-- Name: COLUMN tc_swk_def_com.d_xx4cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_xx4cd IS 'V2(12) 借)コード４';

--
-- Name: COLUMN tc_swk_def_com.d_xx5cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_xx5cd IS 'V2(12) 借)コード５';

--
-- Name: COLUMN tc_swk_def_com.d_kbn1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kbn1 IS '借)区分１';

--
-- Name: COLUMN tc_swk_def_com.d_kbn2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kbn2 IS '借)区分２';

--
-- Name: COLUMN tc_swk_def_com.d_kbn3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kbn3 IS '借)区分３';

--
-- Name: COLUMN tc_swk_def_com.d_kbn4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kbn4 IS '借)区分４';

--
-- Name: COLUMN tc_swk_def_com.d_kbn5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kbn5 IS '借)区分５';

--
-- Name: COLUMN tc_swk_def_com.d_kingaku; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.d_kingaku IS 'N(15) 借)金額';

--
-- Name: COLUMN tc_swk_def_com.c_kamoku; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kamoku IS 'V2(4) 貸)勘定科目';

--
-- Name: COLUMN tc_swk_def_com.c_kessai_basho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kessai_basho IS 'V2(2) 貸)決済場所';

--
-- Name: COLUMN tc_swk_def_com.c_xx1cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_xx1cd IS 'V2(12) 貸)コード１';

--
-- Name: COLUMN tc_swk_def_com.c_xx2cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_xx2cd IS 'V2(12) 貸)コード２';

--
-- Name: COLUMN tc_swk_def_com.c_xx3cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_xx3cd IS 'V2(12) 貸)コード３';

--
-- Name: COLUMN tc_swk_def_com.c_xx4cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_xx4cd IS 'V2(12) 貸)コード４';

--
-- Name: COLUMN tc_swk_def_com.c_xx5cd; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_xx5cd IS 'V2(12) 貸)コード５';

--
-- Name: COLUMN tc_swk_def_com.c_kbn1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kbn1 IS '貸)区分１';

--
-- Name: COLUMN tc_swk_def_com.c_kbn2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kbn2 IS '貸)区分２';

--
-- Name: COLUMN tc_swk_def_com.c_kbn3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kbn3 IS '貸)区分３';

--
-- Name: COLUMN tc_swk_def_com.c_kbn4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kbn4 IS '貸)区分４';

--
-- Name: COLUMN tc_swk_def_com.c_kbn5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kbn5 IS '貸)区分５';

--
-- Name: COLUMN tc_swk_def_com.c_kingaku; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.c_kingaku IS 'N(15) 貸)金額';

--
-- Name: COLUMN tc_swk_def_com.tekiyo_cd1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.tekiyo_cd1 IS 'V2(3) 摘要ｺｰﾄﾞ１';

--
-- Name: COLUMN tc_swk_def_com.tekiyo_cd2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.tekiyo_cd2 IS 'V2(3) 摘要ｺｰﾄﾞ２';

--
-- Name: COLUMN tc_swk_def_com.tekiyo_cd3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.tekiyo_cd3 IS 'V2(3) 摘要ｺｰﾄﾞ３';

--
-- Name: COLUMN tc_swk_def_com.zei; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.zei IS 'N(15) 税額';

--
-- Name: COLUMN tc_swk_def_com.zritu; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.zritu IS 'N(7,6) 税率';

--
-- Name: COLUMN tc_swk_def_com.input_kbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_def_com.input_kbn IS 'V2(12) 入力区分';


CREATE TABLE public.tc_swk_settei (
    xxxxcd1 character varying(20),
    xxxxnm1 character varying(80),
    xxxxcd2 character varying(20),
    xxxxnm2 character varying(80),
    xxxxcd3 character varying(20),
    xxxxnm3 character varying(80),
    xxxxcd4 character varying(20),
    xxxxnm4 character varying(80),
    xxxxcd5 character varying(20),
    xxxxnm5 character varying(80),
    xxxxcd6 character varying(20),
    xxxxnm6 character varying(80),
    xxxxcd7 character varying(20),
    xxxxnm7 character varying(80),
    xxxxcd8 character varying(20),
    xxxxnm8 character varying(80),
    xxxxcd9 character varying(20),
    xxxxnm9 character varying(80),
    xxxxcd10 character varying(20),
    xxxxnm10 character varying(80),
    xxxxcd11 character varying(20),
    xxxxnm11 character varying(80),
    xxxxcd12 character varying(20),
    xxxxnm12 character varying(80),
    xxxxcd13 character varying(20),
    xxxxnm13 character varying(80),
    xxxxcd14 character varying(20),
    xxxxnm14 character varying(80),
    xxxxcd15 character varying(20),
    xxxxnm15 character varying(80),
    text1 character varying(100),
    text2 character varying(100),
    text3 character varying(100),
    text4 character varying(100),
    text5 character varying(100),
    datetime1 timestamp without time zone,
    datetime2 timestamp without time zone,
    datetime3 timestamp without time zone,
    datetime4 timestamp without time zone,
    datetime5 timestamp without time zone,
    flag1 boolean NOT NULL,
    flag2 boolean NOT NULL,
    flag3 boolean NOT NULL,
    flag4 boolean NOT NULL,
    flag5 boolean NOT NULL,
    kingaku1 double precision,
    kingaku2 double precision,
    kingaku3 double precision,
    kingaku4 double precision,
    kingaku5 double precision,
    swk_settei_id integer NOT NULL
);

--
-- Name: TABLE tc_swk_settei; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.tc_swk_settei IS 'その他のデータ：仕訳設定ﾃｰﾌﾞﾙ（2011/2/18時点ではMYCOMのみ）';

--
-- Name: COLUMN tc_swk_settei.xxxxcd1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd1 IS 'V2(20) コード1  MYCOM:会社CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm1 IS 'V2(80) 名称1';

--
-- Name: COLUMN tc_swk_settei.xxxxcd2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd2 IS 'V2(20) コード2  MYCOM:会社区分CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm2 IS 'V2(80) 名称2';

--
-- Name: COLUMN tc_swk_settei.xxxxcd3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd3 IS 'V2(20) コード3  MYCOM:起票部門CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm3 IS 'V2(80) 名称3';

--
-- Name: COLUMN tc_swk_settei.xxxxcd4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd4 IS 'V2(20) コード4  MYCOM:計上部門CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm4 IS 'V2(80) 名称4';

--
-- Name: COLUMN tc_swk_settei.xxxxcd5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd5 IS 'V2(20) コード5  MYCOM:仮払勘定CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm5 IS 'V2(80) 名称5';

--
-- Name: COLUMN tc_swk_settei.xxxxcd6; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd6 IS 'V2(20) コード6  MYCOM:仮払内訳CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm6; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm6 IS 'V2(80) 名称6';

--
-- Name: COLUMN tc_swk_settei.xxxxcd7; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd7 IS 'V2(20) コード7  MYCOM:関係会社未払勘定CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm7; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm7 IS 'V2(80) 名称7';

--
-- Name: COLUMN tc_swk_settei.xxxxcd8; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd8 IS 'V2(20) コード8  MYCOM:関係会社未払内訳CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm8; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm8 IS 'V2(80) 名称8';

--
-- Name: COLUMN tc_swk_settei.xxxxcd9; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd9 IS 'V2(20) コード9  MYCOM:関係会社立替勘定CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm9; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm9 IS 'V2(80) 名称9';

--
-- Name: COLUMN tc_swk_settei.xxxxcd10; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd10 IS 'V2(20) コード10  MYCOM:関係会社立替内訳CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm10; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm10 IS 'V2(80) 名称10';

--
-- Name: COLUMN tc_swk_settei.xxxxcd11; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd11 IS 'V2(20) コード11  MYCOM:経過勘定CD';

--
-- Name: COLUMN tc_swk_settei.xxxxnm11; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm11 IS 'V2(80) 名称11';

--
-- Name: COLUMN tc_swk_settei.xxxxcd12; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd12 IS 'V2(20) コード12';

--
-- Name: COLUMN tc_swk_settei.xxxxnm12; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm12 IS 'V2(80) 名称12';

--
-- Name: COLUMN tc_swk_settei.xxxxcd13; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd13 IS 'V2(20) コード13';

--
-- Name: COLUMN tc_swk_settei.xxxxnm13; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm13 IS 'V2(80) 名称13';

--
-- Name: COLUMN tc_swk_settei.xxxxcd14; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd14 IS 'V2(20) コード14';

--
-- Name: COLUMN tc_swk_settei.xxxxnm14; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm14 IS 'V2(80) 名称14';

--
-- Name: COLUMN tc_swk_settei.xxxxcd15; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxcd15 IS 'V2(20) コード15';

--
-- Name: COLUMN tc_swk_settei.xxxxnm15; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.xxxxnm15 IS 'V2(80) 名称15';

--
-- Name: COLUMN tc_swk_settei.text1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.text1 IS 'V2(100) テキスト1  MYCOM:伝票摘要';

--
-- Name: COLUMN tc_swk_settei.text2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.text2 IS 'V2(100) テキスト2  MYCOM:明細摘要';

--
-- Name: COLUMN tc_swk_settei.text3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.text3 IS 'V2(100) テキスト3';

--
-- Name: COLUMN tc_swk_settei.text4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.text4 IS 'V2(100) テキスト4';

--
-- Name: COLUMN tc_swk_settei.text5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.text5 IS 'V2(100) テキスト6';

--
-- Name: COLUMN tc_swk_settei.datetime1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.datetime1 IS 'D 日付時刻1  MYCOM:前回計上月';

--
-- Name: COLUMN tc_swk_settei.datetime2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.datetime2 IS 'D 日付時刻2  MYCOM:前回日時';

--
-- Name: COLUMN tc_swk_settei.datetime3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.datetime3 IS 'D 日付時刻3';

--
-- Name: COLUMN tc_swk_settei.datetime4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.datetime4 IS 'D 日付時刻4';

--
-- Name: COLUMN tc_swk_settei.datetime5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.datetime5 IS 'D 日付時刻5';

--
-- Name: COLUMN tc_swk_settei.flag1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.flag1 IS 'N(1) フラグ1  MYCOM:有効F';

--
-- Name: COLUMN tc_swk_settei.flag2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.flag2 IS 'N(1) フラグ2';

--
-- Name: COLUMN tc_swk_settei.flag3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.flag3 IS 'N(1) フラグ3';

--
-- Name: COLUMN tc_swk_settei.flag4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.flag4 IS 'N(1) フラグ4';

--
-- Name: COLUMN tc_swk_settei.flag5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.flag5 IS 'N(1) フラグ5';

--
-- Name: COLUMN tc_swk_settei.kingaku1; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.kingaku1 IS 'N(15) 金額1';

--
-- Name: COLUMN tc_swk_settei.kingaku2; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.kingaku2 IS 'N(15) 金額2';

--
-- Name: COLUMN tc_swk_settei.kingaku3; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.kingaku3 IS 'N(15) 金額3';

--
-- Name: COLUMN tc_swk_settei.kingaku4; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.kingaku4 IS 'N(15) 金額4';

--
-- Name: COLUMN tc_swk_settei.kingaku5; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON COLUMN public.tc_swk_settei.kingaku5 IS 'N(15) 金額5';


-- ============================================================
-- ワークテーブル (tw_*)
-- ============================================================

CREATE TABLE public.tw_m_user (
    user_id integer NOT NULL,
    user_name character varying(100),
    user_kana character varying(100),
    create_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    update_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


-- ============================================================
-- シーケンス (SEQUENCE)
-- ============================================================

CREATE SEQUENCE public.t_accounting_unit_act_unit_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: t_accounting_unit_act_unit_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.t_accounting_unit_act_unit_id_seq OWNED BY public.t_accounting_unit.act_unit_id;


CREATE SEQUENCE public.t_amortization_schedule_schedule_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: t_amortization_schedule_schedule_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.t_amortization_schedule_schedule_id_seq OWNED BY public.t_amortization_schedule.schedule_id;


CREATE SEQUENCE public.t_audit_log_log_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: t_audit_log_log_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.t_audit_log_log_id_seq OWNED BY public.t_audit_log.log_id;


CREATE SEQUENCE public.t_opt_opt_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: t_opt_opt_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.t_opt_opt_id_seq OWNED BY public.t_opt.opt_id;


CREATE SEQUENCE public.t_shwak_d_shwak_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: t_shwak_d_shwak_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.t_shwak_d_shwak_id_seq OWNED BY public.t_shwak_d.shwak_id;


CREATE SEQUENCE public.tc_rec_shri_rec_shri_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: tc_rec_shri_rec_shri_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tc_rec_shri_rec_shri_id_seq OWNED BY public.tc_rec_shri.rec_shri_id;


CREATE SEQUENCE public.tc_swk_settei_swk_settei_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

--
-- Name: tc_swk_settei_swk_settei_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.tc_swk_settei_swk_settei_id_seq OWNED BY public.tc_swk_settei.swk_settei_id;


-- ============================================================
-- デフォルト値 (SEQUENCE DEFAULT)
-- ============================================================

--
-- Name: t_accounting_unit act_unit_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_accounting_unit ALTER COLUMN act_unit_id SET DEFAULT nextval('public.t_accounting_unit_act_unit_id_seq'::regclass);


--
-- Name: t_amortization_schedule schedule_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_amortization_schedule ALTER COLUMN schedule_id SET DEFAULT nextval('public.t_amortization_schedule_schedule_id_seq'::regclass);


--
-- Name: t_audit_log log_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_audit_log ALTER COLUMN log_id SET DEFAULT nextval('public.t_audit_log_log_id_seq'::regclass);


--
-- Name: t_opt opt_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_opt ALTER COLUMN opt_id SET DEFAULT nextval('public.t_opt_opt_id_seq'::regclass);


--
-- Name: t_shwak_d shwak_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.t_shwak_d ALTER COLUMN shwak_id SET DEFAULT nextval('public.t_shwak_d_shwak_id_seq'::regclass);


--
-- Name: tc_rec_shri rec_shri_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_rec_shri ALTER COLUMN rec_shri_id SET DEFAULT nextval('public.tc_rec_shri_rec_shri_id_seq'::regclass);


--
-- Name: tc_swk_settei swk_settei_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_swk_settei ALTER COLUMN swk_settei_id SET DEFAULT nextval('public.tc_swk_settei_swk_settei_id_seq'::regclass);


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
