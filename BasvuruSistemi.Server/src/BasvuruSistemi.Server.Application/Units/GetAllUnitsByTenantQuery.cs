using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record GetAllUnitsByTenantQuery(
    ) : IRequest<List<GetAllUnitsByTenantQueryResponse>>;

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
    ) : IRequestHandler<GetAllUnitsByTenantQuery, List<GetAllUnitsByTenantQueryResponse>>
{
    public Task<List<GetAllUnitsByTenantQueryResponse>> Handle(GetAllUnitsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Task.FromResult(new List<GetAllUnitsByTenantQueryResponse>());

        var tenant = tenantRepository.FirstOrDefault(p => p.Id == tenantId.Value);
        if (tenant is null)
            return Task.FromResult(new List<GetAllUnitsByTenantQueryResponse>());

        var units = unitRepository.Where(p => p.TenantId == tenantId.Value && !p.IsDeleted).ToList();

        var response = new List<GetAllUnitsByTenantQueryResponse>();

        response.Add(new GetAllUnitsByTenantQueryResponse
        {
            Id = tenant.Id,
            ParentId = null,
            Name = tenant.Name,
            Code = tenant.Code,
        });

        response.AddRange(units.Select(unit => new GetAllUnitsByTenantQueryResponse
        {
            ParentId = unit.ParentId,
            Id = unit.Id,
            Name = unit.Name,
            Code = unit.Code,
        }).OrderBy(p => p.Name).ToList());

        

        return Task.FromResult(response);
    }
}
