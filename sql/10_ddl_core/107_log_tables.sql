-- ログテーブル (l_*): 操作・更新ログ 3テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- ログテーブル (l_*)
-- ============================================================

DROP TABLE IF EXISTS public.l_bklog CASCADE;

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


DROP TABLE IF EXISTS public.l_slog CASCADE;

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


DROP TABLE IF EXISTS public.l_ulog CASCADE;

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
-- シーケンス: l_slog_slog_no_seq (自動採番)
-- ============================================================

CREATE SEQUENCE IF NOT EXISTS public.l_slog_slog_no_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER TABLE public.l_slog
    ALTER COLUMN slog_no SET DEFAULT nextval('public.l_slog_slog_no_seq');

ALTER SEQUENCE public.l_slog_slog_no_seq
    OWNED BY public.l_slog.slog_no;

-- 既存データがある場合、シーケンスの現在値を最大値に合わせる
SELECT setval('public.l_slog_slog_no_seq',
    COALESCE((SELECT MAX(slog_no) FROM public.l_slog), 0) + 1,
    false);

COMMIT;
