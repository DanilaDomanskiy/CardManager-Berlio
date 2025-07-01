using CardManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardManager.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .Property(u => u.PasswordHash)
                .IsRequired();

            builder
                .Property(u => u.IsAdmin)
                .IsRequired();
        }
    }
}