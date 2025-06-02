using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace OptimitationDatatable.Services
{
    public class SqlToJsonService(string connectionString) : ISqlToJsonService
    {
        
        //public async Task<string> ExecuteSqlQueryAsJsonAsync(string sqlQuery)
        //{
        //    // Si la consulta no tiene FOR JSON, añadirlo para que SQL Server devuelva directamente JSON
        //    if (!sqlQuery.ToUpperInvariant().Contains("FOR JSON"))
        //    {
        //        sqlQuery += " FOR JSON PATH";
        //    }

        //    await using var connection = new SqlConnection(connectionString);
        //    await connection.OpenAsync();
        //    await using var command = new SqlCommand(sqlQuery, connection);
            

        //    await using var reader = await command.ExecuteReaderAsync();
        //    // SQL Server FOR JSON devuelve un string de JSON
        //    var result = new System.Text.StringBuilder();
        //    while (await reader.ReadAsync())
        //    {
        //        result.Append(reader.GetString(0));
        //    }
        //    return result.ToString();
        //}

        // Método adicional en SqlToJsonService.cs
        public async Task<object> GetEmployeesFromStoredProcedureAsync(
            string searchValue,
            string sortColumn,
            string sortDirection,
            int start,
            int length,
            int draw)
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("GetEmployeesForDataTable", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@searchValue", searchValue ?? "");
            command.Parameters.AddWithValue("@sortColumn", sortColumn);
            command.Parameters.AddWithValue("@sortDirection", sortDirection);
            command.Parameters.AddWithValue("@skip", start);
            command.Parameters.AddWithValue("@take", length);

            await using var reader = await command.ExecuteReaderAsync();
            var result = new System.Text.StringBuilder();
            while (await reader.ReadAsync())
            {
                result.Append(reader.GetString(0));
            }

            var jsonData = result.ToString();
            List<Dictionary<string, object>>? data = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonData);
                }
            }
            catch
            {
                data = new List<Dictionary<string, object>>();
            }

            int totalRecords = 0;
            int filteredRecords = 0;

            if (data != null && data.Count > 0 && data[0].ContainsKey("TotalCount") && data[0].ContainsKey("FilteredCount"))
            {
                totalRecords = ((JsonElement)data[0]["TotalCount"]).GetInt32();
                filteredRecords = ((JsonElement)data[0]["FilteredCount"]).GetInt32();

                foreach (var item in data)
                {
                    item.Remove("TotalCount");
                    item.Remove("FilteredCount");
                }
            }
            else
            {
                data = new List<Dictionary<string, object>>();
            }

            return new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = data
            };
        }


        //public async Task<object> ExecuteCompleteDataTablesQueryAsync(
        //    string baseQuery, 
        //    string countQuery, 
        //    string searchValue,
        //    string sortColumn, 
        //    string sortDirection,
        //    int start, 
        //    int length)
        //{
        //    // Validar columnas y dirección de ordenamiento permitidas
        //    var allowedSortColumns = new[] { "first_name", "last_name", "department" };
        //    if (!allowedSortColumns.Contains(sortColumn))
        //        sortColumn = "first_name";
        //    if (sortDirection != "ASC" && sortDirection != "DESC")
        //        sortDirection = "ASC";

        //    // Contar total de registros sin filtro
        //    string totalCountSql = countQuery;
        //    var totalCount = await ExecuteScalarAsync<int>(totalCountSql);

        //    // Construir cláusula WHERE para búsqueda
        //    string whereClause = string.Empty;
        //    var parameters = new List<SqlParameter>();
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        whereClause = " WHERE first_name LIKE @search OR last_name LIKE @search OR department LIKE @search";
        //        parameters.Add(new SqlParameter("@search", $"%{searchValue}%"));
        //    }

        //    // Contar total de registros con filtro
        //    string filteredCountSql = countQuery + whereClause;
        //    var filteredCount = await ExecuteScalarAsync<int>(filteredCountSql, parameters.ToArray());

        //    // Construir consulta final con paginación, orden y formato JSON
        //    string finalQuery = $@"
        //        {baseQuery}
        //        {whereClause}
        //        ORDER BY {sortColumn} {sortDirection}
        //        OFFSET @start ROWS FETCH NEXT @length ROWS ONLY
        //        FOR JSON PATH";
        //    parameters.Add(new SqlParameter("@start", start));
        //    parameters.Add(new SqlParameter("@length", length));

        //    // Ejecutar la consulta final y obtener JSON
        //    string jsonData = await ExecuteSqlQueryAsJsonAsync(finalQuery, parameters.ToArray());

        //    return new {
        //        recordsTotal = totalCount,
        //        recordsFiltered = filteredCount,
        //        data = JsonSerializer.Deserialize<object>(jsonData ?? "[]")
        //    };
        //}

        //private async Task<T> ExecuteScalarAsync<T>(string sql, SqlParameter[]? parameters = null)
        //{
        //    await using var connection = new SqlConnection(connectionString);
        //    await connection.OpenAsync();
        //    await using var command = new SqlCommand(sql, connection);
        //    if (parameters != null)
        //        command.Parameters.AddRange(parameters);
        //    var result = await command.ExecuteScalarAsync();
        //    if (result == DBNull.Value)
        //        return default!;
        //    return (T)Convert.ChangeType(result, typeof(T))!;
        //}
    }
}