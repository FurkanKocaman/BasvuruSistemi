namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record EvaluationFormDto(
    Guid Id,
    string Name,
    Guid EvaluationStageId,
    DateTimeOffset CreatedAt,
    List<EvaluationFormFieldDto>? Fields,
    string? Description = null
    );
