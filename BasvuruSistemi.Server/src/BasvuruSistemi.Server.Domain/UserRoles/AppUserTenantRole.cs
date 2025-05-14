
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.UserRoles;
public class AppUserTenantRole
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public Guid RoleId { get; set; }
    public AppRole Role { get; set; } = default!;

    public Guid? UnitId { get; set; }
    public Unit? Unit { get; set; }


    private AppUserTenantRole() { }
    public AppUserTenantRole(Guid userId, Guid tenantId, Guid roleId, Guid? unitId)
    {
        UserId = userId;
        TenantId = tenantId;
        RoleId = roleId;
        UnitId = unitId;
    }
}
