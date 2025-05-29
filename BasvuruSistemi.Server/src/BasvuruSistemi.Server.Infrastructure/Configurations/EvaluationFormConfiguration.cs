using BasvuruSistemi.Server.Domain.Evaluations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder
            .HasOne(ef => ef.EvaluationStage)
            .WithMany(es => es.EvaluationForms)
            .HasForeignKey(ef => ef.EvaluationStageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(ef => ef.Fields)
            .WithOne(efd => efd.EvaluationForm)
            .HasForeignKey(efd => efd.EvaluationFormId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
