using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIFO_Infineon.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? BadgeNumber { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Role { get; set; }
    }
}