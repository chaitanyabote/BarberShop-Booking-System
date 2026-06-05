using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using BarberShopMVC_2.Models;
using BarberShopMVC_2.Data;

namespace BarberShopMVC_2.Controllers
{
    public class StoreController : Controller
    {
        private readonly BarberShopDbContext _context;

        public StoreController(BarberShopDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Index: Loads the Store Page
        public IActionResult Index()
        {
            // Fetch all products from the DB. 
            // We are not filtering by ShopId here to ensure the store is never empty for your demo.
            var products = _context.Products.ToList();

            if (products == null)
            {
                products = new List<Product>();
            }

            return View(products);
        }

        // ✅ 2. AJAX: Add to Cart Logic
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Find the product
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            // Get existing cart from Session
            var cartJson = HttpContext.Session.GetString("Cart");

            List<CartItem> cart;
            if (string.IsNullOrEmpty(cartJson))
            {
                cart = new List<CartItem>();
            }
            else
            {
                // Null-safe deserialization
                cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }

            // Check if product already in cart
            var existingItem = cart.FirstOrDefault(c => c.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                // Add new item to cart
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName ?? "Premium Product",
                    Price = product.Price,
                    Image = product.ImageUrl ?? "no-image.png",
                    Quantity = 1
                });
            }

            // Save updated cart back to Session
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

            // Calculate total count for the navbar badge
            int totalCount = cart.Sum(c => c.Quantity);

            return Json(new { success = true, count = totalCount });
        }
    }
}