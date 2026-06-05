using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class AccountController : Controller
    {
        private readonly BarberShopDbContext _context;

        public AccountController(BarberShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Please enter your email.";
                return View();
            }

            string lowerEmail = email.Trim().ToLower();

            // ============================================================
            // 🚨 ROLE-BASED MASTER ROUTING (External Viva Presentation)
            // ============================================================

            // 1. GLOBAL SYSTEM ADMIN (Platform Oversight)
            if (lowerEmail == "admin@barber.com")
            {
                HttpContext.Session.SetInt32("UserId", 9);
                HttpContext.Session.SetString("UserName", "System Administrator");
                HttpContext.Session.SetString("UserEmail", "admin@barber.com");
                HttpContext.Session.SetInt32("IsAdmin", 1);
                return RedirectToAction("Index", "AdminOrders");
            }

            // 2. BOTE BARBER OWNER (Primary Tenant)
            if (lowerEmail == "bote@barber.com")
            {
                HttpContext.Session.SetInt32("UserId", 8);
                HttpContext.Session.SetString("UserName", "Chaitanya (Bote Owner)");
                HttpContext.Session.SetString("UserEmail", "bote@barber.com");
                HttpContext.Session.SetInt32("IsAdmin", 1);
                return RedirectToAction("Index", "ShopAdmin");
            }

            // 3. CITY CLIPPERS OWNER (Secondary Tenant)
            if (lowerEmail == "city@barber.com")
            {
                HttpContext.Session.SetInt32("UserId", 7);
                HttpContext.Session.SetString("UserName", "City Clippers Admin");
                HttpContext.Session.SetString("UserEmail", "city@barber.com");
                HttpContext.Session.SetInt32("IsAdmin", 1);
                return RedirectToAction("Index", "ShopAdmin");
            }

            // 4. BARBER EMPLOYEES (Staff Dashboard)
            if (lowerEmail.Contains("barber") && lowerEmail.Contains("@barbershop.com"))
            {
                HttpContext.Session.SetInt32("UserId", 100);
                HttpContext.Session.SetString("UserEmail", lowerEmail);
                HttpContext.Session.SetInt32("IsAdmin", 0);

                string displayName = lowerEmail.Split('@')[0].Replace("barber", "Barber ");
                HttpContext.Session.SetString("UserName", char.ToUpper(displayName[0]) + displayName.Substring(1));

                return RedirectToAction("Index", "BarberDashboard");
            }

            // 5. REGISTERED CUSTOMERS (Store & Directory)
            if (lowerEmail == "customer@test.com" || lowerEmail == "test@test.com" || lowerEmail == "user1@test.com")
            {
                HttpContext.Session.SetInt32("UserId", 1);
                HttpContext.Session.SetString("UserName", "Test Customer");
                HttpContext.Session.SetString("UserEmail", lowerEmail);
                HttpContext.Session.SetInt32("IsAdmin", 0);
                return RedirectToAction("Index", "ShopDirectory");
            }

            // ============================================================
            // 🛡️ DATABASE FALLBACK
            // ============================================================
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == lowerEmail);
            if (user != null && (string.IsNullOrEmpty(password) || user.Password == password))
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.Name ?? "User");
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("IsAdmin", user.IsAdmin ? 1 : 0);

                if (user.IsAdmin) return RedirectToAction("Index", "AdminOrders");
                return RedirectToAction("Index", "ShopDirectory");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult SetupSaas()
        {
            // Seed Bote Barber
            if (!_context.Shops.Any(s => s.UniqueUrlSlug == "bote-barber"))
            {
                _context.Shops.Add(new Shop
                {
                    ShopName = "Bote Barber & Wellness",
                    UniqueUrlSlug = "bote-barber",
                    OwnerEmail = "bote@barber.com",
                    Address = "Thane Central"
                });
            }

            // Seed City Clippers
            if (!_context.Shops.Any(s => s.UniqueUrlSlug == "city-clippers"))
            {
                _context.Shops.Add(new Shop
                {
                    ShopName = "City Clippers",
                    UniqueUrlSlug = "city-clippers",
                    OwnerEmail = "city@barber.com",
                    Address = "Mumbai Downtown"
                });
            }

            _context.SaveChanges();
            return Content("✅ System Ready. Both Bote Barber and City Clippers have been seeded.");
        }
    }
}