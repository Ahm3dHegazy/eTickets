using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
