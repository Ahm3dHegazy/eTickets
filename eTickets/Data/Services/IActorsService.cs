using eTickets.Models;

namespace eTickets.Data.Services
{
    public interface IActorsService
    {
        Task<IEnumerable<Actor>> GetAll();
        Task<Actor?> GetById(int id);
        Task Add(Actor actor);
        Task Update(int id, Actor newActor);
        Task Delete(int id);
        Task Save();
    }
}
