using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record EvaluationFormFieldDto(
    Guid Id,
    Guid EvaluationFormId,
    string Label,
    string? Description,
    FieldTypeEnum Type,
    bool IsRequired,
    int Order,
    string? Placeholder,
    string? OptionsJson,
    bool IsReadonly,
    string? DefaultValue,
    string? AllowedFileTypes,
    int? MaxFileSizeMB
);
