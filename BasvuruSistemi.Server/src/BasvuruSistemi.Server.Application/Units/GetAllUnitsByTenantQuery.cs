using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;

public sealed record GetAllUnitsByTenantQuery() : IRequest<Result<List<GetAllUnitsByTenantQueryResponse>>>;

public sealed class GetAllUnitsByTenantQueryResponse
{
    public Guid? ParentId { get; set; }
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Code { get; set; }
}

internal sealed class GetAllUnitsByTenantQueryHandler(
    ICurrentUserService currentUserService,
    ITenantRepository tenantRepository,
    IUnitRepository unitRepository
) : IRequestHandler<GetAllUnitsByTenantQuery, Result<List<GetAllUnitsByTenantQueryResponse>>>
{
    public async Task<Result<List<GetAllUnitsByTenantQueryResponse>>> Handle(GetAllUnitsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<List<GetAllUnitsByTenantQueryResponse>>.Failure(401,"Unauthorized");

        var tenant = await tenantRepository.FirstOrDefaultAsync(p => p.Id == tenantId.Value, cancellationToken);
        if (tenant is null)
            return Result<List<GetAllUnitsByTenantQueryResponse>>.Failure(404, "Tenant not found");

        var units = await unitRepository
            .Where(p => p.TenantId == tenantId.Value && !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var response = new List<GetAllUnitsByTenantQueryResponse>
        {
            new GetAllUnitsByTenantQueryResponse
            {
                Id = tenant.Id,
                ParentId = null,
                Name = tenant.Name,
                Code = tenant.Code,
            }
        };

        response.AddRange(units.Select(unit => new GetAllUnitsByTenantQueryResponse
        {
            ParentId = unit.ParentId,
            Id = unit.Id,
            Name = unit.Name,
            Code = unit.Code,
        }));

        return Result<List<GetAllUnitsByTenantQueryResponse>>.Succeed(response);
    }
}
