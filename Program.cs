using BarberShopMVC_2.Data;
using BarberShopMVC_2.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<BarberShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BarberShopDbContext>();

    // We leave Dermatologists alone because they aren't tied to a specific Barber Shop yet!
    if (!context.Dermatologists.Any())
    {
        context.Dermatologists.Add(new Dermatologist
        {
            Name = "Dr. Rahul Sharma",
            Specialization = "Hair & Skin Specialist",
            Experience = 8,
            ImageUrl = "/images/doctor1.jpg"
        });

        context.Dermatologists.Add(new Dermatologist
        {
            Name = "Dr. Priya Mehta",
            Specialization = "Dermatologist",
            Experience = 10,
            ImageUrl = "/images/doctor2.jpg"
        });

        context.SaveChanges();
    }
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

// ==========================================
// 1. SAAS ROUTE: Looks for the custom shop link first! (e.g., /fade-cave/Booking/Book)
// ==========================================
app.MapControllerRoute(
    name: "shopRoute",
    pattern: "{shopSlug}/{controller=Home}/{action=Index}/{id?}");

// ==========================================
// 2. DEFAULT ROUTE: Fallback for global login
// ==========================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

/* * ⚠️ SAAS UPDATE: The old Service Seeding block has been commented out. 
 * Services now require a "ShopId". Please use the /Account/SetupSaas URL to generate services!
 * using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BarberShopDbContext>();
    if (!context.Services.Any())
    {
        // ... old seeding logic ...
    }
}
*/

app.Run();