using FIFO_Infineon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIFO_Infineon.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string badgeNumber, string username, string returnUrl = null!)
        {
            var user = _context.Users.FirstOrDefault(u => u.BadgeNumber == badgeNumber && u.Name == username);

            if (user == null)
            {
                // Gagal login
                TempData["ErrorMessage"] = "Badge number or username is incorrect.";
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Role, "Admin"), // Anda bisa sesuaikan peran di sini
                new Claim("BadgeNumber", user.BadgeNumber!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}