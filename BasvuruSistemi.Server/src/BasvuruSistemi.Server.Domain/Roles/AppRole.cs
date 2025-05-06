using BasvuruSistemi.Server.Domain.Tenants;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Domain.Roles;
public sealed class AppRole : IdentityRole<Guid>
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public string? Description { get; set; }

    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    #endregion
    private AppRole() { }

    public AppRole(string name, Guid tenantId, string? description) : base(name)
    {
        Id = Guid.CreateVersion7();
    }
}
