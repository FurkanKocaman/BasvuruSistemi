namespace BasvuruSistemi.Server.Application.Services;
public interface IAuthorizationService
{

    Task<bool> IsTenantManagerAsync(Guid userId, Guid tenantId, CancellationToken ct = default);

    Task<bool> IsUnitManagerAsync(Guid userId, Guid tenantId, Guid unitId, CancellationToken ct = default);

    Task<bool> CanManageUnitAsync(Guid userId, Guid tenantId, Guid? unitId, CancellationToken ct = default);
}
