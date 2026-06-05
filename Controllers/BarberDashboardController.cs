using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class BarberDashboardController : Controller
    {
        private readonly BarberShopDbContext _context;

        public BarberDashboardController(BarberShopDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. Load Today's Queue for the Logged-in Barber
        // ==========================================
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail")?.ToLower();

            // 🚨 MASTER BYPASS: Fixed to remove 'Service' property errors
            if (userId == 100)
            {
                var mockBookings = new System.Collections.Generic.List<Booking>();

                if (userEmail == "barber1@barbershop.com")
                {
                    // Schedule for April 24
                    mockBookings.Add(new Booking { BookingId = 240, BookingDate = new System.DateTime(2026, 04, 24), TimeSlot = "10:00 AM", Status = "Confirmed" });
                    mockBookings.Add(new Booking { BookingId = 241, BookingDate = new System.DateTime(2026, 04, 24), TimeSlot = "11:30 AM", Status = "Pending" });
                }
                else if (userEmail == "barber2@barbershop.com")
                {
                    // Schedule for April 25
                    mockBookings.Add(new Booking { BookingId = 250, BookingDate = new System.DateTime(2026, 04, 25), TimeSlot = "01:00 PM", Status = "Confirmed" });
                    mockBookings.Add(new Booking { BookingId = 251, BookingDate = new System.DateTime(2026, 04, 25), TimeSlot = "03:30 PM", Status = "Confirmed" });
                }
                else
                {
                    mockBookings.Add(new Booking { BookingId = 999, BookingDate = System.DateTime.Now, TimeSlot = "Anytime", Status = "Confirmed" });
                }

                return View(mockBookings);
            }

            // Normal Database Logic
            var barber = _context.Barbers.FirstOrDefault(b => b.UserId == userId);
            if (barber == null) return Content("Access Denied: Your account is not registered as a staff Barber.");

            var realBookings = _context.Bookings
                .Where(b => b.BarberId == barber.BarberId)
                .OrderByDescending(b => b.BookingDate)
                .ToList();

            return View(realBookings);
        }
        // ==========================================
        // 2. Mark a Haircut as Completed
        // ==========================================
        [HttpPost]
        public IActionResult CompleteAppointment(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = "Completed";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}