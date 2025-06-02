using OptimitationDatatable.Data;
using Microsoft.EntityFrameworkCore;
using DataTables.AspNet.AspNetCore;
using OptimitationDatatable.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DataTables.AspNet
builder.Services.RegisterDataTables();

builder.Services.AddScoped<ISqlToJsonService>(_ => 
    new SqlToJsonService(builder.Configuration.GetConnectionString("DefaultConnection")!));

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

app.Run();