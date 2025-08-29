using Microsoft.AspNetCore.Identity;

namespace FIFO_Infineon.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Daftar role yang kita inginkan
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Buat role jika belum ada
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Buat user admin default jika belum ada
        var adminUser = await userManager.FindByEmailAsync("admin@infineon.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "admin@infineon.com",
                Email = "admin@infineon.com",
                EmailConfirmed = true
            };
            // Ganti "Admin@123" dengan password yang lebih aman di aplikasi production
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
