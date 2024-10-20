using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Bill_Book.Data;
using My_Bill_Book.Models;

namespace My_Bill_Book.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult SalesReport()
        {
            var sales = _context.Sales.Include(s => s.Customer).ToList();

            var viewModel = sales.Select(sale => new SalesReportViewModel
            {
                Sale = sale,
                Customer = sale.Customer
            }).ToList();

            return View(viewModel);
        }

        // StockReport Action
        public IActionResult StockReport()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);  // This passes the list of products to the StockReport view.
        }


    }
}
