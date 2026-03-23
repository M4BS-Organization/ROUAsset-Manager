# CTBテーブル DB実行確認レポート

- Issue: #3
- 実行日: 2026-03-23
- 対象DB: lease_m4bs (PostgreSQL 18.1, localhost:5432)

## 実行結果

### 1. マスタテーブル投入（前提）
`sql/master_tables.sql` を実行し、FK参照先の10テーブルを作成。

### 2. CTBテーブル作成
`sql/ctb_tables.sql` を実行。

| テーブル | 結果 |
|---------|------|
| ctb_lease_integrated | CREATE TABLE OK |
| ctb_dept_allocation | CREATE TABLE OK |
| idx_ctb_contract_no | CREATE INDEX OK |
| idx_ctb_asset_no | CREATE INDEX OK |
| idx_dept_allocation_ctb_id | CREATE INDEX OK |
| uq_dept_allocation_ctb_dept | CREATE INDEX OK |

### 3. 既存テーブルとの競合
- 実行前: 69テーブル
- マスタ追加後: 79テーブル
- CTB追加後: 81テーブル
- 名前競合: なし

### 4. INSERT/SELECT動作確認
- ctb_lease_integrated: INSERT/SELECT OK
- ctb_dept_allocation: INSERT/SELECT OK
- JOIN (ctb_lease_integrated ⟕ ctb_dept_allocation): OK
- FK制約 (m_contract_type, m_supplier, m_department): OK
- CASCADE DELETE: OK
