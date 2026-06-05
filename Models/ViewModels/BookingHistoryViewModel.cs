using System;

namespace BarberShopMVC_2.ViewModels
{
    public class BookingHistoryViewModel
    {
        public int BookingId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string BarberName { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;
    }
}