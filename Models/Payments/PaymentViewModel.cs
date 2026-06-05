namespace BarberShopMVC_2.Models
{
    public class PaymentViewModel
    {
        public int ReferenceId { get; set; } // Can be OrderId or DBookingId
        public string Module { get; set; } = string.Empty; // "Store" or "Doctor"
        public decimal Amount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}