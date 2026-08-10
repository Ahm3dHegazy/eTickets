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

        public async Task<IEnumerable<Actor>> GetAll()
        {
            return await _context.Actors.ToListAsync();
        }
        public async Task Add(Actor actor)
        {
          
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Actor?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(int id, Actor newActor)
        {
            throw new NotImplementedException();
        }
    }
}
