using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;

public sealed record GetAllRolesSummariesByTenantQuery(
) : IRequest<Result<List<GetAllRolesSummariesByTenantQueryResponse>>>;

public sealed class GetAllRolesSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

internal sealed class GetAllRolesSummariesByTenantQueryHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
) : IRequestHandler<GetAllRolesSummariesByTenantQuery, Result<List<GetAllRolesSummariesByTenantQueryResponse>>>
{
    public async Task<Result<List<GetAllRolesSummariesByTenantQueryResponse>>> Handle(GetAllRolesSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<List<GetAllRolesSummariesByTenantQueryResponse>>.Failure("Tenant bilgisi bulunamadı.");

        var roles = await roleManager.Roles
            .Where(p => p.TenantId == tenantId.Value)
            .Select(role => new GetAllRolesSummariesByTenantQueryResponse
            {
                Id = role.Id,
                Name = role.Name
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetAllRolesSummariesByTenantQueryResponse>>.Succeed(roles);
    }
}
