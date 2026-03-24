-- tw_m_user テーブル拡張（ログイン認証用カラム追加）
ALTER TABLE tw_m_user ADD COLUMN login_id      VARCHAR(50);
ALTER TABLE tw_m_user ADD COLUMN password_hash VARCHAR(256);
ALTER TABLE tw_m_user ADD COLUMN role          VARCHAR(20)  NOT NULL DEFAULT 'viewer';
ALTER TABLE tw_m_user ADD COLUMN is_active     BOOLEAN      NOT NULL DEFAULT TRUE;
ALTER TABLE tw_m_user ADD COLUMN last_login_at TIMESTAMP;
ALTER TABLE tw_m_user ADD COLUMN failed_login_count INTEGER NOT NULL DEFAULT 0;
ALTER TABLE tw_m_user ADD COLUMN locked_until  TIMESTAMP;

-- 既存行マイグレーション（仮パスワード: changeme → 初回ログイン後に変更を推奨）
UPDATE tw_m_user SET login_id = user_name WHERE login_id IS NULL;
UPDATE tw_m_user SET password_hash = '100000:u6HesG1k6IVNvG8aYkNI7g==:wtMlizpL9uT+pMcdrArzOtZs77p7nzaPLuUWyPWeEbE=' WHERE password_hash IS NULL;

-- NOT NULL 制約
ALTER TABLE tw_m_user ALTER COLUMN login_id      SET NOT NULL;
ALTER TABLE tw_m_user ALTER COLUMN password_hash SET NOT NULL;

-- 制約
ALTER TABLE tw_m_user ADD CONSTRAINT uq_user_login_id UNIQUE (login_id);
ALTER TABLE tw_m_user ADD CONSTRAINT chk_user_role
    CHECK (role IN ('admin', 'accounting', 'general_affairs', 'viewer'));
