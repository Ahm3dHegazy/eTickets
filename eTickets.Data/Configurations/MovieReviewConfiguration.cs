using eTickets.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations;

public class MovieReviewConfiguration : IEntityTypeConfiguration<MovieReview>
{
    public void Configure(EntityTypeBuilder<MovieReview> builder)
    {
        builder.Property(review => review.Rating).IsRequired();
        builder.Property(review => review.Comment).HasMaxLength(1000);
        builder.HasIndex(review => new { review.MovieId, review.ApplicationUserId }).IsUnique();

        builder.HasOne(review => review.Movie)
            .WithMany(movie => movie.Reviews)
            .HasForeignKey(review => review.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(review => review.ApplicationUser)
            .WithMany()
            .HasForeignKey(review => review.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
