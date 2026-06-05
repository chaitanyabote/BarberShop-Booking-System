using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class Dermatologist
    {
        public int DermatologistId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public int Experience { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        // ==========================================
        // ⭐ SAAS UPGRADE: Link to a specific Shop!
        // ==========================================
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}