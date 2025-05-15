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
    public Guid? ParentId { get; set; }
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Code { get; set; }
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

            response.Add(new GetAuthorizedUnitsQueryResponse
            {
                ParentId = null,
                Id = tenantId.Value,
                Name = tenant.Name,
                Code = tenant.Code,
            });

            response.AddRange(allUnits.Select(c => new GetAuthorizedUnitsQueryResponse
            {
                ParentId = c.ParentId,
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
            }).ToList());

        }
        else if (userTenantRoles.Any(p => p.Role.Name == Roles.UnitManager.Name))
        {
            var units = allUnits.Where(p => userTenantRoles.Any(u => u.UnitId == p.Id && u.Role.Name == Roles.UnitManager.Name)).ToList();

            foreach(var unit in units)
            {
                response.Add(new GetAuthorizedUnitsQueryResponse
                {
                    ParentId = unit.ParentId,
                    Id = unit.Id,
                    Name = unit.Name,
                    Code = unit.Code,
                });
                foreach(var children in unit.Children)
                {
                    response.Add(new GetAuthorizedUnitsQueryResponse
                    {
                        ParentId = children.ParentId,
                        Id = children.Id,
                        Name = children.Name,
                        Code = children.Code,
                    });
                }
            }
        }

        response = response.Distinct().ToList();

        return Task.FromResult(response);
    }
}
