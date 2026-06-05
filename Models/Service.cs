using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShopMVC_2.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Duration in minutes
        public int Duration { get; set; }

        public string? Description { get; set; }

        // ==========================================
        // ⭐ SAAS UPGRADE: Link this Service to a Shop
        // ==========================================
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}