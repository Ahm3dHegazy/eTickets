using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eTickets.Data.Configurations
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FullName).IsRequired().HasMaxLength(200);
            builder.Property(p => p.ProfilePictureURL).HasMaxLength(500);
            builder.Property(p => p.Bio).HasMaxLength(2000);

            builder.HasMany(p => p.Movies)
                   .WithOne(m => m.Producer)
                   .HasForeignKey(m => m.ProducerId);
        }
    }
}
