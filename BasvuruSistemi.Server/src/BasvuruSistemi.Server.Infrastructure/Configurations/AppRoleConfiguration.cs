using BasvuruSistemi.Server.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        var defaultIndex = builder.Metadata
             .GetIndexes()
             .FirstOrDefault(ix =>
                 ix.Properties.Count == 1 &&
                 ix.Properties[0].Name == nameof(AppRole.NormalizedName));
        if (defaultIndex != null)
            builder.Metadata.RemoveIndex(defaultIndex);

        builder.HasIndex(r => new { r.NormalizedName, r.TenantId })
              .HasDatabaseName("Roles_NormalizedName_TenantId")
              .IsUnique();

        builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(256);

        builder.Property(r => r.NormalizedName)
               .IsRequired()
               .HasMaxLength(256);
    }
}
