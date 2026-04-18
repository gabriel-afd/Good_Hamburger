using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Seed
{
    public class MenuItemSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(
                // Sanduíches
                new { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), Name = "X Burger", Price = 5.00m, Type = ItemType.Sandwich },
                new { Id = Guid.Parse("11111111-0000-0000-0000-000000000002"), Name = "X Egg", Price = 4.50m, Type = ItemType.Sandwich },
                new { Id = Guid.Parse("11111111-0000-0000-0000-000000000003"), Name = "X Bacon", Price = 7.00m, Type = ItemType.Sandwich },

                // Acompanhamentos
                new { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), Name = "Batata Frita", Price = 2.00m, Type = ItemType.Fries },
                new { Id = Guid.Parse("33333333-0000-0000-0000-000000000001"), Name = "Refrigerante", Price = 2.50m, Type = ItemType.Drink }
            );
        }
    }
}
