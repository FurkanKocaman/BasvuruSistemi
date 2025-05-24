using BasvuruSistemi.Server.Domain.Evaluations;

namespace BasvuruSistemi.Server.Domain.ApplicationEvaluations;
public sealed class ApplicationEvaluationValue
{
    public Guid Id { get;private set; }

    public Guid ApplicationEvaluationId { get; private set; }
    public ApplicationEvaluation ApplicationEvaluation { get; private set; } = default!;

    public Guid EvaluationFormFieldDefinitionId { get; private set; } // Foreign Key: EvaluationFormFieldDefinition
    public EvaluationFormFieldDefinition EvaluationFormFieldDefinition { get; private set; } = default!;

    public string? Value { get; private set; } 

    private ApplicationEvaluationValue() { }
    public ApplicationEvaluationValue(Guid applicationEvaluationId, Guid evaluationFormFieldDefinitionId, string? value = null)
    {
        Id = Guid.CreateVersion7();
        ApplicationEvaluationId = applicationEvaluationId;
        EvaluationFormFieldDefinitionId = evaluationFormFieldDefinitionId;
        Value = value;
    }
}
