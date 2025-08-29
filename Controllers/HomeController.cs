using FIFO_Infineon.Data;
using FIFO_Infineon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace FIFO_Infineon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Hanya user yang sudah login bisa mengakses halaman ini
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetStockData()
        {
            var totalStockData = await _context.StockItems
                .Include(s => s.MasterItem)
                .GroupBy(s => s.MasterItem!.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity)
                })
                .ToListAsync();

            return Json(totalStockData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}