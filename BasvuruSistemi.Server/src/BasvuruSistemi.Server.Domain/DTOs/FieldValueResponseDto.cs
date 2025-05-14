using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FieldValueResponseDto(
     Guid Id,
     string Title,
     FieldTypeEnum Type,
     string? Value
    );
