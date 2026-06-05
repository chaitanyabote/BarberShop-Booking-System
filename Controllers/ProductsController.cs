using Microsoft.AspNetCore.Mvc;
using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BarberShopDbContext _context;

        public ProductsController(BarberShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}
