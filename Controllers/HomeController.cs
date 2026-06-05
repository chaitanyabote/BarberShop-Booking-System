using Microsoft.AspNetCore.Mvc;

namespace BarberShopMVC_2.Controllers
{
    public class HomeController : Controller
    {
        // The default route looks for an action named "Index"
        public IActionResult Index()
        {
            return View();
        }
    }
}