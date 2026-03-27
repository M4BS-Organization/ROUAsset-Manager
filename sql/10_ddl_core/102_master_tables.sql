-- マスタテーブル (m_*): Access移植マスタ 19テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- マスタテーブル (m_*)
-- ============================================================

DROP TABLE IF EXISTS public.m_bcat CASCADE;

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


DROP TABLE IF EXISTS public.m_bkind CASCADE;

CREATE TABLE public.m_bkind (
    bkind_id integer NOT NULL,
    bkind_cd character varying(12),
    bkind_nm character varying(40),
    bkind2_cd character varying(12),
    bkind2_nm character varying(40),
    bkind3_cd character varying(12),
    bkind3_nm character varying(40),
    asset_category_cd character varying(10),  -- 新リース会計: m_asset_category への紐づけ
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


DROP TABLE IF EXISTS public.m_bknri CASCADE;

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


DROP TABLE IF EXISTS public.m_corp CASCADE;

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


DROP TABLE IF EXISTS public.m_genk CASCADE;

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


DROP TABLE IF EXISTS public.m_gsha CASCADE;

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


DROP TABLE IF EXISTS public.m_hkho CASCADE;

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


DROP TABLE IF EXISTS public.m_hkmk CASCADE;

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


DROP TABLE IF EXISTS public.m_kknri CASCADE;

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


DROP TABLE IF EXISTS public.m_koza CASCADE;

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


DROP TABLE IF EXISTS public.m_lcpt CASCADE;

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


DROP TABLE IF EXISTS public.m_mcpt CASCADE;

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


DROP TABLE IF EXISTS public.m_rsrvb1 CASCADE;

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


DROP TABLE IF EXISTS public.m_rsrvh1 CASCADE;

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


DROP TABLE IF EXISTS public.m_rsrvk1 CASCADE;

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


DROP TABLE IF EXISTS public.m_shho CASCADE;

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


DROP TABLE IF EXISTS public.m_skmk CASCADE;

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


DROP TABLE IF EXISTS public.m_skti CASCADE;

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


DROP TABLE IF EXISTS public.m_swptn CASCADE;

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

COMMIT;
