-- =============================================================
-- 仕訳出力標準 設定用ワークテーブル DDL
-- Issue #13: 仕訳出力設定画面 検証・修正
-- =============================================================

-- ---------------------------------------------------------
-- tw_f_仕訳出力標準_設定_swksh（月次支払照合フレックス設定）
-- ---------------------------------------------------------
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_設定_swksh";
CREATE TABLE "tw_f_仕訳出力標準_設定_swksh" (
    id SERIAL PRIMARY KEY,
    -- 計上日種別 (1=入力日, 2=支払日, 3=記帳日)
    swksh_keijo_dt_kind integer DEFAULT 1,
    -- SSN1: 資産リース1（識別）
    swksh_ssn1_out_f boolean DEFAULT false,
    swksh_ssn1_kno_togo_f boolean DEFAULT false,
    swksh_ssn1_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_1d1_cnstcd varchar(30) DEFAULT '', swksh_ssn1_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_ssn1_1d2_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_1d2_cnstcd varchar(30) DEFAULT '', swksh_ssn1_1d2_cnstnm varchar(100) DEFAULT '',
    swksh_ssn1_1d3_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_1d3_cnstcd varchar(30) DEFAULT '', swksh_ssn1_1d3_cnstnm varchar(100) DEFAULT '',
    swksh_ssn1_1d4_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_1d4_cnstcd varchar(30) DEFAULT '', swksh_ssn1_1d4_cnstnm varchar(100) DEFAULT '',
    swksh_ssn1_1d5_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_1d5_cnstcd varchar(30) DEFAULT '', swksh_ssn1_1d5_cnstnm varchar(100) DEFAULT '',
    swksh_ssn1_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn1_2c1_cnstcd varchar(30) DEFAULT '', swksh_ssn1_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN2: 資産リース2
    swksh_ssn2_out_f boolean DEFAULT false,
    swksh_ssn2_kno_togo_f boolean DEFAULT false,
    swksh_ssn2_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn2_1d1_cnstcd varchar(30) DEFAULT '', swksh_ssn2_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_ssn2_1d2_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn2_1d2_cnstcd varchar(30) DEFAULT '', swksh_ssn2_1d2_cnstnm varchar(100) DEFAULT '',
    swksh_ssn2_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn2_2c1_cnstcd varchar(30) DEFAULT '', swksh_ssn2_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN3: 資産リース3（減価償却費のみ）
    swksh_ssn3_out_f boolean DEFAULT false,
    swksh_ssn3_kno_togo_f boolean DEFAULT false,
    swksh_ssn3_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn3_1d1_cnstcd varchar(30) DEFAULT '', swksh_ssn3_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_ssn3_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_ssn3_2c1_cnstcd varchar(30) DEFAULT '', swksh_ssn3_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO1: 費用リース1
    swksh_hiyo1_out_f boolean DEFAULT false,
    swksh_hiyo1_kno_togo_f boolean DEFAULT false,
    swksh_hiyo1_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo1_1d1_cnstcd varchar(30) DEFAULT '', swksh_hiyo1_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo1_1d2_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo1_1d2_cnstcd varchar(30) DEFAULT '', swksh_hiyo1_1d2_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo1_1d3_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo1_1d3_cnstcd varchar(30) DEFAULT '', swksh_hiyo1_1d3_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo1_1d4_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo1_1d4_cnstcd varchar(30) DEFAULT '', swksh_hiyo1_1d4_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo1_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo1_2c1_cnstcd varchar(30) DEFAULT '', swksh_hiyo1_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO2: 費用リース2（減価償却料）
    swksh_hiyo2_out_f boolean DEFAULT false,
    swksh_hiyo2_kno_togo_f boolean DEFAULT false,
    swksh_hiyo2_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo2_1d1_cnstcd varchar(30) DEFAULT '', swksh_hiyo2_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo2_1d2_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo2_1d2_cnstcd varchar(30) DEFAULT '', swksh_hiyo2_1d2_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo2_1d3_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo2_1d3_cnstcd varchar(30) DEFAULT '', swksh_hiyo2_1d3_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo2_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo2_2c1_cnstcd varchar(30) DEFAULT '', swksh_hiyo2_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO3: 費用リース3（月額期間）
    swksh_hiyo3_out_f boolean DEFAULT false,
    swksh_hiyo3_kno_togo_f boolean DEFAULT false,
    swksh_hiyo3_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo3_1d1_cnstcd varchar(30) DEFAULT '', swksh_hiyo3_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo3_1d2_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo3_1d2_cnstcd varchar(30) DEFAULT '', swksh_hiyo3_1d2_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo3_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo3_2c1_cnstcd varchar(30) DEFAULT '', swksh_hiyo3_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO4: 費用リース4（月額期間終了のみ）
    swksh_hiyo4_out_f boolean DEFAULT false,
    swksh_hiyo4_kno_togo_f boolean DEFAULT false,
    swksh_hiyo4_1d1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo4_1d1_cnstcd varchar(30) DEFAULT '', swksh_hiyo4_1d1_cnstnm varchar(100) DEFAULT '',
    swksh_hiyo4_2c1_fldnm varchar(30) DEFAULT 'CONST', swksh_hiyo4_2c1_cnstcd varchar(30) DEFAULT '', swksh_hiyo4_2c1_cnstnm varchar(100) DEFAULT ''
);

