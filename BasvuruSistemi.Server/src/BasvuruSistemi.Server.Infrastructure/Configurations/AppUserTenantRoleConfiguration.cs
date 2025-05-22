using BasvuruSistemi.Server.Domain.UserRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class AppUserTenantRoleConfiguration : IEntityTypeConfiguration<AppUserTenantRole>
{
    public void Configure(EntityTypeBuilder<AppUserTenantRole> builder)
    {
        builder
            .HasOne(p => p.User)
            .WithMany(p => p.Roles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Role)
            .WithMany()
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
