using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Units;
public sealed record GetUnitQuery(
    Guid unitId
    ) : IRequest<GetUnitQueryResponse?>;

public sealed class GetUnitQueryResponse
{
    public Guid? ParentId { get; set; }
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public List<GetUnitQueryResponse> Children { get; set; } = new();
}

internal sealed class GetUnitQueryHandler(
    IUnitRepository unitRepository,
    ICurrentUserService currentUserService,
    IAuthorizationService authorizationService
    ) : IRequestHandler<GetUnitQuery, GetUnitQueryResponse?>
{
    public async Task<GetUnitQueryResponse?> Handle(GetUnitQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.TenantId;
        if (!userId.HasValue)
            return null;

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return null;

        var isAuthorized = await authorizationService.IsTenantManagerAsync(tenantId.Value, userId.Value, cancellationToken);
        if (!isAuthorized)
            return null;

        var allUnits = unitRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId.Value).ToList();

        var rootUnit = allUnits.FirstOrDefault(p => p.Id == request.unitId);
        if (rootUnit is null)
            return null;

        var response = MapToResponseRecursive(rootUnit, allUnits);

        return response;
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
