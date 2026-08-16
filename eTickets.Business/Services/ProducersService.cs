using eTickets.Business.Interfaces;
using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class ProducersService: EntityBaseRepository<Producer>, IProducersService
    {
        private readonly AppDbContext context;
        public ProducersService(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public new async Task<Producer?> GetByIdAsync(int id)
        {
            var producer = await context.Producers
                .Include(p => p.Movies)
                .FirstOrDefaultAsync(p => p.Id == id);

            return producer;
        }
    }
}
