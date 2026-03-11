-- テスト用ユーザー5名（パスワード: password123 / BCrypt WorkFactor 12）
INSERT INTO tm_USER (user_name, user_kana, login_id, password_hash, role, is_active) VALUES
('管理者',       '管理者',         'admin',      '$2a$12$LJ3m4ys3Lf0v7mMYRFEBKeYDiDHd4E6g16OWbEzXsS9RLvCZKARJG', 'admin',            TRUE),
('経理太郎',     'ケイリタロウ',   'keiri01',    '$2a$12$LJ3m4ys3Lf0v7mMYRFEBKeYDiDHd4E6g16OWbEzXsS9RLvCZKARJG', 'accounting',       TRUE),
('総務花子',     'ソウムハナコ',   'soumu01',    '$2a$12$LJ3m4ys3Lf0v7mMYRFEBKeYDiDHd4E6g16OWbEzXsS9RLvCZKARJG', 'general_affairs',  TRUE),
('監査一郎',     'カンサイチロウ', 'viewer01',   '$2a$12$LJ3m4ys3Lf0v7mMYRFEBKeYDiDHd4E6g16OWbEzXsS9RLvCZKARJG', 'viewer',           TRUE),
('無効ユーザー', 'ムコウユーザー', 'disabled01', '$2a$12$LJ3m4ys3Lf0v7mMYRFEBKeYDiDHd4E6g16OWbEzXsS9RLvCZKARJG', 'viewer',           FALSE);
