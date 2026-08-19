using System.ComponentModel.DataAnnotations;

namespace eTickets.web.ViewModels;

public class CreateMovieReviewViewModel
{
    [Required]
    public int MovieId { get; set; }

    [Range(1, 5, ErrorMessage = "Choose a rating from 1 to 5 stars.")]
    public byte Rating { get; set; }

    [StringLength(1000, MinimumLength = 5, ErrorMessage = "Your review must be between 5 and 1000 characters.")]
    public string? Comment { get; set; }
}