-- ---------------------------------------------------------
-- tw_f_仕訳出力標準_設定_swkkj（月次仕訳計上フレックス設定）
-- ---------------------------------------------------------
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_設定_swkkj";
CREATE TABLE "tw_f_仕訳出力標準_設定_swkkj" (
    id SERIAL PRIMARY KEY,
    -- SSN1: 計上1
    swkkj_ssn1_out_f boolean DEFAULT false,
    swkkj_ssn1_kno_togo_f boolean DEFAULT false,
    swkkj_ssn1_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn1_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn1_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn1_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn1_1d2_cnstcd varchar(30) DEFAULT '', swkkj_ssn1_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn1_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn1_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn1_2c1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn1_2c2_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn1_2c2_cnstcd varchar(30) DEFAULT '', swkkj_ssn1_2c2_cnstnm varchar(100) DEFAULT '',
    -- SSN2: 計上2
    swkkj_ssn2_out_f boolean DEFAULT false,
    swkkj_ssn2_kno_togo_f boolean DEFAULT false,
    swkkj_ssn2_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn2_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn2_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn2_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn2_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn2_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN3: 計上3
    swkkj_ssn3_out_f boolean DEFAULT false,
    swkkj_ssn3_kno_togo_f boolean DEFAULT false,
    swkkj_ssn3_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn3_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn3_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn3_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn3_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn3_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN4: 識別
    swkkj_ssn4_out_f boolean DEFAULT false,
    swkkj_ssn4_kno_togo_f boolean DEFAULT false,
    swkkj_ssn4_krzei_out_f boolean DEFAULT false,
    swkkj_ssn4_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn4_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn4_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn4_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn4_1d2_cnstcd varchar(30) DEFAULT '', swkkj_ssn4_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn4_1d3_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn4_1d3_cnstcd varchar(30) DEFAULT '', swkkj_ssn4_1d3_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn4_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn4_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn4_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN5: 計上5
    swkkj_ssn5_out_f boolean DEFAULT false,
    swkkj_ssn5_kno_togo_f boolean DEFAULT false,
    swkkj_ssn5_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn5_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn5_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn5_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn5_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn5_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN6: 終了（資産）
    swkkj_ssn6_out_f boolean DEFAULT false,
    swkkj_ssn6_kno_togo_f boolean DEFAULT false,
    swkkj_ssn6_kaiyk_out_f boolean DEFAULT false,
    swkkj_ssn6_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn6_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn6_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn6_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn6_1d2_cnstcd varchar(30) DEFAULT '', swkkj_ssn6_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn6_1d3_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn6_1d3_cnstcd varchar(30) DEFAULT '', swkkj_ssn6_1d3_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn6_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn6_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn6_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN7: 回収数（資産）
    swkkj_ssn7_out_f boolean DEFAULT false,
    swkkj_ssn7_kno_togo_f boolean DEFAULT false,
    swkkj_ssn7_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn7_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn7_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn7_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn7_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn7_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN8: 計上8
    swkkj_ssn8_out_f boolean DEFAULT false,
    swkkj_ssn8_kno_togo_f boolean DEFAULT false,
    swkkj_ssn8_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn8_1d1_cnstcd varchar(30) DEFAULT '', swkkj_ssn8_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn8_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn8_1d2_cnstcd varchar(30) DEFAULT '', swkkj_ssn8_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_ssn8_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_ssn8_2c1_cnstcd varchar(30) DEFAULT '', swkkj_ssn8_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO1
    swkkj_hiyo1_out_f boolean DEFAULT false,
    swkkj_hiyo1_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo1_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo1_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo1_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo1_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo1_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo1_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO2
    swkkj_hiyo2_out_f boolean DEFAULT false,
    swkkj_hiyo2_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo2_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo2_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo2_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo2_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo2_1d2_cnstcd varchar(30) DEFAULT '', swkkj_hiyo2_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo2_1d3_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo2_1d3_cnstcd varchar(30) DEFAULT '', swkkj_hiyo2_1d3_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo2_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo2_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo2_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO3
    swkkj_hiyo3_out_f boolean DEFAULT false,
    swkkj_hiyo3_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo3_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo3_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo3_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo3_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo3_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo3_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO4
    swkkj_hiyo4_out_f boolean DEFAULT false,
    swkkj_hiyo4_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo4_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo4_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo4_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo4_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo4_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo4_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO5
    swkkj_hiyo5_out_f boolean DEFAULT false,
    swkkj_hiyo5_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo5_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo5_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo5_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo5_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo5_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo5_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO6
    swkkj_hiyo6_out_f boolean DEFAULT false,
    swkkj_hiyo6_kno_togo_f boolean DEFAULT false,
    swkkj_hiyo6_1d1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo6_1d1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo6_1d1_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo6_1d2_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo6_1d2_cnstcd varchar(30) DEFAULT '', swkkj_hiyo6_1d2_cnstnm varchar(100) DEFAULT '',
    swkkj_hiyo6_2c1_fldnm varchar(30) DEFAULT 'CONST', swkkj_hiyo6_2c1_cnstcd varchar(30) DEFAULT '', swkkj_hiyo6_2c1_cnstnm varchar(100) DEFAULT ''
);

