using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Tenants;
public sealed record TenantCreateCommand(
    string name,
    string? code
    ) : IRequest<Result<string>>;


internal sealed class TenantCreateCommandHandler(
    ITenantRepository tenantRepository,
    IRoleSeedService roleSeedService,
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    RoleManager<AppRole> roleManager,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<TenantCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TenantCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;

                if (!userId.HasValue)
                    return Result<string>.Failure("User not found");

                Tenant tenant = new(request.name, request.code);

                tenantRepository.Add(tenant);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await roleSeedService.SeddDefaultRoles(tenant.Id);

                var role = await roleManager.Roles.Where(p => p.Name == "TenantManager" && p.TenantId == tenant.Id).FirstOrDefaultAsync();

                if (role is null)
                    return Result<string>.Failure("Role not found");

                AppUserTenantRole appUserTenantRole = new(userId.Value, tenant.Id, role.Id,null);

                userTenantRoleRepository.Add(appUserTenantRole);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Tenant created successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync( transaction );
                return Result<string>.Failure("Exception: "+ex.Message);
            }
        }
    }
}
