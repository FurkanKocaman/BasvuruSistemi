using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class TenantRepository : Repository<Tenant, ApplicationDbContext>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }
}
