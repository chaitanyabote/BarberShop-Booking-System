using BarberShopMVC_2.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace BarberShopMVC_2.Helpers
{
    public static class CartHelper
    {
        public static int GetCartCount(HttpContext context)
        {
            var cart = context.Session.GetObject<List<CartItem>>("Cart");
            return cart?.Sum(x => x.Quantity) ?? 0;
        }
    }
}
