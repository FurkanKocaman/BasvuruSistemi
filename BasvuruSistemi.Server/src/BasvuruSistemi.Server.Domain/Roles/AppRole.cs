using BasvuruSistemi.Server.Domain.Tenants;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Domain.Roles;
public sealed class AppRole : IdentityRole<Guid>
{
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = default!;

    public string? Description { get; private set; }

    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    #endregion
    private AppRole() { }

    public AppRole(string name, Guid tenantId, string? description) : base(name)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        TenantId = tenantId;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
    }
}
