using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record GetUnitQuery(
    Guid unitId
    ) : IRequest<GetUnitQueryResponse>;

public sealed class GetUnitQueryResponse
{
    public Guid? ParentId { get; set; }
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public List<GetUnitQueryResponse> Children { get; set; } = new();
}

internal sealed class GetUnitQueryHandler(
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<GetUnitQuery, GetUnitQueryResponse>
{
    public Task<GetUnitQueryResponse> Handle(GetUnitQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Task.FromResult<GetUnitQueryResponse>(null!);

        var allUnits = unitRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId.Value).ToList();

        var rootUnit = allUnits.FirstOrDefault(p => p.Id == request.unitId);
        if (rootUnit is null)
            return Task.FromResult<GetUnitQueryResponse>(null!);

        var response = MapToResponseRecursive(rootUnit, allUnits);

        return Task.FromResult(response);
    }

    private GetUnitQueryResponse MapToResponseRecursive(Domain.Units.Unit unit, List<Domain.Units.Unit> allUnits)
    {
        return new GetUnitQueryResponse
        {
            Id = unit.Id,
            ParentId = unit.ParentId,
            Name = unit.Name,
            Children = allUnits
                .Where(u => u.ParentId == unit.Id)
                .Select(child => MapToResponseRecursive(child, allUnits))
                .ToList()
        };
    }
}
