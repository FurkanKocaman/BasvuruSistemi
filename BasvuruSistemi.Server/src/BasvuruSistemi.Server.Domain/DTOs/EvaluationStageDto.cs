namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record EvaluationStageDto(
    Guid Id,
    string Name,
    string? Description,
    int Order,
    Guid? DefaultEvaluationFormId,
    List<EvaluationFormDto>? EvaluationForms,
    DateTimeOffset CreatedAt
    );
