using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FIFO_Infineon.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public int BadgeNumber { get; set; }
    }
}