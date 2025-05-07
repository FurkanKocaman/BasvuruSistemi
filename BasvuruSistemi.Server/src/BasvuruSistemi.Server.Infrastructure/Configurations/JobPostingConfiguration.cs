using BasvuruSistemi.Server.Domain.JobPostings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class JobPostingConfiguration : IEntityTypeConfiguration<JobPosting>
{
    public void Configure(EntityTypeBuilder<JobPosting> builder)
    {
        builder
            .HasOne(p => p.Company)
            .WithMany(p => p.JobPostings)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Department)
            .WithMany(p => p.JobPostings)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);


        builder
            .HasOne(p => p.FormTemplate)
            .WithMany()
            .HasForeignKey(p => p.FormTemplateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.PostingGroup)
            .WithMany(p => p.JobPostings)
            .HasForeignKey(p => p.PostingGroupId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
