using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(200);
            builder.Property(m => m.Description).HasMaxLength(2000);
            builder.Property(m => m.Price).HasPrecision(18, 2);
            builder.Property(m => m.ImageURL).HasMaxLength(500);
            builder.Property(m => m.StartDate).IsRequired();
            builder.Property(m => m.EndDate).IsRequired();
            builder.Property(m => m.MovieCategory).IsRequired();

            builder.HasOne(m => m.Cinema)
                   .WithMany(c => c.Movies)
                   .HasForeignKey(m => m.CinemaId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Producer)
                   .WithMany(p => p.Movies)
                   .HasForeignKey(m => m.ProducerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(m => m.Actor_Movies)
                   .WithOne(am => am.Movie)
                   .HasForeignKey(am => am.MovieId);
        }
    }
}