-- ---------------------------------------------------------
-- tw_f_仕訳出力標準_設定_swksm（リース債務返済明細表設定）
-- ---------------------------------------------------------
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_設定_swksm";
CREATE TABLE "tw_f_仕訳出力標準_設定_swksm" (
    id SERIAL PRIMARY KEY,
    -- SSN1
    swksm_ssn1_out_f boolean DEFAULT false,
    swksm_ssn1_kno_togo_f boolean DEFAULT false,
    swksm_ssn1_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn1_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn1_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn1_1d2_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn1_1d2_cnstcd varchar(30) DEFAULT '', swksm_ssn1_1d2_cnstnm varchar(100) DEFAULT '',
    swksm_ssn1_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn1_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn1_2c1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn1_2c2_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn1_2c2_cnstcd varchar(30) DEFAULT '', swksm_ssn1_2c2_cnstnm varchar(100) DEFAULT '',
    -- SSN2
    swksm_ssn2_out_f boolean DEFAULT false,
    swksm_ssn2_kno_togo_f boolean DEFAULT false,
    swksm_ssn2_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn2_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn2_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn2_1d2_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn2_1d2_cnstcd varchar(30) DEFAULT '', swksm_ssn2_1d2_cnstnm varchar(100) DEFAULT '',
    swksm_ssn2_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn2_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn2_2c1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn2_2c2_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn2_2c2_cnstcd varchar(30) DEFAULT '', swksm_ssn2_2c2_cnstnm varchar(100) DEFAULT '',
    -- SSN3
    swksm_ssn3_out_f boolean DEFAULT false,
    swksm_ssn3_kno_togo_f boolean DEFAULT false,
    swksm_ssn3_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn3_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn3_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn3_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn3_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn3_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN4
    swksm_ssn4_out_f boolean DEFAULT false,
    swksm_ssn4_kno_togo_f boolean DEFAULT false,
    swksm_ssn4_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn4_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn4_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn4_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn4_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn4_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN5
    swksm_ssn5_out_f boolean DEFAULT false,
    swksm_ssn5_kno_togo_f boolean DEFAULT false,
    swksm_ssn5_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn5_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn5_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn5_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn5_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn5_2c1_cnstnm varchar(100) DEFAULT '',
    -- SSN6
    swksm_ssn6_out_f boolean DEFAULT false,
    swksm_ssn6_kno_togo_f boolean DEFAULT false,
    swksm_ssn6_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn6_1d1_cnstcd varchar(30) DEFAULT '', swksm_ssn6_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_ssn6_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_ssn6_2c1_cnstcd varchar(30) DEFAULT '', swksm_ssn6_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO1
    swksm_hiyo1_out_f boolean DEFAULT false,
    swksm_hiyo1_kno_togo_f boolean DEFAULT false,
    swksm_hiyo1_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo1_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo1_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo1_1d2_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo1_1d2_cnstcd varchar(30) DEFAULT '', swksm_hiyo1_1d2_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo1_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo1_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo1_2c1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo1_2c2_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo1_2c2_cnstcd varchar(30) DEFAULT '', swksm_hiyo1_2c2_cnstnm varchar(100) DEFAULT '',
    -- HIYO2
    swksm_hiyo2_out_f boolean DEFAULT false,
    swksm_hiyo2_kno_togo_f boolean DEFAULT false,
    swksm_hiyo2_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo2_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo2_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo2_1d2_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo2_1d2_cnstcd varchar(30) DEFAULT '', swksm_hiyo2_1d2_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo2_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo2_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo2_2c1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo2_2c2_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo2_2c2_cnstcd varchar(30) DEFAULT '', swksm_hiyo2_2c2_cnstnm varchar(100) DEFAULT '',
    -- HIYO3
    swksm_hiyo3_out_f boolean DEFAULT false,
    swksm_hiyo3_kno_togo_f boolean DEFAULT false,
    swksm_hiyo3_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo3_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo3_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo3_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo3_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo3_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO4
    swksm_hiyo4_out_f boolean DEFAULT false,
    swksm_hiyo4_kno_togo_f boolean DEFAULT false,
    swksm_hiyo4_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo4_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo4_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo4_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo4_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo4_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO5
    swksm_hiyo5_out_f boolean DEFAULT false,
    swksm_hiyo5_kno_togo_f boolean DEFAULT false,
    swksm_hiyo5_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo5_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo5_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo5_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo5_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo5_2c1_cnstnm varchar(100) DEFAULT '',
    -- HIYO6
    swksm_hiyo6_out_f boolean DEFAULT false,
    swksm_hiyo6_kno_togo_f boolean DEFAULT false,
    swksm_hiyo6_1d1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo6_1d1_cnstcd varchar(30) DEFAULT '', swksm_hiyo6_1d1_cnstnm varchar(100) DEFAULT '',
    swksm_hiyo6_2c1_fldnm varchar(30) DEFAULT 'CONST', swksm_hiyo6_2c1_cnstcd varchar(30) DEFAULT '', swksm_hiyo6_2c1_cnstnm varchar(100) DEFAULT ''
);

-- ---------------------------------------------------------
-- tw_f_仕訳出力標準_設定_swkky（共通設定）
-- ---------------------------------------------------------
DROP TABLE IF EXISTS "tw_f_仕訳出力標準_設定_swkky";
CREATE TABLE "tw_f_仕訳出力標準_設定_swkky" (
    id SERIAL PRIMARY KEY,
    swkky_kmknm_hokan boolean DEFAULT false,
    swkky_dc_betu_f boolean DEFAULT false,
    swkky_ver varchar(10) DEFAULT '1.0'
);
