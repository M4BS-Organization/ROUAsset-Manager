-- セキュリティテーブル (sec_* + tw_m_user): 認証・権限 5テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- セキュリティテーブル (sec_*)
-- ============================================================

DROP TABLE IF EXISTS public.sec_kngn CASCADE;

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


DROP TABLE IF EXISTS public.sec_kngn_bknri CASCADE;

CREATE TABLE public.sec_kngn_bknri (
    kngn_id integer NOT NULL,
    bknri_id integer NOT NULL,
    access_kind smallint
);

--
-- Name: TABLE sec_kngn_bknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_kngn_bknri IS '権限別物件分類';


DROP TABLE IF EXISTS public.sec_kngn_kknri CASCADE;

CREATE TABLE public.sec_kngn_kknri (
    kngn_id integer NOT NULL,
    kknri_id integer NOT NULL,
    access_kind smallint
);

--
-- Name: TABLE sec_kngn_kknri; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.sec_kngn_kknri IS '権限別契約管理単位';


DROP TABLE IF EXISTS public.sec_user CASCADE;

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
-- ワークテーブル (tw_*)
-- ============================================================

DROP TABLE IF EXISTS public.tw_m_user CASCADE;

CREATE TABLE public.tw_m_user (
    user_id integer NOT NULL,
    user_name character varying(100),
    user_kana character varying(100),
    create_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    update_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);

COMMIT;
