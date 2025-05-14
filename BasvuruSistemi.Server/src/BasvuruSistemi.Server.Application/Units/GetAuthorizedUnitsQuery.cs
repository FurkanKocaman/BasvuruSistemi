using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record GetAuthorizedUnitsQuery(
    ) : IRequest<List<GetAuthorizedUnitsQueryResponse>>;

public sealed class GetAuthorizedUnitsQueryResponse
{
    public string Type { get; set; } = default!;
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

internal sealed class GetAuthorizedUnitsQueryhandler(
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    ITenantRepository tenantRepository,
    IUnitRepository unitRepository
    ) : IRequestHandler<GetAuthorizedUnitsQuery, List<GetAuthorizedUnitsQueryResponse>>
{
    public Task<List<GetAuthorizedUnitsQueryResponse>> Handle(GetAuthorizedUnitsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;
        if (!userId.HasValue || !tenantId.HasValue)
            return Task.FromResult(new List<GetAuthorizedUnitsQueryResponse>());

        var tenant = tenantRepository.FirstOrDefault(p => p.Id == tenantId.Value);
        if(tenant is null)
            return Task.FromResult(new List<GetAuthorizedUnitsQueryResponse>());

        var userTenantRoles = userTenantRoleRepository.Where(p => p.TenantId == tenantId).Include(p => p.Unit).Include(p => p.Role).ToList();

        if (userTenantRoles is null || !userTenantRoles.Any())
            return Task.FromResult(new List<GetAuthorizedUnitsQueryResponse>());

        var allUnits = unitRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId).Include(p => p.Children);

        List<GetAuthorizedUnitsQueryResponse> response = new List<GetAuthorizedUnitsQueryResponse>();

        if (userTenantRoles.Any(p => p.Role.Name == Roles.Admin.Name || p.Role.Name == Roles.TenantManager.Name))
        {
            response = allUnits.Select(c => new GetAuthorizedUnitsQueryResponse
            {
                Type = "Unit",
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            response.Add(new GetAuthorizedUnitsQueryResponse
            {
                Type = "Tenant",
                Id = tenantId.Value,
                Name = tenant.Name,
            });
        }
        else if (userTenantRoles.Any(p => p.Role.Name == Roles.UnitManager.Name))
        {
            var units = allUnits.Where(p => userTenantRoles.Any(u => u.UnitId == p.Id && u.Role.Name == Roles.UnitManager.Name)).ToList();

            foreach(var unit in units)
            {
                response.Add(new GetAuthorizedUnitsQueryResponse
                {
                    Type = "Unit",
                    Id = unit.Id,
                    Name = unit.Name,
                });
                foreach(var children in unit.Children)
                {
                    response.Add(new GetAuthorizedUnitsQueryResponse
                    {
                        Type = "Unit",
                        Id = children.Id,
                        Name = children.Name,
                    });
                }
            }
        }

        response = response.Distinct().ToList();

        return Task.FromResult(response);
    }
}
