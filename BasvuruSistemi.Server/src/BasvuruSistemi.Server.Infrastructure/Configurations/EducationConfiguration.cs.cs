using BasvuruSistemi.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder
            .HasOne(p => p.Candidate)
            .WithMany(p => p.EducationHistory)
            .HasForeignKey(p => p.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}