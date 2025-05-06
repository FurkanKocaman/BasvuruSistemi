using BasvuruSistemi.Server.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .HasOne(p => p.Tenant)
            .WithMany(p => p.Companies)
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
