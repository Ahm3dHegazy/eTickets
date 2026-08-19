using eTickets.Data.Models;
using eTickets.Models;

namespace eTickets.web.ViewModels;

public class MovieDetailsViewModel
{
    public Movie Movie { get; set; } = null!;
    public IReadOnlyList<MovieReview> Reviews { get; set; } = Array.Empty<MovieReview>();
    public CreateMovieReviewViewModel ReviewForm { get; set; } = new();
    public double AverageRating => Reviews.Count == 0 ? 0 : Reviews.Average(review => review.Rating);
    public int ReviewCount => Reviews.Count;
}
