-- ============================================================
-- Issue #28: 操作・更新ログ機構の実装
-- L_SLOG / L_ULOG / L_BKLOG テーブルの ALTER
-- ============================================================

-- 1. L_SLOG: slog_no に SEQUENCE を追加（自動採番）
--    Access版では MAX+1 方式だが、PostgreSQL では SEQUENCE が安全
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
