using FIFO_Infineon.Models;
using Microsoft.EntityFrameworkCore;

namespace FIFO_Infineon.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Periksa apakah tabel MasterItem sudah memiliki data
                if (context.MasterItems.Any())
                {
                    return; // Database sudah berisi data
                }

                context.MasterItems.AddRange(
                    // Tambahkan data MasterItem untuk kategori Chemical
                    new MasterItem
                    {
                        ItemID = "CHM-001",
                        ItemName = "H2SO4",
                        ItemDescription = "Sulphuric Acid",
                        Category = "Chemical"
                    },
                    new MasterItem
                    {
                        ItemID = "CHM-002",
                        ItemName = "NaOH",
                        ItemDescription = "Sodium Hydroxide",
                        Category = "Chemical"
                    },
                    // Anda bisa menambahkan data untuk kategori Disposal, PPE, dan Tool di sini juga
                    new MasterItem
                    {
                        ItemID = "DSP-001",
                        ItemName = "Masker",
                        ItemDescription = "Masker sekali pakai",
                        Category = "Disposal"
                    },
                    new MasterItem
                    {
                        ItemID = "PPE-001",
                        ItemName = "Sarung Tangan",
                        ItemDescription = "Sarung Tangan Karet",
                        Category = "PPE"
                    },
                    new MasterItem
                    {
                        ItemID = "TOOL-001",
                        ItemName = "Obeng",
                        ItemDescription = "Obeng Philips",
                        Category = "Tool"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}