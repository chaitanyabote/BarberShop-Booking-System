using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        // Admin or Normal User
        public bool IsAdmin { get; set; } = false;

        // Navigation Property
        public List<Order> Orders { get; set; } = new();
    }
}
