using Microsoft.AspNetCore.Mvc;
using FIFO_Infineon.Models;
using FIFO_Infineon.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FIFO_Infineon.Controllers
{
    public class ChemicalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChemicalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ChemicalStockItem()
        {
            var stockList = await _context.StockItems
                .Include(s => s.MasterItem)
                .OrderBy(s => s.TanggalMasuk)
                .ToListAsync();
            return View(stockList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MasterItemID,Jumlah")] StockItem stockItem)
        {
            // PENTING: Periksa apakah MasterItemID sudah ada
            var masterItem = await _context.MasterItems.FindAsync(stockItem.MasterItemID);
            if (masterItem == null)
            {
                ModelState.AddModelError("MasterItemID", "Item ID tidak ditemukan.");
                return View(stockItem);
            }

            // Tetapkan kategori secara otomatis
            stockItem.MasterItem = masterItem;

            ModelState.Remove("MasterItem");
            if (ModelState.IsValid)
            {
                stockItem.TanggalMasuk = DateTime.Now;
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ChemicalStockItem)); // Kembali ke halaman daftar Chemical
            }
            return View(stockItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var masterItem = await _context.MasterItems.FindAsync(id);
            if (masterItem == null) return NotFound();
            return Json(new { namaItem = masterItem.NamaItem, deskripsiItem = masterItem.DeskripsiItem });
        }
    }
}