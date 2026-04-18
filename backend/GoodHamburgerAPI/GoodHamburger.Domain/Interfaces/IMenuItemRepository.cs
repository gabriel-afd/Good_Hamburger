using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<MenuItem>> GetAllAsync();
    }
}
