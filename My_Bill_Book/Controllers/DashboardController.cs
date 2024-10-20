using Microsoft.AspNetCore.Mvc;
using My_Bill_Book.Data;

namespace My_Bill_Book.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalSales = _context.Sales.Sum(s => s.TotalAmount);
            var totalProducts = _context.Products.Count();
            var topProducts = _context.SaleItems
                                .GroupBy(si => si.ProductId)
                                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(si => si.Quantity) })
                                .OrderByDescending(g => g.TotalSold)
                                .Take(5)
                                .ToList();

            ViewBag.TotalSales = totalSales;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TopProducts = topProducts;

            return View();
        }
    }
}
