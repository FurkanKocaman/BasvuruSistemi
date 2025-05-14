using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class ApplicationConfigurations : IEntityTypeConfiguration<Domain.Applications.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Applications.Application> builder)
    {
       builder
            .HasOne(p => p.JobPosting)
            .WithMany(p => p.Applications)
            .HasForeignKey(p => p.JobPostingId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Reviewer)
            .WithMany()
            .HasForeignKey(p => p.ReviewerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
