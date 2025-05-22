using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetAllUsersQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetAllUsersQueryResponse>>;

public sealed class GetAllUsersQueryResponse
{
    public Guid Id { get; set; }
    public string? AvatarUrl { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? TCKN { get; set; }
    public string? Email { get; set; } 
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    public DateTimeOffset CreatedAt { get; set; }
}


public sealed class RoleDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

internal sealed class GetAllUsersQueryHandler(
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetAllUsersQuery, PagedResult<GetAllUsersQueryResponse>>
{
    public async Task<PagedResult<GetAllUsersQueryResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return new PagedResult<GetAllUsersQueryResponse>(new List<GetAllUsersQueryResponse>(), 0, 0, 0);

        var canAccess = userTenantRoleRepository.Where(p => p.UserId == userId.Value).Any();

        if (!canAccess)
            return new PagedResult<GetAllUsersQueryResponse>(new List<GetAllUsersQueryResponse>(), 0, 0, 0);

        var users = userManager.Users.Where(p => !p.IsDeleted).Include(p => p.Roles).ThenInclude(p => p.Role);

        var totalCount = await users.AsNoTracking().CountAsync(cancellationToken);

        var pagedUsers = await users
            .Skip((request.page - 1) * request.pageSize)
        .Take(request.pageSize).ToListAsync(cancellationToken);

        var context = httpContextAccessor.HttpContext?.Request;

        var response = pagedUsers
            .Select(user => new GetAllUsersQueryResponse
            {
                Id = user.Id,
                AvatarUrl = user.AvatarUrl != null ? $"{context?.Scheme}://{context?.Host}/{user.AvatarUrl}" : null,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TCKN = user.TCKN,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.Roles != null ? user.Roles.Select(ur => new RoleDto
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name,
                }).ToList() : new List<RoleDto>(),
            })
            .ToList();

        return new PagedResult<GetAllUsersQueryResponse>(response, request.page, request.pageSize, totalCount);
    }
}
