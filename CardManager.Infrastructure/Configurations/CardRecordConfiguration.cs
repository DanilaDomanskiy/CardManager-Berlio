using CardManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardManager.Infrastructure.Configurations
{
    public class CardRecordConfiguration : IEntityTypeConfiguration<CardRecord>
    {
        public void Configure(EntityTypeBuilder<CardRecord> builder)
        {
            builder
                .HasKey(cr => cr.Id);

            builder
                .Property(cr => cr.CardNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder
                .HasIndex(cr => cr.CardNumber)
                .IsUnique();

            builder
                .Property(cr => cr.Track1)
                .IsRequired()
                .HasMaxLength(80);

            builder
                .Property(cr => cr.Track2)
                .IsRequired()
                .HasMaxLength(40);

            builder
                .Property(cr => cr.Track3)
                .IsRequired()
                .HasMaxLength(110);

            builder
                .Property(cr => cr.Created)
                .IsRequired();

            builder
                .HasOne(cr => cr.Creator)
                .WithMany(u => u.CardRecords)
                .HasForeignKey(u => u.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}