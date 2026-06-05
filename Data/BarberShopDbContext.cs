using BarberShopMVC_2.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopMVC_2.Data
{
    public class BarberShopDbContext : DbContext
    {
        public BarberShopDbContext(DbContextOptions<BarberShopDbContext> options)
            : base(options)
        {
        }

        // ⭐ SAAS UPGRADE: The core Shops table
        public DbSet<Shop> Shops { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        // ⭐ Rating System
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Service> Services { get; set; }

        // ⭐ Dermatologist system
        public DbSet<Dermatologist> Dermatologists { get; set; }
        public DbSet<DBooking> DBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 💰 Decimal precision fixes (Kept exactly as you had them)
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.AdvancePaid)
                .HasPrecision(18, 2);

            // ⭐ Rating relationships (Kept exactly as you had them)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Barber)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.BarberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================
            // 🏢 SAAS ARCHITECTURE: Shop Relationships
            // ==========================================

            // 1. A Shop has many Barbers
            modelBuilder.Entity<Barber>()
                .HasOne(b => b.Shop)
                .WithMany(s => s.Barbers)
                .HasForeignKey(b => b.ShopId)
                .OnDelete(DeleteBehavior.Restrict); // Restricts SQL cascade path errors

            // 2. A Shop has many Services
            modelBuilder.Entity<Service>()
                .HasOne(s => s.Shop)
                .WithMany(sh => sh.Services)
                .HasForeignKey(s => s.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. A Shop has many Bookings
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Shop)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. A Shop has many Dermatologists
            modelBuilder.Entity<Dermatologist>()
                .HasOne(d => d.Shop)
                .WithMany()
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. A Shop has many Medical Bookings (DBookings)
            modelBuilder.Entity<DBooking>()
                .HasOne(db => db.Shop)
                .WithMany()
                .HasForeignKey(db => db.ShopId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}