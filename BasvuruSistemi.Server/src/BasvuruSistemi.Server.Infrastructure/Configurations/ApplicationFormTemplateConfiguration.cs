using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class ApplicationFormTemplateConfiguration : IEntityTypeConfiguration<ApplicationFormTemplate>
{
    public void Configure(EntityTypeBuilder<ApplicationFormTemplate> builder)
    {
        builder
            .HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
