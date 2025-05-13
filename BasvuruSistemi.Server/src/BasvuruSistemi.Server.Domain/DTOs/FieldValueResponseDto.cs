using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FieldValueResponseDto(
    Guid FieldValueId,
     FieldTypeEnum Type,
     string? Value
    );
