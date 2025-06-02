using Microsoft.EntityFrameworkCore;
using OptimitationDatatable.Models;

namespace OptimitationDatatable.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
    }
}