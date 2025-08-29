using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFO_Infineon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}