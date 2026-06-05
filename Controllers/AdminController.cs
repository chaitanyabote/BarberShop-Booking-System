using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class AdminController : Controller
    {
        private readonly BarberShopDbContext _context;

        public AdminController(BarberShopDbContext context)
        {
            _context = context;
        }

        private bool CheckAdmin()
        {
            return HttpContext.Session.GetInt32("IsAdmin") == 1;
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        // VIEW BOOKINGS
        public IActionResult Bookings()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");

            var bookings = _context.Bookings.ToList() ?? new System.Collections.Generic.List<Booking>();
            return View(bookings);
        }

        // DELETE BOOKING
        [HttpPost] // Changed to Post for better practice
        public IActionResult DeleteBooking(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Account");

            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }

            return RedirectToAction("Bookings");
        }
    }
}