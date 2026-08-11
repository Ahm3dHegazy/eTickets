using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        private readonly AppDbContext _context;

        public ActorsService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Hide the base generic implementation to provide actor-specific includes
        public new async Task<Actor?> GetByIdAsync(int id)
        {
            return await _context.Actors
                .Include(a => a.Actor_Movies)
                    .ThenInclude(am => am.Movie)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
