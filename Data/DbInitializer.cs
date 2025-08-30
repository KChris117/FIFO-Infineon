using FIFO_Infineon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIFO_Infineon.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>(); // <-- Meminta UserManager<User> yang benar
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Pastikan database sudah dibuat
            context.Database.EnsureCreated();

            // 1. Buat Roles (Admin, User) jika belum ada
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // 2. Buat Admin User jika belum ada
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var adminUser = new User
                {
                    UserName = "Admin", // UserName digunakan untuk login
                    Email = "admin@example.com", // Email wajib ada
                    Name = "Admin",
                    BadgeNumber = 12345678,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser); // Password tidak diperlukan untuk kasus ini

                if (result.Succeeded)
                {
                    // Tetapkan role "Admin" ke user yang baru dibuat
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}