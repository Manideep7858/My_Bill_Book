using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Bill_Book.Data;
using My_Bill_Book.Models;

namespace My_Bill_Book.Controllers
{
    public class BillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sale sale, List<SaleItem> saleItems)
        {
            if (ModelState.IsValid)
            {
                sale.SaleDate = DateTime.Now;
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                foreach (var item in saleItems)
                {
                    item.SaleId = sale.Id;
                    _context.SaleItems.Add(item);

                    // Update inventory
                    var inventoryItem = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);
                    if (inventoryItem != null)
                    {
                        inventoryItem.Quantity -= item.Quantity;
                        inventoryItem.LastUpdated = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                // Generate invoice
                var invoice = new Invoice
                {
                    SaleId = sale.Id,
                    InvoiceNumber = $"INV-{sale.Id:D6}",
                    InvoiceDate = DateTime.Now,
                    TotalAmount = sale.TotalAmount,
                    IsPaid = false
                };
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = sale.Id });
            }

            ViewBag.Products = _context.Products.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            return View(sale);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.SaleId == id);

            ViewBag.Invoice = invoice;

            return View(sale);
        }
    }
}