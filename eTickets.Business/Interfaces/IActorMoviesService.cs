using eTickets.Models;

namespace eTickets.Business.Interfaces
{
    public interface IActorMoviesService
    {
         Task AddAsync(Actor_Movie entity);
         Task SaveAsync();
        Task DeleteByMovieIdAsync(int movieId);
    }
}
