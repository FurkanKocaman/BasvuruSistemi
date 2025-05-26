using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Comissions;
public sealed class CommissionMember : Entity
{
    public Guid CommissionId { get; private set; } = default!;
    public ApprovalCommission Commission { get; private set; } = default!;

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string RoleInCommission { get; private set; } = default!; // Örneğin, "Başkan", "Üye"

    public Guid TenantId { get; private set; } = default!;

    private CommissionMember() { }
    public CommissionMember(Guid commissionId, Guid userId, string roleInCommission, Guid tenantId, Guid createUserId)
    {
        Id = Guid.CreateVersion7();
        CommissionId = commissionId;
        UserId = userId;
        RoleInCommission = roleInCommission;
        CreatedAt = DateTimeOffset.Now;
        TenantId = tenantId;
        CreateUserId = createUserId;
    }
    public void UpdateRole(string newRole)
    {
        RoleInCommission = newRole;
        UpdateAt = DateTimeOffset.Now;
    }
}
