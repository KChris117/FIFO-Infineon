using FIFO_Infineon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FIFO_Infineon.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string badgeNumber, string username, string returnUrl = null!)
        {
            // Ubah input string menjadi int untuk badgeNumber
            if (!int.TryParse(badgeNumber, out int badgeNumberInt))
            {
                TempData["ErrorMessage"] = "Invalid Badge Number format.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.BadgeNumber != badgeNumberInt)
            {
                TempData["ErrorMessage"] = "Badge number or username is incorrect.";
                return RedirectToAction("Index", "Home");
            }

            // Gunakan SignInManager untuk login (tanpa password untuk kasus ini)
            await _signInManager.SignInAsync(user, isPersistent: false);

            var roles = await _userManager.GetRolesAsync(user);

            // Mengarahkan berdasarkan peran setelah login berhasil
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}