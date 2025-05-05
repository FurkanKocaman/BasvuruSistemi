using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class ApplicationFieldValueConfiguration : IEntityTypeConfiguration<ApplicationFieldValue>
{
    public void Configure(EntityTypeBuilder<ApplicationFieldValue> builder)
    {
        builder
         .HasOne(p => p.Application)
         .WithMany(p => p.FieldValues)
         .HasForeignKey(p => p.ApplicationId)
         .OnDelete(DeleteBehavior.NoAction);

        builder
         .HasOne(p => p.FieldDefinition)
         .WithMany()
         .HasForeignKey(p => p.FieldDefinitionId)
         .OnDelete(DeleteBehavior.NoAction);
    }
}
