using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class ActorsService : IActorsService
    {
        private readonly AppDbContext _context;

        public ActorsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            return await _context.Actors.ToListAsync();
        }
        public async Task AddAsync(Actor actor)
        {
            _context.Add(actor);
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Actor?> GetByIdAsync(int id)
        {
            var actor = await _context.Actors
                            .Include(a => a.Actor_Movies)
                            .ThenInclude(am => am.Movie)
                            .FirstOrDefaultAsync(a => a.Id == id);

            return actor;
        }

        public async Task UpdateAsync(int id, Actor newActor)
        {
            newActor.Id = id;
            _context.Update(newActor);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
