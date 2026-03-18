# ドキュメント調査レポート: VB.NET移行のTODO解消パターン

## 1. Access VBA → VB.NET SQL WHERE句の移植ベストプラクティス

**パラメータ化クエリが最重要:**
- Npgsqlでは `@paramName` 構文でパラメータを指定する（`$1`, `$2` のpositional構文も可）
- `cmd.Parameters.AddWithValue("paramName", value)` で値をバインド
- **絶対に文字列結合でSQLを組み立てない** — SQLインジェクション防止 + アポストロフィ等の特殊文字対策

**Npgsqlパラメータの実装例:**
```vb
Dim cmd As New NpgsqlCommand("SELECT * FROM table WHERE id = @id AND status = @status", conn)
cmd.Parameters.AddWithValue("id", someId)
cmd.Parameters.AddWithValue("status", someStatus)
```

**Access → PostgreSQL SQL構文の違いに注意:**
- Access の `IIf()` → PostgreSQL の `CASE WHEN ... THEN ... ELSE ... END`
- Access の `Nz()` → PostgreSQL の `COALESCE()`
- Access の `#date#` → PostgreSQL の `'date'::date` または パラメータで渡す
- Access の `*` ワイルドカード → PostgreSQL の `%`（LIKE句）

**WHERE句条件不足のTODO解消パターン:**
- Access版のクエリデザインビューやVBAコードから元の条件を確認
- パラメータ化した形で再実装
- 条件が不明な場合は、テーブル構造とビジネスロジックから推測し、コメントで根拠を記載

## 2. Npgsql でワークテーブル（一時テーブル）を扱うパターン

**PostgreSQL一時テーブルの基本:**
```sql
CREATE TEMPORARY TABLE temp_work (
    col1 INTEGER,
    col2 TEXT
) ON COMMIT DROP;  -- トランザクション終了時に自動削除
```

**Npgsqlでの使用パターン:**
```vb
Using conn As New NpgsqlConnection(connStr)
    conn.Open()
    Using tx = conn.BeginTransaction()
        Dim cmdCreate As New NpgsqlCommand("CREATE TEMP TABLE w_work (...) ON COMMIT DROP", conn, tx)
        cmdCreate.ExecuteNonQuery()

        Dim cmdInsert As New NpgsqlCommand("INSERT INTO w_work SELECT ... FROM ...", conn, tx)
        cmdInsert.ExecuteNonQuery()

        Dim cmdSelect As New NpgsqlCommand("SELECT ... FROM w_work WHERE ...", conn, tx)
        Dim reader = cmdSelect.ExecuteReader()

        tx.Commit()
    End Using
End Using
```

**重要ポイント:**
- 一時テーブルはセッション（コネクション）スコープ — 同一コネクション内でのみ有効
- `ON COMMIT DROP` でトランザクション終了時自動削除
- Access のワークテーブル（`w_*`）を移植する際は、一時テーブルまたは通常テーブル + トランザクション内DELETE で再現

## 3. CRUD処理の安全な実装パターン（重複キー回避）

**PostgreSQL INSERT ON CONFLICT（UPSERT）パターン:**
```sql
INSERT INTO table_name (id, col1, col2)
VALUES (@id, @col1, @col2)
ON CONFLICT (id) DO UPDATE SET
    col1 = EXCLUDED.col1,
    col2 = EXCLUDED.col2;
```

**エラーハンドリングパターン（PostgreSQLエラーコード）:**
```vb
Try
    cmd.ExecuteNonQuery()
Catch ex As PostgresException
    If ex.SqlState = "23505" Then  ' unique_violation
        ' 重複キーエラー処理
    Else
        Throw
    End If
End Try
```

**トランザクション + セーブポイント:**
```vb
Using tx = conn.BeginTransaction()
    tx.Save("before_insert")
    Try
        cmdInsert.ExecuteNonQuery()
    Catch ex As PostgresException When ex.SqlState = "23505"
        tx.Rollback("before_insert")
        ' 代替処理（UPDATE等）
    End Try
    tx.Commit()
End Using
```

## 4. Access VBA の DLookup/DCount → VB.NET 移植パターン

**基本方針: ExecuteScalar() で単一値を取得**

**DLookup の代替:**
```vb
Public Function DbLookup(fieldName As String, tableName As String,
                          criteria As String, conn As NpgsqlConnection) As Object
    Dim sql = $"SELECT {fieldName} FROM {tableName} WHERE {criteria} LIMIT 1"
    Using cmd As New NpgsqlCommand(sql, conn)
        Dim result = cmd.ExecuteScalar()
        Return If(result Is DBNull.Value, Nothing, result)
    End Using
End Function
```

**DCount の代替:**
```vb
Public Function DbCount(tableName As String, criteria As String,
                         conn As NpgsqlConnection) As Long
    Dim sql = $"SELECT COUNT(*) FROM {tableName} WHERE {criteria}"
    Using cmd As New NpgsqlCommand(sql, conn)
        Return Convert.ToInt64(cmd.ExecuteScalar())
    End Using
End Function
```

## 5. まとめ: TODO解消時の適用優先度

1. **WHERE句条件不足** → Access版VBAコードを参照し、パラメータ化クエリで再実装
2. **計算式不明** → Access版の計算ロジックを確認し、VB.NETメソッドとして実装
3. **未実装CRUD** → `INSERT ON CONFLICT` パターンで安全なUPSERT
4. **DLookup/DCount系** → ExecuteScalar + パラメータ化ヘルパー関数で置換
5. **ワークテーブル** → PostgreSQL TEMP TABLE（ON COMMIT DROP）で再現

## Sources
- Npgsql Basic Usage (npgsql.org)
- CRUD operations on PostgreSQL using Npgsql (code4it.dev)
- PostgreSQL UPSERT Statement (neon.com)
- DLookup alternative in VB.NET (Tek-Tips)
- PostgreSQL INSERT Documentation (postgresql.org)
