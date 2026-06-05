using System;
using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔗 Barber relationship
        public int BarberId { get; set; }
        public Barber? Barber { get; set; }

        // 🔗 User relationship
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
