using BasvuruSistemi.Server.Domain.Abstractions;

namespace BasvuruSistemi.Server.Domain.Evaluations;
public sealed class EvaluationForm : Entity
{
    public string Name { get; private set; } = default!; // Örneğin, "Mühendislik Fakültesi Değerlendirme Formu"
    public string? Description { get; private set; }

    public Guid EvaluationStageId { get; private set; }
    public EvaluationStage EvaluationStage { get; private set; } = default!;

    public ICollection<EvaluationFormFieldDefinition> Fields { get; private set; } = new List<EvaluationFormFieldDefinition>();

    private EvaluationForm() { }
    public EvaluationForm(string name, Guid evaluationStageId, string? description = null)
    {
        Name = name;
        EvaluationStageId = evaluationStageId;
        Description = description;
        CreatedAt = DateTimeOffset.Now;
    }
}
