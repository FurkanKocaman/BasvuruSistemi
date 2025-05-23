using BasvuruSistemi.Server.Application.Auth;
using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Tenants;
public sealed record RefreshJwtByTenantCommand(
    ) : IRequest<Result<LoginCommandResponse>>;

internal sealed class RefreshJwtByTenantCommandHandler(
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider
    ) : IRequestHandler<RefreshJwtByTenantCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RefreshJwtByTenantCommand request, CancellationToken cancellationToken)
    {
       Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<LoginCommandResponse>.Failure("User not found");

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            return Result<LoginCommandResponse>.Failure("User not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<LoginCommandResponse>.Failure("Tenant not found");

        var isUserInTenant = await userTenantRoleRepository
            .AnyAsync(p => p.UserId == userId.Value && p.TenantId == tenantId.Value, cancellationToken);

        if(!isUserInTenant)
            return Result<LoginCommandResponse>.Failure("User not found in tenant");

        var token = await jwtProvider.CreateTokenAsync(user);

        var response = new LoginCommandResponse()
        {
            AccessToken = token
        };

        return Result<LoginCommandResponse>.Succeed(response);
    }
}
