using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShopMVC_2.Models
{
    public class DBooking
    {
        [Key]
        public int DBookingId { get; set; }

        // The Patient making the booking
        public int UserId { get; set; }
        public User? User { get; set; }

        // The Doctor being booked
        public int DermatologistId { get; set; }
        public Dermatologist? Dermatologist { get; set; }

        // Scheduling Data
        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        // Financial Data (Required for the 20% advance system)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdvancePaid { get; set; }

        // ==========================================
        // ⭐ SAAS UPGRADE: Link to a specific Shop!
        // ==========================================
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}