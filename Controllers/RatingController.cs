using Microsoft.AspNetCore.Mvc;
using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class RatingController : Controller
    {
        private readonly BarberShopDbContext _context;

        public RatingController(BarberShopDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /Rating OR /Rating/Index
        public IActionResult Index()
        {
            var model = _context.Barbers
                .Select(b => new
                {
                    Barber = b,
                    AvgRating = _context.Ratings
                        .Where(r => r.BarberId == b.BarberId)
                        .Average(r => (double?)r.Stars) ?? 0,

                    TotalRatings = _context.Ratings
                        .Count(r => r.BarberId == b.BarberId)
                })
                .ToList<dynamic>();

            return View(model);
        }

        // ✅ GET: /Rating/Rate?barberId=1
        [HttpGet]
        public IActionResult Rate(int barberId)
        {
            var barber = _context.Barbers
                .FirstOrDefault(b => b.BarberId == barberId);

            if (barber == null)
                return NotFound();

            ViewBag.Barber = barber;
            return View();
        }

        // ✅ POST: /Rating/Rate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rate(int barberId, int stars, string comment)
        {
            if (stars < 1 || stars > 5)
            {
                ModelState.AddModelError("", "Invalid rating value");
            }

            var barber = _context.Barbers
                .FirstOrDefault(b => b.BarberId == barberId);

            if (barber == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Barber = barber;
                return View();
            }

            var rating = new Rating
            {
                BarberId = barberId,
                Stars = stars,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Ratings.Add(rating);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
