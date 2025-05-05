using BasvuruSistemi.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder
            .HasOne(p => p.Candidate)
            .WithMany(p => p.WorkExperience)
            .HasForeignKey(p => p.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}