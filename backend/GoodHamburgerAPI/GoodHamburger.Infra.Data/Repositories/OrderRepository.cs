using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(new object[] { id });

            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Order order)
        {
            var existingItems = await _context.OrderItems
                .Where(i => i.OrderId == order.Id)
                .ToListAsync();

            _context.OrderItems.RemoveRange(existingItems);

            await _context.Orders
                .Where(o => o.Id == order.Id)
                .ExecuteUpdateAsync(o => o
                .SetProperty(x => x.DiscountPercentage, order.DiscountPercentage)
                .SetProperty(x => x.UpdatedAt, order.UpdatedAt));

            await _context.OrderItems.AddRangeAsync(order.Items);

            await _context.SaveChangesAsync();
        }
    }
}
