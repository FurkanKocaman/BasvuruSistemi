using BasvuruSistemi.Server.Domain.Abstractions;

namespace BasvuruSistemi.Server.Domain.Evaluations;
public sealed class EvaluationStage : Entity
{
    public string Name { get; private set; } = default!; // Örneğin, "Ön Değerlendirme", "Nihai Değerlendirme Komisyon Onayı"
    public string? Description { get; private set; }
    public int Order { get; private set; } // Değerlendirme sürecindeki sıra numarası


    public ICollection<EvaluationForm> EvaluationForms { get; private set; } = new List<EvaluationForm>();

    public Guid TenantId { get; private set; } = default!; // Tenant ID for multi-tenancy support
    private EvaluationStage() { }
    public EvaluationStage(string name, int order, Guid tenantId, string? description = null)
    {
        Name = name;
        Order = order;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
        TenantId = tenantId;
    }
}
