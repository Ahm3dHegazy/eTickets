using eTickets.Business.Interfaces;
using eTickets.Data;
using eTickets.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Business.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Movie)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Movie)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task AddAsync(Order order)
        {
            await _context.AddAsync(order);
        }
        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
                _context.Orders.Remove(order);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}