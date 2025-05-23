using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.UserRoles;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasvuruSistemi.Server.Infrastructure.Service;

internal sealed class JwtProvider(
    IOptions<JwtOptions> options,
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
        };

        Guid? tenantId = currentUserService.TenantId;
        if (tenantId.HasValue)
        {
            var tenantRoles = await userTenantRoleRepository.Where(p => p.UserId == user.Id && p.TenantId == tenantId.Value).Include(p => p.Role).ToListAsync(cancellationToken);

            foreach(var tenantRole in tenantRoles)
            {
                var claim = new Claim(ClaimTypes.Role, tenantRole.Role.Id.ToString());
                claims.Add(claim);
            }

        }

        var expires = DateTime.Now.AddDays(1);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims:claims,
            notBefore: DateTime.Now,
            expires:expires,
            signingCredentials:signingCredentials);
        
        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);

        return token;
    }
}
