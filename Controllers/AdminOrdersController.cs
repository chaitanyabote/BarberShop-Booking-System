using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly BarberShopDbContext _context;

        public AdminOrdersController(BarberShopDbContext context)
        {
            _context = context;
        }

        private bool CheckAdmin()
        {
            return HttpContext.Session.GetInt32("IsAdmin") == 1;
        }

        public IActionResult Index()
        {
            // Redirect if not logged in as admin
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");

            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, OrderStatus status)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");

            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Details", new { id });
        }
    }
}