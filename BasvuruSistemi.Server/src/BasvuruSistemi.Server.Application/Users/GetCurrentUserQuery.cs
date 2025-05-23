using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Roles;
using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetCurrentUserQuery(
    ) : IRequest<Result<GetCurrentUserQueryResponse>>;

public sealed class GetCurrentUserQueryResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => FirstName + " " + LastName;

    public string? AvatarUrl { get; set; }
    public string? Nationality { get; set; }
    public string? TCKN { get; set; }
    public ProfileStatus ProfileStatus { get;  set; }

    public Contact Contact { get; set; } = default!;

    public List<string> Claims { get; set; } = new();
}
internal sealed class GetCurrentUserQueryHandler(
    ICurrentUserService currentUserService,
    IAddressRepository addressRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IUserTenantRoleRepository userTenantRoleRepository,
    RoleManager<AppRole> roleManager
) : IRequestHandler<GetCurrentUserQuery, Result<GetCurrentUserQueryResponse>>
{
    public async Task<Result<GetCurrentUserQueryResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<GetCurrentUserQueryResponse>.Failure(401,"Unauthorized");

        var tenantId = currentUserService.TenantId;

        var roles = await userTenantRoleRepository
            .Where(p => p.UserId == userId && (tenantId != null && p.TenantId == tenantId.Value))
            .Include(p => p.Role)
            .Select(p => p.Role)
            .ToListAsync(cancellationToken);

        var claims = new List<string>();
        foreach (var role in roles)
        {
            var roleClaims = await roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims.Select(p => p.Value));
        }

        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            return Result<GetCurrentUserQueryResponse>.Failure("Kullanıcı bulunamadı.");

        var address = await addressRepository
            .Where(p => p.UserId == user.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var context = httpContextAccessor.HttpContext?.Request;

        var response = new GetCurrentUserQueryResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl != null ? $"{context?.Scheme}://{context?.Host}/{user.AvatarUrl}" : null,
            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,
            Contact = user.Contact,
            Claims = claims
        };

        return Result<GetCurrentUserQueryResponse>.Succeed(response);
    }
}
