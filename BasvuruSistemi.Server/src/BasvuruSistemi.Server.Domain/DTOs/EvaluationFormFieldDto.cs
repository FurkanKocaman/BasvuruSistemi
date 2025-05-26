using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record EvaluationFormFieldDto(
    Guid Id,
    Guid EvaluationFormId,
    FieldTypeEnum FieldType,
    string label,
    string? Options,
    bool IsRequired,
    int Order,
    string? Placeholder = null,
    string? HelpText = null,
    string? ValidationRules = null 
);
