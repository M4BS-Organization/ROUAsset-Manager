-- テスト用ユーザー5名（パスワード: password123 / PBKDF2-SHA256 100,000回）
INSERT INTO tm_USER (user_name, user_kana, login_id, password_hash, role, is_active) VALUES
('管理者',       '管理者',         'admin',      '100000:HX5k4+aubefqPnN4bnpMRg==:tMN6BV+xpiRTQC8Oiyk8mKtYOPKozHgk4dOIlDUlIcs=', 'admin',            TRUE),
('経理太郎',     'ケイリタロウ',   'keiri01',    '100000:HX5k4+aubefqPnN4bnpMRg==:tMN6BV+xpiRTQC8Oiyk8mKtYOPKozHgk4dOIlDUlIcs=', 'accounting',       TRUE),
('総務花子',     'ソウムハナコ',   'soumu01',    '100000:HX5k4+aubefqPnN4bnpMRg==:tMN6BV+xpiRTQC8Oiyk8mKtYOPKozHgk4dOIlDUlIcs=', 'general_affairs',  TRUE),
('監査一郎',     'カンサイチロウ', 'viewer01',   '100000:HX5k4+aubefqPnN4bnpMRg==:tMN6BV+xpiRTQC8Oiyk8mKtYOPKozHgk4dOIlDUlIcs=', 'viewer',           TRUE),
('無効ユーザー', 'ムコウユーザー', 'disabled01', '100000:HX5k4+aubefqPnN4bnpMRg==:tMN6BV+xpiRTQC8Oiyk8mKtYOPKozHgk4dOIlDUlIcs=', 'viewer',           FALSE);
