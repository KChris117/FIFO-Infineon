using FIFO_Infineon.Data;
using FIFO_Infineon.Models; // Pastikan untuk menambahkan ini
using Microsoft.AspNetCore.Identity; // Dan juga ini
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>
(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// --- PERUBAHAN UTAMA DI SINI ---
// Mendaftarkan layanan ASP.NET Core Identity
builder.Services.AddIdentity<User, IdentityRole>() // Menggunakan class User kustom Anda
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Konfigurasi ulang cookie setelah mendaftarkan Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Home/Index";
    options.AccessDeniedPath = "/Home/Index";
});
// --------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Gabungkan proses seeding ke dalam satu blok
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        await DbInitializer.Initialize(services);
        SeedData.Initialize(services); 

        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Konfigurasi pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Pastikan ini ada sebelum UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();