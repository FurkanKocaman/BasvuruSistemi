using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Tenants;

public sealed record GetUserAuthorizedTenantsQuery()
    : IRequest<Result<List<GetUserAuthorizedTenantsQueryResponse>>>;

public sealed class GetUserAuthorizedTenantsQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Code { get; set; }
}

internal sealed class GetUserAuthorizedTenantsQueryHandler(
    ITenantRepository tenantRepository,
    IUserTenantRoleRepository userTenantRoleRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetUserAuthorizedTenantsQuery, Result<List<GetUserAuthorizedTenantsQueryResponse>>>
{
    public async Task<Result<List<GetUserAuthorizedTenantsQueryResponse>>> Handle(GetUserAuthorizedTenantsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<List<GetUserAuthorizedTenantsQueryResponse>>.Failure("Kullanıcı kimliği alınamadı.");

        var userTenantIds = await userTenantRoleRepository
            .Where(x => x.UserId == userId.Value)
            .Select(x => x.TenantId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (!userTenantIds.Any())
            return Result<List<GetUserAuthorizedTenantsQueryResponse>>.Succeed([]);

        var tenants = await tenantRepository
            .Where(p => userTenantIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var response = tenants.Select(t => new GetUserAuthorizedTenantsQueryResponse
        {
            Id = t.Id,
            Name = t.Name,
            Code = t.Code
        }).ToList();

        return Result<List<GetUserAuthorizedTenantsQueryResponse>>.Succeed(response);
    }
}
