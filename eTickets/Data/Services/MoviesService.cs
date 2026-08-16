using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
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
                .ToListAsync();

            return movies;
        }
    }
}
