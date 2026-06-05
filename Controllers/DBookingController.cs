using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarberShopMVC_2.Controllers
{
    // ⭐ FIX: Add two explicit routes to handle both URL styles
    [Route("[controller]/[action]")] // Handles /DBooking/Book
    [Route("{shopSlug}/[controller]/[action]")] // Handles /bote-barber/DBooking/Book
    public class DBookingController : Controller
    {
        private readonly BarberShopDbContext _context;

        public DBookingController(BarberShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Book(int doctorId, string? shopSlug)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            var doctor = _context.Dermatologists
                .Include(d => d.Shop)
                .FirstOrDefault(d => d.DermatologistId == doctorId);

            if (doctor == null)
                return RedirectToAction("Index", "ShopDirectory");

            // AUTO-REPAIR: If the user came from a 'bare' link, redirect them to the proper SaaS URL
            if (string.IsNullOrEmpty(shopSlug) && doctor.Shop != null)
            {
                return RedirectToAction("Book", new { doctorId = doctorId, shopSlug = doctor.Shop.UniqueUrlSlug });
            }

            ViewBag.Doctor = doctor;
            ViewBag.ShopName = doctor.Shop?.ShopName ?? "Medical Clinic";
            ViewBag.ShopSlug = shopSlug;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(DBooking booking, string? shopSlug)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // Find shop by slug OR by the doctor's linked shop
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.UniqueUrlSlug == shopSlug)
                       ?? await _context.Shops.FindAsync(booking.ShopId);

            if (userId == null || shop == null)
                return RedirectToAction("Index", "ShopDirectory");

            booking.UserId = userId.Value;
            booking.ShopId = shop.ShopId;
            booking.Status = "Pending Payment";
            booking.TotalAmount = 1000;
            booking.AdvancePaid = 200;

            _context.DBookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", "Payment", new
            {
                id = booking.DBookingId,
                module = "Medical",
                amount = 200,
                name = HttpContext.Session.GetString("UserName"),
                shopSlug = shop.UniqueUrlSlug
            });
        }

        [HttpGet]
        public IActionResult GetBookedSlots(int doctorId, DateTime date)
        {
            var bookedSlots = _context.DBookings
                .Where(b => b.DermatologistId == doctorId &&
                            b.BookingDate.Date == date.Date &&
                            b.Status != "Cancelled")
                .Select(b => b.TimeSlot)
                .ToList();

            return Json(bookedSlots);
        }

        [HttpGet]
        public IActionResult MyConsultations(string? shopSlug)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var query = _context.DBookings
                .Include(b => b.Shop)
                .Include(b => b.Dermatologist)
                .Where(b => b.UserId == userId);

            if (!string.IsNullOrEmpty(shopSlug))
            {
                query = query.Where(b => b.Shop != null && b.Shop.UniqueUrlSlug == shopSlug);
            }

            var consultations = query.OrderByDescending(b => b.BookingDate).ToList();
            ViewBag.ShopSlug = shopSlug;
            return View(consultations);
        }
    }
}