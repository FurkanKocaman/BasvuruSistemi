using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class ApplicationEvaluationConfiguration : IEntityTypeConfiguration<ApplicationEvaluation>
{
    public void Configure(EntityTypeBuilder<ApplicationEvaluation> builder)
    {
        builder
            .HasOne(ae => ae.Application)
            .WithMany(a => a.Evaluations)
            .HasForeignKey(ae => ae.ApplicationId)
            .OnDelete(DeleteBehavior.NoAction);

        //builder
        //    .HasOne(p => p.EvaluationStage)
        //    .WithMany()
        //    .HasForeignKey(p => p.EvaluationStageId)
        //    .OnDelete(DeleteBehavior.NoAction);

        //builder
        //    .HasOne(ae => ae.AssignedCommission)
        //    .WithMany()
        //    .HasForeignKey(ae => ae.AssignedCommissionId)
        //    .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(ae => ae.JobPostingEvaluationPipelineStage)
            .WithMany()
            .HasForeignKey(ae => ae.JobPostingEvaluationPipelineStageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(ae => ae.Evaluator)
            .WithMany()
            .HasForeignKey(ae => ae.EvaluatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(ae => ae.EvaluationValues)
            .WithOne(ev => ev.ApplicationEvaluation)
            .HasForeignKey(ev => ev.ApplicationEvaluationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
