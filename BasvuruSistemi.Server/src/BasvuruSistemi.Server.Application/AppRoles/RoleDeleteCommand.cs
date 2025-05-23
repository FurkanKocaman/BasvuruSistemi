using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.AppRoles;
public sealed record RoleDeleteCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(
    RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
            return Result<string>.Failure(404, "Role not found");

        var result = await roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            return Result<string>.Failure(500, "Error while deleting role");
        return Result<string>.Succeed("Role deleted successfully");
    }
}
