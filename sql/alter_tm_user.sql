-- tm_USER テーブル拡張（ログイン認証用カラム追加）
ALTER TABLE tm_USER ADD COLUMN login_id      VARCHAR(50);
ALTER TABLE tm_USER ADD COLUMN password_hash VARCHAR(256);
ALTER TABLE tm_USER ADD COLUMN role          VARCHAR(20)  NOT NULL DEFAULT 'viewer';
ALTER TABLE tm_USER ADD COLUMN is_active     BOOLEAN      NOT NULL DEFAULT TRUE;
ALTER TABLE tm_USER ADD COLUMN last_login_at TIMESTAMP;
ALTER TABLE tm_USER ADD COLUMN failed_login_count INTEGER NOT NULL DEFAULT 0;
ALTER TABLE tm_USER ADD COLUMN locked_until  TIMESTAMP;

-- 既存行マイグレーション
UPDATE tm_USER SET login_id = user_name WHERE login_id IS NULL;
UPDATE tm_USER SET password_hash = '$2b$12$placeholder' WHERE password_hash IS NULL;

-- NOT NULL 制約
ALTER TABLE tm_USER ALTER COLUMN login_id      SET NOT NULL;
ALTER TABLE tm_USER ALTER COLUMN password_hash SET NOT NULL;

-- 制約
ALTER TABLE tm_USER ADD CONSTRAINT uq_user_login_id UNIQUE (login_id);
ALTER TABLE tm_USER ADD CONSTRAINT chk_user_role
    CHECK (role IN ('admin', 'accounting', 'general_affairs', 'viewer'));
