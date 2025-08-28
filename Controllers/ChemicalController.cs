using Microsoft.AspNetCore.Mvc;
using FIFO_Infineon.Models;
using FIFO_Infineon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .Where(s => s.MasterItem != null && s.MasterItem.Category == "Chemical")
                .Include(s => s.MasterItem)
                .OrderBy(s => s.EntryDate)
                .ToListAsync();
            return View(stockList);
        }

        public IActionResult Create()
        {
            var chemicalItems = _context.MasterItems
                .Where(m => m.Category == "Chemical")
                .OrderBy(m => m.ItemID)
                .ToList();

            // Properti kedua dan ketiga di SelectList harus sama
            ViewData["MasterItemID"] = new SelectList(chemicalItems, "ItemID", "ItemID");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MasterItemID,Quantity")] StockItem stockItem)
        {
            var existingMasterItem = await _context.MasterItems.FindAsync(stockItem.MasterItemID);

            if (existingMasterItem == null)
            {
                // Untuk saat ini, kita akan asumsikan NamaItem dan DeskripsiItem kosong
                // Di masa depan, Anda bisa menambahkan input untuk ini di formulir Create
                var newMasterItem = new MasterItem
                {
                    ItemID = stockItem.MasterItemID,
                    ItemName = "Nama Item Belum Ditentukan",
                    ItemDescription = "Deskripsi Item Belum Ditentukan",
                    Category = "Chemical",
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
                stockItem.EntryDate = DateTime.Now;
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ChemicalStockItem));
            }

            return View(stockItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ganti `StockItemID` menjadi `Id`
            var stockItem = await _context.StockItems
                .Include(s => s.MasterItem)
                .FirstOrDefaultAsync(m => m.Id == id); // Perbaikan di sini

            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // Tambahkan metode Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ganti `StockItemID` menjadi `Id`
            var stockItem = await _context.StockItems
                .Include(s => s.MasterItem)
                .FirstOrDefaultAsync(m => m.Id == id); // Perbaikan di sini

            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockItem = await _context.StockItems.FindAsync(id);
            if (stockItem != null)
            {
                _context.StockItems.Remove(stockItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ChemicalStockItem));
        }

        [HttpGet]
        public async Task<IActionResult> GetItemDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var masterItem = await _context.MasterItems.FindAsync(id);
            if (masterItem == null) return NotFound();
            return Json(new { itemName = masterItem.ItemName, itemDescription = masterItem.ItemDescription });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity")] StockItem stockItem)
        {
            if (id != stockItem.Id)
            {
                return NotFound();
            }
            
            // Temukan item yang akan diedit di database
            var itemToUpdate = await _context.StockItems
                .Include(s => s.MasterItem)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (itemToUpdate == null)
            {
                return NotFound();
            }

            // Perbarui properti yang dibutuhkan
            itemToUpdate.Quantity = stockItem.Quantity;
            
            try
            {
                _context.Update(itemToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockItemExists(itemToUpdate.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ChemicalStockItem));
        }

        private bool StockItemExists(int id)
        {
        return _context.StockItems.Any(e => e.Id == id);
        }
    }
}