namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record PipelineStageDto(
    Guid EvaluationStageId,
    int OrderInPipeline,
    bool IsMandatory,
    Guid EvaluationFormId,
    Guid ResponsibleCommissionId,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null
    );