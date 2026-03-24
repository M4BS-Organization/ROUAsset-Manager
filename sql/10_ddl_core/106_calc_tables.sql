-- 計算テーブル (tc_*): 仕訳設定・支払実績・登録届 5テーブル

BEGIN;

SET search_path TO public;

-- ============================================================
-- 計算テーブル (tc_*)
-- ============================================================

DROP TABLE IF EXISTS public.tc_hrel CASCADE;

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


DROP TABLE IF EXISTS public.tc_rec_shri CASCADE;

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


DROP TABLE IF EXISTS public.tc_reg_report CASCADE;

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


DROP TABLE IF EXISTS public.tc_swk_def_com CASCADE;

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


DROP TABLE IF EXISTS public.tc_swk_settei CASCADE;

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
-- シーケンス (tc_* テーブル用)
-- ============================================================

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
-- Name: tc_rec_shri rec_shri_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_rec_shri ALTER COLUMN rec_shri_id SET DEFAULT nextval('public.tc_rec_shri_rec_shri_id_seq'::regclass);


--
-- Name: tc_swk_settei swk_settei_id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tc_swk_settei ALTER COLUMN swk_settei_id SET DEFAULT nextval('public.tc_swk_settei_swk_settei_id_seq'::regclass);

COMMIT;
