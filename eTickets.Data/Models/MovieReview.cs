using System.ComponentModel.DataAnnotations;
using eTickets.Models;

namespace eTickets.Data.Models;

public class MovieReview
{
    [Key]
    public int Id { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Range(1, 5)]
    public byte Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
