using BasvuruSistemi.Server.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return null;
            }
            return Guid.Parse(userIdString);
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tenantIdString = httpContextAccessor.HttpContext?.Request.Headers["X-Current-Tenant"];
            if (string.IsNullOrEmpty(tenantIdString))
            {
                return null;
            }
            return Guid.Parse(tenantIdString!);
        }
    }

}
