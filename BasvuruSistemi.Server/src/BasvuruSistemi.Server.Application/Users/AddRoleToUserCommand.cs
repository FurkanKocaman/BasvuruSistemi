using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record AddRoleToUserCommand(
    Guid UserId,
    Guid RoleId,
    Guid? UnitId
    ) : IRequest<Result<string>>;

internal sealed class AddRoleToUserCommandHandler(
    IUserTenantRoleRepository userTenantRoleRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddRoleToUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404, "Tennat not found");

        var userRoles = await userTenantRoleRepository.Where(p =>p.UserId == userId.Value && p.TenantId == tenantId.Value).Include(p => p.Role).ToListAsync(cancellationToken);

        if(!userRoles.Any(p => p.Role.Name == Roles.TenantManager.Name))
        {
            return Result<string>.Failure(403, "You do not have permission to add roles to users");
        }

        var userTenantRole = await userTenantRoleRepository.Where(p => p.UserId == request.UserId && p.TenantId == tenantId.Value).FirstOrDefaultAsync();

        if(userTenantRole is null)
        {
            userTenantRole = new AppUserTenantRole(request.UserId,tenantId.Value, request.RoleId, request.UnitId);

            userTenantRoleRepository.Add(userTenantRole);
        }
        else
        {
            userTenantRole.Update(request.RoleId, request.UnitId);

            userTenantRoleRepository.Update(userTenantRole);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Role added to user successfully");
    }
}
