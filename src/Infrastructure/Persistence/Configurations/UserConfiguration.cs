using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .UseIdentityColumn();

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.Fullname)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.CreatedAt)
                .IsRequired();

            builder
                .Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder
                .Property(x => x.UpdatedBy)
                .IsRequired(false)
                .HasMaxLength(250);
        }
    }
}
