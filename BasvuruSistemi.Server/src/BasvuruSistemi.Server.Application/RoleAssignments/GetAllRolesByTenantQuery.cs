using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record GetAllRolesByTenantQuery(
    ) : IRequest<List<GetAllRolesByTenantQueryResponse>>;

public sealed class GetAllRolesByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; } 
}

internal sealed class GetAllRolesByTenantQueryHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GetAllRolesByTenantQuery, List<GetAllRolesByTenantQueryResponse>>
{
    public Task<List<GetAllRolesByTenantQueryResponse>> Handle(GetAllRolesByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Task.FromResult(new List<GetAllRolesByTenantQueryResponse>());

        var response = roleManager.Roles.Where(p => p.TenantId == tenantId.Value).Select(role => new GetAllRolesByTenantQueryResponse
        {
            Id = role.Id,
            Name = role.Name
        }).ToList();

        return Task.FromResult(response);
    }
}
