using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations
{
    public class Actor_MovieConfiguration : IEntityTypeConfiguration<Actor_Movie>
    {
        public void Configure(EntityTypeBuilder<Actor_Movie> builder)
        {
            builder.HasKey(am => new { am.ActorId, am.MovieId });

            builder.HasOne(am => am.Actor)
                   .WithMany(a => a.Actor_Movies)
                   .HasForeignKey(am => am.ActorId);

            builder.HasOne(am => am.Movie)
                   .WithMany(m => m.Actor_Movies)
                   .HasForeignKey(am => am.MovieId);
        }
    }
}
