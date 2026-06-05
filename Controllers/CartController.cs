using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BarberShopMVC_2.Controllers
{
    public class CartController : Controller
    {
        private readonly BarberShopDbContext _context;

        public CartController(BarberShopDbContext context)
        {
            _context = context;
        }

        // CART PAGE
        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            List<CartItem> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;

            return View(cart);
        }

        // CHECKOUT PAGE
        public IActionResult Checkout()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            List<CartItem> cart = string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;

            return View(cart);
        }

        // CONFIRM ORDER & SEND TO PAYMENT
        [HttpPost]
        public IActionResult ConfirmOrder()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartJson))
                return RedirectToAction("Index");

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;

            // Grab user info from Session to link the order properly
            int? userId = HttpContext.Session.GetInt32("UserId");
            string userName = HttpContext.Session.GetString("UserName") ?? "Guest Customer";

            var order = new Order
            {
                UserId = userId, // Securely ties the order to the logged-in user
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending, // FIXED: Set to Pending until they actually pay!
                TotalAmount = cart.Sum(c => c.Price * c.Quantity)
            };

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            // DO NOT clear the session cart yet. The PaymentController will do that after success.

            // Redirect to the Secure Payment Gateway
            return RedirectToAction("Checkout", "Payment", new
            {
                id = order.OrderId,
                module = "Store",
                amount = order.TotalAmount,
                name = userName,
                email = "customer@barbershop.com" // Can be updated if you store user emails
            });
        }

        // SUCCESS PAGE
        // SUCCESS PAGE WITH DIGITAL RECEIPT
        public IActionResult OrderSuccess(int id)
        {
            // Fetch the completed order along with the products (OrderItems) inside it
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id);

            // If someone tries to access this page without a valid order ID, send them home
            if (order == null)
            {
                return RedirectToAction("Index", "Store");
            }

            return View(order);
        }
    }
}