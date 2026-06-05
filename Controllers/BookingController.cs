using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class BookingController : Controller
    {
        private readonly BarberShopDbContext _context;

        public BookingController(BarberShopDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // SAAS UPGRADE: Load Booking Page by Shop Slug
        // ==========================================
        [HttpGet]
        public IActionResult Book(string shopSlug)
        {
            // 🛑 SAFETY REDIRECT: If slug is missing, send user to pick a shop!
            if (string.IsNullOrEmpty(shopSlug))
            {
                return RedirectToAction("Index", "ShopDirectory");
            }

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentShop = _context.Shops.FirstOrDefault(s => s.UniqueUrlSlug == shopSlug);

            // 🛑 SAFETY REDIRECT: If shop doesn't exist, send them back to directory
            if (currentShop == null)
            {
                return RedirectToAction("Index", "ShopDirectory");
            }

            ViewBag.Services = _context.Services.Where(s => s.ShopId == currentShop.ShopId).ToList();
            ViewBag.Barbers = _context.Barbers.Where(b => b.ShopId == currentShop.ShopId).ToList();
            ViewBag.ShopName = currentShop.ShopName;
            ViewBag.ShopSlug = currentShop.UniqueUrlSlug;

            return View();
        }

        [HttpPost]
        public IActionResult Book(Booking booking, string shopSlug)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var currentShop = _context.Shops.FirstOrDefault(s => s.UniqueUrlSlug == shopSlug);
            if (currentShop == null) return RedirectToAction("Index", "ShopDirectory");

            booking.UserId = userId.Value;
            booking.ShopId = currentShop.ShopId;

            bool alreadyBooked = _context.Bookings.Any(b =>
                b.BarberId == booking.BarberId &&
                b.BookingDate.Date == booking.BookingDate.Date &&
                b.TimeSlot == booking.TimeSlot &&
                b.Status != "Cancelled");

            if (alreadyBooked)
            {
                TempData["ErrorMessage"] = "This time slot is already booked!";
                return RedirectToAction("Book", new { shopSlug = shopSlug });
            }

            var selectedService = _context.Services.FirstOrDefault(s => s.ServiceId == booking.ServiceId);
            decimal servicePrice = selectedService != null ? selectedService.Price : 500m;

            booking.TotalAmount = servicePrice;
            booking.AdvancePaid = servicePrice * 0.20m;
            booking.Status = "Pending Payment";
            booking.AppointmentDateUtc = booking.BookingDate.ToUniversalTime();

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Checkout", "Payment", new
            {
                id = booking.BookingId,
                module = "Barber",
                amount = booking.AdvancePaid,
                name = HttpContext.Session.GetString("UserName"),
                email = HttpContext.Session.GetString("UserEmail"),
                shopSlug = shopSlug
            });
        }

        // ... Keep your MyBookings, CancelBooking, and GetBookedSlots as they are below ...
        public IActionResult MyBookings()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var bookings = (from b in _context.Bookings
                            join s in _context.Services on b.ServiceId equals s.ServiceId
                            join br in _context.Barbers on b.BarberId equals br.BarberId
                            join sh in _context.Shops on b.ShopId equals sh.ShopId
                            where b.UserId == userId.Value
                            orderby b.BookingDate descending
                            select new
                            {
                                BookingId = b.BookingId,
                                ShopName = sh.ShopName,
                                ServiceName = s.Name,
                                BarberName = br.Name,
                                Date = b.BookingDate,
                                Time = b.TimeSlot,
                                Status = b.Status,
                                AdvancePaid = b.AdvancePaid,
                                AppointmentDateUtc = b.AppointmentDateUtc
                            }).ToList();

            return View(bookings);
        }

        [HttpPost]
        public IActionResult CancelBooking(int bookingId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.UserId == userId.Value);
            if (booking == null) return RedirectToAction("MyBookings");
            booking.Status = "Cancelled";
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Booking cancelled.";
            return RedirectToAction("MyBookings");
        }

        [HttpGet]
        public JsonResult GetBookedSlots(DateTime date, int barberId)
        {
            var bookedSlots = _context.Bookings
                .Where(b => b.BarberId == barberId && b.BookingDate.Date == date.Date && b.Status != "Cancelled")
                .Select(b => b.TimeSlot).ToList();
            return Json(bookedSlots);
        }
    }
}