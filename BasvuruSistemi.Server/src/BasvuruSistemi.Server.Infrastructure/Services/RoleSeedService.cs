using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Infrastructure.Services;

internal sealed class RoleSeedService(RoleManager<AppRole> roleManager) : IRoleSeedService
{
    public async Task SeddDefaultRoles(Guid tenantId)
    {
        foreach (var roleDef in Roles.All)
        {
            var exists = await roleManager.Roles
                .AnyAsync(r => r.TenantId == tenantId && r.Name == roleDef.Name);

            if (!exists)
            {
                var role = new AppRole(roleDef.Name, tenantId, roleDef.Description)
                {
                    TenantId = tenantId,
                    Description = roleDef.Description,
                    CreatedAt = DateTimeOffset.Now,
                };

                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Role '{roleDef.Name}' couldn't be created: {errors}");
                }
            }
        }
    }
}
