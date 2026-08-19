using eTickets.Business.Interfaces;
using eTickets.Data;
using eTickets.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Business.Services;

public class MovieReviewsService : IMovieReviewsService
{
    private readonly AppDbContext context;

    public MovieReviewsService(AppDbContext context) => this.context = context;

    public async Task<IReadOnlyList<MovieReview>> GetByMovieIdAsync(int movieId) => await context.MovieReviews
        .AsNoTracking()
        .Include(review => review.ApplicationUser)
        .Where(review => review.MovieId == movieId)
        .OrderByDescending(review => review.UpdatedAt ?? review.CreatedAt)
        .ToListAsync();

    public Task<MovieReview?> GetByMovieAndUserAsync(int movieId, string userId) => context.MovieReviews
        .FirstOrDefaultAsync(review => review.MovieId == movieId && review.ApplicationUserId == userId);

    public Task<MovieReview?> GetByIdAsync(int id) => context.MovieReviews
        .Include(review => review.Movie)
        .FirstOrDefaultAsync(review => review.Id == id);

    public async Task AddOrUpdateAsync(MovieReview review)
    {
        var existing = await GetByMovieAndUserAsync(review.MovieId, review.ApplicationUserId);
        if (existing == null)
        {
            context.MovieReviews.Add(review);
        }
        else
        {
            existing.Rating = review.Rating;
            existing.Comment = review.Comment;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MovieReview review)
    {
        context.MovieReviews.Remove(review);
        await context.SaveChangesAsync();
    }
}
