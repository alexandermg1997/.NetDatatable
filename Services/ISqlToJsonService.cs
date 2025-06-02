namespace OptimitationDatatable.Services
{
    public interface ISqlToJsonService
    {
        //Task<string> ExecuteSqlQueryAsJsonAsync(string sqlQuery);

        Task<object> GetEmployeesFromStoredProcedureAsync(
            string searchValue,
            string sortColumn,
            string sortDirection,
            int start,
            int length,
            int draw);
    }
}