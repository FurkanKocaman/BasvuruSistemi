using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class JobPostingEvaluationPipelineStageConfiguration : IEntityTypeConfiguration<JobPostingEvaluationPipelineStage>
{
    public void Configure(EntityTypeBuilder<JobPostingEvaluationPipelineStage> builder)
    {
        builder.
            HasOne(p => p.JobPosting)
            .WithMany(p => p.EvaluationPipelineStages)
            .HasForeignKey(p => p.JobPostingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.EvaluationStage)
            .WithMany()
            .HasForeignKey(p => p.EvaluationStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.EvaluationForm)
            .WithMany()
            .HasForeignKey(p => p.EvaluationFormId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(p => p.ResponsibleCommission)
            .WithMany()
            .HasForeignKey(p => p.ResponsibleCommissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
