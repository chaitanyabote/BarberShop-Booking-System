using System;
using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int BarberId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        // Needed for exact 2-hour cancellation checks regardless of user timezone
        public DateTime AppointmentDateUtc { get; set; }

        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; }

        public decimal AdvancePaid { get; set; }

        // ==========================================
        // ⭐ SAAS UPGRADE: Link this Booking to a Shop
        // ==========================================
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }

       
        public virtual User? User { get; set; }
    }
}