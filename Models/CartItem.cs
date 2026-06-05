namespace BarberShopMVC_2.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = "no-image.png";
        public int Quantity { get; set; }
    }
}
