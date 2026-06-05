using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarberShopMVC_2.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
