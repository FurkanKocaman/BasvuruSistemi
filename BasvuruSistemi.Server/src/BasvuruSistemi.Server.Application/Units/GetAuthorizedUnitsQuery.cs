using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;

public sealed record GetAuthorizedUnitsQuery()
    : IRequest<Result<List<GetAuthorizedUnitsQueryResponse>>>;

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
) : IRequestHandler<GetAuthorizedUnitsQuery, Result<List<GetAuthorizedUnitsQueryResponse>>>
{
    public async Task<Result<List<GetAuthorizedUnitsQueryResponse>>> Handle(GetAuthorizedUnitsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Result<List<GetAuthorizedUnitsQueryResponse>>.Failure("Kullanıcı veya tenant bilgisi eksik.");

        var tenant = await tenantRepository.FirstOrDefaultAsync(p => p.Id == tenantId.Value, cancellationToken);
        if (tenant is null)
            return Result<List<GetAuthorizedUnitsQueryResponse>>.Failure("Tenant bulunamadı.");

        var userTenantRoles = await userTenantRoleRepository
            .Where(p => p.TenantId == tenantId)
            .Include(p => p.Unit)
            .Include(p => p.Role)
            .ToListAsync(cancellationToken);

        if (!userTenantRoles.Any())
            return Result<List<GetAuthorizedUnitsQueryResponse>>.Succeed([]);

        var allUnits = await unitRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId)
            .Include(p => p.Children)
            .ToListAsync(cancellationToken);

        List<GetAuthorizedUnitsQueryResponse> response = [];

        if (userTenantRoles.Any(p => p.Role.Name == Roles.Admin.Name || p.Role.Name == Roles.TenantManager.Name))
        {
            response.Add(new GetAuthorizedUnitsQueryResponse
            {
                ParentId = null,
                Id = tenantId.Value,
                Name = tenant.Name,
                Code = tenant.Code
            });

            response.AddRange(allUnits.Select(unit => new GetAuthorizedUnitsQueryResponse
            {
                ParentId = unit.ParentId,
                Id = unit.Id,
                Name = unit.Name,
                Code = unit.Code
            }));
        }
        else if (userTenantRoles.Any(p => p.Role.Name == Roles.UnitManager.Name))
        {
            var authorizedUnitIds = userTenantRoles
                .Where(p => p.Role.Name == Roles.UnitManager.Name)
                .Select(p => p.UnitId)
                .Distinct()
                .ToList();

            var authorizedUnits = allUnits.Where(u => authorizedUnitIds.Contains(u.Id)).ToList();

            foreach (var unit in authorizedUnits)
            {
                response.Add(new GetAuthorizedUnitsQueryResponse
                {
                    ParentId = unit.ParentId,
                    Id = unit.Id,
                    Name = unit.Name,
                    Code = unit.Code
                });

                foreach (var child in unit.Children)
                {
                    response.Add(new GetAuthorizedUnitsQueryResponse
                    {
                        ParentId = child.ParentId,
                        Id = child.Id,
                        Name = child.Name,
                        Code = child.Code
                    });
                }
            }
        }

        response = response
            .GroupBy(r => r.Id)
            .Select(g => g.First())
            .ToList();

        return Result<List<GetAuthorizedUnitsQueryResponse>>.Succeed(response);
    }
}
