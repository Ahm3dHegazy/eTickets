using eTickets.Business.Interfaces;
using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        private readonly AppDbContext _context;
        public CinemasService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<Cinema?> GetByIdAsync(int id)  // Override the GetByIdAsync method to include related Movies
        {
            var cinema = await _context.Cinemas
                .Include(c => c.Movies)              // Include the related Movies collection
                .FirstOrDefaultAsync(c => c.Id == id);  

            return cinema;
        }
    }
}
