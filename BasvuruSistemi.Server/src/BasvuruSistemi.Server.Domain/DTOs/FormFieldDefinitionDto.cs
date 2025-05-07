namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FormFieldDefinitionDto(
    string label,
    int type,
    bool isRequired,
    string? description,
    string? placeholder,
    string? optionsJson,
    bool isReadOnly,
    string? defaultValue,
    int verificationSource,
    string? verificationParametersJson,
    string? allowedFileTypes,
    int? maxFileSizeMB
);
