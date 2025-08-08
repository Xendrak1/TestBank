using System.Data;

public static class DataTableExtensions
{
    public static List<Dictionary<string, object?>> ToList(this DataTable dt)
    {
        var rows = new List<Dictionary<string, object?>>(dt.Rows.Count);
        foreach (DataRow r in dt.Rows)
        {
            var dict = new Dictionary<string, object?>(dt.Columns.Count, StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn c in dt.Columns)
            {
                var val = r[c];
                dict[c.ColumnName] = val == DBNull.Value ? null : val;
            }
            rows.Add(dict);
        }
        return rows;
    }
}
