using FIFO_Infineon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // <-- Ganti using
using Microsoft.EntityFrameworkCore;

namespace FIFO_Infineon.Data
{
    // Ganti DbContext menjadi IdentityDbContext<User>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // DbSet untuk model Anda yang lain tetap di sini
        public DbSet<MasterItem> MasterItems { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        
        // Anda TIDAK PERLU lagi menambahkan "public DbSet<User> Users { get; set; }"
        // karena itu sudah disediakan oleh IdentityDbContext.
    }
}