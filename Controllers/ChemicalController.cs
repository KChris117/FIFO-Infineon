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
            var stockList = await _context.StockItems.Include(s => s.MasterItem)
            .OrderBy(s => s.TanggalMasuk)
            .ToListAsync();
            return View(stockList);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}