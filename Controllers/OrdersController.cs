using BarberShopMVC_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopMVC_2.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BarberShopDbContext _context;

        public OrdersController(BarberShopDbContext context)
        {
            _context = context;
        }

        // 📦 MY ORDERS
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // 🔍 ORDER DETAILS (SECURE)
        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
