namespace BasvuruSistemi.Server.Application.Services;
public interface IRoleSeedService
{
    public Task SeddDefaultRoles(Guid tenantId);
}
