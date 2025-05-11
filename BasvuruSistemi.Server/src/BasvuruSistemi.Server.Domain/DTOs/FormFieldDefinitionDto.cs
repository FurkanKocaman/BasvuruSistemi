namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FormFieldDefinitionDto(
    string label,
    string? description,
    int type,
    bool isRequired,
    string? placeholder,
    string? optionsJson,

    bool isReadOnly,
    string? defaultValue,

    string? allowedFileTypes,
    int? maxFileSizeMB
);
