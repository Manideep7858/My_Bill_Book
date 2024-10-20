using Microsoft.EntityFrameworkCore;
using My_Bill_Book.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Custom routing for Categories
app.MapControllerRoute(
    name: "categories",
    pattern: "Categories/{action=Index}/{id?}",
    defaults: new { controller = "Categories", action = "Index" });

// Custom routing for Products
app.MapControllerRoute(
    name: "products",
    pattern: "Products/{action=Index}/{id?}",
    defaults: new { controller = "Products", action = "Index" });

// Custom routing for Admin Management
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin", action = "Index" });

// Custom routing for Reports
app.MapControllerRoute(
    name: "reports",
    pattern: "Reports/{action=SalesReport}/{id?}",
    defaults: new { controller = "Reports", action = "SalesReport" });

app.MapControllerRoute(
    name: "inventory",
    pattern: "Inventory/{action=Index}/{id?}",
    defaults: new { controller = "Inventory" });

app.MapControllerRoute(
    name: "bill",
    pattern: "Bill/{action=Create}/{id?}",
    defaults: new { controller = "Bill" });

app.Run();
