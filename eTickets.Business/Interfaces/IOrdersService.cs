using eTickets.Data.Models;

namespace eTickets.Business.Interfaces
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}