using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.UserRoles;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class AuthorizationService(
    IUserTenantRoleRepository userTenantRoleRepository
    ) : IAuthorizationService
{
    public async Task<bool> CanManageUnitAsync(Guid userId, Guid tenantId, Guid? unitId, CancellationToken ct = default)
    {
        if (await IsTenantManagerAsync(userId, tenantId, ct))
            return true;

        if (!unitId.HasValue)
            return false;

        return await IsUnitManagerAsync(userId, tenantId, unitId.Value, ct);
    }

    public async Task<bool> IsTenantManagerAsync(Guid userId, Guid tenantId, CancellationToken ct = default)
    {
        return await userTenantRoleRepository.Where(p => p.UserId == userId && p.TenantId == tenantId &&  p.Role.Name == Roles.TenantManager.Name).Include(p => p.Role).AnyAsync();
    }

    public async Task<bool> IsUnitManagerAsync(Guid userId, Guid tenantId, Guid unitId, CancellationToken ct = default)
    {
        return await userTenantRoleRepository.Where(p => p.UserId == userId && p.TenantId == tenantId && p.UnitId == unitId && p.Role.Name == Roles.UnitManager.Name).Include(p => p.Role).AnyAsync();
    }
}
