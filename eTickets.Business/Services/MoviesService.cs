using eTickets.Business.Interfaces;
using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class MoviesService:EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext context;
        public MoviesService(AppDbContext context):base(context) 
        {
            this.context = context;
        }

        // Get all movies with their related Cinema and Producer data
        public new async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var movies = await context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.Reviews)
                .ToListAsync();

            return movies;
        }

        public new async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Producer)
                .Include(m => m.Actor_Movies)
                .ThenInclude(am => am.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }
    }
}
