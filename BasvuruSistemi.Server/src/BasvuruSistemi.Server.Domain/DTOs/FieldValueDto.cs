namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FieldValueDto(
    Guid? Id,
    Guid FieldDefinitionId,
    string? Value
);
