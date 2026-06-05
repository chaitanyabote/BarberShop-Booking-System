using BarberShopMVC_2.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class DermatologistController : Controller
    {
        private readonly BarberShopDbContext _context;

        public DermatologistController(BarberShopDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // SAAS UPGRADE: Load Doctors by Shop Slug
        // ==========================================
        public IActionResult Index(string shopSlug)
        {
            // 1. Find the specific shop they are visiting
            var currentShop = _context.Shops.FirstOrDefault(s => s.UniqueUrlSlug == shopSlug);

            if (currentShop == null)
            {
                return Content("⚠️ Clinic not found! Please check the URL.");
            }

            // 2. 🎯 FILTER: Only show Doctors assigned to THIS shop
            var shopDoctors = _context.Dermatologists
                .Where(d => d.ShopId == currentShop.ShopId)
                .ToList();

            // 3. Pass the shop info to the View
            ViewBag.ShopName = currentShop.ShopName;
            ViewBag.ShopSlug = currentShop.UniqueUrlSlug;

            return View(shopDoctors);
        }
    }
}