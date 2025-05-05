using BasvuruSistemi.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;

internal sealed class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder
            .HasOne(p => p.Candidate)
            .WithMany(p => p.Certifications)
            .HasForeignKey(p => p.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}