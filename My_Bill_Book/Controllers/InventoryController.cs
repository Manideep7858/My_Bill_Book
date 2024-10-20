using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Bill_Book.Data;
using My_Bill_Book.Models;

namespace My_Bill_Book.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .ToListAsync();
            return View(inventory);
        }

        public async Task<IActionResult> Update(int id)
        {
            var inventoryItem = await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,ProductId,Quantity")] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    inventory.LastUpdated = DateTime.Now;
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
