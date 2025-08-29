using Microsoft.AspNetCore.Mvc;
using FIFO_Infineon.Models;
using FIFO_Infineon.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization; // Tambahkan baris ini

namespace FIFO_Infineon.Controllers
{
    [Authorize] // Tambahkan atribut ini untuk melindungi seluruh controller
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var stockList = await _context.StockItems.Include(s => s.MasterItem).OrderBy(s => s.EntryDate).ToListAsync();
            return View(stockList);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MasterItemID,Quantity,MasterItem.Category")] StockItem stockItem)
        {
            ModelState.Remove("MasterItem");
            if (ModelState.IsValid)
            {
                stockItem.EntryDate = DateTime.Now;
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var masterItem = await _context.MasterItems.FindAsync(id);
            if (masterItem == null) return NotFound();
            return Json(new { itemName = masterItem.ItemName, itemDescription = masterItem.ItemDescription });
        }
    }
}