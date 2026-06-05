using BarberShopMVC_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class DoctorController : Controller
    {
        private readonly BarberShopDbContext _context;
        public DoctorController(BarberShopDbContext context) { _context = context; }

        public IActionResult Index()
        {
            // .Include(d => d.Shop) is CRITICAL to avoid "Shop Not Found" error
            var doctors = _context.Dermatologists.Include(d => d.Shop).ToList();
            return View(doctors);
        }
    }
}