-- コードテーブル (c_*): 区分定義 10テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- コードテーブル (c_*)
-- ============================================================

DROP TABLE IF EXISTS public.c_chu_hnti CASCADE;

CREATE TABLE public.c_chu_hnti (
    chu_hnti_id smallint NOT NULL,
    chu_hnti_nm character varying(100)
);

--
-- Name: TABLE c_chu_hnti; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_chu_hnti IS '注記単位';


DROP TABLE IF EXISTS public.c_chuum CASCADE;

CREATE TABLE public.c_chuum (
    chuum_id smallint NOT NULL,
    chuum_nm character varying(10)
);

--
-- Name: TABLE c_chuum; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_chuum IS '注記有無';


DROP TABLE IF EXISTS public.c_kjkbn CASCADE;

CREATE TABLE public.c_kjkbn (
    kjkbn_id smallint NOT NULL,
    kjkbn_nm character varying(10)
);

--
-- Name: TABLE c_kjkbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kjkbn IS '計上区分';


DROP TABLE IF EXISTS public.c_kjtaisyo CASCADE;

CREATE TABLE public.c_kjtaisyo (
    kjtaisyo_id smallint CONSTRAINT c_kjtaisyo_kjkbn_id_not_null NOT NULL,
    kjtaisyo_nm character varying(10)
);

--
-- Name: TABLE c_kjtaisyo; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kjtaisyo IS '計上対象';


DROP TABLE IF EXISTS public.c_kkbn CASCADE;

CREATE TABLE public.c_kkbn (
    kkbn_id smallint NOT NULL,
    kkbn_nm character varying(50)
);

--
-- Name: TABLE c_kkbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_kkbn IS '契約区分';


DROP TABLE IF EXISTS public.c_leakbn CASCADE;

CREATE TABLE public.c_leakbn (
    leakbn_id smallint NOT NULL,
    leakbn_nm character varying(100)
);

--
-- Name: TABLE c_leakbn; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_leakbn IS 'リース区分';


DROP TABLE IF EXISTS public.c_rcalc CASCADE;

CREATE TABLE public.c_rcalc (
    rcalc_id smallint NOT NULL,
    rcalc_nm character varying(10)
);

--
-- Name: TABLE c_rcalc; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_rcalc IS '再計算区分';


DROP TABLE IF EXISTS public.c_settei_idfld CASCADE;

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


DROP TABLE IF EXISTS public.c_skyak_ho CASCADE;

CREATE TABLE public.c_skyak_ho (
    skyak_ho_id smallint NOT NULL,
    skyak_ho_nm character varying(10)
);

--
-- Name: TABLE c_skyak_ho; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.c_skyak_ho IS '償却方法';


DROP TABLE IF EXISTS public.c_szei_kjkbn CASCADE;

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

COMMIT;
