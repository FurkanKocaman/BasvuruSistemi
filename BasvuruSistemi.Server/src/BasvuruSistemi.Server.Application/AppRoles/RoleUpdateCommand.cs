using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.AppRoles;
public sealed record RoleUpdateCommand(
    Guid Id,
    string Name,
    string? Description,
    List<string> Permissions
    ) : IRequest<Result<string>>;

internal sealed class RoleUpdateCommandHandler(
   ICurrentUserService currentUserService,
   RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(401, "Tenant not found");

        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
            return Result<string>.Failure(404, "Role not found");

        role.Update(request.Name, request.Description);

        var result = await roleManager.UpdateAsync(role);
        if (!result.Succeeded)
            return Result<string>.Failure(500, "Error while updating role");

        var validRequestedPermissions = request.Permissions
             .Where(p => Roles.AllCustomClaimTypes.Contains(p))
             .Distinct()
             .ToList();

        var currentClaims = await roleManager.GetClaimsAsync(role);
        var currentPermissions = currentClaims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .ToList();

        var claimsToRemove = currentPermissions
            .Where(p => !validRequestedPermissions.Contains(p))
            .ToList();

        foreach (var permission in claimsToRemove)
        {
            var claim = currentClaims.First(c => c.Type == "permission" && c.Value == permission);
            var removeResult = await roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
                return Result<string>.Failure(500, $"Error while removing permission: {permission}");
        }

        var claimsToAdd = validRequestedPermissions
            .Where(p => !currentPermissions.Contains(p))
            .ToList();

        foreach (var permission in claimsToAdd)
        {
            var addResult = await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permission", permission));
            if (!addResult.Succeeded)
                return Result<string>.Failure(500, $"Error while adding permission: {permission}");
        }

        return Result<string>.Succeed(role.Id.ToString());
    }
}