using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eTickets.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _context;

        public EntityBaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
             
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
                _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>()
                .FirstOrDefaultAsync(a => a.Id == id);

            return entity;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, T newEntity)
        {
            EntityEntry entityEntry = _context.Entry<T>(newEntity);
          
            entityEntry.State = EntityState.Modified;
        }
    }
}
