// Controllers/EmployeesController.cs

using System.Text.Json;
using OptimitationDatatable.Services;
//using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;

namespace OptimitationDatatable.Controllers
{
    public class EmployeesController(ISqlToJsonService sqlService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> GetData(IDataTablesRequest request)
        //{
        //    // Extraer parámetros de DataTables
        //    int draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
        //    int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
        //    int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
        //    string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";
        //    string sortColumn = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][name]"].FirstOrDefault() ?? "Id";
        //    string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";

        //    // 1. Consulta para contar total de registros (sin filtrado)
        //    string countQuery = "SELECT COUNT(*) AS TotalCount FROM Employees";
        //    var totalCountJson = await sqlService.ExecuteSqlQueryAsJsonAsync(countQuery);
        //    var totalCountObj = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(totalCountJson);
        //    int totalRecords = totalCountObj != null && totalCountObj.Count > 0 ? 
        //        ((JsonElement)totalCountObj[0]["TotalCount"]).GetInt32() : 0;

        //    // 2. Construir la cláusula WHERE para búsqueda
        //    string whereClause = "";
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        whereClause = $@" WHERE 
        //            first_name LIKE '%{searchValue}%' OR 
        //            last_name LIKE '%{searchValue}%' OR 
        //            department LIKE '%{searchValue}%' OR 
        //            email LIKE '%{searchValue}%'";
        //    }

        //    // 3. Consulta para contar registros filtrados
        //    string filteredCountQuery = $"SELECT COUNT(*) AS FilteredCount FROM Employees {whereClause}";
        //    var filteredCountJson = await sqlService.ExecuteSqlQueryAsJsonAsync(filteredCountQuery);
        //    var filteredCountObj = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(filteredCountJson);
        //    int filteredRecords = filteredCountObj != null && filteredCountObj.Count > 0 ? 
        //        ((JsonElement)filteredCountObj[0]["FilteredCount"]).GetInt32() : 0;

        //    // 4. Consulta principal para obtener los datos
        //    string dataQuery = $@"
        //        SELECT 
        //            id, first_name, last_name, department, salary, email, hire_date
        //        FROM employees
        //        {whereClause}
        //        ORDER BY {sortColumn} {sortDirection}
        //        OFFSET {start} ROWS
        //        FETCH NEXT {length} ROWS ONLY
        //        FOR JSON PATH";

        //    var jsonData = await sqlService.ExecuteSqlQueryAsJsonAsync(dataQuery);
            
        //    // 5. Formatear la respuesta para DataTables
        //    return Json(new {
        //        draw = draw,
        //        recordsTotal = totalRecords,
        //        recordsFiltered = filteredRecords,
        //        data = JsonSerializer.Deserialize<object>(jsonData ?? "[]")
        //    });
        //}
        
        [HttpPost]
        public async Task<IActionResult> GetDataOptimizado()
        {
            int draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";
            string sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault() ?? "0";
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault() ?? "Id";
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";

            var result = await sqlService.GetEmployeesFromStoredProcedureAsync(
                searchValue,
                sortColumn,
                sortDirection,
                start,
                length,
                draw);
    
            return Json(result);
        }

    }
}
