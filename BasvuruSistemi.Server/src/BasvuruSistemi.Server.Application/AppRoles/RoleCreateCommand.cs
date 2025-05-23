using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.AppRoles;
public sealed record RoleCreateCommand(
    string Name,
    string Description,
    List<string> Permissions
    ) : IRequest<Result<string>>;

internal sealed class RoleCreateCommandHandler(
   ICurrentUserService currentUserService,
   RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(401, "Tenant not found");

        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(401, "User not found");

        var role = new AppRole(request.Name, tenantId.Value, request.Description);

        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
            return Result<string>.Failure(500,"Error while creating role");

        foreach (var permission in request.Permissions)
        {
            if(Roles.AllCustomClaimTypes.Any(p => p.Equals(permission)))
            {
                var permissionResult = await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", permission));
                if (!permissionResult.Succeeded)
                    return Result<string>.Failure(500, "Error while adding permission to role");
            }
        }

        return Result<string>.Succeed(role.Id.ToString());
    }
}