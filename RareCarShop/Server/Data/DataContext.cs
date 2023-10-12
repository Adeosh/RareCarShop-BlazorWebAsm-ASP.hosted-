using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RareCarShop.Shared;
using RareCarShop.Shared.Authentication;
using RareCarShop.Shared.Customs;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriceVariant>()
                .HasKey(c => new { c.CarId, c.CarEquipmentId });

            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.UserId, ci.CarId, ci.CarEquipmentId });

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.CarId, oi.CarEquipmentId });
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CarEquipment> CarsEquipment { get; set; }
        public DbSet<PriceVariant> PriceVariants { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
