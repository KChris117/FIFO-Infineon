using Microsoft.AspNetCore.Mvc;
using FIFO_Infineon.Models;
using FIFO_Infineon.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FIFO_Infineon.Controllers
{
    public class DisposalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DisposalController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> DisposalStockItem()
        {
            var stockList = await _context.StockItems
                .Where(s => s.MasterItem != null && s.MasterItem.Kategori == "Disposal")
                .Include(s => s.MasterItem)
                .OrderBy(s => s.TanggalMasuk)
                .ToListAsync();
            return View(stockList);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MasterItemID,Jumlah")] StockItem stockItem)
        {
            var existingMasterItem = await _context.MasterItems.FindAsync(stockItem.MasterItemID);

            if (existingMasterItem == null)
            {
                // Untuk saat ini, kita akan asumsikan NamaItem dan DeskripsiItem kosong
                // Di masa depan, Anda bisa menambahkan input untuk ini di formulir Create
                var newMasterItem = new MasterItem
                {
                    ItemID = stockItem.MasterItemID,
                    NamaItem = "Nama Item Belum Ditentukan",
                    DeskripsiItem = "Deskripsi Item Belum Ditentukan",
                    Kategori = "Disposal",
                };
                _context.MasterItems.Add(newMasterItem);
                await _context.SaveChangesAsync(); // Simpan MasterItem yang baru dibuat
                stockItem.MasterItem = newMasterItem;
            }
            else
            {
                stockItem.MasterItem = existingMasterItem;
            }

            ModelState.Remove("MasterItem");
            if (ModelState.IsValid)
            {
                stockItem.TanggalMasuk = DateTime.Now;
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DisposalStockItem));
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