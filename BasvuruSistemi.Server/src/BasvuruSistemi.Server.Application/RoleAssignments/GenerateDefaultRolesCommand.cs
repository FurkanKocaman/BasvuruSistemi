using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record GenerateDefaultRolesCommand(
    ) : IRequest<Result<string>>;

internal sealed class GenerateDefaultRolesCommandHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GenerateDefaultRolesCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GenerateDefaultRolesCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        foreach (var role in Roles.All)
        {
            var existRole = await roleManager.FindByNameAsync(role.Name);
            if (existRole == null)
            {
                existRole = new AppRole(role.Name, tenantId.Value, role.Description);
                var result = await roleManager.CreateAsync(existRole);
                if (!result.Succeeded)
                    return Result<string>.Failure($"Rol oluşturulamadı: {role.Name}");
            }

            var claims = await roleManager.GetClaimsAsync(existRole);

            foreach (var claim in role.AllowedClaims)
            {
                if(!claims.Any(p => p.Value == claim))
                {
                    await roleManager.AddClaimAsync(existRole, new System.Security.Claims.Claim("permission", claim));
                }
            }
        }

        return Result<string>.Succeed("Varsayılan roller başarıyla oluşturuldu.");

    }
}
