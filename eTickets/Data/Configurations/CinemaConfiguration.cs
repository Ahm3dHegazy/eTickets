using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Logo).HasMaxLength(500);
            builder.Property(c => c.Description).HasMaxLength(2000);

            builder.HasMany(c => c.Movies)
                   .WithOne(m => m.Cinema)
                   .HasForeignKey(m => m.CinemaId);
        }
    }
}
