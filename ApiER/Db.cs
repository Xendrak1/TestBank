using System.Data;
using System.Data.SqlClient;

public class Db
{
    private readonly string _cs;
    public Db(IConfiguration cfg) => _cs = cfg.GetConnectionString("Default")!;

    public async Task<DataTable> ExecTableAsync(string sp, params SqlParameter[] ps)
    {
        using var cn = new SqlConnection(_cs);
        using var cmd = new SqlCommand(sp, cn) { CommandType = CommandType.StoredProcedure };
        if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
        using var da = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        await cn.OpenAsync();
        da.Fill(dt);
        return dt;
    }

    public async Task<int> ExecNonQueryAsync(string sp, params SqlParameter[] ps)
    {
        using var cn = new SqlConnection(_cs);
        using var cmd = new SqlCommand(sp, cn) { CommandType = CommandType.StoredProcedure };
        if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
        await cn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }
}
