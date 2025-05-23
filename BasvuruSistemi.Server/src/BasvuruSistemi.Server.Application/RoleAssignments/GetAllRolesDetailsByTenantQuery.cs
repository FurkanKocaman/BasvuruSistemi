using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.RoleAssignments;

public sealed record GetAllRolesDetailsByTenantQuery(
    int page,
    int pageSize
) : IRequest<Result<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>>;

public sealed class GetAllRolesDetailsByTenantQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string> Claims { get; set; } = new();
    public int UsersCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

internal sealed class GetAllRolesDetailsByTenantQueryHandler(
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    RoleManager<AppRole> roleManager
) : IRequestHandler<GetAllRolesDetailsByTenantQuery, Result<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>>
{
    public async Task<Result<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>> Handle(GetAllRolesDetailsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>.Failure("Tenant bilgisi alınamadı.");

        var rolesQuery = roleManager.Roles
            .Where(p => p.TenantId == tenantId.Value);

        var totalCount = await rolesQuery.CountAsync(cancellationToken);

        var roles = await rolesQuery
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToListAsync(cancellationToken);

        var response = new List<GetAllRolesDetailsByTenantQueryResponse>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            var usersCount = await userTenantRoleRepository
                .Where(p => p.RoleId == role.Id)
                .CountAsync(cancellationToken);

            response.Add(new GetAllRolesDetailsByTenantQueryResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Claims = claims.Select(c => c.Value).ToList(),
                UsersCount = usersCount,
                CreatedAt = role.CreatedAt
            });
        }

        // İsteğe bağlı sıralama
        response = response.OrderByDescending(p => p.CreatedAt).ToList();

        var pagedResult = new PagedResult<GetAllRolesDetailsByTenantQueryResponse>(
            response,
            request.page,
            request.pageSize,
            totalCount
        );

        return Result<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>.Succeed(pagedResult);
    }
}
