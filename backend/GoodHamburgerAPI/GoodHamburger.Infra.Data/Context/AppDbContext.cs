using GoodHamburger.Domain.Entities;
using GoodHamburger.Infra.Data.Mappings;
using GoodHamburger.Infra.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MenuItemMapping());
            modelBuilder.ApplyConfiguration(new OrderMapping());
            modelBuilder.ApplyConfiguration(new OrderItemMapping());

            MenuItemSeed.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
