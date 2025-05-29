using BasvuruSistemi.Server.Domain.Evaluations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class EvaluationStageConfiguration : IEntityTypeConfiguration<EvaluationStage>
{
    public void Configure(EntityTypeBuilder<EvaluationStage> builder)
    {
        builder.
            HasOne(p => p.DefaultEvaluationForm)
            .WithMany()
            .HasForeignKey(p => p.DefaultEvaluationFormId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
