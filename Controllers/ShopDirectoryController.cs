using BarberShopMVC_2.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class ShopDirectoryController : Controller
    {
        private readonly BarberShopDbContext _context;

        public ShopDirectoryController(BarberShopDbContext context)
        {
            _context = context;
        }

        // This is the page where customers choose their shop
        public IActionResult Index()
        {
            var allShops = _context.Shops.ToList();
            return View(allShops);
        }
    }
}