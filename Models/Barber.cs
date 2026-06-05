using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BarberShopMVC_2.Models
{
    public class Barber
    {
        public int BarberId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; }

        public string? Specialization { get; set; }

        public string? ImageUrl { get; set; }

        // ==========================================
        // BARBER PORTAL & STORE IDENTITY FIELDS
        // ==========================================
        public int? UserId { get; set; }
        public string? StoreName { get; set; }
        public string? Address { get; set; }
        public string? GoogleMapsEmbedLink { get; set; }
        public string? Bio { get; set; }
        // ==========================================

        // Rating relationship
        public List<Rating> Ratings { get; set; } = new();

        public double AverageRating =>
            Ratings.Count == 0 ? 0 : Ratings.Average(r => r.Stars);

        // ==========================================
        // ⭐ SAAS UPGRADE: Link this Barber to a Shop
        // ==========================================
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}