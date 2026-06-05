using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class ShopAdminController : Controller
    {
        private readonly BarberShopDbContext _context;

        public ShopAdminController(BarberShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 🚨 DEMO BYPASS: If email is missing, we force it to admin
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "admin@barber.com";

            // Find their shop
            var myShop = _context.Shops
                .Include(s => s.Barbers)
                .FirstOrDefault(s => s.OwnerEmail == userEmail);

            // 🚨 DEMO BYPASS: If no shop is linked to this email, just show the first shop in the DB
            if (myShop == null)
            {
                myShop = _context.Shops.Include(s => s.Barbers).FirstOrDefault();
            }

            if (myShop == null)
            {
                return Content("⚠️ No Shops exist in the Database. Please run /Account/SetupSaas first.");
            }

            return View(myShop);
        }

        [HttpPost]
        public IActionResult AddBarber(string name, string email, string password, string specialization)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail") ?? "admin@barber.com";
            var myShop = _context.Shops.FirstOrDefault(s => s.OwnerEmail == userEmail)
                         ?? _context.Shops.FirstOrDefault();

            if (myShop == null) return RedirectToAction("Index");

            var newBarberUser = new User
            {
                Name = name,
                Email = email,
                Password = password,
                IsAdmin = false
            };
            _context.Users.Add(newBarberUser);
            _context.SaveChanges();

            var newBarber = new Barber
            {
                Name = name,
                Specialization = specialization ?? "Master Barber",
                ImageUrl = "https://ui-avatars.com/api/?name=" + name,
                UserId = newBarberUser.UserId,
                ShopId = myShop.ShopId
            };
            _context.Barbers.Add(newBarber);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}