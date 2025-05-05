using BasvuruSistemi.Server.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Candidate>(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(100);
            address.Property(a => a.District).HasMaxLength(50);
            address.Property(a => a.City).HasMaxLength(50);
            address.Property(a => a.PostalCode).HasMaxLength(10);
            address.Property(a => a.FullAddress).HasMaxLength(200);
        });

        builder.OwnsOne(c => c.Contact, contact =>
        {
            contact.Property(c => c.Email).HasMaxLength(100);
            contact.Property(c => c.Phone).HasMaxLength(20);
        });
    }
}
