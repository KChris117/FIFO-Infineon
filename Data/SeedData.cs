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
                // Logic untuk seeding User sudah dipindahkan sepenuhnya ke DbInitializer.
                
                // Cek apakah MasterItems sudah ada isinya.
                if (context.MasterItems.Any())
                {
                    return; // Database sudah berisi data MasterItem
                }

                context.MasterItems.AddRange(
                    new MasterItem { ItemID = "CHM-00001", ItemName = "H2SO4", ItemDescription = "Sulphuric Acid", Category = "Chemical" },
                    new MasterItem { ItemID = "CHM-00002", ItemName = "NaOH", ItemDescription = "Sodium Hydroxide", Category = "Chemical" },
                    new MasterItem { ItemID = "DSP-00001", ItemName = "Mask", ItemDescription = "Disposal Mask", Category = "Disposal" },
                    new MasterItem { ItemID = "PPE-00001", ItemName = "Gloves", ItemDescription = "Rubber Gloves", Category = "PPE" },
                    new MasterItem { ItemID = "TOOL-00001", ItemName = "Screwdriver", ItemDescription = "Philips Screwdriver", Category = "Tool" }
                );
                context.SaveChanges();
            }
        }
    }
}