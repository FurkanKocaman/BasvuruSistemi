using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record GetAllRolesSummariesByTenantQuery(
    ) : IRequest<List<GetAllRolesSummariesByTenantQueryResponse>>;

public sealed class GetAllRolesSummariesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; } 
}

internal sealed class GetAllRolesSummariesByTenantQueryHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GetAllRolesSummariesByTenantQuery, List<GetAllRolesSummariesByTenantQueryResponse>>
{
    public Task<List<GetAllRolesSummariesByTenantQueryResponse>> Handle(GetAllRolesSummariesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Task.FromResult(new List<GetAllRolesSummariesByTenantQueryResponse>());

        var response = roleManager.Roles.Where(p => p.TenantId == tenantId.Value).Select(role => new GetAllRolesSummariesByTenantQueryResponse
        {
            Id = role.Id,
            Name = role.Name
        }).ToList();

        return Task.FromResult(response);
    }
}
