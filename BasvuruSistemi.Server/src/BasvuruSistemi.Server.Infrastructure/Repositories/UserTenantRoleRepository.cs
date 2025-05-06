using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class UserTenantRoleRepository : Repository<AppUserTenantRole, ApplicationDbContext>, IUserTenantRoleRepository
{
    public UserTenantRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
