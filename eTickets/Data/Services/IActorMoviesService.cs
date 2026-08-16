using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Services
{
    public interface IActorMoviesService
    {
         Task AddAsync(Actor_Movie entity);
         Task SaveAsync();
        Task DeleteByMovieIdAsync(int movieId);
    }
}
