using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.User)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.Text)
                .IsRequired();

            builder
                .Property(x => x.Room)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.CreatedAt)
                .IsRequired();

            builder
                .Property(x => x.CreatedBy)
                .IsRequired();

            builder
                .Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder
                .Property(x => x.UpdatedBy)
                .IsRequired(false);
        }
    }
}
