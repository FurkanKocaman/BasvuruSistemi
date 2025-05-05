using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class FormFieldDefinitionConfiguration : IEntityTypeConfiguration<FormFieldDefinition>
{
    public void Configure(EntityTypeBuilder<FormFieldDefinition> builder)
    {
        //builder
        //    .HasOne(p => p.Template)
        //    .WithMany(p => p.FieldDefinitions)
        //    .HasForeignKey(p => p.Template)
        //    .OnDelete(DeleteBehavior.NoAction);
    }
}
