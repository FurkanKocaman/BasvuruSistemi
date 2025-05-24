using BasvuruSistemi.Server.Domain.Abstractions;

namespace BasvuruSistemi.Server.Domain.Comissions;
public sealed class ApprovalCommission : Entity
{
    public string Name { get; private set; } = default!; // Örneğin, "Mühendislik Fakültesi Değerlendirme Komisyonu"
    public string? Description { get; private set; }

    public ICollection<CommissionMember> CommissionMembers { get; private set; } = new List<CommissionMember>();

    private ApprovalCommission() { }
    public ApprovalCommission(string name, string? description = null)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
    }
}
