using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Bill_Book.Data;
using My_Bill_Book.Models;

namespace My_Bill_Book.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            var totalSales = await _context.Sales.SumAsync(s => s.TotalAmount);
            var totalProducts = await _context.Products.CountAsync();
            var lowStockItems = await _context.Inventories
                .Include(i => i.Product)
                .Where(i => i.Quantity < 10)
                .ToListAsync();

            ViewBag.TotalSales = totalSales;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.LowStockItems = lowStockItems;

            return View();
        }

        // Manage Stock (Updated)
        public async Task<IActionResult> ManageStock()
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();
            return View(inventory);
        }

        // Add Stock
        public IActionResult AddStock()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(int productId, int quantity)
        {
            var inventoryItem = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId);

            if (inventoryItem == null)
            {
                inventoryItem = new Inventory
                {
                    ProductId = productId,
                    Quantity = quantity,
                    LastUpdated = DateTime.Now
                };
                _context.Inventories.Add(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += quantity;
                inventoryItem.LastUpdated = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageStock));
        }
    }
}

