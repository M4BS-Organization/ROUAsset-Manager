-- システム/設定テーブル (t_*): 設定・集計・会計 18テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- トランザクション/設定テーブル (t_*)
-- ============================================================

DROP TABLE IF EXISTS public.t_accounting_unit CASCADE;

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


DROP TABLE IF EXISTS public.t_amortization_schedule CASCADE;

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


DROP TABLE IF EXISTS public.t_audit_log CASCADE;

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


DROP TABLE IF EXISTS public.t_db_version CASCADE;

CREATE TABLE public.t_db_version (
    db_version character varying(30) NOT NULL
);

--
-- Name: TABLE t_db_version; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_db_version IS 'DBバージョン';


DROP TABLE IF EXISTS public.t_holiday CASCADE;

CREATE TABLE public.t_holiday (
    id smallint NOT NULL,
    h_date timestamp without time zone,
    biko character varying(255)
);

--
-- Name: TABLE t_holiday; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_holiday IS '休日';


DROP TABLE IF EXISTS public.t_journal_setting CASCADE;

CREATE TABLE public.t_journal_setting (
    setting_key character varying(50) NOT NULL,
    setting_value character varying(100) NOT NULL,
    description character varying(200)
);

--
-- Name: TABLE t_journal_setting; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_journal_setting IS '拡張: 仕訳生成用設定マスタ';


DROP TABLE IF EXISTS public.t_kari_ritu CASCADE;

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


DROP TABLE IF EXISTS public.t_kykbnj_seq CASCADE;

CREATE TABLE public.t_kykbnj_seq (
    key character varying(30) NOT NULL,
    nextval double precision,
    biko character varying(50)
);

--
-- Name: TABLE t_kykbnj_seq; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_kykbnj_seq IS '契約番号採番';


DROP TABLE IF EXISTS public.t_mstk CASCADE;

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


DROP TABLE IF EXISTS public.t_opt CASCADE;

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


DROP TABLE IF EXISTS public.t_req CASCADE;

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


DROP TABLE IF EXISTS public.t_seq CASCADE;

CREATE TABLE public.t_seq (
    field_nm character varying(30) NOT NULL,
    table_nm character varying(30),
    nextval double precision
);

--
-- Name: TABLE t_seq; Type: COMMENT; Schema: public; Owner: -
--

COMMENT ON TABLE public.t_seq IS '採番管理';


DROP TABLE IF EXISTS public.t_settei CASCADE;

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


DROP TABLE IF EXISTS public.t_shwak_d CASCADE;

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


DROP TABLE IF EXISTS public.t_swk_nm CASCADE;

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


DROP TABLE IF EXISTS public.t_system CASCADE;

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


DROP TABLE IF EXISTS public.t_szei_kmk CASCADE;

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


DROP TABLE IF EXISTS public.t_zei_kaisei CASCADE;

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
-- シーケンス (t_* テーブル用)
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

COMMIT;
