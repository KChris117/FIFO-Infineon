using System.ComponentModel.DataAnnotations;

namespace FIFO_Infineon.Models
{
    public class MasterItem
    {
        [Key] public string? ItemID { get; set; }
        [Required] public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? Category { get; set; }
    }
}