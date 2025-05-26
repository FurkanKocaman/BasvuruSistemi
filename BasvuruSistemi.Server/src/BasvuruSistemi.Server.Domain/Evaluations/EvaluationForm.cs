using BasvuruSistemi.Server.Domain.Abstractions;

namespace BasvuruSistemi.Server.Domain.Evaluations;
public sealed class EvaluationForm : Entity
{
    public string Name { get; private set; } = default!; // Örneğin, "Mühendislik Fakültesi Değerlendirme Formu"
    public string? Description { get; private set; }

    public Guid EvaluationStageId { get; private set; }
    public EvaluationStage EvaluationStage { get; private set; } = default!;

    public ICollection<EvaluationFormFieldDefinition> Fields { get; private set; } = new List<EvaluationFormFieldDefinition>();

    public Guid TenantId { get; private set; } = default!; // Tenant ID for multi-tenancy support

    private EvaluationForm() { }
    public EvaluationForm(string name, Guid evaluationStageId, Guid tenantId, string? description = null)
    {
        Name = name;
        EvaluationStageId = evaluationStageId;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
        TenantId = tenantId;
    }
    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description;
        UpdateAt = DateTimeOffset.Now;
    }
} 
