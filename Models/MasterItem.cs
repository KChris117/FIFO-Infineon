using System.ComponentModel.DataAnnotations;

namespace FIFO_Infineon.Models
{
    public class MasterItem
    {
        [Key] public string? ItemID { get; set; }
        [Required] public string? NamaItem { get; set; }
        public string? DeskripsiItem { get; set; }
    }
}