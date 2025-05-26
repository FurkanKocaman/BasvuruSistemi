using BasvuruSistemi.Server.Domain.Abstractions;

namespace BasvuruSistemi.Server.Domain.Comissions;
public sealed class ApprovalCommission : Entity
{
    public string Name { get; private set; } = default!; // Örneğin, "Mühendislik Fakültesi Değerlendirme Komisyonu"
    public string? Description { get; private set; }

    public ICollection<CommissionMember> CommissionMembers { get; private set; } = new List<CommissionMember>();

    public Guid TenantId { get; private set; } = default!;

    private ApprovalCommission() { }
    public ApprovalCommission(string name, Guid tennatId, string? description = null)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
        TenantId = tennatId;
    }
}
