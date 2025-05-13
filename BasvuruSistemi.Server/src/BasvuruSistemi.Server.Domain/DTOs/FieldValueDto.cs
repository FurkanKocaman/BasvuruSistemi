namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FieldValueDto(
    Guid FieldDefinitionId,
    string Value
);
