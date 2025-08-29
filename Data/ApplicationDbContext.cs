using Microsoft.EntityFrameworkCore;
using FIFO_Infineon.Models;

namespace FIFO_Infineon.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<MasterItem> MasterItems { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StockItem>()
            .HasOne(s => s.MasterItem)
            .WithMany()
            .HasForeignKey(s => s.MasterItemID);

            modelBuilder.Entity<MasterItem>()
            .HasIndex(m => m.ItemID)
            .IsUnique();
        }
    }
}