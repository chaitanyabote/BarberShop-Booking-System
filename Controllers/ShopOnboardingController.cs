using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class ShopOnboardingController : Controller
    {
        private readonly BarberShopDbContext _context;

        public ShopOnboardingController(BarberShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string shopName, string ownerName, string email, string password, string address, string phone)
        {
            // 1. Check if email is already used
            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email is already registered!";
                return View();
            }

            // 2. Auto-Generate the URL Slug (e.g., "The Golden Razor" -> "the-golden-razor")
            string slug = System.Text.RegularExpressions.Regex.Replace(shopName.ToLower().Replace(" ", "-"), "[^a-z0-9-]", "");

            // Ensure unique slug
            if (_context.Shops.Any(s => s.UniqueUrlSlug == slug))
            {
                slug += "-" + new Random().Next(100, 999);
            }

            // 3. Create the Shop (Using your EXACT property names!)
            var newShop = new Shop
            {
                ShopName = shopName,
                UniqueUrlSlug = slug,
                OwnerName = ownerName,
                OwnerEmail = email,
                Address = address,
                ContactPhone = phone,
                CreatedAt = DateTime.UtcNow
            };
            _context.Shops.Add(newShop);
            _context.SaveChanges(); // Save here to generate the newShop.ShopId!

            // 4. Create the Owner's Admin Account
            var newOwner = new User
            {
                Name = ownerName,
                Email = email,
                Password = password,
                IsAdmin = true // ⭐ They are the boss!
            };
            _context.Users.Add(newOwner);
            _context.SaveChanges();

            // 5. Auto-Login and send them to their new shop!
            HttpContext.Session.SetInt32("UserId", newOwner.UserId);
            HttpContext.Session.SetString("UserName", newOwner.Name);
            HttpContext.Session.SetString("UserEmail", newOwner.Email);

            return RedirectToAction("Index", "Home", new { shopSlug = newShop.UniqueUrlSlug });
        }
    }
}