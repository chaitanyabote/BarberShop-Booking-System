using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }

        [Required]
        [StringLength(100)]
        public string ShopName { get; set; } = string.Empty;

        // This is the magic string for their custom link (e.g., "city-clippers")
        [Required]
        [StringLength(50)]
        public string UniqueUrlSlug { get; set; } = string.Empty;

        public string? OwnerName { get; set; }

        [Required]
        public string OwnerEmail { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? ContactPhone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ==========================================
        // NAVIGATION PROPERTIES: All the data that belongs specifically to THIS shop
        // ==========================================
        public List<Barber> Barbers { get; set; } = new();
        public List<Service> Services { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        

    }
}