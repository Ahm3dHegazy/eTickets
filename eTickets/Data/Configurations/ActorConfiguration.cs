using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FullName).IsRequired().HasMaxLength(200);
            builder.Property(a => a.ProfilePictureURL).HasMaxLength(500);
            builder.Property(a => a.Bio).HasMaxLength(2000);

            builder.HasMany(a => a.Actor_Movies)
                   .WithOne(am => am.Actor)
                   .HasForeignKey(am => am.ActorId);
        }
    }
}
