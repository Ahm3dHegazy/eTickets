using eTickets.Data.Models;

namespace eTickets.Business.Interfaces;

public interface IMovieReviewsService
{
    Task<IReadOnlyList<MovieReview>> GetByMovieIdAsync(int movieId);
    Task<MovieReview?> GetByMovieAndUserAsync(int movieId, string userId);
    Task<MovieReview?> GetByIdAsync(int id);
    Task AddOrUpdateAsync(MovieReview review);
    Task DeleteAsync(MovieReview review);
}
