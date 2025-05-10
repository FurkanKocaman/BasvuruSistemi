using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;

namespace BasvuruSistemi.Server.Application.Tenants;
public sealed record GetUserAuthorizedTenantsQuery(
    ) : IRequest<List<GetUserAuthorizedTenantsQueryResponse>>;

public sealed class GetUserAuthorizedTenantsQueryResponse
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Code { get; set; }
}

internal sealed class GetUserAuthorizedTenantsQueryHandler(
    ITenantRepository tenantRepository,
    IUserTenantRoleRepository userTenantRoleRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GetUserAuthorizedTenantsQuery, List<GetUserAuthorizedTenantsQueryResponse>>
{
    public Task<List<GetUserAuthorizedTenantsQueryResponse>> Handle(GetUserAuthorizedTenantsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Task.FromResult(new List<GetUserAuthorizedTenantsQueryResponse>());

        var userTenants = userTenantRoleRepository.Where(x => x.UserId == userId.Value)
            .Select(x => x.TenantId)
            .ToList();

        var tenants =  tenantRepository.Where(p => userTenants.Contains(p.Id)).ToList();
        var response = tenants.Select(t => new GetUserAuthorizedTenantsQueryResponse
        {
            Id = t.Id,
            Name = t.Name,
            Code = t.Code
        }).ToList();

        return Task.FromResult(response);
    }
}