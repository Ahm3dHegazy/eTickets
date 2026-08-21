using eTickets.Business.Interfaces;
using eTickets.Models;

namespace eTickets.Data.Services
{
    public class ActorMoviesService:IActorMoviesService
    {
        private readonly AppDbContext context;
        public ActorMoviesService(AppDbContext context) 
        {
            this.context = context;
        }

        public async Task AddAsync(Actor_Movie entity)
        {
            await context.AddAsync(entity);

        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task DeleteByMovieIdAsync(int movieId)
        {
            var existing = context.Set<Actor_Movie>().Where(am => am.MovieId == movieId);
            context.Set<Actor_Movie>().RemoveRange(existing);
        }
    }
}
