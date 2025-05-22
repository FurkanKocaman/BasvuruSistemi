using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.RoleAssignments;
public sealed record GetAllRolesDetailsByTenantQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetAllRolesDetailsByTenantQueryResponse>>;

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
    ) : IRequestHandler<GetAllRolesDetailsByTenantQuery, PagedResult<GetAllRolesDetailsByTenantQueryResponse>>
{
    public async Task<PagedResult<GetAllRolesDetailsByTenantQueryResponse>> Handle(GetAllRolesDetailsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return new PagedResult<GetAllRolesDetailsByTenantQueryResponse>(new List<GetAllRolesDetailsByTenantQueryResponse>(),0,0,0);

        var roles = await roleManager.Roles.Where(p => p.TenantId == tenantId.Value).ToListAsync(cancellationToken);

        var totalCount = roles.Count;

        roles = roles
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var response = new List<GetAllRolesDetailsByTenantQueryResponse>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            var usersCount = await userTenantRoleRepository.Where(p => p.RoleId == role.Id).CountAsync(cancellationToken);

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

        response.OrderBy(p => p.Claims.Count());

        return new PagedResult<GetAllRolesDetailsByTenantQueryResponse>(
            response,
            request.page,
            request.pageSize,
            totalCount
        );

    }
}
