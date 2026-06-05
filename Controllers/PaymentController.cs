using Microsoft.AspNetCore.Mvc;
using BarberShopMVC_2.Models;
using BarberShopMVC_2.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BarberShopMVC_2.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BarberShopDbContext _context;

        public PaymentController(BarberShopDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout(int id, string module, decimal amount, string name, string email, string shopSlug)
        {
            var model = new PaymentViewModel
            {
                ReferenceId = id,
                Module = module,
                Amount = amount,
                CustomerName = name,
                CustomerEmail = email
            };

            ViewBag.ShopSlug = shopSlug;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int ReferenceId, string Module, string shopSlug)
        {
            await Task.Delay(1500);

            // ✅ CLEAR THE CART
            HttpContext.Session.Remove("Cart");

            if (Module == "Medical" || Module == "Doctor")
            {
                var dBooking = await _context.DBookings.FindAsync(ReferenceId);
                if (dBooking != null)
                {
                    dBooking.Status = "Confirmed";
                    dBooking.AdvancePaid = dBooking.TotalAmount * 0.20m;
                    await _context.SaveChangesAsync();
                }
                TempData["SuccessMessage"] = "Payment successful! Your consultation is secured.";
                return RedirectToAction("MyConsultations", "DBooking", new { shopSlug = shopSlug });
            }

            else if (Module == "Barber")
            {
                var booking = await _context.Bookings.FindAsync(ReferenceId);
                if (booking != null)
                {
                    booking.Status = "Confirmed";
                    await _context.SaveChangesAsync();
                }
                TempData["SuccessMessage"] = "Advance Payment successful! Your slot is confirmed.";
                return RedirectToAction("Index", "Home", new { shopSlug = shopSlug });
            }

            else if (Module == "Store" || Module == "Product")
            {
                var order = await _context.Orders.FindAsync(ReferenceId);
                if (order != null)
                {
                    // 🚨 THE FIX: Explicitly cast the int 1 to the OrderStatus Enum
                    order.Status = (OrderStatus)1;
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("Index", "ShopDirectory", new { shopSlug = shopSlug });
            }

            return RedirectToAction("Index", "Home", new { shopSlug = shopSlug });
        }
    }
}