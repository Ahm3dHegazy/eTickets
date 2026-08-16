using eTickets.Data.Base;
using eTickets.Models;

namespace eTickets.Data.Services
{
    public class ActorMoviesService:EntityBaseRepository<Actor_Movie>, IActorMoviesService
    {
        public ActorMoviesService(AppDbContext context) : base(context)
        {
        }
    }
}
